using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayLogica
{

    using tadLayLan;

    using System.Windows.Forms;
    using System.IO;
    
    /// <summary>
    /// MANAGER USER UI
    /// </summary>
    public class oUserInfo
    {
        #region "CONSTRUCTOR"

        public oUserInfo()
        {

        }

        #endregion
        #region "METODOS PUBLICOS"

        public void showError(Exception iException)
        {
            string myErrorUser = getErrorUser(iException);
            MessageBoxManager.OK = strGeneral.uiAceptar;
            MessageBoxManager.Register();

            var name = oTadil.KAppHeaderName;

            MessageBox.Show(myErrorUser, name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            MessageBoxManager.Unregister();
        }
        public void procesoTerminado()
        {
            this.showInfo(strGeneralUser1.uiProcesoTerminado);
        }
        public void procesoCancelado()
        {
            this.showInfo(strGeneralUser1.uiProcesoCancelado);
        }
        public void errorValidacion()
        {
            this.showInfo(strGeneralUser1.uiErrorValidarDatos);
        }
        public void procesoTerminadoConTiempo(double iTotalMinutos)
        {
            this.showInfo(string.Format(strGeneralUser1.uiProcesoTerminadoConTiempoMinutos, iTotalMinutos.ToString("0.00")));
        }
        public void showInfo(string iMensaje)
        {
            MessageBoxManager.OK = strGeneral.uiAceptar;
            MessageBoxManager.Register();

            var name = oTadil.KAppHeaderName;

            MessageBox.Show(iMensaje, name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();
        }

        public void showInfo(IWin32Window owner, string iMensaje)
        {
            MessageBoxManager.OK = strGeneral.uiAceptar;
            MessageBoxManager.Register();

            var name = oTadil.KAppHeaderName;

            MessageBox.Show(owner, iMensaje, name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();
        }
        public DialogResult showSiNo(string iMensaje)
        {
            MessageBoxManager.No = strGeneral.uiNo;
            MessageBoxManager.Yes = strGeneral.uiSi;
            MessageBoxManager.Register();

            var name = oTadil.KAppHeaderName;

            DialogResult miSolucion =  MessageBox.Show(iMensaje, name, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();
            return miSolucion;
        }


        public DialogResult showSiNo(IWin32Window owner, string iMensaje)
        {
            MessageBoxManager.No = strGeneral.uiNo;
            MessageBoxManager.Yes = strGeneral.uiSi;
            MessageBoxManager.Register();

            var name = oTadil.KAppHeaderName;

            DialogResult miSolucion = MessageBox.Show(owner, iMensaje, name, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();
            return miSolucion;
        }





        #endregion
        #region "METODOS PRIVADOS"
        private string getErrorUser(Exception ex)
        {

            StringBuilder errorInfo = new StringBuilder();
            bool eControlado = false;

            //creamos un contenido que incluiremos en el log del error

            errorInfo.Append("[Error]\n");


            if (ex is FileNotFoundException)
            {
                errorInfo.Append(string.Format(strGeneralUser.uiNoFichSelecionado, ex.Message));
                eControlado = true;
            }
            else if (ex is DirectoryNotFoundException)
            {
                errorInfo.Append(string.Format(strGeneralUser.uiNoFichSelecionado, ex.Message));
                eControlado = true;
            }
            else if (ex is engNex.Net.BussinesObject.Licencia.oExLicencia)
            {
                errorInfo.Append("\n Error000");
                errorInfo.Append(ex.Message);
                eControlado = true;
                return errorInfo.ToString();
            }
            else
            {
                string error = ex.Message + ex.StackTrace + ex.GetType().ToString();
                string miMensaje = ex.Message;
                //if (error.Contains("get_ejeBasico2D"))
                //{
                //    miMensaje = strError.eEjeBasicoNoEncontrado;
                //    eControlado = true;
                //}
                if (error.Contains("get_ejeTrazado"))
                {
                    miMensaje = strGeneralUser.uiEjeTrazadoNoExiste;
                    eControlado = true;
                }
                if (error.Contains("oExEntidadNoExiste") || error.Contains("eWasErased"))
                {
                    miMensaje = strError.eEntidadNoExiste;
                    eControlado = true;
                }
                if (error.Contains("tbSeccionZonasGenerales") && error.Contains("DBNull"))
                {
                    miMensaje = strGeneralUser.uiFaltanDatosEjeBasico;
                    eControlado = true;
                }
                if (error.Contains("Error Caso cálculo Azimut"))
                {
                    miMensaje = strError.eCoincidirPtoIniFin;
                    eControlado = true;
                }
                if ((error.Contains("eKeyNotFound"))&&((error.Contains("set_Layer")||(error.Contains("get_Layer")))))
                {
                    miMensaje = strError.eCapaEliminada;
                    eControlado = true;
                }
                if ((error.Contains("oExPropertieNullValue")))
                {
                    miMensaje = ex.Message;
                    eControlado = true;
                }
                //if (error.Contains("xEl Listado de Mediciones por Sección es Nulo"))
                //{
                //    miMensaje = strError.eErrorListadoMedicionesNull;
                //    eControlado = true;

                //}

                if (error.Contains("System.NullReferenceException") && error.Contains("getLstPto") && error.Contains("oLw.cs:") && error.Contains("373") && error.Contains("oTarget.cs:") && error.Contains("127"))
                {
                    miMensaje = strGeneralUser.uiProcesoAnulado;
                    eControlado = true;

                }


                if (error.Contains("System.NullReferenceException") && error.Contains("getLstPto") && error.Contains("oEntidad.cs:") && error.Contains("61"))
                {
                    miMensaje = strGeneralUser.uiProcesoAnulado;
                    eControlado = true;

                }


                if (error.Contains("tbPtoIniFin") && error.Contains("DBNull"))
                {
                    miMensaje = strError.ePuntoIniFinNoDef;
                    eControlado = true;

                }


                if (error.Contains("Existen puntos fuera de la cartografia PERFIL LONGITUDINAL"))
                {
                    miMensaje = strError.eExistenPuntosFueraCartografia;
                    eControlado = true;
                }


                errorInfo.Append(miMensaje);
            }



            if (!eControlado)
            {

                errorInfo.Append("\n\n");

                errorInfo.Append("[Excepción]\n" + ex.GetType().ToString() + "\n");
            }



            //COMENTAR

            //    errorInfo.Append("\n");

            //    ////añadimos información adicional (Es necesario cargar el .pbd de la aplicación)
            //    System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
            //    errorInfo.Append("[Método]\n" + trace.GetFrame(0).GetMethod().Name + " ; ");
            //    errorInfo.Append("Linea: " + trace.GetFrame(0).GetFileLineNumber().ToString() + " ; ");
            //    errorInfo.Append("Columna: " + trace.GetFrame(0).GetFileColumnNumber().ToString() + "\n");

            //    errorInfo.Append("\n");

            //    errorInfo.Append("[Origen]\n" + ex.StackTrace + "\n");
            
            ////que no se vea el error... ni el origen

            //errorInfo.Append("\n\n");

            //errorInfo.Append("[Excepción]\n" + ex.GetType().ToString() + "\n");


            //errorInfo.Append("\n");


            //COMENTAR


            //Devolvemos el Error
            return errorInfo.ToString();


        }
        #endregion  
    }


}
