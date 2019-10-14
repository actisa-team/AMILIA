using System;
using tadLayShare.puntos;

namespace tadLayShare
{
    public class Triangulo 
    {
        #region "Variables privadas"
            private Punto3d mVerticeA;
            private Punto3d mVerticeB;
            private Punto3d mVerticeC;
        private int mIndex;
        #endregion
        #region "Constructores"
            public Triangulo(Punto3d iVerticeA, Punto3d iVerticeB, Punto3d iVerticeC, int index)
            {
                mIndex = index;
                if (!this.alineados(iVerticeA, iVerticeB, iVerticeC))
                {
                    if ((iVerticeA.CompareTo(iVerticeB) != 0) && (iVerticeB.CompareTo(iVerticeC) != 0) && (iVerticeA.CompareTo(iVerticeC) != 0))
                    {
                        //TODO ordenar los vertices
                        if (iVerticeA.CompareTo(iVerticeB) == 1)
                        {
                            if (iVerticeA.CompareTo(iVerticeC) == 1)
                            {
                                mVerticeA = iVerticeA;
                                if (iVerticeB.CompareTo(iVerticeC) == 1)
                                {
                                    mVerticeB = iVerticeB;
                                    mVerticeC = iVerticeC;
                                }
                                else
                                {
                                    mVerticeB = iVerticeC;
                                    mVerticeC = iVerticeB;
                                }
                            }
                            else
                            {
                                mVerticeA = iVerticeC;
                                mVerticeB = iVerticeA;
                                mVerticeC = iVerticeB;

                            }

                        }
                        else
                        {
                            if (iVerticeA.CompareTo(iVerticeC) == 1)
                            {

                                mVerticeA = iVerticeB;
                                mVerticeB = iVerticeA;
                                mVerticeC = iVerticeC;
                            }
                            else
                            {
                                mVerticeC = iVerticeA;
                                if (iVerticeB.CompareTo(iVerticeC) == 1)
                                {
                                    mVerticeA = iVerticeB;
                                    mVerticeB = iVerticeC;
                                }
                                else
                                {
                                    mVerticeA = iVerticeC;
                                    mVerticeB = iVerticeB;
                                }
                            }
                        }

                    }
                }

            }

            //public Triangulo(Punto3d iLimiteInfIqz, Punto3d iLimiteInfDer, Punto3d iLimiteSupIzq, Punto3d iLimiteSupDer)
            //{
            //    Punto3d miPuntoTriangulo1, miPuntoTriangulo2, miPuntoPendiente1, miPuntoPendiente2;
            //    double distancia = iLimiteInfDer.distancia2d(iLimiteInfIqz);
            //    miPuntoTriangulo1 = new Punto3d((iLimiteInfDer.coordenadaX + (distancia)), iLimiteInfDer.coordenadaY - (distancia/2), 0);
            //    miPuntoTriangulo2 = new Punto3d((iLimiteInfIqz.coordenadaX - (distancia)), iLimiteInfIqz.coordenadaY - (distancia/2), 0);

            //    miPuntoPendiente1 = new Punto3d((iLimiteSupDer.coordenadaX), iLimiteSupDer.coordenadaY + (distancia / 2), 0);
            //    miPuntoPendiente2 = new Punto3d((iLimiteSupIzq.coordenadaX), iLimiteSupIzq.coordenadaY + (distancia / 2), 0);

            //    double miPendiente1 = getPendiente(miPuntoTriangulo1, miPuntoPendiente1);
            //    double miPendiente2 = getPendiente(miPuntoTriangulo2, miPuntoPendiente2);

            //    double miPuntoTriangulo3X = ((miPendiente2 * miPuntoTriangulo2.coordenadaX) - (miPendiente1 * miPuntoTriangulo1.coordenadaX) - miPuntoTriangulo2.coordenadaY + miPuntoTriangulo1.coordenadaY) / (miPendiente2 - miPendiente1);
            //    double miPuntoTriangulo3Y = (miPendiente2 * miPuntoTriangulo3X) - (miPendiente2 * miPuntoTriangulo2.coordenadaX) + miPuntoTriangulo2.coordenadaY;
            //    Punto3d miPuntoTriangulo3 = new Punto3d(miPuntoTriangulo3X, miPuntoTriangulo3Y, 0);

            //    Punto3d iVerticeA = miPuntoTriangulo1;
            //   Punto3d iVerticeB = miPuntoTriangulo2;
            //    Punto3d iVerticeC = miPuntoTriangulo3;


