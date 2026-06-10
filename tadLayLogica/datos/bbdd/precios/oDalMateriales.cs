using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;


namespace tadLayLogica.datos.precios
{
    
    
    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos.BaseDatos;
    using engNet.ClassT;

    using engNet.Extension.String;
    using tadLayShare;

    public class oDalMateriales
    {

 
        public static dsBd.tbMaterialesDataTable getTbMateriales()
        {
            return oSingletonDsBd.getInstance.dataset.tbMateriales; 
        }
        public static dsBd.tbMaterialesRow getMaterialId(Guid iIdMaterial)
        {
            
          dsBd.tbMaterialesRow miRow=oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(iIdMaterial);

          if (miRow == null)
          {
              throw new oExRowNotFound(iIdMaterial.ToString(), "Tabla Materiales");
          }

          return miRow;
                
         
        }
        public static string getMaterialNombreById(Guid iIdMaterial)
        {
            return getMaterialId(iIdMaterial).nombre; 
        }



        /// <summary>
        /// LISTADO DE MATERIALES CUNETA TRAPEZOIDAL
        /// </summary>
        public static List<dsBd.tbMaterialesRow> getLstMaterialesCunetraTrapezoidal(dsBd.tbMaterialesDataTable iTbMateriales)
        {
            return getLstMaterialesByIdClasificacion(iTbMateriales, 7);
        }
        /// <summary>
        /// LISTADO DE MATERIALES CUNETA TRIANGULAR
        /// </summary>
        public static List<dsBd.tbMaterialesRow> getLstMaterialesCunetraTriangular(dsBd.tbMaterialesDataTable iTbMateriales)
        {
            return getLstMaterialesByIdClasificacion(iTbMateriales, 8);
        }
        /// <summary>
        /// LISTADO DE MATERIALES SUELO-EXPROPIACION
        /// </summary>
        public static List<dsBd.tbMaterialesRow> getLstMaterialesSueloExpropiacion()
        {

            

                var miQery = from p in oSingletonDsBd.getInstance.dataset.tbClasificaciones.AsEnumerable()
                             where p.idGrupo == "VES"
                             select p;

                dsBd.tbClasificacionesRow miRowClas = miQery.First();

                return miRowClas.GetChildRows("FK_tbClasificaciones_tbItems").ToList().ConvertAll<dsBd.tbMaterialesRow>(p => (dsBd.tbMaterialesRow)p);

            

        }
        /// <summary>
        /// LISTADO DE MATERIALES SUELO-PRODUCCION
        /// </summary>
        public static List<dsBd.tbMaterialesRow> getLstMaterialesSueloProduccion()
        {

                var miQery = from p in oSingletonDsBd.getInstance.dataset.tbClasificaciones.AsEnumerable()
                             where p.idGrupo == "VPS"
                             select p;

               dsBd.tbClasificacionesRow miRowClas = miQery.First();

              return  miRowClas.GetChildRows("FK_tbClasificaciones_tbItems").ToList().ConvertAll<dsBd.tbMaterialesRow>(p => (dsBd.tbMaterialesRow)p);         
        }



        public static List<dsBd.tbMaterialesRow> getLstMaterialesByIdClasificacion (dsBd.tbMaterialesDataTable iTbMateriales, int iIdClasificacion)
        {


            var miQery = from p in iTbMateriales
                         where p.idClasificacion == iIdClasificacion
                         select p;

            List<dsBd.tbMaterialesRow> miLstRow = miQery.ToList();

            return miLstRow;
        
        }






        public static List<dsBd.tbMaterialesRow> getLstMaterialesByGrupoAndClasificacion (string iLstGrupo, string iLstCodeStr)
        {
        
           List<oValDesT<string, string>> miLstQuery = new List<oValDesT<string, string>>();


           List<dsBd.tbMaterialesRow> miLstMaterialesOut = new List<dsBd.tbMaterialesRow>();
           List<string> miLstGrupo = oExtensionString.lineaSplit(iLstGrupo, "-");
           List<string> miLstClasi = oExtensionString.lineaSplit(iLstCodeStr, "-");
           List<string> miLstItem;

           //Genero el Listado de Consultas
           for (int i = 0; i < miLstGrupo.Count; i++)
           {
               miLstItem = oExtensionString.lineaSplit(miLstClasi[i], ";");

               foreach (string item in miLstItem)
               {
                   miLstQuery.Add(new oValDesT<string, string>(miLstGrupo[i], item));
               }
           }

               foreach (oValDesT<string, string> item in miLstQuery)
               {
                   miLstMaterialesOut.AddRange(getLstMaterialesByGrupoId_Code(item.val, item.des));       
               }

           
           return miLstMaterialesOut;
          
        }




        private static List<dsBd.tbMaterialesRow> getLstMaterialesByGrupoId_Code(string iIdGrupo, string iCode)
        {

          List<dsBd.tbMaterialesRow> miLst = new List<dsBd.tbMaterialesRow>();


       
              foreach (dsBd.tbGruposRow rowGrupo in oSingletonDsBd.getInstance.dataset.tbGrupos.Rows)
              {
                   
                  if (rowGrupo.idGrupo == iIdGrupo)
                  {
                      foreach (dsBd.tbClasificacionesRow item in rowGrupo.GettbClasificacionesRows())
                      {
                                        
                          if (string.IsNullOrEmpty(iCode) | iCode=="*")
                          {
                              foreach (dsBd.tbMaterialesRow miRow in item.GettbMaterialesRows())
                              {
                                  miLst.Add(miRow);
                              }
                          }
                          else
                          {
                              if (item.code == iCode)
                              {
                                  foreach (dsBd.tbMaterialesRow miRow in item.GettbMaterialesRows())
                                  {
                                      miLst.Add(miRow);
                                  }
                              }
                          }
                      }
                  }
              }

          


          return miLst;
 
        }
    }
}
