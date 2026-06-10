//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{

    
//    using tadLayShare;
//    using tadLayShare.puntoOld;
    
//    public abstract class oTramoSec : oTramo
//    {


//        private bool?                   mValido                 = null;
//        private eTramoError?            mError                  = null;
//        private bool?                   mIsAbanicoAvance        = null;

//        private List<oTramoSeccion> mLstTrSeccion = new List<oTramoSeccion>();


//        private bool? mEstGenerar = null;
//        private bool? mEstHas = null;

//        //private double? mTramoLonDiscre = null;

//        /// <summary>
//        /// Coste del Tramo
//        /// </summary>
//        private double? mCoste = null;
 
//        public oTramoSec()
//        {


//        }

//        #region "Propiedades"

//        /// <summary>
//        /// Estructura Tramo
//        /// </summary>
//        public eEstructuraOld pEstructura
//        {
//            get
//            {

     
//               var myQueryPueTun = from p in mLstTrSeccion
//                          where p.pApoyo == eApoyo.estPue & p.pApoyo == eApoyo.estTun
//                          select p;

//               var myQueryPue = from p in mLstTrSeccion
//                                where p.pApoyo == eApoyo.estPue
//                                select p;

//               var myQueryTun = from p in mLstTrSeccion
//                                where p.pApoyo == eApoyo.estTun
//                                select p;

//               var myQueryError = from p in mLstTrSeccion
//                                where p.pApoyo == eApoyo.estError
//                                select p;

              



//               if (myQueryPueTun.Count() > 0)
//               {
//                   return eEstructuraOld.TunelPila;
//               }
//               else if (myQueryTun.Count() > 0)
//               {
//                   return eEstructuraOld.Tunel;
//               }
//               else if (myQueryPue.Count() > 0)
//               {
//                   return eEstructuraOld.Pila;
//               }
//               else if (myQueryError.Count() > 0)
//               {
//                   throw new Exception("El tramo No es Válido ; Error Apoyo."); 
//               }
//               else
//               {
//                   return eEstructuraOld.SinEstructura;
//               }
            
//            }
        
        
        
//        }


//        /// <summary>
//        /// Coste del Tramo
//        /// </summary>
//        public virtual double? pCoste
//        {

//            get
//            {
//                if (mCoste.HasValue)
//                {
//                    return mCoste.Value;
//                }
//                else
//                {
//                    return null;
//                }
            
//            }


//            set
//            {
//                mCoste = value;     
//            }

//        }

//        /// <summary>
//        /// Coste por Metro Lineal del Tramo
//        /// </summary>
//        public virtual double? pCosteMl
//        {

//            get
//            {

//                if (pCoste.HasValue)
//                {
//                    return Math.Round(pCoste.Value / pDis2d.Value, 2); ;
//                }
//                else
//                { 
//                    return null;
//                }
            
//            }     
//        }
//        public bool? pEstGenerar
//        {

//            get
//            {
//                if (mEstGenerar.HasValue)
//                {
//                    return mEstGenerar.Value;
//                }
//                else
//                {
//                    throw new Exception("El Valor del Generar Estructuras es Nulo");
//                }
//            }

//            set
//            {
//                mEstGenerar = value;
//            }

//        }
//        public bool? pEstHas
//        {

//            get
//            {
//                if (mEstHas.HasValue)
//                {
//                    return mEstHas.Value;
//                }
//                else
//                {
//                    throw new Exception("El Valor del Tener Estructuras es Nulo");
//                }
//            }

//            set
//            {
//                mEstHas = value;
//            }

//        }
//        public bool pIsAbanicoAvance
//        {

//            get
//            {

//                if (mIsAbanicoAvance.HasValue)
//                {
                
//                    return mIsAbanicoAvance.Value;
//                }
//            else
//                    {
//                        throw new Exception("El Valor del Avanico de Abance, es NULO");
//                    }
//            }

//            set
//            {
//                mIsAbanicoAvance = value;
            
            
//            }

        
        
//        }
//        public bool? pTramoValido
//        {

//            get
//            {
//                if (mValido.HasValue)
//                {
//                    return mValido;
//                }
//                else
//                {
//                    return null;
//                }
//            }

//            set
//            {
//                mValido = value;
//            }

//        }
//        public eTramoError? pTramoError
//        {

//            get
//            {

//                if (mError.HasValue)
//                {
//                    return mError.Value;

//                }
//                else
//                {
//                    return null;
//                }

//            }

//            set
//            {
//                mError = value;

//            }

//        }
 
//        public double? pPendienteP1P2Terreno
//        {

//            get
//            {

