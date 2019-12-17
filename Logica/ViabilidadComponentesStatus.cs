using System.Collections.Generic;

namespace Logica {
    public class ViabilidadComponentesStatus {
        private List<ViabilidadComponente> viabilidadComponentes = new List<ViabilidadComponente>();
        private ViabilidadComponente casoResuelto;
        private bool terminar;

        public ViabilidadComponente CasoResuelto { get => casoResuelto; set => casoResuelto = value; }
        public bool Terminarejecucion { get => terminar; set => terminar = value; }
        public List<ViabilidadComponente> ViabilidadComponentes { get => viabilidadComponentes; set => viabilidadComponentes = value; }
    }

}
