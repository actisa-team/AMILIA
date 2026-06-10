namespace tadLayUI.adminProyecto
{
    partial class frmDatosProyecto
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
            this.grDatosGenerales = new System.Windows.Forms.GroupBox();
            this.grTipoProyecto = new System.Windows.Forms.GroupBox();
            this.grSeccionTransversalSeparacion = new System.Windows.Forms.GroupBox();
            this.ucSeccionTransversalSeparacionMetros = new tadLayUI.userControl.ucLblTxt();
            this.ucProyectoTipo1 = new tadLayUI.userControl.ucProyectoTipo();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.grDatosGenerales.SuspendLayout();
            this.grTipoProyecto.SuspendLayout();
            this.grSeccionTransversalSeparacion.SuspendLayout();
            this.SuspendLayout();
            // 
            // grDatosGenerales
            // 
            this.grDatosGenerales.Controls.Add(this.ucDescripcion);
            this.grDatosGenerales.Controls.Add(this.ucNombre);
            this.grDatosGenerales.Location = new System.Drawing.Point(10, 13);
            this.grDatosGenerales.Name = "grDatosGenerales";
            this.grDatosGenerales.Size = new System.Drawing.Size(390, 137);
            this.grDatosGenerales.TabIndex = 0;
            this.grDatosGenerales.TabStop = false;
            this.grDatosGenerales.Text = "grDatosGenerales";
            // 
            // grTipoProyecto
            // 
            this.grTipoProyecto.Controls.Add(this.ucProyectoTipo1);
            this.grTipoProyecto.Location = new System.Drawing.Point(10, 156);
            this.grTipoProyecto.Name = "grTipoProyecto";
            this.grTipoProyecto.Size = new System.Drawing.Size(389, 100);
            this.grTipoProyecto.TabIndex = 5;
            this.grTipoProyecto.TabStop = false;
            this.grTipoProyecto.Text = "grTipoProyecto";
            // 
            // grSeccionTransversalSeparacion
            // 
            this.grSeccionTransversalSeparacion.Controls.Add(this.ucSeccionTransversalSeparacionMetros);
            this.grSeccionTransversalSeparacion.Location = new System.Drawing.Point(10, 273);
            this.grSeccionTransversalSeparacion.Name = "grSeccionTransversalSeparacion";
            this.grSeccionTransversalSeparacion.Size = new System.Drawing.Size(387, 100);
            this.grSeccionTransversalSeparacion.TabIndex = 6;
            this.grSeccionTransversalSeparacion.TabStop = false;
            this.grSeccionTransversalSeparacion.Text = "grSeccionTransversalSeparacion";
            // 
            // ucSeccionTransversalSeparacionMetros
            // 
            this.ucSeccionTransversalSeparacionMetros.ancho = 75;
            this.ucSeccionTransversalSeparacionMetros.isEntero = true;
            this.ucSeccionTransversalSeparacionMetros.isNegativo = false;
            this.ucSeccionTransversalSeparacionMetros.isObligatorio = true;
            this.ucSeccionTransversalSeparacionMetros.isSimboloDecimalPunto = true;
            this.ucSeccionTransversalSeparacionMetros.location = new System.Drawing.Point(270, 0);
            this.ucSeccionTransversalSeparacionMetros.Location = new System.Drawing.Point(26, 43);
            this.ucSeccionTransversalSeparacionMetros.lonMax = 32767;
            this.ucSeccionTransversalSeparacionMetros.Name = "ucSeccionTransversalSeparacionMetros";
            this.ucSeccionTransversalSeparacionMetros.Size = new System.Drawing.Size(355, 20);
            this.ucSeccionTransversalSeparacionMetros.TabIndex = 0;
            this.ucSeccionTransversalSeparacionMetros.uiLbl = "ucSeccionTransversalSeparacionMetros";
            this.ucSeccionTransversalSeparacionMetros.uitxt = "";
            this.ucSeccionTransversalSeparacionMetros.valorDoubleNull = null;
            this.ucSeccionTransversalSeparacionMetros.valorMaximo = -1D;
            this.ucSeccionTransversalSeparacionMetros.valorMinimo = 0D;
            // 
            // ucProyectoTipo1
            // 
            this.ucProyectoTipo1.ancho = 200;
            this.ucProyectoTipo1.Enabled = false;
            this.ucProyectoTipo1.isObligatorio = true;
            this.ucProyectoTipo1.location = new System.Drawing.Point(150, 0);
            this.ucProyectoTipo1.Location = new System.Drawing.Point(19, 41);
            this.ucProyectoTipo1.Name = "ucProyectoTipo1";
            this.ucProyectoTipo1.Size = new System.Drawing.Size(364, 21);
            this.ucProyectoTipo1.TabIndex = 0;
            this.ucProyectoTipo1.uiLbl = "Tipo de Estudio";
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(10, 380);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(390, 25);
            this.ucToolDetail1.TabIndex = 4;
            // 
            // ucDescripcion
            // 
            this.ucDescripcion.ancho = 200;
            this.ucDescripcion.isEntero = false;
            this.ucDescripcion.isMultilinea = true;
            this.ucDescripcion.isNegativo = false;
            this.ucDescripcion.isObligatorio = true;
            this.ucDescripcion.isSimboloDecimalPunto = true;
            this.ucDescripcion.isTexto = true;
            this.ucDescripcion.location = new System.Drawing.Point(150, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(19, 67);
            this.ucDescripcion.lonMax = 32767;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(365, 57);
            this.ucDescripcion.TabIndex = 1;
            this.ucDescripcion.uiLbl = "ucDescripcion";
            this.ucDescripcion.uitxt = "";
            this.ucDescripcion.valorDoubleNull = null;
            this.ucDescripcion.valorMaximo = -1D;
            this.ucDescripcion.valorMinimo = -1D;
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 200;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(150, 0);
            this.ucNombre.Location = new System.Drawing.Point(19, 34);
            this.ucNombre.lonMax = 32767;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(365, 20);
            this.ucNombre.TabIndex = 0;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorDoubleNull = null;
            this.ucNombre.valorMaximo = -1D;
            this.ucNombre.valorMinimo = -1D;
            // 
            // frmDatosProyecto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 447);
            this.Controls.Add(this.grSeccionTransversalSeparacion);
            this.Controls.Add(this.grTipoProyecto);
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.grDatosGenerales);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatosProyecto";
            this.Text = "frmDatosProyecto";
            this.grDatosGenerales.ResumeLayout(false);
            this.grTipoProyecto.ResumeLayout(false);
            this.grSeccionTransversalSeparacion.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grDatosGenerales;
        private userControl.ucLblTxt ucDescripcion;
        private userControl.ucLblTxt ucNombre;
        private userControl.ucToolDetail ucToolDetail1;
        private System.Windows.Forms.GroupBox grTipoProyecto;
        private userControl.ucProyectoTipo ucProyectoTipo1;
        private System.Windows.Forms.GroupBox grSeccionTransversalSeparacion;
        private userControl.ucLblTxt ucSeccionTransversalSeparacionMetros;
    }
}