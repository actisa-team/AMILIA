using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
	using engNet.ClassT;
	using engNet.Extension.Integer;



	public abstract class oHistoricoConservacionRehabilitacionRoad
	{

		protected   double?           mCosteMantenerAnual                 = null;
		protected   double?           mCosteRehabilitarAnual              = null;
		protected   int?              mYearsExplotacion                   = null;

		private Dictionary<int, double> mLstCostes = null;


	   #region "Constructores"
	   

		public oHistoricoConservacionRehabilitacionRoad()
		{

		}

		public oHistoricoConservacionRehabilitacionRoad (double iRoadLonKm,
														 double iCosteMantenerUmKm,
														 double iCosteRehabilitarUmKm,
														 int iYearsExplotacion)

		{
			mCosteMantenerAnual = iRoadLonKm * iCosteMantenerUmKm;
			mCosteRehabilitarAnual = iRoadLonKm * iCosteRehabilitarUmKm;
			mYearsExplotacion = iYearsExplotacion;
		}


	   #endregion


		public Dictionary<int, double> lstCostes
		{
			get
			{

				if (mLstCostes == null)
				{
					mLstCostes = getLstCostes();

				}

				return mLstCostes;
			}

		}


		#region "Metodos"

		/// <summary>
		/// Imprimir el Historico de IPC
		/// </summary>
		public void print(string iFilePath, string iFileName)
		{
			List<oValDesT<int, double>> miLst = new List<oValDesT<int, double>>();

			foreach (var item in lstCostes)
			{
				miLst.Add(new oValDesT<int, double>(item.Key, item.Value));
			}

			engNet.oCsvWrite.write<oValDesT<int, double>>(miLst, iFilePath, iFileName);
		}



		#endregion


		protected abstract Dictionary<int, double> getLstCostes();



	}

	public class oHistoricoConservacionRehabilitacionRoad0ParteEstatal : oHistoricoConservacionRehabilitacionRoad
	{


		public oHistoricoConservacionRehabilitacionRoad0ParteEstatal(double iRoadLonKm,
																	double iCosteMantenerUmKm,
																	double iCosteRehabilitarUmKm,
																	int iYearsExplotacion)
			:base(iRoadLonKm,iCosteMantenerUmKm,iCosteRehabilitarUmKm,iYearsExplotacion)

		{

		}


   


		protected override Dictionary<int, double> getLstCostes()
		{

			Dictionary<int, double> miLstCoste = new Dictionary<int, double>();


				for (int i = 1; i <= mYearsExplotacion; i++)
				{
					if (i == 1)
					{
					  miLstCoste.Add(i, mCosteRehabilitarAnual.Value);
					}  
					else if (i.isDivisble(10))
					{
						miLstCoste.Add(i, mCosteRehabilitarAnual.Value);
					}
					else
					{
						miLstCoste.Add(i, mCosteMantenerAnual.Value);
					}

				}
			

			return miLstCoste;

		}



	}
	public class oHistoricoConservacionRehabilitacionRoad11ParteEstatal : oHistoricoConservacionRehabilitacionRoad
	{

		private bool? mIsInversionPrivada = null;


		public oHistoricoConservacionRehabilitacionRoad11ParteEstatal(bool iIsInversionPrivada, 
																	  double iRoadLonKm,
																	  double iCosteMantenerUmKm, 
																	  double iCosteRehabilitarUmKm,
																	  int iYearsExplotacion)

			:base(iRoadLonKm,iCosteMantenerUmKm,iCosteRehabilitarUmKm,iYearsExplotacion)
													
		{
			mIsInversionPrivada = iIsInversionPrivada;
		}





		protected override  Dictionary<int,double>  getLstCostes()
		{


			Dictionary<int, double> miLstCoste = new Dictionary<int, double>();

			//Existe Inversion Privada ; Gastos Computados NO Computados
			if (mIsInversionPrivada.Value)
			{
				for (int i = 1; i <= mYearsExplotacion; i++)
				{
					miLstCoste.Add(i, 0);
				}

			}
			//No Existe Inversion Privada (SI se Computan los Gastos)
			else
			{
				for (int i = 1; i <= mYearsExplotacion; i++)
				{
					if (i.isDivisble(10))
					{
						miLstCoste.Add(i, mCosteRehabilitarAnual.Value);
					}
					else
					{
						miLstCoste.Add(i, mCosteMantenerAnual.Value);
					}
				}    
			}

			return miLstCoste;

		}



	}
	public class oHistoricoConservacionRehabilitacionRoad11PartePrivada : oHistoricoConservacionRehabilitacionRoad
	{

		private bool? mIsInversionPrivada = null;


		public oHistoricoConservacionRehabilitacionRoad11PartePrivada(bool iIsInversionPrivada,
																	  double iRoadLonKm,
																	  double iCosteMantenerUmKm,
																	  double iCosteRehabilitarUmKm,
																	  int iYearsExplotacion)

			: base(iRoadLonKm, iCosteMantenerUmKm, iCosteRehabilitarUmKm, iYearsExplotacion)
		{
			mIsInversionPrivada = iIsInversionPrivada;
		}





		protected override Dictionary<int, double> getLstCostes()
		{


			Dictionary<int, double> miLstCoste = new Dictionary<int, double>();

			//PartePrivada Gastos Imputables
			if (mIsInversionPrivada.Value)
			{

				for (int i = 1; i <= mYearsExplotacion; i++)
				{
					if (i.isDivisble(10))
					{
						miLstCoste.Add(i, mCosteRehabilitarAnual.Value);
					}
					else
					{
						miLstCoste.Add(i, mCosteMantenerAnual.Value);
					}
				}
							
			}
			//No Existe Inversion Privada (SI se Computan los Gastos)
			else
			{
				for (int i = 1; i <= mYearsExplotacion; i++)
				{
					miLstCoste.Add(i, 0);
				}
			}

			return miLstCoste;

		}



	}
	public class oHistoricoConservacionRehabilitacionCosteCero : oHistoricoConservacionRehabilitacionRoad
	{

		public oHistoricoConservacionRehabilitacionCosteCero(int iYearsExplotacion)
		{
			this.mYearsExplotacion = iYearsExplotacion;
		}


		protected override Dictionary<int, double> getLstCostes()
		{
			Dictionary<int, double> miLstCoste = new Dictionary<int, double>();

			for (int i = 1; i <= mYearsExplotacion; i++)
			{                     
			  miLstCoste.Add(i,0);
				   
			}
			return miLstCoste;
		}


	}


}
