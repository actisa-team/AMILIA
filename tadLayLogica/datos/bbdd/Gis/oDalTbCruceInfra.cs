using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Gis
{

    using tadLayData;
    using tadLayLogica.datos.BaseDatos;

    public class oDalTbCruceInfra
    {

        public static dsBd.tbCruceInfraRow getRowById(Guid iId)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                return miDs.dataset.tbCruceInfra.FindByid(iId);

            }

        }
        public static void deleteById(Guid iIdZona)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                miDs.dataset.tbCruceInfra.FindByid(iIdZona).Delete();
                miDs.save(true);
            }
        }
        public static dsBd.tbCruceInfraDataTable getTabla()
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                return miDs.dataset.tbCruceInfra;
            }
        }

    }
}