            //    //TODO ver que los 3 puntos no estan alinados
            //    if ((iVerticeA.CompareTo(iVerticeB) != 0) && (iVerticeB.CompareTo(iVerticeC) != 0) && (iVerticeA.CompareTo(iVerticeC) != 0))
            //    {
            //        //TODO ordenar los vertices
            //        if (iVerticeA.CompareTo(iVerticeB) == 1)
            //        {
            //            if (iVerticeA.CompareTo(iVerticeC) == 1)
            //            {
            //                mVerticeA = iVerticeA;
            //                if (iVerticeB.CompareTo(iVerticeC) == 1)
            //                {
            //                    mVerticeB = iVerticeB;
            //                    mVerticeC = iVerticeC;
            //                }
            //                else
            //                {
            //                    mVerticeB = iVerticeC;
            //                    mVerticeC = iVerticeB;
            //                }
            //            }
            //            else
            //            {
            //                mVerticeA = iVerticeC;
            //                mVerticeB = iVerticeA;
            //                mVerticeC = iVerticeB;

            //            }

            //        }
            //        else
            //        {
            //            if (iVerticeA.CompareTo(iVerticeC) == 1)
            //            {

            //                mVerticeA = iVerticeB;
            //                mVerticeB = iVerticeA;
            //                mVerticeC = iVerticeC;
            //            }
            //            else
            //            {
            //                mVerticeC = iVerticeA;
            //                if (iVerticeB.CompareTo(iVerticeC) == 1)
            //                {
            //                    mVerticeA = iVerticeB;
            //                    mVerticeB = iVerticeC;
            //                }
            //                else
            //                {
            //                    mVerticeA = iVerticeC;
            //                    mVerticeB = iVerticeB;
            //                }
            //            }
            //        }

            //    }
            //    else
            //    {
            //        //Lanzar error geometrico! 
            //    }
                

            //}
        #endregion

        #region "Metodos publicos"

            public Punto3d getVerticeA
            {
                get{ return this.mVerticeA;}
            }

            public Punto3d getVerticeB
            {
                get { return this.mVerticeB; }
            }

            public Punto3d getVerticeC
            {
                get { return this.mVerticeC; }
            }


            public int getIndex
            {
                get { return mIndex; }
            }




            public Punto3d getVertice(Punto3d iVerticeA, Punto3d iVerticeB)
            {
               Punto3d miVertice = null;
               if (this.isTrianguloValido())
               {
                   if ((this.getVerticeA.getHashCode.Equals(iVerticeA.getHashCode)) || (this.getVerticeA.getHashCode.Equals(iVerticeB.getHashCode)))
                   {
                       if ((this.getVerticeA.getHashCode.Equals(iVerticeA.getHashCode)))
                       {
                           if ((this.getVerticeB.getHashCode.Equals(iVerticeB.getHashCode)))
                           {
                               miVertice = this.getVerticeC;
                           }
                           else
                           {
                               if ((this.getVerticeC.getHashCode.Equals(iVerticeB.getHashCode)))
                               {
                                   miVertice = this.getVerticeB;
                               }
                           }
                       }
                       else
                       {
                           if ((this.getVerticeB.getHashCode.Equals(iVerticeA.getHashCode)))
                           {
                               miVertice = this.getVerticeC;
                           }
                           else
                           {
                               if ((this.getVerticeC.getHashCode.Equals(iVerticeA.getHashCode)))
                               {
                                   miVertice = this.getVerticeB;
                               }
                           }

                       }
                   }
                   else
                   {
                       if ((this.getVerticeB.getHashCode.Equals(iVerticeA.getHashCode)) || (this.getVerticeB.getHashCode.Equals(iVerticeB.getHashCode)))
                       {
                           if ((this.getVerticeB.getHashCode.Equals(iVerticeA.getHashCode)))
                           {
                               if ((this.getVerticeC.getHashCode.Equals(iVerticeB.getHashCode)))
                               {
                                   miVertice = this.getVerticeA;
                               }
                           }
                           else
                           {
                               if ((this.getVerticeC.getHashCode.Equals(iVerticeA.getHashCode)))
                               {
                                   miVertice = this.getVerticeA;
                               }
                           }
                       }
                   }
               }

                   return miVertice;
               
            }

            public int getIndice(Punto3d iPunto)
            {
                int miIndice = -1;
                if (this.isTrianguloValido())
                {
                    if (this.getVerticeA.getHashCode.Equals(iPunto.getHashCode))
                    {
                        miIndice = 0;
                    }
                    if (this.getVerticeB.getHashCode.Equals(iPunto.getHashCode))
                    {
                        miIndice = 1;
                    }
                    if (this.getVerticeC.getHashCode.Equals(iPunto.getHashCode))
                    {
                        miIndice = 2;
                    }
                }
            
                return miIndice;

            }

