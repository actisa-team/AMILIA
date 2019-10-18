using System.Collections.Generic;

namespace Logica.verificacion {
    public class VerificacionComponentesStatus {
        private List<ComponentesCurvasSeguidasYDistintoSentido> verificacionDeCurvas = new List<ComponentesCurvasSeguidasYDistintoSentido>();
        private List<ComponentesRectasSeguidas> verificacionDeRectas = new List<ComponentesRectasSeguidas>();

        public List<ComponentesRectasSeguidas> VerificacionDeRectas { get => verificacionDeRectas; set => verificacionDeRectas = value; }
        public List<ComponentesCurvasSeguidasYDistintoSentido> VerificacionDeCurvas { get => verificacionDeCurvas; set => verificacionDeCurvas = value; }
    }
}
