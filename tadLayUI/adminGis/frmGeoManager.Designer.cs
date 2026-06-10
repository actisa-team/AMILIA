namespace tadLayUI.adminGis
{
    partial class frmGeoManager
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

        private System.Windows.Forms.Button btnLink;


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLink = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(272, 520);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(227, 24);
            this.btnLink.TabIndex = 35;
            this.btnLink.Text = "btnLink";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // frmGeoManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(516, 550);
            this.Controls.Add(this.btnLink);
            this.Name = "frmGeoManager";
            this.Controls.SetChildIndex(this.btnLink, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion



    }
}