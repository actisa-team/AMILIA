using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Security.Cryptography;

namespace engNex.Net.BussinesObject.E
{

    using System.IO;


    public class oSC
    {
        /// <summary>
        /// Encriptar Texto
        /// </summary>
        /// <param name="iTexto">Texto a Encriptar</param>
        /// <param name="iKey">Key de Llave</param>
        /// <returns>Texto Encriptado</returns>
        public static string c(string iTexto, string iKey)
        {

            byte[] miKeyArray; 

            byte[] miTextoArray = UTF8Encoding.UTF8.GetBytes(iTexto);
 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            miKeyArray = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(iKey));

            md5.Clear();

            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = miKeyArray;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateEncryptor();
            byte[] resultado = convertir.TransformFinalBlock(miTextoArray, 0, miTextoArray.Length); 
            tripledes.Clear();

            return Convert.ToBase64String(resultado, 0, resultado.Length); 
        }
        /// <summary>
        /// Desencriptar Texto
        /// </summary>
        /// <param name="iTexto">Texto Encriptado</param>
        /// <param name="iKey">Key Llave</param>
        /// <returns>Texto Sin Encriptar</returns>
        public static string d(string iTexto, string iKey)
        {

            byte[] miKeyArray;

            byte[] miTextoArray = Convert.FromBase64String(iTexto); 
 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            miKeyArray = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(iKey));
            md5.Clear();

            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = miKeyArray;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            byte[] resultado = convertir.TransformFinalBlock(miTextoArray, 0, miTextoArray.Length);
            tripledes.Clear();

            string cadena_descifrada = UTF8Encoding.UTF8.GetString(resultado);
            return cadena_descifrada;
        }
    }
}
