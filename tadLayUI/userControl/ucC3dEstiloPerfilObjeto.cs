using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI.userControl
{

    using tadLayLan;
    
    public partial class ucC3dEstiloPerfilObjeto : uclblCmbMaster
    {
        public ucC3dEstiloPerfilObjeto()
        {
            InitializeComponent();
        }

        public override void populate()
        {

            //cmb.DataSource = engCadNet.cv3d.oPerfilLongitudinal.getLstEstiloPerfilObjeto();
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;
        }

        private void ucC3dEstiloPerfilObjeto_Load(object sender, EventArgs e)
        {
            uiLbl = strFrmEstilosEjePerfil.uiEstiloPerfilObjeto;
        }
    }
}
