//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{


//    using Autodesk.AutoCAD.ApplicationServices;
//    using Autodesk.AutoCAD.DatabaseServices;
//    using Autodesk.AutoCAD.EditorInput;
//    using Autodesk.AutoCAD.Geometry;

//    using engCadNet;
//    using tadLayShare;
//    using tadLayShare.puntoOld;
    

//    /// <summary>
//    /// FUNCIONALIDADES
//    /// 
//    /// </summary>
    
//    public class oTargetOld
//    {

//        public enum eFiltro
//        {
//            Max,
//            Min,
//            MaxMin,
//        }


//        private Point2d mPtoIni;
//        private Point2d mPtoFin;

//        /// <summary>
//        /// Listado con todos los SCU Puntos Target Ordenados
//        /// </summary>
//        private List<oP2d> mLstPtoSCU;

//        /// <summary>
//        /// Listado con los Puntos SCW Target Ordenados
//        /// </summary>
//        private List<oP2d> mLstPtoSCW;

//        private Matrix2d mMatrixScwToScu;
//        private Matrix2d mMatrixScuToScw;

//        #region "Constructor"
//        public oTargetOld(oP2d iPtoIni, oP2d iPtoFin)
//        {
//            mPtoIni = new Point2d(iPtoIni.X.Value, iPtoIni.Y.Value);
//            mPtoFin = new Point2d(iPtoFin.X.Value, iPtoFin.Y.Value);   
    
//            getMatrix();
//        }
//        #endregion
//        #region "Propiedades"


//        public List<oP2d> getLstPtoTargetScw
//        {
//            get
//            {
//                return mLstPtoSCW;
//            }
//        }

//        public bool HasEnvolvente
//        {

//            get
//            {
//                if (mLstPtoSCU.Count > 2)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }          
//            }
        
        
//        }


//        #endregion
//        #region "Metodos"


//        /// <summary>
//        /// Listado de Puntos Target desde una Polilinea
//        /// </summary>
//        /// <param name="iPol3d">Polilinea 3D</param>
//        /// <param name="iFiltro">Filtro Valores</param>
//        public void setListPtoTargetFromLw3dScw(Polyline3d iPol3d,eFiltro iFiltro)
//        {
//            List<oP2d> myLstTargetScw = new List<oP2d>();

//            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
//            {
//                using (Transaction tr = oCadManager.StartTransaction())
//                {

//                    foreach (ObjectId myObjId in iPol3d)
//                    {

//                        PolylineVertex3d myVertice = (PolylineVertex3d)tr.GetObject(myObjId, OpenMode.ForRead);

//                        myLstTargetScw.Add(new oP2d(myVertice.Position.X, myVertice.Position.Y));
//                    }

//                }
//            }


//            fillLstPtoScuScw(myLstTargetScw,iFiltro);
//        }

//        /// <summary>
//        /// Listado de Puntos Target desde la Solución Primaria
//        /// </summary>
//        /// <param name="iLstPtoCaminoScw">Listado de Puntos Solución</param>
//        /// <param name="iFiltro">Filtro de Valores Y</param>
//        public void setListPtoTargetFromPathScw(List<oP3d> iLstPtoCaminoScw, eFiltro iFiltro)
//        {


//            List<oP2d> myLstTargetScw = new List<oP2d>();
            

//            foreach (oP3d myPtoObj2d in iLstPtoCaminoScw)
//            {
//                myLstTargetScw.Add(new oP2d(myPtoObj2d.X.Value,myPtoObj2d.Y.Value));
//            }



//            fillLstPtoScuScw(myLstTargetScw, iFiltro);
              
//        }

//        /// <summary>
//        /// Listado de Puntos Target desde la Solución Primaria
//        /// </summary>
//        /// <param name="iLstPtoCaminoScw">Listado de Puntos Solución</param>
//        /// <param name="iFiltro">Filtro de Valores Y</param>
//        public void setListPtoTargetFromPathScw(List<oP2d> iLstPtoCaminoScw, eFiltro iFiltro)
//        {

//            fillLstPtoScuScw(iLstPtoCaminoScw, iFiltro);

//        }



//        //Obtengo el Punto Target, respecto al punto de origen de un abanico
//        public oP2d getPtoTargetScw(oP2d iPtoOrigenAbanico, double iAmin, double iPorcentajeDist)
//        {

//            if (mLstPtoSCU != null && mLstPtoSCU.Count > 0)
//            {

//                //Cast del oP2d to point2d
//                Point2d myPtoOrigenScw = getCastPto2dObjToCad(iPtoOrigenAbanico);

//                //Obtengo el Punto en el SCU
//                Point2d myPtoOrigenAbanicoScu = myPtoOrigenScw.TransformBy(mMatrixScwToScu);

//                //Obtengo la Tolerancia
//                double myTolerancia = iAmin * (iPorcentajeDist / 100);


