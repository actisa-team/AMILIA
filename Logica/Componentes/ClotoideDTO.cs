using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logica.Componentes;
using static Logica.Componentes.Componente;

namespace DTOs
{
    public class ClotoideDTO
    {
        public tipoComponente tipo { get; set; }
        public Punto3d punto_inicial { get; set; }
        public Punto3d punto_final { get; set; }
        public double A { get; set; }
        public double Le { get; set; }
        public double Azimut { get; set; }
        public string Sentido { get; set; }
        public double Pk_inicial { get; set; }
        public double Pk_final { get; set; }
        public string tipoclotoide { get; set; }
        public double radio { get; set; }
    }
}
