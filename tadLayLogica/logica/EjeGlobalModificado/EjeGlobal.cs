using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using engCadNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using tadLayLan;
using tadLayLogica.datos.proyecto;
using tadLayLogica.EjeVisibilidad;
using tadLayLogica.estudioTipo;
using tadLayLogica.logica.EjeBasicoNew;
using tadLayLogica.logica.EjeVisibilidad;
using tadLayLogica.zonaGis;
using tadLayShare.puntos;

namespace tadLayLogica.logica.EjeGlobalModificado
{
    public class EjeGlobal
    {
        Point3d mStart;
        Point3d mGoal;
        Point3d mStart_ext;
        Point3d mGoal_ext;
        double mSeparacion;
        List<oNode> mPathSol = null;
        double? mPendienteMaxCota = null;
        double? mPendienteMaxTrian = null;
        oEjeBasicoSolucion mEjeBasicoSol = null;
        public EjeGlobal(oEjeBasicoSolucion iejeBasicoSol, double Dmin,
            double vertmin, double Lmin, double Lmax, bool dibujar, bool preciso)
        {



            Stopwatch miMedicion = new Stopwatch();
            miMedicion.Start();
            mEjeBasicoSol = iejeBasicoSol;
            bool respetarpuntos = true;
            double R = mEjeBasicoSol.roadDesign.Rp;
            double clmin = Math.Sqrt(12 * Math.Pow(R, 3)) / R;

            //var envolvente =Crear2();

            bool anguloautomatico = true;
            if (vertmin >= 0)
            {
                anguloautomatico = false;
            }
            //oSingletonPuntosTerreno.getInstance.Set_Envolvente(envolvente);
            //oSingletonPuntosTerrenoASC.getInstance.Set_Envolvente(envolvente);
            //Point2d myStart = new Point2d(mStart.X, mStart.Y);
            //Point2d myGoal = new Point2d(mGoal.X, mGoal.Y);

            /* double miLongitud = myStart.GetDistanceTo(myGoal);
             mSeparacion = miLongitud / (int)(miLongitud / separacion);*/
            oNodoGlobal[,] miMatriz = null;
            List<oNode> miNodesD = new List<oNode>();
            oNodeCad miNodoEntrada = null;
            oNodeCad miNodoSalida = null;
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                int miNumFilas, miNumColumnas, miPosFilaCentral;
                double miVelocidad = mEjeBasicoSol.roadDesign.Vp;
                double miPendienteMaxTriang = 0.2;
                oSingletonPuntosTerreno.getInstance.Set_Preciso(preciso);
                oSingletonPuntosTerrenoASC.getInstance.Set_Preciso(preciso);
                miMatriz = CrearPuntos(Lmin);

                DialogResult result = MessageBox.Show("¿Quieres que se respete el punto de salida y de llegada?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    respetarpuntos = true;
                }
                else
                {
                    respetarpuntos = false;
                }


                //oNodoGlobal[,] miMatriz = getLstNode(out miPosFilaCentral);
                Point2d myStart = new Point2d(mStart.X, mStart.Y);
                Point2d myGoal = new Point2d(mGoal.X, mGoal.Y);
                oConexionGlobal.SetUpObject(mEjeBasicoSol.roadDesign, mEjeBasicoSol.roadPendientes, mEjeBasicoSol.abanicoDesign, mEjeBasicoSol.estudioData);

                if (mPendienteMaxCota != null)
                {
                    //filtrar por cota TODO si lo marca el usuario
                    filtarPuntosValidosPorCota(miVelocidad, (double)mPendienteMaxCota, ref miMatriz);
                }

                if (mPendienteMaxTrian != null)
                {
                    //filtrar por pendiente TODO si lo marca el usuario
                    filtarPuntosValidosPorPendiente((double)mPendienteMaxTrian, ref miMatriz);
                }

                for (int i = 0; i < miMatriz.GetLength(0); i++)
                {
                    for (int j = 0; j < miMatriz.GetLength(1); j++)
                    {
                        if (miMatriz[i, j] != null)
                        {
                            double[] miPunto = { miMatriz[i, j].mNodo.X, miMatriz[i, j].mNodo.Y };
                            if (miMatriz[i, j].mIsValido)
                            {
                                /*
                                * Cambios para que no pinte las capas **juanma**
                                */
                                if (dibujar)
                                {
                                    //oMTexto.addMText2D("SI: " + i + ", " + j, miPunto, 10, 0, "0");
                                }
                                // 
                            }
                            else
                            {
                                /*
                                 * Cambios para que no pinte las capas **juanma**
                                 */
                                if (dibujar)
                                {
                                    //oMTexto.addMText2D("NO: " + i + ", " + j, miPunto, 10, 0, "0");
                                }
                                //
                            }
                        }
                    }
                }



                miNodoEntrada = new oNodeCad(0, new Point2d(mStart.X, mStart.Y), 0, 0);
                miNodoSalida = new oNodeCad(1, new Point2d(mGoal.X, mGoal.Y), 0, 0);
                miNodesD.Add(miNodoEntrada);
                miNodesD.Add(miNodoSalida);
                int id = 2;

                for (int i = 0; i < miMatriz.GetLength(0); i++)
                {
                    for (int j = 0; j < miMatriz.GetLength(1); j++)
                    {
                        oNodoGlobal miNodo = miMatriz[i, j];
                        if (miNodo != null)
                        {
                            if ((Math.Round(miNodo.mNodo.X) == Math.Round(mStart.X)) && (Math.Round(miNodo.mNodo.Y) == Math.Round(mStart.Y)))
                            {
                                miNodo.mNodoDijkstra = miNodoEntrada;
                            }
                            else if ((Math.Round(miNodo.mNodo.X) == Math.Round(mGoal.X)) && (Math.Round(miNodo.mNodo.Y) == Math.Round(mGoal.Y)))
                            {
                                miNodo.mNodoDijkstra = miNodoSalida;
                            }
                            else
                            {
                                miNodo.mNodoDijkstra = new oNodeCad(id, new Point2d(miNodo.mNodo.X, miNodo.mNodo.Y), 0, 0);
                                if (miNodo.mIsValido)
                                {
                                    id++;
                                    miNodesD.Add(miNodo.mNodoDijkstra);
                                }
                            }

                        }
                    }

                }
            }


            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                List<oConexionGlobal> misConexionesValidas = new List<oConexionGlobal>();
                List<oConexionGlobal> misConexionesNOValidas = new List<oConexionGlobal>();
                // Convertir a List si es necesario

                for (int i = 0; i < miMatriz.GetLength(0); i++)
                {
                    double porcentaje = (double)i / (double)miMatriz.GetLength(0) * (double)100;
                    engCadNet.oCadManager.thisEditor.WriteMessage("\n Completado " + Math.Round(porcentaje, 2) + "%");
                    System.Windows.Forms.Application.DoEvents();
                    for (int j = 0; j < miMatriz.GetLength(1); j++)
                    {
                        if (i != (int)miMatriz.GetLength(0) / 2 && j == 0)
                        {
                            continue;
                        }
                        if (miMatriz[i, j] != null)
                        {
                            if (miMatriz[i, j].mIsValido)
                            {
                                oArañaBusquedaGlobal miAraña = new oArañaBusquedaGlobal(miMatriz, i, j, Lmin);
                                miAraña.creaConexionesDinamicas(miMatriz, ref misConexionesValidas, ref misConexionesNOValidas,
                                    mGoal, Lmin, Lmax, dibujar);

                                //miAraña.creaConexiones(miMatriz, ref misConexionesValidas, ref misConexionesNOValidas, mGoal, dibujar);
                            }
                        }
                    }
                }
                engCadNet.oCadManager.thisEditor.WriteMessage("\n Completado " + 100 + "%");
                System.Windows.Forms.Application.DoEvents();
                List<oEdge> miEdgesD = createEdges(misConexionesValidas, mEjeBasicoSol);
                foreach (oConexionGlobal miConexion in misConexionesValidas)
                {
                    /*
                    * Cambios para que no pinte las capas **juanma**
                    */
                    if (dibujar)
                    {
                        engCadNet.oMTexto.addMText2D(miConexion.ToString(), miConexion.mTramo.P2.toArray3d(), 0.15, 0, oTadil.data.Layer.visibilidadGrafo.name);
                    }
                    //
                }

