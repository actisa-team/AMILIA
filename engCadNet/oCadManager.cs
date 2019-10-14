using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{
   
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    //using Autodesk.Civil.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;
    
    public class oCadManager
    {
  
         //using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            

        public static Database thisBase
        {
            get { return HostApplicationServices.WorkingDatabase; }
        }

        public static Document thisMdi
        {
            get { return Application.DocumentManager.MdiActiveDocument; }
        
        }

        public static DocumentCollection documenteManager
        {
            get { return Application.DocumentManager; }

        }

        public static Editor thisEditor
        {
            get { return Application.DocumentManager.MdiActiveDocument.Editor; }
        }

        public static Transaction StartTransaction()
        {
            return HostApplicationServices.WorkingDatabase.TransactionManager.StartTransaction();
        }


        public static T getEntidadRead<T>(string iHandle) where T : DBObject
        {
             DBObject miDbObject = null;
            
            using (Transaction tr = oCadManager.StartTransaction())
            {
                long miLong = Convert.ToInt64(iHandle, 16);
                Handle miHandle = new Handle(miLong);
                ObjectId miObjId = oCadManager.thisBase.GetObjectId(false, miHandle, 0);
               
                miDbObject = tr.GetObject(miObjId, OpenMode.ForRead);

                tr.Commit();

                return (T)miDbObject;
            }
        }

        public static T getEntidadRead<T>(ObjectId iObjId) where T : DBObject
        {

           


            DBObject miDbObject = null;
            
            using (Transaction tr = oCadManager.StartTransaction())
            {

                miDbObject = tr.GetObject(iObjId, OpenMode.ForRead);  

                tr.Commit();

                return (T)miDbObject;
            }
        }

    }


}
