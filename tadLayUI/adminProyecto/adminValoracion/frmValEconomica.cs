using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminValoracion
{

    using tadLayLan;
    using tadLayLogica;
    using engNet.ClassT;
    using tadLayShare;
    using tadLayLan.Tdi;
    
    
    public partial class frmValEconomicaDetail:frmValoracionDetail    
    {

        private oValT<bool?> mIsInversionPrivada = new oValT<bool?>();
        
        

        public frmValEconomicaDetail()     
        {
            InitializeComponent();
        }

        #region "Botones"

        //SAVE
        void lnkSave_Click(object sender, EventArgs e)
        {
            base.save();
            //PRUEBA
            OnControlRemoved(new ControlEventArgs(new Control()));
        }

        //CANCEL
        void lnkCancel_Click(object sender, EventArgs e)
        {
            base.cancel();
        }



        #endregion

        private void frmValEconomicaDetail_Load(object sender, EventArgs e)
        {
            #region "Variables FRM"
            mIsInversionPrivada.val = base.ds.isInversionPrivada;
            #endregion
            #region "SetUp & Traducccion"

            grValTrazado.Text = strFrmValoracion.uiGrDisVal;

            ucPublicaPrespupuesto.uiLbl = strFrmValoracion.uiPublicoPresupuesto;
            ucPublicaVAN.uiLbl = strFrmValoracion.uiPublicoVAN;
            ucPublicaBC.uiLbl = strFrmValoracion.uiPublicoBC;

            ucPrivadaPresupuesto.uiLbl = strFrmValoracion.uiPrivadaPresupuesto;
            ucPrivadaVAN.uiLbl = strFrmValoracion.uiPrivadaVAN;
            ucPrivadaBC.uiLbl = strFrmValoracion.uiPrivadaBC;

            ucSaveCancel.lnkSalir.Visible = false;
            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion
            #region "SUMA 100"
            mLstTxtPC100.Add(ucPublicaPrespupuesto);
            mLstTxtPC100.Add(ucPublicaVAN);
            mLstTxtPC100.Add(ucPublicaBC);
            mLstTxtPC100.Add(ucPrivadaPresupuesto);
            mLstTxtPC100.Add(ucPrivadaVAN);
            mLstTxtPC100.Add(ucPrivadaBC);
            #endregion
            #region "DataBind"

            mTabla = ds.dataset.tbValEconomica;

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = mTabla.TableName;
            mBindMaster.DataSource = mTabla;

            if (mTabla.Rows.Count == 0)
            {
                mBindMaster.AddNew();
            }
            else if (mTabla.Rows.Count == 1)
            {
                string miQuery = "id = '{0}'";
                mBindMaster.Filter = string.Format(miQuery, mId);
            }
            else
            {
                throw new oExRowFoundMayorUno("id", mTabla.TableName);
            }


            ucPrivadaPresupuesto.DataBindings.Add("Enabled", mIsInversionPrivada, "val");
            ucPrivadaVAN.DataBindings.Add("Enabled", mIsInversionPrivada, "val");
            ucPrivadaBC.DataBindings.Add("Enabled", mIsInversionPrivada, "val");


            ////Bind Datos
            ucPublicaPrespupuesto.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEconomica.publicaPresupuestoPCColumn.ColumnName, true);
            ucPublicaVAN.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEconomica.publicaVanPCColumn.ColumnName, true);
            ucPublicaBC.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEconomica.publicaBcPCColumn.ColumnName, true);

            ucPrivadaPresupuesto.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEconomica.privadaPresupuestoPCColumn.ColumnName, true);
            ucPrivadaVAN.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEconomica.privadaVanPCColumn.ColumnName, true);
            ucPrivadaBC.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEconomica.privadaBcPCColumn.ColumnName, true);





            #endregion
        }

    }
}
