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

    
    public partial class frmValSocioEconomicasDetail : frmValoracionDetail
    {
        public frmValSocioEconomicasDetail()
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

        private void frmValSocioEconomicasDetail_Load(object sender, EventArgs e)
        {

          

            #region "SetUp & Traducccion"

            grVal.Text = strFrmValoracion.uiGrDisVal;

            ucSectorPrimario.uiLbl = strFrmValoracion.uiSectorPrimario;
            ucSectorSecundario.uiLbl = strFrmValoracion.uiSectorSecundario;
            ucSectorTerciario.uiLbl = strFrmValoracion.uiSectorTerciario;


            ucSaveCancel.lnkSalir.Visible = false;
            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion
            #region "SUMA 100"
            mLstTxtPC100.Add(ucSectorPrimario);
            mLstTxtPC100.Add(ucSectorSecundario);
            mLstTxtPC100.Add(ucSectorTerciario);
            #endregion      
            #region "DataBind"

            mTabla = ds.dataset.tbValSocioEconomicas;

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
            ucSectorPrimario.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValSocioEconomicas.sectorPrimarioPCColumn.ColumnName, true);
            ucSectorSecundario.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValSocioEconomicas.sectorSecundarioPCColumn.ColumnName, true);
            ucSectorTerciario.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValSocioEconomicas.sectorTerciarioPCColumn.ColumnName, true);

            #endregion

            #region "Enables Variables From Dwg"

            ucSectorPrimario.Tag = "SECPRI";
            ucSectorSecundario.Tag = "SECSEC";
            ucSectorTerciario.Tag = "SECTER";

            //Activo solo las Variables que tenemos en el DWG
            setUpVariablesFromDwg("SOC");

            //Activo Desactivo el ucSaveCancel

     
 


         
            #endregion


        }





   
    }
}
