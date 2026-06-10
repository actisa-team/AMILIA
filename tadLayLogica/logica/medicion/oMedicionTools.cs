using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.medicion
{
    using tadLayLogica.datos.precios;
    using tadLayData;
    
    public class oMedicionTools  
    {


        private static oMedCapa factoryCapaFirmeArcenAsiento(Guid iIdMaterial,int iCapaOrden,double iMedicion,eCapaCalzada iCapaTipo)
        {

            dsBd.tbMaterialesRow miRow = oDalMateriales.getMaterialId(iIdMaterial);


            if (miRow.tbClasificacionesRow.idGrupo == "PLA")
            {
                return new oMedFirmePlanta(miRow, iCapaOrden, iMedicion);
            }
            else
            {
                if (iCapaTipo == eCapaCalzada.FIR)
                {
                    return new oMedCapaGranularFirme(miRow, iCapaOrden, iMedicion);
                }
                else if (iCapaTipo == eCapaCalzada.ARC)
                {
                    return new oMedCapaGranularArcen(miRow, iCapaOrden, iMedicion);
                }
                else if (iCapaTipo == eCapaCalzada.ASI)
                {
                    return new oMedCapaAsiento(miRow, iCapaOrden, iMedicion);
                }
                else
                {
                    throw new NotImplementedException(iCapaTipo.ToString());
                }

            }

        }

        /// <summary>
        /// Medicion de Arcen con Talud Ambas Caras
        /// </summary>
        public static List<oMedItemModel> getMedicionArcen (double iAreaTotal, Dictionary<int,dsBd.tbCapasRow> iLstCapasArcen)
        {
             List<oMedItemModel> miLstMedicion= new List<oMedItemModel>();
             oMedCapa miArcen = null;
             double miEspesor = (from p in iLstCapasArcen select p.Value.espesorM).Sum();
             double miAncho = iAreaTotal / miEspesor;

             foreach (KeyValuePair<int,dsBd.tbCapasRow>  item in iLstCapasArcen)
             {
                 miArcen = factoryCapaFirmeArcenAsiento(item.Value.idMaterial, item.Value.orden, miAncho * item.Value.espesorM, eCapaCalzada.ARC); 
                 miLstMedicion.Add(miArcen);
             }

            
             return miLstMedicion;

        }
        /// <summary>
        /// Medicion Capa Asiento ; PUEDE NO HABER CAPA DE ASIENTO
        /// </summary>
        public static List<oMedItemModel> getMedicionCapaAsiento(double iAreaTotal, double iAreasEncimaCapaAsiento, double iTaludFirmeH, Dictionary<int, dsBd.tbCapasRow> iLstCapasAsiento, bool iIsMediaSeccion)
        {

            double miAreaBajoCapaFirme = iAreaTotal - iAreasEncimaCapaAsiento;

            List<oMedItemModel> miLstMedicion = new List<oMedItemModel>();

            if (iLstCapasAsiento != null && iLstCapasAsiento.Count > 0)
            {

                miLstMedicion = getMedicionCapasConTalud(miAreaBajoCapaFirme, iTaludFirmeH, iLstCapasAsiento, iIsMediaSeccion, eCapaCalzada.ASI);


                //Añado la Zonas Por Encima de la Capa de Asiento (Los Recrecidos inferiores del arcen);
                miLstMedicion[0].addMedicion(iAreasEncimaCapaAsiento);  
            }

            return miLstMedicion;

        }
        /// <summary>
        ///Medicion de Firme
        /// </summary>
        public static List<oMedItemModel> getMedicionCapasConTalud(double iAreaTotal, double iTaludH, Dictionary<int, dsBd.tbCapasRow> iLstCapasFirme, bool iIsMediaSeccion, eCapaCalzada iCapaMedir)
        {


            double miFactor = 0;

            if (iIsMediaSeccion)
            {
                miFactor = 1;
            }
            else
            {
                miFactor = 2;
            }

            //Obtener el Area de la Capas de Firme Considerando el Talud
            double miAltura = (from p in iLstCapasFirme select p.Value.espesorM).Sum();
            double miTrianguloBase = miAltura * iTaludH;
            double miTrianguloArea = (miAltura * miTrianguloBase) / 2;

            double miAreaSinTriangulo = iAreaTotal - (miFactor * miTrianguloArea);

            //Realizo la Operación Boleana, de sumar las Areas 
            double miCapaRectanguloBase = miAreaSinTriangulo / miAltura;

            double? miCapaRectanguloArea = null;
            double? miCapaTrianguloArea = null;


            Dictionary<int, double> miLstAreasTaludSumar = getTaludAreasTriangulo(iTaludH, iLstCapasFirme);

            Dictionary<int, double> miAreasCapasFirme = new Dictionary<int, double>();


            //Capa Id - Area Considerando Talud
            foreach (KeyValuePair<int, dsBd.tbCapasRow> miCapa in iLstCapasFirme)
            {

                miCapaRectanguloArea = miCapaRectanguloBase * miCapa.Value.espesorM;

                miCapaTrianguloArea = miFactor * miLstAreasTaludSumar[miCapa.Key];

                miAreasCapasFirme.Add(miCapa.Value.orden, (miCapaRectanguloArea.Value + miCapaTrianguloArea.Value));
            }


            //Obtengo el Listado de mediciones
            List<oMedItemModel> miLstMedFirme = new List<oMedItemModel>();
            oMedCapa miMed;


            foreach (KeyValuePair<int, dsBd.tbCapasRow> miCapaFirme in iLstCapasFirme)
            {  
              // factoryCapaFirmeArcenAsiento(item.Key, item.Value.idMaterial, miAncho * item.Value.espesorM, eCapaCalzada.ARC); 
              miMed = factoryCapaFirmeArcenAsiento (miCapaFirme.Value.idMaterial, miCapaFirme.Value.orden, miAreasCapasFirme[miCapaFirme.Value.orden],iCapaMedir);
                    
              miLstMedFirme.Add(miMed);
            }
           
            return miLstMedFirme;
        }


        private static Dictionary<int, double> getTaludAreasTriangulo(double iTaludH, Dictionary<int, dsBd.tbCapasRow> iLstCapasBaseUno)
        {
            Dictionary<int, double> miLstTriangulosAreas = new Dictionary<int, double>();


            double miTrianguloArea=0;
            double miTrianguloBase;
            double miTrianguloAltura;
            double miEspesorSumatorio=0;
            double miAnchoRectangulo=0;
            double miAreaRectangulo=0;

            foreach (KeyValuePair<int, dsBd.tbCapasRow> miEspesor in iLstCapasBaseUno)
            {
  
                 miTrianguloAltura = miEspesor.Value.espesorM;
                 miTrianguloBase = miTrianguloAltura * iTaludH;
                 miEspesorSumatorio = miEspesorSumatorio + miTrianguloAltura;
                 miTrianguloArea = (miTrianguloAltura*miTrianguloBase)/2;
       

                if (miEspesor.Key == 1)
                {
                    miLstTriangulosAreas.Add(miEspesor.Key, miTrianguloArea);
                }
                else
                {
                    miAnchoRectangulo = miEspesorSumatorio * iTaludH;
                    miAreaRectangulo = miAnchoRectangulo * miEspesor.Value.espesorM;

                    miLstTriangulosAreas.Add(miEspesor.Key, miAreaRectangulo-miTrianguloArea);
                }
            }


            return miLstTriangulosAreas;


        }
        
        
        
  
    }
}
