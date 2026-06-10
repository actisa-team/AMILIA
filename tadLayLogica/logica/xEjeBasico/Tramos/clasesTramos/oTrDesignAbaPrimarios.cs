//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{

//    using tadLayShare;
//    using tadLayShare.puntoOld;
    

//    //CLASE TRAMOS ABANICO INICIAL
//    public class oTrDesignAbaInicio : oTramoSecAba
//    {


//        private double? mTrAziGrados = null;
//        private double? mAngGradosTrPrevioActual = null;
//        private bool? mTrPrevioIsAbanicoAvance = null;
//        private double? mTrPrevioLon = null;
//        private int? mDesviacionMaximaLonPorCiento = null;
//        private bool? mInvalidarTramosAbaPrimarioLon = null;

//        private double? mProRadioMin = null;
//        private double? mProLonAvanceMin = null;
//        private bool mProLonAvanceIsK = false;

//        private bool? mIsAbanicoRotonda = null;

//        private eRoadPreferencias? mRoadPreferencias = null;
     


//        public oTrDesignAbaInicio(bool iEstGen,int iTrOrden, int iTrAbaPos, oP3d iP1, double iTrAziGrados,bool iTrPrevioIsAbanicoAvance, double iTrPrevioLon,int? iDesviacionMaximaLonPorCiento, double? iAngGrTramoPrevio, double iProRadioMin, double iProLonAvanMin, bool iProLonAvanceIsK, oP2d iPtoDestino, bool iInvalidarTramosAbaPriLon, eRoadPreferencias iRoadTipo)
//        {
//            pEstGenerar = iEstGen;
//            pOrden = iTrOrden;
//            pIsAbanicoAvance = false;
//            P1 = iP1;
//            pTrAbanicoID = 1;
//            pTrAbanicoPos = iTrAbaPos;
//            pName = "ABA1;T" + iTrOrden.ToString() + ";[" + iTrAbaPos.ToString() + "]"; 
//            pPtoDestino = iPtoDestino;

//            mRoadPreferencias = iRoadTipo;

//            mTrAziGrados = iTrAziGrados;
//            mAngGradosTrPrevioActual = iAngGrTramoPrevio;
//            mTrPrevioIsAbanicoAvance = iTrPrevioIsAbanicoAvance;
//            mTrPrevioLon = iTrPrevioLon;
//            mDesviacionMaximaLonPorCiento = iDesviacionMaximaLonPorCiento;
//            mInvalidarTramosAbaPrimarioLon = iInvalidarTramosAbaPriLon;
           

//            mProRadioMin = iProRadioMin;
//            mProLonAvanceMin = iProLonAvanMin;
//            mProLonAvanceIsK = iProLonAvanceIsK;

//            if (mAngGradosTrPrevioActual == null)
//            {
//                mIsAbanicoRotonda = true;
//            }
//            else
//            {

//                mIsAbanicoRotonda = false;
//            }



//        }


//        public override void getXY()
//        {
//            //Obtener las Coordenadas del P2
//            oP2d myP2 = oToolTadilFunciones.getP2XyFromAbanico(P1X.Value, P1Y.Value, mTrAziGrados.Value, mAngGradosTrPrevioActual,mIsAbanicoRotonda.Value);

//            //Determino si las Coordenadas del P2 estan dentro del Contorno
//            bool myP1IsIn = oTadil.data.Proyecto.Terreno.isPtoInsideTerreno(myP2);

//            if (myP1IsIn)
//            {
//                P2.X = myP2.X.Value;
//                P2.Y = myP2.Y.Value;
//                pTramoValido = true;
//            }
//            else
//            {
//                pTramoValido = false;
//                pTramoError = eTramoError.puntoExteriorSuperficie;
//                return;
//            }


//            if (mInvalidarTramosAbaPrimarioLon.HasValue && mInvalidarTramosAbaPrimarioLon.Value  &&  ! mIsAbanicoRotonda.Value &&  ! mTrPrevioIsAbanicoAvance.Value)
//            {
//                //Determino si la Longitud del tramo previo y el actual son compatibles
//                double myLonTr = oToolTrigo.getDisXyTwoPoints(P1.XY, myP2.XY).Value;

//                bool myLonTrIsOk = oToolTadilFunciones.getTramoAbanicoPrimarioIsOK(mDesviacionMaximaLonPorCiento.Value, myLonTr, mTrPrevioLon.Value);

//                if (!myLonTrIsOk)
//                {
//                    pTramoValido = false;
//                    pTramoError = eTramoError.LongitudMinimaAbanicoPrimario;
//                    return;
//                }
             
//            }
            
//        }

//    }
//}
