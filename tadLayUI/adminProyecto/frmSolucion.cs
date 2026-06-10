using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLogica.datos.proyecto;
using tayLogicaTijera.EjeBasico;
using tayLogicaTijera.Comandos;
using tayLogicaTijera.data;

namespace tadLayUI.adminProyecto
{

    using engNet.eventos;
    using engNet.Extension.String;

    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayLogica.estudioTipo;
    using tadLayLogica.zonaGis;
    using tadLayLogica.Comandos;
    using tadLayUI;
    using tadLayLogica;
    using tadLayLan;
    using tadLayShare;
    using tadLayLan.Tdi;
    using Newtonsoft.Json;
    using System.IO;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    using tadLayData;
    using engCadNet;
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;
    using tadLayLogica.logica.EjeGlobalModificado;
    using tadLayLogica.logica.LandXml;
    using EjeDeTrazado.puntosDelEje;
    using PerfilLongitudinal;
    using tadLayLogica.EjeLongitudinalTadil;
    using tadLayLogica.logica.Entidades;
    using tadLayLogica.EjeTrazadoTadil;
    using tadLayLogica.Secciones.Calzada;
    using tadLayLogica.ObraLineal;

    public partial class frmSolucion : Form
    {
        #region "Variables Privadas"

        private enum ePageTab
        {
            carretera,
            tijera,
            pendiente,
            valoracion,
            estudioPrevio,
            variablesAvanzadasAbanico,
            variablesAvanzadasCoeficientes,
            ejeglobal,
            ejeglobalactualizado
        }
        private ePageTab? mPageTabPrevio = null;
        private eEstudioTipo? mEstudioTipo = null;

        #endregion

        #region "Singleton"

        private static frmSolucion mInstance = null;
        private frmSolucion(eEstudioTipo iEstudioTipo)
        {
            InitializeComponent();

            mEstudioTipo = iEstudioTipo;

            postConstructor();
        }

        public static frmSolucion getInstance(eEstudioTipo iEstudio)
        {

            if (mInstance == null)
            {
                mInstance = new frmSolucion(iEstudio);

                // tadLayLogica.oTadilCore.evReset += new EventHandler<oEventArgs<bool>>(oTadilCore_evReset);
            }

            return mInstance;
        }


        static void oTadilCore_evReset(object sender, engNet.eventos.oEventArgs<bool> e)
        {
            if (e.Value)
            {
                mInstance = null;

                tadLayLogica.oTadilCore.evReset -= new EventHandler<oEventArgs<bool>>(oTadilCore_evReset);
            }
        }

        #endregion




        #region "CreateObjetos"
        /// <summary>
        /// Datos de la Solución
        /// </summary>
        private oEjeBasicoSolucion getObjEjeBasicoSolucion()
        {

            oEjeBasicoSolucion miEjeBasicoSolucion = new oEjeBasicoSolucion(this.ucSolucionNombre.textbox.Text,
                                                                            this.chkGenerarSolucionEnvolventes.Checked,
                                                                            this.getObjRoadDesign(),
                                                                            this.getObjRoadPendientes(),
                                                                            this.getObjCoeMinAlturasMaximas(),
                                                                             this.getObjValoracion(),
                                                                             this.getObjDataAbanico(),
                                                                             this.getObjEstudioPrevio());


            return miEjeBasicoSolucion;

        }

        /// <summary>
        /// Datos de la Solución
        /// </summary>
        private oEjeBasicoSolucion getObjEjeBasicoSolucionObligadoAvancesCortos()
        {

            oEjeBasicoSolucion miEjeBasicoSolucion = new oEjeBasicoSolucion(this.ucSolucionNombre.textbox.Text,
                                                                            this.chkGenerarSolucionEnvolventes.Checked,
                                                                            this.getObjRoadDesign(),
                                                                            this.getObjRoadPendientes(),
                                                                            this.getObjCoeMinAlturasMaximas(),
                                                                             this.getObjValoracion(),
                                                                             this.getObjDataAbanicoObligadoACortos(),
                                                                             this.getObjEstudioPrevio());


            return miEjeBasicoSolucion;

        }
        /// <summary>
        /// Datos de la Carretera
        /// </summary>
        private oRoadDes getObjRoadDesign()
        {

            oRoadDes miRoadDesign = new oRoadDes(this.ucRoadDatos1.roadGrupo,
                                                  this.ucRoadDatos1.vp,
                                                  this.ucRoadDatos1.rp,
                                                  this.ucRoadDatos1.preferencias,
                                                  this.ucSolucionOptAvanzadas1.isAijConstante,
                                                  this.ucRoadDatos1.allowReducionesPuntualesVelocidad,
                                                  this.ucRoadDatos1.preferenciasKv,
                                                  this.ucRoadDatos1.kvConvexo,
                                                  this.ucRoadDatos1.kvConcavo,
                                                  this.ucRoadDatos1.peraltePC,
                                                  this.ucRoadDatos1.ajMin,
                                                  this.ucRoadDatos1.ajMinSalidaLlegada);


            return miRoadDesign;



        }
        /// <summary>
        /// Datos de las Pendientes
        /// </summary>
        private oRoadPendientes getObjRoadPendientes()
        {
            oRoadPendientes miRoadPendientes = this.ucRoadEstructurasPendientes1.getRoadPendiente();


            miRoadPendientes.coeMinoracionCalzadaPendienteMaxima = this.ucSolucionOptAvanzadasCoeficientes1.coeMinPendienteCarretera;
            miRoadPendientes.coeMinoracionEstructurasPendienteMaxima = this.ucSolucionOptAvanzadasCoeficientes1.coeMinPendienteEstructura;

            return miRoadPendientes;

        }
        /// <summary>
        /// Datos de Valoracion (%DistanciaObjetivo-%PendienteTerreno-%Costes)
        /// </summary>
        private oTramosValoracion getObjValoracion()
        {
            return this.ucSolucionValoracion1.getValoracion();
        }
        /// <summary>
        /// Datos del Abanico
        /// </summary>
        private oAbanicoDesign getObjDataAbanico()
        {
            var abanicoDesign = this.ucSolucionOptAvanzadas1.getDataAbanico(getTipoAbanico());
            abanicoDesign.segundosEntronque = ucEntronqueSegundos.valorDouble;
            abanicoDesign.incluirTramosRectaLimitada = ckLongitudLimitada.Checked;
            abanicoDesign.incluirTramosRectaMaxima = ckLongitudMaxima.Checked;
            abanicoDesign.incluirTramosRectaMinima = ckLongitudMinima.Checked;
            abanicoDesign.incluirTramosSinRecta = ckSinRectas.Checked;
            abanicoDesign.isCurvaGranRadio = ckAplicarGranRadio.Checked;
            abanicoDesign.granRadio = ucGranRadio.valorDouble;
            abanicoDesign.longitudMinimaTramo = double.MinValue;
            if (ckDescartarAlternativasCortas.Checked && ckSinRectas.Checked)
                abanicoDesign.longitudMinimaTramo = ucDistDescartarAltervativa.valorDouble;
            return abanicoDesign;
        }

        /// <summary>
        /// Datos del Abanico obligado a cortos
        /// </summary>
        private oAbanicoDesign getObjDataAbanicoObligadoACortos()
        {
            return this.ucSolucionOptAvanzadas1.getDataAbanico(eAbanicoTipo.corto);
        }
        /// <summary>
        /// CoeficientesMinoracionAlturasMaximas
        /// </summary>
        private oCoeMinoracionAlturasMaximas getObjCoeMinAlturasMaximas()
        {
            return this.ucSolucionOptAvanzadasCoeficientes1.getCoeMinoracionAlturasMaximas();
        }
        /// <summary>
        /// Tipo de Avance Seleccionado
        /// </summary>
        private eAbanicoTipo getTipoAbanico()
        {

            if (this.optAvancesCortos.Checked)
            {
                return eAbanicoTipo.corto;
            }
            else if (this.optAvancesLargos.Checked)
            {
                return eAbanicoTipo.largos;
            }
            else
            {
                throw new Exception("Opción de Tipo de Avance Indeterminado");
            }
        }
        /// <summary>
        /// Devuelvo el Tipo de Estudio
        /// </summary>
        private IEstudio getObjEstudioPrevio()
        {

            IEstudio miEstudioTipo;

            oCoeMinoracionAlturasMaximas miCoeMinAlturasMaximas = getObjCoeMinAlturasMaximas();

            if (mEstudioTipo.Value == eEstudioTipo.ESTPRE)
            {
                miEstudioTipo = this.ucEstudioPrevioDesign1.getEstudioPrevio(miCoeMinAlturasMaximas);
            }
            else if (mEstudioTipo.Value == eEstudioTipo.ESTINF)
            {

                miEstudioTipo = new oEstudioInformativoCarretera(oSingletonProyecto.getInstance.secRoadTipo,
                    oSingletonProyecto.getInstance.seccionCalzadaId, miCoeMinAlturasMaximas, oSingletonProyecto.getInstance.SeccionesVinculadas);


            }
            else
            {
                throw new oExEnumNotImplemented(mEstudioTipo.Value.ToString());
            }

            return miEstudioTipo;
        }


