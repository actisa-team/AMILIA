//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{

//    using tadLayShare;
//    using tadLayShare.puntoOld;

//    public abstract class oTramo 
//    {


//        private int?                mOrden                          = null;
//        private string              mName                           = string.Empty;
//        private oP3d                mP1                             = new oP3d();
//        private oP3d                mP2                             = new oP3d();


//        #region "Constructores"
//        public oTramo()
//        {


//        }

//        public oTramo(int? iTramoOrden, double iP1X, double iP1Y, double iP1Z, double iP2X, double iP2Y, double iP2Z)
//        {
//            mOrden = iTramoOrden;
//            mP1.X = iP1X;
//            mP1.Y = iP1Y;
//            mP1.Z = iP1Z;
//            mP2.X = iP2X;
//            mP2.Y = iP2Y;
//            mP2.Z = iP2Z;
//        }
//        public oTramo(int? iTramoOrden,double iP1X, double iP1Y, double iP1Z)
//        {
//            mOrden = iTramoOrden;
//            mP1.X = iP1X;
//            mP1.Y = iP1Y;
//            mP1.Z = iP1Z;
//        }
//        public oTramo(int? iTramoOrden, oP3d iP1xyz)
//        {
//            mOrden = iTramoOrden;
//            mP1 = iP1xyz;
//        }
        
//        #endregion
//        #region "Propiedades"


//        /// <summary>
//        /// ORDEN DEL TRAMO EN EL CAMINO [1,2,3,4,etc..]
//        /// </summary>
//        public int? pOrden
//        {
//            get
//            {

//                if (mOrden.HasValue)
//                {
//                    return mOrden;
//                }
//                else
//                {
//                    throw new Exception("Valor Orden es Obligatorio");
//                }
//            }

//            set
//            {
//                mOrden = value;
//            }

//        }
//        /// <summary>
//        /// Nombre del Tramo
//        /// </summary>
//        public string pName
//        {
//            get
//            {
//                return mName; 
//            }

//            set

//            {
//                mName = value;          
//            }

//        }
//        public oP3d P1
//        {
//            get { return mP1; }

//            set { mP1 = value; }
//        }
//        public double? P1X
//        {
//            get
//            {
//                return mP1.X;         
//            }     
//        }
//        public double? P1Y
//        {
//            get
//            {
//                return mP1.Y;
//            }
//        }
//        public double? P1Z
//        {
//            get
//            {
//                return mP1.Z;
//            }
//        }
//        public oP3d P2
//        {
//            get { return mP2; }

//            set { mP2 = value; }
//        }
//        public double? P2X
//        {
//            get
//            {
//                return mP2.X;
//            }

//            set
//            {
//                mP2.X = value;
//            }
//        }
//        public double? P2Y
//        {
//            get
//            {
//                return mP2.Y;
//            }

//            set
//            {
//                mP2.Y = value;
//            }
//        }
//        public double? P2Z
//        {
//            get
//            {
//               return mP2.Z;       
//            }

//            set
//            {
//                mP2.Z = value;          
//            }
//        }

//        public double? pDis2d
//        {
//            get
//            {             
//                return oToolTrigo.getDisXyTwoPoints(mP1.X, mP1.Y, mP2.X, mP2.Y); ;
//            }
//        }
//        public double? pAzimutGrados
//        {
//            get
//            {
//                return oToolTrigo.getAzimutGrados(mP1.X, mP1.Y, mP2.X, mP2.Y);
//            }
//        }
//        public double? pPendValorPorCiento
//        {
//            get
//            {
//                return oToolTrigo.getPendientePorCiento(mP1.X, mP1.Y, mP1.Z, mP2.X, mP2.Y, mP2.Z);
//            }
//        }
//        #endregion


//        #region "Metodos"

//        /// <summary>
//        /// Obtener la Coordenada Z, desde el P1
//        /// dada una longitud desde el origen
//        /// </summary>
//        /// <param name="iLon"></param>
//        /// <returns></returns>
//        public double getZFromP1andLon(double iLonFromP1)
//        {
//            return P1Z.Value + ((pPendValorPorCiento.Value / 100) * iLonFromP1);        
//        }
   
//        #endregion


//    }
//}
