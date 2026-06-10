using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdb;

namespace tadLayUI.userControl
{
    using engNet.ClassT;
    using engNet.eventos;
    using tadLayData;
    using tadLayLan;
    using tadLayLogica;
    using tadLayLogica.datos;
    using tadLayLogica.datos.Secciones;
    using tadLayLogica.datos.precios;
    using tadLayShare;
    using tadLayLan.Tdi;
    using tadLayLogica.datos.BaseDatos;


    public partial class ucCuneta : UserControl, IisValido
    {

        
        private bool mIsObligatorio;

        private ErrorProvider errorTipo = new ErrorProvider();
        private ErrorProvider errorGeometria = new ErrorProvider();
        private ErrorProvider errorMaterial = new ErrorProvider();

        private List<oValDesFilT<Guid, string,string>> mLstCunetaGeometria = null;
        private List<dsBd.tbMaterialesRow> mTbMatCunTri = null;
        private List<dsBd.tbMaterialesRow> mTbMatCunTra = null;


        #region "Constructor"
        public ucCuneta()
        {
            InitializeComponent();
            postConstructor();

        }
        #endregion

        #region "PostConstructor"

        private void postConstructor()
        {

            this.CausesValidation = true;
            this.Validating += new CancelEventHandler(cmb_Validating);

            //Vinculo los DATAGRIDVIEW
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.DisplayMember = "des";
            cmbTipo.ValueMember = "val";
           
            //Geometria
            cmbGeometria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGeometria.DisplayMember = "des";
            cmbGeometria.ValueMember = "val";
            cmbGeometria.Enabled = false;
            
            //Material
            cmbMaterial.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMaterial.DisplayMember = "nombre";
            cmbMaterial.ValueMember = "idMaterial";
            cmbMaterial.Enabled = false;


            //Traduccion
            lblTipo.Text = strFrmSecciones.uiCunetaTipo;
            lblGeometria.Text = strFrmSecciones.uiCunetaGeometria;
            lblMaterial.Text = strFrmInformes.uiPrecio;
                   
        }



        #endregion



        #region "Propiedades"
        [Category("_CADnex_Data")]
        public bool isObligatorio
        {

            get
            {
                return mIsObligatorio;
            }
            set
            {
                mIsObligatorio = value;

            }

        }

        [Category("_CADnex")]
        public ComboBox uiComboTipo
        {
            get
            {
                return cmbTipo;
            }
            set
            {
                cmbTipo = value;
            }
        }
        [Category("_CADnex")]
        public ComboBox uiComboGeo
        {
            get
            {
                return cmbGeometria;
            }
            set
            {
                cmbGeometria = value;
            }
        }

        [Category("_CADnex")]
        public ComboBox uiComboMat
        {
            get
            {
                return cmbMaterial;
            }
            set
            {
                cmbMaterial = value;
            }
        }

        [Category("_CADnex")]
        public int ancho
        {
            get { return cmbTipo.Width; }
            set
            {
                cmbTipo.Width = value;
                cmbGeometria.Width = value;
                cmbMaterial.Width = value;
            }
        }
        [Category("_CADnex")]
        public Point locationTipo
        {
            get { return cmbTipo.Location; }
            set
            {
                cmbTipo.Location = value;
            }
        }
        [Category("_CADnex")]
        public Point locationGeo
        {
            get { return cmbGeometria.Location; }
            set
            {
                cmbGeometria.Location = value;        
            }
        }

        [Category("_CADnex")]
        public Point locationMat
        {
            get { return cmbMaterial.Location; }
            set
            {
                cmbMaterial.Location = value;
            }
        }



