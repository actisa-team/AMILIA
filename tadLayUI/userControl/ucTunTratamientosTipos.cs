using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.userControl
{

    using tadLayLogica.datos.Gis;
    using System.Windows.Forms;
    
    public  class ucTunTratamientosTipos : uclblCmbMaster
    {

       public ucTunTratamientosTipos()
       :base()
       { 
       
       
       }


       public override void populate()
       {
           cmb.DropDownStyle = ComboBoxStyle.DropDownList;
           cmb.DisplayMember = "des";
           cmb.ValueMember = "val";
           cmb.DataSource = oDalTbTun.getTratamientosMetodos();

           cmb.SelectedIndex = -1;

       }

    }
}
