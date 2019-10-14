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
    using tadLayLan;
    
    /// <summary>
    /// Clase Para Obtener la Intersección entre 2 Polilineas considerando
    /// que la polilinea iLwForward solo puede extenderse hacía "adelante"
    /// Autocad en la Opción de Extend Alarga la Entidad por ambos extremos y no no sirve para este Caso.
    /// </summary>
    public  class oInterseccionForward
    {

       //HACK JUAN
       const  double TOLERANCIA_INTERSECCION_METROS = -0.05;

        Polyline mLwForward;
        Polyline mLwOther;
        oScw_Scu mSistemaReferencia;


        public oInterseccionForward(Polyline iLwForward, Polyline iLwOther)
        {
            mLwForward = iLwForward;
            mLwOther = iLwOther;

            if (iLwForward.NumberOfVertices.Equals(2))
            {
                mSistemaReferencia = new oScw_Scu(iLwForward.StartPoint, iLwForward.EndPoint);
            }
            else
            {
                throw new Exception(string.Format(strError.eEntidadSolo2Vertices, iLwForward.Id.ToString()));
            }
        }






        public Point3d getInterseccion ()
        {

			//Coleccion Puntos Interseccion
			Point3dCollection miColPtoInter = new Point3dCollection();
	
			mLwForward.IntersectWith(mLwOther, Intersect.ExtendThis, miColPtoInter, IntPtr.Zero, IntPtr.Zero);
	
		    if ( miColPtoInter.Count == 0)  throw new oExInterseccionNull();

            List<Point3d> miLstInterseccionSCP = mSistemaReferencia.translate_Collection_To_SCP(miColPtoInter);

            Point3d miPtoInserccionSCP;

            var miQuery_X_Positivo = from p in miLstInterseccionSCP
                                     where p.X > TOLERANCIA_INTERSECCION_METROS
                                     orderby p.X ascending
                                     select p;

            if (miQuery_X_Positivo.Any())
            {
                miPtoInserccionSCP= miQuery_X_Positivo.First();
            }
            else
            {
                throw new oExInterseccionNull();
            }

            return mSistemaReferencia.translate_Point_To_SCW(miPtoInserccionSCP);  
        }



   


    }


}
