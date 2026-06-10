using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;

using Newtonsoft.Json;

namespace EjeDeTrazado.componentes
{

    [Serializable]
    public class Linea : Componente
    {
        [JsonProperty]
        private double mPeralte;
        [JsonProperty]
        private double mAzimut;
        [JsonProperty]
        private bool mCurvaS=false;
        [JsonProperty]
        private double mPuntoEX;
        [JsonProperty]
        private double mPuntoEY;
        [JsonProperty]
        private double mPuntoSX;
        [JsonProperty]
        private double mPuntoSY;

        public Linea() : base(new Punto3d(0, 0, 0), new Punto3d(0, 0, 0), 0)
        {
        }

        public Linea(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iPkIni, double iPeralte, double iAzimut)
            : base(iPuntoEntrada, iPuntoSalida, iPkIni)
        {
            mPeralte = iPeralte;
            mAzimut = iAzimut;
            mPuntoEX = iPuntoEntrada.coordenadaX;
            mPuntoEY = iPuntoEntrada.coordenadaY;
            mPuntoSX = iPuntoSalida.coordenadaX;
            mPuntoSY = iPuntoSalida.coordenadaY;

        }

        public Linea(Punto3d iPuntoEntrada,Punto3d puntoSalida,  Punto3d puntoES, double iPkIni, double iPeralte, double iAzimut)
            : base(puntoES, puntoES, iPkIni)
        {
            mCurvaS = true;
            mPeralte = iPeralte;
            mAzimut = iAzimut;
            mPuntoEX = iPuntoEntrada.coordenadaX;
            mPuntoEY = iPuntoEntrada.coordenadaY;
            mPuntoSX = puntoSalida.coordenadaX;
            mPuntoSY = puntoSalida.coordenadaY;
            
        }
        public override double getLongitud()
        {
            if (!mCurvaS)
            {
                return Math.Sqrt(Math.Pow((getPuntoSalida.coordenadaX - getPuntoEntrada.coordenadaX), 2) + Math.Pow((getPuntoSalida.coordenadaY - getPuntoEntrada.coordenadaY), 2));
            }
            else
            {
                return 0;
            }

        }

        public override double getPkFinal()
        {
                return getPkIni + getLongitud();
            
        }

        public override double getMargenIzq(double iPk)
        {
            return mPeralte * -1;
        }

        public override double getMargenDer(double iPk)
        {
            return mPeralte;
        }


        public override double getPeralte()
        {
            return mPeralte;
        }


        public override double getVariacionMI()
        {
            return 0;
        }

        public override double getVariacionMD()
        {
            return 0;
        }

        public override tipoComponente getTipoComponente()
        {
            return tipoComponente.linea;
        }


        //Curvas en S
        public double azimut
        {
            get
            {
                if (mCurvaS)
                {
                    return getAzimut(mPuntoSX - mPuntoEX, mPuntoSY - mPuntoEY) ;
                }
                else
                {
                    return getAzimut(mPuntoSX - mPuntoEX, mPuntoSY - mPuntoEY);
                }
            }
        }

        public double AzimutFinal
        {
            get
            {
                return mAzimut;
            }
        }

        public override double[] getPointAtDist(double iDistancia)
        {
            double[] miPunto = new double[2];
            double miPK = iDistancia - getPkIni;
            double miXo, miYo;
                 miXo = getPuntoEntrada.coordenadaX;
                 miYo = getPuntoEntrada.coordenadaY;
           

            double miXi = miXo + miPK * Math.Sin(mAzimut * Math.PI / 180);
            double miYi = miYo + miPK * Math.Cos(mAzimut * Math.PI / 180);

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }

        public override double[] getPointAtLocation(double iDistancia, double iOffset, EjeDeTrazado.puntosDelEje.EjeTrazado.ladoCalzada iLadoCalzada)
        {
            double[] miPuntoDis = getPointAtDist(iDistancia);
            double[] miPunto = new double[3];
            double miD1, miD2;
            if (!mCurvaS)
            {
                 miD1 = getPuntoSalida.coordenadaX - getPuntoEntrada.coordenadaX;
                 miD2 = getPuntoSalida.coordenadaY - getPuntoEntrada.coordenadaY;
            }
            else
            {
                miD1 = getPuntoSalida.coordenadaX - getPuntoEntrada.coordenadaX;
                miD2 = getPuntoSalida.coordenadaY - getPuntoEntrada.coordenadaY;
            }

            double miDelta;
            if ((miD1 == 0) || (miD2 == 0))
            {
                miDelta = 0;
            }
            else
            {
                miDelta = Math.Atan(miD1 / miD2);
            }
            double miDeltaGra = miDelta * 180 / Math.PI;

            double miAzimut = getAzimut(miD1, miD2);
            double miAzimutTrans;


            if ((miAzimut < 180) && (miAzimut > 90))
            {
                miAzimut = miAzimut + 180;

            }else if ((miAzimut < 360) && (miAzimut > 270))
            {
                miAzimut = miAzimut - 180;
            }
            if (iLadoCalzada == puntosDelEje.EjeTrazado.ladoCalzada.Derecha)
            {
                if (miAzimut - 90 >= 0)
                {
                    miAzimutTrans = miAzimut - 90;
                }
                else
                {
                    miAzimutTrans = miAzimut - 90 +360;
                }
               
            }
            else
            {
                if (miAzimut + 90 >= 360)
                {
                    miAzimutTrans = miAzimut + 90 - 360;
                }
                else
                {
                    miAzimutTrans = miAzimut + 90;
                }
            }

            miPunto[0] = miPuntoDis[0] + iOffset * Math.Cos(miAzimutTrans * Math.PI / 180);
            miPunto[1] = miPuntoDis[1] + iOffset * Math.Sin(miAzimutTrans * Math.PI / 180);
            miPunto[2] = miAzimutTrans;
            
            return miPunto;
        }

