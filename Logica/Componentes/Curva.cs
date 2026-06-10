using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logica.Componentes
{
    public class Curva : Componente
    {
        public tipoComponente tipo { get; set; }
        public Punto3d Centro { get; set; }
        public Punto3d punto_inicial { get; set; }
        public Punto3d punto_final { get; set; }
        public double Radio { get; set; }
        public double Az_inicial { get; set; }
        public double Az_final { get; set; }
        public string Sentido { get; set; }
        public double Pk_inicial { get; set; }
        public double Pk_final { get; set; }
        public Curva()
        {

        }
        public Curva(Punto3d c, Punto3d ini,Punto3d fin,double r,double az_ini,double az_fin,string s,double Pkini, double Pkfin)
        {
            Centro = c;
            punto_inicial = ini;
            punto_final = fin;
            Radio = r;
            Az_inicial = az_ini;
            Az_final = az_fin;
            Sentido = s;
            Pk_inicial = Pkini;
            Pk_final = Pkfin;
            this.tipo = tipoComponente.curva;
        }
        public override tipoComponente getTipoComponente()
        {
            return tipoComponente.curva;
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
            return -1;
        }
        public override double[] getPointAtDist(double iDistancia)
        {
            double[] miPunto = new double[2];
            double miPK = iDistancia - Pk_inicial;
            double miCambAng = miPK / Radio;
            double miXo = punto_inicial.X;
            double miYo = punto_inicial.Y;
            double miAzimut;
            if (miXo > Centro.X)
            {
                if (miYo > Centro.Y)
                {
                    miAzimut = 90 - 180 / Math.PI * Math.Atan((miYo - Centro.Y) / (miXo - Centro.X));
                }
                else
                {
                    miAzimut = 90 + 180 / Math.PI * Math.Atan((Centro.Y - miYo) / (miXo - Centro.X));
                }

            }
            else
            {
                if (miYo > Centro.Y)
                {
                    miAzimut = 270 + 180 / Math.PI * Math.Atan((miYo - Centro.Y) / (Centro.X - miXo));
                }
                else
                {
                    miAzimut = 270 - 180 / Math.PI * Math.Atan((Centro.Y - miYo) / (Centro.X - miXo));
                }
            }
            double miAzFinal;
            if (Sentido == "cw")
            {
                miAzFinal = miAzimut + miCambAng * 180 / Math.PI;
            }
            else
            {
                miAzFinal = miAzimut - miCambAng * 180 / Math.PI;
            }
            double miXi = Centro.X + Radio * Math.Sin(miAzFinal * Math.PI / 180);
            double miYi = Centro.Y + Radio * Math.Cos(miAzFinal * Math.PI / 180);

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }
        /*public override double[] getPointAtDist(double iDistancia)
        {
            double[] miPunto = new double[2];

            // Calcular distancia relativa desde Pk_inicial
            double miPK = iDistancia - Pk_inicial;
            double miCambAng = miPK / Radio; // Cambio angular en radianes

            // Coordenadas del punto inicial
            double miXo = punto_inicial.X;
            double miYo = punto_inicial.Y;

            // Calcular azimut inicial usando Math.Atan2 (maneja automáticamente los cuadrantes)
            double miAzimut = Math.Atan2(miYo - Centro.Y, miXo - Centro.X) * 180 / Math.PI;

            // Ajustar el ángulo final según el sentido del giro
            double miAzFinal = (Sentido == "cw")
                ? miAzimut + miCambAng * 180 / Math.PI
                : miAzimut - miCambAng * 180 / Math.PI;

            // Normalizar el ángulo final a [0, 360) grados
            miAzFinal = (miAzFinal + 360) % 360;

            // Calcular coordenadas del nuevo punto en el arco
            double miXi = Centro.X + Radio * Math.Cos(miAzFinal * Math.PI / 180);
            double miYi = Centro.Y + Radio * Math.Sin(miAzFinal * Math.PI / 180);

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }*/
        public override List<double[]> getComponentPoints()
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((Pk_inicial + i) < Pk_final)
            {
                puntos.Add(getPointAtDist(Pk_inicial + i));
                if (Pk_final-Pk_inicial < 50)
                {
                    i += 0.1;
                }
                else
                {
                    if (Pk_final - Pk_inicial > 5000)
                    {
                        i += 10;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            puntos.Add(new double[] { punto_final.X, punto_final.Y });
            return puntos;

        }
        public override List<double[]> getComponentPoints(double pk)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((Pk_inicial + i) < Pk_final)
            {
                puntos.Add(getPointAtDist(Pk_inicial + i));
                if (Pk_final - Pk_inicial < 50)
                {
                    i += 0.1;
                }
                else
                {
                    if (Pk_final - Pk_inicial > 5000)
                    {
                        i += 10;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            puntos.Add(new double[] { punto_final.X, punto_final.Y });
            return puntos;

        }
        public override List<double[]> getComponentPoints(double pk , double pk_fin)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((Pk_inicial + i) < Pk_final)
            {
                puntos.Add(getPointAtDist(Pk_inicial + i));
                if (Pk_final - Pk_inicial < 50)
                {
                    i += 0.1;
                }
                else
                {
                    if (Pk_final - Pk_inicial > 5000)
                    {
                        i += 10;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            puntos.Add(new double[] { punto_final.X, punto_final.Y });
            return puntos;

        }
        public override double getLongitud()
        {
            return Pk_final - Pk_inicial;
        }
        public override double getRadio()
        {
            return Radio;
        }
        public override string getSentido()
        {
            return Sentido;
        }
        public override Punto3d getCentro()
        {
            return Centro;
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
