namespace tadLayUI.adminValoracion
{
    partial class frmValEconomicaDetail
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
            this.grValTrazado = new System.Windows.Forms.GroupBox();
            this.ucPrivadaBC = new tadLayUI.userControl.ucLblTxt();
            this.ucPrivadaVAN = new tadLayUI.userControl.ucLblTxt();
            this.ucPrivadaPresupuesto = new tadLayUI.userControl.ucLblTxt();
            this.ucPublicaBC = new tadLayUI.userControl.ucLblTxt();
            this.ucPublicaVAN = new tadLayUI.userControl.ucLblTxt();
            this.ucPublicaPrespupuesto = new tadLayUI.userControl.ucLblTxt();
            this.ucSaveCancel = new tadLayUI.userControl.ucToolDetail();
            this.grValTrazado.SuspendLayout();
            this.SuspendLayout();
            // 
            // grValTrazado
            // 
            this.grValTrazado.Controls.Add(this.ucPrivadaBC);
            this.grValTrazado.Controls.Add(this.ucPrivadaVAN);
            this.grValTrazado.Controls.Add(this.ucPrivadaPresupuesto);
            this.grValTrazado.Controls.Add(this.ucPublicaBC);
            this.grValTrazado.Controls.Add(this.ucPublicaVAN);
            this.grValTrazado.Controls.Add(this.ucPublicaPrespupuesto);
            this.grValTrazado.Location = new System.Drawing.Point(12, 12);
            this.grValTrazado.Name = "grValTrazado";
            this.grValTrazado.Size = new System.Drawing.Size(260, 297);
            this.grValTrazado.TabIndex = 1;
            this.grValTrazado.TabStop = false;
            this.grValTrazado.Text = "grValTrazado";
            // 
            // ucPrivadaBC
            // 
            this.ucPrivadaBC.ancho = 50;
            this.ucPrivadaBC.isEntero = false;
            this.ucPrivadaBC.isNegativo = false;
            this.ucPrivadaBC.isObligatorio = true;
            this.ucPrivadaBC.isSimboloDecimalPunto = true;
            this.ucPrivadaBC.location = new System.Drawing.Point(180, 0);
            this.ucPrivadaBC.Location = new System.Drawing.Point(20, 260);
            this.ucPrivadaBC.lonMax = 32767;
            this.ucPrivadaBC.Name = "ucPrivadaBC";
            this.ucPrivadaBC.Size = new System.Drawing.Size(233, 20);
            this.ucPrivadaBC.TabIndex = 5;
            this.ucPrivadaBC.uiLbl = "ucPrivadaBC";
            this.ucPrivadaBC.uitxt = "";
            this.ucPrivadaBC.valorMaximo = 100D;
            this.ucPrivadaBC.valorMinimo = 0D;
            // 
            // ucPrivadaVAN
            // 
            this.ucPrivadaVAN.ancho = 50;
            this.ucPrivadaVAN.isEntero = false;
            this.ucPrivadaVAN.isNegativo = false;
            this.ucPrivadaVAN.isObligatorio = true;
            this.ucPrivadaVAN.isSimboloDecimalPunto = true;
            this.ucPrivadaVAN.location = new System.Drawing.Point(180, 0);
            this.ucPrivadaVAN.Location = new System.Drawing.Point(20, 213);
            this.ucPrivadaVAN.lonMax = 32767;
            this.ucPrivadaVAN.Name = "ucPrivadaVAN";
            this.ucPrivadaVAN.Size = new System.Drawing.Size(233, 20);
            this.ucPrivadaVAN.TabIndex = 4;
            this.ucPrivadaVAN.uiLbl = "ucPrivadaVAN";
            this.ucPrivadaVAN.uitxt = "";
            this.ucPrivadaVAN.valorMaximo = 100D;
            this.ucPrivadaVAN.valorMinimo = 0D;
            // 
            // ucPrivadaPresupuesto
            // 
            this.ucPrivadaPresupuesto.ancho = 50;
            this.ucPrivadaPresupuesto.isEntero = false;
            this.ucPrivadaPresupuesto.isNegativo = false;
            this.ucPrivadaPresupuesto.isObligatorio = true;
            this.ucPrivadaPresupuesto.isSimboloDecimalPunto = true;
            this.ucPrivadaPresupuesto.location = new System.Drawing.Point(180, 0);
            this.ucPrivadaPresupuesto.Location = new System.Drawing.Point(20, 166);
            this.ucPrivadaPresupuesto.lonMax = 32767;
            this.ucPrivadaPresupuesto.Name = "ucPrivadaPresupuesto";
            this.ucPrivadaPresupuesto.Size = new System.Drawing.Size(233, 20);
            this.ucPrivadaPresupuesto.TabIndex = 3;
            this.ucPrivadaPresupuesto.uiLbl = "ucPrivadaPresupuesto";
            this.ucPrivadaPresupuesto.uitxt = "";
            this.ucPrivadaPresupuesto.valorMaximo = 100D;
            this.ucPrivadaPresupuesto.valorMinimo = 0D;
            // 
            // ucPublicaBC
            // 
            this.ucPublicaBC.ancho = 50;
            this.ucPublicaBC.isEntero = false;
            this.ucPublicaBC.isNegativo = false;
            this.ucPublicaBC.isObligatorio = true;
            this.ucPublicaBC.isSimboloDecimalPunto = true;
            this.ucPublicaBC.location = new System.Drawing.Point(180, 0);
            this.ucPublicaBC.Location = new System.Drawing.Point(20, 119);
            this.ucPublicaBC.lonMax = 32767;
            this.ucPublicaBC.Name = "ucPublicaBC";
            this.ucPublicaBC.Size = new System.Drawing.Size(233, 20);
            this.ucPublicaBC.TabIndex = 2;
            this.ucPublicaBC.uiLbl = "ucPublicaBC";
            this.ucPublicaBC.uitxt = "";
            this.ucPublicaBC.valorMaximo = 100D;
            this.ucPublicaBC.valorMinimo = 0D;
            // 
            // ucPublicaVAN
            // 
            this.ucPublicaVAN.ancho = 50;
            this.ucPublicaVAN.isEntero = false;
            this.ucPublicaVAN.isNegativo = false;
            this.ucPublicaVAN.isObligatorio = true;
            this.ucPublicaVAN.isSimboloDecimalPunto = true;
            this.ucPublicaVAN.location = new System.Drawing.Point(180, 0);
            this.ucPublicaVAN.Location = new System.Drawing.Point(20, 72);
            this.ucPublicaVAN.lonMax = 32767;
            this.ucPublicaVAN.Name = "ucPublicaVAN";
            this.ucPublicaVAN.Size = new System.Drawing.Size(233, 20);
            this.ucPublicaVAN.TabIndex = 1;
            this.ucPublicaVAN.uiLbl = "ucPublicaVAN";
            this.ucPublicaVAN.uitxt = "";
            this.ucPublicaVAN.valorMaximo = 100D;
            this.ucPublicaVAN.valorMinimo = 0D;
            // 
            // ucPublicaPrespupuesto
            // 
            this.ucPublicaPrespupuesto.ancho = 50;
            this.ucPublicaPrespupuesto.isEntero = false;
            this.ucPublicaPrespupuesto.isNegativo = false;
            this.ucPublicaPrespupuesto.isObligatorio = true;
            this.ucPublicaPrespupuesto.isSimboloDecimalPunto = true;
            this.ucPublicaPrespupuesto.location = new System.Drawing.Point(180, 0);
            this.ucPublicaPrespupuesto.Location = new System.Drawing.Point(20, 25);
            this.ucPublicaPrespupuesto.lonMax = 32767;
            this.ucPublicaPrespupuesto.Name = "ucPublicaPrespupuesto";
            this.ucPublicaPrespupuesto.Size = new System.Drawing.Size(233, 20);
            this.ucPublicaPrespupuesto.TabIndex = 0;
            this.ucPublicaPrespupuesto.uiLbl = "ucPublicaPrespupuesto";
            this.ucPublicaPrespupuesto.uitxt = "";
            this.ucPublicaPrespupuesto.valorMaximo = 100D;
            this.ucPublicaPrespupuesto.valorMinimo = 0D;
            // 
            // ucSaveCancel
            // 
            this.ucSaveCancel.Location = new System.Drawing.Point(12, 315);
            this.ucSaveCancel.Name = "ucSaveCancel";
            this.ucSaveCancel.Size = new System.Drawing.Size(260, 25);
            this.ucSaveCancel.TabIndex = 2;
            // 
            // frmValEconomicaDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.ucSaveCancel);
            this.Controls.Add(this.grValTrazado);
            this.Name = "frmValEconomicaDetail";
            this.Text = "frmValEconomica";
            this.Load += new System.EventHandler(this.frmValEconomicaDetail_Load);
            this.grValTrazado.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grValTrazado;
        private userControl.ucLblTxt ucPrivadaVAN;
        private userControl.ucLblTxt ucPrivadaPresupuesto;
        private userControl.ucLblTxt ucPublicaBC;
        private userControl.ucLblTxt ucPublicaVAN;
        private userControl.ucLblTxt ucPublicaPrespupuesto;
        private userControl.ucToolDetail ucSaveCancel;
        private userControl.ucLblTxt ucPrivadaBC;
    }
}