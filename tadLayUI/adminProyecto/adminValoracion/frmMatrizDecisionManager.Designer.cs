namespace tadLayUI.adminValoracion
{
    partial class frmMatrizDecisionManager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grDgvMatrizDecision = new System.Windows.Forms.GroupBox();
            this.ucToolDgv1 = new tadLayUI.userControl.ucToolDgv();
            this.ucDgvMatriz = new tadLayUI.userControl.ucDgv();
            this.grLstSolucionesValorar = new System.Windows.Forms.GroupBox();
            this.ucDgvSolucionesValorar1 = new tadLayUI.userControl.ucDgvSolucionesValorar();
            this.btnValorarAlternativas = new System.Windows.Forms.Button();
            this.grDgvMatrizDecision.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvMatriz)).BeginInit();
            this.grLstSolucionesValorar.SuspendLayout();
            this.SuspendLayout();
            // 
            // grDgvMatrizDecision
            // 
            this.grDgvMatrizDecision.Controls.Add(this.ucToolDgv1);
            this.grDgvMatrizDecision.Controls.Add(this.ucDgvMatriz);
            this.grDgvMatrizDecision.Location = new System.Drawing.Point(7, 12);
            this.grDgvMatrizDecision.Name = "grDgvMatrizDecision";
            this.grDgvMatrizDecision.Size = new System.Drawing.Size(522, 199);
            this.grDgvMatrizDecision.TabIndex = 0;
            this.grDgvMatrizDecision.TabStop = false;
            this.grDgvMatrizDecision.Text = "grDgvMatrizDecision";
            // 
            // ucToolDgv1
            // 
            this.ucToolDgv1.isEditEnable = true;
            this.ucToolDgv1.isEraseAllEnable = false;
            this.ucToolDgv1.isEraseEnable = true;
            this.ucToolDgv1.isNewEnable = true;
            this.ucToolDgv1.Location = new System.Drawing.Point(6, 19);
            this.ucToolDgv1.Name = "ucToolDgv1";
            this.ucToolDgv1.Size = new System.Drawing.Size(511, 25);
            this.ucToolDgv1.TabIndex = 1;
            // 
            // ucDgvMatriz
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Snow;
            this.ucDgvMatriz.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ucDgvMatriz.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucDgvMatriz.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ucDgvMatriz.Location = new System.Drawing.Point(6, 50);
            this.ucDgvMatriz.Name = "ucDgvMatriz";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDgvMatriz.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.ucDgvMatriz.Size = new System.Drawing.Size(511, 138);
            this.ucDgvMatriz.TabIndex = 0;
            // 
            // grLstSolucionesValorar
            // 
            this.grLstSolucionesValorar.Controls.Add(this.ucDgvSolucionesValorar1);
            this.grLstSolucionesValorar.Location = new System.Drawing.Point(7, 213);
            this.grLstSolucionesValorar.Name = "grLstSolucionesValorar";
            this.grLstSolucionesValorar.Size = new System.Drawing.Size(522, 238);
            this.grLstSolucionesValorar.TabIndex = 1;
            this.grLstSolucionesValorar.TabStop = false;
            this.grLstSolucionesValorar.Text = "grLstSolucionesValorar";
            // 
            // ucDgvSolucionesValorar1
            // 
            this.ucDgvSolucionesValorar1.allowMultipleSeleccion = true;
            this.ucDgvSolucionesValorar1.Location = new System.Drawing.Point(6, 19);
            this.ucDgvSolucionesValorar1.Name = "ucDgvSolucionesValorar1";
            this.ucDgvSolucionesValorar1.Size = new System.Drawing.Size(511, 200);
            this.ucDgvSolucionesValorar1.TabIndex = 0;
            // 
            // btnValorarAlternativas
            // 
            this.btnValorarAlternativas.Location = new System.Drawing.Point(12, 472);
            this.btnValorarAlternativas.Name = "btnValorarAlternativas";
            this.btnValorarAlternativas.Size = new System.Drawing.Size(511, 23);
            this.btnValorarAlternativas.TabIndex = 2;
            this.btnValorarAlternativas.Text = "btnValorarAlternativas";
            this.btnValorarAlternativas.UseVisualStyleBackColor = true;
            this.btnValorarAlternativas.Click += new System.EventHandler(this.btnValorarAlternativas_Click);
            // 
            // frmMatrizDecisionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 507);
            this.Controls.Add(this.btnValorarAlternativas);
            this.Controls.Add(this.grLstSolucionesValorar);
            this.Controls.Add(this.grDgvMatrizDecision);
            this.Name = "frmMatrizDecisionManager";
            this.Text = "frmMatrizDecisionManager";
            this.Load += new System.EventHandler(this.frmMatrizDecisionManager_Load);
            this.Shown += new System.EventHandler(this.frmMatrizDecisionManager_Shown);
            this.grDgvMatrizDecision.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvMatriz)).EndInit();
            this.grLstSolucionesValorar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grDgvMatrizDecision;
        private userControl.ucDgv ucDgvMatriz;
        private userControl.ucToolDgv ucToolDgv1;
        private System.Windows.Forms.GroupBox grLstSolucionesValorar;
        private userControl.ucDgvSolucionesValorar ucDgvSolucionesValorar1;
        private System.Windows.Forms.Button btnValorarAlternativas;
    }
}