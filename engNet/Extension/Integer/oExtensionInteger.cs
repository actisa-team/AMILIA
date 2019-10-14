using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.Extension.Integer
{
	public static class oExtensionInteger
	{
        public static bool isDivisble(this int x, int n)
        {
            return (x % n) == 0;
        }
        public static int roundOff(this int i)
        {
            return ((int)Math.Round(i / 10.0)) * 10;
        }
        public static double toDouble(this int i)
        {
            return Convert.ToDouble(i);
        }
        public static int roundEnteroSuperior(this double i)
        {
            int mi = (int)Math.Truncate(i);

            return mi + 1;
        }

	}
}
