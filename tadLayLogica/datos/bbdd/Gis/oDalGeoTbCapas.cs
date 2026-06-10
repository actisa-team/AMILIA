using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace tadLayLogica.datos.Gis
{
    using tadLayLan;
    using tadLayData;
    using tadLayLan.Tdb;
    using tadLayLogica.datos.BaseDatos;
    
    public class oDalGeoTbCapas
    {


        public static Dictionary<eCapaCalzada, Dictionary<int,dsBd.tbCapasRow>> getCapasByZona(Guid iIdZonaMovTie)
        {


            Dictionary<eCapaCalzada, Dictionary<int, dsBd.tbCapasRow>> miDic = new Dictionary<eCapaCalzada, Dictionary<int, dsBd.tbCapasRow>>();
            List<dsBd.tbCapasRow> miLstByCode = new List<dsBd.tbCapasRow>();

            Dictionary<int,dsBd.tbCapasRow> miLstFir = new Dictionary <int,dsBd.tbCapasRow>();
            Dictionary<int,dsBd.tbCapasRow> miLstArc = new Dictionary <int,dsBd.tbCapasRow>();
            Dictionary<int,dsBd.tbCapasRow> miLstAsi = new Dictionary <int,dsBd.tbCapasRow>();


            //oDsBd miDs = oSingletonDsBd.getInstance.dataset;


            string miZonaName = oSingletonDsBd.getInstance.dataset.tbGeo.FindByid(iIdZonaMovTie).nombre;

               
                //Listado de Capas de la Sección
            var miQueryByCode = from p in oSingletonDsBd.getInstance.dataset.tbCapas
                                 where p.idGeo == iIdZonaMovTie                           
                                 select p;

                miLstByCode = miQueryByCode.ToList();

                //VALIDACIONES
                if (miLstByCode.Count == 0)
                { 
                  throw new Exception (string.Format(strFrmGisGeneral.eMOVTIEcapaIsCero,miZonaName));
                }


                //Capa Firme
                var miQueryFir = from p in miLstByCode
                                 where string.Equals(p.idCapaTipo,"FIR")
                                 orderby p.orden ascending
                                 select p;


                miLstFir = miQueryFir.ToDictionary(x => x.orden, x => x);

                //VALIDACIONES
                if (miLstFir.Count == 0)
                {
                    throw new Exception(string.Format(strFrmGisGeneral.eMOVTIEcapaFirmeIsCero, miZonaName));
                }


                //Capas Arcen
                var miQueryArc = from p in miLstByCode
                                 where string.Equals(p.idCapaTipo, "ARC")
                                 orderby p.orden ascending
                                 select p;

                miLstArc = miQueryArc.ToDictionary(x => x.orden, x => x);

                
                //VALIDACIONES
                if (miLstArc.Count == 0)
                {
                    throw new Exception(string.Format(strFrmGisGeneral.eMOVTIEcapaArcenIsCero, miZonaName));
                }


                //Capa Asiento  (La Capa de Asiento puede ser Cero)
                var miQueryAsi = from p in miLstByCode
                                 where string.Equals(p.idCapaTipo, "ASI")
                                 orderby p.orden ascending
                                 select p;

                miLstAsi = miQueryAsi.ToDictionary(x => x.orden, x => x);



                //Valido Espesores de Capas de Firme y Asiento

                double miEspesorFirme = (from p in miLstFir select p.Value.espesorM).Sum();
                double miEspesorArcen = (from p in miLstArc select p.Value.espesorM).Sum();



                if (miEspesorArcen > miEspesorFirme) 
                {
                    throw new Exception(string.Format(strFrmGisGeneral.eMOVTIEespesorArcenMayorFirme, miZonaName));
                }


                //Devuelvo los Datos
                miDic.Add(eCapaCalzada.FIR, miLstFir);
                miDic.Add(eCapaCalzada.ARC, miLstArc);
                miDic.Add(eCapaCalzada.ASI, miLstAsi);


                return miDic;

            
   

        }
        
        
        
        
        
        
        /// <summary>
        /// Selecciono las Capas Según la Zona Geoetécnica y Tipo Capa de Firme
        /// </summary>
        /// <param name="iIdGeoZona">Id Zona Geotecnica</param>
        /// <param name="iCodeCapaTipo">ASI, FIR, ARC</param>
        /// <returns>Listado de Capas</returns>
        public static List<dsBd.tbCapasRow> selectByZonaAndTipo (Guid iIdGeoZona, string iCodeCapaTipo)
        {

            //oDsBd miDs = oDsBd.getInstance();

            var miQery = from p in oSingletonDsBd.getInstance.dataset.tbCapas
                             where p.idGeo == iIdGeoZona & p.idCapaTipo == iCodeCapaTipo
                             select p;


                List<dsBd.tbCapasRow> miLst = miQery.ToList();

                return miLst;
      
            
            
            
        }




        public static dsBd.tbCapasDataTable getTbCapas()
        {
            //oDsBd miDs = oDsBd.getInstance();

            return oSingletonDsBd.getInstance.dataset.tbCapas;
            
        }


        public static void deleteCapa(int iIdCapa)
        {
            //oDsBd miDs = oDsBd.getInstance();

            oSingletonDsBd.getInstance.dataset.tbCapas.FindByid(iIdCapa).Delete();
            oSingletonDsBd.getInstance.save(true);
            
        }

        public static void deleteCapasByTipo(Guid iIdGeoZona, string iCodeCapaTipo)
        {

            //oDsBd miDs = oDsBd.getInstance();


            var miQery = from p in oSingletonDsBd.getInstance.dataset.tbCapas
                             where p.idGeo == iIdGeoZona & p.idCapaTipo == iCodeCapaTipo
                             select p;


                List<dsBd.tbCapasRow> miLstRowToDelete = miQery.ToList();

       
                if (miLstRowToDelete.Count > 0)
                {
                    foreach (dsBd.tbCapasRow item in miLstRowToDelete)
                    {
                        item.Delete();
                    }
                }

                oSingletonDsBd.getInstance.save(true);

            
        }



        public static void copyCapasByTipo(Guid iIdGeoZona, string iCodeCapaTipoToCopy, string iCodeCapaTipoToPaste)
        {


            //oDsBd miDs = oDsBd.getInstance();
            
                
                //1 Seleccionamos las Capas a Copiar
                List<dsBd.tbCapasRow> miLstRowToCopy;

                //2 Seleccionamos las Capas a Pegar // Para Borrar
                List<dsBd.tbCapasRow> miLstRowToPasteOriginal;


                var miQery = from p in oSingletonDsBd.getInstance.dataset.tbCapas
                             where p.idGeo == iIdGeoZona & p.idCapaTipo == iCodeCapaTipoToCopy
                             select p;


                miLstRowToCopy = miQery.ToList();


                var miQery2 = from p in oSingletonDsBd.getInstance.dataset.tbCapas
                             where p.idGeo == iIdGeoZona & p.idCapaTipo == iCodeCapaTipoToPaste
                             select p;

                miLstRowToPasteOriginal = miQery2.ToList();





                //Si Existen las borro
                if (miLstRowToPasteOriginal.Count > 0)
                {
                    foreach (dsBd.tbCapasRow item in miLstRowToPasteOriginal)
                    {
                        item.Delete();
                    }
                }

                //Copio los elementos
                if (miLstRowToCopy.Count > 0)
                {

                    foreach (dsBd.tbCapasRow item in miLstRowToCopy)
                    {

                        dsBd.tbCapasRow miRowNew = oSingletonDsBd.getInstance.dataset.tbCapas.NewtbCapasRow();

                        miRowNew.orden = item.orden;
                        miRowNew.espesorM = item.espesorM;
                        miRowNew.idCapaTipo = iCodeCapaTipoToPaste;
                        miRowNew.idGeo = item.idGeo;
                        miRowNew.idMaterial = item.idMaterial;

                        oSingletonDsBd.getInstance.dataset.tbCapas.AddtbCapasRow(miRowNew);
                    }


                    oSingletonDsBd.getInstance.save(true);


                

            }
        
        
        
        }





       public static void editCapa(int iIdCapa, int iCapaOrden, double iCapaEspesor, Guid iMaterial)
       {

           //oDsBd miDs = oDsBd.getInstance();

           dsBd.tbCapasRow miRow = oSingletonDsBd.getInstance.dataset.tbCapas.FindByid(iIdCapa);

               miRow.orden = iCapaOrden;
               miRow.espesorM = iCapaEspesor;
               miRow.idMaterial = iMaterial;
               oSingletonDsBd.getInstance.save(true);
           
         
       }
       public static void addCapa(Guid iZonaGeo,string iCapaTipo, int iCapaPosicion, double iCapaEspesor, Guid iMaterial)
       {

           //oDsBd miDs = oDsBd.getInstance();


           dsBd.tbCapasRow miRow = oSingletonDsBd.getInstance.dataset.tbCapas.NewtbCapasRow();

               miRow.idGeo = iZonaGeo;
               miRow.idCapaTipo = iCapaTipo;
               miRow.orden = iCapaPosicion;
               miRow.espesorM = iCapaEspesor;
               miRow.idMaterial = iMaterial;

               oSingletonDsBd.getInstance.dataset.tbCapas.AddtbCapasRow(miRow);

               oSingletonDsBd.getInstance.save(true);
           
           
     
       }
       public static void addCapasTipo()
       {

           //oDsBd miDs = oDsBd.getInstance();

           oSingletonDsBd.getInstance.dataset.tbCapasTipos.AddtbCapasTiposRow("ASI");
           oSingletonDsBd.getInstance.dataset.tbCapasTipos.AddtbCapasTiposRow("FIR");
           oSingletonDsBd.getInstance.dataset.tbCapasTipos.AddtbCapasTiposRow("ARC");

           oSingletonDsBd.getInstance.save(true);   
           
            
       }


    }
}
