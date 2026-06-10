using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{
    using System.ComponentModel;


    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    

    public abstract class oCunetaAbstract : oSeccionDecoradorParent
    {

        /// <summary>
        /// Material
        /// </summary>
        public Guid material { get; set; }
        /// <summary>
        /// Ancho Superior Interior
        /// </summary>
        public double anchoSuperiorInt {get; set;}
        /// <summary>
        /// Espesor 
        /// </summary>
        public double espesor {get; set;}
        /// <summary>
        /// Profundidad Interior
        /// </summary>
        public double alturaInterior { get; set; }




        
        public oCunetaAbstract()
        {
        
        }




        public oCunetaAbstract(Guid iIdMaterial,double iAnchoSuperiorInt, double iEspesor, double iAlturaInterior)
        {
            material = iIdMaterial;
            anchoSuperiorInt = iAnchoSuperiorInt;
            espesor = iEspesor;
            alturaInterior = iAlturaInterior;    
        }



        /// <summary>
        /// Tipo de Cuneta
        /// </summary>
        public abstract eCunetaTipo tipo { get; }

        /// <summary>
        /// Puntos Inferiores Mitad de la Cuneta
        /// </summary>
        public abstract Point3dCollection lstInferiorMid { get; }
        /// <summary>
        /// Colección de Puntos de la Geometria
        /// </summary>
        public abstract Point3dCollection geometria(Point3d iPtoInicio);
        /// <summary>
        /// Colección de Puntos de la Geometria
        /// </summary>
        public abstract Point3dCollection geometriaMitad (Point3d iPtoInicio);
        /// <summary>
        /// Colección de Puntos Exteriores de la Cuneta (Se usa Berma Int DOB-AUT)
        /// </summary>
        public abstract Point3dCollection geometriaMitadExt(Point3d iPtoInicio);
        /// Talud de la Cuneta
        /// </summary>
        public abstract double taludCuneta {get;}
        /// <summary>
        /// Altura Exterior Cuneta
        /// </summary>
        public abstract double alturaExterior { get; }
        /// <summary>
        /// Ancho Superior
        /// </summary>
        public virtual double anchoSupExterior
        {
            get
            {
                return anchoSuperiorInt + (2 * espesorX);
            }
        }


        #region "Propiedades"
        protected double alfaRad
        {
            get
            {
                return Math.Atan(1 / taludCuneta);
            }
        }
        public Color color
        {
            get
            {
                return oColor.getInstance.azul;

            }

        }
        public double espesorX
        {
            get
            { 
                return espesor/Math.Sin(alfaRad);
            }
        }
        public double espesorY
        {
            get
            { 
                return espesor/Math.Cos(alfaRad);
            }
        }

        #endregion



       

    }



    
    


    



}
