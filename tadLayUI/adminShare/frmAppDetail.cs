using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI
{

    using tadLayData;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica;
    using tadLayLan;
    
    public partial class frmAppDetail : Form
    {

        protected string mId = "APP";
        protected object[] mItemCurrentIni;
        protected BindingSource mBindMaster;
    
        
        
        
        public frmAppDetail()
        {
            InitializeComponent();       
        }

        #region "Metodos Privados"
        protected virtual bool isValidoFrm
        {
            get
            {
                return oValidar.isValidoGrupoByFrm(this);
            }
        }
        protected virtual void postConstructor()
        {
            throw new NotImplementedException("PostConstructor NO IMPLEMENTADO");

        }
        protected virtual void bindCreate()
        {
            throw new NotImplementedException("BindCreate NO IMPLEMENTADO");

        }
        #endregion
        #region "Botones"
        //GUARDAR
        protected virtual void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (isValidoFrm)
                {
                    ((DataRowView)mBindMaster.Current)["id"] = mId;

                    mBindMaster.EndEdit();

                    mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;
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
        protected virtual void lnkSalir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
        #region "Eventos FRM"
        private void frm_FormClosing(object sender, FormClosingEventArgs e)
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
                            lnkSave_LinkClicked(null, new LinkLabelLinkClickedEventArgs(new LinkLabel.Link()));
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
