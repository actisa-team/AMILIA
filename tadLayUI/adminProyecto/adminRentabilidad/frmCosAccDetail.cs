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
    /// DATOS TRAFICO Y CONEXION ACTUAL
    /// </summary>
    public partial class frmCosAccDetail : frmRoot
    {

        private string mId = "APP";
        private BindingSource mBindMaster;

        /// <summary>
        /// DATOS TRAFICO Y CONEXION ACTUAL
        /// </summary>
        public frmCosAccDetail()
            : base()    
        {
            InitializeComponent();
            postConstructor();
        }

       #region "Metodos Privados"
        private void  postConstructor()
        {

            #region "Traduccion"

            //Grupo
            grCosteAccidente.Text = strFrmRentabilidad.uiGrAccidentesUnitarios;
            ucCosteMuerto.uiLbl = strFrmRentabilidad.uiAccCosteMuerto;
            ucCosteHerido.uiLbl = strFrmRentabilidad.uiAccHerido;


            //Grupo
            grK.Text = strFrmRentabilidad.uiGrK;
            ucNumHeridosPorAccidente.uiLbl = strFrmRentabilidad.uiNumHeridosAccidente;

            //Grupo
            grRoadCeroIndice.Text = strFrmRentabilidad.uiGrRoadCero;
            ucRoadCeroIP.uiLbl = strFrmRentabilidad.uiAccIP;
            ucRoadCeroIM.uiLbl = strFrmRentabilidad.uiAccIM;

            //Grupo
            grRoadUnoIndice.Text = strFrmRentabilidad.uiGrRoadUno;
            ucRoadUnoIP.uiLbl = strFrmRentabilidad.uiAccIP;
            ucRoadUnoIM.uiLbl = strFrmRentabilidad.uiAccIM;

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            //lnkSalir.Text = strGeneral.uiSalir;


            #endregion


            #region "Binding"


            mBindMaster = new BindingSource();
            mBindMaster.DataMember = ds.dataset.tbCosAcc.TableName;
            mBindMaster.DataSource = ds.dataset.tbCosAcc;

            //Tabla Costes Accidentes
            dsApp.tbCosAccDataTable miTb = ds.dataset.tbCosAcc;

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

            //Costes
            ucCosteHerido.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeHeridoColumn.ColumnName, true);
            ucCosteMuerto.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.costeMuerteColumn.ColumnName,true);

            ucNumHeridosPorAccidente.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.numHeridosPorAccidenteColumn.ColumnName, true);

            //Indice Road Cero
            ucRoadCeroIP.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroIPColumn.ColumnName, true);
            ucRoadCeroIM.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroIMColumn.ColumnName, true);

            //Indice Road Uno
            ucRoadUnoIP.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadUnoIPColumn.ColumnName, true);
            ucRoadUnoIM.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadUnoIMColumn.ColumnName, true);

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

                    ds.saveDataTable(ds.dataset.tbCosAcc, true);


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
       #region "Eventos FRM"

        private void frmCosAccDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            mBindMaster.CancelEdit();
        }

        #endregion



  

    }
}
