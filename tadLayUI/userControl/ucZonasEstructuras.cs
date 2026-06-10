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
    
    
    public partial class ucZonasEstructuras : uclblCmbMaster
    {
        public ucZonasEstructuras()
        {
            InitializeComponent();
        }


        public override void populate()
        {
            uiLbl = strFrmGisEst.uiZonaDefault;

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.DisplayMember = "nombre";
            cmb.ValueMember = "id";
            cmb.DataSource = oDalTbEst.getTablaCache();

            cmb.SelectedIndex = -1;
        }
    }
}
