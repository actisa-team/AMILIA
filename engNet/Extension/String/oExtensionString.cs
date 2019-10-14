using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace engNet.Extension.String
{

    


   public static class oExtensionString
    {


       public static string createFilePathFull (this string iPath, string iFileNameSinExtension, string iExtensionSinPunto)
       {
           return iPath + @"\" + iFileNameSinExtension + "." + iExtensionSinPunto;
       }


       public static string removeEspaciosBlanco (this string iStr)
       {
           return iStr.Replace(" ", "");
       }

       /// <summary>
       /// Descompongo una Linea de texto, Segun un Separador
       /// </summary>
       /// <param name="iLine">Linea a Separar</param>
       /// <param name="iStrSep">Caracteres Separadores ' ' ';' '|' ':' </param>
       /// <returns>Listado de Textos</returns>
       public static List<string> lineaSplit( this string iLine, string iStrSep)
       {

           string[] txtPartes;


           if (iStrSep.Trim().Length == 0)
           {
               txtPartes = Regex.Split(iLine.Trim(), @"\s+");
           }
           else
           {
               txtPartes = iLine.Split(iStrSep.ToCharArray());
           }


           List<string> myListDes = new List<string>();

           foreach (string s in txtPartes)
           {

               if (!string.IsNullOrEmpty(s))
               {
                   myListDes.Add(s);
               }


           }

           return myListDes;


       }


       /// <summary>
       /// Devolver el Nombre de un Fichero SIN Extension
       /// </summary>
       public static string removeExtension(this string iFileNameConExtension)
       {
           return System.IO.Path.GetFileNameWithoutExtension(iFileNameConExtension);
       }


       public static bool isNombreSolucionValido(string iName)
       {
           bool isNombreValido = true;

           foreach (char miCaracter in iName)
           {
               int miValAscii = (int)miCaracter;
               bool isEspacio = (miValAscii == 32);
               bool isCaracter = true;
               bool isCaracteMay = true;
               bool isNumero = true;
               bool isCaracterEsp = true;
               if (!((miValAscii >= 97) && (miValAscii <= 122))) isCaracter = false;
               if (!((miValAscii >= 65) && (miValAscii <= 90))) isCaracteMay = false;
               if (!((miValAscii >= 48) && (miValAscii <= 57))) isNumero = false;
               if (!((miValAscii == 95) || (miValAscii == 225) || (miValAscii == 233) || (miValAscii == 237) || (miValAscii == 243) ||
                   (miValAscii == 250) || (miValAscii == 193) || (miValAscii == 201) || (miValAscii == 205) || (miValAscii == 211) ||
                   (miValAscii == 218) || (miValAscii == 241) || (miValAscii == 209) || (miValAscii == 36) || (miValAscii == 45))) isCaracterEsp = false;

               bool isOk = (isCaracter || isCaracteMay || isNumero || isCaracterEsp || isEspacio);

               if (!isOk) isNombreValido = false;
           }
           return isNombreValido;
       }


    }
}
