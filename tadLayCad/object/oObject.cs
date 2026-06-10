using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad
{

    using engCadNet;
    using tadLayShare;


    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;

    using tadLayShare.puntoOld;
    
    public class oObject
	{

        /// <summary>
        /// Obtener una Polilinea EJE 
        /// </summary>
        public static object getEjeBasico()
        {
            Polyline3d myLw3d;
            SelectionSet mySs = engCadNet.oSs.getSsByUser("Selecciona un Eje Básico", null, eEntidades.POLYLINE, true);

            using (Transaction tr = oCadManager.StartTransaction())
            {
                myLw3d = (Polyline3d)tr.GetObject(mySs[0].ObjectId, OpenMode.ForRead);
            }

            return myLw3d as object;

        }

        public static object addLw2d(List<oP3d> iLstPto2d, bool isClosed, string iLayer, double iZ)
        {

            if (iLstPto2d.Count > 0)
            {


                Point3dCollection myLstPtoCad = new Point3dCollection();

                foreach (oP3d myPto2d in iLstPto2d)
                {
                    myLstPtoCad.Add(new Point3d(myPto2d.X.Value, myPto2d.Y.Value, iZ));
                }



                //Creo la Polilinea
                Polyline myLw = engCadNet.oLw.addLw2d(myLstPtoCad, isClosed, iLayer);



                return (Object)myLw;


            }
            else
            {
                throw new Exception("El listado de puntos es nulo");
            }



        }

        public static object addLw3d(List<oP3d> iLstPto3d, bool isClosed, string iLayer, double iZ)
        {

            if (iLstPto3d.Count > 0)
            {


                Point3dCollection myLstPtoCad = new Point3dCollection();

                foreach (oP3d myPto3d in iLstPto3d)
                {
                    myLstPtoCad.Add(new Point3d(myPto3d.X.Value, myPto3d.Y.Value, myPto3d.Z.Value));
                }

                //Creo la Polilinea
                Polyline3d myLw = engCadNet.oLw.addLw3d(myLstPtoCad, isClosed, iLayer);



                return (Object)myLw;


            }
            else
            {
                throw new Exception("El listado de puntos es nulo");
            }



        }

	}
}
