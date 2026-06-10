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

    using engNet.eventos;
    using System.IO;
    using tadLayLogica;
    using tadLayUI.Properties;
    
    public partial class ucFileSelect : UserControl
    {


        public event EventHandler<oEventArgs<string>> evFile;

        private string mFiltro = string.Empty;
        private string mFolderInitial = string.Empty;
        private string mFile=string.Empty;
        
     


        public ucFileSelect()
        {
            InitializeComponent();
        }



        public string filename
        {
            get
            {
                return mFile;  
            }     
        }



        public void setup(string iInitialFolder, string iFiltro)
        {


            if (Directory.Exists(iInitialFolder))
            {
                mFolderInitial = iInitialFolder;
            }
            else
            {
                mFolderInitial = oTadil.data.Files.folderUserInstallApp;
            }
             
            mFiltro = iFiltro;
        }



        private void btnFileSelect_Click(object sender, EventArgs e)
        {
           
            OpenFileDialog miDialogo = new OpenFileDialog();

            var name = oTadil.KAppHeaderName;
            miDialogo.Title = name;
            miDialogo.Filter = mFiltro;
            miDialogo.InitialDirectory = mFolderInitial;
            miDialogo.Multiselect = false;

           
            if (miDialogo.ShowDialog() == DialogResult.OK)
            {

                if (evFile != null)
                {
                    evFile(this, new oEventArgs<string>(miDialogo.FileName));
                }

                mFile = miDialogo.FileName;

            }
            else
            {
                mFile = string.Empty; 
            }

        }
    }
}
