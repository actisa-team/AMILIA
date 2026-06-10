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
    using tadLayShare;
    
    public partial class frmDatosProyecto : Form
    {

        private eEstudioTipo? mEstudioTipo = null; 
        private string mId = "APP";
        private BindingSource mBindMaster = null;
        
        
        
        
        public frmDatosProyecto(eEstudioTipo iEstudioTipo)
        {
            InitializeComponent();

            mEstudioTipo = iEstudioTipo;

            postConstructor();
        }


        private void postConstructor ()
        {

            #region "Traduccion"

            grDatosGenerales.Text = strFrmDatosProyecto.uiGrNombreDescripcion;
            ucNombre.uiLbl = strGeneral.uiNombre;
            ucDescripcion.uiLbl = strGeneral.uiDescripcion;
            grTipoProyecto.Text = strFrmDatosProyecto.uiTipoEstudio;

            grSeccionTransversalSeparacion.Text = strFrmDatosProyecto.uiGrSecciones;
            ucSeccionTransversalSeparacionMetros.uiLbl = strFrmDatosProyecto.uiSeccionIntervalo;

            #endregion
            #region " SetUp Objetos"

            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Click += new EventHandler(lnkCancel_Click);

            ucProyectoTipo1.populate();


            if (mEstudioTipo.Value == eEstudioTipo.ESTPRE)
            {
                grSeccionTransversalSeparacion.Visible = false;
                grSeccionTransversalSeparacion.Enabled = false;
                ucToolDetail1.Location = new Point(10, 260);
            }
            else if (mEstudioTipo.Value == eEstudioTipo.ESTINF)
            {
                grSeccionTransversalSeparacion.Visible = true;
                grSeccionTransversalSeparacion.Enabled = true;
                ucToolDetail1.Location = new Point(10, 380);
            }
            else
            {
                throw new oExEnumNotImplemented(mEstudioTipo.Value.ToString());
            }

            #endregion

            #region "Bind"

            //Bind
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsApp.getInstance.dataset.tbProyecto.TableName;
            mBindMaster.DataSource = oSingletonDsApp.getInstance.dataset.tbProyecto;

            string miQuery = "id = '{0}' ";

            mBindMaster.Filter = string.Format(miQuery, mId);

            ucNombre.textbox.DataBindings.Add("Text", mBindMaster, oSingletonDsApp.getInstance.dataset.tbProyecto.nombreColumn.ColumnName);
            ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, oSingletonDsApp.getInstance.dataset.tbProyecto.descripcionColumn.ColumnName);
            ucProyectoTipo1.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, oSingletonDsApp.getInstance.dataset.tbProyecto.idProyectoTipoColumn.ColumnName);


            if (mEstudioTipo.Value == eEstudioTipo.ESTINF)
            {
                ucSeccionTransversalSeparacionMetros.textbox.DataBindings.Add("valorDoubleNull",mBindMaster,oSingletonDsApp.getInstance.dataset.tbProyecto.seccionIntervaloMetrosColumn.ColumnName,true);
            }


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
                    oDalTbProyecto.saveTable(true);
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
