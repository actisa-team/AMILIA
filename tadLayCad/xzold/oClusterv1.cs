using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace tadLayCad
//{


//    using System.Windows.Forms;

//    using Autodesk.AutoCAD.Colors;
//    using Autodesk.AutoCAD.ApplicationServices;
//    using Autodesk.AutoCAD.DatabaseServices;
//    using Autodesk.AutoCAD.EditorInput;
//    using Autodesk.AutoCAD.Geometry;
    
//    public class KMeansWorker
//    {


//        List<Point> points = new List<Point>();

//        public KMeansWorker(int iGrupos, List<Point> iLstPto)
//        {
            
         
//            //Datos
//            int clusters = 2; // iGrupos;


//            points = iLstPto;

//            foreach (Point myPto in points)
//            {

//                myPto.Color = System.Drawing.Color.Black;
//            }

       
//            //Obtengo Lista 3  Puntos a Agrupar
//            var randomCenters = PickRandomCenters(clusters, points);

//            ////IMPORTANTE
//            ProcessGroups(points, randomCenters);

           


//        }

//        //Obtengo K Puntos Aleatorios donde Comenzar el Proceso
//        private static List<Point> PickRandomCenters(int clusters, List<Point> points)
//        {
//            //pick random points
//            var randomCenters = new List<Point>();
            
     
//            //Vamos a hacer el caso con 2 grupos

//            randomCenters.Add(points[0]);

//            randomCenters.Add(points[points.Count - 1]);


//            return randomCenters;
//        }

//        //FUNCION
//        private void ProcessGroups(List<Point> points, List<Point> randomCenters)
//        {
//            Dictionary<Point, List<Point>> centerAssignments = GetCenterAssignments(points, randomCenters);

//            // ColorClusters(centerAssignments);

//            List<Point> oldCenters = null;

//            List<Point> newCenters = null;

//            while (true)
//            {
//                //calculate average center
//                newCenters = GetNewCenters(centerAssignments);

//                if (CentersEqual(newCenters, oldCenters))
//                {
//                    break;
//                }

//                centerAssignments = GetCenterAssignments(points, newCenters);

//                ColorClusters(centerAssignments);


//                oldCenters = newCenters;
//            }



//            int mymarron = (from p in points where p.Color == System.Drawing.Color.Maroon select p).Count();
//            int myBlue = (from p in points where p.Color == System.Drawing.Color.Blue select p).Count();

//            // MessageBox.Show("Pasadas : " + Pasadas.ToString());

//            MessageBox.Show("N Puntos : " + points.Count().ToString());
//            MessageBox.Show("Amarrones: " + mymarron.ToString());
//            MessageBox.Show("Azules: " + myBlue.ToString());


//        }


//        private Dictionary<Point, List<Point>> GetCenterAssignments(List<Point> points, List<Point> centers)
//        {
//            var centerAssignments = new Dictionary<Point, List<Point>>();

//            //make them red
//            foreach (Point point in centers)
//            {
//                point.Color = System.Drawing.Color.Red;
//                centerAssignments.Add(point, new List<Point>());
//            }

          

//            foreach (Point point in points)
//            {
//                double x = point.X;
//                double y = point.Y;

//                Point closestCenter = null;
//                double closestCenterDistance = double.MaxValue;

//                foreach (Point centerPoint in centers)
//                {
//                    double centerX = centerPoint.X;
//                    double centerY = centerPoint.Y;

//                    double distance = Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));

//                    if (distance < closestCenterDistance)
//                    {
//                        closestCenterDistance = distance;
//                        closestCenter = centerPoint;
//                    }
//                }

//                lock (centerAssignments)
//                {
//                    centerAssignments[closestCenter].Add(point);
//                }
//            }

//            return centerAssignments;
//        }


//        private void ColorClusters(Dictionary<Point, List<Point>> centerAssignments)
//        {
//            var colorStack = new Stack<System.Drawing.Color>();
//            colorStack.Push(System.Drawing.Color.RoyalBlue);
//            colorStack.Push(System.Drawing.Color.Brown);
//            colorStack.Push(System.Drawing.Color.Orange);
//            colorStack.Push(System.Drawing.Color.Purple);
//            colorStack.Push(System.Drawing.Color.Green);
//            colorStack.Push(System.Drawing.Color.Magenta);
//            colorStack.Push(System.Drawing.Color.Fuchsia);
//            colorStack.Push(System.Drawing.Color.Gold);
//            colorStack.Push(System.Drawing.Color.Lavender);
//            colorStack.Push(System.Drawing.Color.Maroon);
//            colorStack.Push(System.Drawing.Color.Orchid);
//            colorStack.Push(System.Drawing.Color.Pink);
//            colorStack.Push(System.Drawing.Color.YellowGreen);
//            colorStack.Push(System.Drawing.Color.Blue);
//            colorStack.Push(System.Drawing.Color.Maroon);

//            //group            
//            foreach (Point center in centerAssignments.Keys)
//            {
//                System.Drawing.Color color = colorStack.Pop();
//                center.Color = color;
//                foreach (Point point in centerAssignments[center])
//                {
//                    point.Color = color;
//                }

                
//                fillTableroList(centerAssignments[center]);

//            }
//        }


//        private void fillTableroList(List<Point> iLstPto)
//        {


//            foreach (Point myPto in iLstPto)
//            {

//                addPoint(myPto);

//            }


//        }



//        private void addPoint(Point iPoint)
//        {


//            var myQuery = from p in points
//                          where p.X == iPoint.X & p.Y == iPoint.Y
//                          select p.Color = iPoint.Color;



//        }




//        private bool CentersEqual(List<Point> newCenters, List<Point> oldCenters)
//        {
//            if (newCenters == null || oldCenters == null)
//            {
//                return false;
//            }

//            foreach (Point newCenter in newCenters)
//            {
//                bool found = false;
//                foreach (Point oldCenter in oldCenters)
//                {
//                    if (newCenter.X == oldCenter.X && newCenter.Y == oldCenter.Y)
//                    {
//                        found = true;
//                        break;
//                    }
//                }

//                if (!found)
//                {
//                    return false;
//                }
//            }
//            return true;
//        }



//        private List<Point> GetNewCenters(Dictionary<Point, List<Point>> centerAssignments)
//        {
//            double totalX = 0;
//            double totalY = 0;

//            var newCenters = new List<Point>();

//            foreach (Point center in centerAssignments.Keys)
//            {
//                totalX = 0;
//                totalY = 0;

//                foreach (Point point in centerAssignments[center])
//                {
//                    totalX += point.X;
//                    totalY += point.Y;
//                }

//                double averageX = totalX / centerAssignments[center].Count;
//                double averageY = totalY / centerAssignments[center].Count;

//                var newCenter = new Point((int)averageX, (int)averageY);
//                newCenters.Add(newCenter);
//                newCenter.Color = System.Drawing.Color.Black;

               
//            }
//            return newCenters;
//        }









        
//    }






//    public class Point 
//    {
//        public System.Drawing.Color Color
//        {
           
            
//            get;
//            set;
//        }

//        public double X
//        {
//            get;
//            private set;
//        }

//        public double Y
//        {
//            get;
//            private set;
//        }

//        public Point(double x, double y)
//        {
//            this.X = x;
//            this.Y = y;
//        }
//    }
//}
