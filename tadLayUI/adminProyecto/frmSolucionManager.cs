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

    using tadLayLogica;
    
    
    public partial class frmSolucionManager : Form
    {

        private eEstudioTipo mEstudioTipo;
        
        
        public frmSolucionManager(eEstudioTipo iEstudioTipo)
        {
            InitializeComponent();

            mEstudioTipo = iEstudioTipo;

            postConstructor();
        }



        private void postConstructor()
        {

            this.ucDgvSoluciones1.setUp(mEstudioTipo);

        }

        private void frmSolucionManager_Shown(object sender, EventArgs e)
        {
            this.ucDgvSoluciones1.populate();
            this.ucDgvSoluciones1.ucDgvSolucion.ClearSelection();
        }

        private void ucDgvSoluciones1_Load(object sender, EventArgs e)
        {

        }
    }
}
