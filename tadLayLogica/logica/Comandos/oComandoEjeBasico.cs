using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Comandos
{

    using System.Diagnostics;


    using tadLayShare.puntos;
    using tadLayLogica.logica.EjeBasicoNew;
 
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;

    using tadLayLan;
    using tadLayLogica.zonaGis;

    using engCadNet;
    
    public class oComandoEjeBasico
    {


        public static Polyline create(oEjeBasicoSolucion iEjeBasicoSolucion, int iRepeticionesMax, bool iAutoCorrecion,bool dibujar)
        {
            Polyline miLwEjeVisibilidad = null;
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                miLwEjeVisibilidad = engCadNet.oSs.seleccionUsuario<Polyline>(strGeneralUser.uiSelectEjeBasico, strGeneralUser.uiSelectIsNull);

                if (miLwEjeVisibilidad != null)
                {
                    //Reinicio las Zonas No Paso. 
                    oSingletonZonaNoPaso.getInstance.Dispose();


                    Stopwatch miMedicion = new Stopwatch();
                    miMedicion.Start();

                    iEjeBasicoSolucion.createEjesBasicos(miLwEjeVisibilidad, iRepeticionesMax, iAutoCorrecion,dibujar);

                    //UI
                    miMedicion.Stop();
                    oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);
                    
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }
            }
            return miLwEjeVisibilidad;
        }

        public static void createManual(oEjeBasicoSolucion iEjeBasicoSolucion)
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                Polyline miLwEjeVisibilidad = engCadNet.oSs.seleccionUsuario<Polyline>(strGeneralUser.uiSelectEjeBasico, strGeneralUser.uiSelectIsNull);

                if (miLwEjeVisibilidad != null)
                {

                    //Reinicio las Zonas No Paso.
                    oSingletonZonaNoPaso.getInstance.Dispose();


                    Stopwatch miMedicion = new Stopwatch();
                    miMedicion.Start();

                    iEjeBasicoSolucion.createEjesBasicosManual(miLwEjeVisibilidad);

                    //UI
                    miMedicion.Stop();
                    oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }
            }
        }



        public static void testTramoSalidaLlegada(bool iIsTramoSalida,oEjeBasicoSolucion iEjeBasicoSolucion)                                                                                           
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                oTadil.data.Layer.tramoSalidaLlegada.deleteItems();

                
                if (iIsTramoSalida)
                {

                    oP3dSalidaLlegada miPtoSalida = iEjeBasicoSolucion.ptoSalida;

                    oTramoEjeBasico miTramo = oFactoryTramoSalidaLlegada.createTramoSalidaLlegada(true,0,0,iEjeBasicoSolucion.ptoSalida, iEjeBasicoSolucion.roadDesign, iEjeBasicoSolucion.roadPendientes, iEjeBasicoSolucion.estudioData, iEjeBasicoSolucion.abanicoDesign.tramoAbanicoDiscretizacion, false);

                    miTramo.drawTramo3D(oTadil.data.Layer.tramoSalidaLlegada.name);

                    miTramo.drawTramo2D(oTadil.data.Layer.tramoSalidaLlegada.name);
                        
                }
                else
                {
                    oP3dSalidaLlegada miPtoLlegada = iEjeBasicoSolucion.ptoLlegada;

                    oTramoEjeBasico miTramo = oFactoryTramoSalidaLlegada.createTramoLlegada(iEjeBasicoSolucion.ptoLlegada, iEjeBasicoSolucion.roadDesign, iEjeBasicoSolucion.roadPendientes, iEjeBasicoSolucion.estudioData, iEjeBasicoSolucion.abanicoDesign.tramoAbanicoDiscretizacion);

                    miTramo.drawTramo3D(oTadil.data.Layer.tramoSalidaLlegada.name);

                    miTramo.drawTramo2D(oTadil.data.Layer.tramoSalidaLlegada.name);
                }

            }



        }


        /// <summary>
        /// CREAR ABANICO BY POINT
        /// </summary>
        public static void createAbanicoByPoint (oEjeBasicoSolucion iEjeBasicoSolucion)
                                                                                                                                                                                                                                                                              
        {


            //SetUp Objetos Abanico by Point
            oAbanicoByPoint.SetUpObject(iEjeBasicoSolucion.roadDesign, iEjeBasicoSolucion.roadPendientes, iEjeBasicoSolucion.abanicoDesign, iEjeBasicoSolucion.estudioData, iEjeBasicoSolucion.ptoLlegada);
            oAbanicoByPoint.showInfoTramos();


            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                oTadil.data.Layer.abanicoTramos.Off();
                oTadil.data.Layer.abanicoSecciones.Off();


                //Comprobar la formula de angulo 3 puntos
                Point3d iPtoPrevio = oCadManager.thisEditor.GetPoint("PuntoPrevio").Value;
                Point3d iPtoOrigen = oCadManager.thisEditor.GetPoint("PuntoOrigenAbanico").Value; ;
                Point3d iPtoTarget = oCadManager.thisEditor.GetPoint("PuntoTarget").Value;


              
                //Borro las entidades Anteriores
                engCadNet.oCadManager.thisEditor.WriteMessage("Eliminando Entidades Previas");


                oTadil.data.Layer.abanicoTramos.deleteItems();
                oTadil.data.Layer.abanicoSecciones.deleteItems();
         
                //Creo el Objeto Tramo
                oTramoEjeBasico miTramoPrevio = new oTramoEjeBasico(new oP3d(iPtoPrevio.ToArray()),
                                                                   new oP3d(iPtoOrigen.ToArray()),
                                                                   eTramoTipoEjeBasico.avanceCorto);
                miTramoPrevio.idTramo = 0;

                //Creo el Objeto Abanico
                int miIdAbanico = 1;


                //Creo el Objeto Abanico
                tadLayLogica.logica.EjeBasicoNew.oAbanicoByPoint miAbanicoByPoint = new tadLayLogica.logica.EjeBasicoNew.oAbanicoByPoint(miIdAbanico, miTramoPrevio, new oP2d(iPtoTarget.ToArray()));
                miAbanicoByPoint.calcularTramosGanadores(iEjeBasicoSolucion.tramosValoracion, false);

                
                ///Activo las Capas
                oTadil.data.Layer.abanicoTramos.On();
                oTadil.data.Layer.abanicoSecciones.On();
                oTadil.data.Layer.abanicoTramosGanadores.On();

                if (!miAbanicoByPoint.hasTramosGanadores)
                {
                    throw new Exception("Abanico con Todos los Tramos No Validos");
                }
               
                
                miAbanicoByPoint.drawAijMinMaxAvance(oTadil.data.Layer.abanicoTramos.name);
                miAbanicoByPoint.drawAbanicoTramos(oTadil.data.Layer.abanicoTramos.name);
                miAbanicoByPoint.drawAbanicoSecciones(oTadil.data.Layer.abanicoSecciones.name);
                miAbanicoByPoint.drawTramosGanadores(oTadil.data.Layer.abanicoTramosGanadores.name);
               
            }

        }


        public static void createProyeccionPuntoMedio(oEjeBasicoSolucion iEjeBasicoSolucion)
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                Polyline miLwEjeVisibilidad = engCadNet.oSs.seleccionUsuario<Polyline>(strGeneralUser.uiSelectEjeBasico, strGeneralUser.uiSelectIsNull);


                //Reinicio las Zonas No Paso.
                oSingletonZonaNoPaso.getInstance.Dispose();

                if (miLwEjeVisibilidad != null)
                {


                    Stopwatch miMedicion = new Stopwatch();
                    miMedicion.Start();

                    iEjeBasicoSolucion.createEjeBasicoProyeccionPuntoMedio(miLwEjeVisibilidad);

                    //UI
                    miMedicion.Stop();
                    oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }
            }
        }

      


     


    }
}
