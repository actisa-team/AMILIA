using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet.entidades
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;

   
   public static class oView
   {

       public enum ViewDirection {Top, Bottom, Front, Back, Left, Right, SeIso, SwIso, NeIso, NwIso }
    
       /// <summary>
       /// CONFIGURAR LAS VISTAS ORTOGONALES EN ESPACIO MODELO
       /// </summary>
       public static void setView(ViewDirection vDir)
       {
           Document doc = Application.DocumentManager.MdiActiveDocument;
           Database db = doc.Database;
           Editor ed = doc.Editor;

           Vector3d viewDir = new Vector3d();

           switch (vDir)
           {
               case ViewDirection.Top:
                   viewDir = Vector3d.ZAxis;
                   break;
               case ViewDirection.Bottom:
                   viewDir = Vector3d.ZAxis.Negate();
                   break;
               case ViewDirection.Front:
                   viewDir = Vector3d.YAxis.Negate();
                   break;
               case ViewDirection.Back:
                   viewDir = Vector3d.YAxis;
                   break;
               case ViewDirection.Left:
                   viewDir = Vector3d.XAxis.Negate();
                   break;
               case ViewDirection.Right:
                   viewDir = Vector3d.XAxis;
                   break;
               case ViewDirection.SeIso:
                   viewDir = new Vector3d(1.0, -1.0, 1.0);
                   break;
               case ViewDirection.SwIso:
                   viewDir = new Vector3d(-1.0, -1.0, 1.0);
                   break;
               case ViewDirection.NeIso:
                   viewDir = new Vector3d(1.0, 1.0, 1.0);
                   break;
               case ViewDirection.NwIso:
                   viewDir = new Vector3d(-1.0, 1.0, 1.0);
                   break;
           }

           

           Extents3d extents = new Extents3d(db.Extmin, db.Extmax);

           double miFactorEscalaVentana = 1.2;



           using (Transaction tr = db.TransactionManager.StartTransaction())
           {
               using (ViewTableRecord view = ed.GetCurrentView())
               {
                   Matrix3d viewTransform = Matrix3d.PlaneToWorld(viewDir).PreMultiplyBy(Matrix3d.Displacement(view.Target - Point3d.Origin)).PreMultiplyBy(Matrix3d.Rotation(-view.ViewTwist, view.ViewDirection, view.Target)).Inverse();               
                   extents.TransformBy(viewTransform);
                   view.ViewDirection = viewDir;
                   view.Width = (extents.MaxPoint.X - extents.MinPoint.X)* miFactorEscalaVentana; 
                   view.Height = (extents.MaxPoint.Y - extents.MinPoint.Y) *miFactorEscalaVentana;
                   view.CenterPoint = new Point2d((extents.MinPoint.X + extents.MaxPoint.X) / 2.0, (extents.MinPoint.Y + extents.MaxPoint.Y) / 2.0);          
                   ed.SetCurrentView(view);
                   tr.Commit();
               }
           }
       }
   }
}
