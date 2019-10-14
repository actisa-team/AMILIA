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
    }
}