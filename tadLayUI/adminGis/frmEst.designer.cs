namespace tadLayUI.adminGis
{
    partial class frmEst
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
            this.ucColorZona = new tadLayUI.userControl.ucWinColor();
            this.ucProhibirPaso = new tadLayUI.userControl.uclblSiNo();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.grDetalle = new System.Windows.Forms.GroupBox();
            this.btnBuscarSeccion = new System.Windows.Forms.Button();
            this.ucFileSelect1 = new tadLayUI.userControl.ucFileSelect();
            this.ucSecNombre = new tadLayUI.userControl.ucLblTxt();
            this.ucAnchoTablero = new tadLayUI.userControl.ucLblTxt();
            this.ucDistanciaPila = new tadLayUI.userControl.ucLblTxt();
            this.ucAlturaMaxima = new tadLayUI.userControl.ucLblTxt();
            this.ucEstTipoCode1 = new tadLayUI.userControl.ucEstTipoCode();
            this.lnkSalir = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
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
            this.toolStrip1.Size = new System.Drawing.Size(524, 25);
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
            this.grGeneral.Controls.Add(this.ucColorZona);
            this.grGeneral.Controls.Add(this.ucProhibirPaso);
            this.grGeneral.Controls.Add(this.ucDescripcion);
            this.grGeneral.Controls.Add(this.ucNombre);
            this.grGeneral.Location = new System.Drawing.Point(37, 31);
            this.grGeneral.Name = "grGeneral";
            this.grGeneral.Size = new System.Drawing.Size(475, 252);
            this.grGeneral.TabIndex = 24;
            this.grGeneral.TabStop = false;
            this.grGeneral.Text = "grMaster";
            // 
            // ucColorZona
            // 
            this.ucColorZona.ancho = 145;
            this.ucColorZona.location = new System.Drawing.Point(225, 1);
            this.ucColorZona.Location = new System.Drawing.Point(16, 124);
            this.ucColorZona.Name = "ucColorZona";
            this.ucColorZona.Size = new System.Drawing.Size(423, 20);
            this.ucColorZona.TabIndex = 4;
            this.ucColorZona.uiLbl = "ucWinColorZona";
            // 
            // ucProhibirPaso
            // 
            this.ucProhibirPaso.ancho = 50;
            this.ucProhibirPaso.isObligatorio = true;
            this.ucProhibirPaso.location = new System.Drawing.Point(220, 0);
            this.ucProhibirPaso.Location = new System.Drawing.Point(21, 81);
            this.ucProhibirPaso.Name = "ucProhibirPaso";
            this.ucProhibirPaso.Size = new System.Drawing.Size(423, 21);
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
            this.ucDescripcion.location = new System.Drawing.Point(220, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(21, 171);
            this.ucDescripcion.lonMax = 50;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(423, 62);
            this.ucDescripcion.TabIndex = 5;
            this.ucDescripcion.uiLbl = "ucDescripcion";
            this.ucDescripcion.uitxt = "";
            this.ucDescripcion.valorDoubleNull = null;
            this.ucDescripcion.valorMaximo = 0D;
            this.ucDescripcion.valorMinimo = 0D;
            this.ucDescripcion.Load += new System.EventHandler(this.ucDescripcion_Load);
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 145;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(220, 0);
            this.ucNombre.Location = new System.Drawing.Point(21, 35);
            this.ucNombre.lonMax = 50;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(423, 20);
            this.ucNombre.TabIndex = 1;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorDoubleNull = null;
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // grDetalle
            // 
            this.grDetalle.Controls.Add(this.btnBuscarSeccion);
            this.grDetalle.Controls.Add(this.ucFileSelect1);
            this.grDetalle.Controls.Add(this.ucSecNombre);
            this.grDetalle.Controls.Add(this.ucAnchoTablero);
            this.grDetalle.Controls.Add(this.ucDistanciaPila);
            this.grDetalle.Controls.Add(this.ucAlturaMaxima);
            this.grDetalle.Controls.Add(this.ucEstTipoCode1);
            this.grDetalle.Location = new System.Drawing.Point(37, 289);
            this.grDetalle.Name = "grDetalle";
            this.grDetalle.Size = new System.Drawing.Size(475, 270);
            this.grDetalle.TabIndex = 25;
            this.grDetalle.TabStop = false;
            this.grDetalle.Text = "grDetalle";
            // 
            // btnBuscarSeccion
            // 
            this.btnBuscarSeccion.Location = new System.Drawing.Point(298, 190);
            this.btnBuscarSeccion.Name = "btnBuscarSeccion";
            this.btnBuscarSeccion.Size = new System.Drawing.Size(141, 23);
            this.btnBuscarSeccion.TabIndex = 12;
            this.btnBuscarSeccion.Text = "btnBuscarSeccion";
            this.btnBuscarSeccion.UseVisualStyleBackColor = true;
            this.btnBuscarSeccion.Click += new System.EventHandler(this.btnBuscarSeccion_Click);
            // 
            // ucFileSelect1
            // 
            this.ucFileSelect1.Location = new System.Drawing.Point(439, 223);
            this.ucFileSelect1.Name = "ucFileSelect1";
            this.ucFileSelect1.Size = new System.Drawing.Size(25, 25);
            this.ucFileSelect1.TabIndex = 10;
            // 
            // ucSecNombre
            // 
            this.ucSecNombre.ancho = 190;
            this.ucSecNombre.isEntero = false;
            this.ucSecNombre.isNegativo = false;
            this.ucSecNombre.isObligatorio = true;
            this.ucSecNombre.isSimboloDecimalPunto = true;
            this.ucSecNombre.isTexto = true;
            this.ucSecNombre.location = new System.Drawing.Point(210, 0);
            this.ucSecNombre.Location = new System.Drawing.Point(24, 226);
            this.ucSecNombre.lonMax = 50;
            this.ucSecNombre.Name = "ucSecNombre";
            this.ucSecNombre.Size = new System.Drawing.Size(409, 20);
            this.ucSecNombre.TabIndex = 11;
            this.ucSecNombre.uiLbl = "ucSecNombre";
            this.ucSecNombre.uitxt = "";
            this.ucSecNombre.valorDoubleNull = null;
            this.ucSecNombre.valorMaximo = 0D;
            this.ucSecNombre.valorMinimo = 0D;
            // 
            // ucAnchoTablero
            // 
            this.ucAnchoTablero.ancho = 40;
            this.ucAnchoTablero.isEntero = false;
            this.ucAnchoTablero.isNegativo = false;
            this.ucAnchoTablero.isObligatorio = true;
            this.ucAnchoTablero.isSimboloDecimalPunto = true;
            this.ucAnchoTablero.location = new System.Drawing.Point(220, 0);
            this.ucAnchoTablero.Location = new System.Drawing.Point(21, 191);
            this.ucAnchoTablero.lonMax = 32767;
            this.ucAnchoTablero.Name = "ucAnchoTablero";
            this.ucAnchoTablero.Size = new System.Drawing.Size(279, 20);
            this.ucAnchoTablero.TabIndex = 9;
            this.ucAnchoTablero.uiLbl = "ucAnchoTablero";
            this.ucAnchoTablero.uitxt = "";
            this.ucAnchoTablero.valorDoubleNull = null;
            this.ucAnchoTablero.valorMaximo = 1000D;
            this.ucAnchoTablero.valorMinimo = 0D;
            // 
            // ucDistanciaPila
            // 
            this.ucDistanciaPila.ancho = 40;
            this.ucDistanciaPila.isEntero = false;
            this.ucDistanciaPila.isNegativo = false;
            this.ucDistanciaPila.isObligatorio = true;
            this.ucDistanciaPila.isSimboloDecimalPunto = true;
            this.ucDistanciaPila.location = new System.Drawing.Point(220, 0);
            this.ucDistanciaPila.Location = new System.Drawing.Point(21, 152);
            this.ucDistanciaPila.lonMax = 32767;
            this.ucDistanciaPila.Name = "ucDistanciaPila";
            this.ucDistanciaPila.Size = new System.Drawing.Size(398, 20);
            this.ucDistanciaPila.TabIndex = 8;
            this.ucDistanciaPila.uiLbl = "ucDistanciaPila";
            this.ucDistanciaPila.uitxt = "";
            this.ucDistanciaPila.valorDoubleNull = null;
            this.ucDistanciaPila.valorMaximo = 1000D;
            this.ucDistanciaPila.valorMinimo = 0D;
            // 
            // ucAlturaMaxima
            // 
            this.ucAlturaMaxima.ancho = 40;
            this.ucAlturaMaxima.isEntero = false;
            this.ucAlturaMaxima.isNegativo = false;
            this.ucAlturaMaxima.isObligatorio = true;
            this.ucAlturaMaxima.isSimboloDecimalPunto = true;
            this.ucAlturaMaxima.location = new System.Drawing.Point(220, 0);
            this.ucAlturaMaxima.Location = new System.Drawing.Point(21, 113);
            this.ucAlturaMaxima.lonMax = 32767;
            this.ucAlturaMaxima.Name = "ucAlturaMaxima";
            this.ucAlturaMaxima.Size = new System.Drawing.Size(398, 20);
            this.ucAlturaMaxima.TabIndex = 7;
            this.ucAlturaMaxima.uiLbl = "ucAlturaMaxima";
            this.ucAlturaMaxima.uitxt = "";
            this.ucAlturaMaxima.valorDoubleNull = null;
            this.ucAlturaMaxima.valorMaximo = 1000D;
            this.ucAlturaMaxima.valorMinimo = 0D;
            // 
            // ucEstTipoCode1
            // 
            this.ucEstTipoCode1.ancho = 215;
            this.ucEstTipoCode1.isObligatorio = true;
            this.ucEstTipoCode1.Location = new System.Drawing.Point(21, 21);
            this.ucEstTipoCode1.locationDetail = new System.Drawing.Point(220, 52);
            this.ucEstTipoCode1.locationMaster = new System.Drawing.Point(220, 13);
            this.ucEstTipoCode1.Name = "ucEstTipoCode1";
            this.ucEstTipoCode1.Size = new System.Drawing.Size(443, 85);
            this.ucEstTipoCode1.TabIndex = 6;
            this.ucEstTipoCode1.uiLblGrupo = "lblGrupo";
            this.ucEstTipoCode1.uiLblItem = "lblItem";
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(473, 571);
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
            this.lnkSave.Location = new System.Drawing.Point(409, 571);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // frmEst
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(524, 602);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.grDetalle);
            this.Controls.Add(this.grGeneral);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEst";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmEstructuras";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEst_FormClosing);
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
        private userControl.ucWinColor ucColorZona;
        private userControl.uclblSiNo ucProhibirPaso;
        private userControl.ucLblTxt ucDescripcion;
        private userControl.ucLblTxt ucNombre;
        private System.Windows.Forms.GroupBox grDetalle;
        private userControl.ucLblTxt ucAlturaMaxima;
        private userControl.ucEstTipoCode ucEstTipoCode1;
        private userControl.ucLblTxt ucAnchoTablero;
        private userControl.ucLblTxt ucDistanciaPila;
        private userControl.ucLblTxt ucSecNombre;
        private userControl.ucFileSelect ucFileSelect1;
        private System.Windows.Forms.LinkLabel lnkSalir;
        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.Button btnBuscarSeccion;
    }
}