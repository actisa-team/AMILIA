using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminInformes
{

    using tadLayLogica;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.logica.presupuesto;
    using tadLayLogica.logica.rentabilidad;
    using tadLayLan.Tdi;
    using tadLayLan;
    
    
    public partial class frmInfSolucionManager : Form
    {

        private string mMonedaSimbolo = string.Empty;
        
        
        
        
        public frmInfSolucionManager()
        {
            InitializeComponent();
        }



        #region "Propiedades"

       




        #endregion



        #region "Botones"
        //PRESUPUESTO EJECUCION MATERIAL
        private void btnPEM_Click(object sender, EventArgs e)
        {

            try
            {
                if (ucDgvSolucionesValorar1.solucion != null)
                {
                    string miFile = oTadil.data.Files.saveAsFileCsvFromDialog("PresupuestoEjecucionMaterial_Solucion_" + ucDgvSolucionesValorar1.solucionNombre);


                    if (string.IsNullOrEmpty(miFile))
                    {
                        return;
                    }
                    else
                    {

                        oPresupuestoPBL miPresupuestoPBL = new oPresupuestoPBL(ucDgvSolucionesValorar1.solucion.Value);

                        miPresupuestoPBL.write(miFile,this.mMonedaSimbolo);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }
                       
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }




        } 
        //PRESUPUESTO CONOCIMIENTO ADMINISTRACION PUBLICO
        private void btnPCAPublico_Click(object sender, EventArgs e)
        {

            try
            {
                if (ucDgvSolucionesValorar1.solucion != null)
                {
                    string miFile = oTadil.data.Files.saveAsFileCsvFromDialog("PresupuestoConocimietoAdministracionPublico_Solucion_" + ucDgvSolucionesValorar1.solucionNombre);


                    if (string.IsNullOrEmpty(miFile))
                    {
                        return;
                    }
                    else
                    {
                        oPresupuestoPcaPublico miPresupuesto = new oPresupuestoPcaPublico(ucDgvSolucionesValorar1.solucion.Value);
                        miPresupuesto.write(miFile,mMonedaSimbolo);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiSelecSolucion);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        //PRESUPUESTO CONOCIMIENTO ADMINISTRACION PRIVADO
        private void btnPCAPrivado_Click(object sender, EventArgs e)
        {

            try
            {
                if (ucDgvSolucionesValorar1.solucion != null)
                {
                    string miFile = oTadil.data.Files.saveAsFileCsvFromDialog("PresupuestoConocimietoAdministracionPrivado_Solucion_" + ucDgvSolucionesValorar1.solucionNombre);


                    if (string.IsNullOrEmpty(miFile))
                    {
                        return;
                    }
                    else
                    {
                        oPresupuestoPcaPublicoPrivado miPresupuesto = new oPresupuestoPcaPublicoPrivado(ucDgvSolucionesValorar1.solucion.Value);
                        miPresupuesto.write(miFile,mMonedaSimbolo);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiSelecSolucion);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }


        }
        //INFORME RENTABILIDAD PUBLICA
        private void btnRentabilidadPublica_Click(object sender, EventArgs e)
        {

            try
            {
                if (ucDgvSolucionesValorar1.solucion != null)
                {
                    string miFile = oTadil.data.Files.saveAsFileCsvFromDialog("InformeRentabilidadSocial_Solucion_" + ucDgvSolucionesValorar1.solucionNombre);


                    if (string.IsNullOrEmpty(miFile))
                    {
                        return;
                    }
                    else
                    {
                        oRentabilidadAnalisisSocial miInformeRentabilidadSocial = new oRentabilidadAnalisisSocial(ucDgvSolucionesValorar1.solucion.Value);

                        miInformeRentabilidadSocial.print(miFile);

                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiSelecSolucion);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }


        }
        //INFORME RENTABILIDAD PRIVADA
        private void btnRentabilidadPrivada_Click(object sender, EventArgs e)
        {

            try
            {
                if (ucDgvSolucionesValorar1.solucion != null)
                {
                    string miFile = oTadil.data.Files.saveAsFileCsvFromDialog("InformeRentabilidadPrivada_Solucion_" + ucDgvSolucionesValorar1.solucionNombre);


                    if (string.IsNullOrEmpty(miFile))
                    {
                        return;
                    }
                    else
                    {
                        oRentabilidadAnalisisPrivado miInforme = new oRentabilidadAnalisisPrivado(ucDgvSolucionesValorar1.solucion.Value);

                        miInforme.print(miFile);

                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        //VALORACION TRAZADO PLANTA
        private void btnValoracionTrazadoPlanta_Click(object sender, EventArgs e)
        {

            try
            {
                if (ucDgvSolucionesValorar1.solucion != null)
                {
                    string miFile = oTadil.data.Files.saveAsFileCsvFromDialog(strFrmInformes.uiInfValoracionTrazadoPlanta + ucDgvSolucionesValorar1.solucionNombre);


                    if (string.IsNullOrEmpty(miFile))
                    {
                        return;
                    }
                    else
                    {
                        tadLayLogica.Comandos.oComandoInformes.createInformeValoracionTrazadoPlanta(ucDgvSolucionesValorar1.solucion.Value, miFile);
                    }

                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        //VALORACION TRAZADO ALZADO
        private void btnValoracionTrazadoAlzado_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucDgvSolucionesValorar1.solucion != null)
                {
                    string miFile = oTadil.data.Files.saveAsFileCsvFromDialog(strFrmInformes.uiInfValoracionTrazadoAlzado + ucDgvSolucionesValorar1.solucionNombre);

                    if (string.IsNullOrEmpty(miFile))
                    {
                        return;
                    }
                    else
                    {
                        tadLayLogica.Comandos.oComandoInformes.createInformeValoracionTrazadoAlzado(ucDgvSolucionesValorar1.solucion.Value, miFile);
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }



        }

        private void btnValoracionTiempo_Click(object sender, EventArgs e)
        {
            try
            {
                if (ucDgvSolucionesValorar1.solucion != null)
                {
                    string miFile = oTadil.data.Files.saveAsFileCsvFromDialog(strFrmInformes.uiInfValoracionTrazadoTiempo + ucDgvSolucionesValorar1.solucionNombre);

                    if (string.IsNullOrEmpty(miFile))
                    {
                        return;
                    }
                    else
                    {
                        tadLayLogica.Comandos.oComandoInformes.createInformeValoracionTrazadoTiempo(ucDgvSolucionesValorar1.solucion.Value, miFile);
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strGeneralUser.uiDgvRowNull);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        #endregion


        #region "Eventos"

        private void frmInfSolucionManager_Load(object sender, EventArgs e)
        {
            #region "TRADUCCIONES"

            this.Text = strFrmInformes.uiINFSOL;
            this.grLstSoluciones.Text = strFrmInformes.uiGrListadoSoluciones;
            this.grInformes.Text = strFrmInformes.uiInformes;

            this.tabPresupuestos.Text = strFrmInformes.uiTabPresupuestos;
            this.btnPEM.Text = strFrmInformes.uiPresupuestoEjecucionMateria;
            this.btnPCAPublico.Text = strFrmInformes.uiPresupuestoPcaPublico;
            this.btnPCAPrivado.Text = strFrmInformes.uiPresupuestoPcaPrivado;

            this.tabRentabilidad.Text = strFrmInformes.uiTabRentabilidad;
            this.btnRentabilidadPublica.Text = strFrmInformes.uiInformeRentabilidadSocial;
            this.btnRentabilidadPrivada.Text = strFrmInformes.uiInformeRentabilidadPrivada;

            this.btnValoracionTrazadoAlzado.Text = strFrmInformes.uiInfValoracionTrazadoAlzado;
            this.btnValoracionTrazadoPlanta.Text = strFrmInformes.uiInfValoracionTrazadoPlanta;
            this.btnValoracionTiempo.Text = strFrmInformes.uiInfValoracionTrazadoTiempo;

            ucDgvSolucionesValorar1.populate();

            if (oSingletonDsApp.getInstance.isInversionPrivada)
            {
                this.btnRentabilidadPrivada.Enabled = true;
            }
            else
            {
                this.btnRentabilidadPrivada.Enabled = false;
            }

            #endregion


            #region "SETUP-OBJETOS"

            this.mMonedaSimbolo = oSingletonDsBd.getInstance.monedaNombre;


            #endregion

        }
        private void frmInfSolucionManager_Shown(object sender, EventArgs e)
        {
            ucDgvSolucionesValorar1.clearSelection();
        }

        #endregion

        





    }
}
