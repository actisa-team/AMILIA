using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using engCadNet;
using engNet.ClassT;

namespace tadLayUI.userControl
{
    public partial class ucCmbLayersTodas : uclblCmbMaster
    {
        public ucCmbLayersTodas()
            :base()
        {
        }


        public override void populate()
        {

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.DisplayMember = "nombre";

            List<string> miCapas = oLayer.getLayerListStartsWith("", false);
            List<oValDesT<string, string>> miData = new List<oValDesT<string, string>>();

            foreach (string miCapa in miCapas)
            {
                //por que me sale valor: (nombreCapa) des: ??? [DUDA]
                oValDesT<string, string> miNuevaCapa = new oValDesT<string, string>(miCapa, miCapa);
                miData.Add(miNuevaCapa);

            }


            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";
            cmb.DataSource = miData;

            cmb.SelectedIndex = -1;
        }



        public string valor
        {

            get
            {
                return (string)cmb.SelectedValue;

            }
        }
    }
}
