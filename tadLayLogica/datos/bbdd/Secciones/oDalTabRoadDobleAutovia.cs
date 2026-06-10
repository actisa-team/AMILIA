using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Secciones
{

    using tadLayData;

    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;
    using tadLayLan;

    public class oDalTabRoadDobleAutovia
    {

        //SAVE DATATABLE
        public static void saveTabla()
        {
            oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia, true);
        }



        //GET TABLA
        public static dsBd.tbRoadDobleAutoviaDataTable getTabla()
        {
           return oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia;   
        }


        //GET ROW BY ID
        public static dsBd.tbRoadDobleAutoviaRow getAutoviaById (Guid iId)
        { 
               
             dsBd.tbRoadDobleAutoviaRow miRow = oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia.FindByid(iId);

             if (miRow == null)
             {
                 throw new oExRowNotFound(iId.ToString(), strError.eDobleAutovia);
             }

             return miRow;
            
        }

        //DELETE ROW BY ID
        public static void deleteById(Guid iId)
        {
            oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia.FindByid(iId).Delete();

            oDalTabRoadDobleAutovia.saveTabla();     
        }


      



    }


   

}
