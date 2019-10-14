using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.Extension.Double
{
	public static class oExtensionDouble
	{

		/// <summary>
		/// Numero es Divisible por
		/// </summary>
		/// <param name="iNumber"></param>
		public static bool isDivisible (this double iNumber, double iDenominador)
		{   
		   return (iNumber % iDenominador) == 0;
		}



		/// <summary>
		/// Dividir 2 Numeros !!Ojo Daba Problemas al dividir 2 enteros 1/9 = 0
		/// </summary>
		public static double getDivision (double iNumerador, double iDenominador)
		{
			return (double)iNumerador / iDenominador;
		}

		public static double getMayor(this double iValor, double iValorToCompare)
		{
			if (iValor >= iValorToCompare)
			{
				return iValor;
			}
			else
			{
				return iValorToCompare;
			}

		}

		public static double? ConvertToNullableDouble(this object iValor)
		{
			if (iValor == null || iValor == DBNull.Value)
			{
				return null;
			}
			else
			{
				return (double)iValor;
			}
		}



		public static double roundOffTen(this double i)
		{
			return (Math.Round(i / 10.0) * 10);
		}

		/// <summary>
		/// Redonde Cero Decimales
		/// </summary>
		public static double roundOffOne(this double i)
		{
			return Math.Round(i, 0);
		}

		public static double roundOff(this double i, int iDecimales)
		{
			return Math.Round(i, iDecimales);

		}



		public static int toEntero (this double i)
		{

			return Convert.ToInt32(i);


		}

		public static int roundEntero(this double i)
		{
			return Convert.ToInt32(Math.Round(i, 0));
		}

        public static double roundOffTenHighest(this double i)
        {
            double j = i + 0.1;

            return (Math.Ceiling(j / 10) * 10);
        }
	}
}
