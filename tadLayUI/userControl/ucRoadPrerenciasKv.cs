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
    
    
    public partial class ucRoadPrerenciasKv : uclblCmbMaster
    {
        #region "Constructores"

        public ucRoadPrerenciasKv()
        {
            InitializeComponent();
        }

        #endregion
        #region "Propiedades"

        public eKvPrefer valor
        {

            get
            {
                if (this.cmb.SelectedIndex != -1)
                {
                    return (eKvPrefer)this.cmb.SelectedValue;
                }
                else
                {
                    throw new oExPropertieNullValue("cmbPreferenciasKV");
                }
            }
        }

        #endregion
        #region "Metodos"
        public override void populate()
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            List<oValDesT<eKvPrefer, string>> miLst = new List<oValDesT<eKvPrefer, string>>();

            miLst.Add(new oValDesT<eKvPrefer, string>(eKvPrefer.minimo, strFrmSolucion.uiRoadKvMinimo));
            miLst.Add(new oValDesT<eKvPrefer, string>(eKvPrefer.deseable, strFrmSolucion.uiRoadKvDeseable));

            cmb.DataSource = miLst;
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;
        }
        #endregion
        #region "Eventos"

        private void ucRoadPrerenciasKv_Load(object sender, EventArgs e)
        {
            uiLbl = strFrmSolucion.uiRoadPreferenciasKv;
        }

        #endregion
    }
}
