using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNex.Net.BussinesObject.Licencia
{
    public class oExLicencia : Exception
    {
        public oExLicencia(string iMensaje)
            : base(iMensaje)
        {

        }
    }
}
