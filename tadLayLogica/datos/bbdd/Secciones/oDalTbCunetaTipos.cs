using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdb;

namespace tadLayLogica.datos.Secciones
{

    using tadLayLan;
    using tadLayData;
    using engNet.ClassT;
    using tadLayLogica.datos.BaseDatos;
    
    public class oDalTbCunetaTipos
    {


        /// <summary>
        /// AÑADIR TIPOS DE CUNETAS
        /// </summary>
        public static void addTipoCuneta()
        {
            using (oSingletonDsBd miDs = oSingletonDsBd.getInstance)
            {
                miDs.dataset.tbCunetaTipo.AddtbCunetaTipoRow("TRIANG", "Cuneta Triangular");
                miDs.dataset.tbCunetaTipo.AddtbCunetaTipoRow("TRAPEZ", "Cuneta Trapezoidal");
                miDs.save(true);
            }

        }

        /// <summary>
        /// OJO EXISTE DUPLICIDAD DEL TIPO CUNETA en la Tablas de Precios y Secciones OJO!!!!
        /// </summary>
        /// <param name="iTbCunetaTipos"></param>
        /// <returns></returns>
        public static List<oValDesT<string, string>> getLstCunetasTipos(dsBd.tbCunetaTipoDataTable iTbCunetaTipos)
        {

            List<oValDesT<string, string>> miLstIdCode = new List<oValDesT<string, string>>();
            oValDesT<string, string> miItem;


                foreach (dsBd.tbCunetaTipoRow   item in iTbCunetaTipos)
                {
                    miItem = new oValDesT<string, string>();
                    miItem.val = item.id;
                    miItem.des = strFrmSecciones.ResourceManager.GetString("ui" + item.id);
                    miLstIdCode.Add(miItem);
                }

           

            return miLstIdCode;


        }


    }

}
