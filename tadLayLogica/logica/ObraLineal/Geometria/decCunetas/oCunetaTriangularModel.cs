using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{

    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    
    
    public  class oCunetaTriangularModel : oCunetaAbstract
  {

        private Point3d cC0 { get; set; }
        protected Point3d cC1 { get; set; }
        protected Point3d cC1e { get; set; }
        protected Point3d cC2 { get; set; }
        private Point3d cC3 { get; set; }
        private Point3d cC4 { get; set; }
        private Point3d cC5 { get; set; }

       #region "Constructores"


        public oCunetaTriangularModel(Guid iIdMaterial,double iAnchoSuperiorInterior, double iEspesor, double iAlturaInterior)
        {
            material = iIdMaterial;
            anchoSuperiorInt = iAnchoSuperiorInterior;
            espesor = iEspesor;
            alturaInterior = iAlturaInterior;
        }

        #endregion
       #region "Metodos Abstractos"


        public override double taludCuneta
        {
            get
            {
                double miIncX = (anchoSuperiorInt / 2);
                double miIncY = alturaInterior;

                return miIncX / miIncY;
            }
        }
        public override double alturaExterior
        {
            get
            {
                return alturaInterior + espesorY;
            }
        }
        public override Point3dCollection lstInferiorMid
        {
            get
            {
                Point3dCollection miCol = new Point3dCollection();
                miCol.Add(cC1);

                return miCol;
            }
        }






       public override Point3dCollection geometria(Point3d iPtoInicio)
       {

           cC0 = iPtoInicio;

           cC1 = cC0.getFromTalud(true, false, taludCuneta, alturaExterior);

           cC2 = cC1.getFromTalud(true, true, taludCuneta,alturaExterior);

           cC1e = new Point3d(cC2.X, cC1.Y, 0);

           cC3 = cC2.getFromIncXIncY(-espesorX, 0, 0);

           cC4 = cC1.getFromIncXIncY(0, espesorY, 0);

           cC5 = cC0.getFromIncXIncY(espesorX, 0, 0);

           Point3dCollection miCol = new Point3dCollection();

           miCol.Add(cC0);
           miCol.Add(cC1);
           miCol.Add(cC2);
           miCol.Add(cC3);
           miCol.Add(cC4);
           miCol.Add(cC5);


           return miCol;

       }

        /// <summary>
        /// Geometria Mitad Cuneta
        /// </summary>
       public override Point3dCollection geometriaMitad(Point3d iPtoInicio)
       {

           Point3d miC0 = iPtoInicio;
           Point3d miC1 = miC0.getFromTalud(true,true,taludCuneta,alturaInterior);
           Point3d miC2 = miC1.getFromIncXIncY(espesorX, 0, 0);
           Point3d miC3 = miC2.getFromTalud(false, false, taludCuneta, alturaExterior);

           Point3dCollection miCol = new Point3dCollection();

           miCol.Add(miC0);
           miCol.Add(miC1);
           miCol.Add(miC2);
           miCol.Add(miC3);

           return miCol;
       }

       /// <summary>
       /// Puntos Exteriores Mitad
       /// </summary>
       public override Point3dCollection geometriaMitadExt(Point3d iPtoInicio)
       {
           Point3d miC0 = iPtoInicio;
           Point3d miC1 = miC0.getFromTalud(true, true, taludCuneta, alturaInterior);
           Point3d miC2 = miC1.getFromIncXIncY(espesorX, 0, 0);
           Point3d miC3 = miC2.getFromTalud(false, false, taludCuneta, alturaExterior);

           Point3dCollection miCol = new Point3dCollection();
           miCol.Add(miC1);
           miCol.Add(miC2);

           return miCol;

       }

       public override eCunetaTipo tipo
       {
           get
           {
               return eCunetaTipo.TRIANG;
           }
       }

       #endregion

  }
}
