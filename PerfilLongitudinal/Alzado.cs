using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EjeDeTrazado.puntosDelEje;
using EjeDeTrazado.componentes;
using tadLayShare.puntos;
using PerfilLongitudinal.componentes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Autodesk.AutoCAD.DatabaseServices;
using engCadNet;
using Autodesk.AutoCAD.Geometry;



namespace PerfilLongitudinal
{

    /// <summary>
    /// EJE ALZADO TADIL VERSION 3.0
    /// </summary>
    /// 

    [Serializable]
    public class Alzado
    {

        public enum tipoVertice { concavo, convexo };
        private List<double[]> mVerticesAlzado;
        private double[] mPendientes;
        private double mMaxZ;
        private double mMinZ;
        private tipoVertice[] mTipoVertices;
        private double[] mCambioPendientes;
        private List<double[]> mPuntosEje = new List<double[]>();
        private double mVelocidad;
        private double[] mCriterioVis;
        private double[] mKvSolape;
        private double[] mKvElegido;
        private List<List<double[]>> mAcuerdos;
        private double mIntSecciones = 0;
        private double mKvConcavo;
        private double mKvConvexo;
        private List<componentes.ComponenteLong> mEjeAlzado;
        //private Func<double?, double?, double?> mGetCota;
        
        public Alzado(Polyline iEjeTrazado, List<Punto3d> iVertices3d, Func<double?, double?, double?> miGetCota, bool isPendienteEntrada,
            bool isPendienteSalida, double iPendienteEntradaTantoX1, double iPendienteSalidaTantoX1, double iVelocidad, double iKvConcavo,
            double iKvConvexo, double iIntSec,
            Func<double[], double> MDT_Abanico_Punto)
        {
            //mGetCota = miGetCota;
            mIntSecciones = iIntSec;
            mKvConcavo = iKvConcavo;
            mKvConvexo = iKvConvexo;

            double maxZ = 0;
            double minZ = double.PositiveInfinity;

            for (int i = 0; i < iEjeTrazado.Length; i = i + 10)
            {
                AñadePuntoDelTerreno(iEjeTrazado, miGetCota, i, ref maxZ, ref minZ, MDT_Abanico_Punto);
            }
            AñadePuntoDelTerreno(iEjeTrazado, miGetCota, iEjeTrazado.Length, ref maxZ, ref minZ, MDT_Abanico_Punto);


            mVelocidad = iVelocidad;

            mVerticesAlzado = new List<double[]>();
            double[] miVer = new double[4];

            if (iVertices3d.ElementAt(0).coordenadaZ > maxZ)
            {
                maxZ = iVertices3d.ElementAt(0).coordenadaZ;
            }
            if (iVertices3d.ElementAt(0).coordenadaZ < minZ)
            {
                minZ = iVertices3d.ElementAt(0).coordenadaZ;
            }



            bool verDistinto = false;


            foreach (Punto3d miPunto3d in iVertices3d)
            {
                bool isInCurvaNoPaso = false;

                double[] miVerFin = new double[4];
                Point3d miPunto = oLw.getPointMasCercano(iEjeTrazado, new Point3d(miPunto3d.coordenadaX, miPunto3d.coordenadaY, miPunto3d.coordenadaZ));
                miVerFin[0] = miPunto.X;
                miVerFin[1] = miPunto.Y;
                miVerFin[2] = miPunto3d.coordenadaZ;

                miVerFin[3] = iEjeTrazado.GetDistAtPoint(miPunto);
                mVerticesAlzado.Add(miVerFin);
                if (maxZ < miVerFin[2]) maxZ = miVerFin[2];
                if (minZ > miVerFin[2]) minZ = miVerFin[2];
            }


            mPendientes = getPendientes();
            mPendientes = recalcularPendientes(isPendienteEntrada, isPendienteSalida, iPendienteEntradaTantoX1, iPendienteSalidaTantoX1);

            mTipoVertices = getTipoVertices();

            mCambioPendientes = getCambioPendientes();

            mCriterioVis = getCriterioVisivilidad();

            mKvSolape = getKvNoSolape();

            mKvElegido = getKvElegido();

            calcularAcuerdos();


            foreach (double[] miPunto in mPuntosEje)
            {
                if (miPunto[2] > maxZ)
                {
                    maxZ = miPunto[2];
                }
            }

            foreach (double[] miVertice in mVerticesAlzado)
            {
                if (miVertice[2] > maxZ)
                {
                    maxZ = miVertice[2];
                }
                if (miVertice[2] < minZ)
                {
                    minZ = miVertice[2];
                }
            }
            mMaxZ = maxZ;
            mMinZ = minZ;

            creaEjeLong();

        }

