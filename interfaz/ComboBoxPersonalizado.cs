using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace interfaz
{
    [DefaultEvent("OnSelectedIndexChanged")]
    public class ComboBoxPersonalizado : UserControl
    {
        // Campos
        private Color backColor = Color.White;
        private Color iconColor = Color.MediumSlateBlue;
        private Color listBackColor = Color.FromArgb(230, 228, 245);
        private Color listTextColor = Color.DimGray;
        private Color borderColor = Color.MediumSlateBlue;
        private int borderSize = 1;

        // Elementos interactivos
        private ComboBox cmbList;
        private Label lblText;
        private Button btnIcon;

        // Eventos
        public event EventHandler SelectedIndexChanged; // Evento por defecto

        // Constructor
        public ComboBoxPersonalizado()
        {
            cmbList = new ComboBox();
            lblText = new Label();
            btnIcon = new Button();
            this.SuspendLayout();

            // Configurar Combobox
            cmbList.BackColor = listBackColor;
            cmbList.Font = new Font(this.Font.Name, 10F);
            cmbList.ForeColor = listTextColor;
            cmbList.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            cmbList.TextChanged += new EventHandler(ComboBox_TextChanged);

            // Button: Icono del Dropdown
            btnIcon.Dock = DockStyle.Right;
            btnIcon.FlatStyle = FlatStyle.Flat;
            btnIcon.FlatAppearance.BorderSize = 0;
            btnIcon.BackColor = backColor;
            btnIcon.Size = new Size(30, 30);
            btnIcon.Cursor = Cursors.Hand;
            btnIcon.Click += new EventHandler(Icon_Click);
            btnIcon.Paint += new PaintEventHandler(Icon_Paint);

            // Label: Texto mostrado
            lblText.Dock = DockStyle.Fill;
            lblText.AutoSize = false;
            lblText.BackColor = backColor;
            lblText.TextAlign = ContentAlignment.MiddleLeft;
            lblText.Padding = new Padding(8, 0, 0, 0);
            lblText.Font = new Font(this.Font.Name, 9.5F);
            lblText.Click += new EventHandler(Surface_Click);

            // Configurar este UserControl
            this.Controls.Add(lblText);
            this.Controls.Add(btnIcon);
            this.Controls.Add(cmbList);
            this.MinimumSize = new Size(50, 30);
            this.Size = new Size(100, 30);
            this.ForeColor = Color.DimGray;
            this.Padding = new Padding(borderSize);
            this.BackColor = borderColor;
            this.ResumeLayout();
            AdjustComboBoxDimensions();
        }

        // Propiedades de la UI

        [Category("Boxpersonal")]
        public new Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                lblText.BackColor = backColor;
                btnIcon.BackColor = backColor;
            }
        }

        [Category("Boxpersonal")]
        public Color IconColor
        {
            get { return iconColor; }
            set
            {
                iconColor = value;
                btnIcon.Invalidate(); // Redibujar icono
            }
        }

        [Category("Boxpersonal")]
        public Color ListBackColor
        {
            get { return listBackColor; }
            set
            {
                listBackColor = value;
                cmbList.BackColor = listBackColor;
            }
        }

        [Category("Boxpersonal")]
        public Color ListTextColor
        {
            get { return listTextColor; }
            set
            {
                listTextColor = value;
                cmbList.ForeColor = listTextColor;
            }
        }

        [Category("Boxpersonal")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                base.BackColor = borderColor; // Border Color
            }
        }

        [Category("Boxpersonal")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Padding = new Padding(borderSize); // Padding hace la función del grosor del borde
                AdjustComboBoxDimensions();
            }
        }

        [Category("Boxpersonal")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                lblText.ForeColor = value;
            }
        }

        [Category("Boxpersonal")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                lblText.Font = value;
                cmbList.Font = value; 
            }
        }

        [Category("Boxpersonal")]
        public string Texts
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        [Category("Boxpersonal")]
        public ComboBoxStyle DropDownStyle
        {
            get { return cmbList.DropDownStyle; }
            set
            {
                if (cmbList.DropDownStyle != ComboBoxStyle.Simple)
                    cmbList.DropDownStyle = value;
            }
        }

        // Propiedades de Datos / Integración con el ComboBox original

        public ComboBox.ObjectCollection Items
        {
            get { return cmbList.Items; }
        }

        public object DataSource
        {
            get { return cmbList.DataSource; }
            set { cmbList.DataSource = value; }
        }

        public string DisplayMember
        {
            get { return cmbList.DisplayMember; }
            set { cmbList.DisplayMember = value; }
        }

        public string ValueMember
        {
            get { return cmbList.ValueMember; }
            set { cmbList.ValueMember = value; }
        }

        public int SelectedIndex
        {
            get { return cmbList.SelectedIndex; }
            set { cmbList.SelectedIndex = value; }
        }

        public object SelectedValue
        {
            get { return cmbList.SelectedValue; }
            set { cmbList.SelectedValue = value; }
        }

        public object SelectedItem
        {
            get { return cmbList.SelectedItem; }
            set { cmbList.SelectedItem = value; }
        }

        [Category("Boxpersonal")]
        public bool FormattingEnabled
        {
            get { return cmbList.FormattingEnabled; }
            set { cmbList.FormattingEnabled = value; }
        }

        [Category("Boxpersonal")]
        public int MaxDropDownItems
        {
            get { return cmbList.MaxDropDownItems; }
            set { cmbList.MaxDropDownItems = value; }
        }

        [Category("Boxpersonal")]
        public int DropDownWidth
        {
            get { return cmbList.DropDownWidth; }
            set { cmbList.DropDownWidth = value; }
        }


        // Métodos de comportamiento
        private void AdjustComboBoxDimensions()
        {
            cmbList.Width = lblText.Width;
            cmbList.Location = new Point()
            {
                X = this.Width - this.Padding.Right - cmbList.Width,
                Y = lblText.Bottom - cmbList.Height
            };
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged.Invoke(sender, e);
            
            lblText.Text = cmbList.Text;
        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            // Opcional, para que el label refleje lo del combobox
            lblText.Text = cmbList.Text;
        }

        private void Icon_Click(object sender, EventArgs e)
        {
            // Abrir Dropdown
            cmbList.Select();
            cmbList.DroppedDown = true;
        }

        private void Surface_Click(object sender, EventArgs e)
        {
            // Adjuntar este método a label, panel, etc.
            cmbList.Select();
            if (cmbList.DropDownStyle == ComboBoxStyle.DropDownList)
                cmbList.DroppedDown = true;
        }

        private void Icon_Paint(object sender, PaintEventArgs e)
        {
            // Pintar el triángulo del dropdown
            int iconWidth = 14;
            int iconHeight = 6;
            var rectIcon = new Rectangle((btnIcon.Width - iconWidth) / 2, (btnIcon.Height - iconHeight) / 2, iconWidth, iconHeight);
            Graphics graph = e.Graphics;

            // Dibujar triángulo inversor (hacia abajo)
            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(iconColor, 2))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                path.AddLine(rectIcon.X, rectIcon.Y, rectIcon.X + (iconWidth / 2), rectIcon.Bottom);
                path.AddLine(rectIcon.X + (iconWidth / 2), rectIcon.Bottom, rectIcon.Right, rectIcon.Y);
                graph.DrawPath(pen, path);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustComboBoxDimensions();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ComboBoxPersonalizado
            // 
            this.Name = "ComboBoxPersonalizado";
            this.Size = new System.Drawing.Size(89, 150);
            this.ResumeLayout(false);

        }
    }
}
