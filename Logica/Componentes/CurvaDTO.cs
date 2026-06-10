using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logica.Componentes;
using static Logica.Componentes.Componente;

namespace DTOs
{
    public class CurvaDTO 
    {
        public tipoComponente tipo { get; set; }
        public Punto3d Centro { get; set; }
        public Punto3d punto_inicial { get; set; }
        public Punto3d punto_final { get; set; }
        public double Radio { get; set; }
        public double Az_inicial { get; set; }
        public double Az_final { get; set; }
        public string Sentido { get; set; }
        public double Pk_inicial { get; set; }
        public double Pk_final { get; set; }
    }
}
