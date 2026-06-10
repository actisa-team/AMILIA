using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI
{

    using tadLayLogica;
    using tadLayLogica.datos.proyecto;
    using tadLayUI.userControl;
    
    public partial class frmRoot : Form
    {

        protected List<ucLblTxt> mLstTxtPC100 = new List<ucLblTxt>();

          
        public frmRoot()
        {
            InitializeComponent();
        }


        protected bool suma100(ref string iGrNotSum100)
        {

            if (mLstTxtPC100.Count == 0)
            {
                throw new Exception("La matriz de Controles PorCiento es Nula");
            }



            //Agrupo los PorCientos por Grupo
            var miQuery = from p in mLstTxtPC100
                          where p.Enabled == true
                          group p by new { p.Parent.Name } into g
                          select new { nombre = g.First().Parent.Text, porCien = g.Sum(p => p.valorDouble) };



            foreach (var item in miQuery)
            {
                if (item.porCien != 100)
                {
                    iGrNotSum100 = (string.Format("{0} - Suma Porcentajes {1}%", item.nombre, item.porCien)) + "\n" + iGrNotSum100;
                }
            }

            if (string.IsNullOrEmpty(iGrNotSum100))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        protected virtual bool isValidoFrm
        {
            get
            {
                return oValidar.isValidoGrupoByFrm(this);              
            }
        }

        protected oSingletonDsApp ds
        {
            get
            {
                return oSingletonDsApp.getInstance;
            }
        }



    }
}
