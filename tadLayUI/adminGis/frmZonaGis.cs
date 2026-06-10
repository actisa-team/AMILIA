using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminGis
{

    using engCadNet;

    using System.IO;
    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayLogica;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.zonaGis;
    using tadLayData;
    using engNet.Extension.String;
    using tadLayShare;
    

    
    public partial class frmZonaGis : Form
    {


        private string mFichaId = string.Empty;
        
        #region "Constructor"
        public frmZonaGis(string iFichaId)
        {
            InitializeComponent();

            mFichaId = iFichaId;

            inicializa();
        }
        #endregion

        #region "Destructor"

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        #endregion

        #region "Inicializar"

        private void inicializa()
        {

            loadXml();

            traduccion();

            dgvSetUp();            
        }

        #endregion
        #region "Metodos Privados"

        private void populate()
        {
            oSingletonDsBd.getInstance.Dispose();
            loadXml();
        }

        private void loadXml()
        {
           
            //Datos Lbl
            dsBd.tbZgFichaRow myRowFicha = oSingletonDsBd.getInstance.dataset.tbZgFicha.FindByid(mFichaId);
 
      
            if (myRowFicha == null)
            {   
                throw new Exception(string.Format(strFrmGisGeneral.uiFichaNotFound, myRowFicha.nombre));
            }
                 
            //Vinculo los DATAGRIDVIEW
            BindingSource myFicha = new BindingSource();
            myFicha.DataMember = oSingletonDsBd.getInstance.dataset.tbZgFicha.TableName;
            myFicha.DataSource = oSingletonDsBd.getInstance.dataset.tbZgFicha;
            myFicha.Filter = "id LIKE '" + mFichaId + "'";

            BindingSource myClasificacion = new BindingSource();
            myClasificacion.DataSource = myFicha;
            myClasificacion.DataMember = "FK_tbZgFicha_tbZgClasificaciones";

            BindingSource myDetail = new BindingSource();
            myDetail.DataSource = myClasificacion;
            myDetail.DataMember = "FK_tbZgClasificaciones_tbZgItems";

            dgvClasificaciones.DataSource = myClasificacion;
            dgvItems.DataSource = myDetail;

     
        }
        private void traduccion()
        {

            var name = oTadil.KAppHeaderName;

            this.Text = name;

            grClasificaciones.Text = strFrmGisGeneral.uiGrClasificaciones;
            lblClaNombre.Text = strFrmGisGeneral.uiNombre;
            lblClaDescripcion.Text = strFrmGisGeneral.uiDescripcion;
            lnkClaNew.Text = strGeneral.uiNuevo;
            lnkClaEdit.Text = strGeneral.uiEditar;
            lnkClaDelete.Text = strGeneral.uiBorrar;
            lnkClaSave.Text = strGeneral.uiGuardar;
            lnkClaCancel.Text = strGeneral.uiCancelar;


            grItems.Text = strFrmGisGeneral.uiGrItem;
            lblItemNombre.Text = strFrmGisGeneral.uiNombre;
            lblItemDescripcion.Text = strFrmGisGeneral.uiDescripcion;
            chkItemProhibirPaso.Text = strFrmGisGeneral.uiProhibirPaso;
            lblValoracion.Text = strFrmGisGeneral.uiValoracion;

            lnkImgUp.Text = strFrmGisGeneral.uiImagenLoad;


            lnkItemNew.Text = strGeneral.uiNuevo;
            lnkItemEdit.Text = strGeneral.uiEditar;
            lnkItemDelete.Text = strGeneral.uiBorrar;
            lnkItemSave.Text = strGeneral.uiGuardar;
            lnkItemCancel.Text = strGeneral.uiCancelar;


            //Editor de Zonas
            btnZonaLink.Text = strFrmGisGeneral.uiBtnZonaLink + "...";


        }
        private void dgvSetUp()
        {

            //Clasificaciones
            dgvClasificaciones.RowHeadersVisible = false;
            dgvClasificaciones.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClasificaciones.Columns[oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.idColumn.ColumnName].Visible = false;
            dgvClasificaciones.Columns[oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.idzgfichaColumn.ColumnName].Visible = false;
            dgvClasificaciones.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvClasificaciones.AllowUserToAddRows = false;
            dgvClasificaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClasificaciones.MultiSelect = false;
            dgvClasificaciones.AllowUserToResizeRows = false;

            dgvClasificaciones.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvClasificaciones.AlternatingRowsDefaultCellStyle.BackColor = Color.Snow;

            itemClaEnable(false);

            //Traduccion
            dgvClasificaciones.Columns[oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.nombreColumn.ColumnName].HeaderText = strFrmGisGeneral.uiNombre;
            dgvClasificaciones.Columns[oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.descripcionColumn.ColumnName].HeaderText = strFrmGisGeneral.uiDescripcion;


            //ITEMS
            dgvItems.RowHeadersVisible = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Columns[oSingletonDsBd.getInstance.dataset.tbZgItems.idColumn.ColumnName].Visible = false;
            dgvItems.Columns[oSingletonDsBd.getInstance.dataset.tbZgItems.idzgclasificacionesColumn.ColumnName].Visible = false;
            dgvItems.Columns[oSingletonDsBd.getInstance.dataset.tbZgItems.imageNombreColumn.ColumnName].Visible = false;
            dgvItems.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.MultiSelect = false;
            dgvItems.AllowUserToResizeRows = false;

            dgvItems.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvItems.AlternatingRowsDefaultCellStyle.BackColor = Color.Snow;

            //Traduccion
            dgvItems.Columns[oSingletonDsBd.getInstance.dataset.tbZgItems.nombreColumn.ColumnName].HeaderText = strFrmGisGeneral.uiNombre;
            dgvItems.Columns[oSingletonDsBd.getInstance.dataset.tbZgItems.descripcionColumn.ColumnName].HeaderText = strFrmGisGeneral.uiDescripcion;
            dgvItems.Columns[oSingletonDsBd.getInstance.dataset.tbZgItems.valoracionColumn.ColumnName].HeaderText = strFrmGisGeneral.uiValoracion;
            dgvItems.Columns[oSingletonDsBd.getInstance.dataset.tbZgItems.prohibirColumn.ColumnName].HeaderText = strFrmGisGeneral.uiProhibir;

            itemEnable(false);

        }
        private string getFileName(string iImagePath)
        {

            if (string.IsNullOrEmpty(iImagePath))
            {
                return string.Empty;
            }
            else
            {
                FileInfo miFile = new FileInfo(iImagePath);

                return miFile.Name;
            }
        }

        #endregion
        #region "DGV-CLASIFICACIONES"

        //EVENT DGV
        private void dgvClasificaciones_MouseClick(object sender, MouseEventArgs e)
        {
            if (dgvClasificaciones.SelectedRows.Count == 1)
            {
                itemClaFill();            
            }

            if (dgvItems.Rows.Count > 0)
            {
                dgvItems.ClearSelection();
            }
        }
        //---------------------------------------------------------------------------------------
        //NUEVO
        private void lnkClaNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            itemClaEnable(true);
            itemClaLimpia();
            txtClaNombre.Focus();
        }
        //EDITAR
        private void lnkClaEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (dgvClasificaciones.SelectedRows.Count == 0)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                return;
            }


            lnkClaNew_LinkClicked(this,new LinkLabelLinkClickedEventArgs(null));

            itemClaFill();


        }
        //DELETE
        private void lnkClaDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (dgvClasificaciones.SelectedRows.Count == 0)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                return;
            }

            DialogResult myResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroUnico);

            if (myResul == System.Windows.Forms.DialogResult.Yes)
            {
                dgvClasificaciones.Rows.Remove(dgvClasificaciones.CurrentRow);
                
                //oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.RemovetbZgClasificacionesRow(
                oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbZgClasificaciones, true);
                populate();
            }

        }
        //-----------------------------------------------------------------------------------------
        //GUARDAR
        private void lnkClaSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            try
            {

                if (!this.ValidateChildren())
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosNoValidos);
                    return;
                }


                if (string.IsNullOrEmpty(lblClaId.Text))
                {

                    if (oExtensionString.isNombreSolucionValido(txtClaNombre.Text))
                    {
                        //NEVO
                        dsBd.tbZgClasificacionesRow myRow = oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.NewtbZgClasificacionesRow();
                        myRow.id = Guid.NewGuid();
                        myRow.nombre = txtClaNombre.Text.Trim();
                        myRow.descripcion = txtClaDes.Text;
                        myRow.idzgficha = mFichaId;
                        oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.AddtbZgClasificacionesRow(myRow);
                        
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiNombreConCaracteresEspeciales);
                    }

                }
                else
                {
                    //EDITAR
                    Guid rowId = new Guid(lblClaId.Text.Trim());
                    dsBd.tbZgClasificacionesRow myRow = oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.FindByid(rowId);

                    if (oExtensionString.isNombreSolucionValido(txtClaNombre.Text))
                    {

                        //Eventos Subject
                        if (myRow.nombre != txtClaNombre.Text)
                        {
                            myRow.nombre = txtClaNombre.Text;
                        }

                        if (myRow.descripcion != txtClaDes.Text)
                        {
                            myRow.descripcion = txtClaDes.Text;
                        }
                        this.loadXml();
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiNombreConCaracteresEspeciales);
                    }
                    
                   
                    
                    
                }

                oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbZgClasificaciones, true);
                populate();

                itemClaEnable(false);

            }
            catch (oExFileNotFoundImg ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (Exception ex )
            {

                oTadil.data.UserInfo.showError(ex);
            }


           


        }
        //CANCELAR
        private void lnkClaCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            itemClaEnable(false);
            itemClaLimpia();
        }
        //---------------------------------------------------------------------------------------
        //FILL
        private void itemClaFill()
        {
            DataGridViewRow iDgvClaRow = dgvClasificaciones.CurrentRow;
            
            lblClaId.Text = iDgvClaRow.Cells[0].Value.ToString();
            txtClaNombre.Text = iDgvClaRow.Cells[1].Value.ToString();
            txtClaDes.Text = iDgvClaRow.Cells[2].Value.ToString();
        }
        //CLEAR
        private void itemClaLimpia()
        {
            lblClaId.Text = string.Empty;
            txtClaNombre.Text = string.Empty;
            txtClaDes.Text = string.Empty;
        }
        //ENABLE
        private void itemClaEnable(bool iEstado)
        {
            txtClaNombre.Enabled = iEstado;
            txtClaDes.Enabled = iEstado;
            lnkClaSave.Visible = iEstado;
            lnkClaCancel.Visible = iEstado;


            lnkClaNew.Visible = !iEstado;
            lnkClaEdit.Visible = !iEstado;
            lnkClaDelete.Visible = !iEstado;

           

        }
        //--------------------------------------------------------------------------------------





        #endregion
        #region "DGV-ITEMS"

        //NUEVO
        private void lnkItemNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            itemEnable(true);
            itemLimpia();
            txtItemNombre.Focus();

        }



        //EVENT DGV
        private void dgvItems_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 1)
            {
                itemFill();
            }
        }
        //---------------------------------------------------------------------------------

        //EDITAR
        private void lnkItemEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgvItems.SelectedRows.Count == 0)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                return;
            }


            lnkItemNew_LinkClicked(this, new LinkLabelLinkClickedEventArgs(null));

            itemFill();


        }
        //BORRAR
        private void lnkItemDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (dgvItems.SelectedRows.Count == 0)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                return;
            }

            DialogResult myResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroUnico);

            if (myResul == System.Windows.Forms.DialogResult.Yes)
            {
                dgvItems.Rows.Remove(dgvItems.CurrentRow);

                oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbZgItems, true);
                populate();
            }
        }
        //------------------------------------------------------------------------------------------
        //GUARDAR
        private void lnkItemSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (! this.ValidateChildren())
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosNoValidos);
                return;
            }
           

            if (string.IsNullOrEmpty(lblItemId.Text))
            {


                if (oExtensionString.isNombreSolucionValido(txtItemNombre.Text))
                {
                    //NEVO
                    dsBd.tbZgItemsRow myRow = oSingletonDsBd.getInstance.dataset.tbZgItems.NewtbZgItemsRow();
                    myRow.id = Guid.NewGuid();
                    myRow.nombre = txtItemNombre.Text.Trim();
                    myRow.descripcion = txtItemDescripcion.Text;
                    myRow.prohibir = chkItemProhibirPaso.Checked;
                    myRow.imageNombre = getFileName(imgBicho.ImageLocation);
                    myRow.valoracion = (short)traValoracion.Value;

                    myRow.idzgclasificaciones = (Guid)dgvClasificaciones.CurrentRow.Cells[oSingletonDsBd.getInstance.dataset.tbZgClasificaciones.idColumn.ColumnName].Value;

                    oSingletonDsBd.getInstance.dataset.tbZgItems.AddtbZgItemsRow(myRow);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiNombreConCaracteresEspeciales);
                }
            }

            else
            {
                //EDITAR
                Guid rowId = new Guid(lblItemId.Text);

                dsBd.tbZgItemsRow myRow = oSingletonDsBd.getInstance.dataset.tbZgItems.FindByid(rowId);

                if (oExtensionString.isNombreSolucionValido(txtItemNombre.Text))
                {
                    if (myRow.nombre != txtItemNombre.Text.Trim())
                    {
                        myRow.nombre = txtItemNombre.Text;
                    }

                    if (myRow.descripcion != txtItemDescripcion.Text.Trim())
                    {
                        myRow.descripcion = txtItemDescripcion.Text;
                    }

                    if (myRow.prohibir != chkItemProhibirPaso.Checked)
                    {
                        myRow.prohibir = chkItemProhibirPaso.Checked;
                    }


                    if (myRow.valoracion != traValoracion.Value)
                    {
                        myRow.valoracion = (short)traValoracion.Value;
                    }
                    if (myRow.imageNombre != getFileName(imgBicho.ImageLocation))
                    {
                        CopiarArchivo(imgBicho.ImageLocation);
                        myRow.imageNombre = getFileName(imgBicho.ImageLocation);

                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiNombreConCaracteresEspeciales);
                }

            }

            this.ValidateChildren();

            oSingletonDsBd.getInstance.saveDataTable(oSingletonDsBd.getInstance.dataset.tbZgItems, true);
            populate();

            itemEnable(false);


            this.Refresh();


        }
        private void CopiarArchivo(string origen)
        {
            string rutaOrigen = origen;
            string carpetaDestino = oTadil.data.Files.folderImgGis;

            // Asegura que la carpeta de destino exista
            Directory.CreateDirectory(carpetaDestino);

            // Mantiene el nombre original y compone la ruta destino de forma segura
            string nombreArchivo = Path.GetFileName(rutaOrigen);
            string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

            // Copia y sobrescribe si ya existe
            File.Copy(rutaOrigen, rutaDestino, overwrite: true);
        }
        //CANCELAR
        private void lnkItemCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            itemEnable(false);
            itemLimpia();
        }
        //-----------------------------------------------------------------------------------------
        //FILL
        private void itemFill()
        {
            DataGridViewRow iDgvRow = dgvItems.CurrentRow;
            lblItemId.Text = iDgvRow.Cells[0].Value.ToString();
            txtItemNombre.Text = iDgvRow.Cells[oSingletonDsBd.getInstance.dataset.tbZgItems.nombreColumn.ColumnName].Value.ToString();
            txtItemDescripcion.Text = iDgvRow.Cells[oSingletonDsBd.getInstance.dataset.tbZgItems.descripcionColumn.ColumnName].Value.ToString();
            traValoracion.Value = Convert.ToInt16(iDgvRow.Cells[oSingletonDsBd.getInstance.dataset.tbZgItems.valoracionColumn.ColumnName].Value);
            txtValoracion.Text = traValoracion.Value.ToString();
            chkItemProhibirPaso.Checked = (bool)iDgvRow.Cells[oSingletonDsBd.getInstance.dataset.tbZgItems.prohibirColumn.ColumnName].Value;
            imgBicho.ImageLocation = oTadil.data.Files.folderImgGis + @"\" + iDgvRow.Cells[oSingletonDsBd.getInstance.dataset.tbZgItems.imageNombreColumn.ColumnName].Value.ToString();
        }
        //CLEAR
        private void itemLimpia()
        {
            lblItemId.Text = string.Empty;
            txtItemNombre.Text = string.Empty;
            txtItemDescripcion.Text = string.Empty;
            chkItemProhibirPaso.Checked = false;
            imgBicho.ImageLocation = string.Empty;
            txtValoracion.Text = "0";
            traValoracion.Value = 0;
        }
        //ENABLE
        private void itemEnable(bool iEstado)
        {

            txtItemNombre.Enabled = iEstado;
            txtItemDescripcion.Enabled= iEstado;
            chkItemProhibirPaso.Enabled = iEstado;
            imgBicho.Enabled = iEstado;
            traValoracion.Enabled = iEstado;
            lnkImgUp.Enabled = iEstado;

            lnkItemCancel.Visible = iEstado;
            lnkItemSave.Visible = iEstado;

            lnkItemNew.Visible = !iEstado;
            lnkItemEdit.Visible = !iEstado;
            lnkItemDelete.Visible = !iEstado;

           

        }
        //-----------------------------------------------------------------------------------------
        //TRACK VALOR
        private void traValoracion_Scroll(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(traValoracion, strFrmGisGeneral.uiValoracion + " : "  + traValoracion.Value.ToString());
            txtValoracion.Text = traValoracion.Value.ToString();         
        }
        //IMAGEN UP
        private void lnkImgUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                imgBicho.ImageLocation = oTadil.data.Files.getFileImagenGis();
            }
            catch (oExFileNotFoundImg ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        //-------------------------------------------------------------------------------------------




        #endregion
        #region "VALIDACIONES"
        private ErrorProvider myError1 = new ErrorProvider();
        private ErrorProvider myError2 = new ErrorProvider();
        private ErrorProvider myError3 = new ErrorProvider();
        private ErrorProvider myError4 = new ErrorProvider();
        private void txtClaDes_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text) && ((TextBox)sender).Enabled)
            {
                myError1.SetError((Control)sender, strGeneralUser.uiDatoNoValido);
                e.Cancel = true;
            }
            else
            {
                myError1.Clear();
                e.Cancel = false;
            }
        }
        private void txtClaNombre_Validating(object sender, CancelEventArgs e)
        {
          
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text) && ((TextBox)sender).Enabled)
            {
                myError2.SetError((Control)sender, strGeneralUser.uiDatoNoValido);
                e.Cancel = true;
            }
            else
            {
            
                myError2.Clear();
                e.Cancel = false;
            }
        }
        private void txtItemDescripcion_Validating(object sender, CancelEventArgs e)
        {
            

            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text) && ((TextBox)sender).Enabled)
            {
                myError3.SetError((Control)sender, strGeneralUser.uiDatoNoValido);
                e.Cancel = true;
            }
            else
            {
                myError3.Clear();
                e.Cancel = false;
            }
        }
        private void txtItemNombre_Validating(object sender, CancelEventArgs e)
        {

           


            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text) && ((TextBox)sender).Enabled)
            {
                myError4.SetError((Control)sender, strGeneralUser.uiDatoNoValido);
                e.Cancel = true;
            }
            else
            {
                myError4.Clear();
                e.Cancel = false;
            }

        }
        #endregion
        #region "BOTONES ZONAS-CAD"

        private void btnZonaLink_Click(object sender, EventArgs e)
        {

            try
            {

                if (dgvItems.SelectedRows.Count == 0)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                    return;
                }

                frmBb.getInstance.Hide();

                Guid rowId = (Guid)dgvItems.CurrentRow.Cells[oSingletonDsBd.getInstance.dataset.tbZgItems.idColumn.ColumnName].Value;

                oZonaGis miZona = new oZonaClaImg(rowId);

                miZona.link();

            }
            catch (oExFileNotFoundImg ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
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

        private void frmZonaGis_Shown(object sender, EventArgs e)
        {
            dgvClasificaciones.ClearSelection();
            dgvItems.ClearSelection();
        }

        private void chkItemProhibirPaso_CheckedChanged(object sender, EventArgs e)
        {
            if (chkItemProhibirPaso.Checked)
            {
                txtValoracion.Text = "10";
                traValoracion.Value = 10;
            }
        }
    }
}
