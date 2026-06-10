using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Comandos
{

    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Geometry;
    //using Autodesk.Aec.Geometry;
    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Colors;

    using System.Diagnostics;
    using System.IO;

    using engCadNet;
    using engCadNet.extension;

    using tadLayLogica.datos.proyecto;
    using tadLayLan;
    using tadLayLan.Tdi;

    
    public static  class oComandoObraLinealExport
    {

      public static void export(Guid iIdSolucion)
      {

          using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
          {

              string miNombreSolucion = string.Empty;


              using (oSolucion miSolucion = new oSolucion(iIdSolucion))
              {
                  miNombreSolucion = miSolucion.solucionData.nombre;
              }

               string miFileExport = oTadil.data.Files.saveAsFileDwgFromDialog(miNombreSolucion);

            
               if (string.IsNullOrEmpty(miFileExport))
               {
                   oTadil.data.UserInfo.procesoCancelado();
               }
               else
               {
                   string miFileFolder = Path.GetDirectoryName(miFileExport);
                   string miFileSinExtension = Path.GetFileNameWithoutExtension(miFileExport);
                   string miFilePlantaConExtension = miFileFolder + @"\" + miFileSinExtension + strFrmSolucion.uiPlanta + ".dwg";
                   string miFileSeccionesConExtension = miFileFolder + @"\" + miFileSinExtension + strFrmSolucion.uiSeccion + ".dwg";


                   //Creo el Eje Visibilidad
                   Stopwatch miMedicion = new Stopwatch();
                   miMedicion.Start();

                   ObjectIdCollection miCollectionToExport = new ObjectIdCollection();

                   #region "PLANTA"
                 
                   oTadilLayerPlanta miLayerPlanta = new oTadilLayerPlanta(iIdSolucion);
                   oTadilLayerDominioPublicoAdyacente miLayerDominioPublico = new oTadilLayerDominioPublicoAdyacente(iIdSolucion);
                   oTadilLayerExpropiacion miLayerExpropiacion = new oTadilLayerExpropiacion(iIdSolucion);

                   miCollectionToExport.addRange(miLayerPlanta.getEntidades());
                   miCollectionToExport.addRange(miLayerDominioPublico.getEntidades());
                   miCollectionToExport.addRange(miLayerExpropiacion.getEntidades());



                   if (miCollectionToExport.Count == 0)
                   {
                       oTadil.data.UserInfo.showInfo(strGeneralUser.uiExportarEntidadesNotFound);
                       return;
                   }
                   else
                   {
                       engCadNet.oExport.exportarEntidades(miCollectionToExport, miFilePlantaConExtension, true);
                   }

                   #endregion

                   #region "SECCIONES"
         
                   oTadilLayerSecciones miLayerSecciones = new oTadilLayerSecciones(iIdSolucion);

                   miCollectionToExport = new ObjectIdCollection();
                   miCollectionToExport = miLayerSecciones.getEntidades();

                   if (miCollectionToExport.Count == 0)
                   {
                       oTadil.data.UserInfo.showInfo(strGeneralUser.uiExportarEntidadesNotFound);
                       return;
                   }
                   else
                   {
                       engCadNet.oExport.exportarEntidades(miCollectionToExport, miFileSeccionesConExtension, true);
                   }

                   
                   #endregion

                   #region "GUARDAR LOS DATOS"

                   oSingletonDsApp.getInstance.getSolucion(iIdSolucion).isCompleteObraLinealExportar = true;

                   oSingletonDsApp.getInstance.solucionSave();

                   #endregion

                   //Tiempo
                   miMedicion.Stop();

                   oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.Minutes);
               }

            



              

              


             
          }

      }

  


    }
}
