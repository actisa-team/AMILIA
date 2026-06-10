using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria.Saneo
{

    using engCadNet;
    using Autodesk.AutoCAD.Colors;

   public class oSaneoDesmonteModel:oSaneo
    {

      


       public oSaneoDesmonteModel(Guid iIdMaterialExc, Guid iIdMaterialRel, double iEspesorSaneoDesmonte)
         :base(iIdMaterialExc,iIdMaterialRel,iEspesorSaneoDesmonte)
       {
           
       }

       public override Color color
       {
           get
           {
               return oColor.getInstance.azul;
           }
       }
    }
}
