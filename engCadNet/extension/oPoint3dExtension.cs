using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{

    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using tadLayLan;


   
    public static  class oPoint3dExtension
    {



        public static Point3d getPtoMedioTwoPoint (Point3d iPtoInicial, Point3d iPtoFin)
        {
            Line miLine = new Line(iPtoInicial, iPtoFin);

            return miLine.GetPointAtDist(miLine.Length * 0.5);
        }


        /// <summary>
        /// La Primera Linea Se Extiende
        /// </summary>
        public static Point3d getIntPtoTwoLinesExtendLine1(Point3d iLine1Pini, Point3d iLine1Pfin, Point3d iLine2Pini, Point3d iLine2Pfin, bool iThrowErrorInterMayor1)
        {


            Polyline miLw1 = new Polyline(2);
            miLw1.AddVertexAt(0, iLine1Pini.to2d(), 0, 0, 0);
            miLw1.AddVertexAt(1, iLine1Pfin.to2d(), 0, 0, 0);

            Polyline miLw2 = new Polyline(2);
            miLw2.AddVertexAt(0, iLine2Pini.to2d(), 0, 0, 0);
            miLw2.AddVertexAt(1, iLine2Pfin.to2d(), 0, 0, 0);

            //Determino el punto de Intersección
            Point3dCollection miLstInter = new Point3dCollection();

            miLw1.IntersectWith(miLw2, Intersect.ExtendBoth, miLstInter, IntPtr.Zero, IntPtr.Zero);

            //Validaciones
            if ((miLstInter.Count == 0))
            {
                throw new oExInterseccionNull();
            }

            if (iThrowErrorInterMayor1 && miLstInter.Count > 1)
            {
                throw new oExInterseccionMayorUno();
            }

            return miLstInter[0];
        
        }



        /// <summary>
        /// Obtener Punto Interseccion ; Datos Partida
        /// Pto1 + Pto1Pendiente & Pto2 Horizontal
        /// </summary>
        public static Point3d getIntPtoTaludAndPtoHorizontal(Point3d iPtoTalud, bool iXpos, bool iYPos,double iTaludH,Point3d iPtoHor , bool iThrowErrorInterMayor1)
        {

            //Debemos de Determiar la Intersección entre ambas lineas

            //1-Linea Pto-Talud
            Point3d miP11 = iPtoTalud;
            Point3d miP12 = miP11.getFromTalud(iXpos, iYPos, iTaludH, 1);

            Polyline miLw1 = new Polyline(2);

            miLw1.AddVertexAt(0, miP11.to2d(), 0, 0, 0);
            miLw1.AddVertexAt(1, miP12.to2d(), 0, 0, 0);

            //2-Linea Pto Horizontal
            Point3d miP21 = iPtoHor;
            Point3d miP22 = miP21.getFromIncXIncY(1, 0, 0);

            Polyline miLw2 = new Polyline(2);
            miLw2.AddVertexAt(0, miP21.to2d(), 0, 0, 0);
            miLw2.AddVertexAt(1, miP22.to2d(), 0, 0, 0);


            //Determino el punto de Intersección
            Point3dCollection miLstInter = new Point3dCollection();

            miLw1.IntersectWith(miLw2, Intersect.ExtendBoth, miLstInter, IntPtr.Zero, IntPtr.Zero);


            //Validaciones
            if ((miLstInter.Count == 0))
            {
                throw new oExInterseccionNull();
            }

            if (iThrowErrorInterMayor1 && miLstInter.Count > 1)
            {
                throw new oExInterseccionMayorUno();
            }

            return miLstInter[0];

        }


        /// <summary>
        /// Obtener Punto Interseccion ; Datos Partida
        /// Pto1 + Pto1Pendiente & Pto2 Vertical
        /// </summary>
        public static Point3d getIntPtoPendienteAndPtoVertical(Point3d iPtoPendiente, double iPendienteUnoSigno, Point3d iPtoVertical, bool iThrowErrorInterMayor1)
        {

            //Debemos de Determiar la Intersección entre ambas lineas

            //1-Linea Pto-Pendiene
            Point3d miP11 = iPtoPendiente;
            Point3d miP12 = miP11.getFromLonPendiente(1, iPendienteUnoSigno);

            Polyline miLw1 = new Polyline(2);

            miLw1.AddVertexAt(0, miP11.to2d(), 0, 0, 0);
            miLw1.AddVertexAt(1, miP12.to2d(), 0, 0, 0);

            //2-Linea Pto-Talud
            Point3d miP21 = iPtoVertical;
            Point3d miP22 = miP21.getFromIncXIncY(0, 1, 0);

            Polyline miLw2 = new Polyline(2);
            miLw2.AddVertexAt(0, miP21.to2d(), 0, 0, 0);
            miLw2.AddVertexAt(1, miP22.to2d(), 0, 0, 0);

            
            //Determino el punto de Intersección
            Point3dCollection miLstInter = new Point3dCollection();

            miLw1.IntersectWith(miLw2, Intersect.ExtendBoth, miLstInter, IntPtr.Zero, IntPtr.Zero);


            //Validaciones
            if ((miLstInter.Count == 0))
            {
                throw new oExInterseccionNull();
            }

            if (iThrowErrorInterMayor1 && miLstInter.Count > 1)
            {
                throw new oExInterseccionMayorUno();
            }

            return miLstInter[0];
        
        }


        /// <summary>
        /// Obtener Punto Interseccion ; Datos Partida
        /// Pto1 + Pto1Pendiente & Pto2 + Talud
        /// </summary>
        public static Point3d getIntPtoPendienteAndPtoTalud(Point3d iPtoPendiente, double iPendienteUnoSigno, Point3d iPtoTalud, bool iXpos, bool iYPos,double iTaludH, bool iThrowErrorInterMayor1)
        { 
        
            //Debemos de Determiar la Intersección entre ambas lineas

            //1-Linea Pto-Pendiene
            Point3d miP11 = iPtoPendiente;
            Point3d miP12 = miP11.getFromLonPendiente(1, iPendienteUnoSigno);

            Polyline miLw1 = new Polyline(2);

            miLw1.AddVertexAt(0,miP11.to2d(),0,0,0);
            miLw1.AddVertexAt(1,miP12.to2d(),0,0,0);

           
           //2-Linea Pto-Talud
            Point3d miP21 = iPtoTalud;
            Point3d miP22 = miP21.getFromTalud(iXpos, iYPos, iTaludH, 1);

            Polyline miLw2 = new Polyline(2);
            miLw2.AddVertexAt(0,miP21.to2d(),0,0,0);
            miLw2.AddVertexAt(1,miP22.to2d(),0,0,0);

           
            //Determino el punto de Intersección
            Point3dCollection miLstInter= new Point3dCollection();

            miLw1.IntersectWith(miLw2,Intersect.ExtendBoth,miLstInter,IntPtr.Zero,IntPtr.Zero);


            //Validaciones
            if ((miLstInter.Count == 0))
            {
                throw new oExInterseccionNull();
            }

            if (iThrowErrorInterMayor1 && miLstInter.Count > 1)
            {
                throw new oExInterseccionMayorUno();
            }

            return miLstInter[0];
        }

        public static Point3d getFromTalud(this Point3d iBase,bool iXpos, bool iYpos, double iTalud, double iAltura)
        {
            double miX = iAltura * iTalud;
            double miY = iAltura;

            if (iXpos && iYpos)
            {
                return new Point3d(iBase.X + miX, iBase.Y + miY, 0);
            }
            else if (iXpos && !iYpos)
            {
                return new Point3d(iBase.X + miX, iBase.Y - miY, 0);
            }
            else if (!iXpos && iYpos)
            {
                return new Point3d(iBase.X - miX, iBase.Y + miY, 0);
            }
            else if (!iXpos && !iYpos)
            {
                return new Point3d(iBase.X - miX, iBase.Y - miY, 0);
            }
            else
            {
                throw new Exception(strError.eCasoNoContemplado);
            }
         


        }


        public static Point3d getFromLonPendiente(this Point3d iBase, double iLon, double iPendienteUnoSigno)
        {

            double miX = iBase.X + iLon; ;
            double miY = iBase.Y + iPendienteUnoSigno * iLon;
            return new Point3d(miX, miY, 0);
        }

 
        /// <param name="iBase">Pto Base</param>
        /// <param name="iIncX">Inc X Signo</param>
        /// <param name="iIncY">Inc Y Signo</param>
        public  static Point3d getFromIncXIncY (this Point3d iBase, double iIncX, double iIncY, double iIncZ)
        {
            return new Point3d(iBase.X + iIncX, iBase.Y + iIncY,iBase.Z+iIncZ);
        }


        public static Point3d getPtoMasCercano(this Point3d iPtoOrigen, Point3d iP1, Point3d iP2)
        { 
        
            Point3dCollection miCol = new Point3dCollection();
            miCol.Add(iP1);
            miCol.Add(iP2);

            return getPtoMasCercano(iPtoOrigen, miCol);
        
        
        
        }


        public static Point3d getPtoMasCercano(this Point3d iPtoOrigen, Point3dCollection iPtoCol)
        {


            Dictionary<Point3d, double> miDicDis = new Dictionary<Point3d, double>();


            foreach (Point3d miPto in iPtoCol)
            {
                miDicDis.Add(miPto, iPtoOrigen.DistanceTo(miPto));
            }


            //Linq Realizo la Consulta

            var myQuery = from p in miDicDis
                          orderby p.Value ascending
                          select p;

            return myQuery.First().Key;

        }
        public static Point3d getPtoMasLejano(this Point3d iPtoOrigen, Point3dCollection iPtoCol)
        {


            Dictionary<Point3d, double> miDicDis = new Dictionary<Point3d, double>();


            foreach (Point3d miPto in iPtoCol)
            {
                miDicDis.Add(miPto, iPtoOrigen.DistanceTo(miPto));
            }


            //Linq Realizo la Consulta

            var myQuery = from p in miDicDis
                          orderby p.Value ascending
                          select p;

            return myQuery.Last().Key;

        }


        public static Point3d getPtoYmayorInferior (this Point3d iPtoOrigen, Point3dCollection iPtoCollection)
        {

            List<Point3d> miLstPto = iPtoCollection.convertToList();

            var miQuery = from p in miLstPto
                          where p.Y > iPtoOrigen.Y
                          orderby p.Y ascending
                          select p;

            if (miQuery.Any())
            {
                return miQuery.First();
            }
            else
            {
                return miLstPto[0];
            }

        }


        public static Point3d getPtoYmenorSuperior(this Point3d iPtoOrigen, Point3dCollection iPtoCollection)
        {

            List<Point3d> miLstPto = iPtoCollection.convertToList();

            var miQuery = from p in miLstPto
                          where p.Y < iPtoOrigen.Y
                          orderby p.Y descending
                          select p;

            if (miQuery.Any())
            {
                return miQuery.First();
            }
            else
            {
                return miLstPto[0];
            }

        }


        public static List<Point3d> convertToList (this Point3dCollection iPtoCollection)
        {

            List<Point3d> miLstPto = new List<Point3d>();

            foreach (Point3d item in iPtoCollection)
            {
                miLstPto.Add(item);
            }


            return miLstPto;

        }



        public static Point2d to2d(this Point3d iBase)
        {
            return iBase.Convert2d(new Plane());
        
        }

        /// <summary>
        /// Obtener la Pendiente entre 2 Puntos [%]
        /// </summary>
        public static double getPendiente2DPorCientoConSigno(this Point2d iOrigen, Point2d iDestino)
        {

            double miLon = iOrigen.GetDistanceTo(iDestino);

            double miInc = iDestino.Y - iOrigen.Y;

            return (miInc / miLon) * 100;

        }


        public static Point3d convertTo3D(this Point2d iPto2D)
        {
            return new Point3d(iPto2D.X, iPto2D.Y, 0);
        
        
        }

        /// <summary>
        /// Convertir Punto 3d a 3d con Z=0
        /// </summary>
        public static Point3d convertToZ0(this Point3d iPto3D)
        {
            return new Point3d(iPto3D.X, iPto3D.Y, 0);
        }


        public static bool hasPointYmayor(this Point3d iPtoOrigen, Point3dCollection iPtoCollection)
        {

            List<Point3d> miLstPto = iPtoCollection.convertToList();

            var miQuery = from p in miLstPto
                          where p.Y > iPtoOrigen.Y
                          select p;


            if (miQuery == null || miQuery.Count().Equals(0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static bool hasPointYmenor(this Point3d iPtoOrigen, Point3dCollection iPtoCollection)
        {

            List<Point3d> miLstPto = iPtoCollection.convertToList();

            var miQuery = from p in miLstPto
                          where p.Y < iPtoOrigen.Y
                          select p;


            if (miQuery == null || miQuery.Count().Equals(0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // HACK JUAN
        /// <summary>
        /// Obtener el Punto mas cercano a un origen que ademas pertence a una polilinea
        /// </summary>
        public static Point3d getPtoMasCercanoAndOnPolyline(this Point3d iPtoOrigen, Point3dCollection iPtoCol, Polyline iLwPtoOn)
        {

            Dictionary<Point3d, double> miDicDis = new Dictionary<Point3d, double>();


            foreach (Point3d miPto in iPtoCol)
            {

                if (oLw.isPtoOnLw(iLwPtoOn, miPto))
                {
                    miDicDis.Add(miPto, iPtoOrigen.DistanceTo(miPto));
                }

            }


            //Linq Realizo la Consulta

            var miQuery = from p in miDicDis
                          orderby p.Value ascending
                          select p;


            if (miQuery == null || miQuery.Count() == 0)
            {
                throw new oExInterseccionNull();
            }

            return miQuery.First().Key;
        }

    }
}
