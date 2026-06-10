using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Componente_ingenieril
    {
        public int tipo { get; set; } //curva-->0, recta-->1 
        public int inicio { get; set; }
        public int final { get; set; }
        public double azimut { get; set; }
        public double radio { get; set; }
        public double centro_x { get; set; }
        public double centro_y { get; set; }

        public Componente_ingenieril(int ini,int fin,int tip)
        {
            inicio = ini;
            final = fin;
            tipo = tip;
        }
    }
}
