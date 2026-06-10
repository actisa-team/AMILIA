using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{
   
    using tadLayData;
    using tadLayShare;
    using tadLayLan;
    
    
    public class oDalPresupuesto
    {

       public static void saveTable()
       {
           oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbPresupuesto, true);
       }


       public static dsApp.tbPresupuestoRow getPresupuestoRow ()
       {

           dsApp.tbPresupuestoRow miRowApp = oSingletonDsApp.getInstance.dataset.tbPresupuesto.FindByid("APP");

           if (miRowApp != null)
           {
               return miRowApp;
           }
           else
           {
               throw new oExRowNotFound("id", strError.ePresupuesto);

           }

       }

    }
}
