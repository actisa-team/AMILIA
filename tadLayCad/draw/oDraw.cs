using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad.draw
{
    using engCadNet;
    using tadLayShare;


    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;
    using tadLayShare.puntoOld;
    
    public class oDraw
    {





      
       


        public static void addLw3d(List<oP3d> iLstPto3d, bool isClosed, string iLayer)
        {

            if (iLstPto3d.Count > 0)
            {
                Point3dCollection myLstPtoCad = new Point3dCollection();

                foreach (oP3d myPto3d in iLstPto3d)
                {
                    myLstPtoCad.Add(new Point3d(myPto3d.X.Value, myPto3d.Y.Value,myPto3d.Z.Value));
                }

                //Creo la Polilinea
                engCadNet.oLw.addLw3d(myLstPtoCad, isClosed, iLayer);
            }
            else
            {
                throw new Exception("El listado de puntos es nulo");
            }
        }



    }
}
