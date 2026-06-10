//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace tadLayLogica
//{
//    using System.Windows.Forms;

//    using tadLayShare;
//    using tadLayShare.puntoOld;

//    /// <summary>
//    /// Obtengo la Colección de Tramos Abanicos por Punto de Origen
//    /// Obtengo el Tramo con mejor Valoracion
//    /// </summary>
//    public class oAbanicoByPoint
//    {

//        private bool KDebug = false;


//        //VARIABLES DE ENTRADA
//        private int?                            mTrNumId                            = null;
//        private oP3d                            mPtoAbaOrigen                       = null;
//        private bool? mTrPrevioIsAbanicoAvance = null;
//        private double? mTrPrevioLon = null;
//        private double? mAngGradosTrPrevioTrDestino = null;
//        private double? mAzimutGradosPtoOriDestino = null;



//        private oP2d                            mPtoDestino                         = null;


   



//        private oRoadDes                        mDataDesign                         = null;
//        private oRoadGeo                        mDataGeo                            = null;
//        private oDataAlgAba                     mDataAlgAba                         = null;

 
//        private double?                         mDistanciaMin                       = null;
//        private double?                         mDistanciaMax                       = null;
//        private double?                         mCosteMin                           = null;
//        private double?                         mCosteMax                           = null;

        
        
//        //PROPIEDADES SALIDA

//        /// <summary>
//        /// Listado de todos los Tramos por Abanico
//        /// </summary>
//        private List<oTramoSecAba>           mLstTramosAbanicos                  = new List<oTramoSecAba>();

//        /// <summary>
//        /// Listado de Tramos Ganadores por Abanico
//        /// </summary>
//        private List<oTramoSec>              mLstTramosWin                       = new List<oTramoSec>();


//        #region "CONSTRUCTOR"

//        public oAbanicoByPoint(int iTrNum, oP3d iPtoOrigenAbanico, oP2d iPtoDestino, bool iTrPrevioIsAbanicoAvance,double iTrPrevioLon, oP3d iPtoPrevio, oRoadDes iRoadDesign, oRoadGeo iRoadGeo, oDataAlgAba iDataAlgAba)
                                                          
//        {
//            mTrNumId = iTrNum;
//            mPtoAbaOrigen = iPtoOrigenAbanico;
//            mPtoDestino = iPtoDestino;
//            mTrPrevioIsAbanicoAvance = iTrPrevioIsAbanicoAvance;
//            mTrPrevioLon = iTrPrevioLon;
//            mDataDesign = iRoadDesign;
//            mDataGeo = iRoadGeo;
//            mDataAlgAba = iDataAlgAba;
//            mAzimutGradosPtoOriDestino = oToolTrigo.getAzimutGrados(mPtoAbaOrigen.X, mPtoAbaOrigen.Y, mPtoDestino.X, mPtoDestino.Y);                      
//           // mAngGradosTrPrevioTrDestino = oToolTrigo.getAngGradosFrom3Points(mPtoAbaOrigen.X, mPtoAbaOrigen.Y, iPtoPrevio.X, iPtoPrevio.Y, mPtoDestino.X, mPtoDestino.Y);
//        }

//        #endregion


//        #region "METODOS PUBLICOS"
//        public void generarAbanicos()                     
//        {
            
//           //Listado Total de Tramos por Abanico
//            List<oTramoSecAba> myLstTramosAbanicoALL = new List<oTramoSecAba>();

//            //Obtengo el listado de Tramos del Abanico ZERO
//            List<oTramoSecAba> myLstTramosAbanicoZero = getAbanicoPrimario();

//            //Sumo los Tramos
//            myLstTramosAbanicoALL.AddRange(myLstTramosAbanicoZero);

//            if (mDataAlgAba.pAbanicoAbanceGenerar)
//            {

//                //Obtengo el número total de avance, teniendo en cuenta que el Avance Máximo
//                //NO puede ser nunca mayor a la distancia al objetivo.

//                int myAbanicoNumTotal;
//                double myDistanciaObjetivo = oToolTrigo.getDisXyTwoPoints(mPtoAbaOrigen.XY, mPtoDestino.XY).Value;

//                if (mDataDesign.avanceMax > myDistanciaObjetivo)
//                {
//                    myAbanicoNumTotal = (int)Math.Truncate(myDistanciaObjetivo / mDataDesign.AijMin); 
//                }
//                else
//                {
//                    myAbanicoNumTotal = (int)Math.Truncate(mDataDesign.avanceMax / mDataDesign.AijMin);               
//                }
                   

