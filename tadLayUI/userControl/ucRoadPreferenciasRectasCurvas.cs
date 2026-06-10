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

    using tadLayLogica;
    using tadLayLan;
    using engNet.ClassT;
    using tadLayShare;
    using tadLayLan.Tdi;
    
    
    public partial class ucRoadPreferenciasRectasCurvas : uclblCmbMaster
    {
        public ucRoadPreferenciasRectasCurvas()
        {
            InitializeComponent();
        }




        public eRoadPreferencias valor
        {

            get
            {
                if (this.cmb.SelectedIndex != -1)
                {
                    return (eRoadPreferencias)this.cmb.SelectedValue;
                }
                else
                {
                    throw new oExPropertieNullValue("cmbPreferencias");
                }
            }
        }



        public override void populate()
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            List<oValDesT<eRoadPreferencias, string>> miLst = new List<oValDesT<eRoadPreferencias, string>>();

            miLst.Add(new oValDesT<eRoadPreferencias, string>(eRoadPreferencias.rectas, strFrmSolucion.uiRoadPreferenciasRectas));
            miLst.Add(new oValDesT<eRoadPreferencias, string>(eRoadPreferencias.curvas, strFrmSolucion.uiRoadPreferenciasCurvas));

            cmb.DataSource = miLst;
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;
        }



        private void ucRoadPreferenciasRectasCurvas_Load(object sender, EventArgs e)
        {
            uiLbl = strFrmSolucion.uiRoadPreferencias;
        }

    }
}
