using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using tadLayShare.puntos;

namespace Terrenos
{
    public class Terreno
    {
        #region "Variables privadas"


        private List<Punto3d> mLstPuntos = new List<Punto3d>();

        private Punto3d mMinX;
        private Punto3d mMaxX;
        private Punto3d mMinY;
        private Punto3d mMaxY;
        
        #endregion
        
        #region "Constructores"

            public Terreno(List<Punto3d> miLstPuntos)
            {
                
                //foreach (Punto3d miPunto in miLstPuntos)
                //{
                //    if (!mLstPuntos.Contains(miPunto)) mLstPuntos.Add(miPunto);
                    
                //}
                mLstPuntos = miLstPuntos;

            }



        #endregion

        #region "Metodos publicos"

            public List<Punto3d> getPuntosTerreno
            {
                get { return mLstPuntos; }
            }

            public Punto3d[] limitesTerreno
            {
                get
                {
                    Punto3d[] miLimiteTerreno = new Punto3d[4];
                    var miSortedX = from miPunto in mLstPuntos orderby miPunto.coordenadaX select miPunto;
                    var miSortedY = from miPunto in mLstPuntos orderby miPunto.coordenadaY select miPunto;

                    mMinX = miSortedX.First<Punto3d>();
                    mMaxX = miSortedX.Last<Punto3d>();
                    mMinY = miSortedY.First<Punto3d>();
                    mMaxY = miSortedY.Last<Punto3d>();

                    miLimiteTerreno[0] = new Punto3d(mMinX.coordenadaX, mMinY.coordenadaY, 0);
                    miLimiteTerreno[1] = new Punto3d(mMaxX.coordenadaX, mMinY.coordenadaY, 0);
                    miLimiteTerreno[2] = new Punto3d(mMinX.coordenadaX, mMaxY.coordenadaY, 0);
                    miLimiteTerreno[3] = new Punto3d(mMaxX.coordenadaX, mMaxY.coordenadaY, 0);



                    return miLimiteTerreno;
                
                }
            }

            public Punto3d centroTerreno
            {
                get
                {
                    Punto3d[] miLimiteTerreno = new Punto3d[4];
                    var miSortedX = from miPunto in mLstPuntos orderby miPunto.coordenadaX select miPunto;
                    var miSortedY = from miPunto in mLstPuntos orderby miPunto.coordenadaY select miPunto;

                    mMinX = miSortedX.First<Punto3d>();
                    mMaxX = miSortedX.Last<Punto3d>();
                    mMinY = miSortedY.First<Punto3d>();
                    mMaxY = miSortedY.Last<Punto3d>();

                    miLimiteTerreno[0] = new Punto3d(mMinX.coordenadaX, mMinY.coordenadaY, 0);
                    miLimiteTerreno[1] = new Punto3d(mMaxX.coordenadaX, mMinY.coordenadaY, 0);
                    miLimiteTerreno[2] = new Punto3d(mMinX.coordenadaX, mMaxY.coordenadaY, 0);
                    miLimiteTerreno[3] = new Punto3d(mMaxX.coordenadaX, mMaxY.coordenadaY, 0);

                    double mitadX = mMinX.coordenadaX + ((mMaxX.coordenadaX - mMinX.coordenadaX) / 2);
                    double mitadY = mMinY.coordenadaY + ((mMaxY.coordenadaY - mMinY.coordenadaY) / 2);

                    return new Punto3d(mitadX, mitadY, 0);

                }
            }

            public bool isInside(double iX, double iY)
            {
                bool isInside = false;
                if (mMinX == null)
                {
                    Punto3d[] misLimites = this.limitesTerreno;
                }
                if ((iX >= mMinX.coordenadaX) && (iX <= mMaxX.coordenadaX))
                {
                    if ((iY >= mMinY.coordenadaY) && (iY <= mMaxY.coordenadaY))
                    {
                        isInside = true;
                    }
                }
                return isInside;
            }

        
        
        #endregion

        public static Stream getTerrenoDemo(int cartoIndex)
        {
            Assembly _assembly = Assembly.GetExecutingAssembly();
            var names = _assembly.GetManifestResourceNames();
            if (cartoIndex == 0)
            {
                return _assembly.GetManifestResourceStream("Terrenos.mdtsDEMO.Cartografia1.trr");
            }

            if (cartoIndex == 1)
            {
                return _assembly.GetManifestResourceStream("Terrenos.mdtsDEMO.Cartografia2.trr");
            }

            return null;
        }



    }
}
