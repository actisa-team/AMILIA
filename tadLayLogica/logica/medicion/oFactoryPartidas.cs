using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.medicion
{
   
    using tadLayData;
    using tadLayLogica.zonaGis;
    using tadLayShare;
    
    public static class oFactoryPartidas
    {


        public static oMedItemModel createPartidasFromTbMedicionesCad(dsBd.tbMaterialesRow iMatRow, double iMedicion, string iCode,bool iIsBalanceTierras, bool iIsPrecioPrincipal)
        {
 
             eNodo miNodo = (eNodo) Enum.Parse(typeof(eNodo), iCode,true);

            if (iIsBalanceTierras)
            {
                return createMedicionesPartidasBalanceTierras(iMatRow, iMedicion, iIsPrecioPrincipal, miNodo);
            }
            else
            {
                return createMedicionPartidasSimples(iMatRow, iMedicion, miNodo);
            }
       
        }



        /// <summary>
        /// FACTORY DE LAS PARTIDAS DE CAD, BALANCE TIERRAS
        /// </summary>
        public static oMedItemModel createMedicionesPartidasBalanceTierras(dsBd.tbMaterialesRow iMatRow, double iMedicion,bool iIsPrecioPrincipal,eNodo iCode)
        {

            oMedItemModel miPartida;

            if (iCode == eNodo.EXCAVACION)
            {
                miPartida = new oMedExcavacion(iMatRow, iMedicion, iIsPrecioPrincipal);

                return miPartida;
            }
            else if (iCode == eNodo.CAPASGRANULARES)
            {
                miPartida = new oMedCapaGranular(iMatRow, iMedicion, iIsPrecioPrincipal);

                return miPartida;
            }
            else if (iCode == eNodo.CAPASASIENTO)
            {
                miPartida = new oMedCapaAsiento(iMatRow, iMedicion, iIsPrecioPrincipal);

                return miPartida;
            }
            else if (iCode == eNodo.TERRAPLENandSANEOS)
            {
                miPartida = new oMedTerraplenesAndSaneos(iMatRow, iMedicion, iIsPrecioPrincipal);

                return miPartida;
            }
            else
            {
                throw new oExEnumNotImplemented(iCode.ToString());
            }     
        }



     /// <summary>
     /// FACTORY DE LAS PARTIDAS DE CAD ; SIMPLES (SOLO PRECIO PRINCIPAL + MEDICIÖN)
     /// </summary>
        public static oMedItemModel createMedicionPartidasSimples(dsBd.tbMaterialesRow iMatRow, double iMedicion, eNodo iCode)
        {

            if (iCode == eNodo.DESBROCE)
            {
                return new oMedDesbroce(iMatRow, iMedicion);
            }  
            else if (iCode == eNodo.MATERIALPLANTA)
            {
                return new oMedFirmePlanta(iMatRow, 0, iMedicion);
            }
            else if (iCode == eNodo.CUNETAS | iCode == eNodo.CUNTRA | iCode == eNodo.CUNTRI)
            { 
                return new oMedCunetas(iMatRow, iMedicion);
            }
            else if (iCode == eNodo.MURO)
            {
                return new oMedMuro(iMatRow, iMedicion);
            }
            else if (iCode == eNodo.ESTVIADUCTO)
            {
                return new oMedPuentes(iMatRow, iMedicion);
            }
            else if (iCode == eNodo.ESTTUNEL)
            {
                return new oMedTunel(iMatRow, iMedicion);
            }
            else
            {
                throw new oExEnumNotImplemented(iCode.ToString());
            }
       
        }


        /// <summary>
        /// FACTORY DE LAS MEDICIONES DE EXPROPIACION
        /// </summary>
        public static oMedItemModel createMedicionesExpropiacion(Guid iIdMat, double iMedicionM2, eNodo iCode)
        {
            
            if (iCode == eNodo.VALPRO)
            {
                return new oMedValProduccion(iIdMat, iMedicionM2);
            }
            else if (iCode == eNodo.SECPRI)
            {
                return new oMedSecPri(iIdMat, iMedicionM2);
            }
            else if (iCode == eNodo.SECSEC)
            {
                return new oMedSecSec(iIdMat, iMedicionM2);
            }
            else if (iCode == eNodo.SECTER)
            {
                return new oMedSecTer(iIdMat, iMedicionM2);
            }
            else if (iCode == eNodo.VALSUE)
            {
                return new oMedValSuelo(iIdMat, iMedicionM2);
            }
            else if (iCode == eNodo.NOURBA)
            {
                return new oMedSueloNoUrbano(iIdMat, iMedicionM2);
            }
            else if (iCode == eNodo.URBANO)
            {
                return new oMedSueloUrbano(iIdMat, iMedicionM2);
            }
            else if (iCode == eNodo.URBANI)
            {
                return new oMedSueloUrbanizable(iIdMat, iMedicionM2);
            }
            else
            {
                throw new oExEnumNotImplemented(iCode.ToString());
            }
             
        }


        public static oMedItemModel createMedicionesExpropiacion(Guid iIdMaterial, double iMedicionM2, string iCode)
        {
            eNodo miNodo =   (eNodo)Enum.Parse(typeof(eNodo), iCode, true);

            return createMedicionesExpropiacion(iIdMaterial, iMedicionM2, miNodo);
        }

        /// <summary>
        /// FACTORY DE LAS MEDICIONES DE EXPROPIACION
        /// </summary>
        public static oMedItemModel createMedicionesExpropiacionFromZonasGis(Guid iIdMat, double iMedicionM2, eGisZonas  iCode)
        {

            if (iCode == eGisZonas.SECPRI)
            {
                return new oMedSecPri(iIdMat, iMedicionM2);
            }
            else if (iCode == eGisZonas.SECSEC)
            {
                return new oMedSecSec(iIdMat, iMedicionM2);
            }
            else if (iCode == eGisZonas.SECTER)
            {
                return new oMedSecTer(iIdMat, iMedicionM2);
            }
            else if (iCode == eGisZonas.NOURBA)
            {
                return new oMedSueloNoUrbano(iIdMat, iMedicionM2);
            }
            else if (iCode == eGisZonas.URBANO)
            {
                return new oMedSueloUrbano(iIdMat, iMedicionM2);
            }
            else if (iCode == eGisZonas.URBANI)
            {
                return new oMedSueloUrbanizable(iIdMat, iMedicionM2);
            }
            else
            {
                throw new oExEnumNotImplemented(iCode.ToString());
            }
  
        }
    }
}
