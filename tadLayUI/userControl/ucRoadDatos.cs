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
    using tadLayUI;
    using tadLayLogica;
    using tadLayLan;
    using engNet.Extension.Double;
    using tadLayShare;
    using tadLayLan.Tdi;
    
    
    public partial class ucRoadDatos : UserControl, IisValido
    {
        private double _radioNormativa;
        #region "Constructor"
        public ucRoadDatos()
        {
            InitializeComponent();

            postConstructor();
        }
        #endregion
        #region "Propiedades"

        /*
         * Comentado para poner los metodos con un SET ** juanma **
         */
        /*
        public eRoadGrupo roadGrupo
        {
            get
            {
                if (! ucRoadGrupo.isEmptyOrNull())
                {

                   return   (eRoadGrupo)Enum.Parse(typeof(eRoadGrupo), ucRoadGrupo.uitxt, true);

                }
                else
                {
                    throw new oExPropertieNullValue("Grupo Carreteras");
                }       
            }
        }
        public double vp
        {
            get
            {
                if (!this.ucVp.isEmptyOrNull())
                {
                    return this.ucVp.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Velocidad Proyecto");
                }
            }


        }
        public double rp
        {
            get
            {
                if (!this.ucRadio.isEmptyOrNull())
                {
                    return this.ucRadio.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Radio Proyecto");
                }
            }


        }
        public double peraltePC
        {

            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucPeraltePC.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Peralte");
                }


            }

        }
        public double ajMin
        {

            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucAijMinimoTramo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("AjMinimo");
                }


            }

        }
        public double ajMax
        {

            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucAijMaximoTramo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("AjMinimo");
                }


            }

        }
        public double ajMinSalidaLlegada
        {

            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucValorMinimoSalidaLlegada.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("ajMinSalidaLlegada");
                }


            }

        }
        public double ucAMax
        {

            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucAvanceMaximo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("ajMinSalidaLlegada");
                }


            }

        }
        public eRoadPreferencias preferencias
        {
            get
            {
                return this.ucRoadPreferenciasRectasCurvas1.valor;
            }
        }
        public bool allowReducionesPuntualesVelocidad
        {
            get
            {
               return this.chkPermitirReduccionesVelocidad.Checked;
            }
        }
        public eKvPrefer preferenciasKv
        {

            get
            {
                return this.ucRoadPrerenciasKv1.valor;
            }
        }
        public double kvConvexo
        {
            get
            {
                if (!this.ucKvConvexo.isEmptyOrNull())
                {
                    return this.ucKvConvexo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Convexo");
                }
            }


        }
        public double kvConcavo
        {
            get
            {
                if (!this.ucKvConcavo.isEmptyOrNull())
                {
                    return this.ucKvConcavo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Concavo");
                }
            }


        }
        public bool chkPermitirRedVel
        {
            get
            {
                if (!this.ucKvConcavo.isEmptyOrNull())
                {
                    return this.chkPermitirReduccionesVelocidad.Checked;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Concavo");
                }
            }


        }
        public bool ucRadCondLmin
        {
            get
            {
                if (!this.ucKvConcavo.isEmptyOrNull())
                {
                    return this.ucRadioCondicionadoLmin.Checked;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Concavo");
                }
            }


        }
        */
        public eRoadGrupo roadGrupo
        {
            get
            {
                if (!ucRoadGrupo.isEmptyOrNull())
                {
                    return (eRoadGrupo)Enum.Parse(typeof(eRoadGrupo), ucRoadGrupo.uitxt, true);
                }
                else
                {
                    throw new oExPropertieNullValue("Grupo Carreteras");
                }
            }
            set
            {
                ucRoadGrupo.uitxt = value.ToString();
            }
        }

        public double vp
        {
            get
            {
                if (!this.ucVp.isEmptyOrNull())
                {
                    return this.ucVp.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Velocidad Proyecto");
                }
            }
            set
            {
                this.ucVp.uitxt = value.ToString();
            }
        }

        public double rp
        {
            get
            {
                if (!this.ucRadio.isEmptyOrNull())
                {
                    return this.ucRadio.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Radio Proyecto");
                }
            }
            set
            {
                this.ucRadio.uitxt = value.ToString();
            }
        }

        public double peraltePC
        {
            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucPeraltePC.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Peralte");
                }
            }
            set
            {
                this.ucPeraltePC.uitxt = value.ToString();
            }
        }

        public double ajMin
        {
            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucAijMinimoTramo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("AjMinimo");
                }
            }
            set
            {
                this.ucAijMinimoTramo.uitxt = value.ToString();
            }
        }

        public double ajMax
        {
            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucAijMaximoTramo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("AjMinimo");
                }
            }
            set
            {
                this.ucAijMaximoTramo.uitxt = value.ToString();
            }
        }

        public double ajMinSalidaLlegada
        {
            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucValorMinimoSalidaLlegada.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("ajMinSalidaLlegada");
                }
            }
            set
            {
                this.ucValorMinimoSalidaLlegada.uitxt = value.ToString();
            }
        }

        public double ucAMax
        {
            get
            {
                if (!this.ucPeraltePC.isEmptyOrNull())
                {
                    return this.ucAvanceMaximo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("ajMinSalidaLlegada");
                }
            }
            set
            {
                this.ucAvanceMaximo.uitxt = value.ToString();
            }
        }

        public eRoadPreferencias preferencias
        {
            get
            {
                return this.ucRoadPreferenciasRectasCurvas1.valor;
            }
        }

        public bool allowReducionesPuntualesVelocidad
        {
            get
            {
                return this.chkPermitirReduccionesVelocidad.Checked;
            }
            set
            {
                this.chkPermitirReduccionesVelocidad.Checked = value;
            }
        }

        public eKvPrefer preferenciasKv
        {
            get
            {
                return this.ucRoadPrerenciasKv1.valor;
            }
        }

        public double kvConvexo
        {
            get
            {
                if (!this.ucKvConvexo.isEmptyOrNull())
                {
                    return this.ucKvConvexo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Convexo");
                }
            }
            set
            {
                this.ucKvConvexo.uitxt = value.ToString();
            }
        }

        public double kvConcavo
        {
            get
            {
                if (!this.ucKvConcavo.isEmptyOrNull())
                {
                    return this.ucKvConcavo.valorDouble;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Concavo");
                }
            }
            set
            {
                this.ucKvConcavo.uitxt = value.ToString();
            }
        }

        public bool chkPermitirRedVel
        {
            get
            {
                if (!this.ucKvConcavo.isEmptyOrNull())
                {
                    return this.chkPermitirReduccionesVelocidad.Checked;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Concavo");
                }
            }
            set
            {
                this.chkPermitirReduccionesVelocidad.Checked = value;
            }
        }

        public bool ucRadCondLmin
        {
            get
            {
                if (!this.ucKvConcavo.isEmptyOrNull())
                {
                    return this.ucRadioCondicionadoLmin.Checked;
                }
                else
                {
                    throw new oExPropertieNullValue("Kv Concavo");
                }
            }
            set
            {
                this.ucRadioCondicionadoLmin.Checked = value;
            }
        }



        /*
         * ** juanma **
         */


        public string nombreSolucion ()
        {

            return this.ucRoadGrupo.textbox.Text + "-V" +
                   this.ucVp.valorInt.ToString() + "-" +
                   this.ucRoadPreferenciasRectasCurvas1.uiCombo.Text + "-" +
                   this.ucRoadPrerenciasKv1.uiCombo.Text + "-";

        }
   




        #endregion
        #region "MetodosPublicos"

        public bool isValido()
        {
            return oValidar.isValidoGrupoByFrm(this);
        }


  

        #endregion


        #region "MetodosPrivados"
        private void postConstructor()
        {

            #region "Traducción"

            this.btnRoadSelect.Text = strFrmSolucion.uiRoadSelect;

            this.ucRoadGrupo.uiLbl = strFrmSolucion.uiRoadGrupo;
            this.ucVp.uiLbl = strFrmSolucion.uiRoadVelocidad;
            this.ucRadio.uiLbl = strFrmSolucion.uiRoadRadio;
            this.ucPeraltePC.uiLbl = strFrmSolucion.uiPeraltePC;

            this.ucRoadPreferenciasRectasCurvas1.uiLbl = strFrmSolucion.uiRoadPreferencias;
            this.ucValorMinimoSalidaLlegada.uiLbl = strFrmSolucion.uiRoadValorMinimoSalidaLlegada;
            this.ucAijMinimoTramo.uiLbl = strFrmSolucion.uiRoadAijMinimo;
            this.ucAijMaximoTramo.uiLbl = strFrmSolucion.uiRoadAijMaximo;
            this.ucAvanceMaximo.uiLbl = strFrmSolucion.uiRoadAvanceMaximo;
            this.chkPermitirReduccionesVelocidad.Text = strFrmSolucion.uiRoadPermitirReduccionesVelocidad;
            this.ucKvConvexo.uiLbl = strFrmSolucion.uiRoadKvConvexo;
            this.ucKvConcavo.uiLbl = strFrmSolucion.uiRoadKvConcavo;
            this.ucRadioCondicionadoLmin.Text = strFrmSolucion.ucRadioCondicionadoLmin;

            #endregion


            #region "SetUpObjetos"

            this.ucRoadGrupo.textbox.ReadOnly = true;
            this.ucVp.textbox.ReadOnly = true;
            this.ucRadio.textbox.ReadOnly = true;
            this.ucPeraltePC.textbox.ReadOnly = true;
            this.ucAijMaximoTramo.textbox.ReadOnly = true;
            this.ucAvanceMaximo.textbox.ReadOnly = true;
            this.ucKvConvexo.textbox.ReadOnly = true;
            this.ucKvConcavo.textbox.ReadOnly = true;

            this.ucRoadPreferenciasRectasCurvas1.populate();
            this.ucRoadPrerenciasKv1.populate();
            this.ucRoadPreferenciasRectasCurvas1.uiCombo.SelectedIndexChanged += new EventHandler(uiComboPreferenciasRectasCurvas_SelectedIndexChanged);
            this.ucRoadPrerenciasKv1.uiCombo.SelectedIndexChanged += new EventHandler(uiComboPreferenciasKv_SelectedIndexChanged);

            this.ucRoadPreferenciasRectasCurvas1.uiCombo.SelectedIndex = 0;
            this.ucRoadPrerenciasKv1.uiCombo.SelectedIndex = 0;
            
          
            #endregion
        }

        /// <summary>
        /// Update Valores AijMin,Max
        /// </summary>
        private void updateAijMinAvanceMaximo()
        {
            if (!ucVp.isEmptyOrNull() && !ucRadio.isEmptyOrNull() && this.ucRoadPreferenciasRectasCurvas1.uiCombo.SelectedIndex != -1)
            {
                using (oRoadDes miRoad = new oRoadDes(this.roadGrupo, this.ucVp.valorDouble, _radioNormativa, this.ucRoadPreferenciasRectasCurvas1.valor, ucRadioCondicionadoLmin.Checked))
                {
                    this.ucRadio.textbox.valorDouble = miRoad.Rp;
                    this.ucValorMinimoSalidaLlegada.textbox.valorDouble = miRoad.AijMinSalidaLlegada;
                    this.ucAijMinimoTramo.textbox.valorDouble = miRoad.AijMin;
                    this.ucAijMaximoTramo.textbox.valorDouble = miRoad.AijMax;
                    this.ucAvanceMaximo.textbox.valorDouble = miRoad.avanceMax;
                }

            }

        }
        #endregion


        #region "Botones"
        /// <summary>
        /// OPEN FRM ROAD NORMAS
        /// </summary>
        private void btnRoadSelect_Click(object sender, EventArgs e)
        {
            try
            {
                frmRoad miFrmRoad = new frmRoad(oTadil.data.Files.fileNormasCarreteras, true);

                if (miFrmRoad.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.ucRoadGrupo.uitxt  = miFrmRoad.grupo.ToString();
                    this.ucVp.textbox.valorDouble = miFrmRoad.velocidad;
                    this.ucRadio.textbox.valorDouble = miFrmRoad.radio;
                    _radioNormativa = miFrmRoad.radio;
                    this.ucPeraltePC.textbox.valorDouble = miFrmRoad.peralte;

                    //Update Formulas
                    updateAijMinAvanceMaximo();

                    //Kv --> Función de la Velocidad
                    uiComboPreferenciasKv_SelectedIndexChanged(this.ucRoadPrerenciasKv1.uiCombo as object, new EventArgs()); 
                }


            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }


        }
        #endregion

        #region "Eventos"

        //PREFERENCIAS RECTAS-CURVAS
        void uiComboPreferenciasRectasCurvas_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateAijMinAvanceMaximo();
        }

        //PREFERENCIAS KV
        void uiComboPreferenciasKv_SelectedIndexChanged(object sender, EventArgs e)
        {
      
             if (! ucVp.isEmptyOrNull() && this.ucRoadPrerenciasKv1.uiCombo.SelectedIndex != -1)
            {

                using (oKv miKv = tadLayLogica.datos.normas.oDalTbNormasKv.getKvFromXmlProyecto(this.ucVp.valorDouble, this.ucRoadPrerenciasKv1.valor))
                {
                    this.ucKvConvexo.textbox.valorDouble = miKv.kvConvexo;
                    this.ucKvConcavo.textbox.valorDouble = miKv.KvConcavo;
                }
                               
            }
        }


        #endregion

        private void ucRadioCondicionadoLmin_CheckedChanged(object sender, EventArgs e)
        {
            updateAijMinAvanceMaximo();
        }


    }
}
