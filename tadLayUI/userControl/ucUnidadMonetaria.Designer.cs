namespace tadLayUI.userControl
{
    partial class ucUnidadMonetaria
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
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.grAnalisis = new System.Windows.Forms.GroupBox();
            this.ucUnidadMonetariaDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucUnidadMonetariaSimbolo = new tadLayUI.userControl.ucLblTxt();
            this.grAnalisis.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(3, 130);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(433, 25);
            this.ucToolDetail1.TabIndex = 5;
            // 
            // grAnalisis
            // 
            this.grAnalisis.Controls.Add(this.ucUnidadMonetariaDescripcion);
            this.grAnalisis.Controls.Add(this.ucUnidadMonetariaSimbolo);
            this.grAnalisis.Location = new System.Drawing.Point(3, 3);
            this.grAnalisis.Name = "grAnalisis";
            this.grAnalisis.Size = new System.Drawing.Size(433, 121);
            this.grAnalisis.TabIndex = 4;
            this.grAnalisis.TabStop = false;
            // 
            // ucUnidadMonetariaDescripcion
            // 
            this.ucUnidadMonetariaDescripcion.ancho = 225;
            this.ucUnidadMonetariaDescripcion.isEntero = false;
            this.ucUnidadMonetariaDescripcion.isNegativo = false;
            this.ucUnidadMonetariaDescripcion.isObligatorio = true;
            this.ucUnidadMonetariaDescripcion.isSimboloDecimalPunto = true;
            this.ucUnidadMonetariaDescripcion.isTexto = true;
            this.ucUnidadMonetariaDescripcion.location = new System.Drawing.Point(175, 0);
            this.ucUnidadMonetariaDescripcion.Location = new System.Drawing.Point(15, 77);
            this.ucUnidadMonetariaDescripcion.lonMax = 32767;
            this.ucUnidadMonetariaDescripcion.Name = "ucUnidadMonetariaDescripcion";
            this.ucUnidadMonetariaDescripcion.Size = new System.Drawing.Size(412, 20);
            this.ucUnidadMonetariaDescripcion.TabIndex = 1;
            this.ucUnidadMonetariaDescripcion.uiLbl = "ucUnidadMonetariaDescripcion";
            this.ucUnidadMonetariaDescripcion.uitxt = "";
            this.ucUnidadMonetariaDescripcion.valorDoubleNull = null;
            this.ucUnidadMonetariaDescripcion.valorMaximo = -1D;
            this.ucUnidadMonetariaDescripcion.valorMinimo = 0D;
            // 
            // ucUnidadMonetariaSimbolo
            // 
            this.ucUnidadMonetariaSimbolo.ancho = 75;
            this.ucUnidadMonetariaSimbolo.isEntero = false;
            this.ucUnidadMonetariaSimbolo.isNegativo = false;
            this.ucUnidadMonetariaSimbolo.isObligatorio = true;
            this.ucUnidadMonetariaSimbolo.isSimboloDecimalPunto = true;
            this.ucUnidadMonetariaSimbolo.isTexto = true;
            this.ucUnidadMonetariaSimbolo.location = new System.Drawing.Point(175, 0);
            this.ucUnidadMonetariaSimbolo.Location = new System.Drawing.Point(16, 29);
            this.ucUnidadMonetariaSimbolo.lonMax = 32767;
            this.ucUnidadMonetariaSimbolo.Name = "ucUnidadMonetariaSimbolo";
            this.ucUnidadMonetariaSimbolo.Size = new System.Drawing.Size(322, 20);
            this.ucUnidadMonetariaSimbolo.TabIndex = 0;
            this.ucUnidadMonetariaSimbolo.uiLbl = "ucUnidadMonetariaSimbolo";
            this.ucUnidadMonetariaSimbolo.uitxt = "";
            this.ucUnidadMonetariaSimbolo.valorDoubleNull = null;
            this.ucUnidadMonetariaSimbolo.valorMaximo = -1D;
            this.ucUnidadMonetariaSimbolo.valorMinimo = 0D;
            // 
            // ucUnidadMonetaria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.grAnalisis);
            this.Name = "ucUnidadMonetaria";
            this.Size = new System.Drawing.Size(447, 165);
            this.grAnalisis.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ucToolDetail ucToolDetail1;
        private System.Windows.Forms.GroupBox grAnalisis;
        private ucLblTxt ucUnidadMonetariaDescripcion;
        private ucLblTxt ucUnidadMonetariaSimbolo;
    }
}
