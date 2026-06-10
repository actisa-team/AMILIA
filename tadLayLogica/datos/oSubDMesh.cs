using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using engCadNet;
using engNet.ClassT;
using tadLayLogica.Comandos;
using tadLayShare;
using tadLayShare.puntos;
using Terrenos;
using TriangulationTool;

namespace tadLayLogica.datos
{

    //CAD


    using dt = Autodesk.AutoCAD.DatabaseServices;


    public class oSubDMesh
    {


        private static string _triangulationFileName;
        private static string TriangulationFileName
        {
            get
            {
                if (_triangulationFileName == null)
                {
                    var acDoc = oCadManager.thisMdi;
                    var nombreFichero = "TADILtriangulation.dwg";
                    var directoryName = Path.GetTempPath();
                    var filePath = Path.Combine(directoryName, nombreFichero);
                    _triangulationFileName = filePath;
                }
                return _triangulationFileName;
            }
            set { _triangulationFileName = value; }
        }
        private static string MallaKey = "TADILMALLA";

        public static bool createTriangulation(List<Punto3d> iPuntos, String iNombre, double iMaxArista,
             ref List<Triangulo> misZonasMaxPendiente, double? iPendienteMax, string filename, int nodosXhoja, out Triangulacion triangulacion)
        {
            Terreno miTerreno = new Terreno(iPuntos);
            bool isEliminados = true;


            //Triangulacion_optimizada_2 triangulacion_Optimizada_2 = new Triangulacion_optimizada_2(miTerreno, iMaxArista, iNombre, ref isEliminados, nodosXhoja);

            Triangulacion miTriangulacion = new Triangulacion(miTerreno, iMaxArista, iNombre, ref isEliminados, nodosXhoja);
            //Triangulacion_optimizada miTriangulacion_optimizada = new Triangulacion_optimizada(miTerreno, iMaxArista, iNombre, ref isEliminados, nodosXhoja);
            if (iPendienteMax != null)
            {
                misZonasMaxPendiente = miTriangulacion.getPuntosZonasMaxPendiente((double)iPendienteMax);
            }
            triangulacion = miTriangulacion;

            return isEliminados;
        }

        public static bool createMeshFromPoints(List<Punto3d> iPuntos, String iNombre, double iMaxArista,
             ref List<Triangulo> misZonasMaxPendiente, double? iPendienteMax, string filename, out string handle, int nodosXhoja)
        {
            Terreno miTerreno = new Terreno(iPuntos);
            bool isEliminados = true;

            Triangulacion miTriangulacion = new Triangulacion(miTerreno, iMaxArista, iNombre, ref isEliminados, nodosXhoja);

            if (iPendienteMax != null)
            {
                misZonasMaxPendiente = miTriangulacion.getPuntosZonasMaxPendiente((double) iPendienteMax);
            }

            handle = createMesh(miTriangulacion, filename);    
            return isEliminados;
        }



        public static string createMesh(Triangulacion iTriangulacion, string filename,
            string key = "TADILMALLA",
            MemoryStream info = null)
        {

            Triangulacion = iTriangulacion;

            TriangulationManager.SaveToAutoDesk(iTriangulacion.getNodos,
                iTriangulacion.getTriangulos, filename, "_Tadil_MDT_" + Triangulacion.Name);
            OriginalDoc = oCadManager.thisMdi;

            oCadManager.documenteManager.DocumentActivated += acDocMgr_DocumentActivated;
            oCadManager.documenteManager.Open(filename, false);
            ////oCadManager.documenteManager.ExecuteInCommandContextAsync(async (o) => { oCadManager.documenteManager.Open(filename, false); }, null);
            //oCadManager.documenteManager.ExecuteInApplicationContext((o) =>
            //{
            //    oCadManager.documenteManager.Open(filename, false);
            //}, null);
            return "";
        }

