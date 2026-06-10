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
    
    
    public partial class frmValPatrimonialesDetail : frmValoracionDetail
    {
        public frmValPatrimonialesDetail()
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

        private void frmValPatrimonialesDetail_Load(object sender, EventArgs e)
        {
            #region "SetUp & Traducccion"

            grValTrazado.Text = strFrmValoracion.uiGrDisVal;

            ucMontesPublicos.uiLbl = strFrmValoracion.uiMontesPublicos;
            ucSuelosUrbanos.uiLbl = strFrmValoracion.uiSuelosUrbanos;
            ucSuelosUrbanizables.uiLbl = strFrmValoracion.uiSuelosUrbanizables;
            ucSuelosNoUrbanizables.uiLbl = strFrmValoracion.uiSuelosNoUrbanizables;
            ucCruceViasPecuarias.uiLbl = strFrmValoracion.uiCruceViasPecuarias;

            ucYacimientosArqueologicos.uiLbl = strFrmValoracion.uiYacimientosArqueologicos;
            ucZonasEspecialInteres.uiLbl = strFrmValoracion.uiZonasEspecialInteres;
            ucCruceInfraestructurasLineales.uiLbl = strFrmValoracion.uiCruceInfra;
            ucZonasInfraEstructurasPublicas.uiLbl = strFrmValoracion.uiZonasInfraPublicas;
            ucZonasMinasCanteras.uiLbl = strFrmValoracion.uiZonasMinasCanteras;

            ucSaveCancel.lnkSalir.Visible = false;
            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion
            #region "LIST CONTROLES"
            mLstTxtPC100.Add(this.ucMontesPublicos);

            mLstTxtPC100.Add(this.ucSuelosUrbanos);
            mLstTxtPC100.Add(this.ucSuelosUrbanizables);
            mLstTxtPC100.Add(this.ucSuelosNoUrbanizables);

            mLstTxtPC100.Add(this.ucCruceViasPecuarias);
            mLstTxtPC100.Add(this.ucYacimientosArqueologicos);
            mLstTxtPC100.Add(this.ucZonasEspecialInteres);

            mLstTxtPC100.Add(this.ucCruceInfraestructurasLineales);
            mLstTxtPC100.Add(this.ucZonasInfraEstructurasPublicas);
            mLstTxtPC100.Add(this.ucZonasMinasCanteras);
 
            #endregion  
            #region "DataBind"

            mTabla = ds.dataset.tbValPatrimoniales;

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
            ucMontesPublicos.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.montesPublicosPCColumn.ColumnName, true);
            ucSuelosUrbanos.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.suelosUrbanosPCColumn.ColumnName, true);
            ucSuelosUrbanizables.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.suelosUrbanizablesPCColumn.ColumnName, true);
            ucSuelosNoUrbanizables.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.suelosNoUrbanizablesPCColumn.ColumnName, true);
            ucCruceViasPecuarias.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.cruceViasPecuariasPCColumn.ColumnName, true);

            ucYacimientosArqueologicos.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.yacimientosArqueologicosPCColumn.ColumnName, true);
            ucZonasEspecialInteres.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.zonasEspecialInteresPCColumn.ColumnName, true);
            ucCruceInfraestructurasLineales.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.cruceInfraEstructurasLinealesPCColumn.ColumnName, true);
            ucZonasInfraEstructurasPublicas.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.zonasInfraPublicasPCColumn.ColumnName, true);
            ucZonasMinasCanteras.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValPatrimoniales.zonasMinasCanterasPCColumn.ColumnName, true);

            #endregion
            #region "Enables Variables From Dwg"

            ucMontesPublicos.Tag = "MONPUB";
            ucSuelosUrbanos.Tag = "URBANO";
            ucSuelosUrbanizables.Tag = "URBANI";
            ucSuelosNoUrbanizables.Tag = "NOURBA";
            ucCruceViasPecuarias.Tag = "CRVIPE";

            ucYacimientosArqueologicos.Tag = "YACARQ";
            ucZonasEspecialInteres.Tag = "ZOESIN";
            ucCruceInfraestructurasLineales.Tag = "CRUINF";
            ucZonasInfraEstructurasPublicas.Tag = "OCUINF";
            ucZonasMinasCanteras.Tag = "OCUMIN";

            //Activo solo las Variables que tenemos en el DWG
            setUpVariablesFromDwg("PAT");


            #endregion
        }
    }
}
