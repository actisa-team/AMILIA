using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using tadLayLogica.logica.EjeBasicoNew;
using tadLayLogica;
using engCadNet;
using System.Diagnostics;
using tadLayShare;
using System.IO;
using tayLogicaTijera.data;

namespace tayLogicaTijera.EjeBasico
{
    class oEntronqueFerrocarril
    {

        private oP2d mP1Tramo1;
        private oP2d mP2Tramo1;
        private oP2d mP1Tramo2;
        private oP2d mP2Tramo2;

        private double mRc;
        private double mVmax;

        private oTramoEjeBasico mTramoPrevio;
        private bool mIsCurvaGranRadio;

        public oTramosValoracion tramosValoracion = null;

        List<oTramoTijera> mListTramoDesdeT1 = new List<oTramoTijera>();
        List<oTramoTijera> mListTramoDesdeT2 = new List<oTramoTijera>();


        public List<oTramoTijera> mTramosGanadores { get; set; }
        oRoadPendientes roadPendientes = null;
        double longDiscretizacionProy;
        tadLayLogica.estudioTipo.IEstudio estudioData = null;


        oTijera miTijera1;
        oTijera miTijera2;

        Line miLineaTramo1;
        Line miLineaTramo2;

        double mCotaDesdeInicio = 0;
        double mCotaDesdeFin = 0;


        List<oTijera> mLstTijerasDesdeInicio = new List<oTijera>();
        List<oTijera> mLstTijerasDesdeFin = new List<oTijera>();
        Polyline mEjeVisibiliad = null;
        oAbanicoDesign abanicoDesign;
        Stopwatch miMedicion;
        double pkDesdeIni = 0;
        double pkDesdeFin = 0;

        private double mCoeficienteDistanciaTramo;
        private double mCoeficienteDistanciaEje;
        private double mDistanciaMinimaEntradaSalida;
        

        public oEntronqueFerrocarril(oP2d iP1Tramo1, 
                                        oP2d iP2Tramo1, 
                                        oP2d iP1Tramo2, 
                                        oP2d iP2Tramo2,  
                                        Polyline iEjeVisibiliad, 
                                        oAbanicoDesign iAbanicoDesign, 
                                        Stopwatch iMedicionTiempo, 
            oEntronqueData entronqueData)
        {
            mEjeVisibiliad = iEjeVisibiliad;
            mCotaDesdeInicio = entronqueData.CotaIni;
            mCotaDesdeFin = entronqueData.CotaFin;
            mTramosGanadores = new List<oTramoTijera>();
            mP1Tramo1 = iP1Tramo1;
            mP2Tramo1 = iP2Tramo1;

            mP1Tramo2 = iP1Tramo2;
            mP2Tramo2 = iP2Tramo2;

            mRc = entronqueData.RadioEntronque;
            mVmax = entronqueData.Velocidad;


            mTramoPrevio = new oTramoAvanceCorto();
            mTramoPrevio.P1 = new oP3d(iP1Tramo1.X, iP1Tramo1.Y, 0);
            mTramoPrevio.P2 = new oP3d(iP2Tramo1.X, iP2Tramo1.Y, 0);
            mTramoPrevio.idAbanico = entronqueData.IdLastTijera;
            mTramoPrevio.idTramo = entronqueData.IdLastTramo;


            abanicoDesign = iAbanicoDesign;
            tramosValoracion = entronqueData.TramosValoracion;

            miMedicion = iMedicionTiempo;

            mIsCurvaGranRadio = entronqueData.IsCurvaGranRadio;
            mCoeficienteDistanciaTramo = entronqueData.CoefDistTramo;
            mCoeficienteDistanciaEje = entronqueData.CoefDistEje;
            mDistanciaMinimaEntradaSalida = entronqueData.DistanciaMinimaEntradaSalida;
        }

        public void setUpListasTijeras(List<oTijera> iLstTijerasDsdInicio, List<oTijera> iLstTijerasDsdFin, oRoadPendientes iRoadPendientes, double ilongDiscretizacionProy, tadLayLogica.estudioTipo.IEstudio iEstudioData, double pkDsdIni)
        {
            mLstTijerasDesdeInicio = iLstTijerasDsdInicio;
            mLstTijerasDesdeFin = iLstTijerasDsdFin;
            roadPendientes = iRoadPendientes;
            longDiscretizacionProy = ilongDiscretizacionProy;
            estudioData = iEstudioData;   
            pkDesdeIni = pkDsdIni;
        }

        public bool createEntronque(bool iPenalizarTramosCortos, double iVelocidad, double iPendienteAntT1, double longitudTramoAnteriorT1, double iPendienteAntT2, double longitudTramoAnteriorT2)
        {

            List<oTramoTijera> misTramosValidosParesT1 = new List<oTramoTijera>();
            List<oTramoTijera> misTramosValidosParesT2 = new List<oTramoTijera>();

            createTramosEntronque(iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);

            //calcular cual es el tramo o conjunto de tramos con mejor valoracion

            //1. vemos si el entroque tiene un tramo unico o no
            bool existTramoUnico = (mListTramoDesdeT1.Count > mListTramoDesdeT2.Count);
            oTramoTijera miTramoUnico = null;

            if (existTramoUnico)
            {
                miTramoUnico = mListTramoDesdeT1.Last();
                mListTramoDesdeT1.Remove(miTramoUnico);
            }


            int i = 0;
            foreach (oTramoTijera miTramo in mListTramoDesdeT1)
            {

                if (miTramo != null && miTramo.isValido())
                {
                    if (mListTramoDesdeT2[i] != null && mListTramoDesdeT2[i].isValido())
                    {
                        List<Point2d> miLista2 = new List<Point2d>();
                        miLista2.Add(new Point2d(miLineaTramo2.StartPoint.X, miLineaTramo2.StartPoint.Y));
                        miLista2.Add(new Point2d(miLineaTramo2.EndPoint.X, miLineaTramo2.EndPoint.Y));
                        Polyline miPolLinea2 = new Polyline();
                        miPolLinea2.AddVertexAt(0, miLista2[0], 0, 0, 0);
                        miPolLinea2.AddVertexAt(1, miLista2[1], 0, 0, 0);

                        Point3d miPunto = miPolLinea2.GetClosestPointTo(new Point3d(mListTramoDesdeT2[i].P2.X, mListTramoDesdeT2[i].P2.Y, 0), false);
                        oP2d miPunto2d = new oP2d(miPunto.X, miPunto.Y);
                        oP2d miPuntoFinTramo = new oP2d(mListTramoDesdeT2[i].P2.X, mListTramoDesdeT2[i].P2.Y);
                        double dist = miPunto2d.distTo2d(miPuntoFinTramo);
                        if (Math.Round(dist) == 0)
                        {
                            misTramosValidosParesT1.Add(miTramo);
                            misTramosValidosParesT2.Add(mListTramoDesdeT2[i]);

                        }
                        else
                        {
                            mListTramoDesdeT2[i].mIsValido = false;
                            miTramo.mIsValido = false;
                        }
                    }
                }
                i++;
            }

            if (misTramosValidosParesT1.Count != 0 || existTramoUnico)
            {
                //Existe algun entronque valido 
                Polyline miPol = calcularTramoGanador(misTramosValidosParesT1, misTramosValidosParesT2, miTramoUnico,iPenalizarTramosCortos,iVelocidad);

                if (miPol.NumberOfVertices > 0)
                {
                    return true;
                }
                else
                {
                    //No existen entronques validos (Autocorreccion)
                    return false;
                }
            }
            else
            {
                //No existen entronques validos (Autocorreccion)
                return false;
            }

        }


