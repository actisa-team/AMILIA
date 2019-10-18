namespace Logica.verificacion {
    public class ComponentesCurvasSeguidasYDistintoSentido {

        public ComponentesCurvasSeguidasYDistintoSentido(Componente componente1, Componente componente2) {
            this.componente1 = componente1;
            this.componente2 = componente2;
        }

        public Componente componente1 { get; set; }
        public Componente componente2 { get; set; }
    }
}
