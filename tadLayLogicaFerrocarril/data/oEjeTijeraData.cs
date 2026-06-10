namespace tayLogicaTijera.data
{
    public class oEjeTijeraData
    {
        public bool IsAutocorrecion;
        public int NumAutocorrecion;
        public bool RecalcularEjeVisibilidad;
        public double CoefPrioRectas;
        public bool IsEnvolvente;
        public double DistEntronquePC;
        public double DistEntronque;
        public double DistConvergencia;
        public bool PenalizaTramosCortosEntronque;
        public double CoefDistTramo;
        public double CoefDistEje;

        public oEjeTijeraData(bool iIsAutocorrecion,
            int iNumAutocorrecion,
            bool iRecalcularEjeVisibilidad,
            double iCoefPrioRectas,
            bool iIsEnvolvente,
            double iDistEntronquePC,
            double iDistEntronque,
            double iDistConvergencia,
            bool iPenalizaTramosCortosEntronque,
            double iCoefDistTramo,
            double iCoefDistEje)
        {
            IsAutocorrecion = iIsAutocorrecion;
            NumAutocorrecion = iNumAutocorrecion;
            RecalcularEjeVisibilidad = iRecalcularEjeVisibilidad;
            CoefPrioRectas = iCoefPrioRectas;
            IsEnvolvente = iIsEnvolvente;
            DistEntronquePC = iDistEntronquePC;
            DistEntronque = iDistEntronque;
            PenalizaTramosCortosEntronque = iPenalizaTramosCortosEntronque;
            CoefDistTramo = iCoefDistTramo;
            CoefDistEje = iCoefDistEje;
            DistConvergencia = iDistConvergencia;

        }
    }
}