        private void createTramosEntronque(double iPendienteAntT1, double longitudTramoAnteriorT1, double iPendienteAntT2, double longitudTramoAnteriorT2)
        {
            //Esta la interseccion sobre los tramos
            bool isPuntoIntersecOnT1 = false;
            bool isPuntoIntersecOnT2 = false;

            //La distancia entre el punto final de cada tramo y la interseccion es suficiente para realizar el entronque
            bool isDistT1Suficiente = false;
            bool isDistT2Suficiente = false;


            Line miTramo1 = new Line(new Point3d(mP1Tramo1.X, mP1Tramo1.Y, 0), new Point3d(mP2Tramo1.X, mP2Tramo1.Y, 0));
            Line miTramo2 = new Line(new Point3d(mP1Tramo2.X, mP1Tramo2.Y, 0), new Point3d(mP2Tramo2.X, mP2Tramo2.Y, 0));


            //Creo la Intersección
            Point3dCollection miColInter = new Point3dCollection();
            miTramo1.IntersectWith(miTramo2, Intersect.ExtendBoth, miColInter, IntPtr.Zero, IntPtr.Zero);

            double distanciaEntrePuntosEntronque = mP2Tramo1.distTo2d(mP2Tramo2);

            if (miColInter.Count != 0)
            {

                double distanciaP2T1PInt = mP2Tramo1.distTo2d(new oP2d(miColInter[0].X, miColInter[0].Y));
                double distanciaP2T2PInt = mP2Tramo2.distTo2d(new oP2d(miColInter[0].X, miColInter[0].Y));

                bool isValidaInterseccion = /*((0.2*mDistanciaMinimaEntradaSalida) <= distanciaP2T1PInt) &&*/
                                            ((1.1 * mDistanciaMinimaEntradaSalida) >= distanciaP2T1PInt);
                isValidaInterseccion = isValidaInterseccion &&
                                        //(((0.2*mDistanciaMinimaEntradaSalida) <= distanciaP2T2PInt) &&
                                        (1.1 * mDistanciaMinimaEntradaSalida) >= distanciaP2T2PInt;
                if (isValidaInterseccion)
                {

                    double miAzimutTramo1 = oTrigo.getAzimutGrados(mP1Tramo1, mP2Tramo1);
                    double miAzimutTramo2 = oTrigo.getAzimutGrados(mP1Tramo2, mP2Tramo2);

                    double miAzimutTramo2Contrario = oTrigo.getAzimutGrados(mP2Tramo2, mP1Tramo2);


                    double miAzimutTramo1PtoX = oTrigo.getAzimutGrados(mP2Tramo1, new oP2d(miColInter[0].X, miColInter[0].Y));
                    double miAzimutTramo2PtoX = oTrigo.getAzimutGrados(mP2Tramo2, new oP2d(miColInter[0].X, miColInter[0].Y));

                    if (Math.Round(miAzimutTramo1PtoX) != Math.Round(miAzimutTramo1)) isPuntoIntersecOnT1 = true;
                    if (Math.Round(miAzimutTramo2PtoX) != Math.Round(miAzimutTramo2)) isPuntoIntersecOnT2 = true;


                    //Calcular la distancia desde el final de las tijeras hasta el punto de interseccion (Comprobar que son mayores que TeD + vMax/2)
                    double miDistFinalTramo1ToIntersec = miColInter[0].DistanceTo(new Point3d(mP2Tramo1.X, mP2Tramo1.Y, 0));
                    double miDistFinalTramo2ToIntersec = miColInter[0].DistanceTo(new Point3d(mP2Tramo2.X, mP2Tramo2.Y, 0));

                    //Calculo Te del angulo de giro de los dos tramos

                    double miAngulo = Math.Abs(miAzimutTramo1 - miAzimutTramo2Contrario);
                    if (miAngulo > 180)
                    {
                        miAngulo = 360 - miAngulo;
                    }
                    double miAnguloRad = miAngulo / (180 / Math.PI);
                    double miA = Math.Pow((12 * Math.Pow(mRc, 3)), 0.25);
                    double miLe = Math.Pow(miA, 2) / mRc;
                    double miDRad = (miLe / (mRc * 2));
                    double miDGra = miDRad * (90 / Math.PI);
                    double miXe = miLe * (1 - (Math.Pow(miDRad, 2) / 10) + (Math.Pow(miDRad, 4) / 256) - (Math.Pow(miDRad, 6) / 9360) + (Math.Pow(miDRad, 8) / 685440));
                    double miYe = miLe * ((Math.Pow(miDRad, 1) / 3) - (Math.Pow(miDRad, 3) / 42) + (Math.Pow(miDRad, 5) / 1320) - (Math.Pow(miDRad, 7) / 75600));
                    double miIncrR = miYe - mRc * (1 - Math.Cos(miDRad));

                    double miXm = miXe - mRc * Math.Sin(miDRad);
                    double miYm = mRc + miIncrR;
                    double miTe = Math.Abs(miXm + (mRc + miIncrR) * Math.Tan(miAnguloRad / 2));

                    //Es la distancia suficiente?
                    if (miDistFinalTramo1ToIntersec > (miTe + (mVmax / 2))) isDistT1Suficiente = true;
                    if (miDistFinalTramo2ToIntersec > (miTe + (mVmax / 2))) isDistT2Suficiente = true;

                    //Comprobar tambien que los azimuts concuerdan Azimut (B,X) = Azimut (A,B) y Azimut (C,X) = Azimut (D,C)
                    if ((!isPuntoIntersecOnT1 && !isPuntoIntersecOnT2) && isDistT1Suficiente && isDistT2Suficiente)
                    {
                        // entronque CASO 1
                        //Calculo miTe Angulo entre 2

                        //Duda formula
                        double miTeCaso1 = Math.Abs(miXm + (mRc + (miAnguloRad / 2 * mRc)) * Math.Tan(miAnguloRad / 4));

                        crearListTramos(miTeCaso1, miColInter[0], new Point3d(), true, iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);


                    }
                    else
                    {
                        //entronque CASO 2


                        double miAng = ((miAnguloRad / 2) + Math.PI / 2);
                        //Duda formula 
                        double miTeCaso2 = Math.Abs(miXm + ((mRc + (miAng * mRc)) * Math.Tan(miAng / 2)));

                        if (isPuntoIntersecOnT1)
                        {
                            //calcular pXPrima sobre el tramo 1
                            oP2d miPXPrima = oTrigo.getP2FromAzimutLon(mP2Tramo1, miAzimutTramo1, miDistFinalTramo2ToIntersec);
                            crearListTramos(miTeCaso2, miColInter[0], new Point3d(miPXPrima.X, miPXPrima.Y, 0), false, iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);
                        }
                        else if (isPuntoIntersecOnT2)
                        {
                            //calcular pXPrima sobre el tramo 2
                            oP2d miPXPrima = oTrigo.getP2FromAzimutLon(mP2Tramo2, miAzimutTramo2, miDistFinalTramo1ToIntersec);
                            crearListTramos(miTeCaso2, new Point3d(miPXPrima.X, miPXPrima.Y, 0), miColInter[0], false, iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);
                        }
                        else if (miDistFinalTramo1ToIntersec > miDistFinalTramo2ToIntersec)
                        {
                            //calcular pXPrima sobre el tramo 1
                            oP2d miPXPrima = oTrigo.getP2FromAzimutLon(mP2Tramo1, miAzimutTramo1, miDistFinalTramo2ToIntersec);
                            crearListTramos(miTeCaso2, miColInter[0], new Point3d(miPXPrima.X, miPXPrima.Y, 0), false, iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);
                        }
                        else
                        {
                            //calcular pXPrima sobre el tramo 2
                            oP2d miPXPrima = oTrigo.getP2FromAzimutLon(mP2Tramo2, miAzimutTramo2, miDistFinalTramo1ToIntersec);
                            crearListTramos(miTeCaso2, new Point3d(miPXPrima.X, miPXPrima.Y, 0), miColInter[0], false, iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);
                        }


                    }
                }
                


            }
        }

