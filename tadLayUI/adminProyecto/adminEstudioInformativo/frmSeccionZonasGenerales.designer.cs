namespace tadLayUI.adminProyecto
{
    partial class frmSeccionZonasGenerales
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
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.grProyectoSeccion = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.ucSeccionItem = new tadLayUI.userControl.uclblCmbMaster();
            this.ucSeccionTipo = new tadLayUI.userControl.ucCmbCalzadasTipo();
            this.ucSeccionMacroPrecio = new tadLayUI.userControl.uclblCmbMaster();
            this.ucSeccionGrupo = new tadLayUI.userControl.ucCmbCalzadasGrupo();
            this.grZonasGenerales = new System.Windows.Forms.GroupBox();
            this.ucZonasTuneles1 = new tadLayUI.userControl.ucZonasTuneles();
            this.ucZonasEstructuras1 = new tadLayUI.userControl.ucZonasEstructuras();
            this.ucZonasCimentacion1 = new tadLayUI.userControl.ucZonasCimentacion();
            this.ucZonasMovimientoTierras1 = new tadLayUI.userControl.ucZonasMovimientoTierras();
            this.label1 = new System.Windows.Forms.Label();
            this.grProyectoSeccion.SuspendLayout();
            this.grZonasGenerales.SuspendLayout();
            this.SuspendLayout();
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(443, 416);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // grProyectoSeccion
            // 
            this.grProyectoSeccion.Controls.Add(this.label1);
            this.grProyectoSeccion.Controls.Add(this.checkBox1);
            this.grProyectoSeccion.Controls.Add(this.ucSeccionItem);
            this.grProyectoSeccion.Controls.Add(this.ucSeccionTipo);
            this.grProyectoSeccion.Controls.Add(this.ucSeccionMacroPrecio);
            this.grProyectoSeccion.Controls.Add(this.ucSeccionGrupo);
            this.grProyectoSeccion.Location = new System.Drawing.Point(12, 12);
            this.grProyectoSeccion.Name = "grProyectoSeccion";
            this.grProyectoSeccion.Size = new System.Drawing.Size(483, 221);
            this.grProyectoSeccion.TabIndex = 28;
            this.grProyectoSeccion.TabStop = false;
            this.grProyectoSeccion.Text = "grProyectoSeccion";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(14, 29);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(463, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Utilizar secciones vinculadas (si el eje no pasa por una seccion creada se utiliz" +
    "ara la general)";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // ucSeccionItem
            // 
            this.ucSeccionItem.ancho = 275;
            this.ucSeccionItem.isObligatorio = true;
            this.ucSeccionItem.location = new System.Drawing.Point(175, 0);
            this.ucSeccionItem.Location = new System.Drawing.Point(16, 154);
            this.ucSeccionItem.Name = "ucSeccionItem";
            this.ucSeccionItem.Size = new System.Drawing.Size(462, 21);
            this.ucSeccionItem.TabIndex = 3;
            this.ucSeccionItem.uiLbl = "ucSeccionItem";
            // 
            // ucSeccionTipo
            // 
            this.ucSeccionTipo.ancho = 275;
            this.ucSeccionTipo.codeRoad = null;
            this.ucSeccionTipo.isObligatorio = true;
            this.ucSeccionTipo.location = new System.Drawing.Point(175, 0);
            this.ucSeccionTipo.Location = new System.Drawing.Point(16, 119);
            this.ucSeccionTipo.Name = "ucSeccionTipo";
            this.ucSeccionTipo.Size = new System.Drawing.Size(462, 21);
            this.ucSeccionTipo.TabIndex = 2;
            this.ucSeccionTipo.uiLbl = "ucSeccionTipo";
            // 
            // ucSeccionMacroPrecio
            // 
            this.ucSeccionMacroPrecio.ancho = 275;
            this.ucSeccionMacroPrecio.isObligatorio = true;
            this.ucSeccionMacroPrecio.location = new System.Drawing.Point(175, 0);
            this.ucSeccionMacroPrecio.Location = new System.Drawing.Point(16, 191);
            this.ucSeccionMacroPrecio.Name = "ucSeccionMacroPrecio";
            this.ucSeccionMacroPrecio.Size = new System.Drawing.Size(462, 21);
            this.ucSeccionMacroPrecio.TabIndex = 4;
            this.ucSeccionMacroPrecio.uiLbl = "ucSeccionMacroPrecio";
            // 
            // ucSeccionGrupo
            // 
            this.ucSeccionGrupo.ancho = 275;
            this.ucSeccionGrupo.isObligatorio = true;
            this.ucSeccionGrupo.location = new System.Drawing.Point(175, 0);
            this.ucSeccionGrupo.Location = new System.Drawing.Point(16, 86);
            this.ucSeccionGrupo.Name = "ucSeccionGrupo";
            this.ucSeccionGrupo.Size = new System.Drawing.Size(462, 21);
            this.ucSeccionGrupo.TabIndex = 1;
            this.ucSeccionGrupo.uiLbl = "ucSeccionGrupo";
            // 
            // grZonasGenerales
            // 
            this.grZonasGenerales.Controls.Add(this.ucZonasTuneles1);
            this.grZonasGenerales.Controls.Add(this.ucZonasEstructuras1);
            this.grZonasGenerales.Controls.Add(this.ucZonasCimentacion1);
            this.grZonasGenerales.Controls.Add(this.ucZonasMovimientoTierras1);
            this.grZonasGenerales.Location = new System.Drawing.Point(15, 239);
            this.grZonasGenerales.Name = "grZonasGenerales";
            this.grZonasGenerales.Size = new System.Drawing.Size(480, 174);
            this.grZonasGenerales.TabIndex = 29;
            this.grZonasGenerales.TabStop = false;
            this.grZonasGenerales.Text = "grZonasGenerales";
            // 
            // ucZonasTuneles1
            // 
            this.ucZonasTuneles1.ancho = 275;
            this.ucZonasTuneles1.isObligatorio = true;
            this.ucZonasTuneles1.location = new System.Drawing.Point(175, 0);
            this.ucZonasTuneles1.Location = new System.Drawing.Point(12, 145);
            this.ucZonasTuneles1.Name = "ucZonasTuneles1";
            this.ucZonasTuneles1.Size = new System.Drawing.Size(462, 21);
            this.ucZonasTuneles1.TabIndex = 3;
            this.ucZonasTuneles1.uiLbl = "ucZonasTuneles1";
            // 
            // ucZonasEstructuras1
            // 
            this.ucZonasEstructuras1.ancho = 275;
            this.ucZonasEstructuras1.isObligatorio = true;
            this.ucZonasEstructuras1.location = new System.Drawing.Point(175, 0);
            this.ucZonasEstructuras1.Location = new System.Drawing.Point(12, 105);
            this.ucZonasEstructuras1.Name = "ucZonasEstructuras1";
            this.ucZonasEstructuras1.Size = new System.Drawing.Size(462, 21);
            this.ucZonasEstructuras1.TabIndex = 2;
            this.ucZonasEstructuras1.uiLbl = "ucZonasEstructuras1";
            // 
            // ucZonasCimentacion1
            // 
            this.ucZonasCimentacion1.ancho = 275;
            this.ucZonasCimentacion1.isObligatorio = false;
            this.ucZonasCimentacion1.location = new System.Drawing.Point(175, 0);
            this.ucZonasCimentacion1.Location = new System.Drawing.Point(12, 68);
            this.ucZonasCimentacion1.Name = "ucZonasCimentacion1";
            this.ucZonasCimentacion1.Size = new System.Drawing.Size(462, 21);
            this.ucZonasCimentacion1.TabIndex = 1;
            this.ucZonasCimentacion1.uiLbl = "ucZonasCimentacion1";
            // 
            // ucZonasMovimientoTierras1
            // 
            this.ucZonasMovimientoTierras1.ancho = 275;
            this.ucZonasMovimientoTierras1.isObligatorio = true;
            this.ucZonasMovimientoTierras1.location = new System.Drawing.Point(175, 0);
            this.ucZonasMovimientoTierras1.Location = new System.Drawing.Point(12, 28);
            this.ucZonasMovimientoTierras1.Name = "ucZonasMovimientoTierras1";
            this.ucZonasMovimientoTierras1.Size = new System.Drawing.Size(462, 21);
            this.ucZonasMovimientoTierras1.TabIndex = 0;
            this.ucZonasMovimientoTierras1.uiLbl = "ucZonasMovimientoTierras1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Secciónes generales";
            // 
            // frmSeccionZonasGenerales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(572, 445);
            this.Controls.Add(this.grZonasGenerales);
            this.Controls.Add(this.grProyectoSeccion);
            this.Controls.Add(this.lnkSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSeccionZonasGenerales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmProyectoSeccion";
            this.grProyectoSeccion.ResumeLayout(false);
            this.grProyectoSeccion.PerformLayout();
            this.grZonasGenerales.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.GroupBox grProyectoSeccion;
        private userControl.ucCmbCalzadasGrupo ucSeccionGrupo;
        private userControl.uclblCmbMaster ucSeccionMacroPrecio;
        private userControl.ucCmbCalzadasTipo ucSeccionTipo;
        private userControl.uclblCmbMaster ucSeccionItem;
        private System.Windows.Forms.GroupBox grZonasGenerales;
        private userControl.ucZonasMovimientoTierras ucZonasMovimientoTierras1;
        private userControl.ucZonasCimentacion ucZonasCimentacion1;
        private userControl.ucZonasTuneles ucZonasTuneles1;
        private userControl.ucZonasEstructuras ucZonasEstructuras1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
    }
}