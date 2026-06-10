using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Autodesk.AutoCAD.DatabaseServices;
using engCadNet;
using tadLayUI.userControl;

namespace tadLayUI
{
    partial class frmManagerTerreno
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManagerTerreno));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbCurvasRotura = new System.Windows.Forms.CheckBox();
            this.buttonInfo = new System.Windows.Forms.Button();
            this.buttonCrearTerreno = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ucTerrain1 = new tadLayUI.userControl.ucTerrain();
            this.ucLblTxtDistCurvNivel = new tadLayUI.userControl.ucLblTxt();
            this.ucLblTxtRangoBusqueda = new tadLayUI.userControl.ucLblTxt();
            this.ucNodosPorHoja = new tadLayUI.userControl.ucLblTxt();
            this.ucIntervaloLineasR = new tadLayUI.userControl.ucLblTxt();
            this.ucLblTxt4 = new tadLayUI.userControl.ucLblTxt();
            this.ucLblTxt3 = new tadLayUI.userControl.ucLblTxt();
            this.ucLblTxt2 = new tadLayUI.userControl.ucLblTxt();
            this.ucLblTxt1 = new tadLayUI.userControl.ucLblTxt();
            this.ucCmbLayersTodas2 = new tadLayUI.userControl.ucCmbLayersTodas();
            this.ucCmbLayersTodas1 = new tadLayUI.userControl.ucCmbLayersTodas();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucCmbLayersTodas2);
            this.groupBox1.Controls.Add(this.ucCmbLayersTodas1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 97);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbCurvasRotura);
            this.groupBox2.Controls.Add(this.ucLblTxtDistCurvNivel);
            this.groupBox2.Controls.Add(this.ucLblTxtRangoBusqueda);
            this.groupBox2.Controls.Add(this.buttonInfo);
            this.groupBox2.Controls.Add(this.ucNodosPorHoja);
            this.groupBox2.Controls.Add(this.ucIntervaloLineasR);
            this.groupBox2.Controls.Add(this.ucLblTxt4);
            this.groupBox2.Controls.Add(this.ucLblTxt3);
            this.groupBox2.Controls.Add(this.ucLblTxt2);
            this.groupBox2.Controls.Add(this.ucLblTxt1);
            this.groupBox2.Location = new System.Drawing.Point(12, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(440, 101);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // chbCurvasRotura
            // 
            this.chbCurvasRotura.Checked = true;
            this.chbCurvasRotura.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbCurvasRotura.Location = new System.Drawing.Point(235, 10);
            this.chbCurvasRotura.Name = "chbCurvasRotura";
            this.chbCurvasRotura.Size = new System.Drawing.Size(100, 40);
            this.chbCurvasRotura.TabIndex = 6;
            // 
            // buttonInfo
            // 
            this.buttonInfo.Location = new System.Drawing.Point(251, 100);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(70, 23);
            this.buttonInfo.TabIndex = 2;
            this.buttonInfo.Text = "buttonInfo";
            this.buttonInfo.UseVisualStyleBackColor = true;
            this.buttonInfo.Visible = false;
            this.buttonInfo.Click += new System.EventHandler(this.buttonInfo_Click);
            // 
            // buttonCrearTerreno
            // 
            this.buttonCrearTerreno.Location = new System.Drawing.Point(12, 222);
            this.buttonCrearTerreno.Name = "buttonCrearTerreno";
            this.buttonCrearTerreno.Size = new System.Drawing.Size(440, 23);
            this.buttonCrearTerreno.TabIndex = 2;
            this.buttonCrearTerreno.Text = "buttonCrearTerreno";
            this.buttonCrearTerreno.UseVisualStyleBackColor = true;
            this.buttonCrearTerreno.Click += new System.EventHandler(this.buttonCrearTerreno_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 283);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(464, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ucTerrain1);
            this.groupBox4.Location = new System.Drawing.Point(12, 222);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(315, 35);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            this.groupBox4.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(333, 234);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 251);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(440, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 6;
            // 
            // ucTerrain1
            // 
            this.ucTerrain1.ancho = 147;
            this.ucTerrain1.isObligatorio = false;
            this.ucTerrain1.location = new System.Drawing.Point(162, 0);
            this.ucTerrain1.Location = new System.Drawing.Point(12, 12);
            this.ucTerrain1.Name = "ucTerrain1";
            this.ucTerrain1.Size = new System.Drawing.Size(309, 21);
            this.ucTerrain1.TabIndex = 0;
            this.ucTerrain1.uiLbl = "label1";
            // 
            // ucLblTxtDistCurvNivel
            // 
            this.ucLblTxtDistCurvNivel.ancho = 75;
            this.ucLblTxtDistCurvNivel.isEntero = true;
            this.ucLblTxtDistCurvNivel.isNegativo = false;
            this.ucLblTxtDistCurvNivel.isObligatorio = true;
            this.ucLblTxtDistCurvNivel.isSimboloDecimalPunto = true;
            this.ucLblTxtDistCurvNivel.location = new System.Drawing.Point(115, 0);
            this.ucLblTxtDistCurvNivel.Location = new System.Drawing.Point(15, 74);
            this.ucLblTxtDistCurvNivel.lonMax = 32767;
            this.ucLblTxtDistCurvNivel.Name = "ucLblTxtDistCurvNivel";
            this.ucLblTxtDistCurvNivel.Size = new System.Drawing.Size(205, 20);
            this.ucLblTxtDistCurvNivel.TabIndex = 5;
            this.ucLblTxtDistCurvNivel.uiLbl = "ucDistCurvasNivel";
            this.ucLblTxtDistCurvNivel.uitxt = "20";
            this.ucLblTxtDistCurvNivel.valorDoubleNull = 20D;
            this.ucLblTxtDistCurvNivel.valorMaximo = 500D;
            this.ucLblTxtDistCurvNivel.valorMinimo = 15D;
            // 
            // ucLblTxtRangoBusqueda
            // 
            this.ucLblTxtRangoBusqueda.ancho = 75;
            this.ucLblTxtRangoBusqueda.isEntero = true;
            this.ucLblTxtRangoBusqueda.isNegativo = false;
            this.ucLblTxtRangoBusqueda.isObligatorio = true;
            this.ucLblTxtRangoBusqueda.isSimboloDecimalPunto = true;
            this.ucLblTxtRangoBusqueda.location = new System.Drawing.Point(115, 0);
            this.ucLblTxtRangoBusqueda.Location = new System.Drawing.Point(229, 74);
            this.ucLblTxtRangoBusqueda.lonMax = 32767;
            this.ucLblTxtRangoBusqueda.Name = "ucLblTxtRangoBusqueda";
            this.ucLblTxtRangoBusqueda.Size = new System.Drawing.Size(205, 20);
            this.ucLblTxtRangoBusqueda.TabIndex = 5;
            this.ucLblTxtRangoBusqueda.uiLbl = "ucRangoBusqueda";
            this.ucLblTxtRangoBusqueda.uitxt = "1000";
            this.ucLblTxtRangoBusqueda.valorDoubleNull = 1000D;
            this.ucLblTxtRangoBusqueda.valorMaximo = 500D;
            this.ucLblTxtRangoBusqueda.valorMinimo = 15D;
            this.ucLblTxtRangoBusqueda.Visible = false;
            // 
            // ucNodosPorHoja
            // 
            this.ucNodosPorHoja.ancho = 75;
            this.ucNodosPorHoja.isEntero = true;
            this.ucNodosPorHoja.isNegativo = false;
            this.ucNodosPorHoja.isObligatorio = true;
            this.ucNodosPorHoja.isSimboloDecimalPunto = true;
            this.ucNodosPorHoja.location = new System.Drawing.Point(130, 0);
            this.ucNodosPorHoja.Location = new System.Drawing.Point(15, 113);
            this.ucNodosPorHoja.lonMax = 32767;
            this.ucNodosPorHoja.Name = "ucNodosPorHoja";
            this.ucNodosPorHoja.Size = new System.Drawing.Size(205, 20);
            this.ucNodosPorHoja.TabIndex = 4;
            this.ucNodosPorHoja.uiLbl = "ucNodosPorHoja";
            this.ucNodosPorHoja.uitxt = "150";
            this.ucNodosPorHoja.valorDoubleNull = 150D;
            this.ucNodosPorHoja.valorMaximo = 500D;
            this.ucNodosPorHoja.valorMinimo = 15D;
            this.ucNodosPorHoja.Visible = false;
            // 
            // ucIntervaloLineasR
            // 
            this.ucIntervaloLineasR.ancho = 75;
            this.ucIntervaloLineasR.isEntero = false;
            this.ucIntervaloLineasR.isNegativo = false;
            this.ucIntervaloLineasR.isObligatorio = false;
            this.ucIntervaloLineasR.isSimboloDecimalPunto = true;
            this.ucIntervaloLineasR.location = new System.Drawing.Point(330, 0);
            this.ucIntervaloLineasR.Location = new System.Drawing.Point(15, 45);
            this.ucIntervaloLineasR.lonMax = 32767;
            this.ucIntervaloLineasR.Name = "ucIntervaloLineasR";
            this.ucIntervaloLineasR.Size = new System.Drawing.Size(405, 23);
            this.ucIntervaloLineasR.TabIndex = 4;
            this.ucIntervaloLineasR.uiLbl = "ucIntervaloLineasR";
            this.ucIntervaloLineasR.uitxt = "30";
            this.ucIntervaloLineasR.valorDoubleNull = 30D;
            this.ucIntervaloLineasR.valorMaximo = 1E+16D;
            this.ucIntervaloLineasR.valorMinimo = 0D;
            // 
            // ucLblTxt4
            // 
            this.ucLblTxt4.ancho = 75;
            this.ucLblTxt4.isEntero = false;
            this.ucLblTxt4.isNegativo = false;
            this.ucLblTxt4.isObligatorio = false;
            this.ucLblTxt4.isSimboloDecimalPunto = true;
            this.ucLblTxt4.location = new System.Drawing.Point(130, 0);
            this.ucLblTxt4.Location = new System.Drawing.Point(0, 100);
            this.ucLblTxt4.lonMax = 32767;
            this.ucLblTxt4.Name = "ucLblTxt4";
            this.ucLblTxt4.Size = new System.Drawing.Size(205, 20);
            this.ucLblTxt4.TabIndex = 3;
            this.ucLblTxt4.uiLbl = "lbl";
            this.ucLblTxt4.uitxt = "200";
            this.ucLblTxt4.valorDoubleNull = 200D;
            this.ucLblTxt4.valorMaximo = 1E+16D;
            this.ucLblTxt4.valorMinimo = 0D;
            this.ucLblTxt4.Visible = false;
            // 
            // ucLblTxt3
            // 
            this.ucLblTxt3.ancho = 75;
            this.ucLblTxt3.isEntero = false;
            this.ucLblTxt3.isNegativo = false;
            this.ucLblTxt3.isObligatorio = false;
            this.ucLblTxt3.isSimboloDecimalPunto = true;
            this.ucLblTxt3.location = new System.Drawing.Point(130, 0);
            this.ucLblTxt3.Location = new System.Drawing.Point(15, 19);
            this.ucLblTxt3.lonMax = 32767;
            this.ucLblTxt3.Name = "ucLblTxt3";
            this.ucLblTxt3.Size = new System.Drawing.Size(205, 20);
            this.ucLblTxt3.TabIndex = 2;
            this.ucLblTxt3.uiLbl = "lbl";
            this.ucLblTxt3.uitxt = "1000";
            this.ucLblTxt3.valorDoubleNull = 1000D;
            this.ucLblTxt3.valorMaximo = 1E+16D;
            this.ucLblTxt3.valorMinimo = 0D;
            // 
            // ucLblTxt2
            // 
            this.ucLblTxt2.ancho = 75;
            this.ucLblTxt2.isEntero = false;
            this.ucLblTxt2.isNegativo = false;
            this.ucLblTxt2.isObligatorio = true;
            this.ucLblTxt2.isSimboloDecimalPunto = true;
            this.ucLblTxt2.location = new System.Drawing.Point(130, 0);
            this.ucLblTxt2.Location = new System.Drawing.Point(215, 103);
            this.ucLblTxt2.lonMax = 32767;
            this.ucLblTxt2.Name = "ucLblTxt2";
            this.ucLblTxt2.Size = new System.Drawing.Size(205, 20);
            this.ucLblTxt2.TabIndex = 1;
            this.ucLblTxt2.uiLbl = "lbl";
            this.ucLblTxt2.uitxt = "1000";
            this.ucLblTxt2.valorDoubleNull = 1000D;
            this.ucLblTxt2.valorMaximo = 1E+16D;
            this.ucLblTxt2.valorMinimo = 0D;
            this.ucLblTxt2.Visible = false;
            // 
            // ucLblTxt1
            // 
            this.ucLblTxt1.ancho = 75;
            this.ucLblTxt1.isEntero = false;
            this.ucLblTxt1.isNegativo = false;
            this.ucLblTxt1.isObligatorio = true;
            this.ucLblTxt1.isSimboloDecimalPunto = true;
            this.ucLblTxt1.isTexto = true;
            this.ucLblTxt1.location = new System.Drawing.Point(130, 0);
            this.ucLblTxt1.Location = new System.Drawing.Point(116, 103);
            this.ucLblTxt1.lonMax = 32767;
            this.ucLblTxt1.Name = "ucLblTxt1";
            this.ucLblTxt1.Size = new System.Drawing.Size(205, 20);
            this.ucLblTxt1.TabIndex = 0;
            this.ucLblTxt1.uiLbl = "lbl";
            this.ucLblTxt1.uitxt = "111";
            this.ucLblTxt1.valorDoubleNull = 111D;
            this.ucLblTxt1.valorMaximo = 0D;
            this.ucLblTxt1.valorMinimo = 0D;
            this.ucLblTxt1.Visible = false;
            // 
            // ucCmbLayersTodas2
            // 
            this.ucCmbLayersTodas2.ancho = 147;
            this.ucCmbLayersTodas2.isObligatorio = false;
            this.ucCmbLayersTodas2.location = new System.Drawing.Point(162, 0);
            this.ucCmbLayersTodas2.Location = new System.Drawing.Point(57, 55);
            this.ucCmbLayersTodas2.Name = "ucCmbLayersTodas2";
            this.ucCmbLayersTodas2.Size = new System.Drawing.Size(309, 21);
            this.ucCmbLayersTodas2.TabIndex = 1;
            this.ucCmbLayersTodas2.uiLbl = "label1";
            // 
            // ucCmbLayersTodas1
            // 
            this.ucCmbLayersTodas1.ancho = 147;
            this.ucCmbLayersTodas1.isObligatorio = false;
            this.ucCmbLayersTodas1.location = new System.Drawing.Point(162, 0);
            this.ucCmbLayersTodas1.Location = new System.Drawing.Point(57, 28);
            this.ucCmbLayersTodas1.Name = "ucCmbLayersTodas1";
            this.ucCmbLayersTodas1.Size = new System.Drawing.Size(309, 21);
            this.ucCmbLayersTodas1.TabIndex = 0;
            this.ucCmbLayersTodas1.uiLbl = "label1";
            // 
            // frmManagerTerreno
            // 
            this.ClientSize = new System.Drawing.Size(464, 305);
            this.Controls.Add(this.buttonCrearTerreno);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmManagerTerreno";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmManagerTerreno";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button buttonCrearTerreno;
        private Button buttonInfo;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ucCmbLayersTodas ucCmbLayersTodas2;
        private ucCmbLayersTodas ucCmbLayersTodas1;
        private ucLblTxt ucLblTxt3;
        private ucLblTxt ucLblTxt2;
        private ucLblTxt ucLblTxt1;
        private ucLblTxt ucLblTxt4;
        private GroupBox groupBox4;
        private ucTerrain ucTerrain1;
        private Button button2;
        private ucLblTxt ucIntervaloLineasR;
        private ucLblTxt ucNodosPorHoja;
        private CheckBox chbCurvasRotura;
        private ucLblTxt ucLblTxtDistCurvNivel;
        private ucLblTxt ucLblTxtRangoBusqueda;
        private BackgroundWorker backgroundWorker1;
        private ProgressBar progressBar1;
    }
}