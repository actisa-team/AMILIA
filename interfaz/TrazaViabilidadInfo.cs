using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;

namespace interfaz {

    public partial class TrazaViabilidadInfo : MaterialForm {

        private List<Logica.ViabilidadComponentesStatus> trazaViabilidadComponentes;

        public TrazaViabilidadInfo(List<Logica.ViabilidadComponentesStatus> trazaViabilidadComponentes) {
            this.trazaViabilidadComponentes = trazaViabilidadComponentes;
            InitializeComponent();
            this.populateDataTable();
        }

        private void populateDataTable() {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Iteración");
            dataTable.Columns.Add("Componente");
            dataTable.Columns.Add("Tipo");
            dataTable.Columns.Add("Caso");
            dataTable.Columns.Add("Resuelto");

            dataTable.Rows.Add("fasdf", "r3trg", "fasd", "agerge", "fera");

            trazaViablidadDataGridView.DataSource = dataTable;
        }
    }
}