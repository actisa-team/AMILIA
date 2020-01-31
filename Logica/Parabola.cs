using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class  Parabola
    {
        public List<double> parabola = new List<double>();
        public List<PuntoPerfil> polilinea_perfil = new List<PuntoPerfil>();
        public double max_min { get; set; }
        public Parabola()
        {

        }
        public Parabola(List<double> p)
        {
            parabola = p;
        }
        public Parabola(List<double> p , List<PuntoPerfil> poli)
        {
            parabola = p;
            polilinea_perfil = poli;
        }
        public void Add_PuntoPerfil(PuntoPerfil p)
        {
            polilinea_perfil.Add(p);
        }
        public void Add_parabola(List<double> p)
        {
            parabola = p;
        }
    }
}
