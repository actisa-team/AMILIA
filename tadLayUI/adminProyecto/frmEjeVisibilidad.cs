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
    using tadLayLan.Tdi;
    using tadLayLan;
    using System.Diagnostics;
    
    public partial class frmEjeVisibilidad : Form
    {
        public frmEjeVisibilidad()
        {
            InitializeComponent();

            postConstructor();
        }



        private void postConstructor()
        {
            #region "Traducción"
            btnEjeVisibilidadAutomatico.Text = strFrmSolucion.uiEjeVisibilidadAutomatico;
            btnEjeVisibilidadUsuario.Text = strFrmSolucion.uiEjeVisibilidadUsuario;
            ucDistancia.uiLbl = strFrmSolucion.uiDistancia;
            btnCorredores.Text = strFrmSolucion.uiCorredores;

            #endregion
        }


        #region "Botones EjeVisibilidad"
        //Eje Visibilidad Automático
        private void btnEjeVisibilidadAutomatico_Click(object sender, EventArgs e)
        {

            try
            {

                Stopwatch miMedicion = new Stopwatch();

                miMedicion.Start();
                //Oculto la Instancia
                frmAppManager.getInstance.Hide();

                //Genero el Eje de Visibilidad
                tadLayLogica.Comandos.oComandoEjeVisibilidad.createAutomatico();

                miMedicion.Stop();

                //Info UI
                oTadil.data.UserInfo.procesoTerminadoConTiempo(miMedicion.Elapsed.TotalMinutes);

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
        //Eje Visibilidad Usuario
        private void btnEjeVisibilidadUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                //Oculto la Instancia
                frmAppManager.getInstance.Hide();

                //Selecciono Eje de Visibilidad
                tadLayLogica.Comandos.oComandoEjeVisibilidad.selectUsuario();

            }
            catch (NullReferenceException ex)
            {
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiProcesoAnulado);
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

        private void btnCorredores_Click(object sender, EventArgs e)
        {
            try
            {

                if (oValidar.isValidoGrupoByFrm(this))
                {
                    //Oculto la Instancia
                    frmAppManager.getInstance.Hide();

                    //Selecciono Eje de Visibilidad
                    tadLayLogica.logica.Comandos.oComandoCorredores.createCorredores(this.ucDistancia.textbox.valorDouble);

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
            finally
            {
                frmAppManager.getInstance.Show();
            }

        }

    }
}
