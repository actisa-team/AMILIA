using System;
using tadLayShare;

namespace tadLayLogica.logica.EjeBasicoNew
{

    using tadLayShare.puntos;
    using engNet.Extension.Enumeraciones;
    using tadLayLogica.estudioTipo;
    using engCadNet;
    using Autodesk.AutoCAD.Geometry;
    using tadLayLan;
    
    public class oFactoryTramoSalidaLlegada
    {


        public static oTramoEjeBasico createTramoSalidaLlegada(bool isTramoSalida, int iIdAbanico, int iIdTramo,oP3dSalidaLlegada iPtoSalida, oRoadDes iRoadDesign, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, double iLonDiscretizacionTramoProyecto, bool isTramoInvertido)
        {


            string miTramoNombre;

                if (isTramoSalida)
                {
                    miTramoNombre= "Tramo Salida";
                }
                else
                {
                   miTramoNombre = "Tramo Llegada";
                }



            //CASO 1
            if (iPtoSalida.isRotondaSinPendiente)
            {
                return null;
            }
            //CASO 2
            else if (iPtoSalida.isRotondaConPendienteK)
            {
                throw new Exception(miTramoNombre + strError.ecasoIncompatible);
            }
            //CASO 3 CASO ROTONDA
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud == null && iPtoSalida.pendientePC == null)
            {
                
                double miLongitudSalida = iRoadDesign.AijMinSalidaLlegada;

                oTramoEjeBasico miTramoIni = new oTramoEjeBasico(iIdAbanico,iIdTramo,iPtoSalida, miLongitudSalida, iPtoSalida.azimutGrados.Value,eTramoTipoEjeBasico.avanceCorto);

                //Realizo las Validaciones del Tramo
                miTramoIni.validarTramoP2InsideTerreno();
                miTramoIni.validarTramoZonasNoPaso();
                miTramoIni.createSeccion(iLonDiscretizacionTramoProyecto, iRoadPendientes, iEstudioData, false);

                if (!miTramoIni.isTramoValido)
                {
                    if (miTramoNombre == "Tramo Salida")
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoSalidaNoCumple);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoLlegadaNoCumple);
                    }
                    miTramoIni.isTramoValido = true; 
                }
               
                    return miTramoIni; 
            }

            //CASO 4 DATOS AZIMUT & PENDIENTE
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud == null && iPtoSalida.pendientePC != null)
            {

                double miLongitudSalida = iRoadDesign.AijMinSalidaLlegada;
                double miPendiente = iPtoSalida.pendientePC.Value;
                if (isTramoInvertido) miPendiente = miPendiente * (-1);

                oTramoEjeBasico miTramoIni = new oTramoEjeBasico(iIdAbanico, iIdTramo, iPtoSalida, miLongitudSalida, iPtoSalida.azimutGrados.Value, eTramoTipoEjeBasico.avanceCorto);

                //Realizo las Validaciones del Tramo
                miTramoIni.validarTramoP2InsideTerreno();
                miTramoIni.validarTramoZonasNoPaso();
                miTramoIni.createSeccionTramoInicialFinal(iLonDiscretizacionTramoProyecto, miPendiente, iEstudioData);

                if (!miTramoIni.isTramoValido)
                {
                    if (miTramoNombre == "Tramo Salida")
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoSalidaNoCumple);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoLlegadaNoCumple);
                    }
                    miTramoIni.isTramoValido = true;
                }
                
                
                    return miTramoIni;
              

            }
            //CASO 5
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud != null && ! iPtoSalida.isLonMinimaRecta.Value && iPtoSalida.pendientePC == null)
            {

                if (iRoadDesign.AijMinSalidaLlegada > iPtoSalida.longitud.Value)
                {
                    throw new Exception(string.Format(strError.eLongitudMenorAjmin, miTramoNombre));
                }

                oTramoEjeBasico miTramoIni = new oTramoEjeBasico(iIdAbanico, iIdTramo, iPtoSalida, iPtoSalida.longitud.Value, iPtoSalida.azimutGrados.Value, eTramoTipoEjeBasico.avanceCorto);

                //Realizo las Validaciones del Tramo
                miTramoIni.validarTramoP2InsideTerreno();
                miTramoIni.validarTramoZonasNoPaso();
                miTramoIni.createSeccion(iLonDiscretizacionTramoProyecto, iRoadPendientes, iEstudioData, false);

                if (!miTramoIni.isTramoValido)
                {
                    if (miTramoNombre == "Tramo Salida")
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoSalidaNoCumple);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoLlegadaNoCumple);
                    }
                    miTramoIni.isTramoValido = true;
                }
             
                    return miTramoIni;
               
            }
            //CASO 6
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud != null && iPtoSalida.isLonMinimaRecta.Value && iPtoSalida.pendientePC == null)
            {

                double miLongitudSalidaReal = iPtoSalida.longitud.Value + iRoadDesign.AijMinSalidaLlegada;

                oTramoEjeBasico miTramoIni = new oTramoEjeBasico(iIdAbanico, iIdTramo, iPtoSalida, miLongitudSalidaReal, iPtoSalida.azimutGrados.Value, eTramoTipoEjeBasico.avanceCorto);

                //Realizo las Validaciones del Tramo
                miTramoIni.validarTramoP2InsideTerreno();
                miTramoIni.validarTramoZonasNoPaso();
                miTramoIni.createSeccion(iLonDiscretizacionTramoProyecto, iRoadPendientes, iEstudioData, false);

                if (!miTramoIni.isTramoValido)
                {
                    if (miTramoNombre == "Tramo Salida")
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoSalidaNoCumple);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoLlegadaNoCumple);
                    }
                    miTramoIni.isTramoValido = true;
                }
              
                
                  return miTramoIni;
                

            }
            //CASO 7
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud != null && !iPtoSalida.isLonMinimaRecta.Value && iPtoSalida.pendientePC != null)
            {

                double miPendiente = iPtoSalida.pendientePC.Value;
                if (isTramoInvertido) miPendiente = miPendiente * (-1);

                if (iRoadDesign.AijMinSalidaLlegada > iPtoSalida.longitud.Value)
                {
                    throw new Exception(strError.eLongSalidaMenorA);
                }

                oTramoEjeBasico miTramoIni = new oTramoEjeBasico(iIdAbanico, iIdTramo, iPtoSalida, iPtoSalida.longitud.Value, iPtoSalida.azimutGrados.Value, eTramoTipoEjeBasico.avanceCorto);

                //Realizo las Validaciones del Tramo
                miTramoIni.validarTramoP2InsideTerreno();
                miTramoIni.validarTramoZonasNoPaso();
                miTramoIni.createSeccionTramoInicialFinal(iLonDiscretizacionTramoProyecto, miPendiente, iEstudioData);

                if (!miTramoIni.isTramoValido)
                {
                    if (miTramoNombre == "Tramo Salida")
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoSalidaNoCumple);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoLlegadaNoCumple);
                    }
                    miTramoIni.isTramoValido = true;
                }
               
                    return miTramoIni;
               
            }

            //CASO 8
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud != null && iPtoSalida.isLonMinimaRecta.Value && iPtoSalida.pendientePC != null)
            {

                double miPendiente = iPtoSalida.pendientePC.Value;
                if (isTramoInvertido) miPendiente = miPendiente * (-1);

                double miLongitudSalidaReal = iPtoSalida.longitud.Value + iRoadDesign.AijMinSalidaLlegada;


                oTramoEjeBasico miTramoIni = new oTramoEjeBasico(iIdAbanico, iIdTramo, iPtoSalida, miLongitudSalidaReal, iPtoSalida.azimutGrados.Value, eTramoTipoEjeBasico.avanceCorto);

                //Realizo las Validaciones del Tramo
                miTramoIni.validarTramoP2InsideTerreno();
                miTramoIni.validarTramoZonasNoPaso();
                miTramoIni.createSeccionTramoInicialFinal(iLonDiscretizacionTramoProyecto, miPendiente, iEstudioData);

                if (!miTramoIni.isTramoValido)
                {
                    if (miTramoNombre == "Tramo Salida")
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoSalidaNoCumple);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoLlegadaNoCumple);
                    }
                    miTramoIni.isTramoValido = true;
                }
                
                    return miTramoIni;
               

            }
            //CASO 9
            else if (iPtoSalida.azimutGrados == null && iPtoSalida.longitud != null)
            {
                throw new Exception(strError.eCasoIncomTramoSal);
            }
            else
            {
                throw new Exception(strError.eCasoIndeterTramoSal);
            }

        }


        public static oTramoEjeBasico createTramoSalidaLlegadaTijera(bool isTramoSalida, int iIdAbanico, int iIdTramo, oP3dSalidaLlegada iPtoSalida, oRoadDes iRoadDesign, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, double iLonDiscretizacionTramoProyecto, bool isTramoInvertido)
        {


            string miTramoNombre;

            if (isTramoSalida)
            {
                miTramoNombre = "Tramo Salida";
            }
            else
            {
                miTramoNombre = "Tramo Llegada";
            }



            //CASO 1
            if (iPtoSalida.isRotondaSinPendiente)
            {
                throw new oExTramoOrigenTramoDestinoNoConfig();
            }
            //CASO 2
            else if (iPtoSalida.isRotondaConPendienteK)
            {
                throw new oExTramoOrigenTramoDestinoNoConfig();
            }
            //CASO 3 CASO ROTONDA SOLO AZIMUT
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud == null && iPtoSalida.pendientePC == null)
            {
                return null;
            }
            //CASO 4 DATOS AZIMUT & PENDIENTE
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud == null && iPtoSalida.pendientePC != null)
            {
                return null;
            }
            //CASO 5 AZIMUT Y LONGITUD
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud != null  && iPtoSalida.pendientePC == null)
            {
                
                return null;

            }
            //CASO 6 AZIMUT, LONGITUD Y PENDIENTE
            else if (iPtoSalida.azimutGrados != null && iPtoSalida.longitud != null &&  iPtoSalida.pendientePC != null)
            {

                double miPendiente = iPtoSalida.pendientePC.Value;
                if (isTramoInvertido) miPendiente = miPendiente * (-1);

                oTramoEjeBasico miTramoIni = new oTramoEjeBasico(iIdAbanico, iIdTramo, iPtoSalida, iPtoSalida.longitud.Value, iPtoSalida.azimutGrados.Value, eTramoTipoEjeBasico.avanceCorto);

                //Realizo las Validaciones del Tramo
                miTramoIni.validarTramoP2InsideTerreno();
                miTramoIni.validarTramoZonasNoPaso();
                miTramoIni.createSeccionTramoInicialFinal(iLonDiscretizacionTramoProyecto, miPendiente, iEstudioData);

                if (!miTramoIni.isTramoValido)
                {
                    if (miTramoNombre == "Tramo Salida")
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoSalidaNoCumple);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiTramoLlegadaNoCumple);
                    }
                    miTramoIni.isTramoValido = true;
                }

                return miTramoIni;

            }
            //CASO 7
            else if (iPtoSalida.azimutGrados == null && iPtoSalida.longitud != null)
            {
                throw new Exception(strError.eCasoIncomTramoSal);
            }
            else
            {
                throw new Exception(strError.eCasoIndeterTramoSal);
            }

        }


        /// <summary>
        /// CREAR TRAMO DE LLEGADA
        /// </summary>
        public  static oTramoEjeBasico createTramoLlegada(oP3dSalidaLlegada iPtoSalida, oRoadDes iRoadDesign, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, double iLonDiscretizacionTramoProyecto)
        {

            oTramoEjeBasico miTramoLlegadaInvertido = createTramoSalidaLlegada(false,int.MaxValue,int.MaxValue,iPtoSalida, iRoadDesign, iRoadPendientes, iEstudioData, iLonDiscretizacionTramoProyecto, true);

            //Cambio los Puntos

            //Salida Rotonda
            if (miTramoLlegadaInvertido == null)
            {
                return null;
            }
            else
            {
                oP3d miP1 = miTramoLlegadaInvertido.P2;
                oP3d miP2 = miTramoLlegadaInvertido.P1;

                double miPendiente = miP1.getPendienteConSignoPC(miP2);

                oTramoEjeBasico miTramoSalida = new oTramoEjeBasico(int.MaxValue,int.MaxValue, miP1, miP2,eTramoTipoEjeBasico.avanceCorto);

                miTramoSalida.createSeccionTramoInicialFinal(iLonDiscretizacionTramoProyecto,miPendiente, iEstudioData);

                return miTramoSalida;
            }

        }

        public static oTramoEjeBasico createTramoLlegadaTijera(oP3dSalidaLlegada iPtoSalida, oRoadDes iRoadDesign, oRoadPendientes iRoadPendientes, IEstudio iEstudioData, double iLonDiscretizacionTramoProyecto)
        {

            oTramoEjeBasico miTramoLlegada= createTramoSalidaLlegadaTijera(false, int.MaxValue, int.MaxValue, iPtoSalida, iRoadDesign, iRoadPendientes, iEstudioData, iLonDiscretizacionTramoProyecto, true);
            return miTramoLlegada;

        }

    }
}
