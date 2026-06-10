namespace tadLayUI.adminRentabilidad
{
    partial class frmCoTiFuDetail
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
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.grCosteVehiculoLigero = new System.Windows.Forms.GroupBox();
            this.ucCosteAmortizacionesVL = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteNeumaticoVL = new tadLayUI.userControl.ucLblTxt();
            this.ucCoePonderacionCostesTiempoVL = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteTiempoVL = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteLubricante = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteCombustible = new tadLayUI.userControl.ucLblTxt();
            this.grCostesComLub = new System.Windows.Forms.GroupBox();
            this.grCosteVehiculoPesado = new System.Windows.Forms.GroupBox();
            this.ucCosteAmortizacionesVP = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteNeumaticoVP = new tadLayUI.userControl.ucLblTxt();
            this.ucCoePonderacionCostesTiempoVP = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteTiempoVP = new tadLayUI.userControl.ucLblTxt();
            this.grCosteVehiculoLigero.SuspendLayout();
            this.grCostesComLub.SuspendLayout();
            this.grCosteVehiculoPesado.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(276, 461);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // grCosteVehiculoLigero
            // 
            this.grCosteVehiculoLigero.Controls.Add(this.ucCosteAmortizacionesVL);
            this.grCosteVehiculoLigero.Controls.Add(this.ucCosteNeumaticoVL);
            this.grCosteVehiculoLigero.Controls.Add(this.ucCoePonderacionCostesTiempoVL);
            this.grCosteVehiculoLigero.Controls.Add(this.ucCosteTiempoVL);
            this.grCosteVehiculoLigero.Location = new System.Drawing.Point(12, 108);
            this.grCosteVehiculoLigero.Name = "grCosteVehiculoLigero";
            this.grCosteVehiculoLigero.Size = new System.Drawing.Size(319, 164);
            this.grCosteVehiculoLigero.TabIndex = 2;
            this.grCosteVehiculoLigero.TabStop = false;
            this.grCosteVehiculoLigero.Text = "grCosteVehiculoLigero";
            // 
            // ucCosteAmortizacionesVL
            // 
            this.ucCosteAmortizacionesVL.ancho = 50;
            this.ucCosteAmortizacionesVL.isEntero = false;
            this.ucCosteAmortizacionesVL.isNegativo = false;
            this.ucCosteAmortizacionesVL.isObligatorio = true;
            this.ucCosteAmortizacionesVL.isSimboloDecimalPunto = true;
            this.ucCosteAmortizacionesVL.location = new System.Drawing.Point(250, 0);
            this.ucCosteAmortizacionesVL.Location = new System.Drawing.Point(7, 128);
            this.ucCosteAmortizacionesVL.lonMax = 50;
            this.ucCosteAmortizacionesVL.Name = "ucCosteAmortizacionesVL";
            this.ucCosteAmortizacionesVL.Size = new System.Drawing.Size(300, 20);
            this.ucCosteAmortizacionesVL.TabIndex = 4;
            this.ucCosteAmortizacionesVL.uiLbl = "ucCosteAmortizacionesVL";
            this.ucCosteAmortizacionesVL.uitxt = "";
            this.ucCosteAmortizacionesVL.valorDoubleNull = null;
            this.ucCosteAmortizacionesVL.valorMaximo = -1D;
            this.ucCosteAmortizacionesVL.valorMinimo = 0D;
            // 
            // ucCosteNeumaticoVL
            // 
            this.ucCosteNeumaticoVL.ancho = 50;
            this.ucCosteNeumaticoVL.isEntero = false;
            this.ucCosteNeumaticoVL.isNegativo = false;
            this.ucCosteNeumaticoVL.isObligatorio = true;
            this.ucCosteNeumaticoVL.isSimboloDecimalPunto = true;
            this.ucCosteNeumaticoVL.location = new System.Drawing.Point(250, 0);
            this.ucCosteNeumaticoVL.Location = new System.Drawing.Point(7, 96);
            this.ucCosteNeumaticoVL.lonMax = 50;
            this.ucCosteNeumaticoVL.Name = "ucCosteNeumaticoVL";
            this.ucCosteNeumaticoVL.Size = new System.Drawing.Size(300, 20);
            this.ucCosteNeumaticoVL.TabIndex = 3;
            this.ucCosteNeumaticoVL.uiLbl = "ucCosteNeumaticoVL";
            this.ucCosteNeumaticoVL.uitxt = "";
            this.ucCosteNeumaticoVL.valorDoubleNull = null;
            this.ucCosteNeumaticoVL.valorMaximo = -1D;
            this.ucCosteNeumaticoVL.valorMinimo = 0D;
            // 
            // ucCoePonderacionCostesTiempoVL
            // 
            this.ucCoePonderacionCostesTiempoVL.ancho = 50;
            this.ucCoePonderacionCostesTiempoVL.isEntero = false;
            this.ucCoePonderacionCostesTiempoVL.isNegativo = true;
            this.ucCoePonderacionCostesTiempoVL.isObligatorio = true;
            this.ucCoePonderacionCostesTiempoVL.isSimboloDecimalPunto = true;
            this.ucCoePonderacionCostesTiempoVL.location = new System.Drawing.Point(250, 0);
            this.ucCoePonderacionCostesTiempoVL.Location = new System.Drawing.Point(6, 32);
            this.ucCoePonderacionCostesTiempoVL.lonMax = 50;
            this.ucCoePonderacionCostesTiempoVL.Name = "ucCoePonderacionCostesTiempoVL";
            this.ucCoePonderacionCostesTiempoVL.Size = new System.Drawing.Size(301, 20);
            this.ucCoePonderacionCostesTiempoVL.TabIndex = 1;
            this.ucCoePonderacionCostesTiempoVL.uiLbl = "ucCoePonderacionCostesTiempoVL";
            this.ucCoePonderacionCostesTiempoVL.uitxt = "";
            this.ucCoePonderacionCostesTiempoVL.valorDoubleNull = null;
            this.ucCoePonderacionCostesTiempoVL.valorMaximo = -1D;
            this.ucCoePonderacionCostesTiempoVL.valorMinimo = 0D;
            // 
            // ucCosteTiempoVL
            // 
            this.ucCosteTiempoVL.ancho = 50;
            this.ucCosteTiempoVL.isEntero = false;
            this.ucCosteTiempoVL.isNegativo = false;
            this.ucCosteTiempoVL.isObligatorio = true;
            this.ucCosteTiempoVL.isSimboloDecimalPunto = true;
            this.ucCosteTiempoVL.location = new System.Drawing.Point(250, 0);
            this.ucCosteTiempoVL.Location = new System.Drawing.Point(6, 62);
            this.ucCosteTiempoVL.lonMax = 50;
            this.ucCosteTiempoVL.Name = "ucCosteTiempoVL";
            this.ucCosteTiempoVL.Size = new System.Drawing.Size(300, 20);
            this.ucCosteTiempoVL.TabIndex = 2;
            this.ucCosteTiempoVL.uiLbl = "ucCosteTiempoVL";
            this.ucCosteTiempoVL.uitxt = "";
            this.ucCosteTiempoVL.valorDoubleNull = null;
            this.ucCosteTiempoVL.valorMaximo = -1D;
            this.ucCosteTiempoVL.valorMinimo = 0D;
            // 
            // ucCosteLubricante
            // 
            this.ucCosteLubricante.ancho = 50;
            this.ucCosteLubricante.isEntero = false;
            this.ucCosteLubricante.isNegativo = false;
            this.ucCosteLubricante.isObligatorio = true;
            this.ucCosteLubricante.isSimboloDecimalPunto = true;
            this.ucCosteLubricante.location = new System.Drawing.Point(250, 0);
            this.ucCosteLubricante.Location = new System.Drawing.Point(7, 58);
            this.ucCosteLubricante.lonMax = 50;
            this.ucCosteLubricante.Name = "ucCosteLubricante";
            this.ucCosteLubricante.Size = new System.Drawing.Size(300, 20);
            this.ucCosteLubricante.TabIndex = 2;
            this.ucCosteLubricante.uiLbl = "ucCosteLubricante";
            this.ucCosteLubricante.uitxt = "";
            this.ucCosteLubricante.valorDoubleNull = null;
            this.ucCosteLubricante.valorMaximo = -1D;
            this.ucCosteLubricante.valorMinimo = 0D;
            // 
            // ucCosteCombustible
            // 
            this.ucCosteCombustible.ancho = 50;
            this.ucCosteCombustible.isEntero = false;
            this.ucCosteCombustible.isNegativo = false;
            this.ucCosteCombustible.isObligatorio = true;
            this.ucCosteCombustible.isSimboloDecimalPunto = true;
            this.ucCosteCombustible.location = new System.Drawing.Point(250, 0);
            this.ucCosteCombustible.Location = new System.Drawing.Point(7, 28);
            this.ucCosteCombustible.lonMax = 50;
            this.ucCosteCombustible.Name = "ucCosteCombustible";
            this.ucCosteCombustible.Size = new System.Drawing.Size(300, 20);
            this.ucCosteCombustible.TabIndex = 1;
            this.ucCosteCombustible.uiLbl = "ucCosteCombustible";
            this.ucCosteCombustible.uitxt = "";
            this.ucCosteCombustible.valorDoubleNull = null;
            this.ucCosteCombustible.valorMaximo = -1D;
            this.ucCosteCombustible.valorMinimo = 0D;
            // 
            // grCostesComLub
            // 
            this.grCostesComLub.Controls.Add(this.ucCosteCombustible);
            this.grCostesComLub.Controls.Add(this.ucCosteLubricante);
            this.grCostesComLub.Location = new System.Drawing.Point(15, 11);
            this.grCostesComLub.Name = "grCostesComLub";
            this.grCostesComLub.Size = new System.Drawing.Size(316, 90);
            this.grCostesComLub.TabIndex = 1;
            this.grCostesComLub.TabStop = false;
            this.grCostesComLub.Text = "grCostesComLub";
            // 
            // grCosteVehiculoPesado
            // 
            this.grCosteVehiculoPesado.Controls.Add(this.ucCosteAmortizacionesVP);
            this.grCosteVehiculoPesado.Controls.Add(this.ucCosteNeumaticoVP);
            this.grCosteVehiculoPesado.Controls.Add(this.ucCoePonderacionCostesTiempoVP);
            this.grCosteVehiculoPesado.Controls.Add(this.ucCosteTiempoVP);
            this.grCosteVehiculoPesado.Location = new System.Drawing.Point(11, 279);
            this.grCosteVehiculoPesado.Name = "grCosteVehiculoPesado";
            this.grCosteVehiculoPesado.Size = new System.Drawing.Size(319, 164);
            this.grCosteVehiculoPesado.TabIndex = 3;
            this.grCosteVehiculoPesado.TabStop = false;
            this.grCosteVehiculoPesado.Text = "grCosteVehiculoPesado";
            // 
            // ucCosteAmortizacionesVP
            // 
            this.ucCosteAmortizacionesVP.ancho = 50;
            this.ucCosteAmortizacionesVP.isEntero = false;
            this.ucCosteAmortizacionesVP.isNegativo = false;
            this.ucCosteAmortizacionesVP.isObligatorio = true;
            this.ucCosteAmortizacionesVP.isSimboloDecimalPunto = true;
            this.ucCosteAmortizacionesVP.location = new System.Drawing.Point(250, 0);
            this.ucCosteAmortizacionesVP.Location = new System.Drawing.Point(7, 128);
            this.ucCosteAmortizacionesVP.lonMax = 50;
            this.ucCosteAmortizacionesVP.Name = "ucCosteAmortizacionesVP";
            this.ucCosteAmortizacionesVP.Size = new System.Drawing.Size(300, 20);
            this.ucCosteAmortizacionesVP.TabIndex = 4;
            this.ucCosteAmortizacionesVP.uiLbl = "ucCosteAmortizacionesVP";
            this.ucCosteAmortizacionesVP.uitxt = "";
            this.ucCosteAmortizacionesVP.valorDoubleNull = null;
            this.ucCosteAmortizacionesVP.valorMaximo = -1D;
            this.ucCosteAmortizacionesVP.valorMinimo = 0D;
            // 
            // ucCosteNeumaticoVP
            // 
            this.ucCosteNeumaticoVP.ancho = 50;
            this.ucCosteNeumaticoVP.isEntero = false;
            this.ucCosteNeumaticoVP.isNegativo = false;
            this.ucCosteNeumaticoVP.isObligatorio = true;
            this.ucCosteNeumaticoVP.isSimboloDecimalPunto = true;
            this.ucCosteNeumaticoVP.location = new System.Drawing.Point(250, 0);
            this.ucCosteNeumaticoVP.Location = new System.Drawing.Point(7, 96);
            this.ucCosteNeumaticoVP.lonMax = 50;
            this.ucCosteNeumaticoVP.Name = "ucCosteNeumaticoVP";
            this.ucCosteNeumaticoVP.Size = new System.Drawing.Size(300, 20);
            this.ucCosteNeumaticoVP.TabIndex = 3;
            this.ucCosteNeumaticoVP.uiLbl = "ucCosteNeumaticoVP";
            this.ucCosteNeumaticoVP.uitxt = "";
            this.ucCosteNeumaticoVP.valorDoubleNull = null;
            this.ucCosteNeumaticoVP.valorMaximo = -1D;
            this.ucCosteNeumaticoVP.valorMinimo = 0D;
            // 
            // ucCoePonderacionCostesTiempoVP
            // 
            this.ucCoePonderacionCostesTiempoVP.ancho = 50;
            this.ucCoePonderacionCostesTiempoVP.isEntero = false;
            this.ucCoePonderacionCostesTiempoVP.isNegativo = true;
            this.ucCoePonderacionCostesTiempoVP.isObligatorio = true;
            this.ucCoePonderacionCostesTiempoVP.isSimboloDecimalPunto = true;
            this.ucCoePonderacionCostesTiempoVP.location = new System.Drawing.Point(250, 0);
            this.ucCoePonderacionCostesTiempoVP.Location = new System.Drawing.Point(6, 32);
            this.ucCoePonderacionCostesTiempoVP.lonMax = 50;
            this.ucCoePonderacionCostesTiempoVP.Name = "ucCoePonderacionCostesTiempoVP";
            this.ucCoePonderacionCostesTiempoVP.Size = new System.Drawing.Size(301, 20);
            this.ucCoePonderacionCostesTiempoVP.TabIndex = 1;
            this.ucCoePonderacionCostesTiempoVP.uiLbl = "ucCoePonderacionCostesTiempoVP";
            this.ucCoePonderacionCostesTiempoVP.uitxt = "";
            this.ucCoePonderacionCostesTiempoVP.valorDoubleNull = null;
            this.ucCoePonderacionCostesTiempoVP.valorMaximo = -1D;
            this.ucCoePonderacionCostesTiempoVP.valorMinimo = 0D;
            // 
            // ucCosteTiempoVP
            // 
            this.ucCosteTiempoVP.ancho = 50;
            this.ucCosteTiempoVP.isEntero = false;
            this.ucCosteTiempoVP.isNegativo = false;
            this.ucCosteTiempoVP.isObligatorio = true;
            this.ucCosteTiempoVP.isSimboloDecimalPunto = true;
            this.ucCosteTiempoVP.location = new System.Drawing.Point(250, 0);
            this.ucCosteTiempoVP.Location = new System.Drawing.Point(6, 62);
            this.ucCosteTiempoVP.lonMax = 50;
            this.ucCosteTiempoVP.Name = "ucCosteTiempoVP";
            this.ucCosteTiempoVP.Size = new System.Drawing.Size(300, 20);
            this.ucCosteTiempoVP.TabIndex = 2;
            this.ucCosteTiempoVP.uiLbl = "ucCosteTiempoVP";
            this.ucCosteTiempoVP.uitxt = "";
            this.ucCosteTiempoVP.valorDoubleNull = null;
            this.ucCosteTiempoVP.valorMaximo = -1D;
            this.ucCosteTiempoVP.valorMinimo = 0D;
            // 
            // frmCoTiFuDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.grCosteVehiculoPesado);
            this.Controls.Add(this.grCostesComLub);
            this.Controls.Add(this.grCosteVehiculoLigero);
            this.Controls.Add(this.lnkSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCoTiFuDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCoTiFu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCoTiFuDetail_FormClosed);
            this.grCosteVehiculoLigero.ResumeLayout(false);
            this.grCostesComLub.ResumeLayout(false);
            this.grCosteVehiculoPesado.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.GroupBox grCosteVehiculoLigero;
        private userControl.ucLblTxt ucCoePonderacionCostesTiempoVL;
        private userControl.ucLblTxt ucCosteTiempoVL;
        private userControl.ucLblTxt ucCosteAmortizacionesVL;
        private userControl.ucLblTxt ucCosteNeumaticoVL;
        private userControl.ucLblTxt ucCosteLubricante;
        private userControl.ucLblTxt ucCosteCombustible;
        private System.Windows.Forms.GroupBox grCostesComLub;
        private System.Windows.Forms.GroupBox grCosteVehiculoPesado;
        private userControl.ucLblTxt ucCosteAmortizacionesVP;
        private userControl.ucLblTxt ucCosteNeumaticoVP;
        private userControl.ucLblTxt ucCoePonderacionCostesTiempoVP;
        private userControl.ucLblTxt ucCosteTiempoVP;

    }
}