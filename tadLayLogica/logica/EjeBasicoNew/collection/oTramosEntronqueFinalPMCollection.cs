using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using tadLayShare.puntos;
using tadLayLogica.estudioTipo;
using tadLayShare;

namespace tadLayLogica.logica.EjeBasicoNew.collection
{
    public class oTramosEntronqueFinalPMCollection
    {
        oTramoAbanicoCollection mInicioTramoLista = new oTramoAbanicoCollection();
        oTramoAbanicoCollection mFinTramoLista = new oTramoAbanicoCollection();

        private static oRoadDes mRoadDesign = null;
        private static oRoadPendientes mRoadPendiente = null;

        private static oAbanicoDesign mAbanicoDesign = null;
        private static IEstudio mEstudioDatos = null;

        private oTramosValoracion mValoracionesGlobales = null;



        public oTramosEntronqueFinalPMCollection()
        {
        }

        public static void SetUpObject(oRoadDes iRoadDesign, oRoadPendientes iRoadPendiente, oAbanicoDesign iDataAbanico, IEstudio iEstudioDatos)
        {
            mRoadDesign = iRoadDesign;
            mRoadPendiente = iRoadPendiente;
            mAbanicoDesign = iDataAbanico;
            mEstudioDatos = iEstudioDatos;
        }

        public void createTramosEntronqueFinalPM(int iIdAbanico, oTramosValoracion iValoracionesGlobales, List<Polyline> iConexiones, oTramoEjeBasico iTramoPrevioSalida, oTramoEjeBasico iTramoPrevioLlegada)
        {
            List<oTramoEjeBasico> misTramosGanadores = new List<oTramoEjeBasico>();

            Point2d miPuntoInicio = new Point2d();
            Point2d miPuntoMedio = new Point2d();
            Point2d miPuntoFin = new Point2d();

            int miNumTramo = 0;
            foreach (Polyline miConexion in iConexiones)
            {

                miPuntoInicio = miConexion.GetPoint2dAt(0);
                miPuntoMedio = miConexion.GetPoint2dAt(1);
                miPuntoFin = miConexion.GetPoint2dAt(2);

                oTramoAvanceCorto miTramoInicio = new oTramoAvanceCorto(eTramoTipoEjeBasico.avanceCorto, iIdAbanico, miNumTramo, iTramoPrevioSalida, oTrigo.getAzimutGrados(new oP2d(miPuntoInicio.X, miPuntoInicio.Y), new oP2d(miPuntoMedio.X, miPuntoMedio.Y)), miPuntoInicio.GetDistanceTo(miPuntoMedio), new oP2d(miPuntoFin.X, miPuntoFin.Y));
                mInicioTramoLista.Add(miTramoInicio);

                oTramoAvanceCorto miTramoFin = new oTramoAvanceCorto(eTramoTipoEjeBasico.avanceCorto, iIdAbanico + 1, miNumTramo, iTramoPrevioLlegada, oTrigo.getAzimutGrados(new oP2d(miPuntoFin.X, miPuntoFin.Y), new oP2d(miPuntoMedio.X, miPuntoMedio.Y)), miPuntoFin.GetDistanceTo(miPuntoMedio), new oP2d(miPuntoInicio.X, miPuntoInicio.Y));
                mFinTramoLista.Add(miTramoFin);

                miNumTramo = miNumTramo + 1;
            }

            oTramoAvanceCorto miTramo = new oTramoAvanceCorto(eTramoTipoEjeBasico.avanceCorto, iIdAbanico, miNumTramo, iTramoPrevioSalida, oTrigo.getAzimutGrados(new oP2d(miPuntoInicio.X, miPuntoInicio.Y), new oP2d(miPuntoFin.X, miPuntoFin.Y)), miPuntoInicio.GetDistanceTo(miPuntoFin), new oP2d(miPuntoFin.X, miPuntoFin.Y));
            mInicioTramoLista.Add(miTramo);


            mValoracionesGlobales = iValoracionesGlobales;
        }

