using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet
{

    using engNet.CustomAtributos;

    using System.IO;
    using System.Text;
    using System.Reflection;
    using System.ComponentModel;
    
    public class oCsvWrite
    {

       /// <summary>
       /// MEJORAR AÑADIR ATRIBUTO INDEX Y PODER ORDENAR LOS CAMPOS EN EL EXCEL
       /// </summary>
       public static void write<T>(IEnumerable<T> data, string iPath, string iFileNameSinExtension)
       {

           System.IO.TextWriter output = new StreamWriter(iPath + @"\" + iFileNameSinExtension + ".csv");

           PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

           foreach (PropertyDescriptor prop in props)
           {
               if (prop.IsBrowsable)
               {
                   output.Write(prop.DisplayName); // header
                   output.Write(";");
               }

           }


           output.WriteLine();

           string miValor;

           foreach (T item in data)
           {
               foreach (PropertyDescriptor prop in props)
               {
                   if (prop.IsBrowsable)
                   {
                       miValor = prop.GetValue(item).ToString();

                       output.Write(miValor);
                       output.Write(";");
                   }
               }

               output.WriteLine();
           }

           output.Flush();
           output.Close();




       }

    }
}
