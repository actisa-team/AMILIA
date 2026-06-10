using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Planta
{
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    
    public class oSeccionPlantaPk
    {

       public double pk { get; set; }
       public eRoadSeccion seccionTipo { get; set; }
       public oTramoExcavacionTalud taludIzq { get; set; }
       public oTramoExcavacionTalud taludDer { get; set; }


        public oSeccionPlantaPk(double iPk,eRoadSeccion iSeccionTipo, oTramoExcavacionTalud iTaludIzq, oTramoExcavacionTalud iTaludDer)
       {
           this.pk = iPk;
           this.seccionTipo = iSeccionTipo;
           this.taludIzq = iTaludIzq;
           this.taludDer = iTaludDer;
       }



        public void drawSeccionCarreteraIntermedia (oSeccionPlantaPk iSeccionPrevia, oSeccionPlantaPk iSeccionNext, string iLayer)
        {
            #region "Margen Izquierda"

            Line miLinePreviaIzq = new Line(iSeccionPrevia.taludIzq.ptoTaludBase, this.taludIzq.ptoTaludBase);
            Line miLineNextIzq = new Line(this.taludIzq.ptoTaludBase, iSeccionNext.taludIzq.ptoTaludBase);

            Point3d miPtoPrevioIzq = miLinePreviaIzq.GetPointAtDist(miLinePreviaIzq.Length / 2);
            Point3d miPtoNextIzq = miLineNextIzq.GetPointAtDist(miLineNextIzq.Length / 2);

            Point3dCollection miColIzq = new Point3dCollection();
            miColIzq.Add(miPtoPrevioIzq);
            miColIzq.Add(this.taludIzq.ptoTaludBase);
            miColIzq.Add(miPtoNextIzq);


            engCadNet.oLw.addLw2d(miColIzq, false, iLayer);

            #endregion

            #region "Margen Derecho"

            Line miLinePreviaDer = new Line(iSeccionPrevia.taludDer.ptoTaludBase, this.taludDer.ptoTaludBase);
            Line miLineNextDer = new Line(this.taludDer.ptoTaludBase, iSeccionNext.taludDer.ptoTaludBase);

            Point3d miPtoPrevioDer = miLinePreviaDer.GetPointAtDist(miLinePreviaDer.Length / 2);
            Point3d miPtoNextDer = miLineNextDer.GetPointAtDist(miLineNextDer.Length / 2);

            Point3dCollection miColDer = new Point3dCollection();
            miColDer.Add(miPtoPrevioDer);
            miColDer.Add(this.taludDer.ptoTaludBase);
            miColDer.Add(miPtoNextDer);

            engCadNet.oLw.addLw2d(miColDer, false, iLayer);

            #endregion

           

        }
        public void drawSeccionCarreteraInicial (oSeccionPlantaPk iSeccionNext,string iLayer)
        {
            #region "Margen Izquierda"

            Line miLineNextIzq = new Line(this.taludIzq.ptoTaludBase, iSeccionNext.taludIzq.ptoTaludBase);
            Point3d miPtoNextIzq = miLineNextIzq.GetPointAtDist(miLineNextIzq.Length / 2);
            Point3dCollection miColIzq = new Point3dCollection();
            miColIzq.Add(this.taludIzq.ptoTaludBase);
            miColIzq.Add(miPtoNextIzq);

            engCadNet.oLw.addLw2d(miColIzq, false, iLayer);

            #endregion
            #region "Margen Derecha"

            Line miLineNextDer = new Line(this.taludDer.ptoTaludBase, iSeccionNext.taludDer.ptoTaludBase);
            Point3d miPtoNextDer = miLineNextDer.GetPointAtDist(miLineNextDer.Length / 2);

            Point3dCollection miColDer = new Point3dCollection();
            miColDer.Add(this.taludDer.ptoTaludBase);
            miColDer.Add(miPtoNextDer);


            engCadNet.oLw.addLw2d(miColDer, false, iLayer);

            #endregion
        }
        public void drawSeccionCarreteraFinal (oSeccionPlantaPk iSeccionPrevia,string iLayer)
        {

            #region "Margen Izquierda"

            Line miLinePreviaIzq = new Line(iSeccionPrevia.taludIzq.ptoTaludBase, this.taludIzq.ptoTaludBase);
            Point3d miPtoPrevioIzq = miLinePreviaIzq.GetPointAtDist(miLinePreviaIzq.Length / 2);
            Point3dCollection miColIzq = new Point3dCollection();
            miColIzq.Add(miPtoPrevioIzq);
            miColIzq.Add(this.taludIzq.ptoTaludBase);

            engCadNet.oLw.addLw2d(miColIzq, false, iLayer);

            #endregion
            #region "Margen Derecho"

            Line miLinePreviaDer = new Line(iSeccionPrevia.taludDer.ptoTaludBase, this.taludDer.ptoTaludBase);
    
            Point3d miPtoPrevioDer = miLinePreviaDer.GetPointAtDist(miLinePreviaDer.Length / 2);
  
            Point3dCollection miColDer = new Point3dCollection();
            miColDer.Add(miPtoPrevioDer);
            miColDer.Add(this.taludDer.ptoTaludBase);

            engCadNet.oLw.addLw2d(miColDer, false, iLayer);

            #endregion
        }


      


    



    }

    public class oTramoExcavacionTalud
    {

        public Point3d ptoTaludBase { get; set; }
        public Point3d ptoTaludHead { get; set; }
        public eExcavacion excavacion { get; set; }


        public oTramoExcavacionTalud(Point3d iPtoTaludBase, Point3d iPtoTaludHead, eExcavacion iExcavacion)
        {
            ptoTaludBase = iPtoTaludBase;
            ptoTaludHead = iPtoTaludHead;
            excavacion = iExcavacion; 
        }


        public double taludLon
        {        
            get
            {
                return ptoTaludBase.DistanceTo(ptoTaludHead);
            }
        }
    
    
    
    }
}
