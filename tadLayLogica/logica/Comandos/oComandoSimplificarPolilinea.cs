using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using engCadNet;

using dt = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using System.Diagnostics;
using Terrenos;
using tadLayShare.puntos;
using System.Collections;
using tadLayShare;

namespace tadLayLogica.Comandos
{
    public class oComandoSimplificarPolilinea
    {


        public static List<Point3dCollection> simplificarPolilineaEnvolvente(List<Polyline> iPolilineas)
        {
            List<Point3dCollection> miPolylineas = new List<Point3dCollection>();
            

            foreach (Polyline miPol in iPolilineas)
            {
                if (miPol.NumberOfVertices > 4)
                {
                    Point3dCollection miPoly = new Point3dCollection();

                    List<Punto3d> misPuntos = new List<Punto3d>();
                    List<Point3d> misPoints3d = new List<Point3d>();
                    for (int i = 0; i < miPol.NumberOfVertices; i++)
                    {
                        misPuntos.Add(new Punto3d(miPol.GetPoint2dAt(i).X, miPol.GetPoint2dAt(i).Y, 1));
                        misPoints3d.Add(new Point3d(miPol.GetPoint2dAt(i).X, miPol.GetPoint2dAt(i).Y, 0));
                    }

                    //1. triangular los puntos que componen la polilinea
                    Terrenos.Terreno miTerreno = new Terrenos.Terreno(misPuntos);
                    bool isEliminados = true;
                    Triangulacion miTriangulacion = new Terrenos.Triangulacion(miTerreno, double.MaxValue, "simpliPoli", ref isEliminados);

                    //2. escoger aquellos triangulos que tengan menos de tres adyacentes (los que pertenecen a la envolvente)
                    var misTriangulosEnvolvente = new List<Triangulo>();

                    var misAdyacentes = miTriangulacion.getAdyacentes;
                    var misTriangulos = miTriangulacion.getTriangulos;

                    foreach (var miTriangulo in misTriangulos)
                    {
                        var misTriangAdyacentes = misAdyacentes[miTriangulo.getIndex];
                        if (misTriangAdyacentes.Count < 3)
                        {
                            misTriangulosEnvolvente.Add(miTriangulo);
                        }
                    }

                    //3. calcular el punto medio de la zona triangulada
                    Punto3d miCentroTerreno = miTerreno.centroTerreno;

                    //4. para cada triangulo elegir los dos puntos que esten más lejanos al centro
                    List<Point3d> misPuntosEnvolvente = new List<Point3d>();

                    foreach (Triangulo miTriangulo in misTriangulosEnvolvente)
                    {
                        Punto3d miPuntoA = miTriangulo.getVerticeA;
                        Punto3d miPuntoB = miTriangulo.getVerticeB;
                        Punto3d miPuntoC = miTriangulo.getVerticeC;


                        Point3d miPuntoA3d = new Point3d(miPuntoA.coordenadaX, miPuntoA.coordenadaY, 0);
                        Point3d miPuntoB3d = new Point3d(miPuntoB.coordenadaX, miPuntoB.coordenadaY, 0);
                        Point3d miPuntoC3d = new Point3d(miPuntoC.coordenadaX, miPuntoC.coordenadaY, 0);

                        var miTriangAd = miTriangulacion.getAdyacente(miTriangulo, miPuntoA, miPuntoB);
                        if (miTriangAd == -1)
                        {
                            misPuntosEnvolvente.Add(miPuntoA3d);
                            misPuntosEnvolvente.Add(miPuntoB3d);
                        }
                        else
                        {
                            miTriangAd = miTriangulacion.getAdyacente(miTriangulo, miPuntoA, miPuntoC);
                            if (miTriangAd == -1)
                            {
                                misPuntosEnvolvente.Add(miPuntoA3d);
                                misPuntosEnvolvente.Add(miPuntoC3d);
                            }
                            else
                            {
                                misPuntosEnvolvente.Add(miPuntoB3d);
                                misPuntosEnvolvente.Add(miPuntoC3d);
                            }
                        }

                    }

                    //5. Ordenar los puntos que han quedado en la envolvente en el mismo orden que la polilinea original
                    Polyline miPoligono = new Polyline();
                    int vertice=0;
                    if (miTriangulacion.getTriangulos.Count > 1)
                    {
                        foreach (Point3d miPunto in misPoints3d)
                        {
                            if (misPuntosEnvolvente.Contains(miPunto))
                            {
                                miPoly.Add(miPunto);
                                miPoligono.AddVertexAt(vertice, new Point2d(miPunto.X, miPunto.Y), 0, 0, 0);
                            }
                        }
                        miPoly.Add(miPoly[0]);
                        miPoligono.AddVertexAt(vertice, new Point2d(miPoly[0].X, miPoly[0].Y), 0, 0, 0);
                    }
                    else if (miTriangulacion.getTriangulos.Count != 0)
                    {
                        Triangulo miUnicoTriangulo = miTriangulacion.getTriangulos[0];
                        miPoly.Add(new Point3d(miUnicoTriangulo.getVerticeA.coordenadaX, miUnicoTriangulo.getVerticeA.coordenadaY, 0));
                        miPoly.Add(new Point3d(miUnicoTriangulo.getVerticeB.coordenadaX, miUnicoTriangulo.getVerticeB.coordenadaY, 0));
                        miPoly.Add(new Point3d(miUnicoTriangulo.getVerticeC.coordenadaX, miUnicoTriangulo.getVerticeC.coordenadaY, 0));
                        miPoly.Add(new Point3d(miUnicoTriangulo.getVerticeA.coordenadaX, miUnicoTriangulo.getVerticeA.coordenadaY, 0));


                        miPoligono.AddVertexAt(0, new Point2d(miPoly[0].X, miPoly[0].Y), 0, 0, 0);
                        miPoligono.AddVertexAt(1, new Point2d(miPoly[1].X, miPoly[1].Y), 0, 0, 0);
                        miPoligono.AddVertexAt(2, new Point2d(miPoly[2].X, miPoly[2].Y), 0, 0, 0);
                        miPoligono.AddVertexAt(3, new Point2d(miPoly[3].X, miPoly[3].Y), 0, 0, 0);
                    }

                    bool isPuntosDentro = true;
                    foreach (Point3d miPunto in miPoly)
                    {
                        if (!oPolygon.isPointInPolygon(miPoligono, miPunto.X, miPunto.Y)) isPuntosDentro = false;

                    }
                    if (isPuntosDentro)
                    {
                        miPolylineas.Add(miPoly);
                    }
                    else
                    {
                        Point3dCollection miPolyOriginal = new Point3dCollection();
                        for (int i = 0; i < miPol.NumberOfVertices; i++)
                        {
                            miPoly.Add(miPol.GetPoint3dAt(i));

                        }
                        miPolylineas.Add(miPolyOriginal);
                    }

                }
                else
                {
                    Point3dCollection miPoly = new Point3dCollection();
                    for(int i =0;i<miPol.NumberOfVertices;i++)
                    {
                        miPoly.Add(miPol.GetPoint3dAt(i));

                    }
                    miPolylineas.Add(miPoly);
                }

            }


            return miPolylineas;
        }


