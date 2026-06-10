using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI.adminProyecto
{
    using System.IO;
    using System.Globalization;

    using tadLayLan;
    using tadLayData;
    using tadLayLogica.zonaGis;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica;
    using tadLayShare;
    
    
    /// <summary>
    /// EDITAR VALORES PRESUPUESTOS
    /// </summary>
    public partial class frmAppPresupuestoDetail : frmRoot
    {


        private string mId = "APP";
        private BindingSource mBindMaster;

       #region "Constructor"
        public frmAppPresupuestoDetail()
        {
            InitializeComponent();
            post_Constructor();
        }
        #endregion
       #region "Metodos Privados"

        private void post_Constructor()
        {

            #region "Traduccion"

            //Grupo PBL
            grPBL.Text = strFrmPresupuesto.uiPBL;
            ucPBLGastosGenerales.uiLbl = strFrmPresupuesto.uiGastosGenerales;
            ucPBLBeneficioIndustrial.uiLbl = strFrmPresupuesto.uiBeneficioIndustrial;
            ucPBLControlCalidad.uiLbl = strFrmPresupuesto.uiControCalidad;

            //Grupo PCA
            grPCA.Text = strFrmPresupuesto.uiPCA;
            ucPCAControlCalidad.uiLbl = strFrmPresupuesto.uiControCalidad;
            ucPCAConservacionPatrimonio.uiLbl = strFrmPresupuesto.uiConservacionPatrimonio;
            ucPCARestauracionPaisajistica.uiLbl = strFrmPresupuesto.uiRestauracionPaisajistica;
            ucPCAotros.uiLbl = strFrmPresupuesto.uiOtros;
            ucPCAZonaServidumbre.uiLbl = strFrmPresupuesto.uiZonaServidumbre;

            //IVA
            grIva.Text = strFrmPresupuesto.uiGrIVA;
            ucIva.uiLbl = strFrmPresupuesto.uiIVa;


            #endregion
            #region "SetUp Objetos"

            //GuardarSalir
            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Visible = false;

            #endregion


            #region "BindCreate"

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsApp.getInstance.dataset.tbPresupuesto.TableName;
            mBindMaster.DataSource = oSingletonDsApp.getInstance.dataset.tbPresupuesto;



            if (mBindMaster.Count == 0)
            {
                mBindMaster.AddNew();
            }
            else if (mBindMaster.Count == 1)
            {
                string miQuery = "id = '{0}'";
                mBindMaster.Filter = string.Format(miQuery, mId);
            }
            else
            {
                throw new oExRowFoundMayorUno("id", "tbPresupuesto");
            }


            //Tabla Zona Geologica
            dsApp.tbPresupuestoDataTable miTb = oSingletonDsApp.getInstance.dataset.tbPresupuesto;


            //GrupoZona
            ucPBLGastosGenerales.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.pblGastosGeneralesPCColumn.ColumnName, true);
            ucPBLBeneficioIndustrial.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.pblBeneficioIndustrialPCColumn.ColumnName, true);
            ucPBLControlCalidad.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.pblControlCalidadPCColumn.ColumnName, true);

            ucPCAConservacionPatrimonio.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.pcaConservacionPatrimonioPCColumn.ColumnName, true);
            ucPCAControlCalidad.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.pcaControlCalidadPCColumn.ColumnName, true);
            ucPCARestauracionPaisajistica.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.pcaRestauracionPaisajisticaPCColumn.ColumnName, true);
            ucPCAotros.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.pcaOtroPCColumn.ColumnName, true);
            ucPCAZonaServidumbre.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.pcaZonaServidumbreColumn.ColumnName, true);
            ucIva.textbox.DataBindings.Add("ValorDoubleNull", mBindMaster, miTb.IvaPCColumn.ColumnName, true);


            #endregion



        }

 
 
       #endregion
       #region "Botones"
        void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isValidoFrm)
                {
                    ((DataRowView)mBindMaster.Current)["id"] = mId;
                    mBindMaster.EndEdit();
                    oDalPresupuesto.saveTable();

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
        void lnkCancel_Click(object sender, EventArgs e)
        {
            try
            {
                mBindMaster.CancelEdit();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
       #endregion


    }
}