        private void crearListTramos(double iTe, Point3d miPtoX, Point3d miPtoXPrima, bool isCaso1, double iPendienteAntT1, double longitudTramoAnteriorT1, double iPendienteAntT2, double longitudTramoAnteriorT2)
        {
            List<Point3d> misPuntosFinales = new List<Point3d>();
            var tramoFinalTramoOpuesto = oLw.reverseLw(mLstTijerasDesdeFin.Last().getTramoGanador().getTramoFinal());

            if (isCaso1)
            {
                double miDistanciaTramo1 = mP2Tramo1.distTo2d(new oP2d(miPtoX.X, miPtoX.Y));
                double miDistanciaTramo2 = mP2Tramo2.distTo2d(new oP2d(miPtoX.X, miPtoX.Y));

                double miDistanciaApoyoTramo1 = miDistanciaTramo1 / (Math.Max((miDistanciaTramo1 / iTe), 1));
                double miDistanciaApoyoTramo2 = miDistanciaTramo2 / (Math.Max((miDistanciaTramo2 / iTe), 1));

                miLineaTramo1 = new Line(new Point3d(mP2Tramo1.X, mP2Tramo1.Y, 0), miPtoX);
                miLineaTramo2 = new Line(new Point3d(mP2Tramo2.X, mP2Tramo2.Y, 0), miPtoX);

                List<Point3d> misPuntosTramo1 = getPuntosApoyo(miLineaTramo1, miDistanciaApoyoTramo1);
                List<Point3d> misPuntosTramo2 = getPuntosApoyo(miLineaTramo2, miDistanciaApoyoTramo2);

                int i = 0;
                //Crear los primeros tramos
                foreach (Point3d miPunto1 in misPuntosTramo1)
                {

                    if (miMedicion.ElapsedMilliseconds > abanicoDesign.segundosEntronque * 1000) throw new oExPropertieNullValue("No Existe solucion");

                    foreach (Point3d miPunto2 in misPuntosTramo2)
                    {

                        if (miMedicion.ElapsedMilliseconds > abanicoDesign.segundosEntronque * 1000) throw new oExPropertieNullValue("No Existe solucion");

                        //Definida la cota solo de P1
                        double miNextAzimut = oTrigo.getAzimutGrados(new oP2d(miPunto1.X, miPunto1.Y), new oP2d(miPunto2.X, miPunto2.Y));
                        mTramoPrevio.P2.Z = mCotaDesdeInicio;
                        mTramoPrevio.mTrazadoFinal = mLstTijerasDesdeInicio.Last().tramoGanador.getTramoFinal();
                        oTramoTijera miTramo = oTijera.calculaEjeTrazadoTramo(mTramoPrevio, mTramoPrevio.azimutGrados, mP2Tramo2, i, miNextAzimut, 0, 0, mTramoPrevio.idAbanico + 1, mRc, mVmax, new oP2d(miPunto1.X, miPunto1.Y), new Point3d(mP2Tramo1.X, mP2Tramo1.Y,0), false, mIsCurvaGranRadio);

                        if (miTramo != null)
                        {
                            misPuntosFinales.Add(miPunto2);
                            miTramo.createTramosFerrocarril(oTijera.GetSeparacion(true));
                            mListTramoDesdeT1.Add(miTramo);
                            i++;
                        }
                    }
                }

                //Crear los segundos tramos

                double miAzimutTramo2Contrario = oTrigo.getAzimutGrados(mP2Tramo2, mP1Tramo2);
                int index = 0;
                foreach (oTramoTijera miTramoAnterior in mListTramoDesdeT1)
                {

                    if (miMedicion.ElapsedMilliseconds > abanicoDesign.segundosEntronque * 1000) throw new oExPropertieNullValue("No Existe solucion");
                    //Definida la cota de P1 y P2

                    oTramoAvanceCorto TramoPrevioList1 = new oTramoAvanceCorto();
                    TramoPrevioList1.P1 = new oP3d(miTramoAnterior.mPuntoIntermedio.X, miTramoAnterior.mPuntoIntermedio.Y, 0);
                    TramoPrevioList1.P2 = new oP3d(miTramoAnterior.P2.X, miTramoAnterior.P2.Y, 0);
                    TramoPrevioList1.idAbanico = 0;
                    TramoPrevioList1.idTramo = 0;
                    TramoPrevioList1.mTrazadoFinal = miTramoAnterior.getTramoFinal();
                    oTramoTijera miTramoList2 = oTijera.calculaEjeTrazadoTramo(TramoPrevioList1, TramoPrevioList1.azimutGrados, mP2Tramo2, i, miAzimutTramo2Contrario, 0, 0, TramoPrevioList1.idAbanico + 1, mRc, mVmax, new oP2d(misPuntosFinales[index].X, misPuntosFinales[index].Y), new Point3d(miTramoAnterior.P2.X, miTramoAnterior.P2.Y, miTramoAnterior.P2.Z), false, mIsCurvaGranRadio);
                    if (miTramoList2 != null)
                    {
                        double dist = mP2Tramo2.distTo2d(new oP2d(miTramoList2.mTrazadoTramo.EndPoint.X, miTramoList2.mTrazadoTramo.EndPoint.Y));
                        if (dist > 0)
                        {
                            miTramoList2 = oTijera.calculaEjeTrazadoTramo(TramoPrevioList1, TramoPrevioList1.azimutGrados, mP2Tramo2, i, miAzimutTramo2Contrario, 0, dist, TramoPrevioList1.idAbanico + 1, mRc, mVmax, new oP2d(misPuntosFinales[index].X, misPuntosFinales[index].Y), new Point3d(miTramoAnterior.P2.X, miTramoAnterior.P2.Y, miTramoAnterior.P2.Z), false, mIsCurvaGranRadio);
                            miTramoList2.createTramosFerrocarril(oTijera.GetSeparacion(true), true, tramoFinalTramoOpuesto);
                        }
                    }

                    mListTramoDesdeT2.Add(miTramoList2);
                    index++;
                }


                //Crear el tramo unico sin recta intermedia
                //Definida la cota P1 y P2
                oTramoTijera miTramoUnico = oTijera.calculaEjeTrazadoTramo(mTramoPrevio, mTramoPrevio.azimutGrados, mP2Tramo2, i, miAzimutTramo2Contrario, 0, 0, mTramoPrevio.idAbanico + 1, mRc, mVmax, new oP2d(miPtoX.X, miPtoX.Y), new Point3d(mP2Tramo1.X, mP2Tramo1.Y,0), false, mIsCurvaGranRadio);
                if (miTramoUnico != null) miTramoUnico.createTramosFerrocarril(oTijera.GetSeparacion(true), true, tramoFinalTramoOpuesto);

                validarTramos3d(mListTramoDesdeT1, mListTramoDesdeT2, iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);
                validarTramos3d(miTramoUnico, iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);




            }
            else
            {
                double miDistanciaTramo1 = mP2Tramo1.distTo2d(new oP2d(miPtoXPrima.X, miPtoXPrima.Y));
                double miDistanciaTramo2 = mP2Tramo2.distTo2d(new oP2d(miPtoX.X, miPtoX.Y));

                double miDistanciaApoyoTramo1 = miDistanciaTramo1 / (Math.Max((miDistanciaTramo1 / iTe), 1));
                double miDistanciaApoyoTramo2 = miDistanciaTramo2 / (Math.Max((miDistanciaTramo2 / iTe), 1));

                miLineaTramo1 = new Line(new Point3d(mP2Tramo1.X, mP2Tramo1.Y, 0), miPtoXPrima);
                miLineaTramo2 = new Line(new Point3d(mP2Tramo2.X, mP2Tramo2.Y, 0), miPtoX);


                List<Point3d> misPuntosTramo1 = getPuntosApoyo(miLineaTramo1, miDistanciaApoyoTramo1);
                List<Point3d> misPuntosTramo2 = getPuntosApoyo(miLineaTramo2, miDistanciaApoyoTramo1);

                int i = 0;

                foreach (Point3d miPunto1 in misPuntosTramo1)
                {
                    if (miMedicion.ElapsedMilliseconds > abanicoDesign.segundosEntronque * 1000) throw new oExPropertieNullValue("No Existe solucion");
                    foreach (Point3d miPunto2 in misPuntosTramo2)
                    {
                        if (miMedicion.ElapsedMilliseconds > abanicoDesign.segundosEntronque * 1000) throw new oExPropertieNullValue("No Existe solucion");

                        //Definida la cota solo de P1
                        double miNextAzimut = oTrigo.getAzimutGrados(new oP2d(miPunto1.X, miPunto1.Y), new oP2d(miPunto2.X, miPunto2.Y));
                        mTramoPrevio.P2.Z = mCotaDesdeInicio;
                        mTramoPrevio.mTrazadoFinal = mLstTijerasDesdeInicio.Last().tramoGanador.getTramoFinal();
                        oTramoTijera miTramo = oTijera.calculaEjeTrazadoTramo(mTramoPrevio, mTramoPrevio.azimutGrados, mP2Tramo2, i, miNextAzimut, 0, 0, mTramoPrevio.idAbanico + 1, mRc, mVmax, new oP2d(miPunto1.X, miPunto1.Y), new Point3d(mP2Tramo1.X, mP2Tramo1.Y, 0), false, mIsCurvaGranRadio);

                        if (miTramo != null)
                        {
                            miTramo.createTramosFerrocarril(oTijera.GetSeparacion(true));
                            mListTramoDesdeT1.Add(miTramo);
                            misPuntosFinales.Add(miPunto2);
                            i++;
                        }
                    }
                }



                //Crear los segundos tramos

                double miAzimutTramo2Contrario = oTrigo.getAzimutGrados(mP2Tramo2, mP1Tramo2);
                int index = 0;
                foreach (oTramoTijera miTramoAnterior in mListTramoDesdeT1)
                {

                    if (miMedicion.ElapsedMilliseconds > abanicoDesign.segundosEntronque * 1000) throw new oExPropertieNullValue("No Existe solucion");
                    //Definida la cota de P1 y P2

                    oTramoAvanceCorto TramoPrevioList1 = new oTramoAvanceCorto();
                    TramoPrevioList1.P1 = new oP3d(miTramoAnterior.mVerticeEjeTrazado.getPuntoX, miTramoAnterior.mVerticeEjeTrazado.getPuntoY, 0);
                    TramoPrevioList1.P2 = new oP3d(miTramoAnterior.mTrazadoTramo.EndPoint.X, miTramoAnterior.mTrazadoTramo.EndPoint.Y, 0);
                    TramoPrevioList1.idAbanico = 0;
                    TramoPrevioList1.idTramo = 0;
                    TramoPrevioList1.mTrazadoFinal = miTramoAnterior.getTramoFinal();
                    oTramoTijera miTramoList2 = oTijera.calculaEjeTrazadoTramo(TramoPrevioList1, TramoPrevioList1.azimutGrados, mP2Tramo2, i, miAzimutTramo2Contrario, 0, 0, TramoPrevioList1.idAbanico + 1, mRc, mVmax, new oP2d(misPuntosFinales[index].X, misPuntosFinales[index].Y), new Point3d(miTramoAnterior.P2.X, miTramoAnterior.P2.Y, miTramoAnterior.P2.Z), false, mIsCurvaGranRadio);
                    
                    if (miTramoList2 != null)
                    {
                        double dist = mP2Tramo2.distTo2d(new oP2d(miTramoList2.mTrazadoTramo.EndPoint.X, miTramoList2.mTrazadoTramo.EndPoint.Y));
                        if (dist > 0)
                        {
                            miTramoList2 = oTijera.calculaEjeTrazadoTramo(TramoPrevioList1, TramoPrevioList1.azimutGrados, mP2Tramo2, i, miAzimutTramo2Contrario, 0, dist, TramoPrevioList1.idAbanico + 1, mRc, mVmax, new oP2d(misPuntosFinales[index].X, misPuntosFinales[index].Y), new Point3d(miTramoAnterior.P2.X, miTramoAnterior.P2.Y, miTramoAnterior.P2.Z), false, mIsCurvaGranRadio);
                            miTramoList2.createTramosFerrocarril(oTijera.GetSeparacion(true), true, tramoFinalTramoOpuesto);
                        }
                    }


                    mListTramoDesdeT2.Add(miTramoList2);
                    index++;
                }

                //Crear el tramo unico sin recta intermedia
                //Definida la cota P1 y P2
                oTramoTijera miTramoUnico = oTijera.calculaEjeTrazadoTramo(mTramoPrevio, mTramoPrevio.azimutGrados, mP2Tramo2, i, miAzimutTramo2Contrario, 0, 0, mTramoPrevio.idAbanico + 1, mRc, mVmax, new oP2d(miPtoX.X, miPtoX.Y), new Point3d(mP2Tramo1.X, mP2Tramo1.Y, 0), false, mIsCurvaGranRadio);
                if (miTramoUnico != null) miTramoUnico.createTramosFerrocarril(oTijera.GetSeparacion(true), true, tramoFinalTramoOpuesto);


                validarTramos3d(mListTramoDesdeT1, mListTramoDesdeT2, iPendienteAntT1, longitudTramoAnteriorT1, iPendienteAntT2, longitudTramoAnteriorT2);
            }
            

        }
        