        public static List<Polyline> simplificarPolilineaEnvolventeToPols(List<Polyline> iPolilineas)
        {
            List<Point3dCollection> miPols = simplificarPolilineaEnvolvente(iPolilineas);
            List<Polyline> misPolsSimplificadas = new List<Polyline>();

            foreach (Point3dCollection miPointCol in miPols)
            {
                Polyline miNuevaPol = new Polyline();
                for (int i = 0; i < miPointCol.Count; i++)
                {
                    miNuevaPol.AddVertexAt(i, new Point2d(miPointCol[i].X, miPointCol[i].Y), 0, 0, 0);
                }
                misPolsSimplificadas.Add(miNuevaPol);
            }
            return misPolsSimplificadas;
        }


        public static void simplificarPolilineaEnvAndDraw(SelectionSet iSsPolilineas, string iLayer)
        {



            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {


                List<Polyline> miPolylines = new List<Polyline>();
                List<Point3dCollection> miPolylinesSimp = new List<Point3dCollection>();
                short color;

                Stopwatch miMedicion = new Stopwatch();

                miMedicion.Start();

                oLayer.vLayerOffOn(iLayer, false, false);
                oLayer.current(iLayer);

                using (dt.Transaction tr = oCadManager.StartTransaction())
                {
                    if (iSsPolilineas != null)
                    {

                        foreach (SelectedObject item in iSsPolilineas)
                        {
                            Polyline miPol = (Polyline)tr.GetObject(item.ObjectId, OpenMode.ForRead);
                            miPolylines.Add(miPol);

                        }

                    }



                    LayerTableRecord acLyrTableRecord = tr.GetObject(oLayer.getLayerObjId(iLayer), OpenMode.ForRead) as LayerTableRecord;
                    color = acLyrTableRecord.Color.ColorIndex;



                    if (miPolylines.Count != 0)
                    {
                        miPolylinesSimp = simplificarPolilineaEnvolvente(miPolylines);

                        foreach (Point3dCollection miPol in miPolylinesSimp)
                        {

                            Polyline miPolSim = engCadNet.oLw.addLw2d(miPol, false, iLayer, color);
                            //Debo de Abrir el Objeto para poder cargarlo en la Capa
                            using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miPolSim))
                            {
                                miLw.open();

                                miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                                miLw.entidad.Layer = oTadil.data.Layer.visibilidadEje.name;

                                miLw.save();
                            }
                        }

                    }
                    tr.Commit();
                }
                //Desactivo las Capas GIS
                oTadil.data.Layer.zonasGisOff();

