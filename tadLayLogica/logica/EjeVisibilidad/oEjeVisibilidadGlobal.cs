using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLogica.EjeVisibilidad;
using Autodesk.AutoCAD.DatabaseServices;
using tadLayShare.puntos;
using Autodesk.AutoCAD.Geometry;
using engCadNet;
using tadLayLogica.zonaGis;
using System.Collections;
using tadLayLogica.logica.EjeBasicoNew;
using tadLayLogica.estudioTipo;
using tadLayLan;
using tadLayShare;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Windows.Forms;
using System.Diagnostics;

namespace tadLayLogica.logica.EjeVisibilidad
{
    public class oEjeVisibilidadGlobal
    {
        #region "FIELDS PRIVADOS"

        ///// <summary>
        ///// Nodo de Inicio
        ///// </summary>
        //oNodeCad mNodeStart;
        ///// <summary>
        ///// Nodo de Llegada
        ///// </summary>
        //oNodeCad mNodeGoal;
        ///// <summary>
        ///// Listado de Obstaculos
        ///// </summary>
        //List<Polyline> mLstObstacle = new List<Polyline>();
        ///// <summary>
        ///// Listado de Nodos del Grafo
        ///// </summary>
        //List<oNodeCad> mLstNode = new List<oNodeCad>();
        ///// <summary>
        ///// Listado de Diagonales del Grafo
        ///// </summary>
        //List<oEdge> mLstEdge = new List<oEdge>();
        ///// <summary>
        ///// Solucion al Algortimo
        ///// </summary>
        //List<oNode> mPathSol = new List<oNode>();
        ///// <summary>
        ///// Capa donde guardar las lineas del Grafo de Visibilidad
        ///// </summary>
        //string mLayerGV = string.Empty;
        /// <summary>
        /// Separacion entre los nodos
        /// </summary>
        /// 
        Point3d mStart;
        Point3d mGoal;
        Point3d mStart_ext;
        Point3d mGoal_ext;
        double mSeparacion;
        List<oNode> mPathSol = null;
        double? mPendienteMaxCota = null;
        double? mPendienteMaxTrian = null;
        oEjeBasicoSolucion mEjeBasicoSol = null;


        #endregion


        #region "CONSTRUCTOR"
        public oEjeVisibilidadGlobal(double iP0x, double iP0y, double iP1x, double iP1y, double iSeparacion, double? pendienteMaxCota,
            double? pendienteMaxTrian, oEjeBasicoSolucion iEjeBasicoSol, bool dibujar,bool preciso=false)
        {
            if (oSingletonTerreno.getInstance.tipo == 1)
            {
                //oSingletonPuntosTerreno.getInstance.Cargar_MDT();
            }
            else if (oSingletonTerreno.getInstance.tipo == 2)
            {
                //oSingletonPuntosTerreno.getInstance.Cargar_MDT();
            }
            else if (oSingletonTerreno.getInstance.tipo == 3)
            {
                //oSingletonPuntosTerrenoASC.getInstance.Cargar_MDT();
            }
            else
            {
                //oSingletonPuntosTerreno.getInstance.Cargar_MDT();
            }
            oSingletonPuntosTerreno.getInstance.Set_Preciso(preciso);
            oSingletonPuntosTerrenoASC.getInstance.Set_Preciso(preciso);

            Application.DoEvents();
            //Creamos el Tramo de Salida
            oTramoEjeBasico tramoSalida = oFactoryTramoSalidaLlegada.createTramoSalidaLlegada(true, 0, 0, iEjeBasicoSol.ptoSalida, iEjeBasicoSol.roadDesign, iEjeBasicoSol.roadPendientes, iEjeBasicoSol.estudioData, iEjeBasicoSol.abanicoDesign.tramoAbanicoDiscretizacion, false);

            //Creamoe el Tramo de Llegada
            oTramoEjeBasico tramoLlegada = oFactoryTramoSalidaLlegada.createTramoSalidaLlegada(false, 0, 0, iEjeBasicoSol.ptoLlegada, iEjeBasicoSol.roadDesign, iEjeBasicoSol.roadPendientes, iEjeBasicoSol.estudioData, iEjeBasicoSol.abanicoDesign.tramoAbanicoDiscretizacion, true);

            mEjeBasicoSol = iEjeBasicoSol;

            double[] miPunto1 = new double[2];
            if (tramoSalida != null)
            {
                miPunto1[0] = tramoSalida.P2.X;
                miPunto1[1] = tramoSalida.P2.Y; ;
            }
            else
            {
                miPunto1[0] = iP0x;
                miPunto1[1] = iP0y;
            }
            //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(miPunto1);
            double? miCota = GetZ(miPunto1);
            mStart = new Point3d(miPunto1[0], miPunto1[1], (double)miCota);
            Point2d myStart = new Point2d(miPunto1[0], miPunto1[1]);


            double[] miPunto2 = new double[2];
            if (tramoLlegada != null)
            {
                miPunto2[0] = tramoLlegada.P2.X;
                miPunto2[1] = tramoLlegada.P2.Y; ;
            }
            else
            {
                miPunto2[0] = iP1x;
                miPunto2[1] = iP1y;
            }
            //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(miPunto2);
            double? miCota2 = GetZ(miPunto2);
            mGoal = new Point3d(miPunto2[0], miPunto2[1], (double)miCota2);
            Point2d myGoal = new Point2d(miPunto2[0], miPunto2[1]);



            double miLongitud = myStart.GetDistanceTo(myGoal);
            mSeparacion = miLongitud / (int)(miLongitud / iSeparacion);

            Point3d p1 = new Point3d(mStart.X, mStart.Y,0);
            Point3d p2 = new Point3d(mGoal.X, mGoal.Y, 0);
            for (int i=0;i<4;i++)
            {
                p2=ExtenderRecta(p1,p2, mSeparacion);
            }
            for (int i = 0; i < 4; i++)
            {
                p1 = ExtenderRecta(p2, p1, mSeparacion);
            }
            mStart_ext = p1;
            mGoal_ext = p2;
            mPendienteMaxCota = pendienteMaxCota;
            mPendienteMaxTrian = pendienteMaxTrian;
        }
        #endregion


