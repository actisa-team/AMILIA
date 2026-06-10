using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{
    using System.IO;
    using tadLayData;
    using tadLayShare;
    using tadLayLan;

    public class oDalTbProyecto
    {
        public static void saveTable(bool iShowMensaje)
        { 
            oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbProyecto, iShowMensaje);
        }

        public static void setUpEstudio (eEstudioTipo iEstudioTipo)
        {

            if (iEstudioTipo == eEstudioTipo.ESTPRE)
            {
                oSingletonDsApp.getInstance.dataset.tbProyecto.FindByid("APP").idProyectoTipo = "ESTPRE";           
            }
            else if (iEstudioTipo == eEstudioTipo.ESTINF)
            {

                oSingletonDsApp.getInstance.dataset.tbProyecto.FindByid("APP").idProyectoTipo = "ESTINF";

            }
            else
            {
                throw new oExEnumNotImplemented(iEstudioTipo.ToString());
            }

            saveTable(false);

        }


        public static eEstudioTipo getEstudioTipo ()
        {

            dsApp.tbProyectoRow miRow = oSingletonDsApp.getInstance.dataset.tbProyecto.FindByid("APP");


            if (miRow == null)
            {
                throw new oExRowNotFound("APP", strError.eTablaProyecto);
            }

            string miEstudioTipo = miRow.idProyectoTipo;

            return  (eEstudioTipo)Enum.Parse(typeof(eEstudioTipo), miEstudioTipo, true);

        }

    }
}
