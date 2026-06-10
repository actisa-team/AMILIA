using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI
{

    using tadLayLan;
    using System.IO;
    using tadLayData;
    using tadLayShare;
    using tadLayLogica;

    public partial class frmKv : Form
    {

        private string mFileData = string.Empty;
        private dsKv mDsKv = null;


        public frmKv(string iFileData)
        {
            InitializeComponent();

            if (!File.Exists(iFileData))
            {
                throw new FileNotFoundException(iFileData);
            }
            else
            {
                file = iFileData;
                loadXml();
            }


            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(ucMenu_evSave);
            ucToolDetail1.lnkCancel.Visible = false;
        }



        #region "Propiedades"

        /// <summary>
        /// File Data KV
        /// </summary>
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
                    lblOrigenDatos.Text = mFileData;
                }
                else
                {
                    throw new FileNotFoundException(mFileData);
                }
            }

        }

        #endregion


        #region "Metodos Privados"

        private void loadXml()
        {

            //Creo el DATASET
            mDsKv = new dsKv();

            //Cargo con datos el DataSet
            mDsKv.ReadXml(file);

            //Vinculo los DATAGRIDVIEW
            BindingSource myMaster = new BindingSource();
            myMaster.DataMember = mDsKv.tbKv.TableName;
            myMaster.DataSource = mDsKv;

            BindingSource myDetail = new BindingSource();
            myDetail.DataSource = myMaster;

            dgvKv.DataSource = myMaster;

            //Formateo los DGV
            dgvSetUp();

        }

        private void traduccion()
        {

            var name = oTadil.KAppHeaderName;
            this.Text = name;
            lblHeader.Text = strFrmKv.uiHeader;


        }

        private void dgvSetUp()
        {

            //Header Row


            //DGV ITEMS
            dgvKv.AllowUserToResizeRows = false;
            dgvKv.AllowUserToAddRows = false;
            dgvKv.MultiSelect = false;
            dgvKv.RowHeadersVisible = false;

            dgvKv.AutoResizeColumns();
            dgvKv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvKv.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvKv.AlternatingRowsDefaultCellStyle.BackColor = Color.Snow;

            dgvKv.Sort(dgvKv.Columns[mDsKv.tbKv.vpColumn.ColumnName], ListSortDirection.Descending);
            dgvKv.ClearSelection();

            dgvKv.MouseClick += new MouseEventHandler(dgvKv_MouseClick);


            dgvKv.Columns[0].HeaderText = strFrmKv.uiVp;
            dgvKv.Columns[1].HeaderText = strFrmKv.uiKvMinConvexo;
            dgvKv.Columns[2].HeaderText = strFrmKv.uiKvMinConcavo;
            dgvKv.Columns[3].HeaderText = strFrmKv.uiKvOptConvexo;
            dgvKv.Columns[4].HeaderText = strFrmKv.uiKvOptConcavo;

        }


        void dgvKv_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {

                int myRowSelect = dgvKv.HitTest(e.X, e.Y).RowIndex;


                if (myRowSelect != -1)
                {
                    mnuTbItemsEdit.Show(dgvKv, e.X, e.Y);
                    dgvKv.Rows[myRowSelect].Selected = true;
                }

            }


        }

        private void saveXml()
        {

            mDsKv.AcceptChanges();

            mDsKv.WriteXml(file);

            dgvKv.AllowUserToAddRows = false;

            dgvKv.ClearSelection();

            oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);
        }


        #endregion


        #region "MENU CONTEXTUAL"



        //BORRAR REGISTRO
        private void mnuItemDelete_Click(object sender, EventArgs e)
        {

            DialogResult myResul = MessageBox.Show(strGeneralUser.uiBorrarRegistro, "TADIL", MessageBoxButtons.YesNo);

            if (myResul == DialogResult.Yes)
            {
                dgvKv.Rows.RemoveAt(dgvKv.SelectedRows[0].Index);

                saveXml();

            }
        }
        // ADD REGISTRO
        private void mnuItemAdd_Click(object sender, EventArgs e)
        {
            dgvKv.AllowUserToAddRows = true;
        }
        #endregion

        #region "MENU"

        void ucMenu_evNew(object sender, EventArgs e)
        {

            try
            {

                string miFile = oTadil.data.Files.selectNewFileNormasKv();

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
                string myFileData = oTadil.data.Files.getFileVpKv();

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
            try
            {
                if (dgvKv.IsCurrentCellInEditMode)
                {
                    dgvKv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }

                saveXml();
            }
            catch (Exception ex)
            {

                oTadil.data.UserInfo.showError(ex);
            }
        }
        void ucMenu_evSaveAs(object sender, EventArgs e)
        {
            try
            {
                string myFileNew = oTadil.data.Files.saveAsFileFromDialog(file, oTadil.data.Files.KFileNormasKvExtensionFiltro);

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
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            this.Close();
        }

        #endregion



        #region "Eventos"


        private void frmKv_Load(object sender, EventArgs e)
        {
            traduccion();

            ucMenu.evNew += new EventHandler(ucMenu_evNew);
            ucMenu.evOpen += new EventHandler(ucMenu_evOpen);
            ucMenu.evSaveAs += new EventHandler(ucMenu_evSaveAs);
            ucMenu.evExit += new EventHandler(ucMenu_evExit);


        }




        #endregion




    }
}
