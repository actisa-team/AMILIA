using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tadLayLogica.EjeVisibilidad
{

    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;


    using engCadNet;
    using tadLayShare;
    using tadLayShare.puntos;

    /// <summary>
    /// EJE VISIBILIDAD
    /// </summary>
    public class oEjeVisibilidad
    {
        #region "FIELDS PRIVADOS"

        /// <summary>
        /// Nodo de Inicio
        /// </summary>
        oNodeCad mNodeStart;
        /// <summary>
        /// Nodo de Llegada
        /// </summary>
        oNodeCad mNodeGoal;
        /// <summary>
        /// Listado de Obstaculos
        /// </summary>
        List<Polyline> mLstObstacle = new List<Polyline>();
        /// <summary>
        /// Listado de Nodos del Grafo
        /// </summary>
        List<oNodeCad> mLstNode = new List<oNodeCad>();
        /// <summary>
        /// Listado de Diagonales del Grafo
        /// </summary>
        List<oEdge> mLstEdge = new List<oEdge>();
        /// <summary>
        /// Solucion al Algortimo
        /// </summary>
        List<oNode> mPathSol = new List<oNode>();
        /// <summary>
        /// Capa donde guardar las lineas del Grafo de Visibilidad
        /// </summary>
        string mLayerGV = string.Empty;

        #endregion
        #region "CONSTRUCTOR"
        public oEjeVisibilidad(double iP0x, double iP0y, double iP1x, double iP1y)
        {
            Point2d myStart = new Point2d(iP0x, iP0y);
            mNodeStart = new oNodeCad(0, myStart, 0, 0);

            Point2d myGoal = new Point2d(iP1x, iP1y);
            mNodeGoal = new oNodeCad(1, myGoal, 0, 0);
        }
        public oEjeVisibilidad(oP2d iStart, oP2d iGoal)
        {
            Point2d myStart = new Point2d(iStart.X, iStart.Y);
            mNodeStart = new oNodeCad(0, myStart, 0, 0);

            Point2d myGoal = new Point2d(iGoal.X, iGoal.Y);
            mNodeGoal = new oNodeCad(1, myGoal, 0, 0);
        }
        #endregion
        #region "PROPIEDADES"
        /// <summary>
        /// LISTADO COORDENADAS DEL EJE VISIBILIDAD
        /// </summary>
        public List<oP2d> getPathLstPto2d
        {

            get
            {
                if (mPathSol != null && mPathSol.Count > 1)
                {
                    List<oP2d> myLst = new List<oP2d>();

                    foreach (oNode myNodo in mPathSol)
                    {
                        myLst.Add(new oP2d(myNodo.pNodePos.X, myNodo.pNodePos.Y));
                    }

                    return myLst;

                }
                else
                {
                    throw new Exception("La Solución Es Nula");
                }
            }
        }
        /// <summary>
        /// LISTADO NODOS EJE VISIBILIDAD
        /// </summary>
        private List<oNode> pPathSol
        {

            get
            {

                if (mPathSol != null && mPathSol.Count > 1)
                {
                    return mPathSol;
                }
                else
                {

                    throw new Exception("La Solución es Nula");

                }

            }

            set
            {
                mPathSol = value;
            }

        }
        #endregion
        #region "METODOS PUBLICOS"


        /// <summary>
        /// CALCULAR EJE VISIBILIDAD
        /// </summary>
        public void calcularEjeVisibilidad(List<Polyline> iLstPolilineasObstaculos, string iLayerDrawGrafoVisibilidad)
        {

            List<Polyline> misPolilineasAEliminar = new List<Polyline>();
            foreach (Polyline miPol in iLstPolilineasObstaculos)
            {

                List<Point2d> misPuntos = new List<Point2d>();
                for (int i = 0; i < miPol.NumberOfVertices; i++)
                {
                    misPuntos.Add(miPol.GetPoint2dAt(i));
                }
                Polyline miPolCotaCero = oLw.addLw2d(misPuntos, false, "0");
                misPolilineasAEliminar.Add(miPolCotaCero);
            }

            //Gestion Capas
            mLayerGV = iLayerDrawGrafoVisibilidad;
            engCadNet.oLayer.vLayerOffOn(mLayerGV, false, false);
            engCadNet.oLayer.deleteByLayer(mLayerGV);

            //Selecion con los Obstaculos
            mLstObstacle = misPolilineasAEliminar;

            //No Existen Obstaculos
            if (mLstObstacle.Count == 0)
            {
                mPathSol = new List<oNode>();
                mPathSol.Add(mNodeStart);
                mPathSol.Add(mNodeGoal);
            }
            else
            {

                //Obtengo el Conjunto de Nodos
                mLstNode = getLstNode();

                //Obtengo el listado de Edges Visibles
                getLstEdge();

                //Obtengo un listado de Node
                Dijkstra myDijkstra = new Dijkstra(mLstEdge, mLstNode.ToList<oNode>());

                //Indico el Nodo de Inicio
                myDijkstra.calculateDistance(mLstNode[0] as oNode, 0, 0, 0, false);

                //Indico el Nodo de Llegada
                pPathSol = myDijkstra.getPathTo(mLstNode[1] as oNode);

            }

            oLw.eliminarPolilineas(misPolilineasAEliminar);

        }

        /// <summary>
        /// DIBUJAR EJE VISIBILIDAD
        /// </summary>
        public Polyline draw(oTadilLayer iLayerEjeVisibilidad)
        {

            //Logica Capas
            iLayerEjeVisibilidad.On();
            iLayerEjeVisibilidad.Current();
            iLayerEjeVisibilidad.deleteItems();


            Point3dCollection myLstPto = new Point3dCollection();

            foreach (oNode myNode in pPathSol)
            {
                myLstPto.Add(new Point3d(myNode.pNodePos.X, myNode.pNodePos.Y, 0));
            }

            return engCadNet.oLw.addLw2d(myLstPto, false, iLayerEjeVisibilidad.name, iLayerEjeVisibilidad.color);
        }




        #endregion
        #region "METODOS-FUNCIONES PRIVADAS"

        private void getLstEdge()
        {
            #region "UNIR START AND GOAL"

            AddEdgeVisible(mNodeStart, mNodeGoal, false);

            #endregion
            #region "VISIBILIDAD PTO START us POLIGONOS"
            foreach (Polyline myPol in mLstObstacle)
            {
                AddEdgeVisible(mNodeStart, myPol, false);
            }
            #endregion
            #region "VISIBILIDAD PTO GOAL us POLIGONOS"
            foreach (Polyline myPol in mLstObstacle)
            {
                AddEdgeVisible(mNodeGoal, myPol, false);
            }
            #endregion
            #region "VISIBILIDAD ENTRE POLIGONOS"

            long myPolOriId;

            foreach (Polyline myPolOri in mLstObstacle)
            {

                myPolOriId = myPolOri.Handle.Value;

                for (int i = 0; i < myPolOri.NumberOfVertices; i++)
                {

                    foreach (Polyline myPolObstaculo in mLstObstacle)
                    {

                        if (myPolOriId != myPolObstaculo.Handle.Value)
                        {
                            oNode myNodeOri = getNode(myPolOri.Handle.Value, i + 1);
                            AddEdgeVisible(myNodeOri, myPolObstaculo, true);
                        }
                    }

                }
            }

            #endregion
            #region "VISIBILIDAD ENTRE VERTICES CONSECUTIVOS POLIGONO"

            foreach (Polyline myPolObs in mLstObstacle)
            {
                AddEdgeVisibleFromPolObstaculoVerticesConsecutivos(myPolObs);
            }

            #endregion
            #region "VISIBILIDAD ENTRE VERTICES NO CONSECUTIVOS POLIGONO"

            foreach (Polyline myPolObs in mLstObstacle)
            {

                //Realizo la Operacon solo con Poligono NO Convesos
                bool myIsConvex = oPolygon.PolygonIsConvex(myPolObs);

                if (!myIsConvex)
                {
                    AddEdgeVisibleFromPolObstaculoVerticesNoConsecutivos(myPolObs);
                }

            }

            #endregion
        }

        /// <summary>
        /// Obtener el Listado de Nodos del Grafo
        /// </summary>
        /// <param name="iStart">Pto Salida ID = 0</param>
        /// <param name="iGoal">Pto Llegada ID = 1</param>
        private List<oNodeCad> getLstNode()
        {

            List<oNodeCad> myLstNode = new List<oNodeCad>();
            oNodeCad myNodeCad = null;

            //Nodo Inicial
            myLstNode.Add(mNodeStart);

            //Nodo de llegada
            myLstNode.Add(mNodeGoal);


            //Id del Node en el Grafo
            int myNodeIdGrafo = mNodeGoal.pNodeIdGrafo + 1;

            foreach (Polyline myLw in mLstObstacle)
            {

                for (int i = 0; i < myLw.NumberOfVertices; i++)
                {

                    Point2d myNodePosition = myLw.GetPoint2dAt(i);

                    myNodeCad = new oNodeCad();
                    myNodeCad.pNodeIdGrafo = myNodeIdGrafo;
                    myNodeCad.pNodePos = myNodePosition;
                    myNodeCad.pPolHandle = myLw.Handle.Value;
                    myNodeCad.pNodeIdPol = i + 1;

                    myNodeIdGrafo++;

                    myLstNode.Add(myNodeCad);

                }
            }

            return myLstNode;
        }


        /// <summary>
        /// Obtener el Listo de Polilineas Obstaculo del Ss
        /// </summary>
        private List<Polyline> xgetLstPolFrmSs(SelectionSet iSsPol)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {
                List<Polyline> myLstPolObstacle = new List<Polyline>();

                foreach (ObjectId myObj in iSsPol.GetObjectIds())
                {
                    Polyline myPol = myObj.GetObject(OpenMode.ForRead) as Polyline;

                    myLstPolObstacle.Add(myPol);
                }


                return myLstPolObstacle;
            }

        }




        private void AddEdgeVisible(oNode iNodeOri, Polyline iPolObstaculo, bool iIsDiagonalPoligono)
        {

            for (int i = 0; i < iPolObstaculo.NumberOfVertices; i++)
            {

                Point2d myPtoObs = iPolObstaculo.GetPoint2dAt(i);

                //Creo la Linea 
                Line myLine = oLine.addLine2d(iNodeOri.pNodePos, myPtoObs, mLayerGV);

                //Determino si la linea No intercepta con nada

                bool myLineIsCamino = isCaminoSinObstaculos(myLine, iIsDiagonalPoligono, mLstObstacle);


                if (myLineIsCamino)
                {

                    oNode myNodeFin = getNode(iPolObstaculo.Handle.Value, i + 1);

                    mLstEdge.Add(new oEdge(iNodeOri, myNodeFin, myLine.Length));


                    //Si es Nodo Inicio-Fin Añado el Camino Inverso
                    if (iNodeOri.pNodeIdGrafo == 0 | iNodeOri.pNodeIdGrafo == 1)
                    {
                        mLstEdge.Add(new oEdge(myNodeFin, iNodeOri, myLine.Length));

                        oLine.addLine2d(myNodeFin.pNodePos, iNodeOri.pNodePos, mLayerGV);
                    }

                }
                else
                {
                    engCadNet.oTools.entidadDelete(myLine.ObjectId);
                }

            }

        }



        /// <summary>
        /// Añadir Edge Valido como Camino
        /// </summary>
        private void AddEdgeVisible(oNode iNodeOri, oNode iNodeFin, bool iIsDiagonalPoligono)
        {

            //Creo la Linea 
            Line myLine1 = oLine.addLine2d(iNodeOri.pNodePos, iNodeFin.pNodePos, mLayerGV);

            //Determino si la linea No intercepta con nada
            bool myLineIsCamino = isCaminoSinObstaculos(myLine1, iIsDiagonalPoligono, mLstObstacle);

            if (myLineIsCamino)
            {
                mLstEdge.Add(new oEdge(iNodeOri, iNodeFin, myLine1.Length));
            }
            else
            {
                engCadNet.oTools.entidadDelete(myLine1.ObjectId);
            }

        }


        /// <summary>
        /// Visibilidad entre Tramos Consecutivos
        /// </summary>
        /// <param name="iPolObstaculo">Polilinea Obstaculo</param>
        private void AddEdgeVisibleFromPolObstaculoVerticesConsecutivos(Polyline iPolObstaculo)
        {

            //Obtengo el Vertice del Grafo
            oNode myNodeIni = getNode(iPolObstaculo.Handle.Value, 1);

            //Obtengo el ID del Primer Vertice del POl en el Grafo
            int myNodeIniId = myNodeIni.pNodeIdGrafo;

            //Obtengo el ID del vertice del poligono en el Grafo
            int myGrafoVerticeFin = myNodeIni.pNodeIdGrafo + iPolObstaculo.NumberOfVertices - 1;

            oNode myNodeFin = getNode(iPolObstaculo.Handle.Value, iPolObstaculo.NumberOfVertices);



            //Añado todo los Edges del Poligono
            //Condición las Zonas de Obstaculo NO se solapan
            for (int i = 0; i < iPolObstaculo.NumberOfVertices - 1; i++)
            {
                //Nodo Base Grafico
                oNode myNode1 = getNode(iPolObstaculo.Handle.Value, i + 1);
                oNode myNode2 = getNode(iPolObstaculo.Handle.Value, i + 2);
                double myNode12Lon = myNode1.pNodePos.GetDistanceTo(myNode2.pNodePos);

                //Añado el Egge
                mLstEdge.Add(new oEdge(myNode1, myNode2, myNode12Lon));
                oLine.addLine2d(myNode1.pNodePos, myNode2.pNodePos, mLayerGV);

                //Añado el Inverso
                mLstEdge.Add(new oEdge(myNode2, myNode1, myNode12Lon));
                oLine.addLine2d(myNode2.pNodePos, myNode1.pNodePos, mLayerGV);
            }


            //Cierro el PuntoFinal Punto Inicial
            double myNodeIniFinLon = myNodeIni.pNodePos.GetDistanceTo(myNodeFin.pNodePos);


            //Añado el Egge
            mLstEdge.Add(new oEdge(myNodeIni, myNodeFin, myNodeIniFinLon));
            oLine.addLine2d(myNodeIni.pNodePos, myNodeFin.pNodePos, mLayerGV);

            //Añado el Egge Inverso
            mLstEdge.Add(new oEdge(myNodeFin, myNodeIni, myNodeIniFinLon));
            oLine.addLine2d(myNodeFin.pNodePos, myNodeIni.pNodePos, mLayerGV);

        }


        private void AddEdgeVisibleFromPolObstaculoVerticesNoConsecutivos(Polyline iPolObstaculo)
        {
            //Base Cero
            bool myVerticeIsInicio = true;
            int? myVerticeNext = null;
            int? myVerticePrevio = null;

            for (int i = 0; i < iPolObstaculo.NumberOfVertices; i++)
            {

                oNode myNodeIni = getNode(iPolObstaculo.Handle.Value, i + 1);


                if (myVerticeIsInicio)
                {
                    myVerticeNext = 1;
                    myVerticePrevio = iPolObstaculo.NumberOfVertices - 1;
                    myVerticeIsInicio = false;
                }
                else if (i == iPolObstaculo.NumberOfVertices - 1)
                {
                    myVerticeNext = 0;
                    myVerticePrevio = i - 1;
                }
                else
                {
                    myVerticeNext = i + 1;
                    myVerticePrevio = i - 1;
                }


                for (int j = 0; j < iPolObstaculo.NumberOfVertices; j++)
                {

                    if (j != i & j != myVerticePrevio.Value & j != myVerticeNext.Value)
                    {

                        oNode myNodeNext = getNode(iPolObstaculo.Handle.Value, j + 1);

                        Point2d myPtoMid = engCadNet.oTools.getMidPoint(myNodeIni.pNodePos, myNodeNext.pNodePos);

                        //Determino si es una Diagonal Exterior
                        //Compruebo que el Punto Medio sea Exterior a la Polilinea
                        bool myIsDiagonalInterior = oPolygon.isPointInPolygon(iPolObstaculo, myPtoMid.X, myPtoMid.Y);



                        if (!myIsDiagonalInterior)
                        {
                            AddEdgeVisible(myNodeIni, myNodeNext, true);
                        }


                    }


                }

            }


        }


        /// <summary>
        /// Determina si una Linea, esta Libre de Obstaculos
        /// </summary>
        private bool isCaminoSinObstaculos(Line iLine, bool iIsDiagonalPoligono, List<Polyline> iLstObstacle)
        {

            //Creo el Plano de Intersección
            int? myNumMaxIntersect = null;

            if (iIsDiagonalPoligono)
            {
                myNumMaxIntersect = 2;
            }
            else
            {
                myNumMaxIntersect = 1;

            }

            Plane myPlane = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));

            Point3dCollection myLstPtoInter = new Point3dCollection();

            using (Transaction tr = oCadManager.StartTransaction())
            {

                //Determino la interseccion de la linea con todos los obstaculos
                foreach (Polyline myPolObs in iLstObstacle)
                {


                    //Fallo en el numero de intersecciones
                    iLine.IntersectWith(myPolObs, Intersect.OnBothOperands, myPlane, myLstPtoInter, new IntPtr(), new IntPtr());

                    if (myLstPtoInter.Count < 0)
                    {
                        throw new Exception("Número de Intersecciones es Negativo¿?");

                    }

                    if (myLstPtoInter.Count > myNumMaxIntersect.Value)
                    {
                        return false;

                    }


                }

                return true;


            }

        }

        /// <summary>
        /// Obtener el Node del LstNodeCad
        /// </summary>
        private oNode getNode(long iHandle, int iPolVerNumBase1)
        {

            var myQuery = from p in mLstNode
                          where p.pPolHandle == iHandle && p.pNodeIdPol == iPolVerNumBase1
                          select p;


            oNode iNode = (oNode)myQuery.First();

            return iNode;


        }


        #endregion
    }


    #region "INTERNAL CLASS"
    internal class oEdge
    {
        private oNode _origin;
        private oNode _destination;
        private double _distance;


        public oEdge(oNode origin, oNode destination, double distance)
        {
            this._origin = origin;
            this._destination = destination;
            this._distance = distance;
        }


        public oNode Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }


        public oNode Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }


        public double Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
    }
    internal abstract class oNode
    {

        public oNode()
        {

        }

        public oNode(int iNodeGrafoId, Point2d iNodePosition)
        {
            pNodeIdGrafo = iNodeGrafoId;
            pNodePos = iNodePosition;
        }


        /// <summary>
        /// ID del NODO en el GRAFO
        /// </summary>
        public int pNodeIdGrafo { get; set; }


        /// <summary>
        /// Posicion (X,Y) del Nodo
        /// </summary>
        public Point2d pNodePos { get; set; }

    }
    internal class oNodeCad : oNode
    {

        /// <summary>
        /// Handle de la Polilinea
        /// </summary>
        public long pPolHandle { get; set; }

        /// <summary>
        /// ID del NODO en la Polilinea (Base1)
        /// </summary>
        public int pNodeIdPol { get; set; }


        public oNodeCad()
        {

        }
        public oNodeCad(int iNodeGrafoId, Point2d iNodePos, int iNodeIdPol, long iPolHandle)

            : base(iNodeGrafoId, iNodePos)

        {
            pNodeIdPol = iNodeIdPol;
            pPolHandle = iPolHandle;
        }



    }
    internal class Dijkstra
    {
        private List<oNode> _nodes;
        private List<oEdge> _edges;
        private List<oNode> _basis;
        private Dictionary<int, double> _dist;
        private Dictionary<int, oNode> _previous;

        /// <summary>
        /// Constructor
        /// </summary>
        public Dijkstra(List<oEdge> edges, List<oNode> nodes)
        {

            Edges = edges;
            Nodes = nodes;
            Basis = new List<oNode>();
            Dist = new Dictionary<int, double>();
            Previous = new Dictionary<int, oNode>();

            // Knoten aufnehmen
            foreach (oNode n in Nodes)
            {
                Previous.Add(n.pNodeIdGrafo, null);
                Basis.Add(n);
                Dist.Add(n.pNodeIdGrafo, double.MaxValue);
            }
        }

        /// <summary>
        /// Berechnet die kürzesten Wege vom start
        /// Knoten zu allen anderen Knoten
        /// </summary>
        public void calculateDistance(oNode start, double angulomin, double clmin, double R, bool anguloautomatico)
        {
            Dist[start.pNodeIdGrafo] = 0;

            while (Basis.Count > 0)
            {

                oNode myNodeOrigen = getNodeWithSmallestDistance();

                if (myNodeOrigen == null)
                {
                    Basis.Clear();
                }
                else
                {
                    if (myNodeOrigen.pNodeIdGrafo == 1)
                    {
                        int a = 0;
                    }
                    // Obtener el nodo previo en el camino
                    oNode nodoPrevio = Previous[myNodeOrigen.pNodeIdGrafo];

                    //Obtengo los Hijos del NodoOrigen
                    foreach (oNode myNodeHijo in getNeighbors(myNodeOrigen))
                    {
                        double myDistOrigen;
                        double myDistanciaNodoOrigen = Dist[myNodeOrigen.pNodeIdGrafo];
                        double myDistanciaNodoOrigenHijo = getDistanceBetween(myNodeOrigen, myNodeHijo);

                        myDistOrigen = myDistanciaNodoOrigen + myDistanciaNodoOrigenHijo;
                        if (anguloautomatico)
                        {
                            double angulo = VerificarAnguloMinimo(nodoPrevio, myNodeOrigen, myNodeHijo);
                            double angulofinal = 180;
                            if (myNodeHijo.pNodeIdGrafo == 1)
                            {
                                angulofinal = VerificarAnguloMinimoFinal(myNodeOrigen, myNodeHijo);
                            }
                            angulomin = GetAnguloCorrecto(nodoPrevio, myNodeOrigen, myNodeHijo, true, clmin, R);
                            double MinAngle_final = angulomin;
                            if (myNodeHijo.pNodeIdGrafo == 1)
                            {
                                MinAngle_final = GetAnguloCorrecto_final(myNodeOrigen, myNodeHijo, true, clmin, R);

                            }

                            if (angulo >= angulomin && angulofinal >= MinAngle_final)
                            {

                                if (myDistOrigen < Dist[myNodeHijo.pNodeIdGrafo])
                                {
                                    Dist[myNodeHijo.pNodeIdGrafo] = myDistOrigen;
                                    Previous[myNodeHijo.pNodeIdGrafo] = myNodeOrigen;
                                }
                            }
                        }
                        else
                        {
                            double angulo = VerificarAnguloMinimo(nodoPrevio, myNodeOrigen, myNodeHijo);
                            double angulofinal = 180;
                            double MinAngle_final = angulomin;
                            if (myNodeHijo.pNodeIdGrafo == 1)
                            {
                                angulofinal = VerificarAnguloMinimoFinal(myNodeOrigen, myNodeHijo);
                            }
                            if (angulo >= angulomin && angulofinal >= MinAngle_final)
                            {
                                if (myDistOrigen < Dist[myNodeHijo.pNodeIdGrafo])
                                {
                                    Dist[myNodeHijo.pNodeIdGrafo] = myDistOrigen;
                                    Previous[myNodeHijo.pNodeIdGrafo] = myNodeOrigen;
                                }
                            }
                        }
                    }

                    Basis.Remove(myNodeOrigen);
                }
            }
        }


        private double GetAnguloCorrecto(oNode previo, oNode actual, oNode siguiente, bool respetarpuntos, double clmin, double R)
        {
            double A = 0;
            double L = 0;

            if (previo == null && respetarpuntos)
            {
                if (Math.Round(oSingletonProyecto.getInstance.ptoSalida.X, 2) != Math.Round(actual.pNodePos.X, 2) &&
                     Math.Round(oSingletonProyecto.getInstance.ptoSalida.Y, 2) != Math.Round(actual.pNodePos.Y, 2))
                {
                    A = Math.Min(new Point2d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y).GetDistanceTo(actual.pNodePos), actual.pNodePos.GetDistanceTo(siguiente.pNodePos));
                    L = A / 2 - 2 * clmin;

                    return 2 * (90 - Math.Atan(L / R) * 180 / Math.PI);
                }
                else
                {
                    return 0;
                }
            }
            if (previo == null && !respetarpuntos)
            {
                return 0;
            }

            A = Math.Min(previo.pNodePos.GetDistanceTo(actual.pNodePos), actual.pNodePos.GetDistanceTo(siguiente.pNodePos));
            L = A / 2 - 2 * clmin;

            return 2 * (90 - Math.Atan(L / R) * 180 / Math.PI);
        }
        private double GetAnguloCorrecto_final(oNode actual, oNode siguiente, bool respetarpuntos, double clmin, double R)
        {
            double A = 0;
            double L = 0;

            if (Math.Round(oSingletonProyecto.getInstance.ptoSalida.X, 2) != Math.Round(siguiente.pNodePos.X, 2) &&
                     Math.Round(oSingletonProyecto.getInstance.ptoSalida.Y, 2) != Math.Round(siguiente.pNodePos.Y, 2))
            {
                A = Math.Min(new Point2d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y).GetDistanceTo(siguiente.pNodePos), actual.pNodePos.GetDistanceTo(siguiente.pNodePos));
                L = A / 2 - 2 * clmin;

                return 2 * (90 - Math.Atan(L / R) * 180 / Math.PI);
            }

            return 0;
        }

        /// <summary>
        /// Verifica si el ángulo entre tres nodos cumple con la restricción mínima
        /// </summary>
        /// <param name="nodoPrevio">Nodo anterior</param>
        /// <param name="nodoActual">Nodo actual</param>
        /// <param name="nodoSiguiente">Nodo siguiente candidato</param>
        /// <returns>True si el ángulo es mayor o igual al mínimo permitido</returns>
        private double VerificarAnguloMinimo(oNode nodoPrevio, oNode nodoActual, oNode nodoSiguiente)
        {
            // Si no hay nodo previo (es el primer nodo), no hay restricción de ángulo
            if (nodoPrevio == null)
            {
                if (Math.Round(oSingletonProyecto.getInstance.ptoSalida.X, 2) != Math.Round(nodoActual.pNodePos.X, 2) &&
                    Math.Round(oSingletonProyecto.getInstance.ptoSalida.Y, 2) != Math.Round(nodoActual.pNodePos.Y, 2))
                {
                    return CalculateAngle(new Point2d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y), nodoActual.pNodePos, nodoSiguiente.pNodePos);
                }
                else
                {
                    return 180;
                }
            }

            //return 180;

            // Calcular el ángulo entre los tres puntos
            double angulo = CalculateAngle(nodoPrevio.pNodePos, nodoActual.pNodePos, nodoSiguiente.pNodePos);

            // Verificar si el ángulo cumple con el mínimo
            return angulo;
        }
        private double VerificarAnguloMinimoFinal(oNode nodoActual, oNode nodoSiguiente)
        {
            if (Math.Round(oSingletonProyecto.getInstance.ptoLlegada.X, 2) != Math.Round(nodoSiguiente.pNodePos.X, 2) &&
                     Math.Round(oSingletonProyecto.getInstance.ptoLlegada.Y, 2) != Math.Round(nodoSiguiente.pNodePos.Y, 2))
            {
                return CalculateAngle(nodoActual.pNodePos, nodoSiguiente.pNodePos, new Point2d(oSingletonProyecto.getInstance.ptoLlegada.X, oSingletonProyecto.getInstance.ptoLlegada.Y));
            }
            else
            {
                return 180;
            }
        }
        /// <summary>
        /// Calcula el ángulo de giro (deflexión) en grados entre dos segmentos formados por 3 puntos
        /// </summary>
        /// <param name="p1">Punto anterior</param>
        /// <param name="p2">Punto actual (vértice del ángulo)</param>
        /// <param name="p3">Punto siguiente</param>
        /// <returns>Ángulo de giro en grados (0-180)</returns>
        private double CalculateAngle(Point2d p1, Point2d p2, Point2d p3)
        {
            // Vector del punto anterior al actual
            double v1x = p2.X - p1.X;
            double v1y = p2.Y - p1.Y;

            // Vector del punto actual al siguiente
            double v2x = p3.X - p2.X;
            double v2y = p3.Y - p2.Y;

            // Calcular las magnitudes de los vectores
            double mag1 = Math.Sqrt(v1x * v1x + v1y * v1y);
            double mag2 = Math.Sqrt(v2x * v2x + v2y * v2y);

            // Evitar división por cero
            if (mag1 == 0 || mag2 == 0)
                return 180;

            // Normalizar los vectores
            v1x /= mag1;
            v1y /= mag1;
            v2x /= mag2;
            v2y /= mag2;

            // Producto punto para obtener el coseno del ángulo
            double dotProduct = v1x * v2x + v1y * v2y;

            // Producto cruz (en 2D, solo la componente Z) para determinar la dirección
            double crossProduct = v1x * v2y - v1y * v2x;

            // Calcular el ángulo usando atan2 para obtener el ángulo con signo
            double angleRad = Math.Atan2(Math.Abs(crossProduct), dotProduct);

            // Convertir a grados
            double angleDeg = angleRad * (180.0 / Math.PI);

            // El ángulo de deflexión es 180 - ángulo interior
            // Si el ángulo interior es pequeño, el giro es brusco (deflexión grande)
            // Si el ángulo interior es cercano a 180°, el giro es suave (deflexión pequeña)
            double deflectionAngle = 180.0 - angleDeg;

            return deflectionAngle;
        }
        /// <summary>
        /// Obtener Camino Start-Goal
        /// </summary>
        public List<oNode> getPathTo(oNode iNodeGoal)
        {
            List<oNode> path = new List<oNode>();

            path.Insert(0, iNodeGoal);

            while (Previous[iNodeGoal.pNodeIdGrafo] != null)
            {
                iNodeGoal = Previous[iNodeGoal.pNodeIdGrafo];
                path.Insert(0, iNodeGoal);
            }

            return path;
        }

        /// <summary>
        /// Obtener Nodo Menor Coste
        /// </summary>
        private oNode getNodeWithSmallestDistance()
        {
            double distance = double.MaxValue;
            oNode smallest = null;

            foreach (oNode n in Basis)
            {
                if (Dist[n.pNodeIdGrafo] < distance)
                {
                    distance = Dist[n.pNodeIdGrafo];
                    smallest = n;
                }
            }

            return smallest;
        }

        /// <summary>
        /// Liefert alle Nachbarn von n die noch in der Basis sind
        /// </summary>
        /// <param name="n">Knoten</param>
        /// <returns></returns>
        private List<oNode> getNeighbors(oNode n)
        {
            List<oNode> neighbors = new List<oNode>();

            foreach (oEdge e in Edges)
            {
                if (e.Origin.pNodeIdGrafo == n.pNodeIdGrafo && Basis.Contains(n))
                {
                    neighbors.Add(e.Destination);
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Liefert die Distanz zwischen zwei Knoten
        /// </summary>
        /// <param name="o">Startknoten</param>
        /// <param name="d">Endknoten</param>
        /// <returns></returns>
        private double getDistanceBetween(oNode o, oNode d)
        {
            foreach (oEdge e in Edges)
            {

                if (e.Origin.pNodeIdGrafo == o.pNodeIdGrafo && e.Destination.pNodeIdGrafo == d.pNodeIdGrafo)
                {
                    return e.Distance;
                }

            }

            throw new Exception(string.Format("No se encuentra la distancia entre {0} - {1}", o.pNodeIdGrafo.ToString(), d.pNodeIdGrafo.ToString()));
        }

        /// <summary>
        /// Listado de Nodos
        /// </summary>
        public List<oNode> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>
        /// Listado de Diagonales
        /// </summary>
        public List<oEdge> Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }

        /// <summary>
        /// Listado Inicial de Nodos
        /// </summary>
        public List<oNode> Basis
        {
            get { return _basis; }
            set { _basis = value; }
        }

        /// <summary>
        /// Listado ID Nodo, Distancia Minimma
        /// </summary>
        public Dictionary<int, double> Dist
        {
            get { return _dist; }
            set { _dist = value; }
        }

        /// <summary>
        /// Diccionario ID Nodo, Nodo Previo
        /// </summary>
        public Dictionary<int, oNode> Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }
    }
    #endregion


}










