namespace tadLayUI.adminRentabilidad
{
    partial class frmCosAccDetail
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
            this.grCosteAccidente = new System.Windows.Forms.GroupBox();
            this.ucCosteHerido = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteMuerto = new tadLayUI.userControl.ucLblTxt();
            this.grRoadCeroIndice = new System.Windows.Forms.GroupBox();
            this.ucRoadCeroIM = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadCeroIP = new tadLayUI.userControl.ucLblTxt();
            this.grRoadUnoIndice = new System.Windows.Forms.GroupBox();
            this.ucRoadUnoIM = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadUnoIP = new tadLayUI.userControl.ucLblTxt();
            this.ucNumHeridosPorAccidente = new tadLayUI.userControl.ucLblTxt();
            this.grK = new System.Windows.Forms.GroupBox();
            this.grCosteAccidente.SuspendLayout();
            this.grRoadCeroIndice.SuspendLayout();
            this.grRoadUnoIndice.SuspendLayout();
            this.grK.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(218, 406);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // grCosteAccidente
            // 
            this.grCosteAccidente.Controls.Add(this.ucCosteHerido);
            this.grCosteAccidente.Controls.Add(this.ucCosteMuerto);
            this.grCosteAccidente.Location = new System.Drawing.Point(12, 12);
            this.grCosteAccidente.Name = "grCosteAccidente";
            this.grCosteAccidente.Size = new System.Drawing.Size(260, 95);
            this.grCosteAccidente.TabIndex = 1;
            this.grCosteAccidente.TabStop = false;
            this.grCosteAccidente.Text = "grCosteAccidente";
            // 
            // ucCosteHerido
            // 
            this.ucCosteHerido.ancho = 50;
            this.ucCosteHerido.isEntero = true;
            this.ucCosteHerido.isNegativo = true;
            this.ucCosteHerido.isObligatorio = true;
            this.ucCosteHerido.isSimboloDecimalPunto = true;
            this.ucCosteHerido.location = new System.Drawing.Point(190, 0);
            this.ucCosteHerido.Location = new System.Drawing.Point(9, 63);
            this.ucCosteHerido.lonMax = 50;
            this.ucCosteHerido.Name = "ucCosteHerido";
            this.ucCosteHerido.Size = new System.Drawing.Size(244, 20);
            this.ucCosteHerido.TabIndex = 1;
            this.ucCosteHerido.uiLbl = "ucCosteHerido";
            this.ucCosteHerido.uitxt = "";
            this.ucCosteHerido.valorDoubleNull = null;
            this.ucCosteHerido.valorMaximo = -1D;
            this.ucCosteHerido.valorMinimo = 0D;
            // 
            // ucCosteMuerto
            // 
            this.ucCosteMuerto.ancho = 50;
            this.ucCosteMuerto.isEntero = true;
            this.ucCosteMuerto.isNegativo = false;
            this.ucCosteMuerto.isObligatorio = true;
            this.ucCosteMuerto.isSimboloDecimalPunto = true;
            this.ucCosteMuerto.location = new System.Drawing.Point(190, 0);
            this.ucCosteMuerto.Location = new System.Drawing.Point(9, 29);
            this.ucCosteMuerto.lonMax = 50;
            this.ucCosteMuerto.Name = "ucCosteMuerto";
            this.ucCosteMuerto.Size = new System.Drawing.Size(243, 20);
            this.ucCosteMuerto.TabIndex = 2;
            this.ucCosteMuerto.uiLbl = "ucCosteMuerto";
            this.ucCosteMuerto.uitxt = "";
            this.ucCosteMuerto.valorDoubleNull = null;
            this.ucCosteMuerto.valorMaximo = -1D;
            this.ucCosteMuerto.valorMinimo = 0D;
            // 
            // grRoadCeroIndice
            // 
            this.grRoadCeroIndice.Controls.Add(this.ucRoadCeroIM);
            this.grRoadCeroIndice.Controls.Add(this.ucRoadCeroIP);
            this.grRoadCeroIndice.Location = new System.Drawing.Point(12, 186);
            this.grRoadCeroIndice.Name = "grRoadCeroIndice";
            this.grRoadCeroIndice.Size = new System.Drawing.Size(260, 100);
            this.grRoadCeroIndice.TabIndex = 2;
            this.grRoadCeroIndice.TabStop = false;
            this.grRoadCeroIndice.Text = "grRoadCeroIndice";
            // 
            // ucRoadCeroIM
            // 
            this.ucRoadCeroIM.ancho = 50;
            this.ucRoadCeroIM.isEntero = false;
            this.ucRoadCeroIM.isNegativo = false;
            this.ucRoadCeroIM.isObligatorio = true;
            this.ucRoadCeroIM.isSimboloDecimalPunto = true;
            this.ucRoadCeroIM.location = new System.Drawing.Point(190, 0);
            this.ucRoadCeroIM.Location = new System.Drawing.Point(9, 28);
            this.ucRoadCeroIM.lonMax = 50;
            this.ucRoadCeroIM.Name = "ucRoadCeroIM";
            this.ucRoadCeroIM.Size = new System.Drawing.Size(243, 20);
            this.ucRoadCeroIM.TabIndex = 2;
            this.ucRoadCeroIM.uiLbl = "ucRoadCeroIM";
            this.ucRoadCeroIM.uitxt = "";
            this.ucRoadCeroIM.valorDoubleNull = null;
            this.ucRoadCeroIM.valorMaximo = -1D;
            this.ucRoadCeroIM.valorMinimo = -1D;
            // 
            // ucRoadCeroIP
            // 
            this.ucRoadCeroIP.ancho = 50;
            this.ucRoadCeroIP.isEntero = false;
            this.ucRoadCeroIP.isNegativo = false;
            this.ucRoadCeroIP.isObligatorio = true;
            this.ucRoadCeroIP.isSimboloDecimalPunto = true;
            this.ucRoadCeroIP.location = new System.Drawing.Point(190, 0);
            this.ucRoadCeroIP.Location = new System.Drawing.Point(9, 67);
            this.ucRoadCeroIP.lonMax = 50;
            this.ucRoadCeroIP.Name = "ucRoadCeroIP";
            this.ucRoadCeroIP.Size = new System.Drawing.Size(243, 20);
            this.ucRoadCeroIP.TabIndex = 1;
            this.ucRoadCeroIP.uiLbl = "ucRoadCeroIP";
            this.ucRoadCeroIP.uitxt = "";
            this.ucRoadCeroIP.valorDoubleNull = null;
            this.ucRoadCeroIP.valorMaximo = -1D;
            this.ucRoadCeroIP.valorMinimo = -1D;
            // 
            // grRoadUnoIndice
            // 
            this.grRoadUnoIndice.Controls.Add(this.ucRoadUnoIM);
            this.grRoadUnoIndice.Controls.Add(this.ucRoadUnoIP);
            this.grRoadUnoIndice.Location = new System.Drawing.Point(12, 294);
            this.grRoadUnoIndice.Name = "grRoadUnoIndice";
            this.grRoadUnoIndice.Size = new System.Drawing.Size(260, 100);
            this.grRoadUnoIndice.TabIndex = 3;
            this.grRoadUnoIndice.TabStop = false;
            this.grRoadUnoIndice.Text = "grRoadUnoIndice";
            // 
            // ucRoadUnoIM
            // 
            this.ucRoadUnoIM.ancho = 50;
            this.ucRoadUnoIM.isEntero = false;
            this.ucRoadUnoIM.isNegativo = false;
            this.ucRoadUnoIM.isObligatorio = true;
            this.ucRoadUnoIM.isSimboloDecimalPunto = true;
            this.ucRoadUnoIM.location = new System.Drawing.Point(190, 0);
            this.ucRoadUnoIM.Location = new System.Drawing.Point(9, 25);
            this.ucRoadUnoIM.lonMax = 50;
            this.ucRoadUnoIM.Name = "ucRoadUnoIM";
            this.ucRoadUnoIM.Size = new System.Drawing.Size(243, 20);
            this.ucRoadUnoIM.TabIndex = 2;
            this.ucRoadUnoIM.uiLbl = "ucRoadUnoIM";
            this.ucRoadUnoIM.uitxt = "";
            this.ucRoadUnoIM.valorDoubleNull = null;
            this.ucRoadUnoIM.valorMaximo = -1D;
            this.ucRoadUnoIM.valorMinimo = -1D;
            // 
            // ucRoadUnoIP
            // 
            this.ucRoadUnoIP.ancho = 50;
            this.ucRoadUnoIP.isEntero = false;
            this.ucRoadUnoIP.isNegativo = false;
            this.ucRoadUnoIP.isObligatorio = true;
            this.ucRoadUnoIP.isSimboloDecimalPunto = true;
            this.ucRoadUnoIP.location = new System.Drawing.Point(190, 0);
            this.ucRoadUnoIP.Location = new System.Drawing.Point(9, 63);
            this.ucRoadUnoIP.lonMax = 50;
            this.ucRoadUnoIP.Name = "ucRoadUnoIP";
            this.ucRoadUnoIP.Size = new System.Drawing.Size(243, 20);
            this.ucRoadUnoIP.TabIndex = 1;
            this.ucRoadUnoIP.uiLbl = "ucRoadUnoIP";
            this.ucRoadUnoIP.uitxt = "";
            this.ucRoadUnoIP.valorDoubleNull = null;
            this.ucRoadUnoIP.valorMaximo = -1D;
            this.ucRoadUnoIP.valorMinimo = -1D;
            // 
            // ucNumHeridosPorAccidente
            // 
            this.ucNumHeridosPorAccidente.ancho = 50;
            this.ucNumHeridosPorAccidente.isEntero = false;
            this.ucNumHeridosPorAccidente.isNegativo = true;
            this.ucNumHeridosPorAccidente.isObligatorio = true;
            this.ucNumHeridosPorAccidente.isSimboloDecimalPunto = true;
            this.ucNumHeridosPorAccidente.location = new System.Drawing.Point(190, 0);
            this.ucNumHeridosPorAccidente.Location = new System.Drawing.Point(10, 24);
            this.ucNumHeridosPorAccidente.lonMax = 50;
            this.ucNumHeridosPorAccidente.Name = "ucNumHeridosPorAccidente";
            this.ucNumHeridosPorAccidente.Size = new System.Drawing.Size(244, 20);
            this.ucNumHeridosPorAccidente.TabIndex = 3;
            this.ucNumHeridosPorAccidente.uiLbl = "ucNumHeridosPorAccidente";
            this.ucNumHeridosPorAccidente.uitxt = "";
            this.ucNumHeridosPorAccidente.valorDoubleNull = null;
            this.ucNumHeridosPorAccidente.valorMaximo = 2D;
            this.ucNumHeridosPorAccidente.valorMinimo = 1D;
            // 
            // grK
            // 
            this.grK.Controls.Add(this.ucNumHeridosPorAccidente);
            this.grK.Location = new System.Drawing.Point(12, 113);
            this.grK.Name = "grK";
            this.grK.Size = new System.Drawing.Size(260, 65);
            this.grK.TabIndex = 28;
            this.grK.TabStop = false;
            this.grK.Text = "grK";
            // 
            // frmCosAccDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.grK);
            this.Controls.Add(this.grRoadUnoIndice);
            this.Controls.Add(this.grRoadCeroIndice);
            this.Controls.Add(this.grCosteAccidente);
            this.Controls.Add(this.lnkSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCosAccDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCosAcc";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCosAccDetail_FormClosed);
            this.grCosteAccidente.ResumeLayout(false);
            this.grRoadCeroIndice.ResumeLayout(false);
            this.grRoadUnoIndice.ResumeLayout(false);
            this.grK.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.GroupBox grCosteAccidente;
        private userControl.ucLblTxt ucCosteHerido;
        private userControl.ucLblTxt ucCosteMuerto;
        private System.Windows.Forms.GroupBox grRoadCeroIndice;
        private userControl.ucLblTxt ucRoadCeroIP;
        private userControl.ucLblTxt ucRoadCeroIM;
        private System.Windows.Forms.GroupBox grRoadUnoIndice;
        private userControl.ucLblTxt ucRoadUnoIM;
        private userControl.ucLblTxt ucRoadUnoIP;
        private userControl.ucLblTxt ucNumHeridosPorAccidente;
        private System.Windows.Forms.GroupBox grK;

    }
}