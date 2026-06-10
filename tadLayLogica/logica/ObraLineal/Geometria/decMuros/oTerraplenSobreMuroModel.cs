using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{
    
    using Autodesk.AutoCAD.Geometry;

   
    
    public  class oTerraplenSobreMuroModel : oMuroModel
    {


        public double taludH { get; set; }
        public double taludTerraplenAltura { get; set; }

        
        protected Point3d cC0 { get; set; }
        protected Point3d cC1 { get; set; }
        protected Point3d cC1tnd { get; set; }
        protected Point3d cC1emp { get; set; }
        protected Point3d cC2 { get; set; }
        protected Point3d cC2tnd { get; set; }
        protected Point3d cC2emp { get; set; }




        public oTerraplenSobreMuroModel(Guid iIdMaterial,double iTaludTerraplenAltura, double iTaludH, double iMuroEspesor, double iMuroEmpotramiento)
           :base(iIdMaterial,iMuroEspesor,iMuroEmpotramiento)
        {
            taludTerraplenAltura = iTaludTerraplenAltura;
            taludH = iTaludH;
        }


        public override double area
        { 
            get
            {
                throw new NotImplementedException("Terraplen-Muro-Model-m2");
            } 
        }


    }
}
