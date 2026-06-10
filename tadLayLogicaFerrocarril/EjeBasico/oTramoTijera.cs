using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLogica.logica.EjeBasicoNew;
using Autodesk.AutoCAD.DatabaseServices;
using tadLayShare.puntos;
using Autodesk.AutoCAD.Colors;
using engCadNet;
using Autodesk.AutoCAD.Geometry;
using EjeDeTrazado.componentes;
using tadLayLogica;
using tadLayLogica.estudioTipo;
using EjeDeTrazado.puntosDelEje;
using tadLayLogica.zonaGis;

namespace tayLogicaTijera.EjeBasico
{
    public class oTramoTijera
    {

        public int mIdTijera { get; set; }
        public int mIdTramo { get; set; }
        public int mPosicionTijera { get; set; }
        public oTramoEjeBasico mTramoPrevio { get; set; }
        public Polyline mTrazadoTramo { get; set; }
        public oP3d mPuntoIntermedio { get; set; }
        public oP3d P1 { get; set; }
        public oP3d P2 { get; set; }
        public oP2d mPtoTarget { get; set; }
        public bool mIsValido { get; set; }

        public List<EjeDeTrazado.componentes.Componente> mComponentesEjeTrazado { get; set; }
        public Vertice mVerticeEjeTrazado { get; set; }

        public List<oTramoFerrocarril> mTramosFerrocarril = new List<oTramoFerrocarril>();

        public double valoracionDistanciaGlobal_0_10 { get; set; }
        public double valoracionPendienteGlobal_0_10  { get; set; }
        public double valoracionCosteImplantacionGlobal_0_10 { get; set; }

        public double valoracionDistanciaLocal;
        public double valoracionPendienteLocal;
        public double valoracionCosteImplantacionLocal;

        public eTramoEjeBasicoError errorTramo;
        Polyline mEjeVisibiliad = null;


        public double valoracionPonderadaGlobal_0_10 { get; set; }
        public bool isRecto = false;

        oAbanicoDesign abanicoDesign;

        private double mCoeficienteValoracionDistanciaTramo;
        private double mCoeficienteValoracionDistanciaAlEje;
        


        public oTramoTijera(int iIdTijera, int iIdTramoPosicionTijera, oTramoEjeBasico iTramoPrevio, oP3d iP2, oP2d iPtoTarget, oP3d iPtoIntermedio, bool iIsRecto, Polyline iEjeVisibilidad, oAbanicoDesign iAbanicoDesign, double coefDistTramo, double coefDistEje)
        {
            mEjeVisibiliad = iEjeVisibilidad;
            this.mTramoPrevio = iTramoPrevio;
            this.mIdTijera = iIdTijera;
            this.mPosicionTijera = iIdTramoPosicionTijera;
            this.mIdTramo = this.mTramoPrevio.idTramo + 1;

            this.P1 = this.mTramoPrevio.P2;
            this.P2 = iP2;

            this.mPtoTarget = iPtoTarget;

            this.mPuntoIntermedio = iPtoIntermedio;
            this.mIsValido = true;
            this.isRecto = iIsRecto;

            abanicoDesign = iAbanicoDesign;
            mCoeficienteValoracionDistanciaAlEje = coefDistEje;
            mCoeficienteValoracionDistanciaTramo = coefDistTramo;

        }

        public oTramoTijera(int iIdTijera, int iIdTramoPosicionTijera, int idTramo, oP3d ptoOrigen, oP3d iP2, oP2d iPtoTarget, oP3d iPtoIntermedio, bool iIsRecto, Polyline iEjeVisibilidad, oAbanicoDesign iAbanicoDesign, double coefDistTramo, double coefDistEje)
        {
            mEjeVisibiliad = iEjeVisibilidad;
            this.mIdTijera = iIdTijera;
            this.mPosicionTijera = iIdTramoPosicionTijera;
            this.mIdTramo = idTramo;

            this.P1 = ptoOrigen;
            this.P2 = iP2;

            this.mPtoTarget = iPtoTarget;

            this.mPuntoIntermedio = iPtoIntermedio;
            this.mIsValido = true;
            this.isRecto = iIsRecto;

            abanicoDesign = iAbanicoDesign;
            mCoeficienteValoracionDistanciaAlEje = coefDistEje;
            mCoeficienteValoracionDistanciaTramo = coefDistTramo;

        }

