using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logica.Componentes
{
    public abstract class Componente
    {
        public enum tipoComponente { curva, recta, clotoide };
        abstract public tipoComponente getTipoComponente();
        abstract public string get_tipoclotoide();
        abstract public void set_azimut(double az);
        abstract public double get_azimut();
        abstract public double[] getPointAtDist(double iDistancia);

        abstract public List<double[]> getComponentPoints();
        abstract public List<double[]> getComponentPoints(double pk);
        abstract public List<double[]> getComponentPoints(double pk, double pk_fin);
        abstract public double getLongitud();
        abstract public double getRadio();
        abstract public string getSentido();
        abstract public Punto3d getCentro();
        abstract public double getLe();
        abstract public double getA();
        abstract public string getTipoClotoide();
        abstract public double getPkIni();
        abstract public double getPkFin();
    }
}
