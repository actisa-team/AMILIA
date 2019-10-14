using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;
using Logica.puntosDelEje;
using Logica.componentes;
namespace Logica.componentes
{
    
    public class Clotoide : Componente
    {

        public double mRc;
        public double mVariacionMI;
        public double mVariacionMD;
        public double mPeralteAntIzq;
        public double mPeralteAntDer;
        public double mAzimut;
        public EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide mTipo;
        public EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva mSentCurva;
        public bool mReducido;
        public double mDeltaRad;
        public double mA;
        public bool clotoideS;
        public double mLe;



        public Clotoide(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iRc, double iPkIni, Logica.puntosDelEje.EjeTrazado.sentidoCurva iSentCurva, double iPeralteAnt, double iPeraltePos, bool isSigCurva, Logica.puntosDelEje.EjeTrazado.tipoClotoide iTipoClotoide, double iAzimut, bool iReducido, double iDelta, bool isCurvaS, double miLe, double iA=0)
            : base(iPuntoEntrada, iPuntoSalida, iPkIni)
        {
            mRc = iRc;
            double miMICurva, miMDCurva, miMIRecta, miMDRecta;
            mReducido = iReducido;
            mTipo = iTipoClotoide;
            mAzimut = iAzimut;
            mSentCurva = iSentCurva;
            mDeltaRad = iDelta * Math.PI / 180;


            /* if ((!mReducido) && (!isCurvaS))
             {
                 clotoideS = false;
                 mA = Math.Pow(12 * Math.Pow(mRc, 3), 0.25);
             }
             else if ((mReducido) && (!isCurvaS))
             {
                 clotoideS = false;
                 mA = Math.Sqrt((Math.Pow(mRc, 2) * (mDeltaRad - 0.01)));
             }
             else
             {
                 clotoideS = true;
                 mA = Math.Sqrt(mRc * miLe);
                 mLe = miLe;
             }
             if (iA != 0) mA = iA;*/
             
            mA = iA;
            mLe = miLe;

            if (isSigCurva)
            {
                miMIRecta = iPeralteAnt * (-1);
                mPeralteAntIzq = miMIRecta;
                miMDRecta = iPeralteAnt;
                mPeralteAntDer = miMDRecta;
                if (iSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    miMICurva = iPeraltePos;
                    miMDCurva = iPeraltePos;
                }
                else
                {
                    miMICurva = iPeraltePos * (-1);
                    miMDCurva = iPeraltePos * (-1);
                }
                mVariacionMI = (miMICurva - miMIRecta) / getLongitud();
                mVariacionMD = (miMDCurva - miMDRecta) / getLongitud();
            }
            else
            {
                miMIRecta = iPeraltePos * (-1);
                miMDRecta = iPeraltePos;
                if (iSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario)
                {
                    miMICurva = iPeralteAnt;
                    miMDCurva = iPeralteAnt;
                }
                else
                {
                    miMICurva = iPeralteAnt * (-1);
                    miMDCurva = iPeralteAnt * (-1);
                }
                mPeralteAntIzq = miMICurva;
                mPeralteAntDer = miMDCurva;
                mVariacionMI = (miMIRecta - miMICurva) / getLongitud();
                mVariacionMD = (miMDRecta - miMDCurva) / getLongitud();
            }

            if (getLongitud() == 0)
            {
                mVariacionMI = 0;
                mVariacionMD = 0;
            }


        }

        public override double getLongitud()
        {
            double longitud;
            if (!curvaS)
            {
                longitud = (mA * mA / mRc);
                if ((getPuntoEntrada.coordenadaX == getPuntoSalida.coordenadaX) && (getPuntoEntrada.coordenadaY == getPuntoSalida.coordenadaY)) longitud = 0;
            }
            else
            {
                longitud = mLe;
            }
            return longitud;
        }

        public bool curvaS
        {
            get
            {
                return clotoideS;
            }
        }

        public override double getPkFinal()
        {
            return getPkIni + getLongitud();

        }

        public override double getMargenIzq(double iPk)
        {
            double miPK = iPk - getPkIni;
            return mVariacionMI * miPK + mPeralteAntIzq;
        }

        public override double getMargenDer(double iPk)
        {
            double miPK = iPk - getPkIni;
            return mVariacionMD * miPK + mPeralteAntDer;
        }


        public override double getPeralte()
        {
            return 0;
        }

