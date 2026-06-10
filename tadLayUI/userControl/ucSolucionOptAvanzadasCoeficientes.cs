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
    using tadLayLogica.logica.EjeBasicoNew;
    using tadLayLogica;
    using tadLayUI;
    using tadLayLan;
    using tadLayShare;
    using tadLayLan.Tdi;
    
    public partial class ucSolucionOptAvanzadasCoeficientes : UserControl, IisValido

    {
        
        public ucSolucionOptAvanzadasCoeficientes()
        {
            InitializeComponent();
            postConstructor();
        }


        #region "Propiedades-MetodosPublicos"


        public double coeMinPendienteCarretera
        {

            get
            {
                if (!this.ucCoeRoadPendienteMaxima.isEmptyOrNull())
                {
                    return this.ucCoeRoadPendienteMaxima.valorDouble;

                }
                else
                {
                    throw new oExPropertieNullValue("coeMinPendienteCarretera");
                }
            }
        }
        public double coeMinPendienteEstructura
        {

            get
            {
                if (!this.ucCoeEstructurasPendienteMaxima.isEmptyOrNull())
                {
                    return this.ucCoeEstructurasPendienteMaxima.valorDouble;

                }
                else
                {
                    throw new oExPropertieNullValue("coeMinPendienteEstructuras");
                }
            }
        }
        public void Set_coeMinPendienteCarretera(double valor)
        {
            this.ucCoeRoadPendienteMaxima.uitxt = valor.ToString();
        }
        public void Set_coeMinPendienteEstructura(double valor)
        {
            this.ucCoeEstructurasPendienteMaxima.uitxt = valor.ToString();
        }
        public void Set_ucCoeDesmonteAlturaMaxima(double valor)
        {
            this.ucCoeDesmonteAlturaMaxima.uitxt = valor.ToString();
        }
        public void Set_ucCoeTerraplenAlturaMaxima(double valor)
        {
            this.ucCoeTerraplenAlturaMaxima.uitxt = valor.ToString();
        }
        public void Set_ucCoePilaAlturaMaxima(double valor)
        {
            this.ucCoePilaAlturaMaxima.uitxt = valor.ToString();
        }

        public oCoeMinoracionAlturasMaximas getCoeMinoracionAlturasMaximas ()
        {

            oCoeMinoracionAlturasMaximas miCoeMinAlturasMaximas = new oCoeMinoracionAlturasMaximas(this.ucCoeDesmonteAlturaMaxima.valorDouble,
                                                                                                   this.ucCoeTerraplenAlturaMaxima.valorDouble,
                                                                                                   this.ucCoePilaAlturaMaxima.valorDouble);



            return miCoeMinAlturasMaximas;

        }



        public bool isValido()
        {
            return oValidar.isValidoGrupo(this.grCoeficientesMinoracion);
        }


        



        #endregion



        private void postConstructor()
        {
            #region "Traduccion"

            //Coeficientes
            this.grCoeficientesMinoracion.Text = strFrmSolucion.uiGrCoeficientesMinoracion;

            this.ucCoeDesmonteAlturaMaxima.uiLbl = strFrmSolucion.uiCoeMinDesmonteAlturaMaxima;
            this.ucCoeTerraplenAlturaMaxima.uiLbl = strFrmSolucion.uiCoeMinTerraplenAlturaMaxima;

            this.ucCoeRoadPendienteMaxima.uiLbl = strFrmSolucion.uiCoeMinPendienteMaximaRoad;
            this.ucCoeEstructurasPendienteMaxima.uiLbl = strFrmSolucion.uiCoeMinPendienteMaximaEstructuras;


            this.ucCoePilaAlturaMaxima.uiLbl = strFrmSolucion.uiCoeMinAlturaMaximaPila;


            #endregion
            #region "PostConstructor"
            this.ucCoeDesmonteAlturaMaxima.textbox.valorDoubleNull = 0.85;
            this.ucCoeTerraplenAlturaMaxima.textbox.valorDoubleNull = 0.85;
            this.ucCoeRoadPendienteMaxima.textbox.valorDoubleNull = 0.85;
            this.ucCoeEstructurasPendienteMaxima.textbox.valorDoubleNull = 0.85;
            this.ucCoePilaAlturaMaxima.textbox.valorDoubleNull = 0.85;
            #endregion
        }


    }
}
