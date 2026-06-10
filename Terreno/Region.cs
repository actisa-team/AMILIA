using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using tadLayShare;
using tadLayShare.puntos;

namespace Terrenos
{
    public class Region
    {
        private double mMaxX;
        private double mMaxY;
        private double mMinX;
        private double mMinY;

        private Triangulo mTriangulo;

        private Region mPadre;

        private Region mHijoSD;
        private Region mHijoSI;
        private Region mHijoID;
        private Region mHijoII;

        string mEtiqueta = "";
        string mHermanoSD = "";
        string mHermanoSI = "";
        string mHermanoID = "";
        string mHermanoII = "";
        public int contNodos = 0;

        public List<Region> mHojas = new List<Region>();
        private Punto3d mPuntoCentro;
        


        public Region(double iMaximoX, double iMaximoY, double iMinimoX, double iMinimoY, ref List<Region> iHojas , List<Punto3d> iVertices, Region miPadre, string iEtiqueta, int numNodosXRegion = 500)
        {
            int miContNodos = 0;
            mMaxX = iMaximoX;
            mMaxY = iMaximoY;
            mMinX = iMinimoX;
            mMinY = iMinimoY;
            mPadre = miPadre;
            mEtiqueta = iEtiqueta;


            double miMitadX = mMaxX - ((mMaxX - mMinX) / 2);
            double miMitadY = mMaxY - ((mMaxY - mMinY) / 2);


            List<Punto3d> misVertices = new List<Punto3d>();


            foreach (Punto3d miPunto in iVertices)
            {
                if ((miPunto.coordenadaX <= mMaxX) && (miPunto.coordenadaX >= mMinX) && (miPunto.coordenadaY <= mMaxY) && (miPunto.coordenadaY >= mMinY))
                {
                    miContNodos++;
                    misVertices.Add(miPunto);
                }

            }
            contNodos = miContNodos;
            if (miContNodos >= numNodosXRegion && (mPadre!=null && contNodos < mPadre.contNodos) || (mPadre==null))
            {
                mHijoSD = new Region(mMaxX, mMaxY, miMitadX, miMitadY, ref iHojas, misVertices, this, "HijoSD");

                mHijoSI = new Region(miMitadX, mMaxY, mMinX, miMitadY, ref iHojas, misVertices, this, "HijoSI");


                mHijoID = new Region(mMaxX, miMitadY, miMitadX, mMinY, ref iHojas, misVertices, this, "HijoID");


                mHijoII = new Region(miMitadX, miMitadY, mMinX, mMinY, ref iHojas, misVertices, this, "HijoII");



            }
            else
            {
                iHojas.Add(this);
            }

            if (mHijoSD != null)
            {
                mHijoSD.setHermanoSI = mHijoSI.getHashCode;
                mHijoSD.setHermanoID = mHijoID.getHashCode;
                mHijoSD.setHermanoII = mHijoII.getHashCode;

                mHijoSI.setHermanoSD = mHijoSD.getHashCode;
                mHijoSI.setHermanoID = mHijoID.getHashCode;
                mHijoSI.setHermanoII = mHijoII.getHashCode;

                mHijoID.setHermanoSI = mHijoSI.getHashCode;
                mHijoID.setHermanoSD = mHijoSD.getHashCode;
                mHijoID.setHermanoII = mHijoII.getHashCode;

                mHijoII.setHermanoSI = mHijoSI.getHashCode;
                mHijoII.setHermanoID = mHijoID.getHashCode;
                mHijoII.setHermanoSD = mHijoSD.getHashCode;
            }
            mHojas = iHojas;
        }