        private List<Point3d> getPuntosApoyo(Line iLinea, double iSeparacion)
        {
            List<Point3d> misPuntosApoyo = new List<Point3d>();

            double miDistancia = iSeparacion;
            if (iLinea != null)
            {
                while (miDistancia <= iLinea.Length)
                {
                    Point3d miPunto = iLinea.GetPointAtDist(miDistancia);
                    misPuntosApoyo.Add(miPunto);
                    miDistancia = miDistancia + iSeparacion;
                }
            }
            if (misPuntosApoyo.Count == 0)
            {
                misPuntosApoyo.Add(iLinea.EndPoint);
            }

            return misPuntosApoyo;
        }

        private Polyline calcularTramoGanador(List<oTramoTijera> iTramosValidosParesT1, List<oTramoTijera> iTramosValidosParesT2, oTramoTijera iTramoUnico, bool iPenalizarTramosCortos, double iVelocidad)
        {
            Polyline miTrazadoEntronque = new Polyline();


            if (iTramoUnico != null)
            {
                iTramosValidosParesT1.Insert(0,iTramoUnico);
                iTramosValidosParesT2.Insert(0,iTramoUnico);
            }


            bool encontradoEntronque = false;
            int indexMax = 0;

            while (indexMax != -1 && !encontradoEntronque)
            {
                miTrazadoEntronque = new Polyline();
                int index = 0;
                double[] notas = new double[iTramosValidosParesT1.Count];
                indexMax = -1;
                double maximo = double.MinValue;

                foreach (oTramoTijera miTramo1 in iTramosValidosParesT1)
                {
                    if (miTramo1.mIsValido && iTramosValidosParesT2[index].mIsValido)
                    {
                        notas[index] = (miTramo1.valoracionPonderadaGlobal_0_10 + iTramosValidosParesT2[index].valoracionPonderadaGlobal_0_10) / 2;
                        if (iPenalizarTramosCortos)
                        {
                            var miLmin = 2.78*iVelocidad;
                            if (miTramo1.componenteLineaLongitudMenor(miLmin) ||
                                iTramosValidosParesT2[index].componenteLineaLongitudMenor(miLmin))
                                notas[index] = notas[index]/2;
                        }

                        if (notas[index] >= maximo)
                        {
                            maximo = notas[index];
                            indexMax = index;
                        }
                    }
                    index++;
                }

                if (indexMax != -1)
                {

                    if (iTramoUnico != null && indexMax == 0)
                    {
                        mTramosGanadores.Add(iTramoUnico);
                        miTrazadoEntronque.AddVertexAt(0, new Point2d(mP2Tramo1.X, mP2Tramo1.Y), 0, 0, 0);

                        for (int i = 0; i < iTramoUnico.mTrazadoTramo.NumberOfVertices; i++)
                        {
                            miTrazadoEntronque.AddVertexAt(i + 1, iTramoUnico.mTrazadoTramo.GetPoint2dAt(i), 0, 0, 0);
                        }

                        miTrazadoEntronque.AddVertexAt(miTrazadoEntronque.NumberOfVertices, new Point2d(mP2Tramo2.X, mP2Tramo2.Y), 0, 0, 0);
                    }
                    else
                    {

                        mTramosGanadores.Add(iTramosValidosParesT1[indexMax]);
                        mTramosGanadores.Add(iTramosValidosParesT2[indexMax]);

                        for (int i = 0; i < iTramosValidosParesT1[indexMax].mTrazadoTramo.NumberOfVertices; i++)
                        {
                            miTrazadoEntronque.AddVertexAt(i, iTramosValidosParesT1[indexMax].mTrazadoTramo.GetPoint2dAt(i), 0, 0, 0);
                        }

                        for (int i = 1; i < iTramosValidosParesT2[indexMax].mTrazadoTramo.NumberOfVertices; i++)
                        {
                            miTrazadoEntronque.AddVertexAt(miTrazadoEntronque.NumberOfVertices, iTramosValidosParesT2[indexMax].mTrazadoTramo.GetPoint2dAt(i), 0, 0, 0);
                        }
                        miTrazadoEntronque.AddVertexAt(miTrazadoEntronque.NumberOfVertices, new Point2d(mP2Tramo2.X, mP2Tramo2.Y), 0, 0, 0);

                    }
                    //Comprobar que el trazado del entronque no intersecciona con otro tramo 
                    if (!isEntronqueValidoInterseccion(miTrazadoEntronque))
                    {
                        iTramosValidosParesT2[indexMax].mIsValido = false;
                        iTramosValidosParesT2[indexMax].errorTramo =
                            eTramoEjeBasicoError.entronqueInterseccionTramosTijera;
                        iTramosValidosParesT1[indexMax].mIsValido = false;
                        iTramosValidosParesT1[indexMax].errorTramo =
                            eTramoEjeBasicoError.entronqueInterseccionTramosTijera;
                        encontradoEntronque = false;
                        mTramosGanadores.Clear();
                        miTrazadoEntronque.Dispose();
                    }
                    else
                    {
                        encontradoEntronque = true;
                    }
                }
            }

            return miTrazadoEntronque;
        }


