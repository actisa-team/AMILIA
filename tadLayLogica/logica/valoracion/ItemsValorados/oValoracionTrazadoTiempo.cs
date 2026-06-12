using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{
    

    using System.ComponentModel;

    using engNet.CustomAtributos;
    using engNet.ClassT;
    using tadLayLogica.datos;
    using tadLayData;
    using engNet.Extension.Double;
    using Autodesk.AutoCAD.DatabaseServices;
using EjeDeTrazado.componentes;
    using tadLayShare;
    using tadLayLan.Tdi;
    using System.IO;

    public class oValoracionTrazadoTiempo:IValoracionPropiedad
    {

       
       private eRoadGrupo? mRoadGrupo=null;

       private double mTrazadoLon;

       private List<dsInfra.tbCarreteraItemsRow> mLstRoadItemsByGrupo1;
       private List<dsInfra.tbCarreteraItemsRow> mLstRoadItemsByGrupo2;
       private List<oEntidadTrazadoTiempo> mLstEntidades;
       private double? mNotaLocal = null;
       private double? mNotaLocaValoracionPC = null;



       #region "Constructor"
       public oValoracionTrazadoTiempo(eRoadGrupo iRoadGrupo, Polyline iTrazadoPlanta, string iFileNormas, double iValoracionTiempoPC,oSolucion oSolucion=null)
        {
            mRoadGrupo = iRoadGrupo;

            mTrazadoLon = iTrazadoPlanta.Length;

            mNotaLocaValoracionPC = iValoracionTiempoPC;

            using (oDsRoad miDsRoad = new oDsRoad(iFileNormas))
            {
                mLstRoadItemsByGrupo1 = miDsRoad.getRowsByGrupo(eRoadGrupo.Grupo1);
                mLstRoadItemsByGrupo2 = miDsRoad.getRowsByGrupo(eRoadGrupo.Grupo2);
            }
            if (oSolucion == null)
            {
                mLstEntidades = descomponerTrazado(iTrazadoPlanta);
            }
            else
            {
                mLstEntidades = descomponerTrazado(iTrazadoPlanta, oSolucion);
            }

            
        }
       #endregion

       #region "Propiedades"
        public double velocidadMinima
       {
            get
           {
            return   mLstEntidades.ToList().Min(row => row.velocidad.Value);
           }


       }
        public double velocidadMaxima
       {
            get
           {
               if (mRoadGrupo.Value == eRoadGrupo.Grupo1)
               {
                   return 120;
               }
               else if (mRoadGrupo.Value == eRoadGrupo.Grupo2)
               {

                   return 100;
               }
               else
               {
                   throw new oExEnumNotImplemented(mRoadGrupo.Value.ToString());
               }

           }


       }
        public double notaLocal
        {
            get
            {
                if (mNotaLocal == null)
                {
                  mNotaLocal = mLstEntidades.Sum(row => row.tiempo);
                }

                return mNotaLocal.Value;
            }

        }
       #endregion



        #region "ImplementoInterfaz"

        public IValoracion valoracion
        {
            get
            {
                return new oComponentValTrazadoTiempo(notaLocal, mNotaLocaValoracionPC.Value);
            }
        }


        #endregion




        public void writeCSV(string iFilePathFull)
        {


            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oValDesT<string, double?>> miLstFooter = new List<oValDesT<string, double?>>();

            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInformeValTrazadoTiempo, string.Empty));

            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiGrupo, mRoadGrupo.Value.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVelMaximaKmH, velocidadMaxima.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiVelMinimaKmH, velocidadMinima.ToString()));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiLongM, mTrazadoLon.ToString()));

            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiValPropiaMin, notaLocal));
            

            
            List<oEntidadTrazadoTiempoToWrite> miLstEntidadesToWrite = new List<oEntidadTrazadoTiempoToWrite>();
            foreach (oEntidadTrazadoTiempo miEntidad in mLstEntidades)
            {
                miLstEntidadesToWrite.Add(new oEntidadTrazadoTiempoToWrite(miEntidad.id, miEntidad.pk, miEntidad.mEntidad, miEntidad.lon, miEntidad.radio, miEntidad.velocidad, miEntidad.tiempo));
            }

            tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
                                                         oEntidadTrazadoTiempoToWrite,
                                                         oValDesT<string, double?>>(miLstHeader, miLstEntidadesToWrite, miLstFooter, iFilePathFull);

        }



        #region "MetodosPrivados"

        private bool check ()
        {

            throw new NotImplementedException();

            //Compruebo si la Longitud del Trazado es igual a la suma del listado de entidades




            //Compruebo si los PC de tramos son 100



            //Compruebo si valorPropio es menor 1






        }

        private List<oEntidadTrazadoTiempo> descomponerTrazado(Polyline iEjeTrazado,oSolucion oSolucion=null)
        {

            List<oEntidadTrazadoTiempo> miLstEntidades = new List<oEntidadTrazadoTiempo>();
            EjeDeTrazado.puntosDelEje.EjeTrazado miEje = null;
            if (oSolucion == null)
            {
                Xrecord miXrecord = engCadNet.oXrecord.getXrecord(iEjeTrazado.ObjectId, "info");
                miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));
            }
            else
            {

                if (!oSolucion.solucionData.amilia)
                {
                    Xrecord miXrecord = engCadNet.oXrecord.getXrecord(iEjeTrazado.ObjectId, "info");
                    miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));
                }
                else
                {
                    byte[] datosRecuperados = oSolucion.solucionData.EjeTrazado_Amilia;
                    using (MemoryStream ms = new MemoryStream(datosRecuperados))
                    {
                        // 3. Usamos tu método estático para reconstruir la clase
                        miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(ms);
                    }
                }
            }

            /*Xrecord miXrecord = engCadNet.oXrecord.getXrecord(iEjeTrazado.ObjectId, "info");
            EjeDeTrazado.puntosDelEje.EjeTrazado miEje = EjeDeTrazado.puntosDelEje.EjeTrazado.recuperaEjeTrazado(engCadNet.oXrecord.getStream(miXrecord));*/
   

            string miEntidadTipo = string.Empty;
  
            #region "Crear las Entidades"




            foreach (Componente item in miEje.getComponentes)
            {

  

                    switch (item.getTipoComponente())
                    {

                        case Componente.tipoComponente.linea:

                            Linea miLinea = (Linea) item;

                            miLstEntidades.Add(new oEntidadTrazadoTiempo(miLinea.getPkIni, Componente.tipoComponente.linea, miLinea.getLongitud(), null));

                            break;

                        case Componente.tipoComponente.clotoideEntrada:

                            Clotoide miClotoideE = (Clotoide) item;

                            miLstEntidades.Add(new oEntidadTrazadoTiempo(miClotoideE.getPkIni, Componente.tipoComponente.clotoideEntrada, miClotoideE.getLongitud(), null));

                            break;

                        case Componente.tipoComponente.clotoideSalida:

                            Clotoide miClotoideS = (Clotoide)item;

                            miLstEntidades.Add(new oEntidadTrazadoTiempo(miClotoideS.getPkIni, Componente.tipoComponente.clotoideSalida, miClotoideS.getLongitud(), null));

                            break;


                        case Componente.tipoComponente.curva:

                            Curva miSubArco = (Curva)item;
                            
                            miLstEntidades.Add(new oEntidadTrazadoTiempo(miSubArco.getPkIni, Componente.tipoComponente.curva, miSubArco.getLongitud(), miSubArco.getRadio));

                            break;

                        default:

                            throw new oExEnumNotImplemented(item.getTipoComponente().ToString());
                    }

                   
                

              
                }

            //Ordeno El Listado por Pk
            miLstEntidades = (from p in miLstEntidades orderby p.pk ascending select p).ToList();

            //Añado el Id
            int miId = 1;

            foreach (var item in miLstEntidades)
            {
                item.id = miId;

                miId++;
            }


            #endregion
            #region "Asignar Velocidad a las Entidades"


            //-1 Asigno las Velocidades a las Entidades Conocidas


            foreach (var item in miLstEntidades)
            {

                switch (item.mEntidad)
                {
                    case Componente.tipoComponente.linea:

                        item.velocidad = velocidadMaxima;

                        break;
                    case Componente.tipoComponente.curva:

                        item.velocidad = getVelocidadFromCurvas(item.radio.Value.roundOffOne());

                        break;

                    case Componente.tipoComponente.clotoideEntrada:

                        break;
                    case Componente.tipoComponente.clotoideSalida:

                        break;

                    default:

                        throw new oExEnumNotImplemented(item.ToString());
                }

            }


            //2-Asigno las Velocidades a las Espirales
            if (miLstEntidades.Exists(p => (p.mEntidad == Componente.tipoComponente.clotoideEntrada) || (p.mEntidad == Componente.tipoComponente.clotoideSalida)))
            {
                 setUpVelocidadFromEspiral(ref miLstEntidades);
            }

            #endregion
            #region "Check"

            if (miLstEntidades.Exists(row => row.velocidad == null))
            {
               throw new Exception("La Configuración de Velocidades por Entidad No es Correcta");
            }

            #endregion

            return miLstEntidades;
        }

        private double getVelocidadFromCurvas (double iCurvaRadio)
        {
            double miVelocidadMinima = 40;

            var miQuery = from p in mLstRoadItemsByGrupo1
                          where p.radio <= iCurvaRadio
                          orderby p.radio descending
                          select p;

            if(mRoadGrupo==eRoadGrupo.Grupo2)
            {
                 miQuery = from p in mLstRoadItemsByGrupo2
                          where p.radio <= iCurvaRadio
                          orderby p.radio descending
                          select p;
            }
        


            if (miQuery == null || miQuery.Count() == 0)
            {

                miQuery = from p in mLstRoadItemsByGrupo2
                          where p.radio <= iCurvaRadio
                          orderby p.radio descending
                          select p;
                                   
            }


            if (miQuery == null || miQuery.Count() == 0)
            {
                return miVelocidadMinima;
            }
            else
            {

                double miVelocidadCurva = miQuery.First().velocidad;

                if (miVelocidadCurva > velocidadMaxima)
                {
                    return velocidadMaxima;
                }
                else
                {
                    return miVelocidadCurva;
                }
            }

        }

        private void setUpVelocidadFromEspiral (ref List<oEntidadTrazadoTiempo> iLstEntidades)
        {

            var miQuery = from p in iLstEntidades
                          where p.mEntidad == Componente.tipoComponente.curva
                          orderby p.id ascending
                          select p;

            int? miPosBaseCero = null;
            int? miPrevio = null;
            int? miNext = null;

            foreach (var item in miQuery)
            {

                miPosBaseCero = item.id-1;
                miPrevio = miPosBaseCero.Value - 1;
                miNext = miPosBaseCero.Value + 1;


                if ((iLstEntidades[miPrevio.Value].mEntidad == Componente.tipoComponente.clotoideEntrada)||(iLstEntidades[miPrevio.Value].mEntidad == Componente.tipoComponente.clotoideSalida))
                {
                    iLstEntidades[miPrevio.Value].velocidad = (velocidadMaxima + item.velocidad) / 2;
                }

                if ((iLstEntidades[miNext.Value].mEntidad == Componente.tipoComponente.clotoideEntrada)||(iLstEntidades[miNext.Value].mEntidad == Componente.tipoComponente.clotoideSalida))
                {
                    iLstEntidades[miNext.Value].velocidad = (velocidadMaxima + item.velocidad) / 2;
                }

            }

        }

        #endregion




    }
    internal class oEntidadTrazadoTiempo
    {


        [BindingInfo(SortIndex = 1)]
        [LocalizedDisplayName("uiIndice", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public int? id { get; set; }

        [LocalizedDisplayName("uiEntidad", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 2)]
        public Componente.tipoComponente mEntidad { get; private set; }

        [BindingInfo(SortIndex = 3)]
        [LocalizedDisplayName("uiPk", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? pk { get; private set; }

        [BindingInfo(SortIndex = 4)]
        [LocalizedDisplayName("uiLongM", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? lon { get; private set; }

        [BindingInfo(SortIndex = 5)]
        [LocalizedDisplayName("uiRadioM", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? radio { get; private set; }

        [BindingInfo(SortIndex = 6)]
        [LocalizedDisplayName("uiVelocidad", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? velocidad { get; set; }

        [BindingInfo(SortIndex = 6)]
        [LocalizedDisplayName("uiTiempoMin", typeof(strFrmInformes))]
        public double tiempo
        {
            get
            {
                return 60 * (lon.Value / 1000) / velocidad.Value;
            }
        }


        #region "Constructor"

        public oEntidadTrazadoTiempo(double iPk, Componente.tipoComponente iEntidad, double iLon, double? iRadio)
        {
            pk = iPk;
            mEntidad = iEntidad;
            lon = iLon;
            radio = iRadio;
            velocidad = null;
        }

        #endregion

    }

    internal class oEntidadTrazadoTiempoToWrite
    {


        [BindingInfo(SortIndex = 1)]
        [LocalizedDisplayName("uiIndice", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public int? id { get; set; }

        [BindingInfo(SortIndex = 2)]
        [LocalizedDisplayName("uiEntidad", typeof(strFrmInformes))]
        public string mEntidad { get; private set; }

        [BindingInfo(SortIndex = 3)]
        [LocalizedDisplayName("uiPk", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? pk { get; private set; }

        [BindingInfo(SortIndex = 4)]
        [LocalizedDisplayName("uiLongM", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? lon { get; private set; }

        [BindingInfo(SortIndex = 5)]
        [LocalizedDisplayName("uiRadioM", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? radio { get; private set; }

        [BindingInfo(SortIndex = 6)]
        [LocalizedDisplayName("uiVelocidad", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double? velocidad { get; set; }

        [BindingInfo(SortIndex = 7)]
        [LocalizedDisplayName("uiTiempoMin", typeof(strFrmInformes))]
        [DefaultValue(null)]
        public double tiempo { get; set; }

        #region "Constructor"

        public oEntidadTrazadoTiempoToWrite(int? iId, double? iPk, Componente.tipoComponente iEntidad, double? iLon, double? iRadio, double? iVelocidad, double iTiempo)
        {
            id = iId;
            pk = iPk;

            if (iEntidad.ToString() == "linea")
            {
                mEntidad = strFrmInformes.uiRecta;
            }
            else if (iEntidad.ToString() == "curva")
            {
                mEntidad = strFrmInformes.uiCurva;
            }
            else if (iEntidad.ToString() == "clotoideEntrada")
            {
                mEntidad = strFrmInformes.uiClotoideEntrada;
            }
            else if (iEntidad.ToString() == "clotoideSalida")
            {
                mEntidad = strFrmInformes.uiClotiodeSalida;
            }
            lon = iLon;
            radio = iRadio;
            velocidad = iVelocidad;
            tiempo = iTiempo;
        }

        #endregion

    }
}
