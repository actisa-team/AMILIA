using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.adminValoracion
{
    using System.IO;
    using System.Globalization;


    using tadLayUI;

    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica;
    using tadLayUI.userControl;
    
    
    public  partial class frmValoracionDetail : frmRoot
    {
        
        protected string mId = "APP";

        protected DataTable mTabla = null;

        protected BindingSource mBindMaster=null;

        protected bool? isAllValoracionCero = null;

        #region "CONSTRUCTOR"

        public frmValoracionDetail()
            : base()
        {
            InitializeComponent();
        }

        #endregion






       #region "METODOS PROTEGIDOS-PRIVADOS"

        protected void setUpVariablesFromDwg (string iIdGrupoZonaGis)
        {

            List<string> miLstZonasGisDistintasByDwg;


            miLstZonasGisDistintasByDwg = tadLayLogica.zonaGis.oFactoryZonaGis.getZonasGisVariablesByDwgDistintas(iIdGrupoZonaGis);

            if (miLstZonasGisDistintasByDwg == null || miLstZonasGisDistintasByDwg.Count == 0)
            {
                foreach (var item in mLstTxtPC100)
                {
                    item.Enabled = false;
                    item.textbox.valorDouble = 0;
                   
                }

                isAllValoracionCero = true;
            }
            else
            {
                foreach (var item in mLstTxtPC100)
                {
                    if (string.IsNullOrEmpty((string)item.Tag))
                    {
                        throw new Exception(string.Format(strGeneralUser.uiTAGNoConfig, item.uiLbl));
                    }

                    if (miLstZonasGisDistintasByDwg.Exists(p => p == (string)item.Tag))
                    {
                        item.Enabled = true;
                    }
                    else
                    {
                        item.Enabled = false;
                        item.textbox.valorDouble = 0;
                    }
                }

                isAllValoracionCero = false;
            }


          //  mBindMaster.EndEdit();

        }
       #endregion
       #region "BOTONES"
       //GUARDAR
       protected void save()
       {
           try
           {
               if (isValidoFrm)
               {

                   string miError100 = string.Empty;

                   if (suma100(ref miError100))
                   {
                       ((DataRowView)mBindMaster.Current)["id"] = mId;

                       mBindMaster.EndEdit();

                       ds.saveDataTable(mTabla, true);
                   }
                   else
                   {
                       oTadil.data.UserInfo.showInfo(string.Format(strGeneralUser.uiSumaPorcentajes, miError100));
                   }
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
       //SALIR
       protected void cancel()
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
       #endregion
       #region "EVENTOS FRM"
       protected virtual void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            mBindMaster = null;
        }
        #endregion



  

    }
}
