using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using tadLayLogica;
using tadLayLogica.logica.EjeBasicoNew;
using tadLayLogica.estudioTipo;
using tadLayShare.puntos;
using EjeDeTrazado.puntosDelEje;
using Autodesk.AutoCAD.DatabaseServices;
using EjeDeTrazado.componentes;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Publishing;
using engNet.Extension.Collection;

namespace tayLogicaTijera.EjeBasico
{
    public class oTijera
    {
        private static oRoadDes mRoadDesign = null;
        private static oRoadPendientes mRoadPendiente = null;

        private static oAbanicoDesign mAbanicoDesign = null;
        private static IEstudio mEstudioDatos = null;
        private static oP3d mPuntoLlegada = null;
        private static double mCoefPrioridadRectas = 1;

        private int? mIdTijera = null;
        public oP3d mPtoOrigen = null;
        private oP2d mPtoTarget = null;
        private oTarget mTarget = null;

        //private bool? mIsAbanicoRotonda = null;

        public oTramoEjeBasico mTramoPrevio = null;
        oTramoTijera mLastTramoGanador = null;
        private bool isTijeraAlReves = false;

        //private bool mIsTijeraAlReves = false;

        private double mRc = 0;
        private double mVmax = 0;
        private bool mIsAvancesRapidos = false;

        private static Polyline mEjeVisibilidad = null;

        private bool mIsCurvaGranRadio = false;
        private static double mCoeficienteDistanciaTramo;
        private static double mCoeficienteDistanciaEje;
        private double mAzimutEntrada = 0;

        private bool dibujar;
        #region "Propiedades"



        /// <summary>
        /// Lista de Todos los Tramos por Abanico
        /// </summary>
        public List<oTramoTijera> lstTramosTijera { get; set; }

        /// <summary>
        /// Lista de Tramos Ganadores
        /// </summary>
        public oTramoTijera tramoGanador { get; set; }


        public bool hasTramosGanadores
        {

            get
            {
                if (tramoGanador != null)
                {
                    return true;
                }
                else
                {

                    return false;
                }

            }
        }
        #endregion


        public oTijera(int iIdTijera, oTramoEjeBasico iTramoPrevioAbanico, oP2d iPtoTarget, oTarget iTarget, double iRc, double iVmax,
            bool iAvanceRapido, bool iTijeraAlReves, Polyline iEjeVisibilidad, double coefDisTramo, double coefDistEje, bool dibujar = true)
        {
            mEjeVisibilidad = iEjeVisibilidad;
            mIdTijera = iIdTijera;
            mTramoPrevio = iTramoPrevioAbanico;
            mPtoOrigen = iTramoPrevioAbanico.P2;
            mPtoTarget = iPtoTarget;
            mTarget = iTarget;
            mRc = iRc;
            mVmax = iVmax;
            mIsAvancesRapidos = iAvanceRapido;
            isTijeraAlReves = iTijeraAlReves;
            mCoeficienteDistanciaTramo = coefDisTramo;
            mCoeficienteDistanciaEje = coefDistEje;
            mAzimutEntrada = oTrigo.getAzimutGrados(iTramoPrevioAbanico.P1.convertTo2d(), iTramoPrevioAbanico.P2.convertTo2d());
            this.dibujar = dibujar;
        }
        public oTijera(int iIdTijera, oP3d iPtoOrigen, double azimut, oP2d iPtoTarget, oTarget iTarget, double iRc, double iVmax,
            bool iAvanceRapido, bool iTijeraAlReves, Polyline iEjeVisibilidad, double coefDisTramo, double coefDistEje, bool dibujar = true)
        {
            mEjeVisibilidad = iEjeVisibilidad;
            mIdTijera = iIdTijera;
            mPtoOrigen = iPtoOrigen;
            mPtoTarget = iPtoTarget;
            mTarget = iTarget;
            mRc = iRc;
            mVmax = iVmax;
            mIsAvancesRapidos = iAvanceRapido;
            isTijeraAlReves = iTijeraAlReves;
            mCoeficienteDistanciaTramo = coefDisTramo;
            mCoeficienteDistanciaEje = coefDistEje;
            mAzimutEntrada = azimut;
            this.dibujar = dibujar;
        }



        #region "SETUP VARIABLES ESTATICAS"


        public static void SetUpObject(oRoadDes iRoadDesign, oRoadPendientes iRoadPendiente, oAbanicoDesign iDataAbanico, IEstudio iEstudioDatos, oP3d iPuntoLlegada, double iCoefPrioRectas)
        {
            mRoadDesign = iRoadDesign;
            mRoadPendiente = iRoadPendiente;
            mAbanicoDesign = iDataAbanico;
            mEstudioDatos = iEstudioDatos;
            mPuntoLlegada = iPuntoLlegada;
            mCoefPrioridadRectas = iCoefPrioRectas + 1;

        }

        #endregion


        public static double GetSeparacion(bool isCurvaGranRadio)
        {
            double AnguloRasante = 2 * Math.Atan(mRoadPendiente.calzadaPendienteProyectoMaximaPC / 100);
            double miSeparacion = (1.05 * MaxKv() * AnguloRasante) +
                                  (mRoadDesign.Vp / 2);
            if (!isCurvaGranRadio) miSeparacion = double.MaxValue;
            return miSeparacion;
        }


