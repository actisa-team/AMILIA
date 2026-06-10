using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria.Saneo
{
    
    using Autodesk.AutoCAD.DatabaseServices;
    
    
    public  class oLwSaneo 
    {

        public Polyline lwSaneo { get; set; }
        public eSaneo saneo { get; set; }


        public oLwSaneo(Polyline iLwSaneo, eSaneo iSaneo)
        {
            lwSaneo = iLwSaneo;
            saneo = iSaneo;
        }

    }
}
