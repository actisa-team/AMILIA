using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan;

namespace engCadNet
{
    public class oExEntidadNoExiste : ApplicationException
    {
        public oExEntidadNoExiste(string iHandle) 
            :base(iHandle)
        {

        }

    }
    public class oExInterseccionMayorUno : ApplicationException
    {
        public oExInterseccionMayorUno()
            : base(strError.eNumInterMayor1)
        {

        }
  
    }
    public class oExInterseccionNull : ApplicationException
    {
        public oExInterseccionNull()
            : base(strError.eNoExistePuntosDeInterseccion)
        {

        }
  
    }
    public class oExSelectPointNull : ApplicationException
    {

        public oExSelectPointNull()

            : base(string.Format(strError.eNullSelection))
        {


        }

    }
    public class oExBloqueNoExiste : ApplicationException
    {

        public oExBloqueNoExiste(string iBloqueName)

            : base(string.Format(strError.eCapaNoExiste, iBloqueName))
        {


        }

    }
    public class oExLayerNoExiste : ApplicationException
    {

        public oExLayerNoExiste(string iVariable)

            : base(string.Format(strError.eCapaNoExiste, iVariable))
        {


        }

    }
    public class oExEjeEstilosLabelNoExiste : ApplicationException
    {

        public oExEjeEstilosLabelNoExiste()

            : base(string.Format(strError.eEstilosEtiquetaEje))
        {


        }

    }
    public class oExEjeEstilosEjeNoExiste : ApplicationException
    {

        public oExEjeEstilosEjeNoExiste()

            : base(string.Format(strError.eEstilosEtiquetaEje))
        {


        }

    }

}
