using System;
using System.Collections.Generic;
using System.Collections;
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
    public class Triangulacion
    {
        #region "Variables privadas"


        private String mName = "";

        private Triangulo[] mTriangulos;
        private Punto3d[] mNodos;
        private List<int>[] mAdyacenciasTriangulos;
        private List<int>[] mTriangulosDelNodo;
        private Region mRegiones;
        private List<Hashtable> mTriangulosPendienteMayor = new List<Hashtable>();


        public TriangulationResult TriangulationResult;




        #endregion

        #region "Constructores"
        
        public Triangulacion(Terreno iTerreno, double longMax, String name, ref bool isEliminados, int nodosXHoja=500)
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

        private void RecalculateHojas(List<Region> hojas)
        {
            Parallel.ForEach(hojas, (hoja) =>
            {
                if (hoja.TrianguloCentral == null || mTriangulos[hoja.TrianguloCentral.getIndex] == null)
                {
                    hoja.TrianguloCentral = getTrianguloMasCercano(hoja.Centro);
                }
            });
        }

        private void CreateTriangulationStructure(List<Region> hojas)
        {
            var trianglesList = TriangulationResult.GetAllTriangles();
            var vertexList = TriangulationResult.GetAllVertices();

            var hojasEncontradas = new List<Region>();
            mTriangulos = new Triangulo[trianglesList.Count];
            mNodos = new Punto3d[vertexList.Count];
            mTriangulosDelNodo = new List<int>[vertexList.Count];
            mAdyacenciasTriangulos = new List<int>[trianglesList.Count];




            for (int j = 0; j < vertexList.Count; j++)
            {
                var vertice = vertexList[j];
                var punto = new Punto3d(vertice.coordenadaX, vertice.coordenadaY, vertice.coordenadaZ, j);
                mNodos[j] = punto;

            }


            for (int i = 0; i < trianglesList.Count; i++)
            {
                var triangle = trianglesList[i];
                mTriangulos[i] = triangle;

                AddVerticeInTriangulosDelNodo(triangle.getVerticeA, triangle);
                AddVerticeInTriangulosDelNodo(triangle.getVerticeB, triangle);
                AddVerticeInTriangulosDelNodo(triangle.getVerticeC, triangle);


                Parallel.ForEach(hojas, (hoja) =>
                {
                    if (hoja.TrianguloCentral == null)
                    {
                        if (triangle.isDentro(hoja.Centro))
                        {
                            hoja.TrianguloCentral = triangle;
                            hojasEncontradas.Add(hoja);
                        }
                    }
                });

            }


            for (int i = 0; i < mTriangulos.Count(); i++)
            {
                var triangulo = mTriangulos[i];
                if (triangulo == null) continue;
                var listaA = mTriangulosDelNodo[triangulo.getVerticeA.index];
                var listaB = mTriangulosDelNodo[triangulo.getVerticeB.index];
                var listaC = mTriangulosDelNodo[triangulo.getVerticeC.index];


                var adyacentes = new List<int>();
                var adyacenteAB = GetTrianguloAdyacente(listaA, listaB, triangulo);
                var adyacenteAC = GetTrianguloAdyacente(listaA, listaC, triangulo);
                var adyacenteBC = GetTrianguloAdyacente(listaB, listaC, triangulo);
                if (adyacenteAB >= 0) adyacentes.Add(adyacenteAB);
                if (adyacenteAC >= 0) adyacentes.Add(adyacenteAC);
                if (adyacenteBC >= 0) adyacentes.Add(adyacenteBC);

                mAdyacenciasTriangulos[triangulo.getIndex] = adyacentes;
            }




        }

        public Triangulacion(InfoTriangulacionNew info)
        {
            mName = info.Name;

            mTriangulos = new Triangulo[info.getInfoTriangulos.Count];
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

                var miTriangulo = new Triangulo(verticeA, verticeB, verticeC, infoTriangulo.getIndex);


                mTriangulos[miTriangulo.getIndex] = miTriangulo;
                var adyacentes = new List<int>();
                if (infoTriangulo.getAd1 != -1) adyacentes.Add(infoTriangulo.getAd1);
                if (infoTriangulo.getAd2 != -1) adyacentes.Add(infoTriangulo.getAd2);
                if (infoTriangulo.getAd3 != -1) adyacentes.Add(infoTriangulo.getAd3);
                mAdyacenciasTriangulos[miTriangulo.getIndex] = adyacentes;

                AddVerticeInTriangulosDelNodo(verticeA, miTriangulo);
                AddVerticeInTriangulosDelNodo(verticeB, miTriangulo);
                AddVerticeInTriangulosDelNodo(verticeC, miTriangulo);

                if (mNodos[verticeA.index] == null)
                {
                    mNodos[verticeA.index] = verticeA;
                }
                if (mNodos[verticeB.index] == null)
                {
                    mNodos[verticeB.index] = verticeB;
                }
                if (mNodos[verticeC.index] == null)
                {
                    mNodos[verticeC.index] = verticeC;
                }

            }
            mRegiones = new Region(info, this);

            TriangulationResult = new TriangulationResult(mNodos.ToList(), mTriangulos.ToList());
        }




        #endregion

        #region "Metodos privados"

        private void TriangularPuntos(Terreno iTerreno)
        {
            var pointCloud = new PointCloud();
            foreach (var punto in iTerreno.getPuntosTerreno)
            {
                pointCloud.AddPoint(new Punto3d(punto.coordenadaX, punto.coordenadaY,
                    punto.coordenadaZ));
            }

            TriangulationManager triangulation = new TriangulationManager(pointCloud);
            TriangulationResult = triangulation.Triangulate();


        }

        private bool eliminarTriangulosLong(double longMax)
        {
            double mediaDistancias = 0;
            List<Triangulo> miTriangulosEliminar = new List<Triangulo>();
            foreach (Triangulo miTriangulo in mTriangulos)
            {
                if (miTriangulo == null) continue;
                double distAB = miTriangulo.getVerticeA.distancia2d(miTriangulo.getVerticeB);
                double distAC = miTriangulo.getVerticeA.distancia2d(miTriangulo.getVerticeC);
                double distBC = miTriangulo.getVerticeB.distancia2d(miTriangulo.getVerticeC);
                if ((distAB > longMax) || (distAC > longMax) || (distBC > longMax))
                {
                    miTriangulosEliminar.Add(miTriangulo);
                }
                mediaDistancias = mediaDistancias + (distAB + distAC + distBC)/3;

            }
            mediaDistancias = (mediaDistancias/mTriangulos.Count())*2;
            if (mediaDistancias < longMax)
            {
                foreach (Triangulo miTriangulo in miTriangulosEliminar)
                {
                    var misAdyacentes = mAdyacenciasTriangulos[miTriangulo.getIndex];
                    if (misAdyacentes != null)
                    {

                        var miTriangulosEliminarAd = new List<int>();
                        foreach (int miTriangulosAd in misAdyacentes)
                        {
                            miTriangulosEliminarAd.Add(miTriangulosAd);
                        }
                        foreach (int miTriangulosAd in miTriangulosEliminarAd)
                        {
                            var miAdyacentesA = mAdyacenciasTriangulos[miTriangulosAd];
                            if (miAdyacentesA != null)
                            {
                                miAdyacentesA.Remove(miTriangulo.getIndex);
                            }
                        }
                        mAdyacenciasTriangulos[miTriangulo.getIndex] = null;
                    }
                }

                foreach (Triangulo miTriangulo in miTriangulosEliminar)
                {
                    if (miTriangulo != null)
                    {
                        removeTriangulo(miTriangulo);
                    }
                }
                List<int> verticesEliminar = new List<int>();
                foreach (Punto3d miPunto in mNodos)
                {
                    if (miPunto == null) continue;
                    var triangulosVertice = mTriangulosDelNodo[miPunto.index];
                    if (triangulosVertice == null) continue;
                    if (triangulosVertice.Count == 0)
                    {
                        verticesEliminar.Add(miPunto.index);

                    }
                }
                foreach (int miPunto in verticesEliminar)
                {
                    mNodos[miPunto] = null;
                    mTriangulosDelNodo[miPunto] = null;
                }

                return true;

            }
            else
            {
                return false;
            }
        }

        private void removeTriangulo(Triangulo iTrianguloA)
        {
            if (iTrianguloA != null)
            {
                mTriangulos[iTrianguloA.getIndex] = null;

                int[] miNodos = new int[3];
                miNodos[0] = iTrianguloA.getVerticeA.index;
                miNodos[1] = iTrianguloA.getVerticeB.index;
                miNodos[2] = iTrianguloA.getVerticeC.index;
                if (miNodos != null)
                {
                    foreach (int miNodo in miNodos)
                    {
                        var miTriangulos = mTriangulosDelNodo[miNodo];
                        miTriangulos.Remove(iTrianguloA.getIndex);
                    }
                }
            }

        }


        private int GetTrianguloAdyacente(List<int> listaA, List<int> listaB, Triangulo triangulo)
        {
            foreach (var trianguloA in listaA)
            {
                if (trianguloA == triangulo.getIndex) continue;
                if (listaB.Contains(trianguloA)) return trianguloA;
            }
            return -1;
        }

        private void AddVerticeInTriangulosDelNodo(Punto3d v1, Triangulo triangulo)
        {
            if (mTriangulosDelNodo[v1.index] != null)
            {
                var misTriangulos = mTriangulosDelNodo[v1.index];
                misTriangulos.Add(triangulo.getIndex);
            }
            else
            {
                var misTriangulos = new List<int>();
                misTriangulos.Add(triangulo.getIndex);
                mTriangulosDelNodo[v1.index] = misTriangulos;
            }
        }


        #endregion

        #region "Métodos públicos"

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
            while (miAntTriangulo ==null || !miAntTriangulo.Equals(miNextTriangulo))
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
            Triangulo miTrianguloC = getTrianguloReg(miPunto);
            if (miTrianguloC != null)
            {
                miCota = miTrianguloC.getCota(iX, iY);
            }
            return miCota;

        }

        public double? getCotaTriang(double iX, double iY, Triangulo iTriangulo)
        {
            double? miCota = null;
            Punto3d miPunto = new Punto3d(iX, iY, 0);

            var miTrianguloC = getTrianguloTriang(iTriangulo, miPunto);
            if (miTrianguloC == null)
            {
                miTrianguloC = getTrianguloReg(miPunto);
            }
            if (miTrianguloC != null)
            {
                miCota = miTrianguloC.getCota(iX, iY);
            }
            return miCota;

        }

        public Triangulo getTrianguloTriang(Triangulo iTriangulo, Punto3d iPunto)
        {
            var misTriangulosComprobados = new List<int>();
            Triangulo miResultado = null;
            bool encontrado = false;
            Triangulo miTrianguloCentro = iTriangulo;
            if (mTriangulos[miTrianguloCentro.getIndex] != null)
            {
                if (miTrianguloCentro.isDentro(iPunto))
                {
                    miResultado = miTrianguloCentro;
                    encontrado = true;
                }
                else
                {
                    var misAdyacentes = mAdyacenciasTriangulos[miTrianguloCentro.getIndex];
                    var misAdyacentesAux = new List<int>();
                    int repeticiones = 0;
                    misTriangulosComprobados.Add(miTrianguloCentro.getIndex);

                    foreach (var miTrianguloAdIndex in misAdyacentes)
                    {
                        var miTrianguloAd = mTriangulos[miTrianguloAdIndex];
                        if (miTrianguloAd == null) continue;
                        if (miTrianguloAd.isDentro(iPunto))
                        {
                            miResultado = miTrianguloAd;
                            return miResultado;
                        }
                        misTriangulosComprobados.Add(miTrianguloAdIndex);
                        var misAdyacentes2 = mAdyacenciasTriangulos[miTrianguloAd.getIndex];
                        foreach (var miTrianguloAdIndex2 in misAdyacentes2)
                        {
                            if (!misAdyacentesAux.Contains(miTrianguloAdIndex2) && !misTriangulosComprobados.Contains(miTrianguloAdIndex2)
                                 && !misAdyacentes.Contains(miTrianguloAdIndex2))
                                misAdyacentesAux.Add(miTrianguloAdIndex2);
                        }

                    }

                    while ((!encontrado) && (misAdyacentesAux.Count != 0) && (repeticiones < 50))
                    {
                        repeticiones++;
                        var misAdyacentesAux2 = new List<int>();
                        foreach (var miTrianguloAdIndex in misAdyacentesAux)
                        {
                            Triangulo miTrianguloAd = mTriangulos[miTrianguloAdIndex];
                            misTriangulosComprobados.Add(miTrianguloAdIndex);
                            if (miTrianguloAd.isDentro(iPunto))
                            {
                                miResultado = miTrianguloAd;
                                return miResultado;
                            }
                            var misAdyacentes2 = mAdyacenciasTriangulos[miTrianguloAd.getIndex];
                            foreach (int miTrianguloAdIndex2 in misAdyacentes2)
                            {
                                if ((!misAdyacentesAux2.Contains(miTrianguloAdIndex2)) &&
                                    (!misTriangulosComprobados.Contains(miTrianguloAdIndex2)) && !misAdyacentesAux.Contains(miTrianguloAdIndex2))
                                    misAdyacentesAux2.Add(miTrianguloAdIndex2);
                            }
                        }
                        misAdyacentesAux = misAdyacentesAux2;
                    }

                }
                if (!encontrado)
                {
                    miResultado = getTriangulo(iPunto);
                }
            }
            else
            {
                miResultado = null;
            }
            if (miResultado != null && !miResultado.isDentro(iPunto)) miResultado = null;
            return miResultado;
        }

        public Triangulo getTrianguloReg(Punto3d iPunto)
        {
            Triangulo miTrianguloCentro = mRegiones.getRegion(iPunto);
            return getTrianguloTriang(miTrianguloCentro, iPunto);
        }


        public Triangulo getTriangulo(double iX, double iY)
        {
            try
            {
                Punto3d miPunto = new Punto3d(iX, iY, 0);
                Triangulo miTrianguloC = null;

                miTrianguloC = getTrianguloReg(miPunto);
                return miTrianguloC;
            }
            catch (Exception e)
            {
                return null;
            }
            

        }

        public Triangulo getTriangulo(double iX, double iY, Triangulo iTriangulo)
        {
            Punto3d miPunto = new Punto3d(iX, iY, 0);
            Triangulo miTrianguloC = null;

            miTrianguloC = getTrianguloTriang(iTriangulo, miPunto);
            return miTrianguloC;
        }


        public Triangulo getTriangulo(Punto3d iPunto)
        {
            Triangulo miTrianguloDentro = null;

            foreach (Triangulo miTriangulo in mTriangulos)
            {
                if (miTriangulo == null) continue;
                if (miTriangulo.isDentro(iPunto))
                {
                    return miTriangulo;
                }
            }
            return miTrianguloDentro;
        }
        
        public Triangulo getTrianguloMasCercano(Punto3d iPunto)
        {
            double miDist = double.MaxValue;
            Triangulo miTrianguloMasCercano = null;

            foreach (Triangulo miTriangulo in mTriangulos)
            {
                if (miTriangulo == null) continue;
                Punto3d miVerticeA = miTriangulo.getCentro;
                double miDistToPoint = miVerticeA.distancia2d(iPunto);

                if (miDistToPoint < miDist)
                {
                    miTrianguloMasCercano = miTriangulo;
                }
            }
            return miTrianguloMasCercano;
        }

        public double getPendiente(Triangulo iTriangulo)
        {
            return iTriangulo.getPendienteMaxima;
        }

        public double getPendiente(double iX, double iY)
        {
            Punto3d miPunto = new Punto3d(iX, iY, 0);
            Triangulo miTrianguloC = getTrianguloReg(miPunto);
            return miTrianguloC.getPendienteMaxima;
        }

        public List<Triangulo> getTriangulos
        {
            get
            {

                List<Triangulo> miLstTriangulos = new List<Triangulo>();
                foreach (Triangulo miTriangulo in mTriangulos)
                {
                    miLstTriangulos.Add(miTriangulo);
                }
                return miLstTriangulos;
            }
        }

        public List<Punto3d> getNodos
        {
            get
            {

                List<Punto3d> miLstNodos = new List<Punto3d>();
                foreach (Punto3d miNodo in mNodos)
                {
                    miLstNodos.Add(miNodo);
                }
                return miLstNodos;
            }
        }

        public Triangulo[] getTriangulosArray
        {
            get { return mTriangulos; }
        }

        public Region getRegiones
        {
            get { return mRegiones; }
        }

        public List<int>[] getAdyacentes
        {
            get { return mAdyacenciasTriangulos; }
        }

        public MemoryStream guardarTriangulacion()
        {
            InfoTriangulacionNew miInfo = new InfoTriangulacionNew(this);
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            serializer.Serialize(stream, miInfo);
            return stream;

        }

        public List<Hashtable> getTriangulosMaxPendiente()
        {
            return mTriangulosPendienteMayor;
        }

        public List<Triangulo> getPuntosZonasMaxPendiente(double iPendienteMax)
        {
            List<Triangulo> misTriangulos = clasificaTriangulos(iPendienteMax);
            return misTriangulos;
        }

        public List<double[]> getCotas(List<double[]> iListaPuntos)
        {
            List<double[]> miLista3d = new List<double[]>();
            foreach (double[] miPunto in iListaPuntos)
            {
                //[ANGELES] todo, mirar si tiene valor
                double z = getCota(miPunto[0], miPunto[1]).Value;
                double[] miPunto3d = new double[3];
                miPunto3d[0] = miPunto[0];
                miPunto3d[1] = miPunto[1];
                miPunto3d[2] = z;
                miLista3d.Add(miPunto3d);
            }
            return miLista3d;
        }

        public List<Triangulo> clasificaTriangulos(double iPendienteMax)
        {
            List<Triangulo> misTriangulos = new List<Triangulo>();

            foreach (Triangulo miTrian in mTriangulos)
            {
                if (miTrian == null) continue;
                if (miTrian.getPendienteMaxima > iPendienteMax)
                {
                    misTriangulos.Add(miTrian);
                }
            }

            return misTriangulos;
        }

        public bool eliminarTriangulosLista(List<Punto3d> puntos)
        {
            List<Triangulo> miTriangulosEliminar = new List<Triangulo>();
            foreach (var punto in puntos)
            {
                var t = getTrianguloReg(punto);
                if (t == null) t = getTriangulo(punto);
                miTriangulosEliminar.Add(t);
            }

            foreach (Triangulo miTriangulo in miTriangulosEliminar)
            {
                if (miTriangulo == null) continue;
                var misAdyacentes = mAdyacenciasTriangulos[miTriangulo.getIndex];
                if (misAdyacentes != null)
                {

                    var miTriangulosEliminarAd = new List<int>();
                    foreach (var miTriangulosAd in misAdyacentes)
                    {
                        miTriangulosEliminarAd.Add(miTriangulosAd);
                    }
                    foreach (var miTriangulosAd in miTriangulosEliminarAd)
                    {
                        var miAdyacentesA = mAdyacenciasTriangulos[miTriangulosAd];
                        if (miAdyacentesA != null)
                        {
                            miAdyacentesA.Remove(miTriangulo.getIndex);
                        }
                    }
                    mAdyacenciasTriangulos[miTriangulo.getIndex] = null;
                }
            }

            foreach (Triangulo miTriangulo in miTriangulosEliminar)
            {
                if (miTriangulo != null)
                {
                    removeTriangulo(miTriangulo);
                }
            }
            List<int> verticesEliminar = new List<int>();
            foreach (Punto3d miPunto in mNodos)
            {
                if (miPunto == null) continue;
                var triangulosVertice = mTriangulosDelNodo[miPunto.index];
                if (triangulosVertice.Count == 0)
                {
                    verticesEliminar.Add(miPunto.index);

                }
            }
            foreach (int miPunto in verticesEliminar)
            {
                mNodos[miPunto] = null;
                mTriangulosDelNodo[miPunto] = null;
            }


            RecalculateHojas(mRegiones.mHojas);


            return true;
        }

        public List<Punto3d> getPolEnvolvente()
        {
            return calculaEnvolvente();
        }

        public String Name
        {
            get { return mName; }
        }

        #endregion

    }
}
