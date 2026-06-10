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

    using tadLayLogica.datos;

    using tadLayLogica.datos.BaseDatos;

    public  class frmCimManager : frmGeoManager
    {

      

      public frmCimManager()
       :base() 
      {
          mBindMaster = new BindingSource();
          mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbCim.TableName;
          mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbCim;
      }


      public override void populate()
      {
          oSingletonDsBd.getInstance.Dispose();
          mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbCim;
          dgv.DataSource = mBindMaster;


          //oSingletonDsBd.getInstance.Dispose();
          //dgv.DataSource = oSingletonDsBd.getInstance.dataset.tbCim;
      }


        #region "Botonera"
      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {

              frmCim miFrm = new frmCim();

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

                    frmCim miFrm = new frmCim();
   
                    miFrm.edit(id);

                    miFrm.ShowDialog();

                    this.populate();
                    //mBindMaster.RemoveFilter();
       
                    dgv.Rows[miIndice].Selected = true;

                    //oSingletonDsBd.getInstance.Dispose();
 
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


                  this.populate();
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

              oZonaGis miZona = new oZonaGeoCimentacion(id);

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
          // 
          // frmCimManager
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.ClientSize = new System.Drawing.Size(516, 550);
          this.Name = "frmCimManager";
          this.ResumeLayout(false);
          this.PerformLayout();

      }

    }
}
