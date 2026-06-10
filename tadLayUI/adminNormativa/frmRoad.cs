using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using tadLayLan.Tdi;

namespace tadLayUI
{
    using tadLayLan;
    using tadLayShare;
    using tadLayLogica;
    using tadLayData;



    public partial class frmRoad : Form
    {
        private dsInfra mDsTadil = null;
        private string mFileData = string.Empty;
        private double? mVelocidad = null;
        private double? mRadio = null;
        private double? mPeralte = null;
        private eRoadGrupo? mGrupo = null;



        #region"Constructores"
        public frmRoad(string iFileData, bool iShowBtnSelectRoad)
        {
            InitializeComponent();

            if (!File.Exists(iFileData))
            {
                oTadil.data.UserInfo.showInfo(string.Format(strGeneralUser.uiFileNotFoundSelectOrigen, iFileData));
                ucMenu_evOpen(null, new EventArgs());
            }
            else
            {
                if (!iShowBtnSelectRoad)
                {
                    btnSelect.Visible = false;
                }


                this.file = iFileData;
                loadXml();
            }


            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Visible = false;

        }
        #endregion
        #region "Propiedades"
        public string file
        {

            get
            {

                if (File.Exists(mFileData))
                {

                    return mFileData;
                }
                else
                {
                    throw new FileNotFoundException(mFileData);
                }


            }


            set
            {

                if (File.Exists(value))
                {
                    mFileData = value;

                    lblOrigenDatos.Text = value;
                }
                else
                {
                    throw new FileNotFoundException(mFileData);
                }

            }


        }
        public double velocidad
        {

            get
            {

                if (mVelocidad.HasValue)
                {
                    return mVelocidad.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("velocidad");

                }

            }

        }
        public double radio
        {

            get
            {

                if (mRadio.HasValue)
                {
                    return mRadio.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("radio");

                }

            }



        }
        public double peralte
        {

            get
            {

                if (mPeralte.HasValue)
                {
                    return mPeralte.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("peralte");
                }

            }



        }
        public eRoadGrupo grupo
        {

            get
            {

                if (mGrupo.HasValue)
                {
                    return mGrupo.Value;
                }
                else
                {
                    throw new oExPropertieNullValue("Grupo");

                }

            }



        }



        #endregion
        #region "DGV FORMATO"
        private void dgvSetUp()
        {

            //DGV GRUPOS
            dgvGrupos.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvGrupos.AllowUserToAddRows = false;
            dgvGrupos.MultiSelect = false;
            dgvGrupos.Columns[0].Visible = false;
            dgvGrupos.RowHeadersVisible = false;


            dgvGrupos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgvGrupos.AutoResizeColumns();
            dgvGrupos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvGrupos.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvGrupos.AlternatingRowsDefaultCellStyle.BackColor = Color.Snow;

            dgvGrupos.SelectionChanged += new EventHandler(dgvGrupos_SelectionChanged);

            dgvGrupos.Rows[0].Selected = true;

            //Traduccion

            dgvGrupos.Columns[1].HeaderText = strFrmRoadVpRadio.uiNombre;
            dgvGrupos.Columns[2].HeaderText = strFrmRoadVpRadio.uiDescripcion;


            //DGV ITEMS

            dgvCarreteras.AllowUserToResizeRows = false;
            dgvCarreteras.AllowUserToAddRows = false;
            dgvCarreteras.MultiSelect = false;
            dgvCarreteras.RowHeadersVisible = false;

            dgvCarreteras.AutoResizeColumns();
            dgvCarreteras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCarreteras.Columns[mDsTadil.tbCarreteraItems.IDGrupoColumn.ColumnName].Visible = false;
            dgvCarreteras.Columns[mDsTadil.tbCarreteraItems.IDVelColumn.ColumnName].Visible = false;


            dgvCarreteras.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvCarreteras.AlternatingRowsDefaultCellStyle.BackColor = Color.Snow;

            dgvCarreteras.Sort(dgvCarreteras.Columns[mDsTadil.tbCarreteraItems.velocidadColumn.ColumnName], ListSortDirection.Descending);
            dgvCarreteras.ClearSelection();

            dgvCarreteras.MouseClick += new MouseEventHandler(dgvCarreteras_MouseClick);


            //Traduccion
            dgvCarreteras.Columns[0].HeaderText = strFrmRoadVpRadio.uiVp;
            dgvCarreteras.Columns[1].HeaderText = strFrmRoadVpRadio.uiRp;
            dgvCarreteras.Columns[2].HeaderText = strFrmRoadVpRadio.uiPeralte;
        }



        #endregion
        #region "DGV EVENTOS"


        void dgvCarreteras_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {

                int myRowSelect = dgvCarreteras.HitTest(e.X, e.Y).RowIndex;


                if (myRowSelect != -1)
                {
                    mnuTbItemsEdit.Show(dgvCarreteras, e.X, e.Y);
                    dgvCarreteras.Rows[myRowSelect].Selected = true;
                }

            }


        }