        public Region(Region iHijoSD, Region iHijoSI, Region iHijoID, Region iHijoII)
        {
            mMaxX = iHijoSD.getMaxX;
            mMaxY = iHijoSD.getMaxY;
            mMinX = iHijoII.getMinX;
            mMinY = iHijoII.getMinY;

            mHijoSD = iHijoSD;
            mHijoSI = iHijoSI;
            mHijoID = iHijoID;
            mHijoII = iHijoII;

            mHijoSD.setPadre = this;
            mHijoSI.setPadre = this;
            mHijoID.setPadre = this;
            mHijoII.setPadre = this;

            mHojas.AddRange(mHijoSD.mHojas);
            mHojas.AddRange(mHijoSI.mHojas);
            mHojas.AddRange(mHijoID.mHojas);
            mHojas.AddRange(mHijoII.mHojas);


        }

        public Region(InfoRegionNew iInfoRegionNew, Triangulacion miTriangulacion)
        {
            mMaxX = iInfoRegionNew.getMaxX;
            mMaxY = iInfoRegionNew.getMaxY;
            mMinX = iInfoRegionNew.getMinX;
            mMinY = iInfoRegionNew.getMinY;

            mTriangulo = miTriangulacion.getTriangulosArray[iInfoRegionNew.getTriangulo];
            mHojas.Add(this);

        }
       

        public Region(InfoTriangulacionNew Info, Triangulacion miTriangulacion)
        {

            Hashtable miRecomponer = new Hashtable();
            foreach (InfoRegionNew miInfoRegion in Info.getRegiones)
            {
                Region miRegion = new Region(miInfoRegion, miTriangulacion);
                string[] misHermanos = Info.getInfoHermanos[miRegion.getHashCode] as string[];
                if (misHermanos != null)
                {
                    miRegion.setHermanoSD = misHermanos[0];
                    miRegion.setHermanoSI = misHermanos[1];
                    miRegion.setHermanoID = misHermanos[2];
                    miRegion.setHermanoII = misHermanos[3];
                }
                string miEtiqueta = Info.getInfoEtiquetas[miRegion.getHashCode] as string;
                miRegion.setEtiqueta = miEtiqueta;
                miRecomponer.Add(miRegion.getHashCode, miRegion);

            }
            int contador=0;
            IEnumerator miEnum = miRecomponer.Values.GetEnumerator();
            miEnum.MoveNext();
            while (miRecomponer.Count > 4 && miEnum.MoveNext())
            {
                Region miRegion = miEnum.Current as Region;
                string[] misHermanos = Info.getInfoHermanos[miRegion.getHashCode] as string[];
                contador++;
                string hermano1 = "";
                string hermano2 = "";
                string hermano3 = "";
                if (contador==5190318)
                {

                }
                if (miRegion.getEtiqueta == "HijoSD")
                {
                    hermano1 = miRegion.getHermanoSI;
                    hermano2 = miRegion.getHermanoID;
                    hermano3 = miRegion.getHermanoII;
                }
                else if (miRegion.getEtiqueta == "HijoSI")
                {
                    hermano1 = miRegion.getHermanoSD;
                    hermano2 = miRegion.getHermanoID;
                    hermano3 = miRegion.getHermanoII;
                }
                else if (miRegion.getEtiqueta == "HijoID")
                {
                    hermano1 = miRegion.getHermanoSI;
                    hermano2 = miRegion.getHermanoSD;
                    hermano3 = miRegion.getHermanoII;
                }
                else if (miRegion.getEtiqueta == "HijoII")
                {
                    hermano1 = miRegion.getHermanoSI;
                    hermano2 = miRegion.getHermanoID;
                    hermano3 = miRegion.getHermanoSD;
                }
                if ((miRecomponer.ContainsKey(hermano1)) && (miRecomponer.ContainsKey(hermano2)) && (miRecomponer.ContainsKey(hermano3)))
                {
                    if (miRecomponer.Count > 4)
                    {
                        Region miPadre = recomponerPadre(miRegion, Info, miRecomponer);
                        miRecomponer.Remove(hermano1);
                        miRecomponer.Remove(hermano2);
                        miRecomponer.Remove(hermano3);
                        miRecomponer.Remove(miRegion.getHashCode);
                        miRecomponer.Add(miPadre.getHashCode, miPadre);
                        miEnum = miRecomponer.Values.GetEnumerator();
                        miEnum.MoveNext();
                    }else
                    {

                    }
                    
                }
                else
                {
                    if (miRecomponer.Count > 4)
                    {
                        miEnum.MoveNext();
                    }
                    else
                    {

                    }
                }

            }
            foreach (Region miRegion in miRecomponer.Values)
            {
                if (miRegion.getEtiqueta == "HijoSD") mHijoSD = miRegion;
                if (miRegion.getEtiqueta == "HijoSI") mHijoSI = miRegion;
                if (miRegion.getEtiqueta == "HijoID") mHijoID = miRegion;
                if (miRegion.getEtiqueta == "HijoII") mHijoII = miRegion;
            }
            if (mHijoSD != null)
            {
                mMaxX = mHijoSD.getMaxX;
                mMaxY = mHijoSD.getMaxY;
            }
            if (mHijoII != null)
            {
                mMinX = mHijoII.getMinX;
                mMinY = mHijoII.getMinY;
            }

            mHojas.AddRange(mHijoSD.mHojas);
            mHojas.AddRange(mHijoSI.mHojas);
            mHojas.AddRange(mHijoID.mHojas);
            mHojas.AddRange(mHijoII.mHojas);

        }


