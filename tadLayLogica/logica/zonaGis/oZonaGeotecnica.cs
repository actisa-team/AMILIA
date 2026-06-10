using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.zonaGis
{

    using engNet.Extension.DataRow;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;

    using engCadNet;
    using engCadNet.entidades;


    using engNet.Extension.Double;

    using tadLayLogica.datos.Gis;
    using tadLayData;
    using tadLayLan;
    using tadLayLan.Tdb;

    using tadLayLogica.datos;

    using tadLayLogica.datos.BaseDatos;

    using tadLayLogica.logica.valoracion;
    using tadLayShare;

    /// <summary>
    /// CLASE ABSTRACTA ZONA GEOTECNIA
    /// </summary>
    public abstract class oZonaGeotecnica : oZonaGis
    {



        public oZonaGeotecnica(Guid iId)
            : base(iId)
        {

        }


        #region "Propiedades Abstractas"
        public override eGisGrupos grupo
        {
            get
            {
                return eGisGrupos.GEO; 
            }
        }

        public override string clasificacion
        {
            get
            {
                return strFrmGisGeneral.uiClasificacionGeneral;
            }
        }


        public override string block{get{return "gisGeotec";}}
        public override int blockAttNum{get{return 2;}}
        #endregion


        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// ZONAS MOVIMIENTO TIERRAS
    /// </summary>
    public class oZonaGeoMovimientoTierras : oZonaGeotecnica
    {

        dsBd.tbGeoRow mRow = null;

        Dictionary<int, dsBd.tbCapasRow> mLstCapaFirme = null;
        Dictionary<int, dsBd.tbCapasRow> mLstCapaArcen = null;
        Dictionary<int, dsBd.tbCapasRow> mLstCapaAsiento = null;

        #region "Constructor"

        public oZonaGeoMovimientoTierras(Guid iId)
            : base(iId)
        {
            mRow = oSingletonDsBd.getInstance.getZonaMovimientoTierras(id);
        }

        public oZonaGeoMovimientoTierras(dsBd.tbGeoRow iRow)
            : base(iRow.id)
        {
            mRow = iRow;
        }
        #endregion


        #region "Private Metodos"

        private void getCapas()
        {
            Dictionary<eCapaCalzada, Dictionary<int, dsBd.tbCapasRow>> miDic = new Dictionary<eCapaCalzada, Dictionary<int, dsBd.tbCapasRow>>();
            miDic = oDalGeoTbCapas.getCapasByZona(id);

            mLstCapaFirme = miDic[eCapaCalzada.FIR];
            mLstCapaArcen = miDic[eCapaCalzada.ARC];
            mLstCapaAsiento = miDic[eCapaCalzada.ASI];
        }


        #endregion



        #region "Propiedades Abstractas"
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.MOVTIE;
            }
        }
        public override string nombre
        {
            get
            {
                return mRow.nombre;
            }
        }
        public override bool isZonaNoPaso
        {
            get { return mRow.prohibirPaso; }
        }
        public override Autodesk.AutoCAD.Colors.Color color
        {
            get
            {
                System.Drawing.Color rgb = System.Drawing.Color.FromArgb(mRow.color);
                Color miColorA = Color.FromRgb(rgb.R, rgb.G, rgb.B);

                return miColorA;
            }
        }
        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiMOVTIE;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI33;
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {

            get
            {
                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();
                miDicAtt.Add(1, mRow.nombre);
                miDicAtt.Add(2, string.Format(strFrmGisGeneral.attProhibirPaso, oTraductor.traducirSiNo(mRow.prohibirPaso)));
                return miDicAtt;
            }


        }
        #endregion

        #region "Metodo Ivaloracion"

        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            oValoracionGeotecnia miValoracionGeotecnia = new oValoracionGeotecnia(mRow, iNumeroSeccionPerteneceZona, iNumeroSeccionesTotales);

            return miValoracionGeotecnia.valoracion;
        }


        #endregion

        #region "PropiedadesBaseDatos"
        public dsBd.tbGeoRow row
        {
            get
            {
                if (mRow != null)
                {
                    return mRow;
                }
                else
                {
                    throw new oExPropertieNullValue("Row Movimiento Tierras");
                }
            }
        }
        public Dictionary<int, dsBd.tbCapasRow> capasFirme
        {
            get
            {
                if (mLstCapaFirme == null)
                {
                    getCapas();
                }

                return mLstCapaFirme;

            }

        }
        public Dictionary<int, dsBd.tbCapasRow> capasArcen
        {
            get
            {
                if (mLstCapaArcen == null)
                {
                    getCapas();
                }

                return mLstCapaArcen;

            }

        }
        public Dictionary<int, dsBd.tbCapasRow> capasAsiento
        {
            get
            {
                if (mLstCapaAsiento == null)
                {
                    getCapas();
                }

                return mLstCapaAsiento;
            }
        }
        public double espesorFirme
        {
            get
            {
                return (from p in capasFirme select p.Value.espesorM).Sum().roundOff(2);
            }
        }
        public double espesorArcen
        {
            get
            {
                return (from p in capasArcen select p.Value.espesorM).Sum().roundOff(2);
            }
        }
        public double espesorAsiento
        {
            get
            {
                return (from p in capasAsiento select p.Value.espesorM).Sum().roundOff(2);
            }
        }


        public double getCosteImplantacionRoad()
        {

            double miEspesor = 0;
            double miCosteMaterialEmpleo = 0;
            double miCosteMaterialPrestamo = 0;
            double miCosteMaterialMedio = 0;
            double miCosteCapa = 0;
            double miCosteCapaFirmes = 0;
            double miCosteCapaAsiento = 0;

            //Capas de Firme
            foreach (var item in capasFirme)
            {     
                miEspesor = item.Value.espesorM;
                miCosteMaterialEmpleo = item.Value.tbMaterialesRow.precioPrincipal;
                miCosteMaterialPrestamo = this.getCosteMaterialPrestamo(item.Value.tbMaterialesRow);
                miCosteMaterialMedio = 0.5 * (miCosteMaterialEmpleo + miCosteMaterialPrestamo);

                miCosteCapa = miEspesor * miCosteMaterialMedio;

                miCosteCapaFirmes = miCosteCapaFirmes + miCosteCapa;
            }

            //Capas de Asiento
            foreach (var item in capasAsiento)
            {
                miEspesor = item.Value.espesorM;

                miCosteMaterialEmpleo = item.Value.tbMaterialesRow.precioPrincipal;
                miCosteMaterialPrestamo = this.getCosteMaterialPrestamo(item.Value.tbMaterialesRow);
                miCosteMaterialMedio = 0.5*(miCosteMaterialEmpleo+miCosteMaterialPrestamo);

                miCosteCapa = miEspesor * miCosteMaterialMedio;

                miCosteCapaAsiento = miCosteCapaAsiento + miCosteCapa;
            }

            return miCosteCapaFirmes + miCosteCapaAsiento;

        }


        /// <summary>
        /// Obtener el Precio de Prestamo
        /// </summary>
        /// <remarks>MaterialPRocedentePlanta No tiene Precio Secundario</remarks>
        private double getCosteMaterialPrestamo (dsBd.tbMaterialesRow iRowMaterial)
        {

            if (iRowMaterial.IsprecioSecundarioNull())
            {
                return iRowMaterial.precioPrincipal;
            }
            else
            {
                return iRowMaterial.precioSecundario;
            }
        }


        public void getCosteDesmonteTerraplen(out double iDesmonteCosteUnitario, out double iTerraplenCosteUnitario)
        {

            dsBd.tbMaterialesRow miRowDesmonteMaterial = row.tbMaterialesRowByFK_tbMateriales_tbGeoExcavacion;
            iDesmonteCosteUnitario = (miRowDesmonteMaterial.precioPrincipal + miRowDesmonteMaterial.precioSecundario) / 2;

            dsBd.tbMaterialesRow miRowTerraplenMaterial = this.row.tbMaterialesRowByFK_tbMateriales_tbTerraplenRelleno;
            iTerraplenCosteUnitario = (miRowTerraplenMaterial.precioPrincipal + miRowTerraplenMaterial.precioSecundario) / 2;

        }

        #endregion
    }
    /// <summary>
    /// ZONAS CIMENTACIONES
    /// </summary>
    public class oZonaGeoCimentacion : oZonaGeotecnica
    {

        dsBd.tbCimRow mRow = null;


        #region "Constructor"

        public oZonaGeoCimentacion(Guid iId)
            : base(iId)
        {
            mRow = oDalTbCim.getZona(iId);
        }

        public oZonaGeoCimentacion(dsBd.tbCimRow iRow)
            : base(iRow.id)
        {
            mRow = iRow;
        }

        #endregion


        #region "Propiedades Abstractas"

        public override eGisZonas code
        {
            get
            {
                return eGisZonas.ESTCIM; 
            }
        } 
        public override string nombre
        {
            get
            {
                return mRow.nombre;
            }
        }
        public override bool isZonaNoPaso
        {
            get { return mRow.prohibirPaso; }
        }
        public override Color color
        {
            get
            {
                System.Drawing.Color rgb = System.Drawing.Color.FromArgb(mRow.color);
                Color miColorA = Color.FromRgb(rgb.R, rgb.G, rgb.B);

                return miColorA;
            }
        }

        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiESTCIM;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI32;
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {
            get
            {
                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();
                miDicAtt.Add(1, mRow.nombre);
                miDicAtt.Add(2, string.Format(strFrmGisGeneral.attProhibirPaso, oTraductor.traducirSiNo(mRow.prohibirPaso)));
                return miDicAtt;
            }


        }
        #endregion


        #region "Metodo Ivaloracion"

        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            oValoracionPuentesCimentacion miValoracionGeotecnia = new oValoracionPuentesCimentacion(mRow, iNumeroSeccionPerteneceZona, iNumeroSeccionesTotales);

            return miValoracionGeotecnia.valoracion;
        }
        #endregion

    }
    /// <summary>
    /// ZONAS TUNELES
    /// </summary>
    public class oZonaGeoTuneles : oZonaGeotecnica
    {

        dsBd.tbTunRow mRow = null;


        #region "Constructor"

        public oZonaGeoTuneles(Guid iId)
            : base(iId)
        {
            mRow = oDalTbTun.getZona(iId);
        }

        public oZonaGeoTuneles(dsBd.tbTunRow iRow)
            : base(iRow.id)
        {
            mRow = iRow;
        }

        #endregion


        #region "Propiedades Abstractas"
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.TUNTUN; 
            }
        }
        public override string nombre
        {
            get
            {
                return mRow.nombre;
            }
        }
        public override bool isZonaNoPaso
        {
            get { return false; }
        }


        public bool isAllowTuneles
        {
            get { return mRow.allowTuneles; }
        }
        public override Color color
        {
            get
            {
                System.Drawing.Color rgb = System.Drawing.Color.FromArgb(mRow.color);
                Color miColorA = Color.FromRgb(rgb.R, rgb.G, rgb.B);

                return miColorA;
            }
        }
        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiTUNTUN;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI35;
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {

            get
            {
                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();
                miDicAtt.Add(1, mRow.nombre);
                miDicAtt.Add(2, string.Format(strFrmGisTun.uiProhibirTuneles + ": {0}", oTraductor.traducirSiNo(mRow.allowTuneles)));
                return miDicAtt;
            }


        }
        #endregion
        #region "PropiedadesBaseDatos"
        public dsBd.tbTunRow row
        {
           get
            {
                if (mRow != null)
                {
                    return mRow;
                }
                else
                {
                    throw new oExPropertieNullValue("Row Tuneles is Null");
                }
            }
        }
        public double? costeUnitario ()
        {

            if (this.row.IsidEstMatNull())
            {
                return null;
            }
            else
            {
                return this.row.tbMaterialesRow.precioPrincipal;
            }
            
        }
        #endregion

        #region "Metodo Ivaloracion"

        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            oValoracionZonaGisTuneles miValoracionTuneles = new oValoracionZonaGisTuneles(mRow, iNumeroSeccionPerteneceZona, iNumeroSeccionesTotales);

            return miValoracionTuneles.valoracion;
        }


        #endregion

    }
    /// <summary>
    /// ZONAS PUENTES
    /// </summary>
    public class oZonaGeoEstructuras : oZonaGeotecnica
    {

        dsBd.tbEstRow mRow = null;


        #region "Constructor"

        public oZonaGeoEstructuras(Guid iId)
            : base(iId)
        {
            mRow = oDalTbEst.getZona(iId);
        }

        public oZonaGeoEstructuras(dsBd.tbEstRow iRow)
            : base(iRow.id)
        {
            mRow = iRow;
        }

        #endregion


        #region "Propiedades Abstractas"
        public override eGisGrupos grupo
        {
            get
            {
                return eGisGrupos.PUE;
            }
        }
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.ESTEST;
            }
        }
        public override string nombre
        {
            get
            {
                return mRow.nombre;
            }
        }
        public override bool isZonaNoPaso
        {
            get { return false; }
        }


        public bool isAllowEstructuras
        {
            get { return mRow.allowEstructuras; }
        }
        public override Color color
        {
            get
            {
                System.Drawing.Color rgb = System.Drawing.Color.FromArgb(mRow.color);
                Color miColorA = Color.FromRgb(rgb.R, rgb.G, rgb.B);

                return miColorA;
            }
        }
        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiESTEST;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI34;
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {

            get
            {
                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();
                miDicAtt.Add(1, mRow.nombre);
                miDicAtt.Add(2, string.Format(strFrmGisEst.uiProhibirEst + ": {0}", oTraductor.traducirSiNo(mRow.allowEstructuras)));
                return miDicAtt;
            }


        }
        #endregion


        #region"BaseDatos"

        public dsBd.tbEstRow row
        {

            get
            {
                if (mRow == null)
                {
                    throw new oExPropertieNullValue("dataRow Puente");
                }
                else
                {
                    return mRow;


                }
            }
        }

        public double? costeUnitario()
        {

            if (this.row.IsestMatNull())
            {
                return null;

            }
            else
            {
                return this.row.tbMaterialesRow.precioPrincipal;

            }                  
        }
        public double? alturaMaximaPila()
        {
            if (this.row.IsalturaMaxNull())
            {
                return null;
            }
            else
            {
                return this.row.alturaMax;
            }
        }
        #endregion


        

    }

}
