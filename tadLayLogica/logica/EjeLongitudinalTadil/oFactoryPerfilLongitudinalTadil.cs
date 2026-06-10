using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeLongitudinalTadil
{
    
     //CAD
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;

    using PerfilLongitudinal;
    using EjeDeTrazado.puntosDelEje;

    using tadLayShare.puntos;

    using engCadNet;
    using tadLayLogica.datos.proyecto;
    
    public class oFactoryPerfilLongitudinalTadil 
    {

        /// <summary>
        /// FACTORY ALZADO FROM ID SOLUCION
        /// </summary>
        public static Alzado getAlzado (Guid iIdSolucion, EjeTrazado iEjeTrazado, oP3d iPuntoLlegada)
        {

            Alzado miAlzado = null;
            List<Punto3d> miLstPuntoEjeBasico3d=null;
            Polyline miEjeTrazadoPol = null;
            EjeTrazado miEjeTrazado =null;
            double? miKvConvexo = null;
            double? miKvConcavo  = null;

            double? miVp = null;

            bool? miIsPendienteSalidaDefinida =null;
            double? miPendienteSalidaPorUno = null;

            bool? miIsPendienteLlegadaDefinida =null;
            double? miPendienteLlegadaPorUno = null;

            #region "Listado de Vertices - Kv"

            Polyline3d miLwEjeBasico3d;

            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {
                miVp = miSolucion.roadDesign.Vp;
                miLwEjeBasico3d = miSolucion.ejeBasico3D;
                miKvConvexo= miSolucion.roadDesign.kvConvexo;
                miKvConcavo = miSolucion.roadDesign.kvConcavo;
                miEjeTrazadoPol = miSolucion.ejeTrazado;
            }

            List<Point3d> miListadoPuntosCad = oLw.getLstPtoFromLw3d(miLwEjeBasico3d);

             miLstPuntoEjeBasico3d = miListadoPuntosCad.ConvertAll(p => new tadLayShare.puntos.Punto3d(p.X, p.Y, p.Z));

             if (iPuntoLlegada.Z == 0)
             {
                 oSingletonTerreno miTerreno = oSingletonTerreno.getInstance;
                 //double? miCota = miTerreno.getZFromXY(miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1].coordenadaX, miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1].coordenadaY);
                 double? miCota = miTerreno.getZFromXY(iPuntoLlegada.X, iPuntoLlegada.Y);
                 if (miCota != null)
                 {
                     //Punto3d miPuntoFinal = miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1];
                     Punto3d miPuntoFinalCota = new Punto3d(iPuntoLlegada.X, iPuntoLlegada.Y, (double)miCota);
                     miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1] = miPuntoFinalCota;
                 }
             }
             else
             {
                 Punto3d miPuntoFinal = miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1];
                 Punto3d miPuntoFinalCota = new Punto3d(miPuntoFinal.coordenadaX, miPuntoFinal.coordenadaY, iPuntoLlegada.Z);
                 miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1] = miPuntoFinalCota;
             }

            #endregion

            #region "EjeTrazado"

            miEjeTrazado = iEjeTrazado;

            #endregion

            #region "Tramos Salida Llegada"

            if (oSingletonProyecto.getInstance.ptoSalida.isDefinidaPendiente)
            {
                miIsPendienteSalidaDefinida=true;
                miPendienteSalidaPorUno = oSingletonProyecto.getInstance.ptoSalida.pendientePC.Value/100.0;
            }
            else
            {
                 miIsPendienteSalidaDefinida=false;
                miPendienteSalidaPorUno = 0;
            }


             if (oSingletonProyecto.getInstance.ptoLlegada.isDefinidaPendiente)
            {
                miIsPendienteLlegadaDefinida=true;
                miPendienteLlegadaPorUno = oSingletonProyecto.getInstance.ptoLlegada.pendientePC.Value / 100.0;
            }
            else
            {
                miIsPendienteLlegadaDefinida=false;
                miPendienteLlegadaPorUno = 0;
            }

            #endregion

            //var puntosterreno= oSingletonPuntosTerreno.getInstance;

             miAlzado = new Alzado(miEjeTrazadoPol, 
                                  miLstPuntoEjeBasico3d,
                                  oSingletonTerreno.getInstance.getZFromXY,
                                  miIsPendienteSalidaDefinida.Value,
                                  miIsPendienteLlegadaDefinida.Value,
                                  miPendienteSalidaPorUno.Value,
                                  miPendienteLlegadaPorUno.Value,
                                  miVp.Value,
                                  miKvConcavo.Value,
                                  miKvConvexo.Value,
                                  oSingletonProyecto.getInstance.seccionIntervalo, oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);


             

            return miAlzado;

        }

        /// <summary>
        /// FACTORY GUITARRA FROM ALZADO -->Valore Defecto EscalaAncho->100, EscalaAlto -->10
        /// </summary>
        public static Guitarra getGuitarra (Point3d iPtoInsert, Alzado iAlzado, int iEscalaAncho, int iEscalaAlto)
        {

            Punto3d miPtoInsert = new Punto3d(iPtoInsert.X,iPtoInsert.Y,0);

            Guitarra miGuitarra = new Guitarra(0, iAlzado.getMaxPk, iAlzado.getMinZ, iAlzado.getMaxZ, miPtoInsert, iEscalaAncho, iEscalaAlto);

            return miGuitarra;

        }

        

        public static Alzado getAlzadoFromListPoint3d(Guid iIdSolucion, EjeTrazado iEjeTrazado, List<Point3d> iVertices3d, oP3d iPuntoLlegada, oP3d iPuntoSalida, bool iPtoIniMod, bool iPtoFinMod)
        {

            Alzado miAlzado = null;
            List<Punto3d> miLstPuntoEjeBasico3d = null;
            Polyline miEjeTrazadoPol = null;
            EjeTrazado miEjeTrazado = null;
            double? miKvConvexo = null;
            double? miKvConcavo = null;

            double? miVp = null;


            bool? miIsPendienteSalidaDefinida = null;
            double? miPendienteSalidaPorUno = null;

            bool? miIsPendienteLlegadaDefinida = null;
            double? miPendienteLlegadaPorUno = null;

            #region "Listado de Vertices - Kv"

            Polyline3d miLwEjeBasico3d;

            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {
                miVp = miSolucion.roadDesign.Vp;
                miLwEjeBasico3d = miSolucion.ejeBasico3D;
                miKvConvexo = miSolucion.roadDesign.kvConvexo;
                miKvConcavo = miSolucion.roadDesign.kvConcavo;
                miEjeTrazadoPol = miSolucion.ejeTrazado;
            }

            //List<Point3d> miListadoPuntosCad = oLw.getLstPtoFromLw3d(miLwEjeBasico3d);

            miLstPuntoEjeBasico3d = iVertices3d.ConvertAll(p => new tadLayShare.puntos.Punto3d(p.X, p.Y, p.Z));



            #endregion

            #region "EjeTrazado"

            miEjeTrazado = iEjeTrazado;

            #endregion

            #region "Tramos Salida Llegada"

            if (oSingletonProyecto.getInstance.ptoSalida.isDefinidaPendiente)
            {
                miIsPendienteSalidaDefinida = true;
                miPendienteSalidaPorUno = oSingletonProyecto.getInstance.ptoSalida.pendientePC.Value / 100.0;
            }
            else
            {
                miIsPendienteSalidaDefinida = false;
                miPendienteSalidaPorUno = 0;
            }


            if (oSingletonProyecto.getInstance.ptoLlegada.isDefinidaPendiente)
            {
                miIsPendienteLlegadaDefinida = true;
                miPendienteLlegadaPorUno = oSingletonProyecto.getInstance.ptoLlegada.pendientePC.Value / 100.0;
            }
            else
            {
                miIsPendienteLlegadaDefinida = false;
                miPendienteLlegadaPorUno = 0;
            }


            if ((!(bool)miIsPendienteLlegadaDefinida) && (!iPtoFinMod))
            {
                oSingletonTerreno miTerreno = oSingletonTerreno.getInstance;
                double? miCota = miTerreno.getZFromXY(miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1].coordenadaX, miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1].coordenadaY);
                if (miCota != null)
                {
                    Punto3d miPuntoFinal = miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1];
                    Punto3d miPuntoFinalCota = new Punto3d(miPuntoFinal.coordenadaX, miPuntoFinal.coordenadaY, (double)miCota);
                    miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1] = miPuntoFinalCota;
                }
            }
            else if (((bool)miIsPendienteLlegadaDefinida) && (!iPtoFinMod))
            {
                Punto3d miPuntoFinal = miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1];
                Punto3d miPuntoFinalCota = new Punto3d(miPuntoFinal.coordenadaX, miPuntoFinal.coordenadaY, iPuntoLlegada.Z);
                miLstPuntoEjeBasico3d[miLstPuntoEjeBasico3d.Count - 1] = miPuntoFinalCota;
            }


            if ((!(bool)miIsPendienteSalidaDefinida) && (!iPtoIniMod))
            {
                oSingletonTerreno miTerreno = oSingletonTerreno.getInstance;
                double? miCota = miTerreno.getZFromXY(miLstPuntoEjeBasico3d[0].coordenadaX, miLstPuntoEjeBasico3d[0].coordenadaY);
                if (miCota != null)
                {
                    Punto3d miPuntoIni = miLstPuntoEjeBasico3d[0];
                    Punto3d miPuntoIniCota = new Punto3d(miPuntoIni.coordenadaX, miPuntoIni.coordenadaY, (double)miCota);
                    miLstPuntoEjeBasico3d[0] = miPuntoIniCota;
                }
            }
            else if (((bool)miIsPendienteSalidaDefinida) && (!iPtoIniMod))
            {
                Punto3d miPuntoIni = miLstPuntoEjeBasico3d[0];
                Punto3d miPuntoIniCota = new Punto3d(miPuntoIni.coordenadaX, miPuntoIni.coordenadaY, iPuntoSalida.Z);
                miLstPuntoEjeBasico3d[0] = miPuntoIniCota;
            }

            #endregion



            miAlzado = new Alzado(miEjeTrazadoPol, 
                                 miLstPuntoEjeBasico3d,
                                 oSingletonTerreno.getInstance.getZFromXY,
                                 miIsPendienteSalidaDefinida.Value,
                                 miIsPendienteLlegadaDefinida.Value,
                                 miPendienteSalidaPorUno.Value,
                                 miPendienteLlegadaPorUno.Value,
                                 miVp.Value,
                                 miKvConcavo.Value,
                                 miKvConvexo.Value,
                                 oSingletonProyecto.getInstance.seccionIntervalo, oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);




            return miAlzado;

        }
    }
}
