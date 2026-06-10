using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

    using tadLayShare.puntos;
    using tadLayShare;
 
    public class oRoadDes : IDisposable
    {
        /// <summary>
        /// Grupo Carretera 1, 2
        /// </summary>
        private eRoadGrupo? mRoadGrupo = null;
        /// <summary>
        /// Tipo Carretera (Preferencia Rectas, Curvas)
        /// </summary>
        private eRoadPreferencias? mRoadPreferencias = null;
        /// <summary>
        /// Velocidad Proyecto
        /// </summary>
        private double? mVp = null;
        /// <summary>
        /// Radio Proyecto
        /// </summary>
        private double? mRp = null;
        /// <summary>
        /// Longitud Tramo es K
        /// </summary>
        private bool? mIsAijK = null;
        /// <summary>
        /// Aij Minimo
        /// </summary>
        private double? mAijMin = null;
        /// <summary>
        /// Aij Maximo
        /// </summary>
        private double? mAijMax = null;
        /// <summary>
        /// Aij Minimo Salida Llegada
        /// </summary>
        private double? mAijMinSalidaLlegada = null;
        /// <summary>
        /// Avance Máximo
        /// </summary>
        private double? mAvanceMax = null;
        /// <summary>
        /// Preferencias Kv
        /// </summary>
        private eKvPrefer? mKvPreferencias = null;
        /// <summary>
        /// Kv Convexo del Perfil
        /// </summary>
        private double? mKvConvexo = null;
        /// <summary>
        /// Kv Convexo del Perfil
        /// </summary>
        private double? mKvConcavo = null;
        /// <summary>
        /// Permitir Reducciones Puntuales Velocidad
        /// </summary>
        private bool? mAllowReduccionesVelocidad = null;

        /// <summary>
        /// Peralte Por Ciento
        /// </summary>
        private double? mPeraltePc = null;

        public double? AijMinCalculado = null;
        public double? AijMinSalidaLlegadaCalculado = null;


        #region "Constructores"


        public oRoadDes(eRoadGrupo iGrupo, double iVp, double iRp, eRoadPreferencias iRoadPreferencias, bool iRadioCondicionadoLmin)
        {
            this.grupo = iGrupo;
            this.preferencias = iRoadPreferencias;
            this.Vp = iVp;
            this.Rp = iRp;
            if (iRadioCondicionadoLmin)
            {
                if (grupo == eRoadGrupo.Grupo1) Rp = Math.Max(700, iRp);
                if (grupo == eRoadGrupo.Grupo2) Rp = 2 * iRp;
            }

            this.AijMin = Math.Max(oRoadDes.getAijMinimo(this.grupo, this.preferencias, this.Vp, this.Rp), 380);
            this.AijMinSalidaLlegada = oRoadDes.getAijMinSalidaLlegada(this.Rp);
            AijMinCalculado = AijMin;
            AijMinSalidaLlegadaCalculado = AijMinSalidaLlegada;
            this.AijMax = Math.Max(oRoadDes.getAijMaximo(this.preferencias, this.Rp), AijMin);
            this.avanceMax = oRoadDes.getAvanceMaximo(this.Vp, this.Rp, this.AijMax);
        }

        public oRoadDes(eRoadGrupo iGrupo, double iVp, double iRp, eRoadPreferencias iRoadPreferencias, bool iIsAijConstante, bool iAllowReduccionesVelocidad, eKvPrefer iPreferenciaKv, double iKvConvexo, double iKvConvaco, double iPeraltePC, double iAjmin, double iAjminSalidaLlegada)
        {
            this.grupo = iGrupo;
            this.preferencias = iRoadPreferencias;
            this.Vp = iVp;
            this.Rp = iRp;
            this.allowPermitirReduccionesVelocidad = iAllowReduccionesVelocidad;
            this.IsAijK = iIsAijConstante;

            this.preferenciasKv = iPreferenciaKv;
            this.kvConvexo = iKvConvexo;
            this.kvConcavo = iKvConvaco;

            this.AijMin = iAjmin;
            AijMinCalculado = Math.Max(oRoadDes.getAijMinimo(this.grupo, this.preferencias, this.Vp, this.Rp), 380);
            this.AijMinSalidaLlegada = iAjminSalidaLlegada;
            this.AijMinSalidaLlegadaCalculado = oRoadDes.getAijMinSalidaLlegada(this.Rp);

            this.AijMax = Math.Max(oRoadDes.getAijMaximo(this.preferencias, this.Rp), AijMinCalculado.Value);
            this.avanceMax = oRoadDes.getAvanceMaximo(this.Vp, this.Rp, this.AijMax);

            this.peralte = iPeraltePC;

        }

        #endregion

        public bool isNormativaCorrecta()
        {
            return (AijMin == AijMinCalculado) && (AijMinSalidaLlegada == AijMinSalidaLlegadaCalculado);
        }



        #region "Propiedades"
        /// <summary>
        /// Grupo de Carretera
        /// </summary>
        public eRoadGrupo grupo
        {
            get
            {
                if (mRoadGrupo.HasValue)
                {
                    return mRoadGrupo.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Grupo Carretera");
                }
            }

            set
            {
                mRoadGrupo = value;
            }
        }
        /// <summary>
        /// Preferencias de Carretera
        /// </summary>
        public eRoadPreferencias preferencias
        {
            get
            {
                if (mRoadPreferencias.HasValue)
                {
                    return mRoadPreferencias.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Carretera ; Preferencias");
                }
            }

            set
            {
                mRoadPreferencias = value;
            }



        }
        /// <summary>
        /// Radio de Proyecto
        /// </summary>
        public double Vp
        {
            get
            {

                if (mVp.HasValue)
                {
                    return mVp.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Vp");
                }

            }

            set
            {
                mVp = value;
            
            }
        }
        /// <summary>
        /// Radio de Proyecto
        /// </summary>
        public double Rp
        {
            get
            {

                if (mRp.HasValue)
                {
                    return mRp.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Rp");
                }

            }

            set
            {
                mRp = value;
            
            }
        }
        /// <summary>
        /// Aij Es Constante
        /// </summary>
        public bool IsAijK
        {

            get
            {

                if (mIsAijK.HasValue)
                {
                    return mIsAijK.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("IsAijK");
                }
            }

          set  
          {
                mIsAijK = value;
          }

        }
        /// <summary>
        /// Aij Minimo
        /// </summary>
        public double AijMin
        { 
            get
            {
                return mAijMin.Value;
            }

            set
            {
                mAijMin = value;
            }
        }
        /// <summary>
        /// Aij Minimo Salida-Llegada
        /// </summary>
        public double AijMinSalidaLlegada
        {
            get
            {
                return mAijMinSalidaLlegada.Value;
            }

            set
            {
                mAijMinSalidaLlegada = value;
            }

        }
        /// <summary>
        /// Aij Maximo
        /// </summary>
        public double AijMax
        {
            get
            {
                return mAijMax.Value;
            }

            set
            {
                mAijMax = value;
            }
        }
        /// <summary>
        /// Aij Minimo Minimo en Función del Radio
        /// </summary>
        public double AijMinimoMinimoByRadio
        {   
            get
            {
              return 1.05 * this.Rp;
            }
        }
        /// <summary>
        /// Avance Máximo
        /// </summary>
        public double avanceMax
        {
            get
            {
                return mAvanceMax.Value;
            }

            set
            {
                mAvanceMax = value;
            }
        }
        /// <summary>
        /// Preferencias KV
        /// </summary>
        public eKvPrefer preferenciasKv
        {
            get
            {

                if (mKvPreferencias == null)
                {
                    throw new oExPropertieNullValue("Preferencias Kv");
                }

                return mKvPreferencias.Value;
            }

            set
            {
                mKvPreferencias = value;

            }
        }
        /// <summary>
        /// KV Convexo
        /// </summary>
        public double kvConvexo
        {

            get
            {
                if (mKvConvexo == null)
                {
                    throw new oExPropertieNullValue("Kv Convexo");
                }

                return mKvConvexo.Value;
            }

            set
            {
                mKvConvexo = value;
            }

        }
        /// <summary>
        /// KV Concavo
        /// </summary>
        public double kvConcavo
        {

            get
            {
                if (mKvConcavo == null)
                {
                    throw new oExPropertieNullValue("Kv Concavo");
                }

                return mKvConcavo.Value;
            }

            set
            {
                mKvConcavo = value;
            }

        }
        /// <summary>
        /// Permitir Reducciones PuntualesVelocidad
        /// </summary>
        public bool allowPermitirReduccionesVelocidad
        {

            get
            {

                if (mAllowReduccionesVelocidad.HasValue)
                {
                    return mAllowReduccionesVelocidad.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Permitir Reducciones Velocidad");
                }
            }

            private set
            {

                mAllowReduccionesVelocidad = value;

            }


        }
        /// <summary>
        /// Longitud Minima Clotoide
        /// </summary>
        public double LongitudMinimaClotoide
        {

            get
            {

                if (this.grupo == eRoadGrupo.Grupo1)
                {
                    double miV1 = (10 * 3.5);
                    double miV2 = (1.8) - (0.01 * this.Vp);

                    return miV1 / miV2;   
                }
                else if (this.grupo == eRoadGrupo.Grupo2)
                {
                    double miV1 = (9 * 3.5);
                    double miV2 = (1.8) - (0.01 * this.Vp);

                    return miV1 / miV2;
                }
                else
                {
                    throw new oExEnumNotImplemented(this.grupo.ToString());

                }

            }
        }
        /// <summary>
        /// Peralte Por Ciento
        /// </summary>
        public double peralte
        {
            get
            {
                if (mPeraltePc == null)
                {
                    throw new oExPropertieNullValue("Peralte");
                }

                return mPeraltePc.Value;

            }
            set
            {
                mPeraltePc = value;
            }
        }
      


        public string info
        {
            get
            {
                return this.grupo.ToString() + " | " +
                "Vp: " + this.Vp.ToString() + "km/h | " +
                "Rp: " + this.Rp.ToString() + "m | " +
                "Preferencias: " +  this.preferencias.ToString() + " | " +
                "Aij Constante: " + this.IsAijK.ToString() + " | ";
            }
        }



        #endregion
        #region "Funciones"





        /// <summary>
        /// Obtener Aij 
        /// </summary>
        /// <param name="iPtoOrigen">Punto Origen</param>
        /// <param name="iAzimutTramoPrevio">AzimutTramoPrevio</param>
        /// <param name="iAzimutTramoSiguiente">AzimutTramoSiguiente</param>
        /// <returns>Aij</returns>
        public double getAij(IP2d iPtoOrigen, double iAzimutTramoPrevio, double iAzimutTramoSiguiente)
        {
            double miAnguloRadianesTramo = oTrigo.getAngFromAzimut(iPtoOrigen, iAzimutTramoPrevio, iAzimutTramoSiguiente, eAng.radianes);

            return this.getAij(miAnguloRadianesTramo);
        }
        /// <summary>
        /// Obtener el Aij del Abanico
        /// </summary>
        /// <param name="iAngRadianes">Ángulo entre Tramos</param>
        /// <returns>LongitudTramo</returns>
        public double getAij(double iAngRadianes)
        {
            if (this.IsAijK)
            {
                return  this.AijMin;
            }
            else
            {
                //Valor Aij por Ángulo
                double miLongitudAiByAngulo = Math.Abs((3 * Rp) / (Math.Tan(iAngRadianes / 2)));

                return engNet.math.rangos.oMathRangos.getValueFromMinAndMax(miLongitudAiByAngulo, this.AijMin, this.AijMax);
            }
        }
        /// <summary>
        /// Get AijMinimoMinimo en Función del Angulo Tramo Previo
        /// </summary>
        public double getAijMinimoMinimoByAngulo(double iAnguloTramoPrevio, eAng iAnguloFormato)
        {
            return oRoadDes.getAijMinimoMinimo(this.Vp, this.Rp, iAnguloTramoPrevio, iAnguloFormato);
        }
        /// <summary>
        /// Determinar el Angulo Minimo de los Siguientes Tramos cuando el TramoGanador es un Tramo AijMinimoMinimo
        /// </summary>
        public double getAnguloMinimoTramoSiguiente(double iAijMinMin, eAng iAnguloFormato)
        {
            return oRoadDes.getAnguloMinimoTramoSiguiente(this.Vp, this.Rp, iAijMinMin, iAnguloFormato);
        }
        /// <summary>
        /// Obtener el Tipo de Curva del Eje
        /// </summary>
        /// <param name="iAngGr">Ángulo en el vertice</param>
        /// <returns>Tipo de Curva</returns>
        public eRoadCurva getTipoCurva(double iAngGr)
        {

            switch (this.preferencias)
            {
                case eRoadPreferencias.curvas:

                    if (iAngGr <= 120)
                    {
                        return eRoadCurva.Paso;
                    }
                    else if (iAngGr > 120 && iAngGr <= 180)
                    {
                        return eRoadCurva.NoPaso;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Ángulo Tramos");
                    }


                case eRoadPreferencias.rectas:

                    if (iAngGr <= 100)
                    {
                        return eRoadCurva.Paso;
                    }
                    else if (iAngGr > 100 && iAngGr <= 180)
                    {
                        return eRoadCurva.NoPaso;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Ángulo Tramos");
                    }

                default:

                    throw new Exception(string.Format("La Preferencia {0} No esta Definidad", this.preferencias.ToString()));

            }
        }
        /// <summary>
        /// Obtener el Radio según la Norma y El Valor de la Espiral
        /// </summary>
        public oRadioNormaAvalue getRadioNormaAspiral(double iAngGr, eRoadCurva iCurvaTipo)
        {

           //Cuando el usuario aplique Aij = cte= Amín, se aplicarán las reducciones que aquí se describen en las C.P.
 
           // Si ai <83º -> RADIO 0,83 R
           // Si ai <40,7º -> RADIO 0,72 R



            if (iAngGr > 180)
            {
                throw new ArgumentOutOfRangeException(iAngGr.ToString());
            }

            if (this.grupo == eRoadGrupo.Grupo1 && iAngGr > 176.5 && iAngGr <= 180)
            {
                oRadioNormaAvalueTipo miRdAs = new oRadioNormaAvalueTipo(5000);

                return miRdAs;
            }
            else if (this.grupo == eRoadGrupo.Grupo2 && iAngGr > 176.5 && iAngGr <= 180)
            {
                oRadioNormaAvalueTipo miRdAs = new oRadioNormaAvalueTipo(2500);

                return miRdAs;
            }
            else if (IsAijK && iCurvaTipo == eRoadCurva.Paso && iAngGr >= 0 && iAngGr <= 40.7)
            {
                oRadioNormaAvalueTipo miRdAs = new oRadioNormaAvalueTipo(Rp * 0.72);

                return miRdAs;
            }
            else if (IsAijK && iCurvaTipo == eRoadCurva.Paso && iAngGr > 40.7 && iAngGr <= 83)
            {
                oRadioNormaAvalueTipo miRdAs = new oRadioNormaAvalueTipo(Rp * 0.83);

                return miRdAs;
            }
            else if (iCurvaTipo == eRoadCurva.NoPaso)
            {
               
                //Angulo Agudo Giro Rectas
                double miAngAgudoRadianes = tadLayShare.puntos.oTrigo.getRadianesFromGrados(180 - iAngGr);


                double miValor = (Math.Pow(12, 0.5)) * Math.Pow(this.Rp, -0.5); // 12^0.5 * Rp^-0.5

                //Caso Tipo
                if (miAngAgudoRadianes >= miValor)
                {
                    oRadioNormaAvalueTipo miRdAs = new oRadioNormaAvalueTipo(Rp);

                    return miRdAs;
                }
                else
                {

                    double miRadioPrima = 1.5 * this.Rp;
                    double miLe = miRadioPrima * miAngAgudoRadianes;

                    //CNP CALIDAD
                    if (miLe > LongitudMinimaClotoide)
                    {

                        oRadioNormaAvalueCNPCalidad myRdAs = new oRadioNormaAvalueCNPCalidad(Rp, miAngAgudoRadianes);

                        return myRdAs;

                    }
                    //CNP SIN CALIDAD
                    else
                    {
                        oRadioNormaAvalueCNPSinCalidad myRdAs = new oRadioNormaAvalueCNPSinCalidad(this.LongitudMinimaClotoide, miAngAgudoRadianes);

                        return myRdAs;
                    }

                }

            }

            else
            {
                oRadioNormaAvalueTipo myRdAs = new oRadioNormaAvalueTipo(this.Rp);

                return myRdAs;

            }
        }  



 

        #endregion



        #region "Interfaz"

        public void Dispose()
        {
            mRoadGrupo = null;
            mVp = null;
            mRp = null;
            mRoadPreferencias = null;
            mIsAijK = null;
            mKvConvexo = null;
            mKvConcavo = null;
        }


        #endregion


        #region "Metodos Estaticos"


        /// <summary>
        /// Obtener Aij Minimo
        /// </summary>
        /// <param name="iGrupo">Grupo</param>
        /// <param name="iPreferencias">Preferencia</param>
        /// <param name="iVp">Vp</param>
        /// <param name="iRp">Rp</param>
        /// <returns>AijMinimo</returns>
        public static double getAijMinimo(eRoadGrupo iGrupo, eRoadPreferencias iPreferencias, double iVp, double iRp)
        {


            double myAminOp1 = (2.78 * iVp) + (2 / iRp * (Math.Sqrt(12 * (Math.Pow(iRp, 3)))));

            double myAminOp2 = 3 * iRp;

            double myOut;

            if (iPreferencias == eRoadPreferencias.curvas && iGrupo == eRoadGrupo.Grupo1)
            {
                myOut = 3.1 * iRp;
            }
            else if (iPreferencias == eRoadPreferencias.curvas && iGrupo == eRoadGrupo.Grupo2)
            {
                myOut = (((-0.7 / 70) * (iVp - 40)) + 3.8) * iRp;

            }
            else if (iPreferencias == eRoadPreferencias.rectas && iGrupo == eRoadGrupo.Grupo1)
            {

                if (myAminOp1 >= myAminOp2)
                {
                    myOut = myAminOp1;
                }
                else
                {
                    myOut = myAminOp2;
                }
            }
            else if (iPreferencias == eRoadPreferencias.rectas && iGrupo == eRoadGrupo.Grupo2)
            {
                if (myAminOp1 >= myAminOp2)
                {
                    myOut = myAminOp1;
                }
                else
                {
                    myOut = myAminOp2;
                }
            }
            else
            {
                throw new Exception("Error al Obtener la Amin");
            }

            return Math.Round(myOut, 0);
        }

        /// <summary>
        /// Obtener Aij Máximo
        /// </summary>
        /// <param name="iPreferencias">Rectas-Curvas</param>
        /// <param name="iRp">Radio Proyecto</param>
        /// <returns>Aij Máximo</returns>
        public static double getAijMaximo(eRoadPreferencias iPreferencias, double iRp)
        {

            if (iPreferencias == eRoadPreferencias.rectas)
            {
                return Math.Round(iRp * 5.3, 0);
            }
            else if (iPreferencias == eRoadPreferencias.curvas)
            {
                return Math.Round(iRp * 4.1, 0);
            }
            else
            {
                throw new NotImplementedException(iPreferencias.ToString());
            }
        }

        /// <summary>
        ///Obtener Avance Máximo 
        /// </summary>
        public static double getAvanceMaximo(double iVp, double iRp, double iAijMax)
        {
            double miSum1 = 16.70 * iVp;

            double miSum2 = 2 / iRp * (Math.Sqrt(12 * (Math.Pow(iRp, 3))));

            double miAvanceMaximo = Math.Round(miSum1 + miSum2, 0);

            if (miAvanceMaximo > iAijMax)
            {
                return miAvanceMaximo;
            }
            else
            {
                return iAijMax;
            }
        }

        /// <summary>
        /// Amin Mínima Tramo Salida-Llegada 
        /// </summary>
        public static double getAijMinSalidaLlegada(double iRp)
        {

            double myValor01 = Math.Sqrt(Math.Pow(iRp, 3)) / 12;
            double myValor02 = 1.5 * iRp;

            if (myValor01 > myValor02)
            {
                return Math.Round(myValor01, 0);
            }
            else
            {
                return Math.Round(myValor02, 0);
            }
        }


        /// <summary>
        /// Obtener el Valor de Aspiral
        /// </summary>
        public static double getAspiral(double iRadioNorma)
        {
            double value1 = (12 * Math.Pow(iRadioNorma, 3));

            return Math.Pow(value1, 0.25);
        }
        /// <summary>
        /// Obtener la Distancia Minima para validar los puntos Candidatos, obtenidos con Aij Reducido
        /// </summary>
        public static double getAijMinimoMinimo(double iVp, double iRp, double iAnguloTramoPrevio, eAng iAnguloFormato)
        {

 
            double? miAnguloTramoPrevioGrados=null ;

            if (iAnguloFormato == eAng.grados)
            {
                miAnguloTramoPrevioGrados = iAnguloTramoPrevio;
            }
            else if (iAnguloFormato == eAng.radianes)
            {
                miAnguloTramoPrevioGrados = oTrigo.getGradosFromRadianes(iAnguloTramoPrevio);
            }
            else
            {
                throw new oExEnumNotImplemented(iAnguloFormato.ToString());
            }


            
            double miA = getAspiral(iRp);
            double miA2 = Math.Pow(miA, 2);
            double miR2 = Math.Pow(iRp, 2);


            double miSum1 = 2.78 * iVp;
            double miSum2 = miA2 / iRp;
            double miSum31 = (180 - miAnguloTramoPrevioGrados.Value) / (180 / Math.PI);
            double miSum32 = miA2 / miR2;

            double miSum3 = (miSum31 - miSum32) * (miA / 2);

            double miResultado = miSum1 + miSum2 + miSum3;

            return miResultado;

        }
        /// <summary>
        /// Determinar el Angulo Minimo de los Siguientes Tramos cuando el TramoGanador es un Tramo AijMinimoMinimo
        /// </summary>
        public static double getAnguloMinimoTramoSiguiente(double iVp, double iRp, double iAijMinimoMinimo, eAng iAnguloFormato)
        {

            double miA = getAspiral(iRp);
            double miA2 = Math.Pow(miA, 2);
            double miR2 = Math.Pow(iRp, 2);

            double miSum1 = (2 * iAijMinimoMinimo) / miA;
            double miSum2 = (5.56 * iVp) / miA;
            double miSum3 = (miA2 / miR2);
            double miSum4 = (2 * miA) / iRp;

            double miResultadoRadianes = Math.PI - miSum1 + miSum2 - miSum3 + miSum4;

            
            if (iAnguloFormato == eAng.radianes)
            {
                return miResultadoRadianes;
            }
            else if (iAnguloFormato == eAng.grados)
            {
                return oTrigo.getGradosFromRadianes(miResultadoRadianes);
            }
            else
            {
                 throw new oExEnumNotImplemented(iAnguloFormato.ToString());
            }

        }














        #endregion

    }






}
