using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{
   
	using Autodesk.AutoCAD.Runtime;
	using Autodesk.AutoCAD.ApplicationServices;
	using Autodesk.AutoCAD.Geometry;
	using Autodesk.AutoCAD.DatabaseServices;


	using engCadNet;



	public class oEntidad <T>:IDisposable
		where T : DBObject
   {

  

		private T mEntidad = null;
		private Transaction mTr = null;

		#region "Constructor"


		/// <summary>
		/// Entidad Abierta Solo Lectura
		/// </summary>
		public oEntidad (string iHandle)
		{
		   
				mTr = oCadManager.StartTransaction();
				long miLong = Convert.ToInt64(iHandle, 16);
				Handle miHandle = new Handle(miLong);


				try
				{
					ObjectId miObjId = oCadManager.thisBase.GetObjectId(false, miHandle, 0);
					mEntidad = (T)mTr.GetObject(miObjId, OpenMode.ForRead);
				}
				catch (System.Exception )
				{
					mTr.Dispose();

					throw new oExEntidadNoExiste(iHandle);
				}
				

	
		}

		/// <summary>
		/// Entidad Abierta Solo Lectura
		/// </summary>
		public oEntidad(DBObject iObject)
			:this(iObject.ObjectId)
		{

			
		}

		/// <summary>
		/// Entidad Abierta Solo Lectura
		/// </summary>
		/// <param name="iObjId"></param>
		public oEntidad(ObjectId iObjId)
		{
				mTr = oCadManager.StartTransaction();
				mEntidad = (T)mTr.GetObject(iObjId, OpenMode.ForRead);
		}

		#endregion


		public void open()
		{
			   mEntidad.UpgradeOpen();
		}
		public void save()
		{
			mTr.Commit();
		}


		public T entidad
		{
			get
			{
			  return mEntidad;
			}
		
		}

		public void Dispose()
		{
			mTr.Dispose();
			mEntidad = null;
		}
   }

}