//                if (P1X.HasValue && P1Y.HasValue && P1Z.HasValue && P2X.HasValue && P2Y.HasValue && pP2terrenoZ.HasValue)
//                {
//                    return oToolTrigo.getPendientePorCiento(P1X, P1Y, P1Z, P2X, P2Y, pP2terrenoZ).Value;
//                }

//                else
//                {
//                    return null;
//                }





//            }


//        }
//        public double? pP1terrenoZ
//        {

//            get
//            {

//                if (P1.X.HasValue && P1Y.HasValue)
//                {
//                    return oTadil.data.Proyecto.Terreno.getZFromXY(P1X, P1Y);
//                }
//                else
//                {
//                    return null;
//                }

//            }



//        }
//        public double? pP2terrenoZ
//        {

//            get
//            {

//                if (P2X.HasValue && P2Y.HasValue)
//                {

//                    double? myZt = oTadil.data.Proyecto.Terreno.getZFromXY(P1X, P1Y);

//                    if (myZt.HasValue)
//                    {
//                        return myZt.Value;
//                    }
//                    else
//                    {
//                        pTramoValido = false;
//                        pTramoError = eTramoError.puntoExteriorSuperficie;
//                        return null;
//                    }


//                }
//                else
//                {

//                    return null;
//                }

//            }



//        }
//        protected List<oTramoSeccion> pLstTramoSeccion
//        {

//            get
//            {
//                return mLstTrSeccion;
//            }
            
            
//            set
//            {
//                mLstTrSeccion = value;
            
//            }  
//        }

//        #endregion



//        #region "Metodos"

//        //Obtengo los Datos del P1XY-P2XY
//        public abstract void getXY();

        
//        //Obtengo la Sección
//        public virtual void getSeccion(double iTramoLonDiscre,  double iProPendMaxPorCiento, double iProTerDesMax, double iEstPendMaxPorCiento, double iEstPuenteMax)
//        {

//            if (pTramoValido.HasValue && pTramoValido.Value)
//            {

//                //Obtengo El Punto P2Z en función de la Pendiente de Proyecto
//                P2Z = getZFromPendiente(iProPendMaxPorCiento, pPendienteP1P2Terreno.Value);

//                //Obtengo la Sección Sin Estructura
//                mLstTrSeccion = getLstSeccion(iTramoLonDiscre);

//                //Confirmo Si el Movimiento de Tierras es Valido
//                bool myMovTierrasSinEst = getMovTierrasSinEstructuras(iProTerDesMax);



//                //Movimiento Tierras TRUE
//                if (myMovTierrasSinEst)
//                {
//                    pTramoValido = true;
//                    pEstHas = false;
//                    return;
//                }

//                //Movimiento Tierras TRUE & Generar Estructura FALSE
//                if (!myMovTierrasSinEst && !pEstGenerar.Value)
//                {
//                    pTramoValido = false;
//                    return;
//                }

//                //Movimiento Tierras FALSE & Generar Estructura TRUE
//                if (!myMovTierrasSinEst && pEstGenerar.Value)
//                {
//                    //Obtengo el Nuevo P2Z en función de la nueva Pendiente de la Estructura
//                    P2Z = getZFromPendiente(iEstPendMaxPorCiento, pPendienteP1P2Terreno.Value);

//                    //Modifico la Seccion de Puntos Intermedios
//                    gSeccionCambiarPtoTrazado(iTramoLonDiscre);


//                    //Obtengo las Estructuras del Tramo.
//                    bool myMovTierrasConEst = getMovTierrasConEstructuras(iProTerDesMax, iEstPuenteMax);


//                    //Valido el Tramo
//                    if (myMovTierrasConEst)
//                    {
//                        pTramoValido = true;
//                        pEstHas = true;
//                    }
//                    else
//                    {
//                        pTramoValido = false;
//                    }

//                }
//            }
           
//        }


   
        
        
  

//        #endregion

//#region "Metodos Privados"
//        //Genero la Seccion IDnum-PTOtrazado-Zterreno-ZonaGeotecnica
//        protected List<oTramoSeccion> getLstSeccion(double iTramoDiscreLon)
//        {

//            //Obtengo los Puntos Intermedios
//            List<oP3d> myLstPtoIntermedios = new List<oP3d>();
//            //Añado los Puntos Intermedios
//            myLstPtoIntermedios = oToolTrigo.getLstPointIntermediosTramo(pAzimutGrados.Value, P1X.Value, P1Y.Value, P1Z.Value, pDis2d.Value, pPendValorPorCiento.Value,iTramoDiscreLon);
//            //Obtengo la Longitud entre Puntos Reales
//            //TramoLonDiscreProyecto 20 ;
//            //TramoLon = 175m
//            //175/20 = 8.75 = 9
//            //TramoLonReal = 175/9 

