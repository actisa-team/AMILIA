using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerfilLongitudinal;
using tadLayShare.puntos;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.ApplicationServices;
using engCadNet;
using EjeDeTrazado.puntosDelEje;

using EjeDeTrazado.componentes;
using tadLayLogica.logica.Entidades;
using tadLayLan.Tdi;

namespace tadLayLogica.EjeLongitudinalTadil
{



    public class PerfilLongitudinalDraw
    {
        Guitarra miGuitarra;
        Alzado miAlzado;
        EjeTrazado miEje;
        Func<double?, double?, double?> mGetCota;
        Func<double[], double> iMDT_Abanico_Punto;
        List<oEstructura> mEstructuras;
        Polyline miEjeAlzado = null;


        public PerfilLongitudinalDraw(Guitarra iGuitarra, Alzado iAlzado, EjeTrazado iEje, Func<double?, double?, double?> iGetCota,Func<double[], double> MDT_Abanico_Punto)
        {
            miGuitarra= iGuitarra;
            miAlzado = iAlzado;
            miEje= iEje;
            mGetCota = iGetCota;
            iMDT_Abanico_Punto = MDT_Abanico_Punto;
        }

        public void drawEje(string iCapa)
        {

            try
            {
                List<Entity> miLstEntidadesEje = getLstEntidadesEje(iCapa);

                using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
                {

                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                        //Necesito Crear un Nuevo Registro para Añadir la Linea
                        BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                        //Añado los Objetos Restantes a la Coleccion al Dwg
                        foreach (Entity item in miLstEntidadesEje)
                        {
                            acBlockTableRec.AppendEntity(item);
                            tr.AddNewlyCreatedDBObject(item, true);
                            item.SetDatabaseDefaults();
                            item.Layer = iCapa;
                        }


                        tr.Commit();
                    }
                }
            }
            catch(Exception e)
            {

            }
           


        }

        public void drawGuitarra(string iCapa)
        {
            try
            {
                List<Entity> miLstEntidadesGuitarra = getLstEntidadesGuitarra(iCapa);


                using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
                {

                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                        //Necesito Crear un Nuevo Registro para Añadir la Linea
                        BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                        //Añado los Objetos Restantes a la Coleccion al Dwg
                        foreach (Entity item in miLstEntidadesGuitarra)
                        {
                            acBlockTableRec.AppendEntity(item);
                            tr.AddNewlyCreatedDBObject(item, true);
                            item.SetDatabaseDefaults();
                            item.Layer = iCapa;
                        }


                        tr.Commit();
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error en drawGuitarra:\n" + e.Message + "\n\n" + e.StackTrace,
                    "Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
            
        }

        public void drawTerreno(string iCapa)
        {
            try
            {
                List<Entity> miLstEntidadesEje = getLstEntidadesTerreno(iCapa);

                using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
                {

                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                        //Necesito Crear un Nuevo Registro para Añadir la Linea
                        BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                        //Añado los Objetos Restantes a la Coleccion al Dwg
                        foreach (Entity item in miLstEntidadesEje)
                        {
                            acBlockTableRec.AppendEntity(item);
                            tr.AddNewlyCreatedDBObject(item, true);
                            item.SetDatabaseDefaults();
                            item.Layer = iCapa;
                        }


                        tr.Commit();
                    }
                }
            }
            catch (Exception e)
            {

            }

            
        }

        public void drawEjeLongitudinal(string iCapa)
        {
            try
            {
                List<Entity> miLstEntidadesEje = getLstEntidadesEjeYAcuerdos(iCapa);

                using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
                {

                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                        //Necesito Crear un Nuevo Registro para Añadir la Linea
                        BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                        //Añado los Objetos Restantes a la Coleccion al Dwg
                        foreach (Entity item in miLstEntidadesEje)
                        {
                            acBlockTableRec.AppendEntity(item);
                            tr.AddNewlyCreatedDBObject(item, true);
                            item.SetDatabaseDefaults();
                            item.Layer = iCapa;
                        }


                        tr.Commit();
                    }
                }
            }
            catch(Exception e)
            {

            }
           
        }

