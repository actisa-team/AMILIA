namespace tadLayUI.adminProyecto
{
    partial class frmPtoIni
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
            this.ucPtoIni = new tadLayUI.userControl.ucPtoIniFin();
            this.SuspendLayout();
            // 
            // ucPtoIni
            // 
            this.ucPtoIni.Location = new System.Drawing.Point(6, 12);
            this.ucPtoIni.Name = "ucPtoIni";
            this.ucPtoIni.Size = new System.Drawing.Size(270, 512);
            this.ucPtoIni.TabIndex = 0;
            this.ucPtoIni.Load += new System.EventHandler(this.ucPtoIni_Load);
            // 
            // frmPtoIni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 526);
            this.Controls.Add(this.ucPtoIni);
            this.Name = "frmPtoIni";
            this.Text = "frmPtoIniFin";
            this.ResumeLayout(false);

        }

        #endregion

        private userControl.ucPtoIniFin ucPtoIni;
    }
}