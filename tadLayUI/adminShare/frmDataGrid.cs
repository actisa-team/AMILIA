using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminGis
{

    using tadLayUI;
    using tadLayLogica;
    using tadLayData;
    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayLogica.datos.Gis;
    using tadLayLogica.zonaGis;
    
    
    
    public  partial class frmDataGrid: Form, IfrmManagerPopulate
    {
        #region "Constructor"
        public frmDataGrid()
        {
            InitializeComponent();
            traduccion();
            dgvSetUpMode();
            dgvSetUp();
        }
        #endregion



        #region "Propiedades"
        protected Guid id
        {
            get
            {
                return (Guid)dgv.CurrentRow.Cells[0].Value;
            }
        }
        #endregion

        #region "Interfaz"

        public virtual void populate()
        {
            throw new NotImplementedException();
        }

        public bool allowClosed()
        {
            return true;

        }

        #endregion



        #region "MetodosPublicos"

        protected virtual void dgvSetUpMode()
        {
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AutoGenerateColumns = false;
            dgv.MultiSelect = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = true;

            dgv.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Snow;
        }

        protected virtual void dgvSetUp() { ;}
  
   

        /// <summary>
        /// FORMATEO EL COLOR DE LA ZONA
        /// </summary>
        protected virtual void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
  

        #endregion

        #region "Metodos Privados"
        protected virtual void traduccion()
        {
            //Frm
            var name = oTadil.KAppHeaderName;
            this.Text = name;
   
            //Botonera
            lnkNew.Text = strGeneral.uiNuevo;
            lnkEdit.Text = strGeneral.uiEditar;
            lnkDelete.Text = strGeneral.uiBorrar;
            lnkSalir.Text = strGeneral.uiSalir;
            //Frm
            grListado.Text = strFrmGisGeneral.uiListadoRegistro;
        }


        private void dgv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                lnkEdit_LinkClicked(lnkEdit, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
            }
        }
        #endregion


        #region "Botones"
        /// <summary>
        /// ZONA NUEVA
        /// </summary>
        protected virtual void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { 
           throw new NotImplementedException()   ;
        }
        /// <summary>
        /// ZONA EDITAR
        /// </summary>
        protected virtual void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// BORRAR ZONA
        /// </summary>
        protected virtual void lnkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// SALIR
        /// </summary>
        private void lnkSalir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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


        /// <summary>
        /// Validación al Cerrar el Formulario
        /// </summary>
        protected virtual void frmManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                //if (oDalGeoTbZonas.isZonaGeneralOk())
                //{
                //    e.Cancel = true;
                //}
                //else
                //{
                //    oTadil.data.UserInfo.showInfo(strFrmGisGeneral.userZonaGeneralUnica);

                //    e.Cancel = false;

                //}

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }




        #endregion


 










    }
}
