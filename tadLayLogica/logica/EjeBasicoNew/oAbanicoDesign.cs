using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{

    using Autodesk.AutoCAD.ApplicationServices;

    using engCadNet;

    using tadLayShare.puntos;
    using tadLayShare;
    
    
    public class oAbanicoDesign
    {

       private double? mAnguloTotalGrados = null;
       private double? mGradosDiscretizacionProyecto = null;
       private double? mTramoAbanicoDiscretizacion = null;
       private int? mNumeroTramos = null;
       private double? mToleranciaPtoTargetPC = null;
       private double? mSegundosEntronque = null;
       private bool? mIncluirDistTramos = null;
        private double? mLongitudMinimaTramo = null;


        private bool? mInvalidarTramosAvanceCortoByLongitud=null ;

       private double? mInvalidarTramosIncrementoLongitudPC = null;

       private eAbanicoTipo? mAbanicoTipo = null;




       public oAbanicoDesign(double iAnguloAbanicoTotal, 
                             double iGradosDiscretizacion, 
                             double iAbanicoTramoDiscretizacion, 
                             double iToleranciaPtoTargetPC,
                             bool iInvalidarTramosAbanicoPrimarioPorIncrementoLon,
                             double? iInvalidarTramosAbanicoPrimarioIncrementoLonPC,
                             eAbanicoTipo iTipoAbanico)
       {
           this.anguloTotalGrados = iAnguloAbanicoTotal;
           this.gradosDiscretizacionProyecto = iGradosDiscretizacion;
           this.tramoAbanicoDiscretizacion = iAbanicoTramoDiscretizacion;
           this.toleranciaPuntoObjetivoPC = iToleranciaPtoTargetPC;

           this.invalidarTramosAvanceCortoPorIncrementoLongitud   = iInvalidarTramosAbanicoPrimarioPorIncrementoLon;
           this.invalidarTramosIncrementoLongitudPC = iInvalidarTramosAbanicoPrimarioIncrementoLonPC;
           this.abanicoTipo = iTipoAbanico;
       }



        #region "Propiedades"

        public bool incluirTramosRectaMaxima;
        public bool incluirTramosRectaMinima;
        public bool incluirTramosRectaLimitada;
        public bool incluirTramosSinRecta;

        public bool isCurvaGranRadio;
        public double granRadio;



       // <summary>
       /// Segundos entronque
       /// </summary>
       public double longitudMinimaTramo
       {
           get
           {
               if (mLongitudMinimaTramo == null)
               {
                   throw new oExPropertieNullValue("longitudMinimaTramo");
               }

               return mLongitudMinimaTramo.Value;
           }

           set
           {
                mLongitudMinimaTramo = value;
           }

       }


        // <summary>
        /// Longitud minima tramos
        /// </summary>
        public double segundosEntronque
        {
            get
            {
                if (mSegundosEntronque == null)
                {
                    throw new oExPropertieNullValue("segundosEntronque");
                }

                return mSegundosEntronque.Value;
            }

            set
            {
                mSegundosEntronque = value;
            }

        }

        
        // <summary>
        /// Incluir distancia tramos en la valoracion
        /// </summary>
        public bool incluirDistTramos
       {
           get
           {
               if (mIncluirDistTramos == null)
               {
                   throw new oExPropertieNullValue("incluirDistTramos");
               }

               return mIncluirDistTramos.Value;
           }

           set
           {
               mIncluirDistTramos = value;
           }

       }


        /// <summary>
        /// AnguloTotalGrados
        /// </summary>
        public double anguloTotalGrados
        {
            get
            {
                if (mAnguloTotalGrados == null)
                {
                    throw new oExPropertieNullValue("anguloTotalGrados");
                }

                return mAnguloTotalGrados.Value;
            }

           private set
            {
                mAnguloTotalGrados = value;

            }
           
        }
        /// <summary>
        /// Grados Discretizacion Proyecto (Definido Usuario)
        /// </summary>
        public double gradosDiscretizacionProyecto
        {
            get
            {

                if (mGradosDiscretizacionProyecto == null)
                {
                    throw new oExPropertieNullValue("gradosDiscretizacion");
                }

                return mGradosDiscretizacionProyecto.Value;
            }

            private set
            {
                mGradosDiscretizacionProyecto = value;
            }

        }
        /// <summary>
        /// Grados Discretización Calculo
        /// </summary>
        public double gradosDiscretizacionCalculo
        {
            get
            {
                return (this.anguloTotalGrados / (this.numeroTramosAbanico - 1));
            }

        }
        /// <summary>
        /// Longitud Tramo Discretizacion
        /// </summary>
        public double tramoAbanicoDiscretizacion
        {
            get
            {
                if (mTramoAbanicoDiscretizacion == null)
                {
                    throw new oExPropertieNullValue("tramoAbanicoDiscretizacion");
                }

                return mTramoAbanicoDiscretizacion.Value;
            }

            private set
            {
                mTramoAbanicoDiscretizacion = value;
            }

        }
        /// <summary>
        /// Número de Tramos por Abanico.
        /// </summary>
        ///<remarks>El Número de Tramos debe ser Impar</remarks>
        public int numeroTramosAbanico
        {

            get
            {
                if (mNumeroTramos==null)
                {
                    int miNumeroHuecos =  (int)Math.Truncate(this.anguloTotalGrados / this.gradosDiscretizacionProyecto);

                    int miNumeroTramos = miNumeroHuecos + 1;


                    if (miNumeroTramos % 2 == 0) //Es Par
                    {
                        mNumeroTramos = miNumeroTramos + 1;
                    }
                    else
                    {
                        mNumeroTramos = miNumeroTramos;
                    }
           
                }

                return  mNumeroTramos.Value;
            }
           
        }
        /// <summary>
        /// Tolerancia Punto Objetivo
        /// </summary>
        public double toleranciaPuntoObjetivoPC
        {
            get
            {
                if (mToleranciaPtoTargetPC == null)
                {
                    throw new oExPropertieNullValue("Tolerancia Punto Objetivo");
                }

                return mToleranciaPtoTargetPC.Value;

            }

          private  set
            {
                mToleranciaPtoTargetPC = value;
            }



        }


        public bool invalidarTramosAvanceCortoPorIncrementoLongitud
        {
            get
            {
                if (mInvalidarTramosAvanceCortoByLongitud == null)
                {
                    throw new oExPropertieNullValue("InvalidarTramos");
                }

                return mInvalidarTramosAvanceCortoByLongitud.Value;
            }

            private set
            {

                mInvalidarTramosAvanceCortoByLongitud = value;

            }
        }
        public double? invalidarTramosIncrementoLongitudPC
        {

            get
            {
                return mInvalidarTramosIncrementoLongitudPC;
            }

            private set
            {

                mInvalidarTramosIncrementoLongitudPC = value;
            }


        }
        public eAbanicoTipo  abanicoTipo
        {
            get
            {
                if (mAbanicoTipo == null)
                {
                    throw new oExPropertieNullValue("tipoAbanico");
                }

                return mAbanicoTipo.Value;
            }

            private set
            {

                mAbanicoTipo = value;

            }

        }

        #endregion


        #region "Metodos"

        public Dictionary<int, double> getLstAzimutGrados(oP2d iPtoAbanicoOrigen, oP2d iPtoTarget)
        {
            
            int miNumTramos180 = numeroTramosAbanicoByAngulo(this.anguloTotalGrados);

            ////Azimut PTO ORIGEN - PTO TARGET
            double miAzimutGradosPtoOrigenTarget = iPtoAbanicoOrigen.azimut(iPtoTarget, eAng.grados);
            
            //ANGULO TRAMO INCIAL ABANICO
            double miTramoIniAziGrados = miAzimutGradosPtoOrigenTarget + (1.5 * 180);

            if (miTramoIniAziGrados > 360)
            {
                miTramoIniAziGrados = miTramoIniAziGrados - 360;
            }

            var anguloStartAbanico = 90 - (this.anguloTotalGrados/2);
            miTramoIniAziGrados = miTramoIniAziGrados + anguloStartAbanico;

            //Formateo Azimut a 360 Grados
            miTramoIniAziGrados = oTrigo.azimutGradosFormatTo360(miTramoIniAziGrados);
            
            Dictionary<int, double> miDicIdAzimutGrados = new Dictionary<int, double>(miNumTramos180);

            int miNumeroTramosMitadAbanico = (this.numeroTramosAbanico - 1) / 2;
            int miNumeroTramosMitadAbanico180 = (miNumTramos180 - 1) / 2;

            int miIdTramo;

            double? miAzimutGradosTramoi = null;

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                
                for (int i = 0; i < miNumTramos180; i++)
                {
                    //Azimut Tramo i
                    miAzimutGradosTramoi = miTramoIniAziGrados + (i * this.gradosDiscretizacionCalculo);

                    //Formateo Azimut a 360 Grados
                    miAzimutGradosTramoi = oTrigo.azimutGradosFormatTo360(miAzimutGradosTramoi.Value);

                    //Id del Tramo
                    miIdTramo = miNumeroTramosMitadAbanico180 - i;

                    if (!(Math.Abs(miIdTramo) > miNumeroTramosMitadAbanico))
                    {
                        //Populate
                        miDicIdAzimutGrados.Add(miIdTramo, miAzimutGradosTramoi.Value);
                    }

                }
            }


            return miDicIdAzimutGrados;
        }


        public int numeroTramosAbanicoByAngulo(double ianguloTotalGrados)
        {
            if (mNumeroTramos == null)
            {
                int miNumeroHuecos = (int)Math.Truncate(ianguloTotalGrados / this.gradosDiscretizacionProyecto);

                int miNumeroTramos = miNumeroHuecos + 1;


                if (miNumeroTramos % 2 == 0) //Es Par
                {
                    mNumeroTramos = miNumeroTramos + 1;
                }
                else
                {
                    mNumeroTramos = miNumeroTramos;
                }

            }

            return mNumeroTramos.Value;


        }
        
        #endregion

    }
}