                oLayer.vLayerOffOn(iLayer, false, false);

                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);

            }
        }


        public static List<Point2d> ordena4Puntos(List<Point2d> iPuntos)
        {
            List<Point2d> misPuntos = new List<Point2d>();
            List<Point2d> misPuntosAux = new List<Point2d>();
            foreach (Point2d mipunto in iPuntos)
            {
                misPuntosAux.Add(mipunto);
            }

            misPuntos.Add(iPuntos[0]);
            iPuntos.RemoveAt(0);
            int i = 0;
            int numIteraciones = 0;

            while ((iPuntos.Count > 1)&&(numIteraciones<10))
            {
                Polyline miPol1 = new Polyline();
                miPol1.AddVertexAt(0, misPuntos[i], 0, 0, 0);
                miPol1.AddVertexAt(0, iPuntos[0], 0, 0, 0);


                Polyline miPol2 = new Polyline();
                miPol2.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
                miPol2.AddVertexAt(0, iPuntos[1], 0, 0, 0);

                Polyline miPol3 = new Polyline();
                if (i == 0)
                {
                    miPol3.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
                    miPol3.AddVertexAt(0, iPuntos[2], 0, 0, 0);
                }
                else
                {
                    miPol3.AddVertexAt(0, new Point2d(0, 0), 0, 0, 0);
                    miPol3.AddVertexAt(0, misPuntos[0], 0, 0, 0);
                }


                //Creo la Intersección
                Point3dCollection miColInter12 = new Point3dCollection();
                miPol1.IntersectWith(miPol2, Intersect.ExtendThis, miColInter12, IntPtr.Zero, IntPtr.Zero);


                //Creo la Intersección
                Point3dCollection miColInter13 = new Point3dCollection();
                miPol1.IntersectWith(miPol3, Intersect.ExtendThis, miColInter13, IntPtr.Zero, IntPtr.Zero);

                if ((miColInter12.Count > 0 && miColInter13.Count > 0) || (miColInter12.Count == 0 && miColInter13.Count == 0))
                {
                    misPuntos.Add(iPuntos[0]);
                    iPuntos.RemoveAt(0);
                    i++;
                }
                else
                {
                    if (i == 0)
                    {
                        Point2d miPuntoIncorrecto = new Point2d(iPuntos[0].X, iPuntos[0].Y);
                        iPuntos.RemoveAt(0);
                        iPuntos.Add(miPuntoIncorrecto);
                    }
                    else
                    {
                        misPuntos.Add(iPuntos[1]);
                        iPuntos.RemoveAt(1);
                    }

                }
                numIteraciones++;
            }
            misPuntos.Add(iPuntos[0]);

            if (numIteraciones >= 10)
            {
                return misPuntosAux;
            }
            return misPuntos;
        }



    }

     
}
