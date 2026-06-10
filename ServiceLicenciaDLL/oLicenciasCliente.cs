using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNex.Net.BussinesObject.Licencia
{
    using engNex.Net.BussinesObject.Usb;
    using System.Security;
    using System.Runtime.InteropServices;
    using engNex.Net.BussinesObject.E;
    using System.IO;
    using tadLayLan;
    using System.Net;
    using System.Net.Sockets;


    /// <summary>
    /// CLASE ESTATICA LLAMADA CHECK LICENCIA
    /// </summary>
    /// <example>ol.e()  --> Llama al Validador</example>
    public class ol
   {
        /// <summary>
        /// LLAMADA AL VALIDADOR DE LICENCIA
        /// </summary>
        public static void e(bool isDemo, bool isTmp)
        {
            oCheckLicenciaTadilRoad o = new oCheckLicenciaTadilRoad(isTmp);
            if (!isDemo) 
             o.validar();
        }

   }

     internal class oCheckLicenciaTadilRoad
     {
        #region "FIELDS PRIVADOS"

         private static object usbCache = null;
         private static bool _isTmp = false;

         #endregion
        #region "CONSTRUCTOR"

         public oCheckLicenciaTadilRoad(bool isTmp)
         {
             _isTmp = isTmp;
         }

        #endregion
        #region "METODOS PUBLICOS"

        public void validar()
        {
            validarPrivate();
        }

        #endregion
        #region "METODOS PRIVADOS"

         private void validarPrivate()
         {

             object i = this.getUsbDisk();

             string miFileLicenciaFullName = this.getFileLicencia(i);

             string miIdLicenciaEncryptada = this.getIdLicenciaEncriptada(miFileLicenciaFullName).Trim();

             string miIdLicenciaSinEncryptar = this.getIdLicenciaSinEncriptar(miIdLicenciaEncryptada);

             string[] miLstIDS = this.getColumnas(miIdLicenciaSinEncryptar);

             this.checkKey1(miLstIDS[0].Trim());

             this.checkKey2(miLstIDS[1].Trim(), getIdUsb(i));

             if (_isTmp)
             {
                 checkd();
             }
         }

         private object getUsbDisk()
        {

            string miRutaCache = string.Empty;

            if (usbCache != null)
            {
               miRutaCache = ((oUsbDisk)usbCache).letra + @":\" + SecureStringToString(this.getFolderLic()) + @"\" + SecureStringToString(this.getFileLic());
            }


                 
            if (File.Exists(miRutaCache))
            {

                return usbCache;

            }
            else
            {

                object miDisk1 = oUsbDiskService.findUsbDiskByFile(SecureStringToString(this.getFolderLic()), SecureStringToString(this.getFileLic()));

                if (((oUsbDisk)miDisk1).isNull())
                {
                    throw new oExLicencia(strError.eNoFichLic);
                }

                usbCache = miDisk1;

                return miDisk1;
            }
     
        }

        private string getFileLicencia(object i)
        {
            string miFile= ((oUsbDisk) i).letra + @":\" + SecureStringToString(this.getFolderLic()) + @"\" + SecureStringToString(this.getFileLic());

            if (!System.IO.File.Exists(miFile)) throw new oExLicencia(strError.eNoFichLic);

            return miFile;

        }

        private string getIdLicenciaEncriptada(string i)
        {
            string miLicenciaEncriptada=  System.IO.File.ReadAllText(i).Trim();

            if (string.IsNullOrEmpty(miLicenciaEncriptada)) throw new oExLicencia(strError.eNoFichLicNulo);

            return miLicenciaEncriptada;
        }

        private string getIdLicenciaSinEncriptar(string i)
        {

            try
            {

                string j = SecureStringToString(this.getKey2());

                return E.oSC.d(i,j);
            }
            catch (Exception)
            {

                throw new oExLicencia(strError.eErrorIDLic);
            }

        }

        private string[] getColumnas(string i)
        {
            string[] miColumnas = i.Split(';');

            if (miColumnas.Count() != 2) throw new oExLicencia(strError.eErrorIDLicCampos);

            return miColumnas;
        }

        private void checkKey1(string i)
        {

            if (!SecureStringToString(this.getKey1()).Equals(i))
            {
                throw new oExLicencia(strError.eControladorAcceso);
            }

        }
        private string getIdUsb(object i)
        {
            return ((oUsbDisk)i).i();

        }

         /// <summary>
         /// Validamos el USB
         /// </summary>
         /// <param name="i">ID USB</param>
         /// <param name="j">ID USB TXT</param>
        private void checkKey2(string i,string j)
        {

            if (! i.Equals(j))
            {
                throw new oExLicencia(strError.eControladorAcceso);
            }

        }

        private SecureString ConvertToSecureString(string password)
        {


            var securePassword = new SecureString();
            foreach (char c in password)
                securePassword.AppendChar(c);
            securePassword.MakeReadOnly();
            return securePassword;
        }

        private string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

         private void checkd()
         {
             bool ok = true;
             try
             {
                 const string ntpServer = "pool.ntp.org";
                 var ntpData = new byte[48];
                 ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)

                 var addresses = Dns.GetHostEntry(ntpServer).AddressList;
                 var ipEndPoint = new IPEndPoint(addresses[0], 123);
                 var s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                 s.Connect(ipEndPoint);
                 s.Send(ntpData);
                 s.Receive(ntpData);
                 s.Close();

                 ulong i = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | (ulong)ntpData[43];
                 ulong f = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | (ulong)ntpData[47];

                 var m = (i * 1000) + ((f * 1000) / 0x100000000L);
                 var n = (new DateTime(1900, 1, 1)).AddMilliseconds((long)m);

                 string a = "2020";
                 string b = "1";
                 int c = Convert.ToInt32(a);
                 int d = Convert.ToInt32(b);

                 ok = (n.Date.Year < c);
                 if (!ok)
                 {
                     ok = (n.Date.Year == c);
                     if (ok) ok = (n.Date.Month <= d);
                 }
             }
             catch
             {
                 throw new oExLicencia(strError.eInternet);
             }

             if (!ok)
             {
                 throw new oExLicencia(strError.eCaducado);
             }
         }

        #endregion
        #region "DATOS A CONFIGURAR"

        private SecureString getKey1()
        {
            return ConvertToSecureString("ACTISA");
        }
        private SecureString getKey2()
        {
            if(_isTmp)
                return ConvertToSecureString("Ethg-rF56lO_0iujR--630OO_-%jfHEA-PThgrDF__heFTG529_201705_tempROAD");
            else
                return ConvertToSecureString("Ethg-rF56lO_0iujR--630OO_-%jfHEA-PThgrDF__heFTG529");
        }
        private SecureString getFolderLic()
        {
            return ConvertToSecureString("TADIL_ROAD");
        }
        private SecureString getFileLic()
        {
            return ConvertToSecureString("TADIL_ROAD.lic");
        }

        #endregion
     }

}
