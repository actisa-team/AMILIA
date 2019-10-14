using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.math.rangos
{
  public class oMathRangos
    {

        /// <param name="iValue">Valor a Comparar</param>
        /// <param name="iValueMin">Valor Minimo</param>
        /// <param name="iValueMax">Vamor Maximo</param>
        /// <returns>Devuelve Valor Comprendido entre Min y Max</returns>
        public static double getValueFromMinAndMax(double iValue, double iValueMin, double iValueMax)
        {
            if (iValue >= iValueMax)
            {
                return iValueMax;
            }
            else if (iValue <= iValueMin)
            {
                return iValueMin;
            }
            else
            {
                return iValue;
            }
        }

    }
}
