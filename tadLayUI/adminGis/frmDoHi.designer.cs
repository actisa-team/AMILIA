namespace tadLayUI.adminGis
{
    partial class frmDoHi
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
            this.chBoxPasarEnEstructura = new System.Windows.Forms.CheckBox();
            this.ucIsTramoCompleto = new tadLayUI.userControl.uclblSiNo();
            this.ucValoracion = new tadLayUI.userControl.ucCmbValoracion();
            this.ucGalibo = new tadLayUI.userControl.ucLblTxt();
            this.ucAnguloCruceMax = new tadLayUI.userControl.ucLblTxt();
            this.lnkSalir = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.grConfAngulo = new System.Windows.Forms.GroupBox();
            this.checkConfAng = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.grGeneral.SuspendLayout();
            this.grDetalle.SuspendLayout();
            this.grConfAngulo.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHeader});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(385, 25);
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
            this.grGeneral.Location = new System.Drawing.Point(10, 31);
            this.grGeneral.Name = "grGeneral";
            this.grGeneral.Size = new System.Drawing.Size(365, 167);
            this.grGeneral.TabIndex = 24;
            this.grGeneral.TabStop = false;
            this.grGeneral.Text = "grMaster";
            // 
            // ucProhibirPaso
            // 
            this.ucProhibirPaso.ancho = 50;
            this.ucProhibirPaso.isObligatorio = true;
            this.ucProhibirPaso.location = new System.Drawing.Point(175, 0);
            this.ucProhibirPaso.Location = new System.Drawing.Point(21, 56);
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
            this.ucDescripcion.isObligatorio = true;
            this.ucDescripcion.isSimboloDecimalPunto = true;
            this.ucDescripcion.isTexto = true;
            this.ucDescripcion.location = new System.Drawing.Point(175, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(21, 88);
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
            this.grDetalle.Controls.Add(this.chBoxPasarEnEstructura);
            this.grDetalle.Controls.Add(this.ucIsTramoCompleto);
            this.grDetalle.Controls.Add(this.ucValoracion);
            this.grDetalle.Controls.Add(this.ucGalibo);
            this.grDetalle.Location = new System.Drawing.Point(12, 204);
            this.grDetalle.Name = "grDetalle";
            this.grDetalle.Size = new System.Drawing.Size(365, 184);
            this.grDetalle.TabIndex = 25;
            this.grDetalle.TabStop = false;
            this.grDetalle.Text = "grDetalle";
            // 
            // chBoxPasarEnEstructura
            // 
            this.chBoxPasarEnEstructura.AutoSize = true;
            this.chBoxPasarEnEstructura.Location = new System.Drawing.Point(19, 148);
            this.chBoxPasarEnEstructura.Name = "chBoxPasarEnEstructura";
            this.chBoxPasarEnEstructura.Size = new System.Drawing.Size(144, 17);
            this.chBoxPasarEnEstructura.TabIndex = 30;
            this.chBoxPasarEnEstructura.Text = "chBoxPasarEnEstructura";
            this.chBoxPasarEnEstructura.UseVisualStyleBackColor = true;
            // 
            // ucIsTramoCompleto
            // 
            this.ucIsTramoCompleto.ancho = 50;
            this.ucIsTramoCompleto.isObligatorio = true;
            this.ucIsTramoCompleto.location = new System.Drawing.Point(200, 0);
            this.ucIsTramoCompleto.Location = new System.Drawing.Point(19, 98);
            this.ucIsTramoCompleto.Name = "ucIsTramoCompleto";
            this.ucIsTramoCompleto.Size = new System.Drawing.Size(309, 21);
            this.ucIsTramoCompleto.TabIndex = 9;
            this.ucIsTramoCompleto.uiLbl = "ucIsTramoCompleto";
            // 
            // ucValoracion
            // 
            this.ucValoracion.ancho = 50;
            this.ucValoracion.isObligatorio = true;
            this.ucValoracion.location = new System.Drawing.Point(200, 0);
            this.ucValoracion.Location = new System.Drawing.Point(19, 59);
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
            this.ucGalibo.location = new System.Drawing.Point(200, 0);
            this.ucGalibo.Location = new System.Drawing.Point(19, 19);
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
            // ucAnguloCruceMax
            // 
            this.ucAnguloCruceMax.ancho = 50;
            this.ucAnguloCruceMax.isEntero = false;
            this.ucAnguloCruceMax.isNegativo = false;
            this.ucAnguloCruceMax.isObligatorio = true;
            this.ucAnguloCruceMax.isSimboloDecimalPunto = true;
            this.ucAnguloCruceMax.location = new System.Drawing.Point(200, 0);
            this.ucAnguloCruceMax.Location = new System.Drawing.Point(21, 31);
            this.ucAnguloCruceMax.lonMax = 32767;
            this.ucAnguloCruceMax.Name = "ucAnguloCruceMax";
            this.ucAnguloCruceMax.Size = new System.Drawing.Size(325, 20);
            this.ucAnguloCruceMax.TabIndex = 7;
            this.ucAnguloCruceMax.uiLbl = "ucAnguloCruceMax";
            this.ucAnguloCruceMax.uitxt = "";
            this.ucAnguloCruceMax.valorDoubleNull = null;
            this.ucAnguloCruceMax.valorMaximo = 1000D;
            this.ucAnguloCruceMax.valorMinimo = 0D;
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(334, 521);
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
            this.lnkSave.Location = new System.Drawing.Point(282, 521);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // grConfAngulo
            // 
            this.grConfAngulo.Controls.Add(this.ucAnguloCruceMax);
            this.grConfAngulo.Location = new System.Drawing.Point(10, 426);
            this.grConfAngulo.Name = "grConfAngulo";
            this.grConfAngulo.Size = new System.Drawing.Size(365, 79);
            this.grConfAngulo.TabIndex = 28;
            this.grConfAngulo.TabStop = false;
            // 
            // checkConfAng
            // 
            this.checkConfAng.AutoSize = true;
            this.checkConfAng.Location = new System.Drawing.Point(12, 413);
            this.checkConfAng.Name = "checkConfAng";
            this.checkConfAng.Size = new System.Drawing.Size(97, 17);
            this.checkConfAng.TabIndex = 29;
            this.checkConfAng.Text = "checkConfAng";
            this.checkConfAng.UseVisualStyleBackColor = true;
            // 
            // frmDoHi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(385, 547);
            this.Controls.Add(this.checkConfAng);
            this.Controls.Add(this.grConfAngulo);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.grDetalle);
            this.Controls.Add(this.grGeneral);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDoHi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDominioPublicoHidraulico";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grGeneral.ResumeLayout(false);
            this.grDetalle.ResumeLayout(false);
            this.grDetalle.PerformLayout();
            this.grConfAngulo.ResumeLayout(false);
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
        private userControl.ucLblTxt ucAnguloCruceMax;
        private System.Windows.Forms.LinkLabel lnkSalir;
        private System.Windows.Forms.LinkLabel lnkSave;
        private userControl.ucCmbValoracion ucValoracion;
        private userControl.uclblSiNo ucIsTramoCompleto;
        private System.Windows.Forms.GroupBox grConfAngulo;
        private System.Windows.Forms.CheckBox checkConfAng;
        private System.Windows.Forms.CheckBox chBoxPasarEnEstructura;
    }
}