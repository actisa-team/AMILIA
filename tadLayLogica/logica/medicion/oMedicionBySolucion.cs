using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using tadLayData;
using tadLayLan;

namespace tadLayLogica.logica.medicion
{
    using engNet;
    using engNet.ClassT;

    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.datos.Gis;
    using tadLayLogica.logica.medicion;
    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;

    /// <summary>
    /// MEDICION PARTIDAS CAD (BALANCE TIERRAS + PARTIDAS SIMPLE)
    /// </summary>
    /// <remarks>NO MEDIMOS LAS EXPROPIACIONES</remarks>
    public class oMedicionBySolucion : IDisposable
    {
        #region "Variables Privadas"

        //Id Solucion
        private Guid mIdSolucion;

        // Listado de Secciones de la Obra Lineal SIN Filtrar (EST,TUN, CUN, MUR, etc...)
        private List<oSeccionMedicion> mLstSeccionesMediciones = new List<oSeccionMedicion>();

        // Listado de Bancos
        private List<oExcavacionBalanceTierras> mLstBancoMaterial = new List<oExcavacionBalanceTierras>();

        // Mediciones Con Balance Tierras
        private List<oMedItemModel> mLstMedicionBalanceTierrasGranular;
        private List<oMedItemModel> mLstMedicionBalanceTierrasAsiento;
        private List<oMedItemModel> mLstMedicionBalanceTierrasRelleno;
        private List<oMedItemModel> mLstMedicionesBalanceTierrasExcavacion;

        //Mediciones de Partidas Simples No Balance de Tierras
        private List<oMedItemModel> mLstMedicionesPartidasSimples;


        //Mediciones Totales
        private List<oMedItemModel> mLstMedicionesCAD;

        #endregion
        #region "Constructor"

        public oMedicionBySolucion(Guid iIdSolucion)
        {
            mIdSolucion = iIdSolucion;
        }

        #endregion
        #region "Propiedades"
        public int seccionesCount
        {
            get
            {
                return mLstSeccionesMediciones.Count;
            }
        }
        #endregion
        #region "Metodos Publicos"
        //ADD SECCIONES
        public void addSecciones(oSeccionMedicion iSecMedicion)
        {
            mLstSeccionesMediciones.Add(iSecMedicion);
        }
        //REALIZAR LAS MEDICIONES CAD
        public void createMedicionesCad()
        {

            if (mLstSeccionesMediciones == null | mLstSeccionesMediciones.Count == 0)
            {
                throw new Exception("xEl Listado de Mediciones por Sección es Nulo");
            }

            //Creo el Filtro para las partidas de Excavacion
            List<eNodo> miLstNodosExcavacion = new List<eNodo>(new eNodo[] { eNodo.EXCDESMONTE, eNodo.EXCSANEODESMONTE, eNodo.EXCSANEOTERRAPLEN });

            //Creo el Banco de Materiales Excavacion
            mLstBancoMaterial = getBancoExcavacion(miLstNodosExcavacion);

            List<oClientesBalanceTierras> miLstGranularClientes = getClientesBancoExcavacion(4);
            List<oClientesBalanceTierras> miLstAsientoClientes = getClientesBancoExcavacion(3);
            List<oClientesBalanceTierras> miLstTerraplenClientes = getClientesBancoExcavacion(2);

            string miFileCsv = oTadil.data.Files.saveAsFileCsvFromDialog(strGeneralUser.uiInformesObraLineal);
            if (!string.IsNullOrEmpty(miFileCsv))
                EscribirInformes(miFileCsv, miLstGranularClientes, miLstAsientoClientes, miLstTerraplenClientes, true);




            //Balance de Tierras del material Granular
            mLstMedicionBalanceTierrasGranular = createBalanceTierrasRelleno(miLstGranularClientes, eNodo.CAPASGRANULARES);

            //Balance de Tierras del material Asiento
            mLstMedicionBalanceTierrasAsiento = createBalanceTierrasRelleno(miLstAsientoClientes, eNodo.CAPASASIENTO);

            //Balance de Tierras del Material Terraplen
            mLstMedicionBalanceTierrasRelleno = createBalanceTierrasRelleno(miLstTerraplenClientes, eNodo.TERRAPLENandSANEOS);

            if (!string.IsNullOrEmpty(miFileCsv))
                EscribirInformes(miFileCsv, miLstGranularClientes, miLstAsientoClientes, miLstTerraplenClientes, false);


            //Balance de Excavaciones desde Los Bancos de Excavación
            mLstMedicionesBalanceTierrasExcavacion = createBalanceTierrasExcavacion();

            //Mediciones de las Partidas Simples
            mLstMedicionesPartidasSimples = createPartidasSimplesAgruparAndMedir();

            //Mediciones Totales CAD (BalanceTierras+PartidasSimples)
            mLstMedicionesCAD = getMedicionesTotales();

            //Guardo las Mediciones
            oDalAppMedicones.addMedicionesCad(mIdSolucion, mLstMedicionesCAD);


        }

