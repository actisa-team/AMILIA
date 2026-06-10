using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI.adminProyecto
{
    using System.IO;
    using tadLayLogica;
    using tadLayLogica.datos.proyecto;
    using tadLayLan;
    using tadLayShare;
    using tadLayLogica.datos;
    using tadLayLogica.datos.BaseDatos;
    
    
    public partial class frmFiles : Form
    {
        private eEstudioTipo? mEstudioTipo = null;
        private string mId = "APP";
        private BindingSource mBindMaster;
            
        public frmFiles(eEstudioTipo iEstudioTipo)
        {
            InitializeComponent();
            mEstudioTipo = iEstudioTipo;
            postConstructor();
        }

        #region "Metodos Privados"
        private void postConstructor()
        {

            //Traduccion
            this.Text = strFrmFiles.uiCONFIL;

            this.grFilesNormas.Text = strFrmFiles.uiGrFicherosNormas;
            this.ucFileNormasVpRadio.uiLbl = strFrmFiles.uiFileVpRp;
            this.ucFileNormasVpKv.uiLbl = strFrmFiles.uiFileVpKv;

            this.grFileBaseDatos.Text = strFrmFiles.uiGrFicheroDatosGis;
            this.ucFileBaseDatos.uiLbl = strFrmFiles.uiFileBd;
     

            this.lnkFileBdSelect.Text = strGeneral.uiSeleccionar + "..." ;
            this.lnkFileVpRpSelect.Text = strGeneral.uiSeleccionar + "...";
            this.lnkFileVpRpOpen.Text = strGeneral.uiEditar + "...";

            this.lnkFileVpKvSelect.Text = strGeneral.uiSeleccionar + "...";
            this.lnkFileVpKvOpen.Text = strGeneral.uiEditar + "...";


            #region "SetUp Objetos"

            //UC-LBL-TXT
            this.ucFileBaseDatos.textbox.Enabled = false;
            this.ucFileNormasVpRadio.textbox.Enabled = false;
            this.ucFileNormasVpKv.textbox.Enabled = false;

            //Alinear Texto a la Derecha
            this.ucFileBaseDatos.textbox.TextChanged += new EventHandler(textbox_TextChanged);
            this.ucFileNormasVpRadio.textbox.TextChanged += new EventHandler(textbox_TextChanged);
            this.ucFileNormasVpKv.textbox.TextChanged += new EventHandler(textbox_TextChanged);

            //BARRA DETALLES
            this.ucToolDetail1.lnkSalir.Visible = false;
            this.ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            this.ucToolDetail1.lnkCancel.Click += new EventHandler(lnkCancel_Click);


            if (mEstudioTipo == eEstudioTipo.ESTPRE)
            {
                grFilesNormas.Visible = true;
                grFileBaseDatos.Visible = false;
                grFileBaseDatos.Enabled = false;

               ucToolDetail1.Location = new Point(0, 200);

            }
            else if (mEstudioTipo == eEstudioTipo.ESTINF)
            {
                grFilesNormas.Visible = true;
                grFileBaseDatos.Visible = true;

                ucToolDetail1.Location = new Point(0, 310);
            }
            else
            {
                throw new oExEnumNotImplemented(mEstudioTipo.ToString());
            }

            #endregion



            //Bind
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsApp.getInstance.dataset.tbFiles.TableName;
            mBindMaster.DataSource = oSingletonDsApp.getInstance.dataset.tbFiles;

            string miQuery = "id = '{0}' ";

            mBindMaster.Filter = string.Format(miQuery, mId);

            ucFileNormasVpRadio.textbox.DataBindings.Add("Text", mBindMaster, oSingletonDsApp.getInstance.dataset.tbFiles.normasVpRpColumn.ColumnName);
            ucFileNormasVpKv.textbox.DataBindings.Add("Text", mBindMaster, oSingletonDsApp.getInstance.dataset.tbFiles.normasVpKvColumn.ColumnName);

            if (mEstudioTipo == eEstudioTipo.ESTINF)
            {
                ucFileBaseDatos.textbox.DataBindings.Add("Text", mBindMaster, oSingletonDsApp.getInstance.dataset.tbFiles.baseDatosColumn.ColumnName);
            }


        }
        private bool isFilePathOk()
        {

            bool miIsOk = true;


            if (!File.Exists(ucFileNormasVpRadio.uitxt))
            {
                ucFileNormasVpRadio.textbox.addError(strError.eFicheroNormasNoExisteVpRp);
                miIsOk = false;
            }

            if (!File.Exists(ucFileNormasVpKv.uitxt))
            {
                ucFileNormasVpKv.textbox.addError(strError.eFicheroNormasNoExisteVpKv);
                miIsOk = false;
            }

            if (mEstudioTipo== eEstudioTipo.ESTINF &&  !File.Exists(ucFileBaseDatos.uitxt))
            {
                ucFileBaseDatos.textbox.addError(strError.eFicheroBBDDNoExiste);
                miIsOk = false;
            }

            return miIsOk;
        }
        #endregion
        #region" Botonera"
        void lnkSave_Click(object sender, EventArgs e)
        {

            try
            {

                if (oValidar.isValidoGrupoByFrm(this) && isFilePathOk())
                {                                                                          
                    mBindMaster.EndEdit();

                    oDalTbFiles.saveTable();

                    oDalTbFiles.setupArchivosConfiguracion(mEstudioTipo.Value);

                    if (mEstudioTipo == eEstudioTipo.ESTINF)
                    {
                        if (oSingletonDsBd.getInstance != null)
                        {
                            oSingletonDsBd.getInstance.Dispose();
                        }
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
        void lnkCancel_Click(object sender, EventArgs e)
        {

            try
            {
                mBindMaster.CancelEdit();

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        #endregion
        #region "Link Files"

        private void lnkFileBdSelect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string miFile = oTadil.data.Files.getFileBbdd();

                if (!string.IsNullOrEmpty(miFile))
                {
                    ucFileBaseDatos.uitxt = miFile;
                }
                else
                {
                    oTadil.data.UserInfo.procesoCancelado();
                }
                 
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);  
            }

        }

        private void lnkFileVpRpSelect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string miFile = oTadil.data.Files.getFileVpRp();

                if (!string.IsNullOrEmpty(miFile))
                {
                    ucFileNormasVpRadio.uitxt = miFile;
                }
                else
                {
                    oTadil.data.UserInfo.procesoCancelado();
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        private void lnkFileVpRpOpen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {

                if (!ucFileNormasVpRadio.isEmptyOrNull())
                {
                    frmRoad miFrm = new frmRoad(ucFileNormasVpRadio.uitxt,false);

                    if (miFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {  
                        ucFileNormasVpRadio.Text = miFrm.file;
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiNomFichNulo);
                }
          
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
          
        }


        private void lnkFileVpKvSelect_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string miFile = oTadil.data.Files.getFileVpKv();

                if (!string.IsNullOrEmpty(miFile))
                {
                    ucFileNormasVpKv.uitxt = miFile;
                }
                else
                {
                    oTadil.data.UserInfo.procesoCancelado();
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        private void lnkFileVpKvOpen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {

                if (!ucFileNormasVpKv.isEmptyOrNull())
                {
                    frmKv miFrm = new frmKv(ucFileNormasVpKv.uitxt);

                    if (miFrm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ucFileNormasVpKv.Text = miFrm.file;
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiNomFichNulo);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }

        #endregion
        #region "Eventos Formulario"

        private void frmFiles_Shown(object sender, EventArgs e)
        {
            isFilePathOk();
        }

        private void textbox_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            if (txtSender != null && txtSender.Text.Length > 0)
            {
                txtSender.SelectionStart = txtSender.Text.Length - 1;
            }
        }
        #endregion

    }
}
