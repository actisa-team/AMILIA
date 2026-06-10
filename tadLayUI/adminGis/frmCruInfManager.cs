using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.adminGis
{
    using System.Windows.Forms;

    using tadLayLogica;
    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayLogica.datos.Gis;
    using tadLayLogica.zonaGis;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.datos;
    
    public  class frmCruInfManager : frmGeoManager
    {

        public frmCruInfManager()
       :base() 
      {
            InitializeComponent();
            postConstructor();
      }



        private void postConstructor()
        {
            //btnSelectEje.Text = strFrmGisGeneral.uiVincularPolilineaToEjeCruce;

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbCruceInfra.TableName;
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbCruceInfra;
        }

        public override void populate()
        {
            oSingletonDsBd.getInstance.Dispose();
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbCruceInfra;
            dgv.DataSource = mBindMaster;
        }



        /// <summary>
        /// FORMATEO EL COLOR DE LA ZONA
        /// </summary>
        protected override void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.ColumnIndex == 4 && e.Value != null)
            //{
            //    int miColor = (int)e.Value;
            //    e.CellStyle.BackColor = Color.FromArgb(miColor);
            //    e.CellStyle.ForeColor = Color.FromArgb(miColor);
            //    e.Value = null;
            //}
        }

      protected override void dgvSetUp()
        {
            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn miProhibir = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn miPasoNivelExigir  = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn miValoracion = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miGalibo = new DataGridViewTextBoxColumn();

            miId.HeaderText = "Id";
            miNombre.HeaderText = strFrmGisGeneral.uiNombre;
            miProhibir.HeaderText = strFrmGisGeneral.uiProhibirPaso;
            miPasoNivelExigir.HeaderText = strFrmGisGeneral.uiPasoNivelExigir;
            miValoracion.HeaderText = strFrmGisGeneral.uiValoracion;
            miGalibo.HeaderText = strFrmGisGeneral.uiGalibo;

            miId.DataPropertyName = "id";
            miNombre.DataPropertyName = "nombre";
            miProhibir.DataPropertyName = "prohibirPaso";
            miPasoNivelExigir.DataPropertyName = "pasoNivelExigir";
            miValoracion.DataPropertyName = "valoracion";
            miGalibo.DataPropertyName = "galibo";

            dgv.Columns.Add(miId);
            dgv.Columns.Add(miNombre);
            dgv.Columns.Add(miProhibir);
            dgv.Columns.Add(miPasoNivelExigir);
            dgv.Columns.Add(miGalibo);
            dgv.Columns.Add(miValoracion);

            dgv.Columns[0].Visible = false;


        }




 



        #region "Botonera"

      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {

              frmCruInf miFrm = new frmCruInf();

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

                    frmCruInf miFrm = new frmCruInf();

                    miFrm.edit(id);

                    miFrm.ShowDialog();

                    //mBindMaster.RemoveFilter();
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

          }
          catch (Exception ex)
          {
              oTadil.data.UserInfo.showError(ex);
          }

      }

        #endregion

      #region "BOTONES ZONAS TO CAD"

      // LINK ZONAS GIS
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

              oZonaGis miZona = new oZonaCruInf(id);

              miZona.link();

              frmBb.getInstance.Show();

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

      //LINK EJE
      private void btnSelectEje_Click(object sender, EventArgs e)
      {

          try
          {

              if (dgv.SelectedRows.Count == 0)
              {
                  oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                  return;
              }


              frmBb.getInstance.Hide();

              oZonaCruInf miZona = new oZonaCruInf(id);

              miZona.linkEje(strFrmGisGeneral.uiSelectEjeCruceInfraestructuras);

              frmBb.getInstance.Show();

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
          // frmCruInfManager
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.ClientSize = new System.Drawing.Size(516, 550);
          this.Name = "frmCruInfManager";
          this.ResumeLayout(false);
          this.PerformLayout();

      }




  

    }
}
