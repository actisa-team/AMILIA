using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{

    using System.IO;
    using tadLayData;

    using tadLayShare.puntos;
    using tadLayShare;
    using tadLayLan;

    public class oDalTbPtoIniFin
    {
        public static void saveTable()
        {
            oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbPtoIniFin, true);
        }


        /// <summary>
        /// Obtener el Pto Salida - Llegada
        /// </summary>
        public static oP3dSalidaLlegada getPtoSalidaLlegada (ePtoSalidaLlegada iPto)
       {

            int? miIdPto = null;

            if (iPto == ePtoSalidaLlegada.puntoSalida)
            {
                miIdPto = 0;
            }
            else if (iPto== ePtoSalidaLlegada.puntoLlegada)
            {
               miIdPto = 1;    
            }
            else
            {
               throw new oExEnumNotImplemented(iPto.ToString());
            }


           dsApp.tbPtoIniFinRow miPtoRow = oSingletonDsApp.getInstance.dataset.tbPtoIniFin.FindByid(miIdPto.Value);


           if (miPtoRow == null)
           {
               throw new oExRowNotFound(iPto.ToString(), strError.eTablaPuntoIniFin);
           }
           else
           {
               double? miAzimutGrados = null;
               double? miLongitudMetros = null;
               double? miPendiente = null;
               bool? miIsLonMinimaRecta = null;

               if (! miPtoRow.IsazimutGradosNull())
               {
                   miAzimutGrados = miPtoRow.azimutGrados;
               }

               if (!miPtoRow.IslongitudMetrosNull())
               {
                   miLongitudMetros = miPtoRow.longitudMetros;
               }

               if (!miPtoRow.IspendientePCNull())
               {
                   miPendiente = miPtoRow.pendientePC;
               }

               if (!miPtoRow.IsisLongitudMinimaRectaNull())
               {
                   miIsLonMinimaRecta = miPtoRow.isLongitudMinimaRecta;
               }
               else
               {
                   miIsLonMinimaRecta = false;
               }
            

               oP3dSalidaLlegada miPtoIniFin = new oP3dSalidaLlegada(miPtoRow.X,
                                                                     miPtoRow.Y,
                                                                     miPtoRow.Z,
                                                                     miAzimutGrados,
                                                                     miLongitudMetros,
                                                                     miPendiente,
                                                                     miIsLonMinimaRecta);

               return miPtoIniFin;

           }

       }
  
    }

}
