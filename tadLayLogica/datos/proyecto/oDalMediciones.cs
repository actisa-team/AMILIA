using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;


namespace tadLayLogica.datos.proyecto
{



    using engCadNet;

    using tadLayLogica.logica.medicion;
    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;
  

    public class oDalAppMedicones
    {

        /// <summary>
        /// Guardar las Mediciones Obtenidas del CAD en la TABLA
        /// </summary>
        public static void addMedicionesCad (Guid iIdSolucion, List<oMedItemModel> iLstMedicionesCad)
        {     
                //Borro si Existen Mediciones Anteriores
                deleteMedicionesCad(oSingletonDsApp.getInstance.dataset.tbMedicionesCad, iIdSolucion);

                //Añado las mediciones
                dsApp.tbMedicionesCadRow miRow;

                foreach (oMedItemModel item in iLstMedicionesCad)
                {
                    miRow = oSingletonDsApp.getInstance.dataset.tbMedicionesCad.NewtbMedicionesCadRow();

                    miRow.idTbSolucion = iIdSolucion;
                    miRow.idMat = item.row.idMaterial;
                    miRow.medicion = item.medicion;
                    miRow.isBalanceTierras = item.isBalanceTierras;
                    miRow.isPrecioPrincipal = item.isPrecioPrincipal.Value;
                    miRow.code = item.code.ToString();

                    oSingletonDsApp.getInstance.dataset.tbMedicionesCad.AddtbMedicionesCadRow(miRow);

                }

                oSingletonDsApp.getInstance.dataset.tbMedicionesCad.AcceptChanges();

                oSingletonDsApp.getInstance.saveSinAccepChanges();        
        }



        /// <summary>
        /// Mediciones Partidas CAD
        /// </summary>
        public static List<oMedItemModel> getMedicionesCad(Guid iIdSolucion)
        {

            List<oMedItemModel> miLstMedicionesCad = new List<oMedItemModel>();
            oMedItemModel miPartida;
            
            
            var miQuery = from p in oSingletonDsApp.getInstance.dataset.tbMedicionesCad
                            where p.idTbSolucion == iIdSolucion
                            select p;

            dsBd.tbMaterialesRow miRowMat; 


            foreach (var item in miQuery)
            {
                miRowMat =  oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(item.idMat);

                miPartida = oFactoryPartidas.createPartidasFromTbMedicionesCad(miRowMat, item.medicion, item.code, item.isBalanceTierras, item.isPrecioPrincipal);

                miLstMedicionesCad.Add(miPartida);
            }

 
            return miLstMedicionesCad;       
        }


        /// <summary>
        /// Obtengo solo las Partidas de Exvacacion de la Medicion CAD
        /// </summary>
        /// <param name="iIdSolucion"></param>
        /// <returns></returns>
        public static List<oMedItemModel> getMedicionesCadByGrupo (Guid iIdSolucion, eNodo iNodo)
        {

            List<oMedItemModel> miLstMedicionesCad = new List<oMedItemModel>();
            oMedItemModel miPartida;

           
            var miQuery = from p in oSingletonDsApp.getInstance.dataset.tbMedicionesCad
                            where p.idTbSolucion == iIdSolucion && p.code == iNodo.ToString()
                            select p;

            dsBd.tbMaterialesRow miRowMat;


            foreach (var item in miQuery)
            {
                miRowMat = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(item.idMat);

                miPartida = oFactoryPartidas.createPartidasFromTbMedicionesCad(miRowMat, item.medicion, item.code, item.isBalanceTierras, item.isPrecioPrincipal);

                miLstMedicionesCad.Add(miPartida);
            }

  
            return miLstMedicionesCad;  

        }