        private bool isEntronqueValidoInterseccion(Polyline iTrazadoEntronque)
        {
            bool isValido = true;

            int numeroIntersecciones = 0;

            foreach (oTijera miTijera in mLstTijerasDesdeInicio)
            {
                Polyline miTramo = miTijera.tramoGanador.mTrazadoTramo;

                //ver si hay interseccion
                Point3dCollection miColInter = new Point3dCollection();
                miTramo.IntersectWith(iTrazadoEntronque, Intersect.OnBothOperands, miColInter, IntPtr.Zero, IntPtr.Zero);

                numeroIntersecciones = numeroIntersecciones + miColInter.Count;
            }


            foreach (oTijera miTijera in mLstTijerasDesdeFin)
            {
                Polyline miTramo = miTijera.tramoGanador.mTrazadoTramo;

                //ver si hay interseccion
                Point3dCollection miColInter = new Point3dCollection();
                miTramo.IntersectWith(iTrazadoEntronque, Intersect.OnBothOperands, miColInter, IntPtr.Zero, IntPtr.Zero);

                numeroIntersecciones = numeroIntersecciones + miColInter.Count;
            }

            if (numeroIntersecciones > 3)
            {
                isValido = false;
            }


            return isValido;
        }

        private void validarTramos3d(oTramoTijera miTramoUnico, double iPendienteAntT1, double longitudTramoAnteriorT1,
            double iPendienteAntT2, double longitudTramoAnteriorT2)
        {
            if (miTramoUnico != null)
            {
                AddTramoIntermedioTramoUnico(miTramoUnico);
                miTramoUnico.mTramosFerrocarril.First().P1 = mLstTijerasDesdeInicio.Last().tramoGanador.mTramosFerrocarril.Last().P2;
                miTramoUnico.mTramosFerrocarril.Last().P2 = mLstTijerasDesdeFin.Last().tramoGanador.mTramosFerrocarril.Last().P2;

                var ok = true;

                if (miTramoUnico.mTramosFerrocarril.Count > 2)
                {
                    var listTramosIni = new List<oTramoFerrocarril>();
                    var listTramosFin = new List<oTramoFerrocarril>();
                    int numRepeticiones = miTramoUnico.mTramosFerrocarril.Count/2;
                    if (miTramoUnico.mTramosFerrocarril.Count%2 == 0) numRepeticiones--;

                    for (int i = 0; i < numRepeticiones; i++)
                    {
                        listTramosIni.Add(miTramoUnico.mTramosFerrocarril[i]);
                        var j = miTramoUnico.mTramosFerrocarril.Count - i - 1;
                        listTramosFin.Insert(0,miTramoUnico.mTramosFerrocarril[j]);
                    }
                    var numTramos = listTramosIni.Count + listTramosFin.Count;
                    var difTramos = miTramoUnico.mTramosFerrocarril.Count - numTramos;
                    if(difTramos==2) listTramosIni.Add(miTramoUnico.mTramosFerrocarril[listTramosIni.Count]);

                    ok = AjustarAlzado(listTramosIni, listTramosFin,
                        iPendienteAntT1, longitudTramoAnteriorT1,
                        iPendienteAntT2, longitudTramoAnteriorT2, roadPendientes.calzadaPendienteCalculoMaximoPC, roadPendientes.calzadaPendienteCalculoMaximoPC);

                }
                if (miTramoUnico.mTramosFerrocarril.Count == 2) ok = false;
                if (ok)
                {
                    RellenarCotas(miTramoUnico);
                    mListTramoDesdeT1.Add(miTramoUnico);

                    foreach (var tramoFerrocarrilEntrada in miTramoUnico.mTramosFerrocarril)
                    {
                        tramoFerrocarrilEntrada.createSeccionP1P2(abanicoDesign.tramoAbanicoDiscretizacion, roadPendientes, estudioData, false);

                    }
                    miTramoUnico.isValido();
                }
            }
        }


