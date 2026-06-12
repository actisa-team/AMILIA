using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using engNet.Csv;
using tadLayLogica.logica.medicion;

namespace tadLayLogica.Comandos
{
    //NET
    using System.Diagnostics;

    //CAD
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Geometry;
    //using Autodesk.Aec.Geometry;
    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Colors;


    using engCadNet;
    using tadLayLan;
    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.logica.Secciones;
    using tadLayLogica.ObraLineal;
    using tadLayLogica.Secciones.Calzada;
    using EjeDeTrazado.puntosDelEje;
    using tadLayLogica.estudioTipo;
    using tadLayLogica.EjeTrazadoTadil;
    using tadLayLan.Tdi;
    using tadLayShare.puntos;
    using tadLayData;
    using System.IO;
    using PerfilLongitudinal;

    public class oComandoObraLineal : IDisposable
    {


        oObraLineal mObraLineal = null;

        public oComandoObraLineal()
        {

        }


        public void create (Guid iIdSolucion)
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                try{
                //Selecciono Punto Interseccion
                Point3d miPtoIn = (Point3d)engCadNet.oTools.getPointCad(strUserCad.uiSelectPtoInsertSecciones);

                    //CONTROL CAPAS
                    //Activo las Capas Zona Geotecnia
                    oTadil.data.Layer.zonasGeotecniaOn();
                    oTadil.data.Layer.zonasExpropiacionOn();

                    //Creo el Objeto Solucion
                    oSolucion miSolucion = new oSolucion(iIdSolucion);

                    //Creo el Eje Visibilidad
                    Stopwatch miMedicion = new Stopwatch();
                    miMedicion.Start();


                    //Creo el Objeto oEstudioInformativoCarretera
                    oEstudioInformativoCarretera miEstudioCarretera = (oEstudioInformativoCarretera)oFactoryEstudios.getEstudioCarretera(eEstudioTipo.ESTINF, iIdSolucion);


                    //Creo el Objeto Seccion Completa GIS
                    oSeccionRoadCompletaSinGis miSeccionCompletaSinGis = oSingletonProyecto.getInstance.seccionCalzadaCompleta;


                    //Creo el Objeto EjeTrazadoTadil
                    EjeTrazado miEjeTrazadoTadil = oFactoryEjeTrazadoTadil.getEjeTrazadoTadil(iIdSolucion, miSolucion.roadDesign.peralte, miSeccionCompletaSinGis.BombeoPC.Value);

                    /*
                     * Se comenta ya que se va a utilizar KD-Tree
                     */
                    //var puntos_rango_eje=PuntosCercanosAEje(oSingletonPuntosTerreno.getInstance.Get_puntos(),miEjeTrazadoTadil.getVertices,500);
                    //oSingletonPuntosTerreno.getInstance.Nuevo_MDT(oSingletonPuntosTerreno.getInstance.Get_puntos());

                    mObraLineal = new oObraLineal(miSolucion.idSolucion,
                                                  miSolucion.solucionData.nombre,
                                                  miSolucion.ejeTrazado,
                                                  miSolucion.ejePerfilRasante,
                        //miEjeTrazadoTadil,
                                                  miEstudioCarretera,
                                                  oSingletonProyecto.getInstance.seccionCalzadaCompleta);

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiCreandoSecciones);
                    mObraLineal.crearSecciones(oSingletonProyecto.getInstance.seccionIntervalo);

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoSecciones);
                    mObraLineal.dibujarSecciones(miPtoIn);

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoPlanta);
                    mObraLineal.createObraLinealPlanta();

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoMediciones);
                    mObraLineal.createMediciones();

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGuardandoSecciones);
                    mObraLineal.saveSecciones();

                    oSingletonDsApp.getInstance.getSolucion(iIdSolucion).isCompleteObraLineal = true;

                    oSingletonDsApp.getInstance.solucionSave();

                    //Info UI
                    miMedicion.Stop();
                    oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);
                }
                catch(oExSelectPointNull)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }
            }            
        }
        public void create_Amilia(Guid iIdSolucion)
        {

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                try
                {
                    //Selecciono Punto Interseccion
                    Point3d miPtoIn = (Point3d)engCadNet.oTools.getPointCad(strUserCad.uiSelectPtoInsertSecciones);

                    //CONTROL CAPAS
                    //Activo las Capas Zona Geotecnia
                    oTadil.data.Layer.zonasGeotecniaOn();
                    oTadil.data.Layer.zonasExpropiacionOn();

                    //Creo el Objeto Solucion
                    oSolucion miSolucion = new oSolucion(iIdSolucion);

                    //Creo el Eje Visibilidad
                    Stopwatch miMedicion = new Stopwatch();
                    miMedicion.Start();


                    //Creo el Objeto oEstudioInformativoCarretera
                    oEstudioInformativoCarretera miEstudioCarretera = (oEstudioInformativoCarretera)oFactoryEstudios.getEstudioCarretera(eEstudioTipo.ESTINF, iIdSolucion);


                    //Creo el Objeto Seccion Completa GIS
                    oSeccionRoadCompletaSinGis miSeccionCompletaSinGis = oSingletonProyecto.getInstance.seccionCalzadaCompleta;


                    //Creo el Objeto EjeTrazadoTadil
                   // EjeTrazado miEjeTrazadoTadil = oFactoryEjeTrazadoTadil.getEjeTrazadoTadil(iIdSolucion, miSolucion.roadDesign.peralte, miSeccionCompletaSinGis.BombeoPC.Value);

                    EjeTrazado eje_trazado = null;
                    Alzado miAlzado = null;
                    using (oSolucion miSolucion2 = new oSolucion(iIdSolucion))
                    {
                        EjeTrazado miEjeTrazado = null;
                        dsApp.tbSolucionRow miRow = oDalTbSolucion.getSolucion(iIdSolucion);

                        if (!miRow.IsEjeTrazado_AmiliaNull())
                        {
                            // 1. Obtenemos el array de bytes de la columna del DataSet
                            byte[] datosRecuperados = miRow.EjeTrazado_Amilia;

                            // 2. Creamos un MemoryStream con esos bytes
                            using (MemoryStream ms = new MemoryStream(datosRecuperados))
                            {
                                // 3. Usamos tu método estático para reconstruir la clase
                                eje_trazado = EjeTrazado.recuperaEjeTrazado(ms);
                            }

                            byte[] datosRecuperados_alzado = miRow.Alzado_Amilia;
                            using (MemoryStream ms = new MemoryStream(datosRecuperados_alzado))
                            {
                                // 3. Usamos tu método estático para reconstruir la clase
                                miAlzado = Alzado.recuperaAlzado(ms);
                            }
                        }
                    }

                            /*
                             * Se comenta ya que se va a utilizar KD-Tree
                             */
                            //var puntos_rango_eje=PuntosCercanosAEje(oSingletonPuntosTerreno.getInstance.Get_puntos(),miEjeTrazadoTadil.getVertices,500);
                            //oSingletonPuntosTerreno.getInstance.Nuevo_MDT(oSingletonPuntosTerreno.getInstance.Get_puntos());

                           /* mObraLineal = new oObraLineal(miSolucion.idSolucion,
                                                  miSolucion.solucionData.nombre,
                                                  miSolucion.ejeTrazado,
                                                  miSolucion.ejePerfilRasante,
                                                  //miEjeTrazadoTadil,
                                                  miEstudioCarretera,
                                                  oSingletonProyecto.getInstance.seccionCalzadaCompleta);*/

                    mObraLineal = new oObraLineal(miSolucion.idSolucion,
                                                  miSolucion.solucionData.nombre,
                                                     eje_trazado,
                                                     miAlzado,
                                                     miEstudioCarretera,
                                                  oSingletonProyecto.getInstance.seccionCalzadaCompleta);



                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiCreandoSecciones);
                    mObraLineal.crearSecciones(oSingletonProyecto.getInstance.seccionIntervalo);

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoSecciones);
                    mObraLineal.dibujarSecciones(miPtoIn);

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoPlanta);
                    mObraLineal.createObraLinealPlanta();

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGenerandoMediciones);
                    mObraLineal.createMediciones();

                    oCadManager.thisEditor.WriteMessage(strFrmSolucion.uiGuardandoSecciones);
                    mObraLineal.saveSecciones();

                    oSingletonDsApp.getInstance.getSolucion(iIdSolucion).isCompleteObraLineal = true;

                    oSingletonDsApp.getInstance.solucionSave();
                
                    //Info UI
                    miMedicion.Stop();
                    oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);
                }
                catch (oExSelectPointNull)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }
            }
        }


        public bool displaySeccionesConError()
        {
            if (mObraLineal == null)
            {
                return false;
            }
            else if (oTadil.data.displaySeccionesConError() && mObraLineal.hasSeccionesConError)
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        public List<engNet.Str.oStringPropiedad> getListadoPkConErrores ()
        {
            return mObraLineal.getListadoPkConError();
        }

        public Hashtable getMedicionesSecciones(Guid iIdSolucion, double pk)
        {
            var result = new Hashtable();
                var miQuery = from p in oSingletonDsApp.getInstance.dataset.tbMedicionesBySeccion
                          where p.idTbSolucion == iIdSolucion && p.pk == pk
                          select p;

            foreach (var item in miQuery)
            {
                var descripcion = item.grupo + "_" + item.material;
                if(!result.ContainsKey(descripcion))
                    result.Add(descripcion, item.valor);
                else
                {
                    result[descripcion] = (double)result[descripcion] + item.valor;
                }
            }

            return result;
        }

        public void createCSVListMedicionesSecciones(Guid iIdSolucion)
        {
            var columnas = new List<string>();
            var filas = new List<List<double?>>();
            var datos = new List<Hashtable>();
            var listSeccionesPk = oDalTbObraLinealSecciones.getListPksSecciones(iIdSolucion);

            foreach (var pk in listSeccionesPk)
            {
                var results = getMedicionesSecciones(iIdSolucion, pk);
                datos.Add(results);
                foreach (var key in results.Keys)
                {
                    if(!columnas.Contains(key))
                        columnas.Add((string)key);
                }
                
            }


            for (int i = 0; i < listSeccionesPk.Count; i++)
            {
                var pk = listSeccionesPk[i];
                var datosPk = datos[i];
                var fila = new List<double?>();
                foreach (var columna in columnas)
                {
                    if (datosPk.ContainsKey(columna))
                    {
                        fila.Add((double)datosPk[columna]);
                    }
                    else
                    {
                        fila.Add(null);
                    }
                }
                fila.Insert(0, pk);
                filas.Add(fila);
            }


            columnas.Insert(0, "Pk");


            string miFileCsv = oTadil.data.Files.saveAsFileCsvFromDialog(strGeneralUser.uiInformeMediciones);
            if(!string.IsNullOrEmpty(miFileCsv))
                 oCsv.write(columnas, filas, miFileCsv);
        }


        public void Dispose()
        {
            mObraLineal.Dispose();
        }


        static List<Punto3d> PuntosCercanosAEje(List<Punto3d> puntos, List<Vertice> eje, double rango)
        {
            List<Punto3d> puntosCercanos = new List<Punto3d>();

            foreach (var punto in puntos)
            {
                double x0 = punto.coordenadaX, y0 = punto.coordenadaY;
                double distanciaMinima = double.MaxValue;

                // Recorrer cada segmento del eje
                for (int i = 0; i < eje.Count - 1; i++)
                {
                    Punto3d p = new Punto3d(eje[i].getPuntoX, eje[i].getPuntoY, eje[i].getPuntoZ);
                    Punto3d p2 = new Punto3d(eje[i+1].getPuntoX, eje[i+1].getPuntoY, eje[i+1].getPuntoZ);

                    // Calcular la distancia del punto (x0, y0) al segmento (x1, y1)-(x2, y2)
                    double distancia = DistanciaPuntoSegmento(x0, y0, p.coordenadaX, p.coordenadaY, p2.coordenadaX, p2.coordenadaY);

                    // Mantener la menor distancia encontrada
                    if (distancia < distanciaMinima)
                        distanciaMinima = distancia;
                }

                // Si la menor distancia encontrada está dentro del rango, guardar el punto
                if (distanciaMinima <= rango)
                {
                    puntosCercanos.Add(punto);
                }
            }

            return puntosCercanos;
        }

        static double DistanciaPuntoSegmento(double x0, double y0, double x1, double y1, double x2, double y2)
        {
            double A = x0 - x1;
            double B = y0 - y1;
            double C = x2 - x1;
            double D = y2 - y1;

            double dot = A * C + B * D;
            double len_sq = C * C + D * D;
            double param = (len_sq != 0) ? dot / len_sq : -1;

            double xx, yy;

            if (param < 0) // Más cerca del primer punto del segmento
            {
                xx = x1;
                yy = y1;
            }
            else if (param > 1) // Más cerca del segundo punto del segmento
            {
                xx = x2;
                yy = y2;
            }
            else // Proyección cae dentro del segmento
            {
                xx = x1 + param * C;
                yy = y1 + param * D;
            }

            // Distancia euclidiana del punto al segmento
            double dx = x0 - xx;
            double dy = y0 - yy;
            return Math.Sqrt(dx * dx + dy * dy);
        }

    }
}