//               myAbanicoNumTotal = myAbanicoNumTotal - 1;

//               int  myAbanicoId = 1;

//                List<oTramoSecAba> myLstTraAbaAvance = null;
//                List<oTramoSecAba> myLstTraAbaUltimoCalculado = null;

//                for (int i = 1; i < myAbanicoNumTotal; i++)
//                {

//                    myAbanicoId++;
//                    mTrNumId++;
                   
//                    if (i == 1)
//                    {
//                        myLstTraAbaAvance = new List<oTramoSecAba>();
//                        myLstTraAbaAvance = getAbanicosSecundarios(mTrNumId.Value,myAbanicoId,mDataDesign.AijMin, myLstTramosAbanicoZero);
//                        myLstTraAbaUltimoCalculado = myLstTraAbaAvance;
//                        myLstTramosAbanicoALL.AddRange(myLstTraAbaAvance);
//                    }

//                    else
//                    {
//                        myLstTraAbaAvance = new List<oTramoSecAba>();
//                        myLstTraAbaAvance = getAbanicosSecundarios(mTrNumId.Value,myAbanicoId,mDataDesign.AijMin, myLstTraAbaUltimoCalculado);
//                        myLstTraAbaUltimoCalculado = myLstTraAbaAvance;
//                        myLstTramosAbanicoALL.AddRange(myLstTraAbaAvance);
//                    }

//                }
//            }


//            //Cargo todos los Tramos en el listado
//            mLstTramosAbanicos = myLstTramosAbanicoALL;

//            //Valorar los Tramos para Obtener el DisMinMax,CosteMinMax, SlopeMax
//            getValorDistanciaCosteMinMax(mDataAlgAba.pAbanicoAbanceGenerar);


//            //Valoro Todos los Tramos del Abanico
//            mLstTramosWin = getLstTramosWin();


//        }
//        public void draw(string iLayer)
//        {

//            if (mLstTramosAbanicos != null && mLstTramosAbanicos.Count > 0)
//            {
//                foreach (oTramoSecAba myTramo in mLstTramosAbanicos)
//                {

//                    myTramo.draw(iLayer);
                    
//                    myTramo.drawData(iLayer);
//                }
//            }
//            else
//            {
//                throw new Exception("La Colección de Tramos por abanico esta vacia");
//            }


//        }
//        #endregion

//        #region "METODOS PRIVADOS"

//        /// <summary>
//        /// Obtengo el Listado de Tramos del Abanico PRIMARIO
//        /// </summary>
//        private List<oTramoSecAba> getAbanicoPrimario()
//        {

//            List<oTramoSecAba> myLstTramosAbanicos = new List<oTramoSecAba>();

//            //Azimut Primer Punto Abanico
//            //double myTramoIniAziGrados = mAzimutGradosPtoOriDestino.Value - (mDataAlgAba.pAbanicoGradoTotal / 2);



//            double myTramoIniAziGrados = mAzimutGradosPtoOriDestino.Value + (1.5*mDataAlgAba.pAbanicoGradoTotal);

//            if (myTramoIniAziGrados > 360)
//            {
//                myTramoIniAziGrados = myTramoIniAziGrados - 360;
//            }

//            //Obtengo el Angulo entre el TramoIniTramoPrevio
//            double? myAngTramoIniTramoPrevio = mAngGradosTrPrevioTrDestino + (mDataAlgAba.pAbanicoGradoTotal / 2);

//            if (myAngTramoIniTramoPrevio.HasValue && myAngTramoIniTramoPrevio.Value > 180)
//            {
//                myAngTramoIniTramoPrevio = 360 - myAngTramoIniTramoPrevio;
//            }
           

//            //Datos FOR
//            oTrDesignAbaInicio myTramoAba;
//            int myAbaTramosNum = mDataAlgAba.pAbanicoTramosNum;
//            int myTrPosicionAbanicoId = (int)(myAbaTramosNum - 1) / 2;
//            double myAbanicoGradosDiscre = mDataAlgAba.pAbanicoDiscreGrados;
//            double myAbanicoAziGradosPos = 0;
//            double? myAbanicoAngTrPrevActual=null;

