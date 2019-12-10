using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;


namespace EjeDeTrazado.componentes
{
    
    public class Curva : Componente
    {
        private double mCentroCurvaX;
        private double mCentroCurvaY;
        private double mRc;
        private double mPeralte;
        private EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva mSentCurva;
        private double mPkfinal;
        private bool mIsCurvaGranRadio;


        public Curva(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, Punto3d iCentroCurva, double iRc, double iPkIni, double iPeralte, EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva iSentCurva)
            : base(iPuntoEntrada, iPuntoSalida, iPkIni)
        {
            mCentroCurvaX = iCentroCurva.coordenadaX;
            mCentroCurvaY = iCentroCurva.coordenadaY;
            mRc = iRc;
            mPeralte = iPeralte;
            mSentCurva = iSentCurva;
            mPkfinal = iPkIni + getLongitud();
            if (mRc == 5000 || mRc == 2500)
            {
                mIsCurvaGranRadio = true;
            }
            else
            {
                mIsCurvaGranRadio = false;
            }
        }


        public Punto3d getCentroCurva
        {
            get
            {
                return new Punto3d(mCentroCurvaX, mCentroCurvaY, 0);
            }
        }

        public double getRadio
        {
            get
            {
                return mRc;
            }
        }

        public override double getLongitud()
        {

            double miCambioAng = 0;

            double miPendiente1, miPendiente2;
            double miAnguloEntrada, miAnguloSalida;

            if (mCentroCurvaX != getPuntoEntrada.coordenadaX)
            {
                miPendiente1 = (getPuntoEntrada.coordenadaY - mCentroCurvaY) / (getPuntoEntrada.coordenadaX - mCentroCurvaX);
                if ((mCentroCurvaX < getPuntoEntrada.coordenadaX) && (mCentroCurvaY < getPuntoEntrada.coordenadaY))
                {
                    //cuadrante 1
                    miAnguloEntrada = Math.Atan(miPendiente1);
                }
                else if ((mCentroCurvaX > getPuntoEntrada.coordenadaX) && (mCentroCurvaY < getPuntoEntrada.coordenadaY))
                {
                    //cuadrante 2
                    miAnguloEntrada = Math.Atan(miPendiente1) + Math.PI;
                }
                else if ((mCentroCurvaX > getPuntoEntrada.coordenadaX) && (mCentroCurvaY > getPuntoEntrada.coordenadaY))
                {
                    //cuadrante 3
                    miAnguloEntrada = Math.Atan(miPendiente1) + Math.PI;
                }
                else
                {
                    //cuadrante 4
                    miAnguloEntrada = Math.Atan(miPendiente1) + 2 * Math.PI;
                }
            }
            else
            {
                miPendiente1 = 0;
                if (mCentroCurvaY < getPuntoEntrada.coordenadaY)
                {
                    miAnguloEntrada = Math.PI;
                }
                else
                {
                    miAnguloEntrada = 3 * Math.PI / 2;
                }
            }
            if (mCentroCurvaX != getPuntoSalida.coordenadaX)
            {
                miPendiente2 = (getPuntoSalida.coordenadaY - mCentroCurvaY) / (getPuntoSalida.coordenadaX - mCentroCurvaX);
                if ((mCentroCurvaX < getPuntoSalida.coordenadaX) && (mCentroCurvaY < getPuntoSalida.coordenadaY))
                {
                    //cuadrante 1
                    miAnguloSalida = Math.Atan(miPendiente2);
                }
                else if ((mCentroCurvaX > getPuntoSalida.coordenadaX) && (mCentroCurvaY < getPuntoSalida.coordenadaY))
                {
                    //cuadrante 2
                    miAnguloSalida = Math.Atan(miPendiente2) + Math.PI;
                }
                else if ((mCentroCurvaX > getPuntoSalida.coordenadaX) && (mCentroCurvaY > getPuntoSalida.coordenadaY))
                {
                    //cuadrante 3
                    miAnguloSalida = Math.Atan(miPendiente2) + Math.PI;
                }
                else
                {
                    //cuadrante 4
                    miAnguloSalida = Math.Atan(miPendiente2) + 2 * Math.PI;
                }
            }
            else
            {
                miPendiente2 = 0;
                if (mCentroCurvaY < getPuntoSalida.coordenadaY)
                {
                    miAnguloSalida = Math.PI;
                }
                else
                {
                    miAnguloSalida = 3 * Math.PI / 2;
                }
            }

            if ((mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario))
            {
                if (miAnguloEntrada < miAnguloSalida)
                {
                    miCambioAng = (2 * Math.PI - Math.Abs(miAnguloSalida - miAnguloEntrada)) * 180 / Math.PI;
                }
                else
                {

                    miCambioAng = (Math.Abs(miAnguloSalida - miAnguloEntrada)) * 180 / Math.PI;
                }
            }
            else if ((mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Antihorario))
            {
                if (miAnguloEntrada < miAnguloSalida)
                {
                    miCambioAng = (Math.Abs(miAnguloSalida - miAnguloEntrada)) * 180 / Math.PI;
                }
                else
                {
                    miCambioAng = (2 * Math.PI - Math.Abs(miAnguloSalida - miAnguloEntrada)) * 180 / Math.PI;
                }
            }

            double longitud = (2 * miCambioAng * Math.PI * mRc) / 360;
            if ((getPuntoEntrada.coordenadaX == getPuntoSalida.coordenadaX) && (getPuntoEntrada.coordenadaY == getPuntoSalida.coordenadaY))
            {
                longitud = 0;
            }


            return longitud;
        }

