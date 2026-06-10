namespace tadLayUI.adminPresupuesto
{
    partial class frmPreciosFactory
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnLoadGrupo = new System.Windows.Forms.Button();
            this.dgvGrupos = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvClasificacion = new System.Windows.Forms.DataGridView();
            this.btnLoadGrupoClasificacion = new System.Windows.Forms.Button();
            this.dgvGrupoMaster = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnSubGrupo = new System.Windows.Forms.Button();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button5 = new System.Windows.Forms.Button();
            this.uclblCmbPrecios1 = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucCapasEspesor = new tadLayUI.userControl.ucLblTxt();
            this.ucCapasPosicion = new tadLayUI.userControl.ucLblTxt();
            this.btnGeoLoad = new System.Windows.Forms.Button();
            this.dgvGeo = new System.Windows.Forms.DataGridView();
            this.button4 = new System.Windows.Forms.Button();
            this.btnGrupoSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrupos)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClasificacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrupoMaster)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGeo)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(564, 607);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnLoadGrupo);
            this.tabPage1.Controls.Add(this.dgvGrupos);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(556, 581);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "GRUPOS";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnLoadGrupo
            // 
            this.btnLoadGrupo.Location = new System.Drawing.Point(17, 302);
            this.btnLoadGrupo.Name = "btnLoadGrupo";
            this.btnLoadGrupo.Size = new System.Drawing.Size(75, 23);
            this.btnLoadGrupo.TabIndex = 2;
            this.btnLoadGrupo.Text = "Load Grupo";
            this.btnLoadGrupo.UseVisualStyleBackColor = true;
            this.btnLoadGrupo.Click += new System.EventHandler(this.btnLoadGrupo_Click);
            // 
            // dgvGrupos
            // 
            this.dgvGrupos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrupos.Location = new System.Drawing.Point(17, 28);
            this.dgvGrupos.Name = "dgvGrupos";
            this.dgvGrupos.Size = new System.Drawing.Size(406, 268);
            this.dgvGrupos.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvClasificacion);
            this.tabPage2.Controls.Add(this.btnLoadGrupoClasificacion);
            this.tabPage2.Controls.Add(this.dgvGrupoMaster);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(556, 581);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "CLASIFICACIONES";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvClasificacion
            // 
            this.dgvClasificacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClasificacion.Location = new System.Drawing.Point(23, 235);
            this.dgvClasificacion.Name = "dgvClasificacion";
            this.dgvClasificacion.Size = new System.Drawing.Size(499, 300);
            this.dgvClasificacion.TabIndex = 4;
            // 
            // btnLoadGrupoClasificacion
            // 
            this.btnLoadGrupoClasificacion.Location = new System.Drawing.Point(23, 541);
            this.btnLoadGrupoClasificacion.Name = "btnLoadGrupoClasificacion";
            this.btnLoadGrupoClasificacion.Size = new System.Drawing.Size(75, 23);
            this.btnLoadGrupoClasificacion.TabIndex = 3;
            this.btnLoadGrupoClasificacion.Text = "Load Grupo";
            this.btnLoadGrupoClasificacion.UseVisualStyleBackColor = true;
            this.btnLoadGrupoClasificacion.Click += new System.EventHandler(this.btnLoadGrupoClasificacion_Click);
            // 
            // dgvGrupoMaster
            // 
            this.dgvGrupoMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrupoMaster.Location = new System.Drawing.Point(23, 31);
            this.dgvGrupoMaster.Name = "dgvGrupoMaster";
            this.dgvGrupoMaster.Size = new System.Drawing.Size(499, 198);
            this.dgvGrupoMaster.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnSubGrupo);
            this.tabPage3.Controls.Add(this.dgvItems);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(556, 581);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnSubGrupo
            // 
            this.btnSubGrupo.Location = new System.Drawing.Point(30, 519);
            this.btnSubGrupo.Name = "btnSubGrupo";
            this.btnSubGrupo.Size = new System.Drawing.Size(75, 23);
            this.btnSubGrupo.TabIndex = 4;
            this.btnSubGrupo.Text = "Load SubGrupo";
            this.btnSubGrupo.UseVisualStyleBackColor = true;
            this.btnSubGrupo.Click += new System.EventHandler(this.btnSubGrupo_Click);
            // 
            // dgvItems
            // 
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Location = new System.Drawing.Point(21, 23);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.Size = new System.Drawing.Size(442, 360);
            this.dgvItems.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button5);
            this.tabPage4.Controls.Add(this.uclblCmbPrecios1);
            this.tabPage4.Controls.Add(this.ucCapasEspesor);
            this.tabPage4.Controls.Add(this.ucCapasPosicion);
            this.tabPage4.Controls.Add(this.btnGeoLoad);
            this.tabPage4.Controls.Add(this.dgvGeo);
            this.tabPage4.Controls.Add(this.button4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(556, 581);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "tabGeo";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(350, 33);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 7;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // uclblCmbPrecios1
            // 
            this.uclblCmbPrecios1.ancho = 147;
            this.uclblCmbPrecios1.idCode = "ASI";
            this.uclblCmbPrecios1.idGrupo = "REL";
            this.uclblCmbPrecios1.isObligatorio = false;
            this.uclblCmbPrecios1.location = new System.Drawing.Point(162, 0);
            this.uclblCmbPrecios1.Location = new System.Drawing.Point(71, 322);
            this.uclblCmbPrecios1.Name = "uclblCmbPrecios1";
            this.uclblCmbPrecios1.Size = new System.Drawing.Size(309, 21);
            this.uclblCmbPrecios1.TabIndex = 6;
            this.uclblCmbPrecios1.uiLbl = "Material";
            // 
            // ucCapasEspesor
            // 
            this.ucCapasEspesor.ancho = 75;
            this.ucCapasEspesor.isEntero = false;
            this.ucCapasEspesor.isNegativo = false;
            this.ucCapasEspesor.isObligatorio = true;
            this.ucCapasEspesor.isSimboloDecimalPunto = true;
            this.ucCapasEspesor.location = new System.Drawing.Point(130, 0);
            this.ucCapasEspesor.Location = new System.Drawing.Point(71, 285);
            this.ucCapasEspesor.lonMax = 32767;
            this.ucCapasEspesor.Name = "ucCapasEspesor";
            this.ucCapasEspesor.Size = new System.Drawing.Size(205, 20);
            this.ucCapasEspesor.TabIndex = 5;
            this.ucCapasEspesor.uiLbl = "ucCapasEspesor";
            this.ucCapasEspesor.uitxt = "";
            this.ucCapasEspesor.valorMaximo = 1D;
            this.ucCapasEspesor.valorMinimo = 0D;
            // 
            // ucCapasPosicion
            // 
            this.ucCapasPosicion.ancho = 75;
            this.ucCapasPosicion.isEntero = false;
            this.ucCapasPosicion.isNegativo = false;
            this.ucCapasPosicion.isObligatorio = true;
            this.ucCapasPosicion.isSimboloDecimalPunto = true;
            this.ucCapasPosicion.location = new System.Drawing.Point(130, 0);
            this.ucCapasPosicion.Location = new System.Drawing.Point(71, 259);
            this.ucCapasPosicion.lonMax = 32767;
            this.ucCapasPosicion.Name = "ucCapasPosicion";
            this.ucCapasPosicion.Size = new System.Drawing.Size(205, 20);
            this.ucCapasPosicion.TabIndex = 4;
            this.ucCapasPosicion.uiLbl = "ucCapasPosicion";
            this.ucCapasPosicion.uitxt = "";
            this.ucCapasPosicion.valorMaximo = 100D;
            this.ucCapasPosicion.valorMinimo = 0D;
            // 
            // btnGeoLoad
            // 
            this.btnGeoLoad.Location = new System.Drawing.Point(126, 34);
            this.btnGeoLoad.Name = "btnGeoLoad";
            this.btnGeoLoad.Size = new System.Drawing.Size(108, 23);
            this.btnGeoLoad.TabIndex = 3;
            this.btnGeoLoad.Text = "Load";
            this.btnGeoLoad.UseVisualStyleBackColor = true;
            this.btnGeoLoad.Click += new System.EventHandler(this.btnGeoLoad_Click);
            // 
            // dgvGeo
            // 
            this.dgvGeo.AllowUserToAddRows = false;
            this.dgvGeo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGeo.Location = new System.Drawing.Point(33, 64);
            this.dgvGeo.Name = "dgvGeo";
            this.dgvGeo.Size = new System.Drawing.Size(488, 150);
            this.dgvGeo.TabIndex = 1;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(33, 34);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "GeoGeneral";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnGrupoSave
            // 
            this.btnGrupoSave.Location = new System.Drawing.Point(311, 656);
            this.btnGrupoSave.Name = "btnGrupoSave";
            this.btnGrupoSave.Size = new System.Drawing.Size(262, 23);
            this.btnGrupoSave.TabIndex = 1;
            this.btnGrupoSave.Text = "Gurardar XML";
            this.btnGrupoSave.UseVisualStyleBackColor = true;
            this.btnGrupoSave.Click += new System.EventHandler(this.btnGrupoSave_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 656);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Creata Moneda";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 626);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "CreateUDS";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(136, 626);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "CreateCapasTipo";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmPreciosFactory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 710);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGrupoSave);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmPreciosFactory";
            this.Text = "frmPreciosFactory";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrupos)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClasificacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrupoMaster)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGeo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnGrupoSave;
        private System.Windows.Forms.DataGridView dgvGrupos;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnLoadGrupo;
        private System.Windows.Forms.DataGridView dgvGrupoMaster;
        private System.Windows.Forms.Button btnLoadGrupoClasificacion;
        private System.Windows.Forms.DataGridView dgvClasificacion;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnSubGrupo;
        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView dgvGeo;
        private System.Windows.Forms.Button btnGeoLoad;
        private System.Windows.Forms.Button button2;
        private userControl.uclblCmbPrecios uclblCmbPrecios1;
        private userControl.ucLblTxt ucCapasEspesor;
        private userControl.ucLblTxt ucCapasPosicion;
        private System.Windows.Forms.Button button5;

    }
}