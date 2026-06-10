using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Strtree;
using NetTopologySuite.Triangulate;
using tadLayShare.puntos; // Tu namespace

namespace interfaz
{
    public class TerrainService
    {
        private readonly STRtree<Polygon> _spatialIndex;
        private readonly GeometryFactory _factory;
        
        // Exponemos la triangulación generada para poder dibujarla en AutoCAD
        public IGeometryCollection Triangles { get; private set; }

        public TerrainService(List<Punto3d> terrainPoints, List<Punto3d> boundary = null)
        {
            _factory = new GeometryFactory();
            _spatialIndex = new STRtree<Polygon>();
            BuildTIN(terrainPoints, boundary);
        }

        private void BuildTIN(List<Punto3d> points, List<Punto3d> boundary)
        {
            // 1. Convertir tus Punto3d a Coordenadas de NTS
            var coords = points.Select(p => new Coordinate(p.coordenadaX, p.coordenadaY, p.coordenadaZ)).ToList();

            IPolygon boundaryPolygon = null;
            if (boundary != null && boundary.Count > 2)
            {
                var boundaryCoords = boundary.Select(p => new Coordinate(p.coordenadaX, p.coordenadaY)).ToList();
                // Ensure the boundary is closed
                if (!boundaryCoords.First().Equals2D(boundaryCoords.Last()))
                {
                    boundaryCoords.Add(boundaryCoords.First());
                }
                boundaryPolygon = _factory.CreatePolygon(boundaryCoords.ToArray());
                
                // Add boundary points as sites to ensure triangulation aligns with the boundary
                coords.AddRange(boundaryCoords);
            }

            // 2. Generar Triangulación de Delaunay
            var builder = new DelaunayTriangulationBuilder();
            builder.SetSites(coords.ToArray());
            var allTriangles = builder.GetTriangles(_factory);

            var filteredTriangles = new List<Geometry>();
            
            // 3. Filtrar e indexar espacialmente
            for (int i = 0; i < allTriangles.NumGeometries; i++)
            {
                var triangle = (IPolygon)allTriangles.GetGeometryN(i);
                
                bool keep = true;
                if (boundaryPolygon != null)
                {
                    // Filter: Only keep triangles whose centroid is inside the boundary
                    var centroid = triangle.Centroid;
                    if (!boundaryPolygon.Contains(centroid))
                    {
                        keep = false;
                    }
                }
                
                if (keep)
                {
                    filteredTriangles.Add((Geometry)triangle);
                    _spatialIndex.Insert(triangle.EnvelopeInternal, (Polygon)triangle);
                }
            }
            
            Triangles = _factory.CreateGeometryCollection(filteredTriangles.ToArray());
            _spatialIndex.Build();
        }

        public double GetPreciseZ(double x, double y)
        {
            var queryPoint = _factory.CreatePoint(new Coordinate(x, y));

            // Búsqueda en el índice espacial (Casi instantáneo)
            var candidates = _spatialIndex.Query(queryPoint.EnvelopeInternal);

            foreach (var triangle in candidates)
            {
                if (triangle.Intersects(queryPoint))
                {
                    return InterpolateZOnTriangle(x, y, triangle);
                }
            }

            return double.NaN; // Fuera del límite del terreno
        }

        private double InterpolateZOnTriangle(double x, double y, Polygon triangle)
        {
            // Ecuación del plano: Ax + By + Cz + D = 0
            // Obtenemos los 3 vértices del triángulo
            var p1 = triangle.Coordinates[0];
            var p2 = triangle.Coordinates[1];
            var p3 = triangle.Coordinates[2];

            // Vectores del plano
            double ux = p2.X - p1.X, uy = p2.Y - p1.Y, uz = p2.Z - p1.Z;
            double vx = p3.X - p1.X, vy = p3.Y - p1.Y, vz = p3.Z - p1.Z;

            // Producto vectorial para hallar la normal (A, B, C)
            double nx = uy * vz - uz * vy;
            double ny = uz * vx - ux * vz;
            double nz = ux * vy - uy * vx;

            // Si nz es 0, el triángulo es vertical (error topográfico), evitamos división por cero
            if (Math.Abs(nz) < 0.000001) return p1.Z;

            // Hallar Z despejando la ecuación del plano:
            // nz(z - p1.z) = -(nx(x - p1.x) + ny(y - p1.y))
            double z = p1.Z - (nx * (x - p1.X) + ny * (y - p1.Y)) / nz;

            return z;
        }

        // Método para procesar los 10,000 puntos de forma masiva
        public void ProcessPolyline(List<Punto3d> polyline)
        {
            // Usamos Parallel para aprovechar todos los núcleos del CPU
            Parallel.ForEach(polyline, p =>
            {
                double calculatedZ = GetPreciseZ(p.coordenadaX, p.coordenadaY);
                if (!double.IsNaN(calculatedZ))
                {
                    p.coordenadaZ = calculatedZ;
                }
            });
        }
    }
}