        #endregion
        #region "Interface"
        public bool isValido()
        {
            cmb_Validating(this, new CancelEventArgs());

            if (errorTipo.GetError(cmbTipo) == "" && errorGeometria.GetError(cmbGeometria) =="" && errorMaterial.GetError(cmbMaterial)=="")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region "Eventos"

        private void cmb_Validating(object sender, CancelEventArgs e)
        {
            if (isObligatorio)
            {

                if (cmbTipo.Text.Trim().Length == 0)
                {
                    errorTipo.SetIconAlignment(cmbTipo, ErrorIconAlignment.MiddleLeft);
                    errorTipo.SetError(cmbTipo, strError.eValorObligatorio);
                    return;
                }

                if (cmbGeometria.Text.Trim().Length == 0)
                {
                    errorGeometria.SetIconAlignment(cmbGeometria, ErrorIconAlignment.MiddleLeft);
                    errorGeometria.SetError(cmbGeometria, strError.eValorObligatorio);
                    return;
                }

                if (cmbMaterial.Text.Trim().Length == 0)
                {
                    errorMaterial.SetIconAlignment(cmbGeometria, ErrorIconAlignment.MiddleLeft);
                    errorMaterial.SetError(cmbGeometria, strError.eValorObligatorio);
                    return;
                }

            }
        }
        protected override void OnEnter(EventArgs e)
        {
            errorTipo.Clear();
            errorGeometria.Clear();
            errorMaterial.Clear();
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            
            
            base.OnLeave(e);

            if (isObligatorio)
            {

                if (cmbTipo.Text.Trim().Length == 0)
                {
                    errorTipo.SetIconAlignment(cmbTipo, ErrorIconAlignment.MiddleLeft);
                    errorTipo.SetError(cmbTipo, strError.eValorObligatorio);
                    this.CausesValidation = false;
                }
                else if (cmbGeometria.Text.Trim().Length == 0)
                {
                    errorGeometria.SetIconAlignment(cmbGeometria, ErrorIconAlignment.MiddleLeft);
                    errorGeometria.SetError(cmbGeometria, strError.eValorObligatorio);
                    this.CausesValidation = false;
                }
                else if (cmbMaterial.Text.Trim().Length == 0)
                {
                    errorMaterial.SetIconAlignment(cmbGeometria, ErrorIconAlignment.MiddleLeft);
                    errorMaterial.SetError(cmbGeometria, strError.eValorObligatorio);
                    this.CausesValidation = false;
                }
                else
                {
                    this.CausesValidation = true;
                }
            }

        }

        #endregion


        /// <summary>
        /// Cargar el Combo (Grupos EST ó TUN)
        /// </summary>
        /// <param name="iIdGrupo">EST o TUN</param>
        public void populate()
        {

            oSingletonDsBd miDs = oSingletonDsBd.getInstance;

            //El Datasource lanza el evento SelectIndexChanged
            cmbTipo.SelectedIndexChanged -= new EventHandler(cmbTipo_SelectedIndexChanged);

            cmbTipo.DataSource = oDalTbCunetaTipos.getLstCunetasTipos(miDs.dataset.tbCunetaTipo);

            mLstCunetaGeometria = oDalTabCunTri.getLstCunetaGeometria(miDs.dataset.tbCunetaTriang, miDs.dataset.tbCunetaTrapez);

            mTbMatCunTri = oDalMateriales.getLstMaterialesCunetraTriangular(miDs.dataset.tbMateriales);
            mTbMatCunTra = oDalMateriales.getLstMaterialesCunetraTrapezoidal(miDs.dataset.tbMateriales);

            

            cmbTipo.SelectedIndexChanged +=new EventHandler(cmbTipo_SelectedIndexChanged);

            cmbTipo.SelectedIndex = -1;
            cmbGeometria.SelectedIndex = -1;
            cmbMaterial.SelectedIndex = -1;
         
        }

        private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

          
            if (cmbTipo.SelectedIndex != -1)
            {

                cmbGeometria.Enabled = true;
                cmbMaterial.Enabled = true;
                

                string miCunetaTipo  = (string)cmbTipo.SelectedValue;

                
                

                if (miCunetaTipo == "TRIANG")
                {
                    var miquery1 = from p in mLstCunetaGeometria where p.fil == "TRIANG" select p;
                    cmbGeometria.DataSource = miquery1.ToList();
                    cmbMaterial.DataSource = mTbMatCunTri;
                }
                else if (miCunetaTipo == "TRAPEZ")
                {
                    var miquery2 = from p in mLstCunetaGeometria where p.fil == "TRAPEZ" select p;
                    cmbGeometria.DataSource = miquery2.ToList();
                    
                    cmbMaterial.DataSource = mTbMatCunTra;
                   
                }
                else
                {
                    throw new oExEnumNotImplemented(miCunetaTipo);                            
                }

                cmbGeometria.SelectedIndex = -1;
                cmbMaterial.SelectedIndex = -1;

            }

        }




    }


  


}
