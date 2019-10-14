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
	
	
	public class oTools
	{

		public static void sendComand(string iComando)
		{
			oCadManager.thisMdi.SendStringToExecute(iComando, false, false, false);

        
		}
        public static void borrar()
        {
            Application.DocumentManager.MdiActiveDocument.SendStringToExecute("borra todo  ", true, false, true);
            // oCadManager.thisMdi.SendStringToExecute("todo", true, false, false);


        }
        public static void sendComandPline()
		{
			sendComand("_PLINE ");
		}
		public static object getPointCad(string iMensaje)
		{

			PromptPointOptions pprOptions = new PromptPointOptions(iMensaje);
			pprOptions.AllowNone= false;


			PromptPointResult ppr = oCadManager.thisEditor.GetPoint(pprOptions);


			if (ppr.Status == PromptStatus.OK)
			{
				return new  Point3d (ppr.Value.X, ppr.Value.Y, ppr.Value.Z);
			}
			else
			{
				throw new oExSelectPointNull();
			}
		   


		}

        public static Point3d getPointCadPoint(string iMensaje)
        {

            PromptPointOptions pprOptions = new PromptPointOptions(iMensaje);
            pprOptions.AllowNone = false;


            PromptPointResult ppr = oCadManager.thisEditor.GetPoint(pprOptions);


            if (ppr.Status == PromptStatus.OK)
            {
                return new Point3d(ppr.Value.X, ppr.Value.Y, ppr.Value.Z);
            }
            else
            {
                throw new oExSelectPointNull();
            }



        }
		public static tadLayShare.puntos.oP2d getPoint(string iMensaje)
		{

			PromptPointResult ppr = oCadManager.thisEditor.GetPoint(iMensaje);

			if (ppr.Status == PromptStatus.OK)
			{
				Point3d myPto3d = ppr.Value;

                return new tadLayShare.puntos.oP2d(myPto3d.X, myPto3d.Y);
			}

			else
			{
				return null;
			}

		
		}
		public static string getPathDwg()
		{

			string myFolderPath = oCadManager.thisEditor.Document.Name;


			myFolderPath = Path.GetDirectoryName(myFolderPath);


			return myFolderPath;

		}
		public static string getFileNameDwg()
		{
		  return  Path.GetFileNameWithoutExtension(oCadManager.thisEditor.Document.Name);   
		}
		public static void zoomAll()
		{
			Application.DocumentManager.MdiActiveDocument.SendStringToExecute("_zoom _all ", true, false, false);
	  
		}
		public static void navvCubeIsON(bool iIsOn)
		{


			if (iIsOn)
			{
				Application.DocumentManager.MdiActiveDocument.SendStringToExecute("navvcube act ", true, false, false);
			}
			else
			{
				Application.DocumentManager.MdiActiveDocument.SendStringToExecute("navvcube des ", true, false, false);
			}

		}
		public static void regen()
		{

			oCadManager.thisEditor.Regen();
		
		
		}
		public static Point2d getMidPoint(Point2d iP1, Point2d iP2)
		{

		   
			
			double myX;
			double myY;

			myX = (iP2.X + iP1.X)/2;
			myY = (iP2.Y + iP1.Y)/2;


			return new Point2d(myX, myY);

		}
		public static void setVariable(eVariables iVariable, object iValor)
		{
			Application.SetSystemVariable(iVariable.ToString(), iValor);


		}
		public static void entidadDelete(List<string> iLstHandle)
		{

			 List<ObjectId> miLstObjId = new List<ObjectId>();
			
				  
				foreach (string item in iLstHandle)
				{		 
												
				   long ln = Convert.ToInt64(item, 16);
				   // Not create a Handle from the long integer
				   Handle hn = new Handle(ln);
				   ObjectId miObjId = oCadManager.thisBase.GetObjectId(false,hn,0);

					miLstObjId.Add(miObjId);
				}

				 
				 
				 using (Transaction tr = oCadManager.StartTransaction())
				{
				   
					 foreach (ObjectId item in miLstObjId)
					{		                    
					   DBObject obj = tr.GetObject(item, OpenMode.ForWrite);
					   obj.Erase();
					}
		  
					
					tr.Commit();

				}

	   }        



        public static void entidadDelete(List<Entity> iLstToDelete)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {
                foreach (var item in iLstToDelete)
                {
                    Entity myEntidad = tr.GetObject(item.ObjectId, OpenMode.ForWrite) as Entity;

                    myEntidad.Erase();
                }
                
                tr.Commit();
            }

   


        }

		public static void entidadDelete(DBObject iObjId)
		{
			entidadDelete(iObjId.ObjectId);    
		}
		public static void entidadDelete(ObjectId iObjId)
		{

				using (Transaction tr = oCadManager.StartTransaction())
				{
					Entity myEntidad = tr.GetObject(iObjId, OpenMode.ForWrite) as Entity;

					myEntidad.Erase();

					tr.Commit();                          
				}

		}
		public static Entity entidadGet(ObjectId iObjId)
		{

				using (Transaction tr = oCadManager.StartTransaction())
				{

					Entity myEntidad = tr.GetObject(iObjId, OpenMode.ForRead) as Entity;

					tr.Commit();

					return myEntidad;
				}

		}
		public static DBObject entidadGet(string iHandle)
		{
			using (Transaction tr = oCadManager.StartTransaction())
			{     
					long miLong = Convert.ToInt64(iHandle, 16);
					
					Handle miHandle = new Handle(miLong);
				   
					ObjectId miId = oCadManager.thisBase.GetObjectId(false, miHandle, 0);

					DBObject miObj = tr.GetObject(miId, OpenMode.ForRead);

					return  miObj;  
				}
		}
        public static void entidadHighLight (string iHandle)
        {
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                oCadManager.thisEditor.Regen();
                
                using (oEntidad<Entity> miEntidad = new oEntidad<Entity>(iHandle))
                {
                    miEntidad.open();

                    miEntidad.entidad.Highlight();

                    miEntidad.save();
                }

                oCadManager.thisEditor.UpdateScreen();

               
            }
        }




        public static void entidadesRotate_EjeX_90(List<Entity> iLstEntidades, Point3d iPtoOrigen)
        {


            Matrix3d miMatrizTransformacion = new Matrix3d();


           Matrix3d miMatrix3DWorld =oCadManager.thisEditor.CurrentUserCoordinateSystem;

          Vector3d  miVectorRotacion = miMatrix3DWorld.CoordinateSystem3d.Xaxis;

          miMatrizTransformacion = Matrix3d.Rotation(0.5 * System.Math.PI,miVectorRotacion , iPtoOrigen);
        
        
             foreach (Entity item in iLstEntidades)
                {

                    oEntidad<Entity> miEntidad = new oEntidad<Entity>(item);

                    miEntidad.open();

                    miEntidad.entidad.TransformBy(miMatrizTransformacion);

                    miEntidad.save();
           
                }
   
        
      
        }

        public static void entidadesRotate_EjeZ(List<Entity> iLstEntidades, Point3d iPtoOrigen, double iRotateAngle)
        {


            Matrix3d miMatrizTransformacion = new Matrix3d();


            Matrix3d miMatrix3DWorld = oCadManager.thisEditor.CurrentUserCoordinateSystem;

            Vector3d miVectorRotacion = miMatrix3DWorld.CoordinateSystem3d.Zaxis;

            miMatrizTransformacion = Matrix3d.Rotation(iRotateAngle, miVectorRotacion, iPtoOrigen);

            foreach (Entity item in iLstEntidades)
            {

                oEntidad<Entity> miEntidad = new oEntidad<Entity>(item);

                miEntidad.open();

                miEntidad.entidad.TransformBy(miMatrizTransformacion);

                miEntidad.save();
            }

        }

        public static void entidadesMoveTo(List<Entity> iLstEntidades, Point3d iPtoOrigen, Point3d iPtoDestino)
        {
   
            Vector3d miVectorDesplazamiento = iPtoOrigen.GetVectorTo(iPtoDestino);
            Matrix3d miMatrizTransformacion = Matrix3d.Displacement(miVectorDesplazamiento);

         
                foreach (Entity item in iLstEntidades)
                {

                    oEntidad<Entity> miEntidad = new oEntidad<Entity>(item);

                    miEntidad.open();

                    miEntidad.entidad.TransformBy(miMatrizTransformacion);

                    miEntidad.save();
           
                }
   
        }


	    public static void entidadesAdd(List<Entity> iColeccionEntidades)
		{

				using (Transaction tr = oCadManager.StartTransaction())
				{

					BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Necesito Crear un Nuevo Registro para Añadir la Linea
					BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					//configuro los Datos por Defecto


					foreach (Entity  item in iColeccionEntidades)
					{

						item.SetDatabaseDefaults();

						acBlockTableRec.AppendEntity(item);

						tr.AddNewlyCreatedDBObject(item, true);
					   
					}
	 

					tr.Commit();

				}
		}


		public static ObjectId entidadAdd(Entity iEntidad, string iLayer)
		{
		
				using (Transaction tr = oCadManager.StartTransaction())
				{

					BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

					//Necesito Crear un Nuevo Registro para Añadir la Linea
					BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

							   
					//configuro los Datos por Defecto

					iEntidad.SetDatabaseDefaults();

					iEntidad.Layer = iLayer;

					ObjectId myOut =  acBlockTableRec.AppendEntity(iEntidad);

					tr.AddNewlyCreatedDBObject(iEntidad, true);

					tr.Commit();

					return myOut;

				}
				 
		}

       


        public static T clonar <T>(RXObject iLwToClonar)
        {


            using (Transaction tr = oCadManager.StartTransaction())
            {
                object miClone = iLwToClonar.Clone();

                tr.Commit();

                return (T) miClone ;
            }

        }

		public static DBObject entidadClonar(RXObject iObjectClonar)
		{

			using (Transaction tr = oCadManager.StartTransaction())
			{
			 object miClone= iObjectClonar.Clone();

			 tr.Commit();

			 return miClone as DBObject;      
			}

		
		}
	}
}
