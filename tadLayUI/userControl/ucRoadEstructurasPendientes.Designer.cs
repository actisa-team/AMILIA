namespace tadLayUI.userControl
{
    partial class ucRoadEstructurasPendientes
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
            this.grRoadPendientes = new System.Windows.Forms.GroupBox();
            this.grEstructurasPendiente = new System.Windows.Forms.GroupBox();
            this.ucEstructurasPendienteMinimoPC = new tadLayUI.userControl.ucLblTxt();
            this.ucEstructurasPendienteMaximaPC = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadPendienteMinimaPC = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadPendienteMaximaPC = new tadLayUI.userControl.ucLblTxt();
            this.grRoadPendientes.SuspendLayout();
            this.grEstructurasPendiente.SuspendLayout();
            this.SuspendLayout();
            // 
            // grRoadPendientes
            // 
            this.grRoadPendientes.Controls.Add(this.ucRoadPendienteMinimaPC);
            this.grRoadPendientes.Controls.Add(this.ucRoadPendienteMaximaPC);
            this.grRoadPendientes.Location = new System.Drawing.Point(14, 10);
            this.grRoadPendientes.Name = "grRoadPendientes";
            this.grRoadPendientes.Size = new System.Drawing.Size(456, 90);
            this.grRoadPendientes.TabIndex = 0;
            this.grRoadPendientes.TabStop = false;
            this.grRoadPendientes.Text = "grRoadPendientes";
            // 
            // grEstructurasPendiente
            // 
            this.grEstructurasPendiente.Controls.Add(this.ucEstructurasPendienteMinimoPC);
            this.grEstructurasPendiente.Controls.Add(this.ucEstructurasPendienteMaximaPC);
            this.grEstructurasPendiente.Location = new System.Drawing.Point(14, 113);
            this.grEstructurasPendiente.Name = "grEstructurasPendiente";
            this.grEstructurasPendiente.Size = new System.Drawing.Size(456, 86);
            this.grEstructurasPendiente.TabIndex = 2;
            this.grEstructurasPendiente.TabStop = false;
            this.grEstructurasPendiente.Text = "grEstructurasPendiente";
            // 
            // ucEstructurasPendienteMinimoPC
            // 
            this.ucEstructurasPendienteMinimoPC.ancho = 75;
            this.ucEstructurasPendienteMinimoPC.isEntero = false;
            this.ucEstructurasPendienteMinimoPC.isNegativo = false;
            this.ucEstructurasPendienteMinimoPC.isObligatorio = true;
            this.ucEstructurasPendienteMinimoPC.isSimboloDecimalPunto = true;
            this.ucEstructurasPendienteMinimoPC.location = new System.Drawing.Point(250, 0);
            this.ucEstructurasPendienteMinimoPC.Location = new System.Drawing.Point(22, 53);
            this.ucEstructurasPendienteMinimoPC.lonMax = 32767;
            this.ucEstructurasPendienteMinimoPC.Name = "ucEstructurasPendienteMinimoPC";
            this.ucEstructurasPendienteMinimoPC.Size = new System.Drawing.Size(425, 20);
            this.ucEstructurasPendienteMinimoPC.TabIndex = 1;
            this.ucEstructurasPendienteMinimoPC.uiLbl = "ucEstructurasPendienteMinimoPC";
            this.ucEstructurasPendienteMinimoPC.uitxt = "";
            this.ucEstructurasPendienteMinimoPC.valorDoubleNull = null;
            this.ucEstructurasPendienteMinimoPC.valorMaximo = 100D;
            this.ucEstructurasPendienteMinimoPC.valorMinimo = 0D;
            // 
            // ucEstructurasPendienteMaximaPC
            // 
            this.ucEstructurasPendienteMaximaPC.ancho = 75;
            this.ucEstructurasPendienteMaximaPC.isEntero = false;
            this.ucEstructurasPendienteMaximaPC.isNegativo = false;
            this.ucEstructurasPendienteMaximaPC.isObligatorio = true;
            this.ucEstructurasPendienteMaximaPC.isSimboloDecimalPunto = true;
            this.ucEstructurasPendienteMaximaPC.location = new System.Drawing.Point(250, 0);
            this.ucEstructurasPendienteMaximaPC.Location = new System.Drawing.Point(22, 22);
            this.ucEstructurasPendienteMaximaPC.lonMax = 32767;
            this.ucEstructurasPendienteMaximaPC.Name = "ucEstructurasPendienteMaximaPC";
            this.ucEstructurasPendienteMaximaPC.Size = new System.Drawing.Size(425, 20);
            this.ucEstructurasPendienteMaximaPC.TabIndex = 0;
            this.ucEstructurasPendienteMaximaPC.uiLbl = "ucEstructurasPendienteMaximaPC";
            this.ucEstructurasPendienteMaximaPC.uitxt = "";
            this.ucEstructurasPendienteMaximaPC.valorDoubleNull = null;
            this.ucEstructurasPendienteMaximaPC.valorMaximo = 100D;
            this.ucEstructurasPendienteMaximaPC.valorMinimo = 0D;
            // 
            // ucRoadPendienteMinimaPC
            // 
            this.ucRoadPendienteMinimaPC.ancho = 75;
            this.ucRoadPendienteMinimaPC.isEntero = false;
            this.ucRoadPendienteMinimaPC.isNegativo = false;
            this.ucRoadPendienteMinimaPC.isObligatorio = true;
            this.ucRoadPendienteMinimaPC.isSimboloDecimalPunto = true;
            this.ucRoadPendienteMinimaPC.location = new System.Drawing.Point(250, 0);
            this.ucRoadPendienteMinimaPC.Location = new System.Drawing.Point(22, 54);
            this.ucRoadPendienteMinimaPC.lonMax = 32767;
            this.ucRoadPendienteMinimaPC.Name = "ucRoadPendienteMinimaPC";
            this.ucRoadPendienteMinimaPC.Size = new System.Drawing.Size(425, 20);
            this.ucRoadPendienteMinimaPC.TabIndex = 1;
            this.ucRoadPendienteMinimaPC.uiLbl = "ucRoadPendienteMinimaPC";
            this.ucRoadPendienteMinimaPC.uitxt = "";
            this.ucRoadPendienteMinimaPC.valorDoubleNull = null;
            this.ucRoadPendienteMinimaPC.valorMaximo = 100D;
            this.ucRoadPendienteMinimaPC.valorMinimo = 0D;
            // 
            // ucRoadPendienteMaximaPC
            // 
            this.ucRoadPendienteMaximaPC.ancho = 75;
            this.ucRoadPendienteMaximaPC.isEntero = false;
            this.ucRoadPendienteMaximaPC.isNegativo = false;
            this.ucRoadPendienteMaximaPC.isObligatorio = true;
            this.ucRoadPendienteMaximaPC.isSimboloDecimalPunto = true;
            this.ucRoadPendienteMaximaPC.location = new System.Drawing.Point(250, 0);
            this.ucRoadPendienteMaximaPC.Location = new System.Drawing.Point(22, 24);
            this.ucRoadPendienteMaximaPC.lonMax = 32767;
            this.ucRoadPendienteMaximaPC.Name = "ucRoadPendienteMaximaPC";
            this.ucRoadPendienteMaximaPC.Size = new System.Drawing.Size(425, 20);
            this.ucRoadPendienteMaximaPC.TabIndex = 0;
            this.ucRoadPendienteMaximaPC.uiLbl = "ucRoadPendienteMaximaPC";
            this.ucRoadPendienteMaximaPC.uitxt = "";
            this.ucRoadPendienteMaximaPC.valorDoubleNull = null;
            this.ucRoadPendienteMaximaPC.valorMaximo = 100D;
            this.ucRoadPendienteMaximaPC.valorMinimo = 0D;
            // 
            // ucRoadEstructurasPendientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grEstructurasPendiente);
            this.Controls.Add(this.grRoadPendientes);
            this.Name = "ucRoadEstructurasPendientes";
            this.Size = new System.Drawing.Size(500, 214);
            this.grRoadPendientes.ResumeLayout(false);
            this.grEstructurasPendiente.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grRoadPendientes;
        private ucLblTxt ucRoadPendienteMinimaPC;
        private ucLblTxt ucRoadPendienteMaximaPC;
        private System.Windows.Forms.GroupBox grEstructurasPendiente;
        private ucLblTxt ucEstructurasPendienteMinimoPC;
        private ucLblTxt ucEstructurasPendienteMaximaPC;
    }
}
