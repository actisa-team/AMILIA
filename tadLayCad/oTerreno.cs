using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayCad
{
    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using C3Ddt = Autodesk.Civil.Land.DatabaseServices;
    using Autodesk.Civil;
    



    using engCadNet;
    using tadLayShare;
    using tadLayShare.puntoOld;

    
    public class oTerreno
    {
        private string mName = string.Empty;
        private  C3Ddt.TinSurface mTerreno = null;

        #region "Constructor"
        public oTerreno()
        {       
        }


        public oTerreno(string iSurfaceName)
        {

            mName = iSurfaceName;
        
        }

        #endregion
        #region "Propiedades"
        public string name
        {

            get
            {
                if (!string.IsNullOrEmpty(mName))
                {
                    return mName;
                }
                else
                {
                    throw new oExPropertieNullValue("Terreno Nombre");
                }
            }

            set
            {

                mName = value;
            }

        }
        public C3Ddt.TinSurface terreno
        {
            get
            {
                if (mTerreno == null)
                {
                    mTerreno = (C3Ddt.TinSurface)engCadNet.cv3d.oSurface.getSurfaceByName(name);
                }

                return mTerreno;    
            }
        
        
        
        
        }
        #endregion
        #region "Metodos"    
        /// <summary>
        /// Dibujar Polilineas Triangulos Mayor Pendiente
        /// </summary>
        /// <param name="iPendiente">Valor Entre 0 - 1 </param>
        /// <param name="iLayer">Capa Dibujar</param>
        public void slopeAnalisisPreview(double iPendiente, string iLayer)
        {

            C3Ddt.TinSurfaceTriangleCollection myLstTriangle = terreno.Triangles;

            foreach (C3Ddt.TinSurfaceTriangle myTri in myLstTriangle)
            {

                double myPen = engCadNet.cv3d.oSurface.getSlopeFromTriangle(myTri);

                if (myPen > iPendiente)
                {

                    Point3dCollection myLstPto = new Point3dCollection();

                    myLstPto.Add(myTri.Vertex1.Location);
                    myLstPto.Add(myTri.Vertex2.Location);
                    myLstPto.Add(myTri.Vertex3.Location);

                    engCadNet.oLw.addLw2d(myLstPto, true, iLayer);


                }

            }
        
        
        
        
        
        
        }
        /// <summary>
        /// Generar Polilineas Partiendo de los Triangulos de PendienteSuperior a la Indicada
        /// </summary>
        /// <param name="iPendiente">Valor 0-1</param>
        /// <param name="iLayer">Capa Dibujar Polilineas</param>
        /// <param name="iAreaMin">Area Min Descartar Zonas</param>
        public void drawZonasNoPasoAnalisisPendiente(double iPendiente,string iLayer, double iAreaMin)
        {


            C3Ddt.TinSurfaceTriangleCollection myLstTriangle = mTerreno.Triangles;

            foreach (C3Ddt.TinSurfaceTriangle myTri in myLstTriangle)
            {

                double myPen = engCadNet.cv3d.oSurface.getSlopeFromTriangle(myTri);

                if (myPen > iPendiente)
                { 
 
                        Polyline myPol;

                        Point3dCollection myLstPto = new Point3dCollection();

                        myLstPto.Add(myTri.Vertex1.Location);
                        myLstPto.Add(myTri.Vertex2.Location);
                        myLstPto.Add(myTri.Vertex3.Location);

                        myPol = engCadNet.oLw.addLw2d(myLstPto, true, iLayer);

                        //Creo la Region
                        engCadNet.oRegion.addRegionFromLw(myPol, iLayer);

                        //Borro la Polilinea
                        engCadNet.oTools.deleteEntidad(myPol.ObjectId);

                }
                
            }


            //Genero la Unión de todas las Regiones
            SelectionSet mySsRegion = engCadNet.oSs.getSsByLayerAndEntidad(iLayer,eEntidades.REGION);


            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {


                    //Creo una Entidad
                    Region myRegionNew;
                    Region myRegionParte;

                    BlockTable acBlockTable = tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead) as BlockTable;

                    //Necesito Crear un Nuevo Registro para Añadir la Linea
                    BlockTableRecord acBlockTableRec = tr.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                   
                    myRegionNew = tr.GetObject(mySsRegion[0].ObjectId, OpenMode.ForWrite) as Region;

                    for (int i = 1; i < mySsRegion.Count; i++)
                    {
                        myRegionParte = tr.GetObject(mySsRegion[i].ObjectId, OpenMode.ForWrite) as Region;
                        myRegionNew.BooleanOperation(BooleanOperationType.BoolUnite, myRegionParte);
                        myRegionParte.Erase();
                    }

                     myRegionNew.SetDatabaseDefaults();


                     //Polilineas
                     DBObjectCollection objs = engCadNet.oRegion.getPolylineFromRegion(myRegionNew);

                     foreach (Entity ent in objs)
                     {
                         acBlockTableRec.AppendEntity(ent);
                         tr.AddNewlyCreatedDBObject(ent, true);
                     }


                     myRegionNew.Erase();

                    tr.Commit();

                }
            }


            #region "BORRAR POLILINEAS INTERIORES"

            try
            {

                //Creo Seleccion con todas las polilienas
                SelectionSet mySsLwAll = engCadNet.oSs.getSsByLayerAndEntidad(iLayer, eEntidades.LWPOLYLINE);

                //Creo un conjunto de Objetos
                List<Polyline> myLstLw = new List<Polyline>();
                Polyline myLw = null;

                using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {

                        foreach (ObjectId myObjId in mySsLwAll.GetObjectIds())
                        {
                            myLw = tr.GetObject(myObjId, OpenMode.ForRead) as Polyline;

                            if (myLw.Area > iAreaMin)
                            {
                                myLstLw.Add(myLw);
                            }
                            else
                            {
                                engCadNet.oTools.deleteEntidad(myLw.ObjectId);
                            }


                        }

                        tr.Commit();

                    }
                }


                //Ahora Ordeno los Elementos por Area
                myLstLw = myLstLw.OrderByDescending(p => p.Area).ToList();



                //Inicio el Ciclo para  borrar Entidades Superiores

                SelectionSet mySsLwInto;

                List<ObjectId> myLstPolyErase = new List<ObjectId>();

                foreach (Polyline myPol in myLstLw)
                {

                    if (!myLstPolyErase.Contains(myPol.ObjectId))
                    {
                        //Creo el Conjunto de Selección
                        mySsLwInto = engCadNet.oSs.getSsIntoPolylineByLayerAndEntidad(myPol, iLayer, eEntidades.LWPOLYLINE);

                        if (mySsLwInto != null && mySsLwInto.Count > 0)
                        {
                            foreach (ObjectId myObjId in mySsLwInto.GetObjectIds())
                            {
                                myLstPolyErase.Add(myObjId);
                            }
                        }

                        //Borro el conjunto de Selección
                        engCadNet.oSs.deleteSs(mySsLwInto);
                    }

                }

            }
            catch (Exception)
            { 
                throw;
            }



            #endregion

        }

        /// <summary>
        /// Obtener la Pendiente en un Punto del Terreno
        /// </summary>
        /// <param name="iX">Indica X</param>
        /// <param name="iY">Indica Y</param>
        /// <returns>Pendiente</returns>
        public double? getSlopeFromXY(double? iX, double? iY)
        {

            if (iX.HasValue && iY.HasValue)
            {

                try
                {
                    C3Ddt.TinSurfaceTriangle myFace = mTerreno.FindTriangleAtXY(iX.Value, iY.Value);

                    double myPenOut = engCadNet.cv3d.oSurface.getSlopeFromTriangle(myFace);

                    return myPenOut;

                }

                catch (PointNotOnEntityException)
                {
                    return null;
                }

            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }
        /// <summary>
        /// Obtener la Cota Z en un Punto del Terreno
        /// </summary>
        /// <param name="iX">X</param>
        /// <param name="iY">Y</param>
        /// <returns>Cota Z</returns>
        public double? getZFromXY(double? iX, double? iY)
        {

            if (iX.HasValue && iY.HasValue)
            {
                try
                {

                    double? myZ = null;

                    myZ = terreno.FindElevationAtXY(iX.Value, iY.Value);

                    if (myZ.HasValue)
                    {
                        return myZ.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (PointNotOnEntityException)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }

        /// <summary>
        /// Determinar si un Punto esta en el Terreno
        /// </summary>
        /// <param name="iPto">Pto</param>
        /// <returns>True esta Dentro</returns>
        public bool gPtoInsideTerreno(oP2d iPto)
        {
            return gPtoInsideTerreno(iPto.X, iPto.Y);

        }

        /// <summary>
        /// Determinar si un Punto esta en el Terreno
        /// </summary>
        /// <param name="iX">X</param>
        /// <param name="iY">Y</param>
        /// <returns>True esta Dentro</returns>
        public bool gPtoInsideTerreno(double? iX, double? iY)
        {

            if (iX.HasValue && iY.HasValue)
            {

                try
                {

                    double? myPtoZ = terreno.FindElevationAtXY(iX.Value, iY.Value);

                    if (myPtoZ.HasValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (PointNotOnEntityException)
                {

                    return false;
                }



            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }





        
        
        #endregion
    }
}
