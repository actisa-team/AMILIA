namespace tadLayUI
{
    partial class frmRoad
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
            this.dgvGrupos = new System.Windows.Forms.DataGridView();
            this.dgvCarreteras = new System.Windows.Forms.DataGridView();
            this.grItems = new System.Windows.Forms.GroupBox();
            this.mnuTbItemsEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.grMaster = new System.Windows.Forms.GroupBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.header = new System.Windows.Forms.ToolStrip();
            this.lblHeader = new System.Windows.Forms.ToolStripLabel();
            this.footer = new System.Windows.Forms.StatusStrip();
            this.lblOrigenDatos = new System.Windows.Forms.ToolStripStatusLabel();
            this.ucMenu = new tadLayUI.userControl.ucMenuNose();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrupos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarreteras)).BeginInit();
            this.grItems.SuspendLayout();
            this.mnuTbItemsEdit.SuspendLayout();
            this.grMaster.SuspendLayout();
            this.header.SuspendLayout();
            this.footer.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvGrupos
            // 
            this.dgvGrupos.AllowUserToResizeColumns = false;
            this.dgvGrupos.AllowUserToResizeRows = false;
            this.dgvGrupos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrupos.Location = new System.Drawing.Point(6, 27);
            this.dgvGrupos.MultiSelect = false;
            this.dgvGrupos.Name = "dgvGrupos";
            this.dgvGrupos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvGrupos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGrupos.Size = new System.Drawing.Size(391, 84);
            this.dgvGrupos.TabIndex = 2;
            // 
            // dgvCarreteras
            // 
            this.dgvCarreteras.AllowUserToResizeColumns = false;
            this.dgvCarreteras.AllowUserToResizeRows = false;
            this.dgvCarreteras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCarreteras.Location = new System.Drawing.Point(6, 28);
            this.dgvCarreteras.MultiSelect = false;
            this.dgvCarreteras.Name = "dgvCarreteras";
            this.dgvCarreteras.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvCarreteras.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCarreteras.Size = new System.Drawing.Size(397, 397);
            this.dgvCarreteras.TabIndex = 4;
            // 
            // grItems
            // 
            this.grItems.Controls.Add(this.dgvCarreteras);
            this.grItems.Location = new System.Drawing.Point(10, 181);
            this.grItems.Name = "grItems";
            this.grItems.Size = new System.Drawing.Size(421, 442);
            this.grItems.TabIndex = 7;
            this.grItems.TabStop = false;
            this.grItems.Text = "grItems";
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
            // grMaster
            // 
            this.grMaster.Controls.Add(this.dgvGrupos);
            this.grMaster.Location = new System.Drawing.Point(10, 53);
            this.grMaster.Name = "grMaster";
            this.grMaster.Size = new System.Drawing.Size(421, 118);
            this.grMaster.TabIndex = 10;
            this.grMaster.TabStop = false;
            this.grMaster.Text = "grMaster";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(10, 633);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(420, 23);
            this.btnSelect.TabIndex = 11;
            this.btnSelect.Text = "btnRoadSelect";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // header
            // 
            this.header.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblHeader});
            this.header.Location = new System.Drawing.Point(0, 24);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(440, 25);
            this.header.TabIndex = 12;
            this.header.Text = "header";
            // 
            // lblHeader
            // 
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(58, 22);
            this.lblHeader.Text = "lblHeader";
            // 
            // footer
            // 
            this.footer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblOrigenDatos});
            this.footer.Location = new System.Drawing.Point(0, 676);
            this.footer.Name = "footer";
            this.footer.Size = new System.Drawing.Size(440, 22);
            this.footer.TabIndex = 13;
            this.footer.Text = "footer";
            // 
            // lblOrigenDatos
            // 
            this.lblOrigenDatos.Name = "lblOrigenDatos";
            this.lblOrigenDatos.Size = new System.Drawing.Size(86, 17);
            this.lblOrigenDatos.Text = "lblOrigenDatos";
            this.lblOrigenDatos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOrigenDatos.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            // 
            // ucMenu
            // 
            this.ucMenu.Location = new System.Drawing.Point(0, 0);
            this.ucMenu.Name = "ucMenu";
            this.ucMenu.Size = new System.Drawing.Size(440, 24);
            this.ucMenu.TabIndex = 14;
            this.ucMenu.Text = "ucMenuNose1";
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(38, 662);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(390, 25);
            this.ucToolDetail1.TabIndex = 15;
            // 
            // frmRoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 698);
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.footer);
            this.Controls.Add(this.header);
            this.Controls.Add(this.ucMenu);
            this.Controls.Add(this.grItems);
            this.Controls.Add(this.grMaster);
            this.Controls.Add(this.btnSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRoad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTable";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmRoad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrupos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCarreteras)).EndInit();
            this.grItems.ResumeLayout(false);
            this.mnuTbItemsEdit.ResumeLayout(false);
            this.grMaster.ResumeLayout(false);
            this.header.ResumeLayout(false);
            this.header.PerformLayout();
            this.footer.ResumeLayout(false);
            this.footer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvGrupos;
        private System.Windows.Forms.DataGridView dgvCarreteras;
        private System.Windows.Forms.GroupBox grItems;
        private System.Windows.Forms.ContextMenuStrip mnuTbItemsEdit;
        private System.Windows.Forms.ToolStripMenuItem mnuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem mnuItemAdd;
        private System.Windows.Forms.GroupBox grMaster;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ToolStrip header;
        private System.Windows.Forms.ToolStripLabel lblHeader;
        private System.Windows.Forms.StatusStrip footer;
        private System.Windows.Forms.ToolStripStatusLabel lblOrigenDatos;
        private userControl.ucMenuNose ucMenu;
        private userControl.ucToolDetail ucToolDetail1;



    }
}