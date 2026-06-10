using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tadLayData;
using tadLayLogica.datos.BaseDatos;

namespace tadLayLogica.datos.bbdd.Adecuacion
{
    public class oDalAdecuacion
    {
        public static dsBd.tbAdecuacionDataTable getAdecuacion()
        {
            //oDsBd miDs = oDsBd.getInstance();
            //return miDs.dataset.tbGeo;   
            oSingletonDsBd.getInstance.Dispose();
            return oSingletonDsBd.getInstance.dataset.tbAdecuacion;

        }
        public static dsBd.tbAdecuacionRow getAdecuacion(Guid iId)
        {
            return oSingletonDsBd.getInstance.dataset.tbAdecuacion.FindByid(iId);

        }
        public static void deleteZona(Guid iId)
        {
            //oDsBd miDs = oDsBd.getInstance();
            //miDs.dataset.tbGeo.FindByid(iId).Delete();
            //miDs.save();

            oSingletonDsBd.getInstance.dataset.tbAdecuacion.FindByid(iId).Delete();
            oSingletonDsBd.getInstance.save(false);


        }

    }
}
