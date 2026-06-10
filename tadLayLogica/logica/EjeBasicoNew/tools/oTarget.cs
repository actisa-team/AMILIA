using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{


    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;
    using tadLayShare;
    using tadLayShare.puntos;
    using tadLayLan;
    

 
    public class oTarget
    {
        #region "Enumeraciones"
        public enum eFiltro
        {
            Max,
            Min,
            MaxMin,
        }
        #endregion
        #region "Fields Privados"

        private Point2d mPtoIni;
        private Point2d mPtoFin;
        private Point2d? mPtoEntronque = null;


        /// <summary>
        /// Listado con todos los SCU Puntos Target Ordenados
        /// </summary>
        private List<oP2d> mLstPtoSCU;

        /// <summary>
        /// Listado con los Puntos SCW Target Ordenados
        /// </summary>
        private List<oP2d> mLstPtoSCW;

        private Matrix2d mMatrixScwToScu;
        private Matrix2d mMatrixScuToScw;

        private double? mToleranciaPtoTargetDistancia = null;
        Polyline mLwEjeBasico2D = null;

        private int mLastIndex = 0;


        #endregion
        #region "Constructor"
        public oTarget(oP2d iPtoIni, oP2d iPtoFin, double iAijMinimo, double iToleranciaObjetivoPC)
        {
            mPtoIni = new Point2d(iPtoIni.X, iPtoIni.Y);
            mPtoFin = new Point2d(iPtoFin.X, iPtoFin.Y);

            mToleranciaPtoTargetDistancia = iAijMinimo * ((100 +iToleranciaObjetivoPC) / 100);      
              
            getMatrix();
        }
        #endregion
        #region "Propiedades"
        public bool HasEnvolvente
        {

            get
            {
                if (mLstPtoSCU.Count > 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }          
            }
        
        
        }
        #endregion
        #region "Metodos Publicos"

        public double toleranciaSigPunto
        {
            set
            {
                mToleranciaPtoTargetDistancia = value;
            }
        }


        /// <summary>
        /// Configurar los Puntos Target partiendo de un Eje Basico y la Envolvente Definida
        /// </summary>
        public void setLstPtoTargetFromEjeBasico_EnvolventeMaximaMinima(Polyline iLwEjeBasico2D, eFiltro iEnvolvente)
        {


            mLwEjeBasico2D = iLwEjeBasico2D;
            //List Pto CAD del Eje Basico
            List<Point3d> miLstPtoLwCAD = engCadNet.oLw.getLstPto(iLwEjeBasico2D);


            //Cast Pto Tadil
            List<oP2d> miLstTargetScw = miLstPtoLwCAD.ConvertAll(p => new oP2d(p.X, p.Y));
                
            //Obtengo la Lista de puntos Target en SCU y SCW segun la Envolvente
            fillLstPtoScuScw(miLstTargetScw,iEnvolvente);

            //Determino si la envolvente Existe
            if (this.HasEnvolvente)
            {
                //Creo la Envolvente Max o Min
                List<oP2d> miEnvolventePos = oConvexHull.getLstoP2dConvexHull(mLstPtoSCW);

                //Creo el Listado de Target
                fillLstPtoScuScw(miEnvolventePos, iEnvolvente);
            }
        }
        /// <summary>
        /// Configurar los Puntos Target para el Eje de Visibilidad
        /// </summary>
        public void setLstPtoTargetFromEjeVisibilidad_EnvolventeMaxMin(Polyline iEjeVisibilidad)
        {

            mLwEjeBasico2D = iEjeVisibilidad;
            //Listado Vertices EjeVisibilidad
            List<Point3d> miLstPtoCad = engCadNet.oLw.getLstPto(iEjeVisibilidad);

            //Cast Point3d --> oP2d
            List<oP2d> miLstPtoTadil =  miLstPtoCad.ConvertAll(p=> new oP2d(p.X,p.Y));   

            fillLstPtoScuScw(miLstPtoTadil,eFiltro.MaxMin);

        }

        public void setPuntoDeEntronque(oP2d iPuntoEntronque)
        {
            mPtoEntronque = new Point2d(iPuntoEntronque.X, iPuntoEntronque.Y);
        }


        public bool isConfiguredPtoEntronque()
        {
            return mPtoEntronque != null;
        }


        //Obtengo el Punto Target, respecto al punto de origen de un abanico
        private oP2d getPtoTargetScw(oP2d iPtoOrigenAbanico)
        {

            if (mLstPtoSCU != null && mLstPtoSCU.Count > 0)
            {

                //Cast del oP2d to point2d
                Point2d myPtoOrigenScw = getCastPto2dObjToCad(iPtoOrigenAbanico);

                //Obtengo el Punto en el SCU
                Point2d myPtoOrigenAbanicoScu = myPtoOrigenScw.TransformBy(mMatrixScwToScu);


                //Consulto el mas cercano
                oP2d miPtoTarget = (from p in mLstPtoSCU
                                    where p.X > myPtoOrigenAbanicoScu.X + mToleranciaPtoTargetDistancia.Value
                                    orderby p.X ascending
                                    select p).FirstOrDefault();

                //Punto Target en Coordenas SCU

                if (miPtoTarget == null)
                {
                    return getCastPto2dCadToObj(mPtoFin);
                }
                else
                {
                    //Obtengo el Primer Punto del Listado
                    Point2d myPtoTargetScu = getCastPto2dObjToCad(miPtoTarget);
                    //Debo de obtener las coordenadas en SCW
                    Point2d myPtoTargetScw = myPtoTargetScu.TransformBy(mMatrixScuToScw);
                    return getCastPto2dCadToObj(myPtoTargetScw);
                }
            }
            else
            {
                return getCastPto2dCadToObj(mPtoFin);
            }


        }

        public oP2d getPtoTarget(oP2d iPtoOrigenAbanico, bool isEnvolvente)
        {
            oP2d miPuntoTarget = new oP2d();
            if (isEnvolvente)
            {
                miPuntoTarget = getPtoTargetScw(iPtoOrigenAbanico);
            }
            else
            {
                miPuntoTarget = getPtoTargetSecuencial(iPtoOrigenAbanico);
            }

            return miPuntoTarget;
        }
 



        //Obtengo el punto Target secuencial. 
        private oP2d getPtoTargetSecuencial(oP2d iPtoOrigenAbanico)
        {
            Point3d miPuntoOrigenAbanico = new Point3d(iPtoOrigenAbanico.X, iPtoOrigenAbanico.Y, 0);
            oP2d miTarget = new oP2d();

            double? miPk = mLwEjeBasico2D.GetDistAtPoint(mLwEjeBasico2D.getPointMasCercano(miPuntoOrigenAbanico));
            double? miLastPk = oLw.getPkAtPoint(new Point3d(mLwEjeBasico2D.GetPoint2dAt(mLwEjeBasico2D.NumberOfVertices - 2).X, mLwEjeBasico2D.GetPoint2dAt(mLwEjeBasico2D.NumberOfVertices - 2).Y, 0), mLwEjeBasico2D);

            if (miPk >= miLastPk)
            {

                if (isConfiguredPtoEntronque())
                {
                    Point2d miPuntoEntronque = (Point2d)mPtoEntronque;
                    miTarget.X = miPuntoEntronque.X;
                    miTarget.Y = miPuntoEntronque.Y;
                }
                else
                {
                    miTarget.X = mLwEjeBasico2D.GetPoint2dAt(mLwEjeBasico2D.NumberOfVertices - 1).X;
                    miTarget.Y = mLwEjeBasico2D.GetPoint2dAt(mLwEjeBasico2D.NumberOfVertices - 1).Y;
                }
            }
            else
            {
                int i = 0;
                double? miPkSalida = mLwEjeBasico2D.GetDistAtPoint(new Point3d(mLwEjeBasico2D.GetPoint2dAt(i).X, mLwEjeBasico2D.GetPoint2dAt(i).Y, 0));
                while (miPk >= miPkSalida && i < mLwEjeBasico2D.NumberOfVertices)
                {
                    i++;
                    miPkSalida = mLwEjeBasico2D.GetDistAtPoint(new Point3d(mLwEjeBasico2D.GetPoint2dAt(i).X, mLwEjeBasico2D.GetPoint2dAt(i).Y, 0));
                }

                double? miPkEntrada = mLwEjeBasico2D.GetDistAtPoint(new Point3d(mLwEjeBasico2D.GetPoint2dAt(i - 1).X, mLwEjeBasico2D.GetPoint2dAt(i - 1).Y, 0));

                double miDistToTarget = (double)miPkSalida - (double)miPk;

                double TantoX1Recorrido = ((double)miPk - (double)miPkEntrada) / ((double)miPkSalida - (double)miPkEntrada);

                if (miDistToTarget < mToleranciaPtoTargetDistancia)
                {
                    if (i < mLwEjeBasico2D.NumberOfVertices - 1)
                    {
                        if ((i + 1) >= mLastIndex)
                        {
                            miTarget.X = mLwEjeBasico2D.GetPoint2dAt(i + 1).X;
                            miTarget.Y = mLwEjeBasico2D.GetPoint2dAt(i + 1).Y;
                            mLastIndex = i + 1;
                        }
                        else
                        {
                            miTarget.X = mLwEjeBasico2D.GetPoint2dAt(mLastIndex).X;
                            miTarget.Y = mLwEjeBasico2D.GetPoint2dAt(mLastIndex).Y;
                        }
                    }
                    else
                    {
                                miTarget.X = mLwEjeBasico2D.GetPoint2dAt(mLastIndex).X;
                                miTarget.Y = mLwEjeBasico2D.GetPoint2dAt(mLastIndex).Y;
                           
                    }
                }
                else
                {
                    if (i < mLwEjeBasico2D.NumberOfVertices - 1)
                    {
                        if (i >= mLastIndex)
                        {
                            miTarget.X = mLwEjeBasico2D.GetPoint2dAt(i).X;
                            miTarget.Y = mLwEjeBasico2D.GetPoint2dAt(i).Y;
                            mLastIndex = i;
                        }
                        else
                        {
                            miTarget.X = mLwEjeBasico2D.GetPoint2dAt(mLastIndex).X;
                            miTarget.Y = mLwEjeBasico2D.GetPoint2dAt(mLastIndex).Y;
                        }
                    }
                    else
                    {
                            miTarget.X = mLwEjeBasico2D.GetPoint2dAt(mLastIndex).X;
                            miTarget.Y = mLwEjeBasico2D.GetPoint2dAt(mLastIndex).Y;
                        
                    }
                }
            }
            return miTarget;
        }

        #endregion
        #region "Metodos Privados"
        private void getMatrix()
        {

            //Creo la Linea Une Inicio-Fin
            //Es mi sistema de referencia
            Line2d myLineIniFin = new Line2d(mPtoIni, mPtoFin);

            double myX = myLineIniFin.Direction.X;
            double myY = myLineIniFin.Direction.Y;


            mMatrixScwToScu = Matrix2d.AlignCoordinateSystem
                                         (
                                          mPtoIni,
                                          new Vector2d(myX, myY),
                                          new Vector2d(-myY, myX),
                                          Point2d.Origin,
                                          Vector2d.XAxis,
                                          Vector2d.YAxis

                                          );

            mMatrixScuToScw = Matrix2d.AlignCoordinateSystem
                                         (
                                          Point2d.Origin,
                                          Vector2d.XAxis,
                                          Vector2d.YAxis,
                                          mPtoIni,
                                          new Vector2d(myX, myY),
                                          new Vector2d(-myY, myX)
                                          );

        }
        private void fillLstPtoScuScw(List<oP2d> iLstPtoTargetScw, eFiltro iFiltro)
        {
           
            Point2d myPtoSCU;
            mLstPtoSCU = new List<oP2d>();
            mLstPtoSCW = new List<oP2d>();



            //Añado la Colección de Puntos
            if (iLstPtoTargetScw != null && iLstPtoTargetScw.Count > 0)
            {

                foreach (oP2d myPto in iLstPtoTargetScw)
                {

                    myPtoSCU = getCastPto2dObjToCad(myPto);
                    
                    myPtoSCU = myPtoSCU.TransformBy(mMatrixScwToScu);

                    mLstPtoSCU.Add(getCastPto2dCadToObj(myPtoSCU));
                }

            }


            

            //Creo la Envolvente en Funcion de los datos de Entrada
            switch (iFiltro)
            {
                case eFiltro.Max:

                 var  myQuery = from p in mLstPtoSCU
                              where p.Y >= 0
                              orderby p.X ascending
                              select p;


                   mLstPtoSCU=  myQuery.ToList<oP2d>();
                   break;

                case eFiltro.Min:

                  var  myQuery1 = from p in mLstPtoSCU
                              where p.Y <= 0
                              orderby p.X ascending
                              select p;


                    mLstPtoSCU= myQuery1.ToList<oP2d>();
                    break;
                   
                case eFiltro.MaxMin:

                             var myQuery2 = from p in mLstPtoSCU
                              orderby p.X ascending
                              select p;

                    mLstPtoSCU= myQuery2.ToList<oP2d>();
                    break;

                default:

                    throw new Exception(string.Format(strError.eOpcionNoConfig, iFiltro.ToString()));
            }


            //Genero  el LstEnCoordenadas SCW

            foreach (oP2d myForPtoScu in mLstPtoSCU)
            {
                
                Point2d myPtoCad = getCastPto2dObjToCad(myForPtoScu);
                myPtoCad = myPtoCad.TransformBy(mMatrixScuToScw);

                mLstPtoSCW.Add(getCastPto2dCadToObj(myPtoCad));
            }

                     

           


        }
        private Point2d getTranslateScuFromScw(Point2d iPtoScu)
        {
            return iPtoScu.TransformBy(mMatrixScuToScw);
        }
        private oP2d getCastPto2dCadToObj(Point2d iPto2dCad)
        {
            return new oP2d(iPto2dCad.X, iPto2dCad.Y);
        }
        private Point2d getCastPto2dObjToCad(oP2d iPto2dObj)
        {
            return new Point2d(iPto2dObj.X, iPto2dObj.Y);

        }

        //private double getDistFromOrig(Polyline2d iPol2d, int i)
        //{
        //    double miDist=0;
        //    if (i > 0)
        //    {
        //        for (int j = 0; j <= i; j++)
        //        {
        //            miDist = miDist + iPol2d.get
        //        }
        //    }
        //}
        #endregion
    }
}