        public List<oTramoEjeBasico> calcularTramosGanadores(oTramoEjeBasico iTramoPrevioSalida, oTramoEjeBasico iTramoPrevioLlegada, bool isEntronqueEspecial)
        {
            List<oTramoEjeBasico> misTramosGanadores = new List<oTramoEjeBasico>();
            
            foreach (var item in mInicioTramoLista)
            {
                item.validarAnguloEntreTramos();

                item.validarAijDesviacionesMaximas(mAbanicoDesign.invalidarTramosAvanceCortoPorIncrementoLongitud, mAbanicoDesign.invalidarTramosIncrementoLongitudPC.Value);

                item.validarAnguloEntreTramosAijMinimoMinimo(mRoadDesign.getAnguloMinimoTramoSiguiente);

                item.validarTramoP2InsideTerreno();

                item.validarTramoZonasNoPaso();

                item.validarCruceDPH(mEstudioDatos);

                item.validarDentroDPH(mEstudioDatos);

                item.validarDistanciaP2P1PuntoMedio(iTramoPrevioSalida.P1, item.P2, mRoadDesign.AijMin);

                if(item.isTramoValido)
                    item.createSeccion(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, isEntronqueEspecial);
            }

            foreach (var item in mFinTramoLista)
            {
                item.validarAnguloEntreTramos();

                item.validarAijDesviacionesMaximas(mAbanicoDesign.invalidarTramosAvanceCortoPorIncrementoLongitud, mAbanicoDesign.invalidarTramosIncrementoLongitudPC.Value);

                item.validarAnguloEntreTramosAijMinimoMinimo(mRoadDesign.getAnguloMinimoTramoSiguiente);

                item.validarTramoP2InsideTerreno();

                item.validarTramoZonasNoPaso();

                item.validarCruceDPH(mEstudioDatos);

                item.validarDentroDPH(mEstudioDatos);

                item.validarDistanciaP2P1PuntoMedio(iTramoPrevioLlegada.P1, item.P2, mRoadDesign.AijMin);

                if (item.isTramoValido)
                    item.createSeccion(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos,
                        isEntronqueEspecial);
            }



            for (int index = 0; index < mInicioTramoLista.Count - 1; index++)
            {
                if ((mInicioTramoLista[index].isTramoValido) && (mFinTramoLista[index].isTramoValido))
                {
                        oTramoAvanceCorto miTramo1 = (oTramoAvanceCorto)mInicioTramoLista[index];
                        oTramoAvanceCorto miTramo2 = (oTramoAvanceCorto)mFinTramoLista[index];
                    if (mInicioTramoLista[index].P2.Z != mFinTramoLista[index].P2.Z)
                    {

                        if (miTramo1.pendienteConSignoPC == miTramo1.P1P2terrenoPendienteConSignoPC)
                        {
                            oTramoAvanceCorto miNuevoTramo1 = new oTramoAvanceCorto();
                            miNuevoTramo1.P1 = miTramo1.P1;
                            miNuevoTramo1.P2 = miTramo2.P2;
                            miNuevoTramo1.ptoTarget = miTramo1.ptoTarget;
                            miNuevoTramo1.idPosicion = miTramo1.idPosicion;
                            miNuevoTramo1.idAbanico = miTramo1.idAbanico;
                            miNuevoTramo1.idTramo = miTramo1.idTramo;
                            miNuevoTramo1.tramoPrevio = miTramo1.tramoPrevio;
                            miNuevoTramo1.lstTramos = miTramo1.lstTramos;
                            miNuevoTramo1.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;

                            miNuevoTramo1.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);

                            int i = 0;
                            foreach (var item in miTramo1.seccion)
                            {
                                miNuevoTramo1.seccion[i].seccionTipo = item.seccionTipo;
                                i++;
                            }

                            mInicioTramoLista[index] = miNuevoTramo1;



                        }
                        else
                        {
                            if (miTramo2.pendienteConSignoPC == miTramo2.P1P2terrenoPendienteConSignoPC)
                            {
                                oTramoAvanceCorto miNuevoTramo2 = new oTramoAvanceCorto();
                                miNuevoTramo2.P1 = miTramo2.P1;
                                miNuevoTramo2.P2 = miTramo1.P2;
                                miNuevoTramo2.ptoTarget = miTramo2.ptoTarget;
                                miNuevoTramo2.idPosicion = miTramo2.idPosicion;
                                miNuevoTramo2.idAbanico = miTramo2.idAbanico;
                                miNuevoTramo2.idTramo = miTramo2.idTramo;
                                miNuevoTramo2.tramoPrevio = miTramo2.tramoPrevio;
                                miNuevoTramo2.lstTramos = miTramo2.lstTramos;
                                miNuevoTramo2.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;

                                miNuevoTramo2.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);


                                int i = 0;
                                foreach (var item in miTramo2.seccion)
                                {
                                    miNuevoTramo2.seccion[i].seccionTipo = item.seccionTipo;
                                    i++;
                                }

                                miNuevoTramo2.isTramoValido = true;
                                
                                mFinTramoLista[index] = miNuevoTramo2;


                            }
                            else
                            {
                                mInicioTramoLista[index].isTramoValido = false;
                                mInicioTramoLista[index].errorTramo = eTramoEjeBasicoError.entronqueFinalPuntoMedioCotasInvalido;
                                mFinTramoLista[index].isTramoValido = false;
                                mFinTramoLista[index].errorTramo = eTramoEjeBasicoError.entronqueFinalPuntoMedioCotasInvalido;
                            }
                        }
                        
                    }
                    if (index == 36 || index == 29)
                    {
                        double pendienteConSigno = mInicioTramoLista[index].pendienteConSignoPC;
                        double pendienteConSigno2 = mFinTramoLista[index].pendienteConSignoPC;
                        double pendientesinSigno = mInicioTramoLista[index].pendienteAbsolutaPC;
                        double pendientesinSigno2 = mFinTramoLista[index].pendienteAbsolutaPC;
                    }
                    if ((mInicioTramoLista[index].pendienteAbsolutaPC > mRoadPendiente.estructuraPendienteCalculoMaximoPC) && (mInicioTramoLista[index].pendienteAbsolutaPC > mRoadPendiente.estructuraPendienteCalculoMaximoPC))
                    {
                        mInicioTramoLista[index].isTramoValido = false;
                        mInicioTramoLista[index].errorTramo = eTramoEjeBasicoError.entronqueFinalPuntoMedioCotasInvalido;
                    }
                    if ((mFinTramoLista[index].pendienteAbsolutaPC > mRoadPendiente.estructuraPendienteCalculoMaximoPC) && (mFinTramoLista[index].pendienteAbsolutaPC > mRoadPendiente.estructuraPendienteCalculoMaximoPC))
                    {
                        mFinTramoLista[index].isTramoValido = false;
                        mFinTramoLista[index].errorTramo = eTramoEjeBasicoError.entronqueFinalPuntoMedioCotasInvalido;
                    }
                }
            }

            
            int miTramoGanador = -1;
            double miValoracionGanadora = -1;