        private void EscribirInformes(string miFileCsv, List<oClientesBalanceTierras> miLstGranularClientes, List<oClientesBalanceTierras> miLstAsientoClientes,
            List<oClientesBalanceTierras> miLstTerraplenClientes, bool ini)
        {
            try
            {
                var path = Path.GetDirectoryName(miFileCsv);
                var fileName = path + @"\" + Path.GetFileNameWithoutExtension(miFileCsv);

                string fileNameGranular, fileNameAsiento, fileNameTerraplen;

                if (ini)
                {
                    fileNameGranular = fileName + "-ClienteGranularInicial.csv";
                    fileNameAsiento = fileName + "-ClienteAsientoInicial.csv";
                    fileNameTerraplen = fileName + "-ClienteTerraplenInicial.csv";
                }
                else
                {
                    fileNameGranular = fileName + "-ClienteGranularFin.csv";
                    fileNameAsiento = fileName + "-ClienteAsientoFin.csv";
                    fileNameTerraplen = fileName + "-ClienteTerraplenFin.csv";

                    var fileNamebancosFinal = fileName + "-BancosFinal.csv";
                    //Test Imprimir los bancos de material despues de Dejar el material
                    this.printBancos(mLstBancoMaterial, fileNamebancosFinal);

                }
                this.printClientes(miLstGranularClientes, fileNameGranular);
                this.printClientes(miLstAsientoClientes, fileNameAsiento);
                this.printClientes(miLstTerraplenClientes, fileNameTerraplen);

            }
            catch (Exception e)
            {
                oUserInfo userInfo = new oUserInfo();
                userInfo.showError(e);
            }
        }

        #endregion 
        #region "Metodos Privados"
        /// <summary>
        /// Creación del Banco de Materiales
        /// </summary>
        private List<oExcavacionBalanceTierras> getBancoExcavacion(List<eNodo> iLstNodosFiltro)
        {


            List<oValDesT<Guid, oMedItemModel>> miLstExcZonaIdItem = new List<oValDesT<Guid, oMedItemModel>>();

            List<oMedItemModel> miLstMedicionesExc;

            foreach (oSeccionMedicion item in mLstSeccionesMediciones)
            {
                if (item.tipo == eSecMedicion.MOVTIE)
                {
                    miLstMedicionesExc = new List<oMedItemModel>();

                    miLstMedicionesExc = item.filtroByCode(iLstNodosFiltro);

                    foreach (oMedItemModel itemExc in miLstMedicionesExc)
                    {
                        miLstExcZonaIdItem.Add(new oValDesT<Guid, oMedItemModel>(item.zonaId, itemExc));
                    }
                }
            }



            //Agrupo la Medicion
            var miQueryGrByZonaExc = from p in miLstExcZonaIdItem
                                     group p by new { p.val, p.des.row.idMaterial } into g
                                     select new { zonaId = g.Key.val, matRow = g.First().des.row, m3ExcSum = g.Sum(k => k.des.medicion) };

           List<oExcavacionBalanceTierras> miLstExcBalance = new List<oExcavacionBalanceTierras>();
            //Creo una Lista con todas las Excavaciones Banco.
           foreach (var p in miQueryGrByZonaExc)
            {
                dsBd.tbGeoRow miZonaRow = oSingletonDsBd.getInstance.getZonaMovimientoTierras(p.zonaId);
                Guid? granularMat = null, asientoMat = null, terraplenMat = null;
                if (miZonaRow.granularAproPc != 0) granularMat = miZonaRow.granularAproMat;
                if (miZonaRow.asientoAproPc != 0) asientoMat = miZonaRow.asientoAproMat;
                if (miZonaRow.rellenoAproPc != 0) terraplenMat = miZonaRow.rellenoAproMat;

                miLstExcBalance.Add(new oExcavacionBalanceTierras(p.matRow,
                    p.m3ExcSum,
                    miZonaRow.coeEsponjamiento,
                    miZonaRow.coePasoTerraplen,
                    granularMat,
                    miZonaRow.granularAproPc,
                    asientoMat,
                    miZonaRow.asientoAproPc,
                    terraplenMat,
                    miZonaRow.rellenoAproPc));
            }


            //Ordeno los Banco de forma Descendente por precio a vertedero mas alto.(Primero los precio Vertedero Mas Elevado)
            var miQuerySort = from p in miLstExcBalance
                              orderby p.precioVertedero descending
                              select p;

            //TEST PRINT EXCAVACION
           // this.printExcavacion(iLstNodosFiltro,@"c:\01-excavacionMedicion.csv");
           // this.printBancos(miLstExcBalance, @"c:\02-bancosIniciales.csv");


            return miQuerySort.ToList();

        }



