namespace interfaz {
    using MaterialSkin.Controls;

    partial class ViabilidadComponentesStatusInfoPanel : MaterialForm {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent() {
            this.trazaViablidadDataGridView = new System.Windows.Forms.DataGridView();
            this.ejecutarHastaFinalizarButton = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialFlatButton1 = new MaterialSkin.Controls.MaterialFlatButton();
            ((System.ComponentModel.ISupportInitialize)(this.trazaViablidadDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // trazaViablidadDataGridView
            // 
            this.trazaViablidadDataGridView.AllowUserToAddRows = false;
            this.trazaViablidadDataGridView.AllowUserToDeleteRows = false;
            this.trazaViablidadDataGridView.AllowUserToResizeColumns = false;
            this.trazaViablidadDataGridView.AllowUserToResizeRows = false;
            this.trazaViablidadDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.trazaViablidadDataGridView.Location = new System.Drawing.Point(12, 76);
            this.trazaViablidadDataGridView.Name = "trazaViablidadDataGridView";
            this.trazaViablidadDataGridView.ReadOnly = true;
            this.trazaViablidadDataGridView.Size = new System.Drawing.Size(938, 651);
            this.trazaViablidadDataGridView.TabIndex = 0;
            // 
            // ejecutarHastaFinalizarButton
            // 
            this.ejecutarHastaFinalizarButton.AutoSize = true;
            this.ejecutarHastaFinalizarButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ejecutarHastaFinalizarButton.Depth = 0;
            this.ejecutarHastaFinalizarButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ejecutarHastaFinalizarButton.Location = new System.Drawing.Point(12, 753);
            this.ejecutarHastaFinalizarButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ejecutarHastaFinalizarButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.ejecutarHastaFinalizarButton.Name = "ejecutarHastaFinalizarButton";
            this.ejecutarHastaFinalizarButton.Primary = false;
            this.ejecutarHastaFinalizarButton.Size = new System.Drawing.Size(198, 36);
            this.ejecutarHastaFinalizarButton.TabIndex = 1;
            this.ejecutarHastaFinalizarButton.Text = "Ejecutar hasta finalizar";
            this.ejecutarHastaFinalizarButton.UseVisualStyleBackColor = true;
            // 
            // materialFlatButton1
            // 
            this.materialFlatButton1.AutoSize = true;
            this.materialFlatButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton1.Depth = 0;
            this.materialFlatButton1.Location = new System.Drawing.Point(218, 753);
            this.materialFlatButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton1.Name = "materialFlatButton1";
            this.materialFlatButton1.Primary = false;
            this.materialFlatButton1.Size = new System.Drawing.Size(173, 36);
            this.materialFlatButton1.TabIndex = 2;
            this.materialFlatButton1.Text = "Continuar depurando";
            this.materialFlatButton1.UseVisualStyleBackColor = true;
            this.materialFlatButton1.Click += new System.EventHandler(this.materialFlatButton1_Click);
            // 
            // ViabilidadComponentesStatusInfoPanel
            // 
            this.ClientSize = new System.Drawing.Size(962, 804);
            this.Controls.Add(this.materialFlatButton1);
            this.Controls.Add(this.ejecutarHastaFinalizarButton);
            this.Controls.Add(this.trazaViablidadDataGridView);
            this.Name = "ViabilidadComponentesStatusInfoPanel";
            ((System.ComponentModel.ISupportInitialize)(this.trazaViablidadDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView trazaViablidadDataGridView;
        private MaterialFlatButton ejecutarHastaFinalizarButton;
        private MaterialFlatButton materialFlatButton1;
    }
}