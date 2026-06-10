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
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica;
    using engNet.Extension.String;
    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;
    
    
    public partial class frmCim : Form
    {


        private Guid? mId = null;
        private object[] mItemCurrentIni;
        private BindingSource mBindMaster;
        //private oDsBd mDs = null;
        
        public frmCim()
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
        private bool isValidoFrm()
        {
            bool miValidoGrupo = oValidar.isValidoGrupoByFrm(this);
            return miValidoGrupo;
        }
        private void post_Constructor()
        {
            //SetUp UserControl
            ucCimViaPu.populate("CIM");
            ucCimPasInf.populate("CIM");
            ucExcMetodos.populate("EXC");
            ucAguaPresencia.populate("AGU");

            ucProhibirPaso.uiCombo.SelectedIndexChanged += new EventHandler(uiComboProhibirPaso_SelectedIndexChanged);
         
           
            //Traduccion
            traduccion();


        }
        private void traduccion()
        {

            //FRM

            var name = oTadil.KAppHeaderName;

            this.Text = name;
            lblHeader.Text = strFrmGisCim.uiHeader;

            //Grupo Zona
            grGeneral.Text = strFrmGisGeneral.uiDatosZona;
            ucNombre.uiLbl = strFrmGisGeneral.uiNombre;
            ucProhibirPaso.uiLbl = strFrmGisGeneral.uiProhibirPaso;
            ucColorZona.uiLbl = strFrmGisGeneral.uiColor;
            ucDescripcion.uiLbl = strFrmGisGeneral.uiDescripcion;

            //Grupo Estructura
            grDetalle.Text = strFrmGisCim.uiDatosCimentacion;
            ucCimViaPu.uiLbl = strFrmGisCim.uiCimViPu;
            ucCimPasInf.uiLbl = strFrmGisCim.uiCimPaIn;
            ucExcMetodos.uiLbl = strFrmGisCim.uiExcPro;
            ucAguaPresencia.uiLbl = strFrmGisCim.uiAguPre;

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;

        }
        private void grDataEnable(bool iEstado)
        {
            grDetalle.Enabled = iEstado;
 
        }
        private void bindCreate()
        {
                mBindMaster = new BindingSource();
                mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbCim.TableName;
                mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbCim;

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
                dsBd.tbCimDataTable miTb = oSingletonDsBd.getInstance.dataset.tbCim;

                //Habia Problemas con el formateo de los numeros en los databind  1.25 --> 125
                //Con la Culture Info
                CultureInfo cultInfo = new CultureInfo("en-GB", true);


                //!!!!LOS VALORES POR DEFECTO NO LOS CARGA (LOS DEJA EN NULO!!!!!!

       

                //GrupoZona
                ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
                ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);
                ucProhibirPaso.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.prohibirPasoColumn.ColumnName);
                ucColorZona.panel.DataBindings.Add("colorInt", mBindMaster, miTb.colorColumn.ColumnName, false, DataSourceUpdateMode.OnPropertyChanged);

               //GrupoDetalle
                ucCimViaPu.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCimViaPueColumn.ColumnName);
                ucCimPasInf.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCimPasInfColumn.ColumnName);
                ucExcMetodos.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idExcavaMetodoColumn.ColumnName);
                ucAguaPresencia.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idAguaPresenciaColumn.ColumnName);
           
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
                       if (mId == null)
                       {
                           mId = System.Guid.NewGuid();

                           ((DataRowView)mBindMaster.Current)["id"] = mId;
                       }

                       mBindMaster.EndEdit();

                       mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


                       oSingletonDsBd.getInstance.save(true);


                       ////((DataSet)mBindMaster.DataSource).WriteXml(oTadil.data.Files.fileBbdd);

                       ////oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
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
       #endregion
       #region "Eventos"
       protected override void OnClosed(EventArgs e)
       {
           base.OnClosed(e);
       }
       private void frmEst_FormClosing(object sender, FormClosingEventArgs e)
       {

           try
           {

               if (isValidoFrm())
               {       
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

           if (ucProhibirPaso != null && ucProhibirPaso.valor == true)
           {
              
             
               ucCimViaPu.uiCombo.SelectedIndex = -1;
               ucCimPasInf.uiCombo.SelectedIndex = -1;
               ucExcMetodos.uiCombo.SelectedIndex = -1;
               ucAguaPresencia.uiCombo.SelectedIndex = -1;

               grDataEnable(false);
           }

           if (ucProhibirPaso != null && ucProhibirPaso.valor == false)
           {


               grDataEnable(true);
           }

       }



       #endregion


       private void bindUpdate()
       {
           oSingletonDsBd.getInstance.Dispose();

       }













 

    }
}
