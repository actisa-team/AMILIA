using System.Collections.Generic;

namespace Logica {
    class ViabilidadComponentesStatus {
        private List<ViabilidadComponente> viabilidadComponentes = new List<ViabilidadComponente>();

        internal List<ViabilidadComponente> ViabilidadComponentes { get => viabilidadComponentes; set => viabilidadComponentes = value; }
    }

}
