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
        private List<IViabilidadStatusInfoPanelListener> listeners = new List<IViabilidadStatusInfoPanelListener>();

        public ViabilidadComponentesStatusInfoPanel(ViabilidadComponentesStatus viabilidadComponentesStatus, List<Componente> componentes, String title, int whileItIndex) {
            this.Text = title;
            this.whileItIndex = whileItIndex;
            this.viabilidadComponentesStatus = viabilidadComponentesStatus;
            this.componentes = componentes;
            this.ShowInTaskbar = false;
            this.InitializeComponent();
            this.populateDataTable();
            this.ControlBox = false;
            this.CenterToScreen();

            //listener para ejecutar hasta el final sin detener
            this.ejecutarHastaFinalizarButton.Click += this.ejecutarHastaFinalizarButtonClick;

            //listener para continuar depuracion paso a paso
            this.continuarDepuracionButton.Click += this.continuarDepurandoButtonClick;

            //Listener para continuar hasta la iteracion indicada en el textBox
            this.ejecutarHastaIteracionButton.Click += this.ejecutarHastaIteracionButtonClick;
            
        }

        public void addListener(IViabilidadStatusInfoPanelListener listener) {
            this.listeners.Add(listener);
        }

        private void ejecutarHastaFinalizarButtonClick(object sender, EventArgs e) {
            this.listeners.ForEach(listener => listener.continuarHastaElFinal());
            this.Dispose();
        }
        public void ejecutarHastaFinalizarButtonClick()
        {
            this.listeners.ForEach(listener => listener.continuarHastaElFinal());
            this.Dispose();
        }

        private void continuarDepurandoButtonClick(object sender, EventArgs e) {
            this.listeners.ForEach(listener => listener.continuarPasoAPaso());
            this.Dispose();
        }
        private void TerminarButtonClick(object sender, EventArgs e)
        {
            this.listeners.ForEach(listener => listener.Terminar());
            this.Dispose();
        }

        private void ejecutarHastaIteracionButtonClick(object sender, EventArgs e) {
            try {
                string detenerEnIteracion = this.DetenerEnIteracionTextBox.Text;
                int iteracion = Int32.Parse(detenerEnIteracion);
                this.listeners.ForEach(listener => listener.continuarHastaLaIteracion(iteracion));
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                this.listeners.ForEach(listener => listener.continuarPasoAPaso());
            }
            this.Dispose();
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
    }
}