        //el tramo tijera se forma de todos los tramos Ferrocarril que entren (segun la separacion de los puntos de apoyo)
        //ya se ha tenido que rellenar la polilinea del mTrazadoTramo
        public void createTramosFerrocarril(double iSeparacionPuntosApoyo, bool isEntronque=false, Polyline finalTramoContrario=null )
        {
            List<Polyline> miPolAEliminar = new List<Polyline>();
            mTramosFerrocarril = new List<oTramoFerrocarril>();

            double cotafinal;
            var nuevoTrazado = GetNuevoTrazado(out cotafinal, isEntronque, finalTramoContrario);


            List<Point3d> misPuntosApoyo = new List<Point3d>();

            double miSegmentosDecimal = nuevoTrazado.Length / iSeparacionPuntosApoyo;
            int miSegmentoNum = (int)Math.Floor(miSegmentosDecimal) +1;

            double miSeparacionReal = nuevoTrazado.Length / miSegmentoNum;

            var puntoInicial = new Point3d(nuevoTrazado.StartPoint.X, nuevoTrazado.StartPoint.Y, cotafinal);
            misPuntosApoyo.Add(puntoInicial);
            for (int i = 0; i < miSegmentoNum - 1; i++)
            {
                Point3d miPunto = nuevoTrazado.GetPointAtDist((i + 1) * miSeparacionReal);
                misPuntosApoyo.Add(miPunto);
            }
            misPuntosApoyo.Add(nuevoTrazado.EndPoint);

            oTramoEjeBasico miTramoPrevio = mTramoPrevio;

            for (int i = 0; i < misPuntosApoyo.Count- 1; i++)
            {
                    //añado la polilinea del trazado en la capa 0, solo de manera auxiliar (luego debo eliminarla)
                    //Polyline miTrazadoAuxiliar = oLw.addLw2d(oLw.getColPto(mTrazadoTramo), false, "0");

                    oTramoFerrocarril miTramo = new oTramoFerrocarril();
                    miTramo.tramoPrevio = miTramoPrevio;
                    miTramo.idAbanico = mIdTijera;
                    miTramo.idPosicion = mPosicionTijera;
                    miTramo.idTramo = mIdTramo;
                    miTramo.mTrozoTramoTijera = i;
                    miTramo.P1 = new oP3d(misPuntosApoyo[i].X, misPuntosApoyo[i].Y, misPuntosApoyo[i].Z);
                    miTramo.P2 = new oP3d(misPuntosApoyo[i+1].X, misPuntosApoyo[i+1].Y, misPuntosApoyo[i+1].Z);
                    miTramo.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceLargo;
                    miTramo.ptoTarget = mPtoTarget;

                    Point3d miPuntoInicio = nuevoTrazado.getPointMasCercano(misPuntosApoyo[i]);
                    Point3d miPuntoFin = nuevoTrazado.getPointMasCercano(misPuntosApoyo[i + 1]);

                    miTramo.mTrazadoTramo = oLw.splitLwTwoPoints(nuevoTrazado, miPuntoInicio, miPuntoFin, oTadil.data.Layer.abanicoTramosAuxiliar.name);

                    miPolAEliminar.Add(miTramo.mTrazadoTramo);

                    mTramosFerrocarril.Add(miTramo);
                    miTramoPrevio = miTramo;

            }

            oLw.eliminarPolilineas(miPolAEliminar);

        }

