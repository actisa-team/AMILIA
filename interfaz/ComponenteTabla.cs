using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace interfaz
{
    class ComponenteTabla
    {
        public List<Punto> lista_puntos = new List<Punto>();
        public int Tipo { get; set; }//recta==1  // curva==2  //clotoide==3
        public string PuntoInicial => lista_puntos.Count > 0 ? $"({lista_puntos.First().p.X}, {lista_puntos.First().p.Y})" : "N/A";
        public string PuntoFinal => lista_puntos.Count > 0 ? $"({lista_puntos.Last().p.X}, {lista_puntos.Last().p.Y})" : "N/A";
        public double xc { get; set; }
        public double yc { get; set; }
        public double azr { get; set; }
        public double radio { get; set; }
    }
}