            public bool isDentro(Punto3d iPunto)
            {
                bool miIsDentro = false;
                if (this.isTrianguloValido())
                {
                    if (orientacionPositiva(mVerticeA, mVerticeB, mVerticeC))
                    {
                        miIsDentro = orientacionPositiva(mVerticeA, mVerticeB, iPunto) && orientacionPositiva(mVerticeB, mVerticeC, iPunto) && orientacionPositiva(mVerticeC, mVerticeA, iPunto);
                    }
                    else
                    {
                        miIsDentro = (!orientacionPositiva(mVerticeA, mVerticeB, iPunto) || orientacionNula(mVerticeA, mVerticeB, iPunto)) && (!orientacionPositiva(mVerticeB, mVerticeC, iPunto) || orientacionNula(mVerticeB, mVerticeC, iPunto)) && (!orientacionPositiva(mVerticeC, mVerticeA, iPunto) || orientacionNula(mVerticeC, mVerticeA, iPunto));
                    }
                }
            
                return miIsDentro;
            }

            public Punto3d[] isEnLado(Punto3d iPunto)
            {
                Punto3d[] miLado = new Punto3d[2];

                if (this.isTrianguloValido())
                {

                    double miPendienteAB = getPendiente(this.getVerticeA, this.getVerticeB);
                    double miPendienteBC = getPendiente(this.getVerticeC, this.getVerticeB);
                    double miPendienteAC = getPendiente(this.getVerticeA, this.getVerticeC);

                    bool isDentroAB = (((iPunto.coordenadaY - this.getVerticeA.coordenadaY) == (miPendienteAB * (iPunto.coordenadaX - this.getVerticeA.coordenadaX))) || alineados(iPunto, this.getVerticeA, this.getVerticeB));
                    bool isDentroBC = (((iPunto.coordenadaY - this.getVerticeB.coordenadaY) == (miPendienteBC * (iPunto.coordenadaX - this.getVerticeB.coordenadaX))) || alineados(iPunto, this.getVerticeC, this.getVerticeB));
                    bool isDentroAC = (((iPunto.coordenadaY - this.getVerticeA.coordenadaY) == (miPendienteAC * (iPunto.coordenadaX - this.getVerticeA.coordenadaX))) || alineados(iPunto, this.getVerticeA, this.getVerticeC));


                    if (isDentroAB)
                    {
                        miLado[0] = this.getVerticeA;
                        miLado[1] = this.getVerticeB;
                    }
                    else
                    {
                        if (isDentroBC)
                        {
                            miLado[0] = this.getVerticeB;
                            miLado[1] = this.getVerticeC;
                        }
                        else if (isDentroAC)
                        {
                            miLado[0] = this.getVerticeA;
                            miLado[1] = this.getVerticeC;
                        }

                    }
                }

                return miLado;
            }

            private bool alineados(Punto3d iPunto1, Punto3d iPunto2, Punto3d iPunto3)
            {
                bool isAlineados = true;
                double a1 = ((iPunto3.coordenadaX - iPunto1.coordenadaX) / (iPunto3.coordenadaX - iPunto2.coordenadaX));
                double a2 = ((iPunto3.coordenadaY - iPunto1.coordenadaY) / (iPunto3.coordenadaY - iPunto2.coordenadaY));
                if (!(a1 == a2))
                {
                    isAlineados = false;
                }
                if ((iPunto1.Equals(iPunto2)) || (iPunto3.Equals(iPunto2)) || (iPunto1.Equals(iPunto3)))
                {
                    isAlineados = true;
                }
                if (((iPunto1.coordenadaX == iPunto2.coordenadaX) && (iPunto1.coordenadaX == iPunto3.coordenadaX)) || ((iPunto1.coordenadaY == iPunto2.coordenadaY) && (iPunto1.coordenadaY == iPunto3.coordenadaY)))
                {
                    isAlineados = true;
                }
                return isAlineados;
            }

            public bool isDentroCircunferencia(Punto3d iPunto)
            {
                bool miIsDentro = false;
                if (this.isTrianguloValido())
                {
                    Punto3d miCentro = this.getCentro;
                    Punto3d miPunto = iPunto;
                    double miDistancia = iPunto.distancia2d(miCentro);
                    double miRadio = miCentro.distancia2d(this.getVerticeA);


                    miIsDentro = miDistancia < miRadio;
                }
                
                return miIsDentro;
            }

