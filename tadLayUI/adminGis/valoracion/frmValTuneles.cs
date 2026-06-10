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
    using tadLayLogica;
    using tadLayLogica.datos;
    using tadLayLan;
    
    
    public partial class frmValTuneles : frmValoracionGisDetail
    {
        public frmValTuneles()
        {
            InitializeComponent();
        }


        private void frmValTuneles_Load(object sender, EventArgs e)
        {
            #region "SetUp & Traducccion"

            this.Text = strFrmGisTun.uiHeaderValoracion;

            grValExcavacionMetodos.Text = strFrmGisTun.uiGrValoracionExcavacion ;

            ucExcMetodosTradicionalesValoracion.uiLbl = strFrmGisTun.uiMAQCON + " [0-10]";
            ucExcRozadorasValoracion.uiLbl = strFrmGisTun.uiROZADO + " [0-10]";
            ucExcVoladurasValoracion.uiLbl = strFrmGisTun.uiPERVOL+ " [0-10]";
            ucEscudoLodosValoracion.uiLbl = strFrmGisTun.uiESPRLO + " [0-10]";
            ucEscudoTierrasValoracion.uiLbl = strFrmGisTun.uiESPRTI + " [0-10]";
            ucEscudoPresurizadoValoracion.uiLbl = strFrmGisTun.uiESCPRE + " [0-10]";
            ucEscudoAbiertoRocasValoracion.uiLbl = strFrmGisTun.uiESABRO + " [0-10]";
            ucEscudoDobleValoracion.uiLbl = strFrmGisTun.uiESCDOB + " [0-10]";
            ucEscudoOtrosValoracion.uiLbl = strFrmGisTun.uiESCOTR + " [0-10]";

            grValTratamientos.Text = strFrmGisTun.uiGrValoracionTratamientos;

            ucTratamientoSinValoracion.uiLbl = strFrmGisTun.uiTRASIN +   " [0-10]";
            ucTratamientoAguaValoracion.uiLbl = strFrmGisTun.uiTRAAGU + " [0-10]";
            ucTratamientoFrenteValoracion.uiLbl = strFrmGisTun.uiTRAFRE + " [0-10]";
            ucTratamientoSoleraValoracion.uiLbl = strFrmGisTun.uiTRASOL + " [0-10]";
            ucTratamientoOtrosValoracion.uiLbl = strFrmGisTun.uiTRAOTR + " [0-10]";

            ucSaveCancel.lnkCancel.Visible = false;

            ucSaveCancel.lnkSave.Click += new EventHandler(lnkSave_Click);
   

            #endregion
            #region "Comprobar Valores estan entre 0-10"

            mLstTxtPC100.Add(ucExcMetodosTradicionalesValoracion);
            mLstTxtPC100.Add(ucExcRozadorasValoracion);
            mLstTxtPC100.Add(ucExcVoladurasValoracion);

            mLstTxtPC100.Add(ucEscudoLodosValoracion);
            mLstTxtPC100.Add(ucEscudoTierrasValoracion);
            mLstTxtPC100.Add(ucEscudoPresurizadoValoracion);
            mLstTxtPC100.Add(ucEscudoDobleValoracion);
            mLstTxtPC100.Add(ucEscudoAbiertoRocasValoracion);
            mLstTxtPC100.Add(ucEscudoOtrosValoracion);

            mLstTxtPC100.Add(ucTratamientoSinValoracion);
            mLstTxtPC100.Add(ucTratamientoAguaValoracion);
            mLstTxtPC100.Add(ucTratamientoFrenteValoracion);
            mLstTxtPC100.Add(ucTratamientoSoleraValoracion);
            mLstTxtPC100.Add(ucTratamientoOtrosValoracion);

            #endregion
            #region "BindDatos"

            //Metodos
            ucExcMetodosTradicionalesValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("MAQCON").valoracion.ToString();
            ucExcRozadorasValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("ROZADO").valoracion.ToString();
            ucExcVoladurasValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("PERVOL").valoracion.ToString();

            //Escudos
            ucEscudoLodosValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("ESPRLO").valoracion.ToString();
            ucEscudoTierrasValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("ESPRTI").valoracion.ToString();
            ucEscudoPresurizadoValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("ESCPRE").valoracion.ToString();
            ucEscudoAbiertoRocasValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("ESABRO").valoracion.ToString();
            ucEscudoDobleValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("ESCDOB").valoracion.ToString();
            ucEscudoOtrosValoracion.textbox.Text = ds.dataset.tbTunExcMet.FindByid("ESCOTR").valoracion.ToString();

            //Tratamientos    
            ucTratamientoSinValoracion.textbox.Text = ds.dataset.tbTunTipoTratamieto.FindByid("TRASIN").valoracion.ToString();
            ucTratamientoAguaValoracion.textbox.Text = ds.dataset.tbTunTipoTratamieto.FindByid("TRAAGU").valoracion.ToString();
            ucTratamientoFrenteValoracion.textbox.Text = ds.dataset.tbTunTipoTratamieto.FindByid("TRAFRE").valoracion.ToString();
            ucTratamientoSoleraValoracion.textbox.Text = ds.dataset.tbTunTipoTratamieto.FindByid("TRASOL").valoracion.ToString();
            ucTratamientoOtrosValoracion.textbox.Text = ds.dataset.tbTunTipoTratamieto.FindByid("TRAOTR").valoracion.ToString();
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
                        ds.dataset.tbTunExcMet.FindByid("MAQCON").valoracion = ucExcMetodosTradicionalesValoracion.valorDouble;
                        ds.dataset.tbTunExcMet.FindByid("ROZADO").valoracion = ucExcRozadorasValoracion.valorDouble;
                        ds.dataset.tbTunExcMet.FindByid("PERVOL").valoracion = ucExcVoladurasValoracion.valorDouble;
                        ds.dataset.tbTunExcMet.FindByid("ESPRLO").valoracion = ucEscudoLodosValoracion.valorDouble;
                        ds.dataset.tbTunExcMet.FindByid("ESPRTI").valoracion = ucEscudoTierrasValoracion.valorDouble;
                        ds.dataset.tbTunExcMet.FindByid("ESCPRE").valoracion = ucEscudoPresurizadoValoracion.valorDouble;
                        ds.dataset.tbTunExcMet.FindByid("ESABRO").valoracion = ucEscudoAbiertoRocasValoracion.valorDouble;
                        ds.dataset.tbTunExcMet.FindByid("ESCDOB").valoracion = ucEscudoDobleValoracion.valorDouble;
                        ds.dataset.tbTunExcMet.FindByid("ESCOTR").valoracion = ucEscudoOtrosValoracion.valorDouble;


                        ds.dataset.tbTunTipoTratamieto.FindByid("TRASIN").valoracion = ucTratamientoSinValoracion.valorDouble;
                        ds.dataset.tbTunTipoTratamieto.FindByid("TRAAGU").valoracion =  ucTratamientoAguaValoracion.valorDouble;
                        ds.dataset.tbTunTipoTratamieto.FindByid("TRAFRE").valoracion = ucTratamientoFrenteValoracion.valorDouble;
                        ds.dataset.tbTunTipoTratamieto.FindByid("TRASOL").valoracion= ucTratamientoSoleraValoracion.valorDouble;
                        ds.dataset.tbTunTipoTratamieto.FindByid("TRAOTR").valoracion = ucTratamientoOtrosValoracion.valorDouble;

                        ds.saveDataTable(ds.dataset.tbTunExcMet, true);
                        ds.saveDataTable(ds.dataset.tbTunTipoTratamieto, false);

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




        protected override void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose(true);
            this.Close();
        }

    }
}
