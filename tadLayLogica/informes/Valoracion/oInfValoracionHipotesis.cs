using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.informes.Valoracion
{

    using tadLayLogica.logica.valoracion;
    using engNet.ClassT;
    using tadLayLan.Tdi;
    
    
    public class oInfValoracionHipotesis
    {
        public static void printDetalle(oCompositeValoracionHipotesis iRootHipotesis, string iFileToSave)
        {

            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oValDesT<string, string>> miLstFooter = new List<oValDesT<string, string>>();

            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiValoracionHipotesisDetalle, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiHipotesis, iRootHipotesis.nombre));
            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));

            tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
                                                         IValoracion,
                                                         oValDesT<string, string>>(miLstHeader, iRootHipotesis.getDescendientes(), miLstFooter, iFileToSave);

        }
        public static void printResumen(oCompositeValoracionHipotesis iRootHipotesis, string iFileToSave)
        {

            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oValDesT<string, string>> miLstFooter = new List<oValDesT<string, string>>();

            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiValoracionHipotesisResumen, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiHipotesis, iRootHipotesis.nombre));
            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));



            List<IValoracion> miLstRegistros = new List<IValoracion>();

            foreach (IValoracion solucion in iRootHipotesis.lstChild)
            {
                miLstRegistros.Add(solucion);

                List<IValoracion> miCapitulos = solucion.lstChild;

                foreach (IValoracion item in miCapitulos)
                {
                    miLstRegistros.Add(item);

                    miLstRegistros.AddRange(item.lstChild);
                }

            }


            tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
                                                         IValoracion,
                                                         oValDesT<string, string>>(miLstHeader, miLstRegistros, miLstFooter, iFileToSave);

        }
    }
}
