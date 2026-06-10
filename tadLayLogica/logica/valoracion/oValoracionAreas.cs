using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{
    
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;

    using System.ComponentModel;
    
    public class oValoracionAreas
    {
        #region "Constructores"


        public oValoracionAreas(dsApp.tbMatrizDecisionRow iRowTipado)
        {
            getValoracionArea(iRowTipado);
        }

        public oValoracionAreas(int iIdHipotesis)
        {
            dsApp.tbMatrizDecisionRow miRow = oSingletonDsApp.getInstance.getHipotesisValoracion(iIdHipotesis);

            getValoracionArea(miRow);
        }

        #endregion
        #region "Propiedades"


       public string nombre { get; private set; }
       public string descripcion { get; private set; }
       [DefaultValue(null)]
       public double? trazadoPC { get; private set; }
       [DefaultValue(null)]
       public double? geotecniaPC { get; private set; }
       [DefaultValue(null)]
       public double? estructuraTunelMuroPC { get; private set; }
       [DefaultValue(null)]
       public double? medioAmbientalPC { get; private set; }
       [DefaultValue(null)]
       public double? climaticaPC { get; private set; }
       [DefaultValue(null)]
       public double? socioEconomicasPC { get; private set; }
       [DefaultValue(null)]
       public double? patrimonialesPC { get; private set; }
       [DefaultValue(null)]
       public double? economicasPC { get; private set; }
        #endregion
        #region "Metodos Privados"

       private void getValoracionArea(dsApp.tbMatrizDecisionRow iRow)
       {
           nombre = iRow.nombre;
           descripcion = iRow.descripcion;
           trazadoPC = iRow.valoracionTrazadoPC;
           geotecniaPC = iRow.valoracionGeotecniaPC;
           estructuraTunelMuroPC = iRow.valoracionEstructurasTunelesMurosPC;
           medioAmbientalPC = iRow.valoracionMedioAmbientalPC;
           climaticaPC = iRow.valoracionClimaticasPC;
           socioEconomicasPC = iRow.valoracionSocioEconomicasPC;
           patrimonialesPC = iRow.valoracionPatrimonialesPC;
           economicasPC = iRow.valoracionEconomicaPC;
       }

       #endregion
    }
}
