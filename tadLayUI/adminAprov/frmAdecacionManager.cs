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
    using tadLayLogica.datos.bbdd.Adecuacion;
    using tadLayUI.adminAprov;
    using tadLayLogica.logica.Adecuacion;


    /// <summary>
    /// OJO EL FORMULARIO DEL MOVIMIENTO DE TIERRAS, NO CARGA DEL oSingletonBaseDatos
    /// </summary>
    public partial class frmAdecacionManager : frmDataGrid
    {

        protected BindingSource mBindMaster = null;
        
     
        #region "Constructor"
        public frmAdecacionManager()
            :base()
        {
            InitializeComponent();

            
            btnLink.Text = "Vincular polilínea";
        }
        #endregion


        #region "MetodosSobreEscritos"

        public override void  populate()
        {
 	        //Tabla Zona Geologica
            dgv.DataSource = oDalAdecuacion.getAdecuacion();
        }

        protected override void dgvSetUp()
        {
            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miDescripcion = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miEspesor = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miColor = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn mifirme = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn misaneo = new DataGridViewCheckBoxColumn();


            miId.HeaderText = "Id";
            miNombre.HeaderText = "Nombre";
            miEspesor.HeaderText = "Espesor(cm)";
            miDescripcion.HeaderText = "Descripción";
            miColor.HeaderText = "Color";
            mifirme.HeaderText = "Firme";
            misaneo.HeaderText = "Saneo";

            miId.DataPropertyName = "id";
            miNombre.DataPropertyName = "nombre";
            miEspesor.DataPropertyName = "espesorfirme";
            miDescripcion.DataPropertyName = "descripcion";
            miColor.DataPropertyName = "color";
            mifirme.DataPropertyName = "firme";
            misaneo.DataPropertyName = "saneo";

            dgv.Columns.Add(miId);
            dgv.Columns.Add(miNombre);
            dgv.Columns.Add(miEspesor);
            dgv.Columns.Add(miDescripcion);
            dgv.Columns.Add(mifirme);
            dgv.Columns.Add(misaneo);
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

                frmApovDatos frmApov = new frmApovDatos();

                frmApov.create();

                frmApov.ShowDialog();

                dgv.DataSource =  oDalAdecuacion.getAdecuacion();


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

                    frmApovDatos miFrmApov = new frmApovDatos();

                    miFrmApov.edit(id);

                    miFrmApov.ShowDialog();

                    dgv.DataSource = oDalAdecuacion.getAdecuacion();

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
                    oDalAdecuacion.deleteZona(id);
                    dgv.DataSource =  oDalAdecuacion.getAdecuacion();
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
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var column = dgv.Columns[e.ColumnIndex];
                bool isColorColumn = (column.DataPropertyName != null && column.DataPropertyName.Equals("color", StringComparison.OrdinalIgnoreCase))
                                     || (column.HeaderText != null && column.HeaderText.Equals("Color", StringComparison.OrdinalIgnoreCase))
                                     || e.ColumnIndex == 6;

                if (isColorColumn && e.Value != null && e.Value != DBNull.Value)
                {
                    try
                    {
                        int miColor = Convert.ToInt32(e.Value);
                        Color color = Color.FromArgb(miColor);
                        // Asegurar canal Alfa en 255 (totalmente opaco)
                        color = Color.FromArgb(255, color.R, color.G, color.B);

                        // Aplicar al fondo y texto normal
                        e.CellStyle.BackColor = color;
                        e.CellStyle.ForeColor = color;

                        // Aplicar también al estar seleccionado para evitar que la selección lo tape
                        e.CellStyle.SelectionBackColor = color;
                        e.CellStyle.SelectionForeColor = color;

                        e.Value = null; // Usar null igual que en frmGeoManager.cs
                    }
                    catch
                    {
                    }
                }
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

                oZonaAdecuacion miAdecacion = new oZonaAdecuacion(oDalAdecuacion.getAdecuacion(id));

                miAdecacion.link();
              
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
