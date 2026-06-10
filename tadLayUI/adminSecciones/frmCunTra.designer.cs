namespace tadLayUI.adminSecciones
{
    partial class frmCunTra
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
            this.imgCun = new System.Windows.Forms.PictureBox();
            this.lnkSalir = new System.Windows.Forms.LinkLabel();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.ucAnchoIntInfM = new tadLayUI.userControl.ucLblTxt();
            this.ucEspesorIntM = new tadLayUI.userControl.ucLblTxt();
            this.ucAnchoIntSupM = new tadLayUI.userControl.ucLblTxt();
            this.ucAlturaIntM = new tadLayUI.userControl.ucLblTxt();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.toolStrip1.SuspendLayout();
            this.grGeneral.SuspendLayout();
            this.grDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgCun)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(467, 25);
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
            this.grGeneral.Controls.Add(this.ucAnchoIntInfM);
            this.grGeneral.Controls.Add(this.ucEspesorIntM);
            this.grGeneral.Controls.Add(this.ucAnchoIntSupM);
            this.grGeneral.Controls.Add(this.ucAlturaIntM);
            this.grGeneral.Controls.Add(this.ucDescripcion);
            this.grGeneral.Controls.Add(this.ucNombre);
            this.grGeneral.Location = new System.Drawing.Point(9, 31);
            this.grGeneral.Name = "grGeneral";
            this.grGeneral.Size = new System.Drawing.Size(444, 238);
            this.grGeneral.TabIndex = 24;
            this.grGeneral.TabStop = false;
            this.grGeneral.Text = "grMaster";
            // 
            // grDetalle
            // 
            this.grDetalle.Controls.Add(this.imgCun);
            this.grDetalle.Location = new System.Drawing.Point(9, 278);
            this.grDetalle.Name = "grDetalle";
            this.grDetalle.Size = new System.Drawing.Size(444, 197);
            this.grDetalle.TabIndex = 25;
            this.grDetalle.TabStop = false;
            this.grDetalle.Text = "grDetalle";
            // 
            // imgCun
            // 
            this.imgCun.Location = new System.Drawing.Point(220, 27);
            this.imgCun.Name = "imgCun";
            this.imgCun.Size = new System.Drawing.Size(205, 150);
            this.imgCun.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgCun.TabIndex = 0;
            this.imgCun.TabStop = false;
            // 
            // lnkSalir
            // 
            this.lnkSalir.AutoSize = true;
            this.lnkSalir.Location = new System.Drawing.Point(411, 498);
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
            this.lnkSave.Location = new System.Drawing.Point(359, 498);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // ucAnchoIntInfM
            // 
            this.ucAnchoIntInfM.ancho = 75;
            this.ucAnchoIntInfM.isEntero = false;
            this.ucAnchoIntInfM.isNegativo = false;
            this.ucAnchoIntInfM.isObligatorio = true;
            this.ucAnchoIntInfM.isSimboloDecimalPunto = true;
            this.ucAnchoIntInfM.isTexto = true;
            this.ucAnchoIntInfM.location = new System.Drawing.Point(200, 0);
            this.ucAnchoIntInfM.Location = new System.Drawing.Point(19, 181);
            this.ucAnchoIntInfM.lonMax = 50;
            this.ucAnchoIntInfM.Name = "ucAnchoIntInfM";
            this.ucAnchoIntInfM.Size = new System.Drawing.Size(406, 20);
            this.ucAnchoIntInfM.TabIndex = 9;
            this.ucAnchoIntInfM.uiLbl = "ucAnchoIntInfM";
            this.ucAnchoIntInfM.uitxt = "";
            this.ucAnchoIntInfM.valorMaximo = 100D;
            this.ucAnchoIntInfM.valorMinimo = 0D;
            // 
            // ucEspesorIntM
            // 
            this.ucEspesorIntM.ancho = 75;
            this.ucEspesorIntM.isEntero = false;
            this.ucEspesorIntM.isNegativo = false;
            this.ucEspesorIntM.isObligatorio = true;
            this.ucEspesorIntM.isSimboloDecimalPunto = true;
            this.ucEspesorIntM.isTexto = true;
            this.ucEspesorIntM.location = new System.Drawing.Point(200, 0);
            this.ucEspesorIntM.Location = new System.Drawing.Point(19, 208);
            this.ucEspesorIntM.lonMax = 50;
            this.ucEspesorIntM.Name = "ucEspesorIntM";
            this.ucEspesorIntM.Size = new System.Drawing.Size(406, 20);
            this.ucEspesorIntM.TabIndex = 8;
            this.ucEspesorIntM.uiLbl = "ucEspesorIntM";
            this.ucEspesorIntM.uitxt = "";
            this.ucEspesorIntM.valorMaximo = 100D;
            this.ucEspesorIntM.valorMinimo = 0D;
            // 
            // ucAnchoIntSupM
            // 
            this.ucAnchoIntSupM.ancho = 75;
            this.ucAnchoIntSupM.isEntero = false;
            this.ucAnchoIntSupM.isNegativo = false;
            this.ucAnchoIntSupM.isObligatorio = true;
            this.ucAnchoIntSupM.isSimboloDecimalPunto = true;
            this.ucAnchoIntSupM.isTexto = true;
            this.ucAnchoIntSupM.location = new System.Drawing.Point(200, 0);
            this.ucAnchoIntSupM.Location = new System.Drawing.Point(19, 153);
            this.ucAnchoIntSupM.lonMax = 50;
            this.ucAnchoIntSupM.Name = "ucAnchoIntSupM";
            this.ucAnchoIntSupM.Size = new System.Drawing.Size(406, 20);
            this.ucAnchoIntSupM.TabIndex = 7;
            this.ucAnchoIntSupM.uiLbl = "ucAnchoIntSupM";
            this.ucAnchoIntSupM.uitxt = "";
            this.ucAnchoIntSupM.valorMaximo = 100D;
            this.ucAnchoIntSupM.valorMinimo = 0D;
            // 
            // ucAlturaIntM
            // 
            this.ucAlturaIntM.ancho = 75;
            this.ucAlturaIntM.isEntero = false;
            this.ucAlturaIntM.isNegativo = false;
            this.ucAlturaIntM.isObligatorio = true;
            this.ucAlturaIntM.isSimboloDecimalPunto = true;
            this.ucAlturaIntM.isTexto = true;
            this.ucAlturaIntM.location = new System.Drawing.Point(200, 0);
            this.ucAlturaIntM.Location = new System.Drawing.Point(19, 124);
            this.ucAlturaIntM.lonMax = 50;
            this.ucAlturaIntM.Name = "ucAlturaIntM";
            this.ucAlturaIntM.Size = new System.Drawing.Size(406, 20);
            this.ucAlturaIntM.TabIndex = 6;
            this.ucAlturaIntM.uiLbl = "ucAlturaIntM";
            this.ucAlturaIntM.uitxt = "";
            this.ucAlturaIntM.valorMaximo = 100D;
            this.ucAlturaIntM.valorMinimo = 0D;
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
            this.ucDescripcion.Location = new System.Drawing.Point(21, 54);
            this.ucDescripcion.lonMax = 50;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(406, 62);
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
            this.ucNombre.Size = new System.Drawing.Size(406, 20);
            this.ucNombre.TabIndex = 1;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // frmCunTra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(467, 523);
            this.Controls.Add(this.lnkSalir);
            this.Controls.Add(this.lnkSave);
            this.Controls.Add(this.grDetalle);
            this.Controls.Add(this.grGeneral);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCunTra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCunetaTriangular";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEst_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grGeneral.ResumeLayout(false);
            this.grDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgCun)).EndInit();
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
        private userControl.ucLblTxt ucEspesorIntM;
        private userControl.ucLblTxt ucAnchoIntSupM;
        private userControl.ucLblTxt ucAlturaIntM;
        private System.Windows.Forms.PictureBox imgCun;
        private userControl.ucLblTxt ucAnchoIntInfM;
    }
}