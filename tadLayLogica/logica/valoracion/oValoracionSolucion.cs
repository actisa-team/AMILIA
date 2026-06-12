using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{



	using engCadNet.entidades;

  
	using tadLayData;

	using tadLayLogica.datos;
	using tadLayLogica.datos.proyecto;
	using tadLayLogica.zonaGis;
	using tadLayLogica.logica;
	using tadLayLogica.logica.presupuesto;
	using tadLayLogica.logica.rentabilidad;

    using tadLayLan;
    using tadLayLan.Tdb;

	using engNet.ClassT;
    using tadLayLan.Tdi;



	public class oValoracionSolucion : oSolucion
	{

		oValoracionAreas mValoracionAreas = null;

		IValoracion mValoracionSolucion = null;
		IValoracion mValoracionTrazado = null;
		IValoracion mValoracionGeotecnia = null;
		IValoracion mValoracionEstructuraTunelMuro = null;
		IValoracion mValoracionMedioAmbiental = null;
		IValoracion mValoracionClimatica = null;
		IValoracion mValoracionSocioEconomico = null;
		IValoracion mValoracionPatrimonial = null;
		IValoracion mValoracionEconomica = null;


	   public oValoracionSolucion(Guid iIdSolucion, oValoracionAreas iValoracionAreas) 
		   :base(iIdSolucion)
	   {

		   mValoracionAreas = iValoracionAreas;

		   createValoraciones();
	   }




	  public IValoracion solucionValoracion
	   {
			get
		   {
			   if (mValoracionSolucion == null)
			   {
				   throw new Exception("El objeto IValoracion Solucion es Nulo");
			   }
			   else
			   {
				   return mValoracionSolucion;
			   }
		   }
	   }



		private void createValoraciones()
	   {

           //Cargo las Secciones de Valoracion
           base.getLstSeccionesValoracion();
            
            
           //Solucion
		   mValoracionSolucion = new oCompositeValoracionSolucion(base.idSolucion,base.solucionData.nombre);
			
		   //TRAZADO
		   mValoracionTrazado = getValoracionTrazado();
		   mValoracionSolucion.add(mValoracionTrazado);

		   //MOVIMIENTO TIERRAS
		   mValoracionGeotecnia = getValoracionGeotecnia();
		   mValoracionSolucion.add(mValoracionGeotecnia);

		   //PUENTES-TUNELES-MUROS
		   mValoracionEstructuraTunelMuro = getValoracionPuentesTunelesMuros();
		   mValoracionSolucion.add(mValoracionEstructuraTunelMuro);

		   //MEDIOAMBIENTALES
		   mValoracionMedioAmbiental = getValoracionMedioAmbiental();
		   mValoracionSolucion.add(mValoracionMedioAmbiental);

		   //CLIMATICAS
		   mValoracionClimatica = getValoracionClimatica();
		   mValoracionSolucion.add(mValoracionClimatica);

		   //SOCIOECONOMICAS
		   mValoracionSocioEconomico = getValoracionSocioEconomica();
		   mValoracionSolucion.add(mValoracionSocioEconomico);

		   //PATRIMONIALES
		   mValoracionPatrimonial = getValoracionPatrimonial();
		   mValoracionSolucion.add(mValoracionPatrimonial);

		   //ECONOMICAS
		   mValoracionEconomica = getValoracionEconomica(oSingletonDsApp.getInstance.isInversionPrivada);
		   mValoracionSolucion.add(mValoracionEconomica);

	   }



		//TRAZADO
		private IValoracion getValoracionTrazado()
		{

			IValoracion miValoracionTrazado = new oCompositeValoracionTrazadoGrupo(mValoracionAreas.trazadoPC);

			oValoracionTrazadoPlanta miValoracionPlanta = new oValoracionTrazadoPlanta(base.ejeTrazado, oSingletonDsApp.getInstance.valoracionTrazado.trazadoPlantaPC,this);
			miValoracionTrazado.add(miValoracionPlanta.valoracion);

			oValoracionTrazadoAlzado miValoracionAlzado = new oValoracionTrazadoAlzado(base.ejePerfilRasante, oSingletonDsApp.getInstance.valoracionTrazado.trazadoAlzadoPC, this);
			miValoracionTrazado.add(miValoracionAlzado.valoracion);

			oValoracionTrazadoTiempo miValoracionTiempo = new oValoracionTrazadoTiempo(base.roadDesign.grupo, base.ejeTrazado, oTadil.data.Files.fileNormasCarreteras, oSingletonDsApp.getInstance.valoracionTrazado.tiempoRecorridoPC,this);
			miValoracionTrazado.add(miValoracionTiempo.valoracion);

			oValoracionTierrasVolumenMovimiento miValoracionTierrasVolumenMovimiento = new oValoracionTierrasVolumenMovimiento(base.solucionData, oSingletonDsApp.getInstance.valoracionTrazado.volumenMovTierrasPC);
			miValoracionTrazado.add(miValoracionTierrasVolumenMovimiento.valoracion);

			oValoracionTierrasCompensacion miValoracionTierrasCompensacion = new oValoracionTierrasCompensacion(base.solucionData, oSingletonDsApp.getInstance.valoracionTrazado.compensacionTierrasPC);
			miValoracionTrazado.add(miValoracionTierrasCompensacion.valoracion);

			return miValoracionTrazado;
		}

		//GEOTECNIA
		private IValoracion getValoracionGeotecnia()
		{

			IValoracion miValoracion = new oCompositeValoracionGeotecniaGrupo(mValoracionAreas.geotecniaPC);

			List<IValoracion> miValoracionZonasMovimientoTierras = getValoracionZonasDesign(eGisZonas.MOVTIE, base.numeroSeccionesTotales.Value);

			miValoracion.add(miValoracionZonasMovimientoTierras);

			return miValoracion;
		   
		}


		// OJO NO  ESTA IMPLEMENTADO LA VALORACION DE ESTRUCTURAS-TUNELES
		private IValoracion getValoracionPuentesTunelesMuros()
		{

			IValoracion miValoracionGrupo = new oCompositeValoracionEstructurasTunelesMurosGrupo(mValoracionAreas.estructuraTunelMuroPC);
			IValoracion miValoracionPuentesCapitulo = null;
			IValoracion miValoracionTunelesCapitulo = null;
			IValoracion miValoracionMurosCapitulo = null;


		   
			//VALORACION PUENTES-CIMENTACION
			 miValoracionPuentesCapitulo = new oCompositeValoracionPuentesCimentacion(oSingletonDsApp.getInstance.valoracionEstructuraTunelMuro.gloEstructurasGeotecniaPC);
			 List<IValoracion> miLstValoracionesPuentesCimentacion = getValoracionZonasDesign(eGisZonas.ESTCIM, base.numeroSeccionesTotales.Value);
			 miValoracionPuentesCapitulo.add(miLstValoracionesPuentesCimentacion);
			
			//VALORACION TUNELES
			 miValoracionTunelesCapitulo = new oCompositeValoracionTuneles(oSingletonDsApp.getInstance.valoracionEstructuraTunelMuro.gloTunelesGeotecniaPC);
			 List<IValoracion> miLstValoracionesTuneles = getValoracionZonasDesign(eGisZonas.TUNTUN, base.numeroSeccionesTotales.Value);
             miValoracionTunelesCapitulo.add(miLstValoracionesTuneles);

			//VALORACION MURO 
			 miValoracionMurosCapitulo = new oCompositeValoracionMuros(oSingletonDsApp.getInstance.valoracionEstructuraTunelMuro.gloMurosPC); 
			 oValoracionEstructuraMuro miValoracionMuro =  new oValoracionEstructuraMuro(base.solucionData);
			 miValoracionMurosCapitulo.add(miValoracionMuro.valoracion);

			//Añado las Valoraciones al Grupo Estructuras-Puentes-Muros
			 miValoracionGrupo.add(miValoracionPuentesCapitulo);
			 miValoracionGrupo.add(miValoracionTunelesCapitulo);
			 miValoracionGrupo.add(miValoracionMurosCapitulo);


			 return miValoracionGrupo;
		}

		//MEDIO AMBIENTAL
		private IValoracion getValoracionMedioAmbiental()
		{

		   IValoracion  miValoracion = new oCompositeValoracionAmbientalGrupo(mValoracionAreas.medioAmbientalPC);
			
			
			IValoracion miValoracionProteccion = new oCompositeZonaClasificacion("04.01-" + strFrmGisGeneral.uiDEZOPR, oSingletonDsApp.getInstance.valoracionMedioAmbiental.zonasProteccionPC);
			miValoracionProteccion.add(getValoracion(eGisZonas.DEZOPR));
			miValoracion.add(miValoracionProteccion);

			IValoracion miValoracionSuelo = new oCompositeZonaClasificacion("04.02-" + strFrmGisGeneral.uiVALSUE, oSingletonDsApp.getInstance.valoracionMedioAmbiental.suelosPC);
			miValoracionSuelo.add(getValoracion(eGisZonas.VALSUE));
			miValoracion.add(miValoracionSuelo);

			IValoracion miValoracionFauna = new oCompositeZonaClasificacion("04.03-" + strFrmGisGeneral.uiVALFAU, oSingletonDsApp.getInstance.valoracionMedioAmbiental.faunaPC);
			miValoracionFauna.add(getValoracion(eGisZonas.VALFAU));
			miValoracion.add(miValoracionFauna);

			IValoracion miValoracionFlora = new oCompositeZonaClasificacion("04.04-" + strFrmGisGeneral.uiVALFLO, oSingletonDsApp.getInstance.valoracionMedioAmbiental.floraPC);
			miValoracionFlora.add(getValoracion(eGisZonas.VALFLO));
			miValoracion.add(miValoracionFlora);

			IValoracion miValoracionDominioHidraulico = new oCompositeZonaClasificacion("04.05-" + strFrmGisGeneral.uiZODOPU, oSingletonDsApp.getInstance.valoracionMedioAmbiental.zonasDominioPublicoHiraulicoPC);
			miValoracionDominioHidraulico.add(getValoracion(eGisZonas.ZODOPU));
			miValoracion.add(miValoracionDominioHidraulico);

			IValoracion miValoracionAcuiferos = new oCompositeZonaClasificacion("04.06-" + strFrmGisGeneral.uiACUIFE, oSingletonDsApp.getInstance.valoracionMedioAmbiental.acuiferosPC);
			miValoracionAcuiferos.add(getValoracion(eGisZonas.ACUIFE));
			miValoracion.add(miValoracionAcuiferos);

			IValoracion miValoracionInteresPaisaje = new oCompositeZonaClasificacion("04.07-" + strFrmGisGeneral.uiZOINPA, oSingletonDsApp.getInstance.valoracionMedioAmbiental.interesPaisajisticoPC);
			miValoracionInteresPaisaje.add(getValoracion(eGisZonas.ZOINPA));
			miValoracion.add(miValoracionInteresPaisaje);

			IValoracion miValoracionCamposVisuales = new oCompositeZonaClasificacion("04.08-" + strFrmGisGeneral.uiCAVIIN, oSingletonDsApp.getInstance.valoracionMedioAmbiental.camposVisualesPC);
			miValoracionCamposVisuales.add(getValoracion(eGisZonas.CAVIIN));
			miValoracion.add(miValoracionCamposVisuales);

			IValoracion miValoracionPermeabilidadFauna = new oCompositeZonaClasificacion("04.09-" + strFrmGisGeneral.uiPERFAU, oSingletonDsApp.getInstance.valoracionMedioAmbiental.permeabilidadFaunaPC);
			miValoracionPermeabilidadFauna.add(getValoracion(eGisZonas.PERFAU));
			miValoracion.add(miValoracionPermeabilidadFauna);

			return miValoracion;

		}

		//CLIMATICA
		private IValoracion getValoracionClimatica()
		{

			IValoracion miValoracion = new oCompositeValoracionClimaticaGrupo(mValoracionAreas.climaticaPC);


			IValoracion miValoracionFuertesHeladas = new oCompositeZonaClasificacion("05.01-"+ strFrmGisGeneral.uiZOFUHE , oSingletonDsApp.getInstance.valoracionClimatica.fuertesHeladasPC);
			miValoracionFuertesHeladas.add(getValoracion(eGisZonas.ZOFUHE));
			miValoracion.add(miValoracionFuertesHeladas);

			IValoracion miValoracionUmbria = new oCompositeZonaClasificacion("05.02-" +strFrmGisGeneral.uiZONUMB, oSingletonDsApp.getInstance.valoracionClimatica.umbriaPC);
			miValoracionUmbria.add(getValoracion(eGisZonas.ZONUMB));
			miValoracion.add(miValoracionUmbria);

			IValoracion miValoracionTormentas = new oCompositeZonaClasificacion("05.03-" + strFrmGisGeneral.uiZONTOR, oSingletonDsApp.getInstance.valoracionClimatica.tormentasFrecuentesPC);
			miValoracionTormentas.add(getValoracion(eGisZonas.ZONTOR));
			miValoracion.add(miValoracionTormentas);

			IValoracion miValoracionLluvias = new oCompositeZonaClasificacion("05.04-" + strFrmGisGeneral.uiZOLLIN, oSingletonDsApp.getInstance.valoracionClimatica.lluviasIntensasPC);
			miValoracionLluvias.add(getValoracion(eGisZonas.ZOLLIN));
			miValoracion.add(miValoracionLluvias);

			IValoracion miValoracionNevadas = new oCompositeZonaClasificacion("05.05-" + strFrmGisGeneral.uiZONNEV, oSingletonDsApp.getInstance.valoracionClimatica.nevadaPC);
			miValoracionNevadas.add(getValoracion(eGisZonas.ZONNEV));
			miValoracion.add(miValoracionNevadas);

			IValoracion miValoracionFuertesVientos = new oCompositeZonaClasificacion("05.06-" + strFrmGisGeneral.uiZOFUVI, oSingletonDsApp.getInstance.valoracionClimatica.fuertesVientosPC);
			miValoracionFuertesVientos.add(getValoracion(eGisZonas.ZOFUVI));
			miValoracion.add(miValoracionFuertesVientos);


			IValoracion miValoracionNieblasDensas = new oCompositeZonaClasificacion("05.07-" + strFrmGisGeneral.uiZONIDE, oSingletonDsApp.getInstance.valoracionClimatica.nieblasDensasPC);
			miValoracionNieblasDensas.add(getValoracion(eGisZonas.ZONIDE));     
			miValoracion.add(miValoracionNieblasDensas);


			return miValoracion;

		}

		//SOCIO-ECONOMICAS
		private IValoracion getValoracionSocioEconomica()
		{

			IValoracion miValoracion = new oCompositeValoracionSocioEconomicosGrupo(mValoracionAreas.socioEconomicasPC);

			IValoracion miValoracionSectorPrimario = new oCompositeZonaClasificacion("06.01-" + strFrmGisGeneral.uiSECPRI, oSingletonDsApp.getInstance.valoracionSocioEconomicas.sectorPrimarioPC);
			miValoracionSectorPrimario.add(getValoracion(eGisZonas.SECPRI));
			miValoracion.add(miValoracionSectorPrimario);

			IValoracion miValoracionSectorSecundario = new oCompositeZonaClasificacion("06.02-" + strFrmGisGeneral.uiSECSEC, oSingletonDsApp.getInstance.valoracionSocioEconomicas.sectorSecundarioPC);
			miValoracionSectorPrimario.add(getValoracion(eGisZonas.SECSEC));
			miValoracion.add(miValoracionSectorSecundario);

			IValoracion miValoracionSectorTerciario = new oCompositeZonaClasificacion("06.03-" + strFrmGisGeneral.uiSECTER, oSingletonDsApp.getInstance.valoracionSocioEconomicas.sectorTerciarioPC);
			miValoracionSectorPrimario.add(getValoracion(eGisZonas.SECTER));
			miValoracion.add(miValoracionSectorTerciario);

			return miValoracion;

		}

		//PATRIMONIAL
		private IValoracion getValoracionPatrimonial()
		{

			IValoracion miValoracionGrupo = new oCompositeValoracionPatrimonialGrupo(mValoracionAreas.patrimonialesPC);
			IValoracion miValoracionApartado;

			//MONTES PUBLICOS
			miValoracionApartado = new oCompositeZonaClasificacion("07.01-" + strFrmGisGeneral.uiMONPUB, oSingletonDsApp.getInstance.valoracionPatrimoniales.montesPublicosPC);
			miValoracionApartado.add(getValoracion(eGisZonas.MONPUB));
			miValoracionGrupo.add(miValoracionApartado);

			//URBANO
			miValoracionApartado = new oCompositeZonaClasificacion("07.02-"+ strFrmGisGeneral.uiURBANO, oSingletonDsApp.getInstance.valoracionPatrimoniales.suelosUrbanosPC);
			miValoracionApartado.add(getValoracion(eGisZonas.URBANO));
			miValoracionGrupo.add(miValoracionApartado);

			//URBANIZABLE
			miValoracionApartado = new oCompositeZonaClasificacion("07.03-" + strFrmGisGeneral.uiURBANI, oSingletonDsApp.getInstance.valoracionPatrimoniales.suelosUrbanizablesPC);
			miValoracionApartado.add(getValoracion(eGisZonas.URBANI));
			miValoracionGrupo.add(miValoracionApartado);

			//NO URBANO
			miValoracionApartado = new oCompositeZonaClasificacion("07.04-" + strFrmGisGeneral.uiNOURBA, oSingletonDsApp.getInstance.valoracionPatrimoniales.suelosNoUrbanizablesPC);
			miValoracionApartado.add(getValoracion(eGisZonas.NOURBA));
			miValoracionGrupo.add(miValoracionApartado);

			//CRUCE VIAS PECUARIAS
			miValoracionApartado = new oCompositeZonaClasificacion("07.05-" + strFrmGisGeneral.uiCRVIPE, oSingletonDsApp.getInstance.valoracionPatrimoniales.cruceViasPecuariasPC);
			miValoracionApartado.add(getValoracion(eGisZonas.CRVIPE));
			miValoracionGrupo.add(miValoracionApartado);

			//YACIMIENTOS ARQUEOLOGICOS
			miValoracionApartado = new oCompositeZonaClasificacion("07.06-" + strFrmGisGeneral.uiYACARQ, oSingletonDsApp.getInstance.valoracionPatrimoniales.yacimientosArqueologicosPC);
			miValoracionApartado.add(getValoracion(eGisZonas.YACARQ));
			miValoracionGrupo.add(miValoracionApartado);

			//ZONAS ESPECIAL INTERES
			miValoracionApartado = new oCompositeZonaClasificacion("07.07-" + strFrmGisGeneral.uiZOESIN, oSingletonDsApp.getInstance.valoracionPatrimoniales.zonasEspecialInteresPC);
			miValoracionApartado.add(getValoracion(eGisZonas.ZOESIN));
			miValoracionGrupo.add(miValoracionApartado);

			//CRUCE INFRAESTRUCTURAS
			miValoracionApartado = new oCompositeZonaClasificacion("07.08-" + strFrmGisGeneral.uiCRUINF, oSingletonDsApp.getInstance.valoracionPatrimoniales.cruceInfraEstructurasLinealesPC);
			miValoracionApartado.add(getValoracion(eGisZonas.CRUINF));
			miValoracionGrupo.add(miValoracionApartado);


			//OCUPACION INFRAESTRUCTURAS PUBLICAS
			miValoracionApartado = new oCompositeZonaClasificacion("07.09-" + strFrmGisGeneral.uiOCUINF, oSingletonDsApp.getInstance.valoracionPatrimoniales.zonasInfraPublicasPC);
			miValoracionApartado.add(getValoracion(eGisZonas.OCUINF));
			miValoracionGrupo.add(miValoracionApartado);


			//OCUPACION MINAS CANTERAS
			miValoracionApartado = new oCompositeZonaClasificacion("07.10-" + strFrmGisGeneral.uiOCUMIN, oSingletonDsApp.getInstance.valoracionPatrimoniales.zonasMinasCanterasPC);
			miValoracionApartado.add(getValoracion(eGisZonas.OCUMIN));
			miValoracionGrupo.add(miValoracionApartado);

			return miValoracionGrupo;

		}

		//ECONOMICAS 
		private IValoracion getValoracionEconomica(bool iIsInversionPrivada)
		{

			IValoracion miValoracionEconomicaGrupo = new oCompositeValoracionEconomicaGrupo(mValoracionAreas.economicasPC);

			
			//Valoracion Presupuesto
			double? miPresupuestoPublicoPcaSinIva = null;
			double? miPresupuestoPrivadoPcaSinIva = null;

			using (oPresupuestoPcaPublicoPrivado miObjPresupuestoPCA = new oPresupuestoPcaPublicoPrivado(base.idSolucion))
			{
				miPresupuestoPublicoPcaSinIva = miObjPresupuestoPCA.presupuestoPCAPublico.Value;

				if (iIsInversionPrivada)
				{
					miPresupuestoPrivadoPcaSinIva = miObjPresupuestoPCA.presupuestoPCAPrivada.Value;
				}

			}

			//Valoracion de la Rentabilidad Publico-Privada

			double? miRentabilidadSocial_Van = null;
			double? miRentabilidadSocial_BC = null;

			double? miRentabilidadPrivada_Van = null;
			double? miRentabilidadPrivada_BC = null;

			oRentabilidadAnalisisSocial miObjRentabilidadSocial = new oRentabilidadAnalisisSocial(base.idSolucion);

			miRentabilidadSocial_Van = miObjRentabilidadSocial.van;
			miRentabilidadSocial_BC = miObjRentabilidadSocial.balanceBeneficiosCostesActualizados;

	 
			if (iIsInversionPrivada)
			{
				oRentabilidadAnalisisPrivado miObjRentabilidadPrivada = new oRentabilidadAnalisisPrivado(base.idSolucion);
				miRentabilidadPrivada_Van = miObjRentabilidadPrivada.van;
				miRentabilidadPrivada_BC = miObjRentabilidadPrivada.balanceBeneficiosCostesActualizados;
			}


			//Creo las Valoraciones Publicas
			IValoracion miValoracionInversionPublica = new oComponentValEconomicaPresupuestoPcaPublicoSinIva(miPresupuestoPublicoPcaSinIva.Value, oSingletonDsApp.getInstance.valoracionEconomica.publicaPresupuestoPC);
			IValoracion miValoracionRentabilidadSocial_VAN = new oComponentValEconomicaRentabilidadSocial_VAN(miRentabilidadSocial_Van.Value, oSingletonDsApp.getInstance.valoracionEconomica.publicaVanPC);
			IValoracion miValoracionRentabilidadSocial_BC = new oComponentValEconomicaRentabilidadSocial_BC(miRentabilidadSocial_BC.Value, oSingletonDsApp.getInstance.valoracionEconomica.publicaBcPC);

			miValoracionEconomicaGrupo.add(miValoracionInversionPublica);
			miValoracionEconomicaGrupo.add(miValoracionRentabilidadSocial_VAN);
			miValoracionEconomicaGrupo.add(miValoracionRentabilidadSocial_BC);


			if (iIsInversionPrivada)
			{
				IValoracion miValoracionInversionPrivada = new oComponentValEconomicaPresupuestoPcaPrivadoSinIva(miPresupuestoPrivadoPcaSinIva.Value, oSingletonDsApp.getInstance.valoracionEconomica.privadaPresupuestoPC);
				IValoracion miValoracionRentabilidadPrivada_VAN = new oComponentValEconomicaRentabilidadPrivada_VAN(miRentabilidadPrivada_Van.Value, oSingletonDsApp.getInstance.valoracionEconomica.privadaVanPC);
				IValoracion miValoracionRentabilidadPrivada_BC = new oComponentValEconomicaRentabilidadPrivada_BC(miRentabilidadPrivada_BC.Value, oSingletonDsApp.getInstance.valoracionEconomica.privadaBcPC);

				miValoracionEconomicaGrupo.add(miValoracionInversionPrivada);
				miValoracionEconomicaGrupo.add(miValoracionRentabilidadPrivada_VAN);
				miValoracionEconomicaGrupo.add(miValoracionRentabilidadPrivada_BC);
			}



			return miValoracionEconomicaGrupo;

		}


		/// <summary>
		/// Funcion para Valorar las Zonas GIS, que NO tienen Zonas por Defecto 
		/// </summary>
		private List<IValoracion> getValoracion (eGisZonas iGisCode)
		{

			List<IValoracion> miLstClasificacion = new List<IValoracion>();


			//Listado Secciones Contienen Zona con el Codigo de Busqueda
			List<oSeccionValoracion> miLstZonasGisByPk = base.getLstSeccionesByCode(iGisCode);


			//Agrupo por Clasificacion
			var miGrupoClasificacion = from p in miLstZonasGisByPk
									   group p by new { p.zonaGis.clasificacion } into g
									   select new { clasificacion = g.First().zonaGis.clasificacion };


			//Creo el Listado Clasificaciones
			foreach (var item in miGrupoClasificacion)
			{
				 miLstClasificacion.Add(new oCompositeZonaClasificacion(item.clasificacion, null));
			}


			//Añado los Hijos a las Clasificaciones
			foreach (var clasificacion in miLstClasificacion)
			{

				var miItemGroup = from p in miLstZonasGisByPk
								  where p.zonaGis.clasificacion == clasificacion.nombre
								  group p by new {p.zonaGis.id } into g
								  select new { zona = g.First().zonaGis, count = g.Count() };


				foreach (var item in miItemGroup)
				{

					string miZona =  item.zona.nombre;
					
					clasificacion.add(item.zona.getValoracion(item.count,base.numeroSeccionesTotales.Value));
				}

			}


			return miLstClasificacion;


		}





		/// <summary>
		/// Valoracion de las Zonas Design MovimientoTierras-Cimentacion-Puentes-Tuneles
		/// </summary>
		private List<IValoracion> getValoracionZonasDesign (eGisZonas iZonaGisDesign,int iNumeroSeccionesTotales)
		{


			List<IValoracion> miLstZonasValoracion = new List<IValoracion>();

			//Listado Secciones Contienen Zona con el Codigo de Busqueda
			List<oSeccionValoracion> miLstZonasGisByPk = base.getLstSeccionesByCode(iZonaGisDesign);

			//Agrupo las Zonas
			var miZonaGrupo = from p in miLstZonasGisByPk
							  group p by new { p.zonaGis.id } into g
							  select new { zonaGrupo = g.First().zonaGis, count = g.Count() };



			foreach (var item in miZonaGrupo)
			{
				miLstZonasValoracion.Add(item.zonaGrupo.getValoracion(item.count,iNumeroSeccionesTotales));
			}


			return miLstZonasValoracion;




		}


		/// <summary>
		/// Funcion para Valorar la Zona Movimiento Tierras
		/// </summary>
		private List<IValoracion> xgetValoracionZonasMovimientoTierras ()
		{


			List<IValoracion> miLstZonasValoracion = new List<IValoracion>();

			//Listado Secciones Contienen Zona con el Codigo de Busqueda
			List<oSeccionValoracion> miLstZonasGisByPk = base.getLstSeccionesByCode(eGisZonas.MOVTIE);

			//Agrupo las Zonas
			var miZonaGrupo = from p in miLstZonasGisByPk
							  group p by new { p.zonaGis.id } into g                                
							  select new { zonaGrupo = g.First().zonaGis, count = g.Count() };


		   
			foreach (var item in miZonaGrupo)
			{
				miLstZonasValoracion.Add(item.zonaGrupo.getValoracion(item.count ,base.numeroSeccionesCalzada.Value));  
			}

			
			return miLstZonasValoracion;
	 
		}



		private List<IValoracion> getValoracionPuentesCimentacion ()
		{

			List<IValoracion> miLstZonasValoracion = new List<IValoracion>();

			//Listado Secciones Contienen Zona con el Codigo de Busqueda
			List<oSeccionValoracion> miLstZonasGisByPk = base.getLstSeccionesByCode(eGisZonas.ESTCIM);


			//Agrupo las Zonas
			var miZonaGrupo = from p in miLstZonasGisByPk
							  group p by new { p.zonaGis.id } into g
							  select new { zonaGrupo = g.First().zonaGis, count = g.Count() };



			foreach (var item in miZonaGrupo)
			{
				miLstZonasValoracion.Add(item.zonaGrupo.getValoracion(base.numeroSeccionesPuentes.Value, base.numeroSeccionesTotales.Value));
			}


			return miLstZonasValoracion;




		}




		private List<IValoracion> getValoracionZonasEstructurasAndTuneles (eGisZonas iGisZona, oZonaGis iZonaGisDefault)
		{

			List<IValoracion> miLstZonasValoracion = new List<IValoracion>();

			//Listado Secciones Contienen Zona con el Codigo de Busqueda
			List<oSeccionValoracion> miLstZonasGisByPk = base.getLstSeccionesByCode(iGisZona);

			//Agrupo las Zonas
			var miZonaGrupo = from p in miLstZonasGisByPk
							  group p by new { p.zonaGis.id } into g
							  select new { zonaGrupo = g.First().zonaGis, count = g.Count() };

			int miNumeroSeccionesZonasNOdefault = 0;

			foreach (var item in miZonaGrupo)
			{
				miLstZonasValoracion.Add(item.zonaGrupo.getValoracion(item.count, base.numeroSeccionesTotales.Value));

				miNumeroSeccionesZonasNOdefault = miNumeroSeccionesZonasNOdefault + item.count;
			}

			//Ahora Creo la Zona por Defecto // No se Representa con Zonas GIS
			if (miNumeroSeccionesZonasNOdefault > base.numeroSeccionesPuentes)
			{
				throw new Exception("El Número de Secciones de Estructuras Gis\n No puede ser superior al número Totales de Secciones de Estructuras");
			}


			int miNumeroSeccionesZonasDefault = base.numeroSeccionesPuentes.Value - miNumeroSeccionesZonasNOdefault;

			if (miNumeroSeccionesZonasDefault > 0)
			{
				miLstZonasValoracion.Add(iZonaGisDefault.getValoracion(miNumeroSeccionesZonasDefault, base.numeroSeccionesTotales.Value));
			}


			throw new NotImplementedException();

		}


		public void print(string iRuta, string iFicheroSinExtension)
		{

			List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
			List<oValDesT<string, string>> miLstFooter = new List<oValDesT<string, string>>();

			miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
			miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInformeValAlternativas,string.Empty));


			tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
														 IValoracion,
														 oValDesT<string, string>>(miLstHeader, mValoracionSolucion.getDescendientes(), miLstFooter, iRuta, iFicheroSinExtension);
   
		}

	}
}
