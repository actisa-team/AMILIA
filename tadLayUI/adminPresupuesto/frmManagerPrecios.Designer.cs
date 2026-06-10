namespace tadLayUI.adminPresupuesto
{
    partial class frmManagerPrecios
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grItems = new System.Windows.Forms.GroupBox();
            this.lnkClaDelete = new System.Windows.Forms.LinkLabel();
            this.lnkClaEdit = new System.Windows.Forms.LinkLabel();
            this.lnkClaNew = new System.Windows.Forms.LinkLabel();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblFrm = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblGrupo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblClasificacion = new System.Windows.Forms.ToolStripLabel();
            this.grItem = new System.Windows.Forms.GroupBox();
            this.lnkClaCancel = new System.Windows.Forms.LinkLabel();
            this.lnkClaSave = new System.Windows.Forms.LinkLabel();
            this.grUds = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grEditorPrecios = new System.Windows.Forms.GroupBox();
            this.ucTreePrecios1 = new tadLayUI.userControl.ucTreePrecios();
            this.ucUnidad = new tadLayUI.userControl.ucLblTxt();
            this.ucMoneda = new tadLayUI.userControl.ucLblTxt();
            this.ucDescripcion = new tadLayUI.userControl.ucLblTxt();
            this.ucPrecioSecundario = new tadLayUI.userControl.ucLblTxt();
            this.ucPrecioPrincipal = new tadLayUI.userControl.ucLblTxt();
            this.ucNombre = new tadLayUI.userControl.ucLblTxt();
            this.ucUnidadMonetaria1 = new tadLayUI.userControl.ucUnidadMonetaria();
            this.grItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.grItem.SuspendLayout();
            this.grUds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grEditorPrecios.SuspendLayout();
            this.SuspendLayout();
            // 
            // grItems
            // 
            this.grItems.Controls.Add(this.lnkClaDelete);
            this.grItems.Controls.Add(this.lnkClaEdit);
            this.grItems.Controls.Add(this.lnkClaNew);
            this.grItems.Controls.Add(this.dgvData);
            this.grItems.Location = new System.Drawing.Point(8, 68);
            this.grItems.Name = "grItems";
            this.grItems.Size = new System.Drawing.Size(495, 275);
            this.grItems.TabIndex = 1;
            this.grItems.TabStop = false;
            this.grItems.Text = "grItems";
            this.grItems.EnabledChanged += new System.EventHandler(this.grItems_EnabledChanged);
            // 
            // lnkClaDelete
            // 
            this.lnkClaDelete.AutoSize = true;
            this.lnkClaDelete.Location = new System.Drawing.Point(137, 16);
            this.lnkClaDelete.Name = "lnkClaDelete";
            this.lnkClaDelete.Size = new System.Drawing.Size(49, 13);
            this.lnkClaDelete.TabIndex = 29;
            this.lnkClaDelete.TabStop = true;
            this.lnkClaDelete.Text = "lnkBorrar";
            this.lnkClaDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaDelete_LinkClicked);
            // 
            // lnkClaEdit
            // 
            this.lnkClaEdit.AutoSize = true;
            this.lnkClaEdit.Location = new System.Drawing.Point(83, 16);
            this.lnkClaEdit.Name = "lnkClaEdit";
            this.lnkClaEdit.Size = new System.Drawing.Size(48, 13);
            this.lnkClaEdit.TabIndex = 28;
            this.lnkClaEdit.TabStop = true;
            this.lnkClaEdit.Text = "lnkEditar";
            this.lnkClaEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaEdit_LinkClicked);
            // 
            // lnkClaNew
            // 
            this.lnkClaNew.AutoSize = true;
            this.lnkClaNew.Location = new System.Drawing.Point(24, 16);
            this.lnkClaNew.Name = "lnkClaNew";
            this.lnkClaNew.Size = new System.Drawing.Size(53, 13);
            this.lnkClaNew.TabIndex = 27;
            this.lnkClaNew.TabStop = true;
            this.lnkClaNew.Text = "lnkNuevo";
            this.lnkClaNew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaNew_LinkClicked);
            // 
            // dgvData
            // 
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(18, 34);
            this.dgvData.MultiSelect = false;
            this.dgvData.Name = "dgvData";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvData.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvData.RowHeadersVisible = false;
            this.dgvData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvData.Size = new System.Drawing.Size(471, 231);
            this.dgvData.TabIndex = 0;
            this.dgvData.Click += new System.EventHandler(this.dgvData_Click);
            this.dgvData.DoubleClick += new System.EventHandler(this.dgvData_DoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblFrm,
            this.toolStripSeparator1,
            this.lblGrupo,
            this.toolStripSeparator2,
            this.lblClasificacion});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(746, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblFrm
            // 
            this.lblFrm.Name = "lblFrm";
            this.lblFrm.Size = new System.Drawing.Size(41, 22);
            this.lblFrm.Text = "lblFrm";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblGrupo
            // 
            this.lblGrupo.Name = "lblGrupo";
            this.lblGrupo.Size = new System.Drawing.Size(53, 22);
            this.lblGrupo.Text = "lblGrupo";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblClasificacion
            // 
            this.lblClasificacion.Name = "lblClasificacion";
            this.lblClasificacion.Size = new System.Drawing.Size(87, 22);
            this.lblClasificacion.Text = "lblClasificacion";
            // 
            // grItem
            // 
            this.grItem.Controls.Add(this.ucDescripcion);
            this.grItem.Controls.Add(this.ucPrecioSecundario);
            this.grItem.Controls.Add(this.ucPrecioPrincipal);
            this.grItem.Controls.Add(this.lnkClaCancel);
            this.grItem.Controls.Add(this.lnkClaSave);
            this.grItem.Controls.Add(this.ucNombre);
            this.grItem.Location = new System.Drawing.Point(8, 346);
            this.grItem.Name = "grItem";
            this.grItem.Size = new System.Drawing.Size(495, 194);
            this.grItem.TabIndex = 2;
            this.grItem.TabStop = false;
            this.grItem.Text = "grItem";
            // 
            // lnkClaCancel
            // 
            this.lnkClaCancel.AutoSize = true;
            this.lnkClaCancel.Location = new System.Drawing.Point(413, 165);
            this.lnkClaCancel.Name = "lnkClaCancel";
            this.lnkClaCancel.Size = new System.Drawing.Size(63, 13);
            this.lnkClaCancel.TabIndex = 6;
            this.lnkClaCancel.TabStop = true;
            this.lnkClaCancel.Text = "lnkCancelar";
            this.lnkClaCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaCancel_LinkClicked);
            // 
            // lnkClaSave
            // 
            this.lnkClaSave.AutoSize = true;
            this.lnkClaSave.Location = new System.Drawing.Point(340, 165);
            this.lnkClaSave.Name = "lnkClaSave";
            this.lnkClaSave.Size = new System.Drawing.Size(59, 13);
            this.lnkClaSave.TabIndex = 5;
            this.lnkClaSave.TabStop = true;
            this.lnkClaSave.Text = "lnkGuardar";
            this.lnkClaSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaSave_LinkClicked);
            // 
            // grUds
            // 
            this.grUds.Controls.Add(this.ucUnidad);
            this.grUds.Controls.Add(this.ucMoneda);
            this.grUds.Location = new System.Drawing.Point(8, 12);
            this.grUds.Name = "grUds";
            this.grUds.Size = new System.Drawing.Size(495, 51);
            this.grUds.TabIndex = 0;
            this.grUds.TabStop = false;
            this.grUds.Text = "grUds";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ucTreePrecios1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ucUnidadMonetaria1);
            this.splitContainer1.Panel2.Controls.Add(this.grEditorPrecios);
            this.splitContainer1.Size = new System.Drawing.Size(746, 547);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.TabIndex = 3;
            // 
            // grEditorPrecios
            // 
            this.grEditorPrecios.Controls.Add(this.grUds);
            this.grEditorPrecios.Controls.Add(this.grItem);
            this.grEditorPrecios.Controls.Add(this.grItems);
            this.grEditorPrecios.Location = new System.Drawing.Point(7, 3);
            this.grEditorPrecios.Name = "grEditorPrecios";
            this.grEditorPrecios.Size = new System.Drawing.Size(512, 544);
            this.grEditorPrecios.TabIndex = 3;
            this.grEditorPrecios.TabStop = false;
            // 
            // ucTreePrecios1
            // 
            this.ucTreePrecios1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTreePrecios1.Location = new System.Drawing.Point(0, 0);
            this.ucTreePrecios1.Name = "ucTreePrecios1";
            this.ucTreePrecios1.Size = new System.Drawing.Size(220, 547);
            this.ucTreePrecios1.TabIndex = 0;
            // 
            // ucUnidad
            // 
            this.ucUnidad.ancho = 50;
            this.ucUnidad.isEntero = false;
            this.ucUnidad.isNegativo = false;
            this.ucUnidad.isObligatorio = true;
            this.ucUnidad.isSimboloDecimalPunto = true;
            this.ucUnidad.isTexto = true;
            this.ucUnidad.location = new System.Drawing.Point(130, 0);
            this.ucUnidad.Location = new System.Drawing.Point(299, 16);
            this.ucUnidad.lonMax = 32767;
            this.ucUnidad.Name = "ucUnidad";
            this.ucUnidad.Size = new System.Drawing.Size(186, 20);
            this.ucUnidad.TabIndex = 1;
            this.ucUnidad.uiLbl = "lblPrecioPor";
            this.ucUnidad.uitxt = "";
            this.ucUnidad.valorDoubleNull = null;
            this.ucUnidad.valorMaximo = 0D;
            this.ucUnidad.valorMinimo = 0D;
            // 
            // ucMoneda
            // 
            this.ucMoneda.ancho = 50;
            this.ucMoneda.isEntero = false;
            this.ucMoneda.isNegativo = false;
            this.ucMoneda.isObligatorio = true;
            this.ucMoneda.isSimboloDecimalPunto = true;
            this.ucMoneda.isTexto = true;
            this.ucMoneda.location = new System.Drawing.Point(175, 0);
            this.ucMoneda.Location = new System.Drawing.Point(26, 16);
            this.ucMoneda.lonMax = 32767;
            this.ucMoneda.Name = "ucMoneda";
            this.ucMoneda.Size = new System.Drawing.Size(243, 20);
            this.ucMoneda.TabIndex = 0;
            this.ucMoneda.uiLbl = "lblMoneda";
            this.ucMoneda.uitxt = "";
            this.ucMoneda.valorDoubleNull = null;
            this.ucMoneda.valorMaximo = 0D;
            this.ucMoneda.valorMinimo = 0D;
            // 
            // ucDescripcion
            // 
            this.ucDescripcion.ancho = 270;
            this.ucDescripcion.isEntero = false;
            this.ucDescripcion.isMultilinea = true;
            this.ucDescripcion.isNegativo = false;
            this.ucDescripcion.isObligatorio = false;
            this.ucDescripcion.isSimboloDecimalPunto = true;
            this.ucDescripcion.isTexto = true;
            this.ucDescripcion.location = new System.Drawing.Point(200, 0);
            this.ucDescripcion.Location = new System.Drawing.Point(11, 49);
            this.ucDescripcion.lonMax = 32767;
            this.ucDescripcion.Name = "ucDescripcion";
            this.ucDescripcion.Size = new System.Drawing.Size(477, 68);
            this.ucDescripcion.TabIndex = 2;
            this.ucDescripcion.uiLbl = "lbl";
            this.ucDescripcion.uitxt = "";
            this.ucDescripcion.valorDoubleNull = null;
            this.ucDescripcion.valorMaximo = 0D;
            this.ucDescripcion.valorMinimo = 0D;
            // 
            // ucPrecioSecundario
            // 
            this.ucPrecioSecundario.ancho = 100;
            this.ucPrecioSecundario.isEntero = false;
            this.ucPrecioSecundario.isNegativo = false;
            this.ucPrecioSecundario.isObligatorio = true;
            this.ucPrecioSecundario.isSimboloDecimalPunto = true;
            this.ucPrecioSecundario.location = new System.Drawing.Point(200, 0);
            this.ucPrecioSecundario.Location = new System.Drawing.Point(11, 152);
            this.ucPrecioSecundario.lonMax = 32767;
            this.ucPrecioSecundario.Name = "ucPrecioSecundario";
            this.ucPrecioSecundario.Size = new System.Drawing.Size(312, 20);
            this.ucPrecioSecundario.TabIndex = 4;
            this.ucPrecioSecundario.uiLbl = "lblPrecioSecundario";
            this.ucPrecioSecundario.uitxt = "";
            this.ucPrecioSecundario.valorDoubleNull = null;
            this.ucPrecioSecundario.valorMaximo = -1D;
            this.ucPrecioSecundario.valorMinimo = 0D;
            // 
            // ucPrecioPrincipal
            // 
            this.ucPrecioPrincipal.ancho = 100;
            this.ucPrecioPrincipal.isEntero = false;
            this.ucPrecioPrincipal.isNegativo = false;
            this.ucPrecioPrincipal.isObligatorio = true;
            this.ucPrecioPrincipal.isSimboloDecimalPunto = true;
            this.ucPrecioPrincipal.location = new System.Drawing.Point(200, 0);
            this.ucPrecioPrincipal.Location = new System.Drawing.Point(11, 123);
            this.ucPrecioPrincipal.lonMax = 32767;
            this.ucPrecioPrincipal.Name = "ucPrecioPrincipal";
            this.ucPrecioPrincipal.Size = new System.Drawing.Size(312, 20);
            this.ucPrecioPrincipal.TabIndex = 3;
            this.ucPrecioPrincipal.uiLbl = "lblPrecioPrincipal";
            this.ucPrecioPrincipal.uitxt = "";
            this.ucPrecioPrincipal.valorDoubleNull = null;
            this.ucPrecioPrincipal.valorMaximo = -1D;
            this.ucPrecioPrincipal.valorMinimo = 0D;
            // 
            // ucNombre
            // 
            this.ucNombre.ancho = 270;
            this.ucNombre.isEntero = false;
            this.ucNombre.isNegativo = false;
            this.ucNombre.isObligatorio = true;
            this.ucNombre.isSimboloDecimalPunto = true;
            this.ucNombre.isTexto = true;
            this.ucNombre.location = new System.Drawing.Point(200, 0);
            this.ucNombre.Location = new System.Drawing.Point(11, 23);
            this.ucNombre.lonMax = 50;
            this.ucNombre.Name = "ucNombre";
            this.ucNombre.Size = new System.Drawing.Size(478, 20);
            this.ucNombre.TabIndex = 1;
            this.ucNombre.uiLbl = "lblNombre";
            this.ucNombre.uitxt = "";
            this.ucNombre.valorDoubleNull = null;
            this.ucNombre.valorMaximo = 0D;
            this.ucNombre.valorMinimo = 0D;
            // 
            // ucUnidadMonetaria1
            // 
            this.ucUnidadMonetaria1.Location = new System.Drawing.Point(9, 3);
            this.ucUnidadMonetaria1.Name = "ucUnidadMonetaria1";
            this.ucUnidadMonetaria1.Size = new System.Drawing.Size(447, 165);
            this.ucUnidadMonetaria1.TabIndex = 4;
            // 
            // frmManagerPrecios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 572);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmManagerPrecios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPrecios";
            this.grItems.ResumeLayout(false);
            this.grItems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.grItem.ResumeLayout(false);
            this.grItem.PerformLayout();
            this.grUds.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grEditorPrecios.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grItems;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblGrupo;
        private System.Windows.Forms.ToolStripLabel lblClasificacion;
        private System.Windows.Forms.GroupBox grItem;
        private System.Windows.Forms.LinkLabel lnkClaDelete;
        private System.Windows.Forms.LinkLabel lnkClaEdit;
        private System.Windows.Forms.LinkLabel lnkClaNew;
        private System.Windows.Forms.LinkLabel lnkClaCancel;
        private System.Windows.Forms.LinkLabel lnkClaSave;
        private userControl.ucLblTxt ucPrecioSecundario;
        private userControl.ucLblTxt ucPrecioPrincipal;
        private userControl.ucLblTxt ucNombre;
        private System.Windows.Forms.GroupBox grUds;
        private userControl.ucLblTxt ucUnidad;
        private userControl.ucLblTxt ucMoneda;
        private userControl.ucLblTxt ucDescripcion;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private userControl.ucTreePrecios ucTreePrecios1;
        private System.Windows.Forms.ToolStripLabel lblFrm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.GroupBox grEditorPrecios;
        private userControl.ucUnidadMonetaria ucUnidadMonetaria1;
    }
}