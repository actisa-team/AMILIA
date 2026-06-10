using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{
    
    using Autodesk.AutoCAD.DatabaseServices;

    using tadLayShare.puntos;
    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.zonaGis;
    using tadLayLogica.Secciones.Calzada;
    using tadLayLogica.Secciones.Geometria;
      

    public class oSingletonProyecto : IDisposable

    {


       private static oSingletonProyecto mInstance = null;


       private eEstudioTipo? mEstudioTipo = null;
      
       private oZonaGeoMovimientoTierras mZonaGisDefaultMovimientoTierras = null;
       private oZonaGeoTuneles mZonaGisDefaultTuneles = null;
       private oZonaGeoEstructuras mZonaGisDefaultEstructuras = null;
       private oZonaGeoCimentacion mZonaGisDefaultCimentacion = null;


      


        #region "Constructor"
        private oSingletonProyecto ()
       {



       }
        public static oSingletonProyecto getInstance
       {

           get
           {
               if (mInstance == null)
               {
                   mInstance = new oSingletonProyecto();

                   oTadilCore.evReset += new EventHandler<engNet.eventos.oEventArgs<bool>>(oTadilCore_evReset);
               }

               return mInstance;
           }
       }

        static void oTadilCore_evReset(object sender, engNet.eventos.oEventArgs<bool> e)
        {
            if (e.Value)
            {
                if (mInstance != null)
                {
                    mInstance.Dispose();
                }
                   
                 mInstance = null;
                oTadilCore.evReset -= new EventHandler<engNet.eventos.oEventArgs<bool>>(oTadilCore_evReset);      
            }
        }


   


        #endregion



        #region "Propiedades"

        /// <summary>
        /// Tipo de Proyecto
        /// </summary>
        public eEstudioTipo estudioTipo
        {
            get
            {
                if (mEstudioTipo == null)
                {
                    mEstudioTipo = oDalTbProyecto.getEstudioTipo();
                }

                return mEstudioTipo.Value;

            }

        }
        /// <summary>
        /// Punto Salida
        /// </summary>
        public oP3dSalidaLlegada ptoSalida
        {
            get
            {
                return oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoSalida);                 
            }

        }
        /// <summary>
        /// Punto LLegada
        /// </summary>
        public oP3dSalidaLlegada ptoLlegada
        {
            get
            {
                 return oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoLlegada);
            }
        }
       /// <summary>
       /// Grupo Carretera
       /// </summary>
        private Guid? mSeccionActivaId = null;

        /// <summary>
        /// GUID de la sección activa. Si es null, se usará "APP" por defecto.
        /// </summary>
        public Guid? seccionActivaId
        {
            get { return mSeccionActivaId; }
            set { mSeccionActivaId = value; }
        }

        private string ObtenerKeyBusqueda()
        {
            if (this.SeccionesVinculadas && this.seccionActivaId.HasValue)
            {
                return this.seccionActivaId.Value.ToString();
            }
            return "APP";
        }

        /// <summary>
        /// Grupo Carretera
        /// </summary>
        public  eSecRoadGrupo secRoadGrupo
         {

            get
             {                       
                     string key = ObtenerKeyBusqueda();
                     string miRoadGrupo = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid(key).idRoadGrupo;

                     eSecRoadGrupo miSecRoadGrupo = (eSecRoadGrupo)Enum.Parse(typeof(eSecRoadGrupo), miRoadGrupo, true);

                     return miSecRoadGrupo;
             }
         }
        /// <summary>
        /// Tipo Carretera
        /// </summary>
        public eSecRoadTipo secRoadTipo
        {
            get
            {
                
                    string key = ObtenerKeyBusqueda();
                    string miRoadTipo = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid(key).idRoadTipo;

                    eSecRoadTipo  miSecRoadTipo = (eSecRoadTipo)Enum.Parse(typeof(eSecRoadTipo), miRoadTipo, true);

                    return miSecRoadTipo;
            }
        }
        /// <summary>
        /// Id Sección
        /// </summary>
        public Guid seccionCalzadaId
        {
             get
            { 
                 string key = ObtenerKeyBusqueda();
                 Guid  miIdSecRoad = oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid(key).idRoadSeccion;

                 return miIdSecRoad;
            }
        }
         public bool SeccionesVinculadas
         {
             get
             {
                 return oSingletonDsApp.getInstance.dataset.tbSeccionZonasGenerales.FindByid("APP").SeccionesVinculadas;
             }
         }
       
        public oSeccionRoadCompletaSinGis seccionCalzadaCompleta
       {
            get
           {
                 oSeccionRoadCompletaSinGis miSeccionCompletaCalzadaSinGis = tadLayLogica.Secciones.Calzada.oFactorySeccionCalzada.createSeccionRoad(this.secRoadTipo, this.seccionCalzadaId);

                 return miSeccionCompletaCalzadaSinGis;
           }

       }


        /// <summary>
        /// Sección Intervalo Metros
        /// </summary>
       public double seccionIntervalo
       {
           get
           {
             double  miSeccionIntervalo = (double) oSingletonDsApp.getInstance.proyecto.seccionIntervaloMetros;
             return miSeccionIntervalo;     
           }

       }
        public void Set_seccionIntervalo(double valor)
        {
            
                oSingletonDsApp.getInstance.proyecto.seccionIntervaloMetros=(int)valor;
        }


        public oZonaGeoMovimientoTierras zonaGisDefaultMovimientoTierras
        {
           get
            {
                if (mZonaGisDefaultMovimientoTierras == null)
                {
                    mZonaGisDefaultMovimientoTierras = (oZonaGeoMovimientoTierras) oFactoryZonaGis.createZonaGisGeoDefault(eGisZonas.MOVTIE);

                }

                return mZonaGisDefaultMovimientoTierras;
            }

        }
       public oZonaGeoTuneles zonaGisDefaultTuneles
       {

           get
           {
               if (mZonaGisDefaultTuneles == null)
               {
                   mZonaGisDefaultTuneles = (oZonaGeoTuneles)oFactoryZonaGis.createZonaGisGeoDefault(eGisZonas.TUNTUN);
               }

               return mZonaGisDefaultTuneles;
           }

       }
       public oZonaGeoEstructuras zonaGisDefaultEstructuras
       {
           get
           {
               if (mZonaGisDefaultEstructuras == null)
               {
                   mZonaGisDefaultEstructuras = (oZonaGeoEstructuras)oFactoryZonaGis.createZonaGisGeoDefault(eGisZonas.ESTEST);
               }

               return mZonaGisDefaultEstructuras;
           }

       }
       public oZonaGeoCimentacion zonaGisCimentacion
       {
           get
           {
               if (mZonaGisDefaultCimentacion == null)
               {
                   mZonaGisDefaultCimentacion = (oZonaGeoCimentacion)oFactoryZonaGis.createZonaGisGeoDefault(eGisZonas.ESTEST);
               }

               return mZonaGisDefaultCimentacion;
           }


       }

        #endregion


       #region "Metodos Publicos"


 

       #endregion







        public void Dispose()
        {
             mEstudioTipo = null;
             mZonaGisDefaultMovimientoTierras = null;
             mZonaGisDefaultTuneles = null;
             mZonaGisDefaultEstructuras = null;
             mZonaGisDefaultCimentacion = null;
             mInstance = null;
        }
    }
}
