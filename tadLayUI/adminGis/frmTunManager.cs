using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.adminGis
{
    using System.Windows.Forms;

    using tadLayLogica;
    using tadLayLan;
    using tadLayLogica.datos.Gis;
    using tadLayLogica.zonaGis;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.datos;
    using tadLayLan.Tdb;
    
    public  class frmTunManager : frmGeoManager
    {

      public frmTunManager()
       :base() 
      {
          mBindMaster = new BindingSource();
          mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbTun.TableName;
          mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbTun;
      }


      public override void populate()
      {
          oSingletonDsBd.getInstance.Dispose();
          mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbTun;
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
          miProhibir.HeaderText = strFrmGisTun.uiProhibirTuneles;
          miColor.HeaderText = strFrmGisGeneral.uiColor;

          miId.DataPropertyName = "id";
          miNombre.DataPropertyName = "nombre";
          miProhibir.DataPropertyName = "allowTuneles";
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

              frmTun miFrm = new frmTun();

              miFrm.create();

              miFrm.ShowDialog();

              this.populate();


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

                    frmTun miFrm = new frmTun();

                    miFrm.edit(id);

                    miFrm.ShowDialog();


                    this.populate();

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

              this.populate();

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

              oZonaGis miZona = new oZonaGeoTuneles(id);

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
