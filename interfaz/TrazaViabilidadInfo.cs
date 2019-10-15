using MaterialSkin.Controls;
using System.Collections.Generic;

namespace interfaz {

    public partial class TrazaViabilidadInfo : MaterialForm {

        private List<Logica.ViabilidadComponentesStatus> trazaViabilidadComponentes;

        public TrazaViabilidadInfo(List<Logica.ViabilidadComponentesStatus> trazaViabilidadComponentes) {
            InitializeComponent();
            this.trazaViabilidadComponentes = trazaViabilidadComponentes;
        }


    }
}