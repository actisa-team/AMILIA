namespace tadLayUI.userControl
{
    partial class ucPtoIniFin
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkIsLongitudRecta = new System.Windows.Forms.CheckBox();
            this.chkIsAzimut = new System.Windows.Forms.CheckBox();
            this.chkIsLongitud = new System.Windows.Forms.CheckBox();
            this.btnSelectPoint = new System.Windows.Forms.Button();
            this.grAzimut = new System.Windows.Forms.GroupBox();
            this.ucAzimut = new tadLayUI.userControl.ucLblTxt();
            this.grLon = new System.Windows.Forms.GroupBox();
            this.ucLongitud = new tadLayUI.userControl.ucLblTxt();
            this.grPendiente = new System.Windows.Forms.GroupBox();
            this.ucPendiente = new tadLayUI.userControl.ucLblTxt();
            this.chkIsPendiente = new System.Windows.Forms.CheckBox();
            this.grPto = new System.Windows.Forms.GroupBox();
            this.btnEditPoint = new System.Windows.Forms.Button();
            this.ucZ = new tadLayUI.userControl.ucLblTxt();
            this.ucX = new tadLayUI.userControl.ucLblTxt();
            this.ucY = new tadLayUI.userControl.ucLblTxt();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.button1 = new System.Windows.Forms.Button();
            this.grAzimut.SuspendLayout();
            this.grLon.SuspendLayout();
            this.grPendiente.SuspendLayout();
            this.grPto.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIsLongitudRecta
            // 
            this.chkIsLongitudRecta.AutoSize = true;
            this.chkIsLongitudRecta.Location = new System.Drawing.Point(26, 52);
            this.chkIsLongitudRecta.Name = "chkIsLongitudRecta";
            this.chkIsLongitudRecta.Size = new System.Drawing.Size(122, 17);
            this.chkIsLongitudRecta.TabIndex = 6;
            this.chkIsLongitudRecta.Text = "chkIsLongitudRecta";
            this.chkIsLongitudRecta.UseVisualStyleBackColor = true;
            // 
            // chkIsAzimut
            // 
            this.chkIsAzimut.AutoSize = true;
            this.chkIsAzimut.Location = new System.Drawing.Point(16, 219);
            this.chkIsAzimut.Name = "chkIsAzimut";
            this.chkIsAzimut.Size = new System.Drawing.Size(83, 17);
            this.chkIsAzimut.TabIndex = 2;
            this.chkIsAzimut.Text = "chkIsAzimut";
            this.chkIsAzimut.UseVisualStyleBackColor = true;
            this.chkIsAzimut.CheckedChanged += new System.EventHandler(this.chkIsAzimut_CheckedChanged);
            // 
            // chkIsLongitud
            // 
            this.chkIsLongitud.AutoSize = true;
            this.chkIsLongitud.Location = new System.Drawing.Point(14, 293);
            this.chkIsLongitud.Name = "chkIsLongitud";
            this.chkIsLongitud.Size = new System.Drawing.Size(93, 17);
            this.chkIsLongitud.TabIndex = 4;
            this.chkIsLongitud.Text = "chkIsLongitud";
            this.chkIsLongitud.UseVisualStyleBackColor = true;
            this.chkIsLongitud.CheckedChanged += new System.EventHandler(this.chkIsLongitud_CheckedChanged);
            // 
            // btnSelectPoint
            // 
            this.btnSelectPoint.Location = new System.Drawing.Point(12, 116);
            this.btnSelectPoint.Name = "btnSelectPoint";
            this.btnSelectPoint.Size = new System.Drawing.Size(220, 23);
            this.btnSelectPoint.TabIndex = 9;
            this.btnSelectPoint.Text = "btnSelectPoint";
            this.btnSelectPoint.UseVisualStyleBackColor = true;
            this.btnSelectPoint.Click += new System.EventHandler(this.btnSelectPoint_Click);
            // 
            // grAzimut
            // 
            this.grAzimut.Controls.Add(this.ucAzimut);
            this.grAzimut.Location = new System.Drawing.Point(14, 234);
            this.grAzimut.Name = "grAzimut";
            this.grAzimut.Size = new System.Drawing.Size(250, 53);
            this.grAzimut.TabIndex = 3;
            this.grAzimut.TabStop = false;
            // 
            // ucAzimut
            // 
            this.ucAzimut.ancho = 75;
            this.ucAzimut.isEntero = false;
            this.ucAzimut.isNegativo = false;
            this.ucAzimut.isObligatorio = true;
            this.ucAzimut.isSimboloDecimalPunto = true;
            this.ucAzimut.location = new System.Drawing.Point(150, 0);
            this.ucAzimut.Location = new System.Drawing.Point(6, 19);
            this.ucAzimut.lonMax = 32767;
            this.ucAzimut.Name = "ucAzimut";
            this.ucAzimut.Size = new System.Drawing.Size(238, 20);
            this.ucAzimut.TabIndex = 3;
            this.ucAzimut.uiLbl = "ucAzimutGrados";
            this.ucAzimut.uitxt = "";
            this.ucAzimut.valorDoubleNull = null;
            this.ucAzimut.valorMaximo = 360D;
            this.ucAzimut.valorMinimo = 0D;
            // 
            // grLon
            // 
            this.grLon.Controls.Add(this.ucLongitud);
            this.grLon.Controls.Add(this.chkIsLongitudRecta);
            this.grLon.Location = new System.Drawing.Point(14, 312);
            this.grLon.Name = "grLon";
            this.grLon.Size = new System.Drawing.Size(250, 81);
            this.grLon.TabIndex = 5;
            this.grLon.TabStop = false;
            // 
            // ucLongitud
            // 
            this.ucLongitud.ancho = 75;
            this.ucLongitud.isEntero = false;
            this.ucLongitud.isNegativo = false;
            this.ucLongitud.isObligatorio = true;
            this.ucLongitud.isSimboloDecimalPunto = true;
            this.ucLongitud.location = new System.Drawing.Point(150, 0);
            this.ucLongitud.Location = new System.Drawing.Point(9, 19);
            this.ucLongitud.lonMax = 32767;
            this.ucLongitud.Name = "ucLongitud";
            this.ucLongitud.Size = new System.Drawing.Size(236, 20);
            this.ucLongitud.TabIndex = 4;
            this.ucLongitud.uiLbl = "ucLongitud";
            this.ucLongitud.uitxt = "";
            this.ucLongitud.valorDoubleNull = null;
            this.ucLongitud.valorMaximo = -1D;
            this.ucLongitud.valorMinimo = -1D;
            // 
            // grPendiente
            // 
            this.grPendiente.Controls.Add(this.ucPendiente);
            this.grPendiente.Location = new System.Drawing.Point(12, 424);
            this.grPendiente.Name = "grPendiente";
            this.grPendiente.Size = new System.Drawing.Size(252, 68);
            this.grPendiente.TabIndex = 7;
            this.grPendiente.TabStop = false;
            // 
            // ucPendiente
            // 
            this.ucPendiente.ancho = 75;
            this.ucPendiente.isEntero = false;
            this.ucPendiente.isNegativo = true;
            this.ucPendiente.isObligatorio = true;
            this.ucPendiente.isSimboloDecimalPunto = true;
            this.ucPendiente.location = new System.Drawing.Point(150, 0);
            this.ucPendiente.Location = new System.Drawing.Point(11, 29);
            this.ucPendiente.lonMax = 32767;
            this.ucPendiente.Name = "ucPendiente";
            this.ucPendiente.Size = new System.Drawing.Size(235, 20);
            this.ucPendiente.TabIndex = 5;
            this.ucPendiente.uiLbl = "ucPendiente";
            this.ucPendiente.uitxt = "";
            this.ucPendiente.valorDoubleNull = null;
            this.ucPendiente.valorMaximo = -1D;
            this.ucPendiente.valorMinimo = -1D;
            // 
            // chkIsPendiente
            // 
            this.chkIsPendiente.AutoSize = true;
            this.chkIsPendiente.Location = new System.Drawing.Point(14, 403);
            this.chkIsPendiente.Name = "chkIsPendiente";
            this.chkIsPendiente.Size = new System.Drawing.Size(100, 17);
            this.chkIsPendiente.TabIndex = 6;
            this.chkIsPendiente.Text = "chkIsPendiente";
            this.chkIsPendiente.UseVisualStyleBackColor = true;
            this.chkIsPendiente.CheckedChanged += new System.EventHandler(this.chkIsPendiente_CheckedChanged);
            // 
            // grPto
            // 
            this.grPto.Controls.Add(this.button1);
            this.grPto.Controls.Add(this.btnEditPoint);
            this.grPto.Controls.Add(this.ucZ);
            this.grPto.Controls.Add(this.ucX);
            this.grPto.Controls.Add(this.ucY);
            this.grPto.Controls.Add(this.btnSelectPoint);
            this.grPto.Location = new System.Drawing.Point(16, 9);
            this.grPto.Name = "grPto";
            this.grPto.Size = new System.Drawing.Size(248, 204);
            this.grPto.TabIndex = 1;
            this.grPto.TabStop = false;
            this.grPto.Text = "grPto";
            // 
            // btnEditPoint
            // 
            this.btnEditPoint.Location = new System.Drawing.Point(12, 146);
            this.btnEditPoint.Name = "btnEditPoint";
            this.btnEditPoint.Size = new System.Drawing.Size(220, 23);
            this.btnEditPoint.TabIndex = 10;
            this.btnEditPoint.Text = "btnEditPoint";
            this.btnEditPoint.UseVisualStyleBackColor = true;
            this.btnEditPoint.Click += new System.EventHandler(this.btnEditPoint_Click);
            // 
            // ucZ
            // 
            this.ucZ.ancho = 100;
            this.ucZ.isEntero = false;
            this.ucZ.isNegativo = false;
            this.ucZ.isObligatorio = true;
            this.ucZ.isSimboloDecimalPunto = true;
            this.ucZ.location = new System.Drawing.Point(100, 0);
            this.ucZ.Location = new System.Drawing.Point(27, 84);
            this.ucZ.lonMax = 32767;
            this.ucZ.Name = "ucZ";
            this.ucZ.Size = new System.Drawing.Size(205, 20);
            this.ucZ.TabIndex = 2;
            this.ucZ.uiLbl = "ucZ";
            this.ucZ.uitxt = "";
            this.ucZ.valorDoubleNull = null;
            this.ucZ.valorMaximo = -1D;
            this.ucZ.valorMinimo = -1D;
            // 
            // ucX
            // 
            this.ucX.ancho = 100;
            this.ucX.isEntero = false;
            this.ucX.isNegativo = false;
            this.ucX.isObligatorio = true;
            this.ucX.isSimboloDecimalPunto = true;
            this.ucX.location = new System.Drawing.Point(100, 0);
            this.ucX.Location = new System.Drawing.Point(27, 32);
            this.ucX.lonMax = 32767;
            this.ucX.Name = "ucX";
            this.ucX.Size = new System.Drawing.Size(205, 20);
            this.ucX.TabIndex = 0;
            this.ucX.uiLbl = "ucX";
            this.ucX.uitxt = "";
            this.ucX.valorDoubleNull = null;
            this.ucX.valorMaximo = -1D;
            this.ucX.valorMinimo = -1D;
            // 
            // ucY
            // 
            this.ucY.ancho = 100;
            this.ucY.isEntero = false;
            this.ucY.isNegativo = false;
            this.ucY.isObligatorio = true;
            this.ucY.isSimboloDecimalPunto = true;
            this.ucY.location = new System.Drawing.Point(100, 0);
            this.ucY.Location = new System.Drawing.Point(27, 57);
            this.ucY.lonMax = 32767;
            this.ucY.Name = "ucY";
            this.ucY.Size = new System.Drawing.Size(205, 20);
            this.ucY.TabIndex = 1;
            this.ucY.uiLbl = "ucY";
            this.ucY.uitxt = "";
            this.ucY.valorDoubleNull = null;
            this.ucY.valorMaximo = -1D;
            this.ucY.valorMinimo = -1D;
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(3, 498);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(261, 25);
            this.ucToolDetail1.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(220, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Dibujar puntos";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ucPtoIniFin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.grPto);
            this.Controls.Add(this.chkIsPendiente);
            this.Controls.Add(this.grPendiente);
            this.Controls.Add(this.grLon);
            this.Controls.Add(this.grAzimut);
            this.Controls.Add(this.chkIsLongitud);
            this.Controls.Add(this.chkIsAzimut);
            this.Name = "ucPtoIniFin";
            this.Size = new System.Drawing.Size(276, 561);
            this.Load += new System.EventHandler(this.ucPtoIniFin_Load);
            this.grAzimut.ResumeLayout(false);
            this.grLon.ResumeLayout(false);
            this.grLon.PerformLayout();
            this.grPendiente.ResumeLayout(false);
            this.grPto.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ucLblTxt ucX;
        private ucLblTxt ucY;
        private ucLblTxt ucZ;
        private ucLblTxt ucAzimut;
        private ucLblTxt ucLongitud;
        private ucLblTxt ucPendiente;
        private System.Windows.Forms.CheckBox chkIsLongitudRecta;
        private System.Windows.Forms.CheckBox chkIsAzimut;
        private System.Windows.Forms.CheckBox chkIsLongitud;
        private System.Windows.Forms.Button btnSelectPoint;
        private System.Windows.Forms.GroupBox grAzimut;
        private System.Windows.Forms.GroupBox grLon;
        private System.Windows.Forms.GroupBox grPendiente;
        private System.Windows.Forms.CheckBox chkIsPendiente;
        private System.Windows.Forms.GroupBox grPto;
        private ucToolDetail ucToolDetail1;
        private System.Windows.Forms.Button btnEditPoint;
        private System.Windows.Forms.Button button1;
    }
}
