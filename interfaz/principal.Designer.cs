using MaterialSkin;
using MaterialSkin.Controls;
namespace interfaz
{
    using MaterialSkin;
    using MaterialSkin.Controls;
    partial class principal: MaterialForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(principal));
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.materialFlatButton5 = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialLabel11 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel10 = new MaterialSkin.Controls.MaterialLabel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.materialFlatButton1 = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel9 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel6 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel5 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel8 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel7 = new MaterialSkin.Controls.MaterialLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.nCurvasMaxTextField = new System.Windows.Forms.TextBox();
            this.materialLabel19 = new MaterialSkin.Controls.MaterialLabel();
            this.curvaGranRadioTextField = new System.Windows.Forms.TextBox();
            this.materialLabel18 = new MaterialSkin.Controls.MaterialLabel();
            this.clusterizacionTextField = new System.Windows.Forms.TextBox();
            this.materialLabel17 = new MaterialSkin.Controls.MaterialLabel();
            this.toleranciaMaximaTextField = new System.Windows.Forms.TextBox();
            this.materialLabel16 = new MaterialSkin.Controls.MaterialLabel();
            this.toleranciaMediaTextField = new System.Windows.Forms.TextBox();
            this.materialLabel15 = new MaterialSkin.Controls.MaterialLabel();
            this.materialFlatButton4 = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialRaisedButton1 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialLabel14 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel13 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel12 = new MaterialSkin.Controls.MaterialLabel();
            this.filtrado3ExecuteOrderNumericField = new System.Windows.Forms.NumericUpDown();
            this.filtrado2ExecuteOrderNumericField = new System.Windows.Forms.NumericUpDown();
            this.filtrado1ExecuteOrderNumericField = new System.Windows.Forms.NumericUpDown();
            this.aplicarMultiplesFiltradosCheckBox = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialCheckBox4 = new MaterialSkin.Controls.MaterialCheckBox();
            this.filtrado1CheckBox = new MaterialSkin.Controls.MaterialCheckBox();
            this.filtrado3MetrosTextField = new System.Windows.Forms.TextBox();
            this.filtrado2CheckBox = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialFlatButton3 = new MaterialSkin.Controls.MaterialFlatButton();
            this.filtrado3CheckBox = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialFlatButton2 = new MaterialSkin.Controls.MaterialFlatButton();
            this.filtrado3GradosTextField = new System.Windows.Forms.TextBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filtrado3ExecuteOrderNumericField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filtrado2ExecuteOrderNumericField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.filtrado1ExecuteOrderNumericField)).BeginInit();
            this.SuspendLayout();
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Location = new System.Drawing.Point(0, 64);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.materialTabSelector1.Size = new System.Drawing.Size(419, 23);
            this.materialTabSelector1.TabIndex = 1;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Location = new System.Drawing.Point(0, 93);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(847, 591);
            this.materialTabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.materialFlatButton5);
            this.tabPage1.Controls.Add(this.materialLabel11);
            this.tabPage1.Controls.Add(this.materialLabel10);
            this.tabPage1.Controls.Add(this.textBox3);
            this.tabPage1.Controls.Add(this.materialFlatButton1);
            this.tabPage1.Controls.Add(this.materialLabel3);
            this.tabPage1.Controls.Add(this.textBox4);
            this.tabPage1.Controls.Add(this.materialLabel4);
            this.tabPage1.Controls.Add(this.materialLabel9);
            this.tabPage1.Controls.Add(this.materialLabel6);
            this.tabPage1.Controls.Add(this.materialLabel5);
            this.tabPage1.Controls.Add(this.materialLabel8);
            this.tabPage1.Controls.Add(this.materialLabel7);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(839, 565);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // materialFlatButton5
            // 
            this.materialFlatButton5.AutoSize = true;
            this.materialFlatButton5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton5.Depth = 0;
            this.materialFlatButton5.Location = new System.Drawing.Point(330, 292);
            this.materialFlatButton5.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton5.Name = "materialFlatButton5";
            this.materialFlatButton5.Primary = false;
            this.materialFlatButton5.Size = new System.Drawing.Size(137, 36);
            this.materialFlatButton5.TabIndex = 22;
            this.materialFlatButton5.Text = "Cargar Polilinea";
            this.materialFlatButton5.UseVisualStyleBackColor = true;
            this.materialFlatButton5.Click += new System.EventHandler(this.materialFlatButton5_Click);
            // 
            // materialLabel11
            // 
            this.materialLabel11.AutoSize = true;
            this.materialLabel11.Depth = 0;
            this.materialLabel11.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel11.Location = new System.Drawing.Point(510, 29);
            this.materialLabel11.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel11.Name = "materialLabel11";
            this.materialLabel11.Size = new System.Drawing.Size(108, 19);
            this.materialLabel11.TabIndex = 14;
            this.materialLabel11.Text = "Puntos a filtrar";
            // 
            // materialLabel10
            // 
            this.materialLabel10.AutoSize = true;
            this.materialLabel10.Depth = 0;
            this.materialLabel10.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel10.Location = new System.Drawing.Point(21, 229);
            this.materialLabel10.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel10.Name = "materialLabel10";
            this.materialLabel10.Size = new System.Drawing.Size(57, 19);
            this.materialLabel10.TabIndex = 10;
            this.materialLabel10.Text = "Grados";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(330, 230);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 13;
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPruebaNumero_KeyPress);
            // 
            // materialFlatButton1
            // 
            this.materialFlatButton1.AutoSize = true;
            this.materialFlatButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton1.Depth = 0;
            this.materialFlatButton1.Location = new System.Drawing.Point(25, 292);
            this.materialFlatButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton1.Name = "materialFlatButton1";
            this.materialFlatButton1.Primary = false;
            this.materialFlatButton1.Size = new System.Drawing.Size(161, 36);
            this.materialFlatButton1.TabIndex = 0;
            this.materialFlatButton1.Text = "Estudio previo (.txt)";
            this.materialFlatButton1.UseVisualStyleBackColor = true;
            this.materialFlatButton1.Click += new System.EventHandler(this.materialFlatButton1_Click);
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(19, 73);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(455, 19);
            this.materialLabel3.TabIndex = 1;
            this.materialLabel3.Text = "Filtrado de puntos que producen una disrupción del sentido de giro";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(110, 230);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 12;
            this.textBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPruebaNumero_KeyPress);
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel4.Location = new System.Drawing.Point(548, 71);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(13, 19);
            this.materialLabel4.TabIndex = 2;
            this.materialLabel4.Text = "-";
            // 
            // materialLabel9
            // 
            this.materialLabel9.AutoSize = true;
            this.materialLabel9.Depth = 0;
            this.materialLabel9.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel9.Location = new System.Drawing.Point(256, 231);
            this.materialLabel9.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel9.Name = "materialLabel9";
            this.materialLabel9.Size = new System.Drawing.Size(57, 19);
            this.materialLabel9.TabIndex = 11;
            this.materialLabel9.Text = "Metros";
            // 
            // materialLabel6
            // 
            this.materialLabel6.AutoSize = true;
            this.materialLabel6.Depth = 0;
            this.materialLabel6.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel6.Location = new System.Drawing.Point(19, 127);
            this.materialLabel6.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel6.Name = "materialLabel6";
            this.materialLabel6.Size = new System.Drawing.Size(400, 19);
            this.materialLabel6.TabIndex = 3;
            this.materialLabel6.Text = "Filtrado de puntos con radio mínimo en cambio de sentido";
            // 
            // materialLabel5
            // 
            this.materialLabel5.AutoSize = true;
            this.materialLabel5.Depth = 0;
            this.materialLabel5.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel5.Location = new System.Drawing.Point(548, 126);
            this.materialLabel5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel5.Name = "materialLabel5";
            this.materialLabel5.Size = new System.Drawing.Size(13, 19);
            this.materialLabel5.TabIndex = 4;
            this.materialLabel5.Text = "-";
            // 
            // materialLabel8
            // 
            this.materialLabel8.AutoSize = true;
            this.materialLabel8.Depth = 0;
            this.materialLabel8.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel8.Location = new System.Drawing.Point(19, 184);
            this.materialLabel8.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel8.Name = "materialLabel8";
            this.materialLabel8.Size = new System.Drawing.Size(382, 19);
            this.materialLabel8.TabIndex = 5;
            this.materialLabel8.Text = "Filtrado de puntos que exceden la relación giro/longitud";
            // 
            // materialLabel7
            // 
            this.materialLabel7.AutoSize = true;
            this.materialLabel7.Depth = 0;
            this.materialLabel7.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel7.Location = new System.Drawing.Point(548, 182);
            this.materialLabel7.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel7.Name = "materialLabel7";
            this.materialLabel7.Size = new System.Drawing.Size(13, 19);
            this.materialLabel7.TabIndex = 6;
            this.materialLabel7.Text = "-";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.White;
            this.tabPage2.Controls.Add(this.nCurvasMaxTextField);
            this.tabPage2.Controls.Add(this.materialLabel19);
            this.tabPage2.Controls.Add(this.curvaGranRadioTextField);
            this.tabPage2.Controls.Add(this.materialLabel18);
            this.tabPage2.Controls.Add(this.clusterizacionTextField);
            this.tabPage2.Controls.Add(this.materialLabel17);
            this.tabPage2.Controls.Add(this.toleranciaMaximaTextField);
            this.tabPage2.Controls.Add(this.materialLabel16);
            this.tabPage2.Controls.Add(this.toleranciaMediaTextField);
            this.tabPage2.Controls.Add(this.materialLabel15);
            this.tabPage2.Controls.Add(this.materialFlatButton4);
            this.tabPage2.Controls.Add(this.materialRaisedButton1);
            this.tabPage2.Controls.Add(this.materialLabel14);
            this.tabPage2.Controls.Add(this.materialLabel13);
            this.tabPage2.Controls.Add(this.materialLabel12);
            this.tabPage2.Controls.Add(this.filtrado3ExecuteOrderNumericField);
            this.tabPage2.Controls.Add(this.filtrado2ExecuteOrderNumericField);
            this.tabPage2.Controls.Add(this.filtrado1ExecuteOrderNumericField);
            this.tabPage2.Controls.Add(this.aplicarMultiplesFiltradosCheckBox);
            this.tabPage2.Controls.Add(this.materialCheckBox4);
            this.tabPage2.Controls.Add(this.filtrado1CheckBox);
            this.tabPage2.Controls.Add(this.filtrado3MetrosTextField);
            this.tabPage2.Controls.Add(this.filtrado2CheckBox);
            this.tabPage2.Controls.Add(this.materialFlatButton3);
            this.tabPage2.Controls.Add(this.filtrado3CheckBox);
            this.tabPage2.Controls.Add(this.materialFlatButton2);
            this.tabPage2.Controls.Add(this.filtrado3GradosTextField);
            this.tabPage2.Controls.Add(this.materialLabel1);
            this.tabPage2.Controls.Add(this.materialLabel2);
            this.tabPage2.Controls.Add(this.materialDivider1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(839, 565);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // nCurvasMaxTextField
            // 
            this.nCurvasMaxTextField.Location = new System.Drawing.Point(151, 402);
            this.nCurvasMaxTextField.Name = "nCurvasMaxTextField";
            this.nCurvasMaxTextField.Size = new System.Drawing.Size(100, 20);
            this.nCurvasMaxTextField.TabIndex = 31;
            this.nCurvasMaxTextField.Text = "2";
            // 
            // materialLabel19
            // 
            this.materialLabel19.AutoSize = true;
            this.materialLabel19.Depth = 0;
            this.materialLabel19.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel19.Location = new System.Drawing.Point(20, 401);
            this.materialLabel19.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel19.Name = "materialLabel19";
            this.materialLabel19.Size = new System.Drawing.Size(109, 19);
            this.materialLabel19.TabIndex = 30;
            this.materialLabel19.Text = "Nº Curvas Max";
            // 
            // curvaGranRadioTextField
            // 
            this.curvaGranRadioTextField.Location = new System.Drawing.Point(400, 402);
            this.curvaGranRadioTextField.Name = "curvaGranRadioTextField";
            this.curvaGranRadioTextField.Size = new System.Drawing.Size(100, 20);
            this.curvaGranRadioTextField.TabIndex = 29;
            this.curvaGranRadioTextField.Text = "2500";
            // 
            // materialLabel18
            // 
            this.materialLabel18.AutoSize = true;
            this.materialLabel18.Depth = 0;
            this.materialLabel18.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel18.Location = new System.Drawing.Point(257, 401);
            this.materialLabel18.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel18.Name = "materialLabel18";
            this.materialLabel18.Size = new System.Drawing.Size(138, 19);
            this.materialLabel18.TabIndex = 28;
            this.materialLabel18.Text = "Curva de gran radio";
            // 
            // clusterizacionTextField
            // 
            this.clusterizacionTextField.Location = new System.Drawing.Point(650, 458);
            this.clusterizacionTextField.Name = "clusterizacionTextField";
            this.clusterizacionTextField.Size = new System.Drawing.Size(100, 20);
            this.clusterizacionTextField.TabIndex = 27;
            this.clusterizacionTextField.Text = "2";
            // 
            // materialLabel17
            // 
            this.materialLabel17.AutoSize = true;
            this.materialLabel17.Depth = 0;
            this.materialLabel17.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel17.Location = new System.Drawing.Point(506, 457);
            this.materialLabel17.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel17.Name = "materialLabel17";
            this.materialLabel17.Size = new System.Drawing.Size(138, 19);
            this.materialLabel17.TabIndex = 26;
            this.materialLabel17.Text = "% de clusterización";
            // 
            // toleranciaMaximaTextField
            // 
            this.toleranciaMaximaTextField.Location = new System.Drawing.Point(400, 458);
            this.toleranciaMaximaTextField.Name = "toleranciaMaximaTextField";
            this.toleranciaMaximaTextField.Size = new System.Drawing.Size(100, 20);
            this.toleranciaMaximaTextField.TabIndex = 25;
            this.toleranciaMaximaTextField.Text = "1";
            // 
            // materialLabel16
            // 
            this.materialLabel16.AutoSize = true;
            this.materialLabel16.Depth = 0;
            this.materialLabel16.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel16.Location = new System.Drawing.Point(257, 457);
            this.materialLabel16.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel16.Name = "materialLabel16";
            this.materialLabel16.Size = new System.Drawing.Size(137, 19);
            this.materialLabel16.TabIndex = 24;
            this.materialLabel16.Text = "Tolerancia máxima";
            // 
            // toleranciaMediaTextField
            // 
            this.toleranciaMediaTextField.Location = new System.Drawing.Point(151, 458);
            this.toleranciaMediaTextField.Name = "toleranciaMediaTextField";
            this.toleranciaMediaTextField.Size = new System.Drawing.Size(100, 20);
            this.toleranciaMediaTextField.TabIndex = 23;
            this.toleranciaMediaTextField.Text = "1";
            // 
            // materialLabel15
            // 
            this.materialLabel15.AutoSize = true;
            this.materialLabel15.Depth = 0;
            this.materialLabel15.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel15.Location = new System.Drawing.Point(20, 457);
            this.materialLabel15.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel15.Name = "materialLabel15";
            this.materialLabel15.Size = new System.Drawing.Size(125, 19);
            this.materialLabel15.TabIndex = 22;
            this.materialLabel15.Text = "Tolerancia media";
            // 
            // materialFlatButton4
            // 
            this.materialFlatButton4.AutoSize = true;
            this.materialFlatButton4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton4.Depth = 0;
            this.materialFlatButton4.Location = new System.Drawing.Point(510, 505);
            this.materialFlatButton4.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton4.Name = "materialFlatButton4";
            this.materialFlatButton4.Primary = false;
            this.materialFlatButton4.Size = new System.Drawing.Size(137, 36);
            this.materialFlatButton4.TabIndex = 21;
            this.materialFlatButton4.Text = "Cargar Polilinea";
            this.materialFlatButton4.UseVisualStyleBackColor = true;
            this.materialFlatButton4.Click += new System.EventHandler(this.materialFlatButton4_Click);
            // 
            // materialRaisedButton1
            // 
            this.materialRaisedButton1.Depth = 0;
            this.materialRaisedButton1.Location = new System.Drawing.Point(24, 233);
            this.materialRaisedButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton1.Name = "materialRaisedButton1";
            this.materialRaisedButton1.Primary = true;
            this.materialRaisedButton1.Size = new System.Drawing.Size(75, 23);
            this.materialRaisedButton1.TabIndex = 20;
            this.materialRaisedButton1.Text = "Info";
            this.materialRaisedButton1.UseVisualStyleBackColor = true;
            this.materialRaisedButton1.Click += new System.EventHandler(this.materialRaisedButton1_Click);
            // 
            // materialLabel14
            // 
            this.materialLabel14.AutoSize = true;
            this.materialLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel14.Depth = 0;
            this.materialLabel14.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel14.Location = new System.Drawing.Point(255, 278);
            this.materialLabel14.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel14.Name = "materialLabel14";
            this.materialLabel14.Size = new System.Drawing.Size(73, 19);
            this.materialLabel14.TabIndex = 18;
            this.materialLabel14.Text = "Método 3";
            // 
            // materialLabel13
            // 
            this.materialLabel13.AutoSize = true;
            this.materialLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel13.Depth = 0;
            this.materialLabel13.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel13.Location = new System.Drawing.Point(255, 234);
            this.materialLabel13.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel13.Name = "materialLabel13";
            this.materialLabel13.Size = new System.Drawing.Size(73, 19);
            this.materialLabel13.TabIndex = 17;
            this.materialLabel13.Text = "Método 2";
            // 
            // materialLabel12
            // 
            this.materialLabel12.AutoSize = true;
            this.materialLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel12.Depth = 0;
            this.materialLabel12.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel12.Location = new System.Drawing.Point(255, 192);
            this.materialLabel12.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel12.Name = "materialLabel12";
            this.materialLabel12.Size = new System.Drawing.Size(73, 19);
            this.materialLabel12.TabIndex = 16;
            this.materialLabel12.Text = "Método 1";
            // 
            // filtrado3ExecuteOrderNumericField
            // 
            this.filtrado3ExecuteOrderNumericField.Location = new System.Drawing.Point(392, 277);
            this.filtrado3ExecuteOrderNumericField.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.filtrado3ExecuteOrderNumericField.Name = "filtrado3ExecuteOrderNumericField";
            this.filtrado3ExecuteOrderNumericField.Size = new System.Drawing.Size(52, 20);
            this.filtrado3ExecuteOrderNumericField.TabIndex = 15;
            // 
            // filtrado2ExecuteOrderNumericField
            // 
            this.filtrado2ExecuteOrderNumericField.Location = new System.Drawing.Point(392, 233);
            this.filtrado2ExecuteOrderNumericField.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.filtrado2ExecuteOrderNumericField.Name = "filtrado2ExecuteOrderNumericField";
            this.filtrado2ExecuteOrderNumericField.Size = new System.Drawing.Size(52, 20);
            this.filtrado2ExecuteOrderNumericField.TabIndex = 14;
            // 
            // filtrado1ExecuteOrderNumericField
            // 
            this.filtrado1ExecuteOrderNumericField.Location = new System.Drawing.Point(392, 191);
            this.filtrado1ExecuteOrderNumericField.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.filtrado1ExecuteOrderNumericField.Name = "filtrado1ExecuteOrderNumericField";
            this.filtrado1ExecuteOrderNumericField.Size = new System.Drawing.Size(52, 20);
            this.filtrado1ExecuteOrderNumericField.TabIndex = 13;
            // 
            // aplicarMultiplesFiltradosCheckBox
            // 
            this.aplicarMultiplesFiltradosCheckBox.AutoSize = true;
            this.aplicarMultiplesFiltradosCheckBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.aplicarMultiplesFiltradosCheckBox.Depth = 0;
            this.aplicarMultiplesFiltradosCheckBox.Font = new System.Drawing.Font("Roboto", 10F);
            this.aplicarMultiplesFiltradosCheckBox.Location = new System.Drawing.Point(24, 188);
            this.aplicarMultiplesFiltradosCheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.aplicarMultiplesFiltradosCheckBox.MouseLocation = new System.Drawing.Point(-1, -1);
            this.aplicarMultiplesFiltradosCheckBox.MouseState = MaterialSkin.MouseState.HOVER;
            this.aplicarMultiplesFiltradosCheckBox.Name = "aplicarMultiplesFiltradosCheckBox";
            this.aplicarMultiplesFiltradosCheckBox.Ripple = true;
            this.aplicarMultiplesFiltradosCheckBox.Size = new System.Drawing.Size(180, 30);
            this.aplicarMultiplesFiltradosCheckBox.TabIndex = 12;
            this.aplicarMultiplesFiltradosCheckBox.Text = "Ejecutar mas de un filtro";
            this.aplicarMultiplesFiltradosCheckBox.UseVisualStyleBackColor = false;
            this.aplicarMultiplesFiltradosCheckBox.Click += new System.EventHandler(this.materialCheckBox5_Click);
            // 
            // materialCheckBox4
            // 
            this.materialCheckBox4.AutoSize = true;
            this.materialCheckBox4.Depth = 0;
            this.materialCheckBox4.Font = new System.Drawing.Font("Roboto", 10F);
            this.materialCheckBox4.Location = new System.Drawing.Point(24, 344);
            this.materialCheckBox4.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckBox4.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckBox4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckBox4.Name = "materialCheckBox4";
            this.materialCheckBox4.Ripple = true;
            this.materialCheckBox4.Size = new System.Drawing.Size(287, 30);
            this.materialCheckBox4.TabIndex = 10;
            this.materialCheckBox4.Text = "Iterar hasta que no haya puntos que filtrar";
            this.materialCheckBox4.UseVisualStyleBackColor = true;
            // 
            // filtrado1CheckBox
            // 
            this.filtrado1CheckBox.AutoSize = true;
            this.filtrado1CheckBox.Depth = 0;
            this.filtrado1CheckBox.Font = new System.Drawing.Font("Roboto", 10F);
            this.filtrado1CheckBox.Location = new System.Drawing.Point(24, 24);
            this.filtrado1CheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.filtrado1CheckBox.MouseLocation = new System.Drawing.Point(-1, -1);
            this.filtrado1CheckBox.MouseState = MaterialSkin.MouseState.HOVER;
            this.filtrado1CheckBox.Name = "filtrado1CheckBox";
            this.filtrado1CheckBox.Ripple = true;
            this.filtrado1CheckBox.Size = new System.Drawing.Size(359, 30);
            this.filtrado1CheckBox.TabIndex = 2;
            this.filtrado1CheckBox.Text = "Método 1 - Filtrado por disrupción del sentido del giro";
            this.filtrado1CheckBox.UseVisualStyleBackColor = true;
            this.filtrado1CheckBox.Click += new System.EventHandler(this.materialCheckBox1_Click);
            // 
            // filtrado3MetrosTextField
            // 
            this.filtrado3MetrosTextField.Location = new System.Drawing.Point(668, 135);
            this.filtrado3MetrosTextField.Name = "filtrado3MetrosTextField";
            this.filtrado3MetrosTextField.Size = new System.Drawing.Size(100, 20);
            this.filtrado3MetrosTextField.TabIndex = 9;
            this.filtrado3MetrosTextField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPruebaNumero_KeyPress);
            // 
            // filtrado2CheckBox
            // 
            this.filtrado2CheckBox.AutoSize = true;
            this.filtrado2CheckBox.Depth = 0;
            this.filtrado2CheckBox.Font = new System.Drawing.Font("Roboto", 10F);
            this.filtrado2CheckBox.Location = new System.Drawing.Point(24, 79);
            this.filtrado2CheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.filtrado2CheckBox.MouseLocation = new System.Drawing.Point(-1, -1);
            this.filtrado2CheckBox.MouseState = MaterialSkin.MouseState.HOVER;
            this.filtrado2CheckBox.Name = "filtrado2CheckBox";
            this.filtrado2CheckBox.Ripple = true;
            this.filtrado2CheckBox.Size = new System.Drawing.Size(457, 30);
            this.filtrado2CheckBox.TabIndex = 3;
            this.filtrado2CheckBox.Text = "Método 2 - Filtrado de puntos con radio mínimo en cambio de sentido";
            this.filtrado2CheckBox.UseVisualStyleBackColor = true;
            this.filtrado2CheckBox.Click += new System.EventHandler(this.materialCheckBox2_Click);
            // 
            // materialFlatButton3
            // 
            this.materialFlatButton3.AutoSize = true;
            this.materialFlatButton3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton3.Depth = 0;
            this.materialFlatButton3.Location = new System.Drawing.Point(261, 505);
            this.materialFlatButton3.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton3.Name = "materialFlatButton3";
            this.materialFlatButton3.Primary = false;
            this.materialFlatButton3.Size = new System.Drawing.Size(185, 36);
            this.materialFlatButton3.TabIndex = 1;
            this.materialFlatButton3.Text = "Cargar Puntos (propio)";
            this.materialFlatButton3.UseVisualStyleBackColor = true;
            this.materialFlatButton3.Click += new System.EventHandler(this.materialFlatButton3_Click);
            // 
            // filtrado3CheckBox
            // 
            this.filtrado3CheckBox.AutoSize = true;
            this.filtrado3CheckBox.Depth = 0;
            this.filtrado3CheckBox.Font = new System.Drawing.Font("Roboto", 10F);
            this.filtrado3CheckBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.filtrado3CheckBox.Location = new System.Drawing.Point(24, 135);
            this.filtrado3CheckBox.Margin = new System.Windows.Forms.Padding(0);
            this.filtrado3CheckBox.MouseLocation = new System.Drawing.Point(-1, -1);
            this.filtrado3CheckBox.MouseState = MaterialSkin.MouseState.HOVER;
            this.filtrado3CheckBox.Name = "filtrado3CheckBox";
            this.filtrado3CheckBox.Ripple = true;
            this.filtrado3CheckBox.Size = new System.Drawing.Size(443, 30);
            this.filtrado3CheckBox.TabIndex = 4;
            this.filtrado3CheckBox.Text = "Método 3 - Filtrado de puntos que exceden la relación giro/longitud";
            this.filtrado3CheckBox.UseVisualStyleBackColor = true;
            this.filtrado3CheckBox.Click += new System.EventHandler(this.materialCheckBox3_Click);
            // 
            // materialFlatButton2
            // 
            this.materialFlatButton2.AutoSize = true;
            this.materialFlatButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton2.Depth = 0;
            this.materialFlatButton2.Location = new System.Drawing.Point(24, 505);
            this.materialFlatButton2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialFlatButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton2.Name = "materialFlatButton2";
            this.materialFlatButton2.Primary = false;
            this.materialFlatButton2.Size = new System.Drawing.Size(165, 36);
            this.materialFlatButton2.TabIndex = 0;
            this.materialFlatButton2.Text = "Cargar Puntos (.txt)";
            this.materialFlatButton2.UseVisualStyleBackColor = true;
            this.materialFlatButton2.Click += new System.EventHandler(this.materialFlatButton2_Click);
            // 
            // filtrado3GradosTextField
            // 
            this.filtrado3GradosTextField.Location = new System.Drawing.Point(669, 84);
            this.filtrado3GradosTextField.Name = "filtrado3GradosTextField";
            this.filtrado3GradosTextField.Size = new System.Drawing.Size(100, 20);
            this.filtrado3GradosTextField.TabIndex = 8;
            this.filtrado3GradosTextField.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtPruebaNumero_KeyPress);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(580, 83);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(57, 19);
            this.materialLabel1.TabIndex = 5;
            this.materialLabel1.Text = "Grados";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(579, 134);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(57, 19);
            this.materialLabel2.TabIndex = 6;
            this.materialLabel2.Text = "Metros";
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(0, 168);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(776, 145);
            this.materialDivider1.TabIndex = 11;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 687);
            this.Controls.Add(this.materialTabControl1);
            this.Controls.Add(this.materialTabSelector1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "APLITOP";
            this.Load += new System.EventHandler(this.principal_Load);
            this.ResizeEnd += new System.EventHandler(this.principal_ResizeEnd);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filtrado3ExecuteOrderNumericField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filtrado2ExecuteOrderNumericField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.filtrado1ExecuteOrderNumericField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialTabSelector materialTabSelector1;
        private MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MaterialFlatButton materialFlatButton2;
        private MaterialFlatButton materialFlatButton3;
        private MaterialCheckBox filtrado3CheckBox;
        private MaterialCheckBox filtrado2CheckBox;
        private MaterialCheckBox filtrado1CheckBox;
        private System.Windows.Forms.TextBox filtrado3MetrosTextField;
        private System.Windows.Forms.TextBox filtrado3GradosTextField;
        private MaterialLabel materialLabel2;
        private MaterialLabel materialLabel1;
        private MaterialLabel materialLabel7;
        private MaterialLabel materialLabel8;
        private MaterialLabel materialLabel5;
        private MaterialLabel materialLabel6;
        private MaterialLabel materialLabel4;
        private MaterialLabel materialLabel3;
        private MaterialFlatButton materialFlatButton1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private MaterialLabel materialLabel9;
        private MaterialLabel materialLabel10;
        private MaterialLabel materialLabel11;
        private MaterialLabel materialLabel14;
        private MaterialLabel materialLabel13;
        private MaterialLabel materialLabel12;
        private System.Windows.Forms.NumericUpDown filtrado3ExecuteOrderNumericField;
        private System.Windows.Forms.NumericUpDown filtrado2ExecuteOrderNumericField;
        private System.Windows.Forms.NumericUpDown filtrado1ExecuteOrderNumericField;
        private MaterialCheckBox aplicarMultiplesFiltradosCheckBox;
        private MaterialCheckBox materialCheckBox4;
        private MaterialDivider materialDivider1;
        private MaterialRaisedButton materialRaisedButton1;
        private MaterialFlatButton materialFlatButton4;
        private MaterialFlatButton materialFlatButton5;
        private System.Windows.Forms.TextBox toleranciaMaximaTextField;
        private MaterialLabel materialLabel16;
        private System.Windows.Forms.TextBox toleranciaMediaTextField;
        private MaterialLabel materialLabel15;
        private System.Windows.Forms.TextBox clusterizacionTextField;
        private MaterialLabel materialLabel17;
        private System.Windows.Forms.TextBox curvaGranRadioTextField;
        private MaterialLabel materialLabel18;
        private System.Windows.Forms.TextBox nCurvasMaxTextField;
        private MaterialLabel materialLabel19;
    }
}