        public Polyline GetNuevoTrazado(out double cotafinal, bool isEntronque= false, Polyline finalTramoContrario= null)
        {
            var miPolAEliminar = new List<Polyline>();
            var pointMedio = GetPuntoMedioCurva();
            pointMedio = new Point3d(pointMedio.X, pointMedio.Y, mVerticeEjeTrazado.getPuntoZ);
            var point = mTrazadoTramo.getPointMasCercano(pointMedio);
            if (isEntronque) point = mTrazadoTramo.EndPoint;


            var nuevoTrazado = oLw.splitLwTwoPoints(mTrazadoTramo, mTrazadoTramo.StartPoint, point,
                oTadil.data.Layer.abanicoTramosAuxiliar.name);
            cotafinal = P1.Z;
            if (mTramoPrevio != null && mTramoPrevio.mTrazadoFinal != null)
            {
                Polyline mTrazadoFinalAux = oLw.addLw2d(oLw.getColPto(mTramoPrevio.mTrazadoFinal), false,
                    oTadil.data.Layer.abanicoTramosAuxiliar.name);
                Polyline ejeTraxadoAux = oLw.addLw2d(oLw.getColPto(nuevoTrazado), false,
                    oTadil.data.Layer.abanicoTramosAuxiliar.name);
                var miList = new List<Polyline> {mTrazadoFinalAux, ejeTraxadoAux};
                miPolAEliminar.Add(mTrazadoFinalAux);
                miPolAEliminar.Add(ejeTraxadoAux);

                nuevoTrazado = oLw.joinLw(miList, false);
                cotafinal = mTramoPrevio.mCotaFinal;
            }


            if (isEntronque)
            {
                finalTramoContrario.AddVertexAt(0, nuevoTrazado.GetPoint2dAt(nuevoTrazado.NumberOfVertices-1),0,0,0);
                var miList = new List<Polyline> { nuevoTrazado, finalTramoContrario };
                nuevoTrazado = oLw.joinLw(miList, false);

                //for (int i = 0; i < finalTramoContrario.NumberOfVertices; i++)
                //{
                //    nuevoTrazado.AddVertexAt(nuevoTrazado.NumberOfVertices, finalTramoContrario.GetPoint2dAt(i), 0, 0, 0);
                //}
            }

            oLw.eliminarPolilineas(miPolAEliminar);

            return nuevoTrazado;
        }

        private Point3d GetPuntoMedioCurva()
        {
            var curva = mComponentesEjeTrazado[0];
            foreach (var componente in mComponentesEjeTrazado)
            {
                if (componente.getTipoComponente() == Componente.tipoComponente.curva)
                    curva = componente;
            }
            var distMedia = curva.getPkIni + ((curva.getPkFinal() - curva.getPkIni)/2);
            var punto = curva.getPointAtDist(distMedia);
            
            return new Point3d(punto[0], punto[1],0);
        }

        public Polyline getTramoFinal()
        {
            if (mTrazadoTramo.Length > 1000) { int a = 0; }
            var pointMedio = GetPuntoMedioCurva();
            pointMedio = new Point3d(pointMedio.X, pointMedio.Y, mVerticeEjeTrazado.getPuntoZ);
            var point = mTrazadoTramo.getPointMasCercano(pointMedio);
            var nuevoTrazado = oLw.splitLwTwoPoints(mTrazadoTramo, point, mTrazadoTramo.EndPoint, oTadil.data.Layer.abanicoTramosAuxiliar.name);

            return nuevoTrazado;
        }

        #region "propiedades"
        public double longitud2d
        {
            get
            {
                return this.mTrazadoTramo.Length;
            }
        }


        public double longitud2dP1P2
        {
            get
            {
                return this.P1.distTo2d(new oP2d(this.P2.X, this.P2.Y)); 
            }
        }

        public bool isValido()
        {
            if (mIsValido)
            {
                bool isValido = true;
                foreach (oTramoFerrocarril item in mTramosFerrocarril)
                {
                    if (!item.isTramoValido)
                    {
                        isValido = false;
                        this.errorTramo = item.errorTramo;
                    }
                }
                mIsValido = isValido;
            }
            if (!mIsValido)
            {

                foreach (oTramoFerrocarril item in mTramosFerrocarril)
                {
                    item.isTramoValido = false;
                    item.errorTramo = this.errorTramo;
                }
            }
            return mIsValido;
        }

