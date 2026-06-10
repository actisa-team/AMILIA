using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;

    using tadLayLogica.logica.medicion;
    using tadLayLogica.Secciones.Interfaz;
    using tadLayShare;

    public class oCunetaTriangularDrawable : oCunetaTriangularModel, ISecDrawPlus
    {

        /// <summary>
        /// Talud del Desmonte Terraplen
        /// </summary>
        public double taludH { get; set; }

        private Point3dCollection mLstGeometria = null;


        public oCunetaTriangularDrawable(Guid iIdMaterial, double iAnchoSuperior, double iEspesor, double iAlturaInterior,double iTaludDesmonteH)                       
            : base(iIdMaterial,iAnchoSuperior,iEspesor,iAlturaInterior)
        {
            taludH = iTaludDesmonteH;
        }


        public oCunetaTriangularDrawable(oCunetaTriangularModel iCunetaTriangularMode,double iTaludDesmonteH)
          :base (iCunetaTriangularMode.material,iCunetaTriangularMode.anchoSuperiorInt,iCunetaTriangularMode.espesor,iCunetaTriangularMode.alturaInterior)
        
        {
            taludH = iTaludDesmonteH;
        }
       

        /// <summary>
        /// No se Añaden Puntos // No Cuenta
        /// </summary>
        public Point3dCollection envolvente
        {
            get
            {

                Point3dCollection miCol = new Point3dCollection();
                miCol.Add(cC2);

                return miCol;
               
            }
        }




        


        #region "INTERFACE IDRAW"

       public Dictionary<int, ISecDrawPlus> dicIsecDraw { get; set; }
       public ISecDrawPlus parent{ get {return dicIsecDraw[0] ;} }
       public ISecDrawPlus previo { get { return dicIsecDraw[dicIsecDraw.Count - 2]; } }
       public Point3d ptoInsertChild { get { return cC2; }  set{;}}
       public bool taludDraw { get; set; }
       public double pk { get; set; }

       public eLado lado { get; set; }
    
       public Point3dCollection taludLstPol { get; set; }
      

        public void addDecorator(ISecDrawPlus iSecDecorator)
        {
            dicIsecDraw = new Dictionary<int, ISecDrawPlus>(iSecDecorator.dicIsecDraw);
            dicIsecDraw.Add(dicIsecDraw.Count, this);

            pk = parent.pk;
            lado = parent.lado;

            mLstGeometria = geometria(previo.ptoInsertChild);
            taludLstPol = getTalud();
            taludDraw = true;
        }
        

        public void draw(string iLayer, Matrix3d? iMatrix)
        {
            #region "DECORATOR"

            if (dicIsecDraw != null)
            {
                previo.draw(iLayer, iMatrix);
            }
   
            #endregion

            #region "DRAW"              
            oLw.addLw2d(mLstGeometria, true, iLayer, iMatrix, oSeccionDecoradorParent.colorDecorator);
            #endregion
        }





        #endregion


        private Point3dCollection getTalud()
        {

            Point3dCollection miColLw = new Point3dCollection();

            eExcavacion miPtoC2 =   funGetTerrenoCorreccionTnd(pk, lado,cC2);
                                    

            miColLw.Add(cC2);


            switch (miPtoC2)
            {
                case eExcavacion.desmonte:
                    miColLw.Add(cC2.getFromTalud(true, true, taludH, 1));
                    break;
                case eExcavacion.terraplen:
                    miColLw.Add(cC2.getFromTalud(true, false, taludH, 1));
                    break;
                case eExcavacion.acota:
                    miColLw.Add(cC2.getFromIncXIncY(0, 2, 0));
                    break;
                default:
                    throw new oExEnumNotImplemented(miPtoC2.ToString());
            }


            return miColLw;
        
        
        
        }


        #region "INTERFACE MEDICION"

        public List<oMedItemModel> medicion
        {
            get
            {
                oMedItemModel miCuneta = new oMedCunetaTriangular(material,1);
                List<oMedItemModel> miMedicion = new List<oMedItemModel>();
                miMedicion.Add(miCuneta);
                return miMedicion;
            }
        }

        #endregion



    }


}
