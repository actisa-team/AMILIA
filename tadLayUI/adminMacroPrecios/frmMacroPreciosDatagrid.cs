using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.adminMacroPrecios
{
    using System.Windows.Forms;

   using tadLayLogica;
   using tadLayLogica.datos;
   using tadLayLogica.datos.MacroPrecios;
   using tadLayLan;
   using tadLayUI.adminGis;

    
    public  class frmMacroPreciosDatagrid : frmDataGrid
    {

        private string mIdRoadTipo = string.Empty;


       public frmMacroPreciosDatagrid()
       :base() 
      {
          
      }



    protected override void dgvSetUp()
       {

           DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
           DataGridViewTextBoxColumn miIdRoad = new DataGridViewTextBoxColumn();
           DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
           DataGridViewTextBoxColumn miDescripcion = new DataGridViewTextBoxColumn();

           miId.HeaderText = "id";
           miNombre.HeaderText = strGeneral.uiNombre;
           miDescripcion.HeaderText = strGeneral.uiDescripcion;


           miId.DataPropertyName = "id";
           miNombre.DataPropertyName = "nombre";
           miDescripcion.DataPropertyName = "descripcion";

           dgv.Columns.Add(miId);
           dgv.Columns.Add(miNombre);
           dgv.Columns.Add(miDescripcion);

           dgv.Columns[0].Visible = false;


       }


    public void populateByIdRoadTipo(string iIdRoadTipo)
    {
        mIdRoadTipo = iIdRoadTipo;
        dgv.DataSource = oDalMacroPrecios.getMacroPreciosByRoadTipo(mIdRoadTipo);
    }

    public override void populate()
    {
        throw new NotImplementedException();
    }



        #region "Botonera"

      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {
              frmMacroPreciosDetail miFrm = new frmMacroPreciosDetail();
              miFrm.create(mIdRoadTipo);
              miFrm.ShowDialog();

              //Relleno de Nuevo la Tabla
              populateByIdRoadTipo(mIdRoadTipo);
              
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
                   
                    frmMacroPreciosDetail miFrm = new frmMacroPreciosDetail();

                    miFrm.edit(miId,mIdRoadTipo);

                    miFrm.ShowDialog();

                    //DatagridView
                    populateByIdRoadTipo(mIdRoadTipo);

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

                  oDalMacroPrecios.delete(miId);
             
                  //Grid View
                  populateByIdRoadTipo(mIdRoadTipo);
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

      private void InitializeComponent()
      {
          this.SuspendLayout();
          // 
          // frmMacroPreciosDatagrid
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.ClientSize = new System.Drawing.Size(516, 550);
          this.Name = "frmMacroPreciosDatagrid";
          this.ResumeLayout(false);
          this.PerformLayout();

      }

 
    }
}
