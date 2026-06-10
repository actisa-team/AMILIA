using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.presupuesto
{
   
    using System.ComponentModel;

    using engNet.Extension.Double;

    using tadLayLogica.informes;

    using tadLayLogica.datos.proyecto;
    using tadLayLogica.logica.medicion;
    using tadLayLan.Tdi;
    
    /// <summary>
    /// PRESUPUESTO CONOCIMIENTO ADMINISTRACION PUBLICO-PRIVADO (PCA)
    /// </summary>
    public class oPresupuestoPcaPublicoPrivado:oPresupuestoPcaPublico
    {

        
        public oPresupuestoPcaPublicoPrivado(Guid iIdSol)
            :base(iIdSol)
        {
            postConstructor();
        }

        #region "Propiedades"
        [DefaultValue(null)]
        public double? invPrivadaPresupuestoBaseLicitacionPC { get; private set; }
        [DefaultValue(null)]
        public double? invPrivadaExpropiacionPC { get; private set; }
        [DefaultValue(null)]
        public double? invPrivadaConservacionPatrimonioPC { get; private set; }
        [DefaultValue(null)]
        public double? invPrivadaControlCalidadPC { get; private set; }
        [DefaultValue(null)]
        public double? invPrivadaRestauracionPaisajisticaPC { get; private set; }
        [DefaultValue(null)]
        public double? invPrivadaOtrosPC { get; private set; }

        [DefaultValue(null)]
        public double presupuestoBaseLicitacionPrivada
        { 
            get
            {
                return presupuestoBaseLicitacionConIva.Value * (invPrivadaPresupuestoBaseLicitacionPC.Value / 100);
            }
        
        }
        [DefaultValue(null)]
        public double presupuestoBaseLicitacionPublica
        {
            get
            {
                return presupuestoBaseLicitacionConIva.Value * (100 - invPrivadaPresupuestoBaseLicitacionPC.Value) / 100;
            }

        }


        [DefaultValue(null)]
        public double? presupuestoExpropiacionesPrivada
        {
            get
            {
                return presupuestoExpropiaciones.Value * (invPrivadaExpropiacionPC.Value / 100);
            }
        }

        [DefaultValue(null)]
        public double? presupuestoExpropiacionesPublica
        {
            get
            {
                return presupuestoExpropiaciones.Value * ((100 - invPrivadaExpropiacionPC.Value) / 100);
            }
        }

        [DefaultValue(null)]
        public double? presupuestoConservacionPatrimonioPrivada
        {
            get
            {
                return (presupuestoConservacionPatrimonio * (invPrivadaConservacionPatrimonioPC/ 100));
            }
        }

        [DefaultValue(null)]
        public double? presupuestoConservacionPatrimonioPublica
        {
            get
            {
                return (presupuestoConservacionPatrimonio * ((100-invPrivadaConservacionPatrimonioPC) / 100));
            }
        }



        [DefaultValue(null)]
        public double? presupuestoControlCalidadAdicionalPrivada
        {
            get
            {
                return presupuestoControlCalidadAdicional * (invPrivadaControlCalidadPC / 100);
            }
        }

        [DefaultValue(null)]
        public double? presupuestoControlCalidadAdicionalPublica
        {
            get
            {
                return presupuestoControlCalidadAdicional * ((100-invPrivadaControlCalidadPC) / 100);
            }
        }

        [DefaultValue(null)]
        public double? presupuestoRestauracionPaisajisticaPrivada
        {
            get
            {
                return presupuestoRestauracionPaisajistica * (invPrivadaRestauracionPaisajisticaPC / 100);
            }
        }


        [DefaultValue(null)]
        public double? presupuestoRestauracionPaisajisticaPublica
        {
            get
            {
                return presupuestoRestauracionPaisajistica * ((100-invPrivadaRestauracionPaisajisticaPC) / 100);
            }
        }



        [DefaultValue(null)]
        public double? presupuestoOtrosPrivada
        {
            get
            {
                return presupuestoOtros * (invPrivadaOtrosPC / 100);
            }
        }

        [DefaultValue(null)]
        public double? presupuestoOtrosPublica
        {
            get
            {
                return presupuestoOtros * ((100-invPrivadaOtrosPC) / 100);
            }
        }




        [DefaultValue(null)]
        public double? presupuestoPCAPrivada
        {
            get
            {
                return (presupuestoBaseLicitacionPrivada +
                        presupuestoExpropiacionesPrivada +
                        presupuestoConservacionPatrimonioPrivada +
                        presupuestoControlCalidadAdicionalPrivada +
                        presupuestoRestauracionPaisajisticaPrivada +
                        presupuestoOtrosPrivada);
            }       
        }


        [DefaultValue(null)]
        public double? presupuestoPCAPublico
        {
            get
            {
                return (presupuestoBaseLicitacionPublica +
                        presupuestoExpropiacionesPublica +
                        presupuestoConservacionPatrimonioPublica +
                        presupuestoControlCalidadAdicionalPublica +
                        presupuestoRestauracionPaisajisticaPublica +
                        presupuestoOtrosPublica);
            }
        }



        /// <summary>
        /// CALCULO PRESUPUESTO CONSTRUCCION RENTABILIDAD PUBLICA
        /// </summary>
        public double? presupuestoConstruccionRentabilidadPublico
        {
            get
            {
                double miPresupuestoConstruccionConIva = this.presupuestoPCAPublico.Value - this.presupuestoExpropiacionesPublica.Value;

                double miPresupuestoConstruccionSinIva = miPresupuestoConstruccionConIva / (1 + (this.IVA_PC.Value / 100));

                return (miPresupuestoConstruccionSinIva + this.presupuestoExpropiacionesPublica);

            }
        }


        /// <summary>
        /// CALCULO PRESUPUESTO CONSTRUCCION RENTABILIDAD PRIVADA
        /// </summary>
        public double? presupuestoConstruccionRentabilidadPrivada
        {
            get
            {
                double miPresupuestoConstruccionConIva = this.presupuestoPCAPrivada.Value - this.presupuestoExpropiacionesPrivada.Value;

                double miPresupuestoConstruccionSinIva = miPresupuestoConstruccionConIva / (1 + (this.IVA_PC.Value / 100));

                return (miPresupuestoConstruccionSinIva + this.presupuestoExpropiacionesPrivada);

            }
        }

        #endregion



     


        #region "Metodos Privados"

     private void postConstructor()
        {

               if (oSingletonDsApp.getInstance.isInversionPrivada)
                {
                    invPrivadaPresupuestoBaseLicitacionPC = oSingletonDsApp.getInstance.partePrivadaPresupuestoLicitacionPC;
                    invPrivadaExpropiacionPC = oSingletonDsApp.getInstance.partePrivadaExpropiacionesPC;
                    invPrivadaConservacionPatrimonioPC = oSingletonDsApp.getInstance.partePrivadaConservacionPatrimonioPC;
                    invPrivadaControlCalidadPC = oSingletonDsApp.getInstance.partePrivadaControlCalidadPC;
                    invPrivadaRestauracionPaisajisticaPC = oSingletonDsApp.getInstance.partePrivadaRestauracionPaisajisticaPC;
                    invPrivadaOtrosPC = oSingletonDsApp.getInstance.partePrivadaOtrosPC;
                }
                else
                {
                    invPrivadaPresupuestoBaseLicitacionPC = 0;
                    invPrivadaExpropiacionPC = 0;
                    invPrivadaConservacionPatrimonioPC = 0;
                    invPrivadaControlCalidadPC = 0;
                    invPrivadaRestauracionPaisajisticaPC = 0;
                    invPrivadaOtrosPC = 0;
                }
                

        }



        #endregion


        #region "Metodos"


        public override void  write(string iFileConExtension, string iMonedaSimbolo)
        {
            //Header
            List<oRptPresupuestoHeader> miLstHeader = new List<oRptPresupuestoHeader>();
            miLstHeader.Add(new oRptPresupuestoHeader(oDalTbSolucion.getSolucion(solId).nombre,DateTime.Now.ToString()));
            

            //Items
            List<oRptPresupuestoItems> miLstItems = new List<oRptPresupuestoItems>();
            miLstItems = mLstMedicionesExpropiaciones.ConvertAll(p=> new oRptPresupuestoItems(p.descripcion, p.material,"",p.medicion.roundOff(2),p.ud,p.precio,p.coste.roundOffOne()));

            //Footer
            List<oRptPresupuestoFooter> miLstFooter = new List<oRptPresupuestoFooter>();
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreBaseLicitacion + strFrmInformes.uiPrePartePublica, null, presupuestoBaseLicitacionPublica.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreBaseLicitacion + strFrmInformes.uiPrePartePrivada, null, presupuestoBaseLicitacionPrivada.roundOffOne(), iMonedaSimbolo));

            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreExpropiaciones + strFrmInformes.uiPrePartePublica, null, presupuestoExpropiacionesPublica.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreExpropiaciones + strFrmInformes.uiPrePartePrivada, null, presupuestoExpropiacionesPrivada.Value.roundOffOne(), iMonedaSimbolo));

            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreConservacionPatrimonio + strFrmInformes.uiPrePartePublica, conservacionPatrimonioPC, presupuestoConservacionPatrimonioPublica.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreConservacionPatrimonio + strFrmInformes.uiPrePartePrivada, conservacionPatrimonioPC, presupuestoConservacionPatrimonioPrivada.Value.roundOffOne(), iMonedaSimbolo));

            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreCostesAdicControlCalidad + strFrmInformes.uiPrePartePublica, controlCalidadAdicionalPC, presupuestoControlCalidadAdicionalPublica.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreCostesAdicControlCalidad + strFrmInformes.uiPrePartePrivada, controlCalidadAdicionalPC, presupuestoControlCalidadAdicionalPrivada.Value.roundOffOne(), iMonedaSimbolo));

            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreRestauracionPaisajistica + strFrmInformes.uiPrePartePublica, restauracionPaisajisticaPC, presupuestoRestauracionPaisajisticaPublica.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreRestauracionPaisajistica + strFrmInformes.uiPrePartePrivada, restauracionPaisajisticaPC, presupuestoRestauracionPaisajisticaPrivada.Value.roundOffOne(), iMonedaSimbolo));

            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreOtros + strFrmInformes.uiPrePartePublica, otrosPC, presupuestoOtrosPublica.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreOtros + strFrmInformes.uiPrePartePrivada, otrosPC, presupuestoOtrosPrivada.Value.roundOffOne(), iMonedaSimbolo));

            miLstFooter.Add(new oRptPresupuestoFooter(string.Empty, null, null, string.Empty));

            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreInvPrivada + strFrmInformes.uiPrePartePublica, null, presupuestoPCAPublico.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreInvPrivada + strFrmInformes.uiPrePartePrivada, null, presupuestoPCAPrivada.Value.roundOffOne(), iMonedaSimbolo));

            oExcelInforme.WriteCsv<oRptPresupuestoHeader, oRptPresupuestoItems, oRptPresupuestoFooter>(miLstHeader, miLstItems, miLstFooter, iFileConExtension);

        }


        #endregion




    }
}
