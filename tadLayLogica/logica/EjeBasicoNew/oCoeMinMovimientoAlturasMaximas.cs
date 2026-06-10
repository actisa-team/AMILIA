using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{
   public  class oCoeMinoracionAlturasMaximas
   {


       public double desmonteCoeficienteMinoracionAlturaMaxima { get; private set; }
       public double terraplenCoeficienteMinoracionAlturaMaxima { get; private set; }
       public double pilaCoeficienteMinoracionAlturaMaxima { get; private set; }



       public oCoeMinoracionAlturasMaximas(double iDesmonteCoeMinAlturaMaxima, double iTerreplanCoeMinAlturaMaxima, double iPilaCoeMinAlturaMaxima)
       {
           this.desmonteCoeficienteMinoracionAlturaMaxima = iDesmonteCoeMinAlturaMaxima;
           this.terraplenCoeficienteMinoracionAlturaMaxima = iTerreplanCoeMinAlturaMaxima;
           this.pilaCoeficienteMinoracionAlturaMaxima = iPilaCoeMinAlturaMaxima;
       }


   }
}
