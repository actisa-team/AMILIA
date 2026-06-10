namespace tadLayUI.userControl
{
    partial class ucDgvSolucionesValorar
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
            this.ucDgvSoluciones = new tadLayUI.userControl.ucDgv();
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvSoluciones)).BeginInit();
            this.SuspendLayout();
            // 
            // ucDgvSoluciones
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Snow;
            this.ucDgvSoluciones.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ucDgvSoluciones.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucDgvSoluciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ucDgvSoluciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDgvSoluciones.Location = new System.Drawing.Point(0, 0);
            this.ucDgvSoluciones.Name = "ucDgvSoluciones";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDgvSoluciones.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.ucDgvSoluciones.Size = new System.Drawing.Size(569, 282);
            this.ucDgvSoluciones.TabIndex = 0;
            this.ucDgvSoluciones.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ucDgvSoluciones_CellClick);
            // 
            // ucDgvSolucionesValorar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucDgvSoluciones);
            this.Name = "ucDgvSolucionesValorar";
            this.Size = new System.Drawing.Size(569, 282);
            this.Load += new System.EventHandler(this.ucDgvSolucionesValorar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvSoluciones)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ucDgv ucDgvSoluciones;
    }
}
