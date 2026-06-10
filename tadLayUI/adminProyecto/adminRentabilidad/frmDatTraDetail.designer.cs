namespace tadLayUI.adminRentabilidad
{
    partial class frmDatTraDetail
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
            this.grConexionNew = new System.Windows.Forms.GroupBox();
            this.ucConexionActualEliminar = new tadLayUI.userControl.uclblSiNo();
            this.ucRoadUnoVehiculosPesadosPC = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadUnoAbsorcionTraficoYearFinPC = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadUnoAbsorcionTraficoYearInicialPC = new tadLayUI.userControl.ucLblTxt();
            this.grDatosTraficoOpcion0 = new System.Windows.Forms.GroupBox();
            this.ucConexionCeroVpKmH = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadCeroLonKm = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadCeroVehiculosPesadosPC = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadCeroCrecimientoAnualPC = new tadLayUI.userControl.ucLblTxt();
            this.ucRoadCeroIMDyearPuestaServicio = new tadLayUI.userControl.ucLblTxt();
            this.lnkSave = new System.Windows.Forms.LinkLabel();
            this.grConexionNew.SuspendLayout();
            this.grDatosTraficoOpcion0.SuspendLayout();
            this.SuspendLayout();
            // 
            // grConexionNew
            // 
            this.grConexionNew.Controls.Add(this.ucConexionActualEliminar);
            this.grConexionNew.Controls.Add(this.ucRoadUnoVehiculosPesadosPC);
            this.grConexionNew.Controls.Add(this.ucRoadUnoAbsorcionTraficoYearFinPC);
            this.grConexionNew.Controls.Add(this.ucRoadUnoAbsorcionTraficoYearInicialPC);
            this.grConexionNew.Location = new System.Drawing.Point(12, 283);
            this.grConexionNew.Name = "grConexionNew";
            this.grConexionNew.Size = new System.Drawing.Size(392, 170);
            this.grConexionNew.TabIndex = 26;
            this.grConexionNew.TabStop = false;
            this.grConexionNew.Text = "grConexionNew";
            // 
            // ucConexionActualEliminar
            // 
            this.ucConexionActualEliminar.ancho = 50;
            this.ucConexionActualEliminar.isObligatorio = true;
            this.ucConexionActualEliminar.location = new System.Drawing.Point(300, 0);
            this.ucConexionActualEliminar.Location = new System.Drawing.Point(15, 33);
            this.ucConexionActualEliminar.Name = "ucConexionActualEliminar";
            this.ucConexionActualEliminar.Size = new System.Drawing.Size(366, 21);
            this.ucConexionActualEliminar.TabIndex = 5;
            this.ucConexionActualEliminar.uiLbl = "ucConexionActualEliminar";
            // 
            // ucRoadUnoVehiculosPesadosPC
            // 
            this.ucRoadUnoVehiculosPesadosPC.ancho = 50;
            this.ucRoadUnoVehiculosPesadosPC.isEntero = true;
            this.ucRoadUnoVehiculosPesadosPC.isNegativo = false;
            this.ucRoadUnoVehiculosPesadosPC.isObligatorio = true;
            this.ucRoadUnoVehiculosPesadosPC.isSimboloDecimalPunto = true;
            this.ucRoadUnoVehiculosPesadosPC.location = new System.Drawing.Point(300, 0);
            this.ucRoadUnoVehiculosPesadosPC.Location = new System.Drawing.Point(15, 133);
            this.ucRoadUnoVehiculosPesadosPC.lonMax = 50;
            this.ucRoadUnoVehiculosPesadosPC.Name = "ucRoadUnoVehiculosPesadosPC";
            this.ucRoadUnoVehiculosPesadosPC.Size = new System.Drawing.Size(367, 20);
            this.ucRoadUnoVehiculosPesadosPC.TabIndex = 4;
            this.ucRoadUnoVehiculosPesadosPC.uiLbl = "ucRoadUnoVehiculosPesadosPC";
            this.ucRoadUnoVehiculosPesadosPC.uitxt = "";
            this.ucRoadUnoVehiculosPesadosPC.valorDoubleNull = null;
            this.ucRoadUnoVehiculosPesadosPC.valorMaximo = 100D;
            this.ucRoadUnoVehiculosPesadosPC.valorMinimo = 0D;
            // 
            // ucRoadUnoAbsorcionTraficoYearFinPC
            // 
            this.ucRoadUnoAbsorcionTraficoYearFinPC.ancho = 50;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.isEntero = false;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.isNegativo = false;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.isObligatorio = true;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.isSimboloDecimalPunto = true;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.location = new System.Drawing.Point(300, 0);
            this.ucRoadUnoAbsorcionTraficoYearFinPC.Location = new System.Drawing.Point(15, 100);
            this.ucRoadUnoAbsorcionTraficoYearFinPC.lonMax = 50;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.Name = "ucRoadUnoAbsorcionTraficoYearFinPC";
            this.ucRoadUnoAbsorcionTraficoYearFinPC.Size = new System.Drawing.Size(367, 20);
            this.ucRoadUnoAbsorcionTraficoYearFinPC.TabIndex = 2;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.uiLbl = "ucRoadUnoAbsorcionTraficoYearFinPC";
            this.ucRoadUnoAbsorcionTraficoYearFinPC.uitxt = "";
            this.ucRoadUnoAbsorcionTraficoYearFinPC.valorDoubleNull = null;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.valorMaximo = 100D;
            this.ucRoadUnoAbsorcionTraficoYearFinPC.valorMinimo = 0D;
            // 
            // ucRoadUnoAbsorcionTraficoYearInicialPC
            // 
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.ancho = 50;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.isEntero = false;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.isNegativo = false;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.isObligatorio = true;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.isSimboloDecimalPunto = true;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.location = new System.Drawing.Point(300, 0);
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.Location = new System.Drawing.Point(15, 67);
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.lonMax = 50;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.Name = "ucRoadUnoAbsorcionTraficoYearInicialPC";
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.Size = new System.Drawing.Size(366, 20);
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.TabIndex = 1;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.uiLbl = "ucRoadUnoAbsorcionTraficoYearInicialPC";
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.uitxt = "";
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.valorDoubleNull = null;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.valorMaximo = 100D;
            this.ucRoadUnoAbsorcionTraficoYearInicialPC.valorMinimo = 0D;
            // 
            // grDatosTraficoOpcion0
            // 
            this.grDatosTraficoOpcion0.Controls.Add(this.ucConexionCeroVpKmH);
            this.grDatosTraficoOpcion0.Controls.Add(this.ucRoadCeroLonKm);
            this.grDatosTraficoOpcion0.Controls.Add(this.ucRoadCeroVehiculosPesadosPC);
            this.grDatosTraficoOpcion0.Controls.Add(this.ucRoadCeroCrecimientoAnualPC);
            this.grDatosTraficoOpcion0.Controls.Add(this.ucRoadCeroIMDyearPuestaServicio);
            this.grDatosTraficoOpcion0.Location = new System.Drawing.Point(12, 12);
            this.grDatosTraficoOpcion0.Name = "grDatosTraficoOpcion0";
            this.grDatosTraficoOpcion0.Size = new System.Drawing.Size(392, 265);
            this.grDatosTraficoOpcion0.TabIndex = 25;
            this.grDatosTraficoOpcion0.TabStop = false;
            this.grDatosTraficoOpcion0.Text = "grDatosTraficoOpcion0";
            // 
            // ucConexionCeroVpKmH
            // 
            this.ucConexionCeroVpKmH.ancho = 50;
            this.ucConexionCeroVpKmH.isEntero = false;
            this.ucConexionCeroVpKmH.isNegativo = false;
            this.ucConexionCeroVpKmH.isObligatorio = true;
            this.ucConexionCeroVpKmH.isSimboloDecimalPunto = true;
            this.ucConexionCeroVpKmH.location = new System.Drawing.Point(300, 0);
            this.ucConexionCeroVpKmH.Location = new System.Drawing.Point(9, 176);
            this.ucConexionCeroVpKmH.lonMax = 50;
            this.ucConexionCeroVpKmH.Name = "ucConexionCeroVpKmH";
            this.ucConexionCeroVpKmH.Size = new System.Drawing.Size(368, 20);
            this.ucConexionCeroVpKmH.TabIndex = 5;
            this.ucConexionCeroVpKmH.uiLbl = "ucConexionCeroVpKmH";
            this.ucConexionCeroVpKmH.uitxt = "";
            this.ucConexionCeroVpKmH.valorDoubleNull = null;
            this.ucConexionCeroVpKmH.valorMaximo = 200D;
            this.ucConexionCeroVpKmH.valorMinimo = 0D;
            // 
            // ucRoadCeroLonKm
            // 
            this.ucRoadCeroLonKm.ancho = 50;
            this.ucRoadCeroLonKm.isEntero = false;
            this.ucRoadCeroLonKm.isNegativo = false;
            this.ucRoadCeroLonKm.isObligatorio = true;
            this.ucRoadCeroLonKm.isSimboloDecimalPunto = true;
            this.ucRoadCeroLonKm.location = new System.Drawing.Point(300, 0);
            this.ucRoadCeroLonKm.Location = new System.Drawing.Point(9, 127);
            this.ucRoadCeroLonKm.lonMax = 50;
            this.ucRoadCeroLonKm.Name = "ucRoadCeroLonKm";
            this.ucRoadCeroLonKm.Size = new System.Drawing.Size(368, 20);
            this.ucRoadCeroLonKm.TabIndex = 4;
            this.ucRoadCeroLonKm.uiLbl = "ucRoadCeroLonKm";
            this.ucRoadCeroLonKm.uitxt = "";
            this.ucRoadCeroLonKm.valorDoubleNull = null;
            this.ucRoadCeroLonKm.valorMaximo = 100D;
            this.ucRoadCeroLonKm.valorMinimo = 0D;
            // 
            // ucRoadCeroVehiculosPesadosPC
            // 
            this.ucRoadCeroVehiculosPesadosPC.ancho = 50;
            this.ucRoadCeroVehiculosPesadosPC.isEntero = true;
            this.ucRoadCeroVehiculosPesadosPC.isNegativo = false;
            this.ucRoadCeroVehiculosPesadosPC.isObligatorio = true;
            this.ucRoadCeroVehiculosPesadosPC.isSimboloDecimalPunto = true;
            this.ucRoadCeroVehiculosPesadosPC.location = new System.Drawing.Point(300, 0);
            this.ucRoadCeroVehiculosPesadosPC.Location = new System.Drawing.Point(9, 225);
            this.ucRoadCeroVehiculosPesadosPC.lonMax = 50;
            this.ucRoadCeroVehiculosPesadosPC.Name = "ucRoadCeroVehiculosPesadosPC";
            this.ucRoadCeroVehiculosPesadosPC.Size = new System.Drawing.Size(368, 20);
            this.ucRoadCeroVehiculosPesadosPC.TabIndex = 3;
            this.ucRoadCeroVehiculosPesadosPC.uiLbl = "ucRoadCeroVehiculosPesadosPC";
            this.ucRoadCeroVehiculosPesadosPC.uitxt = "";
            this.ucRoadCeroVehiculosPesadosPC.valorDoubleNull = null;
            this.ucRoadCeroVehiculosPesadosPC.valorMaximo = 100D;
            this.ucRoadCeroVehiculosPesadosPC.valorMinimo = 0D;
            // 
            // ucRoadCeroCrecimientoAnualPC
            // 
            this.ucRoadCeroCrecimientoAnualPC.ancho = 50;
            this.ucRoadCeroCrecimientoAnualPC.isEntero = false;
            this.ucRoadCeroCrecimientoAnualPC.isNegativo = true;
            this.ucRoadCeroCrecimientoAnualPC.isObligatorio = true;
            this.ucRoadCeroCrecimientoAnualPC.isSimboloDecimalPunto = true;
            this.ucRoadCeroCrecimientoAnualPC.location = new System.Drawing.Point(300, 0);
            this.ucRoadCeroCrecimientoAnualPC.Location = new System.Drawing.Point(9, 78);
            this.ucRoadCeroCrecimientoAnualPC.lonMax = 50;
            this.ucRoadCeroCrecimientoAnualPC.Name = "ucRoadCeroCrecimientoAnualPC";
            this.ucRoadCeroCrecimientoAnualPC.Size = new System.Drawing.Size(368, 20);
            this.ucRoadCeroCrecimientoAnualPC.TabIndex = 2;
            this.ucRoadCeroCrecimientoAnualPC.uiLbl = "ucRoadCeroCrecimientoAnualPC";
            this.ucRoadCeroCrecimientoAnualPC.uitxt = "";
            this.ucRoadCeroCrecimientoAnualPC.valorDoubleNull = null;
            this.ucRoadCeroCrecimientoAnualPC.valorMaximo = 100D;
            this.ucRoadCeroCrecimientoAnualPC.valorMinimo = 0D;
            // 
            // ucRoadCeroIMDyearPuestaServicio
            // 
            this.ucRoadCeroIMDyearPuestaServicio.ancho = 50;
            this.ucRoadCeroIMDyearPuestaServicio.isEntero = true;
            this.ucRoadCeroIMDyearPuestaServicio.isNegativo = false;
            this.ucRoadCeroIMDyearPuestaServicio.isObligatorio = true;
            this.ucRoadCeroIMDyearPuestaServicio.isSimboloDecimalPunto = true;
            this.ucRoadCeroIMDyearPuestaServicio.location = new System.Drawing.Point(300, 0);
            this.ucRoadCeroIMDyearPuestaServicio.Location = new System.Drawing.Point(9, 29);
            this.ucRoadCeroIMDyearPuestaServicio.lonMax = 50;
            this.ucRoadCeroIMDyearPuestaServicio.Name = "ucRoadCeroIMDyearPuestaServicio";
            this.ucRoadCeroIMDyearPuestaServicio.Size = new System.Drawing.Size(367, 20);
            this.ucRoadCeroIMDyearPuestaServicio.TabIndex = 1;
            this.ucRoadCeroIMDyearPuestaServicio.uiLbl = "ucRoadCeroIMDyearPuestaServicio";
            this.ucRoadCeroIMDyearPuestaServicio.uitxt = "";
            this.ucRoadCeroIMDyearPuestaServicio.valorDoubleNull = null;
            this.ucRoadCeroIMDyearPuestaServicio.valorMaximo = -1D;
            this.ucRoadCeroIMDyearPuestaServicio.valorMinimo = -1D;
            // 
            // lnkSave
            // 
            this.lnkSave.AutoSize = true;
            this.lnkSave.Location = new System.Drawing.Point(342, 469);
            this.lnkSave.Name = "lnkSave";
            this.lnkSave.Size = new System.Drawing.Size(46, 13);
            this.lnkSave.TabIndex = 26;
            this.lnkSave.TabStop = true;
            this.lnkSave.Text = "lnkSave";
            this.lnkSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSave_LinkClicked);
            // 
            // frmDatTraDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(509, 504);
            this.Controls.Add(this.grConexionNew);
            this.Controls.Add(this.grDatosTraficoOpcion0);
            this.Controls.Add(this.lnkSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDatTraDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDatTra";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.grConexionNew.ResumeLayout(false);
            this.grDatosTraficoOpcion0.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkSave;
        private System.Windows.Forms.GroupBox grDatosTraficoOpcion0;
        private userControl.ucLblTxt ucRoadCeroLonKm;
        private userControl.ucLblTxt ucRoadCeroVehiculosPesadosPC;
        private userControl.ucLblTxt ucRoadCeroCrecimientoAnualPC;
        private userControl.ucLblTxt ucRoadCeroIMDyearPuestaServicio;
        private userControl.ucLblTxt ucConexionCeroVpKmH;
        private System.Windows.Forms.GroupBox grConexionNew;
        private userControl.ucLblTxt ucRoadUnoAbsorcionTraficoYearFinPC;
        private userControl.ucLblTxt ucRoadUnoAbsorcionTraficoYearInicialPC;
        private userControl.ucLblTxt ucRoadUnoVehiculosPesadosPC;
        private userControl.uclblSiNo ucConexionActualEliminar;

    }
}