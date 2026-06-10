using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Polilinea_ingenieril
    {
        public double x { get; set; }
        public double y { get; set; }
        public double distancia { get; set; }
        public double az { get; set; }
        public double cambbio_az { get; set; }
        public double distancia_sig { get; set; }
        public double val_1 { get; set; }
        public double val_2 { get; set; }
        public double media_az { get; set; }
        public double var_azimutal { get; set; }
        public double Max_variacion { get; set; }
        public bool recta { get; set; }
        public bool curva { get; set; }
        public double dt1 { get; set; }
        public double dt2 { get; set; }


        public Polilinea_ingenieril()
        {

        }
        public Polilinea_ingenieril(double xx,double yy)
        {
            x = xx;
            y = yy;
        }
    }
}