        public override tipoComponente getTipoComponente()
        {
            if (mTipo == Logica.puntosDelEje.EjeTrazado.tipoClotoide.entrada)
            {
                return tipoComponente.clotoideEntrada;
            }
            else
            {
                return tipoComponente.clotoideSalida;
            }
        }

        public EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide getTipo
        {
            get
            {
                return mTipo;
            }
        }

        public double getQe()
        {
            double res;

            res = 0.5 * Math.Pow((getLongitud() / mA), 2);
            return res; 
        }

        public override double[] getPointAtDist(double iDistancia)
        {
            double[] miPunto = new double[2];
            double miLe;
            double miPK = iDistancia - getPkIni;
            miLe = mA * mA / mRc;
            double miLi, miXo, miYo;
            if (mTipo == Logica.puntosDelEje.EjeTrazado.tipoClotoide.entrada)
            {
                miLi = miPK;
                miXo = getPuntoEntrada.coordenadaX;
                miYo = getPuntoEntrada.coordenadaY;
            }
            else
            {
                miLi = getPkFinal() - iDistancia;
                miXo = getPuntoSalida.coordenadaX;  //salida?!
                miYo = getPuntoSalida.coordenadaY;

            }
            double miQi = 0.5 * Math.Pow((miLi / mA), 2);
            double miRi = Math.Pow(mA, 2) / miLi;
            double miXe = (1 - Math.Pow(miQi, 2) / 10 + Math.Pow(miQi, 4) / 216 - Math.Pow(miQi, 6) / 9360 + Math.Pow(miQi, 8) / 685440 - Math.Pow(miQi, 10) / 3628800 + Math.Pow(miQi, 12) / 479001600 - Math.Pow(miQi, 14) / 87178291200 + Math.Pow(miQi, 16) / 20922789888000) * miLi;
            double miYe = ((miQi / 3) - (Math.Pow(miQi, 3) / 42) + (Math.Pow(miQi, 5) / 1320) - (Math.Pow(miQi, 7) / 75600)+ (Math.Pow(miQi, 9) / 362880)- (Math.Pow(miQi, 11) / 39916800) + (Math.Pow(miQi, 13) / 6227020800)- (Math.Pow(miQi, 15) / 1307674368000)+ (Math.Pow(miQi, 17) / 355687428096000)) * miLi;

            double miXi, miYi;
            if ((mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada) && (mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario))
            {
                miXi = miXo + miXe * Math.Sin(mAzimut * Math.PI / 180) + miYe * Math.Cos(mAzimut * Math.PI / 180);
                miYi = miYo + miXe * Math.Cos(mAzimut * Math.PI / 180) - miYe * Math.Sin(mAzimut * Math.PI / 180);
            }
            else if ((mTipo == puntosDelEje.EjeTrazado.tipoClotoide.salida) && (mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario))
            {
                miXi = miXo - miXe * Math.Sin(mAzimut * Math.PI / 180) + miYe * Math.Cos(mAzimut * Math.PI / 180);
                miYi = miYo - miXe * Math.Cos(mAzimut * Math.PI / 180) - miYe * Math.Sin(mAzimut * Math.PI / 180);
            }
            else if ((mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada) && (mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Antihorario))
            {
                miXi = miXo + miXe * Math.Sin(mAzimut * Math.PI / 180) - miYe * Math.Cos(mAzimut * Math.PI / 180);
                miYi = miYo + miXe * Math.Cos(mAzimut * Math.PI / 180) + miYe * Math.Sin(mAzimut * Math.PI / 180);
            }
            else
            {
                miXi = miXo - miXe * Math.Sin(mAzimut * Math.PI / 180) - miYe * Math.Cos(mAzimut * Math.PI / 180);
                miYi = miYo - miXe * Math.Cos(mAzimut * Math.PI / 180) + miYe * Math.Sin(mAzimut * Math.PI / 180);
            }

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }

        public override double[] getPointAtLocation(double iDistancia, double iOffset, Logica.puntosDelEje.EjeTrazado.ladoCalzada iLadoCalzada)
        {
            double[] miPunto = new double[2];

            return miPunto;
        }