        #region "metodos publicos"
        /// <summary>
        /// CALCULAR EJE VISIBILIDAD
        /// </summary>
        public void calcularEjeVisibilidad(List<Polyline> iLstPolilineasObstaculos, string iLayerDrawGrafoVisibilidad,double angulomin, bool dibujar,bool preciso=false)
        {
            int miNumFilas, miNumColumnas, miPosFilaCentral;
            double miVelocidad = mEjeBasicoSol.roadDesign.Vp;
            double miPendienteMaxTriang = 0.2;
            //Creo la matriz de nodos --> Comprueba si estan dentro de ZNP, en ese caso los pone como no validos
            oSingletonPuntosTerreno.getInstance.Set_Preciso(preciso);
            oSingletonPuntosTerrenoASC.getInstance.Set_Preciso(preciso);

            double R = mEjeBasicoSol.roadDesign.Rp;
            double clmin = Math.Sqrt(12 * Math.Pow(R, 3)) / R;

            //var envolvente =Crear2();

            bool anguloautomatico = true;
            if (angulomin >= 0)
            {
                anguloautomatico = false;
            }


            oNodoGlobal[,] miMatriz = getLstNode(out miPosFilaCentral);

            oConexionGlobal.SetUpObject(mEjeBasicoSol.roadDesign, mEjeBasicoSol.roadPendientes, mEjeBasicoSol.abanicoDesign, mEjeBasicoSol.estudioData);


            if (mPendienteMaxCota != null)
            {
                //filtrar por cota TODO si lo marca el usuario
                filtarPuntosValidosPorCota(miVelocidad, (double)mPendienteMaxCota, ref miMatriz);
            }

            if (mPendienteMaxTrian != null)
            {
                //filtrar por pendiente TODO si lo marca el usuario
                filtarPuntosValidosPorPendiente((double)mPendienteMaxTrian, ref miMatriz);
            }

            for (int i = 0; i < miMatriz.GetLength(0); i++)
            {
                for (int j = 0; j < miMatriz.GetLength(1); j++)
                {
                    if (miMatriz[i, j] != null)
                    {
                        double[] miPunto = { miMatriz[i, j].mNodo.X, miMatriz[i, j].mNodo.Y };
                        if (miMatriz[i, j].mIsValido)
                        {
                            /*
                            * Cambios para que no pinte las capas **juanma**
                            */
                            if (dibujar)
                            {
                                oMTexto.addMText2D("SI: " + i + ", " + j, miPunto, 10, 0, "0");
                            }
                            // 
                        }
                        else
                        {
                            /*
                             * Cambios para que no pinte las capas **juanma**
                             */
                            if (dibujar)
                            {
                                oMTexto.addMText2D("NO: " + i + ", " + j, miPunto, 10, 0, "0");
                            }
                            //
                        }
                    }
                }
            }


            List<oNode> miNodesD = new List<oNode>();
            oNodeCad miNodoEntrada = new oNodeCad(0, new Point2d(mStart.X, mStart.Y), 0, 0);
            oNodeCad miNodoSalida = new oNodeCad(1, new Point2d(mGoal.X, mGoal.Y), 0, 0);
            miNodesD.Add(miNodoEntrada);
            miNodesD.Add(miNodoSalida);
            int id = 2;

            for (int i = 0; i < miMatriz.GetLength(0); i++)
            {
                for (int j = 0; j < miMatriz.GetLength(1); j++)
                {
                    oNodoGlobal miNodo = miMatriz[i, j];
                    if (miNodo != null)
                    {
                        if ((Math.Round(miNodo.mNodo.X) == Math.Round(mStart.X)) && (Math.Round(miNodo.mNodo.Y) == Math.Round(mStart.Y)))
                        {
                            miNodo.mNodoDijkstra = miNodoEntrada;
                        }
                        else if ((Math.Round(miNodo.mNodo.X) == Math.Round(mGoal.X)) && (Math.Round(miNodo.mNodo.Y) == Math.Round(mGoal.Y)))
                        {
                            miNodo.mNodoDijkstra = miNodoSalida;
                        }
                        else
                        {
                            miNodo.mNodoDijkstra = new oNodeCad(id, new Point2d(miNodo.mNodo.X, miNodo.mNodo.Y), 0, 0);
                            if (miNodo.mIsValido)
                            {
                                id++;
                                miNodesD.Add(miNodo.mNodoDijkstra);
                            }
                        }

                    }
                }

            }

            List<oConexionGlobal> misConexionesValidas = new List<oConexionGlobal>();
            List<oConexionGlobal> misConexionesNOValidas = new List<oConexionGlobal>();


            // Usamos ConcurrentBag para manejar las conexiones de manera segura en paralelo
            ConcurrentBag<oConexionGlobal> conexionesValidasParalelas = new ConcurrentBag<oConexionGlobal>();
            ConcurrentBag<oConexionGlobal> conexionesNOValidasParalelas = new ConcurrentBag<oConexionGlobal>();

            /*Parallel.For(0, miMatriz.GetLength(0), i =>
            {
                for (int j = 0; j < miMatriz.GetLength(1); j++)
                {
                    if (miMatriz[i, j] != null)
                    {
                        if (miMatriz[i, j].mIsValido)
                        {
                            oArañaBusquedaGlobal miAraña = new oArañaBusquedaGlobal(miMatriz, i, j);
                            var tupla= miAraña.creaConexiones(miMatriz, mGoal);
                            misConexionesValidas.AddRange(tupla.Item1);
                            misConexionesNOValidas.AddRange(tupla.Item2);
                        }
                    }
                }
            });*/

            // Al final, fusionamos las colecciones paralelas con las originales
            /*misConexionesValidas.AddRange(conexionesValidasParalelas);
            misConexionesNOValidas.AddRange(conexionesNOValidasParalelas);*/

            Stopwatch miMedicion4 = new Stopwatch();
            miMedicion4.Start();
            for (int i = 0; i < miMatriz.GetLength(0); i++)
            {
                double porcentaje = (double)i / (double)miMatriz.GetLength(0) * (double)100;
                engCadNet.oCadManager.thisEditor.WriteMessage("\n Completado " + Math.Round(porcentaje, 2) + "%");
                Application.DoEvents();
                for (int j = 0; j < miMatriz.GetLength(1); j++)
                {
                    if (miMatriz[i, j] != null)
                    {
                        if (miMatriz[i, j].mIsValido)
                        {
                            oArañaBusquedaGlobal miAraña = new oArañaBusquedaGlobal(miMatriz, i, j);
                            miAraña.creaConexiones(miMatriz, ref misConexionesValidas, ref misConexionesNOValidas, mGoal, dibujar);
                        }
                    }
                }
            }
            engCadNet.oCadManager.thisEditor.WriteMessage("\n Completado " + 100 + "%");
            Application.DoEvents();

            List<oEdge> miEdgesD = createEdges(misConexionesValidas, mEjeBasicoSol);

            miMedicion4.Stop();
            oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion4.Elapsed.TotalMinutes);

            foreach (oConexionGlobal miConexion in misConexionesValidas)
            {
                /*
                * Cambios para que no pinte las capas **juanma**
                */
                if (dibujar)
                {
                    engCadNet.oMTexto.addMText2D(miConexion.ToString(), miConexion.mTramo.P2.toArray3d(), 0.15, 0, oTadil.data.Layer.visibilidadGrafo.name);
                }
                //
            }

