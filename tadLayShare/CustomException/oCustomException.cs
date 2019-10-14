using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayShare
{
    using tadLayLan;




    public class oExInterseccionTaludToTerrenoIsNull : Exception
    {
        public oExInterseccionTaludToTerrenoIsNull(string iMensaje)
            :base(iMensaje)
        {

        }
    }



    public class oExFileNotFoundImg : Exception
    {
        public oExFileNotFoundImg(string iNombreFichero)
            : base(string.Format(strGeneralUser.uiFileNotFoundImg, iNombreFichero))
        {

        }
    }


    public class oExTramoEntronqueNoCumple : Exception
    {
        public oExTramoEntronqueNoCumple()
            : base(string.Format(strError.eTramoEntronque))
        {

        }
    }


    public class oExSeccionCalzadaNoCreada : Exception
    {
        public oExSeccionCalzadaNoCreada() 
            : base(string.Format(strError.eSeccionCalzada))
        {
           
        }
    }

    public class oExValorUserNoImplementado : Exception
    {
        public oExValorUserNoImplementado(string iValor)
            : base(string.Format(strError.eValorNoconfig,iValor))
        {


        }

    }
    public class oExMaterialNotImplemented : Exception
    {
        public oExMaterialNotImplemented (string iMatCode)
            :base(string.Format(strError.eMaterialCodeNotlmplemted,iMatCode))
        {


        }

    }
    public class oExCmbFiltroNull : Exception
    {
        public oExCmbFiltroNull(string iNombreCombo)
        : base(string.Format(strError.eComboFiltroNull,iNombreCombo)){}
      
    }
    public class oExCastDouble : Exception
    {
        public oExCastDouble(string iStr)
            : base(strError.eValidacion) { }
    
    }
    public class oExCastInt :Exception
    {
        public oExCastInt(string iStr)
            : base(strError.eValidacion) { }
    }     
    public class oExSeccionOutCartografia : Exception
    {
        public oExSeccionOutCartografia(double iPk)
            : base (string.Format(strError.eSeccionFueraCarografia,iPk))
          
        { 
        
   
        } 
    }
    public class oExRowNotFound : Exception
    {
        public oExRowNotFound(string iKey, string iTabla)
            : base(string.Format(strError.eRegistroNoEncontrado,iKey,iTabla ))
        {

        }
    }
    public class oExRowFoundMayorUno : Exception
    {
        public oExRowFoundMayorUno(string iKey, string iTabla)
            : base(string.Format(strError.eRegistroUnico, iKey, iTabla))
        {

        }
    }
    public class oExPropiedadOnlyRead : Exception
    {
        public oExPropiedadOnlyRead(string iPropiedad)
            : base(string.Format(strError.ePropiedadLectura, iPropiedad))
        {

        }
    }
    public class oExEnumNotImplemented : Exception
    {
        public oExEnumNotImplemented(string  iEnumString)
            : base(string.Format(strError.eEnumNoImplementada, iEnumString))
        {

        }
    }
    public class oExEjeBasicoDuplicate : Exception
    {
        public oExEjeBasicoDuplicate(int iEjeBasicosNum)
            : base(string.Format(strError.eEjeVisibiliadDuplicado, iEjeBasicosNum))
        {
 
        }
    }
    public class oExEjeVisibilidadNullValue : Exception
    {
        public oExEjeVisibilidadNullValue()
            : base(strError.eEjeVisibilidadNoExiste)
        { 
        
        }    
    }
    public class oExLayerNullValue : Exception
    {

        public oExLayerNullValue(string iVariable)
        
            :base(string.Format(strError.eCapaNoDefinida,iVariable))
        { 
        
        
        }

    }
    public class oExPropertieNullValue : Exception
    { 
       public oExPropertieNullValue(string iPropertiesName)
           :base (string.Format(strError.ePropiedadNula,iPropertiesName))
       {
       
       
       }

       public oExPropertieNullValue()
       : base(string.Format(strError.ePropiedadNulaSinValor))
       {


       }



    
    }


    public class oExTramoOrigenTramoDestinoNoConfig : Exception
    {
        public oExTramoOrigenTramoDestinoNoConfig()
            : base(string.Format(strError.eTramoIniTramoFinNoConfigurado))
        {


        }

    }


    public class oExSeccionGeometriaDesignNoValid : Exception
    {
        public oExSeccionGeometriaDesignNoValid(string iMensaje)
            : base(iMensaje)
        {

        }
    }

}
