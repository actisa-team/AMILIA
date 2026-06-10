using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logica.Componentes
{
    public class Punto3d
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public Punto3d(double x,double y,double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
