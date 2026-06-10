using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Datos;
using Autodesk.AutoCAD.Geometry;
using engCadNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.Interop;

namespace Logica
{
    public class GuardarPunto
    {
        public GuardarPunto(ref Point3d fstCnr)
        {
            Set_Punto(ref fstCnr);

        }

        private void Set_Punto(ref Point3d fstCnr)
        {


            /*Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            doc.Window.Focus();
            doc.SendStringToExecute("\x03\x03", false, true, false);*/
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            if (acDoc.CommandInProgress.Count()==0)
            {
                Database acCurDb = acDoc.Database;

                using (DocumentLock docLock = acDoc.LockDocument())
                {
                    Editor acEd = acDoc.Editor;

                    // Starts a new transaction with the Transaction Manager.
                    using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                    {

                        // Open the Block table record for read.
                        BlockTable acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                        // Open the Block table record Model Space for write.
                        BlockTableRecord acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        // Prompt for user input.
                        // Click the first rectangle's corner.
                        PromptPointOptions fstCnrPtOpt = new PromptPointOptions("\nIndique donde quiere que se imprima el resultado");
                        PromptPointResult fstCnrPtRes = acDoc.Editor.GetPoint(fstCnrPtOpt);
                        fstCnr = fstCnrPtRes.Value;
                        // Saves the changes to the database and closes the transaction.
                        acTrans.Commit();
                    }
                }
            }
            else
            {

            }
            
        }

    }
}
