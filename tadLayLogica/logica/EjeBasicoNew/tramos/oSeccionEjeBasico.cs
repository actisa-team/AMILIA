using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace tadLayLogica.logica.EjeBasicoNew
{

    using engNet.Extension.Double;


    using System.Collections.ObjectModel;
    using tadLayLogica.estudioTipo;
    using tadLayLogica.zonaGis;
    using tadLayLan;
    using tadLayShare.puntos;
    using tadLayShare;



    public class oSeccionEje
    {
      
        public oP3d ptoRoad { get; set; }
        public double? terrenoZ { get; set; }
        public double? longitudSeccion { get; set; }
        public eRoadSeccion? seccionTipo { get; set; }


        #region "Constructor"

        public oSeccionEje()
        {

        }

        public oSeccionEje(oP3d iPtoRoad, double iTerrenoZ, double iLongitudSeccion)
        {
            this.ptoRoad = iPtoRoad;
            this.terrenoZ = iTerrenoZ;
            this.longitudSeccion = iLongitudSeccion;
        }


        #endregion


        #region "Propiedades"

        public double alturaMovimientoTierrasAbs
        {
            get
            {
                return Math.Abs(this.ptoRoad.Z - this.terrenoZ.Value);
            }

        }
        public eExcavacion excavacionTipo
        {
            get
            {
                if (this.ptoRoad.Z == this.terrenoZ.Value)
                {
                    return eExcavacion.acota;
                }
                else if (this.ptoRoad.Z > this.terrenoZ.Value)
                {
                    return eExcavacion.terraplen;
                }
                else if (this.ptoRoad.Z < this.terrenoZ.Value)
                {
                    return eExcavacion.desmonte;
                }
                else
                {
                    throw new Exception(string.Format(strError.eCasoIndeterCotas, this.ptoRoad.Z, this.terrenoZ.Value));
                }
            }
        }
        #endregion

    }

    public class oSeccionEjeBasico : oSeccionEje
    {
        #region "Propiedades"

        public int? idPto { get; set; }
        public double? terrenoSlope { get; private set; }
        public ISeccionCalzada seccionCalzada { get; private set; }
        public IzonaMovimientoTierras zonaMovimientoTierras { get; private set; }
        public IzonaPuentes zonaPuente { get; private set; }
        public IzonaTuneles zonaTunel { get; private set; }
        public bool isObligadoEstructura { get; set; }

        #endregion
        #region "Constructor"

        public oSeccionEjeBasico()
        {

        }


        public oSeccionEjeBasico(int iIdPto,
                         oP3d iPtoRoad,
                         double iLonSeccion,
                         double iTerrenoZ,
                         double iTerrenoSlope,
                         ISeccionCalzada iSeccionCalzada,
                         IzonaMovimientoTierras iZonaMovimientoTierras,
                         IzonaPuentes iZonaPuentes,
                         IzonaTuneles iZonaTuneles,
                         bool iObligadoEstructura)
        {
            this.idPto = iIdPto;
            this.ptoRoad = iPtoRoad;
            this.longitudSeccion = iLonSeccion;
            this.terrenoZ = iTerrenoZ;
            this.terrenoSlope = iTerrenoSlope;
            this.seccionCalzada = iSeccionCalzada;
            this.zonaMovimientoTierras = iZonaMovimientoTierras;
            this.zonaPuente = iZonaPuentes;
            this.zonaTunel = iZonaTuneles;
            this.isObligadoEstructura = iObligadoEstructura;
        }

        #endregion
        #region "Valoraciones - Costes"
        /// <summary>
        /// Valoración de la Pendiente Terreno [0-10]
        /// </summary>
        public double valoracionSlopeUnitaria()
        {
            return valoracionSlopeUnitariaByPendiente(this.terrenoSlope.Value);
        }
        /// <summary>
        /// Valoracion de la Pendiente por Sección
        /// </summary>
        public double valoracionSlopeBySeccion()
        {
            return this.valoracionSlopeUnitaria() * this.longitudSeccion.Value;
        }
        /// <summary>
        /// Coste Total Implantacion Seccion
        /// </summary>
        public double costeImplantacionTotalBySeccion()
        {
            double miCosteTotal = this.costeCalzadaBySeccion() +
                                    this.costeDesmonteBySeccion() +
                                    this.costeTerraplenBySeccion() +
                                    this.costePuenteBySeccion() +
                                    this.costeTunelBySeccion();

            return miCosteTotal;
        }
        /// <summary>
        /// Coste Calzada por Seccion
        /// </summary>
        public double costeCalzadaBySeccion()
        {

            if (this.seccionTipo.Value == eRoadSeccion.calzada)
            {
                return this.longitudSeccion.Value * (seccionCalzada.anchoPlataforma.Value * seccionCalzada.costeUnitario.Value);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Coste Desmonte por Seccion
        /// </summary>
        public double costeDesmonteBySeccion()
        {

            if (this.seccionTipo.Value == eRoadSeccion.calzada && this.excavacionTipo == eExcavacion.desmonte)
            {
                double miDesmonte = (this.seccionCalzada.anchoPlataforma.Value * Math.Abs(this.alturaMovimientoTierrasAbs)) + (this.zonaMovimientoTierras.desmonteTaludProyecto.Value * Math.Abs(this.alturaMovimientoTierrasAbs));

                return miDesmonte * this.longitudSeccion.Value * this.zonaMovimientoTierras.desmonteCosteUnitario.Value;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Coste Terraplen por Sección
        /// </summary>
        public double costeTerraplenBySeccion()
        {

            if (this.seccionTipo.Value == eRoadSeccion.calzada && this.excavacionTipo == eExcavacion.terraplen)
            {

                double miTerraplen = (this.seccionCalzada.anchoPlataforma.Value * Math.Abs(this.alturaMovimientoTierrasAbs)) + (this.zonaMovimientoTierras.terraplenTaludProyecto.Value * Math.Abs(this.alturaMovimientoTierrasAbs));

                return miTerraplen * this.longitudSeccion.Value * this.zonaMovimientoTierras.terraplenCosteUnitario.Value;
            }
            else
            {

                return 0;
            }


        }
        /// <summary>
        /// Coste Tunel por Sección
        /// </summary>
        /// <returns></returns>
        public double costeTunelBySeccion()
        {

            if (this.seccionTipo.Value == eRoadSeccion.tunel)
            {
                return this.longitudSeccion.Value * this.zonaTunel.tunelCosteUnitario.Value;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Coste del Puente por Sección
        /// </summary>
        public double costePuenteBySeccion()
        {

            if (this.seccionTipo.Value == eRoadSeccion.puente)
            {
                double miCostePuente = this.zonaPuente.puenteCosteUnitario.Value * this.seccionCalzada.anchoPlataforma.Value;

                return this.longitudSeccion.Value * miCostePuente;
            }
            else
            {
                return 0;
            }



        }

        #endregion
        #region "Metodos"



        public void draw(string iCapa)
        {
            engCadNet.oCircle.addCircle2D(this.ptoRoad.toArray3dZcero(), 0.02, iCapa);
            engCadNet.oMTexto.addMText2D(this.info(), this.ptoRoad.toArray3dZcero(), 0.02, 0, iCapa);
        }

        public string info()
        {

            StringBuilder miInfo = new StringBuilder();
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Sección : " + this.seccionTipo.ToString().ToUpper());
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Pto : " + this.idPto.Value.ToString());
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Zterreno : " + this.terrenoZ.Value.roundOff(3));
            miInfo.AppendLine("Zcalzada : " + this.ptoRoad.Z.roundOff(3));
            miInfo.AppendLine("Movimiento Tierras : " + this.excavacionTipo.ToString());
            miInfo.AppendLine("Altura Movimiento Tierra (ABS) : " + this.alturaMovimientoTierrasAbs.roundOff(3));
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Zona Movimiento Tierras : " + this.zonaMovimientoTierras.nombre);
            miInfo.AppendLine("Altura Máxima Desmonte (Cálculo) : " + this.zonaMovimientoTierras.desmonteAlturaMaximaCalculo.Value.roundOff(2));
            miInfo.AppendLine("Talud Desmonte : " + this.zonaMovimientoTierras.desmonteTaludProyecto.Value.roundOff(2));
            miInfo.AppendLine("Altura Máxima Terraplen (Cálculo) : " + this.zonaMovimientoTierras.terraplenAlturaMaximaCalculo.Value.roundOff(2));
            miInfo.AppendLine("Talud Terraplen : " + this.zonaMovimientoTierras.terraplenTaludProyecto.Value.roundOff(2));
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Zona Puentes : " + this.zonaPuente.nombre);
            miInfo.AppendLine("Permitir Puentes : " + this.zonaPuente.allowPuentes.Value.ToString());
            if (this.zonaPuente.allowPuentes.Value)
            {
                miInfo.AppendLine(" Altura Máxima Pila (Cálculo) : " + this.zonaPuente.puenteAlturaMaximaCalculo.ToString());
            }
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Zona Tuneles : " + this.zonaTunel.nombre);
            miInfo.AppendLine("Permitir Tuneles : " + this.zonaTunel.allowTuneles.Value.ToString());
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("VALORACIONES");
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Valoracion Pendiente Terreno : ");
            miInfo.AppendLine("Pendiente Terreno : " + this.terrenoSlope.Value.roundOff(4));
            miInfo.AppendLine("Valoración Pendiente Terreno [Unitaria] : " + this.valoracionSlopeUnitaria().roundOff(4));
            miInfo.AppendLine("Valoración Pendiente Terreno [Sección] : " + this.valoracionSlopeBySeccion().roundOff(4));
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Valoracion Coste Implantación : ");
            miInfo.AppendLine("Longitud Sección : " + this.longitudSeccion.Value.roundOff(2));
            miInfo.AppendLine("Ancho Plataforma : " + this.seccionCalzada.anchoPlataforma.Value.roundOff(2));
            miInfo.AppendLine("Coste Plataforma [Unitario] : " + this.seccionCalzada.costeUnitario.Value.roundOff(2));
            miInfo.AppendLine("Coste Desmonte [Unitario] : " + this.zonaMovimientoTierras.desmonteCosteUnitario.Value.roundOff(2));
            miInfo.AppendLine("Coste Terraplen [Unitario] : " + this.zonaMovimientoTierras.terraplenCosteUnitario.Value.roundOff(2));
            if (this.zonaTunel.allowTuneles.Value)
            {
                miInfo.AppendLine("Coste Tunel [Unitario] : " + this.zonaTunel.tunelCosteUnitario.Value.roundOff(2));
            }
            if (this.zonaPuente.allowPuentes.Value)
            {
                miInfo.AppendLine("Coste Puente [Unitario] : " + this.zonaPuente.puenteCosteUnitario.Value.roundOff(2));
            }

            miInfo.AppendLine("Coste Calzada [Sección] : " + this.costeCalzadaBySeccion().roundOff(0));
            miInfo.AppendLine("Coste Desmonte [Sección] : " + this.costeDesmonteBySeccion().roundOff(0));
            miInfo.AppendLine("Coste Terraplen [Sección] : " + this.costeTerraplenBySeccion().roundOff(0));

            if (this.zonaTunel.allowTuneles.Value)
            {
                miInfo.AppendLine("Coste Tunel [Sección] : " + this.costeTunelBySeccion().roundOff(0).ToString());
            }
            if (this.zonaPuente.allowPuentes.Value)
            {
                miInfo.AppendLine("Coste Puente [Sección] : " + this.costePuenteBySeccion().roundOff(0).ToString());
            }


            miInfo.AppendLine("Coste Implantacion Total [Sección] : " + this.costeImplantacionTotalBySeccion().roundOff(0).ToString());

            return miInfo.ToString();

        }



        #endregion
        #region "MetodosEstatico"

        public static double valoracionSlopeUnitariaByPendiente(double iPendientePU)
        {

            double miPendienteTerrenoRadianes = Math.Atan(iPendientePU);

            double miPendienteTerrenoGrados = oTrigo.getGradosFromRadianes(miPendienteTerrenoRadianes);


            if (miPendienteTerrenoGrados >= 0 && miPendienteTerrenoGrados < 15)
            {

                return getValoracionTerrenoPendiente(miPendienteTerrenoGrados, 15, 0, 10, 6.5);

            }
            else if (miPendienteTerrenoGrados >= 15 && miPendienteTerrenoGrados < 30)
            {
                return getValoracionTerrenoPendiente(miPendienteTerrenoGrados, 30, 15, 6.5, 3.5);

            }
            else if (miPendienteTerrenoGrados >= 30 && miPendienteTerrenoGrados < 45)
            {
                return getValoracionTerrenoPendiente(miPendienteTerrenoGrados, 45, 30, 3.5, 1);

            }
            else if (miPendienteTerrenoGrados >= 45 && miPendienteTerrenoGrados < 60)
            {
                return getValoracionTerrenoPendiente(miPendienteTerrenoGrados, 60, 45, 1, 0);

            }
            else if (miPendienteTerrenoGrados >= 60)
            {
                return 0;
            }
            else
            {
                throw new oExValorUserNoImplementado(iPendientePU.ToString());
            }


        }
        private static double getValoracionTerrenoPendiente(double iPendienteGrados, double iGradoMax, double iGradoMin, double iNotaMax, double iNotaMin)
        {

            #region "Validaciones"

            if (iNotaMin == iNotaMax | iNotaMin > iNotaMax)
            {
                throw new Exception(string.Format(strError.eAsigancionValoresMaxMin, iNotaMax.roundOff(2), iNotaMin.roundOff(2)));
            }

            if (iGradoMax == iGradoMin | iGradoMin > iGradoMax)
            {
                throw new Exception(string.Format(strError.eAsigancionValoresMaxMin, iGradoMax.roundOff(2), iGradoMin.roundOff(2)));
            }

            #endregion


            double miValorByGrado = (iNotaMax - iNotaMin) / (iGradoMax - iGradoMin);

            double miIncrementoGrado = iPendienteGrados - iGradoMin;

            double miNotaIncrementoGrado = miIncrementoGrado * miValorByGrado;

            double miNota = iNotaMax - miNotaIncrementoGrado;

            return miNota;
        }
        #endregion
    }

    public class oSeccionEjeTrazado : oSeccionEje
    {


        private bool mHasIncumplimiento ;
        private string mInfoIncumplimiento;


        #region "Constructor"

        public oSeccionEjeTrazado(double iPk, oP3d iPtoRoad, double iTerrenoZ, double iLongitudSeccion)
            : base(iPtoRoad, iTerrenoZ,iLongitudSeccion)
        {
            this.pk = iPk;
  
        }


        #endregion
        #region "Propiedades"
        public double? pk { get; set; }
        public oZonaGeoMovimientoTierras zonaMovimientoTierras { get; set; }
        public oZonaGeoCimentacion zonaCimentacion { get; set; }
        public oZonaGeoEstructuras zonaPuentes { get; set; }
        public oZonaGeoTuneles zonaTuneles { get; set; }
        public bool hasIncumplimiento
        { 
           get
           {
               return mHasIncumplimiento;
           }
        }
        public string infoIncumplimiento
        { 
            get
            {
               return mInfoIncumplimiento;
            }
        }
        #endregion
        #region "Metodos"

        /// <summary>
        /// Calcular la Sección Tipo y Las Zonas Gis
        /// </summary>
        public void calculateSeccionTipoAndZonasDesign(oEstudioInformativoCarretera iEstudioCarreteras)
        {
           this.seccionTipo = iEstudioCarreteras.getSeccionTipoProyecto(this.ptoRoad.convertTo2d(), this.terrenoZ.Value, this.ptoRoad.Z, out mHasIncumplimiento, out mInfoIncumplimiento);

                             
                        
           this.zonaMovimientoTierras = iEstudioCarreteras.getZonaMovimientoTierrasByPto(this.ptoRoad);
           this.zonaTuneles = iEstudioCarreteras.getZonaTunelesByPto(this.ptoRoad);
           this.zonaPuentes = iEstudioCarreteras.getZonaPuentesByPto(this.ptoRoad);
           this.zonaCimentacion = iEstudioCarreteras.getZonaCimentacion(this.ptoRoad);
                     
        }


      
        public void draw(string iCapa)
        {
            engCadNet.oCircle.addCircle2D(this.ptoRoad.toArray3dZcero(), 0.02, iCapa);
            engCadNet.oMTexto.addMText2D(this.info(), this.ptoRoad.toArray3dZcero(), 0.02, 0, iCapa);
        }

        public string info()
        {

            StringBuilder miInfo = new StringBuilder();
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Pk : " + this.pk.Value.ToString());
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Zterreno : " + this.terrenoZ.Value.roundOff(3));
            miInfo.AppendLine("Zcalzada : " + this.ptoRoad.Z.roundOff(3));
            miInfo.AppendLine("----------------------------------------");
            miInfo.AppendLine("Sección : " + this.seccionTipo.Value.ToString());
            miInfo.AppendLine("Zona Movimiento Tierras : " + this.zonaMovimientoTierras.nombre);
            miInfo.AppendLine("Zona Cimentación : " + this.zonaCimentacion.nombre);
            miInfo.AppendLine("Zona Tuneles : " + this.zonaTuneles.nombre);
            miInfo.AppendLine("Zona Puentes : " + this.zonaPuentes.nombre);


            return miInfo.ToString();

        }

        #endregion
    }

   




}
