using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
namespace Logica
{
    public class Pendiente
    {
        List<Point2d> puntos = new List<Point2d>();
        public double az { get; set; }
        public List<Point2d> Puntos { get => puntos; set => puntos = value; }
        public Pendiente()
        {

        }
        public Pendiente(List<Point2d> l)
        {
            puntos = l;
        }
    }
}
