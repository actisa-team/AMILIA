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
    using tadLayShare;
    
    /// <summary>
    /// GASTOS CONSERVACION REHABILITACION
    /// </summary>
    public partial class frmGaCoReDetail : frmRoot
    {

        private string mId = "APP";
        private BindingSource mBindMaster;

         /// <summary>
         /// GASTOS CONSERVACION REHABILITACION
        /// </summary>
        public frmGaCoReDetail()
            : base()    
        {
            InitializeComponent();
            postConstructor();
        }

       #region "Metodos Privados"
        private void  postConstructor()
        {


            #region "Traduccion"

            //Grupo 1
            grRoadCero.Text = strFrmRentabilidad.uiGrGastosRoadCero;
            ucRoadCeroGastosConservacion.uiLbl = strFrmRentabilidad.uiGastosConservacion;
            ucRoadCeroGastosRehabilitar.uiLbl = strFrmRentabilidad.uiGastosRehabilitar;
            //grupo2
            grRoadUno.Text = strFrmRentabilidad.uiGrGastosRoadUno;
            ucRoadUnoGastosConservacion.uiLbl = strFrmRentabilidad.uiGastosConservacion;
            ucRoadUnoGastosRehabilitar.uiLbl = strFrmRentabilidad.uiGastosRehabilitar;
            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            //lnkSalir.Text = strGeneral.uiSalir;

            #endregion


            #region "BindCreate"

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = ds.dataset.tbGaCoRe.TableName;
            mBindMaster.DataSource = ds.dataset.tbGaCoRe;

            //Tabla Zona Geologica
            dsApp.tbGaCoReDataTable miTb = ds.dataset.tbGaCoRe;

            if (miTb.Rows.Count == 0)
            {
                mBindMaster.AddNew();
            }
            else if (miTb.Rows.Count == 1)
            {
                string miQuery = "id = '{0}'";
                mBindMaster.Filter = string.Format(miQuery, mId);
            }
            else
            {
                throw new oExRowFoundMayorUno("id", miTb.TableName);
            }


            //Grupo1
            ucRoadCeroGastosConservacion.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroConservarColumn.ColumnName,true);
            ucRoadCeroGastosRehabilitar.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroRehabilitarColumn.ColumnName,true);
            ucRoadUnoGastosConservacion.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadUnoConservarColumn.ColumnName,true);
            ucRoadUnoGastosRehabilitar.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadUnoRehabilitarColumn.ColumnName,true);


            #endregion

        }

       #endregion
       #region "Botones"
        //GUARDAR
        protected virtual void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (isValidoFrm)
                {
                    ((DataRowView)mBindMaster.Current)["id"] = mId;

                    mBindMaster.EndEdit();

                    ds.saveDataTable(ds.dataset.tbGaCoRe, true);


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
        //SALIR
        protected virtual void lnkSalir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        #endregion

        private void frmGaCoReDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            mBindMaster.CancelEdit();
        }


  

    }
}
