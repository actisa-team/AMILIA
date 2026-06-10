using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Terrenos
{
    [Serializable]
    public class InfoTriangulacion
    {
        private List<InfoTriangulo> mInfoTriangulacion = new List<InfoTriangulo>();
        private List<InfoRegion> mInfoRegiones = new List<InfoRegion>();
        private System.Collections.Hashtable mHermanosRegion = new System.Collections.Hashtable();
        private System.Collections.Hashtable mEtiquetasRegion = new System.Collections.Hashtable();
        private String mName = "";


        //public InfoTriangulacion(Terrenos.Triangulacion iTriangulacion)
        //{
        //    mName = iTriangulacion.Name;
        //    foreach (Terrenos.triangulos.Triangulo miTriangulo in iTriangulacion.getTriangulos)
        //    {
        //        var info = new InfoTriangulo(iTriangulacion, miTriangulo);
        //        if (!mInfoTriangulacion.Contains(info))
        //            mInfoTriangulacion.Add(new InfoTriangulo(iTriangulacion, miTriangulo));
        //    }
        //    getInfoRegiones(iTriangulacion.getRegiones);

        //}

        //private void getInfoRegiones(Terrenos.Region iRegion)
        //{

        //    string[] misHermanos = new string[4];
        //    misHermanos[0] = iRegion.getHermanoSD;
        //    misHermanos[1] = iRegion.getHermanoSI;
        //    misHermanos[2] = iRegion.getHermanoID;
        //    misHermanos[3] = iRegion.getHermanoII;
        //    mHermanosRegion.Add(iRegion.getHashCode, misHermanos);

        //    mEtiquetasRegion.Add(iRegion.getHashCode, iRegion.getEtiqueta);

        //    if (iRegion.getHijoID != null)
        //    {
        //        getInfoRegiones(iRegion.getHijoID);
        //        getInfoRegiones(iRegion.getHijoII);
        //        getInfoRegiones(iRegion.getHijoSD);
        //        getInfoRegiones(iRegion.getHijoSI);
        //    }
        //    else
        //    {
        //        mInfoRegiones.Add(new InfoRegion(iRegion));
        //    }
        //}

        public List<InfoTriangulo> getInfoTriangulos
        {
            get
            {
                return mInfoTriangulacion;
            }
        }

        public List<InfoRegion> getRegiones
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

        public static InfoTriangulacion recuperaInformacion(MemoryStream iStream)
        {
                BinaryFormatter formatterR = new BinaryFormatter();
                InfoTriangulacion deserializada = (InfoTriangulacion) formatterR.Deserialize(iStream);
                return deserializada;
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
