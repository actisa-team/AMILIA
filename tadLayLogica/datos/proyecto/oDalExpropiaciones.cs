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
  

    public class oDalAppExpropiacion
    {

        /// <summary>
        /// Guardar las Medicones de Expropicion Agrupadas en PRODUCION Y SUELO
        /// </summary>
        public static void addMedicionesExpropiacion  (Guid iIdSolucion, List<oMedItemModel> iLstMedicionesExpro)
        {
           
                //Borro si Existen Mediciones Anteriores
                deleteMedicionesExpropiacion(oSingletonDsApp.getInstance.dataset.tbMedicionesExpropiaciones, iIdSolucion);


                //Añado las mediciones
                dsApp.tbMedicionesExpropiacionesRow miRow;

                foreach (oMedItemModel item in iLstMedicionesExpro)
                {
                    miRow = oSingletonDsApp.getInstance.dataset.tbMedicionesExpropiaciones.NewtbMedicionesExpropiacionesRow();

                    miRow.idTbSolucion = iIdSolucion;
                    miRow.idMat = item.idMat;
                    miRow.medicion = item.medicion;
                    miRow.code = item.code.ToString();

                    oSingletonDsApp.getInstance.dataset.tbMedicionesExpropiaciones.AddtbMedicionesExpropiacionesRow(miRow);

                }

                oSingletonDsApp.getInstance.dataset.tbMedicionesExpropiaciones.AcceptChanges();

                oSingletonDsApp.getInstance.saveSinAccepChanges();
            

        }


        /// <summary>
        /// Mediciones Expropiaciones
        /// </summary>
        public static List<oMedItemModel> getMediciones(Guid iIdSolucion)
        {

            List<oMedItemModel> miLstMediciones = new List<oMedItemModel>();
            oMedItemModel miPartida;

           
              

            var miQuery = from p in oSingletonDsApp.getInstance.dataset.tbMedicionesExpropiaciones
                            where p.idTbSolucion == iIdSolucion
                            select p;

            dsBd.tbMaterialesRow miRowMat;


            foreach (var item in miQuery)
            {
                miRowMat = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(item.idMat);
                miPartida = oFactoryPartidas.createMedicionesExpropiacion(item.idMat, item.medicion, item.code);
                miLstMediciones.Add(miPartida);
            }

                

            

            return miLstMediciones;
        }


        public static void deleteMedicionesExpropiacion (dsApp.tbMedicionesExpropiacionesDataTable iTb, Guid iIdSol)
        {

            var miQuery = from p in iTb
                          where p.idTbSolucion == iIdSol
                          select p;

            miQuery.ToList().ForEach(row => iTb.Rows.Remove(row));

            iTb.AcceptChanges();

        }














       
    }
}