using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.presupuesto
{

    

    using engNet;
    using engNet.Extension.Double;

    using tadLayLogica.informes;
    using tadLayData;

    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.logica.medicion;
    using tadLayLan.Tdi;
    
    /// <summary>
    /// Presupuesto Base Licitacion (PBL o PEM)
    /// </summary>
    public class oPresupuestoPBL
    {

        private Guid mIdSol = Guid.Empty;


        private dsApp.tbProyectoRow mRowApp;
        private dsApp.tbPresupuestoRow mRowPresupuesto;
        private dsApp.tbSolucionRow mRowSol;

        private List<oMedItemModel> mLstMedicionPartidasCAD;

        private List<oMedItemModel> mLstMediconesMacroPrecios;

        private List<oMedItemModel> mLstMedicionesPresupuesto;

   
        #region "Constructor"

        public oPresupuestoPBL(Guid iIdSolucion)
        {
           this.mIdSol = iIdSolucion;
           postConstructor();  
        }

        #endregion
        #region "Propiedades"
        public double gastosGeneralesPC { get; private set; }
        public double beneficioIndustrialPC { get; private set; }
        public double controlCalidadPC { get; private set; }
        public double ivaPC { get; private set; }
        public double presupuestoTotalPartidasSinSeguridadySalud
        {
            get
            {

                if (mLstMedicionesPresupuesto == null || mLstMedicionesPresupuesto.Count == 0)
                {
                    throw new NotImplementedException("Listado Mediciones Proyecto es Nulo");
                }
                else
                {
                    //Debo de Sumar todas las partidas menos las de Seguridad y Salud

                    var miQuery = from p in mLstMedicionesPresupuesto
                                  where p.code != eNodo.SEGURIDADYSALUD
                                  select p;

                    return miQuery.Sum(p => p.coste);

                   
                }

            }

        }
        /// <summary>
        /// Presupuesto Ejecución Material
        /// </summary>
        public double presupuestoTotalPartidas
        {
            get
            {

                if (mLstMedicionesPresupuesto == null || mLstMedicionesPresupuesto.Count == 0)
                {
                    throw new NotImplementedException("Listado Mediciones Proyecto es Nulo");
                }
                else
                {
                    //Debo de Sumar todas las partidas menos las de Seguridad y Salud
                    var miQuery = from p in mLstMedicionesPresupuesto
                                  select p;

                    return miQuery.Sum(p => p.coste);
                }

            }

        }
        public double presupuestoGastosGenerales
        {
            get
            {
                return presupuestoTotalPartidas * (gastosGeneralesPC / 100);
            }

        }
        public double presupuestoBeneficioIndustrial
        {
            get
            {
                return presupuestoTotalPartidas * (beneficioIndustrialPC / 100);
            }

        }
        public double presupuestoControlCalidad
        {
            get
            {
                return presupuestoTotalPartidas * (controlCalidadPC / 100);
            }

        }
        public double presupuestoPBLbaseImponible
        {

            get
            {
                return presupuestoTotalPartidas +
                       presupuestoGastosGenerales +
                       presupuestoBeneficioIndustrial +
                       presupuestoControlCalidad;
            }

        }
        public double presupuestoIva
        {
            get
            {
                return presupuestoPBLbaseImponible * (ivaPC / 100);
            }



        }
        public double presupuestoPBL
        {
            get
            {
                return presupuestoPBLbaseImponible + presupuestoIva;
            }
        }
        #endregion

        #region "Metodos Públicos"
        public void write(string iFileConExtension, string iMonedaSimbolo)
        {

            //Header
            List<oRptPresupuestoHeader> miLstHeader = new List<oRptPresupuestoHeader>();
            miLstHeader.Add(new oRptPresupuestoHeader(oDalTbSolucion.getSolucion(mIdSol).nombre, DateTime.Now.ToString()));

            //Items
            List<oRptPresupuestoItems> miLstItems = new List<oRptPresupuestoItems>();
            miLstItems = mLstMedicionesPresupuesto.ConvertAll(p => new oRptPresupuestoItems(p.descripcion, p.material, p.descripcionMedicon, p.medicion.roundOff(2), p.ud, p.precio, p.coste.roundOffOne()));

            //Footer
            List<oRptPresupuestoFooter> miLstFooter = new List<oRptPresupuestoFooter>();
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPresupuestoEjMaterial, null, presupuestoTotalPartidas.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreGastosGenerales, gastosGeneralesPC, presupuestoGastosGenerales.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreBeneficioIndustrial, beneficioIndustrialPC, presupuestoBeneficioIndustrial.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreControlCalidad, controlCalidadPC, presupuestoControlCalidad.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(string.Empty, null, null, string.Empty));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreBaseImponible, null, presupuestoPBLbaseImponible.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreIVA, ivaPC, presupuestoIva.roundOffOne(), iMonedaSimbolo));
            miLstFooter.Add(new oRptPresupuestoFooter(string.Empty, null, null, string.Empty));
            miLstFooter.Add(new oRptPresupuestoFooter(strFrmInformes.uiPreBaseLicitacion, null, presupuestoPBL.roundOffOne(), iMonedaSimbolo));

            oExcelInforme.WriteCsv<oRptPresupuestoHeader, oRptPresupuestoItems, oRptPresupuestoFooter>(miLstHeader, miLstItems, miLstFooter,iFileConExtension);


        }
        #endregion

        #region "Metodos Privados"
        private void postConstructor()
        {


            mRowApp = oSingletonDsApp.getInstance.proyecto;
            mRowPresupuesto = oDalPresupuesto.getPresupuestoRow();
            mRowSol = oSingletonDsApp.getInstance.getSolucion(mIdSol);

            gastosGeneralesPC = mRowPresupuesto.pblGastosGeneralesPC;
            beneficioIndustrialPC = mRowPresupuesto.pblBeneficioIndustrialPC;
            controlCalidadPC = mRowPresupuesto.pblControlCalidadPC;
            ivaPC = mRowPresupuesto.IvaPC;


            //Obtengo las Partidas Medidas en Cad
            mLstMedicionPartidasCAD = oDalAppMedicones.getMedicionesCad(mIdSol);

            //Obtengo las Mediciones de los MacroPrecios
            mLstMediconesMacroPrecios = oDalAppMedicones.getMedicionesMacroPrecios(mIdSol);

            if (mLstMedicionPartidasCAD.Count == 0 && mLstMediconesMacroPrecios.Count == 0)
            {
                throw new Exception("x Las Medición de las Partidas de CAD y MacroPrecios son Nulas");
            }
            else if (mLstMedicionPartidasCAD.Count == 0)
            {
                throw new Exception("x Las Medición de las Partidas de CAD son Nulas");
            }
            else if (mLstMediconesMacroPrecios.Count == 0)
            {
                throw new Exception("x Las Medición de las Partidas de Macro Precios son Nulas");
            }
            else
            {
                //Obtengo las Medicones TOTALES para el PEM
                mLstMedicionesPresupuesto = new List<oMedItemModel>();

                mLstMedicionesPresupuesto.AddRange(mLstMedicionPartidasCAD);
                mLstMedicionesPresupuesto.AddRange(mLstMediconesMacroPrecios);

                //Obtengo la partida de Seguridad y Salud
                setUpPartidaSeguridadSalud();

                //Ordeno las Mediciones
                mLstMedicionesPresupuesto.ToList().OrderBy(p => p.orden);
            }
        
        }
        private void setUpPartidaSeguridadSalud()
        {

            var miQuery = from p in mLstMedicionesPresupuesto
                          where p.code == eNodo.SEGURIDADYSALUD
                          select p;

            int miCount = miQuery.ToList().Count;


            if (miCount==0)
            {
                throw new Exception("x La Partida de Seguridad y Salud es Nula");
            }
            else if (miCount == 1)
            {
                miQuery.First().medicion = presupuestoTotalPartidasSinSeguridadySalud;
            }
            else
            {
                throw new Exception("x La Partida de Seguridad y Salud es Mayor que 1");
            }
        }
        #endregion






    }
}
