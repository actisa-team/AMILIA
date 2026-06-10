using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad.EjeRasante
{
    using engCadNet;
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using cv = Autodesk.Civil.Land.DatabaseServices;
    using Autodesk.Civil;


    using tadLayShare.EjeTJ;
    using tadLayShare.EjeTJ.Vertice;
    using tadLayShare.EjeTJ.puntos;
    using tadLayShare;
    
    public  class oEjeTadilRasante : oEjeObj<oAcuerdo,oTramoAcuerdos>
      
    {

        //Variables de Entrada
        private string mSolName;
        private string mTerrenoName;
        private oRoadDes mRoadDes;
        private oRoadGeo mRoadGeo;
        private cv.Alignment mEjeTrazado;


        //Variable Obtenidas
        private cv.Profile mPerfilTerreno;
        private cv.Profile mPerfilRasante;
        private cv.ProfileView mPerfilLongitudinal;



        public oEjeTadilRasante(string iTerreno,
                                string iEjeName,
                                oRoadDes iRoadDes,
                                oRoadGeo iRoadGeo,
                                Dictionary<int, oAcuerdo> iListAcuerdos,
                                Dictionary<int, eEstructura> iListEstructuras,
                                cv.Alignment iEjeTrazado,
                                double? iPendienteSalidaPorCiento,
                                double? iPendienteLlegadaPorCiento
                                
            )
                               

           :base(iEjeName,iListAcuerdos)
        
        {

            //Cargo las Variables
            mSolName = iEjeName;
            mTerrenoName = iTerreno;
            mRoadDes = iRoadDes;
            mRoadGeo = iRoadGeo;
            mEjeTrazado = iEjeTrazado;

           

            //SetUp
            this.firstVertice.IsZKon = true;
            this.endVertice.IsZKon = true;


            if (iPendienteSalidaPorCiento != null)
            {
                this.secondVertice.position.Z = oTrigo.getP2zFromP1andLon(firstVertice.position.Z, iPendienteSalidaPorCiento, firstSegmento.lon2d);
                this.secondVertice.IsZKon = true;
            }

            if (iPendienteLlegadaPorCiento != null)
            {
                preEndVertice.position.Z = oTrigo.getP2zFromP1andLon(endVertice.position.Z, -1 * iPendienteLlegadaPorCiento, endSegmento.lon2d);
                preEndVertice.IsZKon = true;          
            }


            //Cargo las Estructuras a los Tramos
            for (int i = 0; i < iListEstructuras.Count; i++)
            {
                segmentos[i].estructura = iListEstructuras[i];
                segmentos[i].pendMaxPorCiento = mRoadGeo.getPendMaxByTramoPorCiento(segmentos[i].estructura);
                segmentos[i].pendMinPorCiento = mRoadGeo.getPendMinByTramoPorCiento(segmentos[i].estructura);
            }


            
            //Funciones delegadas Vertices
            oAcuerdo.mFunGetAcuerdo = getAcuerdo;
            oAcuerdo.mFunGetIncPend = getIncPendiente;
            oAcuerdo.mFunGetKvVisibilidad = getKvVisibilidad;
            oAcuerdo.mFunGetKvSolape = getKvSolape;
            oAcuerdo.mFunGetKvDesign = getKvDesign;
            oAcuerdo.mFunGetViabilidad = getViabilidadAcuerdo;
            oAcuerdo.mFunGetZTerreno = getZterreno;

            //Funciones delegadas Tramos
            oTramoAcuerdos.verticesNum = segmentosCount - 1; 
            oTramoAcuerdos.funGetIsViable = getIsTramoViable;
            oTramoAcuerdos.funGetZTerreno = getZterreno;
            oTramoAcuerdos.funGetTramo = getTramo;

            oTramoAcuerdos.mRoadGeo = mRoadGeo;
              
           
            //Creo el Perfil Terreno
           // mPerfilTerreno = getPerfilTerreno(iTerreno, iEjeTrazado);
            //Creo el Perfil Rasante
            //mPerfilRasante = getPerfilRasante(iEjeTrazado);
             //Actualizo e Eje Tadil
            // updateEjeTadilFromEjeRasante();




        
        }









        #region "Propiedades"

        /// <summary>
        /// Eje Tiene Tramos Incumplimientos
        /// </summary>
        public bool hasEjeTramosIncumplen
        {
            get
            {


                if (lstTramosIncumplen.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Listado con los Incumlimientos
        /// </summary>
        public List<oTramoAcuerdos> lstTramosIncumplen
        {
            get
            {
                var myQueryKO = from p in segmentos.Values
                                where p.hasIncumplimientos == true
                                orderby p.id ascending
                                select p;

                return myQueryKO.ToList<oTramoAcuerdos>();
            }
        }


        /// <summary>
        /// Listado con las Secciones Incumplen
        /// </summary>
        public List<oSeccion> lstSeccionesIncumplen
        {

            get
            {
                List<oSeccion> myLstSecKO = new List<oSeccion>();

                foreach (oTramoAcuerdos myTramoKO in lstTramosIncumplen)
                {
                    myLstSecKO.AddRange(myTramoKO.SeccionesIncumple);
                }


                return myLstSecKO; 
            }
        
        
        
        }


        public bool hasEjeVerticesIncumple
        {
            get
            {
                if (lstVerticesIncumple.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                
                }     
            }    
        }


        /// <summary>
        /// Listado de Vertices que Incumplen
        /// </summary>
        public List<oAcuerdo> lstVerticesIncumple
        {

            get
            {
                var myQuery = from p in vertices.Values
                              where Math.Abs(p.position.Z.Value - p.Zterreno.Value) > mRoadGeo.terraplenDesmonteMaxProyecto
                              orderby p.id ascending
                              select p;

                return myQuery.ToList<oAcuerdo>();
                     
            }
  
        }


        /// <summary>
        /// Eje Tiene Incumplimientos Viables a Tratar
        /// </summary>
        private bool hasEjeIncumplimientosViables
        {
            get
            {
                
                
                if (lstTramosIncumplenViables.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }                     
            }  
        }
        /// <summary>
        /// Listado con los Incumlimientos Viables
        /// </summary>
        private List<oTramoAcuerdos> lstTramosIncumplenViables
        {
            get
            {
                var myQueryKO = from p in segmentos.Values
                                where p.hasIncumplimientosViables == true
                                orderby p.id ascending
                                select p;

                return myQueryKO.ToList<oTramoAcuerdos>();
            }
        }
        /// <summary>
        /// Obtener la Z del Terreno dado un PK
        /// </summary>
        public double getZterreno(double iPk)
        {

            if (mPerfilTerreno == null)
            {
                throw new Exception("Perfil Terreno es Nulo");
            }
            else
            {
                return mPerfilTerreno.ElevationAt(iPk);
            }
        }
        /// <summary>
        /// Obtener el Acuerdo en Vertice ; BASE CERO
        /// </summary>
        public eAcuertoTipo getAcuerdo(int iIdVertice)
        {


            if (nexSeg(iIdVertice).pendientePorCiento > preSeg(iIdVertice).pendientePorCiento)
            {
                return eAcuertoTipo.concavo;
            }
            else if (nexSeg(iIdVertice).pendientePorCiento < preSeg(iIdVertice).pendientePorCiento)
            {
                return eAcuertoTipo.convexo;
            }
            else
            {
                return eAcuertoTipo.plano;
            }

        }
        /// <summary>
        /// Obtener el Incremento de Pendiente
        /// </summary>
        public double? getIncPendiente(int iIdVertice)
        {

            if (iIdVertice == 0)
            {
                return null;
            }
            else if (this.isEndVertice(iIdVertice))
            {
               return null;
            }
            else
            {
            double miPenNext = nexSeg(iIdVertice).pendientePorUno.Value;
            double miPenPrev = preSeg(iIdVertice).pendientePorUno.Value;
            return Math.Abs(miPenNext - miPenPrev);
            }
        }
        /// <summary>
        /// Kv de Visibilidad
        /// </summary>
        public double? getKvVisibilidad(int iIdVertice)
        {

            if (iIdVertice == 0)
            {
                return null;
            }
            else if (this.isEndVertice(iIdVertice))
            {
                return null;
            }
            else
            {
                return mRoadDes.Vp / getIncPendiente(iIdVertice);
            }
            
            
            
        }
        /// <summary>
        /// Kv de NO Solape
        /// </summary>
        public double? getKvSolape(int iIdVertice)
        {

            if (iIdVertice == 0)
            {
                return null;
            }
            else if (this.isEndVertice(iIdVertice))
            {
                return null;
            }
            else
            {
                return (3 * mRoadDes.Rp) / getIncPendiente(iIdVertice);
            }                        
        }
        /// <summary>
        /// Kv de Diseño
        /// </summary>
        public double? getKvDesign(int iIdVertice)
        {

            if (iIdVertice == 0)
            {
                return null;
            }
            else if (this.isEndVertice(iIdVertice))
            {
                return null;
            }
            else
            {
                double? myKvDesign = null;

                double myKvVisibilidad = getKvVisibilidad(iIdVertice).Value;
                double myKvNoSolape = getKvSolape(iIdVertice).Value;

                eAcuertoTipo myAcuerdo = getAcuerdo(iIdVertice);

                if (myAcuerdo == eAcuertoTipo.concavo)
                {
                    if (myKvVisibilidad < mRoadDes.kv.KvConcavo)
                    {
                        myKvDesign = mRoadDes.kv.KvConcavo;
                    }
                    else
                    {
                        myKvDesign = myKvVisibilidad;
                    }
                }
                else if (myAcuerdo == eAcuertoTipo.convexo)
                {
                    if (myKvVisibilidad < mRoadDes.kv.kvConvexo)
                    {
                        myKvDesign = mRoadDes.kv.kvConvexo;
                    }
                    else
                    {
                        myKvDesign = myKvVisibilidad;
                    }

                }
                else if (myAcuerdo == eAcuertoTipo.plano)
                {
                    myKvDesign = 100000;

                }
                else
                {
                    throw new NotImplementedException(string.Format("Enum {0} No Implementada", myAcuerdo.ToString()));

                }


                //Comparo el Kv con el de No Solape

                if (myKvDesign > myKvNoSolape)
                {
                    return myKvNoSolape;
                }
                else
                {
                    return myKvDesign.Value;
                }
            }
        }
        /// <summary>
        /// Viabiliadad de los Vertices
        /// </summary>
        public oViabilidad getViabilidadAcuerdo(int iIdVertice)
        {

            oViabilidad myViabilidad = new oViabilidad();

            double? myIncPendiente = null;
            double? myB1 = null;
            double? myB2 = null;
            double? myB1MenosUno = null;
            double? myB2MenosUno = null;
            double? myB1MasUno = null;
            double? myB2MasUno = null;

            //Primer Vertice
            if (iIdVertice == 0)
            {

                myIncPendiente = (firstSegmento.lon2d.Value *  firstSegmento.pendMaxPorUno);   // mRoadGeo.getPendMaxProyectoTramo(firstSegmento.estructura));


                myB1 = vertices[iIdVertice].Zterreno.Value + mRoadGeo.terraplenDesmonteMaxProyecto;
                myB2 = vertices[iIdVertice].Zterreno.Value - mRoadGeo.terraplenDesmonteMaxProyecto;

                myViabilidad.B1 = myB1.Value;
                myViabilidad.B2 = myB2.Value;

                myViabilidad.C1 = myB1.Value;
                myViabilidad.C2 = myB2.Value;

                myViabilidad.D1 = myB1.Value + myIncPendiente;
                myViabilidad.D2 = myB2.Value - myIncPendiente;


                return myViabilidad;

            }
            // Ultimo Vertices
            else if (this.isEndVertice(iIdVertice))
            {

                myIncPendiente = (endSegmento.lon2d.Value *   endSegmento.pendMaxPorUno) ; //mRoadGeo.getPendMaxProyectoTramo(endSegmento.estructura));

                myB1 = vertices[iIdVertice].Zterreno.Value + mRoadGeo.terraplenDesmonteMaxProyecto;
                myB2 = vertices[iIdVertice].Zterreno.Value - mRoadGeo.terraplenDesmonteMaxProyecto;

                myB1MenosUno = vertices[iIdVertice - 1].Zterreno.Value + mRoadGeo.terraplenDesmonteMaxProyecto;
                myB2MenosUno = vertices[iIdVertice - 1].Zterreno.Value - mRoadGeo.terraplenDesmonteMaxProyecto;


                myViabilidad.B1 = myB1;
                myViabilidad.B2 = myB2;

                myViabilidad.C1 = myB1MenosUno + myIncPendiente;
                myViabilidad.C2 = myB2MenosUno - myIncPendiente;

                myViabilidad.D1 = myB1;
                myViabilidad.D2 = myB2;

                return myViabilidad;


            }
            else
            {

                myIncPendiente = (segmentos[iIdVertice].lon2d.Value *  segmentos[iIdVertice].pendMaxPorUno) ; //mRoadGeo.getPendMaxProyectoTramo(segmentos[iIdVertice].estructura));

                myB1 = vertices[iIdVertice].Zterreno.Value + mRoadGeo.terraplenDesmonteMaxProyecto;
                myB2 = vertices[iIdVertice].Zterreno.Value - mRoadGeo.terraplenDesmonteMaxProyecto;

                myB1MenosUno = vertices[iIdVertice - 1].Zterreno.Value + mRoadGeo.terraplenDesmonteMaxProyecto;
                myB2MenosUno = vertices[iIdVertice - 1].Zterreno.Value - mRoadGeo.terraplenDesmonteMaxProyecto;

                myB1MasUno = vertices[iIdVertice + 1].Zterreno.Value + mRoadGeo.terraplenDesmonteMaxProyecto;
                myB2MasUno = vertices[iIdVertice + 1].Zterreno.Value - mRoadGeo.terraplenDesmonteMaxProyecto;

                myViabilidad.B1 = myB1;
                myViabilidad.B2 = myB2;

                myViabilidad.C1 = myB1MenosUno + myIncPendiente;
                myViabilidad.C2 = myB2MenosUno - myIncPendiente;

                myViabilidad.D1 = myB1MasUno + myIncPendiente;
                myViabilidad.D2 = myB2MasUno - myIncPendiente;

                return myViabilidad;

            }









        }
        /// <summary>
        /// Perfil Estilo
        /// </summary>
        private ObjectId perfilEstilo
        {
            get
            {
                return oCadManager.thisDoc.Styles.ProfileStyles[0];
            }
        }
        /// <summary>
        /// Perfil Estilo Etiquetas
        /// </summary>
        private ObjectId perfilEstiloEtiquetas
        {
            get
            {
                return oCadManager.thisDoc.Styles.LabelSetStyles.ProfileLabelSetStyles[0];
            }
        }
        /// <summary>
        /// Es el Tramo Viable
        /// </summary>
        public bool getIsTramoViable(int iIdTramo)
        {

            if (vertices[iIdTramo].viabilidad.isViable && vertices[iIdTramo + 1].viabilidad.isViable)
            {

                return true;
            }
            else
            {
                return false;
            
            }

        
        
        
        }

        /// <summary>
        /// Obtener un Tramo por ID
        /// </summary>
        public oTramoAcuerdos getTramo(int iIdTramo)
        {
            return segmentos[iIdTramo];
        }


#endregion

        #region "Metodos"

        public void createPerfilTerreno()
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    string myLayer = string.Format("_Tadil_Sol_{0}_PerfilTerreno", mSolName);

                    //Creo o Reseteo la Capa
                    oLayer.createOrDeleteItems(myLayer, 2);

                    //Layer Id
                    ObjectId layerId = oLayer.getLayerObjId(myLayer);

                    //Obtengo el Id de la Superficie
                    ObjectId surfaceId = engCadNet.cv3d.oSurface.getSurfaceByName(mTerrenoName).ObjectId;


                    //Creo la Perfil del Terreno
                    ObjectId profileTerrenoId = cv.Profile.CreateFromSurface(mSolName + "_Terreno", mEjeTrazado.ObjectId, surfaceId, layerId, perfilEstilo, perfilEstiloEtiquetas);


                    mPerfilTerreno = (cv.Profile)tr.GetObject(profileTerrenoId, OpenMode.ForRead);

                    tr.Commit();

             
                }

            }

        }


        public void createPerfilRasanteOriginal()
        {
            createRasante("_Tadil_Sol_{0}_PerfilRasanteOriginal", "_RasanteOriginal",1);
        
        }

        public void createPerfilRasante()
        {

            createRasante("_Tadil_Sol_{0}_PerfilRasante", "_Rasante",3);
        }



        private void createRasante(string iFormatLayer, string iName, short iColor)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    string myLayer = string.Format(iFormatLayer, mSolName);

                    //Creo o Reseteo la Capa
                    oLayer.createOrDeleteItems(myLayer, iColor);

                    //Layer Id
                    ObjectId layerId = oLayer.getLayerObjId(myLayer);


                    //Creo Perfil Rasante
                    ObjectId profileRasanteId = cv.Profile.CreateByLayout(mSolName +iName, mEjeTrazado.ObjectId, layerId, perfilEstilo, perfilEstiloEtiquetas);

                  


                    // Now add the entities that define the profile.
                     mPerfilRasante = tr.GetObject(profileRasanteId, OpenMode.ForRead) as cv.Profile;


                    //Creo las Entidades
                    Point2d startPoint;
                    Point2d endPoint;
                    cv.ProfilePVI myPvi;


                    //Recorro los Vertices Intermedios
                    for (int i = 1; i < vertices.Count - 1; i++)
                    {

                        startPoint = new Point2d(segmentos[i - 1].Vi.position.X.Value,segmentos[i - 1].Vi.position.Z.Value);
                        endPoint = new Point2d(segmentos[i - 1].Vj.position.X.Value, segmentos[i - 1].Vj.position.Z.Value);

                        mPerfilRasante.Entities.AddFixedTangent(startPoint, endPoint);

                        startPoint = new Point2d(segmentos[i].Vi.position.X.Value, segmentos[i].Vi.position.Z.Value);
                        endPoint = new Point2d(segmentos[i].Vj.position.X.Value, segmentos[i].Vj.position.Z.Value);

                        mPerfilRasante.Entities.AddFixedTangent(startPoint, endPoint);

                        myPvi = mPerfilRasante.PVIs.GetPVIAt(startPoint.X, startPoint.Y);

                        mPerfilRasante.Entities.AddFreeCircularCurveByPVIAndRadius(myPvi,vertices[i].KvDesign.Value);
                    }

                    tr.Commit();
       
                }
            }


        

        

        }


        public string checkPerfil()
        {

            double myLonDicre = mRoadGeo.pTramoLonDiscre;
            double myZterreno;
            double myZrasante;
            double myDesfase;

            List<oAcuerdo> myLstVerKO = new List<oAcuerdo>();
            List<oTramoAcuerdos> myLstSegKO = new List<oTramoAcuerdos>();
            List<oTramoAcuerdos> myLstPkKO = new List<oTramoAcuerdos>();


            #region "Check Acuerdos"

            for (int i = 0; i < vertices.Count; i++)
            {

                myZrasante = vertices[i].position.Z.Value;

                myZterreno = vertices[i].Zterreno.Value;

                myDesfase = Math.Abs(myZterreno - myZrasante);

                if (myDesfase > mRoadGeo.terraplenDesmonteMaxProyecto)
                {
                    myLstVerKO.Add(vertices[i]);
                }
            }

            #endregion


            #region "Check Perfil Entero"




            #endregion


            #region "Informe"

            StringBuilder myInf = new StringBuilder("Perfil Longitudinal; Incumplimientos\n");


            if (myLstVerKO.Count > 0)
            {
                myInf.Append("Vertices\n");
                myInf.Append("---------\n");

                foreach (oVer myVer in myLstVerKO)
                {
                    myInf.Append(myVer.ToString() + "\n");
                }


            }

            if (myLstSegKO.Count > 0)
            {
                myInf.Append("Segmentos\n");
                myInf.Append("---------\n");

                foreach (oTramoAcuerdos mySeg in myLstSegKO)
                {
                    myInf.Append(mySeg.ToString() + "\n");
                }


            }

            if (myLstPkKO.Count > 0)
            {
                myInf.Append("Pk\n");
                myInf.Append("--\n");

                foreach (oTramoAcuerdos mySegKO in myLstPkKO)
                {
                    myInf.Append(mySegKO.ToString() + "\n");
                }

            }




            if (myLstVerKO.Count > 0 | myLstSegKO.Count > 0 | myLstPkKO.Count > 0)
            {
                return myInf.ToString();
            }
            else
            {
                return "El Perfil Longitudinal es Valido";
            }


            #endregion



        }
        public void createPerfilLongitudinal(oP3d iPointInsert)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    //Estilos
                    ObjectId pfrVBSStyleId = oCadManager.thisDoc.Styles.ProfileViewBandSetStyles[0];
                    // If this doesn't exist, get the first style in the collection
                    if (pfrVBSStyleId == null) pfrVBSStyleId = oCadManager.thisDoc.Styles.ProfileViewBandSetStyles[0];


                    //Borro el Perfil Longitudinal
                    if (mEjeTrazado.GetProfileViewIds().Count > 0)
                    {
                        engCadNet.oTools.deleteEntidad(mEjeTrazado.GetProfileViewIds()[0].ConvertToRedirectedId());
                    }

                    Point3d myPto = new Point3d(iPointInsert.toArray3d());

                    ObjectId myProfileViewId = cv.ProfileView.Create(oCadManager.thisDoc, mSolName + "PerfilLongitudinal", pfrVBSStyleId, mEjeTrazado.ObjectId, myPto);

                    mPerfilLongitudinal = (cv.ProfileView)tr.GetObject(myProfileViewId, OpenMode.ForRead);

                    tr.Commit();

                }
            }


        }

        /// <summary>
        /// Actualizo el EjeTadil, al modificarse las Z de los acuerdos.
        /// </summary>
        private void updateEjeTadilFromEjeRasante()
        {

            cv.ProfilePVICollection myPviCol = mPerfilRasante.PVIs;
            cv.ProfilePVI myPvi;

            for (int i = 1; i < myPviCol.Count - 1; i++)
            {
                myPvi = myPviCol[i];

                vertices[i].position.Z = myPvi.Elevation;
            }

        }

        #endregion

        /// <summary>
        /// Obtener Incumplimientos
        /// </summary>
        public void getIncumplimientosPerfil()
        {

            foreach (oTramoAcuerdos iTramo in segmentos.Values)
            {
                iTramo.getIncumplimientoTramo();
            }
        }

        /// <summary>
        /// Secuencia de Comprobacion del Eje
        /// </summary>
        public List<int> idSecuenciaCheck
        {
            get
            {

                int mitramosNum = segmentosCount;
                int mitramoInt = mitramosNum / 2;
                int mitramoPre = mitramoInt;
                int mitramoNex = mitramoInt;


                List<int> miSecuencia = new List<int>();

                miSecuencia.Add(mitramoInt);

                while (mitramoPre > 0 && mitramoNex < mitramosNum)
                {

                    mitramoPre--;
                    mitramoNex++;

                    if (mitramoPre >= 0)
                    {
                        miSecuencia.Add(mitramoPre);
                    }

                    if (mitramoNex < mitramosNum)
                    {
                        miSecuencia.Add(mitramoNex);
                    }

                }


                return miSecuencia;
            
            }
        
        
        
        }



        /// <summary>
        /// Obtener Viabilidad de los Vertices del Eje
        /// </summary>
        private void getViabilidadEje()
        {

            for (int i = 0; i < verticesCount; i++)
            {
                getViabilidadAcuerdo(i);
            }
        
        
        }


        /// <summary>
        /// Obtener los Z max y minimos por Vertice
        /// </summary>
        private void getZMaxMinEje()
        {


            Dictionary<int, oZmaxMinAjuste> myDicVerticesAjustaMaxMin = new Dictionary<int, oZmaxMinAjuste>();
       
            oZmaxMinAjuste myZajuste ; 
              
            //1 Obtengo Zterreno, Viabilidad, ZTerraplen, ZDesmonte
            for (int i = 0; i < verticesCount; i++)
			{			     
                myZajuste = new oZmaxMinAjuste(vertices[i].Zterreno.Value,vertices[i].viabilidad.isViable,vertices[i].IsZKon.Value);
                myDicVerticesAjustaMaxMin.Add(i, myZajuste);
			}


        

            //Avance Positivo
            myDicVerticesAjustaMaxMin[0].ZAvancePosMax = myDicVerticesAjustaMaxMin[0].zTerreno;
            myDicVerticesAjustaMaxMin[0].ZAvancePosMin = myDicVerticesAjustaMaxMin[0].zTerreno;

            for (int i = 0; i < verticesCount-1; i++)
			{

                if (myDicVerticesAjustaMaxMin[i + 1].isZK.Value)
                {
                    myDicVerticesAjustaMaxMin[i + 1].ZAvancePosMax = myDicVerticesAjustaMaxMin[i + 1].zTerreno;
                    myDicVerticesAjustaMaxMin[i + 1].ZAvancePosMin = myDicVerticesAjustaMaxMin[i + 1].zTerreno;
                }
                else
                {
                    
                        //Obtengo los Valores Máximos y Minimos de Pendiente
                        myDicVerticesAjustaMaxMin[i + 1].zAvancePosPendMax = myDicVerticesAjustaMaxMin[i].ZAvancePosMax + (segmentos[i].lon2d * segmentos[i].pendMaxPorUno);
                        myDicVerticesAjustaMaxMin[i + 1].zAvancePosPendMin = myDicVerticesAjustaMaxMin[i].ZAvancePosMin - (segmentos[i].lon2d * segmentos[i].pendMaxPorUno);

                        myDicVerticesAjustaMaxMin[i + 1].zAvancePosTerraplenMax = myDicVerticesAjustaMaxMin[i + 1].zTerreno + mRoadGeo.terraplenDesmonteMaxProyecto;
                        myDicVerticesAjustaMaxMin[i + 1].zAvancePosDesmonteMin = myDicVerticesAjustaMaxMin[i + 1].zTerreno - mRoadGeo.terraplenDesmonteMaxProyecto;
                       
                    
                    //Obtengo el Valor Min del Maximo y Maximo del Minimo

                        myDicVerticesAjustaMaxMin[i + 1].ZAvancePosMax = myDicVerticesAjustaMaxMin[i + 1].getAvancePosZmaxMin;
                        myDicVerticesAjustaMaxMin[i + 1].ZAvancePosMin = myDicVerticesAjustaMaxMin[i + 1].getAvancePosZminMax;
  
                }
			}


            //Avance Negativo

            myDicVerticesAjustaMaxMin[verticesCount-1].ZAvanceNegMax = myDicVerticesAjustaMaxMin[verticesCount-1].zTerreno;
            myDicVerticesAjustaMaxMin[verticesCount-1].ZAvanceNegMin = myDicVerticesAjustaMaxMin[verticesCount-1].zTerreno;

            

            for (int j = verticesCount-1 ; j > 0; j--)
            {

                if (myDicVerticesAjustaMaxMin[j - 1].isZK.Value)
                {
                    myDicVerticesAjustaMaxMin[j - 1].ZAvanceNegMax = myDicVerticesAjustaMaxMin[j - 1].zTerreno;
                    myDicVerticesAjustaMaxMin[j - 1].ZAvanceNegMin = myDicVerticesAjustaMaxMin[j - 1].zTerreno;
                }
                else
                {

                    //Obtengo los Valores Máximos y Minimos de Pendiente
                    myDicVerticesAjustaMaxMin[j - 1].zAvanceNegPendMax = myDicVerticesAjustaMaxMin[j].ZAvanceNegMax + (segmentos[j-1].lon2d * segmentos[j-1].pendMaxPorUno);
                    myDicVerticesAjustaMaxMin[j - 1].zAvanceNegPendMin = myDicVerticesAjustaMaxMin[j].ZAvanceNegMin - (segmentos[j-1].lon2d * segmentos[j-1].pendMaxPorUno);

                    myDicVerticesAjustaMaxMin[j- 1].zAvanceNegTerraplenMax = myDicVerticesAjustaMaxMin[j - 1].zTerreno + mRoadGeo.terraplenDesmonteMaxProyecto;
                    myDicVerticesAjustaMaxMin[j- 1].zAvanceNegDesmonteMin = myDicVerticesAjustaMaxMin[j - 1].zTerreno - mRoadGeo.terraplenDesmonteMaxProyecto;


                    //Obtengo el Valor Min del Maximo y Maximo del Minimo
                    myDicVerticesAjustaMaxMin[j - 1].ZAvanceNegMax = myDicVerticesAjustaMaxMin[j - 1].getAvanceNegZmaxMin;
                    myDicVerticesAjustaMaxMin[j - 1].ZAvanceNegMin = myDicVerticesAjustaMaxMin[j - 1].getAvanceNegZminMax;

                }

            }


            
            //Cargo los Valores en el EjeTadil

            for (int k = 0; k < myDicVerticesAjustaMaxMin.Count; k++)
            {
                this.vertices[k].ZmaxAjuste = myDicVerticesAjustaMaxMin[k].getZmax;
                this.vertices[k].ZminAjuste = myDicVerticesAjustaMaxMin[k].getZmin;
            }
              




            }





        public void ajustePerfil()
        {

            //1-Obtener la Viabilidad de los Vertices
            getViabilidadEje();

            //2-Obtener los Zmax y Zmin de los Vertices
            getZMaxMinEje();


            while (hasEjeIncumplimientosViables)
            {


                foreach (int miId in idSecuenciaCheck)
                {
                    if (segmentos[miId].hasIncumplimientos)
                    {

                        segmentos[miId].ajusteTramo();

                        oCadManager.thisEditor.WriteMessage("Ajustando el tramo :" + segmentos[miId].idBase1 +"\n");

                        break;
                    }
                }


                //Reinicio el EjE
                getIncumplimientosPerfil();


            }

        }          
            
    }      
            
            
        
        
}  
        
        
        

    

