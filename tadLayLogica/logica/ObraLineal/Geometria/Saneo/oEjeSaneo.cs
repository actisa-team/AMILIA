using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria.Saneo
{

    using engCadNet;
    using engCadNet.entidades;

    using tadLayLogica.EjeTJ;
    using tadLayLogica.EjeTJ.Tramos;
    using tadLayLogica.EjeTJ.Vertice;
    using tadLayLogica.EjeTJ.Secciones;
    using engCadNet.extension;

    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    using tadLayShare.puntos;
    
    /// <summary>
    /// JUAN - UPDATE 31-10-2014
    /// </summary>
    public class oEjeSaneo : oEjeObj<oVerticeExplanada,oTramoExplanada>
    {

        

        Polyline mLwExplanda;
        Polyline mLwTndOriginal;
        Polyline mLwTndSeccion;
        double mSeccIntervalo = 0.25;
        Dictionary<int, oVerticeExplanada> mDicSeccion= null;
     

  

        #region "Constructor"

        public oEjeSaneo()
        {

        }

        public oEjeSaneo(string iNombre, Polyline iLwExplanada, Polyline iLwTndOriginal, Polyline iLwTndSeccion)
        {

            mLwExplanda = iLwExplanada;
            mLwTndOriginal = iLwTndOriginal;
            mLwTndSeccion = iLwTndSeccion;
           


            if (!mLwExplanda.isLeftToRight())
            {
                using (oLwPlus miLw = new oLwPlus(mLwExplanda))
                {
                    miLw.lw.ReverseCurve();
                }
            }


            if (!mLwTndOriginal.isLeftToRight())
            {
                using (oLwPlus miLw = new oLwPlus(mLwTndOriginal))
                {
                    miLw.lw.ReverseCurve();
                }

            }


            if (!mLwTndSeccion.isLeftToRight())
            {
                using (oLwPlus miLw = new oLwPlus(mLwTndSeccion))
                {
                    miLw.lw.ReverseCurve();
                }

            }


            Dictionary<int, oVerticeExplanada> miDicVertices = new Dictionary<int, oVerticeExplanada>();
            oVerticeExplanada miVerticeExplanada;
            int i = 0;
            double miLonOrigen = 0;

            for (i = 0; i < mLwExplanda.NumberOfVertices - 1; i++)
            {

                miVerticeExplanada = new oVerticeExplanada(i, mLwExplanda.GetLineSegment2dAt(i).StartPoint.X, mLwExplanda.GetLineSegment2dAt(i).StartPoint.Y, miLonOrigen);
                miVerticeExplanada.seccion = getSeccion(miVerticeExplanada.position);
                miDicVertices.Add(i, miVerticeExplanada);
                miLonOrigen = miLonOrigen + mLwExplanda.GetLineSegment2dAt(i).Length;
            }


            //Añado el Punto Final
            miVerticeExplanada = new oVerticeExplanada(mLwExplanda.NumberOfVertices - 1, mLwExplanda.EndPoint.X, mLwExplanda.EndPoint.Y, mLwExplanda.Length);
            miVerticeExplanada.seccion = getSeccion(miVerticeExplanada.position);
            miDicVertices.Add(miVerticeExplanada.id, miVerticeExplanada);

            base.EjeName = iNombre;
            base.vertices = miDicVertices;
            
        }

        #endregion
        #region "Propiedades"


        /// <summary>
        /// Listado con todos los Puntos de la Seccion
        /// </summary>
        public Dictionary<int, oVerticeExplanada> ejeSeccion
        {
            get
            {
                if (mDicSeccion == null)
                {
                    mDicSeccion = getSeccionCompleta();
                }

                return mDicSeccion;    
            }
        }








        #endregion
        #region "Metodo Publico"

 

        /// <summary>
        /// Obtengo una lista con las Polilineas Origen del Saneo
        /// </summary>
        /// <returns></returns>
        public List<oLwSaneo> getSaneoExplanda(bool iCreateSaneoDesmonte, bool iCreateSaneoTerraplen)
        {

            List<oLwSaneo> miLstLwSaneo = new List<oLwSaneo>();


            //Determino si Existe Interseccion entre Terreno y Explanada
            Point3dCollection miColInter = new Point3dCollection();

            //ORIGINAL
           mLwExplanda.IntersectWith(mLwTndSeccion, Intersect.OnBothOperands, miColInter, IntPtr.Zero, IntPtr.Zero);

           
            //Ver Ptos Coleccion
           // miColInter.viewPto(0.2, "0");


            //No Existe InterSección
            if (miColInter.Count == 0)
            {

                int miVerticesTerraplen = (from p in vertices where p.Value.seccion.terrenoCorrecion == eExcavacion.terraplen select p).Count();
                int miVerticesDesmonte = (from p in vertices where p.Value.seccion.terrenoCorrecion == eExcavacion.desmonte select p).Count();

                if (miVerticesDesmonte > miVerticesTerraplen)
                {
                    miLstLwSaneo.Add(new oLwSaneo(mLwExplanda, eSaneo.desmonte));
                }

                else
                {
                    miLstLwSaneo.Add(new oLwSaneo(mLwTndSeccion, eSaneo.terraplen));

                }
            

            }
            else
            {
                //Consulto los Puntos de Intersección
                var myQuery = from p in ejeSeccion.Values
                              where p.seccion.terrenoCorrecion == eExcavacion.acota
                              orderby p.distanciaOrigen ascending
                              select p;


                List<oVerticeExplanada> miLstPtoCotaSinFiltro = myQuery.ToList();

                List<oVerticeExplanada> miLstPtoCotaFiltrada = this.filtrarVerticesACota(miLstPtoCotaSinFiltro, 0.5);


                foreach (oVerticeExplanada fPtoCota in miLstPtoCotaFiltrada)
                {
                    miLstLwSaneo.AddRange(getLwSaneo(fPtoCota.id));
                }


            }


            //Ahora filtro solo los saneos que necesito
            //Puedo crear solo a Desmonte o Terraplen

            if (iCreateSaneoDesmonte && iCreateSaneoTerraplen)
            {
                return miLstLwSaneo;
            }
            else if (!iCreateSaneoDesmonte && !iCreateSaneoTerraplen)
            {
                return new List<oLwSaneo>();
            }
            else if (iCreateSaneoDesmonte)
            {
                var myQuery = from p in miLstLwSaneo
                              where p.saneo == eSaneo.desmonte
                              select p;

                return myQuery.ToList();
               
            
            }
            else if (iCreateSaneoTerraplen)
            {

                var myQuery = from p in miLstLwSaneo
                              where p.saneo == eSaneo.terraplen
                              select p;

                return myQuery.ToList();
            }
            else
            {

                throw new Exception("Caso No Configurado (SaneoExplanda)");
            }

           

        }

        #endregion
        #region "Metodos Privados"

        /// <summary>
        /// Obtener la Sección de la Explanda
        /// </summary>
        /// <param name="iIntervalo"></param>
        private Dictionary<int, oVerticeExplanada> getSeccionCompleta()
        {

            List<oVerticeExplanada> miLstVerticeExplanada = new List<oVerticeExplanada>();



            //Obtengo la Sección Intermedia de los Tramos
            foreach (oTramoExplanada ftramo in segmentos.Values)
            {
                ftramo.getSeccíon(mSeccIntervalo, getSeccion);
            }


            //Obtengo los Puntos de Interseccion Terreno
            Point3dCollection miColInter = new Point3dCollection();

            mLwExplanda.IntersectWith(mLwTndOriginal, Intersect.OnBothOperands, miColInter, IntPtr.Zero, IntPtr.Zero);


            if (miColInter.Count != 0)
            {
                int miSegmento;

                foreach (Point3d fPto in miColInter)
                {
                    miSegmento = oLw.getTramoByPtoBaseCero(mLwExplanda, fPto);
                    segmentos[miSegmento].addVerticeInterseccion(fPto);
                }

            }



            //Creo la Sección Completa de la Sección
            foreach (oTramoExplanada ftramo in segmentos.Values)
            {
                miLstVerticeExplanada.AddRange(ftramo.lstVerticesSeccion);
            }


            //Ordeno la Sección por Distancia a Origen y Configuro los IDS



            //Reiniciamos los Valores del Id
            var myQuery = from p in miLstVerticeExplanada
                          orderby p.distanciaOrigen ascending
                          select p;


            List<oVerticeExplanada> miLstVerticeExplanadaOrdenada = new List<oVerticeExplanada>();

            miLstVerticeExplanadaOrdenada = myQuery.ToList();



            Dictionary<int, oVerticeExplanada> miDicVerticeExplanada = new Dictionary<int, oVerticeExplanada>();


            int i = 0;



            foreach (oVerticeExplanada fvertice in miLstVerticeExplanadaOrdenada)
            {
                fvertice.id = i;
                miDicVerticeExplanada.Add(i, fvertice);
                i++;
            }


            //Ver Secciones
            //foreach (oVerticeExplanada fvertice in miDicVerticeExplanada.Values)
            //{
            //    fvertice.draw("0");
            //}

            return miDicVerticeExplanada;

        }


        /// <summary>
        /// Dato un ID del Punto 'acota' de la Explanda
        /// Obtenemos la polilinea Base que me sirve para generar el Saneo
        /// -Desmonte --> Tomamos Polilinea Explanda
        /// -Terraplen --> Tomamos Polilinea Terreno Natural
        private List<oLwSaneo> getLwSaneo(int iPtoId)
        {

            //Consulto los Puntos Anteriores a Cota
            var myQuery = from p in ejeSeccion.Values
                          where p.seccion.terrenoCorrecion == eExcavacion.acota & p.id < iPtoId
                          orderby p.distanciaOrigen descending
                          select p;


            List<oLwSaneo> miLstSaneo = new List<oLwSaneo>();

            oVerticeExplanada miVerticeExplanda;
            Polyline miLw;
            Point3d miP1 = Point3d.Origin;
            Point3d miP2 = Point3d.Origin;


            List<oVerticeExplanada> miLstPtoCota = myQuery.ToList();


            //Caso es el Primero Punto a Cota, NoExiste Anterior
            if (miLstPtoCota.Count == 0)
            {
                if (ejeSeccion[iPtoId - 1].seccion.terrenoCorrecion == eExcavacion.desmonte)
                {

                    miP1 = new Point3d(ejeSeccion[0].position.toArray3d());
                    miP2 = new Point3d(ejeSeccion[iPtoId].position.toArray3d());
                    miLw = oLw.splitLwTwoPoints(mLwExplanda, miP1, miP2, "0");

                    miLstSaneo.Add(new oLwSaneo(miLw, eSaneo.desmonte));
                }
                else if (ejeSeccion[iPtoId - 1].seccion.terrenoCorrecion == eExcavacion.terraplen)
                {

                    miP1 = mLwTndSeccion.GetPoint3dAt(0);
                    miP2 = new Point3d(ejeSeccion[iPtoId].position.toArray3d());
                    miLw = oLw.splitLwTwoPoints(mLwTndSeccion, miP1, miP2, "0");

                    miLstSaneo.Add(new oLwSaneo(miLw, eSaneo.terraplen));
                }
                else
                {
                    throw new Exception("Error GetPrevioPtoCota");

                }

            }

            //Tengo el Punto a Cota y el Previo
            else
            {

                miVerticeExplanda = myQuery.First();


                if (ejeSeccion[iPtoId - 1].seccion.terrenoCorrecion == eExcavacion.desmonte)
                {

                    miP1 = new Point3d(miVerticeExplanda.position.toArray3d());
                    miP2 = new Point3d(ejeSeccion[iPtoId].position.toArray3d());
                    miLw = oLw.splitLwTwoPoints(mLwExplanda, miP1, miP2, "0");

                    miLstSaneo.Add(new oLwSaneo(miLw, eSaneo.desmonte));
                }
                else if (ejeSeccion[iPtoId - 1].seccion.terrenoCorrecion == eExcavacion.terraplen)
                {

                    miP1 = new Point3d(miVerticeExplanda.position.toArray3d());
                    miP2 = new Point3d(ejeSeccion[iPtoId].position.toArray3d());
                    miLw = oLw.splitLwTwoPoints(mLwTndSeccion, miP1, miP2, "0");

                    miLstSaneo.Add(new oLwSaneo(miLw, eSaneo.terraplen));
                }
                else
                {
                    throw new Exception("Error GetPrevioPtoCota");
                }
            }



            //Consulto si es el último punto a Cota, para realizar el extremo final
            var myQueryEnd = from p in ejeSeccion.Values
                             where p.seccion.terrenoCorrecion == eExcavacion.acota & p.id > iPtoId
                             orderby p.distanciaOrigen descending
                             select p;



            //Es el Final // Debo de Añadir el ultimo SANEO
            if (myQueryEnd.Count() == 0)
            {
                try  //Error al acceder a la iPtoId+1 ,
                {
                    if (ejeSeccion[iPtoId + 1].seccion.terrenoCorrecion == eExcavacion.desmonte)
                    {

                        miP1 = new Point3d(ejeSeccion[iPtoId].position.toArray3d());
                        miP2 = new Point3d(ejeSeccion[ejeSeccion.Count - 1].position.toArray3d());

                        miLw = oLw.splitLwTwoPoints(mLwExplanda, miP1, miP2, "0");

                        miLstSaneo.Add(new oLwSaneo(miLw, eSaneo.desmonte));
                    }
                    else if (ejeSeccion[iPtoId + 1].seccion.terrenoCorrecion == eExcavacion.terraplen)
                    {

                        miP1 = new Point3d(ejeSeccion[iPtoId].position.toArray3d());
                        miP2 = mLwTndSeccion.GetPoint3dAt(mLwTndSeccion.NumberOfVertices - 1);
                        miLw = oLw.splitLwTwoPoints(mLwTndSeccion, miP1, miP2, "0");
                        miLstSaneo.Add(new oLwSaneo(miLw, eSaneo.terraplen));
                    }
                    else
                    {
                        throw new Exception("Error GetPrevioPtoCota");
                    }  
                }
                catch (Exception)
                {
                    
                   
                }                        
            }

            return miLstSaneo;

        }



        /// <summary>
        /// Dato las Coordenadas de un Punto del Eje Obtenmos su Sección
        /// </summary>
        private oSeccionSimple getSeccion(IP3d iPtoExplanada)

        {
            oSeccionSimple miSeccion;
            Point3d miPtoV1 = new Point3d(iPtoExplanada.toArray3d());
            Point3d miPtoV2 = miPtoV1.getFromIncXIncY(0, 1, 0);

            Line miLineV = new Line(miPtoV1,miPtoV2);

            Point3dCollection miCol = new Point3dCollection();

            miLineV.IntersectWith(mLwTndOriginal, Intersect.ExtendThis, miCol, IntPtr.Zero, IntPtr.Zero);


            if (miCol.Count == 0)
            {
                throw new Exception("Error al Obtener la Intersección en el Eje de Saneo");
            }
            else if (miCol.Count == 1)
            {
              miSeccion = new oSeccionSimple(miCol[0].to2d(), miPtoV1.to2d());            
            }
            else if (miCol.Count > 1)
            {
                miSeccion = new oSeccionSimple(miPtoV1.getPtoMasCercano(miCol).to2d(), miPtoV1.to2d());

                Line miLine = new Line(miPtoV1.getPtoMasCercano(miCol), miPtoV1);      
            }
            else
            {
               // engCadNet.oTools.entidadAdd(miLineV, "0");
                throw new Exception("Error al Obtener la Intersección en el Eje de Saneo \n Puntos de Intersección :" + miCol.Count.ToString());
            }

            return miSeccion;

        }



        private List<oVerticeExplanada> filtrarVerticesACota (List<oVerticeExplanada> iLstVerticesAFiltrar,   double iLonHorizontalMinimaSaneo)
        {


            List<oVerticeExplanada> miListaFiltrada = new List<oVerticeExplanada>();

            double miIncrementoX;


            miListaFiltrada.Add(iLstVerticesAFiltrar[0]);

            for (int i = 1; i < iLstVerticesAFiltrar.Count; i++)
            {

                miIncrementoX = iLstVerticesAFiltrar[i].distanciaOrigen - iLstVerticesAFiltrar[i-1].distanciaOrigen;

                if (miIncrementoX > iLonHorizontalMinimaSaneo)
                {
                    miListaFiltrada.Add(iLstVerticesAFiltrar[i]);
                }


            }

            return miListaFiltrada;
        }

        #endregion
    }
}
