using System.Collections.Generic;

namespace Logica {
    public class ViabilidadComponentesStatus {
        private List<ViabilidadComponente> viabilidadComponentes = new List<ViabilidadComponente>();
        private ViabilidadComponente casoResuelto;

        public ViabilidadComponente CasoResuelto { get => casoResuelto; set => casoResuelto = value; }
        public List<ViabilidadComponente> ViabilidadComponentes { get => viabilidadComponentes; set => viabilidadComponentes = value; }
    }

}
