namespace tadLayUI.adminProyecto
{
    partial class frmDatosAbanico
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
            this.grDatosAbanico = new System.Windows.Forms.GroupBox();
            this.ucAbanicoDiscretizacionGrados = new tadLayUI.userControl.ucLblTxt();
            this.ucAbanicoAnguloTotal = new tadLayUI.userControl.ucLblTxt();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.grDatosAbanico.SuspendLayout();
            this.SuspendLayout();
            // 
            // grDatosAbanico
            // 
            this.grDatosAbanico.Controls.Add(this.ucAbanicoDiscretizacionGrados);
            this.grDatosAbanico.Controls.Add(this.ucAbanicoAnguloTotal);
            this.grDatosAbanico.Location = new System.Drawing.Point(12, 23);
            this.grDatosAbanico.Name = "grDatosAbanico";
            this.grDatosAbanico.Size = new System.Drawing.Size(360, 166);
            this.grDatosAbanico.TabIndex = 0;
            this.grDatosAbanico.TabStop = false;
            this.grDatosAbanico.Text = "grDatosAbanico";
            // 
            // ucAbanicoDiscretizacionGrados
            // 
            this.ucAbanicoDiscretizacionGrados.ancho = 150;
            this.ucAbanicoDiscretizacionGrados.isEntero = true;
            this.ucAbanicoDiscretizacionGrados.isNegativo = false;
            this.ucAbanicoDiscretizacionGrados.isObligatorio = true;
            this.ucAbanicoDiscretizacionGrados.isSimboloDecimalPunto = true;
            this.ucAbanicoDiscretizacionGrados.location = new System.Drawing.Point(175, 0);
            this.ucAbanicoDiscretizacionGrados.Location = new System.Drawing.Point(22, 68);
            this.ucAbanicoDiscretizacionGrados.lonMax = 32767;
            this.ucAbanicoDiscretizacionGrados.Name = "ucAbanicoDiscretizacionGrados";
            this.ucAbanicoDiscretizacionGrados.Size = new System.Drawing.Size(333, 20);
            this.ucAbanicoDiscretizacionGrados.TabIndex = 1;
            this.ucAbanicoDiscretizacionGrados.uiLbl = "ucAbanicoDiscretizacionGrados";
            this.ucAbanicoDiscretizacionGrados.uitxt = "";
            this.ucAbanicoDiscretizacionGrados.valorDoubleNull = null;
            this.ucAbanicoDiscretizacionGrados.valorMaximo = 360D;
            this.ucAbanicoDiscretizacionGrados.valorMinimo = 0D;
            // 
            // ucAbanicoAnguloTotal
            // 
            this.ucAbanicoAnguloTotal.ancho = 150;
            this.ucAbanicoAnguloTotal.isEntero = true;
            this.ucAbanicoAnguloTotal.isNegativo = false;
            this.ucAbanicoAnguloTotal.isObligatorio = true;
            this.ucAbanicoAnguloTotal.isSimboloDecimalPunto = true;
            this.ucAbanicoAnguloTotal.location = new System.Drawing.Point(175, 0);
            this.ucAbanicoAnguloTotal.Location = new System.Drawing.Point(23, 32);
            this.ucAbanicoAnguloTotal.lonMax = 32767;
            this.ucAbanicoAnguloTotal.Name = "ucAbanicoAnguloTotal";
            this.ucAbanicoAnguloTotal.Size = new System.Drawing.Size(331, 20);
            this.ucAbanicoAnguloTotal.TabIndex = 0;
            this.ucAbanicoAnguloTotal.uiLbl = "ucAbanicoAnguloTotalGrados";
            this.ucAbanicoAnguloTotal.uitxt = "";
            this.ucAbanicoAnguloTotal.valorDoubleNull = null;
            this.ucAbanicoAnguloTotal.valorMaximo = 360D;
            this.ucAbanicoAnguloTotal.valorMinimo = 0D;
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(12, 195);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(360, 25);
            this.ucToolDetail1.TabIndex = 1;
            // 
            // frmDatosAbanico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 260);
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.grDatosAbanico);
            this.Name = "frmDatosAbanico";
            this.Text = "frmDatosAbanico";
            this.grDatosAbanico.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grDatosAbanico;
        private userControl.ucLblTxt ucAbanicoDiscretizacionGrados;
        private userControl.ucLblTxt ucAbanicoAnguloTotal;
        private userControl.ucToolDetail ucToolDetail1;
    }
}