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
    using tadLayLan;
    using tadLayLan.Tdi;

    public partial class ucRoadEstructurasPendientes : UserControl, IisValido
    {
        public ucRoadEstructurasPendientes()
        {
            InitializeComponent();

            postConstructor();
        }



        #region "Metodos Publicos"


        public oRoadPendientes getRoadPendiente()
        {

            oRoadPendientes miRoadPendientes = new oRoadPendientes(this.ucRoadPendienteMaximaPC.valorDouble,
                                                                    this.ucRoadPendienteMinimaPC.valorDouble,
                                                                    this.ucEstructurasPendienteMaximaPC.valorDouble,
                                                                    this.ucEstructurasPendienteMinimoPC.valorDouble);

            return miRoadPendientes;


        }

        public bool isValido()
        {
            return oValidar.isValidoGrupoByFrm(this);
        }


        public void Set_ucRoadPendienteMaximaPC(double valor)
        {
            this.ucRoadPendienteMaximaPC.uitxt = valor.ToString();
        }
        public void Set_ucRoadPendienteMinimaPC(double valor)
        {
            this.ucRoadPendienteMinimaPC.uitxt = valor.ToString();
        }
        public void Set_ucEstructurasPendienteMaximaPC(double valor)
        {
            this.ucEstructurasPendienteMaximaPC.uitxt = valor.ToString();
        }
        public void Set_ucEstructurasPendienteMinimoPC(double valor)
        {
            this.ucEstructurasPendienteMinimoPC.uitxt = valor.ToString();
        }
        #endregion



        private void postConstructor()
        {

            #region "Traducción"

            grRoadPendientes.Text = strFrmSolucion.uiGrRoadPendientes;

            this.ucRoadPendienteMaximaPC.uiLbl = strFrmSolucion.uiRoadPendienteMaximaPC;
            this.ucRoadPendienteMinimaPC.uiLbl = strFrmSolucion.uiRoadPendienteMinimaPC;

            this.grEstructurasPendiente.Text = strFrmSolucion.uiGrEstructurasPendiente;

            this.ucEstructurasPendienteMaximaPC.uiLbl = strFrmSolucion.uiEstructurasPendienteMaximaPC;
            this.ucEstructurasPendienteMinimoPC.uiLbl = strFrmSolucion.uiEstructurasPendienteMinimaPC;


            #endregion


            #region "SetUp Objetos"

            this.ucRoadPendienteMaximaPC.textbox.valorDoubleNull = 5;
            this.ucRoadPendienteMinimaPC.textbox.valorDoubleNull = 0.5;

            this.ucEstructurasPendienteMaximaPC.textbox.valorDoubleNull = 2;
            this.ucEstructurasPendienteMinimoPC.textbox.valorDoubleNull = 0.5;

            #endregion

        }











    }
}
