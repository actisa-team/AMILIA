using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using tadLayLan.Tdi;

namespace tadLayUI.adminProyecto
{
    using System.IO;
    using System.Globalization;

    using tadLayLan;
    using tadLayData;
    using tadLayLogica.zonaGis;
    using tadLayLogica.datos;
    using tadLayLogica.datos.proyecto;
    using tadLayLogica;
    using tadLayShare;
    using tadLayLogica.datos.BaseDatos;


    /// <summary>
    /// PROYECTO-DEFINIR SECCION-MACROPRECIOS
    /// </summary>
    public partial class frmSeccionZonasGenerales : frmRoot
    {


        private string mId = "APP";
        private oSingletonDsBd mDsBd = null;


        public frmSeccionZonasGenerales()
        {
            try
            {
                InitializeComponent();
                post_Constructor();

            }
            catch (oExPropertieNullValue)
            {
                //TODO meter en LAN
                oTadil.data.UserInfo.showInfo(strGeneralUser.uiNoBBDD);
            }
        }


        #region "Metodos Privados"

        private void post_Constructor()
        {

            //Traduccion
            var name = oTadil.KAppHeaderName;
            this.Text = name;

            //Grupo PBL
            grProyectoSeccion.Text = strFrmAppSecciones.uiGrSecMacro;

            ucSeccionGrupo.uiLbl = strFrmAppSecciones.uiSecGrupo;
            ucSeccionTipo.uiLbl = strFrmAppSecciones.uiSecTipo;
            ucSeccionItem.uiLbl = strFrmAppSecciones.uiSecItem;
            ucSeccionMacroPrecio.uiLbl = strFrmAppSecciones.uiSecMacroPrecios;

            grZonasGenerales.Text = strFrmAppSecciones.uiGrZonasGenerales;

            //GuardarSalir
            lnkSave.Text = strGeneral.uiGuardar;

            //Combo Calzada Grupo
            ucSeccionGrupo.populate();
            ucSeccionGrupo.uiCombo.SelectedIndexChanged += new EventHandler(uiComboGrupo_SelectedIndexChanged);

            //Combo Calzada Tipo
            ucSeccionTipo.uiCombo.Enabled = true;

            ////Combo Calzada Item ; Cargo de la Base Datos
            ucSeccionItem.uiCombo.Enabled = true;
            ucSeccionItem.uiCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ucSeccionItem.uiCombo.ValueMember = "id";
            ucSeccionItem.uiCombo.DisplayMember = "nombre";

            ////Combo MacroPrecio ; Cargo de la Base Datos
            ucSeccionMacroPrecio.uiCombo.Enabled = true;
            ucSeccionMacroPrecio.uiCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            ucSeccionMacroPrecio.uiCombo.ValueMember = "id";
            ucSeccionMacroPrecio.uiCombo.DisplayMember = "nombre";


            //Carga Datos
            dsApp.tbSeccionZonasGeneralesRow miRow = ds.dataset.tbSeccionZonasGenerales.FindByid(mId);


            if (miRow == null)
            {
                throw new oExRowNotFound("APP", "Tabla Sección - Zonas Generales");
            }


            if (!miRow.IsidRoadGrupoNull())
            {
                ucSeccionGrupo.uiCombo.SelectedValue = miRow.idRoadGrupo;
            }

            if (!miRow.IsidRoadTipoNull())
            {
                ucSeccionTipo.uiCombo.SelectedValue = miRow.idRoadTipo;
            }

            if (!miRow.IsidRoadSeccionNull())
            {
                ucSeccionItem.uiCombo.SelectedValue = miRow.idRoadSeccion;
            }

            if (!miRow.IsidRoadMacroPreciosNull())
            {
                ucSeccionMacroPrecio.uiCombo.SelectedValue = miRow.idRoadMacroPrecios;
            }

            if (!miRow.IsSeccionesVinculadasNull())
            {
                checkBox1.Checked = miRow.SeccionesVinculadas;
            }

            //Zonas Generales
            ucZonasMovimientoTierras1.populate();
            ucZonasCimentacion1.populate();
            ucZonasEstructuras1.populate();
            ucZonasTuneles1.populate();


            if (!miRow.IsidZonaMovimientoTierrasGeneralNull())
            {
                ucZonasMovimientoTierras1.uiCombo.SelectedValue = miRow.idZonaMovimientoTierrasGeneral;
            }

            if (!miRow.IsidZonaCimentacionGeneralNull())
            {
                ucZonasCimentacion1.uiCombo.SelectedValue = miRow.idZonaCimentacionGeneral;
            }

            if (!miRow.IsidZonaEstructurasGeneralNull())
            {
                ucZonasEstructuras1.uiCombo.SelectedValue = miRow.idZonaEstructurasGeneral;
            }

            if (!miRow.IsidZonaTunelesGeneralNull())
            {
                ucZonasTuneles1.uiCombo.SelectedValue = miRow.idZonaTunelesGeneral;
            }



        }

