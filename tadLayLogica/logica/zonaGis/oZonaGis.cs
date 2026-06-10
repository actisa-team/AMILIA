using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.zonaGis
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Geometry;


    using engCadNet;
    using engCadNet.entidades;
   

    using tadLayLogica.datos.Gis;
    using tadLayData;
    using tadLayLan;

    using tadLayLogica.logica.valoracion;
    using tadLayShare;

    public abstract class oZonaGis
    {
        private static Dictionary<int, Color> mDicValoracionColorCad = null;
        private Polyline mLw= null;
        private Guid mId ;
      

       public oZonaGis(Guid iId)
       {
           mId = iId;
       }


       #region "PropiedadesEstaticas"

       /// Dicccionario Valoracion - ColorIndexAutocad
       protected static Dictionary<int, Color> dicValoracionColorCad
       {

           get
           {
               if (mDicValoracionColorCad == null)
               {
                   mDicValoracionColorCad = new Dictionary<int, Color>();

                   mDicValoracionColorCad.Add(10, Color.FromColorIndex(ColorMethod.ByLayer, 10));
                   mDicValoracionColorCad.Add(9, Color.FromColorIndex(ColorMethod.ByLayer, 20));
                   mDicValoracionColorCad.Add(8, Color.FromColorIndex(ColorMethod.ByLayer, 30));
                   mDicValoracionColorCad.Add(7, Color.FromColorIndex(ColorMethod.ByLayer, 40));
                   mDicValoracionColorCad.Add(6, Color.FromColorIndex(ColorMethod.ByLayer, 41));
                   mDicValoracionColorCad.Add(5, Color.FromColorIndex(ColorMethod.ByLayer, 50));
                   mDicValoracionColorCad.Add(4, Color.FromColorIndex(ColorMethod.ByLayer, 60));
                   mDicValoracionColorCad.Add(3, Color.FromColorIndex(ColorMethod.ByLayer, 140));
                   mDicValoracionColorCad.Add(2, Color.FromColorIndex(ColorMethod.ByLayer, 150));
                   mDicValoracionColorCad.Add(1, Color.FromColorIndex(ColorMethod.ByLayer, 160));
                   mDicValoracionColorCad.Add(0, Color.FromColorIndex(ColorMethod.ByLayer, 5));

               }


               return mDicValoracionColorCad;

           }

       }
       #endregion


       #region "Propiedades Publicas"
       public Guid id
       {
           get
           {
               if (mId == null)
               {
                   throw new oExPropertieNullValue("IdZona");
               }
               else
               {
                   return mId;
               }
           }

           set
           {
               mId = value;
           
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
       public static string capaPrefijo
       {
           get
           {
               return oTadil.data.Layer.prefijoCapasZonasGis;
           }

       }
       protected string capaApp
       {
           get
           {
               return capaPrefijo + grupo + "_" + code;
           }
       }
       protected string blockFilePath
       {
           get
           {
               return oTadil.data.Files.folderCadGis + block + ".dwg";
           }

       }
       protected List<ObjectId> lstHandleLinkToLw { get; set; }
       #endregion

       #region "Propiedades Abstractas"

       public abstract eGisGrupos grupo { get; }
       public abstract eGisZonas code { get; }
       public abstract string clasificacion { get; }
       public abstract string nombre { get; }
       public abstract bool isZonaNoPaso { get; }
      

      

       public abstract string capaUser { get; }
       public abstract Color color { get; }
       public abstract eHatch hatch { get; }
       public abstract string block { get; }
       public abstract int blockAttNum { get; }
       public abstract Dictionary<int, string> listadoAtributos { get; }


       #endregion

       #region "Metodos Abstratos"

       public abstract IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales);

       #endregion

       #region "Metodos Publicos"
       public bool isPtoInLwZona(Point3d iPto)
       {
           return oPolygon.isPointInPolygon(lwZona, iPto.X, iPto.Y);  
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

                   //Creo las Capas
                   if (!engCadNet.oLayer.HasLayer(capaUser))
                   {
                       engCadNet.oLayer.addLayer(capaUser, Color.FromColorIndex(ColorMethod.ByLayer, 0).ColorIndex, true);
                   }
                   else
                   {
                       engCadNet.oLayer.current(capaUser);
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
                       lstHandleLinkToLw.Add(oHatch.addHatch((Entity)lwZona, hatch, color, capaUser));





                       //Tercero Añado el Bloque
                       if (!oBlock.hasBlockDef(block))
                       {
                           oBlock.InsertDwgAsBlockDefAndCheckAtributosNum(blockFilePath, blockAttNum, "");
                       }


                       //ADD BLOQUE
                       lstHandleLinkToLw.Add(oBlock.insertBlockReference(block, oLw.getCDG(lwZona), 0, true, listadoAtributos, 0.5, color, capaUser));

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
       #endregion



       #region "Metodos Protegidos"

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

           //GIS CODE        
           engCadNet.oXdata.setXdata(lwZona.ObjectId, eXdataKey.zonaGisCode.ToString(), code.ToString());

           //GUID
           engCadNet.oXdata.setXdata(lwZona.ObjectId, eXdataKey.zonaGisGuid.ToString(), id.ToString());


           string miData = string.Empty;

           foreach (ObjectId item in lstHandleLinkToLw)
           {
               miData = miData + ";" + item.Handle;
           }

           engCadNet.oXdata.setXdata(lwZona.ObjectId, eXdataKey.zonaGisLinkHandle.ToString(), miData);
       }

       #endregion
 
    }


}
