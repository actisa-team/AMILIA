using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

namespace PerfilLongitudinal.componentes
{
    [Serializable]
    public class Recta : ComponenteLong
    {

        public Recta(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iPkIni)
            : base(iPuntoEntrada, iPuntoSalida, iPkIni)
        {

        }

        public override tipoComponente getTipoComponente()
        {
            return tipoComponente.recta;
        }

        public double getPendiente()
        {
            double miPendiente;
            double miIncX = getPuntoSalida.coordenadaX - getPuntoEntrada.coordenadaX;
            double miIncY = getPuntoSalida.coordenadaY - getPuntoEntrada.coordenadaY;
            miPendiente = miIncY / miIncX;
            if (miIncX == 0)
            {
                miPendiente = 0;
            }
            return miPendiente;
        }


        public override Object draw()
        {
            List<Punto3d> miListaPuntos = new List<Punto3d>();
            miListaPuntos.Add(getPuntoEntrada);
            miListaPuntos.Add(getPuntoSalida);


            return miListaPuntos;
        }
    }
}
