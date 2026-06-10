
namespace interfaz
{
    partial class VistaComponentes
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.AniadirEntidad = new System.Windows.Forms.Button();
            this.dsApp1 = new Datos.dsApp();
            this.Crear_trazado = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsApp1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(24, 21);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(798, 324);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // AniadirEntidad
            // 
            this.AniadirEntidad.Location = new System.Drawing.Point(24, 365);
            this.AniadirEntidad.Name = "AniadirEntidad";
            this.AniadirEntidad.Size = new System.Drawing.Size(112, 23);
            this.AniadirEntidad.TabIndex = 1;
            this.AniadirEntidad.Text = "Añadir Entidad";
            this.AniadirEntidad.UseVisualStyleBackColor = true;
            this.AniadirEntidad.Click += new System.EventHandler(this.AniadirEntidad_Click);
            // 
            // dsApp1
            // 
            this.dsApp1.DataSetName = "dsApp";
            this.dsApp1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Crear_trazado
            // 
            this.Crear_trazado.Location = new System.Drawing.Point(165, 365);
            this.Crear_trazado.Name = "Crear_trazado";
            this.Crear_trazado.Size = new System.Drawing.Size(112, 23);
            this.Crear_trazado.TabIndex = 2;
            this.Crear_trazado.Text = "Crear Trazado";
            this.Crear_trazado.UseVisualStyleBackColor = true;
            this.Crear_trazado.Click += new System.EventHandler(this.Crear_trazado_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(695, 365);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Limpiar Componentes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // VistaComponentes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Crear_trazado);
            this.Controls.Add(this.AniadirEntidad);
            this.Controls.Add(this.dataGridView1);
            this.Name = "VistaComponentes";
            this.Text = "VistaComponentes";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsApp1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button AniadirEntidad;
        private Datos.dsApp dsApp1;
        private System.Windows.Forms.Button Crear_trazado;
        private System.Windows.Forms.Button button1;
    }
}