using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare;

namespace Terrenos
{
     [Serializable]
    public class InfoTrianguloNew
    {
        string mIDtriangulo = "";

        public int mAdyacente1 = -1;

        public int mAdyacente2 = -1;

        public int mAdyacente3 = -1;

        double mVerticeAX;
        double mVerticeAY;
        double mVerticeAZ;

        double mVerticeBX;
        double mVerticeBY;
        double mVerticeBZ;

        double mVerticeCX;
        double mVerticeCY;
        double mVerticeCZ;



        private int mIndex;


        private int mIndexVA;
        private int mIndexVB;
        private int mIndexVC;


        public int getIndex
        {
            get
            {
                return mIndex;
            }
        }

        public int getIndexVA
        {
            get
            {
                return mIndexVA;
            }
        }

        public int getIndexVB
        {
            get
            {
                return mIndexVB;
            }
        }

        public int getIndexVC
        {
            get
            {
                return mIndexVC;
            }
        }

        public double getVertCZ
        {
            get
            {
                return mVerticeCZ;
            }
        }



        public double getVertCY
        {
            get
            {
                return mVerticeCY;
            }
        }

        public double getVertCX
        {
            get
            {
                return mVerticeCX;
            }
        }

        public double getVertBZ
        {
            get
            {
                return mVerticeBZ;
            }
        }

        public double getVertBY
        {
            get
            {
                return mVerticeBY;
            }
        }


        public double getVertBX
        {
            get
            {
                return mVerticeBX;
            }
        }

        public double getVertAZ
        {
            get
            {
                return mVerticeAZ;
            }
        }
        public double getVertAY
        {
            get
            {
                return mVerticeAY;
            }
        }

        public double getVertAX
        {
            get
            {
                return mVerticeAX;
            }
        }


        public int getAd3
        {
            get
            {
                return mAdyacente3;
            }
        }

        public int getAd2
        {
            get
            {
                return mAdyacente2;
            }
        }

        public int getAd1
        {
            get
            {
                return mAdyacente1;
            }
        }


        public string getID
        {
            get
            {
                return mIDtriangulo;
            }
        }

         public InfoTrianguloNew(InfoTriangulo info, int indexVa, int indexVb, int indexVc, int index)
         {

             mVerticeAX = info.getVertAX;
             mVerticeAY = info.getVertAY;
             mVerticeAZ = info.getVertAZ;

             mVerticeBX = info.getVertBX;
             mVerticeBY = info.getVertBY;
             mVerticeBZ = info.getVertBZ;

             mVerticeCX = info.getVertCX;
             mVerticeCY =  info.getVertCY;
             mVerticeCZ = info.getVertCZ;

             mIndex = index;
             mIndexVA = indexVa;
             mIndexVB = indexVb;
             mIndexVC = indexVc;
         }


        public InfoTrianguloNew(Terrenos.Triangulacion miTriangulacion, Triangulo iTriangulo)
        {
            mIDtriangulo = iTriangulo.getHashCode;
            var misAdyacencias = miTriangulacion.getAdyacentes;
            var mi3Adyacentes = misAdyacencias[iTriangulo.getIndex];
            for (int i = 0; i < mi3Adyacentes.Count; i++)
            {
                if (i == 0)
                {

                    mAdyacente1 = mi3Adyacentes[i];
                }
                if (i == 1)
                {
                    mAdyacente2  = mi3Adyacentes[i];

                }
                if (i == 2)
                {
                    mAdyacente3 = mi3Adyacentes[i];
                }
            }
            mVerticeAX = iTriangulo.getVerticeA.coordenadaX;
            mVerticeAY = iTriangulo.getVerticeA.coordenadaY;
            mVerticeAZ = iTriangulo.getVerticeA.coordenadaZ;

            mVerticeBX = iTriangulo.getVerticeB.coordenadaX;
            mVerticeBY = iTriangulo.getVerticeB.coordenadaY;
            mVerticeBZ = iTriangulo.getVerticeB.coordenadaZ;

            mVerticeCX = iTriangulo.getVerticeC.coordenadaX;
            mVerticeCY = iTriangulo.getVerticeC.coordenadaY;
            mVerticeCZ = iTriangulo.getVerticeC.coordenadaZ;

            mIndex = iTriangulo.getIndex;
            mIndexVA = iTriangulo.getVerticeA.index;
            mIndexVB = iTriangulo.getVerticeB.index;
            mIndexVC = iTriangulo.getVerticeC.index;


        }
        public InfoTrianguloNew(Terrenos.Triangulacion_old miTriangulacion, Triangulo iTriangulo)
        {
            mIDtriangulo = iTriangulo.getHashCode;
            var misAdyacencias = miTriangulacion.getAdyacentes;
            var mi3Adyacentes = misAdyacencias[iTriangulo.getIndex];
            for (int i = 0; i < mi3Adyacentes.Count; i++)
            {
                if (i == 0)
                {

                    mAdyacente1 = mi3Adyacentes[i];
                }
                if (i == 1)
                {
                    mAdyacente2 = mi3Adyacentes[i];

                }
                if (i == 2)
                {
                    mAdyacente3 = mi3Adyacentes[i];
                }
            }
            mVerticeAX = iTriangulo.getVerticeA.coordenadaX;
            mVerticeAY = iTriangulo.getVerticeA.coordenadaY;
            mVerticeAZ = iTriangulo.getVerticeA.coordenadaZ;

            mVerticeBX = iTriangulo.getVerticeB.coordenadaX;
            mVerticeBY = iTriangulo.getVerticeB.coordenadaY;
            mVerticeBZ = iTriangulo.getVerticeB.coordenadaZ;

            mVerticeCX = iTriangulo.getVerticeC.coordenadaX;
            mVerticeCY = iTriangulo.getVerticeC.coordenadaY;
            mVerticeCZ = iTriangulo.getVerticeC.coordenadaZ;

            mIndex = iTriangulo.getIndex;
            mIndexVA = iTriangulo.getVerticeA.index;
            mIndexVB = iTriangulo.getVerticeB.index;
            mIndexVC = iTriangulo.getVerticeC.index;


        }
       

    }
}
