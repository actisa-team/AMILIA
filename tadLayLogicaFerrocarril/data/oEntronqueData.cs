using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tadLayLogica.logica.EjeBasicoNew;

namespace tayLogicaTijera.data
{
    public class oEntronqueData
    {
        public int IdLastTijera;
        public int IdLastTramo;
        public double RadioEntronque;
        public double Velocidad;
        public oTramosValoracion TramosValoracion;
        public double CotaIni;
        public double CotaFin;
        public bool IsCurvaGranRadio;
        public double CoefDistTramo;
        public double CoefDistEje;
        public double DistanciaMinimaEntradaSalida;

        public oEntronqueData(int iIdLastTijera,
            int iIdLastTramo,
            double iRadioEntronque,
            double iVelocidad,
            oTramosValoracion iTramosValoracion,
            double iCotaIni,
            double iCotaFin,
            bool iIsCurvaGranRadio,
            double iCoefDistTramo,
            double iCoefDistEje, double iDistanciaMinimaES)
        {

            IdLastTijera = iIdLastTijera;
            IdLastTramo = iIdLastTramo;
            RadioEntronque = iRadioEntronque;
            Velocidad = iVelocidad;
            TramosValoracion = iTramosValoracion;
            CotaIni = iCotaIni;
            CotaFin = iCotaFin;
            IsCurvaGranRadio = iIsCurvaGranRadio;
            CoefDistTramo = iCoefDistTramo;
            CoefDistEje = iCoefDistEje;
            DistanciaMinimaEntradaSalida = iDistanciaMinimaES;
        }


    }
}
