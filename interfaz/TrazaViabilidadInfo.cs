using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;

namespace interfaz {

    public partial class TrazaViabilidadInfo : MaterialForm {

        private List<Logica.ViabilidadComponentesStatus> trazaViabilidadComponentes;
        private List<Logica.Componente> componentes;

        public TrazaViabilidadInfo(List<Logica.ViabilidadComponentesStatus> trazaViabilidadComponentes, List<Logica.Componente> componentes) {
            this.trazaViabilidadComponentes = trazaViabilidadComponentes;
            this.componentes = componentes;
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
            
            this.trazaViabilidadComponentes.ForEach(traza => {
                int trazaIndex = this.trazaViabilidadComponentes.IndexOf(traza);
                traza.ViabilidadComponentes.ForEach(viabilidadComponente => {
                    int componenteIndex = this.componentes.IndexOf(viabilidadComponente.Componente);
                    String tipo = "";
                    if (viabilidadComponente.Componente.Tipo == 1) {
                        tipo = "Recta";
                    }
                    if (viabilidadComponente.Componente.Tipo == 2) {
                        tipo = "Curva";
                    }

                    String resuelto = "";
                    if (traza.CasoResuelto.Equals(viabilidadComponente)) {
                        resuelto = "SI";
                    }

                    dataTable.Rows.Add(trazaIndex, componenteIndex, tipo, viabilidadComponente.Caso, resuelto);
                });   
            });

            trazaViablidadDataGridView.DataSource = dataTable;
        }
    }
}