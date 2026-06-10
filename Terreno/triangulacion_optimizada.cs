using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using tadLayShare;
using tadLayShare.puntos;
using TriangulationTool;
using TriangulationTool.Data;
using Triangle = tadLayShare.Triangulo;

namespace Terrenos
{
    /// <summary>
    /// Versión optimizada de Triangulacion con:
    ///  - Construcción de adyacencias por hash de aristas O(n)
    ///  - Eliminación de triángulos largos en paralelo
    ///  - Índice espacial en rejilla uniforme para búsquedas rápidas de triángulo por punto
    ///  - Reasignación de TrianguloCentral por hoja usando el índice espacial (sin bucles anidados)
    /// Mantiene la API pública de la clase original.
    /// </summary>
    public class Triangulacion_old
    {
        #region "Variables privadas"

        private String mName = "";

        private Triangle[] mTriangulos;
        private Punto3d[] mNodos;
        private List<int>[] mAdyacenciasTriangulos;
        private List<int>[] mTriangulosDelNodo;
        private Region mRegiones;
        private List<System.Collections.Hashtable> mTriangulosPendienteMayor = new List<System.Collections.Hashtable>();

        public TriangulationResult TriangulationResult;

        // Índice espacial (rejilla uniforme)
        private GridIndex _grid;

        #endregion

        #region "Constructores"

        public Triangulacion_old(Terreno iTerreno, double longMax, String name, ref bool isEliminados, int nodosXHoja = 500)
        {
            mName = name;
            TriangularPuntos(iTerreno);


            Punto3d[] miLimiteTerreno = iTerreno.limitesTerreno;
            var hojas = new List<Region>();
            mRegiones = new Region(miLimiteTerreno[3].coordenadaX, miLimiteTerreno[3].coordenadaY,
            miLimiteTerreno[0].coordenadaX, miLimiteTerreno[0].coordenadaY, ref hojas, iTerreno.getPuntosTerreno,
            null, "", nodosXHoja);


            CreateTriangulationStructure(hojas);
            isEliminados = eliminarTriangulosLong(longMax);
            RecalculateHojas(hojas);
        }


        public Triangulacion_old(InfoTriangulacionNew info)
        {
            mName = info.Name;


            mTriangulos = new Triangle[info.getInfoTriangulos.Count];
            mNodos = new Punto3d[info.numNodos];
            mTriangulosDelNodo = new List<int>[info.numNodos];
            mAdyacenciasTriangulos = new List<int>[info.getInfoTriangulos.Count];


            foreach (InfoTrianguloNew infoTriangulo in info.getInfoTriangulos)
            {
                if (infoTriangulo == null) continue;
                var verticeA = new Punto3d(infoTriangulo.getVertAX, infoTriangulo.getVertAY, infoTriangulo.getVertAZ,
                infoTriangulo.getIndexVA);
                var verticeB = new Punto3d(infoTriangulo.getVertBX, infoTriangulo.getVertBY, infoTriangulo.getVertBZ,
                infoTriangulo.getIndexVB);
                var verticeC = new Punto3d(infoTriangulo.getVertCX, infoTriangulo.getVertCY, infoTriangulo.getVertCZ,
                infoTriangulo.getIndexVC);


                var miTriangulo = new Triangle(verticeA, verticeB, verticeC, infoTriangulo.getIndex);


                mTriangulos[miTriangulo.getIndex] = miTriangulo;
                var adyacentes = new List<int>();
                if (infoTriangulo.getAd1 != -1) adyacentes.Add(infoTriangulo.getAd1);
                if (infoTriangulo.getAd2 != -1) adyacentes.Add(infoTriangulo.getAd2);
                if (infoTriangulo.getAd3 != -1) adyacentes.Add(infoTriangulo.getAd3);
                mAdyacenciasTriangulos[miTriangulo.getIndex] = adyacentes;


                AddVerticeInTriangulosDelNodo(verticeA, miTriangulo);
                AddVerticeInTriangulosDelNodo(verticeB, miTriangulo);
                AddVerticeInTriangulosDelNodo(verticeC, miTriangulo);


                if (mNodos[verticeA.index] == null) mNodos[verticeA.index] = verticeA;
                if (mNodos[verticeB.index] == null) mNodos[verticeB.index] = verticeB;
                if (mNodos[verticeC.index] == null) mNodos[verticeC.index] = verticeC;
            }

            
            // Restaurado como estaba en tu código original
            mRegiones = new Region(info, this);
            TriangulationResult = new TriangulationResult(mNodos.ToList(), mTriangulos.ToList());


            BuildGridIndex();
        }

