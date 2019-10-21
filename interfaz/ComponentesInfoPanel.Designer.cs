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
            this.label1 = new System.Windows.Forms.Label();
            this.errorValidationPanel = new System.Windows.Forms.Panel();
            this.errorValidacionRectasLabel = new System.Windows.Forms.Label();
            this.errorValidacionCurvasLabel = new System.Windows.Forms.Label();
            this.successValidationPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.componentesInfoDataGridView)).BeginInit();
            this.errorValidationPanel.SuspendLayout();
            this.successValidationPanel.SuspendLayout();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 600);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Estado de la verificación de los componentes";
            // 
            // errorValidationPanel
            // 
            this.errorValidationPanel.BackColor = System.Drawing.Color.White;
            this.errorValidationPanel.Controls.Add(this.errorValidacionRectasLabel);
            this.errorValidationPanel.Controls.Add(this.errorValidacionCurvasLabel);
            this.errorValidationPanel.Location = new System.Drawing.Point(0, 0);
            this.errorValidationPanel.Name = "errorValidationPanel";
            this.errorValidationPanel.Size = new System.Drawing.Size(372, 66);
            this.errorValidationPanel.TabIndex = 2;
            this.errorValidationPanel.Visible = false;
            // 
            // errorValidacionRectasLabel
            // 
            this.errorValidacionRectasLabel.AutoSize = true;
            this.errorValidacionRectasLabel.ForeColor = System.Drawing.Color.Red;
            this.errorValidacionRectasLabel.Location = new System.Drawing.Point(3, 21);
            this.errorValidacionRectasLabel.Name = "errorValidacionRectasLabel";
            this.errorValidacionRectasLabel.Size = new System.Drawing.Size(142, 13);
            this.errorValidacionRectasLabel.TabIndex = 1;
            this.errorValidacionRectasLabel.Text = "Error de validacion de rectas";
            this.errorValidacionRectasLabel.Visible = false;
            // 
            // errorValidacionCurvasLabel
            // 
            this.errorValidacionCurvasLabel.AutoSize = true;
            this.errorValidacionCurvasLabel.ForeColor = System.Drawing.Color.Red;
            this.errorValidacionCurvasLabel.Location = new System.Drawing.Point(3, 3);
            this.errorValidacionCurvasLabel.Name = "errorValidacionCurvasLabel";
            this.errorValidacionCurvasLabel.Size = new System.Drawing.Size(145, 13);
            this.errorValidacionCurvasLabel.TabIndex = 0;
            this.errorValidacionCurvasLabel.Text = "Error de validacion de curvas";
            this.errorValidacionCurvasLabel.Visible = false;
            // 
            // successValidationPanel
            // 
            this.successValidationPanel.BackColor = System.Drawing.Color.White;
            this.successValidationPanel.Controls.Add(this.label2);
            this.successValidationPanel.Controls.Add(this.errorValidationPanel);
            this.successValidationPanel.Location = new System.Drawing.Point(16, 616);
            this.successValidationPanel.Name = "successValidationPanel";
            this.successValidationPanel.Size = new System.Drawing.Size(372, 66);
            this.successValidationPanel.TabIndex = 3;
            this.successValidationPanel.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Validacion de componentes correcta";
            // 
            // ComponentesInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1290, 694);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.componentesInfoDataGridView);
            this.Controls.Add(this.successValidationPanel);
            this.Name = "ComponentesInfoPanel";
            this.Text = "Informe de componentes > etapa 1";
            ((System.ComponentModel.ISupportInitialize)(this.componentesInfoDataGridView)).EndInit();
            this.errorValidationPanel.ResumeLayout(false);
            this.errorValidationPanel.PerformLayout();
            this.successValidationPanel.ResumeLayout(false);
            this.successValidationPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView componentesInfoDataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel errorValidationPanel;
        private System.Windows.Forms.Panel successValidationPanel;
        private System.Windows.Forms.Label errorValidacionRectasLabel;
        private System.Windows.Forms.Label errorValidacionCurvasLabel;
        private System.Windows.Forms.Label label2;
    }
}