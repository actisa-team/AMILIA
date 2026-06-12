using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Geometria.Saneo
{

    using engCadNet;
    using engNet.Extension.Double;
    using engNet.Extension.Integer;

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    
    public abstract class oSaneo
    {

        private static double longitudMinimaSaneo = 0.1;


        #region "Propiedades"

      
        public Guid materialExcavacion { get; set; }
        public Guid materialRelleno { get; set; }
        public double espesor { get; set; }
        public double Pk { get; set; } = -1;


        #endregion


        #region "Propiedades Abstractas"

        public abstract Color color { get; }

        #endregion

        #region "Constructores"

        public oSaneo()
        {

        }

        public oSaneo(Guid iIdMaterialExc, Guid iIdMaterialRel, double iEspesor)
        {
            materialExcavacion = iIdMaterialExc;
            materialRelleno = iIdMaterialRel;
            espesor = iEspesor;
        }



        #endregion



       public static  double  getPendienteSaneoTerraplen_AbsPorCiento (Polyline iLw)
       {


     
               //Pendiente del Terreno PTO INI - PTO FIN
               Point2d miPtoLineaPendienteIni = iLw.StartPoint.to2d();
               Point2d miPtoLineaPendienteFin = iLw.EndPoint.to2d();
               double miPendientePtoIniFin = Math.Abs(miPtoLineaPendienteIni.getPendiente2DPorCientoConSigno(miPtoLineaPendienteFin));


              //Pendiene del Terreno PTO INI - PTO MEDIO
              Point3d miPtoMedio3d = iLw.GetPointAtDist(iLw.Length * 0.5);
              Point2d miPtoMedio2d = miPtoMedio3d.to2d();

              double miPendientePtoIniMedio = Math.Abs(miPtoLineaPendienteIni.getPendiente2DPorCientoConSigno(miPtoMedio2d));


              if (miPendientePtoIniFin >= miPendientePtoIniMedio)
              {
                  return miPendientePtoIniFin;
              }
              else
              {
                  return miPendientePtoIniMedio;
              }
               
              

       }


       public static void getEscalon(Polyline iLw, double iEscalonHmax, ref int EscalonDrawNum, ref double EscalonDrawH)
       {

           double miEscalonesNumBruto = (Math.Abs(iLw.StartPoint.Y - iLw.EndPoint.Y)) / iEscalonHmax;
           EscalonDrawNum= miEscalonesNumBruto.roundEnteroSuperior();
           EscalonDrawH = (Math.Abs(iLw.StartPoint.Y - iLw.EndPoint.Y)) / EscalonDrawNum;
       
    
       }


       public static Point3d getPtoIntermedioMinY(Polyline iLw)
       {

           if (iLw.NumberOfVertices <= 2)
           {
               throw new Exception("La Polilinea No tiene Puntos Intermedios.");
           }


           List<Point3d> miLstPto = oLw.getLstPto(iLw);

           //Elimino los Vertices Iniciales y Finales
           miLstPto.RemoveAt(0);
           miLstPto.RemoveAt(miLstPto.Count() - 1);


           var myQuery = from p in miLstPto
                         orderby p.Y ascending
                         select p;

          return  myQuery.First();
       
       }
       public static Point3d getPtoIntermedioMaxY(Polyline iLw)
       {

           if (iLw.NumberOfVertices <= 2)
           {
               throw new Exception("La Polilinea No tiene Puntos Intermedios.");
           }


           List<Point3d> miLstPto = oLw.getLstPto(iLw);

           //Elimino los Vertices Iniciales y Finales
           miLstPto.RemoveAt(0);
           miLstPto.RemoveAt(miLstPto.Count() - 1);


           var myQuery = from p in miLstPto
                         orderby p.Y ascending
                         select p;

           return myQuery.Last();

       }


       public static List<Polyline> splitLwPuntoMasBajo(Polyline iLw)
       {

           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {

               using (Transaction tr = oCadManager.StartTransaction())
               {

                   //Obtengo el Menor Punto de la Lw
                   Point3d miPtoMin = getPtoIntermedioMinY(iLw);

                   //Obtengo las coordenadas de los extremoa
                   double miPini = iLw.StartPoint.Y;
                   double miPfin = iLw.EndPoint.Y;



                   List<Polyline> miLstLw = new List<Polyline>();
                   Polyline miLw;



                   Point3dCollection miColCorte = new Point3dCollection();
                   miColCorte.Add(miPtoMin);
                   DBObjectCollection miColObj = iLw.GetSplitCurves(miColCorte);
                   ObjectId miObjId = ObjectId.Null;
                   //Realizo el Cast

                   foreach (Entity acEnt in miColObj)
                   {
                       // Add each offset object
                       miObjId = oTools.entidadAdd(acEnt, iLw.Layer);
                       miLw = (Polyline)tr.GetObject(miObjId, OpenMode.ForWrite);
                       miLstLw.Add(miLw);
                   }

                   if (miLstLw.Count > 2)
                   {
                       throw new Exception("Error al Obtener las Polilineas del Saneo Doble");
                   }

                   tr.Commit();

                   return miLstLw;
               }
           }

       }
       public static List<Polyline> splitLwPuntoMasAlto(Polyline iLw)
       {

           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {

               using (Transaction tr = oCadManager.StartTransaction())
               {

                   //Obtengo el Menor Punto de la Lw
                   Point3d miPtoMin = getPtoIntermedioMaxY(iLw);

                   //Obtengo las coordenadas de los extremoa
                   double miPini = iLw.StartPoint.Y;
                   double miPfin = iLw.EndPoint.Y;



                   List<Polyline> miLstLw = new List<Polyline>();
                   Polyline miLw;



                   Point3dCollection miColCorte = new Point3dCollection();
                   miColCorte.Add(miPtoMin);
                   DBObjectCollection miColObj = iLw.GetSplitCurves(miColCorte);
                   ObjectId miObjId = ObjectId.Null;
                   //Realizo el Cast

                   foreach (Entity acEnt in miColObj)
                   {
                       // Add each offset object
                       miObjId = oTools.entidadAdd(acEnt, iLw.Layer);
                       miLw = (Polyline)tr.GetObject(miObjId, OpenMode.ForWrite);
                       miLstLw.Add(miLw);
                   }

                   if (miLstLw.Count > 2)
                   {
                       throw new Exception("Error al Obtener las Polilineas del Saneo Doble");
                   }

                   tr.Commit();

                   return miLstLw;
               }
           }

       }


       public static Polyline drawSaneoSimple (Polyline iLwOrigen, Color iColorSaneo, string iLayer, double iOffSetSigno, bool iAddVerticales, bool iJoinEntities)
       {

           if (iLwOrigen.Length < longitudMinimaSaneo)
           {
               return iLwOrigen;
           }
           
           
           
           using (Transaction tr = oCadManager.StartTransaction())
                   {
                       bool miOffSetPositivo;
                       double miLwOrigenCoordenadaY;
                       double miLwOffsetCoordenadaY;

                       if (iOffSetSigno > 0)
                       {
                           miOffSetPositivo = true;
                              
                       }
                       else
                       {
                           miOffSetPositivo = false;

                       }


                       DBObjectCollection miColOffset;
                       ObjectId miLwObjID = ObjectId.Null;
                       Polyline miLwOffSet;

                       miColOffset = new DBObjectCollection();
                       miColOffset = iLwOrigen.GetOffsetCurves(iOffSetSigno);

                       foreach (Entity miEntidad in miColOffset)
                       {
                           miLwObjID = engCadNet.oTools.entidadAdd(miEntidad, iLayer);
                       }


                       miLwOffSet = (Polyline)tr.GetObject(miLwObjID, OpenMode.ForWrite);

                       miLwOffsetCoordenadaY = miLwOffSet.GetPoint2dAt(0).Y;
                       miLwOrigenCoordenadaY = iLwOrigen.GetPoint2dAt(0).Y;


                       //Debo de Comprobar Si el Sentido es Correcto
                       if (miOffSetPositivo)
                       {
                           if (miLwOrigenCoordenadaY > miLwOffsetCoordenadaY)
                           {
                               oTools.entidadDelete(miLwOffSet.ObjectId);
                               miColOffset = new DBObjectCollection();
                               miColOffset = iLwOrigen.GetOffsetCurves(-iOffSetSigno);

                               foreach (Entity miEntidad in miColOffset)
                               {
                                   miLwObjID = engCadNet.oTools.entidadAdd(miEntidad, iLayer);
                               }


                               miLwOffSet = (Polyline)tr.GetObject(miLwObjID, OpenMode.ForWrite);
                           }


                       }

                       else
                       {
                           if (miLwOffsetCoordenadaY > miLwOrigenCoordenadaY)
                           {
                               oTools.entidadDelete(miLwOffSet.ObjectId);
                               miColOffset = new DBObjectCollection();
                               miColOffset = iLwOrigen.GetOffsetCurves(-iOffSetSigno);

                               foreach (Entity miEntidad in miColOffset)
                               {
                                   miLwObjID = engCadNet.oTools.entidadAdd(miEntidad, iLayer);
                               }


                               miLwOffSet = (Polyline)tr.GetObject(miLwObjID, OpenMode.ForWrite);
                           }


                       }


                       miLwOffsetCoordenadaY = miLwOffSet.GetPoint2dAt(0).Y;
                       miLwOrigenCoordenadaY = iLwOrigen.GetPoint2dAt(0).Y;


                       //DEBO DE CREAR LA PERPENDICULAR           
                       Line miLine1a = new Line(iLwOrigen.StartPoint, iLwOrigen.StartPoint.getFromIncXIncY(0, -1, 0));
                       Line miLine2a = new Line(iLwOrigen.EndPoint, iLwOrigen.EndPoint.getFromIncXIncY(0, -1, 0));
                       Line miLine1b;
                       Line miLine2b;

                       //Ojo al poner 1 a veces la linea era muy corta y no hacia bien la interseccion
                       if (miLwOffSet.NumberOfVertices >= 3)
                       {
                           miLine1b = new Line(miLwOffSet.StartPoint, miLwOffSet.GetPoint3dAt(2)); 
                           miLine2b = new Line(miLwOffSet.EndPoint, miLwOffSet.GetPoint3dAt(miLwOffSet.NumberOfVertices - 3));
                       }
                       else
                       {
                           miLine1b = new Line(miLwOffSet.StartPoint, miLwOffSet.GetPoint3dAt(1)); 
                           miLine2b = new Line(miLwOffSet.EndPoint, miLwOffSet.GetPoint3dAt(miLwOffSet.NumberOfVertices - 2));
                       }

  
                       //Ahora Obtengo las Interseccion 
                       Point3dCollection miColInter = new Point3dCollection();
                       miLine1a.IntersectWith(miLine1b, Intersect.ExtendBoth, miColInter, IntPtr.Zero, IntPtr.Zero);
                       Point3d miEq1 = miColInter[0];

                       miColInter = new Point3dCollection();
                       miLine2a.IntersectWith(miLine2b, Intersect.ExtendBoth, miColInter, IntPtr.Zero, IntPtr.Zero);
                       Point3d miEq2 = miColInter[0];

                       //MOdifico la LwOffSet
                       miLwOffSet.SetPointAt(0, miEq1.Convert2d(new Plane()));

                       miLwOffSet.SetPointAt(miLwOffSet.NumberOfVertices - 1, miEq2.Convert2d(new Plane()));

                       //Añado las Verticales
                       if (iAddVerticales)
                       {
                           miLwOffSet.AddVertexAt(0, iLwOrigen.GetPoint2dAt(0), 0, 0, 0);
                           miLwOffSet.AddVertexAt(miLwOffSet.NumberOfVertices, iLwOrigen.EndPoint.Convert2d(new Plane()), 0, 0, 0);
                       }


                       //Ahora Unimos la PolilineaOrigen con su Equidistancia
                       if (iJoinEntities)
                       {
                           Polyline miLwOrigenClone = iLwOrigen.Clone() as Polyline;

                           miLwOffSet.JoinEntity(miLwOrigenClone);
                       }


                       miLwOffSet.Color = iColorSaneo;

                       tr.Commit(); //Poner al final

                       return miLwOffSet;

                   }

               }                           
        public static Polyline drawSaneoEscalon (Polyline iLwTndOff,double iEscalonHmax,Color iColor, double iPk = -1)
        {


            //Determino cual es el punto mas alto
  
            Point3d miSaneoOrigen;
            Point3d miSaneoFin;
            int miSentidoAvance = 1;


            Point2d miPtoLineaPendienteIni = iLwTndOff.StartPoint.to2d();
            Point2d miPtoLineaPendienteFin = iLwTndOff.EndPoint.to2d();

            int miEscalonDrawNum=0;
            double miEscalonDrawH=0;

            getEscalon(iLwTndOff, iEscalonHmax, ref miEscalonDrawNum, ref miEscalonDrawH);


            //Obtengo el Punto Mas Alto // Origen del Saneo
            if (miPtoLineaPendienteIni.Y > miPtoLineaPendienteFin.Y)
            {
                miSaneoOrigen = miPtoLineaPendienteIni.convertTo3D();
                miSaneoFin = miPtoLineaPendienteFin.convertTo3D();
            }
            else
            {
                miSaneoOrigen = miPtoLineaPendienteFin.convertTo3D();
                miSaneoFin = miPtoLineaPendienteIni.convertTo3D();
            }

            //Determino Si Avanzo en Sentido Positivo o Negativo
            if (miSaneoOrigen.X > miSaneoFin.X)
            {
                miSentidoAvance = -1;
            }
            else
            {
                miSentidoAvance = 1;
            }

            if (iPk != -1)
            {
                try
                {
                    oCadManager.thisEditor.WriteMessage(string.Format("\n[DEBUG] drawSaneoEscalon - PK: {0:F3}", iPk));
                    oCadManager.thisEditor.WriteMessage(string.Format("\n  - Origen: ({0:F3}, {1:F3}), Fin: ({2:F3}, {3:F3})", miSaneoOrigen.X, miSaneoOrigen.Y, miSaneoFin.X, miSaneoFin.Y));
                    oCadManager.thisEditor.WriteMessage(string.Format("\n  - Sentido avance: {0}, Num escalones: {1}, Altura escalon: {2:F3}", miSentidoAvance, miEscalonDrawNum, miEscalonDrawH));
                }
                catch {}
            }

            //Inicio el Bucle
            double miEscalonX = 0.5;

            Line miLineaEscalonH;

            Point3d miEscalonHP1 = miSaneoOrigen.getFromIncXIncY(0, -miEscalonDrawH, 0);
            Point3d miEscalonHP2 = miEscalonHP1.getFromIncXIncY(miEscalonX * miSentidoAvance, 0, 0);

            Point3dCollection miColInter;

            Point3dCollection miLwColPtoSaneoInferior = new Point3dCollection();


            for (int i = 1; i < miEscalonDrawNum; i++)
            {
                if (iPk != -1)
                {
                    try
                    {
                        oCadManager.thisEditor.WriteMessage(string.Format("\n    - Paso {0}: HP1=({1:F3}, {2:F3}), HP2_prev=({3:F3}, {4:F3})", i, miEscalonHP1.X, miEscalonHP1.Y, miEscalonHP2.X, miEscalonHP2.Y));
                    }
                    catch {}
                }

                miLineaEscalonH = new Line(miEscalonHP1, miEscalonHP2);

                miColInter = new Point3dCollection();

                Plane planoProyeccion = new Plane(Point3d.Origin, Vector3d.ZAxis);
                miLineaEscalonH.IntersectWith(iLwTndOff, Intersect.ExtendThis, planoProyeccion, miColInter, IntPtr.Zero, IntPtr.Zero);

                Point3dCollection miColInterFiltrada = new Point3dCollection();
                foreach (Point3d pto in miColInter)
                {
                    if ((pto.X - miEscalonHP1.X) * miSentidoAvance >= -1e-5)
                    {
                        miColInterFiltrada.Add(pto);
                    }
                }

                if (iPk != -1)
                {
                    try
                    {
                        oCadManager.thisEditor.WriteMessage(string.Format("\n      - Encontradas {0} intersecciones filtradas.", miColInterFiltrada.Count));
                    }
                    catch {}
                }

                if (miColInterFiltrada.Count == 1)
                {
                    miEscalonHP2 = miColInterFiltrada[0];
                }
                else if (miColInterFiltrada.Count > 1)
                {
                    miEscalonHP2 = miEscalonHP2.getPtoMasCercano(miColInterFiltrada);
                }
                else
                {
                    string msg = string.Format("Error al Obtener las Intersecciones del Saneo Terraplen en PK {0:F3}. HP1=({1:F3}, {2:F3}), HP2_prev=({3:F3}, {4:F3})", iPk, miEscalonHP1.X, miEscalonHP1.Y, miEscalonHP2.X, miEscalonHP2.Y);
                    throw new Exception(msg);
                }

                if (iPk != -1)
                {
                    try
                    {
                        oCadManager.thisEditor.WriteMessage(string.Format("\n      - HP2_post=({0:F3}, {1:F3})", miEscalonHP2.X, miEscalonHP2.Y));
                    }
                    catch {}
                }

                miLwColPtoSaneoInferior.Add(miEscalonHP1);
                miLwColPtoSaneoInferior.Add(miEscalonHP2);

                //oLine.addLine(miEscalonHP1, miEscalonHP2, "0");

                miEscalonHP1 = miEscalonHP2.getFromIncXIncY(0, -miEscalonDrawH, 0);
                miEscalonHP2 = miEscalonHP1.getFromIncXIncY(miEscalonX * miSentidoAvance, 0, 0);


            }


            //Ahora debo de Crear el ultimo Escalon Horizontal
            miLwColPtoSaneoInferior.Add(miEscalonHP1);
            miLwColPtoSaneoInferior.Add(miSaneoFin);

            if (iPk != -1)
            {
                try
                {
                    oCadManager.thisEditor.WriteMessage(string.Format("\n    - Ultimo escalon: HP1=({0:F3}, {1:F3}), Fin=({2:F3}, {3:F3})", miEscalonHP1.X, miEscalonHP1.Y, miSaneoFin.X, miSaneoFin.Y));
                }
                catch {}
            }

            //Borro la Paralela
            oTools.entidadDelete(iLwTndOff);

            return oLw.addLw2d(miLwColPtoSaneoInferior, false, "0", null,iColor);

        }
   }
}
