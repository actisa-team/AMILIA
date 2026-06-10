using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{

    using engNet.Extension.Double;
    
    /// <summary>
    /// VALORACION DE TRAMOS
    /// </summary>
    public class oTramosValoracion
    {
        #region "CONSTRUCTOR"

        public oTramosValoracion(double iValDisPC, double iValPendientePC, double iValCosteImplantacionPC)
        {
            this.valoracionDistanciaPC = iValDisPC;
            this.valoracionPendientePC = iValPendientePC;
            this.valoracionCosteImplantacionPC = iValCosteImplantacionPC;
        }

        #endregion
        #region "PROPIEDADES"

        public double valoracionDistanciaPC { get; private set; }
        public double valoracionPendientePC { get; private set; }
        public double valoracionCosteImplantacionPC { get; private set; }

        #endregion
        #region "METODOS PUBLICOS"

        /// <summary>
        /// Obtenemos la Nota Pondera del Tramo , partiendo de sus notas Globales 0_10
        /// </summary>
        /// <param name="iValGlobalDistancia_0_10">Valoracion Global 0-10 Distancia</param>
        /// <param name="iValGlobalPendiente_0_10">Valoracion Global 0-10 Pendiente</param>
        /// <param name="iValImplantacion_0_10">Valoracion Global 0-10 Pendiente</param>
        public double getValoracionGlobalPonderada(double iValGlobalDistancia_0_10, double iValGlobalPendiente_0_10, double iValImplantacion_0_10)
        {

            double miValDistancia = (this.valoracionDistanciaPC / 100) * iValGlobalDistancia_0_10;
            double miValPendiente = (this.valoracionPendientePC / 100) * iValGlobalPendiente_0_10;
            double miValCoste = (this.valoracionCosteImplantacionPC / 100) * iValImplantacion_0_10;

            double miSuma = miValDistancia + miValPendiente + miValCoste;

            if (miSuma > 10)
            {
                throw new Exception(string.Format("La Valoracion Global Ponderada del Tramo no puede ser superior a 10 \nValoracionGlobalPonderada {0}", miSuma.roundOff(3).ToString()));
            }

            return miSuma;
        }


        #endregion
    }
}
