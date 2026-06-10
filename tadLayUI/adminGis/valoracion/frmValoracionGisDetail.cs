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
    using System.IO;
    using System.Globalization;


    using tadLayUI;

    using tadLayLan;
    using tadLayData;
    using tadLayLogica.datos;
    using tadLayLogica;
    using tadLayUI.userControl;
    using tadLayLogica.datos.BaseDatos;
    
    
    public  partial class frmValoracionGisDetail : frmRoot
    {
        
       
        protected string mId = string.Empty;

        protected List<ucLblTxt> mLstTxtPC100 = new List<ucLblTxt>();
 
        protected object[] mItemCurrentIni;

        private oSingletonDsBd mDs = null;
        protected DataTable mTabla = null;
        protected BindingSource mBindMaster=null;

        protected bool? mIsEnabledSaveCancel = null;




        #region "CONSTRUCTOR"

        public frmValoracionGisDetail()
            : base()
        {
            InitializeComponent();
        }

        #endregion






        #region "METODOS PROTEGIDOS-PRIVADOS"

        protected oSingletonDsBd ds
        {
            get
            {
                return oSingletonDsBd.getInstance;
            }
        }


        protected bool isValor0and10(ref string iGrNotValueMinMax)
        {

            if (mLstTxtPC100.Count == 0)
            {
                throw new Exception("La matriz de Controles PorCiento es Nula");
            }

           //Agrupo los PorCientos por Grupo
            var miQuery = from p in mLstTxtPC100
                          where p.Enabled == true
                          group p by new { p.Parent.Name } into g
                          select new { nombre = g.First().Parent.Text, max = g.Max(p=> p.valorDouble), min =  g.Min(p=> p.valorDouble) };


            string miError= string.Empty;

            foreach (var item in miQuery)
            {
                if (item.max < 10 && item.min == 0)
                {
                    miError = (string.Format("{0}\n  No has definido Valor Máximo de 10\n", item.nombre));
                    iGrNotValueMinMax = iGrNotValueMinMax + miError;    
                }
                if (item.max >= 10 && item.min > 0)
                {
                    iGrNotValueMinMax = (string.Format("{0}\n  No has definido Valor Mínimo de 0\n", item.nombre));
                    iGrNotValueMinMax = iGrNotValueMinMax + miError;           
                }
                if (item.max <10 && item.min>0)
                {
                    iGrNotValueMinMax = string.Format("Las Valoraciones deben de tener un Valor Máximo de 10\n Y Un Valor Mínimo de 0\n");
                    iGrNotValueMinMax = iGrNotValueMinMax + miError;           
                }
            }

            if (string.IsNullOrEmpty(iGrNotValueMinMax))
           {
               return true;
           }
           else
           { 
             return false;
           }

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

                   string miError0to10 = string.Empty;

                   if (isValor0and10(ref miError0to10))
                   {
                       ((DataRowView)mBindMaster.Current)["id"] = mId;

                       mBindMaster.EndEdit();

                       mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


                       ds.saveDataTable(mTabla, true);
                   }
                   else
                   {
                       oTadil.data.UserInfo.showInfo(string.Format("Error :\n {0}", miError0to10));
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
               this.Close();
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
            try
            {
                if (isValidoFrm)
                {
                    mBindMaster.EndEdit();
                    var itemInicial = mItemCurrentIni;
                    var itemFinal = ((DataRowView)mBindMaster.Current).Row.ItemArray;

                    if (!itemInicial.SequenceEqual(itemFinal))
                    {

                        oTadil.data.UserInfo.showInfo(strGeneralUser.uiDatosNoGuardados);

                        DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiDatosGuardar);

                        if (miResul == DialogResult.Yes)
                        {
                            save();
                        }
                        else
                        {
                            oTadil.data.UserInfo.showInfo(strGeneralUser.uiSalirDatosSinGuardar);

                        }

                    }

                }

                //Salir sin Guardar
                else
                {
                    DialogResult miResul = oTadil.data.UserInfo.showSiNo(strGeneralUser.uiSalirPregunta);

                    if (miResul != DialogResult.Yes)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        this.Dispose();
                        this.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }
        #endregion



  

    }
}
