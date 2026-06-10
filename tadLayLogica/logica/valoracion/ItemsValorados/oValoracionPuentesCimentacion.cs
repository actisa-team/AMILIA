using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdb;

namespace tadLayLogica.logica.valoracion
{

    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica.logica.valoracion;
    using tadLayLogica.datos.proyecto;
    using engNet.Extension.Double;
    using engNet.Extension.Integer;
    

    public class oValoracionPuentesCimentacion
    {

        private dsBd.tbCimRow mRowZona=null;
        private dsApp.tbValEstructurasTunelesRow mRowValEstructuras=null;
        private double? mNotaLocalValoracionPC = null;

        private IValoracion mValoracion = null;


        public oValoracionPuentesCimentacion(dsBd.tbCimRow iRowZona, int iSecciones, int iSeccionesTotales)
       {
           mRowZona = iRowZona;
           mRowValEstructuras = oSingletonDsApp.getInstance.valoracionEstructuraTunelMuro;
           mNotaLocalValoracionPC = 100 * (iSecciones.toDouble() / iSeccionesTotales.toDouble());
           
       }


        #region "Proiedades"
        public IValoracion valoracion
        {
            get
            {
                if (mValoracion == null)
                {
                    mValoracion = new oCompositeValoracionPuentesCimentacionZonas(mRowZona.nombre, mNotaLocalValoracionPC.Value);
                    mValoracion.add(getValoracionCimentacionEstructuras());
                    mValoracion.add(getValoracionExcavacion());
                    mValoracion.add(getValoracionCimentacionObrasMenores());
                    mValoracion.add(getValoracionPresenciaAgua());
                }

                return mValoracion;
            }

        }
        #endregion

        #region "MetodosPrivados"
        private IValoracion getValoracionCimentacionEstructuras()
        {

                string miNombre = strFrmGisCim.ResourceManager.GetString("ui" + mRowZona.idCimViaPue);
                double miValoracion = mRowZona.tbCimTiposRowBytbCimTipos_tbCimEstructuras.valoracion;

                return new oComponetValPuentesCimentacion(miNombre, miValoracion, mRowValEstructuras.estCimentacionEstructurasPC);
            
        }
        private IValoracion getValoracionExcavacion()
        {
                string miNombre = strFrmGisCim.ResourceManager.GetString("ui" + mRowZona.idExcavaMetodo);
                double miValoracion = mRowZona.tbCimTiposRowBytbCimTipos_tbCimExcavacionMetodos.valoracion;

                return new oComponetValPuentesProcedimientosExcavacion(miNombre, miValoracion, mRowValEstructuras.estProcedimientosExcavacionCimientosPC);
            
        }
        private IValoracion getValoracionCimentacionObrasMenores()
        {

                string miNombre = strFrmGisCim.ResourceManager.GetString("ui" + mRowZona.idCimPasInf);
                double miValoracion = mRowZona.tbCimTiposRowBytbCimTipos_tbCimPasosInferiores.valoracion;
          
                return new oComponetValPuentesObrasMenores(miNombre, miValoracion, mRowValEstructuras.estCimentacionObrasMenoresPC);
            
        }
        private IValoracion getValoracionPresenciaAgua()
        {

                string miNombre = strFrmGisCim.ResourceManager.GetString("ui" + mRowZona.idAguaPresencia);
                double miValoracion = mRowZona.tbCimTiposRowBytbCimTipos_tbCimAguaPresencia.valoracion;

                return new oComponetValPuentesPresenciaAgua(miNombre, miValoracion, mRowValEstructuras.estPresenciaAguaPC);
            
        }
        #endregion

    }
}
