using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{

    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    
    
    
    public  class oCunetaTrapezoidalModel : oCunetaAbstract
    {

        private Point3d cC0{ get; set; }
        protected Point3d cC1{ get; set; }
        protected Point3d cC2{ get; set; }
        protected Point3d cC2e { get; set; }
        protected Point3d cC3{ get; set; }
        private Point3d cC4{ get; set; }
        private Point3d cC5{ get; set; }
        private Point3d cC6{ get; set; }
        private Point3d cC7{ get; set; }

        private Point3dCollection mLstPtoInferioMitad = new Point3dCollection();
        
        
         public double anchoInferior { get; set; }


         #region "Constructores"


         public oCunetaTrapezoidalModel(Guid iIdMaterial,double iAnchoSuperiorInt, double iAnchoInferior,double iEspesor,double iAlturaInterior)         
         {
             material = iIdMaterial;
             anchoSuperiorInt = iAnchoSuperiorInt;
             anchoInferior = iAnchoInferior;
             espesor = iEspesor;
             alturaInterior = iAlturaInterior;                    
         }

         #endregion
         #region "Metodos Abstractos"



        public override double taludCuneta
        {
            get
            {
                double miIncX = (anchoSuperiorInt / 2) - (anchoInferior / 2);
                double miIncY = alturaInterior;
                return miIncX / miIncY;
                 
            }
        }
        public override double alturaExterior
        {
            get
            {
                return alturaInterior + espesor;
            }
        }



        public override Point3dCollection geometria(Point3d iPtoInicio)
             {

                 cC0 = iPtoInicio;
                 cC7 = cC0.getFromIncXIncY(espesorX, 0, 0);
                 cC6 = cC7.getFromTalud(true, false, taludCuneta, alturaInterior);
                 cC5 = cC6.getFromIncXIncY(anchoInferior, 0, 0);
                 cC4 = cC5.getFromTalud(true, true, taludCuneta, alturaInterior);
                 cC3 = cC4.getFromIncXIncY(espesorX, 0, 0);
                 cC1 = cC0.getFromTalud(true, false, taludCuneta, alturaExterior);
                 cC2 = cC3.getFromTalud(false, false, taludCuneta,alturaExterior);

                 cC2e = new Point3d(cC3.X, cC1.Y, 0);


                 //Lst Punto Intermedio
                 double miIncX = cC1.DistanceTo(cC2) / 2;
                 Point3d miPtoMid = cC1.getFromIncXIncY(miIncX, 0, 0);
                 mLstPtoInferioMitad.Clear();
                 mLstPtoInferioMitad.Add(miPtoMid);
                 mLstPtoInferioMitad.Add(cC2);


                 Point3dCollection miCol = new Point3dCollection();

                 miCol.Add(cC0);
                 miCol.Add(cC1);
                 miCol.Add(cC2);
                 miCol.Add(cC3);
                 miCol.Add(cC4);
                 miCol.Add(cC5);
                 miCol.Add(cC6);
                 miCol.Add(cC7);
              
                 return miCol;

             }


        public override Point3dCollection geometriaMitad(Point3d iPtoInicio)
             {
                 
                 Point3d miC0 = iPtoInicio;
                 Point3d miC1 = iPtoInicio.getFromIncXIncY(anchoInferior / 2, 0, 0);
                 Point3d miC2 = miC1.getFromTalud(true, true, taludCuneta, alturaInterior);
                 Point3d miC3 = miC2.getFromIncXIncY(espesorX, 0, 0);
                 Point3d miC4 = miC3.getFromTalud(false, false, taludCuneta, alturaExterior);
                 Point3d miC5 = miC0.getFromIncXIncY(0,-espesor, 0);

                 //Puntos Intermedios Inferiores
                 mLstPtoInferioMitad.Clear();
                 mLstPtoInferioMitad.Add(miC5);
                 mLstPtoInferioMitad.Add(miC4);

                 //Colección de Puntos
                 Point3dCollection miCol = new Point3dCollection();
                 miCol.Add(miC0);
                 miCol.Add(miC1);
                 miCol.Add(miC2);
                 miCol.Add(miC3);
                 miCol.Add(miC4);
                 miCol.Add(miC5);

                 return miCol;

             }


        public override Point3dCollection geometriaMitadExt(Point3d iPtoInicio)
             {

                 Point3d miC0 = iPtoInicio;
                 Point3d miC1 = iPtoInicio.getFromIncXIncY(anchoInferior / 2, 0, 0);
                 Point3d miC2 = miC1.getFromTalud(true, true, taludCuneta, alturaInterior);
                 Point3d miC3 = miC2.getFromIncXIncY(espesorX, 0, 0);
                 Point3d miC4 = miC3.getFromTalud(false, false, taludCuneta, alturaExterior);
                 Point3d miC5 = miC0.getFromIncXIncY(0, -espesor, 0);

                 //Colección de Puntos
                 Point3dCollection miCol = new Point3dCollection();
                 miCol.Add(miC1);
                 miCol.Add(miC2);
                 miCol.Add(miC3);

                 return miCol;
             }

        /// <summary>
        /// Punto 
        /// </summary>
        public override Point3dCollection lstInferiorMid
        {
            get
            {
                return mLstPtoInferioMitad;
            }
        }

        public override eCunetaTipo tipo
        {
            get
            {
                return eCunetaTipo.TRAPEZ;
            }
        }

             #endregion

    }
}
