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

    using tadLayLogica;
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayUI;
    using tadLayLan;
    using tadLayLan.Tdi;


    public partial class ucSolucionOptAvanzadas : UserControl,IisValido
    {
        #region "Constructores"

        public ucSolucionOptAvanzadas()
        {
            InitializeComponent();

            postConstructor();
        }

        #endregion

        #region "Propiedades"

        public oAbanicoDesign getDataAbanico (eAbanicoTipo iTipoAbanico)
        {

            oAbanicoDesign miAbanico = new oAbanicoDesign(this.ucAbanicoAnguloTotalGrados.valorDouble,
                                                          this.ucAbanicoDiscretizacionGrados.valorDouble,
                                                          this.ucAbanicoTramoDiscretizacionMetros.valorDouble,
                                                          this.ucAbanicoToleranciaPuntoObjetivo.valorDouble,
                                                          this.invalidarTramosIncrementoLongitud,
                                                          this.invalidarTramosTramosIncrementoLongitudPC(),
                                                          iTipoAbanico);


            return miAbanico;

        }

        public bool isAijConstante
        {
            get
            {
                return this.chkRoadConsiderarAijConstante.Checked;
            }
        }

        public double? invalidarTramosTramosIncrementoLongitudPC()
        {

            if (!this.ucInvalidarTramosLongitudPC.isEmptyOrNull())
            {
                return this.ucInvalidarTramosLongitudPC.valorDouble;
            }
            else
            {
                return null;
            }
        }

        public bool invalidarTramosIncrementoLongitud
        {

            get
            {
                return this.chkAbanicoInvalidarTramosIncrementoLongitud.Checked;
            }
        }


        public void Set_ucAbanicoAnguloTotalGrados(double valor)
        {
            this.ucAbanicoAnguloTotalGrados.uitxt = valor.ToString();
        }
        public void Set_ucAbanicoDiscretizacionGrados(double valor)
        {
            this.ucAbanicoDiscretizacionGrados.uitxt = valor.ToString();
        }
        public void Set_ucAbanicoTramoDiscretizacionMetros(double valor)
        {
            this.ucAbanicoTramoDiscretizacionMetros.uitxt = valor.ToString();
        }
        public void Set_ucAbanicoToleranciaPuntoObjetivo(double valor)
        {
            this.ucAbanicoToleranciaPuntoObjetivo.uitxt = valor.ToString();
        }
        public void Set_invalidarTramosIncrementoLongitud(bool valor)
        {
            this.chkAbanicoInvalidarTramosIncrementoLongitud.Checked = valor;
        }
        public void Set_ucInvalidarTramosLongitudPC(string valor)
        {
            this.ucInvalidarTramosLongitudPC.uitxt = valor;
        }
        public void Set_chkRoadConsiderarAijConstante(bool valor)
        {
            this.chkRoadConsiderarAijConstante.Checked = valor;
        }


        #endregion



        #region "Metodos Publicos"

        public bool isValido()
        {
            return oValidar.isValidoGrupoByFrm(this);
        }

        #endregion
        #region "Metodos Privados"

        private void postConstructor()
        {

            #region "Traducción"

            //Abanico
            this.grAbanico.Text = strFrmSolucion.uiGrAbanico;
            this.ucAbanicoAnguloTotalGrados.uiLbl = strFrmSolucion.uiAbaAnguloTotal;
            this.ucAbanicoDiscretizacionGrados.uiLbl = strFrmSolucion.uiAbaGradosDiscretizacion;
            this.ucAbanicoTramoDiscretizacionMetros.uiLbl = strFrmSolucion.uiAbaTramoDiscretizacion;
            this.chkAbanicoInvalidarTramosIncrementoLongitud.Text = strFrmSolucion.uiAbaInvalidarTramosIncrementoLongitud;
            this.ucAbanicoToleranciaPuntoObjetivo.uiLbl = strFrmSolucion.uiAbaToleranciaPuntoObjetivo;

            //Road
            this.chkRoadConsiderarAijConstante.Text = strFrmSolucion.uiRoadAijConsiderarConstante;


            #endregion


            #region "SetUpObjetos"

            //Abanico
            this.ucAbanicoAnguloTotalGrados.textbox.valorDoubleNull = 180;
            this.ucAbanicoDiscretizacionGrados.textbox.valorDoubleNull = 5;
            this.ucAbanicoTramoDiscretizacionMetros.textbox.valorDoubleNull = 20;
            this.chkAbanicoInvalidarTramosIncrementoLongitud.Checked = true;
            this.ucAbanicoToleranciaPuntoObjetivo.textbox.valorDoubleNull = 50;

            //Road
            this.chkRoadConsiderarAijConstante.Checked = false;

            #endregion

        }


        #endregion
        #region "FRM Eventos"

        private void chkAbanicoInvalidarTramosIncrementoLongitud_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAbanicoInvalidarTramosIncrementoLongitud.Checked)
            {
                this.ucInvalidarTramosLongitudPC.textbox.valorDoubleNull = 500;
            }
            else
            {
                this.ucInvalidarTramosLongitudPC.textbox.valorDoubleNull = null;
            }
        }

        #endregion
    }
}