        #endregion

        #region "Triangulación e índice"

        private void TriangularPuntos(Terreno iTerreno)
        {
            var pointCloud = new PointCloud();
            foreach (var punto in iTerreno.getPuntosTerreno)
                pointCloud.AddPoint(new Punto3d(punto.coordenadaX, punto.coordenadaY, punto.coordenadaZ));


            var triangulation = new TriangulationManager(pointCloud);
            TriangulationResult = triangulation.Triangulate();
        }

        private void CreateTriangulationStructure(List<Region> hojas)
        {
            var trianglesList = TriangulationResult.GetAllTriangles();
            var vertexList = TriangulationResult.GetAllVertices();

            // Reservas
            mTriangulos = new Triangle[trianglesList.Count];
            mNodos = new Punto3d[vertexList.Count];
            mTriangulosDelNodo = new List<int>[vertexList.Count];
            mAdyacenciasTriangulos = new List<int>[trianglesList.Count];

            // Copia de nodos (paralelo)
            Parallel.For(0, vertexList.Count, j =>
            {
                var v = vertexList[j];
                mNodos[j] = new Punto3d(v.coordenadaX, v.coordenadaY, v.coordenadaZ, j);
            });

            // Inicializar contenedores thread-safe por vértice
            var perVertexBags = new ConcurrentBag<int>[vertexList.Count];
            for (int i = 0; i < perVertexBags.Length; i++) perVertexBags[i] = new ConcurrentBag<int>();

            // Cargar triángulos en paralelo y preparar edge map para adyacencias
            var edgeMap = new ConcurrentDictionary<Edge, int>(Environment.ProcessorCount, trianglesList.Count * 3);

            Parallel.For(0, trianglesList.Count, i =>
            {
                var t = trianglesList[i];
                mTriangulos[i] = t;

                // Vincular triángulo a vértices
                perVertexBags[t.getVerticeA.index].Add(i);
                perVertexBags[t.getVerticeB.index].Add(i);
                perVertexBags[t.getVerticeC.index].Add(i);

                // Registrar aristas normalizadas
                edgeMap.TryAdd(new Edge(t.getVerticeA.index, t.getVerticeB.index), i);
                edgeMap.TryAdd(new Edge(t.getVerticeA.index, t.getVerticeC.index), i);
                edgeMap.TryAdd(new Edge(t.getVerticeB.index, t.getVerticeC.index), i);
            });

            // Convertir bolsas a listas
            mTriangulosDelNodo = perVertexBags
                .Select(b => b != null ? b.ToList() : new List<int>())
                .ToArray();

            // Construir adyacencias en O(n) usando hash de aristas
            var ady = new List<int>[trianglesList.Count];
            for (int i = 0; i < ady.Length; i++) ady[i] = new List<int>(3);

            Parallel.For(0, trianglesList.Count, i =>
            {
                var t = mTriangulos[i];
                if (t == null) return;

                AddAdjacency(edgeMap, ady, i, t.getVerticeA.index, t.getVerticeB.index);
                AddAdjacency(edgeMap, ady, i, t.getVerticeA.index, t.getVerticeC.index);
                AddAdjacency(edgeMap, ady, i, t.getVerticeB.index, t.getVerticeC.index);
            });
            mAdyacenciasTriangulos = ady;

            // Índice espacial por rejilla (para queries rápidas)
            BuildGridIndex();

            // Asignar TrianguloCentral por hoja usando el índice (sin paralelismo interno por triángulo)
            Parallel.ForEach(hojas, hoja =>
            {
                hoja.TrianguloCentral = getTrianguloMasCercano(hoja.Centro) ?? _grid.FirstTriangleContainingPoint(hoja.Centro, mTriangulos);
            });
        }

        private static void AddAdjacency(ConcurrentDictionary<Edge, int> edgeMap, List<int>[] ady, int triIndex, int a, int b)
        {
            var e = new Edge(a, b);
            if (edgeMap.TryGetValue(e, out int other))
            {
                if (other != triIndex)
                {
                    lock (ady[triIndex]) ady[triIndex].Add(other);
                    lock (ady[other]) ady[other].Add(triIndex);
                }
            }
        }

