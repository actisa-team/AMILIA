namespace tadLayUI.adminRentabilidad
{
    partial class frmGaCoReDetail
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
            this.grRoadCero = new System.Windows.Forms.GroupBox();
            this.ucRoadCeroGastosRehabilitar = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadCeroGastosConservacion = new tadLayUI.userControl.ucLblTxt();
            this.grRoadUno = new System.Windows.Forms.GroupBox();
            this.ucRoadUnoGastosRehabilitar = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadUnoGastosConservacion = new tadLayUI.userControl.ucLblTxt();
            this.grRoadCero.SuspendLayout();
            this.grRoadUno.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(218, 257);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // grRoadCero
            // 
            this.grRoadCero.Controls.Add(this.ucRoadCeroGastosRehabilitar);
            this.grRoadCero.Controls.Add(this.ucRoadCeroGastosConservacion);
            this.grRoadCero.Location = new System.Drawing.Point(12, 12);
            this.grRoadCero.Name = "grRoadCero";
            this.grRoadCero.Size = new System.Drawing.Size(260, 118);
            this.grRoadCero.TabIndex = 25;
            this.grRoadCero.TabStop = false;
            this.grRoadCero.Text = "grRoadCero";
            // 
            // ucRoadCeroGastosRehabilitar
            // 
            this.ucRoadCeroGastosRehabilitar.ancho = 50;
            this.ucRoadCeroGastosRehabilitar.isEntero = true;
            this.ucRoadCeroGastosRehabilitar.isNegativo = false;
            this.ucRoadCeroGastosRehabilitar.isObligatorio = true;
            this.ucRoadCeroGastosRehabilitar.isSimboloDecimalPunto = true;
            this.ucRoadCeroGastosRehabilitar.location = new System.Drawing.Point(190, 0);
            this.ucRoadCeroGastosRehabilitar.Location = new System.Drawing.Point(9, 72);
            this.ucRoadCeroGastosRehabilitar.lonMax = 50;
            this.ucRoadCeroGastosRehabilitar.Name = "ucRoadCeroGastosRehabilitar";
            this.ucRoadCeroGastosRehabilitar.Size = new System.Drawing.Size(244, 20);
            this.ucRoadCeroGastosRehabilitar.TabIndex = 2;
            this.ucRoadCeroGastosRehabilitar.uiLbl = "ucRoadCeroGastosRehabilitar";
            this.ucRoadCeroGastosRehabilitar.uitxt = "";
            this.ucRoadCeroGastosRehabilitar.valorDoubleNull = null;
            this.ucRoadCeroGastosRehabilitar.valorMaximo = -1D;
            this.ucRoadCeroGastosRehabilitar.valorMinimo = 0D;
            // 
            // ucRoadCeroGastosConservacion
            // 
            this.ucRoadCeroGastosConservacion.ancho = 50;
            this.ucRoadCeroGastosConservacion.isEntero = true;
            this.ucRoadCeroGastosConservacion.isNegativo = false;
            this.ucRoadCeroGastosConservacion.isObligatorio = true;
            this.ucRoadCeroGastosConservacion.isSimboloDecimalPunto = true;
            this.ucRoadCeroGastosConservacion.location = new System.Drawing.Point(190, 0);
            this.ucRoadCeroGastosConservacion.Location = new System.Drawing.Point(9, 23);
            this.ucRoadCeroGastosConservacion.lonMax = 50;
            this.ucRoadCeroGastosConservacion.Name = "ucRoadCeroGastosConservacion";
            this.ucRoadCeroGastosConservacion.Size = new System.Drawing.Size(243, 20);
            this.ucRoadCeroGastosConservacion.TabIndex = 1;
            this.ucRoadCeroGastosConservacion.uiLbl = "ucRoadCeroGastosConservacion";
            this.ucRoadCeroGastosConservacion.uitxt = "";
            this.ucRoadCeroGastosConservacion.valorDoubleNull = null;
            this.ucRoadCeroGastosConservacion.valorMaximo = -1D;
            this.ucRoadCeroGastosConservacion.valorMinimo = 0D;
            // 
            // grRoadUno
            // 
            this.grRoadUno.Controls.Add(this.ucRoadUnoGastosRehabilitar);
            this.grRoadUno.Controls.Add(this.ucRoadUnoGastosConservacion);
            this.grRoadUno.Location = new System.Drawing.Point(12, 136);
            this.grRoadUno.Name = "grRoadUno";
            this.grRoadUno.Size = new System.Drawing.Size(260, 118);
            this.grRoadUno.TabIndex = 26;
            this.grRoadUno.TabStop = false;
            this.grRoadUno.Text = "grRoadUno";
            // 
            // ucRoadUnoGastosRehabilitar
            // 
            this.ucRoadUnoGastosRehabilitar.ancho = 50;
            this.ucRoadUnoGastosRehabilitar.isEntero = true;
            this.ucRoadUnoGastosRehabilitar.isNegativo = false;
            this.ucRoadUnoGastosRehabilitar.isObligatorio = true;
            this.ucRoadUnoGastosRehabilitar.isSimboloDecimalPunto = true;
            this.ucRoadUnoGastosRehabilitar.location = new System.Drawing.Point(190, 0);
            this.ucRoadUnoGastosRehabilitar.Location = new System.Drawing.Point(9, 72);
            this.ucRoadUnoGastosRehabilitar.lonMax = 50;
            this.ucRoadUnoGastosRehabilitar.Name = "ucRoadUnoGastosRehabilitar";
            this.ucRoadUnoGastosRehabilitar.Size = new System.Drawing.Size(244, 20);
            this.ucRoadUnoGastosRehabilitar.TabIndex = 2;
            this.ucRoadUnoGastosRehabilitar.uiLbl = "ucRoadUnoGastosRehabilitar";
            this.ucRoadUnoGastosRehabilitar.uitxt = "";
            this.ucRoadUnoGastosRehabilitar.valorDoubleNull = null;
            this.ucRoadUnoGastosRehabilitar.valorMaximo = -1D;
            this.ucRoadUnoGastosRehabilitar.valorMinimo = 0D;
            // 
            // ucRoadUnoGastosConservacion
            // 
            this.ucRoadUnoGastosConservacion.ancho = 50;
            this.ucRoadUnoGastosConservacion.isEntero = true;
            this.ucRoadUnoGastosConservacion.isNegativo = false;
            this.ucRoadUnoGastosConservacion.isObligatorio = true;
            this.ucRoadUnoGastosConservacion.isSimboloDecimalPunto = true;
            this.ucRoadUnoGastosConservacion.location = new System.Drawing.Point(190, 0);
            this.ucRoadUnoGastosConservacion.Location = new System.Drawing.Point(9, 23);
            this.ucRoadUnoGastosConservacion.lonMax = 50;
            this.ucRoadUnoGastosConservacion.Name = "ucRoadUnoGastosConservacion";
            this.ucRoadUnoGastosConservacion.Size = new System.Drawing.Size(243, 20);
            this.ucRoadUnoGastosConservacion.TabIndex = 1;
            this.ucRoadUnoGastosConservacion.uiLbl = "ucRoadUnoGastosConservacion";
            this.ucRoadUnoGastosConservacion.uitxt = "";
            this.ucRoadUnoGastosConservacion.valorDoubleNull = null;
            this.ucRoadUnoGastosConservacion.valorMaximo = -1D;
            this.ucRoadUnoGastosConservacion.valorMinimo = 0D;
            // 
            // frmGaCoReDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.grRoadUno);
            this.Controls.Add(this.grRoadCero);
            this.Controls.Add(this.lnkSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGaCoReDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmGastosConservacionRehabilitacion";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGaCoReDetail_FormClosed);
            this.grRoadCero.ResumeLayout(false);
            this.grRoadUno.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.GroupBox grRoadCero;
        private userControl.ucLblTxt ucRoadCeroGastosRehabilitar;
        private userControl.ucLblTxt ucRoadCeroGastosConservacion;
        private System.Windows.Forms.GroupBox grRoadUno;
        private userControl.ucLblTxt ucRoadUnoGastosRehabilitar;
        private userControl.ucLblTxt ucRoadUnoGastosConservacion;

    }
}