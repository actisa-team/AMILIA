using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logica.Componentes
{
    public class Clotoide : Componente
    {
        public tipoComponente tipo { get; set; }
        public Punto3d punto_inicial { get; set; }
        public Punto3d punto_final { get; set; }
        public double A { get; set; }
        public double Le { get; set; }
        public double Azimut { get; set; }
        public string Sentido { get; set; }
        public double Pk_inicial { get; set; }
        public double Pk_final { get; set; }
        public string tipoclotoide;
        public double radio { get; set; }
        public Clotoide()
        {

        }
        public Clotoide(Punto3d ini, Punto3d fin, string s, double Pkini, double Pkfin, double a, double le, string tipo, double r)
        {
            punto_inicial = ini;
            punto_final = fin;
            Sentido = s;
            Pk_inicial = Pkini;
            Pk_final = Pkfin;
            A = a;
            Le = le;
            tipoclotoide = tipo;
            radio = r;
            this.tipo = tipoComponente.clotoide;

        }
        public override tipoComponente getTipoComponente()
        {
            return tipoComponente.clotoide;
        }
        public override string get_tipoclotoide()
        {
            return tipoclotoide;
        }
        public override void set_azimut(double az)
        {
            Azimut = az;
        }
        public override double get_azimut()
        {
            return Azimut;
        }
        private double Get_re(double q1, int n)
        {
            return Math.Pow(q1, n) / ((n + n + 1) * factorial(n));
        }
        private double factorial(double n)
        {
            if (n == 1)
                return 1;
            else
                return n * factorial(n - 1);
        }
        public override double getLongitud()
        {
            return Pk_final - Pk_inicial;
        }
        public override double[] getPointAtDist(double iDistancia)
        {
            double[] miPunto = new double[2];
            double miLe;
            double miPK = iDistancia - Pk_inicial;
            miLe = A * A / radio;
            double miLi, miXo, miYo;


            double miXi, miYi;
            if (tipoclotoide == "Entrada")
            {
                miLi = miPK;
                miXo = punto_inicial.X;
                miYo = punto_inicial.Y;
            }
            else
            {
                miLi = Le - iDistancia;
                miXo = punto_final.X;  //salida?!
                miYo = punto_final.Y;

            }
            double miQi = 0.5 * Math.Pow((miLi / A), 2);
            double miRi = Math.Pow(A, 2) / miLi;
            double miXe = (1 - Math.Pow(miQi, 2) / 10 + Math.Pow(miQi, 4) / 216 - Math.Pow(miQi, 6) / 9360 + Math.Pow(miQi, 8) / 685440
                - Get_re(miQi, 10) + Get_re(miQi, 12) - Get_re(miQi, 14) + Get_re(miQi, 16) - Get_re(miQi, 18) + Get_re(miQi, 20) - Get_re(miQi, 22)) * miLi;
            double miYe = ((miQi / 3) - (Math.Pow(miQi, 3) / 42) + (Math.Pow(miQi, 5) / 1320) - (Math.Pow(miQi, 7) / 75600)
                 + Get_re(miQi, 9) - Get_re(miQi, 11) + Get_re(miQi, 13) - Get_re(miQi, 15) + Get_re(miQi, 17) - Get_re(miQi, 19) + Get_re(miQi, 21) - Get_re(miQi, 23)) * miLi;
            if ((tipoclotoide == "Entrada") && (Sentido == "cw"))
            {
                miXi = miXo + miXe * Math.Sin(Azimut * Math.PI / 180) + miYe * Math.Cos(Azimut * Math.PI / 180);
                miYi = miYo + miXe * Math.Cos(Azimut * Math.PI / 180) - miYe * Math.Sin(Azimut * Math.PI / 180);
            }
            else if ((tipoclotoide == "Salida") && (Sentido == "cw"))
            {
                miXi = miXo - miXe * Math.Sin(Azimut * Math.PI / 180) + miYe * Math.Cos(Azimut * Math.PI / 180);
                miYi = miYo - miXe * Math.Cos(Azimut * Math.PI / 180) - miYe * Math.Sin(Azimut * Math.PI / 180);
            }
            else if ((tipoclotoide == "Entrada") && (Sentido == "ccw"))
            {
                miXi = miXo + miXe * Math.Sin(Azimut * Math.PI / 180) - miYe * Math.Cos(Azimut * Math.PI / 180);
                miYi = miYo + miXe * Math.Cos(Azimut * Math.PI / 180) + miYe * Math.Sin(Azimut * Math.PI / 180);
            }
            else
            {
                miXi = miXo - miXe * Math.Sin(Azimut * Math.PI / 180) - miYe * Math.Cos(Azimut * Math.PI / 180);
                miYi = miYo - miXe * Math.Cos(Azimut * Math.PI / 180) + miYe * Math.Sin(Azimut * Math.PI / 180);
            }

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }

        public override List<double[]> getComponentPoints()
        {
            var puntos = new List<double[]>();
            double i = 0;
            double a = 0, b = 0;
            if (tipoclotoide == "Salida")
            {
                a = punto_final.X;
                b = punto_final.Y;
            }
            else
            {
                a = punto_inicial.X;
                b = punto_inicial.Y;

            }
            while ((Pk_inicial + i) < Pk_final)
            {
                puntos.Add(GetPointAtDistClotoide(i, Sentido, tipoclotoide, getLongitud(), radio, Azimut,a,b));
                i++;
            }
            return puntos;

            /*
            if (Le > 0)
            {
                if (tipoclotoide == "Salida")
                {
                    return (getComponentPoints(0, Le + Pk_inicial));
                }
                return (getComponentPoints(Le));
            }
            else
            {
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
            */

        }
        public override List<double[]> getComponentPoints(double pk)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((Pk_inicial + i) < Pk_final)
            {
                if (Pk_inicial + i >= pk)
                {
                    puntos.Add(getPointAtDist(Pk_inicial + i));
                }

                if (getLongitud() < 50)
                {
                    i += 0.1;
                }
                else
                {
                    if (getLongitud() > 5000)
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
        public override List<double[]> getComponentPoints(double pk, double pk_fin)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((Pk_inicial + i) < pk_fin)
            {
                if (Pk_inicial + i >= pk)
                {
                    puntos.Add(getPointAtDist(Pk_inicial + i));
                }

                if (getLongitud() < 50)
                {
                    i += 0.1;
                }
                else
                {
                    if (getLongitud() > 5000)
                    {
                        i += 10;
                    }
                    else
                    {
                        i++;
                    }

                }

            }
            puntos.Add(getPointAtDist(pk_fin));
            //base.setPuntoSalida=new Punto3d(puntos[puntos.Count-1][0], puntos[puntos.Count - 1][1],0);
            //puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }


        public static double[] GetPointAtDistClotoide(double iDistancia, string sentido, string io, double longitud, double radio, double az, double x, double y)
        {
            double mAzimut = az;
            double A = Math.Sqrt(radio * longitud);
            double[] miPunto = new double[2];
            double miLi, miXo, miYo;
            double miPK = (io == "Entrada") ? iDistancia : longitud - iDistancia;

            miLi = miPK;
            miYo = y;
            miXo = x;

            double miQi = 0.5 * Math.Pow((miLi / A), 2);
            double miRi = Math.Pow(A, 2) / miLi;
            double miXe = (1 - Math.Pow(miQi, 2) / 10 + Math.Pow(miQi, 4) / 216 - Math.Pow(miQi, 6) / 9360 + Math.Pow(miQi, 8) / 685440) * miLi;
            double miYe = ((miQi / 3) - (Math.Pow(miQi, 3) / 42) + (Math.Pow(miQi, 5) / 1320) - (Math.Pow(miQi, 7) / 75600)) * miLi;

            double miXi, miYi;
            double radAzimut = mAzimut * Math.PI / 180;

            if (sentido == "cw" && io == "Entrada")
            {
                miXi = miXo + miXe * Math.Sin(radAzimut) + miYe * Math.Cos(radAzimut);
                miYi = miYo + miXe * Math.Cos(radAzimut) - miYe * Math.Sin(radAzimut);
            }
            else if (sentido == "cw" && io == "Salida")
            {
                miXi = miXo - miXe * Math.Sin(radAzimut) + miYe * Math.Cos(radAzimut);
                miYi = miYo - miXe * Math.Cos(radAzimut) - miYe * Math.Sin(radAzimut);
            }
            else if (sentido == "ccw" && io == "Entrada")
            {
                miXi = miXo + miXe * Math.Sin(radAzimut) - miYe * Math.Cos(radAzimut);
                miYi = miYo + miXe * Math.Cos(radAzimut) + miYe * Math.Sin(radAzimut);
            }
            else
            {
                miXi = miXo - miXe * Math.Sin(radAzimut) - miYe * Math.Cos(radAzimut);
                miYi = miYo - miXe * Math.Cos(radAzimut) + miYe * Math.Sin(radAzimut);
            }

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }
        public override double getRadio()
        {
            return radio;
        }
        public override string getSentido()
        {
            return Sentido;
        }
        public override Punto3d getCentro()
        {
            return null;
        }
        public override double getLe()
        {
            return Le;
        }
        public override double getA()
        {
            return A;
        }
        public override string getTipoClotoide()
        {
            return tipoclotoide;
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
