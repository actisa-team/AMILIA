using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{

    //System
    using System.ComponentModel;


    using engNet.CustomAtributos;

    using tadLayLogica.datos.proyecto;

    ////Civil3d
    //using cv = Autodesk.Civil.DatabaseServices;


    using engNet.ClassT;
    using engNet.Extension.String;
    using Autodesk.AutoCAD.DatabaseServices;
    using EjeDeTrazado.componentes;
    using tadLayLan.Tdi;
    using System.IO;

    public class oValoracionTrazadoPlanta : IValoracionPropiedad
    {

        private double? mTrazadoLon = null;
        private List<oEntidadesTrazado> mLstEntidades;



        private double? mTrazadoCurvaturaMayor2500PU = null;
        private double? mTrazadoCurvaturaMenor2500PU = null;
        private double? mRadioMedioTrazadoCurvaturaMenor2500 = null;
        private double? mNotaLocal = null;
        private double? mNotaLocalValoracionPC = null;



        #region "Constructores"

        //       public oValoracionTrazadoPlanta(cv.Alignment iTrazadoPlanta)
        public oValoracionTrazadoPlanta(Polyline iTrazadoPlanta)
        {
            mTrazadoLon = iTrazadoPlanta.Length;
            mLstEntidades = descomponerTrazado(iTrazadoPlanta);
        }



        public oValoracionTrazadoPlanta(Polyline iTrazadoPlanta, double iValoracionTrazadoPlanta, oSolucion oSolucion = null)
        {
            mTrazadoLon = iTrazadoPlanta.Length;
            mNotaLocalValoracionPC = iValoracionTrazadoPlanta;
            if (oSolucion == null)
            {
                mLstEntidades = descomponerTrazado(iTrazadoPlanta);
            }
            else
            {
                mLstEntidades = descomponerTrazado(iTrazadoPlanta,oSolucion);
            }
        }

        #endregion






        #region "Propiedades"

        public double trazadoCurvaturaMayor2500PU
        {

            get
            {

                if (mTrazadoCurvaturaMayor2500PU == null)
                {

                    var miQuery = from p in mLstEntidades
                                  where p.radio >= 2500
                                  select p;


                    double miLon2500Mayor = miQuery.ToList().Sum(p => p.lon.Value);

                    mTrazadoCurvaturaMayor2500PU = miLon2500Mayor / mTrazadoLon;
                }

                return mTrazadoCurvaturaMayor2500PU.Value;
            }


        }
        public double trazadoCurvaturaMenor2500PU
        {
            get
            {

                if (mTrazadoCurvaturaMenor2500PU == null)
                {

                    var miQuery = from p in mLstEntidades
                                  where p.radio < 2500
                                  select p;


                    double miLon2500Menor = miQuery.ToList().Sum(p => p.lon.Value);

                    mTrazadoCurvaturaMenor2500PU = miLon2500Menor / mTrazadoLon;
                }

                return mTrazadoCurvaturaMenor2500PU.Value;
            }


        }
        public double radioMedioTrazadoCurvaturaMenor2500
        {
            get
            {
                if (mRadioMedioTrazadoCurvaturaMenor2500 == null || mRadioMedioTrazadoCurvaturaMenor2500 == double.NaN)
                {

                    var miQuery = from p in mLstEntidades
                                  where p.radio < 2500
                                  select p;


                    double miSumRadioxLon = miQuery.ToList().Sum(p => p.radioxLon);
                    double miSumLon = miQuery.ToList().Sum(p => p.lon.Value);

                    mRadioMedioTrazadoCurvaturaMenor2500 = miSumRadioxLon / miSumLon;

                    if (miSumLon == 0) mRadioMedioTrazadoCurvaturaMenor2500 = 0;
                }

                return mRadioMedioTrazadoCurvaturaMenor2500.Value;
            }

        }
        public double notaLocal
        {

            get
            {
                if (mNotaLocal == null)
                {
                    double miResta = 100 - (0.5 * trazadoCurvaturaMenor2500PU * 100);

                    mNotaLocal = (miResta * radioMedioTrazadoCurvaturaMenor2500) / 250000;
                }

                return mNotaLocal.Value;
            }

        }

        #endregion


        public void writeCSV(string iFilePathFull)
        {
            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oValDesT<string, double?>> miLstFooter = new List<oValDesT<string, double?>>();

            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInformeValTrazadoplanta, string.Empty));

            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiLongM, mTrazadoLon.ToString()));

            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiTrazCurvMayor2500, trazadoCurvaturaMayor2500PU * 100));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiTrazCurvMenor2500, trazadoCurvaturaMenor2500PU * 100));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uitrazRadioMedioMenor2500, radioMedioTrazadoCurvaturaMenor2500));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiValPropiaDe0a10, notaLocal));



            tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
                                                         oEntidadesTrazado,
                                                         oValDesT<string, double?>>(miLstHeader, mLstEntidades, miLstFooter, iFilePathFull);


        }

        public void writeCSV(string iPath, string iFileNameSinExtension)
        {

            string miFileFull = iPath.createFilePathFull(iFileNameSinExtension, "csv");

            this.writeCSV(miFileFull);
        }

        #region "Interface"

        public IValoracion valoracion
        {
            get
            {
                return new oComponentValTrazadoPlanta(notaLocal, mNotaLocalValoracionPC.Value);
            }
        }

        #endregion



        #region "MetodosPrivados"



        private List<oEntidadesTrazado> descomponerTrazado(Polyline iEjeTrazado,oSolucion oSolucion=null)
        {

            List<oEntidadesTrazado> miLstEntidades = new List<oEntidadesTrazado>();

            //A partir de la polilinea recuperar miEjeTadil
            //[ANGELES] : devolver una lista de entidades que contiene el eje
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

           




            oEntidadesTrazado miEntidad;
            Linea miLine = null;
            Curva miArco = null;
            Clotoide miClotoide = null;
            //cv.AlignmentSTS miSTS = null;
            //Clotoide miSpiral = null;



            string miEntidadTipo = string.Empty;



            foreach (Componente item in miEje.getComponentes)
            {

                miEntidadTipo = item.getTipoComponente().ToString();

                //Entidad Linea
                if (miEntidadTipo.Contains("recta") || miEntidadTipo.Contains("linea"))
                {
                    miLine = (Linea)item;

                    miEntidad = new oEntidadesTrazado();
                    miEntidad.addRecta(miLstEntidades.Count, miLine.getLongitud());
                    miLstEntidades.Add(miEntidad);

                    miLine = null;
                }
                //Entidad Arco
                else if (miEntidadTipo.Contains("curva"))
                {
                    miArco = (Curva)item;
                    miEntidad = new oEntidadesTrazado();
                    miEntidad.addCurva(miLstEntidades.Count, miArco.getLongitud(), miArco.getRadio);
                    miLstEntidades.Add(miEntidad);

                    miArco = null;
                }

                else if (miEntidadTipo.Contains("clotoideEntrada"))
                {
                    miClotoide = (Clotoide)item;

                    if (miClotoide.getLongitud() != 0)
                    {
                        //Spiral Entrada
                        miLstEntidades.AddRange(espiralDescomponer(miLstEntidades.Count, true, miClotoide));
                        miClotoide = null;
                    }


                }
                else if (miEntidadTipo.Contains("clotoideSalida"))
                {
                    miClotoide = (Clotoide)item;

                    if (miClotoide.getLongitud() != 0)
                    {
                        //Spiral Entrada
                        miLstEntidades.AddRange(espiralDescomponer(miLstEntidades.Count, false, miClotoide));
                        miClotoide = null;
                    }

                }

                else
                {
                    throw new Exception(string.Format("La SubEntidad del Trazado {0},\n No esta Configurada", item.GetType().ToString()));
                }
            }

            return miLstEntidades;


        }
        private List<oEntidadesTrazado> espiralDescomponer(int iIdNew, Clotoide iEspiral)
        {

            List<oEntidadesTrazado> miLstSpiral = new List<oEntidadesTrazado>();


            oEntidadesTrazado miEntidad1;
            oEntidadesTrazado miEntidad2;
            double? miSpiralLon2500Mayor = null;
            double? miSpiralLon2500Menor = null;
            double? miSpiralLon2500MenorRadioMedio = null;

            //Longitud Radio Mayor 2500
            miSpiralLon2500Mayor = Math.Pow(iEspiral.getValorA(), 2) / 2500;
            //Longitud Spiral Radio Menor 2500;
            miSpiralLon2500Menor = iEspiral.getLongitud() - miSpiralLon2500Mayor.Value;
            //Radio Medio
            miSpiralLon2500MenorRadioMedio = espiralRadioMedio(iEspiral.getValorA(), iEspiral.getLongitud(), miSpiralLon2500Mayor.Value);

            //Parte Recta
            miEntidad1 = new oEntidadesTrazado();
            miEntidad1.addEspiralMayor2500(iIdNew, miSpiralLon2500Mayor.Value, iEspiral.getValorA());
            miLstSpiral.Add(miEntidad1);

            //Parte Curva
            miEntidad2 = new oEntidadesTrazado();
            miEntidad2.addEspiralMenor2500(iIdNew + 1, miSpiralLon2500Menor.Value, miSpiralLon2500MenorRadioMedio.Value, iEspiral.getValorA());
            miLstSpiral.Add(miEntidad2);


            return miLstSpiral;


        }
        private List<oEntidadesTrazado> espiralDescomponer(int iIdNew, bool iIsEspiralEntrada, Clotoide iEspiral)
        {

            List<oEntidadesTrazado> miLstSpiral = new List<oEntidadesTrazado>();
            oEntidadesTrazado miEntidad1;
            oEntidadesTrazado miEntidad2;
            double? miSpiralLon2500Mayor = null;
            double? miSpiralLon2500Menor = null;
            double? miSpiralLon2500MenorRadioMedio = null;


            //Longitud Radio Mayor 2500
            miSpiralLon2500Mayor = Math.Pow(iEspiral.getValorA(), 2) / 2500;
            //Longitud Spiral Radio Menor 2500;
            miSpiralLon2500Menor = iEspiral.getLongitud() - miSpiralLon2500Mayor.Value;
            //Radio Medio
            miSpiralLon2500MenorRadioMedio = espiralRadioMedio(iEspiral.getValorA(), iEspiral.getLongitud(), miSpiralLon2500Mayor.Value);


            if (iIsEspiralEntrada)
            {
                //Parte Recta
                miEntidad1 = new oEntidadesTrazado();
                miEntidad1.addEspiralMayor2500(iIdNew, miSpiralLon2500Mayor.Value, iEspiral.getValorA());
                miLstSpiral.Add(miEntidad1);

                //Parte Curva
                miEntidad2 = new oEntidadesTrazado();
                miEntidad2.addEspiralMenor2500(iIdNew + 1, miSpiralLon2500Menor.Value, miSpiralLon2500MenorRadioMedio.Value, iEspiral.getValorA());
                miLstSpiral.Add(miEntidad2);

            }
            else
            {

                //Parte Curva
                miEntidad2 = new oEntidadesTrazado();
                miEntidad2.addEspiralMenor2500(iIdNew, miSpiralLon2500Menor.Value, miSpiralLon2500MenorRadioMedio.Value, iEspiral.getValorA());
                miLstSpiral.Add(miEntidad2);

                //Parte Recta
                miEntidad1 = new oEntidadesTrazado();
                miEntidad1.addEspiralMayor2500(iIdNew + 1, miSpiralLon2500Mayor.Value, iEspiral.getValorA());
                miLstSpiral.Add(miEntidad1);

            }


            return miLstSpiral;
        }
        private static double espiralRadioMedio(double iAspiral, double iSpiralLon, double iSpiralLon2500mas)
        {

            double miLne = Math.Log(iSpiralLon);
            double miLn2500 = Math.Log(iSpiralLon2500mas);
            double miResta = miLne - miLn2500;
            double miA2 = Math.Pow(iAspiral, 2);

            return (miA2 * miResta) / (iSpiralLon - iSpiralLon2500mas);

        }
        #endregion


    }
    internal class oEntidadesTrazado
    {


        [LocalizedDisplayName("uiIndice", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 1)]
        [DefaultValue(null)]
        public int? id { get; private set; }

        [LocalizedDisplayName("uiEntidad", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 2)]
        public string mEntidad { get; private set; }


        [LocalizedDisplayName("uiLongM", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 3)]
        [DefaultValue(null)]
        public double? lon { get; private set; }


        [LocalizedDisplayName("uiRadioM", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 4)]
        [DefaultValue(null)]
        public double? radio { get; private set; }

        [LocalizedDisplayName("uiRadioXLong", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 5)]
        public double radioxLon { get { return radio.Value * lon.Value; } }

        [LocalizedDisplayName("uiEspiralA", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 6)]
        [DefaultValue(null)]
        public double? Aespiral { get; private set; }



        public oEntidadesTrazado()
        {

        }


        public void addRecta(int iId, double iLon)
        {
            mEntidad = strFrmInformes.uiRecta;
            id = iId;
            lon = iLon;
            radio = 2500;
        }

        public void addCurva(int iId, double iLon, double iRadio)
        {
            mEntidad = strFrmInformes.uiCurva;
            id = iId;
            lon = iLon;
            radio = iRadio;
        }

        public void addEspiralMenor2500(int iId, double iLon, double iRadio, double iAspiral)
        {

            mEntidad = strFrmInformes.uiEspiral;
            id = iId;
            lon = iLon;
            radio = iRadio;
            Aespiral = iAspiral;
        }

        public void addEspiralMayor2500(int iId, double iLon, double iAspiral)
        {
            mEntidad = strFrmInformes.uiEspiral;
            id = iId;
            lon = iLon;
            radio = 2500;
            Aespiral = iAspiral;
        }



    }
}
