using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;


namespace tadLayLogica.datos.proyecto
{
    using tadLayLan;
    using tadLayData;
    using engNet.ClassT;
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayShare;
    using System.IO;

    public class oDalTbSolucion
    {



        /// <summary>
        /// GET SOLUCION BY ID
        /// </summary>
        public static dsApp.tbSolucionRow getSolucion(Guid iIdSol)
        {


            dsApp.tbSolucionRow miRow = oSingletonDsApp.getInstance.getSolucion(iIdSol);


            if (miRow == null)
            {
                throw new oExRowNotFound(iIdSol.ToString(), strError.eTablaSolucion);
            }

            return miRow;

        }












        public static void deleteSolucion(Guid iIdSol, bool iShowInfo)
        {
            getSolucion(iIdSol).Delete();
            oSingletonDsApp.getInstance.saveDataTable(oSingletonDsApp.getInstance.dataset.tbSolucion, iShowInfo);
        }
        public static void deleteSolucionbyName(dsApp.tbSolucionDataTable iTabla, string iSolName)
        {

            var miQuery = from p in iTabla
                          where p.nombre == iSolName
                          select p;

            if (miQuery.Count() > 0)
            {
                miQuery.First().Delete();
                iTabla.AcceptChanges();
            }
        }


        /// <summary>
        ///ADD EJE BASICO
        /// </summary>
        public static Guid addEjeBasico(string iSolucionNombre,
                                        string iEjeBasico3DHandle,
                                        string iEjeBasico2DHandle,
                                        oRoadDes iRoadDesign,
                                        oRoadPendientes iRoadPendientes,
                                        oCoeMinoracionAlturasMaximas iCoeMinAlturasMaximas,
                                        bool amilia=false)
        {

            //Borro las Solución en Caso Exista
            oDalTbSolucion.deleteSolucionbyName(oSingletonDsApp.getInstance.dataset.tbSolucion, iSolucionNombre);



            #region "Tabla Solucion"

            dsApp.tbSolucionRow miRow = oSingletonDsApp.getInstance.dataset.tbSolucion.NewtbSolucionRow();

            Guid miGuid = Guid.NewGuid();
            miRow.id = miGuid;
            miRow.nombre = iSolucionNombre;
            miRow.handleEjeBasico3D = iEjeBasico3DHandle;
            miRow.handleEjeBasico2D = iEjeBasico2DHandle;
            miRow.isCompleteEjeBasico = true;
            miRow.amilia = amilia;
            if (amilia)
            {
                /*miRow.isCompleteEjeBasico = true;
                miRow.isCompleteEjeTrazado = true;

                miRow.isCompletePerfil = true;*/
            }
            

            oSingletonDsApp.getInstance.dataset.tbSolucion.AddtbSolucionRow(miRow);

            oSingletonDsApp.getInstance.dataset.tbSolucion.AcceptChanges();

            #endregion

            #region "SolucionRoad"

            dsApp.tbSolucionRoadRow miRowRoad = oSingletonDsApp.getInstance.dataset.tbSolucionRoad.NewtbSolucionRoadRow();
            miRowRoad.id = miGuid;
            miRowRoad.grupoCarretera = iRoadDesign.grupo.ToString();
            miRowRoad.velocidadProyecto = iRoadDesign.Vp;
            miRowRoad.radioProyecto = iRoadDesign.Rp;
            miRowRoad.preferenciasRectasCurvas = iRoadDesign.preferencias.ToString();
            miRowRoad.preferenciaKv = iRoadDesign.preferenciasKv.ToString();
            miRowRoad.kvConvexo = iRoadDesign.kvConvexo;
            miRowRoad.kvConcavo = iRoadDesign.kvConcavo;
            miRowRoad.allowReduccionesVelocidad = iRoadDesign.allowPermitirReduccionesVelocidad;
            miRowRoad.isAijConstante = iRoadDesign.IsAijK;
            miRowRoad.peraltePC = iRoadDesign.peralte;
            miRowRoad.Aijminimo = iRoadDesign.AijMin;
            miRowRoad.AijMinSalidaLlegada = iRoadDesign.AijMinSalidaLlegada;

            oSingletonDsApp.getInstance.dataset.tbSolucionRoad.AddtbSolucionRoadRow(miRowRoad);
            oSingletonDsApp.getInstance.dataset.tbSolucionRoad.AcceptChanges();

            #endregion

            #region "SolucionPendientes"


            dsApp.tbSolucionPendienteRow miRowPendiente = oSingletonDsApp.getInstance.dataset.tbSolucionPendiente.NewtbSolucionPendienteRow();

            miRowPendiente.id = miGuid;
            miRowPendiente.calzadaPendienteMaximaProyectoPC = iRoadPendientes.calzadaPendienteProyectoMaximaPC;
            miRowPendiente.calzadaPendienteMinimaProyectoPC = iRoadPendientes.calzadaPendienteProyectoMinimaPC;
            miRowPendiente.calzadaCoeMinoracionPendienteMaximaProyecto = iRoadPendientes.coeMinoracionCalzadaPendienteMaxima;

            miRowPendiente.estructurasPendienteMaximaProyectoPC = iRoadPendientes.estructurasPendienteProyectoMaximaPC;
            miRowPendiente.estructuraPendienteMinimaProyectoPC = iRoadPendientes.estructurasPendienteProyectoMinimaPC;
            miRowPendiente.estructuraCoeMinoracionPendienteMaximaProyecto = iRoadPendientes.coeMinoracionEstructurasPendienteMaxima;

            oSingletonDsApp.getInstance.dataset.tbSolucionPendiente.AddtbSolucionPendienteRow(miRowPendiente);
            oSingletonDsApp.getInstance.dataset.tbSolucionPendiente.AcceptChanges();

            #endregion

            #region "Solucion Coeficientes Minoracion"

            dsApp.tbSolucionCoeMinAlturasRow miRowCoeMin = oSingletonDsApp.getInstance.dataset.tbSolucionCoeMinAlturas.NewtbSolucionCoeMinAlturasRow();

            miRowCoeMin.id = miGuid;
            miRowCoeMin.desmonteCoeMinoracionAlturaMaximaProyecto = iCoeMinAlturasMaximas.desmonteCoeficienteMinoracionAlturaMaxima;
            miRowCoeMin.terraplenCoeMinoracionAlturaMaximaProyecto = iCoeMinAlturasMaximas.terraplenCoeficienteMinoracionAlturaMaxima;
            miRowCoeMin.puenteCoeMinoracionAlturaMaximaPilaProyecto = iCoeMinAlturasMaximas.pilaCoeficienteMinoracionAlturaMaxima;


            oSingletonDsApp.getInstance.dataset.tbSolucionCoeMinAlturas.AddtbSolucionCoeMinAlturasRow(miRowCoeMin);
            oSingletonDsApp.getInstance.dataset.tbSolucionCoeMinAlturas.AcceptChanges();

            #endregion


            //SAVE SIN MOSTRAR INFO
            oSingletonDsApp.getInstance.saveSinAccepChanges();

            return miGuid;


        }

