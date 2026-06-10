using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using engCadNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tadLayData;
using tadLayLan;
using tadLayShare;

namespace tadLayLogica.logica.Adecuacion
{
    public class oZonaAdecuacion
    {
        private static Dictionary<int, Color> mDicValoracionColorCad = null;
        private Polyline mLw = null;
        private Guid mId;
        protected List<ObjectId> lstHandleLinkToLw { get; set; }
        public dsBd.tbAdecuacionRow iRow = null;
        public string capaUser { get; set; }
        public Color color { get; set; }
        public eHatch hatch { get; }
        public string block { get; }
        public int blockAttNum { get; }
        public Dictionary<int, string> listadoAtributos { get; }
        protected string blockFilePath
        {
            get
            {
                return oTadil.data.Files.folderCadGis + block + ".dwg";
            }

        }
        public oZonaAdecuacion(dsBd.tbAdecuacionRow iRow_)
        {
            iRow =iRow_;
        }
        public virtual void link()
        {


            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                //Selecciono la Polilinea
                lwZona = engCadNet.oSs.seleccionUsuario<Polyline>(strUserCad.uiPolilineaSelect, strUserCad.uiPolilineaSelectFail);

                if (lwZona != null)
                {

                    //Modifico la Elevación a Z=0
                    using (oEntidad<Polyline> miEntidad = new oEntidad<Polyline>(lwZona))
                    {
                        miEntidad.open();

                        miEntidad.entidad.Elevation = 0;

                        miEntidad.save();
                    }

                    //Limpio la Zona
                    zonaClear();

                    //Lista ObjLink
                    lstHandleLinkToLw = new List<ObjectId>();
                    if (capaUser==null)
                    {
                        capaUser = "Tadil_Adecuacion_"+iRow.nombre;
                    }
                    //Creo las Capas
                    if (!engCadNet.oLayer.HasLayer(capaUser))
                    {
                        engCadNet.oLayer.addLayer(capaUser, Color.FromColorIndex(ColorMethod.ByLayer, 0).ColorIndex, true);
                    }
                    else
                    {
                        engCadNet.oLayer.current(capaUser);
                    }


                    if (color==null)
                    {
                        System.Drawing.Color rgb = System.Drawing.Color.FromArgb(iRow.color);
                        color = Color.FromRgb(rgb.R, rgb.G, rgb.B);
                    }
                    using (oEntidad<Polyline> miPolilinea = new oEntidad<Polyline>(lwZona))
                    {
                        miPolilinea.open();
                        miPolilinea.entidad.Color = color;
                        miPolilinea.entidad.Layer = capaUser;
                        miPolilinea.save();
                    }

                    try
                    {
                        /*lstHandleLinkToLw.Add(oHatch.addHatch((Entity)lwZona, hatch, color, capaUser));





                        //Tercero Añado el Bloque
                        if (!oBlock.hasBlockDef(block))
                        {
                            oBlock.InsertDwgAsBlockDefAndCheckAtributosNum(blockFilePath, blockAttNum, "");
                        }


                        //ADD BLOQUE
                        lstHandleLinkToLw.Add(oBlock.insertBlockReference(block, oLw.getCDG(lwZona), 0, true, listadoAtributos, 0.5, color, capaUser));
                        */
                        addXdata();
                    }
                    catch (Autodesk.AutoCAD.Runtime.Exception)
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiPolilineaAbiertaErronea);
                        return;
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }

            }

        }
        public Polyline lwZona
        {

            get
            {
                if (mLw == null)
                {
                    throw new oExPropertieNullValue("lwZona");
                }
                else
                {
                    return mLw;
                }

            }

            set
            {
                mLw = value;

            }

        }
        protected virtual void zonaClear()
        {

            List<string> miLstHandle = engCadNet.oXdata.getXData(lwZona, eXdataKey.zonaGisLinkHandle.ToString(), ";");

            if (miLstHandle != null && miLstHandle.Count > 0)
            {
                oTools.entidadDelete(miLstHandle);
            }

        }
        protected virtual void addXdata()
        {

            string miData = string.Empty;

            foreach (ObjectId item in lstHandleLinkToLw)
            {
                miData = miData + ";" + item.Handle;
            }

            engCadNet.oXdata.setXdata(lwZona.ObjectId, eXdataKey.zonaGisLinkHandle.ToString(), miData);
        }
    }
}
