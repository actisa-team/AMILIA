using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{
   
    
    
    
    public abstract class oMuroModel : oSeccionDecoradorParent
    {
        public Guid material {get;set;}
        public double espesor { get; set; }
        public double empotramiento { get; set; }


        public oMuroModel(Guid iIdMaterial, double iEspesor, double iEmpotramiento)
        {
            material = iIdMaterial;
            espesor = iEspesor;
            empotramiento = iEmpotramiento;
        }


        public abstract double area { get; }

    }
}
