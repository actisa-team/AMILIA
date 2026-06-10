using System;
using System.Collections.Generic;
using System.Linq;
using tayLogicaTijera.data;

namespace tayLogicaTijera.EjeBasico
{

    using tadLayShare.puntos;
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayLogica;
    using tadLayLogica.estudioTipo;
    using tadLayLogica.datos.proyecto;
    using tadLayShare;
    using Autodesk.AutoCAD.DatabaseServices;
    using tadLayLogica.zonaGis;
    using EjeDeTrazado.puntosDelEje;
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    using EjeDeTrazado.componentes;
    using tadLayLan;
    using tadLayLan.Tdi;
    using tadLayLogica.Comandos;
    using System.Diagnostics;


    public class oEjeBasicoFerrocarril
    {
        private string KSolPrimaria = strFrmSolucion.uiNomSolPrimaria;
        private string KSolEnvolventeMax = strFrmSolucion.uiNomEnvMaxima;
        private string KSolEnvolventeMin = strFrmSolucion.uiNomEnvMinima;


        public oP3dSalidaLlegada ptoSalida { get; private set; }
        public oP3dSalidaLlegada ptoLlegada { get; private set; }

        public oTramoEjeBasico tramoSalida { get; private set; }
        public oTramoEjeBasico tramoLlegada { get; private set; }


        public oRoadDes roadDesign = null;
        public oRoadPendientes roadPendientes = null;

        public oTramosValoracion tramosValoracion = null;

        public oAbanicoDesign abanicoDesign = null;
        public IEstudio estudioData = null;
        public eEstudioTipo? estudioTipo = null;
        public oCoeMinoracionAlturasMaximas coeMinoracionAlturasMaximas = null;

        private string mSolucionNombre = string.Empty;

        private double mRc = 0;
        private double mDistEntronque;
        private double mDistConvergencia;
        private double mRadioEntronque = 0;

        public List<oTijera> miLstTijeraDsdInicio = new List<oTijera>();
        public List<oTijera> miLstTijeraDsdFin = new List<oTijera>();
        oEntronqueFerrocarril mEntronque = null;
        private bool mIsEnvolvente = false;
        private oTarget.eFiltro mEnvolventeTipo = oTarget.eFiltro.MaxMin;
        private double miDistanciaP2ToPtoEntronqueTotal = 0;
        private double miDistEntronquePU = 0;

        //Trazado
        public List<Componente> mListaComponentesTrazado = new List<Componente>();
        public List<Vertice> mListaVerticesTrazado = new List<Vertice>();

        //Eje básico
        Polyline mEjeVisibilidad = null;
        private bool mPenalizarTramosCortosEntronque = false;

        private double mCoefDistTramo;
        private double mCoefDistEje;



        public oEjeBasicoFerrocarril(string iSolucionPrefijo,
                         bool iGenerarEnvolventeMaximaMinima,
                         oRoadDes iRoadDesign,
                         oRoadPendientes iRoadPendientes,
                         oCoeMinoracionAlturasMaximas iCoeMinAlturasMaximas,
                         oTramosValoracion iTramosValoracion,
                         oAbanicoDesign iAbanicoDesign,
                         IEstudio iEstudio, eEstudioTipo iEstudioTipo, double iDistEntronque, double iDistConvergencia, bool iPenalizarTramosCortosEntronque)
        {

            this.solucionPrefijo = iSolucionPrefijo;

            roadDesign = iRoadDesign;
            roadPendientes = iRoadPendientes;
            coeMinoracionAlturasMaximas = iCoeMinAlturasMaximas;
            tramosValoracion = iTramosValoracion;
            abanicoDesign = iAbanicoDesign;
            estudioData = iEstudio;

            //punto de Salida
            ptoSalida = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoSalida);
            //Punto de Llegada
            ptoLlegada = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoLlegada);
            
