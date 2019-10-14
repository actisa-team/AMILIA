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

    public class oLine
    {
        public static void addLine2dWithTextoPuntoMedio (double[] iP1, double[] iP2, string iTexto,double iTxtAltura, string iLayerName)
        {

            Line miLine = addLine(new Point3d(iP1), new Point3d(iP2), iLayerName);

            oText.addText2D(iTexto, miLine.EndPoint.ToArray(), iTxtAltura, miLine.Angle, iLayerName);


        }
        public static Line addLine3d(double iP1X, double iP1Y,double iP1Z, double iP2X, double iP2Y,double iP2Z, string iLayerName)
        {
            Point3d myP1 = new Point3d(iP1X, iP1Y, iP1Z);
            Point3d myP2 = new Point3d(iP2X, iP2Y, iP2Z);
       
            return  addLine(myP1, myP2, iLayerName);
        }
        public static Line addLine2d(Point2d iP1, Point2d iP2, string iLayerName)
        {

            Point3d myP1 = new Point3d(iP1.X, iP1.Y, 0);
            Point3d myP2 = new Point3d(iP2.X, iP2.Y, 0);

            return addLine(myP1, myP2, iLayerName);
        } 
        public static void addLine2dVoid(double iP1X, double iP1Y, double iP2X, double iP2Y, string iLayerName)
        {

            Point3d myP1 = new Point3d(iP1X, iP1Y, 0);
            Point3d myP2 = new Point3d(iP2X, iP2Y, 0);

            addLine(myP1, myP2, iLayerName);      
        }
        public static void addLine3dVoid(double[] iP1,double[] iP2, string iLayerName)
        {

            Point3d myP1 = new Point3d(iP1[0], iP1[1], iP1[2]);
            Point3d myP2 = new Point3d(iP2[0], iP2[1], iP2[2]);

            addLine(myP1, myP2, iLayerName);
        }       
        public static void addLine3dVoid(double iP1X, double iP1Y, double iP1Z, double iP2X, double iP2Y, double iP2Z, string iLayerName)
        {

            Point3d myP1 = new Point3d(iP1X, iP1Y, iP1Z);
            Point3d myP2 = new Point3d(iP2X, iP2Y,iP2Z);

            addLine(myP1, myP2, iLayerName);
        }       
        public static Line addLine2d(double iP1X, double iP1Y, double iP2X, double iP2Y, string iLayerName)
        {
            Point3d myP1 = new Point3d(iP1X, iP1Y, 0);
            Point3d myP2 = new Point3d(iP2X, iP2Y, 0);

            return addLine(myP1, myP2, iLayerName);    
        }
        public static Line addLine(Point3d iP1, Point3d iP2, string iLayer)
        {

                using (Transaction tr = oCadManager.StartTransaction())
                {
                    try
                    {
                        BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                        //Necesito Crear un Nuevo Registro para Añadir la Linea
                        BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        Line myLine = new Line(iP1, iP2);

                        //configuro los Datos por Defecto

                        myLine.SetDatabaseDefaults();

                        myLine.Layer = iLayer;

                        acBlockTableRec.AppendEntity(myLine);

                        tr.AddNewlyCreatedDBObject(myLine, true);

                        tr.Commit();

                        return myLine;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(string.Format(strError.eAddLineaEnCapa, iLayer));
                    }

                }  
        }
        public static Line addLine(double iLonOrigen, double iDelta, eVerHor iPosicion, string iLayer, Matrix3d iMatrix)
        {

                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    Point3d myP1;
                    Point3d myP2;


                    if (iPosicion == eVerHor.vertical)
                    {
                        myP1 = new Point3d(iLonOrigen, -iDelta / 2, 0);
                        myP2 = new Point3d(iLonOrigen, iDelta / 2, 0);  
                    }
                    else if (iPosicion == eVerHor.horizontal)
                    {
                        myP1 = new Point3d(-iDelta / 2, iLonOrigen, 0);
                        myP2 = new Point3d(iDelta / 2, iLonOrigen, 0);

                    }
                    else
                    {
                        throw new Exception(iPosicion.ToString() + strError.eNoExiste);
                    
                    }

                    
                    Line myLine = new Line(myP1, myP2);

                    //configuro los Datos por Defecto

                    myLine.SetDatabaseDefaults();

                    myLine.Layer = iLayer;

                    myLine.TransformBy(iMatrix);

                    acBlockTableRec.AppendEntity(myLine);

                    tr.AddNewlyCreatedDBObject(myLine, true);

                    tr.Commit();

                    return myLine;

                }
            }
    }
}