        /// <summary>
        ///ADD EJE TRAZADO
        /// </summary>
        public static void addEjeTrazado(Guid iIdSolucion, string iHandleEjeTrazado, double iLongitudKm, bool isEjeTijera = false)
        {

            dsApp.tbSolucionRow miRow = getSolucion(iIdSolucion);

            miRow.handleEjeTrazado = iHandleEjeTrazado;
            miRow.longitudKm = iLongitudKm;
            miRow.isCompleteEjeTrazado = true;
            miRow.isEjeTijera = isEjeTijera;

            miRow.AcceptChanges();

            oSingletonDsApp.getInstance.dataset.tbSolucion.AcceptChanges();

            oSingletonDsApp.getInstance.saveSinAccepChanges();
        }
        /// <summary>
        ///ADD PERFIL LONGITUDINAL ADD HANDLE
        /// </summary>
        public static void addPerfilLongitudinal(Guid iIdSolucion, string iHandlePerfilRasante)
        {

            dsApp.tbSolucionRow miRow = getSolucion(iIdSolucion);

            miRow.handlePerfil = iHandlePerfilRasante;
            miRow.isCompletePerfil = true;
            miRow.AcceptChanges();

            oSingletonDsApp.getInstance.dataset.tbSolucion.AcceptChanges();

            oSingletonDsApp.getInstance.saveSinAccepChanges();
        }
        public static void addExpropiacionHandle(Guid iIdSol, string iHandleLwExpropiacion)
        {
            oSingletonDsApp.getInstance.dataset.tbSolucion.FindByid(iIdSol).handleExpropiacion = iHandleLwExpropiacion;
            oSingletonDsApp.getInstance.dataset.tbSolucion.AcceptChanges();
            oSingletonDsApp.getInstance.saveSinAccepChanges();
        }


