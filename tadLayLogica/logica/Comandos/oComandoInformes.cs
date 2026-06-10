using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Comandos
{

    using tadLayLogica.logica.valoracion;
    using tadLayLogica.datos.proyecto;
    
    
    
    public static  class oComandoInformes
    {
        /// <summary>
        /// INFORME VALORACION TRAZADO PLANTA
        /// </summary>
        public static void createInformeValoracionTrazadoPlanta (Guid iIdSolucion,string iFilePathFull)
        {
            oSolucion misolucion = new oSolucion(iIdSolucion);

            oValoracionTrazadoPlanta miValTrazadoPlanta = new oValoracionTrazadoPlanta(misolucion.ejeTrazado);

            miValTrazadoPlanta.writeCSV(iFilePathFull);

        }
        /// <summary>
        /// INFORME VALORACION TRAZADO ALZADO
        /// </summary>
        public static void createInformeValoracionTrazadoAlzado(Guid iIdSolucion, string iFilePathFull)
        {
            oSolucion misolucion = new oSolucion(iIdSolucion);

            oValoracionTrazadoAlzado miValTrazadoAlzado = new oValoracionTrazadoAlzado(misolucion.ejePerfilRasante);

            miValTrazadoAlzado.writeCSV(iFilePathFull);

        }

        public static void createInformeValoracionTrazadoTiempo(Guid iIdSolucion, string iFilePathFull)
        {
            oSolucion misolucion = new oSolucion(iIdSolucion);
            oValoracionTrazadoTiempo miValoracionTiempo = new oValoracionTrazadoTiempo(misolucion.roadDesign.grupo, misolucion.ejeTrazado, oTadil.data.Files.fileNormasCarreteras, oSingletonDsApp.getInstance.valoracionTrazado.tiempoRecorridoPC);
            miValoracionTiempo.writeCSV(iFilePathFull);
        }
    }
}
