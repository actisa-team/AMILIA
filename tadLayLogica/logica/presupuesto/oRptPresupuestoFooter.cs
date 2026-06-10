using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.presupuesto
{
   public class oRptPresupuestoFooter
   {

       public string nombre { get; set; }
       public double? porcentaje { get; set; }
       public double? importe { get; set; }
       public string moneda { get; set; }         
          

       public oRptPresupuestoFooter(string iNombre, double? iPorcentaje, double? iImporte,string iMoneda)
       {
           nombre = iNombre;
           porcentaje = iPorcentaje;
           importe = iImporte;
           moneda = iMoneda;
       }

       public override string ToString()
       {
           return nombre + ";" +
                  porcentaje + ";" +
                  importe + ";" +
                  moneda + ";";

       }

   }
}
