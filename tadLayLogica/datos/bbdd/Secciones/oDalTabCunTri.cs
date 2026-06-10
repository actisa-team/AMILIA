using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Secciones
{
    using engNet.ClassT;
    using tadLayData;


    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;
    using tadLayLan;
    
    public class oDalTabCunTri
    {
        /// <summary>
        /// Get Tabla Triangular
        /// </summary>
        public static dsBd.tbCunetaTriangDataTable getTabla()
        {
            return oSingletonDsBd.getInstance.dataset.tbCunetaTriang;  
        }
        /// <summary>
        /// Save Table
        /// </summary>
        public static void saveTabla()
        {
            oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbCunetaTriang, true);
        }
        /// <summary>
        /// Get Cuneta by ID
        /// </summary>
        public static dsBd.tbCunetaTriangRow getById(Guid iId)
        {

            dsBd.tbCunetaTriangRow miRow = oSingletonDsBd.getInstance.dataset.tbCunetaTriang.FindByid(iId);

            if (miRow == null)
            {
                throw new oExRowNotFound("id", strError.eCunetaTrapezoidal);
            }

            return miRow;               
        }
        /// <summary>
        /// Borrar Cuneta Triangular
        /// </summary>
        public static void deleteById(Guid iId)
        {
            dsBd.tbCunetaTriangRow miRow =  oDalTabCunTri.getById(iId);

            miRow.Delete();

            oDalTabCunTri.saveTabla();       
        }
        /// <summary>
        /// Obtengo un Listado de todas las Geometrias de Cunetas
        /// <summary>
        /// <remarks>(Usando 2 Tablas distintas para el datasource del combo daba probemas con el databind</remarks>
        public static List<oValDesFilT<Guid, string,string>> getLstCunetaGeometria(dsBd.tbCunetaTriangDataTable iTbCunGeoTri, dsBd.tbCunetaTrapezDataTable iTbCunGeoTra)
        {


            List<oValDesFilT<Guid, string,string>> miLst = new List<oValDesFilT<Guid, string,string>>();
          
            foreach (dsBd.tbCunetaTriangRow item in iTbCunGeoTri)
            {
                miLst.Add(new oValDesFilT<Guid, string,string>(item.id, item.nombre,item.idTbCunetaTipo));
            }

            foreach (dsBd.tbCunetaTrapezRow item in iTbCunGeoTra)
            {
                miLst.Add(new oValDesFilT<Guid, string,string>(item.id, item.nombre,item.idTbCunetaTipo));
            }

            return miLst;
      
        }
    }
}
