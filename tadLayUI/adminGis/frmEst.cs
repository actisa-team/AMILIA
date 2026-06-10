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
    using tadLayLogica.Tools;
    using engNet.Extension.String;
    
    
    public partial class frmEst : Form
    {
        private Guid? mIdEst = null;
        private object[] mItemCurrentIni;
        private BindingSource mBindMaster;
        private oSeccionPuenteDwg mSeccionesPuentesDwg = null;
        
        
        public frmEst()
        {
            InitializeComponent();
            postConstructor();
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

        private void postConstructor()
        {

            #region "Traduccion"

            //Traduccion
            var name = oTadil.KAppHeaderName;

            this.Text = name;
            lblHeader.Text = strFrmGisEst.uiHeader;

            //Grupo Zona
            grGeneral.Text = strFrmGisGeneral.uiDatosZona;
            ucNombre.uiLbl = strFrmGisGeneral.uiNombre;
            ucProhibirPaso.uiLbl = strFrmGisEst.uiProhibirEst;
            ucColorZona.uiLbl = strFrmGisGeneral.uiColor;
            ucDescripcion.uiLbl = strFrmGisGeneral.uiDescripcion;

            //Grupo Estructura
            grDetalle.Text = strFrmGisEst.uiDatosEstructura;
            ucEstTipoCode1.uiLblGrupo = strFrmGisGeneral.uiTipología;
            ucEstTipoCode1.uiLblItem = strFrmGisEst.uiEstructura;
            ucSecNombre.uiLbl = strFrmGisGeneral.uiDwgSeccion;
            ucAlturaMaxima.uiLbl = strFrmGisEst.uiAlturaMax;
            ucDistanciaPila.uiLbl = strFrmGisEst.uiDistanciaPilas;
            ucAnchoTablero.uiLbl = strFrmGisEst.uiAnchoMax;

            //Boton Buscar
            this.btnBuscarSeccion.Text = strFrmGisGeneral.uiBuscarSeccion;

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;

            #endregion
            #region "SetUpObjetos"

            ucFileSelect1.setup(oTadil.data.Files.folderCadSecPuentes, oTadil.data.Files.KFileDwgExtensionFiltro);

            ucFileSelect1.evFile += new EventHandler<engNet.eventos.oEventArgs<string>>(ucFileSelect1_evFile);

            ucProhibirPaso.uiCombo.SelectedIndexChanged += new EventHandler(uiComboProhibirPaso_SelectedIndexChanged);



            //Relleno las Estructuras
            ucEstTipoCode1.populate("EST");


            ucSecNombre.textbox.Enabled = false;

            #endregion



        }




        private bool isValidoFrm ()
        {

            bool miValidoGrupo = oValidar.isValidoGrupoByFrm(this);
              
                return miValidoGrupo;
               
        }

        private void grDataEnable(bool iEstado)
        {
            grDetalle.Enabled = iEstado;
        }


        private void bindCreate()
        {
     
                mBindMaster = new BindingSource();
                mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbEst.TableName;
                mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbEst;

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
                dsBd.tbEstDataTable miTb = oSingletonDsBd.getInstance.dataset.tbEst;

                //Habia Problemas con el formateo de los numeros en los databind  1.25 --> 125
                //Con la Culture Info
                CultureInfo cultInfo = new CultureInfo("en-GB", true);


                //!!!!LOS VALORES POR DEFECTO NO LOS CARGA (LOS DEJA EN NULO!!!!!!

                //GrupoZona
                ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
                ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);
                ucProhibirPaso.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.allowEstructurasColumn.ColumnName);
                ucColorZona.panel.DataBindings.Add("colorInt", mBindMaster, miTb.colorColumn.ColumnName, false, DataSourceUpdateMode.OnPropertyChanged);


               //GrupoDetalle
                ucEstTipoCode1.uiComboGrupo.DataBindings.Add("Text", mBindMaster, miTb.estClasificacionColumn.ColumnName);
                ucEstTipoCode1.uiComboItem.DataBindings.Add("SelectedValue", mBindMaster, miTb.estMatColumn.ColumnName);
                ucSecNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.dwgNameColumn.ColumnName);
                ucAlturaMaxima.textbox.DataBindings.Add("Text", mBindMaster, miTb.alturaMaxColumn.ColumnName);
                ucDistanciaPila.textbox.DataBindings.Add("Text",mBindMaster,miTb.distanciaPilaMaxColumn.ColumnName);
                ucAnchoTablero.textbox.DataBindings.Add("Text", mBindMaster, miTb.anchoTableroColumn.ColumnName);
        
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
       /// BUSCAR SECCION
       /// </summary>
       private void btnBuscarSeccion_Click(object sender, EventArgs e)
       {

           if (mSeccionesPuentesDwg == null)
           {
               mSeccionesPuentesDwg = new oSeccionPuenteDwg(oTadil.data.Files.folderCadSecPuentes);
           }

           string miSeccion = mSeccionesPuentesDwg.getFileNameConExtension(this.ucEstTipoCode1.getGrupoCode(), this.ucAnchoTablero.valorDouble, this.ucDistanciaPila.valorDouble);

           if (string.IsNullOrEmpty(miSeccion))
           {
               oTadil.data.UserInfo.showInfo(strError.eAsignarSeccion);
           }
           else
           {
               this.ucSecNombre.textbox.Text = miSeccion;
           }

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
                          
                           mBindMaster.CancelEdit();

                           this.Dispose();
                           this.Close();

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
  
       }



       #endregion

      private void ucDescripcion_Load(object sender, EventArgs e)
       {

       }

  
















 

    }
}