        private void BuildGridIndex()
        {
            // Calcular bounding box
            double minX = double.PositiveInfinity, minY = double.PositiveInfinity;
            double maxX = double.NegativeInfinity, maxY = double.NegativeInfinity;

            foreach (var n in mNodos)
            {
                if (n == null) continue;
                if (n.coordenadaX < minX) minX = n.coordenadaX;
                if (n.coordenadaY < minY) minY = n.coordenadaY;
                if (n.coordenadaX > maxX) maxX = n.coordenadaX;
                if (n.coordenadaY > maxY) maxY = n.coordenadaY;
            }

            // Dimensión de la rejilla: proporcional a la raíz del nº de triángulos (cap a 512)
            int triCount = mTriangulos.Count(t => t != null);
            int gridDim = Math.Max(32, Math.Min(512, (int)Math.Ceiling(Math.Sqrt(triCount) / 2.0)));
            _grid = new GridIndex(minX, minY, maxX, maxY, gridDim, gridDim);

            // Insertar triángulos por su AABB en la rejilla
            Parallel.For(0, mTriangulos.Length, i =>
            {
                var t = mTriangulos[i];
                if (t == null) return;
                var ax = t.getVerticeA.coordenadaX; var ay = t.getVerticeA.coordenadaY;
                var bx = t.getVerticeB.coordenadaX; var by = t.getVerticeB.coordenadaY;
                var cx = t.getVerticeC.coordenadaX; var cy = t.getVerticeC.coordenadaY;

                double minTx = Math.Min(ax, Math.Min(bx, cx));
                double minTy = Math.Min(ay, Math.Min(by, cy));
                double maxTx = Math.Max(ax, Math.Max(bx, cx));
                double maxTy = Math.Max(ay, Math.Max(by, cy));

                _grid.InsertAabb(i, minTx, minTy, maxTx, maxTy);
            });
        }

        #endregion

        #region "Optimización eliminación triángulos largos"

        private bool eliminarTriangulosLong(double longMax)
        {
            double mediaDistancias = 0;
            var miTriangulosEliminar = new ConcurrentBag<Triangle>();

            // 1) Explorar en paralelo para marcar
            double sumaDistancias = 0;
            int contador = 0;

            Parallel.ForEach(mTriangulos, miTriangulo =>
            {
                if (miTriangulo == null) return;
                double distAB = miTriangulo.getVerticeA.distancia2d(miTriangulo.getVerticeB);
                double distAC = miTriangulo.getVerticeA.distancia2d(miTriangulo.getVerticeC);
                double distBC = miTriangulo.getVerticeB.distancia2d(miTriangulo.getVerticeC);

                if ((distAB > longMax) || (distAC > longMax) || (distBC > longMax))
                {
                    miTriangulosEliminar.Add(miTriangulo);
                }

                double promedio = (distAB + distAC + distBC) / 3.0;
                lock (miTriangulosEliminar) // reutilizamos el lock del bag
                {
                    sumaDistancias += promedio;
                    contador++;
                }
            });

            if (contador > 0)
                mediaDistancias = (sumaDistancias / contador) * 2;
            else
                mediaDistancias = 0;

            // mediaDistancias acumulada de forma segura
            mediaDistancias = BitwiseAdder.ToDouble(Interlocked.Read(ref BitwiseAdder.Accumulator));
            BitwiseAdder.Reset();

            mediaDistancias = (mediaDistancias / mTriangulos.Length) * 2;
            if (mediaDistancias >= longMax) return false;

            // 2) Quitar adyacencias
            foreach (var miTriangulo in miTriangulosEliminar)
            {
                var misAdyacentes = mAdyacenciasTriangulos[miTriangulo.getIndex];
                if (misAdyacentes == null) continue;

                var copiaAd = misAdyacentes.ToArray();
                foreach (var ad in copiaAd)
                {
                    var adjList = mAdyacenciasTriangulos[ad];
                    if (adjList != null) lock (adjList) adjList.Remove(miTriangulo.getIndex);
                }
                mAdyacenciasTriangulos[miTriangulo.getIndex] = null;
            }

            // 3) Eliminar triángulos y limpiar vértices huérfanos
            foreach (var miTriangulo in miTriangulosEliminar)
            {
                if (miTriangulo != null) removeTriangulo(miTriangulo);
            }

            var verticesEliminar = new ConcurrentBag<int>();
            Parallel.For(0, mNodos.Length, idx =>
            {
                var p = mNodos[idx];
                if (p == null) return;
                var triList = mTriangulosDelNodo[p.index];
                if (triList == null || triList.Count == 0) verticesEliminar.Add(p.index);
            });

            foreach (var vi in verticesEliminar)
            {
                mNodos[vi] = null;
                mTriangulosDelNodo[vi] = null;
            }

            // Reconstruir índice espacial tras la poda
            BuildGridIndex();
            return true;
        }

