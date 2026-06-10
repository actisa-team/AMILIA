using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.Secciones
{

    using engNet.Extension.Double;
    using engCadNet;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;
    
    
    public class oSeccionGuitarra
    {

        double mPkElevacion;

     
        #region "Constructor"

        

        /// <summary>
        /// Constructor Seccion Puente-Tuneles
        /// </summary>
        public oSeccionGuitarra(Point2d iPtoInsert,double iPkElevacion, double iAnchoTotal, double iAlto)
        {

            this.ptoInserccion = iPtoInsert;
            this.mPkElevacion = iPkElevacion;
            this.dimensiones = new oDimensionesGuitarra(iAnchoTotal / 2.0, iAnchoTotal / 2.0, this.mPkElevacion + 10, this.mPkElevacion - 10);
            this.matrizViewToWorld = getMatrizViewToWorld(this.ptoInserccion);
            this.matrizRoadDerecha = getMatrizRoadDerecha(this.mPkElevacion, this.ptoInserccion);
            this.matrizRoadIzquierda = getMatrizRoadIzquierda(this.mPkElevacion, this.ptoInserccion);

        }

        /// <summary>
        /// Constructor Seccion Calzada
        /// </summary>
        public oSeccionGuitarra(Point2d iPtoInsert, 
                              double iPkElevacion, 
                              double iDesmonteAltura,
                              double iDesmonteTalud,
                              double iTerraplenAltura, 
                              double iTerraplenTalud,
                              double iAnchoPlataformaTotal)

                              
                              
        {

            this.ptoInserccion = iPtoInsert;
            this.mPkElevacion = iPkElevacion;
            this.dimensiones = this.getDimensiones(iPkElevacion, iDesmonteAltura.getMayor(iTerraplenAltura), iDesmonteTalud.getMayor(iTerraplenTalud),iAnchoPlataformaTotal);
            this.matrizViewToWorld = getMatrizViewToWorld(this.ptoInserccion);
            this.matrizRoadDerecha = getMatrizRoadDerecha( this.mPkElevacion, this.ptoInserccion);
            this.matrizRoadIzquierda = getMatrizRoadIzquierda( this.mPkElevacion, this.ptoInserccion);
        }


        #endregion

        #region "Propiedades"

        public Point2d ptoInserccion { get; private set; }

        public oDimensionesGuitarra dimensiones { get; set; }

        public Matrix3d matrizViewToWorld { get; private set; }

        public Matrix3d matrizRoadDerecha { get; private set; }

        public Matrix3d matrizRoadIzquierda { get; private set; }

        #endregion
        #region "Metodos Publicos"

        public void updateSistemaReferencia()
        {
            this.matrizViewToWorld = getMatrizViewToWorld(this.ptoInserccion);
            this.matrizRoadDerecha = getMatrizRoadDerecha(this.mPkElevacion, this.ptoInserccion);
            this.matrizRoadIzquierda = getMatrizRoadIzquierda(this.mPkElevacion, this.ptoInserccion);
        }


        public void draw(string iLayer)
        {
            using (Transaction tr = oCadManager.StartTransaction())
            {

                double myTxtH = 2.5;
                double myOffSet = 2.5;

                //Dibujo el Formato
                Line myLineX = oLine.addLine2d(0, 0, dimensiones.ancho, 0, iLayer);

                (tr.GetObject(myLineX.ObjectId, OpenMode.ForWrite) as Line).TransformBy(matrizViewToWorld);

                Line myLineY1 = oLine.addLine2d(0, 0, 0, dimensiones.alto, iLayer);
                (tr.GetObject(myLineY1.ObjectId, OpenMode.ForWrite) as Line).TransformBy(matrizViewToWorld);

                Line myLineY2 = oLine.addLine2d(dimensiones.ancho, 0, dimensiones.ancho, dimensiones.alto, iLayer);
                (tr.GetObject(myLineY2.ObjectId, OpenMode.ForWrite) as Line).TransformBy(matrizViewToWorld);


                //Añado los Textos Eje Y
                oText.addText2DMatrix(dimensiones.elevacionMin.ToString(), -myOffSet, 0, myTxtH, 0, iLayer, TextHorizontalMode.TextRight, TextVerticalMode.TextVerticalMid, matrizViewToWorld);
                oText.addText2DMatrix(dimensiones.elevacionMax.ToString(), -myOffSet, dimensiones.alto, myTxtH, 0, iLayer, TextHorizontalMode.TextRight, TextVerticalMode.TextVerticalMid, matrizViewToWorld);

                //Añado los Textos Eje X
                oText.addText2DMatrix(dimensiones.anchoIzq.ToString(), 0, -myOffSet, myTxtH, 0, iLayer, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, matrizViewToWorld);
                oText.addText2DMatrix("0", dimensiones.anchoIzq, -myOffSet, myTxtH, 0, iLayer, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, matrizViewToWorld);
                oText.addText2DMatrix(dimensiones.anchoDer.ToString(), dimensiones.ancho, -myOffSet, myTxtH, 0, iLayer, TextHorizontalMode.TextCenter, TextVerticalMode.TextTop, matrizViewToWorld);


                tr.Commit();

            }
        }
        public void draw (string iLayer, Matrix3d iMatrizViewToWorld)
        {

 

        }
        #endregion
        #region "Metodos Privados"
        private oDimensionesGuitarra getDimensiones(double iElevacionPk, double iTerraplenDesmonteMax, double iTaludHMax, double iAnchoPlataformaTotal)
        {
            oDimensionesGuitarra miDimensiones = new oDimensionesGuitarra();

            double miAnchoPlataformaMitad = 0.5 * iAnchoPlataformaTotal;

           /* miDimensiones.anchoIzq = ((iTerraplenDesmonteMax * iTaludHMax) + (miAnchoPlataformaMitad)).roundOffTen();
            miDimensiones.anchoDer = ((iTerraplenDesmonteMax * iTaludHMax) + (miAnchoPlataformaMitad)).roundOffTen();*/
            miDimensiones.anchoIzq = (5+(miAnchoPlataformaMitad)).roundOffOne();
            miDimensiones.anchoDer = (5+(miAnchoPlataformaMitad)).roundOffOne();
            /*miDimensiones.elevacionMin = (iElevacionPk - (iTerraplenDesmonteMax)).roundOffTen();
            miDimensiones.elevacionMax = (iElevacionPk + (iTerraplenDesmonteMax)).roundOffTen();*/
            miDimensiones.elevacionMin = (iElevacionPk - 20).roundOffOne();
            miDimensiones.elevacionMax = (iElevacionPk + 20).roundOffOne();

            return miDimensiones;
        }
        private Matrix3d getMatrizRoadDerecha(double iPkElevation, Point2d iPtoInserccion)
        {
            //Road Derecha
            Matrix3d miMatrixRoadDer = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                                                                  Vector3d.XAxis,
                                                                  Vector3d.YAxis,
                                                                  Vector3d.ZAxis,
                                                                  new Point3d(iPtoInserccion.X + dimensiones.anchoIzq, iPkElevation - dimensiones.elevacionMin + iPtoInserccion.Y, 0),
                                                                  Vector3d.XAxis,
                                                                  Vector3d.YAxis,
                                                                  Vector3d.ZAxis);


            return miMatrixRoadDer;


        }
        private Matrix3d getMatrizRoadIzquierda (double iPkElevation, Point2d iPtoInsert)
        {

            ////ROAD IZQUIERDA
          Matrix3d  miMatrixRoadIzq = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                                                              -Vector3d.XAxis,
                                                               Vector3d.YAxis,
                                                               Vector3d.ZAxis,
                                                               new Point3d(iPtoInsert.X + dimensiones.anchoIzq, iPkElevation - dimensiones.elevacionMin + iPtoInsert.Y, 0),
                                                               Vector3d.XAxis,
                                                               Vector3d.YAxis,
                                                               Vector3d.ZAxis);


          return miMatrixRoadIzq;




        }
        private Matrix3d getMatrizViewToWorld(Point2d iPtoInserccion)
        {
            //View To WORLD
            Matrix3d miMatrixViewToWorld = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                                                                 Vector3d.XAxis,
                                                                 Vector3d.YAxis,
                                                                 Vector3d.ZAxis,
                                                                 new Point3d(iPtoInserccion.X, iPtoInserccion.Y, 0),
                                                                 Vector3d.XAxis, Vector3d.YAxis,
                                                                 Vector3d.ZAxis);


            return miMatrixViewToWorld;



        }
        #endregion

    }
    public class oDimensionesGuitarra
    {
        #region "Constructor"

        public oDimensionesGuitarra()
        {

        }


        public oDimensionesGuitarra(double iAnchoDer, double iAnchoIzq, double iElevMax, double iElevMin)
        {
            anchoDer = iAnchoDer.roundOffTenHighest();
            anchoIzq = iAnchoIzq.roundOffTenHighest();
            elevacionMax = iElevMax.roundOffTenHighest();
            elevacionMin = iElevMin.roundOffTen();
        }

        #endregion
        #region "Propiedades"


        public double elevacionMax { get; set; }
        public double elevacionMin { get; set; }
        public double anchoDer { get; set; }
        public double anchoIzq { get; set; }

        public double ancho
        {
            get
            {
                return anchoIzq + anchoDer;
            }
        }
        public double alto
        {

            get
            {
                return elevacionMax - elevacionMin;


            }

        }
        #endregion

        #region "Metodos"

        public void addAnchoDerAndIzq (double iAnchoToSumar)
        {
            this.anchoDer = this.anchoDer + iAnchoToSumar;
            this.anchoIzq = this.anchoIzq + iAnchoToSumar;
        }



        #endregion
    }
}
