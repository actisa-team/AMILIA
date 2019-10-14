using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.Extension.DataRow
{

    using System.Data;


    public static class oExtensionDataRow
    {

        public static double? convertDBnullToDouble (this DataRow iDataRow, int iColumnIndex)
        {

            if (iDataRow.IsNull(iColumnIndex))
            {
                return null;
            }
            else
            {
                return iDataRow[iColumnIndex] as double?;
            }

        }

    }
}
