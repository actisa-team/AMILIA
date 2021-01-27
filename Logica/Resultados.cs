using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class Resultados
    {
        private int n_curvas;
        private double porcentaje_clusterizacion;
        private int puntos_clusterizacion;
        private double grados_union;
        private bool division_rectas;
        private int n_componentes;

        public int N_C { get => n_curvas; set => n_curvas = value; }
        public double Por_C { get => porcentaje_clusterizacion; set => porcentaje_clusterizacion = value; }
        public int Pun_C { get => puntos_clusterizacion; set => puntos_clusterizacion = value; }
        public double G_U { get => grados_union; set => grados_union = value; }
        public bool D_R { get => division_rectas; set => division_rectas = value; }
        public int Num_C { get => n_componentes; set => n_componentes = value; }

        public Resultados()
        {

        }
    }
}
