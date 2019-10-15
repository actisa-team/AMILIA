namespace interfaz {
    using MaterialSkin.Controls;

    partial class TrazaViabilidadInfo : MaterialForm {

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
            this.trazaViablidadDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.trazaViablidadDataGridView.Location = new System.Drawing.Point(27, 165);
            this.trazaViablidadDataGridView.Name = "trazaViablidadDataGridView";
            this.trazaViablidadDataGridView.Size = new System.Drawing.Size(613, 150);
            this.trazaViablidadDataGridView.TabIndex = 0;
            // 
            // TrazaViabilidadInfo
            // 
            this.ClientSize = new System.Drawing.Size(687, 491);
            this.Controls.Add(this.trazaViablidadDataGridView);
            this.Name = "TrazaViabilidadInfo";
            this.Text = "Trazas de viabilidad";
            ((System.ComponentModel.ISupportInitialize)(this.trazaViablidadDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView trazaViablidadDataGridView;
    }
}