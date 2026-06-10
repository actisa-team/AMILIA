using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terrenos
{
    [Serializable]
    public class InfoRegion
    {
        string mIDregion = "";
        string mIDpadre = "";
        double mMaxX;
        double mMaxY;
        double mMinX;
        double mMinY;
        string mTriangulo;

        //public InfoRegion(Terrenos.Region iRegion)
        //{
        //    mIDregion = iRegion.getHashCode;
        //    if (iRegion.getTrianguloCentral != null)
        //        mTriangulo = iRegion.getTrianguloCentral.getHashCode;
        //    if (iRegion.getPadre != null) mIDpadre = iRegion.getPadre.getHashCode;
        //    mMaxX = iRegion.getMaxX;
        //    mMaxY = iRegion.getMaxY;
        //    mMinX = iRegion.getMinX;
        //    mMinY = iRegion.getMinY;

        //}
        public string getIDregion
        {
            get
            {
                return mIDregion;
            }
        }
        public string getIDpadre
        {
            get
            {
                return mIDpadre;
            }
        }

        public double getMaxX
        {
            get
            {
                return mMaxX;
            }
        }
        public double getMaxY
        {
            get
            {
                return mMaxY;
            }
        }
        public double getMinX
        {
            get
            {
                return mMinX;
            }
        }
        public double getMinY
        {
            get
            {
                return mMinY;
            }
        }

        public string getTriangulo
        {
            get
            {
                return mTriangulo;
            }
        }

    }
    
}
