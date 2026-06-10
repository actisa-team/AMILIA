using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{


    using System.ComponentModel;
    using tadLayShare;
    
    /// <summary>
    /// CARRETERA PENDIENTES
    /// </summary>
    public class oRoadPendientes
    {
       #region "PRIVADOS"

        private double? mCalzadaPendienteProyectoMaximaPC = null;
        private double? mCalzadaPendienteProyectoMinimaPC = null;

        private double? mEstructuraPendienteProyectoMaximaPC = null;
        private double? mEstructuraPendienteProyectoMinimaPC = null;

        private double? mCoeMinoracionCalzadaPendienteMaxima = null;
        private double? mCoeMinoracionEstructuraPendienteMaxima = null;

        #endregion
       #region "CONSTRUCTOR"


       public oRoadPendientes(double iCalzadaPendienteProyectoMaxima,
                                double iCalzadaPendienteProyectoMinima,
                                double iEstructuraPendienteProyectoMaximo,
                                double iEstructuraPendienteProyectoMinima)

       {

           this.calzadaPendienteProyectoMaximaPC = iCalzadaPendienteProyectoMaxima;
           this.calzadaPendienteProyectoMinimaPC = iCalzadaPendienteProyectoMinima;

           this.estructurasPendienteProyectoMaximaPC = iEstructuraPendienteProyectoMaximo;
           this.estructurasPendienteProyectoMinimaPC = iEstructuraPendienteProyectoMinima;
       }


       public oRoadPendientes(double iCalzadaPendienteProyectoMaxima,
                        double iCalzadaPendienteProyectoMinima,
                        double iEstructuraPendienteProyectoMaximo,
                        double iEstructuraPendienteProyectoMinima,
                        double iCoeMinPendienteCalzada,
                        double iCoeMinPendienteEstructuras)
       {

           this.calzadaPendienteProyectoMaximaPC = iCalzadaPendienteProyectoMaxima;
           this.calzadaPendienteProyectoMinimaPC = iCalzadaPendienteProyectoMinima;

           this.estructurasPendienteProyectoMaximaPC = iEstructuraPendienteProyectoMaximo;
           this.estructurasPendienteProyectoMinimaPC = iEstructuraPendienteProyectoMinima;

           this.coeMinoracionCalzadaPendienteMaxima = iCoeMinPendienteCalzada;
           this.coeMinoracionEstructurasPendienteMaxima = iCoeMinPendienteEstructuras;

       }



       #endregion
       #region "PROPIEDADES"
 
       public double calzadaPendienteProyectoMaximaPC 
       { 
           get
           {
               if (mCalzadaPendienteProyectoMaximaPC == null)
               {
                   throw new oExPropertieNullValue();
               }

               return mCalzadaPendienteProyectoMaximaPC.Value;
           }

           set
           {
               mCalzadaPendienteProyectoMaximaPC = value;
           }
       }
       public double calzadaPendienteProyectoMinimaPC
       {
           get
           {
               if (mCalzadaPendienteProyectoMinimaPC == null)
               {
                   throw new oExPropertieNullValue();
               }

               return mCalzadaPendienteProyectoMinimaPC.Value;
           }

           set
           {
               mCalzadaPendienteProyectoMinimaPC = value;
           }
       }
       public double estructurasPendienteProyectoMaximaPC
       {
           get
           {
               if (mEstructuraPendienteProyectoMaximaPC == null)
               {
                   throw new oExPropertieNullValue();
               }

               return mEstructuraPendienteProyectoMaximaPC.Value;
           }

           set
           {
               mEstructuraPendienteProyectoMaximaPC = value;
           }
       }
       public double estructurasPendienteProyectoMinimaPC
       {
           get
           {
               if (mEstructuraPendienteProyectoMinimaPC == null)
               {
                   throw new oExPropertieNullValue();
               }

               return mEstructuraPendienteProyectoMinimaPC.Value;
           }

           set
           {
               mEstructuraPendienteProyectoMinimaPC = value;
           }
       }
       public double coeMinoracionCalzadaPendienteMaxima
       {
           get
           {
               if (mCoeMinoracionCalzadaPendienteMaxima == null)
               {
                   throw new oExPropertieNullValue();
               }

               return mCoeMinoracionCalzadaPendienteMaxima.Value;
           }

           set
           {
               mCoeMinoracionCalzadaPendienteMaxima = value;
           }
       }
       public double coeMinoracionEstructurasPendienteMaxima
       {
           get
           {
               if (mCoeMinoracionEstructuraPendienteMaxima == null)
               {
                   throw new oExPropertieNullValue();
               }

               return mCoeMinoracionEstructuraPendienteMaxima.Value;
           }

           set
           {
               mCoeMinoracionEstructuraPendienteMaxima = value;
           }
       }
       public double calzadaPendienteCalculoMaximoPC
       {
            get
           {
               return this.calzadaPendienteProyectoMaximaPC * this.coeMinoracionCalzadaPendienteMaxima;
           }
       }
       public double estructuraPendienteCalculoMaximoPC
        {
            get
            {
                return this.estructurasPendienteProyectoMaximaPC * this.coeMinoracionEstructurasPendienteMaxima;
            }
        }

       #endregion
   }


}
