using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.adminSecciones
{
    using System.IO;
    using System.Globalization;

    using tadLayLan;
    using tadLayData;
    using tadLayLogica;
    using Properties;


    using tadLayLogica.datos.BaseDatos;
    using tadLayLogica.datos.Secciones;
    using tadLayLan.img;


    /// <summary>
    /// FRM CUNETAS TRAPEZOIDALES
    /// </summary>
    public partial class frmCunTra : Form
    {

       #region "Variables Privadas"
        private Guid? mId = null;
        private BindingSource mBindMaster;
        #endregion
       #region "Constructor"
        public frmCunTra()
        {
            InitializeComponent();
            frmSetUp();
        }
        #endregion
       #region "Metodos Publicos"

        public void create()
        {
            mId = null;
            bindCreate();
           
        }

        public void edit(Guid iId)
        {
            mId = iId;
            bindCreate();
           
        }



        #endregion
       #region "Metodos Privados"
        private bool isValidoFrm()
        {
            bool miValidoGrupo = oValidar.isValidoGrupoByFrm(this);
            return miValidoGrupo;
        }
        private void frmSetUp()
        {

            //FRM
            var name = oTadil.KAppHeaderName;
            this.Text = name;
            lblHeader.Text = strFrmSecciones.uiTRAPEZ;

            //Grupo Zona
            grGeneral.Text = strFrmSecciones.uiDatos;
            ucNombre.uiLbl = strFrmSecciones.uiNombre;
            ucDescripcion.uiLbl = strFrmSecciones.uiDescripcion;
            ucAlturaIntM.uiLbl = strFrmSecciones.uiAltura;
            ucAnchoIntSupM.uiLbl = strFrmSecciones.uiAnchoSuperior;
            ucAnchoIntInfM.uiLbl = strFrmSecciones.uiAnchoInferior;
            ucEspesorIntM.uiLbl = strFrmSecciones.uiEspesor;

            //Grupo Imagen
            grDetalle.Text = strFrmSecciones.uiImagen;
            //Img
            imgCun.Image = imgCuneta.cunTrapezoidal;
            imgCun.SizeMode = PictureBoxSizeMode.StretchImage;


            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;
            lnkSalir.Text = strGeneral.uiSalir;




        }
        private void bindCreate()
        {
            
                mBindMaster = new BindingSource();
                mBindMaster.DataMember = oDalTabCunTra.getTabla().TableName;
                mBindMaster.DataSource = oDalTabCunTra.getTabla();

                if (mId ==null)
                {
                    mBindMaster.AddNew();
                }
                else
                {
                    string miQuery = "id = '{0}'";
                    mBindMaster.Filter = string.Format(miQuery, Convert.ToString(mId));
                }

               

                //Tabla Zona Geologica
                dsBd.tbCunetaTrapezDataTable miTb = oDalTabCunTra.getTabla();

                //GrupoZona
                ucNombre.textbox.DataBindings.Add("Text", mBindMaster, miTb.nombreColumn.ColumnName);
                ucDescripcion.textbox.DataBindings.Add("Text", mBindMaster, miTb.descripcionColumn.ColumnName);
                ucAlturaIntM.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.alturaIntMColumn.ColumnName, true);
                ucAnchoIntSupM.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.anchoIntSupMColumn.ColumnName, true);
                ucAnchoIntInfM.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.anchoIntInfMColumn.ColumnName, true);
                ucEspesorIntM.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, miTb.espesorMColumn.ColumnName, true);
         
        }
       #endregion
       #region "Botones"
       //GUARDAR
       private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       {
           try
           {

               if (isValidoFrm())
               {
                   if (mId==null)
                   {
                       mId = System.Guid.NewGuid();

                       ((DataRowView)mBindMaster.Current)["id"] = mId;
                       ((DataRowView)mBindMaster.Current)["idTbCunetaTipo"] = "TRAPEZ";

                   }

                   mBindMaster.EndEdit();

                   oDalTabCunTra.saveTabla();
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
       private void lnkSalir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
       #region "Eventos"
       private void frmEst_FormClosing(object sender, FormClosingEventArgs e)
       {
           mBindMaster.CancelEdit();
           mBindMaster.RemoveFilter();
       }
       #endregion

    }
}
