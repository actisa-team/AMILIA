using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.zonaGis
{

    
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;

    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica.datos.Gis;
    using tadLayLogica.datos.proyecto;
 
    using System.Data;
    using tadLayShare;


    
    public class oFactoryZonaGis
    {

        /// <summary>
        /// Obtenemos Todas las Zonas Gis del Proyecto
        /// </summary>
        public static List<oZonaGis> getZonasGisByProyecto()
        {
            List<string> miLstCapasGis = oTadil.data.Layer.getLstCapasNombreGis;
            List<string> miLstCapasSinEje = new List<string>();

            foreach (string miCapa in miLstCapasGis)
            {
                if (!miCapa.Contains("EJE"))
                {
                    miLstCapasSinEje.Add(miCapa);
                }
            }


            List<Polyline> miLstLwGis = oSs.getSsLwByLayerListAndXdata(miLstCapasSinEje, eXdataKey.zonaGisGuid.ToString());
            List<oZonaGis> miLstZonasGis = getZonasGisFromLstPolineasGis(miLstLwGis);

            return miLstZonasGis;
        }



        public static List<string> getZonasGisVariablesByDwgDistintas(string iIdGrupo)
        {

            List<oZonaGis> miLstZonas = getZonasGisVariablesByDwg(iIdGrupo);

            if (miLstZonas==null || miLstZonas.Count == 0)
            {
                return null;
            }

            //Agrupo las Capas por GRUPO
            var miQuery = miLstZonas.GroupBy(p => p.code).Select(s => s.First());

            List<string> miLstOut = miQuery.ToList().ConvertAll(p => p.code.ToString());

            return miLstOut;        
        }



        /// <summary>
        /// Obtener el Listado de Zonas  GIS por GRUPO ID (GEO,PUE,AMB,CLI,etc..) EN EL DWG
        /// </summary>
        public static List<oZonaGis> getZonasGisVariablesByDwg(string iIdGrupo)
        {


            List<oZonaGis> miLstZonasGis = new List<oZonaGis>();


            //Obtengo todas las Capas
            List<string> miLstCapasZonasByGrupo = new List<string>();

            miLstCapasZonasByGrupo.AddRange(oLayer.getLayerListStartsWith(oZonaGis.capaPrefijo + iIdGrupo, false));


            if (miLstCapasZonasByGrupo == null || miLstCapasZonasByGrupo.Count == 0)
            {
                return null;
            }
            else
            {
                List<Polyline> miLstZonaLw = oSs.getSsLwByLayerListAndXdata(miLstCapasZonasByGrupo, eXdataKey.zonaGisCode.ToString());

                if (miLstZonaLw != null && miLstZonaLw.Count > 0)
                {

                    foreach (Polyline item in miLstZonaLw)
                    {
                        miLstZonasGis.Add(oFactoryZonaGis.createZonaGisFromPolilinea(item));
                    }
                }
            }


            return miLstZonasGis;

        }



        /// <summary>
        /// Obtener el Listado de Zonas  GIS por GRUPO ID (GEO,PUE,AMB,CLI,etc..) AND CODE (MOVTIE, ESTCIM, TUNTUN, ESTEST)
        /// </summary>
        public static List<oZonaGis> getZonasGisProyectoByIdGrupoAndCode(eGisGrupos iIdGrupo,eGisZonas iZonaGisCode)
        {


            List<oZonaGis> miLstZonasGis = new List<oZonaGis>();


            //Obtengo todas las Capas
            List<string> miLstCapasZonas = oTadil.data.Layer.getLstCapasNombreGisByGrupoAndCode(iIdGrupo, iZonaGisCode);

           


            if (miLstCapasZonas == null || miLstCapasZonas.Count == 0)
            {
                return new List<oZonaGis>();
            }
            else
            {
                List<Polyline> miLstZonaLw = oSs.getSsLwByLayerListAndXdata(miLstCapasZonas, eXdataKey.zonaGisCode.ToString());

                if (miLstZonaLw != null && miLstZonaLw.Count > 0)
                {

                    foreach (Polyline item in miLstZonaLw)
                    {
                        miLstZonasGis.Add(oFactoryZonaGis.createZonaGisFromPolilinea(item));
                    }
                }
            }


            return miLstZonasGis;

        }






        /// <summary>
        /// Obtener la Zona Geotecnica en función Punto y Listado Polilineas
        /// Si no Encotramos Polilinea, devolvemos Zona Defecto
        /// </summary>
        /// <param name="iPtoPk">Pto Pk</param>
        /// <param name="iLstZonas">Listado Zonas</param>
        /// <param name="iFiltro">Filtro "ESTEST", "MOVTIE"</param>
        public static  oZonaGis 
            
            createZonaGisGeo (double[] iPto2dPk, List<oZonaGis> iLstZonas, eGisZonas iZonaGisCode)
        {
            return createZonaGisGeo(new Point3d(iPto2dPk[0],iPto2dPk[1],0), iLstZonas, iZonaGisCode);
        }



        /// <summary>
        /// Obtener la Zona Geotecnica en función Punto y Listado Polilineas
        /// Si no Encotramos Polilinea, devolvemos Zona Defecto
        /// </summary>
        /// <param name="iPtoPk">Pto Pk</param>
        /// <param name="iLstZonas">Listado Zonas</param>
        /// <param name="iFiltro">Filtro "ESTEST", "MOVTIE"</param>
        public static oZonaGis createZonaGisGeo(Point3d iPtoPk, List<oZonaGis> iLstZonas, eGisZonas iZonaGisCode)
        {

            oZonaGis miZona = (from p in iLstZonas
                               where p.code == iZonaGisCode && p.isPtoInLwZona(iPtoPk)
                               orderby p.lwZona.Area ascending
                               select p).FirstOrDefault();


            if (miZona == null)
            {
                return createZonaGisGeoDefault(iZonaGisCode);
            }
            else
            {
                return miZona;

            }

        }






        /// <summary>
        /// Obtener la lista de ZonasGisVariables (AMB-CLI-SOC-PAT) del Proyecto
        /// </summary>
        public static List<oZonaGis> getLstZonasGisVariablesProyecto ()
        {

            //1-Obtengo todas las Capas GIS Valoracion AMB-CLI-SOC-PAT
            List<string> miLstCapasGisValoracion = oTadil.data.Layer.getLstCapasGisZonasValoracion();

            //2-Obtengo todas las polilineas que atraviesan mi eje de Trazado
            List<Polyline> miLstLwZonas = oSs.getLstPolilineasFromListadoCapasAndXdataCode(miLstCapasGisValoracion, eXdataKey.zonaGisCode.ToString()); 

            //3-Obtengo las Zonas Gis desde las Polilineas
            List<oZonaGis> miLstZonasGis = getZonasGisFromLstPolineasGis(miLstLwZonas);

            return miLstZonasGis;
        }



        public static List<oZonaGis> getZonasGisFromLstPolineasGis (List<Polyline> iLstPolilineasGis)
        {
            List<oZonaGis> miLstZonasGis = new List<oZonaGis>();

            foreach (var item in iLstPolilineasGis)
            {
                miLstZonasGis.Add(createZonaGisFromPolilinea(item));
            }

            return miLstZonasGis;
        }


        public static oZonaGis createZonaGisFromPolilinea(Polyline iLwZona)
        {

            string miCode = engCadNet.oXdata.getXData( iLwZona, eXdataKey.zonaGisCode.ToString(), null)[0];
            Guid miId = new Guid (engCadNet.oXdata.getXData(iLwZona, eXdataKey.zonaGisGuid.ToString(), null)[0]);

            oZonaGis miZonaGisCad = createZonaGis(miCode, miId);
            miZonaGisCad.lwZona = iLwZona;

            return miZonaGisCad;
    
        }


        /// <summary>
        /// CREAR ZONAS POR DEFECTO (DONDE NO EXISTEN ZONAS GIS DEFINIDAS)
        /// </summary>
        public static oZonaGis createZonaGisGeoDefault(eGisZonas iCode)
        {

            if (iCode== eGisZonas.MOVTIE)
            {
                return new oZonaGeoMovimientoTierras(oDalTbSeccionZonasGenerales.zonaMovimientoTierrasDefault());
            }
            else if (iCode== eGisZonas.TUNTUN)
            {
                return new oZonaGeoTuneles(oDalTbSeccionZonasGenerales.zonaTunelDefault());
            }
            else if (iCode == eGisZonas.ESTEST)
            {
                return new oZonaGeoEstructuras(oDalTbSeccionZonasGenerales.zonaEstructurasDefault());
            }
            else if (iCode == eGisZonas.ESTCIM)
            {
                return new oZonaGeoCimentacion(oDalTbSeccionZonasGenerales.zonaCimentacionDefault());
            }
            else
            {
                throw new oExEnumNotImplemented(iCode.ToString());               
            }    
        }



       public static oZonaGis createZonaGis(string iCode, DataRow iRowZona)
        {

            if (string.Equals(iCode, "MOVTIE"))
            {
                dsBd.tbGeoRow miRow = (dsBd.tbGeoRow)iRowZona;
                return new oZonaGeoMovimientoTierras(miRow);
            }
            else if (string.Equals(iCode, "ESTCIM"))
            {
                dsBd.tbCimRow miRow = (dsBd.tbCimRow)iRowZona;
                return new oZonaGeoCimentacion(miRow);
            }
            else if (string.Equals(iCode, "TUNTUN"))
            {
                dsBd.tbTunRow miRow = (dsBd.tbTunRow)iRowZona;
                return new oZonaGeoTuneles(miRow);
            }
            else if (string.Equals(iCode, "ESTEST"))
            {
                dsBd.tbEstRow miRow = (dsBd.tbEstRow)iRowZona; 
                return new oZonaGeoEstructuras(miRow);
            }
            else if (string.Equals(iCode, "ZODOPU"))
            {
                dsBd.tbDoHiRow miRow = (dsBd.tbDoHiRow)iRowZona;   
                return new oZonaDominioHidraulico(miRow);
            }
            else if (string.Equals(iCode, "CRUINF"))
            {
                dsBd.tbCruceInfraRow miRow = (dsBd.tbCruceInfraRow)iRowZona;
                return new oZonaCruInf(miRow);
            }
            else if (string.Equals(iCode, "SECPRI"))
            {
                dsBd.tbSocioEconomicosRow miRow = (dsBd.tbSocioEconomicosRow)iRowZona;
                return new oZonaSectorPrimario(miRow);
            }
            else if (string.Equals(iCode, "SECSEC"))
            {
                dsBd.tbSocioEconomicosRow miRow = (dsBd.tbSocioEconomicosRow)iRowZona;
                return new oZonaSectorSecundario(miRow);
            }
            else if (string.Equals(iCode, "SECTER"))
            {
                dsBd.tbSocioEconomicosRow miRow = (dsBd.tbSocioEconomicosRow)iRowZona;
                return new oZonaSectorTerciario(miRow);
            }
            else if (string.Equals(iCode, "NOURBA"))
            {
                dsBd.tbPatrimonioSueloRow miRow = (dsBd.tbPatrimonioSueloRow)iRowZona;
                return new oZonaSueloNoUrbano(miRow);
            }
            else if (string.Equals(iCode, "URBANO"))
            {
                dsBd.tbPatrimonioSueloRow miRow = (dsBd.tbPatrimonioSueloRow)iRowZona;
                return new oZonaSueloUrbano(miRow);
            }
            else if (string.Equals(iCode, "URBANI"))
            {
                dsBd.tbPatrimonioSueloRow miRow = (dsBd.tbPatrimonioSueloRow)iRowZona;
                return new oZonaSueloUrbanizable(miRow);
            }
            else
            {
                throw new NotImplementedException("Zona : " + iCode + " No Implementada.");
            }            
        }
       public static oZonaGis createZonaGis(string iCode, Guid iZonaId)
       { 
                 //GEOTECNIA - GEO
                 if (string.Equals(iCode,"MOVTIE"))
                 {
                    return new oZonaGeoMovimientoTierras(iZonaId);
                 }
                 else if (string.Equals(iCode, "ESTCIM"))
                 {
                     return new oZonaGeoCimentacion(iZonaId);
                 }
                 else if (string.Equals(iCode, "TUNTUN"))
                 {
                     return new oZonaGeoTuneles(iZonaId);
                 }
                 //ESTRUCTURAS - EST
                 else if (string.Equals(iCode, "ESTEST"))
                 {
                     return new oZonaGeoEstructuras(iZonaId);
                 }
                 //AMBIENTALES - AMB
                 else if (string.Equals(iCode, "DEZOPR"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "VALSUE"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "VALFAU"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "VALFLO"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZOINPA"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "CAVIIN"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZODOPU"))
                 {
                     return new oZonaDominioHidraulico(iZonaId);
                 }
                 else if (string.Equals(iCode, "ACUIFE"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "PERFAU"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 //CLIMATICAS - CLI
                 else if (string.Equals(iCode, "ZOFUHE"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZONUMB"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZONTOR"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZOLLIN"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZONNEV"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZOFUVI"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZONIDE"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 //SOCIOECONOMICAS - SOC
                 else if (string.Equals(iCode, "SECPRI"))
                 {
                     return new oZonaSectorPrimario(iZonaId);
                 }
                 else if (string.Equals(iCode, "SECSEC"))
                 {
                     return new oZonaSectorSecundario(iZonaId);
                 }
                 else if (string.Equals(iCode, "SECTER"))
                 {
                     return new oZonaSectorTerciario(iZonaId);
                 }
                 //PATRIMONIALES - SOC
                 else if (string.Equals(iCode, "MONPUB"))
                 {
                     return new oZonaClaImg(iZonaId);
                 } 
                 else if (string.Equals(iCode, "URBANO"))
                 {
                     return new oZonaSueloUrbano(iZonaId);
                 }
                 else if (string.Equals(iCode, "URBANI"))
                 {
                     return new oZonaSueloUrbanizable(iZonaId);
                 }
                 else if (string.Equals(iCode, "NOURBA"))
                 {
                     return new oZonaSueloNoUrbano(iZonaId);
                 }
                 else if (string.Equals(iCode, "CRVIPE"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "YACARQ"))
                 {
                     return new oZonaClaImg(iZonaId);
                 }
                 else if (string.Equals(iCode, "ZOESIN"))
                 {
                     return new oZonaClaImg(iZonaId);
                 } 
                 else if (string.Equals(iCode, "CRUINF"))
                 {
                     return new oZonaCruInf(iZonaId);
                 }
                 else if (string.Equals(iCode, "OCUINF"))
                 {
                     return new oZonaClaImg(iZonaId);
                 } 
                 else if (string.Equals(iCode, "OCUMIN"))
                 {
                     return new oZonaClaImg(iZonaId);
                 } 
                 else
                 {
                     throw new NotImplementedException("Zona : " + iCode + " No Implementada.");
                 } 
       }

    }
}