        public Region(InfoRegionNew iInfoRegionNew, Triangulacion_old miTriangulacion)
        {
            mMaxX = iInfoRegionNew.getMaxX;
            mMaxY = iInfoRegionNew.getMaxY;
            mMinX = iInfoRegionNew.getMinX;
            mMinY = iInfoRegionNew.getMinY;

            mTriangulo = miTriangulacion.getTriangulosArray[iInfoRegionNew.getTriangulo];
            mHojas.Add(this);

        }


        public Region(InfoTriangulacionNew Info, Triangulacion_old miTriangulacion)
        {

            Hashtable miRecomponer = new Hashtable();
            foreach (InfoRegionNew miInfoRegion in Info.getRegiones)
            {
                Region miRegion = new Region(miInfoRegion, miTriangulacion);
                string[] misHermanos = Info.getInfoHermanos[miRegion.getHashCode] as string[];
                if (misHermanos != null)
                {
                    miRegion.setHermanoSD = misHermanos[0];
                    miRegion.setHermanoSI = misHermanos[1];
                    miRegion.setHermanoID = misHermanos[2];
                    miRegion.setHermanoII = misHermanos[3];
                }
                string miEtiqueta = Info.getInfoEtiquetas[miRegion.getHashCode] as string;
                miRegion.setEtiqueta = miEtiqueta;
                miRecomponer.Add(miRegion.getHashCode, miRegion);

            }
            int contador = 0;
            IEnumerator miEnum = miRecomponer.Values.GetEnumerator();
            miEnum.MoveNext();
            while (miRecomponer.Count > 4 && miEnum.MoveNext())
            {
                Region miRegion = miEnum.Current as Region;
                string[] misHermanos = Info.getInfoHermanos[miRegion.getHashCode] as string[];
                contador++;
                string hermano1 = "";
                string hermano2 = "";
                string hermano3 = "";
                if (contador == 5190318)
                {

                }
                if (miRegion.getEtiqueta == "HijoSD")
                {
                    hermano1 = miRegion.getHermanoSI;
                    hermano2 = miRegion.getHermanoID;
                    hermano3 = miRegion.getHermanoII;
                }
                else if (miRegion.getEtiqueta == "HijoSI")
                {
                    hermano1 = miRegion.getHermanoSD;
                    hermano2 = miRegion.getHermanoID;
                    hermano3 = miRegion.getHermanoII;
                }
                else if (miRegion.getEtiqueta == "HijoID")
                {
                    hermano1 = miRegion.getHermanoSI;
                    hermano2 = miRegion.getHermanoSD;
                    hermano3 = miRegion.getHermanoII;
                }
                else if (miRegion.getEtiqueta == "HijoII")
                {
                    hermano1 = miRegion.getHermanoSI;
                    hermano2 = miRegion.getHermanoID;
                    hermano3 = miRegion.getHermanoSD;
                }
                if ((miRecomponer.ContainsKey(hermano1)) && (miRecomponer.ContainsKey(hermano2)) && (miRecomponer.ContainsKey(hermano3)))
                {
                    if (miRecomponer.Count > 4)
                    {
                        Region miPadre = recomponerPadre(miRegion, Info, miRecomponer);
                        miRecomponer.Remove(hermano1);
                        miRecomponer.Remove(hermano2);
                        miRecomponer.Remove(hermano3);
                        miRecomponer.Remove(miRegion.getHashCode);
                        miRecomponer.Add(miPadre.getHashCode, miPadre);
                        miEnum = miRecomponer.Values.GetEnumerator();
                        miEnum.MoveNext();
                    }
                    else
                    {

                    }

                }
                else
                {
                    if (miRecomponer.Count > 4)
                    {
                        miEnum.MoveNext();
                    }
                    else
                    {

                    }
                }

            }
            foreach (Region miRegion in miRecomponer.Values)
            {
                if (miRegion.getEtiqueta == "HijoSD") mHijoSD = miRegion;
                if (miRegion.getEtiqueta == "HijoSI") mHijoSI = miRegion;
                if (miRegion.getEtiqueta == "HijoID") mHijoID = miRegion;
                if (miRegion.getEtiqueta == "HijoII") mHijoII = miRegion;
            }
            if (mHijoSD != null)
            {
                mMaxX = mHijoSD.getMaxX;
                mMaxY = mHijoSD.getMaxY;
            }
            if (mHijoII != null)
            {
                mMinX = mHijoII.getMinX;
                mMinY = mHijoII.getMinY;
            }

            mHojas.AddRange(mHijoSD.mHojas);
            mHojas.AddRange(mHijoSI.mHojas);
            mHojas.AddRange(mHijoID.mHojas);
            mHojas.AddRange(mHijoII.mHojas);

        }



