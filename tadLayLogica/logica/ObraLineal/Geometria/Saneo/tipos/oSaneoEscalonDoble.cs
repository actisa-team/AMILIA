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
    
    public class oSaneoEscalonDobleInferior: oSaneoTerraplenModel ,ISecDrawSaneo
    {


        protected Polyline mLwTndOrigen;
        protected Polyline mLwTndOrigenOff;
        protected Polyline mLwSaneo = null;


        public oSaneoEscalonDobleInferior (Guid iIdMaterialExc,Guid iIdMaterialRelleno, double iEspesorSaneoTerraplen, double iEscalonHmax, double iPendienteMaxSinEscalon,Polyline iLwOrigen)
            : base(iIdMaterialExc,iIdMaterialRelleno, iEspesorSaneoTerraplen, iEscalonHmax, iPendienteMaxSinEscalon)
        {
            mLwTndOrigen = iLwOrigen;
        }


        #region "INTERFACE DRAWSANEO"
        public eSaneo saneoTipo
        {
            get
            {
                return eSaneo.terraplen;
            }

        }
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

        public virtual void drawSaneo(string iLayer)
        {


            List<Polyline> miLstLwSaneoDraw = new List<Polyline>();

            List<Polyline> miLstTndOrigenSplit = new List<Polyline>();
            List<Polyline> miLstTndOrigenOffSplit = new List<Polyline>();


            //Obtengo la Polilinea del OffSet
            mLwTndOrigenOff = oSaneo.drawSaneoSimple(mLwTndOrigen, color, iLayer, -espesor, false, false);

            //Parto la Linea Terreno en su Punto Inferior
            miLstTndOrigenSplit = oSaneo.splitLwPuntoMasBajo(mLwTndOrigen);

            //Parto la Linea Terreno Offset en Caso Saneo Doble
            miLstTndOrigenOffSplit = oSaneo.splitLwPuntoMasBajo(mLwTndOrigenOff);


            //Ahora Genero los Saneos Con Escalones
            foreach (Polyline fLw in miLstTndOrigenOffSplit)
            {
                miLstLwSaneoDraw.Add(oSaneo.drawSaneoEscalon(fLw, escalonHmax,color));
            }



            //Ahora debo de Generar la Polilinea del contorno
            using (Transaction tr = oCadManager.StartTransaction())
            {

                List<Polyline> miLstSaneoContorno = new List<Polyline>();



                //Creo las lineas Verticales
                //al no saber la orientacion busco el mascercano
                Point3d miP1;
                Point3d miP2;
                Polyline miLw1;
                Polyline miLw2;

                //Primer Extremo
                miP1 = miLstLwSaneoDraw[0].StartPoint;
                miP2 = miP1.getPtoMasCercano(miLstTndOrigenSplit[0].StartPoint, miLstTndOrigenSplit[0].EndPoint);
                miLw1 = oLw.addLw2d(miP1, miP2, iLayer, null,color);

                //Segundo Extremo
                miP1 = miLstLwSaneoDraw[1].StartPoint;
                miP2 = miP1.getPtoMasCercano(miLstTndOrigenSplit[1].StartPoint, miLstTndOrigenSplit[1].EndPoint);
                miLw2 = oLw.addLw2d(miP1, miP2, iLayer, null,color);


                miLstSaneoContorno.Add(miLw1);
                miLstSaneoContorno.Add(miLw2);
                miLstSaneoContorno.AddRange(miLstTndOrigenSplit);
                miLstSaneoContorno.AddRange(miLstLwSaneoDraw);

                mLwSaneo = oLw.joinLw(miLstSaneoContorno, true);

                tr.Commit();
            }


            //Borro Entidades
            oTools.entidadDelete(mLwTndOrigenOff);

        }
        #endregion 


        #region "INTERFACE MEDICION"
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


    public class oSaneoEscalonDobleSuperior : oSaneoEscalonDobleInferior
    {

      public oSaneoEscalonDobleSuperior (Guid iIdMaterialExc,Guid iIdMaterialRelleno, double iEspesorSaneoTerraplen, double iEscalonHmax, double iPendienteMaxSinEscalon,Polyline iLwOrigen)
            : base(iIdMaterialExc,iIdMaterialRelleno, iEspesorSaneoTerraplen, iEscalonHmax, iPendienteMaxSinEscalon,iLwOrigen)
        {
            
        }


      public override void drawSaneo(string iLayer)
      {


          List<Polyline> miLstLwSaneoDraw = new List<Polyline>();

          List<Polyline> miLstTndOrigenSplit = new List<Polyline>();
          List<Polyline> miLstTndOrigenOffSplit = new List<Polyline>();


          //Obtengo la Polilinea del OffSet
          mLwTndOrigenOff = oSaneo.drawSaneoSimple(mLwTndOrigen, color, iLayer, -espesor, false, false);

          //Parto la Linea Terreno en su Punto Inferior
          miLstTndOrigenSplit = oSaneo.splitLwPuntoMasAlto(mLwTndOrigen);

          //Parto la Linea Terreno Offset en Caso Saneo Doble
          miLstTndOrigenOffSplit = oSaneo.splitLwPuntoMasAlto(mLwTndOrigenOff);


          //Ahora Genero los Saneos Con Escalones
          foreach (Polyline fLw in miLstTndOrigenOffSplit)
          {
              miLstLwSaneoDraw.Add(oSaneo.drawSaneoEscalon(fLw, escalonHmax, color));
          }



          //Ahora debo de Generar la Polilinea del contorno
          using (Transaction tr = oCadManager.StartTransaction())
          {

              List<Polyline> miLstSaneoContorno = new List<Polyline>();



              //Creo las lineas Verticales
              //al no saber la orientacion busco el mascercano
              Point3d miP1;
              Point3d miP2;
              Polyline miLw1;
              Polyline miLw2;
              Polyline miLwUnion_LWEscalones;

              //Primer Extremo
              miP1 = miLstLwSaneoDraw[0].EndPoint;
              miP2 = miP1.getPtoMasCercano(miLstTndOrigenSplit[0].StartPoint, miLstTndOrigenSplit[0].EndPoint);
              miLw1 = oLw.addLw2d(miP1, miP2, iLayer, null, color);

              //Segundo Extremo
              miP1 = miLstLwSaneoDraw[1].EndPoint;
              miP2 = miP1.getPtoMasCercano(miLstTndOrigenSplit[1].StartPoint, miLstTndOrigenSplit[1].EndPoint);
              miLw2 = oLw.addLw2d(miP1, miP2, iLayer, null, color);


              //Poliinea de Union entre
              miLwUnion_LWEscalones = oLw.addLw2d(miLstLwSaneoDraw[0].StartPoint, miLstLwSaneoDraw[1].StartPoint, iLayer, null, color);


              if (miLwUnion_LWEscalones.Length > 0)
              {
                  miLstSaneoContorno.Add(miLwUnion_LWEscalones);
              }

              miLstSaneoContorno.Add(miLw1);
              miLstSaneoContorno.Add(miLw2);
              miLstSaneoContorno.AddRange(miLstTndOrigenSplit);
              miLstSaneoContorno.AddRange(miLstLwSaneoDraw);

              mLwSaneo = oLw.joinLw(miLstSaneoContorno, true);

              tr.Commit();
          }


          //Borro Entidades
          oTools.entidadDelete(mLwTndOrigenOff);

      }


    }


}
