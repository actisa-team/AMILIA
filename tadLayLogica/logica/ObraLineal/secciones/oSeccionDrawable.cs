using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

namespace tadLayLogica.logica.Secciones
{

    using System.Diagnostics;

    using engNet.Extension.Double;
    using engCadNet;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using tadLayLogica.Secciones.Geometria;
    using tadLayLogica.Secciones.Geometria.Saneo;
    using tadLayLogica.logica.medicion;
    using tadLayLogica.Secciones.Calzada;
    using tadLayLogica.Secciones.Planta;
    using tadLayLogica.zonaGis;
    using tadLayLogica.logica.EjeBasicoNew;


    using System.IO;
    using tadLayShare;

    using engCadNet.tools;
    public abstract class oSeccionDrawable
    {

        #region "MetodosEstaticos"

        /// <summary>
        /// Pk-AnchoDer-AnchoIzq
        /// </summary>
        public static Func<double, double, double, Point3dCollection> funGetLstPtoTns = null;
        /// <summary>
        /// Pk-AnchoDer-AnchoIzq
        /// </summary>
        public static Func<double, double, double, Point3dCollection> funGetLstPtoTnd = null;
        /// <summary>
        /// Elevacion PK
        /// </summary>
        public static Func<double, double> funGetElevationAt = null;
        /// <summary>
        /// PK ; X lado
        /// </summary>
        public static Func<double, double, eLado, Point3d> funGetPtoTnd = null;
        /// <summary>
        /// Obtener un Punto en el Eje de Trazado, dado un punto en el ScRoad
        /// </summary>
        public static Func<double, double, eLado, Point3d> funGetPtoEjeTrazadoFromDesfase = null;
        /// <summary>
        /// Obtener el Punto en el Eje de Trazado desde un Punto en el Sistema Referencia ScRoad
        /// </summary>
        public static Point3d getPtoEjeTrazadoFromScRoadXandLado(double iPk, double iX, eLado iLado)
        {
            Point3d miPtoXy = funGetPtoEjeTrazadoFromDesfase(iPk, iX, iLado);

            return miPtoXy;
        }
        /// <summary>
        /// Obtener la Elevación de un Pk
        /// </summary>
        public static double getElevationByPk(double iPk)
        {
            return Math.Round(funGetElevationAt(iPk), 3);
        }

        #endregion

        public oSeccionGuitarra guitarra { get; protected set; }
        public abstract eRoadSeccion seccionTipo { get; }
        public abstract oSeccionMedicion getMediciones(double iSeccionIntervalo);
        public abstract oSeccionPlantaPk getPlanta();
        public abstract void draw(Point2d iPtoInsert, string iLayer);
        public virtual Point3d getPtoInsertMediciones()
        {
            Point3d miPtoOrigenSeccionWorld = new Point3d(0, 0.1, 0);
            miPtoOrigenSeccionWorld = miPtoOrigenSeccionWorld.TransformBy(this.guitarra.matrizRoadDerecha);
            return miPtoOrigenSeccionWorld;
        }


    }
    public class oSeccionCalzada : oSeccionDrawable
    {

        double KAnchoMaximoSeccionMetros = 2000;
        double KIncrementoDeAnchoSeccion = 10;


        #region "PROPIEDADES PRIVADAS"


        private oSeccionRoadCompletaConGIS mSecRoadFullByPk;

        private Point2d mInsert;

        private string mLayer;

        /// Terreno Natural Superior
        private Polyline mLwTnsOriginal;
        /// Terreno Natural Desbrozado
        private Polyline mLwTndOriginal;
        /// Polilinea Terreno Natural Desbrozado de la Sección (Gris)
        private Polyline mLwSeccionTnd;
        /// Polilinea Terreno Natural Superior de la Sección (Gris)
        private Polyline mLwSeccionTns;
        /// Polilinea Terreno Natural Desbrozado de la Sección (Gris)
        private Polyline mLwSeccionTndTnsIzq;
        /// Polilinea Terreno Natural Superior de la Sección (Gris)
        private Polyline mLwSeccionTndTnsDer;
        /// <summary>
        /// Polilinea Original Talud Izq
        /// </summary>
        private Polyline mLwTaludIzqOriginal;
        /// <summary>
        /// Polilinea Origina Talud Derecho
        /// </summary>
        private Polyline mLwTaludDerOriginal;


        /// <summary>
        /// Polilinea Talud Izquierdo
        /// </summary>
        private oTaludSeccion mTaludDraw_Izq = null;
        /// <summary>
        /// Polilinea Talud Derecho
        /// </summary>
        private oTaludSeccion mTaludDraw_Der = null;

        /// <summary>
        /// Polilinea Determina la Envolvente Inferior
        /// </summary>
        private Polyline mLwEnvolventeInferior = null;

        private oSecSaneo mSaneo = null;


        //Sistemas Referencia
        private Matrix3d mMatrixLineaMuestreo;
        private Matrix3d mMatrixLineaMuestreoInvert;
        private Matrix3d mMatrixRoadDer;
        private Matrix3d mMatrixRoadIzq;


        #endregion
        #region "CONSTRUCTOR"
        public oSeccionCalzada(oSeccionRoadCompletaConGIS iSecRoadFull, oSeccionGuitarra iGuitarra)
        {
            mSecRoadFullByPk = iSecRoadFull;
            this.guitarra = iGuitarra;
            this.elevationPk = getElevationByPk(iSecRoadFull.Pk);

            this.mTaludDraw_Izq = new oTaludSeccion();
            this.mTaludDraw_Der = new oTaludSeccion();

        }
        #endregion
        #region "PROPIEDADES"

        public override eRoadSeccion seccionTipo
        {
            get { return eRoadSeccion.calzada; }
        }


        /// <summary>
        /// Obtener Elevacion Z Eje por PK
        /// </summary>
        private double elevationPk { get; set; }


        /// <summary>
        /// Cota Z del TND segun X y Lado
        /// </summary>
        private double getZtndFromXandLado(double iX, eLado iLado)
        {
            return funGetPtoTnd(mSecRoadFullByPk.Pk, iX, iLado).Z;
        }

        private Point3d getPlantaPtoFromScWorld(Point3d iPto, eLado iLado)
        {

            //OJO SOLO ME QUEDO CON LA COORDENADA X
            Point3d miPtoScLineaMuestreo = iPto.TransformBy(mMatrixLineaMuestreoInvert);

            return funGetPtoEjeTrazadoFromDesfase(mSecRoadFullByPk.Pk, Math.Abs(miPtoScLineaMuestreo.X), iLado);
        }

        #endregion
        #region "METODOS PUBLICOS"

