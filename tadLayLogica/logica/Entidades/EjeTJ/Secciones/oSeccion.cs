using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tadLayLogica.EjeTJ.Secciones
{
    using System.ComponentModel;
    using engNet.CustomAtributos;
    using tadLayLan.Tdi;
    
    public class oSeccion
    {
        [Browsable(false)]
        public int Id { get; set; }

        [LocalizedDisplayName("uiTramo", typeof(strFrmInformes))]
        public int tramo { get; set; }

        [LocalizedDisplayName("uiPk", typeof(strFrmInformes))]
        public double Pk { get; set; }

        [LocalizedDisplayName("uiIncZ", typeof(strFrmInformes))]
        public double ZDesfaseAbs { get; set; }

        [LocalizedDisplayName("uiTerrenoCorrecion", typeof(strFrmInformes))]
        public eExcavacion TerrenoCorreccion { get; set; }
        [Browsable(false)]
        public eApoyo? Apoyo { get; set; }


        public oSeccion(int iIdTramo, int iId, double iPk, double iZrasante, double iZTerreno, bool iHasEstructuras, double iTerraplenDesmonteMax)
        {
            tramo = iIdTramo + 1;
            Id = iId;
            Pk = Math.Round(iPk, 0);
            ZDesfaseAbs = Math.Round(Math.Abs(iZrasante - iZTerreno), 2);
            TerrenoCorreccion = oRoadGeo.getTerrenoCorrecion(iZTerreno, iZrasante);
            Apoyo = oRoadGeo.getApoyo(iHasEstructuras, ZDesfaseAbs, TerrenoCorreccion, iTerraplenDesmonteMax);
        }


        public override string ToString()
        {
            return strFrmInformes.uiId +": " + Id.ToString() + " ; " +
                   strFrmInformes.uiPk+": " + Pk.ToString() + " ; " +
                   strFrmInformes.uiDesfase+": " + ZDesfaseAbs.ToString() + " ; " +
                   strFrmInformes.uiTerrenoCorrecion+":  " + TerrenoCorreccion.ToString() + " ; " +
                   strFrmInformes.uiApoyo+":  " + Apoyo.ToString();

        }





    }


}
