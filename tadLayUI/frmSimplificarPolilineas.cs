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

    using tadLayLan.Tds;
    using tadLayLogica.Comandos;
    using engCadNet;
    using Autodesk.AutoCAD.EditorInput;
    using tadLayLogica;
    using tadLayLan;
    public partial class frmSimplificarPolilineas : Form
    {
        private int mNumPolilineas = 0;
        private static frmSimplificarPolilineas mInstance = null;
        SelectionSet mPolilineas = null;

        public frmSimplificarPolilineas()
        {
            InitializeComponent();
            postConstrutor();
        }


        public static frmSimplificarPolilineas getInstance
        {

            get
            {
                if (mInstance == null)
                {
                    mInstance = new frmSimplificarPolilineas();
                }
                else
                {
                    mInstance.WindowState = FormWindowState.Normal;
                    mInstance.Focus();
                }

                return mInstance;
            }
        }


        private void postConstrutor()
        {

            var name = oTadil.KAppHeaderName;
            this.Text = name;
            toolStripStatusLabel1.Text = " ";


            groupBox1.Text = strSimplPoli.gbOrigDatos;
            groupBox2.Text = strSimplPoli.gbGuardar;
            //ucLblTxt2.uiLbl = strSimplPoli.lbDistancia;

            ucCmbLayersTodas1.uiLbl = strSimplPoli.lbCapaGuardar;

            btnSelPol.Text = strSimplPoli.btSelPoli;
            btnSimplPoly.Text = strSimplPoli.btSimplPoli;




            


            ucCmbLayersTodas1.populate();
            
        }

        private void btnSelPol_Click(object sender, EventArgs e)
        {
            this.Hide();
            mPolilineas = oSs.getSsByUser(strGeneralUser.uiSelPolilineas, null, eEntidades.LWPOLYLINE, false);
            if (mPolilineas != null)
            {
                toolStripStatusLabel1.Text = strSimplPoli.toolStatus1 + mPolilineas.Count + strSimplPoli.toolStatus2;
            }
            this.Show();
        }

        private void btSimplPoly_Click(object sender, EventArgs e)
        {
            bool isValido = oValidar.isValidoGrupoByFrm(this);

            if (isValido)
            {
                if (mPolilineas == null)
                {
                    //hay que seleccionar primero las polilineas
                    //Info UI
                    oTadil.data.UserInfo.showInfo(strSimplPoli.uiInfoError);
                }
                else
                {

                    oComandoSimplificarPolilinea.simplificarPolilineaEnvAndDraw(mPolilineas, ucCmbLayersTodas1.uiCombo.SelectedValue.ToString());
                    //oComandoSimplificarPolilinea.simplificarPolilinea(mPolilineas, ucCmbLayersTodas1.uiCombo.SelectedValue.ToString(), ucLblTxt2.valorDouble);
                }

                if (mPolilineas != null)
                {
                    toolStripStatusLabel1.Text = strSimplPoli.toolStatus1 + mPolilineas.Count + strSimplPoli.toolStatus2;
                }
            }
            else
            {
                oTadil.data.UserInfo.showInfo(strError.eValidacion);
            }
        
        }

        private void frmSimplificarPolilineas_FormClosed(object sender, FormClosedEventArgs e)
        {
            mInstance = null;
        }

    }
}
