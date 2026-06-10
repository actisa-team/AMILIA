using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.medicion
{
	using Autodesk.AutoCAD.DatabaseServices;
	using Autodesk.AutoCAD.EditorInput;

	using tadLayLogica.zonaGis;

    using tadLayLogica.datos;

	using tadLayLogica.datos.proyecto;

	using engCadNet;
	
	public class oMedicionExpropiacion : IDisposable
	{

		private Guid mIdSol = Guid.Empty;

		private oTadilLayer mLayerExpropiacion = null;

		private Polyline mLwExpropiacion = null;

		private List<oZonaExProduccion> mLstZonasProduccion;

		private List<oZonaExSuelo> mLstZonasSuelo;


		#region "Constructor"
		public oMedicionExpropiacion(Guid iIdSol)
		{
			mIdSol = iIdSol;
		}
		#endregion



		public void createMediciones ()
		{
            // Inicializar las listas siempre, para que Dispose() no falle
            mLstZonasProduccion = new List<oZonaExProduccion>();
            mLstZonasSuelo = new List<oZonaExSuelo>();

            //Creo la Capa donde guardar las Areas de Expropiación por Solución
            mLayerExpropiacion = new oTadilLayerExpropiacion(mIdSol);

            //Obtengo el handle de la expropiación
            var miSolucionRow = oSingletonDsApp.getInstance.getSolucion(mIdSol);
            if (miSolucionRow == null || miSolucionRow.IshandleExpropiacionNull() || string.IsNullOrEmpty(miSolucionRow.handleExpropiacion))
            {
                // No hay expropiación dibujada para esta solución (p.ej. importación LandXML)
                return;
            }

           string miHandleExpro = miSolucionRow.handleExpropiacion;


            mLwExpropiacion = oCadManager.getEntidadRead<Polyline>(miHandleExpro);



            List<string> miLstCapaSocioEconomicas = oTadil.data.Layer.getLstCapasExpropiacionSocioeconomicos;
            List<string> miLstCapaPatrimoniales = oTadil.data.Layer.getLstCapasExpropiacionPatrimoniales;


            //Polilineas Zonas Gis que cortan 
            List<Polyline> miLstLwSocioEconomicas = this.getPolilineasCortanExpropiacion(mLwExpropiacion, eEntidades.LWPOLYLINE, miLstCapaSocioEconomicas, eXdataKey.zonaGisGuid.ToString());

            //Creo el listado de Polilineas Suelo
            List<Polyline> miLstLwPatrimoniales = this.getPolilineasCortanExpropiacion(mLwExpropiacion, eEntidades.LWPOLYLINE, miLstCapaPatrimoniales, eXdataKey.zonaGisGuid.ToString());

            //Creo el Listado de Zonas GIS PRODUCCION
            mLstZonasProduccion = getZonasProduccionBySs(miLstLwSocioEconomicas);
            mLstZonasSuelo = getZonasSueloBySs(miLstLwPatrimoniales);

				
			 //Creo la Capa donde Guardar la Expropiacion
			mLayerExpropiacion = new oTadilLayerExpropiacion(mIdSol);


			//Recorro las Zonas
			double? miArea=null;


			List<oMedItemModel> miLstMedExpropiacionesProduccion = new List<oMedItemModel>();
			List<oMedItemModel> miLstMedExpropiacionesSuelo = new List<oMedItemModel>();

			oMedItemModel miMedicion;


			foreach (oZonaExProduccion item in mLstZonasProduccion)
			{
			   miArea= getExpropiacion(mLwExpropiacion,item.lwZona);

               if (miArea != null)
               {
                   miMedicion = oFactoryPartidas.createMedicionesExpropiacionFromZonasGis(item.row.idMatValorProduccionSuelo, miArea.Value, item.code); //ZONAGIS
                   miLstMedExpropiacionesProduccion.Add(miMedicion);
               }

			}

			foreach (oZonaExSuelo item in mLstZonasSuelo)
			{
				miArea = getExpropiacion(mLwExpropiacion, item.lwZona);

                if (miArea != null)
                {
                    miMedicion = oFactoryPartidas.createMedicionesExpropiacionFromZonasGis(item.row.idMatValorExpropiacionSuelo, miArea.Value, item.code); //ZONAGIS
                    miLstMedExpropiacionesSuelo.Add(miMedicion);
                }
			}



			//AGRUPO LAS MEDICIONES
			List<oMedItemModel> miLstMedExpropiacionesGroup = new List<oMedItemModel>();


			//AGRUPO LA PRODUCCION
			if (miLstMedExpropiacionesProduccion.Count > 0)
			{
				var miQueryProduccionGroup = from p in miLstMedExpropiacionesProduccion
											 group p by new {p.row.idMaterial } into g
											 select new { matId = g.First().row.idMaterial, m2Sum = g.Sum(k => k.medicion) };
				
				miLstMedExpropiacionesGroup.AddRange(miQueryProduccionGroup.ToList().ConvertAll(p => oFactoryPartidas.createMedicionesExpropiacion(p.matId, p.m2Sum, eNodo.VALPRO)));
			}

		   //AGRUPO EL SUELO
			if (miLstMedExpropiacionesSuelo.Count > 0)
			{
				var miQuerySueloGroup = from p in miLstMedExpropiacionesSuelo
										group p by new { p.row.idMaterial } into g
										select new { matId = g.First().row.idMaterial, m2Sum = g.Sum(k => k.medicion) };

				miLstMedExpropiacionesGroup.AddRange(miQuerySueloGroup.ToList().ConvertAll(p => oFactoryPartidas.createMedicionesExpropiacion(p.matId, p.m2Sum, eNodo.VALSUE)));

			}


		  //Guardo las Mediciones en la Tabla
		  oDalAppExpropiacion.addMedicionesExpropiacion(mIdSol, miLstMedExpropiacionesGroup);
		}




		#region "Metodos Privados"


        private List<Polyline> getPolilineasCortanExpropiacion(Polyline iLwExpropiacion, eEntidades iEntidadInside, List<string> iLstCapas, string iXdataKey)
        {
            List<Polyline> miLstLw = new List<Polyline>();

            
            //Genero el Filtro.
            SelectionFilter acSelFilter = engCadNet.oSs.getFiltroByLayerListAndEntidadAndXdataKey(iEntidadInside, iLstCapas, iXdataKey);

            //Genero la Selección
            PromptSelectionResult acSsPr = oCadManager.thisEditor.SelectAll(acSelFilter);

            //Obtengo el Grupo de Seleccion
            SelectionSet mySs = acSsPr.Value;

            Polyline miLwZonaGis;


            if (mySs == null)
            {
                return miLstLw;
            }


            using (Transaction tr = oCadManager.StartTransaction())
            {

                foreach (SelectedObject item in mySs)
                {
                    miLwZonaGis = (Polyline)tr.GetObject(item.ObjectId, OpenMode.ForRead);

                    if (oLw.hasIntersection(miLwZonaGis, iLwExpropiacion))
                    {
                        miLstLw.Add(miLwZonaGis);
                    }

                }

            }

            //Cargo el Objeto de Selección
            return miLstLw;
        }



		private double? getExpropiacion(Polyline iLwPerimetroObraLineal, Polyline iLwZonaGis)
		{

			using (Transaction tr = oCadManager.StartTransaction())
			{

				Region miRegionZona = oRegion.addRegionFromLw(iLwZonaGis, iLwZonaGis.Layer);
				Region miRegionObraLineal = oRegion.addRegionFromLw(iLwPerimetroObraLineal, iLwZonaGis.Layer);

				try
				{
					//Abro la Base de Datos
					BlockTable acBlkTbl = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

					// Open the Block table record Model space for write
					BlockTableRecord acBlkTblRec = tr.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

					Region miRegionZonaCopy = miRegionZona.Clone() as Region;

					miRegionZonaCopy.Layer = mLayerExpropiacion.name;

					miRegionZonaCopy.BooleanOperation(BooleanOperationType.BoolIntersect, miRegionObraLineal);

                    if (miRegionZonaCopy.Area > 0)
                    {
                        acBlkTblRec.AppendEntity(miRegionZonaCopy);
                        tr.AddNewlyCreatedDBObject(miRegionZonaCopy, true);
                        return miRegionZonaCopy.Area;
                    }
                    else
                    {

                        return null;

                    }




					

				}
				catch (Exception)
				{
					throw;
				}

				finally
				{
					//Borro las Regiones
					tr.GetObject(miRegionZona.ObjectId, OpenMode.ForWrite).Erase();
                    tr.GetObject(miRegionObraLineal.ObjectId, OpenMode.ForWrite).Erase();
					tr.Commit();
				}


			}




		}


		private List<oZonaExProduccion> getZonasProduccionBySs(List<Polyline> iSsZonas)
		{

			List<oZonaExProduccion> miLstZonaGis = new List<oZonaExProduccion>();
			Polyline miZonaLw;
			oZonaExProduccion miZonaGis;

		   

			if (iSsZonas != null)
			{
                foreach (Polyline item in iSsZonas)
				{
					miZonaLw = oCadManager.getEntidadRead<Polyline>(item.ObjectId);
					miZonaGis = (oZonaExProduccion) oFactoryZonaGis.createZonaGisFromPolilinea(miZonaLw);
					miLstZonaGis.Add(miZonaGis);
				}
			}


			return miLstZonaGis;
		}
        private List<oZonaExSuelo> getZonasSueloBySs(List<Polyline> iSsZonas)
		{

			List<oZonaExSuelo> miLstZonaGis = new List<oZonaExSuelo>();
			Polyline miZonaLw;
			oZonaExSuelo miZonaGis;



			if (iSsZonas != null)
			{
                foreach (Polyline item in iSsZonas)
				{
					miZonaLw = oCadManager.getEntidadRead<Polyline>(item.ObjectId);
					miZonaGis = (oZonaExSuelo)oFactoryZonaGis.createZonaGisFromPolilinea(miZonaLw);
					miLstZonaGis.Add(miZonaGis);
				}
			}


			return miLstZonaGis;
		}
		#endregion   
	   
        public void Dispose()
        {
            mLstZonasProduccion?.Clear();
            mLstZonasSuelo?.Clear();
        }
    }

}
