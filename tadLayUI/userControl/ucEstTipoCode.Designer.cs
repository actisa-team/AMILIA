namespace tadLayUI.userControl
{
    partial class ucEstTipoCode
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
            this.lblGrupo = new System.Windows.Forms.Label();
            this.cmbMaster = new System.Windows.Forms.ComboBox();
            this.cmbDetail = new System.Windows.Forms.ComboBox();
            this.lblItem = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblGrupo
            // 
            this.lblGrupo.AutoSize = true;
            this.lblGrupo.Location = new System.Drawing.Point(0, 17);
            this.lblGrupo.Name = "lblGrupo";
            this.lblGrupo.Size = new System.Drawing.Size(46, 13);
            this.lblGrupo.TabIndex = 0;
            this.lblGrupo.Text = "lblGrupo";
            // 
            // cmbMaster
            // 
            this.cmbMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbMaster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMaster.FormattingEnabled = true;
            this.cmbMaster.Location = new System.Drawing.Point(103, 13);
            this.cmbMaster.Name = "cmbMaster";
            this.cmbMaster.Size = new System.Drawing.Size(200, 21);
            this.cmbMaster.TabIndex = 1;
            this.cmbMaster.SelectedIndexChanged += new System.EventHandler(this.cmbMaster_SelectedIndexChanged);
            // 
            // cmbDetail
            // 
            this.cmbDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbDetail.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDetail.FormattingEnabled = true;
            this.cmbDetail.Location = new System.Drawing.Point(103, 52);
            this.cmbDetail.Name = "cmbDetail";
            this.cmbDetail.Size = new System.Drawing.Size(200, 21);
            this.cmbDetail.TabIndex = 2;
            // 
            // lblItem
            // 
            this.lblItem.AutoSize = true;
            this.lblItem.Location = new System.Drawing.Point(0, 60);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(37, 13);
            this.lblItem.TabIndex = 3;
            this.lblItem.Text = "lblItem";
            // 
            // ucEstTipoCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblGrupo);
            this.Controls.Add(this.lblItem);
            this.Controls.Add(this.cmbMaster);
            this.Controls.Add(this.cmbDetail);
            this.Name = "ucEstTipoCode";
            this.Size = new System.Drawing.Size(309, 85);
            this.Load += new System.EventHandler(this.ucEstTipoCode_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGrupo;
        private System.Windows.Forms.ComboBox cmbMaster;
        private System.Windows.Forms.ComboBox cmbDetail;
        private System.Windows.Forms.Label lblItem;
    }
}
