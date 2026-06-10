using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{

    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayData;
    using tadLayShare;
    
    
    public class oDalTbSolucionPendientes
   {

        public static dsApp.tbSolucionPendienteRow getRowTbSolucionPendiente (Guid iIdSolucion)
        {

            dsApp.tbSolucionPendienteRow miRow = oSingletonDsApp.getInstance.dataset.tbSolucionPendiente.FindByid(iIdSolucion);


            if (miRow == null)
            {
                throw new oExRowNotFound("Id Solucion", "TbSolucionPendiente");
            }

            return miRow;
        }


        public static oRoadPendientes getPendientesBySolucion (Guid iIdSolucion)
        {

            dsApp.tbSolucionPendienteRow miRow = oDalTbSolucionPendientes.getRowTbSolucionPendiente(iIdSolucion);


            oRoadPendientes miRoadPendiente = new oRoadPendientes(miRow.calzadaPendienteMaximaProyectoPC,
                                                                    miRow.calzadaPendienteMinimaProyectoPC,
                                                                    miRow.estructurasPendienteMaximaProyectoPC,
                                                                    miRow.estructuraPendienteMinimaProyectoPC,
                                                                    miRow.calzadaCoeMinoracionPendienteMaximaProyecto,
                                                                    miRow.estructuraCoeMinoracionPendienteMaximaProyecto);
                                                       


            return miRoadPendiente;

        }


     
       

   }
}
