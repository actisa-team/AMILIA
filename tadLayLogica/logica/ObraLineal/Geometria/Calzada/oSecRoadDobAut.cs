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
    
    public  class oSecRoadDobAut : oSecRoadAbstract,ISecDrawPlus
    {

        #region "Puntos"

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
        private Point3d cAsi2;
        private Point3d cAsi3;
        private Point3d cAsi11;


        private Point3d cBerInt00;

        private Point3d cCun00;

        #endregion

        private Dictionary<int, ISecDrawPlus> mdicIsecDraw;
        private List<oMedItemModel> mMedicion;

        #region "Variables Seccion"
        private oCunetaAbstract cunetaTipoInterior;
        #endregion
        #region "Variables por PK"
        public double arcenIntAncho  { get;private set; }
        public double medianaAnchoMitad   { get; private set; }
        public double bermaIntPendiente  {get;private set; }
        #endregion
        #region "Areas"

        protected double areaBermaInterior { get; set; }
        protected double areaArcenInterior { get; set; }

        #endregion

       
          #region "Constructores"

          public oSecRoadDobAut(double iCarrilAncho,
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
                                Double iMedianaAnchoSeccionCompleta,
                                double iBermaIntPendiente,
                                oCunetaAbstract iCunetaInterior)
                              
                                


              :base(iCarrilAncho,iCarrilNum,iFirmeIntoArcen,iArcenExtAncho,iBermaExtAncho,iBermaExtPendiente,iTaludFirme,iTaludFirmeAsientoByCuneta,iCunetaPosicion,iCunetaExterior)

          {
              arcenIntAncho = iArcenIntAncho;
              medianaAnchoMitad = 0.5* iMedianaAnchoSeccionCompleta;
              bermaIntPendiente = iBermaIntPendiente;
              cunetaTipoInterior = iCunetaInterior;
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

                          throw new Exception("Caso Indeterminado ; Sección Desmonte con Talud en Desmoente");

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
                      throw new Exception("La Medición de la Sección DOB-AUT es Nula");
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

              cRoa00 = ptoOrigen.getFromIncXIncY(medianaAnchoMitad, 0, 0);
              cRoa1 = cRoa00.getFromLonPendiente(arcenIntAncho-firmeIntoArcen, peraltePendienteUno);
              cRoa2 = cRoa1.getFromLonPendiente((carrilAncho * carrilNum) + (2 * firmeIntoArcen), peraltePendienteUno);
              cRoa3 = cRoa2.getFromLonPendiente(arcenExtAncho - firmeIntoArcen, peraltePendienteUno);
              cRoa11 = cRoa3.getFromLonPendiente(bermaExtAncho,-bermaPendienteUno);


              //ArcenInterior-Exterior
              Point3d miArcenEspesor = cRoa00.getFromIncXIncY(0, -arcenEspesor, 0);
              Point3d miFirmeEspesor = cRoa00.getFromIncXIncY(0,-firmeEspesor, 0);
             

              cArcInt00 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miArcenEspesor, peraltePendienteUno, cRoa00, false, false, taludFirme, true);
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

              //Determinar que este punto no tenga X negativa y se 'meta' en la otra sección
              cAsi1 = oPoint3dExtension.getIntPtoTaludAndPtoHorizontal(cRoa00, false, false, taludFirme, cAsi00, true);

              if (cAsi1.X < 0)
              {
                  cAsi1 = cAsi00;
              }



              cAsi2 = cRoa00.getFromIncXIncY(0, -(firmeEspesor + asientoEspesor), 0);
            


              //Cuneta
              double miBermaIntLonX = (medianaAnchoMitad) - (cunetaTipoInterior.anchoSupExterior / 2);
              double miBermaIntLonY = (miBermaIntLonX * bermaIntPendiente) / 100;

              cBerInt00 = cRoa00.getFromIncXIncY(-miBermaIntLonX, -miBermaIntLonY, 0);


              //Valido Cara Inferior Cuneta no este por debajo de la Capa Inferior de la Mediana
              double miH1 = cBerInt00.Y;
              double miH2 = cunetaTipoInterior.alturaExterior;
              double miH3 = (firmeEspesor+asientoEspesor)-miH1-miH2;

              //if (miH3<0)
              //{                 
                  //HACK E110 JUAN 23.07.14
                  //+++Añadir a la clase el nombre de la sección y pasar ese nombre al Error lanzado
                  //+++Pasar el String al Fichero de Recursos
                //  throw new tadLayShare.oExSeccionGeometriaDesignNoValid("Sección Autovia Doble\n  La cota inferior de la cuneta interior es inferior a la cota inferior de la mediana");                                  
              //}

              double miEspesorTotal = firmeEspesor+ asientoEspesor;
              double miAltoCuneta = cunetaTipoInterior.alturaExterior;


              cCun00 = ptoOrigen.getFromIncXIncY(0, -(miBermaIntLonY + cunetaTipoInterior.alturaInterior), 0);



              if (taludTipo == eExcavacion.terraplen)
              {
                  cAsi3 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi2, peraltePendienteUno, cFir11, true, false, taludFirme, true);
                  cCunFir = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir11, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
                  cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi3, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
              }
              else if (taludTipo == eExcavacion.desmonte | taludTipo == eExcavacion.acota)
              {

                  //Evitar que se Crucen las Lineas de Berma Exterior e Interior
                  if (taludFirme > taludFirmeAsientoByCuneta)
                  {
                      cAsi3 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi2, peraltePendienteUno, cFir11, true, false, taludFirmeAsientoByCuneta, true);
                  }
                  else
                  {
                      cAsi3 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi2, peraltePendienteUno, cFir11, true, false, taludFirme, true);
                  }


                  if (cunetaPosicion == eCunetaPosicion.berma)
                  {
                      cCunFir = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir11, peraltePendienteUno, cRoa11, true, false, taludFirmeAsientoByCuneta, true);
                      cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi3, peraltePendienteUno, cRoa11, true, false, taludFirmeAsientoByCuneta, true);
                  }
                  else if (cunetaPosicion == eCunetaPosicion.firme)
                  {
                      cCunFir = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir11, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
                      cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi3, peraltePendienteUno, cCunFir, true, false, taludFirmeAsientoByCuneta, true);
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


              //CUNETA INTERIOR
              miCol = new Point3dCollection();
              miCol = cunetaTipoInterior.geometriaMitad(cCun00);
              milstGeometria.Add(miCol);


              //BERMA  INT
              miCol = new Point3dCollection();
              miCol = cunetaTipoInterior.geometriaMitadExt(cCun00);
              miCol.Add(cRoa00);
              miCol.Add(cAsi1);
              miCol.Add(cAsi00);

              areaBermaInterior = miCol.getArea();
              milstGeometria.Add(miCol);


              //ARCEN INT
              miCol = new Point3dCollection();
              miCol.Add(cRoa00);
              miCol.Add(cRoa1);
              miCol.Add(cArcInt11);
              miCol.Add(cArcInt00);


              areaArcenInterior = miCol.getArea();
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
              miCol.Add(cAsi3);

              areaBermaExt = miCol.getArea();
              milstGeometria.Add(miCol);


              //LW ASIENTO
              miCol = new Point3dCollection();
              miCol.Add(cFir00);
              miCol.Add(cArcInt00);
              miCol.Add(cArcInt11);
              miCol.Add(cFir1);
              miCol.Add(cFir2);
              miCol.Add(cArcExt00);
              miCol.Add(cArcExt11);
              miCol.Add(cFir11);
              miCol.Add(cAsi3);
              miCol.Add(cAsi2);
              miCol.Add(cAsi1);

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
                  miColExplanada.Add(cAsi2);
                  miColExplanada.Add(cAsi3);
                  miColExplanada.Add(cAsi11);

                  return miColExplanada;  
              }
          }
          public override Point3d ptoExplanada
          {
              get
              {
                 Point3d miRoa00 = ptoOrigen.getFromIncXIncY(medianaAnchoMitad, 0, 0);
                 Point3d miArcenExterior = miRoa00.getFromLonPendiente(arcenIntAncho + arcenExtAncho + (carrilAncho * carrilNum), peraltePendienteUno);
                 Point3d miBermaExterior = miArcenExterior.getFromLonPendiente(bermaExtAncho, -bermaPendienteUno);
                 Point3d miExplanada00 = miRoa00.getFromIncXIncY(0, -(firmeEspesor + asientoEspesor), 0);
                 Point3d miAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miExplanada00, peraltePendienteUno, miBermaExterior, true, false, taludFirme, true);

                 return miAsi11; 
   
              }
          }
          public override Point3d ptoCalzadaExterior
          {
              get { return this.cRoa11; }
          }




          #endregion


          public override double anchoCalzada
          {
              get
              {
                  double miAnchoParcial1 = carrilAncho * carrilNum;
                  double miAnchoParcial2 = arcenExtAncho;
                  double miAnchoParcial3 = bermaExtAncho;
                  double miAnchoParcial4 = arcenIntAncho;
                  double miAnchoParcial5 = medianaAnchoMitad;

                  return miAnchoParcial1 + miAnchoParcial2 + miAnchoParcial3 + miAnchoParcial4 + miAnchoParcial5;

              }
          }


          public override void setUpMaterialesById(Dictionary<int, tadLayData.dsBd.tbCapasRow> iLstCapaFirme, Dictionary<int, tadLayData.dsBd.tbCapasRow> iLstCapasArcen, Dictionary<int, tadLayData.dsBd.tbCapasRow> iLstCapasFirme, Guid iBermaMat)
          {
              mMedicion = new List<oMedItemModel>();

              //Medimos la Capas de Firme
              mMedicion.AddRange(oMedicionTools.getMedicionCapasConTalud(areaFirme, taludFirme, iLstCapaFirme,false, eCapaCalzada.FIR));

              //Medimos la Capa de Arcen
              mMedicion.AddRange(oMedicionTools.getMedicionArcen(areaArcenExt, iLstCapasArcen));

              //Medimos la Capa de Asiento
              mMedicion.AddRange(oMedicionTools.getMedicionCapaAsiento(areaAsiento, areaAsientoRecrecido, taludFirme, iLstCapasFirme,false));


              ////Medimos la Capa de Berma Exterior+Interior
              mMedicion.Add(new oMedRellenoBerma(iBermaMat, areaBermaExt+areaBermaInterior));


              //PROPIO SECCION DOBLE
              //-ARCEN INTERIOR
              mMedicion.AddRange(oMedicionTools.getMedicionArcen(areaArcenInterior, iLstCapasArcen));

              
              //Medimos la Cuneta Interior
              if (cunetaTipoInterior.tipo == eCunetaTipo.TRIANG)
              {
                 mMedicion.Add(new oMedCunetaTriangular(cunetaTipoInterior.material,0.5));
              }
              else if (cunetaTipoInterior.tipo == eCunetaTipo.TRAPEZ)
              {
               mMedicion.Add(new oMedCunetaTrapezoidal(cunetaTipoInterior.material,0.5));
              }
              else
              {
                 throw new oExEnumNotImplemented(cunetaTipoInterior.tipo.ToString());
              }

          }
    }
}