        private void removeTriangulo(Triangle iTrianguloA)
        {
            if (iTrianguloA == null) return;
            mTriangulos[iTrianguloA.getIndex] = null;

            int a = iTrianguloA.getVerticeA.index;
            int b = iTrianguloA.getVerticeB.index;
            int c = iTrianguloA.getVerticeC.index;

            var la = mTriangulosDelNodo[a]; if (la != null) lock (la) la.Remove(iTrianguloA.getIndex);
            var lb = mTriangulosDelNodo[b]; if (lb != null) lock (lb) lb.Remove(iTrianguloA.getIndex);
            var lc = mTriangulosDelNodo[c]; if (lc != null) lock (lc) lc.Remove(iTrianguloA.getIndex);
        }

        #endregion

        #region "Búsqueda de triángulos (acelerada)"

        private void RecalculateHojas(List<Region> hojas)
        {
            Parallel.ForEach(hojas, hoja =>
            {
                if (hoja.TrianguloCentral == null || mTriangulos[hoja.TrianguloCentral.getIndex] == null)
                {
                    hoja.TrianguloCentral = getTrianguloMasCercano(hoja.Centro) ?? _grid.FirstTriangleContainingPoint(hoja.Centro, mTriangulos);
                }
            });
        }

        public Triangle getTrianguloReg(Punto3d iPunto)
        {
            var miTrianguloCentro = mRegiones.getRegion(iPunto);
            return getTrianguloTriang(miTrianguloCentro, iPunto);
        }

        public Triangle getTrianguloTriang(Triangle iTriangulo, Punto3d iPunto)
        {
            if (iTriangulo != null && mTriangulos[iTriangulo.getIndex] != null && iTriangulo.isDentro(iPunto))
                return iTriangulo;

            // Buscar candidatos por celda
            foreach (int triIdx in _grid.GetCandidates(iPunto.coordenadaX, iPunto.coordenadaY))
            {
                var t = mTriangulos[triIdx];
                if (t != null && t.isDentro(iPunto)) return t;
            }

            // Fallback (muy raro): búsqueda global
            return getTriangulo(iPunto);
        }

        public Triangle getTriangulo(double iX, double iY)
        {
            Punto3d miPunto = new Punto3d(iX, iY, 0);
            // Consulta directa al índice espacial
            foreach (int triIdx in _grid.GetCandidates(iX, iY))
            {
                var t = mTriangulos[triIdx];
                if (t != null && t.isDentro(miPunto)) return t;
            }
            return null;
        }

        public Triangle getTriangulo(double iX, double iY, Triangle iTriangulo)
        {
            Punto3d miPunto = new Punto3d(iX, iY, 0);
            return getTrianguloTriang(iTriangulo, miPunto);
        }

        public Triangle getTriangulo(Punto3d iPunto)
        {
            // Último recurso: recorrer candidatos por vecindad ampliada
            foreach (int triIdx in _grid.GetNeighborhoodCandidates(iPunto.coordenadaX, iPunto.coordenadaY, 1))
            {
                var t = mTriangulos[triIdx];
                if (t != null && t.isDentro(iPunto)) return t;
            }
            return null;
        }

        public Triangle getTrianguloMasCercano(Punto3d iPunto)
        {
            // Buscar en celdas cercanas incrementando el radio
            for (int radius = 0; radius <= 2; radius++)
            {
                double best = double.MaxValue;
                Triangle bestT = null;
                foreach (int triIdx in _grid.GetNeighborhoodCandidates(iPunto.coordenadaX, iPunto.coordenadaY, radius))
                {
                    var t = mTriangulos[triIdx];
                    if (t == null) continue;
                    var center = t.getCentro;
                    double d = center.distancia2d(iPunto);
                    if (d < best)
                    {
                        best = d; bestT = t;
                    }
                }
                if (bestT != null) return bestT;
            }
            return null;
        }

        #endregion

