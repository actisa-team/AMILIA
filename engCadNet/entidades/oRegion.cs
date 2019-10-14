using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{
    
     using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using tadLayLan;




    public class oRegion
    {


        public static Region createRegionFromLw1_Lw2(Polyline iLwMayorArea, Polyline iLwMenorArea)
        {

            DBObjectCollection myObjCol = new DBObjectCollection();
            myObjCol.Add(iLwMayorArea);
            myObjCol.Add(iLwMenorArea);

            DBObjectCollection myObjColRegion = new DBObjectCollection();


            myObjColRegion = Region.CreateFromCurves(myObjCol);


            Region miRegion_0 = myObjColRegion[0] as Region;
            Region miRegion_1 = myObjColRegion[1] as Region;


            if (miRegion_0.Area > miRegion_1.Area)
            {
                miRegion_0.BooleanOperation(BooleanOperationType.BoolSubtract, miRegion_1);

                miRegion_1.Dispose();

                return miRegion_0;
            }
            else
            {
                miRegion_1.BooleanOperation(BooleanOperationType.BoolSubtract, miRegion_0);

                miRegion_0.Dispose();

                return miRegion_1;
               
            }

        }


        public static Region addRegionFromLw (Polyline iLw, string iLayer)
        {

                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForWrite) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    DBObjectCollection myObjCol = new DBObjectCollection();


                    Polyline miLw = (Polyline)tr.GetObject(iLw.Id, OpenMode.ForWrite);
                    miLw.Closed = true;
                    myObjCol.Add(miLw); 

                    DBObjectCollection myObjColRegion = new DBObjectCollection();

                   
                    myObjColRegion = Region.CreateFromCurves(myObjCol);


                    if (myObjColRegion == null | myObjColRegion.Count == 0)
                    {
                        throw new Exception(strError.eGetRegion);
                    }
                    else
                    {
                        Region myRegion = myObjColRegion[0] as Region;

                        //configuro los Datos por Defecto

                        myRegion.SetDatabaseDefaults();

                        myRegion.Layer = iLayer;

                        acBlockTableRec.AppendEntity(myRegion);

                        tr.AddNewlyCreatedDBObject(myRegion, true);

                        tr.Commit();

                        return myRegion;

                    }

            }


        }


        public static void eliminarRegiones(List<Region> iRegions)
        {
            foreach (Region miRegion in iRegions)
            {
                try
                {

                    engCadNet.oTools.entidadDelete(miRegion.ObjectId);
                }
                catch (Exception e)
                { 
                }
            }
        }
        public static DBObjectCollection getPolylineFromRegion(Region reg)
        {


            DBObjectCollection res = new DBObjectCollection();

            try
            {
                // Explode Region -> collection of Curves / Regions

                DBObjectCollection cvs = new DBObjectCollection();

                reg.Explode(cvs);

                // Create a plane to convert 3D coords
                // into Region coord system

                Plane pl = new Plane(new Point3d(0, 0, 0), reg.Normal);


                using (pl)
                {
                    bool finished = false;

                    while (!finished && cvs.Count > 0)
                    {
                        // Count the Curves and the non-Curves, and find
                        // the index of the first Curve in the collection

                        int cvCnt = 0, nonCvCnt = 0, fstCvIdx = -1;

                        for (int i = 0; i < cvs.Count; i++)
                        {
                            Curve tmpCv = cvs[i] as Curve;
                            if (tmpCv == null)
                                nonCvCnt++;
                            else
                            {
                                // Closed curves can go straight into the
                                // results collection, and aren't added
                                // to the Curve count

                                if (tmpCv.Closed)
                                {
                                    res.Add(tmpCv);
                                    cvs.Remove(tmpCv);
                                    // Decrement, so we don't miss an item
                                    i--;
                                }
                                else
                                {
                                    cvCnt++;
                                    if (fstCvIdx == -1)
                                        fstCvIdx = i;
                                }
                            }
                        }

                        if (fstCvIdx >= 0)
                        {
                            // For the initial segment take the first
                            // Curve in the collection

                            Curve fstCv = (Curve)cvs[fstCvIdx];

                            // The resulting Polyline

                            Polyline p = new Polyline();

                            // Set common entity properties from the Region

                            p.SetPropertiesFrom(reg);

                            // Add the first two vertices, but only set the
                            // bulge on the first (the second will be set
                            // retroactively from the second segment)

                            // We also assume the first segment is counter-
                            // clockwise (the default for arcs), as we're
                            // not swapping the order of the vertices to
                            // make them fit the Polyline's order

                            p.AddVertexAt(
                              p.NumberOfVertices,
                              fstCv.StartPoint.Convert2d(pl),
                              BulgeFromCurve(fstCv, false), 0, 0
                            );

                            p.AddVertexAt(
                              p.NumberOfVertices,
                              fstCv.EndPoint.Convert2d(pl),
                              0, 0, 0
                            );

                            cvs.Remove(fstCv);

                            // The next point to look for

                            Point3d nextPt = fstCv.EndPoint;

                            // Find the line that is connected to
                            // the next point

                            // If for some reason the lines returned were not
                            // connected, we could loop endlessly.
                            // So we store the previous curve count and assume
                            // that if this count has not been decreased by
                            // looping completely through the segments once,
                            // then we should not continue to loop.
                            // Hopefully this will never happen, as the curves
                            // should form a closed loop, but anyway...

                            // Set the previous count as artificially high,
                            // so that we loop once, at least.

                            int prevCnt = cvs.Count + 1;
                            while (cvs.Count > nonCvCnt && cvs.Count < prevCnt)
                            {
                                prevCnt = cvs.Count;
                                foreach (DBObject obj in cvs)
                                {
                                    Curve cv = obj as Curve;

                                    if (cv != null)
                                    {
                                        // If one end of the curve connects with the
                                        // point we're looking for...

                                        if (cv.StartPoint == nextPt ||
                                            cv.EndPoint == nextPt)
                                        {
                                            // Calculate the bulge for the curve and
                                            // set it on the previous vertex

                                            double bulge =
                                              BulgeFromCurve(cv, cv.EndPoint == nextPt);
                                            if (bulge != 0.0)
                                                p.SetBulgeAt(p.NumberOfVertices - 1, bulge);

                                            // Reverse the points, if needed

                                            if (cv.StartPoint == nextPt)
                                                nextPt = cv.EndPoint;
                                            else
                                                // cv.EndPoint == nextPt
                                                nextPt = cv.StartPoint;

                                            // Add out new vertex (bulge will be set next
                                            // time through, as needed)

                                            p.AddVertexAt(
                                              p.NumberOfVertices,
                                              nextPt.Convert2d(pl),
                                              0, 0, 0
                                            );

                                            // Remove our curve from the list, which
                                            // decrements the count, of course

                                            cvs.Remove(cv);
                                            break;
                                        }
                                    }
                                }
                            }

                            // Once we have added all the Polyline's vertices,
                            // transform it to the original region's plane

                            p.TransformBy(Matrix3d.PlaneToWorld(pl));
                            res.Add(p);

                            if (cvs.Count == nonCvCnt)
                                finished = true;
                        }

                        // If there are any Regions in the collection,
                        // recurse to explode and add their geometry

                        if (nonCvCnt > 0 && cvs.Count > 0)
                        {
                            foreach (DBObject obj in cvs)
                            {
                                Region subReg = obj as Region;
                                if (subReg != null)
                                {
                                    DBObjectCollection subRes = getPolylineFromRegion(subReg);


                                    foreach (DBObject o in subRes)
                                        res.Add(o);

                                    cvs.Remove(subReg);
                                }
                            }
                        }
                        if (cvs.Count == 0)
                            finished = true;
                    }
                }
            }
            catch
            {
                return res;
            }
            return res;
        }    
        private static double BulgeFromCurve(Curve cv, bool clockwise)
        {
            double bulge = 0.0;

            Arc a = cv as Arc;
            if (a != null)
            {
                double newStart;

                // The start angle is usually greater than the end,
                // as arcs are all counter-clockwise.
                // (If it isn't it's because the arc crosses the
                // 0-degree line, and we can subtract 2PI from the
                // start angle.)

                if (a.StartAngle > a.EndAngle)
                    newStart = a.StartAngle - 8 * Math.Atan(1);
                else
                    newStart = a.StartAngle;

                // Bulge is defined as the tan of
                // one fourth of the included angle

                bulge = Math.Tan((a.EndAngle - newStart) / 4);

                // If the curve is clockwise, we negate the bulge

                if (clockwise)
                    bulge = -bulge;
            }
            return bulge;
        }

        public static bool isPolARegion(Polyline iLw)
        {
            bool isPolARegion = true;
            try
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForWrite) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    DBObjectCollection myObjCol = new DBObjectCollection();


                    Polyline miLw = (Polyline)tr.GetObject(iLw.Id, OpenMode.ForWrite);
                    miLw.Closed = true;
                    myObjCol.Add(miLw);

                    DBObjectCollection myObjColRegion = new DBObjectCollection();


                    myObjColRegion = Region.CreateFromCurves(myObjCol);


                    if (myObjColRegion == null | myObjColRegion.Count == 0)
                    {
                        isPolARegion = false;
                    }
                    return isPolARegion;
                }
            }
            catch
            {
                return false;
            }
        }
    }

}