        public override Object draw()
        {
            List<Punto3d> miListaPuntos = new List<Punto3d>();
                miListaPuntos.Add(getPuntoEntrada);
                miListaPuntos.Add(getPuntoSalida);
           
            
            return miListaPuntos;
        }


        private double getDelta(double iAzimut1, double iAzimut2)
        {
            double miDelta;
            if (Math.Abs(iAzimut2 - iAzimut1) > 180)
            {
                miDelta = 360 - Math.Abs(iAzimut2 - iAzimut1);
            }
            else
            {
                miDelta = Math.Abs(iAzimut2 - iAzimut1);
            }
            return miDelta;
        }

        //no es de la recta, metodo static (solo calculo)
        public double getAzimut(double iDx, double iDy)
        {

            double miDelta;
            if ((iDx == 0) || (iDy == 0))
            {
                miDelta = 0;
            }
            else
            {
                miDelta = Math.Atan(iDx / iDy);
            }
            double miDeltaGra = miDelta * 180 / Math.PI;
            double miAzimut;

            if (miDeltaGra == 0)
            {
                if (iDy == 0)
                {
                    if (iDx < 0)
                    {
                        miAzimut = 180;
                    }
                    else
                    {
                        miAzimut = 0;
                    }
                }
                else
                {
                    if (iDx < 0)
                    {
                        miAzimut = 270;
                    }
                    else
                    {
                        miAzimut = 90;
                    }
                }
            }
            else
            {
                if (miDeltaGra < 0)
                {
                    if (iDx >= 0)
                    {
                        miAzimut = 90 - miDeltaGra;
                    }
                    else
                    {
                        miAzimut = 270 - miDeltaGra;
                    }
                }
                else
                {
                    if (iDy >= 0)
                    {
                        miAzimut = 90 - miDeltaGra;
                    }
                    else
                    {
                        miAzimut = 270 - miDeltaGra;
                    }
                }
            }
            return miAzimut;
        }


        public double[] getPuntoEntradaInterseccion()
        {
            double[] miPunto = new double[2];

            miPunto[0] = mPuntoEX;
            miPunto[1] = mPuntoEY;

            return miPunto;
        }

        public double[] getPuntoSalidaInterseccion()
        {
            double[] miPunto = new double[2];

            miPunto[0] = mPuntoSX;
            miPunto[1] = mPuntoSY;

            return miPunto;
        }
        public override List<double[]> getComponentPoints()
        {
            var puntos = new List<double[]>();
            puntos.Add(new double[] { getPuntoEntrada.coordenadaX, getPuntoEntrada.coordenadaY });
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints(double pk)
        {
            var puntos = new List<double[]>();
            puntos.Add(new double[] { getPuntoEntrada.coordenadaX, getPuntoEntrada.coordenadaY });
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints(double pk,double pk_fin)
        {
            var puntos = new List<double[]>();
            puntos.Add(new double[] { getPuntoEntrada.coordenadaX, getPuntoEntrada.coordenadaY });
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints_p()
        {
            var puntos = new List<double[]>();
            puntos.Add(new double[] { getPuntoEntrada.coordenadaX, getPuntoEntrada.coordenadaY });
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints_p(double pk)
        {
            var puntos = new List<double[]>();
            puntos.Add(new double[] { getPuntoEntrada.coordenadaX, getPuntoEntrada.coordenadaY });
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints_p(double pk, double pk_fin)
        {
            var puntos = new List<double[]>();
            puntos.Add(new double[] { getPuntoEntrada.coordenadaX, getPuntoEntrada.coordenadaY });
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override double Get_Le_r()
        {
            return -1;
        }
        public override double Get_Le_m()
        {
            return -1;
        }
        public override EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva getSentido()
        {
            return EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.noValorado;
        }
        public override double get_Radio()
        {
            return 0;
        }
        public override Punto3d get_Centro()
        {
            return new Punto3d(0,0, 0);
        }
    }
}
