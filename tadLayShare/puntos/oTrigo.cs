using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayShare.puntos
{

    public enum eAng
    {
        grados,
        radianes,
    }
    public enum ePorcentaje
    {
        porCiento,
        porUno,

    }
    public static class oTrigo
    {




        public static double getRadianesFromGrados(double iGrados)
        {
            return (iGrados / 180) * Math.PI;
        }
        public static double getGradosFromRadianes(double iRadianes)
        {
            return (iRadianes * 180) / Math.PI;
        }


        /// <summary>
        /// Angulo entre 3 Puntos
        /// </summary>
        /// <param name="iPtoVerticeComun">Punto Centro</param>
        /// <param name="iP1">Punto 1</param>
        /// <param name="iP2">Punto 2</param>
        /// <param name="iAngFormato">Formato Angulo Devuelto</param>
        /// <returns>Angulo Menor 180º</returns>
        public static double getAngFrom3Point(IP2d  iPVerticeComun,IP2d  iP1,  IP2d  iP2,eAng  iAngFormato)                                                                                                                                  
        {

            double ma = (iP2.X - iPVerticeComun.X) * (iP1.X - iPVerticeComun.X);

            double mb = (iP2.Y - iPVerticeComun.Y) * (iP1.Y - iPVerticeComun.Y);

            double mc = Math.Sqrt(Math.Pow((iP1.X - iPVerticeComun.X), 2) + Math.Pow((iP1.Y - iPVerticeComun.Y), 2));

            double md = Math.Sqrt(Math.Pow((iP2.X - iPVerticeComun.X), 2) + Math.Pow((iP2.Y - iPVerticeComun.Y), 2));

            double mResul = Math.Round((ma + mb) / (mc * md), 5);

            if (mResul < -1 | mResul > 1)
            {
                throw new Exception("Error; El valor debe de estar comprendido entre 1 y -1");
            }

                //Angulo Radianes
                double mAngRad = Math.Acos(mResul);

                if (double.IsNaN(mAngRad))
                {
                    throw new Exception("Error; El valor es Infinito");
                }
                else
                {
                    if (iAngFormato == eAng.grados)
                    {
                        return getGradosFromRadianes(mAngRad);
                    }
                    else
                    {
                        return mAngRad;
                    }
                }
           }
  
        
        public static double getAngFrom3Point(double iVerticeComunX, double iVerticeComunY, 
                                              double iVerticePrevioX, double iVerticePrevioY,
                                              double iVerticeNextX, double iVerticeNextY, eAng iAngFormato)
        {



            oP2d miVerticeComun = new oP2d(iVerticeComunX, iVerticeComunY);
            oP2d miVerticePrevio = new oP2d(iVerticePrevioX, iVerticePrevioY);
            oP2d miVerticeNext = new oP2d(iVerticeNextX, iVerticeNextY);

            return getAngFrom3Point(miVerticeComun, miVerticePrevio, miVerticeNext, iAngFormato);
            
        }


        /// <summary>
        /// Obtener el Angulo de 2 Tramos, Conociendo Vertice Comun y sus Azimut
        /// </summary>
        /// <param name="iP0">Coordenadas del Vertice Comun</param>
        /// <param name="iAzimutGradosTramoPrevio">AzimutGrados</param>
        /// <param name="iAzimutGradosTramoSiguiente">AzimutGrados</param>
        /// <param name="iAngulo">Formato Angulo</param>
        /// <returns>Angulo entre los tramos</returns>
        public static double getAngFromAzimut(IP2d iPtoVerticeComun, double iAzimutGradosTramoPrevio, double iAzimutGradosTramoSiguiente, eAng iAngulo)
        {

                //Calculamos los Puntos P1 y P2, en función de los Azimut
            IP2d myP1dPrevio = getP2FromAzimutLon(iPtoVerticeComun, 180 + iAzimutGradosTramoPrevio, 10);

            IP2d myP2dNext = getP2FromAzimutLon(iPtoVerticeComun, iAzimutGradosTramoSiguiente, 10);

                if (iAngulo == eAng.grados)
                {
                    return getAngFrom3Point(iPtoVerticeComun, myP1dPrevio, myP2dNext, eAng.grados);
                }
                else
                {
                    return getAngFrom3Point(iPtoVerticeComun, myP1dPrevio, myP2dNext, eAng.radianes);
                }
           
        }


        public static double geAngFromTwoPointsAndAzimut (IP2d iPtoPrevio, IP2d iPtoVerticeComun, double iAzimutGradosTramosSiguiente, eAng iAnguloFormato)
        {

            //Obtengo el azimut del tramo2
            IP2d miPtoNext = getP2FromAzimutLon(iPtoVerticeComun, iAzimutGradosTramosSiguiente, 10);

            //Obtengo el Angulo respecto a 3 Puntos
            return getAngFrom3Point(iPtoVerticeComun,iPtoPrevio, miPtoNext, iAnguloFormato);

        }



        public static double azimutGradosFormatTo360 (double iAzimutGrados)
        {

            int miEntero = (int)Math.Truncate(iAzimutGrados / 360);

            double miAzimutGr360 = iAzimutGrados - (miEntero * 360);


            if (miAzimutGr360 < 0)
            {
              return   miAzimutGr360 = 360 + miAzimutGr360;
            }
            else
            {
              return  miAzimutGr360;

            }

        }



        // GET P2, FROM Azimut + Lon
        public static oP2d getP2FromAzimutLon(IP2d iP1, double iAzimutGrados, double iLonFromOrigen)
        {

            double myAngRad;
            double myIncX;
            double myIncY;

            int myEntero = (int)Math.Truncate(iAzimutGrados / 360);

            double myAzimutGr360 = iAzimutGrados - (myEntero * 360);


            if (myAzimutGr360 < 0)
            {
                myAzimutGr360 = 360 + myAzimutGr360;
            }



            if (myAzimutGr360 >= 0 && myAzimutGr360 < 90)
            {

                myAngRad = getRadianesFromGrados(myAzimutGr360);
                myIncX = Math.Sin(myAngRad) * iLonFromOrigen;
                myIncY = Math.Cos(myAngRad) * iLonFromOrigen;

                return new oP2d(iP1.X + myIncX, iP1.Y + myIncY);
            }
            else if (myAzimutGr360 >= 90 && myAzimutGr360 < 180)
            {

                myAngRad = getRadianesFromGrados(180 - myAzimutGr360);
                myIncX = Math.Sin(myAngRad) * iLonFromOrigen;
                myIncY = Math.Cos(myAngRad) * iLonFromOrigen;

                return new oP2d(iP1.X + myIncX, iP1.Y - myIncY);

            }
            else if (myAzimutGr360 >= 180 && myAzimutGr360 < 270)
            {

                myAngRad = getRadianesFromGrados(myAzimutGr360 - 180);
                myIncX = Math.Sin(myAngRad) * iLonFromOrigen;
                myIncY = Math.Cos(myAngRad) * iLonFromOrigen;

                return new oP2d(iP1.X - myIncX, iP1.Y - myIncY);

            }
            else if (myAzimutGr360 >= 270 && myAzimutGr360 <= 360)
            {

                myAngRad = getRadianesFromGrados(360 - myAzimutGr360);
                myIncX = Math.Sin(myAngRad) * iLonFromOrigen;
                myIncY = Math.Cos(myAngRad) * iLonFromOrigen;

                return new oP2d(iP1.X - myIncX, iP1.Y + myIncY);
            }
            else
            {

                throw new Exception("Error al obtener las Coordenadas" + myAzimutGr360.ToString());

            }

        }

        // AZIMUT SEGMENTO
        public static double getAzimutGrados(IP2d iP1, IP2d iP2)
        {
    
                double myIncX = (iP2.X - iP1.X);
                double myIncY = (iP2.Y - iP1.Y);

                double myAngGradosAbs = Math.Abs(getGradosFromRadianes(Math.Atan(myIncX / myIncY)));


                if (myIncX == 0 & myIncY > 0)
                {
                    return 0;
                }
                else if (myIncX == 0 & myIncY < 0)
                {
                    return 180;
                }
                else if (myIncY == 0 & myIncX > 0)
                {
                    return 90;
                }
                else if (myIncY == 0 & myIncX < 0)
                {
                    return 270;
                }
                else if (myIncX > 0 & myIncY > 0)
                {
                    return myAngGradosAbs;
                }
                else if (myIncX > 0 & myIncY < 0)
                {
                    return 180 - myAngGradosAbs;
                }
                else if (myIncX < 0 & myIncY < 0)
                {
                    return 180 + myAngGradosAbs;
                }
                else if (myIncX < 0 & myIncY > 0)
                {
                    return 360 - myAngGradosAbs;

                }
                else
                {
                    return 0;
                }

            }
         
        


        /// <summary>
        /// Pendiente P1z-P2z (CON SIGNO) +Sube P2 ; -Baja P2
        /// </summary>
        public static double getPendiente3D(IP3d iP1, IP3d iP2, ePorcentaje iPorcenta)
        {

                double myDifCotaZ = (iP2.Z - iP1.Z);

                double myDistXy = iP1.distTo2d(iP2);

                         
                if (iPorcenta == ePorcentaje.porCiento)
                {
                    return  Math.Round( 100 * (myDifCotaZ / myDistXy),2);
                }
                else if (iPorcenta == ePorcentaje.porUno)
                {
                    return  Math.Round((myDifCotaZ / myDistXy),6);
                }
                else
                {
                    throw new Exception(string.Format("Porcentaje {0} No Definido", iPorcenta.ToString()));
                }
             
        }

        /// <summary>
        /// Pendiente P1y-P2y (CON SIGNO) +Sube P2 ; -Baja P2
        /// </summary>
        public static double getPendiente2D(IP2d iP1, IP2d iP2, ePorcentaje iPorcenta)
        {

                double myDifCotaY = (iP2.Y - iP1.Y);

                double myDistXy = iP1.distTo2d(iP2);

                if (iPorcenta == ePorcentaje.porCiento)
                {
                    return Math.Round(100 * (myDifCotaY / myDistXy), 2);
                }
                else if (iPorcenta == ePorcentaje.porUno)
                {
                    return Math.Round((myDifCotaY / myDistXy), 6);
                }
                else
                {
                    throw new Exception(string.Format("Porcentaje {0} No Definido", iPorcenta.ToString()));
                }
             
            }
        


        // Z Punto Medio Segmento
        public static double getPtoMedioZ(double iP1Z, double iP2Z)
        {
            return (iP1Z + iP2Z) / 2;  
        }

        /// <summary>
        /// Punto Medio Z
        /// </summary>
        public static double getPtoMedioZ(double iP1z, double iP1P2Lon, double iP1P2Pendiente)
        {
            return (iP1z + ((iP1P2Lon * iP1P2Pendiente / 100) / 2));
        }


        public static double getP2zFromP1andLon(double iP1Z, double iPendientePorCiento, double iLonFromP1)
        {

          return iP1Z + ((iPendientePorCiento / 100) * iLonFromP1);
           
        }








    }

}
