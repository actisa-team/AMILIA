namespace tadLayUI.userControl
{
    partial class ucTreeGis
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
            this.treeGis = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeGis
            // 
            this.treeGis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGis.Location = new System.Drawing.Point(0, 0);
            this.treeGis.Name = "treeGis";
            this.treeGis.Size = new System.Drawing.Size(315, 642);
            this.treeGis.TabIndex = 0;
            this.treeGis.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeGis_NodeMouseClick);
            // 
            // ucTreeGis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeGis);
            this.Name = "ucTreeGis";
            this.Size = new System.Drawing.Size(315, 642);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeGis;
    }
}
