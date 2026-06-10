using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.estudioTipo
{
    using System.Data;

    using engNet.Extension.DataRow;
    using engNet.Extension.String;
    using tadLayShare.puntos;

    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayLogica.datos.proyecto.estudioPrevio;
    using tadLayLogica.datos.proyecto;
    using tadLayLan;
    using tadLayLogica.zonaGis;
    using tadLayLogica.logica.Entidades;
    using tadLayShare;
    using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using engCadNet;
 

    public abstract class oEstudioCarretera : IEstudio
    {

        public oEstudioCarretera()
        {

        }


        public eRoadSeccion getSeccionTipoProyecto (IP2d iPtoSeccion, double iZterreno, double iZroad,out bool iHasIncumplimiento, out string iInfoIncumplimiento)
        {

            IzonaMovimientoTierras miZonaMovimientoTierras = this.getIZonaMovimientoTierrasByPto(iPtoSeccion);
            bool isOnZonaEstructuras = this.isOnZonaPasoObligadoEstructuras(iPtoSeccion);
            bool isOnZonaPuente = this.isOnZonaPasoObligadoPuente(iPtoSeccion);
            IzonaPuentes miZonaPuentes = this.getIZonaPuenteByPto(iPtoSeccion);
            IzonaTuneles miZonaTuneles = this.getIZonaTunelByPto(iPtoSeccion);

            eRoadSeccion miSeccionResultado = this.getSeccionTipoProyecto(iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles, out iHasIncumplimiento, out iInfoIncumplimiento);

            if (isOnZonaPuente)
            {
                miSeccionResultado = this.getSeccionTipoProyectoObligadoEstructuras(true, iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles, out iHasIncumplimiento, out iInfoIncumplimiento);

            }
            else if (isOnZonaEstructuras)
            {
                miSeccionResultado = this.getSeccionTipoProyectoObligadoEstructuras(false, iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles, out iHasIncumplimiento, out iInfoIncumplimiento);
            }
            return miSeccionResultado;

        }
        public eRoadSeccion getSeccionTipoProyecto(double iZterreno, double iZroad, IzonaMovimientoTierras iZonaMovimientoTierras, IzonaPuentes iZonaPuentes, IzonaTuneles iZonaTuneles, out bool iHasIncumplimiento, out string iInfoIncumplimiento)
        {

            eExcavacion miExcavacionTipo = getExcavacion(iZterreno, iZroad);
            double miAlturaMovimientoTierrasAbs = getAlturaMovimientoTierrasAbs(iZterreno, iZroad);


            if (miExcavacionTipo == eExcavacion.acota)
            {

                iHasIncumplimiento = false;
                iInfoIncumplimiento = string.Empty;

                return eRoadSeccion.calzada;

            }
            //DESMONTE--TUNEL
            else if (miExcavacionTipo == eExcavacion.desmonte)
            {

                if (miAlturaMovimientoTierrasAbs <= iZonaMovimientoTierras.desmonteAlturaMaximaCalculo.Value)
                {
                    iHasIncumplimiento = false;
                    iInfoIncumplimiento = string.Empty;

                    return eRoadSeccion.calzada;
                }
                else if (miAlturaMovimientoTierrasAbs > iZonaMovimientoTierras.desmonteAlturaMaximaCalculo.Value && !iZonaTuneles.allowTuneles.Value)
                {

                    iHasIncumplimiento = true;
                    iInfoIncumplimiento = "Altura Desmonte > Altura Desmonte Máxima [sin Estructuras]";

                    return eRoadSeccion.calzada;

                }
                else if (miAlturaMovimientoTierrasAbs > iZonaMovimientoTierras.desmonteAlturaMaximaCalculo.Value && iZonaTuneles.allowTuneles.Value)
                {

                    iHasIncumplimiento = false;
                    iInfoIncumplimiento = string.Empty;

                    return eRoadSeccion.tunel;
                }
                else
                {
                    throw new NotImplementedException("Opción No Implementada");
                }
            }
            //TERRAPLEN - PUENTE
            else if (miExcavacionTipo == eExcavacion.terraplen)
            {
                if (miAlturaMovimientoTierrasAbs <= iZonaMovimientoTierras.terraplenAlturaMaximaCalculo.Value)
                {

                    iHasIncumplimiento = false;
                    iInfoIncumplimiento = string.Empty;

                    return eRoadSeccion.calzada;
                }
                else if (miAlturaMovimientoTierrasAbs > iZonaMovimientoTierras.terraplenAlturaMaximaCalculo.Value && !iZonaPuentes.allowPuentes.Value)
                {

                    iHasIncumplimiento = true;
                    iInfoIncumplimiento = "Altura Terraplen > Altura Terraplen Máxima [sin Estructuras]";

                    return eRoadSeccion.calzada;

                }
                else if (miAlturaMovimientoTierrasAbs > iZonaMovimientoTierras.terraplenAlturaMaximaCalculo.Value && iZonaPuentes.allowPuentes.Value && miAlturaMovimientoTierrasAbs > iZonaPuentes.puenteAlturaMaximaCalculo.Value)
                {
                    iHasIncumplimiento = true;
                    iInfoIncumplimiento = "Altura Pila > Altura Pila Máxima";
                    return eRoadSeccion.puente;
                }

                else if (miAlturaMovimientoTierrasAbs > iZonaMovimientoTierras.terraplenAlturaMaximaCalculo && iZonaPuentes.allowPuentes.Value && miAlturaMovimientoTierrasAbs <= iZonaPuentes.puenteAlturaMaximaCalculo.Value)
                {

                    iHasIncumplimiento = false;
                    iInfoIncumplimiento = string.Empty;

                    return eRoadSeccion.puente;
                }
                else
                {
                    throw new NotImplementedException("Opción No Implementada");
                }
            }
            else
            {
                throw new oExEnumNotImplemented(miExcavacionTipo.ToString());
            }






        }


        public eRoadSeccion getSeccionTipoProyectoObligadoEstructuras(bool isObligadoPuente, double iZterreno, double iZroad, IzonaMovimientoTierras iZonaMovimientoTierras, IzonaPuentes iZonaPuentes, IzonaTuneles iZonaTuneles, out bool iHasIncumplimiento, out string iInfoIncumplimiento)
        {

            eExcavacion miExcavacionTipo = getExcavacion(iZterreno, iZroad);
            double miAlturaMovimientoTierrasAbs = getAlturaMovimientoTierrasAbs(iZterreno, iZroad);


            if (miExcavacionTipo == eExcavacion.acota)
            {
                if (iZonaPuentes.allowPuentes.Value)
                {
                    iHasIncumplimiento = false;
                    iInfoIncumplimiento = string.Empty;

                    return eRoadSeccion.puente;
                }
                else
                {
                    iHasIncumplimiento = true;
                    iInfoIncumplimiento = "Sección obligada a puente, zona con puentes no permitidos";

                    return eRoadSeccion.puente;
                }

            }
            //DESMONTE--TUNEL
            else if (miExcavacionTipo == eExcavacion.desmonte)
            {
                if (isObligadoPuente)
                {
                    if (iZonaPuentes.allowPuentes.Value)
                    {
                        iHasIncumplimiento = false;
                        iInfoIncumplimiento = string.Empty;

                        return eRoadSeccion.puente;
                    }
                    else
                    {
                        iHasIncumplimiento = true;
                        iInfoIncumplimiento = "Sección obligada a puente, zona con puentes no permitidos";

                        return eRoadSeccion.puente;
                    }
                }
                else
                {
                    if (iZonaTuneles.allowTuneles.Value)
                    {
                        iHasIncumplimiento = false;
                        iInfoIncumplimiento = string.Empty;

                        return eRoadSeccion.tunel;
                    }
                    else
                    {
                        iHasIncumplimiento = true;
                        iInfoIncumplimiento = "Sección obligada a tunle, zona con tuneles no permitidos";

                        return eRoadSeccion.tunel;
                    }
                }
            }
            //TERRAPLEN - PUENTE
            else if (miExcavacionTipo == eExcavacion.terraplen)
            {

                if (iZonaPuentes.allowPuentes.Value)
                {
                    iHasIncumplimiento = false;
                    iInfoIncumplimiento = string.Empty;

                    return eRoadSeccion.puente;
                }
                else
                {
                    iHasIncumplimiento = true;
                    iInfoIncumplimiento = "Sección obligada a puente, zona con puentes no permitidos";

                    return eRoadSeccion.puente;
                }
            }
            else
            {
                throw new oExEnumNotImplemented(miExcavacionTipo.ToString());
            }






        }


        public abstract oEstructura getPerfilLonSeccionInfo (double iPk, IP2d iPto, double iZterreno, double iZroad, out bool iHasIncumplimiento, out string iInfoIncumplimientos);
        
        




       
    
     
        #region "Interface"


        public virtual ISeccionCalzada getISeccionCalzadaByPto(IP2d iPto)
        {
            throw new NotImplementedException();
        }

        public virtual  IzonaMovimientoTierras getIZonaMovimientoTierrasByPto(IP2d iPto)
        {
            throw new NotImplementedException();
        }

        public virtual IzonaPuentes getIZonaPuenteByPto(IP2d iPto)
        {
            throw new NotImplementedException();
        }

        public virtual IzonaTuneles getIZonaTunelByPto(IP2d iPto)
        {
            throw new NotImplementedException();
        }

        public virtual bool isValidoCruceConDPH(IP2d iPto1, IP2d iPto2)
        {
            throw new NotImplementedException();
        }

        public virtual bool isValidoTramoDentroZonaDPH(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            throw new NotImplementedException();
        }


        public virtual bool isOnZonaPasoObligadoEstructuras(IP2d iPto)
        {
            throw new NotImplementedException();
        }
        public virtual bool isOnZonaPasoObligadoPuente(IP2d iPto)
        {
            throw new NotImplementedException();
        }

        public virtual bool isTramoObligadoAPuente(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            throw new NotImplementedException();
        }


        public virtual bool isTramoObligadoAPuenteoTunel(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            throw new NotImplementedException();
        }

        

       


        #endregion
        #region "Estaticos"

        protected static eExcavacion getExcavacion(double iZterreno, double iZroad)
        {

            if (iZroad == iZterreno)
            {
                return eExcavacion.acota;
            }
            else if (iZroad > iZterreno)
            {
                return eExcavacion.terraplen;
            }
            else if (iZroad < iZterreno)
            {
                return eExcavacion.desmonte;
            }
            else
            {
                throw new Exception("Caso Indeterminado¿?");
            }

        }

        protected static double getAlturaMovimientoTierrasAbs(double iZterreno, double iZroad)
        {
            return Math.Abs(iZterreno - iZroad);
        }

        #endregion


    }
   



    /// <summary>
    /// Datos Estudio Previo ; Generar el Eje Basico
    /// </summary>
    public class oEstudioPrevioCarretera : oEstudioCarretera
    {

        #region "Campos Privados"

        private Guid? mIdEstudioPrevio = null;
        private ISeccionCalzada mSeccionCalzadaDefault;
        private IzonaMovimientoTierras mZonaMovimientoTierrasDefault;
        private IzonaPuentes mZonaPuenteDefault;
        private IzonaTuneles mZonaTunelDefault;

        #endregion





        #region "Constructores"

        public oEstudioPrevioCarretera()
        {

        }


        public oEstudioPrevioCarretera(Guid iIdEstudioPrevio)
        {
            this.mIdEstudioPrevio = iIdEstudioPrevio;
            this.mSeccionCalzadaDefault = oDalTbSolucionEstudioPrevio.getSeccionCalzadaBySolucion(this.mIdEstudioPrevio.Value);
            this.mZonaMovimientoTierrasDefault = oDalTbSolucionEstudioPrevio.getZonaMovimientoTierrasBySolucion(this.mIdEstudioPrevio.Value);
            this.mZonaPuenteDefault = oDalTbSolucionEstudioPrevio.getZonaPuentesBySolucion(this.mIdEstudioPrevio.Value);
            this.mZonaTunelDefault = oDalTbSolucionEstudioPrevio.getZonaTunelesBySolucion(this.mIdEstudioPrevio.Value);
        }
        public oEstudioPrevioCarretera(ISeccionCalzada iSeccionCalzada, IzonaMovimientoTierras iZonaMovTierras, IzonaPuentes iZonaPuentes, IzonaTuneles iZonaTuneles)
        {
            this.mSeccionCalzadaDefault = iSeccionCalzada;
            this.mZonaMovimientoTierrasDefault = iZonaMovTierras;
            this.mZonaPuenteDefault = iZonaPuentes;
            this.mZonaTunelDefault = iZonaTuneles;
        }
        #endregion

        #region "Metodos Abstractos"


        //public override bool isValidoCruceConInf(IP2d iPto1, IP2d iPto2)
        //{
        //    return true;
        //}

        public override bool isValidoTramoDentroZonaDPH(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            return true;
        }

        public override bool isValidoCruceConDPH(IP2d iPto1, IP2d iPto2)
        {
            return true;
        }

        public override bool isOnZonaPasoObligadoEstructuras(IP2d iPto)
        {
            return false;
        }

        public override bool isTramoObligadoAPuente(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            return false;
        }


        public override bool isTramoObligadoAPuenteoTunel(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            return false;
        }

      
        

        public override oEstructura getPerfilLonSeccionInfo(double iPk, IP2d iPto, double iZterreno, double iZroad, out bool iHasIncumplimiento, out string iInfoIncumplimientos)
        {
            
            IzonaMovimientoTierras miZonaMovimientoTierras = this.getIZonaMovimientoTierrasByPto(iPto);
            IzonaPuentes miZonaPuentes = this.getIZonaPuenteByPto(iPto);
            IzonaTuneles miZonaTuneles = this.getIZonaTunelByPto(iPto);
            bool isOnZonaEstructuras = this.isOnZonaPasoObligadoEstructuras(iPto);
            bool isOnZonaPuente = this.isOnZonaPasoObligadoPuente(iPto);




            eRoadSeccion miRoadSeccion = getSeccionTipoProyecto(iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles,out iHasIncumplimiento,out iInfoIncumplimientos);
           
            if (isOnZonaPuente)
            {
                miRoadSeccion = getSeccionTipoProyectoObligadoEstructuras(true, iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles, out iHasIncumplimiento, out iInfoIncumplimientos);
            }
            else if (isOnZonaEstructuras)
            {
                miRoadSeccion = getSeccionTipoProyectoObligadoEstructuras(false, iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles, out iHasIncumplimiento, out iInfoIncumplimientos);
            }

            switch (miRoadSeccion)
            {
                case eRoadSeccion.calzada:
                    return new oEstructura(iPk, eRoadSeccion.calzada, strSeccionesTipo.uiCalzada);
                case eRoadSeccion.puente:
                    return new oEstructura(iPk, eRoadSeccion.puente, strSeccionesTipo.uiPuente);
                case eRoadSeccion.tunel:
                    return new oEstructura(iPk, eRoadSeccion.tunel, strSeccionesTipo.uiTunel);
                default:
                     throw new oExEnumNotImplemented(miRoadSeccion.ToString());
            }

          
        }



        #endregion

        #region "Interface IzonaProyecto"


        public override ISeccionCalzada getISeccionCalzadaByPto(IP2d iPto)
        {
            return mSeccionCalzadaDefault;
        }
        public override IzonaMovimientoTierras getIZonaMovimientoTierrasByPto(IP2d iPto)
        {
            return mZonaMovimientoTierrasDefault;
        }
        public override IzonaPuentes getIZonaPuenteByPto(IP2d iPto)
        {
            return mZonaPuenteDefault;
        }
        public override IzonaTuneles getIZonaTunelByPto(IP2d iPto)
        {
            return mZonaTunelDefault;
        }




        #endregion



    }



    public class oEstudioInformativoCarretera : oEstudioCarretera
    {

        private Guid mIdCalzada = Guid.Empty;
        private string mNombreCalzada = string.Empty;
        private double? mAnchoCalzada = null;

        public oCoeMinoracionAlturasMaximas mCoeMinAlturasMaximas { get; set; }

        private List<oZonaGis> mLstZonasMovimientoTierras=null;
        private List<oZonaGis> mLstZonasTuneles=null;
        private List<oZonaGis> mLstZonasPuentes=null;
        private List<oZonaGis> mLstZonasCimentacion = null;

        //lista de oZonaDPH
        private List<oZonaGis> mLstZonasDPH = null;
        private List<oZonaGis> mLstZonasDPHSinEje = null;
        private List<oZonaGis> mLstZonasDPHSoloEje = null;

        //lista de oZonaCruceInf
        private List<oZonaGis> mLstZonasCruceInf = null;
        private List<oZonaGis> mLstZonasINFSinEje = null;
        private List<oZonaGis> mLstZonasINFSoloEje = null;


        #region "Constructores"



        public oEstudioInformativoCarretera(eSecRoadTipo iRoadTipo, Guid iIdSeccion,  oCoeMinoracionAlturasMaximas iCoeficientesMinoracionAlturasMaximas,bool SeccionesVinculadas)
        {
            this.seccionTipo = iRoadTipo;
            this.seccionId = iIdSeccion;
            mCoeMinAlturasMaximas = iCoeficientesMinoracionAlturasMaximas;
            this.SeccionesVinculadas = SeccionesVinculadas;
        }


        #endregion

        #region "Propiedades Publicas"
        public eSecRoadTipo seccionTipo { get; set; }
        public Guid seccionId { get; set; }
        public bool SeccionesVinculadas { get; private set; }
        public double anchoCalzada
        {
            get
            {
                if (mAnchoCalzada == null)
                {
                    mAnchoCalzada = tadLayLogica.Secciones.Calzada.oFactorySeccionCalzada.getSeccionRoadAnchoPlataforma(this.seccionTipo, this.seccionId);
                }

                return mAnchoCalzada.Value;
            }
        }

        public List<oZonaGis> lstZonasMovimientoTierras
        {
            get
            {
                if (mLstZonasMovimientoTierras == null)
                {
                    mLstZonasMovimientoTierras = oFactoryZonaGis.getZonasGisProyectoByIdGrupoAndCode(eGisGrupos.GEO, eGisZonas.MOVTIE);
                }

                return mLstZonasMovimientoTierras;
            }

        }
        public List<oZonaGis> lstZonasTuneles
        {
            get
            {
                if (mLstZonasTuneles == null)
                {
                    mLstZonasTuneles = oFactoryZonaGis.getZonasGisProyectoByIdGrupoAndCode(eGisGrupos.GEO, eGisZonas.TUNTUN);
                }

                return mLstZonasTuneles;
            }

        }
        public List<oZonaGis> lstZonasPuentes
        {
            get
            {
                if (mLstZonasPuentes == null)
                {

                    mLstZonasPuentes = oFactoryZonaGis.getZonasGisProyectoByIdGrupoAndCode(eGisGrupos.PUE, eGisZonas.ESTEST);
                }

                return mLstZonasPuentes;
            }



        }
        public List<oZonaGis> lstZonasCimentacion
        {
            get
            {

                if (mLstZonasCimentacion == null)
                {

                    mLstZonasCimentacion = oFactoryZonaGis.getZonasGisProyectoByIdGrupoAndCode(eGisGrupos.GEO, eGisZonas.ESTCIM);

                }

                return mLstZonasCimentacion;
            }


        }
        public List<oZonaGis> lstZonasDPH
        {
            get
            {
                if (mLstZonasDPH == null)
                {
                    mLstZonasDPH = oFactoryZonaGis.getZonasGisProyectoByIdGrupoAndCode(eGisGrupos.AMB, eGisZonas.ZODOPU);
                }

                return mLstZonasDPH;
            }

        }

        public List<oZonaGis> lstZonasDPHsinEJE
        {
            get
            {
                if (mLstZonasDPHSinEje == null)
                {
                    List<string> miLstCapasGis = oTadil.data.Layer.getLstCapasNombreGisByGrupoAndCode(eGisGrupos.AMB, eGisZonas.ZODOPU);
                    List<string> miLstCapasSinEje = new List<string>();

                    foreach (string miCapa in miLstCapasGis)
                    {
                        if (!miCapa.Contains("EJE"))
                        {
                            miLstCapasSinEje.Add(miCapa);
                        }
                    }


                    List<Polyline> miLstLwGis = oSs.getSsLwByLayerListAndXdata(miLstCapasSinEje, eXdataKey.zonaGisGuid.ToString());
                    mLstZonasDPHSinEje = oFactoryZonaGis.getZonasGisFromLstPolineasGis(miLstLwGis);
                }

                return mLstZonasDPHSinEje;

            }
        }


        public List<oZonaGis> lstZonasDPHSoloEje
        {
            get
            {
                if (mLstZonasDPHSoloEje == null)
                {
                    List<string> miLstCapasGis = oTadil.data.Layer.getLstCapasNombreGisByGrupoAndCode(eGisGrupos.AMB, eGisZonas.ZODOPU);
                    List<string> miLstCapasSinEje = new List<string>();

                    foreach (string miCapa in miLstCapasGis)
                    {
                        if (miCapa.Contains("EJE"))
                        {
                            miLstCapasSinEje.Add(miCapa);
                        }
                    }


                    List<Polyline> miLstLwGis = oSs.getSsLwByLayerListAndXdata(miLstCapasSinEje, eXdataKey.zonaGisGuid.ToString());
                    mLstZonasDPHSoloEje = oFactoryZonaGis.getZonasGisFromLstPolineasGis(miLstLwGis);
                }

                return mLstZonasDPHSoloEje;

            }
        }
        public List<oZonaGis> lstZonasCruceInf
        {
            get
            {
                if (mLstZonasCruceInf == null)
                {
                    mLstZonasCruceInf = oFactoryZonaGis.getZonasGisProyectoByIdGrupoAndCode(eGisGrupos.PAT, eGisZonas.CRUINF);
                }

                return mLstZonasCruceInf;
            }

        }
        public List<oZonaGis> lstZonasINFsinEJE
        {
            get
            {
                if (mLstZonasINFSinEje == null)
                {
                    List<string> miLstCapasGis = oTadil.data.Layer.getLstCapasNombreGisByGrupoAndCode(eGisGrupos.PAT, eGisZonas.CRUINF);
                    List<string> miLstCapasSinEje = new List<string>();

                    foreach (string miCapa in miLstCapasGis)
                    {
                        if (!miCapa.Contains("EJE"))
                        {
                            miLstCapasSinEje.Add(miCapa);
                        }
                    }


                    List<Polyline> miLstLwGis = oSs.getSsLwByLayerListAndXdata(miLstCapasSinEje, eXdataKey.zonaGisGuid.ToString());
                    mLstZonasINFSinEje = oFactoryZonaGis.getZonasGisFromLstPolineasGis(miLstLwGis);
                }

                return mLstZonasINFSinEje;

            }
        }


        public List<oZonaGis> lstZonasINFSoloEje
        {
            get
            {
                if (mLstZonasINFSoloEje == null)
                {
                    List<string> miLstCapasGis = oTadil.data.Layer.getLstCapasNombreGisByGrupoAndCode(eGisGrupos.PAT, eGisZonas.CRUINF);
                    List<string> miLstCapasSinEje = new List<string>();

                    foreach (string miCapa in miLstCapasGis)
                    {
                        if (miCapa.Contains("EJE"))
                        {
                            miLstCapasSinEje.Add(miCapa);
                        }
                    }


                    List<Polyline> miLstLwGis = oSs.getSsLwByLayerListAndXdata(miLstCapasSinEje, eXdataKey.zonaGisGuid.ToString());
                    mLstZonasINFSoloEje = oFactoryZonaGis.getZonasGisFromLstPolineasGis(miLstLwGis);
                }

                return mLstZonasINFSoloEje;

            }
        }



        #endregion

        #region "Metodos Publicos"

        public oZonaGeoMovimientoTierras getZonaMovimientoTierrasByPto(IP2d iPto)
        {
            oZonaGeoMovimientoTierras miZonaMovTierrasByPk = (oZonaGeoMovimientoTierras)oFactoryZonaGis.createZonaGisGeo(iPto.toArray2d(), this.lstZonasMovimientoTierras, eGisZonas.MOVTIE);

            return miZonaMovTierrasByPk;
        }
        public oZonaGeoEstructuras getZonaPuentesByPto(IP2d iPto)
        {
            oZonaGeoEstructuras miZonaPuentes = (oZonaGeoEstructuras)oFactoryZonaGis.createZonaGisGeo(iPto.toArray2d(), this.lstZonasPuentes, eGisZonas.ESTEST);

            return miZonaPuentes;

        }
        public oZonaGeoTuneles getZonaTunelesByPto(IP2d iPto)
        {
            oZonaGeoTuneles miZonaTuneles = ( oZonaGeoTuneles)  oFactoryZonaGis.createZonaGisGeo(iPto.toArray2d(), this.lstZonasTuneles, eGisZonas.TUNTUN);

            return miZonaTuneles;
        }
        public oZonaGeoCimentacion getZonaCimentacion (IP2d iPto)
        {

            oZonaGeoCimentacion miZonaCimentacion = (oZonaGeoCimentacion)oFactoryZonaGis.createZonaGisGeo(iPto.toArray2d(), this.lstZonasCimentacion, eGisZonas.ESTCIM);

            return miZonaCimentacion;
        }

        public void Set_seccionTipo(eSecRoadTipo iRoadTipo, Guid iIdSeccion)
        {
            this.seccionTipo = iRoadTipo;
            this.seccionId = iIdSeccion;
        }

        #endregion


        #region "Metodos Publicos Abstractos"


        public override bool isTramoObligadoAPuente(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            bool miIsTramoObligado = false;

            bool miIntersecWithDPH = false;

            foreach (oZonaGis miZonaGis in lstZonasDPHsinEJE)
            {
                oZonaDominioHidraulico miZonaDPH = (oZonaDominioHidraulico)miZonaGis;
                if (miZonaDPH.isPtoInLwZona(new Point3d(miPEntradaTramo.X, miPEntradaTramo.Y, 0)) || miZonaDPH.isPtoInLwZona(new Point3d(miPSalidaTramo.X, miPSalidaTramo.Y, 0)) || miZonaDPH.intersecWith(miPEntradaTramo, miPSalidaTramo))
                {
                    miIntersecWithDPH = true;
                }
                if (miZonaDPH.obligacionEstructura && miIntersecWithDPH)
                {
                    miIsTramoObligado = true;
                }
            }

            if (!miIsTramoObligado)
            {
                if (miIntersecWithDPH)
                {
                    foreach (oZonaGis miZonaGis in lstZonasINFsinEJE)
                    {
                        oZonaCruInf miZonaInf = (oZonaCruInf)miZonaGis;
                        if (miZonaInf.obligacionEstructura && (miZonaInf.intersecWith(miPEntradaTramo, miPSalidaTramo) || miZonaInf.isPtoInLwZona(new Point3d(miPEntradaTramo.X, miPEntradaTramo.Y, 0)) || miZonaInf.isPtoInLwZona(new Point3d(miPSalidaTramo.X, miPSalidaTramo.Y, 0))))
                        {
                            miIsTramoObligado = true;
                        }
                    }
                }
            }

            return miIsTramoObligado;
        }

        public override bool isTramoObligadoAPuenteoTunel(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            bool miIsTramoObligado = false;

            bool miIntersecWithDPH = false;

            foreach (oZonaGis miZonaGis in lstZonasDPHsinEJE)
            {
                oZonaDominioHidraulico miZonaDPH = (oZonaDominioHidraulico)miZonaGis;
                if (miZonaDPH.isPtoInLwZona(new Point3d(miPEntradaTramo.X, miPEntradaTramo.Y, 0)) || miZonaDPH.isPtoInLwZona(new Point3d(miPSalidaTramo.X, miPSalidaTramo.Y, 0)) || miZonaDPH.intersecWith(miPEntradaTramo, miPSalidaTramo))
                {
                    miIntersecWithDPH = true;
                }
            }

            if (!miIntersecWithDPH)
            {
                foreach (oZonaGis miZonaGis in lstZonasINFsinEJE)
                {
                    oZonaCruInf miZonaInf = (oZonaCruInf)miZonaGis;
                    if (!miZonaInf.isZonaNoPaso)
                    {
                        if (miZonaInf.obligacionEstructura && (miZonaInf.intersecWith(miPEntradaTramo, miPSalidaTramo) || miZonaInf.isPtoInLwZona(new Point3d(miPEntradaTramo.X, miPEntradaTramo.Y, 0)) || miZonaInf.isPtoInLwZona(new Point3d(miPSalidaTramo.X, miPSalidaTramo.Y, 0))))
                        {
                            miIsTramoObligado = true;
                        }
                    }
                }
            }

            return miIsTramoObligado;
        }
        public override bool isOnZonaPasoObligadoEstructuras(IP2d iPto)
        {
            bool miIsOn = false;

            foreach (oZonaGis miZonaGis in lstZonasDPHsinEJE)
            {
                oZonaDominioHidraulico miZonaDPH = (oZonaDominioHidraulico)miZonaGis;
                if (miZonaDPH.obligacionEstructura && miZonaDPH.isOnZona(iPto))
                {
                    miIsOn = true;
                }
            }

            if (!miIsOn)
            {
                foreach (oZonaGis miZonaGis in lstZonasINFsinEJE)
                {
                    oZonaCruInf miZonaInf = (oZonaCruInf)miZonaGis;
                    if (miZonaInf.obligacionEstructura && miZonaInf.isOnZona(iPto))
                    {
                        miIsOn = true;
                    }
                }

            }

            return miIsOn;
        }

        public override bool isOnZonaPasoObligadoPuente(IP2d iPto)
        {
            bool miIsOn = false;

            foreach (oZonaGis miZonaGis in lstZonasDPHsinEJE)
            {
                oZonaDominioHidraulico miZonaDPH = (oZonaDominioHidraulico)miZonaGis;
                if (miZonaDPH.obligacionEstructura && miZonaDPH.isOnZona(iPto))
                {
                    miIsOn = true;
                }
            }

            return miIsOn;
        }

        public override bool isValidoTramoDentroZonaDPH(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
        {
            bool miIsValido = true;


            foreach (oZonaGis miZonaGis in lstZonasDPHsinEJE)
            {
                oZonaDominioHidraulico miZonaDPH = (oZonaDominioHidraulico)miZonaGis;
                if (!miZonaDPH.isValidoTramoDentroZona(miPEntradaTramo, miPSalidaTramo))
                {
                    miIsValido = false;
                }
            }

            return miIsValido;
        }

        public override bool isValidoCruceConDPH(IP2d iPto1, IP2d iPto2)
        {
            bool miIsValido = true;

            foreach (oZonaGis miZonaGis in lstZonasDPHSoloEje)
            {
                oZonaDominioHidraulico miZonaDPH = (oZonaDominioHidraulico)miZonaGis;
                if (!miZonaDPH.isCruceConTramoValido(iPto1, iPto2))
                {
                    miIsValido = false;
                }
            }

            return miIsValido;
        }


        public override oEstructura getPerfilLonSeccionInfo(double iPk, IP2d iPto, double iZterreno, double iZroad, out bool iHasIncumplimiento, out string iInfoIncumplimientos)
        {

            IzonaMovimientoTierras miZonaMovimientoTierras = this.getIZonaMovimientoTierrasByPto(iPto);
            IzonaPuentes miZonaPuentes = this.getIZonaPuenteByPto(iPto);
            IzonaTuneles miZonaTuneles = this.getIZonaTunelByPto(iPto);
            bool isOnZonaEstructuras = this.isOnZonaPasoObligadoEstructuras(iPto);
            bool isOnZonaPuente = this.isOnZonaPasoObligadoPuente(iPto);

            

            eRoadSeccion miRoadSeccion = getSeccionTipoProyecto(iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles,out iHasIncumplimiento,out iInfoIncumplimientos);
           
            if (isOnZonaPuente)
            {
                miRoadSeccion = getSeccionTipoProyectoObligadoEstructuras(true, iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles, out iHasIncumplimiento, out iInfoIncumplimientos);
            }
            else if (isOnZonaEstructuras)
            {
                miRoadSeccion = getSeccionTipoProyectoObligadoEstructuras(false, iZterreno, iZroad, miZonaMovimientoTierras, miZonaPuentes, miZonaTuneles, out iHasIncumplimiento, out iInfoIncumplimientos);
            }

            switch (miRoadSeccion)
            {
                case eRoadSeccion.calzada:
                    return new oEstructura(iPk,eRoadSeccion.calzada,strSeccionesTipo.uiCalzada);
                case eRoadSeccion.puente:
                    return   new oEstructura(iPk,eRoadSeccion.puente,miZonaPuentes.bloqueNombreSinExtension);
                case eRoadSeccion.tunel:
                    return new oEstructura(iPk, eRoadSeccion.tunel, miZonaTuneles.bloqueNombreSinExtension);
                default:
                    throw new oExEnumNotImplemented(miRoadSeccion.ToString());
            }


        }



        #endregion

        #region "Interface"

        public override  ISeccionCalzada getISeccionCalzadaByPto(IP2d iPto)
        {
           
            //Obtenemos la Zona Movimiento Tierras por Pk
            //Si no Obtenemos ninguna, devuelve la zona por defecto.
            oZonaGeoMovimientoTierras miZonaMovTierras = this.getZonaMovimientoTierrasByPto(iPto);

            ISeccionCalzada miRoadSeccion = getSeccionCalzada(miZonaMovTierras);

            return miRoadSeccion;


        }
        public override IzonaMovimientoTierras getIZonaMovimientoTierrasByPto(IP2d iPto)
        {

            //Obtenemos la Zona Movimiento Tierras por Pk
            //Si no Obtenemos ninguna, devuelve la zona por defecto.
            oZonaGeoMovimientoTierras miZonaMovTierras = this.getZonaMovimientoTierrasByPto(iPto);

            IzonaMovimientoTierras miMovimientoTierras = getZonaMovimientoTierras(miZonaMovTierras);

             return miMovimientoTierras;

        }
        public override IzonaPuentes getIZonaPuenteByPto(IP2d iPto)
        {
            //Obtenemos la Zona Movimiento Tierras por Pk
            //Si no Obtenemos ninguna, devuelve la zona por defecto.
            oZonaGeoEstructuras miZonaPuentes =  this.getZonaPuentesByPto(iPto);

            IzonaPuentes miEstudioPuentes = getZonaPuentes(miZonaPuentes);

            return miEstudioPuentes;

        }
        public override IzonaTuneles getIZonaTunelByPto(IP2d iPto)
        {
            //Obtenemos la Zona Movimiento Tierras por Pk
            //Si no Obtenemos ninguna, devuelve la zona por defecto.
            oZonaGeoTuneles miZonaTuneles =  this.getZonaTunelesByPto(iPto);

            IzonaTuneles miEstudioTuneles = getZonaTuneles(miZonaTuneles);

            return miEstudioTuneles;
        }

        #endregion

        #region "PropiedadesPrivadas"

        #endregion



        #region "Metodos Privados"
        private ISeccionCalzada getSeccionCalzada(oZonaGeoMovimientoTierras iZonaMovimientoTierras)
        {
            oEstudioRoadSeccion miEstudioRoadSeccion = new oEstudioRoadSeccion(this.mIdCalzada, this.mNombreCalzada, this.anchoCalzada, iZonaMovimientoTierras.getCosteImplantacionRoad());
            return miEstudioRoadSeccion;
        }
        private IzonaMovimientoTierras getZonaMovimientoTierras(oZonaGeoMovimientoTierras iZonaMovimientoTierras)
        {

            double miCosteUnitarioDesmonte;
            double miCosteUnitarioTerraplen;

            iZonaMovimientoTierras.getCosteDesmonteTerraplen(out miCosteUnitarioDesmonte, out miCosteUnitarioTerraplen);


            oEstudioMovimientoTierras miEstudioMovimientoTierras = new oEstudioMovimientoTierras(iZonaMovimientoTierras.id,
                                                                                                iZonaMovimientoTierras.nombre,
                                                                                                iZonaMovimientoTierras.row.desmonteAlturaMaxima,
                                                                                                mCoeMinAlturasMaximas.desmonteCoeficienteMinoracionAlturaMaxima,
                                                                                                iZonaMovimientoTierras.row.desmonteTalud,
                                                                                                miCosteUnitarioDesmonte,
                                                                                                iZonaMovimientoTierras.row.terraplenAlturaMaxima,
                                                                                                mCoeMinAlturasMaximas.terraplenCoeficienteMinoracionAlturaMaxima,
                                                                                                iZonaMovimientoTierras.row.terraplenTalud,
                                                                                                miCosteUnitarioTerraplen);


            return miEstudioMovimientoTierras;


        }
        private IzonaPuentes getZonaPuentes(oZonaGeoEstructuras iZonaPuentes)
        {
            oEstudioPuentes miEstudioPuentes;
            if(!iZonaPuentes.isAllowEstructuras)
                miEstudioPuentes = new oEstudioPuentes(iZonaPuentes.id,
                                                                    iZonaPuentes.nombre,
                                                                    iZonaPuentes.row.dwgName.removeExtension(),
                                                                    !iZonaPuentes.isAllowEstructuras,
                                                                    iZonaPuentes.alturaMaximaPila(),
                                                                    mCoeMinAlturasMaximas.pilaCoeficienteMinoracionAlturaMaxima,
                                                                    iZonaPuentes.costeUnitario());
            else
                miEstudioPuentes = new oEstudioPuentes(iZonaPuentes.id,
                                                                    iZonaPuentes.nombre,
                                                                    "",
                                                                    !iZonaPuentes.isAllowEstructuras,
                                                                    0,
                                                                    0,
                                                                    0);

            return miEstudioPuentes;

        }

        private IzonaTuneles getZonaTuneles(oZonaGeoTuneles iZonaTuneles)
        {
            oEstudioTuneles miEstudioTuneles;
            if (!iZonaTuneles.isAllowTuneles)
                miEstudioTuneles = new oEstudioTuneles(iZonaTuneles.id,
                    iZonaTuneles.nombre,
                    iZonaTuneles.row.dwgName.removeExtension(),
                    !iZonaTuneles.isAllowTuneles,
                    iZonaTuneles.costeUnitario());
            else
                miEstudioTuneles = new oEstudioTuneles(iZonaTuneles.id,
                                                                    iZonaTuneles.nombre,
                                                                    "",
                                                                    !iZonaTuneles.isAllowTuneles,
                                                                    0);
        

    return miEstudioTuneles;

        }






        #endregion
    }



    public class oEstudioRoadSeccion : ISeccionCalzada
    {

        public Guid? idSeccion { get; set; }
        public string nombre { get; set; }
        public double? anchoPlataforma { get; set; }
        public double? costeUnitario { get; set; }


        public oEstudioRoadSeccion(Guid iIdSeccion, string iCalzadaNombre, double iAnchoPlataforma, double iCosteUnitario)
        {
            this.idSeccion = iIdSeccion;
            this.nombre = iCalzadaNombre;
            this.anchoPlataforma = iAnchoPlataforma;
            this.costeUnitario = iCosteUnitario;
        }

    }
    public class oEstudioMovimientoTierras : IzonaMovimientoTierras
    {

        public Guid? idZonaMovimientoTierras {get;set;}
        public string nombre {get;set;}
    
        public double? desmonteAlturaMaximaProyecto{get;set;}
        public double? desmonteCoeMinoracion { get; set; }
        public double? desmonteAlturaMaximaCalculo { get { return this.desmonteAlturaMaximaProyecto.Value * this.desmonteCoeMinoracion.Value; } }
        public double? desmonteTaludProyecto { get; set; }
 

        public double? terraplenAlturaMaximaProyecto {get;set;}
        public double? terraplenCoeMinoracion { get; set; }
        public double? terraplenAlturaMaximaCalculo { get { return this.terraplenAlturaMaximaProyecto.Value * this.terraplenCoeMinoracion.Value; } }
        public double? terraplenTaludProyecto { get; set; }

        public double? desmonteCosteUnitario { get; set; }
        public double? terraplenCosteUnitario { get; set; }



        public oEstudioMovimientoTierras(Guid iIdZonaMovimientoTierras, 
                                               string iZonaNombre, 
                                               double iDesmonteAlturaMaximaProyecto,
                                               double iDesmonteCoeMinoracion,
                                               double iDesmonteTalud,
                                               double iDesmonteCosteUnitario,
                                               double iTerraplenAlturaMaximaProyecto,
                                               double iTerraplenCoeMinoracion,
                                               double iTerraplenTalud,
                                               double iTerraplenCosteUnitario)
        {

            this.idZonaMovimientoTierras = iIdZonaMovimientoTierras;
            this.nombre = iZonaNombre;
            this.desmonteAlturaMaximaProyecto = iDesmonteAlturaMaximaProyecto;
            this.desmonteCoeMinoracion = iDesmonteCoeMinoracion;
            this.desmonteTaludProyecto = iDesmonteTalud;
            this.desmonteCosteUnitario = iDesmonteCosteUnitario;

            this.terraplenAlturaMaximaProyecto = iTerraplenAlturaMaximaProyecto;
            this.terraplenCoeMinoracion = iTerraplenCoeMinoracion;
            this.terraplenTaludProyecto = iTerraplenTalud;
            this.terraplenCosteUnitario = iTerraplenCosteUnitario;
        }











    }
    public class oEstudioPuentes : IzonaPuentes
    {

        public Guid? idZonaPuentes {get;set;}
 
        public string nombre {get;set;}

        public string bloqueNombreSinExtension { get; set; }

        public bool? allowPuentes {get;set;}

        public double? puenteAlturaMaximaProyecto { get; set; }
        public double? puenteAlturaCoeMinoracion { get; set; }
        public double? puenteAlturaMaximaCalculo
        { 
            get 
            {
                if (this.puenteAlturaMaximaProyecto == null)
                {
                    return null;
                }
                else
                {
                    return this.puenteAlturaMaximaProyecto.Value * this.puenteAlturaCoeMinoracion.Value; 
                }  
            } 
        }
        public double? puenteCosteUnitario {get;set;}


        public oEstudioPuentes(Guid iIdPuente, string iPuenteNombre, string iBloquePuenteSinExtension, bool iAllowPuentes,double? iPilaAlturaMaximaProyecto, double? iPilaCoeMinoracion, double? iCosteUnirario)
        {
            this.idZonaPuentes = iIdPuente;
            this.nombre = iPuenteNombre;
            this.bloqueNombreSinExtension = iBloquePuenteSinExtension;
            this.allowPuentes = iAllowPuentes;
            this.puenteAlturaMaximaProyecto = iPilaAlturaMaximaProyecto;
            this.puenteAlturaCoeMinoracion = iPilaCoeMinoracion;
            this.puenteCosteUnitario = iCosteUnirario;      
        }




      
    }
    public class oEstudioTuneles : IzonaTuneles
    {

        public Guid? idZonaTuneles {get;set;}
    
        public string nombre {get;set;}

        public string bloqueNombreSinExtension { get; set; }

        public bool? allowTuneles {get;set;}

        public double? tunelCosteUnitario {get;set;}


        public oEstudioTuneles(Guid iIdTunel, string iNombre,string iBloqueTunelSinExtension, bool iAllowTuneles, double? iTunelCosteUnitarioKm)
        {
            this.idZonaTuneles = iIdTunel;
            this.nombre = iNombre;
            this.bloqueNombreSinExtension = iBloqueTunelSinExtension;
            this.allowTuneles = iAllowTuneles;
            this.tunelCosteUnitario = iTunelCosteUnitarioKm/1000;
        }
 
    }

}
