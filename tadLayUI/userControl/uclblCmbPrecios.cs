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
    using tadLayLogica.datos.precios;
    using tadLayShare;


    public class uclblCmbPrecios : uclblCmbMaster
    {

  


        #region "Constructor"
        public uclblCmbPrecios()
            :base()
        {


        }

  
        #endregion



     



     
 

        [Category("_CADnex_Data")]
        public string idGrupo {get;set;}
        [Category("_CADnex_Data")]
        public string idCode { get; set; }

        public override void populate()
        {

            //Valido los Datos
            if (string.IsNullOrEmpty(idGrupo)) 
            {
                throw new oExCmbFiltroNull(this.Name);
            }

                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                cmb.DisplayMember = "nombre";
                cmb.ValueMember = "idMaterial";
                cmb.DataSource = oDalMateriales.getLstMaterialesByGrupoAndClasificacion(idGrupo, idCode);

                cmb.SelectedIndex = -1;
        }


        public Guid idMaterial
        {
            get
            {
                return (Guid) cmb.SelectedValue;
            }
        }



   
    }


  


}
