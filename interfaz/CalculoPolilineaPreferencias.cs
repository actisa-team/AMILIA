namespace interfaz
{
    internal class CalculoPolilineaPreferencias
    {
        private int opcion = 1;
        private double grados = 0;
        private double metros = 0;
        private double ratio = 0;
        private int it = 0;
        private double t_med;
        private double t_max;
        private double p_cluster;
        private double gran_r;
        private int n_curvas;
        private int puntos_cluster;
        private int solapes;
        private double rotulacion;
        private bool rotu;
        private int suavizado;
        private bool dividir;
        private bool dividir_curvas;
        private double rectas;
        private double curvas;
        private double dis_eliminar;

        int[] orden;
        public double Dis_eliminar { get => dis_eliminar; set => dis_eliminar = value; }
        public int Suavizado { get => suavizado; set => suavizado = value; }
        public int Opcion { get => opcion; set => opcion = value; }
        public double Grados { get => grados; set => grados = value; }
        public double Metros { get => metros; set => metros = value; }
        public double Ratio { get => ratio; set => ratio = value; }
        public int It { get => it; set => it = value; }
        public double T_med { get => t_med; set => t_med = value; }
        public double T_max { get => t_max; set => t_max = value; }
        public double P_cluster { get => p_cluster; set => p_cluster = value; }
        public double Gran_r { get => gran_r; set => gran_r = value; }
        public int N_curvas { get => n_curvas; set => n_curvas = value; }
        public int Puntos_cluster { get => puntos_cluster; set => puntos_cluster = value; }
        public int Solapes { get => solapes; set => solapes = value; }
        public double Rotulacion { get => rotulacion; set => rotulacion = value; }
        public bool Rotu { get => rotu; set => rotu = value; }
        public int[] Orden { get => orden; set => orden = value; }
        public bool Dividir { get => dividir; set => dividir=value; }
        public double Curvas { get => curvas; set => curvas = value; }
        public bool Dividir_curvas { get => dividir_curvas; set => dividir_curvas = value; }
        public double Rectas { get => rectas; set => rectas = value; }
    }
}