        #endregion
        #region "Eventos-COMBOX"
        //CALZADA GRUPO (CALZADA UNICA - CALZADA DOBLE 
        void uiComboGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ucSeccionGrupo.uiCombo.SelectedIndex != -1)
            {

                //Descargo el Evento
                ucSeccionTipo.uiCombo.SelectedIndexChanged -= new EventHandler(uiComboTipo_SelectedIndexChanged);

                ucSeccionTipo.uiCombo.SelectedIndex = -1;
                ucSeccionItem.uiCombo.SelectedIndex = -1;
                ucSeccionMacroPrecio.uiCombo.SelectedIndex = -1;

                ucSeccionTipo.uiCombo.Enabled = true;

                string miCalzadaGrupo = (string)ucSeccionGrupo.uiCombo.SelectedValue;

                //Cargo el Combo

                if (miCalzadaGrupo == "CALUNI")
                {
                    ucSeccionTipo.codeRoad = "CALUNI";
                    ucSeccionTipo.populate();

                }
                else if (miCalzadaGrupo == "CALDOB")
                {
                    ucSeccionTipo.codeRoad = "CALDOB";
                    ucSeccionTipo.populate();
                }
                else
                {
                    throw new oExEnumNotImplemented(miCalzadaGrupo);
                }


                //Cargo el Evento
                ucSeccionTipo.uiCombo.SelectedIndexChanged += new EventHandler(uiComboTipo_SelectedIndexChanged);

            }
        }
        //CALZADA TIPO (UNIGEN-DOBAUT-DOBSIN-DOBURB)
        void uiComboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ucSeccionTipo.uiCombo.SelectedIndex != -1)
            {

                ucSeccionItem.uiCombo.Enabled = true;
                ucSeccionMacroPrecio.uiCombo.Enabled = true;

                string miCalzadaTipo = ucSeccionTipo.valor;

                mDsBd = oSingletonDsBd.getInstance;

                if (miCalzadaTipo == "UNIGEN")
                {
                    ucSeccionItem.uiCombo.DataSource = mDsBd.dataset.tbRoadUniGen.ToList();
                }
                else if (miCalzadaTipo == "DOBAUT")
                {
                    ucSeccionItem.uiCombo.DataSource = mDsBd.dataset.tbRoadDobleAutovia.ToList();
                }
                else if (miCalzadaTipo == "DOBSIN")
                {
                    ucSeccionItem.uiCombo.DataSource = mDsBd.dataset.tbRoadDobleSinMediana.ToList();
                }
                else
                {
                    throw new oExEnumNotImplemented(miCalzadaTipo);
                }


                ucSeccionMacroPrecio.uiCombo.DataSource = mDsBd.dataset.tbMacroPrecios.AsEnumerable().Where(row => row.idTbRoadTipo == miCalzadaTipo).ToList();

                ucSeccionItem.uiCombo.SelectedIndex = -1;
                ucSeccionMacroPrecio.uiCombo.SelectedIndex = -1;


            }
        }
        #endregion
        #region "Botones"
        //GUARDAR
        private void lnkSave_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (isValidoFrm)
                {
                    string miGrupo = null;
                    string miTipo = null;
                    Guid? miSeccion = null;
                    Guid? miMacroPrecio = null;


                    miGrupo = (string)ucSeccionGrupo.uiCombo.SelectedValue;
                    miTipo = (string)ucSeccionTipo.uiCombo.SelectedValue;

                    miSeccion = (Guid)ucSeccionItem.uiCombo.SelectedValue;


                    miMacroPrecio = (Guid)ucSeccionMacroPrecio.uiCombo.SelectedValue;



                    Guid miIdZonaMovimientoTierrasGeneral = (Guid)ucZonasMovimientoTierras1.uiCombo.SelectedValue;
                    Guid miIdZonaCimentacionGeneral = (Guid)ucZonasCimentacion1.uiCombo.SelectedValue;
                    Guid miIdZonaEstructurasGeneral = (Guid)ucZonasEstructuras1.uiCombo.SelectedValue;
                    Guid miIdZonaTunelesGeneral = (Guid)ucZonasTuneles1.uiCombo.SelectedValue;

                    oZonaGeoMovimientoTierras miZonaMovTierra = new oZonaGeoMovimientoTierras(miIdZonaMovimientoTierrasGeneral);
                    oZonaGeoCimentacion miZonaCimentacion = new oZonaGeoCimentacion(miIdZonaCimentacionGeneral);
                    oZonaGeoEstructuras miZonaEstucturas = new oZonaGeoEstructuras(miIdZonaEstructurasGeneral);
                    oZonaGeoTuneles miZonaTunel = new oZonaGeoTuneles(miIdZonaTunelesGeneral);

                    if (!(miZonaMovTierra.isZonaNoPaso || miZonaCimentacion.isZonaNoPaso || miZonaEstucturas.isZonaNoPaso || miZonaTunel.isZonaNoPaso))
                    {

                        dsApp.tbSeccionZonasGeneralesRow miRow = ds.dataset.tbSeccionZonasGenerales.FindByid(mId);

                        
                            miRow.SeccionesVinculadas = checkBox1.Checked;
                            miRow.SetidRoadGrupoNull();
                            miRow.SetidRoadTipoNull();
                            miRow.SetidRoadSeccionNull();
                            miRow.SetidRoadMacroPreciosNull();
                        

                            if (miGrupo != null) miRow.idRoadGrupo = miGrupo;
                            else miRow.SetidRoadGrupoNull();

                            if (miTipo != null) miRow.idRoadTipo = miTipo;
                            else miRow.SetidRoadTipoNull();

                            if (miSeccion != null) miRow.idRoadSeccion = miSeccion.Value;
                            else miRow.SetidRoadSeccionNull();

                            if (miMacroPrecio != null) miRow.idRoadMacroPrecios = miMacroPrecio.Value;
                            else miRow.SetidRoadMacroPreciosNull();
                        

                        miRow.idZonaMovimientoTierrasGeneral = miIdZonaMovimientoTierrasGeneral;
                        miRow.idZonaCimentacionGeneral = miIdZonaCimentacionGeneral;
                        miRow.idZonaEstructurasGeneral = miIdZonaEstructurasGeneral;
                        miRow.idZonaTunelesGeneral = miIdZonaTunelesGeneral;

                        ds.save();
                    }
                    else
                    {
                        oTadil.data.UserInfo.showInfo(strError.eZonaGeneralNP);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
