//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{
//   public class oDataAlgAba
//    {


//        private double? mAbanicoAperturaGrados = null;
//        private int? mAbanicoTramosNum = null;
//        private double? mDesviacionPtoTargetPorCiento = null;


//        private double?             mValoracionDisMinPorCiento              = null;
//        private double?             mValoracionCostePorCiento               = null;
//        private double?             mValoracionSlopePorCiento               = null;

//        private bool?               mAbanicoAvance                          = null;
//        private int?                mDesviacionMaxLonAbaPrimarios           = null;
//        private bool?               mInvalidarTramosLonAbaPrimarios         = null;
 
       
       
//       //Constructor
//        public oDataAlgAba(double iAbaTotalGrados, 
//                           double iAbaDiscreGrados, 
//                           double iValoracionDisPorCiento, 
//                           double iValoracionCostePorCiento, 
//                           double iValoracionSlopePorCiento,           
//                           bool iAbanicoAvance, 
//                           int iDesviacionPorCientoLonTramosAbaPrimarios, 
//                           double iToleranciaPtoTargetAmin, 
//                           bool iInvalidarTramosAbaPrimarioLon)
//        {
//            mAbanicoAperturaGrados = iAbaTotalGrados;
//            mAbanicoTramosNum = getTramosNumByAbanico(iAbaDiscreGrados);
//            mValoracionDisMinPorCiento = iValoracionDisPorCiento;
//            mValoracionCostePorCiento = iValoracionCostePorCiento;
//            mValoracionSlopePorCiento = iValoracionSlopePorCiento;
//            mAbanicoAvance = iAbanicoAvance;
//            mDesviacionMaxLonAbaPrimarios = iDesviacionPorCientoLonTramosAbaPrimarios;
//            mInvalidarTramosLonAbaPrimarios = iInvalidarTramosAbaPrimarioLon; 
//            mDesviacionPtoTargetPorCiento = iToleranciaPtoTargetAmin;
//        }


//       //Datos Algoritmo
//        public  double pAbanicoGradoTotal
//        {
//            get
//            {
//                if (mAbanicoAperturaGrados.HasValue)
//                {
//                    return mAbanicoAperturaGrados.Value;
//                }
//                else
//                {
//                    throw new Exception("Valor Abanico Grado Total es Nulo");
//                }
//            }

//            set
//            {

//                mAbanicoAperturaGrados = value;

//            }
//        }
//        public  double pAbanicoDiscreGrados
//        {
//            get
//            {
//                if (mAbanicoAperturaGrados.HasValue && mAbanicoTramosNum.HasValue && mAbanicoTramosNum.Value !=0)
//                {
//                    return (mAbanicoAperturaGrados.Value / mAbanicoTramosNum.Value);
//                }
//                else
//                {
//                    throw new Exception("Valor Abanico Grado Discretización Total es Nulo");
//                }
//            }

//        }
//        public  double pValoracionDisMinPorCiento
//        {
//            get
//            {
//                if (mValoracionDisMinPorCiento.HasValue)
//                {
//                    return mValoracionDisMinPorCiento.Value;
//                }
//                else
//                {
//                    throw new Exception("Valoración Distancia Mínimo es Nulo");
//                }
//            }

//            set
//            {

//                mValoracionDisMinPorCiento = value;

//            }
//        }
//        public  double pValoracionCostePorCiento
//        {

//            get
//            {
//                if (mValoracionCostePorCiento.HasValue)
//                {
//                    return mValoracionCostePorCiento.Value;
//                }
//                else
//                {
//                    throw new Exception("Valoración por Coste es Nulo");
//                }
//            }

//            set
//            {

//                mValoracionCostePorCiento = value;

//            }
//        }
//        public double pValoracionSlopePorCiento
//        {

//            get
//            {

//                if (mValoracionSlopePorCiento.HasValue)
//                {
//                    return mValoracionSlopePorCiento.Value;
//                }
//                else
//                {
//                    throw new NullReferenceException("mValoracionSlopePorCiento");
//                }
            
//            }

//            set
//            {
//                mValoracionSlopePorCiento = value;           
//            }
             
//        }

//        /// <summary>
//        /// SE GENERAN ABANICOS DE AVANCE
//        /// </summary>
//        public bool pAbanicoAbanceGenerar
//        {

//            get
//            {

//                if (mAbanicoAvance.HasValue)
//                {
//                    return mAbanicoAvance.Value;
//                }
//                else
//                {
//                    throw new Exception("Valor del Abanico Avance es Nulo");
                
//                }
//            }
        
//        }

//       /// <summary>
//       /// % DESVIACIÓN MAXIMA LONGITUDES TRAMOS ABANICOS PRIMARIOS
//       /// </summary>
//        public int? pDesMaxLonAbaPrimariosPorCiento
//        {

//            get
//            {
//                if (mDesviacionMaxLonAbaPrimarios.HasValue)
//                {
//                    return mDesviacionMaxLonAbaPrimarios.Value;
//                }
//                else
//                {
//                    return null;
//                }
            
            
//            }
        
        
//        }

//       /// <summary>
//       /// INVALIDAR TRAMOS POR DIFERENCIA DE LONGITUDES ENTRE ABANICOS PRIMARIOS
//       /// </summary>
//        public bool pInvalidarTramosLonAbaPrimarios
//        {

//            get
//            {

//                if (mInvalidarTramosLonAbaPrimarios.HasValue)
//                {
//                    return mInvalidarTramosLonAbaPrimarios.Value;
//                }
//                else
//                {
//                    throw new NullReferenceException("Invalidar Tramos Longitud Abanicos Primario");
//                }
            
//            }
        
        
//        }


//       /// <summary>
//       /// Desviación Distancia para Obtener el Pto Target
//       /// </summary>
//        public double pDesPtoTargetPorCiento
//        {


//            get
//            {
//                if (mDesviacionPtoTargetPorCiento.HasValue)
//                {

//                    return mDesviacionPtoTargetPorCiento.Value;
//                }
//                else
//                {

//                    throw new ArgumentNullException("% Desviación Pto Target");
                
//                }
            
            
            
//            }
        
        
//        }

//        /// <summary>
//        /// NUMERO DE TRAMOS POR ABANICO
//        /// </summary>
//        public  int    pAbanicoTramosNum
//        {
//            get
//            {
//                if (mAbanicoTramosNum.HasValue)
//                {
//                    return mAbanicoTramosNum.Value;
//                }
//                else
//                {
//                    throw new Exception("El valor del número de Tramos por Abanico es Nulo");
//                }        
//            }
        
//        }
//        public bool    pDrawAbanico {get; set;}



//       //Metodos
//        /// <summary>
//        /// Obtener el número de Tramos por Abanico.
//        /// Debe ser un número IMPAR, el Tr0 Apunta al Objetivo
//        /// </summary>
//        /// <param name="iAbaTotalGrados">Grados Total del Abanico</param>
//        /// <param name="iAbaIntervaloGrados">Intervalo Grados entre Tramos</param>
//        /// <returns></returns>
//        private int? getTramosNumByAbanico(double iAbanicoDiscreGrados)
//        {

//            //Determino el número de tramos
//            int? myTramosInt = (int)Math.Truncate(mAbanicoAperturaGrados.Value/ iAbanicoDiscreGrados);

//            if (myTramosInt.HasValue)
//            {
//                if (myTramosInt % 2 == 0) //Es Par
//                {
//                    return myTramosInt = myTramosInt.Value + 1;
//                }
//                else
//                {
//                    return myTramosInt.Value;
//                }
//            }
//            else
//            {
//                return null;           
//            }


//        }

//    }
//}
