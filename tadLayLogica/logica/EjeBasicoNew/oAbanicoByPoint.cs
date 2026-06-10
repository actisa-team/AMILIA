using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{


    using Autodesk.AutoCAD.Runtime;
    using System.Windows.Forms;

    using engNet.Extension.Collection;

    using tadLayLogica.zonaGis;
    using tadLayLogica.estudioTipo;
    using tadLayShare.puntos;
    using tadLayShare;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using tadLayLogica.Comandos;


    public class oAbanicoByPoint
    {


        private static oRoadDes mRoadDesign = null;
        private static oRoadPendientes mRoadPendiente = null;

        private static oAbanicoDesign mAbanicoDesign = null;
        private static IEstudio mEstudioDatos = null;
        private static oP3d mPuntoLlegada = null;

        private int? mIdAbanico = null;
        private oP2d mPtoOrigen = null;
        private oP2d mPtoTarget = null;

        private bool? mIsAbanicoRotonda = null;

        private oTramoEjeBasico mTramoPrevio = null;
        oTramoEjeBasico mLastTramoGanador = null;
        private bool dibujar = false;






        #region "Constructor"

        /// <summary>
        /// Constructor Salida en Rotonda
        /// </summary>
        public oAbanicoByPoint(int iIdAbanico, oP3d iPtoOrigen, oP2d iPtoTarget, bool dibujar = true)
        {

            //Id del Abanico
            mIdAbanico = iIdAbanico;
            //Obtengo el Azimut 
            double miAzimutGrados = iPtoOrigen.azimut(iPtoTarget, eAng.grados);
            //Generamos el TramoPrevio
            double miAzimutTramoPrevioGrados = oTrigo.azimutGradosFormatTo360(miAzimutGrados + 180);


            oP2d miP1 = iPtoOrigen.getPointFromAzimutAndLongitud(miAzimutTramoPrevioGrados, mRoadDesign.AijMin);

            oP3d miP13d = miP1.convertTo3d(0) as oP3d;

            oTramoEjeBasico miTramoPrevioFicticio = new oTramoEjeBasico(-1, -1, miP13d, iPtoOrigen, eTramoTipoEjeBasico.avanceCorto);

            mTramoPrevio = miTramoPrevioFicticio;

            mPtoOrigen = iPtoOrigen;
            mPtoTarget = iPtoTarget;

            mIsAbanicoRotonda = true;
            this.dibujar = dibujar;

        }


        public oAbanicoByPoint(int iIdAbanico, oTramoEjeBasico iTramoPrevioAbanico, oP2d iPtoTarget, bool dibujar = true)
        {
            mIdAbanico = iIdAbanico;
            mTramoPrevio = iTramoPrevioAbanico;
            mPtoOrigen = iTramoPrevioAbanico.P2.convertTo2d();
            mPtoTarget = iPtoTarget;
            mIsAbanicoRotonda = false;
            this.dibujar = dibujar;
        }


        #endregion
        #region "SETUP VARIABLES ESTATICAS"


        public static void SetUpObject(oRoadDes iRoadDesign, oRoadPendientes iRoadPendiente, oAbanicoDesign iDataAbanico, IEstudio iEstudioDatos, oP3d iPuntoLlegada)
        {
            mRoadDesign = iRoadDesign;
            mRoadPendiente = iRoadPendiente;
            mAbanicoDesign = iDataAbanico;
            mEstudioDatos = iEstudioDatos;
            mPuntoLlegada = iPuntoLlegada;

        }


        public static void showInfoTramos()
        {
            oTramoAbanico.evTramo += new EventHandler<engNet.eventos.oEventArgs<oTramoAbanico>>(oTramoAbanico_evTramo);
        }

        static void oTramoAbanico_evTramo(object sender, engNet.eventos.oEventArgs<oTramoAbanico> e)
        {
            engCadNet.oCadManager.thisEditor.WriteMessage("\n" + e.Value.infoProcess());
            Application.DoEvents();
        }

        public static void showInfoTramosUnload()
        {
            oTramoAbanico.evTramo -= new EventHandler<engNet.eventos.oEventArgs<oTramoAbanico>>(oTramoAbanico_evTramo);
        }

        #endregion
        #region "Propiedades"

        public bool hasTramosGanadores
        {

            get
            {
                if (lstTramosGanadores.Count > 0)
                {
                    return true;
                }
                else
                {

                    return false;
                }

            }
        }


        /// <summary>
        /// Lista de Todos los Tramos por Abanico
        /// </summary>
        public oTramoAbanicoCollection lstTramosAbanico { get; set; }

        /// <summary>
        /// Lista de Tramos Ganadores
        /// </summary>
        public List<oTramoEjeBasico> lstTramosGanadores { get; set; }


        /// <summary>
        /// Ultimo Tramo Ganador
        /// </summary>
        public oTramoEjeBasico lastTramoGanador
        {
            get
            {
                if (mLastTramoGanador == null)
                {
                    mLastTramoGanador = lstTramosGanadores[lstTramosGanadores.Count - 1];
                }
                return mLastTramoGanador;
            }

            set
            {
                mLastTramoGanador = value;
            }


        }




        #endregion


        #region "Metodos Publicos"



        /// <summary>
        /// Obtener Tramos Ganadores
        /// </summary>
        public void calcularTramosGanadores(oTramosValoracion iTramosValoracion, bool isAbanicoEspecial)
        {

            //Creamos el Abanico Avance Corto y Largo
            createAbanicoAvanceCortosLargos(isAbanicoEspecial);

            if (lstTramosAbanico.existenTramosValidos())
            {
                lstTramosAbanico.createValoracion(iTramosValoracion);

                lstTramosGanadores = lstTramosAbanico.getTramosGanadores();
            }
            else if (mRoadDesign.allowPermitirReduccionesVelocidad)
            {

                //Creo el Abanico AijMinimoMinimo
                this.createAbanicoAijMinimoMinimo();

                if (lstTramosAbanico.existenTramosValidos())
                {
                    lstTramosAbanico.createValoracion(iTramosValoracion);

                    lstTramosGanadores = lstTramosAbanico.getTramosGanadores();
                }
                else
                {
                    lstTramosGanadores = new List<oTramoEjeBasico>();
                }

            }
            else
            {
                lstTramosGanadores = new List<oTramoEjeBasico>();
            }

        }

        public List<oTramoEjeBasico> getLstTramosByPosition(int iPosition)
        {
            List<oTramoEjeBasico> lstTramosPosition = new List<oTramoEjeBasico>();

            foreach (oTramoAbanico miTramo in lstTramosAbanico)
            {
                if (miTramo.idPosicion == iPosition)
                {
                    lstTramosPosition.Add(miTramo);
                }
            }

            return lstTramosPosition;
        }


        public List<Point2d> getPuntosCriticosZNP(double AminEntreTres)
        {
            List<Point2d> misPuntosCriticos = new List<Point2d>();

            oTramoAbanico miTramoDerecha = new oTramoAvanceCorto();
            oTramoAbanico miTramoIzquierda = new oTramoAvanceCorto();
            oTramoAbanico miTramoCentro = new oTramoAvanceCorto();

            foreach (oTramoAbanico miTramo in lstTramosAbanico)
            {
                if (miTramo.idPosicion == 0)
                {
                    miTramoCentro = miTramo;
                }
                else if (miTramo.idPosicion == 18)
                {
                    miTramoIzquierda = miTramo;
                }
                else if (miTramo.idPosicion == -18)
                {
                    miTramoDerecha = miTramo;
                }
            }

            Line miLineaPrevia = new Line(new Point3d(mTramoPrevio.P2.X, mTramoPrevio.P2.Y, 0), new Point3d(mTramoPrevio.P1.X, mTramoPrevio.P1.Y, 0));
            Point3d miPunto0 = miLineaPrevia.GetPointAtDist(AminEntreTres);
            misPuntosCriticos.Add(new Point2d(miPunto0.X, miPunto0.Y));

            Line miLineaDer = new Line(new Point3d(miTramoDerecha.P1.X, miTramoDerecha.P1.Y, 0), new Point3d(miTramoDerecha.P2.X, miTramoDerecha.P2.Y, 0));
            Point3d miPunto1 = miLineaDer.GetPointAtDist(AminEntreTres);
            misPuntosCriticos.Add(new Point2d(miPunto1.X, miPunto1.Y));

            Line miLineaCen = new Line(new Point3d(miTramoCentro.P1.X, miTramoCentro.P1.Y, 0), new Point3d(miTramoCentro.P2.X, miTramoCentro.P2.Y, 0));
            Point3d miPunto2 = miLineaCen.GetPointAtDist(AminEntreTres);
            misPuntosCriticos.Add(new Point2d(miPunto2.X, miPunto2.Y));

            Line miLineaIzq = new Line(new Point3d(miTramoIzquierda.P1.X, miTramoIzquierda.P1.Y, 0), new Point3d(miTramoIzquierda.P2.X, miTramoIzquierda.P2.Y, 0));
            Point3d miPunto3 = miLineaIzq.GetPointAtDist(AminEntreTres);
            misPuntosCriticos.Add(new Point2d(miPunto3.X, miPunto3.Y));

            misPuntosCriticos = oComandoSimplificarPolilinea.ordena4Puntos(misPuntosCriticos);

            return misPuntosCriticos;
        }

        #endregion
        #region "Metodos DRAW"

        public void drawAijMinMaxAvance(string iLayer)
        {
            engCadNet.oCircle.addCircle2D(this.mPtoOrigen.toArray2d(), mRoadDesign.AijMin, iLayer);
            engCadNet.oCircle.addCircle2D(this.mPtoOrigen.toArray2d(), mRoadDesign.AijMax, iLayer);
            engCadNet.oCircle.addCircle2D(this.mPtoOrigen.toArray2d(), mRoadDesign.avanceMax, iLayer);
        }


        public void drawAbanicoTramos(string iLayerTramos)
        {
            if (dibujar)
            {
                foreach (var item in lstTramosAbanico)
                {
                    item.drawTramo2D(iLayerTramos);
                }
            }
        }

        public void drawAbanicoSecciones(string iLayerTramoSecciones)
        {
            if (dibujar)
            {
                foreach (var item in lstTramosAbanico)
                {
                    item.drawSeccion(iLayerTramoSecciones);
                }
            }
        }


        public void drawTramosGanadores(string iLayerTramosGanadores)
        {
            if (dibujar)
            {
                foreach (var item in lstTramosGanadores)
                {
                    item.drawTramo3D(iLayerTramosGanadores);
                }
            }
        }

        #endregion
        #region "Metodos Privados"

        /* private void MDT_Abanico(List<oTramoAbanico> miLstTramosAvancesCortos)
         {
             double x_max, x_min, y_max, y_min;
             var target = miLstTramosAvancesCortos[0].P1;
             x_min = target.X;
             x_max = target.X;
             y_min = target.Y;
             y_max = target.Y;
             foreach (var item in miLstTramosAvancesCortos)
             {
                 var target2 = item.P2;
                 if (x_min > target2.X)
                 {
                     x_min = target2.X;
                 }
                 if (x_max < target2.X)
                 {
                     x_max = target2.X;
                 }
                 if (y_min > target2.Y)
                 {
                     y_min = target2.Y;
                 }
                 if (y_max < target2.Y)
                 {
                     y_max = target2.Y;
                 }
             }

             double dis_extra = 500;
             x_min -= dis_extra;
             x_max += dis_extra;
             y_min -= dis_extra;
             y_max += dis_extra;

             var nube_puntos=oSingletonPuntosTerreno.getInstance.Get_puntos();

             // Filtrar los puntos dentro del rango
             List<Punto3d> puntos_en_rango = nube_puntos
                 .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                 .ToList();

             List<Triangulo> _znpTriangulos = new List<Triangulo>();
             var mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", 1000, 200,
                        "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, 150);
             oSingletonTerreno.getInstance.Set_Terreno(mTerreno);
         }*/

        private void createAbanicoAvanceCortosLargos(bool isAbanicoEspecial)
        {

            #region "AbanicoPrimario"

            List<oTramoAbanico> miLstTramosAvancesCortos = new List<oTramoAbanico>();

            //Creo los TramosAbanicosPrimarios
            miLstTramosAvancesCortos = getLstTramosAvancesCortos();

            /*
             * Se va a crear un terreno unico para cada abanico para ver si con eso se soluciona el tamaño de los mdt ** juanma **
             */

            //oSingletonPuntosTerreno.getInstance.MDT_Abanico(miLstTramosAvancesCortos);
            /*
             * 
             */

            //Validaciones Planta
            foreach (var item in miLstTramosAvancesCortos)
            {
                item.validarAnguloEntreTramos();

                item.validarAijDesviacionesMaximas(mAbanicoDesign.invalidarTramosAvanceCortoPorIncrementoLongitud, mAbanicoDesign.invalidarTramosIncrementoLongitudPC.Value);

                item.validarAnguloEntreTramosAijMinimoMinimo(mRoadDesign.getAnguloMinimoTramoSiguiente);


                item.validarTramoP2InsideTerreno();

                item.validarTramoP2NearBordesTerreno();

                item.validarTramoZonasNoPaso();

                item.validarCruceDPH(mEstudioDatos);

                //item.validarCruceInf(mEstudioDatos);

                item.validarDentroDPH(mEstudioDatos);
            }

            //Creamos la Seccion
            foreach (var item in miLstTramosAvancesCortos)
            {

                item.createSeccion(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, isAbanicoEspecial);

                //validacion 3d
                item.validarTramoCercanoPF(3 * mRoadDesign.AijMin, mPuntoLlegada, mRoadPendiente.calzadaPendienteProyectoMaximaPC);
            }

            #endregion

            #region "AbanicosSecundarios"

            List<oTramoAbanico> miLstTramosAvancesLargos = new List<oTramoAbanico>();

            //Creo los Tramos Avance Largo
            // !!!LAS VALIDACIONES SE HACEN EN EL GETLSTTRAMOS!!!!!!!
            if (!mIsAbanicoRotonda.Value && mAbanicoDesign.abanicoTipo == eAbanicoTipo.largos)
            {
                miLstTramosAvancesLargos = this.getLstTramosAvancesLargos(miLstTramosAvancesCortos);

                foreach (var item in miLstTramosAvancesLargos)
                {
                    item.createSeccion(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, isAbanicoEspecial);


                    //validacion 3d
                    item.validarTramoCercanoPF(3 * mRoadDesign.AijMin, mPuntoLlegada, mRoadPendiente.calzadaPendienteProyectoMaximaPC);
                }
            }


            #endregion

            #region "Creamos la lista de tramos por Abanico"

            lstTramosAbanico = new oTramoAbanicoCollection();

            //Cargamos los Tramos en el listado de tramos
            lstTramosAbanico.AddRange(miLstTramosAvancesCortos);


            if (mAbanicoDesign.abanicoTipo == eAbanicoTipo.largos)
            {
                lstTramosAbanico.AddRange(miLstTramosAvancesLargos);
            }

            #endregion

        }

        private void createAbanicoAijMinimoMinimo()
        {

            #region "Tramos Reduccion Puntual Velocidad"


            List<oTramoAbanico> miLstTramosAijMinimoMinimo = new List<oTramoAbanico>();

            miLstTramosAijMinimoMinimo = this.getLstTramosAijMinimoMinimo();



            //Validaciones Planta
            foreach (var item in miLstTramosAijMinimoMinimo)
            {
                item.validarAnguloEntreTramosAijMinimoMinimo(mRoadDesign.getAijMinimoMinimoByAngulo);

                item.validarTramoP2InsideTerreno();

                item.validarTramoZonasNoPaso();
            }


            //Creamos las Secciones
            foreach (var item in miLstTramosAijMinimoMinimo)
            {
                item.createSeccion(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);
            }

            #endregion


            #region "Creo la Coleccion con todos los Tramos"

            lstTramosAbanico.AddRange(miLstTramosAijMinimoMinimo);

            #endregion



        }

        /// <summary>
        /// Get Lista Tramos Avance Cortos
        /// </summary>
        private List<oTramoAbanico> getLstTramosAvancesCortos()
        {

            //Diccionario Posicion Tramo (Key)  - AzimutGrados
            Dictionary<int, double> miDicIdTramoAzimutTramos = mAbanicoDesign.getLstAzimutGrados(mTramoPrevio.P2.convertTo2d(), mPtoTarget);


            List<oTramoAbanico> miLstTramosAbanicoPrimario = new List<oTramoAbanico>();

            oTramoAvanceCorto miTrAbanico;

            double? miAngRadianesTramoPrevioTramo = null;
            double miLonAij = 0;

            eTramoTipoEjeBasico miTramoTipo;

            if (mAbanicoDesign.abanicoTipo == eAbanicoTipo.corto)
            {
                miTramoTipo = eTramoTipoEjeBasico.avanceCorto;
            }
            else if (mAbanicoDesign.abanicoTipo == eAbanicoTipo.largos)
            {
                miTramoTipo = eTramoTipoEjeBasico.avanceLargo;
            }
            else
            {
                throw new oExEnumNotImplemented(mAbanicoDesign.abanicoTipo.ToString());
            }




            //Creo los Tramos Abanico Primario
            foreach (var item in miDicIdTramoAzimutTramos)
            {

                double miAngulo = Math.Abs(mTramoPrevio.azimutGrados - item.Value);
                if (miAngulo > 180) miAngulo = 360 - miAngulo;

                miAngRadianesTramoPrevioTramo = mTramoPrevio.getAnguloConTramoNext(item.Value, eAng.radianes);
                miLonAij = mRoadDesign.getAij(miAngRadianesTramoPrevioTramo.Value);
                miTrAbanico = new oTramoAvanceCorto(miTramoTipo, mIdAbanico.Value, item.Key, mTramoPrevio, item.Value, miLonAij, mPtoTarget);

                if (miAngulo <= 2)
                {
                    miTrAbanico.isTramoValido = false;
                    miTrAbanico.errorTramo = eTramoEjeBasicoError.angulosconsecutivos;
                }
                miLstTramosAbanicoPrimario.Add(miTrAbanico);
            }



            return miLstTramosAbanicoPrimario;

        }
        /// <summary>
        /// Get Lista Tramos Avance Largos and Validaciones
        /// </summary>
        private List<oTramoAbanico> getLstTramosAvancesLargos(List<oTramoAbanico> iLstTramosAvancesCortos)
        {

            List<oTramoAbanico> miLstTramosAvanceLargo = new List<oTramoAbanico>();

            oTramoAvanceLargo miTramoAbanicoAvance;


            double miAijMinimo = mRoadDesign.AijMin;
            double miAijAvanceMaximo = mRoadDesign.avanceMax;


            List<oTramoAbanico> miLstTramosPrevios;
            double? miDistanciaOrigenAbanicoToP2 = null;
            double? miDistanciaP2ToPtoTarget = null;
            bool? miTramoIsValido = null;


            foreach (var item in iLstTramosAvancesCortos)
            {

                if (item.isTramoValido)
                {
                    miTramoIsValido = item.isTramoValido;
                    miDistanciaP2ToPtoTarget = item.P2.distTo2d(mPtoTarget);
                    miDistanciaOrigenAbanicoToP2 = item.longitud2d;

                    miLstTramosPrevios = item.lstTramos;

                    while (miTramoIsValido.Value && miDistanciaP2ToPtoTarget.Value > miAijMinimo && miDistanciaOrigenAbanicoToP2.Value < miAijAvanceMaximo - miAijMinimo)
                    {

                        miTramoAbanicoAvance = new oTramoAvanceLargo(miLstTramosPrevios, mRoadDesign.AijMin);

                        //Valido los Nuevo Tramos
                        miTramoAbanicoAvance.validarTramoP2InsideTerreno();
                        miTramoAbanicoAvance.validarTramoP2NearBordesTerreno();
                        miTramoAbanicoAvance.validarTramoZonasNoPaso();
                        miTramoAbanicoAvance.validarCruceDPH(mEstudioDatos);
                        //miTramoAbanicoAvance.validarCruceInf(mEstudioDatos);
                        miTramoAbanicoAvance.validarDentroDPH(mEstudioDatos);

                        //añadir validaciones nuevos errores 

                        //Add Coleccion
                        miLstTramosAvanceLargo.Add(miTramoAbanicoAvance);

                        //Condiciones  Salir Tramo
                        miDistanciaP2ToPtoTarget = miTramoAbanicoAvance.P2.distTo2d(mPtoTarget);

                        miDistanciaOrigenAbanicoToP2 = miTramoAbanicoAvance.distanciaOrigenAbanicoToP2;

                        //Obtengo el Listado de Tramos
                        miLstTramosPrevios = miTramoAbanicoAvance.lstTramos;

                        //Tramo es Valido
                        miTramoIsValido = miTramoAbanicoAvance.isTramoValido;

                    }

                }

            }


            return miLstTramosAvanceLargo;

        }
        /// <summary>
        ///Get Lista Tramos AijMinimoMinimo
        /// </summary>
        private List<oTramoAbanico> getLstTramosAijMinimoMinimo()
        {

            //Diccionario Posicion Tramo (Key)  - AzimutGrados
            Dictionary<int, double> miDicIdTramoAzimutGrados = mAbanicoDesign.getLstAzimutGrados(mTramoPrevio.P2.convertTo2d(), mPtoTarget);

            double? miAngRadianesTramoPrevioTramo = null;

            List<oTramoAbanico> miLstTramos = new List<oTramoAbanico>();

            List<oTramoAijMinimoMinimo> miLstTramoByAzimut;


            //Creo los Tramos Abanico Primario
            foreach (var item in miDicIdTramoAzimutGrados)
            {

                double miAngulo = Math.Abs(mTramoPrevio.azimutGrados - item.Value);
                if (miAngulo > 180) miAngulo = 360 - miAngulo;
                if (miAngulo > 178) continue;

                miAngRadianesTramoPrevioTramo = mTramoPrevio.getAnguloConTramoNext(item.Value, eAng.radianes);

                miLstTramoByAzimut = getLstTramosAijMinMinByAzimut(item.Key, item.Value, miAngRadianesTramoPrevioTramo.Value);

                if (miLstTramoByAzimut.Count > 0)
                {
                    miLstTramos.AddRange(miLstTramoByAzimut);
                }
            }


            return miLstTramos;
        }
        /// <summary>
        /// Get Lista de Tramos AijMinimoMinimo por Azimut
        /// </summary>
        private List<oTramoAijMinimoMinimo> getLstTramosAijMinMinByAzimut(int iIdTramoPosicion, double iAzimutGrados, double iAngRadianesTramoPrevio)
        {



            double miAijMin = mRoadDesign.AijMin;
            double miAijMinMinByRadio = mRoadDesign.AijMinimoMinimoByRadio;
            double miAijMinMinByAngulo = mRoadDesign.getAijMinimoMinimoByAngulo(iAngRadianesTramoPrevio, eAng.radianes);
            double miAijIncremento = mRoadDesign.Rp * 0.05;

            double? miAijMinMinCalculo = null;

            //Tomamos el Mayor de los 2

            if (miAijMinMinByRadio >= miAijMinMinByAngulo)
            {
                miAijMinMinCalculo = miAijMinMinByRadio;
            }
            else
            {
                miAijMinMinCalculo = miAijMinMinByAngulo;
            }


            List<oTramoAijMinimoMinimo> miLstTramos = new List<oTramoAijMinimoMinimo>();
            oTramoAijMinimoMinimo miTramo;

            while (miAijMinMinCalculo < miAijMin)
            {

                miTramo = new oTramoAijMinimoMinimo(mIdAbanico.Value, iIdTramoPosicion, mTramoPrevio, iAzimutGrados, miAijMinMinCalculo.Value, mPtoTarget);

                miLstTramos.Add(miTramo);

                miAijMinMinCalculo = miAijMinMinCalculo + miAijIncremento;
            }


            return miLstTramos;

        }

        #endregion
    }
}
