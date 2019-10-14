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

        public enum tipoComponente {curva, linea, clotoideEntrada, clotoideSalida};

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

        public static Punto3d[] puntosEnRectaAtDistance(Punto3d iPx, double iPendiente, double iDistance)
        {

            Punto3d[] misPuntos = new Punto3d[2];

            double miA = Math.Pow(iPendiente, 2) + 1;
            double miB = (-1) * (2 * iPx.coordenadaX + 2 * iPx.coordenadaY * iPendiente + 2 * iPendiente * (iPendiente * iPx.coordenadaX + iPx.coordenadaY));
            miB = (-1) * (Math.Pow(iPendiente, 2) + 1) * 2 * iPx.coordenadaX;
            double miC = 2 * iPx.coordenadaY * iPendiente * iPx.coordenadaX - 2 * Math.Pow(iPx.coordenadaY, 2) + iPendiente * iPx.coordenadaX + iPx.coordenadaY - Math.Pow(iDistance, 2) + Math.Pow(iPx.coordenadaX, 2) + Math.Pow(iPx.coordenadaY, 2);
            miC = (Math.Pow(iPendiente, 2) + 1) * iPx.coordenadaX - Math.Pow(iDistance, 2);
            double x1 = ((-1) * miB + Math.Sqrt(Math.Pow(miB, 2) - 4 * miA * miC)) / 2 * miA;
            double x2 = ((-1) * miB - Math.Sqrt(Math.Pow(miB, 2) - 4 * miA * miC)) / 2 * miA;

            double y1 = iPendiente * (x1 - iPx.coordenadaX) + iPx.coordenadaY;
            double y2 = iPendiente * (x2 - iPx.coordenadaX) + iPx.coordenadaY;


            misPuntos[0] = new Punto3d(x1, y1, 0);
            misPuntos[1] = new Punto3d(x2, y2, 0);

            return misPuntos;
        }
        abstract public List<double[]> getComponentPoints();
        abstract public List<double[]> getComponentPoints(double pk);

    }
}
