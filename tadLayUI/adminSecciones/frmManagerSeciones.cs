using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.adminSecciones

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
    public partial class frmManagerSecciones : Form
    {
             

        /// <summary>
        /// DataSet Precios
        /// </summary>
        private oSingletonDsBd mDs;


        private Form mFormDetail = null;


        #region "Constructores"

        public frmManagerSecciones()
        {
            InitializeComponent();
            postConstructor();
            
        }
        #endregion


        #region "PUBLICOS"

        public void display()
        {
            mDs = oSingletonDsBd.getInstance;
            ucTree_evNodeCurrent(ucTree, new oEventArgs<string, string>("CUNETA", "TRIANG"));
            this.Show();
        }

        #endregion


        #region "PRIVADOS"

        private void postConstructor()
        {
            //Header
            var name = oTadil.KAppHeaderName;
            this.Text = name;


            ucTree.populate();

            //Evento Tree Obtengo Los Codigos de los Formularios
            ucTree.evNodeCurrentCode += new EventHandler<oEventArgs<string, string>>(ucTree_evNodeCurrent);

            //Evento la Ruta del Nodo
            ucTree.evNodeCurrentPath += new EventHandler<oEventArgs<string>>(ucTree_evNodeCurrentPath);


            //Traduccion
            lblHeader.Text = strFrmSecciones.uiAdminSecciones;
            lblDetail.Text = string.Empty;

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

            if (mFormDetail != null)
            {
                mFormDetail.Close();
            }


           //CUNETAS
            if (e.Value == "CUNETA")
            {

                if (e.Value2 == "TRIANG")
                {
                    mFormDetail = new frmCunTriManager();
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else if (e.Value2 == "TRAPEZ")
                {
                    mFormDetail = new frmCunTraManager();
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else
                {
                    throw new NotImplementedException(e.Value2);
                
                }
            }

           //CALZADA UNICA
           if (e.Value == "CALUNI")
           {

               if (e.Value2 == "UNIGEN")
               {
                   mFormDetail = new frmRoadUnicaGenManager();
                   ((IfrmManagerPopulate)mFormDetail).populate();
               }
               
               else
               {
                   throw new NotImplementedException(e.Value2);
               }    
           
           }


           //CALZADA DOBLE
           if (e.Value == "CALDOB")
           {

               if (e.Value2 == "DOBAUT") //AUTOVIA
               {
                   mFormDetail = new frmRoadDobleAutoviaManager();
                   ((IfrmManagerPopulate)mFormDetail).populate();
               }
               else if (e.Value2 == "DOBSIN") //SIN MEDIANA
               {
                   mFormDetail = new frmRoadDobleSinMedianaManager();
                   ((IfrmManagerPopulate)mFormDetail).populate();
               }

               else
               {
                   throw new NotImplementedException(e.Value2);
               }

           }


            mFormDetail.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            mFormDetail.TopLevel = false;

            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.Controls.Add(mFormDetail);

            mFormDetail.Show();
           
                
        }

        #endregion
    }
}