        private Region recomponerPadre(Region miHijo, InfoTriangulacionNew Info, Hashtable iRecomponer)
        {
            Region miPadre = null;
            if (miHijo.getEtiqueta == "HijoSD")
            {
                miPadre = new Region(miHijo, iRecomponer[miHijo.getHermanoSI] as Region, iRecomponer[miHijo.getHermanoID] as Region, iRecomponer[miHijo.getHermanoII] as Region);

            }
            else if (miHijo.getEtiqueta == "HijoSI")
            {
                miPadre = new Region(iRecomponer[miHijo.getHermanoSD] as Region, miHijo, iRecomponer[miHijo.getHermanoID] as Region, iRecomponer[miHijo.getHermanoII] as Region);

            }
            else if (miHijo.getEtiqueta == "HijoID")
            {
                miPadre = new Region(iRecomponer[miHijo.getHermanoSD] as Region, iRecomponer[miHijo.getHermanoSI] as Region, miHijo, iRecomponer[miHijo.getHermanoII] as Region);

            }
            else if (miHijo.getEtiqueta == "HijoII")
            {
                miPadre = new Region(iRecomponer[miHijo.getHermanoSD] as Region, iRecomponer[miHijo.getHermanoSI] as Region, iRecomponer[miHijo.getHermanoID] as Region, miHijo);

            }

            if (miPadre != null)
            {
                miPadre.setEtiqueta = Info.getInfoEtiquetas[miPadre.getHashCode] as string;
                string[] misHermanos = Info.getInfoHermanos[miPadre.getHashCode] as string[];
                miPadre.setHermanoSD = misHermanos[0];
                miPadre.setHermanoSI = misHermanos[1];
                miPadre.setHermanoID = misHermanos[2];
                miPadre.setHermanoII = misHermanos[3];
            }

            return miPadre;
        }


