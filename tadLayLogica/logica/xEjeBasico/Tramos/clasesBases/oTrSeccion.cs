//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{
    
//    using tadLayShare;
//    using tadLayShare.puntoOld;

   
//    public class oTramoSeccion
//    {

//        private int?  mId = null;
//        private oP3d mPto = null;
//        private double? mLon = null;
//        private double? mZterreno = null;
//        private string mZonaGeotecnia = string.Empty;


//        private eApoyo? mApoyo = null;
//        private double? mApoyoCoste = null;


//        private double? mSlope = null;


//        public int pId
//        {
//            get
//            {
//                if (mId.HasValue)
//                {
//                    return mId.Value;
//                }
//                else
//                {
//                    throw new Exception("El Valor del ID es nulo");
//                }

//            }
//            set
//            {

//                mId = value;

//            }
//        }
//        public oP3d pPto
//        {

//            get
//            {
//                return mPto;
//            }

//            set
//            {

//                mPto = value;

//            }


//        }
//        public double pLon
//        {

//            get
//            {
//                if (mLon.HasValue)
//                {
//                    return mLon.Value;
//                }
//                else
//                {
//                    throw new Exception("El Valor de la Longitud de la Estructura es nulo");
                
//                }
//            }

//            set
//            {
//                mLon = value;
//            }
        
//        }
//        public double? pZterreno
//        {


//            get
//            {

//                if (mZterreno.HasValue)
//                {

//                    return mZterreno.Value;
//                }

//                else
//                {

//                    throw new Exception("El Valor de Z es Nulo");
//                }
//            }

//            set
//            {
//                mZterreno = value;     
//            }
//        }
//        public double? pZdesfase
//        {


//            get
//            {

//                if (mPto.Z.HasValue && mZterreno.HasValue)
//                {
//                    return   Math.Round( mPto.Z.Value - mZterreno.Value,2);
//                }

//                {
//                    throw new Exception("El Valor del Desfaze Z es Nulo");
//                }



//            }
        
        
        
//        }
//        public string pZonaGeotecnia
//        {

//            get
//            {

//                if (mZonaGeotecnia != string.Empty)
//                {
//                    return mZonaGeotecnia;
//                }
//                else
//                {
//                    return "ZonaGeneral";

//                }
//            }

//            set
//            {
//                mZonaGeotecnia = value;
//            }


//        }
//        public eApoyo pApoyo
//        {

//            get
//            {
//                if (mApoyo.HasValue)
//                {
//                    return mApoyo.Value;
//                }
//                else
//                {

//                    throw new Exception("El Valor de la Estructura es Nulo.");
                
//                }
            
//            }

//            set
//            {
//                mApoyo = value;
//            }
//        }
//        public double? pApoyoCoste
//        {

//            get
//            {
//                if (mApoyoCoste.HasValue)
//                {
//                    return mApoyoCoste.Value;
//                }
//                else
//                {
//                    return null;                
//                }
            
//            }

//            set
//            {

//                mApoyoCoste = value;
//            }   
//        }

//        /// <summary>
//        /// Pendiente del Terreno en ese Punto [Valor 0-1]
//        /// </summary>
//        public double pSlope
//        {

//            get
//            {

//                if (mSlope.HasValue)
//                {
//                    return mSlope.Value;
//                }
//                else
//                {
//                    throw new NullReferenceException("mSlope");
//                }
//            }


//            set
//            {
//                mSlope = value;
//            }
//        }



//        public eExcavacion? pTerrenoCorrecion
//        {

             
//            get
//            {

//                if (pZdesfase.HasValue)
//                {
//                    if (pZdesfase.Value == 0)
//                    {
//                        return eExcavacion.acota;
//                    }
//                    else if (pZdesfase.Value < 0)
//                    {
//                        return eExcavacion.desmonte;
//                    }
//                    else
//                    {
//                        return eExcavacion.terraplen;
//                    }
//                }
//                else
//                {

//                    return null; ;

//                }
//            }

//        }
//    }

//}
