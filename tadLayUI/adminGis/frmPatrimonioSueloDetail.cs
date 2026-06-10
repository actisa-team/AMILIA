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
    
    

    /// <summary>
    /// DEBEMOS DE ELIMINAR EL FILTRO DEL DATABIND DEL DETALLE
    /// </summary>
    public partial class frmPatrimonioSueloDetail : Form
    {

        private string mIdSuelo = string.Empty;
        private Guid? mId = null;
        private object[] mItemCurrentIni;
        private BindingSource mBindMaster;


        public frmPatrimonioSueloDetail(string iIdSueloCode)
        {
            InitializeComponent();
            mIdSuelo = iIdSueloCode;
            post_Constructor();
            traduccion();
        }




        #region "Metodos Publicos"
        public void create()
        {
            mId = null;
            bindCreate();
          
        }
        public void edit(Guid iIdItem)
        {
            mId = iIdItem;
            bindCreate(); 
        }
        #endregion



        #region "Metodos Privados"
        private bool isValidoFrm()
        {
            return oValidar.isValidoGrupoByFrm(this);
        }
        private void post_Constructor()
        {
            //Populate Combo
            ucValoracion.populate();
            ucSueloExpropiacionPrecio.populate();

            ucProhibirPaso.uiCombo.SelectedIndexChanged += new EventHandler(uiComboProhibirPaso_SelectedIndexChanged);
        }
        private void traduccion()
        {

            //FRM
            var name = oTadil.KAppHeaderName;

            this.Text = name;
            lblHeader.Text = strFrmGisGeneral.ResourceManager.GetString("ui" + mIdSuelo);

            //Grupo General
            grGeneral.Text = strFrmGisGeneral.uiDatosZona;
            ucNombre.uiLbl = strFrmGisGeneral.uiNombre;
            ucDescripcion.uiLbl = strFrmGisGeneral.uiDescripcion;
            ucProhibirPaso.uiLbl = strFrmGisGeneral.uiProhibirPaso;

            //Grupo Detalles
            grDetalle.Text = strFrmGisGeneral.uiDatos;
            ucValoracion.uiLbl = strFrmGisGeneral.uiValoracion;

            if (this.mIdSuelo == "URBANO" | this.mIdSuelo == "URBANI" | this.mIdSuelo == "NOURBA")
            {
                ucSueloExpropiacionPrecio.uiLbl = strFrmGisGeneral.uiValorPatrimonialSuelo;

            }
            else
            {
                ucSueloExpropiacionPrecio.uiLbl = strFrmGisGeneral.uiSueloValoracionProduccionSuelo;
            }


            


            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;

        }
        private void bindCreate()
        {
            
                mBindMaster = new BindingSource();
                mBindMaster.DataMember =oSingletonDsBd.getInstance.dataset.tbPatrimonioSuelo.TableName;
                mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbPatrimonioSuelo;
                   
                if (mId ==null)
                {
                    mBindMaster.AddNew();
                }
                else
                {
                    string miQuery = "id = '{0}' AND idCodeSuelo = '{1}'";
                    mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mId),mIdSuelo);
                }

                mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


                //Tabla Zona Geologica
                dsBd.tbPatrimonioSueloDataTable miTb = oSingletonDsBd.getInstance.dataset.tbPatrimonioSuelo;


                //GrupoZona
                ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
                ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);
                ucProhibirPaso.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.prohibirPasoColumn.ColumnName);
               
               //Detalle
                ucValoracion.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.valoracionColumn.ColumnName);
                ucSueloExpropiacionPrecio.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idMatValorExpropiacionSueloColumn.ColumnName);
         
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
                           ((DataRowView)mBindMaster.Current)["idCodeSuelo"] = mIdSuelo;
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
  
        //OJO AL SALIR QUITAR EL FILTRO DEL BINDING
       private void frmEst_FormClosing(object sender, FormClosingEventArgs e)
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
                       mBindMaster.RemoveFilter();
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

       private void uiComboProhibirPaso_SelectedIndexChanged(object sender, EventArgs e)
       {

           if (ucProhibirPaso != null && ucProhibirPaso.valor == true)
           {
               //grDetalle.Enabled = false;
               ucValoracion.Enabled = false;
           }
           else
           {
               //grDetalle.Enabled = true;
               ucValoracion.Enabled = true;
           }

       }


       #endregion
    }
}
