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
    public partial class AutoCloseMessageBox : Form
    {
        public int valor;
        public AutoCloseMessageBox(string message,string caption)
        {
            InitializeComponent();
            this.Text = caption;
            labelMessage.Text = message;
            timerAutoClose.Interval = 300000; // 5 minutos en milisegundos
            timerAutoClose.Start();
        }
        
        private void timerAutoClose_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Cancelar la operación o realizar alguna acción si es necesario
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.valor = 1;
  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.valor = 2;
        }
        public void cerrar()
        {
            this.Close();
        }
    }
}
