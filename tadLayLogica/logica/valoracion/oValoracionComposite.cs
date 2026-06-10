using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using System.ComponentModel;

using System.Runtime.Serialization;



//UPDATE 24.09.14
namespace tadLayLogica.logica.valoracion
{

	using engNet.ClassT;
	using engNet.CustomAtributos;
	using engNet.Extension.Double;

	using System.Xml.Serialization;
	using tadLayShare;
    using tadLayLan.Tdi;

	public interface IValoracionPropiedad
	{
		IValoracion valoracion { get; }
	}  
	public interface  IValoracion
	{

        [LocalizedDisplayName("uiNombre", typeof(strFrmInformes))]
		[BindingInfo(SortIndex = 1)]
		[DefaultValue(null)]
		string nombre { get; set; }


        [LocalizedDisplayName("uiNotaLocal", typeof(strFrmInformes))]
		[BindingInfo(SortIndex = 2)]
		double? notaLocal { get; set; }


        [LocalizedDisplayName("uiNotaGlobal", typeof(strFrmInformes))]
		[BindingInfo(SortIndex = 3)]
		double? notaGlobal { get;}


        [LocalizedDisplayName("uiValNota", typeof(strFrmInformes))]
		[BindingInfo(SortIndex = 4)]
		double? notaValoracionPC { get; set; }


   
		[Browsable(false)]
		double? notaGlobalPorNotaValoracionPC { get; }

		[Browsable(false)]
		List<IValoracion> lstChild { get; set; }

	  
		[Browsable(false)]
		bool isLeaf { get; }

  
		[Browsable(false)]
		oNotaMaximaMinima notasMaximasMinima { get; set; }

		void add(IValoracion iItemValoracion);

		void add(List<IValoracion> iLstValoraciones);

		List<IValoracion> getDescendientes();

		string print();
 
	}


   
	public abstract class oValoracionComponent : IValoracion
	{
	   
		[DefaultValue("")]
		public string nombre { get; set; }


		[DefaultValue(null)]
		public virtual double? notaLocal { get; set; }

		[DefaultValue(null)]
		public abstract double? notaGlobal { get; }


		[DefaultValue(null)]
		public double? notaValoracionPC { get; set; }

 
		[DefaultValue(null)]
		public List<IValoracion> lstChild { get; set; }



		public oNotaMaximaMinima notasMaximasMinima { get; set; }
 


		#region "Constructor"
		public oValoracionComponent()
		{
			lstChild = new List<IValoracion>();
		}
		public oValoracionComponent(string iNombre)
		{
			nombre = iNombre;
			lstChild = new List<IValoracion>();
		}
		public oValoracionComponent(double  iNotaValoracionPC)
		{
			notaValoracionPC = iNotaValoracionPC;
			lstChild = new List<IValoracion>();
		}
		public oValoracionComponent(string iNombre, double iNotaValoracionPC)
		{
			nombre = iNombre;
			notaValoracionPC = iNotaValoracionPC;
			lstChild = new List<IValoracion>();
		}
		public oValoracionComponent(double? iNotaLocal, double iNotaValoracionPC)
		{
			notaLocal = iNotaLocal;
			notaValoracionPC = iNotaValoracionPC;
			lstChild = new List<IValoracion>();
		}
		public oValoracionComponent(string iNombre, double? iNotaLocal, double iNotaValoracionPC)
		{
			nombre = iNombre;
			notaLocal = iNotaLocal;
			notaValoracionPC = iNotaValoracionPC;
			lstChild = new List<IValoracion>();
		}
		#endregion

	   
		public bool isLeaf
		{
			get
			{
				return lstChild.Count == 0;
			}
		}

	   

		public virtual void add(IValoracion iItemValoracion)
		{
			throw new NotImplementedException("No puedes Añadir un Criterio de Valoración");
		}
		public virtual void add(List<IValoracion> iLstValoraciones)
		{
			throw new NotImplementedException();
		}

		public virtual string print()
		{
			return this.nombre;
		}

		public override string ToString()
		{
			return strFrmInformes.uiNombre + ": " + this.nombre + "\n" +
                    strFrmInformes.uiNotaLocal + ": " + this.notaLocal.ToString() + "\n" +
					this.notasMaximasMinima.ToString() + "\n" +
					strFrmInformes.uiNotaGlobal +":" + this.notaGlobal.ToString() + "\n" +
					strFrmInformes.uiValNotaTantoPorCiento + ":" + this.notaValoracionPC.ToString();            
		}





