using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Comandos
{
    
    //NET
    using System.Diagnostics;
    
    
    //CAD
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Geometry;
   // using Autodesk.Aec.Geometry;
    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Colors;


   
   //TADIL
    using tadLayLogica.datos.proyecto;
    using engCadNet;
    using tadLayShare;
    using tadLayLan;
    
   
    //EJE TADIL
    using EjeDeTrazado.puntosDelEje;
    using EjeDeTrazado.componentes;
    using EjeDeTrazado;
    using tadLayShare.puntos;

    using tadLayLogica.EjeTrazadoTadil;
    using System.IO;
    using tadLayLogica.Secciones.Calzada;
    using tadLayData;
    using Newtonsoft.Json;


    /// <summary>
    /// EJE TRAZADO ANGELES ENTIDADES CIVIL3D
    /// </summary>
    public class oComandoEjeTrazadoTadil
    {
        /// <summary>
        /// CREAR EJE TRAZADO (ALGORITMO ANGELES + ENTIDADES CIVIL3D)
        /// </summary>
        public static void create(eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (oSolucion miSolucion = new oSolucion(iIdSolucion))
                {


                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        //Creo la medicion
                        Stopwatch miMedicion = new Stopwatch();
                        miMedicion.Start();

                        BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                        //CREO EJE TRAZADO TADIL ANGELES
                        double miBombeo = 2;
                        if (iEstudioTipo == eEstudioTipo.ESTINF)
                        {
                            oSeccionRoadCompletaSinGis miSeccionCompletaSinGis = oSingletonProyecto.getInstance.seccionCalzadaCompleta;
                            miBombeo = (double)miSeccionCompletaSinGis.BombeoPC;
                        }


                        EjeTrazado miEjeTrazadoTadil = oFactoryEjeTrazadoTadil.getEjeTrazadoTadil(iIdSolucion, miSolucion.roadDesign.peralte, miBombeo);


                        MemoryStream miEjeMem = miEjeTrazadoTadil.guardarEjeTrazado();
                        Polyline miEje = new Polyline();
                        int index = 0;

                        foreach (var componente in miEjeTrazadoTadil.getComponentes)
                        {
                            foreach (var componentPoint in componente.getComponentPoints())
                            {
                                miEje.AddVertexAt(index, new Point2d(componentPoint[0], componentPoint[1]), 0, 0, 0);
                                index++;
                            }
                        }

                        miEje.Layer = new oTadilLayerEjeTrazado(iIdSolucion).name;

                        btr.AppendEntity(miEje);
                        tr.AddNewlyCreatedDBObject(miEje, true);

                        oXdata.setXdata(miEje.ObjectId, "tadilEje", miSolucion.idSolucion.ToString());
                        ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEje, tr, miEjeMem, miEjeTrazadoTadil.GetType().FullName);


                        //Añado el Eje de Trazado al fichero.
                        oDalTbSolucion.addEjeTrazado(miSolucion.idSolucion, miEje.Handle.ToString(), miEjeTrazadoTadil.Length/1000);

                        ////EXPORTAR
                        //miEjeTrazadoTadil.exportarEjeTrazado(miEjeTrazadoTadil.guardarEjeTrazado());
                        

                        miMedicion.Stop();


                        oCadManager.thisEditor.UpdateScreen();

                        //Info UI
                        oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);

                        tr.Commit();
                    }

                }
            }

        }
        public static void create_Amilia(eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (oSolucion miSolucion = new oSolucion(iIdSolucion))
                {
                    dsApp.tbSolucionRow miRow = oDalTbSolucion.getSolucion(iIdSolucion);

                    if (!miRow.IsEjeTrazado_AmiliaNull())
                    {
                        // 1. Obtenemos el array de bytes de la columna del DataSet
                        byte[] datosRecuperados = miRow.EjeTrazado_Amilia;

                        // 2. Creamos un MemoryStream con esos bytes
                        using (MemoryStream ms = new MemoryStream(datosRecuperados))
                        {
                            // 3. Usamos tu método estático para reconstruir la clase
                            EjeTrazado miEjeTrazadoTadil = EjeTrazado.recuperaEjeTrazado(ms);

                            // Ahora miEjeRecuperado tiene todas las listas y variables intactas
                            var eje = Planta(miEjeTrazadoTadil, miRow.nombre);
                            //Añado el Eje de Trazado al fichero.
                            oDalTbSolucion.addEjeTrazado(miSolucion.idSolucion, eje.Handle.ToString(), miEjeTrazadoTadil.Length / 1000);
                        }



                        //var miEjeTrazadoTadil = JsonConvert.DeserializeObject<EjeTrazado>(miRow.EjeTrazado_Amilia);
                        
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo("El eje de trazado no tiene datos de Amilia guardados.");
                    }
                }
            }
                    
        }
        /// <summary>
        /// ROTULAR EJE TRAZADO (ALGORITMO ANGELES + ENTIDADES CIVIL3D)
        /// </summary>
        public static void rotular (Guid iIdSolucion)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (oSolucion miSolucion = new oSolucion(iIdSolucion))
                {
                    //CREO EJE TRAZADO TADIL ANGELES
                    EjeDeTrazado.puntosDelEje.EjeTrazado miEje = null;
                    if (!miSolucion.solucionData.amilia)
                    {
                        Xrecord miXrecord = engCadNet.oXrecord.getXrecord(miSolucion.ejeTrazado.ObjectId, "info");
                        miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));
                    }
                    else
                    {
                        byte[] datosRecuperados = miSolucion.solucionData.EjeTrazado_Amilia;
                        using (MemoryStream ms = new MemoryStream(datosRecuperados))
                        {
                            // 3. Usamos tu método estático para reconstruir la clase
                            miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(ms);
                        }
                    }


                    EjeTrazado miEjeTrazadoTadil = miEje;


                    //GESTION CAPAS
                    oTadilLayerEjeTrazadoTadilPk miLayerPk =  new oTadilLayerEjeTrazadoTadilPk(iIdSolucion);
                    oTadilLayerEjeTrazadoTadilPs miLayerPs =new oTadilLayerEjeTrazadoTadilPs(iIdSolucion);

                    miLayerPk.deleteItems();
                    miLayerPs.deleteItems();


                    oEjeTrazadoTadilRotular miEjeTrazadoCivilRotular = new oEjeTrazadoTadilRotular();

                    miEjeTrazadoCivilRotular.rotular(miEjeTrazadoTadil, miLayerPk.name, miLayerPs.name);

                }

                oTadil.data.UserInfo.procesoTerminado();
            }
        }
        public static void rotular_Amilia(Guid iIdSolucion)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (oSolucion miSolucion = new oSolucion(iIdSolucion))
                {
                    dsApp.tbSolucionRow miRow = oDalTbSolucion.getSolucion(iIdSolucion);

                    if (!miRow.IsEjeTrazado_AmiliaNull())
                    {
                        // 1. Obtenemos el array de bytes de la columna del DataSet
                        byte[] datosRecuperados = miRow.EjeTrazado_Amilia;

                        // 2. Creamos un MemoryStream con esos bytes
                        using (MemoryStream ms = new MemoryStream(datosRecuperados))
                        {
                            // 3. Usamos tu método estático para reconstruir la clase
                            EjeTrazado miEjeTrazadoTadil = EjeTrazado.recuperaEjeTrazado(ms);

                            // Ahora miEjeRecuperado tiene todas las listas y variables intactas
                            Rotular(miEjeTrazadoTadil, miRow.nombre);
                        }



                        //var miEjeTrazadoTadil = JsonConvert.DeserializeObject<EjeTrazado>(miRow.EjeTrazado_Amilia);

                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo("El eje de trazado no tiene datos de Amilia guardados.");
                    }
                }
            }
        }

        /// <summary>
        /// IMPRIMIR INFORME TRAZADO
        /// </summary>
        public static List<EjeDeTrazado.oInformeEje> getListado (Guid iIdSolucion)
        {
            EjeTrazado miEjeTrazadoTadil;
            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {

                //Creo la medicion
                Stopwatch miMedicion = new Stopwatch();
                miMedicion.Start();

                //CREO EJE TRAZADO TADIL ANGELES
                EjeDeTrazado.puntosDelEje.EjeTrazado miEje = null;
                if (!miSolucion.solucionData.amilia)
                {
                    Xrecord miXrecord = engCadNet.oXrecord.getXrecord(miSolucion.ejeTrazado.ObjectId, "info");
                    miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));
                }
                else
                {
                    byte[] datosRecuperados = miSolucion.solucionData.EjeTrazado_Amilia;
                    using (MemoryStream ms = new MemoryStream(datosRecuperados))
                    {
                        // 3. Usamos tu método estático para reconstruir la clase
                        miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(ms);
                    }
                }
                /*Xrecord miXrecord = engCadNet.oXrecord.getXrecord(miSolucion.ejeTrazado.ObjectId, "info");
                EjeDeTrazado.puntosDelEje.EjeTrazado miEje =
                    EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));*/

                miEjeTrazadoTadil = miEje;
            }


            return miEjeTrazadoTadil.escribirInforme();
                 
        }
        public static List<EjeDeTrazado.oInformeEje> getListado_Amilia(Guid iIdSolucion)
        {
            EjeTrazado miEjeTrazadoTadil=null;
            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {

                //Creo la medicion
                Stopwatch miMedicion = new Stopwatch();
                miMedicion.Start();

                dsApp.tbSolucionRow miRow = oDalTbSolucion.getSolucion(iIdSolucion);
                if (!miRow.IsEjeTrazado_AmiliaNull())
                {
                    // 1. Obtenemos el array de bytes de la columna del DataSet
                    byte[] datosRecuperados = miRow.EjeTrazado_Amilia;

                    // 2. Creamos un MemoryStream con esos bytes
                    using (MemoryStream ms = new MemoryStream(datosRecuperados))
                    {
                        // 3. Usamos tu método estático para reconstruir la clase
                        miEjeTrazadoTadil = EjeTrazado.recuperaEjeTrazado(ms);
                    }



                    //var miEjeTrazadoTadil = JsonConvert.DeserializeObject<EjeTrazado>(miRow.EjeTrazado_Amilia);

                }
            }


            return miEjeTrazadoTadil.escribirInforme();

        }


        public static void createFerrocarril(eEstudioTipo iEstudioTipo, Guid iIdSolucion, List<Componente> iListaComponentes, List<Vertice> iListaVertices)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (oSolucion miSolucion = new oSolucion(iIdSolucion))
                {


                    using (Transaction tr = oCadManager.StartTransaction())
                    {

                        BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);


                        EjeTrazado miEjeTrazadoTadil = EjeTrazadoTadil.oFactoryEjeTrazadoTadil.getEjeTrazadoTadilFerrocarril(iIdSolucion, miSolucion.roadDesign.peralte, iListaComponentes, iListaVertices);


                        MemoryStream miEjeMem = miEjeTrazadoTadil.guardarEjeTrazado();
                        Polyline miEje = new Polyline();
                        int index = 0;

                        foreach (var componente in miEjeTrazadoTadil.getComponentes)
                        {
                            foreach (var componentPoint in componente.getComponentPoints())
                            {
                                miEje.AddVertexAt(index, new Point2d(componentPoint[0], componentPoint[1]), 0, 0, 0);
                                index++;
                            }

                        }

                        miEje.Layer = new oTadilLayerEjeTrazado(iIdSolucion).name;

                        btr.AppendEntity(miEje);
                        tr.AddNewlyCreatedDBObject(miEje, true);

                        oXdata.setXdata(miEje.ObjectId, "tadilEje", miSolucion.idSolucion.ToString());
                        ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEje, tr, miEjeMem, miEjeTrazadoTadil.GetType().FullName);


                        //Añado el Eje de Trazado al fichero.
                        oDalTbSolucion.addEjeTrazado(miSolucion.idSolucion, miEje.Handle.ToString(), miEjeTrazadoTadil.Length / 1000, true);



                        oCadManager.thisEditor.UpdateScreen();


                        tr.Commit();
                    }

                }
            }

        }

        public static void exportar(string iNombreArchivo, eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {

            Stopwatch miMedicion = new Stopwatch();
            miMedicion.Start();
            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {

                Polyline miLwEjePerfilTrazado = miSolucion.ejeTrazado;


                Xrecord miXrecordTrazado = engCadNet.oXrecord.getXrecord(miLwEjePerfilTrazado.ObjectId, "info");
                EjeDeTrazado.puntosDelEje.EjeTrazado miTrazado = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecordTrazado));

                miTrazado.exportarEjeTrazado(iNombreArchivo);


                //Tiempo
                miMedicion.Stop();

                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.Minutes);
            }
        }

        public static void exportar_Amilia(string iNombreArchivo, eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {
            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {
                Stopwatch miMedicion = new Stopwatch();
                miMedicion.Start();
                dsApp.tbSolucionRow miRow = oDalTbSolucion.getSolucion(iIdSolucion);

                if (!miRow.IsEjeTrazado_AmiliaNull())
                {
                    // 1. Obtenemos el array de bytes de la columna del DataSet
                    byte[] datosRecuperados = miRow.EjeTrazado_Amilia;

                    // 2. Creamos un MemoryStream con esos bytes
                    using (MemoryStream ms = new MemoryStream(datosRecuperados))
                    {
                        // 3. Usamos tu método estático para reconstruir la clase
                        EjeTrazado miEjeTrazadoTadil = EjeTrazado.recuperaEjeTrazado(ms);
                        miEjeTrazadoTadil.exportarEjeTrazado(iNombreArchivo);
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo("El eje de trazado no tiene datos de Amilia guardados.");
                }
                //Tiempo
                miMedicion.Stop();

                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.Minutes);
            }
        }

        private static Polyline Planta(EjeTrazado trazado, string layer)
        {
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    Polyline miEje = new Polyline();
                    int index = 0;
                    foreach (var componente in trazado.getComponentes)
                    {
                        foreach (var componentPoint in componente.getComponentPoints())
                        {
                            miEje.AddVertexAt(index, new Point2d(componentPoint[0], componentPoint[1]), 0, 0, 0);
                            index++;
                        }
                    }
                    engCadNet.oLayer.addLayer(layer, 5, false);
                    miEje.Layer = layer;
                    btr.AppendEntity(miEje);
                    tr.AddNewlyCreatedDBObject(miEje, true);

                    // Commit al final, una vez dibujadas todas las entidades
                    oCadManager.thisEditor.UpdateScreen();
                    tr.Commit();
                    return miEje;
                }
            }
        }

        private static void Rotular(EjeTrazado trazado, string layer)
        {
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                    //GESTION CAPAS
                    oTadilLayerEjeTrazadoTadilPk miLayerPk = new oTadilLayerEjeTrazadoTadilPk(layer);
                    oTadilLayerEjeTrazadoTadilPs miLayerPs = new oTadilLayerEjeTrazadoTadilPs(layer);

                    miLayerPk.deleteItems();
                    miLayerPs.deleteItems();

                    oEjeTrazadoTadilRotular miEjeTrazadoCivilRotular = new oEjeTrazadoTadilRotular();

                    miEjeTrazadoCivilRotular.rotular(trazado, miLayerPk.name, miLayerPs.name);

                    // Commit al final, una vez dibujadas todas las entidades
                    oCadManager.thisEditor.UpdateScreen();
                    tr.Commit();
                }
            }
        }
    }
 
}
