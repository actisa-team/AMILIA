using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using tadLayLogica.datos;
using tadLayShare;

namespace tadLayLogica.Comandos
{

    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    using System.ComponentModel;
    using tadLayLan;
    using tadLayLan.Tdi;
    using tadLayShare.puntos;
    using Terrenos;
    using dt = Autodesk.AutoCAD.DatabaseServices;


    public class oComandoTerreno
    {

        public static void crearAnalisis(double? iPendienteMayorPC)
        {

            if (iPendienteMayorPC == null || iPendienteMayorPC.Value == 0)
            {
                oTadil.data.UserInfo.showInfo(strFrmTerreno.uiPendienteCero);
                return;
            }


            double mySlope = (iPendienteMayorPC.Value) / 100;


            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Activo la Capa
                oTadil.data.Layer.analisisPendiente.On();

                //Elimino todas las entidades
                oTadil.data.Layer.analisisPendiente.deleteItems();

                //Genero el Ánalisis Previo [ANGELES]
                //oSingletonTerreno.getInstance.slopeAnalisisPreview(mySlope, oTadil.data.Layer.analisisPendiente.name);

                //Regenero el CAD
                engCadNet.oTools.regen();

                //UI
                oTadil.data.UserInfo.showInfo(strFrmTerreno.uiProcesoTerminado);
            }

        }

        public static void selectZonasNoPasoPendiente()
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Selecciono la Entidad Generica
                Polyline miLwUser = oSs.seleccionUsuario<Polyline>(strFrmTerreno.uiSelPolilineaNoPasoPend, strFrmTerreno.uiNoSelPolilinea);

