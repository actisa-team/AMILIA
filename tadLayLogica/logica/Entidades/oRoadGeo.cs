using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare;

namespace tadLayLogica
{
    /// <summary>
    /// DATOS DE GEOMETRIA DE LA ROAD
    /// </summary>
    public class oRoadGeoOLD
    {

        private double? mAnchoPlataforma = null;

        private double? mTaludDesmonte = null;
        private double? mTaludTerraplen = null;


        private double? mPendienteMax = null;
        private double? mPendienteMin = null;

        private double? mTerraplenDesmonteMax = null;
        private bool?   mEstructurasGenerar = null;
        private double? mEstructuraPendienteMax = null;
        private double? mEstructuraPendienteMin = null;

        private double? mEstructuraPilaMaxima = null;
        private double? mTramoLonDiscre = null;

        private double? mCoePendienteMax = null;
        private double? mCoeTerraplenDesmonteMax = null;
        private double? mCoePendienteMaxEstructura = null;
        private double? mCoeAlturaPilaMax = null;


#region "Constructores


        public oRoadGeoOLD()
        { 
        
        
        }
        public oRoadGeoOLD(double iTramoDiscre,  double iAnchoPlataforma, double iTaludDesmonte, double iTaludTerraplen,
                           double iPenMaxPorCiento, double iTerraplenMaxMin,
                           bool iEstGenerar, double iEstPilMax, double iEstPenMax,
                           double iCoePendieneMax,double iCoeTerraplenDesmonteMax,
                           double iCoePendienteMaxEstructura, double iCoeAlturaPilaMax,
                           double iPendMin, double iPendMinEstructura)
                          
        {

            mTramoLonDiscre = iTramoDiscre;
            mAnchoPlataforma = iAnchoPlataforma;
            mTaludDesmonte = iTaludDesmonte;
            mTaludTerraplen = iTaludTerraplen;  
            mPendienteMax = iPenMaxPorCiento;
            mTerraplenDesmonteMax = iTerraplenMaxMin;
            mEstructurasGenerar = iEstGenerar;
            mEstructuraPilaMaxima = iEstPilMax;
            mEstructuraPendienteMax = iEstPenMax;

            mCoePendienteMax = iCoePendieneMax;
            mCoeTerraplenDesmonteMax = iCoeTerraplenDesmonteMax;
            mCoePendienteMaxEstructura = iCoePendienteMaxEstructura;
            mCoeAlturaPilaMax = iCoeAlturaPilaMax;

            mPendienteMin = iPendMin;
            mEstructuraPendienteMin = iPendMinEstructura;

        }

#endregion
#region "Propiedades

        public double pAnchoPlataforma
        {


            get
            {
                if (mAnchoPlataforma.HasValue)
                {
                    return mAnchoPlataforma.Value;
                }
                else
                {
                    throw new Exception("El Valor del Ancho de la Plataforma es Nulo");
                
                }
            }

            set
            {
                mAnchoPlataforma = value;
            }
        }


        public double pTaludDesmonte
        {


            get
            {
                if (mTaludDesmonte.HasValue)
                {
                    return mTaludDesmonte.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Talud Desmonte");

                }
            }

            set
            {
                mTaludDesmonte = value;
            }
        }
        public double pTaludTerraplen
        {

            get
            {
                if (mTaludTerraplen.HasValue)
                {
                    return mTaludTerraplen.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Talud Terraplen");

                }
            }

            set
            {
                mTaludTerraplen = value;
            }
        }
        public double pendMaxPorCientoProyecto
        {

            get
            {
                if (mPendienteMax.HasValue)
                {
                    return mPendienteMax.Value;
                }
                else
                {
                    throw new Exception("Pendiente Maxima Proyecto, Valor Nulo");
                }
            }

            set
            {
                mPendienteMax = value;
            }
        }
        public double pendMaxPorCientoCalculo
        {

            get
            {
                if (mPendienteMax.HasValue && mCoePendienteMax.HasValue)
                {
                    return  mCoePendienteMax.Value *  mPendienteMax.Value;
                }
                else
                {
                    throw new Exception("Pendiente Máxima Cálculo, Valor Nulo");
                }
            }

        
        }
        public double terraplenDesmonteMaxProyecto
        {
            get
            {

                if (mTerraplenDesmonteMax.HasValue)
                {
                    return mTerraplenDesmonteMax.Value;
                }
                else
                {
                    throw new Exception("Valor Terreplen/Desmonte Máximo Valor Nulo");
                }

            }

            set
            {
                mTerraplenDesmonteMax = value;

            }




        }
        public double terraplenDesmonteMaxCalculo
        {

            get
            {

                if (mTerraplenDesmonteMax.HasValue && mCoeTerraplenDesmonteMax.HasValue)
                {
                    return  mCoeTerraplenDesmonteMax.Value * mTerraplenDesmonteMax.Value;
                }
                else
                {
                    throw new Exception("Valor Terraplen/Desmonte Máximo Cálculo,  Valor Nulo");
                }

            }


        }
        public bool   pEstructuraGenerar
        {

            get
            {
                if (mEstructurasGenerar.HasValue)
                {
                    return mEstructurasGenerar.Value;
                }
                else
                {

                    throw new Exception("Valor de Generar Estructura es Nulo");
                }
           
            }


            set
            {
                mEstructurasGenerar = value;
            
            }
        

        }
        public double pilaAlturaMaximaProyecto
        {


            get
            {

                if (mEstructuraPilaMaxima.HasValue)
                {
                    return mEstructuraPilaMaxima.Value;
                }

                else
                {
                    throw new oExPropertieNullValue("Valor de Altura Pila Máxima de Proyecto es Nulo.");
                }
            

            
            
            }

            set
            {
                mEstructuraPilaMaxima = value;
            
            }
        
        }
        public double pilaAlturaMaximaCalculo

