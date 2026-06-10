using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.estudioTipo
{
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayLogica.datos.proyecto.estudioPrevio;
    using tadLayLogica.datos.proyecto;
    using tadLayShare.puntos;

   public interface IEstudio
    {
        ISeccionCalzada getISeccionCalzadaByPto(IP2d iPto);
        IzonaMovimientoTierras getIZonaMovimientoTierrasByPto(IP2d iPto);
        IzonaPuentes getIZonaPuenteByPto(IP2d iPto);
        IzonaTuneles getIZonaTunelByPto(IP2d iPto);

        bool isOnZonaPasoObligadoEstructuras(IP2d iPto);
        bool isTramoObligadoAPuente(IP2d miPEntradaTramo, IP2d miPSalidaTramo);
        bool isTramoObligadoAPuenteoTunel(IP2d miPEntradaTramo, IP2d miPSalidaTramo);

        bool isValidoCruceConDPH(IP2d iPto1, IP2d iPto2);
        bool isValidoTramoDentroZonaDPH(IP2d miPEntradaTramo, IP2d miPSalidaTramo);
    

    }
   public interface ISeccionCalzada
    {
        Guid? idSeccion { get; set; }
        string nombre { get; set; }
        
        double? anchoPlataforma { get; set; }
        double? costeUnitario { get; set; }
    }
   public interface IzonaMovimientoTierras
    {

        Guid? idZonaMovimientoTierras { get; set; }
        string nombre { get; set; }
        
        double? desmonteAlturaMaximaProyecto { get; set; }
        double? desmonteCoeMinoracion { get; set; }
        double? desmonteAlturaMaximaCalculo { get; } 
        double? desmonteTaludProyecto { get; set; }
        double? desmonteCosteUnitario { get; set; }

        double? terraplenAlturaMaximaProyecto { get; set; }
        double? terraplenCoeMinoracion { get; set; }
        double? terraplenAlturaMaximaCalculo { get; }
        double? terraplenTaludProyecto { get; set; }
        double? terraplenCosteUnitario { get; set; }
    } 
   public interface IzonaPuentes
    {

        Guid? idZonaPuentes { get; set; }
        string nombre { get; set; }
        string bloqueNombreSinExtension { get; set; }

        bool? allowPuentes { get; set; }

        double? puenteAlturaMaximaProyecto { get; set; }
        double? puenteAlturaCoeMinoracion { get; set; }
        double? puenteAlturaMaximaCalculo { get;}
 
        double?  puenteCosteUnitario { get; set; }
    }
   public interface IzonaTuneles 
    {
        Guid? idZonaTuneles { get; set; }
        string nombre { get; set; }
        string bloqueNombreSinExtension { get; set; }
        bool? allowTuneles { get; set; }
        double? tunelCosteUnitario { get; set; }
    }
}
