namespace tadLayUI.adminGis
{
    partial class frmMacroPreciosDetail
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblHeader = new System.Windows.Forms.ToolStripLabel();
            this.grGeneral = new System.Windows.Forms.GroupBox();
            this.grDetalle = new System.Windows.Forms.GroupBox();
            this.lnkSalir = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.ucSeguridadSalud = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucMedidasCorrectoras = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucActuacionesComple = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucDesviosProvisionales = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucGeotecnicaCorrecciones = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucServiciosReposicion = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucBalizamiento = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucDrenaje = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.toolStrip1.SuspendLayout();
            this.grGeneral.SuspendLayout();
            this.grDetalle.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHeader});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(456, 25);
            this.toolStrip1.TabIndex = 23;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblHeader
            // 
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(58, 22);
            this.lblHeader.Text = "lblHeader";
            // 
            // grGeneral
            // 
            this.grGeneral.Controls.Add(this.ucDescripcion);
            this.grGeneral.Controls.Add(this.ucNombre);
            this.grGeneral.Location = new System.Drawing.Point(9, 31);
            this.grGeneral.Name = "grGeneral";
            this.grGeneral.Size = new System.Drawing.Size(438, 134);
            this.grGeneral.TabIndex = 24;
            this.grGeneral.TabStop = false;
            this.grGeneral.Text = "grMaster";
            // 
            // grDetalle
            // 
            this.grDetalle.Controls.Add(this.ucSeguridadSalud);
            this.grDetalle.Controls.Add(this.ucMedidasCorrectoras);
            this.grDetalle.Controls.Add(this.ucActuacionesComple);
            this.grDetalle.Controls.Add(this.ucDesviosProvisionales);
            this.grDetalle.Controls.Add(this.ucGeotecnicaCorrecciones);
            this.grDetalle.Controls.Add(this.ucServiciosReposicion);
            this.grDetalle.Controls.Add(this.ucBalizamiento);
            this.grDetalle.Controls.Add(this.ucDrenaje);
            this.grDetalle.Location = new System.Drawing.Point(9, 171);
            this.grDetalle.Name = "grDetalle";
            this.grDetalle.Size = new System.Drawing.Size(438, 303);
            this.grDetalle.TabIndex = 25;
            this.grDetalle.TabStop = false;
            this.grDetalle.Text = "grDetalle";
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(379, 479);
            this.lnkSalir.Name = "lnkSalir";
            this.lnkSalir.Size = new System.Drawing.Size(41, 13);
            this.lnkSalir.TabIndex = 27;
            this.lnkSalir.TabStop = true;
            this.lnkSalir.Text = "lnkSalir";
            this.lnkSalir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSalir_LinkClicked);
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(327, 479);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // ucSeguridadSalud
            // 
            this.ucSeguridadSalud.ancho = 200;
            this.ucSeguridadSalud.idCode = "GEN";
            this.ucSeguridadSalud.idGrupo = "SYS";
            this.ucSeguridadSalud.isObligatorio = true;
            this.ucSeguridadSalud.location = new System.Drawing.Point(200, 0);
            this.ucSeguridadSalud.Location = new System.Drawing.Point(14, 264);
            this.ucSeguridadSalud.Name = "ucSeguridadSalud";
            this.ucSeguridadSalud.Size = new System.Drawing.Size(415, 21);
            this.ucSeguridadSalud.TabIndex = 7;
            this.ucSeguridadSalud.uiLbl = "ucSeguridadSalud";
            // 
            // ucMedidasCorrectoras
            // 
            this.ucMedidasCorrectoras.ancho = 200;
            this.ucMedidasCorrectoras.idCode = "GEN";
            this.ucMedidasCorrectoras.idGrupo = "COR";
            this.ucMedidasCorrectoras.isObligatorio = false;
            this.ucMedidasCorrectoras.location = new System.Drawing.Point(200, 0);
            this.ucMedidasCorrectoras.Location = new System.Drawing.Point(14, 229);
            this.ucMedidasCorrectoras.Name = "ucMedidasCorrectoras";
            this.ucMedidasCorrectoras.Size = new System.Drawing.Size(415, 21);
            this.ucMedidasCorrectoras.TabIndex = 6;
            this.ucMedidasCorrectoras.uiLbl = "ucMedidasCorrectoras";
            // 
            // ucActuacionesComple
            // 
            this.ucActuacionesComple.ancho = 200;
            this.ucActuacionesComple.idCode = "GEN";
            this.ucActuacionesComple.idGrupo = "COM";
            this.ucActuacionesComple.isObligatorio = false;
            this.ucActuacionesComple.location = new System.Drawing.Point(200, 0);
            this.ucActuacionesComple.Location = new System.Drawing.Point(14, 194);
            this.ucActuacionesComple.Name = "ucActuacionesComple";
            this.ucActuacionesComple.Size = new System.Drawing.Size(415, 21);
            this.ucActuacionesComple.TabIndex = 5;
            this.ucActuacionesComple.uiLbl = "ucActuacionesComple";
            // 
            // ucDesviosProvisionales
            // 
            this.ucDesviosProvisionales.ancho = 200;
            this.ucDesviosProvisionales.idCode = "GEN";
            this.ucDesviosProvisionales.idGrupo = "DPR";
            this.ucDesviosProvisionales.isObligatorio = false;
            this.ucDesviosProvisionales.location = new System.Drawing.Point(200, 0);
            this.ucDesviosProvisionales.Location = new System.Drawing.Point(14, 159);
            this.ucDesviosProvisionales.Name = "ucDesviosProvisionales";
            this.ucDesviosProvisionales.Size = new System.Drawing.Size(415, 21);
            this.ucDesviosProvisionales.TabIndex = 4;
            this.ucDesviosProvisionales.uiLbl = "ucDesviosProvisionales";
            // 
            // ucGeotecnicaCorrecciones
            // 
            this.ucGeotecnicaCorrecciones.ancho = 200;
            this.ucGeotecnicaCorrecciones.idCode = "GEN";
            this.ucGeotecnicaCorrecciones.idGrupo = "CGE";
            this.ucGeotecnicaCorrecciones.isObligatorio = false;
            this.ucGeotecnicaCorrecciones.location = new System.Drawing.Point(200, 0);
            this.ucGeotecnicaCorrecciones.Location = new System.Drawing.Point(14, 124);
            this.ucGeotecnicaCorrecciones.Name = "ucGeotecnicaCorrecciones";
            this.ucGeotecnicaCorrecciones.Size = new System.Drawing.Size(415, 21);
            this.ucGeotecnicaCorrecciones.TabIndex = 3;
            this.ucGeotecnicaCorrecciones.uiLbl = "ucGeotecnicaCorrecciones";
            // 
            // ucServiciosReposicion
            // 
            this.ucServiciosReposicion.ancho = 200;
            this.ucServiciosReposicion.idCode = "GEN";
            this.ucServiciosReposicion.idGrupo = "RSE";
            this.ucServiciosReposicion.isObligatorio = false;
            this.ucServiciosReposicion.location = new System.Drawing.Point(200, 0);
            this.ucServiciosReposicion.Location = new System.Drawing.Point(14, 89);
            this.ucServiciosReposicion.Name = "ucServiciosReposicion";
            this.ucServiciosReposicion.Size = new System.Drawing.Size(415, 21);
            this.ucServiciosReposicion.TabIndex = 2;
            this.ucServiciosReposicion.uiLbl = "ucServiciosReposicion";
            // 
            // ucBalizamiento
            // 
            this.ucBalizamiento.ancho = 200;
            this.ucBalizamiento.idCode = "GEN";
            this.ucBalizamiento.idGrupo = "SEN";
            this.ucBalizamiento.isObligatorio = false;
            this.ucBalizamiento.location = new System.Drawing.Point(200, 0);
            this.ucBalizamiento.Location = new System.Drawing.Point(14, 54);
            this.ucBalizamiento.Name = "ucBalizamiento";
            this.ucBalizamiento.Size = new System.Drawing.Size(415, 21);
            this.ucBalizamiento.TabIndex = 1;
            this.ucBalizamiento.uiLbl = "ucBalizamiento";
            // 
            // ucDrenaje
            // 
            this.ucDrenaje.ancho = 200;
            this.ucDrenaje.idCode = "GEN";
            this.ucDrenaje.idGrupo = "DRE";
            this.ucDrenaje.isObligatorio = false;
            this.ucDrenaje.location = new System.Drawing.Point(200, 0);
            this.ucDrenaje.Location = new System.Drawing.Point(14, 19);
            this.ucDrenaje.Name = "ucDrenaje";
            this.ucDrenaje.Size = new System.Drawing.Size(415, 21);
            this.ucDrenaje.TabIndex = 0;
            this.ucDrenaje.uiLbl = "ucDrenaje";
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
            this.ucDescripcion.location = new System.Drawing.Point(200, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(21, 57);
            this.ucDescripcion.lonMax = 50;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(415, 62);
            this.ucDescripcion.TabIndex = 5;
            this.ucDescripcion.uiLbl = "ucDescripcion";
            this.ucDescripcion.uitxt = "";
            this.ucDescripcion.valorMaximo = 0D;
            this.ucDescripcion.valorMinimo = 0D;
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 200;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(200, 0);
            this.ucNombre.Location = new System.Drawing.Point(21, 23);
            this.ucNombre.lonMax = 50;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(415, 20);
            this.ucNombre.TabIndex = 1;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // frmMacroPreciosDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(456, 500);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.grDetalle);
            this.Controls.Add(this.grGeneral);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMacroPreciosDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMacroPrecioDetail";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEst_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grGeneral.ResumeLayout(false);
            this.grDetalle.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblHeader;
        private System.Windows.Forms.GroupBox grGeneral;
        private userControl.ucLblTxt ucDescripcion;
        private userControl.ucLblTxt ucNombre;
        private System.Windows.Forms.GroupBox grDetalle;
        private System.Windows.Forms.LinkLabel lnkSalir;
        private System.Windows.Forms.LinkLabel lnkSave;
        private userControl.uclblCmbPrecios ucDrenaje;
        private userControl.uclblCmbPrecios ucBalizamiento;
        private userControl.uclblCmbPrecios ucSeguridadSalud;
        private userControl.uclblCmbPrecios ucMedidasCorrectoras;
        private userControl.uclblCmbPrecios ucActuacionesComple;
        private userControl.uclblCmbPrecios ucDesviosProvisionales;
        private userControl.uclblCmbPrecios ucGeotecnicaCorrecciones;
        private userControl.uclblCmbPrecios ucServiciosReposicion;

    }
}