//            for (int i = 0; i < myAbaTramosNum; i++)
//            {
               
                
//                myAbanicoAziGradosPos = myTramoIniAziGrados + (i * myAbanicoGradosDiscre);
//                myAbanicoAngTrPrevActual = myAngTramoIniTramoPrevio + (i * myAbanicoGradosDiscre);
//                myTramoAba = new oTrDesignAbaInicio(mDataGeo.pEstructuraGenerar, mTrNumId.Value, myTrPosicionAbanicoId, mPtoAbaOrigen, myAbanicoAziGradosPos, mTrPrevioIsAbanicoAvance.Value, mTrPrevioLon.Value, mDataAlgAba.pDesMaxLonAbaPrimariosPorCiento, myAbanicoAngTrPrevActual, mDataDesign.Rp, mDataDesign.AijMin, mDataDesign.IsAijK, mPtoDestino, mDataAlgAba.pInvalidarTramosLonAbaPrimarios,mDataDesign.preferencias);
                

//                myTramoAba.getXY();
//                myTramoAba.getSeccion(mDataGeo.pTramoLonDiscre,mDataGeo.pendMaxPorCientoCalculo, mDataGeo.terraplenDesmonteMaxCalculo, mDataGeo.pendMaxPorCientoCalculoEstructura, mDataGeo.pilaAlturaMaximaCalculo);
//              //  myTramoAba.getCoste(mDataGeo.pAnchoPlataforma, mDataGeo.pTaludDesmonte, mDataGeo.pTaludTerraplen);

//                if (myTramoAba.pTramoValido.HasValue && myTramoAba.pTramoValido.Value)
//                {
//                    myTramoAba.pDistanciaOrigenAbanico = myTramoAba.pDis2d.Value;
//                    myTramoAba.pCosteTotalOrigenAbanico = myTramoAba.pCoste.Value;
//                }

//                if (KDebug) myTramoAba.drawData(oTadil.data.Layer.pTadil_No.name);

//                myLstTramosAbanicos.Add(myTramoAba);

//                myTrPosicionAbanicoId = myTrPosicionAbanicoId - 1;
//            }


//            // !!!!!!!!  Debo de Comprobar si existe algun tramo valido !!!!!!!!!!!!!!
//            var myQueryTrOk = from p in myLstTramosAbanicos
//                              where p.pTramoValido == true
//                              select p;

//            int myTrOk = myQueryTrOk.Count();


//            if (myTrOk == 0)
//            {
//                throw new Exception("Error001 ; No Existe solución con los datos de partida.");
//            }






//            return myLstTramosAbanicos;



//        }


//        /// <summary>
//        /// Obtengo el Listado de Tramos de los Abanicos SECUNDARIOS
//        /// </summary>
//        private List<oTramoSecAba> getAbanicosSecundarios(int iTramoOrden, int iAbanicoNum, double iAbanicoLonAvance, List<oTramoSecAba> iLstTramos)
//        {

//            List<oTramoSecAba> myLstTramoAbanicoAvance = new List<oTramoSecAba>();

//            oTrDesignAbaAvance myTraAbaAvance=null;

//            //Siempre debe de empezar por 2
//            //El abancio 1, siempre es el Abanico Primario
//            int myAbanicoAvanceNum = iAbanicoNum;

//            double myAbanicoAvanceLon = iAbanicoLonAvance;

//            foreach (oTramoSecAba myTrAba in iLstTramos)
//            {

//                //Solo Obtengo los Tramos de Avance de los Tramos Valido
//                if (myTrAba.pTramoValido.HasValue && myTrAba.pTramoValido.Value)
//                {

//                    oP3d myP1Abanico;


//                    //SI EL TRAMO ES ABANICO PRIMARIO
//                    if (myTrAba.pTrAbanicoID == 1)
//                    {

//                        //Debo de determinar cuando el tramo se cálculo con Amin
//                        double myInc = Math.Abs((myTrAba.pDis2d.Value - mDataDesign.AijMin));


//                        if (myInc < 0.05)
//                        {
//                            myP1Abanico = myTrAba.P2;

//                            //Obtengo los Nuevos Tramos
//                            myTraAbaAvance = new oTrDesignAbaAvance(mDataGeo.pEstructuraGenerar, iTramoOrden, myTrAba.pTrAbanicoPos.Value, myAbanicoAvanceNum, myTrAba.P2, myTrAba.pAzimutGrados.Value, myAbanicoAvanceLon, mPtoDestino);
//                            myTraAbaAvance.getXY();
//                            myTraAbaAvance.getSeccion(mDataGeo.pTramoLonDiscre,mDataGeo.pendMaxPorCientoCalculo, mDataGeo.terraplenDesmonteMaxCalculo, mDataGeo.pendMaxPorCientoCalculoEstructura, mDataGeo.pilaAlturaMaximaCalculo);
//                          //  myTraAbaAvance.getCoste(mDataGeo.pAnchoPlataforma, mDataGeo.pTaludDesmonte, mDataGeo.pTaludTerraplen);

