using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Gis
{

    using tadLayData;
    using tadLayLan;

    using tadLayLogica.datos.BaseDatos;
    
    public class oDalTbEst
    {

       public static dsBd.tbEstRow getZona(Guid iId)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                return miDs.dataset.tbEst.FindByid(iId);
            }     
        }


       public static void deleteById(Guid iId)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                miDs.dataset.tbEst.FindByid(iId).Delete();
                miDs.save(true); 
            }   
        }



       public static dsBd.tbEstDataTable getTabla()
       {
           using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
           {
               return miDs.dataset.tbEst;
           }
       }


       public static dsBd.tbEstDataTable getTablaCache()
       {
          return oSingletonDsBd.getInstance.dataset.tbEst;
       }





    }
}
