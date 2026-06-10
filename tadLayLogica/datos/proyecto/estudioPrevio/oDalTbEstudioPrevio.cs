using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdi;

namespace tadLayLogica.datos.proyecto.estudioPrevio
{

    using tadLayData;
    using tadLayLogica.estudioTipo;
    using tadLayLan;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayShare;

   public class oDalTbSolucionEstudioPrevio
   {
       public static dsApp.tbSolucionEstudioPrevioRow getRowSolucionEstudioPrevioBySolucion (Guid iIdSolucion)
       {
       
            dsApp.tbSolucionEstudioPrevioRow miRow = oSingletonDsApp.getInstance.dataset.tbSolucionEstudioPrevio.FindByid(iIdSolucion);

           if (miRow == null)
           {
               throw new oExRowNotFound("IdSolucion", "tbSolucionEstudioPrevio");
           }
           else
           {
                return miRow;
           }

       }
       public static IzonaMovimientoTierras getZonaMovimientoTierrasBySolucion (Guid iIdSolucion)
       {

          
            dsApp.tbSolucionEstudioPrevioRow miRow = getRowSolucionEstudioPrevioBySolucion(iIdSolucion);
            dsApp.tbSolucionCoeMinAlturasRow miRowCoe = oDalTbSolucionCoeSensibilidad.getRowTbSolucionCoeSensibilidad(iIdSolucion);

            oEstudioMovimientoTierras miEstudioPrevioZonaMovimientoTierras =  new oEstudioMovimientoTierras(miRow.id,
                                                                                                            "estudioPrevio",
                                                                                                            miRow.desmonteAlturaMaxima,
                                                                                                            miRowCoe.desmonteCoeMinoracionAlturaMaximaProyecto,
                                                                                                            miRow.desmonteTalud,
                                                                                                            miRow.desmonteCosteUnitario,
                                                                                                            miRow.terraplenAlturaMaxima,
                                                                                                            miRowCoe.terraplenCoeMinoracionAlturaMaximaProyecto,
                                                                                                            miRow.terraplenTalud,
                                                                                                            miRow.terraplenCosteUnitario);
           return miEstudioPrevioZonaMovimientoTierras;

       }
       public static ISeccionCalzada getSeccionCalzadaBySolucion (Guid iIdSolucion)
       {
                dsApp.tbSolucionEstudioPrevioRow miRow = getRowSolucionEstudioPrevioBySolucion(iIdSolucion);



                oEstudioRoadSeccion miEstudioPrevioSeccion =  new oEstudioRoadSeccion(miRow.id,
                                                                                      "estudioPrevio",
                                                                                       miRow.calzadaAncho,
                                                                                       miRow.calzadaCosteUnitario);

              return miEstudioPrevioSeccion;


       }
       public static IzonaPuentes getZonaPuentesBySolucion (Guid iIdSolucion)
       {
           
           dsApp.tbSolucionEstudioPrevioRow miRow = getRowSolucionEstudioPrevioBySolucion(iIdSolucion);
           dsApp.tbSolucionCoeMinAlturasRow miRowCoe = oDalTbSolucionCoeSensibilidad.getRowTbSolucionCoeSensibilidad(iIdSolucion);

           oEstudioPuentes miEstudioPrevioPuente =  new oEstudioPuentes(miRow.id,
                                                                        strFrmDatosProyecto.uiESTPRE,  
                                                                        strFrmDatosProyecto.uiESTPRE,
                                                                        miRow.puenteAllow,
                                                                        miRow.puenteAlturaMaximaPila,
                                                                        miRowCoe.puenteCoeMinoracionAlturaMaximaPilaProyecto,        
                                                                        miRow.puenteCosteUnitario);
           return miEstudioPrevioPuente;

       }
       public static IzonaTuneles getZonaTunelesBySolucion (Guid iIdSolucion)
       {

           dsApp.tbSolucionEstudioPrevioRow miRow = getRowSolucionEstudioPrevioBySolucion(iIdSolucion);


           oEstudioTuneles miEstudioPrevioTunel = new oEstudioTuneles(miRow.id,
                                                                      strFrmDatosProyecto.uiESTPRE,
                                                                      strFrmDatosProyecto.uiESTPRE,  
                                                                      miRow.tunelAllow, 
                                                                      miRow.tunelCosteUnitario);


           return miEstudioPrevioTunel;

       }
   }
}
