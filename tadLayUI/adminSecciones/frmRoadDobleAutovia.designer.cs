namespace tadLayUI.adminSecciones
{
    partial class frmRoadDobleAutovia
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
            this.ucBombeoPC = new tadLayUI.userControl.ucLblTxt();
            this.ucFirmeIntoArcen = new tadLayUI.userControl.ucLblTxt();
            this.ucBermaExtAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucBermaIntPendiente = new tadLayUI.userControl.ucLblTxt();
            this.ucArcenIntAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucFirmeTalud = new tadLayUI.userControl.ucLblTxt();
            this.ucBermaExtPendiente = new tadLayUI.userControl.ucLblTxt();
            this.ucMedianaAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucCarrilAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucArcenExtAncho = new tadLayUI.userControl.ucLblTxt();
            this.ucCarrilesDerNum = new tadLayUI.userControl.ucLblTxt();
            this.ucCarrilesIzqNum = new tadLayUI.userControl.ucLblTxt();
            this.grCunetaExterior = new System.Windows.Forms.GroupBox();
            this.ucCunetaPos = new tadLayUI.userControl.ucCunetaPosicion();
            this.ucCunetaExtGeoMat = new tadLayUI.userControl.ucCuneta();
            this.grCunetaInterior = new System.Windows.Forms.GroupBox();
            this.ucCunetaInterior = new tadLayUI.userControl.ucCuneta();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnVerSeccion = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblHeader = new System.Windows.Forms.ToolStripLabel();
            this.ucPrecios = new tadLayUI.userControl.ucCunetaPosicion();
            this.grGeneral.SuspendLayout();
            this.grGeometria.SuspendLayout();
            this.grCunetaExterior.SuspendLayout();
            this.grCunetaInterior.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(587, 529);
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
            this.lnkSave.Location = new System.Drawing.Point(535, 529);
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
            this.ucDescripcion.Location = new System.Drawing.Point(21, 54);
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
            this.ucNombre.Location = new System.Drawing.Point(21, 23);
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
            this.grGeometria.Controls.Add(this.ucBombeoPC);
            this.grGeometria.Controls.Add(this.ucFirmeIntoArcen);
            this.grGeometria.Controls.Add(this.ucBermaExtAncho);
            this.grGeometria.Controls.Add(this.ucBermaIntPendiente);
            this.grGeometria.Controls.Add(this.ucArcenIntAncho);
            this.grGeometria.Controls.Add(this.ucFirmeTalud);
            this.grGeometria.Controls.Add(this.ucBermaExtPendiente);
            this.grGeometria.Controls.Add(this.ucMedianaAncho);
            this.grGeometria.Controls.Add(this.ucCarrilAncho);
            this.grGeometria.Controls.Add(this.ucArcenExtAncho);
            this.grGeometria.Controls.Add(this.ucCarrilesDerNum);
            this.grGeometria.Controls.Add(this.ucCarrilesIzqNum);
            this.grGeometria.Location = new System.Drawing.Point(355, 32);
            this.grGeometria.Name = "grGeometria";
            this.grGeometria.Size = new System.Drawing.Size(273, 487);
            this.grGeometria.TabIndex = 28;
            this.grGeometria.TabStop = false;
            this.grGeometria.Text = "grGeometria";
            // 
            // ucBombeoPC
            // 
            this.ucBombeoPC.ancho = 40;
            this.ucBombeoPC.isEntero = false;
            this.ucBombeoPC.isNegativo = false;
            this.ucBombeoPC.isObligatorio = true;
            this.ucBombeoPC.isSimboloDecimalPunto = true;
            this.ucBombeoPC.location = new System.Drawing.Point(219, 0);
            this.ucBombeoPC.Location = new System.Drawing.Point(4, 450);
            this.ucBombeoPC.lonMax = 32767;
            this.ucBombeoPC.Name = "ucBombeoPC";
            this.ucBombeoPC.Size = new System.Drawing.Size(260, 20);
            this.ucBombeoPC.TabIndex = 13;
            this.ucBombeoPC.uiLbl = "ucBombeoPC";
            this.ucBombeoPC.uitxt = "";
            this.ucBombeoPC.valorDoubleNull = null;
            this.ucBombeoPC.valorMaximo = 10D;
            this.ucBombeoPC.valorMinimo = 0D;
            // 
            // ucFirmeIntoArcen
            // 
            this.ucFirmeIntoArcen.ancho = 40;
            this.ucFirmeIntoArcen.isEntero = false;
            this.ucFirmeIntoArcen.isNegativo = false;
            this.ucFirmeIntoArcen.isObligatorio = true;
            this.ucFirmeIntoArcen.isSimboloDecimalPunto = true;
            this.ucFirmeIntoArcen.location = new System.Drawing.Point(219, 0);
            this.ucFirmeIntoArcen.Location = new System.Drawing.Point(4, 139);
            this.ucFirmeIntoArcen.lonMax = 32767;
            this.ucFirmeIntoArcen.Name = "ucFirmeIntoArcen";
            this.ucFirmeIntoArcen.Size = new System.Drawing.Size(260, 40);
            this.ucFirmeIntoArcen.TabIndex = 12;
            this.ucFirmeIntoArcen.uiLbl = "ucFirmeIntoArcen";
            this.ucFirmeIntoArcen.uitxt = "";
            this.ucFirmeIntoArcen.valorDoubleNull = null;
            this.ucFirmeIntoArcen.valorMaximo = 10D;
            this.ucFirmeIntoArcen.valorMinimo = 0D;
            // 
            // ucBermaExtAncho
            // 
            this.ucBermaExtAncho.ancho = 40;
            this.ucBermaExtAncho.isEntero = false;
            this.ucBermaExtAncho.isNegativo = false;
            this.ucBermaExtAncho.isObligatorio = true;
            this.ucBermaExtAncho.isSimboloDecimalPunto = true;
            this.ucBermaExtAncho.location = new System.Drawing.Point(219, 0);
            this.ucBermaExtAncho.Location = new System.Drawing.Point(4, 260);
            this.ucBermaExtAncho.lonMax = 32767;
            this.ucBermaExtAncho.Name = "ucBermaExtAncho";
            this.ucBermaExtAncho.Size = new System.Drawing.Size(260, 20);
            this.ucBermaExtAncho.TabIndex = 11;
            this.ucBermaExtAncho.uiLbl = "ucBermaExtAncho";
            this.ucBermaExtAncho.uitxt = "";
            this.ucBermaExtAncho.valorDoubleNull = null;
            this.ucBermaExtAncho.valorMaximo = 100D;
            this.ucBermaExtAncho.valorMinimo = 0D;
            // 
            // ucBermaIntPendiente
            // 
            this.ucBermaIntPendiente.ancho = 40;
            this.ucBermaIntPendiente.isEntero = false;
            this.ucBermaIntPendiente.isNegativo = false;
            this.ucBermaIntPendiente.isObligatorio = true;
            this.ucBermaIntPendiente.isSimboloDecimalPunto = true;
            this.ucBermaIntPendiente.location = new System.Drawing.Point(219, 0);
            this.ucBermaIntPendiente.Location = new System.Drawing.Point(5, 336);
            this.ucBermaIntPendiente.lonMax = 32767;
            this.ucBermaIntPendiente.Name = "ucBermaIntPendiente";
            this.ucBermaIntPendiente.Size = new System.Drawing.Size(260, 20);
            this.ucBermaIntPendiente.TabIndex = 10;
            this.ucBermaIntPendiente.uiLbl = "ucBermaIntPendiente";
            this.ucBermaIntPendiente.uitxt = "";
            this.ucBermaIntPendiente.valorDoubleNull = null;
            this.ucBermaIntPendiente.valorMaximo = 10D;
            this.ucBermaIntPendiente.valorMinimo = 0D;
            // 
            // ucArcenIntAncho
            // 
            this.ucArcenIntAncho.ancho = 40;
            this.ucArcenIntAncho.isEntero = false;
            this.ucArcenIntAncho.isNegativo = false;
            this.ucArcenIntAncho.isObligatorio = true;
            this.ucArcenIntAncho.isSimboloDecimalPunto = true;
            this.ucArcenIntAncho.location = new System.Drawing.Point(219, 0);
            this.ucArcenIntAncho.Location = new System.Drawing.Point(5, 222);
            this.ucArcenIntAncho.lonMax = 32767;
            this.ucArcenIntAncho.Name = "ucArcenIntAncho";
            this.ucArcenIntAncho.Size = new System.Drawing.Size(260, 20);
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
            this.ucFirmeTalud.Location = new System.Drawing.Point(5, 412);
            this.ucFirmeTalud.lonMax = 32767;
            this.ucFirmeTalud.Name = "ucFirmeTalud";
            this.ucFirmeTalud.Size = new System.Drawing.Size(260, 20);
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
            this.ucBermaExtPendiente.Location = new System.Drawing.Point(5, 298);
            this.ucBermaExtPendiente.lonMax = 32767;
            this.ucBermaExtPendiente.Name = "ucBermaExtPendiente";
            this.ucBermaExtPendiente.Size = new System.Drawing.Size(260, 20);
            this.ucBermaExtPendiente.TabIndex = 6;
            this.ucBermaExtPendiente.uiLbl = "ucBermaExtPendiente";
            this.ucBermaExtPendiente.uitxt = "";
            this.ucBermaExtPendiente.valorDoubleNull = null;
            this.ucBermaExtPendiente.valorMaximo = 100D;
            this.ucBermaExtPendiente.valorMinimo = 0D;
            // 
            // ucMedianaAncho
            // 
            this.ucMedianaAncho.ancho = 40;
            this.ucMedianaAncho.isEntero = false;
            this.ucMedianaAncho.isNegativo = false;
            this.ucMedianaAncho.isObligatorio = true;
            this.ucMedianaAncho.isSimboloDecimalPunto = true;
            this.ucMedianaAncho.location = new System.Drawing.Point(219, 0);
            this.ucMedianaAncho.Location = new System.Drawing.Point(5, 374);
            this.ucMedianaAncho.lonMax = 32767;
            this.ucMedianaAncho.Name = "ucMedianaAncho";
            this.ucMedianaAncho.Size = new System.Drawing.Size(260, 20);
            this.ucMedianaAncho.TabIndex = 5;
            this.ucMedianaAncho.uiLbl = "ucMedianaAncho";
            this.ucMedianaAncho.uitxt = "";
            this.ucMedianaAncho.valorDoubleNull = null;
            this.ucMedianaAncho.valorMaximo = 10D;
            this.ucMedianaAncho.valorMinimo = 0D;
            // 
            // ucCarrilAncho
            // 
            this.ucCarrilAncho.ancho = 40;
            this.ucCarrilAncho.isEntero = false;
            this.ucCarrilAncho.isNegativo = false;
            this.ucCarrilAncho.isObligatorio = true;
            this.ucCarrilAncho.isSimboloDecimalPunto = true;
            this.ucCarrilAncho.location = new System.Drawing.Point(219, 0);
            this.ucCarrilAncho.Location = new System.Drawing.Point(5, 32);
            this.ucCarrilAncho.lonMax = 32767;
            this.ucCarrilAncho.Name = "ucCarrilAncho";
            this.ucCarrilAncho.Size = new System.Drawing.Size(260, 20);
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
            this.ucArcenExtAncho.Location = new System.Drawing.Point(5, 184);
            this.ucArcenExtAncho.lonMax = 32767;
            this.ucArcenExtAncho.Name = "ucArcenExtAncho";
            this.ucArcenExtAncho.Size = new System.Drawing.Size(260, 20);
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
            this.ucCarrilesDerNum.Location = new System.Drawing.Point(5, 108);
            this.ucCarrilesDerNum.lonMax = 32767;
            this.ucCarrilesDerNum.Name = "ucCarrilesDerNum";
            this.ucCarrilesDerNum.Size = new System.Drawing.Size(260, 20);
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
            this.ucCarrilesIzqNum.Location = new System.Drawing.Point(5, 70);
            this.ucCarrilesIzqNum.lonMax = 32767;
            this.ucCarrilesIzqNum.Name = "ucCarrilesIzqNum";
            this.ucCarrilesIzqNum.Size = new System.Drawing.Size(260, 20);
            this.ucCarrilesIzqNum.TabIndex = 2;
            this.ucCarrilesIzqNum.uiLbl = "ucCarrilesIzqNum";
            this.ucCarrilesIzqNum.uitxt = "";
            this.ucCarrilesIzqNum.valorDoubleNull = null;
            this.ucCarrilesIzqNum.valorMaximo = 10D;
            this.ucCarrilesIzqNum.valorMinimo = 0D;
            // 
            // grCunetaExterior
            // 
            this.grCunetaExterior.Controls.Add(this.ucCunetaPos);
            this.grCunetaExterior.Controls.Add(this.ucCunetaExtGeoMat);
            this.grCunetaExterior.Location = new System.Drawing.Point(9, 165);
            this.grCunetaExterior.Name = "grCunetaExterior";
            this.grCunetaExterior.Size = new System.Drawing.Size(340, 164);
            this.grCunetaExterior.TabIndex = 30;
            this.grCunetaExterior.TabStop = false;
            this.grCunetaExterior.Text = "grCunetaExterior";
            // 
            // ucCunetaPos
            // 
            this.ucCunetaPos.ancho = 215;
            this.ucCunetaPos.isObligatorio = false;
            this.ucCunetaPos.location = new System.Drawing.Point(93, 0);
            this.ucCunetaPos.Location = new System.Drawing.Point(16, 125);
            this.ucCunetaPos.Name = "ucCunetaPos";
            this.ucCunetaPos.Size = new System.Drawing.Size(309, 21);
            this.ucCunetaPos.TabIndex = 32;
            this.ucCunetaPos.uiLbl = "Posición cuneta";
            // 
            // ucCunetaExtGeoMat
            // 
            this.ucCunetaExtGeoMat.ancho = 215;
            this.ucCunetaExtGeoMat.isObligatorio = true;
            this.ucCunetaExtGeoMat.Location = new System.Drawing.Point(16, 13);
            this.ucCunetaExtGeoMat.locationGeo = new System.Drawing.Point(93, 44);
            this.ucCunetaExtGeoMat.locationMat = new System.Drawing.Point(93, 79);
            this.ucCunetaExtGeoMat.locationTipo = new System.Drawing.Point(94, 13);
            this.ucCunetaExtGeoMat.Name = "ucCunetaExtGeoMat";
            this.ucCunetaExtGeoMat.Size = new System.Drawing.Size(315, 109);
            this.ucCunetaExtGeoMat.TabIndex = 31;
            // 
            // grCunetaInterior
            // 
            this.grCunetaInterior.Controls.Add(this.ucCunetaInterior);
            this.grCunetaInterior.Location = new System.Drawing.Point(12, 331);
            this.grCunetaInterior.Name = "grCunetaInterior";
            this.grCunetaInterior.Size = new System.Drawing.Size(337, 130);
            this.grCunetaInterior.TabIndex = 31;
            this.grCunetaInterior.TabStop = false;
            this.grCunetaInterior.Text = "grCunetaInterior";
            // 
            // ucCunetaInterior
            // 
            this.ucCunetaInterior.ancho = 215;
            this.ucCunetaInterior.isObligatorio = true;
            this.ucCunetaInterior.Location = new System.Drawing.Point(11, 14);
            this.ucCunetaInterior.locationGeo = new System.Drawing.Point(93, 44);
            this.ucCunetaInterior.locationMat = new System.Drawing.Point(93, 79);
            this.ucCunetaInterior.locationTipo = new System.Drawing.Point(94, 13);
            this.ucCunetaInterior.Name = "ucCunetaInterior";
            this.ucCunetaInterior.Size = new System.Drawing.Size(315, 109);
            this.ucCunetaInterior.TabIndex = 32;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnVerSeccion);
            this.groupBox1.Location = new System.Drawing.Point(9, 500);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 51);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            // 
            // btnVerSeccion
            // 
            this.btnVerSeccion.Location = new System.Drawing.Point(16, 19);
            this.btnVerSeccion.Name = "btnVerSeccion";
            this.btnVerSeccion.Size = new System.Drawing.Size(309, 23);
            this.btnVerSeccion.TabIndex = 0;
            this.btnVerSeccion.Text = "btnVerSeccion";
            this.btnVerSeccion.UseVisualStyleBackColor = true;
            this.btnVerSeccion.Click += new System.EventHandler(this.btnVerSeccion_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHeader});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(631, 25);
            this.toolStrip1.TabIndex = 33;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblHeader
            // 
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(58, 22);
            this.lblHeader.Text = "lblHeader";
            // 
            // ucPrecios
            // 
            this.ucPrecios.ancho = 215;
            this.ucPrecios.isObligatorio = false;
            this.ucPrecios.location = new System.Drawing.Point(93, 0);
            this.ucPrecios.Location = new System.Drawing.Point(25, 473);
            this.ucPrecios.Name = "ucPrecios";
            this.ucPrecios.Size = new System.Drawing.Size(309, 21);
            this.ucPrecios.TabIndex = 35;
            this.ucPrecios.uiLbl = "Posición cuneta";
            // 
            // frmRoadDobleAutovia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(631, 563);
            this.Controls.Add(this.ucPrecios);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grCunetaInterior);
            this.Controls.Add(this.grCunetaExterior);
            this.Controls.Add(this.grGeometria);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.grGeneral);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRoadDobleAutovia";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEst_FormClosing);
            this.grGeneral.ResumeLayout(false);
            this.grGeometria.ResumeLayout(false);
            this.grCunetaExterior.ResumeLayout(false);
            this.grCunetaInterior.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private userControl.ucLblTxt ucMedianaAncho;
        private userControl.ucLblTxt ucCarrilAncho;
        private userControl.ucLblTxt ucArcenExtAncho;
        private userControl.ucLblTxt ucCarrilesDerNum;
        private userControl.ucLblTxt ucCarrilesIzqNum;
        private userControl.ucCuneta ucCunetaExtGeoMat;
        private System.Windows.Forms.GroupBox grCunetaExterior;
        private userControl.ucCunetaPosicion ucCunetaPos;
        private userControl.ucLblTxt ucBermaIntPendiente;
        private userControl.ucLblTxt ucArcenIntAncho;
        private System.Windows.Forms.GroupBox grCunetaInterior;
        private userControl.ucCuneta ucCunetaInterior;
        private userControl.ucLblTxt ucBermaExtAncho;
        private userControl.ucLblTxt ucFirmeIntoArcen;
        private userControl.ucLblTxt ucBombeoPC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnVerSeccion;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblHeader;
        private userControl.ucCunetaPosicion ucPrecios;
    }
}