//            double myTramoDiscreReal = Math.Round(pDis2d.Value / (myLstPtoIntermedios.Count + 1),2);
            
            
//            List<oTramoSeccion> myLstSeccion = new List<oTramoSeccion>();
//            oTramoSeccion myTrSeccion = new oTramoSeccion();
//            int myId = 1;


//            myTrSeccion.pId = 1;
//            myTrSeccion.pPto = P1;
//            myTrSeccion.pLon = myTramoDiscreReal / 2;
//            myTrSeccion.pZterreno = pP1terrenoZ.Value;
//            myTrSeccion.pZonaGeotecnia = "ZonaGeneral";
//            myTrSeccion.pSlope = oTadil.data.Proyecto.Terreno.getSlopeFromXY(P1.X.Value, P1.Y.Value).Value;


//            myLstSeccion.Add(myTrSeccion);


//            foreach (oP3d myPtoInt in myLstPtoIntermedios)
//            {

//                myId++;

//                myTrSeccion = new oTramoSeccion();   

//                myTrSeccion.pId = myId;
//                myTrSeccion.pPto = myPtoInt;
//                myTrSeccion.pLon = myTramoDiscreReal;
//                myTrSeccion.pZterreno = oTadil.data.Proyecto.Terreno.getZFromXY(myPtoInt.X, myPtoInt.Y).Value;
//                myTrSeccion.pZonaGeotecnia = "ZonaGeneral";
//                myTrSeccion.pSlope = oTadil.data.Proyecto.Terreno.getSlopeFromXY(myPtoInt.X.Value, myPtoInt.Y.Value).Value;

//                myLstSeccion.Add(myTrSeccion);

//            }

//            //Añado el Punto Final
//            myTrSeccion = new oTramoSeccion();
//            myTrSeccion.pId = myId+1;
//            myTrSeccion.pPto = P2;
//            myTrSeccion.pLon = myTramoDiscreReal/2;
//            myTrSeccion.pZterreno = pP2terrenoZ.Value;
//            myTrSeccion.pZonaGeotecnia = "ZonaGeneral";
//            myTrSeccion.pSlope = oTadil.data.Proyecto.Terreno.getSlopeFromXY(P2X.Value, P2Y.Value).Value;
      


//            myLstSeccion.Add(myTrSeccion);


//            return myLstSeccion;

//        }
//        //Obtengo el Movimiento de Tierras SIN Estructuras
//        protected bool getMovTierrasSinEstructuras(double iProDesTerMax)
//        {

//            foreach (oTramoSeccion myTr in mLstTrSeccion)
//            {

//                if (myTr.pTerrenoCorrecion == eExcavacion.acota)
//                {
//                    myTr.pApoyo = eApoyo.estCota;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.desmonte && Math.Abs(myTr.pZdesfase.Value) <= iProDesTerMax)
//                {
//                    myTr.pApoyo = eApoyo.estDes;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.desmonte && Math.Abs(myTr.pZdesfase.Value) > iProDesTerMax)
//                {
//                    pTramoError = eTramoError.desmonteSuperior;
//                    myTr.pApoyo = eApoyo.estError;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.terraplen && Math.Abs(myTr.pZdesfase.Value) <= iProDesTerMax)
//                {
//                    myTr.pApoyo = eApoyo.estTer;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.terraplen && Math.Abs(myTr.pZdesfase.Value) > iProDesTerMax)
//                {
//                    pTramoError = eTramoError.terraplenSuperior;
//                    myTr.pApoyo = eApoyo.estError;
//                }
//                else
//                {
//                    throw new Exception("Caso de Estructura Sin Pilas-Tuneles, No Configurado");

//                }

//            }


//            //Valido el Tramo

//            //Configuro el Tramo
//            var myQuery = from p in mLstTrSeccion
//                          where p.pApoyo == eApoyo.estError
//                          select p;

//            if (myQuery.Count() > 0)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }


//        }
//        //Obtengo el Movimeinto de Tierras CON Estructuras
//        private bool getMovTierrasConEstructuras(double iProDesTerMax, double iEstPilaMax)
//        {


//            foreach (oTramoSeccion myTr in mLstTrSeccion)
//            {

