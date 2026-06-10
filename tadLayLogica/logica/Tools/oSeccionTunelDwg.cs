using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Tools
{

    using System.IO;

    using engNet.Extension.String;
    using tadLayLan;
    


    public class oSeccionTunelDwg
    {


        private List<oTunelCodigoDwg> mLstBloques;
        private string mFolderSecciones;



       public oSeccionTunelDwg(string iFolderSecciones)
       {
           this.mFolderSecciones = iFolderSecciones;
           this.mLstBloques = getListadoBloques();
       }



        public string getFileNameConExtension (string iCodeSeccion,bool iHasContravobeda, double iRmr, double iGaliboHorizontal)
       {


           if (iHasContravobeda)
           {
               iCodeSeccion = iCodeSeccion + "_C";
           }
      



    
           oTunelCodigoDwg miBloqueCodigo= mLstBloques.FindAll(p => (p.codigo == iCodeSeccion && 
                                                                     p.rmrFinal >= iRmr &&
                                                                     p.galiboHorizontal >= iGaliboHorizontal)).FirstOrDefault();

 
           if (miBloqueCodigo == null)
           {
             
               return string.Empty;
           }
           else
           {
               return miBloqueCodigo.fileNameConExtension;
           }
       }


       private List<oTunelCodigoDwg> getListadoBloques ()
       {

           //TUN-01-CIRCUL-000_200-90-60
           //TUN-03-HERRAD-000_200-90-60
           //TUN-03-HERRAD_C-000_200-90-60

           //000-200 RMR --> 0-20
           //90 --> Galibo Ancho x10 --> 9 metros
           //60 --> Galibo Vertical x10 --> 6 metros

           string[] miFilesPaths = Directory.GetFiles(this.mFolderSecciones   , "*.dwg");

           string miFileNameSinExtension = string.Empty;

           List<string> miDwgNameSplit;
           List<string> miRmrSplit;

           List<oTunelCodigoDwg> miListadoBloques = new List<oTunelCodigoDwg>();

           oTunelCodigoDwg miTunelCode;


           try
           {

               foreach (string item in miFilesPaths)
               {
                   miFileNameSinExtension = Path.GetFileNameWithoutExtension(item);

                   miDwgNameSplit = oExtensionString.lineaSplit(miFileNameSinExtension, "-");

                   miRmrSplit = oExtensionString.lineaSplit(miDwgNameSplit[3], "_");

                   miTunelCode = new oTunelCodigoDwg(miDwgNameSplit[2], miRmrSplit[0], miRmrSplit[1], miDwgNameSplit[4], miDwgNameSplit[5], miFileNameSinExtension);

                   miListadoBloques.Add(miTunelCode);

               }

           }
           catch (Exception)
           {
               oTadil.data.UserInfo.showInfo(strError.eAnalizandoFichero + miFileNameSinExtension);
           }




           return miListadoBloques;

       }

    }

    internal class oTunelCodigoDwg
    {

        public string codigo { get; private set; }
        public double rmrInicial { get;private set; }
        public double rmrFinal { get; private set; }
        public double galiboHorizontal  { get; private set; }
        public double galiboVertical  { get; private set; }

        public string fileNameConExtension { get; private set; }

        public oTunelCodigoDwg(string iCodigo, string iRmrInicialDwgName,string iRmrFinalDwgName, string iGaliboAnchoFromDwgName,  string iGaliboVerticalFromDgName,string iFileNameSinExtension)
        {
            this.codigo = iCodigo;

            this.rmrInicial = Convert.ToDouble(iRmrInicialDwgName) / 10.0;
            this.rmrFinal = Convert.ToDouble(iRmrFinalDwgName) / 10.0;
            this.galiboHorizontal = Convert.ToDouble(iGaliboAnchoFromDwgName)/10.0;
            this.galiboVertical = Convert.ToDouble(iGaliboVerticalFromDgName)/10.0;
            this.fileNameConExtension = iFileNameSinExtension +".dwg";
        }


  



    }
}
