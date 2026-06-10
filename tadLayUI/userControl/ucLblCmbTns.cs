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
    using tadLayData;



    public class ucLblCmbTns : uclblCmbMaster
    {      
        public ucLblCmbTns()
            :base()
        {
            populate();
        }

 
        public override void populate()
        {

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            List<string> miLst = new List<string>();

           
            miLst.Add("SIN");
            miLst.Add("S00");
            miLst.Add("S0");
            miLst.Add("S1");
            miLst.Add("S2");
            miLst.Add("S3");
            miLst.Add("S4");
            miLst.Add("S-EST1");
            miLst.Add("S-EST2");
            miLst.Add("S-EST3");
            miLst.Add("Z");
            miLst.Add("ROCA");
            miLst.Add("P");
            miLst.Add("TU");


            cmb.DataSource = miLst;

            cmb.SelectedIndex = -1;       
        }
    }



}
