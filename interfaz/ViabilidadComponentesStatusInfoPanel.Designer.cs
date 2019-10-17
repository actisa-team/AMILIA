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
            // TrazaViabilidadInfo
            // 
            this.ClientSize = new System.Drawing.Size(962, 739);
            this.Controls.Add(this.trazaViablidadDataGridView);
            this.Name = "TrazaViabilidadInfo";
            ((System.ComponentModel.ISupportInitialize)(this.trazaViablidadDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView trazaViablidadDataGridView;
    }
}