using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria.Saneo
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    using engCadNet;

    using tadLayLogica.logica.medicion;
    using tadLayLogica.Secciones.Interfaz;
    using tadLayShare;
    
    public class oSaneoSimple : oSaneoDesmonteModel, ISecDrawSaneo
    {



       private Polyline mLwOrigen=null;
       private Polyline mLwSaneo = null;
       private eSaneo mSaneoTipo;

        /// <summary>
        /// TODA LA SECCION EN DESMONTE
        /// </summary>
        public oSaneoSimple(Guid iIdMaterialExc, Guid iIdMaterialRel, double iEspesorSaneo,Polyline iLwOrigen, eSaneo iSaneoTipo) 
            :base(iIdMaterialExc,iIdMaterialRel,iEspesorSaneo)
        
        {
           mLwOrigen = iLwOrigen;
           mSaneoTipo = iSaneoTipo;
       }



        #region "INTERFACE SANEO"
        public Polyline lwSaneo
        {
            get
            {

                if (mLwSaneo != null)
                {
                    return mLwSaneo;
                }
                else
                {
                    throw new Exception("La Polilinea Saneo es Nula");           
                }         
            }
        }
        public eSaneo saneoTipo
        {
            get
            {
                return mSaneoTipo;
            }

        }
        public void drawSaneo(string iLayer)
        {
            mLwSaneo = oSaneo.drawSaneoSimple(mLwOrigen,color,iLayer, -espesor, true,true);
        }
        #endregion


        #region "INTERFACE MEDICIONES"
        public List<oMedItemModel> medicion
        {
            get
            {
                List<oMedItemModel> miMedicion = new List<oMedItemModel>();
                oMedItemModel miSaneoMedicionRelleno;
                oMedItemModel miSaneoMedicionExcavacion;

                switch (saneoTipo)
                {
                    case eSaneo.terraplen:

                      miSaneoMedicionExcavacion = new oMedExcSaneoTerraplen(materialExcavacion, lwSaneo.Area);
                      miSaneoMedicionRelleno = new oMedRellenoSaneoTerraplen(materialRelleno, lwSaneo.Area);
                     
                      break;
   
                    case eSaneo.desmonte:

                      miSaneoMedicionExcavacion = new oMedExcSaneoDesmonte(materialExcavacion, lwSaneo.Area);
                      miSaneoMedicionRelleno = new oMedRellenoSaneoDesmonte(materialRelleno,lwSaneo.Area);

                      break;
                      
                    default:
                        throw new oExEnumNotImplemented(saneoTipo.ToString());                   
                }   
     
    
                miMedicion.Add(miSaneoMedicionExcavacion);
                miMedicion.Add(miSaneoMedicionRelleno);

                return miMedicion;

            }
        }
        #endregion


     



    }
}
