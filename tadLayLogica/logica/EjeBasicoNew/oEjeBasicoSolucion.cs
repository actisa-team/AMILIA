using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{

    using Autodesk.AutoCAD.DatabaseServices;

    using engNet.Extension.Collection;
    using engNet.Frm;

    using tadLayLogica.datos.proyecto;
    using tadLayShare.puntos;
    using tadLayLogica.zonaGis;
    using tadLayLogica.estudioTipo;
    using tadLayLan;
    using tadLayShare;
    using engCadNet;
    using Autodesk.AutoCAD.Geometry;
    using tadLayLogica.logica.EjeBasicoNew.collection;
    using tadLayLan.Tdi;
    using tadLayLogica.Comandos;


    /// <summary>
    /// EJE BASICO
    /// </summary>
    public  class oEjeBasicoSolucion
    {

        private string KSolPrimaria = strFrmSolucion.uiNomSolPrimaria;
        private string KSolEnvolventeMax = strFrmSolucion.uiNomEnvMaxima;
        private string KSolEnvolventeMin = strFrmSolucion.uiNomEnvMinima;

        private string mSolucionNombre = string.Empty;

        private bool? mGenerarSolucionEnvolventeMaximaMinima = null;

        private int mRepeticiones = 1;
        private oAbanicoByPoint mUltimoAbanico = null;

        public oRoadDes roadDesign = null;
        public oRoadPendientes roadPendientes = null;
        public oCoeMinoracionAlturasMaximas coeMinoracionAlturasMaximas = null;
        public oTramosValoracion tramosValoracion = null;

        private oTramosValoracion tramosValoracionFinal = null;

        public oAbanicoDesign abanicoDesign = null;
        public IEstudio estudioData = null;

        public oP3dSalidaLlegada ptoSalida { get; private set; }
        public oP3dSalidaLlegada ptoLlegada { get; private set; }

        public oTramoEjeBasico tramoSalida { get; private set; }
        public oTramoEjeBasico tramoLlegada { get; private set; }



        #region "CONSTRUCTOR"

        public oEjeBasicoSolucion(string iSolucionPrefijo,
                         bool iGenerarEnvolventeMaximaMinima,
                         oRoadDes iRoadDesign,
                         oRoadPendientes iRoadPendientes,
                         oCoeMinoracionAlturasMaximas iCoeMinAlturasMaximas,
                         oTramosValoracion iTramosValoracion,
                         oAbanicoDesign iAbanicoDesign,
                         IEstudio iEstudio)                                                                                                                 
       {

           this.solucionPrefijo = iSolucionPrefijo;
           this.generarSolucionesEnvolventeMaximaMinimo = iGenerarEnvolventeMaximaMinima;

           roadDesign = iRoadDesign;
           roadPendientes = iRoadPendientes;
           coeMinoracionAlturasMaximas = iCoeMinAlturasMaximas;
           tramosValoracion = iTramosValoracion;
           tramosValoracionFinal = new oTramosValoracion(60, 10, 30);
           abanicoDesign = iAbanicoDesign;
           estudioData = iEstudio;

           //punto de Salida
           ptoSalida = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoSalida);
           //Punto de Llegada
           ptoLlegada = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoLlegada);
       }

        #endregion
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
        public bool generarSolucionesEnvolventeMaximaMinimo
        {
            get
            {
                if (mGenerarSolucionEnvolventeMaximaMinima==null)
                {
                    throw new oExPropertieNullValue("generarSolucionesEnvolventesMaximoMinimo");
                }

                return  mGenerarSolucionEnvolventeMaximaMinima.Value;
            }


            private set
            {

                mGenerarSolucionEnvolventeMaximaMinima = value;
            }



        }
        #endregion
        #region "Metodos Publico"
        public void createEjesBasicos(Polyline iEjeVisibilidad, int iRepeticionesMax, bool iAutoCorrecion, bool dibujar)
        {
            //Activo las Capas
            oTadil.data.Layer.zonaNoPasoUsuario.On();
            oTadil.data.Layer.zonaNoPasoPendiente.On();
            oTadil.data.Layer.zonasGisOn();

            //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
            oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();
            List<Polyline> miLstObstacle = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;


            //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(new double[] { ptoSalida.X, ptoSalida.Y });
                        //Creamos el Tramo de Salida
            tramoSalida = oFactoryTramoSalidaLlegada.createTramoSalidaLlegada(true,0,0,ptoSalida, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion, false);

            //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(new double[] { ptoLlegada.X, ptoLlegada.Y });
            //Creamoe el Tramo de Llegada
            tramoLlegada = oFactoryTramoSalidaLlegada.createTramoLlegada(ptoLlegada, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion);

            

            //Seleccionar Polilinea Eje Visibilidad
            Polyline miLwEjeVisibilidadUsuario = iEjeVisibilidad;

            //Creo el Objeto Target
            oTarget miTarget = new oTarget(ptoSalida, ptoLlegada, roadDesign.AijMin, abanicoDesign.toleranciaPuntoObjetivoPC);

            //Configuro la Envolvente
            miTarget.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(miLwEjeVisibilidadUsuario);

            //SetUpAbanico
            oAbanicoByPoint.SetUpObject(roadDesign, roadPendientes,abanicoDesign, estudioData, this.ptoLlegada);
            oAbanicoByPoint.showInfoTramos();

            if (ptoSalida.distTo2d(new oP2d(ptoLlegada.X, ptoLlegada.Y)) < roadDesign.AijMin * 2)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiPtoIniPtoFinMuyCercanos);

                return;
            }
            #region "Calculamos la Solucion Primaria"

             //Creamos la Coleccion Eje Basico
            oLstTramosGanadores miEjeBasicoPrimario = this.generarEjeBasico(miTarget, false,dibujar);

             while (!miEjeBasicoPrimario.existeSolucion() && mRepeticiones < iRepeticionesMax && iAutoCorrecion)
             {
                 mRepeticiones++;
                 List<Point2d> misPuntosCriticosZNP = mUltimoAbanico.getPuntosCriticosZNP(roadDesign.AijMin / 3);
                 misPuntosCriticosZNP.Add(misPuntosCriticosZNP[0]);

                 oLw.addLw2d(misPuntosCriticosZNP, true, oTadil.data.Layer.zonaNoPasoUsuario.name);
                 //Se borra para probar si es mas adecuado para que no genere un nuevo eje muy diferente al que tenemos ** juanma **
                 //miLwEjeVisibilidadUsuario = tadLayLogica.Comandos.oComandoEjeVisibilidad.createAutomatico();
                 miTarget.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(miLwEjeVisibilidadUsuario);
                 miEjeBasicoPrimario = this.generarEjeBasico(miTarget, false);

             }

             Polyline3d miLwEjeBasico3D=null;
             Polyline miLwEjeBasico2D=null;

             if (miEjeBasicoPrimario.existeSolucion())
             {

                 //Dibujo el Eje Basico Polilinea 3D
                 miLwEjeBasico3D = miEjeBasicoPrimario.drawEjeBasico3D(this.solucionPrimariaNombre);

                 //Dibujo el Eje Basico en Planta Polilinea Z=0
                 miLwEjeBasico2D = miEjeBasicoPrimario.drawEjeBasicoPlanta(miLwEjeBasico3D, this.solucionPrimariaNombre);

                 //Añado el Xdata al Eje 2D
                 oTadilXdata.addXdataRoadDesign(miLwEjeBasico2D, roadDesign);

                 //Guardo los Datos en la Base Datos
                 oDalTbSolucion.addEjeBasico(this.solucionPrimariaNombre, miLwEjeBasico3D.Handle.ToString(), miLwEjeBasico2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
             }
             else
             {
                 oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionPrimariaNotFound);

                 return;
             }

            #endregion
            #region "Calculamos las Soluciones por Envolvente"


            if (this.mGenerarSolucionEnvolventeMaximaMinima.Value)
            {

                //Envolvente Maxima
                miTarget.setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(miLwEjeBasico2D, oTarget.eFiltro.Max);

                if (miTarget.HasEnvolvente)
                {
                    oLstTramosGanadores miEjeBasicoEnvolventeMaxima = this.generarEjeBasico(miTarget, true);

                    if (miEjeBasicoEnvolventeMaxima.existeSolucion())
                    {

                        Polyline3d miLw3dEjeBasicoEnvolventeMaxima3D = miEjeBasicoEnvolventeMaxima.drawEjeBasico3D(this.solucionEnvolventeMaximaNombre);

                        Polyline miLw3dEjeBasicoEnvolventeMaxima2D = miEjeBasicoEnvolventeMaxima.drawEjeBasicoPlanta(miLw3dEjeBasicoEnvolventeMaxima3D, this.solucionEnvolventeMaximaNombre);

                        oTadilXdata.addXdataRoadDesign(miLw3dEjeBasicoEnvolventeMaxima2D, roadDesign);

                        oDalTbSolucion.addEjeBasico(this.solucionEnvolventeMaximaNombre, miLw3dEjeBasicoEnvolventeMaxima3D.Handle.ToString(), miLw3dEjeBasicoEnvolventeMaxima2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionEnvolventeMaximaNotFound);
                    }
                }

                //Envolvente Minima
                miTarget.setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(miLwEjeBasico2D, oTarget.eFiltro.Min);

                if (miTarget.HasEnvolvente)
                {

                    oLstTramosGanadores miEjeBasicoEnvolventeMinima = this.generarEjeBasico(miTarget, true);

                    if (miEjeBasicoEnvolventeMinima.existeSolucion())
                    {

                        Polyline3d miLw3dEjeBasicoEnvolventeMinima3D = miEjeBasicoEnvolventeMinima.drawEjeBasico3D(this.solucionEnvolventeMinimaNombre);

                        Polyline miLw3dEjeBasicoEnvolventeMinima2D = miEjeBasicoEnvolventeMinima.drawEjeBasicoPlanta(miLw3dEjeBasicoEnvolventeMinima3D, this.solucionEnvolventeMinimaNombre);

                        oTadilXdata.addXdataRoadDesign(miLw3dEjeBasicoEnvolventeMinima2D, roadDesign);

                        oDalTbSolucion.addEjeBasico(this.solucionEnvolventeMinimaNombre, miLw3dEjeBasicoEnvolventeMinima3D.Handle.ToString(), miLw3dEjeBasicoEnvolventeMinima2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionenvolventeMinimaNotFound);
                    }
                }
            }





            #endregion

        }

        public void createEjeBasicoProyeccionPuntoMedio(Polyline iEjeVisibilidad)
        {

            //Activo las Capas
            oTadil.data.Layer.zonaNoPasoUsuario.On();
            oTadil.data.Layer.zonaNoPasoPendiente.On();
            oTadil.data.Layer.zonasGisOn();

            //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
            oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();
            List<Polyline> miLstObstacle = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;


            //Creamos el Tramo de Salida
            tramoSalida = oFactoryTramoSalidaLlegada.createTramoSalidaLlegada(true, 0, 0, ptoSalida, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion, false);

            //Creamoe el Tramo de Llegada
            tramoLlegada = oFactoryTramoSalidaLlegada.createTramoSalidaLlegada(false, 0, 0, ptoLlegada, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion, true);
            //tramoLlegada = oFactoryTramoSalidaLlegada.createTramoLlegada(ptoLlegada, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion);


            //Seleccionar Polilinea Eje Visibilidad
            Polyline miLwEjeVisibilidadUsuario = iEjeVisibilidad;

            //Creo el Objeto Target
            oTarget miTargetSalida = new oTarget(ptoSalida, ptoLlegada, roadDesign.AijMin, abanicoDesign.toleranciaPuntoObjetivoPC);


            oTarget miTargetLlegada = new oTarget(ptoLlegada, ptoSalida, roadDesign.AijMin, abanicoDesign.toleranciaPuntoObjetivoPC);

            //Configuro la Envolvente
            miTargetSalida.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(miLwEjeVisibilidadUsuario);


            //Configuro la Envolvente
            miTargetLlegada.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(oLw.reverseLw(miLwEjeVisibilidadUsuario));

            //SetUpAbanico
            oAbanicoByPoint.SetUpObject(roadDesign, roadPendientes, abanicoDesign, estudioData, this.ptoLlegada);
            oAbanicoByPoint.showInfoTramos();

            oTramosEntronqueFinalPMCollection.SetUpObject(roadDesign, roadPendientes, abanicoDesign, estudioData);

            #region "Calculamos la Solucion Primaria"


            if (ptoSalida.distTo2d(new oP2d(ptoLlegada.X, ptoLlegada.Y)) < roadDesign.AijMin * 2)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiPtoIniPtoFinMuyCercanos);

                return;
            }

            //Creamos la Coleccion Eje Basico
            oLstTramosGanadores miEjeBasicoPrimario = this.generarEjeBasicoProyeccionPuntoMedio(miTargetSalida, miTargetLlegada, false);

            Polyline3d miLwEjeBasico3D = null;
            Polyline miLwEjeBasico2D = null;

            try
            {
                if (miEjeBasicoPrimario.existeSolucion())
                {

                    //Dibujo el Eje Basico Polilinea 3D
                    miLwEjeBasico3D = miEjeBasicoPrimario.drawEjeBasico3D(this.solucionPrimariaNombre);

                    //Dibujo el Eje Basico en Planta Polilinea Z=0
                    miLwEjeBasico2D = miEjeBasicoPrimario.drawEjeBasicoPlanta(miLwEjeBasico3D, this.solucionPrimariaNombre);

                    //Añado el Xdata al Eje 2D
                    oTadilXdata.addXdataRoadDesign(miLwEjeBasico2D, roadDesign);

                    //Guardo los Datos en la Base Datos
                    oDalTbSolucion.addEjeBasico(this.solucionPrimariaNombre, miLwEjeBasico3D.Handle.ToString(), miLwEjeBasico2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
                }
                else
                {

                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionPrimariaNotFound);

                    return;
                }
            }
            catch (Exception)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionPrimariaNotFound);

                return;
            }

            if (this.mGenerarSolucionEnvolventeMaximaMinima.Value)
            {

                //Envolvente Maxima
                miTargetSalida.setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(miLwEjeBasico2D, oTarget.eFiltro.Max);

                if (miTargetSalida.HasEnvolvente)
                {
                    oLstTramosGanadores miEjeBasicoEnvolventeMaxima = this.generarEjeBasico(miTargetSalida, true);

                    if (miEjeBasicoEnvolventeMaxima.existeSolucion())
                    {

                        Polyline3d miLw3dEjeBasicoEnvolventeMaxima3D = miEjeBasicoEnvolventeMaxima.drawEjeBasico3D(this.solucionEnvolventeMaximaNombre);

                        Polyline miLw3dEjeBasicoEnvolventeMaxima2D = miEjeBasicoEnvolventeMaxima.drawEjeBasicoPlanta(miLw3dEjeBasicoEnvolventeMaxima3D, this.solucionEnvolventeMaximaNombre);

                        oTadilXdata.addXdataRoadDesign(miLw3dEjeBasicoEnvolventeMaxima2D, roadDesign);

                        oDalTbSolucion.addEjeBasico(this.solucionEnvolventeMaximaNombre, miLw3dEjeBasicoEnvolventeMaxima3D.Handle.ToString(), miLw3dEjeBasicoEnvolventeMaxima2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionEnvolventeMaximaNotFound);
                    }
                }

                //Envolvente Minima
                miTargetSalida.setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(miLwEjeBasico2D, oTarget.eFiltro.Min);

                if (miTargetSalida.HasEnvolvente)
                {

                    oLstTramosGanadores miEjeBasicoEnvolventeMinima = this.generarEjeBasico(miTargetSalida, true);

                    if (miEjeBasicoEnvolventeMinima.existeSolucion())
                    {

                        Polyline3d miLw3dEjeBasicoEnvolventeMinima3D = miEjeBasicoEnvolventeMinima.drawEjeBasico3D(this.solucionEnvolventeMinimaNombre);

                        Polyline miLw3dEjeBasicoEnvolventeMinima2D = miEjeBasicoEnvolventeMinima.drawEjeBasicoPlanta(miLw3dEjeBasicoEnvolventeMinima3D, this.solucionEnvolventeMinimaNombre);

                        oTadilXdata.addXdataRoadDesign(miLw3dEjeBasicoEnvolventeMinima2D, roadDesign);

                        oDalTbSolucion.addEjeBasico(this.solucionEnvolventeMinimaNombre, miLw3dEjeBasicoEnvolventeMinima3D.Handle.ToString(), miLw3dEjeBasicoEnvolventeMinima2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionenvolventeMinimaNotFound);
                    }
                }
            }

        }


        public void createEjesBasicosManual(Polyline iEjeVisibilidad)
        {

            //Creamos el Tramo de Salida
            tramoSalida = oFactoryTramoSalidaLlegada.createTramoSalidaLlegada(true, 0, 0, ptoSalida, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion, false);

            //Creamoe el Tramo de Llegada
            tramoLlegada = oFactoryTramoSalidaLlegada.createTramoLlegada(ptoLlegada, roadDesign, roadPendientes, estudioData, abanicoDesign.tramoAbanicoDiscretizacion);


            //Seleccionar Polilinea Eje Visibilidad
            Polyline miLwEjeVisibilidadUsuario = iEjeVisibilidad;

            //Creo el Objeto Target
            oTarget miTarget = new oTarget(ptoSalida, ptoLlegada, roadDesign.AijMin, abanicoDesign.toleranciaPuntoObjetivoPC);


            //Configuro la Envolvente
            miTarget.setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(miLwEjeVisibilidadUsuario);

            //SetUpAbanico
            oAbanicoByPoint.SetUpObject(roadDesign, roadPendientes, abanicoDesign, estudioData, this.ptoLlegada);
            oAbanicoByPoint.showInfoTramos();

            #region "Calculamos la Solucion Primaria"


            if (ptoSalida.distTo2d(new oP2d(ptoLlegada.X, ptoLlegada.Y)) < roadDesign.AijMin * 2)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiPtoIniPtoFinMuyCercanos);

                return;
            }

            //Creamos la Coleccion Eje Basico MANUAL
            oLstTramosGanadores miEjeBasicoPrimario = this.generarEjeBasicoManual(miTarget, false);

            Polyline3d miLwEjeBasico3D = null;
            Polyline miLwEjeBasico2D = null;

            if (miEjeBasicoPrimario.existeSolucion())
            {

                //Dibujo el Eje Basico Polilinea 3D
                miLwEjeBasico3D = miEjeBasicoPrimario.drawEjeBasico3D(this.solucionPrimariaNombre);

                //Dibujo el Eje Basico en Planta Polilinea Z=0
                miLwEjeBasico2D = miEjeBasicoPrimario.drawEjeBasicoPlanta(miLwEjeBasico3D, this.solucionPrimariaNombre);

                //Añado el Xdata al Eje 2D
                oTadilXdata.addXdataRoadDesign(miLwEjeBasico2D, roadDesign);

                //Guardo los Datos en la Base Datos
                oDalTbSolucion.addEjeBasico(this.solucionPrimariaNombre, miLwEjeBasico3D.Handle.ToString(), miLwEjeBasico2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
            }
            else
            {

                oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionPrimariaNotFound);

                return;
            }

            #endregion


            if (this.mGenerarSolucionEnvolventeMaximaMinima.Value)
            {

                //Envolvente Maxima
                miTarget.setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(miLwEjeBasico2D, oTarget.eFiltro.Max);

                if (miTarget.HasEnvolvente)
                {
                    oLstTramosGanadores miEjeBasicoEnvolventeMaxima = this.generarEjeBasico(miTarget, true);

                    if (miEjeBasicoEnvolventeMaxima.existeSolucion())
                    {

                        Polyline3d miLw3dEjeBasicoEnvolventeMaxima3D = miEjeBasicoEnvolventeMaxima.drawEjeBasico3D(this.solucionEnvolventeMaximaNombre);

                        Polyline miLw3dEjeBasicoEnvolventeMaxima2D = miEjeBasicoEnvolventeMaxima.drawEjeBasicoPlanta(miLw3dEjeBasicoEnvolventeMaxima3D, this.solucionEnvolventeMaximaNombre);

                        oTadilXdata.addXdataRoadDesign(miLw3dEjeBasicoEnvolventeMaxima2D, roadDesign);

                        oDalTbSolucion.addEjeBasico(this.solucionEnvolventeMaximaNombre, miLw3dEjeBasicoEnvolventeMaxima3D.Handle.ToString(), miLw3dEjeBasicoEnvolventeMaxima2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionEnvolventeMaximaNotFound);
                    }
                }

                //Envolvente Minima
                miTarget.setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(miLwEjeBasico2D, oTarget.eFiltro.Min);

                if (miTarget.HasEnvolvente)
                {

                    oLstTramosGanadores miEjeBasicoEnvolventeMinima = this.generarEjeBasico(miTarget, true);

                    if (miEjeBasicoEnvolventeMinima.existeSolucion())
                    {

                        Polyline3d miLw3dEjeBasicoEnvolventeMinima3D = miEjeBasicoEnvolventeMinima.drawEjeBasico3D(this.solucionEnvolventeMinimaNombre);

                        Polyline miLw3dEjeBasicoEnvolventeMinima2D = miEjeBasicoEnvolventeMinima.drawEjeBasicoPlanta(miLw3dEjeBasicoEnvolventeMinima3D, this.solucionEnvolventeMinimaNombre);

                        oTadilXdata.addXdataRoadDesign(miLw3dEjeBasicoEnvolventeMinima2D, roadDesign);

                        oDalTbSolucion.addEjeBasico(this.solucionEnvolventeMinimaNombre, miLw3dEjeBasicoEnvolventeMinima3D.Handle.ToString(), miLw3dEjeBasicoEnvolventeMinima2D.Handle.ToString(), roadDesign, roadPendientes, coeMinoracionAlturasMaximas);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionenvolventeMinimaNotFound);
                    }
                }
            }
        }


        #endregion
        #region "Metodos Privados"

        private oLstTramosGanadores generarEjeBasicoProyeccionPuntoMedio(oTarget iTargetSalida, oTarget iTargetLlegada, bool isEnvolvente)
        {
            bool isLlegadaEspecial = false;
            bool isSalidaEspecial = false;

            //Activo las Capas
            oTadil.data.Layer.zonaNoPasoUsuario.On();
            oTadil.data.Layer.zonaNoPasoPendiente.On();
            oTadil.data.Layer.zonasGisOn();

            oTramosAbanicosBySolucionCollection miCollectionTramosBySolucionSalida = new oTramosAbanicosBySolucionCollection();
            oTramosAbanicosBySolucionCollection miCollectionTramosBySolucionLlegada = new oTramosAbanicosBySolucionCollection();
            oLstTramosGanadores miCollectionTramosGanadores = new oLstTramosGanadores();


            List<oTramoEjeBasico> miCollectionTramosGanadoresSalida = new List<oTramoEjeBasico>();
            List<oTramoEjeBasico> miCollectionTramosGanadoresLlegada = new List<oTramoEjeBasico>();
            List<oTramoEjeBasico> miCollectionTramosGanadoresPM = new List<oTramoEjeBasico>();

            double distanciaC1C2 = this.ptoSalida.distTo2d(this.ptoLlegada);
            double quintoDistancia = distanciaC1C2 / 5;


            int miIdAbanico = 1;


            #region "TramoSalida"
            
            double distC1ultimoTramo;
            double distC2ultimoTramo;

            //Salida Rotonda
            if (this.tramoSalida == null)
            {
                oTramoEjeBasico miTramoSalidaRotonda = null;
                oAbanicoByPoint miAbanicoRotonda;
                miTramoSalidaRotonda = generarTramoPrevioFromRotonda(iTargetSalida, isEnvolvente, out miAbanicoRotonda);
                miCollectionTramosGanadoresSalida.Add(miTramoSalidaRotonda);
                distC1ultimoTramo = this.ptoSalida.distTo2d(miTramoSalidaRotonda.P2);
            }
            else
            {
                miCollectionTramosGanadoresSalida.Add(this.tramoSalida);
                distC1ultimoTramo = this.ptoSalida.distTo2d(tramoSalida.P2);
                isSalidaEspecial = true;
            }

            #endregion

            #region "Tramo Llegada"

            //ES ROTONDA
            if (tramoLlegada == null)
            {
                oTramoEjeBasico miTramoLlegadaRotonda = null;
                miTramoLlegadaRotonda = generarTramoPrevioSalida(iTargetLlegada, isEnvolvente);
                miCollectionTramosGanadoresLlegada.Add(miTramoLlegadaRotonda);
                distC2ultimoTramo = this.ptoLlegada.distTo2d(miTramoLlegadaRotonda.P2);
            }
            else
            {

                miCollectionTramosGanadoresLlegada.Add(this.tramoLlegada);
                distC2ultimoTramo = this.ptoLlegada.distTo2d(tramoLlegada.P2);
                isLlegadaEspecial = true;
            }


            #endregion

            MyMessageFilter miFilter = new MyMessageFilter();

            oTramoEjeBasico miTramoPrevioSalida = miCollectionTramosGanadoresSalida.First();
            oTramoEjeBasico miTramoPrevioLlegada = miCollectionTramosGanadoresLlegada.First();

            try
            {
                #region "SetUpESC"
                System.Windows.Forms.Application.AddMessageFilter(miFilter);
               #endregion
                #region "WHILE"

                oAbanicoByPoint miAbanicoByPoint = null;
                oTramosValoracion miTramosValoracionSalida = null;
                oTramosValoracion miTramosValoracionLlegada = null;
                double miDistanciaP2ToPtoEntronque = double.MaxValue;
                oP2d miPtoTargetSalida;
                oP2d miPtoTargetLlegada;


                bool miEntronqueForzado = false;
                    while ((miDistanciaP2ToPtoEntronque > roadDesign.AijMin * 2.4)&&(!miEntronqueForzado))
                    {
                        
                        miPtoTargetSalida = miTramoPrevioLlegada.P2;
                        if (distC1ultimoTramo < quintoDistancia)
                        {
                            miPtoTargetSalida = iTargetSalida.getPtoTarget(miTramoPrevioSalida.P2, isEnvolvente);
                        }

                        miTramosValoracionSalida = this.getValoracionTramoFuncionDistanciaObjetivo(miDistanciaP2ToPtoEntronque, roadDesign.AijMin);
                        miAbanicoByPoint = new oAbanicoByPoint(miIdAbanico, miTramoPrevioSalida, miPtoTargetSalida);
                        bool miAbanicoEspecialSalida = ((miIdAbanico == 1) && isSalidaEspecial);
                        miAbanicoByPoint.calcularTramosGanadores(miTramosValoracionSalida, miAbanicoEspecialSalida);
                        miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

                        if (miAbanicoByPoint.hasTramosGanadores)
                        {

                                oTramoEjeBasico miTramoPrevioSalidaAux = miAbanicoByPoint.lastTramoGanador;
                                double distAuxSal = miTramoPrevioSalidaAux.P2.distTo2d(miTramoPrevioLlegada.P2);

                                if (distAuxSal > roadDesign.AijMin * 1.1)
                                {
                                    miCollectionTramosBySolucionSalida.AddRange(miAbanicoByPoint.lstTramosAbanico);
                                    miCollectionTramosGanadoresSalida.AddRange(miAbanicoByPoint.lstTramosGanadores);
                                    miTramoPrevioSalida = miAbanicoByPoint.lastTramoGanador;
                                    miDistanciaP2ToPtoEntronque = miTramoPrevioSalida.P2.distTo2d(miTramoPrevioLlegada.P2);
                                    miAbanicoByPoint.drawTramosGanadores(oTadil.data.Layer.abanicoTramos.name);
                                    miIdAbanico++;
                                    distC1ultimoTramo = this.ptoSalida.distTo2d(miTramoPrevioSalida.P2);
                                }
                                else
                                {
                                    miEntronqueForzado = true;
                                }
                        }
                        else
                        {
                            miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

                            return new oLstTramosGanadores();
                        }

                        if (miDistanciaP2ToPtoEntronque > roadDesign.AijMin * 2.4)
                        {

                            miPtoTargetLlegada = miTramoPrevioSalida.P2;
                            if (distC2ultimoTramo < quintoDistancia)
                            {
                                miPtoTargetLlegada = iTargetLlegada.getPtoTarget(miTramoPrevioLlegada.P2, isEnvolvente);
                            }
                            miTramosValoracionLlegada = this.getValoracionTramoFuncionDistanciaObjetivo(miDistanciaP2ToPtoEntronque, roadDesign.AijMin);
                            miAbanicoByPoint = new oAbanicoByPoint(miIdAbanico, miTramoPrevioLlegada, miPtoTargetLlegada);
                            bool miAbanicoEspecialLlegada = ((miIdAbanico == 2) && isLlegadaEspecial);
                            miAbanicoByPoint.calcularTramosGanadores(miTramosValoracionLlegada, miAbanicoEspecialLlegada);
                            miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

                            if (miAbanicoByPoint.hasTramosGanadores)
                            {

                                oTramoEjeBasico miTramoPrevioLlegadaAux = miAbanicoByPoint.lastTramoGanador;
                                double distAux = miTramoPrevioLlegadaAux.P2.distTo2d(miTramoPrevioSalida.P2);

                                if (distAux > roadDesign.AijMin * 1.1)
                                {
                                    miTramoPrevioLlegada = miAbanicoByPoint.lastTramoGanador;
                                    miDistanciaP2ToPtoEntronque = miTramoPrevioLlegada.P2.distTo2d(miTramoPrevioSalida.P2);
                                    miCollectionTramosBySolucionLlegada.AddRange(miAbanicoByPoint.lstTramosAbanico);
                                    miCollectionTramosGanadoresLlegada.AddRange(miAbanicoByPoint.lstTramosGanadores);
                                    miAbanicoByPoint.drawTramosGanadores(oTadil.data.Layer.abanicoTramos.name);
                                    miIdAbanico++;
                                    distC2ultimoTramo = this.ptoLlegada.distTo2d(miTramoPrevioLlegada.P2);
                                }
                                else
                                {
                                    miEntronqueForzado = true;
                                }


                            }
                            else
                            {
                                miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

                                return new oLstTramosGanadores();
                            }
                        }
                    }
                    oEntronqueFinalPM.createEntronqueFinalPM(miIdAbanico, tramosValoracion, new Point3d(miTramoPrevioSalida.P2.X, miTramoPrevioSalida.P2.Y, 0), new Point3d(miTramoPrevioLlegada.P2.X, miTramoPrevioLlegada.P2.Y, 0), roadDesign, miTramoPrevioSalida, miTramoPrevioLlegada);
                    miCollectionTramosGanadoresPM = oEntronqueFinalPM.calcularTramosGanadores(miTramoPrevioSalida, miTramoPrevioLlegada);
                    


                


                #endregion

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oAbanicoByPoint.showInfoTramosUnload();

                System.Windows.Forms.Application.RemoveMessageFilter(miFilter);
            }

            int idTramo = 0;
            int idAbanico= 0;

            //añado los tramos calculados desde el punto de inicio hasta el punto medio
            for (int index = 0; index < miCollectionTramosGanadoresSalida.Count; index++)
            {
                miCollectionTramosGanadores.addTramo(miCollectionTramosGanadoresSalida[index]);
                idTramo = miCollectionTramosGanadoresSalida[index].idTramo;
            }


            oTramoEjeBasico miTramoPrevio = new oTramoEjeBasico();

            //añado los tramos calculados en el entroque del PM, hay que tener en cuenta que el segundo tramo esta al reves!
            for (int index = 0; index < miCollectionTramosGanadoresPM.Count; index++)
            {
                idTramo++;
                if (index == 0)
                {
                    miCollectionTramosGanadoresPM[index].idTramo = idTramo;
                    idAbanico = miCollectionTramosGanadoresPM[index].idAbanico;
                    miCollectionTramosGanadores.addTramo(miCollectionTramosGanadoresPM[index]);
                    miTramoPrevio = miCollectionTramosGanadoresPM[index];
                }
                else
                {
                    oTramoAvanceCorto miTramoPM = (oTramoAvanceCorto)miCollectionTramosGanadoresPM[index];
                    oTramoAvanceCorto miTramoReverse1 = new oTramoAvanceCorto();
                    miTramoReverse1.idTramo = idTramo;
                    miTramoReverse1.P1 = miTramoPM.P2;
                    miTramoReverse1.P2 = miTramoPM.P1;
                    miTramoReverse1.idAbanico = miTramoPM.idAbanico;
                    miTramoReverse1.ptoTarget = miTramoPM.ptoTarget;
                    miTramoReverse1.tramoPrevio = miTramoPrevio;

                    miTramoReverse1.createSeccionP1P2(abanicoDesign.tramoAbanicoDiscretizacion, roadPendientes, estudioData, false);

                    int i = 0;
                    foreach (var item in miTramoPM.seccion)
                    {
                        miTramoReverse1.seccion[i].seccionTipo = item.seccionTipo;
                        i++;
                    }
                    miCollectionTramosGanadores.addTramo(miTramoReverse1);
                    miTramoPrevio = miTramoReverse1;

                }

            }

            //añado los tramos calculados desde el punto final al punto medio, hay que tener en cuenta que todos los tramos estan al reves!
            for (int index = 0; index < miCollectionTramosGanadoresLlegada.Count; index++)
            {
                idTramo++;
                idAbanico++;
                oTramoEjeBasico miTramoLlegada = miCollectionTramosGanadoresLlegada[miCollectionTramosGanadoresLlegada.Count - index - 1];


                oTramoEjeBasico miTramoReverse1 = new oTramoEjeBasico();
                miTramoReverse1.idTramo = idTramo;
                miTramoReverse1.P1 = miTramoLlegada.P2;
                miTramoReverse1.P2 = miTramoLlegada.P1;
                miTramoReverse1.idAbanico = miTramoLlegada.idAbanico;
                bool isTramoEspecial = (miCollectionTramosGanadoresLlegada.Count - index - 1 == 1) && isLlegadaEspecial;

                miTramoReverse1.createSeccionP1P2(abanicoDesign.tramoAbanicoDiscretizacion, roadPendientes, estudioData, isTramoEspecial);

                int i = 0;
                foreach (var item in miTramoLlegada.seccion)
                {
                    miTramoReverse1.seccion[i].seccionTipo = item.seccionTipo;
                    i++;
                }

                miCollectionTramosGanadores.addTramo(miTramoReverse1);
                miTramoPrevio = miTramoReverse1;
            }

        #endregion


            return miCollectionTramosGanadores;

        }

        private oLstTramosGanadores generarEjeBasicoManual(oTarget iTarget, bool isEnvolvente)
        {

            bool isLlegadaEspecial = false;
            bool isSalidaEspecial = false;

            oTramosAbanicosBySolucionCollection miCollectionTramosBySolucion = new oTramosAbanicosBySolucionCollection();
            oLstTramosGanadores miCollectionTramosGanadores = new oLstTramosGanadores();


            int miIdAbanico = 1;
            oP3d miPtoEntronque;

            #region "TramoSalida"


            //Salida Rotonda
            if (this.tramoSalida == null)
            {
                oTramoEjeBasico miTramoSalidaRotonda = null;
                oAbanicoByPoint miAbanicoRotonda;
                miTramoSalidaRotonda = generarTramoPrevioFromRotondaManual(iTarget, isEnvolvente, out miAbanicoRotonda);
                miCollectionTramosGanadores.addTramo(miTramoSalidaRotonda);
                miCollectionTramosBySolucion.AddRange(miAbanicoRotonda.lstTramosAbanico);
            }
            else
            {
                miCollectionTramosGanadores.addTramo(this.tramoSalida);
                isSalidaEspecial = true;
            }

            #endregion

            #region "Tramo Llegada"

            //ES ROTONDA
            if (tramoLlegada == null)
            {
                miPtoEntronque = ptoLlegada;
            }
            else
            {
                miPtoEntronque = tramoLlegada.P1;
                isLlegadaEspecial = true;
            }
            iTarget.setPuntoDeEntronque(new oP2d(miPtoEntronque.X, miPtoEntronque.Y));


            #endregion

            MyMessageFilter miFilter = new MyMessageFilter();

            oTramoEjeBasico miTramoPrevio = miCollectionTramosGanadores.tramoInicial;

            try
            {
                #region "SetUpESC"
                System.Windows.Forms.Application.AddMessageFilter(miFilter);
                #endregion
                #region "WHILE"

                oAbanicoByPoint miAbanicoByPoint = null;
                oTramosValoracion miTramosValoracion = null;
                double miDistanciaP2ToPtoEntronque = double.MaxValue;
                oP2d miPtoTarget;


                while (miDistanciaP2ToPtoEntronque > roadDesign.AijMin)
                {

                    //UpdateUI
                    System.Windows.Forms.Application.DoEvents();

                    if (miFilter.bCanceled == true)
                    {
                        System.Windows.Forms.MessageBox.Show(strGeneralUser.uiProcesoAnulado);
                        break;
                    }

                    //miPtoTarget = iTarget.getPtoTargetScw(miTramoPrevio.P2);
                    miPtoTarget = iTarget.getPtoTarget(miTramoPrevio.P2, isEnvolvente);

                    miTramosValoracion = this.getValoracionTramoFuncionDistanciaObjetivo(miDistanciaP2ToPtoEntronque, roadDesign.AijMin);

                    miAbanicoByPoint = new oAbanicoByPoint(miIdAbanico, miTramoPrevio, miPtoTarget);

                    bool miAbanicoEspecial = ((miIdAbanico == 1) && isSalidaEspecial);

                    miAbanicoByPoint.calcularTramosGanadores(miTramosValoracion, miAbanicoEspecial);

                    miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

                    if (miAbanicoByPoint.hasTramosGanadores)
                    {

                        miCollectionTramosBySolucion.AddRange(miAbanicoByPoint.lstTramosAbanico);


                        Line tramoGanador = engCadNet.oSs.seleccionUsuario<Line>(strFrmSolucion.uiSelTramoGanador, strGeneralUser.uiSelectIsNull);
                        List<string> miData = oXdata.getXData(tramoGanador, "idPosicion", string.Empty);
                        int miPos = Convert.ToInt32(miData[0]);

                        miCollectionTramosGanadores.addTramo(miAbanicoByPoint.getLstTramosByPosition(miPos));
                        //seleccionar el tramo que tenga mayor secuencia. 
                        oTramoAbanico miTramoMax = (oTramoAbanico)miAbanicoByPoint.getLstTramosByPosition(miPos)[0];
                        foreach (oTramoAbanico miTramo in miAbanicoByPoint.getLstTramosByPosition(miPos))
                        {
                            if (miTramo.idTramo > miTramoMax.idTramo)
                            {
                                miTramoMax = miTramo;
                            }
                        }


                        miAbanicoByPoint.lastTramoGanador = miTramoMax;
                        miTramoPrevio = miAbanicoByPoint.lastTramoGanador;

                        miDistanciaP2ToPtoEntronque = miTramoPrevio.P2.distTo2d(miPtoEntronque);

                        miAbanicoByPoint.drawTramosGanadores(oTadil.data.Layer.abanicoTramos.name);

                        miIdAbanico++;
                    }
                    else
                    {
                        miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

                        return new oLstTramosGanadores();
                    }
                }


                #endregion

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oAbanicoByPoint.showInfoTramosUnload();

                System.Windows.Forms.Application.RemoveMessageFilter(miFilter);
            }

            
            #region "TramoEntronque"
            //Creo el Tramo de Entronque
            List<oTramoEjeBasico> miTramoEntronque = this.createTramoEntronque(miCollectionTramosBySolucion, miCollectionTramosGanadores.idUltimoTramo, miPtoEntronque, isLlegadaEspecial, tramoLlegada);

            

           

            //Actualizo los Tramo Entronque y Anterior
            miCollectionTramosGanadores.updateTramoEntronque(miTramoEntronque);

            #endregion


            #region "Creo el Ultimo Tramo"

            if (this.tramoLlegada != null)
            {
                miCollectionTramosGanadores.addTramo(this.tramoLlegada);
            }
            #endregion


            return miCollectionTramosGanadores;

        

        }

        private oLstTramosGanadores generarEjeBasico(oTarget iTarget, bool isEnvolvente, bool dibujar=true)
        {


            bool isLlegadaEspecial = false;
            bool isSalidaEspecial = false;
            bool isTramoUnico = false;

            //Activo las Capas
            oTadil.data.Layer.zonaNoPasoUsuario.On();
            oTadil.data.Layer.zonaNoPasoPendiente.On();
            oTadil.data.Layer.zonasGisOn();

            //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
            oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();
            List<Polyline> miLstObstacle = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;

            oTramosAbanicosBySolucionCollection miCollectionTramosBySolucion = new oTramosAbanicosBySolucionCollection();
            oLstTramosGanadores miCollectionTramosGanadores = new oLstTramosGanadores();


            int miIdAbanico = 1;
            oP3d miPtoEntronque;


            #region "TramoSalida"


            //Salida Rotonda
            if (this.tramoSalida == null)
            {
                oTramoEjeBasico miTramoSalidaRotonda = null;
                oAbanicoByPoint miAbanicoRotonda;
                miTramoSalidaRotonda = generarTramoPrevioFromRotonda(iTarget, isEnvolvente, out miAbanicoRotonda, dibujar);
                miCollectionTramosGanadores.addTramo(miTramoSalidaRotonda);
                miCollectionTramosBySolucion.AddRange(miAbanicoRotonda.lstTramosAbanico);
            }
            else
            {
                miCollectionTramosGanadores.addTramo(this.tramoSalida);
                isSalidaEspecial = true;
            }

            #endregion

            #region "Tramo Llegada"

            //ES ROTONDA
            if (tramoLlegada == null)
            {
                miPtoEntronque = ptoLlegada;
            }
            else
            {
                miPtoEntronque = tramoLlegada.P1;
                isLlegadaEspecial = true;
            }
            iTarget.setPuntoDeEntronque(new oP2d(miPtoEntronque.X, miPtoEntronque.Y));


            #endregion

            MyMessageFilter miFilter = new MyMessageFilter();

            oTramoEjeBasico miTramoPrevio = miCollectionTramosGanadores.tramoInicial;

            try
            {
                #region "SetUpESC"
                System.Windows.Forms.Application.AddMessageFilter(miFilter);
                #endregion
                #region "WHILE"

                oAbanicoByPoint miAbanicoByPoint = null;
                oTramosValoracion miTramosValoracion = null;
                oP2d miPtoTarget;

                double miDistanciaP2ToPtoEntronque = miTramoPrevio.P2.distTo2d(miPtoEntronque);

                if (miDistanciaP2ToPtoEntronque < roadDesign.AijMin)
                {

                    miCollectionTramosGanadores = new oLstTramosGanadores();
                    oTramoEjeBasico miTramoEntradaSalida;
                    if (this.tramoSalida == null)
                    {
                        miTramoEntradaSalida = createTramoUnico(miTramoPrevio.P1, miPtoEntronque);
                    }
                    else
                    {
                        miTramoEntradaSalida = createTramoUnico(tramoSalida.P2, miPtoEntronque);
                    }
                    isTramoUnico = true;
                    miCollectionTramosGanadores.addTramo(miTramoEntradaSalida);
                }

                while (miDistanciaP2ToPtoEntronque > roadDesign.AijMin)
                {

                    //UpdateUI
                    System.Windows.Forms.Application.DoEvents();

                    if (miFilter.bCanceled == true)
                    {
                        System.Windows.Forms.MessageBox.Show(strGeneralUser.uiProcesoAnulado);
                        break;
                    }
                    miPtoTarget = iTarget.getPtoTarget(miTramoPrevio.P2, isEnvolvente);
                    
                    miTramosValoracion = this.getValoracionTramoFuncionDistanciaObjetivo(miDistanciaP2ToPtoEntronque, roadDesign.AijMin);

                    miAbanicoByPoint = new oAbanicoByPoint(miIdAbanico, miTramoPrevio, miPtoTarget,dibujar);

                    bool miAbanicoEspecial = ((miIdAbanico == 1) && isSalidaEspecial);

                    miAbanicoByPoint.calcularTramosGanadores(miTramosValoracion, miAbanicoEspecial);

                    //se comenta para ver si mejora la velocidad **juanma**
                    miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

                    if (miAbanicoByPoint.hasTramosGanadores)
                    {

                        miCollectionTramosBySolucion.AddRange(miAbanicoByPoint.lstTramosAbanico);

                        miCollectionTramosGanadores.addTramo(miAbanicoByPoint.lstTramosGanadores);

                        miTramoPrevio = miAbanicoByPoint.lastTramoGanador;

                        miDistanciaP2ToPtoEntronque = miTramoPrevio.P2.distTo2d(miPtoEntronque);

                        miAbanicoByPoint.drawTramosGanadores(oTadil.data.Layer.abanicoTramos.name);

                        miIdAbanico++;

                        mUltimoAbanico = miAbanicoByPoint;
                    }
                    else
                    {
                        miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);
                        mUltimoAbanico = miAbanicoByPoint;

                        return new oLstTramosGanadores();
                    }
                }


                #endregion

            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                oAbanicoByPoint.showInfoTramosUnload();

                System.Windows.Forms.Application.RemoveMessageFilter(miFilter);
            }


            #region "TramoEntronque"
            if (!isTramoUnico)
            {
                //Creo el Tramo de Entronque
                List<oTramoEjeBasico> miTramoEntronque = this.createTramoEntronque(miCollectionTramosBySolucion, miCollectionTramosGanadores.idUltimoTramo, miPtoEntronque, isLlegadaEspecial, tramoLlegada);

                //Actualizo los Tramo Entronque y Anterior
                miCollectionTramosGanadores.updateTramoEntronque(miTramoEntronque);

            }

           


            #endregion


            #region "Creo el Ultimo Tramo"

            if (this.tramoLlegada != null)
            {
                this.tramoLlegada.idTramo = miCollectionTramosGanadores.idUltimoTramo + 1;
                miCollectionTramosGanadores.addTramo(this.tramoLlegada);
            }
            #endregion


            return miCollectionTramosGanadores;

        }


        private oTramosValoracion getValoracionTramoFuncionDistanciaObjetivo(double iDistanciaLlegada,double iAij)
        {



            if (iDistanciaLlegada > 2.0 * iAij)
            {
                return tramosValoracion;
            }
            else
            {
                if (tramosValoracion.valoracionDistanciaPC > 50)
                {
                    return tramosValoracion;
                }
                else
                {
                    return tramosValoracionFinal;
                }



            }

        }

        
        /// <summary>
        /// Lista [0] Tramo Entronque Previo
        /// Lista [1] Tramo Entronque Final
        /// </summary>
        private List<oTramoEjeBasico> createTramoEntronque(oTramosAbanicosBySolucionCollection iCollectionTramosBySolucion, int iIdTramo, oP3d iPtoEntronque, bool isEntronqueEspecial, oTramoEjeBasico iTramoLlegada)
        {

            //Consulto en la Lista de todos los Tramos los candidatos anteriores
            int miIdTramoToFind = iIdTramo - 1;

            if (miIdTramoToFind < 0) miIdTramoToFind = 0;

            var miQuery = from p in iCollectionTramosBySolucion
                          where p.isTramoValido && p.idTramo == miIdTramoToFind
                          orderby p.valoracionPonderadaGlobal_0_10 descending
                          select p;


            oTramoAbanico miTramoEntronque;

            List<oTramoEjeBasico> miLstTramosEntronque = new List<oTramoEjeBasico>();

           

            foreach (var item in miQuery)
            {
                miTramoEntronque = new oTramoAvanceCorto(item, iPtoEntronque);
                //oSingletonPuntosTerreno.getInstance.MDT_Entronque(miTramoEntronque, iPtoEntronque);
                miTramoEntronque.validarTramoZonasNoPaso();
                miTramoEntronque.validarTramoP2InsideTerreno();
                miTramoEntronque.validarAnguloEntreTramosAijMinimoMinimo(roadDesign.getAijMinimoMinimoByAngulo);
                miTramoEntronque.validarAijDesviacionesMaximas(abanicoDesign.invalidarTramosAvanceCortoPorIncrementoLongitud, abanicoDesign.invalidarTramosIncrementoLongitudPC.Value);
                miTramoEntronque.createSeccionP1P2(abanicoDesign.tramoAbanicoDiscretizacion, roadPendientes, estudioData, isEntronqueEspecial);

                if (isEntronqueEspecial)
                {
                    miTramoEntronque.validarAnguloEntreTramosDadoTramo(iTramoLlegada);
                }

                if (miTramoEntronque.isTramoValido)
                {

                    miLstTramosEntronque.Add(item);
                    miLstTramosEntronque.Add(miTramoEntronque);

                    return miLstTramosEntronque;   
                }

            }


            throw new oExTramoEntronqueNoCumple();
           
        }

        private oTramoEjeBasico createTramoUnico(oP3d iP1, oP3d iP2)
        {

            oTramoAvanceCorto miTramoReverse1 = new oTramoAvanceCorto();
            miTramoReverse1.idTramo = 0;
            miTramoReverse1.P1 = iP1;
            miTramoReverse1.P2 = iP2;
            miTramoReverse1.idAbanico = 0;
            miTramoReverse1.idPosicion = 0;
            //miTramoReverse1.ptoTarget = miTramoPM.ptoTarget;
            //miTramoReverse1.tramoPrevio = miTramoPrevio;

            miTramoReverse1.createSeccion(abanicoDesign.tramoAbanicoDiscretizacion, roadPendientes, estudioData, false);

            miTramoReverse1.validarTramoZonasNoPaso();
            miTramoReverse1.validarTramoP2NearBordesTerreno();
            miTramoReverse1.validarTramoP2InsideTerreno();
            miTramoReverse1.validarDentroDPH(estudioData);
            miTramoReverse1.validarCruceDPH(estudioData);


            if (miTramoReverse1.isTramoValido)
            {
                return miTramoReverse1;
            }


            throw new oExTramoEntronqueNoCumple();

        }


        private oTramoEjeBasico generarTramoPrevioFromRotonda(oTarget iTarget, bool isEnvolvente, out oAbanicoByPoint oAbanicoRotonda, bool dibujar=true)
        {


            //Obtengo el Punto Target
            //oP2d miPtoTarget = iTarget.getPtoTargetScw(ptoSalida);
            oP2d miPtoTarget = iTarget.getPtoTarget(ptoSalida, isEnvolvente);


            oAbanicoByPoint miAbanicoRotonda = new oAbanicoByPoint(0, ptoSalida, miPtoTarget, dibujar);

            miAbanicoRotonda.calcularTramosGanadores(tramosValoracion, false);

            miAbanicoRotonda.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

            oAbanicoRotonda = miAbanicoRotonda;

            if (miAbanicoRotonda.hasTramosGanadores)
            {
                return miAbanicoRotonda.lstTramosGanadores[0];
            }
            else
            {
                throw new Exception(strError.eTramoSalidaRotonda);
            }

        }

        private oTramoEjeBasico generarTramoPrevioSalida(oTarget iTarget, bool isEnvolvente)
        {


            //Obtengo el Punto Target
            //oP2d miPtoTarget = iTarget.getPtoTargetScw(ptoLlegada);
            oP2d miPtoTarget = iTarget.getPtoTarget(ptoLlegada, isEnvolvente);


            oAbanicoByPoint miAbanicoRotonda = new oAbanicoByPoint(0, ptoLlegada, miPtoTarget);

            miAbanicoRotonda.calcularTramosGanadores(tramosValoracion, false);

            miAbanicoRotonda.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);

            if (miAbanicoRotonda.hasTramosGanadores)
            {
                return miAbanicoRotonda.lstTramosGanadores[0];
            }
            else
            {
                throw new Exception(strError.eTramoSalidaRotonda);
            }

        }

        private oTramoEjeBasico generarTramoPrevioFromRotondaManual(oTarget iTarget, bool isEnvolvente, out oAbanicoByPoint miAbanicoRotonda)
        {


            //Obtengo el Punto Target
            //oP2d miPtoTarget = iTarget.getPtoTargetScw(ptoSalida);
            oP2d miPtoTarget = iTarget.getPtoTarget(ptoSalida, isEnvolvente);


            miAbanicoRotonda = new oAbanicoByPoint(0, ptoSalida, miPtoTarget);

            miAbanicoRotonda.calcularTramosGanadores(tramosValoracion, false);

            miAbanicoRotonda.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);



            if (miAbanicoRotonda.hasTramosGanadores)
            {
                Line tramoGanador = engCadNet.oSs.seleccionUsuario<Line>(strFrmSolucion.uiSelTramoGanador, strGeneralUser.uiSelectIsNull);
                List<string> miData = oXdata.getXData(tramoGanador, "idPosicion", string.Empty);
                int miPos = Convert.ToInt32(miData[0]);

                return miAbanicoRotonda.getLstTramosByPosition(miPos)[0];
            }
            else
            {
                throw new Exception(strError.eTramoSalidaRotonda);
            }

        }
        #endregion
    }



}
