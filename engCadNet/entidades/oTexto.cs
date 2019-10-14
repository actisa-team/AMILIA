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

    
    public class oTexto
    {


        public static void addText3D(string iTexto,double[] iPto, double iAltura, double iRotateRadianes, int iColorIndex, string iLayer)
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable miBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord miBlockTableRec = tr.GetObject(miBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    DBText miText = new DBText();

                    miText.SetDatabaseDefaults();

                    miText.ColorIndex = iColorIndex;

                    miText.Height = iAltura;

                    //////////////////////////////////////////////////////
                    miText.HorizontalMode = TextHorizontalMode.TextLeft;

                    miText.VerticalMode = TextVerticalMode.TextVerticalMid;
                    ///////////////////////////////////////////////////////

                    miText.Position = new Point3d(iPto);

                    miText.AlignmentPoint = miText.Position;

                    miText.Rotation = iRotateRadianes;

                    miText.TextString = iTexto;

                    miText.Layer = iLayer;

                    miBlockTableRec.AppendEntity(miText);

                    tr.AddNewlyCreatedDBObject(miText, true);

                    tr.Commit();

                }
            }

        }

        public static void addText2D (string iTexto, double iX, double iY, double iAltura, double iRotateRadianes, int iColorIndex, string iLayer)
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable miBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord miBlockTableRec = tr.GetObject(miBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    DBText miText = new DBText();

                    miText.SetDatabaseDefaults();

                    miText.ColorIndex = iColorIndex;

                    miText.Height = iAltura;

                    //////////////////////////////////////////////////////
                    miText.HorizontalMode = TextHorizontalMode.TextLeft;

                    miText.VerticalMode = TextVerticalMode.TextVerticalMid;
                    ///////////////////////////////////////////////////////

                    miText.Position = new Point3d(iX, iY, 0);

                    miText.AlignmentPoint = miText.Position;

                    miText.Rotation = iRotateRadianes;

                    miText.TextString = iTexto;

                    miText.Layer = iLayer;

                    miBlockTableRec.AppendEntity(miText);

                    tr.AddNewlyCreatedDBObject(miText, true);

                    tr.Commit();

                }
            }

        }




       

    }
}
