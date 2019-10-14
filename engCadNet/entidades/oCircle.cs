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
    
    
    public  class oCircle
    {

        public static void addCircle3D(Point3d iPto, double iRadio, string iLayer)
        {
            addCircle3D(iPto.ToArray(), iRadio, iLayer);
        }


    
        public static void addCircle3D(double[] iPto, double iRadio, string iLayer)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    Circle myCircle = new Circle();

                    myCircle.SetDatabaseDefaults();
                    myCircle.Center = new Point3d(iPto[0], iPto[1], iPto[2]);
                    myCircle.Radius = iRadio;
                    myCircle.Layer = iLayer;
                    acBlockTableRec.AppendEntity(myCircle);
                    tr.AddNewlyCreatedDBObject(myCircle, true);
                    tr.Commit();

                }

            }
        }


        public static void addCircle2D(double[] iPto, double iRadio, string iLayer)
        {

            addCircle2D(iPto[0], iPto[1], iRadio, iLayer);

        }


       public static void addCircle2D(double iX, double iY, double iRadio, string iLayer)
       {

               using (Transaction tr = oCadManager.StartTransaction())
               {

                   BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                   //Necesito Crear un Nuevo Registro para Añadir la Linea
                   BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
               
                   Circle myCircle = new Circle();                   

                   myCircle.SetDatabaseDefaults();
                   myCircle.Center = new Point3d(iX, iY, 0);
                   myCircle.Radius = iRadio;
                   myCircle.Layer = iLayer;
                   acBlockTableRec.AppendEntity(myCircle);
                   tr.AddNewlyCreatedDBObject(myCircle, true);
                   tr.Commit();

               }    
       }


       public static Point3d getCenterTreePoints(Point3d iP1, Point3d iP2, Point3d iP3)
       {

           CircularArc3d myArco = new CircularArc3d(iP1, iP2, iP3);

           return myArco.Center;
       
       }


       public static Point2d  getCenterTreePoints(Point2d iP1, Point2d iP2, Point2d iP3, out double iRadio)
       {

           CircularArc2d myArco = new CircularArc2d(iP1, iP2, iP3);

           iRadio = myArco.Radius;

           return myArco.Center;
       
       }


    }
}
