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
using engNet.Extension.String;
    
    
    /// <summary>
    /// DOMINIO PÚBLICO HIDRÁULICO 
    /// </summary>
    public partial class frmDoHi : Form
    {


        private Guid? mId = null;
        private object[] mItemCurrentIni;
        private BindingSource mBindMaster;
        
        
        public frmDoHi()
        {
            InitializeComponent();
            post_Constructor();
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

        private void post_Constructor()
        {

            ucProhibirPaso.uiCombo.SelectedIndexChanged += new EventHandler(uiComboProhibirPaso_SelectedIndexChanged);


            checkConfAng.CheckedChanged += new EventHandler(checkConfAng_CheckedChanged);

            
            ucValoracion.populate();
            traduccion();      
        }


        private void traduccion()
        {

            //FRM

            var name = oTadil.KAppHeaderName;

            this.Text = name;
            lblHeader.Text = strFrmGisGeneral.uiZODOPU;



            //Grupo Zona
            grGeneral.Text = strFrmGisGeneral.uiDatosZona;
            ucNombre.uiLbl = strFrmGisGeneral.uiNombre;
            ucProhibirPaso.uiLbl = strFrmGisGeneral.uiProhibirPaso;
            chBoxPasarEnEstructura.Text = strFrmGisGeneral.uiPasarEstructuraObligado;
            ucDescripcion.uiLbl = strFrmGisGeneral.uiDescripcion;
            

            //Grupo Estructura
            grDetalle.Text = strFrmGisGeneral.uiDatos;
            ucGalibo.uiLbl = strFrmGisGeneral.uiGalibo;
            ucAnguloCruceMax.uiLbl = strFrmGisGeneral.uiAnguloCruceMaximo;
            ucValoracion.uiLbl = strFrmGisGeneral.uiValoracion;
            ucIsTramoCompleto.uiLbl = strFrmGisGeneral.uiTramoCompleto;


            //Grupo Configurar Angulo
            //grConfAngulo.Text = strFrmGisGeneral.uiConfiguarcionAngulo;
            checkConfAng.Text = strFrmGisGeneral.uiCongifurarAngulo;

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;

        }


        private void grDataEnable(bool iEstado)
        {
            grDetalle.Enabled = iEstado;
        }

        private bool isValidoFrm ()
        {

            bool miValidoGrupo = oValidar.isValidoGrupoByFrm(this);
              
                return miValidoGrupo;
               
        }
        private void bindCreate()
        {
     
                mBindMaster = new BindingSource();
                mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbDoHi.TableName;
                mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbDoHi;

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
                dsBd.tbDoHiDataTable miTb = oSingletonDsBd.getInstance.dataset.tbDoHi;

                //Habia Problemas con el formateo de los numeros en los databind  1.25 --> 125
                //Con la Culture Info
                CultureInfo cultInfo = new CultureInfo("en-GB", true);


                //!!!!LOS VALORES POR DEFECTO NO LOS CARGA (LOS DEJA EN NULO!!!!!!


                //GrupoZona
                ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
                ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);
                ucProhibirPaso.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.prohibirPasoColumn.ColumnName);
                chBoxPasarEnEstructura.DataBindings.Add("Checked", mBindMaster, miTb.pasarEstructuraColumn.ColumnName);
                chBoxPasarEnEstructura.DataBindings[0].Format += new ConvertEventHandler(frmDoHi_Format);
                         
               //GrupoDetalle
                ucGalibo.textbox.DataBindings.Add("Text", mBindMaster, miTb.galiboColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 0, "0.0", cultInfo);
                ucValoracion.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.valoracionColumn.ColumnName);
                ucIsTramoCompleto.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.isTramoCompletoColumn.ColumnName);
                

                //Grupo configuracion angulo
                ucAnguloCruceMax.textbox.DataBindings.Add("Text", mBindMaster, miTb.anguloCruceMaxColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged, 0, "0", cultInfo);
                checkConfAng.DataBindings.Add("Checked", mBindMaster, miTb.isAngCruceMaxEnabledColumn.ColumnName);
                checkConfAng.DataBindings[0].Format += new ConvertEventHandler(frmDoHi_Format);

             
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


       void checkConfAng_CheckedChanged(object sender, EventArgs e)
       {
           bool isChecked = checkConfAng.Checked;
           if (isChecked)
           {
               grConfAngulo.Enabled = true;
           }
           else
           {
               grConfAngulo.Enabled = false;
           }
       }


       void frmDoHi_Format(object sender, ConvertEventArgs e)
       {
           if (e.Value == System.DBNull.Value)
               e.Value = false;
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
