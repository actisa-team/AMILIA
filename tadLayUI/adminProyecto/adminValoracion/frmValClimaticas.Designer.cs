namespace tadLayUI.adminValoracion
{
    partial class frmValClimaticasDetail
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
            this.ucSaveCancel = new tadLayUI.userControl.ucToolDetail();
            this.grValTrazado = new System.Windows.Forms.GroupBox();
            this.ucNieblasDensas = new tadLayUI.userControl.ucLblTxt();
            this.ucFuertesVientos = new tadLayUI.userControl.ucLblTxt();
            this.ucNevada = new tadLayUI.userControl.ucLblTxt();
            this.ucLluviasIntensas = new tadLayUI.userControl.ucLblTxt();
            this.ucTormentasFrecuentes = new tadLayUI.userControl.ucLblTxt();
            this.ucUmbria = new tadLayUI.userControl.ucLblTxt();
            this.ucFuertesHeladas = new tadLayUI.userControl.ucLblTxt();
            this.grValTrazado.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucSaveCancel
            // 
            this.ucSaveCancel.Location = new System.Drawing.Point(12, 331);
            this.ucSaveCancel.Name = "ucSaveCancel";
            this.ucSaveCancel.Size = new System.Drawing.Size(275, 25);
            this.ucSaveCancel.TabIndex = 5;
            // 
            // grValTrazado
            // 
            this.grValTrazado.Controls.Add(this.ucNieblasDensas);
            this.grValTrazado.Controls.Add(this.ucFuertesVientos);
            this.grValTrazado.Controls.Add(this.ucNevada);
            this.grValTrazado.Controls.Add(this.ucLluviasIntensas);
            this.grValTrazado.Controls.Add(this.ucTormentasFrecuentes);
            this.grValTrazado.Controls.Add(this.ucUmbria);
            this.grValTrazado.Controls.Add(this.ucFuertesHeladas);
            this.grValTrazado.Location = new System.Drawing.Point(12, 12);
            this.grValTrazado.Name = "grValTrazado";
            this.grValTrazado.Size = new System.Drawing.Size(275, 316);
            this.grValTrazado.TabIndex = 4;
            this.grValTrazado.TabStop = false;
            this.grValTrazado.Text = "grValTrazado";
            // 
            // ucNieblasDensas
            // 
            this.ucNieblasDensas.ancho = 50;
            this.ucNieblasDensas.isEntero = false;
            this.ucNieblasDensas.isNegativo = false;
            this.ucNieblasDensas.isObligatorio = true;
            this.ucNieblasDensas.isSimboloDecimalPunto = true;
            this.ucNieblasDensas.location = new System.Drawing.Point(195, 0);
            this.ucNieblasDensas.Location = new System.Drawing.Point(14, 277);
            this.ucNieblasDensas.lonMax = 32767;
            this.ucNieblasDensas.Name = "ucNieblasDensas";
            this.ucNieblasDensas.Size = new System.Drawing.Size(255, 20);
            this.ucNieblasDensas.TabIndex = 6;
            this.ucNieblasDensas.uiLbl = "ucNieblasDensas";
            this.ucNieblasDensas.uitxt = "";
            this.ucNieblasDensas.valorMaximo = 100D;
            this.ucNieblasDensas.valorMinimo = 0D;
            // 
            // ucFuertesVientos
            // 
            this.ucFuertesVientos.ancho = 50;
            this.ucFuertesVientos.isEntero = false;
            this.ucFuertesVientos.isNegativo = false;
            this.ucFuertesVientos.isObligatorio = true;
            this.ucFuertesVientos.isSimboloDecimalPunto = true;
            this.ucFuertesVientos.location = new System.Drawing.Point(195, 0);
            this.ucFuertesVientos.Location = new System.Drawing.Point(14, 235);
            this.ucFuertesVientos.lonMax = 32767;
            this.ucFuertesVientos.Name = "ucFuertesVientos";
            this.ucFuertesVientos.Size = new System.Drawing.Size(255, 20);
            this.ucFuertesVientos.TabIndex = 5;
            this.ucFuertesVientos.uiLbl = "ucFuertesVientos";
            this.ucFuertesVientos.uitxt = "";
            this.ucFuertesVientos.valorMaximo = 100D;
            this.ucFuertesVientos.valorMinimo = 0D;
            // 
            // ucNevada
            // 
            this.ucNevada.ancho = 50;
            this.ucNevada.isEntero = false;
            this.ucNevada.isNegativo = false;
            this.ucNevada.isObligatorio = true;
            this.ucNevada.isSimboloDecimalPunto = true;
            this.ucNevada.location = new System.Drawing.Point(195, 0);
            this.ucNevada.Location = new System.Drawing.Point(14, 193);
            this.ucNevada.lonMax = 32767;
            this.ucNevada.Name = "ucNevada";
            this.ucNevada.Size = new System.Drawing.Size(255, 20);
            this.ucNevada.TabIndex = 4;
            this.ucNevada.uiLbl = "ucNevada";
            this.ucNevada.uitxt = "";
            this.ucNevada.valorMaximo = 100D;
            this.ucNevada.valorMinimo = 0D;
            // 
            // ucLluviasIntensas
            // 
            this.ucLluviasIntensas.ancho = 50;
            this.ucLluviasIntensas.isEntero = false;
            this.ucLluviasIntensas.isNegativo = false;
            this.ucLluviasIntensas.isObligatorio = true;
            this.ucLluviasIntensas.isSimboloDecimalPunto = true;
            this.ucLluviasIntensas.location = new System.Drawing.Point(195, 0);
            this.ucLluviasIntensas.Location = new System.Drawing.Point(14, 151);
            this.ucLluviasIntensas.lonMax = 32767;
            this.ucLluviasIntensas.Name = "ucLluviasIntensas";
            this.ucLluviasIntensas.Size = new System.Drawing.Size(255, 20);
            this.ucLluviasIntensas.TabIndex = 3;
            this.ucLluviasIntensas.uiLbl = "ucLluviasIntensas";
            this.ucLluviasIntensas.uitxt = "";
            this.ucLluviasIntensas.valorMaximo = 100D;
            this.ucLluviasIntensas.valorMinimo = 0D;
            // 
            // ucTormentasFrecuentes
            // 
            this.ucTormentasFrecuentes.ancho = 50;
            this.ucTormentasFrecuentes.isEntero = false;
            this.ucTormentasFrecuentes.isNegativo = false;
            this.ucTormentasFrecuentes.isObligatorio = true;
            this.ucTormentasFrecuentes.isSimboloDecimalPunto = true;
            this.ucTormentasFrecuentes.location = new System.Drawing.Point(195, 0);
            this.ucTormentasFrecuentes.Location = new System.Drawing.Point(14, 109);
            this.ucTormentasFrecuentes.lonMax = 32767;
            this.ucTormentasFrecuentes.Name = "ucTormentasFrecuentes";
            this.ucTormentasFrecuentes.Size = new System.Drawing.Size(255, 20);
            this.ucTormentasFrecuentes.TabIndex = 2;
            this.ucTormentasFrecuentes.uiLbl = "ucTormentasFrecuentes";
            this.ucTormentasFrecuentes.uitxt = "";
            this.ucTormentasFrecuentes.valorMaximo = 100D;
            this.ucTormentasFrecuentes.valorMinimo = 0D;
            // 
            // ucUmbria
            // 
            this.ucUmbria.ancho = 50;
            this.ucUmbria.isEntero = false;
            this.ucUmbria.isNegativo = false;
            this.ucUmbria.isObligatorio = true;
            this.ucUmbria.isSimboloDecimalPunto = true;
            this.ucUmbria.location = new System.Drawing.Point(195, 0);
            this.ucUmbria.Location = new System.Drawing.Point(14, 67);
            this.ucUmbria.lonMax = 32767;
            this.ucUmbria.Name = "ucUmbria";
            this.ucUmbria.Size = new System.Drawing.Size(255, 20);
            this.ucUmbria.TabIndex = 1;
            this.ucUmbria.uiLbl = "ucUmbria";
            this.ucUmbria.uitxt = "";
            this.ucUmbria.valorMaximo = 100D;
            this.ucUmbria.valorMinimo = 0D;
            // 
            // ucFuertesHeladas
            // 
            this.ucFuertesHeladas.ancho = 50;
            this.ucFuertesHeladas.isEntero = false;
            this.ucFuertesHeladas.isNegativo = false;
            this.ucFuertesHeladas.isObligatorio = true;
            this.ucFuertesHeladas.isSimboloDecimalPunto = true;
            this.ucFuertesHeladas.location = new System.Drawing.Point(195, 0);
            this.ucFuertesHeladas.Location = new System.Drawing.Point(14, 25);
            this.ucFuertesHeladas.lonMax = 32767;
            this.ucFuertesHeladas.Name = "ucFuertesHeladas";
            this.ucFuertesHeladas.Size = new System.Drawing.Size(255, 20);
            this.ucFuertesHeladas.TabIndex = 0;
            this.ucFuertesHeladas.uiLbl = "ucFuertesHeladas";
            this.ucFuertesHeladas.uitxt = "";
            this.ucFuertesHeladas.valorMaximo = 100D;
            this.ucFuertesHeladas.valorMinimo = 0D;
            // 
            // frmValClimaticasDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.ucSaveCancel);
            this.Controls.Add(this.grValTrazado);
            this.Name = "frmValClimaticasDetail";
            this.Text = "frmValClimaticas";
            this.Load += new System.EventHandler(this.frmValClimaticasDetail_Load);
            this.grValTrazado.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private userControl.ucToolDetail ucSaveCancel;
        private System.Windows.Forms.GroupBox grValTrazado;
        private userControl.ucLblTxt ucNieblasDensas;
        private userControl.ucLblTxt ucFuertesVientos;
        private userControl.ucLblTxt ucNevada;
        private userControl.ucLblTxt ucLluviasIntensas;
        private userControl.ucLblTxt ucTormentasFrecuentes;
        private userControl.ucLblTxt ucUmbria;
        private userControl.ucLblTxt ucFuertesHeladas;
    }
}