        public bool isSeccionNull()
        {
            bool isSeccionNull = false;
            foreach (oTramoFerrocarril item in mTramosFerrocarril)
            {
                if (item.seccion == null) isSeccionNull = true; 
            }
            return isSeccionNull;
        }

        public bool hasSeccionEstructuras()
        {
            bool isSeccionHasEstructuras = false;
            foreach (oTramoFerrocarril item in mTramosFerrocarril)
            {
                if (item.seccion != null && item.seccion.hasEstructuras()) isSeccionHasEstructuras = true; ;
            }
            return isSeccionHasEstructuras;
        }

        #endregion

        #region "Valoraciones"

        public double getValoracionDistanciaLocal()
        {
            Point3d miPuntoSobreEjeVisivilidad = mEjeVisibiliad.getPointMasCercano(new Point3d(this.P2.X, this.P2.Y, 0));
            double distanciaAlEje = miPuntoSobreEjeVisivilidad.DistanceTo(new Point3d(this.P2.X, this.P2.Y, 0));

            valoracionDistanciaLocal = this.P2.distTo2d(this.mPtoTarget) +
                                       (mCoeficienteValoracionDistanciaTramo*this.longitud2d) +
                                       (mCoeficienteValoracionDistanciaAlEje*distanciaAlEje);

            return valoracionDistanciaLocal;
        }

        public double getValoracionOrografiaLocal()
        {
            valoracionPendienteLocal = 0;
            double numeroDeSecciones = 0;

            foreach (oTramoFerrocarril item in mTramosFerrocarril)
            {
                foreach (oSeccionEjeBasico miSeccion in item.seccion)
                {
                    valoracionPendienteLocal = valoracionPendienteLocal + miSeccion.valoracionSlopeUnitaria();
                }
                numeroDeSecciones = numeroDeSecciones + item.seccion.Count;
            }
            valoracionPendienteLocal = valoracionPendienteLocal / numeroDeSecciones;

            return valoracionPendienteLocal;
            
        }

        public double getValoracionImplantacionLocal()
        {
            valoracionCosteImplantacionLocal = 0;
            var longitud = 0.0;
            foreach (oTramoFerrocarril item in mTramosFerrocarril)
            {
                valoracionCosteImplantacionLocal = valoracionCosteImplantacionLocal + item.valoracionCosteImplantacionTramo();
                longitud = longitud + item.mTrazadoTramo.Length;
            }
            valoracionCosteImplantacionLocal = valoracionCosteImplantacionLocal / longitud;
            return valoracionCosteImplantacionLocal;
        }

        public bool componenteLineaLongitudMenor(double iLmin)
        {
            var result = false;
            foreach (var componente in mComponentesEjeTrazado)
            {
                if (componente.getTipoComponente() != Componente.tipoComponente.linea) continue;
                if (componente.getLongitud() < iLmin) result = true;
            }
            return result;
        }

        #endregion

        #region "Validaciones"

        public void createSeccionesPendienteObligada(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes,
            IEstudio iEstudioData, bool isEntronque, oP3d iPuntoFinal, double? iPendAnterior, double? ilongAnterior,
            double pendienteObligada)
        {
            if (mIsValido)
            {
                for (int i = 0; i < mTramosFerrocarril.Count; i++)
                {
                    if (mTramosFerrocarril[i].isTramoValido)

                        if (i == 0)
                        {
                            mTramosFerrocarril[i].P2.Z = oTrigo.getP2zFromP1andLon(mTramosFerrocarril[i].P1.Z,
                                pendienteObligada, mTramosFerrocarril[i].mTrazadoTramo.Length);

                            mTramosFerrocarril[i].createSeccionP1P2(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData,
                                false);
                        }
                        else
                        {
                            mTramosFerrocarril[i].createSeccion(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData,
                                false, isEntronque, iPuntoFinal);
                        }

                    //Comprobar que si el tramo anterior tiene estructuras entonces este tambien cumple las condiciones de estructuras
                    ReajustarComprobarPendientes(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData,
                        iPendAnterior, ilongAnterior, i);

                    //actualizar el punto 1 del siguiente tramos con el P2 del anterior
                    ActualizaDatosSiguienteTramo(i);

                    iPendAnterior = (mTramosFerrocarril[i].P2.Z - mTramosFerrocarril[i].P1.Z)/
                                    mTramosFerrocarril[i].mTrazadoTramo.Length;
                    ilongAnterior = mTramosFerrocarril[i].mTrazadoTramo.Length;
                }
            }
            P2.Z = mTramosFerrocarril[mTramosFerrocarril.Count - 1].P2.Z;
            this.mIsValido = isValido();
        }

