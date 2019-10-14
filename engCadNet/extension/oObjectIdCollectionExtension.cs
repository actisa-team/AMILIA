using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet.extension
{
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    
   public static class oObjectIdCollectionExtension
    {

       /// <summary>
       ///Add Rango a la Colección
       /// </summary>
       public static void addRange(this ObjectIdCollection iCollectionToPopulate, ObjectIdCollection iCollectionToAdd)
       {
           foreach (ObjectId item in iCollectionToAdd)
           {
               iCollectionToPopulate.Add(item);


           }
       }


      


       

    }
}
