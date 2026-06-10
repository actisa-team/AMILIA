using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdb;

namespace tadLayLogica.logica.medicion
{
   
    using System.ComponentModel;

    using engNet.CustomAtributos;
    using tadLayData;
    using tadLayLan;
    using tadLayLogica.datos.precios;
    using tadLayShare;
    using tadLayLan.Tdi;



    public enum ePartidaId
    {
        TER,
        ASI,
        FIR,
    }



    public enum eNodo
    {
        //Capitulo
        DESBROCE,
        CAPASASIENTO,
        TERRAPLENandSANEOS,
        EXCAVACION,
        CAPASGRANULARES,
        MATERIALPLANTA,
        CUNETAS,
        DRENAJE,
        MURO,
        ESTTUNEL,
        ESTVIADUCTO,
        BALIZAMIENTO,
        REPOSICIONSERVICIOS,
        GEOTECNICACORRECCION,
        DESVIOSPROVISIONALES,
        ACTUACIONESCOMPLE,
        MEDIDASCORRECTORAS,
        SEGURIDADYSALUD,

        CAPAGRANULARFIRME,
        CAPAGRANULARARCEN,
        
        RELTERRAPLEN,
        RELSANEOTERRAPLEN,
        RELSANEODESMONTE,
        RELBERMA,
        
        EXCDESMONTE,
        EXCSANEOTERRAPLEN,
        EXCSANEODESMONTE,
        
        CUNTRI,
        CUNTRA,

        VALPRO,

        SECPRI,
        SECSEC,
        SECTER,

        VALSUE,

        NOURBA,
        URBANO,
        URBANI,
    }





    /// <summary>
   /// CLASE ABSTRACTA MEDICION
   /// </summary>
    public abstract class oMedItemModel
    {

       
        private dsBd.tbMaterialesRow mRow = null;

        #region "Propiedades"
        [Browsable(false)]
        public Guid idMat { get;  private set; }

        [Browsable(false)]
        public dsBd.tbMaterialesRow row
        { 
            get
            {
                if (mRow == null)
                {
                  mRow= oDalMateriales.getMaterialId(idMat);
                }

                return mRow;
            }


            private set
            {
                mRow = value;
            }
        }



        [LocalizedDisplayName("uiMaterial", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 1)]
        public string material { get {return row.nombre; } }



        [LocalizedDisplayName("uiMedicion", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 2)]
        public double medicion { get; set; }

        #endregion

        #region  "Propiedades Abstractas"


        [LocalizedDisplayName("uiUd", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 3)]
        public abstract string ud { get; }



        [LocalizedDisplayName("uiEsPrecioPrincpal", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 4)]
        [DefaultValue(null)]
        public virtual bool? isPrecioPrincipal { get; set; }



        [LocalizedDisplayName("uiPrecio", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 5)]
        public abstract double precio { get; }



        [LocalizedDisplayName("uiMedicionXPrecio", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 6)]
        public abstract double coste { get; }



        [LocalizedDisplayName("uiTipo", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 7)]
        public abstract string descripcionMedicon { get; }



        [LocalizedDisplayName("uiBalanceTierras", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 8)]
        public abstract bool isBalanceTierras { get; }



        [LocalizedDisplayName("uiCodigo", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 0)]
        public abstract eNodo code { get; }

        [BindingInfo(Visible = false)]
        public abstract int orden { get; }


        [BindingInfo(Visible = false)]
        public abstract string descripcion { get; }


        #endregion

 


        public oMedItemModel(dsBd.tbMaterialesRow iRow, double iMedicion)
            :this(iRow.idMaterial,iMedicion)
        {   
            row = iRow;
        }
        public oMedItemModel(Guid iIdMaterial,  double iMedicion)
       {
           idMat = iIdMaterial;
           medicion = iMedicion;
       }


       #region "Metodos"
       public void addMedicion(double iMedToAdd)
       {
           medicion = medicion + iMedToAdd;
       }
       #endregion

    }


    public abstract class oMedBalanceTierras : oMedItemModel
    {


