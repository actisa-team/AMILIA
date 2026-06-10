using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria.Saneo
{

    using engCadNet;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;

    using tadLayLogica.logica.medicion;
    using tadLayLogica.Secciones.Interfaz;
    
    public class oSaneoEscalon: oSaneoTerraplenModel, ISecDrawSaneo
    {



        private Polyline mLwTndOrigen = null;
        private Polyline mLwSaneo = null;




        #region "CONSTRUCTOR"
        public oSaneoEscalon(Guid iIdMaterialExc,Guid iIdMaterialRelleno, double iEspesorSaneoTerraplen, double iEscalonHmax, double iPendienteMaxSinEscalon,Polyline iLwOrigen)
           :base(iIdMaterialExc,iIdMaterialRelleno,  iEspesorSaneoTerraplen,iEscalonHmax,iPendienteMaxSinEscalon)
        {
            mLwTndOrigen = iLwOrigen;
        }
        #endregion

        #region "INTERFACE SANEO DRAW"
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
                return eSaneo.terraplen;
            }

        }
        public void drawSaneo(string iLayer)
        {

            List<Polyline> miLstLwSaneoContorno = new List<Polyline>();

            Polyline miLwTndOrigenOff = oSaneo.drawSaneoSimple(mLwTndOrigen, color, iLayer, - espesor, false, false);

            Polyline miLwSaneoInferior = oSaneo.drawSaneoEscalon(miLwTndOrigenOff, escalonHmax,color);

            //Ahora debemos de Unir la Lw Terreno + Lw Saneo + Lw Verticales

            Point3d miP1;
            Point3d miP2;
            Polyline miLw1;
            Polyline miLw2;

            //Primer Extremo
            miP1 = miLwSaneoInferior.StartPoint;
            miP2 = miP1.getPtoMasCercano(mLwTndOrigen.StartPoint, mLwTndOrigen.EndPoint);
            miLw1 = oLw.addLw2d(miP1, miP2, iLayer, null,color);


            //Segundo Extremo
            miP1 = miLwSaneoInferior.EndPoint;
            miP2 = miP1.getPtoMasCercano(mLwTndOrigen.StartPoint, mLwTndOrigen.EndPoint);
            miLw2 = oLw.addLw2d(miP1, miP2, iLayer, null, color);

            //Copio el Objeto
            Polyline miLwTndOrigenCopy = mLwTndOrigen.Clone() as Polyline;
            oTools.entidadAdd(miLwTndOrigenCopy, iLayer);


            miLstLwSaneoContorno.Add(miLw1);
            miLstLwSaneoContorno.Add(miLw2);
            miLstLwSaneoContorno.Add(miLwTndOrigenCopy);
            miLstLwSaneoContorno.Add(miLwSaneoInferior);

            mLwSaneo = oLw.joinLw(miLstLwSaneoContorno, true);


        }
        #endregion

        #region "INTERFACE MEDICIONES"
        public List<oMedItemModel> medicion
        {
            get
            {
                List<oMedItemModel> miMedicion = new List<oMedItemModel>();

                miMedicion.Add(new oMedExcSaneoTerraplen(materialExcavacion, lwSaneo.Area));
                miMedicion.Add(new oMedRellenoSaneoTerraplen(materialRelleno, lwSaneo.Area));

               return miMedicion;
            }
        }
        #endregion

       
    }


}
