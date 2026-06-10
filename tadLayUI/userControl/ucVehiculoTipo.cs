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
    using tadLayLogica;
    using tadLayData;
  
    
    
    public  class ucVehiculoTipo : uclblCmbMaster
    {


        
        /// <summary>
        /// AUTOPOPULATE
        /// </summary>
        public ucVehiculoTipo()
            :base()
        {
            populate();
        }






        public string valor
        {

            get
            {
                return (string)cmb.SelectedValue;
            }

            
        }




        public override void populate()
        {

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            List<oValDesT<string, string>> miLst = new List<oValDesT<string, string>>();

            miLst.Add(new oValDesT<string, string>("VEHLIG", strFrmRentabilidad.uiVehiculoLigero));
            miLst.Add(new oValDesT<string, string>("VEHPES", strFrmRentabilidad.uiVehiculoPesado));

            cmb.DataSource = miLst;
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = 0;

        }



        private void ucVehiculoTipo_Load(object sender, EventArgs e)
        {
            lbl.Text = strFrmRentabilidad.uiVehiculoTipo;


        }



 



    }


}
