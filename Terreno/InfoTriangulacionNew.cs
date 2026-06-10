using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using tadLayShare;
using tadLayShare.puntos;

namespace Terrenos
{
    [Serializable]
    public class InfoTriangulacionNew
    {
        private List<InfoTrianguloNew> mInfoTriangulacion = new List<InfoTrianguloNew>();
        private List<InfoRegionNew> mInfoRegiones = new List<InfoRegionNew>();
        private System.Collections.Hashtable mHermanosRegion = new System.Collections.Hashtable();
        private System.Collections.Hashtable mEtiquetasRegion = new System.Collections.Hashtable();
        private String mName = "";
        private int mNumNodos = 0;


        public InfoTriangulacionNew(InfoTriangulacion infoTriangulacion)
        {
            mName = infoTriangulacion.Name;
            var listPuntos = new List<Punto3d>();
            var triangStringIndex = new Hashtable();

            int i = 0;

            foreach (InfoTriangulo infoTriang in infoTriangulacion.getInfoTriangulos)
            {
                var va = new Punto3d(infoTriang.getVertAX, infoTriang.getVertAY, infoTriang.getVertAZ);
                var vb = new Punto3d(infoTriang.getVertBX, infoTriang.getVertBY, infoTriang.getVertBZ);
                var vc = new Punto3d(infoTriang.getVertCX, infoTriang.getVertCY, infoTriang.getVertCZ);
                var indexA = GetIndexVertex(listPuntos, va);
                var indexB = GetIndexVertex(listPuntos, vb);
                var indexC = GetIndexVertex(listPuntos, vc);


                var infoTriangulo = new InfoTrianguloNew(infoTriang, indexA, indexB, indexC, i);
                triangStringIndex.Add(infoTriang.getID, i);
                mInfoTriangulacion.Add(infoTriangulo);
                i++;
            }
            i = 0;
            foreach (InfoTriangulo infoTriang in infoTriangulacion.getInfoTriangulos)
            {
                if (infoTriang.getAd1 != "")
                    mInfoTriangulacion[i].mAdyacente1 = (int)triangStringIndex[infoTriang.getAd1];
                if (infoTriang.getAd2 != "")
                    mInfoTriangulacion[i].mAdyacente2 = (int)triangStringIndex[infoTriang.getAd2];
                if (infoTriang.getAd3 != "")
                    mInfoTriangulacion[i].mAdyacente3 = (int)triangStringIndex[infoTriang.getAd3];
                i++;
            }

            foreach (var infoRegion in infoTriangulacion.getRegiones)
            {
                var index =  triangStringIndex[infoRegion.getTriangulo];
                var infRegionNew = new InfoRegionNew(infoRegion, (int)index);
                mInfoRegiones.Add(infRegionNew);
            }
            mHermanosRegion = infoTriangulacion.getInfoHermanos;
            mEtiquetasRegion = infoTriangulacion.getInfoEtiquetas;
            mNumNodos = listPuntos.Count;
        }

        private static int GetIndexVertex(List<Punto3d> listPuntos, Punto3d va)
        {
            var indexA = -1;

            for(int i=0;i<listPuntos.Count;i++)
            {
                var punto = listPuntos[i];
                if (punto.coordenadaX == va.coordenadaX && punto.coordenadaY == va.coordenadaY &&
                    punto.coordenadaZ == va.coordenadaZ)
                {
                    indexA = i;
                    break;
                }
            }

            if (indexA == -1)
            {
                listPuntos.Add(va);
                indexA = listPuntos.Count - 1;
            }
            return indexA;
        }


        public InfoTriangulacionNew(Triangulacion iTriangulacion)
        {
            mName = iTriangulacion.Name;
            mNumNodos = iTriangulacion.getNodos.Count;
            foreach (Triangulo miTriangulo in iTriangulacion.getTriangulos)
            {
                if (miTriangulo==null)
                {
                    mInfoTriangulacion.Add(null);
                    continue;
                }
                mInfoTriangulacion.Add(new InfoTrianguloNew(iTriangulacion, miTriangulo));
            }
            getInfoRegiones(iTriangulacion.getRegiones);

        }
        public InfoTriangulacionNew(Triangulacion_old iTriangulacion)
        {
            mName = iTriangulacion.Name;
            mNumNodos = iTriangulacion.getNodos.Count;
            foreach (Triangulo miTriangulo in iTriangulacion.getTriangulos)
            {
                if (miTriangulo == null)
                {
                    mInfoTriangulacion.Add(null);
                    continue;
                }
                mInfoTriangulacion.Add(new InfoTrianguloNew(iTriangulacion, miTriangulo));
            }
            getInfoRegiones(iTriangulacion.getRegiones);

        }
      


        private void getInfoRegiones(Region iRegion)
        {

            string[] misHermanos = new string[4];
            misHermanos[0] = iRegion.getHermanoSD;
            misHermanos[1] = iRegion.getHermanoSI;
            misHermanos[2] = iRegion.getHermanoID;
            misHermanos[3] = iRegion.getHermanoII;
            mHermanosRegion.Add(iRegion.getHashCode, misHermanos);

            mEtiquetasRegion.Add(iRegion.getHashCode, iRegion.getEtiqueta);

            if (iRegion.getHijoID != null)
            {
                getInfoRegiones(iRegion.getHijoID);
                getInfoRegiones(iRegion.getHijoII);
                getInfoRegiones(iRegion.getHijoSD);
                getInfoRegiones(iRegion.getHijoSI);
            }
            else
            {
                mInfoRegiones.Add(new InfoRegionNew(iRegion));
            }
        }

        public List<InfoTrianguloNew> getInfoTriangulos
        {
            get
            {
                return mInfoTriangulacion;
            }
        }

        public int numNodos
        {
            get
            {
                return mNumNodos;
            }
        }

        public List<InfoRegionNew> getRegiones
        {
            get
            {
                return mInfoRegiones;
            }
        }

        public System.Collections.Hashtable getInfoHermanos
        {
            get
            {
                return mHermanosRegion;
            }
        }

        public System.Collections.Hashtable getInfoEtiquetas
        {
            get
            {
                return mEtiquetasRegion;
            }
        }

        public static InfoTriangulacionNew recuperaInformacion(MemoryStream iStream)
        {
                BinaryFormatter formatterR = new BinaryFormatter();
            try
            {
                InfoTriangulacionNew deserializada = (InfoTriangulacionNew)formatterR.Deserialize(iStream);
                return deserializada;
            }
            catch (Exception)
            {
                iStream.Position = 0;
                InfoTriangulacion deserializada = (InfoTriangulacion)formatterR.Deserialize(iStream);

                return new InfoTriangulacionNew(deserializada);
            }
        }


        public String Name
        {
            get
            {
                return mName;
            }
        }


        
    }


}
