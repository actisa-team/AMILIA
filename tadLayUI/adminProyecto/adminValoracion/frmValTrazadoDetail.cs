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
    using tadLayData;
    using tadLayLogica.datos.proyecto;
    using tadLayShare;
    using tadLayLan.Tdi;
   
    
    public partial class frmValTrazadoDetail : frmValoracionDetail
    {

       
        
        public frmValTrazadoDetail()
            :base()
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

        private void frmValTrazadoDetail_Load(object sender, EventArgs e)
        {
            #region "SetUp & Traducccion"

            grValTrazado.Text = strFrmValoracion.uiGrDisVal;

            ucTrazadoPlanta.uiLbl = strFrmValoracion.uiTrazadoPlanta;
            ucTrazadoAlzado.uiLbl = strFrmValoracion.uiTrazadoAlzado;
            ucTiempoRecorrido.uiLbl = strFrmValoracion.uiTiempoRecorrido;
            ucVolumenMovimientoTierras.uiLbl = strFrmValoracion.uiVolumenMovTierras;
            ucCompensacionTierras.uiLbl = strFrmValoracion.uiCompensacionTierras;
            #endregion


            #region "SetUpObjetos"


            ucSaveCancel.lnkSalir.Visible = false;
            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion


            #region "SUMA 100"
            mLstTxtPC100.Add(ucTrazadoPlanta);
            mLstTxtPC100.Add(ucTrazadoAlzado);
            mLstTxtPC100.Add(ucTiempoRecorrido);
            mLstTxtPC100.Add(ucVolumenMovimientoTierras);
            mLstTxtPC100.Add(ucCompensacionTierras);
            #endregion
            #region "DataBind"

            mTabla = ds.dataset.tbValTrazado;

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


            ////Grupo2
            ucTrazadoPlanta.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValTrazado.trazadoPlantaPCColumn.ColumnName, true);
            ucTrazadoAlzado.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValTrazado.trazadoAlzadoPCColumn.ColumnName, true);
            ucTiempoRecorrido.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValTrazado.tiempoRecorridoPCColumn.ColumnName, true);
            ucVolumenMovimientoTierras.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValTrazado.volumenMovTierrasPCColumn.ColumnName, true);
            ucCompensacionTierras.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValTrazado.compensacionTierrasPCColumn.ColumnName, true);

            #endregion
        }



      


    }
}