//                            if (myTraAbaAvance.pTramoValido.HasValue && myTraAbaAvance.pTramoValido.Value)
//                            {
//                                myTraAbaAvance.pDistanciaOrigenAbanico = myTraAbaAvance.pDis2d.Value + myTrAba.pDistanciaOrigenAbanico;
//                                myTraAbaAvance.pCosteTotalOrigenAbanico = myTraAbaAvance.pCoste.Value + myTrAba.pCosteTotalOrigenAbanico;
//                            }

//                            if (KDebug) myTrAba.drawData(oTadil.data.Layer.pTadil_No.name);
                            
//                            myLstTramoAbanicoAvance.Add(myTraAbaAvance);
                       
//                        }
           
//                    }

//                    // TRAMO PERTENECE ABANICO SECUNDARIO
//                    else 
//                    {
//                        myP1Abanico = myTrAba.P2;

//                        //Obtengo los Nuevos Tramos
//                        myTraAbaAvance = new oTrDesignAbaAvance(mDataGeo.pEstructuraGenerar, iTramoOrden, myTrAba.pTrAbanicoPos.Value, myAbanicoAvanceNum, myTrAba.P2, myTrAba.pAzimutGrados.Value, myAbanicoAvanceLon, mPtoDestino);
//                        myTraAbaAvance.getXY();
//                        myTraAbaAvance.getSeccion(mDataGeo.pTramoLonDiscre,mDataGeo.pendMaxPorCientoCalculo, mDataGeo.terraplenDesmonteMaxCalculo, mDataGeo.pendMaxPorCientoCalculoEstructura, mDataGeo.pilaAlturaMaximaCalculo);
//                       // myTraAbaAvance.getCoste(mDataGeo.pAnchoPlataforma, mDataGeo.pTaludDesmonte, mDataGeo.pTaludTerraplen);

//                        if (myTraAbaAvance.pTramoValido.HasValue && myTraAbaAvance.pTramoValido.Value)
//                        {
//                            myTraAbaAvance.pDistanciaOrigenAbanico = myTraAbaAvance.pDis2d.Value + myTrAba.pDistanciaOrigenAbanico;
//                            myTraAbaAvance.pCosteTotalOrigenAbanico = myTraAbaAvance.pCoste.Value + myTrAba.pCosteTotalOrigenAbanico;
//                        }

//                        if (KDebug) myTrAba.drawData(oTadil.data.Layer.pTadil_No.name);
                        
//                        myLstTramoAbanicoAvance.Add(myTraAbaAvance);
//                    }


                   
                    
//                }

//            }


//            //Devuelvo el Listado de Tramos
//            return myLstTramoAbanicoAvance;
//        }


//        /// <summary>
//        /// OBTENER LOS VALORES MAX y MIN  DE DISTANCIA y COSTE
//        /// ABANICOS PRIMARIO SIN AVANCE =Valoracion por Distancia = DistObjetivo+LonTramo
//        /// ABANICOS CON AVANCE = Valoración por Distancia = DistObjetivo
//        /// </summary>
//        private void getValorDistanciaCosteMinMax(bool iAvancesLargos)
//        {


//            //TRAMOS ABANICO DE AVANCE
//            if (iAvancesLargos)
//            {
//                var myQueryDist1 = from p in mLstTramosAbanicos
//                                   where p.pTramoValido == true
//                                   select p.pDisP2toDestino; // OJO !!!!!!!

//                mDistanciaMin = myQueryDist1.Min().Value;
//                mDistanciaMax = myQueryDist1.Max().Value;
//            }

//            //TRAMOS ABANICO SIN AVANCE
//            else
//            {
//                var myQueryDist2 = from p in mLstTramosAbanicos
//                                   where p.pTramoValido == true 
//                                   select p.pDisToDestinoMasLonTramo; // OJO !!!!!!!

//                mDistanciaMin = myQueryDist2.Min().Value;
//                mDistanciaMax = myQueryDist2.Max().Value;
//            }


//            #region "COSTE MIN-MAX"

//            //Obtener la Menor y Mayor Valor de la Separación del Punto MEDIO
//            var myQueryCoste = from p in mLstTramosAbanicos
//                               where p.pTramoValido == true
//                               select p.pCosteMl.Value;


//            //Valores Min & Max Por Ajuste Z
//            mCosteMin = myQueryCoste.Min();
//            mCosteMax = myQueryCoste.Max();

