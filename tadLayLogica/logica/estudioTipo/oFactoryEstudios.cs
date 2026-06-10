using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.estudioTipo
{


    using tadLayLogica.datos.proyecto;
    using tadLayShare;
    
    public class oFactoryEstudios 
   {

       public static oEstudioCarretera getEstudioCarretera (eEstudioTipo iEstudioTipo, Guid iIdSolucion)
       {

           switch (iEstudioTipo)
           {
               case eEstudioTipo.ESTPRE:

                   return new oEstudioPrevioCarretera(iIdSolucion);
    
               case eEstudioTipo.ESTINF:

                  return new  oEstudioInformativoCarretera(oSingletonProyecto.getInstance.secRoadTipo,
                                                           oSingletonProyecto.getInstance.seccionCalzadaId,
                                                           oDalTbSolucionCoeSensibilidad.getCoeMinoracionAlturasMaximas(iIdSolucion),
                                                           oSingletonProyecto.getInstance.SeccionesVinculadas);

               default:

                  throw new oExEnumNotImplemented(iEstudioTipo.ToString());
                  
           }

       }

   }
}
