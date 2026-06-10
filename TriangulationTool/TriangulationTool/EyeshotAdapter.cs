using System.Collections.Generic;
using System.Linq;
using devDept.Geometry;
using tadLayShare;
using tadLayShare.puntos;
using TriangulationTool.Data;

namespace TriangulationTool
{
    internal static class EyeshotAdapter
    {
        public static List<Point3D> AdaptPoints(IEnumerable<Punto3d> points)
        {
            var result = new List<Point3D>();
            foreach (var punto3D in points)
            {
                if (punto3D == null) result.Add(new Point3D(0, 0, 0));
                else
                {
                    result.Add(new Point3D(punto3D.coordenadaX, punto3D.coordenadaY, punto3D.coordenadaZ));
                }
            }
            return result;
        }

        public static List<Punto3d> AdaptPoints(IEnumerable<Point3D> points)
        {
            return points.Select(point => new Punto3d(point.X, point.Y, point.Z)).ToList();
        }


        public static List<IndexTriangle> AdaptFaces(IEnumerable<Triangulo> triangles)
        {
            var result = new List<IndexTriangle>();
            foreach (var triangle in triangles)
            {
                if (triangle == null) continue;
                result.Add(new IndexTriangle(triangle.getVerticeA.index, triangle.getVerticeB.index, triangle.getVerticeC.index));
            }
            return result;
        }

    }
}
