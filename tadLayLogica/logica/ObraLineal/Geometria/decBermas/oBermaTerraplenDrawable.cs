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
    using tadLayShare;


   public class oBermaTerraplenDrawable : oBermaDesmonteDrawable
    {

        public oBermaTerraplenDrawable(oTaludBermasModel iBermasModel)
        :base(iBermasModel)
       
       {
           isBermaDesmonte = false;
           
       }


        public override void addDecorator(ISecDrawPlus iSecDecorator)
        {
            dicIsecDraw = new Dictionary<int, ISecDrawPlus>(iSecDecorator.dicIsecDraw);
            dicIsecDraw.Add(dicIsecDraw.Count, this);
            this.pk = iSecDecorator.pk;
            this.lado = parent.lado;
            this.geometria();
        }


        #region "INTERFACE ENVOLVENTE"

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

        protected override void geometria()
        {

            //Obtengo la Geometria del talud berma
            taludLstPol = base.geometria(previo.ptoInsertChild);


            //Determino el Punto
            eExcavacion miPtoCorreccion = funGetTerrenoCorreccionTnd(pk,lado, taludLstPol[0]);


            switch (miPtoCorreccion)
            {
                case eExcavacion.desmonte:  //desmonte

                    taludLstPol = new Point3dCollection();
                    taludLstPol.Add(previo.ptoInsertChild);
                    taludLstPol.Add(previo.ptoInsertChild.getFromTalud(true, true, bermaTaludH, 1));
                    taludDraw = true;
                    break;

                case eExcavacion.terraplen: //terreplen

                    taludDraw = true;
                    break;

                case eExcavacion.acota:
                 
                    taludLstPol = new Point3dCollection();
                    taludLstPol.Add(previo.ptoInsertChild);
                    taludLstPol.Add(previo.ptoInsertChild.getFromIncXIncY(0, 1, 0));
                    taludDraw = false;
                    break;

                default:
                    throw new oExEnumNotImplemented(miPtoCorreccion.ToString());
            }


        }



    }
}
