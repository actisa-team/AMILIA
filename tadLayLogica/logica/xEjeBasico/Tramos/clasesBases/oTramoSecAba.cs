//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{

//    using tadLayShare;
//    using tadLayShare.puntoOld;

//    //CLASE TRAMOS DEL ABANICO
//    public abstract class oTramoSecAba  : oTramoSec
//    {

//        /// <summary>
//        /// Punto Target al cual se Orienta el Abanico
//        /// </summary>
//        private oP2d            mPtoDestino                 = null;


//        /// <summary>
//        /// ID del Abanico 1,2,3 ; Nunca 0
//        /// </summary>
//        private int?            mAbaId                      = null;

//        /// <summary>
//        /// Posicion del Abanico -18 hasta 18
//        /// </summary>
//        private int?            mAbaPos                     = null;


//        /// <summary>
//        /// Distancia del Tramo desde el Origen del Abanico
//        /// </summary>
//        /// <remarks>
//        /// Es la Distancia del Tramo mas el de sus Tramos Predecesores dentro del abanico y misma Posicion
//        /// </remarks>
//        private double?         mAbaDistAlOrigen     = null;

//        /// <summary>
//        /// Coste del Tramo desde el Origen del Abanico
//        /// </summary>
//        /// <remarks>
//        /// Es el Coste del Tramo mas el de sus Tramos Predecesores dentro del abanico y misma Posicion
//        /// </remarks>
//        private double?         mAbaCosteAlOrigen    = null;


//        private double?         mValorTotal                 = null;

//        private double?         mValorDisMin                = null;
//        private double?         mValorCos                   = null;
//        private double?         mValorSlope                 = null;



//        public oTramoSecAba()
//        {


//        }


//        public void gValorarTramo(double iValorDisPorCiento, double iValorCostePorCiento, double iValorSlopePorCiento,
//                                  double iDisMin, double iDisMax, double iCosteMin, double iCosteMax,
//                                  bool iAvanceLargos)
//        {

//            if (pTramoValido != null && pTramoValido.Value)
//            {

//                if (! iAvanceLargos) //Soluciones Avances Cortos
//                {
//                    mValorDisMin = oToolTadilFunciones.getValoracion(pDisToDestinoMasLonTramo.Value, iDisMin, iDisMax);
//                }
//                else  //Soluciones Abanicos Largos
//                {
//                    mValorDisMin = oToolTadilFunciones.getValoracion(pDisP2toDestino.Value, iDisMin, iDisMax);
//                }
                
           
//                mValorCos = oToolTadilFunciones.getValoracion(pCosteMl.Value, iCosteMin, iCosteMax);

//                mValorSlope = oToolTadilFunciones.getSlopeMedioTramo(pLstTramoSeccion); 

//                mValorTotal = oToolTadilFunciones.getValoracionTotal(mValorDisMin.Value, mValorCos.Value, mValorSlope.Value,
//                                                                     iValorDisPorCiento, iValorCostePorCiento,iValorSlopePorCiento);
//            }
//            else
//            {

//                mValorDisMin = null;
//                mValorCos =   null;
//                mValorSlope = null;
//                mValorTotal = null;
//            }
//        }


//        public void drawData(string iLayer)
//        {

             
            
//              string myData = string.Empty;

//              try
//              {

//                  if (pTramoValido.HasValue && pTramoValido.Value)
//                  {

//                      base.draw(iLayer);
                      
//                      myData = pName +
//                    " ; Estructura " + pEstHas.Value.ToString() + 
//                    " ; LonDestino " + pDisP2toDestino.Value.ToString() +
//                    " ; LonDestino+LonTramo " + pDisToDestinoMasLonTramo.Value.ToString() +
//                    " ; LonOrigenAbanico " + pDistanciaOrigenAbanico.ToString()+
//                    " ; CosteOrigenAbanico " + pCosteTotalOrigenAbanico.ToString() +
//                    " ; Coste Tramo " + pCoste.Value.ToString() +
//                    " ; Coste Ml " + pCosteMl.Value.ToString() +
//                    " ; " + pValoracionStr;

//                      engCadNet.oLine.addLine3dVoid(P1X.Value, P1Y.Value, P1Z.Value, P2X.Value, P2Y.Value, P2Z.Value, iLayer);
//                      engCadNet.oText.addText2D(myData, P2X.Value, P2Y.Value-0.75, 0.5, 0, iLayer);
//                  }
//                  else
//                  {
//                      if (pTramoError != eTramoError.puntoExteriorSuperficie)
//                      {

//                          myData = pName + 
//                          "Error  = " + pTramoError.ToString();
//                          engCadNet.oLine.addLine2dVoid(P1X.Value, P1Y.Value, P2X.Value, P2Y.Value, iLayer);
//                          engCadNet.oText.addText2D(myData, P2X.Value + 2, P2Y.Value, 0.5, 0, iLayer);

//                      }
//                  }
//              }
//              catch (Exception)
//              {
                  
//                  throw;
//              }
            
            
                    
//            }
        
        
        
             

//        #region "Propiedades"


//        public double pDistanciaOrigenAbanico
//        {

//            get
//            {

//                if (mAbaDistAlOrigen.HasValue)
//                {

