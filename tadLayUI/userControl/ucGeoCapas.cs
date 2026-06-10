using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.userControl
{

    using engNet.eventos;
    using engNet.Extension.Double;
    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos.precios;
    using tadLayLogica.datos.Gis;
    using tadLayLogica;
    
    public partial class ucGeoCapas : UserControl
    {


        public event EventHandler<oEventArgs<bool>> evIsDataSetUpDate;


        /// <summary>
        /// TIPO De CAPA FIR ; ARC ; ASI (Solo 3)
        /// </summary>
        [Category("_CADnex_Data")]
        public string idCapaTipo { get; set; }

        [Category("_CADnex_Data")]
        public string idGrupo { get; set; }

        [Category("_CADnex_Data")]
        public string capaMaterialCombo { get; set; }



        [DefaultValue(null)]
        public Guid? idGeo { get; set; }
        
        public ucGeoCapas()
        {
            InitializeComponent();          
        }

        #region "Propiedades"
        public int count
        {
            get
            {
                return dgv.Rows.Count;  
            }    
        }
        #endregion
        #region "Metodos Publicos"
        public void iniciar(Guid iIdGeo)
        {

            //Codigo de la zona Geografica
            idGeo = iIdGeo;

            //Cargo los materiales
            ucCmbMaterial.idGrupo = idGrupo;
            ucCmbMaterial.idCode = capaMaterialCombo;
            ucCmbMaterial.populate();

            
            //configuro el Dgv
            dgvSetUp();

            dgvPopulate();

            //SetUp
            grMaster.Enabled = true;
            grDetail.Enabled = false;
            lnkClaCancel.Enabled = false;
            lnkClaSave.Enabled = false;


        }
        public void copyFrom(string iCodeCapaTipoToCopy)
        {
            oDalGeoTbCapas.copyCapasByTipo(idGeo.Value, iCodeCapaTipoToCopy, idCapaTipo);
            dgvPopulate();
            detailReset();
            evDataSetModificado();
        }
        #endregion
        #region "Metodos Privados"
        private void traduccion()
        {

            //Grupos
            grMaster.Text = strFrmGisGeo.ResourceManager.GetString("ui" + idCapaTipo);
            grDetail.Text = strFrmGisGeo.uiDetalle;
       
            //Botonera
            lnkClaNew.Text = strGeneral.uiNuevo;
            lnkClaEdit.Text = strGeneral.uiEditar;
            lnkClaDelete.Text = strGeneral.uiBorrar;
            lnkClaSave.Text = strGeneral.uiGuardar;
            lnkClaCancel.Text = strGeneral.uiCancelar;
            lnkBorrarAll.Text = strGeneral.uiBorrarListado;

            //Detail
            ucCmbMaterial.uiLbl = strFrmGisGeo.uiMaterial;
            ucCapaEspesor.uiLbl = strFrmGisGeo.uiEspesor;
            ucCapaPosicion.uiLbl = strFrmGisGeo.uiOrden;

            lblEspesorTotal.Text = string.Empty;
          
        }
        private void dgvPopulate()
        {
            dgv.DataSource = oDalGeoTbCapas.selectByZonaAndTipo(idGeo.Value, idCapaTipo);

            double miEspesorTotal = 0;

            foreach (DataGridViewRow item in dgv.Rows)
            {
                Guid miIdMaterial = (Guid)item.Cells["idMaterial"].Value;
                item.Cells["material"].Value = oDalMateriales.getMaterialNombreById(miIdMaterial);

                miEspesorTotal = miEspesorTotal + (double)item.Cells["espesor"].Value;
            }

            lblEspesorTotal.Text = string.Format(strFrmGisGeo.uiEspesorTotal, miEspesorTotal.roundOff(2).ToString());

        }
        private void dgvSetUp()
        {

            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AutoGenerateColumns = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            DataGridViewTextBoxColumn miIdCapa = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miIdGeo = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miIdMaterial = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miOrden = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miEspesor = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn miMaterial = new DataGridViewTextBoxColumn();


            miIdCapa.Name = "idCapa";
            miIdGeo.Name = "idGeo";
            miIdMaterial.Name = "idMaterial";
            miOrden.Name = "orden";
            miEspesor.Name = "espesor";
            miMaterial.Name = "material";


            miIdCapa.HeaderText = "idCapa";
            miIdGeo.HeaderText = "idGeo";
            miIdMaterial.HeaderText = "idMaterial";
            miOrden.HeaderText = strFrmGisGeo.uiOrden;
            miEspesor.HeaderText = strFrmGisGeo.uiEspesor;
            miMaterial.HeaderText = strFrmGisGeo.uiMaterial;


            miIdCapa.DataPropertyName = "id";
            miIdGeo.DataPropertyName = "idGeo";
            miIdMaterial.DataPropertyName = "idMaterial";
            miOrden.DataPropertyName = "orden";
            miEspesor.DataPropertyName = "espesorM";


            dgv.Columns.Add(miIdCapa);
            dgv.Columns.Add(miIdGeo);
            dgv.Columns.Add(miIdMaterial);
            dgv.Columns.Add(miOrden);
            dgv.Columns.Add(miEspesor);
            dgv.Columns.Add(miMaterial);
      

            dgv.Columns[0].Visible = false;
            dgv.Columns[1].Visible = false;
            dgv.Columns[2].Visible = false;

           

       
        }
        private void detailFill()
        {
            DataGridViewRow miDgvClaRow = dgv.CurrentRow;
            ucCapaPosicion.uitxt = miDgvClaRow.Cells["orden"].Value.ToString();
            ucCapaEspesor.uitxt = miDgvClaRow.Cells["espesor"].Value.ToString();
            ucCmbMaterial.uiCombo.Text = miDgvClaRow.Cells["material"].Value.ToString();

        }
        private void detailReset()
        {
            ucCapaPosicion.uitxt = string.Empty;
            ucCapaEspesor.uitxt = string.Empty;
        }
        #endregion
        #region "Eventos"


        private void ucGeoCapas_Load(object sender, EventArgs e)
        {
            traduccion();
            grDetail.Enabled = false;
            lnkClaSave.Enabled = false;
            lnkClaCancel.Enabled = false;

            //Reinicio los Valores de las Propiedades por Defecto
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            {
                DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes[typeof(DefaultValueAttribute)];
                if (attr != null)
                {
                    prop.SetValue(this, attr.Value);
                }
            }
        }


        private void grMaster_EnabledChanged(object sender, EventArgs e)
        {
            grDetail.Enabled = !grMaster.Enabled;
            lnkClaSave.Enabled = !grMaster.Enabled;
            lnkClaCancel.Enabled = !grMaster.Enabled;
        }

        private void evDataSetModificado()
        {
            if (evIsDataSetUpDate != null)
            {
                evIsDataSetUpDate(this, new oEventArgs<bool>(true));
            }
       }


  

        /// <summary>
        /// CLICK DATAGRIDVIEW
        /// </summary>
        private void dgv_MouseClick(object sender, MouseEventArgs e)
        {

            if (dgv.SelectedRows.Count == 1)
            {
               detailFill();
            }
        }

        /// <summary>
        /// DOBLE CLICK DATAGRIDVIEW
        /// </summary>
        private void dgv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lnkClaEdit_LinkClicked(lnkClaEdit, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
        }

        /// <summary>
        /// BOTON SUPRIMIR
        /// </summary>
        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {   
            //Boton Suprimir
            if (e.KeyCode == Keys.Delete)
            {  
                lnkClaDelete_LinkClicked(lnkClaDelete, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
            }
        }

        #endregion
        #region "Botonera"

        /// <summary>
        /// BORRAR TODO
        /// </summary>
        private void lnkBorrarAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {

                if (dgv.Rows.Count == 0 )
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowEmpty);
                    return;
                }

                DialogResult myResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroListados);

                if (myResul == System.Windows.Forms.DialogResult.Yes)
                {
                    oDalGeoTbCapas.deleteCapasByTipo(idGeo.Value, idCapaTipo);
                    detailReset();
                    dgvPopulate();
                    evDataSetModificado();
                }


            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);              
            }

        }
        /// <summary>
        /// BORRAR
        /// </summary>
        private void lnkClaDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {

                if (dgv.CurrentRow == null)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                    return;
                }

                DialogResult myResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroUnico);

                if (myResul == System.Windows.Forms.DialogResult.Yes)
                {         
                    oDalGeoTbCapas.deleteCapa(Convert.ToInt32(dgv.CurrentRow.Cells["idCapa"].Value) );                  
                    detailReset();
                    dgvPopulate();
                    evDataSetModificado();
                }


            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        /// <summary>
        /// GUARDAR
        /// </summary>
        private void lnkClaSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                    //Debemos de Validar el GrDetail
                if (oValidar.isValidoGrupo(grDetail))
                    {

                        //NUEVO
                        if (dgv.SelectedRows.Count == 0)
                        {
                            oDalGeoTbCapas.addCapa(idGeo.Value, idCapaTipo, ucCapaPosicion.valorInt, ucCapaEspesor.valorDouble, ucCmbMaterial.idMaterial);
                        }
                        //EDITAR
                        else
                        {
                            int miIdCapa = (int)dgv.CurrentRow.Cells["idCapa"].Value;
                            oDalGeoTbCapas.editCapa(miIdCapa, ucCapaPosicion.valorInt, ucCapaEspesor.valorDouble, ucCmbMaterial.idMaterial);
                        }

                        grMaster.Enabled = true;
                        dgv.ClearSelection();
                        lnkClaNew.Focus();
                        dgvPopulate();
                        evDataSetModificado();
                    }
 
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        /// <summary>
        /// NUEVO
        /// </summary>
        private void lnkClaNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {
                
                if (idGeo.HasValue)
                {
                dgv.ClearSelection();
                grMaster.Enabled = false;
                detailReset();
                ucCapaPosicion.textbox.Focus();
                }
                  else
                {
                    oTadil.data.UserInfo.showInfo(strFrmGisGeo.uiCapasMustSave);       
                }

               
            }
            catch (Exception ex )
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        /// <summary>
        /// EDITAR
        /// </summary>
        private void lnkClaEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                if (dgv.CurrentRow == null)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                    return;
                }

                grMaster.Enabled = false;
                detailFill();
                ucCapaPosicion.textbox.Focus();

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        /// <summary>
        /// CANCELAR
        /// </summary>
        private void lnkClaCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                grMaster.Enabled = true;
                detailReset();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }




        #endregion

  





































    }
}
