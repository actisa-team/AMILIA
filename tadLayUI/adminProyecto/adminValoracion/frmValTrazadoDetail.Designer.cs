namespace tadLayUI.adminValoracion
{
    partial class frmValTrazadoDetail
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
            this.grValTrazado = new System.Windows.Forms.GroupBox();
            this.ucCompensacionTierras = new tadLayUI.userControl.ucLblTxt();
            this.ucVolumenMovimientoTierras = new tadLayUI.userControl.ucLblTxt();
            this.ucTiempoRecorrido = new tadLayUI.userControl.ucLblTxt();
            this.ucTrazadoAlzado = new tadLayUI.userControl.ucLblTxt();
            this.ucTrazadoPlanta = new tadLayUI.userControl.ucLblTxt();
            this.ucSaveCancel = new tadLayUI.userControl.ucToolDetail();
            this.grValTrazado.SuspendLayout();
            this.SuspendLayout();
            // 
            // grValTrazado
            // 
            this.grValTrazado.Controls.Add(this.ucCompensacionTierras);
            this.grValTrazado.Controls.Add(this.ucVolumenMovimientoTierras);
            this.grValTrazado.Controls.Add(this.ucTiempoRecorrido);
            this.grValTrazado.Controls.Add(this.ucTrazadoAlzado);
            this.grValTrazado.Controls.Add(this.ucTrazadoPlanta);
            this.grValTrazado.Location = new System.Drawing.Point(12, 12);
            this.grValTrazado.Name = "grValTrazado";
            this.grValTrazado.Size = new System.Drawing.Size(260, 260);
            this.grValTrazado.TabIndex = 0;
            this.grValTrazado.TabStop = false;
            this.grValTrazado.Text = "grValTrazado";
            // 
            // ucCompensacionTierras
            // 
            this.ucCompensacionTierras.ancho = 50;
            this.ucCompensacionTierras.isEntero = false;
            this.ucCompensacionTierras.isNegativo = false;
            this.ucCompensacionTierras.isObligatorio = true;
            this.ucCompensacionTierras.isSimboloDecimalPunto = true;
            this.ucCompensacionTierras.location = new System.Drawing.Point(180, 0);
            this.ucCompensacionTierras.Location = new System.Drawing.Point(21, 221);
            this.ucCompensacionTierras.lonMax = 32767;
            this.ucCompensacionTierras.Name = "ucCompensacionTierras";
            this.ucCompensacionTierras.Size = new System.Drawing.Size(233, 20);
            this.ucCompensacionTierras.TabIndex = 4;
            this.ucCompensacionTierras.uiLbl = "ucCompensacionTierras";
            this.ucCompensacionTierras.uitxt = "";
            this.ucCompensacionTierras.valorMaximo = 100D;
            this.ucCompensacionTierras.valorMinimo = 0D;
            // 
            // ucVolumenMovimientoTierras
            // 
            this.ucVolumenMovimientoTierras.ancho = 50;
            this.ucVolumenMovimientoTierras.isEntero = false;
            this.ucVolumenMovimientoTierras.isNegativo = false;
            this.ucVolumenMovimientoTierras.isObligatorio = true;
            this.ucVolumenMovimientoTierras.isSimboloDecimalPunto = true;
            this.ucVolumenMovimientoTierras.location = new System.Drawing.Point(180, 0);
            this.ucVolumenMovimientoTierras.Location = new System.Drawing.Point(21, 172);
            this.ucVolumenMovimientoTierras.lonMax = 32767;
            this.ucVolumenMovimientoTierras.Name = "ucVolumenMovimientoTierras";
            this.ucVolumenMovimientoTierras.Size = new System.Drawing.Size(233, 20);
            this.ucVolumenMovimientoTierras.TabIndex = 3;
            this.ucVolumenMovimientoTierras.uiLbl = "ucVolumenMovimientoTierras";
            this.ucVolumenMovimientoTierras.uitxt = "";
            this.ucVolumenMovimientoTierras.valorMaximo = 100D;
            this.ucVolumenMovimientoTierras.valorMinimo = 0D;
            // 
            // ucTiempoRecorrido
            // 
            this.ucTiempoRecorrido.ancho = 50;
            this.ucTiempoRecorrido.isEntero = false;
            this.ucTiempoRecorrido.isNegativo = false;
            this.ucTiempoRecorrido.isObligatorio = true;
            this.ucTiempoRecorrido.isSimboloDecimalPunto = true;
            this.ucTiempoRecorrido.location = new System.Drawing.Point(180, 0);
            this.ucTiempoRecorrido.Location = new System.Drawing.Point(21, 123);
            this.ucTiempoRecorrido.lonMax = 32767;
            this.ucTiempoRecorrido.Name = "ucTiempoRecorrido";
            this.ucTiempoRecorrido.Size = new System.Drawing.Size(233, 20);
            this.ucTiempoRecorrido.TabIndex = 2;
            this.ucTiempoRecorrido.uiLbl = "ucTiempoRecorrido";
            this.ucTiempoRecorrido.uitxt = "";
            this.ucTiempoRecorrido.valorMaximo = 100D;
            this.ucTiempoRecorrido.valorMinimo = 0D;
            // 
            // ucTrazadoAlzado
            // 
            this.ucTrazadoAlzado.ancho = 50;
            this.ucTrazadoAlzado.isEntero = false;
            this.ucTrazadoAlzado.isNegativo = false;
            this.ucTrazadoAlzado.isObligatorio = true;
            this.ucTrazadoAlzado.isSimboloDecimalPunto = true;
            this.ucTrazadoAlzado.location = new System.Drawing.Point(180, 0);
            this.ucTrazadoAlzado.Location = new System.Drawing.Point(21, 74);
            this.ucTrazadoAlzado.lonMax = 32767;
            this.ucTrazadoAlzado.Name = "ucTrazadoAlzado";
            this.ucTrazadoAlzado.Size = new System.Drawing.Size(233, 20);
            this.ucTrazadoAlzado.TabIndex = 1;
            this.ucTrazadoAlzado.uiLbl = "ucTrazadoAlzado";
            this.ucTrazadoAlzado.uitxt = "";
            this.ucTrazadoAlzado.valorMaximo = 100D;
            this.ucTrazadoAlzado.valorMinimo = 0D;
            // 
            // ucTrazadoPlanta
            // 
            this.ucTrazadoPlanta.ancho = 50;
            this.ucTrazadoPlanta.isEntero = false;
            this.ucTrazadoPlanta.isNegativo = false;
            this.ucTrazadoPlanta.isObligatorio = true;
            this.ucTrazadoPlanta.isSimboloDecimalPunto = true;
            this.ucTrazadoPlanta.location = new System.Drawing.Point(180, 0);
            this.ucTrazadoPlanta.Location = new System.Drawing.Point(21, 25);
            this.ucTrazadoPlanta.lonMax = 32767;
            this.ucTrazadoPlanta.Name = "ucTrazadoPlanta";
            this.ucTrazadoPlanta.Size = new System.Drawing.Size(233, 20);
            this.ucTrazadoPlanta.TabIndex = 0;
            this.ucTrazadoPlanta.uiLbl = "ucTrazadoPlanta";
            this.ucTrazadoPlanta.uitxt = "";
            this.ucTrazadoPlanta.valorMaximo = 100D;
            this.ucTrazadoPlanta.valorMinimo = 0D;
            // 
            // ucSaveCancel
            // 
            this.ucSaveCancel.Location = new System.Drawing.Point(12, 278);
            this.ucSaveCancel.Name = "ucSaveCancel";
            this.ucSaveCancel.Size = new System.Drawing.Size(260, 25);
            this.ucSaveCancel.TabIndex = 1;
            // 
            // frmValTrazadoDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 358);
            this.Controls.Add(this.ucSaveCancel);
            this.Controls.Add(this.grValTrazado);
            this.Name = "frmValTrazadoDetail";
            this.Text = "frmValTrazado";
            this.Load += new System.EventHandler(this.frmValTrazadoDetail_Load);
            this.grValTrazado.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grValTrazado;
        private userControl.ucToolDetail ucSaveCancel;
        private userControl.ucLblTxt ucTrazadoPlanta;
        private userControl.ucLblTxt ucTrazadoAlzado;
        private userControl.ucLblTxt ucCompensacionTierras;
        private userControl.ucLblTxt ucVolumenMovimientoTierras;
        private userControl.ucLblTxt ucTiempoRecorrido;
    }
}