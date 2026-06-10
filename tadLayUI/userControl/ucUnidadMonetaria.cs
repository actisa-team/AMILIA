using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.userControl
{
    
     using tadLayLogica;
    using tadLayLan;
    using tadLayLogica.datos.BaseDatos;
    using tadLayShare;
    using tadLayLogica.datos.proyecto;


   
    public partial class ucUnidadMonetaria : UserControl
    {
        private int mId = 0;
        private BindingSource mBindMaster = null;
        
        
        public ucUnidadMonetaria()
        {
            InitializeComponent();

            postConstructor();
        }


        private void postConstructor()
        {
            #region "Traduccion"

            this.ucUnidadMonetariaSimbolo.uiLbl = strFrmUnidadMonetaria.uiUnidadMonetariaSimbolo;
            this.ucUnidadMonetariaDescripcion.uiLbl = strFrmUnidadMonetaria.uiUnidadMonetariaDescripcion;

            #endregion
            #region " SetUp Objetos"

            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Click += new EventHandler(lnkCancel_Click);

            #endregion
        }

        public void populate()
        {

            #region "Bind"

            //Bind
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbMoneda.TableName;
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbMoneda;

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

            ucUnidadMonetariaSimbolo.textbox.DataBindings.Add("Text", mBindMaster, oSingletonDsBd.getInstance.dataset.tbMoneda.simboloColumn.ColumnName);
            ucUnidadMonetariaDescripcion.textbox.DataBindings.Add("Text", mBindMaster, oSingletonDsBd.getInstance.dataset.tbMoneda.descripcionColumn.ColumnName);

            #endregion
        }


        #region "BOTONES"

        //SAVE
        void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (oValidar.isValidoGrupoByFrm(this))
                {
                    ucUnidadMonetariaDescripcion.textbox.isValido();
                    ((DataRowView)mBindMaster.Current)["id"] = mId;

                    mBindMaster.EndEdit();

                    oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbMoneda, true);
                   // oSingletonDsBd.getInstance.save(true);
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
