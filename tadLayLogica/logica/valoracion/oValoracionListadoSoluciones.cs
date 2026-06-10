using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{

	using System.ComponentModel;
	using System.Xml.Serialization;
	using System.IO;


	using Autodesk.AutoCAD.ApplicationServices;
	using engCadNet;  
	using tadLayData;
	using engNet.ClassT;



	public class oValoracionListadoSoluciones
	{
		#region "Variables Privadas"
		oCompositeValoracionHipotesis mRootHipotesis = null;
		private oValoracionAreas mValoracionAreas = null;
		private List<Guid> mLstSoluciones = null;
		#endregion
		#region "Constructor"

		public oValoracionListadoSoluciones(dsApp.tbMatrizDecisionRow iHipotesisRow, List<Guid> iLstSoluciones)
		{
			mValoracionAreas = new oValoracionAreas(iHipotesisRow);
			mLstSoluciones = iLstSoluciones;

			mRootHipotesis = new oCompositeValoracionHipotesis(mValoracionAreas.nombre);

			postConstructor();
		}


		#endregion
		#region "Propiedades"

		public oCompositeValoracionHipotesis rootHipotesis
		{
			get
			{
				return mRootHipotesis;
			}
		}

		#endregion
		#region "Metodos Privados"
		private void postConstructor()
		{


			using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
			{

				//Activo las Capas GIS
				oTadil.data.Layer.zonasGisOn();

				oValoracionSolucion miSolucion = null;

				foreach (Guid item in mLstSoluciones)
				{
					miSolucion = new oValoracionSolucion(item, mValoracionAreas);
					mRootHipotesis.add(miSolucion.solucionValoracion);
				}


				//Evalulo Las Partidas que no son Notas 0-10
				setUpTrazadoNotaGlobal<oComponentValTrazadoPlanta>();
				setUpTrazadoNotaGlobal<oComponentValTrazadoAlzado>();
				setUpTrazadoNotaGlobal<oComponentValTrazadoTiempo>();
				setUpTrazadoNotaGlobal<oComponentValTrazadoTierrasVolumenMovimiento>();
				setUpTrazadoNotaGlobal<oComponentValTrazadoTierrasCompensacion>();
				//setUpTrazadoNotaGlobal<oComponetValTunelesRmr>(); ES NOTA 0-10
				setUpTrazadoNotaGlobal<oComponetValEstructurasMuro>();
				setUpTrazadoNotaGlobal<oComponentValEconomicaPresupuestoPcaPublicoSinIva>();
				setUpTrazadoNotaGlobal<oComponentValEconomicaRentabilidadSocial_VAN>();
				setUpTrazadoNotaGlobal<oComponentValEconomicaRentabilidadSocial_BC>();
				setUpTrazadoNotaGlobal<oComponentValEconomicaPresupuestoPcaPrivadoSinIva>();
				setUpTrazadoNotaGlobal<oComponentValEconomicaRentabilidadPrivada_VAN>();
				setUpTrazadoNotaGlobal<oComponentValEconomicaRentabilidadPrivada_BC>();

				//Puntuo las Areas de Valoracion

				//1-TRAZADO
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionTrazadoGrupo>();

				//2-GEOTECNIA
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionGeotecniaGrupo>();

				//3-ESTRUCTURAS
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionEstructurasTunelesMurosGrupo>();

				//4-MEDIOAMBIENTAL
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionAmbientalGrupo>();

				//5-CLIMATICAS
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionClimaticaGrupo>();

				//6-SOCIOECONOMICOS
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionSocioEconomicosGrupo>();

				//7-PATRIMONIALES
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionPatrimonialGrupo>();

				//8-ECONOMICAS
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionEconomicaGrupo>();

				//Puntuo la Solucion
				setUpTrazadoNotaGlobalComposite<oCompositeValoracionSolucion>();

			}

		}
		private void setUpTrazadoNotaGlobal<T>() where T : IValoracion
		{

			List<T> miLstValoracionTrazados;

			miLstValoracionTrazados = mRootHipotesis.getDescendientes().ToList().OfType<T>().ToList();


			double? valorMaximo = null;
			double? valorMinimo = null;

			if (miLstValoracionTrazados.Count > 0)
			{
				valorMaximo = miLstValoracionTrazados.Max(p => p.notaLocal.Value);
				valorMinimo = miLstValoracionTrazados.Min(p => p.notaLocal.Value);
			}
			else
			{
				valorMaximo = 0;
				valorMinimo = 0;
			}

			foreach (var item in miLstValoracionTrazados)
			{
				item.notasMaximasMinima = new oNotaMaximaMinima(valorMaximo.Value, valorMinimo.Value);
			}


			//foreach (var item in miLstValoracionTrazados)
			//{
			//    MessageBox.Show(item.ToString());
			//}

		}
		private void setUpTrazadoNotaGlobalComposite<T>() where T : IValoracion
		{

			List<T> miLstValoracionTrazados;

			miLstValoracionTrazados = mRootHipotesis.getDescendientes().ToList().OfType<T>().Where(p => p.GetType() == typeof(T)).ToList();

			double valorMaximo = miLstValoracionTrazados.Max(p => p.notaLocal.Value);
			double valorMinimo = miLstValoracionTrazados.Min(p => p.notaLocal.Value);


			foreach (var item in miLstValoracionTrazados)
			{
				item.notasMaximasMinima = new oNotaMaximaMinima(valorMaximo, valorMinimo);
			}


			//foreach (var item in miLstValoracionTrazados)
			//{
			//    MessageBox.Show(item.ToString());
			//}

			// menuStrip1.Items.OfType<ToolStripMenuItem>().Where(it => it.GetType() == typeof(ToolStripMenuItem));



		}
		#endregion
   }
}
