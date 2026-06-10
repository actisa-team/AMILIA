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


    using engNet.ClassT;
    using tadLayLan;
    using tadLayLogica;
    using tadLayData;

    
    
    public  class uclblSiNo : uclblCmbMaster
    {


        
        /// <summary>
        /// AUTOPOPULATE
        /// </summary>
        public uclblSiNo()
            :base()
        {
            populate();
        }




        public bool valor
        {

            get
            {
                if (cmb.SelectedValue != null)
                {
                   return  (bool)cmb.SelectedValue;
                }
                else
                {
                    return false;

                }
                    
                    
                   
            }

            
        }
        public bool ValorSeleccionado
        {
            get { return valor; }
            set { cmb.SelectedValue = value; }
        }
        public override void populate()
        {

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            List<oValDesT<bool, string>> miLst = new List<oValDesT<bool, string>>();

            miLst.Add(new oValDesT<bool, string>(true, strGeneral.uiSi));
            miLst.Add(new oValDesT<bool, string>(false, strGeneral.uiNo));

            cmb.DataSource = miLst;
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;

        }



 



    }


}
