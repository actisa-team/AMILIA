using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLogica.logica.EjeBasicoNew;
using Autodesk.AutoCAD.ApplicationServices;
using engCadNet;
using tadLayLogica.logica.EjeVisibilidad;
using tadLayLogica.zonaGis;
using Autodesk.AutoCAD.DatabaseServices;
using System.Diagnostics;

namespace tadLayLogica.Comandos
{
    public class oComandoEjeVisibilidadGlobal
    {
        public static Polyline create(oEjeBasicoSolucion iejeBasicoSol, double iSeparacion, double? iPendienteCota, double? iPendienteTriangulo,
           double angulomin, bool dibujar, bool preciso = false)
        {

            Stopwatch miMedicion = new Stopwatch();
            miMedicion.Start();

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                //Activo las Capas
                oTadil.data.Layer.zonaNoPasoUsuario.On();
                oTadil.data.Layer.zonaNoPasoPendiente.On();
                oTadil.data.Layer.zonasGisOn();

                //Creo el eje de visibilidad
                    oEjeVisibilidadGlobal miEjeGlobal = new oEjeVisibilidadGlobal(oSingletonProyecto.getInstance.ptoSalida.X,
                                                                       oSingletonProyecto.getInstance.ptoSalida.Y,
                                                                       oSingletonProyecto.getInstance.ptoLlegada.X,
                                                                       oSingletonProyecto.getInstance.ptoLlegada.Y, iSeparacion, 
                                                                       iPendienteCota, iPendienteTriangulo, iejeBasicoSol,dibujar,preciso);

                //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
                oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();

                List<Polyline> misZNP = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;
                List<Polyline> misZNPSinIntersecc = oLw.unirPolsInterccionanZNP(misZNP);

                miEjeGlobal.calcularEjeVisibilidad(misZNPSinIntersecc, oTadil.data.Layer.visibilidadGrafo.name, angulomin, dibujar, preciso);
            
                Polyline miEjeVisibilidadPol = miEjeGlobal.draw(oTadil.data.Layer.visibilidadEje);


                //Desactivo las Capas GIS
                oTadil.data.Layer.zonasGisOff();

                //Elimino el Grafo de Visibilidad
                //oTadil.data.Layer.visibilidadGrafo.deleteItems();


                //UI
                miMedicion.Stop();
                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);
                if (miEjeVisibilidadPol==null)
                {
                    oTadil.data.UserInfo.showInfo("No ha dado solución");
                }

                return miEjeVisibilidadPol;

            }


        }

    }
}
