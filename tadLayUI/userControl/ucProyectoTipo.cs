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
    using tadLayLogica.datos.proyecto;
    
    public partial class ucProyectoTipo : uclblCmbMaster
    {
        public ucProyectoTipo()
        {
            InitializeComponent();
        }

                    
        public override void  populate()
        {

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            List<oValDesT<string, string>> miLst = new List<oValDesT<string, string>>();

            miLst.Add(new oValDesT<string, string>("ESTPRE", strFrmDatosProyecto.uiESTPRE));
            miLst.Add(new oValDesT<string, string>("ESTINF", strFrmDatosProyecto.uiESTINF));

            cmb.DataSource = miLst;
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;

        }

        private void ucProyectoTipo_Load(object sender, EventArgs e)
        {
            uiLbl = strFrmDatosProyecto.uiTipoEstudio;
        }
        
        
 


    }
}
