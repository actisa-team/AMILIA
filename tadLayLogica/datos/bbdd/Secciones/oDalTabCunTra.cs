using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Secciones
{

    using tadLayLogica.datos.BaseDatos;

    using tadLayData;
    using tadLayShare;
    
    
    public class oDalTabCunTra
    {
        /// <summary>
        /// Get Tabla
        /// </summary>
        public static dsBd.tbCunetaTrapezDataTable getTabla()
        {
            return oSingletonDsBd.getInstance.dataset.tbCunetaTrapez;
        }
        /// <summary>
        /// Save Tabla
        /// </summary>
        public static void saveTabla()
        {
            oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbCunetaTrapez, true);
        }
        /// <summary>
        /// Get Row By ID
        /// </summary>
        public static dsBd.tbCunetaTrapezRow getRowById(Guid iId)
        {

            dsBd.tbCunetaTrapezRow miRow = oSingletonDsBd.getInstance.dataset.tbCunetaTrapez.FindByid(iId);

            if (miRow == null)
            {
                throw new oExRowNotFound("Id", "TbCuneta Trapezoidal");
            }


            return miRow;
                   
        }
        /// <summary>
        /// Delete Row By ID
        /// </summary>
        public static void deleteById(oDsBd iDsBd, Guid iId)
        {
            oSingletonDsBd.getInstance.dataset.tbCunetaTriang.FindByid(iId).Delete();
            oSingletonDsBd.getInstance.save(true);
        }
        /// <summary>
        /// Delete Row By ID
        /// </summary>
        public static void deleteById(Guid iId)
        {
            dsBd.tbCunetaTrapezRow miRow = getRowById(iId);
            miRow.Delete();
        }
    }
}
