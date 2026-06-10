using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria
{


    using tadLayData;
    using tadLayLogica.datos.Secciones;
    using tadLayLogica.Secciones.Geometria;
    using tadLayShare;
    
    public  class oFactorySeccionCunetas
    {


        public static oCunetaAbstract createCuneta(eCunetaTipo iCunetaTipo, Guid iIdCunetaGeo, Guid iIdCunetaMaterial)
        {

            oCunetaAbstract miCuneta;
            
            
            if (iCunetaTipo == eCunetaTipo.TRIANG)
            {

                dsBd.tbCunetaTriangRow miRow = oDalTabCunTri.getById(iIdCunetaGeo);

                miCuneta = new oCunetaTriangularModel(iIdCunetaMaterial,miRow.anchoIntSupM, miRow.espesorM, miRow.alturaIntM);

                return miCuneta;

            }
            else if (iCunetaTipo == eCunetaTipo.TRAPEZ)
            {
                dsBd.tbCunetaTrapezRow miRow = oDalTabCunTra.getRowById(iIdCunetaGeo);

                miCuneta = new oCunetaTrapezoidalModel(iIdCunetaMaterial,miRow.anchoIntSupM, miRow.anchoIntInfM, miRow.espesorM, miRow.alturaIntM);

                return miCuneta;
            }
            else
            {
                throw new oExEnumNotImplemented(iCunetaTipo.ToString());
            }
   
        }

    }
}
