using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{

   using engCadNet;
    using Autodesk.AutoCAD.Geometry;
   using Autodesk.AutoCAD.Colors;


    public abstract class oSeccionDecoradorParent
    {


        /// <summary>
        /// PK de la Seccion !!!PASAR A VARIABLE de PROYECTO
        /// </summary>
        public static double pkIntervalo {get; set;} 


        //PK, DesfaseX, DesfaseY,Desbroce, eLado, 
        public static Func<double, double, double, eLado, double> mFunGetZTnd = null;



        /// <summary>
        /// Cota Z TND ; Utiliza Funcion Delegada
        /// </summary>
        public static double funGetZtnd(double iPk, double iDesfaseX, double iDesfaseY, eLado iLado)
        {
            return mFunGetZTnd(iPk, iDesfaseX, iDesfaseY, iLado);
        }


        public static eExcavacion funGetTerrenoCorreccionTnd(double iPk, eLado iLado, Point3d iPto)
        {

            return funGetTerrenoCorreccionTnd(iPk, iLado, iPto.X, iPto.Y);
        }
        public static eExcavacion funGetTerrenoCorreccionTnd(double iPk,eLado iLado, double iDesfaseX, double iDesfaseY)
        {

            double miZtnd = funGetZtnd(iPk, iDesfaseX, iDesfaseY, iLado);

            if (miZtnd > iDesfaseY)
            {
                return eExcavacion.desmonte;
            }
            else if (miZtnd < iDesfaseY)
            {
                return eExcavacion.terraplen;
            }
            else
            {
                return eExcavacion.acota;

            }

        }

        public static Color colorSeccionDer { get { return oColor.getInstance.cyan; } }
        public static Color colorSeccionIzq { get {return oColor.getInstance.rojo;}}
        public static Color colorTalud { get { return oColor.getInstance.morado;}}
        public static Color colorDecorator { get { return oColor.getInstance.amarillo; } }
    }

}
