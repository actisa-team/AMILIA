using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

    using tadLayShare;
    using tadLayShare.puntoOld;
   

    //OK
        public class oToolTadilFunciones
        {



            #region "VALORACIONES TRAMOS"

            /// <summary>
            /// VALORACION TOTAL DEL TRAMO
            /// </summary>
            public static double xgetValoracionTotal(double iValorDist, double iValorCoste, double iValorSlope,
                                        double iAbaDisPorCiento, double iAbaCostePorCiento, double iAbaSlopePorCiento)
            {


                double myPesoDis = iValorDist * (iAbaDisPorCiento / 100);
                double myPesoCoste = iValorCoste * (iAbaCostePorCiento / 100);
                double myPesoSlope = iValorSlope * (iAbaSlopePorCiento / 100);


                double myPesoTotal = Math.Round(myPesoDis + myPesoCoste + myPesoSlope, 2);



                if (myPesoTotal > 10 | myPesoTotal < 0)
                {
                    throw new Exception("Error en la Valoración del Punto: " + myPesoTotal);
                }
                else
                {
                    return myPesoTotal;
                }

            }

            /// <summary>
            /// Obtener Valoración en función de Valores Max y Min
            /// </summary>
            public static double getValoracion(double iValorAbs, double iValorMinAbs, double iValorMaxAbs)
            {

                double myValor1 = (10 / (iValorMinAbs - iValorMaxAbs));

                double myValor2 = Math.Round((iValorAbs * myValor1) - (iValorMaxAbs * myValor1), 2);


                if (myValor2 > 10.00 | myValor2 < 0)
                {
                    throw new Exception("Error en la Valoración del Punto");
                }
                else
                {
                    return myValor2;
                }

            }



            /// <summary>
            /// Obtener la Valoración Slope Media de una Sección
            /// </summary>
            /// <param name="iTrSeccion"></param>
            /// <returns></returns>
            //public static double getSlopeMedioTramo(List<oTramoSeccion> iTrSeccion)
            //{

            //    List<double> myListSlopePeso = new List<double>();

            //    foreach (oTramoSeccion mySec in iTrSeccion)
            //    {
            //        myListSlopePeso.Add(getValorFromSlope(mySec.pSlope));
            //    }


            //    //Obtengo la Media
            //    double mySlopeTotal = (from p in myListSlopePeso select p).Sum();


            //    return (mySlopeTotal / myListSlopePeso.Count);


            //}

            /// <summary>
            /// Obtener la Valoración por la Pendiente
            /// p0=10,p0.15=6.5,p0.3=3.5,p0.4=1,p0.6=0, p>0.6 =0
            /// </summary>
            public static double xgetValorFromSlope(double iSlope)
            {

                if (iSlope >= 0 && iSlope < 0.15)
                {
                    return 10 - (23.333 * iSlope);
                }
                else if (iSlope >= 0.15 && iSlope < 0.3)
                {
                    return 6.5 - ((iSlope - 0.15) * 20);
                }
                else if (iSlope >= 0.3 && iSlope < 0.4)
                {
                    return 3.5 - ((iSlope - 0.30) * 25);
                }
                else if (iSlope >= 0.4 && iSlope < 0.6)
                {
                    return 1 - ((iSlope - 0.40) * 5);
                }
                else if (iSlope >= 0.6)
                {
                    return 0;
                }
                else
                {
                    throw new Exception(string.Format("El Valor de la Pendiente {0} No es Válido", iSlope));
                }

            }



            #endregion












            /// <summary>
            /// DETERMINAR SI LA LON TRAMOS ES VALIDA
            /// SOLO PARA ABANICOS PRIMARIOS
            /// </summary>
            public static bool getTramoAbanicoPrimarioIsOK(int iPorcentajeDesviacion, double iTrCurrentLon, double iTrPrevioLon)
            {


                double myDif = Math.Abs(iTrPrevioLon - iTrCurrentLon);
                double myPorcentaje = ((double)iPorcentajeDesviacion) / 100;
                double myIncLonMax = myPorcentaje * iTrPrevioLon;

                if (myIncLonMax> myDif)
                {
                    return true;
                }
                else
                {
                    return false;                
                }
            
            
            }





            public static oP2d getP2XyFromAbanico(
                               double iP1X,
                               double iP1Y,
                               double iTramoAziGrados,
                               double? iAngGradosTrPrevioActual,
                               bool iIsAbanicoRotonda
                              )
            {

                double myTramoLon;
                
                if (iIsAbanicoRotonda)
                {

                   // myTramoLon = oTadil.data.Proyecto.roadDesign.AijMin;
                }
                else
                {
                    //Obtengo la Longitud Mínima del Tramo.

                  //  myTramoLon = oTadil.data.Proyecto.roadDesign.getAij(oToolTrigo.getRadianesFromGrados(iAngGradosTrPrevioActual.Value));

                    
                }
                


                //Obtengo las Coordenadas del P2
              //  oP2d myP2 = oToolTrigo.getP2FromLonAzimut(iP1X, iP1Y, iTramoAziGrados, myTramoLon);

                throw new Exception();
            }




        }


       
    }

