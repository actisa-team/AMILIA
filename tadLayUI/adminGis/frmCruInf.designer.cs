namespace tadLayUI.adminGis
{
    partial class frmCruInf
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblHeader = new System.Windows.Forms.ToolStripLabel();
            this.grGeneral = new System.Windows.Forms.GroupBox();
            this.ucProhibirPaso = new tadLayUI.userControl.uclblSiNo();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.grDetalle = new System.Windows.Forms.GroupBox();
            this.ucValoracion = new tadLayUI.userControl.ucCmbValoracion();
            this.ucGalibo = new tadLayUI.userControl.ucLblTxt();
            this.lnkSalir = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.ucPasoNivelExigir = new tadLayUI.userControl.uclblSiNo();
            this.toolStrip1.SuspendLayout();
            this.grGeneral.SuspendLayout();
            this.grDetalle.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHeader});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(417, 25);
            this.toolStrip1.TabIndex = 23;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblHeader
            // 
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(58, 22);
            this.lblHeader.Text = "lblHeader";
            // 
            // grGeneral
            // 
            this.grGeneral.Controls.Add(this.ucProhibirPaso);
            this.grGeneral.Controls.Add(this.ucDescripcion);
            this.grGeneral.Controls.Add(this.ucNombre);
            this.grGeneral.Location = new System.Drawing.Point(37, 31);
            this.grGeneral.Name = "grGeneral";
            this.grGeneral.Size = new System.Drawing.Size(365, 164);
            this.grGeneral.TabIndex = 24;
            this.grGeneral.TabStop = false;
            this.grGeneral.Text = "grMaster";
            // 
            // ucProhibirPaso
            // 
            this.ucProhibirPaso.ancho = 50;
            this.ucProhibirPaso.isObligatorio = true;
            this.ucProhibirPaso.location = new System.Drawing.Point(175, 0);
            this.ucProhibirPaso.Location = new System.Drawing.Point(21, 53);
            this.ucProhibirPaso.Name = "ucProhibirPaso";
            this.ucProhibirPaso.Size = new System.Drawing.Size(338, 21);
            this.ucProhibirPaso.TabIndex = 2;
            this.ucProhibirPaso.uiLbl = "ucProhibirPaso";
            // 
            // ucDescripcion
            // 
            this.ucDescripcion.ancho = 145;
            this.ucDescripcion.isEntero = false;
            this.ucDescripcion.isMultilinea = true;
            this.ucDescripcion.isNegativo = false;
            this.ucDescripcion.isObligatorio = false;
            this.ucDescripcion.isSimboloDecimalPunto = true;
            this.ucDescripcion.isTexto = true;
            this.ucDescripcion.location = new System.Drawing.Point(175, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(21, 86);
            this.ucDescripcion.lonMax = 50;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(338, 62);
            this.ucDescripcion.TabIndex = 5;
            this.ucDescripcion.uiLbl = "ucDescripcion";
            this.ucDescripcion.uitxt = "";
            this.ucDescripcion.valorDoubleNull = null;
            this.ucDescripcion.valorMaximo = 0D;
            this.ucDescripcion.valorMinimo = 0D;
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 145;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(175, 0);
            this.ucNombre.Location = new System.Drawing.Point(21, 23);
            this.ucNombre.lonMax = 50;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(338, 20);
            this.ucNombre.TabIndex = 1;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorDoubleNull = null;
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // grDetalle
            // 
            this.grDetalle.Controls.Add(this.ucValoracion);
            this.grDetalle.Controls.Add(this.ucGalibo);
            this.grDetalle.Controls.Add(this.ucPasoNivelExigir);
            this.grDetalle.Location = new System.Drawing.Point(37, 205);
            this.grDetalle.Name = "grDetalle";
            this.grDetalle.Size = new System.Drawing.Size(365, 81);
            this.grDetalle.TabIndex = 25;
            this.grDetalle.TabStop = false;
            this.grDetalle.Text = "grDetalle";
            // 
            // ucValoracion
            // 
            this.ucValoracion.ancho = 50;
            this.ucValoracion.isObligatorio = true;
            this.ucValoracion.location = new System.Drawing.Point(175, 0);
            this.ucValoracion.Location = new System.Drawing.Point(21, 49);
            this.ucValoracion.Name = "ucValoracion";
            this.ucValoracion.Size = new System.Drawing.Size(309, 21);
            this.ucValoracion.TabIndex = 8;
            this.ucValoracion.uiLbl = "ucValoracion";
            // 
            // ucGalibo
            // 
            this.ucGalibo.ancho = 50;
            this.ucGalibo.isEntero = false;
            this.ucGalibo.isNegativo = false;
            this.ucGalibo.isObligatorio = true;
            this.ucGalibo.isSimboloDecimalPunto = true;
            this.ucGalibo.location = new System.Drawing.Point(175, 0);
            this.ucGalibo.Location = new System.Drawing.Point(21, 19);
            this.ucGalibo.lonMax = 32767;
            this.ucGalibo.Name = "ucGalibo";
            this.ucGalibo.Size = new System.Drawing.Size(325, 20);
            this.ucGalibo.TabIndex = 6;
            this.ucGalibo.uiLbl = "ucGalibo";
            this.ucGalibo.uitxt = "";
            this.ucGalibo.valorDoubleNull = null;
            this.ucGalibo.valorMaximo = 1000D;
            this.ucGalibo.valorMinimo = 0D;
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(355, 296);
            this.lnkSalir.Name = "lnkSalir";
            this.lnkSalir.Size = new System.Drawing.Size(41, 13);
            this.lnkSalir.TabIndex = 27;
            this.lnkSalir.TabStop = true;
            this.lnkSalir.Text = "lnkSalir";
            this.lnkSalir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSalir_LinkClicked);
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(303, 296);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // ucPasoNivelExigir
            // 
            this.ucPasoNivelExigir.ancho = 50;
            this.ucPasoNivelExigir.isObligatorio = true;
            this.ucPasoNivelExigir.location = new System.Drawing.Point(175, 0);
            this.ucPasoNivelExigir.Location = new System.Drawing.Point(21, 49);
            this.ucPasoNivelExigir.Name = "ucPasoNivelExigir";
            this.ucPasoNivelExigir.Size = new System.Drawing.Size(338, 21);
            this.ucPasoNivelExigir.TabIndex = 3;
            this.ucPasoNivelExigir.uiLbl = "ucPasoDesnivelExigir";
            // 
            // frmCruInf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(417, 320);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.grDetalle);
            this.Controls.Add(this.grGeneral);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCruInf";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCruceInfra";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grGeneral.ResumeLayout(false);
            this.grDetalle.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblHeader;
        private System.Windows.Forms.GroupBox grGeneral;
        private userControl.uclblSiNo ucProhibirPaso;
        private userControl.ucLblTxt ucDescripcion;
        private userControl.ucLblTxt ucNombre;
        private System.Windows.Forms.GroupBox grDetalle;
        private userControl.ucLblTxt ucGalibo;
        private System.Windows.Forms.LinkLabel lnkSalir;
        private System.Windows.Forms.LinkLabel lnkSave;
        private userControl.ucCmbValoracion ucValoracion;
        private userControl.uclblSiNo ucPasoNivelExigir;
    }
}