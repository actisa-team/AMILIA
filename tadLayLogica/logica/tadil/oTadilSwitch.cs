using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

   using tadLayLan;


   /// <summary>
   /// CONMUTADOR INSTANCIAS TDI-TDB
   /// </summary>
   public static class oTadilSwitch
   {

       public static bool hasInstanceTDI { get; set; }
       public static bool hasInstanceTDB { get; set; }


       public static void Inicializar ()
       {
            hasInstanceTDI=false;
            hasInstanceTDB=false;
       }

       public static bool  canOpenTDI ()
       {
           
           if (hasInstanceTDB)
           {
               return false;
           }
           else
           {
               return true;
           }

       }

       public static bool canOpenTDB ()
       {
           if (hasInstanceTDI)
           {
               return false;
           }
           else
           {
               return true;
           }
       }


       public static void showUserNoPermitirInstanciarTDI()
       {

           oTadil.data.UserInfo.showInfo(strGeneralUser.uiTDInoPermiteInstancia);
       }

       public static void showUserNoPermitirInstanciarTDB()
       {

           oTadil.data.UserInfo.showInfo(strGeneralUser.uiTDBnoPerimiteInstancia);
       }




   }
}
