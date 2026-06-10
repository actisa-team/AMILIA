using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.presupuesto
{

    using tadLayData;

    using System.ComponentModel;

    using engNet.Extension.Double;

    using tadLayLogica.informes;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.logica.medicion;
    using tadLayLan.Tdi;
    using engNet.CustomAtributos;
    
    /// <summary>
    /// PRESUPUESTO CONOCIMIENTO ADMINISTRACION PUBLICO (PCA) 
    /// </summary>
    public class oPresupuestoPcaPublico :IDisposable
    {

        protected Guid solId = Guid.Empty;
        protected List<oMedItemModel> mLstMedicionesExpropiaciones = null;
      

        public oPresupuestoPcaPublico(Guid iIdSol)
        {
            this.solId = iIdSol;
            postConstructor();
        }


       

        #region "Propiedades"

       

        [DefaultValue(null)]
        public double? conservacionPatrimonioPC { get; private set; }
        [DefaultValue(null)]
        public double? controlCalidadAdicionalPC { get; private set; }
        [DefaultValue(null)]
        public double? restauracionPaisajisticaPC { get; private set; }
        [DefaultValue(null)]
        public double? otrosPC { get; private set; }

        [DefaultValue(null)]
        public double? presupuestoBaseLicitacionConIva { get; private set; }
        [DefaultValue(null)]
        public double? presupuestoBaseLicitacionSinIva { get; private set; }

        [DefaultValue(null)]
        public double? presupuestoExpropiaciones { get; private set; }

        [LocalizedDisplayName("uiPreConservacionPatriRenta", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoConservacionPatrimonioRentabilidad
        {
            get
            {
                return this.presupuestoConservacionPatrimonio / (1 + (this.IVA_PC / 100));
            }
        }
        [LocalizedDisplayName("uiPreConservacionPatrimonio", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoConservacionPatrimonio
        {
            get
            {
                return (presupuestoEjecucionMaterial * (conservacionPatrimonioPC / 100));
            }
        }

        [LocalizedDisplayName("uiCostesAdcControlCalRentabilidad", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoControlCalidadAdicionalRentabilidad
        {
            get
            {
                return this.presupuestoControlCalidadAdicional / (1 + (this.IVA_PC / 100));
            }
        }

        [LocalizedDisplayName("uiPreCostesAdicControlCalidad", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoControlCalidadAdicional
        {
            get
            {
                return presupuestoEjecucionMaterial * (controlCalidadAdicionalPC / 100);
            }
        }

        [LocalizedDisplayName("uiResPaisajRentabilidad", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoRestauracionPaisajisticaRentabilidad
        {
            get
            {
                return this.presupuestoRestauracionPaisajistica / (1 + (this.IVA_PC / 100));
            }
        }

        [LocalizedDisplayName("uiPreRestauracionPaisajistica", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoRestauracionPaisajistica
        {
            get
            {
                return presupuestoEjecucionMaterial * (restauracionPaisajisticaPC / 100);
            }
        }

        [LocalizedDisplayName("uiOtrosRentabilidad", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoOtrosRentabilidad
        {
            get
            {
                return this.presupuestoOtros / (1 + (this.IVA_PC / 100));
            }
        }


        [LocalizedDisplayName("uiPreOtros", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoOtros
        {
            get
            {
                return presupuestoEjecucionMaterial * (otrosPC / 100);
            }
        }


        [LocalizedDisplayName("uiPreConAdmSinIVA", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoPcaSinIVA
        {
            get
            {
                return (presupuestoBaseLicitacionSinIva +
                        presupuestoExpropiaciones +
                        presupuestoConservacionPatrimonioRentabilidad +
                        presupuestoControlCalidadAdicionalRentabilidad +
                        presupuestoRestauracionPaisajisticaRentabilidad +
                        presupuestoOtrosRentabilidad);
            }
        }



        [LocalizedDisplayName("uiPreConAdConIVA", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoPcaConIva
        {
            get
            {
                return (presupuestoBaseLicitacionConIva +
                        presupuestoExpropiaciones +
                        presupuestoConservacionPatrimonio +
                        presupuestoControlCalidadAdicional +
                        presupuestoRestauracionPaisajistica +
                        presupuestoOtros);
            }       
        }


        [LocalizedDisplayName("uiPresupuestoEjMaterial", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? presupuestoEjecucionMaterial {get;private set;}

        [LocalizedDisplayName("uiIVAporCien", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? IVA_PC { get; private set; }
     

        #endregion
        #region "Metodos Privados"

        private void postConstructor()
        {

            dsApp.tbPresupuestoRow miRowPresupuesto = oDalPresupuesto.getPresupuestoRow();

            conservacionPatrimonioPC = miRowPresupuesto.pcaConservacionPatrimonioPC;
            controlCalidadAdicionalPC = miRowPresupuesto.pcaControlCalidadPC;
            restauracionPaisajisticaPC = miRowPresupuesto.pcaRestauracionPaisajisticaPC;
            otrosPC = miRowPresupuesto.pcaOtroPC;
           
            //Obtengo el PEM
            oPresupuestoPBL miPresupuestoPBL = new oPresupuestoPBL(solId);
            presupuestoBaseLicitacionConIva = miPresupuestoPBL.presupuestoPBL;
            presupuestoBaseLicitacionSinIva = miPresupuestoPBL.presupuestoPBLbaseImponible;
            presupuestoEjecucionMaterial = miPresupuestoPBL.presupuestoTotalPartidas;
            this.IVA_PC = miPresupuestoPBL.ivaPC;

            //Mediciones de las Expropiaciones
            mLstMedicionesExpropiaciones = oDalAppExpropiacion.getMediciones(solId);

            //Obtengo la Suma de las Expropiaciones
            presupuestoExpropiaciones = mLstMedicionesExpropiaciones.Sum(p => p.coste);
        }



        #endregion
        #region "Metodos Publicos"


        public virtual void write(string iFileConExtension, string iMonedaSimbolo)
        {
            //Header
            List<oRptPresupuestoHeader> miLstHeader = new List<oRptPresupuestoHeader>();
            miLstHeader.Add(new oRptPresupuestoHeader(oDalTbSolucion.getSolucion(solId).nombre,DateTime.Now.ToString()));
            

            //Items
            List<oRptPresupuestoItems> miLstItems = new List<oRptPresupuestoItems>();
            miLstItems = mLstMedicionesExpropiaciones.ConvertAll(p=> new oRptPresupuestoItems(p.descripcion, p.material,"",p.medicion.roundOff(2),p.ud,p.precio,p.coste.roundOffOne()));

            //Footer
            List<oRptPresupuestoFooter> miLstFooter = new List<oRptPresupuestoFooter>();
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreBaseLicitacion, null, presupuestoBaseLicitacionConIva.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreExpropiaciones, null, presupuestoExpropiaciones.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreConservacionPatrimonio, conservacionPatrimonioPC, presupuestoConservacionPatrimonio.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreCostesAdicControlCalidad, controlCalidadAdicionalPC, presupuestoControlCalidadAdicional.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreRestauracionPaisajistica, restauracionPaisajisticaPC, presupuestoRestauracionPaisajistica.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreOtros, otrosPC, presupuestoOtros.Value.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(string.Empty, null, null, string.Empty));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreConoAdmin, null, presupuestoPcaConIva.Value.roundOffOne(), iMonedaSimbolo));

            oExcelInforme.WriteCsv<oRptPresupuestoHeader, oRptPresupuestoItems, oRptPresupuestoFooter>(miLstHeader, miLstItems, miLstFooter,iFileConExtension);
        }


        #endregion



        #region "Interface"

        public void Dispose()
        {
            solId = Guid.Empty;
            mLstMedicionesExpropiaciones = null;
        }

        #endregion


    }
}
