using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{

    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica.logica.valoracion;
    using tadLayLogica.datos.proyecto;
    using engNet.Extension.Double;
    using engNet.Extension.Integer;
    
    public class oValoracionZonaGisTuneles
    {

        private dsBd.tbTunRow mRowZona=null;
        private dsApp.tbValEstructurasTunelesRow mRowValEstructurasTuneles=null;
        private double? mNotaLocalValoracionPC = null;

        private IValoracion mValoracion = null;


        public oValoracionZonaGisTuneles(dsBd.tbTunRow iZonaTunelesRow, int iSecciones, int iSeccionesTotales)
       {
           mRowZona = iZonaTunelesRow;
           mRowValEstructurasTuneles = oSingletonDsApp.getInstance.valoracionEstructuraTunelMuro;
           mNotaLocalValoracionPC = 100 * (iSecciones.toDouble() / iSeccionesTotales.toDouble());
           
       }


        #region "Proiedades"
        public IValoracion valoracion
        {
            get
            {
                if (mValoracion == null)
                {
                    mValoracion = new oCompositeValoracionTunelesZonas(mRowZona.nombre, mNotaLocalValoracionPC.Value);
                    mValoracion.add(getValoracionRmr());
                    mValoracion.add(getValoracionMetodosExcavacion());
                    mValoracion.add(getValoracionTratamientosEspecificos());
                }

                return mValoracion;
            }

        }
        #endregion

        #region "MetodosPrivados"

        private IValoracion getValoracionRmr()
        {
            double miRmr = mRowZona.rmr;

            double miValoracionRmr = ((-10.0 / 100.0) * miRmr) + 10;

            return new oComponetValTunelesRmr(miValoracionRmr, mRowValEstructurasTuneles.tunRmrPC);
        }
        private IValoracion getValoracionMetodosExcavacion()
        {
                
                double miValoracion = mRowZona.tbTunExcMetRow.valoracion;

                return new  oComponentValTunMetodosExcavacion(miValoracion, mRowValEstructurasTuneles.tunMetodosExcavacionPC); 
        }
        private IValoracion getValoracionTratamientosEspecificos()
        {

                double miValoracion = mRowZona.tbTunTipoTratamietoRow.valoracion;

                return new oComponentValTunTratamientosEspecificos(miValoracion, mRowValEstructurasTuneles.tunTratamientosEspecificosPC);
        }


        #endregion

    }
}