        /// <summary>
        /// DIBUJAR LAS SECCIONES CALZADA
        /// </summary>
        public override void draw(Point2d iInsert, string iLayer)
        {
            mInsert = iInsert;
            mLayer = iLayer;

            //Cargo los Sistemas de Referencia
            this.setUpSistemaReferencia();

            //Dibujo el Perfil del Terreno
            this.drawTerrenoAndTalud(this.guitarra.dimensiones.anchoDer, this.guitarra.dimensiones.anchoIzq);

            //Dibujo Saneo
            this.drawSaneo();

            //Dibujo Guitarra
            this.guitarra.draw(iLayer);

            //Dibujo la Seccion de la Carretera
            this.drawSeccion();

        }
        /// <summary>
        /// OBTENER LOS PUNTOS EN PLANTA
        /// </summary>
        public override oSeccionPlantaPk getPlanta()
        {

            //Talud Izquierda
            oTramoExcavacionTalud miTramoExcavaIzq;
            oTramoExcavacionTalud miTramoExcavaDer;


            Point3d miPtoBase = Point3d.Origin;
            Point3d miPtoHead = Point3d.Origin;

            //LADO IZQ
            miPtoBase = getPtoEjeTrazadoFromScRoadXandLado(mSecRoadFullByPk.Pk, mSecRoadFullByPk.secRoadIzq.ptoCalzadaExterior.X, eLado.IZQ);



            //EXISTE LA LINEA TALUD
            if (mTaludDraw_Izq.taludLw != null && mTaludDraw_Izq.taludLw.Length > 0)
            {

                miPtoHead = getPlantaPtoFromScWorld(mTaludDraw_Izq.taludLw.EndPoint, eLado.IZQ);

                switch (mSecRoadFullByPk.secRoadIzq.taludTipo)
                {

                    case eExcavacion.desmonte:

                        miTramoExcavaIzq = new oTramoExcavacionTalud(miPtoBase, miPtoHead, eExcavacion.desmonte);

                        break;

                    case eExcavacion.terraplen:

                        miTramoExcavaIzq = new oTramoExcavacionTalud(miPtoBase, miPtoHead, eExcavacion.terraplen);
                        break;

                    case eExcavacion.acota:

                        miTramoExcavaIzq = new oTramoExcavacionTalud(miPtoBase, miPtoHead, eExcavacion.acota);

                        break;

                    default:

                        throw new oExEnumNotImplemented(mSecRoadFullByPk.secRoadIzq.taludTipo.ToString());


                }


            }

            // NO EXISTE LW TALUD
            else
            {
                miTramoExcavaIzq = new oTramoExcavacionTalud(miPtoBase, miPtoBase, eExcavacion.acota);
            }


            //LADO DERECHO
            miPtoBase = getPtoEjeTrazadoFromScRoadXandLado(mSecRoadFullByPk.Pk, mSecRoadFullByPk.secRoadDer.ptoCalzadaExterior.X, eLado.DER);


            //EXISTE LA LINEA TALUD
            if (mTaludDraw_Der.taludLw != null && mTaludDraw_Der.taludLw.Length > 0)
            {

                miPtoHead = getPlantaPtoFromScWorld(mTaludDraw_Der.taludLw.EndPoint, eLado.DER);


                switch (mSecRoadFullByPk.secRoadDer.taludTipo)
                {

                    case eExcavacion.desmonte:

                        miTramoExcavaDer = new oTramoExcavacionTalud(miPtoBase, miPtoHead, eExcavacion.desmonte);

                        break;

                    case eExcavacion.terraplen:

                        miTramoExcavaDer = new oTramoExcavacionTalud(miPtoBase, miPtoHead, eExcavacion.terraplen);

                        break;

                    case eExcavacion.acota:

                        miTramoExcavaDer = new oTramoExcavacionTalud(miPtoBase, miPtoHead, eExcavacion.acota);
                        break;

                    default:

                        throw new oExEnumNotImplemented(mSecRoadFullByPk.secRoadIzq.taludTipo.ToString());
                }
            }
            // NO EXISTE LW TALUD
            else
            {
                miTramoExcavaDer = new oTramoExcavacionTalud(miPtoBase, miPtoBase, eExcavacion.acota);
            }



            return new oSeccionPlantaPk(mSecRoadFullByPk.Pk, eRoadSeccion.calzada, miTramoExcavaIzq, miTramoExcavaDer);

        }
        /// <summary>
        /// OBTENER LAS MEDECIONES DE LA SECCIÓN
        /// </summary>
        public override oSeccionMedicion getMediciones(double iPkIntervalo)
        {
            oSeccionMedicionMovimientoTierras miSecMedicion = new oSeccionMedicionMovimientoTierras(mSecRoadFullByPk.zonaMovTierra.id, mSecRoadFullByPk.zonaMovTierra.row.nombre, mSecRoadFullByPk.Pk);


            #region "Medicion Desmonte Terraplen"

            //envolvente Puntos Inferiores LineasTalud+Decoradores+LineasTalud
            mLwEnvolventeInferior = getLwEnvolvente();

            oSecMedicionTerraplenDesmonte miDesmonteTerraplen = new oSecMedicionTerraplenDesmonte(mSecRoadFullByPk.zonaMovTierra.row.excavacionMat,
                                                                                                  mSecRoadFullByPk.zonaMovTierra.row.terraplenRellenoMat,
                                                                                                  mLwEnvolventeInferior,
                                                                                                  mLwSeccionTnd);


            miSecMedicion.AddRangeMedicionUnitaria(miDesmonteTerraplen.medicion, iPkIntervalo);


            #endregion

            #region "Medicion Sup. Desbrozado"

            List<Polyline> miLstLwVegetal = new List<Polyline>();
            miLstLwVegetal.Add(mLwSeccionTnd);
            miLstLwVegetal.Add(mLwSeccionTndTnsIzq);
            miLstLwVegetal.Add(mLwSeccionTndTnsDer);
            miLstLwVegetal.Add(mLwSeccionTns);
            Polyline miSupVegetal = oLw.joinLw(miLstLwVegetal, true);

            miSecMedicion.AddMedicionUnitaria(new oMedDesbroce(mSecRoadFullByPk.zonaMovTierra.row.desbroceMat, miSupVegetal.Area), iPkIntervalo);

            #endregion

            #region "Medicion Saneo"

            if (mSaneo != null)
            {
                miSecMedicion.AddRangeMedicionUnitaria(mSaneo.medicion, iPkIntervalo);
            }
            #endregion


            #region "Medion de los Decoradores"

            for (int i = 0; i < mSecRoadFullByPk.secDecoratorIzq.dicIsecDraw.Count; i++)
            {
                miSecMedicion.AddRangeMedicionUnitaria(mSecRoadFullByPk.secDecoratorIzq.dicIsecDraw[i].medicion, iPkIntervalo);
            }

            for (int i = 0; i < mSecRoadFullByPk.secDecoratorDer.dicIsecDraw.Count; i++)
            {
                miSecMedicion.AddRangeMedicionUnitaria(mSecRoadFullByPk.secDecoratorDer.dicIsecDraw[i].medicion, iPkIntervalo);
            }


            #endregion



            return miSecMedicion;


        }

        #endregion
        #region "METODOS PRIVADOS"

        /// <summary>
        /// SET UP SISTEMAS DE REFERENCIA
        /// </summary>
        private void setUpSistemaReferencia()
        {

            //Linea Muestreo to WORLD
            mMatrixLineaMuestreo = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                                                                     Vector3d.XAxis,
                                                                     Vector3d.YAxis,
                                                                     Vector3d.ZAxis,
                                                                     new Point3d(mInsert.X + guitarra.dimensiones.anchoIzq, -guitarra.dimensiones.elevacionMin + mInsert.Y, 0),
                                                                     Vector3d.XAxis,
                                                                     Vector3d.YAxis,
                                                                     Vector3d.ZAxis);