        {

            get
            { 
                    
                if (mEstructuraPilaMaxima.HasValue && mCoeAlturaPilaMax.HasValue)
                {
                    return  mCoeAlturaPilaMax.Value *  mEstructuraPilaMaxima.Value;
                }

                else
                {
                    throw new oExPropertieNullValue("Valor de Altura Pila Máxima de Cálculo es Nulo.");
                }
            
            }
        
        
        
        }
        public double pendMaxPorCientoProyectoEstructura
        {
            get
            { 
       
                if (mEstructuraPendienteMax.HasValue)
                {
                    return mEstructuraPendienteMax.Value;
                }

                else
                {
                    throw new oExPropertieNullValue("Valor de la Pendiente Máximo de Proyecto de la Estructura es Nulo.");               
               }
                        
            }

            set
            {
                mEstructuraPendienteMax = value;
            
            }
        }
        public double pendMaxPorCientoCalculoEstructura
        {
            get
            {

                if (mEstructuraPendienteMax.HasValue && mCoePendienteMaxEstructura.HasValue)
                {
                    return  mCoePendienteMaxEstructura.Value * mEstructuraPendienteMax.Value;
                }

                else
                {
                    throw new oExPropertieNullValue("Valor de la Pendiente Máximo de Calculo de la Estructura es Nulo.");
                }

            }

            set
            {
                mEstructuraPendienteMax = value;

            }
        }
        public double pTramoLonDiscre
        {

            get
            {

                if (mTramoLonDiscre.HasValue)
                {

                    return mTramoLonDiscre.Value;
                }
                else
                {

                    throw new oExPropertieNullValue("Valor de Longitud Discretización Tramo es Nulo");
                }
            }


            set
            {
                mTramoLonDiscre = value;
            
            }

        }    
        public double getPendMaxByTramoPorCiento(eEstructuraOld iEstructura)
        {

            if (iEstructura == eEstructuraOld.SinEstructura)
            {
                return pendMaxPorCientoProyecto;
            }
            else
            {
                return pendMaxPorCientoProyectoEstructura;
            }
        
        
        }
        public double getPendMinByTramoPorCiento(eEstructuraOld iEstructura)
        {

            if (iEstructura == eEstructuraOld.SinEstructura)
            {
                return pendMinProyectoPorCiento;
            }
            else
            {
                return pendMinEstructurasPorCiento;
            }


        }
        /// <summary>
        /// Pendiente Minima Tramos Sin Estructura
        /// </summary>
        public double pendMinProyectoPorCiento
        {
            get
            {
                if (mPendienteMin.HasValue)
                {
                    return mPendienteMin.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Valor de la Pendiente Mínima de Proyecto es Nulo.");    
                }
            
            
            }

            set
            {
                mPendienteMin = value;
            }
        
        
        }
        /// <summary>
        /// Pendiente Minima Tramos Con Estructura
        /// </summary>
        public double pendMinEstructurasPorCiento
        {

            get
            {
                if (mEstructuraPendienteMin.HasValue)
                {
                    return mEstructuraPendienteMin.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Valor de la Pendiente Mínima de Estructuras es Nulo.");
                }


            }


            set
            {
                mEstructuraPendienteMin = value;
            
            }

        }

#endregion
#region "Estaticos"
        public static eExcavacion getTerrenoCorrecion(double iZterreno, double iZRasante)
        {

            iZterreno = Math.Round(iZterreno, 3);
            iZRasante = Math.Round(iZRasante, 3);
            
            
            if (iZterreno == iZRasante)
            {
                return eExcavacion.acota;
            }
            else if (iZterreno > iZRasante)
            {
                return eExcavacion.desmonte;
            }
            else
            {
                return eExcavacion.terraplen;

            }



        }
        public static eApoyo  getApoyo(bool iHasEstructura, double iZdesfaseAbs,  eExcavacion iTerrenoCorrecion, double iProDesTerMax)
        {

            if (!iHasEstructura)
            {
                if (iTerrenoCorrecion == eExcavacion.acota)
                {
                    return eApoyo.estCota;
                }
                else if (iTerrenoCorrecion == eExcavacion.desmonte)
                {
                    return eApoyo.estDes;
                }
                else if (iTerrenoCorrecion == eExcavacion.terraplen)
                {
                    return eApoyo.estTer;
                }
                else
                {
                    throw new oExEnumNotImplemented(iTerrenoCorrecion.ToString());
                }

            }
            else
            {
                if (iTerrenoCorrecion == eExcavacion.acota)
                {
                    return eApoyo.estCota;
                }
                else if (iTerrenoCorrecion == eExcavacion.desmonte && iZdesfaseAbs <= iProDesTerMax)
                {
                    return eApoyo.estDes;
                }
                else if (iTerrenoCorrecion == eExcavacion.desmonte && iZdesfaseAbs > iProDesTerMax)
                {
                    return eApoyo.estTun;
                }
                else if (iTerrenoCorrecion == eExcavacion.terraplen && iZdesfaseAbs <= iProDesTerMax)
                {
                    return eApoyo.estTer;
                }
                else if (iTerrenoCorrecion == eExcavacion.terraplen && iZdesfaseAbs > iProDesTerMax)
                {
                    return eApoyo.estPue;

                }
                else
                {
                    throw new Exception("Caso de Estructura Sin Pilas-Tuneles, No Configurado");

                }

            }

         }
        #endregion

    }