        private void createTijera(double? iPendAnt, bool isEntronque, oP3d iPuntoFinal, double? iLongAnterior, double rectaMinima = 0)
        {
            List<oTramoTijera> misTramosTijera = new List<oTramoTijera>();

            misTramosTijera = getListTramos(rectaMinima);

            var miSeparacion = GetSeparacion(mIsCurvaGranRadio);
            foreach (var miTramo in misTramosTijera)
            {
                //Creo los tramos ferrocarril
                miTramo.createTramosFerrocarril(miSeparacion);

                //Validaciones Planta
                miTramo.validarTramo2d(mEstudioDatos);

                //Creamos la Seccion
                miTramo.createSecciones(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, isEntronque, iPuntoFinal, iPendAnt, iLongAnterior);

                //Validamos tramos en 3d
                miTramo.validarTramo3d(3 * mRoadDesign.AijMinSalidaLlegada, mPuntoLlegada, mRoadPendiente.calzadaPendienteProyectoMaximaPC);
            }


            lstTramosTijera = new List<oTramoTijera>();

            //Cargamos los Tramos en el listado de tramos
            lstTramosTijera.AddRange(misTramosTijera);

        }


        private void createTijeraPendienteObligada(double? iPendAnt, bool isEntronque, oP3d iPuntoFinal,
            double? iLongAnterior, double pendienteObligada)
        {
            List<oTramoTijera> misTramosTijera = new List<oTramoTijera>();

            misTramosTijera = getListTramos();

            var miSeparacion = GetSeparacion(mIsCurvaGranRadio);
            foreach (var miTramo in misTramosTijera)
            {
                //Creo los tramos ferrocarril
                miTramo.createTramosFerrocarril(miSeparacion);

                //Validaciones Planta
                miTramo.validarTramo2d(mEstudioDatos);

                //Creamos la Seccion
                miTramo.createSeccionesPendienteObligada(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos,
                    isEntronque, iPuntoFinal, iPendAnt, iLongAnterior, pendienteObligada);

                //Validamos tramos en 3d
                miTramo.validarTramo3d(3 * mRoadDesign.AijMinSalidaLlegada, mPuntoLlegada,
                    mRoadPendiente.calzadaPendienteProyectoMaximaPC);
            }


            lstTramosTijera = new List<oTramoTijera>();

            //Cargamos los Tramos en el listado de tramos
            lstTramosTijera.AddRange(misTramosTijera);

        }

        public void calcularTramosGanadores(oTramosValoracion iTramosValoracion, bool isAbanicoEspecial, double? iPendAnt, bool isEntronque, oP3d iPuntoFinal, double? iLongAnterior, double rectaMinima = 0)
        {

            createTijera(iPendAnt, isEntronque, iPuntoFinal, iLongAnterior, rectaMinima);

            if (existenTramosValidos())
            {
                createValoracion(iTramosValoracion);

                tramoGanador = getTramoGanador();
            }
            else
            {
                tramoGanador = null;
            }

        }

        public void calcularTramosGanadoresPendienteObligada(oTramosValoracion iTramosValoracion, bool isAbanicoEspecial,
            double? iPendAnt, bool isEntronque, oP3d iPuntoFinal, double? iLongAnterior, double pendienteObligada)
        {

            createTijeraPendienteObligada(iPendAnt, isEntronque, iPuntoFinal, iLongAnterior, pendienteObligada);

            if (existenTramosValidos())
            {
                createValoracion(iTramosValoracion);

                tramoGanador = getTramoGanador();
            }
            else
            {
                tramoGanador = null;
            }

        }


        public void drawTijeraTramos(string iLayerTramos, string iXData)
        {
            if (dibujar)
            {

                foreach (var item in lstTramosTijera)
                {
                    if (item != null)
                    {
                        /*
                         * Se comenta para ver velocidad ** juanma**
                         */
                        item.drawTramo2D(iLayerTramos, iXData);
                    }
                }

            }
        }

        public void drawTramosGanadores(string iLayerTramosGanadores)
        {
            /*
             * Se comenta para ver velocidad ** juanma**
             */
            if (dibujar)
            {

                tramoGanador.drawTramo3D(iLayerTramosGanadores);
            }
        }

