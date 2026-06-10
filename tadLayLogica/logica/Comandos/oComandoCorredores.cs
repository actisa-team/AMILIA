using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayShare.puntos;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using tadLayLogica.zonaGis;
using engCadNet;
using Autodesk.AutoCAD.Colors;
using System.Diagnostics;
using Autodesk.AutoCAD.ApplicationServices;
using tadLayLogica.Comandos;

namespace tadLayLogica.logica.Comandos
{
    public class oComandoCorredores
    {


        public static void createCorredores(double iDistancia)
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                //Activo las Capas
                oTadil.data.Layer.zonaNoPasoUsuario.On();
                oTadil.data.Layer.zonaNoPasoPendiente.On();
                oTadil.data.Layer.zonasGisOn();



                //Logica Capas
                oTadil.data.Layer.visibilidadEje.On();
                oTadil.data.Layer.visibilidadEje.Current();
                oTadil.data.Layer.visibilidadEje.deleteItems();


                //Creo el Eje Visibilidad
                Stopwatch miMedicion = new Stopwatch();

                miMedicion.Start();


                //Debo de Resetear las Zonas de No Paso ; Para que no coja las de la Cache.
                oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();

                List<Polyline> miLstObstacle = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;


                //Creo la polilinea desde puntoSalida hasta puntoLlegada

                generateCorredores(new oP3d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y, 0), new oP3d(oSingletonProyecto.getInstance.ptoLlegada.X, oSingletonProyecto.getInstance.ptoLlegada.Y, 0), iDistancia);


                miMedicion.Stop();

                //Desactivo las Capas GIS
                oTadil.data.Layer.zonasGisOff();

