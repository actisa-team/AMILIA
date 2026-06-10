namespace tadLayUI.userControl
{
    partial class ucRoadDatos
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ucPeraltePC = new tadLayUI.userControl.ucLblTxt();
            this.chkPermitirReduccionesVelocidad = new System.Windows.Forms.CheckBox();
            this.btnRoadSelect = new System.Windows.Forms.Button();
            this.ucRoadGrupo = new tadLayUI.userControl.ucLblTxt();
            this.ucKvConcavo = new tadLayUI.userControl.ucLblTxt();
            this.ucVp = new tadLayUI.userControl.ucLblTxt();
            this.ucKvConvexo = new tadLayUI.userControl.ucLblTxt();
            this.ucRadio = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadPrerenciasKv1 = new tadLayUI.userControl.ucRoadPrerenciasKv();
            this.ucRoadPreferenciasRectasCurvas1 = new tadLayUI.userControl.ucRoadPreferenciasRectasCurvas();
            this.ucAvanceMaximo = new tadLayUI.userControl.ucLblTxt();
            this.ucValorMinimoSalidaLlegada = new tadLayUI.userControl.ucLblTxt();
            this.ucAijMaximoTramo = new tadLayUI.userControl.ucLblTxt();
            this.ucAijMinimoTramo = new tadLayUI.userControl.ucLblTxt();
            this.ucRadioCondicionadoLmin = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucRadioCondicionadoLmin);
            this.groupBox1.Controls.Add(this.ucPeraltePC);
            this.groupBox1.Controls.Add(this.chkPermitirReduccionesVelocidad);
            this.groupBox1.Controls.Add(this.btnRoadSelect);
            this.groupBox1.Controls.Add(this.ucRoadGrupo);
            this.groupBox1.Controls.Add(this.ucKvConcavo);
            this.groupBox1.Controls.Add(this.ucVp);
            this.groupBox1.Controls.Add(this.ucKvConvexo);
            this.groupBox1.Controls.Add(this.ucRadio);
            this.groupBox1.Controls.Add(this.ucRoadPrerenciasKv1);
            this.groupBox1.Controls.Add(this.ucRoadPreferenciasRectasCurvas1);
            this.groupBox1.Controls.Add(this.ucAvanceMaximo);
            this.groupBox1.Controls.Add(this.ucValorMinimoSalidaLlegada);
            this.groupBox1.Controls.Add(this.ucAijMaximoTramo);
            this.groupBox1.Controls.Add(this.ucAijMinimoTramo);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 377);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // ucPeraltePC
            // 
            this.ucPeraltePC.ancho = 75;
            this.ucPeraltePC.isEntero = false;
            this.ucPeraltePC.isNegativo = false;
            this.ucPeraltePC.isObligatorio = false;
            this.ucPeraltePC.isSimboloDecimalPunto = true;
            this.ucPeraltePC.isTexto = true;
            this.ucPeraltePC.location = new System.Drawing.Point(200, 0);
            this.ucPeraltePC.Location = new System.Drawing.Point(20, 91);
            this.ucPeraltePC.lonMax = 32767;
            this.ucPeraltePC.Name = "ucPeraltePC";
            this.ucPeraltePC.Size = new System.Drawing.Size(309, 20);
            this.ucPeraltePC.TabIndex = 12;
            this.ucPeraltePC.uiLbl = "ucPeralte";
            this.ucPeraltePC.uitxt = "";
            this.ucPeraltePC.valorDoubleNull = null;
            this.ucPeraltePC.valorMaximo = -1D;
            this.ucPeraltePC.valorMinimo = 0D;
            // 
            // chkPermitirReduccionesVelocidad
            // 
            this.chkPermitirReduccionesVelocidad.AutoSize = true;
            this.chkPermitirReduccionesVelocidad.Location = new System.Drawing.Point(25, 255);
            this.chkPermitirReduccionesVelocidad.Name = "chkPermitirReduccionesVelocidad";
            this.chkPermitirReduccionesVelocidad.Size = new System.Drawing.Size(188, 17);
            this.chkPermitirReduccionesVelocidad.TabIndex = 8;
            this.chkPermitirReduccionesVelocidad.Text = "chkPermitirReduccionesVelocidad";
            this.chkPermitirReduccionesVelocidad.UseVisualStyleBackColor = true;
            // 
            // btnRoadSelect
            // 
            this.btnRoadSelect.Location = new System.Drawing.Point(304, 12);
            this.btnRoadSelect.Name = "btnRoadSelect";
            this.btnRoadSelect.Size = new System.Drawing.Size(162, 23);
            this.btnRoadSelect.TabIndex = 11;
            this.btnRoadSelect.Text = "btnRoadSelect";
            this.btnRoadSelect.UseVisualStyleBackColor = true;
            this.btnRoadSelect.Click += new System.EventHandler(this.btnRoadSelect_Click);
            // 
            // ucRoadGrupo
            // 
            this.ucRoadGrupo.ancho = 75;
            this.ucRoadGrupo.isEntero = false;
            this.ucRoadGrupo.isNegativo = false;
            this.ucRoadGrupo.isObligatorio = true;
            this.ucRoadGrupo.isSimboloDecimalPunto = true;
            this.ucRoadGrupo.isTexto = true;
            this.ucRoadGrupo.location = new System.Drawing.Point(200, 0);
            this.ucRoadGrupo.Location = new System.Drawing.Point(20, 14);
            this.ucRoadGrupo.lonMax = 32767;
            this.ucRoadGrupo.Name = "ucRoadGrupo";
            this.ucRoadGrupo.Size = new System.Drawing.Size(309, 20);
            this.ucRoadGrupo.TabIndex = 0;
            this.ucRoadGrupo.uiLbl = "ucRoadGrupo";
            this.ucRoadGrupo.uitxt = "";
            this.ucRoadGrupo.valorDoubleNull = null;
            this.ucRoadGrupo.valorMaximo = -1D;
            this.ucRoadGrupo.valorMinimo = -1D;
            // 
            // ucKvConcavo
            // 
            this.ucKvConcavo.ancho = 75;
            this.ucKvConcavo.isEntero = false;
            this.ucKvConcavo.isNegativo = false;
            this.ucKvConcavo.isObligatorio = false;
            this.ucKvConcavo.isSimboloDecimalPunto = true;
            this.ucKvConcavo.isTexto = true;
            this.ucKvConcavo.location = new System.Drawing.Point(200, 0);
            this.ucKvConcavo.Location = new System.Drawing.Point(20, 324);
            this.ucKvConcavo.lonMax = 32767;
            this.ucKvConcavo.Name = "ucKvConcavo";
            this.ucKvConcavo.Size = new System.Drawing.Size(309, 20);
            this.ucKvConcavo.TabIndex = 11;
            this.ucKvConcavo.uiLbl = "ucKvConcavo";
            this.ucKvConcavo.uitxt = "";
            this.ucKvConcavo.valorDoubleNull = null;
            this.ucKvConcavo.valorMaximo = 0D;
            this.ucKvConcavo.valorMinimo = 0D;
            // 
            // ucVp
            // 
            this.ucVp.ancho = 75;
            this.ucVp.isEntero = false;
            this.ucVp.isNegativo = false;
            this.ucVp.isObligatorio = false;
            this.ucVp.isSimboloDecimalPunto = true;
            this.ucVp.isTexto = true;
            this.ucVp.location = new System.Drawing.Point(200, 0);
            this.ucVp.Location = new System.Drawing.Point(20, 39);
            this.ucVp.lonMax = 32767;
            this.ucVp.Name = "ucVp";
            this.ucVp.Size = new System.Drawing.Size(309, 20);
            this.ucVp.TabIndex = 1;
            this.ucVp.uiLbl = "ucVp";
            this.ucVp.uitxt = "";
            this.ucVp.valorDoubleNull = null;
            this.ucVp.valorMaximo = -1D;
            this.ucVp.valorMinimo = 0D;
            // 
            // ucKvConvexo
            // 
            this.ucKvConvexo.ancho = 75;
            this.ucKvConvexo.isEntero = false;
            this.ucKvConvexo.isNegativo = false;
            this.ucKvConvexo.isObligatorio = false;
            this.ucKvConvexo.isSimboloDecimalPunto = true;
            this.ucKvConvexo.isTexto = true;
            this.ucKvConvexo.location = new System.Drawing.Point(200, 0);
            this.ucKvConvexo.Location = new System.Drawing.Point(20, 301);
            this.ucKvConvexo.lonMax = 32767;
            this.ucKvConvexo.Name = "ucKvConvexo";
            this.ucKvConvexo.Size = new System.Drawing.Size(309, 20);
            this.ucKvConvexo.TabIndex = 10;
            this.ucKvConvexo.uiLbl = "ucKvConvexo";
            this.ucKvConvexo.uitxt = "";
            this.ucKvConvexo.valorDoubleNull = null;
            this.ucKvConvexo.valorMaximo = 0D;
            this.ucKvConvexo.valorMinimo = 0D;
            // 
            // ucRadio
            // 
            this.ucRadio.ancho = 75;
            this.ucRadio.isEntero = false;
            this.ucRadio.isNegativo = false;
            this.ucRadio.isObligatorio = false;
            this.ucRadio.isSimboloDecimalPunto = true;
            this.ucRadio.isTexto = true;
            this.ucRadio.location = new System.Drawing.Point(200, 0);
            this.ucRadio.Location = new System.Drawing.Point(20, 65);
            this.ucRadio.lonMax = 32767;
            this.ucRadio.Name = "ucRadio";
            this.ucRadio.Size = new System.Drawing.Size(309, 20);
            this.ucRadio.TabIndex = 2;
            this.ucRadio.uiLbl = "ucRadio";
            this.ucRadio.uitxt = "";
            this.ucRadio.valorDoubleNull = null;
            this.ucRadio.valorMaximo = -1D;
            this.ucRadio.valorMinimo = 0D;
            // 
            // ucRoadPrerenciasKv1
            // 
            this.ucRoadPrerenciasKv1.ancho = 175;
            this.ucRoadPrerenciasKv1.isObligatorio = true;
            this.ucRoadPrerenciasKv1.location = new System.Drawing.Point(198, 0);
            this.ucRoadPrerenciasKv1.Location = new System.Drawing.Point(23, 275);
            this.ucRoadPrerenciasKv1.Name = "ucRoadPrerenciasKv1";
            this.ucRoadPrerenciasKv1.Size = new System.Drawing.Size(389, 21);
            this.ucRoadPrerenciasKv1.TabIndex = 9;
            this.ucRoadPrerenciasKv1.uiLbl = "Preferencias Kv";
            // 
            // ucRoadPreferenciasRectasCurvas1
            // 
            this.ucRoadPreferenciasRectasCurvas1.ancho = 175;
            this.ucRoadPreferenciasRectasCurvas1.isObligatorio = true;
            this.ucRoadPreferenciasRectasCurvas1.location = new System.Drawing.Point(198, 0);
            this.ucRoadPreferenciasRectasCurvas1.Location = new System.Drawing.Point(23, 116);
            this.ucRoadPreferenciasRectasCurvas1.Name = "ucRoadPreferenciasRectasCurvas1";
            this.ucRoadPreferenciasRectasCurvas1.Size = new System.Drawing.Size(389, 21);
            this.ucRoadPreferenciasRectasCurvas1.TabIndex = 3;
            this.ucRoadPreferenciasRectasCurvas1.uiLbl = "Preferencias [Rectas\\Curvas]";
            // 
            // ucAvanceMaximo
            // 
            this.ucAvanceMaximo.ancho = 75;
            this.ucAvanceMaximo.isEntero = false;
            this.ucAvanceMaximo.isNegativo = false;
            this.ucAvanceMaximo.isObligatorio = false;
            this.ucAvanceMaximo.isSimboloDecimalPunto = true;
            this.ucAvanceMaximo.isTexto = true;
            this.ucAvanceMaximo.location = new System.Drawing.Point(200, 0);
            this.ucAvanceMaximo.Location = new System.Drawing.Point(20, 225);
            this.ucAvanceMaximo.lonMax = 32767;
            this.ucAvanceMaximo.Name = "ucAvanceMaximo";
            this.ucAvanceMaximo.Size = new System.Drawing.Size(309, 20);
            this.ucAvanceMaximo.TabIndex = 7;
            this.ucAvanceMaximo.uiLbl = "ucAvanceMaximo";
            this.ucAvanceMaximo.uitxt = "";
            this.ucAvanceMaximo.valorDoubleNull = null;
            this.ucAvanceMaximo.valorMaximo = 0D;
            this.ucAvanceMaximo.valorMinimo = 0D;
            // 
            // ucValorMinimoSalidaLlegada
            // 
            this.ucValorMinimoSalidaLlegada.ancho = 75;
            this.ucValorMinimoSalidaLlegada.isEntero = false;
            this.ucValorMinimoSalidaLlegada.isNegativo = false;
            this.ucValorMinimoSalidaLlegada.isObligatorio = false;
            this.ucValorMinimoSalidaLlegada.isSimboloDecimalPunto = true;
            this.ucValorMinimoSalidaLlegada.isTexto = true;
            this.ucValorMinimoSalidaLlegada.location = new System.Drawing.Point(200, 0);
            this.ucValorMinimoSalidaLlegada.Location = new System.Drawing.Point(20, 144);
            this.ucValorMinimoSalidaLlegada.lonMax = 32767;
            this.ucValorMinimoSalidaLlegada.Name = "ucValorMinimoSalidaLlegada";
            this.ucValorMinimoSalidaLlegada.Size = new System.Drawing.Size(309, 20);
            this.ucValorMinimoSalidaLlegada.TabIndex = 4;
            this.ucValorMinimoSalidaLlegada.uiLbl = "ucValorMinimoSalidaLlegada";
            this.ucValorMinimoSalidaLlegada.uitxt = "";
            this.ucValorMinimoSalidaLlegada.valorDoubleNull = null;
            this.ucValorMinimoSalidaLlegada.valorMaximo = 0D;
            this.ucValorMinimoSalidaLlegada.valorMinimo = 0D;
            // 
            // ucAijMaximoTramo
            // 
            this.ucAijMaximoTramo.ancho = 75;
            this.ucAijMaximoTramo.isEntero = false;
            this.ucAijMaximoTramo.isNegativo = false;
            this.ucAijMaximoTramo.isObligatorio = false;
            this.ucAijMaximoTramo.isSimboloDecimalPunto = true;
            this.ucAijMaximoTramo.isTexto = true;
            this.ucAijMaximoTramo.location = new System.Drawing.Point(200, 0);
            this.ucAijMaximoTramo.Location = new System.Drawing.Point(20, 198);
            this.ucAijMaximoTramo.lonMax = 32767;
            this.ucAijMaximoTramo.Name = "ucAijMaximoTramo";
            this.ucAijMaximoTramo.Size = new System.Drawing.Size(309, 20);
            this.ucAijMaximoTramo.TabIndex = 6;
            this.ucAijMaximoTramo.uiLbl = "ucAijMaximoTramo";
            this.ucAijMaximoTramo.uitxt = "";
            this.ucAijMaximoTramo.valorDoubleNull = null;
            this.ucAijMaximoTramo.valorMaximo = 0D;
            this.ucAijMaximoTramo.valorMinimo = 0D;
            // 
            // ucAijMinimoTramo
            // 
            this.ucAijMinimoTramo.ancho = 75;
            this.ucAijMinimoTramo.isEntero = false;
            this.ucAijMinimoTramo.isNegativo = false;
            this.ucAijMinimoTramo.isObligatorio = false;
            this.ucAijMinimoTramo.isSimboloDecimalPunto = true;
            this.ucAijMinimoTramo.isTexto = true;
            this.ucAijMinimoTramo.location = new System.Drawing.Point(200, 0);
            this.ucAijMinimoTramo.Location = new System.Drawing.Point(20, 171);
            this.ucAijMinimoTramo.lonMax = 32767;
            this.ucAijMinimoTramo.Name = "ucAijMinimoTramo";
            this.ucAijMinimoTramo.Size = new System.Drawing.Size(309, 20);
            this.ucAijMinimoTramo.TabIndex = 5;
            this.ucAijMinimoTramo.uiLbl = "ucAijMinimoTramo";
            this.ucAijMinimoTramo.uitxt = "";
            this.ucAijMinimoTramo.valorDoubleNull = null;
            this.ucAijMinimoTramo.valorMaximo = 0D;
            this.ucAijMinimoTramo.valorMinimo = 0D;
            // 
            // ucRadioCondicionadoLmin
            // 
            this.ucRadioCondicionadoLmin.AutoSize = true;
            this.ucRadioCondicionadoLmin.Location = new System.Drawing.Point(25, 350);
            this.ucRadioCondicionadoLmin.Name = "ucRadioCondicionadoLmin";
            this.ucRadioCondicionadoLmin.Size = new System.Drawing.Size(153, 17);
            this.ucRadioCondicionadoLmin.TabIndex = 13;
            this.ucRadioCondicionadoLmin.Text = "ucRadioCondicionadoLmin";
            this.ucRadioCondicionadoLmin.UseVisualStyleBackColor = true;
            this.ucRadioCondicionadoLmin.CheckedChanged += new System.EventHandler(this.ucRadioCondicionadoLmin_CheckedChanged);
            // 
            // ucRoadDatos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ucRoadDatos";
            this.Size = new System.Drawing.Size(486, 383);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ucLblTxt ucRoadGrupo;
        private ucLblTxt ucVp;
        private ucLblTxt ucRadio;
        private ucRoadPreferenciasRectasCurvas ucRoadPreferenciasRectasCurvas1;
        private ucLblTxt ucValorMinimoSalidaLlegada;
        private ucLblTxt ucAijMinimoTramo;
        private ucLblTxt ucAijMaximoTramo;
        private ucLblTxt ucAvanceMaximo;
        private ucRoadPrerenciasKv ucRoadPrerenciasKv1;
        private ucLblTxt ucKvConvexo;
        private ucLblTxt ucKvConcavo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRoadSelect;
        private System.Windows.Forms.CheckBox chkPermitirReduccionesVelocidad;
        private ucLblTxt ucPeraltePC;
        private System.Windows.Forms.CheckBox ucRadioCondicionadoLmin;
    }
}
