using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.userControl
{

    using tadLayLan;
    
    public partial class ucToolDgv : UserControl
    {
          
        public ucToolDgv()
        {
            InitializeComponent();
        }

        #region "Propiedades"

        [Category("_CADnex_SetUp")]
        public bool isNewEnable {get;set;}


        [Category("_CADnex_SetUp")]
        public bool isEditEnable { get; set; }



        [Category("_CADnex_SetUp")]
        public bool isEraseEnable { get; set; }


        [Category("_CADnex_SetUp")]
        public bool isEraseAllEnable { get; set; }
  


        #endregion

        #region "Eventos"


        private void ucToolDgv_Load(object sender, EventArgs e)
        {

            //Traduccion

            lnkNew.Text = strGeneral.uiNuevo;
            lnkEdit.Text = strGeneral.uiEditar;
            lnkErase.Text = strGeneral.uiBorrar;
            lnkEraseAll.Text = strGeneral.uiBorrarListado;

           
            if (!isNewEnable) { lnkNew.Enabled = false; }
            if (!isEditEnable) {lnkEdit.Enabled = false; }
            if (!isEraseEnable) {lnkErase.Enabled = false; }
            if (!isEraseAllEnable) { lnkEraseAll.Enabled = false; }
        }

        #endregion




    }
}