    /// <summary>
    /// DATOS DE GEOMETRIA DE LA ROAD
    /// </summary>
    public class oRoadGeo
    {


        private double? mTerraplenDesmonteMax = null;

        private double? mTaludDesmonte = null;


        private double? mTaludTerraplen = null;


        private double? mPendienteMax = null;
        private double? mPendienteMin = null;


        private bool? mEstructurasGenerar = null;
        private double? mEstructuraPendienteMax = null;
        private double? mEstructuraPendienteMin = null;

        private double? mEstructuraPilaMaxima = null;

        private double? mTramoLonDiscre = null;

        private double? mCoePendienteMax = null;
        private double? mCoeTerraplenDesmonteMax = null;
        private double? mCoePendienteMaxEstructura = null;
        private double? mCoeAlturaPilaMax = null;


        #region "Constructores


        public oRoadGeo()
        {

        }



        public oRoadGeo(double iPendienteRoadMax,double iPendienteRoadMin,
                        double iPendienteEstructurasMax, double iPendienteEstructurasMin,
                        double iCoeMinoracionPendienteRoad, double iCoeMinPendienteEstruccturas)

        {

            mPendienteMax = iPendienteRoadMax;
            mPendienteMin = iPendienteRoadMin;

            mEstructuraPendienteMax = iPendienteEstructurasMax;
            mEstructuraPendienteMin = iPendienteEstructurasMin;

            mCoePendienteMax = iCoeMinoracionPendienteRoad;
            mCoePendienteMaxEstructura = iCoeMinPendienteEstruccturas;

        }
        public oRoadGeo(double iTramoDiscre, double iTaludDesmonte, double iTaludTerraplen,
                           double iPenMaxPorCiento, double iTerraplenMaxMin,
                           bool iEstGenerar, double iEstPilMax, double iEstPenMax,
                           double iCoePendieneMax, double iCoeTerraplenDesmonteMax,
                           double iCoePendienteMaxEstructura, double iCoeAlturaPilaMax,
                           double iPendMin, double iPendMinEstructura)
        {

            mTramoLonDiscre = iTramoDiscre;

            mTaludDesmonte = iTaludDesmonte;
            mTaludTerraplen = iTaludTerraplen;
            mPendienteMax = iPenMaxPorCiento;
            mTerraplenDesmonteMax = iTerraplenMaxMin;
            mEstructurasGenerar = iEstGenerar;
            mEstructuraPilaMaxima = iEstPilMax;
            mEstructuraPendienteMax = iEstPenMax;

            mCoePendienteMax = iCoePendieneMax;
            mCoeTerraplenDesmonteMax = iCoeTerraplenDesmonteMax;
            mCoePendienteMaxEstructura = iCoePendienteMaxEstructura;
            mCoeAlturaPilaMax = iCoeAlturaPilaMax;

            mPendienteMin = iPendMin;
            mEstructuraPendienteMin = iPendMinEstructura;

        }

