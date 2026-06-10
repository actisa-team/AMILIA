using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.EjeBasicoNew
{
    using engNet.Extension.Enumeraciones;
    using tadLayShare;

    public class oExTramoError : Exception
    {
        public oExTramoError(eTramoEjeBasicoError iTramoError)
            : base(iTramoError.descriptionEnum<eTramoEjeBasicoError>())
        {
            

        }
    }
}
