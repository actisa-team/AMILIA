using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ.Vertice
{

    using tadLayShare.puntos;

    public interface IVertice
    {
        int id { get; set; }
        IP3d position { get; set; }
    }


    public class oVer : IVertice
    {
        private int mId;
        private IP3d mPosition = null;


        public oVer()
        {

        }

        public oVer(int iId, oP3d iPosition)
        {
            mId = iId;
            mPosition = iPosition;
        }


        #region "Propiedades"

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
        public IP3d position
        {
            get
            {
                return mPosition;
            }
            set
            {
                mPosition = value;
            }
        }

        public override string ToString()
        {
            return "id: " + id.ToString() +
                   " X: " + mPosition.X.ToString() +
                   " Y: " + mPosition.Y.ToString() +
                   " Z: " + mPosition.Z.ToString() ;
        }

        #endregion



    }

}