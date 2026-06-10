//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{

//    using Autodesk.AutoCAD.DatabaseServices;

//    using tadLayLogica.datos.proyecto;
    
//    using engNet;
//    using engCadNet;
//    using tadLayShare;
//    using tadLayShare.puntoOld;
    
    
//    public  class oSolAlg
//    {

//       private string  mSolName = null;

     
     

//       private oAlgAbanico mAlgAba = null;

       
//       private oRoadDes mRoadDesign = null;
//       private oRoadGeo mRoadGeo = null;

//       private oDataAlgAba mDataAbanico = null;

//       private string mLayerSolEje ;

//       private string mLayerSolAbanico ;




//        //public oSolAlg(oTrSecIni iTrIni, oTrSecFin iTrFin, oRoadDes iRoadDesign,oRoadGeo iRoadEje, oDataAlgAba iDataAbanico)
//        //{
//        //    mTrIni = iTrIni;
//        //    mTrFin = iTrFin;
//        //    mRoadDesign = iRoadDesign;
//        //    mRoadGeo = iRoadEje;
//        //    mDataAbanico = iDataAbanico;           
//        //}


//        public void findPath(string iSolName,  oTargetOld iTarget)
//        {
//          //  mSolName = iSolName;
//          //  mAlgAba = new oAlgAbanico(mRoadDesign,mRoadGeo, mDataAbanico);
//          //  mAlgAba.getPath(mTrIni, mTrFin, iTarget);
//          ////  mLayerSolEje = string.Format(oTadil.xKSolEjeBasico, mSolName);
//          ////  mLayerSolAbanico = string.Format(oTadil.KSolAba,mSolName);  
//        }


//        public oAlgAbanico pAlgAbanico
//        {

//            get
//            {
//                return mAlgAba; 
//            }
        
//        }


//        /// <summary>
//        /// Dibujar Solución
//        /// </summary>
//        /// <param name="isLw2d">Dibujo LW2D</param>
//        /// <param name="isLw3d">Dibujo LW3D</param>
//        public void draw()
//        {


//                List<oP3d> myLstPto3d = mAlgAba.getLstPtoCamino;
     
//                //Genero Capa & Borro las Entidades
//                if (engCadNet.oLayer.HasLayer(mLayerSolEje))
//                {
//                    engCadNet.oLayer.deleteByLayer(mLayerSolEje);
//                }
//                else
//                {
//                    engCadNet.oLayer.addLayer(mLayerSolEje, 3, true);
//                }

               
//               //Genero la Polilinea 3D
//               Polyline3d miLw3d = oDraw.addLw3d(myLstPto3d, false, mLayerSolEje,0);

//               //Guardo la solucion 
//               Guid miGuidSol= Guid.Empty; // oDalTbSolucion.createSolucionFromEjeBasico(mSolName,miLw3d.Handle.ToString());

//               //Xdata Guid de la Solucion
//               oTadilXdata.addXdataSolucionNombreGuid(miLw3d, miGuidSol.ToString());
 
//               //Xdata de la Solución
//               oTadilXdata.addXdataSolucionNombre(miLw3d, mSolName);

//               //Xdata del RoadDesign
//               oTadilXdata.addXdataRoadDesign(miLw3d, mRoadDesign);

//              //Xdata del RoadGeo
//              // oTadilXdata.addXdataRoadGeo(miLw3d, mRoadGeo);

//              //Xdata de las Estructuras
//               oTadilXdata.addXdataEstructurasTramo(miLw3d, mAlgAba.getLstTramosEstructuras);

              
//        }

//        /// <summary>
//        /// Dibujar Abanico
//        /// </summary>
//        public void drawAbanico(bool isDrawAbanico)
//        {

//            if (isDrawAbanico)
//            {

//                if (mAlgAba.getLstTramosPorAbanicoRuta != null & mAlgAba.getLstTramosPorAbanicoRuta.Count > 0)
//                {               
//                    //Genero Capa & Borro las Entidades
//                    if (engCadNet.oLayer.HasLayer(mLayerSolAbanico))
//                    {
//                        engCadNet.oLayer.deleteByLayer(mLayerSolAbanico);
//                    }
//                    else
//                    {
//                        engCadNet.oLayer.addLayer(mLayerSolAbanico, 4, true);
//                    }

//                    //Represento los Abanico
//                    foreach (oTramoSecAba myTramo in mAlgAba.getLstTramosPorAbanicoRuta)
//                    {
//                        myTramo.drawData(mLayerSolAbanico);
//                    }
//                }

//            }
//        }


//        public void excel (bool isExcel)
//        {

//            if (isExcel)
//            {

//                try
//                {

//                    if (mAlgAba.getLstTramosPorAbanicoRuta != null & mAlgAba.getLstTramosPorAbanicoRuta.Count > 0)
//                    {

//                        string myPathDwg = engCadNet.oTools.getPathDwg();

//                        List<object> myLstObj = mAlgAba.getLstTramosPorAbanicoRuta.ConvertAll<object>(t => (object)t);

//                        oExcelUtilities.writeExcel(myPathDwg, mSolName, myLstObj);
//                    } 
//                }
//                catch (Exception)
//                {   
//                    throw new Exception ("Error al Generar el Excel.");
//                }
                   
//            }
        
//        }
  
//    }
//}
