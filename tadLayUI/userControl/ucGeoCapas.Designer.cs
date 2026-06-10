namespace tadLayUI.userControl
{
    partial class ucGeoCapas
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
            this.grMaster = new System.Windows.Forms.GroupBox();
            this.lnkBorrarAll = new System.Windows.Forms.LinkLabel();
            this.lblEspesorTotal = new System.Windows.Forms.Label();
            this.lnkClaDelete = new System.Windows.Forms.LinkLabel();
            this.lnkClaEdit = new System.Windows.Forms.LinkLabel();
            this.lnkClaNew = new System.Windows.Forms.LinkLabel();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.grDetail = new System.Windows.Forms.GroupBox();
            this.lnkClaCancel = new System.Windows.Forms.LinkLabel();
            this.lnkClaSave = new System.Windows.Forms.LinkLabel();
            this.ucCmbMaterial = new tadLayUI.userControl.uclblCmbPrecios();
            this.ucCapaEspesor = new tadLayUI.userControl.ucLblTxt();
            this.ucCapaPosicion = new tadLayUI.userControl.ucLblTxt();
            this.grMaster.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.grDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // grMaster
            // 
            this.grMaster.Controls.Add(this.lnkBorrarAll);
            this.grMaster.Controls.Add(this.lblEspesorTotal);
            this.grMaster.Controls.Add(this.lnkClaDelete);
            this.grMaster.Controls.Add(this.lnkClaEdit);
            this.grMaster.Controls.Add(this.lnkClaNew);
            this.grMaster.Controls.Add(this.dgv);
            this.grMaster.Location = new System.Drawing.Point(3, 3);
            this.grMaster.Name = "grMaster";
            this.grMaster.Size = new System.Drawing.Size(300, 246);
            this.grMaster.TabIndex = 0;
            this.grMaster.TabStop = false;
            this.grMaster.Text = "grMaster";
            this.grMaster.EnabledChanged += new System.EventHandler(this.grMaster_EnabledChanged);
            // 
            // lnkBorrarAll
            // 
            this.lnkBorrarAll.AutoSize = true;
            this.lnkBorrarAll.Location = new System.Drawing.Point(182, 24);
            this.lnkBorrarAll.Name = "lnkBorrarAll";
            this.lnkBorrarAll.Size = new System.Drawing.Size(60, 13);
            this.lnkBorrarAll.TabIndex = 34;
            this.lnkBorrarAll.TabStop = true;
            this.lnkBorrarAll.Text = "lnkBorrarAll";
            this.lnkBorrarAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkBorrarAll_LinkClicked);
            // 
            // lblEspesorTotal
            // 
            this.lblEspesorTotal.AutoSize = true;
            this.lblEspesorTotal.Location = new System.Drawing.Point(17, 225);
            this.lblEspesorTotal.Name = "lblEspesorTotal";
            this.lblEspesorTotal.Size = new System.Drawing.Size(79, 13);
            this.lblEspesorTotal.TabIndex = 33;
            this.lblEspesorTotal.Text = "lblEspesorTotal";
            // 
            // lnkClaDelete
            // 
            this.lnkClaDelete.AutoSize = true;
            this.lnkClaDelete.Location = new System.Drawing.Point(127, 24);
            this.lnkClaDelete.Name = "lnkClaDelete";
            this.lnkClaDelete.Size = new System.Drawing.Size(49, 13);
            this.lnkClaDelete.TabIndex = 32;
            this.lnkClaDelete.TabStop = true;
            this.lnkClaDelete.Text = "lnkBorrar";
            this.lnkClaDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaDelete_LinkClicked);
            // 
            // lnkClaEdit
            // 
            this.lnkClaEdit.AutoSize = true;
            this.lnkClaEdit.Location = new System.Drawing.Point(73, 24);
            this.lnkClaEdit.Name = "lnkClaEdit";
            this.lnkClaEdit.Size = new System.Drawing.Size(48, 13);
            this.lnkClaEdit.TabIndex = 31;
            this.lnkClaEdit.TabStop = true;
            this.lnkClaEdit.Text = "lnkEditar";
            this.lnkClaEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaEdit_LinkClicked);
            // 
            // lnkClaNew
            // 
            this.lnkClaNew.AutoSize = true;
            this.lnkClaNew.Location = new System.Drawing.Point(14, 24);
            this.lnkClaNew.Name = "lnkClaNew";
            this.lnkClaNew.Size = new System.Drawing.Size(53, 13);
            this.lnkClaNew.TabIndex = 30;
            this.lnkClaNew.TabStop = true;
            this.lnkClaNew.Text = "lnkNuevo";
            this.lnkClaNew.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaNew_LinkClicked);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgv.Location = new System.Drawing.Point(17, 45);
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new System.Drawing.Size(266, 173);
            this.dgv.TabIndex = 0;
            this.dgv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgv_KeyDown);
            this.dgv.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgv_MouseClick);
            this.dgv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgv_MouseDoubleClick);
            // 
            // grDetail
            // 
            this.grDetail.Controls.Add(this.ucCmbMaterial);
            this.grDetail.Controls.Add(this.ucCapaEspesor);
            this.grDetail.Controls.Add(this.ucCapaPosicion);
            this.grDetail.Location = new System.Drawing.Point(3, 253);
            this.grDetail.Name = "grDetail";
            this.grDetail.Size = new System.Drawing.Size(300, 103);
            this.grDetail.TabIndex = 1;
            this.grDetail.TabStop = false;
            this.grDetail.Text = "grDetail";
            // 
            // lnkClaCancel
            // 
            this.lnkClaCancel.AutoSize = true;
            this.lnkClaCancel.Location = new System.Drawing.Point(220, 359);
            this.lnkClaCancel.Name = "lnkClaCancel";
            this.lnkClaCancel.Size = new System.Drawing.Size(63, 13);
            this.lnkClaCancel.TabIndex = 8;
            this.lnkClaCancel.TabStop = true;
            this.lnkClaCancel.Text = "lnkCancelar";
            this.lnkClaCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaCancel_LinkClicked);
            // 
            // lnkClaSave
            // 
            this.lnkClaSave.AutoSize = true;
            this.lnkClaSave.Location = new System.Drawing.Point(155, 359);
            this.lnkClaSave.Name = "lnkClaSave";
            this.lnkClaSave.Size = new System.Drawing.Size(59, 13);
            this.lnkClaSave.TabIndex = 7;
            this.lnkClaSave.TabStop = true;
            this.lnkClaSave.Text = "lnkGuardar";
            this.lnkClaSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClaSave_LinkClicked);
            // 
            // ucCmbMaterial
            // 
            this.ucCmbMaterial.ancho = 140;
            this.ucCmbMaterial.idCode = "ASI";
            this.ucCmbMaterial.idGrupo = "REL";
            this.ucCmbMaterial.isObligatorio = true;
            this.ucCmbMaterial.location = new System.Drawing.Point(125, 0);
            this.ucCmbMaterial.Location = new System.Drawing.Point(17, 72);
            this.ucCmbMaterial.Name = "ucCmbMaterial";
            this.ucCmbMaterial.Size = new System.Drawing.Size(266, 21);
            this.ucCmbMaterial.TabIndex = 2;
            this.ucCmbMaterial.uiLbl = "ucCmbMaterial";
            // 
            // ucCapaEspesor
            // 
            this.ucCapaEspesor.ancho = 140;
            this.ucCapaEspesor.isEntero = false;
            this.ucCapaEspesor.isNegativo = false;
            this.ucCapaEspesor.isObligatorio = true;
            this.ucCapaEspesor.isSimboloDecimalPunto = true;
            this.ucCapaEspesor.location = new System.Drawing.Point(125, 0);
            this.ucCapaEspesor.Location = new System.Drawing.Point(17, 46);
            this.ucCapaEspesor.lonMax = 32767;
            this.ucCapaEspesor.Name = "ucCapaEspesor";
            this.ucCapaEspesor.Size = new System.Drawing.Size(266, 20);
            this.ucCapaEspesor.TabIndex = 1;
            this.ucCapaEspesor.uiLbl = "ucCapaEspesor";
            this.ucCapaEspesor.uitxt = "";
            this.ucCapaEspesor.valorMaximo = 10D;
            this.ucCapaEspesor.valorMinimo = 0D;
            // 
            // ucCapaPosicion
            // 
            this.ucCapaPosicion.ancho = 140;
            this.ucCapaPosicion.isEntero = true;
            this.ucCapaPosicion.isNegativo = false;
            this.ucCapaPosicion.isObligatorio = true;
            this.ucCapaPosicion.isSimboloDecimalPunto = true;
            this.ucCapaPosicion.location = new System.Drawing.Point(125, 0);
            this.ucCapaPosicion.Location = new System.Drawing.Point(17, 20);
            this.ucCapaPosicion.lonMax = 32767;
            this.ucCapaPosicion.Name = "ucCapaPosicion";
            this.ucCapaPosicion.Size = new System.Drawing.Size(266, 20);
            this.ucCapaPosicion.TabIndex = 0;
            this.ucCapaPosicion.uiLbl = "ucCapaPosicion";
            this.ucCapaPosicion.uitxt = "";
            this.ucCapaPosicion.valorMaximo = 10D;
            this.ucCapaPosicion.valorMinimo = 1D;
            // 
            // ucGeoCapas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkClaCancel);
            this.Controls.Add(this.grDetail);
            this.Controls.Add(this.grMaster);
            this.Controls.Add(this.lnkClaSave);
            this.Name = "ucGeoCapas";
            this.Size = new System.Drawing.Size(314, 383);
            this.Load += new System.EventHandler(this.ucGeoCapas_Load);
            this.grMaster.ResumeLayout(false);
            this.grMaster.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.grDetail.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grMaster;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.GroupBox grDetail;
        private System.Windows.Forms.LinkLabel lnkClaDelete;
        private System.Windows.Forms.LinkLabel lnkClaEdit;
        private System.Windows.Forms.LinkLabel lnkClaNew;
        private uclblCmbPrecios ucCmbMaterial;
        private ucLblTxt ucCapaEspesor;
        private ucLblTxt ucCapaPosicion;
        private System.Windows.Forms.LinkLabel lnkClaCancel;
        private System.Windows.Forms.LinkLabel lnkClaSave;
        private System.Windows.Forms.Label lblEspesorTotal;
        private System.Windows.Forms.LinkLabel lnkBorrarAll;
    }
}