        public static Triangulacion Triangulacion;
        public static string LastMdiName;
        private static Document TriangulationDoc;
        private static ObjectIdCollection AcObjIdColl;
        private static Document OriginalDoc;
        private static List<Triangulo> ZnpTriangulos; 
        

        private static void acDocMgr_DocumentActivated(object sender, DocumentCollectionEventArgs e)
        {
                oTools.zoomExtension();
                using (DocumentLock acLck = oCadManager.documenteManager.MdiActiveDocument.LockDocument())
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        var MallasEyeshotSS = oSs.getSsByLayer("_Tadil_MDT_" + Triangulacion.Name);
                        var miMalla = (DBObject)tr.GetObject(MallasEyeshotSS[0].ObjectId, OpenMode.ForWrite, false);

                        oXdata.setXdata(miMalla.ObjectId, MallaKey, Triangulacion.Name);
                        MemoryStream info = Triangulacion.guardarTriangulacion();
                        ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miMalla, tr, info,
                        Triangulacion.GetType().FullName);



                        TriangulationDoc = oCadManager.documenteManager.MdiActiveDocument;

                        AcObjIdColl = new ObjectIdCollection();

                        foreach (SelectedObject item in MallasEyeshotSS)
                        {
                            AcObjIdColl.Add(item.ObjectId);
                        }
                        tr.Commit();

                    }
                }
            oCadManager.documenteManager.DocumentActivated -= acDocMgr_DocumentActivated;
            oTools.zoomExtension();

        }


        private static void acDocMgr_DocumentActivated_Original(object sender, DocumentCollectionEventArgs e)
        {
            using (DocumentLock acLckDocCur = oCadManager.documenteManager.MdiActiveDocument.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable bt =
                        (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);

                    BlockTableRecord btr =
                        (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    if (!oLayer.HasLayer("_Tadil_MDT_" + Triangulacion.Name))
                        oLayer.addLayer("_Tadil_MDT_" + Triangulacion.Name,
                            Color.FromColorIndex(ColorMethod.ByLayer, 0).ColorIndex, true);
                    else
                        oLayer.deleteByLayer("_Tadil_MDT_" + Triangulacion.Name);



                    IdMapping acIdMap = new IdMapping();
                    TriangulationDoc.Database.WblockCloneObjects(AcObjIdColl, btr.ObjectId, acIdMap,
                        DuplicateRecordCloning.Ignore, false);

                    if (ZnpTriangulos.Count>0)
                        oComandoTerreno.añadirZonasDeNoPasoPendienteMaxima(ZnpTriangulos);



                    tr.Commit();



                }
            }

            oCadManager.documenteManager.DocumentActivated -= acDocMgr_DocumentActivated_Original;
            TriangulationDoc.CloseAndSave(TriangulationDoc.Name);
        }

        public static void  CopiarTriangulacionToOriginal(List<Triangulo> znpTriangulo)
        {
            ZnpTriangulos = znpTriangulo;
            oCadManager.documenteManager.DocumentActivated += acDocMgr_DocumentActivated_Original;
            oCadManager.documenteManager.MdiActiveDocument = OriginalDoc;
        }



        public static string createMeshDemo(string iLayerTerreno, Triangulacion iTriangulacion, string key = "TADILMALLA", MemoryStream info = null)
        {

            var handle = string.Empty;
            List<Triangulo> miLstTriangulos = iTriangulacion.getTriangulos;
            List<Punto3d> miLstPuntos = iTriangulacion.getNodos;

            System.Collections.Hashtable mNodos = new System.Collections.Hashtable();

            using (Transaction tr = oCadManager.StartTransaction())
            {

                BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);

                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                Point3dCollection miVertarray = new Point3dCollection();

                Int32Collection miFacearray = new Int32Collection();


                //Coleccion Vertices
                for (int i = 0; i < miLstPuntos.Count(); i++)
                {
                    miVertarray.Add(new Point3d(miLstPuntos[i].coordenadaX, miLstPuntos[i].coordenadaY, miLstPuntos[i].coordenadaZ));
                    mNodos.Add(miLstPuntos[i].getHashCode, i);
                }

                for (int j = 0; j < miLstTriangulos.Count(); j++)
                {
                    miFacearray.Add(3);
                    int indexVertA = (int)mNodos[miLstTriangulos[j].getVerticeA.getHashCode];
                    miFacearray.Add(indexVertA);
                    int indexVertB = (int)mNodos[miLstTriangulos[j].getVerticeB.getHashCode];
                    miFacearray.Add(indexVertB);
                    int indexVertC = (int)mNodos[miLstTriangulos[j].getVerticeC.getHashCode];
                    miFacearray.Add(indexVertC);
                }


                SubDMesh miMalla = new SubDMesh();
                miMalla.SetDatabaseDefaults();
                miMalla.SetLayerId(oLayer.getLayerObjId(iLayerTerreno), true);
                miMalla.SetSubDMesh(miVertarray, miFacearray, 0);


                btr.AppendEntity(miMalla);
                tr.AddNewlyCreatedDBObject(miMalla, true);

                oXdata.setXdata(miMalla.ObjectId, key, iTriangulacion.Name);

                if (info == null) info = iTriangulacion.guardarTriangulacion();
                ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miMalla, tr, info, iTriangulacion.GetType().FullName);
                handle = miMalla.Handle.ToString();
                tr.Commit();


            }
            return handle;
        }

        public static PolyFaceMesh getPolyFaceMeshFromName(string iName, bool isDemo)
        {
            PolyFaceMesh miMallaRes = null;
            PolyFaceMesh miMalla = null;

            using (dt.Transaction tr = oCadManager.StartTransaction())
            {
                var key = MallaKey;
                if (isDemo) key = iName;
                SelectionFilter miSelectionFilter = oSs.getFiltroByLayerListAndXdataKey(oLayer.getListLayerName(), key);
                var miObjectIds = oSs.getEntidadesByFilter(miSelectionFilter);
                if (miObjectIds != null)
                {
                    foreach (ObjectId item in miObjectIds)
                    {
                        miMalla = (PolyFaceMesh) tr.GetObject(item, OpenMode.ForRead);
                        if (isDemo) return miMalla;
                        List<String> name = oXdata.getXData(miMalla, MallaKey, " ");
                        if (name[0] == iName)
                        {
                            miMallaRes = miMalla;
                        }
                    }
                }
            }
            return miMallaRes;
        }


        public static SubDMesh getSubDMeshFromName(string iName, bool isDemo)
        {
            SubDMesh miMallaRes = null;
            SubDMesh miMalla = null;

            using (dt.Transaction tr = oCadManager.StartTransaction())
            {
                var key = MallaKey;
                if (isDemo) key = iName;
                SelectionFilter miSelectionFilter = oSs.getFiltroByLayerListAndXdataKey(oLayer.getListLayerName(), key);
                var miObjectIds = oSs.getEntidadesByFilter(miSelectionFilter);
                if (miObjectIds != null)
                {
                    foreach (ObjectId item in miObjectIds)
                    {
                        miMalla = (SubDMesh)tr.GetObject(item, OpenMode.ForRead);
                        if (isDemo) return miMalla;
                        List<String> name = oXdata.getXData(miMalla, MallaKey, " ");
                        if (name[0] == iName)
                        {
                            miMallaRes = miMalla;
                        }
                    }
                }
            }
            return miMallaRes;
        }

        public static List<oValDesT<string, string>> getLstSuperficies(bool isDemo,string buscado="")
        {

            List<oValDesT<string, string>> miLstValorDescripcion = new List<oValDesT<string, string>>();
            PolyFaceMesh miMalla = null;
            SubDMesh miSubdMesh = null;

            if (!isDemo)
            {
                using (dt.Transaction tr = oCadManager.StartTransaction())
                {
                    /*
                     * Se cambia a un if else para poder buscar solo la capa necesaria, si se cambia  solo se dejaria el else **juanma**
                     */
                    SelectionFilter miSelectionFilter = null;
                    if (buscado!="")
                    {
                        List<string> filteredLayerNames = new List<string>();

                        // Obtener la lista completa de nombres de capas
                        List<string> allLayerNames = oLayer.getListLayerName();

                        // Filtrar capas cuyos nombres contienen el substring especificado
                        foreach (string layerName in allLayerNames)
                        {
                            // Asegurarse de que layerName no sea null o vacío
                            if (!string.IsNullOrEmpty(layerName) && layerName.IndexOf(buscado, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                filteredLayerNames.Add(layerName);
                            }
                        }
                        miSelectionFilter = oSs.getFiltroByLayerListAndXdataKey(filteredLayerNames, MallaKey);
                    }
                    else
                    {
                        miSelectionFilter = oSs.getFiltroByLayerListAndXdataKey(oLayer.getListLayerName(),
                        MallaKey);
                    }


                    var miObjectIds = oSs.getEntidadesByFilter(miSelectionFilter);
                    if (miObjectIds != null)
                    {

                        foreach (ObjectId item in miObjectIds)
                        {
                            try
                            {
                                miMalla = (PolyFaceMesh)tr.GetObject(item, OpenMode.ForRead);
                            }
                            catch (Exception)
                            {
                                miSubdMesh = (SubDMesh)tr.GetObject(item, OpenMode.ForRead);
                            }
                            if (miMalla != null)
                            {
                                List<String> name = oXdata.getXData(miMalla, MallaKey, " ");
                                miLstValorDescripcion.Add(new oValDesT<string, string>(miMalla.Handle.ToString(),
                                    name[0]));
                            }
                            else
                            {
                                List<String> name = oXdata.getXData(miSubdMesh, MallaKey, " ");
                                miLstValorDescripcion.Add(new oValDesT<string, string>(miSubdMesh.Handle.ToString(),
                                    name[0])); 
                            }
                            miMalla = null;
                        }
                    }


                }
            }
            else
            {
                miLstValorDescripcion = getLstSuperficiesDemo();

            }

            return miLstValorDescripcion;
        }

        private static List<oValDesT<string, string>> getLstSuperficiesDemo()
        {
            List<oValDesT<string, string>> miLstValorDescripcion = new List<oValDesT<string, string>>();
            SubDMesh miMalla = null;
            using (dt.Transaction tr = oCadManager.StartTransaction())
            {
                SelectionFilter miSelectionFilter = oSs.getFiltroByLayerListAndXdataKey(oLayer.getListLayerName(),
                    "Cartografia1");
                var miObjectIds = oSs.getEntidadesByFilter(miSelectionFilter);
                if (miObjectIds.Count != 0)
                {

                    foreach (ObjectId item in miObjectIds)
                    {
                        miMalla = (SubDMesh)tr.GetObject(item, OpenMode.ForRead);
                        miLstValorDescripcion.Add(new oValDesT<string, string>(miMalla.Handle.ToString(), "Cartografia1"));
                    }
                }
                else
                {
                    var handler = string.Empty;
                    MemoryStream miTriangSer = new MemoryStream();
                    var stream = Terreno.getTerrenoDemo(0);
                    stream.CopyTo(miTriangSer);
                    using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                    {
                        if (!oLayer.HasLayer("MDT_DEMO_Cartografia1")) oLayer.addLayer("MDT_DEMO_Cartografia1", Color.FromColorIndex(ColorMethod.ByLayer, 0).ColorIndex, true);

                        miTriangSer.Position = 0;
                        handler = createTerreno("MDT_DEMO_Cartografia1", miTriangSer, "Cartografia1");
                    }

                    miLstValorDescripcion.Add(new oValDesT<string, string>(handler, "Cartografia1"));
                }

                miSelectionFilter = oSs.getFiltroByLayerListAndXdataKey(oLayer.getListLayerName(), "Cartografia2");
                miObjectIds = oSs.getEntidadesByFilter(miSelectionFilter);
                if (miObjectIds.Count != 0)
                {

                    foreach (ObjectId item in miObjectIds)
                    {
                        miMalla = (SubDMesh)tr.GetObject(item, OpenMode.ForRead);
                        miLstValorDescripcion.Add(new oValDesT<string, string>(miMalla.Handle.ToString(), "Cartografia2"));
                    }
                }
                else
                {
                    var handler = string.Empty;
                    MemoryStream miTriangSer = new MemoryStream();
                    var stream = Terreno.getTerrenoDemo(1);
                    stream.CopyTo(miTriangSer);
                    using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                    {
                        if (!oLayer.HasLayer("MDT_DEMO_Cartografia2")) oLayer.addLayer("MDT_DEMO_Cartografia2", Color.FromColorIndex(ColorMethod.ByLayer, 0).ColorIndex, true);

                        miTriangSer.Position = 0;
                        handler = createTerreno("MDT_DEMO_Cartografia2", miTriangSer, "Cartografia2");
                    }

                    miLstValorDescripcion.Add(new oValDesT<string, string>(handler, "Cartografia2"));
                }
                tr.Commit();
            }



            return miLstValorDescripcion;
        }



        private static string createTerreno(String iLayer,MemoryStream iTriangulacion, string key)
        {
            var triangulacion = new Triangulacion(new InfoTriangulacionNew(InfoTriangulacion.recuperaInformacion(iTriangulacion)));
            return oSubDMesh.createMeshDemo(iLayer, triangulacion, key, iTriangulacion);
        }

        public static List<string> getLstNames(bool isDemo)
        {

            List<string> miLstNames = new List<string>();

            if (isDemo)
            {
                PolyFaceMesh miMalla = null;
                PolyFaceMesh miMallaWrite = null;

                using (dt.Transaction tr = oCadManager.StartTransaction())
                {
                    SelectionFilter miSelectionFilter = oSs.getFiltroByLayerListAndXdataKey(oLayer.getListLayerName(),
                        MallaKey);
                    var miObjectIds = oSs.getEntidadesByFilter(miSelectionFilter);
                    if (miObjectIds != null)
                    {

                        foreach (ObjectId item in miObjectIds)
                        {
                            miMalla = (PolyFaceMesh) tr.GetObject(item, OpenMode.ForRead);



                            List<String> name = oXdata.getXData(miMalla, MallaKey, " ");

                            miLstNames.Add(name[0]);



                        }
                    }



                }
            }
            else
            {
                miLstNames.Add("Cartografia1");
                miLstNames.Add("Cartografia2");
            }

            return miLstNames;
        }

        public static Triangulacion getTriangulacion(ObjectId miMallaId)
        {
            Xrecord miXrecord = oXrecord.getXrecord(miMallaId, "info");

            InfoTriangulacionNew miInfo = InfoTriangulacionNew.recuperaInformacion(oXrecord.getStream(miXrecord));
            Triangulacion miTriangulacion = new Triangulacion(miInfo);

            return miTriangulacion;
        }


        public static bool existName(string iName, bool isDemo)
        {
            List<string> mimallas = getLstNames(isDemo);
            return mimallas.Contains(iName);
        }

        public static MemoryStream getStreamCartografia2()
        {

            Assembly _assembly = Assembly.GetExecutingAssembly();
            var stream = _assembly.GetManifestResourceStream("Terrenos.mdtsDEMO.Cartografia2.trr");
            MemoryStream miTriangSer = new MemoryStream();
            stream.CopyTo(miTriangSer);
            return miTriangSer;
        }

       
    }
}
    