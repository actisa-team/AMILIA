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
	using Autodesk.AutoCAD.EditorInput;
	
	
   //HACK JUAN EEROR SECCIONES
   public class oScw_Scu
   {

	   /// <summary>
	   /// Matriz Sistema Coordenadas SCW (Sistema Referencia Universal) ; SCP (Sistema Referencia Personal)
	   /// </summary>
	   Matrix2d mMatrixSCW_to_SCP ;
	   /// <summary>
	   /// Matriz Sistema Coordenadas SCP (Sistema Referencia Personal) ; SCW (Sistema Referencia Universal)
	   /// </summary>
	   Matrix2d mMatrixSCP_to_SCW ;


	   #region "Constructor"
	   

	   public oScw_Scu(Point3d iPtoIni, Point3d iPtoFin)
		  :this(iPtoIni.to2d(),iPtoFin.to2d())   
	   {

	   }

	   public oScw_Scu (Point2d iPtoIni, Point2d iPtoFin)
	   {

			Line2d miLineIniFin = new Line2d(iPtoIni, iPtoFin);

			double miX = miLineIniFin.Direction.X;
			double miY = miLineIniFin.Direction.Y;

			Vector2d miVectorSCW_X = Autodesk.AutoCAD.Geometry.Vector2d.XAxis;
			Vector2d miVectorSCW_Y = Autodesk.AutoCAD.Geometry.Vector2d.YAxis;

			mMatrixSCW_to_SCP = Matrix2d.AlignCoordinateSystem(iPtoIni, 
															   new Vector2d(miX, miY), 
															   new Vector2d(-miY, miX),
															   Point2d.Origin,
															   miVectorSCW_X,
															   miVectorSCW_Y);



			mMatrixSCP_to_SCW =  Matrix2d.AlignCoordinateSystem(Point2d.Origin,
																miVectorSCW_X,
																miVectorSCW_Y,
																iPtoIni,
																new Vector2d(miX, miY),
																new Vector2d(-miY, miX));
	   }

	   #endregion


	   /// <summary>
	   /// Convertir Coordenadas del Sistema Universal al Sistema Personal
	   /// </summary>
	   public Point3d translate_Point_To_SCP (Point3d iPtoSCW)
	   {
		   Point2d miPtoSCW = iPtoSCW.to2d();

		   Point2d miPtoSCP = miPtoSCW.TransformBy(mMatrixSCW_to_SCP);

		   return miPtoSCP.convertTo3D();
	   }


	   /// <summary>
	   /// Convertir Coordenadas del Sistema Personal al Universal
	   /// </summary>
	   /// <param name="iPtoSCP"></param>
	   /// <returns></returns>
	   public Point3d translate_Point_To_SCW (Point3d iPtoSCP)
	   {

		   Point2d miPtoSCP = iPtoSCP.to2d();

		   Point2d miPtoSCW = miPtoSCP.TransformBy(mMatrixSCP_to_SCW);

		   return miPtoSCW.convertTo3D();
	   }



	   public List<Point3d> translate_Collection_To_SCP (Point3dCollection iLstPtoSCW)
	   {

		   List<Point3d> miLstOut = new List<Point3d>();

		   Point3d miPtoSCP;

		   foreach (Point3d item in iLstPtoSCW)
		   {
			   miPtoSCP = this.translate_Point_To_SCP(item);

			   miLstOut.Add(miPtoSCP);
		   }

		   return miLstOut;           
	   }



   }

 
}
