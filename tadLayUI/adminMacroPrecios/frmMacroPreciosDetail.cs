using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.adminGis
{
    using System.IO;
    using System.Globalization;


    using tadLayUI;

    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica;
    using tadLayLogica.datos.BaseDatos;
    
    
    public partial class frmMacroPreciosDetail : frmRoot
    {


        private Guid? mId = null;
        private string mIdRoad = string.Empty;

        private object[] mItemCurrentIni;
        private BindingSource mBindMaster;


        public frmMacroPreciosDetail():base()    
        {
            InitializeComponent();
            postConstructor();
        }




        #region "Metodos Publicos"
        public void create(string iIdRoad)
        {
            mId = null;
            mIdRoad = iIdRoad;
            bindCreate();     
        }
        public void edit(Guid iId,string iIdRoad)
        {
            mId = iId;
            mIdRoad = iIdRoad;
            bindCreate(); 
        }
        #endregion



        #region "Metodos Privados"
        private  void  postConstructor()
        {

            //Frm Name
            var name = oTadil.KAppHeaderName;

            this.Text = name;
            
            //Relleno los ComboBox
            ucDrenaje.populate();
            ucBalizamiento.populate();
            ucServiciosReposicion.populate();
            ucGeotecnicaCorrecciones.populate();
            ucDesviosProvisionales.populate();
            ucActuacionesComple.populate();
            ucMedidasCorrectoras.populate();
            ucSeguridadSalud.populate();
           
            //FRM
            lblHeader.Text = strFrmMacroPrecios.uiEditorMacroPrecios;

            //Grupo Zona
            grGeneral.Text = strFrmMacroPrecios.uiDatosGrupo;
            ucNombre.uiLbl = strGeneral.uiNombre;
            ucDescripcion.uiLbl = strGeneral.uiDescripcion;

            //Grupo Estructura
            grDetalle.Text = strFrmMacroPrecios.uiMacroPrecios;

            ucDrenaje.uiLbl = strFrmMacroPrecios.uiDrenaje;
            ucBalizamiento.uiLbl = strFrmMacroPrecios.uiBalizamiento;
            ucServiciosReposicion.uiLbl = strFrmMacroPrecios.uiServiciosReposicion;
            ucGeotecnicaCorrecciones.uiLbl = strFrmMacroPrecios.uiGeotecnicaCorrecciones;
            ucDesviosProvisionales.uiLbl = strFrmMacroPrecios.uiDesviosProvisioneales;
            ucActuacionesComple.uiLbl = strFrmMacroPrecios.uiActuacionesComplementarias;
            ucMedidasCorrectoras.uiLbl = strFrmMacroPrecios.uiMedidasCorrectoras;
            ucSeguridadSalud.uiLbl = strFrmMacroPrecios.uiSeguridadSalud;

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
                mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbMacroPrecios.TableName;
                mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbMacroPrecios;

                if (mId ==null)
                {
                    mBindMaster.AddNew();
                }
                else
                {
                    string miQuery = "id = '{0}' AND idTbRoadTipo = '{1}'";

                    mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mId),mIdRoad);
                }


                mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


                //Tabla Zona Geologica
                dsBd.tbMacroPreciosDataTable miTb = oSingletonDsBd.getInstance.dataset.tbMacroPrecios;

                //GrupoZona
                ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
                ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);

               //MacroPrecios
                ucDrenaje.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.drenajeColumn.ColumnName);
                ucBalizamiento.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.balizamientoColumn.ColumnName);
                ucServiciosReposicion.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.serviciosReposicionColumn.ColumnName);
                ucGeotecnicaCorrecciones.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.geotecnicaCorreccionesColumn.ColumnName);
                ucDesviosProvisionales.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.desviosProvisionalesColumn.ColumnName);
                ucActuacionesComple.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.actuacionesComplemenColumn.ColumnName);
                ucMedidasCorrectoras.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.medidasCorrectorasColumn.ColumnName);
                ucSeguridadSalud.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.seguridadSaludColumn.ColumnName);


            
        }
       #endregion



       #region "Botones"
       //GUARDAR
       private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {
           try
           {

               if (isValidoFrm)
               {
                   if (mId==null)
                   {
                       mId = System.Guid.NewGuid();
                       ((DataRowView)mBindMaster.Current)["id"] = mId;
                       ((DataRowView)mBindMaster.Current)["idTbRoadTipo"] = mIdRoad;
                   }

                   mBindMaster.EndEdit();

                   mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;

                   oSingletonDsBd.getInstance.save(true);
                      
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

               if (isValidoFrm)
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



 

       #endregion













 

    }
}
