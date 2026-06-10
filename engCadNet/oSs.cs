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
	using Autodesk.AutoCAD.EditorInput;
    using tadLayLan;
	
	public static class oSs
	{

        public static List<T> convertSsToList<T>(SelectionSet iSs) where T : DBObject
        {


            List<T> miLstOut = new List<T>();


            if (iSs == null || iSs.Count == 0)
            {
                return miLstOut;

            }
            else
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    DBObject miDbObject = null;

                    T miT = null;

                    foreach (ObjectId item in iSs.GetObjectIds())
                    {
                        miDbObject = tr.GetObject(item, OpenMode.ForRead);

                        miT = (T)miDbObject;

                        miLstOut.Add(miT);
                    }

                }

                return miLstOut;
            }
        }


		/// <summary>
		/// Seleccion por Entidad y Xdata
		/// </summary>
		public static SelectionSet getSsByEntidadAndXdata(eEntidades iEntidad, string iXdata)
		{

				//Creo los SubFiltros.
				TypedValue[] acFilter = new TypedValue[2];
				acFilter.SetValue(new TypedValue((int)DxfCode.Start, iEntidad), 0);
				acFilter.SetValue(new TypedValue((int)DxfCode.ExtendedDataRegAppName, iXdata), 1);

				//Genero el Filtro.
				SelectionFilter acSelFilter = new SelectionFilter(acFilter);

				//Genero la Selección
				PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectAll(acSelFilter);

				//Obtengo el Grupo de Seleccion
				SelectionSet mySs = acSsPr.Value;

				//Cargo el Objeto de Selección
				return mySs;
	
		}	
        
        /// <summary>
        /// Seleccion por Entidad
        /// </summary>
        public static SelectionSet getSsByEntidad(eEntidades iEntidad)
        {

            //Creo los SubFiltros.
            TypedValue[] acFilter = new TypedValue[1];
            acFilter.SetValue(new TypedValue((int)DxfCode.Start, iEntidad), 0);

            //Genero el Filtro.
            SelectionFilter acSelFilter = new SelectionFilter(acFilter);

            //Genero la Selección
            PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectAll(acSelFilter);

            //Obtengo el Grupo de Seleccion
            SelectionSet mySs = acSsPr.Value;

            //Cargo el Objeto de Selección
            return mySs;

        }

		public static SelectionSet getSsByUser(string iMensaje,string iLayer, eEntidades iEntidad, bool iSingleEntidad )
		{

		   //Configuro los Filtros
		   TypedValue[] myTiposFiltros = new TypedValue[2];

		   //FILTRO POR CAPA + ENTIDAD
		   if ((iLayer != null) & (iEntidad != eEntidades.NONE))
		   { 
			myTiposFiltros = new TypedValue[2]
										{
										new TypedValue((int) DxfCode.LayerName,iLayer),
										new TypedValue((int) DxfCode.Start,iEntidad.ToString())
										 };
		   }

		   //FILTRO POR ENTIDAD
		   if ((iLayer == null) & (iEntidad != eEntidades.NONE))
		   {
			   myTiposFiltros = new TypedValue[1]
										{
										new TypedValue((int) DxfCode.Start,iEntidad.ToString())
										 };
		   }

		   //FILTRO POR CAPA
		   if ((iLayer != null) & (iEntidad == eEntidades.NONE))
		   {
			   myTiposFiltros = new TypedValue[1]
										{
										new TypedValue((int) DxfCode.LayerName,iLayer),
										};
		   }


		   
		   if (myTiposFiltros == null)
		   {

			   throw new System.Exception(strError.eFiltroCapaEntidad);

		   }
		   else
		   { 


		   //Creo Seleccion Filtro
		   SelectionFilter mySsFiltros = new SelectionFilter(myTiposFiltros);

		   //Prompt Opciones
		   PromptSelectionOptions myProOpt = new PromptSelectionOptions();
		   myProOpt.SingleOnly = iSingleEntidad;
		   myProOpt.MessageForAdding = iMensaje;

		   //Prompt Resultados
		   PromptSelectionResult myProRes = oCadManager.thisEditor.GetSelection(myProOpt, mySsFiltros);

		   if (myProRes.Status == PromptStatus.Error)
		   {
			   throw new Exception(strError.eGenerarConjuntoSeleccion);
		   }

		   //Seleccion Conjunto
		   SelectionSet mySs = myProRes.Value;

		   //Objeto Seleccion
		   return mySs;

	   }


		
 }

		/// <summary>
		/// Seleccion Entidad Generica
		/// </summary>
		public static T seleccionUsuario <T> (string iMensajeSeleccion,string iMensajeError ) where T : DBObject
		{

			//select an Alignment which we will use to create a Profile
			PromptEntityOptions selalignment = new PromptEntityOptions("\n" + iMensajeSeleccion + "\n");

			selalignment.SetRejectMessage("\n" + iMensajeError);

			selalignment.AddAllowedClass(typeof(T), true);

			selalignment.AllowNone = false;

			PromptEntityResult resalignment = oCadManager.thisEditor.GetEntity(selalignment);


			if (resalignment.Status != PromptStatus.OK)
			{
				return null;
			}

			ObjectId profileId = resalignment.ObjectId;

			return   oCadManager.getEntidadRead<T>(profileId);

		}


        public static void deleteSsByLayerList (List<string> iLstCapas)
        { 
              SelectionSet miSs = getSsByLayerList(iLstCapas);

              oSs.deleteSs(miSs);

              oCadManager.thisEditor.UpdateScreen();
        }
             


		//SELECCION CAPA + ENTIDAD
		public static SelectionSet getSsByLayer(string iLayer)
		{

				//Ojo Debo de Comprobr que la Capa Existe.
				if (oLayer.HasLayer(iLayer) == true)
				{
					//Creo los SubFiltros.
					TypedValue[] acFilter = new TypedValue[1];
					acFilter.SetValue(new TypedValue((int)DxfCode.LayerName, iLayer), 0);

					//Genero el Filtro.
					SelectionFilter acSelFilter = new SelectionFilter(acFilter);

					//Genero la Selección
					PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectAll(acSelFilter);

					//Obtengo el Grupo de Seleccion
					SelectionSet mySs = acSsPr.Value;

					//Cargo el Objeto de Selección
					return mySs;
				}
				else
				{
                    throw new System.Exception(string.Format(strError.eCapaNoExiste, iLayer));
				}

			
		}


		public static List<Polyline> getSsLwByLayerListAndXdata(List<string> iLayerList, string iXdataKey)
		{

            List<Polyline> miLstLw = new List<Polyline>();

            if (iLayerList.Count == 0) { return miLstLw; }
         
           
             SelectionSet miSs = getSsByLayerList(iLayerList);

                if (miSs != null && miSs.Count > 0)
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        foreach (SelectedObject item in miSs)
                        {
                            Entity miEntidad = (Entity)tr.GetObject(item.ObjectId, OpenMode.ForRead);

                            if (miEntidad.ObjectId.ObjectClass.Name.Equals("AcDbPolyline"))
                            {
                                ResultBuffer miBuffer = miEntidad.GetXDataForApplication(iXdataKey);

                                if (miBuffer != null)
                                {
                                    miLstLw.Add(miEntidad as Polyline);
                                }
                            }

                        }

                    }
                }


                return miLstLw;
            }


			

		
		


		public static SelectionSet getSsByLayerList(List<string> iLayerList)
		{


				//Filtro solo las Capas que Existen en el Actual Dwg

            List<string> misCapas = new List<string>();
				foreach (string myLayName in iLayerList)
				{
					if (oLayer.HasLayer(myLayName))
					{
                        misCapas.Add(myLayName);
					}
				}
			
					//Creo los SubFiltros.
                TypedValue[] acFilter = new TypedValue[misCapas.Count + 2];

					 int i  = 1;


					acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "<or"), 0);

                    foreach (string myLayOk in misCapas)
					{
					acFilter.SetValue(new TypedValue((int)DxfCode.LayerName, myLayOk),i);
					i++;
					 
					}

					acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "or>"), i);

				   

					//Genero el Filtro.
					SelectionFilter acSelFilter = new SelectionFilter(acFilter);

					//Genero la Selección
					PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectAll(acSelFilter);

					//Obtengo el Grupo de Seleccion
					SelectionSet mySs = acSsPr.Value;

					//Cargo el Objeto de Selección
					return mySs;
				
		}

	
		public static SelectionSet getSsByLayerAndEntidad(string iLayer, string iEntidad)
		{

					//Creo los SubFiltros.
					TypedValue[] acFilter = new TypedValue[2];
					acFilter.SetValue(new TypedValue((int)DxfCode.Start, iEntidad), 0);
					acFilter.SetValue(new TypedValue((int)DxfCode.LayerName, iLayer), 1);

					//Genero el Filtro.
					SelectionFilter acSelFilter = new SelectionFilter(acFilter);
			  
					//Genero la Selección
					PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectAll(acSelFilter);  

					//Obtengo el Grupo de Seleccion
					SelectionSet miSs = acSsPr.Value;

					//Cargo el Objeto de Selección
					return miSs;
		}


		public static SelectionSet getSsByLayerAndEntidad(string iLayer, eEntidades iEntidad)
		{
			return getSsByLayerAndEntidad(iLayer, iEntidad.ToString());
		}

		public static void deleteSs(SelectionSet iSs)
		{

			if (iSs != null)
			{

					using (Transaction tr = oCadManager.StartTransaction())
					{
						DBObject myEntidad;
		
						foreach (ObjectId myObjId in iSs.GetObjectIds())
						{
							myEntidad = tr.GetObject(myObjId, OpenMode.ForWrite) as DBObject;

							try
							{
								myEntidad.Erase();
							}
							catch (Autodesk.AutoCAD.Runtime.Exception ex)
							{

								if (ex.Message.Contains("eNotApplicable"))
								{

								}
								else
								{
									throw;                            
								}
								
							}
							
							
						}

						tr.Commit();

					}
				}

		}

		public static SelectionSet getSsIntoPolylineByLayerAndEntidad(Polyline iPline, string iLayer, eEntidades iEntidad)
		{
				//Ojo Debo de Comprobr que la Capa Existe.
				if (oLayer.HasLayer(iLayer) == true)
				{
					//Creo los SubFiltros.
					TypedValue[] acFilter = new TypedValue[2];
					acFilter.SetValue(new TypedValue((int)DxfCode.Start, iEntidad.ToString()), 0);
					acFilter.SetValue(new TypedValue((int)DxfCode.LayerName, iLayer), 1);


					Polyline myPline = iPline;

					//Obtengo el listado de vertices
					Point3dCollection myPlineLstVer3d = new Point3dCollection();

					for (int i = 0; i < myPline.NumberOfVertices-1; i++)
					{          
						myPlineLstVer3d.Add(myPline.GetPoint3dAt(i));
					}


					//Genero el Filtro.
					SelectionFilter acSelFilter = new SelectionFilter(acFilter);

					//Genero la Selección
					PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectCrossingPolygon(myPlineLstVer3d, acSelFilter);


				 
					//Obtengo el Grupo de Seleccion
					SelectionSet mySs = acSsPr.Value;


			  
					//Cargo el Objeto de Selección
					return mySs;
				}
				else
				{
                    throw new System.Exception(string.Format(strError.eCapaNoExiste, iLayer));
				}
		}

		public static SelectionSet getSsCrossingPolyline(Polyline iPline,eEntidades iEntidadInside, List<string> iLstCapas, string iXdataKey)
		{

				Polyline myPline = iPline;

				//Obtengo el listado de vertices
				Point3dCollection myPlineLstVer3d = new Point3dCollection();

				for (int i = 0; i < myPline.NumberOfVertices - 1; i++)
				{
					myPlineLstVer3d.Add(myPline.GetPoint3dAt(i));
				}

				//Genero el Filtro.
				SelectionFilter acSelFilter = getFiltroByLayerListAndEntidadAndXdataKey(iEntidadInside, iLstCapas, iXdataKey);

				//Genero la Selección
				PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectCrossingPolygon(myPlineLstVer3d, acSelFilter);

				//Obtengo el Grupo de Seleccion
				SelectionSet mySs = acSsPr.Value;

				//Cargo el Objeto de Selección
				return mySs;
			
		}

		public static SelectionFilter getFiltroByLayerListAndEntidadAndXdataKey(eEntidades iEntidad, List<string> iLayerList, string iXdataKey)
		{

				//Filtro solo las Capas que Existen en el Actual Dwg
				foreach (string myLayName in iLayerList)
				{
					if (!oLayer.HasLayer(myLayName))
					{
						iLayerList.Remove(myLayName);
					}
				}


				//Creo los SubFiltros.
				TypedValue[] acFilter = new TypedValue[(iLayerList.Count*5) + 2];

				int i = 1;


				acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "<or"), 0);

				foreach (string myLayOk in iLayerList)
				{
					
					acFilter.SetValue(new TypedValue((int)DxfCode.Operator,"<and"),i);
					i++;
					acFilter.SetValue(new TypedValue((int)DxfCode.LayerName, myLayOk), i);
					i++;
					acFilter.SetValue(new TypedValue((int)DxfCode.Start, iEntidad.ToString()), i);
					i++;
					acFilter.SetValue(new TypedValue((int)DxfCode.ExtendedDataRegAppName, iXdataKey),i);
					i++;
					acFilter.SetValue(new TypedValue((int)DxfCode.Operator,"and>"),i);
					i++;

				}

				acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "or>"), i);



				//Genero el Filtro.
				SelectionFilter acSelFilter = new SelectionFilter(acFilter);


				return acSelFilter;
			}

        public static ObjectIdCollection getEntidadesOnLayer(string layerName)
        {

            TypedValue[] miCapa = new TypedValue[1] { new TypedValue((int)DxfCode.LayerName, layerName)};

            SelectionFilter miFiltroCapas = new SelectionFilter(miCapa);

            PromptSelectionResult miSeleccionEntidades =  oCadManager.thisEditor.SelectAll(miFiltroCapas);


            if (miSeleccionEntidades.Status == PromptStatus.OK)
            {
                return new ObjectIdCollection(miSeleccionEntidades.Value.GetObjectIds());
            }
            else
            {
                return new ObjectIdCollection();
            }
        }


        /// <summary>
        /// Obtener Listado Polilineas que se Encuentran en un Listado de Capas y Tienen un Xdata
        /// </summary>
        public static List<Polyline> getLstPolilineasFromListadoCapasAndXdataCode(List<string> iLstLayer, string iXdataKey)
        {

            List<Polyline> miLstZonasGis = new List<Polyline>();


            //Filtro Solo lass Capas que Existen
            foreach (string myLayName in iLstLayer)
            {
                if (!oLayer.HasLayer(myLayName))
                {
                    iLstLayer.Remove(myLayName);
                }
            }


            //Creo los SubFiltros.
            TypedValue[] acFilter = new TypedValue[iLstLayer.Count + 2];

            int i = 1;


            acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "<or"), 0);

            foreach (string myLayOk in iLstLayer)
            {
                acFilter.SetValue(new TypedValue((int)DxfCode.LayerName, myLayOk), i);
                i++;

            }

            acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "or>"), i);



            List<Point3d> miLst = new List<Point3d>();

            Point3dCollection miLstP3d = new Point3dCollection();

      
            //Genero el Filtro.
            SelectionFilter acSelFilter = new SelectionFilter(acFilter);

            //Genero la Selección
            PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectAll(acSelFilter); 

            //Obtengo el Grupo de Seleccion
            SelectionSet miSs = acSsPr.Value;

            if (miSs != null)
            {

                //Ahora Debo de Filtrar solo las Polilineas  y las Configuradas con los Xdata
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    foreach (SelectedObject item in miSs)
                    {

                        Entity miEntidad = (Entity)tr.GetObject(item.ObjectId, OpenMode.ForRead);

                        if (miEntidad.ObjectId.ObjectClass.Name.Equals("AcDbPolyline"))
                        {
                            ResultBuffer miBuffer = miEntidad.GetXDataForApplication(iXdataKey);

                            if (miBuffer != null)
                            {
                                miLstZonasGis.Add(miEntidad as Polyline);
                            }
                        }
                    }

                }
            }

            return miLstZonasGis;
        }
		public static SelectionFilter getFiltroByLayerListAndXdataKey(List<string> iLayerList, string iXdataKey)
		{


			//Filtro solo las Capas que Existen en el Actual Dwg
			foreach (string myLayName in iLayerList)
			{
				if (!oLayer.HasLayer(myLayName))
				{
					iLayerList.Remove(myLayName);
				}
			}


			//Creo los SubFiltros.
			TypedValue[] acFilter = new TypedValue[(iLayerList.Count * 4) + 2];

			int i = 1;


			acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "<or"), 0);

			foreach (string myLayOk in iLayerList)
			{

				acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "<and"), i);
				i++;
				acFilter.SetValue(new TypedValue((int)DxfCode.LayerName, myLayOk), i);
				i++;
				acFilter.SetValue(new TypedValue((int)DxfCode.ExtendedDataRegAppName, iXdataKey), i);
				i++;
				acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "and>"), i);
				i++;

			}

			acFilter.SetValue(new TypedValue((int)DxfCode.Operator, "or>"), i);



			//Genero el Filtro.
			SelectionFilter acSelFilter = new SelectionFilter(acFilter);


			return acSelFilter;
		}
		public static ObjectIdCollection getEntidadesByFilter(SelectionFilter iFiltro)
		{
			PromptSelectionResult miSeleccionEntidades = oCadManager.thisEditor.SelectAll(iFiltro);
			if (miSeleccionEntidades.Status == PromptStatus.OK)
			{
				return new ObjectIdCollection(miSeleccionEntidades.Value.GetObjectIds());
			}
			else
			{
				return new ObjectIdCollection();
			}

		}
		
	}
}
