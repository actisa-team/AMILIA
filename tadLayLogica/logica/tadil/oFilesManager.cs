using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tadLayLogica
{

    using engNet.eventos;
    using System.Reflection;
    using System.IO;
    using System.Windows.Forms;
    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayShare;
    using tadLayShare.puntos;
    using Newtonsoft.Json;
    using Terrenos;


    /// <summary>
    /// MANAGER FILES
    /// </summary>
    public class oFilesManager: IDisposable
    {

        #region "EVENTOS"

        public event EventHandler<oEventArgs<string>> evFileAppUpdate;
        public event EventHandler<oEventArgs<string>> evFileBbddUpdate;

        #endregion
        #region "FIELDS PRIVADOS"

        private string mFolderUserInstall = string.Empty;
        private string mFolderDatNormas = string.Empty;
        private string mFolderDatPrecios = string.Empty;
        private string mFolderHelp = string.Empty;

        private string mFileNormasCarreteras = string.Empty;
        private string mFileNormasCarreterasKv = string.Empty;

        private string mFileApp = string.Empty;
        private string mFileBbdd = string.Empty;
        #endregion


        #region "CONSTRUCTOR"

       public oFilesManager()
       {

       }
       #endregion
        #region "CONSTANTES-PUBLICAS"

        //FICHERO TADIL
        public readonly string KFileAppExtensionWithDot = ".tadil";
        public  string KFileAppExtensionFiltro = "Tadil Project Files (*.tadil)|*.tadil";

        //FICHERO NORMAS VP ; RP
        public  string KFileNormasExtensionWithDot = ".tadno";
        public  string KFileNormasExtensionFiltro = "Tadil Files Norma (*.tadno)|*.tadno";

        //FICHERO NORMAS KV
        public string KFileNormasKvExtensionWithDot = ".tadkv";
        public string KFileNormasKvExtensionFiltro = "Tadil Files Kv (*.tadkv)|*.tadkv";

        //FICHERO BD
        public string KFileBdExtensionWithDot = ".tadbd";
        public string KFileBdExtensionFiltro = "Tadil Files Bbdd (*.tadbd)|*.tadbd";

        //FICHERO DWG
        public string KFileDwgExtensionWithDot = ".dwg";
        public string KFileDwgExtensionFiltro = "Autocad Files (*.dwg)|*.dwg";

        //FICHERO CSV
        public string KFileCsvExtensionWithDot = ".csv";
        public string KFileCsvExtensionFiltro = "Comma Separated Value (*.csv)|*.csv";

        //FICHERO JPG
        public string KFileJpgExtensionWithDot = ".jpg";
        public string KFileJpgExtensionFiltro = "Imagen (*.jpg)|*.jpg";

        //FICHERO CONFIG
        public readonly string KFileAppConfig = "AMILIA.dll.config";

        //FICHERO TXT
        public readonly string KFileTxTExtensionWithDot = ".txt";
        public string KFileTxTExtensionFiltro = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";

        #endregion
        #region "CONSTANTES FOLDER NAMES"

        //FOLDER ROOT
        private const string KFolderApp = @"\app";
        private const string KFolderImg = @"\img";
        private const string KFolderCad = @"\cad";
        private const string KFolderDat = @"\dat";
        private const string KFolderHel = @"\hel";
        //FOLDER CHILD
        private const string KFolderDatTadil = @"\tadil";
        private const string KFolderDatTadbd = @"\tadbd";
        private const string KFolderDatNormas = @"\normas";
        private const string KFolderDatDefault = @"\default";

        #endregion
        #region "CONSTANTES FILES NAMES"

        private const string KFileNameTemplateApp = "tadilTemplateApp.tadil";
        private const string KFileNameTemplateBbdd = "tadilTemplateBb.tadbd";
        private const string KFileNameTemplateFileNormasVpRp = "tadilTemplateNormaInstruccionCarreteras.tadno";
        private const string KFileNameTemplateFileNormasKv = "tadilTemplateNormaInstruccionCarreteras.tadkv";

        private const string KFileNameDatNormas = "NormaInstruccionCarreteras.tadno";
        private const string KFileNameDatNormasKv = "NormaInstruccionCarreterasKv.tadkv";

        private const string KFileNameVehiculoConsumo = "vehiculoConsumo.csv";
        private const string KFileNameVehiculoMantenimiento = "vehiculoMantenimiento.csv";
        private const string KFileNameMatrizDecision = "matrizDecision.csv";

        private const string KFileNameHelp_ES = "tadil_Help_ES.pdf";
        private const string KFileNameHelp_EN = "tadil_Help_EN.pdf";
        private const string KFileNameHelp_FR = "tadil_Help_FR.pdf";

        private const string KFileNameGuide_ES = "tadil_Guide_ES.pdf";
        private const string KFileNameGuide_EN = "tadil_Guide_EN.pdf";
        private const string KFileNameGuide_FR = "tadil_Guide_FR.pdf";


        #endregion
        #region "PROPIEDADES"

        /// <summary>
        /// FICHERO BASE DATOS
        /// </summary>
        public string fileBbdd
        {

            get
            {
                if (string.IsNullOrEmpty(mFileBbdd))
                {
                    throw new oExPropertieNullValue("File Data Base");
                }
                else if (!File.Exists(mFileBbdd))
                {
                    throw new FileNotFoundException(mFileBbdd);
                }
                else
                {
                    return mFileBbdd;
                }

            }
            set
            {
                mFileBbdd = value;

                if (evFileBbddUpdate != null)
                {
                    evFileBbddUpdate(null, new oEventArgs<string>(mFileBbdd));
                }
            }


        }
        /// <summary>
        /// FICHERO APLICACIÓN
        /// </summary>
        public string fileApp
        {

            get
            {

                if (string.IsNullOrEmpty(mFileApp))
                {
                    throw new oExPropertieNullValue("File App Data");
                }
                else if (!File.Exists(mFileApp))
                {
                    throw new FileNotFoundException(mFileApp);
                }
                else
                {
                    return mFileApp;
                }

            }
            set
            {
                mFileApp = value;

                if (evFileAppUpdate != null)
                {
                    evFileAppUpdate(null, new oEventArgs<string>(mFileApp));
                }
            }

        }
        /// <summary>
        /// FILE NORMAS *.TADNO
        /// </summary>
        public string fileNormasCarreteras
        {
            get
            {

                if (string.IsNullOrEmpty(mFileNormasCarreteras))
                {
                    string miFile = folderDatNormas + @"\" + KFileNameDatNormas;

                    if (File.Exists(miFile))
                    {
                        mFileNormasCarreteras = miFile;
                    }
                    else
                    {
                        throw new FileNotFoundException(miFile);
                    }

                }

                return mFileNormasCarreteras;


            }

            set
            {
                mFileNormasCarreteras = value;
            }




        }
        /// <summary>
        /// FILE NORMAS KV *.TADKV
        /// </summary>
        public string fileNormasCarreterasKv
        {


            get
            {

                if (string.IsNullOrEmpty(mFileNormasCarreterasKv))
                {
                    string miFile = folderDatNormas + @"\" + KFileNameDatNormasKv;

                    if (File.Exists(miFile))
                    {
                        mFileNormasCarreterasKv = miFile;
                    }
                    else
                    {
                        throw new FileNotFoundException(miFile);
                    }

                }

                return mFileNormasCarreterasKv;

            }

            set
            {
                mFileNormasCarreterasKv = value;
            }




        }
        /// <summary>
        /// FICHERO PLANTILLA APLICACION
        /// </summary>
        public string fileTemplateApp
        {
            get
            {
                string miFile = folderDatApp + @"\" + KFileNameTemplateApp;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }
        /// <summary>
        /// FICHERO PLANTILLA BASE DATOS
        /// </summary>
        public string fileTemplateBbdd
        {
            get
            {
                string miFile = folderDatBbdd + @"\" + KFileNameTemplateBbdd;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }
        /// <summary>
        /// FICHERO PLANTILLA NORMAS VP
        /// </summary>
        public string fileTemplateNormasVpRp
        {
            get
            {
                string miFile = this.folderDatNormas + @"\" + KFileNameTemplateFileNormasVpRp;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);
                }
            }
        }
        /// <summary>
        /// FICHERO PLANTILLA NORMAS KV
        /// </summary>
        public string fileTemplateNormasKv
        {
            get
            {
                string miFile = this.folderDatNormas + @"\" + KFileNameTemplateFileNormasKv;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);
                }
            }
        }
        /// <summary>
        /// FICHERO TABLA VEHICULOS CONSUMO (Valores Defecto)
        /// </summary>
        public string fileDatVehiculoConsumo
        {
            get
            {
                string miFile = folderDatDefault + @"\" + KFileNameVehiculoConsumo;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }
        /// <summary>
        /// Tabla Vehiculos Mantenimiento (Valores Defecto)
        /// </summary>
        public string fileDatVehiculoMantenimiento
        {
            get
            {
                string miFile = folderDatDefault + @"\" + KFileNameVehiculoMantenimiento;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }
        /// <summary>
        /// Tabla Matriz Decisión (ValoresDefecto)
        /// </summary>
        public string fileDatMatrizDecisionDefault
        {
            get
            {
                string miFile = folderDatDefault + @"\" + KFileNameMatrizDecision;

                if (File.Exists(miFile))
                {
                    return miFile;
                }



                throw new FileNotFoundException(miFile);


            }
        }

        #endregion
        #region "FOLDER"
        /// <summary>
        /// RUTA DE INSTALACION
        /// </summary>    
        public string folderUserInstallApp
        {

            get
            {

                if (oTadil.data.isDebug)
                {
                    mFolderUserInstall = @"D:\00-TADIL\10.00-Tadil";
                }
                else
                {

                    if (string.IsNullOrEmpty(mFolderUserInstall))
                    {
                        string myAssembly = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        string[] cadena = myAssembly.Split('\\');
                        int longitud=cadena[cadena.Count() - 1].Length;
                        //string myPathInstall = myAssembly.Remove(myAssembly.Length - KFolderApp.Length);
                        string myPathInstall = myAssembly.Remove(myAssembly.Length - longitud);

                        if (Directory.Exists(myPathInstall))
                        {
                            mFolderUserInstall = myPathInstall;
                        }
                        else
                        {
                            throw new DirectoryNotFoundException(myPathInstall);
                        }

                    }
                }


                return mFolderUserInstall;

            }


            set
            {
                //CASO DEBUG
                mFolderUserInstall = value;


            }


        }
        /// <summary>
        /// Carpeta UserInstall\Dat"
        /// </summary>
        public string folderDat
        {
            get
            {
                string miFolder = folderUserInstallApp + KFolderDat;

                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException(miFolder);
                }
            }
        }
        /// <summary>
        ///Carpeta UserInstall\Dat\normas
        /// </summary>
        public string folderDatNormas
        {
            get
            {

                if (string.IsNullOrEmpty(mFolderDatNormas))
                {

                    string myFolder = folderDat + KFolderDatNormas;

                    if (Directory.Exists(myFolder))
                    {
                        mFolderDatNormas = myFolder;
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(myFolder);
                    }
                }

                return mFolderDatNormas;

            }


        }




        /// <summary>
        /// Carpeta UserInstall\Dat\tadil"
        /// </summary>
        public string folderDatApp
        {
            get
            {
                string miFolder = folderDat + KFolderDatTadil;

                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException(miFolder);
                }
            }
        }
        /// <summary>
        /// Carpeta UserInstall\Dat\tadbd"
        /// </summary>
        public string folderDatBbdd
        {
            get
            {
                string miFolder = folderDat + KFolderDatTadbd;

                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException(miFolder);
                }
            }
        }
        /// <summary>
        /// Carpeta UserInstall\Dat\Default"
        /// </summary>
        public string folderDatDefault
        {
            get
            {
                string miFolder = folderDat + KFolderDatDefault;

                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException(miFolder);
                }
            }
        }
        #endregion
        #region "IMG \ GIS"
        /// <summary>
        /// Carpeta UserInstall\img"
        /// </summary>
        public string folderImg
        {
            get
            {
                string miFolder = folderUserInstallApp + KFolderImg;
                           
                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException(miFolder);
                }
            }
        }



        /// <summary>
        ///  Carpeta UserInstall\img\gis"
        /// </summary>
        public string folderImgGis
        {

            get
            {
                string miFolder = folderImg + @"\gis\";
                       
                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException(miFolder);
                }
            }
        }

        public string fileImgGis(string iFileName)
        {


            string miFile = folderImgGis + iFileName;

            if (File.Exists(miFile))
            {
                return miFile;
            }
            else
            {
                throw new oExFileNotFoundImg(iFileName);
            
            }
   
        }


        #endregion
        #region "CAD"
        /// <summary>
        /// Carpeta  UserInstall/CAD
        /// </summary>
        public string folderCad
        {
            get
            {
                if (Directory.Exists(folderUserInstallApp + KFolderCad))
                {
                    return folderUserInstallApp + KFolderCad;
                }
                else
                {
                    if (oTadil.data.isDebug)
                    {
                        return Environment.CurrentDirectory;
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(folderUserInstallApp + KFolderCad);
                    }   
                }    
            } 
        }
        /// <summary>
        /// Carpeta UserInstall\cad\sec\est
        /// </summary>
        public string folderCadSecPuentes
        {
            get
            {
                string miFolder = folderCad + @"\sec\est\";

                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException(miFolder);
                }
            }
        }
        /// <summary>
        /// Carpeta UserInstall\cad\sec\Tun
        /// </summary>
        public string folderCadSecTun
        {
            get
            {
                string miFolder = folderCad + @"\sec\tun\";

                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {

                    if (oTadil.data.isDebug)
                    {
                        return Environment.CurrentDirectory;
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(miFolder);
                    }

                }
            }
        }
        /// <summary>
        /// Carpeta Sección de las Barreras
        /// </summary>
        public string folderCadSecBar
        {

            get
            {
                string miFolder = folderCad + @"\sec\bar\";

                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                  throw new DirectoryNotFoundException(miFolder);
                }
            }
 
        }
        /// <summary>
        /// Carpeta Bloques Zonas Gis
        /// </summary>
        public string folderCadGis
        {

            get
            {
                string miFolder = folderCad + @"\gis\";

                if (Directory.Exists(miFolder))
                {
                    return miFolder;
                }
                else
                {
                    throw new DirectoryNotFoundException(miFolder);
                }
            }

        }

        #endregion
        #region "HELP"

        /// <summary>
        /// Carpeta de Ayuda UserInstall\Hel\
        /// </summary>
        public string folderHelp
        {
            get
            {

                if (string.IsNullOrEmpty(mFolderHelp))
                {
                    string myFolder = folderUserInstallApp + KFolderHel;

                    if (Directory.Exists(myFolder))
                    {
                        mFolderHelp = myFolder;
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(myFolder);
                    }
                }

                return mFolderHelp;

            }
        }
        /// <summary>
        /// Carpeta Ayuda Español
        /// </summary>
        public string folferHelp_ES
        {
            get
            {
                return this.folderHelp + @"\es";
            }
        }
        /// <summary>
        /// Carpeta Ayuda INGLES
        /// </summary>
        public string folferHelp_EN
        {
            get
            {
                return this.folderHelp + @"\en";
            }
        }
        /// <summary>
        /// Carpeta Ayuda FRANCES
        /// </summary>
        public string folferHelp_FR
        {
            get
            {
                return this.folderHelp + @"\fr";
            }
        }
        /// <summary>
        /// FICHERO AYUDA ESPAÑOL
        /// </summary>
        public string fileHelp_ES
        {
            get
            {
                string miFile = this.folferHelp_ES  + @"\" + KFileNameHelp_ES;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }
        /// <summary>
        /// FICHERO AYUDA INGLES
        /// </summary>
        public string fileHelp_EN
        {
            get
            {
                string miFile = this.folferHelp_EN + @"\" + KFileNameHelp_EN;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }
        /// <summary>
        /// FICHERO AYUDA FRANCES
        /// </summary>
        public string fileHelp_FR
        {
            get
            {
                string miFile = this.folferHelp_FR + @"\" + KFileNameHelp_FR;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }

        /// <summary>
        /// FICHERO GUIA ESPAÑOL
        /// </summary>
        public string fileGuide_ES
        {
            get
            {
                string miFile = this.folferHelp_ES + @"\" + KFileNameGuide_ES;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }
        /// <summary>
        /// FICHERO GUIA INGLES
        /// </summary>
        public string fileGuide_EN
        {
            get
            {
                string miFile = this.folferHelp_EN + @"\" + KFileNameGuide_EN;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }
        /// <summary>
        /// FICHERO AYUDA FRANCES
        /// </summary>
        public string fileGuide_FR
        {
            get
            {
                string miFile = this.folferHelp_FR + @"\" + KFileNameGuide_FR;

                if (File.Exists(miFile))
                {
                    return miFile;
                }
                else
                {
                    throw new FileNotFoundException(miFile);

                }
            }
        }

        #endregion
        #region "FILES-SELECT"

        //NEW
        public string selectNewFileApp()
        {
          //Copio el master
          string miFileAppNew = saveAsFileFromDialog(fileTemplateApp, KFileAppExtensionFiltro);

          return miFileAppNew;

        }
        public string selectNewFileBaseDatos()
        {
            //Copio el master
            string miFileNew = saveAsFileFromDialog(fileTemplateBbdd, KFileBdExtensionFiltro);

            return miFileNew;

        }
        public string selectNewFileNormasKv()
        {
            //Copio el master
            string miFileNew = saveAsFileFromDialog(this.fileTemplateNormasKv, KFileNormasExtensionFiltro);

            return miFileNew;

        }
        public string selectNewFileNormasVpRp()
        {
            //Copio el master
            string miFileNew = saveAsFileFromDialog(this.fileTemplateNormasVpRp, KFileNormasExtensionFiltro);

            return miFileNew;

        }

        #endregion
        #region "FILES-GET"

        public string getFileApp()
        {

            string miFileOut = string.Empty;

            OpenFileDialog miDialogo = new OpenFileDialog();

            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name + " | " + strGeneralUser.uiFileAppSelect;
            miDialogo.Filter = KFileAppExtensionFiltro;
                miDialogo.Multiselect = false;

            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                return miFileOut;
            }
            else
            {
                return string.Empty;
            }

        }
        public string getFileTxt()
        {

            string miFileOut = string.Empty;

            OpenFileDialog miDialogo = new OpenFileDialog();

            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name + " | " + strGeneralUser.uiFileAppSelect;
            miDialogo.Filter = KFileTxTExtensionFiltro;
            miDialogo.Multiselect = false;

            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                return miFileOut;
            }
            else
            {
                return string.Empty;
            }

        }
        public string getFileTxtOrMdt()
        {

            string miFileOut = string.Empty;

            OpenFileDialog miDialogo = new OpenFileDialog();

            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name + " | " + strGeneralUser.uiFileAppSelect;
            miDialogo.Filter = "Archivos de texto y MDT (*.txt;*.amilia_mdt)|*.txt;*.amilia_mdt|Archivos de texto (*.txt)|*.txt|Archivos MDT (*.amilia_mdt)|*.amilia_mdt|Todos los archivos (*.*)|*.*";
            miDialogo.Multiselect = false;

            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                return miFileOut;
            }
            else
            {
                return string.Empty;
            }

        }
        public string getFileAsc()
        {

            string miFileOut = string.Empty;

            OpenFileDialog miDialogo = new OpenFileDialog();

            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name + " | " + strGeneralUser.uiFileAppSelect;
            miDialogo.Filter = "Archivos ASC (*.asc)|*.asc";
            miDialogo.Multiselect = false;

            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                return miFileOut;
            }
            else
            {
                return string.Empty;
            }

        }
        public string getFileBbdd()
        {

            string miFileOut = string.Empty;

            OpenFileDialog miDialogo = new OpenFileDialog();
            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name + " | " + strGeneralUser.uiFileBbSelect;
            miDialogo.Filter = KFileBdExtensionFiltro;
            miDialogo.Multiselect = false;


            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                return miFileOut;
            }
            else
            {
                return string.Empty;
            }

        }
        public string getFileVpRp()
        {

            string miFile = getFileFromDialog(KFileNormasExtensionFiltro);

            if (!string.IsNullOrEmpty(miFile))
            {
                return miFile;
            }
            else
            {
                return string.Empty;
            }


        }
        public string getFileVpKv()
        {
            string miFile = getFileFromDialog(KFileNormasKvExtensionFiltro);

            if (!string.IsNullOrEmpty(miFile))
            {
                return miFile;
            }
            else
            {
                oTadil.data.UserInfo.showInfo(string.Format(strGeneralUser.uiNoFichSelecionado));

                return string.Empty;
            }



        }
        public string getFileImagenGis()
        {
            string miFileOut = string.Empty;

            OpenFileDialog miDialogo = new OpenFileDialog();

            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name + " | " + strFrmGisGeneral.uiFileImgSelect;
            miDialogo.Filter = KFileJpgExtensionFiltro;
            miDialogo.InitialDirectory = oTadil.data.Files.folderImgGis;
            miDialogo.Multiselect = false;


            if (miDialogo.ShowDialog() == DialogResult.OK)
            {
                miFileOut = miDialogo.FileName;
                return miFileOut;
            }
            else
            {
                return string.Empty;

            }



        }

        #endregion
        #region "FILES SAVEAS"

        public string saveAsFileApp()
        {
            return saveAsFileFromDialog(this.fileApp, KFileAppExtensionFiltro);
        }
        public string saveAsFileBbdd()
        {
            return saveAsFileFromDialog(this.fileBbdd, KFileBdExtensionFiltro);
        }
        public string saveAsFileFromDialog(string iFileToCopy, string iFiltro)
        {

            string miFileDestino = string.Empty;

            SaveFileDialog miDialogo = new SaveFileDialog();

            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name;
            miDialogo.Filter = iFiltro;


            DialogResult miRes = miDialogo.ShowDialog();


            if (miRes == DialogResult.OK && !string.IsNullOrEmpty(miDialogo.FileName))
            {
                miFileDestino = miDialogo.FileName;

                File.Copy(iFileToCopy, miFileDestino, true);

                return miFileDestino;

            }
            else
            {
                oTadil.data.UserInfo.showInfo(strError.eErrorSaveas);

                return string.Empty;
            }

        }
        public string getFileFromDialog(string iFiltro)
        {

            string myFileOut = string.Empty;

            OpenFileDialog myDialogo = new OpenFileDialog();

            var name = oTadil.KAppHeaderName;

            myDialogo.Title = name;
            myDialogo.Filter = iFiltro;
            myDialogo.Multiselect = false;


            if (myDialogo.ShowDialog() == DialogResult.OK)
            {
                myFileOut = myDialogo.FileName;
                return myFileOut;
            }
            else
            {
                return string.Empty;

            }
        }
        public string saveFileFromDialog(string iFiltro)
        {

            string miFileDestino = string.Empty;

            SaveFileDialog miDialogo = new SaveFileDialog();

            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name;
            miDialogo.Filter = iFiltro;


            DialogResult miRes = miDialogo.ShowDialog();


            if (miRes == DialogResult.OK && !string.IsNullOrEmpty(miDialogo.FileName))
            {
                miFileDestino = miDialogo.FileName;

                //File.Copy(iFileToCopy, miFileDestino, true);

                return miFileDestino;

            }
            else
            {
                oTadil.data.UserInfo.showInfo(strError.eErrorSaveas);

                return string.Empty;
            }

        }
        public string saveAsFileCsvFromDialog(string iFileName)
        {


            SaveFileDialog miDialogo = new SaveFileDialog();


            miDialogo.FileName = iFileName;

            var name = oTadil.KAppHeaderName;

            miDialogo.Title = name;
            miDialogo.Filter = KFileCsvExtensionFiltro;
            miDialogo.OverwritePrompt = true;


            DialogResult miRes = miDialogo.ShowDialog();


            if (miRes == DialogResult.OK && !string.IsNullOrEmpty(miDialogo.FileName))
            {
                return miDialogo.FileName;
            }
            else
            {
                oTadil.data.UserInfo.showInfo(strError.eProcesoCancelado);

                return string.Empty;
            }

        }
        public string saveAsFileDwgFromDialog(string iFileName)
        {


            SaveFileDialog miDialogo = new SaveFileDialog();

            miDialogo.InitialDirectory = Path.GetDirectoryName(iFileName);
            miDialogo.FileName = Path.GetFileNameWithoutExtension(iFileName);


            miDialogo.Title = oTadil.KAppHeaderName;
            miDialogo.Filter = KFileDwgExtensionFiltro;
            miDialogo.OverwritePrompt = true;


            DialogResult miRes = miDialogo.ShowDialog();


            if (miRes == DialogResult.OK && !string.IsNullOrEmpty(miDialogo.FileName))
            {
                return miDialogo.FileName;
            }
            else
            {
                oTadil.data.UserInfo.showInfo(strError.eProcesoCancelado);

                return string.Empty;
            }

        }
        
        struct nubepuntos
        {
            public List<Punto3d> puntos { get; set; }
            public double miMax { get; set; }
            public double miPendienteMax { get; set; }
            public int ucNodosPorHoja { get; set; }
        }
        public string SaveAsFileFromDialog_Puntos(List<Punto3d> puntos, string iFiltro, double miMax, double miPendienteMax, int ucNodosPorHoja)
        {
            string miFileDestino = string.Empty;
            SaveFileDialog miDialogo = new SaveFileDialog();

            var name = oTadil.KAppHeaderName;
            miDialogo.Title = name;
            miDialogo.Filter = iFiltro;
            miDialogo.FileName = "puntos3D.txt"; // Nombre por defecto

            DialogResult miRes = miDialogo.ShowDialog();

            nubepuntos nube = new nubepuntos
            {
                miMax = miMax,
                miPendienteMax = miPendienteMax,
                ucNodosPorHoja = ucNodosPorHoja,
                puntos = puntos
            };

            if (miRes == DialogResult.OK && !string.IsNullOrEmpty(miDialogo.FileName))
            {
                miFileDestino = miDialogo.FileName;

                try
                {
                    // Serializa la lista de puntos a JSON usando Newtonsoft.Json
                    string json = JsonConvert.SerializeObject(nube, Formatting.Indented);

                    // Guarda el JSON en el archivo
                    File.WriteAllText(miFileDestino, json);

                    return miFileDestino;
                }
                catch (Exception ex)
                {
                    oTadil.data.UserInfo.showInfo($"Error al guardar el archivo: {ex.Message}");
                    return string.Empty;
                }
            }
            else
            {
                oTadil.data.UserInfo.showInfo(strError.eErrorSaveas);
                return string.Empty;
            }
        }
        public string SaveAsFileFromDialog_Puntos(Triangulacion malla, string iFiltro)
        {
            string miFileDestino = string.Empty;
            SaveFileDialog miDialogo = new SaveFileDialog();

            var name = oTadil.KAppHeaderName;
            miDialogo.Title = name;
            miDialogo.Filter = iFiltro;
            miDialogo.FileName = "MDT.txt"; // Nombre por defecto

            DialogResult miRes = miDialogo.ShowDialog();

            if (miRes == DialogResult.OK && !string.IsNullOrEmpty(miDialogo.FileName))
            {
                miFileDestino = miDialogo.FileName;

                try
                {
                    // Serializa la lista de puntos a JSON usando Newtonsoft.Json
                    string json = JsonConvert.SerializeObject(malla, Formatting.Indented);

                    // Guarda el JSON en el archivo
                    File.WriteAllText(miFileDestino, json);

                    return miFileDestino;
                }
                catch (Exception ex)
                {
                    oTadil.data.UserInfo.showInfo($"Error al guardar el archivo: {ex.Message}");
                    return string.Empty;
                }
            }
            else
            {
                oTadil.data.UserInfo.showInfo(strError.eErrorSaveas);
                return string.Empty;
            }
        }

        public string SaveAsFileFromDialog(string json, string iFiltro,string nombre)
        {
            string miFileDestino = string.Empty;
            SaveFileDialog miDialogo = new SaveFileDialog();

            var name = oTadil.KAppHeaderName;
            miDialogo.Title = name;
            miDialogo.Filter = iFiltro;
            miDialogo.FileName = "Datos.txt"; // Nombre por defecto

            //DialogResult miRes = miDialogo.ShowDialog();

            //if (miRes == DialogResult.OK && !string.IsNullOrEmpty(miDialogo.FileName))
            //{
                // Obtener el directorio sin el nombre del archivo
                string directorio = Path.GetDirectoryName(oTadil.data.Files.fileApp);

                // Construir la nueva ruta con el nuevo nombre
                string nuevaRuta = Path.Combine(directorio, nombre+ "_001_Primaria" + ".txt");

                //miFileDestino = miDialogo.FileName;
                miFileDestino = nuevaRuta;
                try
                {
                    // Serializa la lista de puntos a JSON usando Newtonsoft.Json


                    // Guarda el JSON en el archivo
                    File.WriteAllText(miFileDestino, json);

                    return miFileDestino;
                }
                catch (Exception ex)
                {
                    oTadil.data.UserInfo.showInfo($"Error al guardar el archivo: {ex.Message}");
                    return string.Empty;
                }
            //}
            //else
            //{
            //    oTadil.data.UserInfo.showInfo(strError.eErrorSaveas);
            //    return string.Empty;
            //}
        }


        #endregion
        #region "DISPOSE"

        public void Dispose()
        {
            this.mFileApp = string.Empty;
            this.mFileBbdd = string.Empty;
            this.mFileNormasCarreteras = string.Empty;
            this.mFileNormasCarreterasKv = string.Empty;
        }

       #endregion
  
    }

 
}