        private void AñadePuntoDelTerreno(Polyline iEjeTrazado, Func<double?, double?, double?> miGetCota, double i, ref double maxZ, ref double minZ,
            Func<double[], double> MDT_Abanico_Punto)
        {
            var miPunto = iEjeTrazado.GetPointAtDist(i);
            double[] miPuntoCota = new double[3];
            miPuntoCota[0] = miPunto.X;
            miPuntoCota[1] = miPunto.Y;
            /*
             * Se cambia para que se haga con KD-Tree
             */
            /*MDT_Abanico_Punto(miPuntoCota);
            double? miCota = miGetCota(miPunto.X, miPunto.Y);*/
            double? miCota = MDT_Abanico_Punto(miPuntoCota);
            if (miCota != null)
            {
                miPuntoCota[2] = (double)miCota;
                mPuntosEje.Add(miPuntoCota);

                if (miPuntoCota[2] < minZ)
                {
                    minZ = miPuntoCota[2];
                }
                if (miPuntoCota[2] > maxZ)
                {
                    maxZ = miPuntoCota[2];
                }
            }
            else
            {
                throw new Exception("Existen puntos fuera de la cartografia PERFIL LONGITUDINAL");
            }
        }

        private void creaEjeLong()
        {
            mEjeAlzado = new List<componentes.ComponenteLong>();
            double miPKAnt = 0;
            int i = 0;

            foreach (List<double[]> miAcuerdo in mAcuerdos)
            {
                Recta miRecta = new Recta(new Punto3d(miPKAnt, getCotaRasante(miPKAnt), 0), new Punto3d(miAcuerdo.ElementAt(0)[0], miAcuerdo.ElementAt(0)[1], 0), miPKAnt);
                Parabola miParabola;
                if (mTipoVertices[i] == tipoVertice.concavo)
                {
                    miParabola = new Parabola(new Punto3d(miAcuerdo.ElementAt(0)[0], miAcuerdo.ElementAt(0)[1], 0), new Punto3d(miAcuerdo.ElementAt(1)[0], miAcuerdo.ElementAt(1)[1], 0), miAcuerdo.ElementAt(0)[0], mPendientes[i], mKvElegido[i], mCambioPendientes[i], true);
                }
                else
                {
                    miParabola = new Parabola(new Punto3d(miAcuerdo.ElementAt(0)[0], miAcuerdo.ElementAt(0)[1], 0), new Punto3d(miAcuerdo.ElementAt(1)[0], miAcuerdo.ElementAt(1)[1], 0), miAcuerdo.ElementAt(0)[0], mPendientes[i], mKvElegido[i], mCambioPendientes[i], false);
                }
                mEjeAlzado.Add(miRecta);
                mEjeAlzado.Add(miParabola);
                miPKAnt = miAcuerdo.ElementAt(1)[0];
                i++;
            }

            Recta miRectaFin = new Recta(new Punto3d(miPKAnt, getCotaRasante(miPKAnt), 0), new Punto3d(getMaxPk, getCotaRasante(getMaxPk), 0), miPKAnt);
            mEjeAlzado.Add(miRectaFin);

        }

        private double[] getKvElegido()
        {

            double[] misKv = new double[mKvSolape.Count()];
            int i;
            for (i = 0; i < mKvSolape.Count(); i++)
            {
                if (mKvSolape[i] > getKv(mTipoVertices[i]))
                {
                    if (mCriterioVis[i] > getKv(mTipoVertices[i]))
                    {
                        misKv[i] = mCriterioVis[i];
                    }
                    else
                    {
                        misKv[i] = getKv(mTipoVertices[i]);
                    }
                }
                else
                {
                    misKv[i] = mKvSolape[i];
                }


            }

            return misKv;
        }

