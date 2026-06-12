using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet.entidades;

    using engNet.Extension.Double;

    using engCadNet;

    using tadLayData;

    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.zonaGis;
    using tadLayLogica.logica;
    using tadLayLogica.logica.valoracion;
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayLogica.zonaGis.secciones;


    using tadLayLan;

    using engNet.ClassT;
    using tadLayShare;
    using System.IO;

    public class oSolucion :IDisposable
    {

        private dsApp.tbProyectoRow mProyectoData = null;

        private oRoadDes mRoadDesign = null;
        private oRoadPendientes mRoadPendientes = null;
        private oCoeMinoracionAlturasMaximas mCoeMinoracionAlturasMaximas = null;

        private Polyline mEjeTrazado = null;
        private Polyline3d mEjeBasico3D = null;
        private Polyline mEjeBasico2D = null;
        private Polyline mEjePerfilRasante = null;
        private int? mNumeroSecciones = null;

        private List<oZonaGis> mLstZonasGis = null;
        private List<oSeccionValoracion> mLstSeccionesValoracion = null;


        #region "Constructores"

        public oSolucion(Guid iIdSolucion)
        {
            solucionData = oSingletonDsApp.getInstance.getSolucion(iIdSolucion);
            solucionRoad = oSingletonDsApp.getInstance.getSolucionRoad(iIdSolucion);
            this.numeroSeccionesTotales = null;
            this.numeroSeccionesCalzada = null;
            this.numeroSeccionesPuentes = null;
            this.numeroSeccionesTuneles = null;
        }

        #endregion





        #region "Propiedades"

        public Guid idSolucion
        {
            get
            {
                return solucionData.id;
            }
        }
        public dsApp.tbProyectoRow proyectoData
        {
            get
            {
                if (mProyectoData == null)
                {
                    mProyectoData = oSingletonDsApp.getInstance.proyecto;
                }

                return mProyectoData;
            }

        }
        public dsApp.tbSolucionRow solucionData { get; set; }
        public dsApp.tbSolucionRoadRow solucionRoad { get; set; }

        public oRoadDes roadDesign
        {
            get
            {
                if (mRoadDesign == null)
                {
                    mRoadDesign = oDalTbSolucion.getRoadDesign(this.idSolucion);
                }

                return mRoadDesign;

            }
        }
        public oRoadPendientes roadPendientes
        {
            get
            {
                if (mRoadPendientes == null)
                {
                    mRoadPendientes = oDalTbSolucionPendientes.getPendientesBySolucion(this.idSolucion);
                }

                return mRoadPendientes;
            }

        }
        public oCoeMinoracionAlturasMaximas coeficienteMinoracionAlturasMaximas
        {

            get
            {
                if (mCoeMinoracionAlturasMaximas == null)
                {
                    mCoeMinoracionAlturasMaximas = oDalTbSolucionCoeSensibilidad.getCoeMinoracionAlturasMaximas(this.idSolucion);
                }

                return mCoeMinoracionAlturasMaximas;

            }

        }

        public Polyline3d ejeBasico3D
        {

            get
            {
                if (mEjeBasico3D == null)
                {
                    using (oEntidad<Polyline3d> miPolinea = new oEntidad<Polyline3d>(solucionData.handleEjeBasico3D))
                    {
                        mEjeBasico3D = miPolinea.entidad;
                    }
                }

                return mEjeBasico3D;
            }


        }
        public Polyline ejeBasico2D
        {

            get
            {
                if (mEjeBasico2D == null)
                {
                    using (oEntidad<Polyline> miPolinea = new oEntidad<Polyline>(solucionData.handleEjeBasico2D))
                    {
                        mEjeBasico2D = miPolinea.entidad;
                    }
                }

                return mEjeBasico2D;
            }


        }
        public Polyline ejeTrazado
        {
            get
            {
                if (mEjeTrazado == null)
                {

                    using (Transaction tr = oCadManager.StartTransaction())
                    {

                        long miLong = Convert.ToInt64(solucionData.handleEjeTrazado, 16);
                        Handle miHandle = new Handle(miLong);
                        ObjectId miObjId = oCadManager.thisBase.GetObjectId(false, miHandle,0);

                        mEjeTrazado  = (Polyline) tr.GetObject(miObjId, OpenMode.ForRead);

                        tr.Commit();
                    }

                }

                return mEjeTrazado;
            }
        }
        public Polyline ejePerfilRasante
        {
            get
            {
                if (mEjePerfilRasante == null)
                {

                    using (Transaction tr = oCadManager.StartTransaction())
                    {

                        long miLong = Convert.ToInt64(solucionData.handlePerfil, 16);
                        Handle miHandle = new Handle(miLong);
                        ObjectId miObjId = oCadManager.thisBase.GetObjectId(false, miHandle, 0);

                        mEjePerfilRasante = (Polyline)tr.GetObject(miObjId, OpenMode.ForRead);

                        tr.Commit();
                    }

                    //mEjePerfilRasante = oCadManager.getEntidadRead<Profile>(solucionData.handlePerfilLon);

                    ////using (oEntidad<Profile> miEje = new oEntidad<Profile>(solucionData.handlePerfilLon))
                    ////{
                    ////    mEjePerfilRasante = miEje.entidad;
                    ////}
                }

                return mEjePerfilRasante;
            }



        }

       

        public List<oSeccionValoracion> lstSeccionesValoracion
        {

            get
            {

                if (mLstSeccionesValoracion == null)
                {
                    throw new oExPropertieNullValue("Listado Secciones Valorar");
                }

                return mLstSeccionesValoracion;

            }
        }

        public int? numeroSeccionesTotales {get; private set;}

        public int? numeroSeccionesCalzada { get; private set; }

        public int? numeroSeccionesPuentes {get;private set;}
     
        public int? numeroSeccionesTuneles {get; private set;}
 


        #endregion
        #region "MetodosPublicos"

        private Point3d getPtoFromPk (double iPk)
        {
            EjeDeTrazado.puntosDelEje.EjeTrazado miEje = null;
            if (!this.solucionData.amilia)
            {
                Xrecord miXrecord = engCadNet.oXrecord.getXrecord(this.ejeTrazado.ObjectId, "info");
                miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));
            }
            else
            {
                byte[] datosRecuperados = this.solucionData.EjeTrazado_Amilia;
                using (MemoryStream ms = new MemoryStream(datosRecuperados))
                {
                    // 3. Usamos tu método estático para reconstruir la clase
                    miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(ms);
                }
            }



            
            Point3d miPunto = new Point3d(miEje.getPointAtDist(iPk)[0], miEje.getPointAtDist(iPk)[1], 0);
            return miPunto;
        }


        public static void deleteEntidades (string iSolucionNombre)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                //Obtengo la Lista de Capas con la Solucion
                List<string> miLstCapasBySolucion = oTadil.data.Layer.getLstCapasBySolucion(iSolucionNombre);

                //Borro las Entidades
                oSs.deleteSsByLayerList(miLstCapasBySolucion);


                try
                {
                    oTadil.data.Layer.abanicoTramos.deleteItems();
                    oTadil.data.Layer.abanicoSecciones.deleteItems();
                    oTadil.data.Layer.abanicoTramosGanadores.deleteItems();
                }

                catch (System.Exception ex)
                {
                    oTadil.data.UserInfo.showError(ex);
                }
            }
        }



        /// <summary>
        /// Listado de Secciones por CODIGO
        /// </summary>
        public List<oSeccionValoracion> getLstSeccionesByCode(eGisZonas iCode)
        {

            var miQuery = from p in lstSeccionesValoracion
                          where p.zonaGis.code == iCode
                          select p;

            return miQuery.ToList();

        }

        /// <summary>
        /// Listado de Secciones por GRUPO
        /// </summary>
        public List<oSeccionValoracion> getLstSeccionesByCode(eGisGrupos iGrupo)
        {

            var miQuery = from p in lstSeccionesValoracion
                          where p.zonaGis.grupo == iGrupo
                          select p;

            return miQuery.ToList();

        }

        #endregion
        #region "Metodos Privados"
        /// <summary>
        ///Obtener las Secciones de Valoracion
        /// </summary>
        /// <returns></returns>
        public void getLstSeccionesValoracion()
        {

            List<oSeccionValoracion> miLstSeccionValoracion = new List<oSeccionValoracion>();

            oSeccionValoracion miSeccionValoracion;

            int miIdSeccion = 1;
            
   
            //1-Obtengo las Secciones Design ObraLineal
            oCollectionZonasDesign miCollectionZonasDesign = oDalTbObraLinealSecciones.getCollectionZonasDesignOrderByPkAscending(this.idSolucion,this.getPtoFromPk);


            //2-Creo la Lista de Secciones de Valoraciones para las Zonas de Design
            foreach (oSeccionZonasDesign item in miCollectionZonasDesign)
            {
                miSeccionValoracion = new oSeccionValoracion(miIdSeccion, item.pk,item.pkPto, item.seccionTipo, item.getZonaValoracion());

                miLstSeccionValoracion.Add(miSeccionValoracion);

                miIdSeccion++;
            }


           //3-Relleno la Lista con las secciones de valoraciones AMB-CLI-SOC-PAT
           List<oZonaGis> miLstZonasGisVariables = oFactoryZonaGis.getLstZonasGisVariablesProyecto();

            foreach (oSeccionZonasDesign item in miCollectionZonasDesign)
            {

                foreach (var zonaGis in miLstZonasGisVariables)
                {
                    
                    if (! zonaGis.isZonaNoPaso && zonaGis.isPtoInLwZona(item.pkPto))
                    {
                        miSeccionValoracion = new oSeccionValoracion(miIdSeccion, item.pk, item.pkPto, item.seccionTipo, zonaGis);

                        miLstSeccionValoracion.Add(miSeccionValoracion);

                        miIdSeccion++;
                    }
                }


            }


           
            //Relleno las Variables de las secciones

            this.mLstSeccionesValoracion = miLstSeccionValoracion;
            this.numeroSeccionesTotales = miCollectionZonasDesign.Count;
            this.numeroSeccionesCalzada = miCollectionZonasDesign.numeroSeccionesCalzada();
            this.numeroSeccionesPuentes = miCollectionZonasDesign.numeroSeccionesPuente();
            this.numeroSeccionesTuneles = miCollectionZonasDesign.numeroSeccionesTunel();

        }
        #endregion



     

        public void Dispose()
        {
            if (mProyectoData != null)
            {
                mProyectoData.CancelEdit();
                mProyectoData = null;
            }

            if (mLstSeccionesValoracion != null)
            {
                mLstSeccionesValoracion.Clear();
                mLstSeccionesValoracion = null;
            }

            if (mLstZonasGis != null)
            {
                mLstZonasGis.Clear();
                mLstZonasGis = null;
            }
        }
    }

}
