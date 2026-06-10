using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.datos.proyecto
{
    using System.IO;
    using tadLayData;

    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;
    using tadLayLan;

    public class oDalTbSeccionZonasGenerales
    {
       public static dsBd.tbGeoRow zonaMovimientoTierrasDefault()
        {
        
              dsApp.tbSeccionZonasGeneralesRow miRowEstInf =  oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid("APP");


              if (miRowEstInf == null)
              {
                  throw new oExRowNotFound("APP", strError.eTablaSeccionZGenerales);
              }
       

               dsBd.tbGeoRow miRowZonaDefault =  oSingletonDsBd.getInstance.dataset.tbGeo.FindByid(miRowEstInf.idZonaMovimientoTierrasGeneral);


              if (miRowZonaDefault == null)
              {
                  throw new oExRowNotFound(miRowZonaDefault.nombre, strError.eTablaMovTierras);
              }


              return miRowZonaDefault;

        }
       public static dsBd.tbTunRow zonaTunelDefault()
        {

            dsApp.tbSeccionZonasGeneralesRow miRowEstInf = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid("APP");


              if (miRowEstInf == null)
              {
                  throw new oExRowNotFound("APP", strError.eTablaSeccionZGenerales);
              }
       

               dsBd.tbTunRow miRowZonaDefault =  oSingletonDsBd.getInstance.dataset.tbTun.FindByid(miRowEstInf.idZonaTunelesGeneral);


              if (miRowZonaDefault == null)
              {
                  throw new oExRowNotFound(miRowZonaDefault.nombre, strError.eTablaTuneles);
              }


              return miRowZonaDefault;

        }
       public static dsBd.tbEstRow zonaEstructurasDefault()
        {

              dsApp.tbSeccionZonasGeneralesRow miRowEstInf = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid("APP");


              if (miRowEstInf == null)
              {
                  throw new oExRowNotFound("APP", strError.eTablaSeccionZGenerales);
              }
       

              dsBd.tbEstRow miRowZonaDefault =  oSingletonDsBd.getInstance.dataset.tbEst.FindByid(miRowEstInf.idZonaEstructurasGeneral);


              if (miRowZonaDefault == null)
              {
                  throw new oExRowNotFound(miRowZonaDefault.nombre, strError.eTablaEstructuras);
              }


              return miRowZonaDefault;

        }
       public static dsBd.tbCimRow zonaCimentacionDefault()
        {

            dsApp.tbSeccionZonasGeneralesRow miRowEstInf = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid("APP");


              if (miRowEstInf == null)
              {
                  throw new oExRowNotFound("APP", strError.eTablaEstInfTDB);
              }
       

               dsBd.tbCimRow miRowZonaDefault =  oSingletonDsBd.getInstance.dataset.tbCim.FindByid(miRowEstInf.idZonaCimentacionGeneral);


              if (miRowZonaDefault == null)
              {
                  throw new oExRowNotFound(miRowZonaDefault.nombre, strError.eTablaCimentacion);
              }


              return miRowZonaDefault;

        }
    }
}
