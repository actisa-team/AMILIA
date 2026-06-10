using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.userControl
{

    using tadLayLogica;
    using tadLayUI;
    using tadLayUI.Properties;
    using tadLayLan;

    using System.Threading;
    using System.Globalization;

    /// <summary>
    /// MENU TDB
    /// </summary>
    public partial class ucMenuNose : MenuStrip
    {
        #region "Variables Privadas"

        private ToolStripMenuItem mnuFile = null;

        private ToolStripMenuItem mnuNew = null;
        private ToolStripMenuItem mnuOpen = null;


        private ToolStripMenuItem mnuSaveAs = null;

        private ToolStripMenuItem mnuExit = null;

        public event EventHandler evNew;
        public event EventHandler evOpen;
        public event EventHandler evSaveAs;
        public event EventHandler evExit;
        //----HELP-----//

        private ToolStripMenuItem mnuHelp = null;
        private ToolStripMenuItem mnuHelpView = null;
        private ToolStripMenuItem mnuGuideView = null;
        private ToolStripMenuItem mnuAbout = null;

        #endregion


        #region "Constructor"

        public ucMenuNose()
        {
            InitializeComponent();

            populate();
        }

        #endregion
        #region "Metodos Privados"

        private void populate()
        {
            this.Dock = DockStyle.Top;

            #region "FILE"

            //MENU ARCHIVO
            mnuFile = new ToolStripMenuItem("mnuFile");
            mnuFile.Text = strGeneral.uiArchivo;

            //ITEM NUEVO
            mnuNew = new ToolStripMenuItem("mnuNew");
            mnuNew.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuNew.Image = Resources.BtnNew_Image;
            mnuNew.Text = strGeneral.uiNuevoOpen;
            mnuNew.Click += new EventHandler(mnuNew_Click);

            //ITEM ABRIR
            mnuOpen = new ToolStripMenuItem("mnuOpen");
            mnuOpen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuOpen.Image = Resources.BtnLoad_Image;
            mnuOpen.Text = strGeneral.uiAbrirOpen;
            mnuOpen.Click += new EventHandler(mnuOpen_Click);

            //ITEM SEPARATOR1
            ToolStripSeparator mnuSeparator1 = new ToolStripSeparator();
            ToolStripSeparator mnuSeparator2 = new ToolStripSeparator();



            //ITEM GUARDAR COMO
            mnuSaveAs = new ToolStripMenuItem("mnuSaveAs");
            mnuSaveAs.Name = "mnuSaveAs";
            mnuSaveAs.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuSaveAs.Image = Resources.BtnSave_Image;
            mnuSaveAs.Text = strGeneral.uiGuardarComoOpen;
            mnuSaveAs.Click += new EventHandler(mnuSaveAs_Click);

            //ITEM SALIR
            mnuExit = new ToolStripMenuItem("mnuExit");
            mnuExit.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuExit.Text = strGeneral.uiSalirOpen;
            mnuExit.Click += new EventHandler(mnuExit_Click);

            //COMPOSITE
            this.Items.Add(mnuFile);
            mnuFile.DropDownItems.Add(mnuNew);
            mnuFile.DropDownItems.Add(mnuOpen);
            mnuFile.DropDownItems.Add(mnuSeparator1);
            mnuFile.DropDownItems.Add(mnuSaveAs);
            mnuFile.DropDownItems.Add(mnuSeparator2);
            mnuFile.DropDownItems.Add(mnuExit);

            #endregion
            #region "Ayuda"

            //------------HELP-------------------//
            mnuHelp = new ToolStripMenuItem("mnuHelp");
            mnuHelp.Text = strGeneral.uiAyuda;

            //HELP VIEW
            mnuHelpView = new ToolStripMenuItem("mnuAyudaVer");
            mnuHelpView.Name = "mnuAyudaVer";
            mnuHelpView.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuHelpView.Image = Resources.help16x16;
            mnuHelpView.Text = strGeneral.uiManualVer;
            mnuHelpView.Click += new EventHandler(mnuHelpView_Click);

            //GUIDE VIEW
            mnuGuideView = new ToolStripMenuItem("mnuGuideVer");
            mnuGuideView.Name = "mnuGuideVer";
            mnuGuideView.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuGuideView.Image = Resources.help16x16;
            mnuGuideView.Text = strGeneral.uiGuiaVer;
            mnuGuideView.Click += new EventHandler(mnuGuideView_Click);

            //ABOUT
            mnuAbout = new ToolStripMenuItem("mnuAcercaDe");
            mnuAbout.Name = "mnuAcercaDe";
            mnuAbout.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuAbout.Image = Resources.about16x16;
            mnuAbout.Text = strGeneral.uiAcercaDeTadil;
            mnuAbout.Click += new EventHandler(mnuAbout_Click);

            //COMPOSITE
            this.Items.Add(mnuHelp);
            mnuHelp.DropDownItems.Add(mnuHelpView);
            mnuHelp.DropDownItems.Add(mnuGuideView);
            mnuHelp.DropDownItems.Add(mnuAbout);

            #endregion

        }

 



        #endregion
        #region "Propiedades"

       

        public void mnuSaveAsEnable(bool iIsenabled)
        {
           mnuSaveAs.Enabled = iIsenabled;
        }

        #endregion
        #region "Eventos Menu FILE"

        /// <summary>
        /// NEW
        /// </summary>
        void mnuNew_Click(object sender, EventArgs e)
        {
            if (evNew != null)
            {
                evNew(null, new EventArgs());
            }
        }

        /// <summary>
        /// OPEN
        /// </summary>
        public virtual void mnuOpen_Click(object sender, EventArgs e)
        {

            if (evOpen != null)
            {
                evOpen(null, new EventArgs());
            }
        }



        /// <summary>
        /// SAVE AS
        /// </summary>
        void mnuSaveAs_Click(object sender, EventArgs e)
        {
            if (evSaveAs != null)
            {
                evSaveAs(null, new EventArgs());
            }
        }

        /// <summary>
        /// EXIT
        /// </summary>
        void mnuExit_Click(object sender, EventArgs e)
        {
            if (evExit != null)
            {
                evExit(null, new EventArgs());
            }
        }







 


        #endregion
        #region "Eventos Menu HELP"
        /// <summary>
        /// VER AYUDA
        /// </summary>
        void mnuHelpView_Click(object sender, EventArgs e)
        {
            try
            {
                tadLayLogica.Comandos.oComandoHelpView.openFileHelp();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        /// <summary>
        /// VER GUIA
        /// </summary>
        void mnuGuideView_Click(object sender, EventArgs e)
        {
            try
            {
                tadLayLogica.Comandos.oComandoHelpView.openFileGuide();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        /// <summary>
        /// VER ABOUT
        /// </summary>
        void mnuAbout_Click(object sender, EventArgs e)
        {
            try
            {
                frmAbout miFrmAbout = new frmAbout();
                miFrmAbout.ShowDialog();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        #endregion
    }
}