        /// <summary>
        /// Obtener las Partidas de MacroPrecios del Proyecto
        /// </summary>
        public static List<oMedItemModel> getMedicionesMacroPrecios(Guid iIdSolucion)
        { 
              //Obtenemos el Guid del MacroPrecioAsociado al Proyecto 
               Guid miMacroGuid = Guid.Empty;
               double?  miTrazadoLonKM = null;

                dsApp.tbSeccionZonasGeneralesRow miRowEstInformativoTdb = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid("APP");


                 if (miRowEstInformativoTdb.IsidRoadMacroPreciosNull())
                   {
                       throw new oExValorUserNoImplementado(strError.eProyectoMacroPrecios);
                   }
                   else
                   {
                       miMacroGuid = miRowEstInformativoTdb.idRoadMacroPrecios;
                   }

                   if (oSingletonDsApp.getInstance.dataset.tbSolucion.FindByid(iIdSolucion).IshandleEjeTrazadoNull())
                   {
                       throw new oExValorUserNoImplementado(strError.eSolucionEjeTrazado);
                   }
                   else
                   {
                       miTrazadoLonKM = oSingletonDsApp.getInstance.dataset.tbSolucion.FindByid(iIdSolucion).longitudKm;
                   }
               


              
              //Obtengo los MacroPrecios de la Base Datos
               List<oMedItemModel> miLst = new List<oMedItemModel>();
               oMedItemModel miPartida =null;
               dsBd.tbMaterialesRow miMatRow;

             
                   dsBd.tbMacroPreciosRow miMacroPreciosRow = oSingletonDsBd.getInstance.dataset.tbMacroPrecios.FindByid(miMacroGuid);

                  
                   //DRENAJE
                   if (! miMacroPreciosRow.IsdrenajeNull())
                   {
                       miMatRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miMacroPreciosRow.drenaje);
                       miPartida = new oMedDrenaje(miMatRow, miTrazadoLonKM.Value);
                       miLst.Add(miPartida);
                   }

                   //BALIZAMIENTO
                   if (!miMacroPreciosRow.IsbalizamientoNull())
                   {
                       miMatRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miMacroPreciosRow.balizamiento);
                       miPartida = new oMedBalizamiento(miMatRow, miTrazadoLonKM.Value);
                       miLst.Add(miPartida);
                   }

                   //REPOSICION SERVICIOS
                   if (!miMacroPreciosRow.IsserviciosReposicionNull())
                   {
                       miMatRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miMacroPreciosRow.serviciosReposicion);
                       miPartida = new oMedReposicionServicios(miMatRow, miTrazadoLonKM.Value);
                       miLst.Add(miPartida);
                   }

                   //CORRECIONES GEOTECNICAS
                   if (!miMacroPreciosRow.IsgeotecnicaCorreccionesNull())
                   {
                       miMatRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miMacroPreciosRow.geotecnicaCorrecciones);
                       miPartida = new oMedGeotecnicaCorreccion(miMatRow, miTrazadoLonKM.Value);
                       miLst.Add(miPartida);
                   }

                   //DESVIOS PROVISIONALES
                   if (!miMacroPreciosRow.IsdesviosProvisionalesNull())
                   {
                       miMatRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miMacroPreciosRow.desviosProvisionales);
                       miPartida = new oMedDesviosProvisionales(miMatRow, miTrazadoLonKM.Value);
                       miLst.Add(miPartida);
                   }

                   //ACTUACIONES COMPLEMENTARIAS
                   if (!miMacroPreciosRow.IsactuacionesComplemenNull())
                   {
                       miMatRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miMacroPreciosRow.actuacionesComplemen);
                       miPartida = new oMedActuacionesComple(miMatRow, miTrazadoLonKM.Value);
                       miLst.Add(miPartida);
                   }

                   //MEDIDAS CORRECTORAS
                   if (!miMacroPreciosRow.IsmedidasCorrectorasNull())
                   {
                       miMatRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miMacroPreciosRow.medidasCorrectoras);
                       miPartida = new oMedMedidasCorrectoras(miMatRow, miTrazadoLonKM.Value);
                       miLst.Add(miPartida);
                   }

                   //SEGURIDAD Y SALUD
                   if (!miMacroPreciosRow.IsseguridadSaludNull())
                   {
                       miMatRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miMacroPreciosRow.seguridadSalud);
                       miPartida = new oMedSeguridadSalud(miMatRow);
                       miLst.Add(miPartida);
                   }
                   else
                   {
                       throw new oExValorUserNoImplementado(strError.eMacroPreciosSeguridadSauld);
                   }


                   return miLst;
               
              
        }


        public static void deleteMedicionesCad(dsApp.tbMedicionesCadDataTable iTb, Guid iIdSol)
        {
            try
            {

                var miQuery = from p in iTb
                              where p.idTbSolucion == iIdSol
                              select p;

                miQuery.ToList().ForEach(row => iTb.Rows.Remove(row));

                iTb.AcceptChanges();
            }
            catch (Exception)
            {
            }

        }
      
    }
}