//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{
//    using engCadNet;
//    using tadLayShare;
//    using tadLayShare.puntoOld;
    
//    //public class oSolAlgFactory
//    //{

//    //    private string mSolName = null;


//    //    private oDataAlgAba mDataAbanico = null;

//    //    private oSolAlg mSolAlgAba = null;

//    //    private bool? mAbanicoDraw = null;
//    //    private bool? mExcelDraw = null;
//    //    private bool? mSolOptEnvolventes = null;

//    //    public StringBuilder mUserInfo = new StringBuilder("Tadil Info");



//    //     public oSolAlgFactory(string iSolName, oDataAlgAba iDataAbanico)
//    //    {
//    //        mSolName = iSolName;
//    //        mDataAbanico = iDataAbanico;    
//    //    }

//    //     public void setUpSolucion(bool iAbanicoDraw, bool iExcelDraw, bool iSolOptEnvolvente)
//    //     {
//    //         mAbanicoDraw = iAbanicoDraw;
//    //         mExcelDraw = iExcelDraw;
//    //         mSolOptEnvolventes = iSolOptEnvolvente; 
//    //     }

//    //    public void findPath()
//    //    {

//    //        //LOGICA CAPAS 
//    //        oLayer.deleteByStartWith(oTadil.data.Layer.prefijoSoluciones + mSolName, false);

//    //        //Borro el Eje Rastreador
//    //        oLayer.deleteByListName(new List<string>(new string[] { oTadil.data.Layer.pTadil_No.name}));

            
//    //        //Activo la Capa Grafo Visibilidad
//    //        oTadil.data.Layer.visibilidadEje.On();
//    //        oTadil.data.Layer.visibilidadGrafo.Off();


//    //            //CREO EL TRAMO INICIAL
//    //            oTrSecIni  myTrIni = new  oTrSecIni (oTadil.data.Proyecto.PtoOrigen,
//    //                                                 oTadil.data.Proyecto.roadDesign.Rp);
//    //            myTrIni.getXY();

//    //            if (! myTrIni.IsRotonda)
//    //            {
//    //             myTrIni.getSeccion(
//    //                                oTadil.data.Proyecto.roadGeometria.pTramoLonDiscre,
//    //                                oTadil.data.Proyecto.roadGeometria.pendMaxPorCientoCalculo,
//    //                                oTadil.data.Proyecto.roadGeometria.terraplenDesmonteMaxCalculo,
//    //                                oTadil.data.Proyecto.roadGeometria.pendMaxPorCientoCalculoEstructura,
//    //                                oTadil.data.Proyecto.roadGeometria.pilaAlturaMaximaCalculo);
//    //            }

                
//    //           //CREO EL TRAMO FINAL
//    //           oTrSecFin myTrFin = new oTrSecFin(oTadil.data.Proyecto.PtoDestino,
//    //                                             oTadil.data.Proyecto.roadDesign.Rp);
//    //            myTrFin.getXY();

//    //            if (! myTrFin.IsRotonda)
//    //            {
//    //                myTrFin.getSeccion(
//    //                                   oTadil.data.Proyecto.roadGeometria.pTramoLonDiscre,
//    //                                   oTadil.data.Proyecto.roadGeometria.pendMaxPorCientoCalculo,
//    //                                   oTadil.data.Proyecto.roadGeometria.terraplenDesmonteMaxCalculo,
//    //                                   oTadil.data.Proyecto.roadGeometria.pendMaxPorCientoCalculoEstructura,
//    //                                   oTadil.data.Proyecto.roadGeometria.pilaAlturaMaximaCalculo); 
//    //            }


//    //            //Chekeo la Distancia TrIni-P2 Distancia TrFin-P1 es inferior a la Distancia de Avance
//    //            if (oTadil.data.Proyecto.roadDesign.AijMin >= myTrIni.getDistanciaP2ToPdestino(myTrFin.P1.XY))
//    //            {
//    //                throw new Exception("La distancia de la ruta es inferior a la longitud mínima de avance");
//    //            }




//    //            #region "DEFINIR EJE VISIBILIDAD DEL CAD"
//    //          //  List<oP2d> miLstPtoEjeVisibilidad = oEjeVisibilidad.getPathFromCad(oTadil.data.Layer.visibilidadEje.name);
//    //            #endregion


//    //            #region "DEFINO EL OBJ TARGET"

//    //            oTargetOld myTarget = new oTargetOld(oTadil.data.Proyecto.PtoOrigen.XY, oTadil.data.Proyecto.PtoDestino.XY);

//    //           // myTarget.setListPtoTargetFromPathScw(miLstPtoEjeVisibilidad, oTargetOld.eFiltro.MaxMin);

//    //            #endregion



               
              
//    //            //INICIO EL ALGORITMO

