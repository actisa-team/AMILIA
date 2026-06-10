using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using tadLayShare.puntos;

public class KDTree2D
{
    private class Node
    {
        public Punto3d Point;
        public Node Left;
        public Node Right;
        public bool SplitByX;
    }

    private Node root;

    // Bounding box de la nube
    public double MinX { get; private set; }
    public double MaxX { get; private set; }
    public double MinY { get; private set; }
    public double MaxY { get; private set; }
    private int totalPoints;    // total de puntos a insertar
    private int processedPoints; // contador de puntos procesados
    public event Action<double> OnBuildProgress;
    public KDTree2D(List<Punto3d> points)
    {
        if (points == null || points.Count == 0)
            throw new ArgumentException("La lista de puntos está vacía.");

        MinX = points.Min(p => p.coordenadaX);
        MaxX = points.Max(p => p.coordenadaX);
        MinY = points.Min(p => p.coordenadaY);
        MaxY = points.Max(p => p.coordenadaY);

        //ed = edi;
        totalPoints = points.Count;
        processedPoints = 0;
        root = Build(points, depth: 0);
    }

    private Node Build(List<Punto3d> points, int depth)
    {
        if (points.Count == 0) return null;

        bool splitByX = depth % 2 == 0;
        if (splitByX)
            points.Sort((a, b) => a.coordenadaX.CompareTo(b.coordenadaX));
        else
            points.Sort((a, b) => a.coordenadaY.CompareTo(b.coordenadaY));

        int median = points.Count / 2;
        return new Node
        {
            Point = points[median],
            SplitByX = splitByX,
            Left = Build(points.GetRange(0, median), depth + 1),
            Right = Build(points.GetRange(median + 1, points.Count - median - 1), depth + 1)
        };
    }

    /*private Node Build(List<Punto3d> points, int depth)
    {
        if (points.Count == 0) return null;

        bool splitByX = depth % 2 == 0;
        if (splitByX)
            points.Sort((a, b) => a.coordenadaX.CompareTo(b.coordenadaX));
        else
            points.Sort((a, b) => a.coordenadaY.CompareTo(b.coordenadaY));

        int median = points.Count / 2;
        var currentNode = new Node
        {
            Point = points[median],
            SplitByX = splitByX
        };

        processedPoints++;
        // Notificar progreso cada cierto porcentaje
        if (processedPoints % 50000 == 0 || processedPoints == totalPoints)
        {
            double porcentaje = (double)processedPoints / totalPoints * 100.0;
           // ed.WriteMessage($"\rConstruyendo KD-Tree... {Math.Round(porcentaje, 2)}%");
        }

        currentNode.Left = Build(points.GetRange(0, median), depth + 1);
        currentNode.Right = Build(points.GetRange(median + 1, points.Count - median - 1), depth + 1);

        return currentNode;
    }*/
    public bool IsInside(double x, double y)
    {
        return true;
        return x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
    }

    public List<Punto3d> GetNearestNeighbors(double x, double y, int k)
    {
        var best = new List<Tuple<double, Punto3d>>();
        Search(root, x, y, k, best);
        return best.OrderBy(t => t.Item1).Select(t => t.Item2).ToList();
    }

    private void Search(Node node, double x, double y, int k, List<Tuple<double, Punto3d>> best)
    {
        if (node == null) return;

        double dist = Distance(node.Point, x, y);
        best.Add(Tuple.Create(dist, node.Point));

        if (best.Count > k)
            best.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        if (best.Count > k)
            best.RemoveAt(best.Count - 1);

        double diff = node.SplitByX ? x - node.Point.coordenadaX : y - node.Point.coordenadaY;
        Node near = diff < 0 ? node.Left : node.Right;
        Node far = diff < 0 ? node.Right : node.Left;

        Search(near, x, y, k, best);

        if (best.Count < k || Math.Abs(diff) < best.Max(b => b.Item1))
            Search(far, x, y, k, best);
    }

