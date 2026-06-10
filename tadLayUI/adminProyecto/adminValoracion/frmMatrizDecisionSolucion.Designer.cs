namespace tadLayUI.adminValoracion
{
    partial class frmMatrizDecisionSolucion
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grHipotesis = new System.Windows.Forms.GroupBox();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.grLstSoluciones = new System.Windows.Forms.GroupBox();
            this.ucDgvSoluciones = new tadLayUI.userControl.ucDgv();
            this.grLstCapitulos = new System.Windows.Forms.GroupBox();
            this.ucDgvCapitulos = new tadLayUI.userControl.ucDgv();
            this.grLstDetalle = new System.Windows.Forms.GroupBox();
            this.ucDgvDetalle = new tadLayUI.userControl.ucDgv();
            this.grInforme = new System.Windows.Forms.GroupBox();
            this.btnInformeDetalle = new System.Windows.Forms.Button();
            this.btnInformeResumen = new System.Windows.Forms.Button();
            this.grHipotesis.SuspendLayout();
            this.grLstSoluciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvSoluciones)).BeginInit();
            this.grLstCapitulos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvCapitulos)).BeginInit();
            this.grLstDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvDetalle)).BeginInit();
            this.grInforme.SuspendLayout();
            this.SuspendLayout();
            // 
            // grHipotesis
            // 
            this.grHipotesis.Controls.Add(this.ucNombre);
            this.grHipotesis.Location = new System.Drawing.Point(12, 9);
            this.grHipotesis.Name = "grHipotesis";
            this.grHipotesis.Size = new System.Drawing.Size(566, 56);
            this.grHipotesis.TabIndex = 0;
            this.grHipotesis.TabStop = false;
            this.grHipotesis.Text = "grHipotesis";
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 410;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(130, 0);
            this.ucNombre.Location = new System.Drawing.Point(12, 19);
            this.ucNombre.lonMax = 32767;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(543, 20);
            this.ucNombre.TabIndex = 0;
            this.ucNombre.uiLbl = "ucNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // grLstSoluciones
            // 
            this.grLstSoluciones.Controls.Add(this.ucDgvSoluciones);
            this.grLstSoluciones.Location = new System.Drawing.Point(12, 69);
            this.grLstSoluciones.Name = "grLstSoluciones";
            this.grLstSoluciones.Size = new System.Drawing.Size(566, 160);
            this.grLstSoluciones.TabIndex = 1;
            this.grLstSoluciones.TabStop = false;
            this.grLstSoluciones.Text = "grLstSoluciones";
            // 
            // ucDgvSoluciones
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Snow;
            this.ucDgvSoluciones.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ucDgvSoluciones.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucDgvSoluciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ucDgvSoluciones.Location = new System.Drawing.Point(14, 27);
            this.ucDgvSoluciones.Name = "ucDgvSoluciones";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDgvSoluciones.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.ucDgvSoluciones.Size = new System.Drawing.Size(541, 120);
            this.ucDgvSoluciones.TabIndex = 0;
            this.ucDgvSoluciones.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ucDgvSoluciones_CellMouseClick);
            // 
            // grLstCapitulos
            // 
            this.grLstCapitulos.Controls.Add(this.ucDgvCapitulos);
            this.grLstCapitulos.Location = new System.Drawing.Point(12, 232);
            this.grLstCapitulos.Name = "grLstCapitulos";
            this.grLstCapitulos.Size = new System.Drawing.Size(566, 156);
            this.grLstCapitulos.TabIndex = 2;
            this.grLstCapitulos.TabStop = false;
            this.grLstCapitulos.Text = "grLstCapitulos";
            // 
            // ucDgvCapitulos
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Snow;
            this.ucDgvCapitulos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.ucDgvCapitulos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucDgvCapitulos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ucDgvCapitulos.Location = new System.Drawing.Point(14, 22);
            this.ucDgvCapitulos.Name = "ucDgvCapitulos";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDgvCapitulos.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.ucDgvCapitulos.Size = new System.Drawing.Size(541, 120);
            this.ucDgvCapitulos.TabIndex = 0;
            this.ucDgvCapitulos.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ucDgvCapitulos_CellMouseClick);
            // 
            // grLstDetalle
            // 
            this.grLstDetalle.Controls.Add(this.ucDgvDetalle);
            this.grLstDetalle.Location = new System.Drawing.Point(12, 398);
            this.grLstDetalle.Name = "grLstDetalle";
            this.grLstDetalle.Size = new System.Drawing.Size(566, 162);
            this.grLstDetalle.TabIndex = 3;
            this.grLstDetalle.TabStop = false;
            this.grLstDetalle.Text = "grLstDetalle";
            // 
            // ucDgvDetalle
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.Snow;
            this.ucDgvDetalle.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.ucDgvDetalle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucDgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ucDgvDetalle.Location = new System.Drawing.Point(14, 25);
            this.ucDgvDetalle.Name = "ucDgvDetalle";
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDgvDetalle.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.ucDgvDetalle.Size = new System.Drawing.Size(541, 121);
            this.ucDgvDetalle.TabIndex = 0;
            // 
            // grInforme
            // 
            this.grInforme.Controls.Add(this.btnInformeDetalle);
            this.grInforme.Controls.Add(this.btnInformeResumen);
            this.grInforme.Location = new System.Drawing.Point(12, 561);
            this.grInforme.Name = "grInforme";
            this.grInforme.Size = new System.Drawing.Size(566, 85);
            this.grInforme.TabIndex = 4;
            this.grInforme.TabStop = false;
            this.grInforme.Text = "grInforme";
            // 
            // btnInformeDetalle
            // 
            this.btnInformeDetalle.Location = new System.Drawing.Point(12, 54);
            this.btnInformeDetalle.Name = "btnInformeDetalle";
            this.btnInformeDetalle.Size = new System.Drawing.Size(543, 23);
            this.btnInformeDetalle.TabIndex = 1;
            this.btnInformeDetalle.Text = "btnInformeDetalle";
            this.btnInformeDetalle.UseVisualStyleBackColor = true;
            this.btnInformeDetalle.Click += new System.EventHandler(this.btnInformeDetalle_Click);
            // 
            // btnInformeResumen
            // 
            this.btnInformeResumen.Location = new System.Drawing.Point(12, 19);
            this.btnInformeResumen.Name = "btnInformeResumen";
            this.btnInformeResumen.Size = new System.Drawing.Size(543, 23);
            this.btnInformeResumen.TabIndex = 0;
            this.btnInformeResumen.Text = "btnInformeResumen";
            this.btnInformeResumen.UseVisualStyleBackColor = true;
            this.btnInformeResumen.Click += new System.EventHandler(this.btnInformeResumen_Click);
            // 
            // frmMatrizDecisionSolucion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 647);
            this.Controls.Add(this.grInforme);
            this.Controls.Add(this.grLstDetalle);
            this.Controls.Add(this.grLstCapitulos);
            this.Controls.Add(this.grLstSoluciones);
            this.Controls.Add(this.grHipotesis);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMatrizDecisionSolucion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMatrizDecisionSolucion";
            this.Shown += new System.EventHandler(this.frmMatrizDecisionSolucion_Shown);
            this.grHipotesis.ResumeLayout(false);
            this.grLstSoluciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvSoluciones)).EndInit();
            this.grLstCapitulos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvCapitulos)).EndInit();
            this.grLstDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvDetalle)).EndInit();
            this.grInforme.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grHipotesis;
        private System.Windows.Forms.GroupBox grLstSoluciones;
        private userControl.ucDgv ucDgvSoluciones;
        private userControl.ucLblTxt ucNombre;
        private System.Windows.Forms.GroupBox grLstCapitulos;
        private userControl.ucDgv ucDgvCapitulos;
        private System.Windows.Forms.GroupBox grLstDetalle;
        private userControl.ucDgv ucDgvDetalle;
        private System.Windows.Forms.GroupBox grInforme;
        private System.Windows.Forms.Button btnInformeDetalle;
        private System.Windows.Forms.Button btnInformeResumen;
    }
}