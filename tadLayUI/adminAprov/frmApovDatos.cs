using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tadLayData;
using tadLayLan;
using tadLayLogica;
using tadLayLogica.datos.BaseDatos;
using tadLayUI.adminGis;

namespace tadLayUI.adminAprov
{
    public partial class frmApovDatos : Form, IfrmManagerPopulate
    {
        private object[] mItemCurrentIni;
        private BindingSource mBindMaster;
        private Guid? mIdApov = null;
        private bool mUpdatingCheck = false;
        public frmApovDatos()
        {

            InitializeComponent();
            this.Load += new EventHandler(frmApovDatos_Load);
            lnkSave.Text = "Guardar";
            lnkSalir.Text = "Salir";

            chfirme.CheckedChanged += new EventHandler(chfirme_CheckedChanged);
            chsaneo.CheckedChanged += new EventHandler(chsaneo_CheckedChanged);
        }

        private void frmApovDatos_Load(object sender, EventArgs e)
        {
            try
            {
                bindCreate();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }

        private void bindCreate()
        {
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbAdecuacion.TableName;
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset;

            if (isNew)
            {
                mBindMaster.AddNew();
            }
            else
            {
                string miQuery = "id = '{0}'";
                mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mIdApov));
            }

            mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;

            dsBd.tbAdecuacionDataTable miTb = oSingletonDsBd.getInstance.dataset.tbAdecuacion;

            ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
            ucEspesor.textbox.DataBindings.Add("Text", mBindMaster, miTb.espesorfirmeColumn.ColumnName);
            ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);
            ucColor.panel.DataBindings.Add("colorInt", mBindMaster, miTb.colorColumn.ColumnName, false, DataSourceUpdateMode.OnPropertyChanged);
            chfirme.DataBindings.Add("Checked", mBindMaster, miTb.firmeColumn.ColumnName);
            chsaneo.DataBindings.Add("Checked", mBindMaster, miTb.saneoColumn.ColumnName);


        }

        /// <summary>
        /// ZONA NUEVA
        /// </summary>
        public void create()
        {
            mIdApov = null;
            ucNombre.uiLbl = "Nombre";
            ucEspesor.uiLbl = "Espesor (cm)";
            ucDescripcion.uiLbl = "Descripción";
            ucColor.uiLbl = "Color";
            chfirme.Text = "Firme";
            chsaneo.Text = "Saneo";
        }
        /// <summary>
        /// ZONA EDITAR
        /// </summary>
        public void edit(Guid iIdAprov)
        {
            mIdApov = iIdAprov;
            ucNombre.uiLbl = "Nombre";
            ucEspesor.uiLbl = "Espesor (cm)";
            ucDescripcion.uiLbl = "Descripción";
            ucColor.uiLbl = "Color";
            chfirme.Text = "Firme";
            chsaneo.Text = "Saneo";
        }

        public void populate()
        {
            if (isNew)
            {
                create();
            }
        }

        public bool allowClosed()
        {
            return true;
        }

        //Es un Registro Nuevo (False estoy en edición)
        private bool isNew
        {
            get
            {
                if (mIdApov == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        private void lnkSalir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }

        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                if (isNew)
                {
                    mIdApov = System.Guid.NewGuid();
                    ((DataRowView)mBindMaster.Current)["id"] = mIdApov.ToString();
                }

                mBindMaster.EndEdit();


                mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


                oSingletonDsBd.getInstance.save(true);
                oSingletonDsBd.getInstance.Dispose();

                //((DataSet)mBindMaster.DataSource).WriteXml(oTadil.data.Files.fileBbdd);

                //oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosSaveIsOk);

                bindUpdate();
                this.Close();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        private void bindUpdate()
        {
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset;
            mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;

        }

        private void chfirme_CheckedChanged(object sender, EventArgs e)
        {
            if (mUpdatingCheck) return;
            if (chfirme.Checked)
            {
                mUpdatingCheck = true;
                chsaneo.Checked = false;
                mUpdatingCheck = false;
            }
        }

        private void chsaneo_CheckedChanged(object sender, EventArgs e)
        {
            if (mUpdatingCheck) return;
            if (chsaneo.Checked)
            {
                mUpdatingCheck = true;
                chfirme.Checked = false;
                mUpdatingCheck = false;
            }
        }
    }
}
