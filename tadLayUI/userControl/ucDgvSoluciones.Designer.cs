namespace tadLayUI.userControl
{
    partial class ucDgvSoluciones
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnPerfilCivil = new System.Windows.Forms.Button();
            this.btnObraLineal = new System.Windows.Forms.Button();
            this.btnExportarObraLineal = new System.Windows.Forms.Button();
            this.btnInformeObraLineal = new System.Windows.Forms.Button();
            this.btnEjeCrear = new System.Windows.Forms.Button();
            this.btnEjeRotular = new System.Windows.Forms.Button();
            this.grEjeTrazado = new System.Windows.Forms.GroupBox();
            this.btnEjeInforme = new System.Windows.Forms.Button();
            this.grPerfil = new System.Windows.Forms.GroupBox();
            this.btnExportaLong = new System.Windows.Forms.Button();
            this.btnPerfilInforme = new System.Windows.Forms.Button();
            this.grObraLineal = new System.Windows.Forms.GroupBox();
            this.ucToolDgv1 = new tadLayUI.userControl.ucToolDgv();
            this.ucDgvSolucion = new tadLayUI.userControl.ucDgv();
            this.grEjeTrazado.SuspendLayout();
            this.grPerfil.SuspendLayout();
            this.grObraLineal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvSolucion)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPerfilCivil
            // 
            this.btnPerfilCivil.Location = new System.Drawing.Point(6, 20);
            this.btnPerfilCivil.Name = "btnPerfilCivil";
            this.btnPerfilCivil.Size = new System.Drawing.Size(159, 23);
            this.btnPerfilCivil.TabIndex = 3;
            this.btnPerfilCivil.Text = "btnPerfilCivil";
            this.btnPerfilCivil.UseVisualStyleBackColor = true;
            this.btnPerfilCivil.Click += new System.EventHandler(this.btnPerfil_Click);
            // 
            // btnObraLineal
            // 
            this.btnObraLineal.Location = new System.Drawing.Point(6, 18);
            this.btnObraLineal.Name = "btnObraLineal";
            this.btnObraLineal.Size = new System.Drawing.Size(162, 23);
            this.btnObraLineal.TabIndex = 4;
            this.btnObraLineal.Text = "btnObraLineal";
            this.btnObraLineal.UseVisualStyleBackColor = true;
            this.btnObraLineal.Click += new System.EventHandler(this.btnObraLineal_Click);
            // 
            // btnExportarObraLineal
            // 
            this.btnExportarObraLineal.Location = new System.Drawing.Point(6, 44);
            this.btnExportarObraLineal.Name = "btnExportarObraLineal";
            this.btnExportarObraLineal.Size = new System.Drawing.Size(162, 23);
            this.btnExportarObraLineal.TabIndex = 5;
            this.btnExportarObraLineal.Text = "btnExportarObraLineal";
            this.btnExportarObraLineal.UseVisualStyleBackColor = true;
            this.btnExportarObraLineal.Click += new System.EventHandler(this.btnExportarObraLineal_Click);
            // 
            // btnInformeObraLineal
            // 
            this.btnInformeObraLineal.Location = new System.Drawing.Point(6, 72);
            this.btnInformeObraLineal.Name = "btnInformeObraLineal";
            this.btnInformeObraLineal.Size = new System.Drawing.Size(162, 23);
            this.btnInformeObraLineal.TabIndex = 5;
            this.btnInformeObraLineal.Text = "btnInformeObraLineal";
            this.btnInformeObraLineal.UseVisualStyleBackColor = true;
            this.btnInformeObraLineal.Click += new System.EventHandler(this.btnInformeObraLineal_Click);
            // 
            // btnEjeCrear
            // 
            this.btnEjeCrear.Location = new System.Drawing.Point(9, 18);
            this.btnEjeCrear.Name = "btnEjeCrear";
            this.btnEjeCrear.Size = new System.Drawing.Size(148, 23);
            this.btnEjeCrear.TabIndex = 6;
            this.btnEjeCrear.Text = "btnEjeCrear";
            this.btnEjeCrear.UseVisualStyleBackColor = true;
            this.btnEjeCrear.Click += new System.EventHandler(this.btnEjeNew_Click);
            // 
            // btnEjeRotular
            // 
            this.btnEjeRotular.Location = new System.Drawing.Point(9, 44);
            this.btnEjeRotular.Name = "btnEjeRotular";
            this.btnEjeRotular.Size = new System.Drawing.Size(148, 23);
            this.btnEjeRotular.TabIndex = 7;
            this.btnEjeRotular.Text = "btnEjeRotular";
            this.btnEjeRotular.UseVisualStyleBackColor = true;
            this.btnEjeRotular.Click += new System.EventHandler(this.btnEjeTrazadoRotular_Click);
            // 
            // grEjeTrazado
            // 
            this.grEjeTrazado.Controls.Add(this.btnEjeInforme);
            this.grEjeTrazado.Controls.Add(this.btnEjeCrear);
            this.grEjeTrazado.Controls.Add(this.btnEjeRotular);
            this.grEjeTrazado.Location = new System.Drawing.Point(8, 391);
            this.grEjeTrazado.Name = "grEjeTrazado";
            this.grEjeTrazado.Size = new System.Drawing.Size(163, 100);
            this.grEjeTrazado.TabIndex = 8;
            this.grEjeTrazado.TabStop = false;
            this.grEjeTrazado.Text = "grEjeTrazado";
            // 
            // btnEjeInforme
            // 
            this.btnEjeInforme.Location = new System.Drawing.Point(9, 72);
            this.btnEjeInforme.Name = "btnEjeInforme";
            this.btnEjeInforme.Size = new System.Drawing.Size(148, 23);
            this.btnEjeInforme.TabIndex = 8;
            this.btnEjeInforme.Text = "btnEjeInforme";
            this.btnEjeInforme.UseVisualStyleBackColor = true;
            this.btnEjeInforme.Click += new System.EventHandler(this.btnInforme_Click);
            // 
            // grPerfil
            // 
            this.grPerfil.Controls.Add(this.btnExportaLong);
            this.grPerfil.Controls.Add(this.btnPerfilCivil);
            this.grPerfil.Controls.Add(this.btnPerfilInforme);
            this.grPerfil.Location = new System.Drawing.Point(178, 393);
            this.grPerfil.Name = "grPerfil";
            this.grPerfil.Size = new System.Drawing.Size(171, 98);
            this.grPerfil.TabIndex = 9;
            this.grPerfil.TabStop = false;
            this.grPerfil.Text = "grPerfil";
            // 
            // btnExportaLong
            // 
            this.btnExportaLong.Location = new System.Drawing.Point(6, 70);
            this.btnExportaLong.Name = "btnExportaLong";
            this.btnExportaLong.Size = new System.Drawing.Size(157, 23);
            this.btnExportaLong.TabIndex = 6;
            this.btnExportaLong.Text = "Exportar Ejes";
            this.btnExportaLong.UseVisualStyleBackColor = true;
            this.btnExportaLong.Click += new System.EventHandler(this.btnExportaLong_Click);
            // 
            // btnPerfilInforme
            // 
            this.btnPerfilInforme.Location = new System.Drawing.Point(6, 46);
            this.btnPerfilInforme.Name = "btnPerfilInforme";
            this.btnPerfilInforme.Size = new System.Drawing.Size(157, 23);
            this.btnPerfilInforme.TabIndex = 5;
            this.btnPerfilInforme.Text = "btnPerfilInforme";
            this.btnPerfilInforme.UseVisualStyleBackColor = true;
            this.btnPerfilInforme.Click += new System.EventHandler(this.btnPerfilInforme_Click);
            // 
            // grObraLineal
            // 
            this.grObraLineal.Controls.Add(this.btnObraLineal);
            this.grObraLineal.Controls.Add(this.btnExportarObraLineal);
            this.grObraLineal.Controls.Add(this.btnInformeObraLineal);
            this.grObraLineal.Location = new System.Drawing.Point(355, 395);
            this.grObraLineal.Name = "grObraLineal";
            this.grObraLineal.Size = new System.Drawing.Size(174, 100);
            this.grObraLineal.TabIndex = 10;
            this.grObraLineal.TabStop = false;
            this.grObraLineal.Text = "grObraLineal";
            // 
            // ucToolDgv1
            // 
            this.ucToolDgv1.isEditEnable = false;
            this.ucToolDgv1.isEraseAllEnable = false;
            this.ucToolDgv1.isEraseEnable = true;
            this.ucToolDgv1.isNewEnable = false;
            this.ucToolDgv1.Location = new System.Drawing.Point(8, 14);
            this.ucToolDgv1.Name = "ucToolDgv1";
            this.ucToolDgv1.Size = new System.Drawing.Size(400, 25);
            this.ucToolDgv1.TabIndex = 1;
            // 
            // ucDgvSolucion
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Snow;
            this.ucDgvSolucion.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ucDgvSolucion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucDgvSolucion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ucDgvSolucion.Location = new System.Drawing.Point(8, 43);
            this.ucDgvSolucion.Name = "ucDgvSolucion";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDgvSolucion.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.ucDgvSolucion.Size = new System.Drawing.Size(521, 340);
            this.ucDgvSolucion.TabIndex = 0;
            this.ucDgvSolucion.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ucDgvSolucion_CellClick);
            // 
            // ucDgvSoluciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grObraLineal);
            this.Controls.Add(this.grPerfil);
            this.Controls.Add(this.grEjeTrazado);
            this.Controls.Add(this.ucToolDgv1);
            this.Controls.Add(this.ucDgvSolucion);
            this.Name = "ucDgvSoluciones";
            this.Size = new System.Drawing.Size(547, 527);
            this.grEjeTrazado.ResumeLayout(false);
            this.grPerfil.ResumeLayout(false);
            this.grObraLineal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvSolucion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public ucDgv ucDgvSolucion;
        private ucToolDgv ucToolDgv1;
        private System.Windows.Forms.Button btnPerfilCivil;
        private System.Windows.Forms.Button btnObraLineal;
        private System.Windows.Forms.Button btnExportarObraLineal; 
        private System.Windows.Forms.Button btnInformeObraLineal; 
        private System.Windows.Forms.Button btnEjeCrear;
        private System.Windows.Forms.Button btnEjeRotular;
        private System.Windows.Forms.GroupBox grEjeTrazado;
        private System.Windows.Forms.Button btnEjeInforme;
        private System.Windows.Forms.GroupBox grPerfil;
        private System.Windows.Forms.GroupBox grObraLineal;
        private System.Windows.Forms.Button btnPerfilInforme;
        private System.Windows.Forms.Button btnExportaLong;
    }
}
