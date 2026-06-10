using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{
   
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    
    
    public class oMuroDesmonteModel : oMuroModel
    {


        public double altura { get; set; }
      
        protected Point3d cC0 { get; set; }
        protected Point3d cC1{ get; set; }
        protected Point3d cC2{ get; set; }
        protected Point3d cC3{ get; set; }
        protected Point3d cC4{ get; set; }


       public oMuroDesmonteModel(Guid iIdMaterial,  double iEspesor, double iEmpotramiento, double iAltura)
         :base(iIdMaterial,iEspesor,iEmpotramiento)
       
       {
           altura = iAltura;
       }

        /// <summary>
        /// Obtener el area entre 4 Puntos
        /// </summary>
        /// <remarks>Ojo en los Muros Terraplen se puede modificar los puntos cC3 y cC4 y no se actualiza la altura</remarks>
       public override double area
       { 
           get
           {
               double miAncho = Math.Abs(cC2.X - cC1.X);
               double miAlto = Math.Abs(cC3.Y - cC2.Y);

               return miAncho * miAlto;
           } 
       }


       public virtual Point3dCollection geometria (Point3d iPtoInsert)
       {


           //Defino la Geometria
           cC0 = iPtoInsert;
           cC1 = cC0.getFromIncXIncY(0, -empotramiento, 0);
           cC2 = cC1.getFromIncXIncY(espesor, 0, 0);
           cC3 = cC2.getFromIncXIncY(0, empotramiento + altura, 0);
           cC4 = cC3.getFromIncXIncY(-espesor, 0, 0);


           Point3dCollection miCol = new Point3dCollection();

           miCol.Add(cC0);
           miCol.Add(cC1);
           miCol.Add(cC2);
           miCol.Add(cC3);
           miCol.Add(cC4);

           return miCol;
  
       }


 


    }
}
