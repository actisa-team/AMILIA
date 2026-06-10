namespace tadLayUI
{
    partial class frmPointEdit
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
            this.ucZ = new tadLayUI.userControl.ucLblTxt();
            this.ucX = new tadLayUI.userControl.ucLblTxt();
            this.ucY = new tadLayUI.userControl.ucLblTxt();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucZ
            // 
            this.ucZ.ancho = 100;
            this.ucZ.isEntero = false;
            this.ucZ.isNegativo = false;
            this.ucZ.isObligatorio = true;
            this.ucZ.isSimboloDecimalPunto = true;
            this.ucZ.location = new System.Drawing.Point(100, 0);
            this.ucZ.Location = new System.Drawing.Point(19, 90);
            this.ucZ.lonMax = 32767;
            this.ucZ.Name = "ucZ";
            this.ucZ.Size = new System.Drawing.Size(205, 20);
            this.ucZ.TabIndex = 5;
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
            this.ucX.Location = new System.Drawing.Point(19, 27);
            this.ucX.lonMax = 32767;
            this.ucX.Name = "ucX";
            this.ucX.Size = new System.Drawing.Size(205, 20);
            this.ucX.TabIndex = 3;
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
            this.ucY.Location = new System.Drawing.Point(19, 59);
            this.ucY.lonMax = 32767;
            this.ucY.Name = "ucY";
            this.ucY.Size = new System.Drawing.Size(205, 20);
            this.ucY.TabIndex = 4;
            this.ucY.uiLbl = "ucY";
            this.ucY.uitxt = "";
            this.ucY.valorDoubleNull = null;
            this.ucY.valorMaximo = -1D;
            this.ucY.valorMinimo = -1D;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucY);
            this.groupBox1.Controls.Add(this.ucZ);
            this.groupBox1.Controls.Add(this.ucX);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(236, 132);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(12, 150);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(236, 25);
            this.ucToolDetail1.TabIndex = 7;
            // 
            // frmPointEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 191);
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPointEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPointEdit";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private userControl.ucLblTxt ucZ;
        private userControl.ucLblTxt ucX;
        private userControl.ucLblTxt ucY;
        private System.Windows.Forms.GroupBox groupBox1;
        private userControl.ucToolDetail ucToolDetail1;
    }
}