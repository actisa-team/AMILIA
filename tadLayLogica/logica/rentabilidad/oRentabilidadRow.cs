using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
    using engNet.CustomAtributos;
    using System.ComponentModel;
    using tadLayLan.Tdi;
     
    public abstract class oRentabilidadRowModel
    {

        [BindingInfo(SortIndex = 1)]
        [LocalizedDisplayName("uiAño", typeof(strFrmInformes))]
       public int year { get; set; }

        [BindingInfo(SortIndex = 3)]
        [LocalizedDisplayName("uiFase", typeof(strFrmInformes))]
       public string actividad { get; protected set; }

        [BindingInfo(SortIndex = 4)]
        [LocalizedDisplayName("uiAñoActividad", typeof(strFrmInformes))]
       public int yearActividad { get; protected set; }

        [BindingInfo(SortIndex = 5)]
        [LocalizedDisplayName("uiTasaRevPrecConst", typeof(strFrmInformes))]
       public double tasaPreciosConstruccion { get; set; }

        [BindingInfo(SortIndex = 6)]
        [LocalizedDisplayName("uiTasaRevIPC", typeof(strFrmInformes))]
       public double tasaIpc { get; set; }

        [BindingInfo(SortIndex = 7)]
        [LocalizedDisplayName("uiTasaRevSub", typeof(strFrmInformes))]
       public double tasaSubvencion { get; set; }

        [BindingInfo(SortIndex = 8)]
        [LocalizedDisplayName("uiTasaActualizacion", typeof(strFrmInformes))]
       public double tasaActualizacion { get; set; }



        [BindingInfo(SortIndex = 1001)]
        [LocalizedDisplayName("uiCostesTotIPC", typeof(strFrmInformes))]
       public  double costesTotalIPC
       { 
           get
           {
               return getCalculoCosteBeneficioIPC(false);
           }
       }

        [BindingInfo(SortIndex = 1002)]
        [LocalizedDisplayName("uiIngTotIPC", typeof(strFrmInformes))]
       public double beneficiosTotalIPC
       {
           get
           {
               return getCalculoCosteBeneficioIPC(true);
           }
       }

        [BindingInfo(SortIndex = 1003)]
        [LocalizedDisplayName("uiBalanceTotIPC", typeof(strFrmInformes))]
       public double balanceTotalIPC
       {
           get
           {
               return costesTotalIPC +
                      beneficiosTotalIPC;
           }
       }

        [BindingInfo(SortIndex = 1005)]
        [LocalizedDisplayName("uiRentCosTotActual", typeof(strFrmInformes))]
       public double costesTotalActualizados
       { 
           get
           {
               return costesTotalIPC / tasaActualizacion;
           }
       }

       [BindingInfo(SortIndex = 1006)]
       [LocalizedDisplayName("uiRentIngTotActual", typeof(strFrmInformes))]
       public double beneficiosTotalActualizados
       {
           get
           {
               return beneficiosTotalIPC / tasaActualizacion;
           }
       }

       [BindingInfo(SortIndex = 1007)]
       [LocalizedDisplayName("uiBalanceTotAct", typeof(strFrmInformes))]
       public double balanceTotalActualizado
       {
           get
           {
               return balanceTotalIPC / tasaActualizacion;
           }
       }

       /// <summary>
       /// TRUE--> Devuelvo el Beneficio (Valores Postivos)
       /// FALSE --> Devuelvo el Coste (Valores Negativos)
       /// </summary>
       /// <param name="iGetCoste"></param>
       /// <returns></returns>
       protected abstract double getCalculoCosteBeneficioIPC(bool iGetBeneficio);
           

    }
    public class oRentabilidadRowSocial : oRentabilidadRowModel
    {


        [BindingInfo(SortIndex = 20)]
        [LocalizedDisplayName("uiTraOpCero", typeof(strFrmInformes))]
        public int traficoAnual0 { get; private set; }

        [BindingInfo(SortIndex = 21)]
        [LocalizedDisplayName("uiTraConexionAct", typeof(strFrmInformes))]
        public int traficoAnual10 { get; private set; }

        [BindingInfo(SortIndex = 22)]
        [LocalizedDisplayName("uiTraConexionNue", typeof(strFrmInformes))]
        public int traficoAnual11 { get; private set; }

        [BindingInfo(SortIndex = 23)]
        [LocalizedDisplayName("uiCosteConstrEstatal", typeof(strFrmInformes))]
        public double costeContruccion11 { get; private set; }

        [BindingInfo(SortIndex = 24)]
        [LocalizedDisplayName("uiCosteFuncOpCero", typeof(strFrmInformes))]
        public double costeFun0 { get; private set; }

        [BindingInfo(SortIndex = 25)]
        [LocalizedDisplayName("uiCosteFuncConexionAct", typeof(strFrmInformes))]
        public double costeFun10 { get; private set; }

        [BindingInfo(SortIndex = 26)]
        [LocalizedDisplayName("uiCosteFuncConexionNue", typeof(strFrmInformes))]
        public double costeFun11 { get; private set; }

        [BindingInfo(SortIndex = 27)]
        [LocalizedDisplayName("uiCosteTiempoOpCero", typeof(strFrmInformes))]
        public double costeTiempo0 { get; private set; }

        [BindingInfo(SortIndex = 28)]
        [LocalizedDisplayName("uiCosteTiempoConexionAct", typeof(strFrmInformes))]
        public double costeTiempo10 { get; private set; }

        [BindingInfo(SortIndex = 29)]
        [LocalizedDisplayName("uiCosteTiempoConexionNue", typeof(strFrmInformes))]
        public double costeTiempo11 { get; private set; }

        [BindingInfo(SortIndex = 30)]
        [LocalizedDisplayName("uiCosteAccOpCero", typeof(strFrmInformes))]
        public double costeAccidentes0 { get; private set; }

        [BindingInfo(SortIndex = 31)]
        [LocalizedDisplayName("uiCosteAccConexionAct", typeof(strFrmInformes))]
        public double costeAccidentes10 { get; private set; }

        [BindingInfo(SortIndex = 32)]
        [LocalizedDisplayName("uiCosteAccConexionNue", typeof(strFrmInformes))]
        public double costeAccidentes11 { get; private set; }

        [BindingInfo(SortIndex = 33)]
        [LocalizedDisplayName("uiCosteConsOpCero", typeof(strFrmInformes))]
        public double costeConservacion0 { get; private set; }

        [BindingInfo(SortIndex = 34)]
        [LocalizedDisplayName("uiCosteConsConexionActual", typeof(strFrmInformes))]
        public double costeConservacion10 { get; private set; }

        [BindingInfo(SortIndex = 35)]
        [LocalizedDisplayName("uiCostesConsConexionNue", typeof(strFrmInformes))]
        public double costeConservacion11 { get; private set; }

        [BindingInfo(SortIndex = 36)]
        [LocalizedDisplayName("uiCostesPeaje", typeof(strFrmInformes))]
        public double costePeajes { get; private set; }

        [BindingInfo(SortIndex = 37)]
        [LocalizedDisplayName("uiCostesSubVehi", typeof(strFrmInformes))]
        public double costeSubvencionVehiculo { get; private set; }

        [BindingInfo(SortIndex = 38)]
        [LocalizedDisplayName("uiCostesSubAnual", typeof(strFrmInformes))]
        public double costeSubvencionAnual { get; private set; }

        [BindingInfo(SortIndex = 39)]
        [LocalizedDisplayName("uiCostesEmpleo", typeof(strFrmInformes))]
        public double costeEmpleo { get; private set; }


        #region "Constructor"




        public oRentabilidadRowSocial(  
                                        int iYear,
                                        string iActividad,
                                        int iYearActividad,
                                        double iTasaActualizacion,
                                        double iTasaPreciosConstruccion,
                                        double iTasaIPC,
                                        double iTasaSubvencion,

                                        int iTraficoAnual0,
                                        int iTraficoAnual10,
                                        int iTraficoAnual11,

                                        double iCosteConstruccion,

                                        double iCosteFun0,
                                        double iCosteFun10,
                                        double iCosteFun11,

                                        double iCosteTiempo0,
                                        double iCosteTiempo10,
                                        double iCosteTiempo11,

                                        double iCosteAccidentes0,
                                        double iCosteAccidentes10,
                                        double iCosteAccidentes11,

                                        double iCosteConservacion0,
                                        double iCosteConservacion10,
                                        double iCosteConservacion11,

                                        double iCostePeaje,
                                        double iCosteSubvencionVehiculo,
                                        double iCosteSubvencionAnual,
                                        double iCosteEmpleo)
        {

            year = iYear;
            actividad = iActividad;
            yearActividad = iYearActividad;
           

            traficoAnual0 = iTraficoAnual0;
            traficoAnual10 = iTraficoAnual10;
            traficoAnual11 = iTraficoAnual11;

            tasaActualizacion = iTasaActualizacion;
            tasaPreciosConstruccion = iTasaPreciosConstruccion;
            tasaIpc = iTasaIPC;
            tasaSubvencion = iTasaSubvencion;

            costeContruccion11 = iCosteConstruccion;

            costeFun0 = iCosteFun0;
            costeFun10 = iCosteFun10;
            costeFun11 = iCosteFun11;

            costeTiempo0 = iCosteTiempo0;
            costeTiempo10 = iCosteTiempo10;
            costeTiempo11 = iCosteTiempo11;

            costeAccidentes0 = iCosteAccidentes0;
            costeAccidentes10 = iCosteAccidentes10;
            costeAccidentes11 = iCosteAccidentes11;

            costeConservacion0 = iCosteConservacion0;
            costeConservacion10 = iCosteConservacion10;
            costeConservacion11 = iCosteConservacion11;

            costePeajes = iCostePeaje;
            costeSubvencionVehiculo = iCosteSubvencionVehiculo;
            costeSubvencionAnual = iCosteSubvencionAnual;
            costeEmpleo = iCosteEmpleo;

        }




        #endregion





        #region "PropiedadesCalculadas"

        [BindingInfo(SortIndex = 100)]
        [LocalizedDisplayName("uiConstruccEstatalConRevPrecio", typeof(strFrmInformes))]
        public double balanceCosteConstruccionIPC
        {
            get
            {
                return -costeContruccion11 * tasaPreciosConstruccion;
            }
        }

        [BindingInfo(SortIndex = 101)]
        [LocalizedDisplayName("uiPeajeIPC", typeof(strFrmInformes))]
        public double balanceCostesPeajeIPC
        {
            get
            {
                return -costePeajes * tasaIpc;
            }
        }

        [BindingInfo(SortIndex = 102)]
        [LocalizedDisplayName("uiSubIPC", typeof(strFrmInformes))]
        public double balanceCostesSubvencionIPC
        {
            get
            {
                return -(costeSubvencionVehiculo + costeSubvencionAnual) * tasaSubvencion;
            }
        }

        [BindingInfo(SortIndex = 103)]
        [LocalizedDisplayName("uiConserIPC", typeof(strFrmInformes))]
        public double balanceCostesConservacionIPC
        {
            get
            {
                return (costeConservacion0 - costeConservacion10 - costeConservacion11) * tasaIpc;
            }
        }

        [BindingInfo(SortIndex = 104)]
        [LocalizedDisplayName("uiFuncTiempoIPC", typeof(strFrmInformes))]
        public double balanceCosteFuncionamientoTiempoIPC
        {
            get
            {
                return ((costeFun0 - costeFun10 - costeFun11) +
                       (costeTiempo0 - costeTiempo10 - costeTiempo11)) * tasaIpc;
            }
        }

        [BindingInfo(SortIndex = 105)]
        [LocalizedDisplayName("uiAccidentesIPC", typeof(strFrmInformes))]
        public double balanceCosteAccidentesIPC
        {
            get
            {
                return (costeAccidentes0 - costeAccidentes10 - costeAccidentes11) * tasaIpc;
            }
        }

        [BindingInfo(SortIndex = 106)]
        [LocalizedDisplayName("uiEmpleoIPC", typeof(strFrmInformes))]
        public double balanceCostesEmpleoIPC
        {
            get
            {
                return (costeEmpleo) * tasaIpc;
            }
        }

        #endregion

        #region "Metodos Abstractos"

        /// <summary>
        /// TRUE--> Devuelvo el Beneficio (Valores Postivos)
        /// FALSE --> Devuelvo el Coste (Valores Negativos)
        /// </summary>
        /// <param name="iGetCoste"></param>
        /// <returns></returns>
        protected override double getCalculoCosteBeneficioIPC(bool iGetBeneficio)
        {

            List<double> miLstPartidas = new List<double>();

            miLstPartidas.Add(balanceCosteConstruccionIPC);
            miLstPartidas.Add(balanceCostesPeajeIPC);
            miLstPartidas.Add(balanceCostesSubvencionIPC);
            miLstPartidas.Add(balanceCostesConservacionIPC);
            miLstPartidas.Add(balanceCosteFuncionamientoTiempoIPC);
            miLstPartidas.Add(balanceCosteAccidentesIPC);
            miLstPartidas.Add(balanceCostesEmpleoIPC);


            if (iGetBeneficio)
            {
                return (from p in miLstPartidas where p > 0 select p).Sum();
            }
            else
            {
                return (from p in miLstPartidas where p < 0 select p).Sum();
            }
                
        }

        #endregion


    }
    public class oRentabilidadRowPrivada : oRentabilidadRowModel
    {


        [BindingInfo(SortIndex = 20)]
        [LocalizedDisplayName("uiTraConexionNue", typeof(strFrmInformes))]
        public int traficoAnual { get; private set; }

        [BindingInfo(SortIndex = 21)]
        [LocalizedDisplayName("uiCosteConstPriv", typeof(strFrmInformes))]
        public double costeContruccionPrivada { get; private set; }

        [BindingInfo(SortIndex = 22)]
        [LocalizedDisplayName("uiCostesConsyMant", typeof(strFrmInformes))]
        public double costeConservacionMantenimiento { get; private set; }

        [BindingInfo(SortIndex = 23)]
        [LocalizedDisplayName("uiCostesExplotacion", typeof(strFrmInformes))]
        public double costeExplotacion { get; private set; }

        [BindingInfo(SortIndex = 24)]
        [LocalizedDisplayName("uiCostesSeguros", typeof(strFrmInformes))]
        public double costeSeguros { get; private set; }

        [BindingInfo(SortIndex = 38)]
        [LocalizedDisplayName("uiIngresosSubAnual", typeof(strFrmInformes))]
        public double ingresosSubvencionAnual { get; private set; }

        [BindingInfo(SortIndex = 39)]
        [LocalizedDisplayName("uiIngresosSubPeaje", typeof(strFrmInformes))]
        public double ingresosSubvencionPeaje { get; private set; }


        [BindingInfo(SortIndex = 40)]
        [LocalizedDisplayName("uiIngresosPeaje", typeof(strFrmInformes))]
        public double ingresosPeaje { get; private set; }


        #region "Constructor"




        public oRentabilidadRowPrivada(
                                        int iYear,
                                        string iActividad,
                                        int iYearActividad,

                                        double iTasaActualizacion,
                                        double iTasaPreciosConstruccion,
                                        double iTasaIPC,
                                        double iTasaSubvencion,

                                        int iTraficoAnual11,

                                        double iCosteConstruccionPrivada,
                                        double iCosteConservacionMantenimiento,
                                        double iCosteExplotacion,
                                        double iCosteSeguros,

                                        double iIngresosSubvencionAnual,
                                        double iIngresosPeajeSubvencion,
                                        double iIngresosPeaje)

        {

            year = iYear;
            actividad = iActividad;
            yearActividad = iYearActividad;

            traficoAnual = iTraficoAnual11;

            tasaActualizacion = iTasaActualizacion;
            tasaPreciosConstruccion = iTasaPreciosConstruccion;
            tasaIpc = iTasaIPC;
            tasaSubvencion = iTasaSubvencion;

            costeContruccionPrivada = -iCosteConstruccionPrivada;
            costeConservacionMantenimiento = -iCosteConservacionMantenimiento;
            costeExplotacion = -iCosteExplotacion;
            costeSeguros = -iCosteSeguros;

            ingresosSubvencionAnual = iIngresosSubvencionAnual;
            ingresosSubvencionPeaje = iIngresosPeajeSubvencion;
            ingresosPeaje = iIngresosPeaje;

        }




        #endregion





        #region "PropiedadesCalculadas"

        [BindingInfo(SortIndex = 100)]
        [LocalizedDisplayName("uiConstPrivadaRevPrecios", typeof(strFrmInformes))]
        public double costeConstruccionPrivadaIPC
        {
            get
            {
                return costeContruccionPrivada * tasaPreciosConstruccion;
            }
        }


        [BindingInfo(SortIndex = 101)]
        [LocalizedDisplayName("uiConsMantIPC", typeof(strFrmInformes))]
        public double costeConservacioMantenimientoIPC
        {
            get
            {
                return costeConservacionMantenimiento * tasaIpc;
            }
        }

        [BindingInfo(SortIndex = 102)]
        [LocalizedDisplayName("uiExploICP", typeof(strFrmInformes))]
        public double costeExplotacionIPC
        {
            get
            {
                return costeExplotacion* tasaIpc;
            }
        }

        [BindingInfo(SortIndex = 103)]
        [LocalizedDisplayName("uiSegIPC", typeof(strFrmInformes))]
        public double costeSegurosIPC
        {
            get
            {
                return costeSeguros * tasaIpc;
            }
        }


        [BindingInfo(SortIndex = 104)]
        [LocalizedDisplayName("uiSubAnRevTasaSub", typeof(strFrmInformes))]
        public double ingresosSubvencionAnualIPC
        {
            get
            {
                return ingresosSubvencionAnual * tasaSubvencion;
            }
        }

        [BindingInfo(SortIndex = 105)]
        [LocalizedDisplayName("uiSubAnRevTasaSub", typeof(strFrmInformes))]
        public double ingresosSubvencionPeajeIPC
        {
            get
            {
                return ingresosSubvencionPeaje * tasaSubvencion;
            }
        }

        [BindingInfo(SortIndex = 106)]
        [LocalizedDisplayName("uiPeajeIPC", typeof(strFrmInformes))]
        public double ingresosPeajeIPC
        {
            get
            {
                return ingresosPeaje * tasaIpc;
            }
        }


        #endregion

        #region "Metodos Abstractos"

        /// <summary>
        /// TRUE--> Devuelvo el Beneficio (Valores Postivos)
        /// FALSE --> Devuelvo el Coste (Valores Negativos)
        /// </summary>
        /// <param name="iGetCoste"></param>
        /// <returns></returns>
        protected override double getCalculoCosteBeneficioIPC(bool iGetBeneficio)
        {

            List<double> miLstPartidas = new List<double>();

            miLstPartidas.Add(costeConstruccionPrivadaIPC);
            miLstPartidas.Add(costeConservacioMantenimientoIPC);
            miLstPartidas.Add(costeExplotacionIPC);
            miLstPartidas.Add(costeSegurosIPC);
            miLstPartidas.Add(ingresosSubvencionAnualIPC);
            miLstPartidas.Add(ingresosSubvencionPeajeIPC);
            miLstPartidas.Add(ingresosPeajeIPC);


            if (iGetBeneficio)
            {
                return (from p in miLstPartidas where p > 0 select p).Sum();
            }
            else
            {
                return (from p in miLstPartidas where p < 0 select p).Sum();
            }

        }

        #endregion


    }
}
