using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTJ.Secciones
{
    using engNet.Extension.Double;


    using tadLayLogica;
    using Autodesk.AutoCAD.Geometry;
    using tadLayShare;


    public class oSeccionSimple
    {

        private eExcavacion mTerrenoCorrecion;
        private double? mDesfaseAbs;


        public oSeccionSimple(eExcavacion iTerrenoCorrecion, double iDesfaseAbs)
        {
            mTerrenoCorrecion = iTerrenoCorrecion;
            mDesfaseAbs = iDesfaseAbs;      
        }


        public oSeccionSimple(Point2d iPtoTerreno, Point2d iPto)
        {

            if (iPtoTerreno.Y > iPto.Y)
            {

                mTerrenoCorrecion = eExcavacion.desmonte;
                mDesfaseAbs = iPtoTerreno.GetDistanceTo(iPto);

            }
            else if (iPtoTerreno.Y < iPto.Y)
            {

                mTerrenoCorrecion = eExcavacion.terraplen;
                mDesfaseAbs = iPtoTerreno.GetDistanceTo(iPto);

            }
            else
            {
                mTerrenoCorrecion = eExcavacion.acota;
                mDesfaseAbs = 0;
            }
           
        }




        public eExcavacion terrenoCorrecion
        {
            get
            {
                return mTerrenoCorrecion;

            }

            set
            {

                mTerrenoCorrecion = value;
            
            }
        
        
        
        
        
        
        }

        public double desfaseAbs
        {

            get
            {
                if (mDesfaseAbs.HasValue)
                {
                    return mDesfaseAbs.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Desfase Absoluto");
                
                }          
            
            }


            set
            {
                mDesfaseAbs = value;
            } 
        }

        public override string ToString()
        {
            return terrenoCorrecion.ToString() + desfaseAbs.roundOff(2);
        }

    }
}
