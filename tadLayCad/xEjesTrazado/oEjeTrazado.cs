using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using cv = Autodesk.Civil.Land.DatabaseServices;
    using Autodesk.Civil;




    using engCadNet;
    using tadLayShare;


    public class oEjeTrazado
    {
        /// <summary>
        /// Nombre del Eje
        /// </summary>
        private string mEjeName = string.Empty;
        /// <summary>
        /// Eje Civil 3D
        /// </summary>
        private cv.Alignment mRoadAlign = null;

        private eRoadTipo mRoadTipo;
        private double mRp;

        Func<double,eRoadCurva> mFunGetCurva;
        Func<double, eRoadCurva, oRadioNormaAvalue> mFunGetRadioAspiral;
        

        private Dictionary<int, oSegmento> mSegmentos = new Dictionary<int, oSegmento>();
        private Dictionary<int, oVertice> mVertices = new Dictionary<int, oVertice>();


        #region "Constructor"
        public oEjeTrazado(string iEjeName,
                    eRoadTipo iRoadTipo,
                    double iRp,
                    Func<double,eRoadCurva> iFunGetCurva,
                    Func<double,eRoadCurva,oRadioNormaAvalue> iFunGetRadioSpiral)
        {
            mEjeName = iEjeName;
            mRoadTipo = iRoadTipo;
            mRp = iRp;
            mFunGetCurva = iFunGetCurva;
            mFunGetRadioAspiral = iFunGetRadioSpiral;
        }
        #endregion


        #region "Propiedades"

        /// <summary>
        /// Primer Vertice
        /// </summary>
        public oVertice first
        {
            get
            {
                return vertices[0];
            }
        }

        /// <summary>
        /// Primer Vertice
        /// </summary>
        public oVertice second
        {
            get
            {
                return vertices[1];
            }
        }

        /// <summary>
        /// Último Vertice
        /// </summary>
        public oVertice last
        {
            get
            {
                return vertices[vertices.Count - 1];
            }

        }


        private oSegmento firstSeg
        {

            get
            {
                return segmentos[0];
            
            }
        
        }


        private oSegmento lastSeg
        {

            get
            {
                return segmentos[segmentos.Count-1];

            }

        }


        public Dictionary<int, oVertice> vertices
        {

            get
            {
                return mVertices;
            }
        }

        private Dictionary<int,oSegmento> segmentos
        {

            get
            {
                return mSegmentos;
           
            }

    
    
         }




        #endregion

        public void addVertices(object iLwObj)
        {

            Polyline3d iLw = (Polyline3d)iLwObj;

           
           
            #region "Coordenadas"

            //Añado las Coordenadas
            int j = 0;

            foreach (ObjectId myVertice in iLw)
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {
                    PolylineVertex3d myVertice3d = (PolylineVertex3d)tr.GetObject(myVertice, OpenMode.ForRead);

                    mVertices.Add(j, new oVertice(j, new Point3d(myVertice3d.Position.X, myVertice3d.Position.Y, 0)));
                    j++;

                }

            }

            #endregion
            #region "Vertices Angulos"

            double? myAng = null;

            //Vertice Inicial
            first.angGr = null;
            //Vertice Final
            last.angGr = null;


            for (int i = 1; i < vertices.Count - 1; i++)
            {

                myAng = oToolTrigo.getAngGradosFrom3Points(vertices[i].position.X, vertices[i].position.Y,
                                                           vertices[i - 1].position.X, vertices[i - 1].position.Y,
                                                           vertices[i + 1].position.X, vertices[i + 1].position.Y);

                if (myAng.HasValue)
                {
                    vertices[i].angGr = myAng.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Angulo Entre Vertices");
                }

            }

            #endregion
            #region "Tipo de Vertice"

      

            //Vertice Inicial
            first.tipo = eRoadCurva.Paso;
            //Vertice Final
            last.tipo = eRoadCurva.Paso;



            for (int i = 1; i < vertices.Count - 1; i++)
            {
                vertices[i].tipo = mFunGetCurva(vertices[i].angGr.Value);

                       
            }

            #endregion
            #region "Radio Diseño-Aspiral"

            oRadioNormaAvalue myRnAv;

            //Vertice Inicial
            first.radio = null;
            first.Avalue = null;
            //Vertice Final
            last.radio = null;
            last.Avalue = null;


            for (int i = 1; i < vertices.Count - 1; i++)
            {
                myRnAv = mFunGetRadioAspiral(vertices[i].angGr.Value, vertices[i].tipo);
                vertices[i].radio = myRnAv.RadioNorma;
                vertices[i].Avalue = myRnAv.Aspiral;
            }

            #endregion
            #region "Centros Radio Vertices Paso"

            Point3d myArcoPre;
            Point3d myArcoNext;

            //Vertice Inicial
            first.arcoP1 = null;
            first.arcoP2 = null;
            //Vertice Final
            last.arcoP1 = null;
            last.arcoP2 = null;


            for (int i = 1; i < vertices.Count - 1; i++)
            {
                if (vertices[i].tipo == eRoadCurva.Paso)
                {

                    oVertice.getCenterVerticePaso(vertices[i].position, vertices[i].angGr.Value, vertices[i].radio.Value, vertices[i - 1].position, vertices[i + 1].position, out myArcoPre, out myArcoNext);

                    vertices[i].arcoP1 = myArcoPre;
                    vertices[i].arcoP2 = myArcoNext;
                }
                else
                {
                    vertices[i].arcoP1 = null;
                    vertices[i].arcoP2 = null;
                }


            }


            #endregion
            #region "Entidad Vertices"

            //Vertice Inicial
            first.entidad = null;
            //Vertice Final
            last.entidad = null;

            //Radio


            for (int i = 1; i < vertices.Count - 1; i++)
            {
                vertices[i].entidad = oVertice.getEntidadVertice(vertices[i].tipo, vertices[i].angGr.Value);
            }
            #endregion
            #region "Segemento"
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                mSegmentos.Add(i, new oSegmento(i));
            }

            for (int i = 0; i < vertices.Count - 1; i++)
            {
                mSegmentos[i].lon = oVertice.getLonTwoVertices(vertices[i].position, vertices[i + 1].position);
            }

            #endregion
        }
        public bool check(double iAmin, double iAminStartGoal, double iAmax, double iTolerancia)
        {


            bool myCheck = true;
            bool? myError = null;
            StringBuilder myLstError = new StringBuilder(string.Format("Rango de Valores: Amin Salida-Llegada: {0} | Amin: {1} | Amax: {2}",iAminStartGoal,iAmin,iAmax));
           
            //Check Tramo Inicial
            myError=oToolsNet.isOutRange(firstSeg.lon.Value, iAminStartGoal, iAmax,iTolerancia);

            if (myError.Value)
            {
                myLstError.AppendLine();
                myLstError.Append(string.Format("{0} | Esta Fuera de Rango.", mSegmentos[0].ToString()));
                myCheck = false;
            }


            for (int i = 1; i < vertices.Count - 2; i++)
            {
                myError = oToolsNet.isOutRange(mSegmentos[i].lon.Value, iAmin, iAmax, iTolerancia);
                if (myError.Value)
                {
                    myLstError.AppendLine();
                    myLstError.Append(string.Format("{0} | Esta Fuera de Rango.", mSegmentos[i].ToString()));
                  
                    myCheck=false;
                }
            }


            //Check Tramo Inicial
            myError = oToolsNet.isOutRange(lastSeg.lon.Value, iAminStartGoal, iAmax, iTolerancia);

            if (myError.Value)
            {
                myLstError.AppendLine();
                myLstError.Append(string.Format("{0} | Esta Fuera de Rango.",mSegmentos[vertices.Count-1].ToString()));
                myCheck=false;
            }



            if (myCheck)
            {
                return true;
            }
            else
            {
              throw new Exception(myLstError.ToString());
            }
        
        
        }
        public void addRoad(string iLayer,string iEstEje, string iEstLabel, string iInfoRoad)
        {

            //Check Capas
            if (engCadNet.oLayer.HasLayer(iLayer))
            {
                engCadNet.oLayer.deleteByLayer(iLayer);
            }
            else
            {
                engCadNet.oLayer.addLayer(iLayer, 25, true);
            }

            //Creo el Eje
            createEjeCivil(iLayer,iEstEje,iEstLabel,iInfoRoad);

            //Inicio el Proceso Tramo Inicial
            createIni();


            // Creo los Tramos Intermedios
            for (int i = 1; i < vertices.Count - 2; i++)
            {
                createEntidad(vertices[i], vertices[i + 1]);
            }

            //Creo el tramo final
            createFin();


            //Guardo los tipos de Curvas del Trazado
            addXdata();

        }
        private void addXdata()
        {
            
            //Añado el Xdata con el Nombre de la Solución.
            tadLayCad.oTadilXdata.addXdataSolucionNombre(mRoadAlign,mEjeName);

            
            //Añado el Xdata con los Tipos de Curvas.
            string myXdata = string.Empty;

            for (int i = 0; i < mVertices.Count; i++)
            {
                myXdata = myXdata + ";" + mVertices[i].tipo.ToString();
            }

            myXdata = myXdata.Remove(0, 1);

            tadLayCad.oTadilXdata.addXdataCurvasTipo(mRoadAlign, myXdata);

            
        }
        private void createIni()
        {

            if (vertices[1].tipo == eRoadCurva.NoPaso)
            {
                //Creo la Linea Inicial
                cv.AlignmentLine myLine = mRoadAlign.Entities.AddFixedLine(vertices[0].position, vertices[1].position);
                vertices[1].idEntidad = null;

                mSegmentos[0].idEntity = myLine.EntityId;
                mSegmentos[0].isConfig = false;
                mSegmentos[0].entidad = eRoadTramoEntidad.FixedLine;

            }
            else if (vertices[1].tipo == eRoadCurva.Paso)
            {
                //Creo el Arco
                cv.AlignmentArc myArco = mRoadAlign.Entities.AddFixedCurve(0, vertices[1].arcoP1.Value, vertices[1].position, vertices[1].arcoP2.Value);
                //Creo la Linea

    

            
                cv.AlignmentSTS myLineFlotante = mRoadAlign.Entities.AddFloatingLineWithSpiral(myArco.EntityId, cv.EntityAttachType.Prepend, vertices[1].Avalue.Value, cv.SpiralParamType.AValue, vertices[0].position, SpiralType.Clothoid);
                //Añado la Entidad Vertice
                vertices[1].idEntidad = myArco.EntityId;


                mSegmentos[0].entidad = eRoadTramoEntidad.FloatLineSpiral;
                mSegmentos[0].idEntity = myLineFlotante.EntityId;
                mSegmentos[0].isConfig = true;


            }
            else
            {
                throw new Exception("Vertice No Definido");
            }

        }
        private void createFin()
        {

            oVertice myVerIni = vertices[last.id - 1];
            oSegmento mySegPrevio = mSegmentos[myVerIni.id - 1];




            if (myVerIni.tipo == eRoadCurva.NoPaso)
            {
                //Creo la Linea Inicial
                cv.AlignmentLine myLine = mRoadAlign.Entities.AddFixedLine(myVerIni.position, last.position);
                vertices[myVerIni.id].idEntidad = null;


                if (myVerIni.entidad.Value == eRoadVerticeEntidad.FreeSpiral)
                {

                    //Creo la Clotoide
                    cv.AlignmentSCS myScS = mRoadAlign.Entities.AddFreeSCS(mySegPrevio.idEntity.Value, myLine.EntityId, myVerIni.Avalue.Value, myVerIni.Avalue.Value, cv.SpiralParamType.AValue, myVerIni.radio.Value, false, SpiralType.Clothoid);
                    vertices[myVerIni.id].idEntidad = myScS.EntityId;
                    vertices[myVerIni.id].entidad = eRoadVerticeEntidad.FreeSpiral;
                    mSegmentos[myVerIni.id].isConfig = true;

                }
                else if (myVerIni.entidad.Value == eRoadVerticeEntidad.FreeCurve)
                {
                    //Creo la Curva de Empalme
                    cv.AlignmentCurve myCurve = mRoadAlign.Entities.AddFreeCurve(mySegPrevio.idEntity.Value, myLine.EntityId, myVerIni.radio.Value, cv.CurveParamType.Radius, false, cv.CurveType.Compound);
                    vertices[myVerIni.id].idEntidad = myCurve.EntityId;
                    vertices[myVerIni.id].entidad = eRoadVerticeEntidad.FreeCurve;
                    mSegmentos[myVerIni.id].isConfig = true;

                }
                else
                {
                    throw new NotImplementedException("CreateLine");
                }



            }

            else if (myVerIni.tipo == eRoadCurva.Paso)
            {


                //Creo la LineaFlotante
                cv.AlignmentSTS myLineFloatS = mRoadAlign.Entities.AddFloatingLineWithSpiral(myVerIni.idEntidad.Value, cv.EntityAttachType.Append, myVerIni.Avalue.Value, cv.SpiralParamType.AValue, last.position, SpiralType.Clothoid);

                mSegmentos[myVerIni.id].entidad = eRoadTramoEntidad.FloatLineSpiral;
                mSegmentos[myVerIni.id].idEntity = myLineFloatS.EntityId;
                mSegmentos[myVerIni.id].isConfig = true;



            }
            else
            {
                throw new Exception("Vertice No Definido");
            }




        }
        private void createEntidad(oVertice iV1, oVertice iV2)
        {


            oSegmento mySegPrevio = mSegmentos[iV1.id - 1];
            oSegmento mySegNext = mSegmentos[iV1.id];

            try
            {





                #region"NO PASO & NO PASO"




                if (iV1.tipo == eRoadCurva.NoPaso && iV2.tipo == eRoadCurva.NoPaso)
                {
                    cv.AlignmentLine myLine = mRoadAlign.Entities.AddFixedLine(iV1.position, iV2.position);

                    mSegmentos[iV1.id].entidad = eRoadTramoEntidad.FixedLine;
                    mSegmentos[iV1.id].idEntity = myLine.EntityId;
                    mSegmentos[iV1.id].isConfig = false;

                    if (!mySegPrevio.isConfig && mySegPrevio.entidad.HasValue && mySegPrevio.entidad.Value == eRoadTramoEntidad.FixedLine | mySegPrevio.entidad.Value == eRoadTramoEntidad.FloatLineSpiral)
                    {

                        if (iV1.entidad.Value == eRoadVerticeEntidad.FreeSpiral)
                        {

                            //Creo la Clotoide
                            cv.AlignmentSCS myScS = mRoadAlign.Entities.AddFreeSCS(mySegPrevio.idEntity.Value, myLine.EntityId, iV1.Avalue.Value, iV1.Avalue.Value, cv.SpiralParamType.AValue, iV1.radio.Value, false, SpiralType.Clothoid);
                            vertices[iV1.id].idEntidad = myScS.EntityId;
                            mSegmentos[iV1.id - 1].isConfig = true;

                        }
                        else if (iV1.entidad.Value == eRoadVerticeEntidad.FreeCurve)
                        {
                            //Creo la Clotoide
                            cv.AlignmentCurve myCurve = mRoadAlign.Entities.AddFreeCurve(mySegPrevio.idEntity.Value, myLine.EntityId, iV1.radio.Value, cv.CurveParamType.Radius, false, cv.CurveType.Compound);
                            vertices[iV1.id].idEntidad = myCurve.EntityId;
                            mSegmentos[iV1.id - 1].isConfig = true;
                        }
                        else
                        {
                            throw new NotImplementedException("CreateLine");
                        }

                    }



                }

                #endregion
                #region"PASO & PASO"
                else if (iV1.tipo == eRoadCurva.Paso && iV2.tipo == eRoadCurva.Paso)
                {


                    if (iV1.idEntidad == null)
                    {
                        //Creo el Arco
                        cv.AlignmentArc myArco1 = mRoadAlign.Entities.AddFixedCurve(0, vertices[iV1.id].arcoP1.Value, vertices[iV1.id].position, vertices[iV1.id].arcoP2.Value);
                        vertices[iV1.id].idEntidad = myArco1.EntityId;
                    }


                    //Creo el Arco
                    cv.AlignmentArc myArco2 = mRoadAlign.Entities.AddFixedCurve(0, vertices[iV2.id].arcoP1.Value, vertices[iV2.id].position, vertices[iV2.id].arcoP2.Value);
                    vertices[iV2.id].idEntidad = myArco2.EntityId;


                    //Debo de Determinar el tipo de Clotoide a Realizar
                    cv.AlignmentSTS myCurva;


                    if (mRoadTipo == eRoadTipo.preferCurvas)
                    {

                        //Curva Doble Espiral Sin Lineas
                        myCurva = mRoadAlign.Entities.AddFreeSSBetweenCurves(vertices[iV1.id].idEntidad.Value, vertices[iV2.id].idEntidad.Value, 1, cv.SpiralParamType.AValue, SpiralType.Clothoid);

                        mSegmentos[iV1.id].entidad = eRoadTramoEntidad.FreeSSBetweenCurves;

                    }
                    else if (mRoadTipo == eRoadTipo.preferRectas)
                    {
                        double myAijLon = oVertice.getLonTwoVertices(iV1.position, iV2.position);

                        if (myAijLon > 4 * mRp)
                        {

                            double myDisCentros = oVertice.getLonTwoVertices(iV1.center, iV2.center);

                            //Lon Tramo Entre Espirales
                            oSpiralLine mySpiralData = new oSpiralLine(iV1.radio.Value, iV1.Avalue.Value, iV2.radio.Value, iV2.Avalue.Value, myDisCentros);

                            //Curva Spiral Simetrica con Linea
                            myCurva = mRoadAlign.Entities.AddFreeSTS(vertices[iV1.id].idEntidad.Value, vertices[iV2.id].idEntidad.Value, mySpiralData.LonLineaEntreEspirales, SpiralType.Clothoid);

                            mSegmentos[iV1.id].entidad = eRoadTramoEntidad.FreeSTS;
                        }
                        else
                        {

                            //Curva Doble Espiral Sin Lineas
                            myCurva = mRoadAlign.Entities.AddFreeSSBetweenCurves(vertices[iV1.id].idEntidad.Value, vertices[iV2.id].idEntidad.Value, 1, cv.SpiralParamType.AValue, SpiralType.Clothoid);

                            mSegmentos[iV1.id].entidad = eRoadTramoEntidad.FreeSSBetweenCurves;
                        }



                    }
                    else
                    {

                        throw new NotImplementedException("PasoPaso");

                    }





                    mSegmentos[iV1.id].idEntity = myCurva.EntityId;
                    mSegmentos[iV1.id].isConfig = true;

                }
                #endregion
                #region"CASO NOPASO - PASO"
                else if (iV1.tipo == eRoadCurva.NoPaso && iV2.tipo == eRoadCurva.Paso)
                {

                    //Creo el Arco2
                    cv.AlignmentArc myArco2 = mRoadAlign.Entities.AddFixedCurve(0, vertices[iV2.id].arcoP1.Value, vertices[iV2.id].position, vertices[iV2.id].arcoP2.Value);
                    vertices[iV2.id].idEntidad = myArco2.EntityId;

                    //Creo la LineaFlotante
                    cv.AlignmentSTS myLineFloatS = mRoadAlign.Entities.AddFloatingLineWithSpiral(myArco2.EntityId, cv.EntityAttachType.Prepend, iV1.Avalue.Value, cv.SpiralParamType.AValue, iV1.position, SpiralType.Clothoid);

                    mSegmentos[iV1.id].entidad = eRoadTramoEntidad.FloatLineSpiral;
                    mSegmentos[iV1.id].idEntity = myLineFloatS.EntityId;
                    mSegmentos[iV1.id].isConfig = true;



                    if (!mySegPrevio.isConfig)
                    {
                        //Creo el Empalme con la Linea Anterior
                        if (iV1.entidad.Value == eRoadVerticeEntidad.FreeSpiral)
                        {
                            //Creo la Clotoide
                            cv.AlignmentSCS myScS = mRoadAlign.Entities.AddFreeSCS(mySegPrevio.idEntity.Value, myLineFloatS.EntityId, iV1.Avalue.Value, iV1.Avalue.Value, cv.SpiralParamType.AValue, iV1.radio.Value, false, SpiralType.Clothoid);

                            vertices[iV1.id].idEntidad = myScS.EntityId;

                        }
                        else if (iV1.entidad.Value == eRoadVerticeEntidad.FreeCurve)
                        {


                            //Creo la Clotoide
                            cv.AlignmentCurve myCurve = mRoadAlign.Entities.AddFreeCurve(mySegPrevio.idEntity.Value, myLineFloatS.EntityId, iV1.radio.Value, cv.CurveParamType.Radius, false, cv.CurveType.Compound);

                            vertices[iV1.id].idEntidad = myCurve.EntityId;
                        }
                        else
                        {
                            throw new NotImplementedException("CreateLine");
                        }

                    }

                }
                #endregion
                #region "PASO & NO PASO"
                else if (iV1.tipo == eRoadCurva.Paso && iV2.tipo == eRoadCurva.NoPaso)
                {


                    if (iV1.idEntidad == null)
                    {
                        //Creo el Arco
                        cv.AlignmentArc myArco1 = mRoadAlign.Entities.AddFixedCurve(0, vertices[iV1.id].arcoP1.Value, vertices[iV1.id].position, vertices[iV1.id].arcoP2.Value);
                        vertices[iV1.id].idEntidad = myArco1.EntityId;
                    }

                    //Creo la LineaFlotante
                    cv.AlignmentSTS myLineFloatS = mRoadAlign.Entities.AddFloatingLineWithSpiral(vertices[iV1.id].idEntidad.Value, cv.EntityAttachType.Append, iV1.Avalue.Value, cv.SpiralParamType.AValue, iV2.position, SpiralType.Clothoid);

                    mSegmentos[iV1.id].entidad = eRoadTramoEntidad.FloatLineSpiral;
                    mSegmentos[iV1.id].idEntity = myLineFloatS.EntityId;
                    mSegmentos[iV1.id].isConfig = false;

                }
                else
                {
                    throw new NotImplementedException("createEntidad");

                }

                


                #endregion
            }

            catch (Exception)
            {
                throw new Exception(string.Format("Error Generando {0}\n{1}\n{2}",mySegNext.ToString(),iV1.ToString(),iV2.ToString()));
            }



        }
        private void createEjeCivil(string iLayer, string iEstEje, string iEstLabel, string iInfoRoad)
        {

            //Definir los Estilos

     
         
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                
                //Chequeo que Existen al Menos un Estilo de Eje y Etiqueta

                engCadNet.cv3d.oAlignment.existenEstilos();
                
                ObjectId mySite= ObjectId.Null;
                ObjectId myLayerObjId = engCadNet.oLayer.getLayerObjId(iLayer);
                ObjectId myStObjEjeId =engCadNet.cv3d.oAlignment.getEstiloEje(iEstEje);
                ObjectId myStLabEjeId = engCadNet.cv3d.oAlignment.getEstiloLabel(iEstLabel);
          
               using (Transaction tr = oCadManager.StartTransaction())
                {

                    try
                    {
                        ObjectId myEjeId = oCadManager.thisDoc.GetSitelessAlignmentId(mEjeName);
                        mRoadAlign = (cv.Alignment)tr.GetObject(myEjeId, OpenMode.ForWrite);
                        mRoadAlign.Description = iInfoRoad;



                    }
                    catch (ArgumentException)
                    {
                        ObjectId myEje = cv.Alignment.Create(oCadManager.thisDoc, mEjeName,mySite,myLayerObjId,myStObjEjeId, myStLabEjeId);
                        mRoadAlign = (cv.Alignment)tr.GetObject(myEje, OpenMode.ForWrite);
                        mRoadAlign.Description = iInfoRoad;

                    }
                    finally
                    {
                        tr.Commit();
                    }

                }
            }
        }


        public static cv.Alignment getEjeTrazadoUser()
        {
        
            //Seleccion Usuario Eje Trazado
            ObjectId myEjeTrazadoId = engCadNet.oSs.getSsByUser("Selecciona Eje Trazado", null, eEntidades.AECC_ALIGNMENT, true)[0].ObjectId;

            using (Transaction tr = oCadManager.StartTransaction())
            {
               cv.Alignment myAlign = (cv.Alignment)tr.GetObject(myEjeTrazadoId, OpenMode.ForRead);

                tr.Commit();

                return myAlign;
            }

        
        
        }

        public static  Polyline3d getEjeBasicoFromTrazado(string iSolName)
        {

            SelectionSet mySs = engCadNet.oSs.getSsByEntidadAndXdata(eEntidades.POLYLINE, eXdataKey.solucionName.ToString());

            if (mySs != null)
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {

                    string mySolNameXdata;

                    foreach (SelectedObject myObj in mySs)
                    {
                        DBObject myLwId = tr.GetObject(myObj.ObjectId, OpenMode.ForRead);

                        mySolNameXdata = engCadNet.oXdata.getXData(myLwId as object, eXdataKey.solucionName.ToString(), string.Empty)[0];


                        if (mySolNameXdata == iSolName)
                        {
                            return tr.GetObject(myLwId.ObjectId, OpenMode.ForRead) as Polyline3d;
                        }
                    }

                    throw new Exception("No se Encuentra el Eje Básico de la solución : " + iSolName);
                }


            }
            else
            {
                throw new Exception("No se Encuentra el Eje Básico de la solución : " + iSolName);
            }

        }


    }


    internal class oSegmento
    {


        public int idSeg { get; set; }
        public int? idEntity { get; set; }
        public bool isConfig { get; set; }
        public eRoadTramoEntidad? entidad { get; set; }
        public double? lon { get; set; }

 

        public oSegmento(int iIdSeg)
        {
            idSeg = iIdSeg;
            idEntity = null;
            isConfig = false;
            entidad = null;
            lon = null;
        }

        public override string ToString()
        {
            string myidBaseUno = Convert.ToString(idSeg + 1);
            string myLonstr = Convert.ToString(Math.Round(lon.Value, 1));

            return "Tramo: " + myidBaseUno + "| Lon: " + myLonstr;

        }
   
    }
    internal class oSpiralLine
    {
        private double mR1;
        private double mR2;

        private double mDisCenter;

        private double mLe1;
        private double mLe2;

        private double mQe1;
        private double mQe2;

        private double mXm1;
        private double mXm2;

        private double mInc1;
        private double mInc2;

        private double mM1;
        private double mM2;

        private double mL1;
        private double mL2;





        public oSpiralLine(double iRadio1, double iA1spiral, double iRadio2, double iA2spiral, double iDisCenter)
        {

            mR1 = iRadio1;
            mR2 = iRadio2;

            mDisCenter = iDisCenter;

            mLe1 = getLe(iRadio1, iA1spiral);
            mLe2 = getLe(iRadio2, iA2spiral);

            mQe1 = mLe1 / (2 * iRadio1);
            mQe2 = mLe2 / (2 * iRadio2);

            mXm1 = getXm(mLe1, mQe1, mR1);
            mXm2 = getXm(mLe2, mQe2, mR2);

            mInc1 = getInc(mLe1, mQe1, iRadio1);
            mInc2 = getInc(mLe2, mQe2, iRadio2);

            mM1 = getM1();

            mM2 = mDisCenter - mM1;


            mL1 = getL(mM1, mR1, mInc1);
            mL2 = getL(mM2, mR2, mInc2);



        }


        public double LonLineaEntreEspirales
        {


            get
            {
                return mL1 + mL2 - mXm1 - mXm2;

            }



        }



        private double getLe(double iR, double iAspiral)
        {
            return (Math.Pow(iAspiral, 2)) / iR;
        }


        private double getXm(double iLe, double iQe, double iRadio)
        {


            double my1 = 1;
            double my2 = (Math.Pow(iQe, 2)) / 10;
            double my3 = (Math.Pow(iQe, 4)) / 216;
            double my4 = (Math.Pow(iQe, 6)) / 9360;
            double my5 = (Math.Pow(iQe, 8)) / 685440;



            double mySum = my1 - my2 + my3 - my4 + my5;

            double myXe = iLe * mySum;

            return myXe - (iRadio * Math.Sin(iQe));

        }


        private double getInc(double iLe, double iQe, double iRadio)
        {


            double my1 = iQe / 3;
            double my2 = Math.Pow(iQe, 3) / 42;
            double my3 = Math.Pow(iQe, 5) / 1320;
            double my4 = Math.Pow(iQe, 7) / 75600;


            double myYe = iLe * (my1 - my2 + my3 - my4);

            return myYe - (iRadio * (1 - Math.Cos(iQe)));


        }

        private double getM1()
        {

            double my1 = 1 + ((mR2 + mInc2) / (mR1 + mInc1));

            return mDisCenter / my1;

        }


        private double getL(double iM1, double iRadio, double iInc)
        {

            double my1 = (Math.Pow(iM1, 2)) - (Math.Pow(iRadio + iInc, 2));


            return Math.Pow(my1, 0.5);

        }


    }

   






    }




