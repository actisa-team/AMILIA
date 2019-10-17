namespace interfaz {
    using System.Windows.Forms;
    using MaterialSkin.Controls;

    partial class ViabilidadComponentesStatusInfoPanel : MaterialForm {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent() {
            this.trazaViablidadDataGridView = new System.Windows.Forms.DataGridView();
            this.ejecutarHastaFinalizarButton = new MaterialSkin.Controls.MaterialFlatButton();
            this.continuarDepuracionButton = new MaterialSkin.Controls.MaterialFlatButton();
            this.detenerEnIteracionTextBox = new System.Windows.Forms.TextBox();
            this.ejecutarHastaIteracionButton = new MaterialSkin.Controls.MaterialFlatButton();
            ((System.ComponentModel.ISupportInitialize)(this.trazaViablidadDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // trazaViablidadDataGridView
            // 
            this.trazaViablidadDataGridView.AllowUserToAddRows = false;
            this.trazaViablidadDataGridView.AllowUserToDeleteRows = false;
            this.trazaViablidadDataGridView.AllowUserToResizeColumns = false;
            this.trazaViablidadDataGridView.AllowUserToResizeRows = false;
            this.trazaViablidadDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.trazaViablidadDataGridView.Location = new System.Drawing.Point(12, 76);
            this.trazaViablidadDataGridView.Name = "trazaViablidadDataGridView";
            this.trazaViablidadDataGridView.ReadOnly = true;
            this.trazaViablidadDataGridView.Size = new System.Drawing.Size(938, 651);
            this.trazaViablidadDataGridView.TabIndex = 0;
            // 
            // ejecutarHastaFinalizarButton
            // 
            this.ejecutarHastaFinalizarButton.AutoSize = true;
            this.ejecutarHastaFinalizarButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ejecutarHastaFinalizarButton.Depth = 0;
            this.ejecutarHastaFinalizarButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ejecutarHastaFinalizarButton.Location = new System.Drawing.Point(12, 753);
            this.ejecutarHastaFinalizarButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ejecutarHastaFinalizarButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.ejecutarHastaFinalizarButton.Name = "ejecutarHastaFinalizarButton";
            this.ejecutarHastaFinalizarButton.Primary = false;
            this.ejecutarHastaFinalizarButton.Size = new System.Drawing.Size(198, 36);
            this.ejecutarHastaFinalizarButton.TabIndex = 1;
            this.ejecutarHastaFinalizarButton.Text = "Ejecutar hasta finalizar";
            this.ejecutarHastaFinalizarButton.UseVisualStyleBackColor = true;
            // 
            // continuarDepuracionButton
            // 
            this.continuarDepuracionButton.AutoSize = true;
            this.continuarDepuracionButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.continuarDepuracionButton.Depth = 0;
            this.continuarDepuracionButton.Location = new System.Drawing.Point(239, 753);
            this.continuarDepuracionButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.continuarDepuracionButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.continuarDepuracionButton.Name = "continuarDepuracionButton";
            this.continuarDepuracionButton.Primary = false;
            this.continuarDepuracionButton.Size = new System.Drawing.Size(173, 36);
            this.continuarDepuracionButton.TabIndex = 2;
            this.continuarDepuracionButton.Text = "Continuar depurando";
            this.continuarDepuracionButton.UseVisualStyleBackColor = true;
            this.continuarDepuracionButton.Click += new System.EventHandler(this.continuarDepuracionButton_Click);
            // 
            // detenerEnIteracionTextBox
            // 
            this.detenerEnIteracionTextBox.Location = new System.Drawing.Point(220, 818);
            this.detenerEnIteracionTextBox.Name = "detenerEnIteracionTextBox";
            this.detenerEnIteracionTextBox.Size = new System.Drawing.Size(66, 20);
            this.detenerEnIteracionTextBox.TabIndex = 4;
            // 
            // ejecutarHastaIteracionButton
            // 
            this.ejecutarHastaIteracionButton.AutoSize = true;
            this.ejecutarHastaIteracionButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ejecutarHastaIteracionButton.Depth = 0;
            this.ejecutarHastaIteracionButton.Location = new System.Drawing.Point(13, 809);
            this.ejecutarHastaIteracionButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ejecutarHastaIteracionButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.ejecutarHastaIteracionButton.Name = "ejecutarHastaIteracionButton";
            this.ejecutarHastaIteracionButton.Primary = false;
            this.ejecutarHastaIteracionButton.Size = new System.Drawing.Size(200, 36);
            this.ejecutarHastaIteracionButton.TabIndex = 6;
            this.ejecutarHastaIteracionButton.Text = "Ejecutar hasta iteración";
            this.ejecutarHastaIteracionButton.UseVisualStyleBackColor = true;
            // 
            // ViabilidadComponentesStatusInfoPanel
            // 
            this.ClientSize = new System.Drawing.Size(962, 885);
            this.Controls.Add(this.ejecutarHastaIteracionButton);
            this.Controls.Add(this.detenerEnIteracionTextBox);
            this.Controls.Add(this.continuarDepuracionButton);
            this.Controls.Add(this.ejecutarHastaFinalizarButton);
            this.Controls.Add(this.trazaViablidadDataGridView);
            this.Name = "ViabilidadComponentesStatusInfoPanel";
            ((System.ComponentModel.ISupportInitialize)(this.trazaViablidadDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView trazaViablidadDataGridView;
        private MaterialFlatButton ejecutarHastaFinalizarButton;
        private MaterialFlatButton continuarDepuracionButton;
        private System.Windows.Forms.TextBox detenerEnIteracionTextBox;
        private MaterialFlatButton ejecutarHastaIteracionButton;

        public TextBox DetenerEnIteracionTextBox { get => detenerEnIteracionTextBox; set => detenerEnIteracionTextBox = value; }
    }
}