using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad
{
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;
    using tadLayShare;
    
    public class oZonaNoPaso
    {

       private  SelectionSet mSsZonasNoPaso = null;


       #region "Constructores"
       public oZonaNoPaso(string iLayerZonasMuertas)
        {
            mSsZonasNoPaso = engCadNet.oSs.getSsByLayerAndEntidad(iLayerZonasMuertas,eEntidades.LWPOLYLINE);    
        }
       #endregion





       public bool isTramoOnZonaNoPaso(double iP1X, double iP1Y, double iP2X, double iP2Y)
       {

           if (mSsZonasNoPaso == null)
           {
               return false;
           }

           if (mSsZonasNoPaso.Count == 0)
           {
               return false;
           }



           

           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {
               using (Transaction tr = oCadManager.StartTransaction())
               {

                   Line myLine = null;
                   
                  
                       //Creo la Linea 2D
                       myLine = engCadNet.oLine.addLine2d(iP1X, iP1Y, iP2X, iP2Y, "0");

                       //Creo los Puntos de Interseccion
                       Point3dCollection myLstPtoInterseccion = new Point3dCollection();

                       //Creo el Plano de Intersección
                       Plane myPlane = new Plane(new Point3d(0, 0, 0), new Vector3d(0, 0, 1));

                       foreach (ObjectId myObj in mSsZonasNoPaso.GetObjectIds())
                       {
                           Entity myEntidad = myObj.GetObject(OpenMode.ForRead) as Entity;

                           myEntidad.IntersectWith(myLine, Intersect.OnBothOperands, myPlane, myLstPtoInterseccion, new IntPtr(), new IntPtr());

                           if (myLstPtoInterseccion.Count > 0)
                           { 
                               tr.Abort();

                               return true;
                           }
                       }

                       tr.Abort();

                       return false;
                }

                   
               
               }

                
           }
       
    }
}
