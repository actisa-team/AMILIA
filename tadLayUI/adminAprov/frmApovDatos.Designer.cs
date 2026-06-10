
namespace tadLayUI.adminAprov
{
    partial class frmApovDatos
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
            this.lnkSalir = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.chfirme = new System.Windows.Forms.CheckBox();
            this.chsaneo = new System.Windows.Forms.CheckBox();
            this.ucColor = new tadLayUI.userControl.ucWinColor();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.ucEspesor = new tadLayUI.userControl.ucLblTxt();
            this.SuspendLayout();
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(367, 157);
            this.lnkSalir.Name = "lnkSalir";
            this.lnkSalir.Size = new System.Drawing.Size(41, 13);
            this.lnkSalir.TabIndex = 28;
            this.lnkSalir.TabStop = true;
            this.lnkSalir.Text = "lnkSalir";
            this.lnkSalir.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSalir_LinkClicked);
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(315, 157);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 27;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // chfirme
            // 
            this.chfirme.AutoSize = true;
            this.chfirme.Location = new System.Drawing.Point(261, 125);
            this.chfirme.Name = "chfirme";
            this.chfirme.Size = new System.Drawing.Size(128, 17);
            this.chfirme.TabIndex = 34;
            this.chfirme.Text = "chkIsSaneoTerraplen";
            this.chfirme.UseVisualStyleBackColor = true;
            // 
            // chsaneo
            // 
            this.chsaneo.AutoSize = true;
            this.chsaneo.Location = new System.Drawing.Point(343, 125);
            this.chsaneo.Name = "chsaneo";
            this.chsaneo.Size = new System.Drawing.Size(80, 17);
            this.chsaneo.TabIndex = 35;
            this.chsaneo.Text = "checkBox1";
            this.chsaneo.UseVisualStyleBackColor = true;
            // 
            // ucColor
            // 
            this.ucColor.ancho = 145;
            this.ucColor.location = new System.Drawing.Point(250, 1);
            this.ucColor.Location = new System.Drawing.Point(12, 99);
            this.ucColor.Name = "ucColor";
            this.ucColor.Size = new System.Drawing.Size(402, 20);
            this.ucColor.TabIndex = 33;
            this.ucColor.uiLbl = "ucWinColorZona";
            // 
            // ucDescripcion
            // 
            this.ucDescripcion.ancho = 145;
            this.ucDescripcion.isEntero = false;
            this.ucDescripcion.isNegativo = false;
            this.ucDescripcion.isObligatorio = true;
            this.ucDescripcion.isSimboloDecimalPunto = true;
            this.ucDescripcion.isTexto = true;
            this.ucDescripcion.location = new System.Drawing.Point(250, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(12, 47);
            this.ucDescripcion.lonMax = 50;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(423, 20);
            this.ucDescripcion.TabIndex = 32;
            this.ucDescripcion.uiLbl = "lblGrupoLitologico";
            this.ucDescripcion.uitxt = "";
            this.ucDescripcion.valorDoubleNull = null;
            this.ucDescripcion.valorMaximo = 0D;
            this.ucDescripcion.valorMinimo = 0D;
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 145;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(250, 0);
            this.ucNombre.Location = new System.Drawing.Point(12, 21);
            this.ucNombre.lonMax = 50;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(423, 20);
            this.ucNombre.TabIndex = 31;
            this.ucNombre.uiLbl = "lblGrupoLitologico";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorDoubleNull = null;
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // ucEspesor
            // 
            this.ucEspesor.ancho = 145;
            this.ucEspesor.isEntero = false;
            this.ucEspesor.isNegativo = false;
            this.ucEspesor.isObligatorio = true;
            this.ucEspesor.isSimboloDecimalPunto = true;
            this.ucEspesor.location = new System.Drawing.Point(250, 0);
            this.ucEspesor.Location = new System.Drawing.Point(12, 73);
            this.ucEspesor.lonMax = 50;
            this.ucEspesor.Name = "ucEspesor";
            this.ucEspesor.Size = new System.Drawing.Size(423, 20);
            this.ucEspesor.TabIndex = 30;
            this.ucEspesor.uiLbl = "lblPendMaxTerreno";
            this.ucEspesor.uitxt = "";
            this.ucEspesor.valorDoubleNull = null;
            this.ucEspesor.valorMaximo = 1000000D;
            this.ucEspesor.valorMinimo = 0D;
            // 
            // frmApovDatos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 182);
            this.Controls.Add(this.chsaneo);
            this.Controls.Add(this.chfirme);
            this.Controls.Add(this.ucColor);
            this.Controls.Add(this.ucDescripcion);
            this.Controls.Add(this.ucNombre);
            this.Controls.Add(this.ucEspesor);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Name = "frmApovDatos";
            this.Text = "Datos de Adecuación y Saneo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel lnkSalir;
        private System.Windows.Forms.LinkLabel lnkSave;
        private userControl.ucLblTxt ucEspesor;
        private userControl.ucLblTxt ucNombre;
        private userControl.ucLblTxt ucDescripcion;
        private userControl.ucWinColor ucColor;
        private System.Windows.Forms.CheckBox chfirme;
        private System.Windows.Forms.CheckBox chsaneo;
    }
}