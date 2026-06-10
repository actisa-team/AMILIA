namespace tadLayUI.adminGis
{
    partial class frmTun
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
            this.grDetalle = new System.Windows.Forms.GroupBox();
            this.lnkSalir = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.grEjecucion = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDatos = new System.Windows.Forms.TabPage();
            this.tabRmr = new System.Windows.Forms.TabPage();
            this.btnBuscarSeccion = new System.Windows.Forms.Button();
            this.ucColorZona = new tadLayUI.userControl.ucWinColor();
            this.ucProhibirPaso = new tadLayUI.userControl.uclblSiNo();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.ucTunTratamientosTipos1 = new tadLayUI.userControl.ucTunTratamientosTipos();
            this.ucTunExcTipos1 = new tadLayUI.userControl.ucTunExcTipos();
            this.ucConContraboveda = new tadLayUI.userControl.uclblSiNo();
            this.ucCircularIsDovela = new tadLayUI.userControl.uclblSiNo();
            this.ucFileSelect1 = new tadLayUI.userControl.ucFileSelect();
            this.ucSecNombre = new tadLayUI.userControl.ucLblTxt();
            this.ucAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucGaliboVertical = new tadLayUI.userControl.ucLblTxt();
            this.ucRmr = new tadLayUI.userControl.ucLblTxt();
            this.ucTunTipo = new tadLayUI.userControl.ucEstTipoCode();
            this.ucRmrTabla1 = new tadLayUI.userControl.ucRmrTabla();
            this.toolStrip1.SuspendLayout();
            this.grGeneral.SuspendLayout();
            this.grDetalle.SuspendLayout();
            this.grEjecucion.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabDatos.SuspendLayout();
            this.tabRmr.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHeader});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(492, 25);
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
            this.grGeneral.Location = new System.Drawing.Point(15, 6);
            this.grGeneral.Name = "grGeneral";
            this.grGeneral.Size = new System.Drawing.Size(447, 183);
            this.grGeneral.TabIndex = 24;
            this.grGeneral.TabStop = false;
            this.grGeneral.Text = "grMaster";
            // 
            // grDetalle
            // 
            this.grDetalle.Controls.Add(this.btnBuscarSeccion);
            this.grDetalle.Controls.Add(this.ucConContraboveda);
            this.grDetalle.Controls.Add(this.ucCircularIsDovela);
            this.grDetalle.Controls.Add(this.ucFileSelect1);
            this.grDetalle.Controls.Add(this.ucSecNombre);
            this.grDetalle.Controls.Add(this.ucAncho);
            this.grDetalle.Controls.Add(this.ucGaliboVertical);
            this.grDetalle.Controls.Add(this.ucRmr);
            this.grDetalle.Controls.Add(this.ucTunTipo);
            this.grDetalle.Location = new System.Drawing.Point(15, 197);
            this.grDetalle.Name = "grDetalle";
            this.grDetalle.Size = new System.Drawing.Size(447, 282);
            this.grDetalle.TabIndex = 25;
            this.grDetalle.TabStop = false;
            this.grDetalle.Text = "grDetalle";
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(439, 676);
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
            this.lnkSave.Location = new System.Drawing.Point(387, 676);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // grEjecucion
            // 
            this.grEjecucion.Controls.Add(this.ucTunTratamientosTipos1);
            this.grEjecucion.Controls.Add(this.ucTunExcTipos1);
            this.grEjecucion.Location = new System.Drawing.Point(15, 490);
            this.grEjecucion.Name = "grEjecucion";
            this.grEjecucion.Size = new System.Drawing.Size(447, 98);
            this.grEjecucion.TabIndex = 28;
            this.grEjecucion.TabStop = false;
            this.grEjecucion.Text = "grEjecucion";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDatos);
            this.tabControl1.Controls.Add(this.tabRmr);
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(483, 645);
            this.tabControl1.TabIndex = 29;
            // 
            // tabDatos
            // 
            this.tabDatos.BackColor = System.Drawing.Color.LightGray;
            this.tabDatos.Controls.Add(this.grGeneral);
            this.tabDatos.Controls.Add(this.grEjecucion);
            this.tabDatos.Controls.Add(this.grDetalle);
            this.tabDatos.Location = new System.Drawing.Point(4, 22);
            this.tabDatos.Name = "tabDatos";
            this.tabDatos.Padding = new System.Windows.Forms.Padding(3);
            this.tabDatos.Size = new System.Drawing.Size(475, 619);
            this.tabDatos.TabIndex = 0;
            this.tabDatos.Text = "TabDatos";
            // 
            // tabRmr
            // 
            this.tabRmr.BackColor = System.Drawing.Color.LightGray;
            this.tabRmr.Controls.Add(this.ucRmrTabla1);
            this.tabRmr.Location = new System.Drawing.Point(4, 22);
            this.tabRmr.Name = "tabRmr";
            this.tabRmr.Padding = new System.Windows.Forms.Padding(3);
            this.tabRmr.Size = new System.Drawing.Size(475, 619);
            this.tabRmr.TabIndex = 1;
            this.tabRmr.Text = "tabRmr";
            // 
            // btnBuscarSeccion
            // 
            this.btnBuscarSeccion.Location = new System.Drawing.Point(255, 214);
            this.btnBuscarSeccion.Name = "btnBuscarSeccion";
            this.btnBuscarSeccion.Size = new System.Drawing.Size(185, 23);
            this.btnBuscarSeccion.TabIndex = 8;
            this.btnBuscarSeccion.Text = "btnBuscarSeccion";
            this.btnBuscarSeccion.UseVisualStyleBackColor = true;
            this.btnBuscarSeccion.Click += new System.EventHandler(this.btnBuscarSeccion_Click);
            // 
            // ucColorZona
            // 
            this.ucColorZona.ancho = 210;
            this.ucColorZona.location = new System.Drawing.Point(180, 1);
            this.ucColorZona.Location = new System.Drawing.Point(16, 82);
            this.ucColorZona.Name = "ucColorZona";
            this.ucColorZona.Size = new System.Drawing.Size(398, 20);
            this.ucColorZona.TabIndex = 5;
            this.ucColorZona.uiLbl = "ucWinColorZona";
            // 
            // ucProhibirPaso
            // 
            this.ucProhibirPaso.ancho = 50;
            this.ucProhibirPaso.isObligatorio = true;
            this.ucProhibirPaso.location = new System.Drawing.Point(175, 0);
            this.ucProhibirPaso.Location = new System.Drawing.Point(21, 50);
            this.ucProhibirPaso.Name = "ucProhibirPaso";
            this.ucProhibirPaso.Size = new System.Drawing.Size(340, 21);
            this.ucProhibirPaso.TabIndex = 2;
            this.ucProhibirPaso.uiLbl = "ucProhibirPaso";
            // 
            // ucDescripcion
            // 
            this.ucDescripcion.ancho = 210;
            this.ucDescripcion.isEntero = false;
            this.ucDescripcion.isMultilinea = true;
            this.ucDescripcion.isNegativo = false;
            this.ucDescripcion.isObligatorio = false;
            this.ucDescripcion.isSimboloDecimalPunto = true;
            this.ucDescripcion.isTexto = true;
            this.ucDescripcion.location = new System.Drawing.Point(175, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(21, 116);
            this.ucDescripcion.lonMax = 50;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(393, 48);
            this.ucDescripcion.TabIndex = 2;
            this.ucDescripcion.uiLbl = "ucDescripcion";
            this.ucDescripcion.uitxt = "";
            this.ucDescripcion.valorDoubleNull = null;
            this.ucDescripcion.valorMaximo = 0D;
            this.ucDescripcion.valorMinimo = 0D;
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 210;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(175, 0);
            this.ucNombre.Location = new System.Drawing.Point(21, 23);
            this.ucNombre.lonMax = 50;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(393, 20);
            this.ucNombre.TabIndex = 1;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorDoubleNull = null;
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // ucTunTratamientosTipos1
            // 
            this.ucTunTratamientosTipos1.ancho = 215;
            this.ucTunTratamientosTipos1.isObligatorio = true;
            this.ucTunTratamientosTipos1.location = new System.Drawing.Point(175, 0);
            this.ucTunTratamientosTipos1.Location = new System.Drawing.Point(21, 61);
            this.ucTunTratamientosTipos1.Name = "ucTunTratamientosTipos1";
            this.ucTunTratamientosTipos1.Size = new System.Drawing.Size(408, 21);
            this.ucTunTratamientosTipos1.TabIndex = 2;
            this.ucTunTratamientosTipos1.uiLbl = "ucTunTratamientosTipos1";
            // 
            // ucTunExcTipos1
            // 
            this.ucTunExcTipos1.ancho = 215;
            this.ucTunExcTipos1.isObligatorio = true;
            this.ucTunExcTipos1.location = new System.Drawing.Point(175, 0);
            this.ucTunExcTipos1.Location = new System.Drawing.Point(21, 27);
            this.ucTunExcTipos1.Name = "ucTunExcTipos1";
            this.ucTunExcTipos1.Size = new System.Drawing.Size(408, 21);
            this.ucTunExcTipos1.TabIndex = 0;
            this.ucTunExcTipos1.uiLbl = "ucTunExcTipos1";
            // 
            // ucConContraboveda
            // 
            this.ucConContraboveda.ancho = 50;
            this.ucConContraboveda.isObligatorio = true;
            this.ucConContraboveda.location = new System.Drawing.Point(175, 0);
            this.ucConContraboveda.Location = new System.Drawing.Point(20, 126);
            this.ucConContraboveda.Name = "ucConContraboveda";
            this.ucConContraboveda.Size = new System.Drawing.Size(309, 21);
            this.ucConContraboveda.TabIndex = 7;
            this.ucConContraboveda.uiLbl = "ucConContraboveda";
            // 
            // ucCircularIsDovela
            // 
            this.ucCircularIsDovela.ancho = 50;
            this.ucCircularIsDovela.isObligatorio = true;
            this.ucCircularIsDovela.location = new System.Drawing.Point(175, 0);
            this.ucCircularIsDovela.Location = new System.Drawing.Point(21, 96);
            this.ucCircularIsDovela.Name = "ucCircularIsDovela";
            this.ucCircularIsDovela.Size = new System.Drawing.Size(309, 21);
            this.ucCircularIsDovela.TabIndex = 6;
            this.ucCircularIsDovela.uiLbl = "ucCircularIsDovela";
            // 
            // ucFileSelect1
            // 
            this.ucFileSelect1.Location = new System.Drawing.Point(415, 243);
            this.ucFileSelect1.Name = "ucFileSelect1";
            this.ucFileSelect1.Size = new System.Drawing.Size(25, 25);
            this.ucFileSelect1.TabIndex = 5;
            // 
            // ucSecNombre
            // 
            this.ucSecNombre.ancho = 215;
            this.ucSecNombre.isEntero = false;
            this.ucSecNombre.isNegativo = false;
            this.ucSecNombre.isObligatorio = true;
            this.ucSecNombre.isSimboloDecimalPunto = true;
            this.ucSecNombre.isTexto = true;
            this.ucSecNombre.location = new System.Drawing.Point(175, 0);
            this.ucSecNombre.Location = new System.Drawing.Point(21, 246);
            this.ucSecNombre.lonMax = 50;
            this.ucSecNombre.Name = "ucSecNombre";
            this.ucSecNombre.Size = new System.Drawing.Size(393, 20);
            this.ucSecNombre.TabIndex = 4;
            this.ucSecNombre.uiLbl = "ucSecNombre";
            this.ucSecNombre.uitxt = "";
            this.ucSecNombre.valorDoubleNull = null;
            this.ucSecNombre.valorMaximo = 0D;
            this.ucSecNombre.valorMinimo = 0D;
            // 
            // ucAncho
            // 
            this.ucAncho.ancho = 50;
            this.ucAncho.isEntero = false;
            this.ucAncho.isNegativo = false;
            this.ucAncho.isObligatorio = true;
            this.ucAncho.isSimboloDecimalPunto = true;
            this.ucAncho.location = new System.Drawing.Point(175, 0);
            this.ucAncho.Location = new System.Drawing.Point(21, 214);
            this.ucAncho.lonMax = 32767;
            this.ucAncho.Name = "ucAncho";
            this.ucAncho.Size = new System.Drawing.Size(398, 20);
            this.ucAncho.TabIndex = 3;
            this.ucAncho.uiLbl = "ucAncho";
            this.ucAncho.uitxt = "";
            this.ucAncho.valorDoubleNull = null;
            this.ucAncho.valorMaximo = 1000D;
            this.ucAncho.valorMinimo = 0D;
            // 
            // ucGaliboVertical
            // 
            this.ucGaliboVertical.ancho = 50;
            this.ucGaliboVertical.isEntero = false;
            this.ucGaliboVertical.isNegativo = false;
            this.ucGaliboVertical.isObligatorio = true;
            this.ucGaliboVertical.isSimboloDecimalPunto = true;
            this.ucGaliboVertical.location = new System.Drawing.Point(175, 0);
            this.ucGaliboVertical.Location = new System.Drawing.Point(21, 184);
            this.ucGaliboVertical.lonMax = 32767;
            this.ucGaliboVertical.Name = "ucGaliboVertical";
            this.ucGaliboVertical.Size = new System.Drawing.Size(398, 20);
            this.ucGaliboVertical.TabIndex = 2;
            this.ucGaliboVertical.uiLbl = "ucGaliboVertical";
            this.ucGaliboVertical.uitxt = "";
            this.ucGaliboVertical.valorDoubleNull = null;
            this.ucGaliboVertical.valorMaximo = 1000D;
            this.ucGaliboVertical.valorMinimo = 0D;
            // 
            // ucRmr
            // 
            this.ucRmr.ancho = 50;
            this.ucRmr.isEntero = true;
            this.ucRmr.isNegativo = false;
            this.ucRmr.isObligatorio = true;
            this.ucRmr.isSimboloDecimalPunto = true;
            this.ucRmr.location = new System.Drawing.Point(175, 0);
            this.ucRmr.Location = new System.Drawing.Point(21, 156);
            this.ucRmr.lonMax = 32767;
            this.ucRmr.Name = "ucRmr";
            this.ucRmr.Size = new System.Drawing.Size(398, 20);
            this.ucRmr.TabIndex = 1;
            this.ucRmr.uiLbl = "ucRmr";
            this.ucRmr.uitxt = "";
            this.ucRmr.valorDoubleNull = null;
            this.ucRmr.valorMaximo = 100D;
            this.ucRmr.valorMinimo = 0D;
            // 
            // ucTunTipo
            // 
            this.ucTunTipo.ancho = 215;
            this.ucTunTipo.isObligatorio = true;
            this.ucTunTipo.Location = new System.Drawing.Point(21, 13);
            this.ucTunTipo.locationDetail = new System.Drawing.Point(175, 52);
            this.ucTunTipo.locationMaster = new System.Drawing.Point(175, 13);
            this.ucTunTipo.Name = "ucTunTipo";
            this.ucTunTipo.Size = new System.Drawing.Size(398, 78);
            this.ucTunTipo.TabIndex = 0;
            this.ucTunTipo.uiLblGrupo = "lblGrupo";
            this.ucTunTipo.uiLblItem = "lblItem";
            // 
            // ucRmrTabla1
            // 
            this.ucRmrTabla1.Location = new System.Drawing.Point(8, 6);
            this.ucRmrTabla1.Name = "ucRmrTabla1";
            this.ucRmrTabla1.Size = new System.Drawing.Size(461, 230);
            this.ucRmrTabla1.TabIndex = 0;
            // 
            // frmTun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(492, 697);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmEstructuras";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grGeneral.ResumeLayout(false);
            this.grDetalle.ResumeLayout(false);
            this.grEjecucion.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabDatos.ResumeLayout(false);
            this.tabRmr.ResumeLayout(false);
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
        private userControl.ucLblTxt ucRmr;
        private userControl.ucEstTipoCode ucTunTipo;
        private userControl.ucLblTxt ucAncho;
        private userControl.ucLblTxt ucGaliboVertical;
        private userControl.ucLblTxt ucSecNombre;
        private userControl.ucFileSelect ucFileSelect1;
        private System.Windows.Forms.LinkLabel lnkSalir;
        private System.Windows.Forms.LinkLabel lnkSave;
        private userControl.uclblSiNo ucCircularIsDovela;
        private userControl.uclblSiNo ucConContraboveda;
        private System.Windows.Forms.GroupBox grEjecucion;
        private userControl.ucTunTratamientosTipos ucTunTratamientosTipos1;
        private userControl.ucTunExcTipos ucTunExcTipos1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabDatos;
        private System.Windows.Forms.TabPage tabRmr;
        private userControl.ucRmrTabla ucRmrTabla1;
        private System.Windows.Forms.Button btnBuscarSeccion;
    }
}