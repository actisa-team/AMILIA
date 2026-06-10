using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.Secciones
{

    using tadLayLogica.datos.BaseDatos;
    using tadLayData;
    using tadLayLan;


    public class oDalTabRoadUnica
    {
        public static void saveTabla()
        {
            oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbRoadUniGen, true);
        }
        public static dsBd.tbRoadUniGenDataTable getTabla()
        {
            return oSingletonDsBd.getInstance.dataset.tbRoadUniGen;     
        }
        public static dsBd.tbRoadUniGenRow getById(Guid iId)
        {

            dsBd.tbRoadUniGenRow miRow = oSingletonDsBd.getInstance.dataset.tbRoadUniGen.FindByid(iId);

            if (miRow != null)
            {
                return miRow;
            }
            else
            {
                throw new Exception(string.Format(strError.eregistroTablaCalzadaUnicaNoExiste, iId));

            }              
        }
        public static void deleteById(Guid iId)
        {
                    
            dsBd.tbRoadUniGenRow miRow = oSingletonDsBd.getInstance.dataset.tbRoadUniGen.FindByid(iId);

            if (miRow != null)
            {
               oSingletonDsBd.getInstance.dataset.tbRoadUniGen.RemovetbRoadUniGenRow(miRow);
               
               oSingletonDsBd.getInstance.save(true);
            }
            else
            {
                throw new Exception(string.Format(strError.eregistroTablaCalzadaUnicaNoExiste, iId));

            } 
         
        }
    }

}
