using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{

	using System.IO;

	using Autodesk.AutoCAD.Runtime;
	using Autodesk.AutoCAD.ApplicationServices;
	using Autodesk.AutoCAD.EditorInput;
	using Autodesk.AutoCAD.Geometry;
	using Autodesk.AutoCAD.DatabaseServices;

	

	using tadLayShare;
	
	
	public class oExport
	{


		public static void exportarEntidades(ObjectIdCollection iLstEntidades, string iFileConExtension, bool iDeleteOriginalEntidades)
		{
			// get the working database (in AutoCAD)
			Database sourceDb = oCadManager.thisBase;

			// create a new destination database
			using (Database destDb = new Database(true, true))
			{
				// get the model space object ids for both dbs
				ObjectId sourceMsId = SymbolUtilityServices.GetBlockModelSpaceId(sourceDb);
				ObjectId destDbMsId = SymbolUtilityServices.GetBlockModelSpaceId(destDb);

				// next prepare to deepclone the recorded ids to the destdb
				IdMapping mapping = new IdMapping();

				// now clone the objects into the destdb

				sourceDb.WblockCloneObjects(iLstEntidades, destDbMsId, mapping, DuplicateRecordCloning.Replace, false);

				destDb.SaveAs(iFileConExtension, DwgVersion.Current);     
			 }


			if (iDeleteOriginalEntidades)
			{
				eraseCollection(iLstEntidades);
			}

		}


		public static void eraseCollection (ObjectIdCollection iLstEntidades)
		{


			ObjectIdCollection miColObjToBorrarMemoria = new ObjectIdCollection();
			DBObject miObjCad;

			using (Transaction tr = oCadManager.StartTransaction())
			{

				foreach (ObjectId item in iLstEntidades)
				{
		 
					miObjCad = tr.GetObject(item,OpenMode.ForRead);

					miObjCad.UpgradeOpen();

					miObjCad.Erase();

					miColObjToBorrarMemoria.Add(item);
				}

			tr.Commit();

		  
			}

			oCadManager.thisBase.ReclaimMemoryFromErasedObjects(miColObjToBorrarMemoria);
			
		}



		public static void exportarEntidades(ObjectIdCollection iListadoEntidades, string iFileExport)
		{


			ObjectIdCollection acObjIdColl = new ObjectIdCollection();

			// Change the file and path to match a drawing template on your workstation
			string sLocalRoot = Application.GetSystemVariable("LOCALROOTPREFIX") as string;
			string sTemplatePath = sLocalRoot + "Template\\acad.dwt";

			// Create a new drawing to copy the objects to
			DocumentCollection miColleccionDocumentos = oCadManager.documenteManager;
			Document miDocumentoNew = miColleccionDocumentos.Add(sTemplatePath);
			Database miBaseDatosNew = miDocumentoNew.Database;

			// Lock the new document
			using (DocumentLock acLckDoc = miDocumentoNew.LockDocument())
			{
				// Start a transaction in the new database
				using (Transaction acTrans = miDocumentoNew.TransactionManager.StartTransaction())
				{
					// Open the Block table for read
					BlockTable acBlkTblNewDoc;

					acBlkTblNewDoc = acTrans.GetObject(miBaseDatosNew.BlockTableId, OpenMode.ForRead) as BlockTable;

					// Open the Block table record Model space for read
					BlockTableRecord acBlkTblRecNewDoc;

					acBlkTblRecNewDoc = acTrans.GetObject(acBlkTblNewDoc[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

					// Clone the objects to the new database
					IdMapping acIdMap = new IdMapping();

					oCadManager.thisBase.WblockCloneObjects(acObjIdColl, acBlkTblRecNewDoc.ObjectId, acIdMap, DuplicateRecordCloning.Ignore, false);

					// Save the copied objects to the database
					acTrans.Commit();
				}

			}

			oCadManager.documenteManager.MdiActiveDocument = miDocumentoNew;
		}
		
	  

		  
 
 
	}
}
