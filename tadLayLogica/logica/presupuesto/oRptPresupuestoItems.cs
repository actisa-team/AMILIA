using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using engNet.CustomAtributos;
using System.ComponentModel;
using tadLayLan.Tdi;

namespace tadLayLogica.logica.presupuesto
{
   public class oRptPresupuestoItems
   {


       [LocalizedDisplayName("uiCapitulo", typeof(strFrmInformes))]
       [BindingInfo(SortIndex = 5)]
       [DefaultValue(null)]
       public string capitulo { get; set; }


       [LocalizedDisplayName("uiCodigo", typeof(strFrmInformes))]
       [BindingInfo(SortIndex = 6)]
       [DefaultValue(null)]
       public string codigo { get; set; }


       [LocalizedDisplayName("uiOrigen", typeof(strFrmInformes))]
       [BindingInfo(SortIndex = 7)]
       [DefaultValue(null)]
       public string origen { get; set; }



       [LocalizedDisplayName("uiMedicion", typeof(strFrmInformes))]
       [BindingInfo(SortIndex = 4)]
       [DefaultValue(null)]
       public double? medicion { get; set; }


       [LocalizedDisplayName("uiUd", typeof(strFrmInformes))]
       [BindingInfo(SortIndex = 1)]
       [DefaultValue(null)]
       public string  ud { get; set; }


       [LocalizedDisplayName("uiPrecioUnitario", typeof(strFrmInformes))]
       [BindingInfo(SortIndex = 2)]
       [DefaultValue(null)]
       public double? precioUnitario { get; set; }

       [LocalizedDisplayName("uiPrecioTotal", typeof(strFrmInformes))]
       [BindingInfo(SortIndex = 3)]
       [DefaultValue(null)]
       public double? precioTotal { get; set; }         
          

       public oRptPresupuestoItems(string iCapitulo,  string iCodigo, string iOrigen, double? iMedicion,string iUd,double? iPrecioUnitario,double? iPrecioTotal)
       {
          capitulo = iCapitulo;
          codigo = iCodigo;
          origen = iOrigen;
          medicion = iMedicion;
          ud = iUd;
          precioUnitario = iPrecioUnitario;
          precioTotal = iPrecioTotal;
       }


   }
}
