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
    
    
    public partial class frmInvTipDetail : frmRoot
    {

        protected string mId = "APP";
        protected BindingSource mBindMaster;


        public frmInvTipDetail()
            : base()    
        {
            InitializeComponent();
            postConstructor();
 
        }

        #region "Metodos Privados"
        private void  postConstructor()
        {
            #region "Traduccion"

            //Grupo
            grInversion.Text = strFrmRentabilidad.uiGrInversion;
            ucIsInversionPrivada.uiLbl = strFrmRentabilidad.uiInvDes;

            //Grupo
            grIIP.Text = strFrmRentabilidad.uiGrIip;
            ucPrecioPeajePorVehiculo.uiLbl = strFrmRentabilidad.uiIipPrecioPeaje;
            ucSubEstatalAnual.uiLbl = strFrmRentabilidad.uiIipSubEstatalAnual;
            ucSubEstatalAnualFija.Text = strFrmRentabilidad.uiIipSubEstatalAnualFija;
            ucSubEstatalUpdateIPC.Text = strFrmRentabilidad.uiIipSubEstatalAnualUpdateIpc;
            ucSubEstatalVehiculo.uiLbl = strFrmRentabilidad.uiIipSubEstatalVehiculo;

            //Grupo Gastos
            grGas.Text = strFrmRentabilidad.uiGrGastosInvPrivada;
            ucGastosExplotacion.uiLbl = strFrmRentabilidad.uiGasExplotacionInvPrivada;
            ucGastosExplotacionManoObraPC.uiLbl = strFrmRentabilidad.uiGastosExplotacionManoObraPC;
            ucGastosSegurosOtros.uiLbl = strFrmRentabilidad.uiGasSegurosInvPrivada;


            //Grupo
            grDPP.Text = strFrmRentabilidad.uiGrDpp;
            ucLicitacionPC.uiLbl = strFrmRentabilidad.uiDppParteLicitación;
            ucExpropiacionPC.uiLbl = strFrmRentabilidad.uiDppParteExpropiaciones;
            ucPatrimonioPC.uiLbl = strFrmRentabilidad.uiDppPartePatrimonio;
            ucControlCalidadPC.uiLbl = strFrmRentabilidad.uiDppParteCalidad;
            ucPaisajisticaPC.uiLbl = strFrmRentabilidad.uiDppPartePaisajisticas;
            ucOtrosPC.uiLbl = strFrmRentabilidad.uiDppParteOtros;



            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            //lnkSalir.Text = strGeneral.uiSalir;

            #endregion
            #region "SetUpObjetos"

            //Eventos
            ucIsInversionPrivada.uiCombo.SelectedIndexChanged += new EventHandler(uiCombo_SelectedIndexChanged);

            #endregion
            #region "BindCreate"


            mBindMaster = new BindingSource();
            mBindMaster.DataMember = ds.dataset.tbInvTip.TableName;
            mBindMaster.DataSource = ds.dataset.tbInvTip;

            dsApp.tbInvTipDataTable miTb = ds.dataset.tbInvTip;

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


            //Grupo1
            ucIsInversionPrivada.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.isInversionPrivadaColumn.ColumnName);

            //Grupo2
            ucPrecioPeajePorVehiculo.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.precioPeajeColumn.ColumnName, true);
            ucSubEstatalVehiculo.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.subvencionEstatalVehiculoColumn.ColumnName, true);
            ucSubEstatalAnual.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.subvencionEstatalAnualColumn.ColumnName, true);
            ucSubEstatalAnualFija.DataBindings.Add("Checked", mBindMaster, miTb.isSubvencionEstatalFijaColumn.ColumnName);
            ucSubEstatalUpdateIPC.DataBindings.Add("Checked", mBindMaster, miTb.isSubvencionEstatalUpdateIpcColumn.ColumnName);

            //Grupo3
            ucGastosExplotacion.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.gastosExplotacionColumn.ColumnName,true);
            ucGastosExplotacionManoObraPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.gastosExplotacionManoObraPCColumn.ColumnName,true);
            ucGastosSegurosOtros.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.gastosSegurosyOtrosColumn.ColumnName,true);

            //Grupo4
            ucLicitacionPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.partePrivadaPresupuestoLicitacionPCColumn.ColumnName, true);
            ucExpropiacionPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.partePrivadaExpropiacionesPCColumn.ColumnName, true);
            ucPatrimonioPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.partePrivadaConservacionPatrimonioPCColumn.ColumnName, true);
            ucControlCalidadPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.partePrivadaControlCalidadPCColumn.ColumnName, true);
            ucPaisajisticaPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.partePrivadaRestauracionPaisajisticaPCColumn.ColumnName, true);
            ucOtrosPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.partePrivadaOtrosPCColumn.ColumnName, true);


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

                    ds.saveDataTable(ds.dataset.tbInvTip, true);

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


        #region "Eventos FRM"

        private void frmInvTipDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            mBindMaster.CancelEdit();
        }

        void uiCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ucIsInversionPrivada.valor)
            {
                setUpGrupos(true);
            }
            else
            {
                setUpGrupos(false);
                valorToNull();
            }
        }
        private void valorToNull()
        {

            ucPrecioPeajePorVehiculo.textbox.valorDoubleNull= null;
            ucSubEstatalVehiculo.textbox.valorDoubleNull=null;
            ucSubEstatalAnual.textbox.valorDoubleNull = null;
            ucSubEstatalAnualFija.Checked=false;
            ucSubEstatalUpdateIPC.Checked=false;

            //Grupo3
            ucGastosExplotacion.textbox.valorDoubleNull = null;
            ucGastosExplotacionManoObraPC.textbox.valorDoubleNull=null;
            ucGastosSegurosOtros.textbox.valorDoubleNull=null;

            //Grupo4
            ucLicitacionPC.textbox.valorDoubleNull=null;
            ucExpropiacionPC.textbox.valorDoubleNull =null;
            ucPatrimonioPC.textbox.valorDoubleNull=null;
            ucControlCalidadPC.textbox.valorDoubleNull =null;
            ucPaisajisticaPC.textbox.valorDoubleNull=null;
            ucOtrosPC.textbox.valorDoubleNull = null;

        }
        private void setUpGrupos(bool iEnable)
        {
            grIIP.Enabled = iEnable;
            grGas.Enabled = iEnable;
            grDPP.Enabled = iEnable;
        }
        #endregion

      

      


 

   
    }
}
