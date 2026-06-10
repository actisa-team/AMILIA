using System;
using NUnit.Framework;
using TriangulationTool;
using TriangulationTool.Data;
using Point = tadLayShare.puntos.Punto3d;

namespace TriangulationToolUnitTests
{
    [TestFixture]
    public class TriangulationManagerTests
    {
        [Test]
        public void ShouldTriangulationOfPointCloudReturnCorrectNumberOfVerticesAndTriangles()
        {
            const int expectedNumberOfVertices = 360;
            const int expectedNumberOfTriangles = 358;
            var pointCloud = BuildCircularPointCloud(expectedNumberOfVertices);

            var triangulationManager = new TriangulationManager(pointCloud);

            var result = triangulationManager.Triangulate();

            var vertices = result.GetAllVertices();
            var triangles = result.GetAllTriangles();

            
            Assert.AreEqual(expectedNumberOfVertices, vertices.Count);
            Assert.AreEqual(expectedNumberOfTriangles,triangles.Count);
            
        }

        private static PointCloud BuildCircularPointCloud(int numberOfVertices)
        {
            var pointCloud = new PointCloud();
            var center = new Point();
            const double radius = 1000.0;
            for (double angle = 0; angle < numberOfVertices; angle++)
            {
                var x = center.coordenadaX + (radius * Math.Cos(angle));
                var y = center.coordenadaY + (radius * Math.Sin(angle));
                var point = new Point(x, y, 0);
                pointCloud.AddPoint(point);
            }

            return pointCloud;
        }


    }
}