        #region "API pública existente"
        public int getAdyacente(Triangulo iTriangulo, Punto3d miVerticeA, Punto3d miVerticeB)
        {
            var miTrianguloAdyacente = -1;
            var miAdyacencias = mAdyacenciasTriangulos[iTriangulo.getIndex];
            if (miAdyacencias != null)
            {
                foreach (int miTriangulo in miAdyacencias)
                {
                    bool isVerticeA = false;
                    bool isVerticeB = false;

                    int[] miNodos = new int[3];
                    miNodos[0] = mTriangulos[miTriangulo].getVerticeA.index;
                    miNodos[1] = mTriangulos[miTriangulo].getVerticeB.index;
                    miNodos[2] = mTriangulos[miTriangulo].getVerticeC.index;
                    foreach (int miNodo in miNodos)
                    {
                        if (miNodo.Equals(miVerticeA.index)) isVerticeA = true;
                        if (miNodo.Equals(miVerticeB.index)) isVerticeB = true;

                    }

                    if (isVerticeA && isVerticeB) miTrianguloAdyacente = miTriangulo;
                }
            }

            return miTrianguloAdyacente;
        }
        public List<Punto3d> calculaEnvolvente()
        {

            List<Punto3d> miPolEnvolvente = new List<Punto3d>();
            List<Triangulo> misTriangulos = getTriangulos;
            var misAdyacentes = getAdyacentes;

            Triangulo miPrimerTriangulo = misTriangulos[0];

            bool encontrado = false;
            int i = 0;
            while (!encontrado)
            {
                Triangulo miTriangulo = misTriangulos[i];
                i++;
                if (miTriangulo == null) continue;
                var misTriangAdyacentes = misAdyacentes[miTriangulo.getIndex];
                if (misTriangAdyacentes.Count == 2)
                {
                    miPrimerTriangulo = miTriangulo;
                    encontrado = true;
                }
            }

            Punto3d miPuntoA = miPrimerTriangulo.getVerticeA;
            Punto3d miPuntoB = miPrimerTriangulo.getVerticeB;
            Punto3d miPuntoC = miPrimerTriangulo.getVerticeC;

            var miTriangAdAB = getAdyacente(miPrimerTriangulo, miPuntoA, miPuntoB);
            var miTriangAdAC = getAdyacente(miPrimerTriangulo, miPuntoA, miPuntoC);
            var miTriangAdCB = getAdyacente(miPrimerTriangulo, miPuntoC, miPuntoB);

            Punto3d miVerticeBuscar = new Punto3d(0, 0, 0);

            if (miTriangAdAB == -1)
            {
                miPolEnvolvente.Add(miPuntoA);
                miPolEnvolvente.Add(miPuntoB);
                miVerticeBuscar = miPuntoB;
            }
            else if (miTriangAdAC == -1)
            {
                miPolEnvolvente.Add(miPuntoA);
                miPolEnvolvente.Add(miPuntoC);
                miVerticeBuscar = miPuntoC;
            }
            else if (miTriangAdCB == -1)
            {
                miPolEnvolvente.Add(miPuntoC);
                miPolEnvolvente.Add(miPuntoB);
                miVerticeBuscar = miPuntoB;
            }
            Triangulo miNextTriangulo = misTriangulos[0];
            Triangulo miAntTriangulo = misTriangulos[0];

            List<Triangulo> misTriangulosUtilizados = new List<Triangulo>();

            var misTriang = mTriangulosDelNodo[miVerticeBuscar.index];
            foreach (int miTrianIndex in misTriang)
            {
                Triangulo miTriangulo = mTriangulos[miTrianIndex];
                if (miTriangulo == null) continue;
                var misTriangAdyacentes = misAdyacentes[miTrianIndex];
                if (misTriangAdyacentes.Count < 3)
                {
                    miNextTriangulo = miTriangulo;
                }

            }
            while (miAntTriangulo == null || !miAntTriangulo.Equals(miNextTriangulo))
            {
                misTriangulosUtilizados.Add(miNextTriangulo);
                var misTriangAdyacentesNext = misAdyacentes[miNextTriangulo.getIndex];
                miAntTriangulo = miNextTriangulo;


                miPuntoA = miNextTriangulo.getVerticeA;
                miPuntoB = miNextTriangulo.getVerticeB;
                miPuntoC = miNextTriangulo.getVerticeC;

                miTriangAdAB = this.getAdyacente(miNextTriangulo, miPuntoA, miPuntoB);
                miTriangAdAC = this.getAdyacente(miNextTriangulo, miPuntoA, miPuntoC);
                miTriangAdCB = this.getAdyacente(miNextTriangulo, miPuntoC, miPuntoB);

                if (misTriangAdyacentesNext.Count == 1)
                {
                    if (miTriangAdAB != -1)
                    {
                        if (((miPuntoA.coordenadaX == miVerticeBuscar.coordenadaX) &&
                             (miPuntoA.coordenadaY == miVerticeBuscar.coordenadaY)))
                        {
                            miPolEnvolvente.Add(miPuntoC);
                            miPolEnvolvente.Add(miPuntoB);
                            miVerticeBuscar = miPuntoB;
                        }
                        else
                        {
                            miPolEnvolvente.Add(miPuntoC);
                            miPolEnvolvente.Add(miPuntoA);
                            miVerticeBuscar = miPuntoA;
                        }
                    }
                    else if (miTriangAdAC != -1)
                    {
                        if (((miPuntoA.coordenadaX == miVerticeBuscar.coordenadaX) &&
                             (miPuntoA.coordenadaY == miVerticeBuscar.coordenadaY)))
                        {
                            miPolEnvolvente.Add(miPuntoB);
                            miPolEnvolvente.Add(miPuntoC);
                            miVerticeBuscar = miPuntoC;
                        }
                        else
                        {
                            miPolEnvolvente.Add(miPuntoB);
                            miPolEnvolvente.Add(miPuntoA);
                            miVerticeBuscar = miPuntoA;
                        }
                    }
                    else if (miTriangAdCB != -1)
                    {
                        if (((miPuntoB.coordenadaX == miVerticeBuscar.coordenadaX) &&
                             (miPuntoB.coordenadaY == miVerticeBuscar.coordenadaY)))
                        {
                            miPolEnvolvente.Add(miPuntoA);
                            miPolEnvolvente.Add(miPuntoC);
                            miVerticeBuscar = miPuntoC;
                        }
                        else
                        {
                            miPolEnvolvente.Add(miPuntoA);
                            miPolEnvolvente.Add(miPuntoB);
                            miVerticeBuscar = miPuntoB;
                        }
                    }

                }
                else
                {
                    if (miTriangAdAB == -1)
                    {

                        if (((miPuntoA.coordenadaX == miVerticeBuscar.coordenadaX) &&
                             (miPuntoA.coordenadaY == miVerticeBuscar.coordenadaY)))
                        {
                            miPolEnvolvente.Add(miPuntoB);
                            miVerticeBuscar = miPuntoB;
                        }
                        else
                        {
                            miPolEnvolvente.Add(miPuntoA);
                            miVerticeBuscar = miPuntoA;
                        }
                    }
                    else if (miTriangAdAC == -1)
                    {
                        if (((miPuntoA.coordenadaX == miVerticeBuscar.coordenadaX) &&
                             (miPuntoA.coordenadaY == miVerticeBuscar.coordenadaY)))
                        {
                            miPolEnvolvente.Add(miPuntoC);
                            miVerticeBuscar = miPuntoC;
                        }
                        else
                        {
                            miPolEnvolvente.Add(miPuntoA);
                            miVerticeBuscar = miPuntoA;
                        }
                    }
                    else if (miTriangAdCB == -1)
                    {
                        if (((miPuntoB.coordenadaX == miVerticeBuscar.coordenadaX) &&
                             (miPuntoB.coordenadaY == miVerticeBuscar.coordenadaY)))
                        {
                            miPolEnvolvente.Add(miPuntoC);
                            miVerticeBuscar = miPuntoC;
                        }
                        else
                        {
                            miPolEnvolvente.Add(miPuntoB);
                            miVerticeBuscar = miPuntoB;
                        }
                    }

                }

                var misTriangVerticeBuscar = mTriangulosDelNodo[miVerticeBuscar.index];
                int numAdyacentes = int.MaxValue;
                foreach (var miTrianIndex in misTriangVerticeBuscar)
                {
                    Triangulo miTriangulo = mTriangulos[miTrianIndex];
                    if (miTriangulo == null) continue;
                    var misTriangAdyacentes = misAdyacentes[miTrianIndex];
                    if ((misTriangAdyacentes.Count < 3) && (!miTriangulo.Equals(miAntTriangulo)) &&
                        (!misTriangulosUtilizados.Contains(miTriangulo)))
                    {
                        if (misTriangAdyacentes.Count < numAdyacentes)
                        {
                            miNextTriangulo = miTriangulo;
                            numAdyacentes = misTriangAdyacentes.Count;
                        }

                    }

                }

            }

            return miPolEnvolvente;
        }
        public double? getCota(double iX, double iY)
        {
            double? miCota = null;
            Punto3d miPunto = new Punto3d(iX, iY, 0);
            Triangle miTrianguloC = getTrianguloReg(miPunto);
            if (miTrianguloC != null) miCota = miTrianguloC.getCota(iX, iY);
            return miCota;
        }

