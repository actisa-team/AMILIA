using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminProyecto
{
    using tadLayLogica;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.Comandos;
    using tadLayLan;
    using tadLayShare;
    using tadLayLan.Tdi;
    using System.Threading.Tasks;
    using System.IO;

    public partial class frmDatosTerreno : Form
    {

        private string mId = "APP";
        private BindingSource mBindMaster = null;
       
        public frmDatosTerreno()
        {
            InitializeComponent();

            postConstructor();
        }



        private void postConstructor()
        {

            #region "Traduccion"

            grTerreno.Text = strFrmTerreno.uiGrTerreno;
            ucTerreno.uiLbl = strGeneral.uiNombre;

            ucTolerancia.uiLbl = strFrmTerreno.uiTolerancia;


            //grAnalisis.Text = strFrmTerreno.uiGrTerrenoAnalisis;
            //ucPendienteMayorPC.uiLbl = strFrmTerreno.uiPendienteMaxima;
            //btnAnalisisPrevisualiar.Text = strFrmTerreno.uiPrevisulizar;

            grDrawZonasNoPaso.Text = strFrmTerreno.uiGrZonasNoPaso;
            btnSelectZonasNoPasoPendiente.Text = strFrmTerreno.uiSelectZonasNoPasoPendiente;
            btnSelectZonasNoPasoUsuario.Text = strFrmTerreno.uiSelectZonasNoPasoUsuario;
            btnCrearEnvolvente.Text = strFrmTerreno.uiCrearPolEnv;
            btnSelectEnvolvente.Text = strFrmTerreno.uiSeleccionarPolEnv;
            #endregion

            #region " SetUp Objetos"

            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Visible = false;

            ucTerreno.populate("_Tadil_MDT_");


            #endregion

            #region "Bind"

            //Bind
            mBindMaster = new BindingSource();
            mBindMaster.DataMember = oSingletonDsApp.getInstance.dataset.tbTerreno.TableName;
            mBindMaster.DataSource = oSingletonDsApp.getInstance.dataset.tbTerreno;

            if (mBindMaster.Count == 0)
            {
                mBindMaster.AddNew();
            }
            else if (mBindMaster.Count == 1)
            {
                string miQuery = "id = '{0}'";
                mBindMaster.Filter = string.Format(miQuery, mId);
            }
            else
            {
                throw new oExRowFoundMayorUno("id", "tbEstilosVisualizacion");
            }

         
            ucTerreno.uiCombo.DataBindings.Add("SelectedValue", mBindMaster, oSingletonDsApp.getInstance.dataset.tbTerreno.handleTerrenoColumn.ColumnName);
            ucTolerancia.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, oSingletonDsApp.getInstance.dataset.tbTerreno.toleranciaColumn.ColumnName, true);
           
            #endregion

        }


        #region "BOTONES"

        //SAVE
        void lnkSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (oValidar.isValidoGrupoByFrm(this))
                {
                    ((DataRowView)mBindMaster.Current)["id"] = mId;
                    mBindMaster.EndEdit();
                    oDalTbProyecto.saveTable(true);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }

        //CANCEL
        void lnkCancel_Click(object sender, EventArgs e)
        {
            try
            {
                mBindMaster.CancelEdit();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }

        //PREVISUALIZAR ANALISIS
        //private void btnAnalisisPrevisualiar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmAppManager.getInstance.Hide();

        //        //oComandoTerreno.crearAnalisis(ucPendienteMayorPC.valorDoubleNull);
        //    }
        //    catch (Exception ex)
        //    {

        //        oTadil.data.UserInfo.showError(ex);
        //    }
        //    finally
        //    {
        //        frmAppManager.getInstance.Show();

        //    }
        //}

        //NO PASO PENDIENTE ; DRAW POLILINEA
        private void btnDrawZonasNoPasoPendiente_Click(object sender, EventArgs e)
        {
            try
            {
                frmAppManager.getInstance.Hide();

                oComandoTerreno.selectZonasNoPasoPendiente();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }

        }

        // NO PASO USUARIO ; DRAW POLILINEA
        private void btnDrawZonasNoPasoUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                frmAppManager.getInstance.Hide();

                oComandoTerreno.selectZonasNoPasoUsuario();
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }
        }

        #endregion

        private void btnSelectEnvolvente_Click(object sender, EventArgs e)
        {

            try
            {
                frmAppManager.getInstance.Hide();

                oComandoTerreno.selectPolilineaEnvolvente(ucTerreno.uiCombo.Text);
               /* if (ucTerreno.uiCombo.SelectedValue != null)
                {
                    frmAppManager.getInstance.Hide();

                    oComandoTerreno.selectPolilineaEnvolvente(ucTerreno.uiCombo.Text);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                }*/
                
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }

        }

        private void btnCrearEnvolvente_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                 * Se cambia ya que no hace falta tener seleccionado el desplegable sino el archivo cargado ** juanma**
                 */
                if(oSingletonPuntosTerreno.getInstance.get_todo())
                {
                    frmAppManager.getInstance.Hide();

                    oComandoTerreno.createPolilineaEnvolvente(ucTerreno.uiCombo.Text);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                }
                /*if (ucTerreno.uiCombo.SelectedValue != null)
                {
                    frmAppManager.getInstance.Hide();

                    oComandoTerreno.createPolilineaEnvolvente(ucTerreno.uiCombo.Text);
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoTerrenoSeleccionado);
                }*/

            }
            catch(tadLayShare.oExRowNotFound)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiGuardaTerreno);
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }

            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                frmAppManager.getInstance.Hide();
                oSingletonPuntosTerreno.getInstance.Cargar(true);
               
            }
            catch (tadLayShare.oExRowNotFound)
            {
                oTadil.data.UserInfo.showInfo("Error al cargar el MDT");
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                oTadil.data.UserInfo.showInfo("MDT Cargado con éxito");
               
                frmAppManager.getInstance.Show();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                frmAppManager.getInstance.Hide();
                Terrenos.Triangulacion resultado=oSingletonPuntosTerrenoASC.getInstance.Cargar(true);
                if (resultado==null)
                {
                    oTadil.data.UserInfo.showInfo("Error al cargar el MDT");
                    return;
                }
            }
            catch (tadLayShare.oExRowNotFound)
            {
                oTadil.data.UserInfo.showInfo("Error al cargar el MDT");
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                oTadil.data.UserInfo.showInfo("MDT Cargado con éxito");

                frmAppManager.getInstance.Show();
            }
        }
        
    }
}
