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
    
    public class oBermaDesmonteDrawable: oTaludBermasModel, ISecDrawPlus
    {




        public oBermaDesmonteDrawable(double iBermaHmax, bool iIsBermaPie, double iBermaAncho,double iBermaAlto, double iBermaTaludH)
           :base(iBermaHmax,iIsBermaPie,iBermaAncho,iBermaAlto,iBermaTaludH)
        {
            isBermaDesmonte = true;
        }

      
       public oBermaDesmonteDrawable(oTaludBermasModel iBermasModel)
           :base(iBermasModel.Hmax,iBermasModel.isBermaPie,iBermasModel.bermaAncho,iBermasModel.bermaAlto,iBermasModel.bermaTaludH)
       
       {
           isBermaDesmonte = true;
       }







  

       #region "INTERFACE IDRAW"

      public Dictionary<int, ISecDrawPlus> dicIsecDraw { get; set; }
      public ISecDrawPlus parent { get { return dicIsecDraw[0]; } }
      public ISecDrawPlus previo { get { return dicIsecDraw[dicIsecDraw.Count - 2]; } }
      public Point3d ptoInsertChild { get; set; }
      public bool taludDraw { get; set; }
      public double pk { get; set; }
      public eLado lado { get; set; }
   
      public Point3dCollection taludLstPol { get; set; }



      

       public virtual void addDecorator(ISecDrawPlus iSecDecorator)
       {
           dicIsecDraw = new Dictionary<int, ISecDrawPlus>(iSecDecorator.dicIsecDraw);
           dicIsecDraw.Add(dicIsecDraw.Count, this);
           pk = parent.pk;
           lado = parent.lado;
           geometria();
       }
       protected virtual void geometria()
       {

           //Obtengo la Geometria del talud berma desmonte
           taludLstPol = base.geometria(previo.ptoInsertChild);


           //Determino el Punto
           eExcavacion miPtoCorreccion = funGetTerrenoCorreccionTnd(pk,lado, taludLstPol[0]);


           switch (miPtoCorreccion)
           {
               case eExcavacion.desmonte:

                   taludDraw = true;
                   break;


               case eExcavacion.terraplen:

                   taludDraw = false;

                   taludLstPol = new Point3dCollection();
                   taludLstPol.Add(previo.ptoInsertChild);
                   taludLstPol.Add(previo.ptoInsertChild.getFromIncXIncY(0, -1, 0));
                   break;

               case eExcavacion.acota:

                   taludDraw = false;

                   taludLstPol = new Point3dCollection();
                   taludLstPol.Add(previo.ptoInsertChild);
                   taludLstPol.Add(previo.ptoInsertChild.getFromIncXIncY(0, 1, 0));
                   break;
               default:
                   throw new oExEnumNotImplemented(miPtoCorreccion.ToString());
           }


       }
       public void draw(string iLayer,Matrix3d? iMatrix)
       {

           #region "DECORATOR"
           previo.draw(iLayer, iMatrix);
           #endregion
       }
       /// <summary>
       /// No se Añaden Puntos // No Cuenta
       /// </summary>
       public Point3dCollection envolvente
       {
           get
           {
               return new Point3dCollection();
           }
       }

      

       #endregion

       #region "INTERFACE MEDICION"

       //No se Mide
       public List<oMedItemModel> medicion
       {
           get { return new List<oMedItemModel>(); }
       }

       #endregion




   
    }
}