        public oMedBalanceTierras(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public oMedBalanceTierras(Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {

        }


        #region "Propiedades"

        public override eNodo code { get { throw new NotImplementedException(); } }
        public override bool isBalanceTierras { get { return true; } }
        public override string ud { get { return "m3"; } }
        public override double coste
        {
            get
            {
                if (isPrecioPrincipal.HasValue)
                {
                    if (isPrecioPrincipal.Value)
                    {
                        return medicion * row.precioPrincipal;
                    }
                    else
                    {
                        return medicion * row.precioSecundario;
                    }
                }
                else
                {
                    throw new oExPropertieNullValue("IsPrecioPrincipal");
                }
            }
        }



        public override double precio
        {
            get
            {
                if (isPrecioPrincipal.HasValue)
                {
                    if (isPrecioPrincipal.Value)
                    {
                        return row.precioPrincipal;
                    }
                    else
                    {
                        return row.precioSecundario;
                    }
                }
                else
                {
                    return 0;
                }
            
            }
        }
        #endregion

        #region "PropiedadesSoloLectura"

        


        #endregion



    }
    public abstract class oMedPartidasSimples : oMedItemModel
    {

        public oMedPartidasSimples(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public oMedPartidasSimples(Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {

        }



        public override bool? isPrecioPrincipal
        { 
            get
            { 
                return true;
            }
        
        set 
        { 
              throw new oExPropiedadOnlyRead("xPartida Simple (isPrecioPrincipal)");
        }
       }
        public override eNodo code { get { throw new NotImplementedException(); } }
        public override bool isBalanceTierras { get { return false; }}
        public override double coste{get{return medicion*row.precioPrincipal;}}
        public override string ud  { get {return "m3"; }}
        public override string descripcionMedicon {get{return strFrmInformes.uiPrecio;}}
        public override double precio {get{return row.precioPrincipal;}}
 
    }
    public abstract class oMedExpropiaciones : oMedItemModel
    { 
    
            public oMedExpropiaciones (dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

            public oMedExpropiaciones (Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {

        }

        public override eNodo code { get { throw new NotImplementedException(); } }
        public override string descripcion { get { throw new NotImplementedException(); } }
        public override int orden { get { throw new NotImplementedException(); } }

        public override bool? isPrecioPrincipal
        { 
            get
            { 
                return true;
            }
        
        set 
        { 
              throw new oExPropiedadOnlyRead("xPartida Simple (isPrecioPrincipal)");
        }
       }
        public override bool isBalanceTierras { get { return false; }}
        public override double coste{get{return medicion*row.precioPrincipal;}}
        public override string ud  { get {return "m2"; }}
        public override string descripcionMedicon { get { return strFrmInformes.uiPrecio; } }
        public override double precio {get{return row.precioPrincipal;}}
     
    }
    public abstract class oMedMacroPrecios : oMedPartidasSimples
    {
        public oMedMacroPrecios(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public oMedMacroPrecios(Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {

        }

        public override eNodo code { get { throw new NotImplementedException(); } }
        public override string ud { get { return strFrmInformes.uiKm; } }

    }
    public abstract class oMedRellenos : oMedBalanceTierras
    {

        public oMedRellenos(dsBd.tbMaterialesRow iRow, double iMedicion, bool iIsPrecioPrincipal)
            : base(iRow, iMedicion)
        {
            isPrecioPrincipal = iIsPrecioPrincipal;
        }


        public oMedRellenos(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public oMedRellenos(Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {

        }

        public override eNodo code { get { throw new NotImplementedException(); } }
        public override int orden { get { throw new NotImplementedException(); } }
        public override string descripcion { get { return strFrmInformes.uiRellenos; } }
        public override string descripcionMedicon
        {
            get
            {


                if (isPrecioPrincipal.HasValue)
                {

                    if (isPrecioPrincipal.Value)
                    {
                        return strFrmInformes.uiEmpleo;
                    }
                    else
                    {
                        return strFrmInformes.uiPrestamo;
                    }
                }
                else
                {
                    throw new oExPropertieNullValue("IsPrecioPrincipal");
                }
            }

        }


    }
    public abstract class oMedCapa : oMedRellenos
    {

        public int capaOrden { get; set; }


        public oMedCapa(dsBd.tbMaterialesRow iRow, int iCapaOrden, double iMedicion)
            : base(iRow, iMedicion)
        {
            capaOrden = iCapaOrden;
        }

        public oMedCapa(Guid iIdMaterial, int iCapaOrden, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {
            capaOrden = iCapaOrden;
        }

        public override string descripcion { get { return "xCapa"; } }

    }
    public abstract class oMedEstructuras : oMedPartidasSimples
    { 
           public oMedEstructuras (dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {
            
        }

           public oMedEstructuras (Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {


       }


    
    }

   #region "MACROPRECIOS"


    public class oMedDrenaje : oMedMacroPrecios
    { 
    
            public oMedDrenaje(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

       public override eNodo code { get { return eNodo.DRENAJE; } }
       public override int orden{ get {return 8;}}
       public override string descripcion { get {return "xDrenaje";}}
    }
    public class oMedBalizamiento : oMedMacroPrecios
    {

        public oMedBalizamiento(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

 
        public override eNodo code { get { return eNodo.BALIZAMIENTO; } }
        public override int orden { get { return 12; } }
        public override string descripcion { get { return strFrmInformes.uiSeñBalizam; } }
    }
    public class oMedReposicionServicios : oMedMacroPrecios
    {

        public oMedReposicionServicios(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public override eNodo code { get { return eNodo.REPOSICIONSERVICIOS; } }
        public override int orden { get { return 13; } }
        public override string descripcion { get { return strFrmInformes.uiRepServicios; } }
    }
    public class oMedGeotecnicaCorreccion : oMedMacroPrecios
    {

        public oMedGeotecnicaCorreccion(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public override eNodo code { get { return eNodo.GEOTECNICACORRECCION; } }
        public override int orden { get { return 14; } }
        public override string descripcion { get { return strFrmInformes.uiCorreGeo; } }
    }
    public class oMedDesviosProvisionales : oMedMacroPrecios
    {

        public oMedDesviosProvisionales(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public override eNodo code { get { return eNodo.DESVIOSPROVISIONALES; } }
        public override int orden { get { return 15; } }
        public override string descripcion { get { return strFrmInformes.uiDesvProv; } }
    }
    public class oMedActuacionesComple: oMedMacroPrecios
    {

        public  oMedActuacionesComple(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public override eNodo code { get { return eNodo.ACTUACIONESCOMPLE; } }
        public override int orden { get { return 16; } }
        public override string descripcion { get { return strFrmInformes.uiActuComp; } }
    }
    public class oMedMedidasCorrectoras : oMedMacroPrecios
    {

        public oMedMedidasCorrectoras(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public override eNodo code { get { return eNodo.MEDIDASCORRECTORAS; } }
        public override int orden { get { return 17; } }
        public override string descripcion { get { return strFrmInformes.uiMedCorrec; } }
    }
    public class oMedSeguridadSalud : oMedMacroPrecios
    {

        public oMedSeguridadSalud(dsBd.tbMaterialesRow iRow)
            : base(iRow,0)
        {

        }
        public override eNodo code { get { return eNodo.SEGURIDADYSALUD; } }
        public override int orden { get { return 18; } }
        public override string descripcion { get { return strFrmInformes.uiSegSalud; } }
        public override string ud {get { return "%";}}
        //OJO EL PRECIO ES PORCIENTO 
        public override double coste
        {
            get
            {
                return medicion * (row.precioPrincipal/100);
            }
        }
  
    }
    #endregion
   #region "EXPROPIACIONES"

    public class oMedValProduccion : oMedExpropiaciones
    {
     
        public oMedValProduccion (dsBd.tbMaterialesRow iRow, double iMedicion)
        : base(iRow, iMedicion)
        {
            
        }


        public oMedValProduccion (Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }

        public override eNodo code { get { return eNodo.VALPRO; } }
        public override int orden { get { return 1; } }
        public override string descripcion { get { return strFrmInformes.uiValProd; } }
    
    
    }
    public class oMedValSuelo : oMedExpropiaciones
    {

        public oMedValSuelo (dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }

        public oMedValSuelo (Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {


        }


        public override eNodo code { get { return eNodo.VALSUE; } }
        public override int orden { get { return 2; } }
        public override string descripcion { get { return strFrmInformes.uiValSuelo; } }


    }
    public class oMedSecPri : oMedValProduccion
    { 
    
        public oMedSecPri (dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {
            
        }


        public oMedSecPri(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }


        public override eNodo code { get { return eNodo.SECPRI; } }
        public override int orden { get { return 1; } }
        public override string descripcion { get { return strFrmInformes.uiSectorPrimario; } }

    
    }
    public class oMedSecSec : oMedValProduccion
    {

        public oMedSecSec(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }


        public oMedSecSec(Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {


        }


        public override eNodo code { get { return eNodo.SECSEC; } }
        public override int orden { get { return 2; } }
        public override string descripcion { get { return strFrmInformes.uiSectorSecundario; } }


    }
    public class oMedSecTer : oMedValProduccion
    {

        public oMedSecTer(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }


        public oMedSecTer(Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {


        }


        public override eNodo code { get { return eNodo.SECTER; } }
        public override int orden { get { return 3; } }
        public override string descripcion { get { return strFrmInformes.uiSectorTerciario; } }


    }

    public class oMedSueloNoUrbano : oMedValSuelo
    {

        public oMedSueloNoUrbano(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }


        public oMedSueloNoUrbano(Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {


        }


        public override eNodo code { get { return eNodo.NOURBA; } }
        public override int orden { get { return 1; } }
        public override string descripcion { get { return strFrmInformes.uiSueloNoUrbano; } }


    }
    public class oMedSueloUrbano : oMedValSuelo
    {

        public oMedSueloUrbano(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }


        public oMedSueloUrbano(Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {


        }


        public override eNodo code { get { return eNodo.URBANO; } }
        public override int orden { get { return 2; } }
        public override string descripcion { get { return strFrmInformes.uiSueloUrbano; } }


    }
    public class oMedSueloUrbanizable : oMedValSuelo
    {

        public oMedSueloUrbanizable (dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {

        }


        public oMedSueloUrbanizable (Guid iIdMaterial, double iMedicion)
            : base(iIdMaterial, iMedicion)
        {


        }


        public override eNodo code { get { return eNodo.URBANI; } }
        public override int orden { get { return 3; } }
        public override string descripcion { get { return strFrmInformes.uiSueloUrbanizable; } }


    }



    #endregion
   #region "DESBROCE"
    //1.1 ITEM DESBROCE
   public class oMedDesbroce : oMedPartidasSimples
   {

        public oMedDesbroce(dsBd.tbMaterialesRow iRow, double iMedicion)
            : base(iRow, iMedicion)
        {
            
        }


       public oMedDesbroce(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }

       public override eNodo code {get {return eNodo.DESBROCE;}}
       public override int orden  {get {return 1;}}
       public override string ud { get { return "m3"; } }
       public override string descripcion { get { return strFrmPrecios.uiDES; } }
   
   }
    #endregion
   #region "RELLENOS-TERRAPLEN-SANEOS"
   public class oMedTerraplenesAndSaneos : oMedRellenos
   {


       public oMedTerraplenesAndSaneos(dsBd.tbMaterialesRow iRow, double iMedicion, bool iIsPrecioPrincipal)
           : base(iRow, iMedicion)
       {
           isPrecioPrincipal = iIsPrecioPrincipal;
       }
       
       
       public oMedTerraplenesAndSaneos(dsBd.tbMaterialesRow iRow, double iMedicion)
           : base(iRow, iMedicion)
       {

       }

       public oMedTerraplenesAndSaneos(Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {

       }

       public override eNodo code { get { return eNodo.TERRAPLENandSANEOS; } }
       public override int orden { get { return 3; } }
       public override string descripcion { get { return strFrmInformes.uiTerraplenesYSaneos; } }



   }
   /// <summary>
   /// MEDICION RELLENO TERRAPLEN
   /// </summary>
   public class oMedRellenoTerraplen : oMedTerraplenesAndSaneos
   {

       public oMedRellenoTerraplen(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }
       public override eNodo code {get {return eNodo.RELTERRAPLEN;}}
       public override int orden  {get {return 1;}}
       public override string descripcion { get { return strFrmInformes.uiRellenoTerraplen; } }
   }
   /// <summary>
   /// MEDICION RELLENO SANEO TERRAPLEN
   /// </summary>
   public class oMedRellenoSaneoTerraplen : oMedTerraplenesAndSaneos
   {

       public oMedRellenoSaneoTerraplen(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }

       public override eNodo code {get {return eNodo.RELSANEOTERRAPLEN;}}
       public override int orden  {get {return 2;}}
       public override string descripcion { get { return strFrmInformes.uiRellenoSaneoTerr; } }
   }
   /// <summary>
   /// MEDICION RELLENO SANEO DESMONTE
   /// </summary>
   public class oMedRellenoSaneoDesmonte : oMedTerraplenesAndSaneos
   {

       public oMedRellenoSaneoDesmonte(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }

       public override eNodo code {get {return eNodo.RELSANEODESMONTE;}}
       public override int orden  {get {return 3;}}
       public override string descripcion { get { return strFrmInformes.uiRellSanDes; } }
   }
    /// <summary>
   /// MEDICION RELLENO BERMA
   /// </summary>
   public class oMedRellenoBerma : oMedTerraplenesAndSaneos
   {

       public oMedRellenoBerma(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }
       public override eNodo code {get {return eNodo.RELBERMA;}}
       public override int orden  {get {return 4;}}
       public override string descripcion { get { return strFrmInformes.uiRellBerma; } }
   }
   #endregion
   #region "EXCAVACIONES"

   /// <summary>
   /// MEDICION EXCAVACION
   /// </summary>
   public class oMedExcavacion : oMedBalanceTierras
   {

       public oMedExcavacion(dsBd.tbMaterialesRow iRow, double iMedicion, bool iIsPrecioPrincipal)
           : base(iRow, iMedicion)
       {
           isPrecioPrincipal = iIsPrecioPrincipal;
       }


       public oMedExcavacion(dsBd.tbMaterialesRow iRow, double iMedicion)
           : base(iRow, iMedicion)
       {

       }

       public oMedExcavacion(Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {

       }

       public override eNodo code { get { return eNodo.EXCAVACION; } }
       public override int orden { get { return 4; } }
       public override string descripcion { get { return strFrmInformes.uiExcavaciones + ":"; } }
       public override string descripcionMedicon
       {
           get
           {
               if (isPrecioPrincipal.HasValue)
               {

                   if (isPrecioPrincipal.Value)
                   {
                       return strFrmInformes.uiEmpleo;
                   }
                   else
                   {
                       return strFrmInformes.uiVertedero;
                   }
               }
               else
               {
                   throw new oExPropertieNullValue("IsPrecioPrincipal");
               }
           }

       }

   }

   /// <summary>
   /// MEDICION EXCAVACION TERRAPLEN
   /// </summary>
   public class oMedExcDesmonte : oMedExcavacion
   {

       public oMedExcDesmonte(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }

       public override eNodo code {get {return eNodo.EXCDESMONTE;}}
       public override int orden  {get {return 41;}}
       public override string descripcion { get { return strFrmInformes.uiExcDesm; } }
   }
    /// <summary>
   /// MEDICION EXCAVACION SANEO TERRAPLEN
   /// </summary>
   public class oMedExcSaneoDesmonte : oMedExcavacion
   {

       public oMedExcSaneoDesmonte(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }

       public override eNodo code {get {return eNodo.EXCSANEODESMONTE;}}
       public override int orden  {get {return 2;}}
       public override string descripcion { get { return strFrmInformes.uiExcSaneoDes; } }
   }
   /// <summary>
   /// MEDICION EXCAVACION SANEO TERRAPLEN
   /// </summary>
   public class oMedExcSaneoTerraplen : oMedExcavacion
   {

       public oMedExcSaneoTerraplen(Guid iIdMaterial, double iMedicion)
           :base(iIdMaterial,iMedicion)
       { 
       
       
       }
       public override eNodo code {get {return eNodo.EXCSANEOTERRAPLEN;}}
       public override int orden  {get {return 3;}}
       public override string descripcion { get { return strFrmInformes.uiExcSaneoTerr; } }
   }


   #endregion
   #region "CAPAS"

  //CAPA GRANULAR
   public  class oMedCapaGranular : oMedCapa
   {

       public oMedCapaGranular(dsBd.tbMaterialesRow iRow, double iMedicion, bool iIsPrecioPrincipal)
           : base(iRow, 0, iMedicion)
       {
           isPrecioPrincipal = iIsPrecioPrincipal;
       }



       public oMedCapaGranular(dsBd.tbMaterialesRow iRow, int iCapaOrden, double iMedicion)
           : base(iRow, iCapaOrden, iMedicion)
       {

       }

       public oMedCapaGranular(Guid iIdMaterial, int iCapaOrden, double iMedicion)
           : base(iIdMaterial, iCapaOrden, iMedicion)
       {
           capaOrden = iCapaOrden;
       }

       public override eNodo code { get { return eNodo.CAPASGRANULARES; } }
       public override int orden { get { return 5; } }
       public override string descripcion { get { return strFrmInformes.uiCapaGran; } }


   }

   /// <summary>
   /// MEDICION CAPAS GRANULARES FIRME
   /// </summary>
   public class oMedCapaGranularFirme : oMedCapaGranular
   {

       public oMedCapaGranularFirme(dsBd.tbMaterialesRow iRow, int iCapaOrden, double iMedicion)
          :base(iRow,iCapaOrden,iMedicion)
       {
           
       }


       public oMedCapaGranularFirme(Guid iIdMaterial, int iCapaOrden,double iMedicion)
           : base(iIdMaterial,iCapaOrden, iMedicion)
       {

       }

       public override eNodo code { get { return eNodo.CAPAGRANULARFIRME; } }
       public override int orden { get { return 1; } }
       public override string descripcion { get { return strFrmInformes.uiCapaGranFirme; } }
   }
   /// <summary>
   /// MEDICION CAPAS GRANULARES ARCEN
   /// </summary>
   public class oMedCapaGranularArcen : oMedCapaGranular
   {

       public oMedCapaGranularArcen(dsBd.tbMaterialesRow iRow, int iCapaOrden, double iMedicion)
          :base(iRow,iCapaOrden,iMedicion)
       {
           
       }


       public oMedCapaGranularArcen(Guid iIdMaterial,int iCapaOrden, double iMedicion)
           : base(iIdMaterial,iCapaOrden, iMedicion)
       {

       }

       public override eNodo code { get { return eNodo.CAPAGRANULARARCEN; } }
       public override int orden { get { return 2; } }
       public override string descripcion { get { return strFrmInformes.uiCapaGranularArcen; } }
   }

   /// <summary>
   /// MEDICION CAPAS ASIENTO
   /// </summary>
   public class oMedCapaAsiento : oMedCapa
   {

       public oMedCapaAsiento(dsBd.tbMaterialesRow iRow, double iMedicion, bool iIsPrecioPrincipal)
           : base(iRow, 0, iMedicion)
       {
           isPrecioPrincipal = iIsPrecioPrincipal;
       }



       public oMedCapaAsiento(dsBd.tbMaterialesRow iRow, int iCapaOrden, double iMedicion)
           : base(iRow, iCapaOrden, iMedicion)
       {

       }


       public oMedCapaAsiento(Guid iIdMaterial, int iCapaOrden, double iMedicion)
           : base(iIdMaterial, iCapaOrden, iMedicion)
       {

       }
       public override eNodo code { get { return eNodo.CAPASASIENTO; } }
       public override int orden { get { return 2; } }
       public override string descripcion { get { return strFrmInformes.uiCapaAsiento; } }
   }

   /// <summary>
   /// MEDICION MATERIALES PLANTA
   /// </summary>
   public class oMedFirmePlanta : oMedCapa
   {


       public oMedFirmePlanta(dsBd.tbMaterialesRow iRow, int iCapaOrden, double iMedicion)
          :base(iRow,iCapaOrden,iMedicion)
       {
           
       }

       public oMedFirmePlanta(Guid iIdMaterial,int iCapaOrden, double iMedicion)
           : base(iIdMaterial, iCapaOrden, iMedicion)
       {

       }

       public override bool isBalanceTierras { get { return false; } }
       public override eNodo code { get { return eNodo.MATERIALPLANTA; } }
       public override int orden { get { return 6; } }
       public override bool? isPrecioPrincipal { get { return true;}}
       public override string descripcion { get { return strFrmInformes.uiMatPlanta; } }
       public override string descripcionMedicon { get { return strFrmInformes.uiMedicion; } }
       public override double coste { get { return medicion * row.precioPrincipal; } }

   }
   #endregion
   #region "CUNETAS"



   public class oMedCunetas : oMedPartidasSimples
   {




       public oMedCunetas(dsBd.tbMaterialesRow iRow, double iMedicion)
           : base(iRow, iMedicion)
       {

       }

       public oMedCunetas(Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {

       }

       public override eNodo code { get { return eNodo.CUNETAS; } }
       public override int orden { get { return 7; } }
       public override string ud { get { return "ml"; } }
       public override string descripcion { get { return strFrmInformes.uiCunetas; } }

   }


   public  class oMedCunetaTriangular : oMedCunetas
   {
       public oMedCunetaTriangular (dsBd.tbMaterialesRow iRow, double iMedicion)
           : base(iRow, iMedicion)
       {

       }

       public oMedCunetaTriangular(Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {

       }

       public override eNodo code { get { return eNodo.CUNTRI; } }
       public override int orden { get { return 1; } }
       public override string descripcion { get { return strFrmInformes.uiCuentaTrian; } }

   }
   public class oMedCunetaTrapezoidal : oMedCunetas
   {




       public oMedCunetaTrapezoidal(dsBd.tbMaterialesRow iRow, double iMedicion)
           : base(iRow, iMedicion)
       {

       }

       public oMedCunetaTrapezoidal(Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {

       }

       public override eNodo code { get { return eNodo.CUNTRA; } }
       public override int orden { get { return 2; } }
       public override string descripcion { get { return strFrmInformes.uiCunetaTrap; } }

   }
   #endregion
   #region "ESTRUCTURAS"

   public class oMedMuro : oMedEstructuras
   {

       public oMedMuro(dsBd.tbMaterialesRow iRow, double iMedicion)
           : base(iRow, iMedicion)
       {

       }

       public oMedMuro(Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {


       }

       public override eNodo code { get { return eNodo.MURO; } }
       public override int orden { get { return 9; } }
       public override string descripcion { get { return strFrmInformes.uiMuro ; } }

   }
   public class oMedPuentes : oMedEstructuras
   {

       public oMedPuentes(dsBd.tbMaterialesRow iRow, double iMedicion)
           : base(iRow, iMedicion)
       {

       }

       public oMedPuentes(Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {


       }

       public override eNodo code { get { return eNodo.ESTVIADUCTO; } }
       public override int orden { get { return 12; } }
       public override string ud { get { return "m2"; } }
       public override string descripcion { get { return strFrmInformes.uiPuenteViaducto; } }
   }
   public class oMedTunel : oMedEstructuras
   {

       public oMedTunel(dsBd.tbMaterialesRow iRow, double iMedicion)
           : base(iRow, iMedicion)
       {

       }

       public oMedTunel(Guid iIdMaterial, double iMedicion)
           : base(iIdMaterial, iMedicion)
       {


       }

       public override eNodo code { get { return eNodo.ESTTUNEL; } }
       public override int orden { get { return 11; } }
       public override string ud { get { return strFrmInformes.uiKm; } }
       public override string descripcion { get { return strFrmInformes.uiTunel; } }
   }

   #endregion

}
