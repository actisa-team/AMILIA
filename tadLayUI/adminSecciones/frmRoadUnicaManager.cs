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
    using tadLayLogica.datos.bbdd.Secciones;
    using tadLayLogica.logica.secciones;

    public  class frmRoadUnicaGenManager : frmDataGrid
    {
        private Button btnLink;

        public frmRoadUnicaGenManager()
       :base() 
      {
            InitializeComponent();


            btnLink.Text = "Vincular polilínea";
        }


     public override void populate()
        {
            dgv.DataSource = oDalTabRoadUnica.getTabla();
        }
     protected override void dgvSetUp()
        {

            DataGridViewTextBoxColumn miId = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miNombre = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miDescripcion = new DataGridViewTextBoxColumn();

            miId.HeaderText = "id";
            miNombre.HeaderText = strFrmSecciones.uiNombre;
            miDescripcion.HeaderText = strFrmSecciones.uiDescripcion;

            miId.DataPropertyName = "id";
            miNombre.DataPropertyName = "nombre";
            miDescripcion.DataPropertyName = "descripcion";

            dgv.Columns.Add(miId);
            dgv.Columns.Add(miNombre);
            dgv.Columns.Add(miDescripcion);
         
            dgv.Columns[0].Visible = false;

        }
      #region "Botonera"

      //NUEVO
      protected override void lnkNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
          try
          {

              frmRoadUnicaGen miFrm = new frmRoadUnicaGen();

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

                    frmRoadUnicaGen miFrm = new frmRoadUnicaGen();

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

                  oDalTabRoadUnica.deleteById(miId);

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

        private void InitializeComponent()
        {
            this.btnLink = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(268, 518);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(227, 24);
            this.btnLink.TabIndex = 36;
            this.btnLink.Text = "btnLink";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // frmRoadUnicaGenManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(516, 550);
            this.Controls.Add(this.btnLink);
            this.Name = "frmRoadUnicaGenManager";
            this.Controls.SetChildIndex(this.btnLink, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnLink_Click(object sender, EventArgs e)
        {

            try
            {

                if (dgv.SelectedRows.Count == 0)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                    return;
                }

                frmBb.getInstance.Hide();

                oZonaUnicaGen miAdecacion = new oZonaUnicaGen(oDalSecciones.getRoadUni(id));

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
    
    }
}
