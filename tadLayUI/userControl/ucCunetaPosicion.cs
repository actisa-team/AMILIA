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
    using tadLayLogica;
    using tadLayData;

    using engNet.ClassT;

    public class ucCunetaPosicion : uclblCmbMaster
    {      
        
        
        
        
        
        public ucCunetaPosicion()
            :base()
        {
         
        }


        public override string uiLbl
        {
            get
            {
                return lbl.Text;
            }
            set
            {
                    if (string.IsNullOrEmpty(value))
                        lbl.Text = strFrmSecciones.uiCunetaPosicion;
                    else
                        lbl.Text = value;
            }
        }






        public override sealed void populate()
        {

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            List<oValDesT<string, string>> miLst = new List<oValDesT<string, string>>();

            miLst.Add(new oValDesT<string, string>(eCunetaPosicion.firme.ToString(), strFrmSecciones.uiCunetaPosicionFirme));
            miLst.Add(new oValDesT<string, string>(eCunetaPosicion.berma.ToString(), strFrmSecciones.uiCunetaPosicionBerma));
           

            cmb.DataSource = miLst;
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;
    
        }


    


    }



}
