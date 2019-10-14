using System;
using System.Collections.Generic;
using System.Linq;

namespace engCadNet
{

    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;
    using tadLayLan;
    using tadLayShare.puntos;

    public static class oLw
    {
        public static void createLw()
        {
            using (Transaction tr = oCadManager.StartTransaction())
            {

                // Get the current color, for our temp graphics
                Color col = oCadManager.thisBase.Cecolor;

                // Create a point collection to store our vertices
                Point3dCollection pts = new Point3dCollection();

                PromptPointOptions opt = new PromptPointOptions(strGeneralUser.uiSeleccionaPuntosDePolilinea);

                opt.AllowNone = true;

                // Get the start point for the polyline
                PromptPointResult res = oCadManager.thisEditor.GetPoint(opt);


                while (res.Status == PromptStatus.OK)
                {
                    // Add the selected point to the list
                    pts.Add(res.Value);

                    // Drag a temp line during selection
                    // of subsequent points
                    opt.UseBasePoint = true;
                    opt.BasePoint = res.Value;
                    res = oCadManager.thisEditor.GetPoint(opt);
                    if (res.Status == PromptStatus.OK)
                    {
                        // For each point selected,
                        // draw a temporary segment
                        oCadManager.thisEditor.DrawVector(
                           pts[pts.Count - 1], // start point
                           res.Value,          // end point
                           col.ColorIndex,     // current color
                           false);             // highlighted?
                    }
                }
                if (res.Status == PromptStatus.None)
                {
                    // Get the current UCS
                    Matrix3d ucs = oCadManager.thisEditor.CurrentUserCoordinateSystem;

                    Point3d origin = new Point3d(0, 0, 0);
                    Vector3d normal = new Vector3d(0, 0, 1);
                    normal = normal.TransformBy(ucs);

                    // Create a temporary plane, to help with calcs
                    Plane plane = new Plane(origin, normal);

                    // Create the polyline, specifying
                    // the number of vertices up front
                    Polyline pline = new Polyline(pts.Count);
                    pline.Normal = normal;
                    foreach (Point3d pt in pts)
                    {
                        Point3d transformedPt =
                          pt.TransformBy(ucs);
                        pline.AddVertexAt(
                          pline.NumberOfVertices,
                          plane.ParameterOf(transformedPt),
                          0, 0, 0
                        );
                    }


                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead);




                    BlockTableRecord btr =
                      (BlockTableRecord)tr.GetObject(
                        bt[BlockTableRecord.ModelSpace],
                        OpenMode.ForWrite
                      );
                    ObjectId plineId = btr.AppendEntity(pline);
                    tr.AddNewlyCreatedDBObject(pline, true);
                    tr.Commit();
                    oCadManager.thisEditor.WriteMessage(string.Format(strGeneralUser.uiEntidadPolilinea, plineId.ToString()));


                }
            }

            oCadManager.thisEditor.Regen();
        }


        public static Polyline addLw2d_Temporal(List<Point2d> iLstPoint, bool isClosed)
        {

                Polyline myPol = new Polyline(iLstPoint.Count);

                myPol.SetDatabaseDefaults();


                int myVertexNum = 0;


                foreach (Point2d myPto in iLstPoint)
                {
                    myPol.AddVertexAt(myVertexNum, myPto, 0, 0, 0);

                    myVertexNum++;
                }

              

                myPol.Closed = isClosed;

                return myPol;

            
        }


