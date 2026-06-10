namespace tadLayUI.adminValoracion
{
    partial class frmValSocioEconomicasDetail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ucSaveCancel = new tadLayUI.userControl.ucToolDetail();
            this.grVal = new System.Windows.Forms.GroupBox();
            this.ucSectorTerciario = new tadLayUI.userControl.ucLblTxt();
            this.ucSectorSecundario = new tadLayUI.userControl.ucLblTxt();
            this.ucSectorPrimario = new tadLayUI.userControl.ucLblTxt();
            this.grVal.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucSaveCancel
            // 
            this.ucSaveCancel.Location = new System.Drawing.Point(12, 179);
            this.ucSaveCancel.Name = "ucSaveCancel";
            this.ucSaveCancel.Size = new System.Drawing.Size(260, 25);
            this.ucSaveCancel.TabIndex = 3;
            // 
            // grVal
            // 
            this.grVal.Controls.Add(this.ucSectorTerciario);
            this.grVal.Controls.Add(this.ucSectorSecundario);
            this.grVal.Controls.Add(this.ucSectorPrimario);
            this.grVal.Location = new System.Drawing.Point(12, 12);
            this.grVal.Name = "grVal";
            this.grVal.Size = new System.Drawing.Size(260, 161);
            this.grVal.TabIndex = 2;
            this.grVal.TabStop = false;
            this.grVal.Text = "grValTrazado";
            // 
            // ucSectorTerciario
            // 
            this.ucSectorTerciario.ancho = 50;
            this.ucSectorTerciario.isEntero = false;
            this.ucSectorTerciario.isNegativo = false;
            this.ucSectorTerciario.isObligatorio = true;
            this.ucSectorTerciario.isSimboloDecimalPunto = true;
            this.ucSectorTerciario.location = new System.Drawing.Point(180, 0);
            this.ucSectorTerciario.Location = new System.Drawing.Point(21, 123);
            this.ucSectorTerciario.lonMax = 32767;
            this.ucSectorTerciario.Name = "ucSectorTerciario";
            this.ucSectorTerciario.Size = new System.Drawing.Size(233, 20);
            this.ucSectorTerciario.TabIndex = 2;
            this.ucSectorTerciario.uiLbl = "ucSectorTerciario";
            this.ucSectorTerciario.uitxt = "";
            this.ucSectorTerciario.valorMaximo = 100D;
            this.ucSectorTerciario.valorMinimo = 0D;
            // 
            // ucSectorSecundario
            // 
            this.ucSectorSecundario.ancho = 50;
            this.ucSectorSecundario.isEntero = false;
            this.ucSectorSecundario.isNegativo = false;
            this.ucSectorSecundario.isObligatorio = true;
            this.ucSectorSecundario.isSimboloDecimalPunto = true;
            this.ucSectorSecundario.location = new System.Drawing.Point(180, 0);
            this.ucSectorSecundario.Location = new System.Drawing.Point(21, 74);
            this.ucSectorSecundario.lonMax = 32767;
            this.ucSectorSecundario.Name = "ucSectorSecundario";
            this.ucSectorSecundario.Size = new System.Drawing.Size(233, 20);
            this.ucSectorSecundario.TabIndex = 1;
            this.ucSectorSecundario.uiLbl = "ucSectorSecundario";
            this.ucSectorSecundario.uitxt = "";
            this.ucSectorSecundario.valorMaximo = 100D;
            this.ucSectorSecundario.valorMinimo = 0D;
            // 
            // ucSectorPrimario
            // 
            this.ucSectorPrimario.ancho = 50;
            this.ucSectorPrimario.isEntero = false;
            this.ucSectorPrimario.isNegativo = false;
            this.ucSectorPrimario.isObligatorio = true;
            this.ucSectorPrimario.isSimboloDecimalPunto = true;
            this.ucSectorPrimario.location = new System.Drawing.Point(180, 0);
            this.ucSectorPrimario.Location = new System.Drawing.Point(21, 25);
            this.ucSectorPrimario.lonMax = 32767;
            this.ucSectorPrimario.Name = "ucSectorPrimario";
            this.ucSectorPrimario.Size = new System.Drawing.Size(233, 20);
            this.ucSectorPrimario.TabIndex = 0;
            this.ucSectorPrimario.uiLbl = "ucSectorPrimario";
            this.ucSectorPrimario.uitxt = "";
            this.ucSectorPrimario.valorMaximo = 100D;
            this.ucSectorPrimario.valorMinimo = 0D;
            // 
            // frmValSocioEconomicasDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.ucSaveCancel);
            this.Controls.Add(this.grVal);
            this.Name = "frmValSocioEconomicasDetail";
            this.Text = "frmValSocioEconomicasDetail";
            this.Load += new System.EventHandler(this.frmValSocioEconomicasDetail_Load);
            this.grVal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private userControl.ucToolDetail ucSaveCancel;
        private System.Windows.Forms.GroupBox grVal;
        private userControl.ucLblTxt ucSectorTerciario;
        private userControl.ucLblTxt ucSectorSecundario;
        private userControl.ucLblTxt ucSectorPrimario;
    }
}