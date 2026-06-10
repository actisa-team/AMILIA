using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Tools
{

    using System.IO;
    using engNet.Extension.Double;
    using engNet.Extension.String;
    using tadLayLan;
    


    public class oSeccionPuenteDwg
    {


        private List<oPuenteCodigoDwg> mLstBloques;

        private string mFolderSecciones;



       public oSeccionPuenteDwg(string iFolderSecciones)
       {
           this.mFolderSecciones = iFolderSecciones;
           this.mLstBloques = getListadoBloques();
       }



        public string getFileNameConExtension (string iCodeSeccion, double iAnchoMaximoTablero,double iLuzEntrePilas)
       {

           string miCodeBloque = this.getCodigoBusqueda(iCodeSeccion);
           double miCantoPuente = this.getCalculoCantoPuente(iLuzEntrePilas);

    
           oPuenteCodigoDwg miBloqueCodigo= mLstBloques.FindAll(p => (p.codigo == miCodeBloque && 
                                                                     p.canto >= miCantoPuente &&
                                                                     p.ancho >= iAnchoMaximoTablero)).FirstOrDefault();

 
           if (miBloqueCodigo == null)
           {
             
               return string.Empty;
           }
           else
           {
               return miBloqueCodigo.fileNameConExtension;
           }
       }


        private string getCodigoBusqueda (string iCodeSeccion)
        {

            if (iCodeSeccion.Length != 6)
            {
                throw new Exception(string.Format(strError.eCodigoSeccion, iCodeSeccion));
            }

            //Particularidad El Código DOVELA -- POSCON -- POSVAR --> Devuelven una sección EST-01-DOVELA-10-84.dwg


            if (iCodeSeccion == "DOVELA" | iCodeSeccion == "POSCON" | iCodeSeccion == "POSVAR")
            {
                return "DOVELA";
            }
            else
            {
                return iCodeSeccion;
            }

        }



        private double getCalculoCantoPuente (double iLuzEntrePilas)
        {
            return iLuzEntrePilas / 22.0;
        }

       private List<oPuenteCodigoDwg> getListadoBloques ()
       {

           //EST-01-VIGPRE-10-84.dwg
           // 10 --> Canto x10
           // 84 --> Ancho Tablero x10
           //Este nombre marca una sección de Canto 1 metro y Ancho Tablero 8.4metros
           string[] miFilesPaths = Directory.GetFiles(this.mFolderSecciones   , "*.dwg");

           string miFileNameSinExtension = string.Empty;
           List<string> miStringSplit;

           List<oPuenteCodigoDwg> miListadoBloques = new List<oPuenteCodigoDwg>();

           oPuenteCodigoDwg miPuenteCode;

           foreach (string item in miFilesPaths)
           {
               miFileNameSinExtension = Path.GetFileNameWithoutExtension(item);

               miStringSplit = oExtensionString.lineaSplit(miFileNameSinExtension, "-");

               miPuenteCode = new oPuenteCodigoDwg(miStringSplit[2], miStringSplit[3], miStringSplit[4],miFileNameSinExtension);

               miListadoBloques.Add(miPuenteCode);
   
           }


           return miListadoBloques;

       }

    }

    internal class oPuenteCodigoDwg
    {

        public string codigo { get; private set; }
        public double canto { get;private set; }
        public double ancho { get; private set; }
        public string fileNameConExtension { get; private set; }

        public oPuenteCodigoDwg(string iCodigo, string iEspesorFromDwgName, string iAnchoFromDwgName, string iFileNameSinExtension)
        {
            this.codigo = iCodigo;
            this.canto = Convert.ToDouble(iEspesorFromDwgName) / 10.0;
            this.ancho = Convert.ToDouble(iAnchoFromDwgName) / 10.0;
            this.fileNameConExtension = iFileNameSinExtension +".dwg";
        }


        public oPuenteCodigoDwg(string iCodigo, double iEspesorMetros, double iAnchoMetros, string iFileNameConExtension)
        {
            this.codigo = iCodigo;
            this.canto = iEspesorMetros;
            this.ancho = iAnchoMetros;
            this.fileNameConExtension = iFileNameConExtension;

        }



    }
}
