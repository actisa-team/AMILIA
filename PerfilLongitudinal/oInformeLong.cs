using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerfilLongitudinal
{


    using engNet.ClassT;
    using engNet.Csv;
    using engNet.CustomAtributos;
    using System.ComponentModel;


    public class oInformeLong
    {
        [BindingInfo(SortIndex = 1)]
        [LocalizedDisplayName("pendiente", typeof(strEjeLong))]
        public double? pendiente { get; set; }


        [BindingInfo(SortIndex = 2)]
        [LocalizedDisplayName("longitud", typeof(strEjeLong))]
        public double? longitud { get; set; }


        [BindingInfo(SortIndex = 3)]
        [LocalizedDisplayName("parametro", typeof(strEjeLong))]
        public double? parametro { get; set; }

        [BindingInfo(SortIndex = 4)]
        [LocalizedDisplayName("verticePK", typeof(strEjeLong))]
        public double? verticePK { get; set; }


        [BindingInfo(SortIndex = 5)]
        [LocalizedDisplayName("verticeCota", typeof(strEjeLong))]
        public double? verticeCota { get; set; }

        [BindingInfo(SortIndex = 6)]
        [LocalizedDisplayName("entradaPK", typeof(strEjeLong))]
        public double? entradaPK { get; set; }


        [BindingInfo(SortIndex = 7)]
        [LocalizedDisplayName("entradaCota", typeof(strEjeLong))]
        public double? entradaCota { get; set; }

        [BindingInfo(SortIndex = 8)]
        [LocalizedDisplayName("salidaPK", typeof(strEjeLong))]
        public double? salidaPK { get; set; }

        [BindingInfo(SortIndex = 9)]
        [LocalizedDisplayName("salidaCota", typeof(strEjeLong))]
        public double? salidaCota { get; set; }

        [BindingInfo(SortIndex = 10)]
        [LocalizedDisplayName("difpen", typeof(strEjeLong))]
        public double? difpen { get; set; }

        public oInformeLong(double? iPendiente, double? iLongitud, double? iParametro, double? iVerticePK, double? iVerticeCota, double? iEntradaPK, double? iEntradaCota, double? iSalidaPK, double? iSalidaCota, double? iDifpen)
        {
            if (iPendiente != null) pendiente = Math.Round((double)iPendiente, 3); else pendiente = null;
            if (iLongitud != null) longitud = Math.Round((double)iLongitud, 3); else longitud = null;
            if (iParametro != null) parametro = Math.Round((double)iParametro, 3); else parametro = null;
            if (iVerticePK != null) verticePK = Math.Round((double)iVerticePK, 3); else verticePK = null;
            if (iVerticeCota != null) verticeCota = Math.Round((double)iVerticeCota, 3); else verticeCota = null;
            if (iEntradaPK != null) entradaPK = Math.Round((double)iEntradaPK, 3); else entradaPK = null;
            if (iEntradaCota != null) entradaCota = Math.Round((double)iEntradaCota, 3); else entradaCota = null;
            if (iSalidaPK != null) salidaPK = Math.Round((double)iSalidaPK, 3); else salidaPK = null;
            if (iSalidaCota != null) salidaCota = Math.Round((double)iSalidaCota, 3); else salidaCota = null;
            if (iDifpen != null) difpen = Math.Round((double)iDifpen, 3); else difpen = null;

        }


    }
}
