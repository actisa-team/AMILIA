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
    using tadLayLan.Tdi;
    
    
    public partial class frmMatrizDecisionDetail : frmRoot
    {

        private int? mIdHipotesis = null;

        private BindingSource mBindDetail;
        
        
        public frmMatrizDecisionDetail(BindingSource iBinding)
        {
            InitializeComponent();

            mBindDetail = iBinding;

            postConstructor();
        }


        public void itemEdit (int iIdHipotesis)
        {
            mIdHipotesis = iIdHipotesis;
        }

        public void itemNew ()
        {    
            mBindDetail.AddNew();
        }



        private void postConstructor()
        {

            //Traduccion
            this.Text = strFrmValoracion.uiFrmMatrizDecisionDetail;

            this.grDatos.Text = strFrmValoracion.uiGrHipotesisDatos;

            ucNombre.uiLbl = strFrmValoracion.uiDgvHipotesis;
            ucDescripcion.uiLbl = strFrmValoracion.uiDgvDescripcion;


            this.grAreasValoracion.Text = strFrmValoracion.uiGrHipotesisValoraciones;

            ucTrazadoPC.uiLbl = strFrmValoracion.uiDgvTrazado;
            ucGeotecniaPC.uiLbl = strFrmValoracion.uiDgvGeotecnia;
            ucEstructurasTunelesMurosPC.uiLbl = strFrmValoracion.uiDgvEstructurasTunelesMuros;
            ucMedioAmbientalPC.uiLbl = strFrmValoracion.uiDgvMedioambiental;
            ucClimaticaPC.uiLbl = strFrmValoracion.uiDgvClimatica;
            ucSocioEconomicasPC.uiLbl = strFrmValoracion.uiDgvSocioEconomicas;
            ucPatrimonialesPC.uiLbl = strFrmValoracion.uiDgvPatrimoniales;
            ucEconomicaPC.uiLbl = strFrmValoracion.uiDgvEconomicas;


            //Binding
            ucNombre.textbox.DataBindings.Add("Text", mBindDetail, ds.dataset.tbMatrizDecision.nombreColumn.ColumnName, true);
            ucDescripcion.textbox.DataBindings.Add("Text", mBindDetail, ds.dataset.tbMatrizDecision.descripcionColumn.ColumnName, true);
            ucTrazadoPC.textbox.DataBindings.Add("valorDouble", mBindDetail, ds.dataset.tbMatrizDecision.valoracionTrazadoPCColumn.ColumnName, true);
            ucGeotecniaPC.textbox.DataBindings.Add("valorDouble", mBindDetail, ds.dataset.tbMatrizDecision.valoracionGeotecniaPCColumn.ColumnName, true);
            ucEstructurasTunelesMurosPC.textbox.DataBindings.Add("valorDouble", mBindDetail, ds.dataset.tbMatrizDecision.valoracionEstructurasTunelesMurosPCColumn.ColumnName, true);
            ucMedioAmbientalPC.textbox.DataBindings.Add("valorDouble",mBindDetail,ds.dataset.tbMatrizDecision.valoracionMedioAmbientalPCColumn.ColumnName, true);
            ucClimaticaPC.textbox.DataBindings.Add("valorDouble", mBindDetail, ds.dataset.tbMatrizDecision.valoracionClimaticasPCColumn.ColumnName, true);
            ucSocioEconomicasPC.textbox.DataBindings.Add("valorDouble", mBindDetail, ds.dataset.tbMatrizDecision.valoracionSocioEconomicasPCColumn.ColumnName, true);
            ucPatrimonialesPC.textbox.DataBindings.Add("valorDouble", mBindDetail, ds.dataset.tbMatrizDecision.valoracionPatrimonialesPCColumn.ColumnName, true);
            ucEconomicaPC.textbox.DataBindings.Add("valorDouble", mBindDetail, ds.dataset.tbMatrizDecision.valoracionEconomicaPCColumn.ColumnName, true);


            //BarraHerramientas
            ucToolDetail1.lnkSave.Visible = true;
            ucToolDetail1.lnkCancel.Visible = false;
            ucToolDetail1.lnkSalir.Visible = false;

            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkSalir.Click += new EventHandler(lnkSalir_Click);

            //ListadoControlesSuma100
            mLstTxtPC100.Add(ucTrazadoPC);
            mLstTxtPC100.Add(ucGeotecniaPC);
            mLstTxtPC100.Add(ucEstructurasTunelesMurosPC);
            mLstTxtPC100.Add(ucMedioAmbientalPC);
            mLstTxtPC100.Add(ucClimaticaPC);
            mLstTxtPC100.Add(ucSocioEconomicasPC);
            mLstTxtPC100.Add(ucPatrimonialesPC);
            mLstTxtPC100.Add(ucEconomicaPC);
                    
        }






        #region "Botones"

        void lnkSave_Click(object sender, EventArgs e)
        {

            if (base.isValidoFrm)
            {

                string miError100 = string.Empty;

                if (suma100(ref miError100))
                {
                    mBindDetail.EndEdit();

                    ds.saveDataTable(ds.dataset.tbMatrizDecision, true);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(string.Format(strGeneralUser.uiSumaPorcentajes, miError100));
                }
                               
            }
            else
            { 
               
                oTadil.data.UserInfo.showInfo(strError.eValidacion);
            }

        }


        void lnkSalir_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }


        #endregion



    }
}