        /// <summary>
        /// Creación de los Clientes del Banco
        /// </summary>
        private List<oClientesBalanceTierras> getClientesBancoExcavacion(short iIdClasificacionPartidas)    
        {

            List<oMedItemModel> miLstMediciones = new List<oMedItemModel>();

            foreach (oSeccionMedicion item in mLstSeccionesMediciones)
            {
                if (item.tipo == eSecMedicion.MOVTIE)
                {
                    miLstMediciones.AddRange(item.getListadoPartidasByIdClasificacion(iIdClasificacionPartidas));
                }
            }



            //Agrupo la Medicion
            var miQueryGrByZonaAndMaterial = from p in miLstMediciones
                                             group p by new { p.row.idMaterial } into g
                                             select new { matRow = g.First().row, m3Sum = g.Sum(k => k.medicion) };

            //Creo una Lista con todas las Excavaciones Banco.
            List<oClientesBalanceTierras> miLstRellenos = miQueryGrByZonaAndMaterial.ToList().ConvertAll(p => new oClientesBalanceTierras(p.matRow, p.m3Sum));


            //Ordeno las Lista Por Precio, Primero los mas Caros
            var miQuerySort = from p in miLstRellenos
                              orderby p.precioEmpleo descending
                              select p;

            return miQuerySort.ToList();

        }


        /// <summary>
        /// Crear el Balance de Tierras Relleno por Tipo de Relleno
        /// </summary>
        private List<oMedItemModel> createBalanceTierrasRelleno(List<oClientesBalanceTierras> iLstClientesBalanceTierras, eNodo iNodo)
        {

            List<oMedItemModel> miLstMedicionBalanceTierrasRelleno = new List<oMedItemModel>();

            double miM3Empleo = 0;

            foreach (oClientesBalanceTierras itemCliente in iLstClientesBalanceTierras)
            {

                foreach (oExcavacionBalanceTierras itemBanco in mLstBancoMaterial)
                {

                    if (iNodo == eNodo.CAPASGRANULARES)
                    {
                        miM3Empleo = itemBanco.prestarGranular(itemCliente.idMaterial, itemCliente.m3Prestamo);
                    }
                    else if (iNodo == eNodo.CAPASASIENTO)
                    {
                        miM3Empleo = itemBanco.prestarAsiento(itemCliente.idMaterial, itemCliente.m3Prestamo);

                    }
                    else if (iNodo == eNodo.TERRAPLENandSANEOS)
                    {
                        miM3Empleo = itemBanco.prestarTerraplen(itemCliente.idMaterial, itemCliente.m3Prestamo);
                    }
                    else
                    {
                        throw new  oExEnumNotImplemented(iNodo.ToString());
                    }


                   

                    //Material Prestado lo Añado a la medicion
                    itemCliente.addM3Empleo(miM3Empleo);

                    if (itemCliente.isComplete())
                    {
                        break;
                    }
                }

                //He Salido de la solictar Material y No Esta Completado
                miLstMedicionBalanceTierrasRelleno.AddRange(itemCliente.createEmpleoPrestamo(iNodo));
            }


            return miLstMedicionBalanceTierrasRelleno;
           

        }
        /// <summary>
        /// Crear el Balance de Tierras Excavación
        /// </summary>
        private List<oMedItemModel> createBalanceTierrasExcavacion()
        {

            List<oMedExcavacion> miLstMedicionesBalanceTierrasExcavacion = new List<oMedExcavacion>();

            //Balance Tierras de la Excavacion
            //Una vez asignado todos los materiales de los Bancos de Excavación
            //Vamos a determinar cuales son los m3 Empleo y Vertedero.
            foreach (oExcavacionBalanceTierras item in mLstBancoMaterial)
            {
                miLstMedicionesBalanceTierrasExcavacion.AddRange(item.createEmpleoVertedero());
            }



            //Debo de Agrupar el material por Empleo y Vertedero

            //Agrupo la Medicion por Material y Precio Principal is True (Empleo-Vertedero)
            var miQueryGrExcaMatAndPrecio = from p in miLstMedicionesBalanceTierrasExcavacion
                                             group p by new { p.row.idMaterial, p.isPrecioPrincipal} into g
                                             select new { matRow = g.First().row, m3Sum = g.Sum(k => k.medicion), isEmpleo = g.First().isPrecioPrincipal};



            return miQueryGrExcaMatAndPrecio.ToList().ConvertAll(p => oFactoryPartidas.createMedicionesPartidasBalanceTierras(p.matRow,p.m3Sum,p.isEmpleo.Value,eNodo.EXCAVACION));

         
        }

