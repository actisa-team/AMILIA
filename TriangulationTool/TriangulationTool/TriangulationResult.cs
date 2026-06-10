using System.Collections.Generic;
using System.Linq;
using devDept.Eyeshot.Entities;
using Point = tadLayShare.puntos.Punto3d;
using Triangle = tadLayShare.Triangulo;

namespace TriangulationTool
{
    public class TriangulationResult
    {
        private readonly Mesh _mesh;

        public TriangulationResult(List<Point> vertexList, List<Triangle> faces)
        {
            _mesh = new Mesh(EyeshotAdapter.AdaptPoints(vertexList), EyeshotAdapter.AdaptFaces(faces));
            _mesh.Regen(0.1);
        }

        public TriangulationResult(Mesh mesh)
        {
            _mesh = mesh;
            _mesh.Regen(0.1);
        }

        public List<Point> GetAllVertices()
        {
            var vertices = EyeshotAdapter.AdaptPoints(_mesh.Vertices);
            return vertices;
        }

        public List<Triangle> GetAllTriangles()
        {
            var vertices = GetAllVertices();
            return _mesh.Triangles.Select((indexTriangle, index) => new Triangle(new Point(vertices[indexTriangle.V1].coordenadaX, vertices[indexTriangle.V1].coordenadaY, vertices[indexTriangle.V1].coordenadaZ, indexTriangle.V1)
                , new Point(vertices[indexTriangle.V2].coordenadaX, vertices[indexTriangle.V2].coordenadaY, vertices[indexTriangle.V2].coordenadaZ, indexTriangle.V2),
                new Point(vertices[indexTriangle.V3].coordenadaX, vertices[indexTriangle.V3].coordenadaY, vertices[indexTriangle.V3].coordenadaZ, indexTriangle.V3),
                index)).ToList();
        }

    }
}