        public double[] getPointAtLocationClotiode(double iDistancia, double iOffset, Logica.puntosDelEje.EjeTrazado.ladoCalzada iLadoCalzada, double iAzimutAnt)
        {
            double[] miPunto = new double[3];
            double[] miPuntoDis = getPointAtDist(iDistancia);
            double tantoLongitud = 0;

            double miR;

            if (mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada)
            {
                miR = mA * mA / (iDistancia - getPkIni);
                tantoLongitud = ((iDistancia - getPkIni)) / getLongitud();
            }
            else
            {
                miR = mA * mA / (getLongitud() - (iDistancia - getPkIni));
                tantoLongitud = 1 - ((iDistancia - getPkIni)) / getLongitud();
            }

            //if (clotoideS)
            //{
            //    iAzimutAnt = getAzimut(getPuntoSalida.coordenadaX - getPuntoEntrada.coordenadaX, getPuntoSalida.coordenadaY - getPuntoEntrada.coordenadaY);
            //}


            if ((iAzimutAnt < 180) && (iAzimutAnt > 90))
            {
                iAzimutAnt = iAzimutAnt + 180;

            }
            else if ((iAzimutAnt < 360) && (iAzimutAnt > 270))
            {
                iAzimutAnt = iAzimutAnt - 180;
            }

            double miAzimutGra = iAzimutAnt;
            double miAzimutRad = miAzimutGra * Math.PI / 180;
            double miAzimutTrans = miAzimutGra;
            double miAnguloRad;
            if (mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada)
            {
                miAnguloRad = (iDistancia - getPkIni) / (2 * miR);
            }
            else
            {
                miAnguloRad = (getLongitud() - (iDistancia - getPkIni)) / (2 * miR);
            }
            if (mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                if (mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada)
                {
                    miAzimutTrans = (miAzimutRad - miAnguloRad) * 180 / Math.PI;
                }
                else
                {

                    miAzimutTrans = (miAzimutRad + miAnguloRad) * 180 / Math.PI;
                }
            }
            else
            {
                if (mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada)
                {
                    miAzimutTrans = (miAzimutRad + miAnguloRad) * 180 / Math.PI;
                }
                else
                {

                    miAzimutTrans = (miAzimutRad - miAnguloRad) * 180 / Math.PI;
                }
            }
            if (!clotoideS)
            {
                if (iLadoCalzada == puntosDelEje.EjeTrazado.ladoCalzada.Izquierda)
                {
                    if (miAzimutTrans + 90 > 360)
                    {
                        miAzimutTrans = miAzimutTrans + 90 - 360;
                    }
                    else
                    {
                        miAzimutTrans = miAzimutTrans + 90;
                    }

                }
                else
                {
                    if (miAzimutTrans + 270 > 360)
                    {
                        miAzimutTrans = miAzimutTrans + 270 - 360;
                    }
                    else
                    {
                        miAzimutTrans = miAzimutTrans + 270;
                    }
                }
            }
            else
            {
                double error = 10 + 2 * tantoLongitud;
                if (this.mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Antihorario && this.mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada) error = error * (-1);
                if (this.mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario && this.mTipo == puntosDelEje.EjeTrazado.tipoClotoide.salida) error = error * (-1);
                if (iLadoCalzada == puntosDelEje.EjeTrazado.ladoCalzada.Derecha)
                {
                    
                    if (miAzimutTrans + 90 > 360)
                    {
                        miAzimutTrans = miAzimutTrans + 90 - 360 + error;
                    }
                    else
                    {
                        miAzimutTrans = miAzimutTrans + 90 + error;
                    }

                }
                else
                {
                    if (miAzimutTrans + 270 > 360)
                    {
                        miAzimutTrans = miAzimutTrans + 270 - 360 + error;
                    }
                    else
                    {
                        miAzimutTrans = miAzimutTrans + 270 + error;
                    }
                }
            }

            miPunto[0] = miPuntoDis[0] + iOffset * Math.Cos(miAzimutTrans * Math.PI / 180);
            miPunto[1] = miPuntoDis[1] + iOffset * Math.Sin(miAzimutTrans * Math.PI / 180);

            miPunto[2] = miAzimutTrans;


            return miPunto;
        }






        public override double getVariacionMI()
        {
            return mVariacionMI;
        }

        public override double getVariacionMD()
        {
            return mVariacionMD;
        }

        public override Object draw()
        {
            List<Punto3d> miListaPuntos = new List<Punto3d>();

            double miPk = getPkIni + 1;
            double miPkfinal = getPkFinal();
            miListaPuntos.Add(getPuntoEntrada);
            while (miPk < miPkfinal)
            {
                double[] miPuntoCoord = new double[2];
                miPuntoCoord = getPointAtDist(miPk);
                Punto3d miPunto = new Punto3d(miPuntoCoord[0], miPuntoCoord[1], 0);
                miListaPuntos.Add(miPunto);
                miPk = miPk + 1;
            }

            miListaPuntos.Add(getPuntoSalida);
            return miListaPuntos;
        }

