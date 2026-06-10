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

    using engNet.eventos;
    using tadLayUI;
    using tadLayLan;
    using tadLayLan.img;
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica;
    using Properties;

    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.datos.Secciones;


    public partial class frmRoadDobleSinMediana :frmRoot
    {


        private Guid? mId = null;
        private BindingSource mBindMaster;
        
        
        public frmRoadDobleSinMediana()
        {  
            InitializeComponent();
            postConstructor();
        }



        #region "POST CONSTRUCTOR"
        private void postConstructor()
        {
            #region "Traduccion"

            ////////////////////////////////////////////////////
            //TRADUCCION
            /////////////////////////////////////////////////////

            var name = oTadil.KAppHeaderName;
            this.Text = name;
            this.lblHeader.Text = strFrmSecciones.uiCalzadaDoble + " - " + strFrmSecciones.uiDOBSIN;

            //Grupo Zona
            grGeneral.Text = strFrmSecciones.uiDatos;
            ucNombre.uiLbl = strFrmSecciones.uiNombre;
            ucDescripcion.uiLbl = strFrmSecciones.uiDescripcion;

            //Grupo Cunetas
            grCunetaDatos.Text = strFrmSecciones.uiGrCunetaDatos;

            //Grupo Geometria
            grGeometria.Text = strFrmSecciones.uiGeometria;
            ucCarrilAncho.uiLbl = strFrmSecciones.uiAnchoCarril;
            ucCarrilesIzqNum.uiLbl = strFrmSecciones.uiCarrilesMargenIzquierdaNum;
            ucCarrilesDerNum.uiLbl = strFrmSecciones.uiCarrilesMargenDerechaNum;
            ucArcenIntAncho.uiLbl = strFrmSecciones.uiArcenIntAncho;
            ucArcenExtAncho.uiLbl = strFrmSecciones.uiArcenExtAncho;
            ucFirmeIntoArcen.uiLbl = strFrmSecciones.uiFirmeIntoArcen;
            ucBermaExtAncho.uiLbl = strFrmSecciones.uiBermaExtAncho;
            ucBermaExtPendiente.uiLbl = strFrmSecciones.uiBermaExtPendiente;
            ucFirmeTalud.uiLbl = strFrmSecciones.uiFirmeTalud;
            ucBombeoPC.uiLbl = strFrmSecciones.uiBombeoPC;

            //Grupo Barrera
            grBarrera.Text = strFrmSecciones.uiBarrera;
            ucBarreraDwg.uiLbl = strFrmSecciones.uiBarreraDwg;

            //Botón Ver Seccion
            this.btnVerSeccion.Text = strFrmSecciones.uiVerSeccion;

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;


            this.ucPrecios.uiLbl = "Precio";

            ucPrecios.uiCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ucPrecios.uiCombo.DataSource = oSingletonDsBd.getInstance.dataset.tbMacroPrecios
                .AsEnumerable()
                .Where(row => row.idTbRoadTipo == "DOBSIN")
                .ToList();
            ucPrecios.uiCombo.ValueMember = "id";
            ucPrecios.uiCombo.DisplayMember = "nombre";


            #endregion
            #region "SetUp Objetos"

            //InicializarObjetos
            ucCunetaGeoMat.populate();
            ucCunetaPos.populate();

            //SelectFile
            ucFileSelect1.setup(oTadil.data.Files.folderCadSecBar, oTadil.data.Files.KFileDwgExtensionFiltro);
            ucFileSelect1.evFile += new EventHandler<engNet.eventos.oEventArgs<string>>(ucFileSelect1_evFile);

            ucBarreraDwg.textbox.Enabled = false;


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
            mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana.TableName;
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana;

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
            dsBd.tbRoadDobleSinMedianaDataTable miTb = oSingletonDsBd.getInstance.dataset.tbRoadDobleSinMediana;

            //GrupoZona
            ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
            ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);

            ucCunetaGeoMat.uiComboTipo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaTipoColumn.ColumnName);
            ucCunetaGeoMat.uiComboGeo.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaGeoColumn.ColumnName);
            ucCunetaGeoMat.uiComboMat.DataBindings.Add("SelectedValue", mBindMaster, miTb.idCunetaMaterialColumn.ColumnName);


            ucCunetaPos.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.cunetaPosicionColumn.ColumnName);

            ucCarrilAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.carrilAnchoColumn.ColumnName, true);
            ucCarrilesIzqNum.textbox.DataBindings.Add("valorInt", mBindMaster, miTb.carrilIzqNumColumn.ColumnName, true);
            ucCarrilesDerNum.textbox.DataBindings.Add("valorInt", mBindMaster, miTb.carrilDerNumColumn.ColumnName, true);
            ucArcenExtAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.arcenExtAnchoColumn.ColumnName, true);
            ucArcenIntAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.arcenIntAnchoColumn.ColumnName, true);
            ucFirmeIntoArcen.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.firmeIntoArcenColumn.ColumnName, true);
            ucBermaExtAncho.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bermaExtAnchoColumn.ColumnName, true);
            ucBermaExtPendiente.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bermaExtPendienteColumn.ColumnName, true);
            ucFirmeTalud.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.firmeTaludColumn.ColumnName, true);
            ucBombeoPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.bombeoPCColumn.ColumnName, true);
            ucBarreraDwg.textbox.DataBindings.Add("Text", mBindMaster, miTb.barreraDwgColumn.ColumnName, true);

            ucPrecios.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.id_precioColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);

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
                       ((DataRowView)mBindMaster.Current)["idTbRoadTipo"] = "DOBSIN";
                   }

                   mBindMaster.EndEdit();

                   oDalTbRoadDobleSinMediana.saveTabla();
                          
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

       //VER SECCION
       private void btnVerSeccion_Click(object sender, EventArgs e)
       {
           try
           {
               frmRoadImage.createInstance(this.lblHeader.Text,imgSecciones.secAutoviaSinMediana).Show();
           }
           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }

       }


       #endregion
       #region "Eventos"
       void ucFileSelect1_evFile(object sender, engNet.eventos.oEventArgs<string> e)
       {
           FileInfo miFileInfo = new FileInfo(e.Value);
           ucBarreraDwg.uitxt = miFileInfo.Name;
       }
       private void frmEst_FormClosing(object sender, FormClosingEventArgs e)
       {
           mBindMaster.CancelEdit();
           mBindMaster.RemoveFilter();
           frmRoadImage.delteteInstance(); 
       }
       #endregion






















    }
}