            try
            {


                if (miEdgesD.Count > 2)
                {

                    Dijkstra miDijkstra = new Dijkstra(miEdgesD, miNodesD);


                    //Indico el Nodo de Inicio
                    miDijkstra.calculateDistance(miNodoEntrada, angulomin, clmin, R, anguloautomatico);

                    //Indico el Nodo de Llegada
                    mPathSol = miDijkstra.getPathTo(miNodoSalida);
                }
                else if (miEdgesD.Count == 2)
                {
                    mPathSol = new List<oNode>();
                    if ((miEdgesD[0].Destination.pNodePos.X == mGoal.X) && (miEdgesD[0].Destination.pNodePos.Y == mGoal.Y))
                    {
                        if ((miEdgesD[0].Origin.pNodePos.X == mStart.X) && (miEdgesD[0].Origin.pNodePos.Y == mStart.Y))
                        {
                            mPathSol.Add(miEdgesD[0].Origin);
                            mPathSol.Add(miEdgesD[0].Destination);
                        }
                    }
                    if ((miEdgesD[1].Destination.pNodePos.X == mGoal.X) && (miEdgesD[1].Destination.pNodePos.Y == mGoal.Y))
                    {
                        if ((miEdgesD[1].Origin.pNodePos.X == mStart.X) && (miEdgesD[1].Origin.pNodePos.Y == mStart.Y))
                        {
                            mPathSol.Add(miEdgesD[1].Origin);
                            mPathSol.Add(miEdgesD[1].Destination);
                        }
                    }

                }
                else
                {
                    mPathSol = new List<oNode>();
                }
            }
            catch (Exception)
            {

                oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionPrimariaNotFound);

            }

        }
        public double? GetZ_triangulo(double[] punto, Triangulo t)
        {

            try
            {
                if (oSingletonTerreno.getInstance.tipo == 1)
                {
                    return (double)oSingletonTerreno.getInstance.getZFromTriang(punto[0], punto[1], t);
                }
                else if (oSingletonTerreno.getInstance.tipo == 2)
                {
                    return (double)oSingletonPuntosTerreno.getInstance.getZFromTriang(punto[0], punto[1], t);
                }
                else if (oSingletonTerreno.getInstance.tipo == 3)
                {
                    return (double)oSingletonPuntosTerrenoASC.getInstance.getZFromTriang(punto[0], punto[1], t);
                }
                else
                {
                    return (double)oSingletonTerreno.getInstance.getZFromTriang(punto[0], punto[1], t);
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Triangulo GetTriangulo(double[] punto)
        {
            if (oSingletonTerreno.getInstance.tipo == 1)
            {
                return oSingletonTerreno.getInstance.getTrianguloInside(punto[0], punto[1]);
            }
            else if (oSingletonTerreno.getInstance.tipo == 2)
            {
                return oSingletonPuntosTerreno.getInstance.getTrianguloInside(punto[0], punto[1]);
            }
            else if (oSingletonTerreno.getInstance.tipo == 3)
            {
                return oSingletonPuntosTerrenoASC.getInstance.getTrianguloInside(punto[0], punto[1]);
            }
            else
            {
                return oSingletonTerreno.getInstance.getTrianguloInside(punto[0], punto[1]);
            }
            return null;
        }
        public double? GetZ(double[] punto,bool comprobar=false)
        {
            /*
             * Funciona correctamente el 3 para que busque dentro del rango de los puntos
             */
            if (oSingletonTerreno.getInstance.tipo == 1)
            {
                return oSingletonTerreno.getInstance.getZFromXY(punto);
            }
            else if (oSingletonTerreno.getInstance.tipo == 2)
            {
                return oSingletonPuntosTerreno.getInstance.GetZ(punto,comprobar);
            }
            else if (oSingletonTerreno.getInstance.tipo == 3)
            {
                return oSingletonPuntosTerrenoASC.getInstance.GetZ(punto);
            }
            else
            {
                return oSingletonTerreno.getInstance.getZFromXY(punto);
            }
            return -1;
        }

        public double? GetSlope(double[] punto)
        {
            if (oSingletonTerreno.getInstance.tipo == 1)
            {
                return oSingletonTerreno.getInstance.getSlopeFromXY(punto);
            }
            else if (oSingletonTerreno.getInstance.tipo == 2)
            {
                return oSingletonPuntosTerreno.getInstance.Slope(punto[0], punto[1]);
            }
            else if (oSingletonTerreno.getInstance.tipo == 3)
            {
                return oSingletonPuntosTerrenoASC.getInstance.Slope(punto[0],punto[1]);
            }
            else
            {
                return oSingletonTerreno.getInstance.getZFromXY(punto);
            }
            return -1;
        }

        #endregion


        #region "metodos privados"

        private List<oEdge> createEdges(List<oConexionGlobal> misConexionesValidas, oEjeBasicoSolucion iEjeBasicoSol)
        {


            List<oEdge> miEdgesD = new List<oEdge>();
            double maxlongitud = 0;
            double maxCoste = 0;
            double maxOrografia = 0;
            double minOrografia = double.MaxValue;



            foreach (oConexionGlobal micon in misConexionesValidas)
            {


                micon.mDistancia = micon.mTramo.P1.distTo2d(micon.mTramo.P2);
                micon.mCoste = micon.mTramo.valoracionCosteImplantacionTramo() / micon.mDistancia;

                //micon.mOrografia = (micon.mTramo.valoracionPendienteTramo() / micon.mTramo.seccion.Count) / micon.mDistancia;//Math.Abs(micon.mPuntoMayor.Z - micon.mPuntoMenor.Z) / micon.mTramo.P1.distTo2d(micon.mTramo.P2);

                double sumaValoraciones = 0;
                foreach (oSeccionEjeBasico miSeccion in micon.mTramo.seccion)
                {
                    sumaValoraciones = sumaValoraciones + miSeccion.valoracionSlopeUnitaria();
                }
                micon.mOrografia = sumaValoraciones / micon.mTramo.seccion.Count;

                if (maxlongitud < micon.mDistancia) maxlongitud = micon.mDistancia;
                if (maxCoste < micon.mCoste) maxCoste = micon.mCoste;
                if (maxOrografia < micon.mOrografia) maxOrografia = micon.mOrografia;
                //if (minOrografia > micon.mOrografia) minOrografia = micon.mOrografia;

            }



            Point2d miGoal2d = new Point2d(this.mGoal.X, this.mGoal.Y);
            foreach (oConexionGlobal micon in misConexionesValidas)
            {

                micon.mValCoste = 10 * micon.mCoste / maxCoste;

                micon.mValDistancia = 10 * micon.mDistancia / maxlongitud;

                micon.mValOrografia = maxOrografia - micon.mOrografia;

                double costeGlobal = micon.mValCoste * iEjeBasicoSol.tramosValoracion.valoracionCosteImplantacionPC / 100;

                double valDistGlobal = micon.mValDistancia * iEjeBasicoSol.tramosValoracion.valoracionDistanciaPC / 100;

                double valOroGlobal = micon.mValOrografia * iEjeBasicoSol.tramosValoracion.valoracionPendientePC / 100;

                double notaGlobal = costeGlobal + valDistGlobal + valOroGlobal;

                micon.mConexionDijkstra = new oEdge(micon.mConexionDijkstra.Origin, micon.mConexionDijkstra.Destination, notaGlobal);

                miEdgesD.Add(micon.mConexionDijkstra);
                miEdgesD.Add(new oEdge(micon.mConexionDijkstra.Destination, micon.mConexionDijkstra.Origin, notaGlobal));
            }

            return miEdgesD;
        }
        /// <summary>
        /// Obtener el Listado de Nodos del Grafo
        /// Se crea otro para que los puntos que se buscan sean en toda la cartografia y no solo entre punto inicio y punto fin
        /// </summary>
        /// <param name="iStart">Pto Salida ID = 0</param>
        /// <param name="iGoal">Pto Llegada ID = 1</param>
        private oNodoGlobal[,] getLstNode(out int miPosFilaCentral)
        {

            oNodoGlobal[,] myLstNode;


            //inicializo los datos 
            double miDistancia = 0;
            Line miLine = new Line(new Point3d(mStart_ext.X, mStart_ext.Y, 0), new Point3d(mGoal_ext.X, mGoal_ext.Y, 0));
            int miNumeroColumnas = (int)(miLine.Length / mSeparacion) + 1;

            //No olvidar sumar una fila para los nodos centrales numerosMaxFilas = miMaxNumeroDeNodosDerecha + miMaxNumeroDeNodosIzquierda + 1(central)
            int miMaxNumeroDeNodosDerecha = 0;
            int miMaxNumeroDeNodosIzquierda = 0;

            //Guarda los nodos correspondientes a cada columna
            List<oNodoGlobal>[] misNodosPorColumna = new List<oNodoGlobal>[miNumeroColumnas];

            //Guarda la posición del nodo central para cada una de las columnas
            int[] miArrayPosNodoCentral = new int[miNumeroColumnas];

            //para cada columna, generarla y comprobar el numero de nodos que contiene
            for (int i = 0; i < miNumeroColumnas; i++)
            {
                miArrayPosNodoCentral[i] = generaColumna(miDistancia, out misNodosPorColumna[i], miLine, (i == 0), (i == miNumeroColumnas - 1), miMaxNumeroDeNodosDerecha, miMaxNumeroDeNodosIzquierda);

                miDistancia = miDistancia + mSeparacion;
                int miNumNodosIzquierda = miArrayPosNodoCentral[i];
                int miNumNodosDerecha = misNodosPorColumna[i].Count - (miNumNodosIzquierda + 1);

                if (miNumNodosIzquierda > miMaxNumeroDeNodosIzquierda) miMaxNumeroDeNodosIzquierda = miNumNodosIzquierda;
                if (miNumNodosDerecha > miMaxNumeroDeNodosDerecha) miMaxNumeroDeNodosDerecha = miNumNodosDerecha;
            }

            //Guarda la posición de la fila central dentro de la nueva matriz
            miPosFilaCentral = miMaxNumeroDeNodosIzquierda;

            //numero de filas
            int miNumFilas = miMaxNumeroDeNodosIzquierda + 1 + miMaxNumeroDeNodosDerecha;


            //Creo la matriz
            myLstNode = new oNodoGlobal[miNumFilas, miNumeroColumnas];


            for (int i = 0; i < miNumeroColumnas; i++)
            {
                int miPosCentralColi = miArrayPosNodoCentral[i];
                int miPosAux = 0;

                //Añadimos a la matriz el nodo central de esta columna
                myLstNode[miPosFilaCentral, i] = misNodosPorColumna[i][miPosCentralColi];

                //Añadimos a la matriz los nodos de la derecha de esta columna
                miPosAux = miPosFilaCentral;
                for (int iDere = miPosCentralColi + 1; iDere < misNodosPorColumna[i].Count; iDere++)
                {
                    miPosAux++;
                    myLstNode[miPosAux, i] = misNodosPorColumna[i][iDere];
                }

                //Añadimos a la matriz los nodos de la izquierda de esta columna
                miPosAux = miPosFilaCentral - miPosCentralColi - 1;
                for (int iIzq = 0; iIzq < miPosCentralColi; iIzq++)
                {
                    miPosAux++;
                    myLstNode[miPosAux, i] = misNodosPorColumna[i][iIzq];
                }

                //Se Supone que el resto de valores se quedaran a null (comprobar)

            }

            return myLstNode;
        }
        private oNodoGlobal[,] getLstNode_old(out int miPosFilaCentral)
        {

            oNodoGlobal[,] myLstNode;


            //inicializo los datos 
            double miDistancia = 0;
           
            Line miLine = new Line(new Point3d(mStart.X, mStart.Y, 0), new Point3d(mGoal.X, mGoal.Y, 0));
            int miNumeroColumnas = (int)(miLine.Length / mSeparacion) + 1;

            //No olvidar sumar una fila para los nodos centrales numerosMaxFilas = miMaxNumeroDeNodosDerecha + miMaxNumeroDeNodosIzquierda + 1(central)
            int miMaxNumeroDeNodosDerecha = 0;
            int miMaxNumeroDeNodosIzquierda = 0;

            //Guarda los nodos correspondientes a cada columna
            List<oNodoGlobal>[] misNodosPorColumna = new List<oNodoGlobal>[miNumeroColumnas];

            //Guarda la posición del nodo central para cada una de las columnas
            int[] miArrayPosNodoCentral = new int[miNumeroColumnas];

            //para cada columna, generarla y comprobar el numero de nodos que contiene
            for (int i = 0; i < miNumeroColumnas; i++)
            {
                miArrayPosNodoCentral[i] = generaColumna(miDistancia, out misNodosPorColumna[i], miLine, (i == 0), (i == miNumeroColumnas - 1), miMaxNumeroDeNodosDerecha, miMaxNumeroDeNodosIzquierda);

                miDistancia = miDistancia + mSeparacion;
                int miNumNodosIzquierda = miArrayPosNodoCentral[i];
                int miNumNodosDerecha = misNodosPorColumna[i].Count - (miNumNodosIzquierda + 1);

                if (miNumNodosIzquierda > miMaxNumeroDeNodosIzquierda) miMaxNumeroDeNodosIzquierda = miNumNodosIzquierda;
                if (miNumNodosDerecha > miMaxNumeroDeNodosDerecha) miMaxNumeroDeNodosDerecha = miNumNodosDerecha;
            }

            //Guarda la posición de la fila central dentro de la nueva matriz
            miPosFilaCentral = miMaxNumeroDeNodosIzquierda;

            //numero de filas
            int miNumFilas = miMaxNumeroDeNodosIzquierda + 1 + miMaxNumeroDeNodosDerecha;


            //Creo la matriz
            myLstNode = new oNodoGlobal[miNumFilas, miNumeroColumnas];


            for (int i = 0; i < miNumeroColumnas; i++)
            {
                int miPosCentralColi = miArrayPosNodoCentral[i];
                int miPosAux = 0;

                //Añadimos a la matriz el nodo central de esta columna
                myLstNode[miPosFilaCentral, i] = misNodosPorColumna[i][miPosCentralColi];

                //Añadimos a la matriz los nodos de la derecha de esta columna
                miPosAux = miPosFilaCentral;
                for (int iDere = miPosCentralColi + 1; iDere < misNodosPorColumna[i].Count; iDere++)
                {
                    miPosAux++;
                    myLstNode[miPosAux, i] = misNodosPorColumna[i][iDere];
                }

                //Añadimos a la matriz los nodos de la izquierda de esta columna
                miPosAux = miPosFilaCentral - miPosCentralColi - 1;
                for (int iIzq = 0; iIzq < miPosCentralColi; iIzq++)
                {
                    miPosAux++;
                    myLstNode[miPosAux, i] = misNodosPorColumna[i][iIzq];
                }

                //Se Supone que el resto de valores se quedaran a null (comprobar)

            }

            return myLstNode;
        }
        public static Point3d ExtenderRecta(Point3d p1, Point3d p2, double distancia)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;
            double longitud = Math.Sqrt(dx * dx + dy * dy);

            if (longitud < 1e-12)
                throw new ArgumentException("Los dos puntos son iguales. No se puede definir una recta.");

            // Vector unitario
            double ux = dx / longitud;
            double uy = dy / longitud;

            // Nuevo punto a 'distancia' más allá de p2
            double x3 = p2.X + ux * distancia;
            double y3 = p2.Y + uy * distancia;

            // Si también quieres una Z aproximada (en la misma pendiente)
            double dz = p2.Z - p1.Z;
            double uz = dz / longitud;
            double z3 = p2.Z + uz * distancia;

            return new Point3d(x3, y3, z3);
        }
        private int generaColumna(double iDist, out List<oNodoGlobal> iNodos, Line iLinePerpendicular, bool isNodoEntrada, bool isNodoSalida, int maxDer, int maxIzq)
        {
            int miPosNodoCentral = -1;
            iNodos = new List<oNodoGlobal>();
            List<oNodoGlobal> iNodosDer = new List<oNodoGlobal>();
            List<oNodoGlobal> iNodosIzq = new List<oNodoGlobal>();
            double miOffset = mSeparacion;
            bool derechaDentro = true;
            bool izquierdaDentro = true;

            oNodoGlobal miNodoCentral;
            double[] miPosXYNodoCentral = oLine.getPointAtDist(iDist, iLinePerpendicular);
            /*
                 * Se cambia ya que lo que busca es el triangulo del punto **juanma**
                 */
            //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(miPosXYNodoCentral);
            //Triangulo miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPosXYNodoCentral[0], miPosXYNodoCentral[1]);
            //La segunda opcion esta con ****
            //****Triangulo miTriangulo = GetTriangulo(miPosXYNodoCentral);
            if (isNodoEntrada)
            {
                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPosXYNodoCentral[0], miPosXYNodoCentral[1], miTriangulo);
                //****double? miCota = GetZ_triangulo(miPosXYNodoCentral, miTriangulo);
                double? miCota = GetZ(miPosXYNodoCentral,true);
                miNodoCentral = new oNodoGlobal(true, new Point3d(miPosXYNodoCentral[0], miPosXYNodoCentral[1], (double)miCota));
            }
            else if (isNodoSalida)
            {
                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPosXYNodoCentral[0], miPosXYNodoCentral[1], miTriangulo);
                //****double? miCota = GetZ_triangulo(miPosXYNodoCentral, miTriangulo);
                double? miCota = GetZ(miPosXYNodoCentral, true);
                miNodoCentral = new oNodoGlobal(false, new Point3d(miPosXYNodoCentral[0], miPosXYNodoCentral[1], (double)miCota));
            }
            else
            {

                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPosXYNodoCentral[0], miPosXYNodoCentral[1], miTriangulo);
                //****double? miCota = GetZ_triangulo(miPosXYNodoCentral, miTriangulo);
                double? miCota = (double)GetZ(miPosXYNodoCentral, true);
                if (miCota == null || double.IsNaN(miCota.Value))
                {
                    miNodoCentral = null;
                }
                else
                {
                    if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPosXYNodoCentral[0], miPosXYNodoCentral[1]))
                    {
                        miNodoCentral = new oNodoGlobal(new Point3d(miPosXYNodoCentral[0], miPosXYNodoCentral[1], (double)miCota));
                    }
                    else
                    {
                        miNodoCentral = new oNodoGlobal(new Point3d(miPosXYNodoCentral[0], miPosXYNodoCentral[1], (double)miCota), false);
                    }
                }
            }


            //Calculo los puntos por la izquierda
            oLine.getPointAtLocation(iDist, miOffset, false, iLinePerpendicular);

            miOffset = mSeparacion;

            //miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPosXYNodoCentral[0], miPosXYNodoCentral[1]);
            //****miTriangulo = GetTriangulo(miPosXYNodoCentral);
            while ((izquierdaDentro) || (iNodosIzq.Count < maxIzq))
            {

                double[] miPunto = oLine.getPointAtLocation(iDist, miOffset, false, iLinePerpendicular);
                /*
                 * Se cambia ya que lo que busca es el triangulo del punto **juanma**
                 */
                //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(miPunto);
                //miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPunto[0], miPunto[1]);
                //****miTriangulo = GetTriangulo(miPunto);
                /*if (miTriangulo != null)
                {
                    miTriangulo = oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(miPunto[0], miPunto[1], miTriangulo);
                }
                else
                {
                    miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPunto[0], miPunto[1]);
                   
                }*/
                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPunto[0], miPunto[1], miTriangulo);
                double? miCota = null;
                /****if (miTriangulo!=null)
                {
                    miCota = GetZ_triangulo(miPunto, miTriangulo);
                }****/
                miCota = GetZ(miPunto, true);
                if (miCota == null || double.IsNaN(miCota.Value))
                {
                    izquierdaDentro = false;
                    iNodosIzq.Add(null);
                    miOffset = miOffset + mSeparacion;
                }
                else
                {
                    izquierdaDentro = true;
                    if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPunto[0], miPunto[1]))
                    {
                        Point3d miNodo = new Point3d(miPunto[0], miPunto[1], (double)miCota);
                        iNodosIzq.Add(new oNodoGlobal(miNodo));
                    }
                    else
                    {
                        Point3d miNodo = new Point3d(miPunto[0], miPunto[1], (double)miCota);
                        iNodosIzq.Add(new oNodoGlobal(miNodo, false));
                    }
                    miOffset = miOffset + mSeparacion;
                }

            }


            //Caluculo los puntos por la derecha
            miOffset = mSeparacion;
            //miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPosXYNodoCentral[0], miPosXYNodoCentral[1]);
            //****miTriangulo = GetTriangulo(miPosXYNodoCentral);
            while ((derechaDentro) || (iNodosDer.Count < maxDer))
            {
                double[] miPunto = oLine.getPointAtLocation(iDist, miOffset, true, iLinePerpendicular);
                /*
                 * Se cambia ya que lo que busca es el triangulo del punto **juanma**
                 */
                //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(miPunto);
                //miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPunto[0], miPunto[1]);
                //****miTriangulo = GetTriangulo(miPunto);
                /*if (miTriangulo != null)
                {
                    miTriangulo = oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(miPunto[0], miPunto[1], miTriangulo);
                }
                else
                {
                    miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPunto[0], miPunto[1]);
                    
                }*/
                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPunto[0], miPunto[1], miTriangulo);
                double? miCota = null;
                /****if (miTriangulo != null)
                {
                    miCota = GetZ_triangulo(miPunto, miTriangulo);
                }****/
                miCota = GetZ(miPunto, true);
                if (miCota == null || double.IsNaN(miCota.Value))
                {
                    derechaDentro = false;
                    iNodosDer.Add(null);
                    miOffset = miOffset + mSeparacion;
                }
                else
                {
                    derechaDentro = true;
                    if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPunto[0], miPunto[1]))
                    {
                        Point3d miNodo = new Point3d(miPunto[0], miPunto[1], (double)miCota);
                        iNodosDer.Add(new oNodoGlobal(miNodo));
                    }
                    else
                    {
                        Point3d miNodo = new Point3d(miPunto[0], miPunto[1], (double)miCota);
                        iNodosDer.Add(new oNodoGlobal(miNodo, false));
                    }
                    miOffset = miOffset + mSeparacion;
                }

            }

            //ordeno los puntos de izquierda a derecha para crear la salida
            //primero añado los puntos de la izquierda desde el ultimo al primero
            for (int i = iNodosIzq.Count - 1; i >= 0; i--)
            {
                iNodos.Add(iNodosIzq[i]);
            }

            //añado el punto central
            iNodos.Add(miNodoCentral);

            //el numero de la posición del nodo central
            miPosNodoCentral = iNodos.Count - 1;

            //añado los puntos de la derecha desde el primero al ultimo
            for (int i = 0; i < iNodosDer.Count; i++)
            {
                iNodos.Add(iNodosDer[i]);
            }
            return miPosNodoCentral;
        }

        private void filtarPuntosValidosPorCota(double iVelocidad, double iPendMaxima, ref oNodoGlobal[,] miMatriz)
        {
            Point2d myStart2d = new Point2d(mStart.X, mStart.Y);
            Point2d myGoal2d = new Point2d(mGoal.X, mGoal.Y);
            double p = 0;

            if (iVelocidad < 40) p = 0.4;
            if ((iVelocidad >= 40) && (iVelocidad < 80)) p = 0.3;
            if ((iVelocidad >= 80) && (iVelocidad <= 120)) p = 0.2;
            if (iVelocidad > 120) p = 0.1;

            for (int i = 0; i < miMatriz.GetLength(0); i++)
            {
                for (int j = 0; j < miMatriz.GetLength(1); j++)
                {
                    if (miMatriz[i, j] != null)
                    {
                        if (!((miMatriz[i, j].mNodo.X == myStart2d.X) && (miMatriz[i, j].mNodo.Y == myStart2d.Y)) && !((miMatriz[i, j].mNodo.X == myGoal2d.X) && (miMatriz[i, j].mNodo.Y == myGoal2d.Y)))
                        {
                            //comprobacion cota entrada
                            if (miMatriz[i, j].mIsValido)
                            {
                                double miDisToEntrada = myStart2d.GetDistanceTo(new Point2d(miMatriz[i, j].mNodo.X, miMatriz[i, j].mNodo.Y));
                                miDisToEntrada = miDisToEntrada * (1 + p);
                                double distanciaMinima = (miMatriz[i, j].mNodo.Z - mStart.Z) / iPendMaxima;
                                if (!(miDisToEntrada > distanciaMinima)) miMatriz[i, j].mIsValido = false;

                            }
                            //comprobacion cota salida
                            if (miMatriz[i, j].mIsValido)
                            {
                                double miDisToSalida = myGoal2d.GetDistanceTo(new Point2d(miMatriz[i, j].mNodo.X, miMatriz[i, j].mNodo.Y));
                                miDisToSalida = miDisToSalida * (1 + p);
                                double distanciaMinima = (miMatriz[i, j].mNodo.Z - mGoal.Z) / iPendMaxima;
                                if (!(miDisToSalida > distanciaMinima)) miMatriz[i, j].mIsValido = false;

                            }
                        }
                    }
                }
            }

        }

        private void filtarPuntosValidosPorPendiente(double iPendMaxima, ref oNodoGlobal[,] miMatriz)
        {
            Point2d myStart2d = new Point2d(mStart.X, mStart.Y);
            Point2d myGoal2d = new Point2d(mGoal.X, mGoal.Y);

            for (int i = 0; i < miMatriz.GetLength(0); i++)
            {
                for (int j = 0; j < miMatriz.GetLength(1); j++)
                {
                    if (miMatriz[i, j] != null)
                    {
                        if (!((miMatriz[i, j].mNodo.X == myStart2d.X) && (miMatriz[i, j].mNodo.Y == myStart2d.Y)) && !((miMatriz[i, j].mNodo.X == myGoal2d.X) && (miMatriz[i, j].mNodo.Y == myGoal2d.Y)))
                        {
                            //comprobacion pendiente del triangulo
                            if (miMatriz[i, j].mIsValido)
                            {
                                double[] mipunto = new double[2];
                                mipunto[0] = miMatriz[i, j].mNodo.X;
                                mipunto[1] = miMatriz[i, j].mNodo.Y;
                                //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(mipunto);
                                //Triangulo miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miMatriz[i, j].mNodo.X, miMatriz[i, j].mNodo.Y);
                                double[] punto = new double[2];
                                punto[0] = miMatriz[i, j].mNodo.X;
                                punto[1] = miMatriz[i, j].mNodo.Y;
                                //****Triangulo miTriangulo = GetTriangulo(punto);


                                //****if (miTriangulo.getPendienteMaxima > iPendMaxima) miMatriz[i, j].mIsValido = false;
                                if (GetSlope(punto) > iPendMaxima) miMatriz[i, j].mIsValido = false;
                            }
                        }
                    }
                }
            }

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
           /* if (mPathSol != null && mPathSol.Count > 1)
            {
                return null;
            }*/
            foreach (oNode myNode in pPathSol)
            {
                myLstPto.Add(new Point3d(myNode.pNodePos.X, myNode.pNodePos.Y, 0));
            }

            return engCadNet.oLw.addLw2d(myLstPto, false, iLayerEjeVisibilidad.name, iLayerEjeVisibilidad.color);
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
    }

    internal class oNodoGlobal
    {
        public Point3d mNodo { get; set; }
        public bool mIsValido { get; set; }
        bool isEntrada = false;
        bool isSalida = false;
        public oNodeCad mNodoDijkstra { get; set; }

        public oNodoGlobal(Point3d iNodo, bool isValido)
        {
            mNodo = iNodo;
            mIsValido = isValido;
        }


        public oNodoGlobal(Point3d iNodo)
        {
            mNodo = iNodo;
            mIsValido = true;
        }

        public oNodoGlobal(bool iEntradaTrue_SalidaFalse, Point3d iNodo)
        {
            mNodo = iNodo;
            mIsValido = true;
            if (iEntradaTrue_SalidaFalse)
            {
                isEntrada = true;
            }
            else
            {
                isSalida = true;
            }
        }
    }

    internal class oConexionGlobal
    {
        public Point3d mPuntoMenor { get; set; }
        public Point3d mPuntoMayor { get; set; }
        public bool mIsValido { get; set; }
        public double mPuntuacion { get; set; }
        public string hashCode { get; set; }
        public oEdge mConexionDijkstra { get; set; }

        public oTramoAvanceCorto mTramo { get; set; }


        private static oRoadDes mRoadDesign = null;
        private static oRoadPendientes mRoadPendiente = null;

        private static oAbanicoDesign mAbanicoDesign = null;
        private static IEstudio mEstudioDatos = null;

        public double mDistancia;
        public double mCoste;
        public double mOrografia;

        public double mValDistancia;
        public double mValCoste;
        public double mValOrografia;



        public oConexionGlobal(oNodoGlobal iPunto1, oNodoGlobal iPunto2)
        {
            int miComp = compareTo(iPunto1.mNodo, iPunto2.mNodo);
            if (miComp == -1)
            {
                mPuntoMenor = iPunto1.mNodo;
                mPuntoMayor = iPunto2.mNodo;
                this.mConexionDijkstra = new oEdge(iPunto1.mNodoDijkstra, iPunto2.mNodoDijkstra, mPuntoMenor.DistanceTo(mPuntoMayor));
            }
            else if (miComp == 1)
            {
                mPuntoMenor = iPunto2.mNodo;
                mPuntoMayor = iPunto1.mNodo;
                this.mConexionDijkstra = new oEdge(iPunto2.mNodoDijkstra, iPunto1.mNodoDijkstra, mPuntoMenor.DistanceTo(mPuntoMayor));
            }
            else
            {
                throw new Exception("No se puede crear una conexion si los puntos son iguales");
            }
            hashCode = mPuntoMenor.X + "_" + mPuntoMenor.Y + "_" + mPuntoMayor.X + "_" + mPuntoMayor.Y;
        }


        public static void SetUpObject(oRoadDes iRoadDesign, oRoadPendientes iRoadPendiente, oAbanicoDesign iDataAbanico, IEstudio iEstudioDatos)
        {
            mRoadDesign = iRoadDesign;
            mRoadPendiente = iRoadPendiente;
            mAbanicoDesign = iDataAbanico;
            mEstudioDatos = iEstudioDatos;

        }

        public bool validarConexion(ref List<oConexionGlobal> iConexionesValidas, ref List<oConexionGlobal> iConexionesNOValidas, bool dibujar)
        {
            if (iConexionesValidas.Contains(this))
            {
                mIsValido = true;
            }
            else if (iConexionesNOValidas.Contains(this))
            {
                mIsValido = false;
            }
            else
            {
                //Validar
                mIsValido = true;
                mTramo = new oTramoAvanceCorto();
                mTramo.P1 = new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z);
                mTramo.P2 = new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z);
                //(oTramoAbanico)new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                mTramo.idTramo = 0;
                mTramo.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;
                //mTramo.ptoTarget 
                mTramo.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);



                mIsValido = mTramo.isTramoValido;

                if (mIsValido)
                {
                    oTramoEjeBasico miTramoEje = new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                    miTramoEje.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);
                    miTramoEje.idTramo = 0;
                    if (dibujar)
                    {
                        miTramoEje.drawTramo2D(oTadil.data.Layer.visibilidadGrafo.name);
                    }



                    iConexionesValidas.Add(this);
                }
                else
                {
                    iConexionesNOValidas.Add(this);
                }

            }
            return mIsValido;
        }
        public bool validarConexion(ref ConcurrentBag<oConexionGlobal> iConexionesValidas, ref ConcurrentBag<oConexionGlobal> iConexionesNOValidas)
        {
            if (iConexionesValidas.Contains(this))
            {
                mIsValido = true;
            }
            else if (iConexionesNOValidas.Contains(this))
            {
                mIsValido = false;
            }
            else
            {
                //Validar
                mIsValido = true;
                mTramo = new oTramoAvanceCorto();
                mTramo.P1 = new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z);
                mTramo.P2 = new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z);
                //(oTramoAbanico)new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                mTramo.idTramo = 0;
                mTramo.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;
                //mTramo.ptoTarget 
                mTramo.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);



                mIsValido = mTramo.isTramoValido;

                if (mIsValido)
                {
                    oTramoEjeBasico miTramoEje = new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                    miTramoEje.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);
                    miTramoEje.idTramo = 0;
                    miTramoEje.drawTramo2D(oTadil.data.Layer.visibilidadGrafo.name);

                    lock (this)
                    {
                        iConexionesValidas.Add(this);
                    }

                }
                else
                {
                    lock (this)
                    {
                        iConexionesNOValidas.Add(this);
                    }
                }

            }
            return mIsValido;
        }

        public override string ToString()
        {
            StringBuilder miStr = new StringBuilder();
            miStr.AppendLine("P1: " + this.mTramo.P1.ToString());
            miStr.AppendLine("P2 : " + this.mTramo.P2.ToString());
            miStr.AppendLine(" distancia : " + this.mDistancia.ToString());
            miStr.AppendLine(" orografia : " + this.mOrografia.ToString());
            miStr.AppendLine(" coste : " + this.mCoste.ToString());
            miStr.AppendLine("Valoracion distancia : " + this.mValDistancia.ToString());
            miStr.AppendLine("Valoracion orografia : " + this.mValOrografia.ToString());
            miStr.AppendLine("Valoracion coste : " + this.mValCoste.ToString());
            return miStr.ToString();

        }

        public override bool Equals(object obj)
        {
            try
            {
                oConexionGlobal miCon = (oConexionGlobal)obj;

                if (miCon.hashCode == this.hashCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int compareTo(Point3d iP1, Point3d iP2)
        {
            int miComp = 2;
            if (iP1.X < iP2.X)
            {
                miComp = -1;
            }
            else if (iP1.X > iP2.X)
            {
                miComp = 1;
            }
            else
            {
                if (iP1.Y < iP2.Y)
                {
                    miComp = -1;
                }
                else if (iP1.Y > iP2.Y)
                {
                    miComp = 1;
                }
                else
                {
                    miComp = 0;
                }
            }
            return miComp;
        }


    }

    internal class oArañaBusquedaGlobal
    {
        List<oConexionGlobal> mConexiones { get; set; }
        public oNodoGlobal mPuntoCentral { get; set; }
        int mI;
        int mJ;

        public oArañaBusquedaGlobal(oNodoGlobal[,] miMatriz, int iPosINodoCentral, int iPosJNodoCentral)
        {
            mConexiones = new List<oConexionGlobal>();
            mPuntoCentral = miMatriz[iPosINodoCentral, iPosJNodoCentral];
            mI = iPosINodoCentral;
            mJ = iPosJNodoCentral;
        }

        public void creaConexiones(oNodoGlobal[,] miMatriz, ref List<oConexionGlobal> iConexionesValidas, ref List<oConexionGlobal> iConexionesNOValidas, Point3d iGoal, bool dibujar)
        {
            //creo una a una todas las coneciones, si son diferente a null las añado a mi lista de conexiones (ya estan añadidas en iConexionesValidas)
            oConexionGlobal miConexion;
            /*
             * Se comenta el añadido a las mConexiones para intentar que no se pinten **juanma**
             */
            miConexion = creaConexion(miMatriz, 0, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 0, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, 0, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, 0, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);


            miConexion = creaConexion(miMatriz, 1, 2, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 1, -2, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 2, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 2, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, -1, 2, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, -2, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, -2, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -2, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            /*
             * Se comenta para ver velocidad de ejecucion **juanma**
             */
            oCadManager.thisEditor.UpdateScreen();



        }
        public Tuple<List<oConexionGlobal>, List<oConexionGlobal>> creaConexiones(oNodoGlobal[,] miMatriz, Point3d iGoal)
        {
            //creo una a una todas las coneciones, si son diferente a null las añado a mi lista de conexiones (ya estan añadidas en iConexionesValidas)
            oConexionGlobal miConexion;
            /*
             * Se comenta el añadido a las mConexiones para intentar que no se pinten **juanma**
             */
            List<oConexionGlobal> iConexionesValidas_ = new List<oConexionGlobal>();
            List<oConexionGlobal> iConexionesNOValidas_ = new List<oConexionGlobal>();
            miConexion = creaConexion(miMatriz, 0, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 0, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, 0, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, 0, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);


            miConexion = creaConexion(miMatriz, 1, 2, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 1, -2, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 2, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 2, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, -1, 2, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, -2, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, -2, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -2, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            /*
             * Se comenta para ver velocidad de ejecucion **juanma**
             */
            //oCadManager.thisEditor.UpdateScreen();
            return Tuple.Create(iConexionesValidas_, iConexionesNOValidas_);


        }
        private oConexionGlobal creaConexion(oNodoGlobal[,] miMatriz, int iVariacionPosI, int iVariacionPosJ, ref List<oConexionGlobal> iConexionesValidas, ref List<oConexionGlobal> iConexionesNOValidas, Point3d iGoal, bool dibujar = true)
        {
            oConexionGlobal miConexion = null;

            int miFilas = miMatriz.GetLength(0);
            int miColumnas = miMatriz.GetLength(1);

            int miPosIDestino = mI + iVariacionPosI;
            int miPosJDestino = mJ + iVariacionPosJ;

            bool conexionEncontrada = false;

            while (!conexionEncontrada)
            {
                if ((miPosIDestino >= 0) && (miPosIDestino < miFilas))
                {
                    if ((miPosJDestino >= 0) && (miPosJDestino < miColumnas))
                    {
                        oNodoGlobal miDestino = miMatriz[miPosIDestino, miPosJDestino];
                        if (miDestino == null)
                        {
                            conexionEncontrada = true;
                        }
                        else
                        {
                            if (miDestino.mIsValido)
                            {
                                if (!oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso(new oP2d(mPuntoCentral.mNodo.X, mPuntoCentral.mNodo.Y), new oP2d(miDestino.mNodo.X, miDestino.mNodo.Y)))
                                {
                                    miConexion = new oConexionGlobal(mPuntoCentral, miDestino);
                                    if (miConexion.validarConexion(ref iConexionesValidas, ref iConexionesNOValidas, dibujar))
                                    {
                                        if (miConexion.mTramo != null)
                                        {
                                            miConexion.mTramo.ptoTarget = new oP2d(iGoal.X, iGoal.Y);
                                        }
                                        return miConexion;
                                    }
                                }
                                else
                                {
                                    conexionEncontrada = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        conexionEncontrada = true;
                    }
                }
                else
                {
                    conexionEncontrada = true;
                }
                miPosIDestino = miPosIDestino + iVariacionPosI;
                miPosJDestino = miPosJDestino + iVariacionPosJ;

            }

            return miConexion;
        }
        private oConexionGlobal creaConexion(oNodoGlobal[,] miMatriz, int iVariacionPosI, int iVariacionPosJ, ref ConcurrentBag<oConexionGlobal> iConexionesValidas, ref ConcurrentBag<oConexionGlobal> iConexionesNOValidas, Point3d iGoal)
        {
            oConexionGlobal miConexion = null;

            int miFilas = miMatriz.GetLength(0);
            int miColumnas = miMatriz.GetLength(1);

            int miPosIDestino = mI + iVariacionPosI;
            int miPosJDestino = mJ + iVariacionPosJ;

            bool conexionEncontrada = false;

            while (!conexionEncontrada)
            {
                if ((miPosIDestino >= 0) && (miPosIDestino < miFilas))
                {
                    if ((miPosJDestino >= 0) && (miPosJDestino < miColumnas))
                    {
                        oNodoGlobal miDestino = miMatriz[miPosIDestino, miPosJDestino];
                        if (miDestino == null)
                        {
                            conexionEncontrada = true;
                        }
                        else
                        {
                            if (miDestino.mIsValido)
                            {
                                if (!oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso(new oP2d(mPuntoCentral.mNodo.X, mPuntoCentral.mNodo.Y), new oP2d(miDestino.mNodo.X, miDestino.mNodo.Y)))
                                {
                                    miConexion = new oConexionGlobal(mPuntoCentral, miDestino);
                                    if (miConexion.validarConexion(ref iConexionesValidas, ref iConexionesNOValidas))
                                    {
                                        if (miConexion.mTramo != null)
                                        {
                                            miConexion.mTramo.ptoTarget = new oP2d(iGoal.X, iGoal.Y);
                                        }
                                        return miConexion;
                                    }
                                }
                                else
                                {
                                    conexionEncontrada = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        conexionEncontrada = true;
                    }
                }
                else
                {
                    conexionEncontrada = true;
                }
                miPosIDestino = miPosIDestino + iVariacionPosI;
                miPosJDestino = miPosJDestino + iVariacionPosJ;

            }

            return miConexion;
        }


    }


}
