using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLogica.datos;

namespace tadLayUI.userControl
{

    using engNet.ClassT;
    using tadLayLogica;
    using tadLayLan.Tdm;
    
    
    public partial class ucTerrain : uclblCmbMaster
    {
        public ucTerrain()
        {
            InitializeComponent();
        }

        public void AddElement(string val, string des)
        {
            var element = new oValDesT<string, string>(val, des);
            var list = oSubDMesh.getLstSuperficies(oTadil.IsDemo);
            if (!list.Contains(element))
                list.Add(element);
            cmb.DataSource = list;
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;

        }




        public override void populate()
        {
            if(oTadil.IsDemo)
            {
                oTadil.data.UserInfo.showInfo(strFormTdm.dibujandoTerreno);
            }
            cmb.DataSource = oSubDMesh.getLstSuperficies(oTadil.IsDemo); 
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;

         }
        public void populate(string buscado)
        {
            if (oTadil.IsDemo)
            {
                oTadil.data.UserInfo.showInfo(strFormTdm.dibujandoTerreno);
            }
            cmb.DataSource = oSubDMesh.getLstSuperficies(oTadil.IsDemo, buscado);
            cmb.ValueMember = "val";
            cmb.DisplayMember = "des";

            cmb.SelectedIndex = -1;

        }


    }
}
