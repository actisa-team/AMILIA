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
    using engNet.eventos;
    using tadLayLan;
    using tadLayLan.Tdb;
    using tadLayLogica;
    using tadLayUI.adminGis;

    using tadLayUI.adminGis.valoracion;
    using engNex.Net.BussinesObject.Licencia;
    
    
    public partial class frmManagerGis : Form
    {

      private  Form mFormDetail = null;
        
        
        public frmManagerGis()
        {
            InitializeComponent();
            setup_Load();
        }


        #region "Metodos Públicos"


        public void display()
        {                   
            ucTreeGis1.populate();
            ucTreeGis1_evNodeCurrent(ucTreeGis1, new oEventArgs<string,string>("null","null"));
            this.Show();
        }

        #endregion






  

        #region "Eventos"


        private void setup_Load()
        {
            var name = oTadil.KAppHeaderName;

            this.Text = name;
            lblHeader.Text = strFrmGisGeneral.uiHeader;
            lblZona.Text = string.Empty;
            ucTreeGis1.evNodeCurrent += new EventHandler<engNet.eventos.oEventArgs<string, string>>(ucTreeGis1_evNodeCurrent);      
        }


        /// <summary>
        /// Valor = ID ; Valor2 = CODE 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ucTreeGis1_evNodeCurrent(object sender, engNet.eventos.oEventArgs<string, string> e)
        {
            //Llamada al Validador de Licencia
            //ol.e(oTadil.IsDemo, oTadil.IsTmp);

            if (e.Value == "null")
            {
                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.BackColor = Color.White;
                splitContainer1.Panel2.BackgroundImageLayout = ImageLayout.Center;
                splitContainer1.Panel2.BackgroundImage = Properties.Resources.backTadilStr; 
 
            }
            else
            {

                //Traduccion
                lblZona.Text = strFrmGisGeneral.ResourceManager.GetString("ui" + e.Value2);
                
                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.BackColor = Color.FromKnownColor(KnownColor.Control);
                splitContainer1.Panel2.BackgroundImage = null;


                if (e.Value2 == "MOVTIE")
                {
                    mFormDetail = new frmGeoManager();
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else if (e.Value2 == "VAEXTA")
                {
                    mFormDetail = new frmValExcavabilidadTalud();
                }
                else if (e.Value2 == "ESTEST")
                {
                    mFormDetail = new frmEstManager();
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else if (e.Value2 == "ESTCIM")
                {
                    mFormDetail = new frmCimManager();
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else if (e.Value2 == "VALCIM")
                {
                    mFormDetail = new frmValCimentacion();
                }
                else if (e.Value2 == "TUNTUN")
                {
                    mFormDetail = new frmTunManager();
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else if (e.Value2 == "VALTUN")
                {
                    mFormDetail = new frmValTuneles();       
                }
                else if (e.Value2 == "ZODOPU")
                {
                    mFormDetail = new frmDoHiPuManager();
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else if (e.Value2 == "CRUINF")
                {
                    mFormDetail = new frmCruInfManager();
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else if (e.Value2 == "SECPRI" | e.Value2 == "SECSEC" | e.Value2 == "SECTER")
                {
                    mFormDetail = new frmSocioEconomicasDatagrid(e.Value2);
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else if (e.Value2 == "NOURBA" | e.Value2 == "URBANO" | e.Value2 == "URBANI")
                {
                    mFormDetail = new frmPatrimonioSueloDatagrid(e.Value2);
                    ((IfrmManagerPopulate)mFormDetail).populate();
                }
                else
                {
                    mFormDetail = new frmZonaGis(e.Value); 
                }


                mFormDetail.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                mFormDetail.TopLevel = false;
                splitContainer1.Panel2.Controls.Add(mFormDetail);
                mFormDetail.Show();

            }
        }

        #endregion

    }
}
