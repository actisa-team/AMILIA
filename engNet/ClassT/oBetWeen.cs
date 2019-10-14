using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.ClassT
{

    /// <summary>
    /// an extension class for the between operation
    /// name pattern IsBetweenXX where X = I -> Inclusive, X = E -> Exclusive
    /// </summary>
    public static class xBetweenExtensions
    {

        public static bool isBetweenMinMaxInclude<T>(this T value, T min, T max) where T : IComparable
        {
            return (min.CompareTo(value) <= 0) && (value.CompareTo(max) <= 0);
        }

        public static bool isBetweenMinMaxExclude<T>(this T value, T min, T max) where T : IComparable
        {
            return (min.CompareTo(value) < 0) && (value.CompareTo(max) < 0);
        }

        public static bool isBetweenMinExclude_MaxInclude<T>(this T value, T min, T max) where T : IComparable
        {
            return (min.CompareTo(value) < 0) && (value.CompareTo(max) <= 0);
        }

        public static bool isBetweenMinInclude_MaxExclude<T>(this T value, T min, T max) where T : IComparable
        {
            return (min.CompareTo(value) <= 0) && (value.CompareTo(max) < 0);
        }


    }

}
