using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.zonaGis
{



    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;

    using engCadNet;
    using engCadNet.entidades;

    using tadLayLogica.datos.Gis;
    using tadLayData;
    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayLogica.logica.valoracion;
    


    /// <summary>
    /// CLASE ABSTRACTA ZONAS EXPROPiACION
    /// </summary>
    public abstract class oZonaExpropiacion : oZonaGis
    {

        public oZonaExpropiacion(Guid iId)
            : base(iId)
        {

        }


        #region "Propiedades Abstractas"


        public override string clasificacion
        {
            get
            {
                return strFrmGisGeneral.uiClasificacionGeneral;
            }
        }

        public override string block
        {
            get
            {
                return "gisExprop";
            }
        }
        public override int blockAttNum
        {
            get
            {
                return 3;
            }
        }

 
        #endregion


        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// CLASE ABSTRACTA ZONAS PRODUCCION SUELO // VARIABLES SOCIOECONOMICAS // PRIMARIO ; SECUNDARIO ; TERCIARIO
    /// </summary>
    public abstract class oZonaExProduccion : oZonaExpropiacion
    {
        dsBd.tbSocioEconomicosRow mRow = null;


        #region "Constructores

        public oZonaExProduccion(Guid iId)
            : base(iId)
        {

            mRow = oDalTbSocioEconomicas.getZona(iId);
        }
        public oZonaExProduccion(dsBd.tbSocioEconomicosRow iZonaRow)
            : base(iZonaRow.id)
        {
            mRow = iZonaRow;
        }


        #endregion

        #region "Propiedades"
        public dsBd.tbSocioEconomicosRow row
        {
            get
            {
                if (mRow == null)
                {
                    throw new Exception("xRow SocioEconómia es Nula");
                }

                return mRow;
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {
            get
            {
                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();
                miDicAtt.Add(1, row.nombre);
                miDicAtt.Add(2, string.Format(strFrmGisGeneral.attProhibirPaso, oTraductor.traducirSiNo(row.prohibirPaso)));
                if (row.prohibirPaso)
                {
                    miDicAtt.Add(3, string.Empty);
                }
                else
                {
                    miDicAtt.Add(3, string.Format(strFrmGisGeneral.attValoracion, row.valoracion.ToString()));
                }

                return miDicAtt;
            }
        }
        public override eGisGrupos grupo
        {
            get
            {
                return eGisGrupos.SOC;
            }
        }
        public override string nombre
        {
            get
            {
                return mRow.nombre;
            }
        }
        public override Color color
        {
            get
            {
                if (row.prohibirPaso)
                {
                    return dicValoracionColorCad[10];
                }
                else
                {
                    return dicValoracionColorCad[row.valoracion];
                }
            }
        }
        public override bool isZonaNoPaso
        {
            get { return mRow.prohibirPaso; }
        }
        #endregion

        #region "Metodos"
        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            return new oComponentZonaItem(mRow.nombre, mRow.valoracion, iNumeroSeccionPerteneceZona, iNumeroSeccionesTotales);
        }
        #endregion


    }
    /// <summary>
    /// SECTOR PRIMARIO
    /// </summary>
    public class oZonaSectorPrimario : oZonaExProduccion
    {

        #region "Constructor"

        public oZonaSectorPrimario(Guid iId)
            : base(iId)
        {

        }

        public oZonaSectorPrimario(dsBd.tbSocioEconomicosRow iRow)
            : base(iRow)
        {

        }
        #endregion
        #region "Propiedades Abstractas"
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.SECPRI;
            }
        }

        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiSECPRI;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI34;
            }
        }

        #endregion
    }
    /// <summary>
    /// SECTOR SECUNDARIO
    /// </summary>
    public class oZonaSectorSecundario : oZonaExProduccion
    {

        #region "Constructor"

        public oZonaSectorSecundario(Guid iId)
            : base(iId)
        {

        }

        public oZonaSectorSecundario(dsBd.tbSocioEconomicosRow iRow)
            : base(iRow)
        {

        }
        #endregion





        #region "Propiedades Abstractas"
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.SECSEC;
            }
        }

        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiSECSEC;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI34;
            }
        }

        #endregion



    }
    /// <summary>
    /// SECTOR TERCIARIO
    /// </summary>
    public class oZonaSectorTerciario : oZonaExProduccion
    {

        #region "Constructor"

        public oZonaSectorTerciario(Guid iId)
            : base(iId)
        {

        }

        public oZonaSectorTerciario(dsBd.tbSocioEconomicosRow iRow)
            : base(iRow)
        {

        }
        #endregion





        #region "Propiedades Abstractas"
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.SECTER;
            }
        }

        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiSECTER;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI35;
            }
        }

        #endregion



    }


    /// <summary>
    /// CLASE ABSTRACTA ZONAS SUELO  // VARIABLES PATRIMONIALES // URBANO ; NO URBANO ; URBANIZABLE
    /// </summary>
    public abstract class oZonaExSuelo : oZonaExpropiacion
    {
        
        dsBd.tbPatrimonioSueloRow mRow = null;


        public oZonaExSuelo (Guid iId)
            : base(iId)
        {

            mRow = oDalTbPatrimonialesSuelo.getZona(iId);
        }


        public oZonaExSuelo (dsBd.tbPatrimonioSueloRow iZonaRow)
            : base(iZonaRow.id)
        {
            mRow = iZonaRow;
        }


        public dsBd.tbPatrimonioSueloRow row
        {
            get
            {
                if (mRow == null)
                {
                    throw new Exception("xRow Patrimonio Suelo es Nula");
                }

                return mRow;
            }
        }


        #region "Propiedades"
        public override eGisGrupos grupo
        {
            get
            {
                return eGisGrupos.PAT; 
            }
        }
        public override string nombre
        {
            get { return row.nombre; }
        }
        public override bool isZonaNoPaso
        {
            get { return mRow.prohibirPaso; }
        }
        public override Color color
        {
            get
            {
                if (row.prohibirPaso)
                {
                    return dicValoracionColorCad[10];
                }
                else
                {
                    return dicValoracionColorCad[row.valoracion];
                }
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {
            get
            {
                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();
                miDicAtt.Add(1, row.nombre);
                miDicAtt.Add(2, string.Format(strFrmGisGeneral.attProhibirPaso, oTraductor.traducirSiNo(row.prohibirPaso)));
                if (row.prohibirPaso)
                {
                    miDicAtt.Add(3, string.Empty);
                }
                else
                {
                    miDicAtt.Add(3, string.Format(strFrmGisGeneral.attValoracion, row.valoracion.ToString()));
                }

                return miDicAtt;
            }
        }
        #endregion


       


        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            return new oComponentZonaItem(mRow.nombre, mRow.valoracion, iNumeroSeccionPerteneceZona, iNumeroSeccionesTotales);
        }


    }



    /// <summary>
    /// SUELO NO URBANO
    /// </summary>
    public class oZonaSueloNoUrbano : oZonaExSuelo
    {

        #region "Constructor"

        public oZonaSueloNoUrbano(Guid iId)
            : base(iId)
        {

        }

        public oZonaSueloNoUrbano(dsBd.tbPatrimonioSueloRow iRow)
            : base(iRow)
        {

        }
        #endregion
        #region "Propiedades Abstractas"
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.NOURBA;
            }
        }
        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiNOURBA;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI36;
            }
        }

        #endregion

 
    }
    /// <summary>
    /// SUELO URBANO
    /// </summary>
    public class oZonaSueloUrbano : oZonaExSuelo
    {

        #region "Constructor"

        public oZonaSueloUrbano(Guid iId)
            : base(iId)
        {
           
        }

        public oZonaSueloUrbano(dsBd.tbPatrimonioSueloRow iRow)
            : base(iRow)
        {

        }
        #endregion
        #region "Propiedades Abstractas"

        public override eGisZonas code
        {
            get
            {
                return eGisZonas.URBANO; 
            }
        }

        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiURBANO;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI35;
            }
        }

        #endregion
    }

    /// <summary>
    /// SUELO URBANIZABLE
    /// </summary>
    public class oZonaSueloUrbanizable : oZonaExSuelo
    {

        #region "Constructor"

        public oZonaSueloUrbanizable(Guid iId)
            : base(iId)
        {

        }

        public oZonaSueloUrbanizable(dsBd.tbPatrimonioSueloRow iRow)
            : base(iRow)
        {

        }
        #endregion
        #region "Propiedades Abstractas"
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.URBANI;
            }
        }

        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiURBANI;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI36;
            }
        }

        #endregion
    }






}
