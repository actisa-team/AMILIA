using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Secciones
{
    using tadLayData;
    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;
    using tadLayLan;


    public class oDalTbRoadDobleSinMediana
    {
        //SAVE TABLA
        public static void saveTabla()
        {
            oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana, true);
        }
        //GET TABLA
        public static dsBd.tbRoadDobleSinMedianaDataTable getTabla()
        {
            return oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana;
        }
        //DELETE BY ROW
        public static void deleteById(Guid iId)
        {
            oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana.FindByid(iId).Delete();
            oDalTbRoadDobleSinMediana.saveTabla();
        }

        //GET ROW BY ID
        public static dsBd.tbRoadDobleSinMedianaRow getAutoviaById(Guid iId)
        {

            dsBd.tbRoadDobleSinMedianaRow miRow = oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana.FindByid(iId);

            if (miRow == null)
            {
                throw new oExRowNotFound(iId.ToString(), strError.eDobleAutoviaSinMediana);
            }

            return miRow;

        }
    }
}
