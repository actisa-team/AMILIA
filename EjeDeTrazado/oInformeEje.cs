using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using engNet.CustomAtributos;
using tadLayLan.Tdi;

namespace EjeDeTrazado
{
    public class oInformeEje
    {
/*
        [BindingInfo(SortIndex = 1)]
        [LocalizedDisplayName("segmento", typeof(strEje))]
        public int? segmento { get; set; }



        [BindingInfo(SortIndex = 2)]
        [LocalizedDisplayName("vertice", typeof(strEje))]
        public int? vertice { get; set; }
*/
        [BindingInfo(SortIndex = 3)]
        [LocalizedDisplayName("elemento", typeof(strEje))]
        public string elemento { get; set; }

/*
        [BindingInfo(SortIndex = 4)]
        [LocalizedDisplayName("tipo", typeof(strEje))]
        public string tipo { get; set; }
*/
        //[Browsable(false)]
        //public double campo3hide { get; set; }


        [BindingInfo(SortIndex = 5)]
        [LocalizedDisplayName("azimutfinal", typeof(strEje))]
        public double? azimutfinal { get; set; }


        [BindingInfo(SortIndex = 6)]
        [LocalizedDisplayName("puntoinix", typeof(strEje))]
        public double puntoinix { get; set; }


        [BindingInfo(SortIndex = 7)]
        [LocalizedDisplayName("puntoiniy", typeof(strEje))]
        public double puntoiniy { get; set; }

        [BindingInfo(SortIndex = 8)]
        [LocalizedDisplayName("puntofinx", typeof(strEje))]
        public double puntofinx { get; set; }

        [BindingInfo(SortIndex = 9)]
        [LocalizedDisplayName("puntofiny", typeof(strEje))]
        public double puntofiny { get; set; }

        [BindingInfo(SortIndex = 10)]
        [LocalizedDisplayName("centrocurvax", typeof(strEje))]
        public double? centrocurvax { get; set; }

        [BindingInfo(SortIndex = 11)]
        [LocalizedDisplayName("centrocurvay", typeof(strEje))]
        public double? centrocurvay { get; set; }


        [BindingInfo(SortIndex = 12)]
        [LocalizedDisplayName("radio", typeof(strEje))]
        public double? radio { get; set; }

        [BindingInfo(SortIndex = 13)]
        [LocalizedDisplayName("sentgiro", typeof(strEje))]
        public string sentgiro { get; set; }

        [BindingInfo(SortIndex = 14)]
        [LocalizedDisplayName("aclotoide", typeof(strEje))]
        public double? aclotoide { get; set; }

        [BindingInfo(SortIndex = 15)]
        [LocalizedDisplayName("longitud", typeof(strEje))]
        public double longitud { get; set; }

        [BindingInfo(SortIndex = 16)]
        [LocalizedDisplayName("pkini", typeof(strEje))]
        public double pkini { get; set; }

        [BindingInfo(SortIndex = 17)]
        [LocalizedDisplayName("pkfin", typeof(strEje))]
        public double pkfin { get; set; }

        [Browsable(false)]
        [BindingInfo(SortIndex = 18)]
        [LocalizedDisplayName("peralte", typeof(strEje))]
        public double? peralte { get; set; }

        [Browsable(false)]
        [BindingInfo(SortIndex = 19)]
        [LocalizedDisplayName("margenizq", typeof(strEje))]
        public double? margenizq { get; set; }

        [Browsable(false)]
        [BindingInfo(SortIndex = 20)]
        [LocalizedDisplayName("margender", typeof(strEje))]
        public double? margender { get; set; }

        [Browsable(false)]
        [BindingInfo(SortIndex = 21)]
        [LocalizedDisplayName("varimi", typeof(strEje))]
        public double? varimi { get; set; }

        [Browsable(false)]
        [BindingInfo(SortIndex = 22)]
        [LocalizedDisplayName("varmd", typeof(strEje))]
        public double? varmd { get; set; }

        



        public oInformeEje (int? iSegmento, int? iVertice, string iElemento, string iTipo, double? iAzimutfinal, double iPuntoinix, double iPuntoiniy, double iPuntofinx, double iPuntofiny,
            double? iCentrocurvax, double? iCentrocurvay, double? iRadio, string iSentgiro, double? iAclotoide,  double iLongitud, double iPkini, double iPkfin, 
                double? iPeralte, double? iMargenizq, double? iMargender, double? iVarimi, double? iVarmd)
        {
            //segmento = iSegmento;
            //vertice = iVertice;
            elemento = iElemento;
//            tipo = iTipo;
            azimutfinal = iAzimutfinal;
            if (azimutfinal != null) azimutfinal = Math.Round((double)azimutfinal, 4);
            puntoinix = Math.Round(iPuntoinix, 4);
            puntoiniy = Math.Round(iPuntoiniy, 4);
            puntofinx = Math.Round(iPuntofinx, 4);
            puntofiny = Math.Round(iPuntofiny, 4);
            centrocurvax = iCentrocurvax;
            if (centrocurvax != null) centrocurvax = Math.Round((double)centrocurvax, 4);
            centrocurvay = iCentrocurvay;
            if (centrocurvay != null) centrocurvay = Math.Round((double)centrocurvay, 4);
            radio = iRadio;
            if (radio != null) radio = Math.Round((double)radio, 4);
            if (iSentgiro == "Horario")
            {
                sentgiro = strFrmInformes.uiHorario;
            }
            else if (iSentgiro == "Antihorario")
            {
                sentgiro = strFrmInformes.uiAntihorario;
            }
            aclotoide = iAclotoide;
            if (aclotoide != null) aclotoide = Math.Round((double)aclotoide, 4);
            longitud = Math.Round(iLongitud, 4);
            pkini = Math.Round(iPkini, 4);
            pkfin = Math.Round(iPkfin, 4);
            peralte = iPeralte;
            if (peralte != null) peralte = Math.Round((double)peralte, 4);
            margenizq = iMargenizq;
            if (margenizq != null) margenizq = Math.Round((double)margenizq, 4);
            margender = iMargender;
            if (margender != null) margender = Math.Round((double)margender, 4);
            varimi = iVarimi;
            if (varimi != null) varimi = Math.Round((double)varimi, 4);
            varmd = iVarmd;
            if (varmd != null) varmd = Math.Round((double)varmd, 4);

        }

    }

}
