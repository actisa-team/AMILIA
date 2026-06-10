using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{


    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.logica.presupuesto;

    
    public class oRentabilidadCore
    {

        protected Guid solId;
        protected oSingletonDsApp dsApp;

        private oHistoricoTasa mHisTasaActualizacion = null;
        private oHistoricoTasa mHisTasaPreciosConstruccion = null;
        private oHistoricoTasa mHisTasaIpc = null;
        private oHistoricoTasa mHisTasaSubvencion = null;


 

        private oHistoricoConstruccion mHisConstruccionParteEstatal = null;
        private oHistoricoConstruccion mHisConstruccionPartePrivada = null;


        private oHistoricoTrafico mRoad0TraficoHis = null;
        private oHistoricoTrafico mRoad10TraficoHis = null;
        private oHistoricoTrafico mRoad11TraficoHis = null;

        private oHistoricoPeajes mHisPeaje = null;
        private oHistoricoSubvenciones mHisSubvencion = null;
        private oHistoricoExplotacionEmpleo mHisEmpleo = null;

        private oRenCosteFunTieVehiculoLigero mRoad0VehLigero = null;
        private oRenCosteFunTieVehiculoPesado mRoad0VehPesado = null;

        private oRenCosteFunTieVehiculoLigero mRoad1VehLigero = null;
        private oRenCosteFunTieVehiculoPesado mRoad1VehPesado = null;


        private oHistoricoFuncionamientoTiempo mRoad0HisFunTie = null;
        private oHistoricoFuncionamientoTiempo mRoad10HisFunTie = null;
        private oHistoricoFuncionamientoTiempo mRoad11HisFunTie = null;

        private oHistoricoAccidentes mRoad0HisAccidentes = null;
        private oHistoricoAccidentes mRoad10HisAccidentes = null;
        private oHistoricoAccidentes mRoad11HisAccidentes = null;

        private oHistoricoConservacionRehabilitacionRoad mRoad0HisConservacionEstatal= null;
        private oHistoricoConservacionRehabilitacionRoad mRoad10HisConservacionEstatal = null;
        private oHistoricoConservacionRehabilitacionRoad mRoad11HisConservacionEstatal = null;
        private oHistoricoConservacionRehabilitacionRoad mRoad11HisConservacionPrivada = null;

        private oHistoricoExplotacionSeguros mRoad11ExplotacionSegurosPrivada = null;





        public oRentabilidadCore(Guid iIdSol)
        {

            solId = iIdSol;
            dsApp = oSingletonDsApp.getInstance;
            
        }


       protected int yearsTotal
        {
           get
            {
                return yearsConstruccion + yearsExplotacion;

            }

        }
       protected int yearsConstruccion
       { 
           get
           {
               return dsApp.indicesDatosTemporales.numberYearConstruccion;
           }
           
       }

       protected int yearsExplotacion
       { 
           get
           {
               return dsApp.indicesDatosTemporales.numberYearExplotacion;
           }
       }
       protected double road0LonKm
       {
           get
           {
               return dsApp.datosTrafico.roadCeroLongitudKm;
           }

       }
       protected double road1LonKm
       {
           get
           {
               return dsApp.getSolucion(solId).longitudKm;
           }

       }


       protected oHistoricoTasa tasaActualizacion
       {
           get
           {
               if (mHisTasaActualizacion == null)
               {
                   mHisTasaActualizacion= new oHistoricoTasa(dsApp.indicesDatosTemporales.tasaActualizacionPC, dsApp.indicesDatosTemporales.tasaActualizacionPC, yearsTotal);
               }

               return mHisTasaActualizacion;
           }
       }
       protected oHistoricoTasa tasaPreciosConstruccion
       {
           get
           {
               if (mHisTasaPreciosConstruccion == null)
               {
                   mHisTasaPreciosConstruccion = new oHistoricoTasa(dsApp.indicesDatosTemporales.tasaRevisionPreciosPC, yearsConstruccion);
               }

               return mHisTasaPreciosConstruccion;
           }
       }
       protected oHistoricoTasa tasaIpc
        {
            get
            {
                if (mHisTasaIpc == null)
                {

                    mHisTasaIpc = new oHistoricoTasa(dsApp.indicesDatosTemporales.tasaIpcPC, yearsExplotacion);
                }

                return mHisTasaIpc;
            }
        }
       protected oHistoricoTasa tasaSubvencion
        {
            get
            {
                if (mHisTasaSubvencion == null)
                {
                    mHisTasaSubvencion=new oHistoricoTasa(dsApp.isSubvencionEstatalUpdateIpc, dsApp.indicesDatosTemporales.tasaIpcPC, yearsExplotacion);
                }

                return mHisTasaSubvencion;
            }
        }
   
       protected oHistoricoTrafico road0Trafico
       {
           get
           {
               if (mRoad0TraficoHis == null)
               {
                   mRoad0TraficoHis = new oHistoricoTrafico(dsApp.datosTrafico.roadCeroImd, dsApp.datosTrafico.roadCeroCrecimientoAnualPC, 100, 100, dsApp.indicesDatosTemporales.numberYearExplotacion);                                                                    
               }

               return mRoad0TraficoHis;
           }


       }
       protected oHistoricoTrafico road10Trafico
       { 
          
           get
           {
               if (mRoad10TraficoHis == null)
               {
                   mRoad10TraficoHis = new oHistoricoTrafico(dsApp.datosTrafico.roadCeroImd,
                                                             dsApp.datosTrafico.roadCeroCrecimientoAnualPC,
                                                             100 - dsApp.datosTrafico.roadUnoAbsorcionTraficoInicioPC,
                                                             100 - dsApp.datosTrafico.roadUnoAbsorcionTraficoFinalPC,
                                                             dsApp.indicesDatosTemporales.numberYearExplotacion);

               }

               return mRoad10TraficoHis;

           }
       
       }
       protected oHistoricoTrafico road11Trafico
       {

           get
           {
               if (mRoad11TraficoHis == null)
               {
                   mRoad11TraficoHis = new oHistoricoTrafico(dsApp.datosTrafico.roadCeroImd,
                                                             dsApp.datosTrafico.roadCeroCrecimientoAnualPC,
                                                             dsApp.datosTrafico.roadUnoAbsorcionTraficoInicioPC,
                                                             dsApp.datosTrafico.roadUnoAbsorcionTraficoFinalPC,
                                                             dsApp.indicesDatosTemporales.numberYearExplotacion);

               }

               return mRoad11TraficoHis;

           }

       }

       protected oHistoricoConstruccion construccionRentabilidadPublica
       {
           get
           {
               if (mHisConstruccionParteEstatal == null)
               {                 
                   oPresupuestoPcaPublicoPrivado miPcaParteEstatal = new oPresupuestoPcaPublicoPrivado(solId);
                   mHisConstruccionParteEstatal= new oHistoricoConstruccion(miPcaParteEstatal.presupuestoConstruccionRentabilidadPublico.Value, yearsConstruccion);
               }

               return mHisConstruccionParteEstatal; 
           }
       }

       protected oHistoricoConstruccion construccionRentabilidadPrivada
       {
           get
           {
               if (mHisConstruccionPartePrivada == null)
               {                
                   oPresupuestoPcaPublicoPrivado miPcaPartePrivada = new oPresupuestoPcaPublicoPrivado(solId);
                   mHisConstruccionPartePrivada = new oHistoricoConstruccion(miPcaPartePrivada.presupuestoConstruccionRentabilidadPrivada.Value,yearsConstruccion);
               }

               return mHisConstruccionPartePrivada;
           }
       }

       protected oHistoricoPeajes peajes
       {

           get
           {
               if (mHisPeaje == null)
               {

           
                   mHisPeaje = new oHistoricoPeajes(dsApp.isInversionPrivada,
                                                    dsApp.precioPeaje,
                                                    dsApp.indicesDatosTemporales.numberYearExplotacion,
                                                    road11Trafico.traficoByYear);

               }

               return mHisPeaje;
           }


       }
       protected oHistoricoSubvenciones subvenciones
       {

           get
           {
               if (mHisSubvencion == null)
               {

                   mHisSubvencion = new oHistoricoSubvenciones(dsApp.isInversionPrivada,
                                                               dsApp.subvencionEstatalVehiculo,   
                                                               dsApp.subvencionEstatalAnual,
                                                               dsApp.indicesDatosTemporales.numberYearExplotacion,
                                                               road11Trafico.traficoByYear);

               }

               return mHisSubvencion;


           }


       }

       protected oRenCosteFunTieVehiculoLigero road0vehiculoLigero
       {

           get
           {
               if (mRoad0VehLigero == null)
               {

                   double miVpRoad0 = dsApp.datosTrafico.roadCeroVelocidadMedia;

                   mRoad0VehLigero = new oRenCosteFunTieVehiculoLigero(miVpRoad0,
                                                                       oDalVehiculos.vehiculoLigeroConsumoCombustible(dsApp.dataset.tbVehCon, miVpRoad0),
                                                                       oDalVehiculos.VehiculoLigeroConsumoLubricante(dsApp.dataset.tbVehCon, miVpRoad0),
                                                                       dsApp.vehiculoCostes.costeCombustible,
                                                                       dsApp.vehiculoCostes.costeLubricante,
                                                                       dsApp.vehiculoCostes.costeAmortizacionesVehiculoLigero,
                                                                       dsApp.vehiculoCostes.costeNeumaticoVehiculoLigero,
                                                                       oDalVehiculos.VehiculoLigeroCosteMantenimiento(dsApp.dataset.tbVehMan,miVpRoad0),
                                                                       dsApp.vehiculoCostes.costeTiempoVehiculoLigero,
                                                                       dsApp.vehiculoCostes.coePonderacionTiempoVehiculoLigero);



               }

               return mRoad0VehLigero;


           }

       }
       protected oRenCosteFunTieVehiculoPesado road0VehiculoPesado
       {

           get
           {
               if (mRoad0VehPesado == null)
               {

                   double miVpRoad0 = dsApp.datosTrafico.roadCeroVelocidadMedia;

                    mRoad0VehPesado = new oRenCosteFunTieVehiculoPesado(miVpRoad0,
                                                                       oDalVehiculos.VehiculoPesadoConsumoCombustible(dsApp.dataset.tbVehCon, miVpRoad0),
                                                                       oDalVehiculos.VehiculoPesadoconsumoLubricante(dsApp.dataset.tbVehCon, miVpRoad0),
                                                                       dsApp.vehiculoCostes.costeCombustible,
                                                                       dsApp.vehiculoCostes.costeLubricante,
                                                                       dsApp.vehiculoCostes.costeAmortizacionesVehiculoPesado,
                                                                       dsApp.vehiculoCostes.costeNeumaticoVehiculoPesado,
                                                                       oDalVehiculos.VehiculoPesadoCosteMantenimiento(dsApp.dataset.tbVehMan, miVpRoad0),
                                                                       dsApp.vehiculoCostes.costeTiempoVehiculoPesado,
                                                                       dsApp.vehiculoCostes.coePonderacionTiempoVehiculoPesado);



               }

               return mRoad0VehPesado;


           }

       }
       protected oRenCosteFunTieVehiculoLigero road1vehiculoLigero
       {

           get
           {
               if (mRoad1VehLigero == null)
               {

                   double miVpRoad1 = dsApp.getSolucionRoad(solId).velocidadProyecto;

                   mRoad1VehLigero = new oRenCosteFunTieVehiculoLigero(miVpRoad1,
                                                                       oDalVehiculos.vehiculoLigeroConsumoCombustible(dsApp.dataset.tbVehCon, miVpRoad1),
                                                                       oDalVehiculos.VehiculoLigeroConsumoLubricante(dsApp.dataset.tbVehCon, miVpRoad1),
                                                                       dsApp.vehiculoCostes.costeCombustible,
                                                                       dsApp.vehiculoCostes.costeLubricante,
                                                                       dsApp.vehiculoCostes.costeAmortizacionesVehiculoLigero,
                                                                       dsApp.vehiculoCostes.costeNeumaticoVehiculoLigero,
                                                                       oDalVehiculos.VehiculoLigeroCosteMantenimiento(dsApp.dataset.tbVehMan, miVpRoad1),
                                                                       dsApp.vehiculoCostes.costeTiempoVehiculoLigero,
                                                                       dsApp.vehiculoCostes.coePonderacionTiempoVehiculoLigero);



               }

               return mRoad1VehLigero;


           }

       }
       protected oRenCosteFunTieVehiculoPesado road1vehiculoPesado
       {

           get
           {
               if (mRoad1VehPesado == null)
               {

                   double miVpRoad1 = dsApp.getSolucionRoad(solId).velocidadProyecto;

                   mRoad1VehPesado = new oRenCosteFunTieVehiculoPesado(miVpRoad1,
                                                                       oDalVehiculos.VehiculoPesadoConsumoCombustible(dsApp.dataset.tbVehCon, miVpRoad1),
                                                                       oDalVehiculos.VehiculoPesadoconsumoLubricante(dsApp.dataset.tbVehCon, miVpRoad1),
                                                                       dsApp.vehiculoCostes.costeCombustible,
                                                                       dsApp.vehiculoCostes.costeLubricante,
                                                                       dsApp.vehiculoCostes.costeAmortizacionesVehiculoPesado,
                                                                       dsApp.vehiculoCostes.costeNeumaticoVehiculoPesado,
                                                                       oDalVehiculos.VehiculoPesadoCosteMantenimiento(dsApp.dataset.tbVehMan, miVpRoad1),
                                                                       dsApp.vehiculoCostes.costeTiempoVehiculoPesado,
                                                                       dsApp.vehiculoCostes.coePonderacionTiempoVehiculoPesado);



               }

               return mRoad1VehPesado;


           }

       }

      protected oHistoricoFuncionamientoTiempo road0CostesFuncionamientoTiempo
       {

         get
           {
               if (mRoad0HisFunTie == null)
               {

                   mRoad0HisFunTie = new oHistoricoFuncionamientoTiempo(road0vehiculoLigero,
                                                                        road0VehiculoPesado,
                                                                        dsApp.datosTrafico.roadCeroVehiculosPesadosPC,
                                                                        dsApp.indicesDatosTemporales.numberYearExplotacion);
                                                                       



               }

               return mRoad0HisFunTie;


           }



       }
      protected oHistoricoFuncionamientoTiempo road10CostesFuncionamientoTiempo
      {

          get
          {
              if (mRoad10HisFunTie == null)
              {
                
                      mRoad10HisFunTie = new oHistoricoFuncionamientoTiempo(road0vehiculoLigero,
                                                                            road0VehiculoPesado,
                                                                            dsApp.datosTrafico.roadCeroVehiculosPesadosPC,
                                                                            dsApp.indicesDatosTemporales.numberYearExplotacion);
                                                                                    
              }

              return mRoad10HisFunTie;
          }



      }
      protected oHistoricoFuncionamientoTiempo road11CostesFuncionamientoTiempo
      {

          get
          {
              if (mRoad11HisFunTie == null)
              {

                  mRoad11HisFunTie = new oHistoricoFuncionamientoTiempo(road1vehiculoLigero,
                                                                        road1vehiculoPesado,
                                                                        dsApp.datosTrafico.roadCeroVehiculosPesadosPC,
                                                                        dsApp.indicesDatosTemporales.numberYearExplotacion);
                                                                       



              }

              return mRoad11HisFunTie;


          }



      }

      protected oHistoricoAccidentes road0Accidentes
      {
          get
          {
              if (mRoad0HisAccidentes == null)
              {

                  mRoad0HisAccidentes = new oHistoricoAccidentes(road0LonKm,
                                                                dsApp.costesAccidentes.roadCeroIM,
                                                                dsApp.costesAccidentes.roadCeroIP,
                                                                dsApp.costesAccidentes.numHeridosPorAccidente,
                                                                dsApp.costesAccidentes.costeMuerte,
                                                                dsApp.costesAccidentes.costeHerido,
                                                                dsApp.indicesDatosTemporales.numberYearExplotacion,
                                                                road0Trafico.traficoByYear);                                                
              }

              return mRoad0HisAccidentes;
          }

      }
      protected oHistoricoAccidentes road10Accidentes
      {
          get
          {
              if (mRoad10HisAccidentes == null)
              {

                  mRoad10HisAccidentes = new oHistoricoAccidentes(road0LonKm,
                                                                dsApp.costesAccidentes.roadCeroIM,
                                                                dsApp.costesAccidentes.roadCeroIP,
                                                                dsApp.costesAccidentes.numHeridosPorAccidente,
                                                                dsApp.costesAccidentes.costeMuerte,
                                                                dsApp.costesAccidentes.costeHerido,
                                                                dsApp.indicesDatosTemporales.numberYearExplotacion,
                                                                road10Trafico.traficoByYear);
              }

              return mRoad10HisAccidentes;
          }

      }
      protected oHistoricoAccidentes road11Accidentes
      {
          get
          {
              if (mRoad11HisAccidentes == null)
              {


                  mRoad11HisAccidentes = new oHistoricoAccidentes(road1LonKm,
                                                                  dsApp.costesAccidentes.roadUnoIM,
                                                                  dsApp.costesAccidentes.roadUnoIP,
                                                                  dsApp.costesAccidentes.numHeridosPorAccidente,
                                                                  dsApp.costesAccidentes.costeMuerte,
                                                                  dsApp.costesAccidentes.costeHerido,
                                                                  dsApp.indicesDatosTemporales.numberYearExplotacion,
                                                                  road11Trafico.traficoByYear);
              }

              return mRoad11HisAccidentes;
          }

      }

      protected oHistoricoConservacionRehabilitacionRoad road0ConservacionEstatal
      {
          get
          {
              if (mRoad0HisConservacionEstatal == null)
              {
                  mRoad0HisConservacionEstatal = new oHistoricoConservacionRehabilitacionRoad0ParteEstatal(dsApp.datosTrafico.roadCeroLongitudKm,
                                                                                                    dsApp.costeConservacionMantenimiento.roadCeroConservar,
                                                                                                    dsApp.costeConservacionMantenimiento.roadCeroRehabilitar,
                                                                                                    dsApp.indicesDatosTemporales.numberYearExplotacion);   


              }

              return mRoad0HisConservacionEstatal;
          }
      }
      protected oHistoricoConservacionRehabilitacionRoad road10ConservacionEstatal 
      {
          get
          {
              if (mRoad10HisConservacionEstatal == null)
              {

                  if (dsApp.datosTrafico.roadConexionActualEliminar)
                  {
                      mRoad10HisConservacionEstatal = new oHistoricoConservacionRehabilitacionCosteCero(dsApp.indicesDatosTemporales.numberYearExplotacion);
                  }
                  else
                  {
                      mRoad10HisConservacionEstatal = new oHistoricoConservacionRehabilitacionRoad0ParteEstatal(dsApp.datosTrafico.roadCeroLongitudKm,
                                                                                  dsApp.costeConservacionMantenimiento.roadCeroConservar,
                                                                                  dsApp.costeConservacionMantenimiento.roadCeroRehabilitar,
                                                                                  dsApp.indicesDatosTemporales.numberYearExplotacion);
                  }
              }

              return mRoad10HisConservacionEstatal;
          }
      }
      protected oHistoricoConservacionRehabilitacionRoad road11ConservacionEstatal
      {
          get
          {
              if (mRoad11HisConservacionEstatal == null)
              {
                  mRoad11HisConservacionEstatal = new oHistoricoConservacionRehabilitacionRoad11ParteEstatal(dsApp.isInversionPrivada,
                                                                                                            road1LonKm,
                                                                                                            dsApp.costeConservacionMantenimiento.roadUnoConservar,
                                                                                                            dsApp.costeConservacionMantenimiento.roadUnoRehabilitar,
                                                                                                            dsApp.indicesDatosTemporales.numberYearExplotacion);

              }

              return mRoad11HisConservacionEstatal;
          }
      }
      protected oHistoricoConservacionRehabilitacionRoad road11ConservacionPrivada
      {
          get
          {
              if (mRoad11HisConservacionPrivada == null)
              {
                  mRoad11HisConservacionPrivada = new oHistoricoConservacionRehabilitacionRoad11PartePrivada(dsApp.isInversionPrivada,
                                                                                                            road1LonKm,
                                                                                                            dsApp.costeConservacionMantenimiento.roadUnoConservar,
                                                                                                            dsApp.costeConservacionMantenimiento.roadUnoRehabilitar,
                                                                                                            dsApp.indicesDatosTemporales.numberYearExplotacion);

              }

              return mRoad11HisConservacionPrivada;
          }
      }

      protected oHistoricoExplotacionSeguros road11ExplotacionSegurosPrivada
      {

          get
          {
              if (mRoad11ExplotacionSegurosPrivada == null)
              {

                  mRoad11ExplotacionSegurosPrivada = new oHistoricoExplotacionSeguros(dsApp.isInversionPrivada,
                                                                                      dsApp.gastosExplotacion,
                                                                                      dsApp.gastosExplotacionManoObraPC,
                                                                                      dsApp.gastosSegurosyOtros,
                                                                                      yearsExplotacion);
              }

              return mRoad11ExplotacionSegurosPrivada;
          }
      }

    }
}
