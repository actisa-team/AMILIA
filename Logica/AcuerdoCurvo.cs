using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tadLayShare.puntos;

namespace Logica
{
    public class AcuerdoCurvo
    {
        public Punto3d centro { get; set; }
        public Punto3d entrada { get; set; }
        public Punto3d salida { get; set; }
        public double radio { get; set; }
        public EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva sentido { get; set; }
        public AcuerdoCurvo()
        {

        }
        public AcuerdoCurvo(Punto3d e, Punto3d s,Punto3d c,double r, EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva se)
        {
            entrada = e;
            salida = s;
            centro = c;
            radio = r;
            sentido = se;
        }
    }
}
