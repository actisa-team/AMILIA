using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace tadLayLogica.logica.EjeBasicoNew
{

    using engNet.Extension.Double;

    using tadLayShare.puntos;

    using System.Collections.ObjectModel;

    using tadLayLogica.zonaGis;
    using tadLayShare;


   public  class oSeccionesEjeBasico : Collection<oSeccionEjeBasico>
   {


       #region "Constructor"

       public oSeccionesEjeBasico()
       {

       }


       #endregion



       #region "Metodos Publicos"


       /// <summary>
       /// Coste Implantación Total del Tramo
       /// </summary>
       public double valoracionCosteImplantacionSuma()
       {

           double miCosteImplantacionSuma = (from p in this select p.costeImplantacionTotalBySeccion()).Sum();

           return miCosteImplantacionSuma;
       }
       /// <summary>
       /// Obtener la Valoración de las Secciones por Pendiente del Terreno
       /// </summary>
       public double valoracionSlopeSuma()
       {
          double miSlopeLocalSum = (from p in this select p.valoracionSlopeBySeccion()).Sum();

          return miSlopeLocalSum;
       }
       /// <summary>
       /// Sección es NO valida
       /// </summary>
       /// <remarks>Si la Sección Tipo NO se puede generar entonces se marca como NULL</remarks>
       public bool isSeccionesNoValidas()
       {
           var miQuery = from p in this
                         where p.seccionTipo == null
                         select p;


           int miNumeroSeccionesNoValidas = miQuery.Count();



           if (miNumeroSeccionesNoValidas > 0)
           {
               return true;
           }
           else
           {
               return false;
           }

       }
       /// <summary>
       /// Calcular Tramo Sin Estructuras
       /// </summary>
       public void calculateSeccionSinEstructuras()
       {
           foreach (var item in this)
           {
               item.seccionTipo = this.getApoyoSinEstructuras(item);
           }
       }
       /// <summary>
       /// Calcular Sección Permitiendo Estructuras
       /// </summary>
       public void calcultateSeccionConEstructuras()
       {
           foreach (var item in this)
           {
               item.seccionTipo = this.getApoyoConEstructuras(item);
           }
       }


       public void calcultateSeccionObligadoPuente()
       {
           foreach (var item in this)
           {
               if (item.isObligadoEstructura)
               {
                   item.seccionTipo = this.getApoyoObligadoPuente(item);
               }
               else
               {
                   item.seccionTipo = this.getApoyoConEstructuras(item);
               }

           }
       }


       public void calcultateSeccionObligadoPuenteoTunel()
       {
           foreach (var item in this)
           {
               if (item.isObligadoEstructura)
               {
                   item.seccionTipo = this.getApoyoObligadoPuenteoTunel(item);
               }
               else
               {
                   item.seccionTipo = this.getApoyoConEstructuras(item);
               }

           }
       }




       /// <summary>
       /// Sección Tiene Estructuras¿?
       /// </summary>
       /// <returns></returns>
       public bool hasEstructuras()
       {

           var miQuery = from p in this
                         where p.seccionTipo.Value == eRoadSeccion.puente | p.seccionTipo.Value == eRoadSeccion.tunel
                         select p;


           int miCount = miQuery.Count();

           if (miCount > 0)
           {
               return true;
           }
           else
           {
               return false;
           }

       }
       /// <summary>
       /// El tramo Permite estructuras
       /// </summary>
       /// <returns></returns>
       public bool allowEstructuras()
       {
           int miNumeroSeccionesPermitenTuneles = (from p in this where p.zonaTunel.allowTuneles.Value select p).Count();
           int miNumeroSeccionesPermitenPuentes = (from p in this where p.zonaPuente.allowPuentes.Value select p).Count();

           if (miNumeroSeccionesPermitenPuentes == 0 && miNumeroSeccionesPermitenTuneles == 0)
           {
               return false;
           }
           else
           {
               return true;
           }

       }

       /// <summary>
       /// Represento los Puntos de las Secciones con sus valores
       /// </summary>
       public void draw(string iCapa)
       {
           foreach (var item in this)
           {
               item.draw(iCapa);
           }
       }


       /// <summary>
       /// Metodo para Conocer las Estructuras por Tramo
       /// </summary>
       ///<remarks>Se Utiliza solo para el ajuste del perfil longitudina</remarks>
       public eEstructuraOld getEstructurasTramo()
       {

                //OJO AL PERITIR QUE LOS TRAMOS INICIALES Y FINALES PUEDAN INCUMPLIR
                

                //Consulta si la Seccion Contiene Puentes y Tuneles
                var myQueryPuentesyTuneles=  from p in this
                                where p.seccionTipo == eRoadSeccion.puente & p.seccionTipo == eRoadSeccion.tunel
                                select p;

                //Consulta Seccion Tiene Puente
                var myQueryPue = from p in this
                                 where p.seccionTipo== eRoadSeccion.puente   
                                 select p;

                var myQueryTun = from p in this
                                 where p.seccionTipo == eRoadSeccion.tunel
                                 select p;




                if (myQueryPuentesyTuneles.Count()>0)
                {
                    return eEstructuraOld.TunelPila;
                }
                else if (myQueryTun.Count() > 0)
                {
                    return eEstructuraOld.Tunel;
                }
                else if (myQueryPue.Count() > 0)
                {
                    return eEstructuraOld.Pila;
                }
                else
                {
                    return eEstructuraOld.SinEstructura;
                }

            }
        
       #endregion




       #region "Metodos Privados"


       private eRoadSeccion? getApoyoSinEstructuras(oSeccionEjeBasico iSeccion)
       {

           if (iSeccion.excavacionTipo == eExcavacion.acota)
           {
               return eRoadSeccion.calzada;
           }
           else if (iSeccion.excavacionTipo == eExcavacion.desmonte)
           {

               if (iSeccion.zonaMovimientoTierras.desmonteAlturaMaximaCalculo > iSeccion.alturaMovimientoTierrasAbs)
               {
                   return eRoadSeccion.calzada;
               }
               else
               {
                   return null;
               }
           }
           else if (iSeccion.excavacionTipo == eExcavacion.terraplen)
           {
               if (iSeccion.zonaMovimientoTierras.terraplenAlturaMaximaCalculo > iSeccion.alturaMovimientoTierrasAbs)
               {
                   return eRoadSeccion.calzada;
               }
               else
               {
                   return null;
               }
           }
           else
           {
               throw new oExEnumNotImplemented(iSeccion.excavacionTipo.ToString());
           }

       }

       private eRoadSeccion? getApoyoConEstructuras(oSeccionEjeBasico iSeccion)
       {

           if (iSeccion.excavacionTipo == eExcavacion.acota)
           {
               return eRoadSeccion.calzada;
           }
           else if (iSeccion.excavacionTipo == eExcavacion.desmonte)
           {
               if (iSeccion.zonaMovimientoTierras.desmonteAlturaMaximaCalculo.Value > iSeccion.alturaMovimientoTierrasAbs)
               {
                   return eRoadSeccion.calzada;
               }
               else
               {
                   if (iSeccion.zonaTunel.allowTuneles.Value)
                   {
                       return eRoadSeccion.tunel;
                   }
                   else
                   {
                       return null;
                   }
               }
           }
           else if (iSeccion.excavacionTipo == eExcavacion.terraplen)
           {

               if (iSeccion.zonaMovimientoTierras.terraplenAlturaMaximaCalculo.Value > iSeccion.alturaMovimientoTierrasAbs)
               {
                   return eRoadSeccion.calzada;
               }
               else
               {
                   if (iSeccion.zonaPuente.allowPuentes.Value && iSeccion.zonaPuente.puenteAlturaMaximaCalculo.Value > iSeccion.alturaMovimientoTierrasAbs)
                   {
                       return eRoadSeccion.puente;
                   }
                   else
                   {
                       return null;
                   }
               }
           }
           else
           {
               throw new oExPropertieNullValue(iSeccion.excavacionTipo.ToString());
           }


       }

       private eRoadSeccion? getApoyoObligadoPuente(oSeccionEjeBasico iSeccion)
       {

           if (iSeccion.excavacionTipo == eExcavacion.acota)
           {
               return eRoadSeccion.puente;
           }
           else if (iSeccion.excavacionTipo == eExcavacion.desmonte)
           {
               return eRoadSeccion.puente;
           }
           else if (iSeccion.excavacionTipo == eExcavacion.terraplen)
           {

               if (iSeccion.zonaMovimientoTierras.terraplenAlturaMaximaCalculo.Value > iSeccion.alturaMovimientoTierrasAbs)
               {
                   return eRoadSeccion.puente;
               }
               else
               {
                   if (iSeccion.zonaPuente.allowPuentes.Value && iSeccion.zonaPuente.puenteAlturaMaximaCalculo.Value > iSeccion.alturaMovimientoTierrasAbs)
                   {
                       return eRoadSeccion.puente;
                   }
                   else
                   {
                       return null;
                   }
               }
           }
           else
           {
               throw new oExPropertieNullValue(iSeccion.excavacionTipo.ToString());
           }

       }

       private eRoadSeccion? getApoyoObligadoPuenteoTunel(oSeccionEjeBasico iSeccion)
       {
           //acota ---> puente
           if (iSeccion.excavacionTipo == eExcavacion.acota)
           {
               return eRoadSeccion.puente;
           }
           else if (iSeccion.excavacionTipo == eExcavacion.desmonte)
           {
               //desmonte ---> tunel
               if (iSeccion.zonaMovimientoTierras.desmonteAlturaMaximaCalculo.Value > iSeccion.alturaMovimientoTierrasAbs)
               {
                   if (iSeccion.zonaTunel.allowTuneles.Value)
                   {
                       return eRoadSeccion.tunel;
                   }
                   else
                   {
                       return null;
                   }
               }
               else
               {
                   if (iSeccion.zonaTunel.allowTuneles.Value)
                   {
                   return eRoadSeccion.tunel;
                   }
                   else
                   {
                     return null;
                   }
               }
           }
           else if (iSeccion.excavacionTipo == eExcavacion.terraplen)
           {

               //terraplen ---> puente
               if (iSeccion.zonaMovimientoTierras.terraplenAlturaMaximaCalculo.Value > iSeccion.alturaMovimientoTierrasAbs)
               {
                   return eRoadSeccion.puente;
               }
               else
               {
                   if (iSeccion.zonaPuente.allowPuentes.Value && iSeccion.zonaPuente.puenteAlturaMaximaCalculo.Value > iSeccion.alturaMovimientoTierrasAbs)
                   {
                       return eRoadSeccion.puente;
                   }
                   else
                   {
                       return null;
                   }
               }
           }
           else
           {
               throw new oExPropertieNullValue(iSeccion.excavacionTipo.ToString());
           }


       }

       #endregion






       }

   


    
   
}