//                //Consulto el mas cercano
//                var myQuery = from p in mLstPtoSCU
//                              where p.X > myPtoOrigenAbanicoScu.X + myTolerancia
//                              orderby p.X ascending
//                              select p;

//                //Punto Target en Coordenas SCU

//                if (myQuery.Count() == 0)
//                {
//                    return getCastPto2dCadToObj(mPtoFin);
//                }
//                else
//                {
//                    //Obtengo el Primer Punto del Listado
//                    Point2d myPtoTargetScu = getCastPto2dObjToCad((oP2d)myQuery.First().XY);
//                    //Debo de obtener las coordenadas en SCW
//                    Point2d myPtoTargetScw = myPtoTargetScu.TransformBy(mMatrixScuToScw);
//                    return getCastPto2dCadToObj(myPtoTargetScw);
//                }
//            }
//            else
//            {
//                return getCastPto2dCadToObj(mPtoFin);
//            }


//        }

//        #endregion
//        #region "Metodos Privados"
//        private void getMatrix()
//        {

//            //Creo la Linea Une Inicio-Fin
//            //Es mi sistema de referencia
//            Line2d myLineIniFin = new Line2d(mPtoIni, mPtoFin);

//            double myX = myLineIniFin.Direction.X;
//            double myY = myLineIniFin.Direction.Y;


//            mMatrixScwToScu = Matrix2d.AlignCoordinateSystem
//                                         (
//                                          mPtoIni,
//                                          new Vector2d(myX, myY),
//                                          new Vector2d(-myY, myX),
//                                          Point2d.Origin,
//                                          Vector2d.XAxis,
//                                          Vector2d.YAxis

//                                          );

//            mMatrixScuToScw = Matrix2d.AlignCoordinateSystem
//                                         (
//                                          Point2d.Origin,
//                                          Vector2d.XAxis,
//                                          Vector2d.YAxis,
//                                          mPtoIni,
//                                          new Vector2d(myX, myY),
//                                          new Vector2d(-myY, myX)
//                                          );

//        }
//        private void fillLstPtoScuScw(List<oP2d> iLstPtoTargetScw, eFiltro iFiltro)
//        {
           
//            Point2d myPtoSCU;
//            mLstPtoSCU = new List<oP2d>();
//            mLstPtoSCW = new List<oP2d>();



//            //Añado la Colección de Puntos
//            if (iLstPtoTargetScw != null && iLstPtoTargetScw.Count > 0)
//            {

//                foreach (oP2d myPto in iLstPtoTargetScw)
//                {

//                    myPtoSCU = getCastPto2dObjToCad(myPto);
                    
//                    myPtoSCU = myPtoSCU.TransformBy(mMatrixScwToScu);

//                    mLstPtoSCU.Add(getCastPto2dCadToObj(myPtoSCU));
//                }

//            }


            

//            //Creo la Envolvente en Funcion de los datos de Entrada
//            switch (iFiltro)
//            {
//                case eFiltro.Max:

//                 var  myQuery = from p in mLstPtoSCU
//                              where p.Y >= 0
//                              orderby p.X ascending
//                              select p;


//                   mLstPtoSCU=  myQuery.ToList<oP2d>();
//                   break;

//                case eFiltro.Min:

//                  var  myQuery1 = from p in mLstPtoSCU
//                              where p.Y <= 0
//                              orderby p.X ascending
//                              select p;


//                    mLstPtoSCU= myQuery1.ToList<oP2d>();
//                    break;
                   
//                case eFiltro.MaxMin:

//                             var myQuery2 = from p in mLstPtoSCU
//                              orderby p.X ascending
//                              select p;

//                    mLstPtoSCU= myQuery2.ToList<oP2d>();
//                    break;

//                default:

//                   throw new Exception (string.Format("La Opción {0} No esta Configurada",iFiltro.ToString()));
//            }


//            //Genero  el LstEnCoordenadas SCW

//            foreach (oP2d myForPtoScu in mLstPtoSCU)
//            {
                
//                Point2d myPtoCad = getCastPto2dObjToCad(myForPtoScu);
//                myPtoCad = myPtoCad.TransformBy(mMatrixScuToScw);

//                mLstPtoSCW.Add(getCastPto2dCadToObj(myPtoCad));
//            }

                     

           


//        }
//        private Point2d getTranslateScuFromScw(Point2d iPtoScu)
//        {
//            return iPtoScu.TransformBy(mMatrixScuToScw);
//        }
//        private oP2d getCastPto2dCadToObj(Point2d iPto2dCad)
//        {
//            return new oP2d(iPto2dCad.X, iPto2dCad.Y);
//        }
//        private Point2d getCastPto2dObjToCad(oP2d iPto2dObj)
//        {
//            return new Point2d(iPto2dObj.X.Value, iPto2dObj.Y.Value);

//        }
//        #endregion





//    }
//}
