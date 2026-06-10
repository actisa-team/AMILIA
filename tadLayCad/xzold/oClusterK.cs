using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad
{


    using System.Windows.Forms;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;
    
    public class oClusterK
    {

        /// <summary>
        /// Número de Grupos
        /// </summary>
        int mClusters ;

        /// <summary>
        /// Listado de Puntos a Agrupar
        /// </summary>
        List<Point> mLstPoints = new List<Point>();



        public oClusterK(int iGrupos, string iLayerEntidades, eEntidades iEntidad)
        {

            //Genero los Grupos
            mClusters = iGrupos;
            
            
            SelectionSet mySs = engCadNet.oSs.getSsByLayerAndEntidad(iLayerEntidades, iEntidad);

            Point3dCollection myPtoCol = engCadNet.oSs.getPoint3dCollection(mySs);

            //Creo el listado con que trabaja el Cluster

            List<Point> myLstPointCluster = new List<Point>();

            foreach (Point3d myPtoCad in myPtoCol)
            {
                mLstPoints.Add(new Point(0,myPtoCad.X, myPtoCad.Y));
            }


            //Obtengo Lista 3  Puntos a Agrupar
            var randomCenters = PickRandomCenters();

            ////IMPORTANTE
            ProcessGroups(mLstPoints, randomCenters);

       
        }



        //Obtengo K Puntos Aleatorios donde Comenzar el Proceso
        private  List<Point> PickRandomCenters()
        {
            //pick random points
            var randomCenters = new List<Point>();
            
            //Debo de Obtener N Numeros aleatorios

            Random myRandom = new Random();

            int myPtoMax = mLstPoints.Count - 1;

            for (int i = 0; i < mClusters; i++)
            {
                randomCenters.Add(mLstPoints[myRandom.Next(0, myPtoMax)]);
            }
        

            return randomCenters;
        }

        //FUNCION
        private void ProcessGroups(List<Point> iLstPoint, List<Point> randomCenters)
        {
            Dictionary<Point, List<Point>> centerAssignments = GetCenterAssignments(iLstPoint, randomCenters);

            // ColorClusters(centerAssignments);

            List<Point> oldCenters = null;

            List<Point> newCenters = null;

            while (true)
            {
                //calculate average center
                newCenters = GetNewCenters(centerAssignments);

                if (CentersEqual(newCenters, oldCenters))
                {
                    break;
                }

                centerAssignments = GetCenterAssignments(mLstPoints, newCenters);

                ColorClusters(centerAssignments);


                oldCenters = newCenters;
            }

        }


        public void drawEnvolventeByGroup(string iLayer)
        { 
        
        
           //Generar las Envolventes

            List<Point2d> myLstPointGroup;
            List<Point2d> myLstPointEnvolvente;


            for (int i = 1; i <= mClusters; i++)
            {

                //Obtengo el Grupo
                myLstPointGroup = getGroupById(i);

                //Obtengo la Envolvente
                myLstPointEnvolvente = engCadNet.oConvexHull.getConvexHull(myLstPointGroup);

                //Genero la Polilinea

                engCadNet.oLw.addLw2d(myLstPointEnvolvente, true, iLayer);

                  
            }

        
        }


        private List<Point2d> getGroupById(int iIdGroup)
        {


            var myQuery = from p in mLstPoints
                          where p.Id == iIdGroup
                          select p;

            List<Point2d> myLstPoint = myQuery.ToList<Point>().ConvertAll(p => new Point2d(p.X, p.Y));


      


            return myLstPoint;


            


            
        }


        private Dictionary<Point, List<Point>> GetCenterAssignments(List<Point> points, List<Point> centers)
        {
            var centerAssignments = new Dictionary<Point, List<Point>>();

            //make them red
            foreach (Point point in centers)
            {
               // point.Color = System.Drawing.Color.Red;
                centerAssignments.Add(point, new List<Point>());
            }

          

            foreach (Point point in points)
            {
                double x = point.X;
                double y = point.Y;

                Point closestCenter = null;
                double closestCenterDistance = double.MaxValue;

                foreach (Point centerPoint in centers)
                {
                    double centerX = centerPoint.X;
                    double centerY = centerPoint.Y;

                    double distance = Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));

                    if (distance < closestCenterDistance)
                    {
                        closestCenterDistance = distance;
                        closestCenter = centerPoint;
                    }
                }

                lock (centerAssignments)
                {
                    centerAssignments[closestCenter].Add(point);
                }
            }

            return centerAssignments;
        }


        private void ColorClusters(Dictionary<Point, List<Point>> centerAssignments)
        {

            var  grupoStack = new Stack<int>();

            for (int i = mClusters; i > 0; i--)
            {
                grupoStack.Push(i);
            }


            //group            
            foreach (Point center in centerAssignments.Keys)
            {
                int idGrupo  = grupoStack.Pop();

                center.Id = idGrupo;

                foreach (Point point in centerAssignments[center])
                {
                    point.Id = idGrupo;
                }


                fillTableroList(centerAssignments[center]);

            }



        }


        private void fillTableroList(List<Point> iLstPto)
        {


            foreach (Point myPto in iLstPto)
            {

                addPoint(myPto);

            }


        }



        private void addPoint(Point iPoint)
        {


            var myQuery = from p in mLstPoints
                          where p.X == iPoint.X & p.Y == iPoint.Y
                          select p.Id = iPoint.Id;


        }




        private bool CentersEqual(List<Point> newCenters, List<Point> oldCenters)
        {
            if (newCenters == null || oldCenters == null)
            {
                return false;
            }

            foreach (Point newCenter in newCenters)
            {
                bool found = false;
                foreach (Point oldCenter in oldCenters)
                {
                    if (newCenter.X == oldCenter.X && newCenter.Y == oldCenter.Y)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }
            return true;
        }



        private List<Point> GetNewCenters(Dictionary<Point, List<Point>> centerAssignments)
        {
            double totalX = 0;
            double totalY = 0;

            var newCenters = new List<Point>();

            foreach (Point center in centerAssignments.Keys)
            {
                totalX = 0;
                totalY = 0;

                foreach (Point point in centerAssignments[center])
                {
                    totalX += point.X;
                    totalY += point.Y;
                }

                double averageX = totalX / centerAssignments[center].Count;
                double averageY = totalY / centerAssignments[center].Count;

                var newCenter = new Point((int)averageX, (int)averageY);
                newCenters.Add(newCenter);
                newCenter.Id = 0;

               
            }
            return newCenters;
        }
     
    }






    public class Point 
    {
        public int Id
        {
           
            
            get;
            set;
        }

        public double X
        {
            get;
            private set;
        }

        public double Y
        {
            get;
            private set;
        }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point(int iId,double x, double y)
        {
            this.Id = iId;
            this.X = x;
            this.Y = y;
        }
    }
}