        public override double getPkFinal()
        {
            return getPkIni + getLongitud();

        }

        public override double getMargenIzq(double iPk)
        {
            double res = 0;
            if (mIsCurvaGranRadio)
            {
                res = getMargenIzqGranRadio(iPk);
            }
            else
            {
                if (mSentCurva == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    res = mPeralte;
                }
                else
                {
                    res = mPeralte * (-1);
                }
            }
            return res;
        }

        public override double getMargenDer(double iPk)
        {
            double res = 0;
            if (mIsCurvaGranRadio)
            {
                res = getMargenDerGranRadio(iPk);
            }
            else
            {
                if (mSentCurva == EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    res = mPeralte;
                }
                else
                {
                    res = mPeralte * (-1);
                }
            }
            return res;
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
            return tipoComponente.curva;
        }

        public override double[] getPointAtDist(double iDistancia)
        {
            double[] miPunto = new double[2];
            double miPK = iDistancia - getPkIni;
            double miCambAng = miPK / mRc;
            double miXo = getPuntoEntrada.coordenadaX;
            double miYo = getPuntoEntrada.coordenadaY;
            double miAzimut;
            if (miXo > mCentroCurvaX)
            {
                if (miYo > mCentroCurvaY)
                {
                    miAzimut = 90 - 180 / Math.PI * Math.Atan((miYo - mCentroCurvaY) / (miXo - mCentroCurvaX));
                }
                else
                {
                    miAzimut = 90 + 180 / Math.PI * Math.Atan((mCentroCurvaY - miYo) / (miXo - mCentroCurvaX));
                }

            }
            else
            {
                if (miYo > mCentroCurvaY)
                {
                    miAzimut = 270 + 180 / Math.PI * Math.Atan((miYo - mCentroCurvaY) / (mCentroCurvaX - miXo));
                }
                else
                {
                    miAzimut = 270 - 180 / Math.PI * Math.Atan((mCentroCurvaY - miYo) / (mCentroCurvaX - miXo));
                }
            }
            double miAzFinal;
            if (mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                miAzFinal = miAzimut + miCambAng * 180 / Math.PI;
            }
            else
            {
                miAzFinal = miAzimut - miCambAng * 180 / Math.PI;
            }
            double miXi = mCentroCurvaX + mRc * Math.Sin(miAzFinal * Math.PI / 180);
            double miYi = mCentroCurvaY + mRc * Math.Cos(miAzFinal * Math.PI / 180);

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }

