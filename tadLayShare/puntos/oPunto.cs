using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayShare.puntos
{

    public interface IP2d
    {
        double X { get; set; }
        double Y { get; set; }
        double distTo2d(IP2d iPto);
        double[] toArray2d();
  
    }
    public interface IP3d : IP2d
    {
        double Z { get; set; }
        double distTo3d(IP3d iPto);
        double[] toArray3d();
    }

    public class oP2d : IP2d
    {
        #region "Campos Privados"
        private double? mX = null;
        private double? mY = null;
        #endregion
        #region "Constructores"

        public oP2d()
        {

        }
        public oP2d(double? iX, double? iY)
        {
            mX = iX;
            mY = iY;
        }


        public oP2d (double[] iPtos)
        {
            mX = iPtos[0];
            mY = iPtos[1];
        }

        #endregion



        #region "Propiedades"

        public double X
        {
            get
            {
                if (mX.HasValue)
                {
                    return mX.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("X");
                }

            }

            set
            {
                mX = value;
            }
        }
        public double Y
        {

            get
            {
                if (mY.HasValue)
                {
                    return mY.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Y");
                }

            }

            set
            {
               mY = value;
            }
        }
        public bool isNull
        {
            get
            {
                if (this.mX.HasValue && this.mY.HasValue)
                {
                    return false;
                }
                else
                {
                    return true;
                }  
            }
        }
        #endregion
        #region "Metodos Publicos"
        /// <summary>
        /// Distancia al Punto
        /// </summary>
        public double distTo2d(IP2d iPto)
        {   
          return Math.Pow(Math.Pow((iPto.X - mX.Value), 2) + Math.Pow((iPto.Y - mY.Value), 2), 0.5);  
        }
        /// <summary>
        /// Angulo Respecto a 2 Puntos (Devuelve Angulo Menor o Igual a 180)
        /// </summary>
        public double angleFrom3Points (oP2d iP1, oP2d iP2, eAng iAngFormato)
        {

            if (!this.isNull && !iP1.isNull && !iP2.isNull)
            {
                return oTrigo.getAngFrom3Point(this, iP1, iP2, iAngFormato);
            }
            else
            {
                throw new Exception("Punto Valor Nulo");

            }

        }
        /// <summary>
        /// Azimut Respecto Punto
        /// </summary>
        public double azimut(oP2d iPunto, eAng iAnguloFormato)
        {
            if (!this.isNull && !iPunto.isNull)
            {
                return oTrigo.getAzimutGrados(this, iPunto);
            }
            else
            {
                throw new Exception("Punto Valor Nulo");

            }

        }
        /// <summary>
        /// Obtener Punto Dado AzimutGrados y Longitud
        /// </summary>
        public oP2d getPointFromAzimutAndLongitud(double iAzimutGrados, double iLongitud)
        {
            if (!this.isNull)
            {
                return oTrigo.getP2FromAzimutLon(this, iAzimutGrados, iLongitud);
            }
            else
            {
                throw new NullReferenceException();  
            }
        }
        /// <summary>
        /// Convertir Punto 2D - 3D
        /// </summary>
        public IP3d convertTo3d(double? iZ)
        {
            return new oP3d(this.X, this.Y,iZ);
        }
        /// <summary>
        /// Convertir Punto 2D - To Array (Trabajar Autocad)
        /// </summary>
        /// <returns></returns>
        public double[] toArray2d()
        {
            return new double[2] { this.X, this.Y};
        }


        public override string ToString()
        {
            return ("x= " + Math.Round(X,2) +
                    " ; " +
                    "y= " + Math.Round(Y,2));
                 
        }

        #endregion

    }

    public class oP3d : oP2d, IP3d
    {


        private double? mZ = null;


        #region "Connstructores"
        public oP3d()
        {
        }

        public oP3d(double[] iPtos)
        {
            this.X = iPtos[0];
            this.Y = iPtos[1];
            this.Z = iPtos[2];
        }
        public oP3d(double? iX, double? iY)
            : base(iX, iY)
        {

            mZ = null;
        }

        public oP3d(double? iX, double? iY, double? iZ)
            : base(iX, iY)
        {

            mZ = iZ;
        }

        #endregion



        #region "Propiedades"

        public double Z
        {
            get
            {
              if (mZ.HasValue)
                {
                    return mZ.Value;
               }
               else
              {
                   throw new oExPropertieNullValue("Z");
              }
            }

            set
            {
                mZ = value;
            }
        }

        #endregion


        #region "Metodos"

        public double getPendienteConSignoPC (IP3d iPto)
        {
            return oTrigo.getPendiente3D(this, iPto, ePorcentaje.porCiento);
        }



        public double distTo3d(IP3d iPto)
        {
            return Math.Pow(Math.Pow((iPto.X - this.X), 2) + Math.Pow((iPto.Y - this.Y), 2) + Math.Pow((iPto.Z - this.Z), 2), 0.5);
        }
        public oP2d convertTo2d()
        {
            return new oP2d(this.X, this.Y);
        }
        public double[] toArray3d()
        {
            return new double[3] { this.X, this.Y, this.Z };
        }
        public double[] toArray3dZcero()
        {
            return new double[3] { this.X, this.Y, 0 };
        }

        #endregion


        public override string ToString()
        {
            return ("x= " + Math.Round(X,2)+ " ; y=" + Math.Round(Y,2) + " ; z = " + Math.Round(Z,2));
        }
    }

    public class oP3dSalidaLlegada : oP3d
    {

        private double? mAziGrados = null;
        private double? mLongitud = null;
        private double? mPendientePC = null;

        private bool? mIsLonMinimaRecta = null;


        #region "Constructor"

        public oP3dSalidaLlegada(double iX, double iY, double iZ, double? iAziGrados, double? iLongitud, double? iPendientePC, bool? iIsLonMinimaRecta)
            : base(iX, iY, iZ)
        {
            mAziGrados = iAziGrados;
            mLongitud = iLongitud;
            mPendientePC = iPendientePC;
            mIsLonMinimaRecta = iIsLonMinimaRecta;
        }

        public oP3dSalidaLlegada(double iX, double iY, double iZ)
            : base(iX, iY, iZ)
        {

        }


        #endregion


        #region "Propiedades"
        /// <summary>
        /// Azimut en GRADOS
        /// </summary>
        public double? azimutGrados
        {

            get
            {
                if (mAziGrados.HasValue)
                {
                    return mAziGrados.Value;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                mAziGrados = value;
            }
        }
        /// <summary>
        /// Longitud Tramo
        /// </summary>
        public double? longitud
        {
            get
            {
                if (mLongitud.HasValue)
                {
                    return mLongitud.Value;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                mLongitud = value;
            }
        }
        /// <summary>
        /// Pendiente Tramo %100
        /// </summary>
        public double? pendientePC
        {

            get
            {
                if (mPendientePC.HasValue)
                {
                    return mPendientePC.Value;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                mPendientePC = value;
            }

        }
        /// <summary>
        /// Consideramos Lon Salida Llegada Recta
        /// (True ; LSalida = Lon + Amin)
        /// (False ; LSalida = Lon f(Amin))
        /// </summary>
        public bool? isLonMinimaRecta
        {

            get
            {
                if (mIsLonMinimaRecta.HasValue)
                {
                    return mIsLonMinimaRecta.Value;
                }
                else
                {
                    throw new Exception("El Valor de Longitud Minima Recta es Nulo");
                }

            }

        }
        /// <summary>
        /// Pto Salida es Rotonda
        /// </summary>
        public bool isRotondaSinPendiente
        {
            get
            {
            
            if (this.azimutGrados == null && this.longitud == null && this.pendientePC==null)
            {
                return true;
            }
            else
            {
                return false;
            }
            }
        }
        /// <summary>
        /// Salida Rotonda Con Pendiente
        /// </summary>
        /// <returns></returns>
        public bool isRotondaConPendienteK
        {
            
            get
            {
            
            if (this.azimutGrados == null && this.longitud == null && this.pendientePC != null)
            {
                return true;
            }
            else
            {
                return false;
            }

            }
        }
        /// <summary>
        /// LA PENDIENTE ESTA DEFINIDA
        /// </summary>
        public bool isDefinidaPendiente
        {
           get
            {
                if (this.pendientePC == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }


            }



        }

        #endregion

    }

}