namespace tadLayUI.adminRentabilidad
{
    partial class frmInvTipDetail
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
            this.grInversion = new System.Windows.Forms.GroupBox();
            this.ucIsInversionPrivada = new tadLayUI.userControl.uclblSiNo();
            this.grDPP = new System.Windows.Forms.GroupBox();
            this.ucOtrosPC = new tadLayUI.userControl.ucLblTxt();
            this.ucPaisajisticaPC = new tadLayUI.userControl.ucLblTxt();
            this.ucControlCalidadPC = new tadLayUI.userControl.ucLblTxt();
            this.ucPatrimonioPC = new tadLayUI.userControl.ucLblTxt();
            this.ucExpropiacionPC = new tadLayUI.userControl.ucLblTxt();
            this.ucLicitacionPC = new tadLayUI.userControl.ucLblTxt();
            this.grIIP = new System.Windows.Forms.GroupBox();
            this.ucSubEstatalUpdateIPC = new System.Windows.Forms.RadioButton();
            this.ucSubEstatalAnualFija = new System.Windows.Forms.RadioButton();
            this.ucSubEstatalVehiculo = new tadLayUI.userControl.ucLblTxt();
            this.ucSubEstatalAnual = new tadLayUI.userControl.ucLblTxt();
            this.ucPrecioPeajePorVehiculo = new tadLayUI.userControl.ucLblTxt();
            this.grGas = new System.Windows.Forms.GroupBox();
            this.ucGastosExplotacionManoObraPC = new tadLayUI.userControl.ucLblTxt();
            this.ucGastosSegurosOtros = new tadLayUI.userControl.ucLblTxt();
            this.ucGastosExplotacion = new tadLayUI.userControl.ucLblTxt();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.grInversion.SuspendLayout();
            this.grDPP.SuspendLayout();
            this.grIIP.SuspendLayout();
            this.grGas.SuspendLayout();
            this.SuspendLayout();
            // 
            // grInversion
            // 
            this.grInversion.Controls.Add(this.ucIsInversionPrivada);
            this.grInversion.Location = new System.Drawing.Point(6, 12);
            this.grInversion.Name = "grInversion";
            this.grInversion.Size = new System.Drawing.Size(240, 72);
            this.grInversion.TabIndex = 29;
            this.grInversion.TabStop = false;
            this.grInversion.Text = "grInversion";
            // 
            // ucIsInversionPrivada
            // 
            this.ucIsInversionPrivada.ancho = 50;
            this.ucIsInversionPrivada.isObligatorio = true;
            this.ucIsInversionPrivada.location = new System.Drawing.Point(175, 0);
            this.ucIsInversionPrivada.Location = new System.Drawing.Point(8, 32);
            this.ucIsInversionPrivada.Name = "ucIsInversionPrivada";
            this.ucIsInversionPrivada.Size = new System.Drawing.Size(229, 21);
            this.ucIsInversionPrivada.TabIndex = 29;
            this.ucIsInversionPrivada.uiLbl = "ucIsInversionPrivada";
            // 
            // grDPP
            // 
            this.grDPP.Controls.Add(this.ucOtrosPC);
            this.grDPP.Controls.Add(this.ucPaisajisticaPC);
            this.grDPP.Controls.Add(this.ucControlCalidadPC);
            this.grDPP.Controls.Add(this.ucPatrimonioPC);
            this.grDPP.Controls.Add(this.ucExpropiacionPC);
            this.grDPP.Controls.Add(this.ucLicitacionPC);
            this.grDPP.Location = new System.Drawing.Point(249, 12);
            this.grDPP.Name = "grDPP";
            this.grDPP.Size = new System.Drawing.Size(278, 303);
            this.grDPP.TabIndex = 25;
            this.grDPP.TabStop = false;
            this.grDPP.Text = "grDPP";
            // 
            // ucOtrosPC
            // 
            this.ucOtrosPC.ancho = 40;
            this.ucOtrosPC.isEntero = false;
            this.ucOtrosPC.isNegativo = false;
            this.ucOtrosPC.isObligatorio = true;
            this.ucOtrosPC.isSimboloDecimalPunto = true;
            this.ucOtrosPC.location = new System.Drawing.Point(220, 0);
            this.ucOtrosPC.Location = new System.Drawing.Point(9, 268);
            this.ucOtrosPC.lonMax = 50;
            this.ucOtrosPC.Name = "ucOtrosPC";
            this.ucOtrosPC.Size = new System.Drawing.Size(264, 20);
            this.ucOtrosPC.TabIndex = 6;
            this.ucOtrosPC.uiLbl = "ucOtrosPC";
            this.ucOtrosPC.uitxt = "";
            this.ucOtrosPC.valorDoubleNull = null;
            this.ucOtrosPC.valorMaximo = 100D;
            this.ucOtrosPC.valorMinimo = 0D;
            // 
            // ucPaisajisticaPC
            // 
            this.ucPaisajisticaPC.ancho = 40;
            this.ucPaisajisticaPC.isEntero = false;
            this.ucPaisajisticaPC.isNegativo = false;
            this.ucPaisajisticaPC.isObligatorio = true;
            this.ucPaisajisticaPC.isSimboloDecimalPunto = true;
            this.ucPaisajisticaPC.location = new System.Drawing.Point(220, 0);
            this.ucPaisajisticaPC.Location = new System.Drawing.Point(9, 219);
            this.ucPaisajisticaPC.lonMax = 50;
            this.ucPaisajisticaPC.Name = "ucPaisajisticaPC";
            this.ucPaisajisticaPC.Size = new System.Drawing.Size(264, 20);
            this.ucPaisajisticaPC.TabIndex = 5;
            this.ucPaisajisticaPC.uiLbl = "ucPaisajisticaPC";
            this.ucPaisajisticaPC.uitxt = "";
            this.ucPaisajisticaPC.valorDoubleNull = null;
            this.ucPaisajisticaPC.valorMaximo = 100D;
            this.ucPaisajisticaPC.valorMinimo = 0D;
            // 
            // ucControlCalidadPC
            // 
            this.ucControlCalidadPC.ancho = 40;
            this.ucControlCalidadPC.isEntero = false;
            this.ucControlCalidadPC.isNegativo = false;
            this.ucControlCalidadPC.isObligatorio = true;
            this.ucControlCalidadPC.isSimboloDecimalPunto = true;
            this.ucControlCalidadPC.location = new System.Drawing.Point(220, 0);
            this.ucControlCalidadPC.Location = new System.Drawing.Point(9, 170);
            this.ucControlCalidadPC.lonMax = 50;
            this.ucControlCalidadPC.Name = "ucControlCalidadPC";
            this.ucControlCalidadPC.Size = new System.Drawing.Size(264, 20);
            this.ucControlCalidadPC.TabIndex = 4;
            this.ucControlCalidadPC.uiLbl = "ucControlCalidadPC";
            this.ucControlCalidadPC.uitxt = "";
            this.ucControlCalidadPC.valorDoubleNull = null;
            this.ucControlCalidadPC.valorMaximo = 100D;
            this.ucControlCalidadPC.valorMinimo = 0D;
            // 
            // ucPatrimonioPC
            // 
            this.ucPatrimonioPC.ancho = 40;
            this.ucPatrimonioPC.isEntero = false;
            this.ucPatrimonioPC.isNegativo = false;
            this.ucPatrimonioPC.isObligatorio = true;
            this.ucPatrimonioPC.isSimboloDecimalPunto = true;
            this.ucPatrimonioPC.location = new System.Drawing.Point(220, 0);
            this.ucPatrimonioPC.Location = new System.Drawing.Point(9, 121);
            this.ucPatrimonioPC.lonMax = 50;
            this.ucPatrimonioPC.Name = "ucPatrimonioPC";
            this.ucPatrimonioPC.Size = new System.Drawing.Size(264, 20);
            this.ucPatrimonioPC.TabIndex = 3;
            this.ucPatrimonioPC.uiLbl = "ucPatrimonioPC";
            this.ucPatrimonioPC.uitxt = "";
            this.ucPatrimonioPC.valorDoubleNull = null;
            this.ucPatrimonioPC.valorMaximo = 100D;
            this.ucPatrimonioPC.valorMinimo = 0D;
            // 
            // ucExpropiacionPC
            // 
            this.ucExpropiacionPC.ancho = 40;
            this.ucExpropiacionPC.isEntero = false;
            this.ucExpropiacionPC.isNegativo = false;
            this.ucExpropiacionPC.isObligatorio = true;
            this.ucExpropiacionPC.isSimboloDecimalPunto = true;
            this.ucExpropiacionPC.location = new System.Drawing.Point(220, 0);
            this.ucExpropiacionPC.Location = new System.Drawing.Point(9, 72);
            this.ucExpropiacionPC.lonMax = 50;
            this.ucExpropiacionPC.Name = "ucExpropiacionPC";
            this.ucExpropiacionPC.Size = new System.Drawing.Size(264, 20);
            this.ucExpropiacionPC.TabIndex = 2;
            this.ucExpropiacionPC.uiLbl = "ucExpropiacionPC";
            this.ucExpropiacionPC.uitxt = "";
            this.ucExpropiacionPC.valorDoubleNull = null;
            this.ucExpropiacionPC.valorMaximo = 100D;
            this.ucExpropiacionPC.valorMinimo = 0D;
            // 
            // ucLicitacionPC
            // 
            this.ucLicitacionPC.ancho = 40;
            this.ucLicitacionPC.isEntero = false;
            this.ucLicitacionPC.isNegativo = false;
            this.ucLicitacionPC.isObligatorio = true;
            this.ucLicitacionPC.isSimboloDecimalPunto = true;
            this.ucLicitacionPC.location = new System.Drawing.Point(220, 0);
            this.ucLicitacionPC.Location = new System.Drawing.Point(9, 23);
            this.ucLicitacionPC.lonMax = 50;
            this.ucLicitacionPC.Name = "ucLicitacionPC";
            this.ucLicitacionPC.Size = new System.Drawing.Size(263, 20);
            this.ucLicitacionPC.TabIndex = 1;
            this.ucLicitacionPC.uiLbl = "ucLicitacionPC";
            this.ucLicitacionPC.uitxt = "";
            this.ucLicitacionPC.valorDoubleNull = null;
            this.ucLicitacionPC.valorMaximo = 100D;
            this.ucLicitacionPC.valorMinimo = 0D;
            // 
            // grIIP
            // 
            this.grIIP.Controls.Add(this.ucSubEstatalUpdateIPC);
            this.grIIP.Controls.Add(this.ucSubEstatalAnualFija);
            this.grIIP.Controls.Add(this.ucSubEstatalVehiculo);
            this.grIIP.Controls.Add(this.ucSubEstatalAnual);
            this.grIIP.Controls.Add(this.ucPrecioPeajePorVehiculo);
            this.grIIP.Location = new System.Drawing.Point(7, 93);
            this.grIIP.Name = "grIIP";
            this.grIIP.Size = new System.Drawing.Size(239, 222);
            this.grIIP.TabIndex = 25;
            this.grIIP.TabStop = false;
            this.grIIP.Text = "grIIP";
            // 
            // ucSubEstatalUpdateIPC
            // 
            this.ucSubEstatalUpdateIPC.AutoSize = true;
            this.ucSubEstatalUpdateIPC.Location = new System.Drawing.Point(14, 192);
            this.ucSubEstatalUpdateIPC.Name = "ucSubEstatalUpdateIPC";
            this.ucSubEstatalUpdateIPC.Size = new System.Drawing.Size(140, 17);
            this.ucSubEstatalUpdateIPC.TabIndex = 31;
            this.ucSubEstatalUpdateIPC.Text = "ucSubEstatalUpdateIPC";
            this.ucSubEstatalUpdateIPC.UseVisualStyleBackColor = true;
            // 
            // ucSubEstatalAnualFija
            // 
            this.ucSubEstatalAnualFija.AutoSize = true;
            this.ucSubEstatalAnualFija.Checked = true;
            this.ucSubEstatalAnualFija.Location = new System.Drawing.Point(14, 154);
            this.ucSubEstatalAnualFija.Name = "ucSubEstatalAnualFija";
            this.ucSubEstatalAnualFija.Size = new System.Drawing.Size(131, 17);
            this.ucSubEstatalAnualFija.TabIndex = 30;
            this.ucSubEstatalAnualFija.TabStop = true;
            this.ucSubEstatalAnualFija.Text = "ucSubEstatalAnualFija";
            this.ucSubEstatalAnualFija.UseVisualStyleBackColor = true;
            // 
            // ucSubEstatalVehiculo
            // 
            this.ucSubEstatalVehiculo.ancho = 40;
            this.ucSubEstatalVehiculo.isEntero = false;
            this.ucSubEstatalVehiculo.isNegativo = false;
            this.ucSubEstatalVehiculo.isObligatorio = true;
            this.ucSubEstatalVehiculo.isSimboloDecimalPunto = true;
            this.ucSubEstatalVehiculo.location = new System.Drawing.Point(185, 0);
            this.ucSubEstatalVehiculo.Location = new System.Drawing.Point(8, 66);
            this.ucSubEstatalVehiculo.lonMax = 50;
            this.ucSubEstatalVehiculo.Name = "ucSubEstatalVehiculo";
            this.ucSubEstatalVehiculo.Size = new System.Drawing.Size(228, 20);
            this.ucSubEstatalVehiculo.TabIndex = 4;
            this.ucSubEstatalVehiculo.uiLbl = "ucSubEstatalVehiculo";
            this.ucSubEstatalVehiculo.uitxt = "";
            this.ucSubEstatalVehiculo.valorDoubleNull = null;
            this.ucSubEstatalVehiculo.valorMaximo = 1000D;
            this.ucSubEstatalVehiculo.valorMinimo = 0D;
            // 
            // ucSubEstatalAnual
            // 
            this.ucSubEstatalAnual.ancho = 75;
            this.ucSubEstatalAnual.isEntero = false;
            this.ucSubEstatalAnual.isNegativo = false;
            this.ucSubEstatalAnual.isObligatorio = true;
            this.ucSubEstatalAnual.isSimboloDecimalPunto = true;
            this.ucSubEstatalAnual.location = new System.Drawing.Point(150, 0);
            this.ucSubEstatalAnual.Location = new System.Drawing.Point(8, 113);
            this.ucSubEstatalAnual.lonMax = 50;
            this.ucSubEstatalAnual.Name = "ucSubEstatalAnual";
            this.ucSubEstatalAnual.Size = new System.Drawing.Size(228, 20);
            this.ucSubEstatalAnual.TabIndex = 2;
            this.ucSubEstatalAnual.uiLbl = "ucSubEstatalAnual";
            this.ucSubEstatalAnual.uitxt = "";
            this.ucSubEstatalAnual.valorDoubleNull = null;
            this.ucSubEstatalAnual.valorMaximo = 1E+19D;
            this.ucSubEstatalAnual.valorMinimo = 0D;
            // 
            // ucPrecioPeajePorVehiculo
            // 
            this.ucPrecioPeajePorVehiculo.ancho = 40;
            this.ucPrecioPeajePorVehiculo.isEntero = false;
            this.ucPrecioPeajePorVehiculo.isNegativo = false;
            this.ucPrecioPeajePorVehiculo.isObligatorio = true;
            this.ucPrecioPeajePorVehiculo.isSimboloDecimalPunto = true;
            this.ucPrecioPeajePorVehiculo.location = new System.Drawing.Point(185, 0);
            this.ucPrecioPeajePorVehiculo.Location = new System.Drawing.Point(8, 23);
            this.ucPrecioPeajePorVehiculo.lonMax = 50;
            this.ucPrecioPeajePorVehiculo.Name = "ucPrecioPeajePorVehiculo";
            this.ucPrecioPeajePorVehiculo.Size = new System.Drawing.Size(228, 20);
            this.ucPrecioPeajePorVehiculo.TabIndex = 1;
            this.ucPrecioPeajePorVehiculo.uiLbl = "ucPrecioPeajePorVehiculo";
            this.ucPrecioPeajePorVehiculo.uitxt = "";
            this.ucPrecioPeajePorVehiculo.valorDoubleNull = null;
            this.ucPrecioPeajePorVehiculo.valorMaximo = 1000D;
            this.ucPrecioPeajePorVehiculo.valorMinimo = 0D;
            // 
            // grGas
            // 
            this.grGas.Controls.Add(this.ucGastosExplotacionManoObraPC);
            this.grGas.Controls.Add(this.ucGastosSegurosOtros);
            this.grGas.Controls.Add(this.ucGastosExplotacion);
            this.grGas.Location = new System.Drawing.Point(6, 321);
            this.grGas.Name = "grGas";
            this.grGas.Size = new System.Drawing.Size(521, 141);
            this.grGas.TabIndex = 24;
            this.grGas.TabStop = false;
            this.grGas.Text = "grGas";
            // 
            // ucGastosExplotacionManoObraPC
            // 
            this.ucGastosExplotacionManoObraPC.ancho = 125;
            this.ucGastosExplotacionManoObraPC.isEntero = false;
            this.ucGastosExplotacionManoObraPC.isNegativo = false;
            this.ucGastosExplotacionManoObraPC.isObligatorio = true;
            this.ucGastosExplotacionManoObraPC.isSimboloDecimalPunto = true;
            this.ucGastosExplotacionManoObraPC.location = new System.Drawing.Point(380, 0);
            this.ucGastosExplotacionManoObraPC.Location = new System.Drawing.Point(7, 69);
            this.ucGastosExplotacionManoObraPC.lonMax = 50;
            this.ucGastosExplotacionManoObraPC.Name = "ucGastosExplotacionManoObraPC";
            this.ucGastosExplotacionManoObraPC.Size = new System.Drawing.Size(509, 20);
            this.ucGastosExplotacionManoObraPC.TabIndex = 3;
            this.ucGastosExplotacionManoObraPC.uiLbl = "ucGastosExplotacionManoObraPC";
            this.ucGastosExplotacionManoObraPC.uitxt = "";
            this.ucGastosExplotacionManoObraPC.valorDoubleNull = null;
            this.ucGastosExplotacionManoObraPC.valorMaximo = 100D;
            this.ucGastosExplotacionManoObraPC.valorMinimo = 0D;
            // 
            // ucGastosSegurosOtros
            // 
            this.ucGastosSegurosOtros.ancho = 125;
            this.ucGastosSegurosOtros.isEntero = true;
            this.ucGastosSegurosOtros.isNegativo = false;
            this.ucGastosSegurosOtros.isObligatorio = true;
            this.ucGastosSegurosOtros.isSimboloDecimalPunto = true;
            this.ucGastosSegurosOtros.location = new System.Drawing.Point(380, 0);
            this.ucGastosSegurosOtros.Location = new System.Drawing.Point(7, 104);
            this.ucGastosSegurosOtros.lonMax = 50;
            this.ucGastosSegurosOtros.Name = "ucGastosSegurosOtros";
            this.ucGastosSegurosOtros.Size = new System.Drawing.Size(506, 20);
            this.ucGastosSegurosOtros.TabIndex = 2;
            this.ucGastosSegurosOtros.uiLbl = "ucGastosSegurosOtros";
            this.ucGastosSegurosOtros.uitxt = "";
            this.ucGastosSegurosOtros.valorDoubleNull = null;
            this.ucGastosSegurosOtros.valorMaximo = -1D;
            this.ucGastosSegurosOtros.valorMinimo = 0D;
            // 
            // ucGastosExplotacion
            // 
            this.ucGastosExplotacion.ancho = 125;
            this.ucGastosExplotacion.isEntero = true;
            this.ucGastosExplotacion.isNegativo = false;
            this.ucGastosExplotacion.isObligatorio = true;
            this.ucGastosExplotacion.isSimboloDecimalPunto = true;
            this.ucGastosExplotacion.location = new System.Drawing.Point(380, 0);
            this.ucGastosExplotacion.Location = new System.Drawing.Point(7, 32);
            this.ucGastosExplotacion.lonMax = 50;
            this.ucGastosExplotacion.Name = "ucGastosExplotacion";
            this.ucGastosExplotacion.Size = new System.Drawing.Size(508, 20);
            this.ucGastosExplotacion.TabIndex = 1;
            this.ucGastosExplotacion.uiLbl = "ucGastosExplotacion";
            this.ucGastosExplotacion.uitxt = "";
            this.ucGastosExplotacion.valorDoubleNull = null;
            this.ucGastosExplotacion.valorMaximo = -1D;
            this.ucGastosExplotacion.valorMinimo = 0D;
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(475, 472);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // frmInvTipDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(539, 494);
            this.Controls.Add(this.grInversion);
            this.Controls.Add(this.grDPP);
            this.Controls.Add(this.grIIP);
            this.Controls.Add(this.grGas);
            this.Controls.Add(this.lnkSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInvTipDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmIntapeDetail";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInvTipDetail_FormClosed);
            this.grInversion.ResumeLayout(false);
            this.grDPP.ResumeLayout(false);
            this.grIIP.ResumeLayout(false);
            this.grIIP.PerformLayout();
            this.grGas.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grGas;
        private userControl.ucLblTxt ucGastosExplotacion;
        private System.Windows.Forms.LinkLabel lnkSave;
        private userControl.ucLblTxt ucGastosSegurosOtros;
        private System.Windows.Forms.GroupBox grIIP;
        private userControl.ucLblTxt ucSubEstatalVehiculo;
        private userControl.ucLblTxt ucSubEstatalAnual;
        private userControl.ucLblTxt ucPrecioPeajePorVehiculo;
        private System.Windows.Forms.GroupBox grDPP;
        private userControl.ucLblTxt ucOtrosPC;
        private userControl.ucLblTxt ucPaisajisticaPC;
        private userControl.ucLblTxt ucControlCalidadPC;
        private userControl.ucLblTxt ucPatrimonioPC;
        private userControl.ucLblTxt ucExpropiacionPC;
        private userControl.ucLblTxt ucLicitacionPC;
        private System.Windows.Forms.GroupBox grInversion;
        private userControl.uclblSiNo ucIsInversionPrivada;
        private System.Windows.Forms.RadioButton ucSubEstatalAnualFija;
        private System.Windows.Forms.RadioButton ucSubEstatalUpdateIPC;
        private userControl.ucLblTxt ucGastosExplotacionManoObraPC;

    }
}