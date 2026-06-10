namespace tadLayUI.adminGis
{
    partial class frmCim
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
            this.ucAguaPresencia = new tadLayUI.userControl.ucCimTipos();
            this.ucExcMetodos = new tadLayUI.userControl.ucCimTipos();
            this.ucCimPasInf = new tadLayUI.userControl.ucCimTipos();
            this.ucCimViaPu = new tadLayUI.userControl.ucCimTipos();
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
            this.toolStrip1.Size = new System.Drawing.Size(467, 25);
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
            this.grGeneral.Location = new System.Drawing.Point(9, 31);
            this.grGeneral.Name = "grGeneral";
            this.grGeneral.Size = new System.Drawing.Size(444, 252);
            this.grGeneral.TabIndex = 24;
            this.grGeneral.TabStop = false;
            this.grGeneral.Text = "grMaster";
            // 
            // ucColorZona
            // 
            this.ucColorZona.ancho = 200;
            this.ucColorZona.location = new System.Drawing.Point(203, 1);
            this.ucColorZona.Location = new System.Drawing.Point(16, 121);
            this.ucColorZona.Name = "ucColorZona";
            this.ucColorZona.Size = new System.Drawing.Size(422, 20);
            this.ucColorZona.TabIndex = 4;
            this.ucColorZona.uiLbl = "ucWinColorZona";
            // 
            // ucProhibirPaso
            // 
            this.ucProhibirPaso.ancho = 50;
            this.ucProhibirPaso.isObligatorio = true;
            this.ucProhibirPaso.location = new System.Drawing.Point(200, 0);
            this.ucProhibirPaso.Location = new System.Drawing.Point(21, 67);
            this.ucProhibirPaso.Name = "ucProhibirPaso";
            this.ucProhibirPaso.Size = new System.Drawing.Size(406, 21);
            this.ucProhibirPaso.TabIndex = 2;
            this.ucProhibirPaso.uiLbl = "ucProhibirPaso";
            // 
            // ucDescripcion
            // 
            this.ucDescripcion.ancho = 200;
            this.ucDescripcion.isEntero = false;
            this.ucDescripcion.isMultilinea = true;
            this.ucDescripcion.isNegativo = false;
            this.ucDescripcion.isObligatorio = true;
            this.ucDescripcion.isSimboloDecimalPunto = true;
            this.ucDescripcion.isTexto = true;
            this.ucDescripcion.location = new System.Drawing.Point(200, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(18, 171);
            this.ucDescripcion.lonMax = 50;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(406, 62);
            this.ucDescripcion.TabIndex = 5;
            this.ucDescripcion.uiLbl = "ucDescripcion";
            this.ucDescripcion.uitxt = "";
            this.ucDescripcion.valorDoubleNull = null;
            this.ucDescripcion.valorMaximo = 0D;
            this.ucDescripcion.valorMinimo = 0D;
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 200;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(200, 0);
            this.ucNombre.Location = new System.Drawing.Point(21, 23);
            this.ucNombre.lonMax = 50;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(406, 20);
            this.ucNombre.TabIndex = 1;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorDoubleNull = null;
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // grDetalle
            // 
            this.grDetalle.Controls.Add(this.ucAguaPresencia);
            this.grDetalle.Controls.Add(this.ucExcMetodos);
            this.grDetalle.Controls.Add(this.ucCimPasInf);
            this.grDetalle.Controls.Add(this.ucCimViaPu);
            this.grDetalle.Location = new System.Drawing.Point(9, 289);
            this.grDetalle.Name = "grDetalle";
            this.grDetalle.Size = new System.Drawing.Size(444, 197);
            this.grDetalle.TabIndex = 25;
            this.grDetalle.TabStop = false;
            this.grDetalle.Text = "grDetalle";
            // 
            // ucAguaPresencia
            // 
            this.ucAguaPresencia.ancho = 200;
            this.ucAguaPresencia.isObligatorio = true;
            this.ucAguaPresencia.location = new System.Drawing.Point(200, 0);
            this.ucAguaPresencia.Location = new System.Drawing.Point(21, 154);
            this.ucAguaPresencia.Name = "ucAguaPresencia";
            this.ucAguaPresencia.Size = new System.Drawing.Size(406, 21);
            this.ucAguaPresencia.TabIndex = 9;
            this.ucAguaPresencia.uiLbl = "ucAguaPresencia";
            // 
            // ucExcMetodos
            // 
            this.ucExcMetodos.ancho = 200;
            this.ucExcMetodos.isObligatorio = true;
            this.ucExcMetodos.location = new System.Drawing.Point(200, 0);
            this.ucExcMetodos.Location = new System.Drawing.Point(21, 115);
            this.ucExcMetodos.Name = "ucExcMetodos";
            this.ucExcMetodos.Size = new System.Drawing.Size(406, 21);
            this.ucExcMetodos.TabIndex = 8;
            this.ucExcMetodos.uiLbl = "ucExcMetodos";
            // 
            // ucCimPasInf
            // 
            this.ucCimPasInf.ancho = 200;
            this.ucCimPasInf.isObligatorio = true;
            this.ucCimPasInf.location = new System.Drawing.Point(200, 0);
            this.ucCimPasInf.Location = new System.Drawing.Point(21, 78);
            this.ucCimPasInf.Name = "ucCimPasInf";
            this.ucCimPasInf.Size = new System.Drawing.Size(406, 21);
            this.ucCimPasInf.TabIndex = 7;
            this.ucCimPasInf.uiLbl = "ucCimPasInf";
            // 
            // ucCimViaPu
            // 
            this.ucCimViaPu.ancho = 200;
            this.ucCimViaPu.isObligatorio = true;
            this.ucCimViaPu.location = new System.Drawing.Point(200, 0);
            this.ucCimViaPu.Location = new System.Drawing.Point(21, 37);
            this.ucCimViaPu.Name = "ucCimViaPu";
            this.ucCimViaPu.Size = new System.Drawing.Size(406, 21);
            this.ucCimViaPu.TabIndex = 6;
            this.ucCimViaPu.uiLbl = "ucCimViaPu";
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(411, 498);
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
            this.lnkSave.Location = new System.Drawing.Point(359, 498);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // frmCim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(467, 523);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.grDetalle);
            this.Controls.Add(this.grGeneral);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCim";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCim";
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
        private System.Windows.Forms.LinkLabel lnkSalir;
        private System.Windows.Forms.LinkLabel lnkSave;
        private userControl.ucCimTipos ucAguaPresencia;
        private userControl.ucCimTipos ucExcMetodos;
        private userControl.ucCimTipos ucCimPasInf;
        private userControl.ucCimTipos ucCimViaPu;
    }
}