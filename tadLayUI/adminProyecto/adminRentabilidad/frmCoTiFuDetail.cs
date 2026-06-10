using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI.adminRentabilidad
{
    using System.IO;
    using System.Globalization;


    using tadLayUI;

    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica;
    using tadLayShare;
    
    /// <summary>
    /// COSTES TIEMPO Y FUNCIONAMIENTO
    /// </summary>
    public partial class frmCoTiFuDetail : frmRoot
    {

        private string mId = "APP";
        private BindingSource mBindMaster;

        /// <summary>
        /// COSTES TIEMPO Y FUNCIONAMIENTO
        /// </summary>
        public frmCoTiFuDetail()
            : base()    
        {
            InitializeComponent();
            postConstructor();
           
        }

       #region "Metodos Privados"
        private void  postConstructor()
        {

            #region "Traduccion"

            //Grupo Combstible
            grCostesComLub.Text = strFrmRentabilidad.uiGrCosteCombustibleLubricante;
            ucCosteCombustible.uiLbl = strFrmRentabilidad.uiCosCombustible;
            ucCosteLubricante.uiLbl = strFrmRentabilidad.uiCosAceite;

            //Grupo VL
            grCosteVehiculoLigero.Text = strFrmRentabilidad.uiVehiculoLigero;
            ucCoePonderacionCostesTiempoVL.uiLbl = strFrmRentabilidad.uiCosPonderacionTiempo;
            ucCosteTiempoVL.uiLbl = strFrmRentabilidad.uiCosVehiculoLigeros;
            ucCosteNeumaticoVL.uiLbl = strFrmRentabilidad.uiCosNeumatico;
            ucCosteAmortizacionesVL.uiLbl = strFrmRentabilidad.uiCosAmortizaciones;

            //Grupo VL
            grCosteVehiculoPesado.Text = strFrmRentabilidad.uiVehiculoPesado;
            ucCoePonderacionCostesTiempoVP.uiLbl = strFrmRentabilidad.uiCosPonderacionTiempo;
            ucCosteTiempoVP.uiLbl = strFrmRentabilidad.uiCosVehiculoPesado;
            ucCosteNeumaticoVP.uiLbl = strFrmRentabilidad.uiCosNeumatico;
            ucCosteAmortizacionesVP.uiLbl = strFrmRentabilidad.uiCosAmortizaciones;


            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            //lnkSalir.Text = strGeneral.uiSalir;


            #endregion

            #region "BindCreate"

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = ds.dataset.tbCoTiFu.TableName;
            mBindMaster.DataSource = ds.dataset.tbCoTiFu;


            dsApp.tbCoTiFuDataTable miTb = ds.dataset.tbCoTiFu;

            if (miTb.Rows.Count == 0)
            {
                mBindMaster.AddNew();
            }
            else if (miTb.Rows.Count == 1)
            {
                string miQuery = "id = '{0}'";
                mBindMaster.Filter = string.Format(miQuery, mId);
            }
            else
            {
                throw new oExRowFoundMayorUno("id", miTb.TableName);
            }


            //Costes
            ucCosteCombustible.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeCombustibleColumn.ColumnName, true);
            ucCosteLubricante.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeLubricanteColumn.ColumnName, true);
            //VL
            ucCoePonderacionCostesTiempoVL.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.coePonderacionTiempoVehiculoLigeroColumn.ColumnName, true);
            ucCosteTiempoVL.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeTiempoVehiculoLigeroColumn.ColumnName, true);
            ucCosteNeumaticoVL.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeNeumaticoVehiculoLigeroColumn.ColumnName, true);
            ucCosteAmortizacionesVL.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeAmortizacionesVehiculoLigeroColumn.ColumnName, true);
            //VP
            ucCoePonderacionCostesTiempoVP.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.coePonderacionTiempoVehiculoPesadoColumn.ColumnName, true);
            ucCosteTiempoVP.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeTiempoVehiculoPesadoColumn.ColumnName, true);
            ucCosteNeumaticoVP.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeNeumaticoVehiculoPesadoColumn.ColumnName, true);
            ucCosteAmortizacionesVP.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeAmortizacionesVehiculoPesadoColumn.ColumnName, true);

            #endregion

        }



       #endregion

       #region "Botones"

        //GUARDAR
        protected virtual void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (isValidoFrm)
                {
                    ((DataRowView)mBindMaster.Current)["id"] = mId;

                    mBindMaster.EndEdit();

                    ds.saveDataTable(ds.dataset.tbCoTiFu, true);


                    //PRUEBA
                    OnControlRemoved(new ControlEventArgs(new Control()));
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }



        }
        //SALIR
        protected virtual void lnkSalir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
        #endregion

        private void frmCoTiFuDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            mBindMaster.CancelEdit();
        }
    }
}
