using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.userControl
{

    using tadLayLan;
    using tadLayLogica.datos.Gis;
    
    
    public partial class ucZonasTuneles : uclblCmbMaster
    {
        public ucZonasTuneles()
        {
            InitializeComponent();
        }


        public override void populate()
        {
            uiLbl = strFrmGisTun.uiZonaDefault;

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.DisplayMember = "nombre";
            cmb.ValueMember = "id";
            cmb.DataSource = oDalTbTun.getTabla();

            cmb.SelectedIndex = -1;
        }
    }
}
