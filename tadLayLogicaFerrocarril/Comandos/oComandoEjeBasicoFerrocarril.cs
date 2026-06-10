using System.Diagnostics;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using engCadNet;
using tadLayLogica;
using tadLayLogica.zonaGis;
using tayLogicaTijera.data;
using tayLogicaTijera.EjeBasico;


namespace tayLogicaTijera.Comandos
{
    public class oComandoEjeBasicoFerrocarril
    {


        public static Polyline create(oEjeBasicoFerrocarril iEjeBasicoSolucion, oEjeTijeraData tijeraData,bool dibujar)
        {
            Polyline Resultado = null;

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                Polyline miLwEjeVisibilidad = engCadNet.oSs.seleccionUsuario<Polyline>("seleccione eje de visibilidad",
                    "la seleccion es nula");
                RemoveLw2dByLayer(oTadil.data.Layer.zonaNoPasoUsuario_Temp.name);

                //Reinicio las Zonas No Paso. 
                oSingletonZonaNoPaso.getInstance.Dispose();


                Stopwatch miMedicion = new Stopwatch();
                miMedicion.Start();

                if (!tijeraData.IsEnvolvente)
                {

                    Resultado = iEjeBasicoSolucion.createEjesBasicos(miLwEjeVisibilidad, tijeraData,
                        tadLayLogica.logica.EjeBasicoNew.oTarget.eFiltro.MaxMin,dibujar);
                }
                else
                {
                    if (tijeraData.DistEntronquePC > 50)
                    {
                        tijeraData.DistEntronquePC = 100 - tijeraData.DistEntronquePC;
                        oEjeBasicoFerrocarril ejeBasicoMAX =
                            new oEjeBasicoFerrocarril(iEjeBasicoSolucion.solucionPrefijo,
                                tijeraData.IsEnvolvente, iEjeBasicoSolucion.roadDesign, iEjeBasicoSolucion.roadPendientes,
                                iEjeBasicoSolucion.coeMinoracionAlturasMaximas, iEjeBasicoSolucion.tramosValoracion,
                                iEjeBasicoSolucion.abanicoDesign, iEjeBasicoSolucion.estudioData,
                                (eEstudioTipo)iEjeBasicoSolucion.estudioTipo, tijeraData.DistEntronque, tijeraData.DistConvergencia,
                                tijeraData.PenalizaTramosCortosEntronque);

                        Resultado = ejeBasicoMAX.createEjesBasicos(miLwEjeVisibilidad, tijeraData,
                            tadLayLogica.logica.EjeBasicoNew.oTarget.eFiltro.Max);
                    }
                    else
                    {


                        oEjeBasicoFerrocarril ejeBasicoMIN =
                            new oEjeBasicoFerrocarril(iEjeBasicoSolucion.solucionPrefijo,
                               tijeraData.IsEnvolvente, iEjeBasicoSolucion.roadDesign, iEjeBasicoSolucion.roadPendientes,
                                iEjeBasicoSolucion.coeMinoracionAlturasMaximas, iEjeBasicoSolucion.tramosValoracion,
                                iEjeBasicoSolucion.abanicoDesign, iEjeBasicoSolucion.estudioData,
                                (eEstudioTipo)iEjeBasicoSolucion.estudioTipo, tijeraData.DistEntronque, tijeraData.DistConvergencia,
                                tijeraData.PenalizaTramosCortosEntronque);

                        Resultado=ejeBasicoMIN.createEjesBasicos(miLwEjeVisibilidad,tijeraData,
                            tadLayLogica.logica.EjeBasicoNew.oTarget.eFiltro.Min);
                    }
                }
               
                //UI
                miMedicion.Stop();
                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);
            }
            return Resultado;
        }
        public static void RemoveLw2dByLayer(string iLayer)
        {
            using (Transaction tr = oCadManager.StartTransaction())
            {
                // Obtener la tabla de bloques
                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                // Obtener el registro del espacio modelo (ModelSpace)
                BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                // Iterar sobre las entidades en el espacio modelo
                foreach (ObjectId objId in acBlockTableRec)
                {
                    Entity ent = tr.GetObject(objId, OpenMode.ForWrite) as Entity;

                    // Verificar si la entidad es una polilínea y está en el layer especificado
                    if (ent is Polyline && ent.Layer == iLayer)
                    {
                        ent.Erase(); // Eliminar la polilínea
                    }
                }

                tr.Commit(); // Confirmar cambios
            }
        }
    }
}