        public List<oTramoTijera> getListTramos(double rectaMinima = 0)
        {
            List<oTramoTijera> miLstTramosTijeraPrimario = new List<oTramoTijera>();
            var radio = mRc;
            mIsCurvaGranRadio = mAbanicoDesign.isCurvaGranRadio;
            if (mAbanicoDesign.isCurvaGranRadio) radio = mAbanicoDesign.granRadio;
            if (mRoadDesign.grupo == eRoadGrupo.Grupo1 && radio >= 7500) mIsCurvaGranRadio = true;
            if (mRoadDesign.grupo == eRoadGrupo.Grupo2 && radio >= 3500) mIsCurvaGranRadio = true;


            if (mAbanicoDesign.incluirTramosRectaMaxima)
            {
                if (mRoadDesign.grupo == eRoadGrupo.Grupo1 && radio < 700) radio = 700;
                if (mRoadDesign.grupo == eRoadGrupo.Grupo2 && !mAbanicoDesign.isCurvaGranRadio) radio = 2 * radio;
            }

            oP2d ptoTarget = mPtoTarget;
            if (ptoTarget == null) ptoTarget = GetPtoTarget(mAzimutEntrada);

            int index = 0;
            //Creo los Tramos sin recta
            if (mAbanicoDesign.incluirTramosSinRecta)
            {
                var te = rectaMinima;
                index = AddTramosTijera(mPtoOrigen, index, te, radio, miLstTramosTijeraPrimario, ptoTarget);
            }

            //creo los tramos con recta minima
            if (mAbanicoDesign.incluirTramosRectaMinima)
            {
                var teMin = TeRectaMinima(mVmax);
                if (teMin < rectaMinima) teMin = rectaMinima;
                var pointMin = oTrigo.getP2FromAzimutLon(mPtoOrigen, mAzimutEntrada, teMin);
                index = AddTramosTijera(pointMin, index, teMin, radio, miLstTramosTijeraPrimario, ptoTarget);
            }

            //creo los tramos con recta maxima
            if (mAbanicoDesign.incluirTramosRectaMaxima)
            {
                var teMax = TeRectaMaxima(mVmax);
                if (teMax < rectaMinima) teMax = rectaMinima;
                var pointMax = oTrigo.getP2FromAzimutLon(mPtoOrigen, mAzimutEntrada, teMax);
                index = AddTramosTijera(pointMax, index, teMax, radio, miLstTramosTijeraPrimario, ptoTarget);
            }


            //creo los tramos con recta limitada
            if (mAbanicoDesign.incluirTramosRectaLimitada)
            {
                var teLim = TeRectaLimitada(mVmax);
                if (teLim < rectaMinima) teLim = rectaMinima;
                var pointLim = oTrigo.getP2FromAzimutLon(mPtoOrigen, mAzimutEntrada, teLim);
                index = AddTramosTijera(pointLim, index, teLim, radio, miLstTramosTijeraPrimario, ptoTarget);
            }



            return miLstTramosTijeraPrimario;

        }

        private oP2d GetPtoTarget(double azimutentrada)
        {
            var point = mPtoOrigen.convertTo2d();
            if (mAbanicoDesign.incluirTramosRectaMaxima)
            {
                point = oTrigo.getP2FromAzimutLon(mPtoOrigen.convertTo2d(), azimutentrada,
                    TeRectaMaxima(mVmax));
            }
            else if (mAbanicoDesign.incluirTramosRectaLimitada)
            {
                point = oTrigo.getP2FromAzimutLon(mPtoOrigen, azimutentrada,
                    TeRectaLimitada(mVmax));
            }
            else if (mAbanicoDesign.incluirTramosRectaMinima)
            {
                point = oTrigo.getP2FromAzimutLon(mPtoOrigen, azimutentrada, TeRectaMinima(mVmax));
            }
            var ptoTarget = mTarget.getPtoTarget(point, false);
            return ptoTarget;
        }

        private int AddTramosTijera(oP2d point, int index, double teMax, double radio, List<oTramoTijera> miLstTramosTijeraPrimario, oP2d iPtoTarget)
        {
            Dictionary<int, double> miDicIdTramoAzimutTramos;
            miDicIdTramoAzimutTramos = mAbanicoDesign.getLstAzimutGrados(point, iPtoTarget);
            foreach (var item in miDicIdTramoAzimutTramos)
            {
                oTramoTijera miTramo1 = calculaEjeTrazadoTramo(mTramoPrevio, mAzimutEntrada, iPtoTarget, index, item.Value,
                    teMax, 0, (int)mIdTijera, radio, mVmax, null, new Point3d(mPtoOrigen.X, mPtoOrigen.Y, mPtoOrigen.Z),
                    isTijeraAlReves, mIsCurvaGranRadio);
                if (miTramo1 != null && miTramo1.mTrazadoTramo.NumberOfVertices > 1)
                {
                    if (miTramo1.longitud2d < mAbanicoDesign.longitudMinimaTramo)
                    {
                        miTramo1.mIsValido = false;
                        miTramo1.errorTramo = eTramoEjeBasicoError.DescartadoUsuarioLongitudMinima;
                    }
                    miLstTramosTijeraPrimario.Add(miTramo1);
                }
                index++;
            }
            return index;
        }

        public static double TeRectaMinima(double vMax)
        {
            return 2.78 * vMax;
        }

        public static double TeRectaMaxima(double vMax)
        {
            return 16.70 * vMax;
        }

        public static double TeRectaLimitada(double vMax)
        {
            var longTe = 400;
            if (vMax < 100) longTe = 300;
            if (vMax < 90) longTe = 230;
            if (vMax < 80) longTe = 175;
            if (vMax < 70) longTe = 85;
            if (vMax < 60) longTe = 50;
            if (vMax < 50) longTe = 30;
            return longTe;
        }

        public static double getZPendienteCumple(oTramoFerrocarril iTramo, double iPendienteAnt, double longitudTramoAnterior)
        {
            return getZPendienteCumple(iTramo.mTrazadoTramo.Length, iTramo.P1.Z, iTramo.P2terreno.Z, iPendienteAnt, longitudTramoAnterior);
        }

