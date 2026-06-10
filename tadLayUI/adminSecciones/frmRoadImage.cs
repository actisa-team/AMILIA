using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminSecciones
{
    using tadLayLogica;
    using tadLayUI.Properties;
    using tadLayLan;
    
    public partial class frmRoadImage : Form
    {

       private static frmRoadImage mInstance = null;
        
        
        
        private frmRoadImage(string iFrmName,  Image iImagen)
        {
            InitializeComponent();

            this.Text = iFrmName;
            imgSecccion.Image = iImagen;
            imgSecccion.SizeMode = PictureBoxSizeMode.StretchImage;
            this.TopMost = true;
        }


        public static frmRoadImage createInstance (string iFrmName,  Image iImagen)
        {
            if (mInstance == null)
            {
                mInstance = new frmRoadImage(iFrmName, iImagen);
            }

            return mInstance;
        }

        public static void  delteteInstance()
        {

            if (mInstance !=null)
            {
                mInstance.Close();
                
                mInstance=null;  
            }



        }

        private void frmRoadImage_FormClosed(object sender, FormClosedEventArgs e)
        {
            mInstance = null;
        }

        



    }
}
