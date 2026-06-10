using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad
{
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;
    using tadLayShare;

    public class oZonaGeotecniaCad

    {

        private string mLayerCapaZonaGeotecnica = string.Empty;
        private  SelectionSet mSsZonasGeotecnicas = null;


        public oZonaGeotecniaCad(string iLayerZonasGeotecnicas)
        {
            mLayerCapaZonaGeotecnica = iLayerZonasGeotecnicas;           
            mSsZonasGeotecnicas = engCadNet.oSs.getSsByLayerAndEntidad(iLayerZonasGeotecnicas,eEntidades.LWPOLYLINE);
        }



        public  string getHandleFromZonaGeotecnia(double iPX, double iPY)
        {


            if (mSsZonasGeotecnicas != null && mSsZonasGeotecnicas.Count > 0)
            {

                using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        foreach (ObjectId myObj in mSsZonasGeotecnicas.GetObjectIds())
                        {
                            Polyline myLw = myObj.GetObject(OpenMode.ForRead) as Polyline;

                            

                            if (oPolygon.isPointInPolygon(myLw,iPX, iPY))
                            {
                                return myLw.Handle.Value.ToString();
                            }
                        
                        }
                        
                    }
                }     
            }

            return string.Empty;        
              
        }


        public  Dictionary<string,oZonaGeo> getDicZonaGeotecniaFromDwg()
        {


            Dictionary<string, oZonaGeo> myDicZonGeo = new Dictionary<string, oZonaGeo>();

            oZonaGeo myZonaGeo;

            //Creo el Conjunto de Selección de Bloques
            SelectionSet mySsBloques = engCadNet.oSs.getSsByLayerAndEntidad(mLayerCapaZonaGeotecnica,eEntidades.INSERT);

            //Debo de Comparar los valores de las 2 Selecciones
            if (mSsZonasGeotecnicas != null & mySsBloques !=null )
            {
            
              if (mSsZonasGeotecnicas.Count != mySsBloques.Count)
              {  
                throw new Exception ("El número de Bloques y Polilineas de la Zona Geotecnicas Son Distintos");              
              }
                     
            }

            if (mSsZonasGeotecnicas == null && mySsBloques == null)
            {
                return null;
            
            }


            //Ahora Debo de Obtener configurar las zonas con los bloques
              using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                    {
                        using (Transaction tr = oCadManager.StartTransaction())
                        {
                            foreach (ObjectId myBloque in mySsBloques.GetObjectIds())
                            {
                                BlockReference myBloqueRef = myBloque.GetObject(OpenMode.ForRead) as BlockReference;
                                AttributeCollection myBlAttCol = myBloqueRef.AttributeCollection;
                              

                                foreach (ObjectId myZonaLw in mSsZonasGeotecnicas.GetObjectIds())
                                {
                                
                                     Polyline myLw =   myZonaLw.GetObject(OpenMode.ForRead) as Polyline;
                                
                                    
                                    if ( engCadNet.oPolygon.isPointInPolygon(myLw,myBloqueRef.Position.X,myBloqueRef.Position.Y))
                                    {
                                         myZonaGeo = new oZonaGeo();
                                        AttributeReference myZonGeoNombre = (AttributeReference)tr.GetObject(myBlAttCol[0], OpenMode.ForRead);
                                        AttributeReference myZonGeoCosImp = (AttributeReference)tr.GetObject(myBlAttCol[1], OpenMode.ForRead);
                                        AttributeReference myZonGeoCosDes = (AttributeReference)tr.GetObject(myBlAttCol[2], OpenMode.ForRead);
                                        AttributeReference myZonGeoCosTer = (AttributeReference)tr.GetObject(myBlAttCol[3], OpenMode.ForRead);
                                        AttributeReference myZonGeoCosEst = (AttributeReference)tr.GetObject(myBlAttCol[4], OpenMode.ForRead);
                                        AttributeReference myZonGeoCosTun = (AttributeReference)tr.GetObject(myBlAttCol[5], OpenMode.ForRead);

                                        myZonaGeo.pNombre = myLw.Handle.Value.ToString();
                                        myZonaGeo.pCosteImplantacion = Convert.ToDouble(myZonGeoCosImp.TextString);
                                        myZonaGeo.pCosteDesmonte = Convert.ToDouble(myZonGeoCosDes.TextString);
                                        myZonaGeo.pCosteTerraplen = Convert.ToDouble(myZonGeoCosTer.TextString);
                                        myZonaGeo.pCostePuente = Convert.ToDouble(myZonGeoCosEst.TextString);
                                        myZonaGeo.pCosteTunel = Convert.ToDouble(myZonGeoCosTun.TextString);

                                        myDicZonGeo.Add(myZonaGeo.pNombre, myZonaGeo);
                                    
                                    }
                                
                                }

                               

                            }

                            
                        }


                    }


            if (mSsZonasGeotecnicas.Count == myDicZonGeo.Count)
            {
                return myDicZonGeo;
            }
            else
            {
            
                throw new Exception ("Error al Configurar las Zonas Geótecnicas");
            }
       
        }

    }
}

