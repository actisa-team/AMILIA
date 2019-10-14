using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLan
{
   public class oTraductor
    {

       public static string traducirSiNo(bool iValor)
       {
           if (iValor)
           {
               return strGeneral.uiSi;
           }
           else
           {
               return strGeneral.uiNo;

           }           
       }

    }
}
