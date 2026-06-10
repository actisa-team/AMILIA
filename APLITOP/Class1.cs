using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging; // Necesario para imágenes si usas iconos

// Librerías de AutoCAD
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows;



// Tus namespaces
using System.Windows.Forms;
using MaterialSkin;
using Datos;
using interfaz;
using Logica;
using tadLayLogica;
using tadLayUI;

namespace AMILIA
{
    // Implementamos IExtensionApplication para ejecutar código al inicio
    public class Class1 
    {
        

        // ---------------------------------------------------------
        // PARTE 2: TUS COMANDOS EXISTENTES
        // ---------------------------------------------------------

        #region "COMANDOS AMILIA"
        [CommandMethod("amilia")]
        public static void actisa()
        {
            try
            {
                // Tu lógica original intacta
                principal p = new principal();
                // Autodesk.AutoCAD.ApplicationServices.Application.ShowModelessDialog(p); // Recomendado para evitar bloqueos
                p.Show(); 
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
                
                // También mostramos el error en AutoCAD para que lo veas
                Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
                ed.WriteMessage($"\nError al lanzar ventana: {ex.Message}\n");
            }
        }
        #endregion
        #region "COMANDOS TADIL"
        [CommandMethod("tdi")]
        public void tdi()
        {
            try
            {

                if (oTadilSwitch.hasInstanceTDB)
                {
                    oTadilSwitch.showUserNoPermitirInstanciarTDI();
                }
                else
                {
                    oTadilSwitch.hasInstanceTDI = true;

                    oTadil.data.setIdiomaAplicacion();

                    frmAppManager.getInstance.Show();
                }

            }
            catch (System.Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }
        [CommandMethod("tdb")]
        public void tdb()
        {
            try
            {

                if (oTadilSwitch.hasInstanceTDI)
                {
                    oTadilSwitch.showUserNoPermitirInstanciarTDB();
                }
                else
                {
                    oTadilSwitch.hasInstanceTDB = true;

                    oTadil.data.setIdiomaAplicacion();

                    frmBb.getInstance.Show();
                }

            }
            catch (System.Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }

        [CommandMethod("tdm")]
        public void tdm()
        {
            try
            {
                oTadil.data.setIdiomaAplicacion();
                frmManagerTerreno miFrmTerreno = new frmManagerTerreno();
                miFrmTerreno.Show();

            }
            catch (System.Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }


        [CommandMethod("tdsp")]
        public void tdsp()
        {
            try
            {


                frmSimplificarPolilineas miFrmPolilineas = new frmSimplificarPolilineas();
                miFrmPolilineas.ShowDialog();

            }
            catch (System.Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }


        [CommandMethod("tds")]
        public void tds()
        {
            try
            {

                oTadil.data.setIdiomaAplicacion();

                frmSimplificarPolilineas.getInstance.Show();

            }
            catch (System.Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }

        //frmManagerTerreno
        #endregion    
    }
}
