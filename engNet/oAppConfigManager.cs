using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet
{

    using System.Diagnostics;
    using System.Configuration;
    using System.IO;
    using System.Reflection;


    public  class oAppConfigManager
    {

        /// Modificar el Valor del una Key del CONFIG
        /// </summary>
        /// <param name="iConfigFileName">Nombre del Fichero del Config</param>
        /// <param name="iKey">Valor de la Llave a Modificar</param>
        /// <param name="iValue">Nuevo Valor del Parametro</param>
        public static void AppSettingSetKey(string iConfigFileName, string iKey, string iValue)
        {




            ExeConfigurationFileMap myFileConfig = new ExeConfigurationFileMap();

            myFileConfig.ExeConfigFilename = GetAssemblyPath() + @"\" + iConfigFileName;

            if (!File.Exists(myFileConfig.ExeConfigFilename))
            {
                throw new FileNotFoundException();
            }


            Configuration myConfig = ConfigurationManager.OpenMappedExeConfiguration(myFileConfig, ConfigurationUserLevel.None);

            myConfig.AppSettings.Settings[iKey].Value = iValue;

            myConfig.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection("appSettings");

        }



        public static string AppSettingGetKey(string iConfigFileName, string iKey)
        {

            ExeConfigurationFileMap myFileConfig = new ExeConfigurationFileMap();

            myFileConfig.ExeConfigFilename = GetAssemblyPath() + @"\" + iConfigFileName;

            Configuration myConfig = ConfigurationManager.OpenMappedExeConfiguration(myFileConfig, ConfigurationUserLevel.None);

            return myConfig.AppSettings.Settings[iKey].Value;

        }

        /// <summary>
        /// Obtengo la Ruta de la Carpeta donde se encuentra la DLL
        /// </summary>
        /// <returns>Ruta de la Carpeta</returns>
        /// <example>
        /// Assembly c:\cadnet\xyztocad\xyztocad.dll
        /// Return c:\cadnet\xyztocad</example>
        private static string GetAssemblyPath()
        {

            //Obtengo el Assemblie
            Assembly assem = Assembly.GetExecutingAssembly();
            string name = assem.GetName().Name;
            string path = assem.Location;
            FileInfo myFile = new FileInfo(path);
            return myFile.DirectoryName;

        }

    }
}
