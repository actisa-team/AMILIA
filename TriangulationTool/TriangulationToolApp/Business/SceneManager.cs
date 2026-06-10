using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using Terrenos;
using TriangulationTool;

namespace TriangulationToolApp.Business
{
    public class SceneManager
    {
        private Mesh _resultMesh;
        private List<Point3D> _points;
        public bool IsPointCloudLoaded
        {
            get { return _points != null; }
        }

        public bool IsMeshTriangulated
        {
            get { return _resultMesh != null; }
        }

        public int NumberOfPointsInCloud
        {
            get { return _points == null ? 0 : _points.Count; }
        }

        public FastPointCloud LoadPointCloud(string filePath)
        {
            _points = new List<Point3D>();
            try
            {
                var separator = GetCsvSeparator(filePath);
                var coordsText = File.ReadAllLines(filePath);
                var cultureInfo = new CultureInfo("en-EN");
                foreach (var line in coordsText)
                {
                    try
                    {
                        var values = line.Split(separator);
                        var x = double.Parse(values[0], cultureInfo);
                        var y = double.Parse(values[1], cultureInfo);
                        var z = double.Parse(values[2], cultureInfo);

                        var coord = new Point3D(x, y, z);
                        _points.Add(coord);
                    }
                    catch
                    {
                        //Nothing to do
                    }
                }

                var pointCloud = new PointCloud(_points);
                return pointCloud.ConvertToFastPointCloud();
            }
            catch (Exception)
            {
                _points = null;
                return null;
            }
        }

        private static char GetCsvSeparator(string filePath)
        {
            var line = File.ReadLines(filePath).First();
            return line.Contains(";") ? ';' : ',';
        }

        public async Task<FastPointCloud> LoadPointCloudAsync(string filePath)
        {
            return await Task.Run(() => LoadPointCloud(filePath));
        }

        public Mesh TriangulatePointCloud()
        {
            _resultMesh = UtilityEx.Triangulate(_points);
            return _resultMesh;
        }

        public async Task<Mesh> TriangulatePointCloudAsync()
        {
            return await Task.Run(() => TriangulatePointCloud());
        }


        public bool ExportToAutodesk(ViewportLayout viewportLayout, string fileName)
        {
            try
            {
                var writer = new WriteAutodesk(viewportLayout, fileName, linearUnitsType.Meters, true, true);
                writer.DoWork();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public async Task<bool> ExportToAutodeskAsync(ViewportLayout viewportLayout, string fileName)
        {
            return await Task.Run(()=>ExportToAutodesk(viewportLayout, fileName));
        }

        public bool GenerateSearchTree(Mesh mesh, string outputFilePath, int numRegions)
        {
            try
            {
                using (FileStream filestream = new FileStream(outputFilePath, FileMode.OpenOrCreate))
                {
                    var triangulationResult = new TriangulationResult(mesh);
                    var triangulacionTdm = new Triangulacion(triangulationResult, numRegions);
                    var streamMemory = triangulacionTdm.guardarTriangulacion();
                    streamMemory.Position = 0;
                    streamMemory.CopyTo(filestream);
                }


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> GenerateSearchTreeAsync(Mesh mesh, string outputFilePath, int numRegions)
        {
            return await Task.Run(()=>GenerateSearchTree(mesh, outputFilePath, numRegions));
        }
    }
}
