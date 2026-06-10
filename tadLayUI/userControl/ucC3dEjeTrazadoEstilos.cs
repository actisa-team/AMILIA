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

    using engNet.ClassT;
    using tadLayLan;
    
    public partial class ucC3dEjeTrazadoEstilos : uclblCmbMaster
    {
        public ucC3dEjeTrazadoEstilos()
        {
            InitializeComponent();
        }


        public override void populate()
        {

            //cmb.DataSource = engCadNet.cv3d.oAlignment.getListadoEjeTrazadoEstilo();
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;
        }

        private void ucC3dEjeTrazadoEstilos_Load(object sender, EventArgs e)
        {
            uiLbl = strFrmEstilosEjePerfil.uiEstiloEjeTrazado;
        }
    }
}
