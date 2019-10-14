using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

namespace Logica
{
    public class InfoComponente
    {
        EjeDeTrazado.componentes.Componente.tipoComponente mTipo;
        double[] mValoresCurva = new double[5];
        List<Punto3d> mPuntos;
        double mPkIni;
        double mPkFin;
        double mA;
        double mLong;
        private EjeDeTrazado.puntosDelEje.EjeTrazado.tipoSegmento mTipoRecta;
        private EjeDeTrazado.puntosDelEje.EjeTrazado.tipoCurva mTipoCurva;
        private EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva mSentGiro;
        private double mAzimutFinal;

        private double mPeralte;
        private double mMargenD;
        private double mMargenI;
        private double mVariacionMI;
        private double mVariacionMD;
        private Punto3d mPuntoIni;
        private Punto3d mPuntoFin;





        public InfoComponente(double[] iValoresCurva, double iPki, double iPkf, double longitud, Punto3d iPuntoIni, Punto3d iPuntoFin)
        {
            mPuntoIni = iPuntoIni;
            mPuntoFin = iPuntoFin;
            mTipo = componentes.Componente.tipoComponente.curva;
            mValoresCurva = iValoresCurva;
            mPkFin = iPkf;
            mPkIni = iPki;
            mLong = longitud;
        }

        public InfoComponente(EjeDeTrazado.componentes.Componente.tipoComponente iTipo, List<Punto3d> iPuntos, double iPki, double iPkf, double longitud, Punto3d iPuntoIni, Punto3d iPuntoFin)
        {
            mPuntoIni = iPuntoIni;
            mPuntoFin = iPuntoFin;
            mTipo = iTipo;
            mPuntos = iPuntos;
            mPkFin = iPkf;
            mPkIni = iPki;
            mLong = longitud;
        }

        public InfoComponente(EjeDeTrazado.componentes.Componente.tipoComponente iTipo, List<Punto3d> iPuntos, double iPki, double iPkf, double iA, double longitud, Punto3d iPuntoIni, Punto3d iPuntoFin)
        {
            mPuntoIni = iPuntoIni;
            mPuntoFin = iPuntoFin;
            mTipo = iTipo;
            mPuntos = iPuntos;
            mPkFin = iPkf;
            mPkIni = iPki;
            mA = iA;
            mLong = longitud;
        }

        public EjeDeTrazado.componentes.Componente.tipoComponente getTipoComponente
        {
            get
            {
                return mTipo;
            }
        }
        public double[] getValoresCurva
        {
            get
            {
                return mValoresCurva;
            }
        }
        public List<Punto3d> getPolilinea
        {
            get
            {
                return mPuntos;
            }
        }
        public double getPkFinal
        {
            get
            {
                return mPkFin;
            }
        }
        public double getPkInicial
        {
            get
            {
                return mPkIni;
            }
        }
        public double getValorA
        {
            get
            {
                return mA;
            }
        }
        public double getLongitud
        {
            get
            {
                return mLong;
            }
        }
        public double setAzimutFinal
        {
            set
            {
                mAzimutFinal = value;
            }
        }
        public EjeDeTrazado.puntosDelEje.EjeTrazado.tipoSegmento setTipoRecta
        {
            set
            {
                mTipoRecta = value;
            }
        }
        public EjeDeTrazado.puntosDelEje.EjeTrazado.tipoCurva setTipoCurva
        {
            set
            {
                mTipoCurva = value;
            }
        }
        public EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva setSentGiro
        {
            set
            {
                mSentGiro = value;
            }
        }
        public double setPeralte
        {
            set
            {
                mPeralte = value;
            }
        }
        public double setVariacionMI
        {
            set
            {
                mVariacionMI = value;
            }
        }
        public double setVariacionMD
        {
            set
            {
                mVariacionMD = value;
            }
        }
        public double setMargenI
        {
            set
            {
                mMargenI = value;
            }
        }
        public double setMargenD
        {
            set
            {
                mMargenD = value;
            }
        }

        public EjeDeTrazado.puntosDelEje.EjeTrazado.tipoCurva getTipoCurva
        {
            get
            {
               return mTipoCurva;
            }
        }
        public EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva getSentGiro
        {
            get
            {
                return mSentGiro;
            }
        }
        public EjeDeTrazado.puntosDelEje.EjeTrazado.tipoSegmento getTipoRecta
        {
            get
            {
                return mTipoRecta;
            }
        }
        public double getPuntoIniX
        {
            get
            {
                return mPuntoIni.coordenadaX;
            }
        }
        public double getPuntoIniY
        {
            get
            {
                return mPuntoIni.coordenadaY;
            }
        }
        public double getPuntoFinX
        {
            get
            {
                return mPuntoFin.coordenadaX;
            }
        }
        public double getPuntoFinY
        {
            get
            {
                return mPuntoFin.coordenadaY;
            }
        }
        public double getPuntoCentroX
        {
            get
            {
                return mValoresCurva[0];
            }
        }
        public double getPuntoCentroY
        {
            get
            {
                return mValoresCurva[1];
            }
        }
        public double getRadio
        {
            get
            {
                return mValoresCurva[2];
            }
        }
        public double getPeralte
        {
            get
            {
                return mPeralte;
            }
        }
        public double getVariacionMI
        {
            get
            {
               return mVariacionMI;
            }
        }
        public double getVariacionMD
        {
            get
            {
                return mVariacionMD;
            }
        }
        public double getMargenI
        {
            get
            {
                return mMargenI;
            }
        }
        public double getMargenD
        {
            get
            {
               return mMargenD;
            }
        }
        public double getAzimutFinal
        {
            get
            {
                return mAzimutFinal;
            }
        }

    }
}
