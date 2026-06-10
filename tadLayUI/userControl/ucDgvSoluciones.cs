using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PerfilLongitudinal;

namespace tadLayUI.userControl
{

    using engCadNet;
    using engNet.Str;

    using tadLayLan;
    using tadLayData;
    using tadLayLogica;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.Comandos;
    using tadLayLan.Tdi;
    using System.IO;
    using Newtonsoft.Json;
    using tadLayUI.adminProyecto;
    using tadLayLogica.logica.LandXml;
    using Autodesk.AutoCAD.DatabaseServices;
    using Autodesk.AutoCAD.ApplicationServices;
    using Autodesk.AutoCAD.Geometry;
    using tadLayLogica.EjeLongitudinalTadil;
    using tadLayLogica.EjeTrazadoTadil;
    using EjeDeTrazado.puntosDelEje;
    using tadLayLogica.estudioTipo;
    using tadLayLogica.logica.Entidades;

    public partial class ucDgvSoluciones : UserControl
    {
        #region "Fields"
        private BindingSource mBindMaster = null;
        private eEstudioTipo? mEstudioTipo = null;
        dsApp.tbSolucionRow mRowSolucion = null;
        #endregion
        #region "Constructor"

        public ucDgvSoluciones()
        {
            InitializeComponent();
        }

        #endregion
        #region "Propiedades"

        private bool existeEjeTrazadoCivil()
        {
            if (mRowSolucion.IsisCompleteEjeTrazadoNull())
            {
                return false;
            }
            else
            {
                return mRowSolucion.isCompleteEjeTrazado;
            }
        }
        private bool existePerfilLongitudinalCivil()
        {
            if (mRowSolucion.IsisCompletePerfilNull())
            {
                return false;
            }
            else
            {
                return mRowSolucion.isCompletePerfil;
            }
        }
        private bool allowEjeTrazado()
        {

            if (mRowSolucion.IsisCompleteEjeTrazadoNull())
            {
                return true;
            }
            else
            {
                return !mRowSolucion.isCompleteEjeTrazado;
            }

        }
        private bool allowPerfil()
        {

            if (mRowSolucion.IsisCompletePerfilNull())
            {
                return true;
            }
            else
            {
                return !mRowSolucion.isCompletePerfil;
            }


        }
        private bool allowObraLineal()
        {

            if (mRowSolucion.IsisCompleteObraLinealNull())
            {
                return true;
            }
            else
            {
                return !mRowSolucion.isCompleteObraLineal;
            }


        }
        private bool allowObraLinealExportar()
        {

            if (mRowSolucion.IsisCompleteObraLinealExportarNull())
            {
                return true;

            }
            else
            {
                return !mRowSolucion.isCompleteObraLinealExportar;
            }

        }
        private string nombreSolucion()
        {
            return this.ucDgvSolucion.CurrentRow.Cells["nombre"].Value as string;
        }

