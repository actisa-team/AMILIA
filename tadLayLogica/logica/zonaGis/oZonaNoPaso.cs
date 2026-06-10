using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace tadLayLogica.zonaGis
{
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;
    using tadLayShare;
    using engNet.ClassT;
    using engNet.eventos;

    using tadLayShare.puntos;

    public class oSingletonZonaNoPaso : IDisposable
    {


        private static oSingletonZonaNoPaso mInstance = null;

        private eEstudioTipo? mEstudioTipo = null;
        private List<Polyline> mLstPolilineasZonasNoPaso = null;


        #region "Constructores"
        private oSingletonZonaNoPaso()
        {

        }


        public static oSingletonZonaNoPaso getInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new oSingletonZonaNoPaso();
                    // oTadilCore.evReset += new EventHandler<oEventArgs<bool>>(oTadilCore_evReset);
                }

                return mInstance;
            }
        }

        //static void oTadilCore_evReset(object sender, oEventArgs<bool> e)
        //{
        //    if (e.Value)
        //    {
        //        oTadilCore.evReset -= new EventHandler<oEventArgs<bool>>(oTadilCore_evReset);
        //        mInstance.Dispose();
        //    }
        //}


        #endregion


        #region "Propiedades"
        /// <summary>
        /// Listado de Polilineas No Paso
        /// </summary>
        public List<Polyline> lstLwZonasNoPaso
        {

            get
            {
                if (mInstance.mLstPolilineasZonasNoPaso == null)
                {
                    mEstudioTipo = oSingletonProyecto.getInstance.estudioTipo;

                    mInstance.mLstPolilineasZonasNoPaso = new List<Polyline>();
                    mInstance.mLstPolilineasZonasNoPaso.AddRange(getLstPolilineasNoPasoPorCapa(oTadil.data.Layer.zonaNoPasoPendiente.name));
                    mInstance.mLstPolilineasZonasNoPaso.AddRange(getLstPolilineasNoPasoPorCapa(oTadil.data.Layer.zonaNoPasoUsuario.name));
                    mInstance.mLstPolilineasZonasNoPaso.AddRange(getLstPolilineasNoPasoPorCapa(oTadil.data.Layer.zonaNoPasoUsuario_Temp.name));

                    if (mEstudioTipo.Value == eEstudioTipo.ESTINF)
                    {
                        mInstance.mLstPolilineasZonasNoPaso.AddRange(getLstPolilineasNoPasoPorZonasGis());
                    }
                }

                return mInstance.mLstPolilineasZonasNoPaso;
            }

        }


        public bool isTramoOnZonaNoPasoPolilinea(Polyline iTramo)
        {

            return this.isTramoOnZonaNoPaso(iTramo);
        }
        #endregion


        #region "Metodos Publicos"

        public bool isTramoOnZonaNoPaso(IP2d iP1, IP2d iP2)
        {
            return this.isTramoOnZonaNoPaso(iP1.X, iP1.Y, iP2.X, iP2.Y);
        }


        public bool isTramoOnZonaNoPaso(double iP1X, double iP1Y, double iP2X, double iP2Y)
        {
            if(mInstance == null)
            {
                mInstance = new oSingletonZonaNoPaso();
            }
            if (mInstance.lstLwZonasNoPaso.Count == 0)
            {
                return false;
            }

            Polyline miPolTramo = new Polyline();
            miPolTramo.AddVertexAt(0, new Point2d(iP1X, iP1Y), 0, 0, 0);
            miPolTramo.AddVertexAt(1, new Point2d(iP2X, iP2Y), 0, 0, 0);
            //Line myLine = new Line(new Point3d(iP1X, iP1Y, 0), new Point3d(iP2X, iP2Y, 0));

            //Creo los Puntos de Interseccion
            Point3dCollection myLstPtoInterseccion = new Point3dCollection();

            ////Creo el Plano de Intersección
            //Plane myPlane = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));

            foreach (Polyline itemLw in mInstance.lstLwZonasNoPaso)
            {
                Polyline miPol = new Polyline();
                for (int i = 0; i < itemLw.NumberOfVertices; i++)
                {
                    miPol.AddVertexAt(i, itemLw.GetPoint2dAt(i), 0, 0, 0);
                }
                miPol.IntersectWith(miPolTramo, Intersect.OnBothOperands, myLstPtoInterseccion, new IntPtr(), new IntPtr());

                if (myLstPtoInterseccion.Count > 0)
                {
                    return true;
                }
                if(oLw.isPtoOnLw(itemLw, new Point3d(iP1X, iP1Y, 0)))
                {
                    return true;
                }
                if (oLw.isPtoOnLw(itemLw, new Point3d(iP2X, iP2Y, 0)))
                {
                    return true;
                }
            }

            return false;
        }

        public bool isTramoOnZonaNoPaso(Polyline iTramo)
        {
            if (mInstance == null)
            {
                mInstance = new oSingletonZonaNoPaso();
            }
            if (mInstance.lstLwZonasNoPaso.Count == 0)
            {
                return false;
            }

            Polyline miPolTramo = iTramo;

            //Creo los Puntos de Interseccion
            Point3dCollection myLstPtoInterseccion = new Point3dCollection();
            bool P1inside = false;
            bool P2inside = false;
            ////Creo el Plano de Intersección
            //Plane myPlane = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));

            foreach (Polyline itemLw in mInstance.lstLwZonasNoPaso)
            {
                Polyline miPol = new Polyline();
                for (int i = 0; i < itemLw.NumberOfVertices; i++)
                {
                    miPol.AddVertexAt(i, itemLw.GetPoint2dAt(i), 0, 0, 0);
                }
                miPol.IntersectWith(miPolTramo, Intersect.OnBothOperands, myLstPtoInterseccion, new IntPtr(), new IntPtr());

                if (myLstPtoInterseccion.Count > 0)
                {
                    return true;
                }

                P1inside = P1inside || oPolygon.isPointInPolygon(itemLw, iTramo.StartPoint.X, iTramo.StartPoint.Y);
                P2inside = P2inside || oPolygon.isPointInPolygon(itemLw, iTramo.EndPoint.X, iTramo.EndPoint.Y);
            }

            return P1inside || P2inside;
        }

        public void zonasNoPasoReset()
        {
            mInstance.mLstPolilineasZonasNoPaso = null;
        }

        public bool isPtoOnZonaNoPaso(double iP1X, double iP1Y)
        {
            if (mInstance == null)
            {
                mInstance = new oSingletonZonaNoPaso();
            }
            if (mInstance.lstLwZonasNoPaso.Count == 0)
            {
                return false;
            }

            foreach (Polyline itemLw in mInstance.lstLwZonasNoPaso)
            {
                
                if (oPolygon.isPointInPolygon(itemLw, iP1X, iP1Y))
                {
                    return true;
                }
            }
            return false;
        }


        #endregion


        // Obtener Listado Polilineas NO PASO POR CAPA (ANALISIS TERRENO - USUARIO)
        private List<Polyline> getLstPolilineasNoPasoPorCapa(string iCapaName)
        {

            List<Polyline> miLstLw = new List<Polyline>();

            Polyline miLw;

            SelectionSet miSsLwZonasNoPaso = engCadNet.oSs.getSsByLayerAndEntidad(iCapaName, eEntidades.LWPOLYLINE);

            if (miSsLwZonasNoPaso != null && miSsLwZonasNoPaso.Count > 0)
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    foreach (SelectedObject item in miSsLwZonasNoPaso)
                    {
                        miLw = (Polyline)tr.GetObject(item.ObjectId, OpenMode.ForRead);

                        miLstLw.Add(miLw);
                    }
                }
            }

            return miLstLw;

        }

        // Obtener Listado Polilineas No Paso ZONAS GIS
        private List<Polyline> getLstPolilineasNoPasoPorZonasGis()
        {

            List<Polyline> miLstLw = new List<Polyline>();

            List<oZonaGis> miLstZonasGis = oFactoryZonaGis.getZonasGisByProyecto();

            foreach (var item in miLstZonasGis)
            {
                if (item.isZonaNoPaso)
                {
                    miLstLw.Add(item.lwZona);
                }
            }

            return miLstLw;
        }

        public void Dispose()
        {
            mInstance.mLstPolilineasZonasNoPaso = null;
            mInstance = null;
        }
    }
}
