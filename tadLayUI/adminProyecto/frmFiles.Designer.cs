namespace tadLayUI.adminProyecto
{
    partial class frmFiles
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
            this.grFilesNormas = new System.Windows.Forms.GroupBox();
            this.lnkFileVpKvOpen = new System.Windows.Forms.LinkLabel();
            this.lnkFileVpRpOpen = new System.Windows.Forms.LinkLabel();
            this.lnkFileVpKvSelect = new System.Windows.Forms.LinkLabel();
            this.lnkFileVpRpSelect = new System.Windows.Forms.LinkLabel();
            this.ucFileNormasVpKv = new tadLayUI.userControl.ucLblTxt();
            this.ucFileNormasVpRadio = new tadLayUI.userControl.ucLblTxt();
            this.lnkFileBdSelect = new System.Windows.Forms.LinkLabel();
            this.ucFileBaseDatos = new tadLayUI.userControl.ucLblTxt();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.grFileBaseDatos = new System.Windows.Forms.GroupBox();
            this.grFilesNormas.SuspendLayout();
            this.grFileBaseDatos.SuspendLayout();
            this.SuspendLayout();
            // 
            // grFilesNormas
            // 
            this.grFilesNormas.Controls.Add(this.lnkFileVpKvOpen);
            this.grFilesNormas.Controls.Add(this.lnkFileVpRpOpen);
            this.grFilesNormas.Controls.Add(this.lnkFileVpKvSelect);
            this.grFilesNormas.Controls.Add(this.lnkFileVpRpSelect);
            this.grFilesNormas.Controls.Add(this.ucFileNormasVpKv);
            this.grFilesNormas.Controls.Add(this.ucFileNormasVpRadio);
            this.grFilesNormas.Location = new System.Drawing.Point(7, 13);
            this.grFilesNormas.Name = "grFilesNormas";
            this.grFilesNormas.Size = new System.Drawing.Size(523, 182);
            this.grFilesNormas.TabIndex = 0;
            this.grFilesNormas.TabStop = false;
            this.grFilesNormas.Text = "grFilesNormas";
            // 
            // lnkFileVpKvOpen
            // 
            this.lnkFileVpKvOpen.AutoSize = true;
            this.lnkFileVpKvOpen.Location = new System.Drawing.Point(414, 140);
            this.lnkFileVpKvOpen.Name = "lnkFileVpKvOpen";
            this.lnkFileVpKvOpen.Size = new System.Drawing.Size(89, 13);
            this.lnkFileVpKvOpen.TabIndex = 7;
            this.lnkFileVpKvOpen.TabStop = true;
            this.lnkFileVpKvOpen.Text = "lnkFileVpKvOpen";
            this.lnkFileVpKvOpen.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFileVpKvOpen_LinkClicked);
            // 
            // lnkFileVpRpOpen
            // 
            this.lnkFileVpRpOpen.AutoSize = true;
            this.lnkFileVpRpOpen.Location = new System.Drawing.Point(415, 59);
            this.lnkFileVpRpOpen.Name = "lnkFileVpRpOpen";
            this.lnkFileVpRpOpen.Size = new System.Drawing.Size(90, 13);
            this.lnkFileVpRpOpen.TabIndex = 6;
            this.lnkFileVpRpOpen.TabStop = true;
            this.lnkFileVpRpOpen.Text = "lnkFileVpRpOpen";
            this.lnkFileVpRpOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkFileVpRpOpen.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFileVpRpOpen_LinkClicked);
            // 
            // lnkFileVpKvSelect
            // 
            this.lnkFileVpKvSelect.AutoSize = true;
            this.lnkFileVpKvSelect.Location = new System.Drawing.Point(175, 140);
            this.lnkFileVpKvSelect.Name = "lnkFileVpKvSelect";
            this.lnkFileVpKvSelect.Size = new System.Drawing.Size(93, 13);
            this.lnkFileVpKvSelect.TabIndex = 5;
            this.lnkFileVpKvSelect.TabStop = true;
            this.lnkFileVpKvSelect.Text = "lnkFileVpKvSelect";
            this.lnkFileVpKvSelect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFileVpKvSelect_LinkClicked);
            // 
            // lnkFileVpRpSelect
            // 
            this.lnkFileVpRpSelect.AutoSize = true;
            this.lnkFileVpRpSelect.Location = new System.Drawing.Point(174, 59);
            this.lnkFileVpRpSelect.Name = "lnkFileVpRpSelect";
            this.lnkFileVpRpSelect.Size = new System.Drawing.Size(94, 13);
            this.lnkFileVpRpSelect.TabIndex = 4;
            this.lnkFileVpRpSelect.TabStop = true;
            this.lnkFileVpRpSelect.Text = "lnkFileVpRpSelect";
            this.lnkFileVpRpSelect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFileVpRpSelect_LinkClicked);
            // 
            // ucFileNormasVpKv
            // 
            this.ucFileNormasVpKv.ancho = 330;
            this.ucFileNormasVpKv.isEntero = false;
            this.ucFileNormasVpKv.isNegativo = false;
            this.ucFileNormasVpKv.isObligatorio = true;
            this.ucFileNormasVpKv.isSimboloDecimalPunto = true;
            this.ucFileNormasVpKv.isTexto = true;
            this.ucFileNormasVpKv.location = new System.Drawing.Point(160, 0);
            this.ucFileNormasVpKv.Location = new System.Drawing.Point(14, 109);
            this.ucFileNormasVpKv.lonMax = 32767;
            this.ucFileNormasVpKv.Name = "ucFileNormasVpKv";
            this.ucFileNormasVpKv.Size = new System.Drawing.Size(496, 20);
            this.ucFileNormasVpKv.TabIndex = 2;
            this.ucFileNormasVpKv.uiLbl = "ucFileNormasVpKv";
            this.ucFileNormasVpKv.uitxt = "";
            this.ucFileNormasVpKv.valorDoubleNull = null;
            this.ucFileNormasVpKv.valorMaximo = 0D;
            this.ucFileNormasVpKv.valorMinimo = 0D;
            // 
            // ucFileNormasVpRadio
            // 
            this.ucFileNormasVpRadio.ancho = 330;
            this.ucFileNormasVpRadio.isEntero = false;
            this.ucFileNormasVpRadio.isNegativo = false;
            this.ucFileNormasVpRadio.isObligatorio = true;
            this.ucFileNormasVpRadio.isSimboloDecimalPunto = true;
            this.ucFileNormasVpRadio.isTexto = true;
            this.ucFileNormasVpRadio.location = new System.Drawing.Point(160, 0);
            this.ucFileNormasVpRadio.Location = new System.Drawing.Point(14, 27);
            this.ucFileNormasVpRadio.lonMax = 32767;
            this.ucFileNormasVpRadio.Name = "ucFileNormasVpRadio";
            this.ucFileNormasVpRadio.Size = new System.Drawing.Size(496, 20);
            this.ucFileNormasVpRadio.TabIndex = 1;
            this.ucFileNormasVpRadio.uiLbl = "ucFileNormasVpRadio";
            this.ucFileNormasVpRadio.uitxt = "";
            this.ucFileNormasVpRadio.valorDoubleNull = null;
            this.ucFileNormasVpRadio.valorMaximo = 0D;
            this.ucFileNormasVpRadio.valorMinimo = 0D;
            // 
            // lnkFileBdSelect
            // 
            this.lnkFileBdSelect.AutoSize = true;
            this.lnkFileBdSelect.Location = new System.Drawing.Point(174, 68);
            this.lnkFileBdSelect.Name = "lnkFileBdSelect";
            this.lnkFileBdSelect.Size = new System.Drawing.Size(80, 13);
            this.lnkFileBdSelect.TabIndex = 3;
            this.lnkFileBdSelect.TabStop = true;
            this.lnkFileBdSelect.Text = "lnkFileBdSelect";
            this.lnkFileBdSelect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFileBdSelect_LinkClicked);
            // 
            // ucFileBaseDatos
            // 
            this.ucFileBaseDatos.ancho = 330;
            this.ucFileBaseDatos.isEntero = false;
            this.ucFileBaseDatos.isNegativo = false;
            this.ucFileBaseDatos.isObligatorio = true;
            this.ucFileBaseDatos.isSimboloDecimalPunto = true;
            this.ucFileBaseDatos.isTexto = true;
            this.ucFileBaseDatos.location = new System.Drawing.Point(160, 0);
            this.ucFileBaseDatos.Location = new System.Drawing.Point(16, 35);
            this.ucFileBaseDatos.lonMax = 32767;
            this.ucFileBaseDatos.Name = "ucFileBaseDatos";
            this.ucFileBaseDatos.Size = new System.Drawing.Size(496, 20);
            this.ucFileBaseDatos.TabIndex = 0;
            this.ucFileBaseDatos.uiLbl = "ucFileBaseDatos";
            this.ucFileBaseDatos.uitxt = "";
            this.ucFileBaseDatos.valorDoubleNull = null;
            this.ucFileBaseDatos.valorMaximo = 0D;
            this.ucFileBaseDatos.valorMinimo = 0D;
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(0, 310);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(534, 25);
            this.ucToolDetail1.TabIndex = 1;
            // 
            // grFileBaseDatos
            // 
            this.grFileBaseDatos.Controls.Add(this.ucFileBaseDatos);
            this.grFileBaseDatos.Controls.Add(this.lnkFileBdSelect);
            this.grFileBaseDatos.Location = new System.Drawing.Point(9, 205);
            this.grFileBaseDatos.Name = "grFileBaseDatos";
            this.grFileBaseDatos.Size = new System.Drawing.Size(518, 100);
            this.grFileBaseDatos.TabIndex = 2;
            this.grFileBaseDatos.TabStop = false;
            this.grFileBaseDatos.Text = "grFileBaseDatos";
            // 
            // frmFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 336);
            this.Controls.Add(this.grFileBaseDatos);
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.grFilesNormas);
            this.Name = "frmFiles";
            this.Text = "frmFiles";
            this.Shown += new System.EventHandler(this.frmFiles_Shown);
            this.grFilesNormas.ResumeLayout(false);
            this.grFilesNormas.PerformLayout();
            this.grFileBaseDatos.ResumeLayout(false);
            this.grFileBaseDatos.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grFilesNormas;
        private userControl.ucLblTxt ucFileBaseDatos;
        private userControl.ucLblTxt ucFileNormasVpKv;
        private userControl.ucLblTxt ucFileNormasVpRadio;
        private System.Windows.Forms.LinkLabel lnkFileVpKvSelect;
        private System.Windows.Forms.LinkLabel lnkFileVpRpSelect;
        private System.Windows.Forms.LinkLabel lnkFileBdSelect;
        private userControl.ucToolDetail ucToolDetail1;
        private System.Windows.Forms.LinkLabel lnkFileVpRpOpen;
        private System.Windows.Forms.LinkLabel lnkFileVpKvOpen;
        private System.Windows.Forms.GroupBox grFileBaseDatos;
    }
}