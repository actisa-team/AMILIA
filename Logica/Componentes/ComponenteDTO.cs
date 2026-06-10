using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logica.Componentes
{
    public class ComponenteDTO
    {
        public class Centro
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

        public class PuntoInicial
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

        public class PuntoFinal
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }
        public int tipo { get; set; }
        public PuntoInicial punto_inicial { get; set; }
        public PuntoFinal punto_final { get; set; }
        public double A { get; set; }
        public double Le { get; set; }
        public double Azimut { get; set; }
        public string Sentido { get; set; }
        public double Pk_inicial { get; set; }
        public double Pk_final { get; set; }
        public string tipoclotoide { get; set; }
        public Centro centro { get; set; }
        public double Radio { get; set; }
        public double Az_inicial { get; set; }
        public double Az_final { get; set; }
    }
}