                //Info UI
                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);


            }
        }

        //crear corredores
        public static void generateCorredores(oP3d iC1, oP3d iC2, double iDistance)
        {
            List<Polyline> misCorredores = new List<Polyline>();
            Line miLine = new Line(new Point3d(iC1.X, iC1.Y, iC1.Z), new Point3d(iC2.X, iC2.Y, iC2.Z));

            List<double[]> misPuntosAtDist = new List<double[]>();
            bool derechaDentro = true;
            bool izquierdaDentro = true;

            double miDist = iDistance;
            while (derechaDentro)
            {
                double[] miPunto = getPointAtLocation(miDist, true, miLine);
                if (!oSingletonTerreno.getInstance.isPtoInsideTerreno(new oP2d(miPunto[0], miPunto[1])))
                {
                    derechaDentro = false;
                }
                else
                {
                    if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPunto[0], miPunto[1]))
                    {
                        misPuntosAtDist.Add(miPunto);
                    }
                    miDist = miDist + iDistance;
                }
            }

            miDist = iDistance;
            while (izquierdaDentro)
            {
                double[] miPunto = getPointAtLocation(miDist, false, miLine);
                if (!oSingletonTerreno.getInstance.isPtoInsideTerreno(new oP2d(miPunto[0], miPunto[1])))
                {
                    izquierdaDentro = false;
                }
                else
                {
                    if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPunto[0], miPunto[1]))
                    {
                        misPuntosAtDist.Add(miPunto);
                    }
                    miDist = miDist + iDistance;
                }

            }

            oSingletonZonaNoPaso.getInstance.zonasNoPasoReset();
            List<Polyline> misZonasNoPaso = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;

            foreach (double[] miPunto in misPuntosAtDist)
            {


                Polyline miCorredor1 = new Polyline();
                Polyline miCorredor2 = new Polyline();

                miCorredor1 = oComandoEjeVisibilidad.createAutomatico(miLine.StartPoint.X, miLine.StartPoint.Y, miPunto[0], miPunto[1]);
                miCorredor2 = oComandoEjeVisibilidad.createAutomatico(miPunto[0], miPunto[1], miLine.EndPoint.X, miLine.EndPoint.Y);


                Polyline miCorredor = new Polyline();

                for (int i = 0; i < miCorredor1.NumberOfVertices; i++)
                {
                    miCorredor.AddVertexAt(i, miCorredor1.GetPoint2dAt(i), 0, 0, 0);
                }
                int j = miCorredor.NumberOfVertices;
                for (int i = 0; i < miCorredor2.NumberOfVertices; i++)
                {
                    miCorredor.AddVertexAt(i + j, miCorredor2.GetPoint2dAt(i), 0, 0, 0);
                }
                misCorredores.Add(miCorredor);



            }

            foreach (Polyline miCorr in misCorredores)
            {
                List<Point3d> misPuntos = oLw.getLstPto(miCorr);
                Point3dCollection miCol = new Point3dCollection();
                foreach(Point3d miPunto in misPuntos)
                {
                    miCol.Add(miPunto);

                }
                Polyline miCorredor = oLw.addLw2d(miCol, false, oTadil.data.Layer.visibilidadEje.name);

                using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miCorredor))
                {
                    miLw.open();

                    miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oTadil.data.Layer.visibilidadEje.color);

                    miLw.entidad.Layer = oTadil.data.Layer.visibilidadEje.name;

                    miLw.save();
                }

            }



            bool isOnZona = false;
            foreach (Polyline miZonaNP in misZonasNoPaso)
            {

                Point3dCollection miColPtoInter = new Point3dCollection();
                miLine.IntersectWith(miZonaNP, Intersect.OnBothOperands, miColPtoInter, IntPtr.Zero, IntPtr.Zero);

                if (miColPtoInter.Count != 0)
                {
                    isOnZona = true;
                }
            }
            if (!isOnZona)
            {
                Point3dCollection miPuntosLinea = new Point3dCollection();
                miPuntosLinea.Add(miLine.StartPoint);
                miPuntosLinea.Add(miLine.EndPoint);

                Polyline miCorredor = oLw.addLw2d(miPuntosLinea, false, oTadil.data.Layer.visibilidadEje.name);

                using (oEntidad<Polyline> miLw = new oEntidad<Polyline>(miCorredor))
                {
                    miLw.open();

                    miLw.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oTadil.data.Layer.visibilidadEje.color);

                    miLw.entidad.Layer = oTadil.data.Layer.visibilidadEje.name;

                    miLw.save();
                }
            }

            oTadil.data.Layer.visibilidadEje.On();





        }


        private static double[] getPointAtDist(double iDistancia, Line iLinea)
        {
            double[] miPunto = new double[2];
            double miPK = iDistancia;


            double miAzimut = oTrigo.getAzimutGrados(new oP2d(iLinea.StartPoint.X, iLinea.StartPoint.Y), new oP2d(iLinea.EndPoint.X, iLinea.EndPoint.Y));

            double miXi = iLinea.StartPoint.X + miPK * Math.Sin(miAzimut * Math.PI / 180);
            double miYi = iLinea.StartPoint.Y + miPK * Math.Cos(miAzimut * Math.PI / 180);

            miPunto[0] = miXi;
            miPunto[1] = miYi;

            return miPunto;
        }

        private static double[] getPointAtLocation(double iOffset, bool iDerecha, Line iLinea)
        {

            double miDistIniFin = iLinea.Length;
            double miDistPMedio = miDistIniFin/2;

            double[] miPuntoDis = getPointAtDist(miDistPMedio, iLinea);

            double[] miPunto = new double[2];


            double miD1 = iLinea.EndPoint.X - iLinea.StartPoint.X;
            double miD2 = iLinea.EndPoint.Y - iLinea.StartPoint.Y;

            double miAzimut = getAzimut(miD1, miD2); 


            double miAzimutTrans;
            if ((miAzimut < 180) && (miAzimut > 90))
            {
                miAzimut = miAzimut + 180;

            }
            else if ((miAzimut < 360) && (miAzimut > 270))
            {
                miAzimut = miAzimut - 180;
            }
            if (iDerecha)
            {
                if (miAzimut - 90 >= 0)
                {
                    miAzimutTrans = miAzimut - 90;
                }
                else
                {
                    miAzimutTrans = miAzimut - 90 + 360;
                }

            }
            else
            {
                if (miAzimut + 90 >= 360)
                {
                    miAzimutTrans = miAzimut + 90 - 360;
                }
                else
                {
                    miAzimutTrans = miAzimut + 90;
                }
            }

            miPunto[0] = miPuntoDis[0] + iOffset * Math.Cos(miAzimutTrans * Math.PI / 180);
            miPunto[1] = miPuntoDis[1] + iOffset * Math.Sin(miAzimutTrans * Math.PI / 180);

            return miPunto;
        }



        private static double getAzimut(double iDx, double iDy)
        {

            double miDelta;
            if ((iDx == 0) || (iDy == 0))
            {
                miDelta = 0;
            }
            else
            {
                miDelta = Math.Atan(iDx / iDy);
            }
            double miDeltaGra = miDelta * 180 / Math.PI;
            double miAzimut;

            if (miDeltaGra == 0)
            {
                if (iDy == 0)
                {
                    if (iDx < 0)
                    {
                        miAzimut = 180;
                    }
                    else
                    {
                        miAzimut = 0;
                    }
                }
                else
                {
                    if (iDx < 0)
                    {
                        miAzimut = 270;
                    }
                    else
                    {
                        miAzimut = 90;
                    }
                }
            }
            else
            {
                if (miDeltaGra < 0)
                {
                    if (iDx >= 0)
                    {
                        miAzimut = 90 - miDeltaGra;
                    }
                    else
                    {
                        miAzimut = 270 - miDeltaGra;
                    }
                }
                else
                {
                    if (iDy >= 0)
                    {
                        miAzimut = 90 - miDeltaGra;
                    }
                    else
                    {
                        miAzimut = 270 - miDeltaGra;
                    }
                }
            }
            return miAzimut;
        }
    }

}
