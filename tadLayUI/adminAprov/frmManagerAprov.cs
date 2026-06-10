using engNet.eventos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tadLayLan.Tdb;
using tadLayLogica;
using tadLayLogica.datos.BaseDatos;
using tadLayUI.adminGis;
using tadLayUI.adminSecciones;

namespace tadLayUI.adminAprov
{
    public partial class frmManagerAprov : Form
    {
        /// <summary>
        /// DataSet Precios
        /// </summary>
        private oSingletonDsBd mDs;


        private Form mFormDetail = null;
        public frmManagerAprov()
        {
            InitializeComponent();
            postConstructor();
        }
        public void display()
        {
            mDs = oSingletonDsBd.getInstance;
            ucTree_evNodeCurrent(ucTree, new oEventArgs<string, string>("padre", "Adecuación"));
            this.Show();
        }
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
        private void ucTree_evNodeCurrent(object sender, oEventArgs<string, string> e)
        {

            //Llamada al Validador de Licencia
            //cambiado juanma 250117
            //ol.e(oTadil.IsDemo, oTadil.IsTmp);

            if (mFormDetail != null)
            {
                mFormDetail.Close();
                mFormDetail = null;
            }

            //Traduccion


            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.BackColor = Color.FromKnownColor(KnownColor.Control);
            splitContainer1.Panel2.BackgroundImage = null;

            //DATOS
            if (e.Value == "padre")
            {
                mFormDetail = new frmAdecacionManager();
                ((IfrmManagerPopulate)mFormDetail).populate();
            }

            if (mFormDetail != null)
            {
                mFormDetail.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                mFormDetail.TopLevel = false;

                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.Controls.Add(mFormDetail);

                mFormDetail.Show();
            }
            else
            {
                splitContainer1.Panel2.Controls.Clear();
            }


        }

        #endregion
    }
}
