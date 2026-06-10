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

    using engNet.ClassT;

    using tadLayLan;
    using tadLayLogica;
    using tadLayLogica.datos.precios;


    public class ucCmbCalzadasGrupo : uclblCmbMaster
    {

  


        #region "Constructor"
        public ucCmbCalzadasGrupo()
            :base()
        {


        }

  
        #endregion



        public string valor
        {

            get
            {
                return (string)cmb.SelectedValue;
             
            }    
        }


     
        public override void populate()
        {


        List<oValDesT<string, string>> miLst = new List<oValDesT<string, string>>();

        miLst.Add(new oValDesT<string,string>("CALUNI",strFrmSecciones.uiCalzadaUnica));
        miLst.Add(new oValDesT<string, string>("CALDOB", strFrmSecciones.uiCalzadaDoble));
               
        cmb.DropDownStyle = ComboBoxStyle.DropDownList;
        cmb.ValueMember = "val";
        cmb.DisplayMember = "des";
        cmb.DataSource = miLst;

        cmb.SelectedIndex = -1;


        }






   
    }


  


}
