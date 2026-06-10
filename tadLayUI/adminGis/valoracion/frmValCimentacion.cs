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
    using tadLayLogica;
    using tadLayLan;
    
    
    public partial class frmValCimentacion : frmValoracionGisDetail
    {
    
        public frmValCimentacion()
        {
            InitializeComponent();
        }

        private void frmValCimentacion_Load(object sender, EventArgs e)
        {
            #region "SetUp & Traducccion"

            this.Text = strFrmGisCim.uiHeaderValoracion;

            grValCimEstructurasPasosInferiores.Text = strFrmGisCim.uiGrValCimentacionEstructuras;

            ucCimDirectaValoracion.uiLbl = strFrmGisCim.uiCIMDIR + " [0-10]";
            ucCimDirectaSaneosValoracion.uiLbl = strFrmGisCim.uiCIDISA + " [0-10]";
            ucCimPozosValoracion.uiLbl = strFrmGisCim.uiCIMPOZ + " [0-10]";
            ucCimProfundaValoracion.uiLbl = strFrmGisCim.uiCIMPRO + " [0-10]";

            grValExcavacionProcedimientos.Text = strFrmGisCim.uiGrValExvavacionProcedimiento;

            ucExcMediosConvencionalesValoracion.uiLbl = strFrmGisCim.uiEXMECO + " [0-10]";
            ucExcMartilloValoracion.uiLbl = strFrmGisCim.uiEXMANE + " [0-10]";
            ucExcVoladurasValoracion.uiLbl = strFrmGisCim.uiEXCVOL + " [0-10]";
            ucExcPilotadorasValoracion.uiLbl = strFrmGisCim.uiEXCPIL + " [0-10]";


            grValPresenciaAgua.Text = strFrmGisCim.uiGrValPresenciaAgua;

            ucAguaSinAfeccionValoracion.uiLbl = strFrmGisCim.uiAGUSIN + " [0-10]";
            ucAguaAgotamientoValoracion.uiLbl = strFrmGisCim.uiAGNIFR + " [0-10]";
            ucAguaSistemasEspecialesValoracion.uiLbl = strFrmGisCim.uiAGAGES + " [0-10]";

            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucSaveCancel.lnkCancel.Visible = false;

            #endregion
            #region "Comprobar Valores estan entre 0-10"

            mLstTxtPC100.Add(ucCimDirectaValoracion);
            mLstTxtPC100.Add(ucCimDirectaSaneosValoracion);
            mLstTxtPC100.Add(ucCimPozosValoracion);
            mLstTxtPC100.Add(ucCimProfundaValoracion);

            mLstTxtPC100.Add(ucExcMediosConvencionalesValoracion);
            mLstTxtPC100.Add(ucExcMartilloValoracion);
            mLstTxtPC100.Add(ucExcVoladurasValoracion);
            mLstTxtPC100.Add(ucExcPilotadorasValoracion);

            mLstTxtPC100.Add(ucAguaSinAfeccionValoracion);
            mLstTxtPC100.Add(ucAguaAgotamientoValoracion);
            mLstTxtPC100.Add(ucAguaSistemasEspecialesValoracion);

            #endregion
            #region "BindDatos"

            ucCimDirectaValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("CIMDIR").valoracion.ToString();
            ucCimDirectaSaneosValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("CIDISA").valoracion.ToString();
            ucCimPozosValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("CIMPOZ").valoracion.ToString();
            ucCimProfundaValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("CIMPRO").valoracion.ToString();

            ucExcMediosConvencionalesValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("EXMECO").valoracion.ToString();
            ucExcMartilloValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("EXMANE").valoracion.ToString();
            ucExcVoladurasValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("EXCVOL").valoracion.ToString();
            ucExcPilotadorasValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("EXCPIL").valoracion.ToString();

            ucAguaSinAfeccionValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("AGUSIN").valoracion.ToString();
            ucAguaAgotamientoValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("AGNIFR").valoracion.ToString();
            ucAguaSistemasEspecialesValoracion.textbox.Text = ds.dataset.tbCimTipos.FindByid("AGAGES").valoracion.ToString();
          
            #endregion
        }

        #region "Botones"

        //SAVE
        void lnkSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (isValidoFrm)
                {

                    string miError0to10 = string.Empty;

                    if (isValor0and10(ref miError0to10))
                    {
                        ds.dataset.tbCimTipos.FindByid("CIMDIR").valoracion = ucCimDirectaValoracion.valorDouble;
                        ds.dataset.tbCimTipos.FindByid("CIDISA").valoracion = ucCimDirectaSaneosValoracion.valorDouble;
                        ds.dataset.tbCimTipos.FindByid("CIMPOZ").valoracion = ucCimPozosValoracion.valorDouble;
                        ds.dataset.tbCimTipos.FindByid("CIMPRO").valoracion = ucCimProfundaValoracion.valorDouble;

                        ds.dataset.tbCimTipos.FindByid("EXMECO").valoracion = ucExcMediosConvencionalesValoracion.valorDouble;
                        ds.dataset.tbCimTipos.FindByid("EXMANE").valoracion = ucExcMartilloValoracion.valorDouble;
                        ds.dataset.tbCimTipos.FindByid("EXCVOL").valoracion = ucExcVoladurasValoracion.valorDouble;
                        ds.dataset.tbCimTipos.FindByid("EXCPIL").valoracion = ucExcPilotadorasValoracion.valorDouble;

                        ds.dataset.tbCimTipos.FindByid("AGUSIN").valoracion = ucAguaSinAfeccionValoracion.valorDouble;
                        ds.dataset.tbCimTipos.FindByid("AGNIFR").valoracion = ucAguaAgotamientoValoracion.valorDouble;
                        ds.dataset.tbCimTipos.FindByid("AGAGES").valoracion = ucAguaSistemasEspecialesValoracion.valorDouble;

                        ds.saveDataTable(ds.dataset.tbCimTipos, true);

                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(string.Format("Error :\n {0}", miError0to10));
                    }
                }
                else
                {
                    oTadil.data.UserInfo.showInfo(strError.eValidacion);
                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }



        }

 
        #endregion

        #region "Eventos FRM"

        protected override void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            this.Close();
        }


        #endregion


    }
}