            public Punto3d[] getLadoComun(Triangulo iTriangulo)
            {
                Punto3d[] miLado = new Punto3d[2];

                if (this.isTrianguloValido() && iTriangulo.isTrianguloValido())
                {
                    if ((this.getVerticeA.getHashCode.Equals(iTriangulo.getVerticeA.getHashCode)) || (this.getVerticeA.getHashCode.Equals(iTriangulo.getVerticeB.getHashCode)) || (this.getVerticeA.getHashCode.Equals(iTriangulo.getVerticeC.getHashCode)))
                    {
                        if ((this.getVerticeB.getHashCode.Equals(iTriangulo.getVerticeA.getHashCode)) || (this.getVerticeB.getHashCode.Equals(iTriangulo.getVerticeB.getHashCode)) || (this.getVerticeB.getHashCode.Equals(iTriangulo.getVerticeC.getHashCode)))
                        {
                            miLado[0] = this.getVerticeA;
                            miLado[1] = this.getVerticeB;
                        }
                        else
                        {
                            if ((this.getVerticeC.getHashCode.Equals(iTriangulo.getVerticeA.getHashCode)) || (this.getVerticeC.getHashCode.Equals(iTriangulo.getVerticeB.getHashCode)) || (this.getVerticeC.getHashCode.Equals(iTriangulo.getVerticeC.getHashCode)))
                            {
                                miLado[0] = this.getVerticeA;
                                miLado[1] = this.getVerticeC;
                            }
                        }

                    }
                    else
                    {

                        if ((this.getVerticeB.getHashCode.Equals(iTriangulo.getVerticeA.getHashCode)) || (this.getVerticeB.getHashCode.Equals(iTriangulo.getVerticeB.getHashCode)) || (this.getVerticeB.getHashCode.Equals(iTriangulo.getVerticeC.getHashCode)))
                        {
                            if ((this.getVerticeC.getHashCode.Equals(iTriangulo.getVerticeA.getHashCode)) || (this.getVerticeC.getHashCode.Equals(iTriangulo.getVerticeB.getHashCode)) || (this.getVerticeC.getHashCode.Equals(iTriangulo.getVerticeC.getHashCode)))
                            {

                                miLado[0] = this.getVerticeB;
                                miLado[1] = this.getVerticeC;
                            }
                        }
                    }
                }
                return miLado;

            }

            public Punto3d getCentro
            {
                get
                {
                    Punto3d miCentro = null;
                    if (this.isTrianguloValido())
                    {
                        double miPendiente1 = (-1) * (1 / getPendiente(this.getPunto(0), this.getPunto(1)));
                        Punto3d miPuntoM1 = getPuntoMedio(0, 1);
                        double miPendiente2 = (-1) * (1 / getPendiente(this.getPunto(0), this.getPunto(2)));
                        Punto3d miPuntoM2 = getPuntoMedio(0, 2);

                        double miCentroX = ((miPendiente2 * miPuntoM2.coordenadaX) - (miPendiente1 * miPuntoM1.coordenadaX) - miPuntoM2.coordenadaY + miPuntoM1.coordenadaY) / (miPendiente2 - miPendiente1);
                        double miCentroY = (miPendiente2 * miCentroX) - (miPendiente2 * miPuntoM2.coordenadaX) + miPuntoM2.coordenadaY;

                        miCentro = new Punto3d(miCentroX, miCentroY, 0);
                    }

                    return miCentro;

                }
            }

            
        #endregion

        #region "Metodos privados"

            private bool orientacionPositiva(Punto3d iVertice1, Punto3d iVertice2, Punto3d iVertice3)
            {
                double miOrientacion = ((iVertice1.coordenadaX - iVertice3.coordenadaX) * (iVertice2.coordenadaY - iVertice3.coordenadaY)) - ((iVertice1.coordenadaY - iVertice3.coordenadaY) * (iVertice2.coordenadaX - iVertice3.coordenadaX)); 
                return miOrientacion >= 0;
            }

            private bool orientacionNula(Punto3d iVertice1, Punto3d iVertice2, Punto3d iVertice3)
            {
                double miOrientacion = ((iVertice1.coordenadaX - iVertice3.coordenadaX) * (iVertice2.coordenadaY - iVertice3.coordenadaY)) - ((iVertice1.coordenadaY - iVertice3.coordenadaY) * (iVertice2.coordenadaX - iVertice3.coordenadaX));
                return miOrientacion == 0;
            }


