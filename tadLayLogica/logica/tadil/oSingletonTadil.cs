using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayLogica
{

    using System.IO;

    using tadLayLogica.datos.proyecto;
    using tadLayShare.puntos;
    using tadLayShare;
    using tadLayLogica.datos.BaseDatos;

    using System.Threading;
    using System.Globalization;

    using engNet.eventos;

   /// <summary>
   /// MANAGER TADIL
   /// </summary>
    public class oTadil:IDisposable
    {

        #region "CONSTANTES"

        public static string KAppHeaderName = "TADIL Road 3.1 AutoCad 2017";
        public const string KAppProductName = "Técnicas AutotrazadoDiseño Infraestructuras Lineales";
        public const string KAppCopyRight = "Copyright © ACTISA";
        public const string KAppMoreInfo = "www.actisa.net";

        public const bool IsDemo = false;
        public const bool IsTmp = false;
      
        #endregion
        #region "FIELDS PRIVADAS"

        private static oTadil mInstance = null;
        private oFilesManager mFilesManager = null;
        private oLayerManager mLayerManager = null;
        private oUserInfo mUserInfo = null;

        #endregion
        #region "CONSTRUCTOR"

        protected oTadil()
        {
            mUserInfo = new oUserInfo();
            mLayerManager = new oLayerManager();
            mFilesManager = new oFilesManager();
            if (IsDemo && !KAppHeaderName.Contains("Demo")) KAppHeaderName = KAppHeaderName + " Demo";
        }

        public static oTadil data
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new oTadil();

                    oTadilCore.evReset += new EventHandler<oEventArgs<bool>>(oTadilCore_evReset);
                }

                return mInstance;
            }
        }

        #endregion
        #region "Propiedades"

        /// <summary>
        /// Gestor de Capas
        /// </summary>
        public oLayerManager Layer
        {
            get
            {
                return mLayerManager;
            }
        }
        /// <summary>
        /// Gestor de InfoUsuario
        /// </summary>
        public oUserInfo UserInfo
        {
            get
            {
                return mUserInfo;
            }
        }
        /// <summary>
        /// Gestor de Ficheros
        /// </summary>
        public oFilesManager Files
        {
            get
            {
                return mFilesManager;
            }

            set
            {
                mFilesManager = value;
            
            }
        }
        /// <summary>
        /// Aplicación en MODO DEPURACION
        /// </summary>
        public bool isDebug
        {
            get
            {
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.StartsWith("win"))
                {
                    return true;
                }
                else
                {
                    return false;
                }          
            }       
        }
        /// <summary>
        /// Idioma
        /// </summary>
        public eIdioma idioma { get; private set; }

        #endregion
        #region "METODOS"




        /// <summary>
        /// AÑADIR IDIOMA AL APPCONFIG
        /// </summary>
        public void addIdiomaAppConfig(eIdioma iIdioma)
        {
            engNet.oAppConfigManager.AppSettingSetKey(oTadil.data.Files.KFileAppConfig, eAppConfigKey.lan.ToString(), iIdioma.ToString());
        }
        /// <summary>
        /// SET IDIOMA APLICACION
        /// </summary>
        public void setIdiomaAplicacion()
        {          
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.getIdioma().ToString());
        }


        /// <summary>
        /// OBTENER IDIOMA
        /// </summary>
        public eIdioma getIdioma ()
        {
            string miLenguaje = (string)engNet.oAppConfigManager.AppSettingGetKey(oTadil.data.Files.KFileAppConfig, eAppConfigKey.lan.ToString());

            return oExtensionEnumeraciones.getIdioma(miLenguaje);

        }




        /// <summary>
        /// LECTURA DEL APP CONFIG PARA DIBUJAR LOS ABANICOS-TRAMOS
        /// </summary>
        public bool drawAbanicos()
        {

            string miCode = engNet.oAppConfigManager.AppSettingGetKey(Files.KFileAppConfig, eAppConfigKey.mod.ToString());

            if (miCode == "1001")
            {
                return true;
            }
            else if (miCode == "10101")
            {
                return false;
            }
            else
            {
                throw new Exception("Error en la codificación de la clave [mod]");
            }
        }


        public bool displaySeccionesConError()
        {

            string miValor = engNet.oAppConfigManager.AppSettingGetKey(Files.KFileAppConfig, eAppConfigKey.errorSectionDisplay.ToString());

            bool miDisplay = Convert.ToBoolean(miValor);

            return miDisplay;

        }


        #endregion
        #region "DISPOSE"

        public void Dispose()
        {
            mFilesManager =null;
            mLayerManager = null;
            mUserInfo = null;
            mInstance = null;
        }

        #endregion

        //OLD
        static void oTadilCore_evReset(object sender, oEventArgs<bool> e)
        {
            if (e.Value)
            {
                mInstance = null;
                oTadilCore.evReset -= new EventHandler<oEventArgs<bool>>(oTadilCore_evReset);
            }
        }


    }



    public static class oTadilCore
    {
        public static event EventHandler<oEventArgs<bool>> evReset;
        public static void reset()
        {
            if (evReset != null)
            {
                evReset(null, new oEventArgs<bool>(true));
            }
        }
    }



}
