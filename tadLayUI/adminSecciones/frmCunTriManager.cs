using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tadLayLan.Tdb;

namespace tadLayUI.adminSecciones
{
    using System.Windows.Forms;

   using tadLayLogica;
   using tadLayLogica.datos;
   using tadLayLogica.datos.Secciones;
   using tadLayLan;
   using tadLayUI.adminGis;

    
    public  class frmCunTriManager : frmDataGrid
    {

      #region "Constructor"

        public frmCunTriManager()
            : base()
        {

        }

        #endregion
      #region "Metodos Públicos"
        /// <summary>
        /// Populate DataGridView
        /// </summary>
        public override void populate()
        {
            dgv.DataSource = oDalTabCunTri.getTabla();
        }
        #endregion
      #region "Metodos Privados"

        protected override void dgvSetUp()
        {

            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miAltura = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miAncho = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miEspesor = new DataGridViewTextBoxColumn();


            miId.HeaderText = "id";
            miNombre.HeaderText = strFrmSecciones.uiNombre;
            miAltura.HeaderText = strFrmSecciones.uiAltura;
            miAncho.HeaderText = strFrmSecciones.uiAncho;
            miEspesor.HeaderText = strFrmSecciones.uiEspesor;



            miId.DataPropertyName = "id";
            miNombre.DataPropertyName = "nombre";
            miAltura.DataPropertyName = "alturaIntM";
            miAncho.DataPropertyName = "anchoIntSupM";
            miEspesor.DataPropertyName = "espesorM";

            dgv.Columns.Add(miId);
            dgv.Columns.Add(miNombre);
            dgv.Columns.Add(miAltura);
            dgv.Columns.Add(miAncho);
            dgv.Columns.Add(miEspesor);


            dgv.Columns[0].Visible = false;


        }

        #endregion
      #region "Botonera"

      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {

              frmCunTri miFrm = new frmCunTri();

              miFrm.create();

              miFrm.ShowDialog();

              populate();
          }
          catch (Exception ex)
          {
              oTadil.data.UserInfo.showError(ex);
          }

      }
      //EDITAR
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

                    Guid miId = (Guid)dgv.CurrentRow.Cells[0].Value;

                    frmCunTri miFrm = new frmCunTri();

                    miFrm.edit(miId);

                    miFrm.ShowDialog();

                    populate();

                    dgv.Rows[miRowIndexSelect].Selected = true;
                }

            }
            catch (Exception ex)
            {                
                oTadil.data.UserInfo.showError(ex);
            }

      }
      //BORRAR
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
                  Guid miId = (Guid)dgv.CurrentRow.Cells[0].Value;

                  oDalTabCunTri.deleteById(miId);

                  populate();
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
    }
}
