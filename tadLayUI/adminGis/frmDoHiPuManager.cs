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

    using tadLayLogica.zonaGis;

    using tadLayLogica.datos;
    using tadLayLogica.datos.BaseDatos;
    
    public  class frmDoHiPuManager : frmGeoManager
    {
        private Button btnSelectEje;

        public frmDoHiPuManager()
       :base() 
      {

          InitializeComponent();

          postConstructor();

      }



        private void postConstructor()
        {

            btnSelectEje.Text = strFrmGisGeneral.uiVincularPolilineaToCauce;

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbDoHi.TableName;
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbDoHi;
        }


        public override void populate()
        {
            oSingletonDsBd.getInstance.Dispose();
            mBindMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbDoHi;
            dgv.DataSource = mBindMaster;
        }

      protected override void dgvSetUp()
        {
            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn miProhibir = new DataGridViewCheckBoxColumn();
            DataGridViewCheckBoxColumn miPasarEstructura  = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn miValoracion = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miColor = new DataGridViewTextBoxColumn();

            miId.HeaderText = "Id";
            miNombre.HeaderText = strFrmGisGeneral.uiNombre;
            miProhibir.HeaderText = strFrmGisGeneral.uiProhibirPaso;
            miPasarEstructura.HeaderText = strFrmGisGeneral.uiPasarEstructuraObligado;
            miValoracion.HeaderText = strFrmGisGeneral.uiValoracion;
           

            miId.DataPropertyName = "id";
            miNombre.DataPropertyName = "nombre";
            miProhibir.DataPropertyName = "prohibirPaso";
            miPasarEstructura.DataPropertyName = "pasarEstructura";
            miValoracion.DataPropertyName = "valoracion";

            dgv.Columns.Add(miId);
            dgv.Columns.Add(miNombre);
            dgv.Columns.Add(miProhibir);
            dgv.Columns.Add(miPasarEstructura);
 
            dgv.Columns[0].Visible = false;

        }




 



        #region "Botonera"

      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {

              frmDoHi miFrm = new frmDoHi();

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

                    Guid miId = (Guid)dgv.CurrentRow.Cells[0].Value;

                    frmDoHi miFrm = new frmDoHi();

                    miFrm.edit(miId);

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

      //SELECCION POLILINEA TO ZONA
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

              oZonaGis miZona = new oZonaDominioHidraulico(id);

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

      //SELECCION POLILINEA TO EJE 
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

              oZonaDominioHidraulico miZona = new oZonaDominioHidraulico(id);

              miZona.linkEje(strFrmGisGeneral.uiSelectCauce);

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


        #region "Eventos"

      //OJO PROBLEMA HERENCIA FORMULARIO FRMGEOMANAGER, Esta Implementado este Evento para Pintar la Celda de Color
      protected override void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
      {
           
      }

        #endregion

      private void InitializeComponent()
      {
          this.btnSelectEje = new System.Windows.Forms.Button();
          this.SuspendLayout();
          // 
          // btnSelectEje
          // 
          this.btnSelectEje.Location = new System.Drawing.Point(10, 520);
          this.btnSelectEje.Name = "btnSelectEje";
          this.btnSelectEje.Size = new System.Drawing.Size(250, 23);
          this.btnSelectEje.TabIndex = 36;
          this.btnSelectEje.Text = "btnSelectEje";
          this.btnSelectEje.UseVisualStyleBackColor = true;
          this.btnSelectEje.Click += new System.EventHandler(this.btnSelectEje_Click);
          // 
          // frmDoHiPuManager
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.ClientSize = new System.Drawing.Size(516, 550);
          this.Controls.Add(this.btnSelectEje);
          this.Name = "frmDoHiPuManager";
          this.Controls.SetChildIndex(this.btnSelectEje, 0);
          this.ResumeLayout(false);
          this.PerformLayout();

      }




    }
}
