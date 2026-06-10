using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminValoracion
{
    using tadLayData;
    using tadLayLogica;
    using tadLayLogica.datos.proyecto;
    using tadLayLan;

    using tadLayLogica.logica.valoracion;
    using tadLayLan.Tdi;


    public partial class frmMatrizDecisionManager : frmRoot
    {

        private BindingSource mBindMaster;


        #region "Constructores"
        public frmMatrizDecisionManager()
        {
            InitializeComponent();
        }
        #endregion
        #region "Botones"

        //NUEVO
        void lnkNew_Click(object sender, EventArgs e)
        {
            frmMatrizDecisionDetail miFrmDetail = new frmMatrizDecisionDetail(mBindMaster);

            miFrmDetail.itemNew();

            miFrmDetail.ShowDialog();
        }

        //EDITAR
        void lnkEdit_Click(object sender, EventArgs e)
        {
            if (ucDgvMatriz.CurrentRow != null)
            {
                int miId = (int)ucDgvMatriz.CurrentRow.Cells["id"].Value;

                frmMatrizDecisionDetail miFrmDetail = new frmMatrizDecisionDetail(mBindMaster);

                miFrmDetail.itemEdit(miId);

                miFrmDetail.ShowDialog();
            }
            else
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
            }
        }

        //ERASE
        void lnkErase_Click(object sender, EventArgs e)
        {

            if (ucDgvMatriz.SelectedRows.Count == 0)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
            }
            else
            {
                DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiBorrarRegistroUnico);

                if (miResul == DialogResult.Yes)
                {
                    mBindMaster.RemoveCurrent();

                    ds.saveDataTable(ds.dataset.tbMatrizDecision, true);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
                }

            }
        }

        #endregion
        #region "Eventos DatagridView"

        void ucDgvMatriz_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                lnkEdit_Click(null, new EventArgs());
            }
        }

        #endregion
        #region "Evento FRM"
        private void frmMatrizDecisionManager_Load(object sender, EventArgs e)
        {


            //Traduccion
            this.Text = strFrmValoracion.uiVALMAT;
            this.grDgvMatrizDecision.Text = strFrmValoracion.uiGrPonderacionValoraciones;
            this.btnValorarAlternativas.Text = strFrmValoracion.uiValorarSolucionesPorHipotesis;


            //Cargo los Valores PorDefecto Si No Existen
            oDalValoracion.addMatrizValoracionDefault();

            //DataBind
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = ds.dataset.tbMatrizDecision.TableName;
            mBindMaster.DataSource = ds.dataset.tbMatrizDecision;

            //Formateo el DGV
            ucDgvMatriz.DataSource = mBindMaster;
            ucDgvMatriz.dgvSetUpUIDefault(true);
            ucDgvMatriz.CellDoubleClick += new DataGridViewCellEventHandler(ucDgvMatriz_CellDoubleClick);
            ucDgvMatriz.dgvColumnsHide(new int[] { 0, 2 });

            ucDgvMatriz.Columns[1].MinimumWidth = 100;
            ucDgvMatriz.Columns[1].HeaderText = strFrmValoracion.uiDgvHipotesis;
            ucDgvMatriz.Columns[1].ToolTipText = strFrmValoracion.uiDgvHipotesis;

            ucDgvMatriz.Columns[3].HeaderText = "TRA";
            ucDgvMatriz.Columns[3].ToolTipText = strFrmValoracion.uiDgvTrazado;

            ucDgvMatriz.Columns[4].HeaderText = "GEO";
            ucDgvMatriz.Columns[4].ToolTipText = strFrmValoracion.uiDgvGeotecnia;

            ucDgvMatriz.Columns[5].HeaderText = "ETM";
            ucDgvMatriz.Columns[5].ToolTipText = strFrmValoracion.uiDgvEstructurasTunelesMuros;

            ucDgvMatriz.Columns[6].HeaderText = "MED";
            ucDgvMatriz.Columns[6].ToolTipText = strFrmValoracion.uiDgvMedioambiental;

            ucDgvMatriz.Columns[7].HeaderText = "CLI";
            ucDgvMatriz.Columns[7].ToolTipText = strFrmValoracion.uiDgvClimatica;

            ucDgvMatriz.Columns[8].HeaderText = "SOC";
            ucDgvMatriz.Columns[8].ToolTipText = strFrmValoracion.uiDgvSocioEconomicas;


            ucDgvMatriz.Columns[9].HeaderText = "PAT";
            ucDgvMatriz.Columns[9].ToolTipText = strFrmValoracion.uiDgvPatrimoniales;


            ucDgvMatriz.Columns[10].HeaderText = "ECO";
            ucDgvMatriz.Columns[10].ToolTipText = strFrmValoracion.uiDgvEconomicas;


            DataGridViewCellStyle miFormato = new DataGridViewCellStyle();
            miFormato.Alignment = DataGridViewContentAlignment.MiddleCenter;
            miFormato.Font = new Font("Consola", 9F, FontStyle.Regular);

            foreach (DataGridViewColumn column in ucDgvMatriz.Columns)
            {
                column.HeaderCell.Style = miFormato;

            }


            //Formateo La Barra Herramientas
            ucToolDgv1.lnkNew.Click += new EventHandler(lnkNew_Click);
            ucToolDgv1.lnkEdit.Click += new EventHandler(lnkEdit_Click);
            ucToolDgv1.lnkErase.Click += new EventHandler(lnkErase_Click);


            //Rellenar el Grid de las Soluciones
            grLstSolucionesValorar.Text = strFrmValoracion.uiGrValoracionSoluciones;

            ucDgvSolucionesValorar1.populate();

        }
        private void frmMatrizDecisionManager_Shown(object sender, EventArgs e)
        {
            ucDgvMatriz.ClearSelection();
            ucDgvSolucionesValorar1.clearSelection();
        }
        #endregion

        private void btnValorarAlternativas_Click(object sender, EventArgs e)
        {
            try
            {

                #region "Validaciones"

                if (ucDgvMatriz.SelectedRows.Count == 0)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiSelHipotesis);

                    return;
                }

                if (ucDgvSolucionesValorar1.lstSolucionesValorar.Count() <= 1)
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiSel2Soluciones);

                    return;
                }

                #endregion

                DataRowView miDataRowView = (DataRowView)mBindMaster.Current;

                dsApp.tbMatrizDecisionRow miRowTipado = (dsApp.tbMatrizDecisionRow)miDataRowView.Row;

                Cursor.Current = Cursors.WaitCursor;

                oValoracionListadoSoluciones miValoracionSoluciones = new oValoracionListadoSoluciones(miRowTipado, ucDgvSolucionesValorar1.lstSolucionesValorar);

                Cursor.Current = Cursors.Default;


                frmMatrizDecisionSolucion miFrmSolucionesPorHipotesis = new frmMatrizDecisionSolucion(miValoracionSoluciones.rootHipotesis);

                miFrmSolucionesPorHipotesis.ShowDialog();


            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

            }
            


        }

    }
}
