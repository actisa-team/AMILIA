using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

namespace PerfilLongitudinal.componentes
{

    [Serializable]
    public abstract class ComponenteLong
    {
        public enum tipoComponente { recta, parabola };


        private double mPuntoEntradaX;
        private double mPuntoSalidaX;
        private double mPuntoEntradaY;
        private double mPuntoSalidaY;
        private double mPkInicial;



        public ComponenteLong(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iPkIni)
        {
            mPuntoEntradaX = iPuntoEntrada.coordenadaX;
            mPuntoEntradaY = iPuntoEntrada.coordenadaY;
            mPuntoSalidaX = iPuntoSalida.coordenadaX;
            mPuntoSalidaY = iPuntoSalida.coordenadaY;
            mPkInicial = iPkIni;
        }

        public Punto3d getPuntoEntrada
        {
            get
            {
                return new Punto3d(mPuntoEntradaX, mPuntoEntradaY, 0);
            }
        }


        public Punto3d getPuntoSalida
        {
            get
            {
                return new Punto3d(mPuntoSalidaX, mPuntoSalidaY, 0);
            }
        }

        public double getPkIni()
        {
            return mPkInicial;
        }


        public double getLongutid()
        {
            return getPuntoSalida.coordenadaX - getPuntoEntrada.coordenadaX;
        }


        abstract public tipoComponente getTipoComponente();


        abstract public Object draw();

        

    }
}