        public static Polyline addLw2d(List<Point2d> iLstPoint, bool isClosed, string iLayer)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {

                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                //Necesito Crear un Nuevo Registro para Añadir la Linea
                BlockTableRecord acBlockTableRec =
                    tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Polyline myPol = new Polyline(iLstPoint.Count);

                myPol.SetDatabaseDefaults();


                int myVertexNum = 0;


                foreach (Point2d myPto in iLstPoint)
                {
                    myPol.AddVertexAt(myVertexNum, myPto, 0, 0, 0);

                    myVertexNum++;
                }

                myPol.Layer = iLayer;

                myPol.Closed = isClosed;

                acBlockTableRec.AppendEntity(myPol);

                tr.AddNewlyCreatedDBObject(myPol, true);

                tr.Commit();

                return myPol;

            }
        }

        public static Polyline addLw2d(Point3dCollection iLstPoint, bool isClosed, string iLayer, short iColorIndex)
        {
            using (Transaction tr = oCadManager.StartTransaction())
            {

                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                //Necesito Crear un Nuevo Registro para Añadir la Linea
                BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Polyline myPol = new Polyline(iLstPoint.Count);

                myPol.SetDatabaseDefaults();


                int myVertexNum = 0;

                Plane myPlane = new Plane();




                foreach (Point3d myPto in iLstPoint)
                {
                    myPol.AddVertexAt(myVertexNum, myPto.Convert2d(myPlane), 0, 0, 0);

                    myVertexNum++;
                }

                myPol.Layer = iLayer;

                myPol.Color = Color.FromColorIndex(ColorMethod.ByLayer, iColorIndex);

                myPol.Closed = isClosed;

                acBlockTableRec.AppendEntity(myPol);

                tr.AddNewlyCreatedDBObject(myPol, true);

                tr.Commit();

                return myPol;

            }
        }

        public static Polyline addLw2d(Point3dCollection iLstPoint, bool isClosed, string iLayer)
        {
            using (Transaction tr = oCadManager.StartTransaction())
            {

                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                //Necesito Crear un Nuevo Registro para Añadir la Linea
                BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Polyline myPol = new Polyline(iLstPoint.Count);

                myPol.SetDatabaseDefaults();


                int myVertexNum = 0;

                Plane myPlane = new Plane();




                foreach (Point3d myPto in iLstPoint)
                {
                    myPol.AddVertexAt(myVertexNum, myPto.Convert2d(myPlane), 0, 0, 0);

                    myVertexNum++;
                }

                myPol.Layer = iLayer;

                myPol.Closed = isClosed;

                acBlockTableRec.AppendEntity(myPol);

                tr.AddNewlyCreatedDBObject(myPol, true);

                tr.Commit();

                return myPol;

            }
        }

        public static Polyline addLw2d(Point3d iP1, Point3d iP2, string iLayer, Matrix3d? iMatrix, Color iColor)
        {

            Point3dCollection miCol = new Point3dCollection();

            miCol.Add(iP1);
            miCol.Add(iP2);

            return addLw2d(miCol, false, iLayer, iMatrix, iColor);


        }


       
        public static Polyline addLw2d(Point3dCollection iLstPoint, bool isClosed, string iLayer, Matrix3d? iMatrix, Color iColor)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {
                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                //Necesito Crear un Nuevo Registro para Añadir la Linea
                BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                Polyline myPol = new Polyline(iLstPoint.Count);

                myPol.SetDatabaseDefaults();


                int myVertexNum = 0;

                Plane myPlane = new Plane();




                foreach (Point3d myPto in iLstPoint)
                {
                    myPol.AddVertexAt(myVertexNum, myPto.Convert2d(myPlane), 0, 0, 0);

                    myVertexNum++;
                }

                myPol.Layer = iLayer;
                myPol.Closed = isClosed;
                myPol.Color = iColor;

                if (iMatrix != null)
                {
                    myPol.TransformBy(iMatrix.Value);
                }


                acBlockTableRec.AppendEntity(myPol);

                tr.AddNewlyCreatedDBObject(myPol, true);

                tr.Commit();

                return myPol;
            }



        }
        
        public static Polyline3d addLw3d(Point3dCollection iLstPto3d, bool isClosed, string iLayer)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {
                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                //Necesito Crear un Nuevo Registro para Añadir la Linea
                BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Polyline3d myLw3d = new Polyline3d(Poly3dType.SimplePoly, iLstPto3d, isClosed);

                //configuro los Datos por Defecto

                myLw3d.SetDatabaseDefaults();

                myLw3d.Layer = iLayer;

                acBlockTableRec.AppendEntity(myLw3d);

                tr.AddNewlyCreatedDBObject(myLw3d, true);

                tr.Commit();

                return myLw3d;



            }

        }
        
        public static Point3d getCDG(Polyline iLw)
        {
            using (Transaction tr = oCadManager.StartTransaction())
            {

                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                //Necesito Crear un Nuevo Registro para Añadir la Linea
                BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                DBObjectCollection myObjCol = new DBObjectCollection();

                myObjCol.Add(iLw);

                DBObjectCollection myObjColRegion = new DBObjectCollection();

                myObjColRegion = Region.CreateFromCurves(myObjCol);

                Region myRegion = myObjColRegion[0] as Region;

                Solid3d mySolid = new Solid3d();

                mySolid.Extrude(myRegion, 1, 0);

                Point3d myCdg = new Point3d(mySolid.MassProperties.Centroid.X, mySolid.MassProperties.Centroid.Y, 0);

                myRegion.Dispose();
                mySolid.Dispose();

                tr.Abort();

                return myCdg;

            }
        }
        
        public static List<Point3d> getLstPto(Polyline iLw)
        {

            List<Point3d> miLst = new List<Point3d>();

            for (int i = 0; i < iLw.NumberOfVertices; i++)
            {
                Point3d miPto = iLw.GetPoint3dAt(i);
                miLst.Add(miPto);
            }


            return miLst;

        }

        public static Point3dCollection getLstPto(Polyline3d iLw3d)
        {

            Point3dCollection miLstPto = new Point3dCollection();

            using (Transaction tr = oCadManager.StartTransaction())
            {
                foreach (ObjectId myObjId in iLw3d)
                {
                    PolylineVertex3d miVertice = (PolylineVertex3d)tr.GetObject(myObjId, OpenMode.ForRead);

                    miLstPto.Add(miVertice.Position);
                }
            }

            return miLstPto;

        }

        public static List<Point3d> getLstPtoFromLw3d(Polyline3d iLw3d)
        {

            List<Point3d> miLstPto = new List<Point3d>();

            using (Transaction tr = oCadManager.StartTransaction())
            {
                foreach (ObjectId myObjId in iLw3d)
                {
                    PolylineVertex3d miVertice = (PolylineVertex3d)tr.GetObject(myObjId, OpenMode.ForRead);

                    miLstPto.Add(miVertice.Position);
                }
            }

            return miLstPto;

        }

        public static Point3dCollection getColPto(Polyline iLw)
        {

            Point3dCollection miCol = new Point3dCollection();

            for (int i = 0; i < iLw.NumberOfVertices; i++)
            {
                Point3d miPto = iLw.GetPoint3dAt(i);
                miCol.Add(miPto);
            }

            return miCol;

        }

        public static int getTramoByPtoBaseCero(Polyline iLw, Point3d iPto)
        {

            if (!isPtoOnLw(iLw, iPto))
            {
                iPto = getPointMasCercano(iLw, iPto);
            }


            bool miIsOn;

            int miNumTramos = iLw.NumberOfVertices - 1;

            for (int i = 0; i < miNumTramos; i++)
            {
                miIsOn = iLw.GetLineSegmentAt(i).IsOn(iPto);

                if (miIsOn)
                {
                    return i;
                }


            }

            throw new Exception(strError.ePuntoNoPolilinea);

        }

        /// <summary>
        /// Dado un Punto, obtenemos el mas cercano a la polilinea
        /// </summary>
        public static Point3d getPointMasCercano(this Polyline iLw, Point3d iPto)
        {
            return iLw.GetClosestPointTo(iPto, false);

        }

        /// <summary>
        /// ¿PUNTO ESTA EN EL PERIMETRO DE LA POLILINEA?
        /// </summary>
        public static bool isPtoOnLw(this Polyline iLw, Point3d iPto)
        {
            bool miIsOn = false;


            if (iLw.NumberOfVertices > 2)
            {
                for (int i = 0; i < iLw.NumberOfVertices - 1; i++)
                {

                    miIsOn = iLw.GetLineSegmentAt(i).IsOn(iPto);

                    if (miIsOn) { return true; }

                }

                return false;
            }
            else
            {
                return iLw.GetLineSegmentAt(0).IsOn(iPto);
            }

        }

        public static Polyline splitLwTwoPoints(Polyline iLw, Point3d iPtoIni, Point3d iPtoFin, string iLayer)
        {

            Point3dCollection miColPtoInt = new Point3dCollection();


            if (!isPtoOnLw(iLw, iPtoIni))
            {
                iPtoIni = getPointMasCercano(iLw, iPtoIni);
            }

            if (!isPtoOnLw(iLw, iPtoFin))
            {
                iPtoFin = getPointMasCercano(iLw, iPtoFin);
            }


            miColPtoInt.Add(iPtoIni);
            miColPtoInt.Add(iPtoFin);



            using (Transaction tr = oCadManager.StartTransaction())
            {

                DBObjectCollection acDbObjColl = iLw.GetSplitCurves(miColPtoInt);

                ObjectId miObjId = ObjectId.Null;

                List<Polyline> miLstLwBreak = new List<Polyline>();

                Polyline miLw;

                Polyline miLwOk = null;

                foreach (Entity acEnt in acDbObjColl)
                {
                    // Add each offset object
                    miObjId = oTools.entidadAdd(acEnt, iLayer);

                    miLw = (Polyline)tr.GetObject(miObjId, OpenMode.ForWrite);

                    if (miLw.isPtoOnLw(iPtoIni) && miLw.isPtoOnLw(iPtoFin))
                    {
                        miLwOk = miLw;
                    }
                    else
                    {
                        oTools.entidadDelete(miLw);
                    }

                }

                tr.Commit();


                if (miLwOk != null)
                {
                    return miLwOk;
                }
                else
                {
                    throw new Exception(strError.ePolilineaEntre2Puntos);
                }

            }
        }




        public static  void createRevestimiento(ref Polyline iLwExterior, Polyline iLwInterior, string iLayer)
        {

            //Tramo Superior
            Point3dCollection miLstSup = new Point3dCollection();
            miLstSup.Add(iLwExterior.StartPoint);
            miLstSup.Add(iLwInterior.StartPoint);

            Polyline miLwTramoHorizontalSuperior = addLw2d(miLstSup, false, iLayer);


            //Tramo Inferior
            Point3dCollection miLstINF = new Point3dCollection();
            miLstINF.Add(iLwExterior.EndPoint);
            miLstINF.Add(iLwInterior.EndPoint);

            Polyline miLwTramoHorizontalInferior = addLw2d(miLstINF, false, iLayer);


            List<Polyline> miLstLw = new List<Polyline>() {iLwInterior, miLwTramoHorizontalInferior, miLwTramoHorizontalSuperior };

            Entity[] miColLwToJoin = miLstLw.ToArray();
                    
           iLwExterior.JoinEntities(miColLwToJoin);
          
       }



        public static Polyline joinTemporal_Lw(List<Polyline> iLstLw, bool iErasedOriginal)
        {
            using (Transaction tr = oCadManager.StartTransaction())
            {

                Polyline miLw = (Polyline)tr.GetObject(iLstLw[0].ObjectId, OpenMode.ForWrite);

                iLstLw.RemoveAt(0);

                Entity[] miColLwToJoin = iLstLw.ToArray();


                miLw.JoinEntities(miColLwToJoin);


                if (iErasedOriginal)
                {
                    foreach (Polyline cLw in iLstLw)
                    {

                        oTools.entidadDelete(cLw.ObjectId);
                    }

                }

                tr.Commit();

                return miLw;

            }
        }



        public static Polyline joinLw(List<Polyline> iLstLw, bool iErasedOriginal)
        {
            using (Transaction tr = oCadManager.StartTransaction())
            {

                Polyline miLw = (Polyline)tr.GetObject(iLstLw[0].ObjectId, OpenMode.ForWrite);

                iLstLw.RemoveAt(0);

                Entity[] miColLwToJoin = iLstLw.ToArray();


                miLw.JoinEntities(miColLwToJoin);


                if (iErasedOriginal)
                {
                    foreach (Polyline cLw in iLstLw)
                    {

                        oTools.entidadDelete(cLw.ObjectId);
                    }

                }

                tr.Commit();

                return miLw;

            }
        }

        /// <summary>
        /// Polilinea Creada de Izquierda a Derecha
        /// </summary>
        public static bool isLeftToRight(this Polyline iLw)
        {

            Point2d p0 = iLw.GetPoint2dAt(0);
            Point2d p1 = iLw.GetPoint2dAt(iLw.NumberOfVertices - 1);

            if (p1.X - p0.X > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static Point3dCollection discretizarPolilinea(Polyline iLw, double iLonDiscre)
        {
            Point3dCollection miLstPtoDiscreOnLw = new Point3dCollection();

            //Obtengo la Longitud en el Eje X a Discretizar
            double miLwLonX = (iLw.EndPoint.X - iLw.StartPoint.X);

            //Numero de intervalos
            double miIntervalo = miLwLonX / iLonDiscre;

            int miPasos = 1 + ((int)System.Math.Truncate(miIntervalo));

            double miLonDiscreReal = miLwLonX / miPasos;


            Point3d miPtoOrigen = iLw.StartPoint;
            Point3d miPtoH1;
            Point3d miPtoH2;

            Point3dCollection miColInter;

            Polyline miLwHor;

            for (int i = 1; i < miPasos; i++)
            {

                //Creo el Punto Paralelo
                miPtoH1 = miPtoOrigen.getFromIncXIncY(i * miLonDiscreReal, 0, 0);
                miPtoH2 = miPtoH1.getFromIncXIncY(0, 1, 0);

                //Creo una Linea Horizontal
                miLwHor = new Polyline();
                miLwHor.AddVertexAt(0, miPtoH1.to2d(), 0, 0, 0);
                miLwHor.AddVertexAt(1, miPtoH2.to2d(), 0, 0, 0);

                //Creo la Intersección
                miColInter = new Point3dCollection();
                miLwHor.IntersectWith(iLw, Intersect.ExtendThis, miColInter, IntPtr.Zero, IntPtr.Zero);


                if (miColInter == null | miColInter.Count == 0)
                {
                    // oTools.entidadAdd(miLwHor, "0");

                    throw new Exception(strError.eDiscretizarPol);
                }
                else if (miColInter.Count == 1)
                {
                    miLstPtoDiscreOnLw.Add(miColInter[0]);
                }
                else
                {
                    miLstPtoDiscreOnLw.Add(miPtoH1.getPtoMasCercano(miColInter));

                }

            }


            ////Represento los Puntos
            //foreach (Point3d item in miLstPtoDiscreOnLw)
            //{
            // oCircle.addCircle2D(item, 0.02, "0");
            //}


            return miLstPtoDiscreOnLw;

        }

        /// <summary>
        /// Limpiar Polilinea de Vertices
        /// </summary>
        public static Polyline clearPolilinea(Polyline3d iLw, string iLayer)
        {
            List<Point3d> miLstPuntosDirty = getLstPtoFromLw3d(iLw);

            Point3dCollection miLstPuntosClear = new Point3dCollection();


            Point3d miPtoPrevio;

            Point3d miPtoOrigen;

            Point3d miPtoNext;

            double? miAnguloGrados;


            //Añado el Vertice 0
            Point3d miPtoInicial = miLstPuntosDirty.First();
            Point3d miPtoFin = miLstPuntosDirty.Last();

            miLstPuntosClear.Add(miPtoInicial);




            for (int i = 1; i < miLstPuntosDirty.Count - 1; i++)
            {
                miPtoPrevio = miLstPuntosDirty[i - 1];

                miPtoOrigen = miLstPuntosDirty[i];

                miPtoNext = miLstPuntosDirty[i + 1];

                miAnguloGrados = tadLayShare.puntos.oTrigo.getAngFrom3Point(miPtoOrigen.X, miPtoOrigen.Y,
                                                                   miPtoPrevio.X, miPtoPrevio.Y,
                                                                   miPtoNext.X, miPtoNext.Y, tadLayShare.puntos.eAng.grados);

                if (miAnguloGrados > 0 && miAnguloGrados < 180)
                {
                    miLstPuntosClear.Add(miPtoOrigen);
                }

            }


            miLstPuntosClear.Add(miPtoFin);


            return oLw.addLw2d(miLstPuntosClear, false, iLayer);


        }

        public static Polyline getOffset(Polyline iLwOrigen, double iOffSetConSigno, string iLayer)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {

                bool miOffSetPositivo;

                if (iOffSetConSigno > 0)
                {
                    miOffSetPositivo = true;
                }
                else
                {
                    miOffSetPositivo = false;
                }



                DBObjectCollection miColOffset;
                ObjectId miLwObjID = ObjectId.Null;
                Polyline miLwOffSet;

                miColOffset = new DBObjectCollection();
                miColOffset = iLwOrigen.GetOffsetCurves(iOffSetConSigno);

                foreach (Entity miEntidad in miColOffset)
                {
                    miLwObjID = engCadNet.oTools.entidadAdd(miEntidad, iLayer);
                }


                miLwOffSet = (Polyline)tr.GetObject(miLwObjID, OpenMode.ForWrite);


                double miLwOrigenCoordenadaY = iLwOrigen.GetPoint2dAt(0).Y;
                double miLwOffsetCoordenadaY = miLwOffSet.GetPoint2dAt(0).Y;

                //Debo de Comprobar Si el Sentido es Correcto
                if (miOffSetPositivo)
                {
                    if (miLwOrigenCoordenadaY > miLwOffsetCoordenadaY)
                    {
                        oTools.entidadDelete(miLwOffSet.ObjectId);
                        miColOffset = new DBObjectCollection();
                        miColOffset = iLwOrigen.GetOffsetCurves(-iOffSetConSigno);

                        foreach (Entity miEntidad in miColOffset)
                        {
                            miLwObjID = engCadNet.oTools.entidadAdd(miEntidad, iLayer);
                        }

                        miLwOffSet = (Polyline)tr.GetObject(miLwObjID, OpenMode.ForWrite);
                    }


                }

                else
                {
                    if (miLwOffsetCoordenadaY > miLwOrigenCoordenadaY)
                    {
                        oTools.entidadDelete(miLwOffSet.ObjectId);
                        miColOffset = new DBObjectCollection();
                        miColOffset = iLwOrigen.GetOffsetCurves(-iOffSetConSigno);

                        foreach (Entity miEntidad in miColOffset)
                        {
                            miLwObjID = engCadNet.oTools.entidadAdd(miEntidad, iLayer);
                        }


                        miLwOffSet = (Polyline)tr.GetObject(miLwObjID, OpenMode.ForWrite);
                    }


                }

                return miLwOffSet;

            }
        }

        public static double? getPkAtPoint(Point3d iPto, Polyline iLwEje)
        {
            double? miPk = null;
            bool miEncontrado = false;
            if (isPtoOnLw(iLwEje, iPto))
            {
                miPk = 0;
                for (int i = 0; i < iLwEje.NumberOfVertices - 2; i++)
                {
                    LineSegment2d miLinea = iLwEje.GetLineSegment2dAt(i);
                    double distance = miLinea.GetDistanceTo(new Point2d(iPto.X, iPto.Y));
                    if ((distance == 0) && (!miEncontrado))
                    {
                        miPk = miPk + miLinea.StartPoint.GetDistanceTo(new Point2d(iPto.X, iPto.Y));
                        miEncontrado = true;
                    }
                    else if (!miEncontrado)
                    {
                        miPk = miPk + miLinea.Length;
                    }
                }

            }

            return miPk;
        }

        public static int? getTramoAtPoint(Point3d iPto, Polyline iLwEje)
        {
            int? miTramo = null;
            if (isPtoOnLw(iLwEje, iPto))
            {
                miTramo = 0;
                for (int i = 0; i < iLwEje.NumberOfVertices - 2; i++)
                {
                    LineSegment2d miLinea = iLwEje.GetLineSegment2dAt(i);
                    double distance = miLinea.GetDistanceTo(new Point2d(iPto.X, iPto.Y));
                    if (distance == 0)
                    {
                        miTramo = i;
                    }
                }

            }

            return miTramo;
        }

        public static double? getAnguloIntersec(IP2d iPtoIni, IP2d iPtoFin, Polyline iLwEje)
        {
            double? miAngulo = null;

            Polyline miEje = new Polyline();
            for (int i = 0; i < iLwEje.NumberOfVertices; i++)
            {
                miEje.AddVertexAt(i, iLwEje.GetPoint2dAt(i), 0, 0, 0);
            }

            Polyline miLwTramo = new Polyline();
            miLwTramo.AddVertexAt(0, new Point2d(iPtoIni.X, iPtoIni.Y), 0, 0, 0);
            miLwTramo.AddVertexAt(1, new Point2d(iPtoFin.X, iPtoFin.Y), 0, 0, 0);


            Point3dCollection miColPtoInter = new Point3dCollection();
            miEje.IntersectWith(miLwTramo, Intersect.OnBothOperands, miColPtoInter, IntPtr.Zero, IntPtr.Zero);

            if (miColPtoInter.Count != 0)
            {
                for (int i = 0; i < iLwEje.NumberOfVertices - 2; i++)
                {
                    LineSegment2d miLinea = iLwEje.GetLineSegment2dAt(i);
                    LinearEntity2d miLineaTramo = new LineSegment2d(new Point2d(iPtoIni.X, iPtoIni.Y), new Point2d(iPtoFin.X, iPtoFin.Y));
                    Point2d[] miPuntoIntersec = miLinea.IntersectWith(miLineaTramo);
                    if (miPuntoIntersec != null)
                    {

                        double miAzTramo = oTrigo.getAzimutGrados(new oP2d(iPtoIni.X, iPtoIni.Y), new oP2d(iPtoFin.X, iPtoFin.Y));

                        oP2d miPuntoEje1 = new oP2d(miLinea.StartPoint.X, miLinea.StartPoint.Y);
                        oP2d miPuntoEje2 = new oP2d(miLinea.EndPoint.X, miLinea.EndPoint.Y);

                        double miAzEje = oTrigo.getAzimutGrados(miPuntoEje2, miPuntoEje1);

                        miAngulo = Math.Abs(miAzTramo - miAzEje);

                    }
                }
            }



            return miAngulo;
        }

        public static Polyline reverseLw(Polyline iLw)
        {
            Polyline LwReverse = new Polyline();
            if (iLw != null)
            {
                List<Point3d> misPuntos = oLw.getLstPto(iLw);
                int numPoints = misPuntos.Count;

                for (int i = 0; i < numPoints; i++)
                {
                    LwReverse.AddVertexAt(i, iLw.GetPoint2dAt(numPoints - i - 1), 0, 0, 0);
                }
            }
            return LwReverse;
        }

        public static List<Polyline> unirPolsInterccionan(List<Polyline> iPolylines)
        {
            List<Polyline> misUniones = new List<Polyline>();
            List<Polyline> misUnionesAux = new List<Polyline>();
            List<Region> misRegionesEliminar = new List<Region>();

            List<List<Polyline>> misIntersecc = new List<List<Polyline>>();

            misIntersecc.Add(iPolylines);
            Region[] misRegiones = new Region[iPolylines.Count];

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                try
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        int i = 0;

                        Region miRegionWrite = null;
                        foreach (Polyline miPol in iPolylines)
                        {
                            if (oRegion.isPolARegion(miPol))
                            {
                                Region miRegion = oRegion.addRegionFromLw(miPol, "0");
                                misRegiones[i] = miRegion;
                                if (miRegionWrite == null)
                                {
                                    miRegionWrite = (Region)tr.GetObject(miRegion.Id, OpenMode.ForWrite);
                                }
                                else
                                {
                                    misRegionesEliminar.Add(miRegion);
                                }
                            }
                            i++;
                        }


                        for (int j = 0; j < misRegiones.Count(); j++)
                        {
                            if (misRegiones[j] != null)
                            {
                                double area = miRegionWrite.Area;
                                miRegionWrite.BooleanOperation(BooleanOperationType.BoolUnite, misRegiones[j]);
                            }
                        }

                        foreach (Polyline mipol in oRegion.getPolylineFromRegion(miRegionWrite))
                        {
                            misUnionesAux.Add(mipol);
                        }
                        tr.Commit();
                    }

                    oRegion.eliminarRegiones(misRegionesEliminar);

                }
                catch (Exception e)
                {
                    return iPolylines;
                }

            }
            return misUnionesAux;



        }

        public static void eliminarPolilineas(List<Polyline> iPolylines)
        {
            foreach (Polyline miPolilinea in iPolylines)
            {

                engCadNet.oTools.entidadDelete(miPolilinea.ObjectId);
            }
        }

        private static bool isInsidePols(Polyline miPol1, Polyline miPol2)
        {
            bool isInside = false;

            for (int i = 0; i < miPol1.NumberOfVertices; i++)
            {
                if (oPolygon.isPointInPolygon(miPol2, miPol1.GetPoint2dAt(i).X, miPol1.GetPoint2dAt(i).Y))
                {
                    isInside = true;
                }
            }
            if (!isInside)
            {
                for (int i = 0; i < miPol2.NumberOfVertices; i++)
                {
                    if (oPolygon.isPointInPolygon(miPol1, miPol2.GetPoint2dAt(i).X, miPol2.GetPoint2dAt(i).Y))
                    {
                        isInside = true;
                    }
                }
            }


            return isInside;
        }

        public static List<Polyline> unirPolsInterccionanZNP(List<Polyline> iPolylines)
        {
            List<Polyline> misUniones = new List<Polyline>();
            List<Polyline> misUnionesAux = new List<Polyline>();
            List<Region> misRegionesEliminar = new List<Region>();

            List<List<Polyline>> misIntersecc = new List<List<Polyline>>();

            misIntersecc.Add(iPolylines);

            try
            {


                foreach (List<Polyline> miGrupoPol in misIntersecc)
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        if (miGrupoPol.Count > 1)
                        {

                            Region[] misRegiones = new Region[miGrupoPol.Count - 1];
                            for (int i = 0; i < miGrupoPol.Count; i++)
                            {
                                if (i != miGrupoPol.Count - 1)
                                {

                                    Polyline miPol = miGrupoPol[i];
                                    if (oRegion.isPolARegion(miPol))
                                    {
                                        Region miRegion = oRegion.addRegionFromLw(miPol, "0");
                                        misRegionesEliminar.Add(miRegion);
                                        misRegiones[i] = miRegion;
                                    }
                                }
                                else
                                {
                                    Polyline miPol = miGrupoPol[i];
                                    Region miRegionWrite;
                                    if (oRegion.isPolARegion(miPol))
                                    {
                                        Region miRegion = oRegion.addRegionFromLw(miPol, "0");
                                        misRegionesEliminar.Add(miRegion);
                                        miRegionWrite = (Region)tr.GetObject(miRegion.Id, OpenMode.ForWrite);
                                    }
                                    else
                                    {
                                        miRegionWrite = misRegiones[0];
                                    }

                                    for (int j = 0; j < miGrupoPol.Count - 1; j++)
                                    {
                                        if (misRegiones[j] != null)
                                        {
                                            miRegionWrite.BooleanOperation(BooleanOperationType.BoolUnite, misRegiones[j]);
                                        }
                                    }

                                    foreach (Polyline mipol in oRegion.getPolylineFromRegion(miRegionWrite))
                                    {
                                        misUnionesAux.Add(mipol);
                                    }
                                }

                            }
                        }
                        else
                        {
                            misUnionesAux.Add(miGrupoPol[0]);
                        }
                        tr.Commit();
                    }

                }
                oRegion.eliminarRegiones(misRegionesEliminar);
            }
            catch (Exception e)
            {
                return iPolylines;
            }
            return misUnionesAux;
        }

    }
}



