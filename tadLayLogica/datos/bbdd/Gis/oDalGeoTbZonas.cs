using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Gis
{

    using System.Drawing;
    using tadLayLan;
    using tadLayData;

    using tadLayLogica.datos.BaseDatos;
    
    public  class oDalGeoTbZonas
    {

        public static dsBd.tbGeoRow getZonaGeo (Guid iId)
        {
            //oDsBd miDs = oDsBd.getInstance();
                //return miDs.dataset.tbGeo.FindByid(iId);
            return oSingletonDsBd.getInstance.dataset.tbGeo.FindByid(iId);
            
        }


        public static dsBd.tbGeoDataTable getLstZonas()
        {
            //oDsBd miDs = oDsBd.getInstance();
                //return miDs.dataset.tbGeo;   
            oSingletonDsBd.getInstance.Dispose();
            return oSingletonDsBd.getInstance.dataset.tbGeo;
                  
        }

        public static void deleteZona (Guid iId)
        {
            //oDsBd miDs = oDsBd.getInstance();
                //miDs.dataset.tbGeo.FindByid(iId).Delete();
                //miDs.save();

            oSingletonDsBd.getInstance.dataset.tbGeo.FindByid(iId).Delete();
            oSingletonDsBd.getInstance.save(false);
            
     
        }

        public static dsBd.tbGeoDataTable getTablaCache()
        {
            oSingletonDsBd.getInstance.Dispose();
            return oSingletonDsBd.getInstance.dataset.tbGeo;
        }



   




    }
}
