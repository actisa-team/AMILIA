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
    
    public partial class frmValEstTunDetail : frmValoracionDetail
    {
        public frmValEstTunDetail()
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

        private void frmValEstTunDetail_Load(object sender, EventArgs e)
        {
            #region "SetUp & Traducccion"

            grValGeoEst.Text = strFrmValoracion.uiGrValGeoEst;
            ucCimentacionEstructuras.uiLbl = strFrmValoracion.uiCimEst;
            ucProcedimientoExcavacion.uiLbl = strFrmValoracion.uiProExcavacionCimientos;
            ucCimentacionObrasMenores.uiLbl = strFrmValoracion.uiCimObrasMenores;
            ucPresenciaAgua.uiLbl = strFrmValoracion.uiPresenciaAgua;

            grValGeoTun.Text = strFrmValoracion.uiGrValGeoTun;
            ucRmr.uiLbl = strFrmValoracion.uiRmr;
            ucMetodosExcavacion.uiLbl = strFrmValoracion.uiMetodosExcavacion;
            ucTratamientosEspecificos.uiLbl = strFrmValoracion.uiTratamientosEspecificos;

            grValGlobal.Text = strFrmValoracion.uiGrValGlobal;
            ucGeotecniaEstructuras.uiLbl = strFrmValoracion.uiGeoEst;
            ucGeotecniaTuneles.uiLbl = strFrmValoracion.uiGeoTun;
            ucMuros.uiLbl = strFrmValoracion.uiMuros;

            ucSaveCancel.lnkSalir.Visible = false;
            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion
            #region "SUMA 100"
            mLstTxtPC100.Add(ucCimentacionEstructuras);
            mLstTxtPC100.Add(ucProcedimientoExcavacion);
            mLstTxtPC100.Add(ucCimentacionObrasMenores);
            mLstTxtPC100.Add(ucPresenciaAgua);
            mLstTxtPC100.Add(ucRmr);
            mLstTxtPC100.Add(ucMetodosExcavacion);
            mLstTxtPC100.Add(ucTratamientosEspecificos);
            mLstTxtPC100.Add(ucGeotecniaEstructuras);
            mLstTxtPC100.Add(ucGeotecniaTuneles);
            mLstTxtPC100.Add(ucMuros);
            #endregion
            #region "DataBind"

            mTabla = ds.dataset.tbValEstructurasTuneles;

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
            ucCimentacionEstructuras.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.estCimentacionEstructurasPCColumn.ColumnName, true);
            ucProcedimientoExcavacion.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.estProcedimientosExcavacionCimientosPCColumn.ColumnName, true);
            ucCimentacionObrasMenores.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.estCimentacionObrasMenoresPCColumn.ColumnName, true);
            ucPresenciaAgua.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.estPresenciaAguaPCColumn.ColumnName, true);

            ucRmr.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.tunRmrPCColumn.ColumnName, true);
            ucMetodosExcavacion.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.tunMetodosExcavacionPCColumn.ColumnName, true);
            ucTratamientosEspecificos.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.tunTratamientosEspecificosPCColumn.ColumnName, true);

            ucGeotecniaEstructuras.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.gloEstructurasGeotecniaPCColumn.ColumnName, true);
            ucGeotecniaTuneles.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.gloTunelesGeotecniaPCColumn.ColumnName, true);
            ucMuros.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValEstructurasTuneles.gloMurosPCColumn.ColumnName, true);

            #endregion
        }
    }
}
