using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using engCadNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EjeDeTrazado;
using EjeDeTrazado.puntosDelEje;
using Autodesk.AutoCAD.Geometry;
using tadLayShare.puntos;

namespace Logica.Comandos
{
    public class oEjeTrazado
    {

        public static EjeTrazado getEjeTrazadoTadil(List<Point3d> lista, double iPeralteCurvaPC, double iBombeoRectaPC,double A)
        {




                //Variables para Construir el Objeto
                List<tadLayShare.puntos.Punto3d> miLstPuntoTadil;
                int? miGrupo = null;
                bool? miIsPreferenciasCurvas = null;
                double? miVelocidadProyecto = null;
                double? miRadioProyecto = null;
                double? miPeralteCurva = iPeralteCurvaPC;
                double? miPeralteRecta = iBombeoRectaPC;
                bool? miIsAijConstante = null;

            #region "Listado de Puntos"

            //Eje Básico 2D
            
                //Polyline miEjeBasico2d = miSolucion.ejeBasico2D;

                //Listado Puntos Polilinea
                //List<Point3d> miListadoPuntosCad = oLw.getLstPto(miEjeBasico2d);
                List<Point3d> miListadoPuntosCad = lista;

                //Listado de Puntos EjeTadil
            miLstPuntoTadil = miListadoPuntosCad.ConvertAll(p => new tadLayShare.puntos.Punto3d(p.X, p.Y, p.Z));

            #endregion
            #region "Grupo Carretera"   
            miGrupo = 1;
            /*if (miSolucion.roadDesign.grupo == eRoadGrupo.Grupo1)
                {
                    miGrupo = 1;
                }
                else if (miSolucion.roadDesign.grupo == eRoadGrupo.Grupo2)
                {
                    miGrupo = 2;
                }
                else
                {
                    throw new oExEnumNotImplemented(miSolucion.roadDesign.grupo.ToString());
                }*/


            #endregion
            #region "PreferenciasCurvas"
            miIsPreferenciasCurvas = true;
            /*if (miSolucion.roadDesign.preferencias == eRoadPreferencias.curvas)
                {
                    miIsPreferenciasCurvas = true;
                }
                else if (miSolucion.roadDesign.preferencias == eRoadPreferencias.rectas)
                {
                    miIsPreferenciasCurvas = false;
                }
                else
                {
                    throw new oExEnumNotImplemented(miSolucion.roadDesign.preferencias.ToString());
                }*/

            #endregion
            #region "VelocidadRadioProyecto"
            miVelocidadProyecto = 80;
            miRadioProyecto = 1500;
            //miRadioProyecto = A;
            /*miVelocidadProyecto = miSolucion.roadDesign.Vp;
                miRadioProyecto = miSolucion.roadDesign.Rp;*/

            #endregion
            #region "Aij Constante"
            miIsAijConstante = true;
            //miIsAijConstante = miSolucion.roadDesign.IsAijK;

            #endregion


            EjeTrazado miEjeTrazadoTadil = new  EjeTrazado(miLstPuntoTadil,
                                                               miGrupo.Value,
                                                               miIsPreferenciasCurvas.Value,
                                                               miRadioProyecto.Value,
                                                               miVelocidadProyecto.Value,
                                                               miPeralteCurva.Value,
                                                               miPeralteRecta.Value,
                                                               miIsAijConstante.Value);

                return miEjeTrazadoTadil;

        }


        public static void create(int iEstudioTipo,List<Point3d> lista,double A)//1-->estudio informativo 2-->estudio previo
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                //using (oSolucion miSolucion = new oSolucion(iIdSolucion))
                //{


                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        //Creo la medicion
              /*          Stopwatch miMedicion = new Stopwatch();
                        miMedicion.Start();

                        BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                        //CREO EJE TRAZADO TADIL ANGELES
                        double miBombeo = 2;
                        /*if (iEstudioTipo == 1)
                        {
                            oSeccionRoadCompletaSinGis miSeccionCompletaSinGis = oSingletonProyecto.getInstance.seccionCalzadaCompleta;
                            miBombeo = (double)miSeccionCompletaSinGis.BombeoPC;
                        }*/


              /*          EjeTrazado miEjeTrazadoTadil = getEjeTrazadoTadil(lista, 8, miBombeo,A);


                        MemoryStream miEjeMem = miEjeTrazadoTadil.guardarEjeTrazado();
                        Polyline miEje = new Polyline();
                        int index = 0;
                        foreach (var componente in miEjeTrazadoTadil.getComponentes)
                        {
                            foreach (var componentPoint in componente.getComponentPoints())
                            {
                                miEje.AddVertexAt(index, new Point2d(componentPoint[0], componentPoint[1]), 0, 0, 0);
                                index++;
                            }
                        }
                    engCadNet.oLayer.addLayer("Eje Trazado", 5, false);
                    miEje.Layer = "Eje Trazado";

                        btr.AppendEntity(miEje);
                        tr.AddNewlyCreatedDBObject(miEje, true);

                        //oXdata.setXdata(miEje.ObjectId, "tadilEje", miSolucion.idSolucion.ToString());
                        ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEje, tr, miEjeMem, miEjeTrazadoTadil.GetType().FullName);


                        //Añado el Eje de Trazado al fichero.
                        //oDalTbSolucion.addEjeTrazado(miSolucion.idSolucion, miEje.Handle.ToString(), miEjeTrazadoTadil.Length / 1000);

                        ////EXPORTAR
                        //miEjeTrazadoTadil.exportarEjeTrazado(miEjeTrazadoTadil.guardarEjeTrazado());


                        miMedicion.Stop();


                        oCadManager.thisEditor.UpdateScreen();

                        //Info UI
                        //oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);

                        tr.Commit();*/
                    }

                //}
            }

        }
    }
}