        public void createSecciones(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, bool isEntronque, oP3d iPuntoFinal, double? iPendAnterior, double? ilongAnterior)
        {
            if (mIsValido)
            {
                for (int i = 0; i < mTramosFerrocarril.Count; i++)
                {
                    if (mTramosFerrocarril[i].isTramoValido) mTramosFerrocarril[i].createSeccion(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData, false, isEntronque, iPuntoFinal);
                    
                    //Comprobar que si el tramo anterior tiene estructuras entonces este tambien cumple las condiciones de estructuras
                    ReajustarComprobarPendientes(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData, iPendAnterior, ilongAnterior, i);

                    //actualizar el punto 1 del siguiente tramos con el P2 del anterior
                    ActualizaDatosSiguienteTramo(i); 

                    iPendAnterior = (mTramosFerrocarril[i].P2.Z - mTramosFerrocarril[i].P1.Z) / mTramosFerrocarril[i].mTrazadoTramo.Length;
                    ilongAnterior = mTramosFerrocarril[i].mTrazadoTramo.Length;
                }
            }
            P2.Z = mTramosFerrocarril[mTramosFerrocarril.Count - 1].P2.Z;
            this.mIsValido = isValido();
        }

        private void ReajustarComprobarPendientes(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes,
            IEstudio iEstudioData, double? iPendAnterior, double? ilongAnterior, int i)
        {
            if (mTramosFerrocarril[i].isTramoValido)
            {
                if (i > 0)
                {
                    if (mTramosFerrocarril[i - 1].isTramoValido)
                    {
                        ObligarEstructuraSiNecesario(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData, i);
                    }
                    else
                    {
                        mTramosFerrocarril[i].isTramoValido = false;
                        mTramosFerrocarril[i].errorTramo = mTramosFerrocarril[i - 1].errorTramo;
                    }
                }
                CompruebaPendienteAnterior(iLonDiscretizacionProyecto, iRoadPendientes, iEstudioData, iPendAnterior,
                    ilongAnterior, i);
            }
        }

        private void ActualizaDatosSiguienteTramo(int i)
        {
            if (mTramosFerrocarril[i].isTramoValido)
            {
                if ((i + 1) < mTramosFerrocarril.Count) mTramosFerrocarril[i + 1].P1.Z = mTramosFerrocarril[i].P2.Z;
            }
            else
            {
                if ((i + 1) < mTramosFerrocarril.Count)
                {
                    mTramosFerrocarril[i + 1].isTramoValido = false;
                    mTramosFerrocarril[i + 1].errorTramo = mTramosFerrocarril[i].errorTramo;
                }
            }
        }

        private void CompruebaPendienteAnterior(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes,
            IEstudio iEstudioData, double? iPendAnterior, double? ilongAnterior, int i)
        {
            if (iPendAnterior != null && ilongAnterior != null)
            {
                if (!oTijera.cumplePendiente(mTramosFerrocarril[i], iPendAnterior.Value, ilongAnterior.Value))
                {
                    var ZCumple = oTijera.getZPendienteCumple(mTramosFerrocarril[i], iPendAnterior.Value,
                        ilongAnterior.Value);
                    mTramosFerrocarril[i].P2.Z = ZCumple;
                    mTramosFerrocarril[i].createSeccionP1P2(iLonDiscretizacionProyecto, iRoadPendientes,
                        iEstudioData, false);
                }
            }
        }

