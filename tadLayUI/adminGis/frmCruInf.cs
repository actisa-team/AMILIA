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

    using tadLayLan.Tdb;
    using tadLayLan;
    using tadLayData;
    using tadLayLogica.zonaGis;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica;
using engNet.Extension.String;
    
    
    /// <summary>
    /// CRUCE INFRAESTRUCTURAS 
    /// </summary>
    public partial class frmCruInf : Form
    {


        private Guid? mId = null;
        private object[] mItemCurrentIni;
        private BindingSource mBindMaster;


        public frmCruInf()
        {
            InitializeComponent();
            postConstructor();
        }




        #region "Metodos Publicos"
        public void create()
        {
            mId = null;
            bindCreate();
        }
        public void edit(Guid iId)
        {
            mId = iId;
            bindCreate();
        }
        #endregion



        #region "Metodos Privados"

        private void postConstructor()
        {

            ucProhibirPaso.uiCombo.SelectedIndexChanged += new EventHandler(uiComboProhibirPaso_SelectedIndexChanged);
            //checkConfAngulo.CheckedChanged += new EventHandler(checkConfAngulo_CheckedChanged);
            
            ucValoracion.populate();
            traduccion();      
        }

        private void traduccion()
        {

            //FRM

            var name = oTadil.KAppHeaderName;

            this.Text = name;
            lblHeader.Text = strFrmGisGeneral.uiCRUINF;

            //Grupo Zona
            grGeneral.Text = strFrmGisGeneral.uiDatosZona;
            ucNombre.uiLbl = strFrmGisGeneral.uiNombre;
            ucProhibirPaso.uiLbl = strFrmGisGeneral.uiProhibirPaso;
            ucDescripcion.uiLbl = strFrmGisGeneral.uiDescripcion;

            //Grupo Estructura
            grDetalle.Text = strFrmGisGeneral.uiDatos;
            ucGalibo.uiLbl = strFrmGisGeneral.uiGalibo;
            ucValoracion.uiLbl = strFrmGisGeneral.uiValoracion;

            ucPasoNivelExigir.isObligatorio = false;
            //ucAnguloCruceMax.uiLbl = strFrmGisGeneral.uiAnguloCruceMaximo;

            //Grupo Configurar Angulo
            //grConfAng.Text = strFrmGisGeneral.uiConfiguarcionAngulo;
            //checkConfAngulo.Text = strFrmGisGeneral.uiCongifurarAngulo;

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;

        }



        private bool isValidoFrm ()
        {

                bool miValidoGrupo = oValidar.isValidoGrupoByFrm(this);
              
                return miValidoGrupo;
               
        }
        private void bindCreate()
        {


            
                mBindMaster = new BindingSource();
                mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbCruceInfra.TableName;
                mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbCruceInfra;

                if (mId ==null)
                {
                    mBindMaster.AddNew();
                }
                else
                {
                    string miQuery = "id = '{0}'";
                    mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mId));
                }

                mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


                //Tabla Zona Geologica
                dsBd.tbCruceInfraDataTable miTb = oSingletonDsBd.getInstance.dataset.tbCruceInfra;

                //Habia Problemas con el formateo de los numeros en los databind  1.25 --> 125
                //Con la Culture Info
                CultureInfo cultInfo = new CultureInfo("en-GB", true);


                //!!!!LOS VALORES POR DEFECTO NO LOS CARGA (LOS DEJA EN NULO!!!!!!


                //GrupoZona
                ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
                ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);
                ucProhibirPaso.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.prohibirPasoColumn.ColumnName);
                ucPasoNivelExigir.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.pasoNivelExigirColumn.ColumnName);
                         
               //GrupoDetalle
                ucGalibo.textbox.DataBindings.Add("Text", mBindMaster, miTb.galiboColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged,null, "0.0", cultInfo);
                ucValoracion.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.valoracionColumn.ColumnName);

                ////Grupo configuracion angulo
                //ucAnguloCruceMax.textbox.DataBindings.Add("Text", mBindMaster, miTb.anguloCruceMaxColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 0, "0", cultInfo);
                //checkConfAngulo.DataBindings.Add("Checked", mBindMaster, miTb.isAngCruceMaxEnabledColumn.ColumnName);
                //checkConfAngulo.DataBindings[0].Format += new ConvertEventHandler(frmCruInf_Format);

             
        }
        private void grDataEnable(bool iEstado)
        {
            grDetalle.Enabled = iEstado;  
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
                   if (oExtensionString.isNombreSolucionValido(this.ucNombre.textbox.Text))
                   {
                       if (ucGalibo.textbox.Text == string.Empty)
                       {
                           ucPasoNivelExigir.uiCombo.SelectedIndex = -1;
                       }
                       else if (ucGalibo.textbox.Text == "0.0")
                       {
                           ucPasoNivelExigir.uiCombo.SelectedIndex = 1;
                       }
                       else
                       {
                           ucPasoNivelExigir.uiCombo.SelectedIndex = 0;
                       }
                       if (mId == null)
                       {
                           mId = System.Guid.NewGuid();
                           ((DataRowView)mBindMaster.Current)["id"] = mId;
                       }

                       mBindMaster.EndEdit();

                       mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;

                       oSingletonDsBd.getInstance.save(true);

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

       



       #endregion

       #region "Eventos"


       //void checkConfAngulo_CheckedChanged(object sender, EventArgs e)
       //{
       //    bool isChecked = checkConfAngulo.Checked;
       //    if (isChecked)
       //    {
       //        grConfAng.Enabled = true;
       //    }
       //    else
       //    {
       //        grConfAng.Enabled = false;
       //    }
       //}



       //void frmCruInf_Format(object sender, ConvertEventArgs e)
       //{
       //    if (e.Value == System.DBNull.Value)
       //        e.Value = false;
       //}

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
                   mBindMaster.EndEdit();
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

               ucPasoNivelExigir.uiCombo.SelectedIndex = -1;
               ucGalibo.textbox.Text = string.Empty;
               ucValoracion.uiCombo.SelectedIndex = -1;

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