        public override double[] getPointAtLocation(double iDistancia, double iOffset, EjeDeTrazado.puntosDelEje.EjeTrazado.ladoCalzada iLadoCalzada)
        {

            double[] miPuntoDis = getPointAtDist(iDistancia);
            double[] miPunto = new double[3];

            double miD1 = mCentroCurvaX - miPuntoDis[0];
            double miD2 = mCentroCurvaY - miPuntoDis[1];

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

            if ((miAzimut < 180) && (miAzimut > 90))
            {
                miAzimut = miAzimut + 180;

            }
            else if ((miAzimut < 360) && (miAzimut > 270))
            {
                miAzimut = miAzimut - 180;
            }
            double miAzimutTrans;

            if (iLadoCalzada == puntosDelEje.EjeTrazado.ladoCalzada.Derecha)
            {
                if (miAzimut - 90 > 0)
                {
                    miAzimutTrans = miAzimut - 90;
                }
                else
                {
                    miAzimutTrans = miAzimut - 90 + 360;
                }

            }
            else
            {
                if (miAzimut + 90 > 360)
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
            double[] miListaValores = new double[5];
            //los valores seran la coordenada x del centro, la coordenada y, el radio, angulo 1, angulo 2

            miListaValores[0] = mCentroCurvaX;
            miListaValores[1] = mCentroCurvaY;
            //miListaValores[2] = getPuntoEntrada.coordenadaX;
            //miListaValores[3] = getPuntoEntrada.coordenadaY;
            //miListaValores[4] = getPuntoSalida.coordenadaX;
            //miListaValores[5] = getPuntoSalida.coordenadaY;


            miListaValores[2] = mRc;

            double miPendiente1, miPendiente2;
            double miAnguloEntrada, miAnguloSalida;

            if (mCentroCurvaX != getPuntoEntrada.coordenadaX)
            {
                miPendiente1 = (getPuntoEntrada.coordenadaY - mCentroCurvaY) / (getPuntoEntrada.coordenadaX - mCentroCurvaX);
                if ((mCentroCurvaX < getPuntoEntrada.coordenadaX) && (mCentroCurvaY < getPuntoEntrada.coordenadaY))
                {
                    //cuadrante 1
                    miAnguloEntrada = Math.Atan(miPendiente1);
                }
                else if ((mCentroCurvaX > getPuntoEntrada.coordenadaX) && (mCentroCurvaY < getPuntoEntrada.coordenadaY))
                {
                    //cuadrante 2
                    miAnguloEntrada = Math.Atan(miPendiente1) + Math.PI;
                }
                else if ((mCentroCurvaX > getPuntoEntrada.coordenadaX) && (mCentroCurvaY > getPuntoEntrada.coordenadaY))
                {
                    //cuadrante 3
                    miAnguloEntrada = Math.Atan(miPendiente1) + Math.PI;
                }
                else
                {
                    //cuadrante 4
                    miAnguloEntrada = Math.Atan(miPendiente1) + 2 * Math.PI;
                }
            }
            else
            {
                miPendiente1 = 0;
                if (mCentroCurvaY < getPuntoEntrada.coordenadaY)
                {
                    miAnguloEntrada = Math.PI;
                }
                else
                {
                    miAnguloEntrada = 3 * Math.PI / 2;
                }
            }
            if (mCentroCurvaX != getPuntoSalida.coordenadaX)
            {
                miPendiente2 = (getPuntoSalida.coordenadaY - mCentroCurvaY) / (getPuntoSalida.coordenadaX - mCentroCurvaX);
                if ((mCentroCurvaX < getPuntoSalida.coordenadaX) && (mCentroCurvaY < getPuntoSalida.coordenadaY))
                {
                    //cuadrante 1
                    miAnguloSalida = Math.Atan(miPendiente2);
                }
                else if ((mCentroCurvaX > getPuntoSalida.coordenadaX) && (mCentroCurvaY < getPuntoSalida.coordenadaY))
                {
                    //cuadrante 2
                    miAnguloSalida = Math.Atan(miPendiente2) + Math.PI;
                }
                else if ((mCentroCurvaX > getPuntoSalida.coordenadaX) && (mCentroCurvaY > getPuntoSalida.coordenadaY))
                {
                    //cuadrante 3
                    miAnguloSalida = Math.Atan(miPendiente2) + Math.PI;
                }
                else
                {
                    //cuadrante 4
                    miAnguloSalida = Math.Atan(miPendiente2) + 2 * Math.PI;
                }
            }
            else
            {
                miPendiente2 = 0;
                if (mCentroCurvaY < getPuntoSalida.coordenadaY)
                {
                    miAnguloSalida = Math.PI;
                }
                else
                {
                    miAnguloSalida = 3 * Math.PI / 2;
                }
            }

            if ((mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario))
            {
                miListaValores[3] = miAnguloSalida;
                miListaValores[4] = miAnguloEntrada;
            }
            else if ((mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Antihorario))
            {

                miListaValores[3] = miAnguloEntrada;
                miListaValores[4] = miAnguloSalida;
            }

            return miListaValores;
        }

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


            if (mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                if (miAzimut - 90 >= 0)
                {
                    miAzimut = miAzimut - 90;
                }
                else
                {
                    miAzimut = miAzimut - 90 + 360;
                }

            }
            else
            {
                if (miAzimut + 90 >= 360)
                {
                    miAzimut = miAzimut + 90 - 360;
                }
                else
                {
                    miAzimut = miAzimut + 90;
                }
            }
            return miAzimut;
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

        public EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva getSentCurva
        {
            get
            {
                return mSentCurva;
            }
        }

        public Punto3d puntoMedioCurva
        {
            get
            {
                double miDistancia = getPkIni + (getPkFinal() - getPkIni) / 2;
                double[] miPto = getPointAtDist(miDistancia);

                return new Punto3d(miPto[0], miPto[1], 0);
            }
        }

        private double getMargenIzqGranRadio(double iPk)
        {
            return mPeralte * -1;
        }

        private double getMargenDerGranRadio(double iPk)
        {
            return mPeralte;
        }

        public double getAzimutAtDist(double iDistancia)
        {

            double[] miPunto = new double[2];
            double miPK = iDistancia - getPkIni;
            double miCambAng = miPK / mRc;
            double miXo = getPuntoEntrada.coordenadaX;
            double miYo = getPuntoEntrada.coordenadaY;
            double miAzimut;
            if (miXo > mCentroCurvaX)
            {
                if (miYo > mCentroCurvaY)
                {
                    miAzimut = 90 - 180 / Math.PI * Math.Atan((miYo - mCentroCurvaY) / (miXo - mCentroCurvaX));
                }
                else
                {
                    miAzimut = 90 + 180 / Math.PI * Math.Atan((mCentroCurvaY - miYo) / (miXo - mCentroCurvaX));
                }

            }
            else
            {
                if (miYo > mCentroCurvaY)
                {
                    miAzimut = 270 + 180 / Math.PI * Math.Atan((miYo - mCentroCurvaY) / (mCentroCurvaX - miXo));
                }
                else
                {
                    miAzimut = 270 - 180 / Math.PI * Math.Atan((mCentroCurvaY - miYo) / (mCentroCurvaX - miXo));
                }
            }
            double miAzFinal;
            if (mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                miAzFinal = miAzimut + miCambAng * 180 / Math.PI;
            }
            else
            {
                miAzFinal = miAzimut - miCambAng * 180 / Math.PI;
            }

            return miAzFinal;

        }

        public double getAzimutTransAtDist(double iPk, puntosDelEje.EjeTrazado.ladoCalzada iLadoCalzada)
        {

            double[] miPuntoDis = getPointAtDist(iPk);
            double[] miPunto = new double[3];

            double miD1 = mCentroCurvaX - miPuntoDis[0];
            double miD2 = mCentroCurvaY - miPuntoDis[1];

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

            if ((miAzimut < 180) && (miAzimut > 90))
            {
                miAzimut = miAzimut + 180;

            }
            else if ((miAzimut < 360) && (miAzimut > 270))
            {
                miAzimut = miAzimut - 180;
            }

            double miAzimutTrans;

            if (iLadoCalzada == puntosDelEje.EjeTrazado.ladoCalzada.Derecha)
            {
                if (miAzimut - 90 > 0)
                {
                    miAzimutTrans = miAzimut - 90;
                }
                else
                {
                    miAzimutTrans = miAzimut - 90 + 360;
                }

            }
            else
            {
                if (miAzimut + 90 > 360)
                {
                    miAzimutTrans = miAzimut + 90 - 360;
                }
                else
                {
                    miAzimutTrans = miAzimut + 90;
                }
            }

            return miAzimutTrans;

        }
        public override List<double[]> getComponentPoints()
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((getPkIni + i) < getPkFinal())
            {
                puntos.Add(getPointAtDist(getPkIni + i));
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
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints(double pk)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((getPkIni + i) < getPkFinal())
            {
                puntos.Add(getPointAtDist(getPkIni + i));
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
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints(double pk,double pk_fin)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((getPkIni + i) < getPkFinal())
            {
                puntos.Add(getPointAtDist(getPkIni + i));
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
    }
    
}

