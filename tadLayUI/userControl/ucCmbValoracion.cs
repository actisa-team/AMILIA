using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.userControl
{

    using System.Windows.Forms;
    using engNet.ClassT;

    /// <summary>
    /// Combo Valoración 0-10
    /// </summary>
    public class ucCmbValoracion: uclblCmbMaster
    {


        public ucCmbValoracion()
        :base()
        { 
        
        
        }


        public override void populate()
        {

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            List<oValDesT<int, string>> miLst = new List<oValDesT<int, string>>();


            for (int i = 0; i < 11; i++)
            {
                 miLst.Add(new oValDesT<int, string>(i,i.ToString()));
            }

            cmb.DataSource = miLst;
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;

        }


    }
}
