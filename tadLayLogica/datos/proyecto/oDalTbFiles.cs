using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{
    using System.IO;
    using tadLayData;

    public class oDalTbFiles
    {
        public static void saveTable()
        { 
            oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbFiles, true);
        }
        public static bool isConfiguracionRutasValidos(eEstudioTipo iEstudioTipo)
        {

            dsApp.tbFilesRow miRow = oSingletonDsApp.getInstance.files;
            
  
            if (miRow.IsnormasVpRpNull() || !File.Exists(miRow.normasVpRp))
            {
                return false;
            }
            else if (miRow.IsnormasVpKvNull() || !File.Exists(miRow.normasVpKv))
            {
                return false;
            }
            else if (iEstudioTipo == eEstudioTipo.ESTINF && !File.Exists(miRow.baseDatos))
            {
                return false;
            }
            else
            {
                return true;
            }

       
       }
        public static void setupArchivosConfiguracion(eEstudioTipo iEstudioTipo)
        {
                oTadil.data.Files.fileNormasCarreteras = oSingletonDsApp.getInstance.files.normasVpRp;
                oTadil.data.Files.fileNormasCarreterasKv = oSingletonDsApp.getInstance.files.normasVpKv;


                if (iEstudioTipo == eEstudioTipo.ESTINF)
                {
                    oTadil.data.Files.fileBbdd = oSingletonDsApp.getInstance.files.baseDatos;
                }
        } 
    }
}