        private void RellenarCotas(oTramoTijera miTramoUnico)
        {
            for (int i = 0; i < miTramoUnico.mTramosFerrocarril.Count; i++)
            {
                if (miTramoUnico.mTramosFerrocarril[i].P1.Z == 0)
                {
                    if (i - 1 >= 0)
                        miTramoUnico.mTramosFerrocarril[i].P1.Z = miTramoUnico.mTramosFerrocarril[i - 1].P2.Z;
                }
                if (miTramoUnico.mTramosFerrocarril[i].P2.Z == 0)
                {
                    if (i + 1 < miTramoUnico.mTramosFerrocarril.Count)
                        miTramoUnico.mTramosFerrocarril[i].P2.Z = miTramoUnico.mTramosFerrocarril[i + 1].P1.Z;

                }
            }
        }

        private void validarTramos3d(List<oTramoTijera> miLista1, List<oTramoTijera> miLista2, double iPendienteAntT1, double longitudTramoAnteriorT1, double iPendienteAntT2, double longitudTramoAnteriorT2)
        {

            //por cada par 1. calcular apoyado en el terreno
            List<oTramoTijera> miMejoresTramos1 = new List<oTramoTijera>();
            List<oTramoTijera> miMejoresTramos2 = new List<oTramoTijera>();
            List<double> misCotasT1 = new List<double>();
            List<double> misCotasT2 = new List<double>();

            int i = 0;
            foreach (oTramoTijera miTramo1 in miLista1)
            {

                if (miMedicion.ElapsedMilliseconds > abanicoDesign.segundosEntronque * 1000) break;


                if (miTramo1 != null)
                {
                    miTramo1.P1.Z = mCotaDesdeInicio;
                    miTramo1.mTramosFerrocarril[0].P1.Z = mCotaDesdeInicio;
                    oTramoTijera miTramo2 = miLista2[i];
                    if (miTramo2 != null)
                    {

                        AddTramoIntermedio(miTramo1, miTramo2);

                        var ok = AjustarAlzado(miTramo1.mTramosFerrocarril, miTramo2.mTramosFerrocarril,
                            iPendienteAntT1, longitudTramoAnteriorT1,
                            iPendienteAntT2, longitudTramoAnteriorT2, roadPendientes.calzadaPendienteCalculoMaximoPC, roadPendientes.calzadaPendienteCalculoMaximoPC);

                        
                        if (ok)
                        {
                            miMejoresTramos1.Add(miTramo1);
                            miMejoresTramos2.Add(miTramo2);



                            foreach (var tramoFerrocarrilEntrada in miTramo1.mTramosFerrocarril)
                            {
                                tramoFerrocarrilEntrada.createSeccionP1P2(abanicoDesign.tramoAbanicoDiscretizacion, roadPendientes, estudioData, false);

                            }
                            miTramo1.isValido();

                            foreach (var tramoFerrocarrilSalida in miTramo2.mTramosFerrocarril)
                            {
                                tramoFerrocarrilSalida.createSeccionP1P2(abanicoDesign.tramoAbanicoDiscretizacion, roadPendientes, estudioData, false);

                            }
                            miTramo2.isValido();
                        }
                    }
                }
                i++;
            }
            mListTramoDesdeT1 = miMejoresTramos1;
            mListTramoDesdeT2 = miMejoresTramos2;
        }