        public double getValorA()
        {
            return mA;
        }


        public double getRadio
        {
            get
            {
                return mRc;
            }
        }

        public double[] getPuntoInterseccion(Punto3d iCentroCurva, Punto3d iPuntoSECurva, Punto3d iPuntoERecta, Punto3d iPuntoSRecta)
        {
            double[] miPuntoInt = new double[3];
            double miXInt, miYInt;

            double miPendiente1, miPentiendePerp;
            if (iCentroCurva.coordenadaX != iPuntoSECurva.coordenadaX)
            {
                miPendiente1 = (iPuntoSECurva.coordenadaY - iCentroCurva.coordenadaY) / (iPuntoSECurva.coordenadaX - iCentroCurva.coordenadaX);

                if ((iCentroCurva.coordenadaX < iPuntoSECurva.coordenadaX) && (iCentroCurva.coordenadaY < iPuntoSECurva.coordenadaY))
                {
                    //cuadrante 1
                    miPentiendePerp = 1 / miPendiente1 * (-1);
                }
                else if ((iCentroCurva.coordenadaX > iPuntoSECurva.coordenadaX) && (iCentroCurva.coordenadaY < iPuntoSECurva.coordenadaY))
                {
                    //cuadrante 2
                    miPentiendePerp = (1 / miPendiente1) * (-1);
                }
                else if ((iCentroCurva.coordenadaX > iPuntoSECurva.coordenadaX) && (iCentroCurva.coordenadaY > iPuntoSECurva.coordenadaY))
                {
                    //cuadrante 3
                    miPentiendePerp = (1 / miPendiente1) * (-1);
                }
                else
                {
                    //cuadrante 4
                    miPentiendePerp = (1 / miPendiente1) * (-1);
                }

                double miX = iPuntoSECurva.coordenadaX + 5;
                double miY = miPentiendePerp * (miX - iPuntoSECurva.coordenadaX) + iPuntoSECurva.coordenadaY;

                miXInt = getXinterseccion(iPuntoERecta, iPuntoSRecta, iPuntoSECurva, new Punto3d(miX, miY, 0));
                miYInt = getY(iPuntoERecta, iPuntoSRecta, miXInt);

            }
            else
            {
                miXInt = iCentroCurva.coordenadaX;
                miYInt = getY(iPuntoERecta, iPuntoSRecta, miXInt);

            }

            miPuntoInt[0] = miXInt;
            miPuntoInt[1] = miYInt;
            miPuntoInt[2] = 0;

            return miPuntoInt;
        }




        private double getXinterseccion(Punto3d PuntoA1, Punto3d PuntoA2, Punto3d PuntoB1, Punto3d PuntoB2)
        {
            double miBc = ((PuntoB2.coordenadaY - PuntoB1.coordenadaY) / (PuntoB2.coordenadaX - PuntoB1.coordenadaX));
            double miAc = ((PuntoA2.coordenadaY - PuntoA1.coordenadaY) / (PuntoA2.coordenadaX - PuntoA1.coordenadaX));
            double x = (PuntoB1.coordenadaX * miBc - PuntoB1.coordenadaY - PuntoA1.coordenadaX * miAc + PuntoA1.coordenadaY) / (miBc - miAc);

            return x;

        }

        private double getY(Punto3d iPunto1, Punto3d iPunto2, double iX)
        {
            double y = iPunto1.coordenadaY + (((iX - iPunto1.coordenadaX) / (iPunto2.coordenadaX - iPunto1.coordenadaX)) * (iPunto2.coordenadaY - iPunto1.coordenadaY));
            return y;
        }

        private double getX(Punto3d iPunto1, Punto3d iPunto2, double iY)
        {
            double x = iPunto1.coordenadaX + (((iY - iPunto1.coordenadaY) / (iPunto2.coordenadaY - iPunto1.coordenadaY)) * (iPunto2.coordenadaX - iPunto1.coordenadaX));
            return x;
        }

        public bool isHorario()
        {
            bool resul;
            if (mSentCurva == puntosDelEje.EjeTrazado.sentidoCurva.Horario)
            {
                resul = true;
            }
            else
            {
                resul = false;
            }
            return resul;
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
                    i++;
                }
                
            }
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
    }
}