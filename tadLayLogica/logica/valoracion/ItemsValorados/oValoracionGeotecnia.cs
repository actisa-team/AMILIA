using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{
   
	using tadLayData;
	using tadLayLogica.datos;

	using tadLayLogica.datos.proyecto;
	using tadLayLogica.datos.BaseDatos;


	using tadLayLogica.logica.valoracion;

	using engNet.Extension.Double;
	using engNet.Extension.Integer;
	
	public class oValoracionGeotecnia
	{

	  private dsBd.tbGeoRow mRowZona;
	  private dsBd.tbValExcavabilidadTaludRow mRowValoracionExcavabilidadAndTalud;
	  private dsApp.tbValGeotecniaRow mRowValGeotecnia;

	  private IValoracion mValoracion = null;
	  private double? mNotaLocalValoracionPC = null;

	  #region "Constructores"
	  public oValoracionGeotecnia(dsBd.tbGeoRow iRowZona, int iSecciones, int iSeccionesTotales)
	  {
		  mRowZona = iRowZona;
		  mRowValoracionExcavabilidadAndTalud = oSingletonDsBd.getInstance.valoracionExcavabilidadTalud;
		  mRowValGeotecnia = oSingletonDsApp.getInstance.valoracionGeotecnia;
		  mNotaLocalValoracionPC = 100*(iSecciones.toDouble() / iSeccionesTotales.toDouble());
	  }
	  public oValoracionGeotecnia(Guid iIdZona, int iSecciones, int iSeccionesTotales)
	  {
		  mRowZona = oSingletonDsBd.getInstance.getZonaMovimientoTierras(iIdZona);
		  mRowValoracionExcavabilidadAndTalud = oSingletonDsBd.getInstance.valoracionExcavabilidadTalud;
		  mRowValGeotecnia = oSingletonDsApp.getInstance.valoracionGeotecnia;
		  mNotaLocalValoracionPC = 100 * (iSecciones.toDouble() / iSeccionesTotales.toDouble());
	  }
	  #endregion






	   #region "Propiedades"
	   public IValoracion valoracion
	  {
		  get
		  {
			  if (mValoracion == null)
			  {
				  mValoracion = new oCompositeValoracionGeotecniaItem(mRowZona.nombre,mNotaLocalValoracionPC.Value);
				  mValoracion.add(new oComponentValGeotecniaEstabilidadHorizontalTerreno(this.valoracionEstabilidadHorizontalTerreno, this.mRowValGeotecnia.estabilidadHorizontalTerrenoPC));
				  mValoracion.add(new oComponentValGeotecniaEstabilidadTaludes(this.valoracionEstabilidadTaludDesmonte, this.mRowValGeotecnia.estabilidadTaludPC));
				  mValoracion.add(new oComponentValGeotecniaValorCBR(this.valoracionCbr, this.mRowValGeotecnia.valorCbrPC));
				  mValoracion.add(new oComponentValGeotecniaAprovechamientos(this.valoracionAprovechamientos, this.mRowValGeotecnia.aprovechamientosPC));
				  mValoracion.add(this.valoracionExcavabilidad);
				  mValoracion.add(this.valoracionTaludProteccion);
			  }

			  return mValoracion;
		  }
	  }
	   #endregion



	   #region "Propiedades Valoracion"
	   /// <summary>
		///Valoracion Estabilidad Horizontal Terreno
		/// </summary>
		public double valoracionEstabilidadHorizontalTerreno
		{
			get
			{ 
				return getEstabilidadHorizontalTerreno(mRowZona.penMaxTerreno);
			}
		}
		/// <summary>
		///Valoracion Estabilidad Talud Desmonte
		/// </summary>
		public double valoracionEstabilidadTaludDesmonte
		{
			get
			{
				return getEstabilidadTaludDesmonte(mRowZona.desmonteTalud, valChd, valChbm);
			}
		}
		/// <summary>
		/// Valoracion CBR
		/// </summary>
		public double valoracionCbr
		{
			get
			{
				return getCbrValoracion(mRowZona.cbr);
			}
		}
		/// <summary>
		/// Valoracion Aprovechamientos
		/// </summary>
		public double valoracionAprovechamientos
		{
			get
			{
				return getValAprovechamientos(mRowZona.granularAproPc, mRowZona.asientoAproPc, mRowZona.rellenoAproPc);
			}
		}
		/// <summary>
		/// Valoración de la Excavabilidad
		/// </summary>
		public oCompositeValoracionExcavabilidad valoracionExcavabilidad
		{
			get
			{
				return getValoracionExcavabilidad(mRowValGeotecnia, mRowValoracionExcavabilidadAndTalud, mRowZona); 
			}

		}
		/// <summary>
		/// Valoracion Talud
		/// </summary>
		public oCompositeValoracionTalud valoracionTaludProteccion
		{
			get
			{
				return getValoracionProteccionTalud(mRowValGeotecnia,mRowValoracionExcavabilidadAndTalud, mRowZona); 
			}
		}
		/// <summary>
		///Valoracion / Coeficiente Altura Desmonte
		/// </summary>
		public double valChd
		{
			get
			{
				if (mRowZona.desmonteAlturaMaxima <= 20)
				{
					return 1;
				}
				else if (mRowZona.desmonteAlturaMaxima > 20 && mRowZona.desmonteAlturaMaxima <= 30)
				{
					return 0.75;
				}
				else if (mRowZona.desmonteAlturaMaxima > 30)
				{
					return 0.5;
				}
				else
				{
					throw new NotImplementedException(string.Format("Error en la Valoración de la Coeficiente Chd\n Valor Chd : {0}", mRowZona.desmonteAlturaMaxima));
				}

			}

		}
		/// <summary>
		///Valoracion / Coeficiente Estabilidad Taludes Bermas-Muros
		/// </summary>
		public double valChbm
		{
			get
			{
				if (mRowZona.desmonteConstanteIs)
				{
					return 1;
				}
				else if (mRowZona.desmonteBermaIs)
				{
					return 1.2;
				}
				else if (mRowZona.desmonteMuroIs)
				{
					return 1.5;
				}
				else
				{
					throw new NotImplementedException("Estabilidad Desmonte Talud-Berma");
				}

			}


		}

		#endregion


	   


	   #region "Static Function Valoración"

		public static double getEstabilidadHorizontalTerreno (double iPendienteMaxPC)
		{

			double miArcTg = Math.Atan( Math.Abs(iPendienteMaxPC) / 100);

			double miPendienteAdmisible =  tadLayShare.puntos.oTrigo.getGradosFromRadianes(miArcTg);

			double miSumando1 = oExtensionDouble.getDivision(1,9) * miPendienteAdmisible;

			double miValor = 10 - miSumando1;

			return engNet.math.rangos.oMathRangos.getValueFromMinAndMax(miValor,0, 10);

		}
		public static double getEstabilidadTaludDesmonte (double iDesmonteTalud, double iDesmonteAlturaCoe, double iDesmonteBermaMuroCoe)
		{

			if (iDesmonteTalud >= 4)
			{
				return 10;
			}
			else if (iDesmonteTalud == 0)
			{
				return 0;
			}
			else
			{
			   double miValor = 2.5* iDesmonteTalud * iDesmonteAlturaCoe * iDesmonteBermaMuroCoe;

			   return engNet.math.rangos.oMathRangos.getValueFromMinAndMax(miValor,0,10);
			}
			   
		}
		public static double getCbrValoracion (double iCbr)
		{
		
			if (iCbr>50)
			{
				return 0;
			}
			else if (iCbr < 2)
			{
			   return 10;
			}
			else
			{  
				double miValor = (oExtensionDouble.getDivision(-10,48)* iCbr)  +  oExtensionDouble.getDivision(500,48);

				return engNet.math.rangos.oMathRangos.getValueFromMinAndMax(miValor,0,10);
			}
		}
		public static double getValAprovechamientos (double iAproCapasGranulares, double iAproCapasAsiento, double iAproRellenos)
		{
			double miValor = (oExtensionDouble.getDivision(-10,300) * (iAproCapasGranulares+iAproCapasAsiento+iAproRellenos)) +10;

			return engNet.math.rangos.oMathRangos.getValueFromMinAndMax(miValor,0,10);
		}



		public static oCompositeValoracionExcavabilidad getValoracionExcavabilidad(dsApp.tbValGeotecniaRow iRowValGeotecnia, dsBd.tbValExcavabilidadTaludRow iRowValNotas, dsBd.tbGeoRow iRowZona)
		{

			  oCompositeValoracionExcavabilidad miValExcavabilidad = new oCompositeValoracionExcavabilidad(iRowValGeotecnia.excavabilidadPC);

			  miValExcavabilidad.add(new oComponetValExcavacionMediosConvencionales(iRowValNotas.excMediosConvencionales, iRowZona.excavaMediosConvencionalesPC));
			  miValExcavabilidad.add(new oComponetValExcavacionMartillo(iRowValNotas.excMartillo, iRowZona.excavaMartilloPC));
			  miValExcavabilidad.add(new oComponetValExcavacionVoladuras(iRowValNotas.excVoladuras, iRowZona.excavaVoladurasPC));
			  miValExcavabilidad.add(new oComponetValExcavacionNivelFreatico(iRowValNotas.excNivelFreatico, iRowZona.excavaNivelFreaticoPC));
			  miValExcavabilidad.add(new oComponetValExcavacionVertedero2Fases(iRowValNotas.excVertedero2Fases, iRowZona.excavaVertedero2FasesPC));

			  return miValExcavabilidad;
		}
		public static oCompositeValoracionTalud getValoracionProteccionTalud(dsApp.tbValGeotecniaRow iRowValGeotecnia, dsBd.tbValExcavabilidadTaludRow iRowValNotas, dsBd.tbGeoRow iRowZona)
		{
			oCompositeValoracionTalud miValTalud = new oCompositeValoracionTalud(iRowValGeotecnia.proteccionTaludesPC);

			miValTalud.add(new oComponentValTaludProteccionSin(iRowValNotas.talProtecionesSin, iRowZona.taludProteccionSinPC));
			miValTalud.add(new oComponentValTaludProteccionFlexible(iRowValNotas.talProteccionesFlexibles, iRowZona.taludProteccionFlexiblePC));
			miValTalud.add(new oComponentValTaludProteccionRigida(iRowValNotas.talProteccionesRigidas, iRowZona.taludProteccionRigidasPC));

			return miValTalud;
		}
		
	   #endregion


	}
}
