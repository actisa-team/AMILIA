namespace tadLayUI
{
    partial class frmSimplificarPolilineas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSimplificarPolilineas));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelPol = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ucCmbLayersTodas1 = new tadLayUI.userControl.ucCmbLayersTodas();
            this.btnSimplPoly = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelPol);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "gbOrigDatos";
            // 
            // btnSelPol
            // 
            this.btnSelPol.Location = new System.Drawing.Point(30, 28);
            this.btnSelPol.Name = "btnSelPol";
            this.btnSelPol.Size = new System.Drawing.Size(309, 23);
            this.btnSelPol.TabIndex = 0;
            this.btnSelPol.Text = "btnSelPol";
            this.btnSelPol.UseVisualStyleBackColor = true;
            this.btnSelPol.Click += new System.EventHandler(this.btnSelPol_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ucCmbLayersTodas1);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(361, 81);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "gbDestDatos";
            // 
            // ucCmbLayersTodas1
            // 
            this.ucCmbLayersTodas1.ancho = 147;
            this.ucCmbLayersTodas1.isObligatorio = true;
            this.ucCmbLayersTodas1.location = new System.Drawing.Point(162, 0);
            this.ucCmbLayersTodas1.Location = new System.Drawing.Point(30, 29);
            this.ucCmbLayersTodas1.Name = "ucCmbLayersTodas1";
            this.ucCmbLayersTodas1.Size = new System.Drawing.Size(309, 21);
            this.ucCmbLayersTodas1.TabIndex = 1;
            this.ucCmbLayersTodas1.uiLbl = "ucCmbLayersTodas1";
            // 
            // btnSimplPoly
            // 
            this.btnSimplPoly.Location = new System.Drawing.Point(12, 220);
            this.btnSimplPoly.Name = "btnSimplPoly";
            this.btnSimplPoly.Size = new System.Drawing.Size(361, 23);
            this.btnSimplPoly.TabIndex = 5;
            this.btnSimplPoly.Text = "btnSimplPoly";
            this.btnSimplPoly.UseVisualStyleBackColor = true;
            this.btnSimplPoly.Click += new System.EventHandler(this.btSimplPoly_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 270);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(389, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // frmSimplificarPolilineas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 292);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnSimplPoly);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSimplificarPolilineas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSimplificarPolilineas";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSimplificarPolilineas_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private userControl.ucCmbLayersTodas ucCmbLayersTodas1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelPol;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSimplPoly;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}