        private void ObligarEstructuraSiNecesario(double iLonDiscretizacionProyecto, oRoadPendientes iRoadPendientes,
            IEstudio iEstudioData, int i)
        {
            if (mTramosFerrocarril[i - 1].seccion.hasEstructuras())
            {
                if (mTramosFerrocarril[i].pendienteAbsolutaPC >
                    iRoadPendientes.estructuraPendienteCalculoMaximoPC)
                {
                    double miP2Z =
                        getP2ZFromPendienteMaximaAbsoluta(
                            iRoadPendientes.estructuraPendienteCalculoMaximoPC,
                            mTramosFerrocarril[i]);

                    mTramosFerrocarril[i].P2.Z = miP2Z;
                    mTramosFerrocarril[i].createSeccionP1P2(iLonDiscretizacionProyecto,
                        iRoadPendientes, iEstudioData, false);
                }
            }
        }



        public void validarTramo3d(double iTresXAjmin, oP3d iPuntoObjetivo, double iPendiente)
        {
            if (mIsValido)
            {
                foreach (oTramoFerrocarril miTramo in mTramosFerrocarril)
                {
                    miTramo.validarTramoCercanoPF(iTresXAjmin, iPuntoObjetivo, iPendiente);
                }
            }
            this.mIsValido = isValido();

        }


        public void validarTramo2d( IEstudio estudioData)
        {
            foreach (oTramoFerrocarril miTramo in mTramosFerrocarril)
            {
                //ver validaciones, no son correctas
                miTramo.validarTramoP2InsideTerreno();

                miTramo.validarTramoP2NearBordesTerreno();

                miTramo.validarTramoZonasNoPaso();
                miTramo.validarCruceDPH(estudioData);
                miTramo.validarDentroDPH(estudioData);
            }
            this.mIsValido = isValido();
            ValidarTrazadoFinalTramo();
        }

        private void ValidarTrazadoFinalTramo()
        {
            if (mIsValido)
            {
                //comprobar trazado final
                var tramoFinal = getTramoFinal();
                if (oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPasoPolilinea(tramoFinal))
                {
                    mIsValido = false;
                    errorTramo = eTramoEjeBasicoError.zonaNoPaso;
                }
                for (int i = 0; i < tramoFinal.NumberOfVertices; i++)
                {
                    if (!mIsValido) break;
                    var vertex = tramoFinal.GetPoint3dAt(i);
                    if (!oSingletonTerreno.getInstance.isPtoInsideTerreno(vertex.X, vertex.Y))
                    {
                        mIsValido = false;
                        errorTramo = eTramoEjeBasicoError.puntoExteriorSuperficie;
                    }
                }
                for (int i = 0; i < tramoFinal.NumberOfVertices; i++)
                {
                    if (!mIsValido) break;
                    var vertex = tramoFinal.GetPoint3dAt(i);
                    if (oSingletonTerreno.getInstance.isPtoCercaBorde(vertex.X, vertex.Y))
                    {
                        mIsValido = false;
                        errorTramo = eTramoEjeBasicoError.tramoMuyCercanoAlBordeDelTerreno;
                    }
                }
            }
        }

        public bool comprobarCompatibilidadTrazado(oP3d iPuntoOpuesto, double iPendMaxPUSinSigno)
        {

            double distancia = this.P2.distTo2d(new oP2d(iPuntoOpuesto.X, iPuntoOpuesto.Y));
            double cambioPendiente = Math.Abs((this.P2.Z - iPuntoOpuesto.Z));
            double miPendiente = Math.Abs((this.P2.Z - iPuntoOpuesto.Z) / this.P2.distTo2d(new oP2d(iPuntoOpuesto.X, iPuntoOpuesto.Y)));

            //if (miPendiente > (iPendMaxPUSinSigno))
            //{
            //    this.mIsValido = false;
            //    errorTramo = eTramoEjeBasicoError.pendienteTramoFinalRamasTijeraInvalida;
            //    return false;
            //}
            return true;
        }

