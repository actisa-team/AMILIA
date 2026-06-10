using System;
using System.Collections.Generic;
using System.Linq;

namespace tadLayLogica.datos.proyecto
{
    using Autodesk.AutoCAD.Geometry;
    
    using tadLayLogica.zonaGis.secciones;
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayData;


    public class oDalTbObraLinealSecciones
    {

        public static void save(Guid iIdSolucion, List<oSeccionEjeTrazado> iLstSecciones)
        {

            //Elimino los Registros
            oDalTbObraLinealSecciones.deleteSeccionesById(iIdSolucion);


            //Añado las mediciones
            dsApp.tbObraLinealSeccionRow miRow;

            foreach (oSeccionEjeTrazado item in iLstSecciones)
            {
                miRow = oSingletonDsApp.getInstance.dataset.tbObraLinealSeccion.NewtbObraLinealSeccionRow();

                miRow.idTbSolucion = iIdSolucion;
                miRow.pk = item.pk.Value;
                miRow.seccionTipo = item.seccionTipo.Value.ToString();
                miRow.idZonaMovimientoTierras = item.zonaMovimientoTierras.id;
                miRow.idZonaCimentacion = item.zonaCimentacion.id;
                miRow.idZonaPuentes = item.zonaPuentes.id;
                miRow.idZonaTuneles = item.zonaTuneles.id;
             

                oSingletonDsApp.getInstance.dataset.tbObraLinealSeccion.AddtbObraLinealSeccionRow(miRow);

            }

            oSingletonDsApp.getInstance.dataset.tbObraLinealSeccion.AcceptChanges();

            oSingletonDsApp.getInstance.saveSinAccepChanges();
        }

        public static void deleteSeccionesById(Guid iIdSol)
        {
            try
            {
                var miQuery = from p in oSingletonDsApp.getInstance.dataset.tbObraLinealSeccion
                              where p.idTbSolucion == iIdSol
                              select p;


                miQuery.ToList().ForEach(row => oSingletonDsApp.getInstance.dataset.tbObraLinealSeccion.Rows.Remove(row));
            }
            catch (Exception)
            {
            }

            oSingletonDsApp.getInstance.dataset.tbObraLinealSeccion.AcceptChanges();

            oSingletonDsApp.getInstance.saveSinAccepChanges();

        }


        /// <summary>
        /// Listado con las Secciones de Diseño de la Carretera
        /// </summary>
        public static oCollectionZonasDesign  getCollectionZonasDesignOrderByPkAscending(Guid iIdSolucion,Func<double,Point3d> iFunGetPtoFromPk)
        {

            oCollectionZonasDesign miColZonasDesign = new oCollectionZonasDesign();
            oSeccionZonasDesign miSeccion;

            Point3d miPtoPk;

                var miQuery = from p in oSingletonDsApp.getInstance.dataset.tbObraLinealSeccion
                              where p.idTbSolucion == iIdSolucion
                              orderby p.pk ascending
                              select p;

              
                foreach (var item in miQuery)
                {
                    miPtoPk = iFunGetPtoFromPk(item.pk);
                    

                    miSeccion = new oSeccionZonasDesign(item.pk,miPtoPk,item.seccionTipo,item.idZonaMovimientoTierras,item.idZonaCimentacion,item.idZonaPuentes,item.idZonaTuneles);

                  miColZonasDesign.Add(miSeccion);
                }

                return miColZonasDesign;

            }

        public static List<double> getListPksSecciones(Guid iIdSolucion)
        {
            var listPksSecciones = new List<double>();


            var miQuery = from p in oSingletonDsApp.getInstance.dataset.tbObraLinealSeccion
                          where p.idTbSolucion == iIdSolucion
                          select p;

            foreach (var item in miQuery)
            {
                listPksSecciones.Add(item.pk);
            }

            return listPksSecciones;
        }


    }
}
