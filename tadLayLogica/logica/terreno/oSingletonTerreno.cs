using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLogica.datos;

namespace tadLayLogica
{


    using tadLayLogica.datos.proyecto;

    using engCadNet;
    using engNet.ClassT;


    using tadLayShare;
    using tadLayShare.puntos;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    //Inicio modificacion [ANGELES]

    using Terrenos;

    //Fin modificacion [ANGELES]
    using tadLayLan.Tdm;
    using tadLayLan;
    using System.IO;




    public class oSingletonTerreno : IDisposable
    {

        private static oSingletonTerreno mInstance = null;
        //Inicio modificación [ANGELES]
        //private C3Ddt.TinSurface mTerreno = null;


        private Triangulacion mTerreno = null;
        private static Polyline mEnvolvente = null;

        public int tipo { get; set; }//0 -> no tiene nada cargado / 1 -> Carga Mdt normal / 2 -> carga mdt de puntos / 3 -> carga mdt de ASC
        //Inicio modificación [ANGELES]


        #region "Constructor"
        public static oSingletonTerreno getInstance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new oSingletonTerreno();
                }

                return mInstance;
            }
        }
        private oSingletonTerreno()
        {

        }
        #endregion  
        #region "Propiedades"
        public static string layerTerreno
        {
            get
            {
                string capa = "";
                string miTerrenoHandle = oDalTerreno.getTerrenoHandle();
                using (oEntidad<SubDMesh> miSurface = new oEntidad<SubDMesh>(miTerrenoHandle))
                {
                    capa = miSurface.entidad.Layer;
                }
                return capa;
            }
        }


        public Triangulacion terreno
        {
            get
            {
                if (mTerreno == null)
                {
                    /*
                     * Se quita el try catch  para que no coja en ningun momento el MDT de autocad **juanma**
                     */
                    /*
                    oTadil.data.UserInfo.showInfo(strFormTdm.cargandoTerreno);
                        string miTerrenoHandle = oDalTerreno.getTerrenoHandle();

                    try
                    {

                        using (oEntidad<PolyFaceMesh> miSurface = new oEntidad<PolyFaceMesh>(miTerrenoHandle))
                        {
                            
                            mTerreno = oSubDMesh.getTriangulacion(miSurface.entidad.ObjectId);

                            //para guardar la triangulacion del bigtopo 
                            //Xrecord miXrecord = engCadNet.oXrecord.getXrecord(miSurface.entidad.ObjectId, "info");

                            //var miInfo = engCadNet.oXrecord.getStream(miXrecord);


                            //FileStream fileStream = File.Create(mTerreno.Name + ".trr", (int)miInfo.Capacity);
                            //byte[] bytesInStream = new byte[miInfo.Length];
                            //miInfo.Read(bytesInStream, 0, bytesInStream.Length);
                            //miInfo.Close();

                            //fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                            //fileStream.Close();




                        }
                    }
                    catch (oExEntidadNoExiste e)
                    {
                        using (oEntidad<SubDMesh> miSurface = new oEntidad<SubDMesh>(miTerrenoHandle))
                        {
                            mTerreno = oSubDMesh.getTriangulacion(miSurface.entidad.ObjectId);
                        }
                    }
                    catch (Exception e)
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                    }
                    */

                }
                return mTerreno;
            }
        }
        public string name { get { return terreno.Name; } }

        public void Set_Terreno(Triangulacion terreno, int tipo = 0)
        {
            mTerreno = terreno;
            this.tipo = tipo;
        }
        public void Set_envolvente()
        {
            mEnvolvente = this.getPolEnvolvente();
        }
        public Polyline envolvente
        {
            get
            {
                if (mEnvolvente == null)
                {
                    mEnvolvente = this.getPolEnvolvente();
                }
                return mEnvolvente;
                return this.getPolEnvolvente();
            }
        }

        public double tolerancia
        {
            get
            {
                return oDalTerreno.getTerrenoTolerancia();
            }
        }
        #endregion




        #region "Metodos"    


        ///// <summary>
        /// Obtener la Pendiente en un Punto del Terreno
        /// </summary>
        public double? getSlopeFromXY(double? iX, double? iY)
        {

            if (iX.HasValue && iY.HasValue)
            {

                try
                {

                    //C3Ddt.TinSurfaceTriangle myFace = this.terreno.FindTriangleAtXY(iX.Value, iY.Value);
                    //double myPenOut = engCadNet.cv3d.oSurface.getSlopeFromTriangle(myFace);

                    Triangulo miTriangulo = this.terreno.getTrianguloReg(new Punto3d((double)iX.Value, (double)iY.Value, 0));

                    double myPenOut = miTriangulo.getPendienteMaxima;

                    return myPenOut;

                }

                catch (SystemException)
                {
                    return null;
                }

            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }

        public double? getSlopeFromTriang(Triangulo iTriangulo)
        {

            if (iTriangulo != null)
            {

                try
                {

                    double myPenOut = iTriangulo.getPendienteMaxima;

                    return myPenOut;

                }

                catch (SystemException)
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
        /// Obtener la Pendiente en un Punto del Terreno
        /// </summary>
        public double? getSlopeFromXY(double[] iPtoXY)
        {
            return this.getSlopeFromXY(iPtoXY[0], iPtoXY[1]);
        }
        /// <summary>
        /// Obtener la Cota Z en un Punto del Terreno
        /// </summary>
        public double? getZFromXY(double? iX, double? iY)
        {

            if (iX.HasValue && iY.HasValue)
            {
                try
                {
                    if (oSingletonTerreno.getInstance.tipo == 2)
                    {
                        return oSingletonPuntosTerreno.getInstance.GetZ(iX.Value, iY.Value);
                    }
                    else if (oSingletonTerreno.getInstance.tipo == 3)
                    {
                        return oSingletonPuntosTerrenoASC.getInstance.GetZ(iX.Value, iY.Value);
                    }
                    double? myZ = null;

                    //myZ = terreno.FindElevationAtXY(iX.Value, iY.Value);
                    //double zprueba= oSingletonPuntosTerreno.getInstance.GetZ(iX.Value, iY.Value);
                    myZ = this.terreno.getCota(iX.Value, iY.Value);

                    if (myZ.HasValue)
                    {
                        return myZ.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }

        public double? getZFromTriang(double? iX, double? iY, Triangulo iTriangulo)
        {

            if (iX.HasValue && iY.HasValue)
            {
                try
                {

                    double? myZ = null;

                    myZ = this.terreno.getCotaTriang(iX.Value, iY.Value, iTriangulo);

                    if (myZ.HasValue)
                    {
                        return myZ.Value;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }

        public List<Triangulo> getZonasMaxPendientes(double iPendienteMaxima)
        {

            List<Triangulo> misZonasMaxPendientes = this.terreno.getPuntosZonasMaxPendiente(iPendienteMaxima);
            return misZonasMaxPendientes;
        }

        public Triangulo getTrianguloInside(double? iX, double? iY)
        {
            if (iX.HasValue && iY.HasValue)
            {
                try
                {
                    Triangulo miTriangulo = this.terreno.getTriangulo(iX.Value, iY.Value);
                    return miTriangulo;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }
        }


        public Triangulo getTrianguloInsideFromTriangulo(double? iX, double? iY, Triangulo iTriangulo)
        {
            if (iX.HasValue && iY.HasValue)
            {
                try
                {
                    Triangulo miTriangulo = this.terreno.getTrianguloTriang(iTriangulo, new Punto3d(iX.Value, iY.Value, 0));
                    return miTriangulo;
                }
                catch (Exception e)
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
        public double? getZFromXY(double[] iPtoXY)
        {
            return this.getZFromXY(iPtoXY[0], iPtoXY[1]);
        }
        /// <summary>
        /// Obtener cota Z en Funcion del Punto
        /// </summary>
        public double getZFromXYNoNull(double iX, double iY)
        {
            return this.terreno.getCota(iX, iY).Value;
        }
        /// <summary>
        /// Determinar si un Punto esta en el Terreno
        /// </summary>
        /// <param name="iPto">Pto</param>
        /// <returns>True esta Dentro</returns>
        public bool isPtoInsideTerreno(oP2d iPto)
        {
            return isPtoInsideTerreno(iPto.X, iPto.Y);
        }
        /// <summary>
        /// Determinar si un Punto esta en el Terreno
        /// </summary>
        /// <param name="iX">X</param>
        /// <param name="iY">Y</param>
        /// <returns>True esta Dentro</returns>
        public bool isPtoInsideTerreno(double? iX, double? iY)
        {
            if (this.tipo==2)
            {
                return oSingletonPuntosTerreno.getInstance.Inside(iX.Value,iY.Value);
            }
            if (this.tipo == 3)
            {
                return oSingletonPuntosTerrenoASC.getInstance.Inside(iX.Value, iY.Value);
            }
            if (iX.HasValue && iY.HasValue)
            {

                try
                {
                    double? myPtoZ = this.terreno.getCota(iX.Value, iY.Value);

                    if (myPtoZ.HasValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (SystemException)
                {

                    return false;
                }

            }
            else
            {
                throw new Exception("Las Coordendas del Punto son Nulas");
            }

        }

        public bool isPtoCercaBorde(double? iX, double? iY)
        {
            double distancia = -1;
            bool isCerca = false;
            if (this.tipo == 2)
            {
                distancia = oSingletonPuntosTerreno.getInstance.DistanciaABorde((double)iX, (double)iY);
            }
            else if (this.tipo==3)
            {
                distancia= oSingletonPuntosTerrenoASC.getInstance.DistanciaABorde((double)iX,(double)iY);
            }
            else
            {
                

                Point3d miPuntoEnt = new Point3d(iX.Value, iY.Value, 0);
                Point3d miPuntoMasCercano = this.envolvente.getPointMasCercano(miPuntoEnt);

                Point2d miPuntoEnt2d = new Point2d(iX.Value, iY.Value);
                Point2d miPuntoMasCercano2d = new Point2d(miPuntoMasCercano.X, miPuntoMasCercano.Y);

                distancia = miPuntoMasCercano2d.GetDistanceTo(miPuntoEnt2d);

            }

            /*
             * Se pone la toleracncia para debug, hay que ver como ponerla por parametro para los nuevos cambios **juanma**
             */

            try
            {
                if (distancia < this.tolerancia)
                {
                    isCerca = true;
                }
            }
            catch (Exception e)
            {
                if (distancia < 1)
                {
                    isCerca = true;
                }
            }


            return isCerca;


        }

        /// <summary>
        /// Lista de puntos ordenada de la envolvente del terreno
        /// </summary>
        public Polyline getPolEnvolvente()
        {

            Polyline polEnvolvente = new Polyline();
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                List<Punto3d> polEnvolventePunto3d = this.terreno.calculaEnvolvente();

                List<Point2d> misPuntos = new List<Point2d>();
                //int index = 0;
                foreach (Punto3d miPunto3d in polEnvolventePunto3d)
                {
                    misPuntos.Add(new Point2d(miPunto3d.coordenadaX, miPunto3d.coordenadaY));
                    //polEnvolvente.AddVertexAt(index, new Point2d(miPunto3d.coordenadaX, miPunto3d.coordenadaY), 0, 0, 0);
                    //index++;
                }
                polEnvolvente = oLw.addLw2d(misPuntos, false, "0");
            }

            return polEnvolvente;


        }
        public Polyline get_PolEnvolvente()
        {
            return mEnvolvente;
        }

        public static void setPolEnvolvente(Polyline iPolEnv)
        {
            mEnvolvente = iPolEnv;
            oSingletonPuntosTerreno.getInstance.Set_Envolvente(mEnvolvente);
            oSingletonPuntosTerrenoASC.getInstance.Set_Envolvente(mEnvolvente);
        }


        /// <summary>
        /// Dispose0
        /// </summary>
        public void Dispose()
        {

            if (this.mTerreno != null)
            {
                //mTerreno.Dispose();
                mTerreno = null;

            }
            if (mEnvolvente != null)
            {
                mEnvolvente = null;
            }

            mInstance = null;
        }

        #endregion


    }


}
