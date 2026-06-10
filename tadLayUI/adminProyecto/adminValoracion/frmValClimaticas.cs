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
    using tadLayLogica;
    using tadLayLan;
    using tadLayShare;
    using tadLayLan.Tdi;
    
    
    public partial class frmValClimaticasDetail : frmValoracionDetail
    {
        public frmValClimaticasDetail()
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

        private void frmValClimaticasDetail_Load(object sender, EventArgs e)
        {

            #region "SetUp & Traducccion"

            grValTrazado.Text = strFrmValoracion.uiGrDisVal;

            ucFuertesHeladas.uiLbl = strFrmValoracion.uiFuertesHeladas;
            ucUmbria.uiLbl = strFrmValoracion.uiUmbria;
            ucTormentasFrecuentes.uiLbl = strFrmValoracion.uiTormentasFrecuentes;
            ucLluviasIntensas.uiLbl = strFrmValoracion.uiLluviasIntensas;

            ucNevada.uiLbl = strFrmValoracion.uiNevadas;
            ucFuertesVientos.uiLbl = strFrmValoracion.uiFuertesVientos;
            ucNieblasDensas.uiLbl = strFrmValoracion.uiNieblasDensas;

            ucSaveCancel.lnkSalir.Visible = false;
            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion
            #region "SUMA 100"
            mLstTxtPC100.Add(ucFuertesHeladas);
            mLstTxtPC100.Add(ucUmbria);
            mLstTxtPC100.Add(ucTormentasFrecuentes);
            mLstTxtPC100.Add(ucLluviasIntensas);

            mLstTxtPC100.Add(ucNevada);
            mLstTxtPC100.Add(ucFuertesVientos);
            mLstTxtPC100.Add(ucNieblasDensas);
            #endregion
            #region "DataBind"

            mTabla = ds.dataset.tbValClimaticas;

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

            ////Bind
            ucFuertesHeladas.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValClimaticas.fuertesHeladasPCColumn.ColumnName, true);
            ucUmbria.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValClimaticas.umbriaPCColumn.ColumnName, true);
            ucTormentasFrecuentes.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValClimaticas.tormentasFrecuentesPCColumn.ColumnName, true);
            ucLluviasIntensas.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValClimaticas.lluviasIntensasPCColumn.ColumnName, true);
            ucNevada.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValClimaticas.nevadaPCColumn.ColumnName, true);
            ucFuertesVientos.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValClimaticas.fuertesVientosPCColumn.ColumnName, true);
            ucNieblasDensas.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValClimaticas.nieblasDensasPCColumn.ColumnName, true);


            #endregion

            #region "Enables Variables From Dwg"

            ucFuertesHeladas.Tag = "ZOFUHE";
            ucUmbria.Tag = "ZONUMB";
            ucTormentasFrecuentes.Tag = "ZONTOR";
            ucLluviasIntensas.Tag = "ZOLLIN";
            ucNevada.Tag = "ZONNEV";
            ucFuertesVientos.Tag = "ZOFUVI";
            ucNieblasDensas.Tag = "ZONIDE";

            //Activo solo las Variables que tenemos en el DWG
            setUpVariablesFromDwg("CLI");

            #endregion
        }
    }
}
