using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayLogica.datos.proyecto
{
    using tadLayLogica;
    using System.Data;
    using tadLayLan;
    using tadLayData;
    using tadLayShare;
    
    
    public class oSingletonDsApp :IDisposable
    {



        private dsApp.tbFilesRow mFiles = null;
        private dsApp.tbProyectoRow mProyecto = null;


        private dsApp.tbTerrenoRow mTerreno = null;

        private dsApp.tbEstilosVisualizacionRow mEstilosVisualizacion = null;



        private dsApp.tbInvTipRow mInvPrivada = null;
        private dsApp.tbInDaTeRow mInDate = null;
        private dsApp.tbDatTraRow mDatTra = null;
        private dsApp.tbCoTiFuRow mCoTiFu = null;
        private dsApp.tbCosAccRow mCosAcc = null;
        private dsApp.tbGaCoReRow mGaCoRe = null;

        private dsApp.tbValTrazadoRow mValoracionTrazado = null;
        private dsApp.tbValGeotecniaRow mValoracionGeotecnia = null;
        private dsApp.tbValEstructurasTunelesRow mValoracionEstructuraTunel = null;
        private dsApp.tbValMedioAmbientalRow mValoracionMedioAmbiental =null;
        private dsApp.tbValClimaticasRow mValoracionClimatica = null;
        private dsApp.tbValSocioEconomicasRow mValoracionSocioEconomicas = null;
        private dsApp.tbValPatrimonialesRow mValoracionPatrimonial = null;
        private dsApp.tbValEconomicaRow mValoracionEconomica = null;

        private static dsApp mDs =null;

 
        private static oSingletonDsApp mInstance =null;

       #region "Constructores"

       private oSingletonDsApp()
       {     
         mDs = new dsApp();

         mDs.ReadXml(oTadil.data.Files.fileApp);
        }


        public static oSingletonDsApp getInstance
       {
            get
           {
               if (mInstance == null)
               {
                   mInstance = new oSingletonDsApp();

                  // oTadilCore.evReset += new EventHandler<engNet.eventos.oEventArgs<bool>>(oTadilCore_evReset);
               }

               return mInstance;
           }
       }

        static void oTadilCore_evReset(object sender, engNet.eventos.oEventArgs<bool> e)
        {
            if (e.Value)
            {

                oTadilCore.evReset -= new EventHandler<engNet.eventos.oEventArgs<bool>>(oTadilCore_evReset);
                mInstance.Dispose();

               
            }
        }

        public static void deleteInstance()
        {

            if (mDs != null)
            {
                mDs.Clear();
                mDs.Dispose();
                mDs = null;
            }
    
            mInstance = null;
        }

       #endregion





       #region "PROPIEDADES"

   
       /// <summary>
       /// DataSet
       /// </summary>
       public dsApp dataset
       {
           get
           {
               return mDs;
           }
       }



       public dsApp.tbFilesRow files
       {

           get
           {

               if (mFiles == null)
               {
                   mFiles = mDs.tbFiles.FindByid("APP");      
               }

               return mFiles;       
           }
       }
 

        public dsApp.tbProyectoRow proyecto
       {
           get
           {
               if (mProyecto == null)
               { 
                  mProyecto= mDs.tbProyecto.FindByid("APP");               
               }

               return mProyecto;
           } 
       }


        public dsApp.tbTerrenoRow terreno
        {
            get
            {

                if (mTerreno == null)
                {
                    mTerreno = mDs.tbTerreno.FindByid("APP");

                    if (mTerreno == null)
                    {
                       throw new oExRowNotFound("APP", "tbTerreno");
                    }
           
                }

                return mTerreno;

            }

        }


      public dsApp.tbEstilosVisualizacionRow estilosVisualizacion
      {

          get
          {
              if (mEstilosVisualizacion == null)
              {

                  mEstilosVisualizacion = mDs.tbEstilosVisualizacion.FindByid("APP");


                  if (mEstilosVisualizacion == null)
                  {
                      throw new oExRowNotFound("APP", "TablaEstiloVisualizacion");
                  }

                  if (mEstilosVisualizacion.HasErrors)
                  {
                      throw new Exception(strError.eEstiloVisualizacion);
                  }

              }

              return mEstilosVisualizacion;

          }

      }


      public dsApp.tbSolucionRow getSolucion(Guid iIdSol)
       {

           dsApp.tbSolucionRow miRow = mDs.tbSolucion.FindByid(iIdSol);

           if (miRow == null)
           {
               throw new Exception(string.Format(strError.eSolucionNoExiste, iIdSol));
           }
           else
           {
               return miRow;
           }


       }

      public dsApp.tbSolucionRoadRow getSolucionRoad (Guid iIdSol)
      {

          dsApp.tbSolucionRoadRow miRow = mDs.tbSolucionRoad.FindByid(iIdSol);

          if (miRow == null)
          {
              throw new Exception(string.Format(strError.eSolucionNoExiste, iIdSol));
          }
          else
          {
              return miRow;
          }


      }

      public void solucionSave()
      {
          mDs.tbSolucion.AcceptChanges();
          mDs.WriteXml(oTadil.data.Files.fileApp);

      }




      #region "InversionPrivada"


      private dsApp.tbInvTipRow inversionPrivada
      {
          get
          {
              if (mInvPrivada == null)
              {
                  mInvPrivada = mDs.tbInvTip.FindByid("APP");
              }

              return mInvPrivada;

          }
      }


        public bool isInversionPrivada
      {
          get
          {
              if (inversionPrivada.IsisInversionPrivadaNull())
              {
                  throw new Exception(strError.eInversionPrivada);
              }
              else
              {
                  return inversionPrivada.isInversionPrivada;
              }
          }
      }
        public double precioPeaje 
       {
            get
          {
              if (inversionPrivada.IsprecioPeajeNull())
              {
                  return 0;
              }
              else
              {
                  return inversionPrivada.precioPeaje;
              }
          }

      }
        public double subvencionEstatalAnual
        {
            get
            {
                if (inversionPrivada.IssubvencionEstatalAnualNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.subvencionEstatalAnual;
                }


            }



        }
        public double subvencionEstatalVehiculo
        {
            get
            {
                if (inversionPrivada.IssubvencionEstatalVehiculoNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.subvencionEstatalVehiculo;
                }
            }

        }
        public bool isSubvencionEstatalFija
        {
            get
            {
                if (inversionPrivada.IsisSubvencionEstatalFijaNull())
                {
                    return false;
                }
                else
                {
                    return inversionPrivada.isSubvencionEstatalFija;
                }
            }


        }
        public bool isSubvencionEstatalUpdateIpc
        {
            get
            {
                if (inversionPrivada.IsisSubvencionEstatalUpdateIpcNull())
                {
                    return false;
                }
                else
                {
                    return inversionPrivada.isSubvencionEstatalUpdateIpc;
                }
            }


        }
        public double partePrivadaPresupuestoLicitacionPC
        {
            get
            {

                if (inversionPrivada.IspartePrivadaPresupuestoLicitacionPCNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.partePrivadaPresupuestoLicitacionPC;
                }


            }


        }
        public double partePrivadaExpropiacionesPC
        {
            get
            {

                if (inversionPrivada.IspartePrivadaExpropiacionesPCNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.partePrivadaExpropiacionesPC;
                }


            }


        }
        public double partePrivadaConservacionPatrimonioPC
        {
            get
            {

                if (inversionPrivada.IspartePrivadaConservacionPatrimonioPCNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.partePrivadaConservacionPatrimonioPC;
                }


            }


        }
        public double partePrivadaControlCalidadPC
        {
            get
            {

                if (inversionPrivada.IspartePrivadaControlCalidadPCNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.partePrivadaControlCalidadPC;
                }


            }


        }
        public double partePrivadaRestauracionPaisajisticaPC
        {
            get
            {

                if (inversionPrivada.IspartePrivadaRestauracionPaisajisticaPCNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.partePrivadaRestauracionPaisajisticaPC;
                }


            }


        }
        public double partePrivadaOtrosPC
        {
            get
            {

                if (inversionPrivada.IspartePrivadaOtrosPCNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.partePrivadaOtrosPC;
                }


            }


        }
        public double gastosExplotacion
        {
            get
            {
                if (inversionPrivada.IsgastosExplotacionNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.gastosExplotacion;
                }



            }

        }
        public double gastosSegurosyOtros
        {
            get
            {
                if (inversionPrivada.IsgastosSegurosyOtrosNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.gastosSegurosyOtros;
                }



            }

        }
        public double gastosExplotacionManoObraPC
        {
            get
            {
                if (inversionPrivada.IsgastosExplotacionManoObraPCNull())
                {
                    return 0;
                }
                else
                {
                    return inversionPrivada.gastosExplotacionManoObraPC;
                }



            }

        }
        
      #endregion




      public dsApp.tbInDaTeRow indicesDatosTemporales
      {
          get
          {
              if (mInDate == null)
              {
                  mInDate = mDs.tbInDaTe.FindByid("APP");
              }

              return mInDate;
          }
      }
      public dsApp.tbDatTraRow datosTrafico
      {
          get
          {
              if (mDatTra == null)
              {
                  mDatTra = mDs.tbDatTra.FindByid("APP");
              }
              return mDatTra;
          }
      }
      public dsApp.tbCoTiFuRow vehiculoCostes
      {
          get
          {
              if (mCoTiFu == null)
              {
                  mCoTiFu = mDs.tbCoTiFu.FindByid("APP");

              }

              return mCoTiFu;
          }
      }
      public dsApp.tbCosAccRow costesAccidentes
      {
          get
          {
              if (mCosAcc == null)
              {
                  mCosAcc = mDs.tbCosAcc.FindByid("APP");
              }

              return mCosAcc;
          }

      }
      public dsApp.tbGaCoReRow costeConservacionMantenimiento
      {
          get
          {
              if (mGaCoRe == null)
              {
                  mGaCoRe = mDs.tbGaCoRe.FindByid("APP");
              }

              return mGaCoRe;
          }




      }

      #region "Valoraciones-Propiedades"


        public dsApp.tbValTrazadoRow valoracionTrazado
          {
              get
              {
                  if (mValoracionTrazado == null)
                  {
                      mValoracionTrazado = mDs.tbValTrazado.FindByid("APP");
                  }

                  return mValoracionTrazado;
              }
          }
        public dsApp.tbValGeotecniaRow valoracionGeotecnia
        {

            get
            {
                if (mValoracionGeotecnia == null)
                {
                    mValoracionGeotecnia = mDs.tbValGeotecnia.FindByid("APP");
                }

                return mValoracionGeotecnia;
            }

        }
        public dsApp.tbValEstructurasTunelesRow valoracionEstructuraTunelMuro
        {
            get
            {
                if (mValoracionEstructuraTunel == null)
                {
                    mValoracionEstructuraTunel = mDs.tbValEstructurasTuneles.FindByid("APP");
                }

                return mValoracionEstructuraTunel;
            }
        }
        public dsApp.tbValMedioAmbientalRow valoracionMedioAmbiental
        {
            get
            {
                if (mValoracionMedioAmbiental == null)
                {
                    mValoracionMedioAmbiental = mDs.tbValMedioAmbiental.FindByid("APP");
                }

                return mValoracionMedioAmbiental;
            }
        }
        public dsApp.tbValClimaticasRow valoracionClimatica
        {
            get
            {
                if (mValoracionClimatica == null)
                {
                    mValoracionClimatica = mDs.tbValClimaticas.FindByid("APP");
                }

                return mValoracionClimatica;
            }
        }
        public dsApp.tbValSocioEconomicasRow valoracionSocioEconomicas
        {
            get
            {
                if (mValoracionSocioEconomicas == null)
                {
                    mValoracionSocioEconomicas = mDs.tbValSocioEconomicas.FindByid("APP");
                }

                return mValoracionSocioEconomicas;
            }
        }
        public dsApp.tbValPatrimonialesRow valoracionPatrimoniales
        {
            get
            {
                if (mValoracionPatrimonial == null)
                {
                    mValoracionPatrimonial = mDs.tbValPatrimoniales.FindByid("APP");
                }

                return mValoracionPatrimonial;
            }

        }
        public dsApp.tbValEconomicaRow valoracionEconomica
        {
            get
            {
                if (mValoracionEconomica == null)
                {
                    mValoracionEconomica = mDs.tbValEconomica.FindByid("APP");
                }

                return mValoracionEconomica;
            }


        }
      #endregion


        #region "Valoraciones-Hipotesis"

        public dsApp.tbMatrizDecisionRow getHipotesisValoracion (int iIdHipotesis)
        {
            return mDs.tbMatrizDecision.FindByid(iIdHipotesis);
        }

        #endregion

       #endregion

        public void saveDataTable(DataTable iTb, bool iShowInfo)
       {
           iTb.AcceptChanges();
           mDs.WriteXml(oTadil.data.Files.fileApp);
           if (iShowInfo) 
           {
               oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk); 
           }
       
       }


       public void saveProyecto()
       {
           mDs.tbProyecto.AcceptChanges();
           mDs.WriteXml(oTadil.data.Files.fileApp);
       }

       public void saveSinAccepChanges()
       {
           mDs.WriteXml(oTadil.data.Files.fileApp);
       }
       public void save()
       {
           this.dataset.AcceptChanges();
           this.dataset.WriteXml(oTadil.data.Files.fileApp);
           oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
       }

       public void Dispose()
       {     
               mDs.Dispose();
               mInstance = null;         
       }



    }
}