        public Triangulo getRegion(Punto3d iPunto)
        {
            double miMitadX = mMaxX - ((mMaxX - mMinX) / 2);
            double miMitadY = mMaxY - ((mMaxY - mMinY) / 2);
            Triangulo miTrianguloCentro = null;
            if (this.mHijoII == null)
            {
                miTrianguloCentro = this.mTriangulo;
            }
            else
            {
                if (iPunto.coordenadaX < miMitadX)
                {
                    if (iPunto.coordenadaY < miMitadY)
                    {
                        miTrianguloCentro = this.mHijoII.getRegion(iPunto);
                    }
                    else
                    {
                        miTrianguloCentro = this.mHijoSI.getRegion(iPunto);
                    }
                }
                else
                {
                    if (iPunto.coordenadaY < miMitadY)
                    {
                        miTrianguloCentro = this.mHijoID.getRegion(iPunto);
                    }
                    else
                    {
                        miTrianguloCentro = this.mHijoSD.getRegion(iPunto);
                    }
                }
            }
            return miTrianguloCentro;

        }

        public Punto3d Centro
        {
            get
            {

                if (mPuntoCentro == null)
                {
                    double miMitadX = mMaxX - ((mMaxX - mMinX)/2);
                    double miMitadY = mMaxY - ((mMaxY - mMinY)/2);
                    mPuntoCentro = new Punto3d(miMitadX, miMitadY, 0);
                }

                return mPuntoCentro;
            }
        }

        public Triangulo TrianguloCentral
        {
            get
            {
                return this.mTriangulo;
            }
            set { mTriangulo = value; }
        }

        public Region setPadre
        {
            set
            {
                mPadre = value;
            }
        }
        public double getMaxX
        {
            get
            {
                return this.mMaxX;
            }
        }
        public double getMinX
        {
            get
            {
                return this.mMinX;
            }
        }
        public double getMaxY
        {
            get
            {
                return this.mMaxY;
            }
        }
        public double getMinY
        {
            get
            {
                return this.mMinY;
            }
        }

        public Region getPadre
        {
            get
            {
                return this.mPadre;
            }
        }
        public Region getHijoSD
        {
            get
            {
                return this.mHijoSD;
            }
        }
        public Region getHijoSI
        {
            get
            {
                return this.mHijoSI;
            }
        }
        public Region getHijoID
        {
            get
            {
                return this.mHijoID;
            }
        }
        public Region getHijoII
        {
            get
            {
                return this.mHijoII;
            }
        }

        public string getEtiqueta
        {
            get
            {
                return this.mEtiqueta;
            }
        }
        public string getHermanoSD
        {
            get
            {
                return this.mHermanoSD;
            }
        }
        public string getHermanoSI
        {
            get
            {
                return this.mHermanoSI;
            }
        }
        public string getHermanoID
        {
            get
            {
                return this.mHermanoID;
            }
        }
        public string getHermanoII
        {
            get
            {
                return this.mHermanoII;
            }
        }

        public string getHashCode
        {
            get
            {
                return mMaxX + "_" + mMaxY + "_" + mMinX + "_" + mMinY;
            }
        }

        public string setHermanoSD
        {
            set
            {
                this.mHermanoSD = value;
            }
        }
        public string setHermanoSI
        {
            set
            {
                this.mHermanoSI = value;
            }
        }
        public string setHermanoID
        {
            set
            {
                this.mHermanoID = value;
            }
        }
        public string setHermanoII
        {
            set
            {
                this.mHermanoII = value;
            }
        }
        public string setEtiqueta
        {
            set
            {
                this.mEtiqueta = value;
            }
        }

        
        
    }
}