//    //            mSolAlgAba = new oSolAlg(myTrIni,
//    //                                     myTrFin,
//    //                                    oTadil.data.Proyecto.roadDesign,
//    //                                    oTadil.data.Proyecto.roadGeometria,
//    //                                    mDataAbanico);
                
                

//    //            mSolAlgAba.findPath(mSolName + "_01_Primaria",  myTarget);


//    //            mSolAlgAba.draw();


//    //            mSolAlgAba.drawAbanico(mAbanicoDraw.Value);
//    //            mSolAlgAba.excel(mExcelDraw.Value);

//    //            mUserInfo.AppendLine();
//    //            mUserInfo.Append("Solución Primaria = Válida");

               
//    //            List<oP3d> myLstPtoPathOriginal = mSolAlgAba.pAlgAbanico.getLstPtoCamino;

//    //            //Ahora debo de obtener las soluciones Por Envolvente Max y Min

//    //            if (mSolOptEnvolventes.Value)
//    //            {
//    //                //Listado Puntos Y Positiva
//    //                myTarget.setListPtoTargetFromPathScw(myLstPtoPathOriginal, oTargetOld.eFiltro.Max);

//    //                //Obtengo la Envolvente del listado de puntos
//    //                List<oP2d> myEnvolventePos = null;// oConvexHullOld.getLstoP2dConvexHull(myTarget.getLstPtoTargetScw);

//    //                //Creo el Listado de Target
//    //                myTarget.setListPtoTargetFromPathScw(myEnvolventePos, oTargetOld.eFiltro.MaxMin);

//    //                //Solucion Valor Positivo
//    //                try
//    //                {
//    //                    if (myTarget.HasEnvolvente)
//    //                    {
//    //                        mSolAlgAba.findPath(mSolName + "_02_EnvolventeMax", myTarget);
//    //                        mSolAlgAba.draw();
//    //                        mSolAlgAba.drawAbanico(mAbanicoDraw.Value);
//    //                        mSolAlgAba.excel(mExcelDraw.Value);

//    //                        mUserInfo.AppendLine();
//    //                        mUserInfo.Append("Solución Envolvente Máxima = Válida");
//    //                    }
//    //                    else
//    //                    {
//    //                        mUserInfo.AppendLine();
//    //                        mUserInfo.Append("Solución Envolvente Máximos = No Existen Valores Máximos");
                        
//    //                    }
//    //                }
//    //                catch (Exception ex)
//    //                {

//    //                    //Crear una Excepcion Propia
//    //                    if (!ex.Message.StartsWith("Error001"))
//    //                    {
//    //                        throw;
//    //                    }
//    //                    else
//    //                    {
//    //                        mUserInfo.AppendLine();
//    //                        mUserInfo.Append("Solución Envolvente Máxima = NO Válida");
//    //                    }

//    //                }


//    //                //Listado Puntos Y Negativa
//    //                myTarget.setListPtoTargetFromPathScw(myLstPtoPathOriginal, oTargetOld.eFiltro.Min);

//    //                //Obtengo la Envolvente del listado de puntos
//    //                List<oP2d> myEnvolventeNeg = null; // oConvexHull.getLstoP2dConvexHull(myTarget.getLstPtoTargetScw);

//    //                //Creo el Listado de Target
//    //                myTarget.setListPtoTargetFromPathScw(myEnvolventeNeg, oTargetOld.eFiltro.MaxMin);

//    //                try
//    //                {
//    //                    if (myTarget.HasEnvolvente)
//    //                    {
//    //                        mSolAlgAba.findPath(mSolName + "_03_EnvolventeMin", myTarget);
//    //                        mSolAlgAba.draw();
//    //                        mSolAlgAba.drawAbanico(mAbanicoDraw.Value);
//    //                        mSolAlgAba.excel(mExcelDraw.Value);

//    //                        mUserInfo.AppendLine();
//    //                        mUserInfo.Append("Solución Envolvente Mínima = Válida");
//    //                    }
//    //                    else
//    //                    {
//    //                        mUserInfo.AppendLine();
//    //                        mUserInfo.Append("Solución Envolvente Mínima = No Existen Valores Mínimos");
//    //                    }
                       

//    //                }
//    //                catch (Exception ex)
//    //                {

//    //                    //Crear una Excepcion Propia
//    //                    if (!ex.Message.StartsWith("Error001"))
//    //                    {
//    //                        throw;
//    //                    }
//    //                    else
//    //                    {
//    //                        mUserInfo.AppendLine();
//    //                        mUserInfo.Append("Solución Envolvente Mínima = NO Válida");
//    //                    }
//    //                }

//    //            }
                   
           
//    //            //Desactivo las Capas
//    //            engCadNet.oLayer.vLayerListActDes(new List<string>(new string[] 
//    //                                             {oTadil.data.Layer.visibilidadGrafo.name, 
//    //                                             oTadil.data.Layer.pTadil_No.name }), 
//    //                                             true,false);

                         
//    //    }

//    //}
//}
