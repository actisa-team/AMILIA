using Logica;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace interfaz {
    public partial class ComponentesInfoPanel : MaterialForm {
        private List<Componente> componentes;
        public ComponentesInfoPanel(List<Componente> componentes) {
            InitializeComponent();
            this.componentes = componentes;
            this.populateDataTable();
        }

        public void populateDataTable() {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Componente");
            dataTable.Columns.Add("Tipo (Curva / Recta)");
            dataTable.Columns.Add("Sentido");
            dataTable.Columns.Add("xc");
            dataTable.Columns.Add("yc");
            dataTable.Columns.Add("azte");
            dataTable.Columns.Add("azts");
            dataTable.Columns.Add("azr");
            dataTable.Columns.Add("radio");
            dataTable.Columns.Add("ini");
            dataTable.Columns.Add("fin");


            componentesInfoDataGridView.DataSource = dataTable;
        }
    }
}
