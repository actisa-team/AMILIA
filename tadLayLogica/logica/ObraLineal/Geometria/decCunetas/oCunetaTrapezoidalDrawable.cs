using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;

    using tadLayLogica.Secciones.Interfaz;
    using tadLayLogica.logica.medicion;
    using tadLayShare;


    public class oCunetaTrapezoidalDrawable : oCunetaTrapezoidalModel, ISecDrawPlus
    {


        /// <summary>
        /// Talud del Desmonte Terraplen
        /// </summary>
        public double taludH { get; set; }

        private Point3dCollection mLstGeometria = null;





        public oCunetaTrapezoidalDrawable(Guid iIdMaterial,double iAnchoSuperior, double iAnchoInferior, double iEspesor, double iAlturaInterior, double iTaludH)

            : base(iIdMaterial, iAnchoSuperior,iAnchoInferior,iEspesor,iAlturaInterior)
        {
            
            taludH = iTaludH;
        }

        public oCunetaTrapezoidalDrawable(oCunetaTrapezoidalModel iCunetaModel, double iTaludH)
           :base(iCunetaModel.material,iCunetaModel.anchoSuperiorInt,iCunetaModel.anchoInferior,iCunetaModel.espesor,iCunetaModel.alturaInterior)
        {
           
            taludH = iTaludH;
        }


        #region "INTERFACE ENVOLVENTE"

        /// <summary>
        /// Puntos Envolvente Inferior
        /// </summary>
        public Point3dCollection envolvente
        {
            get
            {
                Point3dCollection miCol = new Point3dCollection();
                miCol.Add(cC3);

                return miCol;
            }
        }


        #endregion


        #region "INTERFACES DRAW"

        public double pk { get; set; }
        public eLado lado { get; set; }

        public Point3dCollection taludLstPol { get; set; }
        public Dictionary<int, ISecDrawPlus> dicIsecDraw { get; set; }
        public ISecDrawPlus parent
        {
            get { return dicIsecDraw[0]; }
        }
        public ISecDrawPlus previo
        {
            get { return dicIsecDraw[dicIsecDraw.Count - 2]; }
        }
        public Point3d ptoInsertChild
        {
            get
            {
                return cC3;
            }
    
        }
        public bool taludDraw
        { 
            get {return true;} set{;} 
        }


        public void addDecorator(ISecDrawPlus iSecDecorator)
        {
            dicIsecDraw = new Dictionary<int, ISecDrawPlus>(iSecDecorator.dicIsecDraw);
            dicIsecDraw.Add(dicIsecDraw.Count, this);
            pk = parent.pk;
            lado = parent.lado;
            mLstGeometria=base.geometria(previo.ptoInsertChild);
            taludLstPol = getTalud();
        }


        public void draw(string iLayer, Matrix3d? iMatrix)
        {
            #region "DECORATOR"
            previo.draw(iLayer, iMatrix);
            #endregion

            #region "DRAW"

            if (mLstGeometria != null)
            {
                oLw.addLw2d(mLstGeometria, true, iLayer, iMatrix, oSeccionDecoradorParent.colorDecorator);
            }

            #endregion

        }



        private Point3dCollection getTalud ()
        {

            Point3dCollection miCol = new Point3dCollection();
            miCol.Add(cC3);

            eExcavacion miPunto = funGetTerrenoCorreccionTnd(parent.pk, parent.lado,cC3);

            switch (miPunto)
            {
                case eExcavacion.desmonte:
                    miCol.Add(cC3.getFromTalud(true, true, taludH, 1));
                 
                    break;
                case eExcavacion.terraplen:
                    miCol.Add(cC3.getFromTalud(true, false, taludH, 1));
                    
                    break;
                case eExcavacion.acota:
                    miCol.Add(cC3.getFromIncXIncY(0, 1, 0));
                    
                    break;
                default:
                    throw new oExEnumNotImplemented(miPunto.ToString());
            }

            return miCol;

        }

        #endregion


        #region "INTERFACE MEDICION"

        public List<oMedItemModel> medicion
        {
            get
            {
                oMedItemModel miCuneta = new oMedCunetaTrapezoidal(material,1);
                List<oMedItemModel> miMedicion = new List<oMedItemModel>();
                miMedicion.Add(miCuneta);
                return miMedicion;
            }
        }

        #endregion




    }
}