        void dgvGrupos_SelectionChanged(object sender, EventArgs e)
        {

            try
            {

                if (dgvGrupos.SelectedRows.Count > 0)
                {
                    int myIndexSelected = dgvGrupos.SelectedRows[0].Index;

                    dgvGrupos.Rows[myIndexSelected].Selected = true;

                    dgvCarreteras.Sort(dgvCarreteras.Columns[mDsTadil.tbCarreteraItems.velocidadColumn.ColumnName], ListSortDirection.Descending);

                    dgvCarreteras.ClearSelection();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }



        }

        #endregion
        #region "FRM MENU"


        void ucMenu_evNew(object sender, EventArgs e)
        {
            try
            {

                string miFile = oTadil.data.Files.selectNewFileNormasVpRp();

                if (!string.IsNullOrEmpty(miFile))
                {
                    this.file = miFile;

                    this.loadXml();
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eProcesoCancelado);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }


        }

        void ucMenu_evOpen(object sender, EventArgs e)
        {

            try
            {
                string myFileData = oTadil.data.Files.getFileVpRp();

                if (!string.IsNullOrEmpty(myFileData))
                {
                    file = myFileData;

                    loadXml();
                }
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }

        void ucMenu_evSave(object sender, EventArgs e)
        {
            if (dgvCarreteras.IsCurrentCellInEditMode)
            {
                dgvCarreteras.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }


            saveXml();
        }

        void ucMenu_evSaveAs(object sender, EventArgs e)
        {
            try
            {
                string myFileNew = oTadil.data.Files.saveAsFileFromDialog(file, oTadil.data.Files.KFileNormasExtensionFiltro);
                file = myFileNew;
                saveXml();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);

            }
        }

        void ucMenu_evExit(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }

        #endregion
        #region "MENU CONTEXTUAL"
        //BORRAR REGISTRO
        private void mnuItemDelete_Click(object sender, EventArgs e)
        {

            DialogResult myResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroUnico);

            if (myResul == DialogResult.Yes)
            {
                dgvCarreteras.Rows.RemoveAt(dgvCarreteras.SelectedRows[0].Index);

                saveXml();

            }
        }
        // ADD REGISTRO
        private void mnuItemAdd_Click(object sender, EventArgs e)
        {
            dgvCarreteras.AllowUserToAddRows = true;
        }
        #endregion

        #region "Eventos"

        private void frmRoad_Load(object sender, EventArgs e)
        {

            //Traduccion
            var name = oTadil.KAppHeaderName;
            this.Text = name;
            lblHeader.Text = strFrmRoadVpRadio.uiHeader;
            grMaster.Text = strFrmRoadVpRadio.uiGrupoCarreteras;
            grItems.Text = strFrmRoadVpRadio.uiListadoVpRaPe;
            btnSelect.Text = strFrmRoadVpRadio.uiRoadSeleccionar;

            //Eventos del Menu
            ucMenu.evNew += new EventHandler(ucMenu_evNew);
            ucMenu.evOpen += new EventHandler(ucMenu_evOpen);
            ucMenu.evSaveAs += new EventHandler(ucMenu_evSaveAs);
            ucMenu.evExit += new EventHandler(ucMenu_evExit);
        }



        #endregion




        private void loadXml()
        {

            //Creo el DATASET
            mDsTadil = new dsInfra();

            //Cargo con datos el DataSet
            mDsTadil.ReadXml(file);

            BindingSource myMaster;
            BindingSource myDetail;

            //Vinculo los DATAGRIDVIEW
            myMaster = new BindingSource();
            myMaster.DataMember = mDsTadil.tbCarreteraGrupos.TableName;
            myMaster.DataSource = mDsTadil;

            myDetail = new BindingSource();
            myDetail.DataSource = myMaster;
            myDetail.DataMember = "FK_tbCarreteraGrupos_tbCarreteraItems";

            dgvGrupos.DataSource = myMaster;
            dgvCarreteras.DataSource = myDetail;

            //Formateo los DGV
            dgvSetUp();

        }


        private void saveXml()
        {

            try
            {

                mDsTadil.AcceptChanges();

                mDsTadil.WriteXml(file);

                dgvCarreteras.AllowUserToAddRows = false;

                dgvCarreteras.ClearSelection();

                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// BOTON SELECCIONAR CARRETERA
        /// </summary>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvCarreteras.SelectedCells.Count > 0)
                {
                    dgvCarreteras.CurrentCell.OwningRow.Selected = true;
                }

                if (dgvCarreteras.SelectedRows.Count == 1)
                {
                    mGrupo = (eRoadGrupo)Enum.Parse(typeof(eRoadGrupo), dgvGrupos.CurrentRow.Cells[mDsTadil.tbCarreteraGrupos.nombreColumn.ColumnName].Value.ToString(), true);
                    mVelocidad = (double)dgvCarreteras.CurrentRow.Cells[mDsTadil.tbCarreteraItems.velocidadColumn.ColumnName].Value;
                    mRadio = (double)dgvCarreteras.CurrentRow.Cells[mDsTadil.tbCarreteraItems.radioColumn.ColumnName].Value;
                    mPeralte = (double)dgvCarreteras.CurrentRow.Cells[mDsTadil.tbCarreteraItems.peralteColumn.ColumnName].Value;

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;

                    this.Close();
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



        #region "Botones"
        //SAVE
        void lnkSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (dgvCarreteras.IsCurrentCellInEditMode)
                {
                    dgvCarreteras.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }


                saveXml();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        #endregion





    }
}
