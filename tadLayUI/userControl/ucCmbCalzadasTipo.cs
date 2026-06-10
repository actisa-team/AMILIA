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
    using tadLayShare;


    public class ucCmbCalzadasTipo : uclblCmbMaster
    {

  


        #region "Constructor"
        public ucCmbCalzadasTipo()
            :base()
        {


        }

  
        #endregion

        [Category("_CADnex_Data")]
        public string codeRoad { get; set; }


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

        if (string.IsNullOrEmpty(codeRoad))
        { 
        
        
        }
        else if (codeRoad == "CALUNI")
        {
       
            miLst.Add(new oValDesT<string,string>("UNIGEN",strFrmSecciones.uiUNIGEN));
        }
        else if (codeRoad == "CALDOB")
        {
            miLst.Add(new oValDesT<string, string>("DOBAUT", strFrmSecciones.uiDOBAUT));   
            miLst.Add(new oValDesT<string, string>("DOBSIN", strFrmSecciones.uiDOBSIN));
            //miLst.Add(new oValDesT<string, string>("DOBURB", strFrmSecciones.uiDOBURB));
        }
        else
        {
            throw new oExEnumNotImplemented(codeRoad);
        }

       
        cmb.DropDownStyle = ComboBoxStyle.DropDownList;
        cmb.ValueMember = "val";
        cmb.DisplayMember = "des";
        cmb.DataSource = miLst;
        cmb.SelectedIndex = -1;
   

        }
    }


  


}
