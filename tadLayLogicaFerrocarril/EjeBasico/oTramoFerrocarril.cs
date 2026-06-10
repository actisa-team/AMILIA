using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLogica.logica.EjeBasicoNew;
using tadLayLogica;
using tadLayShare.puntos;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using engCadNet;
using tadLayLogica.zonaGis;

namespace tayLogicaTijera.EjeBasico
{
    public class oTramoFerrocarril: oTramoAbanico
    {


        public Polyline mTrazadoTramo { get; set; }
        //public oP3d mPuntoIntermedio { get; set; }
        public int mTrozoTramoTijera { get; set; }
        private double mSeparacion;
        private static Func<Polyline, bool> mFunIsTramoOnZonaNoPaso = null;


        public oTramoFerrocarril()
        {

        }

        public oTramoFerrocarril(int iIdAbanico, int iIdTramoPosicionAbanico, oTramoFerrocarril iTramoPrevio, oP3d iP2, double iTramoLongitud, oP2d iPtoTarget, oP3d iPtoIntermedio)
        {

            this.tramoPrevio = iTramoPrevio;
            this.idAbanico = iIdAbanico;
            this.idPosicion = iIdTramoPosicionAbanico;
            this.idTramo = this.tramoPrevio.idTramo + 1;

            this.P1 = this.tramoPrevio.P2;
            this.P2 = iP2;

            this.ptoTarget = iPtoTarget;

            this.tramoTipoEjeBasico = eTramoTipoEjeBasico.avanceLargo;
            //this.mPuntoIntermedio = iPtoIntermedio;

            this.lstTramos.Add(this);
        }

        #region "metodos override de oTramoAbanico"


        public override void validarTramoZonasNoPaso()
        {

            if (this.isTramoValido)
            {
                //Tramo No Esta Sobre Zonas No Paso
                bool miIsTramoOnZonaNoPaso = this.fIsTramoOnZonaNoPaso(this.mTrazadoTramo);

                if (miIsTramoOnZonaNoPaso)
                {
                    this.isTramoValido = false;
                    this.errorTramo = eTramoEjeBasicoError.zonaNoPaso;
                }
            }
        }
        /// <summary>
        /// Is Tramo en Zona No Paso
        /// </summary>
        private bool fIsTramoOnZonaNoPaso(Polyline iTrazado)
        {

            if (mFunIsTramoOnZonaNoPaso == null)
            {
                mFunIsTramoOnZonaNoPaso = oSingletonZonaNoPaso.getInstance.isTramoOnZonaNoPasoPolilinea;
            }

            return mFunIsTramoOnZonaNoPaso(iTrazado);


        }


        public override List<oP3d> getLstPointSeccion()
        {

            List<oP3d> miLstPointIntermedios = new List<oP3d>();




            double miDistanciaOrigen;
            oP2d miPtoSeccion2D;
            double miPtoSeccionZ;


            // The example displays the following output to the console:
            //         Value          Ceiling          Floor
            //       
            //          7.03                8              7
            //          7.64                8              7
            //          0.12                1              0


            //Obtenemos numero de Segmentos
            double miSegmentosDecimal = this.mTrazadoTramo.Length / this.lonDiscretizacionProyecto;

            int miSegmentosNum = (int)Math.Ceiling(miSegmentosDecimal);


            //Obtengo el Numero de Puntos Intermedios
            int miPuntosSeccion = miSegmentosNum + 1;

            //Longitud Segemento
            double miSegmentoLongitud = this.mTrazadoTramo.Length / miPuntosSeccion;

            for (int i = 1; i <= miPuntosSeccion; i++)
            {
                //LongitudSegemnto
                miDistanciaOrigen = miSegmentoLongitud * i;

                //Coordenadas del Punto Seccion
                Point3d miPtoSeccion3D = mTrazadoTramo.GetPointAtDist(miDistanciaOrigen);

                miPtoSeccion2D = new oP2d(miPtoSeccion3D.X, miPtoSeccion3D.Y);

                //Obtengo la CoordendaZ -- Como me calculo la z??
                miPtoSeccionZ = this.getP2ZFromP1(miDistanciaOrigen);

                //Añado el Punto a la Colección
                miLstPointIntermedios.Add(new oP3d(miPtoSeccion2D.X, miPtoSeccion2D.Y, miPtoSeccionZ));
            }


            return miLstPointIntermedios;

        }

        public double getP2ZFromP1(double iLonFromOrigen)
        {
            var pendiente = (P2.Z - P1.Z) / mTrazadoTramo.Length;
            return (this.P1.Z) + iLonFromOrigen * pendiente;
        }


        public override string infoProcess()
        {
            return string.Format("Analizando tijera {0} ;  Tramo {1} ; Posición ", this.idAbanico.ToString(), this.idTramo.ToString(), this.idPosicion.ToString());
        }



        #endregion
    }
}
