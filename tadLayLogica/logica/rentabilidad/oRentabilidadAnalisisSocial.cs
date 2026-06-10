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


	public class oRentabilidadAnalisisSocial: oRentabilidadAnalisis<oRentabilidadRowSocial>
	{

		#region "Constructores"
		public oRentabilidadAnalisisSocial(Guid iIdSolucion)
		   :base(iIdSolucion)
	   {
		  

	   }

		#endregion

		#region "Metodos"
		public override void print (string iFileConExtension)
	  {
		  
		  List<oValDesT<string,string>> miLstHeader = new List<oValDesT<string,string>>();
		  List<oValDesT<string,double?>> miLstFooter = new List<oValDesT<string,double?>>();

		  miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInformeRentSocial, string.Empty));

		  miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionActual, string.Empty));
		  miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiLongKm, dsApp.datosTrafico.roadCeroLongitudKm.roundOff(3).ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVelocidad, dsApp.datosTrafico.roadCeroVelocidadMedia.roundOff(0).ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiculosPesados, dsApp.datosTrafico.roadCeroVehiculosPesadosPC.roundOff(2).ToString()));

		  miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiRenConexionNueva, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiNombreSol, dsApp.getSolucion(solId).nombre));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiLongKm, dsApp.getSolucion(solId).longitudKm.roundOff(3).ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVelocidad, dsApp.getSolucionRoad(solId).velocidadProyecto.roundOff(0).ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiculosPesados, dsApp.datosTrafico.roadUnoVehiculosPesadosPC.roundOff(2).ToString()));

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
		  miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiCostesFuncTiempo, string.Empty));
		  miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uivehiLigeroCoef, dsApp.vehiculoCostes.coePonderacionTiempoVehiculoLigero.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiLigCosteTiempo, dsApp.vehiculoCostes.costeTiempoVehiculoLigero.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiLigeroCosteNeumatico, dsApp.vehiculoCostes.costeNeumaticoVehiculoLigero.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiLigeroCosteAmortizacion, dsApp.vehiculoCostes.costeAmortizacionesVehiculoLigero.ToString()));

          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiPesadoCoef, dsApp.vehiculoCostes.coePonderacionTiempoVehiculoPesado.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiPesadoCosteTiempo, dsApp.vehiculoCostes.costeTiempoVehiculoPesado.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiPesadoCosteNeumatico, dsApp.vehiculoCostes.costeNeumaticoVehiculoPesado.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVehiPesadoCosteAmortizacion, dsApp.vehiculoCostes.costeAmortizacionesVehiculoPesado.ToString()));

 
		  miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiAccidentes, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiNumHeridos, dsApp.costesAccidentes.numHeridosPorAccidente.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionActM, dsApp.costesAccidentes.roadCeroIM.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionActIP, dsApp.costesAccidentes.roadCeroIP.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionNuevaIM, dsApp.costesAccidentes.roadUnoIM.ToString()));
		  miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionNuevaIP, dsApp.costesAccidentes.roadUnoIP.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiCosteMuerto, dsApp.costesAccidentes.costeMuerte.ToString()));
		  miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiCosteHerido, dsApp.costesAccidentes.costeHerido.ToString()));

		  miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConsReha, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionActualConserv, dsApp.costeConservacionMantenimiento.roadCeroConservar.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionAcutalReha, dsApp.costeConservacionMantenimiento.roadCeroRehabilitar.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionNuevaConserv, dsApp.costeConservacionMantenimiento.roadUnoConservar.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiConexionNuevaReha, dsApp.costeConservacionMantenimiento.roadUnoRehabilitar.ToString()));

		  miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiExplotacion, string.Empty));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiAñosExpl, dsApp.indicesDatosTemporales.numberYearExplotacion.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInvPrivada, dsApp.isInversionPrivada.ToString()));



          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uicostePeaje, dsApp.precioPeaje.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiSubvencionActuIPC, dsApp.isSubvencionEstatalUpdateIpc.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiSobvencionAnual, dsApp.subvencionEstatalAnual.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiSubvencionVehiculo, dsApp.subvencionEstatalVehiculo.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiGastosExpl, dsApp.gastosExplotacion.ToString()));
          miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiGastosManoObra, dsApp.gastosExplotacionManoObraPC.ToString()));
		  

		  miLstFooter.Add(new oValDesT<string, double?>("", null));
		  miLstFooter.Add(new oValDesT<string, double?>("", null));
		  miLstFooter.Add(new oValDesT<string, double?>("", null));
		  miLstFooter.Add(new oValDesT<string, double?>("", null));
		  miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiVAN+": ",van.roundOff(0)));
		  miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiRentBC+": ", balanceBeneficiosCostesActualizados.roundOff(3)));
		  miLstFooter.Add(new oValDesT<string, double?>("", null));
		  miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiTIR+": ", tir));
		  miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiPRI+": ", pri));


		  tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>, 
													   oRentabilidadRowSocial,
													   oValDesT<string, double?>>(miLstHeader, rentabilidad.Values, miLstFooter, iFileConExtension);
	  }
		 #endregion


	   protected override Dictionary<int, oRentabilidadRowSocial> createRentabilidad()
	  {
		  

			  Dictionary<int, oRentabilidadRowSocial> miDicRentabilidadSocial = new Dictionary<int, oRentabilidadRowSocial>();

			  for (int i = 1; i <= yearsConstruccion; i++)
			  {
				  miDicRentabilidadSocial.Add(i, new oRentabilidadRowSocial(i,
                                                                             strFrmInformes.uiConstruccion,
																			 i,
																			 tasaActualizacion.lstTasa[i],
																			 tasaPreciosConstruccion.lstTasa[i],
																			 0,
																			 0,
																			 0,
																			 0,
																			 0,
																			 construccionRentabilidadPublica.lstCostes[i],
																			 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));
			  }


			  for (int i = 1; i <= yearsExplotacion; i++)
			  {
				  miDicRentabilidadSocial.Add(yearsConstruccion + i, new oRentabilidadRowSocial(yearsConstruccion + i,
                                                                                                 strFrmInformes.uiExplotacion,
																								 i,
																								 tasaActualizacion.lstTasa[yearsConstruccion + i],
																								 0,
																								 tasaIpc.lstTasa[i],
																								 tasaSubvencion.lstTasa[i],

																								 road0Trafico.traficoByYear(i),
																								 road10Trafico.traficoByYear(i),
																								 road11Trafico.traficoByYear(i),
																								 0,

																								 road0CostesFuncionamientoTiempo.lstCostes[i].costesFuncionamientoPorTrayecto(road0LonKm, road0Trafico.traficoByYear(i)),
																								 road10CostesFuncionamientoTiempo.lstCostes[i].costesFuncionamientoPorTrayecto(road0LonKm, road10Trafico.traficoByYear(i)),
																								 road11CostesFuncionamientoTiempo.lstCostes[i].costesFuncionamientoPorTrayecto(road1LonKm, road11Trafico.traficoByYear(i)),

																								 road0CostesFuncionamientoTiempo.lstCostes[i].costesTiempoPorTrayecto(road0LonKm, road0Trafico.traficoByYear(i)),
																								 road10CostesFuncionamientoTiempo.lstCostes[i].costesTiempoPorTrayecto(road0LonKm, road10Trafico.traficoByYear(i)),
																								 road11CostesFuncionamientoTiempo.lstCostes[i].costesTiempoPorTrayecto(road1LonKm, road11Trafico.traficoByYear(i)),

																								 road0Accidentes.lstAccidentesHistory[i].costeTotalAnual,
																								 road10Accidentes.lstAccidentesHistory[i].costeTotalAnual,
																								 road11Accidentes.lstAccidentesHistory[i].costeTotalAnual,

																								 road0ConservacionEstatal.lstCostes[i],
																								 road10ConservacionEstatal.lstCostes[i],
																								 road11ConservacionEstatal.lstCostes[i],

																								 peajes.lstCoste[i],
																								 subvenciones.lstCoste[i].subvencionPorVehiculos,
																								 subvenciones.lstCoste[i].subvencionAnual,
																								 road11ExplotacionSegurosPrivada.lstCoste[i].costesExplotacionManoObra));

			  }

			  return miDicRentabilidadSocial;         
		  
	  }
	}

}
