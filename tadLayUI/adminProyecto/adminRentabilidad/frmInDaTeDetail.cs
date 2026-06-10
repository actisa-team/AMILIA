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
    
    using tadLayShare;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica;

    public partial class frmInDaTeDetail : frmRoot
    {

        private string mId = "APP";
        private BindingSource mBindMaster;


        public frmInDaTeDetail()
            : base()    
        {
            InitializeComponent();
            postConstructor();

        }

       #region "Metodos Privados"
       private void  postConstructor()
        {
            #region "Traduccion"

            //Frm Name
            var name = oTadil.KAppHeaderName;
            this.Text = name;

            //Grupo
            ucTasaActualizacionPC.uiLbl = strFrmRentabilidad.uiIdtTasaActualizacion;
            ucTasaRevisionPreciosPC.uiLbl = strFrmRentabilidad.uiIdtTasaRevisionPrecios;
            ucIPCanualPC.uiLbl = strFrmRentabilidad.uiIdtIpcAnual;
            ucConstruccionYears.uiLbl = strFrmRentabilidad.uiIdtConstruccionYears;
            ucExplotacionYears.uiLbl = strFrmRentabilidad.uiIdrEplotacionYears;


            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            //lnkSalir.Text = strGeneral.uiSalir;

            #endregion
            #region "Binding"

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = ds.dataset.tbInDaTe.TableName;
            mBindMaster.DataSource = ds.dataset.tbInDaTe;

            //Tabla Zona Geologica
            dsApp.tbInDaTeDataTable miTb = ds.dataset.tbInDaTe;

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




            //Grupo2
            ucTasaActualizacionPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.tasaActualizacionPCColumn.ColumnName, true);
            ucTasaRevisionPreciosPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.tasaRevisionPreciosPCColumn.ColumnName, true);
            ucIPCanualPC.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.tasaIpcPCColumn.ColumnName,true);
            ucConstruccionYears.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.numberYearConstruccionColumn.ColumnName,true);
            ucExplotacionYears.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.numberYearExplotacionColumn.ColumnName,true);

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

                    ds.saveDataTable(ds.dataset.tbInDaTe, true);

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


        private void frmInDaTeDetail_FormClosed(object sender, FormClosedEventArgs e)
        {
            mBindMaster.CancelEdit();
        }

      
        #endregion

 

  

    }
}
