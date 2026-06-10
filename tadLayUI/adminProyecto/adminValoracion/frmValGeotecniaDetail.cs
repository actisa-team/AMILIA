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
    
    
    public partial class frmValGeotecniaDetail : frmValoracionDetail
    {
        public frmValGeotecniaDetail()
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

        private void frmValGeotecniaDetail_Load(object sender, EventArgs e)
        {

            #region "SetUp & Traducccion"

            grValTrazado.Text = strFrmValoracion.uiGrDisVal;

            ucEstabilidadHorizontalTerreno.uiLbl = strFrmValoracion.uiEstabilidadTerreno;
            ucEstabilidadTaludes.uiLbl = strFrmValoracion.uiEstabilidadTalud;
            ucValorCBR.uiLbl = strFrmValoracion.uiCBR;
            ucValoracionAprovechamientos.uiLbl = strFrmValoracion.uiAprovechamientos;
            ucValoracionExcavabilidad.uiLbl = strFrmValoracion.uiExcavailidad;
            ucValoracionProteccionTaludes.uiLbl = strFrmValoracion.uiProteccionTalud;




            #endregion



            #region "SetUp Objetos"

            ucSaveCancel.lnkSalir.Visible = false;
            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion

            #region "SUMA 100"
            mLstTxtPC100.Add(ucEstabilidadHorizontalTerreno);
            mLstTxtPC100.Add(ucEstabilidadTaludes);
            mLstTxtPC100.Add(ucValorCBR);
            mLstTxtPC100.Add(ucValoracionAprovechamientos);
            mLstTxtPC100.Add(ucValoracionExcavabilidad);
            mLstTxtPC100.Add(ucValoracionProteccionTaludes);
            #endregion


            #region "DataBind"

            mTabla = ds.dataset.tbValGeotecnia;

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
            ucEstabilidadHorizontalTerreno.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValGeotecnia.estabilidadHorizontalTerrenoPCColumn.ColumnName, true);
            ucEstabilidadTaludes.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValGeotecnia.estabilidadTaludPCColumn.ColumnName, true);
            ucValorCBR.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValGeotecnia.valorCbrPCColumn.ColumnName, true);
            ucValoracionAprovechamientos.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValGeotecnia.aprovechamientosPCColumn.ColumnName, true);
            ucValoracionExcavabilidad.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValGeotecnia.excavabilidadPCColumn.ColumnName, true);
            ucValoracionProteccionTaludes.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValGeotecnia.proteccionTaludesPCColumn.ColumnName, true);

            #endregion
        }

    }
}
