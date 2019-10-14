using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace engCadNet
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;


    using tadLayShare;



    public class oPolygon
    {          
        public static  bool isPointInPolygon (Polyline iLw,double X, double Y)
        {

            Point2dCollection myLstPto2d = new Point2dCollection();

            for (int i = 0; i < iLw.NumberOfVertices; i++)
            {

                Point2d myPto2d = iLw.GetPoint2dAt(i);

                myLstPto2d.Add(myPto2d);
            }
          
            int max_point = myLstPto2d.Count - 1;

           
            double total_angle = GetAngle(myLstPto2d[max_point].X, myLstPto2d[max_point].Y, X, Y, myLstPto2d[0].X, myLstPto2d[0].Y);
                
            // Add the angles from the point
            // to each other pair of vertices.
            for (int i = 0; i < max_point; i++)
            {
                total_angle += GetAngle(myLstPto2d[i].X, myLstPto2d[i].Y,X,Y, myLstPto2d[i + 1].X, myLstPto2d[i + 1].Y);   
            }

            // The total angle should be 2 * PI or -2 * PI if
            // the point is in the polygon and close to zero
            // if the point is outside the polygon.
            return (Math.Abs(total_angle) > 0.000001);
        }

        // Return True if the polygon is convex.
        public static bool PolygonIsConvex(Polyline iLw)
        {

            Point2dCollection myLstPto2d = new Point2dCollection();

            for (int i = 0; i < iLw.NumberOfVertices; i++)
            {
                Point2d myPto2d = iLw.GetPoint2dAt(i);
                myLstPto2d.Add(myPto2d);
            }

            bool got_negative = false;
            bool got_positive = false;
            int nuPoints =myLstPto2d.Count-1;
            int B, C;


            for (int A = 0; A < nuPoints; A++)
            {
                B = (A + 1) % nuPoints;
                C = (B + 1) % nuPoints;

                double cross_product = CrossProductLength(myLstPto2d[A].X, myLstPto2d[A].Y, myLstPto2d[B].X, myLstPto2d[B].Y, myLstPto2d[C].X, myLstPto2d[C].Y);
                                                             
                if (cross_product < 0)
                {
                    got_negative = true;
                }
                else if (cross_product > 0)
                {
                    got_positive = true;
                }
                if (got_negative && got_positive) return false;
            }

            // If we got this far, the polygon is convex.
            return true;
        }


        #region "Cross and Dot Products"
        // Return the cross product AB x BC.
        // The cross product is a vector perpendicular to AB
        // and BC having length |AB| * |BC| * Sin(theta) and
        // with direction given by the right-hand rule.
        // For two vectors in the X-Y plane, the result is a
        // vector with X and Y components 0 so the Z component
        // gives the vector's length and direction.
        private static double CrossProductLength(double Ax, double Ay,
            double Bx, double By, double Cx, double Cy)
        {
            // Get the vectors' coordinates.
            double BAx = Ax - Bx;
            double BAy = Ay - By;
            double BCx = Cx - Bx;
            double BCy = Cy - By;

            // Calculate the Z coordinate of the cross product.
            return (BAx * BCy - BAy * BCx);
        }

        // Return the dot product AB · BC.
        // Note that AB · BC = |AB| * |BC| * Cos(theta).
        private static double DotProduct(double Ax, double Ay,
            double Bx, double By, double Cx, double Cy)
        {
            // Get the vectors' coordinates.
            double BAx = Ax - Bx;
            double BAy = Ay - By;
            double BCx = Cx - Bx;
            double BCy = Cy - By;

            // Calculate the dot product.
            return (BAx * BCx + BAy * BCy);
        }

        // Return the angle ABC.
        // Return a value between PI and -PI.
        // Note that the value is the opposite of what you might
        // expect because Y coordinates increase downward.
        private static double GetAngle(double Ax, double Ay, double Bx, double By, double Cx, double Cy)
        {
            // Get the dot product.
            double dot_product = DotProduct(Ax, Ay, Bx, By, Cx, Cy);

            // Get the cross product.
            double cross_product = CrossProductLength(Ax, Ay, Bx, By, Cx, Cy);

            // Calculate the angle.
            return (double)Math.Atan2(cross_product, dot_product);
        }

        #endregion // Cross and Dot Products
    }

}
