using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ.Eje
{

    using tadLayLogica.EjeTJ.Tramos;
    using tadLayLogica.EjeTJ.Vertice;

    public class oEjePruebas : oEjeObj<oVer, oTramoBase>
    {

        public oEjePruebas(Dictionary<int, oVer> iDicVertice)
                  :base("Pruebas",iDicVertice)
         
        { 
        
        
        }


    }
}
