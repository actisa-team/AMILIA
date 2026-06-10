using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Calzada
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using System.ComponentModel;


    using engCadNet;
    using engCadNet.extension;
    using tadLayLan;

    using tadLayLogica.Secciones.Geometria;
    using tadLayLogica.Secciones.Interfaz;
    using tadLayLogica.Secciones.Geometria.Saneo;
    using tadLayLogica.logica.medicion;
    using tadLayShare;
    
    public  class oSecRoadDobAutSinMediana : oSecRoadAbstract,ISecDrawPlus
    {

          #region "Fields Puntos"

        private Point3d cRoa00;
        private Point3d cRoa1;
        private Point3d cRoa2;
        private Point3d cRoa3;
        private Point3d cRoa11;

        private Point3d cArcInt00;
        private Point3d cArcInt11;

        private Point3d cArcExt00;
        private Point3d cArcExt11;


        private Point3d cFir00;
        private Point3d cFir1;
        private Point3d cFir2;
        private Point3d cFir11;

        private Point3d cCunFir;

        private Point3d cAsi00;
        private Point3d cAsi1;
        private Point3d cAsi11;



        #endregion
          #region "Fields Privados"

        private Dictionary<int, ISecDrawPlus> mdicIsecDraw;
        private List<oMedItemModel> mMedicion;

        private double arcenInteriorAncho;
        private double arcenInteriorArea;

        private string mBarreraBloqueNombreCoExtension;

        #endregion
          #region "Constructores"

          public oSecRoadDobAutSinMediana(double iCarrilAncho,
                                          int iCarrilNum,
                                          double iFirmeIntoArcen,
                                          double iArcenExtAncho,
                                          double iBermaExtAncho,
                                          double iBermaExtPendiente,
                                          double iTaludFirme,
                                          double iTaludFirmeAsientoByCuneta,
                                          eCunetaPosicion iCunetaPosicion,
                                          oCunetaAbstract iCunetaExterior,
                                          double iArcenIntAncho,
                                          string iBarreraBloqueNameConExtension)
                                         
                                                                                
              :base(iCarrilAncho,iCarrilNum,iFirmeIntoArcen,iArcenExtAncho,iBermaExtAncho,iBermaExtPendiente,iTaludFirme,iTaludFirmeAsientoByCuneta,iCunetaPosicion,iCunetaExterior)

          {
              arcenInteriorAncho = iArcenIntAncho;
              mBarreraBloqueNombreCoExtension = iBarreraBloqueNameConExtension;
          }


        #endregion
          #region "INTERFACE IDRAW"

          public double pk { get; set; }
          public eLado lado { get; set; }

          /// <summary>
          /// Puntos Envolvvente
          /// </summary>
          public Point3dCollection envolvente
          {
              get
              {
                  return lstPtoExplanada;
              }
          }
          public Dictionary<int, ISecDrawPlus> dicIsecDraw
          {
              get
              {
                  if (mdicIsecDraw == null)
                  {
                      mdicIsecDraw = new Dictionary<int, ISecDrawPlus>();

                      mdicIsecDraw.Add(0, this);

                  }

                  return mdicIsecDraw;

              }
              set { ; }


          }
          public ISecDrawPlus parent
          {
              get { throw new NotImplementedException("La seccion Base no tiene Padre."); }
          }
          public ISecDrawPlus previo
          {
              get { throw new NotImplementedException("La seccion Base no tiene Previos."); }
          }
          public Point3dCollection taludLstPol
          {
              get
              {

                  Point3dCollection mitaludLstPol = new Point3dCollection();

                  mitaludLstPol.Add(cAsi11);


                  switch (taludTipo)
                  {
                      case eExcavacion.desmonte:

                          throw new Exception("Caso Indeterminado ; Sección Desmonte con Talud en Desmonte");

                      case eExcavacion.terraplen:

                          mitaludLstPol.Add(cAsi11.getFromTalud(true, false, taludTerraplen, 1));

                          return mitaludLstPol;

                      case eExcavacion.acota:

                          taludLstPol.Add(cAsi11.getFromIncXIncY(0, 1, 0));

                          return mitaludLstPol;

                      default:

                          throw new oExEnumNotImplemented(taludTipo.ToString());


                  }


              }


              set { ;}




          }
          public bool taludDraw { get { return true; } set { ;} }
          public void addDecorator(ISecDrawPlus iSecDecorator)
          {
              throw new Exception("No se Puede Añadir un Decorator a la Sección Principal");
          }


          //Configurar el Punto de Insercción del Siguiente Decorator
          public Point3d ptoInsertChild
          {

              get
              {
                  switch (taludTipo)
                  {
                      case eExcavacion.desmonte:

                          if (cunetaPosicion == eCunetaPosicion.berma)
                          {
                              return cRoa11;
                          }
                          else if (cunetaPosicion == eCunetaPosicion.firme)
                          {
                              return cCunFir;
                          }
                          else
                          {
                              throw new oExEnumNotImplemented(cunetaPosicion.ToString());
                          }

                      case eExcavacion.terraplen:

                          return cAsi11;

                      case eExcavacion.acota:

                          return cAsi11;

                      default:
                          throw new oExEnumNotImplemented(taludTipo.ToString());
                  }


              }



          }

          /// <summary>
          /// Metodo Dibujar Decorator
          /// </summary>
          public void draw(string iLayer, Matrix3d? iMatrix)
          {


              if (lstGeometria != null)
              {
                  Color miColor;


                  if (lado == eLado.IZQ)
                  {
                      miColor = oSeccionDecoradorParent.colorSeccionIzq;
                  }
                  else
                  {
                      miColor = oSeccionDecoradorParent.colorSeccionDer;
                  }


                  foreach (Point3dCollection miColLw in lstGeometria)
                  {
                      oLw.addLw2d(miColLw, false, iLayer, iMatrix, miColor);
                  }

                  //Inserto la Barrera
                  engCadNet.oBlock.insertBlockReference(mBarreraBloqueNombreCoExtension, oTadil.data.Files.folderCadSecBar, Point3d.Origin, iLayer, iMatrix.Value);
              }
          }


          #endregion
          #region "INTERFACE MEDICION"

         
          public List<oMedItemModel> medicion
          {

              get
              {
                  if (mMedicion == null)
                  {
                      throw new Exception("La Medición de la Sección Doble Autovia Sin Mediana es Nula");
                  }
                  else
                  {
                      return mMedicion;
                  }
              }
          }


          #endregion
          #region "METODOS ABSTRACTOS"

          public override void setUpByPk(double iPk, 
                                         eLado iLado, 
                                         Point3d iPtoOrigen, 
                                         double iPeraltePcConSigno, 
                                         double iFirmeEspesor, 
                                         double iArcenEspesor, 
                                         double iAsientoEspesor, 
                                         double iTaludTerraplen)
          {
              
              pk = iPk;
              lado = iLado;
              ptoOrigen = iPtoOrigen;
              peralte = iPeraltePcConSigno;
              firmeEspesor = iFirmeEspesor;
              arcenEspesor = iArcenEspesor;
              asientoEspesor = iAsientoEspesor;
              taludTerraplen = iTaludTerraplen;

              geometria();
          }



          protected override void geometria()
          {
              #region "Puntos Geometria"

              cRoa00 = ptoOrigen;
              cRoa1 = cRoa00.getFromLonPendiente(arcenInteriorAncho-firmeIntoArcen, peraltePendienteUno);
              cRoa2 = cRoa1.getFromLonPendiente((carrilAncho * carrilNum) + (2 * firmeIntoArcen), peraltePendienteUno);
              cRoa3 = cRoa2.getFromLonPendiente(arcenExtAncho - firmeIntoArcen, peraltePendienteUno);
              cRoa11 = cRoa3.getFromLonPendiente(bermaExtAncho,-bermaPendienteUno);


              //ArcenInterior-Exterior
              Point3d miArcenEspesor = cRoa00.getFromIncXIncY(0, -arcenEspesor, 0);
              Point3d miFirmeEspesor = cRoa00.getFromIncXIncY(0,-firmeEspesor, 0);


              cArcInt00 = cRoa00.getFromIncXIncY(0, -arcenEspesor, 0);
              cArcInt11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miArcenEspesor, peraltePendienteUno, cRoa1, false, false, taludFirme, true);

              cArcExt00 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miArcenEspesor, peraltePendienteUno, cRoa2, true, false, taludFirme, true);
              cArcExt11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miArcenEspesor, peraltePendienteUno, cRoa3, true, false, taludFirme, true);
              
              //Firme
              cFir1 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miFirmeEspesor, peraltePendienteUno, cRoa1, false, false, taludFirme, true);
              cFir2 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miFirmeEspesor, peraltePendienteUno, cRoa2, true, false, taludFirme, true);

              cFir00 = oPoint3dExtension.getIntPtoTwoLinesExtendLine1(cFir1, cFir2, cRoa00, cArcInt00, true);
              cFir11 = oPoint3dExtension.getIntPtoTwoLinesExtendLine1(cFir1, cFir2, cRoa3, cArcExt11, true);

              //Explanda
              cAsi00 = ptoOrigen.getFromIncXIncY(0, -(firmeEspesor + asientoEspesor), 0);    
              


              //Talud Exterior
              if (taludTipo == eExcavacion.terraplen)
              {
                  cAsi1 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi00, peraltePendienteUno, cFir11, true, false, taludFirme, true);
                  cCunFir = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir11, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
                  cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi1, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
              }
              else if (taludTipo == eExcavacion.desmonte | taludTipo == eExcavacion.acota)
              {

                  //Evitar que se Crucen las Lineas de Berma Exterior e Interior
                  if (taludFirme > taludFirmeAsientoByCuneta)
                  {
                      cAsi1 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi00, peraltePendienteUno, cFir11, true, false, taludFirmeAsientoByCuneta, true);
                  }
                  else
                  {
                      cAsi1 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi00, peraltePendienteUno, cFir11, true, false, taludFirme, true);
                  }


                  if (cunetaPosicion == eCunetaPosicion.berma)
                  {
                      cCunFir = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir11, peraltePendienteUno, cRoa11, true, false, taludFirmeAsientoByCuneta, true);
                      cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi1, peraltePendienteUno, cRoa11, true, false, taludFirmeAsientoByCuneta, true);
                  }
                  else if (cunetaPosicion == eCunetaPosicion.firme)
                  {
                      cCunFir = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir11, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
                      cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi1, peraltePendienteUno, cCunFir, true, false, taludFirmeAsientoByCuneta, true);
                  }
                  else
                  {
                      throw new oExEnumNotImplemented(cunetaPosicion.ToString());
                  }

              }
              else
              {
                  throw new oExEnumNotImplemented(taludTipo.ToString());
              }




              #endregion
              #region "Listado Polilienas"

              //Genero la Geometria
              List<Point3dCollection> milstGeometria = new List<Point3dCollection>();
              Point3dCollection miCol;


  
              //ARCEN INT
              miCol = new Point3dCollection();
              miCol.Add(cRoa00);
              miCol.Add(cRoa1);
              miCol.Add(cArcInt11);
              miCol.Add(cArcInt00);


              arcenInteriorArea = miCol.getArea();
              milstGeometria.Add(miCol);


              //LW FIRME
              miCol = new Point3dCollection();
              miCol.Add(cRoa1);
              miCol.Add(cRoa2);
              miCol.Add(cFir2);
              miCol.Add(cFir1);

              areaFirme = miCol.getArea();
              milstGeometria.Add(miCol);


              //ARCEN EXT
              miCol = new Point3dCollection();
              miCol.Add(cRoa2);
              miCol.Add(cRoa3);
              miCol.Add(cArcExt11);
              miCol.Add(cArcExt00);

              areaArcenExt = miCol.getArea();
              milstGeometria.Add(miCol);

              //LW BERMA EXT
              miCol = new Point3dCollection();
              miCol.Add(cRoa3);
              miCol.Add(cRoa11);
              miCol.Add(cCunFir);
              miCol.Add(cAsi11);
              miCol.Add(cAsi1);

              areaBermaExt = miCol.getArea();
              milstGeometria.Add(miCol);


              //LW ASIENTO
              miCol = new Point3dCollection();
              miCol.Add(cArcInt00);
              miCol.Add(cArcInt11);
              miCol.Add(cFir1);
              miCol.Add(cFir2);
              miCol.Add(cArcExt00);
              miCol.Add(cArcExt11);
              miCol.Add(cAsi1);
              miCol.Add(cAsi00);

              areaAsiento = miCol.getArea();
              milstGeometria.Add(miCol);


              //AREA RECRECIDOS //DEBAJO ARCEN, SI ESPESOR ES DISTINTO
              double miAreaAsientoRecrecidoArcenInterior;
              double miAreaAsientoRecrecidoArcenExterior;

              //Recrecido Interior
              miCol = new Point3dCollection();
              miCol.Add(cArcInt00);
              miCol.Add(cArcInt11);
              miCol.Add(cFir1);
              miCol.Add(cFir00);

              miAreaAsientoRecrecidoArcenInterior = miCol.getArea();

              //Recrecido Exterior
              miCol = new Point3dCollection();
              miCol.Add(cArcExt00);
              miCol.Add(cArcExt11);
              miCol.Add(cFir11);
              miCol.Add(cFir2);

              miAreaAsientoRecrecidoArcenExterior = miCol.getArea();

              areaAsientoRecrecido = miAreaAsientoRecrecidoArcenInterior + miAreaAsientoRecrecidoArcenExterior;

              //CARGO LA VARIABLE
              lstGeometria = milstGeometria;

              #endregion  
          }


          public override eExcavacion taludTipo
          {
              get
              {
                  return oSeccionDecoradorParent.funGetTerrenoCorreccionTnd(pk, lado, ptoExplanada);
              }
          }


          public override Point3dCollection lstPtoExplanada
          {
              get
              {
                  
                  Point3dCollection miColExplanada = new Point3dCollection();

                  //Añado los Puntos Inferiores de la Explanada
                  miColExplanada.Add(cAsi00);
                  miColExplanada.Add(cAsi1);
                  miColExplanada.Add(cAsi11);

                  return miColExplanada;  
              }
          }


          public override Point3d ptoExplanada
          {
              get
              {
                 Point3d miRoa00 = ptoOrigen;
                 Point3d miPtoArcenExterior = miRoa00.getFromLonPendiente(arcenInteriorAncho + arcenExtAncho + (carrilAncho * carrilNum), peraltePendienteUno);
                 Point3d miPtoBermaExterior = miPtoArcenExterior.getFromLonPendiente(bermaExtAncho, -bermaPendienteUno);
                 Point3d miPtoExplanada00 = miRoa00.getFromIncXIncY(0, -(firmeEspesor + asientoEspesor), 0);
                 Point3d miAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miPtoExplanada00,peraltePendienteUno,miPtoBermaExterior,true,false,taludFirme,true);

                 return miAsi11;

              }
          }

          public override Point3d ptoCalzadaExterior
          {
              get { return this.cRoa11; }
          }




          #endregion
          #region "METODOS PUBLICOS"
          public override double anchoCalzada
          {
              get
              {
                  double miAnchoParcial1 = carrilAncho * carrilNum;
                  double miAnchoParcial2 = arcenExtAncho;
                  double miAnchoParcial3 = bermaExtAncho;
                  double miAnchoParcial4 = arcenInteriorAncho;

                  return miAnchoParcial1 + miAnchoParcial2 + miAnchoParcial3 + miAnchoParcial4;

              }
          }
          public override void setUpMaterialesById(Dictionary<int, tadLayData.dsBd.tbCapasRow> iLstCapaFirme, Dictionary<int, tadLayData.dsBd.tbCapasRow> iLstCapasArcen, Dictionary<int, tadLayData.dsBd.tbCapasRow> iLstCapasFirme, Guid iBermaMat)
          {
              mMedicion = new List<oMedItemModel>();

              //Medimos la Capas de Firme
              mMedicion.AddRange(oMedicionTools.getMedicionCapasConTalud(areaFirme, taludFirme, iLstCapaFirme, false, eCapaCalzada.FIR));

              //Medimos la Capa de Arcen Exterior es Simetrico
              mMedicion.AddRange(oMedicionTools.getMedicionArcen(areaArcenExt, iLstCapasArcen));

              //Medimos la Capa de Arcen Interior NO es simetrico !Ojo Signo Negativo Talud Firme Zona Sperior mas ancha que la inferior
              mMedicion.AddRange(oMedicionTools.getMedicionCapasConTalud(arcenInteriorArea, -taludFirme, iLstCapasArcen, true, eCapaCalzada.ARC));

              //Medimos la Capa de Asiento
              mMedicion.AddRange(oMedicionTools.getMedicionCapaAsiento(areaAsiento, areaAsientoRecrecido, taludFirme, iLstCapasFirme, true));

              ////Medimos la Capa de Berma Exterior
              mMedicion.Add(new oMedRellenoBerma(iBermaMat, areaBermaExt));


          }
          #endregion


    }
}