//            #endregion

//        }


//        /// <summary>
//        /// UNA VEZ OBTENIDO LOS VALORES MAX y MIN
//        /// VALORAMOS TODOS LOS TRAMOS y OBTENEMOS
//        /// LOS GANADORES
//        /// </summary>
//        /// <returns></returns>
//        private List<oTramoSec> getLstTramosWin()
//        {

//            //Compruebo que estan configurados los valores Dmin;Dmax;Zmin;Zmax

//            if (! mDistanciaMin.HasValue && ! mDistanciaMin.HasValue && ! mCosteMax.HasValue && ! mCosteMin.HasValue)
//            {
//                throw new Exception("Los Valores Máximos y Minimos de Distancia y Coste, son Nulos");
//            }


//            List<oTramoSec> myOutLst = new List<oTramoSec>();


//            //Valoro todos los tramos del abanico
//            foreach (oTramoSecAba myTrAba in mLstTramosAbanicos)
//            {
//                myTrAba.gValorarTramo(mDataAlgAba.pValoracionDisMinPorCiento, mDataAlgAba.pValoracionCostePorCiento,mDataAlgAba.pValoracionSlopePorCiento, 
//                                     mDistanciaMin.Value, mDistanciaMax.Value, mCosteMin.Value, mCosteMax.Value, mDataAlgAba.pAbanicoAbanceGenerar);
//            }



//            //Obtengo la Mejor valoracion del Tramo por Abanico
//            var myQueryPtoWin = from p in mLstTramosAbanicos
//                                where p.pTramoValido == true
//                                orderby p.pValorTotal.Value descending
//                                select p;


//            //Ojo para Abanicos Superpuestos debo de Devolver un listado de tramos.
//            //Si el tramo es un abanico 3, debo de 

//            oTramoSecAba myTramoWin = (oTramoSecAba) myQueryPtoWin.First();



//            if (myTramoWin.pTrAbanicoID == 0)
//            {
//                throw new Exception("El ID del Abanico del Tramo no pueder ser Cero.");
//            }
//            else if (myTramoWin.pTrAbanicoID == 1)
//            {

//                myOutLst.Add((oTramoSec)myTramoWin);

//                return myOutLst;

//            }
//            else
//            {
//                var myQuery = from p in mLstTramosAbanicos
//                              where p.pTrAbanicoPos == myTramoWin.pTrAbanicoPos
//                              orderby p.pTrAbanicoID.Value ascending
//                              select p;

//                return myQuery.ToList<oTramoSec>(); 
//            }


//        }


//        #endregion

    









       




        



//        #region "PROPIEDADES"

//        /// <summary>
//        /// Listado de TODOS Tramos por Abanico
//        /// </summary>
//        public List<oTramoSecAba> gLstTramosByAbanico
//        {

//            get
//            {
//                if (mLstTramosAbanicos != null && mLstTramosAbanicos.Count > 0)
//                {
//                    return mLstTramosAbanicos;
//                }
//                else
//                {
//                    throw new Exception("El listado de Tramos del Abanico es Nulo.");
//                }

//            }
//        }


//        /// <summary>
//        /// Listado de Tramos Ganadores por Abanico
//        /// </summary>
//        public List<oTramoSec> gLstTramosWin
//        {
//            get
//            {
//                if (mLstTramosWin != null && mLstTramosWin.Count > 0)
//                {

//                          return mLstTramosWin;
//                }
//                else
//                {
//                    throw new Exception("El listado de Tramos Ganadores por Abanico es Nulo");               
//                }         
//            }       
//        }

//        /// <summary>
//        /// Obtengo el Último Tramo Ganador
//        /// </summary>
//        public oTramoSecAba gTramoWinEnd
//        {

//            get
//            {

//                if (mLstTramosWin != null && mLstTramosWin.Count > 0)
//                {

//                    var myQueryPtoWin = from p in mLstTramosWin
//                                        where p.pTramoValido == true
//                                        orderby p.pOrden descending
//                                        select p;

//                    oTramoSecAba myTramoLast = (oTramoSecAba)myQueryPtoWin.First();

//                    return myTramoLast;
//                }

//                else
//                {
//                    throw new Exception("El listado de Tramos Ganadores por Abanico es Nulo");  
//                }

//            }
        
//        }

//        /// <summary>
//        /// Punto Origen del Abanico
//        /// </summary>
//        public oP3d gP1Abanico
//        {
//            get
//            {
//                return mPtoAbaOrigen;
            
//            }
//        }


       

//        #endregion


//    }
//}
