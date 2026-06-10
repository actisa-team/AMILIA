using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.zonaGis
{

    using Autodesk.AutoCAD.Colors;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Geometry;

    using engCadNet;
    using engCadNet.entidades;

    using tadLayLogica.datos;
    using tadLayLogica.datos.Gis;
    using tadLayData;
    using tadLayLan;
    using tadLayLan.Tdb;

    using tadLayLogica.logica.valoracion;

    using tadLayLogica.datos.BaseDatos;
using tadLayShare.puntos;

    public abstract class oZonaCruces : oZonaGis
    {

        private Polyline mLwEje = null;

        //bool lo he buscado??
        private bool isBuscado = false;

        
        public oZonaCruces(Guid iId)
        : base(iId)
       {
         
       }


       #region "Propiedades Abstractas"


        public abstract double anguloGradosCruceMaximo { get; }
        public abstract double galibo { get; }
        public abstract bool obligacionEstructura { get; }
        


        public abstract bool isTramoCompletoPermitido { get; }
        public abstract bool isAnguloCruceConfigurado { get; }


        public Polyline lwEje
        {
            get
            {
                return mLwEje;
            }

            set
            {
                mLwEje = value;

            }
        }

        public abstract string capaEje { get; }

       public override string clasificacion
        {
            get
            {
                return strFrmGisGeneral.uiClasificacionGeneral;
            }
        }
       public override string block
       {
           get
           {
               return "gisCruces";
           }
       }
       public override int blockAttNum
       {
           get
           {
               return 4;
           }
       }
       #endregion

       public bool isOnZona(IP2d iPto)
       {
           return isPtoInLwZona(new Point3d(iPto.X, iPto.Y, 0));
       }

       public bool intersecWith(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
       {
           bool intersecWith = false;

            Polyline miLwTramo = new Polyline();
            miLwTramo.AddVertexAt(0, new Point2d(miPEntradaTramo.X, miPEntradaTramo.Y), 0, 0, 0);
            miLwTramo.AddVertexAt(1, new Point2d(miPSalidaTramo.X, miPSalidaTramo.Y), 0, 0, 0);


            Point3dCollection miColPtoInter = new Point3dCollection();
            this.lwZona.IntersectWith(miLwTramo, Intersect.OnBothOperands, miColPtoInter, IntPtr.Zero, IntPtr.Zero);

            if (miColPtoInter.Count != 0)
            {
                intersecWith = true;
            }


           return intersecWith;
       }

       public bool isValidoTramoDentroZona(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
       {
           bool isValido = true;

           if (!this.isTramoCompletoPermitido)
           {

               Polyline miZona = this.lwZona;

                   bool isP1inZona = isPtoInLwZona(new Point3d(miPEntradaTramo.X, miPEntradaTramo.Y, 0));
                   bool isP2inZona = isPtoInLwZona(new Point3d(miPSalidaTramo.X, miPSalidaTramo.Y, 0));

                   if (isP1inZona && isP2inZona)
                   {
                       isValido = false;
                   }
               
           }

           return isValido;
       }

       public bool isCruceConTramoValido(IP2d miPEntradaTramo, IP2d miPSalidaTramo)
       {
           bool isValido = true;

           if (this.isAnguloCruceConfigurado)
           {

               if ((mLwEje == null) && (!isBuscado))
               {
                   //SELECIONAR POLILINEA

                   List<string> miListaCapa = new List<string>();
                   miListaCapa.Add(this.capaEje);
                   List<Polyline> miListaEje = oSs.getSsLwByLayerListAndXdata(miListaCapa, eXdataKey.zonaGisGuid.ToString());

                   

                   //filtrar por el valor del xData con el getXData, que sea igual al id
                   foreach (Polyline miCauce in miListaEje)
                   {
                       List<string> miXData = oXdata.getXData(miCauce, eXdataKey.zonaGisGuid.ToString(), string.Empty);
                       if(miXData[0].Equals(this.id.ToString()))
                       {
                           mLwEje = miCauce;
                       }
                   }


                   isBuscado = true;
               }
               

               if (mLwEje != null)
               {
                   double? miAnguloCruce = oLw.getAnguloIntersec(miPEntradaTramo, miPSalidaTramo, mLwEje);

                   if (miAnguloCruce != null)
                   {

                       //Comprobar que cumple el angulo maximo
                       double miIntervaloMin = 180 - this.anguloGradosCruceMaximo;
                       double miIntervaloMax = this.anguloGradosCruceMaximo;

                       if (miIntervaloMin < miIntervaloMax)
                       {
                           if ((miIntervaloMin < miAnguloCruce) && (miAnguloCruce < miIntervaloMax))
                           {
                               isValido = true;
                           }
                           else
                           {
                               isValido = false;
                           }
                       }
                       else
                       {
                           if ((miIntervaloMin > miAnguloCruce) && (miAnguloCruce > miIntervaloMax))
                           {
                               isValido = true;
                           }
                           else
                           {
                               isValido = false;
                           }
                       }

                   }
               }
           }

           return isValido;
       }

       public  void linkEje(string iMensaje)
       {

           using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
           {
               //Selecciono la Polilinea
               lwEje = engCadNet.oSs.seleccionUsuario<Polyline>(iMensaje, "La Entidad Debe de Ser una Polilinea.");

               if (lwEje != null)
               {

                   //Valido, la Polilinea debe de ser Abierta
                   if (lwEje.Closed || lwEje.StartPoint == lwEje.EndPoint)
                   {
                       oTadil.data.UserInfo.showInfo("El Eje debe ser una Polilinea Abierta");
                       return;
                   }


                   //Creo las Capas
                   if (!engCadNet.oLayer.HasLayer(capaEje))
                   {
                       engCadNet.oLayer.addLayer(capaEje, Color.FromColorIndex(ColorMethod.ByLayer, 0).ColorIndex, true);
                   }
                   else
                   {
                       engCadNet.oLayer.current(capaEje);
                   }


                   //Asigno la Capa a la Polilinea
                   using (oEntidad<Polyline> miPolilinea = new oEntidad<Polyline>(lwEje))
                   {
                       miPolilinea.open();
                       miPolilinea.entidad.Color = oColor.getInstance.morado;
                       miPolilinea.entidad.Layer = capaEje;
                       miPolilinea.save();
                   }


                   //GUARDO LA INFORMACION EN LA POLILINEA

                   //GIS CODE        
                   engCadNet.oXdata.setXdata(lwEje.ObjectId, eXdataKey.zonaGisCode.ToString(), code.ToString());

                   //GUID
                   engCadNet.oXdata.setXdata(lwEje.ObjectId, eXdataKey.zonaGisGuid.ToString(), id.ToString());
               }
               else
               {
                   oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
               }
           }

       }

    }



    /// <summary>
    /// ZONAS DOMINIO PUBLICO HIDRAULICO
    /// </summary>
    public class oZonaDominioHidraulico : oZonaCruces
    {

        dsBd.tbDoHiRow mRow;


        public oZonaDominioHidraulico(Guid iId)
            : base(iId)
        {
            mRow = oSingletonDsBd.getInstance.getZonaDominioPublicoHidraulico(iId);
        }

        public oZonaDominioHidraulico(dsBd.tbDoHiRow iRow)
            : base(iRow.id)
        {
            mRow = iRow;
        }






        #region "Propiedades Abstractas"

        public override double galibo
        {
            get { return mRow.galibo; }
        }
        
        public double alturaMinRasante
        {
            get { return mRow.alturaMinRasante; }
        }

        public override bool obligacionEstructura
        {
            get { return mRow.pasarEstructura; }
        }


        public override double anguloGradosCruceMaximo
        {
            get { return mRow.anguloCruceMax; }
        }


        public override bool isTramoCompletoPermitido
        {
            get { return mRow.isTramoCompleto; }
        }


        public override bool isAnguloCruceConfigurado
        {
            get { return mRow.isAngCruceMaxEnabled; }
        }

        public override eGisGrupos grupo
        {
            get
            {
                return eGisGrupos.AMB;   
            }
        }
        public override eGisZonas code
        {
            get
            {
                return eGisZonas.ZODOPU; 
            }
        }
        public override string nombre
        {
            get
            {
                return mRow.nombre;
            }
        }
        public override bool isZonaNoPaso
        {
            get { return mRow.prohibirPaso; }
        }
        public override Color color
        {
          
           get
            {
                if (mRow.prohibirPaso)
                {
                    return dicValoracionColorCad[10];
                }
                else
                {
                    return dicValoracionColorCad[mRow.valoracion];
                }   
            }
            
        }
        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiZODOPU;
            }
        }

        public override string capaEje
        {
            get
            {
                return capaApp + "_EJE_" + strFrmGisGeneral.uiZODOPU;
            }
        }


        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI32;
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {

            get
            {

                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();

                miDicAtt.Add(1, mRow.nombre);
                miDicAtt.Add(2, string.Format(strFrmGisGeneral.attProhibirPaso, oTraductor.traducirSiNo(mRow.prohibirPaso)));
                if (mRow.prohibirPaso)
                {
                    miDicAtt.Add(3, string.Empty);
                    miDicAtt.Add(4, string.Empty);
                }
                else
                {
                    miDicAtt.Add(3, string.Format(strFrmGisGeneral.attPermitirEstructuras, oTraductor.traducirSiNo(mRow.pasarEstructura)));
                    miDicAtt.Add(4, string.Format(strFrmGisGeneral.attValoracion, mRow.valoracion.ToString()));
                }


                return miDicAtt;
            }


        }
        #endregion


        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            return new oComponentZonaItem(mRow.nombre, mRow.valoracion, iNumeroSeccionPerteneceZona, iNumeroSeccionesTotales);
        }

        


      


    }

    /// <summary>
    /// ZONAS CRUCES INFRAESTRUCTURAS
    /// </summary>
    public class oZonaCruInf : oZonaCruces
    {

        dsBd.tbCruceInfraRow mRow;


        public oZonaCruInf(Guid iId)
            : base(iId)
        {
            mRow = oDalTbCruceInfra.getRowById(iId);
        }

        public oZonaCruInf(dsBd.tbCruceInfraRow iZonaRow)
            : base(iZonaRow.id)
        {
            mRow = iZonaRow;
        }


        #region "Propiedades Abstractas"


        public override double galibo
        {
            get { return mRow.galibo; }
        }


        public override bool obligacionEstructura
        {
            get {
                if (mRow.prohibirPaso)
                {
                    return false;
                }
                else
                {
                    return mRow.pasoNivelExigir;
                }
            }
        }


        public override bool isTramoCompletoPermitido
        {
            get { return true; }
        }


        public override bool isAnguloCruceConfigurado
        {
            get { return mRow.isAngCruceMaxEnabled; }
        }

        public override double anguloGradosCruceMaximo
        {
            get { return mRow.anguloCruceMax; }
        }


        public override eGisGrupos grupo
        {
            get
            {
                return eGisGrupos.PAT;
            }
        }
        public override eGisZonas code
        {
            get
            {
                return  eGisZonas.CRUINF;
            }
        }
        public override string nombre
        {
            get
            {
                return mRow.nombre;
            }
        }
        public override bool isZonaNoPaso
        {
            get { return mRow.prohibirPaso; }
        }
        public override Color color
        {
            get
            {
                if (mRow.prohibirPaso)
                {
                    return dicValoracionColorCad[10];
                }
                else
                {
                    return dicValoracionColorCad[mRow.valoracion];
                } 
            }
        }
        public override string capaUser
        {
            get
            {
                return capaApp + "_" + strFrmGisGeneral.uiCRUINF;
            }
        }

        public override string capaEje
        {
            get
            {
                return capaApp + "_EJE_" + strFrmGisGeneral.uiCRUINF;
            }
        }

        public override eHatch hatch
        {
            get
            {
                return eHatch.ANSI36;
            }
        }
        public override Dictionary<int, string> listadoAtributos
        {

            get
            {

                Dictionary<int, string> miDicAtt = new Dictionary<int, string>();

                miDicAtt.Add(1, mRow.nombre);
                miDicAtt.Add(2, string.Format(strFrmGisGeneral.attProhibirPaso, oTraductor.traducirSiNo(mRow.prohibirPaso)));

                if (mRow.prohibirPaso)
                {
                    miDicAtt.Add(3, string.Empty);
                    miDicAtt.Add(4, string.Empty);
                }
                else
                {
                    miDicAtt.Add(3, string.Format(strFrmGisGeneral.attPasoDesnivelExigir, oTraductor.traducirSiNo(mRow.pasoNivelExigir)));
                    miDicAtt.Add(4, string.Format(strFrmGisGeneral.attValoracion, mRow.valoracion.ToString()));
                }

                return miDicAtt;
            }


        }
        #endregion




        public override IValoracion getValoracion(int iNumeroSeccionPerteneceZona, int iNumeroSeccionesTotales)
        {
            return new oComponentZonaItem(mRow.nombre, mRow.valoracion, iNumeroSeccionPerteneceZona, iNumeroSeccionesTotales);
        }


    }


}
