using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logica.Componentes;

namespace Logica.Componentes
{
    public class Recta : Componente
    {
        public tipoComponente tipo { get; set; }
        public Punto3d punto_inicial { get; set; }
        public Punto3d punto_final { get; set; }
        public double Pk_inicial { get; set; }
        public double Pk_final { get; set; }
        public double Azimut { get; set; }

        public Recta()
        {

        }
        public Recta(Punto3d p_i,Punto3d p_f,double Pkini,double Pkfin,double az)
        {
            punto_inicial = p_i;
            punto_final = p_f;
            Pk_inicial = Pkini;
            Pk_final = Pkfin;
            Azimut = az;
            this.tipo = tipoComponente.recta;
        }
        public override tipoComponente getTipoComponente()
        {
            return tipoComponente.recta;
        }
        public override string get_tipoclotoide()
        {
            return null;
        }
        public override void set_azimut(double az)
        {
            
        }
        public override double get_azimut()
        {
            return Azimut;
        }
        public override double[] getPointAtDist(double iDistancia)
        {
            double[] miPunto = new double[2];
            double miPK = iDistancia - Pk_inicial;
            double miXo, miYo;
            miXo = punto_inicial.X;
            miYo = punto_inicial.Y;


            double miXi = miXo + miPK * Math.Sin(Azimut * Math.PI / 180);
            double miYi = miYo + miPK * Math.Cos(Azimut * Math.PI / 180);

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }
        public override List<double[]> getComponentPoints()
        {
            List<double[]> lista = new List<double[]>();
            lista.Add(new double[] { punto_inicial.X, punto_inicial.Y });
            lista.Add(new double[] { punto_final.X, punto_final.Y });
            return lista;
        }
        public override List<double[]> getComponentPoints(double pk)
        {
            List<double[]> lista = new List<double[]>();
            lista.Add(new double[] { punto_inicial.X, punto_inicial.Y });
            lista.Add(new double[] { punto_final.X, punto_final.Y });
            return lista;
        }
        public override List<double[]> getComponentPoints(double pk,double pk_fin)
        {
            List<double[]> lista = new List<double[]>();
            lista.Add(new double[] { punto_inicial.X, punto_inicial.Y });
            lista.Add(new double[] { punto_final.X, punto_final.Y });
            return lista;
        }
        public override double getLongitud()
        {
            return Pk_final - Pk_inicial;
        }
        public override double getRadio()
        {
            return 0;
        }
        public override string getSentido()
        {
            return "";
        }
        public override Punto3d getCentro()
        {
            return null;
        }
        public override double getLe()
        {
            return 0;
        }
        public override double getA()
        {
            return 0;
        }
        public override string getTipoClotoide()
        {
            return "";
        }
        public override double getPkIni()
        {
            return Pk_inicial;
        }
        public override double getPkFin()
        {
            return Pk_final;
        }
    }
}
