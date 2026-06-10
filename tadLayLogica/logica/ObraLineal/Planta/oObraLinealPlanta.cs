using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Planta
{

    using engCadNet;

    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;


    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Colors;

    using engNet.Extension.Double;
    using tadLayShare;

    /// <summary>
    /// Obra Lineal Planta
    /// </summary>
    public  class oObraLinealPlantaDrawable : IDisposable
    {

        private Guid mSolId = Guid.Empty;
        private string mSolName = string.Empty;

        private List<oSeccionPlantaPk> mLstPlantaExcavacion;


        private double? mLonExpropiacion = null;

        private string mLayerPlanta;
        private string mLayerDominioPublicoAdyacente;

        private List<Entity> mLstPlantaLineasMuestreo = new List<Entity>();
 
        private List<Entity> mLstPlantaTaludLines = new List<Entity>();
      
        private List<oExcavacionHatch> mLstPlantaHatch = new List<oExcavacionHatch>();



        #region "Constructor"

        public oObraLinealPlantaDrawable(Guid iIdSol, string iSolName, List<oSeccionPlantaPk> iLstPlantaExcavacion)
        {
            mSolId = iIdSol;
            mSolName = iSolName;
            mLstPlantaExcavacion = iLstPlantaExcavacion;
            mLonExpropiacion = oDalPresupuesto.getPresupuestoRow().pcaZonaServidumbre;

            var myQuery = from p in mLstPlantaExcavacion
                          orderby p.pk ascending
                          select p;

            mLstPlantaExcavacion = myQuery.ToList<oSeccionPlantaPk>();

        }

        #endregion
        #region "Propiedades "

         public static Color colorTerraplen { get { return oColor.getInstance.verde;}}
         public static Color colorDesmonte { get { return oColor.getInstance.rojo;}}
         public static Color colorAcota { get { return oColor.getInstance.grisClaro; } }
         public static Color getColor(eExcavacion iExcavacion)
         {

             switch (iExcavacion)
             {
                 case eExcavacion.desmonte:
                     return colorDesmonte;
                 case eExcavacion.terraplen:
                     return colorTerraplen;
                 case eExcavacion.acota:
                     return colorAcota;
                 default:
                     throw new oExEnumNotImplemented(iExcavacion.ToString());
             }


         }        

         #endregion
        #region "Metodos Publicos"

         public void draw(string iLayerPlanta, string iLayerDominioPublicoAdyacente, bool iSaveToDb = true)
         {
             mLayerPlanta = iLayerPlanta;
             mLayerDominioPublicoAdyacente = iLayerDominioPublicoAdyacente;
             drawLineasMuestro();
             drawPlantaCarretera();
             drawPlantaTalud();
             drawExpropiacion(iSaveToDb);
         }


         #endregion
        #region "Metodos Privados"





         private void drawLineasMuestro()
         {

             Line miLine;
             double miAngle;
             DBText myText;

             for (int i = 0; i < mLstPlantaExcavacion.Count; i++)
             {

                 miLine = new Line(mLstPlantaExcavacion[i].taludDer.ptoTaludHead, mLstPlantaExcavacion[i].taludIzq.ptoTaludHead);

                 miLine.Layer = mLayerPlanta;

                 miAngle = miLine.Angle;

                 myText = new DBText();

                 myText.SetDatabaseDefaults();

                 myText.Height = 1;

                 myText.HorizontalMode = TextHorizontalMode.TextLeft;
                 myText.VerticalMode = TextVerticalMode.TextVerticalMid;
                 myText.AlignmentPoint = miLine.EndPoint;
                 myText.Rotation = miLine.Angle;
                 myText.TextString = "      pk: " + mLstPlantaExcavacion[i].pk.roundOff(3).ToString();
                 myText.Color = oColor.getInstance.amarillo;
                 myText.Layer = mLayerPlanta;


                 mLstPlantaLineasMuestreo.Add(myText);
                 mLstPlantaLineasMuestreo.Add(miLine);

             }


             oTools.entidadesAdd(mLstPlantaLineasMuestreo);

         }

         private void drawPlantaCarretera()
         {

             int iSeccionIncial = 0;
             int iSeccionFinal = mLstPlantaExcavacion.Count - 1;
             int iSeccionPenultima = mLstPlantaExcavacion.Count - 2;

             if (mLstPlantaExcavacion.Count > 0)
             {
                 //Genero la Seccion Inicial
                 mLstPlantaExcavacion[iSeccionIncial].drawSeccionCarreteraInicial(mLstPlantaExcavacion[1], mLayerPlanta);



                 //Genero las Secciones Intermedias
                 for (int i = 1; i < iSeccionFinal; i++)
                 {
                     mLstPlantaExcavacion[i].drawSeccionCarreteraIntermedia(mLstPlantaExcavacion[i - 1], mLstPlantaExcavacion[i + 1], mLayerPlanta);
                 }


                 //Genero la Seccion Final
                 mLstPlantaExcavacion[iSeccionFinal].drawSeccionCarreteraFinal(mLstPlantaExcavacion[iSeccionPenultima], mLayerPlanta);
             }
         }





         private void drawPlantaTalud()
         {

             eRoadSeccion miSeccionTipoCurrent;
             eRoadSeccion miSeccionTipoNext;


             //Genero la Representación en Planta de la Excavacion (Lineas)
             for (int i = 0; i < mLstPlantaExcavacion.Count - 1; i++)
             {

                 miSeccionTipoCurrent = mLstPlantaExcavacion[i].seccionTipo;
                 miSeccionTipoNext = mLstPlantaExcavacion[i + 1].seccionTipo;

                 if (miSeccionTipoCurrent == eRoadSeccion.calzada && miSeccionTipoNext == eRoadSeccion.calzada)
                 {
                     //Lado Izquierdo
                     getPlantaExcavacionLinesHatchFactorySeccionCalzada(mLstPlantaExcavacion[i].taludIzq, mLstPlantaExcavacion[i + 1].taludIzq);

                     //Lado Derecho
                     getPlantaExcavacionLinesHatchFactorySeccionCalzada(mLstPlantaExcavacion[i].taludDer, mLstPlantaExcavacion[i + 1].taludDer);
                 }
                 else 
                 {
                     //Lado Izquierdo
                     getPlantaExcavacionLinesHatchFactorySeccionMixto(miSeccionTipoCurrent,  mLstPlantaExcavacion[i].taludIzq, miSeccionTipoNext, mLstPlantaExcavacion[i + 1].taludIzq);

                     getPlantaExcavacionLinesHatchFactorySeccionMixto(miSeccionTipoCurrent, mLstPlantaExcavacion[i].taludDer, miSeccionTipoNext, mLstPlantaExcavacion[i + 1].taludDer);
                 }

             }

             //Guardo los Objeto en la Base de Datos
             oTools.entidadesAdd(mLstPlantaTaludLines);


             //Genero el HATCH
             foreach (oExcavacionHatch item in mLstPlantaHatch)
             {
                 item.drawHatch(mLayerPlanta);
             }

         }


         private void drawExpropiacion(bool iSaveToDb)
         {

             List<Point3d> miLstPtoDer = new List<Point3d>();
             List<Point3d> miLstPtoIzq = new List<Point3d>();

             for (int i = 0; i < mLstPlantaExcavacion.Count; i++)
             {
                 Point3d miPtoDer;
                 Point3d miPtoIzq;
                 getPtoExpropiacion(mLstPlantaExcavacion[i].taludDer.ptoTaludHead, mLstPlantaExcavacion[i].taludIzq.ptoTaludHead, out miPtoDer, out miPtoIzq, mLonExpropiacion.Value);

                 miLstPtoDer.Add(miPtoDer);
                 miLstPtoIzq.Add(miPtoIzq);
             }


             //Ahora debo de Crear la Polilinea de Contorno
             //Invierto el listado Izquierdo para dar los puntos por orden
             miLstPtoIzq.Reverse();

             //Creo la Colleccion de Puntos

             Point3dCollection miLwContorno = new Point3dCollection();

             foreach (var item in miLstPtoDer)
             {
                 miLwContorno.Add(item);
             }

             foreach (var item in miLstPtoIzq)
             {
                 miLwContorno.Add(item);
             }

             //Creo el Contorno de Expropiacion
             string miHandleExpro = oLw.addLw2d(miLwContorno, true, mLayerDominioPublicoAdyacente).Handle.ToString();

             if (iSaveToDb)
             {
                 //Guardo el Dato del handle en la Solucion
                 oDalTbSolucion.addExpropiacionHandle(mSolId, miHandleExpro);
             }
         }



         private void getPtoExpropiacion (Point3d iTaludHeadDer, Point3d iTaludHeadIzq, out Point3d iPtoDerExpro, out Point3d iPtoIzqExpro, double iLonExpro)
         {
         
             //Creo la Linea Interior entre Margenes
             Line miLine = new Line(iTaludHeadDer, iTaludHeadIzq);

             //Creo la Matriz de Transformación
             double miFactorEscala = (miLine.Length + (2 * iLonExpro)) / miLine.Length;
             Matrix3d miMatrixEscala = Matrix3d.Scaling(miFactorEscala, miLine.GetPointAtDist(miLine.Length * 0.5));

             //Aplico la Escala
             miLine.TransformBy(miMatrixEscala);

             iPtoDerExpro = miLine.StartPoint;
             iPtoIzqExpro = miLine.EndPoint;    
         }

        
        /// <summary>
         /// Relleno la Coleccion  mLstPlantaTaludLines.Add(miLine); &&  mLstPlantaHatch
        /// </summary>
        /// <param name="iTaludCurrent"></param>
        /// <param name="iTaludNext"></param>
         private void getPlantaExcavacionLinesHatchFactorySeccionCalzada(oTramoExcavacionTalud iTaludCurrent,oTramoExcavacionTalud iTaludNext)
         {

             Point3dCollection miColHatch;
             Point3d miPto;
             Line miLine;






             //CASO 1
             if (iTaludCurrent.excavacion == eExcavacion.acota && iTaludNext.excavacion == eExcavacion.acota)
             {
                 return;
             }

             //CASO 2 // DESMONTE - TERRAPLEN
             else if (iTaludCurrent.excavacion == eExcavacion.desmonte && iTaludNext.excavacion == eExcavacion.terraplen)
             {

                 miPto = getPointCambioDesmonteTerraplen(iTaludCurrent, iTaludNext);

                 //Creo la Linea 1
                 miLine = new Line(iTaludCurrent.ptoTaludHead, miPto);
                 miLine.Layer = mLayerPlanta;
                 miLine.Color = colorDesmonte;
                 mLstPlantaTaludLines.Add(miLine);

                 //Creo el listado de puntos del Hatch
                 miColHatch = new Point3dCollection();
                 miColHatch.Add(iTaludCurrent.ptoTaludBase);
                 miColHatch.Add(iTaludCurrent.ptoTaludHead);
                 miColHatch.Add(miPto);
                 mLstPlantaHatch.Add(new oExcavacionHatch(eExcavacion.desmonte, miColHatch));



                 //Creo la Segunda Linea
                 miLine = new Line(miPto, iTaludNext.ptoTaludHead);
                 miLine.Layer = mLayerPlanta;
                 miLine.Color = colorTerraplen;
                 mLstPlantaTaludLines.Add(miLine);

                 //Creo el listado de puntos del Hatch
                 miColHatch = new Point3dCollection();
                 miColHatch.Add(miPto);
                 miColHatch.Add(iTaludNext.ptoTaludBase);
                 miColHatch.Add(iTaludNext.ptoTaludHead);
                 mLstPlantaHatch.Add(new oExcavacionHatch(eExcavacion.terraplen, miColHatch));


             }

             //CASO 2 // TERRAPLEN - DESMONTE
             else if (iTaludCurrent.excavacion == eExcavacion.terraplen && iTaludNext.excavacion == eExcavacion.desmonte)
             {

                 miPto = getPointCambioDesmonteTerraplen(iTaludCurrent, iTaludNext);

                 //Creo la Linea 1
                 miLine = new Line(iTaludCurrent.ptoTaludHead, miPto);
                 miLine.Layer = mLayerPlanta;
                 miLine.Color = colorTerraplen;
                 mLstPlantaTaludLines.Add(miLine);

                 //Creo el listado de puntos del Hatch
                 miColHatch = new Point3dCollection();
                 miColHatch.Add(iTaludCurrent.ptoTaludBase);
                 miColHatch.Add(iTaludCurrent.ptoTaludHead);
                 miColHatch.Add(miPto);
                 mLstPlantaHatch.Add(new oExcavacionHatch(eExcavacion.terraplen, miColHatch));





                 //Creo la Segunda Linea
                 miLine = new Line(miPto, iTaludNext.ptoTaludHead);
                 miLine.Layer = mLayerPlanta;
                 miLine.Color = colorDesmonte;
                 mLstPlantaTaludLines.Add(miLine);

                 //Creo el listado de puntos del Hatch
                 miColHatch = new Point3dCollection();
                 miColHatch.Add(miPto);
                 miColHatch.Add(iTaludNext.ptoTaludBase);
                 miColHatch.Add(iTaludNext.ptoTaludHead);
                 mLstPlantaHatch.Add(new oExcavacionHatch(eExcavacion.desmonte, miColHatch));




             }
             //CASO 3 DESMONTE-DESMONTE  o TERRAPLEN-TERRAPLEN o DESMONTE-COTA etc....
             else
             {
                 miLine = new Line(iTaludCurrent.ptoTaludHead, iTaludNext.ptoTaludHead);

                 eExcavacion miExcavacion = getExcavacion(iTaludCurrent.excavacion, iTaludNext.excavacion);

                 Color miColor = getColor(miExcavacion);

                 miLine.Layer = mLayerPlanta;

                 miLine.Color = miColor;

                 mLstPlantaTaludLines.Add(miLine);


                 //Creo el listado de puntos del Hatch
                 miColHatch = new Point3dCollection();

                 miColHatch.Add(iTaludCurrent.ptoTaludBase);
                 miColHatch.Add(iTaludCurrent.ptoTaludHead);
                 miColHatch.Add(iTaludNext.ptoTaludHead);
                 miColHatch.Add(iTaludNext.ptoTaludBase);

                 mLstPlantaHatch.Add(new oExcavacionHatch(miExcavacion, miColHatch));

             }



         }


        private void getPlantaExcavacionLinesHatchFactorySeccionMixto (eRoadSeccion iSeccionCurrent,  oTramoExcavacionTalud iTaludCurrent, eRoadSeccion iSeccionNext,  oTramoExcavacionTalud iTaludNext)
        {




            if (iSeccionCurrent == eRoadSeccion.tunel && iSeccionNext == eRoadSeccion.tunel)
            {
                return;
            }
            else if (iSeccionCurrent == eRoadSeccion.puente && iSeccionNext == eRoadSeccion.puente)
            {
                return;
            }
            else if (iSeccionCurrent == eRoadSeccion.puente && iSeccionNext == eRoadSeccion.tunel)
            {
                return;
            }
            else if (iSeccionCurrent == eRoadSeccion.tunel && iSeccionNext == eRoadSeccion.puente)
            {
                return;
            }
            else
            {

                Point3dCollection miColHatch;
                Line miLine;

                Point3d miPto = oPoint3dExtension.getPtoMedioTwoPoint(iTaludCurrent.ptoTaludBase, iTaludNext.ptoTaludBase);
                eExcavacion miExcavacion = getExcavacion(iTaludCurrent.excavacion, iTaludNext.excavacion);
                Color miColor = getColor(miExcavacion);



                if (iSeccionCurrent == eRoadSeccion.tunel | iSeccionCurrent == eRoadSeccion.puente & iSeccionNext == eRoadSeccion.calzada)
                {
                    //Creo la Segunda Linea
                    miLine = new Line(miPto, iTaludNext.ptoTaludHead);
                    miLine.Layer = mLayerPlanta;
                    miLine.Color = miColor;
                    mLstPlantaTaludLines.Add(miLine);

                    //Creo el listado de puntos del Hatch
                    miColHatch = new Point3dCollection();
                    miColHatch.Add(miPto);
                    miColHatch.Add(iTaludNext.ptoTaludBase);
                    miColHatch.Add(iTaludNext.ptoTaludHead);
                    mLstPlantaHatch.Add(new oExcavacionHatch(miExcavacion, miColHatch));

                }
                else if (iSeccionCurrent == eRoadSeccion.calzada)
                {

                    //Creo la Linea 1
                    miLine = new Line(iTaludCurrent.ptoTaludHead, miPto);
                    miLine.Layer = mLayerPlanta;
                    miLine.Color = miColor;
                    mLstPlantaTaludLines.Add(miLine);

                    //Creo el listado de puntos del Hatch
                    miColHatch = new Point3dCollection();
                    miColHatch.Add(iTaludCurrent.ptoTaludBase);
                    miColHatch.Add(iTaludCurrent.ptoTaludHead);
                    miColHatch.Add(miPto);
                    mLstPlantaHatch.Add(new oExcavacionHatch(miExcavacion, miColHatch));

                }
                else
                {
                    throw new Exception("Caso No Especificado al General los Talud Sección Mixta" + iSeccionCurrent.ToString() + "-" + iSeccionNext.ToString());
                }

            }



        }
   




         /// <summary>
         /// Obtener el Punto Intermedio, En Caso de Pasar de Desmonte a Terraplen o Viceversa
         /// </summary>
         private Point3d getPointCambioDesmonteTerraplen(oTramoExcavacionTalud iTaludCurrrent, oTramoExcavacionTalud iTaludNext)
         {
             double miV1Lon = iTaludCurrrent.taludLon;
             double miV2Lon = iTaludNext.taludLon;


             //Distancia entre Puntos Talud
             double miH1Lon = iTaludCurrrent.ptoTaludBase.DistanceTo(iTaludNext.ptoTaludBase);

             //Distancia desde el origen al punto de Cota
             double miLonOrigen = (miH1Lon * miV1Lon) / (miV1Lon + miV2Lon);


             Line miLineH1 = new Line(iTaludCurrrent.ptoTaludBase, iTaludNext.ptoTaludBase);


             return miLineH1.GetPointAtDist(miLonOrigen);

         }
         private eExcavacion getExcavacion(eExcavacion iTaludCurrent, eExcavacion iTaludNext)
         {


             if (iTaludCurrent == eExcavacion.desmonte | iTaludNext == eExcavacion.desmonte)
             {
                 return eExcavacion.desmonte;
             }

             else if (iTaludCurrent == eExcavacion.terraplen | iTaludNext == eExcavacion.terraplen)
             {
                 return eExcavacion.terraplen;
             }
             else
             {
                 return eExcavacion.acota;
             }

         }
         #endregion

         public void Dispose()
         {
             mLstPlantaLineasMuestreo.Clear();
             mLstPlantaTaludLines.Clear();
             mLstPlantaHatch.Clear();
             mLstPlantaExcavacion.Clear();
         }
    }


    internal class oExcavacionHatch
    { 
    
        Point3dCollection mLstPto= new Point3dCollection();
        eExcavacion mExcavacion;


        public oExcavacionHatch(eExcavacion iExcavacion, Point3dCollection iPtoCol)
        {
            mExcavacion = iExcavacion;
            mLstPto = iPtoCol;
        }


        public void drawHatch(string iLayer)
        { 
            oHatch.addHatchSolid(mLstPto,50,oObraLinealPlantaDrawable.getColor(mExcavacion),iLayer);
        }
    
    
    }
}
