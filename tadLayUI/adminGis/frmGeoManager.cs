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
    using tadLayLogica.datos.BaseDatos;


    /// <summary>
    /// OJO EL FORMULARIO DEL MOVIMIENTO DE TIERRAS, NO CARGA DEL oSingletonBaseDatos
    /// </summary>
    public partial class frmGeoManager : frmDataGrid
    {

        protected BindingSource mBindMaster = null;
        
     
        #region "Constructor"
        public frmGeoManager()
            :base()
        {
            InitializeComponent();

            
            btnLink.Text = strFrmGisGeneral.uiBtnZonaLink + "...";
        }
        #endregion






        #region "MetodosSobreEscritos"

        public override void  populate()
        {
 	        //Tabla Zona Geologica
            dgv.DataSource = oDalGeoTbZonas.getLstZonas();
        }

        protected override void dgvSetUp()
        {
            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn miProhibir = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn miColor = new DataGridViewTextBoxColumn();

            miId.HeaderText = "Id";
            miNombre.HeaderText = strFrmGisGeneral.uiNombre;
            miProhibir.HeaderText = strFrmGisGeneral.uiProhibirPaso;
            miColor.HeaderText = strFrmGisGeneral.uiColor;

            miId.DataPropertyName = "id";
            miNombre.DataPropertyName = "nombre";
            miProhibir.DataPropertyName = "prohibirPaso";
            miColor.DataPropertyName = "color";

            dgv.Columns.Add(miId);
            dgv.Columns.Add(miNombre);
            dgv.Columns.Add(miProhibir);
            dgv.Columns.Add(miColor);

            dgv.Columns[0].Visible = false;

        }
 
        #endregion
        #region "Botones"
        /// <summary>
        /// ZONA NUEVA
        /// </summary>
        protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {

                frmGeo miFrmGeo = new frmGeo();

                miFrmGeo.create();

                miFrmGeo.ShowDialog();

                dgv.DataSource =  oDalGeoTbZonas.getLstZonas();


            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }



        }    
        /// <summary>
        /// ZONA EDITAR
        /// </summary>
        protected override void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {

                if (dgv.SelectedRows.Count == 0)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                    return;
                }
                else
                {
                    int miRowIndexSelect = dgv.CurrentRow.Index;

                    frmGeo miFrmGeo = new frmGeo();

                    miFrmGeo.edit(id);

                    miFrmGeo.ShowDialog();

                    dgv.DataSource = oDalGeoTbZonas.getLstZonas();

                    dgv.Rows[miRowIndexSelect].Selected = true;
                }

            }
            catch (Exception ex)
            {                
                oTadil.data.UserInfo.showError(ex);
            }


        }
        /// <summary>
        /// BORRAR ZONA
        /// </summary>
        protected override void lnkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                if (dgv.SelectedRows.Count == 0)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                    return;
                }

                DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroUnico);

                if (miResul == DialogResult.Yes)
                {
                    oDalGeoTbZonas.deleteZona(id);
                    dgv.DataSource =  oDalGeoTbZonas.getLstZonas();
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }

            }
            catch (Exception ex)
            { 
              oTadil.data.UserInfo.showError(ex);
            }

        }
        #endregion
        #region "Eventos"
        /// <summary>
        /// FORMATEO EL COLOR DE LA ZONA
        /// </summary>
        protected override void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex ==3  && e.Value != null)
            {
                int miColor = (int)e.Value;
                e.CellStyle.BackColor = Color.FromArgb(miColor);
                e.CellStyle.ForeColor = Color.FromArgb(miColor);
                e.Value = null;
            }
        }
        #endregion


        #region "BOTONES ZONAS TO CAD"
        
        protected virtual void btnLink_Click(object sender, EventArgs e)
        {

            try
            {

                if (dgv.SelectedRows.Count == 0)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                    return;
                }

                frmBb.getInstance.Hide();

                oZonaGis miZona = new oZonaGeoMovimientoTierras(oDalGeoTbZonas.getZonaGeo(id));

                miZona.link();
              
            }
            catch (Exception ex)
            {

                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmBb.getInstance.Show();
            }

        }
        #endregion 
    }
}
