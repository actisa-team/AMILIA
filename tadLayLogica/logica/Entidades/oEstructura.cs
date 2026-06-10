using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.Entidades
{
    public class oEstructura
    {
                private double mPk;
        private eRoadSeccion mTipoEstructura;
        private string mNombre;

        public oEstructura(double iPk, eRoadSeccion iTipoEstructura, string iNombre)
        {
            mPk = iPk;
            mTipoEstructura = iTipoEstructura;
            mNombre = iNombre;
        }

        public double getPk
        {
            get
            {
                return mPk;
            }
        }

        public eRoadSeccion getTipoEstructura
        {
            get
            {
                return mTipoEstructura;
            }
        }

        public string getNombre
        {
            get
            {
                return mNombre;
            }
        }

    }
    
}
