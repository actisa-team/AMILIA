using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{

	using engNet.ClassT;
	using Microsoft.VisualBasic;
	using System.ComponentModel;
	using tadLayLogica.logica.presupuesto;
	using engNet.Extension.Double;
	using engNet.Extension.Integer;
    using tadLayLan.Tdi;


	public class oRentabilidadAnalisisPrivado : oRentabilidadAnalisis<oRentabilidadRowPrivada>
	{

	   #region "Constructores"
		public oRentabilidadAnalisisPrivado(Guid iIdSolucion)
		   :base(iIdSolucion)
	   {
		  

	   }
		#endregion

		public override void print(string iFileConExtension)
		{

			List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
			List<oValDesT<string, double?>> miLstFooter = new List<oValDesT<string, double?>>();

			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInformeRentPrivada, string.Empty));


			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiRenConexionNueva, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiNombreSol, dsApp.getSolucion(solId).nombre));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiLongKm, dsApp.getSolucion(solId).longitudKm.roundOff(3).ToString()));


			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiDatosTrafico, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiIMD, dsApp.datosTrafico.roadCeroImd.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiTasaCrecimientoAnual, dsApp.datosTrafico.roadCeroCrecimientoAnualPC.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiAbsorcionTraficoIni, dsApp.datosTrafico.roadUnoAbsorcionTraficoInicioPC.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiAbsorcionTraficoFinal, dsApp.datosTrafico.roadUnoAbsorcionTraficoFinalPC.ToString()));

			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiTasas, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiTasasAct, dsApp.indicesDatosTemporales.tasaActualizacionPC.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiTasaActPrecConst, dsApp.indicesDatosTemporales.tasaRevisionPreciosPC.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiTasaIPC, dsApp.indicesDatosTemporales.tasaIpcPC.ToString()));


			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConsReha, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionNuevaConserv, dsApp.costeConservacionMantenimiento.roadUnoConservar.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionNuevaReha, dsApp.costeConservacionMantenimiento.roadUnoRehabilitar.ToString()));

			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiExplotacion, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInvPrivada, dsApp.isInversionPrivada.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiAñosExpl, dsApp.indicesDatosTemporales.numberYearExplotacion.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiGastosExpl, dsApp.gastosExplotacion.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiGastosManoObra, dsApp.gastosExplotacionManoObraPC.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uicostePeaje, dsApp.precioPeaje.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiSubvencionActuIPC, dsApp.isSubvencionEstatalUpdateIpc.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiSobvencionAnual, dsApp.subvencionEstatalAnual.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiSubvencionVehiculo, dsApp.subvencionEstatalVehiculo.ToString()));


			miLstFooter.Add(new oValDesT<string, double?>("", null));
			miLstFooter.Add(new oValDesT<string, double?>("", null));
			miLstFooter.Add(new oValDesT<string, double?>("", null));
			miLstFooter.Add(new oValDesT<string, double?>("", null));
			miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiVAN+": ", van.roundOff(0)));
			miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiRentBC+": ", balanceBeneficiosCostesActualizados.roundOff(3)));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiTIR+": ", tir));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiPRI+": ", pri));




			tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
														 oRentabilidadRowPrivada,
														 oValDesT<string, double?>>(miLstHeader, rentabilidad.Values, miLstFooter, iFileConExtension);

		}

		protected override Dictionary<int, oRentabilidadRowPrivada> createRentabilidad()
		{
			   
			  Dictionary<int, oRentabilidadRowPrivada> miDicRentabilidad = new Dictionary<int, oRentabilidadRowPrivada>();

			  for (int i = 1; i <= yearsConstruccion; i++)
			  {
				  miDicRentabilidad.Add(i, new oRentabilidadRowPrivada(i,
                                                                        strFrmInformes.uiConstruccion,
																		i,
																		tasaActualizacion.lstTasa[i],
																		tasaPreciosConstruccion.lstTasa[i],
																		0,
																		0,
																		0,
																		construccionRentabilidadPrivada.lstCostes[i],
																		0,0,0,0,0,0));

			  }


			  for (int i = 1; i <= yearsExplotacion; i++)
			  {
				  miDicRentabilidad.Add(yearsConstruccion + i, new oRentabilidadRowPrivada(yearsConstruccion + i,
                                                                                           strFrmInformes.uiExplotacion,
																							i,
																							tasaActualizacion.lstTasa[yearsConstruccion + i],
																							0,
																							tasaIpc.lstTasa[i],
																							tasaSubvencion.lstTasa[i],

																							road11Trafico.traficoByYear(i),
																							0,
																							road11ConservacionPrivada.lstCostes[i],
																							road11ExplotacionSegurosPrivada.lstCoste[i].costesExplotacion,
																							road11ExplotacionSegurosPrivada.lstCoste[i].costesSeguros,
																							subvenciones.lstCoste[i].subvencionAnual,
																							subvenciones.lstCoste[i].subvencionPorVehiculos,
																							peajes.lstCoste[i]));



																							
																								 

			  }

			  return miDicRentabilidad;         
		  
			
			}
		
	}

  
}
