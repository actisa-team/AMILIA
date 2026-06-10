using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{

	using engNet.Extension.Collection;
	using System.Collections.ObjectModel;
	using tadLayShare.puntos;
    using tadLayLan;


	/// <summary>
	/// Colección de Tramos por Abanico
	/// </summary>
	public class oTramoAbanicoCollection : Collection<oTramoAbanico>
	{
		public oTramoAbanicoCollection()
		{

		}


		




		#region "Metodos Públicos"

        //public double getValoracionTramoAt(int index)
        //{
        //    return this[index].valoracionPonderadaGlobal_0_10;
        //}


		/// <summary>
		/// Listado con los Tramos Ganadores de la Coleccion
		/// </summary>
		public List<oTramoEjeBasico> getTramosGanadores()
		{

			var miQuery = from p in this
						  where p.isTramoValido
						  orderby p.valoracionPonderadaGlobal_0_10 descending
						  select p;


			oTramoAbanico miTramoAbanicoWin = miQuery.First();

			List<oTramoEjeBasico> miLstTramosEjesBasicosWin = miTramoAbanicoWin.lstTramos.ConvertAll(p => (oTramoEjeBasico)p);

			return miLstTramosEjesBasicosWin;

		}


		/// <summary>
		/// ID Máximo de Tramo de la Colección
		/// </summary>
		public int getIdTramoMaximo()
		{

			int miIdTramoMaximo = (from p in this select p.idTramo).Max();


			return miIdTramoMaximo;


		}

		/// <summary>
		/// Existen Tramos Valdios en la Colección
		/// </summary>
		public bool existenTramosValidos()
		{
			var miQuery = from p in this
						  where p.isTramoValido
						  select p;

			if (miQuery == null || miQuery.Count() == 0)
			{
				return false;
			}
			else
			{
				return true;
			}

		}

		/// <summary>
		/// Existen Tramos Validos en los Abancos Cortos y Largos
		/// </summary>
		public bool existenTramosValidosAvanceCortosLargos()
		{

			var miQueryCortos = from p in this
								where p.isTramoValido & p.tramoTipoEjeBasico == eTramoTipoEjeBasico.avanceCorto
								select p;

			int miCountAvanceCortos = miQueryCortos.Count();


			var miQueryLargos = from p in this
								where p.isTramoValido & p.tramoTipoEjeBasico == eTramoTipoEjeBasico.avanceLargo
								select p;

			int miCountAvanceLargos = miQueryLargos.Count();


			if (miCountAvanceCortos > 0 | miCountAvanceLargos > 0)
			{
				return true;
			}
			else if (miCountAvanceCortos == 0 && miCountAvanceLargos == 0)
			{
				return false;
			}
			else
			{
                throw new Exception(strError.eTramosValidos);
			}

		}

		/// <summary>
		/// Valoramos el Abanico en función de la Ponderación de Notas de oTramosValoracion
		/// </summary>
		public void createValoracion(oTramosValoracion iValoracionesGlobales)
		{


			oValoracionLocalToGlobal miValoracionDis = this.valoracionDistancia();
			oValoracionLocalToGlobal miValoracionPendiente = this.valoracionSlope();
			oValoracionLocalToGlobal miValoracionCoste = this.valoracionCoste();


			//Valoracion Globales ; Comparando entre Abanicos
			foreach (var item in this)
			{
				if (item.isTramoValido)
				{
					item.valoracionDistanciaGlobal_0_10 = miValoracionDis.getNota_ValorMaximoCalifica_0(item.valoracionDistanciaLocal());
					item.valoracionPendienteGlobal_0_10 = miValoracionPendiente.getNota_ValorMaximoCalifica_10(item.valoracionPendienteML());
					item.valoracionCosteImplantacionGlobal_0_10 = miValoracionCoste.getNota_ValorMaximoCalifica_0(item.valoracionCosteImplantacionML());
				}
			}



			//Obtener La ValoracionPonderada
			foreach (var item in this)
			{
				if (item.isTramoValido)
				{

					item.valoracionPonderadaGlobal_0_10 = iValoracionesGlobales.getValoracionGlobalPonderada(item.valoracionDistanciaGlobal_0_10,
																											 item.valoracionPendienteGlobal_0_10,
																											 item.valoracionCosteImplantacionGlobal_0_10);
				}
			}

		}


		/// <summary>
		/// Obtener Lista de Tramos Validos del último Abanico de la Colección Ordenados por Nota (Descendente)
		/// </summary>
		/// <remarks>El Primero es el mejor Valorado</remarks>
		public List<oTramoAbanico> getLstTramosValidosAndSortByNotaDescendente ()
		{

			if (this == null || this.Count == 0)
			{
                throw new Exception(strError.eColeccionTramosNula);
			}


			int miIdTramoMaximo = this.getIdTramoMaximo();

			var miQuery = from p in this
						  where p.isTramoValido && p.idTramo==miIdTramoMaximo
						  orderby p.valoracionPonderadaGlobal_0_10 descending
						  select p;


			return miQuery.ToList();

		}
	 




		#region "Metodos Privados"


		/// <summary>
		/// Valoración Por Distancia
		/// </summary>
		private oValoracionLocalToGlobal valoracionDistancia()
		{
			double miCosteMimino = (from p in this where p.isTramoValido select p.valoracionDistanciaLocal()).Min();
			double miCosteMaximo = (from p in this where p.isTramoValido select p.valoracionDistanciaLocal()).Max();


			oValoracionLocalToGlobal miValoracion = new oValoracionLocalToGlobal(miCosteMimino, miCosteMaximo);

			return miValoracion;
		}

		/// <summary>
		/// Valoracion Por Pendiente Terreno
		/// </summary>
		private oValoracionLocalToGlobal valoracionSlope()
		{
			double miCosteMimino = (from p in this where p.isTramoValido select p.valoracionPendienteML()).Min();
			double miCosteMaximo = (from p in this where p.isTramoValido select p.valoracionPendienteML()).Max();


			oValoracionLocalToGlobal miValoracion = new oValoracionLocalToGlobal(miCosteMimino, miCosteMaximo);

			return miValoracion;
		}

		/// <summary>
		/// Valoracion por Coste Implantación
		/// </summary>
		private oValoracionLocalToGlobal valoracionCoste()
		{
			double miCosteMimino = (from p in this where p.isTramoValido select p.valoracionCosteImplantacionML()).Min();
			double miCosteMaximo = (from p in this where p.isTramoValido select p.valoracionCosteImplantacionML()).Max();

			oValoracionLocalToGlobal miValoracion = new oValoracionLocalToGlobal(miCosteMimino, miCosteMaximo);

			return miValoracion;
		}



		#endregion










	  



		#endregion

	}


	public class oValoracionLocalToGlobal
	{
	
		public double valorGlobalMinimoAbs { get; private set; }
		public double valorGlobalMaximoAbs { get; private set; }

		public oValoracionLocalToGlobal (double iValorMinimo, double iValorMaximo)
		{
			this.valorGlobalMinimoAbs = Math.Round(Math.Abs(iValorMinimo),3);
			this.valorGlobalMaximoAbs = Math.Round( Math.Abs(iValorMaximo),3);
		}


		/// <summary>
		/// Valoración de las Distancia y Coste
		/// (Mayor Distancia Califica Cero
		/// </summary>
		public double getNota_ValorMaximoCalifica_0 (double iValoracionLocal)
		{

			 iValoracionLocal = Math.Round(iValoracionLocal, 3);


			if (iValoracionLocal == valorGlobalMaximoAbs)
			{
				return 0;
			}

			if (iValoracionLocal == valorGlobalMinimoAbs)
			{

				return 10;
			}


			double miNotaGlobal_0to10 = 10 * ((this.valorGlobalMaximoAbs - iValoracionLocal) / (this.valorGlobalMaximoAbs - this.valorGlobalMinimoAbs));


			if (miNotaGlobal_0to10 > 10 | miNotaGlobal_0to10 < 0)
			{

                throw new Exception(string.Format(strError.eValSeccion, this.valorGlobalMaximoAbs, this.valorGlobalMinimoAbs, iValoracionLocal));
			   
			}
			else
			{

				return miNotaGlobal_0to10;
			}

		}


		/// <summary>
		/// Valoración de la Pendiente
		/// Pendiente Valor 10 --> Terrenos Planos
		/// </summary>
		public double getNota_ValorMaximoCalifica_10(double iValoracionLocal)
		{

			 iValoracionLocal = Math.Round(iValoracionLocal, 3);


			if (iValoracionLocal == valorGlobalMaximoAbs)
			{
				return 10;
			}

			if (iValoracionLocal == valorGlobalMinimoAbs)
			{

				return 0;
			}


			double miNotaGlobal_0to10 = 10 * ((iValoracionLocal - this.valorGlobalMinimoAbs) / (this.valorGlobalMaximoAbs - this.valorGlobalMinimoAbs));


			if (miNotaGlobal_0to10 > 10 | miNotaGlobal_0to10 < 0)
			{

                throw new Exception(string.Format(strError.eValSeccion, this.valorGlobalMaximoAbs, this.valorGlobalMinimoAbs, iValoracionLocal));
			   
			}
			else
			{

				return miNotaGlobal_0to10;
			}

		}


	

	}

  
}
