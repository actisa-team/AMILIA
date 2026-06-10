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
    using tadLayLan;
    using tadLayLogica;
    using engNet.ClassT;
    using engNet.Csv;

    public partial class frmData <T> : Form
    {
        #region "CAMPOS PRIVADOS"
        private List<T> mListado;
        #endregion
        #region "CONSTRUCTOR"

        public frmData()
        {
            InitializeComponent();
            postConstructor();
        }

        #endregion
        #region "BOTONES"
        private void btnImprimir_Click(object sender, EventArgs e)
        {

            try
            {
                string miFileCsv = oTadil.data.Files.saveAsFileCsvFromDialog(string.Empty);

                List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
                List<oValDesT<string, string>> miLstFooter = new List<oValDesT<string, string>>();

                oCsv.write<oValDesT<string, string>, T, oValDesT<string, string>>(miLstHeader, mListado, miLstFooter, miFileCsv);

                oTadil.data.UserInfo.procesoTerminado();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        private void btnClosed_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region "METODOS PUBLICOS"

        public void populate(List<T> iLstDatos, string iFormName, bool iIsVisibleColumnHeader, bool iShowBtnImprimir)
        {
            mListado = iLstDatos;
            var name = oTadil.KAppHeaderName;
            this.Text = name;
            this.ucDgvData.DataSource = mListado;
            this.ucDgvData.dgvSetUpUIDefault(true);
            this.ucDgvData.ColumnHeadersVisible = iIsVisibleColumnHeader;
            this.btnImprimir.Visible = iShowBtnImprimir;
        }

        #endregion
        #region "METODOS PRIVADOS"

        private void postConstructor()
        {
            this.btnClosed.Text = strGeneral.uiCerrar;
            this.btnImprimir.Text = strGeneral.uiImprimir;
        }

        #endregion  
    }
}
