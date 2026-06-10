namespace tadLayUI.adminProyecto
{
    partial class frmSolucionManager
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
            this.ucDgvSoluciones1 = new tadLayUI.userControl.ucDgvSoluciones();
            this.SuspendLayout();
            // 
            // ucDgvSoluciones1
            // 
            this.ucDgvSoluciones1.Location = new System.Drawing.Point(1, 0);
            this.ucDgvSoluciones1.Name = "ucDgvSoluciones1";
            this.ucDgvSoluciones1.Size = new System.Drawing.Size(562, 558);
            this.ucDgvSoluciones1.TabIndex = 0;
            this.ucDgvSoluciones1.Load += new System.EventHandler(this.ucDgvSoluciones1_Load);
            // 
            // frmSolucionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 556);
            this.Controls.Add(this.ucDgvSoluciones1);
            this.Name = "frmSolucionManager";
            this.Text = "frmSolucionManager";
            this.Shown += new System.EventHandler(this.frmSolucionManager_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private userControl.ucDgvSoluciones ucDgvSoluciones1;
    }
}