        public static List<dsApp.tbSolucionRow> getListadoSolucionesCompletas()
        {

            List<dsApp.tbSolucionRow> miLstRow = new List<dsApp.tbSolucionRow>();

            foreach (dsApp.tbSolucionRow item in oSingletonDsApp.getInstance.dataset.tbSolucion.Rows)
            {
                miLstRow.Add(item);
            }

            return miLstRow;

        }



        public static oRoadDes getRoadDesign(Guid iIdSolucion)
        {

            dsApp.tbSolucionRoadRow miRow = oSingletonDsApp.getInstance.dataset.tbSolucionRoad.FindByid(iIdSolucion);


            if (miRow == null)
            {
                throw new oExRowNotFound(iIdSolucion.ToString(), "tbRoadSolucion");
            }
            else
            {



                oRoadDes miRoad = new oRoadDes(oExtensionEnumeraciones.getRoadGrupo(miRow.grupoCarretera),
                                                miRow.velocidadProyecto,
                                                miRow.radioProyecto,
                                                oExtensionEnumeraciones.getRoadPreferencias(miRow.preferenciasRectasCurvas),
                                                miRow.isAijConstante,
                                                miRow.allowReduccionesVelocidad,
                                                oExtensionEnumeraciones.getKvPreferencias(miRow.preferenciaKv),
                                                miRow.kvConvexo,
                                                miRow.kvConcavo,
                                                miRow.peraltePC,
                                                miRow.Aijminimo,
                                                miRow.AijMinSalidaLlegada);


                return miRoad;

            }




        }


        public static void Guardar_EjeAmilia(Guid iIdSolucion, MemoryStream iHandleEjeTrazado)
        {
            dsApp.tbSolucionRow miRow = getSolucion(iIdSolucion);

            using (MemoryStream ms = iHandleEjeTrazado)
            {
                miRow.EjeTrazado_Amilia = iHandleEjeTrazado.ToArray();
            }
            
            miRow.AcceptChanges();

            oSingletonDsApp.getInstance.dataset.tbSolucion.AcceptChanges();

            oSingletonDsApp.getInstance.saveSinAccepChanges();
        }
        public static void Guardar_Alzado(Guid iIdSolucion, MemoryStream iHandleAlzado)
        {
            dsApp.tbSolucionRow miRow = getSolucion(iIdSolucion);
            using (MemoryStream ms = iHandleAlzado)
            {
                miRow.Alzado_Amilia = iHandleAlzado.ToArray();
            }
            //miRow.Alzado_Amilia = iHandleAlzado;

            miRow.AcceptChanges();

            oSingletonDsApp.getInstance.dataset.tbSolucion.AcceptChanges();

            oSingletonDsApp.getInstance.saveSinAccepChanges();
        }
        public static void Guardar_Perfil(Guid iIdSolucion, MemoryStream iHandlePerfil)
        {
            dsApp.tbSolucionRow miRow = getSolucion(iIdSolucion);
            using (MemoryStream ms = iHandlePerfil)
            {
                miRow.Perfil_Amilia = iHandlePerfil.ToArray();
            }
            //miRow.Perfil_Amilia = iHandlePerfil;

            miRow.AcceptChanges();

            oSingletonDsApp.getInstance.dataset.tbSolucion.AcceptChanges();

            oSingletonDsApp.getInstance.saveSinAccepChanges();
        }

    }
}
