using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Terrenos.triangulos;

namespace Terrenos
{
     [Serializable]
    public class InfoTriangulo
    {
        string mIDtriangulo = "";

        string mAdyacente1 = "";

        string mAdyacente2 = "";

        string mAdyacente3 = "";

        double mVerticeAX;
        double mVerticeAY;
        double mVerticeAZ;

        double mVerticeBX;
        double mVerticeBY;
        double mVerticeBZ;

        double mVerticeCX;
        double mVerticeCY;
        double mVerticeCZ;


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


        public string getAd3
        {
            get
            {
                return mAdyacente3;
            }
        }

        public string getAd2
        {
            get
            {
                return mAdyacente2;
            }
        }

        public string getAd1
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

        //public InfoTriangulo(Terrenos.Triangulacion miTriangulacion, Terrenos.triangulos.Triangulo iTriangulo)
        //{
        //    mIDtriangulo = iTriangulo.getHashCode;
        //    System.Collections.Hashtable misAdyacencias = miTriangulacion.getAdyacentes;
        //    List<string> mi3Adyacentes = misAdyacencias[iTriangulo.getHashCode] as List<string>;
        //    for (int i = 0; i < mi3Adyacentes.Count; i++)
        //    {
        //        if (i == 0)
        //        {

        //            mAdyacente1 = mi3Adyacentes[i];
        //        }
        //        if (i == 1)
        //        {
        //            mAdyacente2  = mi3Adyacentes[i];

        //        }
        //        if (i == 2)
        //        {
        //            mAdyacente3 = mi3Adyacentes[i];
        //        }
        //    }
        //    mVerticeAX = iTriangulo.getVerticeA.coordenadaX;
        //    mVerticeAY = iTriangulo.getVerticeA.coordenadaY;
        //    mVerticeAZ = iTriangulo.getVerticeA.coordenadaZ;

        //    mVerticeBX = iTriangulo.getVerticeB.coordenadaX;
        //    mVerticeBY = iTriangulo.getVerticeB.coordenadaY;
        //    mVerticeBZ = iTriangulo.getVerticeB.coordenadaZ;

        //    mVerticeCX = iTriangulo.getVerticeC.coordenadaX;
        //    mVerticeCY = iTriangulo.getVerticeC.coordenadaY;
        //    mVerticeCZ = iTriangulo.getVerticeC.coordenadaZ;


        //}
    }
}