        private double comprobarCotaEntronque(oP3d p1Entronque, oP3d p2Entronque, oP3d iPuntoAComprobar, oRoadPendientes iRoadPendientes)
        {
            double cota1Arriba = ((iRoadPendientes.calzadaPendienteCalculoMaximoPC/100) * (iPuntoAComprobar.X - p1Entronque.X)) + p1Entronque.Y;
            double cota1Abajo = (((iRoadPendientes.calzadaPendienteCalculoMaximoPC/100) * -1) * (iPuntoAComprobar.X - p1Entronque.X)) + p1Entronque.Y;

            double cota2Arriba = (-1) * ((((iRoadPendientes.calzadaPendienteCalculoMaximoPC / 100) * -1) * (p2Entronque.X - iPuntoAComprobar.X)) - p2Entronque.Y);
            double cota2Abajo = (-1) * (((iRoadPendientes.calzadaPendienteCalculoMaximoPC / 100) * (p2Entronque.X - iPuntoAComprobar.X)) - p2Entronque.Y);


            double cotaArriba = cota1Arriba, cotaAbajo = cota1Abajo;

            if (cota1Arriba > cota2Arriba) cotaArriba = cota2Arriba;
            if (cota1Abajo < cota2Abajo) cotaAbajo = cota2Abajo;


            double distCotasArriba = Math.Abs(iPuntoAComprobar.Y - cotaArriba);
            double distCotasAbajo = Math.Abs(iPuntoAComprobar.Y - cotaAbajo);

            if (distCotasArriba < distCotasAbajo)
            {
                if (cotaArriba < iPuntoAComprobar.Y) return cotaArriba; else return iPuntoAComprobar.Y;
            }
            else
            {
                if (cotaAbajo > iPuntoAComprobar.Y) return cotaAbajo; else return iPuntoAComprobar.Y;
            }
        }



        #endregion
      
        public void drawTramo2D(string iCapa, string iXData)
        {
            List<Point2d> miListaPuntos = new List<Point2d>();
            miListaPuntos.Add(this.mTrazadoTramo.GetPoint2dAt(0));
            miListaPuntos.Add(new Point2d(this.mPuntoIntermedio.X, this.mPuntoIntermedio.Y));
            miListaPuntos.Add(this.mTrazadoTramo.GetPoint2dAt(this.mTrazadoTramo.NumberOfVertices-1));
            Polyline miPol = oLw.addLw2d(miListaPuntos, false, oTadil.data.Layer.abanicoTramos.name);
            oXdata.setXdata(miPol.ObjectId, iXData, iXData);
            oXdata.setXdata(miPol.ObjectId, this.mIdTijera.ToString(), this.mIdTijera.ToString());


            using (oEntidad<Polyline> miEntidad = new oEntidad<Polyline>(miPol))
            {

                miEntidad.open();

                if (this.isValido() && this.isSeccionNull())
                {
                    miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cBlancoIndex);
                }
                else if (this.isValido() && !this.isSeccionNull() && !this.hasSeccionEstructuras())
                {
                    miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cMoradoIndex);
                }
                else if (this.isValido() && !this.isSeccionNull() && this.hasSeccionEstructuras())
                {
                    miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cAzulIndex);
                }
                else if (!this.isValido())
                {
                    miEntidad.entidad.Color = Color.FromColorIndex(ColorMethod.ByLayer, oColor.cRojoIndex);
                }
                else
                {
                    throw new Exception("Opción No Configurada");
                }


