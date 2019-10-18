using EjeDeTrazado.puntosDelEje;
using Logica;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;

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

            this.componentes.ForEach(componente => {
                int componenteIndex = this.componentes.IndexOf(componente);

                String tipo = "";
                if (componente.Tipo ==  1) {
                    tipo = "Recta";
                }
                if (componente.Tipo == 2) {
                    tipo = "Curva";
                }

                String sentidoDeLaCurva = "";
                if (componente.direccion.Equals(EjeTrazado.sentidoCurva.Horario)) {
                    sentidoDeLaCurva = "Horario";
                }
                if (componente.direccion.Equals(EjeTrazado.sentidoCurva.Antihorario)) {
                    sentidoDeLaCurva = "Antihorario";
                }

                dataTable.Rows.Add(componenteIndex, tipo, sentidoDeLaCurva, componente.xc, componente.yc, componente.azte, componente.azts, componente.azr, componente.radio,componente.ini, componente.fin);

            });

            componentesInfoDataGridView.DataSource = dataTable;
        }
    }
}
