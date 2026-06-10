using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdb;

namespace tadLayLogica.datos.Gis
{

    using tadLayLan;
    using tadLayData;
    using engNet.ClassT;

    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;
    
    
    public class oDalTbTun
    {
        /// <summary>
        /// GET TABLA
        /// </summary>
        public static dsBd.tbTunDataTable getTabla()
        {
            return oSingletonDsBd.getInstance.dataset.tbTun;
        }
        /// <summary>
        /// GET ZONA TUNELES
        /// </summary>
        public static dsBd.tbTunRow getZona(Guid iId)
        {

            var prueba = oSingletonDsBd.getInstance.dataset.tbTun.ToList();
            dsBd.tbTunRow miRowTun = oSingletonDsBd.getInstance.dataset.tbTun.FindByid(iId);

            if (miRowTun != null)
            {
                return miRowTun;
            }
            else
            {
                //return null;
                 throw new oExRowNotFound(iId.ToString(), "tablaTuneles");
            }

            
        }
        /// <summary>
        /// DELETE BY ID ZONA
        /// </summary>
        public static void deleteById(Guid iId)
        {  
            oSingletonDsBd.getInstance.dataset.tbTun.FindByid(iId).Delete();
            oSingletonDsBd.getInstance.save(true);                              
        }
        /// <summary>
        ///EXCAVACIÓN METODOS
        /// </summary>
        public static List<oValDesT<string, string>> getExcavacionMetodos()
       {
          
            List<oValDesT<string, string>> miLstIdCode = new List<oValDesT<string, string>>();
            oValDesT<string, string> miItem;

            foreach (dsBd.tbTunExcMetRow item in  oSingletonDsBd.getInstance.dataset.tbTunExcMet)
            {
                miItem = new oValDesT<string, string>();
                miItem.val = item.id;
                miItem.des = strFrmGisTun.ResourceManager.GetString("ui" + item.id);
                miLstIdCode.Add(miItem);
            }


            return miLstIdCode;   
       }
        /// <summary>
        /// TUNEL TRATAMIENTOS
        /// </summary>
        public static List<oValDesT<string, string>> getTratamientosMetodos()
        {
         
            

                List<oValDesT<string, string>> miLstIdCode = new List<oValDesT<string, string>>();
                oValDesT<string, string> miItem;

                foreach (dsBd.tbTunTipoTratamietoRow item in oSingletonDsBd.getInstance.dataset.tbTunTipoTratamieto)
                {
                    miItem = new oValDesT<string, string>();
                    miItem.val = item.id;
                    miItem.des = strFrmGisTun.ResourceManager.GetString("ui" + item.id);
                    miLstIdCode.Add(miItem);
                }


                return miLstIdCode;

            

        }

    }
}
