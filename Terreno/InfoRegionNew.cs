using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terrenos
{
    [Serializable]
    public class InfoRegionNew
    {
        string mIDregion = "";
        string mIDpadre = "";
        double mMaxX;
        double mMaxY;
        double mMinX;
        double mMinY;
        int mTriangulo;

        public InfoRegionNew(InfoRegion info, int triangIndex)
        {
            mIDregion = info.getIDregion;
            mIDpadre = info.getIDpadre;
            mMaxX = info.getMaxX;
            mMaxY = info.getMaxY;
            mMinX = info.getMinX;
            mMinY = info.getMinY;
            mTriangulo = triangIndex;
        }

        public InfoRegionNew(Region iRegion)
        {
            mIDregion = iRegion.getHashCode;
            if (iRegion.TrianguloCentral != null)
                mTriangulo = iRegion.TrianguloCentral.getIndex;
            if (iRegion.getPadre != null) mIDpadre = iRegion.getPadre.getHashCode;
            mMaxX = iRegion.getMaxX;
            mMaxY = iRegion.getMaxY;
            mMinX = iRegion.getMinX;
            mMinY = iRegion.getMinY;

        }
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

        public int getTriangulo
        {
            get
            {
                return mTriangulo;
            }
        }

    }
    
}
