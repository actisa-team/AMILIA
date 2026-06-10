using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.EditorInput;
    
    public class oLayer
    {


        //AÑADO UNA CAPA  |NOMBRE ; COLOR ; ISCURRENT ; MENSAJE|
        public static void addLayer(string iLyName, short iLyColorIndex, bool iLayCur)
        {
         
            if (!HasLayer(iLyName))
            {

            Color myColor = new Color();

            myColor = Color.FromColorIndex(ColorMethod.ByLayer,iLyColorIndex);

            addLayer(iLyName, myColor, iLayCur);
            
            }
           
        }

        public static void addLayer(string iLyName, Color iColor, bool iLayCur)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {
                    // Obtengo la Tabla de la Capas
                    LayerTable acLyTb;
                    LayerTableRecord acLy;
                    ObjectId LyObjId;

                    //Abro la Tabla de Capas
                    acLyTb = tr.GetObject(oCadManager.thisBase.LayerTableId, OpenMode.ForRead) as LayerTable;


                    if (acLyTb.Has(iLyName) != true) // La Capa NO Existe
                    {
                        //Abro la Tabla
                        acLyTb.UpgradeOpen();
                        //Creo un Objeto Layer
                        acLy = new LayerTableRecord();
                        //Cargo los Datos de Mi tabla
                        acLy.Name = iLyName;
                        //Añado el Registro a la Tabla de Capas
                        LyObjId = acLyTb.Add(acLy);
                        //Configuro el Registro de la Capa
                        acLy.Color = iColor;
                        //Actualizo la Transacción
                        tr.AddNewlyCreatedDBObject(acLy, true);
                        //Poner Actual la Capa
                        if (iLayCur == true) { oCadManager.thisBase.Clayer = acLyTb[acLy.Name]; }
                        //Acepto los Cambios en la Transacción
                        tr.Commit();
                        //Devuelvo el ObjectoID de la Capa
                        //return LyObjId;
                    }
                    else // La Capa Existe
                    {
                        //Configuro el Registro Capa como Actual
                        acLy = tr.GetObject(acLyTb[iLyName], OpenMode.ForWrite) as LayerTableRecord;
                        LyObjId = acLy.ObjectId;
                        //Configuro la Capa
                        acLy.IsLocked = false;
                        acLy.IsOff = false;
                        //Poner Actual la Capa
                        if (iLayCur == true) { oCadManager.thisBase.Clayer = acLyTb[acLy.Name]; }
                        //Acepto los Cambios en la Transacción
                        tr.Commit();
                        //Devuelvo el ObjectoID de la Capa
                        //return LyObjId;
                    }

                }
            }
        }





        //FUNCION ; DETERMINO SI LA CAPA EXISTE   
        public static bool HasLayer(string iLyName)
        {
 
            using (Transaction tr = oCadManager.StartTransaction())
            {
                // Obtengo la Tabla de la Capas
                LayerTable myLayerTabla;

                //Abro la Tabla de Capas
                myLayerTabla = tr.GetObject(oCadManager.thisBase.LayerTableId, OpenMode.ForRead) as LayerTable;


                if (myLayerTabla.Has(iLyName))
                {
                    return true;
                }
                else
                {
                    return false;
                }   
            }
        }

        /// <summary>
        /// Activar-Desactivar Capa
        /// </summary>
        public static void vLayerOffOn(string iLayer, bool iLayOff, bool iIsLocked)
        {
            vLayerListActDes(new List<string>(new string[] {iLayer}),iLayOff,iIsLocked);
        }

        public static void current(string iLayer)
        {

                using (Transaction tr = oCadManager.StartTransaction())
                {
                    // Obtengo la Tabla de la Capas
                    LayerTable myLayerTabla;

                    //Abro la Tabla de Capas
                    myLayerTabla = tr.GetObject(oCadManager.thisBase.LayerTableId, OpenMode.ForRead) as LayerTable;


                    LayerTableRecord myAcLayer;


                    if (myLayerTabla.Has(iLayer))
                    {
                        myAcLayer = tr.GetObject(myLayerTabla[iLayer], OpenMode.ForWrite) as LayerTableRecord;
                        oCadManager.thisBase.Clayer = myAcLayer.ObjectId;
                    }
                    else
                    { 
                    
                    
                    
                    }

                        tr.Commit();
                
                     }
            }
        
        
        
       
        /// <summary>
        /// Activar-Desactivar Conjunto Capas
        /// </summary>
        public static void vLayerListActDes(List<string> iLayerList, bool iIsOff,bool iIsLocked)
        {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    // Obtengo la Tabla de la Capas
                    LayerTable myLayerTabla;

                    //Abro la Tabla de Capas
                    myLayerTabla = tr.GetObject(oCadManager.thisBase.LayerTableId, OpenMode.ForRead) as LayerTable;

                    LayerTableRecord myAcLayer;

                    foreach (string myLayName in iLayerList)
                    {

                        if (myLayerTabla.Has(myLayName))
                        {
                            myAcLayer = tr.GetObject(myLayerTabla[myLayName], OpenMode.ForWrite) as LayerTableRecord;
                            myAcLayer.IsOff = iIsOff;
                            myAcLayer.IsLocked = iIsLocked;
                        }

                    }

                    tr.Commit();
                }           
        }

        public static ObjectId getLayerObjId(string iLayerName)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {
                LayerTable myLstLay = tr.GetObject(oCadManager.thisBase.LayerTableId, OpenMode.ForRead) as LayerTable;

                if (myLstLay.Has(iLayerName))
                {
                    return myLstLay[iLayerName];
                }
                else
                {
                    throw new oExLayerNoExiste(iLayerName);
                }
            }
        }





        public static List<string> getLayerListContains (List<string> iLstCapaNameContains,bool iAddXref)
        {

            List<string> miLstCapasNombres = new List<string>();

            List<LayerTableRecord> miLstLayer = getLstLayerByDwg(iAddXref);


            foreach (LayerTableRecord item in miLstLayer)
            {

                foreach (string itemBuscar in iLstCapaNameContains)
                {

                    if (item.Name.Contains(itemBuscar))
                    {
                        miLstCapasNombres.Add(item.Name);
                    }

                }

            }

            return miLstCapasNombres;
        }


        public static List<LayerTableRecord> getLstLayerByDwg (bool iAddXref)
        {

             using (Transaction tr = oCadManager.StartTransaction())
            {
                //Creo el Array
                List<LayerTableRecord> miLstLayer = new List<LayerTableRecord>();

                //Obtengo la Tabla de Capas
                LayerTable miTbLayer = tr.GetObject(oCadManager.thisBase.LayerTableId, OpenMode.ForRead) as LayerTable;
               
                //Creo Objeto Capa
                LayerTableRecord miLayer;

                foreach (ObjectId acObjId in miTbLayer)
                {

                    miLayer= tr.GetObject(acObjId, OpenMode.ForRead) as LayerTableRecord;


                    if (isLayerXref(miLayer.Name))
                    {
                           if (iAddXref)
                           {
                              miLstLayer.Add(miLayer);
                           }
                    }
                    else
                    {
                         miLstLayer.Add(miLayer);      
                    }

                }


                return miLstLayer;

                }
        }


        /// <summary>
        /// Obtener listado de todas las Capas, si iLayStarWith es vacio o null te devuelve todas
        /// </summary>
        /// <param name="iFiltroXref">True-Se Añaden</param>
        /// <returns>Listado de Capas</returns>
        public static List<string> getLayerListStartsWith(string iLayStarWith,  bool iAddXref)
        {

           
            using (Transaction tr = oCadManager.StartTransaction())
            {
                //Creo el Array
                List<string> myLayerList = new List<string>();

                //Obtengo la Tabla de Capas
                LayerTable acLyrTb = tr.GetObject(oCadManager.thisBase.LayerTableId, OpenMode.ForRead) as LayerTable;
               
                //Creo Objeto Capa
                LayerTableRecord acLyrTableRecord;

                foreach (ObjectId acObjId in acLyrTb)
                {

                    acLyrTableRecord = tr.GetObject(acObjId, OpenMode.ForRead) as LayerTableRecord;


                    if (iAddXref) //Añadir Capas Xref
                    {
                        if (string.IsNullOrEmpty(iLayStarWith))
                        {
                            myLayerList.Add(acLyrTableRecord.Name);
                        }
                        else
                        {

                            if (acLyrTableRecord.Name.StartsWith(iLayStarWith))
                            {
                                myLayerList.Add(acLyrTableRecord.Name);
                            }
                        
                        }
                        
                                             
                    }
                    else //Filtar Capas que No son Xref
                    {
                        if (! isLayerXref(acLyrTableRecord.Name))
                        {

                            if (string.IsNullOrEmpty(iLayStarWith))
                            {
                                myLayerList.Add(acLyrTableRecord.Name);
                            }
                            else
                            {
                                                                              
                                if (acLyrTableRecord.Name.StartsWith(iLayStarWith,StringComparison.InvariantCultureIgnoreCase))
                                {
                                    myLayerList.Add(acLyrTableRecord.Name);
                                }
                            }
                        
                        }

                    }
                }

                return myLayerList;
            }


        }








        //LA CAPA CONTINE REFERENCIA EXTERNA
        static bool isLayerXref(string iLyName)
        {

            if (iLyName.Contains("|"))
            { return true; }
            else
            { return false; }


        }



        /// <summary>
        /// Borrar Todas las Entidades Que Comienzan por 
        /// </summary>
        public static void deleteByStartWith(string iLayerStartWith, bool iAddXref)
        {

            //Obtengo el listado de Capas que empiezan iLayerStartWith
            List<string> myLst = oLayer.getLayerListStartsWith(iLayerStartWith, iAddXref);


            if (myLst != null || myLst.Count > 0)
            {
                SelectionSet mySs = oSs.getSsByLayerList(myLst);

                oSs.deleteSs(mySs);
            }
        }


        /// <summary>
        /// Borrar Todas Entidades en un Listado de Capas
        /// </summary>
        public static void deleteByListName(List<string> iLayerList)
        {
            if (iLayerList != null || iLayerList.Count > 0)
            {
                SelectionSet mySs = oSs.getSsByLayerList(iLayerList);

                oSs.deleteSs(mySs);
            }
        }

        /// <summary>
        /// Borrar Todas las Entidades de Una Capa
        /// </summary>
        public static void deleteByLayer(string iLayer)
        { 
           List<string> myLstLay =  new List<string>(new string[]{iLayer});

           deleteByListName(myLstLay);
        }


        /// <summary>
        /// Si no Existe -> Creo la Capa
        /// Si Existe -> Borro todos los Elementos
        /// </summary>
        /// <param name="iLayer"></param>
        public static void createOrDeleteItems(string iLayer, short iColor)
        {

            if (HasLayer(iLayer))
            {
                oLayer.deleteByLayer(iLayer); 
            }
            else
            {
                oLayer.addLayer(iLayer,iColor, true);
            }


        }
        public static List<string> getListLayerName()
        {
            var layers = getLstLayerByDwg(true);
            return layers.Select(layer => layer.Name).ToList();
        }

    }
}