        public void drawPeralte(string iCapa)
        {


            List<Entity> miLstEntidadesPeralte = getLstEntidadesPeralte(iCapa);

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                    //Añado los Objetos Restantes a la Coleccion al Dwg
                    foreach (Entity item in miLstEntidadesPeralte)
                    {
                        acBlockTableRec.AppendEntity(item);
                        tr.AddNewlyCreatedDBObject(item, true);
                        item.SetDatabaseDefaults();
                        item.Layer = iCapa;
                    }


                    tr.Commit();
                }
            }
        }

        public void drawAcuerdos(string iCapa)
        {


            List<Entity> miLstEntidadesEje = getLstEntidadesAcuerdos(iCapa);

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                    //Añado los Objetos Restantes a la Coleccion al Dwg
                    foreach (Entity item in miLstEntidadesEje)
                    {
                        acBlockTableRec.AppendEntity(item);
                        tr.AddNewlyCreatedDBObject(item, true);
                        item.SetDatabaseDefaults();
                        item.Layer = iCapa;
                    }


                    tr.Commit();
                }
            }
        }
        public void drawEstructuras (string iCapa, List<oEstructura> iEstructuras)
        {


            List<Entity> miLstEntidadesEje = getLstEntidadesEstructuras(iCapa, iEstructuras);

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                    //Añado los Objetos Restantes a la Coleccion al Dwg
                    foreach (Entity item in miLstEntidadesEje)
                    {
                        acBlockTableRec.AppendEntity(item);
                        tr.AddNewlyCreatedDBObject(item, true);
                        item.SetDatabaseDefaults();
                        item.Layer = iCapa;
                    }


                    tr.Commit();
                }
            }
        }

        private List<Entity> getLstEntidadesEjeYAcuerdos(string iCapa)
        {
            List<Entity> miLst = new List<Entity>();
            int cuadrado = (int)miAlzado.getMinZ / miGuitarra.getEscalaAlto;
            double maxX = miAlzado.getMaxPk;

            miEjeAlzado = new Polyline();

            double miX = 0;

            while (miX <= maxX)
            {
                double miY = miAlzado.getCotaRasante(miX);
                Point2d miP1 = new Point2d(miGuitarra.getPuntoOrigX + (miX * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (miY * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);
                miEjeAlzado.AddVertexAt(0, miP1, 0, 0, 0);
                miX = miX + 10;
            }


            double miYMax = miAlzado.getCotaRasante(maxX);
            Point2d miP1Max = new Point2d(miGuitarra.getPuntoOrigX + (maxX * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (miYMax * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);
            miEjeAlzado.AddVertexAt(0, miP1Max, 0, 0, 0);


            miEjeAlzado.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);
            miLst.Add(miEjeAlzado);
            return miLst;

        }

        private List<Entity> getLstEntidadesPeralte(string iCapa)
        {
                List<Entity> miLista = dibujaCaja(7, iCapa);
                eIdioma miIdioma = oTadil.data.getIdioma();
                if (miIdioma.ToString() == "es")
                {
                    oTexto.addText2D(strFrmInformes.uiGuitarraPeralteMay, miGuitarra.getPuntoOrigX - 113, miGuitarra.getPuntoOrigY - 10.5 - 25 - (10.5 + 50) * 7, 7.5, 0, 0, iCapa);
                }
                else if (miIdioma.ToString() == "en")
                {
                    oTexto.addText2D(strFrmInformes.uiGuitarraPeralteMay, miGuitarra.getPuntoOrigX - 94.36, miGuitarra.getPuntoOrigY - 10.5 - 25 - (10.5 + 50) * 7, 7.5, 0, 0, iCapa);
                }
                else if (miIdioma.ToString() == "fr")
                {
                    oTexto.addText2D(strFrmInformes.uiGuitarraPeralteMay, miGuitarra.getPuntoOrigX - 132.19, miGuitarra.getPuntoOrigY - 10.5 - 25 - (10.5 + 50) * 7, 7.5, 0, 0, iCapa);
                }
                return miLista;
        }


        private List<Entity> getLstEntidadesEstructuras(string iCapa, List<oEstructura> iEstructuras)
        {
            mEstructuras = iEstructuras;
            List<Entity> miLista = dibujaCaja(6, iCapa);

            eIdioma miIdioma = oTadil.data.getIdioma();
            if (miIdioma.ToString() == "es")
            {
                oTexto.addText2D(strFrmInformes.uiGuitarraEstructurasMay, miGuitarra.getPuntoOrigX - 147, miGuitarra.getPuntoOrigY - 10.5 - 25 - (10.5 + 50) * 6, 7.5, 0, 0, iCapa);
            }
            else if (miIdioma.ToString() == "en")
            {
                oTexto.addText2D(strFrmInformes.uiGuitarraEstructurasMay, miGuitarra.getPuntoOrigX - 118.9, miGuitarra.getPuntoOrigY - 10.5 - 25 - (10.5 + 50) * 6, 7.5, 0, 0, iCapa);
            }
            else if (miIdioma.ToString() == "fr")
            {
                oTexto.addText2D(strFrmInformes.uiGuitarraEstructurasMay, miGuitarra.getPuntoOrigX - 108.86, miGuitarra.getPuntoOrigY - 10.5 - 25 - (10.5 + 50) * 6, 7.5, 0, 0, iCapa);
            }
            return miLista;
        }

        private List<Entity> getLstEntidadesGuitarra(string iCapa)
        {
            int cuadrado = (int)miGuitarra.getMinY / miGuitarra.getEscalaAlto;
            cuadrado = cuadrado - 3;
            List<Entity> miLst = new List<Entity>();

            Polyline miLw1 = new Polyline();
            Point2d miP1 = new Point2d(miGuitarra.getPuntoOrigX, miGuitarra.getPuntoOrigY);
            miLw1.AddVertexAt(0, miP1, 0, 0, 0);

            Point2d miP2 = new Point2d(miGuitarra.getMaxX, miGuitarra.getPuntoOrigY);
            miLw1.AddVertexAt(0, miP2, 0, 0, 0);
            miLw1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);
            miLst.Add(miLw1);


            Polyline miLw2 = new Polyline();
            Point2d miP3 = new Point2d(miGuitarra.getPuntoOrigX, miGuitarra.getMaxY);
            miLw2.AddVertexAt(0, miP3, 0, 0, 0);

            Point2d miP4 = new Point2d(miGuitarra.getMaxX, miGuitarra.getMaxY);
            miLw2.AddVertexAt(0, miP4, 0, 0, 0);
            miLw2.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);
            miLst.Add(miLw2);

            double i = miGuitarra.getPuntoOrigY + 100;
            double i1 = miGuitarra.getPuntoOrigY + 25;
            double j = miGuitarra.getPuntoOrigX + 100;
            double j1 = miGuitarra.getPuntoOrigX + 25;


            double texto = cuadrado * miGuitarra.getEscalaAlto;
            texto = Math.Round(texto, 1);
            string textoS = texto.ToString("0.00");
            oTexto.addText2D(textoS, miGuitarra.getPuntoOrigX - 20, miGuitarra.getPuntoOrigY, 4, 0, 3, iCapa);
            oTexto.addText2D(textoS, miGuitarra.getMaxX + 5, miGuitarra.getPuntoOrigY, 4, 0, 3, iCapa);
            cuadrado++;

            while (i < miGuitarra.getMaxY)
            {
                Polyline miLtrasH = new Polyline();
                Point2d miPtransH1 = new Point2d(miGuitarra.getPuntoOrigX, i);
                miLtrasH.AddVertexAt(0, miPtransH1, 0, 0, 0);
                Point2d miPtransH2 = new Point2d(miGuitarra.getMaxX, i);
                miLtrasH.AddVertexAt(0, miPtransH2, 0, 0, 0);
                miLtrasH.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
                miLst.Add(miLtrasH);

                texto = cuadrado * miGuitarra.getEscalaAlto;
                texto = Math.Round(texto, 1);
                textoS = texto.ToString("0.00");
                oTexto.addText2D(textoS, miGuitarra.getPuntoOrigX - 20, i, 4, 0, 3, iCapa);
                oTexto.addText2D(textoS, miGuitarra.getMaxX + 5, i, 4, 0, 3, iCapa);

                cuadrado++;
                i = i + 100;

            }


            texto = cuadrado * miGuitarra.getEscalaAlto;
            texto = Math.Round(texto, 1);
            textoS = texto.ToString("0.00");
            oTexto.addText2D(textoS, miGuitarra.getPuntoOrigX - 20, i, 4, 0, 3, iCapa);
            oTexto.addText2D(textoS, miGuitarra.getMaxX + 5, i, 4, 0, 3, iCapa);


            while (j1 < miGuitarra.getMaxX)
            {

                Polyline miMarca1 = new Polyline();
                Point2d miPmarcaH1 = new Point2d(j1, miGuitarra.getPuntoOrigY + 1.25);
                miMarca1.AddVertexAt(0, miPmarcaH1, 0, 0, 0);
                Point2d miPmarcaH2 = new Point2d(j1, miGuitarra.getPuntoOrigY - 1.25);
                miMarca1.AddVertexAt(0, miPmarcaH2, 0, 0, 0);
                miMarca1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);
                miLst.Add(miMarca1);

                Polyline miMarca2 = new Polyline();
                Point2d miPmarcaH3 = new Point2d(j1, miGuitarra.getMaxY + 1.25);
                miMarca2.AddVertexAt(0, miPmarcaH3, 0, 0, 0);
                Point2d miPmarcaH4 = new Point2d(j1, miGuitarra.getMaxY - 1.25);
                miMarca2.AddVertexAt(0, miPmarcaH4, 0, 0, 0);
                miMarca2.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);
                miLst.Add(miMarca2);

                j1 = j1 + 25;
            }

            Polyline miMarca1final = new Polyline();
            Point2d miPmarcaH1final = new Point2d(miGuitarra.getMaxXEje, miGuitarra.getPuntoOrigY + 1.25);
            miMarca1final.AddVertexAt(0, miPmarcaH1final, 0, 0, 0);
            Point2d miPmarcaH2final = new Point2d(miGuitarra.getMaxXEje, miGuitarra.getPuntoOrigY - 1.25);
            miMarca1final.AddVertexAt(0, miPmarcaH2final, 0, 0, 0);
            miMarca1final.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);
            miLst.Add(miMarca1final);

            Polyline miMarca2final = new Polyline();
            Point2d miPmarcaH3final = new Point2d(miGuitarra.getMaxXEje, miGuitarra.getMaxY + 1.25);
            miMarca2final.AddVertexAt(0, miPmarcaH3final, 0, 0, 0);
            Point2d miPmarcaH4final = new Point2d(miGuitarra.getMaxXEje, miGuitarra.getMaxY - 1.25);
            miMarca2final.AddVertexAt(0, miPmarcaH4final, 0, 0, 0);
            miMarca2final.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);
            miLst.Add(miMarca2final);



            Polyline miLw3 = new Polyline();
            Point2d miP5 = new Point2d(miGuitarra.getPuntoOrigX, miGuitarra.getPuntoOrigY);
            miLw3.AddVertexAt(0, miP5, 0, 0, 0);

            Point2d miP6 = new Point2d(miGuitarra.getPuntoOrigX, miGuitarra.getMaxY);
            miLw3.AddVertexAt(0, miP6, 0, 0, 0);
            miLw3.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
            miLst.Add(miLw3);


            Polyline miLw4 = new Polyline();
            Point2d miP7 = new Point2d(miGuitarra.getMaxX, miGuitarra.getPuntoOrigY);
            miLw4.AddVertexAt(0, miP7, 0, 0, 0);

            Point2d miP8 = new Point2d(miGuitarra.getMaxX, miGuitarra.getMaxY);
            miLw4.AddVertexAt(0, miP8, 0, 0, 0);
            miLw4.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
            miLst.Add(miLw4);

            double miPK = 0;
            string miPKS = getStringPK(miPK);
            oTexto.addText2D(miPKS, miGuitarra.getPuntoOrigX - 5, miGuitarra.getPuntoOrigY - 5, 4, 0, 2, iCapa);
            oTexto.addText2D(miPKS, miGuitarra.getPuntoOrigX - 5, miGuitarra.getMaxY + 5, 4, 0, 2, iCapa);
            miPK = miPK + miGuitarra.getEscalaAncho;


            while (j < miGuitarra.getMaxX)
            {
                Polyline miLtrasH = new Polyline();
                Point2d miPtransH1 = new Point2d(j, miGuitarra.getPuntoOrigY);
                miLtrasH.AddVertexAt(0, miPtransH1, 0, 0, 0);
                Point2d miPtransH2 = new Point2d(j, miGuitarra.getMaxY);
                miLtrasH.AddVertexAt(0, miPtransH2, 0, 0, 0);
                miLtrasH.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
                miLst.Add(miLtrasH);


                miPKS = getStringPK(miPK);
                oTexto.addText2D(miPKS, j - 5, miGuitarra.getPuntoOrigY - 5, 4, 0, 2, iCapa);
                oTexto.addText2D(miPKS, j - 5, miGuitarra.getMaxY + 5, 4, 0, 2, iCapa);

                j = j + 100;
                miPK = miPK + miGuitarra.getEscalaAncho;



            }


            miPKS = getStringPK(miPK);
            oTexto.addText2D(miPKS, j - 5, miGuitarra.getPuntoOrigY - 5, 4, 0, 2, iCapa);
            oTexto.addText2D(miPKS, j - 5, miGuitarra.getMaxY + 5, 4, 0, 2, iCapa);

            miPK = miGuitarra.getMaxPKX;
            miPKS = getStringPK(miPK);
            oTexto.addText2D(miPKS, miGuitarra.getMaxXEje - 5, miGuitarra.getPuntoOrigY - 5, 4, 0, 2, iCapa);
            oTexto.addText2D(miPKS, miGuitarra.getMaxXEje - 5, miGuitarra.getMaxY + 5, 4, 0, 2, iCapa);

            while (i1 < miGuitarra.getMaxY)
            {

                Polyline miMarca1 = new Polyline();
                Point2d miPmarcaH1 = new Point2d(miGuitarra.getPuntoOrigX - 1.25, i1);
                miMarca1.AddVertexAt(0, miPmarcaH1, 0, 0, 0);
                Point2d miPmarcaH2 = new Point2d(miGuitarra.getPuntoOrigX + 1.25, i1);
                miMarca1.AddVertexAt(0, miPmarcaH2, 0, 0, 0);
                miMarca1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
                miLst.Add(miMarca1);

                Polyline miMarca2 = new Polyline();
                Point2d miPmarcaH3 = new Point2d(miGuitarra.getMaxX - 1.25, i1);
                miMarca2.AddVertexAt(0, miPmarcaH3, 0, 0, 0);
                Point2d miPmarcaH4 = new Point2d(miGuitarra.getMaxX + 1.25, i1);
                miMarca2.AddVertexAt(0, miPmarcaH4, 0, 0, 0);
                miMarca2.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
                miLst.Add(miMarca2);

                i1 = i1 + 25;
            }
            string[] misTextos= new string[8];
            misTextos[0] = strFrmInformes.uiGuitarraAcuerdosVerticales;
            misTextos[1] = strFrmInformes.uiGuitarraCotaTerreno;
            misTextos[2] = strFrmInformes.uiGuitarraCotaRasante;
            misTextos[3] = strFrmInformes.uiGuitarraRojaDesmonte;
            misTextos[4] = strFrmInformes.uiGuitarraRojaTerraplen;
            misTextos[5] = strFrmInformes.uiGuitarraDiagCurvaturas;
            misTextos[7] = strFrmInformes.uiGuitarraPeralteMay;
            misTextos[6] = strFrmInformes.uiGuitarraEstructurasMay;
            double[] dist = new double[8];
            eIdioma miIdioma = oTadil.data.getIdioma();
            if (miIdioma.ToString() == "es")
            {
                dist[0] = 191.95;
                dist[1] = 148.87;
                dist[2] = 146.53;
                dist[3] = 173.63;
                dist[4] = 168.63;
                dist[5] = 213.25;
                dist[6] = 147;
                dist[7] = 113;
            }
            else if (miIdioma.ToString() == "en")
            {
                dist[0] = 160.63;
                dist[1] = 111.42;
                dist[2] = 146.53;
                dist[3] = 157.84;
                dist[4] = 168.63;
                dist[5] = 164.12;
                dist[6] = 118.9;
                dist[7] = 97.36;
            }
            else if (miIdioma.ToString() == "fr")
            {
                dist[0] = 160.63;
                dist[1] = 115.26;
                dist[2] = 136.25;
                dist[3] = 138.1;
                dist[4] = 144.53;
                dist[5] = 188.82;
                dist[6] = 108.86;
                dist[7] = 132.19;
            }
            for (int c = 0; c < 6; c++)
            {
                List<Entity> miLista = dibujaCaja(c, iCapa);
                oTexto.addText2D(misTextos[c], miGuitarra.getPuntoOrigX - dist[c], miGuitarra.getPuntoOrigY - 10.5 - 25 - (10.5 + 50) * c, 7.5, 0, 0, iCapa);

                foreach (Entity miEntidad in miLista)
                {
                    miLst.Add(miEntidad);
                }
            }


            return miLst;

        }

        private void dibujaCajaCotaTerreno(int pos, string iCapa)
        {

            double dist = miGuitarra.getEscalaAncho / 4;
            double miPk = 0;
            double miX = miGuitarra.getPuntoOrigX;
            double miY = miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * pos) + 10;


            while (miPk < miAlzado.getMaxPk)
            {
                double[] miPunto = miEje.getPointAtDist(miPk);
                iMDT_Abanico_Punto(miPunto);
                double? miCotaNullable = mGetCota(miPunto[0], miPunto[1]);
                if (miCotaNullable.HasValue)
                {
                    string miCotaS = miCotaNullable.Value.ToString("0.00");
                    oTexto.addText2D(miCotaS, miX, miY, 7.5, Math.PI / 2, 0, iCapa);
                }

                miPk = miPk + dist;
                miX = miX + 25;     
            }
            double midistPkFinal = (miAlzado.getMaxPk*25)/dist;
            miX = miGuitarra.getPuntoOrigX + midistPkFinal;
            double[] miPuntoF = miEje.getPointAtDist(miAlzado.getMaxPk);
            iMDT_Abanico_Punto(miPuntoF);
            double? miCotaFNullable = mGetCota(miPuntoF[0], miPuntoF[1]);
            if (miCotaFNullable.HasValue)
            {
                string miCotaFS = miCotaFNullable.Value.ToString("0.00");
                oTexto.addText2D(miCotaFS, miX, miY, 7.5, Math.PI / 2, 0, iCapa);
            }
        }

        private void dibujaCajaCotaRasante(int pos, string iCapa)
        {

            double dist = miGuitarra.getEscalaAncho / 4;
            double miPk = 0;
            double miX = miGuitarra.getPuntoOrigX;
            double miY = miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * pos) + 10;


            while (miPk < miAlzado.getMaxPk)
            {
                double miCota = miAlzado.getCotaRasante(miPk);
                string miCotaS = miCota.ToString("0.00");
                oTexto.addText2D(miCotaS, miX, miY, 7.5, Math.PI / 2, 0, iCapa);

                miPk = miPk + dist;
                miX = miX + 25;
            }


            double midistPkFinal = (miAlzado.getMaxPk * 25) / dist;
            miX = miGuitarra.getPuntoOrigX + midistPkFinal;
            double miCotaF = miAlzado.getCotaRasante(miAlzado.getMaxPk);
            string miCotaFS = miCotaF.ToString("0.00");
            oTexto.addText2D(miCotaFS, miX, miY, 7.5, Math.PI / 2, 0, iCapa);
        }


        private void dibujaCajaCotaDesmonte(int pos, string iCapa)
        {

            double dist = miGuitarra.getEscalaAncho / 4;
            double miPk = 0;
            double miX = miGuitarra.getPuntoOrigX;
            double miY = miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * pos) + 12;


            while (miPk < miAlzado.getMaxPk)
            {
                double[] miPunto = miEje.getPointAtDist(miPk);
                iMDT_Abanico_Punto(miPunto);
                double? miCotaTNullable = mGetCota(miPunto[0], miPunto[1]);
                if (miCotaTNullable.HasValue)
                {
                    double miCotaR = miAlzado.getCotaRasante(miPk);
                    double miCota = Math.Round(miCotaTNullable.Value - miCotaR, 3);
                    if (miCota > 0)
                    {
                        string miCotaS = miCota.ToString("0.00");
                        oTexto.addText2D(miCotaS, miX, miY, 7.5, Math.PI / 2, 1, iCapa);
                    }
                }
                miPk = miPk + dist;
                miX = miX + 25;
            }


            double midistPkFinal = (miAlzado.getMaxPk * 25) / dist;
            miX = miGuitarra.getPuntoOrigX + midistPkFinal;
            double[] miPuntoF = miEje.getPointAtDist(miAlzado.getMaxPk);
            iMDT_Abanico_Punto(miPuntoF);
            double? miCotaFTNullable = mGetCota(miPuntoF[0], miPuntoF[1]);
            if (miCotaFTNullable.HasValue)
            {
                double miCotaFR = miAlzado.getCotaRasante(miPk);
                double miCotaF = Math.Round(miCotaFTNullable.Value - miCotaFR, 3);
                if (miCotaF > 0)
                {
                    string miCotaS = miCotaF.ToString("0.00");
                    oTexto.addText2D(miCotaS, miX, miY, 7.5, Math.PI / 2, 1, iCapa);
                }
            }
        }

        private void dibujaCajaCotaTerraplen(int pos, string iCapa)
        {

            double dist = miGuitarra.getEscalaAncho / 4;
            double miPk = 0;
            double miX = miGuitarra.getPuntoOrigX;
            double miY = miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * pos) + 12;


            while (miPk < miAlzado.getMaxPk)
            {
                double[] miPunto = miEje.getPointAtDist(miPk);
                iMDT_Abanico_Punto(miPunto);
                double? miCotaTNullable = mGetCota(miPunto[0], miPunto[1]);
                if (miCotaTNullable.HasValue)
                {
                    double miCotaR = miAlzado.getCotaRasante(miPk);
                    double miCota = Math.Round(miCotaR - miCotaTNullable.Value, 3);
                    if (miCota > 0)
                    {
                        string miCotaS = miCota.ToString("0.00");
                        oTexto.addText2D(miCotaS, miX, miY, 7.5, Math.PI / 2, 3, iCapa);
                    }
                }
                miPk = miPk + dist;
                miX = miX + 25;
            }

            double midistPkFinal = (miAlzado.getMaxPk * 25) / dist;
            miX = miGuitarra.getPuntoOrigX + midistPkFinal;
            double[] miPuntoF = miEje.getPointAtDist(miAlzado.getMaxPk);
            iMDT_Abanico_Punto(miPuntoF);
            double? miCotaFTNullable = mGetCota(miPuntoF[0], miPuntoF[1]);
            if (miCotaFTNullable.HasValue)
            {
                double miCotaFR = miAlzado.getCotaRasante(miPk);
                double miCotaF = Math.Round(miCotaFR - miCotaFTNullable.Value, 3);
                if (miCotaF > 0)
                {
                    string miCotaS = miCotaF.ToString("0.00");
                    oTexto.addText2D(miCotaS, miX, miY, 7.5, Math.PI / 2, 3, iCapa);
                }
            }
        }

        private List<Entity> dibujaCajaCurvaturas(int pos, string iCapa)
        {
            List<Entity> miLst = new List<Entity>();
            double miMaxRadio = miEje.getRadioMax;


            double miYMin = miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * pos);
            double miYCero = miGuitarra.getPuntoOrigY - 10.5 - 25 - ((10.5 + 50) * pos);
            double miYMax = miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * pos);

            foreach (Componente miComp in miEje.getComponentes)
            {

                Polyline radio = new Polyline();
                if (miComp.getTipoComponente() == Componente.tipoComponente.linea)
                {
                    Point2d radioi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni *100 / miGuitarra.getEscalaAncho), miYCero);
                    Point2d radiod = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYCero);
                    radio.AddVertexAt(0, radioi, 0, 0, 0);
                    radio.AddVertexAt(0, radiod, 0, 0, 0);
                    radio.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
                }
                else if (miComp.getTipoComponente() == Componente.tipoComponente.curva)
                {
                    Curva miCurva = (Curva)miComp;
                    double radioCurva = miCurva.getRadio;
                    double tanto1 = radioCurva / miMaxRadio;
                    double miYCurva = 0;
                    if (miCurva.getSentCurva == EjeTrazado.sentidoCurva.Horario)
                    {
                        miYCurva = ((miYMax - miYCero) * tanto1) + miYCero;
                    }
                    else
                    {
                        miYCurva = ((miYCero - miYMin) * (1-tanto1)) + miYMin;
                    }
                    string miRadioS = radioCurva.ToString("0.00");

                    double posX = miGuitarra.getPuntoOrigX + ((miComp.getPkIni +((miComp.getPkFinal() - miComp.getPkIni)) / 2) * 100 / miGuitarra.getEscalaAncho)-7;

                    oTexto.addText2D(miRadioS, posX, miYCurva + 3, 3, 0, 0, iCapa);

                    Point2d radioi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYCurva);
                    Point2d radiod = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYCurva);
                    radio.AddVertexAt(0, radioi, 0, 0, 0);
                    radio.AddVertexAt(0, radiod, 0, 0, 0);
                    radio.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);

                }
                else if (miComp.getTipoComponente() == Componente.tipoComponente.clotoideEntrada)
                {

                    Clotoide miClotoide = (Clotoide)miComp;
                    double radioClotoide = miClotoide.getRadio;
                    double tanto1 = radioClotoide / miMaxRadio;
                    double miYClotoide =0;
                    if (miClotoide.mSentCurva == EjeTrazado.sentidoCurva.Horario)
                    {
                        miYClotoide = ((miYMax - miYCero) * tanto1) + miYCero;
                    }
                    else
                    {
                        miYClotoide = ((miYCero - miYMin) * (1 - tanto1)) + miYMin;
                    }

                    Point2d radioi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYCero);
                    Point2d radiod = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYClotoide);
                    radio.AddVertexAt(0, radioi, 0, 0, 0);
                    radio.AddVertexAt(0, radiod, 0, 0, 0);
                    radio.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
                }
                else if (miComp.getTipoComponente() == Componente.tipoComponente.clotoideSalida)
                {
                    Clotoide miClotoide = (Clotoide)miComp;
                    double radioClotoide = miClotoide.getRadio;
                    double tanto1 = radioClotoide / miMaxRadio;
                    double miYClotoide = 0;
                    if (miClotoide.mSentCurva == EjeTrazado.sentidoCurva.Horario)
                    {
                        miYClotoide = ((miYMax - miYCero) * tanto1) + miYCero;
                    }
                    else
                    {
                        miYClotoide = ((miYCero - miYMin) * (1 - tanto1)) + miYMin;
                    }

                    Point2d radioi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYClotoide);
                    Point2d radiod = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYCero);
                    radio.AddVertexAt(0, radioi, 0, 0, 0);
                    radio.AddVertexAt(0, radiod, 0, 0, 0);
                    radio.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
                }

                miLst.Add(radio);
            }

            return miLst;
        }

        private List<Entity> dibujaCajaPeralte(int pos, string iCapa)
        {
            List<Entity> miLst = new List<Entity>();
            double miMaxPeralte = miEje.getPeralteCurva;
            if (miEje.getBombeo > miMaxPeralte) miMaxPeralte = miEje.getBombeo;

            foreach (Componente miComp in miEje.getComponentes)
            {
                double pI1 = 0, pD1 = 0, pI2 = 0, pD2 = 0;
                miEje.getPeralteAlDist(miComp.getPkIni + 0.01, ref pI1, ref pD1);
                miEje.getPeralteAlDist(miComp.getPkFinal() - 0.01, ref pI2, ref pD2);
                miMaxPeralte = Math.Max(miMaxPeralte, Math.Abs(pI1));
                miMaxPeralte = Math.Max(miMaxPeralte, Math.Abs(pD1));
                miMaxPeralte = Math.Max(miMaxPeralte, Math.Abs(pI2));
                miMaxPeralte = Math.Max(miMaxPeralte, Math.Abs(pD2));
            }

            if (miMaxPeralte <= 0) miMaxPeralte = 8; // Valor de seguridad

            double miYCero = miGuitarra.getPuntoOrigY - 10.5 - 25 - ((10.5 + 50) * pos);
            double miYMax = miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * pos);


            foreach (Componente miComp in miEje.getComponentes)
            {

                Polyline margenizq = new Polyline();
                Polyline margender = new Polyline();
                if (miComp.getTipoComponente() == Componente.tipoComponente.linea)
                {
                    double peralteIzq=0, peralteDer=0;
                    miEje.getPeralteAlDist(miComp.getPkIni+0.01, ref peralteIzq, ref peralteDer);


                    double tanto1Izq = peralteIzq / miMaxPeralte;
                    double miYRectaIzq = ((miYMax - miYCero) * tanto1Izq) + miYCero;

                    double tanto1Der = peralteDer / miMaxPeralte;
                    double miYRectaDer = ((miYMax - miYCero) * tanto1Der) + miYCero;


                    string peralteIzqS = peralteIzq.ToString("0.00");
                    string peralteDerS = peralteDer.ToString("0.00");
                    double posX = miGuitarra.getPuntoOrigX + ((miComp.getPkIni + ((miComp.getPkFinal() - miComp.getPkIni)) / 2) * 100 / miGuitarra.getEscalaAncho) - 7;

                    oTexto.addText2D(peralteIzqS, posX, miYRectaIzq + 3, 3, 0, 0, iCapa);
                    oTexto.addText2D(peralteDerS, posX, miYRectaDer + 3, 3, 0, 0, iCapa);


                    Point2d margenizqi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYRectaIzq);
                    Point2d margenizqd = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYRectaIzq);
                    margenizq.AddVertexAt(0, margenizqi, 0, 0, 0);
                    margenizq.AddVertexAt(1, margenizqd, 0, 0, 0);
                    margenizq.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);


                    Point2d margenderi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYRectaDer);
                    Point2d margenderd = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYRectaDer);
                    margender.AddVertexAt(0, margenderi, 0, 0, 0);
                    margender.AddVertexAt(1, margenderd, 0, 0, 0);
                    margender.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);

                }
                else if (miComp.getTipoComponente() == Componente.tipoComponente.curva)
                {
                    double peralteIzq=0, peralteDer=0;
                    miEje.getPeralteAlDist(miComp.getPkIni + 0.01, ref peralteIzq, ref peralteDer);


                    double tanto1Izq = peralteIzq / miMaxPeralte;
                    double miYCurvaIzq = ((miYMax - miYCero) * tanto1Izq) + miYCero;

                    double tanto1Der = peralteDer / miMaxPeralte;
                    double miYCurvaDer = ((miYMax - miYCero) * tanto1Der) + miYCero;


                    string peralteIzqS = peralteIzq.ToString("0.00");
                    string peralteDerS = peralteDer.ToString("0.00");
                    double posX = miGuitarra.getPuntoOrigX + ((miComp.getPkIni + ((miComp.getPkFinal() - miComp.getPkIni)) / 2) * 100 / miGuitarra.getEscalaAncho) - 7;

                    oTexto.addText2D(peralteIzqS, posX, miYCurvaIzq + 3, 3, 0, 0, iCapa);
                    oTexto.addText2D(peralteDerS, posX, miYCurvaDer + 3, 3, 0, 0, iCapa);

                    Point2d margenizqi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYCurvaIzq);
                    Point2d margenizqd = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYCurvaIzq);
                    margenizq.AddVertexAt(0, margenizqi, 0, 0, 0);
                    margenizq.AddVertexAt(1, margenizqd, 0, 0, 0);
                    margenizq.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);


                    Point2d margenderi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYCurvaDer);
                    Point2d margenderd = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYCurvaDer);
                    margender.AddVertexAt(0, margenderi, 0, 0, 0);
                    margender.AddVertexAt(1, margenderd, 0, 0, 0);
                    margender.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);

                }
                else 
                {
                    double peralteIzqAnt=0, peralteDerAnt=0, peralteIzqPos=0, peralteDerPos=0;
                    miEje.getPeralteAlDist(miComp.getPkIni - 0.01, ref peralteIzqAnt, ref peralteDerAnt);
                    miEje.getPeralteAlDist(miComp.getPkFinal() + 0.01, ref peralteIzqPos, ref peralteDerPos);


                    double tanto1IzqAnt = peralteIzqAnt / miMaxPeralte;
                    double miYCloIzqAnt = ((miYMax - miYCero) * tanto1IzqAnt) + miYCero;

                    double tanto1IzqPos = peralteIzqPos / miMaxPeralte;
                    double miYCloIzqPos = ((miYMax - miYCero) * tanto1IzqPos) + miYCero;

                    double tanto1DerAnt = peralteDerAnt/ miMaxPeralte;
                    double miYCloDerAnt = ((miYMax - miYCero) * tanto1DerAnt) + miYCero;

                    double tanto1DerPos = peralteDerPos/ miMaxPeralte;
                    double miYCloDerPos = ((miYMax - miYCero) * tanto1DerPos) + miYCero;

                    Point2d margenizqi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYCloIzqAnt);
                    Point2d margenizqd = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYCloIzqPos);
                    margenizq.AddVertexAt(0, margenizqi, 0, 0, 0);
                    margenizq.AddVertexAt(1, margenizqd, 0, 0, 0);
                    margenizq.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);


                    Point2d margenderi = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkIni * 100 / miGuitarra.getEscalaAncho), miYCloDerAnt);
                    Point2d margenderd = new Point2d(miGuitarra.getPuntoOrigX + (miComp.getPkFinal() * 100 / miGuitarra.getEscalaAncho), miYCloDerPos);
                    margender.AddVertexAt(0, margenderi, 0, 0, 0);
                    margender.AddVertexAt(1, margenderd, 0, 0, 0);
                    margender.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
                }
                miLst.Add(margenizq);
                miLst.Add(margender);

            }

            return miLst;
        }


        private void dibujaCajaAcuerdos(int pos, string iCapa)
        {
            double miY = miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * pos) + 8;
            int i = 0;
            List<List<double[]>> misAcuerdos = miAlzado.getAcuerdos();
            foreach (List<double[]> miAcuerdo in misAcuerdos)
            {
                bool entrada = true;
                foreach (double[] punto in miAcuerdo)
                {
                    double miX = miGuitarra.getPuntoOrigX + (punto[0] * 100 / miGuitarra.getEscalaAncho);
                    double miKv = miAlzado.getKv(i);
                    string miPKS = getStringPK(punto[0]);
                    string miKvS = miKv.ToString("0.00");
                    if (entrada)
                    {
                        oTexto.addText2D(miPKS, miX - 4, miY - 3, 7.5, Math.PI / 2, 0, iCapa);
                        oTexto.addText2D(miKvS, miX + 4, miY, 7.5, Math.PI / 2, 0, iCapa);
                    }
                    else
                    {
                        oTexto.addText2D(miKvS, miX - 4, miY, 7.5, Math.PI / 2, 0, iCapa);
                        oTexto.addText2D(miPKS, miX + 4, miY - 3, 7.5, Math.PI / 2, 0, iCapa);
                    }
                    entrada = false;
                }
                i++;
            }

        }

        private List<Entity> dibujaCajaEstructura(int pos, string iCapa)
        {
            int cuadrado = (int)miAlzado.getMinZ / miGuitarra.getEscalaAlto;
            List<Entity> miLst = new List<Entity>();
            double miYCero = miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * pos);
            double miYMax = miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * pos);
            double miYMitad = ((miYMax - miYCero) * 0.5) + miYCero;

            double miYMitadArriba = miYMitad + 4;
            double miYMitadAbajo = miYMitad - 4;

            int i=0;
            double estrucIni = -1;
            double estrucFin = -1;
            string nombreAnt = "";

            while (i < mEstructuras.Count)
            {
                if (mEstructuras.ElementAt(i).getTipoEstructura == eRoadSeccion.calzada)
                {
                    estrucIni = -1;
                    nombreAnt = "";
                    i++;
                }
                else if(mEstructuras.ElementAt(i).getTipoEstructura == eRoadSeccion.puente)
                {
                    estrucIni = mEstructuras.ElementAt(i).getPk - miAlzado.getIntervaloSecciones / 2;
                    if (estrucIni < 0) estrucIni = 0;
                    nombreAnt = mEstructuras.ElementAt(i).getNombre;
                    while ((i < mEstructuras.Count) && (mEstructuras.ElementAt(i).getTipoEstructura == eRoadSeccion.puente) && (mEstructuras.ElementAt(i).getNombre == nombreAnt))
                    {
                        i++;
                    }
                    estrucFin = mEstructuras.ElementAt(i - 1).getPk + miAlzado.getIntervaloSecciones / 2;
                    double rasanteYIni = miAlzado.getCotaRasante(estrucIni);
                    double rasanteYFin = miAlzado.getCotaRasante(estrucFin);
                    short color = 1;
                    int isPuente = 1;

                    Polyline abajoEst = new Polyline();
                    Polyline derEst = new Polyline();
                    Polyline izqEst = new Polyline();

                    double miY;

                    if (rasanteYIni > rasanteYFin)
                    {

                        miY = miGuitarra.getPuntoOrigY + ((rasanteYFin + (miGuitarra.getEscalaAlto * isPuente / 2)) * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300;
                    }
                    else
                    {
                        miY = miGuitarra.getPuntoOrigY + ((rasanteYIni + (miGuitarra.getEscalaAlto * isPuente / 2)) * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300;
                    }


                    Point2d p1arrEst = new Point2d(miGuitarra.getPuntoOrigX + (estrucIni * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (rasanteYIni * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);
                    Point2d p2arrEst = new Point2d(miGuitarra.getPuntoOrigX + (estrucFin * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (rasanteYFin * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);

                    Point2d p1abEst = new Point2d(miGuitarra.getPuntoOrigX + (estrucIni * 100 / miGuitarra.getEscalaAncho), miY);
                    Point2d p2abEst = new Point2d(miGuitarra.getPuntoOrigX + (estrucFin * 100 / miGuitarra.getEscalaAncho), miY);

                    abajoEst.AddVertexAt(0, p1abEst, 0, 0, 0);
                    abajoEst.AddVertexAt(0, p2abEst, 0, 0, 0);
                    abajoEst.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    derEst.AddVertexAt(0, p2abEst, 0, 0, 0);
                    derEst.AddVertexAt(0, p2arrEst, 0, 0, 0);
                    derEst.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    izqEst.AddVertexAt(0, p1arrEst, 0, 0, 0);
                    izqEst.AddVertexAt(0, p1abEst, 0, 0, 0);
                    izqEst.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);


                    miLst.Add(abajoEst);
                    miLst.Add(derEst);
                    miLst.Add(izqEst);


                    Polyline arriba = new Polyline();
                    Polyline abajo = new Polyline();
                    Polyline der = new Polyline();
                    Polyline izq = new Polyline();

                    Point2d p1arr = new Point2d(miGuitarra.getPuntoOrigX + (estrucIni * 100 / miGuitarra.getEscalaAncho), miYMitadArriba);
                    Point2d p2arr = new Point2d(miGuitarra.getPuntoOrigX + (estrucFin * 100 / miGuitarra.getEscalaAncho), miYMitadArriba);

                    Point2d p1ab = new Point2d(miGuitarra.getPuntoOrigX + (estrucIni * 100 / miGuitarra.getEscalaAncho), miYMitadAbajo);
                    Point2d p2ab = new Point2d(miGuitarra.getPuntoOrigX + (estrucFin * 100 / miGuitarra.getEscalaAncho), miYMitadAbajo);


                    arriba.AddVertexAt(0, p1arr, 0, 0, 0);
                    arriba.AddVertexAt(0, p2arr, 0, 0, 0);
                    arriba.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    abajo.AddVertexAt(0, p1ab, 0, 0, 0);
                    abajo.AddVertexAt(0, p2ab, 0, 0, 0);
                    abajo.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    der.AddVertexAt(0, p2arr, 0, 0, 0);
                    der.AddVertexAt(0, p2ab, 0, 0, 0);
                    der.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    izq.AddVertexAt(0, p1arr, 0, 0, 0);
                    izq.AddVertexAt(0, p1ab, 0, 0, 0);
                    izq.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    miLst.Add(arriba);
                    miLst.Add(abajo);
                    miLst.Add(der);
                    miLst.Add(izq);


                    oTexto.addText2D(nombreAnt, miGuitarra.getPuntoOrigX + ((estrucIni + ((estrucFin - estrucIni) / 2)) * 100 / miGuitarra.getEscalaAncho), miYMitad, 3, 0, 0, iCapa);

                    estrucIni = -1;
                    nombreAnt = "";

                }
                else if (mEstructuras.ElementAt(i).getTipoEstructura == eRoadSeccion.tunel)
                {

                    estrucIni = mEstructuras.ElementAt(i).getPk - miAlzado.getIntervaloSecciones / 2;
                    nombreAnt = mEstructuras.ElementAt(i).getNombre;
                    while ((i < mEstructuras.Count) && (mEstructuras.ElementAt(i).getTipoEstructura == eRoadSeccion.tunel) && (mEstructuras.ElementAt(i).getNombre == nombreAnt))
                    {
                        i++;
                    }
                    estrucFin = mEstructuras.ElementAt(i - 1).getPk + miAlzado.getIntervaloSecciones / 2;
                    double rasanteYIni = miAlzado.getCotaRasante(estrucIni);
                    double rasanteYFin = miAlzado.getCotaRasante(estrucFin);
                    short color = 3;
                    int isPuente = -1;

                    Polyline abajoEst = new Polyline();
                    Polyline derEst = new Polyline();
                    Polyline izqEst = new Polyline();

                    double miY;

                    if (rasanteYIni > rasanteYFin)
                    {

                        miY = miGuitarra.getPuntoOrigY + ((rasanteYFin + (miGuitarra.getEscalaAlto * isPuente / 2)) * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300;
                    }
                    else
                    {
                        miY = miGuitarra.getPuntoOrigY + ((rasanteYIni + (miGuitarra.getEscalaAlto * isPuente / 2)) * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300;
                    }


                    Point2d p1arrEst = new Point2d(miGuitarra.getPuntoOrigX + (estrucIni * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (rasanteYIni * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);
                    Point2d p2arrEst = new Point2d(miGuitarra.getPuntoOrigX + (estrucFin * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (rasanteYFin * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);

                    Point2d p1abEst = new Point2d(miGuitarra.getPuntoOrigX + (estrucIni * 100 / miGuitarra.getEscalaAncho), miY);
                    Point2d p2abEst = new Point2d(miGuitarra.getPuntoOrigX + (estrucFin * 100 / miGuitarra.getEscalaAncho), miY);

                    abajoEst.AddVertexAt(0, p1abEst, 0, 0, 0);
                    abajoEst.AddVertexAt(0, p2abEst, 0, 0, 0);
                    abajoEst.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    derEst.AddVertexAt(0, p2abEst, 0, 0, 0);
                    derEst.AddVertexAt(0, p2arrEst, 0, 0, 0);
                    derEst.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    izqEst.AddVertexAt(0, p1arrEst, 0, 0, 0);
                    izqEst.AddVertexAt(0, p1abEst, 0, 0, 0);
                    izqEst.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);


                    miLst.Add(abajoEst);
                    miLst.Add(derEst);
                    miLst.Add(izqEst);


                    Polyline arriba = new Polyline();
                    Polyline abajo = new Polyline();
                    Polyline der = new Polyline();
                    Polyline izq = new Polyline();

                    Point2d p1arr = new Point2d(miGuitarra.getPuntoOrigX + (estrucIni * 100 / miGuitarra.getEscalaAncho), miYMitadArriba);
                    Point2d p2arr = new Point2d(miGuitarra.getPuntoOrigX + (estrucFin * 100 / miGuitarra.getEscalaAncho), miYMitadArriba);

                    Point2d p1ab = new Point2d(miGuitarra.getPuntoOrigX + (estrucIni * 100 / miGuitarra.getEscalaAncho), miYMitadAbajo);
                    Point2d p2ab = new Point2d(miGuitarra.getPuntoOrigX + (estrucFin * 100 / miGuitarra.getEscalaAncho), miYMitadAbajo);


                    arriba.AddVertexAt(0, p1arr, 0, 0, 0);
                    arriba.AddVertexAt(0, p2arr, 0, 0, 0);
                    arriba.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    abajo.AddVertexAt(0, p1ab, 0, 0, 0);
                    abajo.AddVertexAt(0, p2ab, 0, 0, 0);
                    abajo.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    der.AddVertexAt(0, p2arr, 0, 0, 0);
                    der.AddVertexAt(0, p2ab, 0, 0, 0);
                    der.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    izq.AddVertexAt(0, p1arr, 0, 0, 0);
                    izq.AddVertexAt(0, p1ab, 0, 0, 0);
                    izq.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);

                    miLst.Add(arriba);
                    miLst.Add(abajo);
                    miLst.Add(der);
                    miLst.Add(izq);


                    oTexto.addText2D(nombreAnt, miGuitarra.getPuntoOrigX + ((estrucIni + ((estrucFin - estrucIni) / 2)) * 100 / miGuitarra.getEscalaAncho), miYMitad, 3, 0, 0, iCapa);

                    estrucIni = -1;
                    nombreAnt = "";
                }
            }


            return miLst;
        }

        public List<Entity> dibujaCaja(int pos, string iCapa)
        {
            List<Entity> miLst = new List<Entity>();
            int miPos = pos;

            Polyline acuerdos1 = new Polyline();
            Polyline acuerdos2 = new Polyline();
            Polyline acuerdos3 = new Polyline();
            Polyline acuerdos4 = new Polyline();


            Point2d acuerdo1d = new Point2d(miGuitarra.getPuntoOrigX, miGuitarra.getPuntoOrigY - 10.5 - ((10.5 +50)*miPos));
            Point2d acuerdo1i = new Point2d(miGuitarra.getMaxX, miGuitarra.getPuntoOrigY - 10.5 - (10.5 + 50) * miPos);
            acuerdos1.AddVertexAt(0, acuerdo1d, 0, 0, 0);
            acuerdos1.AddVertexAt(0, acuerdo1i, 0, 0, 0);
            acuerdos1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
            miLst.Add(acuerdos1);

            Point2d acuerdo2d = new Point2d(miGuitarra.getPuntoOrigX, miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * miPos));
            Point2d acuerdo2i = new Point2d(miGuitarra.getMaxX, miGuitarra.getPuntoOrigY - 10.5 - 50 - (10.5 + 50) * miPos);
            acuerdos2.AddVertexAt(0, acuerdo2d, 0, 0, 0);
            acuerdos2.AddVertexAt(0, acuerdo2i, 0, 0, 0);
            acuerdos2.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
            miLst.Add(acuerdos2);

            Point2d acuerdo3ar = new Point2d(miGuitarra.getPuntoOrigX, miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * miPos));
            Point2d acuerdo3ab = new Point2d(miGuitarra.getPuntoOrigX, miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * miPos));
            acuerdos3.AddVertexAt(0, acuerdo3ar, 0, 0, 0);
            acuerdos3.AddVertexAt(0, acuerdo3ab, 0, 0, 0);
            acuerdos3.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
            miLst.Add(acuerdos3);


            Point2d acuerdo4ar = new Point2d(miGuitarra.getMaxX, miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * miPos));
            Point2d acuerdo4ab = new Point2d(miGuitarra.getMaxX, miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * miPos));
            acuerdos4.AddVertexAt(0, acuerdo4ar, 0, 0, 0);
            acuerdos4.AddVertexAt(0, acuerdo4ab, 0, 0, 0);
            acuerdos4.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
            miLst.Add(acuerdos4);

            if( (miPos == 1)||(miPos==2)||(miPos==3)||(miPos==4))
            {
                double j1 = miGuitarra.getPuntoOrigX + 25;
                while (j1 < miGuitarra.getMaxX)
                {
                    Polyline miMarca1 = new Polyline();
                    Point2d miPmarcaH1 = new Point2d(j1, miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * miPos));
                    miMarca1.AddVertexAt(0, miPmarcaH1, 0, 0, 0);
                    Point2d miPmarcaH2 = new Point2d(j1, miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * miPos) - 2);
                    miMarca1.AddVertexAt(0, miPmarcaH2, 0, 0, 0);
                    miMarca1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
                    miLst.Add(miMarca1);

                    Polyline miMarca2 = new Polyline();
                    Point2d miPmarcaH3 = new Point2d(j1, miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * miPos) + 2);
                    miMarca2.AddVertexAt(0, miPmarcaH3, 0, 0, 0);
                    Point2d miPmarcaH4 = new Point2d(j1, miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * miPos));
                    miMarca2.AddVertexAt(0, miPmarcaH4, 0, 0, 0);
                    miMarca2.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
                    miLst.Add(miMarca2);

                    j1 = j1 + 25;
                }
            }
            else if (miPos == 0)
            {
                List<List<double[]>> misAcuerdos = miAlzado.getAcuerdos();
                foreach (List<double[]> miAcuerdo in misAcuerdos)
                {
                    foreach (double[] punto in miAcuerdo)
                    {
                        double j1 = miGuitarra.getPuntoOrigX + (punto[0] * 100 / miGuitarra.getEscalaAncho);

                        Polyline miMarca1 = new Polyline();
                        Polyline miMarca2 = new Polyline();

                        Point2d miPmarcaH1 = new Point2d(j1, miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * miPos));
                        miMarca1.AddVertexAt(0, miPmarcaH1, 0, 0, 0);
                        Point2d miPmarcaH2 = new Point2d(j1, miGuitarra.getPuntoOrigY - 10.5 - ((10.5 + 50) * miPos) - 2);
                        miMarca1.AddVertexAt(0, miPmarcaH2, 0, 0, 0);
                        miMarca1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
                        miLst.Add(miMarca1);

                        Point2d miPmarcaH3 = new Point2d(j1, miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * miPos) + 2);
                        miMarca2.AddVertexAt(0, miPmarcaH3, 0, 0, 0);
                        Point2d miPmarcaH4 = new Point2d(j1, miGuitarra.getPuntoOrigY - 10.5 - 50 - ((10.5 + 50) * miPos));
                        miMarca2.AddVertexAt(0, miPmarcaH4, 0, 0, 0);
                        miMarca2.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);
                        miLst.Add(miMarca2);
                    }

                }
            }

            if (miPos == 0)
            {
                dibujaCajaAcuerdos(miPos, iCapa);
            }else if (miPos == 1)
            {
                dibujaCajaCotaTerreno(miPos, iCapa);
            } else if (miPos == 2)
            {
                dibujaCajaCotaRasante(miPos, iCapa);
            } else if (miPos == 3)
            {
                dibujaCajaCotaDesmonte(miPos, iCapa);
            } else if (miPos == 4)
            {
                dibujaCajaCotaTerraplen(miPos, iCapa);
            }
            else if (miPos == 5)
            {
                List<Entity> miLista = dibujaCajaCurvaturas(miPos, iCapa);
                foreach (Entity miEntidad in miLista)
                {
                    miLst.Add(miEntidad);
                }
            }
            else if (miPos == 7)
            {
                List<Entity> miLista = dibujaCajaPeralte(miPos, iCapa);
                foreach (Entity miEntidad in miLista)
                {
                    miLst.Add(miEntidad);
                }
            }
            else if (miPos == 6)
            {
                List<Entity> miLista = dibujaCajaEstructura(miPos, iCapa);
                foreach (Entity miEntidad in miLista)
                {
                    miLst.Add(miEntidad);
                }
            }

            return miLst;
        }


        private List<Entity> getLstEntidadesEje(string iCapa)
        {
            List<Entity> miLst = new List<Entity>();

            int cuadrado = (int)miAlzado.getMinZ / miGuitarra.getEscalaAlto;

            Polyline miLw1 = new Polyline();

            List<double[]> verticesAlzado = miAlzado.getVerticesAlzado;
            foreach (double[] vertice in verticesAlzado)
            {

                Point2d miP1 = new Point2d(miGuitarra.getPuntoOrigX + (vertice[3] * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (vertice[2] * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);
                miLw1.AddVertexAt(0, miP1, 0, 0, 0);
            }

            miLw1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 1);
            miLst.Add(miLw1);
            return miLst;

        }


        private List<Entity> getLstEntidadesTerreno(string iCapa)
        {
            List<Entity> miLst = new List<Entity>();


            int cuadrado = (int)miAlzado.getMinZ / miGuitarra.getEscalaAlto;
            Polyline miLw1 = new Polyline();

            List<double[]> verticesAlzado = miAlzado.drawTerreno();
            List<string> misCotas = new List<string>();
            foreach (double[] vertice in verticesAlzado)
            {

                Point2d miP1 = new Point2d(miGuitarra.getPuntoOrigX + (vertice[3] * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (vertice[2] * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);
                miLw1.AddVertexAt(0, miP1, 0, 0, 0);
            }

            miLw1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 0);

            
            miLst.Add(miLw1);





            return miLst;

        }

        private List<Entity> getLstEntidadesAcuerdos(string iCapa)
        {
            List<Entity> miLst = new List<Entity>();

            int cuadrado = (int)miAlzado.getMinZ / miGuitarra.getEscalaAlto;

            Polyline miLw1 = new Polyline();

            List<double[]> verticesAlzado = miAlzado.drawAcuerdos();
            foreach (double[] vertice in verticesAlzado)
            {

                Point2d miP1 = new Point2d(miGuitarra.getPuntoOrigX + (vertice[0] * 100 / miGuitarra.getEscalaAncho), miGuitarra.getPuntoOrigY + (vertice[1] * 100 / miGuitarra.getEscalaAlto) - cuadrado * 100 + 300);
                miLw1.AddVertexAt(0, miP1, 0, 0, 0);
            }

            miLw1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 1);
            miLst.Add(miLw1);
            return miLst;

        }
        
        private string getStringPK(double i)
        {


            int miles = (int)(i / 1000);
            double d = i - miles * 1000;
            int cent = (int)(d / 100);
            d = d - cent * 100;
            int dec = (int)(d / 10);
            d = d - dec * 10;
            int uni = (int)d;
            d = d - uni;

            d = Math.Truncate(d * 100);

            string miPk = miles + "+" + cent + dec + uni;
            if (d != 0)
            {
                if (d >= 10)
                    miPk += "." + d;
                else
                    miPk += ".0" + d;
            }

            return miPk;
        }

        public Polyline getPolylineEjeAlzado()
        {
            return miEjeAlzado;
        }



    }
}
