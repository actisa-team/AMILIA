using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.medicion
{


    using engCadNet;
    using engCadNet.entidades;

    using tadLayLogica.EjeTJ;
    using tadLayLogica.EjeTJ.Tramos;
    using tadLayLogica.EjeTJ.Vertice;
    using tadLayLogica.EjeTJ.Secciones;

    using tadLayLogica.logica.medicion;

    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    
    public class oSecMedicionTerraplenDesmonte : ISecMedicion
    {


        Guid mDesmonteMat;
        Guid mTerraplenMat;

        Polyline mLwEnvolvente;
        Polyline mLwTnd;
        double mSecIntervalo = 0.1;

        List<oSeccionSimple> mLstSecciones = new List<oSeccionSimple>();

        

        #region "Constructor"
        public oSecMedicionTerraplenDesmonte(Guid iMatDes, Guid iMatTer, Polyline iLwEnvolvente, Polyline iLwTnd)
        {

           mDesmonteMat = iMatDes;
           mTerraplenMat = iMatTer;

            mLwEnvolvente = iLwEnvolvente;
            mLwTnd = iLwTnd;

            if (!mLwEnvolvente.isLeftToRight())
            {
                using (oLwPlus miLw = new oLwPlus(mLwEnvolvente))
                {
                    miLw.lw.ReverseCurve();
                }
            }

            if (!mLwTnd.isLeftToRight())
            {
                using (oLwPlus miLw = new oLwPlus(mLwTnd))
                {
                    miLw.lw.ReverseCurve();
                }

            }

            //Obtengo la Discretizacion de Puntos
            Point3dCollection miLstPtoDiscre = oLw.discretizarPolilinea(mLwEnvolvente,mSecIntervalo);

            //Obtengo las Lineas de Interseccion
            foreach (Point3d item in miLstPtoDiscre)
            {
                mLstSecciones.Add(getSeccion(item));
            }

  
        }
        #endregion
        #region "Propiedades"

        public List<oMedItemModel> medicion
        {

            get
            {

                List<oMedItemModel> miLstMedicion = new List<oMedItemModel>();

                double miDesmonteM2;
                double miTerraplenM2;


                //Reiniciamos los Valores del Id
                var miDesmonte = from p in mLstSecciones
                                 where p.terrenoCorrecion == eExcavacion.desmonte
                                 select p.desfaseAbs;

                miDesmonteM2 = mSecIntervalo * miDesmonte.Sum();


                //Reiniciamos los Valores del Id
                var myTerraplen = from p in mLstSecciones
                                  where p.terrenoCorrecion == eExcavacion.terraplen
                                  select p.desfaseAbs;

                miTerraplenM2 = mSecIntervalo * myTerraplen.Sum();



                if (miDesmonteM2 > 0.1)
                {
                    miLstMedicion.Add(new oMedExcDesmonte(mDesmonteMat, miDesmonteM2));
                }

                if (miTerraplenM2 > 0.1)
                {
                    miLstMedicion.Add(new oMedRellenoTerraplen(mTerraplenMat, miTerraplenM2));
                }



                if (miLstMedicion.Count == 0)
                {
                    throw new Exception("Los Valores de la medición de desmonte y Terraplen son nulos");
                }
                else
                {
                    return miLstMedicion;
                }

            }

        }

        #endregion
        #region "Metodos Privados"

        private oSeccionSimple getSeccion(Point3d iPtoExplanada)
        {

            oSeccionSimple miSeccion;
            Point3d miPtoV1 = iPtoExplanada;
            Point3d miPtoV2 = miPtoV1.getFromIncXIncY(0, 1, 0);


            Line miLineV = new Line(miPtoV1, miPtoV2);


           // oTools.entidadAdd(miLineV, "0");

            Point3dCollection miCol = new Point3dCollection();

            miLineV.IntersectWith(mLwTnd, Intersect.ExtendThis, miCol, IntPtr.Zero, IntPtr.Zero);

            Point3d miPtoInter = Point3d.Origin;

            if (miCol.Count > 0)
            {
                miPtoInter = miPtoInter.getPtoMasLejano(miCol);

                miSeccion = new oSeccionSimple(miPtoInter.to2d(), miPtoV1.to2d());

              // oLine.addLine2d(miPtoInter.to2d(), miPtoV1.to2d(), "0");
            }
            else
            {
                throw new Exception("Error al Obtener la Intersección en el Eje Medicion Terraplen-Desmonte.");
            }

            return miSeccion;

        }

        #endregion

  
    }
}