            private double getPendiente(Punto3d iPuntoA, Punto3d iPuntoB)
            {
                double miPendiente = 0;

                miPendiente = (iPuntoA.coordenadaY - iPuntoB.coordenadaY) / (iPuntoA.coordenadaX - iPuntoB.coordenadaX);

                return miPendiente;


            }

            private Punto3d getPuntoMedio(int iIndiceA, int iIndiceB)
            {
                Punto3d miPuntoM=null;
                double miXPuntoM = 0;
                double miYPuntoM = 0;

                if ((iIndiceA != iIndiceB)&&(iIndiceA<3)&&(iIndiceB<3)&&this.isTrianguloValido())
                {
                    Punto3d miPuntoA = this.getPunto(iIndiceA);
                    Punto3d miPuntoB = this.getPunto(iIndiceB);

                    miXPuntoM = (miPuntoB.coordenadaX + miPuntoA.coordenadaX) / 2;
                    miYPuntoM = (miPuntoB.coordenadaY + miPuntoA.coordenadaY) / 2;

                    miPuntoM = new Punto3d(miXPuntoM, miYPuntoM, 0);
                }

                return miPuntoM;
            }

            private Punto3d getPunto(int iIndice)
            {
                Punto3d miPunto = null;
                if (iIndice == 0)
                {
                    miPunto = this.getVerticeA;
                }
                if (iIndice == 1)
                {
                    miPunto = this.getVerticeB;
                }
                if (iIndice == 2)
                {
                    miPunto = this.getVerticeC;
                }
                return miPunto;

            }

            

        #endregion

            public string getHashCode
            {
                get { return this.getVerticeA.getHashCode + "_" + this.getVerticeB.getHashCode + "_" + this.getVerticeC.getHashCode; }
            }

            public double getCota(double iX, double iY)
            {

                double miLambda, miMu, miCota;
                Punto3d puntoA = this.getVerticeA;
                Punto3d puntoB = this.getVerticeB;
                Punto3d puntoC = this.getVerticeC;

                Punto3d vectorU = new Punto3d(puntoB.coordenadaX - puntoA.coordenadaX, puntoB.coordenadaY - puntoA.coordenadaY, puntoB.coordenadaZ - puntoA.coordenadaZ);
                Punto3d vectorV = new Punto3d(puntoC.coordenadaX - puntoA.coordenadaX, puntoC.coordenadaY - puntoA.coordenadaY, puntoC.coordenadaZ - puntoA.coordenadaZ);

                miMu = puntoA.coordenadaY - ((puntoA.coordenadaX * vectorU.coordenadaY) / vectorU.coordenadaX) + ((iX * vectorU.coordenadaY) / vectorU.coordenadaX) - iY;
                miMu = miMu / (((vectorV.coordenadaX * vectorU.coordenadaY) / vectorU.coordenadaX) - vectorV.coordenadaY);
                miLambda = (puntoA.coordenadaX + miMu * vectorV.coordenadaX - iX) / vectorU.coordenadaX;

                miCota = puntoA.coordenadaZ - miLambda * vectorU.coordenadaZ + miMu * vectorV.coordenadaZ;

                // Es el caso en que las cotas de los 3 puntos tienen la misma cota, entonces se divide entre 0 
                if (!(miCota>=0)&&!(miCota<=0))
                {
                    miCota = puntoA.coordenadaZ;
                }

                    return miCota;
            }

            public double getPendienteMaxima
            {
                get{

                    double mySlope = double.PositiveInfinity;
                    if (this.isTrianguloValido())
                    {
                        double x1 = this.getVerticeA.coordenadaX;
                        double y1 = this.getVerticeA.coordenadaY;
                        double z1 = this.getVerticeA.coordenadaZ;

                        double x2 = this.getVerticeB.coordenadaX;
                        double y2 = this.getVerticeB.coordenadaY;
                        double z2 = this.getVerticeB.coordenadaZ;

                        double x3 = this.getVerticeC.coordenadaX;
                        double y3 = this.getVerticeC.coordenadaY;
                        double z3 = this.getVerticeC.coordenadaZ;



                        double normX = (y2 - y1) * (z3 - z1) - (y3 - y1) * (z2 - z1);
                        double normY = (z2 - z1) * (x3 - x1) - (z3 - z1) * (x2 - x1);
                        double normZ = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);

                        mySlope = Math.Pow((normX * normX + normY * normY), 0.5) / normZ;
                    }


                    return Math.Abs(mySlope);
                
                }
            }

            public bool isTrianguloValido()
            {
                bool isCorrecto = (mVerticeA!= null)&&(mVerticeB!=null)&&(mVerticeC!=null);
                return isCorrecto;
            }

        
    }
}
