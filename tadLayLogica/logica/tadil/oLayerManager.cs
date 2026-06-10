using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Colors;
    using engCadNet;

    using tadLayLogica.datos.proyecto;
  
    using tadLayLogica.zonaGis;


    /// <summary>
    /// AUTOCAD LAYER MANAGER
    /// </summary>
    public class oLayerManager
    {

        private const string KPrefijoProyecto = "_Tadil_";
        private const string KPrefijoSolucion = "Sol_";
        private const string KPrefijoZonasGis  = "Gis_";

        private oTadilLayer mTadil_No = null;
        private oTadilLayer mPuntosTarget = null;
        private oTadilLayer mVisibilidadEje = null;
        private oTadilLayer mVisibilidadGrafo = null;
        private oTadilLayer mAnalisisPendiente = null;


        private oTadilLayer mZonaNoPasoUsuario = null;
        private oTadilLayer mZonaNoPasoPendiente = null;
        private oTadilLayer mAbanicoTramos = null;
        private oTadilLayer mAbanicoTramosAux = null;
        private oTadilLayer mAbanicoSecciones = null;
        private oTadilLayer mAbanicoTramosGanadores = null;
        private oTadilLayer mTramoSalidaLLegada = null;

        private oTadilLayer mZonaNoPasoUsuario_Temp = null;

        #region "CONSTRUCTOR"

        public oLayerManager()
        {

        }

        #endregion
        #region "PROPIEDADES"

        /// <summary>
        /// _Tadil_Sol_
        /// </summary>
        public string prefijoSoluciones
        {
            get
            {
                return KPrefijoProyecto + KPrefijoSolucion;
            }
        }
        public string prefijoCapasZonasGis
        {
            get
            {
                return KPrefijoProyecto + KPrefijoZonasGis;
            }
        }

        public oTadilLayer zonaNoPasoUsuario_Temp
        {
            get
            {
                if (mZonaNoPasoUsuario_Temp == null)
                {
                    mZonaNoPasoUsuario_Temp = new oTadilLayer("_Tadil_ZonaNoPasoUsuario_Temp", 25);
                }

                return mZonaNoPasoUsuario_Temp;


            }
            set { mZonaNoPasoUsuario = value; }
        }

        public oTadilLayer zonaNoPasoUsuario
        {
            get
            {
                if (mZonaNoPasoUsuario == null)
                {
                    mZonaNoPasoUsuario = new oTadilLayer("_Tadil_ZonaNoPasoUsuario", 30);
                }

                return mZonaNoPasoUsuario;


            }
            set { mZonaNoPasoUsuario = value; }
        }
        public oTadilLayer zonaNoPasoPendiente
        {
            get
            {
                if (mZonaNoPasoPendiente == null)
                {
                    mZonaNoPasoPendiente = new oTadilLayer("_Tadil_ZonaNoPasoPendiente", 10);
                }


                return mZonaNoPasoPendiente;
            }
            set { mZonaNoPasoPendiente = value; }
        }
        public oTadilLayer visibilidadEje
        {

            get
            {

                if (mVisibilidadEje == null)
                {
                    mVisibilidadEje = new oTadilLayer("_Tadil_VisibilidadEje", 4);
                }

                return mVisibilidadEje;
            }
            set { mVisibilidadEje = value; }
        }
        public oTadilLayer visibilidadGrafo
        {
            get
            {
                if (mVisibilidadGrafo == null)
                {
                    mVisibilidadGrafo = new oTadilLayer("_Tadil_VisibilidadGrafo", 5);
                }

                return mVisibilidadGrafo;
            }
            set { mVisibilidadGrafo = value; }
        }
        public oTadilLayer analisisPendiente
        {
            get
            {
                if (mAnalisisPendiente == null)
                {
                    mAnalisisPendiente = new oTadilLayer("_Tadil_AnalisisPendiente", 0);
                }

                return mAnalisisPendiente;
            }
            set { mAnalisisPendiente = value; }


        }
        public oTadilLayer abanicoTramos
        {
            get
            {
                if (mAbanicoTramos == null)
                {
                    mAbanicoTramos = new oTadilLayer("_Tadil_AbanicoTramos", 4);
                }

                return mAbanicoTramos;


            }
            set { mAbanicoTramos = value; }
        }

        public oTadilLayer abanicoTramosAuxiliar
        {
            get
            {
                if (mAbanicoTramosAux == null)
                {
                    mAbanicoTramosAux = new oTadilLayer("_Tadil_AbanicoTramos_Auxiliar", 4);
                }

                return mAbanicoTramosAux;


            }
            set { mAbanicoTramos = value; }
        }
        public oTadilLayer abanicoSecciones
        {
            get
            {
                if (mAbanicoSecciones == null)
                {
                    mAbanicoSecciones = new oTadilLayer("_Tadil_AbanicoSecciones", 4);
                }

                return mAbanicoSecciones;


            }
            set { mAbanicoSecciones = value; }
        }
        public oTadilLayer abanicoTramosGanadores
        {
            get
            {
                if (mAbanicoTramosGanadores == null)
                {
                    mAbanicoTramosGanadores = new oTadilLayer("_Tadil_AbanicoTramosGanadores", 4);
                }

                return mAbanicoTramosGanadores;


            }
            set { mAbanicoTramosGanadores = value; }
        }
        public oTadilLayer tramoSalidaLlegada
        {
            get
            {
                if (mTramoSalidaLLegada == null)
                {
                    mTramoSalidaLLegada = new oTadilLayer("_Tadil_TramoSalidaLlegada",0);
                }

                return mTramoSalidaLLegada;


            }
            set { mTramoSalidaLLegada = value; }
        }

        #endregion
        #region "METODOS PUBLICOS"

        public List<string> getLstCapasBySolucion(string iNombreSolucion)
        {
            List<string> miLstCapasBySolucion = oLayer.getLayerListStartsWith(this.prefijoSoluciones + iNombreSolucion, false);

            return miLstCapasBySolucion;
        }
        public List<string> getLstCapasNombreGisByGrupoAndCode(eGisGrupos iZonaGrupo, eGisZonas iZonaCode)
        {

            return oLayer.getLayerListStartsWith(prefijoCapasZonasGis + iZonaGrupo.ToString() + "_" + iZonaCode.ToString(), false);
        }
        public List<string> getLstCapasNombreGisByGrupo(eGisGrupos iZonaGrupo)
        {
            return oLayer.getLayerListStartsWith(prefijoCapasZonasGis + iZonaGrupo.ToString(), false);
        }
        public List<string> getLstCapasNombreGis
        {
            get
            {
                return oLayer.getLayerListStartsWith(prefijoCapasZonasGis, false);
            }
        }
        public List<string> getLstCapasNombreGrupoGeotecnia
        {
            get
            {
                return oLayer.getLayerListStartsWith(prefijoCapasZonasGis + eGisGrupos.GEO.ToString(), false);
            }
        }


        public List<string> getLstCapasExpropiacionSocioeconomicos
        {
            get
            {
                return oLayer.getLayerListStartsWith(prefijoCapasZonasGis + eGisGrupos.SOC.ToString(), false);
            }

        }
        public List<string> getLstCapasExpropiacionPatrimoniales
        {
            get
            {
                List<string> miLstCapasBuscar = new List<string>();
                miLstCapasBuscar.Add(prefijoCapasZonasGis + eGisGrupos.PAT.ToString() + "_" + eGisZonas.URBANO);   
                miLstCapasBuscar.Add(prefijoCapasZonasGis + eGisGrupos.PAT.ToString() + "_" + eGisZonas.URBANI);  
                miLstCapasBuscar.Add(prefijoCapasZonasGis + eGisGrupos.PAT.ToString() + "_" + eGisZonas.NOURBA); 

                return oLayer.getLayerListContains(miLstCapasBuscar, false);
            }
        }


      

        /// <summary>
        /// Obtener el listado de Capas GIS Zonas Valoración (AMB-CLI-SOC-PAT)
        /// </summary>
        public List<string> getLstCapasGisZonasValoracion()
        {

            List<string> miLstCapas = new List<string>();
            List<string> miLstCapasAmbientales = this.getLstCapasNombreGisByGrupo(eGisGrupos.AMB);
            List<string> miLstCapasClimaticas = this.getLstCapasNombreGisByGrupo(eGisGrupos.CLI);
            List<string> miLstCapasSocioEconomicas = this.getLstCapasNombreGisByGrupo(eGisGrupos.SOC);
            List<string> miLstCapasPatrimoniales = this.getLstCapasNombreGisByGrupo(eGisGrupos.PAT);

            miLstCapas.AddRange(miLstCapasAmbientales);
            miLstCapas.AddRange(miLstCapasClimaticas);
            miLstCapas.AddRange(miLstCapasSocioEconomicas);
            miLstCapas.AddRange(miLstCapasPatrimoniales);

            return miLstCapas;
        }


        public void zonasGisOn()
        {
            zonasGisOnOff(false, false);
        }
        public void zonasGisOff()
        {
            zonasGisOnOff(true, false);
        }
        public void zonasGeotecniaOn()
        {
            zonaGisGeotecniaOnOff(false, false);
        }
        public void zonasGeotecniaOff()
        {
            zonaGisGeotecniaOnOff(true, false);
        }

        public void zonasExpropiacionOn()
        {
            this.zonasExpropiacionOnOff(false, false);
        }

        public void zonasExpropiacionOff()
        {
            this.zonasExpropiacionOnOff(true, false);
        }


        /// <summary>
        /// Activo/Desactivo las Capas de Geotecnia
        /// </summary>
        private void zonaGisGeotecniaOnOff(bool iIsOff, bool iIsLocked)
        {
            List<string> miLstCapasGeotecnia = getLstCapasNombreGrupoGeotecnia;
            oLayer.vLayerListActDes(miLstCapasGeotecnia, iIsOff, iIsLocked);
        }

        /// <summary>
        /// Activo/Desactivo todas las Capas Zonas GIS
        /// </summary>
        private void zonasGisOnOff(bool iIsOff, bool iIsLocked)
        {
            List<string> miLstCapasGis = oLayer.getLayerListStartsWith(prefijoCapasZonasGis, false);
            oLayer.vLayerListActDes(miLstCapasGis, iIsOff, iIsLocked);
        }


        private void zonasExpropiacionOnOff (bool iIsOff, bool iIsLocked)
        {

            List<string> miLstCapasExpropiacion = this.getLstCapasExpropiacionSocioeconomicos;
            miLstCapasExpropiacion.AddRange(this.getLstCapasExpropiacionPatrimoniales);

            oLayer.vLayerListActDes(miLstCapasExpropiacion, iIsOff, iIsLocked);

        }

        #endregion

    }




}
