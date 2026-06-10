namespace tadLayUI.userControl
{
    partial class ucToolDetail
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
            this.tool = new System.Windows.Forms.ToolStrip();
            this.lnkSalir = new System.Windows.Forms.ToolStripLabel();
            this.lnkCancel = new System.Windows.Forms.ToolStripLabel();
            this.lnkSave = new System.Windows.Forms.ToolStripLabel();
            this.tool.SuspendLayout();
            this.SuspendLayout();
            // 
            // tool
            // 
            this.tool.BackColor = System.Drawing.Color.Transparent;
            this.tool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lnkSalir,
            this.lnkCancel,
            this.lnkSave});
            this.tool.Location = new System.Drawing.Point(0, 0);
            this.tool.Name = "tool";
            this.tool.Size = new System.Drawing.Size(400, 25);
            this.tool.TabIndex = 0;
            this.tool.Text = "toolItem";
            // 
            // lnkSalir
            // 
            this.lnkSalir.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lnkSalir.IsLink = true;
            this.lnkSalir.Name = "lnkSalir";
            this.lnkSalir.Size = new System.Drawing.Size(45, 22);
            this.lnkSalir.Text = "lnkSalir";
            // 
            // lnkCancel
            // 
            this.lnkCancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lnkCancel.IsLink = true;
            this.lnkCancel.Name = "lnkCancel";
            this.lnkCancel.Size = new System.Drawing.Size(59, 22);
            this.lnkCancel.Text = "lnkCancel";
            // 
            // lnkSave
            // 
            this.lnkSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.lnkSave.IsLink = true;
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(47, 22);
            this.lnkSave.Text = "lnkSave";
            // 
            // ucToolDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tool);
            this.Name = "ucToolDetail";
            this.Size = new System.Drawing.Size(400, 25);
            this.Load += new System.EventHandler(this.ucToolDetail_Load);
            this.tool.ResumeLayout(false);
            this.tool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tool;
        public System.Windows.Forms.ToolStripLabel lnkCancel;
        public System.Windows.Forms.ToolStripLabel lnkSave;
        public System.Windows.Forms.ToolStripLabel lnkSalir;
    }
}
