using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.adminSecciones
{
    using System.IO;
    using System.Globalization;

    using tadLayUI;
    using tadLayLan;
    using tadLayLan.img;
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica;
    using Properties;


    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.datos.Secciones;

    /// <summary>
    /// FRM CUNETAS TRIANGULARES
    /// </summary>
    public partial class frmRoadDobleAutovia : frmRoot
    {


        private Guid? mId = null;
        private BindingSource mBindMaster;
        
        
        public frmRoadDobleAutovia()
        {  
            InitializeComponent();
            postConstructor();
        }



        #region "POST CONSTRUCTOR"

        private void postConstructor()
        {

            #region "Traduccion"

            var name = oTadil.KAppHeaderName;
            this.Text = name;
         
            this.lblHeader.Text = strFrmSecciones.uiCalzadaDoble + " - " + strFrmSecciones.uiDOBAUT;

            //Grupo Zona
            grGeneral.Text = strFrmSecciones.uiDatos;
            ucNombre.uiLbl = strFrmSecciones.uiNombre;
            ucDescripcion.uiLbl = strFrmSecciones.uiDescripcion;

            //Grupo Cunetas Ixterior
            grCunetaExterior.Text = strFrmSecciones.uiCunetaExterior;

            //Cuneta Interior
            grCunetaInterior.Text = strFrmSecciones.uiCunetaInterior;

            //Grupo Geometria
            grGeometria.Text = strFrmSecciones.uiGeometria;
            ucCarrilAncho.uiLbl = strFrmSecciones.uiAnchoCarril;
            ucCarrilesIzqNum.uiLbl = strFrmSecciones.uiCarrilesMargenIzquierdaNum;
            ucCarrilesDerNum.uiLbl = strFrmSecciones.uiCarrilesMargenDerechaNum;

            ucFirmeIntoArcen.uiLbl = strFrmSecciones.uiFirmeIntoArcen;

            ucArcenExtAncho.uiLbl = strFrmSecciones.uiArcenExtAncho;
            ucArcenIntAncho.uiLbl = strFrmSecciones.uiArcenIntAncho;

            ucBermaExtAncho.uiLbl = strFrmSecciones.uiBermaExtAncho;
            ucBermaExtPendiente.uiLbl = strFrmSecciones.uiBermaExtPendiente;

            ucBermaIntPendiente.uiLbl = strFrmSecciones.uiBermaIntPendiente;
            ucMedianaAncho.uiLbl = strFrmSecciones.uiMedianaAncho; 
            ucFirmeTalud.uiLbl = strFrmSecciones.uiFirmeTalud;

            ucBombeoPC.uiLbl = strFrmSecciones.uiBombeoPC;

            //BotonSeccion
            this.btnVerSeccion.Text = strFrmSecciones.uiVerSeccion;


            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;

            this.ucPrecios.uiLbl = "Precio";

            ucPrecios.uiCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ucPrecios.uiCombo.DataSource = oSingletonDsBd.getInstance.dataset.tbMacroPrecios
                .AsEnumerable()
                .Where(row => row.idTbRoadTipo == "DOBAUT")
                .ToList();
            ucPrecios.uiCombo.ValueMember = "id";
            ucPrecios.uiCombo.DisplayMember = "nombre";



            #endregion
            #region "SetUpObjetos"

            ucCunetaExtGeoMat.populate();
            ucCunetaInterior.populate();
            ucCunetaPos.populate();

            #endregion

        }



        #endregion
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

        protected override bool isValidoFrm
        {
            get
            {

                bool miValidoFrm = base.isValidoFrm;
                bool miValidoAnchos = isValidoAnchos;

                if (!miValidoFrm)
                {
                    return false;
                }

                if (!miValidoAnchos)
                {
                    ucFirmeIntoArcen.textbox.addError(strFrmSecciones.eAnchoArcen);
                    return false;
                }

                return true;
            }
        }


        private bool isValidoAnchos
        {
            get
            {
                if (ucFirmeIntoArcen.valorDouble > ucArcenExtAncho.valorDouble | ucFirmeIntoArcen.valorDouble > ucArcenIntAncho.valorDouble)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        private void bindCreate()
        {

           
             
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia.TableName;
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia;

            if (mId == null)
            {
                mBindMaster.AddNew();
            }
            else
            {
                string miQuery = "id = '{0}'";
                mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mId));
            }

   
            //Tabla Zona Geologica
            dsBd.tbRoadDobleAutoviaDataTable miTb = oSingletonDsBd.getInstance.dataset.tbRoadDobleAutovia;

            //GrupoZona
            ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
            ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);

            //Cuneta Exterior
           ucCunetaExtGeoMat.uiComboTipo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaTipoExtColumn.ColumnName);
           ucCunetaExtGeoMat.uiComboGeo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaGeoExtColumn.ColumnName);
           ucCunetaExtGeoMat.uiComboMat.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaMaterialExtColumn.ColumnName);

            //Cuneta Interior
           ucCunetaInterior.uiComboTipo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaTipoIntColumn.ColumnName);
           ucCunetaInterior.uiComboGeo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaGeoIntColumn.ColumnName);
           ucCunetaInterior.uiComboMat.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaMaterialIntColumn.ColumnName);

            //Cuneta Posicion
            ucCunetaPos.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.cunetaPosicionColumn.ColumnName);


            //Geometria
            ucCarrilAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.carrilAnchoColumn.ColumnName, true);

            ucCarrilesIzqNum.textbox.DataBindings.Add("valorInt", mBindMaster, miTb.carrilIzqNumColumn.ColumnName, true);
            ucCarrilesDerNum.textbox.DataBindings.Add("valorInt", mBindMaster, miTb.carrilDerNumColumn.ColumnName, true);

            ucFirmeIntoArcen.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.firmeIntoArcenColumn.ColumnName, true);
            ucArcenExtAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.arcenExtAnchoColumn.ColumnName, true);
            ucArcenIntAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.arcenIntAnchoColumn.ColumnName, true);

            ucBermaExtAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bermaExtAnchoColumn.ColumnName, true);
            ucBermaExtPendiente.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bermaExtPendienteColumn.ColumnName,true);

            ucBermaIntPendiente.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bermaIntPendienteColumn.ColumnName, true);
            ucMedianaAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.medianaAnchoColumn.ColumnName, true);
            ucFirmeTalud.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.firmeTaludColumn.ColumnName, true);

            ucBombeoPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bombeoPCColumn.ColumnName, true);

            ucPrecios.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.id_precioColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);

        }
        #endregion



        #region "Botones"
        /// <summary>
        /// SAVE
        /// </summary>
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
                       ((DataRowView)mBindMaster.Current)["idTbRoadTipo"] = "DOBAUT";
                   }

                   mBindMaster.EndEdit();

                   oDalTabRoadDobleAutovia.saveTabla();

                         
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
        /// SALIR
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
       /// VER SECCION
       /// </summary>
       private void btnVerSeccion_Click(object sender, EventArgs e)
       {
           try
           {
               frmRoadImage.createInstance(this.lblHeader.Text,imgSecciones.secAutoviaConMediana).Show();
           }
           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }


       }
       #endregion
       #region "Eventos"
       private void frmEst_FormClosing(object sender, FormClosingEventArgs e)
       {

           mBindMaster.CancelEdit();
           mBindMaster.RemoveFilter();

           frmRoadImage.delteteInstance();
       }
       #endregion

 

















    }
}
