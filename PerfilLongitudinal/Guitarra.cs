using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

namespace PerfilLongitudinal
{
    public class Guitarra
    {
        private double mPuntoOrigX;
        private double mPuntoOrigY;

        private double mMaxX;
        private double mMaxY;
        private double mMinX;
        private double mMinY;
        private double mMaxXEje;
        private double mMaxPkEje;


        private int mCuadradosAlto;
        private int mCuadradosAncho;

        private int mEscalaAncho;
        private int mEscalaAlto;

        public Guitarra(double minX, double maxX, double minZ, double maxZ, Punto3d iPuntoOrigen, int escalaAncho, int escalaAlto)
        {
            mMaxPkEje = maxX;
            Punto3d miPuntoOrigen = iPuntoOrigen;
            mPuntoOrigX = miPuntoOrigen.coordenadaX;
            mPuntoOrigY = miPuntoOrigen.coordenadaY;

            mEscalaAncho = escalaAncho;
            mEscalaAlto = escalaAlto;

            mMaxX = maxX;
            mMinX = minX;
            mMaxY = maxZ;
            mMinY = minZ;


            //ancho de la cuadricula. ¿hay que definir una escala?
            double difX = maxX-minX;
            mCuadradosAncho = (int) (difX/escalaAncho)+1;

            //alto cuandricula
            double difZ = maxZ-minZ;
            mCuadradosAlto = (int) (difZ/escalaAlto)+6;

            mMaxX = mPuntoOrigX + mCuadradosAncho * 100;
            mMaxY = mPuntoOrigY + mCuadradosAlto * 100;
            mMaxXEje = mPuntoOrigX + mMaxPkEje;

        }

        public double getPuntoOrigX
        {
            get
            {
                return mPuntoOrigX;
            }
        }
        public double getPuntoOrigY
        {
            get
            {
                return mPuntoOrigY;
            }
        }
        public double getMaxX
        {
            get
            {
                return mMaxX;
            }
        }

        public double getMaxXEje
        {
            get
            {
                return mMaxXEje;
            }
        }

        public double getMaxPKX
        {
            get
            {
                return mMaxPkEje;
            }
        }
        public double getMaxY
        {
            get
            {
                return mMaxY;
            }
        }
        public double getMinX
        {
            get
            {
                return mMinX;
            }
        }
        public double getMinY
        {
            get
            {
                return mMinY;
            }
        }

        public int getEscalaAncho
        {
            get
            {
                return mEscalaAncho;
            }
        }

        public int getEscalaAlto
        {
            get
            {
                return mEscalaAlto;
            }
        }
    }
}
