using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Gis
{


    using tadLayData;
    using tadLayLogica.datos.BaseDatos;
    
    public class oDalTbDoHiPu
    {


       public static dsBd.tbDoHiRow getRowById(Guid iId)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                return miDs.dataset.tbDoHi.FindByid(iId);
               
            } 
        
        }        
       public static void deleteById(Guid iIdZona)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                miDs.dataset.tbDoHi.FindByid(iIdZona).Delete();
                miDs.save(true); 
            }   
        }
       public static dsBd.tbDoHiDataTable getTabla()
       {
           using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
           {
               return miDs.dataset.tbDoHi;
           }
       }


    


    }
}
