using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using tadLayShare;
using tadLayShare.puntos;
using PointCloud = TriangulationTool.Data.PointCloud;

namespace TriangulationTool
{
    public class TriangulationManager
    {
        private const string EyeshotSerialNumber = "PROWPF-97X1-2T1TY-F9HJV-EP5VT";
        private readonly List<Point3D> _points;

        public TriangulationManager(PointCloud pointCloud)
        {
            _points = EyeshotAdapter.AdaptPoints(pointCloud.Points);
        }


        public TriangulationResult Triangulate()
        {
            try
            {
                var result = UtilityEx.Triangulate(_points);
                var triangulationResult = new TriangulationResult(result);
                return triangulationResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TriangulationResult> TriangulateAsync()
        {
            return await Task.Run(()=>Triangulate());
        }

        public static bool SaveToAutoDesk(List<Punto3d> vertexList, List<Triangulo> facesList , string filename, string layerName)
        {
            List<Point3D> points = EyeshotAdapter.AdaptPoints(vertexList);
            List<IndexTriangle> faces = EyeshotAdapter.AdaptFaces(facesList);
            var mesh = new Mesh(points, faces);
            mesh.Regen(0.1);
            return SaveToAutoDesk(mesh, filename, layerName);
        }

        private static bool SaveToAutoDesk(Mesh mesh, string filename, string layerName)
        {
            int numIntentos = 0;
            bool procesoTerminado = false;
            while (!procesoTerminado && numIntentos < 2)
            {
                try
                {
                    Layer layer0 = new Layer(layerName);
                    var entities = new List<Entity> {mesh};
                    var dicBlock = new Dictionary<string, Block>();
                    var writer = new WriteAutodesk(entities, new List<Layer> {layer0}, dicBlock,
                        new ConcurrentDictionary<string, TextStyle>(), filename, linearUnitsType.Meters);
                    writer.Unlock(EyeshotSerialNumber);
                    numIntentos++;
                    writer.DoWork();
                    procesoTerminado = true;
                }
                catch(NullReferenceException)
                {

                }
            }
            return procesoTerminado;
        }
    }
}
