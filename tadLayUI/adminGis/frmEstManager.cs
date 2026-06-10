using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.adminGis
{
    using System.Windows.Forms;

    using tadLayLogica;
    using tadLayLan;
    using tadLayLogica.zonaGis;
    using tadLayLogica.datos;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLan.Tdb;
    
    public  class frmEstManager : frmGeoManager
    {

      public frmEstManager()
       :base() 
      {

          mBindMaster = new BindingSource();
          mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbEst.TableName;
          mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbEst;
      }


      public override void  populate()
      {
          oSingletonDsBd.getInstance.Dispose();
          mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbEst;
          dgv.DataSource = mBindMaster;
      }


      protected override void dgvSetUp()
      {
          DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
          DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
          DataGridViewCheckBoxColumn miProhibir = new DataGridViewCheckBoxColumn();
          DataGridViewTextBoxColumn miColor = new DataGridViewTextBoxColumn();

          miId.HeaderText = "Id";
          miNombre.HeaderText = strFrmGisGeneral.uiNombre;
          miProhibir.HeaderText = strFrmGisEst.uiProhibirEst;
          miColor.HeaderText = strFrmGisGeneral.uiColor;

          miId.DataPropertyName = "id";
          miNombre.DataPropertyName = "nombre";
          miProhibir.DataPropertyName = "allowEstructuras";
          miColor.DataPropertyName = "color";

          dgv.Columns.Add(miId);
          dgv.Columns.Add(miNombre);
          dgv.Columns.Add(miProhibir);
          dgv.Columns.Add(miColor);

          dgv.Columns[0].Visible = false;

      }



        #region "Botonera"
      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {

              frmEst miFrm = new frmEst();

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

                    int miIndice = dgv.CurrentRow.Index;

                    frmEst miFrm = new frmEst();

                    miFrm.edit(id);

                    miFrm.ShowDialog();


                    this.populate();

                    //mBindMaster.RemoveFilter();

                    dgv.Rows[miIndice].Selected = true;

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
                  mBindMaster.RemoveCurrent();

                  oSingletonDsBd.getInstance.save(true);

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

      #region "BOTONES ZONAS TO CAD"

      protected override void btnLink_Click(object sender, EventArgs e)
      {

          try
          {

              if (dgv.SelectedRows.Count == 0)
              {
                  oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                  return;
              }

              frmBb.getInstance.Hide();

              oZonaGis miZona = new oZonaGeoEstructuras(id);

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




      private void InitializeComponent()
      {
          this.SuspendLayout();
 
          // frmEstManager
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.ClientSize = new System.Drawing.Size(516, 550);
          this.Name = "frmEstManager";
          this.ResumeLayout(false);
          this.PerformLayout();

      }


 

    }
}
