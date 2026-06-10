using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Gis
{


    using tadLayData;
    using tadLayLogica.datos.BaseDatos;
    
    public class oDalTbPatrimonialesSuelo
    {

        /// <summary>
        /// Devuelvo las Zonas SocioEcnomicas
        /// </summary>
        /// <param name="iIdSector">URBANO ; URBANI ; NOURBA </param>
       public static List<dsBd.tbPatrimonioSueloRow> getZonasSectorById (string iIdSuelo)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {

                var miQery = from p in miDs.dataset.tbPatrimonioSuelo
                             where p.idCodeSuelo == iIdSuelo
                             select p;

                return miQery.ToList();  
            } 
        
        }


       public static dsBd.tbPatrimonioSueloRow getZona(Guid iIdZona)
       {

           using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
           {
               return miDs.dataset.tbPatrimonioSuelo.FindByid(iIdZona);  
           }
   
       }


       public static void deleteById(Guid iId)
       {
           using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
           {
               miDs.dataset.tbPatrimonioSuelo.FindByid(iId).Delete();
               miDs.dataset.tbPatrimonioSuelo.AcceptChanges();
           } 

       }
        

        



    


    }
}