    private static double Distance(Punto3d p, double x, double y)
    {
        double dx = p.coordenadaX - x;
        double dy = p.coordenadaY - y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    public List<Punto3d> GetClosestOnePerQuadrant(double x, double y, int initialK = 32, int maxK = 4096)
    {
        // Búsqueda directa: un recorrido del árbol por cuadrante, O(log n) cada uno.
        // signX/signY definen el cuadrante: (+,+)=NE  (-,+)=NO  (+,-)=SE  (-,-)=SO
        var result = new List<Punto3d>();

        Punto3d ne = NearestInQuadrant(root, x, y, 1, 1, double.PositiveInfinity);   // NE: dx>=0, dy>=0
        Punto3d nw = NearestInQuadrant(root, x, y, -1, 1, double.PositiveInfinity);  // NO: dx<0,  dy>=0
        Punto3d se = NearestInQuadrant(root, x, y, 1, -1, double.PositiveInfinity);  // SE: dx>=0, dy<0
        Punto3d sw = NearestInQuadrant(root, x, y, -1, -1, double.PositiveInfinity); // SO: dx<0,  dy<0

        if (ne != null) result.Add(ne);
        if (nw != null) result.Add(nw);
        if (se != null) result.Add(se);
        if (sw != null) result.Add(sw);

        return result;
    }

    /// <summary>
    /// Busca el punto más cercano a (qx,qy) que esté en el cuadrante definido por signX/signY.
    /// signX= 1 → dx>=0,  signX=-1 → dx<0   (análogamente signY para dy).
    /// Recorre el KD-Tree podando ramas que no pueden contener puntos del cuadrante
    /// ni mejorar la distancia actual. Complejidad esperada: O(log n).
    /// </summary>
    private Punto3d NearestInQuadrant(Node node, double qx, double qy, int signX, int signY, double bestDist)
    {
        if (node == null) return null;

        Punto3d bestPoint = null;

        // Comprobar si este punto pertenece al cuadrante solicitado
        double dx = node.Point.coordenadaX - qx;
        double dy = node.Point.coordenadaY - qy;
        bool inQuadrant = (signX > 0 ? dx >= 0 : dx < 0) && (signY > 0 ? dy >= 0 : dy < 0);

        if (inQuadrant)
        {
            double d = Math.Sqrt(dx * dx + dy * dy);
            if (d < bestDist)
            {
                bestDist = d;
                bestPoint = node.Point;
            }
        }

        // Determinar qué rama explorar primero
        double diff = node.SplitByX ? qx - node.Point.coordenadaX : qy - node.Point.coordenadaY;
        Node near = diff < 0 ? node.Left : node.Right;
        Node far  = diff < 0 ? node.Right : node.Left;

        // Buscar en la rama cercana
        Punto3d candidate = NearestInQuadrant(near, qx, qy, signX, signY, bestDist);
        if (candidate != null)
        {
            double cd = Distance(candidate, qx, qy);
            if (cd < bestDist)
            {
                bestDist = cd;
                bestPoint = candidate;
            }
        }

        // Podar: ¿la rama lejana puede contener un punto mejor?
        // 1) ¿El hiperplano de splitting está más cerca que bestDist?
        if (Math.Abs(diff) < bestDist)
        {
            // 2) ¿La rama lejana puede contener puntos en el cuadrante deseado?
            //    - Si splitByX y signX>0 (queremos dx>=0, es decir coordX >= qx):
            //      la rama Right contiene puntos con coordX >= split, la Left con coordX < split.
            //      Solo tiene sentido explorar si la rama puede tener coordX >= qx.
            bool farCanHaveQuadrant = true;
            if (node.SplitByX)
            {
                // far es la rama al lado opuesto de diff
                if (signX > 0 && far == node.Left && node.Point.coordenadaX < qx)
                    farCanHaveQuadrant = false; // Left tiene X < split < qx → nunca dx>=0
                if (signX < 0 && far == node.Right && node.Point.coordenadaX >= qx)
                    farCanHaveQuadrant = false; // Right tiene X >= split >= qx → nunca dx<0
            }
            else
            {
                if (signY > 0 && far == node.Left && node.Point.coordenadaY < qy)
                    farCanHaveQuadrant = false;
                if (signY < 0 && far == node.Right && node.Point.coordenadaY >= qy)
                    farCanHaveQuadrant = false;
            }

            if (farCanHaveQuadrant)
            {
                candidate = NearestInQuadrant(far, qx, qy, signX, signY, bestDist);
                if (candidate != null)
                {
                    double cd = Distance(candidate, qx, qy);
                    if (cd < bestDist)
                    {
                        bestDist = cd;
                        bestPoint = candidate;
                    }
                }
            }
        }

        return bestPoint;
    }
    public List<Point3d> GetBoundaryIntersections(Point3d p1, Point3d p2)
    {
        var intersections = new List<Point3d>();

        double dx = p2.X - p1.X;
        double dy = p2.Y - p1.Y;

        // Si los dos puntos son iguales, no hay línea
        if (Math.Abs(dx) < 1e-12 && Math.Abs(dy) < 1e-12)
            return intersections;

        // Candidatos de intersección
        var candidates = new List<Point3d>();

        // Intersección con x = MinX
        if (Math.Abs(dx) > 1e-12)
        {
            double yAtMinX = p1.Y + (MinX - p1.X) * dy / dx;
            candidates.Add(new Point3d(MinX, yAtMinX, 0));
        }

        // Intersección con x = MaxX
        if (Math.Abs(dx) > 1e-12)
        {
            double yAtMaxX = p1.Y + (MaxX - p1.X) * dy / dx;
            candidates.Add(new Point3d(MaxX, yAtMaxX, 0));
        }

        // Intersección con y = MinY
        if (Math.Abs(dy) > 1e-12)
        {
            double xAtMinY = p1.X + (MinY - p1.Y) * dx / dy;
            candidates.Add(new Point3d(xAtMinY, MinY, 0));
        }

        // Intersección con y = MaxY
        if (Math.Abs(dy) > 1e-12)
        {
            double xAtMaxY = p1.X + (MaxY - p1.Y) * dx / dy;
            candidates.Add(new Point3d(xAtMaxY, MaxY, 0));
        }

        // Filtrar los que realmente están dentro del rectángulo
        foreach (var c in candidates)
        {
            if (c.X >= MinX - 1e-9 && c.X <= MaxX + 1e-9 &&
                c.Y >= MinY - 1e-9 && c.Y <= MaxY + 1e-9)
            {
                intersections.Add(c);
            }
        }

        // Eliminar posibles duplicados (por redondeo)
        var unique = new List<Point3d>();
        foreach (var p in intersections)
        {
            bool exists = false;
            foreach (var q in unique)
            {
                if (Math.Abs(p.X - q.X) < 1e-6 &&
                    Math.Abs(p.Y - q.Y) < 1e-6)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
                unique.Add(p);
        }

        return unique;
    }
}

public static class Interpolator
{
    /// <summary>
    /// Estima Z en (x,y) usando interpolación IDW. 
    /// Devuelve double.NaN si el punto está fuera del área o demasiado lejos.
    /// </summary>
    public static double EstimateZ(KDTree2D tree, double x, double y, int k = 8, double power = 2.0, double maxDistance = 100)
    {
        if (!tree.IsInside(x, y))
            return double.NaN;

        var neighbors = tree.GetNearestNeighbors(x, y, k);

        // Aumentar k si no tenemos vecinos en todas las direcciones (hasta cierto límite)
        int attempts = 0;
        /*while (!HasNeighborsInAllQuadrants(neighbors, x, y))
        {
            k += 50; // buscar más vecinos
            neighbors = tree.GetNearestNeighbors(x, y, k);
            attempts++;
        }
        // Si después de varios intentos no hay vecinos en todos los cuadrantes, podemos devolver NaN
        if (!HasNeighborsInAllQuadrants(neighbors, x, y))
            return double.NaN;*/

        double minDist = neighbors.Min(n => Math.Sqrt((n.coordenadaX - x) * (n.coordenadaX - x) +
                                                     (n.coordenadaY - y) * (n.coordenadaY - y)));
        if (minDist > maxDistance)
            if (maxDistance < 1000)
            {
                return EstimateZ(tree, x, y, k, 2.0, maxDistance + 100);
            }
            else
            {
                return double.NaN;
            }


        double num = 0.0, den = 0.0;
        const double epsilon = 1e-12;

        foreach (var n in neighbors)
        {
            double dx = n.coordenadaX - x;
            double dy = n.coordenadaY - y;
            double d = Math.Sqrt(dx * dx + dy * dy) + epsilon;
            double w = 1.0 / Math.Pow(d, power);
            num += w * n.coordenadaZ;
            den += w;
        }
        return num / den;
    }
    private static bool HasNeighborsInAllQuadrants(List<Punto3d> neighbors, double x, double y)
    {
        bool hasNE = false, hasNO = false, hasSE = false, hasSO = false;

        foreach (var n in neighbors)
        {
            double dx = n.coordenadaX - x;
            double dy = n.coordenadaY - y;

            if (dx >= 0 && dy >= 0) hasNE = true;
            if (dx < 0 && dy >= 0) hasNO = true;
            if (dx >= 0 && dy < 0) hasSE = true;
            if (dx < 0 && dy < 0) hasSO = true;
        }

        return hasNE && hasNO && hasSE && hasSO;
    }
    public static double EstimateZQuadrants(KDTree2D tree, double x, double y,double power = 2.0, double maxDistance = 1000.0,int initialK = 32, int maxK = 4096)
    {
        if (!tree.IsInside(x, y))
            return double.NaN;

        var qNeighbors = tree.GetClosestOnePerQuadrant(x, y, initialK, maxK);
        if (qNeighbors.Count < 4)
            return double.NaN; // no hay un vecino en cada cuadrante

        // si hay alguno a distancia 0, devolver su Z directamente
        const double eps = 1e-12;
        foreach (var n in qNeighbors)
        {
            double dx = n.coordenadaX - x;
            double dy = n.coordenadaY - y;
            if (Math.Sqrt(dx * dx + dy * dy) < eps)
                return n.coordenadaZ;
        }

        // control de distancia mínima
        double minDist = qNeighbors.Min(n =>
        {
            double dx = n.coordenadaX - x;
            double dy = n.coordenadaY - y;
            return Math.Sqrt(dx * dx + dy * dy);
        });
        if (minDist > maxDistance)
            return double.NaN;

        // IDW con solo 4 vecinos (uno por cuadrante)
        double num = 0.0, den = 0.0;
        foreach (var n in qNeighbors)
        {
            double dx = n.coordenadaX - x;
            double dy = n.coordenadaY - y;
            double d = Math.Sqrt(dx * dx + dy * dy) + eps;
            double w = 1.0 / Math.Pow(d, power);
            num += w * n.coordenadaZ;
            den += w;
        }
        return num / den;
    }
    


}

public static class SlopeCalculator
{
    /// <summary>
    /// Calcula la pendiente en (x,y) ajustando un plano z = a*x + b*y + c.
    /// Devuelve double.NaN si el punto está fuera del área o demasiado lejos.
    /// </summary>
    public static double EstimateSlope(KDTree2D tree, double x, double y, int k = 8, bool returnDegrees = true, double maxDistance = double.MaxValue)
    {
        if (!tree.IsInside(x, y))
            return double.NaN;

        var neighbors = tree.GetNearestNeighbors(x, y, k);
        double minDist = neighbors.Min(r => Math.Sqrt((r.coordenadaX - x) * (r.coordenadaX - x) +
                                                     (r.coordenadaY - y) * (r.coordenadaY - y)));
        if (minDist > maxDistance)
            return double.NaN;

        double sumX = 0, sumY = 0, sumZ = 0;
        double sumXX = 0, sumYY = 0, sumXY = 0;
        double sumXZ = 0, sumYZ = 0;
        int n = neighbors.Count;

        foreach (var p in neighbors)
        {
            sumX += p.coordenadaX;
            sumY += p.coordenadaY;
            sumZ += p.coordenadaZ;
            sumXX += p.coordenadaX * p.coordenadaX;
            sumYY += p.coordenadaY * p.coordenadaY;
            sumXY += p.coordenadaX * p.coordenadaY;
            sumXZ += p.coordenadaX * p.coordenadaZ;
            sumYZ += p.coordenadaY * p.coordenadaZ;
        }

        double[,] A = new double[3, 3];
        double[] B = new double[3];

        A[0, 0] = sumXX; A[0, 1] = sumXY; A[0, 2] = sumX;
        A[1, 0] = sumXY; A[1, 1] = sumYY; A[1, 2] = sumY;
        A[2, 0] = sumX; A[2, 1] = sumY; A[2, 2] = n;

        B[0] = sumXZ;
        B[1] = sumYZ;
        B[2] = sumZ;

        double[] coeffs = Solve3x3(A, B);
        double a = coeffs[0];
        double b = coeffs[1];

        double slope = Math.Sqrt(a * a + b * b);
        if (returnDegrees)
            slope = Math.Atan(slope) * (180.0 / Math.PI);

        return slope;
    }

    private static double[] Solve3x3(double[,] A, double[] B)
    {
        double[,] M = (double[,])A.Clone();
        double[] X = (double[])B.Clone();
        int n = 3;

        for (int i = 0; i < n; i++)
        {
            double pivot = M[i, i];
            for (int j = i; j < n; j++) M[i, j] /= pivot;
            X[i] /= pivot;

            for (int k = 0; k < n; k++)
            {
                if (k == i) continue;
                double factor = M[k, i];
                for (int j = i; j < n; j++)
                    M[k, j] -= factor * M[i, j];
                X[k] -= factor * X[i];
            }
        }
        return X;
    }
    
}
