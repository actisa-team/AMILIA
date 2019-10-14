using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace engCadNet
{

    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Geometry;
    //using Autodesk.Aec.Geometry;
    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    
    
    public class oImage
    {


        public static ObjectId addImage(string iFilePath, Point3d iPtoIn, double iEscala, string iLayerName, bool iCreateLink)
        {

            if (! File.Exists(iFilePath))
            {
                throw new FileNotFoundException(iFilePath);     
            }
            
         
                string recBase = Path.GetFileNameWithoutExtension(iFilePath);
                
                using (Transaction tr = oCadManager.StartTransaction())
                {


                    ObjectId dictId = RasterImageDef.GetImageDictionary(oCadManager.thisBase);


                    if (dictId.IsNull)
                    {
                        // Image dictionary doesn't exist, create new
                        dictId = RasterImageDef.CreateImageDictionary(oCadManager.thisBase);
                    }

                    // Open the image dictionary
                    DBDictionary dict = (DBDictionary)tr.GetObject(dictId, OpenMode.ForRead);

                    // Get a unique record name for our raster image
                    // definition

                    int i = 0;
                    string recName = recBase + i.ToString();

                    while (dict.Contains(recName))
                    {
                        i++;
                        recName = recBase + i.ToString();
                    }

                    RasterImageDef rid = new RasterImageDef();

                    // Set its source image

                    rid.SourceFileName = iFilePath;

                    // Load it

                    rid.Load();
                    dict.UpgradeOpen();

                    ObjectId defId = dict.SetAt(recName, rid);

                    // Let the transaction know

                    tr.AddNewlyCreatedDBObject(rid, true);

                    // Create the raster image that references the
                    // definition

                    RasterImage ri = new RasterImage();
                    ri.ImageDefId = defId;
                    ri.ShowImage = true;
                    ri.Layer = iLayerName;
                    ri.Orientation = new CoordinateSystem3d(iPtoIn, new Vector3d(1, 0, 0), new Vector3d(0, 1, 0));

          
                    //Escalo la Imagen

                    Matrix3d myMatrix = new Matrix3d();
                    myMatrix = Matrix3d.Scaling(iEscala, iPtoIn);

                    ri.TransformBy(myMatrix);

                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);


                    btr.AppendEntity(ri);
                    tr.AddNewlyCreatedDBObject(ri, true);

                    // Create a reactor between the RasterImage and the
                    // RasterImageDef to avoid the "unreferenced"
                    // warning in the XRef palette

                   
                    RasterImage.EnableReactors(iCreateLink);
                    ri.AssociateRasterDef(rid);
                    

                    tr.Commit();

                    return ri.ObjectId;

                }
        }
    }
}
