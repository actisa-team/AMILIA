namespace tadLayUI.adminRentabilidad
{
    partial class frmVehManDetail
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grTipoVehiculo = new System.Windows.Forms.GroupBox();
            this.ucVehiculoTipo1 = new tadLayUI.userControl.ucVehiculoTipo();
            this.grDgvMaster = new System.Windows.Forms.GroupBox();
            this.ucToolDgv1 = new tadLayUI.userControl.ucToolDgv();
            this.ucDgvMaster = new tadLayUI.userControl.ucDgv();
            this.grDgvDetail = new System.Windows.Forms.GroupBox();
            this.ucToolDetail1 = new tadLayUI.userControl.ucToolDetail();
            this.ucCosteMantenimiento = new tadLayUI.userControl.ucLblTxt();
            this.grTipoVehiculo.SuspendLayout();
            this.grDgvMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvMaster)).BeginInit();
            this.grDgvDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // grTipoVehiculo
            // 
            this.grTipoVehiculo.Controls.Add(this.ucVehiculoTipo1);
            this.grTipoVehiculo.Location = new System.Drawing.Point(12, 6);
            this.grTipoVehiculo.Name = "grTipoVehiculo";
            this.grTipoVehiculo.Size = new System.Drawing.Size(455, 59);
            this.grTipoVehiculo.TabIndex = 1;
            this.grTipoVehiculo.TabStop = false;
            this.grTipoVehiculo.Text = "grTipoVehiculo";
            // 
            // ucVehiculoTipo1
            // 
            this.ucVehiculoTipo1.ancho = 147;
            this.ucVehiculoTipo1.isObligatorio = false;
            this.ucVehiculoTipo1.location = new System.Drawing.Point(0, 0);
            this.ucVehiculoTipo1.Location = new System.Drawing.Point(38, 26);
            this.ucVehiculoTipo1.Name = "ucVehiculoTipo1";
            this.ucVehiculoTipo1.Size = new System.Drawing.Size(156, 21);
            this.ucVehiculoTipo1.TabIndex = 0;
            this.ucVehiculoTipo1.uiLbl = "";
            // 
            // grDgvMaster
            // 
            this.grDgvMaster.Controls.Add(this.ucToolDgv1);
            this.grDgvMaster.Controls.Add(this.ucDgvMaster);
            this.grDgvMaster.Location = new System.Drawing.Point(13, 71);
            this.grDgvMaster.Name = "grDgvMaster";
            this.grDgvMaster.Size = new System.Drawing.Size(454, 288);
            this.grDgvMaster.TabIndex = 28;
            this.grDgvMaster.TabStop = false;
            this.grDgvMaster.Text = "grDgvMaster";
            // 
            // ucToolDgv1
            // 
            this.ucToolDgv1.isEditEnable = true;
            this.ucToolDgv1.isEraseAllEnable = false;
            this.ucToolDgv1.isEraseEnable = false;
            this.ucToolDgv1.isNewEnable = false;
            this.ucToolDgv1.Location = new System.Drawing.Point(23, 19);
            this.ucToolDgv1.Name = "ucToolDgv1";
            this.ucToolDgv1.Size = new System.Drawing.Size(416, 25);
            this.ucToolDgv1.TabIndex = 1;
            // 
            // ucDgvMaster
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Snow;
            this.ucDgvMaster.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ucDgvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ucDgvMaster.Location = new System.Drawing.Point(23, 49);
            this.ucDgvMaster.Name = "ucDgvMaster";
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ucDgvMaster.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.ucDgvMaster.Size = new System.Drawing.Size(416, 232);
            this.ucDgvMaster.TabIndex = 0;
            // 
            // grDgvDetail
            // 
            this.grDgvDetail.Controls.Add(this.ucToolDetail1);
            this.grDgvDetail.Controls.Add(this.ucCosteMantenimiento);
            this.grDgvDetail.Location = new System.Drawing.Point(13, 367);
            this.grDgvDetail.Name = "grDgvDetail";
            this.grDgvDetail.Size = new System.Drawing.Size(454, 127);
            this.grDgvDetail.TabIndex = 29;
            this.grDgvDetail.TabStop = false;
            this.grDgvDetail.Text = "grDgvDetail";
            // 
            // ucToolDetail1
            // 
            this.ucToolDetail1.Location = new System.Drawing.Point(23, 99);
            this.ucToolDetail1.Name = "ucToolDetail1";
            this.ucToolDetail1.Size = new System.Drawing.Size(425, 25);
            this.ucToolDetail1.TabIndex = 30;
            // 
            // ucCosteMantenimiento
            // 
            this.ucCosteMantenimiento.ancho = 75;
            this.ucCosteMantenimiento.isEntero = false;
            this.ucCosteMantenimiento.isNegativo = false;
            this.ucCosteMantenimiento.isObligatorio = true;
            this.ucCosteMantenimiento.isSimboloDecimalPunto = true;
            this.ucCosteMantenimiento.location = new System.Drawing.Point(333, 0);
            this.ucCosteMantenimiento.Location = new System.Drawing.Point(23, 46);
            this.ucCosteMantenimiento.lonMax = 32767;
            this.ucCosteMantenimiento.Name = "ucCosteMantenimiento";
            this.ucCosteMantenimiento.Size = new System.Drawing.Size(416, 20);
            this.ucCosteMantenimiento.TabIndex = 0;
            this.ucCosteMantenimiento.uiLbl = "ucCosteMantenimiento";
            this.ucCosteMantenimiento.uitxt = "";
            this.ucCosteMantenimiento.valorMaximo = -1D;
            this.ucCosteMantenimiento.valorMinimo = 0D;
            // 
            // frmVehManDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(482, 506);
            this.Controls.Add(this.grDgvDetail);
            this.Controls.Add(this.grDgvMaster);
            this.Controls.Add(this.grTipoVehiculo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVehManDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmVehMan";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConVehDetail_FormClosed);
            this.grTipoVehiculo.ResumeLayout(false);
            this.grDgvMaster.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ucDgvMaster)).EndInit();
            this.grDgvDetail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grTipoVehiculo;
        private userControl.ucVehiculoTipo ucVehiculoTipo1;
        private System.Windows.Forms.GroupBox grDgvMaster;
        private System.Windows.Forms.GroupBox grDgvDetail;
        private userControl.ucDgv ucDgvMaster;
        private userControl.ucLblTxt ucCosteMantenimiento;
        private userControl.ucToolDetail ucToolDetail1;
        private userControl.ucToolDgv ucToolDgv1;


    }
}