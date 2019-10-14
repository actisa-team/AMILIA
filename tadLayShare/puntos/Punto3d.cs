using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayShare.puntos
{
    public class Punto3d
    {
        #region "Variables privadas"

        private double mCoordenadaX;
        private double mCoordenadaY;
        private double mCoordenadaZ;
        private int mIndex;

        #endregion

        #region "Contructores"

        public Punto3d()
        {
        }

        public Punto3d(double ix, double iy, double iz)
        {
            mCoordenadaX = ix;
            mCoordenadaY = iy;
            mCoordenadaZ = iz;
            mIndex = -1;

        }

        public Punto3d(double ix, double iy, double iz, int index)
        {
            mCoordenadaX = ix;
            mCoordenadaY = iy;
            mCoordenadaZ = iz;
            mIndex = index;

        }

        #endregion

        #region "Metodos publicos"


        public int index
        {
            get { return mIndex; }
            set { mIndex = value; }
        }

        public double coordenadaX
        {
            get { return mCoordenadaX; }
            set { mCoordenadaX = value; }
        }

        public double coordenadaY
        {
            get { return mCoordenadaY; }
        }
        public double coordenadaZ
        {
            get { return mCoordenadaZ; }
        }

        public double[] toArray3d
        {
            get
            {
                double[] miArray3d = new double[3];
                miArray3d[0] = coordenadaX;
                miArray3d[1] = coordenadaY;
                miArray3d[2] = coordenadaZ;
                return miArray3d;

            }
        }

        public double distancia(Punto3d iPunto)
        {
            double miDistancia = 0;
            miDistancia = Math.Pow(this.coordenadaX - iPunto.coordenadaX, 2) + Math.Pow(this.coordenadaY - iPunto.coordenadaY, 2) + Math.Pow(this.coordenadaZ - iPunto.coordenadaZ, 2);
            miDistancia = Math.Sqrt(miDistancia);
            return miDistancia;
        }

        public double distancia2d(Punto3d iPunto)
        {
            double miDistancia = 0;
            miDistancia = Math.Pow(this.coordenadaX - iPunto.coordenadaX, 2) + Math.Pow(this.coordenadaY - iPunto.coordenadaY, 2);
            miDistancia = Math.Sqrt(miDistancia);
            return miDistancia;
        }


        public int CompareTo(puntos.Punto3d iPunto)
        {
            int miIgual = -1;
            if ((this.mCoordenadaX == iPunto.mCoordenadaX) && (this.mCoordenadaY == iPunto.mCoordenadaY))
            {
                miIgual = 0;
            }
            else
            {
                if (this.mCoordenadaX < iPunto.mCoordenadaX)
                {
                    miIgual = -1;
                }
                else if (this.mCoordenadaX > iPunto.mCoordenadaX)
                {
                    miIgual = 1;
                }
                else
                {
                    if (this.mCoordenadaY < iPunto.mCoordenadaY)
                    {
                        miIgual = -1;
                    }
                    else
                    {
                        miIgual = 1;

                    }

                }
            }
            return miIgual;
        }
        public string getHashCode
        {
            get { return this.coordenadaX + "_" + this.coordenadaY + "_" + this.coordenadaZ; }
        }

        public bool Equals(Punto3d iPunto)
        {
            bool miResultado = false;
            if (this.CompareTo(iPunto) == 0)
            {
                miResultado = true;
            }

            return miResultado;
        }

        #endregion
    }
}
