using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.Secciones.Calzada
{

    using tadLayLogica.datos;

    using engNet.ClassT;
    using tadLayData;
    using tadLayLogica.datos.Secciones;
    using tadLayLogica.Secciones.Geometria;
    using tadLayLogica.Secciones.Calzada;
    using tadLayShare;
    
    public  class oFactorySeccionCalzada
    {



        public static double getSeccionRoadAnchoPlataforma(eSecRoadTipo iSeccionRoadTipo, Guid iIdSeccion)
        {
            
            oSeccionRoadCompletaSinGis miSeccionCompletaSinGis = createSeccionRoad (iSeccionRoadTipo,iIdSeccion);

            return miSeccionCompletaSinGis.anchoCalzadaCompleta;
                
        }



        public static oSeccionRoadCompletaSinGis createSeccionRoad (eSecRoadTipo iSecRoadTipo, Guid iIdSeccion)
        {
            #region "Variables"

            Dictionary<eLado, oSecRoadAbstract> miLstSec = new Dictionary<eLado, oSecRoadAbstract>();
            oSecRoadAbstract miSecIzq;
            oSecRoadAbstract miSecDer;

            #endregion
            #region "CALZADA UNICA"
          
            if (iSecRoadTipo == eSecRoadTipo.UNIGEN)
            {

                dsBd.tbRoadUniGenRow miRow =  oDalTabRoadUnica.getById(iIdSeccion);

                double miBombeoPC = miRow.bombeoPC;

                eCunetaPosicion miCunetaPosicion = (eCunetaPosicion)Enum.Parse(typeof(eCunetaPosicion), miRow.cunetaPosicion, true);

                eCunetaTipo miCunetaTipoExterior = (eCunetaTipo)Enum.Parse(typeof(eCunetaTipo), miRow.idCunetaTipo, true);

                oCunetaAbstract miCunetaExterior = oFactorySeccionCunetas.createCuneta(miCunetaTipoExterior, miRow.idCunetaGeo, miRow.idCunetaMaterial);

              
                miSecIzq = new oSecRoadUniGen(miRow.carrilAncho,
                                                 miRow.carrilIzqNum,
                                                 miRow.firmeIntoArcen,
                                                 miRow.arcenExtAncho,
                                                 miRow.bermaExtAncho,
                                                 miRow.bermaExtPendiente,
                                                 miRow.firmeTalud,
                                                 miCunetaExterior.taludCuneta,
                                                 miCunetaPosicion,
                                                 miCunetaExterior);


                miSecDer = new oSecRoadUniGen(miRow.carrilAncho,
                                                miRow.carrilDerNum,
                                                miRow.firmeIntoArcen,
                                                miRow.arcenExtAncho,
                                                miRow.bermaExtAncho,
                                                miRow.bermaExtPendiente,
                                                miRow.firmeTalud,
                                                miCunetaExterior.taludCuneta,
                                                miCunetaPosicion,
                                                miCunetaExterior);
                                               


                miLstSec.Add(eLado.IZQ, miSecIzq);
                miLstSec.Add(eLado.DER, miSecDer);

                return new oSeccionRoadCompletaSinGis(iSecRoadTipo, miLstSec,miBombeoPC);
            }

            #endregion
            #region "AUTOVIA CON MEDIANA"
            else if (iSecRoadTipo == eSecRoadTipo.DOBAUT)
            {
                dsBd.tbRoadDobleAutoviaRow miRow = oDalTabRoadDobleAutovia.getAutoviaById(iIdSeccion);

                double miBombeoPC = miRow.bombeoPC;
                
                eCunetaPosicion miCunetaPosicion = (eCunetaPosicion)Enum.Parse(typeof(eCunetaPosicion), miRow.cunetaPosicion, true);
                eCunetaTipo miCunetaTipoExterior = (eCunetaTipo)Enum.Parse(typeof(eCunetaTipo), miRow.idCunetaTipoExt, true);

                oCunetaAbstract miCunetaExterior = oFactorySeccionCunetas.createCuneta(miCunetaTipoExterior, miRow.idCunetaGeoExt,miRow.idCunetaMaterialExt);

                eCunetaTipo miCunetaTipoInterior = (eCunetaTipo)Enum.Parse(typeof(eCunetaTipo), miRow.idCunetaTipoInt, true);
                oCunetaAbstract miCunetaInterior = oFactorySeccionCunetas.createCuneta(miCunetaTipoInterior, miRow.idCunetaGeoInt,miRow.idCunetaMaterialInt);

             
                miSecIzq = new oSecRoadDobAut(miRow.carrilAncho,
                                            miRow.carrilIzqNum,
                                            miRow.firmeIntoArcen,
                                            miRow.arcenExtAncho,
                                            miRow.bermaExtAncho,
                                            miRow.bermaExtPendiente,
                                            miRow.firmeTalud,
                                            miCunetaExterior.taludCuneta,
                                            miCunetaPosicion,
                                            miCunetaExterior,
                                            miRow.arcenIntAncho,
                                            miRow.medianaAncho,
                                            miRow.bermaIntPendiente,
                                            miCunetaInterior);


                miSecDer = new oSecRoadDobAut(miRow.carrilAncho,
                                            miRow.carrilDerNum,
                                            miRow.firmeIntoArcen,
                                            miRow.arcenExtAncho,
                                            miRow.bermaExtAncho,
                                            miRow.bermaExtPendiente,
                                            miRow.firmeTalud,
                                            miCunetaExterior.taludCuneta,
                                            miCunetaPosicion,
                                            miCunetaExterior,
                                            miRow.arcenIntAncho,
                                            miRow.medianaAncho,
                                            miRow.bermaIntPendiente,
                                            miCunetaInterior);
                                        



               miLstSec.Add(eLado.IZQ, miSecIzq);
               miLstSec.Add(eLado.DER, miSecDer);

               return new oSeccionRoadCompletaSinGis(iSecRoadTipo, miLstSec,miBombeoPC);
            }
            #endregion
            #region "AUTOVIA SIN MEDIANA"

            else if (iSecRoadTipo == eSecRoadTipo.DOBSIN)
            {
                dsBd.tbRoadDobleSinMedianaRow miRow = oDalTbRoadDobleSinMediana.getAutoviaById(iIdSeccion);

                double miBombeoPC = miRow.bombeoPC;

                eCunetaPosicion miCunetaPosicion = (eCunetaPosicion)Enum.Parse(typeof(eCunetaPosicion), miRow.cunetaPosicion, true);
                eCunetaTipo miCunetaTipo = (eCunetaTipo)Enum.Parse(typeof(eCunetaTipo), miRow.idCunetaTipo, true);

                oCunetaAbstract miCunetaExterior = oFactorySeccionCunetas.createCuneta(miCunetaTipo, miRow.idCunetaGeo, miRow.idCunetaMaterial);


                miSecIzq = new oSecRoadDobAutSinMediana(miRow.carrilAncho,
                                                        miRow.carrilIzqNum,
                                                        miRow.firmeIntoArcen,
                                                        miRow.arcenExtAncho,
                                                        miRow.bermaExtAncho,
                                                        miRow.bermaExtPendiente,
                                                        miRow.firmeTalud,
                                                        miCunetaExterior.taludCuneta,
                                                        miCunetaPosicion,
                                                        miCunetaExterior,
                                                        miRow.arcenIntAncho,
                                                        miRow.barreraDwg);



                miSecDer = new oSecRoadDobAutSinMediana(miRow.carrilAncho,
                                                        miRow.carrilDerNum,
                                                        miRow.firmeIntoArcen,
                                                        miRow.arcenExtAncho,
                                                        miRow.bermaExtAncho,
                                                        miRow.bermaExtPendiente,
                                                        miRow.firmeTalud,
                                                        miCunetaExterior.taludCuneta,
                                                        miCunetaPosicion,
                                                        miCunetaExterior,
                                                        miRow.arcenIntAncho,
                                                        miRow.barreraDwg);
                                           

                miLstSec.Add(eLado.IZQ, miSecIzq);
                miLstSec.Add(eLado.DER, miSecDer);

                return new oSeccionRoadCompletaSinGis(iSecRoadTipo, miLstSec,miBombeoPC);
       
            }
            #endregion
            #region "AUTOVIA URBANA"

            else if (iSecRoadTipo == eSecRoadTipo.DOBURB)
            {

                throw new oExEnumNotImplemented("Autovia Urbana No Implementada");
            }


            #endregion
            #region "NO IMPLEMENTADA"

            else
            {
                throw new oExEnumNotImplemented(iSecRoadTipo.ToString());
            }

            #endregion        
        }



    }
}
