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
    using tadLayShare.puntos;

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
        public static double[] getPointAtDist(double iDistancia, Line iLinea)
        {
            double[] miPunto = new double[2];
            double miPK = iDistancia;


            double miAzimut = oTrigo.getAzimutGrados(new oP2d(iLinea.StartPoint.X, iLinea.StartPoint.Y), new oP2d(iLinea.EndPoint.X, iLinea.EndPoint.Y));

            double miXi = iLinea.StartPoint.X + miPK * Math.Sin(miAzimut * Math.PI / 180);
            double miYi = iLinea.StartPoint.Y + miPK * Math.Cos(miAzimut * Math.PI / 180);

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }
        public static double[] getPointAtLocation(double iDist, double iOffset, bool iDerecha, Line iLinea)
        {

            //double miDistIniFin = iLinea.Length;
            //double miDistPMedio = miDistIniFin / 2;

            double[] miPuntoDis = getPointAtDist(iDist, iLinea);

            double[] miPunto = new double[2];


            double miD1 = iLinea.EndPoint.X - iLinea.StartPoint.X;
            double miD2 = iLinea.EndPoint.Y - iLinea.StartPoint.Y;

            double miAzimut = getAzimut(miD1, miD2);


            double miAzimutTrans;
            if ((miAzimut < 180) && (miAzimut > 90))
            {
                miAzimut = miAzimut + 180;

            }
            else if ((miAzimut < 360) && (miAzimut > 270))
            {
                miAzimut = miAzimut - 180;
            }
            if (iDerecha)
            {
                if (miAzimut - 90 >= 0)
                {
                    miAzimutTrans = miAzimut - 90;
                }
                else
                {
                    miAzimutTrans = miAzimut - 90 + 360;
                }

            }
            else
            {
                if (miAzimut + 90 >= 360)
                {
                    miAzimutTrans = miAzimut + 90 - 360;
                }
                else
                {
                    miAzimutTrans = miAzimut + 90;
                }
            }

            miPunto[0] = miPuntoDis[0] + iOffset * Math.Cos(miAzimutTrans * Math.PI / 180);
            miPunto[1] = miPuntoDis[1] + iOffset * Math.Sin(miAzimutTrans * Math.PI / 180);

            return miPunto;
        }
        public static double getAzimut(double iDx, double iDy)
        {

            double miDelta;
            if ((iDx == 0) || (iDy == 0))
            {
                miDelta = 0;
            }
            else
            {
                miDelta = Math.Atan(iDx / iDy);
            }
            double miDeltaGra = miDelta * 180 / Math.PI;
            double miAzimut;

            if (miDeltaGra == 0)
            {
                if (iDy == 0)
                {
                    if (iDx < 0)
                    {
                        miAzimut = 180;
                    }
                    else
                    {
                        miAzimut = 0;
                    }
                }
                else
                {
                    if (iDx < 0)
                    {
                        miAzimut = 270;
                    }
                    else
                    {
                        miAzimut = 90;
                    }
                }
            }
            else
            {
                if (miDeltaGra < 0)
                {
                    if (iDx >= 0)
                    {
                        miAzimut = 90 - miDeltaGra;
                    }
                    else
                    {
                        miAzimut = 270 - miDeltaGra;
                    }
                }
                else
                {
                    if (iDy >= 0)
                    {
                        miAzimut = 90 - miDeltaGra;
                    }
                    else
                    {
                        miAzimut = 270 - miDeltaGra;
                    }
                }
            }
            return miAzimut;
        }
    }
}
