using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.EjeTrazadoTadil
{


    //CAD
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.Runtime;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Colors;
    //using cv = Autodesk.Civil.DatabaseServices;


    using tadLayLogica.datos.proyecto;
    using engCadNet;
    using tadLayShare;
    
   
    //EJE TADIL
    using EjeDeTrazado.puntosDelEje;
    using EjeDeTrazado.componentes;
    using EjeDeTrazado;
    using tadLayShare.puntos;
  
    

    /// <summary>
    /// FACTORY EJE TRAZADO TADIL
    /// </summary>
    public class oFactoryEjeTrazadoTadil
    {
        public static EjeTrazado getEjeTrazadoTadil(Guid iIdSolucion, double iPeralteCurvaPC, double iBombeoRectaPC)
        {


            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
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
                Polyline miEjeBasico2d = miSolucion.ejeBasico2D;

                //Listado Puntos Polilinea
                List<Point3d> miListadoPuntosCad = oLw.getLstPto(miEjeBasico2d);


                //Listado de Puntos EjeTadil
                miLstPuntoTadil = miListadoPuntosCad.ConvertAll(p => new tadLayShare.puntos.Punto3d(p.X, p.Y, p.Z));

                #endregion
                #region "Grupo Carretera"

                if (miSolucion.roadDesign.grupo == eRoadGrupo.Grupo1)
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
                }


                #endregion
                #region "PreferenciasCurvas"

                if (miSolucion.roadDesign.preferencias == eRoadPreferencias.curvas)
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
                }

                #endregion
                #region "VelocidadRadioProyecto"

                miVelocidadProyecto = miSolucion.roadDesign.Vp;
                miRadioProyecto = miSolucion.roadDesign.Rp;

                #endregion
                #region "Aij Constante"

                miIsAijConstante = miSolucion.roadDesign.IsAijK;

                #endregion


                EjeTrazado miEjeTrazadoTadil = new EjeTrazado(miLstPuntoTadil,
                                                               miGrupo.Value,
                                                               miIsPreferenciasCurvas.Value,
                                                               miRadioProyecto.Value,
                                                               miVelocidadProyecto.Value,
                                                               miPeralteCurva.Value,
                                                               miPeralteRecta.Value,
                                                               miIsAijConstante.Value);

                return miEjeTrazadoTadil;

            }

        }


        public static EjeTrazado getEjeTrazadoTadilFerrocarril(Guid iIdSolucion, double iPeralteCurvaPC, List<Componente> iListComponentes, List<Vertice> iListVertices)
        {


            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {
                double? miPeralteCurva = iPeralteCurvaPC;
                double? miPeralteRecta = 0;

                EjeTrazado miEjeTrazadoTadil = new EjeTrazado(iListVertices, iListComponentes, (double)miPeralteCurva, (double)miPeralteRecta);

                return miEjeTrazadoTadil;

            }

        }

    }
  
}
