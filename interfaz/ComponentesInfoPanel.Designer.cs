using MaterialSkin.Controls;

namespace interfaz {
    partial class ComponentesInfoPanel : MaterialForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.componentesInfoDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.componentesInfoDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // componentesInfoDataGridView
            // 
            this.componentesInfoDataGridView.AllowUserToAddRows = false;
            this.componentesInfoDataGridView.AllowUserToDeleteRows = false;
            this.componentesInfoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.componentesInfoDataGridView.Location = new System.Drawing.Point(12, 76);
            this.componentesInfoDataGridView.Name = "componentesInfoDataGridView";
            this.componentesInfoDataGridView.Size = new System.Drawing.Size(1266, 508);
            this.componentesInfoDataGridView.TabIndex = 0;
            // 
            // ComponentesInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1290, 596);
            this.Controls.Add(this.componentesInfoDataGridView);
            this.Name = "ComponentesInfoPanel";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.componentesInfoDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView componentesInfoDataGridView;
    }
}