                if (miLwUser != null)
                {


                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miLwUser))
                    {
                        miLw.open();

                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oTadil.data.Layer.zonaNoPasoPendiente.color);

                        miLw.entidad.Layer = oTadil.data.Layer.zonaNoPasoPendiente.name;

                        miLw.save();
                    }

                    oTadil.data.Layer.zonaNoPasoPendiente.Current();

                    oTadil.data.UserInfo.procesoTerminado();
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }

            }



        }


        public static void selectPolilineaEnvolvente(string iName)
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Selecciono la Entidad Generica
                //CAMBIAR, meter en LAN 
                Polyline miLwUser = oSs.seleccionUsuario<Polyline>(strGeneralUser.uiSelPolEnvolvente, strFrmTerreno.uiNoSelPolilinea);

                if (miLwUser != null)
                {
                    Entity miMalla = null;
                    try
                    {
                        miMalla = oSubDMesh.getPolyFaceMeshFromName(iName, oTadil.IsDemo);
                    }
                    catch 
                    {
                        miMalla = oSubDMesh.getSubDMeshFromName(iName, oTadil.IsDemo);
                    }
                    //Debo de Abrir el Objeto para poder cargarlo en la Capa
                    using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miLwUser))
                    {
                        miLw.open();

                        miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByColor, 1);
                        if (miMalla!=null)
                        {
                            miLw.entidad.Layer = miMalla.Layer;
                        }
                        

                        miLw.save();
                    }

                    oSingletonTerreno.setPolEnvolvente(miLwUser);

                    oTadil.data.UserInfo.procesoTerminado();
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }

            }



        }

        public static void createPolilineaEnvolvente(string iName)
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Selecciono la Entidad Generica
                //CAMBIAR, meter en LAN 
                Polyline miLwUser = oSingletonTerreno.getInstance.getPolEnvolvente();

                Entity miMalla = null;
                if (oTadil.IsDemo)
                {
                    try
                    {
                        miMalla = oSubDMesh.getPolyFaceMeshFromName(iName, oTadil.IsDemo);
                    }
                    catch
                    {
                        miMalla = oSubDMesh.getSubDMeshFromName(iName, oTadil.IsDemo);
                    }
                }
               

                //Debo de Abrir el Objeto para poder cargarlo en la Capa
                using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miLwUser))
                {
                    miLw.open();

                    miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByColor, 1);
                    if (oTadil.IsDemo)
                    {
                        miLw.entidad.Layer = miMalla.Layer;
                    }
                    

                    miLw.save();
                }

                oSingletonTerreno.setPolEnvolvente(miLwUser);

                oTadil.data.UserInfo.procesoTerminado();

            }



        }


        public static void añadirZonasDeNoPasoPendienteMaxima(List<Triangulo> isZonasNPPendiente)
        {
            if (oLayer.HasLayer(oTadil.data.Layer.analisisPendiente.name))
            {
                oTadil.data.Layer.analisisPendiente.deleteItems();
            }
            else
            {
                oTadil.data.Layer.analisisPendiente.createLayer();
            }
            List<Polyline> misZonas = new List<Polyline>();
            List<Polyline> misZonasAux = new List<Polyline>();
            List<Polyline> misZonasAux2 = new List<Polyline>();

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                foreach (Triangulo miTriangulo in isZonasNPPendiente)
                {
                    List<Point2d> misPuntosPolilinea = new List<Point2d>();
                    misPuntosPolilinea.Add(new Point2d(miTriangulo.getVerticeA.coordenadaX, miTriangulo.getVerticeA.coordenadaY));
                    misPuntosPolilinea.Add(new Point2d(miTriangulo.getVerticeB.coordenadaX, miTriangulo.getVerticeB.coordenadaY));
                    misPuntosPolilinea.Add(new Point2d(miTriangulo.getVerticeC.coordenadaX, miTriangulo.getVerticeC.coordenadaY));
                    misPuntosPolilinea.Add(new Point2d(miTriangulo.getVerticeA.coordenadaX, miTriangulo.getVerticeA.coordenadaY));

                    misZonas.Add(oLw.addLw2d(misPuntosPolilinea, false, "0"));
                    misZonasAux.Add(misZonas[misZonas.Count - 1]);
                }

                foreach (Polyline miPolilinea in misZonas)
                {
                    List<Point2d> misPuntosPolilinea2 = new List<Point2d>();
                    for (int i = 0; i < miPolilinea.NumberOfVertices; i++)
                    {
                        misPuntosPolilinea2.Add(miPolilinea.GetPoint2dAt(i));
                    }
                    oLw.addLw2d(misPuntosPolilinea2, false, oTadil.data.Layer.analisisPendiente.name);
                }

                oLw.eliminarPolilineas(misZonasAux);
            }
        }

        public static void selectZonasNoPasoUsuario()
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //Selecciono la Entidad Generica
                Polyline miLwUser = oSs.seleccionUsuario<Polyline>(strFrmTerreno.uiSelPolilineaNoPasoUsuario, strFrmTerreno.uiNoSelPolilinea);

                if (miLwUser != null)
                {
                    List<Point2d> misPuntos = new List<Point2d>();
                    for (int i = 0; i < miLwUser.NumberOfVertices; i++)
                    {
                        misPuntos.Add(miLwUser.GetPoint2dAt(i));
                    }
                    misPuntos.Add(miLwUser.GetPoint2dAt(0));

                    Polyline miPolCotaCero = oLw.addLw2d(misPuntos, false, oTadil.data.Layer.zonaNoPasoUsuario.name);

                    List<Polyline> miPolAEliminar = new List<Polyline>();
                    miPolAEliminar.Add(miLwUser);

                    oLw.eliminarPolilineas(miPolAEliminar);

                    oTadil.data.Layer.zonaNoPasoUsuario.Current();

                    oTadil.data.UserInfo.procesoTerminado();
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }

            }



        }

        private static List<Punto3d> getListadoPuntosFromSelectionSet(SelectionSet iSeleccionPuntos, List<Curve> curveList, double iDistCurvaNivel, double intervalo, int rangoBusqueda)
        {

            DBPoint miPunto = null;


            List<Punto3d> miLstPuntos = new List<Punto3d>();


            oCadManager.thisEditor.WriteMessage(strFrmTerreno.uiCreandoListadoPuntos);

            using (Transaction tr = oCadManager.StartTransaction())
            {

                foreach (SelectedObject item in iSeleccionPuntos)
                {
                    miPunto = (DBPoint)tr.GetObject(item.ObjectId, OpenMode.ForRead);
                    var punto3D = new Point3d(miPunto.Position.X, miPunto.Position.Y, miPunto.Position.Z);
                    miLstPuntos.Add(new Punto3d(miPunto.Position.X, miPunto.Position.Y, miPunto.Position.Z));

                    if (curveList.Count > 0)
                    {
                        var adjacentsCurvesSuperior = GetAdjacentsCurves(punto3D, curveList, iDistCurvaNivel, true, rangoBusqueda);
                        if (adjacentsCurvesSuperior.Count > 0)
                        {
                            var closestPoint3dS = GetClosestPoint3D(punto3D, adjacentsCurvesSuperior);
                            var closestPoint2dS = new Point2d(closestPoint3dS.X, closestPoint3dS.Y);
                            if (closestPoint2dS.GetDistanceTo(new Point2d(punto3D.X, punto3D.Y)) < intervalo)
                            {
                                miLstPuntos.Add(new Punto3d(closestPoint3dS.X, closestPoint3dS.Y, closestPoint3dS.Z));
                            }
                        }

                        var adjacentsCurvesInferior = GetAdjacentsCurves(punto3D, curveList, iDistCurvaNivel, false, rangoBusqueda);
                        if (adjacentsCurvesInferior.Count > 0)
                        {
                            var closestPoint3dI = GetClosestPoint3D(punto3D, adjacentsCurvesInferior);
                            var closestPoint2dI = new Point2d(closestPoint3dI.X, closestPoint3dI.Y);
                            if (closestPoint2dI.GetDistanceTo(new Point2d(punto3D.X, punto3D.Y)) < intervalo)
                            {
                                miLstPuntos.Add(new Punto3d(closestPoint3dI.X, closestPoint3dI.Y, closestPoint3dI.Z));
                            }
                        }
                    }
                }
                tr.Commit();

            }


            return miLstPuntos;

        }

        private static Point3d GetClosestPoint3D(Point3d point3D, List<Curve> curveList)
        {
            var minDist = double.MaxValue;
            Point3d closestPoint3D = new Point3d();
            var syncObject = new object();

            Parallel.ForEach(curveList, curve =>
            {
                var closestPoint = curve.GetClosestPointTo(point3D, false);
                var closestPoint2d = new Point2d(closestPoint.X, closestPoint.Y);
                var point2D = new Point2d(point3D.X, point3D.Y);
                if (closestPoint2d.GetDistanceTo(point2D) < minDist)
                {
                    lock (syncObject)
                    {
                        minDist = closestPoint2d.GetDistanceTo(point2D);
                        closestPoint3D = closestPoint;
                    }
                }
            });
            return closestPoint3D;
        }

        public static string crearMalla(List<Punto3d> iPuntos, string iNombre, double iMax, double? iPendienteMax, string filename, ref  List<Triangulo> misZonasNPPendiente, int nodosXhoja)
        {
            string handle;
                bool isEliminados = oSubDMesh.createMeshFromPoints(iPuntos, iNombre, iMax, ref misZonasNPPendiente, iPendienteMax, filename, out handle, nodosXhoja);
                //añadirZonasDeNoPasoPendienteMaxima(misZonasNPPendiente);
                if (!isEliminados)
                {
                    oUserInfo UserInfo = new oUserInfo();
                    //TODO poner una variable.. en los recursos
                    UserInfo.showInfo(strFrmTerreno.uiTriangNoEliminados);
                }
            return handle;
        }

        public static Triangulacion crearMallaNoUI(List<Punto3d> iPuntos, string iNombre, double iMax, double? iPendienteMax, string filename, ref  List<Triangulo> misZonasNPPendiente, int nodosXhoja, IWin32Window owner)
        {
            Triangulacion triangulacion;
            bool isEliminados = oSubDMesh.createTriangulation(iPuntos, iNombre, iMax, ref misZonasNPPendiente, iPendienteMax, filename, nodosXhoja, out triangulacion);
            //añadirZonasDeNoPasoPendienteMaxima(misZonasNPPendiente);
            if (!isEliminados)
            {
                oUserInfo UserInfo = new oUserInfo();
                //TODO poner una variable.. en los recursos
                UserInfo.showInfo(owner, strFrmTerreno.uiTriangNoEliminados);
            }
            return triangulacion;
        }
        public static Triangulacion crearMallaNoUI(List<Punto3d> iPuntos, string iNombre, double iMax, double? iPendienteMax, string filename, ref List<Triangulo> misZonasNPPendiente, int nodosXhoja)
        {
            Triangulacion triangulacion;
            bool isEliminados = oSubDMesh.createTriangulation(iPuntos, iNombre, iMax, ref misZonasNPPendiente, iPendienteMax, filename, nodosXhoja, out triangulacion);
            //añadirZonasDeNoPasoPendienteMaxima(misZonasNPPendiente);
            if (!isEliminados)
            {
                oUserInfo UserInfo = new oUserInfo();

            }
            return triangulacion;
        }

        public static int getOnlyPoints(string iCapaPuntos)
        {

            SelectionSet misPuntos = oSs.getSsByLayerAndEntidad(iCapaPuntos, eEntidades.POINT);

            List<Punto3d> misPuntos3d = new List<Punto3d>();
            if (misPuntos != null)
            {

                misPuntos3d = getListadoPuntosFromSelectionSet(misPuntos, new List<Curve>(), 0, 0, 1);
            }
            return misPuntos3d.Count;

        }

        public static int getOnlyCurves(string iCapaPuntos)
        {

            int miCurvas;

            int miPoli=0;
            int miLwpoli=0;
            int miLine=0;

            SelectionSet misPolilineas = oSs.getSsByLayerAndEntidad(iCapaPuntos, eEntidades.POLYLINE);
            SelectionSet misPolilineasLw = oSs.getSsByLayerAndEntidad(iCapaPuntos, eEntidades.LWPOLYLINE);
            SelectionSet misLineas = oSs.getSsByLayerAndEntidad(iCapaPuntos, eEntidades.LINE);

            if (misPolilineasLw != null)
            {
                foreach (SelectedObject item in misPolilineasLw)
                {
                    miPoli++;
                }
            }

            if (misPolilineas != null)
            {
                foreach (SelectedObject item in misPolilineas)
                {
                    miLwpoli++;
                }
            }

            if (misLineas != null)
            {
                foreach (SelectedObject item in misLineas)
                {
                    miLine++;
                }
            }
            miCurvas = miPoli + miLwpoli + miLine;
            return miCurvas;

        }

        public static List<List<Punto3d>> getLineasRotura(string iCapa)
        {

            List<List<Punto3d>> misLineasRot = new List<List<Punto3d>>();

            SelectionSet misLineas = oSs.getSsByLayerAndEntidad(iCapa, eEntidades.LINE);

            using (Transaction tr = oCadManager.StartTransaction())
            {

                if (misLineas != null)
                {
                    foreach (SelectedObject item in misLineas)
                    {
                        Line miLine = (Line)tr.GetObject(item.ObjectId, OpenMode.ForRead);
                        double miLenght = miLine.Length;
                        Point3d miNuevoPuntoEntrada = miLine.GetPointAtDist(0);
                        Point3d miNuevoPuntoSalida = miLine.GetPointAtDist(miLenght);
                        List<Punto3d> miLineaR = new List<Punto3d>();

                        miLineaR.Add(new Punto3d(miNuevoPuntoEntrada.X, miNuevoPuntoEntrada.Y, miNuevoPuntoEntrada.Z));
                        miLineaR.Add(new Punto3d(miNuevoPuntoSalida.X, miNuevoPuntoSalida.Y, miNuevoPuntoSalida.Z));

                        misLineasRot.Add(miLineaR);
                    }
                }
                return misLineasRot;
            }

        }

        public static Triangulacion getTriangulacionByName(string iName)
        {
            Triangulacion miTriangulacion = null;

            Entity miMalla = null;
            try
            {
                miMalla = oSubDMesh.getPolyFaceMeshFromName(iName, oTadil.IsDemo);
            }
            catch
            {
                miMalla = oSubDMesh.getSubDMeshFromName(iName, oTadil.IsDemo);
            }

            miTriangulacion = oSubDMesh.getTriangulacion(miMalla.ObjectId);
            return miTriangulacion;
            
        }

        public static void modificarMalla(Triangulacion triangulacion, string filename)
        {
                var puntos = new List<Point3d>();


                using (dt.Transaction tr = oCadManager.StartTransaction())
                {


                    PromptPointResult ppr = oCadManager.thisEditor.GetPoint(strUserCad.uiSelectPtoInsideTriangle);
                    
        
                    var optPoint = new PromptPointOptions(strUserCad.uiSelectPtoInsideTriangle);
                    optPoint.AllowNone = true;
                    while (ppr.Status == PromptStatus.OK)
                    {
                        puntos.Add(ppr.Value);
                        ppr = oCadManager.thisEditor.GetPoint(optPoint);
                    }
                }

                triangulacion.eliminarTriangulosLista(puntos.Select(punto => new Punto3d(punto.X, punto.Y, punto.Z)).ToList());


                oSubDMesh.createMesh(triangulacion, filename);

        }

        public static List<Punto3d> GetNubeDePuntos(string iCapaPuntos, double iIntervalo,
            List<List<Punto3d>> iLineasRotura, double intervaloLineasRotura, double distCurvaNivel, bool isCurvaAsRotura, int rangobusqueda,BackgroundWorker backgroundWorker)
        {
            var nubeDePuntos = getPoints(iCapaPuntos, iIntervalo, distCurvaNivel, isCurvaAsRotura, rangobusqueda,backgroundWorker);


            if (iLineasRotura != null)
            {
                foreach (List<Punto3d> miLinea in iLineasRotura)
                {
                    if (miLinea != null)
                    {
                        List<Punto3d> miListadePuntosLineas = new List<Punto3d>();
                        miListadePuntosLineas.AddRange(addPuntosLineaRotura(miLinea, intervaloLineasRotura));
                        foreach (Punto3d miPunto in miListadePuntosLineas)
                        {
                            Punto3d miNuevoPunto = new Punto3d(miPunto.coordenadaX, miPunto.coordenadaY, miPunto.coordenadaZ);

                            nubeDePuntos.Add(miNuevoPunto);

                        }

                    }

                }
            }
            return nubeDePuntos;
        }


        private static List<Punto3d> getPoints(string iCapaPuntos, double iIntervalo, double iDistCurvaNivel, bool isCurvaAsRotura, int rangoBusqueda, BackgroundWorker backgroundWorker)
        {

            SelectionSet misPuntos = oSs.getSsByLayerAndEntidad(iCapaPuntos, eEntidades.POINT);
            List<Punto3d> misPuntos3d = new List<Punto3d>();
           
            SelectionSet misPolilineas = oSs.getSsByLayerAndEntidad(iCapaPuntos, eEntidades.POLYLINE);
            SelectionSet misPolilineasLw = oSs.getSsByLayerAndEntidad(iCapaPuntos, eEntidades.LWPOLYLINE);
            SelectionSet misLineas = oSs.getSsByLayerAndEntidad(iCapaPuntos, eEntidades.LINE);

            //Esto lo cambio ya que hay un problema con las polilineas 3d "misPolilineas" **juanma**
            var curveList = GetCurveList(misPolilineas, misPolilineasLw, misLineas);
            //var curveList = GetCurveList( misPolilineasLw, misLineas);

            if (misPuntos != null)
            {
                misPuntos3d = getListadoPuntosFromSelectionSet(misPuntos, curveList, iDistCurvaNivel, iIntervalo, rangoBusqueda);
            }

            var syncObject = new object();

            if (isCurvaAsRotura)
            {
                //GetPointsAsRotura_Lineal(iIntervalo, iDistCurvaNivel, curveList, syncObject, misPuntos3d, rangoBusqueda);
                GetPointsAsRotura(iIntervalo, iDistCurvaNivel, curveList, syncObject, misPuntos3d, rangoBusqueda,backgroundWorker);
                //GetPointsAsRotura_Lineal(iIntervalo, iDistCurvaNivel, curveList, syncObject, misPuntos3d, rangoBusqueda);

            }
            else
            {
                GetPointsAsCurve(iIntervalo, curveList, syncObject, misPuntos3d);
            }
            return misPuntos3d;
        }

        private static void GetPointsAsRotura(double iIntervalo, double iDistCurvaNivel, List<Curve> curveList, object syncObject,
            List<Punto3d> misPuntos3d, int rangoBusqueda, BackgroundWorker backgroundWorker)
        {
            int conta_curve = 0;
            int next = 10;
            Parallel.ForEach(curveList, curve =>    
            {
                conta_curve++;
                //var backgroundWorker1 = new BackgroundWorker();
                int p = (int)(((long)(conta_curve + 1) * 100) / curveList.Count()); // evita overflow y dupes
                if (p >= next)
                {
                    next = p+10;
                    backgroundWorker.ReportProgress(p, "Calculando nube de puntos -> "+p+"%");
                }
                if (Math.Round(curve.StartPoint.Z,2) %iDistCurvaNivel == 0)
                {
                    double miLenght = 0;
                    bool poly3d = false;
                    if (curve is Line)
                        miLenght = ((Line) curve).Length;
                    else if (curve is Polyline)
                        miLenght = ((Polyline) curve).Length;
                    else if(curve is Polyline3d)
                    {
                        miLenght = ((Polyline3d)curve).Length;
                        poly3d=true;
                        //TODO: Esto sería un error que no debería darse
                    }
                    if (poly3d)
                    {
                        //var adjacentsCurves = GetAdjacentsCurves(curve.StartPoint, curveList, iDistCurvaNivel, rangoBusqueda);
                        var intervalo = iIntervalo;
                        for (double i = 0; i < miLenght; i = i + intervalo)
                        {
                            Point3d miNuevoPunto = curve.GetPointAtDist(i);
                            lock (syncObject)
                            {
                                misPuntos3d.Add(new Punto3d(miNuevoPunto.X, miNuevoPunto.Y, curve.StartPoint.Z));
                            }
                            //intervalo = UpdateInterval(curve, adjacentsCurves, miNuevoPunto, iIntervalo);
                        }
                    }
                    else
                    {
                        //var adjacentsCurves = GetAdjacentsCurves(curve.StartPoint, curveList, iDistCurvaNivel, rangoBusqueda);
                        var intervalo = iIntervalo;
                        for (double i = 0; i < miLenght; i = i + intervalo)
                        {
                            Point3d miNuevoPunto = curve.GetPointAtDist(i);
                            lock (syncObject)
                            {
                                misPuntos3d.Add(new Punto3d(miNuevoPunto.X, miNuevoPunto.Y, curve.StartPoint.Z));
                            }
                            //intervalo = UpdateInterval(curve, adjacentsCurves, miNuevoPunto, iIntervalo);
                        }
                    }
                    
                }
            });
        }
        private static void GetPointsAsRotura_Lineal(double iIntervalo, double iDistCurvaNivel, List<Curve> curveList, object syncObject,
            List<Punto3d> misPuntos3d, int rangoBusqueda)
        {
            for (int x=0;x<curveList.Count;x++)
            {
                Curve curve = curveList[x];
                if (Math.Round(curve.StartPoint.Z, 2) % iDistCurvaNivel == 0)
                {
                    double miLenght = 0;
                    bool poly3d = false;
                    if (curve is Line)
                        miLenght = ((Line)curve).Length;
                    else if (curve is Polyline)
                        miLenght = ((Polyline)curve).Length;
                    else if (curve is Polyline3d)
                    {
                        miLenght = ((Polyline3d)curve).Length;
                        poly3d = true;
                        //TODO: Esto sería un error que no debería darse
                    }
                    if (poly3d)
                    {
                        //var adjacentsCurves = GetAdjacentsCurves(curve.StartPoint, curveList, iDistCurvaNivel, rangoBusqueda);
                        var intervalo = iIntervalo;
                        for (double i = 0; i < miLenght; i = i + intervalo)
                        {
                            Point3d miNuevoPunto = curve.GetPointAtDist(i);
                            lock (syncObject)
                            {
                                misPuntos3d.Add(new Punto3d(miNuevoPunto.X, miNuevoPunto.Y, curve.StartPoint.Z));
                            }
                            //intervalo = UpdateInterval(curve, adjacentsCurves, miNuevoPunto, iIntervalo);
                        }
                    }
                    else
                    {
                        //var adjacentsCurves = GetAdjacentsCurves(curve.StartPoint, curveList, iDistCurvaNivel, rangoBusqueda);
                        var intervalo = iIntervalo;
                        for (double i = 0; i < miLenght; i = i + intervalo)
                        {
                            Point3d miNuevoPunto = curve.GetPointAtDist(i);
                            lock (syncObject)
                            {
                                misPuntos3d.Add(new Punto3d(miNuevoPunto.X, miNuevoPunto.Y, curve.StartPoint.Z));
                            }
                            //intervalo = UpdateInterval(curve, adjacentsCurves, miNuevoPunto, iIntervalo);
                        }
                    }

                }
            }
                
        }
        private static void GetPointsAsCurve(double iIntervalo, List<Curve> curveList, object syncObject,
            List<Punto3d> misPuntos3d)
        {
            Parallel.ForEach(curveList, curve =>
            {
                    double miLenght = 0;
                    bool poly3d = false;
                    if (curve is Line)
                        miLenght = ((Line)curve).Length;
                    else if (curve is Polyline)
                        miLenght = ((Polyline)curve).Length;
                    else if (curve is Polyline3d)
                    {
                        miLenght = ((Polyline3d)curve).Length;
                        poly3d = true;
                        //TODO: Esto sería un error que no debería darse
                    }
                if (poly3d)
                {
                    for (double i = 0; i < miLenght; i = i + iIntervalo)
                    {
                        Point3d miNuevoPunto = curve.GetPointAtDist(i);
                        lock (syncObject)
                        {
                            misPuntos3d.Add(new Punto3d(miNuevoPunto.X, miNuevoPunto.Y, miNuevoPunto.Z));
                        }
                    }
                }
                else
                {
                    for (double i = 0; i < miLenght; i = i + iIntervalo)
                    {
                        Point3d miNuevoPunto = curve.GetPointAtDist(i);
                        lock (syncObject)
                        {
                            misPuntos3d.Add(new Punto3d(miNuevoPunto.X, miNuevoPunto.Y, miNuevoPunto.Z));
                        }
                    }
                }
                    
            });
        }

        private static List<Curve> GetAdjacentsCurves(Point3d point, List<Curve> curves, double adjacentDistance, int rangoBusqueda)
        {
            var results = new List<Curve>();
            var zCurvaSuperior = GetZCurvaSuperior(point, adjacentDistance, rangoBusqueda);
            var zCurvaInferior = GetZCurvaInferior(point, adjacentDistance, rangoBusqueda);
            foreach (var curve in curves)
            {
                if (zCurvaSuperior.Contains(curve.StartPoint.Z) || zCurvaInferior.Contains(curve.StartPoint.Z))
                {
                    results.Add(curve);
                }
            }
            return results;
        }

        private static List<Curve> GetAdjacentsCurves(Point3d point, List<Curve> curves, double adjacentDistance, bool superior, int rangoBusqueda)
        {
            var results = new List<Curve>();
            var zCurvaSuperior = GetZCurvaSuperior(point, adjacentDistance, rangoBusqueda);
            var zCurvaInferior = GetZCurvaInferior(point, adjacentDistance, rangoBusqueda);
            var curvesZ = superior ? zCurvaSuperior : zCurvaInferior;
            foreach (var curve in curves)
            {
                if (curvesZ.Contains(curve.StartPoint.Z))
                {
                    results.Add(curve);
                }
            }
            return results;
        }

        private static List<double> GetZCurvaSuperior(Point3d point, double adjacentDistance, int rangobusqueda)
        {
            var numVeces = (int)(point.Z / adjacentDistance) + 1;
            var cotasPermitidas = new List<double>();
            for (int i = 0; i < rangobusqueda; i++)
            {
                cotasPermitidas.Add((numVeces+i) * adjacentDistance);
            }
            return cotasPermitidas;
        }

        private static List<double> GetZCurvaInferior(Point3d point, double adjacentDistance, int rangobusqueda)
        {
            var numVeces = (int)(point.Z / adjacentDistance);
            if (point.Z%adjacentDistance == 0) numVeces--;
            var cotasPermitidas = new List<double>();
            for (int i = 0; i < rangobusqueda; i++)
            {
                cotasPermitidas.Add((numVeces - i) * adjacentDistance);
            }
            return cotasPermitidas;
        }

        private static double UpdateInterval(Curve currentCurve, List<Curve> curveList, Point3d point3D, double interval)
        {
            var newInterval = interval;
            var syncObject = new object();

            Parallel.ForEach(curveList, curve =>
            {
                if (!curve.Equals(currentCurve))
                {

                    var closestPoint = curve.GetClosestPointTo(point3D, false);
                    var closestPoint2d= new Point2d(closestPoint.X, closestPoint.Y);
                    var point2D = new Point2d(point3D.X, point3D.Y);
                    if (closestPoint2d.GetDistanceTo(point2D) < newInterval)
                    {
                        lock (syncObject)
                        {
                            newInterval = closestPoint2d.GetDistanceTo(point2D);
                        }
                    }
                }
            });
            return newInterval;
        }

        private static List<Curve> GetCurveList(SelectionSet polylines, SelectionSet polylinesLw, SelectionSet lines)
        {
            try
            {
                var result = new List<Curve>();
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    //result.AddRange(ExtractPolyLine3dFromSelectionSet(polylines, tr));
                    result.AddRange(ExtractPolyLine3dFromSelectionSet_ok(polylines, tr));
                    result.AddRange(ExtractPolyLineFromSelectionSet(polylinesLw,tr));
                    result.AddRange(ExtractLinesFromSelectionSet(lines,tr));
                    tr.Commit();
                }
                return result;
            }
            catch (Exception e)
            {
                //TODO: Log error?
                return new List<Curve>();
            }
        }
        private static List<Curve> GetCurveList(SelectionSet polylinesLw, SelectionSet lines)
        {
            try
            {
                var result = new List<Curve>();
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    result.AddRange(ExtractPolyLineFromSelectionSet(polylinesLw, tr));
                    //result.AddRange(ExtractLinesFromSelectionSet(lines, tr));
                    tr.Commit();
                }
                return result;
            }
            catch (Exception e)
            {
                //TODO: Log error?
                return new List<Curve>();
            }
        }

        private static List<Curve> ExtractLinesFromSelectionSet(SelectionSet lines, Transaction tr)
        {
            var result = new List<Curve>();
            if (lines != null)
            {
                foreach (SelectedObject item in lines)
                {
                    result.Add((Line)tr.GetObject(item.ObjectId, OpenMode.ForRead));
                }
            }
            return result;
        }

        private static List<Curve> ExtractPolyLineFromSelectionSet(SelectionSet polylines, Transaction tr)
        {
            var result = new List<Curve>();
            if (polylines != null)
            {
                foreach (SelectedObject item in polylines)
                {
                    result.Add((Polyline) tr.GetObject(item.ObjectId, OpenMode.ForRead));
                }
            }
            return result;
        }
        private static List<Curve> ExtractPolyLine3dFromSelectionSet(SelectionSet polylines, Transaction tr)
        {
            var result = new List<Curve>();
            if (polylines != null)
            {
                foreach (SelectedObject item in polylines)
                {
                    result.Add((Polyline3d)tr.GetObject(item.ObjectId, OpenMode.ForRead));
                }
            }
            return result;
        }
        private static List<Curve> ExtractPolyLine3dFromSelectionSet_ok(SelectionSet polylines, Transaction tr)
        {
            var result = new List<Curve>();
            if (polylines != null)
            {
                foreach (SelectedObject item in polylines)
                {
                    Polyline3d pol3d = (Polyline3d)tr.GetObject(item.ObjectId, OpenMode.ForRead);
                    Polyline pol = new Polyline((int)pol3d.EndParam);
                    for (int i = 0; i < pol3d.EndParam; i++)
                    {
                        Point2d p2d = new Point2d(pol3d.GetPointAtParameter(i).X, pol3d.GetPointAtParameter(i).Y);
                        pol.AddVertexAt(i,p2d,0,0,0);
                    }
                    pol.Elevation = pol3d.GetPointAtParameter(0).Z;

                    result.Add(pol);
                }
            }
            return result;
        }

        private static List<Punto3d> addPuntosLineaRotura(List<Punto3d> iLineaRotura, double intervaloLineasRotura)
        {
            List<Punto3d> miListaPuntosAdd = new List<Punto3d>();
            List<Punto3d> miListaPuntosAddAux = new List<Punto3d>();
            Punto3d miPuntoM = null;

            foreach (Punto3d miPunto in iLineaRotura)
            {
                miListaPuntosAdd.Add(miPunto);
            }
            if (iLineaRotura.Count > 1)
            {
                double distancia = iLineaRotura.ElementAt(0).distancia2d(iLineaRotura.ElementAt(1));
                for (int i = 0; i < miListaPuntosAdd.Count - 1; i++)
                {
                    if (iLineaRotura.ElementAt(i).distancia2d(iLineaRotura.ElementAt(i + 1)) > distancia) distancia = iLineaRotura.ElementAt(i).distancia2d(iLineaRotura.ElementAt(i + 1));
                }

                if (distancia > intervaloLineasRotura)
                {
                    while (distancia > intervaloLineasRotura)
                    {
                        for (int i = 0; i < miListaPuntosAdd.Count - 1; i++)
                        {
                            miListaPuntosAddAux.Add(miListaPuntosAdd.ElementAt(i));
                            miListaPuntosAddAux.Add(puntoMedio(miListaPuntosAdd.ElementAt(i), miListaPuntosAdd.ElementAt(i + 1)));
                            miPuntoM = miListaPuntosAdd.ElementAt(i + 1);
                        }
                        if (miPuntoM != null) miListaPuntosAddAux.Add(miPuntoM);
                        miListaPuntosAdd.Clear();
                        foreach (Punto3d miPunto in miListaPuntosAddAux)
                        {
                            miListaPuntosAdd.Add(miPunto);
                        }
                        miListaPuntosAddAux.Clear();
                        distancia = distancia / 2;
                    }
                }
                else
                {
                    return iLineaRotura;
                }
            }
            else
            {
                return iLineaRotura;
            }
            return miListaPuntosAdd;


        }

        private static Punto3d puntoMedio(Punto3d iPunto1, Punto3d iPunto2)
        {
            Punto3d miPuntoMedio = new Punto3d((iPunto1.coordenadaX + iPunto2.coordenadaX) / 2, (iPunto1.coordenadaY + iPunto2.coordenadaY) / 2, (iPunto1.coordenadaZ + iPunto2.coordenadaZ) / 2);
            return miPuntoMedio;
        }
        private Polyline Poly3d_Poly(Polyline3d poly)
        {
            Polyline pol = new Polyline();
            Point3dCollection myPointCollection2 = new Point3dCollection();
            for (int i=0;i<poly.EndParam;i++)
            {
                Point2d p2d = new Point2d(poly.GetPointAtParameter(i).X, poly.GetPointAtParameter(i).Y);
                pol.SetPointAt(i,p2d);
            }
            pol.Elevation = poly.GetPointAtParameter(0).Z;
            return pol;

        }

    }
}