        public double? getCotaTriang(double iX, double iY, Triangle iTriangulo)
        {
            double? miCota = null;
            Punto3d miPunto = new Punto3d(iX, iY, 0);

            var miTrianguloC = getTrianguloTriang(iTriangulo, miPunto) ?? getTrianguloReg(miPunto);
            if (miTrianguloC != null) miCota = miTrianguloC.getCota(iX, iY);
            return miCota;
        }

        public double getPendiente(Triangle iTriangulo) => iTriangulo.getPendienteMaxima;

        public double getPendiente(double iX, double iY)
        {
            Punto3d miPunto = new Punto3d(iX, iY, 0);
            Triangle miTrianguloC = getTrianguloReg(miPunto);
            return miTrianguloC.getPendienteMaxima;
        }

        public List<Triangle> getTriangulos
        {
            get
            {
                List<Triangle> miLstTriangulos = new List<Triangle>(mTriangulos.Length);
                foreach (Triangle miTriangulo in mTriangulos) miLstTriangulos.Add(miTriangulo);
                return miLstTriangulos;
            }
        }

        public List<Punto3d> getNodos
        {
            get
            {
                List<Punto3d> miLstNodos = new List<Punto3d>(mNodos.Length);
                foreach (Punto3d miNodo in mNodos) miLstNodos.Add(miNodo);
                return miLstNodos;
            }
        }

