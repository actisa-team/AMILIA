using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.zonaGis
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;

    using engCadNet;
    using engCadNet.entidades;

    using tadLayLogica.datos.proyecto;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.datos.Gis;
    using tadLayData;
    using tadLayLan;
    using tadLayLan.Tdb;

    using tadLayLogica.logica.valoracion;


    /// <summary>
    /// ZONAS CLASFICACION-IMAGEN
    /// </summary>
    public class oZonaClaImg : oZonaGis
    {

        dsBd.tbZgItemsRow mRow;


        #region "Constructor"

        public oZonaClaImg(Guid iId)
            : base(iId)
        {
            mRow = oSingletonDsBd.getInstance.getZonaClasificacion(iId); 
        }

        #endregion
        #region "Propiedades Abstractas"
        /// <summary>
        /// GRUPO (GEO,PUE,AMB,CLI,SOC,PAT)
        /// </summary>
        public override eGisGrupos grupo
        {
            get
            { 
                   string miGrupo = mRow.GetParentRow("FK_tbZgClasificaciones_tbZgItems").GetParentRow("FK_tbZgFicha_tbZgClasificaciones").GetParentRow("FK_tbZgGrupo_tbZgFicha")["code"].ToString();           
        
                   return (eGisGrupos) Enum.Parse(typeof(eGisGrupos), miGrupo, true);
            }
        }
        /// <summary>
        /// CODE (Ej VALFAU)
        /// </summary>
        public override eGisZonas  code
        {
            get
            {
                string miCode= mRow.GetParentRow("FK_tbZgClasificaciones_tbZgItems").GetParentRow("FK_tbZgFicha_tbZgClasificaciones")["Code"].ToString();

                return (eGisZonas)Enum.Parse(typeof(eGisZonas), miCode, true);
            }
        }
        /// <summary>
        /// CLASIFICACION (Ej Vertebrados, Insectos, etc)
        /// </summary>
        public override string clasificacion
        {
            get
            {
                return mRow.GetParentRow("FK_tbZgClasificaciones_tbZgItems")["nombre"].ToString().Trim();
            }
        }
        /// <summary>
        /// NOMBRE ZONA
        /// </summary>
        public override string nombre
        {
            get
            {
                return mRow.nombre.Trim();
            }
        }
        /// <summary>
        /// ZONA NO PASO
        /// </summary>
        public override bool isZonaNoPaso
        {
            get { return mRow.prohibir; }
        }       
        /// <summary>
        /// COLOR ZONA
        /// </summary>
        public override Color color
        {
            get
            {
                if (isZonaNoPaso)
                {
                    return dicValoracionColorCad[10];
                }
                else
                {
                    return dicValoracionColorCad[mRow.valoracion];
                } 
            }
        }
        /// <summary>
        /// CAPA ZONA
        /// </summary>
        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.ResourceManager.GetString("ui" + code) + "_" + clasificacion;
            }
        }
        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI37;
            }
        }
        public override string block
        {
            get
            {
                return "gisClasImg";
            }
        }
        public override int blockAttNum
        {
            get
            {
                return 4;
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {

            get
            {

                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();

                miDicAtt.Add(1, clasificacion);
                miDicAtt.Add(2, mRow.nombre);
                miDicAtt.Add(3, string.Format(strFrmGisGeneral.attProhibirPaso, oTraductor.traducirSiNo(mRow.prohibir)));
                miDicAtt.Add(4, string.Format(strFrmGisGeneral.attValoracion, mRow.valoracion.ToString()));

                return miDicAtt;
            }


        }
        #endregion


        #region "Metodos Abstractos"

        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            return new oComponentZonaItem(mRow.nombre, mRow.valoracion, iNumeroSeccionPerteneceZona, iNumeroSeccionesTotales);
        }

        #endregion


        public override void link()
        {

                base.link();


                using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                {

                //Add Imagen
                if (!string.IsNullOrWhiteSpace(mRow.imageNombre))
                {
                    ObjectId miId = engCadNet.oImage.addImage(oTadil.data.Files.fileImgGis(mRow.imageNombre), oLw.getCDG(lwZona), 50, capaUser, false);
                    lstHandleLinkToLw.Add(miId);
                }


                addXdata();

               }

        }
    }

}
