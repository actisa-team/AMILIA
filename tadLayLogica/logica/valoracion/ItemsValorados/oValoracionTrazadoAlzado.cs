using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica.logica.valoracion
{

    //System
    using System.ComponentModel;

    //ENG
    using engNet.CustomAtributos;
    using engNet.Extension.String;
    using engNet.ClassT;

    using Autodesk.AutoCAD.DatabaseServices;
    using PerfilLongitudinal.componentes;
    using tadLayLan.Tdi;
    using tadLayData;
    using tadLayLogica.datos.proyecto;
    using System.IO;

    public class oValoracionTrazadoAlzado : IValoracionPropiedad
    {

        private double? mTrazadoLonPlanta = null;
        private List<oEntidadesAlzados> mLstEntidades;
        private double? mNotaLocal = null;
        private double? mNotaLocalValoracionPC = null;


        #region "Constructor"

        public oValoracionTrazadoAlzado(Polyline iTrazadoPerfil)
        {

            mTrazadoLonPlanta = iTrazadoPerfil.Length;
            mLstEntidades = descomponerTrazado(iTrazadoPerfil);
        }

        public oValoracionTrazadoAlzado(Polyline iTrazadoPerfil, double iValoracionTrazadoAlzadoPC, oSolucion oSolucion = null)
        {
            mTrazadoLonPlanta = iTrazadoPerfil.Length;
            if (oSolucion == null)
            {
                mLstEntidades = descomponerTrazado(iTrazadoPerfil);
            }
            else
            {
                mLstEntidades = descomponerTrazado(iTrazadoPerfil, oSolucion);
            }

            mNotaLocalValoracionPC = iValoracionTrazadoAlzadoPC;
        }


        #endregion


        #region "Propiedades"


        public double notaLocal
        {

            get
            {
                if (mNotaLocal == null)
                {
                    double miSumLonPen = mLstEntidades.Sum(p => p.PenxLon);
                    double miSumLon = mLstEntidades.Sum(p => p.lon.Value);

                    mNotaLocal = miSumLonPen / miSumLon;
                }

                return mNotaLocal.Value;

            }

        }

        #endregion

        #region "Interface"

        public IValoracion valoracion
        {
            get
            {
                return new oComponentValTrazadoAlzado(this.notaLocal, this.mNotaLocalValoracionPC.Value);
            }
        }

        #endregion


        public void writeCSV(string iFilePathFull)
        {

            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oValDesT<string, double?>> miLstFooter = new List<oValDesT<string, double?>>();

            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiInformeValTrazadoAlzado, string.Empty));

            miLstHeader.Add(new oValDesT<string, string>(string.Empty, string.Empty));
            miLstHeader.Add(new oValDesT<string, string>(strFrmInformes.uiLongPlanta, mTrazadoLonPlanta.ToString()));

            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>("", null));
            miLstFooter.Add(new oValDesT<string, double?>(strFrmInformes.uiValPropiaNoUnidad, notaLocal));

            tadLayLogica.informes.oExcelInforme.WriteCsv<oValDesT<string, string>,
                                                         oEntidadesAlzados,
                                                         oValDesT<string, double?>>(miLstHeader, mLstEntidades, miLstFooter, iFilePathFull);

        }



        public void writeCSV(string iPath, string iFileNameSinExtension)
        {

            string miFileFull = iPath.createFilePathFull(iFileNameSinExtension, "csv");

            this.writeCSV(miFileFull);
        }



        #region "MetodosPrivados"

        private List<oEntidadesAlzados> descomponerTrazado(Polyline iTrazadoPerfil, oSolucion oSolucion = null)
        {

            PerfilLongitudinal.Alzado miAlzado = null;

            if (oSolucion == null)
            {
                Xrecord miXrecord = engCadNet.oXrecord.getXrecord(iTrazadoPerfil.ObjectId, "info");
                miAlzado = PerfilLongitudinal.Alzado.recuperaAlzado(engCadNet.oXrecord.getStream(miXrecord));
            }
            else
            {
                dsApp.tbSolucionRow miRow = oDalTbSolucion.getSolucion(oSolucion.idSolucion);

                if (!miRow.IsEjeTrazado_AmiliaNull())
                {
                    // 1. Obtenemos el array de bytes de la columna del DataSet
                    byte[] datosRecuperados_alzado = miRow.Alzado_Amilia;
                    using (MemoryStream ms = new MemoryStream(datosRecuperados_alzado))
                    {
                        // 3. Usamos tu método estático para reconstruir la clase
                        miAlzado = PerfilLongitudinal.Alzado.recuperaAlzado(ms);
                    }
                }
            }



            List<oValDesT<double, ComponenteLong>> miLstEntidadesOrderByPk = new List<oValDesT<double, ComponenteLong>>();

            List<ComponenteLong> misEntidades = miAlzado.ejeAlzado;

            foreach (ComponenteLong item in misEntidades)
            {
                miLstEntidadesOrderByPk.Add(new oValDesT<double, ComponenteLong>(item.getPkIni(), item));
            }


            //Ahora Creo la Coleccion de Entidades para su valoracion
            List<oEntidadesAlzados> miLstEntidades = new List<oEntidadesAlzados>();

            double? miAcuerdoPendienteMedia = null;

            for (int i = 0; i < miLstEntidadesOrderByPk.Count; i++)
            {


                if (miLstEntidadesOrderByPk[i].des.getTipoComponente() == ComponenteLong.tipoComponente.recta)
                {

                    Recta miPT = (Recta)miLstEntidadesOrderByPk[i].des;

                    miLstEntidades.Add(new oEntidadesAlzados(i + 1,
                                                             eTrazadoPerfilEntidades.recta,
                                                             miPT.getPkIni(),
                                                             miPT.getLongutid(),
                                                             miPT.getPendiente()));



                }
                else if (miLstEntidadesOrderByPk[i].des.getTipoComponente() == ComponenteLong.tipoComponente.parabola)
                {
                    Parabola miPC = (Parabola)miLstEntidadesOrderByPk[i].des;
                    Recta miPTprevio = (Recta)miLstEntidadesOrderByPk[i - 1].des;
                    Recta miPTNext = (Recta)miLstEntidadesOrderByPk[i + 1].des;

                    miAcuerdoPendienteMedia = pendienteMediaAcuerdo(miPTprevio.getPendiente(), miPTNext.getPendiente());

                    if (!double.IsNaN(miAcuerdoPendienteMedia.Value))
                    {

                        miLstEntidades.Add(new oEntidadesAlzados(i + 1,
                                                                 eTrazadoPerfilEntidades.encuentro,
                                                                 miPC.getPkIni(),
                                                                 miPC.getLongutid(),
                                                                 miAcuerdoPendienteMedia.Value));
                    }

                }
                else
                {
                    throw new Exception(string.Format("La SubEntidad del Trazado {0},\n No esta Configurada", miLstEntidadesOrderByPk[i].des.GetType().ToString()));
                }
            }

            return miLstEntidades;

        }
        private static double pendienteMediaAcuerdo(double iRectaPreviaPendienteConSigno, double iRectaSiguientePendienteConSigno)
        {

            double miLn1 = Math.Log(Math.Cos(iRectaPreviaPendienteConSigno));
            double miLn2 = Math.Log(Math.Cos(-iRectaSiguientePendienteConSigno));
            double miDiv = iRectaPreviaPendienteConSigno - iRectaSiguientePendienteConSigno;

            return (miLn1 + miLn2) / (miDiv);

        }
        #endregion


    }


    internal class oEntidadesAlzados
    {


        [LocalizedDisplayName("uiIndice", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 1)]
        [DefaultValue(null)]
        public int? id { get; private set; }

        [LocalizedDisplayName("uiPk", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 2)]
        [DefaultValue(null)]
        public double? pk { get; private set; }

        [LocalizedDisplayName("uiEntidad", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 3)]
        public string mEntidad { get; private set; }

        [LocalizedDisplayName("uiPendiente", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 4)]
        [DefaultValue(null)]
        public double? pendiente { get; private set; }

        [LocalizedDisplayName("uiLongPlantaM", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 5)]
        [DefaultValue(null)]
        public double? lonPlanta { get; private set; }

        [LocalizedDisplayName("uiLongM", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 6)]
        [DefaultValue(null)]
        public double? lon
        {
            get
            {
                return (lonPlanta.Value + (lonPlanta.Value * Math.Abs(pendiente.Value)));
            }
        }

        [LocalizedDisplayName("uiLongPlantaXPendienteAbs", typeof(strFrmInformes))]
        [BindingInfo(SortIndex = 7)]
        public double PenxLon { get { return Math.Abs(pendiente.Value) * lonPlanta.Value; } }



        public oEntidadesAlzados(int iIndice, eTrazadoPerfilEntidades iEntidad, double iPk, double iLonPlanta, double iPendiente)
        {
            id = iIndice;
            if (iEntidad.ToString() == "encuentro")
            {
                mEntidad = strFrmInformes.uiEncuentro;
            }
            else
            {
                mEntidad = strFrmInformes.uiRecta;
            }
            pk = iPk;
            lonPlanta = iLonPlanta;
            pendiente = iPendiente;
        }

    }



}
