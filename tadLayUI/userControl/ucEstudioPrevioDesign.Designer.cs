namespace tadLayUI.userControl
{
    partial class ucEstudioPrevioDesign
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
            this.grGeometria = new System.Windows.Forms.GroupBox();
            this.ucTerraplenAlturaMaxima = new tadLayUI.userControl.ucLblTxt();
            this.ucDesmonteAlturaMaxima = new tadLayUI.userControl.ucLblTxt();
            this.ucTerraplenTaludPC = new tadLayUI.userControl.ucLblTxt();
            this.ucDesmonteTaludPC = new tadLayUI.userControl.ucLblTxt();
            this.ucAnchoPlataforma = new tadLayUI.userControl.ucLblTxt();
            this.grEstructuras = new System.Windows.Forms.GroupBox();
            this.ucGenerarPuentes = new tadLayUI.userControl.uclblSiNo();
            this.ucGenerarTuneles = new tadLayUI.userControl.uclblSiNo();
            this.ucPuenteAlturaMaxima = new tadLayUI.userControl.ucLblTxt();
            this.grCostesGlobales = new System.Windows.Forms.GroupBox();
            this.ucCosteTunelesUnitario = new tadLayUI.userControl.ucLblTxt();
            this.ucCostePuentesViaductosUnitario = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteTerraplenUnitario = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteDesmonteUnitario = new tadLayUI.userControl.ucLblTxt();
            this.ucCosteImplantacion = new tadLayUI.userControl.ucLblTxt();
            this.grGeometria.SuspendLayout();
            this.grEstructuras.SuspendLayout();
            this.grCostesGlobales.SuspendLayout();
            this.SuspendLayout();
            // 
            // grGeometria
            // 
            this.grGeometria.Controls.Add(this.ucTerraplenAlturaMaxima);
            this.grGeometria.Controls.Add(this.ucDesmonteAlturaMaxima);
            this.grGeometria.Controls.Add(this.ucTerraplenTaludPC);
            this.grGeometria.Controls.Add(this.ucDesmonteTaludPC);
            this.grGeometria.Controls.Add(this.ucAnchoPlataforma);
            this.grGeometria.Location = new System.Drawing.Point(7, 3);
            this.grGeometria.Name = "grGeometria";
            this.grGeometria.Size = new System.Drawing.Size(483, 107);
            this.grGeometria.TabIndex = 0;
            this.grGeometria.TabStop = false;
            this.grGeometria.Text = "grGeometria";
            // 
            // ucTerraplenAlturaMaxima
            // 
            this.ucTerraplenAlturaMaxima.ancho = 50;
            this.ucTerraplenAlturaMaxima.isEntero = false;
            this.ucTerraplenAlturaMaxima.isNegativo = false;
            this.ucTerraplenAlturaMaxima.isObligatorio = true;
            this.ucTerraplenAlturaMaxima.isSimboloDecimalPunto = true;
            this.ucTerraplenAlturaMaxima.location = new System.Drawing.Point(175, 0);
            this.ucTerraplenAlturaMaxima.Location = new System.Drawing.Point(9, 76);
            this.ucTerraplenAlturaMaxima.lonMax = 32767;
            this.ucTerraplenAlturaMaxima.Name = "ucTerraplenAlturaMaxima";
            this.ucTerraplenAlturaMaxima.Size = new System.Drawing.Size(233, 20);
            this.ucTerraplenAlturaMaxima.TabIndex = 4;
            this.ucTerraplenAlturaMaxima.uiLbl = "ucTerraplenAlturaMaxima";
            this.ucTerraplenAlturaMaxima.uitxt = "";
            this.ucTerraplenAlturaMaxima.valorDoubleNull = null;
            this.ucTerraplenAlturaMaxima.valorMaximo = 100D;
            this.ucTerraplenAlturaMaxima.valorMinimo = 0D;
            // 
            // ucDesmonteAlturaMaxima
            // 
            this.ucDesmonteAlturaMaxima.ancho = 50;
            this.ucDesmonteAlturaMaxima.isEntero = false;
            this.ucDesmonteAlturaMaxima.isNegativo = false;
            this.ucDesmonteAlturaMaxima.isObligatorio = true;
            this.ucDesmonteAlturaMaxima.isSimboloDecimalPunto = true;
            this.ucDesmonteAlturaMaxima.location = new System.Drawing.Point(175, 0);
            this.ucDesmonteAlturaMaxima.Location = new System.Drawing.Point(9, 47);
            this.ucDesmonteAlturaMaxima.lonMax = 32767;
            this.ucDesmonteAlturaMaxima.Name = "ucDesmonteAlturaMaxima";
            this.ucDesmonteAlturaMaxima.Size = new System.Drawing.Size(233, 20);
            this.ucDesmonteAlturaMaxima.TabIndex = 2;
            this.ucDesmonteAlturaMaxima.uiLbl = "ucDesmonteAlturaMaxima";
            this.ucDesmonteAlturaMaxima.uitxt = "";
            this.ucDesmonteAlturaMaxima.valorDoubleNull = null;
            this.ucDesmonteAlturaMaxima.valorMaximo = 100D;
            this.ucDesmonteAlturaMaxima.valorMinimo = 0D;
            // 
            // ucTerraplenTaludPC
            // 
            this.ucTerraplenTaludPC.ancho = 50;
            this.ucTerraplenTaludPC.isEntero = false;
            this.ucTerraplenTaludPC.isNegativo = false;
            this.ucTerraplenTaludPC.isObligatorio = true;
            this.ucTerraplenTaludPC.isSimboloDecimalPunto = true;
            this.ucTerraplenTaludPC.location = new System.Drawing.Point(175, 0);
            this.ucTerraplenTaludPC.Location = new System.Drawing.Point(249, 75);
            this.ucTerraplenTaludPC.lonMax = 32767;
            this.ucTerraplenTaludPC.Name = "ucTerraplenTaludPC";
            this.ucTerraplenTaludPC.Size = new System.Drawing.Size(228, 20);
            this.ucTerraplenTaludPC.TabIndex = 2;
            this.ucTerraplenTaludPC.uiLbl = "ucTerraplenTaludPC";
            this.ucTerraplenTaludPC.uitxt = "";
            this.ucTerraplenTaludPC.valorDoubleNull = null;
            this.ucTerraplenTaludPC.valorMaximo = 100D;
            this.ucTerraplenTaludPC.valorMinimo = 0D;
            // 
            // ucDesmonteTaludPC
            // 
            this.ucDesmonteTaludPC.ancho = 50;
            this.ucDesmonteTaludPC.isEntero = false;
            this.ucDesmonteTaludPC.isNegativo = false;
            this.ucDesmonteTaludPC.isObligatorio = true;
            this.ucDesmonteTaludPC.isSimboloDecimalPunto = true;
            this.ucDesmonteTaludPC.location = new System.Drawing.Point(175, 0);
            this.ucDesmonteTaludPC.Location = new System.Drawing.Point(248, 46);
            this.ucDesmonteTaludPC.lonMax = 32767;
            this.ucDesmonteTaludPC.Name = "ucDesmonteTaludPC";
            this.ucDesmonteTaludPC.Size = new System.Drawing.Size(229, 20);
            this.ucDesmonteTaludPC.TabIndex = 3;
            this.ucDesmonteTaludPC.uiLbl = "ucDesmonteTaludPC";
            this.ucDesmonteTaludPC.uitxt = "";
            this.ucDesmonteTaludPC.valorDoubleNull = null;
            this.ucDesmonteTaludPC.valorMaximo = 100D;
            this.ucDesmonteTaludPC.valorMinimo = 0D;
            // 
            // ucAnchoPlataforma
            // 
            this.ucAnchoPlataforma.ancho = 50;
            this.ucAnchoPlataforma.isEntero = false;
            this.ucAnchoPlataforma.isNegativo = false;
            this.ucAnchoPlataforma.isObligatorio = true;
            this.ucAnchoPlataforma.isSimboloDecimalPunto = true;
            this.ucAnchoPlataforma.location = new System.Drawing.Point(175, 0);
            this.ucAnchoPlataforma.Location = new System.Drawing.Point(9, 20);
            this.ucAnchoPlataforma.lonMax = 32767;
            this.ucAnchoPlataforma.Name = "ucAnchoPlataforma";
            this.ucAnchoPlataforma.Size = new System.Drawing.Size(230, 20);
            this.ucAnchoPlataforma.TabIndex = 0;
            this.ucAnchoPlataforma.uiLbl = "ucAnchoPlataforma";
            this.ucAnchoPlataforma.uitxt = "";
            this.ucAnchoPlataforma.valorDoubleNull = null;
            this.ucAnchoPlataforma.valorMaximo = 100D;
            this.ucAnchoPlataforma.valorMinimo = 0D;
            // 
            // grEstructuras
            // 
            this.grEstructuras.Controls.Add(this.ucGenerarPuentes);
            this.grEstructuras.Controls.Add(this.ucGenerarTuneles);
            this.grEstructuras.Controls.Add(this.ucPuenteAlturaMaxima);
            this.grEstructuras.Location = new System.Drawing.Point(10, 115);
            this.grEstructuras.Name = "grEstructuras";
            this.grEstructuras.Size = new System.Drawing.Size(480, 74);
            this.grEstructuras.TabIndex = 6;
            this.grEstructuras.TabStop = false;
            this.grEstructuras.Text = "grEstructuras";
            // 
            // ucGenerarPuentes
            // 
            this.ucGenerarPuentes.ancho = 50;
            this.ucGenerarPuentes.isObligatorio = true;
            this.ucGenerarPuentes.location = new System.Drawing.Point(165, 0);
            this.ucGenerarPuentes.Location = new System.Drawing.Point(17, 19);
            this.ucGenerarPuentes.Name = "ucGenerarPuentes";
            this.ucGenerarPuentes.Size = new System.Drawing.Size(222, 21);
            this.ucGenerarPuentes.TabIndex = 7;
            this.ucGenerarPuentes.uiLbl = "ucGenerarPuentes";
            // 
            // ucGenerarTuneles
            // 
            this.ucGenerarTuneles.ancho = 50;
            this.ucGenerarTuneles.isObligatorio = true;
            this.ucGenerarTuneles.location = new System.Drawing.Point(165, 0);
            this.ucGenerarTuneles.Location = new System.Drawing.Point(18, 45);
            this.ucGenerarTuneles.Name = "ucGenerarTuneles";
            this.ucGenerarTuneles.Size = new System.Drawing.Size(235, 21);
            this.ucGenerarTuneles.TabIndex = 6;
            this.ucGenerarTuneles.uiLbl = "ucGenerarTuneles";
            // 
            // ucPuenteAlturaMaxima
            // 
            this.ucPuenteAlturaMaxima.ancho = 50;
            this.ucPuenteAlturaMaxima.isEntero = false;
            this.ucPuenteAlturaMaxima.isNegativo = false;
            this.ucPuenteAlturaMaxima.isObligatorio = true;
            this.ucPuenteAlturaMaxima.isSimboloDecimalPunto = true;
            this.ucPuenteAlturaMaxima.location = new System.Drawing.Point(175, 0);
            this.ucPuenteAlturaMaxima.Location = new System.Drawing.Point(245, 20);
            this.ucPuenteAlturaMaxima.lonMax = 32767;
            this.ucPuenteAlturaMaxima.Name = "ucPuenteAlturaMaxima";
            this.ucPuenteAlturaMaxima.Size = new System.Drawing.Size(229, 20);
            this.ucPuenteAlturaMaxima.TabIndex = 5;
            this.ucPuenteAlturaMaxima.uiLbl = "ucPuenteAlturaMaxima";
            this.ucPuenteAlturaMaxima.uitxt = "";
            this.ucPuenteAlturaMaxima.valorDoubleNull = null;
            this.ucPuenteAlturaMaxima.valorMaximo = -1D;
            this.ucPuenteAlturaMaxima.valorMinimo = 0D;
            // 
            // grCostesGlobales
            // 
            this.grCostesGlobales.Controls.Add(this.ucCosteTunelesUnitario);
            this.grCostesGlobales.Controls.Add(this.ucCostePuentesViaductosUnitario);
            this.grCostesGlobales.Controls.Add(this.ucCosteTerraplenUnitario);
            this.grCostesGlobales.Controls.Add(this.ucCosteDesmonteUnitario);
            this.grCostesGlobales.Controls.Add(this.ucCosteImplantacion);
            this.grCostesGlobales.Location = new System.Drawing.Point(10, 192);
            this.grCostesGlobales.Name = "grCostesGlobales";
            this.grCostesGlobales.Size = new System.Drawing.Size(480, 151);
            this.grCostesGlobales.TabIndex = 7;
            this.grCostesGlobales.TabStop = false;
            this.grCostesGlobales.Text = "grCostesGlobales";
            // 
            // ucCosteTunelesUnitario
            // 
            this.ucCosteTunelesUnitario.ancho = 50;
            this.ucCosteTunelesUnitario.isEntero = false;
            this.ucCosteTunelesUnitario.isNegativo = false;
            this.ucCosteTunelesUnitario.isObligatorio = true;
            this.ucCosteTunelesUnitario.isSimboloDecimalPunto = true;
            this.ucCosteTunelesUnitario.location = new System.Drawing.Point(405, 0);
            this.ucCosteTunelesUnitario.Location = new System.Drawing.Point(14, 124);
            this.ucCosteTunelesUnitario.lonMax = 32767;
            this.ucCosteTunelesUnitario.Name = "ucCosteTunelesUnitario";
            this.ucCosteTunelesUnitario.Size = new System.Drawing.Size(460, 20);
            this.ucCosteTunelesUnitario.TabIndex = 5;
            this.ucCosteTunelesUnitario.uiLbl = "ucCosteTunelesUnitario";
            this.ucCosteTunelesUnitario.uitxt = "";
            this.ucCosteTunelesUnitario.valorDoubleNull = null;
            this.ucCosteTunelesUnitario.valorMaximo = -1D;
            this.ucCosteTunelesUnitario.valorMinimo = 0D;
            // 
            // ucCostePuentesViaductosUnitario
            // 
            this.ucCostePuentesViaductosUnitario.ancho = 50;
            this.ucCostePuentesViaductosUnitario.isEntero = false;
            this.ucCostePuentesViaductosUnitario.isNegativo = false;
            this.ucCostePuentesViaductosUnitario.isObligatorio = true;
            this.ucCostePuentesViaductosUnitario.isSimboloDecimalPunto = true;
            this.ucCostePuentesViaductosUnitario.location = new System.Drawing.Point(405, 0);
            this.ucCostePuentesViaductosUnitario.Location = new System.Drawing.Point(14, 98);
            this.ucCostePuentesViaductosUnitario.lonMax = 32767;
            this.ucCostePuentesViaductosUnitario.Name = "ucCostePuentesViaductosUnitario";
            this.ucCostePuentesViaductosUnitario.Size = new System.Drawing.Size(460, 20);
            this.ucCostePuentesViaductosUnitario.TabIndex = 4;
            this.ucCostePuentesViaductosUnitario.uiLbl = "ucCostePuentesViaductosUnitario";
            this.ucCostePuentesViaductosUnitario.uitxt = "";
            this.ucCostePuentesViaductosUnitario.valorDoubleNull = null;
            this.ucCostePuentesViaductosUnitario.valorMaximo = -1D;
            this.ucCostePuentesViaductosUnitario.valorMinimo = 0D;
            // 
            // ucCosteTerraplenUnitario
            // 
            this.ucCosteTerraplenUnitario.ancho = 50;
            this.ucCosteTerraplenUnitario.isEntero = false;
            this.ucCosteTerraplenUnitario.isNegativo = false;
            this.ucCosteTerraplenUnitario.isObligatorio = true;
            this.ucCosteTerraplenUnitario.isSimboloDecimalPunto = true;
            this.ucCosteTerraplenUnitario.location = new System.Drawing.Point(405, 0);
            this.ucCosteTerraplenUnitario.Location = new System.Drawing.Point(14, 72);
            this.ucCosteTerraplenUnitario.lonMax = 32767;
            this.ucCosteTerraplenUnitario.Name = "ucCosteTerraplenUnitario";
            this.ucCosteTerraplenUnitario.Size = new System.Drawing.Size(460, 20);
            this.ucCosteTerraplenUnitario.TabIndex = 3;
            this.ucCosteTerraplenUnitario.uiLbl = "ucCosteTerraplenUnitario";
            this.ucCosteTerraplenUnitario.uitxt = "";
            this.ucCosteTerraplenUnitario.valorDoubleNull = null;
            this.ucCosteTerraplenUnitario.valorMaximo = -1D;
            this.ucCosteTerraplenUnitario.valorMinimo = 0D;
            // 
            // ucCosteDesmonteUnitario
            // 
            this.ucCosteDesmonteUnitario.ancho = 50;
            this.ucCosteDesmonteUnitario.isEntero = false;
            this.ucCosteDesmonteUnitario.isNegativo = false;
            this.ucCosteDesmonteUnitario.isObligatorio = true;
            this.ucCosteDesmonteUnitario.isSimboloDecimalPunto = true;
            this.ucCosteDesmonteUnitario.location = new System.Drawing.Point(405, 0);
            this.ucCosteDesmonteUnitario.Location = new System.Drawing.Point(14, 46);
            this.ucCosteDesmonteUnitario.lonMax = 32767;
            this.ucCosteDesmonteUnitario.Name = "ucCosteDesmonteUnitario";
            this.ucCosteDesmonteUnitario.Size = new System.Drawing.Size(460, 20);
            this.ucCosteDesmonteUnitario.TabIndex = 2;
            this.ucCosteDesmonteUnitario.uiLbl = "ucCosteDesmonteUnitario";
            this.ucCosteDesmonteUnitario.uitxt = "";
            this.ucCosteDesmonteUnitario.valorDoubleNull = null;
            this.ucCosteDesmonteUnitario.valorMaximo = -1D;
            this.ucCosteDesmonteUnitario.valorMinimo = 0D;
            // 
            // ucCosteImplantacion
            // 
            this.ucCosteImplantacion.ancho = 50;
            this.ucCosteImplantacion.isEntero = false;
            this.ucCosteImplantacion.isNegativo = false;
            this.ucCosteImplantacion.isObligatorio = true;
            this.ucCosteImplantacion.isSimboloDecimalPunto = true;
            this.ucCosteImplantacion.location = new System.Drawing.Point(405, 0);
            this.ucCosteImplantacion.Location = new System.Drawing.Point(14, 21);
            this.ucCosteImplantacion.lonMax = 32767;
            this.ucCosteImplantacion.Name = "ucCosteImplantacion";
            this.ucCosteImplantacion.Size = new System.Drawing.Size(460, 20);
            this.ucCosteImplantacion.TabIndex = 1;
            this.ucCosteImplantacion.uiLbl = "ucCosteImplantacion";
            this.ucCosteImplantacion.uitxt = "";
            this.ucCosteImplantacion.valorDoubleNull = null;
            this.ucCosteImplantacion.valorMaximo = -1D;
            this.ucCosteImplantacion.valorMinimo = 0D;
            // 
            // ucEstudioPrevioDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grCostesGlobales);
            this.Controls.Add(this.grEstructuras);
            this.Controls.Add(this.grGeometria);
            this.Name = "ucEstudioPrevioDesign";
            this.Size = new System.Drawing.Size(502, 352);
            this.grGeometria.ResumeLayout(false);
            this.grEstructuras.ResumeLayout(false);
            this.grCostesGlobales.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grGeometria;
        private ucLblTxt ucPuenteAlturaMaxima;
        private ucLblTxt ucTerraplenAlturaMaxima;
        private ucLblTxt ucDesmonteAlturaMaxima;
        private ucLblTxt ucTerraplenTaludPC;
        private ucLblTxt ucDesmonteTaludPC;
        private ucLblTxt ucAnchoPlataforma;
        private System.Windows.Forms.GroupBox grEstructuras;
        private uclblSiNo ucGenerarPuentes;
        private uclblSiNo ucGenerarTuneles;
        private System.Windows.Forms.GroupBox grCostesGlobales;
        private ucLblTxt ucCosteTunelesUnitario;
        private ucLblTxt ucCostePuentesViaductosUnitario;
        private ucLblTxt ucCosteTerraplenUnitario;
        private ucLblTxt ucCosteDesmonteUnitario;
        private ucLblTxt ucCosteImplantacion;
    }
}
