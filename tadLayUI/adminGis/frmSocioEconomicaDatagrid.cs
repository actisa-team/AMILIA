using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.adminGis

{
   using System.Windows.Forms;
   using tadLayLogica;
    using tadLayLogica.datos.BaseDatos;
   using tadLayLogica.datos.Gis;
   using tadLayLogica.zonaGis;
    using tadLayLan;
    using tadLayLan.Tdb;
   using tadLayUI.adminGis;
   using tadLayData;

    
    public  class frmSocioEconomicasDatagrid : frmGeoManager
    {


        private string mIdSector = string.Empty;
        string miQuery = "idCodeSector = '{0}'";
        

        public frmSocioEconomicasDatagrid(string iIdSector)
       :base() 
      {
          mIdSector = iIdSector;
      }



    protected override void dgvSetUp()
       {

           DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
           DataGridViewTextBoxColumn miIdSector = new DataGridViewTextBoxColumn();
           DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
           DataGridViewTextBoxColumn miDescripcion = new DataGridViewTextBoxColumn();
           DataGridViewCheckBoxColumn miProhibir = new DataGridViewCheckBoxColumn();
           DataGridViewTextBoxColumn miValoracion = new DataGridViewTextBoxColumn();

           miNombre.HeaderText = strGeneral.uiNombre;
           miDescripcion.HeaderText = strGeneral.uiDescripcion;
           miProhibir.HeaderText = strFrmGisGeneral.uiProhibirPaso;
           miValoracion.HeaderText = strFrmGisGeneral.uiValoracion;


           miId.DataPropertyName = "id";
           miIdSector.DataPropertyName = "idCodeSector";
           miNombre.DataPropertyName = "nombre";
           miDescripcion.DataPropertyName = "descripcion";
           miProhibir.DataPropertyName = "prohibirPaso";
           miValoracion.DataPropertyName = "valoracion";

           dgv.Columns.Add(miId);
           dgv.Columns.Add(miIdSector);
           dgv.Columns.Add(miNombre);
           dgv.Columns.Add(miDescripcion);
           dgv.Columns.Add(miProhibir);
           dgv.Columns.Add(miValoracion);

           dgv.Columns[0].Visible = false;
           dgv.Columns[1].Visible = false;


       }
    protected override void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
    }


    public override void populate()
    {

        mBindMaster = new BindingSource();
        oSingletonDsBd.getInstance.Dispose();
        mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbSocioEconomicos.TableName;
        mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbSocioEconomicos;
        setupFiltro();
    }

    private void setupFiltro()
    {

        oSingletonDsBd.getInstance.Dispose();
        mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbSocioEconomicos;
        mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mIdSector));
        dgv.DataSource = mBindMaster;  
    }



      #region "BOTONERA GRID"
      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {
              frmSocioEconomicaDetail miFrm = new frmSocioEconomicaDetail(mIdSector);
              miFrm.create();
              miFrm.ShowDialog();
              setupFiltro();
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
                  
                    frmSocioEconomicaDetail miFrm = new frmSocioEconomicaDetail(mIdSector);

                    miFrm.edit(id);

                    miFrm.ShowDialog();

                    setupFiltro();

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
                  setupFiltro();

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

              dsBd.tbSocioEconomicosRow miRow = oSingletonDsBd.getInstance.dataset.tbSocioEconomicos.FindByid(id);

              oZonaGis miZona = oFactoryZonaGis.createZonaGis(mIdSector, miRow);

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
          // frmSocioEconomicasDatagrid
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.ClientSize = new System.Drawing.Size(516, 550);
          this.Name = "frmSocioEconomicasDatagrid";
          this.ResumeLayout(false);
          this.PerformLayout();

      }

    }
}
