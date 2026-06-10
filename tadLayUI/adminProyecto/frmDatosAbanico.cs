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

    using tadLayLogica;
    using tadLayLogica.datos.proyecto;
    using tadLayLan;
    
    
    public partial class frmDatosAbanico : Form
    {

        private string mId = "APP";
        private BindingSource mBindMaster = null;
        
        public frmDatosAbanico()
        {
            InitializeComponent();

            postConstructor();
        }



        private void postConstructor()
        {

            #region "Traduccion"

            grDatosAbanico.Text = strFrmDatosAbanico.uiGrAbaGeometria;
            ucAbanicoAnguloTotal.uiLbl = strFrmDatosAbanico.uiAnguloTotal;
            ucAbanicoDiscretizacionGrados.uiLbl = strFrmDatosAbanico.uiAnguloDiscretizacion;

            #endregion

            #region " SetUp Objetos"

            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Click += new EventHandler(lnkCancel_Click);

            #endregion

            #region "Bind"

            //Bind
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsApp.getInstance.dataset.tbSolucionAbanico.TableName;
            mBindMaster.DataSource = oSingletonDsApp.getInstance.dataset.tbSolucion;

            string miQuery = "id = '{0}' ";

            mBindMaster.Filter = string.Format(miQuery, mId);

            ucAbanicoAnguloTotal.textbox.DataBindings.Add("valorDouble", mBindMaster, oSingletonDsApp.getInstance.dataset.tbSolucionAbanico.anguloTotalGradosColumn.ColumnName);
            ucAbanicoDiscretizacionGrados.textbox.DataBindings.Add("valorDouble", mBindMaster, oSingletonDsApp.getInstance.dataset.tbSolucionAbanico.anguloDiscretizacionGradosColumn.ColumnName);

            #endregion

        }


        #region "Botones"
        //SAVE
        void lnkSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (oValidar.isValidoGrupoByFrm(this))
                {
                    mBindMaster.EndEdit();
                    oDalAbanico.saveTable();
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
