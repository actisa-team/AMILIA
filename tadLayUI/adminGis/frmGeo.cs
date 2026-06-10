using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminGis
{

    using System.Threading;
    using System.Globalization;

    using engNet.eventos;
    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayLogica;
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica.datos.BaseDatos;

    
    public partial class frmGeo : Form
    {

       private object[] mItemCurrentIni;
       private BindingSource mBindMaster;
       private Guid? mIdGeo=null;


       #region "Constructor"

       public frmGeo()
        {
            InitializeComponent();
            
           
        }

       #endregion
       #region "Propiedades"
       //Es un Registro Nuevo (False estoy en edición)
        private bool isNew
        {
            get
            {
                if (mIdGeo == null)
                {
                    return true;
                }
                else
                {
                    return false;     
                }
                              
            }      
        }
        //Validacion de los datos del Formulario
        private bool isValidoFrm
        {
            get
            {
                bool miValidoGrupo = oValidar.isValidoGrupoByFrm(this);


                //Caso Zona Prohibir Paso
                if (ucProhibirPaso.valor)
                {
                    return miValidoGrupo;
                }




                bool miValidoExca = isValidoExcavabilidad;
                bool miValidoTalud = isValidoTaludProteccion;
                bool miValidoAprovechamiento = true;



                if (miValidoGrupo && miValidoExca && miValidoTalud && miValidoAprovechamiento)
                {
                    return true;
                }

                if (!miValidoAprovechamiento)
                {
                    oTadil.data.UserInfo.showInfo(strFrmGisGeo.userAprovechamiento);
                }
                if (!miValidoGrupo)
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }
                if (!miValidoExca)
                {
                    oTadil.data.UserInfo.showInfo(strFrmGisGeo.uiUserExcavabilidadNot100);
                }
                if (!miValidoTalud)
                {
                    oTadil.data.UserInfo.showInfo(strFrmGisGeo.uiUserTaludProteccioNot100);
                }

                return false;

            }
    
        }
        //Validación de los datos de los Controles de usuarios de las capas
        private bool isValidoCapas
        {
            get
            {

                if (ucProhibirPaso.valor == true)
                {
                    return true;
                }
                
                
                if (ucCapasFirme.count > 0 && ucCapasArcen.count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
    
            }     
        }
        private bool isValidoExcavabilidad
        {
            get
            {
                double miConvencion = ucExcMediosConvencionalesPC.valorDouble;
                double miMartillo = ucExcMartilloPC.valorDouble;
                double miVoladura = ucExcVoladurasPC.valorDouble;
                double miFreatico = ucExcNivelFreaticoPC.valorDouble;
                double miVertedero = ucExcVertedero2FasesPC.valorDouble;


                if ((miConvencion + miMartillo + miVoladura + miFreatico + miVertedero) != 100)
                {
                    return false;
                }
                else
                {
                    return true;
                }       
            }
      
        }
        private bool isValidoTaludProteccion
        {

            get
            {

                double miSin = ucTaludSinProteccionPC.valorDouble;
                double miFle = ucTaludProteccionFlexiblePC.valorDouble;
                double miRig = ucTaludProteccionRigidaPC.valorDouble;


                if (miSin + miFle + miRig != 100)
                {
                    return false;
                }
                else
                {
                    return true;
                }
       
            }
       
        }
        private bool isValidoAprovechamiento
        {
            get
            {
                double miAproCapaGranular = ucGranularAproPC.textbox.valorDouble;
                double miAproCapaAsiento = ucAsientoAproPC.textbox.valorDouble;
                double miAproCapaTerraplen = ucRellenoAproPC.textbox.valorDouble;

                if (miAproCapaGranular > 0)
                {
                    ucGranularAproMat.isObligatorio = true;
                }
                else
                {
                   ucGranularAproMat.isObligatorio = false;
                   ucGranularAproMat.uiCombo.SelectedIndex = -1;
                }
 
                if (miAproCapaAsiento > 0)
                {
                    ucAsientoAproMat.isObligatorio = true;
                }
                else
                {                
                  ucAsientoAproMat.isObligatorio = false;
                  ucAsientoAproMat.uiCombo.SelectedIndex = -1;
                }

                if (miAproCapaTerraplen > 0)
                {
                    ucRellenoAproMat.isObligatorio = true;
                }
                else
                {                
                   ucRellenoAproMat.isObligatorio = false;
                   ucRellenoAproMat.uiCombo.SelectedIndex = -1;
                }


                //CASO 1 (20-40-60) ; (10-0-10) ; (10-20-0)
                if (miAproCapaGranular > 0 &&  miAproCapaAsiento >=0 && miAproCapaTerraplen >=0)
                {
                    if (miAproCapaTerraplen >= miAproCapaAsiento && miAproCapaAsiento >= miAproCapaGranular)
                    {
                        return true;
                    }
                    else
                    {
                        return false;                   
                    }
                }

                //CASO 2 (0-10-20) ; (0-10-0)
                else if (miAproCapaGranular == 0 && miAproCapaAsiento > 0 && miAproCapaTerraplen >= 0)
                {
                    if ((miAproCapaTerraplen >= miAproCapaAsiento))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }                           
                }
                
                //CASO 3 (0-0-30)
                else if (miAproCapaGranular == 0 && miAproCapaAsiento == 0 && miAproCapaTerraplen > 0)
                {
                    return true;
                }
                //Caso 4  (0-0-0)
                else if (miAproCapaGranular == 0 && miAproCapaAsiento == 0 && miAproCapaTerraplen == 0)
                {

                    return false;
                }

                else
                {
                    throw new NotImplementedException("Caso No Implementado");
                }
       
            }   
        
        }


        #endregion
       #region "Metodos Publicos"
        /// <summary>
        /// ZONA NUEVA
        /// </summary>
        public void create()
        {
            mIdGeo = null;     
        }
        /// <summary>
        /// ZONA EDITAR
        /// </summary>
        public void edit(Guid iIdGeo)
        {
            mIdGeo = iIdGeo;
        }
        #endregion
       #region "Metodos Privados"



        private void traduccion()
        {

            //name
            var name = oTadil.KAppHeaderName;

            this.Text = name;

            //Lbl
            lblFrm.Text = strFrmGisGeo.uiLblFrm;

            //Guardar & Cancelar
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;

            //Tab
            tabDatos.Text = strFrmGisGeo.uiTabDatos;
            tabDesmonte.Text = strFrmGisGeo.uiTabDesmonte;
            tabTerraplen.Text = strFrmGisGeo.uiTabTerraplen;
            tabExcTalud.Text = strFrmGisGeo.uiTabExcPro;
            tabCapaFirmes.Text = strFrmGisGeo.uiTabCapas;

            //Grupo 1
            ucWinColorZona.uiLbl = strFrmGisGeo.uiColor;
            ucGrupoLitologico.uiLbl = strFrmGisGeo.uiGrupoLitologico;
            ucGrupoGeologico.uiLbl = strFrmGisGeo.uiGrupoGeologico;
            ucProhibirPaso.uiLbl = strFrmGisGeneral.uiProhibirPasoRiesgosGeotecnicos;
            ucCoeficienteEsponjamiento.uiLbl = strFrmGisGeo.uiCoeficienteEsponjamiento;
            ucCoeficientePasoTerraplen.uiLbl = strFrmGisGeo.uiCoeficientePasoTerraplen;
            ucPendMaxTerreno.uiLbl = strFrmGisGeo.uiPendienteMaximaTerreno;
            ucCmbDesbroceCode.uiLbl = strFrmGisGeo.uiDesbroceMaterial;
            ucDesbroceEspesor.uiLbl = strFrmGisGeo.uiDesbroceEspesor;
            ucCbr.uiLbl = strFrmGisGeo.uiCBR;
            ucCmbTns.uiLbl = strFrmGisGeo.uiTNS;

            //Grupo 2
            ucCmbExcavacionCode.uiLbl = strFrmGisGeo.uiExcavacionMaterial;
            ucRellenoAproPC.uiLbl = strFrmGisGeo.uiExcavacionTerraplenPC;
            ucRellenoAproMat.uiLbl = strFrmGisGeo.uiTerraplenMaterial;
            ucAsientoAproPC.uiLbl = strFrmGisGeo.uiCapaAsientoRendimientoPC;
            ucAsientoAproMat.uiLbl = strFrmGisGeo.uiCapaAsientoMaterial;        
            ucGranularAproPC.uiLbl = strFrmGisGeo.uiCapaGranularesRendimientoPC;
            ucGranularAproMat.uiLbl = strFrmGisGeo.uiFirmeMaterial;

            //Grupo Saneo
            chkIsSaneoDesmonte.Text = strFrmGisGeo.uiIsSaneoDesmonte;
            ucCmbSaneoDesmonteCode.uiLbl = strFrmGisGeo.uiSaneoDesmonteCode;
            ucSaneoDesmonteEspesor.uiLbl = strFrmGisGeo.uiSaneoDesmonteEspesor;

            chkIsSaneoTerraplen.Text = strFrmGisGeo.uiIsSaneoTerraplen;
            ucCmbSaneoTerraplenCode.uiLbl = strFrmGisGeo.uiSaneoTerraplenCode;
            ucSaneoTerraplenEspesor.uiLbl = strFrmGisGeo.uiSaneoTerraplenEspesor;
            ucSaneoTerraplenEscalonAltura.uiLbl = strFrmGisGeo.uiSaneoTerraplenAlturaEscalon;
            ucSaneoTerraplenPendMaxSinEscalon.uiLbl = strFrmGisGeo.uiSaneoTerraplenPendMaxSinEscalon;


            //Pestaña Excavabilidad y Prtecciones Talud

            grExcavabilidad.Text = strFrmGisGeo.uiExcavabilidad;
            grTaludProteccion.Text = strFrmGisGeo.uiTaludProtecciones;

            ucExcMediosConvencionalesPC.uiLbl = strFrmGisGeo.uiExcaMediosConvencionales + " [%]";
            ucExcMartilloPC.uiLbl = strFrmGisGeo.uiExcaMartillo + " [%]";
            ucExcVoladurasPC.uiLbl = strFrmGisGeo.uiExcaVoladuras + " [%]";
            ucExcNivelFreaticoPC.uiLbl = strFrmGisGeo.uiExcaNivelFreatico + " [%]";
            ucExcVertedero2FasesPC.uiLbl = strFrmGisGeo.uiExcaVertedero2Fases + " [%]";

            ucTaludSinProteccionPC.uiLbl = strFrmGisGeo.uiTaludProtecSin + " [%]";
            ucTaludProteccionFlexiblePC.uiLbl = strFrmGisGeo.uiTaludProtecFlexibles + " [%]"; 
            ucTaludProteccionRigidaPC.uiLbl = strFrmGisGeo.uiTaludProtecRigidas + " [%]"; 


            //Terraplen-desmonte

            ucTerraplenHmaxEje.uiLbl = strFrmGisGeo.uiTaludAlturaMaxEje;
            ucTerraplenT.uiLbl = strFrmGisGeo.uiTalud;
            ucTerraplenRellenoMat.uiLbl = strFrmGisGeo.uiTerraplenMaterial;
            chkTaludTerraplenConstante.Text = strFrmGisGeo.uiTaludConstante;
            chkTaludTerraplenMuroIs.Text = strFrmGisGeo.uiTaludTerMuro;
            chkTaludTerraplenBermaIs.Text = strFrmGisGeo.uiTaludBermas;
            ucTerraplenMuroCode.uiLbl = strFrmGisGeo.uiMaterialMuro;

            ucDesmonteHmaxEje.uiLbl = strFrmGisGeo.uiTaludAlturaMaxEje;
            ucDesmonteT.uiLbl = strFrmGisGeo.uiTalud;
            chkTaludDesmonteConstante.Text = strFrmGisGeo.uiTaludConstante;
            chkTaludDesmonteMuroIs.Text = strFrmGisGeo.uiTaludDesMur;
            chkTaludDesmonteBermaIs.Text = strFrmGisGeo.uiTaludBermas;


            ucTerraplenMuroHmax.uiLbl = strFrmGisGeo.uiTerraplenMuroHmax;
            ucTerraplenMuroEspesor.uiLbl = strFrmGisGeo.uiMuroEspesor;
            ucTerraplenMuroEmpotramiento.uiLbl = strFrmGisGeo.uiMuroEmpotramiento;
            ucTerraplenMuroCode.uiLbl = strFrmGisGeo.uiMaterialMuro;
            ucTerraplenBermasX.uiLbl = strFrmGisGeo.uiBermaX;
            ucTerraplenBermasY.uiLbl = strFrmGisGeo.uiBermaY;

            ucDesmonteMuroHmax.uiLbl = strFrmGisGeo.uiDesmonteMuroHmax;
            ucDesmonteMuroEspesor.uiLbl = strFrmGisGeo.uiMuroEspesor;
            ucDesmonteMuroEmpotramiento.uiLbl = strFrmGisGeo.uiMuroEmpotramiento;
            ucDesmonteMuroCode.uiLbl = strFrmGisGeo.uiMaterialMuro;

            ucDesmonteBermaX.uiLbl = strFrmGisGeo.uiBermaX;
            ucDesmonteBermaY.uiLbl = strFrmGisGeo.uiBermaY;

            rdbDesmonteBermaArrancarPie.Text = strFrmGisGeo.uiBermaPie;




            //Capas
            ucCmbCapaBermas.uiLbl = strFrmGisGeo.uiMaterial;

            grCapaBerma.Text = strFrmGisGeo.uiCapasBerma;
            grFirmeCopy.Text = strFrmGisGeo.uiCopiarFirmeArcen;

            btnFirmeCopy.Text = strFrmGisGeo.uiBtnCopiarFirmeViario;


        }
        private void bindCreate()
        {

                mBindMaster = new BindingSource();
                mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbGeo.TableName;
                mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset;

                if (isNew)
                {
                    mBindMaster.AddNew();
                }
                else
                {
                    string miQuery = "id = '{0}'";
                    mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mIdGeo));
                }

                mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


                //Tabla Zona Geologica
                dsBd.tbGeoDataTable miTbGeo = oSingletonDsBd.getInstance.dataset.tbGeo;

                //Habia Problemas con el formateo de los numeros en los databind  1.25 --> 125
                //Con la Culture Info
                CultureInfo cultInfo = new CultureInfo("en-GB", true);


                //!!!!LOS VALORES POR DEFECTO NO LOS CARGA (LOS DEJA EN NULO!!!!!!


                //Grupo1
                ucGrupoLitologico.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.nombreColumn.ColumnName);

                ucGrupoGeologico.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.grupogeologicoColumn.ColumnName);

                ucProhibirPaso.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.prohibirPasoColumn.ColumnName);
                ucWinColorZona.panel.DataBindings.Add("colorInt", mBindMaster, miTbGeo.colorColumn.ColumnName, false, DataSourceUpdateMode.OnPropertyChanged);


                //Grupo2
                ucCoeficienteEsponjamiento.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.coeEsponjamientoColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucCoeficientePasoTerraplen.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.coePasoTerraplenColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucPendMaxTerreno.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.penMaxTerrenoColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 5, "0.00", cultInfo);
                ucCmbDesbroceCode.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.desbroceMatColumn.ColumnName);
                ucDesbroceEspesor.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.desbroceEspesorColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 5, "0.00", cultInfo);
                ucCbr.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.cbrColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 50, "0.00", cultInfo);
                ucCmbTns.uiCombo.DataBindings.Add("Text", mBindMaster, miTbGeo.tnsColumn.ColumnName);


                //Grupo Excavacion
                ucCmbExcavacionCode.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.excavacionMatColumn.ColumnName);

                ucGranularAproPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.granularAproPcColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 33, "0.00", cultInfo);
                ucGranularAproMat.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.granularAproMatColumn.ColumnName);

                ucAsientoAproPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.asientoAproPcColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 33, "0.00", cultInfo);
                ucAsientoAproMat.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.asientoAproMatColumn.ColumnName);

                ucRellenoAproPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.rellenoAproPcColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 10, "0.00", cultInfo);
                ucRellenoAproMat.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.rellenoAproMatColumn.ColumnName);


                //Grupo 3 Saneo Terraplen
                chkIsSaneoTerraplen.DataBindings.Add("Checked", mBindMaster, miTbGeo.isSaneoTerraplenColumn.ColumnName);
                grSaneoTerraplen.DataBindings.Add("Enabled", mBindMaster, miTbGeo.isSaneoTerraplenColumn.ColumnName);
                ucCmbSaneoTerraplenCode.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.saneoTerraplenMatColumn.ColumnName);
                ucSaneoTerraplenEspesor.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.saneoTerraplenEspesorColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucSaneoTerraplenPendMaxSinEscalon.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.saneoTerraplenPendMaxSinEscalonColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucSaneoTerraplenEscalonAltura.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.saneoTerraplenEscalonAlturaColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);


                //Grupo 4 Saneo Desmonte
                chkIsSaneoDesmonte.DataBindings.Add("Checked", mBindMaster, miTbGeo.isSaneoDesmonteColumn.ColumnName);
                grSaneoDesmonte.DataBindings.Add("Enabled", mBindMaster, miTbGeo.isSaneoDesmonteColumn.ColumnName);
                ucCmbSaneoDesmonteCode.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.saneoDesmonteMatColumn.ColumnName);
                ucSaneoDesmonteEspesor.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.saneoDesmonteEspesorColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);


                //Grupo Talud Desmonte
                ucDesmonteHmaxEje.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.desmonteAlturaMaximaColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucDesmonteT.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.desmonteTaludColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);

                chkTaludDesmonteConstante.DataBindings.Add("Checked", mBindMaster, miTbGeo.desmonteConstanteIsColumn.ColumnName);
                chkTaludDesmonteMuroIs.DataBindings.Add("Checked", mBindMaster, miTbGeo.desmonteMuroIsColumn.ColumnName);
                chkTaludDesmonteBermaIs.DataBindings.Add("Checked", mBindMaster, miTbGeo.desmonteBermaIsColumn.ColumnName);

                grTaludDesmonteC1.DataBindings.Add("Enabled", mBindMaster, miTbGeo.desmonteConstanteIsColumn.ColumnName);
                grTaludDesmonteC2.DataBindings.Add("Enabled", mBindMaster, miTbGeo.desmonteMuroIsColumn.ColumnName);
                grTaludDesmonteC3.DataBindings.Add("Enabled", mBindMaster, miTbGeo.desmonteBermaIsColumn.ColumnName);


                ucDesmonteMuroHmax.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.desmonteMuroAlturaMaximaColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucDesmonteMuroEspesor.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.desmonteMuroEspesorColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucDesmonteMuroEmpotramiento.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.desmonteMuroEmpotramientoColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                
                ucDesmonteMuroCode.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.desmonteMuroMatColumn.ColumnName);

  

                ucDesmonteBermaX.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.desmonteBermaLonHorColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucDesmonteBermaY.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.desmonteBermaLonVerColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);

                rdbDesmonteBermaArrancarPie.DataBindings.Add("Checked", mBindMaster, miTbGeo.desmonteBermaIniciarPieColumn.ColumnName);


                //Grupo Talud Terraplen
                ucTerraplenHmaxEje.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.terraplenAlturaMaximaColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucTerraplenT.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.terraplenTaludColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucTerraplenRellenoMat.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.terraplenRellenoMatColumn.ColumnName);


                chkTaludTerraplenConstante.DataBindings.Add("Checked", mBindMaster, miTbGeo.terraplenConstanteIsColumn.ColumnName);
                chkTaludTerraplenMuroIs.DataBindings.Add("Checked", mBindMaster, miTbGeo.terraplenMuroIsColumn.ColumnName);
                chkTaludTerraplenBermaIs.DataBindings.Add("Checked", mBindMaster, miTbGeo.terraplenBermaIsColumn.ColumnName);

                grTaludTerraplenC1.DataBindings.Add("Enabled", mBindMaster, miTbGeo.terraplenConstanteIsColumn.ColumnName);
                grTaludTerraplenC2.DataBindings.Add("Enabled", mBindMaster, miTbGeo.terraplenMuroIsColumn.ColumnName);
                grTaludTerraplenC3.DataBindings.Add("Enabled", mBindMaster, miTbGeo.terraplenBermaIsColumn.ColumnName);


                ucTerraplenMuroHmax.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.terraplenMuroAlturaMaxColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucTerraplenMuroEspesor.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.terraplenMuroEspesorColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucTerraplenMuroEmpotramiento.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.terraplenMuroEmpotramientoColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucTerraplenMuroCode.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.terraplenMuroMatColumn.ColumnName);
                
                
                ucTerraplenBermasX.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.terraplenBermaLonHorColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);
                ucTerraplenBermasY.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.terraplenBermaLonVerColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0.00", cultInfo);


                //Grupo Excavabilidad
                ucExcMediosConvencionalesPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.excavaMediosConvencionalesPCColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0", cultInfo);
                ucExcMartilloPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.excavaMartilloPCColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0", cultInfo);
                ucExcVoladurasPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.excavaVoladurasPCColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0", cultInfo);
                ucExcNivelFreaticoPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.excavaNivelFreaticoPCColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0", cultInfo);
                ucExcVertedero2FasesPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.excavaVertedero2FasesPCColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0", cultInfo);

                //Talud
                ucTaludSinProteccionPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.taludProteccionSinPCColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0", cultInfo);
                ucTaludProteccionFlexiblePC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.taludProteccionFlexiblePCColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0", cultInfo);
                ucTaludProteccionRigidaPC.textbox.DataBindings.Add("Text", mBindMaster, miTbGeo.taludProteccionRigidasPCColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 1, "0", cultInfo);


                //MaterialBerma
                ucCmbCapaBermas.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTbGeo.bermaMatColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);

            


        }
        private void bindUpdate()
        {
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset;
                // string miQuery = "idGeo = '{0}'";
                // mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mIdGeo));
                 mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;

        }
        private void populateCapas()
        {
            if (mIdGeo == null)
            {
                oTadil.data.UserInfo.showError(new Exception(strFrmGisGeo.uiCapasIdNull));
            }
            else
            {
                ucCapasFirme.iniciar(mIdGeo.Value);
                ucCapasAsiento.iniciar(mIdGeo.Value);
                ucCapasArcen.iniciar(mIdGeo.Value);
            }
        }

        private void grDataEnable(bool iEstado)
        {
            grData2.Enabled = iEstado;
            grData3.Enabled = iEstado;


            grTaludDesmonte.Enabled = iEstado;
            grTaludTerraplen.Enabled = iEstado;

            grExcavabilidad.Enabled = iEstado;
            grTaludProteccion.Enabled = iEstado;

            grCapaBerma.Enabled = iEstado;
   
        }


       

        #endregion
       #region "Botones"
       /// <summary>
       /// BOTON GUARDAR
       /// </summary>
        private void lnkClaSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                if (isValidoFrm)
                {
                    if (isNew)
                    {
                        mIdGeo = System.Guid.NewGuid();
                        ((DataRowView)mBindMaster.Current)["id"] = mIdGeo;
                    }

                    mBindMaster.EndEdit();


                    mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


                    oSingletonDsBd.getInstance.save(true);
                    oSingletonDsBd.getInstance.Dispose();

                    //((DataSet)mBindMaster.DataSource).WriteXml(oTadil.data.Files.fileBbdd);

                    //oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);

                    bindUpdate();
               
                    populateCapas();
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

        }
        /// <summary>
        /// BOTON SALIR
        /// </summary>
        private void lnkSalir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }


        }
        /// <summary>
        /// BOTON COPIAR FIRME ARCEN
        /// </summary>
        private void btnFirmeCopy_Click(object sender, EventArgs e)
        {

            try
            {
                //Validaciones
                if (ucCapasFirme.count == 0)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowEmpty);
                    return;
                }
                else
                {
                    ucCapasArcen.copyFrom("FIR");
                }
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        #endregion
       #region "Eventos"

        private void frmGeo_Load(object sender, EventArgs e)
        {

            try
            {

                this.tabControl1.TabPages[1].Show();
                this.tabControl1.TabPages[1].Hide();
                this.tabControl1.TabPages[2].Show();
                this.tabControl1.TabPages[2].Hide();
                this.tabControl1.TabPages[3].Show();
                this.tabControl1.TabPages[3].Hide();
                this.tabControl1.TabPages[4].Show();
                this.tabControl1.TabPages[4].Hide();
                this.tabControl1.TabPages[0].Show();


                ucCapasFirme.evIsDataSetUpDate += new EventHandler<oEventArgs<bool>>(ucCapas_evIsDataSetUpDate);
                ucCapasAsiento.evIsDataSetUpDate += new EventHandler<oEventArgs<bool>>(ucCapas_evIsDataSetUpDate);
                ucCapasArcen.evIsDataSetUpDate += new EventHandler<oEventArgs<bool>>(ucCapas_evIsDataSetUpDate);

                ucProhibirPaso.uiCombo.SelectedIndexChanged += new EventHandler(uiComboProhibirPaso_SelectedIndexChanged);
              
                ucCmbDesbroceCode.populate();
                ucCmbExcavacionCode.populate();
                ucRellenoAproMat.populate();
                ucAsientoAproMat.populate();
                ucCmbCapaBermas.populate();
                ucGranularAproMat.populate();
                ucCmbSaneoDesmonteCode.populate();
                ucCmbSaneoTerraplenCode.populate();
                ucTerraplenRellenoMat.populate();

                ucDesmonteMuroCode.populate();
                ucTerraplenMuroCode.populate();

                bindCreate();

                traduccion();


                if (!isNew) { populateCapas(); }

            }
            catch (Exception ex )
            {

                throw ex;
            }
            


        }



        private void frmGeo_FormClosing(object sender, FormClosingEventArgs e)
        {

            bool miIsFrmValido = isValidoFrm;


            //Formulario Valido + Capas
            if (miIsFrmValido && isValidoCapas)
            {
                mBindMaster.EndEdit();

                var itemInicial = mItemCurrentIni;
                var itemFinal = ((DataRowView)mBindMaster.Current).Row.ItemArray;

                if (! itemInicial.SequenceEqual(itemFinal))
                {

                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosNoGuardados);

                    DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiDatosGuardar);

                    if (miResul == DialogResult.Yes)
                    {
                        lnkClaSave_LinkClicked(lnkSave, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiSalirDatosSinGuardar);

                    }

                }


            }

            //Formulario Valido ; Capas No Valido (No Permito Salir)
            else if (miIsFrmValido && !isValidoCapas)
            {
                oTadil.data.UserInfo.showInfo(strFrmGisGeo.uiCapasCountCero);
                e.Cancel = true;
            }

            //Formulario No Valido + Capas NO Valido (Permito Salir)
            else
            {
                DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiSalirPregunta);

                if (miResul != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.Dispose();
                    this.Close();
                }

            }



        }


        private void chkTaludDesmonteConstante_CheckedChanged(object sender, EventArgs e)
        {
            grTaludDesmonteC1.Enabled = ((RadioButton)sender).Checked;         
        }
        private void chkTaludDesmonteMuroIs_CheckedChanged(object sender, EventArgs e)
        {
            grTaludDesmonteC2.Enabled = ((RadioButton)sender).Checked;            
        }
        private void chkTaludDesmonteBermaIs_CheckedChanged(object sender, EventArgs e)
        {
            grTaludDesmonteC3.Enabled = ((RadioButton)sender).Checked;    
        }
        private void chkTaludTerraplenConstante_CheckedChanged(object sender, EventArgs e)
        {
            grTaludTerraplenC1.Enabled = ((RadioButton)sender).Checked;      
        }
        private void chkTaludTerraplenMuroIs_CheckedChanged(object sender, EventArgs e)
        {
            grTaludTerraplenC2.Enabled = ((RadioButton)sender).Checked; 
        }
        private void chkTaludTerraplenBermaIs_CheckedChanged(object sender, EventArgs e)
        {
            grTaludTerraplenC3.Enabled = ((RadioButton)sender).Checked; 
        }

        private void chkIsSaneoTerraplen_CheckedChanged(object sender, EventArgs e)
        {
            grSaneoTerraplen.Enabled = ((CheckBox)sender).Checked;
        }

        private void chkIsSaneoDesmonte_CheckedChanged(object sender, EventArgs e)
        {
            grSaneoDesmonte.Enabled = ((CheckBox)sender).Checked;
        }



        //Evento Salta al Modificar el DataSet los Controles de Capas
        void ucCapas_evIsDataSetUpDate(object sender, oEventArgs<bool> e)
        {
            if (e.Value == true)
            {
                bindUpdate();
            }
        }
        void uiComboProhibirPaso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ucProhibirPaso.valor == true)
            {
                grDataEnable(false);
            }
            else
            {
                grDataEnable(true);
            }
           
        }
   

        #endregion










      

 



















    }
}
