namespace tadLayUI.userControl
{
    partial class ucWinColor
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
            this.lbl = new System.Windows.Forms.Label();
            this.panColor = new tadLayUI.userControl.panelNex();
            this.SuspendLayout();
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(4, 4);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(17, 13);
            this.lbl.TabIndex = 0;
            this.lbl.Text = "lbl";
            // 
            // panColor
            // 
            this.panColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.panColor.BackColor = System.Drawing.Color.Transparent;
            this.panColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panColor.isObligatorio = false;
            this.panColor.Location = new System.Drawing.Point(102, 0);
            this.panColor.Name = "panColor";
            this.panColor.Size = new System.Drawing.Size(100, 20);
            this.panColor.TabIndex = 1;
            // 
            // ucWinColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panColor);
            this.Controls.Add(this.lbl);
            this.Name = "ucWinColor";
            this.Size = new System.Drawing.Size(205, 20);
            this.Load += new System.EventHandler(this.ucWinColor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl;
        private panelNex panColor;
    }
}
