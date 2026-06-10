using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace engCadCom
{

   
    using Autodesk.AutoCAD.Interop;
    using Autodesk.AECC.Interop.Land;
    using Autodesk.AECC.Interop.UiLand;
    
    
    
    public class oEngCadCom
    {

        private static oEngCadCom instance = null;
        
        private IAcadApplication mCadApp = null;
        private IAeccApplication mCiv3dApp = null;
        private IAeccDocument mCiv3dDoc = null;
        private IAeccDatabase mCiv3dBas = null;

        string m_sAcadProdID = "AutoCAD.Application";
        string m_sAeccAppProgId = "AeccXUiLand.AeccApplication.9.0";


        protected oEngCadCom()
        {
            getConexion();
        }


        public static oEngCadCom get
        {

            get
            {
                if (instance == null)
                {
                    instance = new oEngCadCom();

                }

                return instance;
            }
        
        
        }


        private void getConexion()
        {

           
            try
            {

                            
                //Cargo la Aplicacion Autocad
                mCadApp = (Autodesk.AutoCAD.Interop.IAcadApplication)System.Runtime.InteropServices.Marshal.GetActiveObject(m_sAcadProdID);
                mCadApp.Visible = true;
                mCadApp.WindowState = Autodesk.AutoCAD.Interop.Common.AcWindowState.acMax;

                if (mCadApp != null)
                {

                    mCadApp.Visible = true;
                    mCiv3dApp = (Autodesk.AECC.Interop.UiLand.IAeccApplication)mCadApp.GetInterfaceObject(m_sAeccAppProgId);
                    mCiv3dDoc = (Autodesk.AECC.Interop.UiLand.IAeccDocument)mCiv3dApp.Application.ActiveDocument;
                    // get the Database object via a late bind
                    mCiv3dBas = (Autodesk.AECC.Interop.Land.IAeccDatabase)mCiv3dDoc.GetType().GetProperty("Database").GetValue(mCiv3dDoc, null);
                }
                else
                {

                    MessageBox.Show("No se puede Obtener la Conexion con Autocad");
                }

            }
            catch (Exception)
            {

                throw;
         

                //System.Type AcadProg = System.Type.GetTypeFromProgID(m_sAcadProdID);
                //mCadApp = (Autodesk.AutoCAD.Interop.IAcadApplication)System.Activator.CreateInstance(AcadProg, true);
                //Instead above two lines of code, simply use following new (). 
                //However, this always creates an instance of the 
                //AutoCAD even if it is already running.
                //m_oAcadApp = new Autodesk.AutoCAD.Interop.AcadApplicationClass();
            }


        }



        public IAcadApplication thisCadApp
        {
            get{return mCadApp;}     
        }

        public IAeccApplication thisCiv3dApp
        {
            get { return mCiv3dApp; }   
        }

        public IAeccDocument thisCiv3dDoc
        {
            get { return mCiv3dDoc; }
        
        }

        public string thisCiv3dDocName
        {
            get { return thisCiv3dDoc.FullName; }

        
        }

    }




}
