using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using engCadNet;

namespace tadLayLogica.logica.EjeBasicoNew
{

    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    using tadLayShare.puntos;
    
    


    public class oLstTramosGanadores
    {

        Dictionary<int, oTramoEjeBasico> mLstTramosGanadores;


        #region "Constructor"

        public oLstTramosGanadores()
        {
            mLstTramosGanadores = new Dictionary<int, oTramoEjeBasico>();
        }
        #endregion
        #region "Propiedades"

        public int idUltimoTramo
        {
            get
            {
                  return (from p in mLstTramosGanadores select p.Value.idTramo).Max();                      
            }
        }



        public bool existeSolucion()
        {
            if (mLstTramosGanadores.Count == null || mLstTramosGanadores.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public oTramoEjeBasico tramoInicial
        {
            get
             {
                 return mLstTramosGanadores[0];
             }
        }


        #endregion



        #region "Metodos"


        /// <summary>
        /// Add Tramo a la Coleccion
        /// </summary>
        public void addTramo (oTramoEjeBasico iTramo)
        {
            mLstTramosGanadores.Add(iTramo.idTramo, iTramo);
        }


        /// <summary>
        /// Add Rango de Tramos a la Colección
        /// </summary>
        public void addTramo(List<oTramoEjeBasico> iLstTramos)
        {
            foreach (var item in iLstTramos)
            {
                mLstTramosGanadores.Add(item.idTramo, item);
            }
        }

        /// <summary>
        /// Update Tramo Entronque
        /// </summary>
        public void updateTramoEntronque(List<oTramoEjeBasico> iLstTramoUpdate)
        {

            oTramoEjeBasico miTramoEntronquePrevio = iLstTramoUpdate[0];
            oTramoEjeBasico miTramoEntronque = iLstTramoUpdate[1];

            //Update Tramo Previo
            mLstTramosGanadores.Remove(miTramoEntronquePrevio.idTramo);
            this.addTramo(miTramoEntronquePrevio);

            //Update Tramo Entronque
            mLstTramosGanadores.Remove(miTramoEntronque.idTramo);        
            this.addTramo(miTramoEntronque);         
        }


        /// <summary>
        /// Draw Eje Basico 3D - Perfil Longitudinal
        /// </summary>
        public Polyline3d drawEjeBasico3D(string iSolucionNombre)
        {


            #region "GestionCapas"
            oTadilLayerEjeBasico3D miLayerEjeBasico3d = new oTadilLayerEjeBasico3D(iSolucionNombre);
            miLayerEjeBasico3d.deleteItems();
            #endregion


            #region "Creo la Polilinea"

            Point3dCollection miLstPuntoEjeBasico = this.getPoint3dCollectionEjeBasico();

            Polyline3d miLw = engCadNet.oLw.addLw3d(miLstPuntoEjeBasico, false, miLayerEjeBasico3d.name);

            #endregion


            #region "AddXdata"

            this.addXdata(miLw);

            #endregion

            return miLw;

        }



        public Polyline drawEjeBasicoPlanta(Polyline3d iEjeBasico3D, string iSolucionNombre)
        {

            #region "GestionCapas"

            oTadilLayerEjeBasico2D miLayer = new oTadilLayerEjeBasico2D(iSolucionNombre);
            miLayer.deleteItems();

            #endregion


            return  engCadNet.oLw.clearPolilinea(iEjeBasico3D, miLayer.name);

        }


        public Polyline drawEjeBasicoPlantaFerrocarril(Polyline3d iEjeBasico3D, string iSolucionNombre)
        {

            #region "GestionCapas"

            oTadilLayerEjeBasico2D miLayer = new oTadilLayerEjeBasico2D(iSolucionNombre);
            miLayer.deleteItems();

            #endregion


            Point3dCollection miLstPuntosClear = new Point3dCollection();
            List<Point3d> miLstPuntosDirty = oLw.getLstPtoFromLw3d(iEjeBasico3D);
            foreach (var point3D in miLstPuntosDirty)
            {
                miLstPuntosClear.Add(point3D);
            }
            return oLw.addLw2d(miLstPuntosClear, false, miLayer.name);

        }



        private Point3dCollection xgetPoint3dCollectionFromLstTramos (List<oTramoEjeBasico> iLstTramos)
        {


            Point3dCollection miPtoCadCollecion = new Point3dCollection();


            miPtoCadCollecion.Add(new Point3d(iLstTramos[0].P1.X,iLstTramos[0].P1.Y,0));


            foreach (var item in iLstTramos)
            {
                miPtoCadCollecion.Add(new Point3d(item.P2.toArray3dZcero()));
            }


            return miPtoCadCollecion;

        }


        /// <summary>
        /// Dibujar los Tramos Ganadores - Depurar
        /// </summary>
        public void draw(string iLayer)
        {
            foreach (var item in mLstTramosGanadores)
            {
                item.Value.drawTramo3D(iLayer);
            }
        }

        #endregion

        #region "Metodos Privados"


        /// <summary>
        /// Listado de Punto del Eje Básico 3D
        /// </summary>
        private List<oP3d> getLstPtoEjeBasico()
        {

            List<oP3d> miLstPtoEjeBasico = new List<oP3d>();

            miLstPtoEjeBasico.Add(mLstTramosGanadores[0].P1);


            foreach (var item in mLstTramosGanadores)
            {
                if (miLstPtoEjeBasico.Last().X != item.Value.P1.X || miLstPtoEjeBasico.Last().Y != item.Value.P1.Y)
                    miLstPtoEjeBasico.Add(item.Value.P1);

                miLstPtoEjeBasico.Add(item.Value.P2);
            }



            return miLstPtoEjeBasico;
        }



        /// <summary>
        /// Listado Puntos CAD Eje Basico 3D
        /// </summary>
        private Point3dCollection getPoint3dCollectionEjeBasico()
        {

            Point3dCollection miCollection = new Point3dCollection();


            foreach (var item in getLstPtoEjeBasico())
            {
                miCollection.Add(new Point3d(item.toArray3d()));
            }

            return miCollection;

        }



        private void addXdata(Polyline3d iLw)
        {

            #region "Estructuras por Tramo"


            //AÑADO EL XDATA CON LAS ESTRUCTURAS POR TRAMO
            string miStrLstEst = string.Empty;

            foreach (var item in mLstTramosGanadores)
            {
                miStrLstEst = miStrLstEst + ";" + item.Value.seccion.getEstructurasTramo().ToString();
            }


            //Elimino el ";"
            miStrLstEst.Remove(0, 1);

            //Xdata de las Estructuras
            oTadilXdata.addXdataEstructurasTramo(iLw, miStrLstEst);


            #endregion

        }

        #endregion

    }

}
