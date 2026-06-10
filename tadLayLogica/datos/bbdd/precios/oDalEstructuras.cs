using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdb;

namespace tadLayLogica.datos.precios
{

    using tadLayLan;
    using tadLayData;
    using engNet.ClassT;
    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;
    
    
    public class oDalEstructuras
    {


        /// <summary>
        /// Obtengo listado de Filas por IdClasificacion (EST, TUN, etc...)
        /// </summary>
        public static List<dsBd.tbMaterialesRow> getLstIemsByIdClasificacion(dsBd.tbMaterialesDataTable iTbMateriales, short iIdClasificacion)
        {
           
                var miQery = from p in iTbMateriales
                             where p.idClasificacion == iIdClasificacion
                             select p;

                List<dsBd.tbMaterialesRow> miLstRow = miQery.ToList();

                return miLstRow;
        }



        /// <summary>
        /// Obtener el Código de la Estructura
        /// </summary>
        public static string getCodeEstructura (short iIdGrupoEstructura)
        {

        dsBd.tbClasificacionesRow miRow= oSingletonDsBd.getInstance.dataset.tbClasificaciones.FindByidClasificacion(iIdGrupoEstructura);

        if (miRow == null)
        {
            throw new oExRowNotFound(iIdGrupoEstructura.ToString(), "tbClasificaciones");
        }
        else
        {
            return miRow.code;
        }

        }


        public static List<oValDesT<short, string>> getLstEstTipos(string iIdGrupo)
        {

            List<oValDesT<short, string>> miLstIdCode = new List<oValDesT<short, string>>();
            oValDesT<short, string> miItem;


            var miQery = from p in  oSingletonDsBd.getInstance.dataset.tbClasificaciones  
                            where p.idGrupo == iIdGrupo
                            select p;

            List<dsBd.tbClasificacionesRow> miLstRow = miQery.ToList();

            foreach (dsBd.tbClasificacionesRow item in miLstRow)
            {
                miItem = new oValDesT<short, string>();
                miItem.val = item.idClasificacion;
                miItem.des = strFrmPrecios.ResourceManager.GetString("ui" + item.code);
                miLstIdCode.Add(miItem);
            }

            

            return miLstIdCode;

          
        }
    }
}