        public static double getZPendienteCumple(double longitud, double p1z, double p2zterreno, double iPendienteAnt, double longitudTramoAnterior)
        {
            var Lv = Math.Min(longitud, longitudTramoAnterior);
            var max = MaxKv();

            double miCambioPendienteMax = ((Lv - (mRoadDesign.Vp / 2)) / (1.05 * max));
            double pendienteMin = (miCambioPendienteMax * (-1)) + iPendienteAnt;
            double pendienteMax = miCambioPendienteMax + iPendienteAnt;

            double ZMin = oTrigo.getP2zFromP1andLon(p1z, pendienteMin * 100, longitud);
            double ZMax = oTrigo.getP2zFromP1andLon(p1z, pendienteMax * 100, longitud);

            double Z;
            if (ZMin < p2zterreno && p2zterreno < ZMax)
                Z = p2zterreno;
            else if (p2zterreno > ZMax)
                Z = ZMax;
            else
                Z = ZMin;


            return Z;
        }

        public static double getMAXZPendienteCumple(double longitud, double p1z, double p2zterreno, double iPendienteAnt, double longitudTramoAnterior)
        {
            var Lv = Math.Min(longitud, longitudTramoAnterior);
            var max = MaxKv();

            double miCambioPendienteMax = ((Lv - (mRoadDesign.Vp / 2)) / (1.05 * max));
            double pendienteMax = miCambioPendienteMax + iPendienteAnt;

            double ZMax = oTrigo.getP2zFromP1andLon(p1z, pendienteMax * 100, longitud);


            return ZMax;
        }


        public static double getMINZPendienteCumple(double longitud, double p1z, double p2zterreno, double iPendienteAnt, double longitudTramoAnterior)
        {
            var Lv = Math.Min(longitud, longitudTramoAnterior);
            var max = MaxKv();

            double miCambioPendienteMax = ((Lv - (mRoadDesign.Vp / 2)) / (1.05 * max));
            double pendienteMin = (miCambioPendienteMax * (-1)) + iPendienteAnt;

            double ZMin = oTrigo.getP2zFromP1andLon(p1z, pendienteMin * 100, longitud);
            return ZMin;
        }


        public static double MaxKv()
        {
            var kvDeseable = tadLayLogica.datos.normas.oDalTbNormasKv.getKvFromXmlProyecto(mRoadDesign.Vp, eKvPrefer.deseable);
            var kvMinimo = tadLayLogica.datos.normas.oDalTbNormasKv.getKvFromXmlProyecto(mRoadDesign.Vp, eKvPrefer.minimo);

            var maxdeseable = Math.Max(kvDeseable.KvConcavo, kvDeseable.kvConvexo);
            var maxminimo = Math.Max(kvMinimo.KvConcavo, kvMinimo.kvConvexo);
            var max = Math.Max(maxminimo, maxdeseable);
            return max;
        }

        public static bool cumplePendiente(oTramoFerrocarril iTramo, double iPendienteAnt, double longitudTramoAnterior)
        {
            var Lv = Math.Min(iTramo.mTrazadoTramo.Length, longitudTramoAnterior);
            var pendienteMaxima = Math.Max(mRoadPendiente.calzadaPendienteCalculoMaximoPC,
                mRoadPendiente.estructuraPendienteCalculoMaximoPC) / 100;
            var AL = 2 * Math.Atan(pendienteMaxima);

            double miPendiente = (iTramo.P2.Z - iTramo.P1.Z) / iTramo.mTrazadoTramo.Length;
            var max = MaxKv();

            var Lmin = (1.05 * max * AL) + (mRoadDesign.Vp / 2);
            if (iTramo.mTrazadoTramo.Length >= Lmin) return true;

            double miCambioPendienteMax = ((Lv - (mRoadDesign.Vp / 2)) / (1.05 * max));
            double pendienteMin = (miCambioPendienteMax * (-1)) + iPendienteAnt;
            double pendienteMax = miCambioPendienteMax + iPendienteAnt;

            if ((miPendiente <= pendienteMax) && (miPendiente >= pendienteMin))
            {
                return true;
            }
            return false;

        }

