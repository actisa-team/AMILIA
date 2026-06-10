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
    using tadLayLogica.Comandos;
    using tadLayLan;
    using tadLayShare;
    
    
    public partial class frmProyectoEstilosVisualizacion : Form
    {

        private string mId = "APP";
        private BindingSource mBindMaster = null;
        
        
        public frmProyectoEstilosVisualizacion()
        {
            InitializeComponent();

            postConstructor();
        }


        private void postConstructor()
        {

            #region "Traduccion"

            grEjeTrazadoEstilos.Text = strFrmEstilosEjePerfil.uiGrEjeEstilos;
            grPerfilEstilos.Text = strFrmEstilosEjePerfil.uiGrPerfilEstilos;


            #endregion

            #region " SetUp Objetos"

            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Click += new EventHandler(lnkCancel_Click);

            ucC3dEjeTrazadoEstilos1.populate();
            ucC3dEjeTrazadoEstilosEtiquetas1.populate();

           // ucC3dEstiloVisualizacionPerfil1.populate();
            ucC3dEstiloVisualizacionGuitarra1.populate();

            ucC3dEstiloPerfilObjeto1.populate();
            ucC3dEstiloPerfilEtiquetas1.populate();

            #endregion

            #region "Bind"

            //Bind
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion.TableName;
            mBindMaster.DataSource = oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion;


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
                throw new oExRowFoundMayorUno("id", "tbEstilosVisualizacion");
            }


            ucC3dEjeTrazadoEstilos1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion.handleEstiloEjeTrazadoObjetoColumn.ColumnName);
            ucC3dEjeTrazadoEstilosEtiquetas1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion.handleEstiloEjeTrazadoEtiquetasColumn.ColumnName);


           // ucC3dEstiloVisualizacionPerfil1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion.handleEstiloVisualizacionPerfilColumn.ColumnName);
            ucC3dEstiloVisualizacionGuitarra1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion.handleEstiloVisualizacionGuitarraColumn.ColumnName);

            ucC3dEstiloPerfilObjeto1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion.handleEstiloPerfilObjetoColumn.ColumnName);

            ucC3dEstiloPerfilEtiquetas1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, oSingletonDsApp.getInstance.dataset.tbEstilosVisualizacion.handleEstiloPerfilEtiquetasColumn.ColumnName);

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
                    
                    ((DataRowView) mBindMaster.Current)["id"] = mId;
                     mBindMaster.EndEdit();
                    oDalTbEstilosVisualizacion.saveTable();
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
