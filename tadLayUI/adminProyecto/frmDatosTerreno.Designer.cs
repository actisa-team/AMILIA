namespace tadLayUI.adminProyecto
{
    partial class frmDatosTerreno
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
            this.grTerreno = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCrearEnvolvente = new System.Windows.Forms.Button();
            this.btnSelectEnvolvente = new System.Windows.Forms.Button();
            this.ucTolerancia = new tadLayUI.userControl.ucLblTxt();
            this.ucTerreno = new tadLayUI.userControl.ucTerrain();
            this.grDrawZonasNoPaso = new System.Windows.Forms.GroupBox();
            this.btnSelectZonasNoPasoUsuario = new System.Windows.Forms.Button();
            this.btnSelectZonasNoPasoPendiente = new System.Windows.Forms.Button();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.grTerreno.SuspendLayout();
            this.grDrawZonasNoPaso.SuspendLayout();
            this.SuspendLayout();
            // 
            // grTerreno
            // 
            this.grTerreno.Controls.Add(this.button2);
            this.grTerreno.Controls.Add(this.button1);
            this.grTerreno.Controls.Add(this.btnCrearEnvolvente);
            this.grTerreno.Controls.Add(this.btnSelectEnvolvente);
            this.grTerreno.Controls.Add(this.ucTolerancia);
            this.grTerreno.Controls.Add(this.ucTerreno);
            this.grTerreno.Location = new System.Drawing.Point(20, 18);
            this.grTerreno.Name = "grTerreno";
            this.grTerreno.Size = new System.Drawing.Size(350, 225);
            this.grTerreno.TabIndex = 0;
            this.grTerreno.TabStop = false;
            this.grTerreno.Text = "grTerreno";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(179, 66);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Cargar Puntos Terreno ASC";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Cargar Puntos Terreno";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCrearEnvolvente
            // 
            this.btnCrearEnvolvente.Location = new System.Drawing.Point(15, 135);
            this.btnCrearEnvolvente.Name = "btnCrearEnvolvente";
            this.btnCrearEnvolvente.Size = new System.Drawing.Size(323, 23);
            this.btnCrearEnvolvente.TabIndex = 5;
            this.btnCrearEnvolvente.Text = "btnCrearEnvolvente";
            this.btnCrearEnvolvente.UseVisualStyleBackColor = true;
            this.btnCrearEnvolvente.Click += new System.EventHandler(this.btnCrearEnvolvente_Click);
            // 
            // btnSelectEnvolvente
            // 
            this.btnSelectEnvolvente.Location = new System.Drawing.Point(15, 106);
            this.btnSelectEnvolvente.Name = "btnSelectEnvolvente";
            this.btnSelectEnvolvente.Size = new System.Drawing.Size(323, 23);
            this.btnSelectEnvolvente.TabIndex = 4;
            this.btnSelectEnvolvente.Text = "btnSelectEnvolvente";
            this.btnSelectEnvolvente.UseVisualStyleBackColor = true;
            this.btnSelectEnvolvente.Click += new System.EventHandler(this.btnSelectEnvolvente_Click);
            // 
            // ucTolerancia
            // 
            this.ucTolerancia.ancho = 75;
            this.ucTolerancia.isEntero = false;
            this.ucTolerancia.isNegativo = false;
            this.ucTolerancia.isObligatorio = true;
            this.ucTolerancia.isSimboloDecimalPunto = true;
            this.ucTolerancia.location = new System.Drawing.Point(240, 0);
            this.ucTolerancia.Location = new System.Drawing.Point(22, 191);
            this.ucTolerancia.lonMax = 32767;
            this.ucTolerancia.Name = "ucTolerancia";
            this.ucTolerancia.Size = new System.Drawing.Size(320, 20);
            this.ucTolerancia.TabIndex = 4;
            this.ucTolerancia.uiLbl = "ucTolerancia";
            this.ucTolerancia.uitxt = "10";
            this.ucTolerancia.valorDoubleNull = 10D;
            this.ucTolerancia.valorMaximo = 1E+16D;
            this.ucTolerancia.valorMinimo = 0D;
            // 
            // ucTerreno
            // 
            this.ucTerreno.ancho = 200;
            this.ucTerreno.isObligatorio = true;
            this.ucTerreno.location = new System.Drawing.Point(120, 0);
            this.ucTerreno.Location = new System.Drawing.Point(22, 39);
            this.ucTerreno.Name = "ucTerreno";
            this.ucTerreno.Size = new System.Drawing.Size(322, 21);
            this.ucTerreno.TabIndex = 0;
            this.ucTerreno.uiLbl = "ucTerreno";
            this.ucTerreno.Visible = false;
            // 
            // grDrawZonasNoPaso
            // 
            this.grDrawZonasNoPaso.Controls.Add(this.btnSelectZonasNoPasoUsuario);
            this.grDrawZonasNoPaso.Controls.Add(this.btnSelectZonasNoPasoPendiente);
            this.grDrawZonasNoPaso.Location = new System.Drawing.Point(20, 258);
            this.grDrawZonasNoPaso.Name = "grDrawZonasNoPaso";
            this.grDrawZonasNoPaso.Size = new System.Drawing.Size(344, 71);
            this.grDrawZonasNoPaso.TabIndex = 2;
            this.grDrawZonasNoPaso.TabStop = false;
            this.grDrawZonasNoPaso.Text = "grSelectZonasNoPaso";
            // 
            // btnSelectZonasNoPasoUsuario
            // 
            this.btnSelectZonasNoPasoUsuario.Location = new System.Drawing.Point(15, 31);
            this.btnSelectZonasNoPasoUsuario.Name = "btnSelectZonasNoPasoUsuario";
            this.btnSelectZonasNoPasoUsuario.Size = new System.Drawing.Size(321, 23);
            this.btnSelectZonasNoPasoUsuario.TabIndex = 3;
            this.btnSelectZonasNoPasoUsuario.Text = "btnSelectZonasNoPasoUsuario";
            this.btnSelectZonasNoPasoUsuario.UseVisualStyleBackColor = true;
            this.btnSelectZonasNoPasoUsuario.Click += new System.EventHandler(this.btnDrawZonasNoPasoUsuario_Click);
            // 
            // btnSelectZonasNoPasoPendiente
            // 
            this.btnSelectZonasNoPasoPendiente.Location = new System.Drawing.Point(19, 19);
            this.btnSelectZonasNoPasoPendiente.Name = "btnSelectZonasNoPasoPendiente";
            this.btnSelectZonasNoPasoPendiente.Size = new System.Drawing.Size(323, 23);
            this.btnSelectZonasNoPasoPendiente.TabIndex = 2;
            this.btnSelectZonasNoPasoPendiente.Text = "btnSelectZonasNoPasoPendiente";
            this.btnSelectZonasNoPasoPendiente.UseVisualStyleBackColor = true;
            this.btnSelectZonasNoPasoPendiente.Visible = false;
            this.btnSelectZonasNoPasoPendiente.Click += new System.EventHandler(this.btnDrawZonasNoPasoPendiente_Click);
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(26, 335);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(344, 25);
            this.ucToolDetail1.TabIndex = 3;
            // 
            // frmDatosTerreno
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 364);
            this.Controls.Add(this.ucToolDetail1);
            this.Controls.Add(this.grDrawZonasNoPaso);
            this.Controls.Add(this.grTerreno);
            this.Name = "frmDatosTerreno";
            this.Text = "frmDatosTerreno";
            this.grTerreno.ResumeLayout(false);
            this.grDrawZonasNoPaso.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grTerreno;
        private userControl.ucTerrain ucTerreno;
        private System.Windows.Forms.GroupBox grDrawZonasNoPaso;
        private System.Windows.Forms.Button btnSelectZonasNoPasoPendiente;
        private userControl.ucToolDetail ucToolDetail1;
        private System.Windows.Forms.Button btnSelectZonasNoPasoUsuario;
        private userControl.ucLblTxt ucTolerancia;
        private System.Windows.Forms.Button btnSelectEnvolvente;
        private System.Windows.Forms.Button btnCrearEnvolvente;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}