namespace tadLayUI.userControl
{
    partial class ucToolDgv
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
            this.lnkNew = new System.Windows.Forms.ToolStripLabel();
            this.lnkEdit = new System.Windows.Forms.ToolStripLabel();
            this.lnkErase = new System.Windows.Forms.ToolStripLabel();
            this.lnkEraseAll = new System.Windows.Forms.ToolStripLabel();
            this.tool.SuspendLayout();
            this.SuspendLayout();
            // 
            // tool
            // 
            this.tool.BackColor = System.Drawing.Color.Transparent;
            this.tool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lnkNew,
            this.lnkEdit,
            this.lnkErase,
            this.lnkEraseAll});
            this.tool.Location = new System.Drawing.Point(0, 0);
            this.tool.Name = "tool";
            this.tool.Size = new System.Drawing.Size(400, 25);
            this.tool.TabIndex = 0;
            this.tool.Text = "toolItem";
            // 
            // lnkNew
            // 
            this.lnkNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.lnkNew.IsLink = true;
            this.lnkNew.Name = "lnkNew";
            this.lnkNew.Size = new System.Drawing.Size(47, 22);
            this.lnkNew.Text = "lnkNew";
            // 
            // lnkEdit
            // 
            this.lnkEdit.IsLink = true;
            this.lnkEdit.Name = "lnkEdit";
            this.lnkEdit.Size = new System.Drawing.Size(43, 22);
            this.lnkEdit.Text = "lnkEdit";
            // 
            // lnkErase
            // 
            this.lnkErase.IsLink = true;
            this.lnkErase.Name = "lnkErase";
            this.lnkErase.Size = new System.Drawing.Size(50, 22);
            this.lnkErase.Text = "lnkErase";
            // 
            // lnkEraseAll
            // 
            this.lnkEraseAll.IsLink = true;
            this.lnkEraseAll.Name = "lnkEraseAll";
            this.lnkEraseAll.Size = new System.Drawing.Size(64, 22);
            this.lnkEraseAll.Text = "lnkEraseAll";
            // 
            // ucToolDgv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tool);
            this.Name = "ucToolDgv";
            this.Size = new System.Drawing.Size(400, 25);
            this.Load += new System.EventHandler(this.ucToolDgv_Load);
            this.tool.ResumeLayout(false);
            this.tool.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tool;
        public System.Windows.Forms.ToolStripLabel lnkEdit;
        public System.Windows.Forms.ToolStripLabel lnkNew;
        public System.Windows.Forms.ToolStripLabel lnkErase;
        public System.Windows.Forms.ToolStripLabel lnkEraseAll;
    }
}
