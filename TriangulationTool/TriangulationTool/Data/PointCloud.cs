using System.Collections.Generic;
using tadLayShare.puntos;

namespace TriangulationTool.Data
{
    public class PointCloud
    {
        public List<Punto3d> Points { get; set; }

        public PointCloud()
        {
            Points = new List<Punto3d>();
        }

        public void AddPoint(Punto3d point)
        {
            Points.Add(point);
        }
    }
}
