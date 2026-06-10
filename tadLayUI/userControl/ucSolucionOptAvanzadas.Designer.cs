namespace tadLayUI.userControl
{
    partial class ucSolucionOptAvanzadas
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
            this.grAbanico = new System.Windows.Forms.GroupBox();
            this.ucAbanicoTramoDiscretizacionMetros = new tadLayUI.userControl.ucLblTxt();
            this.ucAbanicoDiscretizacionGrados = new tadLayUI.userControl.ucLblTxt();
            this.ucAbanicoAnguloTotalGrados = new tadLayUI.userControl.ucLblTxt();
            this.ucAbanicoToleranciaPuntoObjetivo = new tadLayUI.userControl.ucLblTxt();
            this.chkAbanicoInvalidarTramosIncrementoLongitud = new System.Windows.Forms.CheckBox();
            this.ucInvalidarTramosLongitudPC = new tadLayUI.userControl.ucLblTxt();
            this.grRoadDesign = new System.Windows.Forms.GroupBox();
            this.chkRoadConsiderarAijConstante = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grAbanico.SuspendLayout();
            this.grRoadDesign.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grAbanico
            // 
            this.grAbanico.Controls.Add(this.ucAbanicoTramoDiscretizacionMetros);
            this.grAbanico.Controls.Add(this.ucAbanicoDiscretizacionGrados);
            this.grAbanico.Controls.Add(this.ucAbanicoAnguloTotalGrados);
            this.grAbanico.Location = new System.Drawing.Point(11, 11);
            this.grAbanico.Name = "grAbanico";
            this.grAbanico.Size = new System.Drawing.Size(451, 140);
            this.grAbanico.TabIndex = 0;
            this.grAbanico.TabStop = false;
            this.grAbanico.Text = "grAbanico";
            // 
            // ucAbanicoTramoDiscretizacionMetros
            // 
            this.ucAbanicoTramoDiscretizacionMetros.ancho = 75;
            this.ucAbanicoTramoDiscretizacionMetros.isEntero = true;
            this.ucAbanicoTramoDiscretizacionMetros.isNegativo = false;
            this.ucAbanicoTramoDiscretizacionMetros.isObligatorio = true;
            this.ucAbanicoTramoDiscretizacionMetros.isSimboloDecimalPunto = true;
            this.ucAbanicoTramoDiscretizacionMetros.location = new System.Drawing.Point(275, 0);
            this.ucAbanicoTramoDiscretizacionMetros.Location = new System.Drawing.Point(23, 81);
            this.ucAbanicoTramoDiscretizacionMetros.lonMax = 32767;
            this.ucAbanicoTramoDiscretizacionMetros.Name = "ucAbanicoTramoDiscretizacionMetros";
            this.ucAbanicoTramoDiscretizacionMetros.Size = new System.Drawing.Size(376, 20);
            this.ucAbanicoTramoDiscretizacionMetros.TabIndex = 3;
            this.ucAbanicoTramoDiscretizacionMetros.uiLbl = "ucAbanicoTramoDiscretizacionMetros";
            this.ucAbanicoTramoDiscretizacionMetros.uitxt = "";
            this.ucAbanicoTramoDiscretizacionMetros.valorDoubleNull = null;
            this.ucAbanicoTramoDiscretizacionMetros.valorMaximo = 180D;
            this.ucAbanicoTramoDiscretizacionMetros.valorMinimo = 0D;
            // 
            // ucAbanicoDiscretizacionGrados
            // 
            this.ucAbanicoDiscretizacionGrados.ancho = 75;
            this.ucAbanicoDiscretizacionGrados.isEntero = true;
            this.ucAbanicoDiscretizacionGrados.isNegativo = false;
            this.ucAbanicoDiscretizacionGrados.isObligatorio = true;
            this.ucAbanicoDiscretizacionGrados.isSimboloDecimalPunto = true;
            this.ucAbanicoDiscretizacionGrados.location = new System.Drawing.Point(275, 0);
            this.ucAbanicoDiscretizacionGrados.Location = new System.Drawing.Point(23, 49);
            this.ucAbanicoDiscretizacionGrados.lonMax = 32767;
            this.ucAbanicoDiscretizacionGrados.Name = "ucAbanicoDiscretizacionGrados";
            this.ucAbanicoDiscretizacionGrados.Size = new System.Drawing.Size(376, 20);
            this.ucAbanicoDiscretizacionGrados.TabIndex = 2;
            this.ucAbanicoDiscretizacionGrados.uiLbl = "ucAbanicoDiscretizacionGrados";
            this.ucAbanicoDiscretizacionGrados.uitxt = "";
            this.ucAbanicoDiscretizacionGrados.valorDoubleNull = null;
            this.ucAbanicoDiscretizacionGrados.valorMaximo = 180D;
            this.ucAbanicoDiscretizacionGrados.valorMinimo = 0D;
            // 
            // ucAbanicoAnguloTotalGrados
            // 
            this.ucAbanicoAnguloTotalGrados.ancho = 75;
            this.ucAbanicoAnguloTotalGrados.isEntero = true;
            this.ucAbanicoAnguloTotalGrados.isNegativo = false;
            this.ucAbanicoAnguloTotalGrados.isObligatorio = true;
            this.ucAbanicoAnguloTotalGrados.isSimboloDecimalPunto = true;
            this.ucAbanicoAnguloTotalGrados.location = new System.Drawing.Point(275, 0);
            this.ucAbanicoAnguloTotalGrados.Location = new System.Drawing.Point(23, 17);
            this.ucAbanicoAnguloTotalGrados.lonMax = 32767;
            this.ucAbanicoAnguloTotalGrados.Name = "ucAbanicoAnguloTotalGrados";
            this.ucAbanicoAnguloTotalGrados.Size = new System.Drawing.Size(376, 20);
            this.ucAbanicoAnguloTotalGrados.TabIndex = 1;
            this.ucAbanicoAnguloTotalGrados.uiLbl = "ucAbanicoAnguloTotalGrados";
            this.ucAbanicoAnguloTotalGrados.uitxt = "";
            this.ucAbanicoAnguloTotalGrados.valorDoubleNull = null;
            this.ucAbanicoAnguloTotalGrados.valorMaximo = 180D;
            this.ucAbanicoAnguloTotalGrados.valorMinimo = 0D;
            // 
            // ucAbanicoToleranciaPuntoObjetivo
            // 
            this.ucAbanicoToleranciaPuntoObjetivo.ancho = 75;
            this.ucAbanicoToleranciaPuntoObjetivo.isEntero = false;
            this.ucAbanicoToleranciaPuntoObjetivo.isNegativo = false;
            this.ucAbanicoToleranciaPuntoObjetivo.isObligatorio = true;
            this.ucAbanicoToleranciaPuntoObjetivo.isSimboloDecimalPunto = true;
            this.ucAbanicoToleranciaPuntoObjetivo.location = new System.Drawing.Point(275, 0);
            this.ucAbanicoToleranciaPuntoObjetivo.Location = new System.Drawing.Point(34, 122);
            this.ucAbanicoToleranciaPuntoObjetivo.lonMax = 32767;
            this.ucAbanicoToleranciaPuntoObjetivo.Name = "ucAbanicoToleranciaPuntoObjetivo";
            this.ucAbanicoToleranciaPuntoObjetivo.Size = new System.Drawing.Size(376, 20);
            this.ucAbanicoToleranciaPuntoObjetivo.TabIndex = 6;
            this.ucAbanicoToleranciaPuntoObjetivo.uiLbl = "ucAbanicoToleranciaPuntoObjetivo";
            this.ucAbanicoToleranciaPuntoObjetivo.uitxt = "";
            this.ucAbanicoToleranciaPuntoObjetivo.valorDoubleNull = null;
            this.ucAbanicoToleranciaPuntoObjetivo.valorMaximo = 180D;
            this.ucAbanicoToleranciaPuntoObjetivo.valorMinimo = 0D;
            // 
            // chkAbanicoInvalidarTramosIncrementoLongitud
            // 
            this.chkAbanicoInvalidarTramosIncrementoLongitud.AutoSize = true;
            this.chkAbanicoInvalidarTramosIncrementoLongitud.Location = new System.Drawing.Point(23, 31);
            this.chkAbanicoInvalidarTramosIncrementoLongitud.Name = "chkAbanicoInvalidarTramosIncrementoLongitud";
            this.chkAbanicoInvalidarTramosIncrementoLongitud.Size = new System.Drawing.Size(252, 17);
            this.chkAbanicoInvalidarTramosIncrementoLongitud.TabIndex = 4;
            this.chkAbanicoInvalidarTramosIncrementoLongitud.Text = "chkAbanicoInvalidarTramosIncrementoLongitud";
            this.chkAbanicoInvalidarTramosIncrementoLongitud.UseVisualStyleBackColor = true;
            this.chkAbanicoInvalidarTramosIncrementoLongitud.CheckedChanged += new System.EventHandler(this.chkAbanicoInvalidarTramosIncrementoLongitud_CheckedChanged);
            // 
            // ucInvalidarTramosLongitudPC
            // 
            this.ucInvalidarTramosLongitudPC.ancho = 75;
            this.ucInvalidarTramosLongitudPC.isEntero = false;
            this.ucInvalidarTramosLongitudPC.isNegativo = false;
            this.ucInvalidarTramosLongitudPC.isObligatorio = true;
            this.ucInvalidarTramosLongitudPC.isSimboloDecimalPunto = true;
            this.ucInvalidarTramosLongitudPC.location = new System.Drawing.Point(130, 0);
            this.ucInvalidarTramosLongitudPC.Location = new System.Drawing.Point(169, 29);
            this.ucInvalidarTramosLongitudPC.lonMax = 32767;
            this.ucInvalidarTramosLongitudPC.Name = "ucInvalidarTramosLongitudPC";
            this.ucInvalidarTramosLongitudPC.Size = new System.Drawing.Size(224, 20);
            this.ucInvalidarTramosLongitudPC.TabIndex = 5;
            this.ucInvalidarTramosLongitudPC.uiLbl = "";
            this.ucInvalidarTramosLongitudPC.uitxt = "";
            this.ucInvalidarTramosLongitudPC.valorDoubleNull = null;
            this.ucInvalidarTramosLongitudPC.valorMaximo = -1D;
            this.ucInvalidarTramosLongitudPC.valorMinimo = 0D;
            // 
            // grRoadDesign
            // 
            this.grRoadDesign.Controls.Add(this.chkRoadConsiderarAijConstante);
            this.grRoadDesign.Location = new System.Drawing.Point(11, 230);
            this.grRoadDesign.Name = "grRoadDesign";
            this.grRoadDesign.Size = new System.Drawing.Size(451, 42);
            this.grRoadDesign.TabIndex = 1;
            this.grRoadDesign.TabStop = false;
            // 
            // chkRoadConsiderarAijConstante
            // 
            this.chkRoadConsiderarAijConstante.AutoSize = true;
            this.chkRoadConsiderarAijConstante.Location = new System.Drawing.Point(30, 15);
            this.chkRoadConsiderarAijConstante.Name = "chkRoadConsiderarAijConstante";
            this.chkRoadConsiderarAijConstante.Size = new System.Drawing.Size(179, 17);
            this.chkRoadConsiderarAijConstante.TabIndex = 5;
            this.chkRoadConsiderarAijConstante.Text = "chkRoadConsiderarAijConstante";
            this.chkRoadConsiderarAijConstante.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAbanicoInvalidarTramosIncrementoLongitud);
            this.groupBox1.Controls.Add(this.ucInvalidarTramosLongitudPC);
            this.groupBox1.Location = new System.Drawing.Point(11, 157);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(451, 63);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // ucSolucionOptAvanzadas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ucAbanicoToleranciaPuntoObjetivo);
            this.Controls.Add(this.grRoadDesign);
            this.Controls.Add(this.grAbanico);
            this.Name = "ucSolucionOptAvanzadas";
            this.Size = new System.Drawing.Size(478, 296);
            this.grAbanico.ResumeLayout(false);
            this.grRoadDesign.ResumeLayout(false);
            this.grRoadDesign.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grAbanico;
        private ucLblTxt ucAbanicoTramoDiscretizacionMetros;
        private ucLblTxt ucAbanicoDiscretizacionGrados;
        private ucLblTxt ucAbanicoAnguloTotalGrados;
        private ucLblTxt ucAbanicoToleranciaPuntoObjetivo;
        private System.Windows.Forms.CheckBox chkAbanicoInvalidarTramosIncrementoLongitud;
        private ucLblTxt ucInvalidarTramosLongitudPC;
        private System.Windows.Forms.GroupBox grRoadDesign;
        private System.Windows.Forms.CheckBox chkRoadConsiderarAijConstante;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}