        private List<oMedItemModel> createPartidasSimplesAgruparAndMedir()
        {

            List<oMedItemModel> miLstMedicionesTotales = new List<oMedItemModel>();

            foreach (oSeccionMedicion seccion in mLstSeccionesMediciones)
            {
                miLstMedicionesTotales.AddRange(seccion.lstMedicion);
            }



            //Agrupo la Medicion
            var miQueryMedSimpleGrupo = from p in miLstMedicionesTotales
                                        where p.isBalanceTierras == false
                                        group p by new { p.row.idMaterial } into g
                                        select new { matRow = g.First().row, m3Sum = g.Sum(k => k.medicion), code = g.First().code };

            //Creo una Lista con todas las Excavaciones Banco.
            List<oMedItemModel> miLstMedicionesAgrupadas = miQueryMedSimpleGrupo.ToList().ConvertAll(p => oFactoryPartidas.createMedicionPartidasSimples(p.matRow, p.m3Sum, p.code));

            return miLstMedicionesAgrupadas;

        }

        private List<oMedItemModel> getMedicionesTotales()
        {

            List<oMedItemModel> miLstMedicionestotales = new List<oMedItemModel>();

            miLstMedicionestotales.AddRange(mLstMedicionesBalanceTierrasExcavacion);
            miLstMedicionestotales.AddRange(mLstMedicionBalanceTierrasGranular);
            miLstMedicionestotales.AddRange(mLstMedicionBalanceTierrasAsiento);
            miLstMedicionestotales.AddRange(mLstMedicionBalanceTierrasRelleno);
            miLstMedicionestotales.AddRange(mLstMedicionesPartidasSimples);


            //Ordeno las Partidas por su Id Orden

            var miQuery = from p in miLstMedicionestotales
                          orderby p.orden ascending
                          select p;

            return miQuery.ToList();
         
        }

        #endregion




        private void printExcavacion(List<eNodo> iLstNodosFiltro, string iFileFullPathConExtension)
        {
            List<oValDesFilT<string, string, double>> miLstExcZonaIdItem = new List<oValDesFilT<string, string, double>>();

            List<oMedItemModel> miLstMedicionesExc;

            foreach (oSeccionMedicion item in mLstSeccionesMediciones)
            {
                if (item.tipo == eSecMedicion.MOVTIE)
                {
                    miLstMedicionesExc = new List<oMedItemModel>();

                    miLstMedicionesExc = item.filtroByCode(iLstNodosFiltro);

                    foreach (oMedItemModel itemExc in miLstMedicionesExc)
                    {
                        miLstExcZonaIdItem.Add(new oValDesFilT<string, string, double>(itemExc.row.nombre, itemExc.code.ToString(), itemExc.medicion));
                    }
                }
            }


            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oValDesT<string, string>> miLstFooter = new List<oValDesT<string, string>>();


            engNet.Csv.oCsv.write<oValDesT<string, string>,
                                 oValDesFilT<string, string, double>,
                                 oValDesT<string, string>>(miLstHeader, miLstExcZonaIdItem, miLstFooter,iFileFullPathConExtension);


        }



        private void printBancos(List<oExcavacionBalanceTierras> iListadoBancos,string iFileFullPathConExtendion)
        {

   

            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oValDesT<string, string>> miLstFooter = new List<oValDesT<string, string>>();


            engNet.Csv.oCsv.write<oValDesT<string, string>,
                                  oExcavacionBalanceTierras,
                                  oValDesT<string, string>>(miLstHeader, iListadoBancos, miLstFooter, iFileFullPathConExtendion);

        }


        private void printClientes(List<oClientesBalanceTierras> iListadoClientes, string iFileFullPathConExtendion)
        {



            List<oValDesT<string, string>> miLstHeader = new List<oValDesT<string, string>>();
            List<oValDesT<string, string>> miLstFooter = new List<oValDesT<string, string>>();


            engNet.Csv.oCsv.write<oValDesT<string, string>,
                                  oClientesBalanceTierras,
                                  oValDesT<string, string>>(miLstHeader, iListadoClientes, miLstFooter, iFileFullPathConExtendion);

        }
        

        public void Dispose()
        {
        }
    }

    }




