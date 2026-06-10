using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLogica.datos;
using Newtonsoft.Json;

namespace tadLayLogica
{
    using tadLayLogica.datos.proyecto;

    using engCadNet;
    using engNet.ClassT;


    using tadLayShare;
    using tadLayShare.puntos;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using Terrenos;
    using tadLayLan.Tdm;
    using tadLayLan;
    using System.IO;
    using tadLayLogica.Comandos;
    using tadLayLogica.logica.EjeBasicoNew;
    using System.Globalization;


    public class oSingletonPuntosTerrenoASC : IDisposable
    {
        private static oSingletonPuntosTerrenoASC mInstance = null;
        private List<Punto3d> puntos = null;
        private Triangulacion mTerreno = null;
        private static Polyline mEnvolvente = null;
        private bool todo = false;
        private nubepuntos nube = new nubepuntos { };
        private string path;
        private KDTree2D tree;
        private bool preciso = true;
        #region "Constructor"
        public static oSingletonPuntosTerrenoASC getInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new oSingletonPuntosTerrenoASC();
                }

                return mInstance;
            }
        }
        private oSingletonPuntosTerrenoASC()
        {

        }
        struct nubepuntos
        {
            public List<Punto3d> puntos { get; set; }
            public double miMax { get; set; }
            public double miPendienteMax { get; set; }
            public int ucNodosPorHoja { get; set; }
        }
        public void Set_Preciso(bool p)
        {
            preciso = p;
        }
        public bool Get_Preciso()
        {
            return preciso;
        }
        #endregion
        public Triangulacion Cargar(bool cargar = false)
        {
            //Cargo el Fichero
            //cargar = false;
            string miFile = oTadil.data.Files.getFileAsc();
            path = miFile;
            try
            {
                string jsonContent = File.ReadAllText(miFile);
                try
                {
                    CargarPuntosASC(10, 40);
                    oSingletonTerreno.getInstance.tipo = 3;
                    
                }
                catch (Exception e)
                {

                }
                if (puntos != null)
                {
                    if (cargar)
                    {

                        //Cargar_MDT(3);
                    }
                }
                this.todo = cargar;

                return mTerreno;

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public void Dispose()
        {
            puntos = null;
            mTerreno = null;
            mEnvolvente = null;
            mInstance = null;
        }
        static string[] SplitLine(string line)
        {
            return line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }
        private void CargarPuntosASC(double intervalo, int precision)
        {
            //string path = "C:\\Users\\Juanma\\Desktop\\prueba tadil\\1024-1.asc";

            string[] lines = File.ReadAllLines(path);

            int ncols = int.Parse(SplitLine(lines[0])[1]);
            int nrows = int.Parse(SplitLine(lines[1])[1]);
            double xllcorner = double.Parse(SplitLine(lines[2])[1], CultureInfo.InvariantCulture);
            double yllcorner = double.Parse(SplitLine(lines[3])[1], CultureInfo.InvariantCulture);
            double cellsize = double.Parse(SplitLine(lines[4])[1], CultureInfo.InvariantCulture);
            double nodata = double.Parse(SplitLine(lines[5])[1], CultureInfo.InvariantCulture);

            puntos = new List<Punto3d>();
            //int aumento = (int)intervalo / (int)cellsize;
            int aumento = 1;
            int rowOffset = 6; // Donde empieza la matriz
            for (int row = 0; row < nrows; row += aumento)
            {
                string[] zValues = lines[row + rowOffset].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < ncols; col += aumento)
                {
                    double z = double.Parse(zValues[col], CultureInfo.InvariantCulture);
                    if (z == nodata) continue;

                    double x = xllcorner + col * cellsize;
                    double y = yllcorner + (nrows - 1 - row) * cellsize;

                    puntos.Add(new Punto3d
                    {
                        coordenadaX = x,
                        coordenadaY = y,
                        coordenadaZ = z
                    });
                }
            }
            nube = new nubepuntos
            {
                miMax = 5000,
                miPendienteMax = 200,
                ucNodosPorHoja = 500,
                puntos = puntos
            };

            this.tree = new KDTree2D(puntos);


            int aumento_2 = precision / (int)cellsize;
            double step = 5.0; // intervalo de curvas de nivel (ej: cada 5 metros)

            // Busco el rango de alturas
            double zMin = puntos.Min(p => p.coordenadaZ);
            double zMax = puntos.Max(p => p.coordenadaZ);

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                // Recorro la malla fila x columna
                for (int row = 0; row < nrows / (aumento); row += aumento_2)
                {
                    Point3dCollection list = new Point3dCollection();
                    for (int col = 0; col < ncols / (aumento); col += aumento_2)
                    {
                        int idx = row * (ncols / (aumento)) + col;
                        Punto3d p1 = GetPoint(idx);
                        list.Add(new Point3d(p1.coordenadaX, p1.coordenadaY, p1.coordenadaZ));
                    }
                    oLw.addLw3d(list, false, "0");
                }
                Database db = HostApplicationServices.WorkingDatabase;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                    mEnvolvente = CrearPolilineaEnvolvente();

                    if (mEnvolvente != null)
                    {
                        btr.AppendEntity(mEnvolvente);
                        tr.AddNewlyCreatedDBObject(mEnvolvente, true);
                    }

                    tr.Commit();
                }
            }

        }
        // Devuelve un punto de la grilla
        private Punto3d GetPoint(int row, int col, int ncols)
        {
            return puntos[row * ncols + col];
        }
        // Devuelve un punto de la grilla
        private Punto3d GetPoint(int contador)
        {
            return puntos[contador];
        }
        public void Set_Envolvente(Polyline m)
        {
            mEnvolvente = m;
        }

        // Interpola dónde la curva cruza una celda
        private List<Tuple<Point2d, Point2d>> InterpolarCurva(Punto3d p1, Punto3d p2, Punto3d p3, Punto3d p4, double nivel)
        {
            var lista = new List<Tuple<Point2d, Point2d>>();

            // Pares de lados de la celda
            var lados = new[]
            {
        Tuple.Create(p1, p2),
        Tuple.Create(p2, p3),
        Tuple.Create(p3, p4),
        Tuple.Create(p4, p1)
    };

            List<Point2d> intersecciones = new List<Point2d>();

            foreach (var lado in lados)
            {
                if ((nivel >= lado.Item1.coordenadaZ && nivel <= lado.Item2.coordenadaZ) ||
                    (nivel >= lado.Item2.coordenadaZ && nivel <= lado.Item1.coordenadaZ))
                {
                    // Interpolación lineal
                    double t = (nivel - lado.Item1.coordenadaZ) / (lado.Item2.coordenadaZ - lado.Item1.coordenadaZ);
                    double x = lado.Item1.coordenadaX + t * (lado.Item2.coordenadaX - lado.Item1.coordenadaX);
                    double y = lado.Item1.coordenadaY + t * (lado.Item2.coordenadaY - lado.Item1.coordenadaY);

                    intersecciones.Add(new Point2d(x, y));
                }
            }

            if (intersecciones.Count == 2)
                lista.Add(Tuple.Create(intersecciones[0], intersecciones[1]));

            return lista;
        }
        public double GetZ(double[] punto)
        {
            try
            {
                if (tree != null)
                {
                    double estimado = double.NaN;
                    if (preciso)
                    {
                        estimado = Interpolator.EstimateZQuadrants(tree, punto[0], punto[1]);
                    }
                    else
                    {
                        estimado = Interpolator.EstimateZ(tree, punto[0],punto[1]);
                    }
                    return estimado;
                    //double bbb = SlopeCalculator.EstimateSlope(tree, xllcorner + 151, yllcorner + 261);
                }
                var target = punto;
                if (mTerreno == null)
                {
                    CreaTerreno(target);
                }
                if (mTerreno == null)
                {
                    return -1;
                }
                if (mTerreno.getTriangulo(punto[0], punto[1]) == null)
                {
                    CreaTerreno(target);
                }

                double? z = mTerreno.getCota(punto[0], punto[1]);
                return z ?? -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public double GetZ(double x, double y)
        {
            try
            {
                if (tree!=null)
                {
                    double estimado = double.NaN;
                    if (preciso)
                    {
                        estimado = Interpolator.EstimateZQuadrants(tree, x, y);
                    }
                    else
                    {
                        estimado = Interpolator.EstimateZ(tree, x, y);
                    }
                    return estimado;
                    //double bbb = SlopeCalculator.EstimateSlope(tree, xllcorner + 151, yllcorner + 261);
                }
                var target = new double[2] { x, y };
                if (mTerreno == null)
                {
                    CreaTerreno(target);
                }
                if (mTerreno == null)
                {
                    return -1;
                }
                if (mTerreno.getTriangulo(x, y) == null)
                {
                    CreaTerreno(target);
                }

                double? z = mTerreno.getCota(target[0], target[1]);
                return z ?? -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public void CreaTerreno(double[] target)
        {

            double x_max, x_min, y_max, y_min;

            x_min = target[0];
            x_max = target[0];
            y_min = target[1];
            y_max = target[1];
            double dis_extra = 500;
            x_min -= dis_extra;
            x_max += dis_extra;
            y_min -= dis_extra;
            y_max += dis_extra;

            if (puntos == null)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }
            if (puntos.Count() == 0)
            {
                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                return;
            }

            // Filtrar los puntos dentro del rango
            List<Punto3d> puntos_en_rango = puntos
                .Where(p => p.coordenadaX >= x_min && p.coordenadaX <= x_max && p.coordenadaY >= y_min && p.coordenadaY <= y_max)
                .ToList();
            if (puntos_en_rango.Count()==0)
            {
                this.mTerreno = null;
                return;
            }
            List<Triangulo> _znpTriangulos = new List<Triangulo>();
            this.mTerreno = oComandoTerreno.crearMallaNoUI(puntos_en_rango, "nombre", nube.miMax, nube.miPendienteMax,
                       "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
        }
        public void Cargar_MDT(int tipo = 3)
        {
            if (!todo)
            {
                List<Triangulo> _znpTriangulos = new List<Triangulo>();
                oTadil.data.UserInfo.showInfo("La carga del terreno puede tardar varios minutos");
                mTerreno = oComandoTerreno.crearMallaNoUI(puntos, "nombre", nube.miMax, nube.miPendienteMax,
                           "C:\\Users\\Juanma\\AppData\\Local\\Temp\\TADILtriangulation.dwg", ref _znpTriangulos, nube.ucNodosPorHoja);
                oSingletonTerreno.getInstance.Set_Terreno(mTerreno, tipo);
                oSingletonTerreno.getInstance.Set_envolvente();
            }

            this.todo = true;
        }
        public Triangulo getTrianguloInside(double? iX, double? iY)
        {
            if (iX.HasValue && iY.HasValue)
            {
                var target = new double[2] { iX.Value, iY.Value };
                if (mTerreno == null)
                {
                    CreaTerreno(target);
                }
                if (mTerreno == null)
                {
                    return null;
                }
                if (mTerreno.getTriangulo(iX.Value, iY.Value) == null)
                {
                    CreaTerreno(target);
                }

                try
                {
                    Triangulo miTriangulo = this.mTerreno.getTriangulo(iX.Value, iY.Value);
                    return miTriangulo;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }
        }
        public double? getZFromTriang(double? iX, double? iY, Triangulo iTriangulo)
        {

            if (iX.HasValue && iY.HasValue)
            {
                try
                {
                    var target = new double[2] { iX.Value, iY.Value };
                    if (mTerreno == null)
                    {
                        CreaTerreno(target);
                    }
                    if (mTerreno == null)
                    {
                        return null;
                    }
                    if (mTerreno.getTriangulo(iX.Value, iY.Value) == null)
                    {
                        CreaTerreno(target);
                    }
                    double? myZ = null;

                    myZ = this.mTerreno.getCotaTriang(iX.Value, iY.Value, iTriangulo);

                    if (myZ.HasValue)
                    {
                        return myZ.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }
        public Triangulo getTrianguloInsideFromTriangulo(double? iX, double? iY, Triangulo iTriangulo)
        {
            if (iX.HasValue && iY.HasValue)
            {
                try
                {
                    var target = new double[2] { iX.Value, iY.Value };
                    if (mTerreno == null)
                    {
                        CreaTerreno(target);
                    }
                    if (mTerreno == null)
                    {
                        return null;
                    }
                    if (mTerreno.getTriangulo(iX.Value, iY.Value) == null)
                    {
                        CreaTerreno(target);
                    }
                    Triangulo miTriangulo = this.mTerreno.getTrianguloTriang(iTriangulo, new Punto3d(iX.Value, iY.Value, 0));
                    return miTriangulo;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }
        }
        public double? getSlopeFromTriang(Triangulo iTriangulo,double x=-1,double y=-1)
        {

            if (x != -1 && y != -1)
            {
                return Slope(x, y);
            }
            if (iTriangulo != null)
            {

                try
                {
                    
                    
                    double myPenOut = iTriangulo.getPendienteMaxima;

                    return myPenOut;

                }

                catch (SystemException)
                {
                    return null;
                }

            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }
        public double Slope(double x, double y)
        {
            if (tree != null)
            {
                double estimado = SlopeCalculator.EstimateSlope(tree, x, y);
                return estimado;
              
            }
            return -1;
        }
        public bool Inside(double iX, double iY)
        {
            if (!tree.IsInside(iX, iY))
            {
                return false;
            }
                
            return true;
        }
        public double DistanciaABorde(double x, double y)
        {
            if (puntos == null || puntos.Count == 0)
                throw new InvalidOperationException("No hay puntos cargados para calcular el bounding box.");

            // Calculamos bounding box de los puntos
            /*double xMin = puntos.Min(p => p.coordenadaX);
            double xMax = puntos.Max(p => p.coordenadaX);
            double yMin = puntos.Min(p => p.coordenadaY);
            double yMax = puntos.Max(p => p.coordenadaY);*/
            double xMin = tree.MinX;
            double xMax = tree.MaxX;
            double yMin = tree.MinY;
            double yMax = tree.MaxY;

            double distMin = double.PositiveInfinity;

            // Distancia al borde izquierdo (Xmin)
            double dIzq = Math.Abs(x - xMin);
            if (dIzq < distMin) distMin = dIzq;

            // Distancia al borde derecho (Xmax)
            double dDer = Math.Abs(xMax - x);
            if (dDer < distMin) distMin = dDer;

            // Distancia al borde inferior (Ymin)
            double dInf = Math.Abs(y - yMin);
            if (dInf < distMin) distMin = dInf;

            // Distancia al borde superior (Ymax)
            double dSup = Math.Abs(yMax - y);
            if (dSup < distMin) distMin = dSup;

            return distMin;
        }

        private Polyline CrearPolilineaEnvolvente()
        {
            if (puntos == null || puntos.Count == 0)
                return null;

            // Convertir puntos a lista de (X,Y)
            var lista = puntos.Select(p => new Point2d(p.coordenadaX, p.coordenadaY)).ToList();

            // Calcular convex hull
            var hull = GetConvexHull(lista);

            // Crear polilínea en AutoCAD
            Polyline poly = new Polyline();
            for (int i = 0; i < hull.Count; i++)
            {
                poly.AddVertexAt(i, hull[i], 0, 0, 0);
            }
            poly.Closed = true; // Cierra la polilínea
            return poly;
        }

        private List<Point2d> GetConvexHull(List<Point2d> points)
        {
            if (points.Count <= 1)
                return new List<Point2d>(points);

            points = points.OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            List<Point2d> lower = new List<Point2d>();
            foreach (var p in points)
            {
                while (lower.Count >= 2 && Cross(lower[lower.Count - 2], lower[lower.Count - 1], p) <= 0)
                    lower.RemoveAt(lower.Count - 1);
                lower.Add(p);
            }

            List<Point2d> upper = new List<Point2d>();
            for (int i = points.Count - 1; i >= 0; i--)
            {
                var p = points[i];
                while (upper.Count >= 2 && Cross(upper[upper.Count - 2], upper[upper.Count - 1], p) <= 0)
                    upper.RemoveAt(upper.Count - 1);
                upper.Add(p);
            }

            upper.RemoveAt(upper.Count - 1);
            lower.RemoveAt(lower.Count - 1);
            lower.AddRange(upper);
            return lower;
        }

        private double Cross(Point2d o, Point2d a, Point2d b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

    }
}
