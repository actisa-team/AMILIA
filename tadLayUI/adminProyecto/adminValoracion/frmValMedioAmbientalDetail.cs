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
    
    
    public partial class frmValMedioAmbientalDetail : frmValoracionDetail
    {

        List<string> mLstZonaGisCodeDistintasBydwg = null;
        
        
        public frmValMedioAmbientalDetail()
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

        private void frmValMedioAmbientalDetail_Load(object sender, EventArgs e)
        {
            #region "SetUp & Traducccion"

            grValTrazado.Text = strFrmValoracion.uiGrDisVal;

            ucZonasProteccion.uiLbl = strFrmValoracion.uiZonProteccion;
            ucSuelos.uiLbl = strFrmValoracion.uiSuelos;
            ucFauna.uiLbl = strFrmValoracion.uiFauna;
            ucFlora.uiLbl = strFrmValoracion.uiFlora;
            ucZonDominoPublicoHidraulico.uiLbl = strFrmValoracion.uiDominioPublicoHidraulico;

            ucAcuiferos.uiLbl = strFrmValoracion.uiAcuiferos;
            ucInteresPaisajistico.uiLbl = strFrmValoracion.uiInteresPaisajistico;
            ucCamposVisuales.uiLbl = strFrmValoracion.uiCamposVisuales;
            ucPermeabilidadFauna.uiLbl = strFrmValoracion.uiPermeabilidadFauna;


            ucSaveCancel.lnkSalir.Visible = false;
            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion

            #region "LIST CONTROLES"
            mLstTxtPC100.Add(ucZonasProteccion);
            mLstTxtPC100.Add(ucSuelos);
            mLstTxtPC100.Add(ucFauna);
            mLstTxtPC100.Add(ucFlora);
            mLstTxtPC100.Add(ucZonDominoPublicoHidraulico);

            mLstTxtPC100.Add(ucAcuiferos);
            mLstTxtPC100.Add(ucInteresPaisajistico);
            mLstTxtPC100.Add(ucCamposVisuales);
            mLstTxtPC100.Add(ucPermeabilidadFauna);
            #endregion

            #region "DataBind"

            mTabla = ds.dataset.tbValMedioAmbiental;

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
            ucZonasProteccion.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.zonasProteccionPCColumn.ColumnName, true);
            ucSuelos.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.suelosPCColumn.ColumnName, true);
            ucFauna.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.faunaPCColumn.ColumnName, true);
            ucFlora.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.floraPCColumn.ColumnName, true);
            ucZonDominoPublicoHidraulico.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.zonasDominioPublicoHiraulicoPCColumn.ColumnName, true);

            ucAcuiferos.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.acuiferosPCColumn.ColumnName, true);
            ucInteresPaisajistico.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.interesPaisajisticoPCColumn.ColumnName, true);
            ucCamposVisuales.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.camposVisualesPCColumn.ColumnName, true);
            ucPermeabilidadFauna.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValMedioAmbiental.permeabilidadFaunaPCColumn.ColumnName, true);


            #endregion

            #region "Enables Variables From Dwg"

            ucZonasProteccion.Tag = "DEZOPR";
            ucSuelos.Tag = "VALSUE";
            ucFauna.Tag = "VALFAU";
            ucFlora.Tag = "VALFLO";
            ucZonDominoPublicoHidraulico.Tag = "ZODOPU";
            ucAcuiferos.Tag = "ACUIFE";
            ucInteresPaisajistico.Tag = "ZOINPA";
            ucCamposVisuales.Tag = "CAVIIN";
            ucPermeabilidadFauna.Tag = "PERFAU";

            //Activo solo las Variables que tenemos en el DWG
            setUpVariablesFromDwg("AMB");

            #endregion
        }

    }
}
