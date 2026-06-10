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

    
    public  class frmCunTraManager : frmDataGrid
    {

       public frmCunTraManager()
       :base() 
      {
          
      }


    public override void populate()
    {
        dgv.DataSource = oDalTabCunTra.getTabla();
    }

      protected override void dgvSetUp()
        {

            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miAltura = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miAnchoSup = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miAnchoInf = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miEspesor = new DataGridViewTextBoxColumn();


            miId.HeaderText = "id";
            miNombre.HeaderText = strFrmSecciones.uiNombre;
            miAltura.HeaderText = strFrmSecciones.uiAltura;
            miAnchoSup.HeaderText = strFrmSecciones.uiAnchoSuperior;
            miAnchoInf.HeaderText = strFrmSecciones.uiAnchoInferior;
            miEspesor.HeaderText = strFrmSecciones.uiEspesor;

            miId.DataPropertyName = "id";
            miNombre.DataPropertyName = "nombre";
            miAltura.DataPropertyName = "alturaIntM";
            miAnchoSup.DataPropertyName = "anchoIntSupM";
            miAnchoInf.DataPropertyName = "anchoIntInfM";
            miEspesor.DataPropertyName = "espesorM";

            dgv.Columns.Add(miId);
            dgv.Columns.Add(miNombre);
            dgv.Columns.Add(miAltura);
            dgv.Columns.Add(miAnchoSup);
            dgv.Columns.Add(miAnchoInf);
            dgv.Columns.Add(miEspesor);
            
            dgv.Columns[0].Visible = false;

        }

        #region "Botonera"

      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {

              frmCunTra miFrm = new frmCunTra();

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

                    frmCunTra miFrm = new frmCunTra();

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

                  oDalTabCunTra.deleteById(miId);
             
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
