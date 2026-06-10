using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.GraphicsInterface;
namespace engCadNet
{

    using tadLayShare;

    
    
    public class oText
    {

        public static void addText2D (string iTexto, double[] iPto, double iAltura, double iRotateRadianes, string iLayer)
        {
            addText2D(iTexto, new Point2d(iPto), iAltura, iRotateRadianes, iLayer);     
        }


        public static void addText2D(string iStr, Point2d iPto, double iAltura, double iRotate, string iLayer)
        {
            addText2D(iStr, iPto.X, iPto.Y, iAltura, iRotate, iLayer);
        }      
        public static void addText2D (string iTexto, double iX, double iY, double iAltura, double iRotateRadianes, string iLayer)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {

                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    DBText miText = new DBText();

                    miText.SetDatabaseDefaults();

                    miText.Height = iAltura;

                    miText.HorizontalMode = TextHorizontalMode.TextRight;

                    miText.VerticalMode = TextVerticalMode.TextVerticalMid;

                    miText.Position = new Point3d(iX, iY, 0);

                    miText.AlignmentPoint = miText.Position;

                    miText.Rotation = iRotateRadianes;

                    miText.TextString = iTexto;

                    miText.Layer = iLayer;


                    acBlockTableRec.AppendEntity(miText);

                    tr.AddNewlyCreatedDBObject(miText, true);

                    tr.Commit();

                }
            }

        }


        public static void addText2DMatrix(string iStr, 
                                           double iX, 
                                           double iY, 
                                           double iAltura, 
                                           double iRotate, 
                                           string iLayer,
                                           TextHorizontalMode iTxtHorMode,
                                           TextVerticalMode iTxtVerMode,
                                           Matrix3d iMatrix3d)
        {

            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    DBText myText = new DBText();

                    myText.SetDatabaseDefaults();

                    myText.Height = iAltura;

                    myText.HorizontalMode = iTxtHorMode;
                    myText.VerticalMode = iTxtVerMode;
                    myText.AlignmentPoint =new Point3d(iX, iY, 0);
                    myText.Rotation = iRotate;
                    myText.TextString = iStr;
                    myText.Layer = iLayer;
                    myText.TransformBy(iMatrix3d);

                    acBlockTableRec.AppendEntity(myText);

                    tr.AddNewlyCreatedDBObject(myText, true);

                    tr.Commit();

                }
            }

        }

        public static ObjectId AddTextStyle(string iTxtStyleName, eFuentes iFuente, bool iIsCurrent)
        {

            try
            {


                //Creo la Conexion Cad
              

                using (Transaction tr = oCadManager.StartTransaction())
                {

                    //Obtengo Listado
                    TextStyleTable myLstEstilo;
                    myLstEstilo = (TextStyleTable) oCadManager.thisBase.TextStyleTableId.GetObject(OpenMode.ForRead);

                    //TxtstyleId
                    ObjectId myTxtEstiloID = ObjectId.Null;


                    //Compruebo que no Exista el Estilo de Texto
                    if (myLstEstilo.Has(iTxtStyleName))
                    {
                        myTxtEstiloID = myLstEstilo[iTxtStyleName];
                        if (iIsCurrent) {oCadManager.thisBase.Textstyle = myTxtEstiloID; }
                        tr.Commit();
                        return myTxtEstiloID;
                    }
                    else
                    {
                        //Creo el Estilo de Texto
                        TextStyleTableRecord myEstiloTr = new TextStyleTableRecord();
                        //Pongo el Nombre
                        myEstiloTr.Name = iTxtStyleName.Trim();
                        //Configuro la Fuentes
                        FontDescriptor myFont = new FontDescriptor(iFuente.ToString(), false, false, 0, 0);
                        //Cargo la Fuente
                        myEstiloTr.Font = myFont;
                        //Abro el Objeto
                        myLstEstilo.UpgradeOpen();
                        //Añado el Estilo al Listado
                        myTxtEstiloID = myLstEstilo.Add(myEstiloTr);
                        //Añado Objeto a la Transaccion
                        tr.AddNewlyCreatedDBObject(myEstiloTr, true);
                        //Pongo Activo el Estilo
                        if (iIsCurrent) {oCadManager.thisBase.Textstyle = myTxtEstiloID; }
                        //Guardo la Transaccion
                        tr.Commit();

                        //Devuelvo el Estilo de Texto
                        return myEstiloTr.ObjectId;
                    }

                }
            }
            catch (Exception)
            {
                throw;

            }

        }

    }
}
