using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{

    using tadLayShare;
    using Autodesk.AutoCAD.Geometry;
    using tadLayShare.puntos;
    
    public class oConvexHullOld
    {


        private static Point2d mP0;
     

        public static List<oP2d> getLstoP2dConvexHull(List<oP2d> iLstPto)
        {

            List<Point2d> myLstPtoCad = new List<Point2d>();

            foreach (oP2d myPto in iLstPto)
            {
                myLstPtoCad.Add(new Point2d(myPto.X, myPto.Y));
            }

            List<Point2d> myLstPtoCadConvexHull = getConvexHull(myLstPtoCad);


            List<oP2d> myLstPtoConvexHull = new List<oP2d>();


            foreach (Point2d myPtoCad in myLstPtoCadConvexHull)
            {
                myLstPtoConvexHull.Add(new oP2d(myPtoCad.X,myPtoCad.Y));
            }

            return myLstPtoConvexHull;
        
        
        }


       

        
        public static List<Point2d> getConvexHull(List<Point2d> pts)
        {
            mP0 = pts.OrderBy(p => p.Y).ThenBy(p => p.X).First();
            pts = pts.OrderByDescending(p => Cosine(p)).ThenBy(p => mP0.GetDistanceTo(p)).ToList();
            for (int i = 1; i < pts.Count - 1; i++)
            {
                while (i > 0 && Clockwise(pts[i - 1], pts[i], pts[i + 1]))
                {
                    pts.RemoveAt(i);
                    i--;
                }
            }
            return pts;
        }



        private static bool Clockwise(Point2d p1, Point2d p2, Point2d p3)
        {
            return ((p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X)) < 1e-9;
        }

        private static double Cosine(Point2d pt)
        {
            double d = mP0.GetDistanceTo(pt);
            return d == 0.0 ? 1.0 : Math.Round((pt.X - mP0.X) / d, 9);
        }




    }


   
}
