namespace tadLayUI.adminRentabilidad
{
    partial class frmInDaTeDetail
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
            this.grDPP = new System.Windows.Forms.GroupBox();
            this.ucTasaRevisionPreciosPC = new tadLayUI.userControl.ucLblTxt();
            this.ucExplotacionYears = new tadLayUI.userControl.ucLblTxt();
            this.ucConstruccionYears = new tadLayUI.userControl.ucLblTxt();
            this.ucIPCanualPC = new tadLayUI.userControl.ucLblTxt();
            this.ucTasaActualizacionPC = new tadLayUI.userControl.ucLblTxt();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.grDPP.SuspendLayout();
            this.SuspendLayout();
            // 
            // grDPP
            // 
            this.grDPP.Controls.Add(this.ucTasaRevisionPreciosPC);
            this.grDPP.Controls.Add(this.ucExplotacionYears);
            this.grDPP.Controls.Add(this.ucConstruccionYears);
            this.grDPP.Controls.Add(this.ucIPCanualPC);
            this.grDPP.Controls.Add(this.ucTasaActualizacionPC);
            this.grDPP.Location = new System.Drawing.Point(12, 12);
            this.grDPP.Name = "grDPP";
            this.grDPP.Size = new System.Drawing.Size(260, 251);
            this.grDPP.TabIndex = 25;
            this.grDPP.TabStop = false;
            // 
            // ucTasaRevisionPreciosPC
            // 
            this.ucTasaRevisionPreciosPC.ancho = 50;
            this.ucTasaRevisionPreciosPC.isEntero = false;
            this.ucTasaRevisionPreciosPC.isNegativo = false;
            this.ucTasaRevisionPreciosPC.isObligatorio = true;
            this.ucTasaRevisionPreciosPC.isSimboloDecimalPunto = true;
            this.ucTasaRevisionPreciosPC.location = new System.Drawing.Point(190, 0);
            this.ucTasaRevisionPreciosPC.Location = new System.Drawing.Point(9, 69);
            this.ucTasaRevisionPreciosPC.lonMax = 50;
            this.ucTasaRevisionPreciosPC.Name = "ucTasaRevisionPreciosPC";
            this.ucTasaRevisionPreciosPC.Size = new System.Drawing.Size(243, 20);
            this.ucTasaRevisionPreciosPC.TabIndex = 5;
            this.ucTasaRevisionPreciosPC.uiLbl = "ucTasaRevisionPreciosPC";
            this.ucTasaRevisionPreciosPC.uitxt = "";
            this.ucTasaRevisionPreciosPC.valorDoubleNull = null;
            this.ucTasaRevisionPreciosPC.valorMaximo = 100D;
            this.ucTasaRevisionPreciosPC.valorMinimo = 0D;
            // 
            // ucExplotacionYears
            // 
            this.ucExplotacionYears.ancho = 50;
            this.ucExplotacionYears.isEntero = false;
            this.ucExplotacionYears.isNegativo = false;
            this.ucExplotacionYears.isObligatorio = true;
            this.ucExplotacionYears.isSimboloDecimalPunto = true;
            this.ucExplotacionYears.location = new System.Drawing.Point(190, 0);
            this.ucExplotacionYears.Location = new System.Drawing.Point(9, 213);
            this.ucExplotacionYears.lonMax = 50;
            this.ucExplotacionYears.Name = "ucExplotacionYears";
            this.ucExplotacionYears.Size = new System.Drawing.Size(244, 20);
            this.ucExplotacionYears.TabIndex = 4;
            this.ucExplotacionYears.uiLbl = "ucExplotacionYears";
            this.ucExplotacionYears.uitxt = "";
            this.ucExplotacionYears.valorDoubleNull = null;
            this.ucExplotacionYears.valorMaximo = 100D;
            this.ucExplotacionYears.valorMinimo = 0D;
            // 
            // ucConstruccionYears
            // 
            this.ucConstruccionYears.ancho = 50;
            this.ucConstruccionYears.isEntero = true;
            this.ucConstruccionYears.isNegativo = false;
            this.ucConstruccionYears.isObligatorio = true;
            this.ucConstruccionYears.isSimboloDecimalPunto = true;
            this.ucConstruccionYears.location = new System.Drawing.Point(190, 0);
            this.ucConstruccionYears.Location = new System.Drawing.Point(9, 164);
            this.ucConstruccionYears.lonMax = 50;
            this.ucConstruccionYears.Name = "ucConstruccionYears";
            this.ucConstruccionYears.Size = new System.Drawing.Size(244, 20);
            this.ucConstruccionYears.TabIndex = 3;
            this.ucConstruccionYears.uiLbl = "ucConstruccionYears";
            this.ucConstruccionYears.uitxt = "";
            this.ucConstruccionYears.valorDoubleNull = null;
            this.ucConstruccionYears.valorMaximo = 100D;
            this.ucConstruccionYears.valorMinimo = 0D;
            // 
            // ucIPCanualPC
            // 
            this.ucIPCanualPC.ancho = 50;
            this.ucIPCanualPC.isEntero = false;
            this.ucIPCanualPC.isNegativo = false;
            this.ucIPCanualPC.isObligatorio = true;
            this.ucIPCanualPC.isSimboloDecimalPunto = true;
            this.ucIPCanualPC.location = new System.Drawing.Point(190, 0);
            this.ucIPCanualPC.Location = new System.Drawing.Point(9, 115);
            this.ucIPCanualPC.lonMax = 50;
            this.ucIPCanualPC.Name = "ucIPCanualPC";
            this.ucIPCanualPC.Size = new System.Drawing.Size(244, 20);
            this.ucIPCanualPC.TabIndex = 2;
            this.ucIPCanualPC.uiLbl = "ucIPCanualPC";
            this.ucIPCanualPC.uitxt = "";
            this.ucIPCanualPC.valorDoubleNull = null;
            this.ucIPCanualPC.valorMaximo = 100D;
            this.ucIPCanualPC.valorMinimo = 0D;
            // 
            // ucTasaActualizacionPC
            // 
            this.ucTasaActualizacionPC.ancho = 50;
            this.ucTasaActualizacionPC.isEntero = false;
            this.ucTasaActualizacionPC.isNegativo = false;
            this.ucTasaActualizacionPC.isObligatorio = true;
            this.ucTasaActualizacionPC.isSimboloDecimalPunto = true;
            this.ucTasaActualizacionPC.location = new System.Drawing.Point(190, 0);
            this.ucTasaActualizacionPC.Location = new System.Drawing.Point(9, 23);
            this.ucTasaActualizacionPC.lonMax = 50;
            this.ucTasaActualizacionPC.Name = "ucTasaActualizacionPC";
            this.ucTasaActualizacionPC.Size = new System.Drawing.Size(243, 20);
            this.ucTasaActualizacionPC.TabIndex = 1;
            this.ucTasaActualizacionPC.uiLbl = "ucTasaActualizacionPC";
            this.ucTasaActualizacionPC.uitxt = "";
            this.ucTasaActualizacionPC.valorDoubleNull = null;
            this.ucTasaActualizacionPC.valorMaximo = 100D;
            this.ucTasaActualizacionPC.valorMinimo = 0D;
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(219, 266);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // frmInDaTeDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.grDPP);
            this.Controls.Add(this.lnkSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInDaTeDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmIntapeDetail";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInDaTeDetail_FormClosed);
            this.grDPP.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.GroupBox grDPP;
        private userControl.ucLblTxt ucExplotacionYears;
        private userControl.ucLblTxt ucConstruccionYears;
        private userControl.ucLblTxt ucIPCanualPC;
        private userControl.ucLblTxt ucTasaActualizacionPC;
        private userControl.ucLblTxt ucTasaRevisionPreciosPC;

    }
}