        #endregion
        #region "Propiedades

      


        public double pTaludDesmonte
        {


            get
            {
                if (mTaludDesmonte.HasValue)
                {
                    return mTaludDesmonte.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Talud Desmonte");

                }
            }

            set
            {
                mTaludDesmonte = value;
            }
        }
        public double pTaludTerraplen
        {

            get
            {
                if (mTaludTerraplen.HasValue)
                {
                    return mTaludTerraplen.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Talud Terraplen");

                }
            }

            set
            {
                mTaludTerraplen = value;
            }
        }
        public double pendMaxPorCientoProyecto
        {

            get
            {
                if (mPendienteMax.HasValue)
                {
                    return mPendienteMax.Value;
                }
                else
                {
                    throw new Exception("Pendiente Maxima Proyecto, Valor Nulo");
                }
            }

            set
            {
                mPendienteMax = value;
            }
        }
        public double pendMaxPorCientoCalculo
        {

            get
            {
                if (mPendienteMax.HasValue && mCoePendienteMax.HasValue)
                {
                    return mCoePendienteMax.Value * mPendienteMax.Value;
                }
                else
                {
                    throw new Exception("Pendiente Máxima Cálculo, Valor Nulo");
                }
            }


        }
        public double terraplenDesmonteMaxProyecto
        {
            get
            {

                if (mTerraplenDesmonteMax.HasValue)
                {
                    return mTerraplenDesmonteMax.Value;
                }
                else
                {
                    throw new Exception("Valor Terreplen/Desmonte Máximo Valor Nulo");
                }

            }

            set
            {
                mTerraplenDesmonteMax = value;

            }




        }
        public double terraplenDesmonteMaxCalculo
        {

            get
            {

                if (mTerraplenDesmonteMax.HasValue && mCoeTerraplenDesmonteMax.HasValue)
                {
                    return mCoeTerraplenDesmonteMax.Value * mTerraplenDesmonteMax.Value;
                }
                else
                {
                    throw new Exception("Valor Terraplen/Desmonte Máximo Cálculo,  Valor Nulo");
                }

            }


        }
        public bool pEstructuraGenerar
        {

            get
            {
                if (mEstructurasGenerar.HasValue)
                {
                    return mEstructurasGenerar.Value;
                }
                else
                {

                    throw new Exception("Valor de Generar Estructura es Nulo");
                }

            }


            set
            {
                mEstructurasGenerar = value;

            }


        }
        public double pilaAlturaMaximaProyecto
        {


            get
            {

                if (mEstructuraPilaMaxima.HasValue)
                {
                    return mEstructuraPilaMaxima.Value;
                }

                else
                {
                    throw new oExPropertieNullValue("Valor de Altura Pila Máxima de Proyecto es Nulo.");
                }




            }

            set
            {
                mEstructuraPilaMaxima = value;

            }

        }
        public double pilaAlturaMaximaCalculo
        {

            get
            {

                if (mEstructuraPilaMaxima.HasValue && mCoeAlturaPilaMax.HasValue)
                {
                    return mCoeAlturaPilaMax.Value * mEstructuraPilaMaxima.Value;
                }

                else
                {
                    throw new oExPropertieNullValue("Valor de Altura Pila Máxima de Cálculo es Nulo.");
                }

            }



        }
        public double pendMaxPorCientoProyectoEstructura
        {
            get
            {

                if (mEstructuraPendienteMax.HasValue)
                {
                    return mEstructuraPendienteMax.Value;
                }

                else
                {
                    throw new oExPropertieNullValue("Valor de la Pendiente Máximo de Proyecto de la Estructura es Nulo.");
                }

            }

            set
            {
                mEstructuraPendienteMax = value;

            }
        }
        public double pendMaxPorCientoCalculoEstructura
        {
            get
            {

                if (mEstructuraPendienteMax.HasValue && mCoePendienteMaxEstructura.HasValue)
                {
                    return mCoePendienteMaxEstructura.Value * mEstructuraPendienteMax.Value;
                }

                else
                {
                    throw new oExPropertieNullValue("Valor de la Pendiente Máximo de Calculo de la Estructura es Nulo.");
                }

            }

            set
            {
                mEstructuraPendienteMax = value;

            }
        }
        public double pTramoLonDiscre
        {

            get
            {

                if (mTramoLonDiscre.HasValue)
                {

                    return mTramoLonDiscre.Value;
                }
                else
                {

                    throw new oExPropertieNullValue("Valor de Longitud Discretización Tramo es Nulo");
                }
            }


            set
            {
                mTramoLonDiscre = value;

            }

        }
        public double getPendMaxByTramoPorCiento(eEstructuraOld iEstructura)
        {

            if (iEstructura == eEstructuraOld.SinEstructura)
            {
                return pendMaxPorCientoProyecto;
            }
            else
            {
                return pendMaxPorCientoProyectoEstructura;
            }


        }
        public double getPendMinByTramoPorCiento(eEstructuraOld iEstructura)
        {

            if (iEstructura == eEstructuraOld.SinEstructura)
            {
                return pendMinProyectoPorCiento;
            }
            else
            {
                return pendMinEstructurasPorCiento;
            }


        }
        /// <summary>
        /// Pendiente Minima Tramos Sin Estructura
        /// </summary>
        public double pendMinProyectoPorCiento
        {
            get
            {
                if (mPendienteMin.HasValue)
                {
                    return mPendienteMin.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Valor de la Pendiente Mínima de Proyecto es Nulo.");
                }


            }

            set
            {
                mPendienteMin = value;
            }


        }
        /// <summary>
        /// Pendiente Minima Tramos Con Estructura
        /// </summary>
        public double pendMinEstructurasPorCiento
        {

            get
            {
                if (mEstructuraPendienteMin.HasValue)
                {
                    return mEstructuraPendienteMin.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Valor de la Pendiente Mínima de Estructuras es Nulo.");
                }


            }


            set
            {
                mEstructuraPendienteMin = value;

            }

        }

