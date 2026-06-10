using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace interfaz
{
    public class BotonPersonalizado : Button
    {
        // Propiedades de apariencia
        public int BorderRadius { get; set; } = 10;
        public Color ColorSolidoBase { get; set; } = Color.LightGray;
        public Color ColorSolidoHover { get; set; } = Color.Silver;
        public Color ColorBorde { get; set; } = Color.DarkGray;
        public int GrosorBorde { get; set; } = 1;

        // Propiedades para gradiente (si se quiere usar en lugar de color sólido)
        public bool UsaGradiente { get; set; } = false;
        public Color GradienteColorInicio { get; set; } = Color.FromArgb(70, 110, 160);
        public Color GradienteColorFin { get; set; } = Color.FromArgb(50, 80, 120);
        public Color GradienteHoverInicio { get; set; } = Color.FromArgb(80, 120, 170);
        public Color GradienteHoverFin { get; set; } = Color.FromArgb(60, 90, 130);
        public LinearGradientMode ModoGradiente { get; set; } = LinearGradientMode.Vertical;

        // Sombra
        public bool Sombrear { get; set; } = true;
        public int SombraDesplazamiento { get; set; } = 3;

        private bool IsHovered = false;
        private bool IsPressed = false;

        public BotonPersonalizado()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.Transparent;
            this.Cursor = Cursors.Hand;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            IsHovered = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            IsHovered = false;
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                IsPressed = true;
                this.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                IsPressed = false;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            // No llamar a base.OnPaint(pevent) para tener control total
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Limpiar el fondo con el color del contenedor padre para evitar esquinas negras
            if (this.Parent != null)
            {
                using (SolidBrush bgBrush = new SolidBrush(this.Parent.BackColor))
                {
                    g.FillRectangle(bgBrush, this.ClientRectangle);
                }
            }

            Rectangle rectArea = new Rectangle(0, 0, this.Width - GrosorBorde, this.Height - GrosorBorde);
            Rectangle recInterior = new Rectangle(1, 1, this.Width - 2 - GrosorBorde, this.Height - 2 - GrosorBorde);

            // Ajuste por pulsación
            if (IsPressed)
            {
                rectArea.Y += 1;
                rectArea.Height -= 1;
                recInterior.Y += 1;
                recInterior.Height -= 1;
            }

            // Crear ruta redondeada
            using (GraphicsPath pathArea = GetRoundPath(rectArea, BorderRadius))
            using (GraphicsPath pathInterior = GetRoundPath(recInterior, BorderRadius))
            {
                // 1. Dibujar sombra (Si no está presionado)
                if (Sombrear && !IsPressed)
                {
                    Rectangle rectSombra = rectArea;
                    rectSombra.Y += SombraDesplazamiento;
                    using (GraphicsPath pathSombra = GetRoundPath(rectSombra, BorderRadius))
                    using (Brush brushSombra = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                    {
                        g.FillPath(brushSombra, pathSombra);
                    }
                }

                // 2. Fondo
                Color colorFondo;
                if (UsaGradiente)
                {
                    Color inicio = IsHovered ? GradienteHoverInicio : GradienteColorInicio;
                    Color fin = IsHovered ? GradienteHoverFin : GradienteColorFin;
                    using (LinearGradientBrush lgb = new LinearGradientBrush(rectArea, inicio, fin, ModoGradiente))
                    {
                        g.FillPath(lgb, pathArea);
                    }
                }
                else
                {
                    colorFondo = IsHovered ? ColorSolidoHover : ColorSolidoBase;
                    using (SolidBrush brushFondo = new SolidBrush(colorFondo))
                    {
                        g.FillPath(brushFondo, pathArea);
                    }
                }

                // 3. Borde superior 3D suave
                if (!IsPressed)
                {
                    using (Pen penTop = new Pen(Color.FromArgb(50, Color.White), 1))
                    {
                        g.DrawPath(penTop, pathInterior);
                    }
                }

                // 4. Borde Principal
                if (GrosorBorde > 0)
                {
                    using (Pen penBorde = new Pen(ColorBorde, GrosorBorde))
                    {
                        g.DrawPath(penBorde, pathArea);
                    }
                }

                // 5. Imagen y Texto
                Rectangle textoRect = rectArea;
                if (this.Image != null)
                {
                    // Posicionamiento de la imagen a la izquierda
                    int margin = 10;
                    int imgX = margin;
                    int imgY = (this.Height - this.Image.Height) / 2;
                    
                    if (IsPressed)
                    {
                        imgX += 1;
                        imgY += 1;
                    }

                    g.DrawImage(this.Image, imgX, imgY, this.Image.Width, this.Image.Height);

                    // El texto se dibuja a la derecha de la imagen
                    int offsetX = imgX + this.Image.Width + 5;
                    textoRect.X += offsetX;
                    textoRect.Width -= (offsetX + margin);
                }

                // 6. Dibujo del Texto
                TextRenderer.DrawText(g, this.Text, this.Font, textoRect, this.ForeColor, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
            }
        }

        private GraphicsPath GetRoundPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(rect.X, rect.Y, diameter, diameter);

            // Esquina superior izquierda
            path.AddArc(arc, 180, 90);
            
            // Esquina superior derecha
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);
            
            // Esquina inferior derecha
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            
            // Esquina inferior izquierda
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);
            
            path.CloseFigure();
            return path;
        }
    }
}
