using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.ObraLineal
{

    using System.IO;

    using engCadNet;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

   
    using tadLayLogica;
    using tadLayLan;
    using tadLayLogica.logica.medicion;
    using tadLayLogica.Secciones.Geometria;
    using tadLayLogica.Secciones.Planta;
    using tadLayLogica.zonaGis;
    using tadLayLogica.logica.Secciones;
    using tadLayLogica.Secciones.Geometria.Saneo;
    using tadLayLogica.estudioTipo;


    using tadLayShare.puntos;
    using tadLayLogica.Secciones.Calzada;
    using tadLayLogica.logica.EjeBasicoNew;

    using EjeDeTrazado.puntosDelEje;

    using engNet.Extension.Double;
    using PerfilLongitudinal;
    using tadLayShare;
    using tadLayLogica.datos.BaseDatos;
    using tadLayData;

    public class oObraLineal:IDisposable
    {

        private Guid mIdSolucion = Guid.Empty;
        private string mSolucionName = string.Empty;
        //private Polyline mEjePlanta;
        private EjeTrazado mEjeTrazadoTadil;
        //private Polyline mEjePerfil;
        private Alzado mAlzado;
        private oEstudioInformativoCarretera mEstudioInformativoCarretera;
        private oSeccionRoadCompletaSinGis mSeccionCalzadaCompletaSinGis;


        //CAPAS
        private oTadilLayerSecciones mLayerSecciones;
        private oTadilLayerPlanta mLayerPlanta;
        private oTadilLayerDominioPublicoAdyacente mLayerDominioPublicoAdyacente;
        private oTadilLayerExpropiacion mLayerExpropiacion;

       

        private oMedicionBySolucion mMedicionBySolucion;
        private List<oSeccionEjeTrazado> mLstSecciones;
        private List<oSeccionPlantaPk> mLstSeccionesPlanta;
        private List<engNet.Str.oStringPropiedad> mLstSeccionesConError;

        private double mLongitudDiscretizacionTerrenoNatural = 1;
        private int mSeparacionEntreGuitarras = 10;

        //OJO VARIABLE GIS
        private double? mDesbroce = null;
        #region "Constructor"

        public oObraLineal(Guid iIdSolucion,
                           string iNameSolucion,
                           Polyline iEjePlanta,
                           Polyline iEjePerfil,
                           oEstudioInformativoCarretera iEstudioInformativoCarretera,
                           oSeccionRoadCompletaSinGis iSeccionCalzadaCompletaSinGis)
            
                        
                              
        {
            mIdSolucion = iIdSolucion;
            mSolucionName = iNameSolucion;
            //mEjePlanta = iEjePlanta;
           // mEjePerfil = iEjePerfil;

            Xrecord miXrecord = engCadNet.oXrecord.getXrecord(iEjePlanta.ObjectId, "info");
            EjeDeTrazado.puntosDelEje.EjeTrazado miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));
            mEjeTrazadoTadil = miEje;


            Xrecord miXrecordAlzado = engCadNet.oXrecord.getXrecord(iEjePerfil.ObjectId, "info");
            Alzado miEjeAlzado = Alzado.recuperaAlzado(engCadNet.oXrecord.getStream(miXrecordAlzado));
            mAlzado = miEjeAlzado;

            mEstudioInformativoCarretera = iEstudioInformativoCarretera;
            mSeccionCalzadaCompletaSinGis = iSeccionCalzadaCompletaSinGis;
        }

        public oObraLineal(Guid iIdSolucion,
                           string iNameSolucion,
                           EjeTrazado iEjeTrazadoTadil,
                           Alzado iAlzado,
                           oEstudioInformativoCarretera iEstudioInformativoCarretera,
                           oSeccionRoadCompletaSinGis iSeccionCalzadaCompletaSinGis)
        {
            mIdSolucion = iIdSolucion;
            mSolucionName = iNameSolucion;
            mEjeTrazadoTadil = iEjeTrazadoTadil;
            mAlzado = iAlzado;
            mEstudioInformativoCarretera = iEstudioInformativoCarretera;
            mSeccionCalzadaCompletaSinGis = iSeccionCalzadaCompletaSinGis;
        }


        #endregion
        #region "Propiedades"

        public bool hasSeccionesConError
        {
            get
            {
                if (mLstSeccionesConError == null || mLstSeccionesConError.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public List<engNet.Str.oStringPropiedad> getListadoPkConError()
        {
            return mLstSeccionesConError;
        }

      
        #endregion
        #region "Metodos Publicos"


        public void crearSecciones(double iSeccionIntervalo)
        {

            mLstSecciones = new List<oSeccionEjeTrazado>();

            oSeccionEjeTrazado miSeccionPk;
            


            miSeccionPk = this.getSeccion(0, iSeccionIntervalo/2);
            mLstSecciones.Add(miSeccionPk);

            for (double miPk = iSeccionIntervalo; miPk < mEjeTrazadoTadil.Length; )
            {
                Cambiar_EstudioCarretera(miPk);
                miSeccionPk = this.getSeccion(miPk, iSeccionIntervalo);

                mLstSecciones.Add(miSeccionPk);

                miPk = miPk + iSeccionIntervalo;
            }

            //Recalculo la penúltima sección
            double miSeccionUltimaLongitud = mEjeTrazadoTadil.Length - mLstSecciones.Last().pk.Value;
            Cambiar_EstudioCarretera(mLstSecciones.Last().pk.Value);
            mLstSecciones[mLstSecciones.Count-1] = this.getSeccion(mLstSecciones.Last().pk.Value, iSeccionIntervalo/2 + miSeccionUltimaLongitud/2);

            //Calculo la Última Seccion

            miSeccionPk = this.getSeccion(mEjeTrazadoTadil.Length, miSeccionUltimaLongitud/2);

            mLstSecciones.Add(miSeccionPk);

            foreach (var item in mLstSecciones)
            {
                item.draw(oTadil.data.Layer.abanicoSecciones.name);
            }

        }
        private oSeccionEjeTrazado getSeccion(double iPk, double iSeccionIntervalo)
        {

            oP3d miRoadPtoPk;

            double miZterrenoPk;

            oSeccionEjeTrazado miSeccionEjePk;

            //Punto del Eje de Trazado
            miRoadPtoPk = this.getPtoFromPk(iPk);
            //Z del Terreno
            miZterrenoPk = this.getZterreno(iPk);

            miSeccionEjePk = new oSeccionEjeTrazado(iPk, miRoadPtoPk, miZterrenoPk, iSeccionIntervalo);

            miSeccionEjePk.calculateSeccionTipoAndZonasDesign(mEstudioInformativoCarretera);

            return miSeccionEjePk;

        }



        public void dibujarSecciones(Point3d iPtoInserccion)
        {

            #region "GestionCapas"

            mLayerSecciones = new oTadilLayerSecciones(mSolucionName);
            mLayerPlanta = new oTadilLayerPlanta(mSolucionName);
            mLayerDominioPublicoAdyacente = new oTadilLayerDominioPublicoAdyacente(mSolucionName);
            mLayerExpropiacion = new oTadilLayerExpropiacion(mSolucionName);

            mLayerSecciones.deleteItems();
            mLayerPlanta.deleteItems();
            mLayerDominioPublicoAdyacente.deleteItems();
            mLayerExpropiacion.deleteItems();

            #endregion

            #region "Creamos las Colecciones"

            mMedicionBySolucion = new oMedicionBySolucion(mIdSolucion);
            mLstSeccionesPlanta = new List<oSeccionPlantaPk>();
            mLstSeccionesConError = new List<engNet.Str.oStringPropiedad>();

            #endregion

            #region "SetUp Objetos"

            //Cargo las funciones ESTATICAS
            oSeccionDecoradorParent.mFunGetZTnd = getZscRoadByPkDesfaseLado;
            oSeccionDecoradorParent.pkIntervalo = mLstSecciones.First().longitudSeccion.Value;

            //VARIABLES ESTATICOS
            oSeccionDrawable.funGetElevationAt = getElevationAt;
            oSeccionDrawable.funGetLstPtoTns = getSeccionTnsXY;
            oSeccionDrawable.funGetLstPtoTnd = getSeccionTndXY;
            oSeccionDrawable.funGetPtoTnd = getPtoTndFromPkDesfase;
            oSeccionDrawable.funGetPtoEjeTrazadoFromDesfase = getPto2dEjeTrazadoFromPkDesfaseLado_ForPlanta;


            #endregion

            #region "Creamos las Secciones"

            //Seccion por PK
            oSeccionDrawable miSeccionObraLineal = null;

            ////Obtengo las mediciones de la Seccion
            oSeccionMedicion miSeccionMedicion = null;

            ////Obtengo la Planta de la Seccion
            oSeccionPlantaPk miSeccionPlanta = null;


            Point2d miPtoInsertSeccion = new Point2d(iPtoInserccion.X, iPtoInserccion.Y);


            // using (StreamWriter writer = new StreamWriter(@"c:\debug.txt", true))
            //  {


            foreach (var item in mLstSecciones)
            {

                //  writer.WriteLine(item.pk.Value.ToString());

                //  if (item.pk.Value == 56450)
                //  {
                bool error = false;
                try
                {
                        Cambiar_EstudioCarretera(item.pk.Value);

                        if (item.seccionTipo.Value == eRoadSeccion.calzada)
                        {
                            miSeccionObraLineal = this.getSeccionCalzada(item, miPtoInsertSeccion);
                        }
                        else if (item.seccionTipo.Value == eRoadSeccion.puente)
                        {
                            miSeccionObraLineal = this.getSeccionPuente(item);
                        }
                        else if (item.seccionTipo.Value == eRoadSeccion.tunel)
                        {
                            miSeccionObraLineal = this.getSeccionTunel(item);
                        }
                        else
                        {
                            throw new oExEnumNotImplemented(item.seccionTipo.Value.ToString());
                        }
                    


                    //DIBUJO LA SECCION
                    miSeccionObraLineal.draw(miPtoInsertSeccion, mLayerSecciones.name);

                    ////OBTENGO LAS MEDICIONES
                    miSeccionMedicion = miSeccionObraLineal.getMediciones(item.longitudSeccion.Value);
                    mMedicionBySolucion.addSecciones(miSeccionMedicion);

                    //DIBUJO LAS MEDICIONES
                    miSeccionObraLineal.guitarra.updateSistemaReferencia();

                    Point3d miPtoInsertMediciones = miSeccionObraLineal.getPtoInsertMediciones();

                    miSeccionMedicion.drawMedicion(miPtoInsertMediciones, mLayerSecciones.name, mIdSolucion);

                    ////OBTENGO LOS DATOS PLANTA
                    miSeccionPlanta = miSeccionObraLineal.getPlanta();
                    mLstSeccionesPlanta.Add(miSeccionPlanta);


                    if (item.pk.Value.isDivisible(100))
                    {
                        oCadManager.thisEditor.UpdateScreen();
                    }

                    oCadManager.thisEditor.WriteMessage("\n" + string.Format(strUserCad.uiGenerandolaSeccion, item.pk.ToString()));

                }
                catch (tadLayShare.oExSeccionGeometriaDesignNoValid)
                {
                    error = true;
                    throw;
                }   
                catch (Exception ex)
                {
                    error = true;
                    mLstSeccionesConError.Add(new engNet.Str.oStringPropiedad("Pk: " + item.pk.Value.ToString() + "-" + ex.Message));
                }

                //CREO EL NUEVO PUNTO DE INSERCCION
                if (miSeccionObraLineal != null && miSeccionObraLineal.guitarra != null && !error)
                {
                    miPtoInsertSeccion = new Point2d(miPtoInsertSeccion.X + miSeccionObraLineal.guitarra.dimensiones.ancho + mSeparacionEntreGuitarras, miPtoInsertSeccion.Y);
                }
                else
                {
                    // La sección falló antes de poder dibujarse; avanzar un espacio mínimo para no quedarse en la misma posición
                    miPtoInsertSeccion = new Point2d(miPtoInsertSeccion.X + mSeparacionEntreGuitarras, miPtoInsertSeccion.Y);
                }



            //  }

          //  }
        }

            #endregion

        }
        public void createObraLinealPlanta()
        {

            //Gestion de Capas
            oTadilLayerPlanta miLayerPlanta = new oTadilLayerPlanta(mSolucionName);
            oTadilLayerDominioPublicoAdyacente miLayerDominioPublicoAdyacente = new oTadilLayerDominioPublicoAdyacente(mSolucionName);


            //Objeto Obra Lineal
            using (oObraLinealPlantaDrawable miObraLineal = new oObraLinealPlantaDrawable(mIdSolucion, mSolucionName, mLstSeccionesPlanta))
            {
                miObraLineal.draw(miLayerPlanta.name, miLayerDominioPublicoAdyacente.name);

                mLstSeccionesPlanta.Clear();
            }
           
        }

        public void createObraLinealPlantaSinGuardar()
        {

            //Gestion de Capas
            oTadilLayerPlanta miLayerPlanta = new oTadilLayerPlanta(mSolucionName);
            oTadilLayerDominioPublicoAdyacente miLayerDominioPublicoAdyacente = new oTadilLayerDominioPublicoAdyacente(mSolucionName);


            //Objeto Obra Lineal
            using (oObraLinealPlantaDrawable miObraLineal = new oObraLinealPlantaDrawable(mIdSolucion, mSolucionName, mLstSeccionesPlanta))
            {
                miObraLineal.draw(miLayerPlanta.name, miLayerDominioPublicoAdyacente.name, false);

                mLstSeccionesPlanta.Clear();
            }
           
        }

        public void createMediciones()
        {

            try
            {
                //1-MEDICIONES CAD (MovimientoTierras+PartidasSimples)
                mMedicionBySolucion.createMedicionesCad();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                mMedicionBySolucion.Dispose();
            }

            //2-Mediciones de las Expropiaciones (DESPUES DEL DIBUJO EN PLANTA)
            using (oMedicionExpropiacion miMedExpropiacion = new oMedicionExpropiacion(mIdSolucion))
            {
                miMedExpropiacion.createMediciones();
            }
                    
        }


        /// <summary>
        /// Guardar las Secciones
        /// </summary>
        public void saveSecciones()
        {
            tadLayLogica.datos.proyecto.oDalTbObraLinealSecciones.save(mIdSolucion,mLstSecciones);
        }

        #endregion
        #region "MetodosPrivados"

        private oSeccionDrawable getSeccionCalzada(oSeccionEjeTrazado iSeccion, Point2d iPtoInserccion)
        {

            oSeccionRoadCompletaConGIS miSecRoadFullByPk;
            oSeccionCalzada miSeccion;
            oZonaGeoMovimientoTierras miZonaMovimientoTierras;

            //Obtengo la Zona GIS
            miZonaMovimientoTierras = iSeccion.zonaMovimientoTierras;

            //Obtengo el Desbroce
            mDesbroce = miZonaMovimientoTierras.row.desbroceEspesor;

            //Obtengo los Peraltes
            oPeralte miPeralte = this.getPeralteByPk(iSeccion.pk.Value);

            //Creo la Carretera Completa en su PK
            miSecRoadFullByPk = new oSeccionRoadCompletaConGIS(mSeccionCalzadaCompletaSinGis,
                                                               iSeccion.pk.Value,                                                               
                                                               miPeralte.peralteIzqPC,
                                                               miPeralte.peralteDerPC,
                                                               miZonaMovimientoTierras);
            //Creo la Guitarra Seccion
            oSeccionGuitarra miGuitarra = new oSeccionGuitarra(iPtoInserccion,
                                                               this.getElevationAt(iSeccion.pk.Value),
                                                               miZonaMovimientoTierras.row.desmonteAlturaMaxima,
                                                               miZonaMovimientoTierras.row.desmonteTalud,
                                                               miZonaMovimientoTierras.row.terraplenAlturaMaxima,
                                                               miZonaMovimientoTierras.row.terraplenTalud,
                                                               miSecRoadFullByPk.anchoCalzadaCompleta);
            
           


            miSeccion = new oSeccionCalzada(miSecRoadFullByPk, miGuitarra);

            return miSeccion;

        }
        private oSeccionDrawable getSeccionTunel(oSeccionEjeTrazado iSeccion)
        {

            oSeccionDrawable miSeccionTunel;


            if (mSeccionCalzadaCompletaSinGis.createSeccionesEstructurasDobles)
            {
                miSeccionTunel = new oSeccionTunelDoble(iSeccion,mSeccionCalzadaCompletaSinGis.getSeparacionEntreEstructuras());
                return miSeccionTunel;
            }
            else
            {
               miSeccionTunel = new oSeccionTunelSimple(iSeccion);
               return miSeccionTunel;
            }
        
        }
        private oSeccionDrawable getSeccionPuente(oSeccionEjeTrazado iSeccion)
        {

            oSeccionDrawable miSeccionPuente;

            if (mSeccionCalzadaCompletaSinGis.createSeccionesEstructurasDobles)
            {
                miSeccionPuente = new oSeccionPuenteDoble(iSeccion, mSeccionCalzadaCompletaSinGis.getSeparacionEntreEstructuras());
                return miSeccionPuente;
            }
            else
            {

                miSeccionPuente = new oSeccionPuenteSimple(iSeccion);
                return miSeccionPuente;
            }

        }

        /// <summary>
        /// ELEVACION RASANTE POR PK
        /// </summary>
        private double getElevationAt(double iPk)
        {
            return mAlzado.getCotaRasante(iPk);        
        }
        /// <summary>
        /// PTO CALZADA
        /// </summary>
        private oP3d getPtoFromPk (double iPk)
        {

            double[] miPunto = mEjeTrazadoTadil.getPointAtDist(iPk);

            Point3d miPto = new Point3d(miPunto[0], miPunto[1], 0);

            double miPtoZ = this.getElevationAt(iPk);

            return new oP3d(miPto.X, miPto.Y, miPtoZ);
        }
        /// <summary>
        /// Z Terreno
        /// </summary>
        private double getZterreno (double iPk)
        {
            double[] miPunto = mEjeTrazadoTadil.getPointAtDist(iPk);

            Point3d miPto = new Point3d(miPunto[0], miPunto[1], 0);

            return oSingletonTerreno.getInstance.getZFromXY(miPto.X, miPto.Y).Value;
        }

        private Point3d getPto2dEjeTrazadoFromPkDesfaseLado (double iPk, double iDesfase, eLado iLado)
        {

            double miX = 0;
            double miY = 0;


            double miDesfase = iDesfase;

            //if (iLado == eLado.DER)
            //{
            //    miDesfase = 1 * iDesfase;
            //}
            //else
            //{
            //    miDesfase = -1 * iDesfase;
            //}

           // mEjePlanta.PointLocation(iPk, miDesfase, ref miX, ref miY);

            double[] miPunto = new double[2];
            if (iLado == eLado.DER)
            {
                miPunto = mEjeTrazadoTadil.getPointLocation(iPk, miDesfase, EjeTrazado.ladoCalzada.Derecha);
            }
            else
            {
                miPunto = mEjeTrazadoTadil.getPointLocation(iPk, miDesfase, EjeTrazado.ladoCalzada.Izquierda);

            }

            miX = miPunto[0];
            miY = miPunto[1];

            return new Point3d(miX, miY, 0);
        }

        /// <summary>
        /// Obtener un Punto 2D en el Eje de Trazado desde un PK, desfase y lado.
        /// Versión corregida para uso en Planta (DominioPublicoAdyacente y _Primaria_planta):
        /// En clotoide, calcula el azimut local del eje mediante puntos anterior y posterior
        /// para evitar el intercambio de lados que produce getPointLocation en clotoide.
        /// El método original getPto2dEjeTrazadoFromPkDesfaseLado no se modifica.
        /// </summary>
        private Point3d getPto2dEjeTrazadoFromPkDesfaseLado_ForPlanta(double iPk, double iDesfase, eLado iLado)
        {
            // Detectar si el componente en ese PK es clotoide
            var miComponente = mEjeTrazadoTadil.getComponente(iPk);
            bool esClotoide = (miComponente.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideEntrada
                            || miComponente.getTipoComponente() == EjeDeTrazado.componentes.Componente.tipoComponente.clotoideSalida);

            if (!esClotoide)
            {
                // En recta y curva, el método original funciona correctamente
                return getPto2dEjeTrazadoFromPkDesfaseLado(iPk, iDesfase, iLado);
            }

            // --- CORRECCIÓN PARA CLOTOIDE ---
            // Calcular azimut local del eje mediante puntos anterior y posterior
            // (igual que la corrección aplicada en oEjeTrazadoTadilRotular)
            double pkIniComponente = miComponente.getPkIni;
            double pkFinComponente = miComponente.getPkFinal();

            double iAnt  = Math.Max(pkIniComponente + 0.001, iPk - 0.01);
            double iPost = Math.Min(pkFinComponente - 0.001, iPk + 0.01);

            double[] ptoAnt  = mEjeTrazadoTadil.getPointAtDist(iAnt);
            double[] ptoPost = mEjeTrazadoTadil.getPointAtDist(iPost);

            // Azimut local en grados (sistema matemático, antihorario desde Este)
            double miAzEje = Math.Atan2(ptoPost[1] - ptoAnt[1], ptoPost[0] - ptoAnt[0]) * 180.0 / Math.PI;

            // Transversal: derecha = -90°, izquierda = +90° (respecto al azimut del eje)
            double miAzTrans;
            if (iLado == eLado.DER)
            {
                miAzTrans = miAzEje - 90.0;
            }
            else
            {
                miAzTrans = miAzEje + 90.0;
            }

            double[] ptoCentro = mEjeTrazadoTadil.getPointAtDist(iPk);
            double miX = ptoCentro[0] + iDesfase * Math.Cos(miAzTrans * Math.PI / 180.0);
            double miY = ptoCentro[1] + iDesfase * Math.Sin(miAzTrans * Math.PI / 180.0);

            return new Point3d(miX, miY, 0);
        }

        /// <summary>
        /// Punto Terreno Natural Superior
        /// </summary>
        private Point3d getPtoTnsFromPkDesfase(double iPk, double iDesfase, eLado iLado)
        {

            
                double miX = 0;
                double miY = 0;
                double? miZ = null;

                double miDesfase = Math.Abs(iDesfase);

                //if (iLado == eLado.DER)
                //{
                //    miDesfase = 1 * iDesfase;
                //}
                //else
                //{
                //    miDesfase = -1 * iDesfase;
                //}

                // mEjePlanta.PointLocation(iPk, miDesfase, ref miX, ref miY);

                double[] miPunto = new double[2];
                if (iLado == eLado.DER)
                {
                    miPunto = mEjeTrazadoTadil.getPointLocation(iPk, miDesfase, EjeTrazado.ladoCalzada.Derecha);
                }
                else
                {
                    miPunto = mEjeTrazadoTadil.getPointLocation(iPk, miDesfase, EjeTrazado.ladoCalzada.Izquierda);

                }

                miX = miPunto[0];
                miY = miPunto[1];

                //OJO ERROR AL OBTENER EL Z de un Punto Fuera del Contorno
                miZ = oSingletonTerreno.getInstance.getZFromXY(miX, miY);


                if (miZ.HasValue)
                {
                    return new Point3d(miX, miY, miZ.Value);
                }
                else
                {
                  throw new oExSeccionOutCartografia(iPk);
   
                } 
        }
        /// <summary>
        ///Punto Terreno Natural Desbrozado
        /// </summary>
        private Point3d getPtoTndFromPkDesfase(double iPk, double iDesfase, eLado iLado)
        {

            Point3d miPtoTnd = getPtoTnsFromPkDesfase(iPk, iDesfase, iLado);

            return new Point3d(miPtoTnd.X, miPtoTnd.Y, miPtoTnd.Z - mDesbroce.Value);

        }

        private Point3dCollection getSeccionTnsXY(double iPk, double iAnchoDer, double iAnchoIzq)
        {

                Point3dCollection myLstTn = new Point3dCollection();
                Point3d miPto;

                double lastValidZ = double.NaN;

                //Ancho Derecho
                for (double i = iAnchoDer; i > 0; )
                {
                    miPto = getPtoTnsFromPkDesfase(iPk, i, eLado.DER);
                    double z = miPto.Z;

                    if (double.IsNaN(z))
                    {
                        if (!double.IsNaN(lastValidZ)) z = lastValidZ;
                    }
                    else
                    {
                        lastValidZ = z;
                    }

                    myLstTn.Add(new Point3d(i, z, 0));

                    i = i - mLongitudDiscretizacionTerrenoNatural;
                }

                //Ancho Izquierdo
                for (double j = 0; j <= iAnchoIzq; )
                {
                    miPto = getPtoTnsFromPkDesfase(iPk, j, eLado.IZQ);
                    double z = miPto.Z;

                    if (double.IsNaN(z))
                    {
                        if (!double.IsNaN(lastValidZ)) z = lastValidZ;
                    }
                    else
                    {
                        lastValidZ = z;
                    }

                    myLstTn.Add(new Point3d(-j, z, 0));

                    j = j + mLongitudDiscretizacionTerrenoNatural;
                }

                return myLstTn;

          }

        
        private Point3dCollection getSeccionTndXY(double iPk, double iAnchoDer, double iAnchoIzq)
        {    
                Point3dCollection myLstTn = new Point3dCollection();
                Point3d miPto;

                double lastValidZ = double.NaN;

                //Ancho Derecho
                for (double i = iAnchoDer; i > 0; )
                {
                    miPto = getPtoTnsFromPkDesfase(iPk, i, eLado.DER);
                    double z = miPto.Z;

                    if (double.IsNaN(z))
                    {
                        if (!double.IsNaN(lastValidZ)) z = lastValidZ;
                    }
                    else
                    {
                        lastValidZ = z;
                    }

                    myLstTn.Add(new Point3d(i, z - mDesbroce.Value, 0));

                    i = i - mLongitudDiscretizacionTerrenoNatural;
                }

                //Ancho Izquierdo
                for (double j = 0; j <= iAnchoIzq; )
                {
                    miPto = getPtoTnsFromPkDesfase(iPk, j, eLado.IZQ);
                    double z = miPto.Z;

                    if (double.IsNaN(z))
                    {
                        if (!double.IsNaN(lastValidZ)) z = lastValidZ;
                    }
                    else
                    {
                        lastValidZ = z;
                    }

                    myLstTn.Add(new Point3d(-j, z - mDesbroce.Value, 0));

                    j = j + mLongitudDiscretizacionTerrenoNatural;
                }

                return myLstTn;

        }
        /// <summary>
        /// Obtener la Coordenada Z en el Sc Road de un punto de la sección
        /// </summary>
        private double getZscRoadByPkDesfaseLado(double iPk, double iDesfaseEjeX, double iDesfaseEjeY, eLado iLado)
        {

            //Coordenadas del TND del Punto Talud Origen

            Point3d miTndWorld = getPtoTndFromPkDesfase(iPk, iDesfaseEjeX, iLado);

            double miZPkEje = getElevationAt(iPk);


            double miZtnd = miTndWorld.Z - miZPkEje;


            return miZtnd;

        }

        /// <summary>
        /// Obtener el Peralte en función del PK
        /// </summary>
        private oPeralte getPeralteByPk (double iPk)
        {

            EjeDeTrazado.componentes.Componente.tipoComponente miEjeEntidad;


            //CASO NO COINCIDE LON EJE TRAZADO CIVIL us ANGELES !!!!
            if (iPk > mEjeTrazadoTadil.Length)
            {
                iPk = mEjeTrazadoTadil.Length;
            }


            miEjeEntidad = mEjeTrazadoTadil.getComponente(iPk).getTipoComponente();



            if (miEjeEntidad == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
            {

                //Problema de unidades?!?!
                return new oPeralte(-mEjeTrazadoTadil.getBombeo, -mEjeTrazadoTadil.getBombeo);
            }
            else
            {

                double miPeralteIzq = 0;
                double miPeralteDer = 0;
                double miCoefienteConversionPeralte = -1;

                mEjeTrazadoTadil.getPeralteAlDist(iPk, ref miPeralteIzq, ref miPeralteDer);

                //Problema de unidades?!?!
                return new oPeralte(miPeralteIzq, miCoefienteConversionPeralte * miPeralteDer);
                
            }

        }

        #endregion


        public void Dispose()
        {
            mSeccionCalzadaCompletaSinGis.Dispose();
            mLstSecciones.Clear();  
        }
        internal class oPeralte
        {

          public  double peralteIzqPC { get; private set; }
          public  double peralteDerPC { get; private set; }

            public oPeralte(double iPeralteIzqPC, double iPeralteDerPC)
            {
                this.peralteIzqPC = iPeralteIzqPC;
                this.peralteDerPC = iPeralteDerPC;
            }

        }

        #region Cambio de seccion de tipo de carretera

        /// <summary>
        /// Obtiene el punto en coordenadas (oP3d) para un PK dado y comprueba si dicho punto
        /// se encuentra dentro de alguna polilínea cerrada cuya capa comience por los prefijos indicados.
        /// </summary>
        /// <param name="iPk">PK de búsqueda.</param>
        /// <param name="capaPolilinea">Capa de la polilínea en la que se encuentra dentro (salida).</param>
        /// <param name="estaDentro">Indica si está dentro de alguna polilínea válida (salida).</param>
        /// <returns>El punto obtenido en coordenadas (oP3d).</returns>
        public oP3d ObtenerPuntoYComprobarPolilineas(double iPk, out string capaPolilinea, out bool estaDentro)
        {
            capaPolilinea = string.Empty;
            estaDentro = false;

            // 1. Obtener las coordenadas del punto para el PK dado
            oP3d ptoRoad = this.getPtoFromPk(iPk);
            Point2d p2d = new Point2d(ptoRoad.X, ptoRoad.Y);

            try
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = doc.Database;

                using (DocumentLock docLock = doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

                        foreach (ObjectId id in btr)
                        {
                            // Comprobar si la clase es una Polyline estándar
                            if (id.ObjectClass.IsDerivedFrom(Autodesk.AutoCAD.Runtime.RXClass.GetClass(typeof(Polyline))))
                            {
                                Polyline poly = tr.GetObject(id, OpenMode.ForRead) as Polyline;
                                if (poly != null && poly.Closed)
                                {
                                    string layerName = poly.Layer;
                                    if (layerName.StartsWith("Tadil_Doble_", StringComparison.OrdinalIgnoreCase) ||
                                        layerName.StartsWith("Tadil_DobleSinMediana_", StringComparison.OrdinalIgnoreCase) ||
                                        layerName.StartsWith("Tadil_UnicaGen_", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (IsPointInPolyline2D(p2d, poly))
                                        {
                                            capaPolilinea = layerName;
                                            estaDentro = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        tr.Commit();
                    }
                }
            }
            catch { }

            return ptoRoad;
        }

        /// <summary>
        /// Comprueba si un punto 2D está dentro de una polilínea cerrada utilizando el algoritmo de Ray Casting.
        /// </summary>
        private bool IsPointInPolyline2D(Point2d p, Polyline poly)
        {
            int numVertices = poly.NumberOfVertices;
            if (numVertices < 3) return false;

            int crossings = 0;
            for (int i = 0; i < numVertices; i++)
            {
                Point2d p1 = poly.GetPoint2dAt(i);
                Point2d p2 = poly.GetPoint2dAt((i + 1) % numVertices);

                // Algoritmo de Ray Casting (cruce de rayos)
                if (((p1.Y > p.Y) != (p2.Y > p.Y)) &&
                    (p.X < (p2.X - p1.X) * (p.Y - p1.Y) / (p2.Y - p1.Y) + p1.X))
                {
                    crossings++;
                }
            }
            return (crossings % 2) != 0;
        }

        /// <summary>
        /// Busca en la base de datos (dsBd) la sección cuyo nombre coincide con la parte
        /// posterior al prefijo de la capa, buscando en la tabla correspondiente.
        /// </summary>
        /// <param name="capaPolilinea">Nombre de la capa de la polilínea encontrada.</param>
        /// <returns>El GUID de la sección encontrada, o null si no se encuentra.</returns>
        public Guid? ObtenerGuidSeccionDesdeCapa(string capaPolilinea)
        {
            if (string.IsNullOrEmpty(capaPolilinea)) return null;

            string nombreSeccion = string.Empty;
            string tipoTabla = string.Empty;

            if (capaPolilinea.StartsWith("Tadil_UnicaGen_", StringComparison.OrdinalIgnoreCase))
            {
                nombreSeccion = capaPolilinea.Substring("Tadil_UnicaGen_".Length);
                tipoTabla = "UNIGEN";
            }
            else if (capaPolilinea.StartsWith("Tadil_DobleSinMediana_", StringComparison.OrdinalIgnoreCase))
            {
                nombreSeccion = capaPolilinea.Substring("Tadil_DobleSinMediana_".Length);
                tipoTabla = "DOBSIN";
            }
            else if (capaPolilinea.StartsWith("Tadil_Doble_", StringComparison.OrdinalIgnoreCase))
            {
                nombreSeccion = capaPolilinea.Substring("Tadil_Doble_".Length);
                tipoTabla = "DOBAUT";
            }
            else
            {
                return null;
            }

            if (string.IsNullOrEmpty(nombreSeccion)) return null;

            try
            {

                var dsBdInstance = oSingletonDsBd.getInstance;
                if (dsBdInstance == null || dsBdInstance.dataset == null) return null;

                if (tipoTabla == "UNIGEN")
                {
                    // Buscar en tbRoadUniGen
                    foreach (dsBd.tbRoadUniGenRow row in dsBdInstance.dataset.tbRoadUniGen)
                    {
                        if (row.nombre.Equals(nombreSeccion, StringComparison.OrdinalIgnoreCase))
                        {
                            return row.id;
                        }
                    }
                }
                else if (tipoTabla == "DOBAUT")
                {
                    // Buscar en tbRoadDobleAutovia
                    foreach (dsBd.tbRoadDobleAutoviaRow row in dsBdInstance.dataset.tbRoadDobleAutovia)
                    {
                        if (row.nombre.Equals(nombreSeccion, StringComparison.OrdinalIgnoreCase))
                        {
                            return row.id;
                        }
                    }
                }
                else if (tipoTabla == "DOBSIN")
                {
                    // Buscar en tbRoadDobleSinMediana
                    foreach (dsBd.tbRoadDobleSinMedianaRow row in dsBdInstance.dataset.tbRoadDobleSinMediana)
                    {
                        if (row.nombre.Equals(nombreSeccion, StringComparison.OrdinalIgnoreCase))
                        {
                            return row.id;
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        public void Cambiar_EstudioCarretera(double miPk)
        {

            string capaPolilinea = "";
            bool estaDentro = false;
            ObtenerPuntoYComprobarPolilineas(miPk, out capaPolilinea, out estaDentro);
            oCoeMinoracionAlturasMaximas coe = null;
            if (mEstudioInformativoCarretera is oEstudioInformativoCarretera infoEstudio)
            {
                coe = infoEstudio.mCoeMinAlturasMaximas;
            }
            if (estaDentro)
            {
                Guid id = (Guid)ObtenerGuidSeccionDesdeCapa(capaPolilinea);

                eSecRoadTipo tipo = oSingletonProyecto.getInstance.secRoadTipo;
                if (capaPolilinea.StartsWith("Tadil_UnicaGen_", StringComparison.OrdinalIgnoreCase))
                {
                    tipo = eSecRoadTipo.UNIGEN;
                }
                else if (capaPolilinea.StartsWith("Tadil_DobleSinMediana_", StringComparison.OrdinalIgnoreCase))
                {
                    tipo = eSecRoadTipo.DOBSIN;
                }
                else if (capaPolilinea.StartsWith("Tadil_DobleUrbana_", StringComparison.OrdinalIgnoreCase) ||
                         capaPolilinea.StartsWith("Tadil_DobleUrb_", StringComparison.OrdinalIgnoreCase))
                {
                    tipo = eSecRoadTipo.DOBURB;
                }
                else if (capaPolilinea.StartsWith("Tadil_Doble_", StringComparison.OrdinalIgnoreCase))
                {
                    tipo = eSecRoadTipo.DOBAUT;
                }

                mEstudioInformativoCarretera = new oEstudioInformativoCarretera(tipo, id, coe, oSingletonProyecto.getInstance.SeccionesVinculadas);
                mSeccionCalzadaCompletaSinGis = tadLayLogica.Secciones.Calzada.oFactorySeccionCalzada.createSeccionRoad(tipo, id);

            }
            else
            {
                mEstudioInformativoCarretera = new oEstudioInformativoCarretera(oSingletonProyecto.getInstance.secRoadTipo, 
                    oSingletonProyecto.getInstance.seccionCalzadaId, coe, oSingletonProyecto.getInstance.SeccionesVinculadas);
                mSeccionCalzadaCompletaSinGis = tadLayLogica.Secciones.Calzada.oFactorySeccionCalzada.createSeccionRoad(oSingletonProyecto.getInstance.secRoadTipo,
                    oSingletonProyecto.getInstance.seccionCalzadaId); 
            }
        }

        #endregion
    }


}