            //Calculamos los datos comunes para todos los tramos 
            this.mRc = roadDesign.Rp;
            this.estudioTipo = iEstudioTipo;
            mDistEntronque = iDistEntronque;
            mDistConvergencia = iDistConvergencia;
            mPenalizarTramosCortosEntronque = iPenalizarTramosCortosEntronque;
            mRadioEntronque = mRc;
            var isCurvaGranRadio = false;
            if (iAbanicoDesign.isCurvaGranRadio) mRadioEntronque = iAbanicoDesign.granRadio;
            else
            {
                if (roadDesign.grupo == eRoadGrupo.Grupo1)
                    mRadioEntronque = Math.Max(700, mRc);
                else
                    mRadioEntronque = 2*mRc;
            }

        }




        #region "Propiedades"
        public string solucionPrefijo
        {
            get
            {
                if (string.IsNullOrEmpty(mSolucionNombre))
                {
                    throw new oExPropertieNullValue("NombreSolución");
                }

                return mSolucionNombre;
            }

            private set
            {
                mSolucionNombre = value;

            }

        }

        public string solucionPrimariaNombre
        {
            get
            {
                return string.Format(mSolucionNombre + "_" + this.KSolPrimaria);
            }
        }
        public string solucionEnvolventeMaximaNombre
        {
            get
            {
                return string.Format(mSolucionNombre + "_" + this.KSolEnvolventeMax);
            }
        }
        public string solucionEnvolventeMinimaNombre
        {
            get
            {
                return string.Format(mSolucionNombre + "_" + this.KSolEnvolventeMin);
            }
        }
        #endregion

        
        #region "Metodos publicos"
        public Polyline createEjesBasicos(Polyline iEjeVisibilidad,oEjeTijeraData tijeraData,oTarget.eFiltro miEnv,bool dibujar=true)
        {
            mCoefDistEje = tijeraData.CoefDistEje;
            mCoefDistTramo = tijeraData.CoefDistTramo;
            mIsEnvolvente = tijeraData.IsEnvolvente;
            mEnvolventeTipo = miEnv;
          
            //Activo las Capas
            oTadil.data.Layer.zonaNoPasoUsuario.On();
            oTadil.data.Layer.zonaNoPasoPendiente.On();
            oTadil.data.Layer.zonasGisOn();

            //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
            oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();

            roadDesign.AijMinSalidaLlegada = CalculateMinLongitudEs();

            //Creamos el Tramo de Salida
            tramoSalida = oFactoryTramoSalidaLlegada.createTramoSalidaLlegadaTijera(true, 0, 0, ptoSalida, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion, false);
            //Creamoe el Tramo de Llegada
            tramoLlegada = oFactoryTramoSalidaLlegada.createTramoLlegadaTijera(ptoLlegada, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion);
            

            //Seleccionar Polilinea Eje Visibilidad
            mEjeVisibilidad = iEjeVisibilidad;

            //Creo el Objeto Target
            oTarget miTargetSalida = new oTarget(ptoSalida, ptoLlegada, roadDesign.AijMinSalidaLlegada, 0);
            oTarget miTargetLlegada = new oTarget(ptoLlegada, ptoSalida, roadDesign.AijMinSalidaLlegada, 0); 

            if (mIsEnvolvente)
            {
                miTargetSalida.setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(mEjeVisibilidad, mEnvolventeTipo);
                miTargetLlegada.setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(oLw.reverseLw(mEjeVisibilidad), mEnvolventeTipo);
                miDistEntronquePU = tijeraData.DistEntronquePC / 100;
            }
            else
            {
                miTargetSalida.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(mEjeVisibilidad);
                miTargetLlegada.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(oLw.reverseLw(mEjeVisibilidad));
                miDistEntronquePU = 0.4;
            }


            //SetUpAbanico
            oTijera.SetUpObject(roadDesign, roadPendientes, abanicoDesign, estudioData, this.ptoLlegada, tijeraData.CoefPrioRectas);
            oAbanicoByPoint.showInfoTramos();

            #region "Calculamos solución"

            bool ejeCreado = false;
            int numIntentosEje = 1;
            try
            {
                ejeCreado = generarEjeBasico(miTargetSalida, miTargetLlegada, dibujar);



                if (tijeraData.IsAutocorrecion)
                {
                    while (!ejeCreado && numIntentosEje <= tijeraData.NumAutocorrecion)
                    {
                        
                        double distanciaTotal = ptoSalida.distTo2d(ptoLlegada);
                        double distanciaDesdeIni = distanciaTotal;
                        double distanciaDesdeFin = distanciaTotal;
                        if (miLstTijeraDsdInicio.Count != 0)
                        {
                            distanciaDesdeIni = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P2.distTo2d(ptoLlegada);
                        }
                        if (miLstTijeraDsdFin.Count != 0)
                        {
                            distanciaDesdeFin = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P2.distTo2d(ptoSalida);
                        }

                        numIntentosEje++;
                        if (tijeraData.RecalcularEjeVisibilidad)
                        {
                            if ((distanciaDesdeIni > (0.6 * distanciaTotal)) || (distanciaDesdeFin > (0.6 * distanciaTotal)))
                            {
                                mEjeVisibilidad = oComandoEjeVisibilidad.createAutomatico();
                            }
                        }
                        miTargetSalida.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(mEjeVisibilidad);
                        miTargetLlegada.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(oLw.reverseLw(mEjeVisibilidad));


                        oTijera.SetUpObject(roadDesign, roadPendientes, abanicoDesign, estudioData, this.ptoLlegada, tijeraData.CoefPrioRectas);
                        ejeCreado = this.generarEjeBasico(miTargetSalida, miTargetLlegada,dibujar);



                    }
                }
            }
            catch (oExPropertieNullValue e)
            {
                if (e.Message.Contains("No Existe solucion"))
                {
                    numIntentosEje = tijeraData.NumAutocorrecion + 1;
                }
                else
                {
                    throw e;
                }
            }

            if (!ejeCreado)
            {
                //no se ha encontrado solucion
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionPrimariaNotFound);
                return null;
            }
            else
            {
                string nombreSolucion = this.solucionPrimariaNombre;
                if (mIsEnvolvente && mEnvolventeTipo == oTarget.eFiltro.Max) nombreSolucion = this.solucionEnvolventeMaximaNombre;
                if (mIsEnvolvente && mEnvolventeTipo == oTarget.eFiltro.Min) nombreSolucion = this.solucionEnvolventeMinimaNombre;
                Polyline3d miLwEjeBasico3D = null;
                Polyline miLwEjeBasico2D = null;

                oCadManager.thisEditor.UpdateScreen();
                crearEjeTrazado();
                //Point3dCollection miLwEjeBasico3D = crearEjeBasico();
                oCadManager.thisEditor.UpdateScreen();
                oLstTramosGanadores miEjeBasicoPrimario = generarTramosGanadores();
                oCadManager.thisEditor.UpdateScreen();

                //Dibujo el Eje Basico Polilinea 3D
                miLwEjeBasico3D = miEjeBasicoPrimario.drawEjeBasico3D(nombreSolucion);

                //Dibujo el Eje Basico en Planta Polilinea Z=0
                miLwEjeBasico2D = miEjeBasicoPrimario.drawEjeBasicoPlantaFerrocarril(miLwEjeBasico3D, nombreSolucion);

                //Añado el Xdata al Eje 2D
                oTadilXdata.addXdataRoadDesign(miLwEjeBasico2D, roadDesign);

                //Guardo los Datos en la Base Datos
                Guid miIdSolucion = oDalTbSolucion.addEjeBasico(nombreSolucion, miLwEjeBasico3D.Handle.ToString(), miLwEjeBasico2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);


                oComandoEjeTrazadoTadil.createFerrocarril((eEstudioTipo)estudioTipo, miIdSolucion, this.mListaComponentesTrazado, this.mListaVerticesTrazado);

                return miLwEjeBasico2D;
               
            }


            #endregion
        
        }

        private double CalculateMinLongitudEs()
        {
            var min = (1.05*Math.Max(roadDesign.kvConcavo,roadDesign.kvConvexo)*(roadPendientes.calzadaPendienteCalculoMaximoPC/100)) + (roadDesign.Vp/4);
            return min;
        }

        #endregion


        private bool generarEjeBasico(oTarget iTargetSalida, oTarget iTargetLlegada, bool dibujar=true)
        {

            #region "SetUp"

            //Activo las Capas
            oTadil.data.Layer.zonaNoPasoUsuario.On();
            oTadil.data.Layer.zonaNoPasoPendiente.On();
            oTadil.data.Layer.zonasGisOn();

            //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
            oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();

            iTargetSalida.toleranciaSigPunto = 1.2 * mRc;
            iTargetLlegada.toleranciaSigPunto = 1.2 * mRc;

            int miIdTijera = 1;
            oP3d miPtoEntronque;
            oP3d miPtoSalida;
            var isLastTurnInicio = true;

            SetUpTramosES(iTargetSalida, iTargetLlegada, out miPtoEntronque, out miPtoSalida, dibujar);

            #endregion

            oTramoEjeBasico miTramoPrevioSalida;
            oTramoEjeBasico miTramoPrevioLlegada;
            if (miLstTijeraDsdInicio.Count == 0)
            {
                miTramoPrevioSalida = this.tramoSalida;
                miTramoPrevioSalida.mPendienteUltimoTramo = (tramoSalida.P2.Z - tramoSalida.P1.Z) / tramoSalida.longitud2d;
                miTramoPrevioSalida.mLongitudUltimoTramo = tramoSalida.longitud2d;
            }
            else
            {
                miTramoPrevioSalida = getTramoEjeBasico(miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador);
                if (miIdTijera < miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.mIdTijera) miIdTijera = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.mIdTijera+1;
            }
            if (miLstTijeraDsdFin.Count == 0)
            {
                miTramoPrevioLlegada = this.tramoLlegada;
                miTramoPrevioLlegada.mPendienteUltimoTramo = (miTramoPrevioLlegada.P2.Z - miTramoPrevioLlegada.P1.Z) / miTramoPrevioLlegada.longitud2d;
                miTramoPrevioLlegada.mLongitudUltimoTramo = tramoLlegada.longitud2d;
            }
            else
            {
                miTramoPrevioLlegada = getTramoEjeBasico(miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador);
                if (miIdTijera < miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.mIdTijera) miIdTijera = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.mIdTijera+1;
            }



            try
            {

                double miDistanciaP2ToPtoEntronqueDesdeInicio = miTramoPrevioSalida.P2.distTo2d(miPtoEntronque);
                double miDistanciaP2ToPtoEntronqueDesdeFin = miTramoPrevioLlegada.P2.distTo2d(miPtoSalida);
                miDistanciaP2ToPtoEntronqueTotal = miPtoSalida.distTo2d(miPtoEntronque);
                double miDistanciaRecorridaRamaIni = miTramoPrevioSalida.P2.distTo2d(miPtoSalida);
                double miDistanciaRecorridaRamaFin = miTramoPrevioLlegada.P2.distTo2d(miPtoEntronque);

                bool isListIniSobrepasado = (miDistanciaRecorridaRamaIni >= mDistConvergencia);
                bool isListFinSobrepasado = (miDistanciaRecorridaRamaFin >= mDistConvergencia);
                bool hasSolucion = true;
                bool entronqueObligado = false;


                double distanciaEndRama = (1 - miDistEntronquePU) * miDistanciaP2ToPtoEntronqueTotal;

                while ((!isListIniSobrepasado || !isListFinSobrepasado)&& !entronqueObligado)
                {
                    if (!isListIniSobrepasado && !entronqueObligado)
                    {
                        isLastTurnInicio = true;
                        hasSolucion = generaTijeraFrontToEnd(ref miIdTijera,
                                ref miLstTijeraDsdInicio, ref miDistanciaP2ToPtoEntronqueDesdeInicio, iTargetSalida,
                                ref miTramoPrevioSalida, miPtoEntronque, "INI", miTramoPrevioLlegada.P2,dibujar);

                            if (!hasSolucion)
                            {
                                return false;
                            }
                    }

                    if (miLstTijeraDsdInicio.Count > 0 && miLstTijeraDsdFin.Count > 0)
                    {
                        var miDistEntreObjetivos1 = MinimaDistanciaEntronque(isLastTurnInicio);
                        double distAvanceSalidaIni1 =
                            miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P2.distTo2d(ptoSalida);
                        double distAvanceSalidaFin1 =
                            miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P2.distTo2d(ptoSalida) - mRc;

                        if (distAvanceSalidaIni1 > distAvanceSalidaFin1 || miDistEntreObjetivos1 <= mDistEntronque)
                        {
                            entronqueObligado = true;
                        }
                    }


                    if (!isListFinSobrepasado && !entronqueObligado)
                    {
                        isLastTurnInicio = false;
                        hasSolucion = generaTijeraFrontToEnd(ref miIdTijera,
                                ref miLstTijeraDsdFin, ref miDistanciaP2ToPtoEntronqueDesdeFin, iTargetLlegada,
                                ref miTramoPrevioLlegada, miPtoSalida, "FIN", miTramoPrevioSalida.P2,dibujar);

                        if (!hasSolucion)
                        {
                            return false;
                        }
                    }

                    //nueva comprobacion

                    var miDistEntreObjetivos =  MinimaDistanciaEntronque(isLastTurnInicio);

                    miDistanciaRecorridaRamaIni = miTramoPrevioSalida.P2.distTo2d(miPtoSalida);
                    miDistanciaRecorridaRamaFin = miTramoPrevioLlegada.P2.distTo2d(miPtoEntronque);


                    isListIniSobrepasado = ((miDistanciaRecorridaRamaIni >= mDistConvergencia)|| (mIsEnvolvente && mEnvolventeTipo == oTarget.eFiltro.Min && miDistanciaP2ToPtoEntronqueDesdeInicio < distanciaEndRama));
                    isListFinSobrepasado = ((miDistanciaRecorridaRamaFin >= mDistConvergencia) || (mIsEnvolvente && mEnvolventeTipo == oTarget.eFiltro.Max && miDistanciaP2ToPtoEntronqueDesdeInicio < distanciaEndRama));


                    var distAvanceSalidaIni = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P2.distTo2d(ptoSalida);
                    var distAvanceSalidaFin = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P2.distTo2d(ptoSalida) - mRc;


                    if (distAvanceSalidaIni > distAvanceSalidaFin || miDistEntreObjetivos <= mDistEntronque)
                    {
                        entronqueObligado = true;
                    }


                }



                if (!entronqueObligado)
                {
                    bool isNextIni;
                    hasSolucion = generarTijerarFrontToFront(ref miIdTijera, distanciaEndRama, out isNextIni,dibujar);
                    isLastTurnInicio = !isNextIni;
                    if (!hasSolucion)
                    {
                        return false;
                    }
                }


                #region "Entronque y autocorreccion"


                return generarEntronque(miIdTijera, isLastTurnInicio);
                
                #endregion


            }
            finally
            {
                oAbanicoByPoint.showInfoTramosUnload();
            }

        }

        private void SetUpTramosES(oTarget iTargetSalida, oTarget iTargetLlegada, out oP3d miPtoEntronque,
            out oP3d miPtoSalida,bool dibujar)
        {
            var hasSolucion = true;
            var miIdTijera = 0;
            miPtoSalida = new oP3d(ptoSalida.X, ptoSalida.Y, ptoSalida.Z);
            var miDistanciaP2ToPtoEntronqueDesdeInicio = mEjeVisibilidad.Length;
            miPtoEntronque = new oP3d(ptoLlegada.X, ptoLlegada.Y, ptoLlegada.Z);
            
            if (this.tramoSalida == null)
            {
                if (miLstTijeraDsdInicio.Count == 0)
                {
                    if (ptoSalida.pendientePC == null)
                    {
                        var rectaMinima = 0.0;
                        if (ptoSalida.longitud != null)
                        {
                            rectaMinima = ptoSalida.longitud.Value;
                        }
                        var miTramosValoracion = tramosValoracion;

                        var miTijera = new oTijera(miIdTijera, ptoSalida, ptoSalida.azimutGrados.Value, null, iTargetSalida, mRc, roadDesign.Vp, (abanicoDesign.abanicoTipo == eAbanicoTipo.largos), false,
                            mEjeVisibilidad, mCoefDistTramo, mCoefDistEje, dibujar);

                        miTijera.calcularTramosGanadores(miTramosValoracion, false, null, false, null, null, rectaMinima);
                        miTijera.drawTijeraTramos(oTadil.data.Layer.abanicoTramos.name, "INI");

                        if (miTijera.hasTramosGanadores)
                            miLstTijeraDsdInicio.Add(miTijera);


                    }
                    else
                    {
                        var miTramosValoracion = tramosValoracion;
                        var miTijera = new oTijera(miIdTijera, ptoSalida, ptoSalida.azimutGrados.Value, null, iTargetSalida, mRc, roadDesign.Vp, (abanicoDesign.abanicoTipo == eAbanicoTipo.largos), false,
                            mEjeVisibilidad, mCoefDistTramo, mCoefDistEje, dibujar);
                        miTijera.calcularTramosGanadoresPendienteObligada(miTramosValoracion, false, null, false, null, null, ptoSalida.pendientePC.Value);
                        miTijera.drawTijeraTramos(oTadil.data.Layer.abanicoTramos.name, "INI");

                        if (miTijera.hasTramosGanadores)
                            miLstTijeraDsdInicio.Add(miTijera);

                    }
                }
            }
            else
            {
                /*
                 * Se comenta para ver velocidad ** juanma **
                 */
                if (dibujar)
                {
                    this.tramoSalida.drawTramo3D(oTadil.data.Layer.abanicoTramos.name);
                }
                miPtoSalida = this.tramoSalida.P2;
            }


            if (this.tramoLlegada == null)
            {
                if (ptoLlegada.pendientePC == null)
                {
                    if (miLstTijeraDsdFin.Count == 0)
                    {

                        var rectaMinima = 0.0;
                        if (ptoLlegada.longitud != null)
                        {
                            rectaMinima = ptoSalida.longitud.Value;
                        }

                        var miTramosValoracion = tramosValoracion;
                        var miTijera = new oTijera(miIdTijera, ptoLlegada, ptoLlegada.azimutGrados.Value, null, iTargetLlegada, mRc, roadDesign.Vp, (abanicoDesign.abanicoTipo == eAbanicoTipo.largos), true,
                            mEjeVisibilidad, mCoefDistTramo, mCoefDistEje,dibujar);
                        miTijera.calcularTramosGanadores(miTramosValoracion, false, null, false, null, null, rectaMinima);
                        miTijera.drawTijeraTramos(oTadil.data.Layer.abanicoTramos.name, "FIN");

                        if (miTijera.hasTramosGanadores)
                            miLstTijeraDsdFin.Add(miTijera);
                    }

                }
                else
                {
                    var miTramosValoracion = tramosValoracion;
                    var miTijera = new oTijera(miIdTijera, ptoLlegada, ptoLlegada.azimutGrados.Value, null, iTargetLlegada, mRc, roadDesign.Vp, (abanicoDesign.abanicoTipo == eAbanicoTipo.largos), true,
                        mEjeVisibilidad, mCoefDistTramo, mCoefDistEje, dibujar);
                    miTijera.calcularTramosGanadoresPendienteObligada(miTramosValoracion, false, null, false, null, null, (ptoLlegada.pendientePC.Value * -1));
                    miTijera.drawTijeraTramos(oTadil.data.Layer.abanicoTramos.name, "FIN");

                    if (miTijera.hasTramosGanadores)
                        miLstTijeraDsdFin.Add(miTijera);

                }
            }
            else
            {
                miPtoEntronque = tramoLlegada.P2;
            }


            iTargetSalida.setPuntoDeEntronque(new oP2d(miPtoEntronque.X, miPtoEntronque.Y));
            iTargetLlegada.setPuntoDeEntronque(new oP2d(miPtoSalida.X, miPtoSalida.Y));

        }


        private bool generaTijeraFrontToEnd(ref int miIdTijera, ref List<oTijera> iLstTijera, ref double iDist, oTarget iTarget, 
            ref oTramoEjeBasico iTramoPrevio, oP3d iPtoEntronque, string iXData, oP3d miPuntoOpuesto,bool dibujar)
        {
            bool isTijeraIni = (iXData.Equals("INI"));
            miDistanciaP2ToPtoEntronqueTotal = ptoLlegada.distTo2d(ptoSalida);
            int numeroTijeras = iLstTijera.Count;
            bool retroceso = false;
            var tijeraGenerada = false;
            do
            {
                //UpdateUI
                System.Windows.Forms.Application.DoEvents();
                
                double miPendienteAnterior = iTramoPrevio.mPendienteUltimoTramo;
                double miLongAnterior = iTramoPrevio.mLongitudUltimoTramo;

                bool tijeraEliminada = crearTijeraEjeBasico(ref retroceso, null, iTarget, ref iDist, iLstTijera, ref miIdTijera, 
                    ref iTramoPrevio, iPtoEntronque, !isTijeraIni, ref miPendienteAnterior, iXData, miLongAnterior,dibujar);
                if (tijeraEliminada)
                {
                    if (iLstTijera.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        throw new oExPropertieNullValue("No Existe solucion");
                    }
                }

                if ((iLstTijera.Count > 0))
                {
                    if (!iLstTijera[iLstTijera.Count - 1].tramoGanador.comprobarCompatibilidadTrazado(miPuntoOpuesto, roadPendientes.calzadaPendienteCalculoMaximoPC/100)  &&(!(iDist < 0.8 * miDistanciaP2ToPtoEntronqueTotal)))
                    {
                        retroceso = true;
                    }
                }
                else
                {
                    throw new oExPropertieNullValue("No Existe solucion");
                }
                tijeraGenerada = (iLstTijera.Count > numeroTijeras);
            }
            while (!tijeraGenerada);
            return true;
        }
        
        private bool generarTijerarFrontToFront(ref int miIdTijera, double distanciaEndRama, out bool nextTurnIni,bool dibujar)
        {
            #region "WHILE una tijera por cada lado"

            bool retrocesoIni = false;
            bool retrocesoFin = false;

            bool isTurnoInicio = true;
            nextTurnIni = true;

            oP2d miPtoTarget;

            oP3d miUltimoPtoDesdeInicio = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P2;
            oP3d miUltimoPtoDesdeFin = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P2;

            oTramoEjeBasico miTramoPrevioSalida = getTramoEjeBasico(miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador);
            oTramoEjeBasico miTramoPrevioLlegada = getTramoEjeBasico(miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador);
            double miPendienteAnteriorInicio = miTramoPrevioSalida.mPendienteUltimoTramo;
            double miPendienteAnteriorFin = miTramoPrevioLlegada.mPendienteUltimoTramo; 
            double miDistEntreObjetivos = MinimaDistanciaEntronque(isTurnoInicio);

            double miDistanciaP2ToPtoEntronqueDesdeInicio = miUltimoPtoDesdeInicio.distTo2d(ptoLlegada);
            double miDistanciaP2ToPtoEntronqueDesdeFin = miUltimoPtoDesdeFin.distTo2d(ptoSalida);


            int numListTijeraInicio = miLstTijeraDsdInicio.Count;
            int numListTijeraFin = miLstTijeraDsdFin.Count;

            bool entronqueObligado = false;

            //Comprobar cotas
            while ((miDistEntreObjetivos > mDistEntronque && !entronqueObligado))
            {

                if (isTurnoInicio && (miDistEntreObjetivos > mDistEntronque))
                {
                    if (!(mIsEnvolvente && mEnvolventeTipo == oTarget.eFiltro.Min && miDistanciaP2ToPtoEntronqueDesdeInicio < distanciaEndRama))
                    {

                        miPtoTarget = new oP2d(miUltimoPtoDesdeFin.X, miUltimoPtoDesdeFin.Y);
                        bool tijeraEliminada = crearTijeraEjeBasico(ref retrocesoIni, miPtoTarget, null, ref miDistanciaP2ToPtoEntronqueDesdeInicio,
                            miLstTijeraDsdInicio, ref miIdTijera, ref miTramoPrevioSalida, ptoLlegada, false, ref miPendienteAnteriorInicio, "INI",
                            miTramoPrevioSalida.mLongitudUltimoTramo,dibujar);
                        if (tijeraEliminada)
                        {
                            return false;
                        }

                        if (!retrocesoIni)
                        {
                            if (miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.comprobarCompatibilidadTrazado(miUltimoPtoDesdeFin, roadPendientes.calzadaPendienteCalculoMaximoPC/100))
                            {
                                double distAvanceSalidaIni = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P2.distTo2d(ptoSalida);
                                double distAvanceSalidaFin = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P2.distTo2d(ptoSalida) - mRc;

                                if (distAvanceSalidaIni > distAvanceSalidaFin)
                                {
                                    entronqueObligado = true;
                                    this.eliminarUltimaTijera(miLstTijeraDsdInicio);
                                }

                                //recalcular distancia entre objetivos

                                miUltimoPtoDesdeInicio = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P2;
                                miDistEntreObjetivos = MinimaDistanciaEntronque(isTurnoInicio);

                                if (numListTijeraInicio < miLstTijeraDsdInicio.Count)
                                {
                                    isTurnoInicio = false;
                                    numListTijeraInicio = miLstTijeraDsdInicio.Count;
                                }
                            }
                            else
                            {
                                retrocesoIni = true;
                            }
                        }
                    }
                    else
                    {
                        isTurnoInicio = false;
                    }
                }
                else if (miDistEntreObjetivos > mDistEntronque)
                {
                    if (!(mIsEnvolvente && mEnvolventeTipo == oTarget.eFiltro.Max && miDistanciaP2ToPtoEntronqueDesdeInicio < distanciaEndRama))
                    {
                        miPtoTarget = new oP2d(miUltimoPtoDesdeInicio.X, miUltimoPtoDesdeInicio.Y);
                        bool tijeraEliminada = crearTijeraEjeBasico(ref retrocesoFin, miPtoTarget, null, 
                            ref miDistanciaP2ToPtoEntronqueDesdeFin, miLstTijeraDsdFin, ref miIdTijera, 
                            ref miTramoPrevioLlegada, ptoSalida, true, ref miPendienteAnteriorFin, "FIN", miTramoPrevioLlegada.mLongitudUltimoTramo,dibujar);
                        if (tijeraEliminada)
                        {
                            return false;
                        }

                        if (!retrocesoFin)
                        {

                            if (miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.comprobarCompatibilidadTrazado(miUltimoPtoDesdeInicio, roadPendientes.calzadaPendienteCalculoMaximoPC/100))
                            {

                                double distAvanceSalidaIni = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P2.distTo2d(ptoSalida);
                                double distAvanceSalidaFin = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P2.distTo2d(ptoSalida) - mRc;

                                if (distAvanceSalidaIni > distAvanceSalidaFin)
                                {
                                    entronqueObligado = true;
                                    this.eliminarUltimaTijera(miLstTijeraDsdFin);
                                }

                                //recalcular distancia entre objetivos
                                miUltimoPtoDesdeFin = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P2;
                                miDistEntreObjetivos = MinimaDistanciaEntronque(isTurnoInicio);

                                if (numListTijeraFin < miLstTijeraDsdFin.Count)
                                {
                                    isTurnoInicio = true;
                                    numListTijeraFin = miLstTijeraDsdFin.Count;
                                }
                            }
                            else
                            {
                                retrocesoFin = true;
                            }
                        }
                    }
                    else
                    {
                        isTurnoInicio = true;
                    }
                }
                nextTurnIni = isTurnoInicio;

            }
            return true;
        }

        private double MinimaDistanciaEntronque(bool isLastTramoIni)
        {
            oTijera miTijera = null;
            var ultimoPuntoIniP2 = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P2;
            var ultimoPuntoFinP2 = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P2;
            var ultimoPuntoIniP1 = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.P1;
            var ultimoPuntoFinP1 = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.P1;

            var distancia1 = double.MaxValue;
            var distancia2 = double.MaxValue;
            var distancia3 = double.MaxValue;
            var distancia4 = double.MaxValue;

            if (isLastTramoIni)
            {
                if (miLstTijeraDsdInicio.Count > 1)
                {
                    miTijera = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 2];
                    var tramoPrevio = getTramoEjeBasico(miTijera.tramoGanador);
                    var puntoRectaMasLarga = PuntoFinalRectaMasLarga(tramoPrevio);
                    distancia3 = puntoRectaMasLarga.distTo2d(ultimoPuntoFinP2);
                    distancia4 = puntoRectaMasLarga.distTo2d(ultimoPuntoFinP1);
                }
                distancia1 = ultimoPuntoIniP2.distTo2d(ultimoPuntoFinP2);
                distancia2 = ultimoPuntoIniP2.distTo2d(ultimoPuntoFinP1);
            }
            else
            {
                if (miLstTijeraDsdFin.Count > 1)
                {
                    miTijera = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 2];
                    var tramoPrevio = getTramoEjeBasico(miTijera.tramoGanador);
                    var puntoRectaMasLarga = PuntoFinalRectaMasLarga(tramoPrevio);
                    distancia3 = puntoRectaMasLarga.distTo2d(ultimoPuntoIniP2);
                    distancia4 = puntoRectaMasLarga.distTo2d(ultimoPuntoIniP1);
                }
                distancia1 = ultimoPuntoFinP2.distTo2d(ultimoPuntoIniP2);
                distancia2 = ultimoPuntoFinP2.distTo2d(ultimoPuntoIniP1);
            }

            var distancia = Math.Min(distancia1, distancia2);
            distancia = Math.Min(distancia, distancia3);
            distancia = Math.Min(distancia, distancia4);
            return distancia;

        }

        private oP2d PuntoFinalRectaMasLarga(oTramoEjeBasico iTramoPrevio)
        {

            var azimutentrada = oTrigo.getAzimutGrados(iTramoPrevio.P1.convertTo2d(), iTramoPrevio.P2.convertTo2d());
            var point = iTramoPrevio.P2.convertTo2d();
            if (abanicoDesign.incluirTramosRectaMaxima)
            {
                point = oTrigo.getP2FromAzimutLon(iTramoPrevio.P2.convertTo2d(), azimutentrada,
                    oTijera.TeRectaMaxima(roadDesign.Vp));
            }
            else if (abanicoDesign.incluirTramosRectaLimitada)
            {
                point = oTrigo.getP2FromAzimutLon(iTramoPrevio.P2.convertTo2d(), azimutentrada,
                    oTijera.TeRectaLimitada(roadDesign.Vp));
            }
            else if (abanicoDesign.incluirTramosRectaMinima)
            {
                point = oTrigo.getP2FromAzimutLon(iTramoPrevio.P2.convertTo2d(), azimutentrada, oTijera.TeRectaMinima(roadDesign.Vp));
            }

            return point;
        }

        private bool generarEntronque(int miIdTijera, bool isLastIni)
        {
            var distanciaMinimaEntronque = MinimaDistanciaEntronque(isLastIni);
            if (distanciaMinimaEntronque < 0.87*mDistEntronque)
            {
                if (isLastIni)
                {
                    miLstTijeraDsdInicio.RemoveAt(miLstTijeraDsdInicio.Count-1);
                }
                else
                {
                    miLstTijeraDsdFin.RemoveAt(miLstTijeraDsdFin.Count - 1);
                }
            }

            var radio = mRc;
            var isCurvaGranRadio = abanicoDesign.isCurvaGranRadio;
            if (abanicoDesign.isCurvaGranRadio) radio = abanicoDesign.granRadio;
            if (roadDesign.grupo == eRoadGrupo.Grupo1 && radio >= 7500) isCurvaGranRadio = true;
            if (roadDesign.grupo == eRoadGrupo.Grupo2 && radio >= 3500) isCurvaGranRadio = true;

            oTramoTijera miUltimoTramoDesdeInicio = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador;
            oTramoTijera miUltimoTramoDesdeFin = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador;

            oP2d miP1T1 = new oP2d(miUltimoTramoDesdeInicio.mPuntoIntermedio.X, miUltimoTramoDesdeInicio.mPuntoIntermedio.Y);
            oP2d miP2T1 = new oP2d(miUltimoTramoDesdeInicio.P2.X, miUltimoTramoDesdeInicio.P2.Y);
            oP2d miP1T2 = new oP2d(miUltimoTramoDesdeFin.mPuntoIntermedio.X, miUltimoTramoDesdeFin.mPuntoIntermedio.Y);
            oP2d miP2T2 = new oP2d(miUltimoTramoDesdeFin.P2.X, miUltimoTramoDesdeFin.P2.Y);

            Stopwatch miMedicionTiempo = new Stopwatch();
            miMedicionTiempo.Start();

            var entronqueData = new oEntronqueData(miIdTijera, 0, mRadioEntronque, roadDesign.Vp, tramosValoracion, miUltimoTramoDesdeInicio.P2.Z, miUltimoTramoDesdeFin.P2.Z, isCurvaGranRadio, mCoefDistTramo, mCoefDistEje, mDistEntronque);

            mEntronque = new oEntronqueFerrocarril(miP1T1, miP2T1, miP1T2, miP2T2, mEjeVisibilidad, abanicoDesign, miMedicionTiempo, entronqueData);
            bool tijeraList1Eliminada = false;
            bool tijeraList2Eliminada = false;

            //setUp las listas de tijeras para comprobar la interseccion entre tramos
            mEntronque.setUpListasTijeras(miLstTijeraDsdInicio, miLstTijeraDsdFin, roadPendientes, abanicoDesign.tramoAbanicoDiscretizacion, estudioData, calculaPkIniEntronque());

            var ultimoTramoFDesdeini =
                miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.mTramosFerrocarril[
                    miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.mTramosFerrocarril.Count - 1];
            var pendientedesdeini = (ultimoTramoFDesdeini.P2.Z - ultimoTramoFDesdeini.P1.Z) / ultimoTramoFDesdeini.mTrazadoTramo.Length;
            var ultimoTramoFDesdefin =
                miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.mTramosFerrocarril[
                    miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.mTramosFerrocarril.Count - 1];
            var pendientedesdefin = (ultimoTramoFDesdefin.P2.Z - ultimoTramoFDesdefin.P1.Z) / ultimoTramoFDesdefin.mTrazadoTramo.Length;

            bool existEntonque = mEntronque.createEntronque(mPenalizarTramosCortosEntronque, roadDesign.Vp, pendientedesdeini, ultimoTramoFDesdeini.mTrazadoTramo.Length, pendientedesdefin, ultimoTramoFDesdefin.mTrazadoTramo.Length);

            if (existEntonque)
            {
                return true;
            }
            else
            {
                //en el caso en que no exista entronque hacer autocorrecion
                //empezamos a hacer la autocorrecion por la ultima tijera de la lista de tijeras desde incio

                int numCorrecciones = 1;
                while (!existEntonque)
                {

                    var miTijera = miLstTijeraDsdInicio.Last();
                    miTijera.tramoGanador.mIsValido = false;
                    if (miLstTijeraDsdInicio.Last().existenTramosValidos())
                    {
                        miTijera.tramoGanador = miTijera.getTramoGanador();
                    }
                    else
                    {
                        miTijera.ErrorNoDefToValid();
                        miLstTijeraDsdFin.Last().tramoGanador.mIsValido = false;
                        if (miLstTijeraDsdFin.Last().existenTramosValidos())
                            miLstTijeraDsdFin.Last().tramoGanador = miLstTijeraDsdFin.Last().getTramoGanador();
                    }

                    if (miLstTijeraDsdFin.Last().existenTramosValidos())
                    {


                        //crear el entronque
                        if (miLstTijeraDsdInicio.Count - 1 >= 0)
                        {
                            miUltimoTramoDesdeInicio = miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador;
                        }
                        else
                        {
                            return false;
                        }
                        if (miLstTijeraDsdFin.Count - 1 >= 0)
                        {
                            miUltimoTramoDesdeFin = miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador;
                        }
                        else
                        {
                            return false;
                        }

                        miP1T1 = new oP2d(miUltimoTramoDesdeInicio.mPuntoIntermedio.X,
                            miUltimoTramoDesdeInicio.mPuntoIntermedio.Y);
                        miP2T1 = new oP2d(miUltimoTramoDesdeInicio.P2.X, miUltimoTramoDesdeInicio.P2.Y);
                        miP1T2 = new oP2d(miUltimoTramoDesdeFin.mPuntoIntermedio.X,
                            miUltimoTramoDesdeFin.mPuntoIntermedio.Y);
                        miP2T2 = new oP2d(miUltimoTramoDesdeFin.P2.X, miUltimoTramoDesdeFin.P2.Y);

                        var entronqueD = new oEntronqueData(miIdTijera, 0, mRadioEntronque, roadDesign.Vp,
                            tramosValoracion, miUltimoTramoDesdeInicio.P2.Z, miUltimoTramoDesdeFin.P2.Z,
                            isCurvaGranRadio, mCoefDistTramo, mCoefDistEje, mDistEntronque);


                        mEntronque = new oEntronqueFerrocarril(miP1T1, miP2T1, miP1T2, miP2T2, mEjeVisibilidad,
                            abanicoDesign, miMedicionTiempo, entronqueD);

                        mEntronque.setUpListasTijeras(miLstTijeraDsdInicio, miLstTijeraDsdFin, roadPendientes,
                            abanicoDesign.tramoAbanicoDiscretizacion, estudioData, calculaPkIniEntronque());
                        ultimoTramoFDesdeini =
                            miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.mTramosFerrocarril[
                                miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1].tramoGanador.mTramosFerrocarril
                                    .Count - 1];
                        pendientedesdeini = (ultimoTramoFDesdeini.P2.Z - ultimoTramoFDesdeini.P1.Z)/
                                            ultimoTramoFDesdeini.mTrazadoTramo.Length;
                        ultimoTramoFDesdefin =
                            miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.mTramosFerrocarril[
                                miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 1].tramoGanador.mTramosFerrocarril.Count - 1
                                ];
                        pendientedesdefin = (ultimoTramoFDesdefin.P2.Z - ultimoTramoFDesdefin.P1.Z)/
                                            ultimoTramoFDesdefin.mTrazadoTramo.Length;

                        existEntonque = mEntronque.createEntronque(mPenalizarTramosCortosEntronque, roadDesign.Vp,
                            pendientedesdeini, ultimoTramoFDesdeini.mTrazadoTramo.Length, pendientedesdefin,
                            ultimoTramoFDesdefin.mTrazadoTramo.Length);
                    }
                    else
                    {
                        //crear znp

                        oP2d miA = null, miB = null;
                        if (miLstTijeraDsdInicio.Count > 0)
                            //crear ZNP con la ultima tijera (list1)
                            miA = crearZNP(miLstTijeraDsdInicio[miLstTijeraDsdInicio.Count - 1]);

                        if (miLstTijeraDsdFin.Count > 1)
                            //crear ZNP con la penultima tijera (list2)
                            miB = crearZNP(miLstTijeraDsdFin[miLstTijeraDsdFin.Count - 2]);


                        if (miA != null)
                            eliminaTijerasZNP(true, miA);
                        if (miB != null)
                            eliminaTijerasZNP(false, miB);


                        return false;

                    }
                    numCorrecciones++;

                }


                miMedicionTiempo.Stop();
                return true;

            }


        }

        private double calculaPkIniEntronque()
        {
            double pkDesdeIni = 0;
            foreach (oTijera miTijera in miLstTijeraDsdInicio)
            {
                pkDesdeIni = pkDesdeIni + miTijera.tramoGanador.longitud2d;
            }
            if (tramoSalida!=null)
                pkDesdeIni = pkDesdeIni + tramoSalida.longitud2d;
            return pkDesdeIni;
        }

        private oP2d crearZNP(oTijera iTijera)
        {
            Point3d miP3d = new Point3d(iTijera.mPtoOrigen.X, iTijera.mPtoOrigen.Y, 0);
            oP2d miP = new oP2d(iTijera.mPtoOrigen.X, iTijera.mPtoOrigen.Y);
            Point3d miP2 = mEjeVisibilidad.getPointMasCercano(miP3d);

            var longitud = roadDesign.Rp/6;

                double miAzimut1 = oTrigo.getAzimutGrados(new oP2d(iTijera.mPtoOrigen.X, iTijera.mPtoOrigen.Y), new oP2d(miP2.X, miP2.Y));
                oP2d miA = oTrigo.getP2FromAzimutLon(miP, miAzimut1, longitud);

                double miAzimut2 = miAzimut1 + 90;
                if (miAzimut2 > 360) miAzimut2 = miAzimut2 - 360;
                oP2d miB = oTrigo.getP2FromAzimutLon(miP, miAzimut2, longitud);


                double miAzimut3 = miAzimut2 + 90;
                if (miAzimut3 > 360) miAzimut3 = miAzimut3 - 360;
                oP2d miC = oTrigo.getP2FromAzimutLon(miP, miAzimut3, longitud);


                double miAzimut4 = miAzimut3 + 90;
                if (miAzimut4 > 360) miAzimut4 = miAzimut4 - 360;
                oP2d miD = oTrigo.getP2FromAzimutLon(miP, miAzimut4, longitud);



                List<Point2d> misPuntosCriticosZNP = new List<Point2d>();
                misPuntosCriticosZNP.Add(new Point2d(miA.X, miA.Y));
                misPuntosCriticosZNP.Add(new Point2d(miB.X, miB.Y));
                misPuntosCriticosZNP.Add(new Point2d(miC.X, miC.Y));
                misPuntosCriticosZNP.Add(new Point2d(miD.X, miD.Y));
                misPuntosCriticosZNP.Add(new Point2d(miA.X, miA.Y));
            /*
             * Se cambia para hacer una zona de no paso temporal
             */
            //oLw.addLw2d(misPuntosCriticosZNP, true, oTadil.data.Layer.zonaNoPasoUsuario.name);
            oLw.addLw2d(misPuntosCriticosZNP, true, oTadil.data.Layer.zonaNoPasoUsuario_Temp.name);

            return miA;



        }

        private void eliminaTijerasZNP(bool isTijerasIni, oP2d miPuntoZNP)
        {
            List<string> misIdTijeras = new List<string>();
            List<oTijera> miListaTijeras;
            List<oTijera> miListaTijerasAux;
            if (isTijerasIni)
            {
                miListaTijeras = miLstTijeraDsdInicio;
                miListaTijerasAux = miLstTijeraDsdFin;

            }
            else
            {

                miListaTijeras = miLstTijeraDsdFin;
                miListaTijerasAux = miLstTijeraDsdInicio;
            }

            if (miListaTijeras.Count > 0)
            {
                double distancia = miPuntoZNP.distTo2d(miListaTijeras[miListaTijeras.Count - 1].tramoGanador.P1);
                misIdTijeras.Add(miListaTijeras[miListaTijeras.Count - 1].tramoGanador.mIdTijera.ToString());
                eliminarUltimaTijera(miListaTijeras);

                while ((distancia < 2 * mRc) && (miListaTijeras.Count > 0))
                {
                    distancia = miPuntoZNP.distTo2d(miListaTijeras[miListaTijeras.Count - 1].tramoGanador.P1);
                    misIdTijeras.Add(miListaTijeras[miListaTijeras.Count - 1].tramoGanador.mIdTijera.ToString());

                    if (miListaTijerasAux.Count > 0)
                    {
                        misIdTijeras.Add(miListaTijerasAux[miListaTijerasAux.Count - 1].tramoGanador.mIdTijera.ToString());
                        eliminarUltimaTijera(miListaTijerasAux);
                    }
                    eliminarUltimaTijera(miListaTijeras);
                }
            }

        }

        private bool crearTijeraEjeBasico(ref bool retroceso, oP2d iPtoTarget, oTarget iTarget , ref double iDistObjetivo,
            List<oTijera> iLstTijera, ref int iIdTijera, ref oTramoEjeBasico iTramoPrevio, oP3d iPtoEntronque, bool isTijeraAlReves,
            ref double iPendAnt, string ixData, double iLonganterior,bool dibujar)
        {

            int numTijeras = iLstTijera.Count;
            oTijera miTijera;
            if (!retroceso)
            {

                var miTramosValoracion = tramosValoracion;
                miTijera = new oTijera(iIdTijera, iTramoPrevio, iPtoTarget, iTarget, mRc, roadDesign.Vp, 
                    (abanicoDesign.abanicoTipo == eAbanicoTipo.largos), isTijeraAlReves, mEjeVisibilidad, mCoefDistTramo, mCoefDistEje,dibujar);
                miTijera.calcularTramosGanadores(miTramosValoracion, false, iPendAnt, false, null, iLonganterior);
                miTijera.drawTijeraTramos(oTadil.data.Layer.abanicoTramos.name, ixData);

                iLstTijera.Add(miTijera);
            }
            else
            {
                if (iLstTijera.Count > 0)
                {
                    miTijera = iLstTijera[iLstTijera.Count - 1];
                    miTijera.tramoGanador.mIsValido = false;
                    if (miTijera.existenTramosValidos())
                    {
                        miTijera.tramoGanador = miTijera.getTramoGanador();
                        iPendAnt = miTijera.tramoGanador.mTramosFerrocarril[miTijera.tramoGanador.mTramosFerrocarril.Count - 1].pendienteConSignoPU;
                        iDistObjetivo = miTijera.tramoGanador.P2.distTo2d(iPtoEntronque);
                        retroceso = false;
                    }
                    else
                    {
                        //crear ZNP con la ultima tijera (list1)
                        oP2d miA = crearZNP(iLstTijera[iLstTijera.Count - 1]);
                        if (miA != null)
                            eliminaTijerasZNP(!isTijeraAlReves, miA);
                        retroceso = true;
                    }
                }
                else
                {
                    return true;
                }
            }


            if (retroceso) return numTijeras > iLstTijera.Count;

            if (miTijera.existenTramosValidos())
            {
                oTramoTijera miUltimoGanador = miTijera.tramoGanador;
                iTramoPrevio = getTramoEjeBasico(miTijera.tramoGanador);


                iDistObjetivo = iTramoPrevio.P2.distTo2d(iPtoEntronque);
                miTijera.drawTramosGanadores(oTadil.data.Layer.abanicoTramos.name);
                iPendAnt = miUltimoGanador.mTramosFerrocarril[miUltimoGanador.mTramosFerrocarril.Count - 1].pendienteConSignoPU;

                iIdTijera++;
                
            }
            else
            {
                eliminarUltimaTijera(iLstTijera);
                retroceso = true;
            }


            return numTijeras > iLstTijera.Count;

        }

        private oTramoEjeBasico getTramoEjeBasico(oTramoTijera iTramoTijera)
        {
            oTramoEjeBasico miTramo = new oTramoAvanceCorto();

            miTramo.P1 = iTramoTijera.mPuntoIntermedio;
            miTramo.P2 = iTramoTijera.P2;
            miTramo.idAbanico = iTramoTijera.mIdTijera;
            miTramo.idTramo = iTramoTijera.mIdTramo;
            miTramo.mTrazadoFinal = iTramoTijera.getTramoFinal();
            miTramo.mCotaFinal = iTramoTijera.mTramosFerrocarril[iTramoTijera.mTramosFerrocarril.Count-1].P2.Z;
            var ultimoTramo = iTramoTijera.mTramosFerrocarril[iTramoTijera.mTramosFerrocarril.Count - 1];
            miTramo.mLongitudUltimoTramo = ultimoTramo.longitud2d;
            if (ultimoTramo.mTrazadoTramo != null)
                miTramo.mLongitudUltimoTramo = ultimoTramo.mTrazadoTramo.Length;
            miTramo.mPendienteUltimoTramo = (ultimoTramo.P2.Z - ultimoTramo.P1.Z) / miTramo.mLongitudUltimoTramo;




            return miTramo;
        }

        private oLstTramosGanadores generarTramosGanadores()
        {

            int idTramo = 0;
            oLstTramosGanadores miEjeBasicoPrimario = new oLstTramosGanadores();

            if (tramoSalida != null)
            {
                tramoSalida.idTramo = idTramo;
                miEjeBasicoPrimario.addTramo(tramoSalida);
                idTramo++;
            }


            foreach (oTijera miTijera1 in miLstTijeraDsdInicio)
            {
                foreach (oTramoFerrocarril miTramoF in miTijera1.tramoGanador.mTramosFerrocarril)
                {
                    miTramoF.idTramo = idTramo;
                    idTramo++;
                    miEjeBasicoPrimario.addTramo(miTramoF);
                }
            }

            foreach (oTramoTijera miTramoT in mEntronque.mTramosGanadores)
            {
                foreach (oTramoFerrocarril miTramoF in miTramoT.mTramosFerrocarril)
                {
                    miTramoF.idTramo = idTramo;
                    idTramo++;
                    miEjeBasicoPrimario.addTramo(miTramoF);
                }
            }

            for (int i = miLstTijeraDsdFin.Count - 1; i >= 0; i--)
            {
                oTramoTijera miTramoT = miLstTijeraDsdFin[i].tramoGanador;

                for (int j = miTramoT.mTramosFerrocarril.Count - 1; j >= 0; j--)
                {
                    oTramoFerrocarril miTramoF = miTramoT.mTramosFerrocarril[j];
                    oP3d miP1 = miTramoF.P1;
                    oP3d miP2 = miTramoF.P2;

                    miTramoF.idTramo = idTramo;
                    idTramo++;
                    miTramoF.P1 = miP2;
                    miTramoF.P2 = miP1;
                    miTramoF.createSeccionP1P2(abanicoDesign.tramoAbanicoDiscretizacion, roadPendientes, estudioData, false);

                    miEjeBasicoPrimario.addTramo(miTramoF);
                }

            }

            if (tramoLlegada != null)
            {
                tramoLlegada.idTramo = idTramo;
                oP3d miP1llegada = tramoLlegada.P1;
                oP3d miP2llegada = tramoLlegada.P1;
                tramoLlegada.P1 = miP2llegada;
                tramoLlegada.P2 = miP1llegada;
                miEjeBasicoPrimario.addTramo(tramoLlegada);
            }

            return miEjeBasicoPrimario;
        }

        #endregion

        public void crearEjeTrazado()
        {
            double miBombeo = 2;
            if (estudioTipo == eEstudioTipo.ESTINF)
                      miBombeo = oSingletonProyecto.getInstance.seccionCalzadaCompleta.BombeoPC.Value;
            Punto3d miUltimoPunto = new Punto3d();
            double miAzAnt = 0;
            double miPk = 0;

            miUltimoPunto = new Punto3d(ptoSalida.X, ptoSalida.Y, ptoSalida.Z);
            if (tramoSalida != null)
            {
                miUltimoPunto = new Punto3d(tramoSalida.P1.X, tramoSalida.P1.Y, 0);


                miAzAnt = oTrigo.getAzimutGrados(new oP2d(tramoSalida.P1.X, tramoSalida.P1.Y),
                    new oP2d(tramoSalida.P2.X, tramoSalida.P2.Y));

                Punto3d miCentro = new Punto3d(0, 0, 0);
                mListaVerticesTrazado.Add(new Vertice(new Punto3d(tramoSalida.P1.X, tramoSalida.P1.Y, 0), miAzAnt,
                    EjeTrazado.sentidoCurva.noValorado, 0,
                    EjeTrazado.tipoCurva.noValorado,
                    EjeTrazado.tipoSegmento.noValorado, 0, miCentro));
            }
            else
            {

                miAzAnt = ptoSalida.azimutGrados.Value;
                Punto3d miCentro = new Punto3d(0, 0, 0);
                mListaVerticesTrazado.Add(new Vertice(new Punto3d(ptoSalida.X, ptoSalida.Y, 0), miAzAnt,
                    EjeTrazado.sentidoCurva.noValorado, 0,
                    EjeTrazado.tipoCurva.noValorado,
                    EjeTrazado.tipoSegmento.noValorado, 0, miCentro));
            }
            bool isTramoSalida = true;

            foreach (oTijera miTijera1 in miLstTijeraDsdInicio)
            {
                if (miTijera1.tramoGanador.mVerticeEjeTrazado != null) mListaVerticesTrazado.Add(miTijera1.tramoGanador.mVerticeEjeTrazado);

                

                if (miTijera1.tramoGanador.mComponentesEjeTrazado.Count > 1)
                {
                    if (!isTramoSalida)
                    {
                        miAzAnt = oTrigo.getAzimutGrados(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY), new oP2d(miTijera1.tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaX, miTijera1.tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaY));
                    }
                    else isTramoSalida = false;
                    Linea miLinea = new Linea(miUltimoPunto, miTijera1.tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada, miPk, miBombeo, miAzAnt);
                    if (Math.Round(miLinea.getLongitud()) == 0)
                    {
                        miLinea = new Linea(new Punto3d(miTijera1.tramoGanador.P1.X, miTijera1.tramoGanador.P1.Y, 0), new Punto3d(miTijera1.tramoGanador.mPuntoIntermedio.X, miTijera1.tramoGanador.mPuntoIntermedio.Y, 0), miTijera1.tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada, miPk, miBombeo, oTrigo.getAzimutGrados(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY), new oP2d(miTijera1.tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaX, miTijera1.tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaY)));
                    }

                    mListaComponentesTrazado.Add(miLinea);
                    miPk = miPk + miLinea.getLongitud();

                    Clotoide miClotoide1 = (Clotoide)miTijera1.tramoGanador.mComponentesEjeTrazado[0];
                    Clotoide newClo1 = new Clotoide(miClotoide1.getPuntoEntrada, miClotoide1.getPuntoSalida, miClotoide1.getRadio, miPk, miClotoide1.mSentCurva, miBombeo, roadDesign.peralte, true, EjeTrazado.tipoClotoide.entrada, miClotoide1.mAzimut, false, miClotoide1.mDeltaRad / Math.PI * 180, false, miClotoide1.mLe, miClotoide1.mA);
                    mListaComponentesTrazado.Add(newClo1);
                    miPk = miPk + newClo1.getLongitud();


                    Curva miCurva = (Curva)miTijera1.tramoGanador.mComponentesEjeTrazado[1];
                    var peralte = roadDesign.peralte;
                    if (miCurva.isCurvaGranRadio()) peralte = miBombeo;
                    Curva newCurva = new Curva(miCurva.getPuntoEntrada, miCurva.getPuntoSalida, miCurva.getCentroCurva, miCurva.getRadio, miPk, peralte, miBombeo, miCurva.getSentCurva, mListaComponentesTrazado.Last().getLongitud());
                    mListaComponentesTrazado.Add(newCurva);
                    miPk = miPk + newCurva.getLongitud();


                    Clotoide miClotoide2 = (Clotoide)miTijera1.tramoGanador.mComponentesEjeTrazado[2];
                    Clotoide newClo2 = new Clotoide(miClotoide2.getPuntoEntrada, miClotoide2.getPuntoSalida, miClotoide2.getRadio, miPk, miClotoide1.mSentCurva, roadDesign.peralte, miBombeo, false, EjeTrazado.tipoClotoide.salida, miClotoide2.mAzimut, false, miClotoide2.mDeltaRad / Math.PI * 180, false, miClotoide2.mLe, miClotoide1.mA);
                    mListaComponentesTrazado.Add(newClo2);
                    miPk = miPk + newClo2.getLongitud();

                    miUltimoPunto = miTijera1.tramoGanador.mComponentesEjeTrazado[2].getPuntoSalida;
                }
            }

            foreach (oTramoTijera miTramoEntronque in mEntronque.mTramosGanadores)
            {
                mListaVerticesTrazado.Add(miTramoEntronque.mVerticeEjeTrazado);

                Linea miLinea = new Linea(miUltimoPunto, miTramoEntronque.mComponentesEjeTrazado[0].getPuntoEntrada, miPk, miBombeo, oTrigo.getAzimutGrados(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY), new oP2d(miTramoEntronque.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaX, miTramoEntronque.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaY)));
                if (Math.Round(miLinea.getLongitud()) == 0)
                {
                    miLinea = new Linea(new Punto3d(miTramoEntronque.P1.X, miTramoEntronque.P1.Y, 0), new Punto3d(miTramoEntronque.mPuntoIntermedio.X, miTramoEntronque.mPuntoIntermedio.Y, 0), miTramoEntronque.mComponentesEjeTrazado[0].getPuntoEntrada, miPk, miBombeo, oTrigo.getAzimutGrados(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY), new oP2d(miTramoEntronque.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaX, miTramoEntronque.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaY)));
                }
                mListaComponentesTrazado.Add(miLinea);
                miPk = miPk + miLinea.getLongitud();

                Clotoide miClotoide1 = (Clotoide)miTramoEntronque.mComponentesEjeTrazado[0];
                Clotoide newClo1 = new Clotoide(miClotoide1.getPuntoEntrada, miClotoide1.getPuntoSalida, miClotoide1.getRadio, miPk, miClotoide1.mSentCurva, miBombeo, roadDesign.peralte, true, EjeTrazado.tipoClotoide.entrada, miClotoide1.mAzimut, false, miClotoide1.mDeltaRad / Math.PI * 180, false, miClotoide1.mLe, miClotoide1.mA);
                mListaComponentesTrazado.Add(newClo1);
                miPk = miPk + newClo1.getLongitud();

                Curva miCurva = (Curva)miTramoEntronque.mComponentesEjeTrazado[1];
                Curva newCurva = new Curva(miCurva.getPuntoEntrada, miCurva.getPuntoSalida, miCurva.getCentroCurva, miCurva.getRadio, miPk, roadDesign.peralte, miBombeo, miCurva.getSentCurva, mListaComponentesTrazado.Last().getLongitud());
                mListaComponentesTrazado.Add(newCurva);
                miPk = miPk + newCurva.getLongitud();


                Clotoide miClotoide2 = (Clotoide)miTramoEntronque.mComponentesEjeTrazado[2];
                Clotoide newClo2 = new Clotoide(miClotoide2.getPuntoEntrada, miClotoide2.getPuntoSalida, miClotoide2.getRadio, miPk, miClotoide1.mSentCurva, roadDesign.peralte, miBombeo, false, EjeTrazado.tipoClotoide.salida, miClotoide2.mAzimut, false, miClotoide2.mDeltaRad / Math.PI * 180, false, miClotoide2.mLe, miClotoide1.mA);
                mListaComponentesTrazado.Add(newClo2);
                miPk = miPk + newClo2.getLongitud();

                miUltimoPunto = miTramoEntronque.mComponentesEjeTrazado[2].getPuntoSalida;

            }

            for (int i = miLstTijeraDsdFin.Count - 1; i >= 0; i--)
            {


                if (miLstTijeraDsdFin[i].tramoGanador.mVerticeEjeTrazado != null) mListaVerticesTrazado.Add(miLstTijeraDsdFin[i].tramoGanador.mVerticeEjeTrazado);

                if (miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado.Count > 1)
                {
                    Linea miLinea = new Linea(miUltimoPunto, miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada, miPk, miBombeo, oTrigo.getAzimutGrados(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY), new oP2d(miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaX, miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaY)));
                    if (Math.Round(miLinea.getLongitud()) == 0)
                    {
                        miLinea = new Linea(new Punto3d(miLstTijeraDsdFin[i].tramoGanador.P2.X, miLstTijeraDsdFin[i].tramoGanador.P2.Y, 0), new Punto3d(miLstTijeraDsdFin[i].tramoGanador.mPuntoIntermedio.X, miLstTijeraDsdFin[i].tramoGanador.mPuntoIntermedio.Y, 0), miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada, miPk, miBombeo, oTrigo.getAzimutGrados(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY), new oP2d(miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaX, miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[0].getPuntoEntrada.coordenadaY)));
                    }
                    mListaComponentesTrazado.Add(miLinea);
                    miPk = miPk + miLinea.getLongitud();

                    Clotoide miClotoide1 = (Clotoide)miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[0];
                    Clotoide newClo1 = new Clotoide(miClotoide1.getPuntoEntrada, miClotoide1.getPuntoSalida, miClotoide1.getRadio, miPk, miClotoide1.mSentCurva, miBombeo, roadDesign.peralte, true, EjeTrazado.tipoClotoide.entrada, miClotoide1.mAzimut, false, miClotoide1.mDeltaRad / Math.PI * 180, false, miClotoide1.mLe, miClotoide1.mA);
                    mListaComponentesTrazado.Add(newClo1);
                    miPk = miPk + newClo1.getLongitud();

                    Curva miCurva = (Curva)miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[1];
                    Curva newCurva = new Curva(miCurva.getPuntoEntrada, miCurva.getPuntoSalida, miCurva.getCentroCurva, miCurva.getRadio, miPk, roadDesign.peralte, miBombeo, miCurva.getSentCurva, mListaComponentesTrazado.Last().getLongitud());
                    mListaComponentesTrazado.Add(newCurva);
                    miPk = miPk + newCurva.getLongitud();


                    Clotoide miClotoide2 = (Clotoide)miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[2];
                    Clotoide newClo2 = new Clotoide(miClotoide2.getPuntoEntrada, miClotoide2.getPuntoSalida, miClotoide2.getRadio, miPk, miClotoide1.mSentCurva, roadDesign.peralte, miBombeo, false, EjeTrazado.tipoClotoide.salida, miClotoide2.mAzimut, false, miClotoide2.mDeltaRad / Math.PI * 180, false, miClotoide2.mLe, miClotoide1.mA);
                    mListaComponentesTrazado.Add(newClo2);
                    miPk = miPk + newClo2.getLongitud();

                    miUltimoPunto = miLstTijeraDsdFin[i].tramoGanador.mComponentesEjeTrazado[2].getPuntoSalida;
                }
            }


            Linea miLineaFinal = new Linea(miUltimoPunto, new Punto3d(ptoLlegada.X, ptoLlegada.Y, 0), miPk, miBombeo, oTrigo.getAzimutGrados(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY), new oP2d(ptoLlegada.X, ptoLlegada.Y)));

            if(tramoLlegada==null && Math.Round(miLineaFinal.getLongitud(),2) == 0)
            {
                var azimutLlegada = ptoLlegada.azimutGrados.Value - 180;
                if (ptoLlegada.azimutGrados.Value <= 180)
                    azimutLlegada = ptoLlegada.azimutGrados.Value + 180;
                var p2 = oTrigo.getP2FromAzimutLon(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY),
                    azimutLlegada, 100);
                miLineaFinal = new Linea(miUltimoPunto, new Punto3d(p2.X, p2.Y, 0), miUltimoPunto, miPk, miBombeo, oTrigo.getAzimutGrados(new oP2d(miUltimoPunto.coordenadaX, miUltimoPunto.coordenadaY), p2));

                mListaComponentesTrazado.Last().setPuntoSalida = new Punto3d(ptoLlegada.X, ptoLlegada.Y, ptoLlegada.Z);
            }
            mListaComponentesTrazado.Add(miLineaFinal);


        }

       private void eliminarUltimaTijera(List<oTijera> iTijeras)
        {
            if (iTijeras.Count > 0)
            {
                iTijeras.RemoveAt(iTijeras.Count - 1);
            }
        }


    }
}
