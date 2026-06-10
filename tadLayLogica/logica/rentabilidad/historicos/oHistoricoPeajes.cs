using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
  public  class oHistoricoPeajes
	{

	  private bool? mIsInversionPrivada = null;
	  private double? mCostePeajeVehiculo = null;
	  private int? mYearsExplotacion = null;
	  private Func<int, int> mTraficoHistorico = null;

	  Dictionary<int, double> mLstCostes = null;

	  public oHistoricoPeajes(bool iIsInversionPrivada,    
							  double iCostePeajeVehiculo,
							  int iYearsExplotacion,
							  Func<int,int> iTraficoHistorico)
	  {

		  mIsInversionPrivada = iIsInversionPrivada;
		  mCostePeajeVehiculo = iCostePeajeVehiculo;
		  mYearsExplotacion = iYearsExplotacion;
		  mTraficoHistorico = iTraficoHistorico;
	  }


	  public Dictionary<int,double> lstCoste
	  {

		  get
		  {

			  if (mLstCostes == null)
			  {
				  mLstCostes = getCostes();
			  }

			  return mLstCostes;
		  }

	  }


	  private Dictionary<int,double> getCostes()
	  {

		  Dictionary<int, double> miLstCostes = new Dictionary<int, double>();

		  double? miTraficoAnual = null;

		  if (mIsInversionPrivada.Value)
		  {
			  for (int i = 1 ; i <= mYearsExplotacion.Value; i++)
			  {  
				  miTraficoAnual = mTraficoHistorico(i);
				  miLstCostes.Add(i,miTraficoAnual.Value * mCostePeajeVehiculo.Value);
				  miTraficoAnual=null;
			  }
		  }
		  else
		  {
			  for (int i = 1; i <= mYearsExplotacion.Value ; i++)
			  {
				  miLstCostes.Add(i, 0);
			  }
		  }


		  return miLstCostes;

	  }

	}
}
