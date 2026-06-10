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
    using tadLayUI.Properties;
    using tadLayLan;

    /// <summary>
    /// MENU NEW OPEN / SAVE SAVEAS / EXIT
    /// </summary>
    public partial class ucMenuTDI : MenuStrip
    {
        #region "Variables Privadas"

        //--FILE--//
        private ToolStripMenuItem mnuFile = null;
        private ToolStripMenuItem mnuNewEstudioPrevio = null;
        private ToolStripMenuItem mnuNewEstudioInformativo = null;
        private ToolStripMenuItem mnuOpen = null;
        private ToolStripMenuItem mnuSaveAs = null;
        private ToolStripMenuItem mnuExit = null;

        public event EventHandler evNewEstudioPrevio;
        public event EventHandler evNewEstudioInformativo;
        public event EventHandler evOpen;
        public event EventHandler evSaveAs;
        public event EventHandler evExit;

        //----AYUDA-----//
        private ToolStripMenuItem mnuHelp = null;
        private ToolStripMenuItem mnuHelpView = null;
        private ToolStripMenuItem mnuGuideView = null;
        private ToolStripMenuItem mnuAbout = null;

       


        #endregion
        #region "Constructor"

        public ucMenuTDI()
        {
            InitializeComponent();

            populate();
        }

        #endregion
        #region "MetodosPrivados"

        private void populate()
        {
            this.Dock = DockStyle.Top;

            //MENU ARCHIVO
            mnuFile = new ToolStripMenuItem("mnuFile");
            mnuFile.Text = strGeneral.uiArchivo;

            //ITEM NUEVO - ESTUDIO PREVIO
            mnuNewEstudioPrevio = new ToolStripMenuItem("mnuNewEstudioPrevio");
            mnuNewEstudioPrevio.Name = "mnuNewEstudioPrevio";
            mnuNewEstudioPrevio.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuNewEstudioPrevio.Image = Resources.BtnNew_Image;
            mnuNewEstudioPrevio.Text = strGeneral.uiNewEstudioPrevio;
            mnuNewEstudioPrevio.Click += new EventHandler(mnuNewEstudioPrevio_Click);

            //ITEM NUEVO - ESTUDIO INFORMATIVO
            mnuNewEstudioInformativo = new ToolStripMenuItem("mnuNewEstudioInformativo");
            mnuNewEstudioInformativo.Name = "mnuNewEstudioInformativo";
            mnuNewEstudioInformativo.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuNewEstudioInformativo.Image = Resources.BtnNew_Image;
            mnuNewEstudioInformativo.Text = strGeneral.uiNewEstudioInformativo;
            mnuNewEstudioInformativo.Click += new EventHandler(mnuNewEstudioInformativo_Click);

            //ITEM ABRIR
            mnuOpen = new ToolStripMenuItem("mnuOpen");
            mnuOpen.Name = "mnuOpen";
            mnuOpen.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            mnuOpen.Image = Resources.BtnLoad_Image;
            mnuOpen.Text = strGeneral.uiAbrirOpen;
            mnuOpen.Click += new EventHandler(mnuOpen_Click);

            //ITEM SEPARATOR1
            ToolStripSeparator mnuSeparator1 = new ToolStripSeparator();
            ToolStripSeparator mnuSeparator2 = new ToolStripSeparator();
            ToolStripSeparator mnuSeparator3 = new ToolStripSeparator();




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
            mnuFile.DropDownItems.Add(mnuNewEstudioPrevio);
            mnuFile.DropDownItems.Add(mnuNewEstudioInformativo);
            mnuFile.DropDownItems.Add(mnuSeparator1);
            mnuFile.DropDownItems.Add(mnuOpen);
            mnuFile.DropDownItems.Add(mnuSeparator2);
            mnuFile.DropDownItems.Add(mnuSaveAs);
            mnuFile.DropDownItems.Add(mnuSeparator3);
            mnuFile.DropDownItems.Add(mnuExit);


            //---------AYUDA-------------------//
            mnuHelp = new ToolStripMenuItem("mnuHelp");
            mnuHelp.Text = strGeneral.uiAyuda;


            //MANUAL VIEW
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

        }

   

 
        #endregion
        #region "Metodos Publicos"

       

        public void enableMnuSaveAs(bool iIsenabled)
        {
           mnuSaveAs.Enabled = iIsenabled;
        }

        public void enableMnuEstudioPrevio(bool iIsEnabled)
        {
            mnuNewEstudioPrevio.Enabled = iIsEnabled;
            mnuNewEstudioInformativo.Enabled = !iIsEnabled;
        }

        public void enableMnuEstudioInformativo(bool iIsEnabled)
        {
            mnuNewEstudioPrevio.Enabled = ! iIsEnabled;
            mnuNewEstudioInformativo.Enabled = iIsEnabled;
        }

        #endregion
        #region "Eventos Menu FILE"

        /// <summary>
        /// NEW ESTUDIO PREVIO
        /// </summary>
        void mnuNewEstudioPrevio_Click(object sender, EventArgs e)
        {
            if (evNewEstudioPrevio != null)
            {
                evNewEstudioPrevio(null, new EventArgs());
            }
        }

        /// <summary>
        /// NEW ESTUDIO INFORMATIVO
        /// </summary>
        void mnuNewEstudioInformativo_Click(object sender, EventArgs e)
        {
            if (evNewEstudioInformativo != null)
            {
                evNewEstudioInformativo(null, new EventArgs());
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
        /// VER MANUAL USUARIO
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
        /// VER GUIDE
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
        /// 
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
