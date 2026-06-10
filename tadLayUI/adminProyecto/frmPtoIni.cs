using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminProyecto
{


    using tadLayShare.puntos;
    using tadLayLogica.Comandos;
    
    public partial class frmPtoIni : Form
    {

        private int mIdPto;

        
        public frmPtoIni(int iIdPto)
        {
            InitializeComponent();

            mIdPto = iIdPto;

            postConstrutor();
        }


        private void postConstrutor()
        {
            #region "SetUp Objetos"
            ucPtoIni.evHideFrm += new EventHandler<engNet.eventos.oEventArgs<bool>>(ucPtoIni_evHideFrm);
            ucPtoIni.populate(mIdPto);
            #endregion
        }

        void ucPtoIni_evHideFrm(object sender, engNet.eventos.oEventArgs<bool> e)
        {
            if (e.Value)
            {

                frmAppManager.getInstance.Hide();

            }
            else
            {
                frmAppManager.getInstance.Show();       
            }

        }

        private void ucPtoIni_Load(object sender, EventArgs e)
        {

        }
    }
}