        public Triangle[] getTriangulosArray => mTriangulos;
        public Region getRegiones => mRegiones;
        public List<int>[] getAdyacentes => mAdyacenciasTriangulos;
        public String Name => mName;

        public MemoryStream guardarTriangulacion()
        {
            InfoTriangulacionNew miInfo = new InfoTriangulacionNew(this);
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, miInfo);
            return stream;
        }

        public List<System.Collections.Hashtable> getTriangulosMaxPendiente() => mTriangulosPendienteMayor;

        public List<Triangle> getPuntosZonasMaxPendiente(double iPendienteMax)
        {
            List<Triangle> misTriangulos = clasificaTriangulos(iPendienteMax);
            return misTriangulos;
        }

        public List<double[]> getCotas(List<double[]> iListaPuntos)
        {
            List<double[]> miLista3d = new List<double[]>();
            foreach (double[] miPunto in iListaPuntos)
            {
                double z = getCota(miPunto[0], miPunto[1]).Value;
                miLista3d.Add(new double[] { miPunto[0], miPunto[1], z });
            }
            return miLista3d;
        }

        public List<Triangle> clasificaTriangulos(double iPendienteMax)
        {
            List<Triangle> misTriangulos = new List<Triangle>();
            foreach (Triangle miTrian in mTriangulos)
                if (miTrian != null && miTrian.getPendienteMaxima > iPendienteMax) misTriangulos.Add(miTrian);
            return misTriangulos;
        }

        public bool eliminarTriangulosLista(List<Punto3d> puntos)
        {
            List<Triangle> miTriangulosEliminar = new List<Triangle>();
            foreach (var punto in puntos)
            {
                var t = getTrianguloReg(punto) ?? getTriangulo(punto);
                if (t != null) miTriangulosEliminar.Add(t);
            }

            foreach (Triangle miTriangulo in miTriangulosEliminar)
            {
                var misAdyacentes = mAdyacenciasTriangulos[miTriangulo.getIndex];
                if (misAdyacentes == null) continue;

                var copiaAd = misAdyacentes.ToArray();
                foreach (var ad in copiaAd)
                {
                    var adjList = mAdyacenciasTriangulos[ad];
                    if (adjList != null) lock (adjList) adjList.Remove(miTriangulo.getIndex);
                }
                mAdyacenciasTriangulos[miTriangulo.getIndex] = null;
            }

            foreach (Triangle miTriangulo in miTriangulosEliminar)
                removeTriangulo(miTriangulo);

            var verticesEliminar = new List<int>();
            foreach (Punto3d miPunto in mNodos)
            {
                if (miPunto == null) continue;
                var triangulosVertice = mTriangulosDelNodo[miPunto.index];
                if (triangulosVertice == null || triangulosVertice.Count == 0)
                    verticesEliminar.Add(miPunto.index);
            }
            foreach (int miPunto in verticesEliminar)
            {
                mNodos[miPunto] = null;
                mTriangulosDelNodo[miPunto] = null;
            }

            RecalculateHojas(mRegiones.mHojas);
            BuildGridIndex();
            return true;
        }

        #endregion

        #region "Utilidades internas"

        private void AddVerticeInTriangulosDelNodo(Punto3d v1, Triangle triangulo)
        {
            if (mTriangulosDelNodo[v1.index] != null)
            {
                var misTriangulos = mTriangulosDelNodo[v1.index];
                misTriangulos.Add(triangulo.getIndex);
            }
            else
            {
                var misTriangulos = new List<int> { triangulo.getIndex };
                mTriangulosDelNodo[v1.index] = misTriangulos;
            }
        }

        // Estructura de arista normalizada para hash/igualdad
        private readonly struct Edge : IEquatable<Edge>
        {
            public readonly int A; public readonly int B;
            public Edge(int a, int b)
            {
                if (a < b) { A = a; B = b; }
                else { A = b; B = a; }
            }
            public bool Equals(Edge other) => A == other.A && B == other.B;
            public override bool Equals(object obj) => obj is Edge e && Equals(e);
            public override int GetHashCode() => (A * 397) ^ B;
        }

