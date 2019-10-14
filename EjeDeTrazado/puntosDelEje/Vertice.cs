using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

namespace EjeDeTrazado.puntosDelEje
{
    [Serializable]
    public class Vertice
    {
        private double mPuntoIniX;
        private double mPuntoIniY;
        private double mPuntoIniZ;
        private double mAzimut;
        private EjeTrazado.tipoCurva mtipocurva;
        private double mRadioR;
        private EjeTrazado.sentidoCurva mSentCurva;
        private EjeTrazado.tipoSegmento mTipoSeg;
        private double mDelta;
        private double mCentroX;
        private double mCentroY;

        public Vertice(Punto3d iPuntoIni, double iAz, EjeTrazado.sentidoCurva iSent, double iRadioReducido, EjeTrazado.tipoCurva iTipoC, EjeTrazado.tipoSegmento iTipoS, double iDelta, Punto3d iCentro)
        {
            mPuntoIniX = iPuntoIni.coordenadaX;
            mPuntoIniY = iPuntoIni.coordenadaY;
            mPuntoIniZ = iPuntoIni.coordenadaZ;
            mAzimut = iAz;
            mtipocurva = iTipoC;
            mRadioR = iRadioReducido;
            mSentCurva = iSent;
            mTipoSeg = iTipoS;
            mDelta = iDelta;
            mCentroX = iCentro.coordenadaX;
            mCentroY = iCentro.coordenadaY;
        }

        public double getDelta
        {
            get
            {
                return mDelta;
            }
        }

        public Punto3d getVertice
        {
            get
            {
                return new Punto3d(mPuntoIniX, mPuntoIniY, 0);
            }
        }

        public double getAzimut
        {
            get
            {
                return mAzimut;
            }
        }

        public double setAzimut
        {
            set
            {
                mAzimut = value;
            }
        }

        public double getRadioR
        {
            get
            {
                return mRadioR;
            }
        }

        public Punto3d getCentro
        {
            get
            {
                return new Punto3d(mCentroX, mCentroY, 0);
            }
        }

        public Punto3d setCentro
        {
            set
            {
                mCentroX =value.coordenadaX;
                mCentroY = value.coordenadaY;
            }
        }

        public EjeTrazado.tipoCurva getTipocurva
        {
            get
            {
                return mtipocurva;
            }
        }

        public EjeTrazado.sentidoCurva getSentCurva
        {
            get
            {
                return mSentCurva;
            }
        }

        public EjeTrazado.tipoSegmento getTipoSeg
        {
            get
            {
                return mTipoSeg;
            }
        }

        public EjeTrazado.tipoSegmento setTipoSeg
        {
            set
            {
                mTipoSeg = value;
            }
        }

        public double setRadio
        {
            set
            {
                mRadioR = value;
            }
        }
        public double setDelta
        {
            set
            {
                mDelta = value;
            }
        }

        public double getPuntoX
        {
            get
            {
                return mPuntoIniX;
            }
        }

        public double getPuntoY
        {
            get
            {
                return mPuntoIniY;
            }
        }

        public double getPuntoZ
        {
            get
            {
                return mPuntoIniZ;
            }
        }

        public EjeTrazado.tipoCurva setTipoCurva
        {
            set
            {
                mtipocurva = value;
            }
        }

        public EjeTrazado.sentidoCurva setSentidoCurva
        {
            set
            {
                mSentCurva = value;
            }
        }

    }
}
