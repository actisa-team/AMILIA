//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;



//namespace tadLayLogica
//{

   
//    using tadLayShare;
//    using tadLayShare.puntoOld;

//    using System.Windows.Forms;

    
//    public  class oAlgAbanico
//    {


//       private oRoadDes                              mDataDesign                     = null;
//       private oRoadGeo mDataGeo = null;
//       private oDataAlgAba                          mDatAlgAba                      = null;

//       private Dictionary<int, oTramoSec>           mLstTramosOptimos               = null;
//       private List<oTramoSecAba>                   mLstTramosPorAbanicoCamino      = null;
//       private List<oP3d> mLstPto3dPath = null;

//        //Constructor
//        public oAlgAbanico (oRoadDes iRoadDesign,oRoadGeo iRoadGeo, oDataAlgAba iDataAlgo)                                                                                                                                        
//        {
//            mDataDesign = iRoadDesign;
//            mDataGeo = iRoadGeo;
//            mDatAlgAba = iDataAlgo;
//        }


//        #region "Propiedades" 
 
//        /// <summary>
//        /// Listado de Estructuras en Tramos (Para el Xdata)
//        /// </summary>
//        public string getLstTramosEstructuras
//        {
//            get
//            {

//                string myStrLstEst = string.Empty;

//                foreach (KeyValuePair<int, oTramoSec> myTramo in mLstTramosOptimos)
//                {
//                    myStrLstEst = myStrLstEst + ";" + myTramo.Value.pEstructura.ToString();
//                }

//                //Elimino el ";"
//                return  myStrLstEst.Remove(0, 1);
            
       
//            }
        
        
//        }



//        public Dictionary<int, oTramoSec> getLstTramos
//        {


//            get
//            {

//                if (mLstTramosOptimos != null && mLstTramosOptimos.Count > 0)
//                {
//                    return mLstTramosOptimos;
//                }
//                else
//                {
//                    throw new Exception("El Listado De Tramos Óptimos Es Nulo.");
//                }
            
            
            
//            }
        
//        }

//        /// <summary>
//        /// Listado con todos los puntos del Camino, (1,2,3,etc)
//        /// </summary>
//        public List<oP3d> getLstPtoCamino
//        {
//            get
//            {

//                if (mLstTramosOptimos != null && mLstTramosOptimos.Count > 0)
//                {
//                    return mLstPto3dPath;
//                }
//                else
//                {
//                    throw new Exception("El Listado De Tramos Óptimos Es Nulo.");
//                }
            
            
//            }
        
        
//        }


//        /// <summary>
//        /// Listado con todos los Tramos por Abanico de la Solución
//        /// </summary>
//        public List<oTramoSecAba> getLstTramosPorAbanicoRuta
//        {

//            get
//            { 
//                if (mLstTramosPorAbanicoCamino != null && mLstTramosPorAbanicoCamino.Count>0)
//                {
                
//                    return mLstTramosPorAbanicoCamino;
//                }
//                else
//                {
//                throw new Exception("El Listado de Tramos por Abanico y Solución es Nulo.");
//                }        
//            }

        
//        }

//        #endregion


//        #region "MetodosPublicos"

//        //public void getPath(oTrSecIni iTrIni, oTrSecFin iTrFin, oTargetOld iTarget)      
                                                                                                                                                                                
//        //{


//        //    try
//        //    {

//        //        mLstTramosOptimos = new Dictionary<int, oTramoSec>();
//        //        mLstTramosPorAbanicoCamino = new List<oTramoSecAba>();
//        //        mLstPto3dPath = new List<oP3d>();
                
//        //        int myTramoId =0;


//        //        //Añado el Tramo Inicial a la Colección
//        //         if (iTrIni.IsRotonda)
//        //         {
//        //             myTramoId = 1;
//        //         }
//        //         else
//        //         {
//        //             mLstTramosOptimos.Add(1, iTrIni);
//        //             myTramoId = 2;
//        //         }
             


              
//        //        double myTrPrevioLon = iTrIni.pDis2d.Value;
//        //        bool myTrPrevioIsAbanicoAvance = false;

//        //        oP3d myPtoOrigen = iTrIni.P2;
//        //        oP3d myPtoPrevio = iTrIni.P1;
//        //        oP2d myPtoObjetivo = iTarget.getPtoTargetScw(myPtoOrigen.XY,mDataDesign.AijMin,mDatAlgAba.pDesPtoTargetPorCiento);


             
//        //        double mydistancia = mDataDesign.AijMin + 1;
//        //        double mydistMin = mDataDesign.AijMin;

//        //        oAbanicoByPoint myAbanico=null;

//        //        int miIdAbanico = 1;

//        //        while (mydistancia > mydistMin)
//        //        {
//        //            //Creo los sucesivos abanicos
//        //            myAbanico = new oAbanicoByPoint(myTramoId, myPtoOrigen, myPtoObjetivo,myTrPrevioIsAbanicoAvance,myTrPrevioLon,myPtoPrevio, mDataDesign,mDataGeo, mDatAlgAba);

//        //            //Genero el Listado de Tramos por Abanico
//        //            myAbanico.generarAbanicos();

//        //            engCadNet.oCadManager.thisEditor.WriteMessage("\n Generando Abanico: " + miIdAbanico.ToString());
                    
//        //            //Añado a la colección al Dicionario
//        //            foreach (oTramoSec myTrSec in myAbanico.gLstTramosWin)
//        //            {
//        //                mLstTramosOptimos.Add(myTrSec.pOrden.Value, myTrSec);
//        //            }

                   
//        //            //Añado a la Colección
//        //            mLstTramosPorAbanicoCamino.AddRange(myAbanico.gLstTramosByAbanico);


