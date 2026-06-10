using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ

{
    
    using tadLayLogica.EjeTJ.Tramos;
    using tadLayLogica.EjeTJ.Vertice;
    using tadLayShare.puntos;
    
    /// <summary>
    /// Eje Tadil
    /// </summary>
    /// <typeparam name="T">Diccionario Vertices</typeparam>
    /// <typeparam name="J">Tipo de Segementos</typeparam>
    public abstract class oEjeObj<T, J>
        where T : IVertice 
        where J : ISegmento, new()
    {

        private string mName = string.Empty;
        private Dictionary<int, T> mVertices = null;
        private Dictionary<int, J> mSegmentos = null;


        public oEjeObj()
        { 
        
        
        }

        public oEjeObj(string iEjeName, Dictionary<int, T> iListadoVertices)
        {
            mName = iEjeName;
            mVertices = new Dictionary<int, T>();
            mVertices = iListadoVertices;

            
        }


        #region "Propiedades Vertices"
        /// <summary>
        /// Diccionario Vertices
        /// </summary>
        public Dictionary<int, T> vertices
        {
            get
            {
                return mVertices;
            }

            set
            {
                mVertices = value;
            }
        }
        /// <summary>
        /// Primer Vertice
        /// </summary>
        public T firstVertice
        {
            get
            {
                return vertices[0];
            }
        }
        /// <summary>
        /// Segundo Vertice
        /// </summary>
        public T secondVertice
        {
            get
            {
                return vertices[1];
            }
        }
        /// <summary>
        /// Ultimo Vertice
        /// </summary>
        public T endVertice
        {
            get
            {
                return vertices[vertices.Count - 1];
            }
        }
        /// <summary>
        /// Penultimo Vertice
        /// </summary>
        public T preEndVertice
        {
            get
            {
                return vertices[vertices.Count - 2];
            }
        }
        #endregion
        #region "Propiedades Segmentos"





        /// <summary>
        /// Diccionario Vertices
        /// </summary>
        public Dictionary<int, J> segmentos
        {
            get
            {

                if (mSegmentos == null)
                {

                    mSegmentos = new Dictionary<int, J>();

                    for (int i = 0; i <= vertices.Count - 2; i++)
                    {

                        mSegmentos.Add(i, segmento(i));

                    }


                }

                return mSegmentos;


            }
        }

        /// <summary>
        /// Primer Segmento
        /// </summary>
        public J firstSegmento
        {
            get
            {
                return segmentos[0];
            }
        }
        
        /// <summary>
        /// Segundo Segmento
        /// </summary>
        public J secondSegmento
        {
            get
            {
                return segmentos[1];
            }
        }

        /// <summary>
        /// Penultimo Segmento
        /// </summary>
        public J preEndSegmento
        {
            get
            {
                return segmentos[vertices.Count - 3];
            }

        }

        /// <summary>
        /// Ultimo Segmento
        /// </summary>
        public J endSegmento
        {

            get
            {
                return segmentos[vertices.Count - 2];
            }
        }

        /// <summary>
        /// Segmento Previo Dado Vertice
        /// </summary>
        public J preSeg(int iIdVertice)
        {
            return segmentos[iIdVertice - 1];
        }

        /// <summary>
        /// Segmento Next dado Vertice
        /// </summary>
        public J nexSeg(int iIdVertice)
        {
            return segmentos[iIdVertice];

        }

        /// <summary>
        /// Segmento Previo Dado Id Segmento
        /// </summary>
        public J preSegByTramo(int iIdTramo)
        {
           return segmentos[iIdTramo - 1];
        }


        /// <summary>
        /// Segmento Next Dado Id Segmento
        /// </summary>
        public J nexSegByTramo(int iIdTramo)
        {
            return segmentos[iIdTramo + 1];
        }
      


        /// <summary>
        /// SEGMENTO
        /// </summary>
        /// <param name="id">ID en BASE CERO</param>
        private J segmento(int id)
        {

         
            J myJ = new J();

            oTramoBase myTrBase = new oTramoBase(vertices[id] as oVer, vertices[id + 1] as oVer);

            return (J)myJ.createDerivedFromBase(myTrBase);


        }








        /// <summary>
        /// Es Vertice Final
        /// </summary>
        public bool isEndVertice(int iId)
        {
            if (iId == verticesCount - 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        
        
        }

        /// <summary>
        /// BASE UNO
        /// </summary>
        public int segmentosCount
        {
            get
            {
                return vertices.Count() - 1;
            }
        }





        #endregion

        #region "Propiedades Eje"
        /// <summary>
        /// Nombre del Eje
        /// </summary>
        public string EjeName
        {
            get
            {
                return mName;
            }

            set
            {
                mName = value;
            }
        }
        /// <summary>
        /// Número Vertices Base 1
        /// </summary>
        public int verticesCount
        {
            get
            {
                return vertices.Count();
            }
        }
        /// <summary>
        /// Longitud 2d
        /// </summary>
        public double longitud2d
        {
            get
            {
                double myLon = 0;

                foreach (KeyValuePair<int, J> myTramo in segmentos)
                {
                    myLon = myLon + myTramo.Value.lon2d.Value;
                }

                return myLon;
            }
        }
        /// <summary>
        /// Ángulo Vertice
        /// </summary>
        /// <param name="iVertice">ID VERTICE BASE 0</param>
        public double anguloVertice(int iVertice, eAng iAng)
        {

            IP2d myP0 = vertices[iVertice].position;
            IP2d myP1 = vertices[iVertice - 1].position;
            IP2d myP2 = vertices[iVertice + 1].position;

            return oTrigo.getAngFrom3Point(myP0, myP1, myP2, iAng);
        }
        #endregion




    }
}