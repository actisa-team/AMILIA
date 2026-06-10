using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.adminGis.valoracion
{

    using tadLayData;
    using tadLayLan;
    using tadLayLogica;
    using tadLayShare;
    
    
    public partial class frmValExcavabilidadTalud : frmValoracionGisDetail
    {
        public frmValExcavabilidadTalud()
        {
            InitializeComponent();
        }


        #region "Botones"

        //SAVE
        void lnkSave_Click(object sender, EventArgs e)
        {
            base.save();
        }


        #endregion

        private void frmValExcavabilidadTalud_Load(object sender, EventArgs e)
        {

            #region "SetUp & Traducccion"

            grValExca.Text = strFrmGisGeo.uiValoracionExcavabilidad;

            ucExcaMediosConvencionales.uiLbl = strFrmGisGeo.uiExcaMediosConvencionales + " [0-10]";
            ucExcaMartillo.uiLbl = strFrmGisGeo.uiExcaMartillo + " [0-10]";
            ucExcaVoladuras.uiLbl = strFrmGisGeo.uiExcaVoladuras + " [0-10]";
            ucExcaNivelFreatico.uiLbl = strFrmGisGeo.uiExcaNivelFreatico + " [0-10]";
            ucExcaVertedero2Fases.uiLbl = strFrmGisGeo.uiExcaVertedero2Fases + " [0-10]";


            grValTalud.Text = strFrmGisGeo.uiValoracionTaludProtecciones;

            ucTaludSinProteccion.uiLbl = strFrmGisGeo.uiTaludProtecSin + " [0-10]";
            ucTaludProteccionFlexibles.uiLbl = strFrmGisGeo.uiTaludProtecFlexibles + " [0-10]";
            ucTaludProteccionesRigidas.uiLbl = strFrmGisGeo.uiTaludProtecRigidas + " [0-10]";

            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion

            #region "Comprobar Valores estan entre 0-10"
            mLstTxtPC100.Add(ucExcaMediosConvencionales);
            mLstTxtPC100.Add(ucExcaMartillo);
            mLstTxtPC100.Add(ucExcaVoladuras);
            mLstTxtPC100.Add(ucExcaNivelFreatico);
            mLstTxtPC100.Add(ucExcaVertedero2Fases);

            mLstTxtPC100.Add(ucTaludSinProteccion);
            mLstTxtPC100.Add(ucTaludProteccionFlexibles);
            mLstTxtPC100.Add(ucTaludProteccionesRigidas);
          
            #endregion


            #region "DataBind"

            mId = "VAEXTA";

            mTabla = ds.dataset.tbValExcavabilidadTalud;

            mBindMaster = new BindingSource();
            mBindMaster.DataMember = mTabla.TableName;
            mBindMaster.DataSource = mTabla;

            if (mTabla.Rows.Count == 0)
            {
                mBindMaster.AddNew();
            }
            else if (mTabla.Rows.Count == 1)
            {
                string miQuery = "id = '{0}'";
                mBindMaster.Filter = string.Format(miQuery, mId);
            }
            else
            {
                throw new oExRowFoundMayorUno("id", mTabla.TableName);
            }


            mItemCurrentIni = ((DataRowView)mBindMaster.Current).Row.ItemArray;


            ////Grupo1
            ucExcaMediosConvencionales.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValExcavabilidadTalud.excMediosConvencionalesColumn.ColumnName, true,DataSourceUpdateMode.OnPropertyChanged);
            ucExcaMartillo.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValExcavabilidadTalud.excMartilloColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);
            ucExcaVoladuras.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValExcavabilidadTalud.excVoladurasColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);
            ucExcaNivelFreatico.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValExcavabilidadTalud.excNivelFreaticoColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);
            ucExcaVertedero2Fases.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValExcavabilidadTalud.excVertedero2FasesColumn.ColumnName, true, DataSourceUpdateMode.OnPropertyChanged);

            //Grupo2
            ucTaludSinProteccion.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValExcavabilidadTalud.talProtecionesSinColumn.ColumnName, true);
            ucTaludProteccionFlexibles.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValExcavabilidadTalud.talProteccionesFlexiblesColumn.ColumnName, true);
            ucTaludProteccionesRigidas.textbox.DataBindings.Add("valorDouble", mBindMaster, ds.dataset.tbValExcavabilidadTalud.talProteccionesRigidasColumn.ColumnName, true);

            #endregion

        }
    }
}
