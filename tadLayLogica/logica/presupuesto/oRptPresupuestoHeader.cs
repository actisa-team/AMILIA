using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.presupuesto
{
   public class oRptPresupuestoHeader
   {

       public string nombre { get; set; }
       public string fecha { get; set; }
        
          

       public oRptPresupuestoHeader(string iNombre, string iFecha)
       {
           nombre = iNombre;
           fecha = iFecha;
       }


   }
}