//        //            miIdAbanico++;
//        //            myTramoId = myAbanico.gTramoWinEnd.pOrden.Value + 1;
//        //            myTrPrevioLon = myAbanico.gTramoWinEnd.pDis2d.Value;
//        //            myTrPrevioIsAbanicoAvance = myAbanico.gTramoWinEnd.pIsAbanicoAvance;
//        //            myPtoOrigen = myAbanico.gTramoWinEnd.P2; 
//        //            myPtoPrevio = myAbanico.gP1Abanico;
//        //            myPtoObjetivo = iTarget.getPtoTargetScw(myPtoOrigen.XY,mDataDesign.AijMin,mDatAlgAba.pDesPtoTargetPorCiento);
                  
//        //            mydistancia = oToolTrigo.getDisXyTwoPoints(myAbanico.gTramoWinEnd.P2.XY,iTrFin.P1.XY).Value;
//        //        }

        
//        //        //Debo de Generar el Tramo de Entronque
//        //        oTramoSec myTramoEntronque = getTramoEntronque(myAbanico.gTramoWinEnd.pOrden.Value, iTrFin.P1);

//        //        //Si  el tramo de entronque es Nulo, debido a que no cumple, pero queremos continuar el proceso.

//        //        if (myTramoEntronque != null)
//        //        {

//        //            //Debo de sustituir este nuevo por el antiguo
//        //            if (mLstTramosOptimos.ContainsKey(myTramoEntronque.pOrden.Value))
//        //            {
//        //                mLstTramosOptimos.Remove(myTramoEntronque.pOrden.Value);

//        //                mLstTramosOptimos.Add(myTramoEntronque.pOrden.Value, myTramoEntronque);

//        //                //Hemos cambiado el punto final del tramo
//        //                mLstTramosOptimos[mLstTramosOptimos.Count - 1].P2 = myTramoEntronque.P1;

//        //            }
//        //            else
//        //            {
//        //                throw new Exception("Error de Programación al Generar el Tramo de Entronque");
//        //            }
//        //        }

//        //        else
//        //        {                       
//        //            mLstTramosOptimos[mLstTramosOptimos.Count].P2 = iTrFin.P1;      
//        //        }

                

//        //        //Debo de Crear el Tramo de Llegada
//        //        //Añado el Último Tramo en Caso de NO ser Rotonda

//        //        if (!iTrFin.IsRotonda)
//        //        {
//        //            iTrFin.pOrden = mLstTramosOptimos.Count + 1;
//        //            mLstTramosOptimos.Add(iTrFin.pOrden.Value, iTrFin);
//        //        }


//        //        //Odeno el Diccionario por el TrId
//        //         var myQueryOrden = from p in mLstTramosOptimos.Keys
//        //                            orderby mLstTramosOptimos[p] ascending
//        //                            select p;


//        //        //Genero el Listado de Pto2d del Camino

//        //         bool myIni = true;

//        //         foreach (KeyValuePair<int, oTramoSec> myTramo in mLstTramosOptimos)
//        //         {
//        //             if (myIni)
//        //             {
//        //                 mLstPto3dPath.Add(myTramo.Value.P1);
//        //                 mLstPto3dPath.Add(myTramo.Value.P2);
//        //                 myIni = false;
//        //             }
//        //             else
//        //             {
//        //                 mLstPto3dPath.Add(myTramo.Value.P2);
//        //             }
//        //         }

//        //    }
//        //    catch (Exception)
//        //    {
               
//        //        throw;
//        //    }



//        //}
//        //private oTramoSec getTramoEntronque(int iTramoOrden, oP3d iP1TramoFin)
//        //{ 
        
        
//        //    var myQuery = from p in mLstTramosPorAbanicoCamino
//        //                  where p.pTramoValido.Value == true && p.pOrden == (iTramoOrden-1)
//        //                  orderby p.pValorTotal descending
//        //                  select p;


           

//        //    oTrSecIniFin myTramoEntronque = null;



//        //    foreach (var myTramo in myQuery.ToList<oTramoSecAba>())
//        //    {

//        //        myTramoEntronque = new oTrSecIniFin(iTramoOrden,myTramo.P2, iP1TramoFin);

//        //        myTramoEntronque.getXY();
//        //        myTramoEntronque.getSeccion(mDataGeo.pTramoLonDiscre,mDataGeo.pendMaxPorCientoCalculo, mDataGeo.terraplenDesmonteMaxCalculo, mDataGeo.pendMaxPorCientoCalculoEstructura, mDataGeo.pilaAlturaMaximaCalculo);

//        //        if (myTramoEntronque.pTramoValido.HasValue && myTramoEntronque.pTramoValido.Value)
//        //        {
//        //            return (oTramoSec)myTramoEntronque;
//        //        }
                
//        //    }


//        //    DialogResult myResul = oTadil.data.UserInfo.showSiNo("El Tramo de Entronque con el Tramo de Llegada No Cumple\n con los valores máximos de Terraplen-Desmonte\n ¿Deseas Continuar");


//        //    if (myResul == DialogResult.Yes)
//        //    {

//        //        return null;
//        //    }
//        //    else
//        //    {

//        //        throw new Exception("Proceso Cancelado Usuario");
            
//        //    }
           
                  
//        //}

//        #endregion

//    }
//}