        private double[] getKvNoSolape()
        {
            double[] misKv = new double[mVerticesAlzado.Count - 2];

            int i;
            for (i = 0; i < mVerticesAlzado.Count - 2; i++)
            {
                Punto3d P1 = new Punto3d(mVerticesAlzado.ElementAt(i)[3], mVerticesAlzado.ElementAt(i)[2], 0);
                Punto3d P2 = new Punto3d(mVerticesAlzado.ElementAt(i + 1)[3], mVerticesAlzado.ElementAt(i + 1)[2], 0);
                Punto3d P3 = new Punto3d(mVerticesAlzado.ElementAt(i + 2)[3], mVerticesAlzado.ElementAt(i + 2)[2], 0);
                double L1 = P1.distancia2d(P2);
                double L2 = P2.distancia2d(P3);
                if (mCambioPendientes[i] != 0)
                {
                    misKv[i] = Math.Min(L1, L2) / mCambioPendientes[i];
                }
                else
                {
                    misKv[i] = 0;
                }
            }

            return misKv;
        }

        private double[] getCriterioVisivilidad()
        {
            double[] misKv = new double[mVerticesAlzado.Count() - 2];
            int i = 0;
            for (i = 0; i < mVerticesAlzado.Count() - 2; i++)
            {
                if (mCambioPendientes[i] != 0)
                {
                    misKv[i] = mVelocidad / mCambioPendientes[i];
                }
                else
                {
                    misKv[i] = 0;
                }
            }
            return misKv;
        }

        private tipoVertice[] getTipoVertices()
        {
            tipoVertice[] misTipos = new tipoVertice[mPendientes.Count() - 1];

            int i = 0;
            for (i = 0; i < mPendientes.Count() - 1; i++)
            {
                if (mPendientes[i] > mPendientes[i + 1])
                {
                    misTipos[i] = tipoVertice.convexo;
                }
                else
                {
                    misTipos[i] = tipoVertice.concavo;
                }
            }
            return misTipos;
        }

        private double[] getCambioPendientes()
        {
            double[] misCambios = new double[mPendientes.Count() - 1];
            int i = 0;


            for (i = 0; i < mPendientes.Count() - 1; i++)
            {
                double miCambio = Math.Abs(mPendientes[i + 1] - mPendientes[i]);
                misCambios[i] = Math.Abs(mPendientes[i + 1] - mPendientes[i]);
            }
            return misCambios;
        }

        public double[] getPendientes()
        {
            double[] misPendientes = new double[mVerticesAlzado.Count - 1];
            int i = 0;
            for (i = 0; i < mVerticesAlzado.Count - 1; i++)
            {
                if ((mVerticesAlzado.ElementAt(i + 1)[3] - mVerticesAlzado.ElementAt(i)[3]) == 0)
                {
                    misPendientes[i] = 0;
                }
                else
                {
                    misPendientes[i] = (mVerticesAlzado.ElementAt(i + 1)[2] - mVerticesAlzado.ElementAt(i)[2]) / (mVerticesAlzado.ElementAt(i + 1)[3] - mVerticesAlzado.ElementAt(i)[3]);
                }
            }
            return misPendientes;
        }

        private double[] recalcularPendientes(bool isPendienteEntrada, bool isPendienteSalida, double iPendienteEntradaTantoX1, double iPendienteSalidaTantoX1)
        {
            double[] misPendientes = mPendientes;

            if (isPendienteEntrada)
            {
                double z = ((mVerticesAlzado.ElementAt(1)[3] - mVerticesAlzado.ElementAt(0)[3]) * (iPendienteEntradaTantoX1)) + mVerticesAlzado.ElementAt(0)[2];
                mVerticesAlzado.ElementAt(1)[2] = z;
            }
            if (isPendienteSalida)
            {
                double z = ((mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[3] - mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 2)[3]) * (iPendienteSalidaTantoX1 * -1)) + mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[2];
                mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 2)[2] = z;
            }

            misPendientes = getPendientes();

