using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

   
    using tadLayShare;

    public abstract class oRadioNormaAvalue
    {

        private double? mRadioNorma=null;

        public oRadioNormaAvalue()
        {

        }
        public oRadioNormaAvalue(double iRadioNorma)
        {
            mRadioNorma = iRadioNorma;

        }
        public double RadioNorma
        {

            get
            {
                if (mRadioNorma.HasValue)
                {
                    return mRadioNorma.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Radio Norma");
                }

            }

            set
            {
                mRadioNorma = value;

            }

        }
        public abstract double Aspiral { get; }

    }

    /// <summary>
    /// VALOR RADIO-ESPIRAL TIPO
    /// </summary>
    public class oRadioNormaAvalueTipo : oRadioNormaAvalue
    {

           public oRadioNormaAvalueTipo(double iRadioNorma)
               :base(iRadioNorma)
             
           {
             
        
           }

        public override double Aspiral
        {
            get {

                double value1 = (12 * Math.Pow(RadioNorma, 3));

                return Math.Pow(value1, 0.25);
                }
        }
    
    }
    /// <summary>
    /// VALOR RADIO-ESPIRAL CURVAS DE NO PASO PERALTE > 20M
    /// </summary>
    public class oRadioNormaAvalueCNPCalidad : oRadioNormaAvalue
    {

        private double mAngAgudoRadianes;
       
        public oRadioNormaAvalueCNPCalidad(double iRp,double iAngAgudoRadianes)          
           {
               RadioNorma = 1.5 * iRp;
               mAngAgudoRadianes = iAngAgudoRadianes-0.01;
           }

        public override double Aspiral
        {
            get {

                double value1 = (mAngAgudoRadianes * Math.Pow(RadioNorma, 2)); // alfa x Rnorma^2

                return Math.Pow(value1, 0.5);
                }
        }

    }
    /// <summary>
    /// VALOR RADIO-ESPIRAL CURVAS DE NO PASO PERALTE < 20M
    /// </summary>
    public class oRadioNormaAvalueCNPSinCalidad : oRadioNormaAvalue
    {
        private double mAngAgudoRadianes;

        private double mLongitudMinimaClotoide;

        public oRadioNormaAvalueCNPSinCalidad(double iLongitudMinimaClotoide,  double iAngAgudoRadianes)
           
        {

            mLongitudMinimaClotoide = iLongitudMinimaClotoide;
            mAngAgudoRadianes = iAngAgudoRadianes-0.01;

            this.RadioNorma = iLongitudMinimaClotoide / (mAngAgudoRadianes);                  
        }
        public override double Aspiral
        {
            get
            {
                double miValue1 = this.RadioNorma * mLongitudMinimaClotoide;

                return Math.Pow(miValue1,0.5);
            }
        }
    }

}
