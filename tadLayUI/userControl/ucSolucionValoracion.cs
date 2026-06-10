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
    using tadLayUI;
    using tadLayLogica;
    using tadLayLan;
    using tadLayLan.Tdi;
    
    
    public partial class ucSolucionValoracion : UserControl , IisValido
    {
        public ucSolucionValoracion()
        {
            InitializeComponent();

            postConstructor();
        }



        #region "Metodos Publicos"


        public string nombreValoracion ()
        {

            return this.ucValoracionDistanciaPC.valorInt.ToString() + "-" +
                    this.ucValoracionPendientePC.valorInt.ToString() + "-" +
                    this.ucValoracionCostesPC.valorInt.ToString() + "-";

        }


        public oTramosValoracion getValoracion()
        {


            oTramosValoracion miValoracion = new oTramosValoracion(this.ucValoracionDistanciaPC.valorDouble,
                                                                   this.ucValoracionPendientePC.valorDouble,
                                                                   this.ucValoracionCostesPC.valorDouble);



            return miValoracion;

        }





        public bool isValido()
        {
            if (oValidar.isValidoGrupo(this.grValoracion) && isSumaValoracion100())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void Set_ucValoracionDistanciaPC(double valor)
        {
            this.ucValoracionDistanciaPC.uitxt = valor.ToString();
        }
        public void Set_ucValoracionPendientePC(double valor)
        {
            this.ucValoracionPendientePC.uitxt = valor.ToString();
        }
        public void Set_ucValoracionCostesPC(double valor)
        {
            this.ucValoracionCostesPC.uitxt = valor.ToString();
        }
        #endregion



        #region "Metodos Privados"

        private void postConstructor()
        {

            #region "Traduccion"

            this.grValoracion.Text = strFrmSolucion.uiGrValoracion;

            this.ucValoracionDistanciaPC.uiLbl = strFrmSolucion.uiValoracionDistanciaPC;
            this.ucValoracionPendientePC.uiLbl = strFrmSolucion.uiValoracionPendientePC;
            this.ucValoracionCostesPC.uiLbl = strFrmSolucion.uiValoracionCostesPC;

            #endregion



            #region "SetUp Objetos"

            this.ucValoracionDistanciaPC.textbox.valorDoubleNull = 60;
            this.ucValoracionPendientePC.textbox.valorDoubleNull = 10;
            this.ucValoracionCostesPC.textbox.valorDoubleNull = 30;

            #endregion



            #region "Bind"


            #endregion


        }
        private bool isSumaValoracion100 ()
        {

            double miValorDistancia = this.ucValoracionDistanciaPC.valorDouble;
            double miValorPendiente = this.ucValoracionPendientePC.valorDouble;
            double miValorCoste = this.ucValoracionCostesPC.valorDouble;
                

            double miSum = miValorDistancia+miValorPendiente+miValorCoste;

            if (miSum == 100)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
 

        #endregion










    }
}
