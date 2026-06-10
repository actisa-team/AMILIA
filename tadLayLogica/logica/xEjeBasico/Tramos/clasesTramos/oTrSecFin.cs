//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace tadLayLogica
//{
   
//    using tadLayShare;
//    using tadLayShare.puntoOld;

//    //public class oTrSecFin : oTramoSec
//    //{


//    //    private double? mTrAziGrados = null;
//    //    private double? mTrLon = null;
//    //    private double? mRadioMin = null;
//    //    private double? mPendienteSalida = null;
//    //    private bool?   mIsLonMinRecta = null;



//    //    public oTrSecFin( oP3dAziLonPen iPtoFin, double iRadioMin)
//    //    {

//    //        pName = "TFin";
//    //        pEstGenerar = false;
//    //        pIsAbanicoAvance = false;

//    //        P2 = iPtoFin.p3d;
//    //        mTrAziGrados = iPtoFin.aziGr;
//    //        mTrLon = iPtoFin.lon;
//    //        mPendienteSalida = iPtoFin.Pendiente;
//    //        mIsLonMinRecta = iPtoFin.IsLonMinRecta;
//    //        mRadioMin = iRadioMin;


//    //    }

//    //    public bool IsRotonda
//    //    {

//    //        get
//    //        {

//    //            if (mTrLon.HasValue && mTrAziGrados.HasValue)
//    //            {

//    //                return false;
//    //            }
//    //            else
//    //            {
//    //                return true;
//    //            }

//    //        }

//    //    }

//    //    public override void getSeccion(double iTramoLonDiscre,  double iProPendMaxPorCiento, double iProTerDesMax, double iEstPendMaxPorCiento, double iEstPuenteMax)
//    //    {

//    //        if (pTramoValido.HasValue && pTramoValido.Value)
//    //        {

//    //            //Se define el P1Z en función de la Pendiente de Salida
//    //            if (mPendienteSalida != null)
//    //            {
//    //               // P1.Z = oToolTrigo.getP2zFromP1andLon(P2Z.Value, mPendienteSalida.Value, mTrLon.Value);
//    //            }
//    //            //Se define el P2Z sin Criterio de Pendiente de Salida
//    //            else
//    //            {
//    //               // P1.Z = oToolTrigo.getP2zFromP1andLon(P2.Z.Value, -iProPendMaxPorCiento, mTrLon.Value);
//    //            }


//    //            //Obtengo la Sección Sin Estructura
//    //            pLstTramoSeccion = getLstSeccion(iTramoLonDiscre);

//    //            //Confirmo Si el Movimiento de Tierras es Valido
//    //            bool myMovTierras = getMovTierrasSinEstructuras(iProTerDesMax);

//    //            //Movimiento Tierras OK
//    //            if (myMovTierras)
//    //            {
//    //                pTramoValido = true;
//    //                pEstHas = false;
//    //                return;
//    //            }
//    //            else
//    //            {
//    //                DialogResult myRes = oTadil.data.UserInfo.showSiNo("El Tramo Final No Cumple el Valor Máximo de Terraplen-Desmonte Definido\n ¿Deseas Continuar?");

//    //                if (myRes == DialogResult.Yes)
//    //                {
//    //                    pTramoValido = true;
//    //                    pEstHas = false;
//    //                    return;
//    //                }
//    //                else
//    //                {
//    //                    throw new Exception("Error el Tramo Final No cumple por TerraplenDesmonte Máximo.");
//    //                }

//    //            }




//    //        }

//    //    }

//    //    public override void getXY()
//    //    {

//    //        //Determino el P1, en función de los parametros de Entrada.

//    //        //-CASO 1 ; ROTONDA  AZIMUT NULL ; LON NULL
//    //        if (mTrAziGrados == null && mTrLon == null)
//    //        {
//    //            P1.X = P2.X.Value;
//    //            P1.Y = P2.Y.Value;
//    //            P1.Z = P2.Z.Value;
//    //            pTramoValido = true;
//    //            return;
//    //        }
//    //        //-CASO 2, AZIMUT TRUE ; LON SALIDA NULL
//    //        else if (mTrAziGrados != null && mTrLon == null)
//    //        {
//    //            mTrLon = oTadil.data.Proyecto.roadDesign.AijMinSalidaLlegada;   //oToolTadilFunciones.getAminTramosSalidaLlegada(mRadioMin.Value);
//    //            getP1();
//    //            return;
//    //        }
//    //        //CASO 3, AZIMUT TRUE ; LON SALIDA TRUE
//    //        else if (mTrAziGrados != null && mTrLon != null)
//    //        {
//    //            double myAmin = oTadil.data.Proyecto.roadDesign.AijMinSalidaLlegada;//oToolTadilFunciones.getAminTramosSalidaLlegada(mRadioMin.Value);

//    //            if (mIsLonMinRecta.Value)
//    //            {
//    //                mTrLon = mTrLon + myAmin;
//    //            }
//    //            else
//    //            {
//    //                if (myAmin > mTrLon)
//    //                {
//    //                    mTrLon = myAmin;
//    //                }

//    //            }

//    //            getP1();
//    //            return;
//    //        }
//    //        //CASO 4, AZIMUT NULL ; LON SALIDA TRUE ; ERROR
//    //        else if (mTrAziGrados == null && mTrLon != null)
//    //        {
//    //            throw new Exception("Opción No Valida ; Valor Azimut Nulo y Valor Tramo Longitud No Nulo.");
//    //        }
//    //        else
//    //        {
//    //            throw new Exception("Caso No Especificado en el Tramo de Salida.");
//    //        }





//    //    }

//    //    private void getP1()
//    //    {

//    //        //Obtener las Coordenadas del P1
//    //        oP2d myP1 = oToolTrigo.getP2FromLonAzimut(P2.X.Value, P2.Y.Value, mTrAziGrados.Value, mTrLon.Value);

//    //        //Determino si las Coordenadas del P1 estan dentro del Contorno
//    //        bool myP1IsIn = oTadil.data.Proyecto.Terreno.isPtoInsideTerreno(myP1);

//    //        if (myP1IsIn)
//    //        {
//    //            P1.X = myP1.X.Value;
//    //            P1.Y = myP1.Y.Value;
//    //            pTramoValido = true;
//    //        }
//    //        else
//    //        {
//    //            throw new Exception("El Punto P1 del tramo de Llegada\nEsta fuera de la superficie.");
//    //        }

//    //        //Determino si el Tramo Atravisa una zona de No Paso
//    //        if (oTadil.data.Proyecto.ZonaNoPaso.isTramoOnZonaNoPaso(P1X.Value, P1Y.Value, P2X.Value, P2Y.Value))
//    //        {
//    //            throw new Exception("El Tramo de Llegada Atraviesa una Zona de No Paso");
//    //        }

//    //    }


//    //}    

//}
    

