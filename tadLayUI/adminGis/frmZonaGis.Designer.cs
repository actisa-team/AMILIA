namespace tadLayUI.adminGis
{
    partial class frmZonaGis
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
            this.components = new System.ComponentModel.Container();
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.txtValoracion = new System.Windows.Forms.TextBox();
            this.chkItemProhibirPaso = new System.Windows.Forms.CheckBox();
            this.traValoracion = new System.Windows.Forms.TrackBar();
            this.lblValoracion = new System.Windows.Forms.Label();
            this.txtItemDescripcion = new System.Windows.Forms.TextBox();
            this.lblItemDescripcion = new System.Windows.Forms.Label();
            this.txtItemNombre = new System.Windows.Forms.TextBox();
            this.lblItemNombre = new System.Windows.Forms.Label();
            this.grItems = new System.Windows.Forms.GroupBox();
            this.lnkImgUp = new System.Windows.Forms.LinkLabel();
            this.lnkItemCancel = new System.Windows.Forms.LinkLabel();
            this.lnkItemSave = new System.Windows.Forms.LinkLabel();
            this.lblItemId = new System.Windows.Forms.Label();
            this.imgBicho = new System.Windows.Forms.PictureBox();
            this.lnkItemDelete = new System.Windows.Forms.LinkLabel();
            this.lnkItemEdit = new System.Windows.Forms.LinkLabel();
            this.lnkItemNew = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnZonaLink = new System.Windows.Forms.Button();
            this.dgvClasificaciones = new System.Windows.Forms.DataGridView();
            this.grClasificaciones = new System.Windows.Forms.GroupBox();
            this.lnkClaCancel = new System.Windows.Forms.LinkLabel();
            this.lblClaId = new System.Windows.Forms.Label();
            this.lblClaDescripcion = new System.Windows.Forms.Label();
            this.txtClaDes = new System.Windows.Forms.TextBox();
            this.lnkClaSave = new System.Windows.Forms.LinkLabel();
            this.lnkClaDelete = new System.Windows.Forms.LinkLabel();
            this.lnkClaEdit = new System.Windows.Forms.LinkLabel();
            this.lblClaNombre = new System.Windows.Forms.Label();
            this.txtClaNombre = new System.Windows.Forms.TextBox();
            this.lnkClaNew = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.traValoracion)).BeginInit();
            this.grItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBicho)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClasificaciones)).BeginInit();
            this.grClasificaciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvItems
            // 
            this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvItems.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItems.Location = new System.Drawing.Point(16, 38);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.Size = new System.Drawing.Size(321, 268);
            this.dgvItems.TabIndex = 0;
            this.dgvItems.Click += new System.EventHandler(this.dgvItems_Click);
            // 
            // txtValoracion
            // 
            this.txtValoracion.Enabled = false;
            this.txtValoracion.Location = new System.Drawing.Point(459, 165);
            this.txtValoracion.Name = "txtValoracion";
            this.txtValoracion.ReadOnly = true;
            this.txtValoracion.Size = new System.Drawing.Size(30, 20);
            this.txtValoracion.TabIndex = 8;
            // 
            // chkItemProhibirPaso
            // 
            this.chkItemProhibirPaso.AutoSize = true;
            this.chkItemProhibirPaso.Location = new System.Drawing.Point(351, 138);
            this.chkItemProhibirPaso.Name = "chkItemProhibirPaso";
            this.chkItemProhibirPaso.Size = new System.Drawing.Size(103, 17);
            this.chkItemProhibirPaso.TabIndex = 7;
            this.chkItemProhibirPaso.Text = "chkProhibirPaso";
            this.chkItemProhibirPaso.UseVisualStyleBackColor = true;
            this.chkItemProhibirPaso.CheckedChanged += new System.EventHandler(this.chkItemProhibirPaso_CheckedChanged);
            // 
            // traValoracion
            // 
            this.traValoracion.Location = new System.Drawing.Point(342, 188);
            this.traValoracion.Name = "traValoracion";
            this.traValoracion.Size = new System.Drawing.Size(154, 45);
            this.traValoracion.SmallChange = 0;
            this.traValoracion.TabIndex = 6;
            this.traValoracion.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.traValoracion.Scroll += new System.EventHandler(this.traValoracion_Scroll);
            // 
            // lblValoracion
            // 
            this.lblValoracion.AutoSize = true;
            this.lblValoracion.Location = new System.Drawing.Point(348, 168);
            this.lblValoracion.Name = "lblValoracion";
            this.lblValoracion.Size = new System.Drawing.Size(67, 13);
            this.lblValoracion.TabIndex = 5;
            this.lblValoracion.Text = "lblValoracion";
            // 
            // txtItemDescripcion
            // 
            this.txtItemDescripcion.Location = new System.Drawing.Point(351, 79);
            this.txtItemDescripcion.Multiline = true;
            this.txtItemDescripcion.Name = "txtItemDescripcion";
            this.txtItemDescripcion.Size = new System.Drawing.Size(142, 53);
            this.txtItemDescripcion.TabIndex = 4;
            this.txtItemDescripcion.Validating += new System.ComponentModel.CancelEventHandler(this.txtItemDescripcion_Validating);
            // 
            // lblItemDescripcion
            // 
            this.lblItemDescripcion.AutoSize = true;
            this.lblItemDescripcion.Location = new System.Drawing.Point(351, 62);
            this.lblItemDescripcion.Name = "lblItemDescripcion";
            this.lblItemDescripcion.Size = new System.Drawing.Size(93, 13);
            this.lblItemDescripcion.TabIndex = 3;
            this.lblItemDescripcion.Text = "lblItemDescripcion";
            // 
            // txtItemNombre
            // 
            this.txtItemNombre.Location = new System.Drawing.Point(351, 38);
            this.txtItemNombre.Name = "txtItemNombre";
            this.txtItemNombre.Size = new System.Drawing.Size(142, 20);
            this.txtItemNombre.TabIndex = 2;
            this.txtItemNombre.Validating += new System.ComponentModel.CancelEventHandler(this.txtItemNombre_Validating);
            // 
            // lblItemNombre
            // 
            this.lblItemNombre.AutoSize = true;
            this.lblItemNombre.Location = new System.Drawing.Point(348, 20);
            this.lblItemNombre.Name = "lblItemNombre";
            this.lblItemNombre.Size = new System.Drawing.Size(74, 13);
            this.lblItemNombre.TabIndex = 1;
            this.lblItemNombre.Text = "lblItemNombre";
            // 
            // grItems
            // 
            this.grItems.Controls.Add(this.lnkImgUp);
            this.grItems.Controls.Add(this.lnkItemCancel);
            this.grItems.Controls.Add(this.lnkItemSave);
            this.grItems.Controls.Add(this.lblItemId);
            this.grItems.Controls.Add(this.chkItemProhibirPaso);
            this.grItems.Controls.Add(this.imgBicho);
            this.grItems.Controls.Add(this.lnkItemDelete);
            this.grItems.Controls.Add(this.txtValoracion);
            this.grItems.Controls.Add(this.traValoracion);
            this.grItems.Controls.Add(this.lnkItemEdit);
            this.grItems.Controls.Add(this.lblValoracion);
            this.grItems.Controls.Add(this.lnkItemNew);
            this.grItems.Controls.Add(this.dgvItems);
            this.grItems.Controls.Add(this.lblItemNombre);
            this.grItems.Controls.Add(this.txtItemNombre);
            this.grItems.Controls.Add(this.txtItemDescripcion);
            this.grItems.Controls.Add(this.lblItemDescripcion);
            this.grItems.Location = new System.Drawing.Point(12, 178);
            this.grItems.Name = "grItems";
            this.grItems.Size = new System.Drawing.Size(517, 322);
            this.grItems.TabIndex = 16;
            this.grItems.TabStop = false;
            this.grItems.Text = "grRegistrosPorClasificacion";
            // 
            // lnkImgUp
            // 
            this.lnkImgUp.AutoSize = true;
            this.lnkImgUp.Location = new System.Drawing.Point(381, 218);
            this.lnkImgUp.Name = "lnkImgUp";
            this.lnkImgUp.Size = new System.Drawing.Size(83, 13);
            this.lnkImgUp.TabIndex = 24;
            this.lnkImgUp.TabStop = true;
            this.lnkImgUp.Text = "lnlImagenCargar";
            this.lnkImgUp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkImgUp_LinkClicked);
            // 
            // lnkItemCancel
            // 
            this.lnkItemCancel.AutoSize = true;
            this.lnkItemCancel.Location = new System.Drawing.Point(433, 305);
            this.lnkItemCancel.Name = "lnkItemCancel";
            this.lnkItemCancel.Size = new System.Drawing.Size(74, 13);
            this.lnkItemCancel.TabIndex = 31;
            this.lnkItemCancel.TabStop = true;
            this.lnkItemCancel.Text = "lnkItemCancel";
            this.lnkItemCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkItemCancel_LinkClicked);
            // 
            // lnkItemSave
            // 
            this.lnkItemSave.AutoSize = true;
            this.lnkItemSave.Location = new System.Drawing.Point(347, 305);
            this.lnkItemSave.Name = "lnkItemSave";
            this.lnkItemSave.Size = new System.Drawing.Size(79, 13);
            this.lnkItemSave.TabIndex = 30;
            this.lnkItemSave.TabStop = true;
            this.lnkItemSave.Text = "lnkItemGuardar";
            this.lnkItemSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkItemSave_LinkClicked);
            // 
            // lblItemId
            // 
            this.lblItemId.AutoSize = true;
            this.lblItemId.Location = new System.Drawing.Point(16, 309);
            this.lblItemId.Name = "lblItemId";
            this.lblItemId.Size = new System.Drawing.Size(35, 13);
            this.lblItemId.TabIndex = 28;
            this.lblItemId.Text = "label6";
            this.lblItemId.Visible = false;
            // 
            // imgBicho
            // 
            this.imgBicho.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgBicho.Location = new System.Drawing.Point(385, 236);
            this.imgBicho.Name = "imgBicho";
            this.imgBicho.Size = new System.Drawing.Size(73, 62);
            this.imgBicho.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgBicho.TabIndex = 0;
            this.imgBicho.TabStop = false;
            // 
            // lnkItemDelete
            // 
            this.lnkItemDelete.AutoSize = true;
            this.lnkItemDelete.Location = new System.Drawing.Point(102, 20);
            this.lnkItemDelete.Name = "lnkItemDelete";
            this.lnkItemDelete.Size = new System.Drawing.Size(72, 13);
            this.lnkItemDelete.TabIndex = 27;
            this.lnkItemDelete.TabStop = true;
            this.lnkItemDelete.Text = "lnkItemDelete";
            this.lnkItemDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkItemDelete_LinkClicked);
            // 
            // lnkItemEdit
            // 
            this.lnkItemEdit.AutoSize = true;
            this.lnkItemEdit.Location = new System.Drawing.Point(61, 20);
            this.lnkItemEdit.Name = "lnkItemEdit";
            this.lnkItemEdit.Size = new System.Drawing.Size(59, 13);
            this.lnkItemEdit.TabIndex = 26;
            this.lnkItemEdit.TabStop = true;
            this.lnkItemEdit.Text = "lnkItemEdit";
            this.lnkItemEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkItemEdit_LinkClicked);
            // 
            // lnkItemNew
            // 
            this.lnkItemNew.AutoSize = true;
            this.lnkItemNew.Location = new System.Drawing.Point(16, 20);
            this.lnkItemNew.Name = "lnkItemNew";
            this.lnkItemNew.Size = new System.Drawing.Size(63, 13);
            this.lnkItemNew.TabIndex = 25;
            this.lnkItemNew.TabStop = true;
            this.lnkItemNew.Text = "lnkItemNew";
            this.lnkItemNew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkItemNew_LinkClicked);
            // 
            // btnZonaLink
            // 
            this.btnZonaLink.Location = new System.Drawing.Point(12, 514);
            this.btnZonaLink.Name = "btnZonaLink";
            this.btnZonaLink.Size = new System.Drawing.Size(516, 24);
            this.btnZonaLink.TabIndex = 18;
            this.btnZonaLink.Text = "btnZonaLink";
            this.btnZonaLink.UseVisualStyleBackColor = true;
            this.btnZonaLink.Click += new System.EventHandler(this.btnZonaLink_Click);
            // 
            // dgvClasificaciones
            // 
            this.dgvClasificaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClasificaciones.Location = new System.Drawing.Point(16, 41);
            this.dgvClasificaciones.Name = "dgvClasificaciones";
            this.dgvClasificaciones.Size = new System.Drawing.Size(321, 97);
            this.dgvClasificaciones.TabIndex = 21;
            this.dgvClasificaciones.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvClasificaciones_MouseClick);
            // 
            // grClasificaciones
            // 
            this.grClasificaciones.Controls.Add(this.lnkClaCancel);
            this.grClasificaciones.Controls.Add(this.lblClaId);
            this.grClasificaciones.Controls.Add(this.lblClaDescripcion);
            this.grClasificaciones.Controls.Add(this.txtClaDes);
            this.grClasificaciones.Controls.Add(this.lnkClaSave);
            this.grClasificaciones.Controls.Add(this.lnkClaDelete);
            this.grClasificaciones.Controls.Add(this.lnkClaEdit);
            this.grClasificaciones.Controls.Add(this.lblClaNombre);
            this.grClasificaciones.Controls.Add(this.txtClaNombre);
            this.grClasificaciones.Controls.Add(this.lnkClaNew);
            this.grClasificaciones.Controls.Add(this.dgvClasificaciones);
            this.grClasificaciones.Location = new System.Drawing.Point(12, 5);
            this.grClasificaciones.Name = "grClasificaciones";
            this.grClasificaciones.Size = new System.Drawing.Size(517, 168);
            this.grClasificaciones.TabIndex = 23;
            this.grClasificaciones.TabStop = false;
            this.grClasificaciones.Text = "grClasificaciones";
            // 
            // lnkClaCancel
            // 
            this.lnkClaCancel.AutoSize = true;
            this.lnkClaCancel.Location = new System.Drawing.Point(429, 148);
            this.lnkClaCancel.Name = "lnkClaCancel";
            this.lnkClaCancel.Size = new System.Drawing.Size(69, 13);
            this.lnkClaCancel.TabIndex = 26;
            this.lnkClaCancel.TabStop = true;
            this.lnkClaCancel.Text = "lnkClaCancel";
            this.lnkClaCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaCancel_LinkClicked);
            // 
            // lblClaId
            // 
            this.lblClaId.AutoSize = true;
            this.lblClaId.Location = new System.Drawing.Point(16, 143);
            this.lblClaId.Name = "lblClaId";
            this.lblClaId.Size = new System.Drawing.Size(35, 13);
            this.lblClaId.TabIndex = 24;
            this.lblClaId.Text = "label6";
            this.lblClaId.Visible = false;
            // 
            // lblClaDescripcion
            // 
            this.lblClaDescripcion.AutoSize = true;
            this.lblClaDescripcion.Location = new System.Drawing.Point(354, 68);
            this.lblClaDescripcion.Name = "lblClaDescripcion";
            this.lblClaDescripcion.Size = new System.Drawing.Size(88, 13);
            this.lblClaDescripcion.TabIndex = 7;
            this.lblClaDescripcion.Text = "lblClaDescripcion";
            // 
            // txtClaDes
            // 
            this.txtClaDes.Location = new System.Drawing.Point(354, 86);
            this.txtClaDes.Multiline = true;
            this.txtClaDes.Name = "txtClaDes";
            this.txtClaDes.Size = new System.Drawing.Size(145, 53);
            this.txtClaDes.TabIndex = 8;
            this.txtClaDes.Validating += new System.ComponentModel.CancelEventHandler(this.txtClaDes_Validating);
            // 
            // lnkClaSave
            // 
            this.lnkClaSave.AutoSize = true;
            this.lnkClaSave.Location = new System.Drawing.Point(354, 148);
            this.lnkClaSave.Name = "lnkClaSave";
            this.lnkClaSave.Size = new System.Drawing.Size(61, 13);
            this.lnkClaSave.TabIndex = 25;
            this.lnkClaSave.TabStop = true;
            this.lnkClaSave.Text = "lnkClaSave";
            this.lnkClaSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaSave_LinkClicked);
            // 
            // lnkClaDelete
            // 
            this.lnkClaDelete.AutoSize = true;
            this.lnkClaDelete.Location = new System.Drawing.Point(102, 25);
            this.lnkClaDelete.Name = "lnkClaDelete";
            this.lnkClaDelete.Size = new System.Drawing.Size(67, 13);
            this.lnkClaDelete.TabIndex = 24;
            this.lnkClaDelete.TabStop = true;
            this.lnkClaDelete.Text = "lnkClaDelete";
            this.lnkClaDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaDelete_LinkClicked);
            // 
            // lnkClaEdit
            // 
            this.lnkClaEdit.AutoSize = true;
            this.lnkClaEdit.Location = new System.Drawing.Point(61, 25);
            this.lnkClaEdit.Name = "lnkClaEdit";
            this.lnkClaEdit.Size = new System.Drawing.Size(54, 13);
            this.lnkClaEdit.TabIndex = 23;
            this.lnkClaEdit.TabStop = true;
            this.lnkClaEdit.Text = "lnkClaEdit";
            this.lnkClaEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaEdit_LinkClicked);
            // 
            // lblClaNombre
            // 
            this.lblClaNombre.AutoSize = true;
            this.lblClaNombre.Location = new System.Drawing.Point(354, 20);
            this.lblClaNombre.Name = "lblClaNombre";
            this.lblClaNombre.Size = new System.Drawing.Size(69, 13);
            this.lblClaNombre.TabIndex = 5;
            this.lblClaNombre.Text = "lblClaNombre";
            // 
            // txtClaNombre
            // 
            this.txtClaNombre.Location = new System.Drawing.Point(354, 38);
            this.txtClaNombre.Name = "txtClaNombre";
            this.txtClaNombre.Size = new System.Drawing.Size(145, 20);
            this.txtClaNombre.TabIndex = 6;
            this.txtClaNombre.Validating += new System.ComponentModel.CancelEventHandler(this.txtClaNombre_Validating);
            // 
            // lnkClaNew
            // 
            this.lnkClaNew.AutoSize = true;
            this.lnkClaNew.Location = new System.Drawing.Point(16, 25);
            this.lnkClaNew.Name = "lnkClaNew";
            this.lnkClaNew.Size = new System.Drawing.Size(58, 13);
            this.lnkClaNew.TabIndex = 22;
            this.lnkClaNew.TabStop = true;
            this.lnkClaNew.Text = "lnkClaNew";
            this.lnkClaNew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaNew_LinkClicked);
            // 
            // frmZonaGis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(540, 550);
            this.Controls.Add(this.btnZonaLink);
            this.Controls.Add(this.grClasificaciones);
            this.Controls.Add(this.grItems);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmZonaGis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Shown += new System.EventHandler(this.frmZonaGis_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.traValoracion)).EndInit();
            this.grItems.ResumeLayout(false);
            this.grItems.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgBicho)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClasificaciones)).EndInit();
            this.grClasificaciones.ResumeLayout(false);
            this.grClasificaciones.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItems;
        private System.Windows.Forms.CheckBox chkItemProhibirPaso;
        private System.Windows.Forms.TrackBar traValoracion;
        private System.Windows.Forms.Label lblValoracion;
        private System.Windows.Forms.TextBox txtItemDescripcion;
        private System.Windows.Forms.Label lblItemDescripcion;
        private System.Windows.Forms.TextBox txtItemNombre;
        private System.Windows.Forms.PictureBox imgBicho;
        private System.Windows.Forms.Label lblItemNombre;
        private System.Windows.Forms.GroupBox grItems;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtValoracion;
        private System.Windows.Forms.Button btnZonaLink;
        private System.Windows.Forms.DataGridView dgvClasificaciones;
        private System.Windows.Forms.GroupBox grClasificaciones;
        private System.Windows.Forms.LinkLabel lnkClaNew;
        private System.Windows.Forms.LinkLabel lnkItemDelete;
        private System.Windows.Forms.LinkLabel lnkItemEdit;
        private System.Windows.Forms.LinkLabel lnkItemNew;
        private System.Windows.Forms.LinkLabel lnkClaDelete;
        private System.Windows.Forms.LinkLabel lnkClaEdit;
        private System.Windows.Forms.TextBox txtClaDes;
        private System.Windows.Forms.Label lblClaDescripcion;
        private System.Windows.Forms.TextBox txtClaNombre;
        private System.Windows.Forms.Label lblClaNombre;
        private System.Windows.Forms.Label lblClaId;
        private System.Windows.Forms.LinkLabel lnkClaCancel;
        private System.Windows.Forms.LinkLabel lnkClaSave;
        private System.Windows.Forms.LinkLabel lnkItemCancel;
        private System.Windows.Forms.LinkLabel lnkItemSave;
        private System.Windows.Forms.Label lblItemId;
        private System.Windows.Forms.LinkLabel lnkImgUp;
    }
}