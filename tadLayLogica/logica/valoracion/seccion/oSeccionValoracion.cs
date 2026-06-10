using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{

    using Autodesk.AutoCAD.Geometry;
    using tadLayLogica.zonaGis;
    
    public class oSeccionValoracion
    {

        #region "Constructores"

        public oSeccionValoracion(int iIdBaseUno, double iPk, Point3d iPtoPk, eRoadSeccion iEstructuraTipo, oZonaGis iZonaGis)
        {
            idBaseUno = iIdBaseUno;
            pk = iPk;
            ptoPk = iPtoPk;
            estructuraTipo = iEstructuraTipo;
            zonaGis = iZonaGis;
        }

        #endregion
        #region "Propiedades"

        public int? idBaseUno { get; private set; }
        public double? pk { get; private set; }
        public Point3d ptoPk { get; private set; }
        public eRoadSeccion estructuraTipo { get; private set; }
        public oZonaGis zonaGis {get; private set; }

        #endregion

    }





}
