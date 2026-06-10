using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{
    using System.IO;
    using tadLayData;

    public class oDalTbProyectosTipo
    {
      

        public static dsApp.tbProyectosTipoDataTable getTabla ()
        {
            return oSingletonDsApp.getInstance.dataset.tbProyectosTipo;
        }



       
    }
}