        private void AddTramoIntermedioTramoUnico(oTramoTijera miTramo1)
        {

            var tramosFinal1 = JoinTrazadosFinales(miTramo1.mTramosFerrocarril.Last().mTrazadoTramo, oLw.reverseLw(mLstTijerasDesdeFin.Last().tramoGanador.getTramoFinal()));

            miTramo1.mTramosFerrocarril.Last().mTrazadoTramo = tramosFinal1;

        }

        private static void AddTramoIntermedio(oTramoTijera miTramo1, oTramoTijera miTramo2)
        {

            var tramosFinal1 = JoinTrazadosFinales(miTramo1.mTramosFerrocarril.Last().mTrazadoTramo, miTramo1.getTramoFinal());
            var tramosFinal2 = JoinTrazadosFinales(miTramo2.mTramosFerrocarril.Last().mTrazadoTramo, miTramo2.getTramoFinal());

            miTramo1.mTramosFerrocarril.Last().mTrazadoTramo = tramosFinal1;
            miTramo2.mTramosFerrocarril.Last().mTrazadoTramo = tramosFinal2;
            
        }

        private static Polyline JoinTrazadosFinales(Polyline trazadoFinal1, Polyline trazadoFinal2)
        {
            var mitrazado = new Polyline();
            for (int j = 0; j < trazadoFinal1.NumberOfVertices; j++)
            {
                var pto = trazadoFinal1.GetPoint2dAt(j);
                mitrazado.AddVertexAt(j, pto, 0, 0, 0);
            }

            for (int j = 0; j < trazadoFinal2.NumberOfVertices; j++)
            {
                var pto = trazadoFinal2.GetPoint2dAt(j);
                mitrazado.AddVertexAt(trazadoFinal1.NumberOfVertices + j, pto, 0, 0, 0);
            }
            return mitrazado;
        }

        private double? GetMejorZ(double iPendienteAntT1, double longitudTramoAnteriorT1, oTramoFerrocarril miTramo1,
            double cotaOpuesta, double pendienteMaximaPermitida, double distEntronque, bool isEntrada)
        {
            double? Z = null;
            double longitud = miTramo1.mTrazadoTramo.Length;
            double? zterrenot1 = oSingletonTerreno.getInstance.getZFromXY(miTramo1.P2.X, miTramo1.P2.Y);
            double zpartida = miTramo1.P1.Z;
            if (!isEntrada)
            {
                zterrenot1 = oSingletonTerreno.getInstance.getZFromXY(miTramo1.P1.X, miTramo1.P1.Y);
                zpartida = miTramo1.P2.Z;
            }

            if (zterrenot1 == null) return null;
            double p1Z1Min = oTijera.getMINZPendienteCumple(longitud, zpartida, zterrenot1.Value,
                iPendienteAntT1, longitudTramoAnteriorT1);
            double p1Z1Max = oTijera.getMAXZPendienteCumple(longitud, zpartida, zterrenot1.Value,
                iPendienteAntT1, longitudTramoAnteriorT1);

            double p1Z2Min = GetPendienteMinimaCumplePermitida(zpartida,
                pendienteMaximaPermitida, longitud);
            double p1Z2Max = GetPendienteMaximaCumplePermitida(zpartida,
                pendienteMaximaPermitida, longitud);

            double p1Z3Min = GetPendienteMinimaCumplePermitida(cotaOpuesta,
                pendienteMaximaPermitida, distEntronque);
            double p1Z3Max = GetPendienteMaximaCumplePermitida(cotaOpuesta,
                pendienteMaximaPermitida, distEntronque);

            double zMax = Math.Min(p1Z1Max, p1Z2Max);
            zMax = Math.Min(zMax, p1Z3Max);
            double zMin = Math.Max(p1Z1Min, p1Z2Min);
            zMin = Math.Max(zMin, p1Z3Min);
            if (zMin <= zterrenot1 && zterrenot1 <= zMax)
                Z = zterrenot1;
            else if (zterrenot1 > zMax && zMax >= zMin)
                Z = zMax;
            else if (zterrenot1 < zMin && zMin <= zMax)
                Z = zMin;
            return Z;
        }

