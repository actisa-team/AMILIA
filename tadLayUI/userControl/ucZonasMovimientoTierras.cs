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
    
    
    public partial class ucZonasMovimientoTierras : uclblCmbMaster
    {
        public ucZonasMovimientoTierras()
        {
            InitializeComponent();
            postConstructor();
        }




        public override void populate()
        {
            uiLbl = strFrmGisGeo.uiZonaDefault;
           
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.DisplayMember = "nombre";
            cmb.ValueMember = "id";
            cmb.DataSource = oDalGeoTbZonas.getTablaCache(); 

            cmb.SelectedIndex = -1;
        }
    }
}
