using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.rentabilidad
{
	using engNet.ClassT;
	
	
	
	public class oHistoricoExplotacionEmpleo
	{

		private bool? mIsInversionPrivada = null;
		private double? mEmpleoCosteAnual = null;
		private int? mYearsExplotacion = null;
		private Dictionary<int, double> mLstCostes = null;

		public oHistoricoExplotacionEmpleo(bool iIsInversionPrivada, 
										   int iEmpleadosNum,
										   double iEmpleadosCosteUnitario, 
										   int iYearsExplotacion)
													
		{

			mIsInversionPrivada = iIsInversionPrivada;
            mEmpleoCosteAnual = iEmpleadosNum * iEmpleadosCosteUnitario;
			mYearsExplotacion = iYearsExplotacion;

		}


		public Dictionary<int,double> lstCostes
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


		private Dictionary<int,double>  getLstCostes()
		{


			Dictionary<int, double> miLstCoste = new Dictionary<int, double>();

			//Existe Inversion Privada ; Gastos Computados
			if (mIsInversionPrivada.Value)
			{
				for (int i = 1; i <= mYearsExplotacion; i++)
				{
                    miLstCoste.Add(i, mEmpleoCosteAnual.Value);
				}
			}
			//Existe Inversion Privada (No se Computan los Gastos)
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
}