                miEntidad.save();
            }


            oP3d miPtoTexto = new oP3d(this.P2.X, this.P2.Y - 4, 0);

            oMTexto.addMText2D(this.ToString(), miPtoTexto.toArray3d(), 1, 0, iCapa, iXData, this.mIdTijera.ToString());

        }


        public void drawTramo2DsoloLineas(string iCapa)
        {
            Line miLine = engCadNet.oLine.addLine2d(this.P1.X, this.P1.Y, this.mPuntoIntermedio.X, this.mPuntoIntermedio.Y, iCapa);
            Line miLine2 = engCadNet.oLine.addLine2d(this.mPuntoIntermedio.X, this.mPuntoIntermedio.Y, this.P2.X, this.P2.Y, iCapa);
            oXdata.setXdata(miLine.ObjectId, "idPosicion", this.mPosicionTijera.ToString());


            List<Point2d> miListaPuntos = new List<Point2d>();
            for (int i = 0; i < this.mTrazadoTramo.NumberOfVertices; i++)
            {
                miListaPuntos.Add(this.mTrazadoTramo.GetPoint2dAt(i));
            }
            Polyline miPol = oLw.addLw2d(miListaPuntos, false, oTadil.data.Layer.abanicoTramos.name);
        }


        public void drawTramo3D(string iCapa)
        {
            List<Point2d> miListaPuntos = new List<Point2d>();
            for (int i = 0; i < this.mTrazadoTramo.NumberOfVertices; i++)
            {
                miListaPuntos.Add(this.mTrazadoTramo.GetPoint2dAt(i));
            }
            oLw.addLw2d(miListaPuntos, false, iCapa);
        }

        public string ToString()
        {

            Point3d miPuntoSobreEjeVisivilidad = mEjeVisibiliad.getPointMasCercano(new Point3d(this.P2.X, this.P2.Y, 0));
            double distanciaAlEje = miPuntoSobreEjeVisivilidad.DistanceTo(new Point3d(this.P2.X, this.P2.Y, 0));

            StringBuilder miStr = new StringBuilder();


            if (this.mIsValido)
            {
                miStr.AppendLine("Es Tramo Valido : " + this.mIsValido.ToString());
                miStr.AppendLine("P1 : " + this.P1.ToString());
                miStr.AppendLine("P2 : " + this.P2.ToString());
                miStr.AppendLine("---------------------------------------------------------");
                miStr.AppendLine("#Valoración Distancia#");
                miStr.AppendLine("Longitud del tramo :  " + this.longitud2d.ToString());
                miStr.AppendLine("Distancia P2 - PtoTarget :  " + this.P2.distTo2d(this.mPtoTarget).ToString());
                miStr.AppendLine("Distancia al eje visibiliad :  " + distanciaAlEje.ToString());
                miStr.AppendLine("Distancia Valoración Global : " + this.valoracionDistanciaLocal.ToString());
                miStr.AppendLine("#Valoración Pendiente#");
                miStr.AppendLine("Valoración Pendiente [Tramo] :  " + this.valoracionPendienteLocal.ToString());
                miStr.AppendLine("#Valoración Implantación#");
                miStr.AppendLine("Valoración Coste Implantacion [Tramo] :  " + this.valoracionCosteImplantacionLocal.ToString());
                miStr.AppendLine("---------------------------------------------------------");
                miStr.AppendLine("Valoración Global Ponderada Tramo [0-10] : " + this.valoracionPonderadaGlobal_0_10.ToString());
                miStr.AppendLine("Valoración Global Distancia [0-10] : " + this.valoracionDistanciaGlobal_0_10.ToString());
                miStr.AppendLine("Valoración Global Pendiente [0-10] : " + this.valoracionPendienteGlobal_0_10.ToString());
                miStr.AppendLine("Valoración Global Coste Implantacion [0-10] : " + this.valoracionCosteImplantacionGlobal_0_10.ToString());
            }
            else
            {
                miStr.AppendLine("Es Tramo Valido : " + this.mIsValido.ToString());
                miStr.AppendLine("Error Tramo : " + this.errorTramo.ToString());
                int i = 0;
                foreach (var tramoF in mTramosFerrocarril)
                {
                    i++;
                    miStr.AppendLine("P1 Apoyo " + i + ":" + tramoF.P1.ToString());
                    miStr.AppendLine("P1 Apoyo " + i + ":" + tramoF.P2.ToString());
                }
            }

            return miStr.ToString();
        }


        private double getP2ZFromPendienteMaximaAbsoluta(double iPendienteMaximaAbsPC, oTramoFerrocarril iTramo)
        {

            double miIncrementoZmaximo = Math.Abs(iPendienteMaximaAbsPC / 100) * this.longitud2d;

            if (iTramo.P1P2terrenoPendienteConSignoPC > 0)
            {
                return iTramo.P1.Z + miIncrementoZmaximo;
            }
            else
            {
                return iTramo.P1.Z - miIncrementoZmaximo;
            }



        }


    }
}
