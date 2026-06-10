using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Comandos
{
   using System.Diagnostics;
    using tadLayShare;
    
    public class oComandoHelpView
    {
      /// <summary>
      /// OPEN FILE HELP
      /// </summary>
       public static void openFileHelp ()
       {

           eIdioma miIdiomaAplicación = oTadil.data.getIdioma();


           switch (miIdiomaAplicación)
           {
               case eIdioma.es: 
                   oComandoHelpView.openFile(oTadil.data.Files.fileHelp_ES);
                   break;
               case eIdioma.fr:
                   oComandoHelpView.openFile(oTadil.data.Files.fileHelp_FR);
                   break;
               case eIdioma.en:
                   oComandoHelpView.openFile(oTadil.data.Files.fileHelp_EN);
                   break;
               default:
                   throw new  oExEnumNotImplemented(miIdiomaAplicación.ToString());
           }

       }
       /// <summary>
       /// OPEN FILE GUIDE
       /// </summary>
       public static void openFileGuide()
       {

           eIdioma miIdiomaAplicación = oTadil.data.getIdioma();


           switch (miIdiomaAplicación)
           {
               case eIdioma.es:
                   oComandoHelpView.openFile(oTadil.data.Files.fileGuide_ES);
                   break;
               case eIdioma.fr:
                   oComandoHelpView.openFile(oTadil.data.Files.fileGuide_FR);
                   break;
               case eIdioma.en:
                   oComandoHelpView.openFile(oTadil.data.Files.fileGuide_EN);
                   break;
               default:
                   throw new oExEnumNotImplemented(miIdiomaAplicación.ToString());
           }

       }

       private static void openFile (string iFileName)
       {
           System.Diagnostics.Process miProceso = new System.Diagnostics.Process();
           miProceso.StartInfo.FileName = iFileName;
           miProceso.Start();
           miProceso.Close();
       }

    }
}
