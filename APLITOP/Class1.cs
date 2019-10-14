using System.Windows.Forms;
using MaterialSkin;
using Datos;
using interfaz;
using Logica;
namespace APLITOP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Interop;
    using Autodesk.AutoCAD.Interop.Common;
    using Autodesk.AutoCAD.Windows;
    using System.IO;
    using System.Reflection;
    using interfaz;
    
    public class Class1
    {
        
        #region "COMANDOS APLITOP"
        [CommandMethod("aplitop")]
        public static void aplitop()
        {
            /*
            Document acDoc = Application.DocumentManager.MdiActiveDocument  ;
            Database AcCurDb = acDoc.Database;
            
                using (Transaction acTrans = AcCurDb.TransactionManager.StartTransaction())
                {
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(AcCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    Editor e = Application.DocumentManager.MdiActiveDocument.Editor;
                    Document d = Application.DocumentManager.MdiActiveDocument;

                    PromptResult r = e.GetString("estriba su nombre");
                    e.WriteMessage("Hola" + r.StringResult);
                    Line l = new Line(new Point3d(0, 0, 0), new Point3d(50, 0, 0));

                    acBlkTblRec.AppendEntity(l);
                    acTrans.AddNewlyCreatedDBObject(l, true);

                    acTrans.Commit();
                }
            */

 /*            string strTemplatePath = "ejemplo.dwg";

            DocumentCollection acDocMgr = Application.DocumentManager;
            Document acDoc = acDocMgr.Add(strTemplatePath);
            
           // acDocMgr.MdiActiveDocument = acDoc;
            DBObjectCollection a =acDoc.TransactionManager.GetAllObjects();*/
             try
                 {
                     principal p = new principal();
                     p.Show();
                 }
                 catch (System.Exception ex)
                 {
                     System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
                 }

             }
            }
#endregion

}
