using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using engCadNet;
using tadLayLogica.zonaGis;
using tadLayLogica.logica.EjeBasicoNew.collection;

namespace tadLayLogica.logica.EjeBasicoNew
{
    public class oEntronqueFinalPM
    {

        private static oTramosEntronqueFinalPMCollection mEntronqueFinalCollection = null;



        public static void createEntronqueFinalPM(int iIdAbanico, oTramosValoracion iValoracionesGlobales, Point3d iPuntoFinalTramoSalida, Point3d iPuntoFinalTramoLlegada, oRoadDes iRoadDesign, oTramoEjeBasico iTramoPrevioSalida, oTramoEjeBasico iTramoPrevioLlegada)
        {
            List<Circle> misCirculosSalida = new List<Circle>();
            List<Circle> misCirculosLlegada = new List<Circle>();

            double miRadio = 0.5 * iRoadDesign.AijMin;
            double distanciaRadios = 0.25 * iRoadDesign.AijMin;
            while (miRadio <= 1.75 * iRoadDesign.AijMin)
            {
                Circle myCircleSalida = new Circle();
                Circle myCircleLlegada = new Circle();

                myCircleSalida.Center = iPuntoFinalTramoSalida;
                myCircleSalida.Radius = miRadio;
                misCirculosSalida.Add(myCircleSalida);
                //prueba
                double[] point = new double[2];
                point[0] = iPuntoFinalTramoSalida.X;
                point[1] = iPuntoFinalTramoSalida.Y;

                //oCircle.addCircle2D(point, miRadio, oTadil.data.Layer.visibilidadEje.name);

                myCircleLlegada.Center = iPuntoFinalTramoLlegada;
                myCircleLlegada.Radius = miRadio;
                misCirculosLlegada.Add(myCircleLlegada);

                //prueba
                double[] point2 = new double[2];
                point2[0] = iPuntoFinalTramoLlegada.X;
                point2[1] = iPuntoFinalTramoLlegada.Y;

                //oCircle.addCircle2D(point2, miRadio, oTadil.data.Layer.visibilidadEje.name);

                miRadio = miRadio + distanciaRadios;

            }

            Point3dCollection misPuntosintersec = new Point3dCollection();
            foreach (Circle miCirculoSalida in misCirculosSalida)
            {
                foreach (Circle miCirculoLlegada in misCirculosLlegada)
                {

                    Point3dCollection miColInter = new Point3dCollection();
                    miCirculoSalida.IntersectWith(miCirculoLlegada, Intersect.ExtendThis, miColInter, IntPtr.Zero, IntPtr.Zero);
                    foreach (Point3d miPunto in miColInter)
                    {
                        misPuntosintersec.Add(miPunto);
                    }
                }
            }

            List<Polyline> misZonasNoPaso = oSingletonZonaNoPaso.getInstance.lstLwZonasNoPaso;
            List<Polyline> misTramos = new List<Polyline>();
            foreach (Point3d miPunto in misPuntosintersec)
            {
                Polyline miTramo = new Polyline();
                miTramo.AddVertexAt(0, new Point2d(iPuntoFinalTramoSalida.X, iPuntoFinalTramoSalida.Y), 0, 0, 0);
                miTramo.AddVertexAt(1, new Point2d(miPunto.X, miPunto.Y), 0, 0, 0);
                miTramo.AddVertexAt(2, new Point2d(iPuntoFinalTramoLlegada.X, iPuntoFinalTramoLlegada.Y), 0, 0, 0);

                bool isOnZonaNP = false;
                foreach (Polyline miZonaNP in misZonasNoPaso)
                {

                    Point3dCollection miColPtoInter = new Point3dCollection();
                    miTramo.IntersectWith(miZonaNP, Intersect.OnBothOperands, miColPtoInter, IntPtr.Zero, IntPtr.Zero);

                    if (miColPtoInter.Count != 0)
                    {
                        isOnZonaNP = true;
                    }
                }
                if (!isOnZonaNP)
                {
                    misTramos.Add(miTramo);
                }
            }
            mEntronqueFinalCollection = new oTramosEntronqueFinalPMCollection();
            mEntronqueFinalCollection.createTramosEntronqueFinalPM(iIdAbanico, iValoracionesGlobales, misTramos, iTramoPrevioSalida, iTramoPrevioLlegada);

        }

        


        public static List<oTramoEjeBasico> calcularTramosGanadores(oTramoEjeBasico iTramoPrevioSalida, oTramoEjeBasico iTramoPrevioLlegada)
        {
            return mEntronqueFinalCollection.calcularTramosGanadores(iTramoPrevioSalida, iTramoPrevioLlegada, false);
        }


    }

}
