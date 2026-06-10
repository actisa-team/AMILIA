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
    
    public partial class ucToolDetail : UserControl
    {
        
        
        public ucToolDetail()
        {
            InitializeComponent();
        }

        private void ucToolDetail_Load(object sender, EventArgs e)
        {
            lnkSave.Text = strGeneral.uiGuardar;
            lnkCancel.Text = strGeneral.uiCancelar;
            lnkSalir.Text = strGeneral.uiSalir;
        }


 



    }
}
