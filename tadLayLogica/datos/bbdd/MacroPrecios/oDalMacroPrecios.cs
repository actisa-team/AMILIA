using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.MacroPrecios
{

    using tadLayLan;
    using tadLayData;
    using engNet.ClassT;
    using tadLayLogica.datos.BaseDatos;



    public class oDalMacroPrecios
    {

        /// <summary>
        /// LISTADO DE MATERIALES POR TIPO DE ROAD
        /// </summary>
        public static List<dsBd.tbMacroPreciosRow> getMacroPreciosByRoadTipo(string iFiltro)
        {

            using ( oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {

                if (miDs.dataset.tbRoadTipo.FindByid(iFiltro) == null)
                {
                    throw new Exception(string.Format(strError.eTipoCalzadaNoImpl, iFiltro));
                }
                else
                {
                    if (miDs.dataset.tbMacroPrecios.Rows.Count > 0)
                    {
                        var miQery = from p in miDs.dataset.tbMacroPrecios
                                     where p.idTbRoadTipo == iFiltro
                                     select p;

                        return miQery.ToList();

                    }
                    else
                    {
                        return new List<dsBd.tbMacroPreciosRow>();
                    }
  
                }
   
            }


        }

        public static void delete(Guid iIdMacroPrecio)
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                miDs.dataset.tbMacroPrecios.FindByid(iIdMacroPrecio).Delete();

                miDs.save(false);
            }
        }

    }


}
