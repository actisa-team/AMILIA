namespace tadLayUI
{
    partial class frmData <T>
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
            this.btnClosed = new System.Windows.Forms.Button();
            this.ucDgvData = new tadLayUI.userControl.ucDgv();
            this.btnImprimir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClosed
            // 
            this.btnClosed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClosed.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClosed.Location = new System.Drawing.Point(425, 345);
            this.btnClosed.Name = "btnClosed";
            this.btnClosed.Size = new System.Drawing.Size(75, 23);
            this.btnClosed.TabIndex = 1;
            this.btnClosed.Text = "Cerrar";
            this.btnClosed.UseVisualStyleBackColor = true;
            this.btnClosed.Click += new System.EventHandler(this.btnClosed_Click);
            // 
            // ucDgvData
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Snow;
            this.ucDgvData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ucDgvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucDgvData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ucDgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ucDgvData.Location = new System.Drawing.Point(5, 12);
            this.ucDgvData.Name = "ucDgvData";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDgvData.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.ucDgvData.Size = new System.Drawing.Size(495, 324);
            this.ucDgvData.TabIndex = 2;
            // 
            // btnImprimir
            // 
            this.btnImprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImprimir.Location = new System.Drawing.Point(13, 345);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(75, 23);
            this.btnImprimir.TabIndex = 3;
            this.btnImprimir.Text = "btnImprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // frmData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 377);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.ucDgvData);
            this.Controls.Add(this.btnClosed);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmData";
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClosed;
        private userControl.ucDgv ucDgvData;
        private System.Windows.Forms.Button btnImprimir;
    }
}