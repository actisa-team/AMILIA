using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engCadNet
{

    using System.IO;


    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using tadLayLan;
    
    
    public class oBlock
    {


        public static void insertBlockReference (string iBloqueNameConExtension,string iFolderPath, Point3d iPtoInsert, string iLayer, Matrix3d iMatriz)
        {

            //Nombre Sin Extension
            string miBloqueNombreSinExtension = Path.GetFileNameWithoutExtension(iBloqueNameConExtension);

            //Compruebo que la referencia se encuentra en el dwg
            if (!engCadNet.oBlock.hasBlockDef(miBloqueNombreSinExtension))
            {
                string miRuta = iFolderPath + iBloqueNameConExtension;

                engCadNet.oBlock.InsertDwgAsBlockDefAndCheckAtributosNum(miRuta, 0, string.Empty);

            }


            //Inserto la Referencia, en las Coordenadas 0,0,0 y con la matriz de transformación la situo en la guitarra
            ObjectId miBloqueId = engCadNet.oBlock.insertBlockReference(miBloqueNombreSinExtension, iPtoInsert, 0, 1, iLayer);

            using (oEntidad<BlockReference> miBloqueCAD = new oEntidad<BlockReference>(miBloqueId))
            {
                miBloqueCAD.open();
                miBloqueCAD.entidad.TransformBy(iMatriz);
                miBloqueCAD.save();
            }



        }



        public static bool hasBlockDef(string iBlockDefName)
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {

                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                if (acBlockTable.Has(iBlockDefName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
   
        }

        public static ObjectId insertBlockReference(string iBlockName, Point3d iPto3d, double iAngRad, double iBlockFactorEscala,string iLayerName)                                                                                                          
        {

            using (Transaction tr = oCadManager.StartTransaction())
            {

                BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                //Necesito Crear un Nuevo Registro para Añadir la Linea
                BlockTableRecord myModelo = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                if (acBlockTable.Has(iBlockName))
                {

                    //Obtengo la Definición de Bloque
                    ObjectId myBlockId = acBlockTable[iBlockName];
                    BlockTableRecord myBlockDef = (BlockTableRecord)tr.GetObject(myBlockId, OpenMode.ForWrite);
                    //Inserto el Bloque
                    BlockReference myBlockRef = new BlockReference(iPto3d, myBlockId);
                    //Roto el Bloque
                    myBlockRef.Rotation = iAngRad;
                    //Escala Bloque
                    Scale3d myEsc = new Scale3d(iBlockFactorEscala);
                    myBlockRef.ScaleFactors = myEsc;
                    //Configuro Valores Por Defecto
                    myBlockRef.SetDatabaseDefaults();

                   
                    if (!string.IsNullOrEmpty(iLayerName) && oLayer.HasLayer(iLayerName))
                    {
                        myBlockRef.Layer = iLayerName;
                    }

                    //Inserto el Objeto
                    ObjectId myBlockRefId = myModelo.AppendEntity(myBlockRef);

                    //Inserto el Objeto en la Transaccion
                    tr.AddNewlyCreatedDBObject(myBlockRef, true);
                    tr.Commit();

                    return myBlockRefId;
                }
                else
                {
                    throw new System.Exception(string.Format(strError.eBloqueNoExisteFichero,  iBlockName ));
                }

            }
        }


        public static ObjectId insertBlockReference(string iBlockName,                                            
                                                    Point3d iPto3d,
                                                    double iAngRad,
                                                    bool iAttConfigurar,
                                                    Dictionary<int, string> iDicAtributosBaseUno,
                                                    double iBlockFactorEscala,
                                                    Color iColor,
                                                    string iLayerName)
        {

                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord myModelo = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;


                    if (acBlockTable.Has(iBlockName))
                    {

                        //Obtengo la Definición de Bloque
                        ObjectId myBlockId = acBlockTable[iBlockName];
                        BlockTableRecord myBlockDef =(BlockTableRecord) tr.GetObject(myBlockId,OpenMode.ForWrite);
                        //Inserto el Bloque
                        BlockReference myBlockRef = new BlockReference(iPto3d, myBlockId);
                        //Roto el Bloque
                        myBlockRef.Rotation = iAngRad;
                        //Escala Bloque
                        Scale3d myEsc = new Scale3d(iBlockFactorEscala);
                        myBlockRef.ScaleFactors = myEsc;
                        //Configuro Valores Por Defecto
                        myBlockRef.SetDatabaseDefaults();

                        //Capa

                        if (! string.IsNullOrEmpty(iLayerName) && oLayer.HasLayer(iLayerName))
                        {
                            myBlockRef.Layer = iLayerName;
                        }

                        if (iColor != null)
                        {
                            myBlockRef.Color = Color.FromColorIndex(ColorMethod.ByBlock, iColor.ColorIndex);  

                        }

                        //Inserto el Objeto
                        ObjectId myBlockRefId =myModelo.AppendEntity(myBlockRef);

                        //Inserto los Atributos
                        if (myBlockDef.HasAttributeDefinitions)
                        {
                            //Listado de Atributos
                            List<AttributeDefinition> myListAttDef = oBlock.GetAtributosDefinicionList(tr,myBlockDef);


                            //Creo el Atrbuto Reference
                            AttributeReference myAttRef;


                            for (int i = 0; i < myListAttDef.Count; i++)
                            {
                                myAttRef = new AttributeReference();
                                myAttRef.SetDatabaseDefaults();
                                myAttRef.SetAttributeFromBlock(myListAttDef[i], myBlockRef.BlockTransform);
                                myAttRef.Tag = myListAttDef[i].Tag;
                                //Añado el Atributo a la Referencia
                                myBlockRef.AttributeCollection.AppendAttribute(myAttRef);
                                tr.AddNewlyCreatedDBObject(myAttRef, true);
                            }
                        }


                        //Edito los Atributos

                        if (iAttConfigurar == true && iDicAtributosBaseUno != null && iDicAtributosBaseUno.Count != 0)
                        {

                            AttributeCollection myBlRefAttCol = myBlockRef.AttributeCollection;

                            foreach (KeyValuePair<int, string> myDicKey in iDicAtributosBaseUno)
                            {

                                AttributeReference myBlockAttRef = (AttributeReference)tr.GetObject(myBlRefAttCol[myDicKey.Key - 1], OpenMode.ForWrite);

                                myBlockAttRef.TextString = myDicKey.Value;
                            }

                        }


                        //Inserto el Objeto en la Transaccion
                        tr.AddNewlyCreatedDBObject(myBlockRef, true);
                        tr.Commit();

                        return myBlockRefId;
                    }
                    else
                    {
                        throw new System.Exception(string.Format(strError.eBloqueNoExisteFichero, iBlockName));
                    }

                }              
        }



        private static List<AttributeDefinition> GetAtributosDefinicionList(Transaction iTr, BlockTableRecord iBlockBTR)
        {
            List<AttributeDefinition> myListAtt = new List<AttributeDefinition>();


            if (iBlockBTR.HasAttributeDefinitions)
            {

                foreach (ObjectId attId in iBlockBTR)
                {

                    DBObject obj = (DBObject)iTr.GetObject(attId, OpenMode.ForRead);

                    AttributeDefinition ad2 = obj as AttributeDefinition;
                    if (ad2 != null)
                    {
                        myListAtt.Add(ad2);
                    }
                }

                return myListAtt;

            }
            else
            {
                return myListAtt; // No existen Atributos
            }


        }


        public static void InsertDwgAsBlockDefAndCheckAtributosNum(string iDwgName, int iAtributosNum, string iPassWord)
        {

            if (!File.Exists(iDwgName))
            {
                throw new FileNotFoundException(iDwgName);
            }
     
            string myBlockName = SymbolUtilityServices.GetBlockNameFromInsertPathName(iDwgName);

       
                using (Database myDbNew = new Database(false, true))
                {
                
                  myDbNew.ReadDwgFile(iDwgName, FileShare.Read, true, iPassWord);

                using (Transaction tr = oCadManager.StartTransaction())
                {

                      ObjectId myDwgId =  oCadManager.thisBase.Insert(myBlockName, myDbNew, true);

                      BlockTableRecord myBlockDef = GetBlockDefTrById(tr, myDwgId, OpenMode.ForRead);


                    if (myBlockDef.HasAttributeDefinitions)
                        {

                            //Obtengo el Listado de Definicion de Atributos
                            List<AttributeDefinition> myAttList = GetAtributosDefinicionList(tr, myBlockDef);


                            //Compruebo el Máximo Numero de Atributos
                            if (iAtributosNum > myAttList.Count)
                            {
                                tr.Abort();
                            }

                        }


                    tr.Commit();

                }

                }
                
        }


        //OBTENGO EL BLOCKTABLERECORD DEL BLOQUE POR OBJECTID
        public static BlockTableRecord GetBlockDefTrById(Transaction iTr, ObjectId iBlockDefId, OpenMode iOpenMode)
        {
            return iTr.GetObject(iBlockDefId, iOpenMode) as BlockTableRecord;
        }
            

    }
}
