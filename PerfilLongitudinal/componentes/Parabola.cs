using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

namespace PerfilLongitudinal.componentes
{
    [Serializable]
    public class Parabola : ComponenteLong
    {

        public enum tipoParabola { concavo, convexo };
        private tipoParabola mTipoParabola;

        private double mCambioPendiente;
        private double mKv;
        double mT;
        double mPendienteEntrada;



        public Parabola(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iPkIni, double iPendienteEntr, double iKv, double iCambioPendi, bool isConcavo)
            : base(iPuntoEntrada, iPuntoSalida, iPkIni)
        {
            mPendienteEntrada = iPendienteEntr;
            mCambioPendiente = iCambioPendi;
            mKv = iKv;
            mT = (mKv * mCambioPendiente) / 2;
            if (isConcavo) mTipoParabola = tipoParabola.concavo; else mTipoParabola = tipoParabola.convexo;
        }




        public override tipoComponente getTipoComponente()
        {
            return tipoComponente.parabola;
        }

        public tipoParabola getTipoParabola()
        {
            return mTipoParabola;
        }


        public override Object draw()
        {
            List<Punto3d> miListaPuntos = new List<Punto3d>();
            miListaPuntos.Add(getPuntoEntrada);

            double miX = getPuntoEntrada.coordenadaX + (getPuntoSalida.coordenadaX - getPuntoEntrada.coordenadaX) / 2;
            double miY = getCotaAcuerdo(miX);

            miListaPuntos.Add(new Punto3d(miX, miY,0));

            miListaPuntos.Add(getPuntoSalida);


            return miListaPuntos;
        }


        private double getCotaAcuerdo(double iX)
        {
            double res;
            if(mTipoParabola == tipoParabola.concavo)
            {
                res = (getPuntoEntrada.coordenadaY + (mPendienteEntrada * (iX - getPuntoEntrada.coordenadaX)) + (Math.Pow(iX - getPuntoEntrada.coordenadaX, 2) / (2 * mKv)));
            }
            else
            {
                res = (getPuntoEntrada.coordenadaY + (mPendienteEntrada * (iX - getPuntoEntrada.coordenadaX)) + (Math.Pow(iX - getPuntoEntrada.coordenadaX, 2) / (2 * mKv * (-1))));
            }
            return res;
        }
    }
}
