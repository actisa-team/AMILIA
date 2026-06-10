using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.zonaGis.secciones
{


    using System.Collections.ObjectModel;

    using Autodesk.AutoCAD.Geometry;

    using tadLayLogica;
    using tadLayLogica.zonaGis;
    using tadLayShare;
 
    public class oCollectionZonasDesign : Collection<oSeccionZonasDesign>
    {

        public oCollectionZonasDesign()
        {

        }



        public int numeroSeccionesCalzada ()
        {
            return this.contarSecciones(eRoadSeccion.calzada);  
        }

        public int numeroSeccionesPuente ()
        {
            return this.contarSecciones(eRoadSeccion.puente);
        }

        public int numeroSeccionesTunel()
        {
            return this.contarSecciones(eRoadSeccion.tunel);
        }

         
        private int contarSecciones (eRoadSeccion iSeccionRoad)
        {
            var miQuery = from p in this
                          where p.seccionTipo == iSeccionRoad
                          select p;

            if (miQuery != null)
            {
                return miQuery.Count();
            }
            else
            {
                return 0;
            }

        }

    }
    public class oSeccionZonasDesign
    {


        public double pk { get; private set; }
        public eRoadSeccion seccionTipo { get; private set; }
        public oZonaGeoMovimientoTierras zonaMovimientoTierras { get; private set; }
        public oZonaGeoCimentacion zonaCimentacion { get; private set; }
        public oZonaGeoEstructuras zonaPuentes { get; private set; }
        public oZonaGeoTuneles zonaTuneles { get; private set; }
        public Point3d pkPto { get; private set; }


        public oSeccionZonasDesign(double iPk, 
                                   Point3d iPkPto,
                                   string iRoadSeccionEnum,
                                   Guid  iIdZonaMovTierras,                             
                                   Guid iIdZonaCimentacion,
                                   Guid iIdZonaPuentes,
                                   Guid iIdZonaTuneles)
      {

          this.pk = iPk;
          this.pkPto = iPkPto;
          this.seccionTipo = tadLayLogica.oExtensionEnumeraciones.getRoadSeccion(iRoadSeccionEnum);
          this.zonaMovimientoTierras = new oZonaGeoMovimientoTierras(iIdZonaMovTierras);
          this.zonaCimentacion = new oZonaGeoCimentacion(iIdZonaCimentacion);
          this.zonaPuentes = new oZonaGeoEstructuras(iIdZonaPuentes);
          this.zonaTuneles = new oZonaGeoTuneles(iIdZonaTuneles);
        }


        public oZonaGis getZonaValoracion ()
        {

            if (this.seccionTipo == eRoadSeccion.calzada)
            {
                return (oZonaGis)this.zonaMovimientoTierras;
            }
            else if (this.seccionTipo == eRoadSeccion.puente)
            {
                return (oZonaGis)this.zonaCimentacion;
            }
            else if (this.seccionTipo == eRoadSeccion.tunel)
            {
                return (oZonaGis)this.zonaTuneles;
            }
            else
            {
               throw new oExEnumNotImplemented(this.seccionTipo.ToString());
            }

        }


    }
}
