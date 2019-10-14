using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet.entidades
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    
    
    
    public class oLwPlus : IDisposable

    {

       private Transaction mTr;
       private Polyline mLw;


       public oLwPlus(ObjectId iObjId)
       {

           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {
               mTr = oCadManager.StartTransaction();

               mLw = (Polyline)mTr.GetObject(iObjId, OpenMode.ForWrite);
           }
       
       
       }


       public oLwPlus (DBObject iLw)
       {
           

           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {
               mTr = oCadManager.StartTransaction();
               
               mLw = (Polyline)mTr.GetObject(iLw.ObjectId, OpenMode.ForWrite);
           }

       }


       public Polyline lw
       {
           get
           {
               return mLw;
           }
       }


       public void Dispose()
       {
           mTr.Commit();
       }


    }
}