        public string tipoAvance()
        {

            if (this.optAvancesCortos.Checked)
            {
                return this.optAvancesCortos.Text;
            }
            else if (this.optAvancesLargos.Checked)
            {
                return this.optAvancesLargos.Text;
            }
            else
            {
                throw new Exception("Opción de Tipo de Avance Indeterminado");
            }


        }


        #endregion
        #region "MetodosPrivados"


        private void postConstructor()
        {
            #region "Traductor"

            //TabPage
            tabCarretera.Text = strFrmSolucion.uiTabCarretera;
            tabPendientes.Text = strFrmSolucion.uiTabPendientes;
            tabValoraciones.Text = strFrmSolucion.uiTabValoracion;
            tabEstudioPrevioGeometria.Text = strFrmSolucion.uiTabEstudioPrevio;
            tabOptAvanzadas.Text = strFrmSolucion.uiTabOptAvanzadas1;
            tabOptAvanzadasCoeficientes.Text = strFrmSolucion.uiTabOptAvanzadas2;
            tabEjeVisibilidadGlobal.Text = strFrmSolucion.uiEjeVisibilidadGlobal;
            tabTijeras.Text = strFrmSolucion.uiTabTijera;

            //tab Angeles Eje visibilidad global

            lbTxtSeparacion.Enabled = true;
            lbTxtSeparacion.uiLbl = strFrmSolucion.uiSeparacionCarreteras;
            lbTxtCotaMax.uiLbl = strFrmSolucion.uiPendMaxFiltroCotas;
            lbTxtTrianguloMax.uiLbl = strFrmSolucion.uiPendMaxFiltroTerreno;
            btnCreaEjeVisibilidadG.Text = strFrmSolucion.uiEjeVisibilidadGlobalCrear;
            gbDatos.Text = " ";




            //Solucion
            grSolucionDatos.Text = strFrmSolucion.uiGrSolucion;

            this.ucSolucionNombre.uiLbl = strFrmSolucion.uiSolucionNombre;
            this.chkGenerarSolucionEnvolventes.Text = strFrmSolucion.uiSolucionGenerarEnvolventeMaxMin;
            this.optAvancesCortos.Text = strFrmSolucion.uiSolucionOptAvancesCortos;
            this.optAvancesLargos.Text = strFrmSolucion.uiSolucionOptAvancesLargos;

            this.ucRepeticiones.uiLbl = strFrmSolucion.uiRepetionesMax;
            this.chkAutocorreccion.Text = strFrmSolucion.uiAutocorrecion;


            //Botones
            this.btnEjeBasico.Text = strFrmSolucion.uiBtnEjeBasico;
            this.btnCreateNombre.Text = strFrmSolucion.uiBtnCrearNombreSolucion;
            this.btnEjeBasicoManual.Text = strFrmSolucion.uiBtnEjeBasicoManual;
            this.btnEjeBasicoPM.Text = strFrmSolucion.uiBtnEjeBasicoPM;
            btnEjeTijera.Text = strFrmSolucion.uiBtnEjeBasicoTijera;

            //Tab tijera
            ckSinRectas.Text = strFrmSolucion.uiChBSinRectas;
            ckLongitudMinima.Text = strFrmSolucion.uiChBLongitudMinima;
            ckLongitudMaxima.Text = strFrmSolucion.uiChBLongitudMaxima;
            ckLongitudLimitada.Text = strFrmSolucion.uiChBLongitudLimitada;
            grAddRectas.Text = strFrmSolucion.uiGrAddRectas;
            gbCurvas.Text = strFrmSolucion.uiGrCurvas;
            ucCoeficienteDistEje.uiLbl = strFrmSolucion.ucCoefDistanciaEje;
            ucCoeficienteDistTramo.uiLbl = strFrmSolucion.ucCoefDistanciaTramo;
            ucEntronqueSegundos.uiLbl = strFrmSolucion.uiEntronqueSegundos;
            ckAplicarGranRadio.Text = strFrmSolucion.uiAplicarGranRadio;
            ucGranRadio.uiLbl = strFrmSolucion.uiGranRadio;
            ckPenalizarTramosCortos.Text = strFrmSolucion.uiPenalizarTramosCortos;
            ucEntronqueDistancia.uiLbl = strFrmSolucion.uiDistDistEntronque;
            ucDistanciaConvergencia.uiLbl = strFrmSolucion.uiDistanciaEnfrentarTijeras;
            ckDistEntronquePC.Text = strFrmSolucion.uiDitanciaEntronquePC;
            ckDescartarAlternativasCortas.Text = strFrmSolucion.uiChDescartarAlternCortas;


            #endregion
            #region "SetUp Objetos"


            //SetUp TabPage
            tabCarretera.Tag = ePageTab.carretera;
            tabTijeras.Tag = ePageTab.tijera;
            tabPendientes.Tag = ePageTab.pendiente;
            tabValoraciones.Tag = ePageTab.valoracion;
            tabEstudioPrevioGeometria.Tag = ePageTab.estudioPrevio;

            tabOptAvanzadas.Tag = ePageTab.variablesAvanzadasAbanico;
            tabOptAvanzadasCoeficientes.Tag = ePageTab.variablesAvanzadasCoeficientes;
            tabEjeVisibilidadGlobal.Tag = ePageTab.ejeglobal;
            tabPage1.Tag = ePageTab.ejeglobalactualizado;

            //HIDE TAB PAGES ESTUDIO PREVIO
            if (mEstudioTipo == eEstudioTipo.ESTINF)
            {
                this.tabSolucion.TabPages.Remove(this.tabEstudioPrevioGeometria);
            }

            tabSolucion.SelectedTab = tabCarretera;
            mPageTabPrevio = ePageTab.carretera;

            this.optAvancesCortos.Checked = true;


            var p1 = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoSalida);
            var p2 = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoLlegada);
            var distancia = p1.distTo2d(p2);
            this.ucDistanciaConvergencia.textbox.valorDoubleNull = Math.Round(distancia / 5, 2);

