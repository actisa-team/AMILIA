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
    using System.IO;
    using System.Globalization;

    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayData;
    using tadLayLogica.zonaGis;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica;
    using tadLayLogica.Tools;
using engNet.Extension.String;
    using tadLayLogica.datos;
    
    
    public partial class frmTun : Form
    {


        private Guid? mIdEst = null;
        private object[] mItemCurrentIni;
        private BindingSource mBindMaster;
        private oSeccionTunelDwg mSeccionesTunelesDwg = null;
        
        
        public frmTun()
        {
            InitializeComponent();
            post_Constructor();
        }




        #region "Metodos Publicos"

        public void create()
        {
            mIdEst = null;
            bindCreate();
        }

        public void edit(Guid iIdEst)
        {
            mIdEst = iIdEst;
            bindCreate();
        }



        #endregion



        #region "Metodos Privados"

        private void post_Constructor()
        {

            //ucFileSelect
            ucFileSelect1.setup(oTadil.data.Files.folderCadSecTun, oTadil.data.Files.KFileDwgExtensionFiltro);
            ucFileSelect1.evFile += new EventHandler<engNet.eventos.oEventArgs<string>>(ucFileSelect1_evFile);

            //Relleno las Estructuras
            ucTunTipo.populate("TUN");

            ucTunTipo.evIsTunCircular += new EventHandler<engNet.eventos.oEventArgs<bool>>(ucTunTipo_evIsTunCircular);

            ucProhibirPaso.uiCombo.SelectedIndexChanged += new EventHandler(uiComboProhibirPaso_SelectedIndexChanged);
           
            ucTunExcTipos1.populate();
            ucTunTratamientosTipos1.populate();

          
            ucSecNombre.textbox.Enabled = false;

            //Traduccion
            traduccion();
        }
        private void traduccion()
        {

            //FRM
            var name = oTadil.KAppHeaderName;

            this.Text = name;
            lblHeader.Text = strFrmGisTun.uiHeader;

            tabDatos.Text = strFrmGisTun.uiPagDatos;
            tabRmr.Text = strFrmGisTun.uiPagRmr;

            //Grupo Zona
            grGeneral.Text = strFrmGisGeneral.uiDatosZona;
            ucNombre.uiLbl = strFrmGisGeneral.uiNombre;
            ucProhibirPaso.uiLbl = strFrmGisTun.uiProhibirTuneles;
            ucColorZona.uiLbl = strFrmGisGeneral.uiColor;
            ucDescripcion.uiLbl = strFrmGisGeneral.uiDescripcion;

            //Detalle
            grDetalle.Text = strFrmGisTun.uiDatosTunel;
            ucTunTipo.uiLblGrupo = strFrmGisGeneral.uiTipología;
            ucTunTipo.uiLblItem = strFrmGisTun.uiTunel;
            ucCircularIsDovela.uiLbl = strFrmGisTun.uiCircularConDovelas;
            ucConContraboveda.uiLbl = strFrmGisTun.uiConContraboveda;
            ucRmr.uiLbl = strFrmGisTun.uiRmr;
            ucGaliboVertical.uiLbl = strFrmGisTun.uiGaliboVertical;
            ucAncho.uiLbl = strFrmGisTun.uiAncho;
            ucSecNombre.uiLbl = strFrmGisGeneral.uiDwgSeccion;

            //Revestimiento
            grEjecucion.Text = strFrmGisTun.uiGrProcedimientosEje;
            ucTunExcTipos1.uiLbl = strFrmGisTun.uiExcavacionMetodos;
            ucTunTratamientosTipos1.uiLbl = strFrmGisTun.uiTratamientos;

            //Botón de Busqueda Sección
            this.btnBuscarSeccion.Text = strFrmGisGeneral.uiBuscarSeccion;

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;

        }

        private void grDataEnable(bool iEstado)
        {
            grDetalle.Enabled = iEstado;
            grEjecucion.Enabled = iEstado;
        }



        void ucTunTipo_evIsTunCircular(object sender, engNet.eventos.oEventArgs<bool> e)
        {

            if (e.Value == true)
            {
                ucCircularIsDovela.uiCombo.SelectedValue = false;
                ucCircularIsDovela.uiCombo.Enabled = true;
                ucConContraboveda.uiCombo.SelectedValue = false;
                ucConContraboveda.uiCombo.Enabled = false;
            }
            else
            {
                ucCircularIsDovela.uiCombo.SelectedValue = false;
                ucCircularIsDovela.uiCombo.Enabled = false;
                ucConContraboveda.uiCombo.SelectedValue = false;
                ucConContraboveda.uiCombo.Enabled = true;

            }

        }

        private bool isValidoFrm ()
        {

            bool miValidoGrupo = oValidar.isValidoGrupoByFrm(this);
              
                return miValidoGrupo;
               
        }

        private void bindCreate()
        {
                       
                mBindMaster = new BindingSource();
                mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbTun.TableName;
                mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbTun;

                if (mIdEst ==null)
                {
                    mBindMaster.AddNew();
                }
                else
                {
                    string miQuery = "id = '{0}'";
                    mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mIdEst));
                }

                mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;

                //Tabla Zona Geologica
                dsBd.tbTunDataTable miTb = oSingletonDsBd.getInstance.dataset.tbTun;

                //Habia Problemas con el formateo de los numeros en los databind  1.25 --> 125
                //Con la Culture Info
                CultureInfo cultInfo = new CultureInfo("en-GB", true);


                //!!!!LOS VALORES POR DEFECTO NO LOS CARGA (LOS DEJA EN NULO!!!!!!

                //GrupoZona
                ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
                ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);
                ucProhibirPaso.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.allowTunelesColumn.ColumnName);
                ucColorZona.panel.DataBindings.Add("colorInt", mBindMaster, miTb.colorColumn.ColumnName, false, DataSourceUpdateMode.OnPropertyChanged);

               ////GrupoDetalle
                ucTunTipo.uiComboGrupo.DataBindings.Add("Text", mBindMaster, miTb.strEstMatColumn.ColumnName);
                ucTunTipo.uiComboItem.DataBindings.Add("SelectedValue", mBindMaster, miTb.idEstMatColumn.ColumnName);
                ucCircularIsDovela.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.isConDovelasColumn.ColumnName);
                ucConContraboveda.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.isContraBovedaColumn.ColumnName);
                ucRmr.textbox.DataBindings.Add("Text", mBindMaster, miTb.rmrColumn.ColumnName);
                ucGaliboVertical.textbox.DataBindings.Add("Text", mBindMaster, miTb.galiboColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 0, "0.00", cultInfo);
                ucAncho.textbox.DataBindings.Add("Text", mBindMaster, miTb.anchoColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged,0, "0.00", cultInfo);
                ucSecNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.dwgNameColumn.ColumnName);
               
                //Ejecución
                ucTunExcTipos1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idTunExcMetColumn.ColumnName);
                ucTunTratamientosTipos1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idTunTipoTratamientoColumn.ColumnName);

           
        }
       #endregion



       #region "Botones"

       //GUARDAR
       private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {
           try
           {

               if (isValidoFrm())
               {
                   if (oExtensionString.isNombreSolucionValido(ucNombre.textbox.Text))
                   {
                       if (mIdEst == null)
                       {
                           mIdEst = System.Guid.NewGuid();

                           ((DataRowView)mBindMaster.Current)["id"] = mIdEst;
                       }

                       mBindMaster.EndEdit();

                       mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;

                       oSingletonDsBd.getInstance.save(true);

                       bindUpdate();
                   }
                   else
                   {
                       oTadil.data.UserInfo.showInfo(strGeneralUser.uiNombreConCaracteresEspeciales);
                   }
                      
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
       //SALIR
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
       /// Buscar Sección
       /// </summary>
       private void btnBuscarSeccion_Click(object sender, EventArgs e)
       {
           try
           {

               if (mSeccionesTunelesDwg == null)
               {
                   mSeccionesTunelesDwg = new oSeccionTunelDwg(oTadil.data.Files.folderCadSecTun);
               }

               string miSeccion = mSeccionesTunelesDwg.getFileNameConExtension(this.ucTunTipo.getGrupoCode(),this.ucConContraboveda.valor, this.ucRmr.valorDouble, this.ucAncho.valorDouble);

               if (string.IsNullOrEmpty(miSeccion))
               {
                   oTadil.data.UserInfo.showInfo(strError.eAsignarSeccion);
               }
               else
               {
                   this.ucSecNombre.textbox.Text = miSeccion;
               }

           }
           catch (Exception ex)
           {
               
               oTadil.data.UserInfo.showError(ex);
           }

       }



       private void bindUpdate()
       {
           oSingletonDsBd.getInstance.Dispose();

       }


       #endregion

       #region "Eventos"


       void ucFileSelect1_evFile(object sender, engNet.eventos.oEventArgs<string> e)
       {
           FileInfo miFileInfo = new FileInfo(e.Value);
           ucSecNombre.uitxt = miFileInfo.Name;
       }

       protected override void OnClosed(EventArgs e)
       {
           base.OnClosed(e);
       }


       private void frm_FormClosing(object sender, FormClosingEventArgs e)
       {

           try
           {

               if (isValidoFrm())
               {



                  // mBindMaster.EndEdit();
                   var itemInicial = mItemCurrentIni;
                   var itemFinal = ((DataRowView)mBindMaster.Current).Row.ItemArray;

                   if (! itemInicial.SequenceEqual(itemFinal))
                   {

                       oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosNoGuardados);

                       DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiDatosGuardar);

                       if (miResul == DialogResult.Yes)
                       {
                           lnkSave_LinkClicked(lnkSave, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
                       }
                       else
                       {
                           oTadil.data.UserInfo.showInfo(strGeneralUser.uiSalirDatosSinGuardar);

                       }

                   }

               }


               //Salir sin Guardar
               else
               {
                   DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiSalirPregunta);

                   if (miResul != DialogResult.Yes)
                   {
                       e.Cancel = true;
                   }
                   else
                   {
                       mBindMaster.CancelEdit();
                       this.Dispose();
                       this.Close();
                   }
               
               }
               

           }
           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }

       }




       void uiComboProhibirPaso_SelectedIndexChanged(object sender, EventArgs e)
       {

           if (ucProhibirPaso.valor == true)
           { 
               grDataEnable(false);
           }
           if (ucProhibirPaso.valor == false)
           {
               grDataEnable(true);
           }

       }


  


       #endregion


 













 

    }
}