        #endregion
        #region "MetodosPublicos"
        public void setUp(eEstudioTipo iEstudioTipo)
        {
            mEstudioTipo = iEstudioTipo;

            #region "DatagridView"

            ucDgvSolucion.dgvSetUpUIDefault(false);

            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();

            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn miIsEjeBasicoComplete = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn miIsEjeTrazadoComplete = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn miIsPerfilComplete = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn miIsObraLinealComplete = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn miIsObraLinealExportComplete = new DataGridViewCheckBoxColumn();



            miId.Name = "id";
            miId.HeaderText = "id";
            miId.DataPropertyName = "id";


            miNombre.Name = "nombre";
            miNombre.HeaderText = strFrmSolucion.uiNombre;
            miNombre.DataPropertyName = "nombre";
            miIsEjeBasicoComplete.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            /*
             * Se añade una columa nueva para cargar los datos de la solucion ** juanma **
             */
            DataGridViewButtonColumn datossolucion = new DataGridViewButtonColumn();
            datossolucion.Name = "Datos Solucion";
            datossolucion.HeaderText = "Datos Solucion";
            datossolucion.DataPropertyName = "isCompleteDatos";
            datossolucion.Text = "Cargar";
            datossolucion.UseColumnTextForButtonValue = true; // Habilita el uso del texto en el botón
            datossolucion.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;



            miIsEjeBasicoComplete.Name = "ejeBasico";
            miIsEjeBasicoComplete.HeaderText = strFrmSolucion.uiEjeBasico;
            miIsEjeBasicoComplete.DataPropertyName = "isCompleteEjeBasico";
            miIsEjeBasicoComplete.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;

            miIsEjeTrazadoComplete.Name = "ejeTrazado";
            miIsEjeTrazadoComplete.HeaderText = strFrmSolucion.uiEjeTrazado;
            miIsEjeTrazadoComplete.DataPropertyName = "isCompleteEjeTrazado";
            miIsEjeTrazadoComplete.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;


            miIsPerfilComplete.Name = "perfilLongitudinal";
            miIsPerfilComplete.HeaderText = strFrmSolucion.uiPerfilLongitudinal;
            miIsPerfilComplete.DataPropertyName = "isCompletePerfil";
            miIsPerfilComplete.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;



            ucDgvSolucion.Columns.Add(miId);
            ucDgvSolucion.Columns.Add(miNombre);

            ucDgvSolucion.Columns.Add(miIsEjeBasicoComplete);
            ucDgvSolucion.Columns.Add(miIsEjeTrazadoComplete);
            ucDgvSolucion.Columns.Add(miIsPerfilComplete);


            if (mEstudioTipo.Value == eEstudioTipo.ESTINF)
            {
                miIsObraLinealComplete.Name = "obraLineal";
                miIsObraLinealComplete.HeaderText = strFrmSolucion.uiObraLineal;
                miIsObraLinealComplete.DataPropertyName = "isCompleteObraLineal";

                ucDgvSolucion.Columns.Add(miIsObraLinealComplete);


                miIsObraLinealExportComplete.Name = "obraLinealExport";
                miIsObraLinealExportComplete.HeaderText = strFrmSolucion.uiObraLinealExport;
                miIsObraLinealExportComplete.DataPropertyName = "isCompleteObraLinealExportar";

                ucDgvSolucion.Columns.Add(miIsObraLinealExportComplete);

            }

            ucDgvSolucion.Columns.Add(datossolucion);
            ucDgvSolucion.CellContentClick += UcDgvSolucion_CellContentClick;

            ucDgvSolucion.dgvColumnsHide(new int[] { 0 });

            #endregion
            #region "SET UP Tools Dgv"

            this.ucToolDgv1.Enabled = true;

            this.ucToolDgv1.lnkEdit.Visible = false;
            this.ucToolDgv1.lnkNew.Visible = false;
            this.ucToolDgv1.lnkEraseAll.Visible = false;

            this.ucToolDgv1.lnkErase.Enabled = true;

            this.ucToolDgv1.lnkErase.Click += new EventHandler(lnkErase_Click);


            #endregion
            #region "SET UP BOTONES"

            //EJE
            this.grEjeTrazado.Text = strFrmSolucion.uiEjeTrazado;
            this.btnEjeCrear.Text = strFrmSolucion.uiCrear;
            this.btnEjeRotular.Text = strFrmSolucion.uiRotular;
            this.btnEjeInforme.Text = strFrmSolucion.uiInformeVer;


            //PERFIL
            this.grPerfil.Text = strFrmSolucion.uiPerfilLongitudinal;
            this.btnPerfilCivil.Text = strFrmSolucion.uiCrear;
            //this.btnPerfilTadil.Text = strFrmSolucion.uiRotular;
            this.btnPerfilInforme.Text = strFrmSolucion.uiInformeVer;

            //OBRA LINEAL
            if (mEstudioTipo == eEstudioTipo.ESTINF)
            {

                this.grObraLineal.Text = strFrmSolucion.uiObraLineal;
                this.grObraLineal.Visible = true;

                this.btnObraLineal.Text = strFrmSolucion.uiCrear;
                this.btnObraLineal.Visible = true;

                this.btnExportarObraLineal.Visible = true;
                this.btnExportarObraLineal.Text = strFrmSolucion.uiObraLinealExportar;

                this.btnInformeObraLineal.Visible = true;
                this.btnInformeObraLineal.Text = strFrmSolucion.uiCrearInformeMedicionesSecciones;
            }
            else
            {
                this.grObraLineal.Visible = false;
                this.btnObraLineal.Visible = false;
                this.btnExportarObraLineal.Visible = false;
                this.btnInformeObraLineal.Visible = false;
            }

            #endregion
        }
        public void populate()
        {
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsApp.getInstance.dataset.tbSolucion.TableName;
            mBindMaster.DataSource = oSingletonDsApp.getInstance.dataset.tbSolucion;
            ucDgvSolucion.DataSource = mBindMaster;
        }
        #endregion
        #region "Botones"
        // Manejador de eventos para el clic en el botón
        private void UcDgvSolucion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica si la celda pertenece a la columna del botón
            if (ucDgvSolucion.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var filas = ucDgvSolucion.Rows;
                var fila = filas[e.RowIndex];
                string nombre = fila.Cells[1].Value.ToString();

                string directorio = Path.GetDirectoryName(oTadil.data.Files.fileApp);
                // Construir la nueva ruta con el nuevo nombre
                string nuevaRuta = Path.Combine(directorio, nombre + ".txt");

                string jsonContent = File.ReadAllText(nuevaRuta);
                var datosSolucion = JsonConvert.DeserializeObject<DatosSolucion>(jsonContent);
                eEstudioTipo iEstudio;
                if (this.mEstudioTipo != null)
                {
                    frmSolucion.getInstance(mEstudioTipo.Value).Cargar_Datos_Solucion(datosSolucion);
                }

                MessageBox.Show("Datos Cargados");

            }
        }

