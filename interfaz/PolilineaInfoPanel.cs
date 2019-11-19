using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using EjeDeTrazado.puntosDelEje;
using Logica;

namespace interfaz
{
    public partial class PolilineaInfoPanel : MaterialForm
    {
        private List<Punto> polilinea;
        public PolilineaInfoPanel(List<Punto> Polilinea)
        {
            InitializeComponent();
            polilinea = Polilinea;
            this.populateDataTable();
        }
        public void populateDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Componente");
            dataTable.Columns.Add("X");
            dataTable.Columns.Add("Y");
            dataTable.Columns.Add("Recta");
            dataTable.Columns.Add("Curva");
            dataTable.Columns.Add("Radio");
            dataTable.Columns.Add("Radio-p");
            dataTable.Columns.Add("giro");
            dataTable.Columns.Add("Cambio de giro");
            dataTable.Columns.Add("minimo");
            dataTable.Columns.Add("minimi-p");

            this.polilinea.ForEach(punto => {
                int puntoIndex = this.polilinea.IndexOf(punto);

                String tipo = "";
                String tipo2 = "";
                String giro = "";
                String C_giro = "";
                if (punto.Recta == 1)
                {
                    tipo = "Recta";
                }
                if (punto.Curva == 1)
                {
                    tipo2 = "Curva";
                }
                if (punto.Tipogiro==1)
                {
                    giro = "horario";
                }else if (punto.Tipogiro ==2)
                {
                    giro = "antihorario";
                }
                else
                {
                    giro = "recta";
                }
                if (punto.secuenciagiro == 2)
                {
                    C_giro = "Mismo sentido";
                }
                else
                {
                    C_giro = "Cambio sentido";
                }
                dataTable.Rows.Add(puntoIndex,punto.p.X, punto.p.Y, tipo, tipo2, punto.R, punto.Rp, giro, C_giro, punto.minimo, punto.minimop);

            });

            dataGridView1.DataSource = dataTable;
        }
    }
}
