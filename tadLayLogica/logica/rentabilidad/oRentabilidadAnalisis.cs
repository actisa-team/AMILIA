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
    using tadLayLan.Tdi;
    using engNet.CustomAtributos;




	public abstract class oRentabilidadAnalisis <T>: oRentabilidadCore  where T : oRentabilidadRowModel
					
	{

		private Dictionary<int, T> mDicRentabilidad =null;

		#region "Constructores"

		public oRentabilidadAnalisis(Guid iIdSol)
			: base(iIdSol)
		{

		}

		#endregion
		#region "Propiedades"

	 public Dictionary<int,T> rentabilidad
		{
			   get
			   {
				   if ( mDicRentabilidad==null)
				   {
					  mDicRentabilidad = createRentabilidad();
				   }

				   return mDicRentabilidad;

			   }
		}
		   
	  /// <summary>
	  /// VAN
	  /// </summary>
     [LocalizedDisplayName("uiRentValActualNeto", typeof(strFrmInformes))]
	  public double van
	   {
		   get
		   {
			 return  rentabilidad.Sum(row => row.Value.balanceTotalActualizado);             
		   }
	   }
	  /// <summary>
	  /// B/C
	  /// </summary>
     [LocalizedDisplayName("uiRentBC", typeof(strFrmInformes))]
	  public double balanceBeneficiosCostesActualizados
	  {
		  get
		  {
			  return Math.Abs(sumBeneficiosTotalesActualizados / sumCostesTotalesActualizados);
		  }
	  }
	  /// <summary>
	  /// TIR
	  /// </summary>
     [LocalizedDisplayName("uiRentTasaIntRet", typeof(strFrmInformes))]
	  public double? tir
	  {
		  get
		  {
              try
              {
                  double[] miValores = rentabilidad.Select(row => row.Value.balanceTotalIPC).ToArray();

                  return Financial.IRR(ref miValores);
              }
              catch (Exception)
              {

                  return null;
              }
			 
		  }
	  }
	  /// <summary>
	  /// PRI
	  /// </summary>
     [LocalizedDisplayName("uiRentPeriodoRecup", typeof(strFrmInformes))]
	  public int? pri
	  {
		  get
		  {
			  double miBalanceActualizadoAcumulado = 0;
			  int i = 1;

			  for (i = 1; i <= yearsTotal; i++)
			  {
				  miBalanceActualizadoAcumulado = miBalanceActualizadoAcumulado + rentabilidad[i].balanceTotalActualizado;

				  if (miBalanceActualizadoAcumulado > 0)
				  {
					  break;
				  }

			  }

              if (i > yearsTotal)
              {
                  return null;
              }
              else
              {
                  return i;
              }

			 

		  }
	  }
	  /// <summary>
	  /// Costes Totales Actualizados
	  /// </summary>
     [LocalizedDisplayName("uiRentCosTotActual", typeof(strFrmInformes))]
	  public double sumCostesTotalesActualizados
	   {
		   get
		   {
			   return rentabilidad.Sum(row => row.Value.costesTotalActualizados);
		   }
	   }
	  /// <summary>
	  /// Sumatorio Balance Beneficios
	  /// </summary>
     [LocalizedDisplayName("uiRentIngTotActual", typeof(strFrmInformes))]
	  public double sumBeneficiosTotalesActualizados
	   { 
		  get
		   {
			   return rentabilidad.Sum(row => row.Value.beneficiosTotalActualizados);
		   }
	   }

	   #endregion
		#region "Metodos Publicos"
	  public abstract void print(string iFileConExtension);
	  #endregion
		#region "Metodos Privados"
		protected abstract Dictionary<int,T> createRentabilidad ();
		#endregion



	}

}
