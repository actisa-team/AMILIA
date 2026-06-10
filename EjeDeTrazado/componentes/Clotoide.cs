using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;


using Newtonsoft.Json;

namespace EjeDeTrazado.componentes
{
    [Serializable]
    public class Clotoide : Componente
    {

        [JsonProperty]
        public double mRc;
        [JsonProperty]
        public double mVariacionMI;
        [JsonProperty]
        public double mVariacionMD;
        [JsonProperty]
        public double mPeralteAntIzq;
        [JsonProperty]
        public double mPeralteAntDer;
        [JsonProperty]
        public double mAzimut;
        public EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide mTipo;
        public EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva mSentCurva;
        [JsonProperty]
        public bool mReducido;
        [JsonProperty]
        public double mDeltaRad;
        [JsonProperty]
        public double mA;
        [JsonProperty]
        public bool clotoideS;
        [JsonProperty]
        public double mLe;
        [JsonProperty]
        public double le_r;




        public Clotoide() : base(new Punto3d(0, 0, 0), new Punto3d(0, 0, 0), 0)
        {
        }

        public Clotoide(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iRc, double iPkIni, EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva iSentCurva, double iPeralteAnt, double iPeraltePos, bool isSigCurva, EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide iTipoClotoide, double iAzimut, bool iReducido, double iDelta, bool isCurvaS, double miLe,double miA)
            : base(iPuntoEntrada, iPuntoSalida, iPkIni)
        {
            mRc = iRc;
            double miMICurva, miMDCurva, miMIRecta, miMDRecta;
            mReducido = iReducido;
            mTipo = iTipoClotoide;
            mAzimut = iAzimut;
            mSentCurva = iSentCurva;
            mDeltaRad = iDelta * Math.PI / 180;
            le_r = 0;

            if ((!mReducido) && (!isCurvaS))
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
            mA = miA;
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
        public Clotoide(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iRc, double iPkIni, EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva iSentCurva, double iPeralteAnt, double iPeraltePos, bool isSigCurva, EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide iTipoClotoide, double iAzimut, bool iReducido, double iDelta, bool isCurvaS, double miLe, double miA,double miLer)
           : base(iPuntoEntrada, iPuntoSalida, iPkIni)
        {
            mRc = iRc;
            double miMICurva, miMDCurva, miMIRecta, miMDRecta;
            mReducido = iReducido;
            mTipo = iTipoClotoide;
            mAzimut = iAzimut;
            mSentCurva = iSentCurva;
            mDeltaRad = iDelta * Math.PI / 180;
            le_r = miLer;

            if ((!mReducido) && (!isCurvaS))
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
            mA = miA;
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
        public Clotoide(Punto3d iPuntoEntrada, Punto3d iPuntoSalida, double iRc, double iPkIni, EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva iSentCurva, double iPeralteAnt, double iPeraltePos, bool isSigCurva, EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide iTipoClotoide, double iAzimut, bool iReducido, double iDelta, bool isCurvaS, double miLe)
            : base(iPuntoEntrada, iPuntoSalida, iPkIni)
        {
            mRc = iRc;
            double miMICurva, miMDCurva, miMIRecta, miMDRecta;
            mReducido = iReducido;
            mTipo = iTipoClotoide;
            mAzimut = iAzimut;
            mSentCurva = iSentCurva;
            mDeltaRad = iDelta * Math.PI / 180;


            if ((!mReducido) && (!isCurvaS))
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
        public override double Get_Le_r()
        {
            return le_r;
        }
        public override double Get_Le_m()
        {
            return mLe;
        }
        public double getLe_r()
        {
            return le_r;
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
            if (mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada)
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
        public double getRadio
        {
            get
            {
                return mRc;
            }
        }

        public override double[] getPointAtDist(double iDistancia)
        {
            double[] miPunto = new double[2];
            double miLe;
            double miPK = iDistancia - getPkIni;
            miLe = mA * mA / mRc;
            double miLi, miXo, miYo;
            

            double miXi, miYi;
                if (mTipo == puntosDelEje.EjeTrazado.tipoClotoide.entrada)
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
                double miXe = (1 - Math.Pow(miQi, 2) / 10 + Math.Pow(miQi, 4) / 216 - Math.Pow(miQi, 6) / 9360 + Math.Pow(miQi, 8) / 685440
                    - Get_re(miQi, 10) + Get_re(miQi, 12) - Get_re(miQi, 14) + Get_re(miQi, 16) - Get_re(miQi, 18) + Get_re(miQi, 20) - Get_re(miQi, 22)) * miLi;
                double miYe = ((miQi / 3) - (Math.Pow(miQi, 3) / 42) + (Math.Pow(miQi, 5) / 1320) - (Math.Pow(miQi, 7) / 75600)
                     + Get_re(miQi, 9) - Get_re(miQi, 11) + Get_re(miQi, 13) - Get_re(miQi, 15) + Get_re(miQi, 17) - Get_re(miQi, 19) + Get_re(miQi, 21) - Get_re(miQi, 23)) * miLi;
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

        public override double[] getPointAtLocation(double iDistancia, double iOffset, EjeDeTrazado.puntosDelEje.EjeTrazado.ladoCalzada iLadoCalzada)
        {
            double[] miPunto = new double[2];

            return miPunto;
        }

        public double[] getPointAtLocationClotiode(double iDistancia, double iOffset, EjeDeTrazado.puntosDelEje.EjeTrazado.ladoCalzada iLadoCalzada, double iAzimutAnt)
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


        public override double get_Radio()
        {

                return mRc;
            
        }
        public override Punto3d get_Centro()
        {
            return new Punto3d(0,0, 0);
        }
        public override EjeDeTrazado.puntosDelEje.EjeTrazado.sentidoCurva getSentido()
        {
            return mSentCurva;
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
            if (le_r>0)
            {
                if (this.mTipo== EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.salida)
                {
                    return (getComponentPoints(0,le_r));
                }
               return (getComponentPoints(le_r));
            }
            else
            {
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
            

        }
        public override List<double[]> getComponentPoints(double pk)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((getPkIni + i) < getPkFinal())
            {
                if (getPkIni + i >= pk)
                {
                    puntos.Add(getPointAtDist(getPkIni + i));
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
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints(double pk,double pk_fin)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((getPkIni + i) < pk_fin)
            {
                if (getPkIni + i >= pk)
                {
                    puntos.Add(getPointAtDist(getPkIni + i));
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
        public override List<double[]> getComponentPoints_p()
        {
            var puntos = new List<double[]>();
            double i = 0;
            if (le_r > 0)
            {
                if (this.mTipo == EjeDeTrazado.puntosDelEje.EjeTrazado.tipoClotoide.salida)
                {
                    return (getComponentPoints_p(0, le_r));
                }
                return (getComponentPoints_p(le_r));
            }
            else
            {
                while ((getPkIni + i) < getPkFinal())
                {
                    puntos.Add(getPointAtDist(getPkIni + i));
                    i += 0.01;

                }
                puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
                return puntos;
            }


        }
        public override List<double[]> getComponentPoints_p(double pk)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((getPkIni + i) < getPkFinal())
            {
                if (getPkIni + i >= pk)
                {
                    puntos.Add(getPointAtDist(getPkIni + i));
                }

                i += 0.01;

            }
            puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public override List<double[]> getComponentPoints_p(double pk, double pk_fin)
        {
            var puntos = new List<double[]>();
            double i = 0;

            while ((getPkIni + i) < pk_fin)
            {
                if (getPkIni + i >= pk)
                {
                    puntos.Add(getPointAtDist(getPkIni + i));
                }

                i += 0.01;

            }
            puntos.Add(getPointAtDist(pk_fin));
            //base.setPuntoSalida=new Punto3d(puntos[puntos.Count-1][0], puntos[puntos.Count - 1][1],0);
            //puntos.Add(new double[] { getPuntoSalida.coordenadaX, getPuntoSalida.coordenadaY });
            return puntos;

        }
        public double Azimut { get { return mAzimut; } }
       
        //public double getAzimut(double iDx, double iDy)
        //{

        //    double miDelta;
        //    if ((iDx == 0) || (iDy == 0))
        //    {
        //        miDelta = 0;
        //    }
        //    else
        //    {
        //        miDelta = Math.Atan(iDx / iDy);
        //    }
        //    double miDeltaGra = miDelta * 180 / Math.PI;
        //    double miAzimut;

        //    if (miDeltaGra == 0)
        //    {
        //        if (iDy == 0)
        //        {
        //            if (iDx < 0)
        //            {
        //                miAzimut = 180;
        //            }
        //            else
        //            {
        //                miAzimut = 0;
        //            }
        //        }
        //        else
        //        {
        //            if (iDx < 0)
        //            {
        //                miAzimut = 270;
        //            }
        //            else
        //            {
        //                miAzimut = 90;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (miDeltaGra < 0)
        //        {
        //            if (iDx >= 0)
        //            {
        //                miAzimut = 90 - miDeltaGra;
        //            }
        //            else
        //            {
        //                miAzimut = 270 - miDeltaGra;
        //            }
        //        }
        //        else
        //        {
        //            if (iDy >= 0)
        //            {
        //                miAzimut = 90 - miDeltaGra;
        //            }
        //            else
        //            {
        //                miAzimut = 270 - miDeltaGra;
        //            }
        //        }
        //    }
        //    return miAzimut;
        //}


    }
}