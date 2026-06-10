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
    
    /// <summary>
    /// DATOS TRAFICO Y CONEXION ACTUAL
    /// </summary>
    public partial class frmVehManDetail : frmRoot
    {

        private string mId = "APP";
        private string mIdVehiculoTipo = string.Empty;
        private BindingSource mBindMaster;

        /// <summary>
        /// DATOS TRAFICO Y CONEXION ACTUAL
        /// </summary>
        public frmVehManDetail()
            : base()    
        {
            InitializeComponent();
            postConstructor();
          
        }

        #region "Metodos Privados"
        private void  postConstructor()
        {

            //Traduccion
            grTipoVehiculo.Text = strFrmRentabilidad.uiVehiculoTipo;
            grDgvMaster.Text = strFrmRentabilidad.uiGrTablaConsumo;
            grDgvDetail.Text = strFrmRentabilidad.uiGrDetalle;
            ucCosteMantenimiento.uiLbl = strFrmRentabilidad.uiCosMantenimiento;

            //Conexion
            bindCreate();
            
            //Enable
            grDgvMaster.Enabled = true;
            grDgvDetail.Enabled = false;

            ucToolDetail1.lnkSalir.Visible = false;

           //Eventos
            ucVehiculoTipo1.uiCombo.SelectedIndexChanged += new EventHandler(uiCombo_SelectedIndexChanged);
            ucToolDgv1.lnkEdit.Click += new EventHandler(lnkEdit_Click);
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Click += new EventHandler(lnkCancel_Click);
            uiCombo_SelectedIndexChanged(ucVehiculoTipo1.uiCombo, new EventArgs());

            //Ojo Si pone antes del lanzar el Evento Combo No trabaja ¿?¿?
            dgvSetUp();
          
        }
        private void bindCreate()
        {

            //Si no Existen Valores de Consumo Obtengo los Defecto
            oDalVehiculos.addMantenimientoDefault();

            //DataBind
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = ds.dataset.tbVehMan.TableName;
            mBindMaster.DataSource = ds.dataset.tbVehMan;
      
            CultureInfo miCulInfo = CultureInfo.InvariantCulture;

            ucCosteMantenimiento.textbox.DataBindings.Add("Text", mBindMaster, "costeMantenimiento", true, DataSourceUpdateMode.OnPropertyChanged, null, "0.0000", miCulInfo);
  
        }
        private void dgvSetUp()
        {
            ucDgvMaster.DataSource = mBindMaster;
            ucDgvMaster.dgvSetUpUIDefault(true);
            ucDgvMaster.CellDoubleClick += new DataGridViewCellEventHandler(ucDgvMaster_CellDoubleClick);
            ucDgvMaster.dgvColumnsHide(new int[] { 0, 3 });

            ucDgvMaster.Columns[1].HeaderText = strFrmRentabilidad.uiVelocidad;
            ucDgvMaster.Columns[2].HeaderText = strFrmRentabilidad.uiGastosMantenimiento;
      
        }
        private void grMasterEnable(bool iEstado)
        {
            grDgvMaster.Enabled = iEstado;
            ucDgvMaster.Enabled = iEstado;
            grDgvDetail.Enabled = !iEstado;
        }
       #endregion
        #region "BOTON MASTER"
        private void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ucCosteMantenimiento.textbox.Focus();
                grMasterEnable(false);
            }
            catch (Exception ex)
            {
               oTadil.data.UserInfo.showError(ex);
            }
        }
        #endregion
        #region "BOTON DETAIL"
        //SAVE
        private void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isValidoFrm)
                {
                    mBindMaster.EndEdit();

                    ds.saveDataTable(ds.dataset.tbVehMan, true);

                    grMasterEnable(true);
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
        //CANCEL
        private void lnkCancel_Click(object sender, EventArgs e)
        {

            try
            {
                mBindMaster.ResetCurrentItem();
                grMasterEnable(true);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        #endregion
        #region "FRM EVENTOS"
        private void frmConVehDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            mBindMaster.Dispose();
        }
       private void uiCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            grMasterEnable(true);

            string miQuery = "idVehiculoTipo = '{0}'";

            mBindMaster.Filter = string.Format(miQuery, ucVehiculoTipo1.valor);

            mBindMaster.MoveFirst();
        }
       private void ucDgvMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lnkEdit_Click(null, new EventArgs());
        }
        #endregion


    }
}
