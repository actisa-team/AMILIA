namespace tadLayUI.adminGis.valoracion
{
    partial class frmValCimentacion
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
            this.grValCimEstructurasPasosInferiores = new System.Windows.Forms.GroupBox();
            this.ucCimDirectaSaneosValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucCimProfundaValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucCimPozosValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucCimDirectaValoracion = new tadLayUI.userControl.ucLblTxt();
            this.grValExcavacionProcedimientos = new System.Windows.Forms.GroupBox();
            this.ucExcMartilloValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucExcPilotadorasValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucExcVoladurasValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucExcMediosConvencionalesValoracion = new tadLayUI.userControl.ucLblTxt();
            this.grValPresenciaAgua = new System.Windows.Forms.GroupBox();
            this.ucAguaAgotamientoValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucAguaSistemasEspecialesValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucAguaSinAfeccionValoracion = new tadLayUI.userControl.ucLblTxt();
            this.ucSaveCancel = new tadLayUI.userControl.ucToolDetail();
            this.grValCimEstructurasPasosInferiores.SuspendLayout();
            this.grValExcavacionProcedimientos.SuspendLayout();
            this.grValPresenciaAgua.SuspendLayout();
            this.SuspendLayout();
            // 
            // grValCimEstructurasPasosInferiores
            // 
            this.grValCimEstructurasPasosInferiores.Controls.Add(this.ucCimDirectaSaneosValoracion);
            this.grValCimEstructurasPasosInferiores.Controls.Add(this.ucCimProfundaValoracion);
            this.grValCimEstructurasPasosInferiores.Controls.Add(this.ucCimPozosValoracion);
            this.grValCimEstructurasPasosInferiores.Controls.Add(this.ucCimDirectaValoracion);
            this.grValCimEstructurasPasosInferiores.Location = new System.Drawing.Point(1, 8);
            this.grValCimEstructurasPasosInferiores.Name = "grValCimEstructurasPasosInferiores";
            this.grValCimEstructurasPasosInferiores.Size = new System.Drawing.Size(506, 154);
            this.grValCimEstructurasPasosInferiores.TabIndex = 4;
            this.grValCimEstructurasPasosInferiores.TabStop = false;
            this.grValCimEstructurasPasosInferiores.Text = "grValCimEstructurasPasosInferiores";
            // 
            // ucCimDirectaSaneosValoracion
            // 
            this.ucCimDirectaSaneosValoracion.ancho = 50;
            this.ucCimDirectaSaneosValoracion.isEntero = false;
            this.ucCimDirectaSaneosValoracion.isNegativo = false;
            this.ucCimDirectaSaneosValoracion.isObligatorio = true;
            this.ucCimDirectaSaneosValoracion.isSimboloDecimalPunto = true;
            this.ucCimDirectaSaneosValoracion.location = new System.Drawing.Point(425, 0);
            this.ucCimDirectaSaneosValoracion.Location = new System.Drawing.Point(21, 57);
            this.ucCimDirectaSaneosValoracion.lonMax = 32767;
            this.ucCimDirectaSaneosValoracion.Name = "ucCimDirectaSaneosValoracion";
            this.ucCimDirectaSaneosValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucCimDirectaSaneosValoracion.TabIndex = 4;
            this.ucCimDirectaSaneosValoracion.uiLbl = "ucCimDirectaSaneosValoracion";
            this.ucCimDirectaSaneosValoracion.uitxt = "";
            this.ucCimDirectaSaneosValoracion.valorMaximo = 10D;
            this.ucCimDirectaSaneosValoracion.valorMinimo = 0D;
            // 
            // ucCimProfundaValoracion
            // 
            this.ucCimProfundaValoracion.ancho = 50;
            this.ucCimProfundaValoracion.isEntero = false;
            this.ucCimProfundaValoracion.isNegativo = false;
            this.ucCimProfundaValoracion.isObligatorio = true;
            this.ucCimProfundaValoracion.isSimboloDecimalPunto = true;
            this.ucCimProfundaValoracion.location = new System.Drawing.Point(425, 0);
            this.ucCimProfundaValoracion.Location = new System.Drawing.Point(21, 123);
            this.ucCimProfundaValoracion.lonMax = 32767;
            this.ucCimProfundaValoracion.Name = "ucCimProfundaValoracion";
            this.ucCimProfundaValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucCimProfundaValoracion.TabIndex = 3;
            this.ucCimProfundaValoracion.uiLbl = "ucCimProfundaValoracion";
            this.ucCimProfundaValoracion.uitxt = "";
            this.ucCimProfundaValoracion.valorMaximo = 10D;
            this.ucCimProfundaValoracion.valorMinimo = 0D;
            // 
            // ucCimPozosValoracion
            // 
            this.ucCimPozosValoracion.ancho = 50;
            this.ucCimPozosValoracion.isEntero = false;
            this.ucCimPozosValoracion.isNegativo = false;
            this.ucCimPozosValoracion.isObligatorio = true;
            this.ucCimPozosValoracion.isSimboloDecimalPunto = true;
            this.ucCimPozosValoracion.location = new System.Drawing.Point(425, 0);
            this.ucCimPozosValoracion.Location = new System.Drawing.Point(21, 90);
            this.ucCimPozosValoracion.lonMax = 32767;
            this.ucCimPozosValoracion.Name = "ucCimPozosValoracion";
            this.ucCimPozosValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucCimPozosValoracion.TabIndex = 2;
            this.ucCimPozosValoracion.uiLbl = "ucCimPozosValoracion";
            this.ucCimPozosValoracion.uitxt = "";
            this.ucCimPozosValoracion.valorMaximo = 10D;
            this.ucCimPozosValoracion.valorMinimo = 0D;
            // 
            // ucCimDirectaValoracion
            // 
            this.ucCimDirectaValoracion.ancho = 50;
            this.ucCimDirectaValoracion.isEntero = false;
            this.ucCimDirectaValoracion.isNegativo = false;
            this.ucCimDirectaValoracion.isObligatorio = true;
            this.ucCimDirectaValoracion.isSimboloDecimalPunto = true;
            this.ucCimDirectaValoracion.location = new System.Drawing.Point(425, 0);
            this.ucCimDirectaValoracion.Location = new System.Drawing.Point(21, 25);
            this.ucCimDirectaValoracion.lonMax = 32767;
            this.ucCimDirectaValoracion.Name = "ucCimDirectaValoracion";
            this.ucCimDirectaValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucCimDirectaValoracion.TabIndex = 0;
            this.ucCimDirectaValoracion.uiLbl = "ucCimDirectaValoracion";
            this.ucCimDirectaValoracion.uitxt = "";
            this.ucCimDirectaValoracion.valorMaximo = 10D;
            this.ucCimDirectaValoracion.valorMinimo = 0D;
            // 
            // grValExcavacionProcedimientos
            // 
            this.grValExcavacionProcedimientos.Controls.Add(this.ucExcMartilloValoracion);
            this.grValExcavacionProcedimientos.Controls.Add(this.ucExcPilotadorasValoracion);
            this.grValExcavacionProcedimientos.Controls.Add(this.ucExcVoladurasValoracion);
            this.grValExcavacionProcedimientos.Controls.Add(this.ucExcMediosConvencionalesValoracion);
            this.grValExcavacionProcedimientos.Location = new System.Drawing.Point(1, 165);
            this.grValExcavacionProcedimientos.Name = "grValExcavacionProcedimientos";
            this.grValExcavacionProcedimientos.Size = new System.Drawing.Size(506, 155);
            this.grValExcavacionProcedimientos.TabIndex = 5;
            this.grValExcavacionProcedimientos.TabStop = false;
            this.grValExcavacionProcedimientos.Text = "grValExcavacionProcedimientos";
            // 
            // ucExcMartilloValoracion
            // 
            this.ucExcMartilloValoracion.ancho = 50;
            this.ucExcMartilloValoracion.isEntero = false;
            this.ucExcMartilloValoracion.isNegativo = false;
            this.ucExcMartilloValoracion.isObligatorio = true;
            this.ucExcMartilloValoracion.isSimboloDecimalPunto = true;
            this.ucExcMartilloValoracion.location = new System.Drawing.Point(425, 0);
            this.ucExcMartilloValoracion.Location = new System.Drawing.Point(21, 56);
            this.ucExcMartilloValoracion.lonMax = 32767;
            this.ucExcMartilloValoracion.Name = "ucExcMartilloValoracion";
            this.ucExcMartilloValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucExcMartilloValoracion.TabIndex = 4;
            this.ucExcMartilloValoracion.uiLbl = "ucExcMartilloValoracion";
            this.ucExcMartilloValoracion.uitxt = "";
            this.ucExcMartilloValoracion.valorMaximo = 10D;
            this.ucExcMartilloValoracion.valorMinimo = 0D;
            // 
            // ucExcPilotadorasValoracion
            // 
            this.ucExcPilotadorasValoracion.ancho = 50;
            this.ucExcPilotadorasValoracion.isEntero = false;
            this.ucExcPilotadorasValoracion.isNegativo = false;
            this.ucExcPilotadorasValoracion.isObligatorio = true;
            this.ucExcPilotadorasValoracion.isSimboloDecimalPunto = true;
            this.ucExcPilotadorasValoracion.location = new System.Drawing.Point(425, 0);
            this.ucExcPilotadorasValoracion.Location = new System.Drawing.Point(21, 122);
            this.ucExcPilotadorasValoracion.lonMax = 32767;
            this.ucExcPilotadorasValoracion.Name = "ucExcPilotadorasValoracion";
            this.ucExcPilotadorasValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucExcPilotadorasValoracion.TabIndex = 3;
            this.ucExcPilotadorasValoracion.uiLbl = "ucExcPilotadorasValoracion";
            this.ucExcPilotadorasValoracion.uitxt = "";
            this.ucExcPilotadorasValoracion.valorMaximo = 10D;
            this.ucExcPilotadorasValoracion.valorMinimo = 0D;
            // 
            // ucExcVoladurasValoracion
            // 
            this.ucExcVoladurasValoracion.ancho = 50;
            this.ucExcVoladurasValoracion.isEntero = false;
            this.ucExcVoladurasValoracion.isNegativo = false;
            this.ucExcVoladurasValoracion.isObligatorio = true;
            this.ucExcVoladurasValoracion.isSimboloDecimalPunto = true;
            this.ucExcVoladurasValoracion.location = new System.Drawing.Point(425, 0);
            this.ucExcVoladurasValoracion.Location = new System.Drawing.Point(21, 89);
            this.ucExcVoladurasValoracion.lonMax = 32767;
            this.ucExcVoladurasValoracion.Name = "ucExcVoladurasValoracion";
            this.ucExcVoladurasValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucExcVoladurasValoracion.TabIndex = 2;
            this.ucExcVoladurasValoracion.uiLbl = "ucExcVoladurasValoracion";
            this.ucExcVoladurasValoracion.uitxt = "";
            this.ucExcVoladurasValoracion.valorMaximo = 10D;
            this.ucExcVoladurasValoracion.valorMinimo = 0D;
            // 
            // ucExcMediosConvencionalesValoracion
            // 
            this.ucExcMediosConvencionalesValoracion.ancho = 50;
            this.ucExcMediosConvencionalesValoracion.isEntero = false;
            this.ucExcMediosConvencionalesValoracion.isNegativo = false;
            this.ucExcMediosConvencionalesValoracion.isObligatorio = true;
            this.ucExcMediosConvencionalesValoracion.isSimboloDecimalPunto = true;
            this.ucExcMediosConvencionalesValoracion.location = new System.Drawing.Point(425, 0);
            this.ucExcMediosConvencionalesValoracion.Location = new System.Drawing.Point(21, 23);
            this.ucExcMediosConvencionalesValoracion.lonMax = 32767;
            this.ucExcMediosConvencionalesValoracion.Name = "ucExcMediosConvencionalesValoracion";
            this.ucExcMediosConvencionalesValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucExcMediosConvencionalesValoracion.TabIndex = 0;
            this.ucExcMediosConvencionalesValoracion.uiLbl = "ucExcMediosConvencionalesValoracion";
            this.ucExcMediosConvencionalesValoracion.uitxt = "";
            this.ucExcMediosConvencionalesValoracion.valorMaximo = 10D;
            this.ucExcMediosConvencionalesValoracion.valorMinimo = 0D;
            // 
            // grValPresenciaAgua
            // 
            this.grValPresenciaAgua.Controls.Add(this.ucAguaAgotamientoValoracion);
            this.grValPresenciaAgua.Controls.Add(this.ucAguaSistemasEspecialesValoracion);
            this.grValPresenciaAgua.Controls.Add(this.ucAguaSinAfeccionValoracion);
            this.grValPresenciaAgua.Location = new System.Drawing.Point(1, 329);
            this.grValPresenciaAgua.Name = "grValPresenciaAgua";
            this.grValPresenciaAgua.Size = new System.Drawing.Size(506, 119);
            this.grValPresenciaAgua.TabIndex = 6;
            this.grValPresenciaAgua.TabStop = false;
            this.grValPresenciaAgua.Text = "grValPresenciaAgua";
            // 
            // ucAguaAgotamientoValoracion
            // 
            this.ucAguaAgotamientoValoracion.ancho = 50;
            this.ucAguaAgotamientoValoracion.isEntero = false;
            this.ucAguaAgotamientoValoracion.isNegativo = false;
            this.ucAguaAgotamientoValoracion.isObligatorio = true;
            this.ucAguaAgotamientoValoracion.isSimboloDecimalPunto = true;
            this.ucAguaAgotamientoValoracion.location = new System.Drawing.Point(425, 0);
            this.ucAguaAgotamientoValoracion.Location = new System.Drawing.Point(21, 56);
            this.ucAguaAgotamientoValoracion.lonMax = 32767;
            this.ucAguaAgotamientoValoracion.Name = "ucAguaAgotamientoValoracion";
            this.ucAguaAgotamientoValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucAguaAgotamientoValoracion.TabIndex = 4;
            this.ucAguaAgotamientoValoracion.uiLbl = "ucAguaAgotamientoValoracion";
            this.ucAguaAgotamientoValoracion.uitxt = "";
            this.ucAguaAgotamientoValoracion.valorMaximo = 10D;
            this.ucAguaAgotamientoValoracion.valorMinimo = 0D;
            // 
            // ucAguaSistemasEspecialesValoracion
            // 
            this.ucAguaSistemasEspecialesValoracion.ancho = 50;
            this.ucAguaSistemasEspecialesValoracion.isEntero = false;
            this.ucAguaSistemasEspecialesValoracion.isNegativo = false;
            this.ucAguaSistemasEspecialesValoracion.isObligatorio = true;
            this.ucAguaSistemasEspecialesValoracion.isSimboloDecimalPunto = true;
            this.ucAguaSistemasEspecialesValoracion.location = new System.Drawing.Point(425, 0);
            this.ucAguaSistemasEspecialesValoracion.Location = new System.Drawing.Point(21, 89);
            this.ucAguaSistemasEspecialesValoracion.lonMax = 32767;
            this.ucAguaSistemasEspecialesValoracion.Name = "ucAguaSistemasEspecialesValoracion";
            this.ucAguaSistemasEspecialesValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucAguaSistemasEspecialesValoracion.TabIndex = 2;
            this.ucAguaSistemasEspecialesValoracion.uiLbl = "ucAguaSistemasEspecialesValoracion";
            this.ucAguaSistemasEspecialesValoracion.uitxt = "";
            this.ucAguaSistemasEspecialesValoracion.valorMaximo = 10D;
            this.ucAguaSistemasEspecialesValoracion.valorMinimo = 0D;
            // 
            // ucAguaSinAfeccionValoracion
            // 
            this.ucAguaSinAfeccionValoracion.ancho = 50;
            this.ucAguaSinAfeccionValoracion.isEntero = false;
            this.ucAguaSinAfeccionValoracion.isNegativo = false;
            this.ucAguaSinAfeccionValoracion.isObligatorio = true;
            this.ucAguaSinAfeccionValoracion.isSimboloDecimalPunto = true;
            this.ucAguaSinAfeccionValoracion.location = new System.Drawing.Point(425, 0);
            this.ucAguaSinAfeccionValoracion.Location = new System.Drawing.Point(21, 23);
            this.ucAguaSinAfeccionValoracion.lonMax = 32767;
            this.ucAguaSinAfeccionValoracion.Name = "ucAguaSinAfeccionValoracion";
            this.ucAguaSinAfeccionValoracion.Size = new System.Drawing.Size(479, 20);
            this.ucAguaSinAfeccionValoracion.TabIndex = 0;
            this.ucAguaSinAfeccionValoracion.uiLbl = "ucAguaSinAfeccionValoracion";
            this.ucAguaSinAfeccionValoracion.uitxt = "";
            this.ucAguaSinAfeccionValoracion.valorMaximo = 10D;
            this.ucAguaSinAfeccionValoracion.valorMinimo = 0D;
            // 
            // ucSaveCancel
            // 
            this.ucSaveCancel.Location = new System.Drawing.Point(1, 457);
            this.ucSaveCancel.Name = "ucSaveCancel";
            this.ucSaveCancel.Size = new System.Drawing.Size(506, 25);
            this.ucSaveCancel.TabIndex = 7;
            // 
            // frmValCimentacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 494);
            this.Controls.Add(this.ucSaveCancel);
            this.Controls.Add(this.grValPresenciaAgua);
            this.Controls.Add(this.grValExcavacionProcedimientos);
            this.Controls.Add(this.grValCimEstructurasPasosInferiores);
            this.Name = "frmValCimentacion";
            this.Text = "oFrmValCimentacion";
            this.Load += new System.EventHandler(this.frmValCimentacion_Load);
            this.grValCimEstructurasPasosInferiores.ResumeLayout(false);
            this.grValExcavacionProcedimientos.ResumeLayout(false);
            this.grValPresenciaAgua.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grValCimEstructurasPasosInferiores;
        private userControl.ucLblTxt ucCimDirectaSaneosValoracion;
        private userControl.ucLblTxt ucCimProfundaValoracion;
        private userControl.ucLblTxt ucCimPozosValoracion;
        private userControl.ucLblTxt ucCimDirectaValoracion;
        private System.Windows.Forms.GroupBox grValExcavacionProcedimientos;
        private userControl.ucLblTxt ucExcMartilloValoracion;
        private userControl.ucLblTxt ucExcPilotadorasValoracion;
        private userControl.ucLblTxt ucExcVoladurasValoracion;
        private userControl.ucLblTxt ucExcMediosConvencionalesValoracion;
        private System.Windows.Forms.GroupBox grValPresenciaAgua;
        private userControl.ucLblTxt ucAguaAgotamientoValoracion;
        private userControl.ucLblTxt ucAguaSistemasEspecialesValoracion;
        private userControl.ucLblTxt ucAguaSinAfeccionValoracion;
        private userControl.ucToolDetail ucSaveCancel;
    }
}