using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace tadLayUI.userControl
{
    using engNet.eventos;
    using tadLayData;
    using tadLayLan;
    using tadLayLogica;
    using tadLayLogica.datos.precios;


    public partial class ucEstTipoCode : UserControl, IisValido
    {

        public event EventHandler<oEventArgs<bool>> evIsTunCircular;
        private bool mIsObligatorio;
        private ErrorProvider errorGrupo = new ErrorProvider();
        private ErrorProvider errorItem = new ErrorProvider();

        private dsBd.tbMaterialesDataTable mTbMateriales=null;


        #region "Constructor"
        public ucEstTipoCode()
        {
            InitializeComponent();

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
        public string uiLblGrupo
        {
            get
            {
                return lblGrupo.Text;
            }
            set
            {
                lblGrupo.Text = value;
            }
        }
        [Category("_CADnex")]
        public string uiLblItem
        {
            get
            {
                return lblItem.Text;
            }
            set
            {
                lblItem.Text = value;
            }
        }
        [Category("_CADnex")]
        public ComboBox uiComboGrupo
        {
            get
            {
                return cmbMaster;
            }
            set
            {
                cmbMaster = value;
            }
        }
        [Category("_CADnex")]
        public ComboBox uiComboItem
        {
            get
            {
                return cmbDetail;
            }
            set
            {
                cmbDetail = value;
            }
        }
        [Category("_CADnex")]
        public int ancho
        {
            get { return cmbMaster.Width; }
            set
            {
                cmbMaster.Width = value;
                cmbDetail.Width = value;
            }
        }
        [Category("_CADnex")]
        public Point locationMaster
        {
            get { return cmbMaster.Location; }
            set
            {
                cmbMaster.Location = value;
            }
        }
        [Category("_CADnex")]
        public Point locationDetail
        {
            get { return cmbDetail.Location; }
            set
            {
                cmbDetail.Location = value;        
            }
        }


        public string getGrupoCode()
        {

            short miIdGrupo = (short) this.cmbMaster.SelectedValue;

            return oDalEstructuras.getCodeEstructura(miIdGrupo);
        }

        private dsBd.tbMaterialesDataTable tbMateriales
        {

            get
            {
                if (mTbMateriales == null)
                {
                    mTbMateriales = oDalMateriales.getTbMateriales();
                }

                return mTbMateriales;
 
            }
              
        }


        #endregion
        #region "Interface"
        public bool isValido()
        {
            cmb_Validating(this, new CancelEventArgs());

            if (errorGrupo.GetError(cmbMaster) == "" && errorItem.GetError(cmbDetail) =="")
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

        //LOAD
        private void ucEstTipoCode_Load(object sender, EventArgs e)
        {
            this.CausesValidation = true;
            this.Validating += new CancelEventHandler(cmb_Validating);
        }
        private void cmb_Validating(object sender, CancelEventArgs e)
        {
            if (isObligatorio)
            {

                if (cmbMaster.Text.Trim().Length == 0)
                {
                    errorGrupo.SetIconAlignment(cmbMaster, ErrorIconAlignment.MiddleLeft);
                    errorGrupo.SetError(cmbMaster, strError.eValorObligatorio);
                    return;
                }

                if (cmbDetail.Text.Trim().Length == 0)
                {
                    errorItem.SetIconAlignment(cmbDetail, ErrorIconAlignment.MiddleLeft);
                    errorItem.SetError(cmbDetail, strError.eValorObligatorio);
                    return;
                }

            }
        }
        protected override void OnEnter(EventArgs e)
        {
            errorGrupo.Clear();
            errorItem.Clear();
            base.OnEnter(e);

            if (isObligatorio && cmbDetail.Items.Count == 0)
            {
                errorItem.SetIconAlignment(cmbDetail, ErrorIconAlignment.MiddleLeft);
                errorItem.SetError(cmbDetail, strError.eComboCount0);
                this.CausesValidation = false;
            }

        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            if (cmbMaster.Text.Trim().Length == 0)
            {
                errorGrupo.SetIconAlignment(cmbMaster, ErrorIconAlignment.MiddleLeft);
                errorGrupo.SetError(cmbMaster, strError.eValorObligatorio);
                this.CausesValidation = false;
            }
            else if (cmbDetail.Text.Trim().Length == 0)
            {
                errorItem.SetIconAlignment(cmbDetail, ErrorIconAlignment.MiddleLeft);
                errorItem.SetError(cmbDetail, strError.eValorObligatorio);
                this.CausesValidation = false;
            }
            else
            {
                this.CausesValidation = true;
            }

        }

        #endregion


        /// <summary>
        /// Cargar el Combo (Grupos EST ó TUN)
        /// </summary>
        /// <param name="iIdGrupo">EST o TUN</param>
        public void populate(string iIdGrupo)
        {
            //Vinculo los DATAGRIDVIEW
            cmbMaster.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMaster.DisplayMember = "des";
            cmbMaster.ValueMember = "val";
            cmbMaster.DataSource = oDalEstructuras.getLstEstTipos(iIdGrupo);
            cmbMaster.SelectedIndex = -1; 

            //Detail
            cmbDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDetail.DisplayMember = "nombre";
            cmbDetail.ValueMember = "idMaterial";
            cmbDetail.SelectedIndex = -1;       
        }


        private void cmbMaster_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMaster.SelectedIndex != -1)
            {

                short miId = (short)cmbMaster.SelectedValue;
                
                
                cmbDetail.DataSource = oDalEstructuras.getLstIemsByIdClasificacion(tbMateriales ,miId);
                cmbDetail.SelectedIndex = -1;

                //Lanzo el Evento

                //OJO 14 = CIRCULAR ; 15 = HERRADURA ; 16 == BOVEDA

                if (evIsTunCircular != null)
                {
                    if (miId == 14)
                    {
                        evIsTunCircular(cmbMaster, new oEventArgs<bool>(true));
                    }
                    else
                    { 

                     evIsTunCircular(cmbMaster, new oEventArgs<bool>(false));
                    }                                
                }
            }

        }







  







   
    }


  


}