                try
                {


                    if (miEdgesD.Count > 2)
                    {

                        Dijkstra miDijkstra = new Dijkstra(miEdgesD, miNodesD, Dmin, vertmin);


                        //Indico el Nodo de Inicio
                        miDijkstra.calculateDistance(miNodoEntrada, respetarpuntos, clmin, R, anguloautomatico);

                        //Indico el Nodo de Llegada
                        mPathSol = miDijkstra.getPathTo(miNodoSalida);
                    }
                    else if (miEdgesD.Count == 2)
                    {
                        mPathSol = new List<oNode>();
                        if ((miEdgesD[0].Destination.pNodePos.X == mGoal.X) && (miEdgesD[0].Destination.pNodePos.Y == mGoal.Y))
                        {
                            if ((miEdgesD[0].Origin.pNodePos.X == mStart.X) && (miEdgesD[0].Origin.pNodePos.Y == mStart.Y))
                            {
                                mPathSol.Add(miEdgesD[0].Origin);
                                mPathSol.Add(miEdgesD[0].Destination);
                            }
                        }
                        if ((miEdgesD[1].Destination.pNodePos.X == mGoal.X) && (miEdgesD[1].Destination.pNodePos.Y == mGoal.Y))
                        {
                            if ((miEdgesD[1].Origin.pNodePos.X == mStart.X) && (miEdgesD[1].Origin.pNodePos.Y == mStart.Y))
                            {
                                mPathSol.Add(miEdgesD[1].Origin);
                                mPathSol.Add(miEdgesD[1].Destination);
                            }
                        }

                    }
                    else
                    {
                        mPathSol = new List<oNode>();
                    }
                    //Polyline miEjeVisibilidadPol = draw(oTadil.data.Layer.visibilidadEje);



                    //Dibujo el Eje Basico Polilinea 3D
                    var miLwEjeBasico3D = drawEjeBasico3D(mEjeBasicoSol.solucionPrimariaNombre, respetarpuntos);

                    //Dibujo el Eje Basico en Planta Polilinea Z=0
                    var miLwEjeBasico2D = drawEjeBasicoPlanta(miLwEjeBasico3D, mEjeBasicoSol.solucionPrimariaNombre);

                    //Añado el Xdata al Eje 2D
                    oTadilXdata.addXdataRoadDesign(miLwEjeBasico2D, mEjeBasicoSol.roadDesign);

                    //Guardo los Datos en la Base Datos
                    oDalTbSolucion.addEjeBasico(mEjeBasicoSol.solucionPrimariaNombre, miLwEjeBasico3D.Handle.ToString(), miLwEjeBasico2D.Handle.ToString(), mEjeBasicoSol.roadDesign, mEjeBasicoSol.roadPendientes, mEjeBasicoSol.coeMinoracionAlturasMaximas);



                }
                catch (Exception)
                {

                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiSolucionPrimariaNotFound);

                }
            }

            miMedicion.Stop();
            oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);


        }
        private Polyline drawEjeBasicoPlanta(Polyline3d iEjeBasico3D, string iSolucionNombre)
        {
            #region "GestionCapas"

            oTadilLayerEjeBasico2D miLayer = new oTadilLayerEjeBasico2D(iSolucionNombre);
            miLayer.deleteItems();

            #endregion


            return engCadNet.oLw.clearPolilinea(iEjeBasico3D, miLayer.name);
        }
        private Polyline3d drawEjeBasico3D(string iSolucionNombre, bool respetarpuntos = true)
        {
            #region "GestionCapas"
            oTadilLayerEjeBasico3D miLayerEjeBasico3d = new oTadilLayerEjeBasico3D(iSolucionNombre);
            miLayerEjeBasico3d.deleteItems();
            #endregion


            #region "Creo la Polilinea"
            Point3dCollection miLstPuntoEjeBasico = new Point3dCollection();
            if (respetarpuntos)
            {
                miLstPuntoEjeBasico.Add(new Point3d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y, oSingletonProyecto.getInstance.ptoSalida.Z));
                this.getPoint3dCollectionEjeBasico_ref(ref miLstPuntoEjeBasico);
                miLstPuntoEjeBasico.Add(new Point3d(oSingletonProyecto.getInstance.ptoLlegada.X, oSingletonProyecto.getInstance.ptoLlegada.Y, oSingletonProyecto.getInstance.ptoLlegada.Z));
            }
            else
            {
                miLstPuntoEjeBasico = this.getPoint3dCollectionEjeBasico();
            }
            //Point3dCollection miLstPuntoEjeBasico = this.getPoint3dCollectionEjeBasico();


            Polyline3d miLw = engCadNet.oLw.addLw3d(miLstPuntoEjeBasico, false, miLayerEjeBasico3d.name);

            #endregion


            #region "AddXdata"

            //this.addXdata(miLw);

            #endregion

            return miLw;
        }
        private Point3dCollection getPoint3dCollectionEjeBasico()
        {
            Point3dCollection myLstPto = new Point3dCollection();
            /* if (mPathSol != null && mPathSol.Count > 1)
             {
                 return null;
             }*/
            foreach (oNode myNode in pPathSol)
            {
                double[] punto = new double[2];
                punto[0] = myNode.pNodePos.X;
                punto[1] = myNode.pNodePos.Y;
                myLstPto.Add(new Point3d(myNode.pNodePos.X, myNode.pNodePos.Y, (double)GetZ(punto, true)));
            }
            return myLstPto;
        }
        private void getPoint3dCollectionEjeBasico_ref(ref Point3dCollection miLstPuntoEjeBasico)
        {
            foreach (oNode myNode in pPathSol)
            {
                double[] punto = new double[2];
                punto[0] = myNode.pNodePos.X;
                punto[1] = myNode.pNodePos.Y;
                miLstPuntoEjeBasico.Add(new Point3d(myNode.pNodePos.X, myNode.pNodePos.Y, (double)GetZ(punto, true)));
            }
        }

        private oNodoGlobal[,] CrearPuntos(double separacion)
        {
            Polyline miLwEjeVisibilidad = null;

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                miLwEjeVisibilidad = engCadNet.oSs.seleccionUsuario<Polyline>(strGeneralUser.uiSelectEjeBasico, strGeneralUser.uiSelectIsNull);
                if (miLwEjeVisibilidad == null)
                {
                    oTadil.data.UserInfo.showInfo("No se seleccionó ninguna polilínea");
                    return null;
                }
                mStart = miLwEjeVisibilidad.StartPoint;
                mGoal = miLwEjeVisibilidad.EndPoint;
                mStart_ext = miLwEjeVisibilidad.StartPoint;
                mGoal_ext = miLwEjeVisibilidad.EndPoint;

                double longitud = miLwEjeVisibilidad.Length;
                mSeparacion = longitud / (int)(longitud / separacion);
                double distanciaOffset = mSeparacion; // distancia entre puntos laterales (ajusta según necesites)
                int numPuntosLado = 5;
                List<Point3d> todosPuntos = new List<Point3d>();
                int miNumeroColumnas = (int)(miLwEjeVisibilidad.Length / mSeparacion) + 1;
                List<oNodoGlobal>[] misNodosPorColumna = new List<oNodoGlobal>[miNumeroColumnas];

                int columna = 0;
                for (double dist = 0; dist < longitud + 1; dist += mSeparacion)
                {
                    try
                    {
                        misNodosPorColumna[columna] = new List<oNodoGlobal>();
                        // Punto en la polilínea
                        Point3d puntoEnPoly = miLwEjeVisibilidad.GetPointAtDist(dist);

                        // Obtener el vector tangente (primera derivada)
                        Vector3d tangente = miLwEjeVisibilidad.GetFirstDerivative(puntoEnPoly);
                        tangente = tangente.GetNormal(); // normalizar

                        // Calcular vector perpendicular (rotar 90 grados en el plano XY)
                        Vector3d perpendicular = new Vector3d(-tangente.Y, tangente.X, 0);
                        perpendicular = perpendicular.GetNormal();
                        double[] punto = new double[2];
                        double miCota = 0;
                        Point3d miNodo = new Point3d(0, 0, 0);
                        // Agregar puntos en el lado izquierdo
                        for (int i = numPuntosLado; i >= 1; i--)
                        {
                            Point3d puntoIzq = puntoEnPoly + (perpendicular * (distanciaOffset * i));
                            punto = new double[2];
                            punto[0] = puntoIzq.X;
                            punto[1] = puntoIzq.Y;
                            miCota = (double)GetZ(punto, false);
                            miNodo = new Point3d(punto[0], punto[1], (double)miCota);


                            if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(punto[0], punto[1]))
                            {
                                misNodosPorColumna[columna].Add(new oNodoGlobal(miNodo));
                            }
                            else
                            {
                                misNodosPorColumna[columna].Add(new oNodoGlobal(miNodo, false));
                            }



                        }

                        // Agregar el punto central (en la polilínea)
                        punto = new double[2];
                        punto[0] = puntoEnPoly.X;
                        punto[1] = puntoEnPoly.Y;
                        miCota = (double)GetZ(punto, false);
                        miNodo = new Point3d(punto[0], punto[1], (double)miCota);
                        if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(punto[0], punto[1]))
                        {
                            misNodosPorColumna[columna].Add(new oNodoGlobal(miNodo));
                        }
                        else
                        {
                            misNodosPorColumna[columna].Add(new oNodoGlobal(miNodo, false));
                        }


                        // Agregar puntos en el lado derecho
                        for (int i = 1; i <= numPuntosLado; i++)
                        {
                            Point3d puntoDer = puntoEnPoly - (perpendicular * (distanciaOffset * i));
                            punto = new double[2];
                            punto[0] = puntoDer.X;
                            punto[1] = puntoDer.Y;
                            miCota = (double)GetZ(punto, false);
                            miNodo = new Point3d(punto[0], punto[1], (double)miCota);
                            if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(punto[0], punto[1]))
                            {
                                misNodosPorColumna[columna].Add(new oNodoGlobal(miNodo));
                            }
                            else
                            {
                                misNodosPorColumna[columna].Add(new oNodoGlobal(miNodo, false));
                            }
                        }
                        columna++;
                    }
                    catch
                    {
                        // Por si la distancia excede la longitud
                        break;
                    }
                }

                //Creo la matriz
                oNodoGlobal[,] myLstNode = new oNodoGlobal[numPuntosLado * 2 + 1, miNumeroColumnas];
                try
                {

                    for (int i = 0; i < miNumeroColumnas; i++)
                    {
                        for (int t = 0; t < numPuntosLado * 2 + 1; t++)
                        {
                            myLstNode[t, i] = misNodosPorColumna[i][t];
                        }
                    }
                }
                catch (Exception e)
                {

                }



                return myLstNode;



            }
        }

        private Polyline Crear2()
        {
            Polyline miLwEjeVisibilidad = null;

            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                // Selección de usuario
                miLwEjeVisibilidad = engCadNet.oSs.seleccionUsuario<Polyline>(strGeneralUser.uiSelectEjeBasico, strGeneralUser.uiSelectIsNull);

                if (miLwEjeVisibilidad == null)
                {
                    oTadil.data.UserInfo.showInfo("No se seleccionó ninguna polilínea");
                    return null;
                }
                mStart = miLwEjeVisibilidad.StartPoint;
                mGoal = miLwEjeVisibilidad.EndPoint;
                mStart_ext = miLwEjeVisibilidad.StartPoint;
                mGoal_ext = miLwEjeVisibilidad.EndPoint;



                // Obtener la base de datos del documento actual
                Database db = oCadManager.thisEditor.Document.Database;

                // Iniciar transacción
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        double offsetDist = 500.0; // 500 metros

                        Polyline envolventePolyline = null;

                        // Verificar si la polilínea está cerrada
                        if (miLwEjeVisibilidad.Closed)
                        {
                            // Si está cerrada, un solo offset hacia afuera crea la envolvente
                            DBObjectCollection offsetCurves = miLwEjeVisibilidad.GetOffsetCurves(offsetDist);

                            if (offsetCurves.Count > 0)
                            {
                                envolventePolyline = offsetCurves[0] as Polyline;
                            }

                            // Limpiar objetos temporales
                            foreach (DBObject obj in offsetCurves)
                            {
                                if (obj != envolventePolyline)
                                {
                                    obj.Dispose();
                                }
                            }
                        }
                        else
                        {
                            // Si está abierta, crear envolvente completa
                            envolventePolyline = CrearEnvolvente(miLwEjeVisibilidad, offsetDist);
                        }

                        if (envolventePolyline != null)
                        {
                            // Obtener el espacio actual para agregar la nueva polilínea
                            BlockTableRecord btr = tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                            // Agregar la polilínea envolvente al dibujo
                            btr.AppendEntity(envolventePolyline);
                            tr.AddNewlyCreatedDBObject(envolventePolyline, true);

                            oTadil.data.UserInfo.showInfo("Envolvente creada exitosamente");

                        }
                        else
                        {
                            oTadil.data.UserInfo.showInfo("No se pudo crear la envolvente");
                        }

                        // Confirmar los cambios en la base de datos
                        tr.Commit();
                        return envolventePolyline;
                    }
                    catch (Exception ex)
                    {
                        oTadil.data.UserInfo.showInfo("Error al crear envolvente: " + ex.Message);
                        tr.Abort();
                        return null;
                    }
                }
            }
        }

        private Polyline CrearEnvolvente(Polyline plineOriginal, double distancia)
        {
            try
            {
                // Crear offset hacia un lado (positivo)
                DBObjectCollection offsetCurves1 = plineOriginal.GetOffsetCurves(distancia);

                // Crear offset hacia el otro lado (negativo)
                DBObjectCollection offsetCurves2 = plineOriginal.GetOffsetCurves(-distancia);

                if (offsetCurves1.Count == 0 || offsetCurves2.Count == 0)
                {
                    return null;
                }

                Polyline offset1 = offsetCurves1[0] as Polyline;
                Polyline offset2 = offsetCurves2[0] as Polyline;

                if (offset1 == null || offset2 == null)
                {
                    return null;
                }

                // Crear la nueva polilínea envolvente
                Polyline envolvente = new Polyline();
                int vertexIndex = 0;

                // Agregar todos los vértices del primer offset (de inicio a fin)
                for (int i = 0; i < offset1.NumberOfVertices; i++)
                {
                    Point2d pt = offset1.GetPoint2dAt(i);
                    double bulge = offset1.GetBulgeAt(i);
                    envolvente.AddVertexAt(vertexIndex++, pt, bulge, 0, 0);
                }

                // Conectar el final del offset1 con el final del offset2
                Point2d endOffset1 = offset1.GetPoint2dAt(offset1.NumberOfVertices - 1);
                Point2d endOffset2 = offset2.GetPoint2dAt(offset2.NumberOfVertices - 1);

                // No agregar línea de conexión como vértice duplicado, 
                // simplemente continuar con los vértices del offset2

                // Agregar los vértices del segundo offset en orden inverso (de fin a inicio)
                for (int i = offset2.NumberOfVertices - 1; i >= 0; i--)
                {
                    Point2d pt = offset2.GetPoint2dAt(i);
                    // El bulge debe invertirse cuando se recorre en sentido inverso
                    double bulge = -offset2.GetBulgeAt((i > 0) ? i - 1 : offset2.NumberOfVertices - 1);
                    envolvente.AddVertexAt(vertexIndex++, pt, bulge, 0, 0);
                }

                // Cerrar la polilínea
                envolvente.Closed = true;

                // Limpiar objetos temporales
                foreach (DBObject obj in offsetCurves1)
                {
                    obj.Dispose();
                }
                foreach (DBObject obj in offsetCurves2)
                {
                    obj.Dispose();
                }

                return envolvente;
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showInfo("Error en CrearEnvolvente: " + ex.Message);
                return null;
            }
        }

        private oNodoGlobal[,] getLstNode(out int miPosFilaCentral)
        {

            oNodoGlobal[,] myLstNode;


            //inicializo los datos 
            double miDistancia = 0;
            Line miLine = new Line(new Point3d(mStart_ext.X, mStart_ext.Y, 0), new Point3d(mGoal_ext.X, mGoal_ext.Y, 0));
            int miNumeroColumnas = (int)(miLine.Length / mSeparacion) + 1;

            //No olvidar sumar una fila para los nodos centrales numerosMaxFilas = miMaxNumeroDeNodosDerecha + miMaxNumeroDeNodosIzquierda + 1(central)
            int miMaxNumeroDeNodosDerecha = 0;
            int miMaxNumeroDeNodosIzquierda = 0;

            //Guarda los nodos correspondientes a cada columna
            List<oNodoGlobal>[] misNodosPorColumna = new List<oNodoGlobal>[miNumeroColumnas];

            //Guarda la posición del nodo central para cada una de las columnas
            int[] miArrayPosNodoCentral = new int[miNumeroColumnas];

            //para cada columna, generarla y comprobar el numero de nodos que contiene
            for (int i = 0; i < miNumeroColumnas; i++)
            {
                miArrayPosNodoCentral[i] = generaColumna(miDistancia, out misNodosPorColumna[i], miLine, (i == 0), (i == miNumeroColumnas - 1), miMaxNumeroDeNodosDerecha, miMaxNumeroDeNodosIzquierda);

                miDistancia = miDistancia + mSeparacion;
                int miNumNodosIzquierda = miArrayPosNodoCentral[i];
                int miNumNodosDerecha = misNodosPorColumna[i].Count - (miNumNodosIzquierda + 1);

                if (miNumNodosIzquierda > miMaxNumeroDeNodosIzquierda) miMaxNumeroDeNodosIzquierda = miNumNodosIzquierda;
                if (miNumNodosDerecha > miMaxNumeroDeNodosDerecha) miMaxNumeroDeNodosDerecha = miNumNodosDerecha;
            }

            //Guarda la posición de la fila central dentro de la nueva matriz
            miPosFilaCentral = miMaxNumeroDeNodosIzquierda;

            //numero de filas
            int miNumFilas = miMaxNumeroDeNodosIzquierda + 1 + miMaxNumeroDeNodosDerecha;


            //Creo la matriz
            myLstNode = new oNodoGlobal[miNumFilas, miNumeroColumnas];


            for (int i = 0; i < miNumeroColumnas; i++)
            {
                int miPosCentralColi = miArrayPosNodoCentral[i];
                int miPosAux = 0;

                //Añadimos a la matriz el nodo central de esta columna
                myLstNode[miPosFilaCentral, i] = misNodosPorColumna[i][miPosCentralColi];

                //Añadimos a la matriz los nodos de la derecha de esta columna
                miPosAux = miPosFilaCentral;
                for (int iDere = miPosCentralColi + 1; iDere < misNodosPorColumna[i].Count; iDere++)
                {
                    miPosAux++;
                    myLstNode[miPosAux, i] = misNodosPorColumna[i][iDere];
                }

                //Añadimos a la matriz los nodos de la izquierda de esta columna
                miPosAux = miPosFilaCentral - miPosCentralColi - 1;
                for (int iIzq = 0; iIzq < miPosCentralColi; iIzq++)
                {
                    miPosAux++;
                    myLstNode[miPosAux, i] = misNodosPorColumna[i][iIzq];
                }

                //Se Supone que el resto de valores se quedaran a null (comprobar)

            }

            return myLstNode;
        }
        private int generaColumna(double iDist, out List<oNodoGlobal> iNodos, Line iLinePerpendicular, bool isNodoEntrada, bool isNodoSalida, int maxDer, int maxIzq)
        {
            int miPosNodoCentral = -1;
            iNodos = new List<oNodoGlobal>();
            List<oNodoGlobal> iNodosDer = new List<oNodoGlobal>();
            List<oNodoGlobal> iNodosIzq = new List<oNodoGlobal>();
            double miOffset = mSeparacion;
            bool derechaDentro = true;
            bool izquierdaDentro = true;

            oNodoGlobal miNodoCentral;
            double[] miPosXYNodoCentral = oLine.getPointAtDist(iDist, iLinePerpendicular);
            /*
                 * Se cambia ya que lo que busca es el triangulo del punto **juanma**
                 */
            //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(miPosXYNodoCentral);
            //Triangulo miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPosXYNodoCentral[0], miPosXYNodoCentral[1]);
            //La segunda opcion esta con ****
            //****Triangulo miTriangulo = GetTriangulo(miPosXYNodoCentral);
            if (isNodoEntrada)
            {
                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPosXYNodoCentral[0], miPosXYNodoCentral[1], miTriangulo);
                //****double? miCota = GetZ_triangulo(miPosXYNodoCentral, miTriangulo);
                double? miCota = GetZ(miPosXYNodoCentral, true);
                miNodoCentral = new oNodoGlobal(true, new Point3d(miPosXYNodoCentral[0], miPosXYNodoCentral[1], (double)miCota));
            }
            else if (isNodoSalida)
            {
                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPosXYNodoCentral[0], miPosXYNodoCentral[1], miTriangulo);
                //****double? miCota = GetZ_triangulo(miPosXYNodoCentral, miTriangulo);
                double? miCota = GetZ(miPosXYNodoCentral, true);
                miNodoCentral = new oNodoGlobal(false, new Point3d(miPosXYNodoCentral[0], miPosXYNodoCentral[1], (double)miCota));
            }
            else
            {

                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPosXYNodoCentral[0], miPosXYNodoCentral[1], miTriangulo);
                //****double? miCota = GetZ_triangulo(miPosXYNodoCentral, miTriangulo);
                double? miCota = (double)GetZ(miPosXYNodoCentral, true);
                if (miCota == null || double.IsNaN(miCota.Value))
                {
                    miNodoCentral = null;
                }
                else
                {
                    if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPosXYNodoCentral[0], miPosXYNodoCentral[1]))
                    {
                        miNodoCentral = new oNodoGlobal(new Point3d(miPosXYNodoCentral[0], miPosXYNodoCentral[1], (double)miCota));
                    }
                    else
                    {
                        miNodoCentral = new oNodoGlobal(new Point3d(miPosXYNodoCentral[0], miPosXYNodoCentral[1], (double)miCota), false);
                    }
                }
            }


            //Calculo los puntos por la izquierda
            oLine.getPointAtLocation(iDist, miOffset, false, iLinePerpendicular);

            miOffset = mSeparacion;

            //miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPosXYNodoCentral[0], miPosXYNodoCentral[1]);
            //****miTriangulo = GetTriangulo(miPosXYNodoCentral);
            while ((izquierdaDentro) || (iNodosIzq.Count < maxIzq))
            {

                double[] miPunto = oLine.getPointAtLocation(iDist, miOffset, false, iLinePerpendicular);
                /*
                 * Se cambia ya que lo que busca es el triangulo del punto **juanma**
                 */
                //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(miPunto);
                //miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPunto[0], miPunto[1]);
                //****miTriangulo = GetTriangulo(miPunto);
                /*if (miTriangulo != null)
                {
                    miTriangulo = oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(miPunto[0], miPunto[1], miTriangulo);
                }
                else
                {
                    miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPunto[0], miPunto[1]);
                   
                }*/
                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPunto[0], miPunto[1], miTriangulo);
                double? miCota = null;
                /****if (miTriangulo!=null)
                {
                    miCota = GetZ_triangulo(miPunto, miTriangulo);
                }****/
                miCota = GetZ(miPunto, true);
                if (miCota == null || double.IsNaN(miCota.Value))
                {
                    izquierdaDentro = false;
                    iNodosIzq.Add(null);
                    miOffset = miOffset + mSeparacion;
                }
                else
                {
                    izquierdaDentro = true;
                    if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPunto[0], miPunto[1]))
                    {
                        Point3d miNodo = new Point3d(miPunto[0], miPunto[1], (double)miCota);
                        iNodosIzq.Add(new oNodoGlobal(miNodo));
                    }
                    else
                    {
                        Point3d miNodo = new Point3d(miPunto[0], miPunto[1], (double)miCota);
                        iNodosIzq.Add(new oNodoGlobal(miNodo, false));
                    }
                    miOffset = miOffset + mSeparacion;
                }

            }


            //Caluculo los puntos por la derecha
            miOffset = mSeparacion;
            //miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPosXYNodoCentral[0], miPosXYNodoCentral[1]);
            //****miTriangulo = GetTriangulo(miPosXYNodoCentral);
            while ((derechaDentro) || (iNodosDer.Count < maxDer))
            {
                double[] miPunto = oLine.getPointAtLocation(iDist, miOffset, true, iLinePerpendicular);
                /*
                 * Se cambia ya que lo que busca es el triangulo del punto **juanma**
                 */
                //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(miPunto);
                //miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPunto[0], miPunto[1]);
                //****miTriangulo = GetTriangulo(miPunto);
                /*if (miTriangulo != null)
                {
                    miTriangulo = oSingletonTerreno.getInstance.getTrianguloInsideFromTriangulo(miPunto[0], miPunto[1], miTriangulo);
                }
                else
                {
                    miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miPunto[0], miPunto[1]);
                    
                }*/
                //double? miCota = oSingletonTerreno.getInstance.getZFromTriang(miPunto[0], miPunto[1], miTriangulo);
                double? miCota = null;
                /****if (miTriangulo != null)
                {
                    miCota = GetZ_triangulo(miPunto, miTriangulo);
                }****/
                miCota = GetZ(miPunto, true);
                if (miCota == null || double.IsNaN(miCota.Value))
                {
                    derechaDentro = false;
                    iNodosDer.Add(null);
                    miOffset = miOffset + mSeparacion;
                }
                else
                {
                    derechaDentro = true;
                    if (!oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPunto[0], miPunto[1]))
                    {
                        Point3d miNodo = new Point3d(miPunto[0], miPunto[1], (double)miCota);
                        iNodosDer.Add(new oNodoGlobal(miNodo));
                    }
                    else
                    {
                        Point3d miNodo = new Point3d(miPunto[0], miPunto[1], (double)miCota);
                        iNodosDer.Add(new oNodoGlobal(miNodo, false));
                    }
                    miOffset = miOffset + mSeparacion;
                }

            }

            //ordeno los puntos de izquierda a derecha para crear la salida
            //primero añado los puntos de la izquierda desde el ultimo al primero
            for (int i = iNodosIzq.Count - 1; i >= 0; i--)
            {
                iNodos.Add(iNodosIzq[i]);
            }

            //añado el punto central
            iNodos.Add(miNodoCentral);

            //el numero de la posición del nodo central
            miPosNodoCentral = iNodos.Count - 1;

            //añado los puntos de la derecha desde el primero al ultimo
            for (int i = 0; i < iNodosDer.Count; i++)
            {
                iNodos.Add(iNodosDer[i]);
            }
            return miPosNodoCentral;
        }
        private void filtarPuntosValidosPorCota(double iVelocidad, double iPendMaxima, ref oNodoGlobal[,] miMatriz)
        {
            Point2d myStart2d = new Point2d(mStart.X, mStart.Y);
            Point2d myGoal2d = new Point2d(mGoal.X, mGoal.Y);
            double p = 0;

            if (iVelocidad < 40) p = 0.4;
            if ((iVelocidad >= 40) && (iVelocidad < 80)) p = 0.3;
            if ((iVelocidad >= 80) && (iVelocidad <= 120)) p = 0.2;
            if (iVelocidad > 120) p = 0.1;

            for (int i = 0; i < miMatriz.GetLength(0); i++)
            {
                for (int j = 0; j < miMatriz.GetLength(1); j++)
                {
                    if (miMatriz[i, j] != null)
                    {
                        if (!((miMatriz[i, j].mNodo.X == myStart2d.X) && (miMatriz[i, j].mNodo.Y == myStart2d.Y)) && !((miMatriz[i, j].mNodo.X == myGoal2d.X) && (miMatriz[i, j].mNodo.Y == myGoal2d.Y)))
                        {
                            //comprobacion cota entrada
                            if (miMatriz[i, j].mIsValido)
                            {
                                double miDisToEntrada = myStart2d.GetDistanceTo(new Point2d(miMatriz[i, j].mNodo.X, miMatriz[i, j].mNodo.Y));
                                miDisToEntrada = miDisToEntrada * (1 + p);
                                double distanciaMinima = (miMatriz[i, j].mNodo.Z - mStart.Z) / iPendMaxima;
                                if (!(miDisToEntrada > distanciaMinima)) miMatriz[i, j].mIsValido = false;

                            }
                            //comprobacion cota salida
                            if (miMatriz[i, j].mIsValido)
                            {
                                double miDisToSalida = myGoal2d.GetDistanceTo(new Point2d(miMatriz[i, j].mNodo.X, miMatriz[i, j].mNodo.Y));
                                miDisToSalida = miDisToSalida * (1 + p);
                                double distanciaMinima = (miMatriz[i, j].mNodo.Z - mGoal.Z) / iPendMaxima;
                                if (!(miDisToSalida > distanciaMinima)) miMatriz[i, j].mIsValido = false;

                            }
                        }
                    }
                }
            }

        }

        private void filtarPuntosValidosPorPendiente(double iPendMaxima, ref oNodoGlobal[,] miMatriz)
        {
            Point2d myStart2d = new Point2d(mStart.X, mStart.Y);
            Point2d myGoal2d = new Point2d(mGoal.X, mGoal.Y);

            for (int i = 0; i < miMatriz.GetLength(0); i++)
            {
                for (int j = 0; j < miMatriz.GetLength(1); j++)
                {
                    if (miMatriz[i, j] != null)
                    {
                        if (!((miMatriz[i, j].mNodo.X == myStart2d.X) && (miMatriz[i, j].mNodo.Y == myStart2d.Y)) && !((miMatriz[i, j].mNodo.X == myGoal2d.X) && (miMatriz[i, j].mNodo.Y == myGoal2d.Y)))
                        {
                            //comprobacion pendiente del triangulo
                            if (miMatriz[i, j].mIsValido)
                            {
                                double[] mipunto = new double[2];
                                mipunto[0] = miMatriz[i, j].mNodo.X;
                                mipunto[1] = miMatriz[i, j].mNodo.Y;
                                //oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(mipunto);
                                //Triangulo miTriangulo = oSingletonTerreno.getInstance.getTrianguloInside(miMatriz[i, j].mNodo.X, miMatriz[i, j].mNodo.Y);
                                double[] punto = new double[2];
                                punto[0] = miMatriz[i, j].mNodo.X;
                                punto[1] = miMatriz[i, j].mNodo.Y;
                                //****Triangulo miTriangulo = GetTriangulo(punto);


                                //****if (miTriangulo.getPendienteMaxima > iPendMaxima) miMatriz[i, j].mIsValido = false;
                                if (GetSlope(punto) > iPendMaxima) miMatriz[i, j].mIsValido = false;
                            }
                        }
                    }
                }
            }

        }

        private List<oEdge> createEdges(List<oConexionGlobal> misConexionesValidas, oEjeBasicoSolucion iEjeBasicoSol)
        {


            List<oEdge> miEdgesD = new List<oEdge>();
            double maxlongitud = 0;
            double maxCoste = 0;
            double maxOrografia = 0;
            double minOrografia = double.MaxValue;



            foreach (oConexionGlobal micon in misConexionesValidas)
            {


                micon.mDistancia = micon.mTramo.P1.distTo2d(micon.mTramo.P2);
                micon.mCoste = micon.mTramo.valoracionCosteImplantacionTramo() / micon.mDistancia;

                //micon.mOrografia = (micon.mTramo.valoracionPendienteTramo() / micon.mTramo.seccion.Count) / micon.mDistancia;//Math.Abs(micon.mPuntoMayor.Z - micon.mPuntoMenor.Z) / micon.mTramo.P1.distTo2d(micon.mTramo.P2);

                double sumaValoraciones = 0;
                foreach (oSeccionEjeBasico miSeccion in micon.mTramo.seccion)
                {
                    sumaValoraciones = sumaValoraciones + miSeccion.valoracionSlopeUnitaria();
                }
                micon.mOrografia = sumaValoraciones / micon.mTramo.seccion.Count;

                if (maxlongitud < micon.mDistancia) maxlongitud = micon.mDistancia;
                if (maxCoste < micon.mCoste) maxCoste = micon.mCoste;
                if (maxOrografia < micon.mOrografia) maxOrografia = micon.mOrografia;
                //if (minOrografia > micon.mOrografia) minOrografia = micon.mOrografia;

            }



            Point2d miGoal2d = new Point2d(this.mGoal.X, this.mGoal.Y);
            foreach (oConexionGlobal micon in misConexionesValidas)
            {

                micon.mValCoste = 10 * micon.mCoste / maxCoste;

                micon.mValDistancia = 10 * micon.mDistancia / maxlongitud;

                micon.mValOrografia = maxOrografia - micon.mOrografia;

                double costeGlobal = micon.mValCoste * iEjeBasicoSol.tramosValoracion.valoracionCosteImplantacionPC / 100;

                double valDistGlobal = micon.mValDistancia * iEjeBasicoSol.tramosValoracion.valoracionDistanciaPC / 100;

                double valOroGlobal = micon.mValOrografia * iEjeBasicoSol.tramosValoracion.valoracionPendientePC / 100;

                double notaGlobal = costeGlobal + valDistGlobal + valOroGlobal;

                micon.mConexionDijkstra = new oEdge(micon.mConexionDijkstra.Origin, micon.mConexionDijkstra.Destination, notaGlobal);

                miEdgesD.Add(micon.mConexionDijkstra);
                miEdgesD.Add(new oEdge(micon.mConexionDijkstra.Destination, micon.mConexionDijkstra.Origin, notaGlobal));
            }

            return miEdgesD;
        }
        public Polyline draw(oTadilLayer iLayerEjeVisibilidad)
        {
            //Logica Capas
            iLayerEjeVisibilidad.On();
            iLayerEjeVisibilidad.Current();
            iLayerEjeVisibilidad.deleteItems();


            Point3dCollection myLstPto = new Point3dCollection();
            /* if (mPathSol != null && mPathSol.Count > 1)
             {
                 return null;
             }*/
            foreach (oNode myNode in pPathSol)
            {
                myLstPto.Add(new Point3d(myNode.pNodePos.X, myNode.pNodePos.Y, 0));
            }

            return engCadNet.oLw.addLw2d(myLstPto, false, iLayerEjeVisibilidad.name, iLayerEjeVisibilidad.color);
        }
        private List<oNode> pPathSol
        {

            get
            {

                if (mPathSol != null && mPathSol.Count > 1)
                {
                    return mPathSol;
                }
                else
                {

                    throw new Exception("La Solución es Nula");

                }

            }

            set
            {
                mPathSol = value;
            }

        }
        public double? GetZ(double[] punto, bool comprobar = false)
        {
            /*
             * Funciona correctamente el 3 para que busque dentro del rango de los puntos
             */
            if (oSingletonTerreno.getInstance.tipo == 1)
            {
                return oSingletonTerreno.getInstance.getZFromXY(punto);
            }
            else if (oSingletonTerreno.getInstance.tipo == 2)
            {
                return oSingletonPuntosTerreno.getInstance.GetZ(punto, comprobar);
            }
            else if (oSingletonTerreno.getInstance.tipo == 3)
            {
                return oSingletonPuntosTerrenoASC.getInstance.GetZ(punto);
            }
            else
            {
                return oSingletonTerreno.getInstance.getZFromXY(punto);
            }
            return -1;
        }
        public double? GetSlope(double[] punto)
        {
            if (oSingletonTerreno.getInstance.tipo == 1)
            {
                return oSingletonTerreno.getInstance.getSlopeFromXY(punto);
            }
            else if (oSingletonTerreno.getInstance.tipo == 2)
            {
                return oSingletonPuntosTerreno.getInstance.Slope(punto[0], punto[1]);
            }
            else if (oSingletonTerreno.getInstance.tipo == 3)
            {
                return oSingletonPuntosTerrenoASC.getInstance.Slope(punto[0], punto[1]);
            }
            else
            {
                return oSingletonTerreno.getInstance.getZFromXY(punto);
            }
            return -1;
        }

        /* public Polyline3d drawEjeBasico3D(string iSolucionNombre)
         {
             #region "GestionCapas"
             oTadilLayerEjeBasico3D miLayerEjeBasico3d = new oTadilLayerEjeBasico3D(iSolucionNombre);
             miLayerEjeBasico3d.deleteItems();
             #endregion


             #region "Creo la Polilinea"

             Point3dCollection miLstPuntoEjeBasico = this.getPoint3dCollectionEjeBasico();

             Polyline3d miLw = engCadNet.oLw.addLw3d(miLstPuntoEjeBasico, false, miLayerEjeBasico3d.name);

             #endregion


             #region "AddXdata"

             this.addXdata(miLw);

             #endregion

             return miLw;
         }*/
    }

    #region "INTERNAL CLASS"
    internal class oNodoGlobal
    {
        public Point3d mNodo { get; set; }
        public bool mIsValido { get; set; }
        bool isEntrada = false;
        bool isSalida = false;
        public oNodeCad mNodoDijkstra { get; set; }

        public oNodoGlobal(Point3d iNodo, bool isValido)
        {
            mNodo = iNodo;
            mIsValido = isValido;
        }


        public oNodoGlobal(Point3d iNodo)
        {
            mNodo = iNodo;
            mIsValido = true;
        }

        public oNodoGlobal(bool iEntradaTrue_SalidaFalse, Point3d iNodo)
        {
            mNodo = iNodo;
            mIsValido = true;
            if (iEntradaTrue_SalidaFalse)
            {
                isEntrada = true;
            }
            else
            {
                isSalida = true;
            }
        }
    }

    internal class oConexionGlobal
    {
        public Point3d mPuntoMenor { get; set; }
        public Point3d mPuntoMayor { get; set; }
        public bool mIsValido { get; set; }
        public double mPuntuacion { get; set; }
        public string hashCode { get; set; }
        public oEdge mConexionDijkstra { get; set; }

        public oTramoAvanceCorto mTramo { get; set; }


        private static oRoadDes mRoadDesign = null;
        private static oRoadPendientes mRoadPendiente = null;

        private static oAbanicoDesign mAbanicoDesign = null;
        private static IEstudio mEstudioDatos = null;

        public double mDistancia;
        public double mCoste;
        public double mOrografia;

        public double mValDistancia;
        public double mValCoste;
        public double mValOrografia;



        public oConexionGlobal(oNodoGlobal iPunto1, oNodoGlobal iPunto2)
        {
            int miComp = compareTo(iPunto1.mNodo, iPunto2.mNodo);
            if (miComp == -1)
            {
                mPuntoMenor = iPunto1.mNodo;
                mPuntoMayor = iPunto2.mNodo;
                this.mConexionDijkstra = new oEdge(iPunto1.mNodoDijkstra, iPunto2.mNodoDijkstra, mPuntoMenor.DistanceTo(mPuntoMayor));
            }
            else if (miComp == 1)
            {
                mPuntoMenor = iPunto2.mNodo;
                mPuntoMayor = iPunto1.mNodo;
                this.mConexionDijkstra = new oEdge(iPunto2.mNodoDijkstra, iPunto1.mNodoDijkstra, mPuntoMenor.DistanceTo(mPuntoMayor));
            }
            else
            {
                throw new Exception("No se puede crear una conexion si los puntos son iguales");
            }
            hashCode = mPuntoMenor.X + "_" + mPuntoMenor.Y + "_" + mPuntoMayor.X + "_" + mPuntoMayor.Y;
        }


        public static void SetUpObject(oRoadDes iRoadDesign, oRoadPendientes iRoadPendiente, oAbanicoDesign iDataAbanico, IEstudio iEstudioDatos)
        {
            mRoadDesign = iRoadDesign;
            mRoadPendiente = iRoadPendiente;
            mAbanicoDesign = iDataAbanico;
            mEstudioDatos = iEstudioDatos;

        }

        public bool validarConexion(ref List<oConexionGlobal> iConexionesValidas, ref List<oConexionGlobal> iConexionesNOValidas, bool dibujar)
        {
            if (iConexionesValidas.Contains(this))
            {
                mIsValido = true;
            }
            else if (iConexionesNOValidas.Contains(this))
            {
                mIsValido = false;
            }
            else
            {
                //Validar
                mIsValido = true;
                mTramo = new oTramoAvanceCorto();
                mTramo.P1 = new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z);
                mTramo.P2 = new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z);
                //(oTramoAbanico)new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                mTramo.idTramo = 0;
                mTramo.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;
                //mTramo.ptoTarget 
                mTramo.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);



                mIsValido = mTramo.isTramoValido;

                if (mIsValido)
                {
                    oTramoEjeBasico miTramoEje = new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                    miTramoEje.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);
                    miTramoEje.idTramo = 0;
                    if (dibujar)
                    {
                        miTramoEje.drawTramo2D(oTadil.data.Layer.visibilidadGrafo.name);
                    }



                    iConexionesValidas.Add(this);
                }
                else
                {
                    iConexionesNOValidas.Add(this);
                }

            }
            return mIsValido;
        }
        public bool validarConexion(ref ConcurrentBag<oConexionGlobal> iConexionesValidas, ref ConcurrentBag<oConexionGlobal> iConexionesNOValidas, bool dibujar)
        {
            if (iConexionesValidas.Contains(this))
            {
                mIsValido = true;
            }
            else if (iConexionesNOValidas.Contains(this))
            {
                mIsValido = false;
            }
            else
            {
                //Validar
                mIsValido = true;
                mTramo = new oTramoAvanceCorto();
                mTramo.P1 = new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z);
                mTramo.P2 = new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z);
                //(oTramoAbanico)new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                mTramo.idTramo = 0;
                mTramo.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;
                //mTramo.ptoTarget 
                mTramo.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);



                mIsValido = mTramo.isTramoValido;

                if (mIsValido)
                {
                    oTramoEjeBasico miTramoEje = new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                    miTramoEje.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);
                    miTramoEje.idTramo = 0;
                    if (dibujar)
                    {
                        miTramoEje.drawTramo2D(oTadil.data.Layer.visibilidadGrafo.name);
                    }



                    iConexionesValidas.Add(this);
                }
                else
                {
                    iConexionesNOValidas.Add(this);
                }

            }
            return mIsValido;
        }
        public bool validarConexion(ref ConcurrentBag<oConexionGlobal> iConexionesValidas, ref ConcurrentBag<oConexionGlobal> iConexionesNOValidas)
        {
            if (iConexionesValidas.Contains(this))
            {
                mIsValido = true;
            }
            else if (iConexionesNOValidas.Contains(this))
            {
                mIsValido = false;
            }
            else
            {
                //Validar
                mIsValido = true;
                mTramo = new oTramoAvanceCorto();
                mTramo.P1 = new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z);
                mTramo.P2 = new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z);
                //(oTramoAbanico)new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                mTramo.idTramo = 0;
                mTramo.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceCorto;
                //mTramo.ptoTarget 
                mTramo.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);



                mIsValido = mTramo.isTramoValido;

                if (mIsValido)
                {
                    oTramoEjeBasico miTramoEje = new oTramoEjeBasico(new oP3d(this.mPuntoMenor.X, this.mPuntoMenor.Y, this.mPuntoMenor.Z), new oP3d(this.mPuntoMayor.X, this.mPuntoMayor.Y, this.mPuntoMayor.Z), eTramoTipoEjeBasico.avanceCorto);

                    miTramoEje.createSeccionP1P2(mAbanicoDesign.tramoAbanicoDiscretizacion, mRoadPendiente, mEstudioDatos, false);
                    miTramoEje.idTramo = 0;
                    miTramoEje.drawTramo2D(oTadil.data.Layer.visibilidadGrafo.name);

                    lock (this)
                    {
                        iConexionesValidas.Add(this);
                    }

                }
                else
                {
                    lock (this)
                    {
                        iConexionesNOValidas.Add(this);
                    }
                }

            }
            return mIsValido;
        }

        public override string ToString()
        {
            StringBuilder miStr = new StringBuilder();
            miStr.AppendLine("P1: " + this.mTramo.P1.ToString());
            miStr.AppendLine("P2 : " + this.mTramo.P2.ToString());
            miStr.AppendLine(" distancia : " + this.mDistancia.ToString());
            miStr.AppendLine(" orografia : " + this.mOrografia.ToString());
            miStr.AppendLine(" coste : " + this.mCoste.ToString());
            miStr.AppendLine("Valoracion distancia : " + this.mValDistancia.ToString());
            miStr.AppendLine("Valoracion orografia : " + this.mValOrografia.ToString());
            miStr.AppendLine("Valoracion coste : " + this.mValCoste.ToString());
            return miStr.ToString();

        }

        public override bool Equals(object obj)
        {
            try
            {
                oConexionGlobal miCon = (oConexionGlobal)obj;

                if (miCon.hashCode == this.hashCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private int compareTo(Point3d iP1, Point3d iP2)
        {
            int miComp = 2;
            if (iP1.X < iP2.X)
            {
                miComp = -1;
            }
            else if (iP1.X > iP2.X)
            {
                miComp = 1;
            }
            else
            {
                if (iP1.Y < iP2.Y)
                {
                    miComp = -1;
                }
                else if (iP1.Y > iP2.Y)
                {
                    miComp = 1;
                }
                else
                {
                    miComp = 0;
                }
            }
            return miComp;
        }


    }

    internal class oArañaBusquedaGlobal
    {
        List<oConexionGlobal> mConexiones { get; set; }
        public oNodoGlobal mPuntoCentral { get; set; }
        int mI;
        int mJ;
        double mTamañoCelda; // Añadir para calcular distancias

        public oArañaBusquedaGlobal(oNodoGlobal[,] miMatriz, int iPosINodoCentral, int iPosJNodoCentral)
        {
            mConexiones = new List<oConexionGlobal>();
            mPuntoCentral = miMatriz[iPosINodoCentral, iPosJNodoCentral];
            mI = iPosINodoCentral;
            mJ = iPosJNodoCentral;
        }
        public oArañaBusquedaGlobal(oNodoGlobal[,] miMatriz, int iPosINodoCentral, int iPosJNodoCentral, double iTamañoCelda)
        {
            mConexiones = new List<oConexionGlobal>();
            mPuntoCentral = miMatriz[iPosINodoCentral, iPosJNodoCentral];
            mI = iPosINodoCentral;
            mJ = iPosJNodoCentral;
            mTamañoCelda = iTamañoCelda;
        }


        public void creaConexiones(oNodoGlobal[,] miMatriz, ref List<oConexionGlobal> iConexionesValidas, ref List<oConexionGlobal> iConexionesNOValidas,
            Point3d iGoal, bool dibujar)
        {
            //creo una a una todas las coneciones, si son diferente a null las añado a mi lista de conexiones (ya estan añadidas en iConexionesValidas)
            oConexionGlobal miConexion;
            /*
             * Se comenta el añadido a las mConexiones para intentar que no se pinten **juanma**
             */
            miConexion = creaConexion(miMatriz, 0, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 0, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, 0, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, 0, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);


            miConexion = creaConexion(miMatriz, 1, 2, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 1, -2, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 2, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 2, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, -1, 2, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, -2, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, -2, 1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -2, -1, ref iConexionesValidas, ref iConexionesNOValidas, iGoal, dibujar);
            if (miConexion != null) mConexiones.Add(miConexion);

            /*
             * Se comenta para ver velocidad de ejecucion **juanma**
             */
            oCadManager.thisEditor.UpdateScreen();



        }
        public Tuple<List<oConexionGlobal>, List<oConexionGlobal>> creaConexiones(oNodoGlobal[,] miMatriz, Point3d iGoal)
        {
            //creo una a una todas las coneciones, si son diferente a null las añado a mi lista de conexiones (ya estan añadidas en iConexionesValidas)
            oConexionGlobal miConexion;
            /*
             * Se comenta el añadido a las mConexiones para intentar que no se pinten **juanma**
             */
            List<oConexionGlobal> iConexionesValidas_ = new List<oConexionGlobal>();
            List<oConexionGlobal> iConexionesNOValidas_ = new List<oConexionGlobal>();
            miConexion = creaConexion(miMatriz, 0, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 0, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, 0, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, 0, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 1, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);


            miConexion = creaConexion(miMatriz, 1, 2, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 1, -2, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, 2, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, 2, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, -1, 2, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -1, -2, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            miConexion = creaConexion(miMatriz, -2, 1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);
            miConexion = creaConexion(miMatriz, -2, -1, ref iConexionesValidas_, ref iConexionesNOValidas_, iGoal);
            //if (miConexion != null) mConexiones.Add(miConexion);

            /*
             * Se comenta para ver velocidad de ejecucion **juanma**
             */
            //oCadManager.thisEditor.UpdateScreen();
            return Tuple.Create(iConexionesValidas_, iConexionesNOValidas_);


        }
        private oConexionGlobal creaConexion(oNodoGlobal[,] miMatriz, int iVariacionPosI, int iVariacionPosJ, ref List<oConexionGlobal> iConexionesValidas, ref List<oConexionGlobal> iConexionesNOValidas, Point3d iGoal, bool dibujar = true)
        {
            oConexionGlobal miConexion = null;

            int miFilas = miMatriz.GetLength(0);
            int miColumnas = miMatriz.GetLength(1);

            int miPosIDestino = mI + iVariacionPosI;
            int miPosJDestino = mJ + iVariacionPosJ;

            bool conexionEncontrada = false;

            while (!conexionEncontrada)
            {
                if ((miPosIDestino >= 0) && (miPosIDestino < miFilas))
                {
                    if ((miPosJDestino >= 0) && (miPosJDestino < miColumnas))
                    {
                        oNodoGlobal miDestino = miMatriz[miPosIDestino, miPosJDestino];
                        if (miDestino == null)
                        {
                            conexionEncontrada = true;
                        }
                        else
                        {
                            if (miDestino.mIsValido)
                            {
                                if (!oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso(new oP2d(mPuntoCentral.mNodo.X, mPuntoCentral.mNodo.Y), new oP2d(miDestino.mNodo.X, miDestino.mNodo.Y)))
                                {
                                    miConexion = new oConexionGlobal(mPuntoCentral, miDestino);
                                    if (miConexion.validarConexion(ref iConexionesValidas, ref iConexionesNOValidas, dibujar))
                                    {
                                        if (miConexion.mTramo != null)
                                        {
                                            miConexion.mTramo.ptoTarget = new oP2d(iGoal.X, iGoal.Y);
                                        }
                                        return miConexion;
                                    }
                                }
                                else
                                {
                                    conexionEncontrada = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        conexionEncontrada = true;
                    }
                }
                else
                {
                    conexionEncontrada = true;
                }
                miPosIDestino = miPosIDestino + iVariacionPosI;
                miPosJDestino = miPosJDestino + iVariacionPosJ;

            }

            return miConexion;
        }
        private oConexionGlobal creaConexion(oNodoGlobal[,] miMatriz, int iVariacionPosI, int iVariacionPosJ, ref ConcurrentBag<oConexionGlobal> iConexionesValidas, ref ConcurrentBag<oConexionGlobal> iConexionesNOValidas, Point3d iGoal)
        {
            oConexionGlobal miConexion = null;

            int miFilas = miMatriz.GetLength(0);
            int miColumnas = miMatriz.GetLength(1);

            int miPosIDestino = mI + iVariacionPosI;
            int miPosJDestino = mJ + iVariacionPosJ;

            bool conexionEncontrada = false;

            while (!conexionEncontrada)
            {
                if ((miPosIDestino >= 0) && (miPosIDestino < miFilas))
                {
                    if ((miPosJDestino >= 0) && (miPosJDestino < miColumnas))
                    {
                        oNodoGlobal miDestino = miMatriz[miPosIDestino, miPosJDestino];
                        if (miDestino == null)
                        {
                            conexionEncontrada = true;
                        }
                        else
                        {
                            if (miDestino.mIsValido)
                            {
                                if (!oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso(new oP2d(mPuntoCentral.mNodo.X, mPuntoCentral.mNodo.Y), new oP2d(miDestino.mNodo.X, miDestino.mNodo.Y)))
                                {
                                    miConexion = new oConexionGlobal(mPuntoCentral, miDestino);
                                    if (miConexion.validarConexion(ref iConexionesValidas, ref iConexionesNOValidas))
                                    {
                                        if (miConexion.mTramo != null)
                                        {
                                            miConexion.mTramo.ptoTarget = new oP2d(iGoal.X, iGoal.Y);
                                        }
                                        return miConexion;
                                    }
                                }
                                else
                                {
                                    conexionEncontrada = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        conexionEncontrada = true;
                    }
                }
                else
                {
                    conexionEncontrada = true;
                }
                miPosIDestino = miPosIDestino + iVariacionPosI;
                miPosJDestino = miPosJDestino + iVariacionPosJ;

            }

            return miConexion;
        }



        /// <summary>
        /// Busca conexiones entre los puntos
        /// </summary>
        /// <param name="miMatriz"></param>
        /// <param name="iConexionesValidas"></param>
        /// <param name="iConexionesNOValidas"></param>
        /// <param name="iGoal"></param>
        /// <param name="iDistanciaMinima"></param>
        /// <param name="iDistanciaMaxima"></param>
        /// <param name="dibujar"></param>
        public void creaConexionesDinamicas_old(oNodoGlobal[,] miMatriz,
                                     ref List<oConexionGlobal> iConexionesValidas,
                                     ref List<oConexionGlobal> iConexionesNOValidas,
                                     Point3d iGoal,
                                     double iDistanciaMinima,
                                     double iDistanciaMaxima,
                                     bool dibujar)
        {
            int miFilas = miMatriz.GetLength(0);
            int miColumnas = miMatriz.GetLength(1);

            int radioMaxIndices = (int)Math.Ceiling(iDistanciaMaxima / mTamañoCelda) + 1;

            for (int offsetI = -radioMaxIndices; offsetI <= radioMaxIndices; offsetI++)
            {
                for (int offsetJ = -radioMaxIndices; offsetJ <= radioMaxIndices; offsetJ++)
                {
                    // Saltar el nodo central
                    if (offsetI == 0 && offsetJ == 0)
                        continue;

                    int miPosIDestino = mI + offsetI;
                    int miPosJDestino = mJ + offsetJ;

                    if (miPosIDestino < 0 || miPosIDestino >= miFilas)
                        continue;
                    if (miPosJDestino < 0 || miPosJDestino >= miColumnas)
                        continue;

                    oNodoGlobal miDestino = miMatriz[miPosIDestino, miPosJDestino];

                    if (miDestino == null || !miDestino.mIsValido)
                        continue;

                    double distanciaReal = mPuntoCentral.mNodo.DistanceTo(miDestino.mNodo);

                    if (distanciaReal >= iDistanciaMinima && distanciaReal <= iDistanciaMaxima)
                    {
                        if (!oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso(
                            new oP2d(mPuntoCentral.mNodo.X, mPuntoCentral.mNodo.Y),
                            new oP2d(miDestino.mNodo.X, miDestino.mNodo.Y)))
                        {
                            oConexionGlobal miConexion = new oConexionGlobal(mPuntoCentral, miDestino);

                            // CORRECCIÓN: Verificar si es válida primero
                            bool esValida = miConexion.validarConexion(ref iConexionesValidas, ref iConexionesNOValidas, dibujar);

                            // IMPORTANTE: Agregar a mConexiones si es válida, 
                            // independientemente de si ya existía en las listas globales
                            if (esValida || miConexion.mIsValido)
                            {
                                if (miConexion.mTramo != null)
                                {
                                    miConexion.mTramo.ptoTarget = new oP2d(iGoal.X, iGoal.Y);
                                }

                                // Verificar que no se agregue duplicada a mConexiones
                                if (!mConexiones.Contains(miConexion))
                                {
                                    mConexiones.Add(miConexion);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// busca conexiones entre los puntos teniendo como minimo una conexion por fila
        /// </summary>
        /// <param name="miMatriz"></param>
        /// <param name="iConexionesValidas"></param>
        /// <param name="iConexionesNOValidas"></param>
        /// <param name="iGoal"></param>
        /// <param name="iDistanciaMinima"></param>
        /// <param name="iDistanciaMaxima"></param>
        /// <param name="dibujar"></param>
        public void creaConexionesDinamicas(oNodoGlobal[,] miMatriz,
                                      ref List<oConexionGlobal> iConexionesValidas,
                                      ref List<oConexionGlobal> iConexionesNOValidas,
                                      Point3d iGoal,
                                      double iDistanciaMinima,
                                      double iDistanciaMaxima,
                                      bool dibujar)
        {
            int miFilas = miMatriz.GetLength(0);
            int miColumnas = miMatriz.GetLength(1);

            int radioMaxIndices = (int)Math.Ceiling(iDistanciaMaxima / mTamañoCelda) + 1;

            // Registro de filas que ya tienen al menos una conexión
            HashSet<int> filasConectadas = new HashSet<int>();

            // FASE 1: Búsqueda normal dentro del radio (SOLO HACIA ADELANTE)
            for (int offsetI = 0; offsetI <= miFilas; offsetI++)
            {
                for (int offsetJ = mJ; offsetJ <=mJ+ radioMaxIndices && offsetJ < miColumnas; offsetJ++)
                {
                    if (offsetI == mI && offsetJ == mJ)
                        continue;

                    int miPosIDestino = offsetI;
                    int miPosJDestino = offsetJ;

                    // RESTRICCIÓN: Solo conexiones hacia adelante (columnas mayores)
                    /*if (miPosJDestino <= mJ)
                        continue;*/

                    if (miPosIDestino < 0 || miPosIDestino >= miFilas)
                        continue;
                    if (miPosJDestino < 0 || miPosJDestino >= miColumnas)
                        continue;

                    oNodoGlobal miDestino = miMatriz[miPosIDestino, miPosJDestino];

                    if (miDestino == null || !miDestino.mIsValido)
                        continue;

                    double distanciaReal = mPuntoCentral.mNodo.DistanceTo(miDestino.mNodo);

                    if (distanciaReal >= iDistanciaMinima && distanciaReal <= iDistanciaMaxima)
                    {
                        if (!oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso(
                            new oP2d(mPuntoCentral.mNodo.X, mPuntoCentral.mNodo.Y),
                            new oP2d(miDestino.mNodo.X, miDestino.mNodo.Y)))
                        {
                            oConexionGlobal miConexion = new oConexionGlobal(mPuntoCentral, miDestino);

                            bool esValida = miConexion.validarConexion(ref iConexionesValidas, ref iConexionesNOValidas, dibujar);

                            if (esValida || miConexion.mIsValido)
                            {
                                if (miConexion.mTramo != null)
                                {
                                    miConexion.mTramo.ptoTarget = new oP2d(iGoal.X, iGoal.Y);
                                }

                                if (!mConexiones.Contains(miConexion))
                                {
                                    mConexiones.Add(miConexion);
                                    // Registrar que esta fila ya tiene conexión
                                    filasConectadas.Add(miPosIDestino);
                                }
                            }
                        }
                    }
                }
            }

            // FASE 2: Garantizar al menos una conexión con cada fila
            for (int fila = 0; fila < miFilas; fila++)
            {
                // Si esta fila ya tiene conexión, continuar
                if (filasConectadas.Contains(fila))
                    continue;

                // Buscar el nodo válido más cercano en esta fila (solo hacia adelante)
                oNodoGlobal mejorDestino = null;
                double mejorDistancia = double.MaxValue;

                // Buscar desde la siguiente columna hasta el final
                for (int col = mJ + 1; col < miColumnas; col++)
                {
                    oNodoGlobal candidato = miMatriz[fila, col];

                    if (candidato == null || !candidato.mIsValido)
                        continue;

                    double distanciaReal = mPuntoCentral.mNodo.DistanceTo(candidato.mNodo);

                    // Verificar distancia mínima
                    if (distanciaReal < iDistanciaMinima)
                        continue;

                    // Verificar zona no paso
                    if (oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso(
                        new oP2d(mPuntoCentral.mNodo.X, mPuntoCentral.mNodo.Y),
                        new oP2d(candidato.mNodo.X, candidato.mNodo.Y)))
                        continue;

                    oConexionGlobal miConexion = new oConexionGlobal(mPuntoCentral, candidato);

                    bool esValida = miConexion.validarConexion(ref iConexionesValidas, ref iConexionesNOValidas, dibujar);

                    if (esValida || miConexion.mIsValido)
                    {
                        if (miConexion.mTramo != null)
                        {
                            miConexion.mTramo.ptoTarget = new oP2d(iGoal.X, iGoal.Y);
                        }

                        if (!mConexiones.Contains(miConexion))
                        {
                            mConexiones.Add(miConexion);
                            // Registrar que esta fila ya tiene conexión
                            //filasConectadas.Add(miPosIDestino);
                            break;
                        }
                    }
                }
            }
        }





        // Nueva versión con distancia mínima y máxima
        public void creaConexionesDinamicas(oNodoGlobal[,] miMatriz,
                                     ref ConcurrentBag<oConexionGlobal> iConexionesValidas_p,
                                     ref ConcurrentBag<oConexionGlobal> iConexionesNOValidas_p,
                                     Point3d iGoal,
                                     double iDistanciaMinima,
                                     double iDistanciaMaxima,
                                     bool dibujar)
        {
            int miFilas = miMatriz.GetLength(0);
            int miColumnas = miMatriz.GetLength(1);

            int radioMaxIndices = (int)Math.Ceiling(iDistanciaMaxima / mTamañoCelda) + 1;

            for (int offsetI = -radioMaxIndices; offsetI <= radioMaxIndices; offsetI++)
            {
                for (int offsetJ = -radioMaxIndices; offsetJ <= radioMaxIndices; offsetJ++)
                {
                    // Saltar el nodo central
                    if (offsetI == 0 && offsetJ == 0)
                        continue;

                    int miPosIDestino = mI + offsetI;
                    int miPosJDestino = mJ + offsetJ;

                    if (miPosIDestino < 0 || miPosIDestino >= miFilas)
                        continue;
                    if (miPosJDestino < 0 || miPosJDestino >= miColumnas)
                        continue;

                    oNodoGlobal miDestino = miMatriz[miPosIDestino, miPosJDestino];

                    if (miDestino == null || !miDestino.mIsValido)
                        continue;

                    double distanciaReal = mPuntoCentral.mNodo.DistanceTo(miDestino.mNodo);

                    if (distanciaReal >= iDistanciaMinima && distanciaReal <= iDistanciaMaxima)
                    {
                        if (!oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPaso(
                            new oP2d(mPuntoCentral.mNodo.X, mPuntoCentral.mNodo.Y),
                            new oP2d(miDestino.mNodo.X, miDestino.mNodo.Y)))
                        {
                            oConexionGlobal miConexion = new oConexionGlobal(mPuntoCentral, miDestino);

                            // CORRECCIÓN: Verificar si es válida primero
                            bool esValida = miConexion.validarConexion(ref iConexionesValidas_p, ref iConexionesNOValidas_p, dibujar);

                            // IMPORTANTE: Agregar a mConexiones si es válida, 
                            // independientemente de si ya existía en las listas globales
                            if (esValida || miConexion.mIsValido)
                            {
                                if (miConexion.mTramo != null)
                                {
                                    miConexion.mTramo.ptoTarget = new oP2d(iGoal.X, iGoal.Y);
                                }

                                // Verificar que no se agregue duplicada a mConexiones
                                if (!mConexiones.Contains(miConexion))
                                {
                                    mConexiones.Add(miConexion);
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    internal class oEdge
    {
        private oNode _origin;
        private oNode _destination;
        private double _distance;


        public oEdge(oNode origin, oNode destination, double distance)
        {
            this._origin = origin;
            this._destination = destination;
            this._distance = distance;
        }


        public oNode Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }


        public oNode Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }


        public double Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
    }
    internal abstract class oNode
    {

        public oNode()
        {

        }

        public oNode(int iNodeGrafoId, Point2d iNodePosition)
        {
            pNodeIdGrafo = iNodeGrafoId;
            pNodePos = iNodePosition;
        }


        /// <summary>
        /// ID del NODO en el GRAFO
        /// </summary>
        public int pNodeIdGrafo { get; set; }


        /// <summary>
        /// Posicion (X,Y) del Nodo
        /// </summary>
        public Point2d pNodePos { get; set; }

    }
    internal class oNodeCad : oNode
    {

        /// <summary>
        /// Handle de la Polilinea
        /// </summary>
        public long pPolHandle { get; set; }

        /// <summary>
        /// ID del NODO en la Polilinea (Base1)
        /// </summary>
        public int pNodeIdPol { get; set; }


        public oNodeCad()
        {

        }
        public oNodeCad(int iNodeGrafoId, Point2d iNodePos, int iNodeIdPol, long iPolHandle)

            : base(iNodeGrafoId, iNodePos)

        {
            pNodeIdPol = iNodeIdPol;
            pPolHandle = iPolHandle;
        }



    }
    internal class Dijkstra
    {
        private List<oNode> _nodes;
        private List<oEdge> _edges;
        private List<oNode> _basis;
        private Dictionary<int, double> _dist;
        private Dictionary<int, oNode> _previous;
        private double _maxDistance;
        private double _minAngle;
        private Dictionary<int, double> angulos;
        private Dictionary<int, double> distancia;

        /// <summary>
        /// Constructor
        /// </summary>
        public Dijkstra(List<oEdge> edges, List<oNode> nodes, double maxDistance, double angulo)
        {

            Edges = edges;
            Nodes = nodes;
            Basis = new List<oNode>();
            Dist = new Dictionary<int, double>();
            Previous = new Dictionary<int, oNode>();
            angulos = new Dictionary<int, double>();
            distancia = new Dictionary<int, double>();
            MaxDistance = maxDistance;
            MinAngle = angulo;

            // Knoten aufnehmen
            foreach (oNode n in Nodes)
            {
                Previous.Add(n.pNodeIdGrafo, null);
                Basis.Add(n);
                Dist.Add(n.pNodeIdGrafo, double.MaxValue);
            }
        }

        /// <summary>
        /// Berechnet die kürzesten Wege vom start
        /// Knoten zu allen anderen Knoten
        /// </summary>
        public void calculateDistance(oNode start, bool respetarpuntos, double clmin, double R, bool anguloautomatico)
        {
            Dist[start.pNodeIdGrafo] = 0;

            while (Basis.Count > 0)
            {

                oNode myNodeOrigen = getNodeWithSmallestDistance();

                if (myNodeOrigen == null)
                {
                    Basis.Clear();
                }
                else
                {
                    if (myNodeOrigen.pNodeIdGrafo == 1)
                    {
                        int a = 0;
                    }
                    // Obtener el nodo previo en el camino
                    oNode nodoPrevio = Previous[myNodeOrigen.pNodeIdGrafo];

                    //Obtengo los Hijos del NodoOrigen
                    foreach (oNode myNodeHijo in getNeighbors(myNodeOrigen))
                    {
                        double myDistOrigen;
                        double myDistanciaNodoOrigen = Dist[myNodeOrigen.pNodeIdGrafo];
                        double myDistanciaNodoOrigenHijo = getDistanceBetween(myNodeOrigen, myNodeHijo);

                        double dist = myNodeOrigen.pNodePos.GetDistanceTo(myNodeHijo.pNodePos);
                        double angulo = VerificarAnguloMinimo(nodoPrevio, myNodeOrigen, myNodeHijo, respetarpuntos);
                        double angulofinal = 180;
                        if (myNodeHijo.pNodeIdGrafo == 1 && respetarpuntos)
                        {
                            angulofinal = VerificarAnguloMinimoFinal(myNodeOrigen, myNodeHijo);
                        }
                        if (nodoPrevio != null && angulo > 179)
                        {
                            double dist2 = nodoPrevio.pNodePos.GetDistanceTo(myNodeOrigen.pNodePos);
                            dist += dist2;
                        }
                        double MinAngle_final = MinAngle;
                        if (anguloautomatico)
                        {
                            if (myNodeHijo.pNodeIdGrafo == 1)
                            {
                                MinAngle_final =  GetAnguloCorrecto_final(myNodeOrigen, myNodeHijo, respetarpuntos, clmin, R);

                            }

                            MinAngle =  GetAnguloCorrecto(nodoPrevio, myNodeOrigen, myNodeHijo, respetarpuntos, clmin, R);


                        }
                        // Verificar que la distancia de la arista no supere el límite
                        if (dist >= MaxDistance)
                        {
                            // Verificar que el ángulo cumple con la restricción mínima

                            if (angulo >= MinAngle && angulofinal >= MinAngle_final)
                            {
                                myDistOrigen = myDistanciaNodoOrigen + myDistanciaNodoOrigenHijo;
                                if (myDistOrigen < Dist[myNodeHijo.pNodeIdGrafo])
                                {
                                    Dist[myNodeHijo.pNodeIdGrafo] = myDistOrigen;
                                    Previous[myNodeHijo.pNodeIdGrafo] = myNodeOrigen;
                                    /* angulos[myNodeHijo.pNodeIdGrafo] = angulo;
                                     distancia[myNodeHijo.pNodeIdGrafo] = dist;*/
                                }
                            }
                        }
                    }

                    Basis.Remove(myNodeOrigen);
                }
            }
        }

        /// <summary>
        /// Obtener Camino Start-Goal
        /// </summary>
        public List<oNode> getPathTo(oNode iNodeGoal)
        {
            List<oNode> path = new List<oNode>();
            List<double> distanciass = new List<double>();
            List<double> anguloss = new List<double>();
            path.Insert(0, iNodeGoal);
            /* distanciass.Add(distancia[iNodeGoal.pNodeIdGrafo]);
             anguloss.Add(angulos[iNodeGoal.pNodeIdGrafo]);*/
            int i = 0;

            while (Previous[iNodeGoal.pNodeIdGrafo] != null)
            {
                i++;
                try
                {
                    iNodeGoal = Previous[iNodeGoal.pNodeIdGrafo];
                    path.Insert(0, iNodeGoal);
                    /* distanciass.Add(distancia[iNodeGoal.pNodeIdGrafo]);
                     anguloss.Add(angulos[iNodeGoal.pNodeIdGrafo]);*/
                }
                catch (Exception e)
                {
                    /*distanciass.Add(-1);
                    anguloss.Add(-1);*/
                }

            }

            return path;
        }

        /// <summary>
        /// Obtener Nodo Menor Coste
        /// </summary>
        private oNode getNodeWithSmallestDistance()
        {
            double distance = double.MaxValue;
            oNode smallest = null;

            foreach (oNode n in Basis)
            {
                if (Dist[n.pNodeIdGrafo] < distance)
                {
                    distance = Dist[n.pNodeIdGrafo];
                    smallest = n;
                }
            }

            return smallest;
        }

        /// <summary>
        /// Liefert alle Nachbarn von n die noch in der Basis sind
        /// </summary>
        /// <param name="n">Knoten</param>
        /// <returns></returns>
        private List<oNode> getNeighbors(oNode n)
        {
            List<oNode> neighbors = new List<oNode>();

            foreach (oEdge e in Edges)
            {
                if (e.Origin.pNodeIdGrafo == n.pNodeIdGrafo && Basis.Contains(n))
                {
                    neighbors.Add(e.Destination);
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Liefert die Distanz zwischen zwei Knoten
        /// </summary>
        /// <param name="o">Startknoten</param>
        /// <param name="d">Endknoten</param>
        /// <returns></returns>
        private double getDistanceBetween(oNode o, oNode d)
        {
            foreach (oEdge e in Edges)
            {

                if (e.Origin.pNodeIdGrafo == o.pNodeIdGrafo && e.Destination.pNodeIdGrafo == d.pNodeIdGrafo)
                {
                    return e.Distance;
                }

            }

            throw new Exception(string.Format("No se encuentra la distancia entre {0} - {1}", o.pNodeIdGrafo.ToString(), d.pNodeIdGrafo.ToString()));
        }

        /// <summary>
        /// Listado de Nodos
        /// </summary>
        public List<oNode> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>
        /// Listado de Diagonales
        /// </summary>
        public List<oEdge> Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }

        /// <summary>
        /// Listado Inicial de Nodos
        /// </summary>
        public List<oNode> Basis
        {
            get { return _basis; }
            set { _basis = value; }
        }

        /// <summary>
        /// Listado ID Nodo, Distancia Minimma
        /// </summary>
        public Dictionary<int, double> Dist
        {
            get { return _dist; }
            set { _dist = value; }
        }

        /// <summary>
        /// Diccionario ID Nodo, Nodo Previo
        /// </summary>
        public Dictionary<int, oNode> Previous
        {
            get { return _previous; }
            set { _previous = value; }
        }
        public double MaxDistance
        {
            get { return _maxDistance; }
            set { _maxDistance = value; }
        }
        /// <summary>
        /// Ángulo mínimo en grados permitido entre dos conexiones consecutivas
        /// </summary>
        public double MinAngle
        {
            get { return _minAngle; }
            set { _minAngle = value; }
        }
        /// <summary>
        /// Verifica si el ángulo entre tres nodos cumple con la restricción mínima
        /// </summary>
        /// <param name="nodoPrevio">Nodo anterior</param>
        /// <param name="nodoActual">Nodo actual</param>
        /// <param name="nodoSiguiente">Nodo siguiente candidato</param>
        /// <returns>True si el ángulo es mayor o igual al mínimo permitido</returns>
        private double VerificarAnguloMinimo(oNode nodoPrevio, oNode nodoActual, oNode nodoSiguiente, bool respetarpuntos)
        {
            // Si no hay nodo previo (es el primer nodo), no hay restricción de ángulo
            if (nodoPrevio == null)
            {
                if (Math.Round(oSingletonProyecto.getInstance.ptoSalida.X, 2) != Math.Round(nodoActual.pNodePos.X, 2) &&
                    Math.Round(oSingletonProyecto.getInstance.ptoSalida.Y, 2) != Math.Round(nodoActual.pNodePos.Y, 2) && respetarpuntos)
                {
                    return CalculateAngle(new Point2d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y), nodoActual.pNodePos, nodoSiguiente.pNodePos);
                }
                else
                {
                    return 180;
                }
            }

            //return 180;

            // Calcular el ángulo entre los tres puntos
            double angulo = CalculateAngle(nodoPrevio.pNodePos, nodoActual.pNodePos, nodoSiguiente.pNodePos);

            // Verificar si el ángulo cumple con el mínimo
            return angulo;
        }
        private double VerificarAnguloMinimoFinal(oNode nodoActual, oNode nodoSiguiente)
        {
            if (Math.Round(oSingletonProyecto.getInstance.ptoLlegada.X, 2) != Math.Round(nodoSiguiente.pNodePos.X, 2) &&
                     Math.Round(oSingletonProyecto.getInstance.ptoLlegada.Y, 2) != Math.Round(nodoSiguiente.pNodePos.Y, 2))
            {
                return CalculateAngle(nodoActual.pNodePos, nodoSiguiente.pNodePos, new Point2d(oSingletonProyecto.getInstance.ptoLlegada.X, oSingletonProyecto.getInstance.ptoLlegada.Y));
            }
            else
            {
                return 180;
            }
        }

        private double GetAnguloCorrecto(oNode previo, oNode actual, oNode siguiente, bool respetarpuntos, double clmin, double R)
        {
            double A = 0;
            double L = 0;

            if (previo == null && respetarpuntos)
            {
                if (Math.Round(oSingletonProyecto.getInstance.ptoSalida.X, 2) != Math.Round(actual.pNodePos.X, 2) &&
                     Math.Round(oSingletonProyecto.getInstance.ptoSalida.Y, 2) != Math.Round(actual.pNodePos.Y, 2))
                {
                    A = Math.Min(new Point2d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y).GetDistanceTo(actual.pNodePos), actual.pNodePos.GetDistanceTo(siguiente.pNodePos));
                    L = A / 2 - 2 * clmin;

                    return 2 * (90 - Math.Atan(L / R)*180/Math.PI);
                }
                else
                {
                    return 0;
                }
            }
            if (previo == null && !respetarpuntos)
            {
                return 0;
            }

            A = Math.Min(previo.pNodePos.GetDistanceTo(actual.pNodePos), actual.pNodePos.GetDistanceTo(siguiente.pNodePos));
            L = A / 2 - 2 * clmin;

            return 2 * (90 - Math.Atan(L / R) * 180 / Math.PI);
        }
        private double GetAnguloCorrecto_final(oNode actual, oNode siguiente, bool respetarpuntos, double clmin, double R)
        {
            double A = 0;
            double L = 0;

            if (Math.Round(oSingletonProyecto.getInstance.ptoSalida.X, 2) != Math.Round(siguiente.pNodePos.X, 2) &&
                     Math.Round(oSingletonProyecto.getInstance.ptoSalida.Y, 2) != Math.Round(siguiente.pNodePos.Y, 2))
            {
                A = Math.Min(new Point2d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y).GetDistanceTo(siguiente.pNodePos), actual.pNodePos.GetDistanceTo(siguiente.pNodePos));
                L = A / 2 - 2 * clmin;

                return 2 * (90 - Math.Atan(L / R) * 180 / Math.PI);
            }

            return 0;
        }


        /// <summary>
        /// Calcula el ángulo en grados entre dos vectores formados por 3 puntos
        /// </summary>
        /// <param name="p1">Punto anterior</param>
        /// <param name="p2">Punto actual (vértice del ángulo)</param>
        /// <param name="p3">Punto siguiente</param>
        /// <returns>Ángulo en grados (0-180)</returns>
        private double CalculateAngle_old(Point2d p1, Point2d p2, Point2d p3)
        {
            // Vector del punto anterior al actual
            double v1x = p2.X - p1.X;
            double v1y = p2.Y - p1.Y;

            // Vector del punto actual al siguiente
            double v2x = p3.X - p2.X;
            double v2y = p3.Y - p2.Y;

            // Calcular las magnitudes de los vectores
            double mag1 = Math.Sqrt(v1x * v1x + v1y * v1y);
            double mag2 = Math.Sqrt(v2x * v2x + v2y * v2y);

            // Evitar división por cero
            if (mag1 == 0 || mag2 == 0)
                return 180;

            // Producto punto
            double dotProduct = v1x * v2x + v1y * v2y;

            // Calcular el ángulo en radianes y convertir a grados
            double angleRad = Math.Acos(dotProduct / (mag1 * mag2));
            double angleDeg = angleRad * (180.0 / Math.PI);

            return angleDeg;
        }
        /// <summary>
        /// Calcula el ángulo de giro (deflexión) en grados entre dos segmentos formados por 3 puntos
        /// </summary>
        /// <param name="p1">Punto anterior</param>
        /// <param name="p2">Punto actual (vértice del ángulo)</param>
        /// <param name="p3">Punto siguiente</param>
        /// <returns>Ángulo de giro en grados (0-180)</returns>
        private double CalculateAngle(Point2d p1, Point2d p2, Point2d p3)
        {
            // Vector del punto anterior al actual
            double v1x = p2.X - p1.X;
            double v1y = p2.Y - p1.Y;

            // Vector del punto actual al siguiente
            double v2x = p3.X - p2.X;
            double v2y = p3.Y - p2.Y;

            // Calcular las magnitudes de los vectores
            double mag1 = Math.Sqrt(v1x * v1x + v1y * v1y);
            double mag2 = Math.Sqrt(v2x * v2x + v2y * v2y);

            // Evitar división por cero
            if (mag1 == 0 || mag2 == 0)
                return 180;

            // Normalizar los vectores
            v1x /= mag1;
            v1y /= mag1;
            v2x /= mag2;
            v2y /= mag2;

            // Producto punto para obtener el coseno del ángulo
            double dotProduct = v1x * v2x + v1y * v2y;

            // Producto cruz (en 2D, solo la componente Z) para determinar la dirección
            double crossProduct = v1x * v2y - v1y * v2x;

            // Calcular el ángulo usando atan2 para obtener el ángulo con signo
            double angleRad = Math.Atan2(Math.Abs(crossProduct), dotProduct);

            // Convertir a grados
            double angleDeg = angleRad * (180.0 / Math.PI);

            // El ángulo de deflexión es 180 - ángulo interior
            // Si el ángulo interior es pequeño, el giro es brusco (deflexión grande)
            // Si el ángulo interior es cercano a 180°, el giro es suave (deflexión pequeña)
            double deflectionAngle = 180.0 - angleDeg;

            return deflectionAngle;
        }
    }
    #endregion
}
