using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.adminPresupuesto
{

    using engNet;
    using engNet.eventos;
    using tadLayLogica;
    using tadLayLogica.datos.BaseDatos;
    using tadLayData;

    using tadLayLan;
    using engNex.Net.BussinesObject.Licencia;
 
    
    
    /// <summary>
    /// Administrador de Precios
    /// </summary>
    /// <remarks>SE AÑADIO EL UC UNIDAD MONETARIA --> SACAR A CONTROL EL EDITOR DE PRECiOS</remarks>
    public partial class frmManagerPrecios : Form
    {
       #region "Variables Privadas"
        /// <summary>
        /// ID de la Clasificación
        /// </summary>
        private short mIdClasificacion;
        /// <summary>
        /// Tiene Precios Secundarios
        /// </summary>
        private bool? mHasPrecioSecundario = null;

        private string miClas = "";
        private string miGrupo = "";


        #endregion
       #region "Constructores"
        public frmManagerPrecios()
        {
            InitializeComponent();
            postConstructor();       
        }
        #endregion
       #region "Metodos Publicos"

        public void display()
        {
            ucTreePrecios1_evNodeCurrent(ucTreePrecios1, new oEventArgs<string, string>("DES", "GEN"));
            this.Show();
        }
        #endregion
       #region "Eventos Formulario"
      
        /// <summary>
        /// GRUPO - CLASIFICACION
        /// </summary>
       private  void ucTreePrecios1_evNodeCurrent(object sender, engNet.eventos.oEventArgs<string, string> e)
        {
            //Llamada al Validador de Licencia
            //cambiado juanma 250117
            //ol.e(oTadil.IsDemo, oTadil.IsTmp);

            //Selecciono Nodo Padre
            if (e.Value == "null")
            {
                dgvData.DataSource = null;
                grItems.Enabled = false;
                grItem.Enabled = false;
                ucUnidad.uitxt = string.Empty;
            }
            else
            {                  
                dgvDataPopulate(e.Value, e.Value2);
            }
            
        }
       private void dgvData_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 1)
            {
                fillItem();
            }
        }
       private void dgvData_DoubleClick(object sender, EventArgs e)
       {
           if (dgvData.SelectedRows.Count == 1)
           {
            lnkClaEdit_LinkClicked(this, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
           }
       }
       private void grItems_EnabledChanged(object sender, EventArgs e)
       {
           if (grItems.Enabled)
           {
               grItem.Enabled = false;
           }
           else
           {
               grItem.Enabled = true;
           }

       }
        #endregion
       #region "Botonera"
       //NUEVO
       private void lnkClaNew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {
           dgvData.ClearSelection();
           grItems.Enabled = false;
           resetItem();
           ucNombre.textbox.Focus();
       }
       //EDITAR
       private void lnkClaEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {

           if (dgvData.CurrentRow == null)
           {
               oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
               return;
           }

           grItems.Enabled = false;
           fillItem();
           ucNombre.textbox.Focus();

       }
       //BORRAR REGISTRO
       private void lnkClaDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {

           try
           {

               if (dgvData.CurrentRow == null)
               {
                   oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                   return;
               }

               DialogResult myResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroUnico);

               if (myResul == System.Windows.Forms.DialogResult.Yes)
               {
                   dgvData.Rows.Remove(dgvData.CurrentRow);

                   oSingletonDsBd.getInstance.save(true);

                   resetItem();
               }

           }

           catch (InvalidConstraintException)
           {

               oTadil.data.UserInfo.showInfo(strFrmPrecios.uiMaterialBorrarNoPermitido);
           }

           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }
           
           

       }
       //GUARDAR REGISTRO
       private void lnkClaSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {


           try
           {

               //Validar los Datos del Grupo
               if (oValidar.isValidoGrupo(grItem))
               {

                   //NUEVO
                   if (dgvData.SelectedRows.Count == 0)
                   {

                       dsBd.tbMaterialesRow myRow = oSingletonDsBd.getInstance.dataset.tbMateriales.NewtbMaterialesRow();
                       myRow.idMaterial = Guid.NewGuid();
                       myRow.nombre = ucNombre.uitxt;
                       myRow.precioPrincipal = ucPrecioPrincipal.valorDouble;

                       if (mHasPrecioSecundario.Value)
                       {
                           myRow.precioSecundario = ucPrecioSecundario.valorDouble;
                       }

                       myRow.descripcion = ucDescripcion.uitxt;
                       myRow.idClasificacion = mIdClasificacion;

                      oSingletonDsBd.getInstance.dataset.tbMateriales.AddtbMaterialesRow(myRow);

                   }
                   //EDITAR
                   else
                   {

                       Guid miIdMaterial = (Guid)dgvData.CurrentRow.Cells["idMaterial"].Value;

                       dsBd.tbMaterialesRow myRow = oSingletonDsBd.getInstance.dataset.tbMateriales.FindByidMaterial(miIdMaterial);

                       myRow.nombre = ucNombre.uitxt.Trim();
                       myRow.precioPrincipal = ucPrecioPrincipal.textbox.valorDouble;

                       if (mHasPrecioSecundario.Value)
                       {
                           myRow.precioSecundario = ucPrecioSecundario.textbox.valorDouble;
                       }
                       else
                       {
                           myRow.precioSecundario = -1;
                       }

                       myRow.descripcion = ucDescripcion.uitxt;


                   }

                   oSingletonDsBd.getInstance.save(true);
                   dgvDataPopulate(this.miGrupo, this.miClas);

                   grItems.Enabled = true;

                   lnkClaNew.Focus();
               }
               else
               {
                   oTadil.data.UserInfo.showInfo(strError.eValidacion);
               }

           }

           catch (ConstraintException)
           {

               oTadil.data.UserInfo.showInfo(strFrmPrecios.uiMaterialNombreDuplicado);
           }


           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }





       }
       //CANCELAR
       private void lnkClaCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {
           grItems.Enabled = true;
           resetItem();
       }
       #endregion
       #region "Metodos Privados"
       private void postConstructor()
       {
           //Header
           var name = oTadil.KAppHeaderName;
           this.Text = name;

           ucTreePrecios1.populate();

           //Evento Tree
           ucTreePrecios1.evNodeCurrentCode += new EventHandler<engNet.eventos.oEventArgs<string, string>>(ucTreePrecios1_evNodeCurrent);

           //Solo Lectura
           ucMoneda.textbox.ReadOnly = true;
           ucUnidad.textbox.ReadOnly = true;

           //Grupo Item
           grItems.Enabled = false;
           grItem.Enabled = false;
           ucNombre.textbox.MaxLength = 50;
           ucNombre.textbox.isTexto = true;
           ucPrecioSecundario.Visible = false;


           //DgvSetUp
           dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
           dgvData.EditMode = DataGridViewEditMode.EditProgrammatically;
           dgvData.AllowUserToAddRows = false;
           dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
           dgvData.MultiSelect = false;
           dgvData.AllowUserToResizeRows = false;
           dgvData.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
           dgvData.AlternatingRowsDefaultCellStyle.BackColor = Color.Snow;

           //Precios

           this.ucUnidadMonetaria1.populate();

           traduccion();

       }
       private void traduccion()
       {
           //Cabezado
           lblFrm.Text = strFrmPrecios.uiLblFrm;
           lblGrupo.Text = string.Empty;
           lblClasificacion.Text = string.Empty;

           //Grupos
           grUds.Text = strFrmPrecios.uiGrUnidades;
           grItems.Text = strFrmPrecios.uiGrListado;
           grItem.Text = strFrmPrecios.uiGrDetalle;

           //Grupo Unidades
           ucMoneda.uiLbl = strFrmPrecios.uiMoneda;
           ucUnidad.uiLbl = strFrmPrecios.uiPrecioPor;


           //Grupo Listado
           lnkClaNew.Text = strGeneral.uiNuevo;
           lnkClaEdit.Text = strGeneral.uiEditar;
           lnkClaDelete.Text = strGeneral.uiBorrar;
           lnkClaSave.Text = strGeneral.uiGuardar;
           lnkClaCancel.Text = strGeneral.uiCancelar;

           //Grupo Item
           ucNombre.uiLbl = strFrmPrecios.uiNombre;
           ucDescripcion.uiLbl = strFrmPrecios.uiDescripcion;


       }




       private void dgvDataPopulate(string iIdGrupo, string iClasCode)
       {
           this.miClas = iClasCode;
           this.miGrupo = iIdGrupo;
           //SetUp Cabezera
           lblGrupo.Text = strFrmPrecios.ResourceManager.GetString("ui" + iIdGrupo);
           lblClasificacion.Text = strFrmPrecios.ResourceManager.GetString("ui" + iClasCode);
          
           //SHOW CONTROL UNIDADES MONETARIAS
           if (iIdGrupo=="UDS" && iClasCode== "MON")
           {

               this.grEditorPrecios.Visible = false;
               this.grEditorPrecios.Enabled = false;
               this.grEditorPrecios.SendToBack();

               this.ucUnidadMonetaria1.Visible = true;
               this.ucUnidadMonetaria1.Enabled = true;             
           }
           else
           {

            this.ucUnidadMonetaria1.Visible = false;
            this.ucUnidadMonetaria1.Enabled = false;
            this.ucUnidadMonetaria1.SendToBack();
 
            this.grEditorPrecios.Visible = true;
            this.grEditorPrecios.Enabled = true;
            this.grItems.Enabled = true;

            ucMoneda.textbox.Text = oSingletonDsBd.getInstance.dataset.tbMoneda[0].simbolo;
           
            string miQuery = "idGrupo  like  '{0}%' AND code like '{1}%'";

           //Vinculo los DATAGRIDVIEW


            BindingSource miMaster = new BindingSource();
            BindingSource myDetail = new BindingSource();
           miMaster.DataMember = oSingletonDsBd.getInstance.dataset.tbClasificaciones.TableName;
           miMaster.Filter = string.Format(miQuery, iIdGrupo, iClasCode);
           miMaster.DataSource = oSingletonDsBd.getInstance.dataset.tbClasificaciones;

           DataRowView miRow = (DataRowView)miMaster.Current;

           mIdClasificacion = (short)miRow[0];

           mHasPrecioSecundario = oSingletonDsBd.getInstance.dataset.tbClasificaciones.FindByidClasificacion(mIdClasificacion).hasPrecioSecundario;

           myDetail.DataSource = miMaster;
           myDetail.DataMember = "FK_tbClasificaciones_tbItems";

           dgvData.DataSource = myDetail;

           ucUnidad.uitxt = oSingletonDsBd.getInstance.dataset.tbClasificaciones.FindByidClasificacion(mIdClasificacion).tbGruposRow.idUd;

           //Edito las Columnas
           dgvData.Columns[0].Visible = false;
           dgvData.Columns[1].Visible = true; //Nombre
           dgvData.Columns[2].Visible = true; //Precio 
           dgvData.Columns[3].Visible = mHasPrecioSecundario.Value; //Precio Secundario           
           dgvData.Columns[4].Visible = false;
           dgvData.Columns[5].Visible = true;


           if (mHasPrecioSecundario.Value)
           {
               ucPrecioSecundario.Visible = true;
               ucPrecioSecundario.isObligatorio = true;
               ucPrecioSecundario.isTexto = false;
           }
           else
           {
               ucPrecioSecundario.Visible = false;
               ucPrecioSecundario.isObligatorio = false;
           }


           setUpPrecioTipo(iIdGrupo);

 
           dgvData.ClearSelection();


           resetItem();

           }

       }
  
       private void setUpPrecioTipo (string iIdGrupo)
       {
           if (iIdGrupo == "EXC")
           {
               ucPrecioPrincipal.uiLbl = strFrmPrecios.uiPrecioEmpleo;
               ucPrecioSecundario.uiLbl = strFrmPrecios.uiPrecioVertedero;
               dgvData.Columns[3].HeaderText = strFrmPrecios.uiPrecioVertedero;
           }
           else if (iIdGrupo == "REL")
           {
               ucPrecioPrincipal.uiLbl = strFrmPrecios.uiPrecioEmpleo;
               ucPrecioSecundario.uiLbl = strFrmPrecios.uiPrecioPrestamo;
               dgvData.Columns[3].HeaderText = strFrmPrecios.uiPrecioPrestamo;
           }
           else if (iIdGrupo == "VPS")
           {
               ucPrecioPrincipal.uiLbl = strFrmPrecios.uiVPS;
           }
           else if (iIdGrupo == "VES")
           {
               ucPrecioPrincipal.uiLbl = strFrmPrecios.uiVES;
           }
           else
           {
              ucPrecioPrincipal.uiLbl = strFrmPrecios.uiPrecio;
           }
     

           dgvData.Columns[1].HeaderText = strFrmPrecios.uiNombre;
           dgvData.Columns[2].HeaderText = ucPrecioPrincipal.uiLbl;
           dgvData.Columns[5].HeaderText = strFrmPrecios.uiDescripcion;

       }
       private void fillItem()
       {
           DataGridViewRow miDgvClaRow = dgvData.CurrentRow;
           ucNombre.uitxt = (string)miDgvClaRow.Cells["nombre"].Value;
           ucPrecioPrincipal.uitxt = miDgvClaRow.Cells["precioPrincipal"].Value.ToString();
           ucPrecioSecundario.uitxt = miDgvClaRow.Cells["precioSecundario"].Value.ToString();
           ucDescripcion.uitxt = miDgvClaRow.Cells["descripcion"].Value.ToString();
       }
       private void resetItem()
       {
           ucNombre.uitxt = string.Empty;
           ucPrecioPrincipal.uitxt = string.Empty;
           ucPrecioSecundario.uitxt = string.Empty;
           ucDescripcion.uitxt = string.Empty;
       }
       #endregion


    }
}
