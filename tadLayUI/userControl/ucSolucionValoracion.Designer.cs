namespace tadLayUI.userControl
{
    partial class ucSolucionValoracion
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
            this.grValoracion = new System.Windows.Forms.GroupBox();
            this.ucValoracionCostesPC = new tadLayUI.userControl.ucLblTxt();
            this.ucValoracionPendientePC = new tadLayUI.userControl.ucLblTxt();
            this.ucValoracionDistanciaPC = new tadLayUI.userControl.ucLblTxt();
            this.grValoracion.SuspendLayout();
            this.SuspendLayout();
            // 
            // grValoracion
            // 
            this.grValoracion.Controls.Add(this.ucValoracionCostesPC);
            this.grValoracion.Controls.Add(this.ucValoracionPendientePC);
            this.grValoracion.Controls.Add(this.ucValoracionDistanciaPC);
            this.grValoracion.Location = new System.Drawing.Point(6, 4);
            this.grValoracion.Name = "grValoracion";
            this.grValoracion.Size = new System.Drawing.Size(458, 193);
            this.grValoracion.TabIndex = 1;
            this.grValoracion.TabStop = false;
            this.grValoracion.Text = "grValoracion";
            // 
            // ucValoracionCostesPC
            // 
            this.ucValoracionCostesPC.ancho = 75;
            this.ucValoracionCostesPC.isEntero = true;
            this.ucValoracionCostesPC.isNegativo = false;
            this.ucValoracionCostesPC.isObligatorio = true;
            this.ucValoracionCostesPC.isSimboloDecimalPunto = true;
            this.ucValoracionCostesPC.location = new System.Drawing.Point(250, 0);
            this.ucValoracionCostesPC.Location = new System.Drawing.Point(19, 150);
            this.ucValoracionCostesPC.lonMax = 32767;
            this.ucValoracionCostesPC.Name = "ucValoracionCostesPC";
            this.ucValoracionCostesPC.Size = new System.Drawing.Size(364, 20);
            this.ucValoracionCostesPC.TabIndex = 2;
            this.ucValoracionCostesPC.uiLbl = "ucValoracionCostesPC";
            this.ucValoracionCostesPC.uitxt = "";
            this.ucValoracionCostesPC.valorDoubleNull = null;
            this.ucValoracionCostesPC.valorMaximo = 100D;
            this.ucValoracionCostesPC.valorMinimo = 0D;
            // 
            // ucValoracionPendientePC
            // 
            this.ucValoracionPendientePC.ancho = 75;
            this.ucValoracionPendientePC.isEntero = true;
            this.ucValoracionPendientePC.isNegativo = false;
            this.ucValoracionPendientePC.isObligatorio = true;
            this.ucValoracionPendientePC.isSimboloDecimalPunto = true;
            this.ucValoracionPendientePC.location = new System.Drawing.Point(250, 0);
            this.ucValoracionPendientePC.Location = new System.Drawing.Point(19, 89);
            this.ucValoracionPendientePC.lonMax = 32767;
            this.ucValoracionPendientePC.Name = "ucValoracionPendientePC";
            this.ucValoracionPendientePC.Size = new System.Drawing.Size(364, 20);
            this.ucValoracionPendientePC.TabIndex = 1;
            this.ucValoracionPendientePC.uiLbl = "ucValoracionPendientePC";
            this.ucValoracionPendientePC.uitxt = "";
            this.ucValoracionPendientePC.valorDoubleNull = null;
            this.ucValoracionPendientePC.valorMaximo = 100D;
            this.ucValoracionPendientePC.valorMinimo = 0D;
            // 
            // ucValoracionDistanciaPC
            // 
            this.ucValoracionDistanciaPC.ancho = 75;
            this.ucValoracionDistanciaPC.isEntero = true;
            this.ucValoracionDistanciaPC.isNegativo = false;
            this.ucValoracionDistanciaPC.isObligatorio = true;
            this.ucValoracionDistanciaPC.isSimboloDecimalPunto = true;
            this.ucValoracionDistanciaPC.location = new System.Drawing.Point(250, 0);
            this.ucValoracionDistanciaPC.Location = new System.Drawing.Point(19, 28);
            this.ucValoracionDistanciaPC.lonMax = 32767;
            this.ucValoracionDistanciaPC.Name = "ucValoracionDistanciaPC";
            this.ucValoracionDistanciaPC.Size = new System.Drawing.Size(376, 20);
            this.ucValoracionDistanciaPC.TabIndex = 0;
            this.ucValoracionDistanciaPC.uiLbl = "ucValoracionDistanciaPC";
            this.ucValoracionDistanciaPC.uitxt = "";
            this.ucValoracionDistanciaPC.valorDoubleNull = null;
            this.ucValoracionDistanciaPC.valorMaximo = 100D;
            this.ucValoracionDistanciaPC.valorMinimo = 0D;
            // 
            // ucSolucionValoracion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grValoracion);
            this.Name = "ucSolucionValoracion";
            this.Size = new System.Drawing.Size(500, 200);
            this.grValoracion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ucLblTxt ucValoracionDistanciaPC;
        private System.Windows.Forms.GroupBox grValoracion;
        private ucLblTxt ucValoracionCostesPC;
        private ucLblTxt ucValoracionPendientePC;
    }
}
