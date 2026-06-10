using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdb;

namespace tadLayLogica.datos.Gis
{

    using engNet.ClassT;
    using tadLayLan;
    using tadLayData;

    using tadLayLogica.datos.BaseDatos;

    public class oDalTbCim
    {

 
        /// <summary>
        /// Obtener Zona por Id
        /// </summary>
        public static dsBd.tbCimRow getZona(Guid iId)
        {
            //oDsBd miDs = oDsBd.getInstance();
            return oSingletonDsBd.getInstance.dataset.tbCim.FindByid(iId);
            
        }
        /// <summary>
        /// Eliminar Registro
        /// </summary>
        public static void deleteById(Guid iId)
        {
            //oDsBd miDs = oDsBd.getInstance();
            oSingletonDsBd.getInstance.dataset.tbCim.FindByid(iId).Delete();
            oSingletonDsBd.getInstance.save(true);
            
        }
        /// <summary>
        /// Obtener Tabla
        /// </summary>
        public static dsBd.tbCimDataTable getTabla()
        {
            //oDsBd miDs = oDsBd.getInstance();
            return oSingletonDsBd.getInstance.dataset.tbCim;
            
        }


        public static dsBd.tbCimDataTable getTablaCache()
        {
            return oSingletonDsBd.getInstance.dataset.tbCim;  
        }

        /// <summary>
        /// Devuelvo Lista
        /// string 1 --> Code CIMDIR
        /// string 2 --> Traduccion 
        /// </summary>
        public static List<oValDesT<string, string>> getLstCimTipos(string iFiltro)
        {
            
            //oDsBd miDs = oDsBd.getInstance();

                List<oValDesT<string, string>> miLstIdCode = new List<oValDesT<string, string>>();
                oValDesT<string, string> miItem;




                var miQery = from p in oSingletonDsBd.getInstance.dataset.tbCimTipos
                                 where p.filtro == iFiltro
                                 select p;

                    List<dsBd.tbCimTiposRow> miLstRow = miQery.ToList();

                    foreach (dsBd.tbCimTiposRow item in miLstRow)
                    {
                        miItem = new oValDesT<string, string>();
                        miItem.val = item.id;
                        miItem.des = strFrmGisCim.ResourceManager.GetString("ui" + item.id);
                        miLstIdCode.Add(miItem);
                    }

                
                return miLstIdCode;
           
            
        
        
        
        }
      /// <summary>
      /// AÑADIR LAS CIMENTACIONES TIPOS
      /// POR EL ID --> Traduzco las Cimentaciones
      /// Campo Descripción solo es a nivel interno
      /// filtro = Sirve para filtrar el Combo
      /// </summary>
      public static void addCimTipo()
      {

          using (oSingletonDsBd mDs = oSingletonDsBd.getInstance)
          {
   
              //Creo los tipos Cimentación
              dsBd.tbCimTiposRow miRowCimTipo = mDs.dataset.tbCimTipos.NewtbCimTiposRow();

              miRowCimTipo.id = "CIMDIR";
              miRowCimTipo.descripcion = "Cimentacion Directa";
              miRowCimTipo.filtro = "CIM";
              miRowCimTipo.valoracion = 0;

              mDs.dataset.tbCimTipos.AddtbCimTiposRow(miRowCimTipo);



            
              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("CIDISA","Cimentación Directa Saneos", "CIM",4);
              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("CIMPOZ","Cimentación Pozos", "CIM",7);
              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("CIMPRO","Cimentación Profunda", "CIM",10);

              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("EXMECO", "Excavación Medios Convencionales", "EXC");
              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("EXMANE", "Excavación Martillo Neúmatico", "EXC");
              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("EXCVOL", "Excavación Voladuras", "EXC");
              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("EXCPIL", "Excavación Ejecución Pilotes", "EXC");


              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("AGUSIN", "Sin Afección", "AGU");
              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("AGNIFR", "Agotamiento Nivel Freático", "AGU");
              //mDs.dataset.tbCimTipos.AddtbCimTiposRow("AGAGES", "Sistema Agotamiento Especiales", "AGU");

              mDs.save(true);
 
          }
      
      }




    }
}
