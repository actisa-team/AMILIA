namespace tadLayUI
{
    partial class frmKv
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
            this.components = new System.ComponentModel.Container();
            this.dgvKv = new System.Windows.Forms.DataGridView();
            this.mnuTbItemsEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.footer = new System.Windows.Forms.StatusStrip();
            this.lblOrigenDatos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ucMenu = new tadLayUI.userControl.ucMenuNose();
            this.header = new System.Windows.Forms.ToolStrip();
            this.lblHeader = new System.Windows.Forms.ToolStripLabel();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKv)).BeginInit();
            this.mnuTbItemsEdit.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.footer.SuspendLayout();
            this.header.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvKv
            // 
            this.dgvKv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKv.Location = new System.Drawing.Point(6, 19);
            this.dgvKv.Name = "dgvKv";
            this.dgvKv.Size = new System.Drawing.Size(531, 242);
            this.dgvKv.TabIndex = 0;
            // 
            // mnuTbItemsEdit
            // 
            this.mnuTbItemsEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuItemDelete,
            this.mnuItemAdd});
            this.mnuTbItemsEdit.Name = "mnuTbItemsEdit";
            this.mnuTbItemsEdit.Size = new System.Drawing.Size(164, 48);
            // 
            // mnuItemDelete
            // 
            this.mnuItemDelete.Name = "mnuItemDelete";
            this.mnuItemDelete.Size = new System.Drawing.Size(163, 22);
            this.mnuItemDelete.Text = "Eliminar Registro";
            this.mnuItemDelete.Click += new System.EventHandler(this.mnuItemDelete_Click);
            // 
            // mnuItemAdd
            // 
            this.mnuItemAdd.Name = "mnuItemAdd";
            this.mnuItemAdd.Size = new System.Drawing.Size(163, 22);
            this.mnuItemAdd.Text = "Añadir Registro";
            this.mnuItemAdd.Click += new System.EventHandler(this.mnuItemAdd_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvKv);
            this.groupBox1.Location = new System.Drawing.Point(12, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(543, 276);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.UseCompatibleTextRendering = true;
            // 
            // footer
            // 
            this.footer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblOrigenDatos});
            this.footer.Location = new System.Drawing.Point(0, 378);
            this.footer.Name = "footer";
            this.footer.Size = new System.Drawing.Size(571, 22);
            this.footer.TabIndex = 17;
            this.footer.Text = "footer";
            // 
            // lblOrigenDatos
            // 
            this.lblOrigenDatos.Name = "lblOrigenDatos";
            this.lblOrigenDatos.Size = new System.Drawing.Size(86, 17);
            this.lblOrigenDatos.Text = "lblOrigenDatos";
            // 
            // ucMenu
            // 
            this.ucMenu.Location = new System.Drawing.Point(0, 0);
            this.ucMenu.Name = "ucMenu";
            this.ucMenu.Size = new System.Drawing.Size(571, 24);
            this.ucMenu.TabIndex = 18;
            this.ucMenu.Text = "menuKv";
            // 
            // header
            // 
            this.header.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHeader});
            this.header.Location = new System.Drawing.Point(0, 24);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(571, 25);
            this.header.TabIndex = 19;
            this.header.Text = "header";
            // 
            // lblHeader
            // 
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(58, 22);
            this.lblHeader.Text = "lblHeader";
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(169, 350);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(390, 25);
            this.ucToolDetail1.TabIndex = 20;
            // 
            // frmKv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 400);
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.header);
            this.Controls.Add(this.footer);
            this.Controls.Add(this.ucMenu);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.ucMenu;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmKv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmKv";
            this.Load += new System.EventHandler(this.frmKv_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKv)).EndInit();
            this.mnuTbItemsEdit.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.footer.ResumeLayout(false);
            this.footer.PerformLayout();
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvKv;
        private System.Windows.Forms.ContextMenuStrip mnuTbItemsEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuItemAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip footer;
        private System.Windows.Forms.ToolStripStatusLabel lblOrigenDatos;
        private userControl.ucMenuNose ucMenu;
        private System.Windows.Forms.ToolStrip header;
        private System.Windows.Forms.ToolStripLabel lblHeader;
        private userControl.ucToolDetail ucToolDetail1;
    }
}