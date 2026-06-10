using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTrazadoTadil
{
    using sd = System.Drawing;


    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;

    using engCadNet;
    using EjeDeTrazado.puntosDelEje;
    using EjeDeTrazado;
    using tadLayShare.puntos;
    using System.Windows.Forms;


    public class oEjeTrazadoTadilRotular
    {



        public oEjeTrazadoTadilRotular()
        {

        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="iEjeTrazado"></param>
       /// <param name="iCapaPK"></param>
       /// <param name="iCapaPuntosSing"></param>
        public void rotular(EjeTrazado iEjeTrazado, string iCapaPK, string iCapaPuntosSing)
        {

            //Obtengo la Coleccion
            List<Entity> miLstEntidades = getLstEntidades(iEjeTrazado);

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    miLstEntidades = getLstTransversales(iEjeTrazado,20, 1.25, 1.25, false, iCapaPK);

                    //Añado los Objetos Restantes a la Coleccion al Dwg
                    foreach (Entity item in miLstEntidades)
                    {
                        acBlockTableRec.AppendEntity(item);
                        tr.AddNewlyCreatedDBObject(item, true);
                        item.SetDatabaseDefaults();
                        item.Layer = iCapaPK;
                    }


                    miLstEntidades = getLstTransversales(iEjeTrazado, 100, 25, 0, true, iCapaPK);
                    //Añado los Objetos Restantes a la Coleccion al Dwg
                    foreach (Entity item in miLstEntidades)
                    {
                        acBlockTableRec.AppendEntity(item);
                        tr.AddNewlyCreatedDBObject(item, true);
                        item.SetDatabaseDefaults();
                        item.Layer = iCapaPK;
                    }


                    miLstEntidades = getLstSingulares(iEjeTrazado, 0, 10, true, iCapaPuntosSing);
                    //Añado los Objetos Restantes a la Coleccion al Dwg
                    foreach (Entity item in miLstEntidades)
                    {
                        acBlockTableRec.AppendEntity(item);
                        tr.AddNewlyCreatedDBObject(item, true);
                        item.SetDatabaseDefaults();
                        item.Layer = iCapaPuntosSing;
                    }


                    tr.Commit();

                }
            }

        }

        private List<Entity> getLstEntidades(EjeTrazado miEje)
        {


            InfoComponentes miInfo = miEje.draw();
            List<InfoComponente> misComponentes = miInfo.getInfoComponentes;
            List<Entity> miLst = new List<Entity>();

            foreach (InfoComponente miInfoC in misComponentes)
            {
                if (miInfoC.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                {
                    double[] misDatosCurva = miInfoC.getValoresCurva;
                    Point3d miCentro = new Point3d(misDatosCurva[0], misDatosCurva[1], 0);

                    Arc miArco = new Arc(miCentro, misDatosCurva[2], misDatosCurva[3], misDatosCurva[4]);
                    miArco.Color = Color.FromColorIndex(ColorMethod.ByLayer, 2);


                    miLst.Add(miArco);
                }
                else if (miInfoC.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                {
                    List<Punto3d> miListaPuntos = miInfoC.getPolilinea;
                    Polyline miLw1 = new Polyline();
                    foreach (Punto3d miPunto in miListaPuntos)
                    {
                        Point2d miP1 = new Point2d(miPunto.coordenadaX, miPunto.coordenadaY);
                        miLw1.AddVertexAt(0, miP1, 0, 0, 0);
                        miLw1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 1);
                    }


                    miLst.Add(miLw1);


                }
                else
                {
                    List<Punto3d> miListaPuntos = miInfoC.getPolilinea;
                    Polyline miLw1 = new Polyline();
                    foreach (Punto3d miPunto in miListaPuntos)
                    {
                        Point2d miP1 = new Point2d(miPunto.coordenadaX, miPunto.coordenadaY);
                        miLw1.AddVertexAt(0, miP1, 0, 0, 0);
                        miLw1.Color = Color.FromColorIndex(ColorMethod.ByLayer, 3);
                    }


                    miLst.Add(miLw1);
                }
            }

            return miLst;
        }

        private List<Entity> getLstTransversales(EjeTrazado miEje, double dist, double dere, double izq, bool blanco, string iCapa)
        {
            short colorCurva;
            short colorClo;
            short colorLinea;
            if (blanco)
            {
                colorCurva = 0;
                colorClo = 0;
                colorLinea = 0;
            }
            else
            {
                colorCurva = 1;
                colorClo = 3;
                colorLinea = 2;
            }

            InfoComponentes miInfo = miEje.draw();
            List<InfoComponente> misComponentes = miInfo.getInfoComponentes;
            List<Entity> miLst = new List<Entity>();

            double i = 0;

            foreach (InfoComponente miInfoC in misComponentes)
            {


                if (miInfoC.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                {

                    while (i < miInfoC.getPkFinal - 0.1)
                    {
                        // Calcular azimut del eje geometricamente (robusto, no depende de estado interno)
                        double iAnt = Math.Max(miInfoC.getPkInicial + 0.001, i - 0.01);
                        double iPost = Math.Min(miInfoC.getPkFinal - 0.001, i + 0.01);
                        double[] miPuntoAnt = miEje.getPointAtDist(iAnt);
                        double[] miPuntoPost = miEje.getPointAtDist(iPost);
                        double miAzEje = Math.Atan2(miPuntoPost[1] - miPuntoAnt[1], miPuntoPost[0] - miPuntoAnt[0]) * 180 / Math.PI;
                        double miAzDer = miAzEje - 90;

                        Polyline miLw1 = new Polyline();
                        double[] miPuntoIni = miEje.getPointAtDist(i);
                        double[] miPunto1 = new double[] {
                            miPuntoIni[0] + dere * Math.Cos(miAzDer * Math.PI / 180),
                            miPuntoIni[1] + dere * Math.Sin(miAzDer * Math.PI / 180)
                        };
                        double miAzIzq = miAzEje + 90;
                        double[] miPunto2 = new double[] {
                            miPuntoIni[0] + izq * Math.Cos(miAzIzq * Math.PI / 180),
                            miPuntoIni[1] + izq * Math.Sin(miAzIzq * Math.PI / 180)
                        };
                        Point2d miP1 = new Point2d(miPunto1[0], miPunto1[1]);
                        Point2d miP2 = new Point2d(miPunto2[0], miPunto2[1]);
                        Point2d miPIni = new Point2d(miPuntoIni[0], miPuntoIni[1]);
                        miLw1.AddVertexAt(0, miP1, 0, 0, 0);
                        miLw1.AddVertexAt(0, miPIni, 0, 0, 0);
                        miLw1.AddVertexAt(0, miP2, 0, 0, 0);
                        miLw1.Color = Color.FromColorIndex(ColorMethod.ByLayer, colorCurva);
                        miLst.Add(miLw1);

                        string miPk = getStringPK(i, dist);
                        addTransversalText(miPk, miPunto1, miAzDer, colorCurva, iCapa);

                        i = i + dist;
                    }
                }
                else if (miInfoC.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                {

                    while (i < miInfoC.getPkFinal - 0.1)
                    {
                        // Calcular azimut del eje geometricamente (robusto, no depende de estado interno)
                        double iAnt = Math.Max(miInfoC.getPkInicial + 0.001, i - 0.01);
                        double iPost = Math.Min(miInfoC.getPkFinal - 0.001, i + 0.01);
                        double[] miPuntoAnt = miEje.getPointAtDist(iAnt);
                        double[] miPuntoPost = miEje.getPointAtDist(iPost);
                        double miAzEje = Math.Atan2(miPuntoPost[1] - miPuntoAnt[1], miPuntoPost[0] - miPuntoAnt[0]) * 180 / Math.PI;
                        double miAzDer = miAzEje - 90;

                        Polyline miLw3 = new Polyline();
                        double[] miPuntoIni = miEje.getPointAtDist(i);
                        double[] miPunto1 = new double[] {
                            miPuntoIni[0] + dere * Math.Cos(miAzDer * Math.PI / 180),
                            miPuntoIni[1] + dere * Math.Sin(miAzDer * Math.PI / 180)
                        };
                        double miAzIzq = miAzEje + 90;
                        double[] miPunto2 = new double[] {
                            miPuntoIni[0] + izq * Math.Cos(miAzIzq * Math.PI / 180),
                            miPuntoIni[1] + izq * Math.Sin(miAzIzq * Math.PI / 180)
                        };
                        Point2d miP = new Point2d(miPunto1[0], miPunto1[1]);
                        Point2d miP2 = new Point2d(miPunto2[0], miPunto2[1]);
                        Point2d miPIni = new Point2d(miPuntoIni[0], miPuntoIni[1]);
                        miLw3.AddVertexAt(0, miP, 0, 0, 0);
                        miLw3.AddVertexAt(0, miPIni, 0, 0, 0);
                        miLw3.AddVertexAt(0, miP2, 0, 0, 0);
                        miLw3.Color = Color.FromColorIndex(ColorMethod.ByLayer, colorLinea);
                        miLst.Add(miLw3);

                        string miPk = getStringPK(i, dist);
                        addTransversalText(miPk, miPunto1, miAzDer, colorLinea, iCapa);

                        i = i + dist;
                    }


                }
                else
                {

                    while (i < miInfoC.getPkFinal - 0.1)
                    {
                        // 1. Punto del eje en la distancia i
                        double[] miPuntoIni = miEje.getPointAtDist(i);

                        // 2. Puntos anterior y posterior para calcular azimut local del eje
                        double iAnt = Math.Max(miInfoC.getPkInicial + 0.001, i - 0.01);
                        double iPost = Math.Min(miInfoC.getPkFinal - 0.001, i + 0.01);
                        double[] miPuntoAnt = miEje.getPointAtDist(iAnt);
                        double[] miPuntoPost = miEje.getPointAtDist(iPost);

                        // 3. Azimut local del eje en grados (sistema matemático, antihorario desde Este)
                        double miAzEje = Math.Atan2(miPuntoPost[1] - miPuntoAnt[1], miPuntoPost[0] - miPuntoAnt[0]) * 180 / Math.PI;

                        // 4. Transversal: +90° derecha, -90° izquierda (corregido para clotoides)
                        double miAzDer = miAzEje - 90;
                        double miAzIzq = miAzEje + 90;

                        // 5. Puntos laterales aplicando el offset sobre miPuntoIni
                        double[] miPunto1 = new double[] {
                            miPuntoIni[0] + dere * Math.Cos(miAzDer * Math.PI / 180),
                            miPuntoIni[1] + dere * Math.Sin(miAzDer * Math.PI / 180)
                        };
                        double[] miPunto2 = new double[] {
                            miPuntoIni[0] + izq * Math.Cos(miAzIzq * Math.PI / 180),
                            miPuntoIni[1] + izq * Math.Sin(miAzIzq * Math.PI / 180)
                        };

                        Polyline miLw3 = new Polyline();
                        Point2d miP = new Point2d(miPunto1[0], miPunto1[1]);
                        Point2d miPIni = new Point2d(miPuntoIni[0], miPuntoIni[1]);
                        Point2d miP2 = new Point2d(miPunto2[0], miPunto2[1]);
                        miLw3.AddVertexAt(0, miP, 0, 0, 0);
                        miLw3.AddVertexAt(0, miPIni, 0, 0, 0);
                        miLw3.AddVertexAt(0, miP2, 0, 0, 0);
                        miLw3.Color = Color.FromColorIndex(ColorMethod.ByLayer, colorClo);
                        miLst.Add(miLw3);

                        string miPk = getStringPK(i, dist);
                        addTransversalText(miPk, miPunto1, miAzDer, colorClo, iCapa);

                        i = i + dist;
                    }
                }
            }


            return miLst;
        }

        private void addTransversalText(string pkText, double[] miPunto1, double miAzDer, short color, string iCapa)
        {
            double angleDeg = (miAzDer % 360 + 360) % 360;
            double finalAngleRad = angleDeg * Math.PI / 180;
            TextHorizontalMode alignment = TextHorizontalMode.TextLeft;

            double posX = miPunto1[0] + 3 * Math.Cos(miAzDer * Math.PI / 180);
            double posY = miPunto1[1] + 3 * Math.Sin(miAzDer * Math.PI / 180);

            addText2DAligned(pkText, posX, posY, 4, finalAngleRad, color, iCapa, alignment);
        }

        private static void addText2DAligned(string iTexto, double iX, double iY, double iAltura, double iRotateRadianes, int iColorIndex, string iLayer, TextHorizontalMode alignment)
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable miBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord miBlockTableRec = tr.GetObject(miBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    DBText miText = new DBText();
                    miText.SetDatabaseDefaults();
                    miText.ColorIndex = iColorIndex;
                    miText.Height = iAltura;
                    miText.HorizontalMode = alignment;
                    miText.VerticalMode = TextVerticalMode.TextVerticalMid;
                    miText.Position = new Point3d(iX, iY, 0);
                    miText.AlignmentPoint = miText.Position;
                    miText.Rotation = iRotateRadianes;
                    miText.TextString = iTexto;
                    miText.Layer = iLayer;

                    miBlockTableRec.AppendEntity(miText);
                    tr.AddNewlyCreatedDBObject(miText, true);
                    tr.Commit();
                }
            }
        }

        private string getStringPK(double i, double dist)
        {

            int miles = (int)(i / 1000);
            double d = i - miles * 1000;
            int cent = (int)(d / 100);
            d = d - cent * 100;
            int dec = (int)(d / 10);
            d = d - dec * 10;
            int uni = (int) d;
            d = d - uni;

            d = Math.Truncate(d*100);
            
            string miPk = miles + "+" + cent + dec + uni;
            if (d != 0)
            {
                if (d >= 10)
                    miPk += "." + d;
                else
                    miPk += ".0" + d;
            }


            if ((dist != 100) && (dec == 0) && (d == 0))
            {
                miPk = " ";
            }


            return miPk;
        }

        private List<Entity> getLstSingulares(EjeTrazado miEje, double dere, double izq, bool blanco, string iCapa)
        {
            short colorCurva;
            short colorClo;
            short colorLinea;
            if (blanco)
            {
                colorCurva = 0;
                colorClo = 0;
                colorLinea = 0;
            }
            else
            {
                colorCurva = 1;
                colorClo = 3;
                colorLinea = 2;
            }

            InfoComponentes miInfo = miEje.draw();
            List<InfoComponente> misComponentes = miInfo.getInfoComponentes;
            List<Entity> miLst = new List<Entity>();


            for (int i = 0; i < misComponentes.Count; i++)
            {
                double pk = misComponentes[i].getPkInicial;
                InfoComponente compAnterior = i > 0 ? misComponentes[i - 1] : null;
                InfoComponente compPosterior = misComponentes[i];
                dibujarPuntoSingular(miEje, pk, compAnterior, compPosterior, false, dere, izq, colorCurva, colorLinea, colorClo, iCapa, miLst);
            }

            if (misComponentes.Count > 0)
            {
                double pk = misComponentes[misComponentes.Count - 1].getPkFinal;
                InfoComponente compAnterior = misComponentes[misComponentes.Count - 1];
                InfoComponente compPosterior = null;
                dibujarPuntoSingular(miEje, pk, compAnterior, compPosterior, true, dere, izq, colorCurva, colorLinea, colorClo, iCapa, miLst);
            }

            return miLst;

        }

        private void dibujarPuntoSingular(EjeTrazado miEje, double pk, InfoComponente compAnterior, InfoComponente compPosterior, bool isEnd, double dere, double izq, short colorCurva, short colorLinea, short colorClo, string iCapa, List<Entity> miLst)
        {
            InfoComponente primaryComp = compPosterior ?? compAnterior;
            if (primaryComp == null) return;

            short color = 0;
            if (primaryComp.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                color = colorCurva;
            else if (primaryComp.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                color = colorLinea;
            else
                color = colorClo;

            double[] ptAnt, ptPost;
            if (isEnd)
            {
                ptAnt = miEje.getPointAtDist(pk - 0.02);
                ptPost = miEje.getPointAtDist(pk);
            }
            else
            {
                ptAnt = miEje.getPointAtDist(pk);
                ptPost = miEje.getPointAtDist(pk + 0.02);
            }

            double miAzEje = Math.Atan2(ptPost[1] - ptAnt[1], ptPost[0] - ptAnt[0]) * 180 / Math.PI;
            double miAzDer = miAzEje - 90;
            double miAzIzq = miAzEje + 90;

            double[] pt = miEje.getPointAtDist(pk);
            double[] miPunto1 = new double[] {
                pt[0] + dere * Math.Cos(miAzDer * Math.PI / 180),
                pt[1] + dere * Math.Sin(miAzDer * Math.PI / 180)
            };
            double[] miPunto2 = new double[] {
                pt[0] + izq * Math.Cos(miAzIzq * Math.PI / 180),
                pt[1] + izq * Math.Sin(miAzIzq * Math.PI / 180)
            };

            Polyline miLw = new Polyline();
            miLw.AddVertexAt(0, new Point2d(miPunto1[0], miPunto1[1]), 0, 0, 0);
            miLw.AddVertexAt(0, new Point2d(miPunto2[0], miPunto2[1]), 0, 0, 0);
            miLw.Color = Color.FromColorIndex(ColorMethod.ByLayer, color);
            miLst.Add(miLw);

            string miPk = getStringPK(pk, 100);
            double tex1X = miPunto1[0] + 40 * Math.Cos(miAzIzq * Math.PI / 180);
            double tex1Y = miPunto1[1] + 40 * Math.Sin(miAzIzq * Math.PI / 180);
            oTexto.addText2D(miPk, tex1X, tex1Y, 4, miAzDer * Math.PI / 180, color, iCapa);

            double miDir = miAzIzq + 90;

            if (compAnterior != null)
            {
                short colorAnt = 0;
                string etiquetaAnt = "";
                if (compAnterior.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                {
                    colorAnt = colorCurva;
                    etiquetaAnt = "R=" + Math.Round(compAnterior.getValoresCurva[2], 2);
                }
                else if (compAnterior.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                {
                    colorAnt = colorLinea;
                    etiquetaAnt = "RECTA";
                }
                else
                {
                    colorAnt = colorClo;
                    etiquetaAnt = "A=" + Math.Round(compAnterior.getValorA, 2);
                }

                double texAntX = tex1X + 7 * Math.Cos(miDir * Math.PI / 180);
                double texAntY = tex1Y + 7 * Math.Sin(miDir * Math.PI / 180);
                oTexto.addText2D(etiquetaAnt, texAntX, texAntY, 4, miAzDer * Math.PI / 180, colorAnt, iCapa);
            }

            if (compPosterior != null)
            {
                short colorPost = 0;
                string etiquetaPost = "";
                if (compPosterior.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.curva)
                {
                    colorPost = colorCurva;
                    etiquetaPost = "R=" + Math.Round(compPosterior.getValoresCurva[2], 2);
                }
                else if (compPosterior.getTipoComponente == EjeDeTrazado.componentes.Componente.tipoComponente.linea)
                {
                    colorPost = colorLinea;
                    etiquetaPost = "RECTA";
                }
                else
                {
                    colorPost = colorClo;
                    etiquetaPost = "A=" + Math.Round(compPosterior.getValorA, 2);
                }

                double texPostX = tex1X - 7 * Math.Cos(miDir * Math.PI / 180);
                double texPostY = tex1Y - 7 * Math.Sin(miDir * Math.PI / 180);
                oTexto.addText2D(etiquetaPost, texPostX, texPostY, 4, miAzDer * Math.PI / 180, colorPost, iCapa);
            }
        }

        

    }

}