//                    return Math.Round(mAbaDistAlOrigen.Value, 2);
//                }
//                else
//                {
//                    throw new Exception("El Valor de la Longitud del Tramo Previo es Nulo");
                
//                }
//            }

//            set
//            {

//                mAbaDistAlOrigen = value;
//            }
        
        
        
//        }
//        public double pCosteTotalOrigenAbanico
//        {

//            get
//            {

//                if (mAbaCosteAlOrigen.HasValue)
//                {
//                    return Math.Round(mAbaCosteAlOrigen.Value, 2);
//                }
//                else
//                {
//                    throw new Exception("El Valor del Coste del Tramo Previo es Nulo");
//                }
//            }

//            set
//            {
//                mAbaCosteAlOrigen = value;
//            }



//        }
//        public override double? pCosteMl
//        {
//            get
//            {

//                return Math.Round( (pCosteTotalOrigenAbanico / pDistanciaOrigenAbanico),2);
                  
            
//            }

//        }


//        public oP2d pPtoDestino
//        {

//            get
//            {
//                return mPtoDestino;
//            }

//            set
//            {
//                mPtoDestino = value;
//            }
        
        
//        }
   
//        /// <summary>
//        /// Posicion del Tramo dentro del Abanico
//        /// +18,-18
//        /// </summary>
//        public int? pTrAbanicoPos
//        {

//            get
//            {
//                if (mAbaPos.HasValue)
//                {
//                    return mAbaPos.Value;
//                }
//                else
//                {
//                    return null;
//                }
            
//            }


//            set
//            {
//                mAbaPos = value;
            
//            }
        
        
        
//        }

       
//        public double? pDisToDestinoMasLonTramo
//        {

//            get
//            {

//                if (P2.X.HasValue && P2.Y.HasValue && mPtoDestino.X.HasValue && mPtoDestino.Y.HasValue && pTramoValido.HasValue)
//                {
//                    if (pTramoValido.Value)
//                    {
//                        return Math.Round((pDis2d.Value + pDisP2toDestino.Value),3) ;
//                    }
//                }

//                return null;
//            }

//        }
//        public double? pDisP2toDestino
//        {
//            get
//            {
//                if (P2.X.HasValue && P2.Y.HasValue && mPtoDestino.X.HasValue && mPtoDestino.Y.HasValue && pTramoValido.HasValue && pTramoValido.Value)
//                {
                   
                    
                    
//                    return Math.Round(oToolTrigo.getDisXyTwoPoints(P2.X, P2.Y, mPtoDestino.X, mPtoDestino.Y).Value,3);
                   
//                }

//                return null;
//            }

//        }

//        /// <summary>
//        /// ID del Abanico al que pertenece el Tramo
//        /// Siempre => 1
//        /// </summary>
//        public int? pTrAbanicoID
//        {

//            get
//            {

//                if (mAbaId.HasValue)
//                {
//                    return mAbaId.Value;
//                }
//                else
//                {
//                    throw new Exception("El Valor del Id del Abanico del Tramo es Nulo.");
                
//                }
            
//            }


//            set
//            {

//                if (value == 0)
//                {
//                    throw new Exception("El Valor del Id del Abanico No puede Ser Cero.");
//                }
//                else
//                {
//                    mAbaId = value;
//                }
            
//            }
        
        
//        }
//        /// <summary>
//        /// Valoracion por Distancia Minima al Objetivo
//        /// </summary>
//        public double? pValorDisMin
//        {
//            get
//            {

//                if (mValorDisMin.HasValue)
//                {
//                    return Math.Round(mValorDisMin.Value,3);
//                }
//                else
//                {
//                    return null;
//                }
//            }
//        }
//        /// <summary>
//        /// Valoracion por Coste del Tramo
//        /// </summary>
//        public double? pValorCoste
//        {
//            get
//            {

//                if (mValorCos.HasValue)
//                {
//                   return mValorCos.Value;
//                }
//                else
//                {
//                    return null;
//                }
//            }
//        }

//        /// <summary>
//        /// Valoracion de la Pendiente Media del Tramo
//        /// </summary>
//        public double? pValorPendiente
//        {

//            get
//            {

//                if (mValorSlope.HasValue)
//                {
//                  return  mValorSlope.Value;
//                }
//                else
//                {
//                    return null;
//                }
//         }
        
        
        
//        }

//        /// <summary>
//        /// Valoracion Total (Función % Usuario)
//        /// </summary>
//        public double? pValorTotal
//        {

//            get
//            {

//                if (mValorTotal.HasValue)
//                {
//                    return mValorTotal.Value;
//                }
//                else
//                {
//                    return null;
//                }


//            }


//        }
//        public string pValoracionStr
//        {
//            get
//            {
//                if (pTramoValido.HasValue && pTramoValido.Value && pValorTotal.HasValue && pValorDisMin.HasValue && pValorCoste.HasValue)
//                {
//                    return "Total = " + pValorTotal.ToString() + "(Dis=" + pValorDisMin + "Coste Ml= " + pValorCoste.ToString() + ")";
//                }
//                else
//                {
//                    return string.Empty;
//                }
          
//            }
    
//        }


//        #endregion
//    }
//}
