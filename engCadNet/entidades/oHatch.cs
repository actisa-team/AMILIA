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
    
    
    public class oHatch
    {


        public static void addHatchSolid(Point3dCollection iPtoCol, int iTransperenciaPorciento, Color iColor, string iLayer)
        {
                using (Transaction tr = oCadManager.StartTransaction())
                {


                    BlockTable acBlkTbl = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    // Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                    ObjectIdCollection acObjIdColl = new ObjectIdCollection();


                    Polyline miPol = new Polyline(iPtoCol.Count);

                    
                    int myVertexNum = 0;

                    foreach (Point3d myPto in iPtoCol)
                    {
                        miPol.AddVertexAt(myVertexNum, myPto.to2d(), 0, 0, 0);

                        myVertexNum++;
                    }

                    miPol.SetDatabaseDefaults();

                    miPol.Closed = true;

                    miPol.Layer = iLayer;


                    acBlkTblRec.AppendEntity(miPol);
                    tr.AddNewlyCreatedDBObject(miPol, true);


                    acObjIdColl.Add(miPol.ObjectId);

                    // Create the hatch object and append it to the block table record
                    Hatch acHatch = new Hatch();
                    acBlkTblRec.AppendEntity(acHatch);
                    tr.AddNewlyCreatedDBObject(acHatch, true);
                    // Set the properties of the hatch object
                    // Associative must be set after the hatch object is appended to the
                    // block table record and before AppendLoop
                    acHatch.SetDatabaseDefaults();

                    acHatch.SetHatchPattern(HatchPatternType.PreDefined, eHatch.SOLID.ToString());

                    byte miTransparencia = Convert.ToByte((255*(100-iTransperenciaPorciento)/100));

                    acHatch.Transparency = new Transparency(miTransparencia);
                    acHatch.Layer = iLayer;
                    acHatch.Color = iColor;           
                    acHatch.Associative = false;

                    acHatch.AppendLoop(HatchLoopTypes.Outermost, acObjIdColl);
                    acHatch.EvaluateHatch(true);
                    // Save the new object to the database

                   // miPol.Erase();

                    tr.Commit();

                }
        }




 

        public  static ObjectId addHatch(Entity iEntidad, eHatch iHatchName, Color iHatchColor, string iLayer)
        {
        
                using (Transaction tr = oCadManager.StartTransaction())
                {
      
                   BlockTable  acBlkTbl = tr.GetObject(oCadManager.thisBase.BlockTableId,OpenMode.ForRead) as BlockTable;

                   // Open the Block table record Model space for write
                   BlockTableRecord  acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],OpenMode.ForWrite) as BlockTableRecord;


                           ObjectIdCollection acObjIdColl = new ObjectIdCollection();
                           acObjIdColl.Add(iEntidad.ObjectId);


                            // Create the hatch object and append it to the block table record
                            Hatch acHatch = new Hatch();
                            acBlkTblRec.AppendEntity(acHatch);
                            tr.AddNewlyCreatedDBObject(acHatch, true);
                            // Set the properties of the hatch object
                            // Associative must be set after the hatch object is appended to the
                            // block table record and before AppendLoop
                            acHatch.SetDatabaseDefaults();
                           
                            acHatch.SetHatchPattern(HatchPatternType.PreDefined, iHatchName.ToString());
                            acHatch.Layer = iLayer;
                            acHatch.Color = iHatchColor;
                            acHatch.Associative = true;

                          
                            acHatch.AppendLoop(HatchLoopTypes.Outermost, acObjIdColl);
                            acHatch.EvaluateHatch(true);

                          
                            tr.Commit();

                            return acHatch.ObjectId;
              

                }


              }     
    }
}
