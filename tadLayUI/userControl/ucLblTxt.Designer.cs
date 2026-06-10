namespace tadLayUI.userControl
{
    partial class ucLblTxt
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
            this.lbl = new System.Windows.Forms.Label();
            this.txt = new tadLayUI.userControl.txtNex();
            this.SuspendLayout();
            // 
            // lbl
            // 
            this.lbl.AutoSize = true;
            this.lbl.Location = new System.Drawing.Point(4, 5);
            this.lbl.Name = "lbl";
            this.lbl.Size = new System.Drawing.Size(17, 13);
            this.lbl.TabIndex = 1;
            this.lbl.Text = "lbl";
            // 
            // txt
            // 
            this.txt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.txt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt.isEntero = false;
            this.txt.isNegativo = false;
            this.txt.isObligatorio = true;
            this.txt.isSimboloDecimalPunto = true;
            this.txt.isTexto = false;
            this.txt.Location = new System.Drawing.Point(130, 0);
            this.txt.Name = "txt";
            this.txt.Size = new System.Drawing.Size(75, 20);
            this.txt.TabIndex = 2;
            this.txt.valorDouble = 0D;
            this.txt.valorMax = 0D;
            this.txt.valorMin = 0D;
            // 
            // ucLblTxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl);
            this.Controls.Add(this.txt);
            this.Name = "ucLblTxt";
            this.Size = new System.Drawing.Size(205, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl;
        private txtNex txt;
    }
}
