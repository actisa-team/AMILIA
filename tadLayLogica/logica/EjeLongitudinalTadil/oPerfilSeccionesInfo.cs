using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeLongitudinalTadil
{

    //CAD
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;



    using tadLayShare.puntos;
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayLogica.estudioTipo;
    using EjeDeTrazado.puntosDelEje;
    using PerfilLongitudinal;
    using tadLayLogica.logica.Entidades;
    using tadLayData;
    using tadLayLogica.datos.BaseDatos;

    public class oPerfilSeccionesInfo
    {

        private List<oSeccionEjeTrazado> mLstSecciones;
        //private Polyline mEjeTrazado;
        private EjeTrazado mEjeTrazadoTadil;
        //private Polyline mEjePerfil;
        private Alzado mAlzado;
        //private cv.Profile mPerfilCarretera;
        private oEstudioCarretera mEstudioCarretera;
        Func<double[], double> iMDT_Abanico_Punto;

        public oPerfilSeccionesInfo(Polyline iEjeTrazado, Polyline iPerfilCarretera, oEstudioCarretera iEstudioCarretera, Func<double[], double> MDT_Abanico_Punto)
        {

            Xrecord miXrecord = engCadNet.oXrecord.getXrecord(iEjeTrazado.ObjectId, "info");
            EjeDeTrazado.puntosDelEje.EjeTrazado miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));
            mEjeTrazadoTadil = miEje;


            Xrecord miXrecordAlzado = engCadNet.oXrecord.getXrecord(iPerfilCarretera.ObjectId, "info");
            Alzado miEjeAlzado = Alzado.recuperaAlzado(engCadNet.oXrecord.getStream(miXrecordAlzado));
            mAlzado = miEjeAlzado;

            //this.mEjeTrazado = iEjeTrazado;
            //this.mPerfilCarretera = iPerfilCarretera;


            mEstudioCarretera = iEstudioCarretera;
            iMDT_Abanico_Punto = MDT_Abanico_Punto;
        }

        public oPerfilSeccionesInfo(EjeTrazado iEjeTrazadoTadil, Alzado iAlzado, oEstudioCarretera iEstudioCarretera, Func<double[], double> MDT_Abanico_Punto)
        {
            mEjeTrazadoTadil = iEjeTrazadoTadil;
            mAlzado = iAlzado;
            mEstudioCarretera = iEstudioCarretera;
            iMDT_Abanico_Punto = MDT_Abanico_Punto;
        }



        #region "Metodos Publicos"


        public List<oEstructura> getPerfilSeccionesInfo(double iSeccionIntervalo)
        {

            List<oEstructura> miLstSecciones = new List<oEstructura>();

            oEstructura miSeccionPk;

            double miPk;


            for (miPk = 0; miPk < mEjeTrazadoTadil.Length; )
            {

                Cambiar_EstudioCarretera(miPk);
                miSeccionPk = this.getSeccion(miPk);

                miLstSecciones.Add(miSeccionPk);

                miPk = miPk + iSeccionIntervalo;
            }


            //Calculo la Última Seccion
            double miSeccionUltimaLongitud = (miPk - iSeccionIntervalo) - mEjeTrazadoTadil.Length;

            if (miSeccionUltimaLongitud > 0)
            {
                Cambiar_EstudioCarretera(miPk);
                miSeccionPk = this.getSeccion(miPk);

                miLstSecciones.Add(miSeccionPk);
            }

            return miLstSecciones;
        }


       


        #endregion


        #region "Metodos Privados"
        /// <summary>
        /// Sección Perfil Longitudinal
        /// </summary>
        private oEstructura getSeccion(double iPk)
        {

            oEstructura miSeccionPerfilInfoByPk;

            bool miHasIncumplimiento;
            string miInfoIncumplimiento;

            oP3d miPtoRoad = this.getPtoFromPk(iPk);
            double[] miPunto = mEjeTrazadoTadil.getPointAtDist(iPk);
            iMDT_Abanico_Punto(miPunto);
            double miZterreno = this.getZterreno(iPk);
            double miZroad = this.getElevationAt(iPk);

            miSeccionPerfilInfoByPk = mEstudioCarretera.getPerfilLonSeccionInfo(iPk, miPtoRoad, miZterreno, miZroad, out miHasIncumplimiento, out miInfoIncumplimiento);

            return miSeccionPerfilInfoByPk;

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
        private oP3d getPtoFromPk(double iPk)
        {

            double[] miPunto = mEjeTrazadoTadil.getPointAtDist(iPk);

            Point3d miPto = new Point3d(miPunto[0], miPunto[1], 0);

            double miPtoZ = this.getElevationAt(iPk);

            return new oP3d(miPto.X, miPto.Y, miPtoZ);
        }
        /// <summary>
        /// Z Terreno
        /// </summary>
        private double getZterreno(double iPk)
        {

            double[] miPunto = mEjeTrazadoTadil.getPointAtDist(iPk);

            Point3d miPto = new Point3d(miPunto[0], miPunto[1], 0);

            return oSingletonTerreno.getInstance.getZFromXY(miPto.X, miPto.Y).Value;
        }
        #endregion


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
            if (mEstudioCarretera is oEstudioInformativoCarretera infoEstudio)
            {
                coe = infoEstudio.mCoeMinAlturasMaximas;
            }
            Guid? id = null;
            if (estaDentro)
            {
                id = ObtenerGuidSeccionDesdeCapa(capaPolilinea);
            }

            if (estaDentro && id.HasValue)
            {
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

                mEstudioCarretera = new oEstudioInformativoCarretera(tipo, id.Value, coe, oSingletonProyecto.getInstance.SeccionesVinculadas);

            }
            else
            {
                mEstudioCarretera = new oEstudioInformativoCarretera(oSingletonProyecto.getInstance.secRoadTipo, oSingletonProyecto.getInstance.seccionCalzadaId,
                    coe, oSingletonProyecto.getInstance.SeccionesVinculadas);
            }
        }

        #endregion

    }
}
