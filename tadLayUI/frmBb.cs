using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI
{

    using System.IO;

    using engNet.eventos;
    using tadLayUI.adminGis;
    using tadLayUI.adminPresupuesto;
    using tadLayUI.adminSecciones;
    using tadLayUI.adminMacroPrecios;
    using tadLayLogica;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLan;

    using System.Globalization;
    using System.Threading;
    using tadLayUI.adminAprov;

    /// <summary>
    /// CONSTRUIR frmBd,getInstance
    /// </summary>
    public partial class frmBb : Form
    {


        //Crear solo una Instancia
        private static frmBb mInstance = null;

        private ToolStripButton[] mLstBtnIzq;



        #region "Constructor"
        private frmBb(string iFileBd)
        {
            InitializeComponent();
            postConstructor();
            Files_evFileBbddUpdate(null,new oEventArgs<string>(iFileBd));
        }
        private frmBb()
        { 
            InitializeComponent();
            postConstructor();
            create();
        }
        #endregion


        //Metodo Llamar al Formulario
        public static frmBb getInstance
        {

            get
            {
                if (mInstance == null)
                {
                    mInstance = new frmBb();
                }
                else
                {
                    mInstance.WindowState = FormWindowState.Normal;
                    mInstance.StartPosition = FormStartPosition.CenterParent;
                    mInstance.Focus();
                }

                return mInstance;
            }
        }


        public static frmBb editInstance(string iFileBd)
        {

            if (mInstance == null | oTadil.data.Files.fileBbdd != iFileBd)
            {
                oTadil.data.Files.fileBbdd = iFileBd;
                mInstance = new frmBb(iFileBd);
            }
            else
            {
                mInstance.WindowState = FormWindowState.Normal;
                mInstance.StartPosition = FormStartPosition.CenterParent;
                mInstance.Focus();
            }

            return mInstance;
        }






        #region "Metodos Privados"

        private void postConstructor()
        {
            // Prepare the shape tools.
            mLstBtnIzq = new ToolStripButton[] { btnPrecios, btnGis,btnSec,btnMacroPrecios,btnapro};

            oTadil.data.Files.evFileBbddUpdate += new EventHandler<oEventArgs<string>>(Files_evFileBbddUpdate);

            ucMenu.evNew += new EventHandler(ucMenu_New);
            ucMenu.evOpen += new EventHandler(ucMenu_Open);
            ucMenu.evSaveAs += new EventHandler(ucMenu_SaveAs);
            ucMenu.evExit += new EventHandler(ucMenu_Exit);



            //Traduccion
            var name = oTadil.KAppHeaderName;
            this.Text = name + " - " + strFrmBb.uiAdminBd;
            btnPrecios.Text = strFrmBb.uiPRE;
            btnGis.Text = strFrmBb.uiSIG;
            btnMacroPrecios.Text = strFrmBb.uiMacroPrecios;
            btnSec.Text = strFrmBb.uiSEC;
            lblFooter.Text = string.Empty;
            //btnapro.Text = strFrmBb.uiAdec;

        }




        private void create()
        {   
            ucMenu.mnuSaveAsEnable(false);

            foreach (ToolStripButton item in mLstBtnIzq)
            {
                item.Enabled = false;
            }

            lblFooter.Text = strGeneralUser.uiFileBbSelect;
            lblFooter.ForeColor = Color.Red;
        }

        #endregion

        #region "EVENTOS FILE DATABASE"
        //ACTUALIZO EL FICHERO
        void Files_evFileBbddUpdate(object sender, oEventArgs<string> e)
        {
  
            //Descargo la imagen de fondo
            panel1.BackgroundImage = null;

            lblFooter.Text = e.Value;
            lblFooter.ForeColor = Color.Green;

            ucMenu.mnuSaveAsEnable(true);


            foreach (ToolStripButton item in mLstBtnIzq)
            {
                item.Enabled = true;
            }

            btnPrecios_Click(btnPrecios, new EventArgs());


            var name = oTadil.KAppHeaderName;
            this.Text = name + " - " + strFrmBb.uiAdminBd + " - " + oSingletonDsBd.getInstance.version;

        }
        #endregion

        #region "EVENTOS MENU IZQ"

        // Select the indicated button and deselect the others.
        private void SelectToolStripButton(ToolStripButton selected_button, ToolStripButton[] buttons)
        {
            selected_button.BackColor = SystemColors.ActiveCaption;

            foreach (ToolStripButton test_button in buttons)
            {

                if (selected_button == test_button)
                {
                    selected_button.BackColor = SystemColors.ActiveCaption;
                }
                else
                {
                    test_button.BackColor = SystemColors.Control;
                   
                }
            }

        }

        #endregion

        #region "EVENTOS MENU"
        //NEW
        void ucMenu_New(object sender, EventArgs e)
        {
            try
            {

                string miFile = oTadil.data.Files.selectNewFileBaseDatos(); 

                if (!string.IsNullOrEmpty(miFile))
                {
                    oTadil.data.Files.fileBbdd = miFile;
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eProcesoCancelado);
                }
           
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        //OPEN
        void ucMenu_Open(object sender, EventArgs e)
        {
            try
            {
                string miFile = oTadil.data.Files.getFileBbdd();

                if (!string.IsNullOrEmpty(miFile))
                {
                    oTadil.data.Files.fileBbdd = miFile;
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eProcesoCancelado); 
                }
  
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        //SAVE
        void ucMenu_Save(object sender, EventArgs e)
        {
            try
            {
                oSingletonDsBd.getInstance.save(true);  
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        //SAVEAS
        void ucMenu_SaveAs(object sender, EventArgs e)
        {

            try
            {
                string miFile = oTadil.data.Files.saveAsFileBbdd();

                if (!string.IsNullOrEmpty(miFile))
                {
                    oTadil.data.Files.fileBbdd = miFile;
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eProcesoCancelado);
                }                        
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        //EXIT
        void ucMenu_Exit(object sender, EventArgs e)
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

        #region "EVENTOS FRM"

        private void frmBb_Shown(object sender, EventArgs e)
        {
            oTadil.data.setIdiomaAplicacion();
        }

        private void frmBb_FormClosed(object sender, FormClosedEventArgs e)
        {
            oSingletonDsBd.deleteInstance();
            frmBb.mInstance = null;
            oTadilSwitch.hasInstanceTDB = false;
            base.OnClosed(e);
        }

        #endregion

        #region "Botones IZQUIERDA"
        //PRECIOS
        private void btnPrecios_Click(object sender, EventArgs e)
        {
            try
            {
                oSingletonDsBd.deleteInstance();
                
                SelectToolStripButton(sender as ToolStripButton, mLstBtnIzq);

                panel1.Controls.Clear();

                frmManagerPrecios miFrm = new frmManagerPrecios();
                miFrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                miFrm.TopLevel = false;
                panel1.Controls.Add(miFrm);

                miFrm.display();

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        //GIS
        private void btnGis_Click(object sender, EventArgs e)
        {
            try
            {

                oSingletonDsBd.deleteInstance();
                
                SelectToolStripButton(sender as ToolStripButton, mLstBtnIzq);

                panel1.Controls.Clear();

                frmManagerGis miFrm = new frmManagerGis();
                miFrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                miFrm.TopLevel = false;
                panel1.Controls.Add(miFrm);
                miFrm.display();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        //MACRO PRECIOS
        private void btnMacroPrecios_Click(object sender, EventArgs e)
        {

            try
            {

                oSingletonDsBd.deleteInstance();
                
                SelectToolStripButton(sender as ToolStripButton, mLstBtnIzq);

                panel1.Controls.Clear();

                frmMacroPreciosManager miFrm = new frmMacroPreciosManager();
                miFrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                miFrm.TopLevel = false;
                panel1.Controls.Add(miFrm);
                miFrm.display();
            }
            catch (Exception ex)
            {
                 oTadil.data.UserInfo.showError(ex);
            }

        }
        //SECCIONES
        private void btnSec_Click(object sender, EventArgs e)
        {
            try
            {
                oSingletonDsBd.deleteInstance();

               SelectToolStripButton(sender as ToolStripButton, mLstBtnIzq);
               panel1.Controls.Clear();

                frmManagerSecciones miFrm = new frmManagerSecciones();
                miFrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                miFrm.TopLevel = false;
                panel1.Controls.Add(miFrm);
                miFrm.display();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }




        #endregion

        private void btnapro_Click(object sender, EventArgs e)
        {
            try
            {
                oSingletonDsBd.deleteInstance();

                SelectToolStripButton(sender as ToolStripButton, mLstBtnIzq);
                panel1.Controls.Clear();

                frmManagerAprov miFrm = new frmManagerAprov();
                miFrm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                miFrm.TopLevel = false;
                panel1.Controls.Add(miFrm);
                miFrm.display();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
    }
}