        private bool AjustarAlzado(List<oTramoFerrocarril> entradaEntronqueTramos,
            List<oTramoFerrocarril> salidaEntronqueTramos, double iPendienteAntEntrada, double longitudTramoAnteriorEntrada,
            double iPendienteAntSalida, double longitudTramoAnteriorSalida, double pendienteMaxEntrada, double pendienteMaximaSalida)
        {
            
            entradaEntronqueTramos.First().P1 =
                mLstTijerasDesdeInicio.Last().tramoGanador.mTramosFerrocarril.Last().P2;
            if(salidaEntronqueTramos.Count>0)
            salidaEntronqueTramos.Last().P2 =
                mLstTijerasDesdeFin.Last().tramoGanador.mTramosFerrocarril.Last().P2;

            var ultimoPuntoEntrada = entradaEntronqueTramos.First().P1;
            var ultimoPuntoSalida = mLstTijerasDesdeFin.Last().tramoGanador.mTramosFerrocarril.Last().P2;
            var longitudEntrePuntos = entradaEntronqueTramos.Sum(entradaEntronqueTramo => entradaEntronqueTramo.mTrazadoTramo.Length);
            if (salidaEntronqueTramos.Count > 0)
                longitudEntrePuntos += salidaEntronqueTramos.Sum(salidaEntronqueTramo => salidaEntronqueTramo.mTrazadoTramo.Length);

            var indexEntrada = 0;
            var indexSalida = 0;
            var isTurnoEntrada = true;


            while (indexEntrada < entradaEntronqueTramos.Count || indexSalida < salidaEntronqueTramos.Count-1)
            {
                if (isTurnoEntrada)
                {
                    if (indexEntrada < entradaEntronqueTramos.Count)
                    {
                        double? Z = null;
                        if (indexEntrada == 0)
                        {
                            Z = GetMejorZ(iPendienteAntEntrada, longitudTramoAnteriorEntrada,
                                entradaEntronqueTramos[indexEntrada], ultimoPuntoSalida.Z,
                                pendienteMaxEntrada, longitudEntrePuntos,
                                true);
                        }
                        else
                        {
                            Z = GetMejorZ(entradaEntronqueTramos[indexEntrada-1].pendienteConSignoPU, 
                                entradaEntronqueTramos[indexEntrada-1].mTrazadoTramo.Length,
                                entradaEntronqueTramos[indexEntrada], ultimoPuntoSalida.Z,
                                pendienteMaxEntrada, longitudEntrePuntos,
                                true);
                        }

                        if (Z == null) return false;
                        entradaEntronqueTramos[indexEntrada].P2.Z = Z.Value;
                        ultimoPuntoEntrada = entradaEntronqueTramos[indexEntrada].P2;
                        longitudEntrePuntos = longitudEntrePuntos -
                                              entradaEntronqueTramos[indexEntrada].mTrazadoTramo.Length;
                        if (indexEntrada + 1 < entradaEntronqueTramos.Count)
                            entradaEntronqueTramos[indexEntrada + 1].P1.Z = Z.Value;
                        indexEntrada++;
                        isTurnoEntrada = false;
                    }
                    else
                        isTurnoEntrada = false;
                }
                else
                {
                    double? Z = null;
                    if (indexSalida < salidaEntronqueTramos.Count-1)
                    {
                        var index = salidaEntronqueTramos.Count - indexSalida - 1;
                        if (index == salidaEntronqueTramos.Count-1)
                        {
                            Z = GetMejorZ(iPendienteAntSalida * (-1), longitudTramoAnteriorSalida,
                                salidaEntronqueTramos[index], ultimoPuntoEntrada.Z,
                                pendienteMaximaSalida, longitudEntrePuntos,
                                false);
                        }
                        else
                        {
                            Z = GetMejorZ(salidaEntronqueTramos[index + 1].pendienteConSignoPU * (-1),
                                salidaEntronqueTramos[index + 1].mTrazadoTramo.Length,
                                salidaEntronqueTramos[index], ultimoPuntoEntrada.Z,
                                pendienteMaximaSalida, longitudEntrePuntos,
                                false);
                        }
                        if (Z == null) return false;
                        salidaEntronqueTramos[index].P1.Z = Z.Value;
                        ultimoPuntoSalida = salidaEntronqueTramos[index].P1;
                        longitudEntrePuntos = longitudEntrePuntos -
                                              salidaEntronqueTramos[index].mTrazadoTramo.Length;
                        if (index - 1 >= 0)
                            salidaEntronqueTramos[index - 1].P2.Z = Z.Value;
                        indexSalida++;
                        isTurnoEntrada = true;

                    }
                    else
                        isTurnoEntrada = true;
                }
            }
            if (salidaEntronqueTramos.Count > 0)
            {
                salidaEntronqueTramos.First().P1.Z = entradaEntronqueTramos.Last().P2.Z;
                if (salidaEntronqueTramos.First().pendienteAbsolutaPC > pendienteMaximaSalida)
                    return false;
            }

            return true;

        }


        private static double GetPendienteMaximaCumplePermitida(double p1Z, double pendiente, double longitud)
        {
            double miZMASPendienteMaxCalzada = p1Z + (pendiente / 100 * longitud);
            return miZMASPendienteMaxCalzada;
        }

        private static double GetPendienteMinimaCumplePermitida(double p1Z, double pendiente, double longitud)
        {
            double miZMENOSPendienteMaxCalzada = p1Z - (pendiente / 100 * longitud);
            return miZMENOSPendienteMaxCalzada;
        }



    }
}
