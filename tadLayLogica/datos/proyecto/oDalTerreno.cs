using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{
    using System.IO;
    using tadLayData;
    using tadLayShare;

    public class oDalTerreno
    {

        public static void saveTable()
        { 
            oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbTerreno, true);
        }


        public static string getTerrenoHandle ()
        {

            dsApp.tbTerrenoRow miRow = oSingletonDsApp.getInstance.dataset.tbTerreno.FindByid("APP");

            if (miRow == null)
            {
                throw new oExRowNotFound("id", "tbTerreno");
            }
            else
            {
                return miRow.handleTerreno;
            }
        }


        public static double getTerrenoTolerancia()
        {

            dsApp.tbTerrenoRow miRow = oSingletonDsApp.getInstance.dataset.tbTerreno.FindByid("APP");

            if (miRow == null)
            {
                throw new oExRowNotFound("id", "tbTerreno");
            }
            else
            {
                return miRow.tolerancia;
            }
        }
    }
}
