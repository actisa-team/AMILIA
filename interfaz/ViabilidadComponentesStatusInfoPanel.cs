using Logica;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;

namespace interfaz {

    public partial class ViabilidadComponentesStatusInfoPanel : MaterialForm {

        private ViabilidadComponentesStatus viabilidadComponentesStatus;
        private List<Logica.Componente> componentes;
        private int whileItIndex;

        public ViabilidadComponentesStatusInfoPanel(ViabilidadComponentesStatus viabilidadComponentesStatus, List<Componente> componentes, String title, int whileItIndex) {
            this.Text = title;
            this.whileItIndex = whileItIndex;
            this.viabilidadComponentesStatus = viabilidadComponentesStatus;
            this.componentes = componentes;
            InitializeComponent();
            this.populateDataTable();
            this.ControlBox = false;
            this.CenterToScreen();

            this.ejecutarHastaFinalizarButton.Click += this.ejecutarHastaFinalizarButtonClick;
            this.continuarDepuracionButton.Click += this.continuarDepurandoButtonClick;
        }

        private void ejecutarHastaFinalizarButtonClick(object sender, EventArgs e) {
            this.Dispose();
        }

        private void continuarDepurandoButtonClick(object sender, EventArgs e) {
            this.Dispose();
        }

        public void addEjecutarHastaFinalizar(EventHandler p) {
            this.ejecutarHastaFinalizarButton.Click += p;
        }

        public void addDetenerEnIteracion(EventHandler p) {
            this.ejecutarHastaIteracionButton.Click += p;
        }

        public ViabilidadComponentesStatus ViabilidadComponentesStatus { get => viabilidadComponentesStatus; set => viabilidadComponentesStatus = value; }
        public List<Componente> Componentes { get => componentes; set => componentes = value; }
        public int WhileItIndex { get => whileItIndex; set => whileItIndex = value; }

        public void populateDataTable() {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Iteración");
            dataTable.Columns.Add("Componente");
            dataTable.Columns.Add("Tipo");
            dataTable.Columns.Add("Caso");
            dataTable.Columns.Add("¿Marcado para resolver?");


            this.viabilidadComponentesStatus.ViabilidadComponentes.ForEach(viabilidadComponente =>
            {
                int componenteIndex = this.componentes.IndexOf(viabilidadComponente.Componente);
                String tipo = "";
                if (viabilidadComponente.Componente.Tipo == 1) {
                    tipo = "Recta";
                }
                if (viabilidadComponente.Componente.Tipo == 2) {
                    tipo = "Curva";
                }

                String resuelto = "";
                if (this.viabilidadComponentesStatus.CasoResuelto != null && this.viabilidadComponentesStatus.CasoResuelto.Equals(viabilidadComponente)) {
                    resuelto = "SI";
                }

                dataTable.Rows.Add(this.whileItIndex, componenteIndex, tipo, viabilidadComponente.Caso, resuelto);
            });

            trazaViablidadDataGridView.DataSource = dataTable;

        }

        private void continuarDepuracionButton_Click(object sender, EventArgs e) {

        }
    }
}