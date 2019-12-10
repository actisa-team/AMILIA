using EjeDeTrazado.puntosDelEje;
using Logica;
using Logica.verificacion;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace interfaz {
    public partial class ComponentesInfoPanel : MaterialForm {
        private List<Componente> componentes;
        VerificacionComponentesStatus verificacionComponentesStatus;

        public ComponentesInfoPanel(List<Componente> componentes, VerificacionComponentesStatus verificacionComponentesStatus) {
            InitializeComponent();
            this.componentes = componentes;
            this.verificacionComponentesStatus = verificacionComponentesStatus;
            this.populateDataTable();
            this.populateVerificacionComponentesStatus2();
        }
        public ComponentesInfoPanel(List<Componente> componentes)
        {
            InitializeComponent();
            this.componentes = componentes;
            
            this.populateDataTable();
            this.populateVerificacionComponentesStatus();
        }

        public void populateVerificacionComponentesStatus() {
            if (this.verificacionComponentesStatus != null) {
                if (!this.verificacionComponentesStatus.VerificacionDeCurvas.Any() && !this.verificacionComponentesStatus.VerificacionDeRectas.Any()) {
                    this.showValidacionComponentesCorrecta();
                } else {
                    if (this.verificacionComponentesStatus.VerificacionDeCurvas.Any()) {
                        this.showErrorValidacionCurva();
                    }
                    if (this.verificacionComponentesStatus.VerificacionDeRectas.Any()) {
                        this.showErrorValidacionRecta();
                    }
                }
            }       
        }
        public void populateVerificacionComponentesStatus2()
        {
            if (this.verificacionComponentesStatus != null)
            {
                this.showValidacionComponentesCorrecta();
            }
        }

        private void showErrorValidacionCurva() {
            this.errorValidationPanel.Visible = true;
            this.errorValidacionCurvasLabel.Visible = true;
        }

        private void showErrorValidacionRecta() {
            this.errorValidationPanel.Visible = true;
            this.errorValidacionRectasLabel.Visible = true;
        }

        private void showValidacionComponentesCorrecta() {
            this.successValidationPanel.Visible = true;
        }

        private void hideValidacionComponentesCorrecta() {
            this.successValidationPanel.Visible = false;
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
