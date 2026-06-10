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

    using tadLayLan;
    using tadLayLogica.logica.valoracion;
    using tadLayLogica;
    using tadLayLogica.informes.Valoracion;
    using tadLayLan.Tdi;
    
    public partial class frmMatrizDecisionSolucion : Form
    {
        #region "Variables Privadas"
        private oCompositeValoracionHipotesis mRoot = null;
        private oCompositeValoracionSolucion mSolucion = null;
        #endregion
        #region "Constructor"
        public frmMatrizDecisionSolucion(oCompositeValoracionHipotesis iCompositeHipotesis)
        {
            InitializeComponent();

            mRoot = iCompositeHipotesis;

            postConstructor();

        }
        #endregion
        #region "Metodos Privados"
        private void postConstructor()
        {

            //Traduccion
            this.Text = strFrmValoracion.uiFrmHipotesisSoluciones;
            this.grHipotesis.Text = strFrmValoracion.uiHipotesis;
            this.ucNombre.uiLbl = strGeneral.uiNombre;

            this.grLstSoluciones.Text = strFrmValoracion.uiGrLstSolucionesValoradas;
            this.grLstCapitulos.Text = strFrmValoracion.uiGrLstCapitulosValorados;
            this.grLstDetalle.Text = strFrmValoracion.uiGrLstSubcapitulosValorados;

            this.grInforme.Text = strFrmInformes.uiInformes;
            this.btnInformeDetalle.Text = strFrmInformes.uiValoracionHipotesisDetalle;
            this.btnInformeResumen.Text = strFrmInformes.uiValoracionHipotesisResumen;

            //Load Datos
            this.ucNombre.uitxt = mRoot.nombre;
            this.ucNombre.textbox.Enabled = false;

            //Configuramos el primer DataGridView
            List<IValoracion> miLstSoluciones = mRoot.lstChild.OrderBy(p => p.notaGlobal).ToList();

            //DataGridView Solucion
            ucDgvSoluciones.dgvSetUpUIDefault(true);
            ucDgvSoluciones.DataSource = miLstSoluciones;
            ucDgvSoluciones.Columns[1].DefaultCellStyle.Format = "n2";
            ucDgvSoluciones.Columns[2].DefaultCellStyle.Format = "n2";
            ucDgvSoluciones.dgvColumnsHide(new int[] { 3 });

        }
        #endregion
        #region "Eventos Formulario"

        private void frmMatrizDecisionSolucion_Shown(object sender, EventArgs e)
        {
            ucDgvSoluciones.ClearSelection();
        }

        #endregion
        #region "Eventos DatagridView"

        /// <summary>
        /// DGV SOLUCIONES
        /// </summary>
        private void ucDgvSoluciones_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            string miSolucionNombre = ucDgvSoluciones.CurrentRow.Cells[0].Value.ToString().Trim();

            mSolucion = mRoot.getSolucionByName(miSolucionNombre);

            ucDgvCapitulos.dgvSetUpUIDefault(true);
            ucDgvCapitulos.DataSource = mSolucion.lstChild;
            ucDgvCapitulos.Columns[1].DefaultCellStyle.Format = "n2";
            ucDgvCapitulos.Columns[2].DefaultCellStyle.Format = "n2";
            ucDgvCapitulos.ClearSelection();
            ucDgvDetalle.DataSource = null;
        }


        /// <summary>
        /// DGV CAPITULOS
        /// </summary>
        private void ucDgvCapitulos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            string miCapituloName = ucDgvCapitulos.CurrentRow.Cells[0].Value.ToString().Trim();

            IValoracion miCapitulo = mSolucion.getCapitulos(miCapituloName);


            ucDgvDetalle.dgvSetUpUIDefault(true);
            ucDgvDetalle.DataSource = mSolucion.lstChild;
            ucDgvDetalle.Columns[1].DefaultCellStyle.Format = "n2";
            ucDgvDetalle.Columns[2].DefaultCellStyle.Format = "n2";
            ucDgvDetalle.DataSource = miCapitulo.lstChild;
            ucDgvDetalle.ClearSelection();





        }



        #endregion
        #region "Botones Informes"
        /// <summary>
        /// INFORME RESUMEN
        /// </summary>
        private void btnInformeResumen_Click(object sender, EventArgs e)
        {

            try
            {
                string miFile = oTadil.data.Files.saveAsFileCsvFromDialog("InformeValoracionAlternativas_Resumen_ " + mRoot.nombre);

                if (string.IsNullOrEmpty(miFile))
                {
                    return;
                }
                else
                {
                    oInfValoracionHipotesis.printResumen(mRoot, miFile);
                }
     
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        /// <summary>
        /// INFORME DETALLE
        /// </summary>
        private void btnInformeDetalle_Click(object sender, EventArgs e)
        {

            try
            {
                string miFile = oTadil.data.Files.saveAsFileCsvFromDialog("InformeValoracionAlternativas_Detalle_ " + mRoot.nombre);


                if (string.IsNullOrEmpty(miFile))
                {
                    return;
                }
                else
                {
                   oInfValoracionHipotesis.printDetalle(mRoot, miFile);
                }
      
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }


        }
        #endregion
    }
}
