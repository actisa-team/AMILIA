using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{

    using tadLayLogica.logica.EjeBasicoNew;

    using tadLayData;
    using tadLayShare;
    
    
    public class oDalTbSolucionCoeSensibilidad
   {

        public static dsApp.tbSolucionCoeMinAlturasRow getRowTbSolucionCoeSensibilidad (Guid iIdSolucion)
        {

            dsApp.tbSolucionCoeMinAlturasRow miRow = oSingletonDsApp.getInstance.dataset.tbSolucionCoeMinAlturas.FindByid(iIdSolucion);


            if (miRow == null)
            {
                throw new oExRowNotFound("Id Solucion", "TbSolucionCoeSensibilidad");
            }

            return miRow;
        }



        public static oCoeMinoracionAlturasMaximas getCoeMinoracionAlturasMaximas (Guid iIdSolucion)
        {

            dsApp.tbSolucionCoeMinAlturasRow  miRow = getRowTbSolucionCoeSensibilidad(iIdSolucion);



            oCoeMinoracionAlturasMaximas miCoeMinAlturas = new oCoeMinoracionAlturasMaximas(miRow.desmonteCoeMinoracionAlturaMaximaProyecto,
                                                                                             miRow.terraplenCoeMinoracionAlturaMaximaProyecto,
                                                                                             miRow.puenteCoeMinoracionAlturaMaximaPilaProyecto);

            return miCoeMinAlturas;



        }

       

   }
}