//                if (myTr.pTerrenoCorrecion == eExcavacion.acota)
//                {
//                    myTr.pApoyo = eApoyo.estCota;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.desmonte && Math.Abs(myTr.pZdesfase.Value) <= iProDesTerMax)
//                {
//                    myTr.pApoyo = eApoyo.estDes;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.desmonte && Math.Abs(myTr.pZdesfase.Value) > iProDesTerMax)
//                {
//                    myTr.pApoyo = eApoyo.estTun;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.terraplen && Math.Abs(myTr.pZdesfase.Value) <= iProDesTerMax)
//                {
//                    myTr.pApoyo = eApoyo.estTer;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.terraplen && Math.Abs(myTr.pZdesfase.Value) > iProDesTerMax && Math.Abs(myTr.pZdesfase.Value) <= iEstPilaMax)
//                {
//                    myTr.pApoyo = eApoyo.estPue;
//                }
//                else if (myTr.pTerrenoCorrecion == eExcavacion.terraplen && Math.Abs(myTr.pZdesfase.Value) > iProDesTerMax && Math.Abs(myTr.pZdesfase.Value) > iEstPilaMax)
//                {
//                    myTr.pApoyo = eApoyo.estError;
//                    pTramoError = eTramoError.puenteAlturaSuperior;
//                }
//                else
//                {
//                    throw new Exception("Caso de Estructura Con Pilas-Tuneles, No Configurado");
//                }

//            }


//            //Configuro el Tramo
//            var myQuery = from p in mLstTrSeccion
//                          where p.pApoyo == eApoyo.estError
//                          select p;

//            if (myQuery.Count() > 0)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }



//        }
//        //Modifico la Sección Cuando Cambio la Sección por la Estructura
//        private void gSeccionCambiarPtoTrazado(double iTramoLonDiscre)
//        {

//            List<oP3d> myLstPtoIntermedios = new List<oP3d>();

//            //Obtengo el listado de Puntos del Tramo
//            myLstPtoIntermedios = oToolTrigo.getLstPointIntermediosTramo(pAzimutGrados.Value, P1X.Value, P1Y.Value, P1Z.Value, pDis2d.Value, pPendValorPorCiento.Value,iTramoLonDiscre);


//            int myTrId = 1;

//            //Añado los Puntos Intermedios
//            foreach (oP3d myPtoInt in myLstPtoIntermedios)
//            {

//                mLstTrSeccion[myTrId].pPto = myPtoInt;

//                myTrId++;
//            }

//        }


//        protected double? getZFromPendiente(double iPendMaxAbs,double iPendTerreno)
//        {


//            if (Math.Abs(iPendTerreno) > iPendMaxAbs)
//            {
                
//                double myZIncremento = (pDis2d.Value * iPendMaxAbs) / 100;

//                if (pPendienteP1P2Terreno.Value > 0)
//                {
//                    return P1Z.Value + myZIncremento;
//                }
//                else
//                {
//                    return P1Z.Value - myZIncremento;
//                }
               
//            }
//            else
//            {
//                return pP2terrenoZ;
//            }

//        }

//#endregion

//        public void draw(string iLayer)
//        {
 	       
//                if (pTramoValido.HasValue && pTramoValido.Value &&  mError != eTramoError.puntoExteriorSuperficie)
//                {
//                        foreach (oTramoSeccion myTr in mLstTrSeccion)
//                        {
//                            if (myTr.pId != 1)
//                            {
//                                engCadNet.oCircle.addCircle2D(myTr.pPto.X.Value, myTr.pPto.Y.Value, 0.05, iLayer);
//                                engCadNet.oText.addText2D("Id: " + myTr.pId.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Zd: " + myTr.pPto.Z.Value.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value + 0.3, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Zt: " + myTr.pZterreno.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value + 0.6, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Desfase: " + myTr.pZdesfase.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value + 0.9, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Terreno: " + myTr.pTerrenoCorrecion.Value.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value + 1.2, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Pendiente:" + myTr.pSlope.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value + 1.5, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Pendiente Valoración:" + oToolTadilFunciones.getValorFromSlope(myTr.pSlope), myTr.pPto.X.Value, myTr.pPto.Y.Value + 1.8, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Apoyo: " + myTr.pApoyo.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value + 2.1, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("ApoyoLon: " + myTr.pLon.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value + 2.4, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Zona: " + myTr.pZonaGeotecnia, myTr.pPto.X.Value, myTr.pPto.Y.Value + 2.7, 0.25, 0, iLayer);
//                                engCadNet.oText.addText2D("Coste: " + myTr.pApoyoCoste.ToString(), myTr.pPto.X.Value, myTr.pPto.Y.Value + 3, 0.25, 0, iLayer);
//                            }
//                        }                   
//                }
//            }
//        }
//}


    

