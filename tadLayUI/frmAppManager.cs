using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI

{

    using engNet;
    using engNet.eventos;
    using tadLayLogica;
    using tadLayLogica.datos;
    using tadLayData;
    using tadLayLan;

    using tadLayUI.adminProyecto;
    using tadLayUI.adminInformes;
    using tadLayUI.adminRentabilidad;
    using tadLayUI.adminValoracion;

    using tadLayLogica.datos.proyecto;
    using tadLayLogica.datos.BaseDatos;

    using tadLayLogica.zonaGis;

    using System.Threading;
    using System.Globalization;
    using engNex.Net.BussinesObject.Licencia;

    public partial class frmAppManager : frmRoot
    {

       #region "Private Fields"

        private Form mFormDetail;
        private eEstudioTipo? mEstudioTipo = null;
        private static frmAppManager mInstance = null;

        string nextItem = "";

        #endregion
       #region "Constructores"
   
        private frmAppManager()
        {
            InitializeComponent();
            postConstructor();
        }

    
        public static frmAppManager getInstance
        {

            get
            {
                if (mInstance == null)
                {
                    mInstance = new frmAppManager();
                }
                else
                {
                    mInstance.WindowState = FormWindowState.Normal;
                    mInstance.Focus();
                }

                return mInstance;
            }
        }



        #endregion
       #region "PUBLICOS"



        #endregion
       #region "PRIVADOS"

        private  void  postConstructor()
        {

            var name = oTadil.KAppHeaderName;
            this.Text = name;
            //this.Icon = new System.Drawing.Icon("\\Resources\\icono_tadil.ico");


            splitContainer1.Panel1Collapsed = true;
            splitContainer1.Panel2.BackColor = Color.White;
            splitContainer1.Panel2.BackgroundImageLayout = ImageLayout.Center;
            splitContainer1.Panel2.BackgroundImage = Properties.Resources.backTadilStr;

            //Evento Tree Obtengo Los Codigos de los Formularios
            ucTree.evNodeCurrentCode += new EventHandler<oEventArgs<string, string>>(ucTree_evNodeCurrent);

            //Evento la Ruta del Nodo
            ucTree.evNodeCurrentPath += new EventHandler<oEventArgs<string>>(ucTree_evNodeCurrentPath);

            //Traduccion
            lblHeader.Text = strFrmApp.uiAdministrador;
            lblDetail.Text = string.Empty;
            lblFileApp.Text = strFrmFiles.uiFileAppSelect;
            lblFileApp.ForeColor = Color.Red;

            //Barra Menu
             ucMenuTDI1.enableMnuSaveAs(false);

            ucMenuTDI1.evNewEstudioPrevio += new EventHandler(ucMenuTDI1_evNewEstudioPrevio);
            ucMenuTDI1.evNewEstudioInformativo += new EventHandler(ucMenuTDI1_evNewEstudioInformativo);
            ucMenuTDI1.evOpen += new EventHandler(ucMenuTDI1_evOpen);
            ucMenuTDI1.evSaveAs += new EventHandler(ucMenuTDI1_evSaveAs);
            ucMenuTDI1.evExit += new EventHandler(ucMenuTDI1_evExit);

        }





        void Files_evFileAppUpdate(object sender, oEventArgs<string> e)
        {
            lblFileApp.Text = e.Value;
            lblFileApp.ForeColor = Color.Green;
        }


        private void displayFrmReset ()
        {
            ucTree.clear();
            ucTree_evNodeCurrent(null, new oEventArgs<string, string>(string.Empty, string.Empty));
        }


        private void displayFrmFilesSetup()
        {
            ucTree_evNodeCurrent(null, new oEventArgs<string, string>("CONCON", "CONFIL"));
        }



        #endregion
       #region "EVENTOS ARBOL"
     
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


            try
            {
                //Llamada al Validador de Licencia
                //cambiado juanma 250117
                //ol.e(oTadil.IsDemo, oTadil.IsTmp);


            splitContainer1.Panel2.Controls.Clear();
            splitContainer1.Panel2.BackColor = Color.White;
            splitContainer1.Panel2.BackgroundImage = null;
      
           if (e.Value == string.Empty && e.Value2 == string.Empty)
            {
               splitContainer1.Panel2.BackgroundImageLayout = ImageLayout.Center;
               splitContainer1.Panel2.BackgroundImage = Properties.Resources.backTadilStr;

               return;
            }

           else if (e.Value2 == "CONFIL")
           {
               mFormDetail = new frmFiles(mEstudioTipo.Value);
           }

           //DATOS PROYECTO
           else if (e.Value2 == "DATPRO")
           {
               mFormDetail = new frmDatosProyecto(mEstudioTipo.Value);
           }
           else if (e.Value2 == "DATTER")
           {
               mFormDetail = new frmDatosTerreno();
           }
           else if (e.Value2 == "PTOORI")
           {
               mFormDetail = new frmPtoIni(0);
           }
           else if (e.Value2 == "PTODES")
           {
               mFormDetail = new frmPtoIni(1);
           }
           //else if (e.Value2 == "ESTVIS")
           //{
           //    mFormDetail = new frmProyectoEstilosVisualizacion();
           //}
           else if (e.Value2 == "EJEVIS")
           {
               mFormDetail = new frmEjeVisibilidad();
           }

           else if (e.Value2 == "EJEBAS")
           {
               mFormDetail = frmSolucion.getInstance(mEstudioTipo.Value);
           }

           else if (e.Value2 == "SOLEDI")
           {
               mFormDetail = new frmSolucionManager(mEstudioTipo.Value);
           }

           //------ESTUDIO PREVIO---ESPECIFICOS---------//





           //------ESTUDIO INFORMATIVO----ESPECIFICOS----//
           else if (e.Value2 == "INFDAT")
           {
               mFormDetail = new frmSeccionZonasGenerales();
           }
           //RENTABILIDAD
           else if (e.Value2 == "INDATE")
           {
               mFormDetail = new frmInDaTeDetail();
               nextItem = "INVTIP";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "INVTIP")
           {
               mFormDetail = new frmInvTipDetail();
               nextItem = "DATTRA";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "DATTRA")
           {
               mFormDetail = new frmDatTraDetail();
               nextItem = "COSACC";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "COSACC")
           {
               mFormDetail = new frmCosAccDetail();
               nextItem = "COTIFU";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "COTIFU")
           {
               mFormDetail = new frmCoTiFuDetail();
               nextItem = "GACORE";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "GACORE")
           {
               mFormDetail = new frmGaCoReDetail();
               nextItem = "VEHCON";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VEHCON")
           {
               mFormDetail = new frmConVehDetail();
           }
           else if (e.Value2 == "VEHMAN")
           {
               mFormDetail = new frmVehManDetail();
           }
           //VALORACION
           else if (e.Value2 == "VALTRA")
           {
               mFormDetail = new frmValTrazadoDetail();
               nextItem = "VALGEO";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VALGEO")
           {
               mFormDetail = new frmValGeotecniaDetail();
               nextItem = "VAESTU";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VAESTU")
           {
               mFormDetail = new frmValEstTunDetail();
               nextItem = "VALMED";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VALMED")
           {
               mFormDetail = new frmValMedioAmbientalDetail();
               nextItem = "VALCLI";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VALCLI")
           {
               mFormDetail = new frmValClimaticasDetail();
               nextItem = "VALSOC";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VALSOC")
           {
               mFormDetail = new frmValSocioEconomicasDetail();
               nextItem = "VALPAT";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VALPAT")
           {
               mFormDetail = new frmValPatrimonialesDetail();
               nextItem = "VALECO";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VALECO")
           {
               mFormDetail = new frmValEconomicaDetail();
               nextItem = "VALMAT";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }
           else if (e.Value2 == "VALMAT")
           {
               mFormDetail = new frmMatrizDecisionManager();
           }
           //PRESUPUESTO
           else if (e.Value2 == "SETPRE")
           {
               mFormDetail = new frmAppPresupuestoDetail();
               nextItem = "INDATE";
               mFormDetail.ControlRemoved += new ControlEventHandler(cambiaColorev);
           }

           else if (e.Value2 == "INFSOL")
           {
               mFormDetail = new frmInfSolucionManager();
           }
           else
           {
               throw new NotImplementedException();
           }

           mFormDetail.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
           mFormDetail.WindowState = FormWindowState.Maximized;
           mFormDetail.TopLevel = false;
           splitContainer1.Panel2.Controls.Add(mFormDetail);
           mFormDetail.Show();

          }
         catch (Exception ex)
         {
              oTadil.data.UserInfo.showError(ex);
          }
        }

        #endregion
       #region "BARRA MENUS"

       //NUEVO ESTUDIO PREVIO
       void ucMenuTDI1_evNewEstudioPrevio(object sender, EventArgs e)
       {

           try
           {
                //Reset la Instancia
              // oTadilCore.reset();

               this.reset();

               //Cargo Evento
               oTadil.data.Files.evFileAppUpdate += new EventHandler<oEventArgs<string>>(Files_evFileAppUpdate);

               //Tipo Estudio
                mEstudioTipo = eEstudioTipo.ESTPRE;

               //Fichero Nuevo 
                string miFile = oTadil.data.Files.selectNewFileApp();

                
                splitContainer1.Panel1Collapsed = false;


                if (!string.IsNullOrEmpty(miFile))
                {
                    //Cargo el Archivo
                    oTadil.data.Files.fileApp = miFile;

                    //Cargo el Arbol
                    ucTree.populateByEstudio(mEstudioTipo.Value);

                    //Configuro el Proyecto a Estudio Previo
                    oDalTbProyecto.setUpEstudio(mEstudioTipo.Value);

                    //Configuro el Menu
                    ucMenuTDI1.enableMnuSaveAs(true);
                   
                    //Visualizo los archivos de configuración
                    displayFrmFilesSetup();

                }
                else
                {
                    oTadil.data.UserInfo.procesoCancelado();
                }
              
           }
           catch (Exception ex)
           { 
                oTadil.data.UserInfo.showError(ex);
           }


       }
       //NUEVO ESTUDIO INFORMATIVO
       void ucMenuTDI1_evNewEstudioInformativo(object sender, EventArgs e)
       {
           try
           {
               //Reset Tadil
               // oTadilCore.reset();

               this.reset();

               //Cargo Evento
               oTadil.data.Files.evFileAppUpdate += new EventHandler<oEventArgs<string>>(Files_evFileAppUpdate);

               //Tipo Estudio
               mEstudioTipo = eEstudioTipo.ESTINF;

               //Nuevo Fichero APP
               string miFile = oTadil.data.Files.selectNewFileApp();

               splitContainer1.Panel1Collapsed = false;


               if (!string.IsNullOrEmpty(miFile))
               {
                   //Cargo el Archivo
                   oTadil.data.Files.fileApp = miFile;

                   //Cargo el Arbol
                   ucTree.populateByEstudio(mEstudioTipo.Value);

                   //Configuro el Proyecto a Estudio Previo
                   oDalTbProyecto.setUpEstudio(mEstudioTipo.Value);

                   //Configuro el Menu
                   ucMenuTDI1.enableMnuSaveAs(true);
                  
                   //Visualizo los archivos de configuración
                   displayFrmFilesSetup();

               }
               else
               {
                   oTadil.data.UserInfo.procesoCancelado();
               }

           }
           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }

       }
       //FILE OPEN
       void ucMenuTDI1_evOpen(object sender, EventArgs e)
       {

           try
           {
               //Elimino la Instacia
              // oTadilCore.reset();
               this.reset();

               //Cargo Evento
               oTadil.data.Files.evFileAppUpdate += new EventHandler<oEventArgs<string>>(Files_evFileAppUpdate);

               //Cargo el Fichero
               string miFile = oTadil.data.Files.getFileApp();

               splitContainer1.Panel1Collapsed = false;


               if (!string.IsNullOrEmpty(miFile))
               {
                   
                   oTadil.data.Files.fileApp = miFile;

                   //Configuro el Tipo de Proyecto
                   mEstudioTipo = oDalTbProyecto.getEstudioTipo();

                   //Cargo el Arbol
                   ucTree.populateByEstudio(mEstudioTipo.Value);

                   //Valido las Rutas
                   if (!oDalTbFiles.isConfiguracionRutasValidos(mEstudioTipo.Value))
                   {
                       displayFrmFilesSetup();
                   }
                   else
                   {
                       displayFrmReset();

                       oDalTbFiles.setupArchivosConfiguracion(mEstudioTipo.Value);           
                   }

                   //Configuro la Barra de Herramientas
                   ucMenuTDI1.enableMnuSaveAs(true);
                  
               }
               else
               {
                   oTadil.data.UserInfo.showInfo(strError.eProcesoCancelado);
               }
           }
           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }

       }
       //SAVE
       void ucMenuTDI1_evSave(object sender, EventArgs e)
       {

           try
           {
               oSingletonDsApp.getInstance.save();
           }
           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }

       }
       //SAVEAS
       void ucMenuTDI1_evSaveAs(object sender, EventArgs e)
       {
           try
           {

               string miFile = oTadil.data.Files.saveAsFileApp();

               if (!string.IsNullOrEmpty(miFile))
               {
                   displayFrmReset();

                   oSingletonDsApp.deleteInstance();

                   oTadil.data.Files.fileApp = miFile;

               }
               else
               {
                   oTadil.data.UserInfo.showInfo(strError.eProcesoCancelado);
               }
           }
           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }

       }
       //EXIT
       void ucMenuTDI1_evExit(object sender, EventArgs e)
       {
           try
           {
               this.Close();
           }
           catch (Exception ex)
           {
               oTadil.data.UserInfo.showError(ex);
           }
       }


   
 //PRUEBA

       private void cambiaColorev(object sender, EventArgs e)
       {
           if (nextItem != "")
           {
               ucTree.cambiaColorTreeNodeAt(nextItem);
           }
           if (nextItem == "VEHCON")
           {
               ucTree.cambiaColorTreeNodeAt("VEHMAN");
               ucTree.cambiaColorTreeNodeAt("VALTRA");

           }
           if (nextItem == "VALMAT")
           {
               ucTree.cambiaColorTreeNodeAt("INFSOL");
           }
       }

       



       #endregion
       #region "EVENTOS FRM"
       private void frmAppManager_Shown(object sender, EventArgs e)
       {
           oTadil.data.setIdiomaAplicacion();    
       }
       private void frmAppManager_FormClosed(object sender, FormClosedEventArgs e)
       {
           mInstance = null;
           oTadilSwitch.hasInstanceTDI = false;
           this.reset();
       }
       private void reset ()
       {
           oTadil.data.Dispose();
           oSingletonProyecto.getInstance.Dispose();
           oSingletonTerreno.getInstance.Dispose();
           oSingletonZonaNoPaso.getInstance.Dispose();
           frmSolucion.deleteInstance();
           oSingletonDsApp.deleteInstance();
           oSingletonDsBd.deleteInstance();            
       }
       #endregion

       private void frmAppManager_Load(object sender, EventArgs e)
       {

       }

    }
}
