using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Gis
{


    using tadLayData;
    using tadLayLogica.datos.BaseDatos;
    
    public class oDalTbSocioEconomicas
    {

        /// <summary>
        /// Devuelvo las Zonas SocioEcnomicas
        /// </summary>
        /// <param name="iIdSector">SECPRI ; SECSEC ; SECTER </param>
       public static List<dsBd.tbSocioEconomicosRow> getZonasSectorById (string iIdSector)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {

                var miQery = from p in miDs.dataset.tbSocioEconomicos
                             where p.idCodeSector == iIdSector
                             select p;

                return miQery.ToList();  
            } 
        
        }


       public static dsBd.tbSocioEconomicosRow getZona(Guid iIdZona)
       {

           using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
           {
               return miDs.dataset.tbSocioEconomicos.FindByid(iIdZona);  
           }
   
       }


       public static void deleteById(Guid iId)
       {
           using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
           {
               miDs.dataset.tbSocioEconomicos.FindByid(iId).Delete();
               miDs.dataset.tbSocioEconomicos.AcceptChanges();
           } 

       }
        

        



    


    }
}
