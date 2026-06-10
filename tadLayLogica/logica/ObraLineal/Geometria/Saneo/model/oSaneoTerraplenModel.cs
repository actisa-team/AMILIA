using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria.Saneo
{

    using engCadNet;
    using Autodesk.AutoCAD.Colors;
    
    public class oSaneoTerraplenModel:oSaneo

    {


  
        public double escalonHmax {get; set;}
        public double pendienteMaxSinEscalon { get; set; }



       public oSaneoTerraplenModel (Guid iIdMaterialExc,Guid iIdMaterialRel, double iEspesorSaneoTerraplen, double iEscalonHmax, double iPendienteMaxSinEscalon)
          :base(iIdMaterialExc,iIdMaterialRel,iEspesorSaneoTerraplen)
       
       { 
           escalonHmax = iEscalonHmax;
           pendienteMaxSinEscalon = iPendienteMaxSinEscalon;
       }



       public override Color color
       {
           get
           {
               return oColor.getInstance.morado;
           }
       }

    }
}
