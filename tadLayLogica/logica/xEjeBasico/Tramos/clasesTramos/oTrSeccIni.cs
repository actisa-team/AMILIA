//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace tadLayLogica
//{

//    using tadLayShare;
//    using tadLayShare.puntoOld;
    
//    //public  class oTrSecIni : oTramoSec
//    //{


//    //   private double? mTrAziGrados = null;
//    //   private double? mTrLon = null;
//    //   private double? mRadioMin = null;
//    //   private double? mPendienteSalida = null;
//    //   private bool? mIsLonMinRecta = null;
  


    
//    //   public oTrSecIni(oP3dAziLonPen iPtoIni,double iRadioMin)
//    //   {
//    //       pOrden = 1;
//    //       pEstGenerar = false;
//    //       pIsAbanicoAvance = false;

//    //       P1 = iPtoIni.p3d;
//    //       mTrAziGrados = iPtoIni.aziGr;
//    //       mPendienteSalida = iPtoIni.Pendiente;
//    //       mIsLonMinRecta = iPtoIni.IsLonMinRecta;

//    //       pName = "Tinicio";
          
//    //       mTrLon = iPtoIni.lon;
//    //       mRadioMin = iRadioMin;

//    //   }


//    //   public bool IsRotonda
//    //   {

//    //       get
//    //       {

//    //           if (mTrLon.HasValue &&  mTrAziGrados.HasValue)
//    //           {

//    //               return false;
//    //           }
//    //           else
//    //           {
//    //               return true;
//    //           }         
           
//    //       }
           
//    //   }


//    //   public override void getXY()
//    //   {


//    //       //Determino el P2, en función de los parametros de Entrada.

//    //       //CASO 1 ; ROTONDA  AZIMUT NULL ; LON NULL
//    //       if (mTrAziGrados == null && mTrLon == null)
//    //       {
//    //           P2.X = P1.X.Value;
//    //           P2.Y = P1.Y.Value;
//    //           P2.Z = P1.Z.Value;
//    //           pTramoValido = true;
//    //           return;
//    //       }
//    //       //CASO 2, AZIMUT TRUE ; LON SALIDA NULL
//    //       else if (mTrAziGrados != null && mTrLon == null)
//    //       {
//    //           mTrLon = oTadil.data.Proyecto.roadDesign.AijMinSalidaLlegada;  //oToolTadilFunciones.getAminTramosSalidaLlegada(mRadioMin.Value);
//    //           getP2();
//    //           return;
//    //       }
//    //       //CASO 3, AZIMUT TRUE ; LON SALIDA TRUE
//    //       else if (mTrAziGrados != null && mTrLon != null)
//    //       {
//    //           double myAmin = oTadil.data.Proyecto.roadDesign.AijMinSalidaLlegada; // oToolTadilFunciones.getAminTramosSalidaLlegada(mRadioMin.Value);

//    //           if (mIsLonMinRecta.Value)
//    //           {
//    //               mTrLon = mTrLon + myAmin;
//    //           }
//    //           else
//    //           {
//    //               if (myAmin > mTrLon)
//    //               {
//    //                   mTrLon = myAmin;
//    //               }
               
//    //           }
               
//    //           getP2();
//    //           return;
//    //       }
//    //       //CASO 4, AZIMUT NULL ; LON SALIDA TRUE ; ERROR
//    //       else if (mTrAziGrados == null && mTrLon != null)
//    //       {
//    //           throw new Exception("Opción No Valida ; Valor Azimut Nulo y Valor Tramo Longitud No Nulo.");
//    //       }
//    //       else
//    //       {
//    //           throw new Exception("Caso No Especificado en el Tramo de Salida.");
//    //       }

//    //   }


//    //   public override void getSeccion(double iTramoLonDiscre, double iProPendMaxPorCiento, double iProTerDesMax, double iEstPendMaxPorCiento, double iEstPuenteMax)
//    //   {

//    //       if (pTramoValido.HasValue && pTramoValido.Value)
//    //       {

//    //           //Se define el P2Z en función de la Pendiente de Salida
//    //           if (mPendienteSalida != null)
//    //           {
//    //              // P2Z = oToolTrigo.getP2zFromP1andLon(P1Z.Value, mPendienteSalida.Value, mTrLon.Value);
//    //           }
//    //           //Se define el P2Z sin Criterio de Pendiente de Salida
//    //           else
//    //           {
//    //               //Obtengo El Punto P2Z en función de la Pendiente de Proyecto
//    //              // P2Z = base.getZFromPendiente(iProPendMaxPorCiento, pPendienteP1P2Terreno.Value);
//    //           }
               
               
//    //           //Obtengo la Sección Sin Estructura
//    //           pLstTramoSeccion = getLstSeccion(iTramoLonDiscre);

//    //           //Confirmo Si el Movimiento de Tierras es Valido
//    //           bool myMovTierras = getMovTierrasSinEstructuras(iProTerDesMax);

//    //           //Movimiento Tierras OK
//    //           if (myMovTierras)
//    //           {
//    //               pTramoValido = true;
//    //               pEstHas = false;
//    //               return;
//    //           }
//    //           else
//    //           {
//    //               DialogResult myRes = oTadil.data.UserInfo.showSiNo("El Tramo Inicial No Cumple el Valor Máximo de Terraplen-Desmonte Definido\n ¿Deseas Continuar?");

//    //               if (myRes == DialogResult.Yes)
//    //               {
//    //                   pTramoValido = true;
//    //                   pEstHas = false;
//    //                   return;
//    //               }
//    //               else
//    //               {
//    //                throw new Exception ("Error el Tramo Inicial No cumple por TerraplenDesmonte Máximo.");
//    //               }
               
//    //           }

   

               
//    //       }

//    //   }


//    //   private void getP2()
//    //   {

//    //       //Obtener las Coordenadas del P2
//    //       oP2d myP2 = oToolTrigo.getP2FromLonAzimut(P1.X.Value, P1.Y.Value, mTrAziGrados.Value, mTrLon.Value);

//    //       //Determino si las Coordenadas del P2 estan dentro del Contorno
//    //       bool myP1IsIn = oTadil.data.Proyecto.Terreno.isPtoInsideTerreno(myP2);

//    //       if (myP1IsIn)
//    //       {
//    //           P2.X = myP2.X.Value;
//    //           P2.Y = myP2.Y.Value;
//    //           pTramoValido = true;
//    //       }
//    //       else
//    //       {
//    //           throw new Exception("El Punto P2 del tramo de Salida\nEsta fuera de la superficie.");
//    //       }

//    //       //Determino si el Tramo Atravisa una zona de No Paso
//    //       if (oTadil.data.Proyecto.ZonaNoPaso.isTramoOnZonaNoPaso(P1X.Value, P1Y.Value, P2X.Value, P2Y.Value))
//    //       {
//    //           throw new Exception("El Tramo de Salida Atraviesa una Zona de No Paso");
//    //       }

//    //   }


//    //}
//}