		public virtual List<IValoracion> getDescendientes()
		{
		   

				List<IValoracion> miLst = new List<IValoracion>();

				miLst.Add(this);

				return miLst;
			
		}



		public double? notaGlobalPorNotaValoracionPC
		{
			get
			{
				return notaGlobal * notaValoracionPC;
			
			}
		}


		
	
	}


	public abstract class oValoracionComposite : oValoracionComponent
	{

		#region "Constructores"

		public oValoracionComposite()
		{

		}

		public oValoracionComposite(string iNombre)
			: base(iNombre)
		{

		}

		public oValoracionComposite(string iNombre, double iNotaValoracionPC)
			: base(iNombre, iNotaValoracionPC)
		{

		}

		public oValoracionComposite(double iNotaValoracionPC)
			:base(iNotaValoracionPC)
		{


		}


		#endregion



		public override void add(IValoracion iItemValoracion)
		{
			lstChild.Add(iItemValoracion);
		}
		public override void add(List<IValoracion> iLstValoraciones)
		{
			
			foreach (var item in iLstValoraciones)
			{
				lstChild.Add(item);
			}


		}


	 
		public override string print()
		{

			string miPrint = this.nombre;
			

			IEnumerator<IValoracion> miEnumerator = lstChild.GetEnumerator();

			while (miEnumerator.MoveNext())
			{
				miPrint = miPrint + "\n" + miEnumerator.Current.print();
			}

			return miPrint;
		}

	 
		public override List<IValoracion> getDescendientes()
		{

			

				List<IValoracion> miLst = new List<IValoracion>();

				miLst.Add(this);

				IEnumerator<IValoracion> miEnumerator = lstChild.GetEnumerator();


				while (miEnumerator.MoveNext())
				{
					miLst.AddRange(miEnumerator.Current.getDescendientes());
				}



				return miLst;

			
			
		}


		public override double? notaLocal
		{
			get
			{
				double? miSumNotasPorValoracion = lstChild.Sum(p => p.notaGlobalPorNotaValoracionPC);
				double? miPorcentajesSum = lstChild.Sum(p => p.notaValoracionPC);

				if (miSumNotasPorValoracion.HasValue && miPorcentajesSum.HasValue)
				{
					if (miPorcentajesSum.Value == 0)
					{
						return 0;
					}
					else
					{
						return miSumNotasPorValoracion / miPorcentajesSum;
					}     
				}
				else
	
				{
					return null;
				}

				
			}
			set
			{
				throw new Exception("No se Puede Asignar una Nota Local a un Objeto Composite");
			}
		}

		public override double? notaGlobal
		{
			get
			{
				return null;
			}
		}
	}

	public class oCompositeValoracionHipotesis : oValoracionComposite
	{

		public oCompositeValoracionHipotesis()
		{

		}


		public oCompositeValoracionHipotesis(string iHipotesisNombre)
			:base(iHipotesisNombre)
		{
			notaValoracionPC = null;
		}


#region "Propiedades"

	   public override double? notaLocal
		{
			get
			{
				return null;
			}
		}
#endregion

#region "Metodos"


		public void printResumen (string iFile)
	   {








	   }



		public oCompositeValoracionSolucion getSolucionByName (string iNameSolucion)
		{
		
		   foreach (var item in lstChild)
			{
		 
			   if (item.nombre.Equals(iNameSolucion,StringComparison.OrdinalIgnoreCase))
			   {
				   return item as oCompositeValoracionSolucion;
			   }

			}

			throw new Exception (string.Format("La solución {0} No Existe en la Actual Valoración",iNameSolucion));
		}



#endregion



	}

	public class oCompositeValoracionSolucion : oValoracionComposite
	{

		private Guid? mId = null;
		
		
		
		#region "Constructores"
		public oCompositeValoracionSolucion(Guid iIdSolucion, string iSolucionNombre)
			: base(iSolucionNombre)
		{
			mId = iIdSolucion;
		}
		#endregion


		#region "Propiedades"

		[DisplayName("Id")]
		[BindingInfo(SortIndex = 0)]
		public Guid id
		{
			get
			{

				if (mId.HasValue)
				{
				   return mId.Value;
				}
				else
				{
					throw new oExPropertieNullValue("id Valoración Solución");
				}
			   

			}

		}