        // Rejilla uniforme de celdas con listas de índices de triángulo
        private class GridIndex
        {
            private readonly double _minX, _minY, _maxX, _maxY, _cellW, _cellH;
            private readonly int _cols, _rows;
            private readonly ConcurrentDictionary<int, ConcurrentBag<int>> _cells = new ConcurrentDictionary<int, ConcurrentBag<int>>();

            public GridIndex(double minX, double minY, double maxX, double maxY, int cols, int rows)
            {
                _minX = minX; _minY = minY; _maxX = maxX; _maxY = maxY; _cols = Math.Max(1, cols); _rows = Math.Max(1, rows);
                _cellW = (_maxX - _minX) / _cols;
                _cellH = (_maxY - _minY) / _rows;
                if (_cellW <= 0) _cellW = 1; if (_cellH <= 0) _cellH = 1;
            }

            private int Clamp(int v, int min, int max) => v < min ? min : (v > max ? max : v);

            private int CellIndex(int cx, int cy) => (cy * _cols) + cx;

            private void ToCellRange(double minx, double miny, double maxx, double maxy, out int cx0, out int cy0, out int cx1, out int cy1)
            {
                cx0 = Clamp((int)((minx - _minX) / _cellW), 0, _cols - 1);
                cy0 = Clamp((int)((miny - _minY) / _cellH), 0, _rows - 1);
                cx1 = Clamp((int)((maxx - _minX) / _cellW), 0, _cols - 1);
                cy1 = Clamp((int)((maxy - _minY) / _cellH), 0, _rows - 1);
            }

            public void InsertAabb(int triIndex, double minx, double miny, double maxx, double maxy)
            {
                ToCellRange(minx, miny, maxx, maxy, out int cx0, out int cy0, out int cx1, out int cy1);
                for (int cy = cy0; cy <= cy1; cy++)
                for (int cx = cx0; cx <= cx1; cx++)
                {
                    int idx = CellIndex(cx, cy);
                    var bag = _cells.GetOrAdd(idx, _ => new ConcurrentBag<int>());
                    bag.Add(triIndex);
                }
            }

            public IEnumerable<int> GetCandidates(double x, double y)
            {
                int cx = Clamp((int)((x - _minX) / _cellW), 0, _cols - 1);
                int cy = Clamp((int)((y - _minY) / _cellH), 0, _rows - 1);
                int idx = CellIndex(cx, cy);
                if (_cells.TryGetValue(idx, out var bag)) return bag;
                return new int[0]; // equivalente a Array.Empty<int>()
            }


            public IEnumerable<int> GetNeighborhoodCandidates(double x, double y, int radius)
            {
                int cx = Clamp((int)((x - _minX) / _cellW), 0, _cols - 1);
                int cy = Clamp((int)((y - _minY) / _cellH), 0, _rows - 1);
                int cx0 = Clamp(cx - radius, 0, _cols - 1);
                int cy0 = Clamp(cy - radius, 0, _rows - 1);
                int cx1 = Clamp(cx + radius, 0, _cols - 1);
                int cy1 = Clamp(cy + radius, 0, _rows - 1);
                var set = new HashSet<int>();
                for (int yy = cy0; yy <= cy1; yy++)
                for (int xx = cx0; xx <= cx1; xx++)
                {
                    int idx = CellIndex(xx, yy);
                    if (_cells.TryGetValue(idx, out var bag))
                        foreach (var v in bag) set.Add(v);
                }
                return set;
            }

            public Triangle FirstTriangleContainingPoint(Punto3d p, Triangle[] tris)
            {
                foreach (var idx in GetNeighborhoodCandidates(p.coordenadaX, p.coordenadaY, 1))
                {
                    var t = tris[idx];
                    if (t != null && t.isDentro(p)) return t;
                }
                return null;
            }
        }

        // Utilidad para acumular doubles en paralelo con Interlocked (sin bloquear)
        private static class BitwiseAdder
        {
            public static long Accumulator;
            public static long AverageToLong(double value)
            {
                long bits = BitConverter.DoubleToInt64Bits(value);
                Interlocked.Add(ref Accumulator, bits);
                return bits;
            }
            public static double ToDouble(long bitsSum) => BitConverter.Int64BitsToDouble(bitsSum);
            public static void Reset() => Interlocked.Exchange(ref Accumulator, 0);
        }

        #endregion
    }
}
