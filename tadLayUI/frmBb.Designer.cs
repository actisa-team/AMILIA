namespace tadLayUI
{
    partial class frmBb
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBb));
            this.ucMenu = new tadLayUI.userControl.ucMenuNose();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblFooter = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrecios = new System.Windows.Forms.ToolStripButton();
            this.btnGis = new System.Windows.Forms.ToolStripButton();
            this.btnMacroPrecios = new System.Windows.Forms.ToolStripButton();
            this.btnSec = new System.Windows.Forms.ToolStripButton();
            this.btnapro = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucMenu
            // 
            resources.ApplyResources(this.ucMenu, "ucMenu");
            this.ucMenu.Name = "ucMenu";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFooter});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // lblFooter
            // 
            this.lblFooter.Name = "lblFooter";
            resources.ApplyResources(this.lblFooter, "lblFooter");
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrecios,
            this.btnGis,
            this.btnMacroPrecios,
            this.btnSec,
            this.btnapro});
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnPrecios
            // 
            this.btnPrecios.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnPrecios.Image = global::tadLayUI.Properties.Resources.btnPrecios;
            resources.ApplyResources(this.btnPrecios, "btnPrecios");
            this.btnPrecios.Name = "btnPrecios";
            this.btnPrecios.Click += new System.EventHandler(this.btnPrecios_Click);
            // 
            // btnGis
            // 
            this.btnGis.Image = global::tadLayUI.Properties.Resources.btnGis;
            resources.ApplyResources(this.btnGis, "btnGis");
            this.btnGis.Name = "btnGis";
            this.btnGis.Click += new System.EventHandler(this.btnGis_Click);
            // 
            // btnMacroPrecios
            // 
            this.btnMacroPrecios.Image = global::tadLayUI.Properties.Resources.macroPrecios;
            resources.ApplyResources(this.btnMacroPrecios, "btnMacroPrecios");
            this.btnMacroPrecios.Name = "btnMacroPrecios";
            this.btnMacroPrecios.Click += new System.EventHandler(this.btnMacroPrecios_Click);
            // 
            // btnSec
            // 
            this.btnSec.Image = global::tadLayUI.Properties.Resources.btnSec;
            resources.ApplyResources(this.btnSec, "btnSec");
            this.btnSec.Name = "btnSec";
            this.btnSec.Click += new System.EventHandler(this.btnSec_Click);
            // 
            // btnapro
            // 
            this.btnapro.Image = global::tadLayUI.Properties.Resources.adecuacion_y_mejora;
            resources.ApplyResources(this.btnapro, "btnapro");
            this.btnapro.Name = "btnapro";
            this.btnapro.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.btnapro.Click += new System.EventHandler(this.btnapro_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImage = global::tadLayUI.Properties.Resources.backTadilStr;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Name = "panel1";
            // 
            // frmBb
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.ucMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.ucMenu;
            this.MaximizeBox = false;
            this.Name = "frmBb";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBb_FormClosed);
            this.Shown += new System.EventHandler(this.frmBb_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private userControl.ucMenuNose ucMenu;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblFooter;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPrecios;
        private System.Windows.Forms.ToolStripButton btnGis;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton btnSec;
        private System.Windows.Forms.ToolStripButton btnMacroPrecios;
        private System.Windows.Forms.ToolStripButton btnapro;
    }
}