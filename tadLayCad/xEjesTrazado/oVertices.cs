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
    using tadLayShare.puntoOld;



    using engCadNet;
    using tadLayShare;
    
    
    public class oVertice
    {


        public int id { get; set; }
        /// <summary>
        /// Coordenadas del Vertice
        /// </summary>
        public Point3d position { get; set; }
        /// <summary>
        /// Ang Grados con los Vertices Anterior y Posterior
        /// </summary>
        public double? angGr { get; set; }
        /// <summary>
        /// Tipo de Curva Paso, No Paso
        /// </summary>
        public eRoadCurva tipo { get; set; }
        /// <summary>
        /// Radio de la Curva por Norma
        /// </summary>
        public double? radio { get; set; }
        /// <summary>
        /// Valor A Spiral
        /// </summary>
        public double? Avalue { get; set; }
        /// <summary>
        /// Puntos Paso Previo
        /// </summary>
        public Point3d? arcoP1 { get; set; }
        /// <summary>
        /// Puntos Paso Next
        /// </summary>
        public Point3d? arcoP2 { get; set; }
        /// <summary>
        /// Entidad Vertice
        /// </summary>
        public eRoadVerticeEntidad? entidad { get; set; }
        /// <summary>
        /// Id Entidad Creada Vertice
        /// </summary>
        public int? idEntidad { get; set; }


        public bool isClockWise
        {

            get
            {
                if (id % 2 == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
              
            }
        
        
        }




        public Point3d center
        {

            get
            {
                return oCircle.getCenterTreePoints(arcoP1.Value, position, arcoP2.Value);
            }
        
        }

        public oVertice(int iId,Point3d iVertice)
        {
            id = iId;
            position = iVertice;
        }

        public override string ToString()
        {
            string myIdBaseUno = Convert.ToString(id + 1);
            string myAngle = Convert.ToString(Math.Round(angGr.Value, 1));
            string myAvalue = Convert.ToString(Math.Round(Avalue.Value, 1));


            return "Vertice:" + myIdBaseUno.ToString() + 
                   "| x:" + position.X.ToString() + 
                   "| y:" + position.Y.ToString() + 
                   "| ang:" + myAngle + 
                   "| Curva:" + tipo.ToString() +
                   "| radio:" + radio.ToString() +
                   "| Aclotoide:" + myAvalue;
        }


        /// Obtener el Centro de la Curva en los Vertices de Paso
        public static void getCenterVerticePaso(Point3d iVertice,double iAngGr, double iRadio, Point3d iPreviousVertice, Point3d iNextVertice, out Point3d outArcoP1, out Point3d outArcoP2)
        { 
        
            //1 Obtengo Azimut Tramo Previo
            double myAziPre = oToolTrigo.getAzimutGrados(iVertice.X, iVertice.Y, iPreviousVertice.X, iPreviousVertice.Y).Value;
            double myAziNext = oToolTrigo.getAzimutGrados(iVertice.X, iVertice.Y, iNextVertice.X, iNextVertice.Y).Value;

            //2 Obtengo la Longitud 
            double myLon = getLonVerticesPaso(iRadio, iAngGr);

            //3 Obtengo la Coordenada a L
            oP2d myPtoPrevio = oToolTrigo.getP2FromLonAzimut(iVertice.X, iVertice.Y, myAziPre, myLon);
            oP2d myPtoNext = oToolTrigo.getP2FromLonAzimut(iVertice.X, iVertice.Y, myAziNext, myLon);


            Point3d myPtoCadPrevio = new Point3d(myPtoPrevio.X.Value, myPtoPrevio.Y.Value, 0);
            Point3d myPtoCadNext = new Point3d(myPtoNext.X.Value, myPtoNext.Y.Value, 0);

            outArcoP1 = myPtoCadPrevio;
            outArcoP2 = myPtoCadNext;
        }

        /// Determinar la Entidad del Vertice
        public static eRoadVerticeEntidad getEntidadVertice(eRoadCurva iCurvaTipo, double iAngGr)
        {

           if (iCurvaTipo== eRoadCurva.Paso)
           {
             return eRoadVerticeEntidad.FixedCurve; 
           }
           else if (iAngGr > 176.5 && iAngGr <= 180)
           {
             return eRoadVerticeEntidad.FreeCurve;
           }
           else
           {
               return eRoadVerticeEntidad.FreeSpiral;
           }
        }

        /// Determinar la Longitud Puntos Paso Curva
        public static double getLonVerticesPaso(double iRadio, double iAngGra)
        {       
            return (2*iRadio* Math.Cos(oToolTrigo.getRadianesFromGrados(iAngGra/2)));      
        }

         /// Lon entre 2 Vertices
        public static double getLonTwoVertices(Point3d iV1Pos, Point3d iV2Pos)
        {
            Line myLine = new Line(iV1Pos, iV2Pos);
            return myLine.Length;
        }
   
        }

    }

