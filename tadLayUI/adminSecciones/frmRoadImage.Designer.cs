namespace tadLayUI.adminSecciones
{
    partial class frmRoadImage
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
            this.imgSecccion = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgSecccion)).BeginInit();
            this.SuspendLayout();
            // 
            // imgSecccion
            // 
            this.imgSecccion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgSecccion.Location = new System.Drawing.Point(0, 0);
            this.imgSecccion.Name = "imgSecccion";
            this.imgSecccion.Size = new System.Drawing.Size(904, 343);
            this.imgSecccion.TabIndex = 0;
            this.imgSecccion.TabStop = false;
            // 
            // frmRoadImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 343);
            this.Controls.Add(this.imgSecccion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmRoadImage";
            this.Text = "frmRoadUnicaGenImage";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRoadImage_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.imgSecccion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imgSecccion;
    }
}