        public static oTramoTijera calculaEjeTrazadoTramo(oTramoEjeBasico iTramoPrevio, double azimutGrados, oP2d iPtoTarget, int iTramoId, double item, double iIncrementoTe, double iIncrementoPost, int iIdTijera, double mRc, double mVmax, oP2d iPtoInt, Point3d iPtoOrigen, bool isTijeraAlReves, bool isCurvaGranRadio)
        {
            try
            {

                double distEntrePuntos = 0;
                bool defPuntoInt = false;
                if (iPtoInt != null)
                {
                    defPuntoInt = true;
                    distEntrePuntos = iPtoInt.distTo2d(new oP2d(iPtoOrigen.X, iPtoOrigen.Y));
                }

                double iRc = mRc;
                oTramoTijera miTramo = null;
                double azimut = azimutGrados;
                double miAngulo = Math.Abs(azimut - item);

                if (miAngulo > 180)
                {
                    miAngulo = 360 - miAngulo;
                }
                if (miAngulo <= 2) return null;
                double miAnguloRad = miAngulo / (180 / Math.PI);
                int recalculado = 0;

                Curva miCurva = null;
                Linea miLinea = null;
                Clotoide miClo1 = null;
                Clotoide miClo2 = null;
                bool longitudMinimaCumple = true;
                Vertice miVertice = null;
                EjeTrazado.tipoCurva tipoCurva = GetTipoCurva(miAngulo, iRc);
                int miGrupo = 1;
                if (mRoadDesign.grupo == eRoadGrupo.Grupo2) miGrupo = 2;

                var param = (325 - (27.7777 * miAngulo)) * (57.2957 / miAngulo);
                if (tipoCurva == EjeTrazado.tipoCurva.c3500)
                {
                    isCurvaGranRadio = true;
                    iRc = 3500;
                    if (param > iRc) iRc = param;
                }
                if (tipoCurva == EjeTrazado.tipoCurva.c7500)
                {
                    isCurvaGranRadio = true;
                    iRc = 7500;
                    if (param > iRc) iRc = param;
                }

                List<Componente> misComponentesCurvaNoPaso = new List<Componente>();
                double[] miPuntoMedio = new double[2];
                double miQe2 = 0;
                double miQe1 = 0;
                Punto3d[] misPuntos = new Punto3d[5];

                oP3d miPuntointermedio = new oP3d();
                oP3d mioP3d3 = new oP3d();

                do
                {
                    recalculado++;


                    double miLe;
                    double miTe;
                    double miA;
                    if (
                        !CalculaParametrosCurva(ref iIncrementoTe, tipoCurva, miAnguloRad, miGrupo, miAngulo,
                            defPuntoInt, distEntrePuntos, ref iRc, mVmax, out miLe, out miTe, out miA, isCurvaGranRadio))
                        return null;

                    List<Punto3d> miPuntosEje = new List<Punto3d>();

                    Punto3d miP2tp = new Punto3d(iPtoOrigen.X, iPtoOrigen.Y, 0);
                    oP2d miP2d2 = oTrigo.getP2FromAzimutLon(new oP2d(iPtoOrigen.X, iPtoOrigen.Y), azimut, miTe + iIncrementoTe);
                    double? miZTerreno = oSingletonTerreno.getInstance.getZFromXY(miP2d2.toArray2d());
                    if (miZTerreno != null)
                    {
                        miPuntointermedio = new oP3d(miP2d2.X, miP2d2.Y, (double)miZTerreno);
                        Punto3d miP2 = new Punto3d(miP2d2.X, miP2d2.Y, 0);
                        miPuntosEje.Add(miP2);

                        oP2d miP2d3 = oTrigo.getP2FromAzimutLon(miP2d2, item, miTe);
                        double? miZP3 = oSingletonTerreno.getInstance.getZFromXY(miP2d3.toArray2d());
                        if (miZP3 != null)
                        {
                            Punto3d miP3 = new Punto3d(miP2d3.X, miP2d3.Y, (double)miZP3);
                            mioP3d3 = new oP3d(miP3.coordenadaX, miP3.coordenadaY, miP3.coordenadaZ);
                            miPuntosEje.Add(miP3);


                            EjeTrazado miEjeTramo = new EjeTrazado(miPuntosEje, miGrupo, false, mRoadDesign.Rp,
                                mRoadDesign.Vp, mRoadDesign.peralte, 2, true);
                            double A;
                            if (!isCurvaGranRadio)
                            {
                                misPuntos = miEjeTramo.addCurvaNoPaso(ref mRc, azimut, item, miP2tp,
                                    miP2, EjeTrazado.getSentidoCurva(azimut, item),
                                    tipoCurva == EjeTrazado.tipoCurva.cnpAnguloReducido, out A);
                            }
                            else
                            {
                                misPuntos = miEjeTramo.addCurvaGranRadio(iRc, azimut, item, miP2,
                                    EjeTrazado.getSentidoCurva(azimut, item));
                                miLe = 0;
                            }




                            misComponentesCurvaNoPaso = new List<Componente>();
                            miClo1 = new Clotoide(misPuntos[0],
                                misPuntos[1],
                                iRc,
                                0,
                                EjeTrazado.getSentidoCurva(azimut, item),
                                0,
                                0,
                                true,
                                EjeTrazado.tipoClotoide.entrada,
                                azimutGrados,
                                false,
                                EjeTrazado.getDelta(azimut, item),
                                false,
                                miLe, miA);
                            misComponentesCurvaNoPaso.Add(miClo1);
                            miCurva = new Curva(misPuntos[1], misPuntos[2], misPuntos[4], iRc, 0, 0, 0,
                                EjeTrazado.getSentidoCurva(azimutGrados, item),
                                misComponentesCurvaNoPaso.Last().getLongitud());
                            misComponentesCurvaNoPaso.Add(miCurva);
                            miClo2 = new Clotoide(misPuntos[2],
                                misPuntos[3],
                                iRc,
                                0,
                                EjeTrazado.getSentidoCurva(azimut, item),
                                0,
                                0,
                                false,
                                EjeTrazado.tipoClotoide.salida,
                                item,
                                false,
                                EjeTrazado.getDelta(azimut, item),
                                false,
                                miLe, miA);
                            misComponentesCurvaNoPaso.Add(miClo2);

                            if (iIncrementoPost > 0)
                            {
                                oP2d miPuntoLineaFinal =
                                    oTrigo.getP2FromAzimutLon(
                                        new oP2d(misPuntos[3].coordenadaX, misPuntos[3].coordenadaY), item,
                                        iIncrementoPost);
                                Linea miLineaFinal = new Linea(misPuntos[3],
                                    new Punto3d(miPuntoLineaFinal.X, miPuntoLineaFinal.Y, 0), 0, 0, item);
                                misComponentesCurvaNoPaso.Add(miLineaFinal);

                            }

                            miVertice = new Vertice(new Punto3d(miPuntointermedio.X, miPuntointermedio.Y, 0), item,
                                EjeTrazado.getSentidoCurva(azimutGrados, item), iRc,
                                EjeTrazado.tipoCurva.cnp, EjeTrazado.tipoSegmento.noValorado,
                                EjeTrazado.getDelta(azimutGrados, item), miCurva.getCentroCurva);



                            miPuntoMedio =
                                miCurva.getPointAtDist(miCurva.getPkIni + (miCurva.getPkFinal() - miCurva.getPkIni) / 2);
                            miQe2 = miClo2.getQe();
                            miQe1 = miClo1.getQe();

                        }
                    }

                    if (misPuntos[0] != null &&
                        !EjeTrazado.isSolapeS(misPuntos[4], misPuntos[1], misPuntos[2],
                            new Punto3d(miPuntoMedio[0], miPuntoMedio[1], 0), miQe1, miQe2))
                    {
                        Punto3d miP = misPuntos[0];

                        Polyline miEje2 = new Polyline();
                        miEje2.AddVertexAt(0, new Point2d(iPtoOrigen.X, iPtoOrigen.Y), 0, 0, 0);
                        double miPk = 0;
                        int index = 1;
                        Point2d? miPuntoAnt = null;
                        foreach (Componente miComponente in misComponentesCurvaNoPaso)
                        {
                            if (miComponente.getLongitud() != 0)
                            {
                                while (miPk < miComponente.getPkFinal())
                                {
                                    double[] miPunto;
                                    miPunto = miComponente.getPointAtDist(miPk);
                                    var miPunto2d = new Point2d(miPunto[0], miPunto[1]);
                                    miEje2.AddVertexAt(index, miPunto2d, 0, 0, 0);
                                    miPk = miPk + 10;
                                    index++;


                                }
                                double[] miPunto2;
                                miPunto2 = miComponente.getPointAtDist(miComponente.getPkFinal());
                                miEje2.AddVertexAt(index, new Point2d(miPunto2[0], miPunto2[1]), 0, 0, 0);
                                index++;
                            }

                        }
                        Point2d miUltimoPunto = new Point2d(miEje2.EndPoint.X, miEje2.EndPoint.Y);

                        if (iTramoPrevio == null)
                            miTramo = new oTramoTijera((int)iIdTijera, iTramoId, 0, new oP3d(iPtoOrigen.X, iPtoOrigen.Y, iPtoOrigen.Z),
                                new oP3d(miUltimoPunto.X, miUltimoPunto.Y, mioP3d3.Z), iPtoTarget, miPuntointermedio, false,
                                mEjeVisibilidad, mAbanicoDesign, mCoeficienteDistanciaTramo, mCoeficienteDistanciaEje);
                        else
                            miTramo = new oTramoTijera((int)iIdTijera, iTramoId, iTramoPrevio,
                                new oP3d(miUltimoPunto.X, miUltimoPunto.Y, mioP3d3.Z), iPtoTarget, miPuntointermedio, false,
                                mEjeVisibilidad, mAbanicoDesign, mCoeficienteDistanciaTramo, mCoeficienteDistanciaEje);
                        miTramo.mTrazadoTramo = miEje2;
                        miTramo.mComponentesEjeTrazado = misComponentesCurvaNoPaso;
                        miTramo.mVerticeEjeTrazado = miVertice;


                    }
                    else
                    {
                        iRc = 1.5 * mRc;
                    }
                } while (misPuntos[0] != null &&
                         (EjeTrazado.isSolapeS(misPuntos[4], misPuntos[1], misPuntos[2],
                             new Punto3d(miPuntoMedio[0], miPuntoMedio[1], 0), miQe1, miQe2) || !longitudMinimaCumple) &&
                         recalculado < 2);


                if (isTijeraAlReves && miTramo != null)
                {
                    crearComponentesAlReves(ref miTramo, azimutGrados, item);

                }
                return miTramo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool CalculaParametrosCurva(ref double iIncrementoTe, EjeTrazado.tipoCurva tipoCurva, double miAnguloRad,
            int miGrupo, double miAngulo, bool defPuntoInt, double distEntrePuntos, ref double iRc, double velocidad, out double miLe,
            out double miTe, out double miA, bool isGranRadio)
        {
            miLe = EjeTrazado.CalculoParametrosCurva(ref iRc, miAnguloRad, tipoCurva == EjeTrazado.tipoCurva.cnpAnguloReducido, out miA, miGrupo, velocidad);

            double miDRad = (miLe / (iRc * 2));
            double miDGra = miDRad * (90 / Math.PI);
            double miXe = miLe *
                          (1 - (Math.Pow(miDRad, 2) / 10) + (Math.Pow(miDRad, 4) / 256) - (Math.Pow(miDRad, 6) / 9360) +
                           (Math.Pow(miDRad, 8) / 685440));
            double miYe = miLe *
                          ((Math.Pow(miDRad, 1) / 3) - (Math.Pow(miDRad, 3) / 42) + (Math.Pow(miDRad, 5) / 1320) -
                           (Math.Pow(miDRad, 7) / 75600));

            double miIncrR = miYe - iRc * (1 - Math.Cos(miDRad));

            double miXm = miXe - iRc * Math.Sin(miDRad);
            double miYm = iRc + miIncrR;


            miTe = Math.Abs(miXm + (iRc + miIncrR) * Math.Tan(miAnguloRad / 2));
            if (isGranRadio)
                miTe = Math.Abs(iRc * Math.Tan(miAnguloRad / 2));

            if (defPuntoInt) iIncrementoTe = distEntrePuntos - miTe;
            if (iIncrementoTe < 0)
            {
                return false;
            }
            return true;
        }

        private static EjeTrazado.tipoCurva GetTipoCurva(double miAngulo, double iRc)
        {
            EjeTrazado.tipoCurva tipoCurva;
            var grupo = 1;
            if (mRoadDesign.grupo == eRoadGrupo.Grupo2) grupo = 2;
            tipoCurva = EjeTrazado.getTipoCurva(miAngulo, false, grupo, iRc);
            return tipoCurva;
        }

        #region "operaciones sobre la lista de tramos de la tijera"


        public void comprobarCompatibilidadTrazado(oP3d iPuntoOpuesto)
        {
            foreach (oTramoTijera miTramoTijera in lstTramosTijera)
            {
                miTramoTijera.comprobarCompatibilidadTrazado(iPuntoOpuesto, mRoadPendiente.calzadaPendienteCalculoMaximoPC / 100);
            }
        }

        public bool existenTramosValidos()
        {
            var miQuery = from p in lstTramosTijera
                          where p.isValido()
                          select p;

            if (miQuery == null || miQuery.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Valoramos el Abanico en función de la Ponderación de Notas de oTramosValoracion
        /// </summary>
        public void createValoracion(oTramosValoracion iValoracionesGlobales)
        {


            oValoracionLocalToGlobal miValoracionDis = this.valoracionDistancia();
            oValoracionLocalToGlobal miValoracionPendiente = this.valoracionSlope();
            oValoracionLocalToGlobal miValoracionCoste = this.valoracionCoste();


            //Valoracion Globales ; Comparando entre Abanicos
            foreach (var item in lstTramosTijera)
            {
                if (item.isValido())
                {
                    item.valoracionDistanciaGlobal_0_10 = miValoracionDis.getNota_ValorMaximoCalifica_0(item.getValoracionDistanciaLocal());
                    item.valoracionPendienteGlobal_0_10 = miValoracionPendiente.getNota_ValorMaximoCalifica_10(item.getValoracionOrografiaLocal());
                    item.valoracionCosteImplantacionGlobal_0_10 = miValoracionCoste.getNota_ValorMaximoCalifica_0(item.getValoracionImplantacionLocal());


                }
            }



            //Obtener La ValoracionPonderada
            foreach (var item in lstTramosTijera)
            {
                if (item.isValido())
                {

                    item.valoracionPonderadaGlobal_0_10 = iValoracionesGlobales.getValoracionGlobalPonderada(item.valoracionDistanciaGlobal_0_10,
                                                                                                             item.valoracionPendienteGlobal_0_10,
                                                                                                             item.valoracionCosteImplantacionGlobal_0_10);


                    if (item.isRecto) item.valoracionPonderadaGlobal_0_10 = item.valoracionPonderadaGlobal_0_10 * mCoefPrioridadRectas;

                }


                foreach (oTramoFerrocarril miTramoF in item.mTramosFerrocarril)
                {
                    miTramoF.valoracionDistanciaGlobal_0_10 = item.valoracionDistanciaGlobal_0_10;
                    miTramoF.valoracionPendienteGlobal_0_10 = item.valoracionPendienteGlobal_0_10;
                    miTramoF.valoracionCosteImplantacionGlobal_0_10 = item.valoracionCosteImplantacionGlobal_0_10;

                    miTramoF.valoracionPonderadaGlobal_0_10 = item.valoracionPonderadaGlobal_0_10;
                }
            }

        }

        /// <summary>
        /// Valoración Por Distancia
        /// </summary>
        private oValoracionLocalToGlobal valoracionDistancia()
        {
            double miCosteMimino = (from p in lstTramosTijera where p.isValido() select p.getValoracionDistanciaLocal()).Min();
            double miCosteMaximo = (from p in lstTramosTijera where p.isValido() select p.getValoracionDistanciaLocal()).Max();


            oValoracionLocalToGlobal miValoracion = new oValoracionLocalToGlobal(miCosteMimino, miCosteMaximo);

            return miValoracion;
        }

        /// <summary>
        /// Valoracion Por Pendiente Terreno
        /// </summary>
        private oValoracionLocalToGlobal valoracionSlope()
        {
            double miCosteMimino = (from p in lstTramosTijera where p.isValido() select p.getValoracionOrografiaLocal()).Min();
            double miCosteMaximo = (from p in lstTramosTijera where p.isValido() select p.getValoracionOrografiaLocal()).Max();


            oValoracionLocalToGlobal miValoracion = new oValoracionLocalToGlobal(miCosteMimino, miCosteMaximo);

            return miValoracion;
        }

        /// <summary>
        /// Valoracion por Coste Implantación
        /// </summary>
        private oValoracionLocalToGlobal valoracionCoste()
        {
            double miCosteMimino = (from p in lstTramosTijera where p.isValido() select p.getValoracionImplantacionLocal()).Min();
            double miCosteMaximo = (from p in lstTramosTijera where p.isValido() select p.getValoracionImplantacionLocal()).Max();

            oValoracionLocalToGlobal miValoracion = new oValoracionLocalToGlobal(miCosteMimino, miCosteMaximo);

            return miValoracion;
        }

        /// <summary>
        /// Listado con los Tramos Ganadores de la Coleccion
        /// </summary>
        public oTramoTijera getTramoGanador()
        {

            var miQuery = from p in lstTramosTijera
                          where p.isValido()
                          orderby p.valoracionPonderadaGlobal_0_10 descending
                          select p;


            oTramoTijera miTramoTijeraGanador = miQuery.First();

            //List<oTramoEjeBasico> miLstTramosEjesBasicosWin = miTramoAbanicoWin.lstTramos.ConvertAll(p => (oTramoEjeBasico)p);

            return miTramoTijeraGanador;

        }


        #endregion

        public static void crearComponentesAlReves(ref oTramoTijera miTramo, double azimutAnt, double azimut)
        {
            List<Componente> miListaAlReves = new List<Componente>();
            if (miTramo.mComponentesEjeTrazado.Count > 1)
            {
                Clotoide miClo1 = (Clotoide)miTramo.mComponentesEjeTrazado[0];
                Curva miCurva = (Curva)miTramo.mComponentesEjeTrazado[1];
                Clotoide miClo2 = (Clotoide)miTramo.mComponentesEjeTrazado[2];

                double miAzimut = miClo2.mAzimut + 180;
                if (miAzimut >= 360) miAzimut = miAzimut - 360;
                Clotoide miNewClo1 = new Clotoide(miClo2.getPuntoSalida,
                                                            miClo2.getPuntoEntrada,
                                                            miClo2.getRadio,
                                                            0,
                                                            EjeTrazado.getSentidoCurva(azimut, azimutAnt),
                                                            0,
                                                            0,
                                                            true,
                                                            EjeTrazado.tipoClotoide.entrada,
                                                            miAzimut,
                                                            false,
                                                            EjeTrazado.getDelta(azimut, azimutAnt),
                                                            false,
                                                             miClo2.mLe, miClo2.mA);
                miListaAlReves.Add(miNewClo1);

                Curva miNewCurva = new Curva(miCurva.getPuntoSalida, miCurva.getPuntoEntrada, miCurva.getCentroCurva, miCurva.getRadio, 0, 0, 0, EjeTrazado.getSentidoCurva(azimut, azimutAnt), miListaAlReves.Last().getLongitud());
                miListaAlReves.Add(miNewCurva);


                miAzimut = miClo1.mAzimut + 180;
                if (miAzimut >= 360) miAzimut = miAzimut - 360;
                Clotoide miNewClo2 = new Clotoide(miClo1.getPuntoSalida,
                                                            miClo1.getPuntoEntrada,
                                                miClo1.getRadio,
                                                0,
                                                EjeTrazado.getSentidoCurva(azimut, azimutAnt),
                                                0,
                                                0,
                                                false,
                                                EjeTrazado.tipoClotoide.salida,
                                                miAzimut,
                                                false,
                                                EjeTrazado.getDelta(azimut, azimutAnt),
                                                false,
                                                miClo1.mLe, miClo1.mA);
                miListaAlReves.Add(miNewClo2);
            }
            else
            {
                Linea miLinea = (Linea)miTramo.mComponentesEjeTrazado[0];
                Linea miNewLinea = new Linea(miLinea.getPuntoSalida, miLinea.getPuntoEntrada, 0, 0, oTrigo.getAzimutGrados(new oP2d(miLinea.getPuntoSalida.coordenadaX, miLinea.getPuntoSalida.coordenadaY), new oP2d(miLinea.getPuntoEntrada.coordenadaX, miLinea.getPuntoEntrada.coordenadaY)));
                miListaAlReves.Add(miNewLinea);
            }

            miTramo.mComponentesEjeTrazado = miListaAlReves;


        }

        public void ErrorNoDefToValid()
        {
            foreach (var tramoTijera in lstTramosTijera)
            {
                if (tramoTijera.isValido() == false &&
                    tramoTijera.errorTramo == eTramoEjeBasicoError.errorNoIdentificado)
                {
                    tramoTijera.mIsValido = true;

                    foreach (var tramoFerrocarril in tramoTijera.mTramosFerrocarril)
                    {
                        tramoFerrocarril.isTramoValido = true;
                    }
                }
            }
        }



    }
}
