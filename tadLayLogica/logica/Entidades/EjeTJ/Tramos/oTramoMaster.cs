using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ.Tramos
{
    
    using tadLayLogica.EjeTJ.Vertice;
    using tadLayShare.puntos;


    public interface ISegmento
    {
        int id { get; set; }
        double? lon2d { get; }
        double? lon3d { get; }
        object createDerivedFromBase(object myBase);
    }




    public abstract class oTramoMaster<K> : ISegmento
    where K : IVertice, new()
    {

        private int mId;
        private K mVi;
        private K mVj;


        //Vertices Total Eje Base Cero
        public static int? verticesNum = null;

        public oTramoMaster()
        {



        }

        public oTramoMaster(K iVi, K iVj)
        {
            mId = iVi.id;
            mVi = iVi;
            mVj = iVj;
        }



        #region "Propiedades"

        /// <summary>
        /// ID Base 1 ; Tramo 0 = Tramo 1 Usuario
        /// </summary>
        public string idBase1
        {
            get
            {
                return Convert.ToString(mId + 1);
            } 
        }


        /// <summary>
        /// ID del Tramo  / BASE CERO
        /// </summary>
        public int id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }
        /// <summary>
        /// Vertice Inicial Tramo / BASE CERO
        /// </summary>
        public K Vi
        {
            get
            {
                return mVi;
            }
            set
            {
                mVi = value;
            }
        }
        /// <summary>
        /// Vertice Final Tramo / BASE CERO
        /// </summary>
        public K Vj
        {
            get
            {
                return mVj;
            }
            set
            {
                mVj = value;
            }
        }
        /// <summary>
        /// Incremento de X (Vjx-Vix)
        /// </summary>
        public double IncX
        {
            get
            {
                return mVj.position.X - mVi.position.X;                 
            }
        }
        /// <summary>
        /// Incremento de Y (Vjy-Viy)
        /// </summary>
        public double IncY
        {
            get
            {
               return mVj.position.Y - mVi.position.Y;
            }

        }
        /// <summary>
        /// Incremento de Z (Vjz-Viz)
        /// </summary>
        public double IncZ
        {
            get
            {
              return mVj.position.Z - mVi.position.Z;
            }

        }

        /// <summary>
        /// Azimut Tramo Grados
        /// </summary>
        public double azimutGrados
        {
            get
            {
                return oTrigo.getAzimutGrados(mVi.position, mVj.position);
            }
        }

        /// <summary>
        /// Longitud Plano XY
        /// </summary>
        public double? lon2d
        {
            get
            {
                return mVi.position.distTo2d(mVj.position);
            }
        }
        /// <summary>
        /// Longitud Real
        /// </summary>
        public double? lon3d
        {

            get
            {
                return mVi.position.distTo3d(mVj.position);

            }


        }
        /// <summary>
        /// Pendiente del Tramo 3D [Por Ciento]
        /// </summary>
        public double? pendientePorCiento3D
        {
            get
            {
                return oTrigo.getPendiente3D(mVi.position, mVj.position, ePorcentaje.porCiento);

            }

        }
        /// <summary>
        /// Pendiente del Tramo 3D [Por Uno]
        /// </summary>
        public double? pendientePorUno3D
        {
            get
            {
                return oTrigo.getPendiente3D(mVi.position, mVj.position, ePorcentaje.porUno);
            }

        }
        /// <summary>
        /// Pendiente del Tramo 3D ; Valor Absoluto
        /// </summary>
        public double? pendientePorCientoAbs3D
        {
            get
            {
                return Math.Abs(pendientePorCiento3D.Value);
            }

        }
        /// <summary>
        /// Pendiente del Tramo 2D [Por Ciento]
        /// </summary>
        public double? pendientePorCiento2D
        {
            get
            {
                return oTrigo.getPendiente2D(mVi.position, mVj.position, ePorcentaje.porCiento);

            }

        }
        /// <summary>
        /// Pendiente del Tramo 2D [Por Uno]
        /// </summary>
        public double? pendientePorUno2D
        {
            get
            {
                return oTrigo.getPendiente2D(mVi.position, mVj.position, ePorcentaje.porUno);
            }

        }
        /// <summary>
        /// Pendiente del Tramo 2D ; Valor Absoluto
        /// </summary>
        public double? pendientePorCientoAbs2D
        {
            get
            {
                return Math.Abs(pendientePorCiento2D.Value);
            }

        }
        /// <summary>
        /// Is Tramo Inicial
        /// </summary>
        public bool isTramoInicial
        {
            get
            {

                if (this.id == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }     
        }
        /// <summary>
        /// Is Tramo Final
        /// </summary>
        public bool isTramoFinal
        {

            get
            {
                if (verticesNum == null)
                {
                    throw new Exception("No se ha inicial la función Vertices Número");
                
                }
                   
                if (verticesNum == this.id)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
     
        }



        #endregion

        /// <summary>
        /// Obtener el Valor de Z del Tramo a una Distancia X
        /// </summary>
        /// <param name="iLonFromOrigen">Longitud Origen</param>
        /// <returns>Cota Z</returns>
        public double getZFromOrigen(double iLonFromOrigen)
        {

            return (Vi.position.Z) + iLonFromOrigen * pendientePorUno3D.Value;



        }

        public override string ToString()
        {
            return ("Tramo= " + (id + 1).ToString() + "\n" +
                    "Vi= " + Vi.ToString() + "\n" +
                    "Vj= " + Vj.ToString() + "\n" +
                    "Lon XY= " + lon2d.ToString() + "\n" +
                    "Pendiente 3D [%] = " + pendientePorCiento3D.ToString() + "\n" +
                    "Pendiente 2D [%] = " + pendientePorCiento2D.ToString());
        }




        public virtual object createDerivedFromBase(object myBase)
        {
            throw new NotImplementedException("Clase Tramo Master");
        }
    }



    public class oTramoBase : oTramoMaster<oVer>
    {

 

        #region "Constructores"


        public oTramoBase()
        {

        }


        public oTramoBase(int iIdSegmento, IP3d iVi, IP3d iVj)
        {
            id = iIdSegmento;
            Vi.id = iIdSegmento - 1;
            Vi.position = iVi;
            Vj.id = iIdSegmento - 1;
            Vj.position = iVj;
        }

        public oTramoBase(oVer iVi, oVer iVj)
        {
            id = iVi.id;
            Vi = iVi;
            Vj = iVj;
        }

        public oTramoBase(oVer iVi, double iAzimutGrados, double iLon)
        {
            id = iVi.id;
            Vi = iVi;
            Vj.position = (IP3d)oTrigo.getP2FromAzimutLon(iVi.position, iAzimutGrados, iLon);
        }

        #endregion



        public override object createDerivedFromBase(object myBase)
        {
            oTramoBase mySegmento = (oTramoBase)myBase;

            this.id = mySegmento.id;
            this.Vi = mySegmento.Vi as oVer;
            this.Vj = mySegmento.Vj as oVer;

            return this; 
        }
    }


}
