using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

namespace EjeDeTrazado.componentes
{
    [Serializable]
    public abstract class Componente
    {

        public enum tipoComponente { curva, linea, clotoideEntrada, clotoideSalida };

        private double mPuntoEntradaX;
        private double mPuntoSalidaX;
        private double mPuntoEntradaY;
        private double mPuntoSalidaY;
        private double mPkInicial;



        public Componente(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iPkIni)
        {
            mPuntoEntradaX = iPuntoEntrada.coordenadaX;
            mPuntoEntradaY = iPuntoEntrada.coordenadaY;
            mPuntoSalidaX = iPuntoSalida.coordenadaX;
            mPuntoSalidaY = iPuntoSalida.coordenadaY;
            mPkInicial = iPkIni;
        }

        abstract public double getLongitud();
        abstract public double getPkFinal();

        abstract public double getMargenIzq(double iPk);
        abstract public double getMargenDer(double iPk);

        abstract public double getPeralte();

        abstract public double getVariacionMI();

        abstract public double getVariacionMD();

        abstract public tipoComponente getTipoComponente();

        abstract public Object draw();

        abstract public double[] getPointAtDist(double iDistancia);

        abstract public double[] getPointAtLocation(double iDistancia, double iOffset, EjeDeTrazado.puntosDelEje.EjeTrazado.ladoCalzada iLadoCalzada);

        public double getPkIni
        {
            get
            {
                return mPkInicial;
            }
        }

        public Punto3d getPuntoEntrada
        {
            get
            {
                return new Punto3d(mPuntoEntradaX,mPuntoEntradaY, 0);
            }
        }

        public Punto3d setPuntoEntrada
        {
            set
            {
                mPuntoEntradaX = value.coordenadaX;
                mPuntoEntradaY = value.coordenadaY;
            }
        }

        public Punto3d getPuntoSalida
        {
            get
            {
                return new Punto3d(mPuntoSalidaX, mPuntoSalidaY, 0);
            }
        }

        public Punto3d setPuntoSalida
        {
            set
            {
                mPuntoSalidaX = value.coordenadaX;
                mPuntoSalidaY = value.coordenadaY;
            }
        }


        abstract public List<double[]> getComponentPoints();

    }
}
