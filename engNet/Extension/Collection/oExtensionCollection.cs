using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.Extension.Collection
{
   public static class oExtensionCollection
    {

       public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)                                    
       {
               foreach (T item in source)
               {
                   destination.Add(item);
               }
       }
       

    }
}
