using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Comandos
{

    using System.Diagnostics;


    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Colors;
    using tadLayLogica.zonaGis;

    using engCadNet;

    using tadLayLogica.EjeVisibilidad;
    using Autodesk.AutoCAD.Geometry;
    using tadLayShare.puntos;
    using tadLayLan.Tdi;
    using tadLayLan;



    public class oComandoEjeVisibilidad
    {
        /// <summary>
        /// Creación Eje Visibilidad Automático
        /// </summary>
        public static Polyline createAutomaticoConObstaculos()
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Activo las Capas
                oTadil.data.Layer.zonaNoPasoUsuario.On();
                oTadil.data.Layer.zonaNoPasoPendiente.On();
                oTadil.data.Layer.zonasGisOn();


                //Creo el Eje Visibilidad

                //Creo el Objeto que Crea el Eje de Visibilidad 
                oEjeVisibilidad miEjeVisibilidad = new oEjeVisibilidad(oSingletonProyecto.getInstance.ptoSalida.X,
                                                                       oSingletonProyecto.getInstance.ptoSalida.Y,
                                                                       oSingletonProyecto.getInstance.ptoLlegada.X,
                                                                       oSingletonProyecto.getInstance.ptoLlegada.Y);

                //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
                oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();

                List<Polyline> misZNP = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;

                //misZNP = oComandoSimplificarPolilinea.simplificarPolilineaEnvolvente(misZNP);

                List<Polyline> misZNPSinIntersecc = oLw.unirPolsInterccionanZNP(misZNP);
                //misZNPSinIntersecc = oComandoSimplificarPolilinea.simplificarPolilineaEnvolventeToPols(misZNPSinIntersecc);

                //Genero el Path
                miEjeVisibilidad.calcularEjeVisibilidad(misZNPSinIntersecc, oTadil.data.Layer.visibilidadGrafo.name);


                ////Represento el Path
                
                Polyline miEjeVisibilidadPol = miEjeVisibilidad.draw(oTadil.data.Layer.visibilidadEje);


                //Desactivo las Capas GIS
                oTadil.data.Layer.zonasGisOff();

                //Elimino el Grafo de Visibilidad
                oTadil.data.Layer.visibilidadGrafo.deleteItems();


                return miEjeVisibilidadPol;

            }

            


        }
        /// <summary>
        /// Selección Eje Visibilidad Usuario
        /// </summary>
        public static void selectUsuario()
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Selecciono la Entidad Generica
                Polyline miEjeVisibilidadUsuario = oSs.seleccionUsuario<Polyline>(strFrmSolucion.uiSeleccionaEjeVisibilidad, strFrmSolucion.uiNoSelPolilinea);

                if (miEjeVisibilidadUsuario != null)
                {

                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miEjeVisibilidadUsuario))
                    {
                        miLw.open();

                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oTadil.data.Layer.visibilidadEje.color);

                        miLw.entidad.Layer = oTadil.data.Layer.visibilidadEje.name;

                        miLw.save();
                    }

                    oTadil.data.Layer.visibilidadEje.On();

                    oTadil.data.UserInfo.procesoTerminado();
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }

            }


        }

        public static Polyline createAutomatico()
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Activo las Capas
                oTadil.data.Layer.zonaNoPasoUsuario.On();
                oTadil.data.Layer.zonaNoPasoPendiente.On();
                oTadil.data.Layer.zonasGisOn();

                /*
                 * se añade para capa de zona de no paso temporal ** juanma **
                 */
                oTadil.data.Layer.zonaNoPasoUsuario_Temp.On();

                //Logica Capas
                oTadil.data.Layer.visibilidadEje.On();
                oTadil.data.Layer.visibilidadEje.Current();
                oTadil.data.Layer.visibilidadEje.deleteItems();


                //Creo el Eje Visibilidad


                //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
                oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();

                List<Polyline> miLstObstacle = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;


                //Creo la polilinea desde puntoSalida hasta puntoLlegada

                Point3dCollection misPuntosES = new Point3dCollection();
                misPuntosES.Add(new Point3d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y, 0));
                misPuntosES.Add(new Point3d(oSingletonProyecto.getInstance.ptoLlegada.X, oSingletonProyecto.getInstance.ptoLlegada.Y, 0));

                Polyline miEjeVisibilidad = oLw.addLw2d(misPuntosES, false, oTadil.data.Layer.visibilidadEje.name);
                int miNumIntersecciones = 0;

                List<Polyline> misPolilineasAEliminar = new List<Polyline>();
                foreach (Polyline miObstaculo in miLstObstacle)
                {
                    List<Point2d> misPuntos = new List<Point2d>();
                    for (int i = 0; i < miObstaculo.NumberOfVertices; i++)
                    {
                        misPuntos.Add(miObstaculo.GetPoint2dAt(i));
                    }
                    misPuntos.Add(miObstaculo.GetPoint2dAt(0));
                    Polyline miPolCotaCero = oLw.addLw2d(misPuntos, false, "0");

                    Point3dCollection miColPtoInter = new Point3dCollection();
                    miEjeVisibilidad.IntersectWith(miPolCotaCero, Intersect.OnBothOperands, miColPtoInter, IntPtr.Zero, IntPtr.Zero);
                    miNumIntersecciones = miNumIntersecciones + miColPtoInter.Count;
                    misPolilineasAEliminar.Add(miPolCotaCero);

                }
                oLw.eliminarPolilineas(misPolilineasAEliminar);

                if (miNumIntersecciones == 0)
                {

                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miEjeVisibilidad))
                    {
                        miLw.open();

                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oTadil.data.Layer.visibilidadEje.color);

                        miLw.entidad.Layer = oTadil.data.Layer.visibilidadEje.name;

                        miLw.save();
                    }

                    oTadil.data.Layer.visibilidadEje.On();


                    //Elimino el Grafo de Visibilidad
                    oTadil.data.Layer.visibilidadGrafo.deleteItems();

                }
                else
                {
                    miEjeVisibilidad = oComandoEjeVisibilidad.createAutomaticoConObstaculos();
                }


                //Desactivo las Capas GIS
                oTadil.data.Layer.zonasGisOff();


                return miEjeVisibilidad;
            }
        }

        public static Polyline createAutomatico(double iPSalidaX, double iPSalidaY, double iPLlegadaX, double iPLlegadaY)
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Activo las Capas
                oTadil.data.Layer.zonaNoPasoUsuario.On();
                oTadil.data.Layer.zonaNoPasoPendiente.On();
                oTadil.data.Layer.zonasGisOn();




                //Logica Capas
                oTadil.data.Layer.visibilidadEje.On();
                oTadil.data.Layer.visibilidadEje.Current();
                oTadil.data.Layer.visibilidadEje.deleteItems();


                //Creo el Eje Visibilidad


                //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
                oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();

                List<Polyline> miLstObstacle = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;


                //Creo la polilinea desde puntoSalida hasta puntoLlegada

                Point3dCollection misPuntosES = new Point3dCollection();
                misPuntosES.Add(new Point3d(iPSalidaX, iPSalidaY, 0));
                misPuntosES.Add(new Point3d(iPLlegadaX, iPLlegadaY, 0));

                Polyline miEjeVisibilidad = oLw.addLw2d(misPuntosES, false, oTadil.data.Layer.visibilidadEje.name);
                int miNumIntersecciones = 0;

                List<Polyline> misPolilineasAEliminar = new List<Polyline>();
                foreach (Polyline miObstaculo in miLstObstacle)
                {
                    List<Point2d> misPuntos = new List<Point2d>();
                    for (int i = 0; i < miObstaculo.NumberOfVertices; i++)
                    {
                        misPuntos.Add(miObstaculo.GetPoint2dAt(i));
                    }
                    Polyline miPolCotaCero = oLw.addLw2d(misPuntos, false, "0");

                    Point3dCollection miColPtoInter = new Point3dCollection();
                    miEjeVisibilidad.IntersectWith(miPolCotaCero, Intersect.OnBothOperands, miColPtoInter, IntPtr.Zero, IntPtr.Zero);
                    miNumIntersecciones = miNumIntersecciones + miColPtoInter.Count;
                    misPolilineasAEliminar.Add(miPolCotaCero);

                }
                oLw.eliminarPolilineas(misPolilineasAEliminar);

                if (miNumIntersecciones == 0)
                {

                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miEjeVisibilidad))
                    {
                        miLw.open();

                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oTadil.data.Layer.visibilidadEje.color);

                        miLw.entidad.Layer = oTadil.data.Layer.visibilidadEje.name;

                        miLw.save();
                    }

                    oTadil.data.Layer.visibilidadEje.On();


                    //Elimino el Grafo de Visibilidad
                    oTadil.data.Layer.visibilidadGrafo.deleteItems();

                }
                else
                {
                    miEjeVisibilidad = oComandoEjeVisibilidad.createAutomaticoConObstaculos(iPSalidaX, iPSalidaY, iPLlegadaX, iPLlegadaY);
                }


                //Desactivo las Capas GIS
                oTadil.data.Layer.zonasGisOff();


                return miEjeVisibilidad;
            }
        }

        /// <summary>
        /// Creación Eje Visibilidad Automático
        /// </summary>
        public static Polyline createAutomaticoConObstaculos(double iPSalidaX, double iPSalidaY, double iPLlegadaX, double iPLlegadaY)
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Activo las Capas
                oTadil.data.Layer.zonaNoPasoUsuario.On();
                oTadil.data.Layer.zonaNoPasoPendiente.On();
                oTadil.data.Layer.zonasGisOn();


                //Creo el Eje Visibilidad

                //Creo el Objeto que Crea el Eje de Visibilidad 
                oEjeVisibilidad miEjeVisibilidad = new oEjeVisibilidad(iPSalidaX,
                                                                       iPSalidaY,
                                                                       iPLlegadaX,
                                                                       iPLlegadaY);

                //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
                oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();

                List<Polyline> misZNP = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;

                //misZNP = oComandoSimplificarPolilinea.simplificarPolilineaEnvolvente(misZNP);

                List<Polyline> misZNPSinIntersecc = oLw.unirPolsInterccionanZNP(misZNP);
                //misZNPSinIntersecc = oComandoSimplificarPolilinea.simplificarPolilineaEnvolventeToPols(misZNPSinIntersecc);

                //Genero el Path
                miEjeVisibilidad.calcularEjeVisibilidad(misZNPSinIntersecc, oTadil.data.Layer.visibilidadGrafo.name);


                ////Represento el Path

                Polyline miEjeVisibilidadPol = miEjeVisibilidad.draw(oTadil.data.Layer.visibilidadEje);


                //Desactivo las Capas GIS
                oTadil.data.Layer.zonasGisOff();

                //Elimino el Grafo de Visibilidad
                oTadil.data.Layer.visibilidadGrafo.deleteItems();


                return miEjeVisibilidadPol;

            }




        }

        public static Polyline modificaEjeDeVisibilidad(Polyline iEjeVisibilidad, List<Point2d> misPuntosCriticosZNP)
        {
            Point3dCollection misPuntosEje = new Point3dCollection();
            Polyline miNewEje = new Polyline();
            
            for (int i = 0; i < iEjeVisibilidad.NumberOfVertices; i++)
            {
                //miNewEje.AddVertexAt(i, new Point2d(iEjeVisibilidad.GetPoint2dAt(i).X, iEjeVisibilidad.GetPoint2dAt(i).Y), 0, 0, 0);
                misPuntosEje.Add(new Point3d(iEjeVisibilidad.GetPoint2dAt(i).X, iEjeVisibilidad.GetPoint2dAt(i).Y, 0));
            }

                using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
                {

                    //Activo las Capas
                    oTadil.data.Layer.zonaNoPasoUsuario.On();
                    oTadil.data.Layer.zonaNoPasoPendiente.On();
                    oTadil.data.Layer.zonasGisOn();



                    //Logica Capas
                    oTadil.data.Layer.visibilidadEje.On();
                    oTadil.data.Layer.visibilidadEje.Current();
                    oTadil.data.Layer.visibilidadEje.deleteItems();


                    //Creo el Eje Visibilidad


                    //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
                    oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();


                    double minY = misPuntosCriticosZNP.Min(pointCoordY => pointCoordY.Y);
                    var leftPoints = misPuntosCriticosZNP.Where(point => point.Y == minY);
                    double? miPk = oLw.getPkAtPoint(iEjeVisibilidad.getPointMasCercano(new Point3d(leftPoints.First().X, leftPoints.First().Y, 0)), iEjeVisibilidad);
                    int index = 0;
                    double? miPkVertice = oLw.getPkAtPoint(iEjeVisibilidad.GetPoint3dAt(index), iEjeVisibilidad);
                    while (miPk > miPkVertice && index < iEjeVisibilidad.NumberOfVertices - 1)
                    {
                        index++;
                        miPkVertice = oLw.getPkAtPoint(iEjeVisibilidad.GetPoint3dAt(index), iEjeVisibilidad);
                    }

                    misPuntosEje.Insert(index , new Point3d(leftPoints.First().X, leftPoints.First().Y, 0));
                    miNewEje = oLw.addLw2d(misPuntosEje, false, oTadil.data.Layer.visibilidadEje.name);

                    List<Polyline> miLstObstacle = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;
                    List<Polyline> miLstObstacleIntersectan = new List<Polyline>();

                    bool intersecc = false;
                    foreach (Polyline miObstaculo in miLstObstacle)
                    {

                        Point3dCollection miColPtoInter = new Point3dCollection();
                        miNewEje.IntersectWith(miObstaculo, Intersect.OnBothOperands, miColPtoInter, IntPtr.Zero, IntPtr.Zero);
                        if (miColPtoInter.Count != 0)
                        {
                            intersecc = true;
                        }
                    }
                    if (intersecc)
                    {
                        double maxY = misPuntosCriticosZNP.Max(pointCoordY => pointCoordY.Y);
                        var rigthPoints = misPuntosCriticosZNP.Where(point => point.Y == maxY);

                        misPuntosEje.RemoveAt(index);
                        misPuntosEje.Insert(index, new Point3d(rigthPoints.First().X, rigthPoints.First().Y, 0));

                        oTadil.data.Layer.visibilidadEje.deleteItems();
                        miNewEje = oLw.addLw2d(misPuntosEje, false, oTadil.data.Layer.visibilidadEje.name);
                    }


                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miNewEje))
                    {
                        miLw.open();

                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oTadil.data.Layer.visibilidadEje.color);

                        miLw.entidad.Layer = oTadil.data.Layer.visibilidadEje.name;

                        miLw.save();
                    }

                    oTadil.data.Layer.visibilidadEje.On();


                    //Elimino el Grafo de Visibilidad
                    oTadil.data.Layer.visibilidadGrafo.deleteItems();


                    //Desactivo las Capas GIS
                    oTadil.data.Layer.zonasGisOff();
                }


            return miNewEje;
        }
    }
}
