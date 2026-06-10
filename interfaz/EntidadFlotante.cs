using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace interfaz
{
    public partial class EntidadFlotante : Form
    {
        public double dato { get; set; }
        public bool clotoide { get; set; }
        public double A { get; set; }
        public string entidad { get; set; }
        public EntidadFlotante()
        {
            InitializeComponent();
        }
        public void CargarDatos(string entidad, double dato, int posicion,double A)
        {
            this.entidad = entidad;
            if (entidad == "RECTA")
            {
                label1.Text = "Se va a crear una Recta auxiliar para poder hacer el trazado entre las entidades " + posicion + " y " + (posicion + 1);
                label2.Text = "Desplazamiento lateral de la recta respecto a la tangencia de las curvas";
            }
            if (entidad == "CURVA")
            {
                label1.Text = "Se va a crear una Curva auxiliar para poder hacer el trazado entre las entidades " + posicion + " y " + (posicion + 1);
                label2.Text = "Radio de la curva creada";
            }
            textBox1.Text = dato.ToString();
            textBox2.Text = A.ToString();
            if (A==0)
            {
                checkBox1.Visible = false;
            }
            else
            {
                checkBox1.Visible = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double resultado))  // Evita errores al convertir
            {
                dato = resultado;
                if (checkBox1.Checked)
                {
                    if (double.TryParse(textBox2.Text, out double resultado2))  // Evita errores al convertir
                    {
                        A = resultado2;
                        clotoide = true;
                    }
                    else
                    {
                        MessageBox.Show("Por favor, ingrese un valor numérico válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    clotoide = false;
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un valor numérico válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                label3.Visible = true;
                textBox2.Visible = true;
                if (this.entidad == "CURVA")
                {
                    textBox1.Text = (double.Parse(textBox1.Text) / 2).ToString();
                }
                    
            }
            else
            {
                label3.Visible = false;
                textBox2.Visible = false;
                if (this.entidad == "CURVA")
                {
                    textBox1.Text = (double.Parse(textBox1.Text) * 2).ToString();
                }
                
            }
        }
    }
}