        /// <summary>
        /// DELETE SOLUCION
        /// </summary>
        private void lnkErase_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {

                    DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroUnico);

                    if (miResul == DialogResult.Yes)
                    {
                        //Borro Entidades del Cad
                        oSolucion.deleteEntidades(this.nombreSolucion());



                        //Borro de la Base Datos
                        mBindMaster.RemoveCurrent();
                        mBindMaster.EndEdit();

                        oSingletonDsApp.getInstance.save();

                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }


            }
            catch (Exception ex)
            {

                oTadil.data.UserInfo.showError(ex);

            }
        }
        /// <summary>
        ///EJE TRAZADO ANGELES CREAR
        /// </summary>
        private void btnEjeNew_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();

                    frmAppManager.getInstance.Hide();

                    if (!mRowSolucion.amilia)
                    {
                        tadLayLogica.Comandos.oComandoEjeTrazadoTadil.create((eEstudioTipo)mEstudioTipo, mRowSolucion.id);
                    }
                    else
                    {
                        tadLayLogica.Comandos.oComandoEjeTrazadoTadil.create_Amilia((eEstudioTipo)mEstudioTipo, mRowSolucion.id);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (StrongTypingException ex)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiFaltanDatosEjeBasico);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }
        }
        /// <summary>
        ///EJE TRAZADO TADIL --> ROTULAR
        /// </summary>
        private void btnEjeTrazadoRotular_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();
                    if (!mRowSolucion.amilia)
                    {
                        if (this.existeEjeTrazadoCivil())
                        {
                            tadLayLogica.Comandos.oComandoEjeTrazadoTadil.rotular(mRowSolucion.id);
                        }
                        else
                        {
                            oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjeTrazadoNoExiste);
                        }
                    }
                    else
                    {
                        if (this.existeEjeTrazadoCivil())
                        {
                            tadLayLogica.Comandos.oComandoEjeTrazadoTadil.rotular_Amilia(mRowSolucion.id);
                        }
                        else
                        {
                            oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjeTrazadoNoExiste);
                        }

                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        /// <summary>
        /// EJE TRAZADO TADIL --> INFORME
        /// </summary>
        private void btnInforme_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();

                    if (this.existeEjeTrazadoCivil())
                    {
                        List<EjeDeTrazado.oInformeEje> miListadoInforme = new List<EjeDeTrazado.oInformeEje>();
                        if (!mRowSolucion.amilia)
                        {
                            miListadoInforme = tadLayLogica.Comandos.oComandoEjeTrazadoTadil.getListado(mRowSolucion.id);

                        }
                        else
                        {
                            miListadoInforme = tadLayLogica.Comandos.oComandoEjeTrazadoTadil.getListado_Amilia(mRowSolucion.id);

                        }


                        frmData<EjeDeTrazado.oInformeEje> miFrm = new frmData<EjeDeTrazado.oInformeEje>();
                        miFrm.WindowState = FormWindowState.Maximized;
                        miFrm.populate(miListadoInforme, "Listado Eje Trazado", true, true);
                        miFrm.ShowDialog();

                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjeTrazadoNoExiste);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }



            }
            catch (Exception ex)
            {

                oTadil.data.UserInfo.showError(ex);
            }

        }
        /// <summary>
        /// PERFIL LONGITUDINAL CIVIL 3D --> CREAR
        /// </summary>
        private void btnPerfil_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (!oSingletonPuntosTerreno.getInstance.Creado())
                {
                    oTadil.data.UserInfo.showInfo("No se ha cargado el terreno");
                }
                else
                {

                    if (this.ucDgvSolucion.SelectedRows.Count != 0)
                    {
                        //Cargo los Datos del Current
                        this.getRowSolucionFromDataGrid();


                        if (this.existeEjeTrazadoCivil())
                        {

                            frmAppManager.getInstance.Hide();
                            if (!mRowSolucion.amilia)
                            {
                                tadLayLogica.Comandos.oComandoPerfilLongitudinalTadil.create(mEstudioTipo.Value, mRowSolucion.id);

                            }
                            else
                            {

                                tadLayLogica.Comandos.oComandoPerfilLongitudinalTadil.create_Amilia(mEstudioTipo.Value, mRowSolucion.id);
                            }
                        }
                        else
                        {
                            oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjeTrazadoNoExiste);
                        }



                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                    }
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }


        }
        /// <summary>
        /// PERFIL LONGITUDINAL TADIL --> CREAR 
        /// </summary>
        private void btnPerfilTadil_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();

                    frmAppManager.getInstance.Hide();

                    if (this.existeEjeTrazadoCivil())
                    {

                        tadLayLogica.Comandos.oComandoPerfilLongitudinalTadil.create(mEstudioTipo.Value, mRowSolucion.id);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjeTrazadoNoExiste);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }

        }
        /// <summary>
        /// PERFIL LONGITUDINAL TADIL --> INFORME
        /// </summary>
        private void btnPerfilInforme_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();

                    if (this.existeEjeTrazadoCivil())
                    {

                        if (this.existePerfilLongitudinalCivil())
                        {
                            Alzado miAlzado = null;
                            if (!mRowSolucion.amilia)
                            {
                                oComandoPerfilLongitudinalTadil.GetAlzadoGalibo(mEstudioTipo.Value, mRowSolucion.id, out miAlzado);
                            }
                            else
                            {
                                using (oSolucion miSolucion = new oSolucion(mRowSolucion.id))
                                {
                                    EjeTrazado miEjeTrazado = null;
                                    dsApp.tbSolucionRow miRow = oDalTbSolucion.getSolucion(mRowSolucion.id);

                                    if (!miRow.IsEjeTrazado_AmiliaNull())
                                    {
                                        byte[] datosRecuperados_alzado = miRow.Alzado_Amilia;
                                        using (MemoryStream ms = new MemoryStream(datosRecuperados_alzado))
                                        {
                                            // 3. Usamos tu método estático para reconstruir la clase
                                            miAlzado = Alzado.recuperaAlzado(ms);
                                        }
                                    }
                                }
                            }
                                
                            

                            List<oInformeLong> miListadoInforme = miAlzado.escribirInforme();
                            frmData<oInformeLong> miFrm = new frmData<PerfilLongitudinal.oInformeLong>();
                            miFrm.WindowState = FormWindowState.Maximized;
                            miFrm.populate(miListadoInforme, "Listado Perfil", true, true);
                            miFrm.ShowDialog();
                        }
                        else
                        {
                            oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjePerfilNoExiste);
                        }
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjeTrazadoNoExiste);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (Exception ex)
            {

                oTadil.data.UserInfo.showError(ex);
            }

        }
        /// <summary>
        /// OBRA LINEAL
        /// </summary>
        private void btnObraLineal_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();

                    if (this.existePerfilLongitudinalCivil())
                    {
                        frmAppManager.getInstance.Hide();

                        oComandoObraLineal miObraLineal = new tadLayLogica.Comandos.oComandoObraLineal();

                        if (!mRowSolucion.amilia)
                        {
                            miObraLineal.create(mRowSolucion.id);

                        }else
                        {
                            miObraLineal.create_Amilia(mRowSolucion.id);

                        }

                        if (miObraLineal.displaySeccionesConError())
                        {
                            frmData<oStringPropiedad> miFrm = new frmData<oStringPropiedad>();

                            miFrm.populate(miObraLineal.getListadoPkConErrores(), strGeneralUser.uiListadoSeccionesError, false, false);

                            miFrm.ShowDialog();
                        }
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjePerfilNoExiste);
                    }


                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }

        }
        /// <summary>
        /// EXPORTAR OBRA LINEAL
        /// </summary>
        private void btnExportarObraLineal_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();

                    if (this.allowObraLinealExportar())
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        tadLayLogica.Comandos.oComandoObraLinealExport.export(mRowSolucion.id);
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiObraLinealExportarRealizado);
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnInformeObraLineal_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();

                    oComandoObraLineal miObraLineal = new tadLayLogica.Comandos.oComandoObraLineal();
                    miObraLineal.createCSVListMedicionesSecciones(mRowSolucion.id);

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }



        #endregion

        #region "Eventos"

        /// <summary>
        /// EVENTO CLICK CELL DGV
        /// </summary>
        private void ucDgvSolucion_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex != -1)
            {
                this.getRowSolucionFromDataGrid();

                try
                {

                    if (e.ColumnIndex == 1 || e.ColumnIndex == 2 && !mRowSolucion.IshandleEjeBasico2DNull())
                    {
                        if (!string.IsNullOrEmpty(mRowSolucion.handleEjeBasico2D))
                        {
                            engCadNet.oTools.entidadHighLight(mRowSolucion.handleEjeBasico2D);
                        }
                    }
                    else if (e.ColumnIndex == 3 && !mRowSolucion.IsisCompleteEjeTrazadoNull() && mRowSolucion.isCompleteEjeTrazado)
                    {
                        if (!mRowSolucion.IshandleEjeTrazadoNull() && !string.IsNullOrEmpty(mRowSolucion.handleEjeTrazado))
                        {
                            engCadNet.oTools.entidadHighLight(mRowSolucion.handleEjeTrazado);
                        }
                    }
                    else if (e.ColumnIndex == 4 && !mRowSolucion.IshandlePerfilNull() && mRowSolucion.isCompletePerfil)
                    {
                        if (!string.IsNullOrEmpty(mRowSolucion.handlePerfil))
                        {
                            engCadNet.oTools.entidadHighLight(mRowSolucion.handlePerfil);
                        }
                    }
                    try
                    {
                        btnEjeCrear.Enabled = !mRowSolucion.isEjeTijera;
                    }
                    catch (Exception)
                    {
                        btnEjeCrear.Enabled = true;
                    }

                }
                catch (oExEntidadNoExiste ex)
                {
                    oTadil.data.UserInfo.showInfo(string.Format(strGeneralUser.uiEntidadNotFound, ex.Message));

                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiEntidadNotFoundRecomendacion);
                }

            }


        }


        /// <summary>
        /// 
        /// </summary>
        private void getRowSolucionFromDataGrid()
        {

            DataRowView miDataRowView = (DataRowView)mBindMaster.Current;

            mRowSolucion = (dsApp.tbSolucionRow)miDataRowView.Row;
        }

        #endregion

        private void btnExportaLong_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ucDgvSolucion.SelectedRows.Count != 0)
                {
                    //Cargo los Datos del Current
                    this.getRowSolucionFromDataGrid();

                    if (this.existeEjeTrazadoCivil())
                    {

                        string miFileExport = oTadil.data.Files.saveFileFromDialog(/*mRowSolucion.id.ToString() + "_trazado",*/ "(*.tadeje)|*.tadeje");

                        if (string.IsNullOrEmpty(miFileExport))
                        {
                            oTadil.data.UserInfo.procesoCancelado();
                        }
                        else
                        {
                            if (!mRowSolucion.amilia)
                            {
                                tadLayLogica.Comandos.oComandoEjeTrazadoTadil.exportar(miFileExport, mEstudioTipo.Value, mRowSolucion.id);

                            }
                            else
                            {
                                tadLayLogica.Comandos.oComandoEjeTrazadoTadil.exportar_Amilia(miFileExport, mEstudioTipo.Value, mRowSolucion.id);

                            }

                            if (this.existePerfilLongitudinalCivil())
                            {
                                string miFileExportPer = oTadil.data.Files.saveFileFromDialog(/*mRowSolucion.id.ToString() + "_perfil",*/ "(*.tadper)|*.tadper");
                                if (string.IsNullOrEmpty(miFileExportPer))
                                {
                                    oTadil.data.UserInfo.procesoCancelado();
                                }
                                else
                                {
                                    if (!mRowSolucion.amilia)
                                    {
                                        tadLayLogica.Comandos.oComandoPerfilLongitudinalTadil.exportarPerfil(miFileExportPer, mEstudioTipo.Value, mRowSolucion.id);

                                    }
                                    else
                                    {
                                        tadLayLogica.Comandos.oComandoPerfilLongitudinalTadil.exportarPerfil_Amilia(miFileExportPer, mEstudioTipo.Value, mRowSolucion.id);

                                    }
                                }
                            }
                            else
                            {
                                oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjePerfilNoExiste);
                            }
                        }
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiEjeTrazadoNoExiste);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }



            }
            catch (Exception ex)
            {

                oTadil.data.UserInfo.showError(ex);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecciona un archivo LandXML", // Título de la ventana
                Filter = "Archivos LandXML (*.xml)|*.xml|Todos los archivos (*.*)|*.*", // Filtro para solo mostrar archivos .landxml
                DefaultExt = "xml" // Extensión por defecto
            };

            // Mostrar el cuadro de diálogo y comprobar si se ha seleccionado un archivo
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Obtener la ruta del archivo seleccionado
                string archivoLandXML = openFileDialog.FileName;

                ImportResult landXmlImporter = new LandXmlImporter().Import(archivoLandXML, 2, 8);
                TrazadoLandXml trazado = new TrazadoLandXml(archivoLandXML, 8);
                EjeTrazado miEjeTrazadoTadil = new EjeTrazado(trazado.vertices, trazado.Trazado, 2, 2);

                var eje = Planta(miEjeTrazadoTadil);
                // Pedimos el punto de inserción ANTES de abrir la transacción
                // (getPointCad requiere interacción del usuario y no debe ejecutarse dentro de una transacción CAD)
                Point3d miPtoInsert = (Point3d)engCadNet.oTools.getPointCad(strUserCad.uiSelectPtoInsertPerfilLongitudinal);




                using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
                {
                    using (Transaction tr = oCadManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);


                        // --- PERFIL LONGITUDINAL ---
                        // Reconstruir el Alzado original
                        Alzado miAlzado = AlzadoAmilia.ReconstruirAlzado(
                            landXmlImporter.AlzadoComponentes,
                            landXmlImporter.KVs.ToArray(),
                            velocidad: 100,
                            intervaloSecciones: 20,
                            eje,
                            oSingletonTerreno.getInstance.getZFromXY,
                            oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto
                        );

                        Guitarra miGuitarra = tadLayLogica.EjeLongitudinalTadil.oFactoryPerfilLongitudinalTadil.getGuitarra(miPtoInsert, miAlzado, 100, 10);

                        PerfilLongitudinalDraw miPerfilDraw = new PerfilLongitudinalDraw(
                            miGuitarra,
                            miAlzado,
                            miEjeTrazadoTadil,
                            oSingletonTerreno.getInstance.getZFromXY,
                            oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);

                        string capap = "prueba perfil";
                        string capap1 = "prueba perfil - Eje";
                        string capap2 = "prueba perfil - Terreno";
                        string capap3 = "prueba perfil - EjeLongitudinal";
                        engCadNet.oLayer.addLayer(capap, 4, false);
                        engCadNet.oLayer.addLayer(capap1, 4, false);
                        engCadNet.oLayer.addLayer(capap2, 4, false);
                        engCadNet.oLayer.addLayer(capap3, 4, false);

                        miPerfilDraw.drawEje(capap1);
                        miPerfilDraw.drawTerreno(capap2);
                        miPerfilDraw.drawEjeLongitudinal(capap3);
                        miPerfilDraw.drawGuitarra(capap);




                        Polyline miEjeAlzado = miPerfilDraw.getPolylineEjeAlzado();
                        MemoryStream miEjeMem = miAlzado.guardarAlzado();

                        /*oXdata.setXdata(miEjeAlzado.ObjectId, "tadilEjeAlzado", miSolucion.idSolucion.ToString());
                        ObjectId miId = oSerializarEntidad.StoreObjectInExtensionDictionary("info", miEjeAlzado, tr, miEjeMem, miAlzado.GetType().FullName);

                        oDalTbSolucion.addPerfilLongitudinal(iIdSolucion, miEjeAlzado.Handle.ToString());
                        */

                        string capap_p = "prueba perfil -peralte";
                        engCadNet.oLayer.addLayer(capap_p, 4, false);
                        miPerfilDraw.drawPeralte(capap_p);//mal


                        #region "Calcular Estructuras"
                        //Obtengo el oEstudioCarretera
                        oEstudioCarretera miEstudioCarretera = oFactoryEstudios.getEstudioCarretera(eEstudioTipo.ESTINF, new Guid());

                        Polyline miLwEjeTrazado = eje;
                        Polyline miLwEjePerfilRasante = miPerfilDraw.getPolylineEjeAlzado();
                        double miSeccionIntervalo = oSingletonProyecto.getInstance.seccionIntervalo;

                        oPerfilSeccionesInfo miPerfilSeccionesInfo = new oPerfilSeccionesInfo(miLwEjeTrazado, miLwEjePerfilRasante, miEstudioCarretera,
                                                                                 oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto);

                        List<oEstructura> miLstPerfilEstructurasInfo = miPerfilSeccionesInfo.getPerfilSeccionesInfo(miSeccionIntervalo);

                        #endregion

                        miPerfilDraw.drawEstructuras(capap_p, miLstPerfilEstructurasInfo);





                        // Commit al final, una vez dibujadas todas las entidades
                        oCadManager.thisEditor.UpdateScreen();
                        tr.Commit();
                    }
                }
            }
        }
        private Polyline Planta(EjeTrazado trazado)
        {
            using (DocumentLock myDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                using (Transaction tr = oCadManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(oCadManager.thisBase.BlockTableId, OpenMode.ForRead, false);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                    Polyline miEje = new Polyline();
                    int index = 0;
                    foreach (var componente in trazado.getComponentes)
                    {
                        foreach (var componentPoint in componente.getComponentPoints())
                        {
                            miEje.AddVertexAt(index, new Point2d(componentPoint[0], componentPoint[1]), 0, 0, 0);
                            index++;
                        }
                    }
                    engCadNet.oLayer.addLayer("Prueba Eje", 5, false);
                    miEje.Layer = "Prueba Eje";
                    btr.AppendEntity(miEje);
                    tr.AddNewlyCreatedDBObject(miEje, true);

                    string capapk = "Prueba Eje - pk";
                    string capaps = "prueba Eje - ps";
                    engCadNet.oLayer.addLayer(capapk, 4, false);
                    engCadNet.oLayer.addLayer(capaps, 4, false);

                    oEjeTrazadoTadilRotular miEjeTrazadoCivilRotular = new oEjeTrazadoTadilRotular();

                    miEjeTrazadoCivilRotular.rotular(trazado, capapk, capaps);



                    // Commit al final, una vez dibujadas todas las entidades
                    oCadManager.thisEditor.UpdateScreen();
                    tr.Commit();
                    return miEje;
                }
            }
        }
    }
}
