using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;
namespace interfaz
{
    public partial class EditarParametros : Form
    {
        public Logica.Componente c  { get; private set; }

        public EditarParametros()
        {
            InitializeComponent();
        }

        public void CargarDatos(Logica.Componente c)
        {
            if (c.Tipo==1)
            {
                label1.Text = "RECTA";
                textBox_cx.Enabled = false;
                textBox_cy.Enabled = false;
                textBox_r.Enabled = false;
            }
            if (c.Tipo == 2)
            {
                label1.Text = "CURVA";
                textBox_cx.Enabled = true;
                textBox_cy.Enabled = true;
                textBox_r.Enabled = true;

            }
            if (c.Tipo == 3)
            {
                label1.Text = "CLOTOIDE";
                textBox_cx.Enabled = false;
                textBox_cy.Enabled = false;
                textBox_r.Enabled = false;

            }

            textBox_a.Text = c.azr.ToString();
            textBox_cx.Text=c.xc.ToString();
            textBox_cy.Text = c.yc.ToString();
            textBox_pfx.Text = c.lista_puntos[c.lista_puntos.Count() - 1].p.X.ToString();
            textBox_pfy.Text = c.lista_puntos[c.lista_puntos.Count() - 1].p.Y.ToString();
            textBox_pix.Text = c.lista_puntos[0].p.X.ToString();
            textBox_piy.Text = c.lista_puntos[0].p.Y.ToString();
            textBox_r.Text = c.radio.ToString();

            textBox_a.Enabled = false;
            

        }
        private void Aceptar_Click(object sender, EventArgs e)
        {
            CalculoPolilinea calculoPolilinea = new CalculoPolilinea();
            c = new Componente();
            if (label1.Text=="RECTA")
            {
                c.lista_puntos = new List<Logica.Punto>();
                c.lista_puntos.Add(new Logica.Punto(new Autodesk.AutoCAD.Geometry.Point2d(double.Parse(textBox_pix.Text), double.Parse(textBox_piy.Text))));
                c.lista_puntos.Add(new Logica.Punto(new Autodesk.AutoCAD.Geometry.Point2d(double.Parse(textBox_pfx.Text), double.Parse(textBox_pfy.Text))));
                calculoPolilinea.Rellenar_Recta(c);
                c.Tipo = 1;
            }
            if (label1.Text == "CURVA")
            {
                c.radio = double.Parse(textBox_r.Text);
                c.xc = double.Parse(textBox_cx.Text);
                c.yc = double.Parse(textBox_cy.Text);
                c.lista_puntos = null;
                c.lista_puntos = new List<Logica.Punto>();
                Logica.Punto p1 = new Logica.Punto(new Autodesk.AutoCAD.Geometry.Point2d(double.Parse(textBox_pix.Text), double.Parse(textBox_piy.Text)));
                Logica.Punto p3 = new Logica.Punto(new Autodesk.AutoCAD.Geometry.Point2d(double.Parse(textBox_pfx.Text), double.Parse(textBox_pfy.Text)));
                Logica.Punto p2 = calculoPolilinea.Crear_Curva_2Puntos_Modificada(p1, p3, c.xc, c.yc, c.radio, c);
                c.lista_puntos.Add(p1);
                c.lista_puntos.Add(p2);
                c.lista_puntos.Add(p3);
                c.Tipo = 2;
            }
            DialogResult = DialogResult.OK;
            Close();

        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp = textBox_pix.Text;
            textBox_pix.Text = textBox_pfx.Text;
            textBox_pfx.Text = temp;

            string temp2 = textBox_piy.Text;
            textBox_piy.Text = textBox_pfy.Text;
            textBox_pfy.Text = temp2;
        }
    }
}
