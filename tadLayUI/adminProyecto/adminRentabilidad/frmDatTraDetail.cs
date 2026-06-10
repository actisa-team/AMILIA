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
    public partial class frmDatTraDetail : frmRoot
    {

        private string mId = "APP";
        private BindingSource mBindMaster;

        /// <summary>
        /// DATOS TRAFICO Y CONEXION ACTUAL
        /// </summary>
        public frmDatTraDetail()
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
            grDatosTraficoOpcion0.Text = strFrmRentabilidad.uiGrDto;
            ucRoadCeroIMDyearPuestaServicio.uiLbl = strFrmRentabilidad.uiDtoImd;
            ucRoadCeroCrecimientoAnualPC.uiLbl = strFrmRentabilidad.uiDtoCrecimientoAnual;
            ucRoadCeroLonKm.uiLbl = strFrmRentabilidad.uiDtoLon;
            ucConexionCeroVpKmH.uiLbl = strFrmRentabilidad.uiDtoVp;
            ucRoadCeroVehiculosPesadosPC.uiLbl = strFrmRentabilidad.uiDtoVehiculoPesados;

            //Grupo Conexion Nueva
            grConexionNew.Text = strFrmRentabilidad.uiGrDtc;
            ucRoadUnoAbsorcionTraficoYearInicialPC.uiLbl = strFrmRentabilidad.uiDtcAbsorcionInicial;
            ucRoadUnoAbsorcionTraficoYearFinPC.uiLbl = strFrmRentabilidad.uiDtcAbosorcionFinal;
            ucRoadUnoVehiculosPesadosPC.uiLbl = strFrmRentabilidad.uiDtoVehiculoPesados;

            ucConexionActualEliminar.uiLbl = strFrmRentabilidad.uiConexionActualEliminar;
           

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            //lnkSalir.Text = strGeneral.uiSalir;
            #endregion



            #region "SetUp Objetos"

            ucConexionActualEliminar.populate();
            ucConexionActualEliminar.uiCombo.SelectedIndexChanged += new EventHandler(uiCombo_SelectedIndexChanged);

            #endregion




            #region "BindCreate"

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = ds.dataset.tbDatTra.TableName;
            mBindMaster.DataSource = ds.dataset.tbDatTra;

            //Tabla Zona Geologica
            dsApp.tbDatTraDataTable miTb = ds.dataset.tbDatTra;

            if (miTb.Rows.Count == 0)
            {
                if (mBindMaster.Count > 0)
                {
                    mBindMaster.CancelEdit();  //BUG a veces al cancelar, no actualiza el databind
                }
                else
                {
                    mBindMaster.AddNew();
                }
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


            //Grupo ROAD CERO
            ucRoadCeroIMDyearPuestaServicio.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroImdColumn.ColumnName,true);
            ucRoadCeroCrecimientoAnualPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroCrecimientoAnualPCColumn.ColumnName, true);
            ucRoadCeroVehiculosPesadosPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroVehiculosPesadosPCColumn.ColumnName,true);
            ucRoadCeroLonKm.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroLongitudKmColumn.ColumnName,true);
            ucConexionCeroVpKmH.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadCeroVelocidadMediaColumn.ColumnName,true);

            //Grupo ROAD UNO
            ucConexionActualEliminar.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, miTb.roadConexionActualEliminarColumn.ColumnName, true);
            ucRoadUnoAbsorcionTraficoYearInicialPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadUnoAbsorcionTraficoInicioPCColumn.ColumnName, true);
            ucRoadUnoAbsorcionTraficoYearFinPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadUnoAbsorcionTraficoFinalPCColumn.ColumnName, true);
            ucRoadUnoVehiculosPesadosPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.roadUnoVehiculosPesadosPCColumn.ColumnName,true);

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

                    ds.saveDataTable(ds.dataset.tbDatTra, true);


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

        #region "Eventos"

        void uiCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ucConexionActualEliminar.valor)
            {
                this.ucRoadUnoAbsorcionTraficoYearInicialPC.textbox.valorDoubleNull = 100;
                this.ucRoadUnoAbsorcionTraficoYearInicialPC.textbox.Enabled = false;
                this.ucRoadUnoAbsorcionTraficoYearFinPC.textbox.valorDoubleNull = 100;
                this.ucRoadUnoAbsorcionTraficoYearFinPC.textbox.Enabled = false;
            }
            else
            {
                this.ucRoadUnoAbsorcionTraficoYearInicialPC.textbox.valorDoubleNull = null;
                this.ucRoadUnoAbsorcionTraficoYearInicialPC.textbox.Enabled = true;
                this.ucRoadUnoAbsorcionTraficoYearFinPC.textbox.valorDoubleNull = null;
                this.ucRoadUnoAbsorcionTraficoYearFinPC.textbox.Enabled = true;
            }
        }



        #endregion





    }
}
