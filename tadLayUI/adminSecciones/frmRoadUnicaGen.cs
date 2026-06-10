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
    public partial class frmRoadUnicaGen : frmRoot
    {


        private Guid? mId = null;
        private BindingSource mBindMaster;
        
        
        public frmRoadUnicaGen()
        {  
            InitializeComponent();
            postConstructor();
        }



        #region "POST CONSTRUCTOR"

        private void postConstructor()
        {
            #region "Traduccion"
            this.Text = oTadil.KAppHeaderName;

            this.lblHeader.Text = strFrmSecciones.uiCalzadaUnica + " - " + strFrmSecciones.uiUNIGEN;

            //Grupo Zona
            this.grGeneral.Text = strFrmSecciones.uiDatos;
            this.ucNombre.uiLbl = strFrmSecciones.uiNombre;
            this.ucDescripcion.uiLbl = strFrmSecciones.uiDescripcion;

            //Grupo Cunetas
            this.grCunetaDatos.Text = strFrmSecciones.uiGrCunetaDatos;

            //Grupo Geometria
            this.grGeometria.Text = strFrmSecciones.uiGeometria;
            this.ucCarrilAncho.uiLbl = strFrmSecciones.uiAnchoCarril;
            this.ucCarrilesIzqNum.uiLbl = strFrmSecciones.uiCarrilesMargenIzquierdaNum;
            this.ucCarrilesDerNum.uiLbl = strFrmSecciones.uiCarrilesMargenDerechaNum;
            this.ucFirmeIntoArcen.uiLbl = strFrmSecciones.uiFirmeIntoArcen;
            this.ucArcenExtAncho.uiLbl = strFrmSecciones.uiArcenExtAncho;
            this.ucBermaExtAncho.uiLbl = strFrmSecciones.uiBermaExtAncho;
            this.ucBermaExtPendiente.uiLbl = strFrmSecciones.uiBermaExtPendiente;
            this.ucFirmeTalud.uiLbl = strFrmSecciones.uiFirmeTalud;
            this.ucBombeoPC.uiLbl = strFrmSecciones.uiBombeoPC;

            //Boton Imagen
            this.btnSeccionVer.Text = strFrmSecciones.uiVerSeccion;

            //GuardarSalir
            this.lnkSave.Text = strGeneral.uiGuardar;
            this.lnkSalir.Text = strGeneral.uiSalir;

            this.ucPrecios.uiLbl = "Precio";
            
            ucPrecios.uiCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ucPrecios.uiCombo.DataSource = oSingletonDsBd.getInstance.dataset.tbMacroPrecios
                .AsEnumerable()
                .Where(row => row.idTbRoadTipo == "UNIGEN")
                .ToList();
            ucPrecios.uiCombo.ValueMember = "id";
            ucPrecios.uiCombo.DisplayMember = "nombre";
            #endregion

            #region "SetUpObjetos"

            this.ucCunetaGeoMat.populate();
            this.ucCunetaPos.populate();

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

        private void bindCreate()
        {
             
            mBindMaster = new BindingSource();
            mBindMaster.DataMember =  oSingletonDsBd.getInstance.dataset.tbRoadUniGen.TableName;
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbRoadUniGen;

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
            dsBd.tbRoadUniGenDataTable miTb = oSingletonDsBd.getInstance.dataset.tbRoadUniGen;

            //GrupoZona
            ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
            ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);

            ucCunetaGeoMat.uiComboTipo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaTipoColumn.ColumnName);
            ucCunetaGeoMat.uiComboGeo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaGeoColumn.ColumnName);
            ucCunetaGeoMat.uiComboMat.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaMaterialColumn.ColumnName);


            ucCunetaPos.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.cunetaPosicionColumn.ColumnName);

            ucPrecios.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.id_precioColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);

            ucCarrilAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.carrilAnchoColumn.ColumnName, true);
            ucCarrilesIzqNum.textbox.DataBindings.Add("valorInt", mBindMaster, miTb.carrilIzqNumColumn.ColumnName, true);
            ucCarrilesDerNum.textbox.DataBindings.Add("valorInt", mBindMaster, miTb.carrilDerNumColumn.ColumnName, true);
            ucFirmeIntoArcen.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.firmeIntoArcenColumn.ColumnName, true);
            ucArcenExtAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.arcenExtAnchoColumn.ColumnName, true);
            ucBermaExtAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bermaExtAnchoColumn.ColumnName, true);
            ucBermaExtPendiente.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bermaExtPendienteColumn.ColumnName, true);
            ucFirmeTalud.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.firmeTaludColumn.ColumnName, true);
            ucBombeoPC.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.bombeoPCColumn.ColumnName, true);
         

        }
       #endregion


        #region "Validaciones"

        protected override bool isValidoFrm
        {
            get
            {

                bool miValidoFrm = base.isValidoFrm;
                bool miValidoAnchos = isValidoAnchos;
                
                if ( ! miValidoFrm )
                {
                   return false;
                }

                if (! miValidoAnchos)
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
                if (ucFirmeIntoArcen.valorDouble > ucArcenExtAncho.valorDouble)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
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
                       ((DataRowView)mBindMaster.Current)["idTbRoadTipo"] = "UNIGEN";
                   }

                   mBindMaster.EndEdit();

                   oDalTabRoadUnica.saveTabla();
                      
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
       private void btnSeccionVer_Click(object sender, EventArgs e)
       {
           try
           {
               frmRoadImage.createInstance(this.lblHeader.Text,imgSecciones.secCalzadaUnica).Show();
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

       private void ucBombeoPC_Load(object sender, EventArgs e)
       {

       }

 

    }
}
