using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
    
{  
    
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;
    
    /// <summary>
    /// JUAN - 2014-10-20
    /// </summary>
    public class oTaludBermasModel: oSeccionDecoradorParent
    {





       public double Hmax {get;set;}
       public bool   isBermaPie {get;set;}
       public double bermaAncho {get;set;}
       public double bermaAlto {get;set;}
       public double bermaTaludH {get;set;}
       public bool isBermaDesmonte { get; set; }

       /// <summary>
       /// Al Crear la berma, desconozco la altura a la que se produce la
       /// intersección con el terreno.
       /// </summary>
       private const double KAlturaBermaMargen = 100;
       



       public oTaludBermasModel(double iHmax, bool iBermaPie,double iBermaAncho, double iBermaAlto,double iBermaTaludH)
       {
           Hmax = iHmax;
           isBermaPie = iBermaPie;
           bermaAncho = iBermaAncho;
           bermaAlto = iBermaAlto;
           bermaTaludH = iBermaTaludH;        
       }



       public virtual Point3dCollection geometria(Point3d iPtoInicio)
       {

           //Puntos con la Geometría de la Berma
           Point3dCollection miColPto  = new Point3dCollection();

           Point3d miC0= iPtoInicio;

           miColPto.Add(miC0);


           //Añado Berma Pie
           if (isBermaPie)
           {
               Point3d miC1 = miC0.getFromIncXIncY(bermaAncho, 0, 0);
               Point3d miC2 = miC1.getFromTalud(true,isBermaDesmonte, bermaTaludH, bermaAlto);

               miColPto.Add(miC1);
               miColPto.Add(miC2);
           }
           else
           {
               Point3d miC3 = miC0.getFromTalud(true, isBermaDesmonte, bermaTaludH, bermaAlto);
               miColPto.Add(miC3);
           }


           //Inicio el Bucle de Añadir Talud de Berma 
           Point3d miPrevio = miColPto[miColPto.Count - 1];
           Point3d miNext1 = Point3d.Origin;
           Point3d miNext2 = Point3d.Origin;

           double miHMax = this.Hmax + KAlturaBermaMargen;

           double miHBerma = 0;

           while (miHBerma < miHMax)
           {
               //Berma Horizontal
               miNext1 = miPrevio.getFromIncXIncY(bermaAncho, 0, 0);

               //Berma Talud
               miNext2 = miNext1.getFromTalud(true,isBermaDesmonte, bermaTaludH, bermaAlto);

               miColPto.Add(miNext1);
               miColPto.Add(miNext2);

               miPrevio = miNext2;

               miHBerma = miHBerma + bermaAlto;

           }

           return miColPto;
       
       }

    }
}
