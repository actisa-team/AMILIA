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
    
    
    public partial class ucC3dEstiloPerfilEtiquetas : uclblCmbMaster
    {
        public ucC3dEstiloPerfilEtiquetas()
        {
            InitializeComponent();
        }


        public override void populate()
        {

            //cmb.DataSource = engCadNet.cv3d.oPerfilLongitudinal.getLstEstiloPerfilEtiquetas();
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;
        }

        private void ucC3dEstiloPerfilEtiquetas_Load(object sender, EventArgs e)
        {
            uiLbl = strFrmEstilosEjePerfil.uiEstiloPerfilEtiquetas;
        }
    }
}
