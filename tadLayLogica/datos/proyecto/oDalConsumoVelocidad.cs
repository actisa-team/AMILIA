using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{

    using tadLayLan;
    using tadLayData;
    using engNet.ClassT;
    
    
    public class oDalVehiculos
    {

     /// <summary>
     /// Si no Existen Valores de Consumos los Cargamos
     /// </summary>
     public static void addConsumosDefault()
       {
                //Consulto el número de Registros
               if (oSingletonDsApp.getInstance.dataset.tbVehCon.Rows.Count==0)
               {
                 engNet.oCsvLoad miCsvLoad = new engNet.oCsvLoad(oTadil.data.Files.fileDatVehiculoConsumo,";","/",true);

                 miCsvLoad.loadCsvIntoDataTableTyped(oSingletonDsApp.getInstance.dataset.tbVehCon);
                   
                 oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbVehCon,false);
               }

       }
     public static void addMantenimientoDefault()
       {

           if (oSingletonDsApp.getInstance.dataset.tbVehMan.Rows.Count == 0)
           {
               engNet.oCsvLoad miCsvLoad = new engNet.oCsvLoad(oTadil.data.Files.fileDatVehiculoMantenimiento, ";", "/", true);
               miCsvLoad.loadCsvIntoDataTableTyped(oSingletonDsApp.getInstance.dataset.tbVehMan);
              oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbVehMan, false);
           }
       }
     public static double vehiculoLigeroConsumoCombustible(dsApp.tbVehConDataTable iTb, double iVelocidad)
     {

         var miQuery = from p in iTb
                       where p.idVehiculoTipo == "VEHLIG" && p.velocidad >= iVelocidad 
                       orderby p.velocidad ascending 
                       select p;

         if (miQuery.ToList().Count == 0)
         {
             return iTb.Where(row=> row.idVehiculoTipo=="VEHLIG").Max(row => row.consumoCombustible);
         }
         else
         {
             return miQuery.First().consumoCombustible;
         }
     }
     public static double VehiculoLigeroConsumoLubricante(dsApp.tbVehConDataTable iTb, double iVelocidad)
     {

         var miQuery = from p in iTb
                       where p.idVehiculoTipo == "VEHLIG" && p.velocidad >= iVelocidad
                       orderby p.velocidad ascending
                       select p;

         if (miQuery.ToList().Count == 0)
         {
             return iTb.Where(row => row.idVehiculoTipo == "VEHLIG").Max(row => row.consumoLubricante);
         }
         else
         {
             return miQuery.First().consumoLubricante;
         }
     }
     public static double VehiculoLigeroCosteMantenimiento(dsApp.tbVehManDataTable iTb, double iVelocidad)
     {

         var miQuery = from p in iTb
                       where p.idVehiculoTipo == "VEHLIG" && p.velocidad >= iVelocidad
                       orderby p.velocidad ascending
                       select p;

         if (miQuery.ToList().Count == 0)
         {
             return iTb.Where(row => row.idVehiculoTipo == "VEHLIG").Max(row => row.costeMantenimiento);
         }
         else
         {
             return miQuery.First().costeMantenimiento;
         }


     }
     public static double VehiculoPesadoConsumoCombustible(dsApp.tbVehConDataTable iTb, double iVelocidad)
     {

         var miQuery = from p in iTb
                       where p.idVehiculoTipo == "VEHPES" && p.velocidad >= iVelocidad
                       orderby p.velocidad ascending
                       select p;

         if (miQuery.ToList().Count == 0)
         {
             return iTb.Where(row => row.idVehiculoTipo == "VEHPES").Max(row => row.consumoCombustible);
         }
         else
         {
             return miQuery.First().consumoCombustible;
         }


     }
     public static double VehiculoPesadoconsumoLubricante(dsApp.tbVehConDataTable iTb, double iVelocidad)
     {

         var miQuery = from p in iTb
                       where p.idVehiculoTipo == "VEHPES" && p.velocidad >= iVelocidad
                       orderby p.velocidad ascending
                       select p;

         if (miQuery.ToList().Count == 0)
         {
             return iTb.Where(row => row.idVehiculoTipo == "VEHPES").Max(row => row.consumoLubricante);
         }
         else
         {
             return miQuery.First().consumoLubricante;
         }


     }
     public static double VehiculoPesadoCosteMantenimiento(dsApp.tbVehManDataTable iTb, double iVelocidad)
     {

         var miQuery = from p in iTb
                       where p.idVehiculoTipo == "VEHPES" && p.velocidad >= iVelocidad
                       orderby p.velocidad ascending
                       select p;

         if (miQuery.ToList().Count == 0)
         {
             return iTb.Where(row => row.idVehiculoTipo == "VEHPES").Max(row => row.costeMantenimiento);
         }
         else
         {
             return miQuery.First().costeMantenimiento;
         }


     }

    }
}
