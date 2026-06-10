using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{
    using System.IO;
    using tadLayData;

    public class oDalTbEstilosVisualizacion
    {
        public static void saveTable()
        { 
            oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion, true);
        }

    }
}
