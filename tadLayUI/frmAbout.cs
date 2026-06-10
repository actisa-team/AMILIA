using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace tadLayUI
{

    using tadLayLogica;
    
    
    partial class frmAbout : Form
    {
        public frmAbout()
        {
            InitializeComponent();
            var name = oTadil.KAppHeaderName;
            this.Text = name;
            this.labelProductName.Text = oTadil.KAppProductName;
            this.labelCopyright.Text = oTadil.KAppCopyRight;
            this.labelInfo.Text = "+Info " + oTadil.KAppMoreInfo;    
        }

        #region Assembly Attribute Accessors


        #endregion
    }
}
