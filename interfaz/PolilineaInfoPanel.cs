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
        private List<PuntoPerfil> polilinea_perfil;
        private List<Parabola> parabolas;
        public PolilineaInfoPanel(List<Punto> Polilinea)
        {
            InitializeComponent();
            polilinea = Polilinea;
            this.populateDataTable();
        }
        public PolilineaInfoPanel(List<PuntoPerfil> Polilinea)
        {
            InitializeComponent();
            polilinea_perfil = Polilinea;
            this.populateDataTablePerfil();
        }
        public PolilineaInfoPanel(List<Parabola> p)
        {
            InitializeComponent();
            parabolas = p;
            this.populateDataTableParabola();
        }
        public void populateDataTableParabola()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Punto");
            dataTable.Columns.Add("a");
            dataTable.Columns.Add("b");
            dataTable.Columns.Add("c");
            dataTable.Columns.Add("Primer punto");
            dataTable.Columns.Add("Ultimo punto");
            dataTable.Columns.Add("Pendiente Entrada");
            dataTable.Columns.Add("Pendiente salida");



            this.parabolas.ForEach(parabola => {
                int puntoIndex = this.parabolas.IndexOf(parabola);

                dataTable.Rows.Add(puntoIndex, parabola.lista_parabola[0], parabola.lista_parabola[1], parabola.lista_parabola[2],
                    parabola.Polilinea_Perfil[0].p.X, parabola.Polilinea_Perfil[parabola.Polilinea_Perfil.Count-1].p.X,
                    parabola.Polilinea_Perfil[0].pendiente, parabola.Polilinea_Perfil[parabola.Polilinea_Perfil.Count - 1].pendiente);

            });

            dataGridView1.DataSource = dataTable;
        }
        public void populateDataTablePerfil()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Punto");
            dataTable.Columns.Add("X");
            dataTable.Columns.Add("Y");
            dataTable.Columns.Add("Vertice");
            dataTable.Columns.Add("Pendiente");
            dataTable.Columns.Add("Primero");
            dataTable.Columns.Add("Último");
            dataTable.Columns.Add("Varianza");
            dataTable.Columns.Add("Var. acum");
            dataTable.Columns.Add("Resultado");
            dataTable.Columns.Add("Valor");


            this.polilinea_perfil.ForEach(punto => {
                int puntoIndex = this.polilinea_perfil.IndexOf(punto);
                string resultado = "";
                if (punto.tipo==1)
                {
                    resultado = "Uniforme";
                }
                if (punto.tipo == 2)
                {
                    resultado = "Depuracion";
                }
                string vertice = "";
                if (punto.vertice == 1)
                {
                    vertice = "Superior";
                }
                if (punto.vertice == 2)
                {
                    vertice = "Inferior";
                }
                dataTable.Rows.Add(puntoIndex, punto.p.X, punto.p.Y,vertice, punto.pendiente, punto.primero, punto.ultimo, punto.varianza, punto.varianza_a, resultado, punto.valor*100);

            });

            dataGridView1.DataSource = dataTable;
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
