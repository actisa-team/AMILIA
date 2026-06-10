using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI.userControl
{

    using Autodesk.AutoCAD.Geometry;
    using Autodesk.AutoCAD.EditorInput;

    using engCadNet;

    using engNet.eventos;
    using tadLayLogica;
    using tadLayLan;
    using tadLayLogica.datos.proyecto;
    using engNet.Extension.Double;

    using tadLayData;
    using tadLayShare.puntos;
    using tadLayLogica.zonaGis;
    using Autodesk.AutoCAD.ApplicationServices;
    using tadLayLogica.logica.EjeBasicoNew;

    public partial class ucPtoIniFin : UserControl
    {

        private BindingSource mBindMaster;
        private int mIdPto;

        public event EventHandler<oEventArgs<bool>> evHideFrm;

        public ucPtoIniFin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// SetUp Punto 0-->PuntoSalida ; 1-->PuntoLlegada
        /// </summary>
        /// <param name="iIdPto"></param>
        public void populate(int iIdPto)
        {

            mIdPto = iIdPto;

            //Bind
            mBindMaster = new BindingSource();

            mBindMaster.DataMember = oSingletonDsApp.getInstance.dataset.tbPtoIniFin.TableName;
            mBindMaster.DataSource = oSingletonDsApp.getInstance.dataset.tbPtoIniFin;

            string miQuery = "id = '{0}' ";

            mBindMaster.Filter = string.Format(miQuery, mIdPto);

            ucX.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.XColumn.ColumnName, true);
            ucY.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.YColumn.ColumnName, true);
            ucZ.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.ZColumn.ColumnName, true);

            chkIsAzimut.DataBindings.Add("checked", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.isAzimutColumn.ColumnName, true);
            ucAzimut.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.azimutGradosColumn.ColumnName, true);

            chkIsLongitud.DataBindings.Add("checked", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.isLongitudColumn.ColumnName, true);
            ucLongitud.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.longitudMetrosColumn.ColumnName, true);
            chkIsLongitudRecta.DataBindings.Add("checked", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.isLongitudMinimaRectaColumn.ColumnName, true);

            chkIsPendiente.DataBindings.Add("checked", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.isPendienteColumn.ColumnName, true);
            ucPendiente.textbox.DataBindings.Add("valorDoubleNull", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.pendientePCColumn.ColumnName, true);


            grAzimut.DataBindings.Add("Enabled", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.isAzimutColumn.ColumnName, true);
            grLon.DataBindings.Add("Enabled", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.isLongitudColumn.ColumnName, true);
            grPendiente.DataBindings.Add("Enabled", mBindMaster, oSingletonDsApp.getInstance.dataset.tbPtoIniFin.isPendienteColumn.ColumnName, true);

        }


        private bool isOpcionesOk()
        {

            bool miIsOk = true;

            if (chkIsLongitud.Checked && !chkIsAzimut.Checked)
            {

                oTadil.data.UserInfo.showInfo("Opción No Valida ; \nValor Azimut Nulo \nValor Longitud No Nulo.");

                miIsOk = false;
            }

            if (!chkIsAzimut.Checked && !chkIsLongitud.Checked && chkIsPendiente.Checked)
            {
                oTadil.data.UserInfo.showInfo("Opción No Valida ; \nValor Azimut Nulo \nValor Longitud Nulo. \nValor Pendiente No Nulo");

                miIsOk = false;
            }



            return miIsOk;
        }




        private oP3d getPto()
        {

            double miX = this.ucX.valorDouble;
            double miY = this.ucY.valorDouble;
            double miZ = this.ucZ.valorDouble;

            return new oP3d(miX, miY, miZ);
        }

        private void setPto(oP3d iPto)
        {
            this.ucX.textbox.valorDoubleNull = iPto.X;
            this.ucY.textbox.valorDoubleNull = iPto.Y;
            this.ucZ.textbox.valorDoubleNull = iPto.Z;
        }



        #region "Botones"

        //SELECT POINT SUPERFICIE
        void btnSelectPoint_Click(object sender, EventArgs e)
        {
            try
            {
                Point3d miPtoUser;

                if (this.evHideFrm != null)
                {
                    evHideFrm(this, new oEventArgs<bool>(true));
                }

                frmAppManager.getInstance.Hide();

                PromptPointResult ppr = oCadManager.thisEditor.GetPoint(strUserCad.uiPointSelect);

                if (ppr.Status == PromptStatus.OK)
                {
                    miPtoUser = ppr.Value;
                }
                else
                {
                    oTadil.data.UserInfo.procesoCancelado();
                    return;
                }
                //oSingletonPuntosTerreno.getInstance.Cargar();
                double[] punto = new double[2];
                punto[0] = miPtoUser.X;
                punto[1] = miPtoUser.Y;
                double? miTerrenoZ = null;
                if (oSingletonTerreno.getInstance.tipo == 1)
                {
                    miTerrenoZ = oSingletonTerreno.getInstance.getZFromXY(miPtoUser.X, miPtoUser.Y);
                }
                else if (oSingletonTerreno.getInstance.tipo == 2)
                {
                    if (!oSingletonPuntosTerreno.getInstance.IsPointInsideClosedPolyline2D(new Point2d(miPtoUser.X, miPtoUser.Y)))
                    {
                        oTadil.data.UserInfo.showInfo(strUserCad.uiPointOutCartografia);
                        return;
                    }
                    miTerrenoZ = oSingletonPuntosTerreno.getInstance.GetZ(miPtoUser.X, miPtoUser.Y);
                }
                else if (oSingletonTerreno.getInstance.tipo == 3)
                {
                    miTerrenoZ = oSingletonPuntosTerrenoASC.getInstance.GetZ(miPtoUser.X, miPtoUser.Y);
                }
                else
                {
                    miTerrenoZ = null;
                }
                //double a=oSingletonPuntosTerreno.getInstance.MDT_Abanico_Punto(punto);


                if (miTerrenoZ == null || double.IsNaN((double)miTerrenoZ))
                {
                    oTadil.data.UserInfo.showInfo(strUserCad.uiPointOutCartografia);

                    return;
                }

                if (oSingletonZonaNoPaso.getInstance.isPtoOnZonaNoPaso(miPtoUser.X, miPtoUser.Y))
                {
                    oTadil.data.UserInfo.showInfo(strUserCad.uiPointZNP);
                    return;

                }

                if (oSingletonTerreno.getInstance.isPtoCercaBorde(miPtoUser.X, miPtoUser.Y))
                {
                    oTadil.data.UserInfo.showInfo(strUserCad.uiPointCercaBorde);
                    return;
                }

                this.setPto(new oP3d(miPtoUser.X.roundOff(3), miPtoUser.Y.roundOff(3), miTerrenoZ.Value.roundOff(3)));

                this.lnkSave_Click(null, null);
            }

            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
            finally
            {
                if (this.evHideFrm != null)
                {
                    evHideFrm(this, new oEventArgs<bool>(false));
                }
            }

        }

        //EDIT POINT
        private void btnEditPoint_Click(object sender, EventArgs e)
        {
            try
            {
                frmPointEdit miFrmEditPoint = new frmPointEdit();
                miFrmEditPoint.populate(this.getPto());


                if (miFrmEditPoint.ShowDialog() == DialogResult.OK)
                {
                    this.setPto(miFrmEditPoint.getPto());

                    miFrmEditPoint.Close();

                    this.lnkSave_Click(null, null);

                }

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }
        }



        //SAVE
        void lnkSave_Click(object sender, EventArgs e)
        {

            try
            {

                if (oValidar.isValidoGrupoByFrm(this) && isOpcionesOk())
                {
                    mBindMaster.EndEdit();

                    oDalTbPtoIniFin.saveTable();

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

        //CANCEL
        void lnkCancel_Click(object sender, EventArgs e)
        {

            try
            {
                mBindMaster.CancelEdit();

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showError(ex);
            }

        }

        #endregion
        #region "Eventos FRM"

        private void ucPtoIniFin_Load(object sender, EventArgs e)
        {
            #region "Traduccion"


            if (mIdPto == 0)
            {
                grPto.Text = strFrmPtoIniFin.uiPuntoOrigen;
                ucAzimut.uiLbl = strFrmPtoIniFin.uiAzimutSalidaValor;
                ucLongitud.uiLbl = strFrmPtoIniFin.uiLongitudSalidaValor;
                ucPendiente.uiLbl = strFrmPtoIniFin.uiPendienteSalidaValor;

            }
            else if (mIdPto == 1)
            {
                grPto.Text = strFrmPtoIniFin.uiPuntoDestino;
                ucAzimut.uiLbl = strFrmPtoIniFin.uiAzimutLlegadaValor;
                ucLongitud.uiLbl = strFrmPtoIniFin.uiLongitudLlegadaValor;
                ucPendiente.uiLbl = strFrmPtoIniFin.uiPendienteLLegadaValor;
            }


            ucX.uiLbl = strFrmPtoIniFin.uiX;
            ucY.uiLbl = strFrmPtoIniFin.uiY;
            ucZ.uiLbl = strFrmPtoIniFin.uiZ;


            chkIsAzimut.Text = strFrmPtoIniFin.uiAzimutChecked;
            chkIsLongitud.Text = strFrmPtoIniFin.uiLongitudChecked;
            chkIsLongitudRecta.Text = strFrmPtoIniFin.uiLongitudRectaConsiderar;
            chkIsPendiente.Text = strFrmPtoIniFin.uiPendienteChecked;

            btnSelectPoint.Text = strFrmPtoIniFin.uiBtnSelectPunto;
            btnEditPoint.Text = strFrmPtoIniFin.uiBtnWritePunto;

            #endregion

            #region "SetUp Object"

            ucX.textbox.Enabled = false;
            ucY.textbox.Enabled = false;
            ucZ.textbox.Enabled = false;

            grAzimut.Enabled = false;
            grLon.Enabled = false;
            grPendiente.Enabled = false;

            ucToolDetail1.lnkSalir.Visible = false;
            ucToolDetail1.lnkSave.Click += new EventHandler(lnkSave_Click);
            ucToolDetail1.lnkCancel.Visible = false;

            #endregion
        }

        private void chkIsAzimut_CheckedChanged(object sender, EventArgs e)
        {

            grAzimut.Enabled = ((CheckBox)sender).Checked;

            if (!grAzimut.Enabled)
            {
                ucAzimut.uitxt = string.Empty;
            }

        }

        private void chkIsLongitud_CheckedChanged(object sender, EventArgs e)
        {
            grLon.Enabled = ((CheckBox)sender).Checked;

            if (!grLon.Enabled)
            {
                ucLongitud.uitxt = string.Empty;
                chkIsLongitudRecta.Checked = false;
            }
        }

        private void chkIsPendiente_CheckedChanged(object sender, EventArgs e)
        {
            grPendiente.Enabled = ((CheckBox)sender).Checked;

            if (!grPendiente.Enabled)
            {
                ucPendiente.uitxt = string.Empty;
            }
        }



        #endregion
        public void Dibujar_Ini_Fin()
        {
            using (DocumentLock miDockLock = oCadManager.thisEditor.Document.LockDocument())
            {
                engCadNet.oLayer.createOrDeleteItems("_Tadil_Salida_Llegada", 7);
                Point3d p1 = new Point3d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y, 0);
                Point3d p2 = new Point3d(oSingletonProyecto.getInstance.ptoLlegada.X, oSingletonProyecto.getInstance.ptoLlegada.Y, 0);
                Point3dCollection point2Ds = new Point3dCollection();
                point2Ds.Add(p1);
                point2Ds.Add(p2);
                engCadNet.oLw.addLw2d(point2Ds, false, "_Tadil_Salida_Llegada", oTadil.data.Layer.visibilidadEje.color);
            }
            //engCadNet.oLine.addLine2d(oSingletonProyecto.getInstance.ptoSalida.X, oSingletonProyecto.getInstance.ptoSalida.Y, oSingletonProyecto.getInstance.ptoLlegada.X, oSingletonProyecto.getInstance.ptoLlegada.Y, "_Tadil_VisibilidadEje");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                frmAppManager.getInstance.Hide();
                Dibujar_Ini_Fin();

            }
            catch (Exception ex)
            {
                oTadil.data.UserInfo.showInfo(ex.Message);
            }
            finally
            {
                frmAppManager.getInstance.Show();
            }
        }
    }
}
