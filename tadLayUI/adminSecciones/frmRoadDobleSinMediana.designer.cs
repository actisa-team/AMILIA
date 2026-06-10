namespace tadLayUI.adminSecciones
{
    partial class frmRoadDobleSinMediana
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
            this.lnkSalir = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.grGeneral = new System.Windows.Forms.GroupBox();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.grGeometria = new System.Windows.Forms.GroupBox();
            this.ucFirmeIntoArcen = new tadLayUI.userControl.ucLblTxt();
            this.ucBombeoPC = new tadLayUI.userControl.ucLblTxt();
            this.ucArcenIntAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucFirmeTalud = new tadLayUI.userControl.ucLblTxt();
            this.ucBermaExtPendiente = new tadLayUI.userControl.ucLblTxt();
            this.ucBermaExtAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucCarrilAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucArcenExtAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucCarrilesDerNum = new tadLayUI.userControl.ucLblTxt();
            this.ucCarrilesIzqNum = new tadLayUI.userControl.ucLblTxt();
            this.grCunetaDatos = new System.Windows.Forms.GroupBox();
            this.ucCunetaPos = new tadLayUI.userControl.ucCunetaPosicion();
            this.ucCunetaGeoMat = new tadLayUI.userControl.ucCuneta();
            this.grBarrera = new System.Windows.Forms.GroupBox();
            this.ucBarreraDwg = new tadLayUI.userControl.ucLblTxt();
            this.ucFileSelect1 = new tadLayUI.userControl.ucFileSelect();
            this.toolHeader = new System.Windows.Forms.ToolStrip();
            this.lblHeader = new System.Windows.Forms.ToolStripLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnVerSeccion = new System.Windows.Forms.Button();
            this.ucPrecios = new tadLayUI.userControl.ucCunetaPosicion();
            this.grGeneral.SuspendLayout();
            this.grGeometria.SuspendLayout();
            this.grCunetaDatos.SuspendLayout();
            this.grBarrera.SuspendLayout();
            this.toolHeader.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(582, 452);
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
            this.lnkSave.Location = new System.Drawing.Point(531, 452);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // grGeneral
            // 
            this.grGeneral.Controls.Add(this.ucDescripcion);
            this.grGeneral.Controls.Add(this.ucNombre);
            this.grGeneral.Location = new System.Drawing.Point(9, 31);
            this.grGeneral.Name = "grGeneral";
            this.grGeneral.Size = new System.Drawing.Size(340, 131);
            this.grGeneral.TabIndex = 24;
            this.grGeneral.TabStop = false;
            this.grGeneral.Text = "grMaster";
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
            this.ucDescripcion.location = new System.Drawing.Point(100, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(13, 54);
            this.ucDescripcion.lonMax = 50;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(307, 62);
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
            this.ucNombre.location = new System.Drawing.Point(100, 0);
            this.ucNombre.Location = new System.Drawing.Point(13, 23);
            this.ucNombre.lonMax = 50;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(307, 20);
            this.ucNombre.TabIndex = 1;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorDoubleNull = null;
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // grGeometria
            // 
            this.grGeometria.BackColor = System.Drawing.Color.LightGray;
            this.grGeometria.Controls.Add(this.ucFirmeIntoArcen);
            this.grGeometria.Controls.Add(this.ucBombeoPC);
            this.grGeometria.Controls.Add(this.ucArcenIntAncho);
            this.grGeometria.Controls.Add(this.ucFirmeTalud);
            this.grGeometria.Controls.Add(this.ucBermaExtPendiente);
            this.grGeometria.Controls.Add(this.ucBermaExtAncho);
            this.grGeometria.Controls.Add(this.ucCarrilAncho);
            this.grGeometria.Controls.Add(this.ucArcenExtAncho);
            this.grGeometria.Controls.Add(this.ucCarrilesDerNum);
            this.grGeometria.Controls.Add(this.ucCarrilesIzqNum);
            this.grGeometria.Location = new System.Drawing.Point(359, 32);
            this.grGeometria.Name = "grGeometria";
            this.grGeometria.Size = new System.Drawing.Size(270, 329);
            this.grGeometria.TabIndex = 28;
            this.grGeometria.TabStop = false;
            this.grGeometria.Text = "grGeometria";
            // 
            // ucFirmeIntoArcen
            // 
            this.ucFirmeIntoArcen.ancho = 40;
            this.ucFirmeIntoArcen.isEntero = false;
            this.ucFirmeIntoArcen.isNegativo = false;
            this.ucFirmeIntoArcen.isObligatorio = true;
            this.ucFirmeIntoArcen.isSimboloDecimalPunto = true;
            this.ucFirmeIntoArcen.location = new System.Drawing.Point(219, 0);
            this.ucFirmeIntoArcen.Location = new System.Drawing.Point(5, 163);
            this.ucFirmeIntoArcen.lonMax = 32767;
            this.ucFirmeIntoArcen.Name = "ucFirmeIntoArcen";
            this.ucFirmeIntoArcen.Size = new System.Drawing.Size(259, 43);
            this.ucFirmeIntoArcen.TabIndex = 10;
            this.ucFirmeIntoArcen.uiLbl = "ucFirmeIntoArcen";
            this.ucFirmeIntoArcen.uitxt = "";
            this.ucFirmeIntoArcen.valorDoubleNull = null;
            this.ucFirmeIntoArcen.valorMaximo = 10D;
            this.ucFirmeIntoArcen.valorMinimo = 0D;
            // 
            // ucBombeoPC
            // 
            this.ucBombeoPC.ancho = 40;
            this.ucBombeoPC.isEntero = false;
            this.ucBombeoPC.isNegativo = false;
            this.ucBombeoPC.isObligatorio = true;
            this.ucBombeoPC.isSimboloDecimalPunto = true;
            this.ucBombeoPC.location = new System.Drawing.Point(219, 0);
            this.ucBombeoPC.Location = new System.Drawing.Point(5, 302);
            this.ucBombeoPC.lonMax = 32767;
            this.ucBombeoPC.Name = "ucBombeoPC";
            this.ucBombeoPC.Size = new System.Drawing.Size(259, 20);
            this.ucBombeoPC.TabIndex = 9;
            this.ucBombeoPC.uiLbl = "ucBombeoPC";
            this.ucBombeoPC.uitxt = "";
            this.ucBombeoPC.valorDoubleNull = null;
            this.ucBombeoPC.valorMaximo = 10D;
            this.ucBombeoPC.valorMinimo = 0D;
            // 
            // ucArcenIntAncho
            // 
            this.ucArcenIntAncho.ancho = 40;
            this.ucArcenIntAncho.isEntero = false;
            this.ucArcenIntAncho.isNegativo = false;
            this.ucArcenIntAncho.isObligatorio = true;
            this.ucArcenIntAncho.isSimboloDecimalPunto = true;
            this.ucArcenIntAncho.location = new System.Drawing.Point(219, 0);
            this.ucArcenIntAncho.Location = new System.Drawing.Point(5, 136);
            this.ucArcenIntAncho.lonMax = 32767;
            this.ucArcenIntAncho.Name = "ucArcenIntAncho";
            this.ucArcenIntAncho.Size = new System.Drawing.Size(259, 23);
            this.ucArcenIntAncho.TabIndex = 8;
            this.ucArcenIntAncho.uiLbl = "ucArcenIntAncho";
            this.ucArcenIntAncho.uitxt = "";
            this.ucArcenIntAncho.valorDoubleNull = null;
            this.ucArcenIntAncho.valorMaximo = 10D;
            this.ucArcenIntAncho.valorMinimo = 0D;
            // 
            // ucFirmeTalud
            // 
            this.ucFirmeTalud.ancho = 40;
            this.ucFirmeTalud.isEntero = false;
            this.ucFirmeTalud.isNegativo = false;
            this.ucFirmeTalud.isObligatorio = true;
            this.ucFirmeTalud.isSimboloDecimalPunto = true;
            this.ucFirmeTalud.location = new System.Drawing.Point(219, 0);
            this.ucFirmeTalud.Location = new System.Drawing.Point(5, 272);
            this.ucFirmeTalud.lonMax = 32767;
            this.ucFirmeTalud.Name = "ucFirmeTalud";
            this.ucFirmeTalud.Size = new System.Drawing.Size(259, 20);
            this.ucFirmeTalud.TabIndex = 7;
            this.ucFirmeTalud.uiLbl = "ucFirmeTalud";
            this.ucFirmeTalud.uitxt = "";
            this.ucFirmeTalud.valorDoubleNull = null;
            this.ucFirmeTalud.valorMaximo = 10D;
            this.ucFirmeTalud.valorMinimo = 0D;
            // 
            // ucBermaExtPendiente
            // 
            this.ucBermaExtPendiente.ancho = 40;
            this.ucBermaExtPendiente.isEntero = false;
            this.ucBermaExtPendiente.isNegativo = false;
            this.ucBermaExtPendiente.isObligatorio = true;
            this.ucBermaExtPendiente.isSimboloDecimalPunto = true;
            this.ucBermaExtPendiente.location = new System.Drawing.Point(219, 0);
            this.ucBermaExtPendiente.Location = new System.Drawing.Point(5, 242);
            this.ucBermaExtPendiente.lonMax = 32767;
            this.ucBermaExtPendiente.Name = "ucBermaExtPendiente";
            this.ucBermaExtPendiente.Size = new System.Drawing.Size(259, 20);
            this.ucBermaExtPendiente.TabIndex = 6;
            this.ucBermaExtPendiente.uiLbl = "ucBermaExtPendiente";
            this.ucBermaExtPendiente.uitxt = "";
            this.ucBermaExtPendiente.valorDoubleNull = null;
            this.ucBermaExtPendiente.valorMaximo = 100D;
            this.ucBermaExtPendiente.valorMinimo = 0D;
            // 
            // ucBermaExtAncho
            // 
            this.ucBermaExtAncho.ancho = 40;
            this.ucBermaExtAncho.isEntero = false;
            this.ucBermaExtAncho.isNegativo = false;
            this.ucBermaExtAncho.isObligatorio = true;
            this.ucBermaExtAncho.isSimboloDecimalPunto = true;
            this.ucBermaExtAncho.location = new System.Drawing.Point(219, 0);
            this.ucBermaExtAncho.Location = new System.Drawing.Point(5, 212);
            this.ucBermaExtAncho.lonMax = 32767;
            this.ucBermaExtAncho.Name = "ucBermaExtAncho";
            this.ucBermaExtAncho.Size = new System.Drawing.Size(259, 20);
            this.ucBermaExtAncho.TabIndex = 5;
            this.ucBermaExtAncho.uiLbl = "ucBermaExtAncho";
            this.ucBermaExtAncho.uitxt = "";
            this.ucBermaExtAncho.valorDoubleNull = null;
            this.ucBermaExtAncho.valorMaximo = 10D;
            this.ucBermaExtAncho.valorMinimo = 0D;
            // 
            // ucCarrilAncho
            // 
            this.ucCarrilAncho.ancho = 40;
            this.ucCarrilAncho.isEntero = false;
            this.ucCarrilAncho.isNegativo = false;
            this.ucCarrilAncho.isObligatorio = true;
            this.ucCarrilAncho.isSimboloDecimalPunto = true;
            this.ucCarrilAncho.location = new System.Drawing.Point(219, 0);
            this.ucCarrilAncho.Location = new System.Drawing.Point(5, 18);
            this.ucCarrilAncho.lonMax = 32767;
            this.ucCarrilAncho.Name = "ucCarrilAncho";
            this.ucCarrilAncho.Size = new System.Drawing.Size(259, 20);
            this.ucCarrilAncho.TabIndex = 1;
            this.ucCarrilAncho.uiLbl = "ucCarrilAncho";
            this.ucCarrilAncho.uitxt = "";
            this.ucCarrilAncho.valorDoubleNull = null;
            this.ucCarrilAncho.valorMaximo = 10D;
            this.ucCarrilAncho.valorMinimo = 0D;
            // 
            // ucArcenExtAncho
            // 
            this.ucArcenExtAncho.ancho = 40;
            this.ucArcenExtAncho.isEntero = false;
            this.ucArcenExtAncho.isNegativo = false;
            this.ucArcenExtAncho.isObligatorio = true;
            this.ucArcenExtAncho.isSimboloDecimalPunto = true;
            this.ucArcenExtAncho.location = new System.Drawing.Point(219, 0);
            this.ucArcenExtAncho.Location = new System.Drawing.Point(5, 108);
            this.ucArcenExtAncho.lonMax = 32767;
            this.ucArcenExtAncho.Name = "ucArcenExtAncho";
            this.ucArcenExtAncho.Size = new System.Drawing.Size(259, 20);
            this.ucArcenExtAncho.TabIndex = 4;
            this.ucArcenExtAncho.uiLbl = "ucArcenExtAncho";
            this.ucArcenExtAncho.uitxt = "";
            this.ucArcenExtAncho.valorDoubleNull = null;
            this.ucArcenExtAncho.valorMaximo = 10D;
            this.ucArcenExtAncho.valorMinimo = 0D;
            // 
            // ucCarrilesDerNum
            // 
            this.ucCarrilesDerNum.ancho = 40;
            this.ucCarrilesDerNum.isEntero = true;
            this.ucCarrilesDerNum.isNegativo = false;
            this.ucCarrilesDerNum.isObligatorio = true;
            this.ucCarrilesDerNum.isSimboloDecimalPunto = true;
            this.ucCarrilesDerNum.location = new System.Drawing.Point(219, 0);
            this.ucCarrilesDerNum.Location = new System.Drawing.Point(5, 78);
            this.ucCarrilesDerNum.lonMax = 32767;
            this.ucCarrilesDerNum.Name = "ucCarrilesDerNum";
            this.ucCarrilesDerNum.Size = new System.Drawing.Size(259, 20);
            this.ucCarrilesDerNum.TabIndex = 3;
            this.ucCarrilesDerNum.uiLbl = "ucCarrilesDerNum";
            this.ucCarrilesDerNum.uitxt = "";
            this.ucCarrilesDerNum.valorDoubleNull = null;
            this.ucCarrilesDerNum.valorMaximo = 10D;
            this.ucCarrilesDerNum.valorMinimo = 0D;
            // 
            // ucCarrilesIzqNum
            // 
            this.ucCarrilesIzqNum.ancho = 40;
            this.ucCarrilesIzqNum.isEntero = true;
            this.ucCarrilesIzqNum.isNegativo = false;
            this.ucCarrilesIzqNum.isObligatorio = true;
            this.ucCarrilesIzqNum.isSimboloDecimalPunto = true;
            this.ucCarrilesIzqNum.location = new System.Drawing.Point(219, 0);
            this.ucCarrilesIzqNum.Location = new System.Drawing.Point(5, 48);
            this.ucCarrilesIzqNum.lonMax = 32767;
            this.ucCarrilesIzqNum.Name = "ucCarrilesIzqNum";
            this.ucCarrilesIzqNum.Size = new System.Drawing.Size(259, 20);
            this.ucCarrilesIzqNum.TabIndex = 2;
            this.ucCarrilesIzqNum.uiLbl = "ucCarrilesIzqNum";
            this.ucCarrilesIzqNum.uitxt = "";
            this.ucCarrilesIzqNum.valorDoubleNull = null;
            this.ucCarrilesIzqNum.valorMaximo = 10D;
            this.ucCarrilesIzqNum.valorMinimo = 0D;
            // 
            // grCunetaDatos
            // 
            this.grCunetaDatos.Controls.Add(this.ucCunetaPos);
            this.grCunetaDatos.Controls.Add(this.ucCunetaGeoMat);
            this.grCunetaDatos.Location = new System.Drawing.Point(9, 165);
            this.grCunetaDatos.Name = "grCunetaDatos";
            this.grCunetaDatos.Size = new System.Drawing.Size(340, 178);
            this.grCunetaDatos.TabIndex = 30;
            this.grCunetaDatos.TabStop = false;
            this.grCunetaDatos.Text = "grCunetaDatos";
            // 
            // ucCunetaPos
            // 
            this.ucCunetaPos.ancho = 215;
            this.ucCunetaPos.isObligatorio = false;
            this.ucCunetaPos.location = new System.Drawing.Point(93, 0);
            this.ucCunetaPos.Location = new System.Drawing.Point(16, 143);
            this.ucCunetaPos.Name = "ucCunetaPos";
            this.ucCunetaPos.Size = new System.Drawing.Size(309, 21);
            this.ucCunetaPos.TabIndex = 32;
            this.ucCunetaPos.uiLbl = "Posición cuneta";
            // 
            // ucCunetaGeoMat
            // 
            this.ucCunetaGeoMat.ancho = 215;
            this.ucCunetaGeoMat.isObligatorio = true;
            this.ucCunetaGeoMat.Location = new System.Drawing.Point(16, 25);
            this.ucCunetaGeoMat.locationGeo = new System.Drawing.Point(93, 44);
            this.ucCunetaGeoMat.locationMat = new System.Drawing.Point(93, 79);
            this.ucCunetaGeoMat.locationTipo = new System.Drawing.Point(94, 13);
            this.ucCunetaGeoMat.Name = "ucCunetaGeoMat";
            this.ucCunetaGeoMat.Size = new System.Drawing.Size(315, 109);
            this.ucCunetaGeoMat.TabIndex = 31;
            // 
            // grBarrera
            // 
            this.grBarrera.Controls.Add(this.ucBarreraDwg);
            this.grBarrera.Controls.Add(this.ucFileSelect1);
            this.grBarrera.Location = new System.Drawing.Point(12, 376);
            this.grBarrera.Name = "grBarrera";
            this.grBarrera.Size = new System.Drawing.Size(337, 71);
            this.grBarrera.TabIndex = 31;
            this.grBarrera.TabStop = false;
            this.grBarrera.Text = "grBarrera";
            // 
            // ucBarreraDwg
            // 
            this.ucBarreraDwg.ancho = 190;
            this.ucBarreraDwg.isEntero = false;
            this.ucBarreraDwg.isNegativo = false;
            this.ucBarreraDwg.isObligatorio = true;
            this.ucBarreraDwg.isSimboloDecimalPunto = true;
            this.ucBarreraDwg.isTexto = true;
            this.ucBarreraDwg.location = new System.Drawing.Point(95, 0);
            this.ucBarreraDwg.Location = new System.Drawing.Point(10, 31);
            this.ucBarreraDwg.lonMax = 50;
            this.ucBarreraDwg.Name = "ucBarreraDwg";
            this.ucBarreraDwg.Size = new System.Drawing.Size(288, 20);
            this.ucBarreraDwg.TabIndex = 2;
            this.ucBarreraDwg.uiLbl = "ucBarreraDwg";
            this.ucBarreraDwg.uitxt = "";
            this.ucBarreraDwg.valorDoubleNull = null;
            this.ucBarreraDwg.valorMaximo = 0D;
            this.ucBarreraDwg.valorMinimo = 0D;
            // 
            // ucFileSelect1
            // 
            this.ucFileSelect1.Location = new System.Drawing.Point(304, 28);
            this.ucFileSelect1.Name = "ucFileSelect1";
            this.ucFileSelect1.Size = new System.Drawing.Size(25, 25);
            this.ucFileSelect1.TabIndex = 0;
            // 
            // toolHeader
            // 
            this.toolHeader.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHeader});
            this.toolHeader.Location = new System.Drawing.Point(0, 0);
            this.toolHeader.Name = "toolHeader";
            this.toolHeader.Size = new System.Drawing.Size(631, 25);
            this.toolHeader.TabIndex = 32;
            this.toolHeader.Text = "toolHeader";
            // 
            // lblHeader
            // 
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(58, 22);
            this.lblHeader.Text = "lblHeader";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnVerSeccion);
            this.groupBox1.Location = new System.Drawing.Point(359, 376);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 71);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            // 
            // btnVerSeccion
            // 
            this.btnVerSeccion.Location = new System.Drawing.Point(19, 31);
            this.btnVerSeccion.Name = "btnVerSeccion";
            this.btnVerSeccion.Size = new System.Drawing.Size(241, 23);
            this.btnVerSeccion.TabIndex = 0;
            this.btnVerSeccion.Text = "btnVerSeccion";
            this.btnVerSeccion.UseVisualStyleBackColor = true;
            this.btnVerSeccion.Click += new System.EventHandler(this.btnVerSeccion_Click);
            // 
            // ucPrecios
            // 
            this.ucPrecios.ancho = 215;
            this.ucPrecios.isObligatorio = false;
            this.ucPrecios.location = new System.Drawing.Point(93, 0);
            this.ucPrecios.Location = new System.Drawing.Point(25, 349);
            this.ucPrecios.Name = "ucPrecios";
            this.ucPrecios.Size = new System.Drawing.Size(309, 21);
            this.ucPrecios.TabIndex = 34;
            this.ucPrecios.uiLbl = "Posición cuneta";
            // 
            // frmRoadDobleSinMediana
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(631, 473);
            this.Controls.Add(this.ucPrecios);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolHeader);
            this.Controls.Add(this.grBarrera);
            this.Controls.Add(this.grCunetaDatos);
            this.Controls.Add(this.grGeometria);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.grGeneral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRoadDobleSinMediana";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEst_FormClosing);
            this.grGeneral.ResumeLayout(false);
            this.grGeometria.ResumeLayout(false);
            this.grCunetaDatos.ResumeLayout(false);
            this.grBarrera.ResumeLayout(false);
            this.toolHeader.ResumeLayout(false);
            this.toolHeader.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSalir;
        private System.Windows.Forms.LinkLabel lnkSave;
        private userControl.ucLblTxt ucNombre;
        private userControl.ucLblTxt ucDescripcion;
        private System.Windows.Forms.GroupBox grGeneral;
        private System.Windows.Forms.GroupBox grGeometria;
        private userControl.ucLblTxt ucFirmeTalud;
        private userControl.ucLblTxt ucBermaExtPendiente;
        private userControl.ucLblTxt ucBermaExtAncho;
        private userControl.ucLblTxt ucCarrilAncho;
        private userControl.ucLblTxt ucArcenExtAncho;
        private userControl.ucLblTxt ucCarrilesDerNum;
        private userControl.ucLblTxt ucCarrilesIzqNum;
        private userControl.ucCuneta ucCunetaGeoMat;
        private System.Windows.Forms.GroupBox grCunetaDatos;
        private userControl.ucCunetaPosicion ucCunetaPos;
        private userControl.ucLblTxt ucArcenIntAncho;
        private System.Windows.Forms.GroupBox grBarrera;
        private userControl.ucLblTxt ucBarreraDwg;
        private userControl.ucFileSelect ucFileSelect1;
        private System.Windows.Forms.ToolStrip toolHeader;
        private System.Windows.Forms.ToolStripLabel lblHeader;
        private userControl.ucLblTxt ucBombeoPC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnVerSeccion;
        private userControl.ucLblTxt ucFirmeIntoArcen;
        private userControl.ucCunetaPosicion ucPrecios;
    }
}