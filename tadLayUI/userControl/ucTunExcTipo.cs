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
    using tadLayLogica;
    using tadLayLogica.datos.Gis;

    /// <summary>
    /// TIPOS DE EXCAVACIÓN DE TUNELES
    /// </summary>
    public  class ucTunExcTipos : uclblCmbMaster
    {

        #region "Constructor"
        public ucTunExcTipos()
            :base()
        {

        }
        #endregion

        public override void  populate()
        {
                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb.DisplayMember = "des";
                cmb.ValueMember = "val";
                cmb.DataSource = oDalTbTun.getExcavacionMetodos();

                cmb.SelectedIndex = -1;
        }

    }


  


}