            if ((mInicioTramoLista.existenTramosValidos()) && (mFinTramoLista.existenTramosValidos()))
            {

                mInicioTramoLista.createValoracion(mValoracionesGlobales);
                mFinTramoLista.createValoracion(mValoracionesGlobales);

                double?[] misValoracionesConjuntas = new double?[mInicioTramoLista.Count];


                for (int index = 0; index < mInicioTramoLista.Count-1; index++)
                {
                    if ((mInicioTramoLista[index].isTramoValido) && (mFinTramoLista[index].isTramoValido))
                    {
                            misValoracionesConjuntas[index] = mInicioTramoLista[index].valoracionPonderadaGlobal_0_10 + mFinTramoLista[index].valoracionPonderadaGlobal_0_10;
                            if (miValoracionGanadora < misValoracionesConjuntas[index])
                            {
                                miValoracionGanadora = (double)misValoracionesConjuntas[index];
                                miTramoGanador = index;
                            }
                    }
                    else
                    {
                        misValoracionesConjuntas[index] = null;
                    }
                }

                if (mInicioTramoLista[mInicioTramoLista.Count-1].isTramoValido)
                {
                    oTramoAvanceCorto miTramo1 = (oTramoAvanceCorto)mInicioTramoLista[mInicioTramoLista.Count-1];
                    if (miTramo1.P2.Z != iTramoPrevioLlegada.P2.Z)
                    {
                        if (miTramo1.pendienteConSignoPC == miTramo1.P1P2terrenoPendienteConSignoPC)
                        {
                            oTramoAvanceCorto miNuevoTramo1 = new oTramoAvanceCorto();
                            miNuevoTramo1.P1 = miTramo1.P1;
                            miNuevoTramo1.P2 = iTramoPrevioLlegada.P2;
                            miNuevoTramo1.ptoTarget = miTramo1.ptoTarget;
                            miNuevoTramo1.idPosicion = miTramo1.idPosicion;
                            miNuevoTramo1.idAbanico = miTramo1.idAbanico;
                            miNuevoTramo1.idTramo = miTramo1.idTramo;
                            miNuevoTramo1.tramoPrevio = miTramo1.tramoPrevio;
                            miNuevoTramo1.lstTramos = miTramo1.lstTramos;
                            miNuevoTramo1.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;
                            miNuevoTramo1.valoracionPonderadaGlobal_0_10 = miTramo1.valoracionPonderadaGlobal_0_10;

                            miNuevoTramo1.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);


                            int i = 0;
                            foreach (var item in miTramo1.seccion)
                            {
                                miNuevoTramo1.seccion[i].seccionTipo = item.seccionTipo;
                                i++;
                            }

                            mInicioTramoLista[mInicioTramoLista.Count - 1] = miNuevoTramo1;
                            mInicioTramoLista.createValoracion(mValoracionesGlobales);
                        }
                        else
                        {
                            mInicioTramoLista[mInicioTramoLista.Count - 1].isTramoValido = false;
                            mInicioTramoLista[mInicioTramoLista.Count - 1].errorTramo = eTramoEjeBasicoError.entronqueFinalPuntoMedioCotasInvalido;
                        }

                    }
                    if (mInicioTramoLista[mInicioTramoLista.Count - 1].isTramoValido)
                    {
                        misValoracionesConjuntas[mInicioTramoLista.Count - 1] = mInicioTramoLista[mInicioTramoLista.Count - 1].valoracionPonderadaGlobal_0_10 * 2;
                        if (miValoracionGanadora < misValoracionesConjuntas[mInicioTramoLista.Count - 1])
                        {
                            miValoracionGanadora = (double)misValoracionesConjuntas[mInicioTramoLista.Count - 1];
                            miTramoGanador = mInicioTramoLista.Count - 1;
                        }
                    }
                }

            }

            if (miTramoGanador == -1)
            {
                throw new oExTramoEntronqueNoCumple();
            }

            if (miTramoGanador == mInicioTramoLista.Count - 1)
            {
                misTramosGanadores.Add(mInicioTramoLista[miTramoGanador]);
            }
            else
            {
                misTramosGanadores.Add(mInicioTramoLista[miTramoGanador]);
                misTramosGanadores.Add(mFinTramoLista[miTramoGanador]);
            }

            foreach (oTramoAvanceCorto miTramoDraw in mInicioTramoLista)
            {
                miTramoDraw.drawTramo2D(oTadil.data.Layer.abanicoTramos.name);
            }


            foreach (oTramoAvanceCorto miTramoDraw in mFinTramoLista)
            {
                miTramoDraw.drawTramo2D(oTadil.data.Layer.abanicoTramos.name);
            }

            return misTramosGanadores;
        }

    }
}
