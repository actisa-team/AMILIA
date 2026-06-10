using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

   
    using tadLayShare;
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

               mAngAgudoRadianes = iAngAgudoRadianes;
        
           }

        public override double Aspiral
        {
            get {

                double value1 = (mAngAgudoRadianes * Math.Pow(RadioNorma, 2)); // alfa x Rnorma^2

                return Math.Pow(value1, 0.25);
                }
        }


    
    }
    /// <summary>
    /// VALOR RADIO-ESPIRAL CURVAS DE NO PASO PERALTE < 20M
    /// </summary>
    public class oRadioNormaAvalueCNPSinCalidad : oRadioNormaAvalue
    {

        private double mAngAgudoRadianes;
        


        public oRadioNormaAvalueCNPSinCalidad(double iAngAgudoRadianes)
           
        {

            mAngAgudoRadianes = iAngAgudoRadianes-0.060999257;

            this.RadioNorma = 20 / (mAngAgudoRadianes);
            
            

        }

        public override double Aspiral
        {
            get
            {

                double myValue1 = 400 / mAngAgudoRadianes; // alfa x Rnorma^2

                return Math.Pow(myValue1,0.5);
            }
        }



    }

}