            return misPendientes;
        }

        public List<double[]> getVerticesAlzado
        {
            get
            {
                return mVerticesAlzado;
            }
        }

        public double getMaxZ
        {
            get
            {
                return mMaxZ;
            }
        }

        public double getMinZ
        {
            get
            {
                return mMinZ;
            }
        }

        public double getMaxPk
        {
            get
            {
                return mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[3];
            }
        }

        public List<double[]> drawTerreno()
        {
            List<double[]> miLista = new List<double[]>();
            double pk = 0;
            foreach (double[] miPunto in mPuntosEje)
            {
                if (pk < this.getMaxPk)
                {
                    double[] miPuntoPk = new double[4];
                    miPuntoPk[0] = miPunto[0];
                    miPuntoPk[1] = miPunto[1];
                    miPuntoPk[2] = miPunto[2];
                    miPuntoPk[3] = pk;
                    pk = pk + 10;
                    miLista.Add(miPuntoPk);
                }
            }

            double[] miPuntoPkFinal = new double[4];
            miPuntoPkFinal[0] = mPuntosEje[mPuntosEje.Count - 1][0];
            miPuntoPkFinal[1] = mPuntosEje[mPuntosEje.Count - 1][1];
            miPuntoPkFinal[2] = mPuntosEje[mPuntosEje.Count - 1][2];
            miPuntoPkFinal[3] = this.getMaxPk;
            miLista.Add(miPuntoPkFinal);


            return miLista;
        }

        public double getAnguloEntre(Punto3d iPuntoA, Punto3d iPuntoB, double mCentroCurvaX, double mCentroCurvaY)
        {
            double miPendiente1, miPendiente2;
            double miAnguloEntrada, miAnguloSalida;

            if (mCentroCurvaX != iPuntoA.coordenadaX)
            {
                miPendiente1 = (iPuntoA.coordenadaY - mCentroCurvaY) / (iPuntoA.coordenadaX - mCentroCurvaX);
                if ((mCentroCurvaX < iPuntoA.coordenadaX) && (mCentroCurvaY < iPuntoA.coordenadaY))
                {
                    //cuadrante 1
                    miAnguloEntrada = Math.Atan(miPendiente1);
                }
                else if ((mCentroCurvaX > iPuntoA.coordenadaX) && (mCentroCurvaY < iPuntoA.coordenadaY))
                {
                    //cuadrante 2
                    miAnguloEntrada = Math.Atan(miPendiente1) + Math.PI;
                }
                else if ((mCentroCurvaX > iPuntoA.coordenadaX) && (mCentroCurvaY > iPuntoA.coordenadaY))
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
                if (mCentroCurvaY < iPuntoA.coordenadaY)
                {
                    miAnguloEntrada = Math.PI;
                }
                else
                {
                    miAnguloEntrada = 3 * Math.PI / 2;
                }
            }
            if (mCentroCurvaX != iPuntoB.coordenadaX)
            {
                miPendiente2 = (iPuntoB.coordenadaY - mCentroCurvaY) / (iPuntoB.coordenadaX - mCentroCurvaX);
                if ((mCentroCurvaX < iPuntoB.coordenadaX) && (mCentroCurvaY < iPuntoB.coordenadaY))
                {
                    //cuadrante 1
                    miAnguloSalida = Math.Atan(miPendiente2);
                }
                else if ((mCentroCurvaX > iPuntoB.coordenadaX) && (mCentroCurvaY < iPuntoB.coordenadaY))
                {
                    //cuadrante 2
                    miAnguloSalida = Math.Atan(miPendiente2) + Math.PI;
                }
                else if ((mCentroCurvaX > iPuntoB.coordenadaX) && (mCentroCurvaY > iPuntoB.coordenadaY))
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
                if (mCentroCurvaY < iPuntoA.coordenadaY)
                {
                    miAnguloSalida = Math.PI;
                }
                else
                {
                    miAnguloSalida = 3 * Math.PI / 2;
                }
            }

            if (miAnguloSalida - miAnguloEntrada < 0)
            {
                return miAnguloEntrada - miAnguloSalida;
            }
            else
            {
                return miAnguloSalida - miAnguloEntrada;
            }
        }

        private double getKv(tipoVertice iTipo)
        {
            double miKv = 0;

            if (iTipo == tipoVertice.concavo) miKv = mKvConcavo; else miKv = mKvConvexo;

            return miKv;
        }

        private void calcularAcuerdos()
        {
            mAcuerdos = new List<List<double[]>>();
            int i;
            double[] misCamboisPendientesSigno = getCambioPendientes();
            for (i = 0; i < mKvElegido.Count(); i++)
            {
                List<double[]> misPuntosES = new List<double[]>();
                double[] miPuntoE = new double[2];
                double[] miPuntoS = new double[2];
                double T = (mKvElegido[i] * misCamboisPendientesSigno[i]) / 2;
                miPuntoE[0] = mVerticesAlzado.ElementAt(i + 1)[3] - T;
                miPuntoS[0] = mVerticesAlzado.ElementAt(i + 1)[3] + T;
                miPuntoE[1] = ((miPuntoE[0] - mVerticesAlzado.ElementAt(i)[3]) / (mVerticesAlzado.ElementAt(i + 1)[3] - mVerticesAlzado.ElementAt(i)[3]) * (mVerticesAlzado.ElementAt(i + 1)[2] - mVerticesAlzado.ElementAt(i)[2])) + mVerticesAlzado.ElementAt(i)[2];
                miPuntoS[1] = ((miPuntoS[0] - mVerticesAlzado.ElementAt(i + 1)[3]) / (mVerticesAlzado.ElementAt(i + 2)[3] - mVerticesAlzado.ElementAt(i + 1)[3]) * (mVerticesAlzado.ElementAt(i + 2)[2] - mVerticesAlzado.ElementAt(i + 1)[2])) + mVerticesAlzado.ElementAt(i + 1)[2];
                misPuntosES.Add(miPuntoE);
                misPuntosES.Add(miPuntoS);
                mAcuerdos.Add(misPuntosES);
            }
        }

        public List<double[]> drawAcuerdos()
        {
            List<double[]> miEjeLong = new List<double[]>();
            double[] miPunto0 = new double[2];
            miPunto0[0] = mVerticesAlzado.ElementAt(0)[3];
            miPunto0[1] = mVerticesAlzado.ElementAt(0)[2];
            miEjeLong.Add(miPunto0);


            int i;
            for (i = 0; i < mAcuerdos.Count; i++)
            {
                double T = (mKvElegido[i] * mCambioPendientes[i]) / 2;
                double Xe = mAcuerdos.ElementAt(i).ElementAt(0)[0];
                double Ye = mAcuerdos.ElementAt(i).ElementAt(0)[1];
                miEjeLong.Add(mAcuerdos.ElementAt(i).ElementAt(0));
                double aux = 5;
                while (aux < 2 * T)
                {
                    double[] miPuntoA = new double[2];
                    miPuntoA[0] = Xe + aux;
                    if (mTipoVertices[i] == tipoVertice.concavo)
                    {
                        miPuntoA[1] = getCotaAcuerdo(Ye, Xe, mPendientes[i], mKvElegido[i], miPuntoA[0]);
                    }
                    else
                    {
                        miPuntoA[1] = getCotaAcuerdo(Ye, Xe, mPendientes[i], mKvElegido[i] * (-1), miPuntoA[0]);
                    }
                    miEjeLong.Add(miPuntoA);
                    aux = aux + 5;
                }
                miEjeLong.Add(mAcuerdos.ElementAt(i).ElementAt(1));
            }


            double[] miPuntoFIN = new double[2];
            miPuntoFIN[0] = mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[3];
            miPuntoFIN[1] = mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[2];
            miEjeLong.Add(miPuntoFIN);

            return miEjeLong;
        }

        private double getCotaAcuerdo(double iYe, double iXe, double iPendienteEntr, double iKv, double iX)
        {
            return (iYe + (iPendienteEntr * (iX - iXe)) + (Math.Pow(iX - iXe, 2) / (2 * iKv)));
        }

        public double getCotaRasante(double iPk)
        {
            double miCota;
            if (mVerticesAlzado.Count > 2)
            {
                int i = -1;
                double miPK = 0;
                if (iPk > mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[3]) iPk = mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[3];
                if (iPk == mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[3])
                {
                    miCota = mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[2];
                }
                else
                {

                    while (miPK <= iPk)
                    {
                        i++;
                        if (i >= 13)
                        {
                            int a = 0;
                        }
                        miPK = mVerticesAlzado.ElementAt(i)[3];

                    }
                    if (i == mVerticesAlzado.Count - 1)
                    {
                        if (iPk < mAcuerdos.ElementAt(i - 2).ElementAt(1)[0])
                        {
                            //esta en el acuerdo i-2
                            if (mTipoVertices[i - 2] == tipoVertice.concavo)
                            {
                                miCota = getCotaAcuerdo(mAcuerdos.ElementAt(i - 2).ElementAt(0)[1], mAcuerdos.ElementAt(i - 2).ElementAt(0)[0], mPendientes[i - 2], mKvElegido[i - 2], iPk);
                            }
                            else
                            {
                                miCota = getCotaAcuerdo(mAcuerdos.ElementAt(i - 2).ElementAt(0)[1], mAcuerdos.ElementAt(i - 2).ElementAt(0)[0], mPendientes[i - 2], mKvElegido[i - 2] * (-1), iPk);
                            }
                        }
                        else
                        {
                            //esta en la recta (i-1,i)
                            miCota = ((iPk - mVerticesAlzado.ElementAt(i - 1)[3]) / (mVerticesAlzado.ElementAt(i)[3] - mVerticesAlzado.ElementAt(i - 1)[3]) * (mVerticesAlzado.ElementAt(i)[2] - mVerticesAlzado.ElementAt(i - 1)[2])) + mVerticesAlzado.ElementAt(i - 1)[2];

                        }
                    }
                    else if (i == 0)
                    {
                        miCota = mVerticesAlzado.ElementAt(0)[2];
                    }
                    else if (i == 1)
                    {
                        if (iPk < mAcuerdos.ElementAt(0).ElementAt(0)[0])
                        {
                            //esta en la recta 0-1

                            miCota = ((iPk - mVerticesAlzado.ElementAt(0)[3]) / (mVerticesAlzado.ElementAt(1)[3] - mVerticesAlzado.ElementAt(0)[3]) * (mVerticesAlzado.ElementAt(1)[2] - mVerticesAlzado.ElementAt(0)[2])) + mVerticesAlzado.ElementAt(0)[2];

                        }
                        else if (iPk > mAcuerdos.ElementAt(0).ElementAt(0)[0])
                        {
                            //esta en el acuerdo 0

                            if (mTipoVertices[0] == tipoVertice.concavo)
                            {
                                miCota = getCotaAcuerdo(mAcuerdos.ElementAt(0).ElementAt(0)[1], mAcuerdos.ElementAt(0).ElementAt(0)[0], mPendientes[0], mKvElegido[0], iPk);
                            }
                            else
                            {
                                miCota = getCotaAcuerdo(mAcuerdos.ElementAt(0).ElementAt(0)[1], mAcuerdos.ElementAt(0).ElementAt(0)[0], mPendientes[0], mKvElegido[0] * (-1), iPk);
                            }
                        }
                        else
                        {
                            miCota = mAcuerdos.ElementAt(0).ElementAt(0)[1];
                        }
                    }
                    else
                    {
                        if (iPk < mAcuerdos.ElementAt(i - 1).ElementAt(0)[0])
                        {
                            if (iPk < mAcuerdos.ElementAt(i - 2).ElementAt(1)[0])
                            {
                                //esta en el acuerdo i-2
                                if (mTipoVertices[i - 2] == tipoVertice.concavo)
                                {
                                    miCota = getCotaAcuerdo(mAcuerdos.ElementAt(i - 2).ElementAt(0)[1], mAcuerdos.ElementAt(i - 2).ElementAt(0)[0], mPendientes[i - 2], mKvElegido[i - 2], iPk);
                                }
                                else
                                {
                                    miCota = getCotaAcuerdo(mAcuerdos.ElementAt(i - 2).ElementAt(0)[1], mAcuerdos.ElementAt(i - 2).ElementAt(0)[0], mPendientes[i - 2], mKvElegido[i - 2] * (-1), iPk);
                                }
                            }
                            else
                            {
                                //esta en la recta (i-1,i)
                                miCota = ((iPk - mVerticesAlzado.ElementAt(i - 1)[3]) / (mVerticesAlzado.ElementAt(i)[3] - mVerticesAlzado.ElementAt(i - 1)[3]) * (mVerticesAlzado.ElementAt(i)[2] - mVerticesAlzado.ElementAt(i - 1)[2])) + mVerticesAlzado.ElementAt(i - 1)[2];

                            }
                        }
                        else if (iPk > mAcuerdos.ElementAt(i - 1).ElementAt(0)[0])
                        {
                            //esta en el acuerdo i-1
                            if (mTipoVertices[i - 1] == tipoVertice.concavo)
                            {
                                miCota = getCotaAcuerdo(mAcuerdos.ElementAt(i - 1).ElementAt(0)[1], mAcuerdos.ElementAt(i - 1).ElementAt(0)[0], mPendientes[i - 1], mKvElegido[i - 1], iPk);
                            }
                            else
                            {
                                miCota = getCotaAcuerdo(mAcuerdos.ElementAt(i - 1).ElementAt(0)[1], mAcuerdos.ElementAt(i - 1).ElementAt(0)[0], mPendientes[i - 1], mKvElegido[i - 1] * (-1), iPk);
                            }
                        }
                        else
                        {
                            miCota = mAcuerdos.ElementAt(i - 1).ElementAt(0)[1];
                        }

                    }
                }
            }
            else
            {
                miCota = ((iPk - mVerticesAlzado.ElementAt(0)[3]) / (mVerticesAlzado.ElementAt(1)[3] - mVerticesAlzado.ElementAt(0)[3]) * (mVerticesAlzado.ElementAt(1)[2] - mVerticesAlzado.ElementAt(0)[2])) + mVerticesAlzado.ElementAt(0)[2];
            }
            return miCota;

        }

        public List<List<double[]>> getAcuerdos()
        {
            return mAcuerdos;
        }

        public double getIntervaloSecciones
        {
            get
            {
                return mIntSecciones;
            }
        }

        public double getKv(int pos)
        {
            return mKvElegido[pos];
        }


        public List<oInformeLong> escribirInforme()
        {
            List<oInformeLong> miInforme = new List<oInformeLong>();
            oInformeLong infVertice0 = new oInformeLong(null, null, null, 0, getCotaRasante(0), null, null, null, null, null);
            miInforme.Add(infVertice0);

            int i;
            for (i = 0; i < mAcuerdos.Count; i++)
            {
                oInformeLong infVerticeI = new oInformeLong(mPendientes[i], mAcuerdos.ElementAt(i).ElementAt(1)[0] - mAcuerdos.ElementAt(i).ElementAt(0)[0], mKvElegido[i], mVerticesAlzado.ElementAt(i + 1)[3], getCotaRasante(mVerticesAlzado.ElementAt(i + 1)[3]),
                    mAcuerdos.ElementAt(i).ElementAt(0)[0], mAcuerdos.ElementAt(i).ElementAt(0)[1], mAcuerdos.ElementAt(i).ElementAt(1)[0], mAcuerdos.ElementAt(i).ElementAt(1)[1], mPendientes[i + 1] - mPendientes[i]);

                miInforme.Add(infVerticeI);
            }


            oInformeLong infVerticefin = new oInformeLong(mPendientes[mPendientes.Count() - 1], null, null, mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[3], getCotaRasante(mVerticesAlzado.ElementAt(mVerticesAlzado.Count - 1)[3]), null, null, null, null, null);

            miInforme.Add(infVerticefin);


            return miInforme;

        }

        public List<ComponenteLong> ejeAlzado
        {
            get
            {
                return mEjeAlzado;
            }
        }


        public MemoryStream guardarAlzado()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, this);
            return stream;

        }


        public static Alzado recuperaAlzado(MemoryStream iStream)
        {
            ResolveEventHandler resolveHandler = (sender, args) =>
            {
                string shortName = args.Name.Split(',')[0];
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (assembly.GetName().Name == shortName)
                    {
                        return assembly;
                    }
                }
                return null;
            };

            AppDomain.CurrentDomain.AssemblyResolve += resolveHandler;
            try
            {
                BinaryFormatter formatterR = new BinaryFormatter();
                Alzado deserializada = (Alzado)formatterR.Deserialize(iStream);
                return deserializada;
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= resolveHandler;
            }
        }

        public void exportarPerfilLongitudinal(string iNombreArchivo)
        {
            MemoryStream iSerializado = guardarAlzado();
            string fileName = iNombreArchivo;// + ".tadper";
            FileStream stream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            byte[] miBuffer = iSerializado.GetBuffer();

            for (int i = 0; i < miBuffer.Length; i++)
            {
                stream.WriteByte(miBuffer[i]);
            }
            stream.Close();
        }


    }
}