		#endregion


		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_ValorMaximoCalificaDiez(notaLocal.Value);
				}
				else
				{
					return null;
				}
				 
			}
		}


		public IValoracion getCapitulos (string iCapitulo)
		{

			foreach (var item in lstChild)
			{
				if (item.nombre.Trim().Equals(iCapitulo.Trim(), StringComparison.OrdinalIgnoreCase))
				{
					return item as IValoracion;
				}

			}

			throw new Exception(string.Format("La Valoracion {0} No Existe en la Actual Valoración", iCapitulo));

		}


	}
   

	//COMPOSITE CAPITULOS VALORACION
	public abstract class oCompositeValoracionGrupo : oValoracionComposite
	{

		public oCompositeValoracionGrupo()
		{

		}
		public oCompositeValoracionGrupo(string iNombre, double? iNotaValoracionPC)
		{
			this.nombre = iNombre;
			this.notaValoracionPC = iNotaValoracionPC;
		}


		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_Capitulo(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}

	}

	public class oCompositeValoracionTrazadoGrupo : oCompositeValoracionGrupo
	{
		#region "Constructores"

		public oCompositeValoracionTrazadoGrupo(double? iNotaValoracionPC)
            : base("01-" + strFrmInformes.uiValGrupo01, iNotaValoracionPC)
		{

		}

		#endregion


	}


	public class oCompositeValoracionGeotecniaGrupo : oCompositeValoracionGrupo
	{
		#region "Constructores"

		public oCompositeValoracionGeotecniaGrupo()
		{

		}


		public oCompositeValoracionGeotecniaGrupo(double? iNotaValoracionPC)
            : base("02-" + strFrmInformes.uiValGrupo02, iNotaValoracionPC)
		{

		}
		#endregion


		public override double? notaLocal
		{
			get
			{
				double? miSumNotasPorValoracion = lstChild.Sum(p => p.notaGlobalPorNotaValoracionPC);
				double? miPorcentajesSum = 100;

				if (miSumNotasPorValoracion.HasValue && miPorcentajesSum.HasValue)
				{
					if (miPorcentajesSum.Value == 0)
					{
						return 0;
					}
					else
					{
						return miSumNotasPorValoracion / miPorcentajesSum;
					}
				}
				else
				{
					return null;
				}


			}
			set
			{
				throw new Exception("No se Puede Asignar una Nota Local a un Objeto Composite");
			}
		}


	}
	public class oCompositeValoracionEstructurasTunelesMurosGrupo : oCompositeValoracionGrupo
	{
		#region "Constructores"
		public oCompositeValoracionEstructurasTunelesMurosGrupo(double? iNotaValoracionPC)
            : base("03-" + strFrmInformes.uiValGrupo03, iNotaValoracionPC)    
		{

		}

		#endregion
	}
	public class oCompositeValoracionAmbientalGrupo : oCompositeValoracionGrupo
	{
		public oCompositeValoracionAmbientalGrupo(double? iNotaValoracionPC)
            : base("04-" + strFrmInformes.uiValGrupo04, iNotaValoracionPC)
		{

		}
	}
	public class oCompositeValoracionClimaticaGrupo : oCompositeValoracionGrupo
	{
		public oCompositeValoracionClimaticaGrupo(double? iNotaValoracionPC)
            : base("05-" + strFrmInformes.uiValGrupo05, iNotaValoracionPC)
		{

		}
	}
	public class oCompositeValoracionSocioEconomicosGrupo : oCompositeValoracionGrupo
	{
		public oCompositeValoracionSocioEconomicosGrupo(double? iNotaValoracionPC)
            : base("06-" + strFrmInformes.uiValGrupo06, iNotaValoracionPC)
		{

		}
	}
	public class oCompositeValoracionPatrimonialGrupo : oCompositeValoracionGrupo
	{
		public oCompositeValoracionPatrimonialGrupo(double? iNotaValoracionPC)
            : base("07-" +strFrmInformes.uiValGrupo07, iNotaValoracionPC)
		{

		}
	}
	public class oCompositeValoracionEconomicaGrupo : oCompositeValoracionGrupo
	{
		public oCompositeValoracionEconomicaGrupo(double? iNotaValoracionPC)
            : base("08-" + strFrmInformes.uiValGrupo08, iNotaValoracionPC)
		{

		}


	}



	//COMPONENT TRAZADO
	public class oComponentValTrazadoPlanta : oValoracionComponent
	{


		public oComponentValTrazadoPlanta(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiPlanta, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_Capitulo(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}




	}
	public class oComponentValTrazadoAlzado : oValoracionComponent
	{
		public oComponentValTrazadoAlzado(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiAlzado, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue && !double.IsNaN(notaLocal.Value))
				{
					return notasMaximasMinima.getNota_Capitulo(notaLocal.Value);
				}
				else
				{
					return null;
				}
			}
		}
	}
	public class oComponentValTrazadoTiempo : oValoracionComponent
	{
		public oComponentValTrazadoTiempo(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiTiempo, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue && !double.IsNaN(notaLocal.Value))
				{
					return notasMaximasMinima.getNota_Capitulo(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}

	}
	public class oComponentValTrazadoTierrasVolumenMovimiento : oValoracionComponent
	{
		public oComponentValTrazadoTierrasVolumenMovimiento(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiVolMovTierras, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{

				if (notasMaximasMinima != null && notaLocal.HasValue && !double.IsNaN(notaLocal.Value))
				{
					return notasMaximasMinima.getNota_Capitulo(notaLocal.Value);
				}
				else
				{
					return null;
				}


			}
		}


	}
	public class oComponentValTrazadoTierrasCompensacion : oValoracionComponent
	{
		public oComponentValTrazadoTierrasCompensacion(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiTierrasComp, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue && !double.IsNaN(notaLocal.Value))
				{
					return notasMaximasMinima.getNota_Capitulo(notaLocal.Value);
				}
				else
				{
					return null;
				}


			}
		}

	}


	//COMPOSITE GEOTECNIA
	public class oCompositeValoracionGeotecniaItem : oCompositeValoracionGeotecniaGrupo
	{

		#region "Constructores"

		public oCompositeValoracionGeotecniaItem(string iZonaName, int iSeccionesZona, int iSeccionesTotales)
		{
			this.nombre = iZonaName;
			this.notaValoracionPC = Convert.ToDouble(iSeccionesZona / iSeccionesTotales).roundOff(2);
		}

		public oCompositeValoracionGeotecniaItem(string iZonaName, double iNotaValoracionPC)
		{
			this.nombre = iZonaName;
			this.notaValoracionPC = iNotaValoracionPC;
		}

		public oCompositeValoracionGeotecniaItem(string iZonaName)
		{
			this.nombre = iZonaName;
			this.notaValoracionPC = null;
		}
		#endregion


		public override double? notaGlobal
		{
			get
			{
				return base.notaLocal;
			}
		}

	}
	public class oComponentValGeotecniaEstabilidadHorizontalTerreno : oValoracionComponent
	{

		public oComponentValGeotecniaEstabilidadHorizontalTerreno(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiEstabHorTerreno, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				return notaLocal.Value;
			}
		}

	}
	public class oComponentValGeotecniaEstabilidadTaludes : oComponentValGeotecniaEstabilidadHorizontalTerreno
	{

		public oComponentValGeotecniaEstabilidadTaludes(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiEstabTaludes;
		}


	}
	public class oComponentValGeotecniaValorCBR : oComponentValGeotecniaEstabilidadHorizontalTerreno
	{

		public oComponentValGeotecniaValorCBR(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiValCBR;
		}

	}
	public class oComponentValGeotecniaAprovechamientos : oComponentValGeotecniaEstabilidadHorizontalTerreno
	{

		public oComponentValGeotecniaAprovechamientos(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiValAprovechamientos;
		}

	}
	public class oCompositeValoracionExcavabilidad : oValoracionComposite
	{
		public oCompositeValoracionExcavabilidad(double iNotaValoracionPC)
            : base(strFrmInformes.uiValExcavabilidad, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				return notaLocal.Value;
			}
		}

	}
	public class oComponetValExcavacionMediosConvencionales : oValoracionComponent
	{
		public oComponetValExcavacionMediosConvencionales(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiExcavMediosConvenc;

		}

		public override double? notaGlobal
		{
			get
			{
				return notaLocal.Value;
			}
		}


	}
	public class oComponetValExcavacionMartillo : oComponetValExcavacionMediosConvencionales
	{
		public oComponetValExcavacionMartillo(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiExcavMatillo;

		}
	}
	public class oComponetValExcavacionVoladuras : oComponetValExcavacionMediosConvencionales
	{
		public oComponetValExcavacionVoladuras(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiExcavVoladuras;

		}
	}
	public class oComponetValExcavacionNivelFreatico : oComponetValExcavacionMediosConvencionales
	{
		public oComponetValExcavacionNivelFreatico(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiExcavSisAgotamienro;

		}
	}
	public class oComponetValExcavacionVertedero2Fases : oComponetValExcavacionMediosConvencionales
	{
		public oComponetValExcavacionVertedero2Fases(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiExcavRetiradaVer;

		}
	}
	public class oCompositeValoracionTalud : oValoracionComposite
	{

		public oCompositeValoracionTalud(double iNotaValoracionPC)
            : base(strFrmInformes.uiProteccTaludes, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				return notaLocal.Value;
			}
		}


	}
	public class oComponentValTaludProteccionSin : oValoracionComponent
	{
		public oComponentValTaludProteccionSin(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiTaludSinProtec;

		}

		public override double? notaGlobal
		{
			get
			{
				return notaLocal.Value;
			}
		}

	}
	public class oComponentValTaludProteccionFlexible : oComponentValTaludProteccionSin
	{
		public oComponentValTaludProteccionFlexible(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiProtecFlexible;

		}

	}
	public class oComponentValTaludProteccionRigida : oComponentValTaludProteccionSin
	{
		public oComponentValTaludProteccionRigida(double iNotaLocal, double iNotaValoracionPC)
			: base(iNotaLocal, iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiTaludprotecRigida;

		}

	}

	//VALORACION ESTRUCTURAS
	public class oCompositeValoracionPuentesCimentacion : oCompositeValoracionEstructurasTunelesMurosGrupo
	{
		public oCompositeValoracionPuentesCimentacion(double iNotaValoracionPC)
			:base(iNotaValoracionPC)
		{
			this.nombre = "03.01-" + strFrmInformes.uiValGrupo0301;
		}

		public override double? notaGlobal
		{
			get
			{
				return notaLocal.Value;
			}
		}


		//ES IGUAL QUE GrupoMovimientoTierras+Puentes+Tuneles
		//No Dividimos por el sumatorio de los hijos sino por 100
		public override double? notaLocal
		{
			get
			{
				double? miSumNotasPorValoracion = lstChild.Sum(p => p.notaGlobalPorNotaValoracionPC);
				double? miPorcentajesSum = 100;

				if (miSumNotasPorValoracion.HasValue && miPorcentajesSum.HasValue)
				{
					if (miPorcentajesSum.Value == 0)
					{
						return 0;
					}
					else
					{
						return miSumNotasPorValoracion / miPorcentajesSum;
					}
				}
				else
				{
					return null;
				}


			}
			set
			{
				throw new Exception("No se Puede Asignar una Nota Local a un Objeto Composite");
			}
		}




	}
	public class oCompositeValoracionPuentesCimentacionZonas : oCompositeValoracionPuentesCimentacion
	{
		public oCompositeValoracionPuentesCimentacionZonas(string iZonaNombre, double iNotaValoracionPC)
			: base(iNotaValoracionPC)
		{
			this.nombre = iZonaNombre; 
		}

	}
	public class oComponetValPuentesCimentacion : oValoracionComponent
	{
		public oComponetValPuentesCimentacion(string iNombre, double iNotaLocal, double iNotaValoracionPC)
			:base(iNombre,iNotaLocal,iNotaValoracionPC)
		{

		}


		public override double? notaGlobal
		{
			get
			{
				return base.notaLocal;
			}
		}

	}
	public class oComponetValPuentesProcedimientosExcavacion : oComponetValPuentesCimentacion
	{
		public oComponetValPuentesProcedimientosExcavacion(string iNombre, double iNotaLocal, double iNotaValoracionPC)
			: base(iNombre, iNotaLocal, iNotaValoracionPC)
		{

		}



	}
	public class oComponetValPuentesObrasMenores : oComponetValPuentesCimentacion
	{
		public oComponetValPuentesObrasMenores(string iNombre, double iNotaLocal, double iNotaValoracionPC)
			: base(iNombre, iNotaLocal, iNotaValoracionPC)
		{

		}



	}
	public class oComponetValPuentesPresenciaAgua : oComponetValPuentesCimentacion
	{
		public oComponetValPuentesPresenciaAgua(string iNombre, double iNotaLocal, double iNotaValoracionPC)
			: base(iNombre, iNotaLocal, iNotaValoracionPC)
		{

		}
	}


	//VALORACION TUNELES
	public class oCompositeValoracionTuneles : oCompositeValoracionEstructurasTunelesMurosGrupo
	{
		public oCompositeValoracionTuneles(double iNotaValoracionPC)
			: base(iNotaValoracionPC)
		{
            this.nombre = "03.02-" + strFrmInformes.uiValGrupo0302;
		}

		public override double? notaGlobal
		{
			get
			{
				return notaLocal.Value;
			}
		}


		//ES IGUAL QUE GrupoMovimientoTierras+Puentes+Tuneles
		//No Dividimos por el sumatorio de los hijos sino por 100
		public override double? notaLocal
		{
			get
			{
				double? miSumNotasPorValoracion = lstChild.Sum(p => p.notaGlobalPorNotaValoracionPC);
				double? miPorcentajesSum = 100;

				if (miSumNotasPorValoracion.HasValue && miPorcentajesSum.HasValue)
				{
					if (miPorcentajesSum.Value == 0)
					{
						return 0;
					}
					else
					{
						return miSumNotasPorValoracion / miPorcentajesSum;
					}
				}
				else
				{
					return null;
				}


			}
			set
			{
				throw new Exception("No se Puede Asignar una Nota Local a un Objeto Composite");
			}
		}

	}
	public class oComponetValTunelesSinTuneles : oValoracionComponent
	{

		/// <summary>
		///  Valoración de Zonas Que No Tienen Tuneles
		/// </summary>
		/// <param name="iNotaValoracionPC">% de Secciones que NO Tienes Tuneles</param>
		public oComponetValTunelesSinTuneles(double iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiZonaSinTuneles;
			this.notaLocal = 0;
			this.notaValoracionPC = iNotaValoracionPC;
		}



		public override double? notaGlobal
		{
			get
			{
				return base.notaLocal;
			}
		}


	}
	public class oCompositeValoracionTunelesZonas : oCompositeValoracionTuneles
	{
		public oCompositeValoracionTunelesZonas(string iZonaNombre, double iNotaValoracionPC)
			: base(iNotaValoracionPC)
		{
			this.nombre = iZonaNombre;
		}



	}
	public class oComponetValTunelesRmr : oValoracionComponent
	{

		public oComponetValTunelesRmr (double iNotaLocal, double iNotaValoracionPC)
			:base(iNotaLocal,iNotaValoracionPC)
		{
			this.nombre = "Rmr";
		}

		public override double? notaGlobal
		{
			get
			{
				return base.notaLocal;
				   
			}
		}

	}
	public class oComponentValTunMetodosExcavacion : oValoracionComponent
	{
		public oComponentValTunMetodosExcavacion(double iNotaLocal, double iNotaValoracionPC)
			:base(iNotaLocal,iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiMetodosExcav;
		}

		public override double? notaGlobal
		{
			get
			{
				return base.notaLocal;
			}
		}
	}
	public class oComponentValTunTratamientosEspecificos : oComponentValTunMetodosExcavacion
	{

		public oComponentValTunTratamientosEspecificos(double iNotaLocal, double iNotaValoracionPC)
			:base(iNotaLocal,iNotaValoracionPC)
		{
            this.nombre = strFrmInformes.uiTratamientosEspecif;
		}
	}


	//VALORACION MUROS
	public class oCompositeValoracionMuros : oCompositeValoracionEstructurasTunelesMurosGrupo
	{
		public oCompositeValoracionMuros(double iNotaValoracionPC)
			: base(iNotaValoracionPC)
		{
            this.nombre = "03.03-" + strFrmInformes.uiValGrupo0303;
		}

		public override double? notaGlobal
		{
			get
			{
				return notaLocal.Value;
			}
		}

	}
	public class oComponetValEstructurasMuro : oValoracionComponent
	{

		public oComponetValEstructurasMuro(double iMedicionMuroM3)
		{
            this.nombre = strFrmInformes.uiMuro;
			this.notaLocal = iMedicionMuroM3;
			this.notaValoracionPC = 100; //Solo Tenemos un Tipo de Muro
		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_Muro(notaLocal.Value);
				}
				else
				{
					return null;
				}
			}
		}
	}



	//ZONAS GIS ; GRUPO-CLASIFICACION-ITEM
	public class oCompositeZonaGrupo : oValoracionComposite
	{

		#region "Constructores"
		public oCompositeZonaGrupo()
		{

		}
		public oCompositeZonaGrupo(string iGrupo, double? iGrupoValoracionPC)
		{
			this.nombre = iGrupo;
			this.notaValoracionPC = iGrupoValoracionPC;
		}
		#endregion

		public override double? notaLocal
		{
			get
			{

				double miNotaLocal = lstChild.Sum(p => p.notaLocal.Value);

				if (miNotaLocal > 10)
				{
					return 10;
				}
				else
				{
					return miNotaLocal;
				}

			}
			set
			{
				throw new Exception("No Podemos Asignar una Nota Local a una Zona de Clasificación");
			}
		}
		public override double? notaGlobal
		{
			get
			{
				return this.notaLocal;
			}
		}

	}
	public class oCompositeZonaClasificacion : oCompositeZonaGrupo
	{

		public oCompositeZonaClasificacion(string iZonaCode, double? iNotaValoracionPC)   
		{
			this.nombre = iZonaCode;
			this.notaValoracionPC = iNotaValoracionPC;
		}

	}
	public class oComponentZonaItem : oValoracionComponent
	{

		double? mZonaNota = null;
		double? mSeccionesPC = null;
 
		public oComponentZonaItem(string iZonaNombre, double iZonaNota, double iNumeroZonas, double iNumeroSeccionesTotales)
		{
			this.nombre = iZonaNombre;
			this.notaValoracionPC = null;
			mZonaNota = iZonaNota;
			mSeccionesPC = iNumeroZonas / iNumeroSeccionesTotales;
		}
		public override double? notaLocal
		{
			  get 
			{
				return mSeccionesPC * mZonaNota;
			}
			  set 
			{
				throw new Exception("No Podemos Asignar una Nota Local a una Zona.");
			}
		}
		public override double? notaGlobal
		{
			get
			{
				return notaLocal;
			}
		}

	}


	//ECONOMICAS
	public abstract class oComponentValEconomicaPresupuesto : oValoracionComponent
	{


		public  oComponentValEconomicaPresupuesto (string iNombre,  double iNotaLocal, double iNotaValoracionPC)
			: base(iNombre , iNotaLocal, iNotaValoracionPC)
		{

		}






	}

	public class oComponentValEconomicaPresupuestoPcaPublicoSinIva : oComponentValEconomicaPresupuesto
	{


		public oComponentValEconomicaPresupuestoPcaPublicoSinIva(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiPresConoAdminisPublico, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_Capitulo(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}




	}
	public class oComponentValEconomicaRentabilidadSocial_VAN : oComponentValEconomicaPresupuesto
	{
		public oComponentValEconomicaRentabilidadSocial_VAN(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiRentabSocialVAN, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_VAN_BC(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}

	}
	public class oComponentValEconomicaRentabilidadSocial_BC : oComponentValEconomicaPresupuesto
	{
		public oComponentValEconomicaRentabilidadSocial_BC(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiRentabSocialBC, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_VAN_BC(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}

	}

	public class oComponentValEconomicaPresupuestoPcaPrivadoSinIva : oComponentValEconomicaPresupuesto
	{

		public oComponentValEconomicaPresupuestoPcaPrivadoSinIva(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiPresupuConoAdminisPrivado, iNotaLocal, iNotaValoracionPC)
		{

		}


		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_Capitulo(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}




	}
	public class oComponentValEconomicaRentabilidadPrivada_VAN : oComponentValEconomicaPresupuesto
	{
		public oComponentValEconomicaRentabilidadPrivada_VAN(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiRentabprivadaVAN, iNotaLocal, iNotaValoracionPC)
		{

		}

		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_VAN_BC(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}

	}
	public class oComponentValEconomicaRentabilidadPrivada_BC : oComponentValEconomicaPresupuesto
	{
		public oComponentValEconomicaRentabilidadPrivada_BC(double iNotaLocal, double iNotaValoracionPC)
            : base(strFrmInformes.uiRentabPrivadaBC, iNotaLocal, iNotaValoracionPC)
		{

		}


		public override double? notaGlobal
		{
			get
			{
				if (notasMaximasMinima != null && notaLocal.HasValue)
				{
					return notasMaximasMinima.getNota_VAN_BC(notaLocal.Value);
				}
				else
				{
					return null;
				}

			}
		}

	}
 
	public class oNotaMaximaMinima
	{
		public double valorMaximo { get; set; }
		public double valorMinimo { get; set; }


		public oNotaMaximaMinima()
		{

		}

		public oNotaMaximaMinima(double iValorMax, double iValorMin)
		{
			valorMaximo = iValorMax;
			valorMinimo = iValorMin;
		}


		public double getNota_ValorMaximoCalificaCero(double iValor)
		{


			if (valorMaximo == valorMinimo)
			{
				return 0;
			}
			
		   
			double miNotaGlobal_0to10 = 10 * ((valorMaximo - iValor) / (valorMaximo - valorMinimo));

			if (miNotaGlobal_0to10.isBetweenMinMaxInclude(0, 10))
			{
				return miNotaGlobal_0to10;
			}
			else
			{
				throw new Exception(string.Format("El valor de {0}\n Debe de estar Comprendido entre\n" +
											   "Valor Minimo {1}\n Valor Máximo {2}", iValor.ToString(), valorMinimo.ToString(), valorMaximo.ToString()));
			}
		}
		public double getNota_ValorMaximoCalificaDiez(double iValor)
		{

			if (valorMaximo == valorMinimo)
			{
				return 0;
			}


			double miNotaGlobal_0to10 = 10 * ((iValor - valorMinimo) / (valorMaximo - valorMinimo));

			if (miNotaGlobal_0to10.isBetweenMinMaxInclude(0, 10))
			{
				return miNotaGlobal_0to10;
			}
			else
			{
				throw new Exception(string.Format("El valor de {0}\n Debe de estar Comprendido entre\n" +
											   "Valor Minimo {1}\n Valor Máximo {2}", iValor.ToString(), valorMinimo.ToString(), valorMaximo.ToString()));
			}
		}



		public double getNota_Capitulo(double iValor)
		{

			if (this.valorMinimo == 0)
			{
				return iValor;
			}
			else
			{
				double miNotaCapitulo_0to10 = 10 * ((iValor - valorMinimo) / (valorMinimo));

				if (miNotaCapitulo_0to10 > 10)
				{
					return 10;
				}
				else if (miNotaCapitulo_0to10 >= 0)
				{
					return miNotaCapitulo_0to10;
				}
				else
				{

					throw new Exception(string.Format("El valor de {0}\n Debe de estar Comprendido entre\n" +
													   "Valor Minimo {1}\n Valor Máximo {2}", iValor.ToString(), valorMinimo.ToString(), valorMaximo.ToString()));
				}

			}

		}
		public double getNota_VAN_BC (double iValor)
		{


			if (iValor < 0)
			{
				return 10;
			}

			if (this.valorMaximo == 0)
			{
				return 0;
			}

			double miNotaCapituloBaseCero = 10 * (-iValor + this.valorMaximo) / (this.valorMaximo);

			if (miNotaCapituloBaseCero > 10)
			{
				return 10;
			}
			else if (miNotaCapituloBaseCero >= 0)
			{
				return miNotaCapituloBaseCero;
			}
			else
			{
				throw new Exception(string.Format("El valor de {0}\n Debe de estar Comprendido entre\n" +
											  "   Valor Minimo {1}\n Valor Máximo {2}", iValor.ToString(), valorMinimo.ToString(), valorMaximo.ToString()));
			}


		}


		public double getNota_Muro(double iValor)
		{

			if (this.valorMinimo == this.valorMaximo && this.valorMaximo== iValor)
			{
				return 0;
			}
			else if (this.valorMinimo == this.valorMaximo && this.valorMaximo != iValor)
			{

				throw new Exception(string.Format("Error Valoración de Muro\n -Valor Mi de {0}\n  -Valor Máximo {1}\n -Valor Mínimo {2}",
												   iValor.ToString(), valorMinimo.ToString(), valorMaximo.ToString()));
			}
			else
			{
				double miNotaCapitulo_0to10 = 10 * ((iValor - valorMinimo) / (this.valorMaximo - this.valorMinimo));

				if (miNotaCapitulo_0to10 > 10)
				{
					return 10;
				}
				else if (miNotaCapitulo_0to10 >= 0)
				{
					return miNotaCapitulo_0to10;
				}
				else
				{

					throw new Exception(string.Format("El valor de {0}\n Debe de estar Comprendido entre\n" +
													   "Valor Minimo {1}\n Valor Máximo {2}", iValor.ToString(), valorMinimo.ToString(), valorMaximo.ToString()));
				}

			}

		}

		public override string ToString()
		{
			return string.Format("Valor Máximo : {0} | Valor Mínimo : {1} ", valorMaximo.ToString(), valorMinimo.ToString());
		}

	}








}

