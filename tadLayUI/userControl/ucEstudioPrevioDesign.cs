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

    using tadLayLogica.estudioTipo;
    using tadLayLogica;
    using tadLayLan;
    using tadLayLan.Tdi;

    
    
    public partial class ucEstudioPrevioDesign : UserControl, IisValido
    {
        #region "Constructor"

        public ucEstudioPrevioDesign()
        {
            InitializeComponent();
            postConstructor();
        }

        #endregion


        #region "Propiedades"



        public IEstudio getEstudioPrevio (oCoeMinoracionAlturasMaximas iCoeMinoracionAlturasMaximas)
        {

            ////Datos de la Seccion
            oEstudioRoadSeccion miRoadSeccion = new oEstudioRoadSeccion(Guid.NewGuid(),
                                                                        strFrmDatosProyecto.uiESTPRE,
                                                                        this.ucAnchoPlataforma.valorDouble, 
                                                                        this.ucCosteImplantacion.valorDouble);



            //Datos del Movimiento de Tierras
            oEstudioMovimientoTierras miMovTierras = new oEstudioMovimientoTierras(Guid.NewGuid(),
                                                                                    strFrmDatosProyecto.uiESTPRE,
                                                                                    this.ucDesmonteAlturaMaxima.valorDouble,
                                                                                    iCoeMinoracionAlturasMaximas.desmonteCoeficienteMinoracionAlturaMaxima,
                                                                                    this.ucDesmonteTaludPC.valorDouble,
                                                                                    this.ucCosteDesmonteUnitario.valorDouble,
                                                                                    this.ucTerraplenAlturaMaxima.valorDouble,
                                                                                    iCoeMinoracionAlturasMaximas.terraplenCoeficienteMinoracionAlturaMaxima,
                                                                                    this.ucTerraplenTaludPC.valorDouble,
                                                                                    this.ucCosteTerraplenUnitario.valorDouble);



            //Datos del Puentes 
            oEstudioPuentes miPuentes = new oEstudioPuentes(Guid.NewGuid(),
                                                            strFrmDatosProyecto.uiESTPRE,
                                                            strFrmDatosProyecto.uiESTPRE,
                                                            this.ucGenerarPuentes.valor,
                                                            this.ucPuenteAlturaMaxima.valorDoubleNull,
                                                            iCoeMinoracionAlturasMaximas.pilaCoeficienteMinoracionAlturaMaxima,
                                                            this.ucCostePuentesViaductosUnitario.valorDoubleNull);


            //Datos de los Tuneles
            oEstudioTuneles miTunel = new oEstudioTuneles(Guid.NewGuid(),
                                                        strFrmDatosProyecto.uiESTPRE,
                                                        strFrmDatosProyecto.uiESTPRE,
                                                        this.ucGenerarTuneles.valor,
                                                        this.ucCosteTunelesUnitario.valorDoubleNull);

            //Creo el Objeto Estudio
            oEstudioPrevioCarretera miEstudioPrevio = new oEstudioPrevioCarretera(miRoadSeccion, miMovTierras, miPuentes, miTunel);


            return miEstudioPrevio;

        }




        #endregion

        #region "Metodos Publicos"

        public bool isValido()
        {
            return oValidar.isValidoGrupoByFrm(this);
        }

        public double getucAnchoPlataforma()
        {
            return this.ucAnchoPlataforma.valorDouble;
        }
        public double getucDesmonteAlturaMaxima()
        {
            return this.ucDesmonteAlturaMaxima.valorDouble;
        }
        public double getucTerraplenAlturaMaxima()
        {
            return this.ucTerraplenAlturaMaxima.valorDouble;
        }
        public double getucDesmonteTaludPC()
        {
            return this.ucDesmonteTaludPC.valorDouble;
        }
        public double getucTerraplenTaludPC()
        {
            return this.ucTerraplenTaludPC.valorDouble;
        }
        public string getucGenerarPuntes()
        {
            return this.ucGenerarPuentes.ToString();
        }
        public string getucGenerarTuneles()
        {
            return this.ucGenerarTuneles.ToString();
        }

        public double getucPuenteAlturaMaxima()
        {
            return this.ucPuenteAlturaMaxima.valorDouble;
        }
        public double getucCosteImplantacion()
        {
            return this.ucCosteImplantacion.valorDouble;
        }
        public double geucCosteDesmonteUnitario()
        {
            return this.ucCosteDesmonteUnitario.valorDouble;
        }
        public double getucCosteTerraplenUnitario()
        {
            return this.ucCosteTerraplenUnitario.valorDouble;
        }
        public double getucCostePuentesViaductosUnitario()
        {
            return this.ucCostePuentesViaductosUnitario.valorDouble;
        }
        public double getucCosteTunelesUnitario()
        {
            return this.ucCosteTunelesUnitario.valorDouble;
        }
        public void setucAnchoPlataforma(double value)
        {
            this.ucAnchoPlataforma.uitxt = value.ToString();
        }

        public void setucDesmonteAlturaMaxima(double value)
        {
            this.ucDesmonteAlturaMaxima.uitxt = value.ToString();
        }

        public void setucTerraplenAlturaMaxima(double value)
        {
            this.ucTerraplenAlturaMaxima.uitxt = value.ToString();
        }

        public void setucDesmonteTaludPC(double value)
        {
            this.ucDesmonteTaludPC.uitxt = value.ToString();
        }

        public void setucTerraplenTaludPC(double value)
        {
            this.ucTerraplenTaludPC.uitxt = value.ToString();
        }

        public void setucGenerarPuntes(string value)
        {
            if (value.Equals("Si", StringComparison.OrdinalIgnoreCase))
            {
                this.ucGenerarPuentes.ValorSeleccionado = true;
            }
            else if (value.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                this.ucGenerarPuentes.ValorSeleccionado = false;
            }
            else
            {
                this.ucGenerarPuentes.ValorSeleccionado = false;
            }
        }
        public void setucGenerarTuneles(string value)
        {
            if (value.Equals("Si", StringComparison.OrdinalIgnoreCase))
            {
                this.ucGenerarTuneles.ValorSeleccionado = true;
            }
            else if (value.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                this.ucGenerarTuneles.ValorSeleccionado = false;
            }
            else
            {
                this.ucGenerarTuneles.ValorSeleccionado = false;
            }
        }
        public void setucPuenteAlturaMaxima(double value)
        {
            this.ucPuenteAlturaMaxima.uitxt = value.ToString();
        }

        public void setucCosteImplantacion(double value)
        {
            this.ucCosteImplantacion.uitxt = value.ToString();
        }

        public void setucCosteDesmonteUnitario(double value)
        {
            this.ucCosteDesmonteUnitario.uitxt = value.ToString();
        }

        public void setucCosteTerraplenUnitario(double value)
        {
            this.ucCosteTerraplenUnitario.uitxt = value.ToString();
        }

        public void setucCostePuentesViaductosUnitario(double value)
        {
            this.ucCostePuentesViaductosUnitario.uitxt = value.ToString();
        }

        public void setucCosteTunelesUnitario(double value)
        {
            this.ucCosteTunelesUnitario.uitxt = value.ToString();
        }


        #endregion
        #region "Metodos Privados"
        private void postConstructor()
        {

            #region "Traducción"

            this.grGeometria.Text = strFrmSolucion.uiGrEstudioPrevioGeometria;

            this.ucAnchoPlataforma.uiLbl = strFrmSolucion.uiEstudioPrevioAnchoPlataforma;

            this.ucDesmonteAlturaMaxima.uiLbl = strFrmSolucion.uiEstudioPrevioDesmonteAlturaMaxima;
            this.ucDesmonteTaludPC.uiLbl = strFrmSolucion.uiEstudioPrevioDesmonteTalud;

            this.ucTerraplenAlturaMaxima.uiLbl = strFrmSolucion.uiEstudioPrevioTerraplenAlturaMaxima;
            this.ucTerraplenTaludPC.uiLbl = strFrmSolucion.uiEstudioPrevioTerraplenTalud;

            this.grEstructuras.Text = strFrmSolucion.uiGrEstructuras;

            this.ucGenerarTuneles.uiLbl = strFrmSolucion.uiTunelesGenerar;
            this.ucGenerarPuentes.uiLbl = strFrmSolucion.uiPuenteGenerar;

            this.ucPuenteAlturaMaxima.uiLbl = strFrmSolucion.uiEstudioPrevioPuenteAlturaMaxima;

            this.grCostesGlobales.Text = strFrmSolucion.uiGrEstudioPrevioCostesGlobales;
            this.ucCosteImplantacion.uiLbl = strFrmSolucion.uiEstudioPrevioCosteImplantacion;

            this.ucCosteDesmonteUnitario.uiLbl = strFrmSolucion.uiEstudioPrevioCosteDesmonte;
            this.ucCosteTerraplenUnitario.uiLbl = strFrmSolucion.uiEstudioPrevioCosteTerraplen;

            this.ucCostePuentesViaductosUnitario.uiLbl = strFrmSolucion.uiEstudioPrevioCostePuentes;
            this.ucCosteTunelesUnitario.uiLbl = strFrmSolucion.uiEstudioPrevioCosteTuneles;


            #endregion


            #region "SetUpObjetos"

            this.ucAnchoPlataforma.textbox.valorDoubleNull = 11;

            this.ucDesmonteAlturaMaxima.textbox.valorDoubleNull = 50;
            this.ucDesmonteTaludPC.textbox.valorDoubleNull = 1.5;

            this.ucTerraplenAlturaMaxima.textbox.valorDoubleNull = 50;
            this.ucTerraplenTaludPC.textbox.valorDoubleNull = 1.5;

            this.ucGenerarTuneles.populate();
            this.ucGenerarTuneles.uiCombo.SelectedIndex = 1;

            this.ucGenerarPuentes.populate();
            this.ucGenerarPuentes.uiCombo.SelectedIndex = 1;

            this.ucPuenteAlturaMaxima.Enabled = false;
            this.ucPuenteAlturaMaxima.textbox.valorDoubleNull = null;


            this.ucCosteImplantacion.textbox.valorDoubleNull = 40;
            this.ucCosteDesmonteUnitario.textbox.valorDoubleNull = 4;
            this.ucCosteTerraplenUnitario.textbox.valorDoubleNull = 2;

            this.ucCostePuentesViaductosUnitario.Enabled = false;
            this.ucCostePuentesViaductosUnitario.textbox.valorDoubleNull = null;

            this.ucCosteTunelesUnitario.Enabled = false;
            this.ucCosteTunelesUnitario.textbox.valorDoubleNull = null;

            this.ucGenerarPuentes.uiCombo.SelectedIndexChanged += new EventHandler(uiComboPuentes_SelectedIndexChanged);
            this.ucGenerarTuneles.uiCombo.SelectedIndexChanged += new EventHandler(uiComboTuneles_SelectedIndexChanged);

            #endregion



        }
        #endregion
        #region "Frm Eventos"
        void uiComboPuentes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ucGenerarPuentes.valor)
            {
                this.ucPuenteAlturaMaxima.Enabled = true;
                this.ucPuenteAlturaMaxima.textbox.Enabled = true;
                this.ucPuenteAlturaMaxima.textbox.valorDoubleNull = 100;

                this.ucCostePuentesViaductosUnitario.Enabled = true;
                this.ucCostePuentesViaductosUnitario.textbox.Enabled = true;
                this.ucCostePuentesViaductosUnitario.textbox.valorDoubleNull = 900;
            }
            else
            {
                this.ucPuenteAlturaMaxima.Enabled = false;
                this.ucPuenteAlturaMaxima.textbox.Enabled = false;
                this.ucPuenteAlturaMaxima.textbox.valorDoubleNull = null;

                this.ucCostePuentesViaductosUnitario.Enabled = false;
                this.ucCostePuentesViaductosUnitario.textbox.Enabled = false;
                this.ucCostePuentesViaductosUnitario.textbox.valorDoubleNull = null;
            }
        }
        void uiComboTuneles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ucGenerarTuneles.valor)
            {
                this.ucCosteTunelesUnitario.Enabled = true;
                this.ucCosteTunelesUnitario.textbox.Enabled = true;
                this.ucCosteTunelesUnitario.textbox.valorDoubleNull = 12000000;        
            }
            else
            {
                this.ucCosteTunelesUnitario.Enabled = false;
                this.ucCosteTunelesUnitario.textbox.Enabled = false;
                this.ucCosteTunelesUnitario.textbox.valorDoubleNull = null; 
            }
        }
        #endregion
    }
}
