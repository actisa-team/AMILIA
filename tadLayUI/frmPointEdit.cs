using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI
{
    using tadLayLogica;
    using tadLayLan;
    using tadLayShare.puntos;

    
    public partial class frmPointEdit : frmRoot
    {
        public frmPointEdit()
        {
            InitializeComponent();
            postConstructor();
        }


        private void postConstructor()
        {
            #region "Traducion"
            var name = oTadil.KAppHeaderName;
            this.Text = name;
            ucX.uiLbl = strFrmPtoIniFin.uiX;
            ucY.uiLbl = strFrmPtoIniFin.uiY;
            ucZ.uiLbl = strFrmPtoIniFin.uiZ;
            #endregion
            #region "SetUpObjecto"

            this.ucToolDetail1.lnkCancel.Visible = true;
            this.ucToolDetail1.lnkSave.Visible = true;
            this.ucToolDetail1.lnkSalir.Visible = false;

            this.ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            this.ucToolDetail1.lnkCancel.Click += new EventHandler(lnkCancel_Click);

            
            #endregion
        }






        public oP3d getPto()
        {

            double miX = this.ucX.valorDouble;
            double miY = this.ucY.valorDouble;
            double miZ = this.ucZ.valorDouble;

            return new oP3d(miX, miY, miZ);
        }

        public void populate (oP3d iPunto)
        {
            this.ucX.textbox.valorDoubleNull = iPunto.X;
            this.ucY.textbox.valorDoubleNull = iPunto.Y;
            this.ucZ.textbox.valorDoubleNull = iPunto.Z;
        }


        #region "Botones"
        void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Valido Formulario
                if (this.isValidoFrm)
                {

                    //Valido que el punto pertence al terreno
                    if (oSingletonTerreno.getInstance.isPtoInsideTerreno(ucX.valorDoubleNull, ucY.valorDoubleNull))
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Hide();
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strUserCad.uiPointOutCartografia);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.errorValidacion();
                }



            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex); 
            }


        }
        void lnkCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Hide();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        #endregion
    }
}
