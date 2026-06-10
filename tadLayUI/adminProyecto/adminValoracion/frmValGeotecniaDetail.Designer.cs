namespace tadLayUI.adminValoracion
{
    partial class frmValGeotecniaDetail
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
            this.ucValoracionProteccionTaludes = new tadLayUI.userControl.ucLblTxt();
            this.ucValoracionExcavabilidad = new tadLayUI.userControl.ucLblTxt();
            this.ucValoracionAprovechamientos = new tadLayUI.userControl.ucLblTxt();
            this.ucValorCBR = new tadLayUI.userControl.ucLblTxt();
            this.ucEstabilidadTaludes = new tadLayUI.userControl.ucLblTxt();
            this.ucEstabilidadHorizontalTerreno = new tadLayUI.userControl.ucLblTxt();
            this.grValTrazado.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucSaveCancel
            // 
            this.ucSaveCancel.Location = new System.Drawing.Point(12, 325);
            this.ucSaveCancel.Name = "ucSaveCancel";
            this.ucSaveCancel.Size = new System.Drawing.Size(260, 25);
            this.ucSaveCancel.TabIndex = 3;
            // 
            // grValTrazado
            // 
            this.grValTrazado.Controls.Add(this.ucValoracionProteccionTaludes);
            this.grValTrazado.Controls.Add(this.ucValoracionExcavabilidad);
            this.grValTrazado.Controls.Add(this.ucValoracionAprovechamientos);
            this.grValTrazado.Controls.Add(this.ucValorCBR);
            this.grValTrazado.Controls.Add(this.ucEstabilidadTaludes);
            this.grValTrazado.Controls.Add(this.ucEstabilidadHorizontalTerreno);
            this.grValTrazado.Location = new System.Drawing.Point(12, 12);
            this.grValTrazado.Name = "grValTrazado";
            this.grValTrazado.Size = new System.Drawing.Size(260, 307);
            this.grValTrazado.TabIndex = 2;
            this.grValTrazado.TabStop = false;
            this.grValTrazado.Text = "grValTrazado";
            // 
            // ucValoracionProteccionTaludes
            // 
            this.ucValoracionProteccionTaludes.ancho = 50;
            this.ucValoracionProteccionTaludes.isEntero = false;
            this.ucValoracionProteccionTaludes.isNegativo = false;
            this.ucValoracionProteccionTaludes.isObligatorio = true;
            this.ucValoracionProteccionTaludes.isSimboloDecimalPunto = true;
            this.ucValoracionProteccionTaludes.location = new System.Drawing.Point(180, 0);
            this.ucValoracionProteccionTaludes.Location = new System.Drawing.Point(20, 265);
            this.ucValoracionProteccionTaludes.lonMax = 32767;
            this.ucValoracionProteccionTaludes.Name = "ucValoracionProteccionTaludes";
            this.ucValoracionProteccionTaludes.Size = new System.Drawing.Size(233, 20);
            this.ucValoracionProteccionTaludes.TabIndex = 5;
            this.ucValoracionProteccionTaludes.uiLbl = "ucValoracionProteccionTaludes";
            this.ucValoracionProteccionTaludes.uitxt = "";
            this.ucValoracionProteccionTaludes.valorMaximo = 100D;
            this.ucValoracionProteccionTaludes.valorMinimo = 0D;
            // 
            // ucValoracionExcavabilidad
            // 
            this.ucValoracionExcavabilidad.ancho = 50;
            this.ucValoracionExcavabilidad.isEntero = false;
            this.ucValoracionExcavabilidad.isNegativo = false;
            this.ucValoracionExcavabilidad.isObligatorio = true;
            this.ucValoracionExcavabilidad.isSimboloDecimalPunto = true;
            this.ucValoracionExcavabilidad.location = new System.Drawing.Point(180, 0);
            this.ucValoracionExcavabilidad.Location = new System.Drawing.Point(21, 217);
            this.ucValoracionExcavabilidad.lonMax = 32767;
            this.ucValoracionExcavabilidad.Name = "ucValoracionExcavabilidad";
            this.ucValoracionExcavabilidad.Size = new System.Drawing.Size(233, 20);
            this.ucValoracionExcavabilidad.TabIndex = 4;
            this.ucValoracionExcavabilidad.uiLbl = "ucValoracionExcavabilidad";
            this.ucValoracionExcavabilidad.uitxt = "";
            this.ucValoracionExcavabilidad.valorMaximo = 100D;
            this.ucValoracionExcavabilidad.valorMinimo = 0D;
            // 
            // ucValoracionAprovechamientos
            // 
            this.ucValoracionAprovechamientos.ancho = 50;
            this.ucValoracionAprovechamientos.isEntero = false;
            this.ucValoracionAprovechamientos.isNegativo = false;
            this.ucValoracionAprovechamientos.isObligatorio = true;
            this.ucValoracionAprovechamientos.isSimboloDecimalPunto = true;
            this.ucValoracionAprovechamientos.location = new System.Drawing.Point(180, 0);
            this.ucValoracionAprovechamientos.Location = new System.Drawing.Point(21, 169);
            this.ucValoracionAprovechamientos.lonMax = 32767;
            this.ucValoracionAprovechamientos.Name = "ucValoracionAprovechamientos";
            this.ucValoracionAprovechamientos.Size = new System.Drawing.Size(233, 20);
            this.ucValoracionAprovechamientos.TabIndex = 3;
            this.ucValoracionAprovechamientos.uiLbl = "ucValoracionAprovechamientos";
            this.ucValoracionAprovechamientos.uitxt = "";
            this.ucValoracionAprovechamientos.valorMaximo = 100D;
            this.ucValoracionAprovechamientos.valorMinimo = 0D;
            // 
            // ucValorCBR
            // 
            this.ucValorCBR.ancho = 50;
            this.ucValorCBR.isEntero = false;
            this.ucValorCBR.isNegativo = false;
            this.ucValorCBR.isObligatorio = true;
            this.ucValorCBR.isSimboloDecimalPunto = true;
            this.ucValorCBR.location = new System.Drawing.Point(180, 0);
            this.ucValorCBR.Location = new System.Drawing.Point(21, 121);
            this.ucValorCBR.lonMax = 32767;
            this.ucValorCBR.Name = "ucValorCBR";
            this.ucValorCBR.Size = new System.Drawing.Size(233, 20);
            this.ucValorCBR.TabIndex = 2;
            this.ucValorCBR.uiLbl = "ucValorCBR";
            this.ucValorCBR.uitxt = "";
            this.ucValorCBR.valorMaximo = 100D;
            this.ucValorCBR.valorMinimo = 0D;
            // 
            // ucEstabilidadTaludes
            // 
            this.ucEstabilidadTaludes.ancho = 50;
            this.ucEstabilidadTaludes.isEntero = false;
            this.ucEstabilidadTaludes.isNegativo = false;
            this.ucEstabilidadTaludes.isObligatorio = true;
            this.ucEstabilidadTaludes.isSimboloDecimalPunto = true;
            this.ucEstabilidadTaludes.location = new System.Drawing.Point(180, 0);
            this.ucEstabilidadTaludes.Location = new System.Drawing.Point(21, 73);
            this.ucEstabilidadTaludes.lonMax = 32767;
            this.ucEstabilidadTaludes.Name = "ucEstabilidadTaludes";
            this.ucEstabilidadTaludes.Size = new System.Drawing.Size(233, 20);
            this.ucEstabilidadTaludes.TabIndex = 1;
            this.ucEstabilidadTaludes.uiLbl = "ucEstabilidadTaludes";
            this.ucEstabilidadTaludes.uitxt = "";
            this.ucEstabilidadTaludes.valorMaximo = 100D;
            this.ucEstabilidadTaludes.valorMinimo = 0D;
            // 
            // ucEstabilidadHorizontalTerreno
            // 
            this.ucEstabilidadHorizontalTerreno.ancho = 50;
            this.ucEstabilidadHorizontalTerreno.isEntero = false;
            this.ucEstabilidadHorizontalTerreno.isNegativo = false;
            this.ucEstabilidadHorizontalTerreno.isObligatorio = true;
            this.ucEstabilidadHorizontalTerreno.isSimboloDecimalPunto = true;
            this.ucEstabilidadHorizontalTerreno.location = new System.Drawing.Point(180, 0);
            this.ucEstabilidadHorizontalTerreno.Location = new System.Drawing.Point(21, 25);
            this.ucEstabilidadHorizontalTerreno.lonMax = 32767;
            this.ucEstabilidadHorizontalTerreno.Name = "ucEstabilidadHorizontalTerreno";
            this.ucEstabilidadHorizontalTerreno.Size = new System.Drawing.Size(233, 20);
            this.ucEstabilidadHorizontalTerreno.TabIndex = 0;
            this.ucEstabilidadHorizontalTerreno.uiLbl = "ucEstabilidadHorizontalTerreno";
            this.ucEstabilidadHorizontalTerreno.uitxt = "";
            this.ucEstabilidadHorizontalTerreno.valorMaximo = 100D;
            this.ucEstabilidadHorizontalTerreno.valorMinimo = 0D;
            // 
            // frmValGeotecniaDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.ucSaveCancel);
            this.Controls.Add(this.grValTrazado);
            this.Name = "frmValGeotecniaDetail";
            this.Text = "frmValGeotecnia";
            this.Load += new System.EventHandler(this.frmValGeotecniaDetail_Load);
            this.grValTrazado.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private userControl.ucToolDetail ucSaveCancel;
        private System.Windows.Forms.GroupBox grValTrazado;
        private userControl.ucLblTxt ucValoracionExcavabilidad;
        private userControl.ucLblTxt ucValoracionAprovechamientos;
        private userControl.ucLblTxt ucValorCBR;
        private userControl.ucLblTxt ucEstabilidadTaludes;
        private userControl.ucLblTxt ucEstabilidadHorizontalTerreno;
        private userControl.ucLblTxt ucValoracionProteccionTaludes;
    }
}