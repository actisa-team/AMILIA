using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Comandos
{
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.ApplicationServices;


    using engCadNet;
    using tadLayLan;
    using tadLayLogica.datos.proyecto;

    using EjeDeTrazado.puntosDelEje;
    using PerfilLongitudinal;
    using EjeLongitudinalTadil;
    using tadLayLogica.estudioTipo;
    

    using System.Diagnostics;
    using System.IO;
    using tadLayLogica.logica.Entidades;
    using tadLayShare;
    using tadLayLogica.zonaGis;
    using tadLayShare.puntos;
    using tadLayData;

    public class oComandoPerfilLongitudinalTadil
    {

        static Polyline mEjePol = null;
        static List<Point3d> misVerticesOrig = null;

        public static void create(eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                try{

                Point3d miPtoInsert = (Point3d) engCadNet.oTools.getPointCad(strUserCad.uiSelectPtoInsertPerfilLongitudinal);

                    //Creo la medicion
                    Stopwatch miMedicion = new Stopwatch();
                    miMedicion.Start();







                    using (oSolucion miSolucion = new oSolucion(iIdSolucion))
                    {
                        Alzado miAlzado;
                        var miEjeTrazado = GetAlzadoGalibo(iEstudioTipo, iIdSolucion, out miAlzado);


                        Guitarra miGuitarra = tadLayLogica.EjeLongitudinalTadil.oFactoryPerfilLongitudinalTadil.getGuitarra(miPtoInsert, miAlzado, 100, 10);


                        PerfilLongitudinalDraw miPerfilDraw = new PerfilLongitudinalDraw(miGuitarra,
                                                                                         miAlzado,
                                                                                         miEjeTrazado,
                                                                                         oSingletonTerreno.getInstance.getZFromXY,
                                                                                         oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);



                        using (Transaction tr = oCadManager.StartTransaction())
                        {

                            //Gestion de Capas
                            oTadilLayerPerfilLongitudinalTadil miLayerPerfilRotular = new oTadilLayerPerfilLongitudinalTadil(iIdSolucion);
                            miLayerPerfilRotular.deleteItems();
                            //DRAW
                            miPerfilDraw.drawGuitarra(miLayerPerfilRotular.name);
                            miPerfilDraw.drawTerreno(miLayerPerfilRotular.name);
                            miPerfilDraw.drawEje(miLayerPerfilRotular.name);
                            miPerfilDraw.drawEjeLongitudinal(miLayerPerfilRotular.name);


                            Polyline miEjeAlzado = miPerfilDraw.getPolylineEjeAlzado();
                            MemoryStream miEjeMem = miAlzado.guardarAlzado();

                            oXdata.setXdata(miEjeAlzado.ObjectId, "tadilEjeAlzado", miSolucion.idSolucion.ToString());
                            ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEjeAlzado, tr, miEjeMem, miAlzado.GetType().FullName);

                            oDalTbSolucion.addPerfilLongitudinal(iIdSolucion, miEjeAlzado.Handle.ToString());


                            if (iEstudioTipo == eEstudioTipo.ESTINF)
                            {
                                miPerfilDraw.drawPeralte(miLayerPerfilRotular.name);


                                #region "Calcular Estructuras"
                                //Obtengo el oEstudioCarretera
                                oEstudioCarretera miEstudioCarretera = oFactoryEstudios.getEstudioCarretera(iEstudioTipo, iIdSolucion);

                                Polyline miLwEjeTrazado = miSolucion.ejeTrazado;
                                Polyline miLwEjePerfilRasante = miSolucion.ejePerfilRasante;
                                double miSeccionIntervalo = oSingletonProyecto.getInstance.seccionIntervalo;

                                oPerfilSeccionesInfo miPerfilSeccionesInfo = new oPerfilSeccionesInfo(miLwEjeTrazado, miLwEjePerfilRasante, miEstudioCarretera,
                                                                                         oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);

                                List<oEstructura> miLstPerfilEstructurasInfo = miPerfilSeccionesInfo.getPerfilSeccionesInfo(miSeccionIntervalo);

                                #endregion

                                miPerfilDraw.drawEstructuras(miLayerPerfilRotular.name, miLstPerfilEstructurasInfo);
                            }


                            //Para medicion
                            miMedicion.Stop();

                            //Info UI
                            oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);

                            tr.Commit();
                        }

                    }
                }
                catch(oExSelectPointNull)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }
               
            }
        }
        public static void create_Amilia(eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                try
                {
                    
                    Point3d miPtoInsert = (Point3d)engCadNet.oTools.getPointCad(strUserCad.uiSelectPtoInsertPerfilLongitudinal);

                    //Creo la medicion
                    Stopwatch miMedicion = new Stopwatch();
                    miMedicion.Start();

                    using (oSolucion miSolucion = new oSolucion(iIdSolucion))
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
                                miEjeTrazado = EjeTrazado.recuperaEjeTrazado(ms);
                            }


                            Alzado miAlzado;
                            byte[] datosRecuperados_alzado = miRow.Alzado_Amilia;
                            using (MemoryStream ms = new MemoryStream(datosRecuperados_alzado))
                            {
                                // 3. Usamos tu método estático para reconstruir la clase
                                miAlzado = Alzado.recuperaAlzado(ms);
                            }

                            //var miEjeTrazado = GetAlzadoGalibo(iEstudioTipo, iIdSolucion, out miAlzado);


                            Guitarra miGuitarra = tadLayLogica.EjeLongitudinalTadil.oFactoryPerfilLongitudinalTadil.getGuitarra(miPtoInsert, miAlzado, 100, 10);


                            PerfilLongitudinalDraw miPerfilDraw = new PerfilLongitudinalDraw(miGuitarra,
                                                                                             miAlzado,
                                                                                             miEjeTrazado,
                                                                                             oSingletonTerreno.getInstance.getZFromXY,
                                                                                             oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);



                            using (Transaction tr = oCadManager.StartTransaction())
                            {

                                //Gestion de Capas
                                oTadilLayerPerfilLongitudinalTadil miLayerPerfilRotular = new oTadilLayerPerfilLongitudinalTadil(iIdSolucion);
                                miLayerPerfilRotular.deleteItems();
                                //DRAW
                                miPerfilDraw.drawGuitarra(miLayerPerfilRotular.name);
                                miPerfilDraw.drawTerreno(miLayerPerfilRotular.name);
                                miPerfilDraw.drawEje(miLayerPerfilRotular.name);
                                miPerfilDraw.drawEjeLongitudinal(miLayerPerfilRotular.name);


                                Polyline miEjeAlzado = miPerfilDraw.getPolylineEjeAlzado();
                                MemoryStream miEjeMem = miAlzado.guardarAlzado();

                                /*oXdata.setXdata(miEjeAlzado.ObjectId, "tadilEjeAlzado", miSolucion.idSolucion.ToString());
                                ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEjeAlzado, tr, miEjeMem, miAlzado.GetType().FullName);
                                */
                                oDalTbSolucion.addPerfilLongitudinal(iIdSolucion, miEjeAlzado.Handle.ToString());


                                if (iEstudioTipo == eEstudioTipo.ESTINF)
                                {
                                    miPerfilDraw.drawPeralte(miLayerPerfilRotular.name);


                                    #region "Calcular Estructuras"
                                    //Obtengo el oEstudioCarretera
                                    oEstudioCarretera miEstudioCarretera = oFactoryEstudios.getEstudioCarretera(iEstudioTipo, iIdSolucion);

                                    double miSeccionIntervalo = oSingletonProyecto.getInstance.seccionIntervalo;

                                    oPerfilSeccionesInfo miPerfilSeccionesInfo = new oPerfilSeccionesInfo(miEjeTrazado, miAlzado, miEstudioCarretera,
                                                                                             oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);

                                    List<oEstructura> miLstPerfilEstructurasInfo = miPerfilSeccionesInfo.getPerfilSeccionesInfo(miSeccionIntervalo);


                                    #endregion

                                    miPerfilDraw.drawEstructuras(miLayerPerfilRotular.name, miLstPerfilEstructurasInfo);
                                }


                                //Para medicion
                                miMedicion.Stop();

                                //Info UI
                                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);

                                tr.Commit();
                            }
                        }
                    }
                    
                }
                catch (oExSelectPointNull)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }

            }
        }

        public static EjeTrazado GetAlzadoGalibo(eEstudioTipo iEstudioTipo, Guid iIdSolucion, out Alzado miAlzado)
        {

            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {
                Polyline iEjeTrazado = miSolucion.ejeTrazado;
                Xrecord miXrecord = engCadNet.oXrecord.getXrecord(iEjeTrazado.ObjectId, "info");
                EjeTrazado miEjeTrazado =
                    EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));
                oP3d ptoLlegada = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoLlegada);
                miAlzado = tadLayLogica.EjeLongitudinalTadil.oFactoryPerfilLongitudinalTadil.getAlzado(iIdSolucion,
                    miEjeTrazado,
                    ptoLlegada);
                miAlzado = modAlzadoImposicionGalibo(miAlzado, miEjeTrazado, iEstudioTipo, iIdSolucion);
                return miEjeTrazado;
            }
        }

        private static Alzado modAlzadoImposicionGalibo(Alzado iAlzado, EjeTrazado iEjeTrazado, eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {
            Alzado miNuevoAlzado;
            oSolucion miSolucion = new oSolucion(iIdSolucion);
            switch (iEstudioTipo)
            {
                case eEstudioTipo.ESTPRE:
                    miNuevoAlzado = iAlzado;
                    return miNuevoAlzado;

                case eEstudioTipo.ESTINF:

                    oEstudioInformativoCarretera miEstudioDatos = (oEstudioInformativoCarretera)oFactoryEstudios.getEstudioCarretera(iEstudioTipo, iIdSolucion);


                    List<oZonaGis> misDPH = miEstudioDatos.lstZonasDPHsinEJE;
                    List<oZonaGis> misINF = miEstudioDatos.lstZonasINFsinEJE;
                    Polyline3d miEjeBasico = miSolucion.ejeBasico3D;
                    List<Point3d> misVertices = oLw.getLstPtoFromLw3d(miEjeBasico);
                    miNuevoAlzado = iAlzado;


                    oP3d ptoLlegada = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoLlegada);
                    oP3d ptoSalida = oDalTbPtoIniFin.getPtoSalidaLlegada(ePtoSalidaLlegada.puntoSalida);

                    bool miPtoIniMod = false;
                    bool miPtoFinMod = false;

                    List<Point3d> miNuevoEjeBasicoDPH = modificarEjeBasicoCheckIntersec(miEstudioDatos, miSolucion, iAlzado, misDPH, misVertices, true, new List<oZonaGis>(), ref miPtoIniMod, ref miPtoFinMod);
                    miNuevoAlzado = oFactoryPerfilLongitudinalTadil.getAlzadoFromListPoint3d(iIdSolucion, iEjeTrazado, miNuevoEjeBasicoDPH, ptoLlegada, ptoSalida, miPtoIniMod, miPtoFinMod);


                  List<Point3d> miNuevoEjeBasicoINF = modificarEjeBasicoCheckIntersec(miEstudioDatos, miSolucion, miNuevoAlzado, misINF, miNuevoEjeBasicoDPH, false, misDPH, ref miPtoIniMod, ref miPtoFinMod);
                  miNuevoAlzado = oFactoryPerfilLongitudinalTadil.getAlzadoFromListPoint3d(iIdSolucion, iEjeTrazado, miNuevoEjeBasicoINF, ptoLlegada, ptoSalida, miPtoIniMod, miPtoFinMod);

                    return miNuevoAlzado;



                default:

                    throw new oExEnumNotImplemented(iEstudioTipo.ToString());


            }

        }

        private static List<Point3d> modificarEjeBasicoCheckIntersec(oEstudioInformativoCarretera iEstudioDatos, oSolucion iSolucion, Alzado iAlzado, List<oZonaGis> iZonasCruces, List<Point3d> iVertices, bool isDPH, List<oZonaGis> iZonasDPH, ref bool iPtoIniMod, ref bool iPtoFinMod)
        {
            mEjePol = iSolucion.ejeTrazado;

            oSingletonTerreno miTerreno = oSingletonTerreno.getInstance;
            bool isArriba = true;

            misVerticesOrig = new List<Point3d>();
            foreach (Point3d miPunto in iVertices)
            {
                Point3d miPuntoOrig = new Point3d(miPunto.X, miPunto.Y, miPunto.Z);
                misVerticesOrig.Add(miPuntoOrig);
            }


            List<oZonaGis> misCrucesconGalibo = new List<oZonaGis>();

            foreach (oZonaGis miZona in iZonasCruces)
            {
                Polyline miZonaPoly = miZona.lwZona;
                oZonaCruces miZonaCruce = (oZonaCruces)miZona;

                if (miZonaCruce.galibo != 0)
                {
                    misCrucesconGalibo.Add(miZonaCruce);
                }
            }


            foreach (oZonaGis miZona in misCrucesconGalibo)
            {
                Polyline miZonaPoly = miZona.lwZona;
                oZonaCruces miZonaCruce = (oZonaCruces)miZona;

                Point3dCollection miColPtoInter = new Point3dCollection();
                List<List<Point3d>> miColPtoInterES = new List<List<Point3d>>();
                miColPtoInterES.Add(miColPtoInter.convertToList());

                miZonaPoly.IntersectWith(mEjePol, Intersect.OnBothOperands, miColPtoInter, IntPtr.Zero, IntPtr.Zero);
                List<List<Point3d>> miColPtoInterCotas = new List<List<Point3d>>();

                miColPtoInterES = OrdenarTramosPorPks(miColPtoInter, mEjePol, miZonaCruce, iVertices);

                foreach (List<Point3d> miTramo in miColPtoInterES)
                {

                    bool encimaRasante = isRasanteArriba(miTramo, iAlzado, miTerreno);

                    bool dentro = false;
                    if (!isDPH)
                    {
                        dentro = isTramoDentroDPH(iZonasDPH, miTramo);
                    }

                    List<Point3d> miNewTramo = new List<Point3d>();
                    foreach (Point3d miPuntoIntersec in miTramo)
                    {
                        Point3d miPuntoEnEje = mEjePol.getPointMasCercano(miPuntoIntersec);
                        //ver que cota de rasante tiene
                        double? miPk = mEjePol.GetDistAtPoint(miPuntoEnEje);// oLw.getPkAtPoint(miPuntoEnEje, mEjePol);

                        //Calculo la cota de la rasante en la interseccion con el DPH
                        double miCotaRasInters = iAlzado.getCotaRasante((double)miPk);

                   

                        if (isDPH || dentro || encimaRasante)
                        {
                            if (miPk != null)
                            {
                                imponerGaliboPorArriba(iVertices, miZonaCruce, miPuntoEnEje, ref miCotaRasInters, (double)miPk, miTerreno);

                                if (iVertices[0].Z != misVerticesOrig[0].Z) iPtoIniMod = true;
                                if (iVertices[iVertices.Count - 1].Z != misVerticesOrig[iVertices.Count - 1].Z) iPtoFinMod = true;

                            }
                        }
                        //PARA INFRAESTRUCTURAS!! :)
                        else
                        {
                            if (miPk != null)
                            {

                                imponerGaliboPorAbajo(iVertices, miZonaCruce, miPuntoEnEje, ref miCotaRasInters, (double)miPk, miTerreno);
                                if (iVertices[0].Z != misVerticesOrig[0].Z) iPtoIniMod = true;
                                if (iVertices[iVertices.Count - 1].Z != misVerticesOrig[iVertices.Count - 1].Z) iPtoFinMod = true;
                                isArriba = false;

                            }

                        }


                        miNewTramo.Add(new Point3d(miPuntoEnEje.X, miPuntoEnEje.Y, miCotaRasInters));

                    }
                    //COMPROBAR LOS VERTICES QUE ESTAN DENTRO DE LA ZONA DPH
                    //miColPtoInter.. ya tiene que tener sus cotas correctas! :)
                    if (miNewTramo.Count == 2)
                    {
                        iVertices = checkVertexInsideZonaGalibo(iVertices, (oZonaCruces)miZona, miNewTramo, miTerreno, mEjePol, ref iPtoIniMod, ref iPtoFinMod, isArriba);
                        if (iVertices[0].Z != misVerticesOrig[0].Z) iPtoIniMod = true;
                        if (iVertices[iVertices.Count - 1].Z != misVerticesOrig[iVertices.Count - 1].Z) iPtoFinMod = true;
                    }
                    isArriba = true;
                }
            }



            return iVertices;
        }



        private static void imponerGaliboPorArriba(List<Point3d> iVertices, oZonaCruces miZonaCruce, Point3d miPuntoIntersec, ref double miCotaRasInters, double miPk, oSingletonTerreno miTerreno)
        {
            //Calculo la cota del terreno en el punto de interseccion
            Point3d miPuntoEnElEje = mEjePol.getPointMasCercano(miPuntoIntersec);
            double miCotaTerrenoIntersec = (double)miTerreno.getZFromXY(miPuntoIntersec.X, miPuntoIntersec.Y);

            //Se la sumo al gálibo y esta será la menor cota permitida a la rasante dentro de DPH
            double miRestriccionGalibo = miZonaCruce.galibo + miCotaTerrenoIntersec;


            if (miRestriccionGalibo > miCotaRasInters)
            {
                //buscar los vertices que componen el tramo al que pertenece el punto de interseccion!

                int index = 0;
                Point3d miPuntoEnElEjeVertice = mEjePol.getPointMasCercano(iVertices[index]);
                double miPkVertice = mEjePol.GetDistAtPoint(miPuntoEnElEjeVertice);// (double)oLw.getPkAtPoint(miPuntoEnElEjeVertice, mEjePol);
                while (miPk > miPkVertice)
                {
                    index++;
                    Point3d miPuntoEnElEjeV = mEjePol.getPointMasCercano(iVertices[index]);
                    miPkVertice = mEjePol.GetDistAtPoint(miPuntoEnElEjeV);
                }

                Point3d miPuntoTramoEntrada = iVertices[index - 1];
                Point3d miPuntoTramoSalida = iVertices[index];

                double disPE = miPuntoTramoEntrada.DistanceTo(miPuntoIntersec);
                double disPS = miPuntoTramoSalida.DistanceTo(miPuntoIntersec);

                Point3d miPuntoTramoEntradaAux = new Point3d(miPuntoTramoEntrada.X, 0, 0);
                Point3d miPuntoTramoSalidaAux = new Point3d(miPuntoTramoSalida.X, 0, 0);
                Point3d miPuntoIntersecAux = new Point3d(miPuntoIntersec.X, 0, 0);

                Point3d miPuntoEnElEjeE = mEjePol.getPointMasCercano(miPuntoTramoEntrada);
                Point3d miPuntoEnElEjeS = mEjePol.getPointMasCercano(miPuntoTramoSalida);

                double? miPkE = mEjePol.GetDistAtPoint(miPuntoEnElEjeE);// oLw.getPkAtPoint(miPuntoEnElEjeE, mEjePol);
                double? miPkS = mEjePol.GetDistAtPoint(miPuntoEnElEjeS);// oLw.getPkAtPoint(miPuntoEnElEjeS, mEjePol);
                if (miPkE != null && miPkS != null)
                {
                    disPE = (double)miPk - (double)miPkE;
                    disPS = (double)miPkS - (double)miPk;
                }


                double miL = disPE + disPS;

                double miHmin = miRestriccionGalibo - miCotaRasInters;
                double miHj = (miHmin * miL) / (((miL * disPS) / disPE) + disPE - disPS);
                double miHjAnt = miHj * (disPS / disPE);

                if (misVerticesOrig[index - 1].Z + miHjAnt > iVertices[index - 1].Z)
                {
                    miPuntoTramoEntrada = new Point3d(miPuntoTramoEntrada.X, miPuntoTramoEntrada.Y, misVerticesOrig[index - 1].Z + miHjAnt);
                }
                if (misVerticesOrig[index].Z + miHj > iVertices[index].Z)
                {
                    miPuntoTramoSalida = new Point3d(miPuntoTramoSalida.X, miPuntoTramoSalida.Y, misVerticesOrig[index].Z + miHj);
                }

                iVertices[index - 1] = miPuntoTramoEntrada;
                iVertices[index] = miPuntoTramoSalida;

                miCotaRasInters = miRestriccionGalibo;



            }
        }



        private static void imponerGaliboPorAbajo(List<Point3d> iVertices, oZonaCruces miZonaCruce, Point3d miPuntoIntersec, ref double miCotaRasInters, double miPk, oSingletonTerreno miTerreno)
        {
            //Calculo la cota del terreno en el punto de interseccion
            Point3d miPuntoEnElEje = mEjePol.getPointMasCercano(miPuntoIntersec);
            double miCotaTerrenoIntersec = (double)miTerreno.getZFromXY(miPuntoIntersec.X, miPuntoIntersec.Y);

            //Se la sumo al gálibo y esta será la menor cota permitida a la rasante dentro de DPH
            double miRestriccionGalibo = miCotaTerrenoIntersec - miZonaCruce.galibo;


            if (miRestriccionGalibo < miCotaRasInters)
            {
                //buscar los vertices que componen el tramo al que pertenece el punto de interseccion!

                int index = 0;
                Point3d miPuntoEnElEjeVertice = mEjePol.getPointMasCercano(iVertices[index]);
                double miPkVertice = mEjePol.GetDistAtPoint(miPuntoEnElEjeVertice);// (double)oLw.getPkAtPoint(miPuntoEnElEjeVertice, mEjePol);
                while (miPk > miPkVertice)
                {
                    index++;
                    Point3d miPuntoEnElEjeV = mEjePol.getPointMasCercano(iVertices[index]);
                    miPkVertice = mEjePol.GetDistAtPoint(miPuntoEnElEjeV);// (double)oLw.getPkAtPoint(miPuntoEnElEjeV, mEjePol);
                }

                Point3d miPuntoTramoEntrada = iVertices[index - 1];
                Point3d miPuntoTramoSalida = iVertices[index];

                double disPE = miPuntoTramoEntrada.DistanceTo(miPuntoIntersec);
                double disPS = miPuntoTramoSalida.DistanceTo(miPuntoIntersec);

                Point3d miPuntoTramoEntradaAux = new Point3d(miPuntoTramoEntrada.X, 0, 0);
                Point3d miPuntoTramoSalidaAux = new Point3d(miPuntoTramoSalida.X, 0, 0);
                Point3d miPuntoIntersecAux = new Point3d(miPuntoIntersec.X, 0, 0);

                Point3d miPuntoEnElEjeE = mEjePol.getPointMasCercano(miPuntoTramoEntrada);
                Point3d miPuntoEnElEjeS = mEjePol.getPointMasCercano(miPuntoTramoSalida);

                double? miPkE = mEjePol.GetDistAtPoint(miPuntoEnElEjeE);// oLw.getPkAtPoint(miPuntoEnElEjeE, mEjePol);
                double? miPkS = mEjePol.GetDistAtPoint(miPuntoEnElEjeS);// oLw.getPkAtPoint(miPuntoEnElEjeS, mEjePol);
                if (miPkE != null && miPkS != null)
                {
                    disPE = (double)miPk - (double)miPkE;
                    disPS = (double)miPkS - (double)miPk;
                }


                double miL = disPE + disPS;


                double miHmin = miRestriccionGalibo - miCotaRasInters;
                double miHj = (miHmin * miL) / (((miL * disPS) / disPE) + disPE - disPS);
                double miHjAnt = miHj * (disPS / disPE);

                if (misVerticesOrig[index - 1].Z + miHjAnt < iVertices[index - 1].Z)
                {
                    miPuntoTramoEntrada = new Point3d(miPuntoTramoEntrada.X, miPuntoTramoEntrada.Y, misVerticesOrig[index - 1].Z + miHjAnt);
                }
                if (misVerticesOrig[index].Z + miHj < iVertices[index].Z)
                {
                    miPuntoTramoSalida = new Point3d(miPuntoTramoSalida.X, miPuntoTramoSalida.Y, misVerticesOrig[index].Z + miHj);
                }

                iVertices[index - 1] = miPuntoTramoEntrada;
                iVertices[index] = miPuntoTramoSalida;

                miCotaRasInters = miRestriccionGalibo;



            }
        }


        private static List<Point3d> checkVertexInsideZonaGalibo(List<Point3d> iVertices, oZonaCruces miZona, List<Point3d> iColPtoInter, oSingletonTerreno iTerreno, Polyline iEje, ref bool iPtoIniMod, ref bool iPtoFinMod, bool isArriba)
        {

            //TODO ver si el punto final o el punto de inicio de modifican
            List<Point3d> misVerticesDentro = new List<Point3d>();


            double? miPkIntersecEntrada = iEje.GetDistAtPoint(iEje.getPointMasCercano(iColPtoInter[0]));
            double? miPkIntersecSalida = iEje.GetDistAtPoint(iEje.getPointMasCercano(iColPtoInter[1]));
            Point3d puntoInterEntrada, puntoInterSalida;
            if (miPkIntersecEntrada > miPkIntersecSalida)
            {
                double aux = (double)miPkIntersecEntrada;
                miPkIntersecEntrada = miPkIntersecSalida;
                miPkIntersecSalida = aux;
                puntoInterEntrada = iColPtoInter[1];
                puntoInterSalida = iColPtoInter[0];

            }
            else
            {
                puntoInterEntrada = iColPtoInter[0];
                puntoInterSalida = iColPtoInter[1];
            }

            foreach (Point3d miVertice in iVertices)
            {
                double miPk = (double)iEje.GetDistAtPoint(iEje.getPointMasCercano(miVertice));
                if (miPk > miPkIntersecEntrada && miPk < miPkIntersecSalida)
                {
                    misVerticesDentro.Add(miVertice);
                }
            }

            if (misVerticesDentro.Count == 0)
            {
                return iVertices;
            }
            else if (misVerticesDentro.Count == 1)
            {
                double? miPk = iEje.GetDistAtPoint(iEje.getPointMasCercano(misVerticesDentro[0]));
                double miRasante = misVerticesDentro[0].Z;
                double? miCotaTerreno = iTerreno.getZFromXY(misVerticesDentro[0].X, misVerticesDentro[0].Y);
                double miRestriccionGalibo = 0;
                if (miCotaTerreno != null)
                {
                    if (isArriba)
                    {
                        miRestriccionGalibo = (double)miCotaTerreno + miZona.galibo;
                        if (miRestriccionGalibo > miRasante)
                        {
                            int index = iVertices.IndexOf(misVerticesDentro[0]);
                            iVertices[index] = new Point3d(misVerticesDentro[0].X, misVerticesDentro[0].Y, miRestriccionGalibo);

                            ////Comprobar que nada el null.... PROBAR
                            //double miA = (double)miPk - (double)miPkIntersecEntrada;
                            //double miB = miRestriccionGalibo - puntoInterEntrada.Z;
                            //double? miPkPointAnt = iEje.GetDistAtPoint(iEje.getPointMasCercano(iVertices[index - 1]));
                            //double miNuevaCota = (((double)miPkPointAnt - (double)miPkIntersecEntrada) * miB / miA) + puntoInterEntrada.Z;

                            //if (miNuevaCota > iVertices[index - 1].Z)
                            //{
                            //    iVertices[index - 1] = new Point3d(iVertices[index - 1].X, iVertices[index - 1].Y, miNuevaCota);
                            //}


                            //miA = (double)miPkIntersecSalida - (double)miPk;
                            //miB = puntoInterSalida.Z - miRestriccionGalibo;
                            //double? miPkPointPost = iEje.GetDistAtPoint(iEje.getPointMasCercano(iVertices[index + 1]));
                            //miNuevaCota = (((double)miPkPointPost - (double)miPk) * miB / miA) + miRestriccionGalibo;

                            //if (miNuevaCota > iVertices[index + 1].Z)
                            //{
                            //    iVertices[index + 1] = new Point3d(iVertices[index + 1].X, iVertices[index + 1].Y, miNuevaCota);
                            //}

                        }
                    }
                    else
                    {
                        miRestriccionGalibo = (double)miCotaTerreno - miZona.galibo;
                        if (miRestriccionGalibo < miRasante)
                        {
                            int index = iVertices.IndexOf(misVerticesDentro[0]);
                            iVertices[index] = new Point3d(misVerticesDentro[0].X, misVerticesDentro[0].Y, miRestriccionGalibo);

                            ////Comprobar que nada el null.... PROBAR
                            //double miA = (double)miPk - (double)miPkIntersecEntrada;
                            //double miB = miRestriccionGalibo - puntoInterEntrada.Z;
                            //double? miPkPointAnt = iEje.GetDistAtPoint(iEje.getPointMasCercano(iVertices[index - 1]));
                            //double miNuevaCota = (((double)miPkPointAnt - (double)miPkIntersecEntrada) * miB / miA) + puntoInterEntrada.Z;


                            //if (miNuevaCota < iVertices[index - 1].Z)
                            //{
                            //    iVertices[index - 1] = new Point3d(iVertices[index - 1].X, iVertices[index - 1].Y, miNuevaCota);
                            //}


                            //miA = (double)miPkIntersecSalida - (double)miPk;
                            //miB = puntoInterSalida.Z - miRestriccionGalibo;
                            //double? miPkPointPost = iEje.GetDistAtPoint(iEje.getPointMasCercano(iVertices[index + 1]));
                            //miNuevaCota = (((double)miPkPointPost - (double)miPk) * miB / miA) + miRestriccionGalibo;

                            //if (miNuevaCota < iVertices[index + 1].Z)
                            //{
                            //    iVertices[index + 1] = new Point3d(iVertices[index + 1].X, iVertices[index + 1].Y, miNuevaCota);
                            //}

                        }
                    }
                }

            }
            //MAS DE UN VERTICE
            else
            {

                int numVerticeActual = 0;
                foreach (Point3d miVerticeDentro in misVerticesDentro)
                {
                    double miRasante = miVerticeDentro.Z;
                    double? miPk = iEje.GetDistAtPoint(iEje.getPointMasCercano(miVerticeDentro));


                    if (miPk != null && miPkIntersecEntrada != null && miPkIntersecSalida != null)
                    {

                        double miA = (((double)miPk - (double)miPkIntersecEntrada) / ((double)miPkIntersecSalida - (double)miPkIntersecEntrada));
                        double miB = puntoInterSalida.Z - puntoInterEntrada.Z;
                        double miC = puntoInterEntrada.Z;

                        double miGaliboPunto = (miA * miB) + miC;

                        if (isArriba)
                        {
                            if (miGaliboPunto > miRasante)
                            {
                                int index = iVertices.IndexOf(miVerticeDentro);
                                iVertices[index] = new Point3d(miVerticeDentro.X, miVerticeDentro.Y, miGaliboPunto);
                            }
                        }
                        else
                        {
                            if (miGaliboPunto < miRasante)
                            {
                                int index = iVertices.IndexOf(miVerticeDentro);
                                iVertices[index] = new Point3d(miVerticeDentro.X, miVerticeDentro.Y, miGaliboPunto);
                            }
                        }
                    }
                    numVerticeActual++;
                }

            }


            return iVertices;
        }

        private static List<List<Point3d>> OrdenarTramosPorPks(Point3dCollection iColPtoInter, Polyline miEjePol, oZonaCruces miZona, List<Point3d> iVertices)
        {

            List<List<Point3d>> miColPtoInterES = new List<List<Point3d>>();
            if (!miZona.isPtoInLwZona(iVertices[0]) && !miZona.isPtoInLwZona(iVertices[iVertices.Count - 1]))
            {
                if (iColPtoInter.Count > 2)
                {
                    List<double> misPks = new List<double>();
                    foreach (Point3d miPunto in iColPtoInter)
                    {
                        double? miPk = miEjePol.GetDistAtPoint(miEjePol.getPointMasCercano(miPunto));// oLw.getPkAtPoint(miEjePol.getPointMasCercano(miPunto), miEjePol);
                        misPks.Add((double)miPk);
                    }
                    misPks.Sort();
                    List<Point3d> miTramo = new List<Point3d>();
                    bool isEntrada = true;
                    for (int i = 0; i < misPks.Count; i++)
                    {
                        Point3d miPuntoConPk = new Point3d();
                        foreach (Point3d miPunto in iColPtoInter)
                        {
                            double? miPk = miEjePol.GetDistAtPoint(miEjePol.getPointMasCercano(miPunto));//oLw.getPkAtPoint(miEjePol.getPointMasCercano(miPunto), miEjePol);
                            if (misPks[i] == miPk)
                            {
                                miPuntoConPk = miPunto;
                            }
                        }
                        if (isEntrada)
                        {
                            miTramo = new List<Point3d>();
                            miTramo.Add(miPuntoConPk);
                            isEntrada = false;
                        }
                        else
                        {
                            miTramo.Add(miPuntoConPk);
                            miColPtoInterES.Add(miTramo);
                            isEntrada = true;
                        }
                    }

                }
                else
                {
                    if (iColPtoInter.Count > 0)
                    {
                        List<Point3d> miTramo = new List<Point3d>();
                        miTramo.Add(iColPtoInter[0]);
                        miTramo.Add(iColPtoInter[1]);
                        miColPtoInterES.Add(miTramo);
                    }
                }

            }
            else
            {
                oTadil.data.UserInfo.showInfo(string.Format(strError.ePuntoInicioFinDentroDeZonaCruce, miZona.nombre.ToString()));
            }
            return miColPtoInterES;
        }

        private static bool isRasanteArriba(List<Point3d> iTramo, Alzado iAlzado,  oSingletonTerreno iTerreno)
        {
            bool encimaRasante = true;
            bool encimaRasanteInt1 = true;
            bool encimaRasanteInt2 = true;
            double difRasanteTerreno1 = 0;
            double difRasanteTerreno2 = 0;
            int i = 0;
            foreach (Point3d miPuntoIntersec in iTramo)
            {
                i++;
                double? miPk = mEjePol.GetDistAtPoint(mEjePol.getPointMasCercano(miPuntoIntersec));// oLw.getPkAtPoint(miPuntoIntersec, mEjePol);
                if (miPk != null)
                {
                    double miRasante = iAlzado.getCotaRasante((double)miPk);
                    double miCotaT = (double)iTerreno.getZFromXY(miPuntoIntersec.X, miPuntoIntersec.Y);
                    if (miCotaT > miRasante)
                    {
                        if (i == 1)
                        {
                            encimaRasanteInt1 = false;
                        }
                        else if (i == 2)
                        {
                            encimaRasanteInt2 = false;
                        }
                    }

                    if (i == 1)
                    {
                        difRasanteTerreno1 = Math.Abs(miCotaT - miRasante);
                    }
                    else if (i == 2)
                    {
                        difRasanteTerreno2 = Math.Abs(miCotaT - miRasante);
                    }
                }
            }

            if ((encimaRasanteInt1 && encimaRasanteInt2) || (!encimaRasanteInt1 && !encimaRasanteInt2))
            {
                encimaRasante = encimaRasanteInt1;
            }
            else
            {
                if (difRasanteTerreno1 > difRasanteTerreno2)
                {
                    encimaRasante = encimaRasanteInt1;
                }
                else
                {
                    encimaRasante = encimaRasanteInt2;
                }
            }
            return encimaRasante;
        }

        private static bool isTramoDentroDPH(List<oZonaGis> iZonasDPH, List<Point3d> iTramo)
        {

            bool dentro = false;

            foreach (oZonaGis miZonaDPH in iZonasDPH)
            {
                Polyline miPolDPH = miZonaDPH.lwZona;

                foreach(Point3d miPuntoTramo in iTramo)
                {
                    if (oPolygon.isPointInPolygon(miPolDPH, miPuntoTramo.X, miPuntoTramo.Y))
                    {
                        dentro = true;
                    }
                }

            }
            return dentro;
        }

        public static void exportarPerfil(string iNombreArchivo, eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {

            Stopwatch miMedicion = new Stopwatch();
            miMedicion.Start();
            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {

                Polyline miLwEjePerfilRasante = miSolucion.ejePerfilRasante;


                Xrecord miXrecordAlzado = engCadNet.oXrecord.getXrecord(miLwEjePerfilRasante.ObjectId, "info");
                Alzado miAlzado = Alzado.recuperaAlzado(engCadNet.oXrecord.getStream(miXrecordAlzado));



                miAlzado.exportarPerfilLongitudinal(iNombreArchivo);


                //Tiempo
                miMedicion.Stop();

                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.Minutes);
            }
        }
        public static void exportarPerfil_Amilia(string iNombreArchivo, eEstudioTipo iEstudioTipo, Guid iIdSolucion)
        {
            using (oSolucion miSolucion = new oSolucion(iIdSolucion))
            {
                Stopwatch miMedicion = new Stopwatch();
                miMedicion.Start();
                dsApp.tbSolucionRow miRow = oDalTbSolucion.getSolucion(iIdSolucion);

                if (!miRow.IsEjeTrazado_AmiliaNull())
                {
                    // 1. Obtenemos el array de bytes de la columna del DataSet
                    byte[] datosRecuperados = miRow.Alzado_Amilia;

                    // 2. Creamos un MemoryStream con esos bytes
                    using (MemoryStream ms = new MemoryStream(datosRecuperados))
                    {
                        // 3. Usamos tu método estático para reconstruir la clase
                        Alzado miAlzado = Alzado.recuperaAlzado(ms);
                        miAlzado.exportarPerfilLongitudinal(iNombreArchivo);
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo("El eje de trazado no tiene datos de Amilia guardados.");
                }
                //Tiempo
                miMedicion.Stop();

                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.Minutes);
            }
        }
    }
}
