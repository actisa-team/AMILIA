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
    using tadLayLan;
    using tadLayLogica;
    using tadLayLogica.datos.precios;



    /// <summary>
    /// LBL-COMBO MASTER
    /// </summary>
    public partial  class uclblCmbMaster : UserControl, IisValido
    {

        private bool mIsObligatorio;
        private ErrorProvider error = new ErrorProvider();


        #region "Constructor"
        public uclblCmbMaster()
        {
            InitializeComponent();
            this.CausesValidation = true;
            this.Validating += new CancelEventHandler(cmb_Validating);
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
        public virtual string uiLbl
        {
            get
            {
                return lbl.Text;
            }
            set
            {
                lbl.Text = value;
            }
        }
        [Category("_CADnex")]
        public ComboBox uiCombo
        {
            get
            {
                return cmb;
            }
            set
            {
                cmb = value;
            }
        }
        [Category("_CADnex")]
        public int ancho
        {
            get { return cmb.Width; }
            set { cmb.Width = value; }
        }
        [Category("_CADnex")]
        public Point location
        {
            get { return cmb.Location; }
            set { cmb.Location = value; }
        }
        #endregion
        #region "Interface"
        public bool isValido()
        {
            cmb_Validating(this, new CancelEventArgs());

            if (error.GetError(cmb) == "")
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
        protected override void OnEnter(EventArgs e)
        {      
            error.Clear();
            base.OnEnter(e);

            if (isObligatorio && cmb.Items.Count == 0)
            {
                error.SetIconAlignment(cmb, ErrorIconAlignment.MiddleLeft);
                error.SetError(cmb, strError.eComboCount0);
                this.CausesValidation = false;
            }

        }
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            if (isObligatorio && cmb.Text.Trim().Length == 0)
            {
                error.SetIconAlignment(cmb, ErrorIconAlignment.MiddleLeft);
                error.SetError(cmb, strError.eValorObligatorio);
                this.CausesValidation = false;
            }
            else
            {
                this.CausesValidation = true;
            }

        }
        private void cmb_Validating(object sender, CancelEventArgs e)
        {
            if (isObligatorio)
            {
                if (cmb.Text.Trim().Length == 0)
                {
                    error.SetIconAlignment(cmb, ErrorIconAlignment.MiddleLeft);
                    error.SetError(cmb, strError.eValorObligatorio);
                    return;        
                }
             
            }
        }
        #endregion


   
        protected virtual void postConstructor()
        {


        }


        public virtual void populate()
        {

    
        }
   
    }


  


}
