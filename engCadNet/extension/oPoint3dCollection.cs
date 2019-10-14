using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet.extension
{
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    
   public static class oPoint3dCollection
    {

       /// <summary>
       /// Obtener el Area de una Coleción ORDENADA de Puntos
       /// </summary>
       public static double getArea(this Point3dCollection iLstPoint3d)
       {


           Polyline miPol = new Polyline(iLstPoint3d.Count);

           int miVertexNum = 0;

          

      

           foreach (Point3d item in iLstPoint3d)
           {
               miPol.AddVertexAt(miVertexNum,item.to2d(), 0, 0, 0);
               miVertexNum++;
           }

           return miPol.Area;

       }


       public static void viewPto(this Point3dCollection iLstPoint3d,double iRadio,string iLayer)
       {
           foreach (Point3d item in iLstPoint3d)
           {
               oCircle.addCircle3D(item, iRadio, iLayer);
           }
       }

    }
}