            //Linea Muestreo to WORLD
            mMatrixLineaMuestreoInvert = Matrix3d.AlignCoordinateSystem(
                                                                     new Point3d(mInsert.X + guitarra.dimensiones.anchoIzq, -guitarra.dimensiones.elevacionMin + mInsert.Y, 0),
                                                                     Vector3d.XAxis,
                                                                     Vector3d.YAxis,
                                                                     Vector3d.ZAxis,
                                                                     Point3d.Origin,
                                                                     Vector3d.XAxis,
                                                                     Vector3d.YAxis,
                                                                     Vector3d.ZAxis);
            //Road Derecha
            mMatrixRoadDer = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                                                               Vector3d.XAxis,
                                                               Vector3d.YAxis,
                                                               Vector3d.ZAxis,
                                                               new Point3d(mInsert.X + guitarra.dimensiones.anchoIzq, elevationPk - guitarra.dimensiones.elevacionMin + mInsert.Y, 0),
                                                               Vector3d.XAxis,
                                                               Vector3d.YAxis,
                                                               Vector3d.ZAxis);
            ////ROAD IZQUIERDA
            mMatrixRoadIzq = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                                                              -Vector3d.XAxis,
                                                               Vector3d.YAxis,
                                                               Vector3d.ZAxis,
                                                               new Point3d(mInsert.X + guitarra.dimensiones.anchoIzq, elevationPk - guitarra.dimensiones.elevacionMin + mInsert.Y, 0),
                                                               Vector3d.XAxis,
                                                               Vector3d.YAxis,
                                                               Vector3d.ZAxis);
        }
        /// <summary>
        /// DRAW TERRENO
        /// </summary>
        private void drawTN(double iAnchoDerecho, double iAnchoIzquierdo)
        {

            Point3dCollection miLstPtoTnd = funGetLstPtoTnd(mSecRoadFullByPk.Pk, iAnchoDerecho, iAnchoIzquierdo);
            Point3dCollection miLstPtoTns = funGetLstPtoTns(mSecRoadFullByPk.Pk, iAnchoDerecho, iAnchoIzquierdo);

            mLwTnsOriginal = oLw.addLw2d(miLstPtoTns, false, mLayer, mMatrixLineaMuestreo, oColor.getInstance.verde);
            mLwTndOriginal = oLw.addLw2d(miLstPtoTnd, false, mLayer, mMatrixLineaMuestreo, oColor.getInstance.grisClaro);
        }
        /// <summary>
        /// DRAW SECCION
        /// </summary>
        private void drawSeccion()
        {
            mSecRoadFullByPk.secDecoratorIzq.draw(mLayer, mMatrixRoadIzq);
            mSecRoadFullByPk.secDecoratorDer.draw(mLayer, mMatrixRoadDer);
        }
        /// <summary>
        /// DRAW TERREO AND TALUDES
        /// </summary>
        private void drawTerrenoAndTalud(double iAnchoDer, double iAnchoIzq)
        {
            while (true)
            {
                try
                {
                    //Dibujo el Terreno
                    this.drawTN(this.guitarra.dimensiones.anchoDer, this.guitarra.dimensiones.anchoIzq);

                    //Dibujo el Talud
                    this.drawTalud();

                    // Si todo ha ido bien, salimos del bucle
                    break;
                }
                catch (Exception ex)
                {
                    if (ex is oExInterseccionTaludToTerrenoIsNull || ex is engCadNet.oExInterseccionNull)
                    {
                        engCadNet.oTools.entidadDelete(mLwTndOriginal);
                        engCadNet.oTools.entidadDelete(mLwTnsOriginal);
                        engCadNet.oTools.entidadDelete(mLwTaludIzqOriginal);
                        engCadNet.oTools.entidadDelete(mLwTaludDerOriginal);

                        this.mTaludDraw_Izq.deleteTalud();
                        this.mTaludDraw_Der.deleteTalud();

                        this.guitarra.dimensiones.addAnchoDerAndIzq(KIncrementoDeAnchoSeccion);
                        this.setUpSistemaReferencia();

                        if (this.guitarra.dimensiones.ancho > KAnchoMaximoSeccionMetros)
                        {
                            drawTaludHastaBorde(iAnchoDer, iAnchoIzq);
                            break;
                            //TODO  
                        }

                        // Si no se supera el máximo, el bucle continúa y reintenta con los nuevos anchos
                    }
                    else
                    {
                        throw new Exception(string.Format("Error no controlado en talud"));
                    }
                }
            }
        }

        /// <summary>
        /// DRAW TALUD HASTA BORDE (sin intersección con terreno — ancho máximo superado)
        /// Proyecta el talud hasta el extremo del terreno disponible y lo dibuja
        /// en color naranja para indicar visualmente que no cerró.
        /// </summary>
        private void drawTaludHastaBorde(double iAnchoDerInicial, double iAnchoIzqInicial)
        {
            try
            {
                // Primero ajustamos las dimensiones al primer ancho más 10
                this.guitarra.dimensiones.anchoDer = iAnchoDerInicial + 10;
                this.guitarra.dimensiones.anchoIzq = iAnchoIzqInicial + 10;
                this.setUpSistemaReferencia();

                // Dibujamos el terreno con este nuevo ancho para tener los objetos mLwTndOriginal y mLwTnsOriginal
                this.drawTN(this.guitarra.dimensiones.anchoDer, this.guitarra.dimensiones.anchoIzq);

                //Polilinea Talud Original Sin Recortar
                mLwTaludIzqOriginal = oLw.addLw2d(mSecRoadFullByPk.secDecoratorIzq.taludLstPol, false, mLayer, mMatrixRoadIzq, oColor.getInstance.cyan);
                mLwTaludDerOriginal = oLw.addLw2d(mSecRoadFullByPk.secDecoratorDer.taludLstPol, false, mLayer, mMatrixRoadDer, oColor.getInstance.cyan);

                // Punto extremo del terreno en cada lado (las polilíneas de terreno van de derecha a izquierda)
                Point3d ptoBordeTndIzq = mLwTndOriginal.EndPoint;
                Point3d ptoBordeTndDer = mLwTndOriginal.StartPoint;
                Point3d ptoBordeTnsIzq = mLwTnsOriginal.EndPoint;
                Point3d ptoBordeTnsDer = mLwTnsOriginal.StartPoint;

                // Color naranja para indicar talud sin cerrar
                Color colorSinCerrar = Color.FromRgb(255, 140, 0);

                // Talud Izq: desde el pie de calzada hasta el borde izq del terreno
                Polyline miLwTaludIzqBorde = oLw.addLw2d(mLwTaludIzqOriginal.StartPoint, ptoBordeTndIzq, mLayer, null, colorSinCerrar);
                mTaludDraw_Izq = new oTaludSeccion(miLwTaludIzqBorde, ptoBordeTnsIzq, ptoBordeTndIzq);

                // Talud Der: desde el pie de calzada hasta el borde der del terreno
                Polyline miLwTaludDerBorde = oLw.addLw2d(mLwTaludDerOriginal.StartPoint, ptoBordeTndDer, mLayer, null, colorSinCerrar);
                mTaludDraw_Der = new oTaludSeccion(miLwTaludDerBorde, ptoBordeTnsDer, ptoBordeTndDer);

                // Líneas verticales TND-TNS en cada extremo
                mLwSeccionTndTnsIzq = oLw.addLw2d(ptoBordeTndIzq, ptoBordeTnsIzq, mLayer, null, oColor.getInstance.grisClaro);
                mLwSeccionTndTnsDer = oLw.addLw2d(ptoBordeTndDer, ptoBordeTnsDer, mLayer, null, oColor.getInstance.grisClaro);

                // El terreno visible es el tramo completo entre los dos bordes
                mLwSeccionTns = mLwTnsOriginal;
                mLwSeccionTnd = mLwTndOriginal;

                // Borro las originales temporales
                oTools.entidadDelete(mLwTaludIzqOriginal.ObjectId);
                oTools.entidadDelete(mLwTaludDerOriginal.ObjectId);

                // Casos especiales: si el muro hace de talud, borramos el talud proyectado
                if (!mSecRoadFullByPk.secDecoratorIzq.taludDraw)
                {
                    mTaludDraw_Izq.deleteTalud();
                }
                if (!mSecRoadFullByPk.secDecoratorDer.taludDraw)
                {
                    mTaludDraw_Der.deleteTalud();
                }
            }
            catch
            {
                throw new Exception(string.Format("El ancho de la sección supera el valor máximo {0}", KAnchoMaximoSeccionMetros.ToString()));
            }
        }
        /// <summary>
        /// DRAW TALUD
        /// </summary>
        private void drawTalud()
        {

            //Polilinea Talud Original Sin Recortar
            mLwTaludIzqOriginal = oLw.addLw2d(mSecRoadFullByPk.secDecoratorIzq.taludLstPol, false, mLayer, mMatrixRoadIzq, oColor.getInstance.cyan);
            mLwTaludDerOriginal = oLw.addLw2d(mSecRoadFullByPk.secDecoratorDer.taludLstPol, false, mLayer, mMatrixRoadDer, oColor.getInstance.cyan);


            mTaludDraw_Izq = this.getTalud(mLwTaludIzqOriginal);
            mTaludDraw_Der = this.getTalud(mLwTaludDerOriginal);


            mLwSeccionTndTnsIzq = oLw.addLw2d(mTaludDraw_Izq.ptoHeadTnd, mTaludDraw_Izq.ptoHeadTns, mLayer, null, oColor.getInstance.grisClaro);
            mLwSeccionTndTnsDer = oLw.addLw2d(mTaludDraw_Der.ptoHeadTnd, mTaludDraw_Der.ptoHeadTns, mLayer, null, oColor.getInstance.grisClaro);

            mLwSeccionTns = this.getSplitTN(mLwTnsOriginal, mTaludDraw_Izq.ptoHeadTns, mTaludDraw_Der.ptoHeadTns);
            mLwSeccionTnd = this.getSplitTN(mLwTndOriginal, mTaludDraw_Izq.ptoHeadTnd, mTaludDraw_Der.ptoHeadTnd);

            //Borro las Primarias
            oTools.entidadDelete(mLwTaludIzqOriginal.ObjectId);
            oTools.entidadDelete(mLwTaludDerOriginal.ObjectId);


            //Borro los Talud //Casos Especiales el muro hace de talud, etc...
            if (!mSecRoadFullByPk.secDecoratorIzq.taludDraw)
            {

                mTaludDraw_Izq.deleteTalud();

            }
            if (!mSecRoadFullByPk.secDecoratorDer.taludDraw)
            {
                mTaludDraw_Der.deleteTalud();
            }


        }
        /// <summary>
        /// DRAW SANEO
        /// </summary>
        private void drawSaneo()
        {

            #region "Listado Puntos Explanada"

            Polyline miLwExplanda;

            Point3dCollection miLstLwExplanda = new Point3dCollection();

            Point3dCollection miColIzq = mSecRoadFullByPk.secRoadIzq.lstPtoExplanada;
            Point3dCollection miColDer = mSecRoadFullByPk.secRoadDer.lstPtoExplanada;

            int j = miColIzq.Count - 1;


            while (j >= 0)
            {
                miLstLwExplanda.Add(miColIzq[j].TransformBy(mMatrixRoadIzq));

                j--;
            }

            for (int i = 1; i < miColDer.Count; i++)
            {
                miLstLwExplanda.Add(miColDer[i].TransformBy(mMatrixRoadDer));
            }


            miLwExplanda = oLw.addLw2d(miLstLwExplanda, false, mLayer, null, oColor.getInstance.grisClaro);


            #endregion


            oSaneoDesmonteModel miSaneoDesmonte = null;
            oSaneoTerraplenModel miSaneoTerraplen = null;


            if (mSecRoadFullByPk.createSaneoDesmonte)
            {

                miSaneoDesmonte = new oSaneoDesmonteModel(mSecRoadFullByPk.zonaMovTierra.row.excavacionMat,
                                                          mSecRoadFullByPk.zonaMovTierra.row.saneoDesmonteMat,
                                                          mSecRoadFullByPk.zonaMovTierra.row.saneoDesmonteEspesor);

            }


            if (mSecRoadFullByPk.createSaneoTerraplen)
            {

                miSaneoTerraplen = new oSaneoTerraplenModel(mSecRoadFullByPk.zonaMovTierra.row.excavacionMat,
                                                             mSecRoadFullByPk.zonaMovTierra.row.saneoTerraplenMat,
                                                             mSecRoadFullByPk.zonaMovTierra.row.saneoTerraplenEspesor,
                                                             mSecRoadFullByPk.zonaMovTierra.row.saneoTerraplenEscalonAltura,
                                                             mSecRoadFullByPk.zonaMovTierra.row.saneoTerraplenPendMaxSinEscalon);


            }


            if (mSecRoadFullByPk.createSaneoDesmonte | mSecRoadFullByPk.createSaneoTerraplen)
            {
                mSaneo = new oSecSaneo(miLwExplanda, mLwTndOriginal, mLwSeccionTnd, miSaneoDesmonte, miSaneoTerraplen, mSecRoadFullByPk.Pk);

                mSaneo.draw(mLayer);

                oTools.entidadDelete(miLwExplanda);
            }

        }
        /// <summary>
        /// OBTENER ENVOLVENTE INFERIOR
        /// </summary>
        private Polyline getLwEnvolvente()
        {

            //1 Puntos de los Talud
            Polyline miLwEnvolventeTotal;



            //2 Puntos Singulares de los Decoradores

            Point3d miPtoScw;
            Point3dCollection miCol = new Point3dCollection();
            List<Point3d> listaAux = new List<Point3d>();


            int k = 0;
            for (int i = 0; i < mSecRoadFullByPk.secDecoratorIzq.dicIsecDraw.Count; i++)
            {
                miCol = mSecRoadFullByPk.secDecoratorIzq.dicIsecDraw[i].envolvente;

                foreach (Point3d fPto in miCol)
                {
                    miPtoScw = fPto.TransformBy(mMatrixRoadIzq);
                    if (!listaAux.Contains(miPtoScw))
                        listaAux.Add(miPtoScw);
                    k++;
                }
            }




            k = 0;
            for (int i = 0; i < mSecRoadFullByPk.secDecoratorDer.dicIsecDraw.Count; i++)
            {
                miCol = new Point3dCollection();
                miCol = mSecRoadFullByPk.secDecoratorDer.dicIsecDraw[i].envolvente;

                foreach (Point3d fPto in miCol)
                {
                    miPtoScw = fPto.TransformBy(mMatrixRoadDer);

                    if (!listaAux.Contains(miPtoScw))
                        listaAux.Add(miPtoScw);
                    k++;
                }
            }
            if (mTaludDraw_Izq.taludLw != null)
            {

                for (int i = 0; i < mTaludDraw_Izq.taludLw.NumberOfVertices; i++)
                {
                    var fPto = mTaludDraw_Izq.taludLw.GetPoint3dAt(i);
                    if (!listaAux.Contains(fPto))
                        listaAux.Add(fPto);
                }

            }
            if (mTaludDraw_Der.taludLw != null)
            {

                for (int i = 0; i < mTaludDraw_Der.taludLw.NumberOfVertices; i++)
                {
                    var fPto = mTaludDraw_Der.taludLw.GetPoint3dAt(i);
                    if (!listaAux.Contains(fPto))
                        listaAux.Add(fPto);
                }
            }

            var polilineaAux = CreatePoinCollectionOrdenada(listaAux);
            miLwEnvolventeTotal = oLw.addLw2d(polilineaAux, false, mLayer, null, oColor.getInstance.grisClaro);

            return miLwEnvolventeTotal;

        }

        private Point3dCollection CreatePoinCollectionOrdenada(List<Point3d> listaAux)
        {
            var polOrdX = OrdenarX(listaAux);
            polOrdX = LimpiarXIguales(polOrdX);
            polOrdX = OrdenarX(polOrdX);
            var polOrdXY = OrdenarY(polOrdX);


            return polOrdXY;
        }

        private static Point3dCollection OrdenarY(List<Point3d> polOrdX)
        {
            Point3dCollection polOrdXY = new Point3dCollection();

            bool siguienteInsertado = false;
            for (int i = 0; i < polOrdX.Count; i++)
            {
                if (siguienteInsertado)
                {
                    siguienteInsertado = false;
                    continue;
                }
                if (i + 1 < polOrdX.Count && polOrdX[i].X == polOrdX[i + 1].X)
                {
                    siguienteInsertado = true;
                    if (i + 2 < polOrdX.Count)
                    {
                        var dif1 = Math.Abs(polOrdX[i + 2].Y - polOrdX[i].Y);
                        var dif2 = Math.Abs(polOrdX[i + 2].Y - polOrdX[i + 1].Y);

                        if (dif1 >= dif2)
                        {
                            polOrdXY.Add(polOrdX[i]);
                            polOrdXY.Add(polOrdX[i + 1]);
                        }
                        else
                        {
                            polOrdXY.Add(polOrdX[i + 1]);
                            polOrdXY.Add(polOrdX[i]);
                        }
                    }
                    else if (i - 1 >= 0)
                    {
                        var dif1 = Math.Abs(polOrdX[i - 1].Y - polOrdX[i].Y);
                        var dif2 = Math.Abs(polOrdX[i - 1].Y - polOrdX[i + 1].Y);

                        if (dif1 >= dif2)
                        {
                            polOrdXY.Add(polOrdX[i + 1]);
                            polOrdXY.Add(polOrdX[i]);
                        }
                        else
                        {
                            polOrdXY.Add(polOrdX[i]);
                            polOrdXY.Add(polOrdX[i + 1]);
                        }
                    }
                }
                else
                {
                    polOrdXY.Add(polOrdX[i]);
                }
            }
            return polOrdXY;
        }

        private static List<Point3d> OrdenarX(List<Point3d> listaAux)
        {
            List<Point3d> polOrdX = new List<Point3d>();
            while (listaAux.Count > 0)
            {
                var puntoMinX = new Point3d();
                var minX = double.MaxValue;

                for (int i = 0; i < listaAux.Count; i++)
                {
                    var point3D = listaAux[i];
                    if (minX > point3D.X)
                    {
                        puntoMinX = point3D;
                        minX = point3D.X;
                    }
                }
                var puntoRedondeado = new Point3d(Math.Round(puntoMinX.X, 2), Math.Round(puntoMinX.Y, 2), 0);
                if (!polOrdX.Contains(puntoRedondeado)) polOrdX.Add(puntoRedondeado);
                listaAux.Remove(puntoMinX);
            }
            return polOrdX;
        }

        private List<Point3d> LimpiarXIguales(List<Point3d> listaAux)
        {
            var limpios = new List<Point3d>();


            foreach (var point3D in listaAux)
            {
                var coincidencias = new List<Point3d>();
                coincidencias.Add(point3D);
                coincidencias.AddRange(listaAux.Where(d => !d.Equals(point3D) && Math.Abs(d.X - point3D.X) < 1));
                if (coincidencias.Count > 0 && coincidencias.Count < 3 && !limpios.Contains(coincidencias[0]))
                    limpios.AddRange(coincidencias);
                if (coincidencias.Count >= 3)
                {
                    var puntoMax = GetMaxYPoint(coincidencias);
                    var puntoMin = GetMinYPoint(coincidencias);
                    if (!limpios.Contains(puntoMax)) limpios.Add(puntoMax);
                    if (!limpios.Contains(puntoMin)) limpios.Add(puntoMin);
                }
            }

            return limpios;

        }

        private Point3d GetMinYPoint(List<Point3d> listaAux)
        {
            var minY = double.MaxValue;
            var pointMinY = new Point3d();
            foreach (var point3D in listaAux)
            {
                if (point3D.Y < minY)
                {
                    minY = point3D.Y;
                    pointMinY = point3D;
                }
            }
            return pointMinY;
        }

        private Point3d GetMaxYPoint(List<Point3d> listaAux)
        {
            var maxY = double.MinValue;
            var pointaxY = new Point3d();
            foreach (var point3D in listaAux)
            {
                if (point3D.Y > maxY)
                {
                    maxY = point3D.Y;
                    pointaxY = point3D;
                }
            }
            return pointaxY;
        }


        /// <summary>
        /// PARTIR LINEAS TERRENO
        /// </summary>
        /// <remarks> A veces da Errores por decimales</remarks>
        private Polyline getSplitTN(Polyline iLwTerreno, Point3d iPtoIzqInter, Point3d iPtoDerInter)
        {

            string miLado = string.Empty; ;

            if (engCadNet.oLw.isLeftToRight(iLwTerreno))
            {
                miLado = "izq-to-der";
            }
            else
            {
                miLado = "der-to-izq";
            }


            Point3dCollection miColPtoInt = new Point3dCollection();

            //Punto Derecho
            if (iLwTerreno.isPtoOnLw(iPtoDerInter))
            {
                miColPtoInt.Add(iPtoDerInter);
            }
            else
            {
                miColPtoInt.Add(iLwTerreno.getPointMasCercano(iPtoDerInter));
            }


            //Punto Izquierdo
            if (iLwTerreno.isPtoOnLw(iPtoIzqInter))
            {
                miColPtoInt.Add(iPtoIzqInter);
            }
            else
            {
                miColPtoInt.Add(iLwTerreno.getPointMasCercano(iPtoIzqInter));
            }

            using (Transaction tr = oCadManager.StartTransaction())
            {
                DBObjectCollection miLstLwSplit = iLwTerreno.GetSplitCurves(miColPtoInt);

                if (miLstLwSplit.Count == 3)
                {

                    ObjectId miObjId = ObjectId.Null;

                    List<Polyline> miLstLwBreak = new List<Polyline>();

                    Polyline miLw;

                    foreach (Entity acEnt in miLstLwSplit)
                    {
                        // Add each offset object
                        miObjId = oTools.entidadAdd(acEnt, mLayer);

                        miLw = (Polyline)tr.GetObject(miObjId, OpenMode.ForWrite);

                        miLstLwBreak.Add(miLw);

                    }

                    miLstLwBreak[0].Color = oColor.getInstance.verde;
                    miLstLwBreak[1].Color = oColor.getInstance.grisClaro;
                    miLstLwBreak[2].Color = oColor.getInstance.verde;

                    tr.Commit();

                    return miLstLwBreak[1];
                }
                else
                {

                    throw new Exception("Error al Dividir la linea del Terreno");
                }

            }
        }
        /// <summary>
        /// OBTENER LOS DATOS DEL TALUD DE LA SECCION
        /// </summary>
        private oTaludSeccion getTalud(Polyline iLwTalud)
        {

            oTaludSeccion miTaludSeccion = new oTaludSeccion();


            //CASO TALUD SIMPLE
            if (iLwTalud.NumberOfVertices == 2)
            {
                miTaludSeccion = this.getTaludSimpleTerreno(iLwTalud);
                return miTaludSeccion;

            }
            // CASO TALUD CON BERMAS
            else
            {
                miTaludSeccion = this.getTaludConBermas(iLwTalud);
                return miTaludSeccion;
            }
        }
        /// <summary>
        /// TALUD SIMPLE US TERRENO
        /// </summary>
        private oTaludSeccion getTaludSimpleTerreno(Polyline iLwTalud)
        {

            oTaludSeccion miTaludSeccion = new oTaludSeccion();


            //INTERSECCION Terreno Natural Desbrozado
            miTaludSeccion.ptoHeadTnd = this.getPtoInterseccionTaludSimpleToTerrenoDesbrozado(iLwTalud, mLwTndOriginal);

            //INTERSECCION Terreno Natural Superior
            miTaludSeccion.ptoHeadTns = this.getPtoInterseccionTaludSimpleToTerrenoSuperior(miTaludSeccion.ptoHeadTnd, iLwTalud.Layer, mLwTnsOriginal);

            //Creo el Talud
            Polyline miLwTalud = oLw.addLw2d(iLwTalud.StartPoint, miTaludSeccion.ptoHeadTnd, mLayer, null, oColor.getInstance.morado);

            miTaludSeccion.taludLw = miLwTalud;


            return miTaludSeccion;

        }
        /// <summary>
        /// INTERSECCION TALUD SIMPLE CON TERRENO NATURAL DESBROZADO
        /// </summary>
        private Point3d getPtoInterseccionTaludSimpleToTerrenoDesbrozado(Polyline iLwTalud, Polyline iPolilineaTerreno)
        {

            try
            {
                oInterseccionForward miInterseccion_Talud_Terreno = new oInterseccionForward(iLwTalud, iPolilineaTerreno);

                return miInterseccion_Talud_Terreno.getInterseccion();
            }
            catch (engCadNet.oExInterseccionNull)
            {
                throw new oExInterseccionTaludToTerrenoIsNull("No se ha podido Determinar la Intersección Talud-Terraplén");
            }
            catch (System.Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// INTERSECCION TALUD SIMPLE TERRENO SUPERIOR
        /// </summary>
        private Point3d getPtoInterseccionTaludSimpleToTerrenoSuperior(Point3d iPtoTnd, string iCapaTalud, Polyline iPolilineaTNS)
        {


            Point3d miPtoTndSuperior = iPtoTnd.getFromIncXIncY(0, 1, 0);

            Polyline miLwTaludVertical = oLw.addLw2d(iPtoTnd, miPtoTndSuperior, iCapaTalud, null, Color.FromColorIndex(ColorMethod.ByLayer, 0));

            try
            {

                oInterseccionForward miInterseccion = new oInterseccionForward(miLwTaludVertical, iPolilineaTNS);

                return miInterseccion.getInterseccion();
            }
            catch (engCadNet.oExInterseccionNull)
            {
                throw new oExInterseccionTaludToTerrenoIsNull("No se ha podido Determinar la Intersección Talud-Terraplén");
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {

                engCadNet.oTools.entidadDelete(miLwTaludVertical.ObjectId);
            }


        }
        /// <summary>
        /// INTERSECCION TALUD BERMAS US TERRENO
        /// </summary>
        private oTaludSeccion getTaludConBermas(Polyline iLwTaludBermas)
        {
            oTaludSeccion miTaludSeccion = new oTaludSeccion();

            miTaludSeccion.ptoHeadTnd = this.getPtoInterseccionTaludBermasToTerrenoNaturalDesbrozado(iLwTaludBermas, mLwTndOriginal);

            miTaludSeccion.ptoHeadTns = this.getPtoInterseccionTaludBermasToTerrenoNaturalSuperior(iLwTaludBermas, miTaludSeccion.ptoHeadTnd, mLwTnsOriginal);

            if (oLw.isPtoOnLw(iLwTaludBermas, miTaludSeccion.ptoHeadTnd))
            {
                miTaludSeccion.taludLw = getSplitTaludBermas(iLwTaludBermas, miTaludSeccion.ptoHeadTnd);

                return miTaludSeccion;
            }
            else
            {

                throw new oExInterseccionNull();
            }



        }
        /// <summary>
        /// INTERSECCION TALUD BERMAS TO TERRENO DESBROZADO
        /// </summary>
        public Point3d getPtoInterseccionTaludBermasToTerrenoNaturalDesbrozado(Polyline iLwTaludBermas, Polyline iLwTerrenoDesbrozado)
        {


            Point3dCollection miColPtoInter = new Point3dCollection();

            miColPtoInter = engCadNet.tools.oIntersectWithPlus.getInterseccion(iLwTerrenoDesbrozado, iLwTaludBermas, Intersect.OnBothOperands);

            if (miColPtoInter.Count == 0)
            {
                throw new oExInterseccionTaludToTerrenoIsNull("No se ha podido Determinar la Intersección Talud-Terreno");
            }
            else if (miColPtoInter.Count == 1)
            {
                return miColPtoInter[0];
            }
            else
            {
                return iLwTaludBermas.GetPointAtDist(0).getPtoMasCercanoAndOnPolyline(miColPtoInter, iLwTaludBermas);
            }
        }
        /// <summary>
        /// INTERSECCION TALUD BERMAS TERRENO SUPERIOR
        /// </summary>
        private Point3d getPtoInterseccionTaludBermasToTerrenoNaturalSuperior(Polyline iLwTaludBermas, Point3d iPtoHeadTnd, Polyline iLwTerrenoSuperior)
        {

            //OBTENGO LA INTERSECCION DEL TNS
            Point3d miTnsP2 = iPtoHeadTnd.getFromIncXIncY(0, 1, 0);

            Line miLineDesbroce = new Line(iPtoHeadTnd, miTnsP2);

            //Ahora realizo la intersección con el TND
            Point3dCollection miColPtoInter = new Point3dCollection();

            miLineDesbroce.IntersectWith(iLwTerrenoSuperior, Intersect.ExtendThis, miColPtoInter, IntPtr.Zero, IntPtr.Zero);


            if (miColPtoInter.Count == 0)
            {
                throw new oExInterseccionTaludToTerrenoIsNull("Talud con Bermas - Terreno Natural Superior");
            }
            else if (miColPtoInter.Count == 1)
            {
                return miColPtoInter[0];
            }
            else
            {
                return iPtoHeadTnd.getPtoMasCercano(miColPtoInter);
            }

        }
        /// <summary>
        /// INTERSECCION BERMAS - TERRENO NATURAL DESBROZADO ; CASO ESPECIAL TOLERANCIAS
        /// </summary>
        private Point3d getPointInterseccionTaludBermasToTerrenoNaturalDesbrozadoCasoEspecialTolerancias(Polyline iLwTaludBermas, Polyline iLwTerrenoDesbrozado)
        {

            //HACK JUAN

            //2-Caso Especial, Solución Error 133 --> Problema de Tolerancias
            //Hacemos un Talud Vertical hacia abajo
            Point3d miPtoTaludIni = iLwTaludBermas.StartPoint;
            Point3d miPtoTaludFin = miPtoTaludIni.getFromIncXIncY(0, -1, 0);

            Polyline miTaludVertical = engCadNet.oLw.addLw2d(miPtoTaludIni, miPtoTaludFin, iLwTaludBermas.Layer, null, iLwTaludBermas.Color);

            try
            {
                oInterseccionForward miInterseccion_Talud_Terreno = new oInterseccionForward(miTaludVertical, iLwTerrenoDesbrozado);

                Point3d miPtoOut = miInterseccion_Talud_Terreno.getInterseccion();

                oTools.entidadDelete(miTaludVertical);

                return miPtoOut;
            }
            catch (engCadNet.oExInterseccionNull)
            {
                throw new oExInterseccionTaludToTerrenoIsNull("No se ha podido Determinar la Intersección Bermas-Terreno");
            }
            catch (System.Exception)
            {
                throw;
            }

        }
        /// <summary>
        ///PARTIR TALUD BERMAS
        /// <summary>
        private Polyline getSplitTaludBermas(Polyline iLwTaludBermas, Point3d iPtoIntTnd)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {

                Point3dCollection miCol = new Point3dCollection();
                miCol.Add(iPtoIntTnd);


                DBObjectCollection acDbObjColl = iLwTaludBermas.GetSplitCurves(miCol);

                ObjectId miObjId = ObjectId.Null;

                List<Polyline> miLstLwBreak = new List<Polyline>();

                Polyline miLw;

                foreach (Entity acEnt in acDbObjColl)
                {
                    // Add each offset object
                    miObjId = oTools.entidadAdd(acEnt, mLayer);

                    miLw = (Polyline)tr.GetObject(miObjId, OpenMode.ForWrite);

                    miLstLwBreak.Add(miLw);

                }



                miLstLwBreak[0].Color = oColor.getInstance.morado;

                oTools.entidadDelete(miLstLwBreak[1].ObjectId);

                tr.Commit();

                return miLstLwBreak[0];
            }
        }

        #endregion

        internal class oTaludSeccion
        {
            /// <summary>
            /// Polilinea Talud Sección
            /// </summary>
            public Polyline taludLw { get; set; }
            /// <summary>
            /// Punto Cabeza Talud Terreno Natural Superior
            /// </summary>
            public Point3d ptoHeadTns { get; set; }
            /// <summary>
            /// Punto Cabeza Talud Terreno Natural Desbrozado
            /// </summary>
            public Point3d ptoHeadTnd { get; set; }


            public oTaludSeccion()
            {
                this.taludLw = null;
            }



            public oTaludSeccion(Polyline iLwTalud, Point3d iPtoHeadTerrenoNaturalSuperior, Point3d iPtoHeadTerrenoNaturalDesbrozado)
            {
                this.taludLw = iLwTalud;
                this.ptoHeadTns = iPtoHeadTerrenoNaturalSuperior;
                this.ptoHeadTnd = iPtoHeadTerrenoNaturalDesbrozado;
            }


            public void deleteTalud()
            {
                if (this.taludLw != null)
                {
                    engCadNet.oTools.entidadDelete(this.taludLw);
                    this.taludLw = null;
                    this.ptoHeadTnd = Point3d.Origin;
                    this.ptoHeadTns = Point3d.Origin;
                }

            }

        }
    }
    public abstract class oSeccionEstructura : oSeccionDrawable
    {

        protected oSeccionEjeTrazado mSeccion = null;


        #region "Constructor"

        public oSeccionEstructura()
        {

        }

        public oSeccionEstructura(oSeccionEjeTrazado iSeccionEje)
        {
            this.mSeccion = iSeccionEje;
        }


        #endregion

        /// <summary>
        /// Ancho de la Sección Completa
        /// </summary>
        public abstract double anchoSeccionCompleta { get; }
        /// <summary>
        /// Ancho de la Estructura Puente o Tunel
        /// </summary>
        public abstract double anchoEstructura { get; }
        /// <summary>
        /// Nombre del Bloque de la Sección
        /// </summary>
        public abstract string bloqueNombreConExtesion { get; }
        /// <summary>
        /// Ruta de las Carpetas donde se Guardan los Bloques
        /// </summary>
        public abstract string folderPathBloques { get; }


        public override oSeccionPlantaPk getPlanta()
        {

            Point3d miPtoScEjeIzq = getPtoEjeTrazadoFromScRoadXandLado(mSeccion.pk.Value, (this.anchoSeccionCompleta / 2), eLado.IZQ);
            Point3d miPtoScEjeDer = getPtoEjeTrazadoFromScRoadXandLado(mSeccion.pk.Value, (this.anchoSeccionCompleta / 2), eLado.DER);

            oTramoExcavacionTalud miTaludIzq = new oTramoExcavacionTalud(miPtoScEjeIzq, miPtoScEjeIzq, eExcavacion.acota);
            oTramoExcavacionTalud miTaludDer = new oTramoExcavacionTalud(miPtoScEjeDer, miPtoScEjeDer, eExcavacion.acota);

            oSeccionPlantaPk miPlanta = new oSeccionPlantaPk(this.mSeccion.pk.Value, this.seccionTipo, miTaludIzq, miTaludDer);

            return miPlanta;
        }





    }
    public class oSeccionPuenteSimple : oSeccionEstructura
    {



        public oSeccionPuenteSimple(oSeccionEjeTrazado iSeccionEje)
            : base(iSeccionEje)
        {

        }



        #region "Propiedades"


        public override eRoadSeccion seccionTipo
        {
            get { return eRoadSeccion.puente; }
        }
        public override double anchoSeccionCompleta
        {
            get
            {
                return this.anchoEstructura;
            }
        }
        public override double anchoEstructura
        {
            get
            {
                return this.mSeccion.zonaPuentes.row.anchoTablero;
            }
        }
        public override string bloqueNombreConExtesion
        {
            get
            {
                return this.mSeccion.zonaPuentes.row.dwgName;
            }
        }
        public override string folderPathBloques
        {
            get
            {
                return oTadil.data.Files.folderCadSecPuentes;
            }
        }

        #endregion



        #region "Metodos Publicos"


        public override void draw(Point2d iPtoInsert, string iLayer)
        {

            //Creo la Guitarra
            this.guitarra = new oSeccionGuitarra(iPtoInsert, mSeccion.ptoRoad.Z, this.anchoEstructura, this.anchoEstructura);

            //Represento la Guitarra
            this.guitarra.draw(iLayer);

            //Inserto el Bloque
            engCadNet.oBlock.insertBlockReference(this.bloqueNombreConExtesion, this.folderPathBloques, Point3d.Origin, iLayer, this.guitarra.matrizRoadDerecha);

        }





        public override oSeccionMedicion getMediciones(double iPkIntervaloMetros)
        {

            //Creo la Partida de Medicion
            oMedPuentes miMedicionTunel = new oMedPuentes(mSeccion.zonaPuentes.row.estMat, this.anchoEstructura);

            //Creo la Seccion de Medcion
            oSeccionMedicionPuentes miSeccionMedicionTuneles = new oSeccionMedicionPuentes(mSeccion.zonaPuentes.id, mSeccion.zonaPuentes.nombre, mSeccion.pk.Value);

            miSeccionMedicionTuneles.AddMedicionUnitaria(miMedicionTunel, iPkIntervaloMetros);

            return miSeccionMedicionTuneles;

        }





        #endregion




    }
    public class oSeccionPuenteDoble : oSeccionPuenteSimple
    {

        private double? mAnchoMediana = null;



        public oSeccionPuenteDoble(oSeccionEjeTrazado iSeccionEje, double iAnchoMediana)
            : base(iSeccionEje)
        {
            this.mAnchoMediana = iAnchoMediana;
        }



        public override double anchoSeccionCompleta
        {
            get
            {
                return ((2.0 * this.anchoEstructura) + mAnchoMediana.Value);
            }
        }

        public override void draw(Point2d iPtoInsert, string iLayer)
        {

            double miAnchoGuitarra = 20 + (2.0 * this.anchoEstructura) + this.mAnchoMediana.Value;
            double miAltoGuitarra = this.anchoEstructura;

            //Creo la Guitarra
            this.guitarra = new oSeccionGuitarra(iPtoInsert, mSeccion.ptoRoad.Z, miAnchoGuitarra, miAltoGuitarra);

            //Represento la Guitarra
            this.guitarra.draw(iLayer);


            Point3d miInsertSeccion1 = getInsertPointSeccion1();
            Point3d miInsertSeccion2 = getInsertPointSeccion2();

            engCadNet.oBlock.insertBlockReference(this.bloqueNombreConExtesion, this.folderPathBloques, miInsertSeccion1, iLayer, this.guitarra.matrizRoadDerecha);
            engCadNet.oBlock.insertBlockReference(this.bloqueNombreConExtesion, this.folderPathBloques, miInsertSeccion2, iLayer, this.guitarra.matrizRoadDerecha);

        }

        public override oSeccionMedicion getMediciones(double iPkIntervaloMetros)
        {

            double miNumeroSecciones = 2;

            //Creo la Partida de Medicion
            oMedPuentes miMedicionTunel = new oMedPuentes(mSeccion.zonaPuentes.row.estMat, this.anchoEstructura);

            //Creo la Seccion de Medcion
            oSeccionMedicionPuentes miSeccionMedicionTuneles = new oSeccionMedicionPuentes(mSeccion.zonaPuentes.id, mSeccion.zonaPuentes.nombre, mSeccion.pk.Value);

            miSeccionMedicionTuneles.AddMedicionUnitaria(miMedicionTunel, miNumeroSecciones * iPkIntervaloMetros);

            return miSeccionMedicionTuneles;

        }



        private Point3d getInsertPointSeccion1()
        {

            double miDistanciaDesdeOrigen = -(0.5 * this.anchoEstructura + (0.5 * this.mAnchoMediana.Value));

            Point3d miPto = new Point3d(miDistanciaDesdeOrigen, 0, 0);

            return miPto;
        }
        private Point3d getInsertPointSeccion2()
        {

            double miDistanciaDesdeOrigen = (0.5 * this.anchoEstructura + (0.5 * this.mAnchoMediana.Value));

            Point3d miPto = new Point3d(miDistanciaDesdeOrigen, 0, 0);

            return miPto;
        }

    }
    public class oSeccionTunelSimple : oSeccionEstructura
    {


        public oSeccionTunelSimple(oSeccionEjeTrazado iSeccionEje)
            : base(iSeccionEje)
        {

        }

        #region "Propiedades"

        public override eRoadSeccion seccionTipo
        {
            get { return eRoadSeccion.tunel; }
        }
        public override double anchoSeccionCompleta
        {
            get
            {
                return this.anchoEstructura;
            }
        }
        public override double anchoEstructura
        {
            get
            {
                return this.mSeccion.zonaTuneles.row.ancho;
            }
        }
        public override string bloqueNombreConExtesion
        {
            get
            {
                return this.mSeccion.zonaTuneles.row.dwgName;
            }
        }
        public override string folderPathBloques
        {
            get
            {
                return oTadil.data.Files.folderCadSecTun;
            }
        }

        #endregion


        #region "Metodos Publicos"

        public override void draw(Point2d iPtoInsert, string iLayer)
        {
            //Creo la Guitarra
            guitarra = new oSeccionGuitarra(iPtoInsert, mSeccion.ptoRoad.Z, this.anchoEstructura, this.anchoEstructura);

            //Represento la Guitarra
            this.guitarra.draw(iLayer);

            //Inserto el Bloque
            engCadNet.oBlock.insertBlockReference(this.bloqueNombreConExtesion, this.folderPathBloques, Point3d.Origin, iLayer, this.guitarra.matrizRoadDerecha);

        }

        public override oSeccionMedicion getMediciones(double iPkIntervaloMetros)
        {

            //Partida de Mediciones
            oMedTunel miMedicionTunel = new oMedTunel(mSeccion.zonaTuneles.row.idEstMat, 1);

            //Seccion de Medicion
            oSeccionMedicionTuneles miSeccionMedicionTuneles = new oSeccionMedicionTuneles(mSeccion.zonaTuneles.id, mSeccion.zonaTuneles.nombre, mSeccion.pk.Value);

            miSeccionMedicionTuneles.AddMedicionUnitaria(miMedicionTunel, iPkIntervaloMetros / 1000);

            return miSeccionMedicionTuneles;

        }


        #endregion
    }
    public class oSeccionTunelDoble : oSeccionTunelSimple
    {

        private double? mAnchoMediana = null;


        public oSeccionTunelDoble(oSeccionEjeTrazado iSeccionEje, double iAnchoMediana)
            : base(iSeccionEje)
        {
            mAnchoMediana = iAnchoMediana;
        }


        public override double anchoSeccionCompleta
        {
            get
            {
                return ((2.0 * this.anchoEstructura) + mAnchoMediana.Value);
            }
        }

        public override void draw(Point2d iPtoInsert, string iLayer)
        {


            double miAnchoGuitarra = (2.0 * this.anchoEstructura) + this.mAnchoMediana.Value;
            double miAltoGuitarra = this.anchoEstructura;

            //Creo la Guitarra
            guitarra = new oSeccionGuitarra(iPtoInsert, mSeccion.ptoRoad.Z, miAnchoGuitarra, miAltoGuitarra);

            //Represento la Guitarra
            this.guitarra.draw(iLayer);


            Point3d miInsertSeccion1 = getInsertPointSeccion1();
            Point3d miInsertSeccion2 = getInsertPointSeccion2();


            engCadNet.oBlock.insertBlockReference(this.bloqueNombreConExtesion, this.folderPathBloques, miInsertSeccion1, iLayer, this.guitarra.matrizRoadDerecha);
            engCadNet.oBlock.insertBlockReference(this.bloqueNombreConExtesion, this.folderPathBloques, miInsertSeccion2, iLayer, this.guitarra.matrizRoadDerecha);
        }

        public override oSeccionMedicion getMediciones(double iPkIntervaloMetros)
        {
            //Partida de Mediciones
            oMedTunel miMedicionTunel = new oMedTunel(mSeccion.zonaTuneles.row.idEstMat, 2);

            //Seccion de Medicion
            oSeccionMedicionTuneles miSeccionMedicionTuneles = new oSeccionMedicionTuneles(mSeccion.zonaTuneles.id, mSeccion.zonaTuneles.nombre, mSeccion.pk.Value);

            miSeccionMedicionTuneles.AddMedicionUnitaria(miMedicionTunel, iPkIntervaloMetros / 1000);

            return miSeccionMedicionTuneles;


        }

        private Point3d getInsertPointSeccion1()
        {

            double miDistanciaDesdeOrigen = -(0.5 * this.anchoEstructura + (0.5 * this.mAnchoMediana.Value));

            Point3d miPto = new Point3d(miDistanciaDesdeOrigen, 0, 0);

            return miPto;
        }

        private Point3d getInsertPointSeccion2()
        {

            double miDistanciaDesdeOrigen = (0.5 * this.anchoEstructura + (0.5 * this.mAnchoMediana.Value));

            Point3d miPto = new Point3d(miDistanciaDesdeOrigen, 0, 0);

            return miPto;
        }

    }


}
