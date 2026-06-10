namespace tadLayUI.adminProyecto
{
    partial class frmAppPresupuestoDetail
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
            this.grPBL = new System.Windows.Forms.GroupBox();
            this.ucPBLControlCalidad = new tadLayUI.userControl.ucLblTxt();
            this.ucPBLBeneficioIndustrial = new tadLayUI.userControl.ucLblTxt();
            this.ucPBLGastosGenerales = new tadLayUI.userControl.ucLblTxt();
            this.grPCA = new System.Windows.Forms.GroupBox();
            this.ucPCAZonaServidumbre = new tadLayUI.userControl.ucLblTxt();
            this.ucPCAotros = new tadLayUI.userControl.ucLblTxt();
            this.ucPCARestauracionPaisajistica = new tadLayUI.userControl.ucLblTxt();
            this.ucPCAControlCalidad = new tadLayUI.userControl.ucLblTxt();
            this.ucPCAConservacionPatrimonio = new tadLayUI.userControl.ucLblTxt();
            this.grIva = new System.Windows.Forms.GroupBox();
            this.ucIva = new tadLayUI.userControl.ucLblTxt();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.grPBL.SuspendLayout();
            this.grPCA.SuspendLayout();
            this.grIva.SuspendLayout();
            this.SuspendLayout();
            // 
            // grPBL
            // 
            this.grPBL.Controls.Add(this.ucPBLControlCalidad);
            this.grPBL.Controls.Add(this.ucPBLBeneficioIndustrial);
            this.grPBL.Controls.Add(this.ucPBLGastosGenerales);
            this.grPBL.Location = new System.Drawing.Point(11, 6);
            this.grPBL.Name = "grPBL";
            this.grPBL.Size = new System.Drawing.Size(291, 125);
            this.grPBL.TabIndex = 28;
            this.grPBL.TabStop = false;
            this.grPBL.Text = "grPBL";
            // 
            // ucPBLControlCalidad
            // 
            this.ucPBLControlCalidad.ancho = 50;
            this.ucPBLControlCalidad.isEntero = false;
            this.ucPBLControlCalidad.isNegativo = false;
            this.ucPBLControlCalidad.isObligatorio = true;
            this.ucPBLControlCalidad.isSimboloDecimalPunto = true;
            this.ucPBLControlCalidad.location = new System.Drawing.Point(200, 0);
            this.ucPBLControlCalidad.Location = new System.Drawing.Point(15, 86);
            this.ucPBLControlCalidad.lonMax = 32767;
            this.ucPBLControlCalidad.Name = "ucPBLControlCalidad";
            this.ucPBLControlCalidad.Size = new System.Drawing.Size(270, 20);
            this.ucPBLControlCalidad.TabIndex = 2;
            this.ucPBLControlCalidad.uiLbl = "ucPBLControlCalidad";
            this.ucPBLControlCalidad.uitxt = "";
            this.ucPBLControlCalidad.valorMaximo = 100D;
            this.ucPBLControlCalidad.valorMinimo = 0D;
            // 
            // ucPBLBeneficioIndustrial
            // 
            this.ucPBLBeneficioIndustrial.ancho = 50;
            this.ucPBLBeneficioIndustrial.isEntero = false;
            this.ucPBLBeneficioIndustrial.isNegativo = false;
            this.ucPBLBeneficioIndustrial.isObligatorio = true;
            this.ucPBLBeneficioIndustrial.isSimboloDecimalPunto = true;
            this.ucPBLBeneficioIndustrial.location = new System.Drawing.Point(200, 0);
            this.ucPBLBeneficioIndustrial.Location = new System.Drawing.Point(15, 57);
            this.ucPBLBeneficioIndustrial.lonMax = 32767;
            this.ucPBLBeneficioIndustrial.Name = "ucPBLBeneficioIndustrial";
            this.ucPBLBeneficioIndustrial.Size = new System.Drawing.Size(270, 20);
            this.ucPBLBeneficioIndustrial.TabIndex = 1;
            this.ucPBLBeneficioIndustrial.uiLbl = "ucPBLBeneficioIndustrial";
            this.ucPBLBeneficioIndustrial.uitxt = "";
            this.ucPBLBeneficioIndustrial.valorMaximo = 100D;
            this.ucPBLBeneficioIndustrial.valorMinimo = 0D;
            // 
            // ucPBLGastosGenerales
            // 
            this.ucPBLGastosGenerales.ancho = 50;
            this.ucPBLGastosGenerales.isEntero = false;
            this.ucPBLGastosGenerales.isNegativo = false;
            this.ucPBLGastosGenerales.isObligatorio = true;
            this.ucPBLGastosGenerales.isSimboloDecimalPunto = true;
            this.ucPBLGastosGenerales.location = new System.Drawing.Point(200, 0);
            this.ucPBLGastosGenerales.Location = new System.Drawing.Point(15, 28);
            this.ucPBLGastosGenerales.lonMax = 32767;
            this.ucPBLGastosGenerales.Name = "ucPBLGastosGenerales";
            this.ucPBLGastosGenerales.Size = new System.Drawing.Size(270, 20);
            this.ucPBLGastosGenerales.TabIndex = 0;
            this.ucPBLGastosGenerales.uiLbl = "ucPBLGastosGenerales";
            this.ucPBLGastosGenerales.uitxt = "";
            this.ucPBLGastosGenerales.valorMaximo = 100D;
            this.ucPBLGastosGenerales.valorMinimo = 0D;
            // 
            // grPCA
            // 
            this.grPCA.Controls.Add(this.ucPCAZonaServidumbre);
            this.grPCA.Controls.Add(this.ucPCAotros);
            this.grPCA.Controls.Add(this.ucPCARestauracionPaisajistica);
            this.grPCA.Controls.Add(this.ucPCAControlCalidad);
            this.grPCA.Controls.Add(this.ucPCAConservacionPatrimonio);
            this.grPCA.Location = new System.Drawing.Point(12, 137);
            this.grPCA.Name = "grPCA";
            this.grPCA.Size = new System.Drawing.Size(291, 186);
            this.grPCA.TabIndex = 29;
            this.grPCA.TabStop = false;
            this.grPCA.Text = "grPCA";
            // 
            // ucPCAZonaServidumbre
            // 
            this.ucPCAZonaServidumbre.ancho = 50;
            this.ucPCAZonaServidumbre.isEntero = false;
            this.ucPCAZonaServidumbre.isNegativo = false;
            this.ucPCAZonaServidumbre.isObligatorio = true;
            this.ucPCAZonaServidumbre.isSimboloDecimalPunto = true;
            this.ucPCAZonaServidumbre.location = new System.Drawing.Point(200, 0);
            this.ucPCAZonaServidumbre.Location = new System.Drawing.Point(15, 143);
            this.ucPCAZonaServidumbre.lonMax = 32767;
            this.ucPCAZonaServidumbre.Name = "ucPCAZonaServidumbre";
            this.ucPCAZonaServidumbre.Size = new System.Drawing.Size(270, 20);
            this.ucPCAZonaServidumbre.TabIndex = 4;
            this.ucPCAZonaServidumbre.uiLbl = "ucPCAZonaServidumbre";
            this.ucPCAZonaServidumbre.uitxt = "";
            this.ucPCAZonaServidumbre.valorMaximo = 100D;
            this.ucPCAZonaServidumbre.valorMinimo = 0D;
            // 
            // ucPCAotros
            // 
            this.ucPCAotros.ancho = 50;
            this.ucPCAotros.isEntero = false;
            this.ucPCAotros.isNegativo = false;
            this.ucPCAotros.isObligatorio = true;
            this.ucPCAotros.isSimboloDecimalPunto = true;
            this.ucPCAotros.location = new System.Drawing.Point(200, 0);
            this.ucPCAotros.Location = new System.Drawing.Point(15, 112);
            this.ucPCAotros.lonMax = 32767;
            this.ucPCAotros.Name = "ucPCAotros";
            this.ucPCAotros.Size = new System.Drawing.Size(270, 20);
            this.ucPCAotros.TabIndex = 3;
            this.ucPCAotros.uiLbl = "ucPCAotros";
            this.ucPCAotros.uitxt = "";
            this.ucPCAotros.valorMaximo = 100D;
            this.ucPCAotros.valorMinimo = 0D;
            // 
            // ucPCARestauracionPaisajistica
            // 
            this.ucPCARestauracionPaisajistica.ancho = 50;
            this.ucPCARestauracionPaisajistica.isEntero = false;
            this.ucPCARestauracionPaisajistica.isNegativo = false;
            this.ucPCARestauracionPaisajistica.isObligatorio = true;
            this.ucPCARestauracionPaisajistica.isSimboloDecimalPunto = true;
            this.ucPCARestauracionPaisajistica.location = new System.Drawing.Point(200, 0);
            this.ucPCARestauracionPaisajistica.Location = new System.Drawing.Point(15, 86);
            this.ucPCARestauracionPaisajistica.lonMax = 32767;
            this.ucPCARestauracionPaisajistica.Name = "ucPCARestauracionPaisajistica";
            this.ucPCARestauracionPaisajistica.Size = new System.Drawing.Size(270, 20);
            this.ucPCARestauracionPaisajistica.TabIndex = 2;
            this.ucPCARestauracionPaisajistica.uiLbl = "ucPCARestauracionPaisajistica";
            this.ucPCARestauracionPaisajistica.uitxt = "";
            this.ucPCARestauracionPaisajistica.valorMaximo = 100D;
            this.ucPCARestauracionPaisajistica.valorMinimo = 0D;
            // 
            // ucPCAControlCalidad
            // 
            this.ucPCAControlCalidad.ancho = 50;
            this.ucPCAControlCalidad.isEntero = false;
            this.ucPCAControlCalidad.isNegativo = false;
            this.ucPCAControlCalidad.isObligatorio = true;
            this.ucPCAControlCalidad.isSimboloDecimalPunto = true;
            this.ucPCAControlCalidad.location = new System.Drawing.Point(200, 0);
            this.ucPCAControlCalidad.Location = new System.Drawing.Point(15, 57);
            this.ucPCAControlCalidad.lonMax = 32767;
            this.ucPCAControlCalidad.Name = "ucPCAControlCalidad";
            this.ucPCAControlCalidad.Size = new System.Drawing.Size(270, 20);
            this.ucPCAControlCalidad.TabIndex = 1;
            this.ucPCAControlCalidad.uiLbl = "ucPCAControlCalidad";
            this.ucPCAControlCalidad.uitxt = "";
            this.ucPCAControlCalidad.valorMaximo = 100D;
            this.ucPCAControlCalidad.valorMinimo = 0D;
            // 
            // ucPCAConservacionPatrimonio
            // 
            this.ucPCAConservacionPatrimonio.ancho = 50;
            this.ucPCAConservacionPatrimonio.isEntero = false;
            this.ucPCAConservacionPatrimonio.isNegativo = false;
            this.ucPCAConservacionPatrimonio.isObligatorio = true;
            this.ucPCAConservacionPatrimonio.isSimboloDecimalPunto = true;
            this.ucPCAConservacionPatrimonio.location = new System.Drawing.Point(200, 0);
            this.ucPCAConservacionPatrimonio.Location = new System.Drawing.Point(15, 28);
            this.ucPCAConservacionPatrimonio.lonMax = 32767;
            this.ucPCAConservacionPatrimonio.Name = "ucPCAConservacionPatrimonio";
            this.ucPCAConservacionPatrimonio.Size = new System.Drawing.Size(270, 20);
            this.ucPCAConservacionPatrimonio.TabIndex = 0;
            this.ucPCAConservacionPatrimonio.uiLbl = "ucPCAConservacionPatrimonio";
            this.ucPCAConservacionPatrimonio.uitxt = "";
            this.ucPCAConservacionPatrimonio.valorMaximo = 100D;
            this.ucPCAConservacionPatrimonio.valorMinimo = 0D;
            // 
            // grIva
            // 
            this.grIva.Controls.Add(this.ucIva);
            this.grIva.Location = new System.Drawing.Point(12, 329);
            this.grIva.Name = "grIva";
            this.grIva.Size = new System.Drawing.Size(291, 59);
            this.grIva.TabIndex = 30;
            this.grIva.TabStop = false;
            this.grIva.Text = "grIva";
            // 
            // ucIva
            // 
            this.ucIva.ancho = 50;
            this.ucIva.isEntero = false;
            this.ucIva.isNegativo = false;
            this.ucIva.isObligatorio = true;
            this.ucIva.isSimboloDecimalPunto = true;
            this.ucIva.location = new System.Drawing.Point(200, 0);
            this.ucIva.Location = new System.Drawing.Point(15, 23);
            this.ucIva.lonMax = 32767;
            this.ucIva.Name = "ucIva";
            this.ucIva.Size = new System.Drawing.Size(270, 20);
            this.ucIva.TabIndex = 4;
            this.ucIva.uiLbl = "ucIva";
            this.ucIva.uitxt = "";
            this.ucIva.valorMaximo = 100D;
            this.ucIva.valorMinimo = 0D;
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(12, 395);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(290, 25);
            this.ucToolDetail1.TabIndex = 31;
            // 
            // frmAppPresupuestoDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(310, 431);
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.grIva);
            this.Controls.Add(this.grPCA);
            this.Controls.Add(this.grPBL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAppPresupuestoDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.grPBL.ResumeLayout(false);
            this.grPCA.ResumeLayout(false);
            this.grIva.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grPBL;
        private userControl.ucLblTxt ucPBLControlCalidad;
        private userControl.ucLblTxt ucPBLBeneficioIndustrial;
        private userControl.ucLblTxt ucPBLGastosGenerales;
        private System.Windows.Forms.GroupBox grPCA;
        private userControl.ucLblTxt ucPCAotros;
        private userControl.ucLblTxt ucPCARestauracionPaisajistica;
        private userControl.ucLblTxt ucPCAControlCalidad;
        private userControl.ucLblTxt ucPCAConservacionPatrimonio;
        private System.Windows.Forms.GroupBox grIva;
        private userControl.ucLblTxt ucIva;
        private userControl.ucLblTxt ucPCAZonaServidumbre;
        private userControl.ucToolDetail ucToolDetail1;
    }
}