        #endregion
        #region "Estaticos"
        public static eExcavacion getTerrenoCorrecion(double iZterreno, double iZRasante)
        {

            iZterreno = Math.Round(iZterreno, 3);
            iZRasante = Math.Round(iZRasante, 3);


            if (iZterreno == iZRasante)
            {
                return eExcavacion.acota;
            }
            else if (iZterreno > iZRasante)
            {
                return eExcavacion.desmonte;
            }
            else
            {
                return eExcavacion.terraplen;

            }



        }
        public static eApoyo getApoyo(bool iHasEstructura, double iZdesfaseAbs, eExcavacion iTerrenoCorrecion, double iProDesTerMax)
        {

            if (!iHasEstructura)
            {
                if (iTerrenoCorrecion == eExcavacion.acota)
                {
                    return eApoyo.estCota;
                }
                else if (iTerrenoCorrecion == eExcavacion.desmonte)
                {
                    return eApoyo.estDes;
                }
                else if (iTerrenoCorrecion == eExcavacion.terraplen)
                {
                    return eApoyo.estTer;
                }
                else
                {
                    throw new oExEnumNotImplemented(iTerrenoCorrecion.ToString());
                }

            }
            else
            {
                if (iTerrenoCorrecion == eExcavacion.acota)
                {
                    return eApoyo.estCota;
                }
                else if (iTerrenoCorrecion == eExcavacion.desmonte && iZdesfaseAbs <= iProDesTerMax)
                {
                    return eApoyo.estDes;
                }
                else if (iTerrenoCorrecion == eExcavacion.desmonte && iZdesfaseAbs > iProDesTerMax)
                {
                    return eApoyo.estTun;
                }
                else if (iTerrenoCorrecion == eExcavacion.terraplen && iZdesfaseAbs <= iProDesTerMax)
                {
                    return eApoyo.estTer;
                }
                else if (iTerrenoCorrecion == eExcavacion.terraplen && iZdesfaseAbs > iProDesTerMax)
                {
                    return eApoyo.estPue;

                }
                else
                {
                    throw new Exception("Caso de Estructura Sin Pilas-Tuneles, No Configurado");

                }

            }

        }
        #endregion

    }

}
