using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tadLayCad
{

    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

  
    using engCadNet;
    using tadLayShare;
    using tadLayCad.draw;
    using tadLayShare.puntoOld;
 
    
   public  class oEjeBasico
    {

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



       public oEjeBasico(oP2d iStart, oP2d iGoal)       
       {
           Point2d myStart = new Point2d(iStart.X.Value, iStart.Y.Value);
           mNodeStart = new oNodeCad(0, myStart, 0, 0);

           Point2d myGoal  = new Point2d(iGoal.X.Value, iGoal.Y.Value);
           mNodeGoal = new oNodeCad(1, myGoal, 0, 0); 
       }

       
     



       /// <summary>
       /// Listado de Coordenadas de la Solucion
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




       #region "MetodosPublicos"

       public void getPath(string iLayerObstaculos, string iLayerDrawGrafoVisibilidad)
       {
           //Gestion Capas
           mLayerGV = iLayerDrawGrafoVisibilidad;
           engCadNet.oLayer.vLayerOffOn(mLayerGV, false);
           engCadNet.oLayer.deleteByLayer(mLayerGV);

           //Selecion con los Obstaculos
           SelectionSet mySs = oSs.getSsByLayerAndEntidad(iLayerObstaculos, eEntidades.LWPOLYLINE);

           //No Existen Obstaculos

           if (mySs == null)
           {
               mPathSol = new List<oNode>();  
               mPathSol.Add(mNodeStart);
               mPathSol.Add(mNodeGoal);
           }
           else
           {
               //Listado de Polilineas del Ss
               mLstObstacle = getLstPolFrmSs(mySs);

               //Obtengo el Conjunto de Nodos
               mLstNode = getLstNode();

               //Obtengo el listado de Edges Visibles
               getLstEdge();

               //Obtengo un listado de Node
               Dijkstra myDijkstra = new Dijkstra(mLstEdge, mLstNode.ToList<oNode>());

               //Indico el Nodo de Inicio
               myDijkstra.calculateDistance(mLstNode[0] as oNode);

               //Indico el Nodo de Llegada
               pPathSol = myDijkstra.getPathTo(mLstNode[1] as oNode);
           
           }

       }


       //Obtener las Coordenadas desde el Cad
       public static List<oP2d> getPathFromCad(string iLayerEjeBasico)
       { 
       
           //Genero Conjunto de Selección 

           SelectionSet mySsPolEjeBasico = engCadNet.oSs.getSsByLayerAndEntidad(iLayerEjeBasico, eEntidades.LWPOLYLINE);

           if (mySsPolEjeBasico == null || mySsPolEjeBasico.Count == 0)
           {
               throw new oExEjeBasicoNullValue();
           }
           else if (mySsPolEjeBasico.Count > 1)
           {
               throw new oExEjeBasicoDuplicate(mySsPolEjeBasico.Count);
           }
           else
           { 
           
              using (Transaction tr = oCadManager.StartTransaction())
                {

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    Polyline myEjeBasicoPol = tr.GetObject(mySsPolEjeBasico[0].ObjectId,OpenMode.ForRead) as Polyline ;

                    List<oP2d> myLstPto2d = new List<oP2d>();

                   for (int i = 0; i < myEjeBasicoPol.NumberOfVertices; i++)
                    {

                        Point2d myPto2d = myEjeBasicoPol.GetPoint2dAt(i);

                        myLstPto2d.Add(new oP2d(myPto2d.X,myPto2d.Y));
                    }


                   myLstPto2d.Reverse();

                  return myLstPto2d;

                }          
            }
   
           }
    
       #endregion

       public void draw(string iLayer)
       {
       
           //Logica Capas
           engCadNet.oLayer.vLayerOffOn(iLayer, false);
           engCadNet.oLayer.deleteByLayer(iLayer);

           Point3dCollection myLstPto = new Point3dCollection();

           foreach (oNode myNode in pPathSol)
           {
               myLstPto.Add(new Point3d(myNode.pNodePos.X,myNode.pNodePos.Y,0));
           }

           engCadNet.oLw.addLw2d(myLstPto, false, iLayer);
       }


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

                           AddEdgeVisible(myNodeOri, myPolObstaculo, false);
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
           int myNodeIdGrafo = mNodeGoal.pNodeIdGrafo+1;

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
       private List<Polyline> getLstPolFrmSs(SelectionSet iSsPol)
       {

           //Creo el listado de Obstaculos
           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
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

       }



       /// <summary>
       /// Añadir Edge Valido como Camino
       /// </summary>
       private void AddEdgeVisible(oNode iNodeOri, Polyline iPolObstaculo, bool iIsDiagonalPoligono)
       {
           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {

               for (int i = 0; i < iPolObstaculo.NumberOfVertices; i++)
               {

                   Point2d myPtoObs = iPolObstaculo.GetPoint2dAt(i);

                   //Creo la Linea 
                   Line myLine = oLine.addLine2d(iNodeOri.pNodePos, myPtoObs, mLayerGV);

                   //Determino si la linea No intercepta con nada

                   bool myLineIsCamino = isCaminoSinObstaculos(myLine, iIsDiagonalPoligono);


                   if (myLineIsCamino)
                   {
          
                       oNode myNodeFin = getNode(iPolObstaculo.Handle.Value, i + 1);

                       mLstEdge.Add(new oEdge(iNodeOri,myNodeFin,myLine.Length));


                       //Si es Nodo Inicio-Fin Añado el Camino Inverso
                       if (iNodeOri.pNodeIdGrafo == 0 | iNodeOri.pNodeIdGrafo == 1)
                       {
                           mLstEdge.Add(new oEdge(myNodeFin, iNodeOri, myLine.Length));

                           oLine.addLine2d(myNodeFin.pNodePos, iNodeOri.pNodePos, mLayerGV);
                       }

                   }
                   else
                   {
                       oTools.deleteEntidad(myLine.ObjectId);
                   }

               }

           }


       }


       /// <summary>
       /// Añadir Edge Valido como Camino
       /// </summary>
       private void AddEdgeVisible(oNode iNodeOri, oNode iNodeFin, bool iIsDiagonalPoligono)
       {

           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {

               //Creo la Linea 
               Line myLine1 = oLine.addLine2d(iNodeOri.pNodePos, iNodeFin.pNodePos, mLayerGV);

               //Determino si la linea No intercepta con nada
               bool myLineIsCamino = isCaminoSinObstaculos(myLine1, iIsDiagonalPoligono);

               if (myLineIsCamino)
               {
                   mLstEdge.Add(new oEdge(iNodeOri, iNodeFin, myLine1.Length));
               }
               else
               {
                   oTools.deleteEntidad(myLine1.ObjectId);
               }

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

               oNode myNodeIni = getNode(iPolObstaculo.Handle.Value, i+1);

           
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
                             
                       Point2d myPtoMid = oTools.getMidPoint(myNodeIni.pNodePos, myNodeNext.pNodePos);

                       //Determino si es una Diagonal Exterior
                       //Compruebo que el Punto Medio sea Exterior a la Polilinea
                       bool myIsDiagonalInterior = oPolygon.isPointInPolygon(iPolObstaculo, myPtoMid.X, myPtoMid.Y);



                       if (!myIsDiagonalInterior)
                       {
                           AddEdgeVisible(myNodeIni,myNodeNext,true);
                       }


                   }


               }

           }


       }


       /// <summary>
       /// Determina si una Linea, esta Libre de Obstaculos
       /// </summary>
       private bool isCaminoSinObstaculos(Line iLine, bool iIsDiagonalPoligono)
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


           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {
               using (Transaction tr = oCadManager.StartTransaction())
               {


                   //Determino la interseccion de la linea con todos los obstaculos
                   foreach (Polyline myPolObs in mLstObstacle)
                   {

                       myLstPtoInter.Clear();

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





       private void addText(int iIdIni, Point2d iPIni, int iIdFin, Point2d iPFin)
       {

           Point2d myMid = oTools.getMidPoint(iPIni,iPFin);

           oText.addText2D(iIdIni.ToString() + "-" + iIdFin.ToString(), myMid.X, myMid.Y, 0.25, 0, "_Tadil_OK");
 
       }

   }




 #region "INTERNAL CLASS"
 internal  class oEdge
   {
       private oNode _origin;
       private oNode _destination;
       private double _distance;

       /// <summary>
       /// Konstruktor
       /// </summary>
       /// <param name="origin">Startknoten</param>
       /// <param name="destination">Zielknoten</param>
       /// <param name="distance">Distanz</param>
       public oEdge(oNode origin, oNode destination, double distance)
       {
           this._origin = origin;
           this._destination = destination;
           this._distance = distance;
       }

       /// <summary>
       /// Startknoten
       /// </summary>
       public oNode Origin
       {
           get { return _origin; }
           set { _origin = value; }
       }

       /// <summary>
       /// Zielknoten
       /// </summary>
       public oNode Destination
       {
           get { return _destination; }
           set { _destination = value; }
       }

       /// <summary>
       /// Distanz
       /// </summary>
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
      
          :base(iNodeGrafoId,iNodePos)
      
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
       public void calculateDistance(oNode start)
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
                   //Obtengo los Hijos del NodoOrigen
                   foreach (oNode myNodeHijo in getNeighbors(myNodeOrigen))
                   {
                       double myDistOrigen;
                       double myDistanciaNodoOrigen = Dist[myNodeOrigen.pNodeIdGrafo];
                       double myDistanciaNodoOrigenHijo = getDistanceBetween(myNodeOrigen, myNodeHijo);

                       myDistOrigen = myDistanciaNodoOrigen + myDistanciaNodoOrigenHijo;


                       if (myDistOrigen < Dist[myNodeHijo.pNodeIdGrafo])
                       {
                           Dist[myNodeHijo.pNodeIdGrafo] = myDistOrigen;
                           Previous[myNodeHijo.pNodeIdGrafo] = myNodeOrigen;                       
                       }                                            
                   }

                   Basis.Remove(myNodeOrigen);
               }
           }
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








   

