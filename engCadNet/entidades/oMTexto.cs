using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{


    using tadLayShare;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.GraphicsInterface;
    
    
    public static class oMTexto
    {

        public static void addMText2D(string iTexto, double iX, double iY, double iAltura, double iRotateRadianes, string iLayer)
        {
            
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable miBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord miBlockTableRec = tr.GetObject(miBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    MText miTexto = new MText();

                    miTexto.Contents = iTexto;

                    miTexto.TextHeight = iAltura;

                    miTexto.SetDatabaseDefaults();

                    miTexto.Location = new Point3d(iX, iY, 0);

                    miTexto.Rotation = iRotateRadianes;

                    miTexto.Layer = iLayer;

                   // miTexto.Attachment = AttachmentPoint.BottomMid; //Salta Exception
       
                    miBlockTableRec.AppendEntity(miTexto);

                    tr.AddNewlyCreatedDBObject(miTexto, true);

                    tr.Commit();

                }
            

        }

        public static void addMText2D(string iStr, double[] iPto, double iAltura, double iRotate, string iLayer)
        {

          
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    MText miTexto = new MText();

                    miTexto.TextHeight = iAltura;

                    miTexto.Location = new Point3d(iPto[0], iPto[1], 0);

                    miTexto.Rotation = iRotate;

                    miTexto.Contents = iStr;

                    miTexto.Layer = iLayer;

                    acBlockTableRec.AppendEntity(miTexto);

                    tr.AddNewlyCreatedDBObject(miTexto, true);

                    tr.Commit();

                }
            

        }


    }
}
