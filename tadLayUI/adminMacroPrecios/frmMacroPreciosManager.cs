using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.adminMacroPrecios

{

    using engNet;
    using engNet.eventos;
    using tadLayLogica;
    using tadLayLogica.datos;
    using tadLayData;
    using tadLayLan;
    using tadLayUI.adminGis;
    using engNex.Net.BussinesObject.Licencia;
    using tadLayLogica.datos.BaseDatos;
    
    /// <summary>
    /// Administrador de Precios
    /// frmPrecios.getInstance.create().Show();
    /// frmPrecios.getInstance.edit(iFile).Show();
    /// </summary>
    public partial class frmMacroPreciosManager : frmRoot
    {
             

        /// <summary>
        /// DataSet Precios
        /// </summary>
        private oSingletonDsBd mDs;
        private frmMacroPreciosDatagrid mFrmDataGrid = null;


        #region "Constructores"

        public frmMacroPreciosManager()
        {
            InitializeComponent();
            postConstructor();
            
        }
        #endregion


        #region "PUBLICOS"

        public void display()
        {
            mDs = oSingletonDsBd.getInstance;
            ucTree_evNodeCurrent(ucTree, new oEventArgs<string, string>("CALUNI", "UNIGEN"));
            this.Show();
        }

        #endregion


        #region "PRIVADOS"

        private  void  postConstructor()
        {
            //Fill Arbol
            ucTree.populate();

            //Evento Tree Obtengo Los Codigos de los Formularios
            ucTree.evNodeCurrentCode += new EventHandler<oEventArgs<string, string>>(ucTree_evNodeCurrent);

            //Evento la Ruta del Nodo
            ucTree.evNodeCurrentPath += new EventHandler<oEventArgs<string>>(ucTree_evNodeCurrentPath);

            //Traduccion
            lblHeader.Text = strFrmMacroPrecios.uiAdminMacroPrecios;
            lblDetail.Text = string.Empty;

            //Creo la Instancia del Formulario
            
            mFrmDataGrid = new frmMacroPreciosDatagrid();
            mFrmDataGrid.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            mFrmDataGrid.TopLevel = false;
            splitContainer1.Panel2.Controls.Add(mFrmDataGrid);
        }

        #endregion

        #region "EVENTOS FRM"
     
        /// <summary>
        /// EVENTO LANZADO ARBOL --> RUTA COMPLETA DEL NODO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ucTree_evNodeCurrentPath(object sender, oEventArgs<string> e)
        {
            lblDetail.Text = e.Value;
        }
        /// <summary>
        ///EVENTO LANZADO ARBOL --> GRUPO - CLASIFICACION
        /// </summary>
       private  void ucTree_evNodeCurrent(object sender,oEventArgs<string, string> e)
        {

            //Llamada al Validador de Licencia
            //cambiado juanma 250117
            //ol.e(oTadil.IsDemo, oTadil.IsTmp);
            if (e.Value == string.Empty && e.Value2 == string.Empty)
            {
                mFrmDataGrid.Hide();    
            }
            else
            {
                mFrmDataGrid.populateByIdRoadTipo(e.Value2);
                mFrmDataGrid.Show();
            }
        }

        #endregion





















      



       















   
 




    }
}
