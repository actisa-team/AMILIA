namespace tadLayUI.adminInformes
{
    partial class frmInfSolucionManager
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
            this.grLstSoluciones = new System.Windows.Forms.GroupBox();
            this.ucDgvSolucionesValorar1 = new tadLayUI.userControl.ucDgvSolucionesValorar();
            this.tabValoracion = new System.Windows.Forms.TabControl();
            this.tabPresupuestos = new System.Windows.Forms.TabPage();
            this.btnPCAPrivado = new System.Windows.Forms.Button();
            this.btnPCAPublico = new System.Windows.Forms.Button();
            this.btnPEM = new System.Windows.Forms.Button();
            this.tabRentabilidad = new System.Windows.Forms.TabPage();
            this.btnRentabilidadPrivada = new System.Windows.Forms.Button();
            this.btnRentabilidadPublica = new System.Windows.Forms.Button();
            this.tabVarios = new System.Windows.Forms.TabPage();
            this.btnValoracionTrazadoAlzado = new System.Windows.Forms.Button();
            this.btnValoracionTrazadoPlanta = new System.Windows.Forms.Button();
            this.grInformes = new System.Windows.Forms.GroupBox();
            this.btnValoracionTiempo = new System.Windows.Forms.Button();
            this.grLstSoluciones.SuspendLayout();
            this.tabValoracion.SuspendLayout();
            this.tabPresupuestos.SuspendLayout();
            this.tabRentabilidad.SuspendLayout();
            this.tabVarios.SuspendLayout();
            this.grInformes.SuspendLayout();
            this.SuspendLayout();
            // 
            // grLstSoluciones
            // 
            this.grLstSoluciones.Controls.Add(this.ucDgvSolucionesValorar1);
            this.grLstSoluciones.Location = new System.Drawing.Point(12, 16);
            this.grLstSoluciones.Name = "grLstSoluciones";
            this.grLstSoluciones.Size = new System.Drawing.Size(518, 291);
            this.grLstSoluciones.TabIndex = 0;
            this.grLstSoluciones.TabStop = false;
            this.grLstSoluciones.Text = "grLstSoluciones";
            // 
            // ucDgvSolucionesValorar1
            // 
            this.ucDgvSolucionesValorar1.Location = new System.Drawing.Point(19, 30);
            this.ucDgvSolucionesValorar1.Name = "ucDgvSolucionesValorar1";
            this.ucDgvSolucionesValorar1.Size = new System.Drawing.Size(486, 248);
            this.ucDgvSolucionesValorar1.TabIndex = 0;
            // 
            // tabValoracion
            // 
            this.tabValoracion.Controls.Add(this.tabPresupuestos);
            this.tabValoracion.Controls.Add(this.tabRentabilidad);
            this.tabValoracion.Controls.Add(this.tabVarios);
            this.tabValoracion.Location = new System.Drawing.Point(18, 29);
            this.tabValoracion.Name = "tabValoracion";
            this.tabValoracion.SelectedIndex = 0;
            this.tabValoracion.Size = new System.Drawing.Size(486, 138);
            this.tabValoracion.TabIndex = 1;
            // 
            // tabPresupuestos
            // 
            this.tabPresupuestos.Controls.Add(this.btnPCAPrivado);
            this.tabPresupuestos.Controls.Add(this.btnPCAPublico);
            this.tabPresupuestos.Controls.Add(this.btnPEM);
            this.tabPresupuestos.Location = new System.Drawing.Point(4, 22);
            this.tabPresupuestos.Name = "tabPresupuestos";
            this.tabPresupuestos.Padding = new System.Windows.Forms.Padding(3);
            this.tabPresupuestos.Size = new System.Drawing.Size(478, 112);
            this.tabPresupuestos.TabIndex = 0;
            this.tabPresupuestos.Text = "tabPresupuestos";
            this.tabPresupuestos.UseVisualStyleBackColor = true;
            // 
            // btnPCAPrivado
            // 
            this.btnPCAPrivado.Location = new System.Drawing.Point(21, 74);
            this.btnPCAPrivado.Name = "btnPCAPrivado";
            this.btnPCAPrivado.Size = new System.Drawing.Size(422, 23);
            this.btnPCAPrivado.TabIndex = 2;
            this.btnPCAPrivado.Text = "btnPCAPrivado";
            this.btnPCAPrivado.UseVisualStyleBackColor = true;
            this.btnPCAPrivado.Click += new System.EventHandler(this.btnPCAPrivado_Click);
            // 
            // btnPCAPublico
            // 
            this.btnPCAPublico.Location = new System.Drawing.Point(21, 45);
            this.btnPCAPublico.Name = "btnPCAPublico";
            this.btnPCAPublico.Size = new System.Drawing.Size(422, 23);
            this.btnPCAPublico.TabIndex = 1;
            this.btnPCAPublico.Text = "btnPCAPublico";
            this.btnPCAPublico.UseVisualStyleBackColor = true;
            this.btnPCAPublico.Click += new System.EventHandler(this.btnPCAPublico_Click);
            // 
            // btnPEM
            // 
            this.btnPEM.Location = new System.Drawing.Point(21, 18);
            this.btnPEM.Name = "btnPEM";
            this.btnPEM.Size = new System.Drawing.Size(422, 23);
            this.btnPEM.TabIndex = 0;
            this.btnPEM.Text = "btnPEM";
            this.btnPEM.UseVisualStyleBackColor = true;
            this.btnPEM.Click += new System.EventHandler(this.btnPEM_Click);
            // 
            // tabRentabilidad
            // 
            this.tabRentabilidad.Controls.Add(this.btnRentabilidadPrivada);
            this.tabRentabilidad.Controls.Add(this.btnRentabilidadPublica);
            this.tabRentabilidad.Location = new System.Drawing.Point(4, 22);
            this.tabRentabilidad.Name = "tabRentabilidad";
            this.tabRentabilidad.Padding = new System.Windows.Forms.Padding(3);
            this.tabRentabilidad.Size = new System.Drawing.Size(478, 112);
            this.tabRentabilidad.TabIndex = 1;
            this.tabRentabilidad.Text = "tabRentabilidad";
            this.tabRentabilidad.UseVisualStyleBackColor = true;
            // 
            // btnRentabilidadPrivada
            // 
            this.btnRentabilidadPrivada.Location = new System.Drawing.Point(28, 45);
            this.btnRentabilidadPrivada.Name = "btnRentabilidadPrivada";
            this.btnRentabilidadPrivada.Size = new System.Drawing.Size(422, 23);
            this.btnRentabilidadPrivada.TabIndex = 2;
            this.btnRentabilidadPrivada.Text = "btnRentabilidadPrivada";
            this.btnRentabilidadPrivada.UseVisualStyleBackColor = true;
            this.btnRentabilidadPrivada.Click += new System.EventHandler(this.btnRentabilidadPrivada_Click);
            // 
            // btnRentabilidadPublica
            // 
            this.btnRentabilidadPublica.Location = new System.Drawing.Point(29, 16);
            this.btnRentabilidadPublica.Name = "btnRentabilidadPublica";
            this.btnRentabilidadPublica.Size = new System.Drawing.Size(422, 23);
            this.btnRentabilidadPublica.TabIndex = 1;
            this.btnRentabilidadPublica.Text = "btnRentabilidadPublica";
            this.btnRentabilidadPublica.UseVisualStyleBackColor = true;
            this.btnRentabilidadPublica.Click += new System.EventHandler(this.btnRentabilidadPublica_Click);
            // 
            // tabVarios
            // 
            this.tabVarios.Controls.Add(this.btnValoracionTiempo);
            this.tabVarios.Controls.Add(this.btnValoracionTrazadoAlzado);
            this.tabVarios.Controls.Add(this.btnValoracionTrazadoPlanta);
            this.tabVarios.Location = new System.Drawing.Point(4, 22);
            this.tabVarios.Name = "tabVarios";
            this.tabVarios.Size = new System.Drawing.Size(478, 112);
            this.tabVarios.TabIndex = 2;
            this.tabVarios.Text = "tabVarios";
            this.tabVarios.UseVisualStyleBackColor = true;
            // 
            // btnValoracionTrazadoAlzado
            // 
            this.btnValoracionTrazadoAlzado.Location = new System.Drawing.Point(26, 44);
            this.btnValoracionTrazadoAlzado.Name = "btnValoracionTrazadoAlzado";
            this.btnValoracionTrazadoAlzado.Size = new System.Drawing.Size(176, 23);
            this.btnValoracionTrazadoAlzado.TabIndex = 1;
            this.btnValoracionTrazadoAlzado.Text = "btnValoracionTrazaoAlzado";
            this.btnValoracionTrazadoAlzado.UseVisualStyleBackColor = true;
            this.btnValoracionTrazadoAlzado.Click += new System.EventHandler(this.btnValoracionTrazadoAlzado_Click);
            // 
            // btnValoracionTrazadoPlanta
            // 
            this.btnValoracionTrazadoPlanta.Location = new System.Drawing.Point(26, 15);
            this.btnValoracionTrazadoPlanta.Name = "btnValoracionTrazadoPlanta";
            this.btnValoracionTrazadoPlanta.Size = new System.Drawing.Size(176, 23);
            this.btnValoracionTrazadoPlanta.TabIndex = 0;
            this.btnValoracionTrazadoPlanta.Text = "btnValoracionTrazadoPlanta";
            this.btnValoracionTrazadoPlanta.UseVisualStyleBackColor = true;
            this.btnValoracionTrazadoPlanta.Click += new System.EventHandler(this.btnValoracionTrazadoPlanta_Click);
            // 
            // grInformes
            // 
            this.grInformes.Controls.Add(this.tabValoracion);
            this.grInformes.Location = new System.Drawing.Point(13, 314);
            this.grInformes.Name = "grInformes";
            this.grInformes.Size = new System.Drawing.Size(517, 185);
            this.grInformes.TabIndex = 2;
            this.grInformes.TabStop = false;
            this.grInformes.Text = "grInformes";
            // 
            // btnValoracionTiempo
            // 
            this.btnValoracionTiempo.Location = new System.Drawing.Point(26, 73);
            this.btnValoracionTiempo.Name = "btnValoracionTiempo";
            this.btnValoracionTiempo.Size = new System.Drawing.Size(176, 23);
            this.btnValoracionTiempo.TabIndex = 2;
            this.btnValoracionTiempo.Text = "btnValoracionTiempo";
            this.btnValoracionTiempo.UseVisualStyleBackColor = true;
            this.btnValoracionTiempo.Click += new System.EventHandler(this.btnValoracionTiempo_Click);
            // 
            // frmInfSolucionManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 514);
            this.Controls.Add(this.grInformes);
            this.Controls.Add(this.grLstSoluciones);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInfSolucionManager";
            this.Text = "frmInfSolucionManager";
            this.Load += new System.EventHandler(this.frmInfSolucionManager_Load);
            this.Shown += new System.EventHandler(this.frmInfSolucionManager_Shown);
            this.grLstSoluciones.ResumeLayout(false);
            this.tabValoracion.ResumeLayout(false);
            this.tabPresupuestos.ResumeLayout(false);
            this.tabRentabilidad.ResumeLayout(false);
            this.tabVarios.ResumeLayout(false);
            this.grInformes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grLstSoluciones;
        private userControl.ucDgvSolucionesValorar ucDgvSolucionesValorar1;
        private System.Windows.Forms.TabControl tabValoracion;
        private System.Windows.Forms.TabPage tabPresupuestos;
        private System.Windows.Forms.Button btnPEM;
        private System.Windows.Forms.TabPage tabRentabilidad;
        private System.Windows.Forms.GroupBox grInformes;
        private System.Windows.Forms.Button btnPCAPrivado;
        private System.Windows.Forms.Button btnPCAPublico;
        private System.Windows.Forms.Button btnRentabilidadPublica;
        private System.Windows.Forms.Button btnRentabilidadPrivada;
        private System.Windows.Forms.TabPage tabVarios;
        private System.Windows.Forms.Button btnValoracionTrazadoPlanta;
        private System.Windows.Forms.Button btnValoracionTrazadoAlzado;
        private System.Windows.Forms.Button btnValoracionTiempo;
    }
}