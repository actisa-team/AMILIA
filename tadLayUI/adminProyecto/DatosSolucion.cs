using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tadLayUI.adminProyecto
{
    public class DatosSolucion
    {
        #region carreteras
        public string ucRoadGrupo { get; set; }
        public double ucVp { get; set; }
        public double ucRadio { get; set; }
        public double ucPeralte { get; set; }
        public double ucValorMinimoSalidaLlegada { get; set; }
        public double ucAijMinimoTramo { get; set; }
        public double ucAijMaximoTramo { get; set; }
        public double ucAvanceMaximo { get; set; }
        public double ucKvConvexo { get; set; }
        public double ucKvConcavo { get; set; }

        public bool chkPermitirReduccionesVelocidad { get; set; }
        public bool ucRadioCondicionadoLmin { get; set; }

        #endregion

        #region tijera

        #region addrectas
        public bool sinrectas { get; set; }
        public bool longitudminima { get; set; }
        public bool longitudmaxima { get; set; }
        public bool longitudlimitada { get; set; }
        public bool descartaralternativascortas { get; set; }
        public string distdescartaralternativa { get; set; }
        #endregion

        #region curvas
        public bool aplicargranradio { get; set; }
        public string granradio { get; set; }
        #endregion 
        public string coeficientedisttramo { get; set; }
        public string coeficientedisteje { get; set; }
        public string entronquesegundos { get; set; }
        public string entronquedistancia { get; set; }
        public string distanciaconvergencia { get; set; }
        public bool penalizartramoscortos { get; set; }
        public bool distentronque { get; set; }
        public string distentronquepc { get; set; }

        #endregion

        #region pendientes
        public double ucRoadPendienteMaximaPC { get; set; }
        public double ucRoadPendienteMinimaPC { get; set; }
        public double ucEstructurasPendienteMaximaPC { get; set; }
        public double ucEstructurasPendienteMinimoPC { get; set; }


        #endregion

        #region valoraciones
        public double valoracionDistanciaPC { get; set; }
        public double valoracionPendientePC { get; set; }
        public double valoracionCosteImplantacionPC { get; set; }


        #endregion

        #region estudiopreviogeometria
        public double ucAnchoPlataforma { get; set; }
        public double ucDesmonteAlturaMaxima { get; set; }
        public double ucTerraplenAlturaMaxima { get; set; }
        public double ucDesmonteTaludPC { get; set; }
        public double ucTerraplenTaludPC { get; set; }
        public string ucGenerarPuntes { get; set; }
        public string ucGenerarTuneles { get; set; }

        public double ucPuenteAlturaMaxima { get; set; }

        public double ucCosteImplantacion { get; set; }
        public double ucCosteDesmonteUnitario { get; set; }
        public double ucCosteTerraplenUnitario { get; set; }
        public double ucCostePuentesViaductosUnitario { get; set; }
        public double ucCosteTunelesUnitario { get; set; }


        #endregion

        #region opcionesavanzadas
        public double ucAbanicoAnguloTotalGrados { get; set; }
        public double ucAbanicoDisretizacionGrados { get; set; }
        public double ucAbanicoTramoDiscretizacionMetros { get; set; }
        public double ucAbanicoToleranciaPuntoObjetivo { get; set; }
        public bool chkAbanicoInvalidar { get; set; }
        public double? ucInvalidarTramosLongitudPC { get; set; }
        public bool chkRoadConsiderarAijConstante { get; set; }


        #endregion

        #region optavanzandascoeficientes
        public double ucCoeRoadPendienteMaxima { get; set; }
        public double ucCoeEstructurasPendienteMaxima { get; set; }
        public double ucCoeDesmonteAlturaMaxima { get; set; }
        public double ucCoeTerraplenAlturaMaxima { get; set; }
        public double ucCoePilaAlturaMaxima { get; set; }


        #endregion

        #region ejevisibilidadglobal
        public string triangulomax { get; set; }
        public string cotamax { get; set; }
        public string separacion { get; set; }
        #endregion

        public string nombresol { get; set; }
        public bool solEnvolventes { get; set; }
        public bool avancescortos { get; set; }
        public bool avanceslargos { get; set; }
        public bool autocorreccion { get; set; }
        public string repeticiones { get; set; }
        public List<Point2DJson> Eje_Visibilidad { get; set; }
        public string ucFileNormasVpRadio { get; set; }
        public string ucFileNormasVpKv { get; set; }
        public string ucFileBaseDatos { get; set; }
        public double seccionIntervalo { get; set; }
        public string ucSeccionGrupo { get; set; }
        public string ucSeccionTipo { get; set; }
        public string ucSeccionItem { get; set; }
        public string ucSeccionMacroPrecio { get; set; }
        public string ucZonasMovimientoTierras1 { get; set; }
        public string ucZonasCimentacion1 { get; set; }
        public string ucZonasEstructuras1 { get; set; }
        public string ucZonasTuneles1 { get; set; }

    }
    public class Point2DJson
    {
        public double X { get; set; }
        public double Y { get; set; }

        // Constructor vacío necesario para la deserialización
        public Point2DJson() { }

        // Constructor para convertir desde Point2d
        public Point2DJson(Point2d point)
        {
            X = point.X;
            Y = point.Y;
        }

        // Método para convertir de vuelta a Point2d
        public Point2d ToPoint2d()
        {
            return new Point2d(X, Y);
        }
    }
}
