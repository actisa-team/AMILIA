using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Calzada
{
    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayLogica.zonaGis;

    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.Geometry;

    using tadLayLogica.Secciones.Geometria;
    using tadLayLogica.Secciones.Interfaz;
    using tadLayShare;
    



   public class oSeccionRoadCompletaSinGis : IDisposable
   {

       #region "Constructores"

       public oSeccionRoadCompletaSinGis()
       {

       }


       public oSeccionRoadCompletaSinGis(eSecRoadTipo iSeccionTipo, Dictionary<eLado, oSecRoadAbstract> iSeccionCompleta)
       {
           this.seccionTipo = iSeccionTipo;
           this.seccionCompleta = iSeccionCompleta;
           this.anchoCalzadaCompleta = this.getAnchoCalzada();
           this.BombeoPC = null;
       }


       public oSeccionRoadCompletaSinGis(eSecRoadTipo iSeccionTipo, Dictionary<eLado, oSecRoadAbstract> iSeccionCompleta, double iBombeoPC)
       {
           this.seccionTipo = iSeccionTipo;
           this.seccionCompleta = iSeccionCompleta;
           this.anchoCalzadaCompleta = this.getAnchoCalzada();
           this.BombeoPC = iBombeoPC;
       }


       #endregion
       #region "Propiedades"

       /// <summary>
       /// SECCION TIPO
       /// </summary>
       public eSecRoadTipo seccionTipo { get; private set; }
       /// <summary>
       /// ANCHO CALZADA COMPLETA
       /// </summary>
       public double anchoCalzadaCompleta { get; private set; }
       /// <summary>
       /// CREAR SECCIONES DOBLES CUANDO EXISTAN TUNELES o PUENTES
       /// </summary>
       public bool createSeccionesEstructurasDobles 
       {

           get
           {
               switch (this.seccionTipo)
               {
                   case eSecRoadTipo.UNIGEN:
                       return false;
                   case eSecRoadTipo.DOBAUT:
                       return true;
                   case eSecRoadTipo.DOBURB:
                       throw new oExEnumNotImplemented(this.seccionTipo.ToString());
                   case eSecRoadTipo.DOBSIN:
                       return false;
                   default:
                       throw new oExEnumNotImplemented(this.seccionTipo.ToString());
               }
           }

       }
       /// <summary>
       /// DICCIONARIO CON CADA LADO DE LA SECCION
       /// </summary>
       public Dictionary<eLado, oSecRoadAbstract> seccionCompleta { get; private set; }
       /// <summary>
       /// CALZADA IZQUIERDA
       /// </summary>
       public oSecRoadAbstract secRoadIzq
       {
           get
           {
               return this.seccionCompleta[eLado.IZQ];
           }
               
       }
       /// <summary>
       /// CALZADA DERECHA
       /// </summary>
       public oSecRoadAbstract secRoadDer 
       {
           get
           {
               return this.seccionCompleta[eLado.DER];
           }

       }
       /// <summary>
       /// BOMBEO POR CIENTO (PENDIENTE EN RECTA)
       /// </summary>
       public double? BombeoPC { get; private set; }

       #endregion


       #region "Metodos Publicos"


       public double getSeparacionEntreEstructuras()
       {

           if (this.createSeccionesEstructurasDobles)
           {

               if (this.seccionTipo == eSecRoadTipo.DOBAUT)
               {

                   oSecRoadDobAut miSeccionTipo = (oSecRoadDobAut)this.seccionCompleta[eLado.DER];

                   return miSeccionTipo.medianaAnchoMitad * 2.0;
               }
               else
               {
                   throw new oExEnumNotImplemented(this.seccionCompleta.ToString());
               }


           }
           else
           {
               throw new Exception("Error al Obtener la Separación entre Estructuras");
           }

       }

       public void Dispose()
       {
           this.seccionCompleta.Clear();
       }

       #endregion

     
       private double getAnchoCalzada()
       {
           double miAnchoIzq = this.seccionCompleta[eLado.IZQ].anchoCalzada;
           double miAnchoDer = this.seccionCompleta[eLado.DER].anchoCalzada;

           return miAnchoIzq + miAnchoDer;

       }





   
   }






    /// <summary>
    /// SECCION CALZADA COMPLETA (IZQ-DER) CONSIDERANDO GIS + DECORADORES
    /// </summary>
    public  class oSeccionRoadCompletaConGIS : oSeccionRoadCompletaSinGis
    {

        // -Variables GIS-
        // TerraplenTalud;
        // DesmonteTalud;
        // FirmeArcenEspesor;
        // AsientoEspesor;


        #region "Variables Privadas"
        private double mPeralteIzqByPk;
        private double mPeralteDerByPk;
        private oCunetaAbstract mCunetaExterior = null;
        #endregion
        #region "Constructores"
        public oSeccionRoadCompletaConGIS(oSeccionRoadCompletaSinGis iSeccionCompletaSinGis, 
                                         double iPk,  
                                         double iPeralteIzqConSigno,
                                         double iPeralteDerConSigno, 
                                         oZonaGeoMovimientoTierras iZonaMovTierra)
            :base(iSeccionCompletaSinGis.seccionTipo,iSeccionCompletaSinGis.seccionCompleta)
             
        {
            
            //Cargo el Pk
            Pk= iPk;
      
            //Zona según PK
            zonaMovTierra = iZonaMovTierra;

            //Variable Road
            mPeralteIzqByPk = iPeralteIzqConSigno;
            mPeralteDerByPk = iPeralteDerConSigno;

            //Obtengo la Cuneta Exterior
            mCunetaExterior = secRoadIzq.cunetaExterior;


           secRoadIzq.setUpByPk(iPk,
                            eLado.IZQ,
                            Point3d.Origin,
                            mPeralteIzqByPk,
                            zonaMovTierra.espesorFirme,
                            zonaMovTierra.espesorArcen,
                            zonaMovTierra.espesorAsiento,
                            zonaMovTierra.row.terraplenTalud);

           secRoadIzq.setUpMaterialesById(zonaMovTierra.capasFirme,
                                       zonaMovTierra.capasArcen,
                                       zonaMovTierra.capasAsiento,
                                       zonaMovTierra.row.bermaMat);


             secRoadDer.setUpByPk(iPk,
                            eLado.DER,
                            Point3d.Origin,
                            mPeralteDerByPk,
                            zonaMovTierra.espesorFirme,
                            zonaMovTierra.espesorArcen,
                            zonaMovTierra.espesorAsiento,
                            zonaMovTierra.row.terraplenTalud);


             secRoadDer.setUpMaterialesById(zonaMovTierra.capasFirme,
                             zonaMovTierra.capasArcen,
                             zonaMovTierra.capasAsiento,
                             zonaMovTierra.row.bermaMat);

    
            //Creo los DECORATOR
            secDecoratorIzq = getSeccionDecorator(secRoadIzq);
            secDecoratorDer = getSeccionDecorator(secRoadDer);
        }


        #endregion
        #region "Propiedades"

         public double Pk { get; set; }

         public oZonaGeoMovimientoTierras zonaMovTierra { get; private set; }

         public bool createSaneoDesmonte
         {
             get
             {
                 return this.zonaMovTierra.row.isSaneoDesmonte;
             }
         }

         public bool createSaneoTerraplen
         {
             get
             {
                 return this.zonaMovTierra.row.isSaneoTerraplen;
             }
         }



         public ISecDrawPlus secDecoratorIzq { get; private set; }
         public ISecDrawPlus secDecoratorDer { get; private set; }


         #endregion

        #region "Metodos Privados"
         private ISecDrawPlus getSeccionDecorator(oSecRoadAbstract iSecRoadDrawable)
        {

            if (iSecRoadDrawable.taludTipo == eExcavacion.desmonte)
            {
                return decorarDesmonte(iSecRoadDrawable as ISecDrawPlus);
            }
            else if (iSecRoadDrawable.taludTipo == eExcavacion.terraplen | iSecRoadDrawable.taludTipo == eExcavacion.acota)
            {
                return decorarTerraplen(iSecRoadDrawable as ISecDrawPlus);
            }
            else
            {
                throw new oExEnumNotImplemented(iSecRoadDrawable.taludTipo.ToString());
            }
        
        }






        private ISecDrawPlus decorarTerraplen(ISecDrawPlus iSecRoadDrawable)
        {

            ISecDrawPlus miMuroDrawable;
            ISecDrawPlus miBermaDrawable;
            
            //DECORATOR CASO 1 (SOLO LA SECCION)
            if (zonaMovTierra.row.terraplenConstanteIs)
            { 
                 return iSecRoadDrawable;
            }
            //DECORATOR CASO 2 (SECCION+MURO)
            else if (zonaMovTierra.row.terraplenMuroIs)
            {

                miMuroDrawable = new oTerraplenSobreMuroDrawable(zonaMovTierra.row.terraplenMuroMat,
                                                                 zonaMovTierra.row.terraplenMuroAlturaMax,
                                                                 zonaMovTierra.row.terraplenTalud,
                                                                 zonaMovTierra.row.terraplenMuroEspesor,
                                                                 zonaMovTierra.row.terraplenMuroEmpotramiento);



              miMuroDrawable.addDecorator(iSecRoadDrawable);

             return miMuroDrawable;

            }
            //DECORATOR CASO 3 (SECCION+BERMA)
            else if (zonaMovTierra.row.terraplenBermaIs)
            {

                //Decorator Berma
                miBermaDrawable = new oBermaTerraplenDrawable(new oTaludBermasModel(zonaMovTierra.row.terraplenAlturaMaxima,
                                                                                    false,
                                                                                    zonaMovTierra.row.terraplenBermaLonHor,
                                                                                    zonaMovTierra.row.terraplenBermaLonVer,
                                                                                    zonaMovTierra.row.terraplenTalud));


                miBermaDrawable.addDecorator(iSecRoadDrawable);

                return miBermaDrawable;

            }
            else
            {

                throw new oExEnumNotImplemented(string.Format(strFrmGisGeneral.eMOVTIEdesmonteCasoNull, zonaMovTierra.row.nombre));
            }
        
        
        }
        private ISecDrawPlus decorarDesmonte(ISecDrawPlus iSecRoadDrawable)
        {


            ISecDrawPlus miCunetaDrawable;
            ISecDrawPlus miMuroDrawable;
            ISecDrawPlus miBermaDrawable;


            //CREO LA CUNETA DESMONTE
            if (mCunetaExterior.tipo == eCunetaTipo.TRIANG)
            {
                oCunetaTriangularModel miCunetaModel = (oCunetaTriangularModel)mCunetaExterior;
                miCunetaDrawable = new oCunetaTriangularDrawable(miCunetaModel, zonaMovTierra.row.desmonteTalud);
            }
            else if (mCunetaExterior.tipo == eCunetaTipo.TRAPEZ)
            {
                oCunetaTrapezoidalModel miCunetaModel = (oCunetaTrapezoidalModel)mCunetaExterior;
                miCunetaDrawable = new oCunetaTrapezoidalDrawable(miCunetaModel, zonaMovTierra.row.desmonteTalud);
            }
            else
            {
                throw new oExEnumNotImplemented(mCunetaExterior.tipo.ToString());
            }


            //DECORATOR CASO 1
            if (zonaMovTierra.row.desmonteConstanteIs)
            {
                miCunetaDrawable.addDecorator(iSecRoadDrawable);

                return miCunetaDrawable;
            }

            //DECORATOR CASO 2 (CUNETA+MURO)
            if (zonaMovTierra.row.desmonteMuroIs)
            {
                
                //Decorador Cuneta Desmonte
                miCunetaDrawable.addDecorator(iSecRoadDrawable);

                //Decorator Muro Desmonte
                miMuroDrawable = new oMuroDesmonteDrawable(zonaMovTierra.row.desmonteMuroMat,
                                                           zonaMovTierra.row.desmonteMuroEspesor,
                                                           zonaMovTierra.row.desmonteMuroEmpotramiento,
                                                           zonaMovTierra.row.desmonteMuroAlturaMaxima,
                                                           zonaMovTierra.row.desmonteTalud);


                miMuroDrawable.addDecorator(miCunetaDrawable);

                return miMuroDrawable;
            
            }

            //DECORATOR CASO 3 (CUNETA+BERMA)
            if (zonaMovTierra.row.desmonteBermaIs)
            {

                //Decorador Cuneta Desmonte
                miCunetaDrawable.addDecorator(iSecRoadDrawable);



                //Decorator Berma
                miBermaDrawable = new oBermaDesmonteDrawable(zonaMovTierra.row.desmonteAlturaMaxima,
                                                             zonaMovTierra.row.desmonteBermaIniciarPie,
                                                             zonaMovTierra.row.desmonteBermaLonHor,
                                                             zonaMovTierra.row.desmonteBermaLonVer,
                                                             zonaMovTierra.row.desmonteTalud);

                miBermaDrawable.addDecorator(miCunetaDrawable);


                return miBermaDrawable;
            
            }


            throw new oExEnumNotImplemented(string.Format(strFrmGisGeneral.eMOVTIEdesmonteCasoNull,zonaMovTierra.row.nombre));




        }
        #endregion

    }
}
