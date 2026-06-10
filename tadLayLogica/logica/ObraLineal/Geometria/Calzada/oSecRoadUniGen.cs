using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Calzada
{

    using System.ComponentModel;


    using engCadNet;
    using engCadNet.extension;


    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.EditorInput;
    using Autodesk.AutoCAD.Geometry;

    using tadLayData;

    using tadLayLogica.Secciones.Geometria.Saneo;
    using tadLayLogica.logica.medicion;
    using tadLayLogica.Secciones.Geometria;
    using tadLayLogica.Secciones.Interfaz;
    using tadLayShare;


   public class oSecRoadUniGen: oSecRoadAbstract,ISecDrawPlus
    {

        private Point3d cRoa00;
        private Point3d cRoa1 ;
        private Point3d cRoa2 ;
        private Point3d cRoa3 ;
        private Point3d cRoa11;

        private Point3d cArc00;
        private Point3d cArc11;

        private Point3d cFir00;
        private Point3d cFir1;
        private Point3d cFir11;

        private Point3d cAsi00;
        private Point3d cAsi1;
        private Point3d cAsi11;

        private Point3d cCunFir;




        private List<oMedItemModel> mMedicion;


        Dictionary<int, ISecDrawPlus> mdicIsecDraw;

      #region "CONSTRUCTORES"

      public oSecRoadUniGen( double iCarrilAncho,
                             int iCarrilNum,
                            double iFirmeIntoArcen,                                                   
                            double iArcenExtAncho,
                            double iBermaExtAncho,
                            double iBermaExtPendiente,
                            double iTaludFirme,
                            double iTaludFirmeAsientoByCuneta,
                            eCunetaPosicion iCunetaPosicion,
                            oCunetaAbstract iCunetaExterior)
                            
       :base(iCarrilAncho,iCarrilNum,iFirmeIntoArcen,iArcenExtAncho,iBermaExtAncho,iBermaExtPendiente,iTaludFirme,iTaludFirmeAsientoByCuneta,iCunetaPosicion,iCunetaExterior)
      {
      
      
      
      
      
      }

        #endregion

      #region "Propiedades"


      public override double anchoCalzada
      {
          get
          {
              double miAnchoParcial1 = carrilAncho * carrilNum;
              double miAnchoParcial2 = arcenExtAncho;
              double miAnchoParcial3 = bermaExtAncho;

              return miAnchoParcial1 + miAnchoParcial2 + miAnchoParcial3;
          }
      }

  




      #endregion



      #region "INTERFACE IDRAWPLUS"
      public double pk { get; set; }
      public eLado lado { get; set; }  
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
      public bool taludDraw
      {
          get { return true; }

          set { ;}
      }
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


      public Point3dCollection envolvente
      {
          get
          {

              return lstPtoExplanada;

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
                  throw new Exception("La Medición de la Sección UNI-GEN es Nula");
              }
              else
              {
                return  mMedicion;
              }

          }
      }
      #endregion
      #region "METODOS ABSTRACTOS"
      public override void setUpByPk(double iPk, eLado iLado, Point3d iPtoOrigen, double iPeraltePcConSigno, double iFirmeEspesor, double iArcenEspesor, double iAsientoEspesor, double iTaludTerraplen)
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
              Point3dCollection miCol = new Point3dCollection();
              miCol.Add(cAsi00);
              miCol.Add(cAsi11);

              return miCol;
          }
      }

      public override Point3d ptoExplanada
      {
          get
          {
              //No puedo Tomar el cAsi1, ya que este depende de si Terraplen-Desmonte
              Point3d miPtoArcenExterior = this.ptoOrigen.getFromLonPendiente((carrilAncho * carrilNum) + arcenExtAncho, peraltePendienteUno);
              Point3d miPtoBermaExterior = miPtoArcenExterior.getFromLonPendiente(bermaExtAncho, -bermaPendienteUno);
              Point3d micAsi0 = this.ptoOrigen.getFromIncXIncY(0, -(firmeEspesor + asientoEspesor), 0);
              Point3d miPtoExplandaAprox = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(micAsi0, peraltePendienteUno,miPtoBermaExterior, true, false, taludFirme, true);

              return miPtoExplandaAprox;
          }
      }


      public override Point3d ptoCalzadaExterior
      {
          get { return this.cRoa11; }
      }



      protected override void geometria()
      {
          #region "Puntos Geometria"

          cRoa00 = ptoOrigen;
          cFir00 = cRoa00.getFromIncXIncY(0, -firmeEspesor, 0);
          cAsi00 = cRoa00.getFromIncXIncY(0, -(firmeEspesor + asientoEspesor), 0);


          cRoa1 = cRoa00.getFromLonPendiente(carrilAncho * carrilNum, peraltePendienteUno);
          cRoa2 = cRoa1.getFromLonPendiente(firmeIntoArcen, peraltePendienteUno);
          cRoa3 = cRoa2.getFromLonPendiente(arcenExtAncho - firmeIntoArcen, peraltePendienteUno);
          cRoa11 = cRoa3.getFromLonPendiente(bermaExtAncho, -bermaPendienteUno);


          //Puntos del Arcen
          Point3d miArc00 = cRoa00.getFromIncXIncY(0, -arcenEspesor, 0);
          cArc00 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miArc00, peraltePendienteUno, cRoa2, true, false, taludFirme, true);
          cArc11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(miArc00, peraltePendienteUno, cRoa3, true, false, taludFirme, true);

          //Puntos Firme 
          cFir1 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir00, peraltePendienteUno, cRoa2, true, false, taludFirme, true);
          cFir11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir00, peraltePendienteUno, cRoa3, true, false, taludFirme, true);

   



          //Debo de Generar la Sección en Función del Talud Tipo
          if (taludTipo == eExcavacion.terraplen)
          {
              
              cAsi1 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi00, peraltePendienteUno, cFir11, true, false, taludFirme, true);
              cCunFir = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir11, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
              cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi00, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
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
                  cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi00, peraltePendienteUno, cRoa11, true, false, taludFirmeAsientoByCuneta, true);
              }
              else if (cunetaPosicion == eCunetaPosicion.firme)
              {
                  cCunFir = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cFir11, peraltePendienteUno, cRoa11, true, false, taludFirme, true);
                  cAsi11 = oPoint3dExtension.getIntPtoPendienteAndPtoTalud(cAsi00, peraltePendienteUno, cCunFir, true, false, taludFirmeAsientoByCuneta, true);
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


          #region "Listado Polilineas"

          //Genero la Geometria
          List<Point3dCollection> milstGeometria = new List<Point3dCollection>();

          //LW FIRME
          Point3dCollection miCol = new Point3dCollection();
          miCol.Add(cRoa00);
          miCol.Add(cRoa1);
          miCol.Add(cRoa2);
          miCol.Add(cFir1);
          miCol.Add(cFir00);

          areaFirme = miCol.getArea();
          milstGeometria.Add(miCol);

          //LW ARCEN
          miCol = new Point3dCollection();
          miCol.Add(cRoa2);
          miCol.Add(cRoa3);
          miCol.Add(cArc11);
          miCol.Add(cArc00);

          areaArcenExt = miCol.getArea();
          milstGeometria.Add(miCol);

          //LW BERMA
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
          miCol.Add(cFir00);
          miCol.Add(cFir1);
          miCol.Add(cArc00);
          miCol.Add(cArc11);
          miCol.Add(cFir11);
          miCol.Add(cAsi1);
          miCol.Add(cAsi00);

          areaAsiento = miCol.getArea();
          milstGeometria.Add(miCol);


          lstGeometria = milstGeometria;

          //AREA RECRECIDOS //DEBAJO ARCEN, SI ESPESOR ES DISTINTO
          miCol = new Point3dCollection();
          miCol.Add(cFir1);
          miCol.Add(cArc00);
          miCol.Add(cArc11);
          miCol.Add(cFir11);

          areaAsientoRecrecido = miCol.getArea();


          #endregion
      }
      #endregion



      public override void setUpMaterialesById(Dictionary<int, dsBd.tbCapasRow> iLstCapaFirme, 
                                               Dictionary<int,dsBd.tbCapasRow> iLstCapasArcen, 
                                               Dictionary<int, dsBd.tbCapasRow> iLstCapasFirme, 
                                               Guid iBermaMat)
      {


          mMedicion = new List<oMedItemModel>();

          //Medimos la Capas de Firme
          mMedicion.AddRange(oMedicionTools.getMedicionCapasConTalud(areaFirme,taludFirme,iLstCapaFirme,true,eCapaCalzada.FIR));

          //Medimos la Capa de Arcen
          mMedicion.AddRange(oMedicionTools.getMedicionArcen(areaArcenExt,iLstCapasArcen));

          //Medimos la Capa de Asiento
          mMedicion.AddRange(oMedicionTools.getMedicionCapaAsiento(areaAsiento,areaAsientoRecrecido,taludFirme,iLstCapasFirme,true));

          //Medimos la Capa de Berma
          mMedicion.Add(new oMedRellenoBerma(iBermaMat, areaBermaExt));

      }

    }
}