            #endregion
        }


        //Validar Formulario
        private bool isValido(out string iInfoError)
        {

            StringBuilder miErrorInfo = new StringBuilder(string.Empty);

            bool miIsSolucionOk = oValidar.isValidoGrupo(this.grSolucionDatos);
            bool miIsNombreSolucionOk = oExtensionString.isNombreSolucionValido(this.ucSolucionNombre.textbox.Text);
            bool miIsRoadOk = ucRoadDatos1.isValido();
            bool miIsPendienteOk = ucRoadEstructurasPendientes1.isValido();
            bool miIsValoracionOk = ucSolucionValoracion1.isValido();

            bool miIsEstudioPrevioGeometriaOk;

            bool miIsOptAvanzadasAbanicoOk = ucSolucionOptAvanzadas1.isValido();
            bool miIsOptAvanzadasCoeficientesOk = ucSolucionOptAvanzadasCoeficientes1.isValido();


            if (mEstudioTipo == eEstudioTipo.ESTPRE)
            {
                miIsEstudioPrevioGeometriaOk = ucEstudioPrevioDesign1.isValido();
            }
            else if (mEstudioTipo == eEstudioTipo.ESTINF)
            {
                miIsEstudioPrevioGeometriaOk = true;
            }
            else
            {
                throw new oExEnumNotImplemented(mEstudioTipo.ToString());
            }


            if (!miIsSolucionOk)
            {
                miErrorInfo.AppendLine("Datos Solución");
            }

            if (!miIsNombreSolucionOk)
            {
                miErrorInfo.AppendLine(strGeneralUser.uiNombreConCaracteresEspeciales);
            }


            if (!miIsRoadOk)
            {
                miErrorInfo.AppendLine("Datos Carretera");
            }
            if (!miIsPendienteOk)
            {
                miErrorInfo.AppendLine("Datos Pendiente-Estructuras");
            }
            if (!miIsValoracionOk)
            {
                miErrorInfo.AppendLine("Datos Valoración");
            }
            if (!miIsEstudioPrevioGeometriaOk)
            {
                miErrorInfo.AppendLine("Estudio Previo Geometría");
            }
            if (!miIsOptAvanzadasAbanicoOk)
            {
                miErrorInfo.AppendLine("Opciones Avanzadas 1");
            }
            if (!miIsOptAvanzadasCoeficientesOk)
            {
                miErrorInfo.AppendLine("Opciones Avanzadas 2");
            }


            //Salida
            if (miErrorInfo.Length == 0)
            {

                iInfoError = string.Empty;

                return true;
            }
            else
            {
                miErrorInfo.Insert(0, "Error Validación \n");

                iInfoError = miErrorInfo.ToString();

                return false;
            }

        }


        #endregion
        #region "EVENTOS FRM"
        //CHECK VALIDACIONES POR PESTAÑA
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {

            bool miExisteError = false;


            //TAB CARRETERA
            if (mPageTabPrevio == ePageTab.carretera)
            {
                if (!ucRoadDatos1.isValido())
                {
                    miExisteError = true;
                }
            }
            //TAB PENDIENTES
            else if (mPageTabPrevio == ePageTab.pendiente)
            {
                if (!ucRoadEstructurasPendientes1.isValido())
                {
                    miExisteError = true;
                }
            }
            //TAB VALORACIONES
            else if (mPageTabPrevio == ePageTab.valoracion)
            {
                if (!ucSolucionValoracion1.isValido())
                {
                    miExisteError = true;
                }
            }
            //TAB ESTUDIO PREVIO DATOS DISEÑO
            else if (mPageTabPrevio == ePageTab.estudioPrevio)
            {
                if (!ucEstudioPrevioDesign1.isValido())
                {
                    miExisteError = true;
                }
            }

            //TAB OPCIONES AVANZADAS 1
            else if (mPageTabPrevio == ePageTab.variablesAvanzadasAbanico)
            {
                if (!ucSolucionOptAvanzadas1.isValido())
                {
                    miExisteError = true;
                }
            }
            //TAB OPCIONES AVANZADAS COEFICIENTES
            else if (mPageTabPrevio == ePageTab.variablesAvanzadasCoeficientes)
            {
                if (!ucSolucionOptAvanzadasCoeficientes1.isValido())
                {
                    miExisteError = true;
                }
            }
            else if (mPageTabPrevio == ePageTab.ejeglobal)
            {
            }
            else if (mPageTabPrevio == ePageTab.tijera)
            {
            }
            else if (mPageTabPrevio == ePageTab.ejeglobalactualizado)
            {

            }
            else
            {
                throw new oExEnumNotImplemented(mPageTabPrevio.Value.ToString());
            }


            if (miExisteError)
            {
                e.Cancel = true;

                oTadil.data.UserInfo.errorValidacion();
            }

        }
        //CARGO EL ÚLTIMO PAGE SELECCIONADO
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mPageTabPrevio = (ePageTab)tabSolucion.SelectedTab.Tag;
            if (mPageTabPrevio == ePageTab.tijera)
            {
                ucEntronqueDistancia.textbox.valorDoubleNull = getObjRoadDesign().Rp * 2;
            }
            if (mPageTabPrevio == ePageTab.ejeglobalactualizado)
            {
                if (textBox1.Text == "")
                {
                    var sol = this.getObjEjeBasicoSolucion();
                    textBox1.Text = (sol.roadDesign.AijMin / 2).ToString();

                    textBox3.Text = (sol.roadDesign.AijMin / 2).ToString();
                    textBox4.Text = sol.roadDesign.AijMax.ToString();
                }


            }
        }



        private void frmSolucion_FormClosed(object sender, FormClosedEventArgs e)
        {
            mInstance = null;
        }

        #endregion
        #region "Botones"
        public struct DatosFormulario
        {
            public Dictionary<string, Dictionary<string, string>> LabelTexts; // Almacena textos por TabPage
            public Dictionary<string, Dictionary<string, bool>> CheckBoxes;  // Almacena estados de CheckBox por TabPage


        }
        //<summary>
        //EJE BASICO
        //</summary>
        private void btnEjeBasico_Click(object sender, EventArgs e)
        {
            try
            {
                //Validaciones
                string miErrorValidacion;

                if (isValido(out miErrorValidacion))
                {
                    //if (oSingletonTerreno.getInstance.terreno != null && oSingletonTerreno.getInstance.get_PolEnvolvente() != null)
                    //{


                    frmAppManager.getInstance.Hide();

                    var datossolucion = this.getObjEjeBasicoSolucion();
                    if (!datossolucion.roadDesign.isNormativaCorrecta())
                    {
                        var userInfo = new oUserInfo();
                        var result = userInfo.showSiNo(
                            "La modificación de los valores por defecto no garantiza el cumplimiento de la normativa, ¿Desea continuar?");
                        if (result == DialogResult.No) return;
                    }


                    var eje_visibilidad = oComandoEjeBasico.create(datossolucion, this.ucRepeticiones.textbox.valorInt, chkAutocorreccion.Checked, Dibujar.Checked);
                    if (eje_visibilidad != null)
                    {
                        if (eje_visibilidad.NumberOfVertices > 0)
                        {
                            GuardarDatosEnJSON(eje_visibilidad, datossolucion.solucionPrefijo);
                        }
                    }

                    /*}
                    else
                    {
                        oTadil.data.UserInfo.showInfo("El terreno no esta cargado");
                    }*/
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                if (trace.GetFrame(0).GetMethod().Name == "set_Layer")
                {
                    oTadil.data.UserInfo.showInfo(strError.eCapaEliminada);
                }
                else
                {
                    oTadil.data.UserInfo.showError(ex);
                }
            }
            catch (oExTramoEntronqueNoCumple ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }
        }

        private void btnEjeTijera_Click(object sender, EventArgs e)
        {
            try
            {
                //Validaciones
                string miErrorValidacion;

                if (isValido(out miErrorValidacion))
                {
                    //if (oSingletonTerreno.getInstance.terreno != null && oSingletonTerreno.getInstance.get_PolEnvolvente() != null)
                    //{

                    /*
                     * Se cambia para utilizar el KD-Tree
                     */
                    //oSingletonPuntosTerreno.getInstance.Cargar_MDT();
                    frmAppManager.getInstance.Hide();

                    //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
                    oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();




                    var tijeradata = new oEjeTijeraData(chkAutocorreccion.Checked, ucRepeticiones.textbox.valorInt,
                        false,
                        0.05, ckDistEntronquePC.Checked, ucDistEntronquePC.valorDouble, ucEntronqueDistancia.valorDouble, ucDistanciaConvergencia.valorDouble,
                        ckPenalizarTramosCortos.Checked, ucCoeficienteDistTramo.textbox.valorDouble,
                        ucCoeficienteDistEje.textbox.valorDouble);


                    var datossolucion = getObjEjeBasicoFerrocarril();
                    if (!datossolucion.roadDesign.isNormativaCorrecta())
                    {
                        var userInfo = new oUserInfo();
                        var result = userInfo.showSiNo(
                            "La modificación de los valores por defecto no garantiza el cumplimiento de la normativa, ¿Desea continuar?");
                        if (result == DialogResult.No) return;
                    }

                    Polyline eje_visibilidad = oComandoEjeBasicoFerrocarril.create(datossolucion, tijeradata, Dibujar.Checked);
                    if (eje_visibilidad != null)
                    {
                        if (eje_visibilidad.NumberOfVertices > 0)
                        {
                            GuardarDatosEnJSON(eje_visibilidad, datossolucion.solucionPrefijo);
                        }
                    }
                    /*}
                    else
                    {
                        oTadil.data.UserInfo.showInfo("El terreno no esta cargado");
                    }*/
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                if (trace.GetFrame(0).GetMethod().Name == "set_Layer")
                {
                    oTadil.data.UserInfo.showInfo(strError.eCapaEliminada);
                }
                else
                {
                    oTadil.data.UserInfo.showError(ex);
                }
            }
            catch (oExTramoEntronqueNoCumple ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (oExTramoOrigenTramoDestinoNoConfig ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }
        }


        private void btnEjeBasicoManual_Click(object sender, EventArgs e)
        {

            try
            {
                //Validaciones
                string miErrorValidacion;

                if (isValido(out miErrorValidacion))
                {
                    frmAppManager.getInstance.Hide();

                    var datossolucion = this.getObjEjeBasicoSolucion();
                    if (!datossolucion.roadDesign.isNormativaCorrecta())
                    {
                        var userInfo = new oUserInfo();
                        var result = userInfo.showSiNo(
                            "La modificación de los valores por defecto no garantiza el cumplimiento de la normativa, ¿Desea continuar?");
                        if (result == DialogResult.No) return;
                    }
                    oComandoEjeBasico.createManual(datossolucion);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (oExTramoEntronqueNoCumple ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }
        }


        private void btnCreateNombre_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucRoadDatos1.isValido())
                {
                    string miNombre = this.ucRoadDatos1.nombreSolucion() + this.ucSolucionValoracion1.nombreValoracion() + this.tipoAvance();

                    this.ucSolucionNombre.textbox.Text = miNombre.removeEspaciosBlanco();
                }

            }
            catch (Exception ex)
            {

                oTadil.data.UserInfo.showError(ex);
            }
        }


        #endregion




        public static void deleteInstance()
        {
            mInstance = null;
        }

        private void btnCrearAbanico_Click(object sender, EventArgs e)
        {
            try
            {

                //Validaciones
                string miErrorValidacion;

                if (isValido(out miErrorValidacion))
                {
                    frmAppManager.getInstance.Hide();

                    tadLayLogica.Comandos.oComandoEjeBasico.createAbanicoByPoint(this.getObjEjeBasicoSolucion());

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {

                frmAppManager.getInstance.Show();
            }
        }

        private void btnEjeBasicoPM_Click(object sender, EventArgs e)
        {

            try
            {
                //Validaciones
                string miErrorValidacion;

                if (isValido(out miErrorValidacion))
                {
                    frmAppManager.getInstance.Hide();
                    if (this.getTipoAbanico() == eAbanicoTipo.largos)
                    {
                        oTadil.data.UserInfo.showInfo(strFrmSolucion.uiObligadoAbanicosCortos);
                    }


                    var datossolucion = this.getObjEjeBasicoSolucionObligadoAvancesCortos();
                    if (!datossolucion.roadDesign.isNormativaCorrecta())
                    {
                        var userInfo = new oUserInfo();
                        var result = userInfo.showSiNo(
                            "La modificación de los valores por defecto no garantiza el cumplimiento de la normativa, ¿Desea continuar?");
                        if (result == DialogResult.No) return;
                    }

                    tadLayLogica.Comandos.oComandoEjeBasico.createProyeccionPuntoMedio(datossolucion);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (oExTramoEntronqueNoCumple ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }
        }

        private void chkAutocorreccion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutocorreccion.Checked)
            {
                ucRepeticiones.Enabled = true;
            }
            else
            {
                ucRepeticiones.Enabled = false;
            }
        }

        private void btnCreaEjeVisibilidadG_Click(object sender, EventArgs e)
        {


            try
            {
                //Validaciones
                string miErrorValidacion;

                if (isValido(out miErrorValidacion))
                {
                    //if (oSingletonTerreno.getInstance.terreno != null && oSingletonTerreno.getInstance.get_PolEnvolvente() != null)
                    //{
                    frmAppManager.getInstance.Hide();
                    if (this.lbTxtSeparacion.Validate(true) && this.lbTxtTrianguloMax.Validate(true) && this.lbTxtCotaMax.Validate(true))
                    {
                        //if (this.chBFerrocarriles.Checked)
                        // {
                        //   tadLayLogica.Comandos.oComandoEjeVisibilidadGlobal.create(this.getObjEjeBasicoSolucion(), this.lbTxtSeparacionFerrorcarril.valorDouble, this.lbTxtCotaMax.valorDoubleNull, this.lbTxtTrianguloMax.valorDoubleNull);
                        // }
                        //else
                        //{
                        double angulo = -1;
                        if (textBox5.Text != "")
                        {
                            angulo = double.Parse(textBox5.Text);
                        }
                        tadLayLogica.Comandos.oComandoEjeVisibilidadGlobal.create(this.getObjEjeBasicoSolucion(), this.lbTxtSeparacion.valorDouble,
                            this.lbTxtCotaMax.valorDoubleNull, this.lbTxtTrianguloMax.valorDoubleNull, angulo, Dibujar.Checked, Preciso.Checked);
                        //}
                    }
                    /*}
                    else
                    {
                        oTadil.data.UserInfo.showInfo("El terreno no esta cargado");
                    }*/
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (oExTramoEntronqueNoCumple ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
                oSingletonPuntosTerreno.getInstance.Set_Preciso(true);
                oSingletonPuntosTerrenoASC.getInstance.Set_Preciso(true);
            }
        }


        /// <summary>
        /// Datos de la Solución
        /// </summary>
        private oEjeBasicoFerrocarril getObjEjeBasicoFerrocarril()
        {

            oEjeBasicoFerrocarril miEjeBasicoSolucion = new oEjeBasicoFerrocarril(this.ucSolucionNombre.textbox.Text,
                                                                            this.chkGenerarSolucionEnvolventes.Checked,
                                                                            this.getObjRoadDesign(),
                                                                            this.getObjRoadPendientes(),
                                                                            this.getObjCoeMinAlturasMaximas(),
                                                                             this.getObjValoracion(),
                                                                             this.getObjDataAbanico(),
                                                                   this.getObjEstudioPrevio(), (eEstudioTipo)mEstudioTipo, ucEntronqueDistancia.valorDouble, ucDistanciaConvergencia.valorDouble, ckPenalizarTramosCortos.Checked);


            return miEjeBasicoSolucion;

        }

        private void ckAplicarGranRadio_CheckedChanged(object sender, EventArgs e)
        {
            ucGranRadio.Enabled = ckAplicarGranRadio.Checked;
        }

        private void ckDistEntronquePC_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDistEntronquePC.Checked)
            {
                ucDistEntronquePC.Enabled = true;
            }
            else
            {
                ucDistEntronquePC.Enabled = false;
            }
        }


        private void ckSinRectas_CheckedChanged(object sender, EventArgs e)
        {
            ckDescartarAlternativasCortas.Enabled = ckSinRectas.Checked;
            ucDistDescartarAltervativa.Enabled = ckSinRectas.Checked && ckDescartarAlternativasCortas.Checked;
        }

        private void ckDescartarAlternativasCortas_CheckedChanged(object sender, EventArgs e)
        {
            ucDistDescartarAltervativa.Enabled = ckSinRectas.Checked && ckDescartarAlternativasCortas.Checked;
        }



        public void GuardarDatosEnJSON(Polyline eje_visibilidad, string nombre)
        {

            DatosSolucion datosSolucion = new DatosSolucion();
            //carreteras
            datosSolucion.ucRoadGrupo = ucRoadDatos1.roadGrupo.ToString();
            datosSolucion.ucVp = ucRoadDatos1.vp;
            datosSolucion.ucRadio = ucRoadDatos1.rp;
            datosSolucion.ucPeralte = ucRoadDatos1.peraltePC;
            datosSolucion.ucValorMinimoSalidaLlegada = ucRoadDatos1.ajMinSalidaLlegada;
            datosSolucion.ucAijMinimoTramo = ucRoadDatos1.ajMin;
            datosSolucion.ucAijMaximoTramo = ucRoadDatos1.ajMax;
            datosSolucion.ucAvanceMaximo = ucRoadDatos1.ucAMax;
            datosSolucion.ucKvConvexo = ucRoadDatos1.kvConvexo;
            datosSolucion.ucKvConcavo = ucRoadDatos1.kvConcavo;
            datosSolucion.chkPermitirReduccionesVelocidad = ucRoadDatos1.chkPermitirRedVel;
            datosSolucion.ucRadioCondicionadoLmin = ucRoadDatos1.ucRadCondLmin;

            //tijeras
            datosSolucion.sinrectas = ckSinRectas.Checked;
            datosSolucion.longitudminima = ckLongitudMinima.Checked;
            datosSolucion.longitudmaxima = ckLongitudMaxima.Checked;
            datosSolucion.longitudlimitada = ckLongitudLimitada.Checked;
            datosSolucion.descartaralternativascortas = ckDescartarAlternativasCortas.Checked;
            datosSolucion.distdescartaralternativa = ucDistDescartarAltervativa.uitxt;

            datosSolucion.aplicargranradio = ckAplicarGranRadio.Checked;
            datosSolucion.granradio = ucGranRadio.uitxt;

            datosSolucion.coeficientedisttramo = ucCoeficienteDistTramo.uitxt;
            datosSolucion.coeficientedisteje = ucCoeficienteDistEje.uitxt;
            datosSolucion.entronquesegundos = ucEntronqueSegundos.uitxt;
            datosSolucion.entronquedistancia = ucEntronqueDistancia.uitxt;
            datosSolucion.distanciaconvergencia = ucDistanciaConvergencia.uitxt;
            datosSolucion.penalizartramoscortos = ckPenalizarTramosCortos.Checked;
            datosSolucion.distentronque = ckDistEntronquePC.Checked;
            datosSolucion.distentronquepc = ucDistEntronquePC.uitxt;

            //pendientes
            var estrucpendientes = ucRoadEstructurasPendientes1.getRoadPendiente();
            datosSolucion.ucRoadPendienteMaximaPC = estrucpendientes.calzadaPendienteProyectoMaximaPC;
            datosSolucion.ucRoadPendienteMinimaPC = estrucpendientes.calzadaPendienteProyectoMinimaPC;
            datosSolucion.ucEstructurasPendienteMaximaPC = estrucpendientes.estructurasPendienteProyectoMaximaPC;
            datosSolucion.ucEstructurasPendienteMinimoPC = estrucpendientes.estructurasPendienteProyectoMinimaPC;



            //valoraciones
            var valoraciones = ucSolucionValoracion1.getValoracion();
            datosSolucion.valoracionDistanciaPC = valoraciones.valoracionDistanciaPC;
            datosSolucion.valoracionPendientePC = valoraciones.valoracionPendientePC;
            datosSolucion.valoracionCosteImplantacionPC = valoraciones.valoracionCosteImplantacionPC;


            //estudiopreviogeometria
            datosSolucion.ucAnchoPlataforma = ucEstudioPrevioDesign1.getucAnchoPlataforma();
            datosSolucion.ucDesmonteAlturaMaxima = ucEstudioPrevioDesign1.getucDesmonteAlturaMaxima();
            datosSolucion.ucTerraplenAlturaMaxima = ucEstudioPrevioDesign1.getucTerraplenAlturaMaxima();
            datosSolucion.ucDesmonteTaludPC = ucEstudioPrevioDesign1.getucDesmonteTaludPC();
            datosSolucion.ucTerraplenTaludPC = ucEstudioPrevioDesign1.getucTerraplenTaludPC();
            datosSolucion.ucGenerarPuntes = ucEstudioPrevioDesign1.getucGenerarPuntes();
            datosSolucion.ucGenerarTuneles = ucEstudioPrevioDesign1.getucGenerarTuneles();
            datosSolucion.ucPuenteAlturaMaxima = ucEstudioPrevioDesign1.getucPuenteAlturaMaxima();
            datosSolucion.ucCosteImplantacion = ucEstudioPrevioDesign1.getucCosteImplantacion();
            datosSolucion.ucCosteDesmonteUnitario = ucEstudioPrevioDesign1.geucCosteDesmonteUnitario();
            datosSolucion.ucCosteTerraplenUnitario = ucEstudioPrevioDesign1.getucCosteTerraplenUnitario();
            datosSolucion.ucCostePuentesViaductosUnitario = ucEstudioPrevioDesign1.getucCostePuentesViaductosUnitario();
            datosSolucion.ucCosteTunelesUnitario = ucEstudioPrevioDesign1.getucCosteTunelesUnitario();



            //opcionesavanzadas
            var optavanzadas = ucSolucionOptAvanzadas1.getDataAbanico(getTipoAbanico());
            datosSolucion.ucAbanicoAnguloTotalGrados = optavanzadas.anguloTotalGrados;
            datosSolucion.ucAbanicoDisretizacionGrados = optavanzadas.gradosDiscretizacionProyecto;
            datosSolucion.ucAbanicoTramoDiscretizacionMetros = optavanzadas.tramoAbanicoDiscretizacion;
            datosSolucion.ucAbanicoToleranciaPuntoObjetivo = optavanzadas.toleranciaPuntoObjetivoPC;
            datosSolucion.chkAbanicoInvalidar = ucSolucionOptAvanzadas1.invalidarTramosIncrementoLongitud;
            datosSolucion.ucInvalidarTramosLongitudPC = ucSolucionOptAvanzadas1.invalidarTramosTramosIncrementoLongitudPC();
            datosSolucion.chkRoadConsiderarAijConstante = ucSolucionOptAvanzadas1.isAijConstante;


            //optavanzadascoeficientes
            datosSolucion.ucCoeRoadPendienteMaxima = ucSolucionOptAvanzadasCoeficientes1.coeMinPendienteCarretera;
            datosSolucion.ucCoeEstructurasPendienteMaxima = ucSolucionOptAvanzadasCoeficientes1.coeMinPendienteEstructura;
            var datos_ucSolucionOptAvanzadasCoeficientes1 = ucSolucionOptAvanzadasCoeficientes1.getCoeMinoracionAlturasMaximas();
            datosSolucion.ucCoeDesmonteAlturaMaxima = datos_ucSolucionOptAvanzadasCoeficientes1.desmonteCoeficienteMinoracionAlturaMaxima;
            datosSolucion.ucCoeTerraplenAlturaMaxima = datos_ucSolucionOptAvanzadasCoeficientes1.terraplenCoeficienteMinoracionAlturaMaxima;
            datosSolucion.ucCoePilaAlturaMaxima = datos_ucSolucionOptAvanzadasCoeficientes1.pilaCoeficienteMinoracionAlturaMaxima;

            //ejevisibilidadglobal
            datosSolucion.triangulomax = lbTxtTrianguloMax.uitxt;
            datosSolucion.cotamax = lbTxtCotaMax.uitxt;
            datosSolucion.separacion = lbTxtSeparacion.uitxt;

            //datos generales
            datosSolucion.nombresol = ucSolucionNombre.uitxt;
            datosSolucion.solEnvolventes = chkGenerarSolucionEnvolventes.Checked;
            datosSolucion.avancescortos = optAvancesCortos.Checked;
            datosSolucion.avanceslargos = optAvancesLargos.Checked;
            datosSolucion.autocorreccion = chkAutocorreccion.Checked;
            datosSolucion.repeticiones = ucRepeticiones.uitxt;

            //eje visibilidad
            List<Point2DJson> eje_vis = new List<Point2DJson>();
            for (int i = 0; i < eje_visibilidad.NumberOfVertices; i++)
            {
                // Obtener cada vértice (punto) de la polilínea
                eje_vis.Add(new Point2DJson(eje_visibilidad.GetPoint2dAt(i)));
            }
            datosSolucion.Eje_Visibilidad = eje_vis;

            //ficheros BBDD
            datosSolucion.ucFileNormasVpRadio = oTadil.data.Files.fileNormasCarreteras;
            datosSolucion.ucFileNormasVpKv = oTadil.data.Files.fileNormasCarreterasKv;
            datosSolucion.ucFileBaseDatos = oTadil.data.Files.fileBbdd;

            //datos proyecto
            datosSolucion.seccionIntervalo = oSingletonProyecto.getInstance.seccionIntervalo;

            //datos seccion zonas generales
            dsApp.tbSeccionZonasGeneralesRow miRow = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid("APP");

            datosSolucion.ucSeccionGrupo = miRow.idRoadGrupo;
            datosSolucion.ucSeccionTipo = miRow.idRoadTipo;
            datosSolucion.ucSeccionItem = miRow.idRoadSeccion.ToString();
            datosSolucion.ucSeccionMacroPrecio = miRow.idRoadMacroPrecios.ToString();
            datosSolucion.ucZonasMovimientoTierras1 = miRow.idZonaMovimientoTierrasGeneral.ToString();
            datosSolucion.ucZonasCimentacion1 = miRow.idZonaCimentacionGeneral.ToString();
            datosSolucion.ucZonasEstructuras1 = miRow.idZonaEstructurasGeneral.ToString();
            datosSolucion.ucZonasTuneles1 = miRow.idZonaTunelesGeneral.ToString();


            string json = JsonConvert.SerializeObject(datosSolucion, Formatting.Indented);
            oTadil.data.UserInfo.showInfo("Se guardará un archivo con los datos de la solución");
            string filtro = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            oTadil.data.Files.SaveAsFileFromDialog(json, filtro, nombre);

        }

        private void CargarDatos_Click(object sender, EventArgs e)
        {
            try
            {

                frmAppManager.getInstance.Hide();
                //Cargo el Fichero
                string miFile = oTadil.data.Files.getFileTxt();
                try
                {
                    string jsonContent = File.ReadAllText(miFile);
                    var datosSolucion = JsonConvert.DeserializeObject<DatosSolucion>(jsonContent);

                    Cargar_Datos_Solucion(datosSolucion);

                }
                catch (Exception ex)
                {
                    oTadil.data.UserInfo.showInfo("Error al cargar los datos");
                }

            }
            catch (tadLayShare.oExRowNotFound)
            {
                oTadil.data.UserInfo.showInfo("Error al cargar los datos");
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                oTadil.data.UserInfo.showInfo("Datos Cargados con éxito");

                frmAppManager.getInstance.Show();
            }
        }

        public void Cargar_Datos_Solucion(DatosSolucion datosSolucion)
        {
            //carreteras

            eRoadGrupo grupo = (eRoadGrupo)Enum.Parse(typeof(eRoadGrupo), datosSolucion.ucRoadGrupo, true);
            ucRoadDatos1.roadGrupo = grupo;
            ucRoadDatos1.vp = datosSolucion.ucVp;
            ucRoadDatos1.rp = datosSolucion.ucRadio;
            ucRoadDatos1.peraltePC = datosSolucion.ucPeralte;
            ucRoadDatos1.ajMinSalidaLlegada = datosSolucion.ucValorMinimoSalidaLlegada;
            ucRoadDatos1.ajMin = datosSolucion.ucAijMinimoTramo;
            ucRoadDatos1.ajMax = datosSolucion.ucAijMaximoTramo;
            ucRoadDatos1.ucAMax = datosSolucion.ucAvanceMaximo;
            ucRoadDatos1.kvConvexo = datosSolucion.ucKvConvexo;
            ucRoadDatos1.kvConcavo = datosSolucion.ucKvConcavo;
            ucRoadDatos1.chkPermitirRedVel = datosSolucion.chkPermitirReduccionesVelocidad;
            ucRoadDatos1.ucRadCondLmin = datosSolucion.ucRadioCondicionadoLmin;


            //tijeras
            ckSinRectas.Checked = datosSolucion.sinrectas;
            ckLongitudMinima.Checked = datosSolucion.longitudminima;
            ckLongitudMaxima.Checked = datosSolucion.longitudmaxima;
            ckLongitudLimitada.Checked = datosSolucion.longitudlimitada;
            ckDescartarAlternativasCortas.Checked = datosSolucion.descartaralternativascortas;
            ucDistDescartarAltervativa.uitxt = datosSolucion.distdescartaralternativa;

            ckAplicarGranRadio.Checked = datosSolucion.aplicargranradio;
            ucGranRadio.uitxt = datosSolucion.granradio;

            ucCoeficienteDistTramo.uitxt = datosSolucion.coeficientedisttramo;
            ucCoeficienteDistEje.uitxt = datosSolucion.coeficientedisteje;
            ucEntronqueSegundos.uitxt = datosSolucion.entronquesegundos;
            ucEntronqueDistancia.uitxt = datosSolucion.entronquedistancia;
            ucDistanciaConvergencia.uitxt = datosSolucion.distanciaconvergencia;
            ckPenalizarTramosCortos.Checked = datosSolucion.penalizartramoscortos;
            ckDistEntronquePC.Checked = datosSolucion.distentronque;
            ucDistEntronquePC.uitxt = datosSolucion.distentronquepc;

            //pendientes
            ucRoadEstructurasPendientes1.Set_ucEstructurasPendienteMaximaPC(datosSolucion.ucRoadPendienteMaximaPC);
            ucRoadEstructurasPendientes1.Set_ucRoadPendienteMinimaPC(datosSolucion.ucRoadPendienteMinimaPC);
            ucRoadEstructurasPendientes1.Set_ucEstructurasPendienteMaximaPC(datosSolucion.ucEstructurasPendienteMaximaPC);
            ucRoadEstructurasPendientes1.Set_ucEstructurasPendienteMinimoPC(datosSolucion.ucEstructurasPendienteMinimoPC);

            //valoraciones
            ucSolucionValoracion1.Set_ucValoracionDistanciaPC(datosSolucion.valoracionDistanciaPC);
            ucSolucionValoracion1.Set_ucValoracionPendientePC(datosSolucion.valoracionPendientePC);
            ucSolucionValoracion1.Set_ucValoracionCostesPC(datosSolucion.valoracionCosteImplantacionPC);

            //estudiopreviogeometria
            ucEstudioPrevioDesign1.setucAnchoPlataforma(datosSolucion.ucAnchoPlataforma);
            ucEstudioPrevioDesign1.setucDesmonteAlturaMaxima(datosSolucion.ucDesmonteAlturaMaxima);
            ucEstudioPrevioDesign1.setucTerraplenAlturaMaxima(datosSolucion.ucTerraplenAlturaMaxima);
            ucEstudioPrevioDesign1.setucDesmonteTaludPC(datosSolucion.ucDesmonteTaludPC);
            ucEstudioPrevioDesign1.setucTerraplenTaludPC(datosSolucion.ucTerraplenTaludPC);
            ucEstudioPrevioDesign1.setucGenerarPuntes(datosSolucion.ucGenerarPuntes);
            ucEstudioPrevioDesign1.setucGenerarTuneles(datosSolucion.ucGenerarTuneles);
            ucEstudioPrevioDesign1.setucPuenteAlturaMaxima(datosSolucion.ucPuenteAlturaMaxima);
            ucEstudioPrevioDesign1.setucCosteImplantacion(datosSolucion.ucCosteImplantacion);
            ucEstudioPrevioDesign1.setucCosteDesmonteUnitario(datosSolucion.ucCosteDesmonteUnitario);
            ucEstudioPrevioDesign1.setucCosteTerraplenUnitario(datosSolucion.ucCosteTerraplenUnitario);
            ucEstudioPrevioDesign1.setucCostePuentesViaductosUnitario(datosSolucion.ucCostePuentesViaductosUnitario);
            ucEstudioPrevioDesign1.setucCosteTunelesUnitario(datosSolucion.ucCosteTunelesUnitario);

            //opcionesavanzadas
            ucSolucionOptAvanzadas1.Set_ucAbanicoAnguloTotalGrados(datosSolucion.ucAbanicoAnguloTotalGrados);
            ucSolucionOptAvanzadas1.Set_ucAbanicoDiscretizacionGrados(datosSolucion.ucAbanicoDisretizacionGrados);
            ucSolucionOptAvanzadas1.Set_ucAbanicoTramoDiscretizacionMetros(datosSolucion.ucAbanicoTramoDiscretizacionMetros);
            ucSolucionOptAvanzadas1.Set_ucAbanicoToleranciaPuntoObjetivo(datosSolucion.ucAbanicoToleranciaPuntoObjetivo);
            ucSolucionOptAvanzadas1.Set_invalidarTramosIncrementoLongitud(datosSolucion.chkAbanicoInvalidar);
            ucSolucionOptAvanzadas1.Set_ucInvalidarTramosLongitudPC(datosSolucion.ucInvalidarTramosLongitudPC.ToString());
            ucSolucionOptAvanzadas1.Set_chkRoadConsiderarAijConstante(datosSolucion.chkRoadConsiderarAijConstante);


            //optavanzadascoeficientes
            ucSolucionOptAvanzadasCoeficientes1.Set_coeMinPendienteCarretera(datosSolucion.ucCoeRoadPendienteMaxima);
            ucSolucionOptAvanzadasCoeficientes1.Set_coeMinPendienteEstructura(datosSolucion.ucCoeEstructurasPendienteMaxima);
            ucSolucionOptAvanzadasCoeficientes1.Set_ucCoeDesmonteAlturaMaxima(datosSolucion.ucCoeDesmonteAlturaMaxima);
            ucSolucionOptAvanzadasCoeficientes1.Set_ucCoeTerraplenAlturaMaxima(datosSolucion.ucCoeTerraplenAlturaMaxima);
            ucSolucionOptAvanzadasCoeficientes1.Set_ucCoePilaAlturaMaxima(datosSolucion.ucCoePilaAlturaMaxima);

            // ejevisibilidadglobal
            lbTxtTrianguloMax.uitxt = datosSolucion.triangulomax;
            lbTxtCotaMax.uitxt = datosSolucion.cotamax;
            lbTxtSeparacion.uitxt = datosSolucion.separacion;

            // datos generales
            ucSolucionNombre.uitxt = datosSolucion.nombresol;
            chkGenerarSolucionEnvolventes.Checked = datosSolucion.solEnvolventes;
            optAvancesCortos.Checked = datosSolucion.avancescortos;
            optAvancesLargos.Checked = datosSolucion.avanceslargos;
            chkAutocorreccion.Checked = datosSolucion.autocorreccion;
            ucRepeticiones.uitxt = datosSolucion.repeticiones;


            //eje visibilidad

            Polyline myPol = new Polyline();
            myPol.SetDatabaseDefaults();
            foreach (var item in datosSolucion.Eje_Visibilidad)
            {
                myPol.AddVertexAt(0, new Point2d(item.X, item.Y), 0, 0, 0);
            }
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead);

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                    // Crear la Polyline con puntos
                    Polyline polyline = new Polyline();
                    for (int i = 0; i < datosSolucion.Eje_Visibilidad.Count(); i++)
                    {
                        polyline.AddVertexAt(i, new Point2d(datosSolucion.Eje_Visibilidad[i].X, datosSolucion.Eje_Visibilidad[i].Y), 0, 0, 0);
                    }
                    polyline.Layer = oTadil.data.Layer.visibilidadEje.name;
                    polyline.Closed = false; // Cierra la polilínea

                    // Agregar la Polyline al dibujo
                    acBlockTableRec.AppendEntity(polyline);
                    tr.AddNewlyCreatedDBObject(polyline, true);

                    // Confirmar la transacción
                    tr.Commit();

                }
            }


            //ficheros BBDD
            oTadil.data.Files.fileNormasCarreteras = datosSolucion.ucFileNormasVpRadio;
            oTadil.data.Files.fileNormasCarreterasKv = datosSolucion.ucFileNormasVpKv;
            oTadil.data.Files.fileBbdd = datosSolucion.ucFileBaseDatos;


            //datos proyecto
            oSingletonProyecto.getInstance.Set_seccionIntervalo(datosSolucion.seccionIntervalo);

            //datos seccion zonas generales
            dsApp.tbSeccionZonasGeneralesRow miRow = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid("APP");

            miRow.idRoadGrupo = datosSolucion.ucSeccionGrupo;
            miRow.idRoadTipo = datosSolucion.ucSeccionTipo;
            miRow.idRoadSeccion = Guid.Parse(datosSolucion.ucSeccionItem);
            miRow.idRoadMacroPrecios = Guid.Parse(datosSolucion.ucSeccionMacroPrecio);
            miRow.idZonaMovimientoTierrasGeneral = Guid.Parse(datosSolucion.ucZonasMovimientoTierras1);
            miRow.idZonaCimentacionGeneral = Guid.Parse(datosSolucion.ucZonasCimentacion1);
            miRow.idZonaEstructurasGeneral = Guid.Parse(datosSolucion.ucZonasEstructuras1);
            miRow.idZonaTunelesGeneral = Guid.Parse(datosSolucion.ucZonasTuneles1);


        }

        private void Preciso_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Validaciones
                string miErrorValidacion;

                if (isValido(out miErrorValidacion))
                {

                    frmAppManager.getInstance.Hide();
                    if (true)
                    {
                        double angulo = -1;
                        if (textBox2.Text != "")
                        {
                            angulo = double.Parse(textBox2.Text);
                        }
                        EjeGlobal ejeGlobal = new EjeGlobal(this.getObjEjeBasicoSolucion(), double.Parse(textBox1.Text), angulo, double.Parse(textBox3.Text), double.Parse(textBox4.Text), Dibujar.Checked, Preciso.Checked);
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            finally
            {
                frmAppManager.getInstance.Show();
                oSingletonPuntosTerreno.getInstance.Set_Preciso(true);
                oSingletonPuntosTerrenoASC.getInstance.Set_Preciso(true);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos (0-9) y teclas de control (Backspace, Delete, etc.)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo dígitos (0-9) y teclas de control (Backspace, Delete, etc.)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ucRoadDatos1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //Validaciones
                string miErrorValidacion;

                if (isValido(out miErrorValidacion))
                {
                    frmAppManager.getInstance.Hide();




                    var datossolucion = this.getObjEjeBasicoSolucion();



                    if (!datossolucion.roadDesign.isNormativaCorrecta())
                    {
                        var userInfo = new oUserInfo();
                        var result = userInfo.showSiNo(
                            "La modificación de los valores por defecto no garantiza el cumplimiento de la normativa, ¿Desea continuar?");
                        if (result == DialogResult.No) return;
                    }




                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Title = "Selecciona un archivo LandXML", // Título de la ventana
                        Filter = "Archivos LandXML (*.xml)|*.xml|Todos los archivos (*.*)|*.*", // Filtro para solo mostrar archivos .landxml
                        DefaultExt = "xml" // Extensión por defecto
                    };

                    // Mostrar el cuadro de diálogo y comprobar si se ha seleccionado un archivo
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {

                        //cargarlandxml(datossolucion, openFileDialog.FileName);

                        // Obtener la ruta del archivo seleccionado
                        string archivoLandXML = openFileDialog.FileName;
                        
                        ImportResult landXmlImporter = new LandXmlImporter().Import(archivoLandXML, 2,datossolucion.roadDesign.peralte);
                        TrazadoLandXml trazado = new TrazadoLandXml(archivoLandXML, datossolucion.roadDesign.peralte);
                        EjeTrazado miEjeTrazadoTadil = new EjeTrazado(trazado.vertices, trazado.Trazado, datossolucion.roadDesign.peralte, 2);
                        MemoryStream memory_miEjeTrazadoTadil = miEjeTrazadoTadil.guardarEjeTrazado();



                        //string json_miEjeTrazadoTadil = JsonConvert.SerializeObject(miEjeTrazadoTadil, Formatting.Indented);


                        Guid mIdSolucionObraLineal = Guid.NewGuid();

                        // Registrar el GUID en tbSolucion ANTES de createMediciones,
                        // porque la FK FK_tbRoadSolucion_tbMedicionesCad exige que el
                        // registro padre exista en tbSolucion cuando se inserten mediciones.
                        oDalTbSolucion.addEjeBasico(
                            datossolucion.solucionPrimariaNombre,
                            "",//eje.Handle.ToString(),
                            "",//eje.Handle.ToString(),
                            datossolucion.roadDesign,
                            datossolucion.roadPendientes,
                            getObjCoeMinAlturasMaximas(), true);

                        // Recuperar el GUID real que addEjeBasico asignó a tbSolucion
                        var miFilaSolucion = oSingletonDsApp.getInstance.dataset.tbSolucion
                            .Cast<tadLayData.dsApp.tbSolucionRow>()
                            .FirstOrDefault(r => r.nombre == datossolucion.solucionPrimariaNombre);
                        if (miFilaSolucion != null)
                        {
                            mIdSolucionObraLineal = miFilaSolucion.id;
                        }

                        using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                        {
                            using (oSolucion miSolucion = new oSolucion(mIdSolucionObraLineal))
                            {
                                using (Transaction tr = oCadManager.StartTransaction())
                                {
                                    oDalTbSolucion.Guardar_EjeAmilia(miSolucion.idSolucion, memory_miEjeTrazadoTadil);
                                }
                            }
                        }
                        var eje = Planta(miEjeTrazadoTadil, datossolucion.solucionPrimariaNombre);

                        Alzado miAlzado = AlzadoAmilia.ReconstruirAlzado(
                            landXmlImporter.AlzadoComponentes,
                            landXmlImporter.KVs.ToArray(),
                            velocidad: datossolucion.roadDesign.Vp,
                            intervaloSecciones: oSingletonProyecto.getInstance.seccionIntervalo,
                            eje,
                            oSingletonTerreno.getInstance.getZFromXY,
                            oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto
                        );
                        MemoryStream memory_miAlzado = miAlzado.guardarAlzado();
                        using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                        {
                            using (oSolucion miSolucion = new oSolucion(mIdSolucionObraLineal))
                            {
                                using (Transaction tr = oCadManager.StartTransaction())
                                {
                                    oDalTbSolucion.Guardar_Alzado(miSolucion.idSolucion, memory_miAlzado);
                                }
                            }
                        }

                        return;






                        string json_miAlzado = JsonConvert.SerializeObject(miAlzado, Formatting.Indented);
                        Point3d miPtoInsert = (Point3d)engCadNet.oTools.getPointCad(strUserCad.uiSelectPtoInsertPerfilLongitudinal);


                        Guitarra miGuitarra = tadLayLogica.EjeLongitudinalTadil.oFactoryPerfilLongitudinalTadil.getGuitarra(miPtoInsert, miAlzado, 100, 10);

                        PerfilLongitudinalDraw miPerfilDraw = new PerfilLongitudinalDraw(
                            miGuitarra,
                            miAlzado,
                            miEjeTrazadoTadil,
                            oSingletonTerreno.getInstance.getZFromXY,
                            oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);
                        string json_miPerfilDraw = JsonConvert.SerializeObject(miPerfilDraw, Formatting.Indented);
                        //Gestion de Capas
                        oTadilLayerPerfilLongitudinalTadil miLayerPerfilRotular = new oTadilLayerPerfilLongitudinalTadil(datossolucion.solucionPrimariaNombre);
                        miLayerPerfilRotular.deleteItems();
                        //DRAW
                        miPerfilDraw.drawGuitarra(miLayerPerfilRotular.name);
                        miPerfilDraw.drawTerreno(miLayerPerfilRotular.name);
                        miPerfilDraw.drawEje(miLayerPerfilRotular.name);
                        miPerfilDraw.drawEjeLongitudinal(miLayerPerfilRotular.name);

                        Guid guid = new Guid();



                        miPerfilDraw.drawPeralte(miLayerPerfilRotular.name);


                        #region "Calcular Estructuras"
                        //Obtengo el oEstudioCarretera
                        //oEstudioCarretera miEstudioCarretera = oFactoryEstudios.getEstudioCarretera(eEstudioTipo.ESTINF, guid);
                        oEstudioCarretera miEstudioCarretera = new oEstudioInformativoCarretera(oSingletonProyecto.getInstance.secRoadTipo,
                                                           oSingletonProyecto.getInstance.seccionCalzadaId,
                                                           getObjCoeMinAlturasMaximas(), oSingletonProyecto.getInstance.SeccionesVinculadas);

                        Polyline miLwEjeTrazado = eje;
                        Polyline miLwEjePerfilRasante = miPerfilDraw.getPolylineEjeAlzado();
                        double miSeccionIntervalo = oSingletonProyecto.getInstance.seccionIntervalo;

                        oPerfilSeccionesInfo miPerfilSeccionesInfo = new oPerfilSeccionesInfo(miEjeTrazadoTadil, miAlzado, miEstudioCarretera,
                                                                                 oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);

                        List<oEstructura> miLstPerfilEstructurasInfo = miPerfilSeccionesInfo.getPerfilSeccionesInfo(miSeccionIntervalo);

                        #endregion

                        miPerfilDraw.drawEstructuras(miLayerPerfilRotular.name, miLstPerfilEstructurasInfo);


                        Point3d miPtoIn = (Point3d)engCadNet.oTools.getPointCad(strUserCad.uiSelectPtoInsertSecciones);

                        //Creo el Objeto Seccion Completa GIS
                        oSeccionRoadCompletaSinGis miSeccionCompletaSinGis = oSingletonProyecto.getInstance.seccionCalzadaCompleta;







                        //miFilaSolucion.EjeTrazado_Amilia = memory_json_miEjeTrazadoTadil;
                        /*
                         * Guardar EjeTrazado + XData en AutoCAD.
                         * REQUIERE DocumentLock porque setXdata y StoreObjectInExtensionDictionary
                         * escriben en la base de datos de AutoCAD.
                         */
                        using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                        {
                            using (oSolucion miSolucion = new oSolucion(mIdSolucionObraLineal))
                            {
                                using (Transaction tr = oCadManager.StartTransaction())
                                {
                                    /*oDalTbSolucion.Guardar_EjeAmilia(miSolucion.idSolucion, json_miEjeTrazadoTadil);
                                    oDalTbSolucion.Guardar_Alzado(miSolucion.idSolucion, json_miAlzado);
                                    oDalTbSolucion.Guardar_Perfil(miSolucion.idSolucion, json_miPerfilDraw);*/
                                    /*MemoryStream miEjeMem = miEjeTrazadoTadil.guardarEjeTrazado();
                                    oXdata.setXdata(eje.ObjectId, "tadilEje", miSolucion.idSolucion.ToString());
                                    ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", eje, tr, miEjeMem, miEjeTrazadoTadil.GetType().FullName);

                                    //Añado el Eje de Trazado al fichero.
                                    oDalTbSolucion.addEjeTrazado(miSolucion.idSolucion, eje.Handle.ToString(), miEjeTrazadoTadil.Length / 1000);*/
                                }
                            }

                            var mObraLineal = new oObraLineal(mIdSolucionObraLineal,
                                                     datossolucion.solucionPrimariaNombre,
                                                     miEjeTrazadoTadil,
                                                     miAlzado,
                                                     (oEstudioInformativoCarretera)miEstudioCarretera,
                                                     oSingletonProyecto.getInstance.seccionCalzadaCompleta);

                            oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiCreandoSecciones);
                            mObraLineal.crearSecciones(oSingletonProyecto.getInstance.seccionIntervalo);

                            oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoSecciones);
                            mObraLineal.dibujarSecciones(miPtoIn);

                            oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoPlanta);
                            mObraLineal.createObraLinealPlantaSinGuardar();

                            oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoMediciones);
                            mObraLineal.createMediciones();

                            oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGuardandoSecciones);
                            mObraLineal.saveSecciones();
                            miFilaSolucion.isCompleteObraLineal = true;
                        }





                        //Dibujo el Eje Basico Polilinea 3D
                        //  miLwEjeBasico3D = miEjeBasicoPrimario.drawEjeBasico3D(datossolucion.solucionPrimariaNombre);

                        //Dibujo el Eje Basico en Planta Polilinea Z=0
                        // miLwEjeBasico2D = miEjeBasicoPrimario.drawEjeBasicoPlanta(miLwEjeBasico3D, this.solucionPrimariaNombre);

                        //Añado el Xdata al Eje 2D
                        /* using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                         {
                             oTadilXdata.addXdataRoadDesign(eje, datossolucion.roadDesign);
                         }
                        */
                        //Guardo los Datos en la Base Datos
                        // oDalTbSolucion.addEjeBasico(datossolucion.solucionPrimariaNombre, "", eje.Handle.ToString(), datossolucion.roadDesign, datossolucion.roadPendientes, getObjCoeMinAlturasMaximas());

                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                if (trace.GetFrame(0).GetMethod().Name == "set_Layer")
                {
                    oTadil.data.UserInfo.showInfo(strError.eCapaEliminada);
                }
                else
                {
                    oTadil.data.UserInfo.showError(ex);
                }
            }
            catch (oExTramoEntronqueNoCumple ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }
        }
        private Polyline Planta(EjeTrazado trazado, string layer)
        {
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    Polyline miEje = new Polyline();
                    int index = 0;
                    foreach (var componente in trazado.getComponentes)
                    {
                        foreach (var componentPoint in componente.getComponentPoints())
                        {
                            miEje.AddVertexAt(index, new Point2d(componentPoint[0], componentPoint[1]), 0, 0, 0);
                            index++;
                        }
                    }
                    return miEje;
                    engCadNet.oLayer.addLayer(layer, 5, false);
                    miEje.Layer = layer;
                    btr.AppendEntity(miEje);
                    tr.AddNewlyCreatedDBObject(miEje, true);

                    //GESTION CAPAS
                    oTadilLayerEjeTrazadoTadilPk miLayerPk = new oTadilLayerEjeTrazadoTadilPk(layer);
                    oTadilLayerEjeTrazadoTadilPs miLayerPs = new oTadilLayerEjeTrazadoTadilPs(layer);

                    miLayerPk.deleteItems();
                    miLayerPs.deleteItems();

                    oEjeTrazadoTadilRotular miEjeTrazadoCivilRotular = new oEjeTrazadoTadilRotular();

                    miEjeTrazadoCivilRotular.rotular(trazado, miLayerPk.name, miLayerPs.name);

                    // Commit al final, una vez dibujadas todas las entidades
                    oCadManager.thisEditor.UpdateScreen();
                    tr.Commit();
                    return miEje;
                }
            }
        }

        private void cargarlandxml(oEjeBasicoSolucion datossolucion, string archivo_LandXml)
        {
            // Obtener la ruta del archivo seleccionado
            string archivoLandXML = archivo_LandXml;

            ImportResult landXmlImporter = new LandXmlImporter().Import(archivoLandXML, 2, datossolucion.roadDesign.peralte);
            TrazadoLandXml trazado = new TrazadoLandXml(archivoLandXML, datossolucion.roadDesign.peralte);
            EjeTrazado miEjeTrazadoTadil = new EjeTrazado(trazado.vertices, trazado.Trazado, 2, 2);

            var eje = Planta(miEjeTrazadoTadil, datossolucion.solucionPrimariaNombre);
            // Pedimos el punto de inserción ANTES de abrir la transacción
            // (getPointCad requiere interacción del usuario y no debe ejecutarse dentro de una transacción CAD)
            Point3d miPtoInsert = (Point3d)engCadNet.oTools.getPointCad(strUserCad.uiSelectPtoInsertPerfilLongitudinal);

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    // --- PERFIL LONGITUDINAL ---
                    // Reconstruir el Alzado original
                    Alzado miAlzado = AlzadoAmilia.ReconstruirAlzado(
                        landXmlImporter.AlzadoComponentes,
                        landXmlImporter.KVs.ToArray(),
                        velocidad: datossolucion.roadDesign.Vp,
                        intervaloSecciones: oSingletonProyecto.getInstance.seccionIntervalo,
                        eje,
                        oSingletonTerreno.getInstance.getZFromXY,
                        oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto
                    );

                    Guitarra miGuitarra = tadLayLogica.EjeLongitudinalTadil.oFactoryPerfilLongitudinalTadil.getGuitarra(miPtoInsert, miAlzado, 100, 10);

                    PerfilLongitudinalDraw miPerfilDraw = new PerfilLongitudinalDraw(
                        miGuitarra,
                        miAlzado,
                        miEjeTrazadoTadil,
                        oSingletonTerreno.getInstance.getZFromXY,
                        oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);
                    /*
                                        string capap = "prueba perfil";
                                        string capap1 = "prueba perfil - Eje";
                                        string capap2 = "prueba perfil - Terreno";
                                        string capap3 = "prueba perfil - EjeLongitudinal";
                                        engCadNet.oLayer.addLayer(capap, 4, false);
                                        engCadNet.oLayer.addLayer(capap1, 4, false);
                                        engCadNet.oLayer.addLayer(capap2, 4, false);
                                        engCadNet.oLayer.addLayer(capap3, 4, false);

                                        miPerfilDraw.drawEje(capap1);
                                        miPerfilDraw.drawTerreno(capap2);
                                        miPerfilDraw.drawEjeLongitudinal(capap3);
                                        miPerfilDraw.drawGuitarra(capap);


                                        Polyline miEjeAlzado = miPerfilDraw.getPolylineEjeAlzado();
                                        MemoryStream miEjeMem = miAlzado.guardarAlzado();
                    */
                    /*oXdata.setXdata(miEjeAlzado.ObjectId, "tadilEjeAlzado", miSolucion.idSolucion.ToString());
                    ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEjeAlzado, tr, miEjeMem, miAlzado.GetType().FullName);

                    oDalTbSolucion.addPerfilLongitudinal(iIdSolucion, miEjeAlzado.Handle.ToString());
                    */
                    /*
                                        string capap_p = "prueba perfil -peralte";
                                        engCadNet.oLayer.addLayer(capap_p, 4, false);
                                        miPerfilDraw.drawPeralte(capap_p);//mal


                                        #region "Calcular Estructuras"
                                        //Obtengo el oEstudioCarretera
                                        oEstudioCarretera miEstudioCarretera = oFactoryEstudios.getEstudioCarretera(eEstudioTipo.ESTINF, new Guid());

                                        Polyline miLwEjeTrazado = eje;
                                        Polyline miLwEjePerfilRasante = miPerfilDraw.getPolylineEjeAlzado();
                                        double miSeccionIntervalo = oSingletonProyecto.getInstance.seccionIntervalo;

                                        oPerfilSeccionesInfo miPerfilSeccionesInfo = new oPerfilSeccionesInfo(miLwEjeTrazado, miLwEjePerfilRasante, miEstudioCarretera,
                                                                                                 oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);

                                        List<oEstructura> miLstPerfilEstructurasInfo = miPerfilSeccionesInfo.getPerfilSeccionesInfo(miSeccionIntervalo);

                                        #endregion

                                        miPerfilDraw.drawEstructuras(capap_p, miLstPerfilEstructurasInfo);


                    */








                    //Gestion de Capas
                    oTadilLayerPerfilLongitudinalTadil miLayerPerfilRotular = new oTadilLayerPerfilLongitudinalTadil(datossolucion.solucionPrimariaNombre);
                    miLayerPerfilRotular.deleteItems();
                    //DRAW
                    miPerfilDraw.drawGuitarra(miLayerPerfilRotular.name);
                    miPerfilDraw.drawTerreno(miLayerPerfilRotular.name);
                    miPerfilDraw.drawEje(miLayerPerfilRotular.name);
                    miPerfilDraw.drawEjeLongitudinal(miLayerPerfilRotular.name);

                    oDalTbSolucion.addEjeBasico(datossolucion.solucionPrimariaNombre, "", eje.Handle.ToString(), datossolucion.roadDesign, datossolucion.roadPendientes, getObjCoeMinAlturasMaximas());

                    Polyline miEjeAlzado = miPerfilDraw.getPolylineEjeAlzado();
                    MemoryStream miEjeMem = miAlzado.guardarAlzado();

                    /*oXdata.setXdata(miEjeAlzado.ObjectId, "tadilEjeAlzado", miSolucion.idSolucion.ToString());
                    ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEjeAlzado, tr, miEjeMem, miAlzado.GetType().FullName);

                    oDalTbSolucion.addPerfilLongitudinal(iIdSolucion, miEjeAlzado.Handle.ToString());
                    */

                    miPerfilDraw.drawPeralte(miLayerPerfilRotular.name);


                    #region "Calcular Estructuras"
                    //Obtengo el oEstudioCarretera
                    oEstudioCarretera miEstudioCarretera = oFactoryEstudios.getEstudioCarretera(eEstudioTipo.ESTINF, new Guid());

                    Polyline miLwEjeTrazado = eje;
                    Polyline miLwEjePerfilRasante = miPerfilDraw.getPolylineEjeAlzado();
                    double miSeccionIntervalo = oSingletonProyecto.getInstance.seccionIntervalo;

                    oPerfilSeccionesInfo miPerfilSeccionesInfo = new oPerfilSeccionesInfo(miLwEjeTrazado, miLwEjePerfilRasante, miEstudioCarretera,
                                                                             oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);

                    List<oEstructura> miLstPerfilEstructurasInfo = miPerfilSeccionesInfo.getPerfilSeccionesInfo(miSeccionIntervalo);

                    #endregion

                    miPerfilDraw.drawEstructuras(miLayerPerfilRotular.name, miLstPerfilEstructurasInfo);







































                    // Commit al final, una vez dibujadas todas las entidades
                    oCadManager.thisEditor.UpdateScreen();
                    tr.Commit();
                }
            }
        }


    }
}
