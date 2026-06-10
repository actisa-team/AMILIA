namespace tadLayUI.adminProyecto
{
    partial class frmEjeVisibilidad
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
            this.btnEjeVisibilidadUsuario = new System.Windows.Forms.Button();
            this.btnEjeVisibilidadAutomatico = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ucDistancia = new tadLayUI.userControl.ucLblTxt();
            this.btnCorredores = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEjeVisibilidadUsuario
            // 
            this.btnEjeVisibilidadUsuario.Location = new System.Drawing.Point(11, 88);
            this.btnEjeVisibilidadUsuario.Name = "btnEjeVisibilidadUsuario";
            this.btnEjeVisibilidadUsuario.Size = new System.Drawing.Size(437, 23);
            this.btnEjeVisibilidadUsuario.TabIndex = 3;
            this.btnEjeVisibilidadUsuario.Text = "btnEjeVisibilidadUsuario";
            this.btnEjeVisibilidadUsuario.UseVisualStyleBackColor = true;
            this.btnEjeVisibilidadUsuario.Click += new System.EventHandler(this.btnEjeVisibilidadUsuario_Click);
            // 
            // btnEjeVisibilidadAutomatico
            // 
            this.btnEjeVisibilidadAutomatico.Location = new System.Drawing.Point(11, 47);
            this.btnEjeVisibilidadAutomatico.Name = "btnEjeVisibilidadAutomatico";
            this.btnEjeVisibilidadAutomatico.Size = new System.Drawing.Size(437, 23);
            this.btnEjeVisibilidadAutomatico.TabIndex = 2;
            this.btnEjeVisibilidadAutomatico.Text = "btnEjeVisibilidadAutomatico";
            this.btnEjeVisibilidadAutomatico.UseVisualStyleBackColor = true;
            this.btnEjeVisibilidadAutomatico.Click += new System.EventHandler(this.btnEjeVisibilidadAutomatico_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucDistancia);
            this.groupBox1.Controls.Add(this.btnCorredores);
            this.groupBox1.Controls.Add(this.btnEjeVisibilidadUsuario);
            this.groupBox1.Controls.Add(this.btnEjeVisibilidadAutomatico);
            this.groupBox1.Location = new System.Drawing.Point(12, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 194);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // ucDistancia
            // 
            this.ucDistancia.ancho = 75;
            this.ucDistancia.isEntero = false;
            this.ucDistancia.isNegativo = false;
            this.ucDistancia.isObligatorio = true;
            this.ucDistancia.isSimboloDecimalPunto = true;
            this.ucDistancia.location = new System.Drawing.Point(130, 0);
            this.ucDistancia.Location = new System.Drawing.Point(11, 127);
            this.ucDistancia.lonMax = 32767;
            this.ucDistancia.Name = "ucDistancia";
            this.ucDistancia.Size = new System.Drawing.Size(205, 20);
            this.ucDistancia.TabIndex = 5;
            this.ucDistancia.uiLbl = "ucDistancia";
            this.ucDistancia.uitxt = "10";
            this.ucDistancia.valorDoubleNull = 10D;
            this.ucDistancia.valorMaximo = 1000000000D;
            this.ucDistancia.valorMinimo = 0D;
            // 
            // btnCorredores
            // 
            this.btnCorredores.Location = new System.Drawing.Point(222, 125);
            this.btnCorredores.Name = "btnCorredores";
            this.btnCorredores.Size = new System.Drawing.Size(226, 23);
            this.btnCorredores.TabIndex = 4;
            this.btnCorredores.Text = "btnCorredores";
            this.btnCorredores.UseVisualStyleBackColor = true;
            this.btnCorredores.Click += new System.EventHandler(this.btnCorredores_Click);
            // 
            // frmEjeVisibilidad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 462);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmEjeVisibilidad";
            this.Text = "frmEjeVisibilidad";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEjeVisibilidadUsuario;
        private System.Windows.Forms.Button btnEjeVisibilidadAutomatico;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCorredores;
        private userControl.ucLblTxt ucDistancia;
    }
}