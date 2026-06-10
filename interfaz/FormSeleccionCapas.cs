using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace interfaz
{
    public partial class FormSeleccionCapas : MaterialForm
    {
        public List<string> SelectedLayers { get; private set; }

        public FormSeleccionCapas(List<string> availableLayers)
        {
            InitializeComponent();
            SelectedLayers = new List<string>();

            // Poblar CheckedListBox
            foreach (var layer in availableLayers)
            {
                checkedListBoxCapas.Items.Add(layer);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            SelectedLayers.Clear();
            foreach (var item in checkedListBoxCapas.CheckedItems)
            {
                SelectedLayers.Add(item.ToString());
            }

            if (SelectedLayers.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos una capa.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
